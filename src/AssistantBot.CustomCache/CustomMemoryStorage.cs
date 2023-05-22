using AssistantBot.Common.Interfaces;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace AssistantBot.CustomCache
{
    public class CustomMemoryStorage<T> : IIndexedVectorStorage<T> where T : IVectorWithObject
    {
        private const int _vectorSize = 1536;
        private readonly ConcurrentDictionary<string, T> _dict;

        public CustomMemoryStorage()
        {
            _dict = new();
        }

        public int VectorSize => _vectorSize;

        public string AddVector(T vector)
        {
            var newGuid = Guid.NewGuid().ToString();
            _dict[newGuid] = vector;
            return newGuid;
        }

        public bool Contains(T vector)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllKeys()
        {
            _dict.Clear();
        }

        public string? GetDataByKey(string key)
        {
            if (!_dict.TryGetValue(key, out var value))
            {
                return null;
            }

            var result = JsonConvert.SerializeObject(value.Data);

            return result;
        }

        public IEnumerable<string> GetKeys()
        {
            return _dict.Keys;
        }

        public void RemoveVector(T vector)
        {
            throw new NotImplementedException();
        }

        public IList<TResult> SearchDataBySimilarVector<TResult>(T vectorToSearch, int numResults = 1)
        {
            var vector1 = Vector<double>.Build.Dense(vectorToSearch.Values);
            
            var vectorsWithScore = _dict
                .Select(x => new 
                { 
                    Score = 1 - Distance.Euclidean(vector1, Vector<double>.Build.Dense(x.Value.Values)),
                    Vector = x.Value
                })
                .ToList();

            var orderedData = vectorsWithScore
                .OrderByDescending(x => x.Score)
                .Take(numResults)
                .ToList();

            return orderedData.Select(x => (TResult)x.Vector.Data).ToList();
        }

        public IEnumerable<T> SearchForValues(T valueToSearch, int numberOfResults = 10)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, T value)
        {
            _dict[key] = value;
        }

        public string TestConnection()
        {
            return _dict != null ? "OK" : "No OK";
        }
    }
}
