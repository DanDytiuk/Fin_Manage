using FinManage.Models;
using Newtonsoft.Json;
using System.IO;

namespace FinManage.Services
{
    public class SettingsService
    {
        private const string FileName = "Settings.json";
        internal SettingsModel Load()
        {
            if(!File.Exists(FileName)) return new SettingsModel();
            return JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(FileName));
        }

        internal void Save(SettingsModel settings) 
        { 
            File.WriteAllText(FileName, JsonConvert.SerializeObject(settings, Formatting.Indented));
        }
    }
}
