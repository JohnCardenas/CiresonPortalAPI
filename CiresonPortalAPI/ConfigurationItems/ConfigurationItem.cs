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
        /// <summary>
        /// If true, this object is "alive" -- it has an ObjectStatus equal to Active.
        /// </summary>
        public bool IsActive
        {
            get
            {
                if (ObjectStatus == null)
                    return false;

                return ObjectStatus.Id == EnumerationConstants.ConfigItem.BuiltinValues.ObjectStatus.Active;
            }
        }

        /// <summary>
        /// If true, this object has been marked for deletion.
        /// </summary>
        public bool IsDeleted
        {
            get
            {
                if (ObjectStatus == null)
                    return false;

                return (ObjectStatus.Id == EnumerationConstants.ConfigItem.BuiltinValues.ObjectStatus.Deleted ||
                        ObjectStatus.Id == EnumerationConstants.ConfigItem.BuiltinValues.ObjectStatus.PendingDelete);
            }
        }

        /// <summary>
        /// Returns the status of this object. Read only.
        /// </summary>
        public Enumeration ObjectStatus
        {
            get { return this.GetEnumeration("ObjectStatus"); }
            internal set { this.SetEnumeration("ObjectStatus", value); }
        }
        #endregion

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

        #region Internal Properties
        /// <summary>
        /// If true, allow a commit to fire for an object marked as deleted.
        /// </summary>
        internal bool AllowCommitDeleted { get; set; }
        #endregion // Internal Properties

        #region Constructors
        internal ConfigurationItem(ExpandoObject obj, bool existingObject = false, bool readOnly = true) : base(obj, existingObject, readOnly) { }
        internal ConfigurationItem() : this(null, true, false) { }
        #endregion // Constructors

        #region Public Methods
        /// <summary>
        /// Attempts to commit the configuration item to the portal. Throws an exception if not successful.
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        public override Task<bool> Commit(AuthorizationToken authToken)
        {
            if (!AllowCommitDeleted && this.IsDeleted)
                throw new CiresonApiException("Cannot commit a deleted object.");

            // Any object with a null ObjectStatus is a brand new object and needs to have the status set to Active
            if (this.ObjectStatus == null)
                this.ObjectStatus = new Enumeration(EnumerationConstants.ConfigItem.BuiltinValues.ObjectStatus.Active, "Active", "Active", false, false);

            return base.Commit(authToken);
        }
        #endregion // Public Methods
    }
}
