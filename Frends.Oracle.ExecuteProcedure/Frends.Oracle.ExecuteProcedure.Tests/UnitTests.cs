using NUnit.Framework;
using System.Threading;
using Frends.Oracle.ExecuteProcedure.Definitions;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace Frends.Oracle.ExecuteProcedure.Tests;

[TestFixture]
class UnitTests
{
    private static Connection _con;
    private static Options _options;

    private static string schema = "test_user";
    private static string _connectionString = $"Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 51521))(CONNECT_DATA = (SERVICE_NAME = XEPDB1))); User Id = {schema}; Password={schema};";
    private static string _connectionStringSys = "Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 51521))(CONNECT_DATA = (SERVICE_NAME = XEPDB1))); User Id = sys; Password=mysecurepassword; DBA PRIVILEGE=SYSDBA";
    private readonly string _proc = "unitestproc";

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

    [SetUp]
    public void Setup()
    {
        using var con = new OracleConnection(_connectionStringSys);
        con.Open();
        try
        {
            Helpers.CreateTestTable(con);
            Helpers.InsertTestData(con);
        }
        finally { con.Close(); }
    }

    [TearDown]
    public void TearDown()
    {
        using var con = new OracleConnection(_connectionStringSys);
        con.Open();
        try
        {
            Helpers.DropTestTable(con);
            Helpers.DropProcedure(con, _proc);
        }
        finally { con.Close(); }
    }

    [Test]
    public async Task ExecuteProcedure_ProcedureJSONString()
    {
        var input = new Input
        {
            Command = @$"
create or replace procedure {_proc} (name in varchar2, address out varchar2) as
begin
  select address into address from workers where name = name;
end {_proc};",
            CommandType = OracleCommandType.Command
        };

        var output = new Output
        {
            DataReturnType = OracleCommandReturnType.JSONString
        };

        var result = await Oracle.ExecuteProcedure(_con, input, output, _options, new CancellationToken());
        Assert.IsTrue(result.Success);

        input.Command = _proc;
        input.CommandType = OracleCommandType.StoredProcedure;
        input.Parameters = new InputParameter[]
        {
            new InputParameter
            {
                Name = "name",
                Value = "risto",
                DataType = ProcedureParameterType.Varchar2,
                Size = 255
            }
        };

        output.OutputParameters = new OutputParameter[]
        {
            new OutputParameter
            {
                Name = "address",
                DataType = ProcedureParameterType.Varchar2,
                Size = 255
            }
        };

        result = await Oracle.ExecuteProcedure(_con, input, output, _options, new CancellationToken());
        Assert.IsTrue(result.Success);
    }

    [Test]
    public async Task ExecuteProcedure_ProcedureXmlString()
    {
        var input = new Input
        {
            Command = @$"
create or replace procedure {_proc} (name in varchar2, address out varchar2) as
begin
  select address into address from workers where name = name;
end {_proc};",
            CommandType = OracleCommandType.Command
        };

        var output = new Output
        {
            DataReturnType = OracleCommandReturnType.XmlString
        };

        var result = await Oracle.ExecuteProcedure(_con, input, output, _options, new CancellationToken());
        Assert.IsTrue(result.Success);

        input.Command = _proc;
        input.CommandType = OracleCommandType.StoredProcedure;
        input.Parameters = new InputParameter[]
        {
            new InputParameter
            {
                Name = "name",
                Value = "risto",
                DataType = ProcedureParameterType.Varchar2,
                Size = 255
            }
        };

        output.OutputParameters = new OutputParameter[]
        {
            new OutputParameter
            {
                Name = "address",
                DataType = ProcedureParameterType.Varchar2,
                Size = 255
            }
        };

        result = await Oracle.ExecuteProcedure(_con, input, output, _options, new CancellationToken());
        Assert.IsTrue(result.Success);
    }

