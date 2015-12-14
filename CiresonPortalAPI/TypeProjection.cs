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
    /// <summary>
    /// TypeProjection
    /// </summary>
    [JsonConverter(typeof(TypeProjectionSerializer))]
    public abstract class TypeProjection : INotifyPropertyChanged
    {
        #region Constants
        const string COMMIT_ENDPOINT = "/api/V3/Projection/Commit";
        #endregion // Constants

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion // Events

        #region Fields
        private bool _bDirtyObject = false;
        private bool _bReadOnly = true;
        private dynamic _oOriginalObject = null;
        private dynamic _oCurrentObject = null;
        #endregion // Fields

        #region Properties
        internal dynamic OriginalObject
        {
            get { return _oOriginalObject; }
            set { _oOriginalObject = value; }
        }

        internal dynamic CurrentObject
        {
            get { return _oCurrentObject; }
            set { _oCurrentObject = value; }
        }

        public bool DirtyObject
        {
            get { return _bDirtyObject; }
            set { _bDirtyObject = value; }
        }

        public bool ReadOnly
        {
            get { return _bReadOnly; }
            set { _bReadOnly = value; }
        }
        #endregion // Properties

        #region Constructors
        /// <summary>
        /// Creates a new type projection
        /// </summary>
        /// <param name="obj">JSON object</param>
        /// <param name="existingObject">Is this an existing object?</param>
        /// <param name="readOnly">Should this object be read only?</param>
        internal TypeProjection(ExpandoObject obj, bool existingObject = false, bool readOnly = true)
        {
            this.CurrentObject = obj;

            if (existingObject)
                this.OriginalObject = obj;

            this.ReadOnly = readOnly;
        }

        /// <summary>
        /// Creates a copy of a TypeProjection from another TypeProjection
        /// </summary>
        /// <param name="otherProjection"></param>
        internal TypeProjection(TypeProjection otherProjection)
        {
            this.CurrentObject = otherProjection.CurrentObject;
            this.OriginalObject = otherProjection.OriginalObject;
            this.DirtyObject = otherProjection.DirtyObject;
            this.ReadOnly = otherProjection.ReadOnly;
        }

        /// <summary>
        /// Creates a blank type projection for a new object
        /// </summary>
        internal TypeProjection()
        {
            this.CurrentObject = new ExpandoObject();
        }
        #endregion // Constructors

        #region General Methods
        /// <summary>
        /// Attempts to commit the type projection to the portal. Throws an exception if not successful.
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        public async Task<bool> Commit(AuthorizationToken authToken)
        {
            if (this.ReadOnly)
                throw new CiresonApiException("Cannot commit a read-only type projection.");

            if (!authToken.IsValid)
                throw new InvalidCredentialException("AuthorizationToken is not valid.");

            if (!this.DirtyObject)
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
        /// Converts this TypeProjection to a JSON string representation
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Emits a property changed event
        /// </summary>
        /// <param name="propertyName">Name of the property to notify listeners</param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion // General Methods

        #region Enumeration Methods
        /// <summary>
        /// Gets an enumeration from the underlying data model.
        /// </summary>
        /// <param name="modelProperty">Enumeration's data model property name</param>
        /// <returns></returns>
        protected Enumeration GetEnumeration(string modelProperty)
        {
            if (DynamicObjectHelpers.HasProperty(this.CurrentObject, modelProperty))
            {
                var objectData = (IDictionary<string,object>)this.CurrentObject;
                dynamic rawEnum = objectData[modelProperty];

                if (rawEnum.Id == null)
                    return null;

                return new Enumeration(rawEnum.Id, rawEnum.Name, rawEnum.Name, true, false);
            }
            else
                return null;
        }

        /// <summary>
        /// Gets an enumeration list from the underlying data model.
        /// </summary>
        /// <param name="modelProperty">Enumeration list's data model property name</param>
        /// <returns></returns>
        protected List<Enumeration> GetEnumerationList(string modelProperty)
        {
            List<Enumeration> returnList = new List<Enumeration>();

            if (DynamicObjectHelpers.HasProperty(this.CurrentObject, modelProperty))
            {
                var objectData = (IDictionary<string, object>)this.CurrentObject;
                dynamic objectList = objectData[modelProperty];

                foreach (dynamic obj in objectList)
                {
                    if (obj.Id == null)
                        continue;

                    returnList.Add(new Enumeration(obj.Id, obj.Name, obj.Name, true, false));
                }

                return returnList;
            }
            else
                return null;
        }

        /// <summary>
        /// Sets an enumeration in the underlying data model.
        /// </summary>
        /// <param name="modelProperty">Enumeration's data model property name</param>
        /// <param name="value">New enumeration value</param>
        /// <param name="objectProperty">Derived object's property name, if it is different than the data model property name.</param>
        protected void SetEnumeration(string modelProperty, Enumeration value, string objectProperty = null)
        {
            if (this.ReadOnly)
                throw new CiresonReadOnlyException("Cannot set enumeration; this object is read-only.");

            var objectData = (IDictionary<string, object>)this.CurrentObject;

            // Add a null value in case it doesn't exist already
            if (!DynamicObjectHelpers.HasProperty(this.CurrentObject, modelProperty))
            {
                objectData.Add(modelProperty, null);
            }

            dynamic rawEnum = objectData[modelProperty];

            if (value == null)
            {
                rawEnum.Id = null;
                rawEnum.Name = string.Empty;
            }
            else if (value.Id == Guid.Empty)
            {
                rawEnum.Id = null;
                rawEnum.Name = string.Empty;
            }
            else
            {
                rawEnum.Id = value.Id;
                rawEnum.Name = value.Name;
            }

            this.CurrentObject = (ExpandoObject)objectData;
            this.DirtyObject = true;

            if (String.IsNullOrEmpty(objectProperty))
                NotifyPropertyChanged(modelProperty);
            else
                NotifyPropertyChanged(objectProperty);
        }

        #endregion // Enumeration Methods

        #region Relationship Methods
        /// <summary>
        /// Gets a related object based on a relationship.
        /// </summary>
        /// <typeparam name="T">Type of the related object</typeparam>
        /// <param name="modelProperty">Relationship's data model property name</param>
        /// <returns></returns>
        protected T GetRelatedObject<T>(string modelProperty) where T : TypeProjection
        {
            if (DynamicObjectHelpers.HasProperty(this.CurrentObject, modelProperty))
            {
                var objectData = (IDictionary<string, object>)this.CurrentObject;

                T relObj = Activator.CreateInstance<T>();

                relObj.CurrentObject = (ExpandoObject)objectData[modelProperty];
                relObj.ReadOnly = true;

                return relObj;
            }

            return null;
        }

        /// <summary>
        /// Gets a list of related objects based on a relationship.
        /// </summary>
        /// <typeparam name="T">Type of the related objects</typeparam>
        /// <param name="modelProperty">Relationship's data model property name</param>
        /// <returns></returns>
        protected List<T> GetRelatedObjectsList<T>(string modelProperty) where T : TypeProjection
        {
            List<T> memberList = new List<T>();

            if (DynamicObjectHelpers.HasProperty(this.CurrentObject, modelProperty))
            {
                var objectData = (IDictionary<string, object>)this.CurrentObject;
                dynamic objectList = objectData[modelProperty];

                foreach (dynamic obj in objectList)
                {
                    T relObj = Activator.CreateInstance<T>();

                    relObj.CurrentObject = obj;
                    relObj.ReadOnly = true;

                    memberList.Add(relObj);
                }
            }

            return memberList;
        }

        /// <summary>
        /// Sets a relationship to a list of TypeProjections
        /// </summary>
        /// <param name="modelProperty">Relationship's data model property name</param>
        /// <param name="objects">List of objects to set</param>
        /// <param name="objectProperty">Derived object's property name, if it is different than the data model property name.</param>
        protected void SetRelatedObjectsList(string modelProperty, List<TypeProjection> objects, string objectProperty = null)
        {
            if (this.ReadOnly)
                throw new CiresonReadOnlyException("Cannot set related objects; object is read-only.");

            var objectData = (IDictionary<string, object>)this.CurrentObject;
            var objectList = new dynamic[objects.Count];

            for (int i = 0; i < objects.Count; i++)
            {
                objectList[i] = objects[i].CurrentObject;
            }

            objectData[modelProperty] = objectList;
            this.CurrentObject = (ExpandoObject)objectData;
            this.DirtyObject = true;

            if (String.IsNullOrEmpty(objectProperty))
                NotifyPropertyChanged(modelProperty);
            else
                NotifyPropertyChanged(objectProperty);
        }

        /// <summary>
        /// Sets a relationship to the specified TypeProjection
        /// </summary>
        /// <param name="modelProperty">Relationship's data model property name</param>
        /// <param name="obj">Object to set</param>
        /// <param name="objectProperty">Derived object's property name, if it is different than the data model property name.</param>
        protected void SetRelatedObject(string modelProperty, TypeProjection obj, string objectProperty = null)
        {
            if (this.ReadOnly)
                throw new CiresonReadOnlyException("Cannot set related object; object is read-only.");

            var objectData = (IDictionary<string, object>)this.CurrentObject;
            objectData[modelProperty] = obj.CurrentObject;

            this.CurrentObject = (ExpandoObject)objectData;
            this.DirtyObject = true;

            if (String.IsNullOrEmpty(objectProperty))
                NotifyPropertyChanged(modelProperty);
            else
                NotifyPropertyChanged(objectProperty);
        }
        #endregion // Relationship Methods

        #region Primitive Methods
        /// <summary>
        /// Gets a primitive value by property name.
        /// </summary>
        /// <typeparam name="T">Primitive type</typeparam>
        /// <param name="modelProperty">Primitive's data model property name</param>
        /// <returns></returns>
        protected T GetPrimitiveValue<T>(string modelProperty)
        {
            if (DynamicObjectHelpers.HasProperty(this.CurrentObject, modelProperty))
            {
                return DynamicObjectHelpers.GetProperty<T>(this.CurrentObject, modelProperty);
            }

            return default(T);
        }

        /// <summary>
        /// Sets a primitive value by property name
        /// </summary>
        /// <typeparam name="T">Type of the primitive</typeparam>
        /// <param name="modelProperty">Primitive's data model property name name</param>
        /// <param name="value">Primitive value</param>
        /// <param name="objectProperty">Derived object's property name, if it is different than the data model property name.</param>
        protected void SetPrimitiveValue<T>(string modelProperty, T value, string objectProperty = null)
        {
            if (this.ReadOnly)
                throw new CiresonReadOnlyException("Cannot set primitive value; object is read-only.");

            var objectData = (IDictionary<string, object>)this.CurrentObject;
            objectData[modelProperty] = value.ToString();

            this.CurrentObject = (ExpandoObject)objectData;
            this.DirtyObject = true;

            if (String.IsNullOrEmpty(objectProperty))
                NotifyPropertyChanged(modelProperty);
            else
                NotifyPropertyChanged(objectProperty);
        }
        #endregion // Primitive Methods
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
                writer.WriteValue(projection.DirtyObject);

                //"current" object
                writer.WritePropertyName("current");
                writer.WriteRawValue(JsonConvert.SerializeObject(projection.CurrentObject));
                
                //"original" object
                writer.WritePropertyName("original");
                writer.WriteRawValue(JsonConvert.SerializeObject(projection.OriginalObject));

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
