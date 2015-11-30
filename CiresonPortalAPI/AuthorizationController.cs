using System;
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
using CiresonPortalAPI;
using Newtonsoft.Json;

namespace CiresonPortalAPI
{
    public static partial class AuthorizationController
    {
        const string AUTHORIZATION_ENDPOINT = "/api/V3/Authorization/GetToken";

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
        public static async Task<AuthorizationToken> GetAuthorizationToken(string portalUrl, string userName, SecureString password, string languageCode = "ENU")
        {
            try
            {
                // First check to see if we have Windows Authentication enabled
                bool windowsAuthEnabled = await DetectWindowsAuthentication(portalUrl);

                // Set up credentials
                PortalCredentials credentials = new PortalCredentials();
                credentials.Username = userName;
                credentials.SecurePassword = password;

                // Initialize the HTTP helper and do the heavy lifting
                PortalHttpHelper helper = new PortalHttpHelper(portalUrl, credentials, windowsAuthEnabled);
                string result = await helper.PostAsync(AUTHORIZATION_ENDPOINT, JsonConvert.SerializeObject(new { UserName = credentials.Username, Password = credentials.Password, LanguageCode = languageCode }));

                // Strip off beginning and ending quotes
                result = result.TrimStart('\"').TrimEnd('\"');

                // Return the authorization token
                return new AuthorizationToken(portalUrl, credentials, languageCode, result, windowsAuthEnabled);
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
    }
}
