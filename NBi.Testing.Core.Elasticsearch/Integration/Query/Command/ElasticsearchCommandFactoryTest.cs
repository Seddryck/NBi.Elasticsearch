using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Query;
using NUnit.Framework;
using Moq;
using NBi.Core.Elasticsearch.Query.Client;
using NBi.Core.Elasticsearch.Query.Command;

namespace NBi.Testing.Core.Elasticsearch.Integration.Query.Command
{
    [TestFixture]
    public class ElasticsearchCommandFactoryTest
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

        //[Test]
        //public void Instantiate_NoParameter_CorrectResultSet()
        //{
        //    ElasticsearchClient ElasticsearchClient = new ElasticsearchClientFactory().Instantiate(ConnectionStringReader.GetLocaleElasticsearch()) as ElasticsearchClient;
        //    var query = Mock.Of<IQuery>(
        //        x => x.Statement == "g.V()"
        //        );
        //    var factory = new ElasticsearchCommandFactory();
        //    var ElasticsearchQuery = (factory.Instantiate(ElasticsearchClient, query).Implementation) as ElasticsearchCommandOperation;
        //    var statement = ElasticsearchQuery.PreparedStatement;

        //    var client = ElasticsearchClient.
        //    var results = client.Run(statement);
        //    Assert.That(results.Count, Is.EqualTo(4));
        //}
    }
}
