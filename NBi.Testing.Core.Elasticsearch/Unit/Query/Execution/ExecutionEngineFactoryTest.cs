using Moq;
using NBi.Core.Configuration;
using NBi.Core.Elasticsearch.Query.Command;
using NBi.Core.Elasticsearch.Query.Execution;
using NBi.Core.Elasticsearch.Query.Client;
using NBi.Core.Query.Command;
using NBi.Core.Query.Execution;
using NBi.Core.Query.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Testing.Core.Elasticsearch.Unit.Query.Execution
{
    public class ExecutionEngineFactoryTest
    {
        private string base64AuthKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("@uthK3y"));

        private class ElasticsearchConfig : IExtensionsConfiguration
        {
            public IReadOnlyCollection<Type> Extensions => new List<Type>()
            {
                typeof(ElasticsearchClientFactory),
                typeof(ElasticsearchCommandFactory),
                typeof(ElasticsearchExecutionEngine),
            };
        }

        [Test]
        public void Instantiate_ElasticsearchConnectionString_ElasticsearchExecutionEngine()
        {
            var config = new ElasticsearchConfig();
            var clientProvider = new ClientProvider(config);
            var commandProvider = new CommandProvider(config);
            var factory = new ExecutionEngineFactory(clientProvider, commandProvider, config);

            var query = Mock.Of<IQuery>
                (
                    x => x.ConnectionString == $@"Hostname=localhost;port=9200;Username=admin;password=p@ssw0rd;api=Elasticsearch"
                    && x.Statement == @"GET index/type/_search { ""query"": {""match_all"": { }} }"
            
                );

            var engine = factory.Instantiate(query);
            Assert.That(engine, Is.Not.Null);
            Assert.That(engine, Is.TypeOf<ElasticsearchExecutionEngine>());
        }

    }
}
