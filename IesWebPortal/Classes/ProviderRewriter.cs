using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IesWebPortal.Classes
{
    public static class ProviderRewriter
    {
        private static Dictionary<string, string> substitutions = new Dictionary<string, string>()
        {
            {"XXXXX","fournisseur"},
            {"3GIVS","GIVAUDAN"},
            {"2GIVQA","GIVAUDAN"},
            {"GIVFAR","GIVAUDAN"},
            {"3DSMS","DSM"},
            {"3DSMAC","DSM"},
            {"3JIANGSU","JIANGSU"},
            {"VIEILLE","ALBERT VIEILLE"},
        };

        public static string GetVendorDisplayName(string code)
        {
            string result = null;
            if (substitutions.TryGetValue(code, out result))
            {
                return result;
            }
            return string.Empty;
        }
    }
}
