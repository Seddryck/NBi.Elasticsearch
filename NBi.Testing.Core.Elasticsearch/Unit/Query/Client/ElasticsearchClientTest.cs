using NBi.Core;
using NBi.Core.Elasticsearch.Query.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Elasticsearch.Unit.Query.Client
{
    public class ElasticsearchClientTest
    {
        [Test]
        public void InstantiateUnderlyingSession_ElasticsearchConnectionString_IClient()
        {
            var factory = new ElasticsearchClientFactory();
            var client = factory.Instantiate($@"Hostname=localhost;port=9200;Username=admin;password=p@ssw0rd;api=Elasticsearch");
            Assert.That(client.UnderlyingSessionType, Is.EqualTo(typeof(ElasticsearchClientOperation)));
        }

        [Test]
        public void InstantiateCreate_ElasticsearchConnectionString_IClient()
        {
            var factory = new ElasticsearchClientFactory();
            var client = factory.Instantiate($@"Hostname=localhost;port=9200;Username=admin;password=p@ssw0rd;api=Elasticsearch");
            var underlyingSession = client.CreateNew();
            Assert.That(underlyingSession, Is.Not.Null);
            Assert.That(underlyingSession, Is.AssignableTo<ElasticsearchClientOperation>());
        }
    }
}
