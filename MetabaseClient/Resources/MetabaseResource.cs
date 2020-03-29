using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using MetabaseClient.Exceptions;

namespace MetabaseClient.Resources
{
    public class MetabaseResource
    {
        public string _token;
        public string _endpoint;
        public HttpClient httpClient;
        public enum clientActions { GET, POST, PUT, DELETE };
        public HttpResponseMessage response;
        public string resultContent;
        // Request will be serialized and passed into the http request.
        public Dictionary<string, object> requestData = new Dictionary<string, object>();
        public MetabaseResource(HttpClient client, string token)
        {
            //_baseUrl = URL;
            httpClient = client;
            _token = token;
        }

        public void ValidateResponse(clientActions action)
        {
            switch (action)
            {
                case clientActions.GET:
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new RequestException(response.ReasonPhrase);
                    }
                    break;
                case clientActions.POST:
                    if ((response.StatusCode != System.Net.HttpStatusCode.OK) &&
                        (response.StatusCode != System.Net.HttpStatusCode.Created))
                    {
                        throw new RequestException(response.ReasonPhrase);
                    }
                    break;
                case clientActions.PUT:
                    if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
                    {
                        throw new RequestException(response.ReasonPhrase);
                    }
                    break;
                case clientActions.DELETE:
                    if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
                    {
                        throw new RequestException(response.ReasonPhrase);
                    }
                    break;
            }
        }
    }
}
