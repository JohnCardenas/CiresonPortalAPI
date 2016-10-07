using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using CiresonPortalAPI.ConfigurationItems;

namespace CiresonPortalAPI.WorkItems
{
    public class Incident : WorkItem
    {
        #region Read-Only Properties
        /// <summary>
        /// Gets the date the Incident was closed. Read only.
        /// </summary>
        public DateTime? ClosedDate
        {
            get { return this.GetPrimitiveValue<DateTime?>("ClosedDate"); }
        }

        /// <summary>
        /// Gets the date the Incident was created. Read only.
        /// </summary>
        public DateTime? CreatedDate
        {
            get { return this.GetPrimitiveValue<DateTime?>("CreatedDate"); }
        }

        /// <summary>
        /// Gets the date the Incident was first assigned. Read only.
        /// </summary>
        public DateTime? FirstAssignedDate
        {
            get { return this.GetPrimitiveValue<DateTime?>("FirstAssignedDate"); }
        }

        /// <summary>
        /// Gets the date the Incident was last modified. Read only.
        /// </summary>
        public DateTime? LastModifiedDate
        {
            get { return this.GetPrimitiveValue<DateTime?>("LastModified"); }
        }

        /// <summary>
        /// Gets the priority of this Incident. Read only.
        /// </summary>
        public int? Priority
        {
            get { return this.GetPrimitiveValue<int?>("Priority"); }
        }

        /// <summary>
        /// Gets the date the Incident was resolved. Read only.
        /// </summary>
        public DateTime? ResolvedDate
        {
            get { return this.GetPrimitiveValue<DateTime?>("ResolvedDate"); }
        }

        /// <summary>
        /// Gets the Incident's target resolution time. Read only.
        /// </summary>
        public DateTime? TargetResolutionTime
        {
            get { return this.GetPrimitiveValue<DateTime?>("TargetResolutionTime"); }
        }
        #endregion Read-Only Properties

        #region Read-Write Properties
        /// <summary>
        /// Gets or sets the Incident's description.
        /// </summary>
        public string Description
        {
            get { return this.GetPrimitiveValue<string>("Description"); }
            set { this.SetPrimitiveValue<string>("Description", value); }
        }

        /// <summary>
        /// Gets or sets the Incident's classification.
        /// </summary>
        public Enumeration Classification
        {
            get { return this.GetEnumeration("Classification"); }
            set { this.SetEnumeration("Classification", value); }
        }

        /// <summary>
        /// Gets or sets the Incident's escalation status.
        /// </summary>
        public bool Escalated
        {
            get { return this.GetPrimitiveValue<bool>("Escalated"); }
            set { this.SetPrimitiveValue("Escalated", value); }
        }

        /// <summary>
        /// Gets or sets the date the Incident was first responded to (Acknowledged).
        /// </summary>
        public DateTime? FirstResponseDate
        {
            get { return this.GetPrimitiveValue<DateTime?>("FirstResponseDate"); }
            set { this.SetPrimitiveValue("FirstResponseDate", value); }
        }

        /// <summary>
        /// Gets or sets the Incident's impact.
        /// </summary>
        public Enumeration Impact
        {
            get { return this.GetEnumeration("Impact"); }
            set { this.SetEnumeration("Impact", value); }
        }

        /// <summary>
        /// Gets or sets the Incident's resolution category.
        /// </summary>
        public Enumeration ResolutionCategory
        {
            get { return this.GetEnumeration("ResolutionCategory"); }
            set { this.SetEnumeration("ResolutionCategory", value); }
        }

        /// <summary>
        /// Gets or sets the Incident's resolution description.
        /// </summary>
        public string ResolutionDescription
        {
            get { return this.GetPrimitiveValue<string>("ResolutionDescription"); }
            set { this.SetPrimitiveValue<string>("ResolutionDescription", value); }
        }

        /// <summary>
        /// Gets or sets the Incident's source.
        /// </summary>
        public Enumeration Source
        {
            get { return this.GetEnumeration("Source"); }
            set { this.SetEnumeration("Source", value); }
        }

        /// <summary>
        /// Gets or sets the Incident's status.
        /// </summary>
        public Enumeration Status
        {
            get { return this.GetEnumeration("Status"); }
            set { this.SetEnumeration("Status", value); }
        }

        /// <summary>
        /// Gets or sets the Incident's support group.
        /// </summary>
        public Enumeration SupportGroup
        {
            get { return this.GetEnumeration("TierQueue"); }
            set { this.SetEnumeration("TierQueue", value); }
        }

        /// <summary>
        /// Gets or sets the Incident's urgency.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the assigned user.
        /// </summary>
        public User AssignedToUser
        {
            get { return this.GetRelatedObject<User>("AssignedWorkItem"); }
            set { this.SetRelatedObject("AssignedWorkItem", value, "AssignedToUser"); }
        }
        #endregion Relationship Properties

        #region Constructors
        internal Incident(ExpandoObject obj, bool existingObject = false, bool readOnly = true) : base(obj, existingObject, readOnly) { }
        internal Incident() : base() { }
        #endregion

        #region Public Methods
        /// <summary>
        /// Refreshes the Incident, discarding any changes that may have been made.
        /// This method must be called before accessing properties of children in relationship collections in order to populate all properties.
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <returns></returns>
        public override async Task<bool> Refresh(AuthorizationToken authToken)
        {
            return await this.RefreshType<Incident>(authToken);
        }
        #endregion // Public Methods
    }
}
