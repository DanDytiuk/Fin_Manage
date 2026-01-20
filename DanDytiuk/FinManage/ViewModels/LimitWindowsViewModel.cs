using FinManage.Infrastructure.Commands;
using FinManage.Models;
using FinManage.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;

namespace FinManage.ViewModels
{
    internal class LimitWindowsViewModel : BaseViewModel
    {
        #region Path

        private static readonly string AppDataPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FinManage");

        private static readonly string FilePath =
            Path.Combine(AppDataPath, "limits.json");

        #endregion

        #region ComboBox

        public ObservableCollection<string> Categories { get; } =
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
                "E Tickets",
                "Education",
                "Transport",
                "Charity",
                "Commission",
                "Project Support"
            };

        #endregion

        #region PropertyChanged

        private string _selectedCategory;
        private decimal _amount;
        private LimitModel _selectedLimit;

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
        
        public LimitModel SelectedLimit
        {
            get => _selectedLimit;
            set => Set(ref _selectedLimit, value);
        }

        #endregion

        #region Collections

        public ObservableCollection<LimitModel> Limits { get; }
            = new ObservableCollection<LimitModel>();

        #endregion

        #region Commands

        public ICommand AddLimitCommand { get; }
        public ICommand DeleteLimitCommand { get; }
        public ICommand CancelCommand { get; }

        public Action CloseAction { get; set; }

        #endregion

        #region Command
        private void ShowError(string message)
        {
            System.Windows.MessageBox.Show(
                message,
                "Error",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Warning);
        }

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
                ShowError("Limit for this category already exists.");
                return;
            }

            Limits.Add(new LimitModel
            {
                Category = SelectedCategory,
                MonthlyLimit = Amount
            });

            SaveToFile();
        }

        private void DeleteLimit(object _)
        {
            if (SelectedLimit == null) return;

            Limits.Remove(SelectedLimit);
            SaveToFile();
        }

        private void Cancel(object _)
        {
            CloseAction?.Invoke();
        }

        #endregion

        #region Functions Save, Load, Delete

        private void SaveToFile()
        {
            if (!Directory.Exists(AppDataPath))
                Directory.CreateDirectory(AppDataPath);

            var dict = Limits.ToDictionary(
                l => l.Category,
                l => l.MonthlyLimit);

            var json = JsonSerializer.Serialize(dict, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(FilePath, json);
        }

        private void LoadFromFile()
        {
            if (!File.Exists(FilePath)) return;

            var json = File.ReadAllText(FilePath);
            var dict = JsonSerializer.Deserialize<Dictionary<string, decimal>>(json);

            Limits.Clear();

            foreach (var item in dict)
            {
                Limits.Add(new LimitModel
                {
                    Category = item.Key,
                    MonthlyLimit = item.Value
                });
            }
        }

        #endregion

        public LimitWindowsViewModel()
        {
            AddLimitCommand = new LambdaCommand(AddLimit);
            DeleteLimitCommand = new LambdaCommand(DeleteLimit);
            CancelCommand = new LambdaCommand(Cancel);

            LoadFromFile();
        }
    }
}
