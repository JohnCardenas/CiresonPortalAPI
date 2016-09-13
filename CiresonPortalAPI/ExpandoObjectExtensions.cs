using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CiresonPortalAPI
{
    internal static class ExpandoObjectExtensions
    {
        /// <summary>
        /// Deep copies an ExpandoObject though serialization
        /// </summary>
        /// <param name="obj">ExpandoObject to copy</param>
        /// <returns></returns>
        public static ExpandoObject DeepCopy(this ExpandoObject obj)
        {
            string data = JsonConvert.SerializeObject(obj);
            ExpandoObjectConverter converter = new ExpandoObjectConverter();

            return JsonConvert.DeserializeObject<ExpandoObject>(data, converter);
        }
    }
}
