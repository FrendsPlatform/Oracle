using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Oracle.ExecuteQuery.Definitions
{
    /// <summary>
    /// Properties for the query to be executed.
    /// </summary>
    public class QueryProperties
    {
        /// <summary>
        /// Query to be executed in string format.
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        [DefaultValue("SELECT ColumnName FROM TableName")]
        public string Query { get; set; }

        /// <summary>
        /// Parameters for the database query.
        /// </summary>
        public QueryParameter[] Parameters { get; set; }
    }
}
