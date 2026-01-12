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
        public ObservableCollection<TypeOperation> Operation { get; }
        #endregion

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action CloseAction { get; set; }

        public SettingsWindowsViewModel()
        {
            Themes = new ObservableCollection<Themes>((Themes[])Enum.GetValues(typeof(Themes)));
            Currency = new ObservableCollection<TypesOfCurrency>((TypesOfCurrency[])Enum.GetValues(typeof(TypesOfCurrency)));
            Operation = new ObservableCollection<TypeOperation>((TypeOperation[])Enum.GetValues(typeof(TypeOperation)));

            _settingsService = new SettingsService();
            Settings = _settingsService.Load();

            SaveCommand = new LambdaCommand(Save);
            CancelCommand = new LambdaCommand(Cancel);
        }

        private void Save(object parameter)
        {
            _settingsService.Save(Settings);
            CloseAction?.Invoke();
        }

        private void Cancel(object parameter)
        {
            CloseAction?.Invoke();
        }
    }
}
