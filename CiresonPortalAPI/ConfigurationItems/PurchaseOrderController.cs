using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiresonPortalAPI.ConfigurationItems
{
    public static class PurchaseOrderController
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
            return await ConfigurationItem.GetConfigurationItemsByCriteria<PurchaseOrder>(authToken, criteria);
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

            QueryCriteria criteria = new QueryCriteria(TypeProjectionConstants.PurchaseOrder);
            criteria.GroupingOperator = QueryCriteriaGroupingOperator.And;
            criteria.Expressions.Add(expression);

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
}
