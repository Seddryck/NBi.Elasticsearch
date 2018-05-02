using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Elasticsearch.Query.Client
{
    class TokenConnectionStringParser : IConnectionStringParser
    {
        private const string Hostname = "hostname";
        private const string Port = "port";
        private const string Username = "username";
        private const string Password = "password";
        private const string Api = "api";

        public virtual bool CanHandle(string connectionString)
        {
            var tokens = new DbConnectionStringBuilder();
            try
            { tokens.ConnectionString = connectionString; }
            catch
            { return false; }
            var api = tokens.Get(Api, string.Empty);
            return StringComparer.InvariantCultureIgnoreCase.Compare(api, "elasticsearch") == 0;
        }
        
        public virtual ElasticsearchClientOption Execute(string connectionString)
        {
            var tokens = new DbConnectionStringBuilder() { ConnectionString = connectionString };
            var option = new ElasticsearchClientOption(connectionString)
            {
                Hostname = tokens.TryGet(Hostname, out string host) ? host : throw new ArgumentException("Hostname is mandatory for an Elasticsearch connection string"),
                Port = tokens.Get(Port, 9200)
            };

            if (tokens.TryGet(Username, out string username) ^ tokens.TryGet(Password, out string password))
                throw new ArgumentException("username and password must be both filled or none of them must be filled");

            option.Username = username;
            option.Password = password;

            return option;
        }


    }
}
