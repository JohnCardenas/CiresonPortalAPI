using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Authentication;
using System.Dynamic;
using Newtonsoft.Json;

namespace CiresonPortalAPI.ConfigurationItems
{
    public static class UserController
    {
        #region Constants
        const string USER_LIST_ENDPOINT = "/api/V3/User/GetUserList";
        #endregion // Constants

        #region Public Methods
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
        public static async Task<List<User>> GetUserList(AuthorizationToken authToken, string userFilter = "", bool filterByAnalyst = false, bool groupsOnly = false, int maxNumberOfResults = 10, bool sort = true)
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
                List<PartialUser> partialList = JsonConvert.DeserializeObject<List<PartialUser>>(result);

                if (sort)
                {
                    partialList.Sort(new PartialUserComparer());
                }

                // Convert PartialUsers to Users
                List<User> returnList = new List<User>();
                foreach (PartialUser partialUser in partialList)
                {
                    returnList.Add(new User(partialUser));
                }

                return returnList;
            }
            catch (Exception)
            {
                throw; // Rethrow exceptions
            }
        }

        /// <summary>
        /// Fetches a User object by its BaseId
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="userId">User BaseId</param>
        /// <returns></returns>
        public static async Task<User> GetUserById(AuthorizationToken authToken, Guid userId)
        {
            if (!authToken.IsValid)
                throw new InvalidCredentialException("AuthorizationToken is not valid.");

            return await TypeProjectionController.GetByBaseId<User>(authToken, userId);
        }
        #endregion // Public Methods
    }
}
