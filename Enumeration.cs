using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;
using CiresonPortalAPI;
using Newtonsoft.Json;

namespace CiresonPortalAPI
{
    internal class EnumJson
    {
        public string ID;
        public string Text;
        public string Name;
        public bool HasChildren;
        public decimal Ordinal;

        public EnumJson() { }
    }

    class Enumeration
    {
        const string LIST_ENDPOINT = "/api/V3/Enum/GetList";

        private Guid    _id;
        private string  _text;
        private string  _name;
        private bool    _hasChildren;
        private bool    _childrenLoaded = false;
        private decimal _ordinal;

        private List<Enumeration> _children;

        public Guid    Id      { get { return _id;      } }
        public string  Text    { get { return _text;    } }
        public string  Name    { get { return _name;    } }
        public decimal Ordinal { get { return _ordinal; } }

        public static async Task<List<Enumeration>> GetEnumerationList(string portalUrl, AuthorizationToken authToken, Guid enumList)
        {
            if (!authToken.IsValid)
            {
                throw new InvalidCredentialException("AuthorizationToken is not valid.");
            }

            try
            {
                // Initialize the HTTP helper and get going
                PortalHttpHelper helper = new PortalHttpHelper(portalUrl, authToken);
                string result = await helper.GetAsync(LIST_ENDPOINT + "/" + enumList.ToString() + "/");

                List<EnumJson> jEnumList = JsonConvert.DeserializeObject<List<EnumJson>>(result);
                List<Enumeration> returnList = new List<Enumeration>();

                foreach (EnumJson jEnum in jEnumList)
                {
                    returnList.Add(new Enumeration(jEnum.ID, jEnum.Text, jEnum.Name, jEnum.HasChildren, jEnum.Ordinal));
                }

                return returnList;
            }
            catch (Exception e)
            {
                throw; // Rethrow exceptions
            }
        }

        private Enumeration(string id, string text, string name, bool hasChildren, decimal ordinal)
        {
            _id = new Guid(id);
            _text = text;
            _name = name;
            _hasChildren = hasChildren;
            _ordinal = ordinal;
        }
    }
}
