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

        public   string            LanguageCode { get { return _sLanguageCode;  } }
        public   string            PortalUrl    { get { return _sPortalUrl;     } }
        internal string            Token        { get { return _sToken;         } }
        internal PortalCredentials Credentials  { get { return _oCredentials;   } }

        public string UserName { get { return _oCredentials.Username; } }
        public string Domain   { get { return _oCredentials.Domain;   } }

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
    }
}
