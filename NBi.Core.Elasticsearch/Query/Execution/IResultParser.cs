using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Elasticsearch.Query.Execution
{
    interface IResultParser
    {
        DataTable Execute(JToken result);
        bool CanHandle (JObject result);
    }
}
