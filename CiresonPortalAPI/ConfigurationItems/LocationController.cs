using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiresonPortalAPI.ConfigurationItems
{
    public static class LocationController
    {
        /// <summary>
        /// Gets a list of Locations based on the supplied criteria
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="criteria">QueryCriteria to search for</param>
        /// <returns></returns>
        public static async Task<List<Location>> GetLocationsByCriteria(AuthorizationToken authToken, QueryCriteria criteria)
        {
            criteria.ProjectionID = TypeProjectionConstants.PurchaseOrder;
            return await ConfigurationItemController.GetConfigurationItemsByCriteria<Location>(authToken, criteria);
        }

        /// <summary>
        /// Gets a list of all Locations that are active
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <returns></returns>
        public static async Task<List<Location>> GetAllLocations(AuthorizationToken authToken)
        {
            QueryCriteria criteria = new QueryCriteria(TypeProjectionConstants.Location);

            return await TypeProjectionController.GetProjectionByCriteria<Location>(authToken, ConfigurationItemController.ExcludeInactiveItems<Location>(criteria));
        }

        /// <summary>
        /// Returns a Location by ID
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="id">ID of the location</param>
        /// <returns></returns>
        public static async Task<Location> GetLocationById(AuthorizationToken authToken, Guid id)
        {
            return await ConfigurationItemController.GetConfigurationItemByBaseId<Location>(authToken, id);
        }

        /// <summary>
        /// Creates a new Location
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="name">Name of the Location</param>
        /// <param name="displayName">DisplayName of the Location</param>
        /// <returns></returns>
        public static async Task<Location> CreateNewLocation(AuthorizationToken authToken, string name, string displayName)
        {
            return await ConfigurationItemController.CreateConfigurationItem<Location>(authToken, name, displayName);
        }

        /// <summary>
        /// Marks a Location for deletion
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="location">Location to delete</param>
        /// <returns></returns>
        public static async Task<bool> DeleteLocation(AuthorizationToken authToken, Location location)
        {
            return await ConfigurationItemController.DeleteConfigurationItem(authToken, location);
        }
    }
}
