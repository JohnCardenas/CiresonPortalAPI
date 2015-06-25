using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiresonPortalAPI
{
    /// <summary>
    /// Enumerations built into Service Manager
    /// </summary>
    public static class EnumerationConstants
    {
        /// <summary>
        /// Enumerations for Incidents
        /// </summary>
        public static class Incidents
        {
            /// <summary>
            /// Incident enumeration lists
            /// </summary>
            public static class Lists
            {
                /// <summary>IncidentClassificationEnum</summary>
                public static const Guid Classification = new Guid("{1f77f0ce-9e43-340f-1fd5-b11cc36c9cba}");

                /// <summary>System.WorkItem.TroubleTicket.ImpactEnum</summary>
                public static const Guid Impact = new Guid("{11756265-f18e-e090-eed2-3aa923a4c872}");

                /// <summary>System.WorkItem.TroubleTicket.UrgencyEnum</summary>
                public static const Guid Urgency = new Guid("{04b28bfb-8898-9af3-009b-979e58837852}");

                /// <summary>IncidentSourceEnum</summary>
                public static const Guid Source = new Guid("{5d59071e-69b3-7ef4-6dee-aacc5b36d898}");

                /// <summary>IncidentStatusEnum</summary>
                public static const Guid Status = new Guid("{89b34802-671e-e422-5e38-7dae9a413ef8}");

                /// <summary>IncidentTierQueuesEnum</summary>
                public static const Guid TierQueues = new Guid("{c3264527-a501-029f-6872-31300080b3bf}");
            }
            /// <summary>
            /// Incident enumeration built-in values
            /// </summary>
            public static class BuiltinValues
            {
                /// <summary>
                /// System.WorkItem.TroubleTicket.ImpactEnum built-in values
                /// </summary>
                public static class Impact
                {
                    /// <summary>System.WorkItem.TroubleTicket.ImpactEnum.High</summary>
                    public static const Guid High = new Guid("{d2b5e816-2d24-8e7d-a61f-2cceaeac2664}");

                    /// <summary>System.WorkItem.TroubleTicket.ImpactEnum.Medium</summary>
                    public static const Guid Medium = new Guid("{80cc222b-2653-2f68-8cee-3a7dd3b723c1}");

                    /// <summary>System.WorkItem.TroubleTicket.ImpactEnum.Low</summary>
                    public static const Guid Low = new Guid("{8f1a713e-53aa-9d8a-31b9-a9540074f305}");
                }

                /// <summary>
                /// System.WorkItem.TroubleTicket.UrgencyEnum built-in values
                /// </summary>
                public static class Urgency
                {
                    /// <summary>System.WorkItem.TroubleTicket.UrgencyEnum.High</summary>
                    public static const Guid High = new Guid("{2f8f0747-b6cb-7996-fd4a-84d09743f218}");

                    /// <summary>System.WorkItem.TroubleTicket.UrgencyEnum.Medium</summary>
                    public static const Guid Medium = new Guid("{02625c30-08c6-4181-b2ed-222fa473280e}");

                    /// <summary>System.WorkItem.TroubleTicket.UrgencyEnum.Low</summary>
                    public static const Guid Low = new Guid("{725a4cad-088c-4f55-a845-000db8872e01}");
                }

                /// <summary>
                /// IncidentSourceEnum built-in values
                /// </summary>
                public static class Source
                {
                    /// <summary>IncidentSourceEnum.Console</summary>
                    public static const Guid Console = new Guid("{76480d55-a19d-7cef-4446-0f1ccaef11ce}");

                    /// <summary>IncidentSourceEnum.DCM</summary>
                    public static const Guid DCM = new Guid("{551ed1e6-0a12-9f2b-a057-528b115e17e3}");

                    /// <summary>IncidentSourceEnum.Email</summary>
                    public static const Guid Email = new Guid("{92b68e4f-c5bf-52ec-9bda-f55b43a9f4b5}");

                    /// <summary>IncidentSourceEnum.Phone</summary>
                    public static const Guid Phone = new Guid("{7b417fee-0516-a75a-c62c-426905fda768}");

                    /// <summary>IncidentSourceEnum.Portal</summary>
                    public static const Guid Portal = new Guid("{6ef191ce-3124-2974-94fb-020c677f4017}");

                    /// <summary>IncidentSourceEnum.SCOM</summary>
                    public static const Guid SCOM = new Guid("{564c1afc-6453-4b25-5564-b46d84155545}");

                    /// <summary>IncidentSourceEnum.System</summary>
                    public static const Guid System = new Guid("{615778d5-485a-752f-3c5d-f412f806e624}");
                }

                /// <summary>
                /// IncidentStatusEnum built-in values
                /// </summary>
                public static class Status
                {
                    /// <summary>IncidentStatusEnum.Active</summary>
                    public static const Guid Active = new Guid("{5e2d3932-ca6d-1515-7310-6f58584df73e}");

                    /// <summary>IncidentStatusEnum.Active.Pending</summary>
                    public static const Guid Active_Pending = new Guid("{b6679968-e84e-96fa-1fec-8cd4ab39c3de}");

                    /// <summary>IncidentStatusEnum.Closed</summary>
                    public static const Guid Closed = new Guid("{bd0ae7c4-3315-2eb3-7933-82dfc482dbaf}");

                    /// <summary>IncidentStatusEnum.Resolved</summary>
                    public static const Guid Resolved = new Guid("{2b8830b6-59f0-f574-9c2a-f4b4682f1681}");
                }
            }
        }
    }
}
