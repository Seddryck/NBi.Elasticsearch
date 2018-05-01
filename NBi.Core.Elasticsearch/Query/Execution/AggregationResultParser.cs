using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Elasticsearch.Query.Execution
{
    class AggregationResultParser : BaseResultParser, IResultParser
    {
        protected override bool OnComplexParsing(object jsonField, out object dataField)
        {
            if (jsonField is JObject
                    && (jsonField as JObject).Children<JProperty>().Count() == 1
                    && (jsonField as JObject).SelectToken("value") != null)
            {
                dataField = (jsonField as JObject).SelectToken("value").ToObject<object>();
                return true;
            }
            else
            {
                dataField = null;
                return false;
            }
        }

        public bool CanHandle(JObject result)
            => result.Properties().Any(x => x.Name=="aggregations");
                
        protected override IEnumerable<JToken> OnExtractObjects(JToken root)
            => root.SelectTokens("aggregations.*.buckets").Values().ToArray();
    }
}
