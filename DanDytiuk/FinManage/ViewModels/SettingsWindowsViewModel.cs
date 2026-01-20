using FinManage.Infrastructure.Commands;
using FinManage.Models;
using FinManage.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Input;
using static FinManage.Infrastructure.EnumInfrastructure;

namespace FinManage.ViewModels
{
    internal class SettingsWindowsViewModel : BaseViewModel
    {
        private readonly string FilePath;

        private readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            WriteIndented = true,
        };
        public SettingsModel Settings { get; }

        #region Заполнение ComboBox
        public ObservableCollection<Themes> Themes { get; }
        public ObservableCollection<TypesOfCurrency> Currency { get; }
        public ObservableCollection<Category> Category { get; }
        #endregion

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action CloseAction { get; set; }
        private bool CanSaveCommandExecuted(object parameter) => true;
        private bool CanCancelCommandExecuted(object parameter) => true;
        private SettingsModel LoadSettings()
        {
            if (!File.Exists(FilePath)) return new SettingsModel();

            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<SettingsModel>(json, Options) ?? new SettingsModel();
        }

        private void SaveSettings(SettingsModel settings)
        {
            string json = JsonSerializer.Serialize(settings, Options);
            File.WriteAllText(FilePath, json);
        }
        private void SaveFromAppExecute(object parameter)
        {
            SaveSettings(Settings);
            CloseAction?.Invoke();
        }

        private void CancelFromAppExecute(object parameter)
        {
            CloseAction?.Invoke();
        }

        public SettingsWindowsViewModel()
        {
            Themes = new ObservableCollection<Themes>((Themes[])Enum.GetValues(typeof(Themes)));
            Currency = new ObservableCollection<TypesOfCurrency>((TypesOfCurrency[])Enum.GetValues(typeof(TypesOfCurrency)));
            Category = new ObservableCollection<Category>((Category[])Enum.GetValues(typeof(Category)));
            
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string appFolder = Path.Combine(appData, "FinManage");

            Directory.CreateDirectory(appFolder);

            FilePath = Path.Combine(appFolder, "settings.json");
            Options.Converters.Add(new JsonStringEnumConverter());

            Settings = LoadSettings();

            SaveCommand = new LambdaCommand(SaveFromAppExecute, CanSaveCommandExecuted);
            CancelCommand = new LambdaCommand(CancelFromAppExecute, CanCancelCommandExecuted);
        }
    }
}
