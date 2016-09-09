using System.Configuration;
using System.Security;

namespace CiresonPortalAPI.Tests.Integration
{
    public static class ConfigurationHelper
    {
        public static string UserName
        {
            get { return ConfigurationManager.AppSettings["UserName"]; }
        }

        public static SecureString Password
        {
            get
            {
                SecureString pwd = new SecureString();

                foreach (char c in ConfigurationManager.AppSettings["Password"])
                {
                    pwd.AppendChar(c);
                }

                pwd.MakeReadOnly();
                return pwd;
            }
        }

        public static string Domain
        {
            get { return ConfigurationManager.AppSettings["Domain"]; }
        }

        public static string PortalUrl
        {
            get { return ConfigurationManager.AppSettings["PortalUrl"]; }
        }

        public static string UserFilter
        {
            get { return ConfigurationManager.AppSettings["UserFilter"]; }
        }
    }
}
