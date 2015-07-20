using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CredentialManagement;

namespace CiresonPortalAPI
{
    internal class PortalCredentials : Credential
    {
        private string _sPortalUrl;

        public string UserNameNoDomain { get { return GetUserName(this.Username); } }
        public string Domain { get { return GetDomainName(this.Username); } }

        private static string GetUserName(string userNameDomainToken)
        {
            // sAMAccountName syntax: DOMAIN\USER
            if (userNameDomainToken.Contains("\\"))
                return userNameDomainToken.After("\\");

            // userPrincipalName syntax: user@domain.tld
            if (userNameDomainToken.Contains("@"))
                return userNameDomainToken.Before("@");

            // Just a user name
            return userNameDomainToken;
        }

        private static string GetDomainName(string userNameDomainToken)
        {
            // sAMAccountName syntax: DOMAIN\USER
            if (userNameDomainToken.Contains("\\"))
                return userNameDomainToken.Before("\\");

            // userPrincipalName syntax: user@domain.tld
            if (userNameDomainToken.Contains("@"))
                return userNameDomainToken.After("@");

            // No domain!
            return string.Empty;
        }
    }
}
