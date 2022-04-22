using Oracle.ManagedDataAccess.Client;
using System.ComponentModel;
using Frends.Oracle.ExecuteQuery.Definitions;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Frends.Oracle.ExecuteQuery.Tests")]
namespace Frends.Oracle.ExecuteQuery
{
    /// <summary>
    /// Task class
    /// </summary>
    public class Oracle
    {
        /// <summary>
        /// Task for performing queries in Oracle database.
        /// [Documentation](https://tasks.frends.com/tasks#frends-tasks/Frends.Oracle.ExecuteQuery)
        /// </summary>
        /// <param name="connection">Properties to establish connection to Oracle databse</param>
        /// <param name="properties">Properties for the query to be executed</param>
        /// <param name="options">Task options</param>
        /// <param name="cancellationToken">CancellationToken is given by Frends UI</param>
        /// <returns>Object { bool Success, int RowsAffected, List Output }</returns>
        public static Result ExecuteQuery([PropertyTab] ConnectionProperties connection, [PropertyTab] QueryProperties properties, [PropertyTab] Options options, CancellationToken cancellationToken)
        {
            var rows = new List<JObject>();
            int rowsAffected = 0;

            try
            {
                using (OracleConnection con = new OracleConnection(connection.ConnectionString))
                {
                    con.Open();

                    var command = con.CreateCommand();

                    command.CommandTimeout = connection.TimeoutSeconds;

                    command.CommandText = properties.Query;
                    command.BindByName = options.BindParameterByName;

                    if (properties.Parameters != null)
                        command.Parameters.AddRange(properties.Parameters.Select(p => CreateOracleParameter(p)).ToArray());

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var jsonObject = new JObject();
                            for (var i = 0; i < reader.FieldCount; i++)
                            {
                                cancellationToken.ThrowIfCancellationRequested();
                                jsonObject.Add(reader.GetName(i), reader.GetValue(i).ToString());

                            }
                            rows.Add(jsonObject);
                        }

                        // select query returns -1 that is converted into 0
                        rowsAffected = (reader.RecordsAffected == -1) ? 0 : reader.RecordsAffected;
                        reader.Close();
                        reader.Dispose();
                    }

                    command.Dispose();

                    con.Close();
                    con.Dispose();
                }
                return new QueryResult(true, rowsAffected, rows);
            } 
            catch (Exception ex)
            {
                if (options.ThrowErrorOnFailure)
                    throw new Exception(ex.Message);

                return new ErrorResult(false, ex.Message);
            }
        }

        private static OracleParameter CreateOracleParameter(QueryParameter parameter)
        {
            return new OracleParameter()
            {
                ParameterName = parameter.Name, 
                Value = parameter.Value,
                OracleDbType = (OracleDbType)Enum.Parse(typeof(OracleDbType), parameter.DataType.ToString())
            };
        }
    }
}