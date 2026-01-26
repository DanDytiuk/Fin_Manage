using FinManage.Infrastructure.Commands;
using FinManage.Models;
using FinManage.Models.Models_for_db;
using FinManage.Services;
using FinManage.ViewModels.Base;
using Microsoft.Data.Sqlite;
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

        public ObservableCollection<string> Categories { get; }
            = new ObservableCollection<string>();

        public ObservableCollection<TypesOfCurrency> CurrencyCB { get; }

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

        private string _selectedCategory;
        private decimal _amount;
        private string _currency;
        private string _description;
        private LimitsModel _selectedLimit;

        public string SelectedCategory
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

        #region Commands Logic

        private void AddLimit(object _)
        {
            if (string.IsNullOrWhiteSpace(SelectedCategory))
            {
                ShowError("Please select a category.");
                return;
            }

            if (Amount <= 0)
            {
                ShowError("Please enter a valid amount.");
                return;
            }

            if (Limits.Any(l => l.Category == SelectedCategory))
            {
                ShowError($"Limit for this categoty - {SelectedCategory} already exists!");
                return;
            }

            using (var connection = _database.GetConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                    @"
                    INSERT INTO Limits (Category, Amount, Currency, Description)
                    VALUES ($category, $amount, $currency, $description);
                    ";

                    command.Parameters.AddWithValue("$category", SelectedCategory);
                    command.Parameters.AddWithValue("$amount", Amount);
                    command.Parameters.AddWithValue("$currency", Currency ?? "");
                    command.Parameters.AddWithValue("$description", Description ?? "");

                    command.ExecuteNonQuery();
                }
            }

            LoadFromDatabase();
        }

        private void DeleteLimit(object _)
        {
            if (SelectedLimit == null)
                return;

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

        private void Cancel(object _)
        {
            CloseAction?.Invoke();
        }

        #endregion

        #region Database Load

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
                    "SELECT Id, Category, Amount, Currency, Description FROM Limits;";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var limit = new LimitsModel
                            {
                                Id = reader.GetInt32(0),
                                Category = reader.GetString(1),
                                Amount = reader.GetDecimal(2),
                                Currency = reader.GetString(3),
                                Description = reader.IsDBNull(4) ? null : reader.GetString(4)
                            };

                            Limits.Add(limit);

                            if (!Categories.Contains(limit.Category))
                                Categories.Add(limit.Category);
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


            AddLimitCommand = new LambdaCommand(AddLimit);
            DeleteLimitCommand = new LambdaCommand(DeleteLimit);
            CancelCommand = new LambdaCommand(Cancel);

            LoadFromDatabase();
        }

        #endregion
    }
}
