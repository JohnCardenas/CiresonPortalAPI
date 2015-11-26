using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CiresonPortalAPI
{
    public static partial class PurchaseOrderController
    {
        /// <summary>
        /// Gets a list of Purchase Orders based on the supplied criteria
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="criteria">QueryCriteria to search for</param>
        /// <returns></returns>
        public static async Task<List<PurchaseOrder>> GetPurchaseOrdersByCriteria(AuthorizationToken authToken, QueryCriteria criteria)
        {
            criteria.ProjectionID = TypeProjectionConstants.PurchaseOrder;
            return await TypeProjectionController.GetProjectionByCriteria<PurchaseOrder>(authToken, criteria);
        }

        /// <summary>
        /// Convenience method that returns a list of Purchase Orders with the specified order type
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="poTypeGuid">Purchase Order Type ID to search for</param>
        /// <returns></returns>
        public static async Task<List<PurchaseOrder>> GetPurchaseOrdersByType(AuthorizationToken authToken, Guid poTypeGuid)
        {
            QueryCriteriaExpression expression = new QueryCriteriaExpression
            {
                PropertyName = (new PropertyPathHelper(ClassConstants.PurchaseOrder, "PurchaseOrderType")).ToString(),
                PropertyType = QueryCriteriaPropertyType.Property,
                Operator = QueryCriteriaExpressionOperator.Equal,
                Value = poTypeGuid.ToString("B")
            };

            // Purchase Orders inherit System.ConfigItem, so we need to exclude Deleted and PendingDeletion POs
            QueryCriteriaExpression activeItemsOnly = new QueryCriteriaExpression
            {
                PropertyName = (new PropertyPathHelper(ClassConstants.PurchaseOrder, "ObjectStatus")).ToString(),
                PropertyType = QueryCriteriaPropertyType.Property,
                Operator = QueryCriteriaExpressionOperator.Equal,
                Value = EnumerationConstants.ConfigItem.BuiltinValues.ObjectStatus.Active.ToString("B")
            };

            QueryCriteria criteria = new QueryCriteria(TypeProjectionConstants.PurchaseOrder);
            criteria.GroupingOperator = QueryCriteriaGroupingOperator.And;
            criteria.Expressions.Add(expression);
            criteria.Expressions.Add(activeItemsOnly);

            List<PurchaseOrder> purchaseOrderList = await PurchaseOrderController.GetPurchaseOrdersByCriteria(authToken, criteria);

            if (purchaseOrderList.Count == 0)
                return null;

            return purchaseOrderList;
        }

        /// <summary>
        /// Overloaded method, takes an Enumeration instead of a GUID
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="purchaseOrderType">Purchase Order Type to search for</param>
        /// <returns></returns>
        public static async Task<List<PurchaseOrder>> GetPurchaseOrdersByType(AuthorizationToken authToken, Enumeration purchaseOrderType)
        {
            return await GetPurchaseOrdersByType(authToken, purchaseOrderType.Id);
        }
    }

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
        public decimal Amount
        {
            get { return this.GetPrimitiveValue<decimal>("Amount"); }
            set { this.SetPrimitiveValue<decimal>("Amount", value); }
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
        /// Returns a list of all child Purchase Orders. Read only.
        /// </summary>
        public List<PurchaseOrder> Children
        {
            get { return this.GetRelatedObjectsList<PurchaseOrder>("Target_PurchaseOrderHasChildPurchaseOrder"); }
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
    }
}
