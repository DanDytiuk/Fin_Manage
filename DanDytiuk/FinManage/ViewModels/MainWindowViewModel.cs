using FinManage.Infrastructure.Commands;
using FinManage.Models.Models_for_db;
using FinManage.Properties;
using FinManage.Services;
using FinManage.View.Windows;
using FinManage.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.ComponentModel;
using static FinManage.Infrastructure.EnumInfrastructure;
using LiveCharts;
using LiveCharts.Wpf;

namespace FinManage.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        #region Analysis



        #endregion


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

        public Array CurrencyCB => Enum.GetValues(typeof(TypesOfCurrency));
        public ObservableCollection<TypeOperation> OperationCB { get; }
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

        public DateTime MinDate { get; } = new DateTime(2020, 1, 1);
        public DateTime MaxDate { get; } = new DateTime(2099, 12, 31);


        private string _category;
        private TypeOperation _typeOperation;
        private string _nameOfAmount;
        private string _typesOfCurrency;
        private decimal _amount;
        private DateTime? _dataTime = DateTime.Today;
        private string _description;
        private FinAllTableModel _selectedFinManage;
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
        public string Currency
        {
            get => _typesOfCurrency;
            set => Set(ref _typesOfCurrency, value);
        }
        public decimal Amount
        {
            get => _amount;
            set => Set(ref _amount, value);
        }
        public DateTime? DataTime
        {
            get => _dataTime;
            set => Set(ref _dataTime, value);
        }
        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }
        public FinAllTableModel SelectedFinManage
        {
            get => _selectedFinManage;
            set => Set(ref _selectedFinManage, value);
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

        private void ShowMessage(string message)
        {
            System.Windows.MessageBox.Show(
                message,
                "Info",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }

        private void ShowAttention(string message)
        {
            System.Windows.MessageBox.Show(
                message,
                "Attention",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Warning);
        }

        private void ShowLearn(string message)
        {
            System.Windows.MessageBox.Show(
                message,
                "Welcome",
                System.Windows.MessageBoxButton.OKCancel,
                System.Windows.MessageBoxImage.Question);
        }

        #endregion

        #region Main functions
        private void AddFinDataInfo(object p)
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
                    Insert Into MainData (Category, OperationType, Name_of_Amount, Amount, Currency, DateInfo, Description)
                    Values ($category, $operationtype, $nameofamount, $amount, $currency, $dateinfo, $description);
                    ";

                    command.Parameters.AddWithValue("$category", Category.ToString());
                    command.Parameters.AddWithValue("$operationtype", TypeOperation.ToString());
                    command.Parameters.AddWithValue("$nameofamount", NameOfAmount);
                    command.Parameters.AddWithValue("$amount", Amount);
                    command.Parameters.AddWithValue("$currency", Currency);
                    command.Parameters.AddWithValue("$dateinfo", DataTime);
                    command.Parameters.AddWithValue("$description", Description ?? "");

                    command.ExecuteNonQuery();
                }
            }

            LoadFromDB();
        }
        private void DeleteFinDatainfo(object p)
        {
            if (SelectedFinManage == null)  return;

            using (var connection = _database.GetConnection()) 
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                    "Delete From MainData Where Id = $id";

                    command.Parameters.AddWithValue("$id", SelectedFinManage.Id);

                    command.ExecuteNonQuery();
                }
            }

            MainFinAllTableColection.Remove(SelectedFinManage);

            LoadFromDB();
        }
        #endregion

        #region DataBase Functions
        private void LoadFromDB()
        {
            MainFinAllTableColection.Clear();

            using (var connection = _database.GetConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "Select Id, Category, OperationType, Name_of_Amount, Amount, Currency, DateInfo, Description From MainData;";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var maindata = new FinAllTableModel
                            {   
                                Id = reader.GetInt32(0),
                                Category = reader.GetString(1),
                                OperationType = reader.GetString(2),
                                NameOfAmount = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Amount = reader.GetInt32(4),
                                Currency = reader.GetString(5),
                                DateInfo = reader.GetDateTime(6),
                                Description = reader.IsDBNull(7) ? null : reader.GetString(7)
                            };

                            MainFinAllTableColection.Add(maindata);
                        }
                    }
                }
            }
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

            #region FinManageData

            _database = new DataBaseWork();
            //CurrencyCB = new ObservableCollection<TypesOfCurrency>((TypesOfCurrency[])Enum.GetValues(typeof(TypesOfCurrency)));
            OperationCB = new ObservableCollection<TypeOperation>((TypeOperation[])Enum.GetValues(typeof(TypeOperation)));

            AddFinDataInfoCommand = new LambdaCommand(AddFinDataInfo, CanAddFinDataInfoCommandExecute);
            DeleteFinDataInfoCommand = new LambdaCommand(DeleteFinDatainfo, CanDeleteDataInfoCommandExecute);

            LoadFromDB();
            #endregion
        }

    }
}
