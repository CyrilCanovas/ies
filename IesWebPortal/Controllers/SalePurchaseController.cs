using IesWebPortal.Classes;
using IesWebPortal.Model;
using IesWebPortal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using IesWebPortal.Models;
using System.Globalization;
using ReportTools.Core;
using Microsoft.AspNetCore.Hosting;

namespace IesWebPortal.Controllers
{
    public class SalePurchaseController : Controller
    {
        private readonly IDataService _dataService;
        private readonly IMLLabelConfigs _mlLabelConfigs;
        private readonly string _picturePath;
        private readonly string _reportPath;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SalePurchaseController(IDataService dataService, IMLLabelConfigs mlLabelConfigs, IIesWebPortalSettings iesWebPortalSettings,IWebHostEnvironment environment)
        {
            _dataService = dataService;
            _mlLabelConfigs = mlLabelConfigs;
            _picturePath = iesWebPortalSettings.PicturePath;
            _reportPath = IesWebPortal.Classes.IesWebPortalConstants.REPORT_PATH;
            _webHostEnvironment = environment;
        }
        private static int FIRST_DOCUMENT_TYPE = IesWebPortalConstants.CST_DOCUMENTS.First().Key;
        public IActionResult Index(int? documentType, string sortColumn, string sortDirection)
        {
            if (documentType == null)
            {
                var lastdocumenttypestr = Request.Cookies[IesWebPortalConstants.COOKIE_LASTDOCUMENTTYPE];
                int lastdocumenttype;
                if (!int.TryParse(lastdocumenttypestr, out lastdocumenttype))
                    lastdocumenttype = FIRST_DOCUMENT_TYPE;

                return RedirectToAction("Index", ControllerContext.GetCurrentControlerName(), new { documenttype = lastdocumenttype, sortcolumn = sortColumn, sortdirection = sortDirection });
            }
            if (Request.Method == "POST")
            {
                return RedirectToAction("Index", ControllerContext.GetCurrentControlerName(), new { documenttype = documentType, sortcolumn = sortColumn, sortdirection = sortDirection });
            }

            var q = _dataService.GetDocHeaders(documentType ?? FIRST_DOCUMENT_TYPE);

            ViewBag.SortDirection = string.IsNullOrEmpty(sortDirection) ? SortDirection.Ascending : Enum.Parse(typeof(SortDirection), sortDirection);
            ViewBag.SortColumn = string.IsNullOrEmpty(sortColumn) ? "DocumentNo" : sortColumn;

            switch ((SortDirection)ViewBag.SortDirection)
            {
                case SortDirection.Descending:
                    q = q.OrderByDescending(x => Tools.GetPropertyValue(x, ViewBag.SortColumn)).ToArray();
                    break;
                default:
                    q = q.OrderBy(x => Tools.GetPropertyValue(x, ViewBag.SortColumn)).ToArray();
                    break;
            }

            ViewData["DocumentTypeSelectListItems"] = (from i in IesWebPortalConstants.CST_DOCUMENTS
                                                       select new SelectListItem() { Text = i.Value, Value = i.Key.ToString(), Selected = (i.Key == (documentType ??  FIRST_DOCUMENT_TYPE)) }).ToArray();
            Response.SetCookie(
                IesWebPortalConstants.COOKIE_LASTSORTSALEPURCHASE,
                    new KeyValuePair<string, string>("SortColumn", ViewBag.SortColumn),
                    new KeyValuePair<string, string>("SortDirection", ((int)ViewBag.SortDirection).ToString())
                );

            Response.SetCookie(IesWebPortalConstants.COOKIE_LASTDOCUMENTTYPE, documentType.ToString());

            return View(q);
        }

        
        public ActionResult Details(int documenttype, string documentno)
        {
            var lastreportname = Request.Cookies[IesWebPortalConstants.COOKIE_LASTREPORTNAME];
            ViewData["Reports"] = _mlLabelConfigs;
            var reportlastlanguage = Request.Cookies["ReportLanguage"];

            var a = (from i in _mlLabelConfigs
                     select new SelectListItem() { Text = i.Value.Description, Value = i.Key, Selected = (i.Key == lastreportname) }).ToArray();

            if (a.Where(x => x.Selected).Count() == 0) a.First().Selected = true;

            var selectlistitem = a.Where(x => x.Selected).First();
            ViewData["ReportName"] = selectlistitem.Value;
            ViewData["ReportListItems"] = a;

            var reportLanguages = (new Dictionary<string, string>() { { string.Empty, "(Défaut)" }, { "SA", "Arabie saoudite" } }).Select(kv => new SelectListItem() { Text = kv.Value, Value = kv.Key, Selected = (kv.Key == reportlastlanguage) }).ToArray();

            if (reportLanguages.Where(x => x.Selected).Count() == 0) reportLanguages.First().Selected = true;

            ViewData["ReportLanguage"] = reportLanguages.Where(x => x.Selected).First().Value;
            ViewData["ReportLanguages"] = reportLanguages;

            var cookieonlyaddress = Request.Cookies[IesWebPortalConstants.COOKIE_ONLYADDRESS];
            bool onlyaddress = false;
            if (cookieonlyaddress != null)
            {
                bool.TryParse(cookieonlyaddress, out onlyaddress);
            }
            ViewData["OnlyAddress"] = onlyaddress;
            
            var q = _dataService.GetDocHeader( documenttype, documentno);
            if (q.Lines != null)
            {
                Array.ForEach(q.Lines, x => x.LabelCount = 1);
            }
            return View(q);
        }
        [HttpPost]
        public FileContentResult Print(PrintModel printModel)
        {

            var reportLanguage = printModel.ReportLanguage ?? string.Empty;

            var docHeader = _dataService.GetDocHeader(printModel.DocumentType, printModel.DocumentNo);
            var lines = printModel.Lines ?? new PrintModelDetail[] { };
            List<IMLSalePurchaseLine> tagsToPrint = new List<IMLSalePurchaseLine>();
            var itemNos = docHeader.Lines.Select(x => x.ItemNo).Distinct().ToArray();
            var itemVendors = _dataService.ItemToVendor(itemNos);
            for (int i = 0; i < docHeader.Lines.Count(); i++)
            {

                var line = docHeader.Lines[i];
                var _line = lines.Where(x => x.DlNo == line.DlNo).SingleOrDefault();
                
                if (_line == null)
                {
                    docHeader.Lines[i] = null;
                    continue;
                }

                var labelCount = 0;
                if (!int.TryParse(_line.LabelCount, out labelCount))
                {
                    docHeader.Lines[i] = null;
                    continue;
                }


                double tare = 0.0;
                bool bTare = double.TryParse((_line.Tare ?? string.Empty).Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out tare);

                double qty = 0.0;
                bool bQty = double.TryParse((_line.Qty ?? string.Empty).Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out qty);

                DateTime manufacturedDate;
                bool bManufacturedDate = DateTime.TryParseExact(_line.ManufacturedDate, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out manufacturedDate);

                DateTime bestBeforeDate;
                bool bBestBeforeDate = DateTime.TryParseExact(_line.BestBeforeDate, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out bestBeforeDate);

                for (int a = 0; a < labelCount; a++)
                {
                    tagsToPrint.Add(
                        new MLSalePurchaseLine()
                        {
                            DocumentDate = docHeader.DocumentDate,
                            DlNo = line.DlNo,
                            Header = line.Header,
                            LineNo = line.LineNo,
                            ItemNo = line.ItemNo,
                            Description = line.Description,
                            GrossWeight = line.GrossWeight,
                            Tare = tare,
                            ExtItemNo = line.ExtItemNo,
                            DocumentNo = line.DocumentNo,
                            Qty = qty,
                            ManufacturedDate = bManufacturedDate ? manufacturedDate : new DateTime?(),
                            SerialNo = _line.SerialNo,
                            BestBeforeDate = bBestBeforeDate ? bestBeforeDate : new DateTime?(),
                            OnlyAddress = printModel.OnlyAddress ?? false,
                            VendorDisplayName = itemVendors.ContainsKey(line.ItemNo) ? ProviderRewriter.GetVendorDisplayName(itemVendors[line.ItemNo]) : ProviderRewriter.GetVendorDisplayName(string.Empty)

                        }
                        );
                }
            }

            int fillerCount = printModel.ShiftTagCount ?? 0;

            if (fillerCount > 0)
            {
                var filler = from i in Enumerable.Range(1, fillerCount)
                             select new MLSalePurchaseLine()
                             {
                                 OnlyAddress = printModel.OnlyAddress ?? false
                             };

                docHeader.Lines = filler.Concat(tagsToPrint).ToArray();

            }
            else
            {
                docHeader.Lines = tagsToPrint.ToArray();
            }

            _dataService.FillLabels(docHeader, reportLanguage,
                x =>
                {
                    return string.IsNullOrEmpty(_picturePath) ? 
                        Path.Combine(_webHostEnvironment.WebRootPath, IesWebPortalConstants.PICTURE_PATH , x) : Path.Combine(_picturePath, x);
                }
                );

            if (!string.IsNullOrEmpty(reportLanguage))
            {
                var all = docHeader.Lines.Select(x => x.Item).Distinct().ToList();
                all.ForEach(ii => ii.DangerousFrench = string.Empty);
            }

            var reportSettings = (from i in _mlLabelConfigs
                                  where i.Key == printModel.ReportName
                                  select i.Value.Settings).SingleOrDefault();
            
            var result = GetRawResult(
                Path.Combine(_webHostEnvironment.WebRootPath, _reportPath , printModel.ReportName) ,
                reportSettings,
                docHeader.Lines,
                reportLanguage
            );

            
            Response.Headers.Add("content-disposition", "inline;filename=" + printModel.DocumentNo + ".pdf");
            
            var contentresult = new FileContentResult(result, MediaTypeHeaderValue.Parse("application/pdf"));
            //Tools.SetCookie(Reponse,"ReportLanguage", reportlanguage);
            //SetCookie(Constants.COOKIE_LASTREPORTNAME, reportname);
            //SetCookie(Constants.COOKIE_ONLYADDRESS, onlyaddress.ToString());
            return contentresult;

        }

        protected byte[] GetRawResult(
            string reportFilePath,
            string reportSettings,
            IEnumerable<object> mainDataset,
            string reportLanguage
        )
        {
            
            ReportHelper.PageInfo pageSettings = null;
            if (!string.IsNullOrEmpty(reportSettings))
                pageSettings = ReportHelper.ParsePageInfoString(reportSettings);

            return ReportHelper.RenderReport(
                    reportFilePath,
                    new string[] { "MainDataSet" },
                    new object[] { mainDataset },
                    //      subreportprocessing,
                    null,
                    pageSettings == null ? 0 : pageSettings.PageWidth,
                    pageSettings == null ? 0 : pageSettings.PageHeight,
                    pageSettings == null ? 0 : pageSettings.MarginTop,
                    pageSettings == null ? 0 : pageSettings.MarginLeft,
                    pageSettings == null ? 0 : pageSettings.MarginRight,
                    pageSettings == null ? 0 : pageSettings.MarginBottom,
                    reportLanguage == "SA" ? new Dictionary<string, string>() { { "CustomDirection", "RTL" } } : new Dictionary<string, string>() { { "CustomDirection", "LTR" } }
                );
        }
    }
}
