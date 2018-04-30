using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Elasticsearch.Query.Client
{
    public interface IConnectionStringParser
    {
        bool CanHandle(string connectionString);
        ElasticsearchClientOption Execute(string connectionString);
    }
}
