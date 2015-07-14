using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;
using Newtonsoft.Json;

namespace CiresonPortalAPI
{
    internal class UserJson
    {
        public string Id;
        public string Name;
    }

    public class User
    {
        const string ENDPOINT = "/api/V3/User/GetUserList";

        private Guid _oId;
        private string _sName;

        public Guid   Id   { get { return _oId;   } }
        public string Name { get { return _sName; } }

        private User(string id, string name)
        {
            _oId   = new Guid(id);
            _sName = name;
        }

        public static async Task<List<User>> GetUserList(AuthorizationToken authToken, string userFilter = "", bool filterByAnalyst = false, bool groupsOnly = false, int maxNumberOfResults = 10)
        {
            if (!authToken.IsValid)
            {
                throw new InvalidCredentialException("AuthorizationToken is not valid.");
            }

            string endpoint = ENDPOINT + "?userFilter=" + userFilter + "&filterByAnalyst=" + filterByAnalyst + "&groupsOnly=" + groupsOnly + "&maxNumberOfResults=" + maxNumberOfResults;

            try
            {
                // Initialize the HTTP helper and get going
                PortalHttpHelper helper = new PortalHttpHelper(authToken);
                string result = await helper.GetAsync(endpoint);

                // This gets returned
                List<User> returnList = new List<User>();

                // Deserialize
                List<UserJson> jUserList = JsonConvert.DeserializeObject<List<UserJson>>(result);
                foreach (var jUser in jUserList)
                {
                    // Skip empty enumerations
                    if (jUser.Id != Guid.Empty.ToString())
                        returnList.Add(new User(jUser.Id, jUser.Name));
                }

                return returnList;
            }
            catch (Exception e)
            {
                throw; // Rethrow exceptions
            }
        }

        public override string ToString()
        {
            return _sName;
        }
    }
}
