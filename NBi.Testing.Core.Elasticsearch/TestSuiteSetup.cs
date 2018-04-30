using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Elasticsearch
{
    [SetUpFixture]
    public class TestSuiteSetup
    {
        //private string[] Statements
        //{
        //    get => new[]
        //    {
        //        "g.V().drop()"
        //        , "g.addV('person').property('id', 'thomas').property('firstName', 'Thomas').property('age', 44)"
        //        , "g.addV('person').property('id', 'mary').property('firstName', 'Mary').property('lastName', 'Andersen').property('age', 39)"
        //        , "g.addV('person').property('id', 'ben').property('firstName', 'Ben').property('lastName', 'Miller')"
        //        , "g.addV('person').property('id', 'robin').property('firstName', 'Robin').property('lastName', 'Wakefield')"
        //        , "g.V().has('firstName','Thomas').addE('knows').to(g.V().has('firstName','Mary'))"
        //        , "g.V().has('firstName','Thomas').addE('knows').to(g.V().has('firstName','Ben'))"
        //        , "g.V().has('firstName','Ben').addE('knows').to(g.V().has('firstName','Robin'))"
        //    };
        //}

        //[SetUp]
        //public virtual void Init()
        //{
        //    if (!string.IsNullOrEmpty(ConnectionStringReader.GetElasticsearch()))
        //    {
        //        var ElasticsearchConnectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = ConnectionStringReader.GetElasticsearch() };
        //        var endpoint = new Uri(ElasticsearchConnectionStringBuilder["endpoint"].ToString());
        //        var authKey = ElasticsearchConnectionStringBuilder["authkey"].ToString();
        //        var databaseId = ElasticsearchConnectionStringBuilder["database"].ToString();
        //        var collectionId = ElasticsearchConnectionStringBuilder["collection"].ToString();

        //        using (var client = new DocumentClient(endpoint, authKey))
        //        {
        //            var databaseResponse = client.CreateDatabaseIfNotExistsAsync(new Database() { Id = databaseId }).Result;
        //            switch (databaseResponse.StatusCode)
        //            {
        //                case System.Net.HttpStatusCode.OK:
        //                    Console.WriteLine($"Database {databaseId} already exists.");
        //                    break;
        //                case System.Net.HttpStatusCode.Created:
        //                    Console.WriteLine($"Database {databaseId} created.");
        //                    break;
        //                default:
        //                    throw new ArgumentException($"Can't create database {databaseId}: {databaseResponse.StatusCode}");
        //            }

        //            var databaseUri = UriFactory.CreateDatabaseUri(databaseId);
        //            var collectionResponse = client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, new DocumentCollection() { Id = collectionId }).Result;
        //            switch (collectionResponse.StatusCode)
        //            {
        //                case System.Net.HttpStatusCode.OK:
        //                    Console.WriteLine($"Collection {collectionId} already exists.");
        //                    break;
        //                case System.Net.HttpStatusCode.Created:
        //                    Console.WriteLine($"Database {collectionId} created.");
        //                    break;
        //                default:
        //                    throw new ArgumentException($"Can't create database {collectionId}: {collectionResponse.StatusCode}");
        //            }
        //        }
        //    }
        //    var ElasticsearchConnectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = ConnectionStringReader.GetElasticsearch() };

        //    FillDatabase(
        //        ElasticsearchConnectionStringBuilder["hostname"].ToString(),
        //        Int32.Parse(ElasticsearchConnectionStringBuilder["port"].ToString()),
        //        Boolean.Parse(ElasticsearchConnectionStringBuilder["enablessl"].ToString()),
        //        ElasticsearchConnectionStringBuilder["username"].ToString(),
        //        ElasticsearchConnectionStringBuilder["password"].ToString()
        //    );
        //}

        //private void FillDatabase(string hostname, int port, bool enableSsl, string username, string password)
        //{
        //    var ElasticsearchServer = new ElasticsearchServer(hostname, port, enableSsl, username, password);

        //    using (var ElasticsearchClient = new ElasticsearchClient(ElasticsearchServer, new GraphSON2Reader(), new GraphSON2Writer(), ElasticsearchClient.GraphSON2MimeType))
        //    {
        //        var task = ElasticsearchClient.SubmitWithSingleResultAsync<Int64>("g.V().count()");
        //        task.Wait();
        //        if (task.Result != 4)
        //        {
        //            foreach (var statement in Statements)
        //            {
        //                var query = ElasticsearchClient.SubmitAsync(statement);
        //                Console.WriteLine($"Setup database: { statement }");
        //                query.Wait();
        //            }
        //        }
        //    }

        //    Console.WriteLine($"Tests will be run on '{hostname}:{port}'");

        //}
    }
}
