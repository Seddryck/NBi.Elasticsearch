using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Query;
using NUnit.Framework;
using System.Data;
using NBi.Core.Elasticsearch.Query.Client;
using NBi.Core.Elasticsearch.Query.Command;
using NBi.Core.Elasticsearch.Query.Execution;
using Moq;
using NBi.Extensibility.Query;

namespace NBi.Testing.Core.Elasticsearch.Integration.Query.Execution
{
    [TestFixture]
    public class ElasticsearchExecutionEngineTest
    {

        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [TestFixtureTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion

        [Test]
        public void Execute_Documents_DataSetFilled()
        {
            ElasticsearchClient client = new ElasticsearchClientFactory().Instantiate(ConnectionStringReader.GetElasticsearch()) as ElasticsearchClient;
            var statement = Mock.Of<IQuery>(
                x => x.Statement == 
                @"GET bank/_search" +
                @"{" +
                @"   ""query"": {""match_all"": { }}"+
                @"   , ""size"": 5" +
                @"   , ""_source"": [""gender"", ""age"", ""balance""]" +
                @"   , ""sort"" : [ { ""balance"" : {""order"" : ""desc""}}]" +
                @"}");
            ElasticsearchCommandOperation ElasticsearchQuery = new ElasticsearchCommandFactory().Instantiate(client, statement).Implementation as ElasticsearchCommandOperation;

            var engine = new ElasticsearchExecutionEngine((ElasticsearchClientOperation)(client.CreateNew()), ElasticsearchQuery);

            var ds = engine.Execute();
            Assert.That(ds.Tables, Has.Count.EqualTo(1));
            Assert.That(ds.Tables[0].Rows, Has.Count.EqualTo(5));
            Assert.That(ds.Tables[0].Columns, Has.Count.EqualTo(3));

            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("gender"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("age"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("balance"));

            var genders = new List<object>();
            var ages = new List<object>();
            foreach (DataRow row in ds.Tables[0].Rows)
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    if (row.Table.Columns[i].ColumnName == "gender")
                        genders.Add(row.ItemArray[i]);
                    else if (row.Table.Columns[i].ColumnName == "age")
                        ages.Add(row.ItemArray[i]);
                }

            foreach (var expectedFirstName in new[] { "F", "M" })
                Assert.That(genders, Has.Member(expectedFirstName));

            foreach (var expectedAge in new object[] { 36, 25, 35, 40, 23 })
                Assert.That(ages, Has.Member(expectedAge));

            foreach (DataRow row in ds.Tables[0].Rows)
                Assert.That(row["balance"], Is.Not.Null.Or.Empty);
        }

        

        [Test]
        public void Execute_Aggregations_DataSetFilled()
        {
            ElasticsearchClient client = new ElasticsearchClientFactory().Instantiate(ConnectionStringReader.GetElasticsearch()) as ElasticsearchClient;
            var statement = Mock.Of<IQuery>(
                x => x.Statement ==
                @"GET /bank/_search" +
                @"{" +
                @"  ""size"": 5,"+
                @"  ""aggs"": {"+
                @"      ""group_by_state"": {"+
                @"          ""terms"": {"+
                @"            ""field"": ""state.keyword""," +
                @"            ""size"": 3," +
                @"            ""order"": {" +
                @"              ""average_balance"": ""desc""" +
                @"            }"+
                @"          },"+
                @"          ""aggs"": {"+
                @"              ""average_balance"": {"+
                @"                  ""avg"": {"+
                @"                      ""field"": ""balance"""+
                @"                  }" +
                @"              }" +
                @"          }" +
                @"      }" +
                @"  }" +
                @"}");
            ElasticsearchCommandOperation ElasticsearchQuery = new ElasticsearchCommandFactory().Instantiate(client, statement).Implementation as ElasticsearchCommandOperation;

            var engine = new ElasticsearchExecutionEngine((ElasticsearchClientOperation)(client.CreateNew()), ElasticsearchQuery);

            var ds = engine.Execute();
            Assert.That(ds.Tables, Has.Count.EqualTo(1));
            Assert.That(ds.Tables[0].Rows, Has.Count.EqualTo(3));

            Assert.That(ds.Tables[0].Columns, Has.Count.GreaterThanOrEqualTo(2));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("key"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("average_balance"));

            var states = new List<object>();
            foreach (DataRow row in ds.Tables[0].Rows)
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    if (row.Table.Columns[i].ColumnName == "key")
                        states.Add(row.ItemArray[i]);
                }

            foreach (var expectedState in new[] { "WA", "AL", "RI" })
                Assert.That(states, Has.Member(expectedState));

            foreach (DataRow row in ds.Tables[0].Rows)
                Assert.That(row["average_balance"], Is.Positive);

        }

