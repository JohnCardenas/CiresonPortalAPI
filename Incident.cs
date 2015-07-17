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
            if (!authToken.IsValid)
            {
                throw new InvalidCredentialException("AuthorizationToken is not valid.");
            }

            criteria.ProjectionID = TypeProjectionConstants.Incident;

            List<Incident> returnList = new List<Incident>();

            // Get the raw Incident TypeProjections and convert them
            List<TypeProjection> projectionList = await TypeProjectionController.GetProjectionByCriteria(authToken, criteria);
            foreach (TypeProjection projection in projectionList)
            {
                returnList.Add(new Incident(projection));
            }

            return returnList;
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
            expression.PropertyName = "$Context/Property[Type='a604b942-4c7b-2fb2-28dc-61dc6f465c68']/28b1c58f-aefa-a449-7496-4805186bd94f$";
            expression.PropertyType = QueryCriteriaPropertyType.Property;
            expression.Operator = QueryCriteriaExpressionOperator.Equal;
            expression.Value = incidentID;

            QueryCriteria criteria = new QueryCriteria(TypeProjectionConstants.Incident);
            criteria.GroupingOperator = QueryCriteriaGroupingOperator.SimpleExpression;
            criteria.Expressions.Add(expression);

            List<TypeProjection> projectionList = await TypeProjectionController.GetProjectionByCriteria(authToken, criteria);

            if (projectionList.Count == 0)
                return null;

            return new Incident(projectionList[0]);
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
            TypeProjection projection = await TypeProjectionController.CreateProjectionByTemplate(authToken, templateId, userId);
            return new Incident(projection);
        }
    }

    public class Incident : TypeProjection
    {
        #region Read-Only Properties

        /// <summary>Gets the date the Incident was closed. Read only.</summary>
        public Nullable<DateTime> ClosedDate { get { return _oCurrentObject.ClosedDate; } }

        /// <summary>Gets the date the Incident was created. Read only.</summary>
        public Nullable<DateTime> CreatedDate { get { return _oCurrentObject.CreatedDate; } }

        /// <summary>Gets the DisplayName of the Incident. Read only.</summary>
        public string DisplayName { get { return _oCurrentObject.DisplayName; } }

        /// <summary>Gets the date the Incident was first assigned. Read only.</summary>
        public Nullable<DateTime> FirstAssignedDate { get { return _oCurrentObject.FirstAssignedDate; } }

        /// <summary>Gets the date the Incident was first responded to. Read only.</summary>
        public Nullable<DateTime> FirstResponseDate { get { return _oCurrentObject.FirstResponseDate; } }

        /// <summary>Gets the ID of the Incident. Read only.</summary>
        public string Id { get { return _oCurrentObject.Id; } }

        /// <summary>Gets the date the Incident was last modified. Read only.</summary>
        public Nullable<DateTime> LastModifiedDate { get { return _oCurrentObject.LastModified; } }

        /// <summary>Gets the priority of this Incident. Read only.</summary>
        public int Priority { get { return _oCurrentObject.Priority; } }

        /// <summary>Gets the date the Incident was resolved. Read only.</summary>
        public Nullable<DateTime> ResolvedDate { get { return _oCurrentObject.ResolvedDate; } }

        /// <summary>Gets the Incident's target resolution time. Read only.</summary>
        public Nullable<DateTime> TargetResolutionTime { get { return _oCurrentObject.TargetResolutionTime; } }

        #endregion Read-Only Properties

        #region Read-Write Properties

        /// <summary>Gets or sets the Incident's description.</summary>
        public string Description { get { return _oCurrentObject.Description; } set { _oCurrentObject.Description = value; SetDirtyBit(); } }

        /// <summary>Gets or sets the Incident's classification.</summary>
        public Enumeration Classification
        {
            get
            {
                return DeserializeEnumeration(_oCurrentObject.Classification.Id, _oCurrentObject.Classification.Name);
            }
            set
            {
                _oCurrentObject.Classification.Id = value.Id;
                _oCurrentObject.Classification.Name = value.Name;
                SetDirtyBit();
            }
        }

        /// <summary>Gets or sets the Incident's escalation status.</summary>
        public bool Escalated { get { return _oCurrentObject.Escalated; } set { _oCurrentObject.Escalated = value; SetDirtyBit(); } }

        /// <summary>Gets or sets the Incident's impact.</summary>
        public Enumeration Impact
        {
            get
            {
                return DeserializeEnumeration(_oCurrentObject.Impact.Id, _oCurrentObject.Impact.Name);
            }
            set
            {
                _oCurrentObject.Impact.Id = value.Id;
                _oCurrentObject.Impact.Name = value.Name;
                SetDirtyBit();
            }
        }

        /// <summary>Gets or sets the Incident's resolution category.</summary>
        public Enumeration ResolutionCategory
        {
            get
            {
                return DeserializeEnumeration(_oCurrentObject.ResolutionCategory.Id, _oCurrentObject.ResolutionCategory.Name);
            }
            set
            {
                _oCurrentObject.ResolutionCategory.Id = value.Id;
                _oCurrentObject.ResolutionCategory.Name = value.Name;
                SetDirtyBit();
            }
        }

        /// <summary>Gets or sets the Incident's resolution description.</summary>
        public string ResolutionDescription { get { return _oCurrentObject.ResolutionDescription; } set { _oCurrentObject.ResolutionDescription = value; SetDirtyBit(); } }

        /// <summary>Gets or sets the Incident's source.</summary>
        public Enumeration Source
        {
            get
            {
                return DeserializeEnumeration(_oCurrentObject.Source.Id, _oCurrentObject.Source.Name);
            }
            set
            {
                _oCurrentObject.Source.Id = value.Id;
                _oCurrentObject.Source.Name = value.Name;
                SetDirtyBit();
            }
        }

        /// <summary>Gets or sets the Incident's status.</summary>
        public Enumeration Status
        {
            get
            {
                return DeserializeEnumeration(_oCurrentObject.Status.Id, _oCurrentObject.Status.Name);
            }
            set
            {
                _oCurrentObject.Status.Id = value.Id;
                _oCurrentObject.Status.Name = value.Name;
                SetDirtyBit();
            }
        }

        /// <summary>Gets or sets the Incident's support group.</summary>
        public Enumeration SupportGroup
        {
            get
            {
                return DeserializeEnumeration(_oCurrentObject.TierQueue.Id, _oCurrentObject.TierQueue.Name);
            }
            set
            {
                _oCurrentObject.TierQueue.Id = value.Id;
                _oCurrentObject.TierQueue.Name = value.Name;
                SetDirtyBit();
            }
        }

        /// <summary>Gets or sets the Incident's title.</summary>
        public string Title { get { return _oCurrentObject.Title; } set { _oCurrentObject.Title = value; SetDirtyBit(); } }

        /// <summary>Gets or sets the Incident's urgency.</summary>
        public Enumeration Urgency
        {
            get
            {
                return DeserializeEnumeration(_oCurrentObject.Urgency.Id, _oCurrentObject.Urgency.Name);
            }
            set
            {
                _oCurrentObject.Urgency.Id = value.Id;
                _oCurrentObject.Urgency.Name = value.Name;
                SetDirtyBit();
            }
        }

        #endregion Read-Write Properties

        #region Constructors

        /// <summary>
        /// Constructor used internally when an existing object has been queried
        /// </summary>
        /// <param name="projection">Parent type projection</param>
        internal Incident(TypeProjection projection)
        {
            _oOriginalObject = projection._oOriginalObject;
            _oCurrentObject = projection._oCurrentObject;
        }

        #endregion Constructors

        public override string ToString()
        {
            return this.DisplayName;
        }
    }
}
