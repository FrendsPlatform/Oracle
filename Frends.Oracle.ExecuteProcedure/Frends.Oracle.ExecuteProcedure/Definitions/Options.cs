using System.ComponentModel;

namespace Frends.Oracle.ExecuteProcedure.Definitions;

/// <summary>
/// Source transfer options
/// </summary>
public class Options
{
    /// <summary>
    /// Choose if error should be thrown if Task failes.
    /// Otherwise returns Object { Success = false }
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; }

    /// <summary>
    /// Timeout value in seconds.
    /// </summary>
    /// <example>30</example>
    [DefaultValue(30)]
    public int TimeoutSeconds { get; set; }

    /// <summary>
    /// Choose to bind the parameter by name.
    /// If set to false parameter order is crucial.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool BindParameterByName { get; set; }
}

