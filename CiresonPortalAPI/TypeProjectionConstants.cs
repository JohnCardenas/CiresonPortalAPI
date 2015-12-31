using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CiresonPortalAPI.ConfigurationItems;
using CiresonPortalAPI.WorkItems;

namespace CiresonPortalAPI
{
    /// <summary>
    /// Type Projections built into Service Manager
    /// </summary>
    public static class TypeProjectionConstants
    {
        /// <summary>
        /// Returns the SCSM projection ID of the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Guid GetProjectionIdByType<T>()
        {
            if (typeof(T) == typeof(HardwareAsset))
                return TypeProjectionConstants.HardwareAsset;
            else if (typeof(T) == typeof(Location))
                return TypeProjectionConstants.Location;
            else if (typeof(T) == typeof(PurchaseOrder))
                return TypeProjectionConstants.PurchaseOrder;
            else if (typeof(T) == typeof(User))
                return TypeProjectionConstants.User;
            else
                throw new CiresonApiException("Unrecognized type " + typeof(T).FullName);
        }

        /// <summary>
        /// Build (advanced)
        /// System.Build.ProjectionType
        /// </summary>
        public static Guid Build { get { return new Guid("{400645c1-1599-60c3-5f29-7bef95a32bbc}"); } }

        /// <summary>
        /// Cireson Catalog Item (Full)
        /// Cireson.AssetManagement.CatalogItem.ProjectionType
        /// </summary>
        public static Guid CatalogItem { get { return new Guid("{7cf0d527-e917-00a4-37ca-62b40e2f7fa9}"); } }

        /// <summary>
        /// Change Request (advanced)
        /// System.WorkItem.ChangeRequestProjection
        /// </summary>
        public static Guid ChangeRequest { get { return new Guid("{674194d8-0246-7b90-d871-e1ea015b2ea7}"); } }

        /// <summary>
        /// Computer (advanced)
        /// Microsoft.Windows.Computer.ProjectionType
        /// </summary>
        public static Guid Computer { get { return new Guid("{c95d4106-3f24-d3cc-232e-9f51198b295c}"); } }

        /// <summary>
        /// Configuration Item (advanced)
        /// System.ConfigItem.Projection
        /// </summary>
        public static Guid ConfigurationItem { get { return new Guid("{8ab27adb-13b1-2b7b-56e6-91598417cbee}"); } }

        /// <summary>
        /// Cireson Cost Center (Full)
        /// Cireson.AssetManagement.CostCenter
        /// </summary>
        public static Guid CostCenter { get { return new Guid("{832bc830-7120-1619-1811-ce0db12593b7}"); } }

        /// <summary>
        /// Dependent Activity (advanced)
        /// System.WorkItem.Activity.DependentActivityProjection
        /// </summary>
        public static Guid DependentActivity { get { return new Guid("{9bd2843c-086b-1829-24e8-1b911db2cfc2}"); } }

        /// <summary>
        /// Desired Configuration Management Incidents (advanced)
        /// System.WorkItem.Incident.DCMProjectionType
        /// </summary>
        public static Guid DCM { get { return new Guid("{c3eec496-48d1-39c2-a82a-1131d3a31c84}"); } }

        /// <summary>
        /// Environment (advanced)
        /// System.Environment.ProjectionType
        /// </summary>
        public static Guid Environment { get { return new Guid("{569c5a3b-1e68-78eb-0963-0ce3e6c3c3d2}"); } }

        /// <summary>
        /// Cireson Hardware Asset (Full)
        /// Cireson.AssetManagement.HardwareAsset
        /// </summary>
        public static Guid HardwareAsset { get { return new Guid("{7dd5144c-bd5d-af27-e3af-debcb5a53546}"); } }

        /// <summary>
        /// Incident (advanced)
        /// System.WorkItem.Incident.ProjectionType
        /// </summary>
        public static Guid Incident { get { return new Guid("{285cb0a2-f276-bccb-563e-bb721df7cdec}"); } }

        /// <summary>
        /// Incident Activities (advanced)
        /// System.WorkItem.Incident.Activities.ProjectionType
        /// </summary>
        public static Guid IncidentActivities { get { return new Guid("{e3003765-95da-3c80-2716-530515a10732}"); } }

        /// <summary>
        /// Cireson Invoice (Full)
        /// Cireson.AssetManagement.Invoice
        /// </summary>
        public static Guid Invoice { get { return new Guid("{3333dfb2-ad14-f7c1-98dd-fec564c5a98a}"); } }

        /// <summary>
        /// Knowledge Article (advanced)
        /// System.Knowledge.ArticleProjection
        /// </summary>
        public static Guid KnowledgeArticle { get { return new Guid("{c421d300-5c3e-621b-73e8-e85e37aae4b7}"); } }

        /// <summary>
        /// Cireson Lease (Full)
        /// Cireson.AssetManagement.Lease
        /// </summary>
        public static Guid Lease { get { return new Guid("{97aaa390-0ce2-8612-e10f-d7df5e5a5147}"); } }

        /// <summary>
        /// Cireson License (Full)
        /// Cireson.AssetManagement.License
        /// </summary>
        public static Guid License { get { return new Guid("{cf0b962b-f78a-dc78-d586-ea5f991bfe50}"); } }

        /// <summary>
        /// Cireson Location (Full)
        /// Cireson.AssetManagement.Location
        /// </summary>
        public static Guid Location { get { return new Guid("{1887019a-8451-848e-ae65-23b385a97037}"); } }

