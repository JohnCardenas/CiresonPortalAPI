using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiresonPortalAPI.ConfigurationItems
{
    public abstract class ConfigurationItem : TypeProjection
    {
        #region Read-Write Properties
        /// <summary>
        /// Gets or sets this Configuration Item's Notes field.
        /// </summary>
        public string Notes
        {
            get { return this.GetPrimitiveValue<string>("Notes"); }
            set { this.SetPrimitiveValue<string>("Notes", value); }
        }
        #endregion // Read-Write Properties

        #region Constructors
        internal ConfigurationItem(ExpandoObject obj, bool existingObject = false, bool readOnly = true) : base(obj, existingObject, readOnly) { }
        internal ConfigurationItem() : this(null, true, false) { }
        #endregion // Constructors
    }
}
