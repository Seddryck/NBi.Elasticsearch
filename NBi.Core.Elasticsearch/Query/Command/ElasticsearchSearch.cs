using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Elasticsearch.Query.Command
{
    public class ElasticsearchSearch
    {
        public string Index { get; set; }
        public string Type { get; set; }
        public string Query { get; set; }
    }
}
