using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Oracle.ExecuteQuery.Definitions
{
    /// <summary>
    /// Properties for the query to be executed.
    /// </summary>
    public class Input
    {
        /// <summary>
        /// Query to be executed in string format.
        /// </summary>
        /// <example>"SELECT ColumnName FROM TableName"</example>
        [DisplayFormat(DataFormatString = "Text")]
        [DefaultValue("SELECT ColumnName FROM TableName")]
        public string Query { get; set; }

        /// <summary>
        /// Parameters for the database query.
        /// </summary>
        /// <example>{ Name = "name", Value = "Matti", DataType = QueryParameterType.Varchar2 }</example>
        public QueryParameter[] Parameters { get; set; }

        /// <summary>
        /// Oracle connection string.
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        [PasswordPropertyText]
        [DefaultValue("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=MyPort))(CONNECT_DATA=(SERVICE_NAME=MyOracleSID)));User Id=myUsername;Password=myPassword;")]
        public string ConnectionString { get; set; }
    }
}
