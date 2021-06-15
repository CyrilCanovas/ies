using System;

namespace IesWebPortal.Services.Interfaces
{
    public interface IMLSalePurchaseHeader
    {
        string CustomerVendorNo { get; set; }
        DateTime? DeleveryDate { get; set; }
        DateTime? DocumentDate { get; set; }
        string DocumentNo { get; set; }
        int DocumentType { get; set; }
        string Intitule { get; set; }
        IMLSalePurchaseLine[] Lines { get; set; }
        string Reference { get; set; }
        IMLShipTo To { get; set; }
        int ToId { get; set; }
    }
}