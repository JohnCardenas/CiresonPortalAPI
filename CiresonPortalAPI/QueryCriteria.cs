using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CiresonPortalAPI
{
    /// <summary>
    /// Query criteria grouping operator types
    /// </summary>
    public enum QueryCriteriaGroupingOperator
    {
        /// <summary>
        /// Simple expression, no grouping
        /// </summary>
        SimpleExpression,

        /// <summary>
        /// AND grouping
        /// </summary>
        And,

        /// <summary>
        /// OR grouping
        /// </summary>
        Or
    }

    /// <summary>
    /// Query criteria expression operator types
    /// </summary>
    public enum QueryCriteriaExpressionOperator
    {
        /// <summary>
        /// Property must equal the value
        /// </summary>
        Equal,

        /// <summary>
        /// Property must be greater than the value
        /// </summary>
        Greater,

        /// <summary>
        /// Property must be greater than or equal to the value
        /// </summary>
        GreaterEqual,

        /// <summary>
        /// Property must be less than the value
        /// </summary>
        Less,

        /// <summary>
        /// Property must be less than or equal to the value
        /// </summary>
        LessEqual,

        /// <summary>
        /// Property must be like the value - use % to denote wildcards in the value
        /// </summary>
        Like,

        /// <summary>
        /// Property must not be equal to the value
        /// </summary>
        NotEqual
    }

    /// <summary>
    /// Query criteria property types
    /// </summary>
    public enum QueryCriteriaPropertyType
    {
        /// <summary>
        /// Query criteria by Service Manager property name
        /// </summary>
        Property,

        /// <summary>
        /// Query criteria by generic property name
        /// </summary>
        GenericProperty
    }

    /// <summary>
    /// The QueryCriteria class is passed to GetProjectionByCriteria to define the criteria for the projection query
    /// </summary>
    [JsonConverter(typeof(QueryCriteriaSerializer))]
    public class QueryCriteria
    {
        private Guid _oProjectionID;
        private List<QueryCriteriaExpression> _lExpressions;

        /// <summary>
        /// List of QueryCriteriaExpression objects
        /// </summary>
        public List<QueryCriteriaExpression> Expressions { get { return _lExpressions; } }

        /// <summary>
        /// The ID of the type projection we want to query
        /// </summary>
        public Guid ProjectionID { get { return _oProjectionID; } internal set { _oProjectionID = value; } }

        /// <summary>
        /// What type of grouping operator should be used?
        /// </summary>
        public QueryCriteriaGroupingOperator GroupingOperator = QueryCriteriaGroupingOperator.SimpleExpression;

        /// <summary>
        /// Generates a new QueryCriteria for the desired projection type
        /// </summary>
        /// <param name="projectionId">ID of the desired projection type, refer to TypeProjectionConstants for well-known projections</param>
        public QueryCriteria(Guid projectionId)
        {
            _oProjectionID = projectionId;

            _lExpressions = new List<QueryCriteriaExpression>();
        }

        /// <summary>
        /// Serializes this QueryCriteria to a JSON string without formatting
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }

        /// <summary>
        /// Serializes this QueryCriteria to a JSON string with human-readable formatting
        /// </summary>
        /// <returns></returns>
        public string ToIndentedString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// Query criteria expression, used by QueryCriteria.Expressions list
    /// </summary>
    public class QueryCriteriaExpression
    {
        /// <summary>
        /// Name of the property to query
        /// </summary>
        public string PropertyName = null;

        /// <summary>
        /// Type of the property - direct Property or a GenericProperty
        /// </summary>
        public QueryCriteriaPropertyType PropertyType;

        /// <summary>
        /// What type of comparison to perform
        /// </summary>
        public QueryCriteriaExpressionOperator Operator;

        /// <summary>
        /// Value to check the property for
        /// </summary>
        public string Value = null;

        /// <summary>
        /// Returns true if this QueryCriteriaExpression is valid and fully populated
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return (!String.IsNullOrEmpty(PropertyName)) && Enum.IsDefined(typeof(QueryCriteriaPropertyType), PropertyType) &&
                   Enum.IsDefined(typeof(QueryCriteriaExpressionOperator), Operator) && (!String.IsNullOrEmpty(Value));
        }

        /// <summary>
        /// Create a new QueryCriteriaExpression
        /// </summary>
        /// <param name="propertyName">Property to query for a value</param>
        /// <param name="propertyType">Type of the property - direct Property or GenericProperty</param>
        /// <param name="qcOperator">What type of comparison should we perform?</param>
        /// <param name="value">Value to check the property for</param>
        public QueryCriteriaExpression(string propertyName, QueryCriteriaPropertyType propertyType, QueryCriteriaExpressionOperator qcOperator, string value)
        {
            this.PropertyName = propertyName;
            this.PropertyType = propertyType;
            this.Operator = qcOperator;
            this.Value = value;
        }

        /// <summary>
        /// Default constructor - you'll need to populate your own property values
        /// </summary>
        public QueryCriteriaExpression() { }
    }

    /// <summary>
    /// Custom serializer for QueryCriteria objects
    /// </summary>
    public class QueryCriteriaSerializer : JsonConverter
    {
        /// <summary>
        /// Takes a QueryCriteria object and serializes it to JSON format
        /// </summary>
        /// <param name="writer">Writer in use</param>
        /// <param name="value">QueryCriteria object to serialize</param>
        /// <param name="serializer">Serializer in use</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            QueryCriteria criteria = (QueryCriteria)value;

            // Error checking
            if (criteria.Expressions.Count == 0)
            {
                throw new JsonSerializationException("Cannot serialize QueryCriteria with no expressions!");
            }
            else if (criteria.GroupingOperator == QueryCriteriaGroupingOperator.SimpleExpression && criteria.Expressions.Count > 1)
            {
                throw new JsonSerializationException("Cannot serialize QueryCriteria as SimpleExpression with more than one expression!");
            }
            else if ((criteria.GroupingOperator == QueryCriteriaGroupingOperator.And || criteria.GroupingOperator == QueryCriteriaGroupingOperator.Or) && criteria.Expressions.Count < 2)
            {
                throw new JsonSerializationException("Cannot serialize QueryCriteria as non-SimpleExpression with less than two expressions!");
            }
            else
            {
                // Check that all our expressions are valid
                foreach (QueryCriteriaExpression expression in criteria.Expressions)
                {
                    if (!expression.IsValid())
                        throw new JsonSerializationException("QueryCriteria contains an invalid expression!");
                }
            }

            // Let's go!
            writer.WriteStartObject();

            // Id Property
            writer.WritePropertyName("Id");
            writer.WriteValue(criteria.ProjectionID.ToString("D"));

            #region Criteria JSON object
            {
                writer.WritePropertyName("Criteria");
                writer.WriteStartObject();

                #region Base JSON object
                {
                    writer.WritePropertyName("Base");
                    writer.WriteStartObject();

                    #region Expression JSON object
                    {
                        writer.WritePropertyName("Expression");
                        writer.WriteStartObject();

                        // If we're not doing any kind of logical grouping, just dump out the SimpleExpression
                        if (criteria.GroupingOperator == QueryCriteriaGroupingOperator.SimpleExpression)
                        {
                            WriteSimpleExpression(writer, criteria.Expressions[0]);
                        }
                        else
                        {
                            // If we're doing logical grouping, we need to enumerate all the expressions and dump them out
                            #region And/Or object
                            {
                                writer.WritePropertyName(criteria.GroupingOperator.ToString());
                                writer.WriteStartObject();

                                #region Nested Expression object
                                {
                                    writer.WritePropertyName("Expression");
                                    writer.WriteStartArray();

                                    // Iterate over the expressions and write them
                                    foreach (QueryCriteriaExpression expression in criteria.Expressions)
                                    {
                                        writer.WriteStartObject();
                                        WriteSimpleExpression(writer, expression);
                                        writer.WriteEndObject();
                                    }

                                    writer.WriteEndArray();
                                }
                                #endregion Nested Expression object

                                writer.WriteEndObject();
                            }
                            #endregion
                        }

                        writer.WriteEndObject();
                    }
                    #endregion Expression JSON object

                    writer.WriteEndObject();
                }
                #endregion Base JSON object

                writer.WriteEndObject();
            }
            #endregion Criteria JSON object

            writer.WriteEndObject(); // JSON
        }

        /// <summary>
        /// Takes a QueryCriteriaExpression and serializes it as a SimpleExpression JSON object
        /// </summary>
        /// <param name="writer">Writer to use</param>
        /// <param name="expression">QueryCriteriaExpression object to serialize</param>
        internal static void WriteSimpleExpression(JsonWriter writer, QueryCriteriaExpression expression)
        {
            writer.WritePropertyName("SimpleExpression");
            writer.WriteStartObject();

            #region ValueExpressionLeft JSON object
            {
                writer.WritePropertyName("ValueExpressionLeft");
                writer.WriteStartObject();

                #region Property/GenericProperty Property
                {
                    writer.WritePropertyName(expression.PropertyType.ToString());
                    writer.WriteValue(expression.PropertyName);
                }
                #endregion Property/GenericProperty Property

                writer.WriteEndObject();
            }
            #endregion ValueExpressionLeft JSON object

            #region Operator Property
            {
                writer.WritePropertyName("Operator");
                writer.WriteValue(expression.Operator.ToString());
            }
            #endregion Operator Property

            #region ValueExpressionRight JSON object
            {
                writer.WritePropertyName("ValueExpressionRight");
                writer.WriteStartObject();

                #region Value property
                writer.WritePropertyName("Value");
                writer.WriteValue(expression.Value);
                #endregion Value property

                writer.WriteEndObject();
            }
            #endregion ValueExpressionRight JSON object

            writer.WriteEndObject();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}