        //[Test]
        //public void Execute_Integer_ScalarReturned()
        //{
        //    ElasticsearchClient client = new ElasticsearchClientFactory().Instantiate(ConnectionStringReader.GetElasticsearch()) as ElasticsearchClient;
        //    var statement = Mock.Of<IQuery>(x => x.Statement == "g.V().count()");
        //    ElasticsearchCommandOperation ElasticsearchQuery = new ElasticsearchCommandFactory().Instantiate(client, statement).Implementation as ElasticsearchCommandOperation;

        //    var engine = new ElasticsearchExecutionEngine((ElasticsearchClientOperation)(client.CreateNew()), ElasticsearchQuery);

        //    var count = engine.ExecuteScalar();
        //    Assert.That(count, Is.EqualTo(4));
        //}

        //[Test]
        //public void Execute_String_ScalarReturned()
        //{
        //    ElasticsearchClient client = new ElasticsearchClientFactory().Instantiate(ConnectionStringReader.GetElasticsearch()) as ElasticsearchClient;
        //    var statement = Mock.Of<IQuery>(x => x.Statement == "g.V().has('firstName','Mary').values('lastName')");
        //    ElasticsearchCommandOperation ElasticsearchQuery = new ElasticsearchCommandFactory().Instantiate(client, statement).Implementation as ElasticsearchCommandOperation;

        //    var engine = new ElasticsearchExecutionEngine((ElasticsearchClientOperation)(client.CreateNew()), ElasticsearchQuery);

        //    var count = engine.ExecuteScalar();
        //    Assert.That(count, Is.EqualTo("Andersen"));
        //}

        //[Test]
        //public void Execute_NullString_ScalarReturned()
        //{
        //    ElasticsearchClient client = new ElasticsearchClientFactory().Instantiate(ConnectionStringReader.GetElasticsearch()) as ElasticsearchClient;
        //    var statement = Mock.Of<IQuery>(x => x.Statement == "g.V().has('firstName','Thomas').values('lastName')");
        //    ElasticsearchCommandOperation commandOperation = new ElasticsearchCommandFactory().Instantiate(client, statement).Implementation as ElasticsearchCommandOperation;

        //    var engine = new ElasticsearchExecutionEngine((ElasticsearchClientOperation)(client.CreateNew()), commandOperation);

        //    var count = engine.ExecuteScalar();
        //    Assert.That(count, Is.Null);
        //}

        //[Test]
        //public void Execute_ListOfString_ListReturned()
        //{
        //    ElasticsearchClient client = new ElasticsearchClientFactory().Instantiate(ConnectionStringReader.GetElasticsearch()) as ElasticsearchClient;
        //    var statement = Mock.Of<IQuery>(x => x.Statement == "g.V().values('lastName')");
        //    ElasticsearchCommandOperation ElasticsearchQuery = new ElasticsearchCommandFactory().Instantiate(client, statement).Implementation as ElasticsearchCommandOperation;

        //    var engine = new ElasticsearchExecutionEngine((ElasticsearchClientOperation)(client.CreateNew()), ElasticsearchQuery);

        //    var count = engine.ExecuteList<string>();
        //    Assert.That(count, Has.Member("Andersen"));
        //    Assert.That(count, Has.Member("Miller"));
        //    Assert.That(count, Has.Member("Wakefield"));
        //}

    }
}
