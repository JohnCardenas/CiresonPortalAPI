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
        /// <summary>Gets the DisplayName of the Work Item. Read only.</summary>
        public string DisplayName
        {
            get { return this.GetPrimitiveValue<string>("DisplayName"); }
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
            if (!includeInactiveItems)
            {
                PropertyPathHelper pathHelper = new PropertyPathHelper();
                pathHelper.PropertyName = "ObjectStatus";

                if (typeof(T) == typeof(HardwareAsset))
                    pathHelper.ObjectClass = ClassConstants.HardwareAsset;
                else if (typeof(T) == typeof(Location))
                    pathHelper.ObjectClass = ClassConstants.Location;
                else if (typeof(T) == typeof(PurchaseOrder))
                    pathHelper.ObjectClass = ClassConstants.PurchaseOrder;
                else
                    throw new CiresonApiException("Unrecognized type " + typeof(T).FullName);

                QueryCriteriaExpression activeItemsOnly = new QueryCriteriaExpression
                {
                    PropertyName = pathHelper.ToString(),
                    PropertyType = QueryCriteriaPropertyType.Property,
                    Operator = QueryCriteriaExpressionOperator.Equal,
                    Value = EnumerationConstants.ConfigItem.BuiltinValues.ObjectStatus.Active.ToString("B")
                };

                criteria.GroupingOperator = QueryCriteriaGroupingOperator.And;
                criteria.Expressions.Add(activeItemsOnly);
            }

            return await TypeProjectionController.GetProjectionByCriteria<T>(authToken, criteria);
        }
        #endregion // General Methods
    }
}
