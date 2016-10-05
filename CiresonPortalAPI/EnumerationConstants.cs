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
                public static Guid Classification { get { return new Guid("{1f77f0ce-9e43-340f-1fd5-b11cc36c9cba}"); } }

                /// <summary>System.WorkItem.TroubleTicket.ImpactEnum</summary>
                public static Guid Impact { get { return new Guid("{11756265-f18e-e090-eed2-3aa923a4c872}"); } }

                /// <summary>System.WorkItem.TroubleTicket.UrgencyEnum</summary>
                public static Guid Urgency { get { return new Guid("{04b28bfb-8898-9af3-009b-979e58837852}"); } }

                /// <summary>IncidentSourceEnum</summary>
                public static Guid Source { get { return new Guid("{5d59071e-69b3-7ef4-6dee-aacc5b36d898}"); } }

                /// <summary>IncidentStatusEnum</summary>
                public static Guid Status { get { return new Guid("{89b34802-671e-e422-5e38-7dae9a413ef8}"); } }

                /// <summary>IncidentTierQueuesEnum</summary>
                public static Guid TierQueues { get { return new Guid("{c3264527-a501-029f-6872-31300080b3bf}"); } }

                /// <summary>Alias for IncidentTierQueuesEnum</summary>
                public static Guid SupportGroups { get { return TierQueues; } }
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
                    public static Guid High { get { return new Guid("{d2b5e816-2d24-8e7d-a61f-2cceaeac2664}"); } }

                    /// <summary>System.WorkItem.TroubleTicket.ImpactEnum.Medium</summary>
                    public static Guid Medium { get { return new Guid("{80cc222b-2653-2f68-8cee-3a7dd3b723c1}"); } }

                    /// <summary>System.WorkItem.TroubleTicket.ImpactEnum.Low</summary>
                    public static Guid Low { get { return new Guid("{8f1a713e-53aa-9d8a-31b9-a9540074f305}"); } }
                }

                /// <summary>
                /// System.WorkItem.TroubleTicket.UrgencyEnum built-in values
                /// </summary>
                public static class Urgency
                {
                    /// <summary>System.WorkItem.TroubleTicket.UrgencyEnum.High</summary>
                    public static Guid High { get { return new Guid("{2f8f0747-b6cb-7996-fd4a-84d09743f218}"); } }

                    /// <summary>System.WorkItem.TroubleTicket.UrgencyEnum.Medium</summary>
                    public static Guid Medium { get { return new Guid("{02625c30-08c6-4181-b2ed-222fa473280e}"); } }

                    /// <summary>System.WorkItem.TroubleTicket.UrgencyEnum.Low</summary>
                    public static Guid Low { get { return new Guid("{725a4cad-088c-4f55-a845-000db8872e01}"); } }
                }

                /// <summary>
                /// IncidentSourceEnum built-in values
                /// </summary>
                public static class Source
                {
                    /// <summary>IncidentSourceEnum.Console</summary>
                    public static Guid Console { get { return new Guid("{76480d55-a19d-7cef-4446-0f1ccaef11ce}"); } }

                    /// <summary>IncidentSourceEnum.DCM</summary>
                    public static Guid DCM { get { return new Guid("{551ed1e6-0a12-9f2b-a057-528b115e17e3}"); } }

                    /// <summary>IncidentSourceEnum.Email</summary>
                    public static Guid Email { get { return new Guid("{92b68e4f-c5bf-52ec-9bda-f55b43a9f4b5}"); } }

                    /// <summary>IncidentSourceEnum.Phone</summary>
                    public static Guid Phone { get { return new Guid("{7b417fee-0516-a75a-c62c-426905fda768}"); } }

                    /// <summary>IncidentSourceEnum.Portal</summary>
                    public static Guid Portal { get { return new Guid("{6ef191ce-3124-2974-94fb-020c677f4017}"); } }

                    /// <summary>IncidentSourceEnum.SCOM</summary>
                    public static Guid SCOM { get { return new Guid("{564c1afc-6453-4b25-5564-b46d84155545}"); } }

                    /// <summary>IncidentSourceEnum.System</summary>
                    public static Guid System { get { return new Guid("{615778d5-485a-752f-3c5d-f412f806e624}"); } }
                }

                /// <summary>
                /// IncidentStatusEnum built-in values
                /// </summary>
                public static class Status
                {
                    /// <summary>IncidentStatusEnum.Active</summary>
                    public static Guid Active { get { return new Guid("{5e2d3932-ca6d-1515-7310-6f58584df73e}"); } }

                    /// <summary>IncidentStatusEnum.Active.Pending</summary>
                    public static Guid Active_Pending { get { return new Guid("{b6679968-e84e-96fa-1fec-8cd4ab39c3de}"); } }

                    /// <summary>IncidentStatusEnum.Closed</summary>
                    public static Guid Closed { get { return new Guid("{bd0ae7c4-3315-2eb3-7933-82dfc482dbaf}"); } }

                    /// <summary>IncidentStatusEnum.Resolved</summary>
                    public static Guid Resolved { get { return new Guid("{2b8830b6-59f0-f574-9c2a-f4b4682f1681}"); } }
                }
            }
        }

        /// <summary>
        /// Enumerations for Cireson Purchase Orders
        /// </summary>
        public static class PurchaseOrder
        {
            /// <summary>
            /// Purchase Order enumeration lists
            /// </summary>
            public static class Lists
            {
                /// <summary>Cireson.AssetManagement.PurchaseOrderStatusEnum</summary>
                public static Guid PurchaseOrderStatus { get { return new Guid("{4150e434-e09e-70db-3573-eec1fff96bd4}"); } }

                /// <summary>Cireson.AssetManagement.PurchaseOrderTypeEnum</summary>
                public static Guid PurchaseOrderType { get { return new Guid("{2e654705-a8bc-d13f-7013-d5d1d7fc529c}"); } }
            }
        }

        /// <summary>
        /// Enumerations for base Configuration Items
        /// </summary>
        public static class ConfigItem
        {
            /// <summary>
            /// Configuration Item lists
            /// </summary>
            public static class Lists
            {
                /// <summary>System.ConfigItem.ObjectStatusEnum</summary>
                public static Guid ObjectStatus { get { return new Guid("{3dc28152-62a3-bd53-ccc1-66e0ad3df8e8}"); } }
            }

            /// <summary>
            /// Configuration Item built-in values
            /// </summary>
            public static class BuiltinValues
            {
                /// <summary>
                /// System.ConfigItem.ObjectStatusEnum built-in values
                /// </summary>
                public static class ObjectStatus
                {
                    /// <summary>System.ConfigItem.ObjectStatusEnum.Active</summary>
                    public static Guid Active { get { return new Guid("{acdcedb7-100c-8c91-d664-4629a218bd94}"); } }

                    /// <summary>System.ConfigItem.ObjectStatusEnum.Deleted</summary>
                    public static Guid Deleted { get { return new Guid("{eec83e3c-0106-d4c0-99ea-93b75fd23020}"); } }

                    /// <summary>System.ConfigItem.ObjectStatusEnum.PendingDelete</summary>
                    public static Guid PendingDelete { get { return new Guid("{47101e64-237f-12c8-e3f5-ec5a665412fb}"); } }
                }
            }
        }
    }
}
