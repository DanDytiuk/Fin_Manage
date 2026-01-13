using FinManage.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FinManage.Services
{
    public class SettingsService
    {
        private readonly string _filePath;

        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true,
        };
        public SettingsService() 
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string appFolder = Path.Combine(appData, "FinManage");

            Directory.CreateDirectory(appFolder);

            _filePath = Path.Combine(appFolder, "settings.json");
            _options.Converters.Add(new JsonStringEnumConverter());
        }
        internal SettingsModel Load()
        {
            if (!File.Exists(_filePath)) return new SettingsModel();

            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<SettingsModel>(json, _options) ?? new SettingsModel();
        }

        internal void Save(SettingsModel settings)
        { 
            string json = JsonSerializer.Serialize(settings, _options);
            File.WriteAllText(_filePath, json);
        }
    }
}
