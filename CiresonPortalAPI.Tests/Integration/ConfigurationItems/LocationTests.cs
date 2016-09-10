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
        private static Location _locObject;

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

            // Save
            _locObject = locationList[0];
        }
        #endregion

        #region LOC02_LocationPropertiesTest
        [TestMethod]
        [TestCategory("Integration - Location")]
        [Description("Tests exposing data from the underlying data model as properties")]
        public void LOC02_LocationPropertiesTest()
        {
            // Arrange
            string strData;
            Guid guidData;
            Enumeration enumData;

            // Act
            try
            {
                strData = _locObject.AddressLine1;
                strData = _locObject.AddressLine2;
                strData = _locObject.City;
                strData = _locObject.ContactEmail;
                strData = _locObject.ContactFax;
                strData = _locObject.ContactName;
                strData = _locObject.ContactPhone;
                strData = _locObject.Country;
                strData = _locObject.DisplayName;
                strData = _locObject.Notes;
                strData = _locObject.PostalCode;
                strData = _locObject.State;

                guidData = _locObject.Id;

                enumData = _locObject.ObjectStatus;
            }
            // Assert
            catch (Exception e)
            {
                Assert.Fail("Expected no exception, got " + e.Message);
            }

            Assert.IsTrue(_locObject.ObjectStatus.Id == EnumerationConstants.ConfigItem.BuiltinValues.ObjectStatus.Active);
        }
        #endregion

        #region LOC03_LocationRelatedObjectsTest
        [TestMethod]
        [TestCategory("Integration - Location")]
        [Description("Tests retrieving related objects")]
        public void LOC03_LocationRelatedObjectsTest()
        {
            // Arrange
            Location parent;
            List<Location> children;

            // Act
            try
            {
                parent = _locObject.Parent;
                children = _locObject.Children;
            }
            // Assert
            catch (Exception e)
            {
                Assert.Fail("Expected no exception, got " + e.Message);
            }
        }
        #endregion
    }
}
