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
            
            // Act
            _incident = await IncidentController.Create(_authToken, templateId, userId);
            
            // Assert
            Assert.IsNotNull(_incident, "Failed to create a new Incident");
        }
        #endregion

        #region IR02_IncidentReadPropertyTest
        [TestMethod]
        [TestCategory("Integration - Incidents")]
        [Description("Tests reading all Incident properties")]
        public void IR02_IncidentReadPropertyTest()
        {
            // Arrange
            User userData;
            Enumeration enumData;
            DateTime? dateData;
            string strData;
            bool boolData;
            int? intData;

            // Act
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

            // Assert
            catch (Exception e)
            {
                Assert.Fail("Expected no exception from property read test, got " + e.Message);
            }
        }
        #endregion

        #region IR03_SetIncidentEnumerationCommitTest
        [TestMethod]
        [TestCategory("Integration - Incidents")]
        [Description("Tests setting enumeration changes to an Incident")]
        public async Task IR03_SetIncidentEnumerationCommitTest()
        {
            // Arrange
            Enumeration classification = (await GetIncidentClassifications())[0];
            Enumeration source = (await GetIncidentSources())[0];
            _incident.Classification = classification;
            _incident.Source = source;

            // Act
            await _incident.Commit(_authToken);

            // Assert
            Assert.AreEqual(classification, _incident.Classification, "Incident.Classification does not match test data");
            Assert.AreEqual(source, _incident.Source,                 "Incident.Source does not match test data");
        }
        #endregion

        #region IR04_ClearIncidentEnumerationCommitTest
        [TestMethod]
        [TestCategory("Integration - Incidents")]
        [Description("Clears and commits Incident enumerations")]
        public async Task IR04_ClearIncidentEnumerationCommitTest()
        {
            // Arrange
            _incident.Classification = null;
            _incident.Source = null;

            // Act
            await _incident.Commit(_authToken);

            // Assert
            Assert.IsNull(_incident.Classification, "Incident.Classification was not cleared successfully");
            Assert.IsNull(_incident.Source,         "Incident.Source was not cleared successfully");
        }
        #endregion

        #region IR05_SetIncidentPrimitivesCommitTest
        [TestMethod]
        [TestCategory("Integration - Incidents")]
        [Description("Tests committing changes Incident primitive properties")]
        public async Task IR05_SetIncidentPrimitivesCommitTest()
        {
            // Arrange
            string testString = "1234567890 abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ !@#$%^&*()-_=+[{]}\\|;:'\",<.>/?`~";
            DateTime testDate = DateTime.Parse(DateTime.Now.ToString()); // Convert current time to string first to remove unnecessary precision from the Ticks property

            _incident.Title = testString;
            _incident.Description = testString;
            _incident.FirstResponseDate = testDate;

            // Act
            await _incident.Commit(_authToken);

            // Assert
            Assert.AreEqual(testString, _incident.Title,             "Incident.Title does not match test data");
            Assert.AreEqual(testString, _incident.Description,       "Incident.Description does not match test data");
            Assert.AreEqual(testDate,   _incident.FirstResponseDate, "Incident.FirstResponseDate does not match test data");
        }
        #endregion

        #region IR06_ClearIncidentPrimitivesCommitTest
        [TestMethod]
        [TestCategory("Integration - Incidents")]
        [Description("Clears and commits Incident primitive properties")]
        public async Task IR06_ClearIncidentPrimitivesCommitTest()
        {
            // Arrange
            _incident.Title = null;
            _incident.Description = null;

            // Act
            await _incident.Commit(_authToken);

            // Assert
            Assert.IsTrue(string.IsNullOrEmpty(_incident.Title),       "Incident.Title was not cleared successfully");
            Assert.IsTrue(string.IsNullOrEmpty(_incident.Description), "Incident.Description was not cleared successfully");
        }
        #endregion
        
        #region IR07_GetIncidentByIdTest
        [TestMethod]
        [TestCategory("Integration - Incidents")]
        [Description("Attempts to fetch the test Incident by its Id")]
        public async Task IR07_GetIncidentByIdTest()
        {
            // Arrange 
            Incident inc;

            // Act
            inc = await IncidentController.GetById(_authToken, _incident.Id);

            // Assert
            Assert.IsNotNull(inc, "Failed to fetch the Incident by its WorkItemID");
            Assert.AreEqual(_incident, inc, "Failed to fetch a matching Incident");
        }
        #endregion

        #region IR08_SetIncidentAffectedUserCommitTest
        [TestMethod]
        [TestCategory("Integration - Incidents")]
        [Description("Sets and commits Incident.AffectedUser")]
        public async Task IR08_SetIncidentAffectedUserCommitTest()
        {
            // Arrange
            User user = await UserController.GetUserById(_authToken, _authToken.User.Id);
            _incident.AffectedUser = user;

            // Act
            await _incident.Commit(_authToken);

            // Assert
            Assert.AreEqual(user, _incident.AffectedUser, "Incident.AffectedUser does not match test data");
        }
        #endregion

        #region IR09_ClearIncidentAffectedUserCommitTest
        [TestMethod]
        [TestCategory("Integration - Incidents")]
        [Description("Clears and commits Incident.AffectedUser")]
        public async Task IR09_ClearIncidentAffectedUserCommitTest()
        {
            // Arrange
            _incident.AffectedUser = null;

            // Act
            await _incident.Commit(_authToken);

            // Assert
            Assert.IsNull(_incident.AffectedUser, "Incident.AffectedUser was not cleared successfully");
        }
        #endregion

        #region IR10_SetIncidentAssignedToUserCommitTest
        [TestMethod]
        [TestCategory("Integration - Incidents")]
        [Description("Sets and commits Incident.AssignedToUser")]
        public async Task IR10_SetIncidentAssignedToUserCommitTest()
        {
            // Arrange
            User user = await UserController.GetUserById(_authToken, _authToken.User.Id);
            _incident.AssignedToUser = user;

            // Act
            await _incident.Commit(_authToken);

            // Assert
            Assert.AreEqual(user, _incident.AssignedToUser, "Incident.AssignedToUser does not match test data");
        }
        #endregion

        #region IR11_ClearIncidentAssignedToUserCommitTest
        [TestMethod]
        [TestCategory("Integration - Incidents")]
        [Description("Clears and commits Incident.AssignedToUser")]
        public async Task IR11_ClearIncidentAssignedToUserCommitTest()
        {
            // Arrange
            _incident.AssignedToUser = null;

            // Act
            await _incident.Commit(_authToken);

            // Assert
            Assert.IsNull(_incident.AssignedToUser, "Incident.AssignedToUser was not cleared successfully");
        }
        #endregion
        
        #region IR99_CloseIncidentTest
        [TestMethod]
        [TestCategory("Integration - Incidents")]
        [Description("Attempts to close an Incident")]
        public async Task IR99_IncidentCloseTest()
        {
            // Arrange
            _incident.Status = new Enumeration(EnumerationConstants.Incidents.BuiltinValues.Status.Closed, "Closed", "Closed", false, false);

            // Act
            bool status = await _incident.Commit(_authToken);

            // Assert
            Assert.IsTrue(status);
            Assert.AreEqual(EnumerationConstants.Incidents.BuiltinValues.Status.Closed, _incident.Status.Id);
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
