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
        public static AuthorizationToken _authToken;
        public static Guid _userId;
        public static User _userObject;

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

        #region Test Cases
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            Task<AuthorizationToken> tokenTask = AuthorizationController.GetAuthorizationToken(ConfigurationHelper.PortalUrl, ConfigurationHelper.UserName, ConfigurationHelper.Password, ConfigurationHelper.Domain);
            tokenTask.Wait();
            _authToken = tokenTask.Result;
        }

        [TestMethod]
        [TestCategory("Integration - UserController")]
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

        [TestMethod]
        [TestCategory("Integration - UserController")]
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

        [TestMethod]
        [TestCategory("Integration - UserController")]
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

        [TestMethod]
        [TestCategory("Integration - User")]
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

        [TestMethod]
        [TestCategory("Integration - User")]
        [Description("Tests refresh functionality")]
        public async Task USER05_RefreshUserObjectTest()
        {
            // Arrange

            // Act
            await _userObject.Refresh(_authToken);

            // Assert
            Assert.IsFalse(_userObject.DirtyObject);
        }

        [TestMethod]
        [TestCategory("Integration - User")]
        [Description("Fetches the manager of the user retrieved from the UserFilter property in App.config")]
        public async Task USER06_GetRelatedManagerTest()
        {
            // Arrange
            User manager;

            // Act
            manager = await UserController.GetUserById(_authToken, _userObject.Manager.Id);

            // Assert
            Assert.IsNotNull(manager);
            Assert.IsFalse(manager.IsPartialUser);
        }
        #endregion // Test Cases
    }
}
