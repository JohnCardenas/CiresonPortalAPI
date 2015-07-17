using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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

                return new TypeProjection(jsonObject);
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
                    returnList.Add(new TypeProjection(obj));
                }

                return returnList;
            }
            catch (Exception e)
            {
                throw; // Rethrow exceptions
            }
        }
    }

    /// <summary>
    /// TypeProjection
    /// </summary>
    [JsonConverter(typeof(TypeProjectionSerializer))]
    public class TypeProjection
    {
        protected internal bool _bDirtyObject = false;
        protected internal dynamic _oOriginalObject;
        protected internal dynamic _oCurrentObject;

        /// <summary>
        /// Called when a property changes to set the dirty object flag
        /// </summary>
        protected void SetDirtyBit()
        {
            _bDirtyObject = true;
        }

        /// <summary>
        /// Attempts to update the type projection
        /// </summary>
        /// <returns>Returns true if successful, false if not.</returns>
        public async Task<bool> Update()
        {
            if (_bDirtyObject == false)
                return false;

            return false;
            //return await WriteObject();
        }

        /// <summary>
        /// Creates a new type projection object based on an existing object
        /// </summary>
        /// <param name="obj"></param>
        public TypeProjection(dynamic obj)
        {
            _oCurrentObject = obj;
            _oOriginalObject = obj;
        }

        /// <summary>
        /// Creates a new type projection for a new object
        /// </summary>
        public TypeProjection()
        {
            _oCurrentObject = new ExpandoObject();
            _oOriginalObject = null;
        }

        protected Enumeration DeserializeEnumeration(string id, string name)
        {
            return new Enumeration(id, name, name, true, false);
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
                writer.WriteValue(JsonConvert.SerializeObject(projection._oCurrentObject));
                
                //"original" object
                writer.WritePropertyName("original");
                writer.WriteValue(JsonConvert.SerializeObject(projection._oOriginalObject));

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
