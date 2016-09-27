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
        private static List<Location> _objectsToCleanup;
        private static AuthorizationToken _authToken;
        private static Location _location;

        private TestContext _testContextInstance;
        #endregion // Fields

        #region Constructor
        public LocationTests()
        {
        }
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
            _objectsToCleanup = new List<Location>();

            Task<AuthorizationToken> tokenTask = AuthorizationController.GetAuthorizationToken(ConfigurationHelper.PortalUrl, ConfigurationHelper.UserName, ConfigurationHelper.Password, ConfigurationHelper.Domain);
            tokenTask.Wait();
            _authToken = tokenTask.Result;
        }
        #endregion // Class Initializer

        #region Class Cleanup
        [ClassCleanup]
        public static void Cleanup()
        {
            foreach (Location obj in _objectsToCleanup)
            {
                LocationController.Delete(_authToken, obj, false).Wait();
            }
        }
        #endregion // Class Cleanup

        #region LOC01_CreateLocationTest
        [TestMethod]
        [TestCategory("Integration - Locations")]
        [Description("Tests creating a new Location")]
        public async Task LOC01_CreateLocationTest()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            string strData;
            Guid guidData;
            Enumeration enumData;

            // Act
            _location = await LocationController.Create(_authToken, "TestLocation" + id.ToString(), "Test Location");
            _objectsToCleanup.Add(_location);

            try
            {
                strData = _location.AddressLine1;
                strData = _location.AddressLine2;
                strData = _location.City;
                strData = _location.ContactEmail;
                strData = _location.ContactFax;
                strData = _location.ContactName;
                strData = _location.ContactPhone;
                strData = _location.Country;
                strData = _location.DisplayName;
                strData = _location.Notes;
                strData = _location.PostalCode;
                strData = _location.State;

                guidData = _location.BaseId;

                enumData = _location.ObjectStatus;
            }
            // Assert
            catch (Exception e)
            {
                Assert.Fail("Expected no exception from property read test, got " + e.Message);
            }

            Assert.IsNotNull(_location);
            Assert.IsTrue(_location.IsActive);
        }
        #endregion

        #region LOC02_LocationPropertiesCommitTest
        [TestMethod]
        [TestCategory("Integration - Locations")]
        [Description("Tests committing changes to this Location")]
        public async Task LOC02_LocationPropertiesCommitTest()
        {
            // Arrange
            string testString = "1234567890 abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ !@#$%^&*()-_=+[{]}\\|;:'\",<.>/?`~";

            // Act
            _location.AddressLine1 = testString;
            _location.AddressLine2 = testString;
            _location.City = testString;
            _location.ContactEmail = testString;
            _location.ContactFax = testString;
            _location.ContactName = testString;
            _location.ContactPhone = testString;
            _location.Country = testString;
            _location.Notes = testString;
            _location.PostalCode = testString;
            _location.State = testString;

            await _location.Commit(_authToken);

            // Assert
            Assert.IsNotNull(_location);
            Assert.AreEqual(testString, _location.AddressLine1);
            Assert.AreEqual(testString, _location.AddressLine2);
            Assert.AreEqual(testString, _location.City);
            Assert.AreEqual(testString, _location.ContactEmail);
            Assert.AreEqual(testString, _location.ContactFax);
            Assert.AreEqual(testString, _location.ContactName);
            Assert.AreEqual(testString, _location.ContactPhone);
            Assert.AreEqual(testString, _location.Country);
            Assert.AreEqual(testString, _location.Notes);
            Assert.AreEqual(testString, _location.PostalCode);
            Assert.AreEqual(testString, _location.State);
        }
        #endregion

        #region LOC03_GetAllLocationsTest
        [TestMethod]
        [TestCategory("Integration - Locations")]
        [Description("Tests a fetch of all location objects")]
        public async Task LOC03_GetAllLocationsTest()
        {
            // Arrange
            List<Location> locationList;

            // Act
            locationList = await LocationController.GetAll(_authToken);

            // Assert
            Assert.IsNotNull(locationList);
            Assert.IsTrue(locationList.Count >= 1);
            Assert.IsTrue(locationList.Contains(_location));
        }
        #endregion

        #region LOC04_LocationRelatedObjectCommitTest
        [TestMethod]
        [TestCategory("Integration - Locations")]
        [Description("Tests committing related objects")]
        public async Task LOC04_LocationRelatedObjectCommitTest()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            Location parentLocation = await LocationController.Create(_authToken, "TestParentLocation" + id.ToString(), "Test Parent Location");
            Location childLocation1 = await LocationController.Create(_authToken, "TestChildLocation1" + id.ToString(), "Test Child Location 1");
            Location childLocation2 = await LocationController.Create(_authToken, "TestChildLocation2" + id.ToString(), "Test Child Location 2");

            _objectsToCleanup.Add(parentLocation);
            _objectsToCleanup.Add(childLocation1);
            _objectsToCleanup.Add(childLocation2);

            // Act
            _location.Parent = parentLocation;
            _location.Children.Add(childLocation1);
            _location.Children.Add(childLocation2);

            await _location.Commit(_authToken);

            // Assert
            Assert.AreEqual(parentLocation, _location.Parent);
            Assert.IsTrue(_location.Children.Contains(childLocation1));
            Assert.IsTrue(_location.Children.Contains(childLocation2));
        }
        #endregion

        #region LOC99_DeleteLocationTest
        [TestMethod]
        [TestCategory("Integration - Locations")]
        [Description("Tests marking a Location as deleted")]
        public async Task LOC99_DeleteLocationTest()
        {
            // Arrange
            bool deleteLocation;

            // Act
            deleteLocation = await LocationController.Delete(_authToken, _location, false);

            // Assert
            Assert.IsTrue(deleteLocation);
            Assert.IsTrue(_location.IsDeleted);

            if (deleteLocation)
                _objectsToCleanup.Remove(_location);
        }
        #endregion
    }
}
