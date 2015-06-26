using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiresonPortalAPI
{
    public static class TemplateConstants
    {
        /// <summary>
        /// Built-in templates for Incidents
        /// </summary>
        public static class Incident
        {
            /// <summary>
            /// Default Incident Template (DefaultIncidentTemplate)
            /// Use when you want to open an incident for which you do not have a specific template
            /// </summary>
            public static Guid Default { get { return new Guid("{a77bb0c9-e201-dd93-230c-799a66d9e8fa}"); } }
        }

        /// <summary>
        /// Built-in templates for Service Requests
        /// </summary>
        public static class ServiceRequest
        {
            /// <summary>
            /// Default Service Request (ServiceManager.ServiceRequest.Library.Template.DefaultServiceRequest)
            /// Default Service Request
            /// </summary>
            public static Guid Default { get { return new Guid("{03bc9162-041f-c987-8ce4-a5547cd9ca04}"); } }
        }
    }
}
