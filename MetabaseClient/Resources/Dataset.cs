using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace MetabaseClient.Resources
{
    public class Dataset : MetabaseResource
    {
        public string Endpoint
        {
            get { return _endpoint; }
            private set { _endpoint = value; }
        }

        public Dataset(HttpClient client, string token) : base(client, token)
        {
            Endpoint = "api/dataset";
        }

        public string BuildRequestDict(int databaseId, string queryString)
        {
            requestData = new Dictionary<string, object>();
            requestData.Add("native", new
            {
                query = queryString,
                template_tags = new { }
            });
            requestData.Add("type", "native");
            requestData.Add("database", databaseId);
            requestData.Add("parameters", new object[] { });
            string json = JsonConvert.SerializeObject(requestData, Formatting.Indented);
            return json;
        }

        public async Task<string> Post(int databaseId, string query)
        {
            string requestJson = BuildRequestDict(databaseId, query);
            await Execute(clientActions.POST, Endpoint, requestJson);
            return resultContent;
        }

        private async Task Execute(clientActions action, string route, string jsonData = "")
        {
            try
            {
                switch (action)
                {
                    case clientActions.POST:
                        response = await httpClient.PostAsync(route,
                            new StringContent(jsonData, Encoding.UTF8, "application/json"));
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
