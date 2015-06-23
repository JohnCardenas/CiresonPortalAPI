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
    public class AuthorizationToken
    {
        const string AUTHORIZATION_ENDPOINT = "/api/V3/Authorization/GetToken";
        const int EXPIRATION_SECONDS = 300;

        private string            _sLanguageCode;
        private string            _sPortalUrl;
        private string            _sToken;
        private DateTime          _oExpirationTime;
        private PortalCredentials _oCredentials;

        public   string            LanguageCode { get { return _sLanguageCode; } }
        public   string            PortalUrl    { get { return _sPortalUrl;    } }
        internal string            Token        { get { return _sToken;        } }
        internal PortalCredentials Credentials  { get { return _oCredentials;  } }

        public string UserName { get { return _oCredentials.UserName; } }
        public string Domain   { get { return _oCredentials.Domain;   } }

        private AuthorizationToken(PortalCredentials credentials, string languageCode, string portalUrl, string token)
        {
            _oCredentials    = credentials;
            _sLanguageCode   = languageCode;
            _sToken          = token;
            _sPortalUrl      = portalUrl;
            _oExpirationTime = DateTime.Now.AddSeconds(EXPIRATION_SECONDS);
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

                if (DateTime.Now >= _oExpirationTime)
                    return false;

                return true;
            }
        }

        /// <summary>
        /// Retrieves an authorization token from the server
        /// </summary>
        /// <param name="portalUrl">URL of the Cireson Portal</param>
        /// <param name="userCreds">PortalCredentials object</param>
        /// <param name="languageCode">Portal language code</param>
        /// <returns></returns>
        public static async Task<AuthorizationToken> GetAuthorizationToken(string portalUrl, PortalCredentials userCreds, string languageCode = "ENU")
        {
            try
            {
                // Initialize the HTTP helper and do the heavy lifting
                PortalHttpHelper helper = new PortalHttpHelper(portalUrl, userCreds);
                string result = await helper.PostAsync(AUTHORIZATION_ENDPOINT, JsonConvert.SerializeObject(new {UserName = userCreds.Username, Password = userCreds.Password, LanguageCode = languageCode}));

                return new AuthorizationToken(userCreds, languageCode, portalUrl, result);
            }
            catch (Exception e)
            {
                throw; // Rethrow exceptions
            }
        }
    }
}
