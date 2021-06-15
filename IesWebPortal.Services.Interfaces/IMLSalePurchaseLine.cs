using System;

namespace IesWebPortal.Services.Interfaces
{
    public interface IMLSalePurchaseLine
    {
        DateTime? BestBeforeDate { get; set; }
        string BestBeforeDateString { get; }
        string DangerousEnglish { get; }
        string DangerousFrench { get; }
        string Description { get; set; }
        int DlNo { get; set; }
        DateTime? DocumentDate { get; set; }
        string DocumentNo { get; set; }
        string EinecsCodes { get; }
        string EinecsDescriptions { get; }
        string ExtItemNo { get; set; }
        string FlashPoint { get; }
        string FullAddress { get; }
        double GrossWeight { get; set; }
        string GrossWeightString { get; }
        IMLSalePurchaseHeader Header { get; set; }
        string ICADIATA { get; }
        string IMDG { get; }
        string Intitule { get; set; }
        IMLItemInfo Item { get; set; }
        string ItemNo { get; set; }
        double LabelCount { get; set; }
        int LineNo { get; set; }
        DateTime? ManufacturedDate { get; set; }
        string ManufacturedDateString { get; }
        string NetWeightString { get; }
        bool OnlyAddress { get; set; }
        byte[] Picture1 { get; }
        byte[] Picture2 { get; }
        byte[] Picture3 { get; }
        byte[] Picture4 { get; }
        byte[] Picture5 { get; }
        double Qty { get; set; }
        string Reference { get; set; }
        string RID { get; }
        string RisksOthersEN { get; }
        string RisksOthersFR { get; }
        string RisksPEN { get; }
        string RisksPFR { get; }
        string SerialNo { get; set; }
        double Tare { get; set; }
        string TareString { get; }
        string UN { get; }
        string UNCode { get; }
        string VendorDisplayName { get; set; }
    }
}