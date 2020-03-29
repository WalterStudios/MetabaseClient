using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace MetabaseClient.Resources
{
    public class Card : MetabaseResource
    {
        public string Endpoint
        {
            get { return _endpoint; }
            private set { _endpoint = value; }
        }

        public Card(HttpClient client, string token) : base(client, token)
        {
            Endpoint = "api/card";
        }
        public string BuildCustomQueryRequestDict(int databaseId, string name, string queryString)
        {
            requestData = new Dictionary<string, object>();
            requestData.Add("name", name);
            requestData.Add("display", "scalar");
            requestData.Add("visualization_settings", new { });
            requestData.Add("dataset_query", new
            {
                database = databaseId,
                type = "native",
                native = new
                {
                    query = queryString,
                    collection = "",
                    template_tags = new { }
                }
            });
            requestData.Add("description", "");
            requestData.Add("collection_id", "");
            requestData.Add("collection_position", "");
            requestData.Add("result_metadata", "");
            requestData.Add("metadata_checksum", "");
            string json = JsonConvert.SerializeObject(requestData, Formatting.Indented);
            return json;
        }

        public string BuildCardParameterDict(Dictionary<string, object> prm)
        {
            requestData = new Dictionary<string, object>();
            requestData.Add("ignore_cache", true);
            List<object> paramList = new List<object>();
            foreach (KeyValuePair<string, object> kvp in prm)
            {
                paramList.Add(new { 
                    type = "category",
                    target = new object[]
                    {
                        "variable", new string[] {"template-tag", kvp.Key}
                    },
                    value = kvp.Value
                });
            }
            object[] param = paramList.ToArray();
            requestData.Add("parameters", param);
            string json = JsonConvert.SerializeObject(requestData, Formatting.Indented);
            return json;
        }

        //public string Get(int? cardId = null)
        //{
        //    string route = Endpoint + cardId;
        //    Execute(clientActions.GET, route).GetAwaiter().GetResult();
        //    return resultContent;
        //}

        public async Task<string> Get(int? cardId = null)
        {
            string route = String.Format("{0}/{1}", Endpoint, cardId);
            await Execute(clientActions.GET, route);
            return resultContent;
        }

        /*
         string collection = null, string description = null, string collectionId = null
         */
         // POST for a custom query
        public async Task<string> Post(int databaseId, string name, string query)
        {
            string requestJson = BuildCustomQueryRequestDict(databaseId, name, query);
            await Execute(clientActions.POST, Endpoint, requestJson);
            return resultContent;
        }


        /*
            Need to add an argument for a parameter dictionary that will be added to the Request Payload.
            Probably needs to be a second method like BuildRequestDict().
            At some level there needs to be knowledge of the query: Either a custom query will be run or
            the names of the parameters must be known.
            ...
            Actually, the Dictionary will have to be built on the UI side anyway. This method should just
            take an optional Dictionary.
         */
        // POST for the query in a card
        public async Task<string> Query(int cardId, Dictionary<string, object> parameters)
        {
            string requestJson = BuildCardParameterDict(parameters);
            string route = String.Format("{0}/{1}/query", Endpoint, cardId);
            await Execute(clientActions.POST, route, requestJson);
            return resultContent;
        }

        public string Download(int cardId, string format)
        {
            /* Can only download to csv and json for now. */
            if (!new string[] { "csv", "json" }.Contains(format))
            {
                throw new ArgumentException(String.Format("Format '{0}' not supported. Must be 'csv' or 'json'", format));
            }
            string route = String.Format("{0}/{1}/query/{2}", Endpoint, cardId, format);
            Execute(clientActions.POST, route).GetAwaiter().GetResult();
            // Result Content needs to be dumped to a file with the corresponding extension.
            return resultContent;
        }

        public string Delete(int cardId)
        {
            string route = String.Format("{0}/{1}", Endpoint, cardId);
            Execute(clientActions.DELETE, route).GetAwaiter().GetResult();
            return resultContent;
        }

        private async Task Execute(clientActions action, string route, string jsonData = "")
        {
            try
            {
                switch (action)
                {
                    case clientActions.GET:
                        response = await httpClient.GetAsync(route);
                        ValidateResponse(action);
                        resultContent = await response.Content.ReadAsStringAsync();
                        break;
                    case clientActions.POST:
                        response = await httpClient.PostAsync(route,
                            new StringContent(jsonData, Encoding.UTF8, "application/json"));
                        ValidateResponse(action);
                        resultContent = await response.Content.ReadAsStringAsync();
                        break;
                    case clientActions.DELETE:
                        response = await httpClient.DeleteAsync(route);
                        ValidateResponse(action);
                        resultContent = await response.Content.ReadAsStringAsync();
                        break;
                    default:
                        // Unknown action/action not enumerated is provided.
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Message from Client: {0}", e.Message);
            }
        }
    }
}
