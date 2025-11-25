using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Library.DAL
{
    public interface IStorageService<T>
    {
        List<T> Load();
        void Save(List<T> items);
    }

    public class FileStorageService<T> : IStorageService<T>
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _options;

        public FileStorageService(string fileName)
        {
            _filePath = fileName;
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }

        public List<T> Load()
        {
            if (!File.Exists(_filePath)) return new List<T>();
            var json = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json)) return new List<T>();
            return JsonSerializer.Deserialize<List<T>>(json, _options) ?? new List<T>();
        }

        public void Save(List<T> items)
        {
            var json = JsonSerializer.Serialize(items, _options);
            File.WriteAllText(_filePath, json);
        }
    }
}