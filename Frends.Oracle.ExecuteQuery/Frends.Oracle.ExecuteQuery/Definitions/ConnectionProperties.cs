using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Oracle.ExecuteQuery.Definitions
{
    /// <summary>
    /// Properties to establish connection to the Oracle database.
    /// </summary>
    public class ConnectionProperties
    {
        /// <summary>
        /// Oracle connection string.
        /// </summary>
        [PasswordPropertyText]
        [DefaultValue("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=MyPort))(CONNECT_DATA=(SERVICE_NAME=MyOracleSID)));User Id=myUsername;Password=myPassword;")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Timeout value in seconds.
        /// </summary>
        [DefaultValue(30)]
        public int TimeoutSeconds { get; set; }

    }
}
