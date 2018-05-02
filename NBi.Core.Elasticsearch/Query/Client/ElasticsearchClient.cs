using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Elasticsearch.Query.Client
{
    class ElasticsearchClient : Extensibility.Query.IClient
    {
        private ElasticsearchClientOption Option { get; set; }

        public string ConnectionString { get => Option.ConnectionString; }
        public Type UnderlyingSessionType { get => typeof(ElasticsearchClientOperation); }

        public virtual object CreateNew() => CreateClientOperation();
        private ElasticsearchClientOperation CreateClientOperation()
            =>  new ElasticsearchClientOperation(Option);

        internal ElasticsearchClient(ElasticsearchClientOption option)
            => Option = option;
    }
}
