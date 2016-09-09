using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CiresonPortalAPI.ConfigurationItems;

namespace CiresonPortalAPI.Tests.Integration.ConfigurationItems
{
    /// <summary>
    /// Summary description for LocationTests
    /// </summary>
    [TestClass]
    public class LocationTests
    {
        #region Fields
        private static AuthorizationToken _authToken;

        private TestContext _testContextInstance;
        #endregion // Fields

        #region Constructor
        public LocationTests() { }
        #endregion // Constructor

        #region Properties
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return _testContextInstance;
            }
            set
            {
                _testContextInstance = value;
            }
        }
        #endregion // Properties

        #region Class Initializer
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            Task<AuthorizationToken> tokenTask = AuthorizationController.GetAuthorizationToken(ConfigurationHelper.PortalUrl, ConfigurationHelper.UserName, ConfigurationHelper.Password, ConfigurationHelper.Domain);
            tokenTask.Wait();
            _authToken = tokenTask.Result;
        }
        #endregion // Class Initializer

        #region LOC01_GetAllLocationsTest
        [TestMethod]
        [TestCategory("Integration - LocationController")]
        [Description("Tests a fetch of all location objects")]
        public async Task LOC01_GetAllLocationsTest()
        {
            // Arrange
            List<Location> locationList;

            // Act
            locationList = await LocationController.GetAllLocations(_authToken);

            // Assert
            Assert.IsNotNull(locationList);
            Assert.IsTrue(locationList.Count >= 1);
        }
        #endregion
    }
}
