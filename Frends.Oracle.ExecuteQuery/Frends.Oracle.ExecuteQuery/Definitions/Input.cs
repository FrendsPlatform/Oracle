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
        /// <example>"INSERT INTO MyTable (id, first_name, last_name) VALUES (:id, :first_name, :last_name)"</example>
        [DisplayFormat(DataFormatString = "Sql")]
        [DefaultValue("INSERT INTO MyTable (id, first_name, last_name) VALUES (:id, :first_name, :last_name)")]
        public string Query { get; set; }

        /// <summary>
        /// Parameters for the database query.
        /// </summary>
        /// <example>[
        /// { Name = "id", Value = 1, DataType = QueryParameterType.Int32 },
        /// { Name = "first_name", Value = "John", DataType = QueryParameterType.Varchar2 },
        /// { Name = "last_name", Value = "Doe", DataType = QueryParameterType.Varchar2 }
        /// ]</example>
        public QueryParameter[] Parameters { get; set; }

        /// <summary>
        /// Oracle connection string.
        /// </summary>
        /// <example>Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=MyPort))(CONNECT_DATA=(SERVICE_NAME=MyOracleSID)));User Id=myUsername;Password=myPassword;</example>
        [DisplayFormat(DataFormatString = "Text")]
        [PasswordPropertyText]
        [DefaultValue("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=MyPort))(CONNECT_DATA=(SERVICE_NAME=MyOracleSID)));User Id=myUsername;Password=myPassword;")]
        public string ConnectionString { get; set; }
    }
}
