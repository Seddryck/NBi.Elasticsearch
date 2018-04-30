using NBi.Core.Elasticsearch.Query.Command;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Elasticsearch.Unit.Query.Command
{
    public class ElasticsearchCommandParserTest
    {
        [Test]
        public void Execute_GetIndexTypeSearch_Successfull()
        {
            var parser = new ElasticsearchCommandParser();
            var search = parser.Execute(@"GET index/type/_search { ""query"": {""match_all"": { }} }");
            Assert.That(search.Index, Is.EqualTo("index"));
            Assert.That(search.Type, Is.EqualTo("type"));
        }

        [Test]
        public void Execute_GetIndexTypeSearchNewLine_Successfull()
        {
            var parser = new ElasticsearchCommandParser();
            var search = parser.Execute(@"GET  index/type/_search \r\n { ""query"": {""match_all"": { }} }");
            Assert.That(search.Index, Is.EqualTo("index"));
            Assert.That(search.Type, Is.EqualTo("type"));
        }

        [Test]
        public void Execute_GetIndexTypeNoSearch_Successfull()
        {
            var parser = new ElasticsearchCommandParser();
            var search = parser.Execute(@"GET  index/type { ""query"": {""match_all"": { }} }");
            Assert.That(search.Index, Is.EqualTo("index"));
            Assert.That(search.Type, Is.EqualTo("type"));
        }

        [Test]
        public void Execute_GetIndexNoTypeNoSearch_Successfull()
        {
            var parser = new ElasticsearchCommandParser();
            var search = parser.Execute(@"GET index{ ""query"": {""match_all"": { }} }");
            Assert.That(search.Index, Is.EqualTo("index"));
            Assert.That(search.Type, Is.Empty);
        }

        [Test]
        public void Execute_NoGetIndexNoTypeSearch_Successfull()
        {
            var parser = new ElasticsearchCommandParser();
            var search = parser.Execute(@"index/_search { ""query"": {""match_all"": { }} }");
            Assert.That(search.Index, Is.EqualTo("index"));
            Assert.That(search.Type, Is.Empty);
        }

        [Test]
        public void Execute_CurlyBrace_Successfull()
        {
            var parser = new ElasticsearchCommandParser();
            var search = parser.Execute(@"index/_search { ""query"": {""match_all"": { }} }");
            Assert.That(search.Query, Is.EqualTo(@"{ ""query"": {""match_all"": { }} }"));
        }

        [Test]
        public void Execute_CurlyBraceTrailingNewLine_Successfull()
        {
            var parser = new ElasticsearchCommandParser();
            var search = parser.Execute("index/_search {\r\n\t\"query\":\r\n\t\t{\"match_all\": { }}\r\n\t}\r\n");
            Assert.That(search.Query, Is.EqualTo("{\r\n\t\"query\":\r\n\t\t{\"match_all\": { }}\r\n\t}"));
        }

        [Test]
        public void Execute_SearchNewLine_Successfull()
        {
            var parser = new ElasticsearchCommandParser();
            var search = parser.Execute("GET index/_search\n{\r\n\t\"query\":\r\n\t\t{\"match_all\": { }}\r\n\t}\r\n");
            Assert.That(search.Type, Is.Null.Or.Empty);
        }
    }
}
