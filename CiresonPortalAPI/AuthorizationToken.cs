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
        #region Constants
        const int EXPIRATION_SECONDS = 300;
        #endregion // Constants

        #region Fields
        private string            _sLanguageCode;
        private string            _sPortalUrl;
        private string            _sToken;
        private bool              _bWindowsAuthEnabled;
        private DateTime          _dExpirationTime;
        private PortalCredentials _oCredentials = null;
        private ConsoleUser       _oConsoleUser = null;
        #endregion // Fields

        #region Properties
        /// <summary>
        /// Returns the language code of this user's auth token
        /// </summary>
        public string LanguageCode
        {
            get { return _sLanguageCode; }
        }

        /// <summary>
        /// Returns the URL of the portal that owns this token
        /// </summary>
        public string PortalUrl
        {
            get { return _sPortalUrl; }
        }

        /// <summary>
        /// Returns the user associated with this token
        /// </summary>
        public ConsoleUser User
        {
            get { return _oConsoleUser; }
            internal set { _oConsoleUser = value; }
        }

        /// <summary>
        /// Returns the auth token string
        /// </summary>
        internal string Token
        {
            get { return _sToken; }
        }

        /// <summary>
        /// Returns the credentials associated with this token
        /// </summary>
        internal PortalCredentials Credentials
        {
            get { return _oCredentials; }
        }

        /// <summary>
        /// If true, Windows Auth is enabled on the portal associated with this token
        /// </summary>
        internal bool WindowsAuthEnabled
        {
            get { return _bWindowsAuthEnabled; }
        }

        /// <summary>
        /// Return the user name associated with this token
        /// </summary>
        [Obsolete("AuthorizationToken.UserName is deprecated, please use AuthorizationToken.User.UserName instead.")]
        public string UserName
        {
            get { return _oCredentials.Username; }
        }

        /// <summary>
        /// Return the domain associated with this token
        /// </summary>
        [Obsolete("AuthorizationToken.Domain is deprecated, please use AuthorizationToken.User.Domain instead.")]
        public string Domain
        {
            get { return _oCredentials.Domain; }
        }

        #endregion // Properties

        #region Constructor
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
        #endregion // Constructor

        #region General Methods
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
        #endregion // General Methods
    }
}
