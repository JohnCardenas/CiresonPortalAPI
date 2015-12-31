using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiresonPortalAPI.ConfigurationItems
{
    internal static class ConfigurationItemController
    {
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
            QueryCriteriaExpression expr = new QueryCriteriaExpression
            {
                PropertyName = "Id",
                PropertyType = QueryCriteriaPropertyType.GenericProperty,
                Operator = QueryCriteriaExpressionOperator.Equal,
                Value = baseId.ToString("D")
            };

            QueryCriteria criteria = new QueryCriteria(TypeProjectionConstants.GetProjectionIdByType<T>())
            {
                GroupingOperator = QueryCriteriaGroupingOperator.SimpleExpression
            };

            criteria.Expressions.Add(expr);

            List<T> retList = await TypeProjectionController.GetProjectionByCriteria<T>(authToken, criteria);

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
            pathHelper.ObjectClass = ClassConstants.GetClassIdByType<T>();

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
        /// Creates a new ConfigurationItem of derived type T and returns it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="authToken"></param>
        /// <returns></returns>
        internal static async Task<T> CreateConfigurationItem<T>(AuthorizationToken authToken, string name, string displayName, dynamic objProps = null) where T : ConfigurationItem
        {
            dynamic ci = new ExpandoObject();

            ci.formJson = new ExpandoObject();
            ci.formJson.isDirty = true;
            ci.formJson.original = null;

            ci.formJson.current = new ExpandoObject();
            ci.formJson.current.BaseId = null;
            ci.formJson.current.ClassTypeId = ClassConstants.GetClassIdByType<T>();
            ci.formJson.current.ClassName = ClassConstants.GetClassNameByType<T>();
            ci.formJson.current.Name = name;
            ci.formJson.current.DisplayName = displayName;
            ci.formJson.current.TimeAdded = "0001-01-01T00:00:00";

            ci.formJson.current.ObjectStatus = new ExpandoObject();
            ci.formJson.current.ObjectStatus.Id = EnumerationConstants.ConfigItem.BuiltinValues.ObjectStatus.Active;

            // Merge another property object in
            if (objProps != null)
            {
                IDictionary<string, object> ciDict = (IDictionary<string, object>)ci.formJson.current;

                foreach (var property in objProps.GetType().GetProperties())
                {
                    if (property.CanRead)
                    {
                        ciDict[property.Name] = property.GetValue(objProps);
                    }
                }
            }

            // Create the new configuration item, then return the full-property object
            T newCI = await TypeProjectionController.CreateProjectionByData<T>(authToken, ci);
            return await ConfigurationItemController.GetConfigurationItemByBaseId<T>(authToken, newCI.Id);
        }
    }
}
