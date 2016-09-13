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
        /// Built-in templates for Cireson Hardware Assets
        /// </summary>
        public static class HardwareAsset
        {
            /// <summary>
            /// Asset Management Hardware Asset Template (HardwareAssetTempate)
            /// Cireson Asset Management Hardware Asset Tempate (Portal)
            /// </summary>
            public static Guid Default { get { return new Guid("5b20b20f-0e6d-556b-8247-9b53e2393b73"); } }
        }

        /// <summary>
        /// Built-in templates for Cireson Locations
        /// </summary>
        public static class Location
        {
            public static Guid Default { get { return new Guid("{a6b537ca-d0cf-b0b9-3a3c-40e96d4c8ba0}"); } }
        }

        /// <summary>
        /// Built-in templates for Cireson Purchase Orders
        /// </summary>
        public static class PurchaseOrder
        {
            public static Guid Default { get { return new Guid("{6288c684-569f-d9fa-efce-3cf0ca512e61}"); } }
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
