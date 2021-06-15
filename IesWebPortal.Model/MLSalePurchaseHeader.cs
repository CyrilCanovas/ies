using IesWebPortal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IesWebPortal.Model
{
    public class MLSalePurchaseHeader : IMLSalePurchaseHeader
    {

        [DisplayName("Document type")]
        [ScaffoldColumn(false)]
        public int DocumentType { get; set; }

        [DisplayName("N° document")]
        public string DocumentNo { get; set; }

        [DisplayName("Date document")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy}")]
        public DateTime? DocumentDate { get; set; }

        [DisplayName("Date liv.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy}")]
        public DateTime? DeleveryDate { get; set; }

        [DisplayName("N° Tiers")]
        public string CustomerVendorNo { get; set; }

        [DisplayName("Référence")]
        public string Reference { get; set; }

        [DisplayName("Intitulé client")]
        public string Intitule { get; set; }

        [ScaffoldColumn(false)]
        public IMLShipTo To { get; set; }

        [DisplayName("Id adresse")]
        [ScaffoldColumn(false)]
        public int ToId { get; set; }

        [ScaffoldColumn(false)]
        public IMLSalePurchaseLine[] Lines { get; set; }


    }

}
