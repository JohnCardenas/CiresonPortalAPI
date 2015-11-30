using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;
using Newtonsoft.Json;

namespace CiresonPortalAPI.ConfigurationItems
{
    /// <summary>
    /// Represents a user id and name pairing
    /// </summary>
    public class PartialUser
    {
        protected Guid   _oId;
        protected string _sName;

        public virtual Guid   Id   { get { return _oId;   } }
        public virtual string Name { get { return _sName; } }

        [JsonConstructor]
        internal PartialUser(string id, string name)
        {
            _oId = new Guid(id);
            _sName = name;
        }

        internal PartialUser(Guid id, string name)
        {
            _oId   = id;
            _sName = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    internal class PartialUserComparer : IComparer<PartialUser>
    {
        public int Compare(PartialUser a, PartialUser b)
        {
            return string.Compare(a.ToString(), b.ToString());
        }
    }
}
