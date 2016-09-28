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

            // Act
            _location = await LocationController.Create(_authToken, "TestLocation" + id.ToString(), "Test Location");
            _objectsToCleanup.Add(_location);

            // Assert
            Assert.IsNotNull(_location, "Failed to create a new Location");
            Assert.IsTrue(_location.IsActive, "Location.IsActive evaluated to false");
        }
        #endregion

        #region LOC02_LocationReadPropertyTest
        [TestMethod]
        [TestCategory("Integration - Locations")]
        [Description("Tests reading Location Properties")]
        public void LOC02_LocationReadPropertyTest()
        {
            // Arrange
            string strData;
            Guid guidData;
            Enumeration enumData;
            Location parent;
            RelatedObjectList<Location> children;

            // Act
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

                parent = _location.Parent;

                children = _location.Children;
            }

            // Assert
            catch (Exception e)
            {
                Assert.Fail("Expected no exception from property read test, got " + e.Message);
            }
        }
        #endregion

        #region LOC03_SetLocationPrimitivesCommitTest
        [TestMethod]
        [TestCategory("Integration - Locations")]
        [Description("Tests committing changes to Location primitive properties")]
        public async Task LOC03_SetLocationPrimitivesCommitTest()
        {
            // Arrange
            string testString = "1234567890 abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ !@#$%^&*()-_=+[{]}\\|;:'\",<.>/?`~";

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

            // Act
            await _location.Commit(_authToken);

            // Assert
            Assert.AreEqual(testString, _location.AddressLine1, "Location.AddressLine1 does not match the test data");
            Assert.AreEqual(testString, _location.AddressLine2, "Location.AddressLine2 does not match the test data");
            Assert.AreEqual(testString, _location.City,         "Location.City does not match the test data");
            Assert.AreEqual(testString, _location.ContactEmail, "Location.ContactEmail does not match the test data");
            Assert.AreEqual(testString, _location.ContactFax,   "Location.ContactFax does not match the test data");
            Assert.AreEqual(testString, _location.ContactName,  "Location.ContactName does not match the test data");
            Assert.AreEqual(testString, _location.ContactPhone, "Location.ContactPhone does not match the test data");
            Assert.AreEqual(testString, _location.Country,      "Location.Country does not match the test data");
            Assert.AreEqual(testString, _location.Notes,        "Location.Notes does not match the test data");
            Assert.AreEqual(testString, _location.PostalCode,   "Location.PostalCode does not match the test data");
            Assert.AreEqual(testString, _location.State,        "Location.State does not match the test data");
        }
        #endregion

        #region LOC04_ClearLocationPrimitivesCommitTest()
        [TestMethod]
        [TestCategory("Integration - Locations")]
        [Description("Tests clearing and committing Location primitive properties")]
        public async Task LOC04_ClearLocationPrimitivesCommitTest()
        {
            // Arrange
            _location.AddressLine1 = null;
            _location.AddressLine2 = null;
            _location.City = null;
            _location.ContactEmail = null;
            _location.ContactFax = null;
            _location.ContactName = null;
            _location.ContactPhone = null;
            _location.Country = null;
            _location.Notes = null;
            _location.PostalCode = null;
            _location.State = null;

            // Act
            await _location.Commit(_authToken);

            // Assert
            Assert.IsTrue(string.IsNullOrEmpty(_location.AddressLine1), "Location.AddressLine1 was not cleared successfully");
            Assert.IsTrue(string.IsNullOrEmpty(_location.AddressLine2), "Location.AddressLine2 was not cleared successfully");
            Assert.IsTrue(string.IsNullOrEmpty(_location.City),         "Location.City was not cleared successfully");
            Assert.IsTrue(string.IsNullOrEmpty(_location.ContactEmail), "Location.ContactEmail was not cleared successfully");
            Assert.IsTrue(string.IsNullOrEmpty(_location.ContactFax),   "Location.ContactFax was not cleared successfully");
            Assert.IsTrue(string.IsNullOrEmpty(_location.ContactName),  "Location.ContactName was not cleared successfully");
            Assert.IsTrue(string.IsNullOrEmpty(_location.ContactPhone), "Location.ContactPhone was not cleared successfully");
            Assert.IsTrue(string.IsNullOrEmpty(_location.Country),      "Location.Country was not cleared successfully");
            Assert.IsTrue(string.IsNullOrEmpty(_location.Notes),        "Location.Notes was not cleared successfully");
            Assert.IsTrue(string.IsNullOrEmpty(_location.PostalCode),   "Location.PostalCode was not cleared successfully");
            Assert.IsTrue(string.IsNullOrEmpty(_location.State),        "Location.State was not cleared successfully");
        }
        #endregion

        #region LOC05_GetAllLocationsTest
        [TestMethod]
        [TestCategory("Integration - Locations")]
        [Description("Tests a fetch of all location objects")]
        public async Task LOC05_GetAllLocationsTest()
        {
            // Arrange
            List<Location> locationList;

            // Act
            locationList = await LocationController.GetAll(_authToken);

            // Assert
            Assert.IsNotNull(locationList, "Expected a List<Location>, got null");
            Assert.IsTrue(locationList.Count >= 1, "Expected at least one member of this list, got " + locationList.Count);
            Assert.IsTrue(locationList.Contains(_location), "List does not contain the test location");
        }
        #endregion

        #region LOC06_SetLocationParentCommitTest
        [TestMethod]
        [TestCategory("Integration - Locations")]
        [Description("Performs a commit test on a Location.Parent")]
        public async Task LOC06_SetLocationParentCommitTest()
        {
            // Arrange
            Guid testId = Guid.NewGuid();
            Location parent = await LocationController.Create(_authToken, "TestParentLocation" + testId.ToString(), "Test Parent Location");
            _objectsToCleanup.Add(parent);
            _location.Parent = parent;

            // Act
            await _location.Commit(_authToken);

            // Assert
            Assert.AreEqual(parent, _location.Parent, "Location.Parent does not match the test data");
        }
        #endregion

        #region LOC07_ClearLocationParentCommitTest
        [TestMethod]
        [TestCategory("Integration - Locations")]
        [Description("Clears and commits the Location.Parent property")]
        public async Task LOC07_ClearLocationParentCommitTest()
        {
            // Arrange
            _location.Parent = null;

            // Act
            await _location.Commit(_authToken);

            // Assert
            Assert.IsNull(_location.Parent, "Location.Parent was not cleared successfully");
        }
        #endregion

        #region LOC08_SetLocationChildrenCommitTest
        [TestMethod]
        [TestCategory("Integration - Locations")]
        [Description("Tests committing related objects")]
        public async Task LOC08_SetLocationChildrenCommitTest()
        {
            // Arrange
            Guid testId = Guid.NewGuid();
            Location childLocation1 = await LocationController.Create(_authToken, "TestChildLocation1" + testId.ToString(), "Test Child Location 1");
            Location childLocation2 = await LocationController.Create(_authToken, "TestChildLocation2" + testId.ToString(), "Test Child Location 2");

            _objectsToCleanup.Add(childLocation1);
            _objectsToCleanup.Add(childLocation2);

            _location.Children.Add(childLocation1);
            _location.Children.Add(childLocation2);

            // Act
            await _location.Commit(_authToken);

            // Assert
            Assert.AreEqual(2, _location.Children.Count, "Expected 2 Location.Children, got " + _location.Children.Count);
            Assert.IsTrue(_location.Children.Contains(childLocation1), "Location.Children does not contain the first test child");
            Assert.IsTrue(_location.Children.Contains(childLocation2), "Location.Children does not contain the second test child");
        }
        #endregion

        #region LOC09_ClearLocationChildrenCommitTest
        [TestMethod]
        [TestCategory("Integration - Locations")]
        [Description("Clears and commits Location.Children")]
        public async Task LOC09_ClearLocationChildrenCommitTest()
        {
            // Arrange
            _location.Children.Clear();

            // Act
            await _location.Commit(_authToken);

            // Assert
            Assert.AreEqual(0, _location.Children.Count, "Expected 0 Location.Children, got " + _location.Children.Count);
        }
        #endregion

        #region LOC99_DeleteLocationTest
        [TestMethod]
        [TestCategory("Integration - Locations")]
        [Description("Tests marking a Location as deleted")]
        public async Task LOC99_DeleteLocationTest()
        {
            // Arrange

            // Act
            await LocationController.Delete(_authToken, _location, false);

            // Assert
            Assert.IsTrue(_location.IsDeleted, "Location.IsDeleted evaluated to false");

            if (_location.IsDeleted)
                _objectsToCleanup.Remove(_location);
        }
        #endregion
    }
}
