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

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            Task<AuthorizationToken> tokenTask = AuthorizationController.GetAuthorizationToken(ConfigurationHelper.PortalUrl, ConfigurationHelper.UserName, ConfigurationHelper.Password, ConfigurationHelper.Domain);
            tokenTask.Wait();
            _authToken = tokenTask.Result;
        }

        [TestMethod]
        [TestCategory("Integration - UserController")]
        public async Task GetUserListTest()
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
        public async Task GetAnalystUserListTest()
        {
            // Arrange
            List<User> userList;

            // Act
            userList = await UserController.GetUserList(_authToken, "", true, false, 1);

            // Assert
            Assert.AreEqual(1, userList.Count);
            Assert.IsNotNull(userList[0]);

            // Setup for next test
            _userId = userList[0].Id;
        }

        [TestMethod]
        [TestCategory("Integration - UserController")]
        public async Task GetUserByIdTest()
        {
            // Arrange
            User user;

            // Act
            user = await UserController.GetUserById(_authToken, _userId);

            // Assert
            Assert.IsNotNull(user);
        }
    }
}
