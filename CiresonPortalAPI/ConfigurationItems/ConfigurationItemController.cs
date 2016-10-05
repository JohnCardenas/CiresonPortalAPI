using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiresonPortalAPI.ConfigurationItems
{
    internal static class ConfigurationItemController
    {
        #region Internal Methods
        /// <summary>
        /// Queries the Cireson Portal for objects using specified criteria.
        /// </summary>
        /// <param name="authToken">AuthenticationToken to use</param>
        /// <param name="criteria">QueryCriteria rules</param>
        /// <param name="excludeInactive">If true, exclude items pending deletion.</param>
        /// <returns>List of ConfigurationItems</returns>
        internal static async Task<List<T>> GetByCriteria<T>(AuthorizationToken authToken, QueryCriteria criteria, bool excludeInactive = true) where T : ConfigurationItem
        {
            List<T> itemList = await TypeProjectionController.GetByCriteria<T>(authToken, criteria);

            if (excludeInactive)
                itemList.RemoveAll(item => item.IsActive == false);

            return itemList;
        }

        /// <summary>
        /// Marks a ConfigurationItem as deleted
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="item">ConfigurationItem to delete</param>
        /// <param name="markPending">If true, mark the object as Pending Deletion instead of Deleted.</param>
        /// <returns></returns>
        internal static async Task<bool> DeleteObject(AuthorizationToken authToken, ConfigurationItem item, bool markPending)
        {
            Guid deleteType;

            if (markPending)
                deleteType = EnumerationConstants.ConfigItem.BuiltinValues.ObjectStatus.PendingDelete;
            else
                deleteType = EnumerationConstants.ConfigItem.BuiltinValues.ObjectStatus.Deleted;

            item.ObjectStatus = new Enumeration(deleteType, "", "", false, false);
            item.AllowCommitDeleted = true;

            bool commitState = await item.Commit(authToken);
            item.AllowCommitDeleted = false;
            return commitState;
        }
        #endregion // Internal Methods
    }
}
