using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection;
using System.Globalization;

namespace CiresonPortalAPI
{
    public static partial class TypeProjectionController
    {
        const string GET_BY_CRITERIA_ENDPOINT               = "/api/V3/Projection/GetProjectionByCriteria";
        const string CREATE_PROJECTION_BY_TEMPLATE_ENDPOINT = "/api/V3/Projection/CreateProjectionByTemplate";

        /// <summary>
        /// Creates an object projection from the specified template, by the specified user.
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="templateId">ID of the object template to project</param>
        /// <param name="creatingUserId">ID of the user creating the object</param>
        /// <returns></returns>
        public static async Task<TypeProjection> CreateProjectionByTemplate(AuthorizationToken authToken, Guid templateId, Guid creatingUserId)
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

                return new TypeProjection(jsonObject, false);
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
        public static async Task<List<TypeProjection>> GetProjectionByCriteria(AuthorizationToken authToken, QueryCriteria criteria)
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
                List<TypeProjection> returnList = new List<TypeProjection>();
                foreach (var obj in objectList)
                {
                    returnList.Add(new TypeProjection(obj, true));
                }

                return returnList;
            }
            catch (Exception e)
            {
                throw; // Rethrow exceptions
            }
        }

        /// <summary>
        /// Helper method that queries for TypeProjections using the specified criteria, then converts them to desired type T
        /// </summary>
        /// <typeparam name="T">CiresonPortalAPI object type</typeparam>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="criteria">QueryCriteria rules</param>
        /// <returns>List of specific CiresonPortalAPI objects</returns>
        internal static async Task<List<T>> GenericToSpecific<T>(AuthorizationToken authToken, QueryCriteria criteria)
        {
            if (!authToken.IsValid)
            {
                throw new InvalidCredentialException("AuthorizationToken is not valid.");
            }

            List<T> returnList = new List<T>();

            List<TypeProjection> projectionList = await TypeProjectionController.GetProjectionByCriteria(authToken, criteria);
            foreach (TypeProjection projection in projectionList)
            {
                // Use binding flags to find internal constructors
                BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
                CultureInfo culture = null;

                // Build parameters list
                object[] parameters = new object[1];
                parameters[0] = projection;

                // Instantiate and add to the list
                var instanceType = (T)Activator.CreateInstance(typeof(T), flags, null, parameters, culture);
                returnList.Add(instanceType);
            }

            return returnList;
        }
    }

    /// <summary>
    /// TypeProjection
    /// </summary>
    [JsonConverter(typeof(TypeProjectionSerializer))]
    public class TypeProjection
    {
        const string COMMIT_ENDPOINT = "/api/V3/Projection/Commit";

        protected internal bool _bDirtyObject = false;
        protected internal bool _bReadOnly = true;
        protected internal dynamic _oOriginalObject = null;
        protected internal dynamic _oCurrentObject = null;

        /// <summary>
        /// Called when a property changes to set the dirty object flag
        /// </summary>
        protected void SetDirtyBit()
        {
            _bDirtyObject = true;
        }

        /// <summary>
        /// Attempts to commit the type projection to the portal. Throws an exception if not successful.
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        public async Task<bool> Commit(AuthorizationToken authToken)
        {
            if (_bReadOnly)
                throw new CiresonApiException("Cannot commit a read-only type projection.");

            if (!authToken.IsValid)
                throw new InvalidCredentialException("AuthorizationToken is not valid.");

            if (!_bDirtyObject)
                throw new CiresonApiException("Object is not dirty, Commit() aborted.");

            try
            {
                string jsonObj = this.Serialize();

                // Initialize the HTTP helper and get going
                PortalHttpHelper helper = new PortalHttpHelper(authToken);
                string result = await helper.PostAsync(COMMIT_ENDPOINT, jsonObj);

                // Retrieve the result
                ExpandoObjectConverter converter = new ExpandoObjectConverter();
                dynamic resultObj = JsonConvert.DeserializeObject<ExpandoObject>(result, converter);

                // Throw an exception if we didn't succeed
                if (!resultObj.success)
                    throw new CiresonApiException(resultObj.exception);
            }
            catch (Exception e)
            {
                throw; // Rethrow exceptions
            }

            return true;
        }

        /// <summary>
        /// Creates a new type projection
        /// </summary>
        /// <param name="obj">JSON object</param>
        /// <param name="existingObject">Is this an existing object?</param>
        internal TypeProjection(dynamic obj, bool existingObject = false)
        {
            _oCurrentObject = obj;

            if (existingObject)
                _oOriginalObject = obj;
        }

        /// <summary>
        /// Creates a blank type projection for a new object
        /// </summary>
        internal TypeProjection()
        {
            _oCurrentObject = new ExpandoObject();
        }

        /// <summary>
        /// Converts this TypeProjection to a JSON string representation
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Deserializes an enumeration from the specified strings
        /// </summary>
        /// <param name="id">ID of the enumeration</param>
        /// <param name="name">Name of the enumeration</param>
        /// <returns></returns>
        protected Enumeration DeserializeEnumeration(string id, string name)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            return new Enumeration(new Guid(id), name, name, true, false);
        }

        /// <summary>
        /// Deserializes an enumeration from the specified Guid and string
        /// </summary>
        /// <param name="id">ID of the enumeration</param>
        /// <param name="name">Name of the enumeration</param>
        /// <returns></returns>
        protected Enumeration DeserializeEnumeration(Guid id, string name)
        {
            if (id == null)
                return null;

            return new Enumeration(id, name, name, true, false);
        }

        /// <summary>
        /// Sets an underlying data model enumeration if the Enumeration object's ID is not empty
        /// </summary>
        /// <param name="objectEnum">Object enumeration to set</param>
        /// <param name="enumValue">Enumeration object to set as the value</param>
        protected void SetEnumerationValue(dynamic objectEnum, Enumeration enumValue)
        {
            if (enumValue.Id == Guid.Empty)
            {
                objectEnum.Id = null;
                objectEnum.Name = string.Empty;
            }
            else
            {
                objectEnum.Id = enumValue.Id;
                objectEnum.Name = enumValue.Name;
            }
        }
    }

    /// <summary>
    /// Serializer class for TypeProjections
    /// </summary>
    public class TypeProjectionSerializer : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            TypeProjection projection = (TypeProjection)value;

            writer.WriteStartObject();

            #region formJson object
            {
                writer.WritePropertyName("formJson");
                writer.WriteStartObject();

                writer.WritePropertyName("isDirty");
                writer.WriteValue(projection._bDirtyObject);

                //"current" object
                writer.WritePropertyName("current");
                writer.WriteRawValue(JsonConvert.SerializeObject(projection._oCurrentObject));
                
                //"original" object
                writer.WritePropertyName("original");
                writer.WriteRawValue(JsonConvert.SerializeObject(projection._oOriginalObject));

                writer.WriteEndObject(); // formJson
            }
            #endregion formJsonObject

            writer.WriteEndObject(); // JSON
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}
