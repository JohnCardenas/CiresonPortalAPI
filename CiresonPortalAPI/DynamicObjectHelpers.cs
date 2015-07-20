using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace CiresonPortalAPI
{
    public static class DynamicObjectHelpers
    {
        /// <summary>
        /// Returns true if the specified dynamic has a given property, false otherwise.
        /// </summary>
        /// <param name="expando">ExpandoObject</param>
        /// <param name="key">Property to check for</param>
        /// <returns></returns>
        public static bool HasProperty(ExpandoObject expando, string key)
        {
            return ((IDictionary<string, Object>) expando).ContainsKey(key);
        }
    }
}
