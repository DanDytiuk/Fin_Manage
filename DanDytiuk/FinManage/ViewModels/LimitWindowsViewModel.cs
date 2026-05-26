using FinManage.Infrastructure.Commands;
using FinManage.Models;
using FinManage.Models.Models_for_db;
using FinManage.Services;
using FinManage.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using static FinManage.Infrastructure.EnumInfrastructure;

namespace FinManage.ViewModels
{
    internal class LimitWindowsViewModel : BaseViewModel
    {
        #region Data

        private readonly DataBaseWork _database;

        #endregion

        #region ComboBox

        public ObservableCollection<int> YearsCB { get; } = new ObservableCollection<int>(Enumerable.Range(2025, 20));
        
        public ObservableCollection<MonthModel> Months { get; }

        public ObservableCollection<string> Categories { get; }
            = new ObservableCollection<string>();

        public ObservableCollection<TypesOfCurrency> CurrencyCB { get; }

        public ObservableCollection<CategoryModel> CategoryComboBox { get; }

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

        #region PropertyChanged

        public DateTime MinDate { get; } = new DateTime(2020, 1, 1);
        public DateTime MaxDate { get; } = new DateTime(2099, 12, 31);

        private CategoryModel _selectedCategory;
        private decimal _amount;
        private string _currency;
        private string _description;
        private LimitsModel _selectedLimit;
        private DateTime _startDate;
        private DateTime _endDate;
        private MonthModel _Month;
        private int _Year;

        public CategoryModel SelectedCategory
        {
            get => _selectedCategory;
            set => Set(ref _selectedCategory, value);
        }

        public decimal Amount
        {
            get => _amount;
            set => Set(ref _amount, value);
        }

        public LimitsModel SelectedLimit
        {
            get => _selectedLimit;
            set => Set(ref _selectedLimit, value);
        }

        public string Currency
        {
            get => _currency;
            set => Set(ref _currency, value);
        }

        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }
        public DateTime StartDate
        {
            get => _startDate;
            set => Set(ref _startDate, value);
        }
        public DateTime EndDate
        {
            get => _endDate;
            set => Set(ref _endDate,value);
        }
        public MonthModel Month
        {
            get => _Month;
            set => Set(ref _Month, value);
        }
        public int Year
        {
            get => _Year;
            set => Set(ref _Year, value);
        }

        #endregion

        #region Collections

        public ObservableCollection<LimitsModel> Limits { get; }
            = new ObservableCollection<LimitsModel>();

        #endregion

        #region Commands

        public ICommand AddLimitCommand { get; }
        public ICommand DeleteLimitCommand { get; }
        public ICommand CancelCommand { get; }

        public Action CloseAction { get; set; }

        #endregion

        #region Commands Logic

        private void AddLimit(object p)
        {
            if (SelectedCategory == null || Amount <= 0)
            {
                MessageHelper.ShowError("PleaseSelectCategoryAmountMessage", "Warning");
                return;
            }

            if (Limits.Any(l => l.CategoryKey == SelectedCategory.ResourceKey && l.Currency == Currency))
            {
                MessageHelper.ShowError("LimitExistsMessage", "Warning");
                return;
            }

            using (var connection = _database.GetConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                    @"
                    INSERT INTO Limits (Category, Amount, Currency, Description, Month, Year)
                    VALUES ($category, $amount, $currency, $description, $month, $year);
                    ";

                    command.Parameters.AddWithValue("$category", SelectedCategory.ResourceKey);
                    command.Parameters.AddWithValue("$amount", Amount);
                    command.Parameters.AddWithValue("$currency", Currency ?? "");
                    command.Parameters.AddWithValue("$description", Description ?? "");
                    //command.Parameters.AddWithValue("$month", Month);
                    command.Parameters.AddWithValue("$month", Month?.ValueMonth ?? 0);
                    command.Parameters.AddWithValue("$year", Year);

                    command.ExecuteNonQuery();
                }
            }

            LoadFromDatabase();
        }

        private void DeleteLimit(object p)
        {
            if (SelectedLimit == null) 
            {
                MessageHelper.ShowError("SelectedLimitNullMessage", "Warning");
                return; 
            }
                

            using (var connection = _database.GetConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                    "DELETE FROM Limits WHERE Id = $id";

                    command.Parameters.AddWithValue("$id", SelectedLimit.Id);
                    command.ExecuteNonQuery();
                }
            }

            Limits.Remove(SelectedLimit);
        }

        private void Cancel(object p)
        {
            CloseAction?.Invoke();
        }

        #endregion

        #region Database Load

        /*private void LoadFromDatabase()
        {
            Limits.Clear();
            Categories.Clear();

            using (var connection = _database.GetConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                    "SELECT Id, Category, Amount, Currency, Description, Month, Year FROM Limits;";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var limit = new LimitsModel
                            {
                                Id = reader.GetInt32(0),
                                CategoryKey = reader.GetString(1),
                                Amount = reader.GetDecimal(2),
                                Currency = reader.GetString(3),
                                Description = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Month = reader.GetString(5),
                                Year = reader.GetInt32(6)
                            };

                            Limits.Add(limit);

                            if (!Categories.Contains(limit.CategoryKey))
                                Categories.Add(limit.CategoryKey);
                        }
                    }
                }
            }
        }*/

        private void LoadFromDatabase()
        {
            Limits.Clear();
            Categories.Clear();

            using (var connection = _database.GetConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT Id, Category, Amount, Currency, Description, Month, Year FROM Limits;";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int monthValue = reader.GetInt32(5);

                            var monthObj = Months.FirstOrDefault(m => m.ValueMonth == monthValue);

                            var limit = new LimitsModel
                            {
                                Id = reader.GetInt32(0),
                                CategoryKey = reader.GetString(1),
                                Amount = reader.GetDecimal(2),
                                Currency = reader.GetString(3),
                                Description = reader.IsDBNull(4) ? "" : reader.GetString(4),

                                Month = monthObj,

                                Year = reader.GetInt32(6)
                            };

                            Limits.Add(limit);

                            if (!Categories.Contains(limit.CategoryKey))
                                Categories.Add(limit.CategoryKey);
                        }
                    }
                }
            }
        }

        #endregion

        #region Constructor

        public LimitWindowsViewModel()
        {
            _database = new DataBaseWork();
            CurrencyCB = new ObservableCollection<TypesOfCurrency>((TypesOfCurrency[])Enum.GetValues(typeof(TypesOfCurrency)));

            CategoryComboBox = new ObservableCollection<CategoryModel>
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

            AddLimitCommand = new LambdaCommand(AddLimit);
            DeleteLimitCommand = new LambdaCommand(DeleteLimit);
            CancelCommand = new LambdaCommand(Cancel);

            LocalizationHelper.Instance.PropertyChanged += (s, e) =>
            {
                foreach (var cat in CategoryComboBox)
                    cat.Refresh();

                foreach (var cat in Months)
                    cat.Refresh();
            };

            LoadFromDatabase();
        }

        #endregion
    }
}
