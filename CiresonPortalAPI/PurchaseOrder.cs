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
            return await TypeProjectionController.GenericToSpecific<PurchaseOrder>(authToken, criteria);
        }

        /// <summary>
        /// Convenience method that returns a list of Purchase Orders with the specified order type
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="purchaseOrderType">Purchase Order Type</param>
        /// <returns></returns>
        public static async Task<List<PurchaseOrder>> GetPurchaseOrdersByType(AuthorizationToken authToken, Enumeration purchaseOrderType)
        {
            QueryCriteriaExpression expression = new QueryCriteriaExpression();
            expression.PropertyName = (new PropertyPathHelper(ClassConstants.PurchaseOrder, "PurchaseOrderType")).ToString();
            expression.PropertyType = QueryCriteriaPropertyType.Property;
            expression.Operator = QueryCriteriaExpressionOperator.Equal;
            expression.Value = purchaseOrderType.Id.ToString("B");

            QueryCriteria criteria = new QueryCriteria(TypeProjectionConstants.PurchaseOrder);
            criteria.GroupingOperator = QueryCriteriaGroupingOperator.SimpleExpression;
            criteria.Expressions.Add(expression);

            List<PurchaseOrder> purchaseOrderList = await PurchaseOrderController.GetPurchaseOrdersByCriteria(authToken, criteria);

            if (purchaseOrderList.Count == 0)
                return null;

            return purchaseOrderList;
        }
    }

    public class PurchaseOrder : TypeProjection
    {
        #region Read-Only Properties

        /// <summary>Gets the DisplayName of the Purchase Order. Read only.</summary>
        public string DisplayName { get { return DynamicObjectHelpers.GetProperty<string>(_oCurrentObject, "DisplayName"); } }

        /// <summary>Gets the number of the Purchase Order. Read only.</summary>
        public string PurchaseOrderNumber { get { return DynamicObjectHelpers.GetProperty<string>(_oCurrentObject, "PurchaseOrderNumber"); } }

        #endregion Read-Only Properties

        #region Read-Write Properties

        /// <summary>Gets or sets the Purchase Order's amount</summary>
        public decimal Amount { get { return DynamicObjectHelpers.GetProperty<decimal>(_oCurrentObject, "Amount"); } set { _oCurrentObject.Amount = value; SetDirtyBit(); } }

        /// <summary>Gets or sets the Purchase Order's asset status</summary>
        public Enumeration AssetStatus
        {
            get
            {
                return DeserializeEnumeration(_oCurrentObject.AssetStatus.Id, _oCurrentObject.AssetStatus.Name);
            }
            set
            {
                SetEnumerationValue(_oCurrentObject.AssetStatus, value);
                SetDirtyBit();
            }
        }

        /// <summary>Gets or sets the Purchase Order's currency</summary>
        public Enumeration Currency
        {
            get
            {
                return DeserializeEnumeration(_oCurrentObject.Currency.Id, _oCurrentObject.Currency.Name);
            }
            set
            {
                SetEnumerationValue(_oCurrentObject.Currency, value);
                SetDirtyBit();
            }
        }

        /// <summary>Gets or sets the Purchase Order's notes field.</summary>
        public string Notes { get { return DynamicObjectHelpers.GetProperty<string>(_oCurrentObject, "Notes"); } set { _oCurrentObject.Notes = value; SetDirtyBit(); } }

        /// <summary>Gets or sets the Purchase Order date.</summary>
        public Nullable<DateTime> PurchaseOrderDate { get { return DynamicObjectHelpers.GetProperty<Nullable<DateTime>>(_oCurrentObject, "PurchaseOrderDate"); } set { _oCurrentObject.PurchaseOrderDate = value; SetDirtyBit(); } }

        /// <summary>Gets or sets the Purchase Order's status</summary>
        public Enumeration PurchaseOrderStatus
        {
            get
            {
                return DeserializeEnumeration(_oCurrentObject.PurchaseOrderStatus.Id, _oCurrentObject.PurchaseOrderStatus.Name);
            }
            set
            {
                SetEnumerationValue(_oCurrentObject.PurchaseOrderStatus, value);
                SetDirtyBit();
            }
        }

        /// <summary>Gets or sets the Purchase Order's type.</summary>
        public Enumeration PurchaseOrderType
        {
            get
            {
                return DeserializeEnumeration(_oCurrentObject.PurchaseOrderType.Id, _oCurrentObject.PurchaseOrderType.Name);
            }
            set
            {
                SetEnumerationValue(_oCurrentObject.PurchaseOrderType, value);
                SetDirtyBit();
            }
        }

        #endregion Read-Write Properties

        #region Relationship Properties
        #endregion Relationship Properties

        #region Relationship Setters
        #endregion Relationship Setters

        #region Constructors

        /// <summary>
        /// Constructor used internally when an existing object has been queried
        /// </summary>
        /// <param name="projection">Parent type projection</param>
        internal PurchaseOrder(TypeProjection projection) : this((ExpandoObject)projection._oCurrentObject) { }

        /// <summary>
        /// Constructor used to build a PurchaseOrder from a JSON data set
        /// </summary>
        /// <param name="obj"></param>
        internal PurchaseOrder(dynamic obj)
        {
            _oOriginalObject = obj;
            _oCurrentObject = obj;
            _bReadOnly = false;            
        }

        #endregion Constructors
    }
}
