using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IesWebPortal.Services.Interfaces
{
    public interface IMLSalePurchaseLine
    {
        [DisplayName("Péremption")]
        [ScaffoldColumn(false)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy}", ApplyFormatInEditMode = true)]
        DateTime? BestBeforeDate { get; set; }
        [ScaffoldColumn(false)]
        string BestBeforeDateString { get; }
        [ScaffoldColumn(false)]
        [DisplayName("Rubrique dangeureux english")]
        string DangerousEnglish { get; }
        [ScaffoldColumn(false)]
        [DisplayName("Rubrique dangeureux français")]
        string DangerousFrench { get; }
        [DisplayName("Désignation")]
        string Description { get; set; }
        [DisplayName("Identifiant")]
        [ScaffoldColumn(false)]
        int DlNo { get; set; }
        [DisplayName("Date document")]
        [ScaffoldColumn(false)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy}")]
        DateTime? DocumentDate { get; set; }
        [DisplayName("No document")]
        string DocumentNo { get; set; }
        [ScaffoldColumn(false)]
        string EinecsCodes { get; }
        [ScaffoldColumn(false)]
        string EinecsDescriptions { get; }
        [DisplayName("Réf. externe (article)")]
        string ExtItemNo { get; set; }
        [ScaffoldColumn(false)]
        [DisplayName("Point éclair")]
        string FlashPoint { get; }
        [ScaffoldColumn(false)]
        string FullAddress { get; }

        [DisplayName("Poids brut")]
        [ScaffoldColumn(false)]
        double GrossWeight { get; set; }
        [ScaffoldColumn(false)]
        string GrossWeightString { get; }
        [ScaffoldColumn(false)]
        IMLSalePurchaseHeader Header { get; set; }
        [ScaffoldColumn(false)]
        string ICADIATA { get; }
        [ScaffoldColumn(false)]
        string IMDG { get; }

        [ScaffoldColumn(false)]
        [DisplayName("Intitule")]
        string Intitule { get; set; }
        IMLItemInfo Item { get; set; }
        [DisplayName("Réf.")]
        string ItemNo { get; set; }
        [DisplayName("Nbr étiquettes")]
        int LabelCount { get; set; }
        [DisplayName("N° ligne")]
        [ScaffoldColumn(false)]
        int LineNo { get; set; }

        [DisplayName("Fabrication")]
        [ScaffoldColumn(false)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy}", ApplyFormatInEditMode = true)]
        DateTime? ManufacturedDate { get; set; }
        [ScaffoldColumn(false)]
        string ManufacturedDateString { get; }
        [ScaffoldColumn(false)]
        string NetWeightString { get; }
        [ScaffoldColumn(false)]
        bool OnlyAddress { get; set; }

        [DisplayName("Image1")]
        [ScaffoldColumn(false)]
        byte[] Picture1 { get; }
        [DisplayName("Image2")]
        [ScaffoldColumn(false)]
        byte[] Picture2 { get; }
        [DisplayName("Image3")]
        [ScaffoldColumn(false)]
        byte[] Picture3 { get; }

        [DisplayName("Image4")]
        [ScaffoldColumn(false)]
        byte[] Picture4 { get; }
        [DisplayName("Image5")]
        [ScaffoldColumn(false)]
        byte[] Picture5 { get; }
        [DisplayName("Qté")]
        double Qty { get; set; }
        [ScaffoldColumn(false)]
        [DisplayName("Reference")]
        string Reference { get; set; }
        [ScaffoldColumn(false)]
        string RID { get; }
        [ScaffoldColumn(false)]
        string RisksOthersEN { get; }
        [ScaffoldColumn(false)]
        string RisksOthersFR { get; }
        [ScaffoldColumn(false)]
        string RisksPEN { get; }
        [ScaffoldColumn(false)]
        string RisksPFR { get; }
        [DisplayName("N° Lot")]
        [ScaffoldColumn(false)]
        string SerialNo { get; set; }
        [DisplayName("Tare")]
        [ScaffoldColumn(false)]
        double Tare { get; set; }
        [ScaffoldColumn(false)]
        string TareString { get; }
        [ScaffoldColumn(false)]
        string UN { get; }
        [ScaffoldColumn(false)]
        string UNCode { get; }
        [ScaffoldColumn(false)]
        string VendorDisplayName { get; set; }
    }
}