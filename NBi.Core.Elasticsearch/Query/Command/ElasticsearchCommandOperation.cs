using NBi.Core.Elasticsearch.Query.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Elasticsearch.Query.Command
{
    class ElasticsearchCommandOperation
    {
        public ElasticsearchSearch Search { get; }
        public ElasticsearchClientOperation Client { get; }
        public ElasticsearchCommandOperation(ElasticsearchClientOperation client, string statement)
        {
            Client = client;
            var parser = new ElasticsearchCommandParser();
            Search = parser.Execute(statement);
        }
    }
}
