using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;
using Newtonsoft.Json;
using System.Dynamic;

namespace CiresonPortalAPI.ConfigurationItems
{
    public class HardwareAsset : ConfigurationItem
    {
        #region Read-Only Properties
        /// <summary>
        /// Gets this hardware asset's unique hardware asset ID. Read only.
        /// </summary>
        public string HardwareAssetID
        {
            get { return this.GetPrimitiveValue<string>("HardwareAssetID"); }
        }
        #endregion Read-Only Properties

        #region Read-Write Properties
        /// <summary>
        /// Gets or sets this hardware asset's asset tag.
        /// </summary>
        public string AssetTag
        {
            get { return this.GetPrimitiveValue<string>("AssetTag"); }
            set { this.SetPrimitiveValue<string>("AssetTag", value); }
        }

        /// <summary>
        /// Gets or sets this hardware asset's manufacturer.
        /// </summary>
        public string Manufacturer
        {
            get { return this.GetPrimitiveValue<string>("Manufacturer"); }
            set { this.SetPrimitiveValue<string>("Manufacturer", value); }
        }

        /// <summary>
        /// Gets or sets this hardware asset's model.
        /// </summary>
        public string Model
        {
            get { return this.GetPrimitiveValue<string>("Model"); }
            set { this.SetPrimitiveValue<string>("Model", value); }
        }

        /// <summary>
        /// Gets or sets this hardware asset's name.
        /// </summary>
        public string Name
        {
            get { return this.GetPrimitiveValue<string>("Name"); }
            set { this.SetPrimitiveValue<string>("Name", value); }
        }

        /// <summary>
        /// Gets or sets this hardware asset's serial number.
        /// </summary>
        public string SerialNumber
        {
            get { return this.GetPrimitiveValue<string>("SerialNumber"); }
            set { this.SetPrimitiveValue<string>("SerialNumber", value); }
        }

        /// <summary>
        /// Date this hardware asset was received.
        /// </summary>
        public DateTime? ReceivedDate
        {
            get { return this.GetPrimitiveValue<DateTime?>("ReceivedDate"); }
            set { this.SetPrimitiveValue<DateTime?>("ReceivedDate", value); }
        }
        #endregion Read-Write Properties

        #region Relationship Properties
        /// <summary>
        /// Gets or sets the owning User of this HardwareAsset
        /// </summary>
        public User Custodian
        {
            get { return this.GetRelatedObject<User>("OwnedBy"); }
            set { this.SetRelatedObject("OwnedBy", value); }
        }

        /// <summary>
        /// Gets or sets the Location of this HardwareAsset
        /// </summary>
        public Location Location
        {
            get { return this.GetRelatedObject<Location>("Target_HardwareAssetHasLocation"); }
            set { this.SetRelatedObject("Target_HardwareAssetHasLocation", value, "Location"); }
        }

        /// <summary>
        /// Gets or sets the PurchaseOrder related to this HardwareAsset
        /// </summary>
        public PurchaseOrder PurchaseOrder
        {
            get { return this.GetRelatedObject<PurchaseOrder>("Target_HardwareAssetHasPurchaseOrder"); }
            set { this.SetRelatedObject("Target_HardwareAssetHasPurchaseOrder", value, "PurchaseOrder"); }
        }
        #endregion Relationship Properties

        #region Constructors
        internal HardwareAsset(ExpandoObject obj, bool existingObject = false, bool readOnly = true) : base(obj, existingObject, readOnly) { }
        internal HardwareAsset() : base() { }
        #endregion // Constructors

        #region General Methods
        /// <summary>
        /// Refreshes this HardwareAsset from the portal. This will reset any changes made to the object.
        /// This method must be called before accessing properties of children in relationship collections in order to populate all properties.
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <returns></returns>
        public override async Task<bool> Refresh(AuthorizationToken authToken)
        {
            return await this.RefreshType<HardwareAsset>(authToken);
        }
        #endregion // General Methods
    }
}
