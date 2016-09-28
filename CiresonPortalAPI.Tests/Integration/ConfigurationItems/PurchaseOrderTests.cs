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
                PurchaseOrderController.Delete(_authToken, obj, false).Wait();
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

            // Act
            _purchaseOrder = await PurchaseOrderController.Create(_authToken, poName, "Test PurchaseOrder", poName, DateTime.Today);
            _objectsToCleanup.Add(_purchaseOrder);

            // Assert
            Assert.IsNotNull(_purchaseOrder, "Failed to create new PurchaseOrder");
            Assert.IsTrue(_purchaseOrder.IsActive, "PurchaseOrder.IsActive evaluated to false");
        }
        #endregion

        #region PO02_PurchaseOrderReadPropertyTest
        [TestMethod]
        [TestCategory("Integration - PurchaseOrders")]
        [Description("Tests reading PurchaseOrder properties")]
        public void PO02_PurchaseOrderReadPropertyTest()
        {
            // Arrange
            string strData;
            decimal? decData;
            Guid guidData;
            Enumeration enumData;
            DateTime? dateData;
            PurchaseOrder parent;
            RelatedObjectList<PurchaseOrder> children;

            // Act
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
        }
        #endregion

        #region PO03_SetPurchaseOrderPrimitivesCommitTest
        [TestMethod]
        [TestCategory("Integration - PurchaseOrders")]
        [Description("Tests committing changes to primitive PurchaseOrder properties")]
        public async Task PO03_SetPurchaseOrderPrimitivesCommitTest()
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
            Assert.AreEqual(dec, _purchaseOrder.Amount, "Expected PurchaseOrder.Amount = " + dec + ", got: " + _purchaseOrder.Amount);
            Assert.IsTrue(date.Equals(_purchaseOrder.OrderDate), "PurchaseOrder.PurchaseDate does not match test data");
        }
        #endregion

        #region PO04_ClearPurchaseOrderPrimitivesCommitTest
        [TestMethod]
        [TestCategory("Integration - PurchaseOrders")]
        [Description("Clears and commits primitive PurchaseOrder properties")]
        public async Task PO04_ClearPurchaseOrderPrimitivesCommitTest()
        {
            // Arrange
            _purchaseOrder.Amount = null;
            _purchaseOrder.OrderDate = null;

            // Act
            await _purchaseOrder.Commit(_authToken);

            // Assert
            Assert.IsNull(_purchaseOrder.Amount,    "PurchaseOrder.Amount was not cleared successfully");
            Assert.IsNull(_purchaseOrder.OrderDate, "PurchaseOrder.OrderDate was not cleared successfully");
        }
        #endregion

        #region PO05_GetAllPurchaseOrdersTest
        [TestMethod]
        [TestCategory("Integration - PurchaseOrders")]
        [Description("Tests fetching a list of all PurchaseOrders")]
        public async Task PO05_GetAllPurchaseOrdersTest()
        {
            // Arrange
            List<PurchaseOrder> poList;

            // Act
            poList = await PurchaseOrderController.GetAll(_authToken);

            // Assert
            Assert.IsNotNull(poList, "Expected a List<PurchaseOrder>, got null");
            Assert.IsTrue(poList.Count >= 1, "Expected at least one member of this list, got " + poList.Count);
            Assert.IsTrue(poList.Contains(_purchaseOrder), "List does not contain the test PurchaseOrder");
        }
        #endregion

        #region PO06_SetPurchaseOrderParentCommitTest
        [TestMethod]
        [TestCategory("Integration - PurchaseOrders")]
        [Description("Performs a commit test on a PurchaseOrder.Parent")]
        public async Task PO06_SetPurchaseOrderParentCommitTest()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            PurchaseOrder parent = await PurchaseOrderController.Create(_authToken, "TestParentPO" + id.ToString(), "Test ParentPO", "TestParentPO" + id.ToString(), DateTime.Now);
            _objectsToCleanup.Add(parent);
            _purchaseOrder.Parent = parent;

            // Act
            await _purchaseOrder.Commit(_authToken);

            // Assert
            Assert.AreEqual(parent, _purchaseOrder.Parent, "PuchaseOrder.Parent does not match the test data");
        }
        #endregion

        #region PO07_ClearPurchaseOrderParentCommitTest
        [TestMethod]
        [TestCategory("Integration - PurchaseOrders")]
        [Description("Clears and commits PurchaseOrder.Parent")]
        public async Task PO07_ClearPurchaseOrderParentCommitTest()
        {
            // Arrange
            _purchaseOrder.Parent = null;

            // Act
            await _purchaseOrder.Commit(_authToken);

            // Assert
            Assert.IsNull(_purchaseOrder.Parent, "PurchaseOrder.Parent was not cleared successfully");
        }
        #endregion

        #region PO08_SetPurchaseOrderChildrenCommitTest
        [TestMethod]
        [TestCategory("Integration - PurchaseOrders")]
        [Description("Performs a commit test on PurchaseOrder.Children")]
        public async Task PO08_PurchaseOrderRelatedObjectCommitTest()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            
            PurchaseOrder child1 = await PurchaseOrderController.Create(_authToken, "TestChildPO1" + id.ToString(), "Test ChildPO1", "TestChildPO1" + id.ToString(), DateTime.Now);
            PurchaseOrder child2 = await PurchaseOrderController.Create(_authToken, "TestChildPO2" + id.ToString(), "Test ChildPO2", "TestChildPO2" + id.ToString(), DateTime.Now);

            _objectsToCleanup.Add(child1);
            _objectsToCleanup.Add(child2);

            _purchaseOrder.Children.Add(child1);
            _purchaseOrder.Children.Add(child2);

            // Act
            await _purchaseOrder.Commit(_authToken);

            // Assert
            Assert.AreEqual(2, _purchaseOrder.Children.Count, "Expected 2 PurchaseOrder.Children, got " + _purchaseOrder.Children.Count);
            Assert.IsTrue(_purchaseOrder.Children.Contains(child1), "PurchaseOrder.Children does not contain the first test child");
            Assert.IsTrue(_purchaseOrder.Children.Contains(child2), "PurchaseOrder.Children does not contain the second test child");
        }
        #endregion

        #region PO09_ClearPurchaseOrderChildrenCommitTest
        [TestMethod]
        [TestCategory("Integration - PurchaseOrders")]
        [Description("Clears and commits PurchaseOrder.Children")]
        public async Task PO09_ClearPurchaseOrderChildrenCommitTest()
        {
            // Arrange
            _purchaseOrder.Children.Clear();

            // Act
            await _purchaseOrder.Commit(_authToken);

            // Assert
            Assert.AreEqual(0, _purchaseOrder.Children.Count, "Expected 0 PurchaseOrder.Children, got " + _purchaseOrder.Children.Count);
        }
        #endregion



        #region PO99_DeletePurchaseOrderTest
        [TestMethod]
        [TestCategory("Integration - PurchaseOrders")]
        [Description("Tests deleting PurchaseOrders")]
        public async Task PO99_DeletePurchaseOrderTest()
        {
            // Arrange

            // Act
            await PurchaseOrderController.Delete(_authToken, _purchaseOrder, false);

            // Assert
            Assert.IsTrue(_purchaseOrder.IsDeleted, "PurchaseOrder.IsDeleted evaluated to false");

            if (_purchaseOrder.IsDeleted)
                _objectsToCleanup.Remove(_purchaseOrder);
        }
        #endregion
    }
}
