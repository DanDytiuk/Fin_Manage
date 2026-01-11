using FinManage.Models;
using Newtonsoft.Json;
namespace FinManage.Services
{
    public class SettingsService
    {
        private const string FileName = "Settings.json";
        public SettingsModel Load()
        {
            if(!File.Exists(FileName)) return new SettingsModel();
            return JsonConvert.DeserializeObject<SettingsModel>(File)
        }
    }
}
