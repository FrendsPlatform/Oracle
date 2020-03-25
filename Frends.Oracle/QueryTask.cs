using Oracle.ManagedDataAccess.Client;
using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#pragma warning disable 1591

namespace Frends.Oracle.Query
{
    public static class QueryTask
    {
        /// <summary>
        /// Task for performing queries in Oracle databases. See documentation at https://github.com/CommunityHiQ/Frends.Community.Oracle.Query
        /// </summary>
        /// <param name="database"></param>
        /// <param name="queryProperties"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Object { bool Success, string Message, string Result }</returns>
        public static async Task<Output> Query(
            [PropertyTab] ConnectionProperties database,
            [PropertyTab] QueryProperties queryProperties,
            [PropertyTab] Options options,
            CancellationToken cancellationToken)
        {
            try
            {
                using (var connection = new OracleConnection(database.ConnectionString))
                {
                    try
                    {
                        await connection.OpenAsync(cancellationToken);

                        using (var command = new OracleCommand(queryProperties.Query, connection))
                        {
                            command.CommandTimeout = database.TimeoutSeconds;
                            //command.BindByName = true; // is this xmlCommand specific

                            // check for command parameters and set them
                            if (queryProperties.Parameters != null)
                                command.Parameters.AddRange(queryProperties.Parameters.Select(p => CreateOracleParameter(p)).ToArray());

                            // declare Result object
                            command.CommandType = CommandType.Text;

                            var queryResult = await command.ToJtokenAsync(cancellationToken);
                            return new Output { Success = true, Result = queryResult };
                        }
                    }
                    catch (Exception ex) { throw ex; }
                    finally
                    {
                        // Close connection
                        connection.Dispose();
                        connection.Close();
                        OracleConnection.ClearPool(connection);
                    }
                }
            }
            catch (Exception ex)
            {
                if (options.ThrowErrorOnFailure)
                    throw ex;
                return new Output
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        private static async Task<JToken> ToJtokenAsync(this OracleCommand command, CancellationToken cancellationToken)
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
        public static OracleParameter CreateOracleParameter(QueryParameter parameter)
        {

            return new OracleParameter()
            {
                ParameterName = parameter.Name,
                Value = parameter.Value,
                OracleDbType = parameter.DataType.ConvertEnum<OracleDbType>()
            };

        }
    }
}