    [Test]
    public async Task ExecuteProcedure_ProcedureXDocument()
    {
        var input = new Input
        {
            Command = @$"
create or replace procedure {_proc} (name in varchar2, address out varchar2) as
begin
  select address into address from workers where name = name;
end {_proc};",
            CommandType = OracleCommandType.Command
        };

        var output = new Output
        {
            DataReturnType = OracleCommandReturnType.XDocument
        };

        var result = await Oracle.ExecuteProcedure(_con, input, output, _options, new CancellationToken());
        Assert.IsTrue(result.Success);

        input.Command = _proc;
        input.CommandType = OracleCommandType.StoredProcedure;
        input.Parameters = new InputParameter[]
        {
            new InputParameter
            {
                Name = "name",
                Value = "risto",
                DataType = ProcedureParameterType.Varchar2,
                Size = 255
            }
        };

        output.OutputParameters = new OutputParameter[]
        {
            new OutputParameter
            {
                Name = "address",
                DataType = ProcedureParameterType.Varchar2,
                Size = 255
            }
        };

        result = await Oracle.ExecuteProcedure(_con, input, output, _options, new CancellationToken());
        Assert.IsTrue(result.Success);
    }

    [Test]
    public async Task ExecuteProcedure_ProcedureParameters()
    {
        var input = new Input
        {
            Command = @$"
create or replace procedure {_proc} (name in varchar2, address out varchar2) as
begin
  select address into address from workers where name = name;
end {_proc};",
            CommandType = OracleCommandType.Command
        };

        var output = new Output
        {
            DataReturnType = OracleCommandReturnType.Parameters
        };

        var result = await Oracle.ExecuteProcedure(_con, input, output, _options, new CancellationToken());
        Assert.IsTrue(result.Success);

        input.Command = _proc;
        input.CommandType = OracleCommandType.StoredProcedure;
        input.Parameters = new InputParameter[]
        {
            new InputParameter
            {
                Name = "name",
                Value = "risto",
                DataType = ProcedureParameterType.Varchar2,
                Size = 255
            }
        };

        output.OutputParameters = new OutputParameter[]
        {
            new OutputParameter
            {
                Name = "address",
                DataType = ProcedureParameterType.Varchar2,
                Size = 255
            }
        };

        result = await Oracle.ExecuteProcedure(_con, input, output, _options, new CancellationToken());
        Assert.IsTrue(result.Success);
    }

    [Test]
    public async Task ExecuteProcedure_ProcedureAffectedRows()
    {
        var input = new Input
        {
            Command = @$"
create or replace procedure {_proc} (name in varchar2, address out varchar2) as
begin
  select address into address from workers where name = name;
end {_proc};",
            CommandType = OracleCommandType.Command
        };

        var output = new Output
        {
            DataReturnType = OracleCommandReturnType.AffectedRows
        };

        var result = await Oracle.ExecuteProcedure(_con, input, output, _options, new CancellationToken());
        Assert.IsTrue(result.Success);

        input.Command = _proc;
        input.CommandType = OracleCommandType.StoredProcedure;
        input.Parameters = new InputParameter[]
        {
            new InputParameter
            {
                Name = "name",
                Value = "risto",
                DataType = ProcedureParameterType.Varchar2,
                Size = 255
            }
        };

        output.OutputParameters = new OutputParameter[]
        {
            new OutputParameter
            {
                Name = "address",
                DataType = ProcedureParameterType.Varchar2,
                Size = 255
            }
        };

        result = await Oracle.ExecuteProcedure(_con, input, output, _options, new CancellationToken());
        Assert.IsTrue(result.Success);
    }

    [Test]
    public async Task ExecuteProcedure_ProcedureWithoutBindByName()
    {
        var options = new Options
        {
            BindParameterByName = false,
            ThrowErrorOnFailure = true
        };

        var input = new Input
        {
            Command = @$"
create or replace procedure {_proc} (name in varchar2, address out varchar2) as
begin
  select address into address from workers where name = name;
end {_proc};",
            CommandType = OracleCommandType.Command
        };

        var output = new Output
        {
            DataReturnType = OracleCommandReturnType.JSONString
        };

        var result = await Oracle.ExecuteProcedure(_con, input, output, options, new CancellationToken());
        Assert.IsTrue(result.Success);

        input.Command = _proc;
        input.CommandType = OracleCommandType.StoredProcedure;
        input.Parameters = new InputParameter[]
        {
            new InputParameter
            {
                Name = "n",
                Value = "risto",
                DataType = ProcedureParameterType.Varchar2,
                Size = 255
            }
        };

        output.OutputParameters = new OutputParameter[]
        {
            new OutputParameter
            {
                Name = "a",
                DataType = ProcedureParameterType.Varchar2,
                Size = 255
            }
        };

        result = await Oracle.ExecuteProcedure(_con, input, output, options, new CancellationToken());
        Assert.IsTrue(result.Success);
    }
}
