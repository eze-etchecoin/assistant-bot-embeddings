using AssistantBot.Services.Interfaces;
using System.Collections.Generic;

namespace QAAssistantBot.Services.RedisStorage
{
    public class RedisStorageService : IMemoryStorage
    {
        public object Get(string key)
        {
            throw new System.NotImplementedException();
        }

        public void Set(string key, object value)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<object> SearchForValues(object valueToSearch, int numberOfResults = 10)
        {
            throw new System.NotImplementedException();
        }
    }
}
