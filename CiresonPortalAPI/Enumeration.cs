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

        public EnumJson() { }
    }

    internal class EnumJsonOrdinal : EnumJson
    {
        public decimal Ordinal;
    }

    /// <summary>
    /// IComparer for an Enumeration object
    /// </summary>
    internal class EnumerationComparer : IComparer<Enumeration>
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
    public class Enumeration : IEquatable<Enumeration>
    {
        private Guid    _oId;
        private string  _sText;
        private string  _sName;
        private bool    _bIsFlat;
        private bool    _bHasChildren;
        private decimal _dOrdinal;

        public Guid    Id      { get { return _oId;      } }
        public string  Text    { get { return _sText;    } }
        public string  Name    { get { return _sName;    } }
        public decimal Ordinal { get { return _dOrdinal; } }

        internal Enumeration(Guid id, string text, string name, bool isFlat, bool hasChildren, decimal ordinal = 0)
        {
            _oId = id;
            _sText = text;
            _sName = name;
            _bIsFlat = isFlat;
            _bHasChildren = hasChildren;
            _dOrdinal = ordinal;
        }

        internal Enumeration(string id, string text, string name, bool isFlat, bool hasChildren, decimal ordinal = 0) : this(new Guid(id), text, name, isFlat, hasChildren, ordinal) {}

        /// <summary>
        /// Compares another Enumeration for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Enumeration other)
        {
            if (other == null)
                return false;

            return (this.Id.ToString() == other.Id.ToString());
        }

        /// <summary>
        /// Casts another object to an Enumeration and checks for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Enumeration enumObj = obj as Enumeration;

            if (enumObj == null)
                return false;
            else
                return Equals(enumObj);
        }

        /// <summary>
        /// Override base GetHashCode() for IEquatable
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override string ToString()
        {
            return _sText;
        }
    }
}
