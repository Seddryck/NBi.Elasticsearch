using NBi.Core.Elasticsearch.Query.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Elasticsearch.Unit.Query.Client
{
    public class UriConnectionStringParserTest
    {
        [Test]
        public void CanHandle_UriFull_True()
        {
            var parser = new UriConnectionStringParser();
            var value = parser.CanHandle($@"elasticsearch://user:passw0rd@host:9200");
            Assert.That(value, Is.True);
        }

        [Test]
        public void CanHandle_ElasticsearchWithApi_False()
        {
            var parser = new UriConnectionStringParser();
            var value = parser.CanHandle($@"Hostname=localhost;port=9200;Username=admin;password=p@ssw0rd;api=Elasticsearch");
            Assert.That(value, Is.False);
        }

        [Test]
        public void CanHandle_OleDbConnectionString_False()
        {
            var parser = new UriConnectionStringParser();
            var value = parser.CanHandle("data source=SERVER;initial catalog=DB;IntegratedSecurity=true;Provider=OLEDB.1");
            Assert.That(value, Is.False);
        }

        [Test]
        public void Execute_Hostname_FromConnectionString()
        {
            var parser = new UriConnectionStringParser();
            var option = parser.Execute($@"elasticsearch://user:passw0rd@localhost:9200");
            Assert.That(option.Hostname, Is.EqualTo("localhost"));
        }

        [Test]
        public void Execute_NoPort_DefaultPort()
        {
            var parser = new UriConnectionStringParser();
            var option = parser.Execute($@"elasticsearch://localhost");
            Assert.That(option.Port, Is.EqualTo(9200));
        }

        [Test]
        public void Execute_WithPort_PortFromConnectionString()
        {
            var parser = new UriConnectionStringParser();
            var option = parser.Execute($@"elasticsearch://localhost:9201");
            Assert.That(option.Port, Is.EqualTo(9201));
        }

        [Test]
        public void Execute_WithBasicAuth_BasicAuth()
        {
            var parser = new UriConnectionStringParser();
            var option = parser.Execute($@"elasticsearch://admin:passw0rd@localhost:9200");
            Assert.That(option.Username, Is.EqualTo("admin"));
            Assert.That(option.Password, Is.EqualTo("passw0rd"));
        }
    }
}
