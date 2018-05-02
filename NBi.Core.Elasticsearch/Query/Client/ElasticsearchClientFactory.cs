using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Elasticsearch.Query.Client
{
    public class ElasticsearchClientFactory : Extensibility.Query.IClientFactory
    {
        private IEnumerable<IConnectionStringParser> Parsers { get; }

        public ElasticsearchClientFactory()
        {
            Parsers = new List<IConnectionStringParser>()
            {
                new UriConnectionStringParser(),
                new TokenConnectionStringParser()
            };
        }

        internal ElasticsearchClientFactory(IEnumerable<IConnectionStringParser> parsers)
        {
            Parsers = new List<IConnectionStringParser>(parsers);
        }

        public bool CanHandle(string connectionString)
            => Parsers.Any(p => p.CanHandle(connectionString));
        
        public Extensibility.Query.IClient Instantiate(string connectionString)
        {
            var parser = Parsers.First(p => p.CanHandle(connectionString));
            var option = parser.Execute(connectionString);
            return new ElasticsearchClient(option);
        }
    }
}
