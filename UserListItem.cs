using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;
using Newtonsoft.Json;

namespace CiresonPortalAPI
{
    public static partial class UserController
    {
        const string USER_LIST_ENDPOINT = "/api/V3/User/GetUserList";

        /// <summary>
        /// Returns a list of UserListItems, which contain an ID/Name pair
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="userFilter">Should we filter the query?</param>
        /// <param name="filterByAnalyst">Should we filter by analysts?</param>
        /// <param name="groupsOnly">Should we see groups only?</param>
        /// <param name="maxNumberOfResults">Maximum number of results in this query</param>
        /// <returns></returns>
        public static async Task<List<UserListItem>> GetUserList(AuthorizationToken authToken, string userFilter = "", bool filterByAnalyst = false, bool groupsOnly = false, int maxNumberOfResults = 10)
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
                List<UserListItem> returnList = JsonConvert.DeserializeObject<List<UserListItem>>(result);

                return returnList;
            }
            catch (Exception e)
            {
                throw; // Rethrow exceptions
            }
        }
    }

    /// <summary>
    /// Represents a user id and name pairing
    /// </summary>
    public class UserListItem
    {
        private Guid _oId;
        private string _sName;

        public Guid   Id   { get { return _oId;   } }
        public string Name { get { return _sName; } }

        [JsonConstructor]
        internal UserListItem(string id, string name)
        {
            _oId   = new Guid(id);
            _sName = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
