using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using MetabaseClient.Exceptions;
using MetabaseClient.Resources;

namespace MetabaseClient
{
    public class Client
    {
        // The metabase site
        private string _baseUrl;

        // Credentials
        private string _username;
        private string _password;

        private bool _authenticated;

        // The token id that is returned after authentication
        private string clientToken;

        private HttpClient httpClient = new HttpClient();

        public string URL
        {
            get { return _baseUrl; }
            set
            {
                _baseUrl = value;
                _authenticated = false;
            }
        }
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                _authenticated = false;
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                _authenticated = false;
            }
        }
        public bool IsAuthenticated
        {
            get { return _authenticated; }
        }


        /* Resource Properties */
        public Database Databases
        {
            get { return new Database(httpClient, clientToken); }
        }
        public Card Cards
        {
            get { return new Card(httpClient, clientToken); }
        }
        public Dataset Datasets
        {
            get { return new Dataset(httpClient, clientToken); }
        }



        /* Always require a URL */
        //public Client() { }
        public Client(string url)
        {
            URL = url;
            httpClient.BaseAddress = new Uri(_baseUrl);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public bool Login()
        {
            Authenticate().GetAwaiter().GetResult();
            SetRequestHeader();
            return _authenticated;
        }

        /* Improve failed authentication handling */
        private async Task Authenticate()
        {

            string authContent = $"{{\"username\": \"{_username}\",\"password\": \"{_password}\"}}";
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync("api/session",
                    new StringContent(authContent, Encoding.UTF8, "application/json"));
                string resultContent = await response.Content.ReadAsStringAsync();


                // Consider making a class for the AuthResponse
                // Possibly use <string>.Contains() to see if keys are in json string.
                // JObject.Parse(resultContent).ContainsKey()
                if (JObject.Parse(resultContent).ContainsKey("errors"))
                {
                    _authenticated = false;
                    //_errorMessage = JObject.Parse(resultContent)["errors"].ToString();
                    throw new AuthenticationException("Username or Password is incorrect.");
                }
                if (JObject.Parse(resultContent).ContainsKey("id"))
                {
                    clientToken = JObject.Parse(resultContent)["id"].ToString();
                    _authenticated = true;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error Message from Client: {0}", e.Message);
            }
        }

        /* Set header for future requests by resources */
        public void SetRequestHeader()
        {
            //string header = $"{{\"X-Metabase-Session\": \"{clientToken}\"}}";
            httpClient.DefaultRequestHeaders.Add("X-Metabase-Session", clientToken);
        }
    }
}
