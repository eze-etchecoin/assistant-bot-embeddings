using AssistantBot.Common.Interfaces;
using Newtonsoft.Json;

namespace AssistantBot.Services.Cache
{
    public class InDiskCache<T> : IDiskPersistor<T> where T : class
    {
        private readonly string _path;

        public InDiskCache(string path)
        {
            _path = path;
        }

        public async Task<T> LoadAsync()
        {
            var directoryName = Path.GetDirectoryName(_path);
            if (Directory.Exists(directoryName) == false)
                Directory.CreateDirectory(directoryName);
            
            if (!File.Exists(_path))
                return Activator.CreateInstance<T>();

            var data = await File.ReadAllTextAsync(_path);
            return JsonConvert.DeserializeObject<T>(data) ?? Activator.CreateInstance<T>();
        }

        public async Task SaveAsync(T obj)
        {
            var data = JsonConvert.SerializeObject(obj);
            await File.WriteAllTextAsync(_path, data);
        }
    }
}
