using AssistantBot.Services.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Diagnostics;
using System.Globalization;

namespace AssistantBot.Services.RedisStorage
{
    public class RedisVectorStorageService<T> : IIndexedVectorStorage<T> where T : IVectorWithObject
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;
        private readonly IServer _server;

        private const string _indexName = "textVectorIndex";
        private const int _indexVectorSize = 1536;
        private const string _textField = "text";
        private const string _embeddingField = "embedding";

        public RedisVectorStorageService(string redisUrl)
        {
            _redis = ConnectionMultiplexer.Connect(redisUrl);
            _db = _redis.GetDatabase();
            _server = _redis.GetServer(redisUrl);

            CreateIndex();
        }

        public int VectorSize => _indexVectorSize;

        public string? GetDataByKey(string key)
        {
            var value = _db.HashGet(key, _textField);
            if (value.HasValue)
                return value;
            else
                return null;
        }

        public void Set(string key, object value)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<object> SearchForValues(object valueToSearch, int numberOfResults = 10)
        {
            throw new System.NotImplementedException();
        }

        public string AddVector(T vector)
        {
            var vectorString = string.Join(",", vector.Values.Select(x => x.ToString(CultureInfo.InvariantCulture)));
            var jsonData = JsonConvert.SerializeObject(vector.Data);
            var stringGuid = Guid.NewGuid().ToString();

            _db.Execute("HSET", stringGuid, _textField, jsonData, _embeddingField, vectorString);
            //_db.Execute("FT.ADD", _indexName, stringGuid, "1.0", "FIELDS", _textField, jsonData, _embeddingField, vectorString);

            return stringGuid;
        }

        public void RemoveVector(T vector)
        {
            throw new NotImplementedException();
        }

        public bool Contains(T vector)
        {
            throw new NotImplementedException();
        }

        public IList<object> SearchDataBySimilarVector(T vector, int numResults = 1)
        {
            var result = new List<object>();
            
            if (numResults < 1)
                return result;
            
            string queryVectorString = string.Join(
                ",", 
                vector.Values.Select(x => x.ToString(CultureInfo.InvariantCulture)));

            var queryResult = _db.Execute(
                "FT.SEARCH", _indexName, 
                $"\"*=>[KNN {numResults} {queryVectorString}\"]"
                /*"DIALECT 2"*/);

            var searchResults = (RedisValue[]?)queryResult;
            
            if ((long)searchResults[0] <= 0)
                return result;

            for (int i = 1; i < searchResults.Length; i += 2)
            {
                string docId = (string)searchResults[i];
                var data = JsonConvert.DeserializeObject(searchResults[i + 1]);
                result.Add(data);

                //Console.WriteLine($"Document ID: {docId}, Text: {text}");
            }

            return result;
        }

        private void CreateIndex()
        {
            if (!IndexExists(_indexName))
            {
                _server.Execute(
                    "FT.CREATE", 
                    _indexName, "ON", "HASH", "SCHEMA", 
                    _textField, "TEXT", 
                    _embeddingField, "VECTOR", 
                    "HNSW", "6", 
                    "TYPE", "FLOAT32", 
                    "DIM", _indexVectorSize, 
                    "DISTANCE_METRIC", "L2");
            }
        }

        private bool IndexExists(string indexName)
        {
            try
            {
                _server.Execute("FT.INFO", indexName);
                return true;
            }
            catch (RedisServerException ex)
            {
                if (ex.Message.StartsWith("Unknown Index name"))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public string TestConnection()
        {
            var stopwatch = Stopwatch.StartNew();
            
            // Execute the PING command.
            string result = _server.Ping().ToString();

            stopwatch.Stop();

            return $"PING result: {result} | Response time: {stopwatch.ElapsedMilliseconds}ms";
        }

        public IEnumerable<string> GetKeys()
        {
            return _server.Keys().Select(x => x.ToString());
        }
    }
}
