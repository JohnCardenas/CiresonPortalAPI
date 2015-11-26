using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;
using Newtonsoft.Json;
using System.Dynamic;

namespace CiresonPortalAPI
{
    public static partial class HardwareAssetController
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
            if (!includeInactiveItems)
            {
                // Hardware Assets inherit System.ConfigItem, so we need to exclude Deleted and PendingDeletion POs
                QueryCriteriaExpression activeItemsOnly = new QueryCriteriaExpression
                {
                    PropertyName = (new PropertyPathHelper(ClassConstants.HardwareAsset, "ObjectStatus")).ToString(),
                    PropertyType = QueryCriteriaPropertyType.Property,
                    Operator = QueryCriteriaExpressionOperator.Equal,
                    Value = EnumerationConstants.ConfigItem.BuiltinValues.ObjectStatus.Active.ToString("B")
                };

                criteria.GroupingOperator = QueryCriteriaGroupingOperator.And;
                criteria.Expressions.Add(activeItemsOnly);
            }

            criteria.ProjectionID = TypeProjectionConstants.HardwareAsset;
            return await TypeProjectionController.GetProjectionByCriteria<HardwareAsset>(authToken, criteria);
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

        #endregion Finders

        #region Creators

        /// <summary>
        /// Creates a new Hardware Asset based on the supplied Template ID
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="templateId">TemplateID to use</param>
        /// <param name="userId">ID of the user creating the Incident</param>
        /// <returns></returns>
        public static async Task<HardwareAsset> CreateNewHardwareAsset(AuthorizationToken authToken, Guid templateId, Guid userId)
        {
            TypeProjection projection = await TypeProjectionController.CreateProjectionByTemplate<HardwareAsset>(authToken, templateId, userId);
            return (HardwareAsset)projection;
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
