using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Elasticsearch.Query.Execution
{
    class QueryResultParser : BaseResultParser, IResultParser
    {
        public bool CanHandle(JObject result)
            => result.Properties().Any(x => x.Name == "hits");

        protected override IEnumerable<JToken> OnExtractObjects(JToken root)
            => root.SelectTokens("hits.hits[*]._source").ToArray();
    }
}
