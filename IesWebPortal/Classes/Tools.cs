using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
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

        public static string GetMetricsCurrentResourceName(this HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            Endpoint endpoint = httpContext.GetEndpoint();
            return endpoint?.Metadata.GetMetadata<EndpointNameMetadata>()?.EndpointName;
        }

        public static object GetPropertyValue(this object obj, string property)
        {
            System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }

        public static string GetCurrentControlerName(this ControllerContext controllerContext)
        {
            return (string)controllerContext.RouteData.Values["controler"];
        }


        public static void SetCookie(this HttpResponse reponse,string name, params KeyValuePair<string, string>[] values)
        {
            if (values == null) return;

            reponse.Cookies.Append(name, JsonConvert.SerializeObject(values), new CookieOptions() { Expires = DateTime.Now.AddYears(1), HttpOnly = false, Secure = false, SameSite = SameSiteMode.Strict});
            //var cookie = new HttpCookie(name);
            //cookie.Expires = DateTime.Now.AddYears(1);
            //foreach (var item in values)
            //    cookie.Values.Add(item.Key, item.Value);
            //Response.Cookies.Set(cookie);
        }

        public static void SetCookie(this HttpResponse reponse, string name, string value)
        {
            if (value == null) return;

            reponse.Cookies.Append(name, value, new CookieOptions() { Expires = DateTime.Now.AddYears(1), HttpOnly = false, Secure = false, SameSite = SameSiteMode.Strict });
            //var cookie = new HttpCookie(name);
            //cookie.Expires = DateTime.Now.AddYears(1);
            //foreach (var item in values)
            //    cookie.Values.Add(item.Key, item.Value);
            //Response.Cookies.Set(cookie);
        }
        //public static string MapPath(string path)
        //{
        //    return Path.Combine(
        //        (string)AppDomain.CurrentDomain.GetData("ContentRootPath"),
        //        path);
        //}
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


        public static string GetRawUrl(this HttpRequest request)
        {
            var httpContext = request.HttpContext;
            return $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}{httpContext.Request.QueryString}";
            
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
