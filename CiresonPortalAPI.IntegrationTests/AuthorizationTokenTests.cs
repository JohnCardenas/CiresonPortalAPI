using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Security;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CiresonPortalAPI.IntegrationTests
{
    /// <summary>
    /// Summary description for AuthorizationTokenTests
    /// </summary>
    [TestClass]
    public class AuthorizationTokenTests
    {
        #region Fields
        private string _portalUrl;
        private string _userName;
        private string _domain;
        private SecureString _password;
        #endregion // Fields

        #region Constructor
        public AuthorizationTokenTests()
        {
            _portalUrl = ConfigurationManager.AppSettings["PortalUrl"];
            _userName = ConfigurationManager.AppSettings["UserName"];
            _domain = ConfigurationManager.AppSettings["Domain"];
            _password = new SecureString();

            foreach (char c in ConfigurationManager.AppSettings["Password"])
            {
                _password.AppendChar(c);
            }

            _password.MakeReadOnly();
        }
        #endregion // Constructor

        [TestMethod]
        public async Task GetAuthenticatonTokenTest()
        {
            // Arrange
            AuthorizationToken authToken;

            // Act
            authToken = await AuthorizationController.GetAuthorizationToken(_portalUrl, _userName, _password, _domain);

            // Assert
            Assert.IsNotNull(authToken);
            Assert.AreEqual(_userName, authToken.UserName);
            Assert.AreEqual(_domain, authToken.Domain);
            Assert.AreEqual(_portalUrl, authToken.PortalUrl);
            Assert.IsTrue(authToken.IsValid);
        }

        [TestMethod]
        public async Task GetUserRightsTest()
        {
            // Arrange
            AuthorizationToken authToken;
            ConsoleUser userRights;

            // Act
            authToken = await AuthorizationController.GetAuthorizationToken(_portalUrl, _userName, _password, _domain);
            userRights = await AuthorizationController.GetUserRights(authToken);

            // Assert
            Assert.IsNotNull(userRights);
            Assert.AreEqual(_userName, userRights.UserName);
            Assert.AreEqual(_domain, userRights.Domain);
            Assert.IsFalse(string.IsNullOrWhiteSpace(userRights.PrincipalName));
        }
    }
}
