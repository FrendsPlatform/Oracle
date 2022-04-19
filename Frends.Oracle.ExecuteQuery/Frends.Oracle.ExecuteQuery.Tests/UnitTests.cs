using NUnit.Framework;
using System;
using System.Threading;
using System.Collections.Generic;
using Frends.Oracle.ExecuteQuery.Definitions;
using Newtonsoft.Json.Linq;

namespace Frends.Oracle.ExecuteQuery.Tests
{
    [TestFixture]
    class TestClass
    {
        /// <summary>
        /// Connection string for Oracle database.
        /// </summary>
        private static string _connectionString = Environment.GetEnvironmentVariable("HiQ_OracleDb_connectionString");
        private static int _connectionTimeout = 300;

        /// <summary>
        /// Global variables.
        /// </summary>
        private static ConnectionProperties _connectionProperties;
        private static QueryProperties _queryProperties;
        private static Options _options;

        [SetUp]
        public void SetUp()
        {
            _connectionProperties = new ConnectionProperties { ConnectionString = _connectionString, TimeoutSeconds = _connectionTimeout };
            _queryProperties = new QueryProperties
            {
                Query = "create table workers(" +
                "id NUMBER, " +
                "first_name varchar2(50), " +
                "last_name varchar2(50), " +
                "start_date date, " +
                "primary key(id))"
            };
            _options = new Options { ThrowErrorOnFailure = true, BindParameterByName = true };

            Oracle.ExecuteQuery(_connectionProperties, _queryProperties, _options, new CancellationToken());
        }

        [TearDown]
        public void TearDown()
        {
            _connectionProperties = new ConnectionProperties { ConnectionString = _connectionString, TimeoutSeconds = _connectionTimeout };
            _queryProperties = new QueryProperties
            {
                Query = "drop table workers"
            };
            _options = new Options { ThrowErrorOnFailure = true };

            Oracle.ExecuteQuery(_connectionProperties, _queryProperties, _options, new CancellationToken());        
        }

        [Test]
        public void ExecuteQuery_InsertWithParameters()
        {
            _queryProperties = new QueryProperties
            {
                Query = "insert " +
                "into workers (id, first_name, last_name) values (:id, :name, 'Meikäläinen')",
                Parameters = new QueryParameter[]
                {
                    new QueryParameter { Name = "name", Value = "Matti", DataType = QueryParameterType.Varchar2 },
                    new QueryParameter { Name = "id", Value = 3, DataType = QueryParameterType.Int32 },
                }
            };

            var result = (QueryResult)Oracle.ExecuteQuery(_connectionProperties, _queryProperties, _options, new CancellationToken());
            Assert.That(result, Is.Not.Null);
            Assert.AreEqual(1, result.RowsAffected);

            _queryProperties = new QueryProperties
            {
                Query = "select first_name from workers where id = 3"
            };

            var expected = "Matti";
            result = (QueryResult)Oracle.ExecuteQuery(_connectionProperties, _queryProperties, _options, new CancellationToken());
            result.Output.ForEach(i => Console.WriteLine("{0}\t", i));
            Assert.AreEqual(result.Output.GetType(), typeof(List<JObject>));
            Assert.AreEqual(expected, result.Output[0]["FIRST_NAME"].ToString());
        }

        [Test]
        public void ExecuteQuery_WithAllValues()
        {
            _queryProperties = new QueryProperties
            {
                Query = "insert " +
                "into workers values (1, 'Matti', 'Meikäläinen', DATE '2022-04-12')"
            };
            var result = (QueryResult)Oracle.ExecuteQuery(_connectionProperties, _queryProperties, _options, new CancellationToken());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(1, result.RowsAffected);
        }

        [Test]
        public void ExecuteQuery_InsertMultipleRowsIntoTable()
        {
            _queryProperties = new QueryProperties
            {
                Query = "insert all " +
                "into workers (id, first_name, last_name) values (1, 'Matti', 'Meikäläinen') " +
                "into workers (id, first_name, last_name) values (2, 'Teppo', 'Teikäläinen') " +
                "select * from dual",
            };

            var result = (QueryResult)Oracle.ExecuteQuery(_connectionProperties, _queryProperties, _options, new CancellationToken());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(2, result.RowsAffected);

            _queryProperties = new QueryProperties
            {
                Query = "select * from workers " +
                "where id = :id",
                Parameters = new QueryParameter[]
                {
                    new QueryParameter { Name = "id", Value = 1, DataType = QueryParameterType.Int32 }
                }
            };

            result = (QueryResult)Oracle.ExecuteQuery(_connectionProperties, _queryProperties, _options, new CancellationToken());
            Assert.That(result, Is.Not.Null);
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(0, result.RowsAffected);
        }

