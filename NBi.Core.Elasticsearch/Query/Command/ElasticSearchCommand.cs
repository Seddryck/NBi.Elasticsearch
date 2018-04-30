using NBi.Core.Elasticsearch.Query.Client;
using NBi.Extensibility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Elasticsearch.Query.Command
{
    class ElasticsearchCommand : ICommand
    {
        public object Implementation { get; }
        public object Client { get; }

        public ElasticsearchCommand(ElasticsearchClientOperation client, ElasticsearchCommandOperation query)
        {
            Client = client;
            Implementation = query;
        }
    }
}
