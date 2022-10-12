using NUnit.Framework;
using Frends.Oracle.ExecuteProcedure.Definitions;

namespace Frends.Oracle.ExecuteProcedure.Tests;

public class ExecuteProcedureTestBase
{
    protected static Connection _con;
    protected static Options _options;

    protected static string schema = "test_user";
    protected static string _connectionString = $"Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 51521))(CONNECT_DATA = (SERVICE_NAME = XEPDB1))); User Id = {schema}; Password={schema};";

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

        Helpers.CreateTestUser();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Helpers.DropTestUser();
    }
}