        /// <summary>
        /// Manual Activity (advanced)
        /// System.WorkItem.Activity.ManualActivityProjection
        /// </summary>
        public static Guid ManualActivity { get { return new Guid("{d651bf4a-9f5d-0374-3e91-eb91111865c3}"); } }

        /// <summary>
        /// Cireson Organization (Full)
        /// Cireson.AssetManagement.Organization
        /// </summary>
        public static Guid Organization { get { return new Guid("{a24ece34-f3df-d7e5-f9df-cba08e374ffa}"); } }

        /// <summary>
        /// Parallel Activity (advanced)
        /// System.WorkItem.Activity.ParallelActivityProjection
        /// </summary>
        public static Guid ParallelActivity { get { return new Guid("{d69d13d3-2295-a9d2-b704-ada489d4a757}"); } }

        /// <summary>
        /// Printer (advanced)
        /// Microsoft.Windows.Printer.ProjectionType
        /// </summary>
        public static Guid Printer { get { return new Guid("{65e9c5e0-83a2-ed58-f57d-d912bf006ed4}"); } }

        /// <summary>
        /// Problem (advanced)
        /// System.WorkItem.Problem.ProjectionType
        /// </summary>
        public static Guid Problem { get { return new Guid("{45c1c404-f3fe-1050-dcef-530e1c2533e1}"); } }

        /// <summary>
        /// Cireson Purchase (Full)
        /// Cireson.AssetManagement.Purchase
        /// </summary>
        public static Guid Purchase { get { return new Guid("{376feaff-84be-25f5-0ad0-e591f843479c}"); } }

        /// <summary>
        /// Cireson Purchase Order (Full)
        /// Cireson.AssetManagement.PurchaseOrder
        /// </summary>
        public static Guid PurchaseOrder { get { return new Guid("{f27daae2-280c-dd8b-24e7-9bdb5120d6d2}"); } }

        /// <summary>
        /// Review Activity (advanced)
        /// System.WorkItem.Activity.ReviewActivityProjection
        /// </summary>
        public static Guid ReviewActivity { get { return new Guid("{125d26e0-03c7-adb5-7e4b-77f75adc9270}"); } }

        /// <summary>
        /// Reviewer (advanced)
        /// System.ReviewerProjection
        /// </summary>
        public static Guid Reviewer { get { return new Guid("{18476888-f6a2-e50a-2a73-d5f5cbc4c192}"); } }

        /// <summary>
        /// Sequential Activity (advanced)
        /// System.WorkItem.Activity.SequentialActivityProjection
        /// </summary>
        public static Guid SequentialActivity { get { return new Guid("{dc023860-a9e5-52e7-a823-79b6e8006f1a}"); } }

        /// <summary>
        /// Service Request (advanced)
        /// System.WorkItem.ServiceRequestProjection
        /// </summary>
        public static Guid ServiceRequest { get { return new Guid("{e44b7c06-590d-64d6-56d2-2219c5e763e0}"); } }

        /// <summary>
        /// Software (advanced)
        /// Microsoft.Windows.Software.ProjectionType
        /// </summary>
        public static Guid Software { get { return new Guid("{22cc3106-ce67-9e76-2e98-26309e12c153}"); } }

        /// <summary>
        /// Cireson Software Asset (Full)
        /// Cireson.AssetManagement.SoftwareAsset
        /// </summary>
        public static Guid SoftwareAsset { get { return new Guid("{989248a2-922b-03a7-f467-14897a6bdfe3}"); } }

        /// <summary>
        /// Software Update (advanced)
        /// Microsoft.Windows.SoftwareUpdate.ProjectionType
        /// </summary>
        public static Guid SoftwareUpdate { get { return new Guid("{4897f606-6105-ca7a-e294-7ac2b68d59fb}"); } }

        /// <summary>
        /// Cireson Standard
        /// Cireson.AssetManagement.Standard
        /// </summary>
        public static Guid Standard { get { return new Guid("{69078f60-d13f-57bd-1796-6c6f030b67be}"); } }

        /// <summary>
        /// Cireson Subnet
        /// Cireson.AssetManagement.Subnet
        /// </summary>
        public static Guid Subnet { get { return new Guid("{c7e75e4d-bbf6-6b26-f021-8ab4c9d719fb}"); } }

        /// <summary>
        /// Cireson Support Contract
        /// Cireson.AssetManagement.SupportContract
        /// </summary>
        public static Guid SupportContract { get { return new Guid("{1576e04b-f141-3677-10c7-023fee04450f}"); } }

        /// <summary>
        /// User (advanced)
        /// System.User.Projection
        /// </summary>
        public static Guid User { get { return new Guid("{0e1313ab-dc5c-cf9d-d6b0-e2e9835a132a}"); } }

        /// <summary>
        /// Cireson Vendor
        /// Cireson.AssetManagement.Vendor
        /// </summary>
        public static Guid Vendor { get { return new Guid("{233c4102-bc14-06f9-ec95-a151250f0242}"); } }

        /// <summary>
        /// Cireson Warranty
        /// Cireson.AssetManagement.Warranty
        /// </summary>
        public static Guid Warranty { get { return new Guid("{5b8253dc-df8d-bb53-a833-a9b99720330e}"); } }

        /// <summary>
        /// Work Item (advanced)
        /// System.WorkItem.Projection
        /// </summary>
        public static Guid WorkItem { get { return new Guid("{bfe4cd41-b43b-93fc-b9bf-109ffd8b8627}"); } }
    }
}
