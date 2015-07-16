using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CiresonPortalAPI
{
    public static partial class TypeProjectionController
    {
        const string GET_BY_CRITERIA_ENDPOINT = "/api/V3/Projection/GetProjectionByCriteria";

        /// <summary>
        /// Queries the Cireson Portal for objects using specified criteria.
        /// </summary>
        /// <param name="authToken">AuthenticationToken to use</param>
        /// <param name="criteria">QueryCriteria rules</param>
        /// <returns>List of ExpandoObjects</returns>
        public static async Task<List<ExpandoObject>> GetProjectionByCriteria(AuthorizationToken authToken, QueryCriteria criteria)
        {
            if (!authToken.IsValid)
            {
                throw new InvalidCredentialException("AuthorizationToken is not valid.");
            }

            try
            {
                // Initialize the HTTP helper and get going
                PortalHttpHelper helper = new PortalHttpHelper(authToken);
                string result = await helper.PostAsync(GET_BY_CRITERIA_ENDPOINT, criteria.ToString());

                // TypeProjections have no set properties, so we deserialize to a list of ExpandoObjects
                ExpandoObjectConverter converter = new ExpandoObjectConverter();
                dynamic objectList = JsonConvert.DeserializeObject<List<ExpandoObject>>(result, converter);

                return objectList;
            }
            catch (Exception e)
            {
                throw; // Rethrow exceptions
            }
        }
    }
}
