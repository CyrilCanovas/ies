using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IesWebPortal.Services.Interfaces
{
    public interface IMLSalePurchaseHeader
    {
        [DisplayName("N° Tiers")]
        string CustomerVendorNo { get; set; }
        
        [DisplayName("Date liv.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy}")]
        DateTime? DeleveryDate { get; set; }
        [DisplayName("Date document")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy}")]
        DateTime? DocumentDate { get; set; }
        [DisplayName("N° document")]
        string DocumentNo { get; set; }
        [DisplayName("Document type")]
        [ScaffoldColumn(false)]
        int DocumentType { get; set; }
        [DisplayName("Intitulé client")]
        string Intitule { get; set; }
        [ScaffoldColumn(false)]
        IMLSalePurchaseLine[] Lines { get; set; }
        [DisplayName("Référence")]
        string Reference { get; set; }
        [ScaffoldColumn(false)]
        IMLShipTo To { get; set; }
        [DisplayName("Id adresse")]
        [ScaffoldColumn(false)]
        int ToId { get; set; }
    }
}