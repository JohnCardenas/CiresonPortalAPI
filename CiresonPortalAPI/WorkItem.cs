using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiresonPortalAPI
{
    public abstract class WorkItem : TypeProjection
    {
        #region Read-Only Properties
        /// <summary>Gets the DisplayName of the Work Item. Read only.</summary>
        public string DisplayName
        {
            get { return this.GetPrimitiveValue<string>("DisplayName"); }
        }
        #endregion // Read-Only Properties

        #region General Methods
        public override string ToString()
        {
            return this.DisplayName;
        }
        #endregion // General Methods
    }
}
