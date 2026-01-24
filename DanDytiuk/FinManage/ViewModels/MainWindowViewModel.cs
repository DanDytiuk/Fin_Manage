using FinManage.Infrastructure.Commands;
using FinManage.Models.Models_for_db;
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

        public string Category
        {
            get => _category;
            set => Set(ref _category, value);
        }
        public TypeOperation TypeOperation
        {
            get => _typeOperation;
            set => Set(ref _typeOperation, value);
        }
        public string NameOfAmount
        {
            get => _nameOfAmount;
            set => Set(ref _nameOfAmount, value);
        }
        public decimal Amount
        {
            get => _amount;
            set => Set(ref _amount, value);
        }
        public DateTime DataTime
        {
            get => _dataTime;
            set => Set(ref _dataTime, value);
        }

        #endregion

        #region Collections

        public ObservableCollection<FinAllTableModel> MainFinAllTableColection { get; } 
            = new ObservableCollection<FinAllTableModel>();

        #endregion

        #region Helpers

        private void ShowError(string message)
        {
            System.Windows.MessageBox.Show(
                message,
                "Error",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Warning);
        }

        #endregion

        #region Main functions
        private void AddFinDataInfo()
        {
            if (string.IsNullOrWhiteSpace(Category))
            {
                ShowError("Please select a category.");
            }

            if(Amount <= 0)
            {
                ShowError("Please enter a valid amount.");
                return;
            }

            if(TypeOperation == TypeOperation.Unknown)
            {
                ShowError("Please select a valid type operation.");
            }

            using(var connection = _database.GetConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                    @"
                    Insert Into 
                    ";
                }
            }
        }
        private void DeleteFinDatainfo()
        {

        }
        #endregion

        #region DataBase Functions
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
            #region MenuBar
            CloseAppCommand = new LambdaCommand(CloseAppCommandExecute, CanCloseAppCommandExecute);
            OpenSettingsCommand = new LambdaCommand(OpenSettingsCommandExecute, CanOpenSettingsCommandExecute);
            OpenLimitsCommand = new LambdaCommand(OpenLimitsCommandExecute, CanOpenLimitsCommandExecute);
            #endregion
        }

    }
}
