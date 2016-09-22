using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CiresonPortalAPI.ConfigurationItems
{
    public class PurchaseOrder : ConfigurationItem
    {
        #region Read-Only Properties
        /// <summary>
        /// Gets the number of the Purchase Order. Read only.
        /// </summary>
        public string OrderNumber
        {
            get { return this.GetPrimitiveValue<string>("PurchaseOrderNumber"); }
        }
        #endregion Read-Only Properties

        #region Read-Write Properties
        /// <summary>
        /// Gets or sets the Purchase Order's amount
        /// </summary>
        public decimal? Amount
        {
            get { return this.GetPrimitiveValue<decimal?>("Amount"); }
            set
            {
                if (value.HasValue)
                {
                    if (value.Value < 0)
                        throw new ArgumentOutOfRangeException("Amount", "Cannot assign a negative value to a PurchaseOrder Amount.");
                    else
                        this.SetPrimitiveValue<decimal?>("Amount", value.Value);
                }
                else
                    this.SetPrimitiveValue<decimal?>("Amount", null);
            }
        }

        /// <summary>
        /// Gets or sets the Purchase Order's asset status
        /// </summary>
        public Enumeration AssetStatus
        {
            get { return this.GetEnumeration("AssetStatus"); }
            set { this.SetEnumeration("AssetStatus", value); }
        }

        /// <summary>
        /// Gets or sets the Purchase Order's currency.
        /// </summary>
        public Enumeration Currency
        {
            get { return this.GetEnumeration("Currency"); }
            set { this.SetEnumeration("Currency", value); }
        }

        /// <summary>
        /// Gets or sets the Purchase Order date.
        /// </summary>
        public DateTime? OrderDate
        {
            get { return this.GetPrimitiveValue<DateTime?>("PurchaseOrderDate"); }
            set { this.SetPrimitiveValue<DateTime?>("PurchaseOrderDate", value, "OrderDate"); }
        }

        /// <summary>
        /// Gets or sets the Purchase Order's status.
        /// </summary>
        public Enumeration OrderStatus
        {
            get { return this.GetEnumeration("PurchaseOrderStatus"); }
            set { this.SetEnumeration("PurchaseOrderStatus", value, "OrderStatus"); }
        }

        /// <summary>
        /// Gets or sets the Purchase Order's type.
        /// </summary>
        public Enumeration OrderType
        {
            get { return this.GetEnumeration("PurchaseOrderType"); }
            set { this.SetEnumeration("PurchaseOrderType", value, "OrderType"); }
        }
        #endregion Read-Write Properties

        #region Relationship Properties
        /// <summary>
        /// Returns a list of all child Purchase Orders.
        /// </summary>
        public RelatedObjectList<PurchaseOrder> Children
        {
            get
            {
                return new RelatedObjectList<PurchaseOrder>(this, "Target_PurchaseOrderHasChildPurchaseOrder");
            }
        }

        /// <summary>
        /// Gets or sets the parent Purchase Order. Read-only.
        /// </summary>
        public PurchaseOrder Parent
        {
            get { return this.GetRelatedObject<PurchaseOrder>("Source_PurchaseOrderHasChildPurchaseOrder"); }
            set { this.SetRelatedObject("Source_PurchaseOrderHasChildPurchaseOrder", value, "Parent"); }
        }
        #endregion Relationship Properties

        #region Constructors
        internal PurchaseOrder(ExpandoObject obj, bool existingObject = false, bool readOnly = true) : base(obj, existingObject, readOnly) { }
        internal PurchaseOrder() : base() { }
        #endregion // Constructors

        #region Public Methods
        /// <summary>
        /// Refreshes this PurchaseOrder from the portal. This will reset any changes made to the object.
        /// This method must be called before accessing properties of children in relationship collections in order to populate all properties.
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <returns></returns>
        public override async Task<bool> Refresh(AuthorizationToken authToken)
        {
            return await this.RefreshType<PurchaseOrder>(authToken);
        }
        #endregion // Public Methods
    }
}
