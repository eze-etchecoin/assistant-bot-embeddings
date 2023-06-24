using AssistantBot.Common.Interfaces;
using AssistantBot.Configuration;
using AssistantBot.Common.Helpers;
using RestSharp;
using AssistantBot.Common.DataTypes;

namespace AssistantBot.Services.Integrations
{
    public class CustomMemoryStorageService<T> : IIndexedVectorStorage<T> where T : IVectorWithObject
    {
        private readonly RestClient _client;
        private const int _vectorSize = 1536;

        public CustomMemoryStorageService(AssistantBotConfiguration config)
        {
            _client = new RestClient(config.CustomCacheUrl);
        }

        public int VectorSize => _vectorSize;

        public string AddVector(T vector, string? keyComplementStr = null)
        {
            var restSharpHelper = new RestSharpJsonHelper<AddVectorRequest, string>(_client);
            var response = restSharpHelper.ExecuteRequestAsync(
                url: "/AddVector", 
                method: Method.Post, 
                body: new AddVectorRequest
                {
                    Vector = vector as EmbeddedTextVector,
                    KeyComplementStr = keyComplementStr
                }).Result;

            return response;
        }

        public bool Contains(T vector)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllKeys()
        {
            var restSharpHelper = new RestSharpJsonHelper<object, object>(_client);
            _ = restSharpHelper.ExecuteRequestAsync("/DeleteAllKeys", Method.Delete);
        }

        public string? GetDataByKey(string key)
        {
            var restSharpHelper = new RestSharpJsonHelper<object, string>(_client);
            var response = restSharpHelper.ExecuteRequestAsync($"/GetDataByKey?key={key}", Method.Get).Result;

            return response;
        }

        public IEnumerable<string> GetKeys()
        {
            var restSharpHelper = new RestSharpJsonHelper<object, IEnumerable<string>>(_client);
            var response = restSharpHelper.ExecuteRequestAsync("/GetKeys", Method.Get).Result;
            return response;
        }

        public void RemoveVector(T vector)
        {
            throw new NotImplementedException();
        }

        public IList<TResult> SearchDataBySimilarVector<TResult>(T vectorToSearch, int numResults = 1)
        {
            var restSharpHelper = new RestSharpJsonHelper<T,IList<TResult>>(_client);
            var response = restSharpHelper
                .ExecuteRequestAsync($"/SearchDataBySimilarVector?numResults={numResults}", Method.Post, vectorToSearch)
                .Result;

            return response;
        }

        public IEnumerable<T> SearchForValues(T valueToSearch, int numberOfResults = 10)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, T value)
        {
            throw new NotImplementedException();
        }

        public string? TestConnection()
        {
            var restSharpHelper = new RestSharpJsonHelper<object, string>(_client);
            var response = restSharpHelper.ExecuteRequestAsync("/Check", Method.Get).Result;
            return response;
        }
    }
}
