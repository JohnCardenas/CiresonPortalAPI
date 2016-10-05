using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
    public abstract class TypeProjection : INotifyPropertyChanged, IEquatable<TypeProjection>
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
        /// <summary>
        /// Data layer for the original object
        /// </summary>
        internal dynamic OriginalObject
        {
            get { return _oOriginalObject; }
            set { _oOriginalObject = value; }
        }

        /// <summary>
        /// Data layer for modifications to this object
        /// </summary>
        internal dynamic CurrentObject
        {
            get { return _oCurrentObject; }
            set { _oCurrentObject = value; }
        }

        /// <summary>
        /// If true, this object has changes uncommitted.
        /// </summary>
        public bool IsDirty
        {
            get { return _bDirtyObject; }
            internal set { _bDirtyObject = value; }
        }

        /// <summary>
        /// If true, this object is read only.
        /// </summary>
        public bool ReadOnly
        {
            get { return _bReadOnly; }
            internal set { _bReadOnly = value; }
        }

        /// <summary>
        /// Gets the DisplayName of the TypeProjection. Read only.
        /// </summary>
        public string DisplayName
        {
            get { return this.GetPrimitiveValue<string>("DisplayName"); }
            internal set { this.SetPrimitiveValue<string>("DisplayName", value); }
        }

        /// <summary>
        /// Returns this object's BaseId. Read only.
        /// </summary>
        public Guid BaseId
        {
            get { return this.GetPrimitiveValue<Guid>("BaseId"); }
        }

        /// <summary>
        /// Returns this object's FullName. Read only.
        /// </summary>
        public string FullName
        {
            get { return this.GetPrimitiveValue<string>("FullName"); }
        }

        /// <summary>
        /// Returns the class name of this object. Read only.
        /// </summary>
        public string ClassName
        {
            get { return this.GetPrimitiveValue<string>("ClassName"); }
        }

        /// <summary>
        /// Returns the default DisplayName that should be set for this object. This should be overriden by derived classes.
        /// </summary>
        public virtual string DefaultDisplayName
        {
            get
            {
                return this.FullName;
            }
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
            this.CurrentObject = obj.DeepCopy();

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
            this.CurrentObject = (otherProjection.CurrentObject as ExpandoObject).DeepCopy();
            this.OriginalObject = otherProjection.OriginalObject;
            this.IsDirty = otherProjection.IsDirty;
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

        #region Operator Overloads
        /// <summary>
        /// Overload equality check operator
        /// </summary>
        /// <param name="a">First operand to check for equality</param>
        /// <param name="b">Second operand to check for equality</param>
        /// <returns></returns>
        public static bool operator ==(TypeProjection a, TypeProjection b)
        {
            return Equals(a, b);
        }

        /// <summary>
        /// Overload non-equality check operator
        /// </summary>
        /// <param name="a">First operand to check for non-equality</param>
        /// <param name="b">Second operand to check for non-equality</param>
        /// <returns></returns>
        public static bool operator !=(TypeProjection a, TypeProjection b)
        {
            return !Equals(a, b);
        }
        #endregion // Operator Overloads

        #region Public Methods
        /// <summary>
        /// Equality check method
        /// </summary>
        /// <param name="other">Other TypeProjection to check for equality</param>
        /// <returns></returns>
        public bool Equals(TypeProjection other)
        {
            return Equals(this, other);
        }

        /// <summary>
        /// Equality check method
        /// </summary>
        /// <param name="obj">Other object to check for equality</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as TypeProjection);
        }

        /// <summary>
        /// Equality check method
        /// </summary>
        /// <param name="a">First object to check for equality</param>
        /// <param name="b">Second object to check for equality</param>
        /// <returns></returns>
        public static bool Equals(TypeProjection a, TypeProjection b)
        {
            // If both or null or the same instance, return true
            if (System.Object.ReferenceEquals(a, b))
                return true;

            // If one is null but not both, return false
            if (((object)a == null) || ((object)b == null))
                return false;

            // Return true if the IDs match
            if (a.BaseId == b.BaseId)
                return true;

            // Return true if the FullNames match
            if (a.FullName == b.FullName)
                return true;

            return false;
        }

        /// <summary>
        /// Hash code method
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.BaseId.GetHashCode();
        }

        /// <summary>
        /// Attempts to commit the type projection to the portal. Throws an exception if not successful.
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        public async virtual Task<bool> Commit(AuthorizationToken authToken)
        {
            if (this.ReadOnly)
                throw new CiresonApiException("Cannot commit a read-only type projection.");

            if (!authToken.IsValid)
                throw new InvalidCredentialException("AuthorizationToken is not valid.");

            if (!this.IsDirty)
                throw new CiresonApiException("Object is not dirty, Commit() aborted.");

            // Every object needs a DisplayName set
            if (string.IsNullOrEmpty(this.DisplayName))
                this.DisplayName = this.DefaultDisplayName;

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

                // Refresh object data from the server
                await this.Refresh(authToken);
            }
            catch (Exception)
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
        /// Refreshes an object, discarding any changes that may have been made.
        /// This method must be called before accessing properties of children in relationship collections in order to populate all properties.
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <returns></returns>
        public abstract Task<bool> Refresh(AuthorizationToken authToken);

        /// <summary>
        /// Returns the object's DisplayName.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.DisplayName;
        }
        #endregion // Public Methods

        #region Protected Methods
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
        /// <param name="objectProperty">Derived object's property name, if it is different than the data model property name. Note that this will automatically be set to the caller's member name.</param>
        protected void SetEnumeration(string modelProperty, Enumeration value, [CallerMemberName] string objectProperty = null)
        {
            if (this.ReadOnly)
                throw new CiresonReadOnlyException("Cannot set enumeration; this object is read-only.");

            var objectData = (IDictionary<string, object>)this.CurrentObject;

            // Add a new Expando if the model property doesn't exist
            if (!DynamicObjectHelpers.HasProperty(this.CurrentObject, modelProperty))
            {
                objectData.Add(modelProperty, new ExpandoObject());
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
            this.IsDirty = true;

            if (String.IsNullOrEmpty(objectProperty))
                NotifyPropertyChanged(modelProperty);
            else
                NotifyPropertyChanged(objectProperty);
        }

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

                BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                CultureInfo culture = null;
                T relObj = (T)Activator.CreateInstance(typeof(T), flags, null, null, culture);

                relObj.CurrentObject = (ExpandoObject)objectData[modelProperty];
                relObj.ReadOnly = true;

                return relObj;
            }

            return null;
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
                throw new CiresonReadOnlyException("Cannot set relationship; object is read-only.");

            var objectData = (IDictionary<string, object>)this.CurrentObject;

            if (obj == null)
                objectData[modelProperty] = null;
            else
                objectData[modelProperty] = obj.CurrentObject;

            //this.CurrentObject = (ExpandoObject)objectData;
            this.IsDirty = true;

            if (String.IsNullOrEmpty(objectProperty))
                NotifyPropertyChanged(modelProperty);
            else
                NotifyPropertyChanged(objectProperty);
        }

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

            if (value == null)
                objectData[modelProperty] = null;
            else
                objectData[modelProperty] = value.ToString();

            //this.CurrentObject = (ExpandoObject)objectData;
            this.IsDirty = true;

            if (String.IsNullOrEmpty(objectProperty))
                NotifyPropertyChanged(modelProperty);
            else
                NotifyPropertyChanged(objectProperty);
        }

        /// <summary>
        /// Refreshes a TypeProjection from the database
        /// </summary>
        /// <typeparam name="T">TypeProjection derived type to refresh</typeparam>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <returns></returns>
        protected async Task<bool> RefreshType<T>(AuthorizationToken authToken) where T : TypeProjection
        {
            TypeProjection p = await TypeProjectionController.GetByBaseId<T>(authToken, this.BaseId);

            if (p == null)
                return false;

            this.CurrentObject = (p.CurrentObject as ExpandoObject).DeepCopy();
            this.OriginalObject = p.OriginalObject;
            this.ReadOnly = p.ReadOnly;
            this.IsDirty = false;

            return true;
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
        #endregion // Protected Methods
    }

    /// <summary>
    /// TypeProjectionComparer, uses object FullNames
    /// </summary>
    public class TypeProjectionComparer : IComparer<TypeProjection>
    {
        public int Compare(TypeProjection a, TypeProjection b)
        {
            return string.Compare(a.ToString(), b.ToString());
        }
    }

    /// <summary>
    /// Serializer class for TypeProjections
    /// </summary>
    public class TypeProjectionSerializer : JsonConverter
    {
        /// <summary>
        /// Serializes a TypeProjection to a JSON string
        /// </summary>
        /// <param name="writer">JsonWriter to use</param>
        /// <param name="value">Object to convert</param>
        /// <param name="serializer">JsonSerializer to use</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            TypeProjection projection = (TypeProjection)value;

            writer.WriteStartObject();

            #region formJson object
            {
                writer.WritePropertyName("formJson");
                writer.WriteStartObject();

                writer.WritePropertyName("isDirty");
                writer.WriteValue(projection.IsDirty);

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
