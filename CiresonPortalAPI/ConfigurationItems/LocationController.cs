using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiresonPortalAPI.ConfigurationItems
{
    public static class LocationController
    {
        #region Public Methods
        /// <summary>
        /// Creates a new blank Location
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="name">Name of the Location</param>
        /// <param name="displayName">DisplayName of the Location</param>
        /// <returns></returns>
        public static async Task<Location> Create(AuthorizationToken authToken, string name, string displayName)
        {
            return await TypeProjectionController.CreateBlankObject<Location>(authToken, name, displayName);
        }

        /// <summary>
        /// Gets a list of Locations based on the supplied criteria
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="criteria">QueryCriteria to search for</param>
        /// <returns></returns>
        public static async Task<List<Location>> GetByCriteria(AuthorizationToken authToken, QueryCriteria criteria)
        {
            criteria.ProjectionID = TypeProjectionConstants.Location.Id;
            return await TypeProjectionController.GetByCriteria<Location>(authToken, criteria);
        }

        /// <summary>
        /// Convenience method that gets a list of all Locations that are active
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <returns></returns>
        public static async Task<List<Location>> GetAll(AuthorizationToken authToken)
        {
            PropertyPathHelper pathHelper = new PropertyPathHelper();
            pathHelper.PropertyName = "ObjectStatus";
            pathHelper.ObjectClass = ClassConstants.GetClassIdByType<Location>();

            QueryCriteriaExpression expr = new QueryCriteriaExpression
            {
                PropertyName = pathHelper.ToString(),
                PropertyType = QueryCriteriaPropertyType.Property,
                Operator = QueryCriteriaExpressionOperator.Equal,
                Value = EnumerationConstants.TypeProjection.BuiltinValues.ObjectStatus.Active.ToString("D")
            };

            QueryCriteria criteria = new QueryCriteria(TypeProjectionConstants.Location.Id)
            {
                GroupingOperator = QueryCriteriaGroupingOperator.SimpleExpression
            };

            criteria.Expressions.Add(expr);

            return await GetByCriteria(authToken, criteria);
        }

        /// <summary>
        /// Returns a Location by ID
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="id">ID of the location</param>
        /// <returns></returns>
        public static async Task<Location> GetByBaseId(AuthorizationToken authToken, Guid id)
        {
            return await TypeProjectionController.GetByBaseId<Location>(authToken, id);
        }

        /// <summary>
        /// Returns a Location by its FullName
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="fullName">FullName to find</param>
        /// <returns></returns>
        public static async Task<Location> GetByFullName(AuthorizationToken authToken, string fullName)
        {
            return await TypeProjectionController.GetByFullName<Location>(authToken, fullName);
        }

        /// <summary>
        /// Marks a Location for deletion
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="location">Location to delete</param>
        /// <param name="markPending">If true, mark the object as Pending Deletion instead of Deleted.</param>
        /// <returns></returns>
        public static async Task<bool> Delete(AuthorizationToken authToken, Location location, bool markPending = true)
        {
            return await TypeProjectionController.DeleteObject(authToken, location, markPending);
        }
        #endregion // Public Methods
    }
}
