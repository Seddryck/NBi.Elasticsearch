using NBi.Core.Query;
using NBi.Core.Query.Command;
using NBiClient = NBi.Core.Query.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using NBi.Core.Elasticsearch.Query.Client;
using NBi.Core.Elasticsearch.Query.Command;
using NBi.Extensibility.Query;

namespace NBi.Testing.Core.Elasticsearch.Unit.Query.Command
{
    public class ElasticsearchCommandFactoryTest
    {
        private string base64AuthKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("@uthK3y"));

        [Test]
        public void CanHandle_ElasticsearchClient_True()
        {
            var client = new ElasticsearchClient("host", 9200, "user", "p@ssw0rd");
            var factory = new ElasticsearchCommandFactory();
            Assert.That(factory.CanHandle(client), Is.True);
        }

        [Test]
        public void CanHandle_OtherKindOfClient_False()
        {
            var client = Mock.Of<IClient>();
            var factory = new ElasticsearchCommandFactory();
            Assert.That(factory.CanHandle(client), Is.False);
        }

        [Test]
        public void Instantiate_ElasticsearchClientAndQuery_CommandNotNull()
        {
            var client = new ElasticsearchClient("host", 9200, "user", "p@ssw0rd");
            var query = Mock.Of<IQuery>(x => x.Statement == @"GET index/type/_search { ""query"": {""match_all"": { }} }");
            var factory = new ElasticsearchCommandFactory();
            var command = factory.Instantiate(client, query);
            Assert.That(command, Is.Not.Null);
        }

        [Test]
        public void Instantiate_ElasticsearchClientAndQuery_CommandImplementationCorrectType()
        {
            var client = new ElasticsearchClient("host", 9200, "user", "p@ssw0rd");
            var query = Mock.Of<IQuery>(x => x.Statement == @"GET index/type/_search { ""query"": {""match_all"": { }} }");
            var factory = new ElasticsearchCommandFactory();
            var command = factory.Instantiate(client, query);
            var impl = command.Implementation;
            Assert.That(impl, Is.Not.Null);
            Assert.That(impl, Is.TypeOf<ElasticsearchCommandOperation>());
        }

        [Test]
        public void Instantiate_ElasticsearchClientAndQuery_ClientCorrectType()
        {
            var client = new ElasticsearchClient("host", 9200, "user", "p@ssw0rd");
            var query = Mock.Of<IQuery>(x => x.Statement == @"GET index/type/_search { ""query"": {""match_all"": { }} }");
            var factory = new ElasticsearchCommandFactory();
            var command = factory.Instantiate(client, query);
            var impl = command.Client;
            Assert.That(impl, Is.Not.Null);
            Assert.That(impl, Is.InstanceOf<ElasticsearchClientOperation>());
        }
    }
}
