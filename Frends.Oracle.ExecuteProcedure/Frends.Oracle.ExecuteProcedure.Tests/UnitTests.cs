using NUnit.Framework;
using System.Threading;
using Frends.Oracle.ExecuteProcedure.Definitions;
using System.Threading.Tasks;

namespace Frends.Oracle.ExecuteProcedure.Tests;

[TestFixture]
class UnitTests : ExecuteProcedureTestBase
{
    private readonly string _proc = "unitestproc";
    [SetUp]
    public void Setup()
    {
        Helpers.CreateTestTable(_connectionString);
        Helpers.InsertTestData(_connectionString);
    }

    [TearDown]
    public void TearDown()
    {
        Helpers.DropTestTable(_connectionString);
        Helpers.DropProcedure(_connectionString, _proc);
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
        Assert.AreEqual(@"{""address"":""haapatie 9""}", string.Join(",", result.Output));
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
        Assert.AreEqual("<Root>\r\n  <address>haapatie 9</address>\r\n</Root>", string.Join(",", result.Output));
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
        Assert.AreEqual("<Root>\r\n  <address>haapatie 9</address>\r\n</Root>", string.Join(",", result.Output));
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
        Assert.AreEqual("address", string.Join(",", result.Output));
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
        Assert.AreEqual(-1, result.Output);
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
        Assert.AreEqual(@"{""a"":""haapatie 9""}", string.Join(",", result.Output));
    }
}
