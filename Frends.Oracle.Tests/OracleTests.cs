using NUnit.Framework;
using Oracle.ManagedDataAccess;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frends.Oracle;
using Frends.Oracle.Query;
using Frends.Oracle.ExecuteCommand;
using Newtonsoft.Json;
using Options = Frends.Oracle.Query.Options;
using Output = Frends.Oracle.Query.Output;

namespace Frends.Oracle.Tests
{
    /// <summary>
    /// THESE TESTS DO NOT WORK UNLESS YOU INSTALL ORACLE LOCALLY ON YOUR OWN COMPUTER!
    /// </summary>
    [TestFixture]
    [Ignore("Cannot be run unless you have a properly configured Oracle DB running on your local computer")]
    public class OracleQueryTests
    {
        // Problems with local oracle, tests not implemented yet

        ConnectionProperties _conn = new ConnectionProperties
        {
            ConnectionString =
                "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));User Id=SYSTEM;Password=<<your password>>;",
            TimeoutSeconds = 300
        };

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            using (var connection = new OracleConnection(_conn.ConnectionString))
            {
                await connection.OpenAsync();

                using (var command =
                    new OracleCommand("create table DecimalTest(DecimalValue decimal(38,35))", connection))
                {
                    await command.ExecuteNonQueryAsync();
                }

                using (var command =
                    new OracleCommand(
                        "insert into DecimalTest (DecimalValue) values (1.12345678912345678912345678912345678)",
                        connection))
                {
                    await command.ExecuteNonQueryAsync();
                }

                using (var command = new OracleCommand("create table HodorTest(name varchar2(15), value number(10,0))",
                    connection))
                {
                    await command.ExecuteNonQueryAsync();
                }

                using (var command =
                    new OracleCommand(
                        "insert all into HodorTest values('hodor', 123) into HodorTest values('jon', 321) select 1 from dual",
                        connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            using (var connection = new OracleConnection(_conn.ConnectionString))
            {
                await connection.OpenAsync();

                using (var command = new OracleCommand("drop table HodorTest", connection))
                {
                    await command.ExecuteNonQueryAsync();
                }

                using (var command = new OracleCommand("drop table DecimalTest", connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }

    // ORACLE EXECUTE

    [TestFixture]
    public class ExecuteQueryTests
    {
        string connectionString = "Data Source=localhost;User Id=SYSTEM;Password=salasana1;Persist Security Info=True;";
        //private string connectionString = ConfigHandler.ReadConfigValue("HiQ.OracleDb.connectionString");
        private Frends.Oracle.ExecuteCommand.Options _taskOptions;

        [SetUp]
        public void TestSetup()
        {
            _taskOptions = new Frends.Oracle.ExecuteCommand.Options { ThrowErrorOnFailure = true };
        }

        /// <summary>
        /// Creates a table that is used in other tests.
        /// </summary>
        [Test, Order(5)]
        public async System.Threading.Tasks.Task CreateTable()
        {
            var query = "CREATE TABLE TestTable(textField VARCHAR(255))";

            var output = new OutputProperties();
            var input = new Input
            {
                ConnectionString = connectionString,
                CommandOrProcedureName = query,

                CommandType = OracleCommandType.Command,
                TimeoutSeconds = 60
            };
            var result = await ExecuteCommand.ExecuteCommand.Execute(input, output, _taskOptions);

            Assert.AreEqual(true, result.Success);
        }

        /// <summary>
        /// Creates a stored procedure UnitTestProc that is used in other tests.
        /// </summary>
        [Test, Order(10)]
        public async System.Threading.Tasks.Task CreateProcedure()
        {
            var query = @"
                            CREATE PROCEDURE UnitTestProc (returnVal OUT VARCHAR2) AS
                            BEGIN
                            SELECT TEXTFIELD INTO returnVal FROM TESTTABLE WHERE ROWNUM = 1;
                            END;";

            var output = new OutputProperties();
            var input = new Input
            {
                ConnectionString = connectionString,
                CommandOrProcedureName = query,

                CommandType = OracleCommandType.Command,
                TimeoutSeconds = 60
            };

            var result = await ExecuteCommand.ExecuteCommand.Execute(input, output, _taskOptions);

            Assert.AreEqual(true, result.Success);
        }

        /// <summary>
        /// Insert data to the TestTable.
        /// </summary>
        [Test, Order(15)]
        public async System.Threading.Tasks.Task InsertValues()
        {
            var query = "INSERT INTO TestTable (textField) VALUES ('unit test text')";

            var output = new OutputProperties();
            var input = new Input
            {
                ConnectionString = connectionString,
                CommandOrProcedureName = query,

                CommandType = OracleCommandType.Command,
                TimeoutSeconds = 60
            };
            var result = await ExecuteCommand.ExecuteCommand.Execute(input, output, _taskOptions);

            Assert.AreEqual(true, result.Success);
        }

        /// <summary>
        /// Insert data to the TestTable using task parameters.
        /// </summary>
        [Test, Order(15)]
        public async System.Threading.Tasks.Task InsertValuesViaParametersAsync()
        {
            var query = "INSERT INTO TestTable (textField) VALUES (:param1)";

            OracleParametersForTask ownOracleParam = new OracleParametersForTask
            {
                DataType = OracleParametersForTask.ParameterDataType.Varchar2,
                Name = "param1",
                Value = "Text from parameter"
            };

            var input = new Input
            {
                ConnectionString = connectionString,
                CommandOrProcedureName = query,

                CommandType = OracleCommandType.Command,
                TimeoutSeconds = 60,
                InputParameters = new OracleParametersForTask[1]
            };
            input.InputParameters[0] = ownOracleParam;

            var output = new OutputProperties();

            var result = await ExecuteCommand.ExecuteCommand.Execute(input, output, _taskOptions);

            Assert.AreEqual(true, result.Success);
        }

        /// <summary>
        /// Execute stored procedure UnitTestProc.
        /// </summary>
        [Test, Order(20)]
        public async System.Threading.Tasks.Task ExecuteStoredProcedureWithOutputParamAsync()
        {
            var query = "UnitTestProc";

            var input = new Input
            {
                ConnectionString = connectionString,
                CommandOrProcedureName = query,

                CommandType = OracleCommandType.StoredProcedure,
                TimeoutSeconds = 60
            };

            var oracleParam = new OracleParametersForTask
            {
                DataType = OracleParametersForTask.ParameterDataType.Varchar2,
                Name = "returnVal",
                Size = 255
            };

            var output = new OutputProperties();

            output.OutputParameters = new OracleParametersForTask[1];
            output.OutputParameters[0] = oracleParam;

            var result = await ExecuteCommand.ExecuteCommand.Execute(input, output, _taskOptions);

            Assert.AreEqual(true, result.Success);
        }

        /// <summary>
        /// Get ref cursor and pass it to another task.
        /// </summary>
        [Test, Order(20)]
        public async Task GetAndUseRefCursor()
        {

            // Replicate of test of  https://docs.oracle.com/database/121/ODPNT/featRefCursor.htm#ODPNT319

            // create table and procedure:
            /*
            connect scott/tiger@oracle
            create table test (col1 number);
            insert into test(col1) values (1);
            commit;

            create or replace package testPkg as type empCur is REF Cursor;
            end testPkg;
            /

            create or replace procedure testSP(param1 IN testPkg.empCur, param2 OUT NUMBER)
            as
            begin
            FETCH param1 into param2;
            end;
            /
            */

            // Note this kind of usage of ref cursors don't work in frends. When run in frends task doesn't accept ref cursors as input parameters.

            //////////////////////////////////////////////////
            /// Get refcursor

            var oracleParam = new OracleParametersForTask
            {
                DataType = OracleParametersForTask.ParameterDataType.RefCursor,
                Name = "outRefPrm",
                Value = DBNull.Value,
                Size = 0
            };

            var output = new OutputProperties
            {
                DataReturnType = OracleCommandReturnType.Parameters
            };

            var input = new Input
            {
                ConnectionString = connectionString,
                CommandOrProcedureName = "begin open :1 for select col1 from test; end;",
                CommandType = OracleCommandType.Command,
                BindParametersByName = false,
                TimeoutSeconds = 60
            };

            output.OutputParameters = new OracleParametersForTask[1];
            output.OutputParameters[0] = oracleParam;

            var result = await ExecuteCommand.ExecuteCommand.Execute(input, output, _taskOptions);

            //////////////////////////////////////////////////
            /// Use refcursor

            var secondInput = new Input
            {
                ConnectionString = connectionString,
                CommandOrProcedureName = "testSP",
                CommandType = OracleCommandType.StoredProcedure,
                InputParameters = new OracleParametersForTask[1],
                BindParametersByName = false,
                TimeoutSeconds = 60
            };

            OracleParametersForTask secondInputParameters = new OracleParametersForTask
            {
                DataType = OracleParametersForTask.ParameterDataType.RefCursor,
                Name = "param1",
                Value = result.Result[0].Value,
                Size = 0
            };

            secondInput.InputParameters[0] = secondInputParameters;

            var secondOutputParameters = new OracleParametersForTask
            {
                DataType = OracleParametersForTask.ParameterDataType.Int32,
                Name = "param2",
                Value = DBNull.Value,
                Size = 0
            };

            var secondOutput = new OutputProperties
            {
                OutputParameters = new OracleParametersForTask[1],
                DataReturnType = OracleCommandReturnType.XmlString
            };

            secondOutput.OutputParameters[0] = secondOutputParameters;

            var secondResult = await ExecuteCommand.ExecuteCommand.Execute(secondInput, secondOutput, _taskOptions);

            Assert.AreEqual("<Root>\r\n  <param2>1</param2>\r\n</Root>", secondResult.Result);
        }

        /// <summary>
        /// Get ref cursor and pass it to another task.
        /// </summary>
        [Test, Order(20)]
        public async System.Threading.Tasks.Task GatAndUseRefCursorToJtoken()
        {

            // Replicate of test of  https://docs.oracle.com/database/121/ODPNT/featRefCursor.htm#ODPNT319

            //////////////////////////////////////////////////
            /// Get refcursor

            var oracleParam = new OracleParametersForTask
            {
                DataType = OracleParametersForTask.ParameterDataType.RefCursor,
                Name = "outRefPrm",
                Value = DBNull.Value,
                Size = 0
            };

            var output = new OutputProperties
            {
                DataReturnType = OracleCommandReturnType.Parameters
            };

            var input = new Input
            {
                ConnectionString = connectionString,
                CommandOrProcedureName = "begin open :1 for select col1 from test; end;",
                CommandType = OracleCommandType.Command,
                BindParametersByName = false,
                TimeoutSeconds = 60
            };

            output.OutputParameters = new OracleParametersForTask[1];
            output.OutputParameters[0] = oracleParam;

            var result = await ExecuteCommand.ExecuteCommand.Execute(input, output, _taskOptions);

            //////////////////////////////////////////////////
            /// Ref cursor to JToken

            var secondInput = new RefCursorToJTokenInput
            {
                Refcursor = result.Result[0]
            };

            var secondResult = ExecuteCommand.ExecuteCommand.RefCursorToJToken(secondInput);

            StringAssert.Contains(@"[{""COL1"":1.0}]", JsonConvert.SerializeObject(secondResult.Result));
        }

        /// <summary>
        /// Drop TestTable and UnitTestProc.
        /// </summary>
        [Test, Order(50)]
        public async System.Threading.Tasks.Task TestTearDownAsync()
        {
            var query = "DROP TABLE TestTable";
            var output = new OutputProperties();
            var input = new Input
            {
                ConnectionString = connectionString,
                CommandOrProcedureName = query,

                CommandType = OracleCommandType.Command,
                TimeoutSeconds = 60
            };
            var result = await ExecuteCommand.ExecuteCommand.Execute(input, output, _taskOptions);

            Assert.AreEqual(true, result.Success);

            input.CommandOrProcedureName =
                "DROP PROCEDURE UnitTestProc";

            result = await ExecuteCommand.ExecuteCommand.Execute(input, output, _taskOptions);

            Assert.AreEqual(true, result.Success);
        }


    }
}

