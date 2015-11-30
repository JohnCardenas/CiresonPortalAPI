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
        #region Properties
        public Guid ObjectClass { get; set; }
        public string PropertyName { get; set; }
        #endregion // Properties

        /// <summary>
        /// Constructs a new PropertyPathHelper
        /// </summary>
        /// <param name="objectClass">GUID of the object class</param>
        /// <param name="propertyName">Name of the property</param>
        public PropertyPathHelper(Guid objectClass, string propertyName)
        {
            this.ObjectClass = objectClass;
            this.PropertyName = propertyName;
        }

        /// <summary>
        /// Default constructor; creates a blank PropertyPathHelper
        /// </summary>
        public PropertyPathHelper() { }

        public override string ToString()
        {
            return "$Context/Property[Type='" + this.ObjectClass.ToString() + "']/" + this.PropertyName + "$";
        }
    }
}
