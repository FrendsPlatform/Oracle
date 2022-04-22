namespace Frends.Oracle.ExecuteQuery.Definitions
{
    /// <summary>
    /// Return object of ErrorResult with private setters.
    /// </summary>
    public class ErrorResult : Result
    {
        /// <summary>
        /// String value of the Error message that comes from Oracle.
        /// </summary>
        /// <example>"ORA-01722: invalid number"</example>
        public string Error { get; private set; }

        internal ErrorResult(bool success, string message)
        {
            Success = success;
            Error = message;
        }
    }
}
