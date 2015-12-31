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
        #region Read-Only Properties
        /// <summary>Gets the DisplayName of the Configuration Item. Read only.</summary>
        public string DisplayName
        {
            get { return this.GetPrimitiveValue<string>("DisplayName"); }
        }

        /// <summary>
        /// Returns this Configuration Item's BaseId. Read only.
        /// </summary>
        public Guid Id
        {
            get { return this.GetPrimitiveValue<Guid>("BaseId"); }
        }

        /// <summary>
        /// Returns the status of this object
        /// </summary>
        public Enumeration ObjectStatus
        {
            get { return this.GetEnumeration("ObjectStatus"); }
        }
        #endregion // Read-Only Properties

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

        #region General Methods
        public override string ToString()
        {
            return this.DisplayName;
        }
        #endregion // General Methods

        #region Constructors
        internal ConfigurationItem(ExpandoObject obj, bool existingObject = false, bool readOnly = true) : base(obj, existingObject, readOnly) { }
        internal ConfigurationItem() : this(null, true, false) { }
        #endregion // Constructors
    }

    internal class ConfigurationItemComparer : IComparer<ConfigurationItem>
    {
        public int Compare(ConfigurationItem a, ConfigurationItem b)
        {
            return string.Compare(a.ToString(), b.ToString());
        }
    }
}
