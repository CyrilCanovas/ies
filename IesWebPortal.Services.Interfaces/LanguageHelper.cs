using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IesWebPortal.Services.Interfaces
{
    public static class LanguageHelper
    {
        private static Dictionary<string, ELanguages> dicolanguages =
        new List<KeyValuePair<string, ELanguages>>(){
                new KeyValuePair<string,ELanguages>("fr",ELanguages.French),
                new KeyValuePair<string,ELanguages>("gb",ELanguages.English)}.ToDictionary(x => x.Key, x => x.Value);

        public static ELanguages ToELanguages(this string value)
        {
            var result = ELanguages.None;
            dicolanguages.TryGetValue(value.ToLower(), out result);
            return result;
        }

    }
}
