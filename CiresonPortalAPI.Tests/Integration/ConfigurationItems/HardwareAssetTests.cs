using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CiresonPortalAPI.ConfigurationItems;

namespace CiresonPortalAPI.Tests.Integration.ConfigurationItems
{
    /// <summary>
    /// Summary description for HardwareAssetTests
    /// </summary>
    [TestClass]
    public class HardwareAssetTests
    {
        #region Fields
        private static HardwareAsset _asset;

        private static AuthorizationToken _authToken;
        private static List<ConfigurationItem> _objectsToCleanup;
        private TestContext _testContextInstance;
        #endregion // Fields

        #region Properties
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return _testContextInstance; }
            set { _testContextInstance = value; }
        }
        #endregion // Properties

        #region Constructor
        public HardwareAssetTests() { }
        #endregion // Constructor

        #region Class Initializer
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            _objectsToCleanup = new List<ConfigurationItem>();

            Task<AuthorizationToken> tokenTask = AuthorizationController.GetAuthorizationToken(ConfigurationHelper.PortalUrl, ConfigurationHelper.UserName, ConfigurationHelper.Password, ConfigurationHelper.Domain);
            tokenTask.Wait();
            _authToken = tokenTask.Result;
        }
        #endregion // Class Initializer

        #region Class Cleanup
        [ClassCleanup]
        public static void Cleanup()
        {
            foreach (ConfigurationItem obj in _objectsToCleanup)
            {
                Task<bool> deleteTask;

                if (obj is PurchaseOrder)
                    deleteTask = PurchaseOrderController.Delete(_authToken, (obj as PurchaseOrder), false);
                else if (obj is Location)
                    deleteTask = LocationController.Delete(_authToken, (obj as Location), false);
                else
                    deleteTask = HardwareAssetController.Delete(_authToken, (obj as HardwareAsset), false);

                deleteTask.Wait();
            }
        }
        #endregion // Class Cleanup

        #region HWA01_CreateHardwareAssetTest
        [TestMethod]
        [TestCategory("Integration - HardwareAssets")]
        [Description("Creates a new HardwareAsset")]
        public async Task HWA01_CreateHardwareAssetTest()
        {
            // Arrange
            Guid userId = _authToken.User.Id;
            Guid testId = Guid.NewGuid();
            string model = "Testiplex 420" + testId.ToString();
            string manufacturer = "Doll" + testId.ToString();
            string assetTag = "ABC-A1234" + testId.ToString();
            string serialNumber = "SDHZ2891000000000K13" + testId.ToString();

            // Act
            _asset = await HardwareAssetController.Create(_authToken, model, manufacturer, assetTag, serialNumber);
            _objectsToCleanup.Add(_asset);

            // Assert
            Assert.IsNotNull(_asset, "Expected a HardwareAsset, got null");
            Assert.IsTrue(_asset.IsActive, "Expected HardwareAsset.IsActive to be true, got false");
            Assert.AreEqual(model, _asset.Model, "HardwareAsset.Model does not match test data");
            Assert.AreEqual(manufacturer, _asset.Manufacturer, "HardwareAsset.Manufacturer does not match test data");
            Assert.AreEqual(assetTag, _asset.AssetTag, "HardwareAsset.AssetTag does not match test data");
            Assert.AreEqual(serialNumber, _asset.SerialNumber, "HardwareAsset.SerialNumber does not match test data");
        }
        #endregion

        #region HWA02_HardwareAssetReadPropertyTest
        [TestMethod]
        [TestCategory("Integration - HardwareAssets")]
        [Description("Performs a read test on all HardwareAsset properties")]
        public void HWA02_HardwareAssetReadPropertyTest()
        {
            // Arrange
            PurchaseOrder poData;
            User userData;
            Location locData;
            string strData;
            DateTime? dateData;

            // Act
            try
            {
                strData = _asset.AssetTag;
                strData = _asset.HardwareAssetID;
                strData = _asset.Manufacturer;
                strData = _asset.Model;
                strData = _asset.Name;
                strData = _asset.Notes;
                strData = _asset.SerialNumber;

                userData = _asset.Custodian;

                locData = _asset.Location;

                poData = _asset.PurchaseOrder;

                dateData = _asset.ReceivedDate;
            }
            // Assert
            catch (Exception e)
            {
                Assert.Fail("Expected no exception from property read test, got " + e.Message);
            }
        }
        #endregion

        #region HWA03_SetHardwareAssetPrimitivesCommitTest
        [TestMethod]
        [TestCategory("Integration - HardwareAssets")]
        [Description("Performs a commit test on primitive HardwareAsset properties")]
        public async Task HWA03_SetHardwareAssetPrimitivesCommitTest()
        {
            // Arrange
            Guid testId = Guid.NewGuid();
            string testString = "1234567890 abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ !@#$%^&*()-_=+[{]}\\|;:'\",<.>/?`~" + testId.ToString();
            DateTime testDate = DateTime.Parse(DateTime.Now.ToString()); // Convert current time to string first to remove unnecessary precision from the Ticks property

            // Act
            _asset.AssetTag = testString;
            _asset.Manufacturer = testString;
            _asset.Model = testString;
            _asset.Name = testString;
            _asset.Notes = testString;
            _asset.ReceivedDate = testDate;
            _asset.SerialNumber = testString;

            await _asset.Commit(_authToken);

            // Assert
            Assert.AreEqual(testString, _asset.AssetTag,     "HardwareAsset.AssetTag does not match test data");
            Assert.AreEqual(testString, _asset.Manufacturer, "HardwareAsset.Manufacturer does not match test data");
            Assert.AreEqual(testString, _asset.Model,        "HardwareAsset.Model does not match test data");
            Assert.AreEqual(testString, _asset.Name,         "HardwareAsset.Name does not match test data");
            Assert.AreEqual(testString, _asset.Notes,        "HardwareAsset.Notes does not match test data");
            Assert.AreEqual(testDate,   _asset.ReceivedDate, "HardwareAsset.ReceivedDate does not match test data");
            Assert.AreEqual(testString, _asset.SerialNumber, "HardwareAsset.SerialNumber does not match test data");
        }
        #endregion

        #region HWA04_ClearHardwareAssetPrimitivesCommitTest
        [TestMethod]
        [TestCategory("Integration - HardwareAssets")]
        [Description("Clears and commits primitive HardwareAsset properties")]
        public async Task HWA04_ClearHardwareAssetPrimitivesCommitTest()
        {
            // Arrange
            _asset.AssetTag = null;
            _asset.Manufacturer = null;
            _asset.Model = null;
            _asset.Name = null;
            _asset.Notes = null;
            _asset.ReceivedDate = null;
            _asset.SerialNumber = null;

            // Act
            await _asset.Commit(_authToken);

            // Assert
            Assert.IsTrue(string.IsNullOrEmpty(_asset.AssetTag),     "HardwareAsset.AssetTag was not cleared successfully");
            Assert.IsTrue(string.IsNullOrEmpty(_asset.Manufacturer), "HardwareAsset.Manufacturer was not cleared successfully");
            Assert.IsTrue(string.IsNullOrEmpty(_asset.Model),        "HardwareAsset.Model was not cleared successfully");
            Assert.IsTrue(string.IsNullOrEmpty(_asset.Name),         "HardwareAsset.Name was not cleared successfully");
            Assert.IsTrue(string.IsNullOrEmpty(_asset.Notes),        "HardwareAsset.Notes was not cleared successfully");
            Assert.IsTrue(string.IsNullOrEmpty(_asset.SerialNumber), "HardwareAsset.SerialNumber was not cleared successfully");

            Assert.IsNull(_asset.ReceivedDate, "HardwareAsset.ReceivedDate was not cleared successfully");
        }
        #endregion

        #region HWA05_SetHardwareAssetCustodianCommitTest
        [TestMethod]
        [TestCategory("Integration - HardwareAssets")]
        [Description("Performs a commit test on a HardwareAsset.Custodian")]
        public async Task HWA05_SetHardwareAssetCustodianCommitTest()
        {
            // Arrange
            User user = await UserController.GetUserById(_authToken, _authToken.User.Id);

            // Act
            _asset.Custodian = user;
            await _asset.Commit(_authToken);

            // Assert
            Assert.AreEqual(user, _asset.Custodian, "HardwareAsset.Custodian does not match the test data");
        }
        #endregion

        #region HWA06_ClearHardwareAssetCustodianCommitTest
        [TestMethod]
        [TestCategory("Integration - HardwareAssets")]
        [Description("Clears and commits the HardwareAsset.Custodian property")]
        public async Task HWA06_ClearHardwareAssetCustodianCommitTest()
        {
            // Arrange
            _asset.Custodian = null;

            // Act
            await _asset.Commit(_authToken);

            // Assert
            Assert.IsNull(_asset.Custodian, "HardwareAsset.Custodian was not cleared successfully");
        }
        #endregion

        #region HWA07_SetHardwareAssetPurchaseOrderCommitTest
        [TestMethod]
        [TestCategory("Integration - HardwareAssets")]
        [Description("Performs a commit test on a HardwareAsset.PurchaseOrder")]
        public async Task HWA07_SetHardwareAssetPurchaseOrderCommitTest()
        {
            // Arrange
            Guid testId = Guid.NewGuid();
            PurchaseOrder po = await PurchaseOrderController.Create(_authToken, testId.ToString(), testId.ToString(), testId.ToString(), DateTime.Now);
            _objectsToCleanup.Add(po);
            _asset.PurchaseOrder = po;

            // Act
            await _asset.Commit(_authToken);

            // Assert
            Assert.AreEqual(po, _asset.PurchaseOrder, "HardwareAsset.PurchaseOrder does not match the test data");
        }
        #endregion

        #region HWA08_ClearHardwareAssetPurchaseOrderCommitTest
        [TestMethod]
        [TestCategory("Integration - HardwareAssets")]
        [Description("Clears and commits the HardwareAsset.PurchaseOrder property")]
        public async Task HWA08_ClearHardwareAssetPurchaseOrderCommitTest()
        {
            // Arrange
            _asset.PurchaseOrder = null;

            // Act
            await _asset.Commit(_authToken);

            // Assert
            Assert.IsNull(_asset.PurchaseOrder, "HardwareAsset.PurchaseOrder was not cleared successfully");
        }
        #endregion

        #region HWA09_SetHardwareAssetLocationCommitTest()
        [TestMethod]
        [TestCategory("Integration - HardwareAssets")]
        [Description("Performs a commit test on a HardwareAsset.Location")]
        public async Task HWA09_SetHardwareAssetLocationCommitTest()
        {
            // Arrange
            Guid testId = Guid.NewGuid();
            Location loc = await LocationController.Create(_authToken, testId.ToString(), testId.ToString());
            _objectsToCleanup.Add(loc);
            _asset.Location = loc;

            // Act
            await _asset.Commit(_authToken);

            // Assert
            Assert.AreEqual(loc, _asset.Location, "HardwareAsset.Location does not match the test data");
        }
        #endregion

        #region HWA10_ClearHardwareAssetLocationCommitTest()
        [TestMethod]
        [TestCategory("Integration - HardwareAssets")]
        [Description("Clears and commits the HardwareAsset.Location property")]
        public async Task HWA10_ClearHardwareAssetLocationCommitTest()
        {
            // Arrange
            _asset.Location = null;

            // Act
            await _asset.Commit(_authToken);

            // Assert
            Assert.IsNull(_asset.Location, "HardwareAsset.Location was not cleared successfully");
        }
        #endregion

        #region HWA99_DeleteHardwareAssetTest
        [TestMethod]
        [TestCategory("Integration - HardwareAssets")]
        [Description("Tests marking a HardwareAsset as deleted")]
        public async Task HWA99_DeleteHardwareAssetTest()
        {
            // Arrange

            // Act
            await HardwareAssetController.Delete(_authToken, _asset, false);

            // Assert
            Assert.IsTrue(_asset.IsDeleted, "HardwareAsset.IsDeleted evaluated to false");

            if (_asset.IsDeleted)
                _objectsToCleanup.Remove(_asset);
        }
        #endregion
    }
}
