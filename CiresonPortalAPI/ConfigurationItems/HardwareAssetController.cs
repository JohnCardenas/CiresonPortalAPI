using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiresonPortalAPI.ConfigurationItems
{
    public static class HardwareAssetController
    {
        #region Public Methods
        /// <summary>
        /// Creates a new HardwareAsset with the specified parameters.
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="model">Model of the asset</param>
        /// <param name="manufacturer">Manufacturer of the asset</param>
        /// <param name="assetTag">Asset tag</param>
        /// <param name="serialNumber">Asset's serial number</param>
        /// <returns></returns>
        public static async Task<HardwareAsset> Create(AuthorizationToken authToken, string model, string manufacturer, string assetTag, string serialNumber)
        {
            dynamic extraProps = new
            {
                Model = model,
                Manufacturer = manufacturer,
                AssetTag = assetTag,
                SerialNumber = serialNumber,
                HardwareAssetID = assetTag
            };

            return await TypeProjectionController.CreateBlankObject<HardwareAsset>(authToken, assetTag, assetTag, extraProps);
        }

        /// <summary>
        /// Retrieves a list of HardwareAssets that match the given criteria
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="criteria">QueryCriteria to search for</param>
        /// <param name="includeInactiveItems">If true, override the criteria to exclude inactive items (this will set the grouping operator to AND!)</param>
        /// <returns></returns>
        public static async Task<List<HardwareAsset>> GetByCriteria(AuthorizationToken authToken, QueryCriteria criteria, bool includeInactiveItems = false)
        {
            criteria.ProjectionID = TypeProjectionConstants.HardwareAsset.Id;
            return await ConfigurationItemController.GetByCriteria<HardwareAsset>(authToken, criteria);
        }

        /// <summary>
        /// Convenience method that returns a list of HardwareAssets with matching asset tags
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="assetTag">Asset's unique asset tag</param>
        /// <returns>HardwareAsset</returns>
        public static async Task<List<HardwareAsset>> GetByAssetTag(AuthorizationToken authToken, string assetTag)
        {
            QueryCriteria criteria = BuildCriteria(new PropertyPathHelper(ClassConstants.HardwareAsset.Id, "AssetTag"), assetTag);
            return await GetByCriteria(authToken, criteria);
        }

        /// <summary>
        /// Convenience method that returns a HardwareAsset by its unique HardwareAssetID
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="id">HardwareAssetID</param>
        /// <returns></returns>
        public static async Task<HardwareAsset> GetByHardwareAssetID(AuthorizationToken authToken, string id)
        {
            QueryCriteria criteria = BuildCriteria(new PropertyPathHelper(ClassConstants.HardwareAsset.Id, "HardwareAssetID"), id);
            List<HardwareAsset> assetList = await GetByCriteria(authToken, criteria);

            if (assetList == null)
                return null;

            if (assetList.Count == 0)
                return null;

            return assetList[0];
        }

        /// <summary>
        /// Convenience method that returns a list of HardwareAssets with matching serial numbers
        /// </summary>
        /// <param name="authToken"></param>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public static async Task<List<HardwareAsset>> GetBySerialNumber(AuthorizationToken authToken, string serialNumber)
        {
            QueryCriteria criteria = BuildCriteria(new PropertyPathHelper(ClassConstants.HardwareAsset.Id, "SerialNumber"), serialNumber);
            return await GetByCriteria(authToken, criteria);
        }

        /// <summary>
        /// Convenience method that returns a list of HardwareAssets by asset type. Depending on the size of the asset database,
        /// this might be a VERY large list, and therefore very time consuming!
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="assetType">HardwareAssetType</param>
        /// <returns></returns>
        public static async Task<List<HardwareAsset>> GetHardwareAssetsByType(AuthorizationToken authToken, Enumeration assetType)
        {
            QueryCriteria criteria = BuildCriteria(new PropertyPathHelper(ClassConstants.HardwareAsset.Id, "HardwareAssetType"), assetType.Id.ToString());
            return await GetByCriteria(authToken, criteria);
        }

        /// <summary>
        /// Marks a HardwareAsset for deletion
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="item">HardwareAsset to delete</param>
        /// <param name="markPending">If true, mark the object as Pending Deletion instead of Deleted.</param>
        /// <returns></returns>
        public static async Task<bool> Delete(AuthorizationToken authToken, HardwareAsset item, bool markPending = true)
        {
            return await ConfigurationItemController.DeleteObject(authToken, item, markPending);
        }
        #endregion // Public Methods

        #region Private Methods
        /// <summary>
        /// Helper method that returns a QueryCriteria used by the GetHardwareAssetBy.. methods
        /// </summary>
        /// <param name="property">Property to query</param>
        /// <param name="value">Property value to look for</param>
        /// <returns></returns>
        private static QueryCriteria BuildCriteria(PropertyPathHelper property, string value)
        {
            QueryCriteriaExpression expression = new QueryCriteriaExpression
            {
                PropertyName = property.ToString(),
                PropertyType = QueryCriteriaPropertyType.Property,
                Operator = QueryCriteriaExpressionOperator.Equal,
                Value = value
            };

            QueryCriteria criteria = new QueryCriteria(TypeProjectionConstants.HardwareAsset.Id)
            {
                GroupingOperator = QueryCriteriaGroupingOperator.SimpleExpression
            };

            criteria.Expressions.Add(expression);

            return criteria;
        }
        #endregion // Private Methods
    }
}
