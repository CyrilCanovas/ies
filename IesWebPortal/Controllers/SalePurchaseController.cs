using IesWebPortal.Classes;
using IesWebPortal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace IesWebPortal.Controllers
{
    public class SalePurchaseController : Controller
    {
        private readonly IDataService _dataService;
        public SalePurchaseController(IDataService dataService)
        {
            _dataService = dataService;
        }

        private static int FIRST_DOCUMENT_TYPE = IesWebPortalConstants.CST_DOCUMENTS.First().Key;
        public IActionResult Index(int? documentType, string sortColumn, string sortDirection)
        {
            //if (documenttype == null)
            //{
            //    var cookie = Request.Cookies[Constants.COOKIE_LASTDOCUMENTTYPE];
            //    var lastdocumenttypestr = cookie == null ? string.Empty : cookie.Value;
            //    int lastdocumenttype;
            //    if (!int.TryParse(lastdocumenttypestr, out lastdocumenttype))
            //        lastdocumenttype = Constants.CST_DOCUMENTS.First().Key;

            //    return RedirectToAction("Index", GetCurrentControlerName(), new { documenttype = lastdocumenttype, sortcolumn = sortcolumn, sortdirection = sortdirection });
            //}
            //if (Request.RequestType == "POST")
            //    return RedirectToAction("Index", GetCurrentControlerName(), new { documenttype = documenttype, sortcolumn = sortcolumn, sortdirection = sortdirection });
            //var sagedatacontext = Tools.GetSageDataContext();

            var q = _dataService.GetDocHeaders(documentType ?? FIRST_DOCUMENT_TYPE);

            //ViewBag.SortDirection = string.IsNullOrEmpty(sortdirection) ? SortDirection.Ascending : Enum.Parse(typeof(SortDirection), sortdirection);
            //ViewBag.SortColumn = string.IsNullOrEmpty(sortcolumn) ? "DocumentNo" : sortcolumn;

            //switch ((SortDirection)ViewBag.SortDirection)
            //{
            //    case SortDirection.Descending:
            //        q = q.OrderByDescending(x => GetPropertyValue(x, ViewBag.SortColumn)).ToArray();
            //        break;
            //    default:
            //        q = q.OrderBy(x => GetPropertyValue(x, ViewBag.SortColumn)).ToArray();
            //        break;
            //}

            //ViewData["DocumentTypeSelectListItems"] = (from i in Constants.CST_DOCUMENTS
            //                                           select new SelectListItem() { Text = i.Value, Value = i.Key.ToString(), Selected = (i.Key == documenttype) }).ToArray();
            ViewData["DocumentTypeSelectListItems"] = (from i in IesWebPortalConstants.CST_DOCUMENTS
                                                       select new SelectListItem() { Text = i.Value, Value = i.Key.ToString(), Selected = (i.Key == (documentType ??  FIRST_DOCUMENT_TYPE)) }).ToArray();
            Response.SetCookie(
                IesWebPortalConstants.COOKIE_LASTSORTSALEPURCHASE,
                    new KeyValuePair<string, string>("SortColumn", ViewBag.SortColumn)
                //new KeyValuePair<string, string>("SortDirection", ((int)ViewBag.SortDirection ?? 0).ToString())
                );

            //Response.SetCookie(IesWebPortalConstants.COOKIE_LASTDOCUMENTTYPE, documentType.ToString());
            //return View(q);
            return View(q);
        }
    }
}
