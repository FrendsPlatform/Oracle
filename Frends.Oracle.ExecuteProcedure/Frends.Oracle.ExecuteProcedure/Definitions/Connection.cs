using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Oracle.ExecuteProcedure.Definitions;

/// <summary>
/// Properties to establish connection to the Oracle database.
/// </summary>
public class Connection
{
    /// <summary>
    /// Oracle connection string.
    /// </summary>
    /// <example>"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=MyPort))(CONNECT_DATA=(SERVICE_NAME=MyOracleSID)));User Id=myUsername;Password=myPassword;"</example>
    [PasswordPropertyText]
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=MyPort))(CONNECT_DATA=(SERVICE_NAME=MyOracleSID)));User Id=myUsername;Password=myPassword;")]
    public string ConnectionString { get; set; }

    /// <summary>
    /// Timeout value in seconds.
    /// </summary>
    /// <example>30</example>
    [DefaultValue(30)]
    public int TimeoutSeconds { get; set; }

}

