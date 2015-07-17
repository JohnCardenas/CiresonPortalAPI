using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiresonPortalAPI
{
    /// <summary>
    /// Classes built into Service Manager
    /// </summary>
    public static class ClassConstants
    {
        /// <summary>
        /// Change Request (System.WorkItem.ChangeRequest)
        /// </summary>
        public static Guid ChangeRequest { get { return new Guid("{e6c9cf6e-d7fe-1b5d-216c-c3f5d2c7670c}"); } }
        
        /// <summary>
        /// Incident (System.WorkItem.Incident)
        /// </summary>
        public static Guid Incident { get { return new Guid("{a604b942-4c7b-2fb2-28dc-61dc6f465c68}"); } }

        /// <summary>
        /// Manual Activity (System.WorkItem.Activity.ManualActivity)
        /// </summary>
        public static Guid ManualActivity { get { return new Guid("{7ac62bd4-8fce-a150-3b40-16a39a61383d}"); } }

        /// <summary>
        /// Problem (System.WorkItem.Problem)
        /// </summary>
        public static Guid Problem { get { return new Guid("{422afc88-5eff-f4c5-f8f6-e01038cde67f}"); } }
        
        /// <summary>
        /// Release Record (System.WorkItem.ReleaseRecord)
        /// </summary>
        public static Guid ReleaseRecord { get { return new Guid("{d02dc3b6-d709-46f8-cb72-452fa5e082b8}"); } }

        /// <summary>
        /// Review Activity (System.WorkItem.Activity.ReviewActivity)
        /// </summary>
        public static Guid ReviewActivity { get { return new Guid("{bfd90aaa-80dd-0fbb-6eaf-65d92c1d8e36}"); } }

        /// <summary>
        /// Service Request (System.WorkItem.ServiceRequest)
        /// </summary>
        public static Guid ServiceRequest { get { return new Guid("{04b69835-6343-4de2-4b19-6be08c612989}"); } }
    }
}
