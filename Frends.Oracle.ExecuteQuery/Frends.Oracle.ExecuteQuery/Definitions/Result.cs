
#pragma warning disable 1591

namespace Frends.Oracle.ExecuteQuery.Definitions
{
    /// <summary>
    /// Return object with private setters.
    /// </summary>
    public class Result
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public dynamic Output { get; private set; }

        public Result(bool success, string message, dynamic output)
        {
            Success = success;
            Message = message;
            Output = output;
        }
    }
}
