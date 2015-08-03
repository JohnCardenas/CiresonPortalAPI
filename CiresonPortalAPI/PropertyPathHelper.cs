using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiresonPortalAPI
{
    /// <summary>
    /// Helper class to construct the path to a property in the format $Context/Property[Type]/PropertyName$
    /// </summary>
    internal class PropertyPathHelper
    {
        private Guid   _oObjectClass;
        private string _sPropertyName;

        /// <summary>
        /// Constructs a new PropertyPathHelper
        /// </summary>
        /// <param name="objectClass">GUID of the object class</param>
        /// <param name="propertyName">Name of the property</param>
        public PropertyPathHelper(Guid objectClass, string propertyName)
        {
            _oObjectClass = objectClass;
            _sPropertyName = propertyName;
        }

        public override string ToString()
        {
            return "$Context/Property[Type='" + _oObjectClass.ToString() + "']/" + _sPropertyName + "$";
        }
    }
}
