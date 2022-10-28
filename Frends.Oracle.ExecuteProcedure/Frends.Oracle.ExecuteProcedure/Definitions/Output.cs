using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Oracle.ExecuteProcedure.Definitions;

/// <summary>
/// Properties for the query to be executed.
/// </summary>
public class Output
{
    /// <summary>
    /// Data type for the Result of the procedure.
    /// </summary>
    /// <example>"SpGetResultsByAge"</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue(OracleCommandReturnType.XDocument)]
    public OracleCommandReturnType DataReturnType { get; set; }

    /// <summary>
    /// Parameters for the output of the procedure or command.
    /// </summary>
    /// <example>{ Name = "name", DataType = QueryParameterType.Varchar2 }</example>
    public OutputParameter[] OutputParameters { get; set; }
}