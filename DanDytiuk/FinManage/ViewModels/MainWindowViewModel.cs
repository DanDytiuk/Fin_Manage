using FinManage.Infrastructure.Commands;
using FinManage.Models;
using FinManage.Models.Models_for_db;
using FinManage.Services;
using FinManage.View.Windows;
using FinManage.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using static FinManage.Infrastructure.EnumInfrastructure;

namespace FinManage.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        #region Data

        private readonly DataBaseWork _database;

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

        private void CleanComboBox()
        {
            Category = null;
            TypeOperation = TypeOperation.Unknown;
            NameOfAmount = string.Empty;
            Amount = 0;
            Description = string.Empty;
            DataTime = DateTime.Today;
        }

        private void CleanComboBoxAnalyse()
        {

        }

        #endregion

        #region AnalyseExpenses

        #region Commands Unit

        public ICommand GoInfoCommand { get; }
        public ICommand RefreshInfoCommand { get; }
        public ICommand CleanCBCommand { get; }
        private bool CanRefreshInfoCommandExecute(object p) => true;
        private bool CanGoInfoCommandExecute(object p) => true;
        private bool CanCleanCBCommandExecute(object p) => true;

        #endregion

        #region ComboBox

        public Array MonthCB => Enum.GetValues(typeof(Months));

        public ObservableCollection<int> YearsCB { get; } = new ObservableCollection<int>(Enumerable.Range(2025, 20));

        public ObservableCollection<StatisticsModel> Months { get; } =
            new ObservableCollection<StatisticsModel>
            {
                new StatisticsModel { ValueMonth = 1, NameMonth = "January" },
                new StatisticsModel { ValueMonth = 2, NameMonth = "February" },
                new StatisticsModel { ValueMonth = 3, NameMonth = "March" },
                new StatisticsModel { ValueMonth = 4, NameMonth = "April" },
                new StatisticsModel { ValueMonth = 5, NameMonth = "May" },
                new StatisticsModel { ValueMonth = 6, NameMonth = "June" },
                new StatisticsModel { ValueMonth = 7, NameMonth = "July" },
                new StatisticsModel { ValueMonth = 8, NameMonth = "August" },
                new StatisticsModel { ValueMonth = 9, NameMonth = "September" },
                new StatisticsModel { ValueMonth = 10, NameMonth = "October" },
                new StatisticsModel { ValueMonth = 11, NameMonth = "November" },
                new StatisticsModel { ValueMonth = 12, NameMonth = "December" }
            };

        #endregion

        #region Property Changed

        private string _selectedYearCB;
        private StatisticsModel _selectedMonthCB;
        private string _selectedCurrency;

        public string SelectedYearCB
        {
            get => _selectedYearCB;
            set => Set(ref _selectedYearCB, value);
        }
        public StatisticsModel SelectedMonthCB
        {
            get => _selectedMonthCB;
            set => Set(ref _selectedMonthCB, value);
        }
        public string SelectedCurrency
        {
            get => _selectedCurrency;
            set => Set(ref _selectedCurrency, value);
        }

        #endregion

        #region Collections
        public ObservableCollection<StatisticsModel> StatisticsList { get; set; } = new ObservableCollection<StatisticsModel>();
        public ObservableCollection<StatisticsModel> ValueList { get; set; } = new ObservableCollection<StatisticsModel>();

        #endregion

        #region Main functions

        private void GoLoadAnalytics(object p)
        {
            var expenseStats = LoadStatistics();
            var limits = LoadLimits();

            StatisticsList = BuildStatisticsList(
                CategoriesCB,
                expenseStats,
                limits);

            OnPropertyChanged(nameof(StatisticsList));

            ValueList = BuildValuelist();

            OnPropertyChanged(nameof(ValueList));
        }

        private void CleanCBAnalytics(object p)
        {
            SelectedYearCB = null;
            SelectedMonthCB = null;
            SelectedCurrency = null;
        }
        #endregion

        #region Database functions

        private Dictionary<string, StatisticsModel> LoadStatistics()
        {
            if (SelectedCurrency == null || SelectedMonthCB == null || SelectedYearCB == null)
            {
                ShowError("Please select a month, year or type currency.");
                return new Dictionary<string, StatisticsModel>();
            }
            else
            {
                var result = new Dictionary<string, StatisticsModel>();

                using (var connection = _database.GetConnection())
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText =
                        @"SELECT Category,
                        IFNULL(MIN(Amount),0),
                        IFNULL(AVG(Amount),0),
                        IFNULL(MAX(Amount),0),
                        IFNULL(SUM(Amount),0)
                    FROM MainData
                    WHERE Currency = @currency
                        AND strftime('%m', DateInfo) = @month
                        AND strftime('%Y', DateInfo) = @year
                    GROUP BY Category";

                        command.Parameters.AddWithValue("@currency", SelectedCurrency);
                        command.Parameters.AddWithValue("@month", SelectedMonthCB.ValueMonth.ToString("D2"));
                        command.Parameters.AddWithValue("@year", SelectedYearCB.ToString());

                        using (var reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                var category = reader.GetString(0);

                                result[category] = new StatisticsModel
                                {
                                    Category = category,
                                    MinAmount = reader.GetDecimal(1),
                                    AvgAmount = reader.GetDecimal(2),
                                    MaxAmount = reader.GetDecimal(3),
                                    TotalAmount = reader.GetDecimal(4)
                                };
                            }
                        }
                    }
                }
                return result;
            }
        }

        private Dictionary<string, decimal> LoadLimits()
        {
            if (SelectedCurrency == null || SelectedMonthCB == null || SelectedYearCB == null)
            {
                return new Dictionary<string, decimal>();
            }

            var limits = new Dictionary<string, decimal>();

            using (var connection = _database.GetConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        @"SELECT Category, IFNULL(Amount,0)
                  FROM Limits
                  WHERE Currency = $currency";

                    command.Parameters.AddWithValue("$currency", SelectedCurrency);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            limits[reader.GetString(0)] = reader.GetDecimal(1);
                    }
                }
            }

            return limits;
        }

        private ObservableCollection<StatisticsModel> BuildStatisticsList(
        IEnumerable<string> categories,
        Dictionary<string, StatisticsModel> expenses,
        Dictionary<string, decimal> limits)
        {
            var list = new ObservableCollection<StatisticsModel>();

            foreach (var category in categories)
            {
                expenses.TryGetValue(category, out var stat);
                limits.TryGetValue(category, out var limit);

                list.Add(new StatisticsModel
                {
                    Category = category,
                    MinAmount = stat?.MinAmount ?? 0,
                    AvgAmount = stat?.AvgAmount ?? 0,
                    MaxAmount = stat?.MaxAmount ?? 0,
                    TotalAmount = stat?.TotalAmount ?? 0,
                    Limit = limit
                });
            }

            return list;
        }

        private ObservableCollection<StatisticsModel> BuildValuelist()
        {
            if (SelectedCurrency == null || SelectedMonthCB == null || SelectedYearCB == null)
            {
                return new ObservableCollection<StatisticsModel>();
            }

            var list = new ObservableCollection<StatisticsModel>
            {
                new StatisticsModel
                {
                    SelectedCurrency = SelectedCurrency,
                    SelectedMonth = SelectedMonthCB.ValueMonth.ToString(),
                    SelectedYear = SelectedYearCB
                }
            };

            return list;
        }


        #endregion

        #endregion

        #region FinDataGrid

        #region Commands Unit
        public ICommand AddFinDataInfoCommand { get; }
        public ICommand DeleteFinDataInfoCommand { get; }
        
        private bool CanAddFinDataInfoCommandExecute(object p) => true;
        private bool CanDeleteDataInfoCommandExecute(object p) => true;
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

        public ObservableCollection<CategoryModel> IncomeCategories { get; }
        public ObservableCollection<CategoryModel> ExpensesCategories { get; }

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
            set 
            { 
                Set(ref _typeOperation, value);

                OnPropertyChanged(nameof(IsIncomeVisible));
                OnPropertyChanged(nameof(IsExpenseVisible));
            }
        }

        public bool IsIncomeVisible =>
            TypeOperation == TypeOperation.Income;

        public bool IsExpenseVisible =>
            TypeOperation == TypeOperation.Expenses;

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

        #region Main functions
        private void AddFinDataInfo(object p)
        {
            if (string.IsNullOrWhiteSpace(Category) | Amount <= 0 | TypeOperation == TypeOperation.Unknown)
            {
                ShowError("Please select a category, valid amount or type operation.");
            }
            else
            {
                using (var connection = _database.GetConnection())
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

                CleanComboBox();
            }
        }
        private void DeleteFinDatainfo(object p)
        {
            if (SelectedFinManage == null) { ShowAttention("Please a select string for delete!"); return; }

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
                                Amount = reader.GetDecimal(4),
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

        #region Filter

        #region Commands Unit

        public ICommand FilterCommand { get; }
        public ICommand FilterCleanValueCommand { get; }

        private bool CanFilterCommandExecute (object p) => true;
        private bool CanFilterCleanValueCommandExecute(object p) => true;

        #endregion

        #region ComboBox

        public Array CurrencyCBFilter => Enum.GetValues(typeof(TypesOfCurrency));
        public ObservableCollection<TypeOperation> OperationCBFilter { get; }
        public ObservableCollection<string> CategoriesCBFilter { get; } =
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

        private string _filterCategory;
        private decimal? _filterAmountFrom;
        private decimal? _filterAmountTo;
        private DateTime? _filterDateFrom;
        private DateTime? _filterDateTo;
        private string _filterShopName;
        private string _filterOperationType;
        private string _filterCurrency;
        private string _filterDescription;

        public string FilterCategory
        {
            get => _filterCategory;
            set => Set(ref _filterCategory, value);
        }

        public decimal? FilterAmountFrom
        {
            get => _filterAmountFrom ?? (decimal?)null;
            set => Set(ref _filterAmountFrom, value);
        }

        public decimal? FilterAmountTo
        {
            get => (_filterAmountTo ?? (decimal?)null);
            set => Set(ref _filterAmountTo, value);
        }

        public DateTime? FilterDateFrom
        {
            get => _filterDateFrom ?? (DateTime?)null;
            set => Set(ref _filterDateFrom, value);
        }

        public DateTime? FilterDateTo
        {
            get => _filterDateTo ?? (DateTime?)null;
            set => Set(ref _filterDateTo, value);
        }

        public string FilterShopName
        {
            get => _filterShopName;
            set => Set(ref _filterShopName, value);
        }

        public string FilterOperationType
        {
            get => _filterOperationType;
            set => Set(ref _filterOperationType, value);
        }

        public string FilterCurrency
        {
            get => _filterCurrency;
            set => Set(ref _filterCurrency, value);
        }

        public string FilterDescription
        {
            get => _filterDescription;
            set => Set(ref _filterDescription, value);
        }
        #endregion

        #region Collections

        private ICollectionView _filterFinCollection;

        public ICollectionView FilterFinCollection
        {
            get => _filterFinCollection;
            set => Set(ref _filterFinCollection, value);
        }

        #endregion

        #region Main functions



        #endregion

        #region DataBase Functions



        #endregion

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
            OperationCB = new ObservableCollection<TypeOperation>((TypeOperation[])Enum.GetValues(typeof(TypeOperation)));

            IncomeCategories = new ObservableCollection<CategoryModel>
            {
                new CategoryModel { ResourceKey = "Salary" },
                new CategoryModel { ResourceKey = "Gift" },
            };

            ExpensesCategories = new ObservableCollection<CategoryModel> 
            { 
                new CategoryModel { ResourceKey = "" },
            };

            AddFinDataInfoCommand = new LambdaCommand(AddFinDataInfo, CanAddFinDataInfoCommandExecute);
            DeleteFinDataInfoCommand = new LambdaCommand(DeleteFinDatainfo, CanDeleteDataInfoCommandExecute);

            LoadFromDB();
            #endregion

            #region Analysis

            GoInfoCommand = new LambdaCommand(GoLoadAnalytics, CanGoInfoCommandExecute);
            CleanCBCommand = new LambdaCommand(CleanCBAnalytics, CanCleanCBCommandExecute);
            RefreshInfoCommand = new LambdaCommand(GoLoadAnalytics, CanRefreshInfoCommandExecute);

            #endregion

            #region Filter



            #endregion
        }
    }
}
