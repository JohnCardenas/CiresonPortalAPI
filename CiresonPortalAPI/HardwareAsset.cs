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
        /// <summary>
        /// Convenience method that returns a HardwareAsset by its Asset Tag
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="assetTag">Asset Tag to retrieve</param>
        /// <returns>HardwareAsset</returns>
        public static async Task<HardwareAsset> GetAssetByAssetTag(AuthorizationToken authToken, string assetTag)
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

            return new HardwareAsset(projectionList[0]);
        }
    }


    public class HardwareAsset : TypeProjection
    {
        private PurchaseOrder _oPurchaseOrder = null;

        #region Read-Only Properties

        /// <summary>
        /// Returns this hardware asset's Hardware Asset ID. Read only.
        /// </summary>
        public Guid HardwareAssetID { get { return DynamicObjectHelpers.GetProperty<Guid>(_oCurrentObject, "HardwareAssetID"); } }

        #endregion Read-Only Properties

        #region Read-Write Properties

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
