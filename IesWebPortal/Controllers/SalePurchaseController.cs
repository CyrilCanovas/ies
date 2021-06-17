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


namespace IesWebPortal.Controllers
{
    public class SalePurchaseController : Controller
    {
        private readonly IDataService _dataService;
        private readonly IMLLabelConfigs _mlLabelConfigs;
        public SalePurchaseController(IDataService dataService, IMLLabelConfigs mlLabelConfigs)
        {
            _dataService = dataService;
            _mlLabelConfigs = mlLabelConfigs;
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
        public FileContentResult Print()
        {
            //Response.Headers.Add("content-disposition", "inline;filename=" + model.DocumentNo + ".pdf");
            var result = System.IO.File.ReadAllBytes("test.pdf");
            var contentresult = new FileContentResult(result, MediaTypeHeaderValue.Parse("application/pdf"));
            //Tools.SetCookie(Reponse,"ReportLanguage", reportlanguage);
            //SetCookie(Constants.COOKIE_LASTREPORTNAME, reportname);
            //SetCookie(Constants.COOKIE_ONLYADDRESS, onlyaddress.ToString());
            return contentresult;

        }
    }
}
