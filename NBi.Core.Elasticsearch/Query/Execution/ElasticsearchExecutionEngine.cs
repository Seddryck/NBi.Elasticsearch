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

        protected internal ElasticsearchExecutionEngine(ElasticsearchClientOperation client, ElasticsearchCommandOperation command)
        {
            Client = client;
            Command = command;
        }

        public DataSet Execute()
        {
            DataSet ds = null;
            StartWatch();
            ds = OnExecuteDataSet(Command.Client, Command.Search);
            StopWatch();
            return ds;
        }

        protected dynamic OnExecute(ElasticsearchClientOperation client, ElasticsearchSearch query)
        {
            var response = client.Execute(query);

            if (!response.Success)
                throw new Exception(response.Body);

            var result = JObject.Parse(response.Body);
            if (result.TryGetValue("aggregations", out JToken root))
                return root.SelectTokens("*.buckets").Values().ToArray();
            else if (result.TryGetValue("hits", out root))
                return root.SelectTokens("hits[*]._source").ToArray();
            else
                throw new ArgumentOutOfRangeException();
        }

        protected DataSet OnExecuteDataSet(ElasticsearchClientOperation client, ElasticsearchSearch query)
        {
            var ds = new DataSet();
            var dt = new DataTable();
            ds.Tables.Add(dt);

            var results = OnExecute(client, query);

            foreach (JObject result in results)
            {
                var dico = result.ToObject<Dictionary<string, object>>();
                AddObjectToDataTable(dico, ref dt);
            }
            dt.AcceptChanges();

            return ds;
        }

        private void AddObjectToDataTable(IDictionary<string, object> dico, ref DataTable dt)
        {
            var dr = dt.NewRow();
            foreach (var attribute in dico.Keys)
            {
                if (!dt.Columns.Contains(attribute))
                    dt.Columns.Add(new DataColumn(attribute, typeof(object)) { DefaultValue = DBNull.Value });
                // If the result has a single child named 'value' then we should parse it directly
                if (dico[attribute] is JObject 
                    && (dico[attribute] as JObject).Children<JProperty>().Count() == 1 
                    && (dico[attribute] as JObject).SelectToken("value")!=null)
                    dr[attribute] = (dico[attribute] as JObject).SelectToken("value").ToObject<object>(); 
                else
                    dr[attribute] = dico[attribute];

            }
            dt.Rows.Add(dr);
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
            var ds = new DataSet();
            var dt = new DataTable();
            ds.Tables.Add(dt);

            var result = OnExecute(client, query);
            if (result!=null && result.Count>0)
                return result.First();
            return null;
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
            var list = new List<T>();

            var results = OnExecute(client, query);

            foreach (var result in results)
                list.Add(result);
            
            return list;
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
