using IesWebPortal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IesWebPortal.Model
{

    public class MLSalePurchaseLine : IMLSalePurchaseLine
    {

        [DisplayName("Date document")]
        [ScaffoldColumn(false)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy}")]
        public DateTime? DocumentDate { get; set; }


        [DisplayName("Identifiant")]
        [ScaffoldColumn(false)]
        public int DlNo { get; set; }

        [DisplayName("N° ligne")]
        [ScaffoldColumn(false)]
        public int LineNo { get; set; }

        [DisplayName("Réf.")]
        public string ItemNo { get; set; }

        [DisplayName("Réf. externe (article)")]
        public string ExtItemNo { get; set; }

        [DisplayName("Désignation")]
        public string Description { get; set; }

        [DisplayName("Qté")]
        public double Qty { get; set; }

        [DisplayName("Nbr étiquettes")]
        public double LabelCount { get; set; }

        [DisplayName("N° Lot")]
        [ScaffoldColumn(false)]
        public string SerialNo { get; set; }

        [DisplayName("Fabrication")]
        [ScaffoldColumn(false)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy}", ApplyFormatInEditMode = true)]
        public DateTime? ManufacturedDate { get; set; }

        [DisplayName("Péremption")]
        [ScaffoldColumn(false)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy}", ApplyFormatInEditMode = true)]
        public DateTime? BestBeforeDate { get; set; }

        [DisplayName("Poids brut")]
        [ScaffoldColumn(false)]
        public double GrossWeight { get; set; }

        [DisplayName("Tare")]
        [ScaffoldColumn(false)]
        public double Tare { get; set; }

        [DisplayName("Image1")]
        [ScaffoldColumn(false)]
        public byte[] Picture1
        {
            get
            {
                return Item == null ? null : Item.Picture1;
            }
        }

        [DisplayName("Image2")]
        [ScaffoldColumn(false)]
        public byte[] Picture2
        {
            get
            {
                return Item == null ? null : Item.Picture2;
            }
        }

        [DisplayName("Image3")]
        [ScaffoldColumn(false)]
        public byte[] Picture3
        {
            get
            {
                return Item == null ? null : Item.Picture3;
            }
        }

        [DisplayName("Image4")]
        [ScaffoldColumn(false)]
        public byte[] Picture4
        {
            get
            {
                return Item == null ? null : Item.Picture4;
            }
        }

        [DisplayName("Image5")]
        [ScaffoldColumn(false)]
        public byte[] Picture5
        {
            get
            {
                return Item == null ? null : Item.Picture5;
            }
        }

        [ScaffoldColumn(false)]
        public IMLSalePurchaseHeader Header { get; set; }

        #region Header properties

        private string documentno;

        [DisplayName("No document")]
        public string DocumentNo
        {
            get
            {
                if (string.IsNullOrEmpty(documentno))
                    return Header == null ? string.Empty : Header.DocumentNo;
                else return documentno;
            }
            set
            {
                documentno = value;
            }
        }

        private string reference;

        [ScaffoldColumn(false)]
        [DisplayName("Reference")]
        public string Reference
        {
            get
            {
                if (string.IsNullOrEmpty(reference))
                    return Header == null ? string.Empty : Header.Reference;
                else return reference;
            }
            set
            {
                reference = value;
            }
        }

        private string intitule;

        [ScaffoldColumn(false)]
        [DisplayName("Intitule")]
        public string Intitule
        {
            get
            {
                if (string.IsNullOrEmpty(intitule))
                    return Header == null ? string.Empty : Header.Intitule;
                else return intitule;
            }
            set
            {
                intitule = value;
            }
        }
        #endregion

        [ScaffoldColumn(false)]
        public IMLItemInfo Item { get; set; }
        #region Item properties

        [ScaffoldColumn(false)]
        [DisplayName("Point éclair")]
        public string FlashPoint
        {
            get
            {
                return Item == null ? string.Empty : (double.IsNaN(Item.FlashPoint) ? string.Empty : Item.FlashPoint.ToString("0.00") + "°C");
            }
        }

        [ScaffoldColumn(false)]
        [DisplayName("Rubrique dangeureux français")]
        public string DangerousFrench
        {
            get
            {
                return Item == null ? string.Empty : Item.DangerousFrench;
            }
        }

        [ScaffoldColumn(false)]
        [DisplayName("Rubrique dangeureux english")]
        public string DangerousEnglish
        {
            get
            {
                return Item == null ? string.Empty : Item.DangerousEnglish;
            }
        }

        [ScaffoldColumn(false)]
        public string RisksOthersFR
        {
            get
            {
                return Item == null ? string.Empty : Item.GetRisksOthers(ELanguages.French);
            }
        }

        [ScaffoldColumn(false)]
        public string RisksOthersEN
        {
            get
            {
                return Item == null ? string.Empty : Item.GetRisksOthers(ELanguages.English);
            }
        }

        [ScaffoldColumn(false)]
        public string RisksPFR
        {
            get
            {
                return Item == null ? string.Empty : Item.GetRisksP(ELanguages.French);
            }
        }

        [ScaffoldColumn(false)]
        public string RisksPEN
        {
            get
            {
                return Item == null ? string.Empty : Item.GetRisksP(ELanguages.English);
            }
        }

        [ScaffoldColumn(false)]
        public string EinecsCodes
        {
            get
            {
                return Item == null ? string.Empty : Item.GetEinecsCodes();
            }
        }
        [ScaffoldColumn(false)]
        public string EinecsDescriptions
        {
            get
            {
                return Item == null ? string.Empty : Item.GetEinecsDescriptions();
            }
        }

        [ScaffoldColumn(false)]
        public string UN
        {
            get
            {
                return Item == null ? string.Empty : Item.GetUN();
            }
        }

        [ScaffoldColumn(false)]
        public string UNCode
        {
            get
            {
                return Item == null ? string.Empty : Item.GetUNCode();
            }
        }

        #endregion

        #region ShipTo properties
        [ScaffoldColumn(false)]
        public string FullAddress
        {
            get
            {
                if (Header == null) return string.Empty;
                return Header.To == null ? string.Empty : Header.To.FullAddress;
            }
        }



        #endregion

        [ScaffoldColumn(false)]
        public string BestBeforeDateString
        {
            get
            {
                return BestBeforeDate == null ? string.Empty : BestBeforeDate.Value.ToString("dd/MM/yyyy");

            }
        }

        [ScaffoldColumn(false)]
        public string ManufacturedDateString
        {
            get
            {
                return ManufacturedDate == null ? string.Empty : ManufacturedDate.Value.ToString("dd/MM/yyyy");

            }
        }



        private static string ToFormatKg(double value)
        {
            var result = string.Empty;
            if (value != double.NaN) result = value.ToString("0.000 kg");
            return result;
        }

        [ScaffoldColumn(false)]
        public string NetWeightString
        {
            get
            {
                return ToFormatKg(Qty);
            }
        }

        [ScaffoldColumn(false)]
        public string TareString
        {
            get
            {
                return ToFormatKg(Tare);
            }
        }

        [ScaffoldColumn(false)]
        public string GrossWeightString
        {
            get
            {
                if (Tare == double.NaN || Qty == double.NaN) return string.Empty;
                return ToFormatKg(Tare + Qty);
            }
        }

        [ScaffoldColumn(false)]
        public string RID
        {
            get
            {
                return Item == null ? string.Empty : Item.RID;
            }
        }
        [ScaffoldColumn(false)]
        public string IMDG
        {
            get
            {
                return Item == null ? string.Empty : Item.IMDG;
            }
        }
        [ScaffoldColumn(false)]
        public string ICADIATA
        {
            get
            {
                return Item == null ? string.Empty : Item.ICADIATA;
            }
        }

        [ScaffoldColumn(false)]
        public bool OnlyAddress
        {
            get;
            set;
        }

        [ScaffoldColumn(false)]
        public string VendorDisplayName
        {
            get;
            set;
        }
        
    }
}
