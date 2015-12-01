using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Authentication;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using CiresonPortalAPI.ConfigurationItems;

namespace CiresonPortalAPI
{
    /// <summary>
    /// A ConsoleUser represents an authorized user's security rights and preferences
    /// </summary>
    public class ConsoleUser
    {
        private dynamic _oConsoleUser;
        private List<Enumeration> _lIncidentSupportGroups;

        #region User Properties
        /// <summary>
        /// Returns the ID of the user. Read only.
        /// </summary>
        public Guid Id { get { return new Guid((String)_oConsoleUser.Id); } }

        /// <summary>
        /// Returns the name of the user. Read only.
        /// </summary>
        public string Name { get { return _oConsoleUser.Name; } }

        /// <summary>
        /// Distinguished name of the user. Read only.
        /// </summary>
        public string PrincipalName { get { return _oConsoleUser.PrincipalName; } }

        /// <summary>
        /// User name. Read only.
        /// </summary>
        public string UserName { get { return _oConsoleUser.UserName; } }

        /// <summary>
        /// User's domain. Read only.
        /// </summary>
        public string Domain { get { return _oConsoleUser.Domain; } }

        /// <summary>
        /// User's company. Read only.
        /// </summary>
        public string Company { get { return _oConsoleUser.Company; } }

        /// <summary>
        /// Job title of the user. Read only.
        /// </summary>
        public string Title { get { return _oConsoleUser.Title; } }

        /// <summary>
        /// User's first name. Read only.
        /// </summary>
        public string FirstName { get { return _oConsoleUser.FirstName; } }

        /// <summary>
        /// User's last name. Read only.
        /// </summary>
        public string LastName { get { return _oConsoleUser.LastName; } }

        /// <summary>
        /// User's employee ID. Read only.
        /// </summary>
        public string EmployeeId { get { return _oConsoleUser.EmployeeId; } }

        /// <summary>
        /// List of Incident Support Groups for which this user is a member. Read only.
        /// </summary>
        public List<Enumeration> IncidentSupportGroups { get { return _lIncidentSupportGroups; } internal set { _lIncidentSupportGroups = value; } }

        #endregion User Properties

        #region User Roles

        /// <summary>
        /// Is this user an analyst? Read only.
        /// </summary>
        public bool IsAnalyst { get { return Convert.ToBoolean(_oConsoleUser.Analyst); } }

        /// <summary>
        ///  Is this user an asset manager? Read only.
        /// </summary>
        public bool IsAssetManager { get { return Convert.ToBoolean(_oConsoleUser.AssetManager); } }

        /// <summary>
        /// Is this user a knowledge manager? Read only.
        /// </summary>
        public bool IsKnowledgeManager { get { return Convert.ToBoolean(_oConsoleUser.KnowledgeManager); } }

        /// <summary>
        /// Is this user an admin? Read only.
        /// </summary>
        public bool IsAdmin { get { return _oConsoleUser.IsAdmin; } }

        /// <summary>
        /// Is this user a localization manager? Read only.
        /// </summary>
        public bool IsLocalizationManager { get { return _oConsoleUser.IsLocalizationManager; } }

        #endregion User Roles

        #region Security

        /// <summary>
        /// Can this user create an Incident? Read only.
        /// </summary>
        public bool CanCreateIncident { get { return _oConsoleUser.Security.CanCreateIncident; } }

        /// <summary>
        /// Can this user create a Service Request? Read only.
        /// </summary>
        public bool CanCreateServiceRequest { get { return _oConsoleUser.Security.CanCreateServiceRequest; } }

        /// <summary>
        /// Can this user create a Change Request? Read only.
        /// </summary>
        public bool CanCreateChangeRequest { get { return _oConsoleUser.Security.CanCreateChangeRequest; } }

        /// <summary>
        /// Can this user create a Problem? Read only.
        /// </summary>
        public bool CanCreateProblem { get { return _oConsoleUser.Security.CanCreateProblem; } }

        /// <summary>
        /// Can this user create a Release Record? Read only.
        /// </summary>
        public bool CanCreateReleaseRecord { get { return _oConsoleUser.Security.CanCreateReleaseRecord; } }

        /// <summary>
        /// Can this user edit a Manual Activity? Read only.
        /// </summary>
        public bool CanEditManualActivity { get { return _oConsoleUser.Security.CanEditManualActivity; } }

        /// <summary>
        /// Can this user mark a Manual Activity as Complete? Read only.
        /// </summary>
        public bool CanCompleteManualActivity { get { return _oConsoleUser.Security.CanCompleteManualActivity; } }

        /// <summary>
        /// Can this user mark a Manual Activity as Failed? Read only.
        /// </summary>
        public bool CanFailManualActivity { get { return _oConsoleUser.Security.CanFailManualActivity; } }

