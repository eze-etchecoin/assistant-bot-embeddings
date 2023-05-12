using AssistantBot.Common.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Diagnostics;
using System.Text;

namespace AssistantBot.Services.RedisStorage
{
    public class RedisVectorStorageService<T> : IIndexedVectorStorage<T> where T : IVectorWithObject
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;
        private readonly IServer _server;

        private const string _indexName = "textVectorIndex";
        private const int _vectorSize = 1536;
        private const int _indexVectorSize = _vectorSize * sizeof(double);
        private const string _textField = "text";
        private const string _embeddingField = "embedding";

        public RedisVectorStorageService(string redisUrl)
        {
            _redis = ConnectionMultiplexer.Connect(redisUrl);
            _db = _redis.GetDatabase();
            _server = _redis.GetServer(redisUrl);

            CreateIndex();
        }

        public int VectorSize => _vectorSize;

        public string? GetDataByKey(string key)
        {
            var value = _db.HashGet(key, _textField);
            if (value.HasValue)
                return value;
            else
                return null;
        }

        public void Set(string key, T value)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> SearchForValues(T valueToSearch, int numberOfResults = 10)
        {
            throw new System.NotImplementedException();
        }

        public string AddVector(T vector)
        {
            var vectorString = GetVectorHexRepresentation(vector);
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

        public IList<TResult> SearchDataBySimilarVector<TResult>(T vector, int numResults = 1)
        {
            var result = new List<TResult>();
            
            if (numResults < 1)
                return result;

            var queryArgs = new string[]
            {
                _indexName,
                $"*=>[KNN {numResults} @{_embeddingField} $BLOB]",
                "PARAMS", "2",
                "BLOB", $"{GetVectorHexRepresentation(vector)}",
                "DIALECT", "2"
            };
                
            var queryResult = _db.Execute("FT.SEARCH", queryArgs);

            var searchResults = ((RedisResult[]?)queryResult) ?? Array.Empty<RedisResult>();

            if ((long)searchResults[0] <= 0)
                return result;

            for (int i = 1; i < searchResults.Length; i += 2)
            {
                var docId = (string?)searchResults[i];

                var values = (RedisValue[]?)searchResults[i + 1];

                var indexOfJsonDataField = Array.IndexOf(
                    values.Select(x => x.ToString()).ToArray(), 
                    _textField);

                var data = JsonConvert.DeserializeObject<TResult>(values[indexOfJsonDataField + 1]);

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

        public void DeleteAllKeys()
        {
            _ = _db.Execute("FLUSHDB");
        }

        private string GetVectorHexRepresentation(T vector)
        {
            byte[] bytes = new byte[_vectorSize * sizeof(double)];

            Buffer.BlockCopy(vector.Values, 0, bytes, 0, bytes.Length);

            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append("\\x" + b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
