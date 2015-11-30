using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CiresonPortalAPI.Authorization;

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
            return await TypeProjectionController.GetProjectionByCriteria<Location>(authToken, criteria);
        }
    }
}
