using FinManage.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using static FinManage.Infrastructure.EnumInfrastructure;

namespace FinManage.Services
{
    public class LimitService
    {
        private readonly string json_file_name;

        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        public LimitService()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = Path.Combine(appData, "FinManage");
            json_file_name = Path.Combine(appFolder, "limit.json");
            _options.Converters.Add(new JsonStringEnumConverter());
        }

        /*internal LimitModel Load()
        {
            if (!File.Exists(json_file_name)) return new LimitModel();

            string json = File.ReadAllText(json_file_name);
            return JsonSerializer.Deserialize<LimitModel>(json, _options) ?? new LimitModel();
        }*/
        internal Dictionary<Category, decimal> Load()
        {
            if (!File.Exists("limits.json")) return new Dictionary<Category, decimal>();
            var json = File.ReadAllText("limits.json");
            return JsonSerializer.Deserialize<Dictionary<Category, decimal>>(json, _options) ?? new Dictionary<Category, decimal>();
        }
        internal LimitModel Save(LimitModel limitModel)
        {
            string json = JsonSerializer.Serialize(limitModel, _options);
            File.WriteAllText(json_file_name, json);
            return limitModel;
        }
    }
}
