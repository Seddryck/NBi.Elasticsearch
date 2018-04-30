using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Elasticsearch.Query.Client
{
    class UriConnectionStringParser : IConnectionStringParser
    {
        public virtual bool CanHandle(string connectionString)
        {
            if (Uri.IsWellFormedUriString(connectionString, UriKind.Absolute))
            {
                var api = new Uri(connectionString).Scheme;
                return StringComparer.InvariantCultureIgnoreCase.Compare(api, "elasticsearch") == 0;
            }
            else
                return false;
        }

        public virtual ElasticsearchClientOption Execute(string connectionString)
        {
            if (!CanHandle(connectionString))
                throw new InvalidOperationException();

            var uri = new Uri(connectionString);
            var option = new ElasticsearchClientOption(connectionString)
            {
                Hostname = uri.Host,
                Port = uri.Port == -1 ? 9200 : uri.Port
            };

            if (!string.IsNullOrEmpty(uri.UserInfo))
            {
                option.Username = uri.UserInfo.Split(':')[0];
                option.Password = uri.UserInfo.Split(':')[1];
            }

            return option;
        }
    }
}
