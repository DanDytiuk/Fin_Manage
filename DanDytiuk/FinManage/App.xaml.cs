using FinManage.ViewModels;
using SQLitePCL;
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

            var mainSettings = new SettingsWindowsViewModel();

            var settings = mainSettings.Settings;

            Batteries.Init();
        }
    }
}
