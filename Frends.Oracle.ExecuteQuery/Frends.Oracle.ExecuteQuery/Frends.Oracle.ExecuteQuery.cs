using Oracle.ManagedDataAccess.Client;
using System.ComponentModel;
using Frends.Oracle.ExecuteQuery.Definitions;
using System.Data;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#pragma warning disable 1591
#pragma warning disable 1573

namespace Frends.Oracle.ExecuteQuery
{
    public class Oracle
    {
        /// <summary>
        /// Task for performing queries in Oracle database.
        /// [Documentation](https://tasks.frends.com/tasks#frends-tasks/Frends.Oracle.ExecuteQuery)
        /// </summary>
        /// <param name="connection">Properties to establish connection to Oracle databse</param>
        /// <param name="properties">Properties for the query to be executed</param>
        /// <param name="options">Task options</param>
        /// <returns>Object { bool success, string Message, string Output }</returns>
        public static async Task<Result> ExecuteQuery([PropertyTab] ConnectionProperties connection, [PropertyTab] QueryProperties properties, [PropertyTab] Options options, CancellationToken cancellationToken)
        {
            try
            {
                using (var con = new OracleConnection(connection.ConnectionString))
                {
                    try
                    {
                        await con.OpenAsync(cancellationToken);

                        using (var command = new OracleCommand(properties.Query, con))
                        {
                            command.CommandTimeout = connection.TimeoutSeconds;

                            if (properties.Parameters != null)
                                command.Parameters.AddRange(properties.Parameters.Select(p => CreateOracleParameter(p)).ToArray());

                            command.CommandType = CommandType.Text;

                            var queryResult = await ToJTokenAsync(command, cancellationToken);
                            return new Result(true, "Successful query executed.", queryResult);
                        }
                    } catch (Exception ex) { throw ex; }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        OracleConnection.ClearPool(con);
                    }
                }
            }
            catch (Exception ex)
            {
                if (options.ThrowErrorOnFailure)
                    throw ex;
                return new Result(false, ex.Message, null);
            }
        }

        private static async Task<JToken> ToJTokenAsync(OracleCommand command, CancellationToken cancellationToken)
        {
            command.CommandType = CommandType.Text;

            using (var reader = await command.ExecuteReaderAsync(cancellationToken) as OracleDataReader)
            {
                var culture = CultureInfo.InvariantCulture;

                using (var writer = new JTokenWriter() as JsonWriter)
                {
                    await writer.WriteStartArrayAsync(cancellationToken);

                    cancellationToken.ThrowIfCancellationRequested();

                    while (reader != null && reader.Read())
                    {
                        // start row object
                        await writer.WriteStartObjectAsync(cancellationToken);

                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            // add row element name
                            await writer.WritePropertyNameAsync(reader.GetName(i), cancellationToken);
                            await writer.WriteValueAsync(reader.GetValue(i) ?? string.Empty, cancellationToken);

                            cancellationToken.ThrowIfCancellationRequested();
                        }

                    }

                    // end array
                    await writer.WriteEndArrayAsync(cancellationToken);

                    return ((JTokenWriter)writer).Token;
                }
            }
        }

        /// <summary>
        /// Oracle parameters.
        /// </summary>
        private static OracleParameter CreateOracleParameter(QueryParameter parameter)
        {

            return new OracleParameter()
            {
                ParameterName = parameter.Name,
                Value = parameter.Value,
                OracleDbType = ConvertEnum<OracleDbType>(parameter.DataType)
            };

        }

        private static TEnum ConvertEnum<TEnum>(Enum source)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), source.ToString(), true);
        }
    }
}
