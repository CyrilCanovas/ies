using IesWebPortal.Services.Interfaces;
using System;

namespace IesWebPortal.Model
{
    public class MLItemInventory : IMLItemInventory
    {
        public int DeNo { get; set; }
        public string DeIntitule { get; set; }
        public double Qty { get; set; }
        public string ItemNo { get; set; }
        public IMLItemInfo Item { get; set; }
        public int NoPrincipal { get; set; }
        public string LocationCode { get; set; }
    }
}
