using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Data;

namespace MetabaseClient.Helpers
{
    /*
     Loader just returns a .NET DataTable.
     */
    public static class TableResponseLoader
    {
        // Metabase seems to return everything as a string anyway. Can worry about casting later if needed.
        //private static Dictionary<string, Type> ColDataTypes
        //public static void Load(string jsonResponse, out TableResponse tableResponse)
        //{
        //    JObject data = JObject.Parse(jsonResponse);
        //    // Load initial data
        //    tableResponse = new TableResponse
        //    {
        //        DatabaseId = data["database_id"].ToObject<int>(),
        //        Status = data["status"].ToString(),
        //        Context = data["context"].ToString(),
        //        Rows = new List<object[]>(),
        //        Columns = new List<string>()
        //    };
        //    // Load rows
        //    IJEnumerable<JToken> dataRows = data["data"]["rows"].Children();//.AsJEnumerable();
        //    foreach (JToken r in dataRows)
        //    {
        //        tableResponse.Rows.Add(r.ToObject<object[]>());
        //    }
        //    // Load columns
        //    IJEnumerable<JToken> dataCols = data["data"]["cols"].Children();
        //    foreach(JToken c in dataCols)
        //    {
        //        tableResponse.Columns.Add(c["name"].ToString());
        //    }
        //    //tableResponse.Columns = colNames.ToArray();
        //}

        public static DataTable LoadTable(string jsonResponse)
        {
            DataTable table = new DataTable();
            JObject data = JObject.Parse(jsonResponse);

            // Load columns
            IJEnumerable<JToken> dataCols = data["data"]["cols"].Children();
            foreach (JToken c in dataCols)
            {
                table.Columns.Add(c["name"].ToString());
            }
            // Load rows
            IJEnumerable<JToken> dataRows = data["data"]["rows"].Children();
            foreach (JToken r in dataRows)
            {
                table.Rows.Add(r.ToObject<object[]>());
            }
            return table;
        }
    }
}
