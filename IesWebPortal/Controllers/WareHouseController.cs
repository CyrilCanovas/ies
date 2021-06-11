using IesWebPortal.Classes;
using IesWebPortal.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SageClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using Microsoft.Extensions.Configuration;

namespace IesWebPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WareHouseController : ControllerBase
    {
        private static XNamespace XSI = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
        private readonly IDataService _dataService;
        private readonly string _picturePath;
        public WareHouseController(IDataService dataService, IIesWebPortalSettings iesWebPortalSettings)
        {
            _dataService = dataService;
            _picturePath = iesWebPortalSettings.PicturePath;
        }
        private XDocument GetXDocument(string schemaname, string rootElementName, out XNamespace mainxmlns)
        {
            var _mainxmlns = XNamespace.Get(string.Format("http://tempuri.org/{0}", schemaname));
            var url = Request.GetRawUrl();
            var uri = new Uri(new Uri(url), string.Format("/Xsd/{0}", schemaname));
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

        private IEnumerable<XAttribute> ToXAttributes(IMLItemInventory i)
        {
            var xAttributes = new List<XAttribute>()
                    {
                        new XAttribute("Depot", i.DeIntitule),
                        new XAttribute("Reference", i.ItemNo),
                        new XAttribute("Designation", i.Item.Description),
                        new XAttribute("CodeFamille", i.Item.Family),
                        new XAttribute("QuantiteStock", i.Qty),

                    };

            if (!string.IsNullOrEmpty(i.LocationCode))
            {
                xAttributes.Add(new XAttribute("Emplacement", i.LocationCode));
            }
            if (!double.IsNaN(i.Item.FlashPoint))
            {
                xAttributes.Add(new XAttribute("PointEclair", i.Item.FlashPoint));
            }
            if (!double.IsNaN(i.Item.FlashPoint))
            {
                xAttributes.Add(new XAttribute("Coeff", Math.Round(Tools.GetCoeff(i.Item.FlashPoint), 4)));
            }
            if (!double.IsNaN(i.Item.FlashPoint))
            {
                xAttributes.Add(new XAttribute("CTE", Tools.GetCTE(i.Item.FlashPoint, i.Qty)));
            }

            if (i.Item.GetRisks().Keys.Count() != 0)
            {
                xAttributes.Add(new XAttribute("PhraseRisque", string.Join(",", i.Item.GetRisks().Keys.ToArray()) + "."));
            }

            if (i.Item.GetEiniecs().Keys.Count() != 0)
            {
                xAttributes.Add(new XAttribute("NumeroEiniecs", i.Item.GetEinecsCodes()));
            }

            if (!string.IsNullOrEmpty(i.Item.RID))
            {
                xAttributes.Add(new XAttribute("RID", i.Item.RID));
            }
            if (!string.IsNullOrEmpty(i.Item.IMDG))
            {
                xAttributes.Add(new XAttribute("IMDG", i.Item.IMDG));
            }

            if (!string.IsNullOrEmpty(i.Item.ICADIATA))
            {
                xAttributes.Add(new XAttribute("ICADIATA", i.Item.ICADIATA));
            }

            if (!string.IsNullOrEmpty(i.Item.Files))
            {
                xAttributes.Add(new XAttribute("Pictogramme", i.Item.Files));
            }

            if (!string.IsNullOrEmpty(i.Item.Dangerous))
            {
                xAttributes.Add(new XAttribute("Dangeureux", i.Item.Dangerous));
            }
            if (i.Item.GetUnMemoStruct() != null)
            {
                if (!string.IsNullOrEmpty(i.Item.GetUnMemoStruct().Description))
                {
                    xAttributes.Add(new XAttribute("UN", i.Item.GetUnMemoStruct().Description));
                }
            }
            return xAttributes.ToArray();
        }

        public ActionResult FlashPoint()
        {
            var datas = _dataService.GetInventories(x =>
            {
                return string.IsNullOrEmpty(_picturePath) ? Tools.MapPath(IesWebPortalConstants.PICTURE_PATH + x) : Path.Combine(_picturePath, x);
            }
            );
            return ReturnAsXmlContent("FlashPoint.xsd", "ROOT",
              (xdocument, xmlns) =>
              {
                  var q = from i in datas orderby i.DeIntitule, i.ItemNo select new XElement(xmlns + "DATA", ToXAttributes(i));
                  xdocument.Root.Add(q);
              }
              );
        }

    }
}
