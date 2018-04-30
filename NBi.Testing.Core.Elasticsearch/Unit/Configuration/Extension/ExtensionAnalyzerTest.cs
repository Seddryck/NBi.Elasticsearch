using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Configuration.Extension;
using NBi.Core.Elasticsearch.Query.Client;
using NBi.Core.Elasticsearch.Query.Command;
using NBi.Core.Elasticsearch.Query.Execution;

namespace NBi.Testing.Core.Elasticsearch.Unit.Configuration.Extension
{
    public class ExtensionAnalyzerTest
    {
        [Test]
        public void Execute_Elasticsearch_Six()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.Elasticsearch");
            Assert.That(types.Count(), Is.EqualTo(3));
        }

        [Test]
        public void Execute_ElasticsearchApi_IClientFactory()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.Elasticsearch");
            Assert.That(types, Has.Member(typeof(ElasticsearchClientFactory)));
        }

        [Test]
        public void Execute_ElasticsearchApi_ICommandFactory()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.Elasticsearch");
            Assert.That(types, Has.Member(typeof(ElasticsearchCommandFactory)));
        }

        [Test]
        public void Execute_ElasticsearchApi_IExecutionEngine()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.Elasticsearch");
            Assert.That(types, Has.Member(typeof(ElasticsearchExecutionEngine)));
        }
    }
}
