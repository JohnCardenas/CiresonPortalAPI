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
        public static async Task<List<HardwareAsset>> GetHardwareAssetsByCriteria(AuthorizationToken authToken, QueryCriteria criteria)
        {
            criteria.ProjectionID = TypeProjectionConstants.HardwareAsset;
            return await TypeProjectionController.GenericToSpecific<HardwareAsset>(authToken, criteria);
        }

        /// <summary>
        /// Convenience method that returns a HardwareAsset by its asset tag
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="assetTag">Asset's unique asset tag</param>
        /// <returns>HardwareAsset</returns>
        public static async Task<HardwareAsset> GetHardwareAssetByAssetTag(AuthorizationToken authToken, string assetTag)
        {
            QueryCriteriaExpression expression = new QueryCriteriaExpression();
            expression.PropertyName = (new PropertyPathHelper(ClassConstants.HardwareAsset, "AssetTag")).ToString();
            expression.PropertyType = QueryCriteriaPropertyType.Property;
            expression.Operator = QueryCriteriaExpressionOperator.Equal;
            expression.Value = assetTag;

            QueryCriteria criteria = new QueryCriteria(TypeProjectionConstants.HardwareAsset);
            criteria.GroupingOperator = QueryCriteriaGroupingOperator.SimpleExpression;
            criteria.Expressions.Add(expression);

            List<TypeProjection> projectionList = await TypeProjectionController.GetProjectionByCriteria(authToken, criteria);

            if (projectionList.Count == 0)
                return null;

            if (projectionList.Count > 1)
                throw new Exception("More than one asset found with the same asset tag!");

            return new HardwareAsset(projectionList[0]);
        }
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
