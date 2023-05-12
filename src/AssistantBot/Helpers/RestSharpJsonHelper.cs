﻿using AssistantBot.Exceptions;
using Newtonsoft.Json;
using RestSharp;

namespace AssistantBot.Helpers
{
    public class RestSharpJsonHelper<TBody, TResponse>
    {
        private readonly IRestClient _client;

        public RestSharpJsonHelper(IRestClient client)
        {
            _client = client;
        }

        public async Task<TResponse> ExecuteRequestAsync(
            string url, 
            Method method, 
            TBody? body = default, 
            IEnumerable<(string, string)>? headers = null)
        {
            // Request is built here, pointing to OpenAI corresponding endpoint
            var request = RestSharpJsonHelper<TBody, TResponse>.GetRequest("/v1/chat/completions", Method.Post, headers);

            // Content type application/json is built here
            if (body != null)
                request.AddJsonBody(JsonConvert.SerializeObject(body));

            // Request is executed, and a response must be received
            var response = await _client.ExecuteAsync(request);

            return GetResponse(response);
        }

        //public async Task<TResponse> ExecuteRequestAsync<TResponse>(
        //    string url,
        //    Method method,
        //    IEnumerable<(string, string)>? headers = null)
        //{
        //    return await ExecuteRequestAsync<object, TResponse>(url, method, headers);
        //}

        private static RestRequest GetRequest(string url, Method method, IEnumerable<(string, string)>? headers)
        {
            var request = new RestRequest(url, method);

            if(headers != null) 
            {
                foreach( var header in headers)
                {
                    request.AddHeader(header.Item1, header.Item2);
                }
            }

            return request;
        }

        private static TResponse GetResponse(RestResponse restResponse)
        {
            if (!restResponse.IsSuccessful)
                throw new AssistantBotException(restResponse.ErrorMessage ?? restResponse.Content);

            // The response content is deserialized (it comes in JSON format)
            var response = JsonConvert.DeserializeObject<TResponse>(restResponse.Content);

            return response ?? Activator.CreateInstance<TResponse>();
        }
    }
}
