using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CiresonPortalAPI.WorkItems;
using CiresonPortalAPI.ConfigurationItems;

namespace CiresonPortalAPI.Tests.Integration.WorkItems
{
    /// <summary>
    /// Summary description for IncidentControllerTests
    /// </summary>
    [TestClass]
    public class IncidentTests
    {
        #region Fields
        private static AuthorizationToken _authToken;
        private static List<Incident> _incidentsToCleanup;
        private static Incident _incident;

        private TestContext _testContextInstance;
        #endregion // Fields

        #region Properties
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return _testContextInstance; }
            set { _testContextInstance = value; }
        }
        #endregion // Properties

        #region Constructor
        public IncidentTests() { }
        #endregion // Constructor

        #region Class Initializer
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            _incidentsToCleanup = new List<Incident>();

            Task<AuthorizationToken> tokenTask = AuthorizationController.GetAuthorizationToken(ConfigurationHelper.PortalUrl, ConfigurationHelper.UserName, ConfigurationHelper.Password, ConfigurationHelper.Domain);
            tokenTask.Wait();
            _authToken = tokenTask.Result;

            if (!_authToken.User.CanCreateIncident)
            {
                Assert.Fail("Must run Incident tests with an account that can create Incidents.");
            }
        }
        #endregion // Class Initializer

        #region Class Cleanup
        [ClassCleanup]
        public static void Cleanup()
        {
            foreach (Incident obj in _incidentsToCleanup)
            {
                //Task<bool> deleteTask = IncidentController.Delete(_authToken, obj, false);
            }
        }
        #endregion // Class Cleanup

        #region IR01_CreateIncidentTest
        [TestMethod]
        [TestCategory("Integration - Incidents")]
        [Description("Creates a new Incident")]
        public async Task IR01_CreateIncidentTest()
        {
            // Arrange
            Guid templateId = TemplateConstants.Incident.Default;
            Guid userId = _authToken.User.Id;

            User userData;
            Enumeration enumData;
            DateTime? dateData;
            string strData;
            bool boolData;
            int? intData;

            // Act
            _incident = await IncidentController.Create(_authToken, templateId, userId);
            _incidentsToCleanup.Add(_incident);

            try
            {
                userData = _incident.AffectedUser;
                userData = _incident.AssignedToUser;

                enumData = _incident.Classification;
                enumData = _incident.Impact;
                enumData = _incident.ResolutionCategory;
                enumData = _incident.Source;
                enumData = _incident.Status;
                enumData = _incident.SupportGroup;
                enumData = _incident.Urgency;

                dateData = _incident.ClosedDate;
                dateData = _incident.CreatedDate;
                dateData = _incident.FirstAssignedDate;
                dateData = _incident.FirstResponseDate;
                dateData = _incident.LastModifiedDate;
                dateData = _incident.ResolvedDate;
                dateData = _incident.TargetResolutionTime;

                strData = _incident.Description;
                strData = _incident.Id;
                strData = _incident.ResolutionDescription;
                strData = _incident.Title;

                boolData = _incident.Escalated;

                intData = _incident.Priority;
            }
            catch (Exception e)
            {
                Assert.Fail("Expected no exception from property read test, got " + e.Message);
            }

            // Assert
            Assert.IsNotNull(_incident);
        }
        #endregion

        #region IR02_IncidentPropertiesCommitTest
        [TestMethod]
        [TestCategory("Integration - Incidents")]
        [Description("Tests committing changes to the test Incident")]
        public async Task IR02_IncidentPropertiesCommitTest()
        {
            // Arrange
            string testString = "1234567890 abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ !@#$%^&*()-_=+[{]}\\|;:'\",<.>/?`~";

            // Act
            _incident.Title = testString;
            _incident.Description = testString;
            _incident.Classification = (await GetIncidentClassifications())[0];
            _incident.Source = (await GetIncidentSources())[0];

            await _incident.Commit(_authToken);

            // Assert
            Assert.IsNotNull(_incident);
            Assert.AreEqual(testString, _incident.Title);
            Assert.AreEqual(testString, _incident.Description);
        }
        #endregion

        #region IR03_GetIncidentByIdTest
        [TestMethod]
        [TestCategory("Integration - Incidents")]
        [Description("Attempts to fetch the test Incident by its Id")]
        public async Task IR03_GetIncidentByIdTest()
        {
            // Arrange 
            Incident inc;

            // Act
            inc = await IncidentController.GetById(_authToken, _incident.Id);

            // Assert
            Assert.IsNotNull(inc);
            Assert.AreEqual(_incident, inc);
        }
        #endregion

        #region IR04_IncidentRelatedObjectCommitTest
        [TestMethod]
        [TestCategory("Integration - Incidents")]
        [Description("Attempts to commit changes to an Incident's related objects")]
        public async Task IR04_IncidentRelatedObjectCommitTest()
        {
            // Arrange
            User tokenUser = await UserController.GetUserById(_authToken, _authToken.User.Id);

            // Act
            _incident.AffectedUser = tokenUser;
            _incident.AssignedToUser = tokenUser;

            await _incident.Commit(_authToken);

            // Assert
            Assert.AreEqual(_incident.AffectedUser, tokenUser);
            Assert.AreEqual(_incident.AssignedToUser, tokenUser);
        }
        #endregion

        public static async Task<List<Enumeration>> GetIncidentClassifications()
        {
            return await EnumerationController.GetEnumerationList(_authToken, EnumerationConstants.Incidents.Lists.Classification, false, false, false);
        }

        public static async Task<List<Enumeration>> GetIncidentSources()
        {
            return await EnumerationController.GetEnumerationList(_authToken, EnumerationConstants.Incidents.Lists.Source, false, false, false);
        }
    }
}
