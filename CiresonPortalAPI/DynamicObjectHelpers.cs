﻿using System;
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

        
        /// <summary>
        /// Returns a dynamic object property if it is set, otherwise it returns the default value of the type.
        /// </summary>
        /// <typeparam name="T">Type of the property to retrieve</typeparam>
        /// <param name="expando">Dynamic object</param>
        /// <param name="key">Property to retrieve</param>
        /// <returns></returns>
        public static T GetProperty<T>(ExpandoObject expando, string key)
        {
            if (!HasProperty(expando, key))
                return default(T);

            var exp = expando as IDictionary<string, Object>;

            return (T)Convert.ChangeType(exp[key], typeof(T));
        }
    }
}
