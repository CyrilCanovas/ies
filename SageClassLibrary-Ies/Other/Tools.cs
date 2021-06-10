using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SageClassLibrary.Other
{
    public static class Tools
    {
        public static string IncStr(string value, int length)
        {
            string result = new string('0', length);
            int a = 1;
            int no = 0;
            int i = value.Length - 1;
            while (i >= 0)
            {
                if (Char.IsNumber(value, i))
                {
                    a *= 10;
                    i--;
                    continue;
                }
                break;
            }
            try
            {
                no = int.Parse(value.Substring(i + 1));
                no = (no + 1) % a;
                result = value.Substring(0, i + 1)
                    + no.ToString(
                        new String('0', value.Length - i - 1)
                    );
            }
            catch
            {
            }
            return result;
        }
    }
}
