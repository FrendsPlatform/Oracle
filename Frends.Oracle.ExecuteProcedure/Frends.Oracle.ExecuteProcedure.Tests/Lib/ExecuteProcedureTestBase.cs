using NUnit.Framework;
using Frends.Oracle.ExecuteProcedure.Definitions;
using Oracle.ManagedDataAccess.Client;

namespace Frends.Oracle.ExecuteProcedure.Tests;

public class ExecuteProcedureTestBase
{
    protected static Connection _con;
    protected static Options _options;

    protected static string schema = "test_user";
    protected static string _connectionString = $"Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 51521))(CONNECT_DATA = (SERVICE_NAME = XEPDB1))); User Id = {schema}; Password={schema};";
    protected static string _connectionStringSys = "Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 51521))(CONNECT_DATA = (SERVICE_NAME = XEPDB1))); User Id = sys; Password=mysecurepassword; DBA PRIVILEGE=SYSDBA";

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _con = new Connection
        {
            ConnectionString = _connectionString,
            TimeoutSeconds = 30
        };

        _options = new Options
        {
            ThrowErrorOnFailure = true,
            BindParameterByName = true
        };

        Helpers.TestConnectionBeforeRunningTests();

        using var con = new OracleConnection(_connectionStringSys);
        con.Open();
        Helpers.CreateTestUser(con);
        con.Close();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        using var con = new OracleConnection(_connectionStringSys);
        con.Open();
        Helpers.DropTestUser(con);
        con.Close();
    }
}

