using Newtonsoft.Json.Linq;

#pragma warning disable 1591

namespace Frends.Oracle.ExecuteQuery.Definitions
{
    /// <summary>
    /// Return object of Query (select) with private setters.
    /// </summary>
    public class QueryResult : Result
    {
        public int RowsAffected { get; private set; }
        public List<JObject> Output { get; private set; }

        public QueryResult(bool success, int rowsAffected, List<JObject> output)
        {
            Success = success;
            RowsAffected = rowsAffected;
            Output = output;
        }
    }
}
