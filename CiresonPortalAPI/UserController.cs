using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Authentication;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CiresonPortalAPI
{
    public static class UserController
    {
        const string IS_USER_AUTHORIZED_ENDPOINT = "/api/V3/User/IsUserAuthorized";
        const string GET_TIER_QUEUES_ENDPOINT = "/api/V3/User/GetUsersTierQueueEnumerations";
        const string USER_LIST_ENDPOINT = "/api/V3/User/GetUserList";

        /// <summary>
        /// Returns a list of UserListItems, which contain an ID/Name pair
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="userFilter">Should we filter the query?</param>
        /// <param name="filterByAnalyst">Should we filter by analysts?</param>
        /// <param name="groupsOnly">Should we see groups only?</param>
        /// <param name="maxNumberOfResults">Maximum number of results in this query</param>
        /// <param name="sort">If true, this list will be sorted before returning it</param>
        /// <param name="insertNullItem">If true, a null item will be inserted as the first item</param>
        /// <returns></returns>
        public static async Task<List<PartialUser>> GetUserList(AuthorizationToken authToken, string userFilter = "", bool filterByAnalyst = false, bool groupsOnly = false, int maxNumberOfResults = 10, bool sort = true, bool insertNullItem = true)
        {
            if (!authToken.IsValid)
            {
                throw new InvalidCredentialException("AuthorizationToken is not valid.");
            }

            string endpoint = USER_LIST_ENDPOINT + "?userFilter=" + userFilter + "&filterByAnalyst=" + filterByAnalyst + "&groupsOnly=" + groupsOnly + "&maxNumberOfResults=" + maxNumberOfResults;

            try
            {
                // Initialize the HTTP helper and get going
                PortalHttpHelper helper = new PortalHttpHelper(authToken);
                string result = await helper.GetAsync(endpoint);

                // Deserialize
                List<PartialUser> returnList = JsonConvert.DeserializeObject<List<PartialUser>>(result);

                if (sort)
                {
                    returnList.Sort(new PartialUserComparer());
                }

                if (insertNullItem)
                {
                    returnList.Insert(0, new PartialUser(Guid.Empty, string.Empty));
                }

                return returnList;
            }
            catch (Exception e)
            {
                throw; // Rethrow exceptions
            }
        }

        /// <summary>
        /// Queries the Cireson Portal for the specified user's security rights
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="userName">User name to query</param>
        /// <param name="domain">Domain of the user</param>
        /// <returns></returns>
        public static async Task<ConsoleUser> GetIsUserAuthorized(AuthorizationToken authToken, string userName, string domain)
        {
            if (!authToken.IsValid)
            {
                throw new InvalidCredentialException("AuthorizationToken is not valid.");
            }

            string endpointUrl = IS_USER_AUTHORIZED_ENDPOINT + "?userName=" + userName + "&domain=" + domain;

            try
            {
                // Initialize the HTTP helper and get going
                PortalHttpHelper helper = new PortalHttpHelper(authToken);
                string result = await helper.PostAsync(endpointUrl, String.Empty);

                // Deserialize the object to an ExpandoObject and return a ConsoleUser
                ExpandoObjectConverter converter = new ExpandoObjectConverter();
                dynamic obj = JsonConvert.DeserializeObject<ExpandoObject>(result, converter);

                ConsoleUser returnObj = new ConsoleUser(obj);
                returnObj.IncidentSupportGroups = await UserController.GetUsersTierQueueEnumerations(authToken, returnObj);

                return returnObj;
            }
            catch (Exception e)
            {
                throw; // Rethrow exceptions
            }
        }

        /// <summary>
        /// Convenience method to query the Cireson Portal for the authenticated user's security rights.
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <returns></returns>
        public static async Task<ConsoleUser> GetIsUserAuthorized(AuthorizationToken authToken)
        {
            return await GetIsUserAuthorized(authToken, authToken.UserName, authToken.Domain);
        }

        /// <summary>
        /// Returns a list of tier queue (support group) enumerations that the specified ConsoleUser is a member of
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="user">ConsoleUser token</param>
        /// <returns></returns>
        internal static async Task<List<Enumeration>> GetUsersTierQueueEnumerations(AuthorizationToken authToken, ConsoleUser user)
        {
            if (!authToken.IsValid)
            {
                throw new InvalidCredentialException("AuthorizationToken is not valid.");
            }

            string endpointUrl = GET_TIER_QUEUES_ENDPOINT + "/" + user.Id.ToString("D");

            try
            {
                PortalHttpHelper helper = new PortalHttpHelper(authToken);
                string result = await helper.GetAsync(endpointUrl);

                dynamic obj = JsonConvert.DeserializeObject<List<ExpandoObject>>(result, new ExpandoObjectConverter());

                List<Enumeration> returnList = new List<Enumeration>();

                foreach (var enumJson in obj)
                {
                    returnList.Add(new Enumeration(enumJson.Id, enumJson.Text, enumJson.Name, true, false));
                }

                return returnList;
            }
            catch (Exception e)
            {
                throw; // Rethrow exceptions
            }
        }
    }
}
