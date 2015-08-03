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


    public class HardwareAsset
    {

    }
}
