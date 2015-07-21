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
                    returnList.Insert(0, new PartialUser(new Guid(), string.Empty));
                }

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
    public class PartialUser
    {
        protected Guid   _oId;
        protected string _sName;

        public virtual Guid   Id   { get { return _oId;   } }
        public virtual string Name { get { return _sName; } }

        [JsonConstructor]
        internal PartialUser(string id, string name)
        {
            _oId = new Guid(id);
            _sName = name;
        }

        internal PartialUser(Guid id, string name)
        {
            _oId   = id;
            _sName = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    internal class PartialUserComparer : IComparer<PartialUser>
    {
        public int Compare(PartialUser a, PartialUser b)
        {
            return string.Compare(a.ToString(), b.ToString());
        }
    }
}
