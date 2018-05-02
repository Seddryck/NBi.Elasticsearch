using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Elasticsearch.Query.Client;
using Moq;

namespace NBi.Testing.Core.Elasticsearch.Unit.Query.Client
{
    public class ElasticsearchClientFactoryTest
    {
        [Test]
        public void CanHandle_WrongParsers_AllCalled()
        {
            var uriParser = new Mock<IConnectionStringParser>();
            uriParser.Setup(x => x.CanHandle(It.IsAny<string>())).Returns(false);

            var tokenParser = new Mock<IConnectionStringParser>();
            tokenParser.Setup(x => x.CanHandle(It.IsAny<string>())).Returns(false);

            var factory = new ElasticsearchClientFactory(new IConnectionStringParser[] { uriParser.Object, tokenParser.Object });
            var result = factory.CanHandle("myConnectionString");

            uriParser.Verify(x => x.CanHandle("myConnectionString"), Times.Once);
            tokenParser.Verify(x => x.CanHandle("myConnectionString"), Times.Once);
        }

        [Test]
        public void CanHandle_ValidParser_NextNotCalled()
        {
            var uriParser = new Mock<IConnectionStringParser>();
            uriParser.Setup(x => x.CanHandle(It.IsAny<string>())).Returns(true);

            var tokenParser = new Mock<IConnectionStringParser>();
            tokenParser.Setup(x => x.CanHandle(It.IsAny<string>())).Returns(false);

            var factory = new ElasticsearchClientFactory(new IConnectionStringParser[] { uriParser.Object, tokenParser.Object });
            var result = factory.CanHandle("myConnectionString");

            uriParser.Verify(x => x.CanHandle("myConnectionString"), Times.Once);
            tokenParser.Verify(x => x.CanHandle(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Instantiate_ElasticsearchTokens_ElasticsearchClient()
        {
            var factory = new ElasticsearchClientFactory();
            var client = factory.Instantiate($@"Hostname=localhost;port=9200;Username=admin;password=p@ssw0rd;api=Elasticsearch");
            Assert.That(client, Is.Not.Null);
            Assert.That(client, Is.TypeOf<ElasticsearchClient>());
        }

        [Test]
        public void Instantiate_ElasticsearchUri_ElasticsearchClient()
        {
            var factory = new ElasticsearchClientFactory();
            var client = factory.Instantiate($@"elasticsearch://locahost");
            Assert.That(client, Is.Not.Null);
            Assert.That(client, Is.TypeOf<ElasticsearchClient>());
        }
        
    }
}
