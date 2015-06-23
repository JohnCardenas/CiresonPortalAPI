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

namespace CiresonPortalAPI
{
    public class CiresonApiException : Exception
    {
        public CiresonApiException() { }
        public CiresonApiException(string message) : base(message) { }
        public CiresonApiException(string message, Exception inner) : base(message, inner) { }
    }

    public class AuthorizationToken
    {
        const string AUTHORIZATION_ENDPOINT = "/api/V3/Authorization/GetToken";
        const int EXPIRATION_SECONDS = 300;

        private string   _sLanguageCode;
        private string   _sPortalUrl;
        private string   _sAuthToken;
        private DateTime _oExpirationTime;

        private Credential _credentials;

        public   string     LanguageCode { get { return _sLanguageCode; } }
        public   string     PortalUrl    { get { return _sPortalUrl;    } }
        internal string     AuthToken    { get { return _sAuthToken;    } }
        internal Credential Credentials  { get { return _credentials;   } }

        public string UserName { get { return GetUserName(_credentials.Username);   } }
        public string Domain   { get { return GetDomainName(_credentials.Username); } }

        private AuthorizationToken(Credential credentials, string languageCode, string portalUrl, string authToken)
        {
            _credentials     = credentials;
            _sLanguageCode   = languageCode;
            _sAuthToken      = authToken;
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
                if (string.IsNullOrEmpty(_sAuthToken))
                    return false;

                if (DateTime.Now >= _oExpirationTime)
                    return false;

                return true;
            }
        }

        private static string GetUserName(string userNameDomainToken)
        {
            // sAMAccountName syntax: DOMAIN\USER
            if (userNameDomainToken.Contains("\\"))
                return userNameDomainToken.After("\\");

            // userPrincipalName syntax: user@domain.tld
            if (userNameDomainToken.Contains("@"))
                return userNameDomainToken.Before("@");
            
            // Just a user name
            return userNameDomainToken;
        }

        private static string GetDomainName(string userNameDomainToken)
        {
            // sAMAccountName syntax: DOMAIN\USER
            if (userNameDomainToken.Contains("\\"))
                return userNameDomainToken.Before("\\");

            // userPrincipalName syntax: user@domain.tld
            if (userNameDomainToken.Contains("@"))
                return userNameDomainToken.After("@");

            // No domain!
            return string.Empty;
        }

        public static async Task<AuthorizationToken> GetAuthorizationToken(Credential userCreds, string languageCode, string portalUrl)
        {
            string domain = GetDomainName(userCreds.Username);
            string userName = GetUserName(userCreds.Username);

            // Create the network credential cache
            CredentialCache cache = new CredentialCache();
            cache.Add(new Uri(portalUrl), "Negotiate", new NetworkCredential(userCreds.Username, userCreds.SecurePassword, domain));

            // Create the HttpClientHandler
            HttpClientHandler handler = new HttpClientHandler();
            handler.Credentials = cache;

            // Create the HTTP client
            HttpClient client = new HttpClient(handler);
            client.BaseAddress = new Uri(portalUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Do the heavy lifting
            using (HttpResponseMessage response = await client.PostAsync(AUTHORIZATION_ENDPOINT, new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new {UserName = userCreds.Username, Password = userCreds.Password, LanguageCode = languageCode}))))
            {
                string result = string.Empty;

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    client.Dispose();
                    throw new InvalidCredentialException("Invalid username and/or password.");
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    // Get the error message from the server
                    result = await response.Content.ReadAsStringAsync();
                    client.Dispose();
                    throw new CiresonApiException(result);
                }
                else
                {
                    // Other unhandled errors
                    try
                    {
                        response.EnsureSuccessStatusCode();
                    }
                    catch (Exception e)
                    {
                        // Rethrow exception
                        throw e;
                    }
                    finally
                    {
                        response.Dispose();
                        client.Dispose();
                    }
                }

                return new AuthorizationToken(userCreds, languageCode, portalUrl, result);
            }
        }
    }
}
