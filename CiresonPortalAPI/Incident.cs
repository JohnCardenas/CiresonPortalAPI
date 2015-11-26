using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CiresonPortalAPI
{
    public static partial class IncidentController
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

    public class Incident : WorkItem
    {
        #region Read-Only Properties
        /// <summary>Gets the date the Incident was closed. Read only.</summary>
        public DateTime? ClosedDate
        {
            get { return this.GetPrimitiveValue<DateTime?>("ClosedDate"); }
        }

        /// <summary>Gets the date the Incident was created. Read only.</summary>
        public DateTime? CreatedDate
        {
            get { return this.GetPrimitiveValue<DateTime?>("CreatedDate"); }
        }

        /// <summary>Gets the date the Incident was first assigned. Read only.</summary>
        public DateTime? FirstAssignedDate
        {
            get { return this.GetPrimitiveValue<DateTime?>("FirstAssignedDate"); }
        }

        /// <summary>Gets the date the Incident was first responded to. Read only.</summary>
        public DateTime? FirstResponseDate
        {
            get { return this.GetPrimitiveValue<DateTime?>("FirstResponseDate"); }
        }

        /// <summary>Gets the ID of the Incident. Read only.</summary>
        public string Id
        {
            get { return this.GetPrimitiveValue<string>("Id"); }
        }

        /// <summary>Gets the date the Incident was last modified. Read only.</summary>
        public DateTime? LastModifiedDate
        {
            get { return this.GetPrimitiveValue<DateTime?>("LastModified"); }
        }

        /// <summary>Gets the priority of this Incident. Read only.</summary>
        public int Priority
        {
            get { return this.GetPrimitiveValue<int>("Priority"); }
        }

        /// <summary>Gets the date the Incident was resolved. Read only.</summary>
        public DateTime? ResolvedDate
        {
            get { return this.GetPrimitiveValue<DateTime?>("ResolvedDate"); }
        }

        /// <summary>Gets the Incident's target resolution time. Read only.</summary>
        public DateTime? TargetResolutionTime
        {
            get { return this.GetPrimitiveValue<DateTime?>("TargetResolutionTime"); }
        }
        #endregion Read-Only Properties

        #region Read-Write Properties
        /// <summary>Gets or sets the Incident's description.</summary>
        public string Description
        {
            get { return this.GetPrimitiveValue<string>("Description"); }
            set { this.SetPrimitiveValue<string>("Description", value); }
        }

        /// <summary>Gets or sets the Incident's classification.</summary>
        public Enumeration Classification
        {
            get { return this.GetEnumeration("Classification"); }
            set { this.SetEnumeration("Classification", value); }
        }

        /// <summary>Gets or sets the Incident's escalation status.</summary>
        public bool Escalated
        {
            get { return this.GetPrimitiveValue<bool>("Escalated"); }
            set { this.SetPrimitiveValue("Escalated", value); }
        }

        /// <summary>Gets or sets the Incident's impact.</summary>
        public Enumeration Impact
        {
            get { return this.GetEnumeration("Impact"); }
            set { this.SetEnumeration("Impact", value); }
        }

        /// <summary>Gets or sets the Incident's resolution category.</summary>
        public Enumeration ResolutionCategory
        {
            get { return this.GetEnumeration("ResolutionCategory"); }
            set { this.SetEnumeration("ResolutionCategory", value); }
        }

        /// <summary>Gets or sets the Incident's resolution description.</summary>
        public string ResolutionDescription
        {
            get { return this.GetPrimitiveValue<string>("ResolutionDescription"); }
            set { this.SetPrimitiveValue<string>("ResolutionDescription", value); }
        }

        /// <summary>Gets or sets the Incident's source.</summary>
        public Enumeration Source
        {
            get { return this.GetEnumeration("Source"); }
            set { this.SetEnumeration("Source", value); }
        }

        /// <summary>Gets or sets the Incident's status.</summary>
        public Enumeration Status
        {
            get { return this.GetEnumeration("Status"); }
            set { this.SetEnumeration("Status", value); }
        }

        /// <summary>Gets or sets the Incident's support group.</summary>
        public Enumeration SupportGroup
        {
            get { return this.GetEnumeration("SupportGroup"); }
            set { this.SetEnumeration("SupportGroup", value); }
        }

        /// <summary>Gets or sets the Incident's title.</summary>
        public string Title
        {
            get { return this.GetPrimitiveValue<string>("Title"); }
            set { this.SetPrimitiveValue<string>("Title", value); }
        }

        /// <summary>Gets or sets the Incident's urgency.</summary>
        public Enumeration Urgency
        {
            get { return this.GetEnumeration("Urgency"); }
            set { this.SetEnumeration("Urgency", value); }
        }
        #endregion Read-Write Properties

        #region Relationship Properties

        /// <summary>
        /// Gets or sets the affected user.
        /// </summary>
        public User AffectedUser
        {
            get { return this.GetRelatedObject<User>("RequestedWorkItem"); }
            set { this.SetRelatedObject("RequestedWorkItem", value, "AffectedUser"); }
        }

        public User AssignedToUser
        {
            get { return this.GetRelatedObject<User>("AssignedWorkItem"); }
            set { this.SetRelatedObject("AssignedWorkItem", value, "AssignedToUser"); }
        }
        #endregion Relationship Properties
    }
}
