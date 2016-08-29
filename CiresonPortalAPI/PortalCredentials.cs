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
        string _domain;

        public string Domain
        {
            get { return _domain; }
            internal set { _domain = value; }
        }
    }
}
