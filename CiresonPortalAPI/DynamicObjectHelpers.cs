using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.ComponentModel;

namespace CiresonPortalAPI
{
    internal static class DynamicObjectHelpers
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

            Type t = typeof(T);
            Type u = Nullable.GetUnderlyingType(t);

            var exp = expando as IDictionary<string, Object>;

            // If we're natively dealing with a Guid, return it
            if ((t == typeof(Guid)) && (exp[key] is Guid))
            {
                return (T)exp[key];
            }
            // If we're dealing with a string but we need a Guid, cast it and return
            else if ((t == typeof(Guid)) && (exp[key] is String))
            {
                Guid g = new Guid((string)exp[key]);
                return (T)Convert.ChangeType(g, t);
            }
            // If we have a nullable type we need to handle it differently
            else if (u != null)
            {
                return (exp[key] == null) ? default(T) : (T)Convert.ChangeType(exp[key], u);
            }
            else
            {
                return (T)Convert.ChangeType(exp[key], t);
            }
        }
    }
}
