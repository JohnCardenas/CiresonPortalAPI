using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CiresonPortalAPI.WorkItems;

namespace CiresonPortalAPI.Tests.Integration.WorkItems
{
    /// <summary>
    /// Summary description for IncidentControllerTests
    /// </summary>
    [TestClass]
    public class IncidentTests
    {
        #region Fields
        private TestContext _testContextInstance;
        private static AuthorizationToken _authToken;
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
        }
        #endregion // Class Initializer

        #region IR01_CreateIncidentTest
        [TestMethod]
        [Description("Creates a new Incident")]
        [TestCategory("Integration - Incidents")]
        public async Task IR01_CreateIncidentTest()
        {
            // Arrange
            Incident inc;
            Guid templateId = TemplateConstants.Incident.Default;
            Guid userId;

            // Act
            userId = (await AuthorizationController.GetUserRights(_authToken)).Id;
            inc = await IncidentController.CreateObject(_authToken, templateId, userId);

            // Assert
            Assert.IsNotNull(inc);
        }
        #endregion
    }
}
