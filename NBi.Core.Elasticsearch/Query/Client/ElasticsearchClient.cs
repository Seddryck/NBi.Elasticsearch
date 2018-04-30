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
        public const string HostnameToken = "hostname";
        public const string PortToken = "port";
        public const string UsernameToken = "username";
        public const string PasswordToken = "password";
        public const string ApiToken = "api";

        protected string Hostname { get => (string)ConnectionStringTokens[HostnameToken]; }
        protected int Port { get => Int32.Parse((string)ConnectionStringTokens[PortToken]); }
        protected string Username { get => ConnectionStringTokens.ContainsKey(UsernameToken) ? (string)ConnectionStringTokens[UsernameToken] : string.Empty; }
        protected string Password { get => ConnectionStringTokens.ContainsKey(PasswordToken) ? (string)ConnectionStringTokens[PasswordToken] : string.Empty; }
        
        protected DbConnectionStringBuilder ConnectionStringTokens => new DbConnectionStringBuilder() { ConnectionString = ConnectionString };

        public string ConnectionString { get; }
        public Type UnderlyingSessionType { get => typeof(ElasticsearchClientOperation); }

        public virtual object CreateNew() => CreateClientOperation();
        private ElasticsearchClientOperation CreateClientOperation()
            =>  new ElasticsearchClientOperation(Hostname, Port, Username, Password);

        internal ElasticsearchClient(string hostname, int port, string username, string password)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder
            {
                { HostnameToken, hostname },
                { PortToken, port.ToString() },
                { UsernameToken, username },
                { PasswordToken, password}
            };
            ConnectionString = connectionStringBuilder.ToString();
        }
    }
}
