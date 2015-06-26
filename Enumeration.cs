﻿using System;
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

        public EnumJson() { }
    }

    internal class EnumJsonOrdinal : EnumJson
    {
        public decimal Ordinal;
    }

    /// <summary>
    /// IComparer for an Enumeration object
    /// </summary>
    public class EnumerationComparer : IComparer<Enumeration>
    {
        public int Compare(Enumeration a, Enumeration b)
        {
            // Null value comparison
            if (a == null && b != null)
                return -1;
            else if (a == null && b == null)
                return 0;
            else if (a != null && b == null)
                return 1;

            // Non-null value comparison
            if (a.Ordinal == 0 && b.Ordinal == 0)
            {
                // Text comparison
                return string.Compare(a.ToString(), b.ToString());
            }
            else
            {
                // Ordinal property comparison
                if (a.Ordinal < b.Ordinal)
                    return -1;
                else if (a.Ordinal > b.Ordinal)
                    return 1;
                else
                    return 0;
            }
        }
    }

    /// <summary>
    /// Represents an enumeration in Service Manager
    /// </summary>
    public class Enumeration
    {
        const string LIST_ENDPOINT_TREE = "/api/V3/Enum/GetList";
        const string LIST_ENDPOINT_FLAT = "/api/V3/Enum/GetFlatList";

        private Guid    _id;
        private string  _text;
        private string  _name;
        private bool    _childrenLoaded = false;
        private bool    _isFlat;
        private bool    _hasChildren;
        private decimal _ordinal;

        private List<Enumeration> _children;

        public Guid    Id      { get { return _id;      } }
        public string  Text    { get { return _text;    } }
        public string  Name    { get { return _name;    } }
        public decimal Ordinal { get { return _ordinal; } }

        /// <summary>
        /// Fetches a list of enumerations from the server
        /// </summary>
        /// <param name="portalUrl">URL of the Cireson Portal</param>
        /// <param name="authToken">Authorization token</param>
        /// <param name="enumList">Enumeration list to fetch</param>
        /// <param name="flatten">If true, flatten the entire enumeration tree into one list; if false, only return the first-level items</param>
        /// <returns></returns>
        public static async Task<List<Enumeration>> GetEnumerationList(AuthorizationToken authToken, Guid enumList, bool flatten)
        {
            if (!authToken.IsValid)
            {
                throw new InvalidCredentialException("AuthorizationToken is not valid.");
            }

            string endpoint = (flatten ? LIST_ENDPOINT_FLAT : LIST_ENDPOINT_TREE);
            endpoint += "/" + enumList.ToString() + "/" + (flatten ? "?itemFilter=" : "");

            try
            {
                // Initialize the HTTP helper and get going
                PortalHttpHelper helper = new PortalHttpHelper(authToken);
                string result = await helper.GetAsync(endpoint);

                List<Enumeration> returnList = new List<Enumeration>();

                if (flatten)
                {
                    // A flat enumeration list has null Ordinals, so we use the base EnumJson class
                    List<EnumJson> jEnumList = JsonConvert.DeserializeObject<List<EnumJson>>(result);
                    foreach (var jEnum in jEnumList)
                    {
                        // Skip empty enumerations
                        if (jEnum.ID != Guid.Empty.ToString())
                            returnList.Add(new Enumeration(jEnum.ID, jEnum.Text, jEnum.Name, flatten, jEnum.HasChildren));
                    }
                }
                else
                {
                    // A non-flat enumeration list has non-null Ordinals, so we have to use a different conversion class
                    List<EnumJsonOrdinal> jEnumList = JsonConvert.DeserializeObject<List<EnumJsonOrdinal>>(result);
                    foreach (var jEnum in jEnumList)
                    {
                        // Skip empty enumerations
                        if (jEnum.ID != Guid.Empty.ToString())
                            returnList.Add(new Enumeration(jEnum.ID, jEnum.Text, jEnum.Name, flatten, jEnum.HasChildren, jEnum.Ordinal));
                    }
                }

                return returnList;
            }
            catch (Exception e)
            {
                throw; // Rethrow exceptions
            }
        }

        private Enumeration(string id, string text, string name, bool isFlat, bool hasChildren, decimal ordinal = 0)
        {
            _id = new Guid(id);
            _text = text;
            _name = name;
            _isFlat = isFlat;
            _hasChildren = hasChildren;
            _ordinal = ordinal;
        }

        public override string ToString()
        {
            return _text;
        }
    }
}
