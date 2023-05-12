using AssistantBot.Common.Interfaces;
using AssistantBot.Configuration;
using AssistantBot.Helpers;
using RestSharp;

namespace AssistantBot.Services.Integrations
{
    public class CustomMemoryStorageService<T> : IIndexedVectorStorage<T> where T : IVectorWithObject
    {
        private readonly RestClient _client;

        public CustomMemoryStorageService(AssistantBotConfiguration config)
        {
            _client = new RestClient(config.CustomCacheUrl);
        }

        public int VectorSize => throw new NotImplementedException();

        public string AddVector(T vector)
        {
            var restSharpHelper = new RestSharpJsonHelper<T, string>(_client);
            var response = restSharpHelper.ExecuteRequestAsync("/AddVector", Method.Post, vector).Result;
            return response;
        }

        public bool Contains(T vector)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllKeys()
        {
            throw new NotImplementedException();
        }

        public string? GetDataByKey(string key)
        {
            throw new NotImplementedException();
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

        public string TestConnection()
        {
            var restSharpHelper = new RestSharpJsonHelper<object, string>(_client);
            var response = restSharpHelper.ExecuteRequestAsync("/Check", Method.Get).Result;
            return response;
        }
    }
}
