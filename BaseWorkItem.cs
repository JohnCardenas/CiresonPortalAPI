using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CiresonPortalAPI
{
    [JsonConverter(typeof(WorkItemSerializer))]
    public abstract class BaseWorkItem
    {
        protected internal bool    _bDirtyObject = false;
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
        /// Called during initialization of an existing work item to clear the dirty object flag
        /// </summary>
        internal void FinalizeInitialization()
        {
            _bDirtyObject = false;
        }

        /// <summary>
        /// Attempts to update the work item.
        /// </summary>
        /// <returns>Returns true if successful, false if not.</returns>
        public async Task<bool> Update()
        {
            if (_bDirtyObject == false)
                return false;

            return false;
            //return await WriteObject();
        }

        protected Enumeration DeserializeEnumeration(string id, string name)
        {
            return new Enumeration(id, name, name, true, false);
        }
    }

    public class WorkItemSerializer : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            BaseWorkItem workItem = (BaseWorkItem)value;

            writer.WriteStartObject();

            #region formJson object
            {
                writer.WritePropertyName("formJson");
                writer.WriteStartObject();

                writer.WritePropertyName("isDirty");
                writer.WriteValue(workItem._bDirtyObject);

                #region current object
                writer.WritePropertyName("current");
                writer.WriteStartObject();
                writer.WriteValue(workItem._oCurrentObject);
                writer.WriteEndObject();
                #endregion current object

                #region original object
                writer.WritePropertyName("original");
                writer.WriteStartObject();
                writer.WriteValue(workItem._oOriginalObject);
                writer.WriteEndObject();
                #endregion original object

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
