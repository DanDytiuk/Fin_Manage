using FinManage.Models;
using FinManage.Services;
using SQLitePCL;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace FinManage
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ApplySaveLang();
            base.OnStartup(e);

            Batteries.Init();
        }
        private void ApplySaveLang()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = Path.Combine(appData, "FinManage");
            string filePath = Path.Combine(appFolder, "settings.json");

            if (!File.Exists(filePath))
                return;

            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());

            var json = File.ReadAllText(filePath);
            var settings = JsonSerializer.Deserialize<SettingsModel>(json, options);

            if (!string.IsNullOrWhiteSpace(settings?.Language))
            {
                LocalizationHelper.Instance.SetLanguage(settings.Language);
            }
        }
    }
}
