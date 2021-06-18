using IesWebPortal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IesWebPortal.Model
{
    public class MLItemInfo : IMLItemInfo
    {
        private Dictionary<string, IMLMemoStruct[]> _risks;
        private Dictionary<string, IMLMemoStruct> _einiecs;
        private IMLMemoStruct _un;
        private readonly byte[] _emptyPicture;
        public MLItemInfo(Dictionary<string, IMLMemoStruct[]> risks, Dictionary<string, IMLMemoStruct> einiecs, IMLMemoStruct un, byte[] emptyPicture)
        {
            _risks = risks;
            _einiecs = einiecs;
            _un = un;
            _emptyPicture = emptyPicture;
        }
        public MLItemInfo(byte[] emptyPicture) : this(new Dictionary<string, IMLMemoStruct[]>(), new Dictionary<string, IMLMemoStruct>(), new MLMemoStruct(), emptyPicture)
        {

        }
        public void SetRisks(Dictionary<string, IMLMemoStruct[]> keyValuePairs)
        {
            _risks = keyValuePairs;
        }

        public Dictionary<string, IMLMemoStruct[]> GetRisks()
        {
            return _risks;
        }
            
        public void SetEiniecs(Dictionary<string, IMLMemoStruct> keyValuePairs)
        {
            _einiecs = keyValuePairs;
        }

        public Dictionary<string, IMLMemoStruct> GetEiniecs()
        {
            return _einiecs;
        }

        public void SetUn(IMLMemoStruct memoStruct)
        {
            _un = memoStruct;
        }

        public IMLMemoStruct GetUnMemoStruct()
        {
            return _un;
        }


        //private static byte[] emptypicture;
        //public static byte[] EmptyPicture
        //{
        //    get
        //    {
        //        if (emptypicture == null && (!string.IsNullOrEmpty(global::IesWebPortal.Properties.Settings.Default.EmptyPicture)))
        //        {
        //            try
        //            {
        //                emptypicture = File.ReadAllBytes(global::IesWebPortal.Properties.Settings.Default.EmptyPicture);
        //            }
        //            catch
        //            {
        //            }
        //        }
        //        return emptypicture;
        //    }
        //}
        public string GetRisksP(ELanguages language)
        {
            if (_risks == null) return string.Empty;

            var q = from i in _risks.Keys
                    where i.StartsWith("P")
                    orderby i
                    select (string.Format("{0} {1}", i.Trim(), (_risks[i].Where(x => x.Language == language).Select(x => x.FullDescription).SingleOrDefault() ?? string.Empty)).Trim());

            var sb = new StringBuilder();
            foreach (var value in q)
            {
                sb.Append(value);
                if (!value.EndsWith(".")) sb.Append(".");
            }
            return sb.ToString();
        }

        public string GetRisksOthers(ELanguages language)
        {
            if (_risks == null) return string.Empty;

            var q = from i in _risks.Keys
                    where (!i.StartsWith("P"))
                    orderby i
                    select (string.Format("{0} {1}", i.Trim(), (_risks[i].Where(x => x.Language == language).Select(x => x.FullDescription).SingleOrDefault() ?? string.Empty)).Trim());

            var sb = new StringBuilder();
            foreach (var value in q)
            {
                sb.Append(value);
                if (!value.EndsWith(".")) sb.Append(".");
            }
            return sb.ToString();
        }

        public string GetEinecsCodes()
        {
            if (_einiecs == null) return string.Empty;
            var q = (from i in _einiecs.Keys
                     orderby i
                     select i).ToArray();
            return string.Join(",", q) + ".";
        }

        public string GetEinecsDescriptions()
        {
            if (_einiecs == null) return string.Empty;
            var q = from i in _einiecs
                    orderby i.Key
                    //                    select (string.Format("{0} {1}", i.Key, i.Value.FullDescription??string.Empty).Trim());
                    select (string.Format("{0}", i.Value.FullDescription ?? string.Empty).Trim());

            var sb = new StringBuilder();
            foreach (var value in q)
            {
                if (string.IsNullOrEmpty(value)) continue;
                sb.Append(value);
                if (!value.EndsWith(".")) sb.Append(".");
            }
            return sb.ToString();
        }



        public string GetUN()
        {
            if (_un == null) return string.Empty;
            return string.Format("{0} {1}", _un.ShortName, _un.FullDescription);
        }
        private static Regex regexextractuncode = new Regex(@"(?<un>(UN)([ ])+([0-9]+))", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public string GetUNCode()
        {
            var match = regexextractuncode.Match(RID ?? string.Empty);
            if (!match.Success)
                match = regexextractuncode.Match(ICADIATA ?? string.Empty);
            if (!match.Success)
                match = regexextractuncode.Match(IMDG ?? string.Empty);
            if (match.Success)
                return match.Groups["un"].Value;
            else return string.Empty;
        }

        [DisplayName("No")]
        public string ItemNo { get; set; }

        [DisplayName("Désignation")]
        public string Description { get; set; }

        [DisplayName("Point éclair")]
        public double FlashPoint { get; set; }

        [DisplayName("Rubrique dangeureux")]
        public string Dangerous
        {
            get
            {
                var result = string.Empty;
                if (string.IsNullOrEmpty(DangerousEnglish) && string.IsNullOrEmpty(DangerousFrench)) return result;
                result = string.Format("{0}/{1}", DangerousFrench ?? string.Empty, DangerousEnglish ?? string.Empty);
                return result;
            }
            set
            {
                DangerousEnglish = string.Empty;
                DangerousFrench = string.Empty;

                if (string.IsNullOrEmpty(value)) return;
                var values = value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (values == null) return;
                if (values.Length == 0) return;
                DangerousFrench = values[0];
                if (values.Length > 1) DangerousEnglish = values[1];

            }
        }

        [DisplayName("Rubrique dangeureux français")]
        public string DangerousFrench { get; set; }

        [DisplayName("Rubrique dangeureux english")]
        public string DangerousEnglish { get; set; }

        public string RID { get; set; }
        public string IMDG { get; set; }
        public string ICADIATA { get; set; }

        private byte[] picture1;
        [DisplayName("Image1")]
        [ScaffoldColumn(false)]
        public byte[] Picture1
        {
            get
            {
                if (picture1 == null) return _emptyPicture;
                return picture1;
            }
            set
            {
                picture1 = value;
            }
        }


        private byte[] picture2;
        [DisplayName("Image2")]
        [ScaffoldColumn(false)]
        public byte[] Picture2
        {
            get
            {
                if (picture2 == null) return _emptyPicture;
                return picture2;
            }
            set
            {
                picture2 = value;
            }
        }


        private byte[] picture3;
        [DisplayName("Image3")]
        [ScaffoldColumn(false)]
        public byte[] Picture3
        {
            get
            {
                if (picture3 == null) return _emptyPicture;
                return picture3;
            }
            set
            {
                picture3 = value;
            }
        }

        private byte[] picture4;
        [DisplayName("Image4")]
        [ScaffoldColumn(false)]
        public byte[] Picture4
        {
            get
            {
                if (picture4 == null) return _emptyPicture;
                return picture4;
            }
            set
            {
                picture4 = value;
            }
        }

        private byte[] picture5;
        [DisplayName("Image5")]
        [ScaffoldColumn(false)]
        public byte[] Picture5
        {
            get
            {
                if (picture5 == null) return _emptyPicture;
                return picture5;
            }
            set
            {
                picture5 = value;
            }
        }
        [DisplayName("Fichiers")]
        [ScaffoldColumn(false)]
        public string Files
        {
            get;
            set;
        }

        [DisplayName("Famille")]
        [ScaffoldColumn(false)]
        public string Family
        {
            get;
            set;
        }

    }
}
