using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Elasticsearch.Query.Client
{
    static class DbConnectionStringBuilderExtension
    {
        public static T Get<T>(this DbConnectionStringBuilder tokens, string name, T defaultValue)
            => tokens.ContainsKey(name) ? (T)Convert.ChangeType(tokens[name], typeof(T)) : defaultValue;

        public static bool TryGet<T>(this DbConnectionStringBuilder tokens, string name, out T value)
        {
            if (tokens.ContainsKey(name))
            {
                value = (T)tokens[name];
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }

        }
    }
}
