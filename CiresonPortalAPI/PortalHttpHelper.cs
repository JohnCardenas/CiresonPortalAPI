using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Security.Authentication;
using CredentialManagement;

namespace CiresonPortalAPI
{
    /// <summary>
    /// This class is used internally within CiresonPortalAPI for communicating with the Cireson Portal's REST endpoints
    /// </summary>
    internal class PortalHttpHelper
    {
        private HttpClient _oHttpClient = null;

        /// <summary>
        /// Default constructor, no Authorization header; should only be used with the /Authorization/GetToken endpoint
        /// </summary>
        /// <param name="portalUrl">URL of the Cireson Portal</param>
        /// <param name="credentials">PortalCredentials object</param>
        /// <param name="windowsAuthEnabled">Does the portal have Windows Authentication enabled?</param>
        public PortalHttpHelper(string portalUrl, PortalCredentials credentials, bool windowsAuthEnabled)
        {
            // Create the HttpClientHandler
            HttpClientHandler handler = new HttpClientHandler();

            // Add credentials to the client handler if we're using Windows Auth
            if (windowsAuthEnabled)
            {
                // Create the network credential cache and add it to the handler
                CredentialCache cache = new CredentialCache();
                cache.Add(new Uri(portalUrl), "Negotiate", new NetworkCredential(credentials.Username, credentials.SecurePassword, credentials.Domain));
                handler.Credentials = cache;
            }

            // Create the HTTP client
            _oHttpClient = new HttpClient(handler);
            _oHttpClient.BaseAddress = new Uri(portalUrl);
            _oHttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        /// <summary>
        /// Overloaded constructor with an Authorization header; suitable for connecting to endpoints other than /Authorization/GetToken
        /// </summary>
        /// <param name="authToken">AuthorizationToken object</param>
        /// <exception cref="CiresonPortalAPI.CiresonApiException"></exception>
        public PortalHttpHelper(AuthorizationToken authToken) : this(authToken.PortalUrl, authToken.Credentials, authToken.WindowsAuthEnabled)
        {
            if (!authToken.IsValid)
            {
                throw new CiresonApiException("Authorization token is not valid.");
            }

            _oHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authToken.Token);
        }

        /// <summary>
        /// Performs an asynchronous POST and processes the result
        /// </summary>
        /// <param name="url">URL to POST to</param>
        /// <param name="payload">Data to POST</param>
        /// <returns>Result message from the server</returns>
        /// <exception cref="System.Security.Authentication.InvalidCredentialException"></exception>
        /// <exception cref="CiresonPortalAPI.CiresonApiException"></exception>
        public async Task<string> PostAsync(string url, string payload)
        {
            try
            {
                using (HttpResponseMessage response = await _oHttpClient.PostAsync(url, new StringContent(payload, Encoding.UTF8, "application/json")))
                {
                    return await ProcessResponse(response);
                }   
            }
            // Rethrow expected exceptions, crash out for bugs
            catch (InvalidCredentialException) { throw; }
            catch (CiresonApiException) { throw; }
        }

        /// <summary>
        /// Performs an asynchronous GET and processes the result
        /// </summary>
        /// <param name="url">URL to GET from</param>
        /// <returns>Result message from the server</returns>
        /// <exception cref="System.Security.Authentication.InvalidCredentialException"></exception>
        /// <exception cref="CiresonPortalAPI.CiresonApiException"></exception>
        public async Task<string> GetAsync(string url)
        {
            try
            {
                using (HttpResponseMessage response = await _oHttpClient.GetAsync(url))
                {
                    return await ProcessResponse(response);
                }
            }
            // Rethrow expected exceptions, crash out for bugs
            catch (InvalidCredentialException) { throw; }
            catch (CiresonApiException) { throw; }
        }

        /// <summary>
        /// Handles processing a response from the server
        /// </summary>
        /// <param name="response">HttpResponseMessage from the server</param>
        /// <returns>Task&lt;string&gt;</returns>
        /// <exception cref="System.Security.Authentication.InvalidCredentialException">Thrown when an invalid username or password is supplied.</exception>
        /// <exception cref="CiresonPortalAPI.CiresonApiException">Thrown when an internal server error (HTTP 500) occurs.</exception>
        /// <exception cref="System.Net.Http.HttpRequestException">Thrown when any other HTTP exception occurs.</exception>
        private async Task<string> ProcessResponse(HttpResponseMessage response)
        {
            string result = string.Empty;

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new InvalidCredentialException("Invalid username or password.");
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                // Get the error message from the server
                result = await response.Content.ReadAsStringAsync();
                throw new CiresonApiException(result);
            }
            else
            {
                // Other unhandled errors
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException)
                {
                    // Rethrow exception
                    throw;
                }
            }

            return result;
        }

        /// <summary>
        /// Cleanup destructor
        /// </summary>
        ~PortalHttpHelper()
        {
            if (_oHttpClient != null)
                _oHttpClient.Dispose();
        }
    }
}
