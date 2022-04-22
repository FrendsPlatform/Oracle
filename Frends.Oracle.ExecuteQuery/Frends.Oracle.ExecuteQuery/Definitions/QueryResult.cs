using Newtonsoft.Json.Linq;

namespace Frends.Oracle.ExecuteQuery.Definitions
{
    /// <summary>
    /// Return object of Query (select) with private setters.
    /// </summary>
    public class QueryResult : Result
    {
        /// <summary>
        /// Rows affected by the Query.
        /// </summary>
        /// <example>1</example>
        public int RowsAffected { get; private set; }

        /// <summary>
        /// List of JObjects showing the output of the query.
        /// </summary>
        /// <example>[{"ID": "1","FIRST_NAME": "Saija","LAST_NAME": "Saijalainen","START_DATE": ""}]</example>
        public List<JObject> Output { get; private set; }

        internal QueryResult(bool success, int rowsAffected, List<JObject> output)
        {
            Success = success;
            RowsAffected = rowsAffected;
            Output = output;
        }
    }
}
