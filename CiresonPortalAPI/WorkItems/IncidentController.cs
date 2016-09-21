﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiresonPortalAPI.WorkItems
{
    public static class IncidentController
    {
        #region Public Methods
        /// <summary>
        /// Creates a new Incident based on the supplied Template ID
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="templateId">TemplateID to use</param>
        /// <param name="userId">ID of the user creating the Incident</param>
        /// <returns></returns>
        public static async Task<Incident> CreateObject(AuthorizationToken authToken, Guid templateId, Guid userId)
        {
            TypeProjection projection = await TypeProjectionController.CreateObjectFromTemplate<Incident>(authToken, templateId, userId);
            return (Incident)projection;
        }

        /// <summary>
        /// Creates a new Incident based on the supplied Template ID
        /// </summary>
        /// <param name="authToken">User AuthorizationToken</param>
        /// <param name="templateId">TemplateID to use</param>
        /// <returns></returns>
        public static async Task<Incident> CreateObject(AuthorizationToken authToken, Guid templateId)
        {
            return await CreateObject(authToken, templateId, authToken.User.Id);
        }

        /// <summary>
        /// Gets a list of Incidents based on the supplied criteria
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="criteria">QueryCriteria to search for</param>
        /// <returns></returns>
        public static async Task<List<Incident>> GetByCriteria(AuthorizationToken authToken, QueryCriteria criteria)
        {
            criteria.ProjectionID = TypeProjectionConstants.Incident;
            return await TypeProjectionController.GetByCriteria<Incident>(authToken, criteria);
        }

        /// <summary>
        /// Convenience method that returns an Incident by its ID
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="incidentID">ID of the Incident to retrieve</param>
        /// <returns>Incident</returns>
        public static async Task<Incident> GetByID(AuthorizationToken authToken, string incidentID)
        {
            QueryCriteriaExpression expression = new QueryCriteriaExpression();
            expression.PropertyName = (new PropertyPathHelper(ClassConstants.Incident.Id, "ID")).ToString();
            expression.PropertyType = QueryCriteriaPropertyType.Property;
            expression.Operator = QueryCriteriaExpressionOperator.Equal;
            expression.Value = incidentID;

            QueryCriteria criteria = new QueryCriteria(TypeProjectionConstants.Incident);
            criteria.GroupingOperator = QueryCriteriaGroupingOperator.SimpleExpression;
            criteria.Expressions.Add(expression);

            List<Incident> incidentList = await IncidentController.GetByCriteria(authToken, criteria);

            if (incidentList.Count == 0)
                return null;

            return incidentList[0];
        }

        /// <summary>
        /// Marks an Incident for deletion
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <param name="incident">Incident to delete</param>
        /// <param name="markPending">If true, mark the object as Pending Deletion instead of Deleted.</param>
        /// <returns></returns>
        public static async Task<bool> DeleteObject(AuthorizationToken authToken, Incident incident, bool markPending = true)
        {
            return await TypeProjectionController.DeleteObject(authToken, incident, markPending);
        }
        #endregion // Public Methods
    }
}
