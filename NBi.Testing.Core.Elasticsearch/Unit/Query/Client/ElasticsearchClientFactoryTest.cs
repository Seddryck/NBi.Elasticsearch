using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Elasticsearch.Query.Client;

namespace NBi.Testing.Core.Elasticsearch.Unit.Query.Client
{
    public class ElasticsearchClientFactoryTest
    {
        [Test]
        public void CanHandle_ElasticsearchWithApi_True()
        {
            var factory = new ElasticsearchClientFactory();
            Assert.That(factory.CanHandle($@"Hostname=localhost;port=9200;Username=admin;password=p@ssw0rd;api=Elasticsearch"), Is.True);
        }

        [Test]
        public void CanHandle_ElasticsearchWithApi_WithoutBasicAuthTrue()
        {
            var factory = new ElasticsearchClientFactory();
            Assert.That(factory.CanHandle($@"Hostname=localhost;port=9200;api=Elasticsearch"), Is.True);
        }

        [Test]
        public void CanHandle_OleDbConnectionString_False()
        {
            var factory = new ElasticsearchClientFactory();
            Assert.That(factory.CanHandle("data source=SERVER;initial catalog=DB;IntegratedSecurity=true;Provider=OLEDB.1"), Is.False);
        }

        [Test]
        public void CanHandle_OleDbConnectionString_HalfBasicAuthFalse()
        {
            var factory = new ElasticsearchClientFactory();
            Assert.That(factory.CanHandle($@"Hostname=localhost;port=9200;Username=admin;api=Elasticsearch"), Is.False);
        }

        [Test]
        public void Instantiate_ElasticsearchConnectionString_ElasticsearchClient()
        {
            var factory = new ElasticsearchClientFactory();
            var session = factory.Instantiate($@"Hostname=localhost;port=9200;Username=admin;password=p@ssw0rd;api=Elasticsearch");
            Assert.That(session, Is.Not.Null);
            Assert.That(session, Is.TypeOf<ElasticsearchClient>());
        }

        [Test]
        public void Instantiate_ElasticsearchConnectionStringWithoutBasicAuth_ElasticsearchClient()
        {
            var factory = new ElasticsearchClientFactory();
            var session = factory.Instantiate($@"Hostname=localhost;port=9200;api=Elasticsearch");
            Assert.That(session, Is.Not.Null);
            Assert.That(session, Is.TypeOf<ElasticsearchClient>());
        }

    }
}
