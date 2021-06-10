using IesWebPortal.Classes;
using IesWebPortal.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SageClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IesWebPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WareHouseController : ControllerBase
    {
        private static XNamespace XSI = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
        private readonly IDataService _dataService;
        public WareHouseController(IDataService dataService)
        {
            _dataService = dataService;
        }
        private XDocument GetXDocument(string schemaname, string rootElementName, out XNamespace mainxmlns)
        {
            var _mainxmlns = XNamespace.Get(string.Format("http://tempuri.org/{0}", schemaname));
            var uri = new Uri(new Uri(Request.PathBase), string.Format("/Xsd/{0}", schemaname));
            var schemalocation = XNamespace.Get(string.Format("http://tempuri.org/{0}", schemaname) + " " + uri.AbsoluteUri);

            var xdocument = new XDocument(
                new XElement(rootElementName,
                new XAttribute(XNamespace.Xmlns + "xsi", XSI),
                new XAttribute(XSI + "schemaLocation", schemalocation)
            ));
            xdocument.Declaration = new XDeclaration("1.0", "utf-8", "yes");
            xdocument.Root.Name = _mainxmlns + xdocument.Root.Name.LocalName;
            mainxmlns = _mainxmlns;
            return xdocument;
        }
        private ContentResult ReturnAsXmlContent(string schemaname, string rootElementName, Action<XDocument, XNamespace> action)
        {
            var result = new ContentResult() { ContentType = "text/xml" };
            XNamespace mainxmlns;
            var xdocument = GetXDocument(schemaname, rootElementName, out mainxmlns);
            if (action != null) action(xdocument, mainxmlns);
            result.Content = xdocument.ToUTF8String();
            return result;
        }

        public ActionResult FlashPoint()
        {

            //var datas = DataHelper.GetInventories(_sageDataContext);
            var datas = _dataService.GetInventories();
            return ReturnAsXmlContent("FlashPoint.xsd", "ROOT",
              (xdocument, xmlns) =>
              {
                  var q = from i in datas
                          orderby i.DeIntitule, i.ItemNo
                          select new XElement(xmlns + "DATA",
                              new XAttribute("Depot", i.DeIntitule),
                              new XAttribute("Reference", i.ItemNo),
                              new XAttribute("Designation", i.Item.Description),
                              new XAttribute("CodeFamille", i.Item.Family),
                              string.IsNullOrEmpty(i.LocationCode) ? null : new XAttribute("Emplacement", i.LocationCode),
                              double.IsNaN(i.Item.FlashPoint) ? null : new XAttribute("PointEclair", i.Item.FlashPoint),
                              double.IsNaN(i.Item.FlashPoint) ? null : new XAttribute("Coeff", Math.Round(Tools.GetCoeff(i.Item.FlashPoint), 4)),
                              new XAttribute("QuantiteStock", i.Qty),
                              double.IsNaN(i.Item.FlashPoint) ? null : new XAttribute("CTE", Tools.GetCTE(i.Item.FlashPoint, i.Qty)),
                              i.Item.GetRisks().Keys.Count() == 0 ? null : new XAttribute("PhraseRisque", string.Join(",", i.Item.GetRisks().Keys.ToArray()) + "."),
                              i.Item.GetEiniecs().Keys.Count() == 0 ? null : new XAttribute("NumeroEiniecs", i.Item.GetEinecsCodes()),
                              string.IsNullOrEmpty(i.Item.RID) ? null : new XAttribute("RID", i.Item.RID),
                              string.IsNullOrEmpty(i.Item.IMDG) ? null : new XAttribute("IMDG", i.Item.IMDG),
                              string.IsNullOrEmpty(i.Item.ICADIATA) ? null : new XAttribute("ICADIATA", i.Item.ICADIATA),
                              string.IsNullOrEmpty(i.Item.Files) ? null : new XAttribute("Pictogramme", i.Item.Files),
                              string.IsNullOrEmpty(i.Item.Dangerous) ? null : new XAttribute("Dangeureux", i.Item.Dangerous),
                              string.IsNullOrEmpty(i.Item.GetUnMemoStruct().Description)  ? null : new XAttribute("UN", i.Item.GetUnMemoStruct().Description)
                            );

                  xdocument.Root.Add(q);
              }
              );
        }

    }
}
