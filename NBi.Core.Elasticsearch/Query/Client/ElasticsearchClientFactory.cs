using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Elasticsearch.Query.Client
{
    public class ElasticsearchClientFactory : Extensibility.Query.IClientFactory
    {

        public bool CanHandle(string connectionString)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = connectionString };
            return connectionStringBuilder.ContainsKey(ElasticsearchClient.HostnameToken)
                && HasBasicAuthOrNot(connectionStringBuilder)
                && HasApiSetTo(connectionStringBuilder, "Elasticsearch");
        }

        protected bool HasApiSetTo(DbConnectionStringBuilder connectionStringBuilder, string api)
            => connectionStringBuilder.ContainsKey(ElasticsearchClient.ApiToken) && ((string)connectionStringBuilder[ElasticsearchClient.ApiToken]).ToLowerInvariant() == api.ToLowerInvariant();

        protected bool HasBasicAuthOrNot(DbConnectionStringBuilder connectionStringBuilder)
            =>  (
                    connectionStringBuilder.ContainsKey(ElasticsearchClient.UsernameToken) 
                    && connectionStringBuilder.ContainsKey(ElasticsearchClient.PasswordToken)
                )
                ||
                (
                    !connectionStringBuilder.ContainsKey(ElasticsearchClient.UsernameToken)
                    && !connectionStringBuilder.ContainsKey(ElasticsearchClient.PasswordToken)
                );

        public Extensibility.Query.IClient Instantiate(string connectionString)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = connectionString };
            var hostname = (string)connectionStringBuilder[ElasticsearchClient.HostnameToken];
            var port = Int32.Parse((string)connectionStringBuilder[ElasticsearchClient.PortToken]);
            var username = connectionStringBuilder.ContainsKey(ElasticsearchClient.UsernameToken) ? (string)connectionStringBuilder[ElasticsearchClient.UsernameToken] : string.Empty;
            var password = connectionStringBuilder.ContainsKey(ElasticsearchClient.PasswordToken) ? (string)connectionStringBuilder[ElasticsearchClient.PasswordToken] : string.Empty;

            return new ElasticsearchClient(hostname, port, username, password);
        }
    }
}
