using IesWebPortal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IesWebPortal.Model
{
    public class MLItemLotSerial : IMLItemLotSerial
    {
        public int DeNo { get; set; }
        public string DeIntitule { get; set; }
        public double Qty { get; set; }
        public string SerialNo { get; set; }
        public IMLItemInfo Item { get; set; }
        public string ItemNo { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
