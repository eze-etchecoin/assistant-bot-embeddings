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

        private ConcurrentDictionary<int, T> _dict;
        private string _cacheFolderPath = Path.Combine(".", "Cache");

        private static Timer? _timer;

        public CustomMemoryStorage()
        {
            _dict = new();

            LoadCacheFromDisk();
            SetUpTimer();
        }

        private void SetUpTimer()
        {
            Timer = new Timer(PersistCache, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
        }

        public int VectorSize => _vectorSize;

        public static Timer? Timer { get => _timer; set => _timer = value; }

        public int AddVector(T vector)
        {
            var hashCode = CalculateHashCode(vector);

            if (!_dict.ContainsKey(hashCode))
            {
                _dict[hashCode] = vector;
            }

            return hashCode;
        }

        public bool Contains(T vector)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllKeys()
        {
            _dict = new ConcurrentDictionary<int, T>();
        }

        public string? GetDataByKey(int key)
        {
            if (!_dict.TryGetValue(key, out var value))
            {
                return null;
            }

            var result = JsonConvert.SerializeObject(value.Data);

            return result;
        }

        public IEnumerable<int> GetKeys()
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

        public void Set(int key, T value)
        {
            _dict[key] = value;
        }

        public string TestConnection()
        {
            return _dict != null ? "OK" : "No OK";
        }

        private static int CalculateHashCode(T vector) =>
            vector.Values.Aggregate(
                new HashCode(),
                (hash, value) =>
                {
                    hash.Add(value);
                    return hash;
                })
                .ToHashCode();

        private void PersistCache(object? state)
        {
            if(!Directory.Exists(_cacheFolderPath))
            {
                Directory.CreateDirectory(_cacheFolderPath);
            }

            // Folder structure is created taking the first 5 characters of the hash code
            // and creating a folder with that name. The file name is the hash code itself.

            foreach (var item in _dict)
            {
                var hashCode = item.Key.ToString();
                var charsForFolder = hashCode.Length < 5 ? hashCode : hashCode.Substring(0, 5);
                var fileName = $"{hashCode}.json";

                var folderPath = Path.Combine(_cacheFolderPath);
                foreach(var c in charsForFolder)
                {
                    folderPath = Path.Combine(folderPath, c.ToString());
                }

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var filePath = Path.Combine(folderPath, fileName);

                // If file already exists, it is skipped.
                if (File.Exists(filePath))
                {
                    continue;
                }

                var data = JsonConvert.SerializeObject(item);

                File.WriteAllText(filePath, data);
            }
        }

        private void LoadCacheFromDisk()
        {
            if (!Directory.Exists(_cacheFolderPath))
            {
                return;
            }

            var files = Directory.GetFiles(_cacheFolderPath, "*", SearchOption.AllDirectories);

            foreach(var file in files)
            {
                var data = File.ReadAllText(file);
                var item = JsonConvert.DeserializeObject<KeyValuePair<int, T>>(data);

                _dict[item.Key] = item.Value;
            }
        }
    }
}
