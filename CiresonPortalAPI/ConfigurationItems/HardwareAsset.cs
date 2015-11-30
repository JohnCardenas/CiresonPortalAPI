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
    public class HardwareAsset : TypeProjection
    {
        //private PurchaseOrder _oPurchaseOrder = null;

        #region Read-Only Properties

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
        /// Gets or sets this hardware asset's unique hardware asset ID.
        /// </summary>
        public string HardwareAssetID
        {
            get { return this.GetPrimitiveValue<string>("HardwareAssetID"); }
            set { this.SetPrimitiveValue<string>("HardwareAssetID", value); }
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
        /// Gets or sets the PurchaseOrder related to this HardwareAsset
        /// </summary>
        public PurchaseOrder PurchaseOrder
        {
            get { return this.GetRelatedObject<PurchaseOrder>("Target_HardwareAssetHasPurchaseOrder"); }
            set { this.SetRelatedObject("Target_HardwareAssetHasPurchaseOrder", value, "PurchaseOrder"); }
        }
        #endregion Relationship Properties
    }
}
