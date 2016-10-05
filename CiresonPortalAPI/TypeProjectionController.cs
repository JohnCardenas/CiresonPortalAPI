using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CiresonPortalAPI
{
    internal static class TypeProjectionController
    {
        #region Constants
        const string GET_BY_CRITERIA_ENDPOINT               = "/api/V3/Projection/GetProjectionByCriteria";
        const string CREATE_PROJECTION_BY_TEMPLATE_ENDPOINT = "/api/V3/Projection/CreateProjectionByTemplate";
        const string CREATE_PROJECTION_BY_DATA_ENDPOINT     = "/api/V3/Projection/Commit";
        #endregion // Constants

        #region Internal Methods
        /// <summary>
        /// Creates an object projection from the specified template, by the specified user.
        /// </summary>
        /// <typeparam name="T">Type of projection to create</typeparam>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="templateId">ID of the object template to project</param>
        /// <param name="creatingUserId">ID of the user creating the object</param>
        /// <returns></returns>
        internal static async Task<T> CreateObjectFromTemplate<T>(AuthorizationToken authToken, Guid templateId, Guid creatingUserId) where T : TypeProjection
        {
            if (!authToken.IsValid)
            {
                throw new InvalidCredentialException("AuthorizationToken is not valid.");
            }

            string endpointUrl = CREATE_PROJECTION_BY_TEMPLATE_ENDPOINT + "?id=" + templateId.ToString("D") + "&createdById=" + creatingUserId.ToString("D");

            try
            {
                // Initialize the HTTP helper and get going
                PortalHttpHelper helper = new PortalHttpHelper(authToken);
                string result = await helper.GetAsync(endpointUrl);

                // Convert the result into a TypeProjection and return it
                ExpandoObjectConverter converter = new ExpandoObjectConverter();
                dynamic jsonObject = JsonConvert.DeserializeObject<ExpandoObject>(result, converter);

                // Instantiate!
                BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                CultureInfo culture = null;
                T instanceType = (T)Activator.CreateInstance(typeof(T), flags, null, null, culture);

                instanceType.CurrentObject = (jsonObject as ExpandoObject).DeepCopy();
                instanceType.OriginalObject = jsonObject;
                instanceType.ReadOnly = false;

                return instanceType;
            }
            catch (Exception)
            {
                throw; // Rethrow exceptions
            }
        }

        /// <summary>
        /// Creates a new TypeProjection of derived type T and returns it
        /// </summary>
        /// <typeparam name="T">TypeProjection derived type</typeparam>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <returns></returns>
        internal static async Task<T> CreateBlankObject<T>(AuthorizationToken authToken, string name, string displayName, dynamic objProps = null) where T : TypeProjection
        {
            // See if we have a CI matching this name first
            T item = await GetByFullName<T>(authToken, name);

            if (item != null)
            {
                string fullName = ClassConstants.GetClassNameByType<T>() + ":" + name;
                throw new CiresonDuplicateItemException("An object by the name " + fullName + " already exists.");
            }

            // Setup the CI
            dynamic ci = new ExpandoObject();

            ci.formJson = new ExpandoObject();
            ci.formJson.isDirty = true;
            ci.formJson.original = null;

            ci.formJson.current = new ExpandoObject();
            ci.formJson.current.BaseId = null;
            ci.formJson.current.ClassTypeId = ClassConstants.GetClassIdByType<T>();
            ci.formJson.current.ClassName = ClassConstants.GetClassNameByType<T>();
            ci.formJson.current.Name = name;
            ci.formJson.current.DisplayName = displayName;
            ci.formJson.current.TimeAdded = "0001-01-01T00:00:00";

            //ci.formJson.current.ObjectStatus = new ExpandoObject();
            //ci.formJson.current.ObjectStatus.Id = EnumerationConstants.TypeProjection.BuiltinValues.ObjectStatus.Active;

            // Merge another property object in
            if (objProps != null)
            {
                IDictionary<string, object> ciDict = (IDictionary<string, object>)ci.formJson.current;

                foreach (var property in objProps.GetType().GetProperties())
                {
                    if (property.CanRead)
                    {
                        ciDict[property.Name] = property.GetValue(objProps);
                    }
                }
            }

            // Create the new TypeProjection, then return the full-property object
            T newCI = await CreateObjectFromData<T>(authToken, ci);
            return await GetByBaseId<T>(authToken, newCI.BaseId);
        }

        /// <summary>
        /// Queries the Cireson Portal for objects using specified criteria.
        /// </summary>
        /// <param name="authToken">AuthenticationToken to use</param>
        /// <param name="criteria">QueryCriteria rules</param>
        /// <returns>List of TypeProjections</returns>
        internal static async Task<List<T>> GetByCriteria<T>(AuthorizationToken authToken, QueryCriteria criteria) where T : TypeProjection
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
                    BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                    CultureInfo culture = null;
                    T instanceType = (T)Activator.CreateInstance(typeof(T), flags, null, null, culture);

                    instanceType.CurrentObject = obj.DeepCopy();
                    instanceType.OriginalObject = obj;
                    instanceType.ReadOnly = false;

                    returnList.Add(instanceType);
                }

                return returnList;
            }
            catch (Exception)
            {
                throw; // Rethrow exceptions
            }
        }

        /// <summary>
        /// Convenience method to retrieve an object derived from the TypeProjection class, using its BaseId.
        /// </summary>
        /// <typeparam name="T">TypeProjection derived type</typeparam>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="baseId">BaseId to find</param>
        /// <returns></returns>
        internal static async Task<T> GetByBaseId<T>(AuthorizationToken authToken, Guid baseId) where T : TypeProjection
        {
            QueryCriteriaExpression expr = new QueryCriteriaExpression
            {
                PropertyName = "Id",
                PropertyType = QueryCriteriaPropertyType.GenericProperty,
                Operator = QueryCriteriaExpressionOperator.Equal,
                Value = baseId.ToString("D")
            };

            QueryCriteria criteria = new QueryCriteria(TypeProjectionConstants.GetProjectionIdByType<T>())
            {
                GroupingOperator = QueryCriteriaGroupingOperator.SimpleExpression
            };

            criteria.Expressions.Add(expr);

            List<T> retList = await GetByCriteria<T>(authToken, criteria);

            if (retList.Count == 0)
                return null;
            else if (retList.Count == 1)
                return retList[0];
            else
                throw new CiresonApiException("More than one item found with identical BaseId");
        }

        /// <summary>
        /// Convenience method to retrieve an object derived from the TypeProjection class, using its FullName
        /// </summary>
        /// <typeparam name="T">TypeProjection derived type</typeparam>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="name">FullName to find</param>
        /// <returns></returns>
        internal static async Task<T> GetByFullName<T>(AuthorizationToken authToken, string name) where T : TypeProjection
        {
            QueryCriteriaExpression expr = new QueryCriteriaExpression
            {
                PropertyName = "FullName",
                PropertyType = QueryCriteriaPropertyType.GenericProperty,
                Operator = QueryCriteriaExpressionOperator.Equal,
                Value = ClassConstants.GetClassNameByType<T>() + ":" + name
            };

            QueryCriteria criteria = new QueryCriteria(TypeProjectionConstants.GetProjectionIdByType<T>())
            {
                GroupingOperator = QueryCriteriaGroupingOperator.SimpleExpression
            };

            criteria.Expressions.Add(expr);

            List<T> retList = await GetByCriteria<T>(authToken, criteria);

            if (retList.Count == 0)
                return null;
            else if (retList.Count == 1)
                return retList[0];
            else
                throw new CiresonApiException("More than one item found with identical full name");
        }
        #endregion // Internal Methods

        #region Private Methods
        /// <summary>
        /// Modifies a given QueryCriteria to exclude inactive items
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <param name="criteria">QueryCriteria to adjust</param>
        /// <returns></returns>
        private static QueryCriteria ExcludeInactiveItems<T>(QueryCriteria criteria) where T : TypeProjection
        {
            PropertyPathHelper pathHelper = new PropertyPathHelper();
            pathHelper.PropertyName = "ObjectStatus";
            pathHelper.ObjectClass = ClassConstants.GetClassIdByType<T>();

            QueryCriteriaExpression activeItemsOnly = new QueryCriteriaExpression
            {
                PropertyName = pathHelper.ToString(),
                PropertyType = QueryCriteriaPropertyType.Property,
                Operator = QueryCriteriaExpressionOperator.Equal,
                Value = EnumerationConstants.ConfigItem.BuiltinValues.ObjectStatus.Active.ToString("D")
            };

            QueryCriteria newCriteria = criteria;

            if (newCriteria.Expressions.Count > 0)
                newCriteria.GroupingOperator = QueryCriteriaGroupingOperator.And;
            else
                newCriteria.GroupingOperator = QueryCriteriaGroupingOperator.SimpleExpression;

            newCriteria.Expressions.Add(activeItemsOnly);

            return newCriteria;
        }

        /// <summary>
        /// Creates an object projection using the specified data.
        /// </summary>
        /// <typeparam name="T">Type of projection to create</typeparam>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="data">Data object to specify for creation</param>
        /// <param name="readOnly">Should this projection be read only?</param>
        /// <returns></returns>
        private static async Task<T> CreateObjectFromData<T>(AuthorizationToken authToken, dynamic data, bool readOnly = false) where T : TypeProjection
        {
            if (!authToken.IsValid)
            {
                throw new InvalidCredentialException("AuthorizationToken is not valid.");
            }

            try
            {
                string jsonObj = JsonConvert.SerializeObject(data);

                // Initialize the HTTP helper and get going
                PortalHttpHelper helper = new PortalHttpHelper(authToken);
                string result = await helper.PostAsync(CREATE_PROJECTION_BY_DATA_ENDPOINT, jsonObj);

                // Retrieve the result
                ExpandoObjectConverter converter = new ExpandoObjectConverter();
                dynamic resultObj = JsonConvert.DeserializeObject<ExpandoObject>(result, converter);

                // Throw an exception if we didn't succeed
                if (!resultObj.success)
                    throw new CiresonApiException(resultObj.exception);

                // Fetch the BaseId of the new object and create the projection
                data.formJson.current.BaseId = new Guid(resultObj.BaseId);

                // Instantiate
                BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                CultureInfo culture = null;
                T instanceType = (T)Activator.CreateInstance(typeof(T), flags, null, null, culture);

                instanceType.CurrentObject = (data.formJson.current as ExpandoObject).DeepCopy();
                instanceType.OriginalObject = data.formJson.current;
                instanceType.ReadOnly = readOnly;

                return instanceType;
            }
            catch (Exception)
            {
                throw; // Rethrow exceptions
            }
        }
        #endregion // Private Methods
    }
}
