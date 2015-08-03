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
            return await TypeProjectionController.GenericToSpecific<Incident>(authToken, criteria);
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
            TypeProjection projection = await TypeProjectionController.CreateProjectionByTemplate(authToken, templateId, userId);
            return new Incident(projection);
        }
    }

    public class Incident : TypeProjection
    {
        private User _oAffectedUser   = null;
        private User _oAssignedToUser = null;

        #region Read-Only Properties

        /// <summary>Gets the date the Incident was closed. Read only.</summary>
        public Nullable<DateTime> ClosedDate { get { return DynamicObjectHelpers.GetProperty<Nullable<DateTime>>(_oCurrentObject, "ClosedDate"); } }

        /// <summary>Gets the date the Incident was created. Read only.</summary>
        public Nullable<DateTime> CreatedDate { get { return DynamicObjectHelpers.GetProperty<Nullable<DateTime>>(_oCurrentObject, "CreatedDate"); } }

        /// <summary>Gets the DisplayName of the Incident. Read only.</summary>
        public string DisplayName { get { return DynamicObjectHelpers.GetProperty<string>(_oCurrentObject, "DisplayName"); } }

        /// <summary>Gets the date the Incident was first assigned. Read only.</summary>
        public Nullable<DateTime> FirstAssignedDate { get { return DynamicObjectHelpers.GetProperty<Nullable<DateTime>>(_oCurrentObject, "FirstAssignedDate"); } }

        /// <summary>Gets the date the Incident was first responded to. Read only.</summary>
        public Nullable<DateTime> FirstResponseDate { get { return DynamicObjectHelpers.GetProperty<Nullable<DateTime>>(_oCurrentObject, "FirstResponseDate"); } }

        /// <summary>Gets the ID of the Incident. Read only.</summary>
        public string Id { get { return DynamicObjectHelpers.GetProperty<string>(_oCurrentObject, "Id"); } }

        /// <summary>Gets the date the Incident was last modified. Read only.</summary>
        public Nullable<DateTime> LastModifiedDate { get { return DynamicObjectHelpers.GetProperty<Nullable<DateTime>>(_oCurrentObject, "LastModified"); } }

        /// <summary>Gets the priority of this Incident. Read only.</summary>
        public int Priority { get { return DynamicObjectHelpers.GetProperty<int>(_oCurrentObject, "Priority"); } }

        /// <summary>Gets the date the Incident was resolved. Read only.</summary>
        public Nullable<DateTime> ResolvedDate { get { return DynamicObjectHelpers.GetProperty<Nullable<DateTime>>(_oCurrentObject, "ResolvedDate"); } }

        /// <summary>Gets the Incident's target resolution time. Read only.</summary>
        public Nullable<DateTime> TargetResolutionTime { get { return DynamicObjectHelpers.GetProperty<Nullable<DateTime>>(_oCurrentObject, "TargetResolutionTime"); } }

        #endregion Read-Only Properties

        #region Read-Write Properties

        /// <summary>Gets or sets the Incident's description.</summary>
        public string Description { get { return DynamicObjectHelpers.GetProperty<string>(_oCurrentObject, "Description"); } set { _oCurrentObject.Description = value; SetDirtyBit(); } }

        /// <summary>Gets or sets the Incident's classification.</summary>
        public Enumeration Classification
        {
            get
            {
                return DeserializeEnumeration(_oCurrentObject.Classification.Id, _oCurrentObject.Classification.Name);
            }
            set
            {
                SetEnumerationValue(_oCurrentObject.Classification, value);
                SetDirtyBit();
            }
        }

        /// <summary>Gets or sets the Incident's escalation status.</summary>
        public bool Escalated { get { return DynamicObjectHelpers.GetProperty<bool>(_oCurrentObject, "Escalated"); } set { _oCurrentObject.Escalated = value; SetDirtyBit(); } }

        /// <summary>Gets or sets the Incident's impact.</summary>
        public Enumeration Impact
        {
            get
            {
                return DeserializeEnumeration(_oCurrentObject.Impact.Id, _oCurrentObject.Impact.Name);
            }
            set
            {
                SetEnumerationValue(_oCurrentObject.Impact, value);
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
                SetEnumerationValue(_oCurrentObject.ResolutionCategory, value);
                SetDirtyBit();
            }
        }

        /// <summary>Gets or sets the Incident's resolution description.</summary>
        public string ResolutionDescription { get { return DynamicObjectHelpers.GetProperty<string>(_oCurrentObject, "ResolutionDescription"); } set { _oCurrentObject.ResolutionDescription = value; SetDirtyBit(); } }

        /// <summary>Gets or sets the Incident's source.</summary>
        public Enumeration Source
        {
            get
            {
                return DeserializeEnumeration(_oCurrentObject.Source.Id, _oCurrentObject.Source.Name);
            }
            set
            {
                SetEnumerationValue(_oCurrentObject.Source, value);
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
                SetEnumerationValue(_oCurrentObject.Status, value);
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
                SetEnumerationValue(_oCurrentObject.TierQueue, value);
                SetDirtyBit();
            }
        }

        /// <summary>Gets or sets the Incident's title.</summary>
        public string Title { get { return DynamicObjectHelpers.GetProperty<string>(_oCurrentObject, "Title"); } set { _oCurrentObject.Title = value; SetDirtyBit(); } }

        /// <summary>Gets or sets the Incident's urgency.</summary>
        public Enumeration Urgency
        {
            get
            {
                return DeserializeEnumeration(_oCurrentObject.Urgency.Id, _oCurrentObject.Urgency.Name);
            }
            set
            {
                SetEnumerationValue(_oCurrentObject.Urgency, value);
                SetDirtyBit();
            }
        }

        #endregion Read-Write Properties

        #region Relationship Properties

        /// <summary>
        /// Returns the properties of the Affected User. Read only.
        /// </summary>
        public User AffectedUser { get { return _oAffectedUser; } set { SetAffectedUser(value); } }

        public User AssignedToUser { get { return _oAssignedToUser; } set { SetAssignedToUser(value); } }

        #endregion Relationship Properties

        #region Relationship Setters

        /// <summary>
        /// Sets the Affected User of this Incident.
        /// </summary>
        /// <param name="user">User to set as the Affected User</param>
        private void SetAffectedUser(User user)
        {
            // Set the new Affected User
            _oCurrentObject.RequestedWorkItem = user._oUserObj;
            _oAffectedUser = new User(_oCurrentObject.RequestedWorkItem);
        }

        /// <summary>
        /// Sets the Assigned To User of this Incident.
        /// </summary>
        /// <param name="user"></param>
        private void SetAssignedToUser(User user)
        {
            // Set the new Assigned To User
            _oCurrentObject.AssignedWorkItem = user._oUserObj;
            _oAssignedToUser = new User(_oCurrentObject.AssignedWorkItem);
        }

        #endregion Relationship Setters

        #region Constructors

        /// <summary>
        /// Constructor used internally when an existing object has been queried
        /// </summary>
        /// <param name="projection">Parent type projection</param>
        internal Incident(TypeProjection projection)
        {
            _oOriginalObject = projection._oOriginalObject;
            _oCurrentObject = projection._oCurrentObject;
            _bReadOnly = false;

            // Related objects
            if (DynamicObjectHelpers.HasProperty(_oCurrentObject, "RequestedWorkItem"))
                _oAffectedUser = new User(_oCurrentObject.RequestedWorkItem);
        }

        #endregion Constructors

        public override string ToString()
        {
            return this.DisplayName;
        }
    }
}
