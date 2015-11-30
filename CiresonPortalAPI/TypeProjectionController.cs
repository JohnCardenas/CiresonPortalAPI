using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CiresonPortalAPI
{
    public static class TypeProjectionController
    {
        const string GET_BY_CRITERIA_ENDPOINT = "/api/V3/Projection/GetProjectionByCriteria";
        const string CREATE_PROJECTION_BY_TEMPLATE_ENDPOINT = "/api/V3/Projection/CreateProjectionByTemplate";

        /// <summary>
        /// Creates an object projection from the specified template, by the specified user.
        /// </summary>
        /// <typeparam name="T">Type of projection to create</typeparam>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="templateId">ID of the object template to project</param>
        /// <param name="creatingUserId">ID of the user creating the object</param>
        /// <returns></returns>
        public static async Task<T> CreateProjectionByTemplate<T>(AuthorizationToken authToken, Guid templateId, Guid creatingUserId) where T : TypeProjection
        {
            if (!authToken.IsValid)
            {
                throw new InvalidCredentialException("AuthorizationToken is not valid.");
            }

            string endpointUrl = CREATE_PROJECTION_BY_TEMPLATE_ENDPOINT + "/" + templateId.ToString("D") + "?createdById=" + creatingUserId.ToString("D");

            try
            {
                // Initialize the HTTP helper and get going
                PortalHttpHelper helper = new PortalHttpHelper(authToken);
                string result = await helper.GetAsync(endpointUrl);

                // Convert the result into a TypeProjection and return it
                ExpandoObjectConverter converter = new ExpandoObjectConverter();
                dynamic jsonObject = JsonConvert.DeserializeObject<ExpandoObject>(result, converter);

                return (T)Activator.CreateInstance(typeof(T), new object[] { jsonObject, false });
            }
            catch (Exception e)
            {
                throw; // Rethrow exceptions
            }
        }

        /// <summary>
        /// Queries the Cireson Portal for objects using specified criteria.
        /// </summary>
        /// <param name="authToken">AuthenticationToken to use</param>
        /// <param name="criteria">QueryCriteria rules</param>
        /// <returns>List of ExpandoObjects</returns>
        public static async Task<List<T>> GetProjectionByCriteria<T>(AuthorizationToken authToken, QueryCriteria criteria) where T : TypeProjection
        {
            if (!authToken.IsValid)
            {
                throw new InvalidCredentialException("AuthorizationToken is not valid.");
            }

            try
            {
                // Initialize the HTTP helper and get going
                PortalHttpHelper helper = new PortalHttpHelper(authToken);
                string result = await helper.PostAsync(GET_BY_CRITERIA_ENDPOINT, criteria.ToString());

                // TypeProjections have no set properties, so we deserialize to a list of ExpandoObjects
                ExpandoObjectConverter converter = new ExpandoObjectConverter();
                dynamic objectList = JsonConvert.DeserializeObject<List<ExpandoObject>>(result, converter);

                // Convert the ExpandoObjects into proper TypeProjection objects
                List<T> returnList = new List<T>();
                foreach (ExpandoObject obj in objectList)
                {
                    // Instantiate and add to the list
                    T instanceType = Activator.CreateInstance<T>();

                    instanceType.OriginalObject = obj;
                    instanceType.CurrentObject = obj;
                    instanceType.ReadOnly = false;

                    returnList.Add(instanceType);
                }

                return returnList;
            }
            catch (Exception e)
            {
                throw; // Rethrow exceptions
            }
        }
    }
}
