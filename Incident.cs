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
        public static async Task<List<Incident>> GetIncidentsByCriteria(AuthorizationToken authToken, QueryCriteria criteria)
        {
            if (!authToken.IsValid)
            {
                throw new InvalidCredentialException("AuthorizationToken is not valid.");
            }

            criteria.ProjectionID = TypeProjectionConstants.Incident;

            List<Incident> returnList = new List<Incident>();

            // Get the raw Incident TypeProjections and convert them
            List<ExpandoObject> rawIncidentList = await TypeProjectionController.GetProjectionByCriteria(authToken, criteria);
            foreach (ExpandoObject rawIncident in rawIncidentList)
            {
                returnList.Add(CreateIncidentFromDynamicObject(rawIncident));
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

            List<ExpandoObject> rawIncident = await TypeProjectionController.GetProjectionByCriteria(authToken, criteria);

            if (rawIncident.Count == 0)
                return null;

            return CreateIncidentFromDynamicObject(rawIncident[0]);
        }

        internal static Incident CreateIncidentFromDynamicObject(dynamic obj)
        {
            Incident returnObj = new Incident(true);

            returnObj.Classification = new Enumeration(obj.Classification.Id, obj.Classification.Name, obj.Classification.Name, true, false);
            returnObj.ClosedDate = obj.ClosedDate;
            returnObj.DisplayName = obj.DisplayName;
            returnObj.LastModifiedDate = obj.LastModified;

            returnObj._bObjectInitialized = true;
            return returnObj;
        }
    }

    public class Incident : BaseWorkItem
    {
        #region Internal data objects
        string _sDescription;
        string _sDisplayName;
        string _sID;
        string _sResolutionDescription;
        string _sTitle;

        Nullable<DateTime> _dClosedDate;
        Nullable<DateTime> _dCreatedDate;
        Nullable<DateTime> _dFirstAssignedDate;
        Nullable<DateTime> _dFirstResponseDate;
        Nullable<DateTime> _dLastModifiedDate;
        Nullable<DateTime> _dResolvedDate;
        Nullable<DateTime> _dTargetResolutionTime;

        Enumeration _oClassification;
        Enumeration _oImpact;
        Enumeration _oResolutionCategory;
        Enumeration _oSource;
        Enumeration _oStatus;
        Enumeration _oTierQueue;
        Enumeration _oUrgency;

        bool _bEscalated;

        int _iPriority;
        #endregion Internal data objects

        #region Read-Only Properties

        /// <summary>Gets the date the Incident was closed. Read only.</summary>
        public Nullable<DateTime> ClosedDate { get { return _dClosedDate; } internal set { _dClosedDate = value; } }

        /// <summary>Gets the date the Incident was created. Read only.</summary>
        public Nullable<DateTime> CreatedDate { get { return _dCreatedDate; } internal set { _dCreatedDate = value; } }

        /// <summary>Gets the DisplayName of the Incident. Read only.</summary>
        public string DisplayName { get { return _sDisplayName; } internal set { _sDisplayName = value; } }

        /// <summary>Gets the date the Incident was first assigned. Read only.</summary>
        public Nullable<DateTime> FirstAssignedDate { get { return _dFirstAssignedDate; } internal set { _dFirstAssignedDate = value; } }

        /// <summary>Gets the date the Incident was first responded to. Read only.</summary>
        public Nullable<DateTime> FirstResponseDate { get { return _dFirstResponseDate; } internal set { _dFirstResponseDate = value; } }

        /// <summary>Gets the ID of the Incident. Read only.</summary>
        public string ID { get { return _sID; } internal set { _sID = value; } }

        /// <summary>Gets the date the Incident was last modified. Read only.</summary>
        public Nullable<DateTime> LastModifiedDate { get { return _dLastModifiedDate; } internal set { _dLastModifiedDate = value; } }

        /// <summary>Gets the priority of this Incident. Read only.</summary>
        public int Priority { get { return _iPriority; } internal set { _iPriority = value; } }

        /// <summary>Gets the date the Incident was resolved. Read only.</summary>
        public Nullable<DateTime> ResolvedDate { get { return _dResolvedDate; } internal set { _dResolvedDate = value; } }

        /// <summary>Gets the Incident's target resolution time. Read only.</summary>
        public Nullable<DateTime> TargetResolutionTime { get { return _dTargetResolutionTime; } internal set { _dTargetResolutionTime = value; } }

        #endregion Read-Only Properties

        #region Read-Write Properties

        /// <summary>Gets or sets the Incident's description.</summary>
        public string Description { get { return _sDescription; } set { SetDirtyBit(); _sDescription = value; } }

        /// <summary>Gets or sets the Incident's classification.</summary>
        public Enumeration Classification { get { return _oClassification; } set { SetDirtyBit(); _oClassification = value; } }

        /// <summary>Gets or sets the Incident's escalation status.</summary>
        public bool Escalated { get { return _bEscalated; } set { SetDirtyBit(); _bEscalated = value; } }

        /// <summary>Gets or sets the Incident's impact.</summary>
        public Enumeration Impact { get { return _oImpact; } set { SetDirtyBit(); _oImpact = value; } }

        /// <summary>Gets or sets the Incident's resolution category.</summary>
        public Enumeration ResolutionCategory { get { return _oResolutionCategory; } set { SetDirtyBit(); _oResolutionCategory = value; } }

        /// <summary>Gets or sets the Incident's resolution description.</summary>
        public string ResolutionDescription { get { return _sResolutionDescription; } set { SetDirtyBit(); _sResolutionDescription = value; } }

        /// <summary>Gets or sets the Incident's source.</summary>
        public Enumeration Source { get { return _oSource; } set { SetDirtyBit(); _oSource = value; } }

        /// <summary>Gets or sets the Incident's status.</summary>
        public Enumeration Status { get { return _oStatus; } set { SetDirtyBit(); _oStatus = value; } }

        /// <summary>Gets or sets the Incident's support group.</summary>
        public Enumeration SupportGroup { get { return _oTierQueue; } set { SetDirtyBit(); _oTierQueue = value; } }

        /// <summary>Gets or sets the Incident's title.</summary>
        public string Title { get { return _sTitle; } set { SetDirtyBit(); _sTitle = value; } }

        /// <summary>Gets or sets the Incident's urgency.</summary>
        public Enumeration Urgency { get { return _oUrgency; } set { SetDirtyBit(); _oUrgency = value; } }

        #endregion Read-Write Properties

        public Incident()
        {
            _bExistingObject = false;
        }

        /// <summary>
        /// Create an Incident as an existing object, for internal use within the API library.
        /// </summary>
        /// <param name="existingObject"></param>
        internal Incident(bool existingObject)
        {
            _bExistingObject = existingObject;
        }

        public override string ToString()
        {
            return this.DisplayName;
        }
    }
}
