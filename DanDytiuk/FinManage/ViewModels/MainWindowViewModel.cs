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
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Data;
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

        private void CleanComboBox()
        {
            Category = null;
            TypeOperation = TypeOperation.Unknown;
            NameOfAmount = string.Empty;
            Amount = 0;
            Description = string.Empty;
            DataTime = DateTime.Today;
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

        public ObservableCollection<MonthModel> Months { get; }

        #endregion

        #region Property Changed

        private string _selectedYearCB;
        private StatisticsModel _selectedMonthCB;
        private MonthModel _selectedMonth;
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
        public MonthModel SelectedMonth
        {
            get => _selectedMonth;
            set => Set(ref _selectedMonth, value);
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
                CategoriesCBExpenses,
                expenseStats,
                limits);

            foreach (var item in StatisticsList)
                item.RefreshLocalizationCategory();

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
            if (SelectedCurrency == null || SelectedMonth == null || SelectedYearCB == null)
            {
                MessageHelper.ShowError("PleaseSelectCurrencyMonthYearMessage", "Error");
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
                        command.Parameters.AddWithValue("@month", SelectedMonth.ValueMonth.ToString("D2"));
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
            if (SelectedCurrency == null || SelectedMonth == null || SelectedYearCB == null)
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
            if (SelectedCurrency == null || SelectedMonth == null || SelectedYearCB == null)
            {
                return new ObservableCollection<StatisticsModel>();
            }

            var list = new ObservableCollection<StatisticsModel>
            {
                new StatisticsModel
                {
                    SelectedCurrency = SelectedCurrency,
                    SelectedMonth = SelectedMonth.DisplayName,
                    SelectedYear = SelectedYearCB
                }
            };

            return list;
        }


        #endregion

        #endregion

        #region AnalyseIncome

        #region Commands Unit

        public ICommand GoInfoIncomeCommand { get; }
        public ICommand RefreshInfoIncomeCommand { get; }
        public ICommand CleanCBIncomeCommand { get; }
        private bool CanGoInfoIncomeCommandExecute(object p) => true;
        private bool CanRefreshInfoIncomeCommandExecute(object p) => true;
        private bool CanCleanCBIncomeCommandExecute(object p) => true;

        #endregion

        #region PropertyChanged

        private string _selectedYearIncomeCB;
        private MonthModel _selectedMonthIncomeCB;
        private string _selectedCurrencyIncome;

        public string SelectedYearIncomeCB
        {
            get => _selectedYearIncomeCB;
            set => Set(ref _selectedYearIncomeCB, value);
        }
        public MonthModel SelectedMonthIncomeCB
        {
            get => _selectedMonthIncomeCB;
            set => Set(ref _selectedMonthIncomeCB, value);
        }
        public string SelectedCurrencyIncomeCB
        {
            get => _selectedCurrencyIncome;
            set => Set(ref _selectedCurrencyIncome, value);
        }

        #endregion

        #region Collections

        public ObservableCollection<StatisticsModel> StatisticsListIncome { get; set; } = new ObservableCollection<StatisticsModel>();
        public ObservableCollection<StatisticsModel> ValueListIncome { get; set; } = new ObservableCollection<StatisticsModel>();

        #endregion

        #region Main functions

        private void GoLoadAnalyticsIncome(object p)
        {
            var expenseStats = LoadStatisticsIncome();

            StatisticsListIncome = BuildStatisticsListIncome(
                CategoriesCBIncome,
                expenseStats
                );

            foreach (var item in StatisticsListIncome)
                item.RefreshLocalizationCategory();

            OnPropertyChanged(nameof(StatisticsListIncome));

            ValueList = BuildValueIncomeList();

            OnPropertyChanged(nameof(ValueListIncome));
        }
        private void CleanCBAnalyticsIncome(object p)
        {
            SelectedYearIncomeCB = null;
            SelectedMonthIncomeCB = null;
            SelectedCurrencyIncomeCB = null;
        }

        #endregion

        #region Database functions

        private Dictionary<string, StatisticsModel> LoadStatisticsIncome()
        {
            if (SelectedCurrencyIncomeCB == null || SelectedYearIncomeCB == null || SelectedYearIncomeCB == null)
            {
                MessageHelper.ShowError("PleaseSelectCurrencyMonthYearMessage", "Error");
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

                        command.Parameters.AddWithValue("@currency", SelectedCurrencyIncomeCB);
                        command.Parameters.AddWithValue("@month", SelectedMonthIncomeCB.ValueMonth.ToString("D2"));
                        command.Parameters.AddWithValue("@year", SelectedYearIncomeCB.ToString());
                    
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

        private ObservableCollection<StatisticsModel> BuildStatisticsListIncome(
        IEnumerable<string> categories,
        Dictionary<string, StatisticsModel> expenses)
        {
            var list = new ObservableCollection<StatisticsModel>();

            foreach (var category in categories)
            {
                expenses.TryGetValue(category, out var stat);
                
                list.Add(new StatisticsModel 
                { 
                    Category = category,
                    MinAmount = stat?.MinAmount ?? 0,
                    AvgAmount = stat?.AvgAmount ?? 0,
                    MaxAmount = stat?.MaxAmount ?? 0,
                    TotalAmount = stat?.TotalAmount ?? 0
                });
            }
            return list;
        }

        private ObservableCollection<StatisticsModel> BuildValueIncomeList()
        {
            if (SelectedCurrencyIncomeCB == null || SelectedYearIncomeCB == null || SelectedYearCB == null)
            {
                return new ObservableCollection<StatisticsModel>();
            }

            var list = new ObservableCollection<StatisticsModel>
            {
                new StatisticsModel
                {
                    SelectedCurrency = SelectedCurrencyIncomeCB,
                    SelectedMonth = SelectedMonthIncomeCB.DisplayName,
                    SelectedYear = SelectedYearIncomeCB
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
        public ObservableCollection<string> CategoriesCBExpenses { get; } =
           new ObservableCollection<string>
           {
                "Food",
                "Store",
                "Entertainment",
                "Online_store",
                "Games",
                "Public_utilities",
                "Phone_top_up",
                "Internet_and_TV",
                "Security",
                "Insurance",
                "E_tickets",
                "Education",
                "Transport",
                "Charity",
                "Project_support",
                "Other"
           };

        public ObservableCollection<string> CategoriesCBIncome { get; } =
           new ObservableCollection<string>
           {
                "Salary",
                "Gift",
                "Vacation_pay",
                "Cashback",
                "Income_from_the_sale_of_shares",
                "Interest_on_deposits",
                "Government_benefits",
                "Pension",
                "Scholarship",
                "Child_support",
                "Debt_collection",
                "Insurance_payments",
                "Lottery_or_contest_winnings",
                "Other"
           };

        public ObservableCollection<CategoryModel> IncomeCategories { get; }
        public ObservableCollection<CategoryModel> ExpensesCategories { get; }
        public ObservableCollection<OperationModel> OperationTypes { get; }
        public ObservableCollection<CategoryModel> FullCategories { get; }

        #endregion

        #region Property Changed

        public DateTime MinDate { get; } = new DateTime(2020, 1, 1);
        public DateTime MaxDate { get; } = new DateTime(2099, 12, 31);

        private CategoryModel _category;
        private TypeOperation _typeOperation;
        private string _nameOfAmount;
        private string _typesOfCurrency;
        private decimal _amount;
        private DateTime? _dataTime = DateTime.Today;
        private string _description;
        private FinAllTableModel _selectedFinManage;

        public CategoryModel Category
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

                Category = null;
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
            if (Category == null | Amount <= 0 | TypeOperation == TypeOperation.Unknown)
            {
                MessageHelper.ShowError("PleaseSelectCategoryAmountMessage", "Error");
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

                        command.Parameters.AddWithValue("$category", Category.ResourceKey);
                        command.Parameters.AddWithValue("$operationtype", (int)TypeOperation);
                        command.Parameters.AddWithValue("$nameofamount", NameOfAmount ?? "");
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
            if (SelectedFinManage == null) { MessageHelper.ShowAttention("SelectStringMessage", "Attention"); return; }

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
                                CategoryKey = reader.GetString(1),
                                OperationType = (TypeOperation)reader.GetInt32(2),
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

        public ICommand GoFilterCommand { get; }
        public ICommand FilterCleanValueCommand { get; }
        public ICommand CleanFilterCategoryCommand { get; }
        public ICommand CleanFilterAmountFromCommand { get; }
        public ICommand CleanFilterAmountToCommand { get; }
        public ICommand CleanFilterDateFromCommand { get; }
        public ICommand CleanFilterCurrencyCommand { get; }
        public ICommand CleanFilterDateToCommand { get; }
        public ICommand CleanFilterShopNameCommand { get; }
        public ICommand CleanFilterOperationTypeCommand { get; }
        public ICommand CleanFilterDescriptionCommand { get; }
        private bool CanFilterCommandExecute (object p) => true;
        private bool CanFilterCleanValueCommandExecute(object p) => true;

        #endregion

        #region ComboBox

        public Array CurrencyCBFilter => Enum.GetValues(typeof(TypesOfCurrency));
        
        #endregion

        #region Property Changed

        private CategoryModel _filterCategory;
        private decimal? _filterAmountFrom;
        private decimal? _filterAmountTo;
        private DateTime? _filterDateFrom;
        private DateTime? _filterDateTo;
        private string _filterShopName;
        private OperationModel _filterOperationType;
        private string _filterCurrency;
        private string _filterDescription;

        public CategoryModel FilterCategory
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

        public OperationModel FilterOperationType
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

        private bool FilterData(object p)
        {
            if (!(p is FinAllTableModel item))
                return false;

            if (FilterCategory != null && item.CategoryKey != FilterCategory.ResourceKey) { return false; }

            if (FilterAmountFrom.HasValue && item.Amount < FilterAmountFrom.Value) { return false; }

            if (FilterAmountTo.HasValue && item.Amount > FilterAmountTo.Value) { return false; }
        
            if (FilterDateFrom.HasValue && item.DateInfo < FilterDateFrom.Value) { return false; }

            if (FilterDateTo.HasValue && item.DateInfo > FilterDateTo.Value) { return false; }

            if (!string.IsNullOrEmpty(FilterShopName))
            {
                if (item.NameOfAmount == null ||
                    !item.NameOfAmount.ToLower().Contains(FilterShopName.ToLower()))
                    return false;
            }

            if (FilterOperationType != null &&
                item.OperationType != FilterOperationType.Type)
                return false;

            if (!string.IsNullOrEmpty(FilterCurrency) &&
                item.Currency != FilterCurrency)
                return false;

            if (!string.IsNullOrWhiteSpace(FilterDescription) && 
                (item.Description == null || 
                item.Description.IndexOf(FilterDescription, StringComparison.OrdinalIgnoreCase) < 0)) return false;
            
            return true;
        }

        private void GoFilter(object p)
        {
            if (FilterFinCollection == null) return;

            FilterFinCollection.Refresh();
        }

        private void CleanFilter(object p)
        {
            FilterCategory = null;
            FilterAmountFrom = null;
            FilterAmountTo = null;
            FilterDateFrom = null;
            FilterDateTo = null;
            FilterShopName = null;
            FilterOperationType = null;
            FilterCurrency = null;
            FilterDescription = null;

            FilterFinCollection.Refresh();
        }
        
        private void CleanFilterCategory(object p)
        {
            if (FilterCategory == null) return;

            FilterCategory = null;
            FilterFinCollection.Refresh();
        }

        private void CleanFilterAmountFrom(object p)
        {
            if (FilterAmountFrom == null) return;

            FilterAmountFrom = null;
            FilterFinCollection.Refresh();
        }

        private void CleanFilterAmountTo(object p)
        {
            if (FilterAmountTo == null) return;

            FilterAmountTo = null;
            FilterFinCollection.Refresh();
        }
        private void CleanFilterDateFrom(object p)
        {
            if (FilterDateFrom == null) return;

            FilterAmountFrom = null;
            FilterFinCollection.Refresh();
        }
        private void CleanFilterDateTo(object p)
        {
            if (FilterDateTo == null) return;

            FilterDateTo = null;
            FilterFinCollection.Refresh();
        }
        private void CleanFilterShopName(object p)
        {
            if (FilterShopName == null) return;

            FilterShopName = null;
            FilterFinCollection.Refresh();
        }
        private void CleanFilterOperationType(object p)
        {
            if (FilterOperationType == null) return;

            FilterOperationType = null;
            FilterFinCollection.Refresh();
        }
        private void CleanFilterCurrency(object p)
        {
            if (FilterCurrency == null) return;

            FilterCurrency = null;
            FilterFinCollection.Refresh();
        }
        private void CleanFilterDescription(object p)
        {
            if (FilterDescription == null) return;

            FilterDescription = null; 
            FilterFinCollection.Refresh();
        }

        #endregion

        #region DataBase Functions



        #endregion

        #endregion

        public MainWindowViewModel()
        {
            #region ComboBox

            IncomeCategories = new ObservableCollection<CategoryModel>
            {
                new CategoryModel { ResourceKey = "Salary" },
                new CategoryModel { ResourceKey = "Gift" },
                new CategoryModel { ResourceKey = "Vacation_pay" },
                new CategoryModel { ResourceKey = "Cashback" },
                new CategoryModel { ResourceKey = "Income_from_the_sale_of_shares" },
                new CategoryModel { ResourceKey = "Interest_on_deposits" },
                new CategoryModel { ResourceKey = "Government_benefits" },
                new CategoryModel { ResourceKey = "Pension" },
                new CategoryModel { ResourceKey = "Scholarship" },
                new CategoryModel { ResourceKey = "Child_support" },
                new CategoryModel { ResourceKey = "Debt_collection" },
                new CategoryModel { ResourceKey = "Insurance_payments" },
                new CategoryModel { ResourceKey = "Lottery_or_contest_winnings" }
            };

            ExpensesCategories = new ObservableCollection<CategoryModel>
            {
                new CategoryModel { ResourceKey = "Food" },
                new CategoryModel { ResourceKey = "Store" },
                new CategoryModel { ResourceKey = "Entertainment" },
                new CategoryModel { ResourceKey = "Online_store" },
                new CategoryModel { ResourceKey = "Games" },
                new CategoryModel { ResourceKey = "Public_utilities" },
                new CategoryModel { ResourceKey = "Phone_top_up" },
                new CategoryModel { ResourceKey = "Internet_and_TV" },
                new CategoryModel { ResourceKey = "Security" },
                new CategoryModel { ResourceKey = "Insurance" },
                new CategoryModel { ResourceKey = "E_tickets" },
                new CategoryModel { ResourceKey = "Education" },
                new CategoryModel { ResourceKey = "Transport" },
                new CategoryModel { ResourceKey = "Charity" },
                new CategoryModel { ResourceKey = "Project_support" },
                new CategoryModel { ResourceKey = "Other" }
            };

            FullCategories = new ObservableCollection<CategoryModel>
            {
                new CategoryModel { ResourceKey = "Food" },
                new CategoryModel { ResourceKey = "Store" },
                new CategoryModel { ResourceKey = "Entertainment" },
                new CategoryModel { ResourceKey = "Online_store" },
                new CategoryModel { ResourceKey = "Games" },
                new CategoryModel { ResourceKey = "Public_utilities" },
                new CategoryModel { ResourceKey = "Phone_top_up" },
                new CategoryModel { ResourceKey = "Internet_and_TV" },
                new CategoryModel { ResourceKey = "Security" },
                new CategoryModel { ResourceKey = "Insurance" },
                new CategoryModel { ResourceKey = "E_tickets" },
                new CategoryModel { ResourceKey = "Education" },
                new CategoryModel { ResourceKey = "Transport" },
                new CategoryModel { ResourceKey = "Charity" },
                new CategoryModel { ResourceKey = "Project_support" },
                new CategoryModel { ResourceKey = "Other" },
                new CategoryModel { ResourceKey = "Salary" },
                new CategoryModel { ResourceKey = "Gift" },
                new CategoryModel { ResourceKey = "Vacation_pay" },
                new CategoryModel { ResourceKey = "Cashback" },
                new CategoryModel { ResourceKey = "Income_from_the_sale_of_shares" },
                new CategoryModel { ResourceKey = "Interest_on_deposits" },
                new CategoryModel { ResourceKey = "Government_benefits" },
                new CategoryModel { ResourceKey = "Pension" },
                new CategoryModel { ResourceKey = "Scholarship" },
                new CategoryModel { ResourceKey = "Child_support" },
                new CategoryModel { ResourceKey = "Debt_collection" },
                new CategoryModel { ResourceKey = "Insurance_payments" },
                new CategoryModel { ResourceKey = "Lottery_or_contest_winnings" }
            };

            OperationTypes = new ObservableCollection<OperationModel>
            {
                new OperationModel { Type = TypeOperation.Expenses  ,ResourceKey = "TypeOperation_Expenses" },
                new OperationModel { Type = TypeOperation.Income ,ResourceKey = "TypeOperation_Income" }
            };

            Months = new ObservableCollection<MonthModel>
            {
                new MonthModel { ValueMonth = 1, ResourceKey = "January" },
                new MonthModel { ValueMonth = 2, ResourceKey = "February" },
                new MonthModel { ValueMonth = 3, ResourceKey = "March" },
                new MonthModel { ValueMonth = 4, ResourceKey = "April" },
                new MonthModel { ValueMonth = 5, ResourceKey = "May" },
                new MonthModel { ValueMonth = 6, ResourceKey = "June" },
                new MonthModel { ValueMonth = 7, ResourceKey = "July" },
                new MonthModel { ValueMonth = 8, ResourceKey = "August" },
                new MonthModel { ValueMonth = 9, ResourceKey = "September" },
                new MonthModel { ValueMonth = 10, ResourceKey = "October" },
                new MonthModel { ValueMonth = 11, ResourceKey = "November" },
                new MonthModel { ValueMonth = 12, ResourceKey = "December" }
            };

            LocalizationHelper.Instance.PropertyChanged += (s, e) =>
            {
                foreach (var cat in IncomeCategories)
                    cat.Refresh();

                foreach (var cat in ExpensesCategories)
                    cat.Refresh();

                foreach (var cat in OperationTypes)
                    cat.Refresh();

                foreach (var cat in Months)
                    cat.Refresh();

                foreach (var cat in FullCategories) 
                    cat.Refresh();
            };

            #endregion

            #region MenuBar

            CloseAppCommand = new LambdaCommand(CloseAppCommandExecute, CanCloseAppCommandExecute);
            OpenSettingsCommand = new LambdaCommand(OpenSettingsCommandExecute, CanOpenSettingsCommandExecute);
            OpenLimitsCommand = new LambdaCommand(OpenLimitsCommandExecute, CanOpenLimitsCommandExecute);

            #endregion

            #region FinManageData

            _database = new DataBaseWork();

            OperationCB = new ObservableCollection<TypeOperation>((TypeOperation[])Enum.GetValues(typeof(TypeOperation)));

            AddFinDataInfoCommand = new LambdaCommand(AddFinDataInfo, CanAddFinDataInfoCommandExecute);
            DeleteFinDataInfoCommand = new LambdaCommand(DeleteFinDatainfo, CanDeleteDataInfoCommandExecute);

            LoadFromDB();

            #endregion

            #region AnalyseExpenses

            GoInfoCommand = new LambdaCommand(GoLoadAnalytics, CanGoInfoCommandExecute);
            CleanCBCommand = new LambdaCommand(CleanCBAnalytics, CanCleanCBCommandExecute);
            RefreshInfoCommand = new LambdaCommand(GoLoadAnalytics, CanRefreshInfoCommandExecute);

            #endregion

            #region AnalyseIncome

            GoInfoIncomeCommand = new LambdaCommand(GoLoadAnalyticsIncome, CanGoInfoIncomeCommandExecute);
            CleanCBIncomeCommand = new LambdaCommand(CleanCBAnalyticsIncome, CanCleanCBIncomeCommandExecute);
            RefreshInfoIncomeCommand = new LambdaCommand(GoLoadAnalyticsIncome, CanRefreshInfoIncomeCommandExecute);

            #endregion

            #region Filter

            FilterFinCollection = CollectionViewSource.GetDefaultView(MainFinAllTableColection);
            FilterFinCollection.Filter = FilterData;

            GoFilterCommand = new LambdaCommand(GoFilter, CanFilterCommandExecute);
            FilterCleanValueCommand = new LambdaCommand(CleanFilter, CanFilterCleanValueCommandExecute);

            CleanFilterCategoryCommand = new LambdaCommand(CleanFilterCategory);
            CleanFilterAmountFromCommand = new LambdaCommand(CleanFilterAmountFrom);
            CleanFilterAmountToCommand = new LambdaCommand(CleanFilterAmountTo);
            CleanFilterDateFromCommand = new LambdaCommand(CleanFilterDateFrom);
            CleanFilterDateToCommand = new LambdaCommand(CleanFilterDateTo);
            CleanFilterCurrencyCommand = new LambdaCommand(CleanFilterCurrency);
            CleanFilterShopNameCommand = new LambdaCommand(CleanFilterShopName);
            CleanFilterOperationTypeCommand = new LambdaCommand(CleanFilterOperationType);
            CleanFilterDescriptionCommand = new LambdaCommand(CleanFilterDescription);

            #endregion
        }
    }
}
