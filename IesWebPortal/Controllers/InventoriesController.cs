using IesWebPortal.Classes;
using IesWebPortal.Models;
using IesWebPortal.Services.Interfaces;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IesWebPortal.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class InventoriesController : ControllerBase
    {
        private readonly IDataService _dataService;
        private readonly string _picturePath;
        public InventoriesController(IDataService dataService, IIesWebPortalSettings iesWebPortalSettings)
        {
            _dataService = dataService;
            _picturePath = iesWebPortalSettings.PicturePath;
        }

        [HttpGet]
        [EnableQuery()]
        public IEnumerable<Inventory> Get()
        {
            var datas = _dataService.GetItemsLotSerial();
            var result = from i in datas
                         select new Inventory()
                         {
                             Depot = i.DeIntitule,
                             Reference = i.ItemNo,
                             NoLot = i.SerialNo ?? string.Empty,
                             Designation = i.Item.Description,
                             CodeFamille = i.Item.Family,
                             QuantiteStock = i.Qty,
                             PointEclair = double.IsNaN(i.Item.FlashPoint) ? new double?() : i.Item.FlashPoint,
                             CTE = double.IsNaN(i.Item.FlashPoint) ? new double?() : Tools.GetCTE(i.Item.FlashPoint, i.Qty),
                             DateFabrication = i.ManufacturingDate <= SageClassLibrary.DataModel.SageObject.SageMinDate ? new DateTime?() : i.ManufacturingDate,
                             DatePeremption = i.ExpirationDate <= SageClassLibrary.DataModel.SageObject.SageMinDate ? new DateTime?() : i.ExpirationDate,
                             Dangeureux = i.Item.Dangerous,
                         };

            return result.ToArray();
        }
    }
}
