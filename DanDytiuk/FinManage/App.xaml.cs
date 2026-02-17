using FinManage.Models;
using FinManage.Services;
using FinManage.ViewModels;
using SQLitePCL;
using System.IO;
using System.Text.Json;
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
            base.OnStartup(e);

            //var mainSettings = new SettingsWindowsViewModel();

            //var settings = mainSettings.Settings;

            if (File.Exists("settings.json"))
            {
                var json = File.ReadAllText("settings.json");
                var settings = JsonSerializer.Deserialize<SettingsModel>(json);

                LocalizationHelper.Instance.ChangeLang(settings.Language);
            }

            Batteries.Init();
        }
    }
}
