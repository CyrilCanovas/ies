using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IesWebPortal
{
    public class IesWebPortalSettings : IIesWebPortalSettings
    {
        public string RiskCapture { get; set; }
        public string EiniecsCapture { get; set; }
        public string UnCapture { get; set; }
        public string PictureCapture { get; set; }
        public string PicturePath { get; set; }

    }
}
