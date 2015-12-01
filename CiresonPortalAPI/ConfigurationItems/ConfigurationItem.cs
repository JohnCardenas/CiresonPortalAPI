using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiresonPortalAPI.ConfigurationItems
{
    public abstract class ConfigurationItem : TypeProjection
    {
        #region Read-Only Properties
        /// <summary>Gets the DisplayName of the Configuration Item. Read only.</summary>
        public string DisplayName
        {
            get { return this.GetPrimitiveValue<string>("DisplayName"); }
        }

        /// <summary>
        /// Returns this Configuration Item's BaseId. Read only.
        /// </summary>
        public Guid Id
        {
            get { return this.GetPrimitiveValue<Guid>("BaseId"); }
        }

        /// <summary>
        /// Returns the status of this object
        /// </summary>
        public Enumeration ObjectStatus
        {
            get { return this.GetEnumeration("ObjectStatus"); }
        }
        #endregion // Read-Only Properties

        #region Read-Write Properties
        /// <summary>
        /// Gets or sets this Configuration Item's Notes field.
        /// </summary>
        public string Notes
        {
            get { return this.GetPrimitiveValue<string>("Notes"); }
            set { this.SetPrimitiveValue<string>("Notes", value); }
        }
        #endregion // Read-Write Properties

        #region General Methods
        public override string ToString()
        {
            return this.DisplayName;
        }

        /// <summary>
        /// Helper method to retrieve a list of objects derived from the ConfigurationItem class, using the specified criteria
        /// </summary>
        /// <typeparam name="T">ConfigurationItem derived type</typeparam>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="criteria">QueryCriteria to use</param>
        /// <param name="includeInactiveItems">If true, include inactive (deleted) configuration items</param>
        /// <returns></returns>
        internal static async Task<List<T>> GetConfigurationItemsByCriteria<T>(AuthorizationToken authToken, QueryCriteria criteria, bool includeInactiveItems = false) where T : ConfigurationItem
        {
            QueryCriteria useCriteria = criteria;

            if (!includeInactiveItems)
            {
                useCriteria = ExcludeInactiveItems<T>(criteria);
            }

            return await TypeProjectionController.GetProjectionByCriteria<T>(authToken, useCriteria);
        }

        /// <summary>
        /// Helper method to retrieve an object derived from the ConfigurationItem class, using its baseId
        /// </summary>
        /// <typeparam name="T">ConfigurationItem derived type</typeparam>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="baseId">BaseId to find</param>
        /// <returns></returns>
        internal static async Task<T> GetConfigurationItemByBaseId<T>(AuthorizationToken authToken, Guid baseId) where T : ConfigurationItem
        {
            PropertyPathHelper pathHelper = new PropertyPathHelper();
            pathHelper.PropertyName = "BaseId";
            pathHelper.ObjectClass = GetClassIdByType<T>();

            QueryCriteriaExpression expr = new QueryCriteriaExpression
            {
                PropertyName = pathHelper.ToString(),
                PropertyType = QueryCriteriaPropertyType.Property,
                Operator = QueryCriteriaExpressionOperator.Equal,
                Value = baseId.ToString("B")
            };

            QueryCriteria criteria = new QueryCriteria(GetProjectionIdByType<T>())
            {
                GroupingOperator = QueryCriteriaGroupingOperator.SimpleExpression
            };

            criteria.Expressions.Add(expr);

            List<T> retList = await TypeProjectionController.GetProjectionByCriteria<T>(authToken, ExcludeInactiveItems<T>(criteria));

            if (retList.Count == 0)
                return null;
            else if (retList.Count == 1)
                return retList[0];
            else
                throw new CiresonApiException("More than one item found with identical baseId");
        }

        /// <summary>
        /// Modifies a given QueryCriteria to exclude inactive items
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <param name="criteria">QueryCriteria to adjust</param>
        /// <returns></returns>
        internal static QueryCriteria ExcludeInactiveItems<T>(QueryCriteria criteria)
        {
            PropertyPathHelper pathHelper = new PropertyPathHelper();
            pathHelper.PropertyName = "ObjectStatus";
            pathHelper.ObjectClass = GetClassIdByType<T>();

            QueryCriteriaExpression activeItemsOnly = new QueryCriteriaExpression
            {
                PropertyName = pathHelper.ToString(),
                PropertyType = QueryCriteriaPropertyType.Property,
                Operator = QueryCriteriaExpressionOperator.Equal,
                Value = EnumerationConstants.ConfigItem.BuiltinValues.ObjectStatus.Active.ToString("D")
            };

            QueryCriteria newCriteria = criteria;

            if (newCriteria.Expressions.Count > 0)
                newCriteria.GroupingOperator = QueryCriteriaGroupingOperator.And;
            else
                newCriteria.GroupingOperator = QueryCriteriaGroupingOperator.SimpleExpression;

            newCriteria.Expressions.Add(activeItemsOnly);

            return newCriteria;
        }

        /// <summary>
        /// Returns the SCSM projection ID of the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static Guid GetProjectionIdByType<T>()
        {
            if (typeof(T) == typeof(HardwareAsset))
                return TypeProjectionConstants.HardwareAsset;
            else if (typeof(T) == typeof(Location))
                return TypeProjectionConstants.Location;
            else if (typeof(T) == typeof(PurchaseOrder))
                return TypeProjectionConstants.PurchaseOrder;
            else
                throw new CiresonApiException("Unrecognized type " + typeof(T).FullName);
        }

        /// <summary>
        /// Returns the SCSM class ID of the specified type
        /// </summary>
        /// <typeparam name="T">Class to find</typeparam>
        /// <returns></returns>
        private static Guid GetClassIdByType<T>()
        {
            if (typeof(T) == typeof(HardwareAsset))
                return ClassConstants.HardwareAsset;
            else if (typeof(T) == typeof(Location))
                return ClassConstants.Location;
            else if (typeof(T) == typeof(PurchaseOrder))
                return ClassConstants.PurchaseOrder;
            else
                throw new CiresonApiException("Unrecognized type " + typeof(T).FullName);
        }
        #endregion // General Methods
    }
}
