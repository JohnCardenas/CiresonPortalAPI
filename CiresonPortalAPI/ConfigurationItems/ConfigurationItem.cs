using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiresonPortalAPI.ConfigurationItems
{
    public abstract class ConfigurationItem : TypeProjection, IEquatable<ConfigurationItem>
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
            internal set { this.SetEnumeration("ObjectStatus", value); }
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

        /// <summary>
        /// Equality check method
        /// </summary>
        /// <param name="other">Other ConfigurationItem to check for equality</param>
        /// <returns></returns>
        public bool Equals(ConfigurationItem other)
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
            return Equals(obj as ConfigurationItem);
        }

        /// <summary>
        /// Equality check method
        /// </summary>
        /// <param name="a">First object to check for equality</param>
        /// <param name="b">Second object to check for equality</param>
        /// <returns></returns>
        public static bool Equals(ConfigurationItem a, ConfigurationItem b)
        {
            // If both or null or the same instance, return true
            if (System.Object.ReferenceEquals(a, b))
                return true;

            // If one is null but not both, return false
            if (((object)a == null) || ((object)b == null))
                return false;

            // Return true if the IDs match
            return a.Id == b.Id;
        }

        /// <summary>
        /// Overload equality check operator
        /// </summary>
        /// <param name="a">First operand to check for equality</param>
        /// <param name="b">Second operand to check for equality</param>
        /// <returns></returns>
        public static bool operator ==(ConfigurationItem a, ConfigurationItem b)
        {
            return Equals(a, b);
        }

        /// <summary>
        /// Overload non-equality check operator
        /// </summary>
        /// <param name="a">First operand to check for non-equality</param>
        /// <param name="b">Second operand to check for non-equality</param>
        /// <returns></returns>
        public static bool operator !=(ConfigurationItem a, ConfigurationItem b)
        {
            return !Equals(a, b);
        }

        /// <summary>
        /// Hash code method
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        /// <summary>
        /// Refreshes a configuration item from the portal.
        /// This method must be called before accessing properties of children in relationship collections in order to populate all properties.
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <returns></returns>
        public abstract Task<bool> Refresh(AuthorizationToken authToken);

        /// <summary>
        /// Refreshes a configuration item from the database
        /// </summary>
        /// <typeparam name="T">Type of the ConfigurationItem to refresh</typeparam>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <returns></returns>
        internal async Task<bool> RefreshType<T>(AuthorizationToken authToken) where T : ConfigurationItem
        {
            ConfigurationItem ci = await ConfigurationItemController.GetConfigurationItemByBaseId<T>(authToken, this.Id);

            if (ci == null)
                return false;

            this.CurrentObject  = ci.CurrentObject;
            this.OriginalObject = ci.OriginalObject;
            this.ReadOnly       = ci.ReadOnly;
            this.DirtyObject    = false;

            return true;
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
