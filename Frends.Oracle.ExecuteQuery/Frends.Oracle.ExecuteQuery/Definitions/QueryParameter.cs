using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Oracle.ExecuteQuery.Definitions
{
    /// <summary>
    /// Query parameter.
    /// </summary>
    public class QueryParameter
    {
        /// <summary>
        /// The name of the parameter.
        /// </summary>
        [DefaultValue("ParameterName")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Name { get; set; }

        /// <summary>
        /// The value of the parameter.
        /// </summary>
        [DefaultValue("Parameter value")]
        [DisplayFormat(DataFormatString = "Text")]
        public dynamic Value { get; set; }

        /// <summary>
        /// The type of the parameter.
        /// </summary>
        [DefaultValue(QueryParameterType.NVarchar2)]
        public QueryParameterType DataType { get; set; }
    }
}
