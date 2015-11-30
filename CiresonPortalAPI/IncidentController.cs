using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiresonPortalAPI
{
    public static class IncidentController
    {
        /// <summary>
        /// Gets a list of Incidents based on the supplied criteria
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="criteria">QueryCriteria to search for</param>
        /// <returns></returns>
        public static async Task<List<Incident>> GetIncidentsByCriteria(AuthorizationToken authToken, QueryCriteria criteria)
        {
            criteria.ProjectionID = TypeProjectionConstants.Incident;
            return await TypeProjectionController.GetProjectionByCriteria<Incident>(authToken, criteria);
        }

        /// <summary>
        /// Convenience method that returns an Incident by its ID
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="incidentID">ID of the Incident to retrieve</param>
        /// <returns>Incident</returns>
        public static async Task<Incident> GetIncidentByID(AuthorizationToken authToken, string incidentID)
        {
            QueryCriteriaExpression expression = new QueryCriteriaExpression();
            expression.PropertyName = (new PropertyPathHelper(ClassConstants.Incident, "ID")).ToString();
            expression.PropertyType = QueryCriteriaPropertyType.Property;
            expression.Operator = QueryCriteriaExpressionOperator.Equal;
            expression.Value = incidentID;

            QueryCriteria criteria = new QueryCriteria(TypeProjectionConstants.Incident);
            criteria.GroupingOperator = QueryCriteriaGroupingOperator.SimpleExpression;
            criteria.Expressions.Add(expression);

            List<Incident> incidentList = await IncidentController.GetIncidentsByCriteria(authToken, criteria);

            if (incidentList.Count == 0)
                return null;

            return incidentList[0];
        }

        /// <summary>
        /// Creates a new Incident based on the supplied Template ID
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="templateId">TemplateID to use</param>
        /// <param name="userId">ID of the user creating the Incident</param>
        /// <returns></returns>
        public static async Task<Incident> CreateNewIncident(AuthorizationToken authToken, Guid templateId, Guid userId)
        {
            TypeProjection projection = await TypeProjectionController.CreateProjectionByTemplate<Incident>(authToken, templateId, userId);
            return (Incident)projection;
        }
    }
}
