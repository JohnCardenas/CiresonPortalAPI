using System;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Authentication;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CredentialManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CiresonPortalAPI
{
    public static partial class AuthorizationController
    {
        const string AUTHORIZATION_ENDPOINT      = "/api/V3/Authorization/GetToken";
        const string IS_USER_AUTHORIZED_ENDPOINT = "/api/V3/User/IsUserAuthorized";
        const string GET_TIER_QUEUES_ENDPOINT    = "/api/V3/User/GetUsersTierQueueEnumerations";

        /// <summary>
        /// Retrieves an authorization token from the server
        /// </summary>
        /// <param name="portalUrl">URL of the Cireson Portal</param>
        /// <param name="userName">User name</param>
        /// <param name="password">User's password</param>
        /// <param name="languageCode">Portal language code</param>
        /// <exception cref="System.Security.Authentication.InvalidCredentialException">Thrown when user credentials are invalid.</exception>
        /// <exception cref="CiresonPortalAPI.CiresonApiException">Thrown when an error occurs in the API.</exception>
        /// <returns></returns>
        public static async Task<AuthorizationToken> GetAuthorizationToken(string portalUrl, string userName, SecureString password, string domain, string languageCode = "ENU")
        {
            try
            {
                // First check to see if we have Windows Authentication enabled
                bool windowsAuthEnabled = await DetectWindowsAuthentication(portalUrl);

                // Set up credentials
                PortalCredentials credentials = new PortalCredentials();
                credentials.Username = userName;
                credentials.SecurePassword = password;
                credentials.Domain = domain;

                // Initialize the HTTP helper and do the heavy lifting
                PortalHttpHelper helper = new PortalHttpHelper(portalUrl, credentials, windowsAuthEnabled);
                string result = await helper.PostAsync(AUTHORIZATION_ENDPOINT, JsonConvert.SerializeObject(new { UserName = credentials.Username, Password = credentials.Password, LanguageCode = languageCode }));

                // Strip off beginning and ending quotes
                result = result.TrimStart('\"').TrimEnd('\"');

                // Create a new authorization token
                AuthorizationToken token = new AuthorizationToken(portalUrl, credentials, languageCode, result, windowsAuthEnabled);

                // Fetch this user's properties
                ConsoleUser user = await AuthorizationController.GetUserRights(token, userName, domain);
                token.User = user;

                return token;
            }
            catch
            {
                throw; // Rethrow exceptions
            }
        }

        /// <summary>
        /// Detects if the Portal is running Windows Authentication
        /// </summary>
        /// <param name="portalUrl">URL of the Cireson Portal</param>
        /// <returns></returns>
        private static async Task<bool> DetectWindowsAuthentication(string portalUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(portalUrl))
                {
                    // Headers to detect for Windows Authentication
                    AuthenticationHeaderValue negotiate = new AuthenticationHeaderValue("Negotiate");
                    AuthenticationHeaderValue ntlm = new AuthenticationHeaderValue("NTLM");
                    AuthenticationHeaderValue kerberos = new AuthenticationHeaderValue("Kerberos");

                    bool found = (response.Headers.WwwAuthenticate.Contains(negotiate) || response.Headers.WwwAuthenticate.Contains(ntlm) || response.Headers.WwwAuthenticate.Contains(kerberos));
                    return found;
                }
            }
        }

        /// <summary>
        /// Queries the Cireson Portal for the specified user's security rights
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="userName">User name to query</param>
        /// <param name="domain">Domain of the user</param>
        /// <returns></returns>
        public static async Task<ConsoleUser> GetUserRights(AuthorizationToken authToken, string userName, string domain)
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
                returnObj.IncidentSupportGroups = await GetUsersTierQueueEnumerations(authToken, returnObj);

                return returnObj;
            }
            catch (Exception)
            {
                throw; // Rethrow exceptions
            }
        }

        /// <summary>
        /// Convenience method to query the Cireson Portal for the authenticated user's security rights.
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <returns></returns>
        public static async Task<ConsoleUser> GetUserRights(AuthorizationToken authToken)
        {
            return await GetUserRights(authToken, authToken.User.UserName, authToken.User.Domain);
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

            string endpointUrl = GET_TIER_QUEUES_ENDPOINT + "?id=" + user.Id.ToString("D");

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
            catch (Exception)
            {
                throw; // Rethrow exceptions
            }
        }
    }
}
