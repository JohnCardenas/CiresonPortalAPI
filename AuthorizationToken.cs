using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CredentialManagement;

namespace CiresonPortalAPI
{
    public class AuthorizationException : Exception
    {
        public AuthorizationException() { }
        public AuthorizationException(string message) : base(message) { }
        public AuthorizationException(string message, Exception inner) : base(message, inner) { }
    }

    public class AuthorizationToken
    {
        const string AUTHORIZATION_ENDPOINT = "/api/V3/Authorization/GetToken";

        private string _sUserName;
        private string _sDomain;
        private string _sLanguageCode;
        private string _sAuthToken;

        public string UserName     { get { return _sUserName;     } }
        public string Domain       { get { return _sDomain;       } }
        public string LanguageCode { get { return _sLanguageCode; } }
        public string AuthToken    { get { return _sAuthToken;    } }

        private AuthorizationToken(string userName, string domain, string languageCode, string authToken)
        {
            _sUserName     = userName;
            _sDomain       = domain;
            _sLanguageCode = languageCode;
            _sAuthToken    = authToken;
        }

        public static AuthorizationToken GetToken(string userName, string domain, string password, string languageCode, string portalUrl, bool remember)
        {
            return new AuthorizationToken(userName, domain, languageCode, "");
        }
    }

    /// <summary>
    /// Extension methods to the System.String and System.Security.SecureString classes
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Extends System.String to include a method to convert a regular string to a SecureString
        /// </summary>
        /// <param name="unsecStr">Unsecure string to convert</param>
        /// <returns></returns>
        public static SecureString ConvertToSecureString(this string unsecStr)
        {
            if (unsecStr == null)
                throw new ArgumentNullException("unsecStr");

            unsafe
            {
                // Read in the string as a character array
                fixed(char* stringChars = unsecStr)
                {
                    var secStr = new SecureString(stringChars, unsecStr.Length);
                    secStr.MakeReadOnly();
                    return secStr;
                }
            }
        }

        /// <summary>
        /// Extends System.Security.SecureString to include a method to convert a SecureString to a regular string
        /// </summary>
        /// <param name="secStr">Secure string to convert</param>
        /// <returns></returns>
        public static string ConvertToInsecureString(this SecureString secStr)
        {
            if (secStr == null)
                throw new ArgumentNullException("secStr");

            // Marshal the SecureString to a pointer and convert
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secStr);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                // Flush the native buffer to zero out the password
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
