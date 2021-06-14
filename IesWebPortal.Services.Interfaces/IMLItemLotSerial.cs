using System;

namespace IesWebPortal.Services.Interfaces
{
    public interface IMLItemLotSerial
    {
        string DeIntitule { get; set; }
        int DeNo { get; set; }
        DateTime ExpirationDate { get; set; }
        IMLItemInfo Item { get; set; }
        string ItemNo { get; set; }
        DateTime ManufacturingDate { get; set; }
        double Qty { get; set; }
        string SerialNo { get; set; }
    }
}