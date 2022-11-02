using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.Common;
using System.ComponentModel;
using System.Globalization;
using Frends.Oracle.ExecuteQuery.Definitions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Frends.Oracle.ExecuteQuery;

/// <summary>
/// Task class
/// </summary>
public static class Oracle
{
    /// <summary>
    /// Task for performing queries in Oracle database.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Oracle.ExecuteQuery)
    /// </summary>
    /// <param name="input">Properties for the query to be executed</param>
    /// <param name="options">Task options</param>
    /// <param name="cancellationToken">CancellationToken is given by Frends UI</param>
    /// <returns>Object { bool Success, string Message, JToken.JObject[] Output }</returns>
    public static async Task<Result> ExecuteQuery([PropertyTab] Input input, [PropertyTab] Options options, CancellationToken cancellationToken)
    {
        try
        {
            using OracleConnection con = new OracleConnection(input.ConnectionString);
            await con.OpenAsync(cancellationToken);

            using var command = con.CreateCommand();
            using var transaction = con.BeginTransaction(GetIsolationLevel(options.OracleIsolationLevel));
            
            command.Transaction = transaction;
            command.CommandTimeout = options.TimeoutSeconds;
            command.CommandText = input.Query;
            command.BindByName = options.BindParameterByName;

            if (input.Parameters != null)
                command.Parameters.AddRange(input.Parameters.Select(p => CreateOracleParameter(p)).ToArray());
            try
            {
                // Execute query
                if (input.Query.ToLower().StartsWith("select"))
                {
                    var reader = await command.ExecuteReaderAsync(cancellationToken);
                    var result = reader.ToJson(cancellationToken);
                    await con.CloseAsync();
                    return new Result(true, "Success", result);
                }
                else
                {
                    var rows = await command.ExecuteNonQueryAsync(cancellationToken);
                    transaction.Commit();
                    await con.CloseAsync();
                    return new Result(true, "Success", JToken.FromObject(new { AffectedRows = rows }));
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                if (options.ThrowErrorOnFailure)
                    throw new Exception(ex.Message);

                return new Result(false, ex.Message, null);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    await con.CloseAsync();
            }
            
        } 
        catch (Exception ex)
        {
            if (options.ThrowErrorOnFailure)
                throw new Exception(ex.Message);

            return new Result(false, ex.Message, null);
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

    private static JToken ToJson(this DbDataReader reader, CancellationToken cancellationToken)
    {
        using var writer = new JTokenWriter();
        writer.Formatting = Formatting.Indented;
        writer.Culture = CultureInfo.InvariantCulture; 

        writer.WriteStartArray();

        while (reader.Read())
        {
            cancellationToken.ThrowIfCancellationRequested();
            writer.WriteStartObject();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                writer.WritePropertyName(reader.GetName(i));

                writer.WriteValue(reader.GetValue(i) ?? string.Empty);
            }
            writer.WriteEndObject();
        }

        writer.WriteEndArray();
        return writer.Token;
    }

    private static IsolationLevel GetIsolationLevel(TransactionIsolationLevel level)
    {
        return level switch
        {
            TransactionIsolationLevel.None => IsolationLevel.Unspecified,
            TransactionIsolationLevel.ReadCommitted => IsolationLevel.ReadCommitted,
            TransactionIsolationLevel.RepeatableRead => IsolationLevel.RepeatableRead,
            TransactionIsolationLevel.Serializable => IsolationLevel.Serializable,
            TransactionIsolationLevel.ReadUncommitted => IsolationLevel.ReadUncommitted,
            TransactionIsolationLevel.Default => IsolationLevel.Serializable,
            _ => IsolationLevel.Serializable,
        };
    }
}
