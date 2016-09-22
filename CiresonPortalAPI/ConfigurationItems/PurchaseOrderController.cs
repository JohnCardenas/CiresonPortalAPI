using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiresonPortalAPI.ConfigurationItems
{
    public static class PurchaseOrderController
    {
        #region Public Methods
        /// <summary>
        /// Creates a new blank PurchaseOrder
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="name">Name of the PurchaseOrder</param>
        /// <param name="displayName">DisplayName of the PurchaseOrder</param>
        /// <returns></returns>
        public static async Task<PurchaseOrder> Create(AuthorizationToken authToken, string name, string displayName)
        {
            return await TypeProjectionController.CreateBlankObject<PurchaseOrder>(authToken, name, displayName);
        }

        /// <summary>
        /// Gets a list of Purchase Orders based on the supplied criteria
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="criteria">QueryCriteria to search for</param>
        /// <returns></returns>
        public static async Task<List<PurchaseOrder>> GetByCriteria(AuthorizationToken authToken, QueryCriteria criteria)
        {
            criteria.ProjectionID = TypeProjectionConstants.PurchaseOrder.Id;
            return await TypeProjectionController.GetByCriteria<PurchaseOrder>(authToken, criteria);
        }

        /// <summary>
        /// Convenience method that gets a list of all PurchaseOrders that are active
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <returns></returns>
        public static async Task<List<PurchaseOrder>> GetAll(AuthorizationToken authToken)
        {
            PropertyPathHelper pathHelper = new PropertyPathHelper();
            pathHelper.PropertyName = "ObjectStatus";
            pathHelper.ObjectClass = ClassConstants.GetClassIdByType<PurchaseOrder>();

            QueryCriteriaExpression expr = new QueryCriteriaExpression
            {
                PropertyName = pathHelper.ToString(),
                PropertyType = QueryCriteriaPropertyType.Property,
                Operator = QueryCriteriaExpressionOperator.Equal,
                Value = EnumerationConstants.TypeProjection.BuiltinValues.ObjectStatus.Active.ToString("D")
            };

            QueryCriteria criteria = new QueryCriteria(TypeProjectionConstants.PurchaseOrder.Id)
            {
                GroupingOperator = QueryCriteriaGroupingOperator.SimpleExpression
            };

            criteria.Expressions.Add(expr);

            return await GetByCriteria(authToken, criteria);
        }

        /// <summary>
        /// Returns a PurchaseOrder by ID
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="id">ID of the PurchaseOrder</param>
        /// <returns></returns>
        public static async Task<PurchaseOrder> GetByBaseId(AuthorizationToken authToken, Guid id)
        {
            return await TypeProjectionController.GetByBaseId<PurchaseOrder>(authToken, id);
        }

        /// <summary>
        /// Returns a PurchaseOrder by its FullName
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="fullName">FullName to find</param>
        /// <returns></returns>
        public static async Task<PurchaseOrder> GetByFullName(AuthorizationToken authToken, string fullName)
        {
            return await TypeProjectionController.GetByFullName<PurchaseOrder>(authToken, fullName);
        }

        /// <summary>
        /// Convenience method that returns a list of Purchase Orders with the specified order type
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="poTypeGuid">Purchase Order Type ID to search for</param>
        /// <returns></returns>
        public static async Task<List<PurchaseOrder>> GetByOrderType(AuthorizationToken authToken, Guid poTypeGuid)
        {
            QueryCriteriaExpression expression = new QueryCriteriaExpression
            {
                PropertyName = (new PropertyPathHelper(ClassConstants.PurchaseOrder.Id, "PurchaseOrderType")).ToString(),
                PropertyType = QueryCriteriaPropertyType.Property,
                Operator = QueryCriteriaExpressionOperator.Equal,
                Value = poTypeGuid.ToString("B")
            };

            QueryCriteria criteria = new QueryCriteria(TypeProjectionConstants.PurchaseOrder.Id);
            criteria.GroupingOperator = QueryCriteriaGroupingOperator.And;
            criteria.Expressions.Add(expression);

            List<PurchaseOrder> purchaseOrderList = await PurchaseOrderController.GetByCriteria(authToken, criteria);

            if (purchaseOrderList.Count == 0)
                return null;

            return purchaseOrderList;
        }

        /// <summary>
        /// Convenience method that returns a list of Purchase Orders with the specified order type
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="purchaseOrderType">Purchase Order Type to search for</param>
        /// <returns></returns>
        public static async Task<List<PurchaseOrder>> GetPurchaseOrdersByType(AuthorizationToken authToken, Enumeration purchaseOrderType)
        {
            return await GetByOrderType(authToken, purchaseOrderType.Id);
        }

        /// <summary>
        /// Marks a PurchaseOrder for deletion
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="item">PurchaseOrder to delete</param>
        /// <param name="markPending">If true, mark the object as Pending Deletion instead of Deleted.</param>
        /// <returns></returns>
        public static async Task<bool> Delete(AuthorizationToken authToken, PurchaseOrder item, bool markPending = true)
        {
            return await TypeProjectionController.DeleteObject(authToken, item, markPending);
        }
        #endregion // Public Methods
    }
}
