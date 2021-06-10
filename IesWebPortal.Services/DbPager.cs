using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IesWebPortal.Services
{
    public static class DbPager<T>
    {
        private static int PARAM_COUNT = 128;

        public static IEnumerable<T> Get<V>(V[] items, Func<V[], IEnumerable<T>> func)
        {
            if (func == null) return null;
            int i = 0;
            var result = new List<T>();
            while (i < items.Length)
            {
                var values = items.Skip(i).Take(PARAM_COUNT).ToArray();
                if (values == null) break;
                if (values.Length == 0) break;
                result.AddRange(func(values));

                i += PARAM_COUNT;
            }

            return result.ToArray();
        }
    }
}
