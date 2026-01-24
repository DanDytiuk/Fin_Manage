using FinManage.Infrastructure.Commands;
using FinManage.Services;
using FinManage.View.Windows;
using FinManage.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using static FinManage.Infrastructure.EnumInfrastructure;

namespace FinManage.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        #region FinDataGrid

        #region Commands Unit
        public ICommand AddFinDataInfoCommand { get; }
        public ICommand DeleteFinDataInfoCommand { get; }
        public ICommand LoadFromDBCommand { get; }
        public ICommand SaveToDBCommand { get; }
        public ICommand DeleteFromDBCommand { get; }
        private bool CanAddFinDataInfoCommandExecute(object p) => true;
        private bool CanDeleteDataInfoCommandExecute(object p) => true;
        #endregion

        #region Data

        private readonly DataBaseWork _database;

        #endregion

        #region ComboBox

        public ObservableCollection<TypesOfCurrency> CurrencyCB {  get; }
        public ObservableCollection<string> Categories { get; }
        public ObservableCollection<string> CategoriesCB { get; } =
           new ObservableCollection<string>
           {
                "Food",
                "Store",
                "Entertainment",
                "Online store",
                "Games",
                "Public Utilities",
                "Phone Top Up",
                "Card Top Up",
                "Internet And TV",
                "Security",
                "Insurance",
                "E-Tickets",
                "Education",
                "Transport",
                "Charity",
                "Commission",
                "Project Support",
                "Other"
           };

        #endregion

        #region Property Changed

        private string _category;
        private TypeOperation _typeOperation;
        private string _nameOfAmount;
        private decimal _amount;
        private DateTime _dataTime;

        #endregion
        private void AddFinDataInfo()
        {

        }
        private void DeleteFinDatainfo()
        {

        }
        private void LoadFromDB() 
        {

        }
        private void SaveToDB()
        {

        }
        private void DeleteFromDB()
        {

        }
        #endregion

        #region MenuBar
        public ICommand CloseAppCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public ICommand OpenLimitsCommand { get; }
        
        private void CloseAppCommandExecute(object p)
        {
            Application.Current.Shutdown();
        }

        private void OpenSettingsCommandExecute(object p) 
        {
            var window = new SettingsWindow();
            window.ShowDialog();
        }

        private void OpenLimitsCommandExecute(object p)
        {
            var window = new LimitWindow();
            window.ShowDialog();
        }
        private bool CanCloseAppCommandExecute(object p) => true;
        private bool CanOpenSettingsCommandExecute(object p) => true;
        private bool CanOpenLimitsCommandExecute(object p) => true;

        #endregion
        public MainWindowViewModel() 
        {
            CloseAppCommand = new LambdaCommand(CloseAppCommandExecute, CanCloseAppCommandExecute);
            OpenSettingsCommand = new LambdaCommand(OpenSettingsCommandExecute, CanOpenSettingsCommandExecute);
            OpenLimitsCommand = new LambdaCommand(OpenLimitsCommandExecute, CanOpenLimitsCommandExecute);
        }

    }
}
