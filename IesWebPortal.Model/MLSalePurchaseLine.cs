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

        public DateTime? DocumentDate { get; set; }


        public int DlNo { get; set; }

        public int LineNo { get; set; }

        
        public string ItemNo { get; set; }

        
        public string ExtItemNo { get; set; }

        
        public string Description { get; set; }

        
        public double Qty { get; set; }

        
        public int LabelCount { get; set; }

        public string SerialNo { get; set; }

        public DateTime? ManufacturedDate { get; set; }

        public DateTime? BestBeforeDate { get; set; }

        public double GrossWeight { get; set; }

        public double Tare { get; set; }

        public byte[] Picture1
        {
            get
            {
                return Item == null ? null : Item.Picture1;
            }
        }


        public byte[] Picture2
        {
            get
            {
                return Item == null ? null : Item.Picture2;
            }
        }


        public byte[] Picture3
        {
            get
            {
                return Item == null ? null : Item.Picture3;
            }
        }

        public byte[] Picture4
        {
            get
            {
                return Item == null ? null : Item.Picture4;
            }
        }


        public byte[] Picture5
        {
            get
            {
                return Item == null ? null : Item.Picture5;
            }
        }

        
        public IMLSalePurchaseHeader Header { get; set; }

        #region Header properties

        private string documentno;
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

        
        public IMLItemInfo Item { get; set; }
        #region Item properties


        public string FlashPoint
        {
            get
            {
                return Item == null ? string.Empty : (double.IsNaN(Item.FlashPoint) ? string.Empty : Item.FlashPoint.ToString("0.00") + "°C");
            }
        }


        public string DangerousFrench
        {
            get
            {
                return Item == null ? string.Empty : Item.DangerousFrench;
            }
        }


        public string DangerousEnglish
        {
            get
            {
                return Item == null ? string.Empty : Item.DangerousEnglish;
            }
        }
        public string RisksOthersFR
        {
            get
            {
                return Item == null ? string.Empty : Item.GetRisksOthers(ELanguages.French);
            }
        }
        public string RisksOthersEN
        {
            get
            {
                return Item == null ? string.Empty : Item.GetRisksOthers(ELanguages.English);
            }
        }

        
        public string RisksPFR
        {
            get
            {
                return Item == null ? string.Empty : Item.GetRisksP(ELanguages.French);
            }
        }

        
        public string RisksPEN
        {
            get
            {
                return Item == null ? string.Empty : Item.GetRisksP(ELanguages.English);
            }
        }

        
        public string EinecsCodes
        {
            get
            {
                return Item == null ? string.Empty : Item.GetEinecsCodes();
            }
        }
        
        public string EinecsDescriptions
        {
            get
            {
                return Item == null ? string.Empty : Item.GetEinecsDescriptions();
            }
        }

        
        public string UN
        {
            get
            {
                return Item == null ? string.Empty : Item.GetUN();
            }
        }

        
        public string UNCode
        {
            get
            {
                return Item == null ? string.Empty : Item.GetUNCode();
            }
        }

        #endregion

        #region ShipTo properties
        public string FullAddress
        {
            get
            {
                if (Header == null) return string.Empty;
                return Header.To == null ? string.Empty : Header.To.FullAddress;
            }
        }



        #endregion

        
        public string BestBeforeDateString
        {
            get
            {
                return BestBeforeDate == null ? string.Empty : BestBeforeDate.Value.ToString("dd/MM/yyyy");

            }
        }

        
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

        
        public string NetWeightString
        {
            get
            {
                return ToFormatKg(Qty);
            }
        }

        
        public string TareString
        {
            get
            {
                return ToFormatKg(Tare);
            }
        }


        public string GrossWeightString
        {
            get
            {
                if (Tare == double.NaN || Qty == double.NaN) return string.Empty;
                return ToFormatKg(Tare + Qty);
            }
        }

        
        public string RID
        {
            get
            {
                return Item == null ? string.Empty : Item.RID;
            }
        }
        
        public string IMDG
        {
            get
            {
                return Item == null ? string.Empty : Item.IMDG;
            }
        }
        
        public string ICADIATA
        {
            get
            {
                return Item == null ? string.Empty : Item.ICADIATA;
            }
        }

        
        public bool OnlyAddress
        {
            get;
            set;
        }

        
        public string VendorDisplayName
        {
            get;
            set;
        }
        
    }
}
