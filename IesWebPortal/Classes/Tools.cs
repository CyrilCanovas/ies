using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace IesWebPortal.Classes
{
    public static class Tools
    {
        public static string ToUTF8String(this XDocument xdocument)
        {
            string result;
            using (var sw = new MemoryStream())
            {
                using (var strw = new StreamWriter(sw, System.Text.UTF8Encoding.UTF8))
                {
                    xdocument.Save(strw, SaveOptions.DisableFormatting | SaveOptions.OmitDuplicateNamespaces);

                    result = System.Text.UTF8Encoding.UTF8.GetString(sw.ToArray());
                }
            }
            return result;
        }



        public static double GetAsDouble(string value)
        {
            double result = 0;
            if (!string.IsNullOrEmpty(value))
                try
                {
                    result = XmlConvert.ToDouble(value.Replace(",", ".").Trim());
                }
                catch
                {
                }
            return result;

        }

        public static DateTime GetAsDateTime(string value, out bool berror)
        {
            berror = false;
            DateTime result = DateTime.MinValue;
            if (!string.IsNullOrEmpty(value))
                try
                {
                    result = XmlConvert.ToDateTime(value, "dd/MM/yy");
                }
                catch
                {
                    berror = true;
                }
            return result;

        }


        public static int GetAsInt(string value)
        {
            int result = 0;
            Int32.TryParse(value, out result);
            return result;
        }

        public static double GetCoeff(double flashpoint)
        {
            double coeff;
            if (flashpoint > 100) coeff = 1 / 15d;
            else if (flashpoint > 55) coeff = 1 / 5d;
            else if (flashpoint > 0) coeff = 1d;
            else coeff = 10d;
            return coeff;
        }

        public static double GetCTE(double flashpoint, double qty)
        {
            return Math.Round(GetCoeff(flashpoint) * qty, 3);
        }




    }
}
