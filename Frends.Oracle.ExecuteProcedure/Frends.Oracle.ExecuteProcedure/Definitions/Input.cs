using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Frends.Oracle.ExecuteProcedure.Definitions;

/// <summary>
/// Properties for the query to be executed.
/// </summary>
public class Input
{
    /// <summary>
    /// Query to be executed in string format.
    /// </summary>
    /// <example>"SpGetResultsByAge"</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string Command { get; set; }

    /// <summary>
    /// Type of the command: Command or Stored Procedure
    /// </summary>
    /// <example>Type.StoredProcedure</example>
    [DefaultValue(OracleCommandType.StoredProcedure)]
    public OracleCommandType CommandType { get; set; } 

    /// <summary>
    /// Parameters for the database query.
    /// </summary>
    /// <example>{ Name = "ParamName", Value = "1", DataType = QueryParameterType.NVarchar2 }</example>
    public InputParameter[] Parameters { get; set; }
}