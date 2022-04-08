using NUnit.Framework;
using System;
using Frends.Oracle.ExecuteQuery.Definitions;

namespace Frends.Oracle.ExecuteQuery.Tests
{
    [TestFixture]
    class TestClass
    {
        private static string _connectionString;

        [OneTimeSetUp]
        public void setup()
        {
            _connectionString = Environment.GetEnvironmentVariable("HiQ_OracleDb_connectionString");
        }
    }
}
