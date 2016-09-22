using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CiresonPortalAPI.Tests.Unit
{
    /// <summary>
    /// Summary description for QueryCriteriaTests
    /// </summary>
    [TestClass]
    public class QueryCriteriaTests
    {
        #region Fields
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
        public QueryCriteriaTests() { }
        #endregion // Constructor

        #region CRITERIA01_SimpleExpressionSerializeTest
        [TestMethod]
        [Description("Builds and serializes a SimpleExpression QueryCriteria and compares to known good output")]
        [TestCategory("Unit - QueryCriteria")]
        public void CRITERIA01_SimpleExpressionSerializeTest()
        {
            // Arrange
            Guid baseId = new Guid("{b3e98851-27ab-4d70-bb10-3cf71c80d838}");
            string knownGood = "{\"Id\":\"" + TypeProjectionConstants.User.Id.ToString("D") + "\",\"Criteria\":{\"Base\":{\"Expression\":{\"SimpleExpression\":{\"ValueExpressionLeft\":{\"GenericProperty\":\"Id\"},\"Operator\":\"Equal\",\"ValueExpressionRight\":{\"Value\":\"" + baseId.ToString("D") + "\"}}}}}}";

            QueryCriteriaExpression expr = new QueryCriteriaExpression
            {
                PropertyName = "Id",
                PropertyType = QueryCriteriaPropertyType.GenericProperty,
                Operator = QueryCriteriaExpressionOperator.Equal,
                Value = baseId.ToString("D")
            };

            QueryCriteria criteria = new QueryCriteria(TypeProjectionConstants.User.Id)
            {
                GroupingOperator = QueryCriteriaGroupingOperator.SimpleExpression
            };

            criteria.Expressions.Add(expr);

            // Act
            string testSerialize = criteria.ToString();

            // Assert
            Assert.AreEqual(knownGood, testSerialize);
        }
        #endregion

        #region CRITERIA02_CompoundExpressionSerializeTest
        [TestMethod]
        [Description("Builds and serializes an AND QueryCriteria and compares to known good output")]
        [TestCategory("Unit - QueryCriteria")]
        public void CRITERIA02_CompoundExpressionSerializeTest()
        {
            // Arrange
            Guid typeGuid = new Guid("{2bc63f3a-a7a1-4ded-a727-b14f7b2cef69}");
            string poNumber = "Testing123";
            string knownGood = "{\"Id\":\"f27daae2-280c-dd8b-24e7-9bdb5120d6d2\",\"Criteria\":{\"Base\":{\"Expression\":{\"And\":{\"Expression\":[{\"SimpleExpression\":{\"ValueExpressionLeft\":{\"Property\":\"$Context/Property[Type='2afe355c-24a7-b20f-36e3-253b7249818d']/PurchaseOrderType$\"},\"Operator\":\"Equal\",\"ValueExpressionRight\":{\"Value\":\"" + typeGuid.ToString("B") + "\"}}},{\"SimpleExpression\":{\"ValueExpressionLeft\":{\"Property\":\"$Context/Property[Type='2afe355c-24a7-b20f-36e3-253b7249818d']/PurchaseOrderNumber$\"},\"Operator\":\"Equal\",\"ValueExpressionRight\":{\"Value\":\"" + poNumber + "\"}}}]}}}}}";

            QueryCriteriaExpression expr1 = new QueryCriteriaExpression
            {
                PropertyName = (new PropertyPathHelper(ClassConstants.PurchaseOrder.Id, "PurchaseOrderType")).ToString(),
                PropertyType = QueryCriteriaPropertyType.Property,
                Operator = QueryCriteriaExpressionOperator.Equal,
                Value = typeGuid.ToString("B")
            };

            QueryCriteriaExpression expr2 = new QueryCriteriaExpression
            {
                PropertyName = (new PropertyPathHelper(ClassConstants.PurchaseOrder.Id, "PurchaseOrderNumber")).ToString(),
                PropertyType = QueryCriteriaPropertyType.Property,
                Operator = QueryCriteriaExpressionOperator.Equal,
                Value = poNumber
            };

            QueryCriteria criteria = new QueryCriteria(TypeProjectionConstants.PurchaseOrder.Id);
            criteria.GroupingOperator = QueryCriteriaGroupingOperator.And;
            criteria.Expressions.Add(expr1);
            criteria.Expressions.Add(expr2);

            // Act
            string testSerialize = criteria.ToString();

            // Assert
            Assert.AreEqual(knownGood, testSerialize);
        }
        #endregion
    }
}
