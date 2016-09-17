using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CiresonPortalAPI.ConfigurationItems;

namespace CiresonPortalAPI.Tests.Integration.ConfigurationItems
{
    /// <summary>
    /// Summary description for UserTests
    /// </summary>
    [TestClass]
    public class UserTests
    {
        #region Fields
        private static AuthorizationToken _authToken;
        private static Guid _userId;
        private static User _userObject;

        private TestContext _testContextInstance;
        #endregion // Fields

        #region Constructor
        public UserTests() { }
        #endregion // Constructor

        #region Properties
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return _testContextInstance;  }
            set { _testContextInstance = value; }
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

        #region USER01_GetUserListTest
        [TestMethod]
        [TestCategory("Integration - Users")]
        [Description("Tests a fetch of user objects with a set limit")]
        public async Task USER01_GetUserListTest()
        {
            // Arrange
            const int MAX_USERS = 5; // Set this low to make sure the basic get list function works
            List<User> userList;

            // Act
            userList = await UserController.GetUserList(_authToken, "", false, false, MAX_USERS, true);

            // Assert
            Assert.AreEqual(MAX_USERS, userList.Count);
        }
        #endregion

        #region USER02_GetAnalystUserListTest
        [TestMethod]
        [TestCategory("Integration - Users")]
        [Description("Tests fetching only analyst users")]
        public async Task USER02_GetAnalystUserListTest()
        {
            // Arrange
            List<User> userList;

            // Act
            userList = await UserController.GetUserList(_authToken, "", true, false, 1);

            // Assert
            Assert.AreEqual(1, userList.Count);
            Assert.IsNotNull(userList[0]);
        }
        #endregion

        #region USER03_GetUserByFilterTest
        [TestMethod]
        [TestCategory("Integration - Users")]
        [Description("Fetches a single user as indicated by the UserFilter property in App.config")]
        public async Task USER03_GetUserByFilterTest()
        {
            // Arrange
            List<User> userList;

            // Act
            userList = await UserController.GetUserList(_authToken, ConfigurationHelper.UserFilter, false, false, 1);
            _userObject = userList[0];

            // Assert
            Assert.IsNotNull(_userObject);
            Assert.IsTrue(_userObject.IsPartialUser);
        }
        #endregion

        #region USER04_ExpandPartialUserTest
        [TestMethod]
        [TestCategory("Integration - Users")]
        [Description("Expands a partial user to include the full set of attributes")]
        public async Task USER04_ExpandPartialUserTest()
        {
            // Arrange

            // Act
            bool expandTest = await _userObject.FetchFullAttributes(_authToken);

            // Assert
            Assert.IsTrue(expandTest);
            Assert.IsFalse(string.IsNullOrEmpty(_userObject.OrganizationalUnit));
        }
        #endregion

        #region USER05_RefreshUserObjectTest
        [TestMethod]
        [TestCategory("Integration - Users")]
        [Description("Tests refresh functionality")]
        public async Task USER05_RefreshUserObjectTest()
        {
            // Arrange

            // Act
            await _userObject.Refresh(_authToken);

            // Assert
            Assert.IsFalse(_userObject.IsDirty);
        }
        #endregion

        #region USER06_UserPropertiesTest
        [TestMethod]
        [TestCategory("Integration - Users")]
        [Description("Tests exposing data from the underlying data model as properties")]
        public void USER06_UserPropertiesTest()
        {
            // Arrange
            string strData;
            Guid guidData;
            Enumeration enumData;

            // Act
            try
            {
                strData = _userObject.BusinessPhone;
                strData = _userObject.BusinessPhone2;
                strData = _userObject.City;
                strData = _userObject.Company;
                strData = _userObject.Country;
                strData = _userObject.Department;
                strData = _userObject.DisplayName;
                strData = _userObject.DistinguishedName;
                strData = _userObject.Domain;
                strData = _userObject.Email;
                strData = _userObject.EmployeeId;
                strData = _userObject.Fax;
                strData = _userObject.FirstName;
                strData = _userObject.FQDN;
                strData = _userObject.HomePhone;
                strData = _userObject.HomePhone2;
                strData = _userObject.Initials;
                strData = _userObject.LastName;
                strData = _userObject.Mobile;
                strData = _userObject.Notes;
                strData = _userObject.Office;
                strData = _userObject.OrganizationalUnit;
                strData = _userObject.Pager;
                strData = _userObject.State;
                strData = _userObject.StreetAddress;
                strData = _userObject.Title;
                strData = _userObject.UPN;
                strData = _userObject.UserName;
                strData = _userObject.Zip;

                guidData = _userObject.BaseId;

                enumData = _userObject.ObjectStatus;
            }
            // Assert
            catch (Exception e)
            {
                Assert.Fail("Expected no exception, got " + e.Message);
            }
        }
        #endregion

        #region USER07_UserRelatedObjectsTest
        [TestMethod]
        [TestCategory("Integration - Users")]
        [Description("Tests retrieving related objects from the User retrieved from the UserFilter property in App.config")]
        public void USER07_UserRelatedObjectsTest()
        {
            // Arrange
            User manager;

            // Act
            try
            {
                manager = _userObject.Manager;
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
