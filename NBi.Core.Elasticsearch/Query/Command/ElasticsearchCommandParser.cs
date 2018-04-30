using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Elasticsearch.Query.Command
{
    class ElasticsearchCommandParser
    {
        public ElasticsearchSearch Execute(string statement)
        {
            statement = statement.Trim();
            var startCurlyBraceIndex = statement.IndexOf('{');
            if (startCurlyBraceIndex < 0)
                throw new ArgumentException("Statement must contain a '{'");
            if (statement.Last() != '}')
                throw new ArgumentException("Statement must end by a '}'");

            var spaceTokens = statement.Substring(0, startCurlyBraceIndex).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (spaceTokens[0] == "GET")
                spaceTokens = spaceTokens.Skip(1).ToArray();

            var slashTokens = spaceTokens[0].Trim().Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            slashTokens = slashTokens.Where(x => x != "_search").ToArray();
            if (slashTokens.Count() == 0)
                throw new ArgumentException("You must specify the index and optionally the type");
            if (slashTokens.Count() > 2)
                throw new ArgumentException("You must specify the index and optionally the type, nothing else.");

            var search = new ElasticsearchSearch
            {
                Index = slashTokens[0],
                Type = slashTokens.Count() > 1 ? slashTokens[1] : string.Empty,
                Query = statement.Substring(startCurlyBraceIndex)
            };

            return search;

        }
    }
}
