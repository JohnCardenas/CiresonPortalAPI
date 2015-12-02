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
        /// Active Directory User (Microsoft.AD.User)
        /// </summary>
        public static Guid ADUser { get { return new Guid("{10a7f898-e672-ccf3-8881-360bfb6a8f9a}"); } }

        /// <summary>
        /// Catalog Item (Cireson.AssetManagement.CatalogItem)
        /// </summary>
        public static Guid CatalogItem { get { return new Guid("{c0c58e7f-7865-55cc-4600-753305b9be64}"); } }
        
        /// <summary>
        /// Change Request (System.WorkItem.ChangeRequest)
        /// </summary>
        public static Guid ChangeRequest { get { return new Guid("{e6c9cf6e-d7fe-1b5d-216c-c3f5d2c7670c}"); } }

        /// <summary>
        /// Configuration Item (System.ConfigItem)
        /// </summary>
        public static Guid ConfigItem { get { return new Guid("{62f0be9f-ecea-e73c-f00d-3dd78a7422fc}"); } }

        /// <summary>
        /// Cost Center (Cireson.AssetManagement.CostCenter)
        /// </summary>
        public static Guid CostCenter { get { return new Guid("{128bdb2d-f5bd-f8b6-440e-e3f7d8ab4858}"); } }

        /// <summary>
        /// Domain User (System.Domain.User)
        /// </summary>
        public static Guid DomainUser { get { return new Guid("{eca3c52a-f273-5cdc-f165-3eb95a2b26cf}"); } }

        /// <summary>
        /// Hardware Asset (Cireson.AssetManagement.HardwareAsset)
        /// </summary>
        public static Guid HardwareAsset { get { return new Guid("{c0c58e7f-7865-55cc-4600-753305b9be64}"); } }

        /// <summary>
        /// Incident (System.WorkItem.Incident)
        /// </summary>
        public static Guid Incident { get { return new Guid("{a604b942-4c7b-2fb2-28dc-61dc6f465c68}"); } }

        /// <summary>
        /// Invoice (Cireson.AssetManagement.Invoice)
        /// </summary>
        public static Guid Invoice { get { return new Guid("{e57c1c12-16cf-3e2d-576e-c9be562a1a37}"); } }

        /// <summary>
        /// Lease (Cireson.AssetManagement.Lease)
        /// </summary>
        public static Guid Lease { get { return new Guid("{da47b130-6bf6-10eb-b8a0-a9288b729160}"); } }

        /// <summary>
        /// License (Cireson.AssetManagement.License)
        /// </summary>
        public static Guid License { get { return new Guid("{a3ad0993-def0-e2ff-dbcf-9ca04040a219}"); } }

        /// <summary>
        /// Location (Cireson.AssetManagement.Location)
        /// </summary>
        public static Guid Location { get { return new Guid("{b1ae24b1-f520-4960-55a2-62029b1ea3f0}"); } }

        /// <summary>
        /// Manual Activity (System.WorkItem.Activity.ManualActivity)
        /// </summary>
        public static Guid ManualActivity { get { return new Guid("{7ac62bd4-8fce-a150-3b40-16a39a61383d}"); } }

        /// <summary>
        /// Organization (Cireson.AssetManagement.Organization)
        /// </summary>
        public static Guid Organization { get { return new Guid("{ed0d8659-fba9-6e08-c213-5cd88f5480a8}"); } }

        /// <summary>
        /// Problem (System.WorkItem.Problem)
        /// </summary>
        public static Guid Problem { get { return new Guid("{422afc88-5eff-f4c5-f8f6-e01038cde67f}"); } }

        /// <summary>
        /// Purchase (Cireson.AssetManagement.Purchase)
        /// </summary>
        public static Guid Purchase { get { return new Guid("{001556ed-3ad5-5640-fee8-beb748da9e03}"); } }

        /// <summary>
        /// Purchase Order (Cireson.AssetManagement.PurchaseOrder)
        /// </summary>
        public static Guid PurchaseOrder { get { return new Guid("{2afe355c-24a7-b20f-36e3-253b7249818d}"); } }

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

        /// <summary>
        /// Software Asset (Cireson.AssetManagement.SoftwareAsset)
        /// </summary>
        public static Guid SoftwareAsset { get { return new Guid("{81e3da4f-e41c-311e-5b05-3ca779d030db}"); } }

        /// <summary>
        /// Standard (Cireson.AssetManagement.Standard)
        /// </summary>
        public static Guid Standard { get { return new Guid("{f096d2f1-22cc-6b40-3b49-a615a5946ce5}"); } }

        /// <summary>
        /// Subnet (Cireson.AssetManagement.Subnet)
        /// </summary>
        public static Guid Subnet { get { return new Guid("{1a7a71a6-de25-61c4-a9c6-a3420c5a8564}"); } }

        /// <summary>
        /// Support Contract (Cireson.AssetManagement.SupportContract)
        /// </summary>
        public static Guid SupportContract { get { return new Guid("{b2c105d4-d8c7-b57c-fe3d-205d47e07141}"); } }

        /// <summary>
        /// User (System.User)
        /// </summary>
        public static Guid User { get { return new Guid("{943d298f-d79a-7a29-a335-8833e582d252}"); } }

        /// <summary>
        /// Vendor (Cireson.AssetManagement.Vendor)
        /// </summary>
        public static Guid Vendor { get { return new Guid("{f26c94f2-1045-3d60-4c1f-59b8cbfe9931}"); } }
    }
}
