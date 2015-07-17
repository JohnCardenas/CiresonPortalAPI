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
            catch (Exception e)
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

    /// <summary>
    /// Authorization token used by all CiresonPortalAPI methods interacting with endpoints.
    /// </summary>
    public class AuthorizationToken
    {
        const int EXPIRATION_SECONDS = 300;

        private string            _sLanguageCode;
        private string            _sPortalUrl;
        private string            _sToken;
        private bool              _bWindowsAuthEnabled;
        private DateTime          _dExpirationTime;
        private PortalCredentials _oCredentials = null;
        private ConsoleUser       _oConsoleUser = null;

        public   string            LanguageCode { get { return _sLanguageCode;  } }
        public   string            PortalUrl    { get { return _sPortalUrl;     } }
        internal string            Token        { get { return _sToken;         } }
        internal PortalCredentials Credentials  { get { return _oCredentials;   } }

        public string UserName { get { return _oCredentials.UserNameNoDomain; } }
        public string Domain   { get { return _oCredentials.Domain;           } }

        internal bool WindowsAuthEnabled { get { return _bWindowsAuthEnabled; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="portalUrl">URL to access</param>
        /// <param name="credentials">Credentials</param>
        /// <param name="languageCode">Language code of the Portal</param>
        /// <param name="token">Authorization token</param>
        /// <param name="windowsAuthEnabled">Is Windows Authentication enabled?</param>
        internal AuthorizationToken(string portalUrl, PortalCredentials credentials, string languageCode, string token, bool windowsAuthEnabled)
        {
            _oCredentials        = credentials;
            _sLanguageCode       = languageCode;
            _sToken              = token;
            _sPortalUrl          = portalUrl;
            _dExpirationTime     = DateTime.Now.AddSeconds(EXPIRATION_SECONDS);
            _bWindowsAuthEnabled = windowsAuthEnabled;
        }

        /// <summary>
        /// Checks the state of the AuthorizationToken
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(_sToken))
                    return false;

                if (DateTime.Now >= _dExpirationTime)
                    return false;

                return true;
            }
        }

        /// <summary>
        /// Caching convenience method to retrieve a ConsoleUser object
        /// </summary>
        /// <returns></returns>
        public async Task<ConsoleUser> ConsoleUser()
        {
            if (!this.IsValid)
                return null;

            try
            {
                if (_oConsoleUser == null)
                    _oConsoleUser = await UserController.GetIsUserAuthorized(this);
            }
            catch (Exception e)
            {
                throw; // Rethrow
            }

            return _oConsoleUser;
        }
    }
}
