using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiresonPortalAPI.WorkItems
{
    public abstract class WorkItem : TypeProjection
    {
        #region Constructors
        internal WorkItem(ExpandoObject obj, bool existingObject = false, bool readOnly = true) : base(obj, existingObject, readOnly) { }
        internal WorkItem() : this(null, true, false) { }
        #endregion // Constructors

        #region Read-Only Properties
        /// <summary>Gets the DisplayName of the Work Item. Read only.</summary>
        public string DisplayName
        {
            get { return this.GetPrimitiveValue<string>("DisplayName"); }
        }
        #endregion // Read-Only Properties

        #region General Methods
        public override string ToString()
        {
            return this.DisplayName;
        }
        #endregion // General Methods
    }
}
