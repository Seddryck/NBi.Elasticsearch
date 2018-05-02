using NBi.Core.Elasticsearch.Query.Command;
using NBi.Core.Elasticsearch.Query.Client;
using NBi.Extensibility.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using NBi.Extensibility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NBi.Core.Elasticsearch.Query.Execution
{
    [SupportedCommandType(typeof(ElasticsearchCommandOperation))]
    internal class ElasticsearchExecutionEngine : IExecutionEngine
    {
        protected ElasticsearchCommandOperation Command { get; }
        protected ElasticsearchClientOperation Client { get; }

        private readonly Stopwatch stopWatch = new Stopwatch();

        private readonly IEnumerable<IResultParser> Parsers;

        protected internal ElasticsearchExecutionEngine(ElasticsearchClientOperation client, ElasticsearchCommandOperation command)
        {
            Client = client;
            Command = command;
            Parsers = new List<IResultParser>() { new AggregationResultParser(), new QueryResultParser() };
        }

        public DataSet Execute()
        {
            DataSet ds = null;
            StartWatch();
            ds = OnExecuteDataSet(Command.Client, Command.Search);
            StopWatch();
            return ds;
        }

        protected JObject OnExecute(ElasticsearchClientOperation client, ElasticsearchSearch query)
        {
            var response = client.Execute(query);

            if (!response.Success)
                throw new Exception(response.Body);

            var root = JObject.Parse(response.Body);
            return root;
        }

        protected DataSet OnExecuteDataSet(ElasticsearchClientOperation client, ElasticsearchSearch query)
        {
            var root = OnExecute(client, query);
            var parser = Parsers.FirstOrDefault(x => x.CanHandle(root)) ?? throw new ArgumentException();

            var ds = new DataSet();
            var dt = parser.Execute(root);

            ds.Tables.Add(dt);
            dt.AcceptChanges();

            return ds;
        }

        

        public object ExecuteScalar()
        {
            object result = null;
            StartWatch();
            result = OnExecuteScalar(Command.Client, Command.Search);
            StopWatch();
            return result;
        }

        public object OnExecuteScalar(ElasticsearchClientOperation client, ElasticsearchSearch query)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ExecuteList<T>()
        {
            List<T> result = null;
            StartWatch();
            result = OnExecuteList<T>(Command.Client, Command.Search);
            StopWatch();
            return result;
        }

        public List<T> OnExecuteList<T>(ElasticsearchClientOperation client, ElasticsearchSearch query)
        {
            throw new NotImplementedException();
        }

        protected void StartWatch()
        {
            stopWatch.Restart();
        }

        protected void StopWatch()
        {
            stopWatch.Stop();
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Time needed to execute query [Elasticsearch]: {stopWatch.Elapsed:d'.'hh':'mm':'ss'.'fff'ms'}");
        }

        protected internal TimeSpan Elapsed { get => stopWatch.Elapsed; }


    }
}
