﻿using NBi.Core.Elasticsearch.Query.Execution;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Elasticsearch.Unit.Query.Execution
{
    public class QueryResultParserTest
    {
        private JObject JSon;

        [SetUp]
        public void SetUp()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"{GetType().Namespace}.Resources.QueryResult.json";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                var text = reader.ReadToEnd();
                JSon = JObject.Parse(text);
            }
        }

        [Test]
        public void CanHandle_QueryResult_True()
        {
            var parser = new QueryResultParser();
            Assert.That(parser.CanHandle(JSon), Is.True);
        }

        [Test]
        public void Execute_QueryResult_DataSet()
        {
            var parser = new QueryResultParser();
            var dt = parser.Execute(JSon);
            Assert.That(dt.Rows.Count, Is.EqualTo(5));
            Assert.That(dt.Columns.Count, Is.EqualTo(3));
        }

    }
}