        /// <summary>
        /// Can this user edit a Review Activity? Read only.
        /// </summary>
        public bool CanEditReviewActivity { get { return _oConsoleUser.Security.CanEditReviewActivity; } }

        /// <summary>
        /// Can this user mark a Review Activity as Approved? Read only.
        /// </summary>
        public bool CanApproveReviewActivity { get { return _oConsoleUser.Security.CanApproveReviewActivity; } }

        /// <summary>
        /// Can this user mark a Review Activity as Rejected? Read only.
        /// </summary>
        public bool CanRejectReviewActivity { get { return _oConsoleUser.Security.CanRejectReviewActivity; } }

        /// <summary>
        /// Can this user add a reviewer to a Review Activity? Read only.
        /// </summary>
        public bool CanAddReviewerReviewActivity { get { return _oConsoleUser.Security.CanAddReviewerReviewActivity; } }

        /// <summary>
        /// Can this user edit a reviewer in a Review Activity? Read only.
        /// </summary>
        public bool CanEditReviewerReviewActivity { get { return _oConsoleUser.Security.CanEditReviewerReviewActivity; } }

        /// <summary>
        /// Can this user delete a reviewer from a Review Activity? Read only.
        /// </summary>
        public bool CanDeleteReviewerReviewActivity { get { return _oConsoleUser.Security.CanDeleteReviewerReviewActivity; } }

        /// <summary>
        /// Is this user scoped to only a subset of work items? Read only.
        /// </summary>
        public bool IsWorkItemScoped { get { return _oConsoleUser.Security.IsWorkItemScoped; } }

        /// <summary>
        /// Is this user scoped to only a subset of config items? Read only.
        /// </summary>
        public bool IsConfigItemScoped { get { return _oConsoleUser.Security.IsConfigItemScoped; } }

        /// <summary>
        /// Is this user scoped to only a subset of the service catalog? Read only.
        /// </summary>
        public bool IsServiceCatalogScoped { get { return _oConsoleUser.Security.IsServiceCatalogScoped; } }

        #endregion Security

        #region Work Item Forms

        /// <summary>
        /// What Incident form should be presented to the user? Read only.
        /// </summary>
        public Guid IncidentForm { get { return new Guid((String)_oConsoleUser.IncidentForm); } }

        /// <summary>
        /// What Service Request form should be presented to the user? Read only.
        /// </summary>
        public Guid ServiceRequestForm { get { return new Guid((String)_oConsoleUser.ServiceRequestForm); } }

        /// <summary>
        /// What Change Request form should be presented to the user? Read only.
        /// </summary>
        public Guid ChangeRequestForm { get { return new Guid((String)_oConsoleUser.ChangeRequestForm); } }

        /// <summary>
        /// What Problem form should be presented to the user? Read only.
        /// </summary>
        public Guid ProblemForm { get { return new Guid((String)_oConsoleUser.ProblemForm); } }

        /// <summary>
        /// What Release Record form should be presented to the user? Read only.
        /// </summary>
        public Guid ReleaseRecordForm { get { return new Guid((String)_oConsoleUser.ReleaseRecordForm); } }

        /// <summary>
        /// What projection ID should the user use to create Incidents? Read only.
        /// </summary>
        public Guid IncidentFormProjectionId { get { return new Guid((String)_oConsoleUser.IncidentFormProjectionId); } }

        /// <summary>
        /// What projection ID should the user use to create Service Requests? Read only.
        /// </summary>
        public Guid ServiceRequestFormProjectionId { get { return new Guid((String)_oConsoleUser.ServiceRequestFormProjectionId); } }

        /// <summary>
        /// What projection ID should the user use to create Problems? Read only.
        /// </summary>
        public Guid ProblemFormProjectionId { get { return new Guid((String)_oConsoleUser.ProblemFormProjectionId); } }

        /// <summary>
        /// What projection ID should the user use to create Release Records? Read only.
        /// </summary>
        public Guid ReleaseRecordFormProjectionId { get { return new Guid((String)_oConsoleUser.ReleaseRecordFormProjectionId); } }

        /// <summary>
        /// What projection ID should the user use to create Change Requests? Read only.
        /// </summary>
        public Guid ChangeRequestFormProjectionId { get { return new Guid((String)_oConsoleUser.ChangeRequestFormProjectionId); } }

        #endregion

        #region User Preferences

        /// <summary>
        /// What language code should this user have? Read only.
        /// </summary>
        public string LanguageCode { get { return _oConsoleUser.LanguageCode; } }

        #endregion User Preferences

        #region Constructors

        internal ConsoleUser(dynamic obj)
        {
            _oConsoleUser = obj;
        }

        #endregion Constructors

    }
}
