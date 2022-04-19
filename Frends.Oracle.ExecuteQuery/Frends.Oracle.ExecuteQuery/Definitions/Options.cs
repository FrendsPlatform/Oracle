using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Oracle.ExecuteQuery.Definitions
{
    /// <summary>
    /// Source transfer options
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Choose if error should be thrown if Task failes.
        /// Otherwise returns Object { Success = false }
        /// </summary>
        [DefaultValue(true)]
        public bool ThrowErrorOnFailure { get; set; }

        /// <summary>
        /// Choose to bind the parameter by name.
        /// If set to false parameter order is crucial.
        /// </summary>
        [DefaultValue(true)]
        public bool BindParameterByName { get; set; }
    }
}
