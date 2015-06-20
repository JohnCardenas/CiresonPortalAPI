using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CiresonPortalAPI;

namespace CiresonPortalAPI
{
    class Enumeration
    {
        private Guid    _id;
        private string  _text;
        private string  _name;
        private bool    _hasChildren;
        private bool    _childrenLoaded;
        private decimal _ordinal;

        private List<Enumeration> _children;

        public Guid    Id      { get { return _id;      } }
        public string  Text    { get { return _text;    } }
        public string  Name    { get { return _name;    } }
        public decimal Ordinal { get { return _ordinal; } }

        public static async Task<List<Enumeration>> GetEnumList(Guid enumList, PortalSession session)
        {
            if (session.IsValid)
            {
                // ...
            }
        }

        private Enumeration(string id, string text, string name, bool hasChildren, decimal ordinal)
        {
            _id = new Guid(id);
            _text = text;
            _name = name;
            _hasChildren = hasChildren;
            _childrenLoaded = false;
            _ordinal = ordinal;
        }
    }
}
