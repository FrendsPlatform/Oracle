using Newtonsoft.Json.Linq;

#pragma warning disable 1591

namespace Frends.Oracle.ExecuteQuery.Definitions
{
    /// <summary>
    /// Return object of ErrorResult with private setters.
    /// </summary>
    public class ErrorResult : Result
    {
        public string Error { get; private set; }

        public ErrorResult(bool success, string message)
        {
            Success = success;
            Error = message;
        }
    }
}
