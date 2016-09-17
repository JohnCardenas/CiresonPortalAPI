using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Security;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CiresonPortalAPI.Tests.Integration
{
    /// <summary>
    /// Summary description for AuthorizationTokenTests
    /// </summary>
    [TestClass]
    public class AuthorizationTokenTests
    {
        #region Fields
        private static AuthorizationToken _authToken;
        #endregion // Fields

        #region Constructor
        public AuthorizationTokenTests() { }
        #endregion // Constructor

        #region AUTH01_GetAuthorizationTokenTest
        [TestMethod]
        [TestCategory("Integration - AuthorizationTokens")]
        public async Task AUTH01_GetAuthorizationTokenTest()
        {
            // Arrange

            // Act
            _authToken = await AuthorizationController.GetAuthorizationToken(ConfigurationHelper.PortalUrl, ConfigurationHelper.UserName, ConfigurationHelper.Password, ConfigurationHelper.Domain);

            // Assert
            Assert.IsNotNull(_authToken);
            Assert.AreEqual(ConfigurationHelper.UserName,  _authToken.User.UserName);
            Assert.AreEqual(ConfigurationHelper.Domain,    _authToken.User.Domain);
            Assert.AreEqual(ConfigurationHelper.PortalUrl, _authToken.PortalUrl);
            Assert.IsTrue(_authToken.IsValid);
        }
        #endregion

        #region AUTH02_GetUserRightsTest
        [TestMethod]
        [TestCategory("Integration - AuthorizationTokens")]
        public async Task AUTH02_GetUserRightsTest()
        {
            // Arrange
            ConsoleUser userRights;

            // Act
            userRights = await AuthorizationController.GetUserRights(_authToken);

            // Assert
            Assert.IsNotNull(userRights);
            Assert.AreEqual(ConfigurationHelper.UserName, userRights.UserName);
            Assert.AreEqual(ConfigurationHelper.Domain, userRights.Domain);
            Assert.IsFalse(string.IsNullOrWhiteSpace(userRights.PrincipalName));
        }
        #endregion
    }
}
