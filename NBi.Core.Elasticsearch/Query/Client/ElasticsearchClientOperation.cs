using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;
using NBi.Core.Elasticsearch.Query.Command;

namespace NBi.Core.Elasticsearch.Query.Client
{
    class ElasticsearchClientOperation
    {
        private readonly ElasticLowLevelClient lowLevelClient;

        public ElasticsearchClientOperation(ElasticsearchClientOption operation)
        {
            var node = new Uri($"http://{operation.Hostname}:{operation.Port.ToString()}");
            var config = new ConnectionConfiguration(node);

            if (!string.IsNullOrEmpty(operation.Username))
                config = config.BasicAuthentication(operation.Username, operation.Password);

            lowLevelClient = new ElasticLowLevelClient(config);
        }

        public StringResponse Execute(ElasticsearchSearch query)
        {
            if (string.IsNullOrEmpty(query.Index))
                return lowLevelClient.Search<StringResponse>(query.Query);
            else if (string.IsNullOrEmpty(query.Type))
                return lowLevelClient.Search<StringResponse>(query.Index, query.Query);
            else
                return lowLevelClient.Search<StringResponse>(query.Index, query.Type, query.Query);           
        }

    }
}
    
