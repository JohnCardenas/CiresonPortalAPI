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
        public PortalHttpHelper(string portalUrl, PortalCredentials credentials)
        {
            // Create the network credential cache
            CredentialCache cache = new CredentialCache();
            cache.Add(new Uri(portalUrl), "Negotiate", new NetworkCredential(credentials.Username, credentials.SecurePassword, credentials.Domain));

            // Create the HttpClientHandler
            HttpClientHandler handler = new HttpClientHandler();
            handler.Credentials = cache;

            // Create the HTTP client
            _oHttpClient = new HttpClient(handler);
            _oHttpClient.BaseAddress = new Uri(portalUrl);
            _oHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Overloaded constructor with an Authorization header; suitable for connecting to endpoints other than /Authorization/GetToken
        /// </summary>
        /// <param name="portalUrl">URL of the Cireson Portal</param>
        /// <param name="authToken">AuthorizationToken object</param>
        public PortalHttpHelper(string portalUrl, AuthorizationToken authToken) : this(portalUrl, authToken.Credentials)
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
        public async Task<string> PostAsync(string url, string payload)
        {
            try
            {
                using (HttpResponseMessage response = await _oHttpClient.PostAsync(url, new StringContent(payload)))
                {
                    return await ProcessResponse(response);
                }   
            }
            // Rethrow expected exceptions, crash out for bugs
            catch (InvalidCredentialException e) { throw; }
            catch (CiresonApiException e) { throw; }
        }

        /// <summary>
        /// Performs an asynchronous GET and processes the result
        /// </summary>
        /// <param name="url">URL to GET from</param>
        /// <returns>Result message from the server</returns>
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
            catch (InvalidCredentialException e) { throw; }
            catch (CiresonApiException e) { throw; }
        }

        /// <summary>
        /// Handles processing a response from the server
        /// </summary>
        /// <param name="response">HttpResponseMessage from the server</param>
        /// <returns>Task&lt;string&gt;</returns>
        private async Task<string> ProcessResponse(HttpResponseMessage response)
        {
            string result = string.Empty;

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
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
                catch (Exception e)
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
