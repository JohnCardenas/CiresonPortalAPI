using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;
using CiresonPortalAPI;
using Newtonsoft.Json;

namespace CiresonPortalAPI
{
    public static class EnumerationController
    {
        const string LIST_ENDPOINT_TREE = "/api/V3/Enum/GetList";
        const string LIST_ENDPOINT_FLAT = "/api/V3/Enum/GetFlatList";

        /// <summary>
        /// Fetches a list of enumerations from the server
        /// </summary>
        /// <param name="authToken">Authorization token</param>
        /// <param name="enumList">Enumeration list to fetch</param>
        /// <param name="flatten">If true, flatten the entire enumeration tree into one list; if false, only return the first-level items</param>
        /// <param name="sort">If true, sorts the list before returning it</param>
        /// <param name="insertNullItem">If true, add a null item to the list as the first item</param>
        /// <returns></returns>
        /// 
        public static async Task<List<Enumeration>> GetEnumerationList(AuthorizationToken authToken, Guid enumList, bool flatten, bool sort, bool insertNullItem)
        {
            if (!authToken.IsValid)
            {
                throw new InvalidCredentialException("AuthorizationToken is not valid.");
            }

            string endpoint = (flatten ? LIST_ENDPOINT_FLAT : LIST_ENDPOINT_TREE);
            endpoint += "?id=" + enumList.ToString() + (flatten ? "&itemFilter=" : "");

            try
            {
                // Initialize the HTTP helper and get going
                PortalHttpHelper helper = new PortalHttpHelper(authToken);
                string result = await helper.GetAsync(endpoint);

                List<Enumeration> returnList = new List<Enumeration>();

                if (flatten)
                {
                    // A flat enumeration list has null Ordinals, so we use the base EnumJson class
                    List<EnumJson> jEnumList = JsonConvert.DeserializeObject<List<EnumJson>>(result);
                    foreach (var jEnum in jEnumList)
                    {
                        // Skip empty enumerations
                        if (jEnum.ID != Guid.Empty.ToString())
                            returnList.Add(new Enumeration(new Guid(jEnum.ID), jEnum.Text, jEnum.Name, flatten, jEnum.HasChildren));
                    }
                }
                else
                {
                    // A non-flat enumeration list has non-null Ordinals, so we have to use a different conversion class
                    List<EnumJsonOrdinal> jEnumList = JsonConvert.DeserializeObject<List<EnumJsonOrdinal>>(result);
                    foreach (var jEnum in jEnumList)
                    {
                        // Skip empty enumerations
                        if (jEnum.ID != Guid.Empty.ToString())
                            returnList.Add(new Enumeration(new Guid(jEnum.ID), jEnum.Text, jEnum.Name, flatten, jEnum.HasChildren, jEnum.Ordinal));
                    }
                }

                if (sort)
                {
                    returnList.Sort(new EnumerationComparer());
                }

                if (insertNullItem)
                {
                    returnList.Insert(0, new Enumeration(Guid.Empty, string.Empty, string.Empty, true, false));
                }

                return returnList;
            }
            catch (Exception)
            {
                throw; // Rethrow exceptions
            }
        }
    }

}
