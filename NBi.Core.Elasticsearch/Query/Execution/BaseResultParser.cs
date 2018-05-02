using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Elasticsearch.Query.Execution
{
    abstract class BaseResultParser
    { 
        public DataTable Execute(JToken root)
        {
            var results = OnExtractObjects(root);

            var dt = new DataTable();
            foreach (JObject result in results)
            {
                var dico = result.ToObject<Dictionary<string, object>>();
                AddObjectToDataTable(dico, ref dt);
            }
            return dt;
        }

        protected void AddObjectToDataTable(IDictionary<string, object> dico, ref DataTable dt)
        {
            var dr = dt.NewRow();
            foreach (var attribute in dico.Keys)
            {
                if (!dt.Columns.Contains(attribute))
                    dt.Columns.Add(new DataColumn(attribute, typeof(object)) { DefaultValue = DBNull.Value });
                if (OnComplexParsing(dico[attribute], out var dataField))
                    dr[attribute] = dataField;
                else
                    dr[attribute] = dico[attribute];

            }
            dt.Rows.Add(dr);
        }

        protected virtual bool OnComplexParsing(object jsonField, out object dataField)
        {
            dataField = null;
            return false;
        }

        protected abstract IEnumerable<JToken> OnExtractObjects(JToken root);
    }
}
