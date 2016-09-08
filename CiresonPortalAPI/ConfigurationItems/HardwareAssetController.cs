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
        #region Finders

        /// <summary>
        /// Retrieves a list of HardwareAssets that match the given criteria
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="criteria">QueryCriteria to search for</param>
        /// <param name="includeInactiveItems">If true, override the criteria to exclude inactive items (this will set the grouping operator to AND!)</param>
        /// <returns></returns>
        public static async Task<List<HardwareAsset>> GetHardwareAssetsByCriteria(AuthorizationToken authToken, QueryCriteria criteria, bool includeInactiveItems = false)
        {
            criteria.ProjectionID = TypeProjectionConstants.HardwareAsset;
            return await ConfigurationItemController.GetConfigurationItemsByCriteria<HardwareAsset>(authToken, criteria);
        }

        /// <summary>
        /// Convenience method that returns a list of HardwareAssets with matching asset tags
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="assetTag">Asset's unique asset tag</param>
        /// <returns>HardwareAsset</returns>
        public static async Task<List<HardwareAsset>> GetHardwareAssetsByAssetTag(AuthorizationToken authToken, string assetTag)
        {
            QueryCriteria criteria = BuildCriteria(new PropertyPathHelper(ClassConstants.HardwareAsset, "AssetTag"), assetTag);
            return await HardwareAssetController.GetHardwareAssetsByCriteria(authToken, criteria);
        }

        /// <summary>
        /// Convenience method that returns a HardwareAsset by its unique HardwareAssetID
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="id">HardwareAssetID</param>
        /// <returns></returns>
        public static async Task<HardwareAsset> GetHardwareAssetByID(AuthorizationToken authToken, string id)
        {
            QueryCriteria criteria = BuildCriteria(new PropertyPathHelper(ClassConstants.HardwareAsset, "HardwareAssetID"), id);
            List<HardwareAsset> assetList = await HardwareAssetController.GetHardwareAssetsByCriteria(authToken, criteria);

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
        public static async Task<List<HardwareAsset>> GetHardwareAssetsBySerialNumber(AuthorizationToken authToken, string serialNumber)
        {
            QueryCriteria criteria = BuildCriteria(new PropertyPathHelper(ClassConstants.HardwareAsset, "SerialNumber"), serialNumber);
            return await HardwareAssetController.GetHardwareAssetsByCriteria(authToken, criteria);
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
            QueryCriteria criteria = BuildCriteria(new PropertyPathHelper(ClassConstants.HardwareAsset, "HardwareAssetType"), assetType.Id.ToString());
            return await HardwareAssetController.GetHardwareAssetsByCriteria(authToken, criteria);
        }
        #endregion Finders

        #region Creators
        /// <summary>
        /// Creates a new HardwareAsset
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="model">Model of the asset</param>
        /// <param name="manufacturer">Manufacturer of the asset</param>
        /// <param name="assetTag">Asset tag</param>
        /// <param name="serialNumber">Asset's serial number</param>
        /// <returns></returns>
        public static async Task<HardwareAsset> CreateNewHardwareAsset(AuthorizationToken authToken, string model, string manufacturer, string assetTag, string serialNumber)
        {
            dynamic extraProps = new {
                Model = model,
                Manufacturer = manufacturer,
                AssetTag = assetTag,
                SerialNumber = serialNumber,
                HardwareAssetID = assetTag
            };

            return await ConfigurationItemController.CreateConfigurationItem<HardwareAsset>(authToken, assetTag, assetTag, extraProps);
        }
        #endregion Creators

        #region Helpers

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

            QueryCriteria criteria = new QueryCriteria(TypeProjectionConstants.HardwareAsset)
            {
                GroupingOperator = QueryCriteriaGroupingOperator.SimpleExpression
            };

            criteria.Expressions.Add(expression);

            return criteria;
        }

        #endregion Helpers
    }
}
