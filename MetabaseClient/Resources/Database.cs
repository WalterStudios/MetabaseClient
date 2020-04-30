using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace MetabaseClient.Resources
{
    public class Database : MetabaseResource
    {
        //public string resultContent;
        public string Endpoint
        {
            get { return _endpoint; }
            private set { _endpoint = value; }
        }

        public Database(HttpClient client, string token) : base(client, token)
        {
            Endpoint = "api/database";
        }

        public async Task<string> Get(int? databaseId = null)
        {
            string route = String.Format("{0}/{1}", Endpoint, databaseId);
            await Execute(clientActions.GET, route);
            return resultContent;
        }

        public async Task<string> GetByName(string dbName)
        {
            var getAllResult = await Get();
            JArray jsonResult = JArray.Parse(getAllResult) as JArray;
            dynamic dbs = jsonResult;
            foreach (dynamic db in dbs)
            {
                if (db.name == dbName)
                {
                    return db.ToString();
                }
            }
            return null;
        }

        private async Task Execute(clientActions action, string route)
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
