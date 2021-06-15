using System.Collections.Generic;

namespace IesWebPortal
{
    public interface IIesWebPortalSettings
    {
        string[] CompanyInfo { get; set; }
        string EiniecsCapture { get; set; }
        string EmptyPicture { get; set; }
        Dictionary<string, string> LabelSettings { get; set; }
        string PictureCapture { get; set; }
        string PicturePath { get; set; }
        string RiskCapture { get; set; }
        string UnCapture { get; set; }
    }
}