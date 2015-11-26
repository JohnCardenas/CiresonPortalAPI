using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiresonPortalAPI
{
    public abstract class ConfigurationItem : TypeProjection
    {
        #region Read-Only Properties
        /// <summary>Gets the DisplayName of the Work Item. Read only.</summary>
        public string DisplayName
        {
            get { return this.GetPrimitiveValue<string>("DisplayName"); }
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
    }
}
