using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Elasticsearch.Query.Client
{
    public class ElasticsearchClientOption
    {
        public ElasticsearchClientOption(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }

        public string Hostname { get; internal set; }
        public int Port { get; internal set; }
        public string Username { get; internal set; }
        public string Password { get; internal set; }

    }
}
