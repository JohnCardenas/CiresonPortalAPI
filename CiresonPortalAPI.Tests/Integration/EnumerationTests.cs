using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CiresonPortalAPI.Tests.Integration
{
    /// <summary>
    /// Summary description for EnumerationControllerTests
    /// </summary>
    [TestClass]
    public class EnumerationTests
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
        public EnumerationTests() { }
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

        #region ENUM01_GetIncidentStatusListTest
        [TestMethod]
        [Description("Fetches a list of all Incident statuses")]
        [TestCategory("Integration - Enumerations")]
        public async Task ENUM01_GetIncidentStatusListTest()
        {
            // Arrange
            List<Enumeration> enumList;

            // Act
            enumList = await EnumerationController.GetEnumerationList(_authToken, EnumerationConstants.Incidents.Lists.Status, false, false, false);

            // Assert
            Assert.IsNotNull(enumList);
            Assert.IsTrue(enumList.Count > 0);
        }
        #endregion

        #region ENUM02_GetIncidentStatusSortedListTest
        [TestMethod]
        [Description("Fetches a sorted list of all Incident statuses")]
        [TestCategory("Integration - Enumerations")]
        public async Task ENUM02_GetIncidentStatusSortedListTest()
        {
            // Arrange
            List<Enumeration> enumList;

            // Act
            enumList = await EnumerationController.GetEnumerationList(_authToken, EnumerationConstants.Incidents.Lists.Status, false, true, false);

            // Assert
            Assert.IsNotNull(enumList);
            Assert.IsTrue(enumList.Count > 0);
        }
        #endregion

        #region ENUM03_GetIncidentStatusFlatListTest
        [TestMethod]
        [Description("Fetches a flat list of all Incident statuses")]
        [TestCategory("Integration - Enumerations")]
        public async Task ENUM03_GetIncidentStatusFlatListTest()
        {
            // Arrange
            List<Enumeration> enumList;

            // Act
            enumList = await EnumerationController.GetEnumerationList(_authToken, EnumerationConstants.Incidents.Lists.Status, true, false, false);

            // Assert
            Assert.IsNotNull(enumList);
            Assert.IsTrue(enumList.Count > 0);
        }
        #endregion

        #region ENUM04_GetIncidentStatusSortedFlatListTest
        [TestMethod]
        [Description("Fetches a flat list of all Incident statuses")]
        [TestCategory("Integration - Enumerations")]
        public async Task ENUM04_GetIncidentStatusSortedFlatListTest()
        {
            // Arrange
            List<Enumeration> enumList;

            // Act
            enumList = await EnumerationController.GetEnumerationList(_authToken, EnumerationConstants.Incidents.Lists.Status, true, true, false);

            // Assert
            Assert.IsNotNull(enumList);
            Assert.IsTrue(enumList.Count > 0);
        }
        #endregion
    }
}
