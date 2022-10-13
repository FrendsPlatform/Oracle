using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Frends.Oracle.ExecuteProcedure.Tests;

internal static class Helpers
{
    private static string _connectionStringSys = "Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 51521))(CONNECT_DATA = (SERVICE_NAME = XEPDB1))); User Id = sys; Password=mysecurepassword; DBA PRIVILEGE=SYSDBA";

    internal static void TestConnectionBeforeRunningTests()
    {
        using var con = new OracleConnection(_connectionStringSys);
        while (con.State != ConnectionState.Open)
            con.Open();
        con.Close();

    }
    internal static void CreateTestTable(string connectionString)
    {

        using var con = new OracleConnection(connectionString);
        con.Open();

        using var cmd = con.CreateCommand();
        cmd.CommandType = CommandType.Text;

        cmd.CommandText = @"CREATE TABLE test_user.workers(id NUMBER, name VARCHAR2(100) NULL, address VARCHAR2(100) NULL, PRIMARY KEY(id))";
        cmd.ExecuteNonQuery();

        con.Close();
    }

    internal static void InsertTestData(string connectionString)
    {
        using var con = new OracleConnection(connectionString);
        con.Open();

        using var cmd = con.CreateCommand();
        cmd.CommandType = CommandType.Text;

        cmd.CommandText = @"INSERT INTO test_user.workers(id, name, address) VALUES (1, 'risto', 'haapatie 9')";
        cmd.ExecuteNonQuery();

        con.Close();
    }

    internal static void DropTestTable(string connectionString)
    {
        using var con = new OracleConnection(connectionString);
        con.Open();

        using var cmd = con.CreateCommand();
        cmd.CommandText = "drop table test_user.workers";

        cmd.CommandType = CommandType.Text;

        cmd.ExecuteNonQuery();
        con.Close();
    }

    internal static void DropProcedure(string connectionString, string name)
    {
        using var con = new OracleConnection(connectionString);
        con.Open();

        using var cmd = con.CreateCommand();
        cmd.CommandText = $"DROP PROCEDURE {name}";

        cmd.CommandType = CommandType.Text;

        cmd.ExecuteNonQuery();
        con.Close();
    }

    internal static void CreateTestUser()
    {
        using var con = new OracleConnection(_connectionStringSys);
        con.Open();

        using var cmd = con.CreateCommand();
        cmd.CommandText = @"
create or replace procedure p_create_user (par_username in varchar2) is
begin
  execute immediate ('create user '    || par_username ||
                     ' identified by ' || par_username ||
                     ' temporary tablespace temp '     ||
                     ' profile default ');

  execute immediate ('grant create session to ' || par_username);
  execute immediate ('grant create table to ' || par_username);
  execute immediate ('grant create procedure to ' || par_username);
  execute immediate ('grant unlimited tablespace to ' || par_username);
end;";

        cmd.CommandType = CommandType.Text;

        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
begin
  p_create_user('test_user');
end;";
        cmd.ExecuteNonQuery();
        con.Close();
    }

    internal static void DropTestUser()
    {
        using var con = new OracleConnection(_connectionStringSys);
        con.Open();

        using var cmd = con.CreateCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "DROP USER test_user;";
        cmd.ExecuteNonQuery();
        con.Close();
    }
}

