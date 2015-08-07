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
            return await TypeProjectionController.GenericToSpecific<HardwareAsset>(authToken, criteria);
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
            TypeProjection projection = await TypeProjectionController.CreateProjectionByTemplate(authToken, templateId, userId);
            return new HardwareAsset(projection);
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
        private PurchaseOrder _oPurchaseOrder = null;

        #region Read-Only Properties

        #endregion Read-Only Properties

        #region Read-Write Properties

        /// <summary>
        /// Gets or sets this hardware asset's asset tag.
        /// </summary>
        public string AssetTag { get { return DynamicObjectHelpers.GetProperty<string>(_oCurrentObject, "AssetTag"); } set { _oCurrentObject.AssetTag = value; SetDirtyBit(); } }

        /// <summary>
        /// Gets or sets this hardware asset's unique hardware asset ID.
        /// </summary>
        public string HardwareAssetID { get { return DynamicObjectHelpers.GetProperty<string>(_oCurrentObject, "HardwareAssetID"); } set { _oCurrentObject.HardwareAssetID = value; SetDirtyBit(); } }

        /// <summary>
        /// Gets or sets this hardware asset's manufacturer.
        /// </summary>
        public string Manufacturer { get { return DynamicObjectHelpers.GetProperty<string>(_oCurrentObject, "Manufacturer"); } set { _oCurrentObject.Manufacturer = value; SetDirtyBit(); } }

        /// <summary>
        /// Gets or sets this hardware asset's model.
        /// </summary>
        public string Model { get { return DynamicObjectHelpers.GetProperty<string>(_oCurrentObject, "Model"); } set { _oCurrentObject.Model = value; SetDirtyBit(); } }

        /// <summary>
        /// Gets or sets this hardware asset's name.
        /// </summary>
        public string Name { get { return DynamicObjectHelpers.GetProperty<string>(_oCurrentObject, "Name"); } set { _oCurrentObject.Name = value; SetDirtyBit(); } }

        /// <summary>
        /// Gets or sets this hardware asset's serial number.
        /// </summary>
        public string SerialNumber { get { return DynamicObjectHelpers.GetProperty<string>(_oCurrentObject, "SerialNumber"); } set { _oCurrentObject.SerialNumber = value; SetDirtyBit(); } }

        /// <summary>
        /// Date this hardware asset was received.
        /// </summary>
        public Nullable<DateTime> ReceivedDate { get { return DynamicObjectHelpers.GetProperty<DateTime>(_oCurrentObject, "ReceivedDate"); } }

        #endregion Read-Write Properties

        #region Relationship Properties

        /// <summary>
        /// Gets or sets the PurchaseOrder related to this HardwareAsset
        /// </summary>
        public PurchaseOrder PurchaseOrder { get { return _oPurchaseOrder; } set { SetPurchaseOrder(value); } }

        #endregion Relationship Properties

        /// <summary>
        /// Sets the PurchaseOrder of this HardwareAsset.
        /// </summary>
        /// <param name="user">User to set as the Affected User</param>
        private void SetPurchaseOrder(PurchaseOrder order)
        {
            // Set the new Affected User
            _oCurrentObject.Target_HardwareAssetHasPurchaseOrder = order._oCurrentObject;
            _oPurchaseOrder = new PurchaseOrder(_oCurrentObject.Target_HardwareAssetHasPurchaseOrder);
            SetDirtyBit();
        }

        #region Relationship Setters
        #endregion Relationship Setters

        #region Constructors

        /// <summary>
        /// Constructor used internally when an existing object has been queried
        /// </summary>
        /// <param name="projection">Parent type projection</param>
        internal HardwareAsset(TypeProjection projection)
        {
            _oOriginalObject = projection._oOriginalObject;
            _oCurrentObject = projection._oCurrentObject;
            _bReadOnly = false;

            // Related objects
            if (DynamicObjectHelpers.HasProperty(_oCurrentObject, "Target_HardwareAssetHasPurchaseOrder"))
                _oPurchaseOrder = new PurchaseOrder(_oCurrentObject.Target_HardwareAssetHasPurchaseOrder);
        }

        #endregion Constructors
    }
}
