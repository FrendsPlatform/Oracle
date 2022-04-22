namespace Frends.Oracle.ExecuteQuery.Definitions
{
    /// <summary>
    /// Return object with private setters.
    /// </summary>
    public class Result
    { 
        /// <summary>
        /// Boolean value of success of the query 
        /// </summary>
        /// <example>true</example>
        public bool Success { get; set; }

        internal Result()
        {}
}
}
