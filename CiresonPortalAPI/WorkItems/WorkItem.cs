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
        #region Read-Only Properties
        /// <summary>
        /// Gets the ID of the Work Item. Note that this is not the same as an object's BaseId, which identifies it within Service Manager. Read-only.
        /// </summary>
        public string Id
        {
            get { return this.GetPrimitiveValue<string>("Id"); }
        }

        /// <summary>
        /// Gets or sets the WorkItem's title.
        /// </summary>
        public string Title
        {
            get { return this.GetPrimitiveValue<string>("Title"); }
            set { this.SetPrimitiveValue<string>("Title", value); }
        }

        /// <summary>
        /// Returns this WorkItem's default display name.
        /// </summary>
        public override string DefaultDisplayName
        {
            get
            {
                return this.Id + " - " + this.Title;
            }
        }
        #endregion // Read-Only Properties

        #region Constructors
        internal WorkItem(ExpandoObject obj, bool existingObject = false, bool readOnly = true) : base(obj, existingObject, readOnly) { }
        internal WorkItem() : this(null, true, false) { }
        #endregion // Constructors
    }
}