        [Test]
        public void ExecuteQuery_InsertMultipleRowsWithMultipleParameters()
        {
            _queryProperties = new QueryProperties
            {
                Query = "insert all " +
                "into workers (id, first_name, last_name) values (:id1, :fname1, :lname1) " +
                "into workers (id, first_name, last_name) values (:id2, :fname2, :lname2) " +
                "select * from dual",
                Parameters = new QueryParameter[]
                {
                    new QueryParameter { Name = "id1", Value = 1, DataType = QueryParameterType.Int32 },
                    new QueryParameter { Name = "fname1", Value = "Matti", DataType = QueryParameterType.Varchar2 },
                    new QueryParameter { Name = "lname1", Value ="Meikäläinen", DataType = QueryParameterType.Varchar2 },
                    new QueryParameter { Name = "id2", Value = 2, DataType = QueryParameterType.Int32 },
                    new QueryParameter { Name = "fname2", Value = "Teppo", DataType = QueryParameterType.Varchar2 },
                    new QueryParameter { Name = "lname2", Value = "Teikäläinen", DataType = QueryParameterType.Varchar2 }
                }
            };

            var result = (QueryResult)Oracle.ExecuteQuery(_connectionProperties, _queryProperties, _options, new CancellationToken());
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(2, result.RowsAffected);
        }

        [Test]
        public void ExecuteQuery_Update()
        {
            _queryProperties = new QueryProperties
            {
                Query = "insert " +
                "into workers (id, first_name, last_name) values (1, 'Matti', 'Meikäläinen')"
            };

            var result = (QueryResult)Oracle.ExecuteQuery(_connectionProperties, _queryProperties, _options, new CancellationToken());
            Assert.IsNotNull(result);

            _queryProperties = new QueryProperties
            {
                Query = "update workers " +
                "set first_name = 'Saija', " +
                "last_name = 'Saijalainen' " +
                "where id = 1"
            };

            result = (QueryResult)Oracle.ExecuteQuery(_connectionProperties, _queryProperties, _options, new CancellationToken());
            Assert.AreEqual(1, result.RowsAffected);

            _queryProperties = new QueryProperties
            {
                Query = "select first_name from workers where id = 1"
            };

            var expected = "Saija";
            result = (QueryResult)Oracle.ExecuteQuery(_connectionProperties, _queryProperties, _options, new CancellationToken());
            Assert.AreEqual(expected, result.Output[0]["FIRST_NAME"].ToString());
        }

        [Test]
        public void ExecuteQuery_SelectWithNonExistingRow()
        {
            _queryProperties = new QueryProperties
            {
                Query = "insert " +
                "into workers (id, first_name, last_name) values (1, 'Matti', 'Meikäläinen')"
            };

            Oracle.ExecuteQuery(_connectionProperties, _queryProperties, _options, new CancellationToken());

            _queryProperties = new QueryProperties
            {
                Query = "select first_name from workers where id = 2"
            };

            var result = (QueryResult)Oracle.ExecuteQuery(_connectionProperties, _queryProperties, _options, new CancellationToken());
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Output, Is.Not.Null);
            Assert.That(result.Success, Is.True);
            Assert.AreEqual(0, result.Output.Count);
            Assert.AreEqual(0, result.RowsAffected);
        }

        [Test]
        public void ExecuteQuery_WithoutThrowErrorOnFailure()
        {

            _options = new Options { ThrowErrorOnFailure = false };

            _queryProperties = new QueryProperties
            {
                Query = "insert " +
                "into workers (id, first_name, last_name) values ('Matti', 1, 'Meikäläinen')",
            };

            var result = (ErrorResult)Oracle.ExecuteQuery(_connectionProperties, _queryProperties, _options, new CancellationToken());
            Assert.That(result, Is.Not.Null);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual("ORA-01722: invalid number", result.Error);
        }

        [Test]
        public void ExecuteQuery_ThatThrowsException()
        {
            _queryProperties = new QueryProperties
            {
                Query = "insert " +
                "into workers (id, first_name, last_name) values ('Matti', 1, 'Meikäläinen')",
            };

            var error = Assert.Throws<Exception>(() => Oracle.ExecuteQuery(_connectionProperties, _queryProperties, _options, new CancellationToken()));
            Assert.AreEqual("ORA-01722: invalid number", error.Message);
        }

        [Test]
        public void ExecuteQuery_InsertWithBindParameterByNameFalse()
        {
            _options.BindParameterByName = false;
            _queryProperties = new QueryProperties
            {
                Query = "insert " +
                "into workers (id, first_name, last_name) values (:p, :p, :p)",
                Parameters = new QueryParameter[]
                {
                    new QueryParameter { Name = "p", Value = 1, DataType = QueryParameterType.Int32 },
                    new QueryParameter { Name = "p", Value = "Matti", DataType = QueryParameterType.Varchar2 },
                    new QueryParameter { Name = "p", Value ="Meikäläinen", DataType = QueryParameterType.Varchar2 },
                }
            };

            var result = (QueryResult)Oracle.ExecuteQuery(_connectionProperties, _queryProperties, _options, new CancellationToken());
            Assert.That(result, Is.Not.Null);
            Assert.AreEqual(true, result.Success);

            _queryProperties = new QueryProperties
            {
                Query = "select first_name, last_name from workers where id = 1"
            };

            result = (QueryResult)Oracle.ExecuteQuery(_connectionProperties, _queryProperties, _options, new CancellationToken());
            Assert.AreEqual("Matti", result.Output[0]["FIRST_NAME"].ToString());
            Assert.AreEqual("Meikäläinen", result.Output[0]["LAST_NAME"].ToString());
        }
    }
}
