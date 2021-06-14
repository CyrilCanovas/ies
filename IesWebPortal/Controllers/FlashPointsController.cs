using IesWebPortal.Classes;
using IesWebPortal.Models;
using IesWebPortal.Services.Interfaces;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IesWebPortal.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class FlashPointsController : ControllerBase
    {
        private readonly IDataService _dataService;
        private readonly string _picturePath;
        public FlashPointsController(IDataService dataService, IIesWebPortalSettings iesWebPortalSettings)
        {
            _dataService = dataService;
            _picturePath = iesWebPortalSettings.PicturePath;
        }

        [HttpGet]
        [EnableQuery()]
        public IEnumerable<FlashPoint> Get()
        {
            var datas = _dataService.GetInventories(x =>
            {
                return string.IsNullOrEmpty(_picturePath) ? Tools.MapPath(IesWebPortalConstants.PICTURE_PATH + x) : Path.Combine(_picturePath, x);
            }
            );
            var result = from i in datas

                         select new FlashPoint()
                         {
                             Depot = i.DeIntitule,
                             Reference = i.ItemNo,
                             Designation = i.Item.Description,
                             CodeFamille = i.Item.Family,
                             QuantiteStock = i.Qty,
                             Emplacement = i.LocationCode ?? string.Empty,
                             PointEclair = double.IsNaN(i.Item.FlashPoint) ? new double?() : i.Item.FlashPoint,
                             Coeff = double.IsNaN(i.Item.FlashPoint) ? new double?() : Math.Round(Tools.GetCoeff(i.Item.FlashPoint), 4),
                             CTE = double.IsNaN(i.Item.FlashPoint) ? new double?() : Tools.GetCTE(i.Item.FlashPoint, i.Qty),
                             PhraseRisque = i.Item.GetRisks().Keys.Count() == 0 ? null : string.Join(",", i.Item.GetRisks().Keys.ToArray()) + ".",
                             NumeroEiniecs = i.Item.GetEiniecs().Keys.Count() == 0 ? null : i.Item.GetEinecsCodes(),
                             RID = i.Item.RID,
                             IMDG = i.Item.IMDG,
                             ICADIATA = i.Item.ICADIATA,
                             Pictogramme = i.Item.Files,
                             UN = i.Item.GetUnMemoStruct() == null ? null : i.Item.GetUnMemoStruct().Description
                         };
            //return new List<FlashPoint>
            //{
            //    CreateNewStudent("Cody Allen", 130),
            //    CreateNewStudent("Todd Ostermeier", 160),
            //    CreateNewStudent("Viral Pandya", 140)
            //};
            return result.ToArray();
        }

        private static FlashPoint CreateNewStudent(string name, int score)
        {
            return new FlashPoint
            {
                Reference = Guid.NewGuid().ToString(),
                Depot = Guid.NewGuid().ToString(),
                Emplacement = string.Empty
            };
        }
    }
}
