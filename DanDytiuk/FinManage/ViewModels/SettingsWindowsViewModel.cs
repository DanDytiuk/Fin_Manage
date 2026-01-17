using FinManage.Infrastructure.Commands;
using FinManage.Models;
using FinManage.Services;
using FinManage.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static FinManage.Infrastructure.EnumInfrastructure;

namespace FinManage.ViewModels
{
    internal class SettingsWindowsViewModel : BaseViewModel
    {
        private readonly SettingsService _settingsService;
        public SettingsModel Settings { get; }

        #region Заполнение ComboBox
        public ObservableCollection<Themes> Themes { get; }
        public ObservableCollection<TypesOfCurrency> Currency { get; }
        public ObservableCollection<Category> Category { get; }
        #endregion

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action CloseAction { get; set; }
        private bool CanSaveCommand(object parameter) => true;
        private bool CanCancelCommand(object parameter) => true;
        private void Save(object parameter)
        {
            _settingsService.Save(Settings);
            CloseAction?.Invoke();
        }

        private void Cancel(object parameter)
        {
            CloseAction?.Invoke();
        }
        public SettingsWindowsViewModel()
        {
            Themes = new ObservableCollection<Themes>((Themes[])Enum.GetValues(typeof(Themes)));
            Currency = new ObservableCollection<TypesOfCurrency>((TypesOfCurrency[])Enum.GetValues(typeof(TypesOfCurrency)));
            Category = new ObservableCollection<Category>((Category[])Enum.GetValues(typeof(Category)));

            _settingsService = new SettingsService();
            Settings = _settingsService.Load();

            SaveCommand = new LambdaCommand(Save, CanSaveCommand);
            CancelCommand = new LambdaCommand(Cancel, CanCancelCommand);
        }
    }
}
