using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CiresonPortalAPI.ConfigurationItems;

namespace CiresonPortalAPI.Tests.Integration.ConfigurationItems
{
    /// <summary>
    /// Summary description for PurchaseOrderTests
    /// </summary>
    [TestClass]
    public class PurchaseOrderTests
    {
        #region Fields
        private static List<PurchaseOrder> _objectsToCleanup;
        private static AuthorizationToken _authToken;
        private static PurchaseOrder _purchaseOrder;

        private TestContext _testContextInstance;
        #endregion // Fields

        #region Constructor
        public PurchaseOrderTests() { }
        #endregion // Constructor

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

        #region Class Initializer
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            _objectsToCleanup = new List<PurchaseOrder>();

            Task<AuthorizationToken> tokenTask = AuthorizationController.GetAuthorizationToken(ConfigurationHelper.PortalUrl, ConfigurationHelper.UserName, ConfigurationHelper.Password, ConfigurationHelper.Domain);
            tokenTask.Wait();
            _authToken = tokenTask.Result;
        }
        #endregion // Class Initializer

        #region Class Cleanup
        [ClassCleanup]
        public static void Cleanup()
        {
            foreach (PurchaseOrder obj in _objectsToCleanup)
            {
                Task<bool> deleteTask = PurchaseOrderController.Delete(_authToken, obj, false);
            }
        }
        #endregion // Class Cleanup

        #region PO01_CreatePurchaseOrderTest
        [TestMethod]
        [TestCategory("Integration - PurchaseOrders")]
        [Description("Tests creating a new PurchaseOrder")]
        public async Task PO01_CreatePurchaseOrderTest()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            string poName = "TestPurchaseOrder" + id.ToString();

            string strData;
            decimal? decData;
            Guid guidData;
            Enumeration enumData;
            DateTime? dateData;
            PurchaseOrder parent;
            RelatedObjectList<PurchaseOrder> children;

            // Act
            _purchaseOrder = await PurchaseOrderController.Create(_authToken, poName, "Test PurchaseOrder", poName, DateTime.Today);
            _objectsToCleanup.Add(_purchaseOrder);

            try
            {
                dateData = _purchaseOrder.OrderDate;

                decData = _purchaseOrder.Amount;

                enumData = _purchaseOrder.AssetStatus;
                enumData = _purchaseOrder.Currency;
                enumData = _purchaseOrder.OrderStatus;
                enumData = _purchaseOrder.OrderType;

                guidData = _purchaseOrder.BaseId;

                strData = _purchaseOrder.ClassName;
                strData = _purchaseOrder.DisplayName;
                strData = _purchaseOrder.FullName;
                strData = _purchaseOrder.OrderNumber;

                parent = _purchaseOrder.Parent;
                children = _purchaseOrder.Children;
            }
            // Assert
            catch (Exception e)
            {
                Assert.Fail("Expected no exception from property read test, got " + e.Message);
            }

            Assert.IsNotNull(_purchaseOrder);
            Assert.IsTrue(_purchaseOrder.ObjectStatus.Id == EnumerationConstants.TypeProjection.BuiltinValues.ObjectStatus.Active);
        }
        #endregion

        #region PO02_PurchaseOrderPropertiesCommitTest
        [TestMethod]
        [TestCategory("Integration - PurchaseOrders")]
        [Description("Tests committing changes to the test PurchaseOrder")]
        public async Task PO02_PurchaseOrderPropertiesCommitTest()
        {
            // Arrange
            DateTime date = DateTime.Parse(DateTime.Now.ToString()); // Convert current time to string first to remove unnecessary precision from the Ticks property
            decimal dec = 221.38M;
            decimal negDec = -1293.12M;

            // Act
            _purchaseOrder.Amount = dec;
            _purchaseOrder.OrderDate = date;

            try
            {
                _purchaseOrder.Amount = negDec;
                Assert.Fail("Expected exception from assigning a negative value to PurchaseOrder.Amount");
            }
            catch (ArgumentOutOfRangeException) {}
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }

            await _purchaseOrder.Commit(_authToken);

            // Assert
            Assert.IsNotNull(_purchaseOrder);
            Assert.AreEqual(dec, _purchaseOrder.Amount);
            Assert.IsTrue(date.Equals(_purchaseOrder.OrderDate));
        }
        #endregion

        #region PO03_GetAllPurchaseOrdersTest
        [TestMethod]
        [TestCategory("Integration - PurchaseOrders")]
        [Description("Tests fetching a list of all PurchaseOrders")]
        public async Task PO03_GetAllPurchaseOrdersTest()
        {
            // Arrange
            List<PurchaseOrder> poList;

            // Act
            poList = await PurchaseOrderController.GetAll(_authToken);

            // Assert
            Assert.IsNotNull(poList);
            Assert.IsTrue(poList.Count >= 1);
            Assert.IsTrue(poList.Contains(_purchaseOrder));
        }
        #endregion

        #region PO04_PurchaseOrderRelatedObjectCommitTest
        [TestMethod]
        [TestCategory("Integration - PurchaseOrders")]
        [Description("Tests making the changes to the relationships of PurchaseOrders")]
        public async Task PO04_PurchaseOrderRelatedObjectCommitTest()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            PurchaseOrder parent = await PurchaseOrderController.Create(_authToken, "TestParentPO" + id.ToString(), "Test ParentPO", "TestParentPO" + id.ToString(), DateTime.Now);
            PurchaseOrder child1 = await PurchaseOrderController.Create(_authToken, "TestChildPO1" + id.ToString(), "Test ChildPO1", "TestChildPO1" + id.ToString(), DateTime.Now);
            PurchaseOrder child2 = await PurchaseOrderController.Create(_authToken, "TestChildPO2" + id.ToString(), "Test ChildPO2", "TestChildPO2" + id.ToString(), DateTime.Now);

            _objectsToCleanup.Add(parent);
            _objectsToCleanup.Add(child1);
            _objectsToCleanup.Add(child2);

            // Act
            _purchaseOrder.Parent = parent;
            _purchaseOrder.Children.Add(child1);
            _purchaseOrder.Children.Add(child2);

            await _purchaseOrder.Commit(_authToken);

            // Assert
            Assert.AreEqual(parent, _purchaseOrder.Parent);
            Assert.IsTrue(_purchaseOrder.Children.Contains(child1));
            Assert.IsTrue(_purchaseOrder.Children.Contains(child2));
        }
        #endregion

        #region PO99_DeletePurchaseOrderTest
        [TestMethod]
        [TestCategory("Integration - PurchaseOrders")]
        [Description("Tests deleting PurchaseOrders")]
        public async Task PO99_DeletePurchaseOrderTest()
        {
            // Arrange
            bool deleted;

            // Act
            deleted = await PurchaseOrderController.Delete(_authToken, _purchaseOrder, false);

            // Assert
            Assert.IsTrue(deleted);

            if (deleted)
                _objectsToCleanup.Remove(_purchaseOrder);
        }
        #endregion
    }
}
