using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IesWebPortal
{


    public class IesWebPortalSettings : IIesWebPortalSettings
    {
        public string EiniecsCapture { get; set; }
        public string RiskCapture { get; set; }
        public string[] CompanyInfo { get; set; }
        public string UnCapture { get; set; }
        public string EmptyPicture { get; set; }
        public string PictureCapture { get; set; }
        public string PicturePath { get; set; }
        public Dictionary<string, string> LabelSettings { get; set; }
    }
}
