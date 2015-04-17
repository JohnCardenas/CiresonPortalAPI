using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static AuthorizationToken GetToken(string userName, string domain, string password, string languageCode)
        {
            return new AuthorizationToken(userName, domain, languageCode, "");
        }
    }
}
