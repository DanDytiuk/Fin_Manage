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
using static FinManage.Infrastructure.EnumInfrastructure;

namespace FinManage.ViewModels
{
    internal class LimitWindowsViewModel : BaseViewModel
    {
        private readonly string FileName = "limits.json";
        private static readonly string Appdata = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FinManage");
        private static readonly string Filepath = Path.Combine(Appdata, "limits.json");
        private LimitModel LimitModel;
        #region Заполнение ComboBox
        public ObservableCollection<Category> Categories { get; }
        public ObservableCollection<LimitModel> Limits { get; } = new ObservableCollection<LimitModel>();

        #endregion
        public ICommand AddLimitCommand { get; }
        public ICommand DeleteLimitCommand { get; }
        public Array Category => Enum.GetValues(typeof(Category));
        private void AddLimitCommandExecute(object p)
        {
            if (LimitModel.Monthlylimit <= 0) return;
            if (Limits.Any(l => l.Category == LimitModel.SelectCategoryFromUser)) return;
            Limits.Add(new LimitModel
            {
                Category = LimitModel.SelectCategoryFromUser,
                Monthlylimit = (decimal)LimitModel.FillAmountFromUser
            });

            SavetoFileCommand();
        } 
        private void DeleteLimitCommandExecute(object p)
        {
            if (LimitModel.FillAmountFromUser == null) return;
            Limits.Remove(LimitModel.Selectedlimit);

            SavetoFileCommand();
        }
        private void SavetoFileCommand()
        {
            if(!Directory.Exists(Appdata)) Directory.CreateDirectory(Appdata);

            var dict = Limits.ToDictionary(l => l.Category.ToString(), l => l.FillAmountFromUser);

            var json = JsonSerializer.Serialize(dict, new JsonSerializerOptions 
            { 
                WriteIndented = true,
            });

            File.WriteAllText(Filepath, json);
        }
        private void LoadFromFileCommand()
        {
            if(!File.Exists(Filepath)) return;

            var json  = File.ReadAllText(Filepath);

            var dict = JsonSerializer.Deserialize<Dictionary<string, decimal>>(json);

            Limits.Clear();

            foreach ( var item in dict)
            {
                if (Enum.TryParse(item.Key, out Category category))
                {
                    Limits.Add(new LimitModel
                    {
                        Category = category,
                        FillAmountFromUser = item.Value
                    });
                }
            }
        }
        private bool CanAddLimitCommand(object parameter) => true;
        private bool CanDeleteLimitCommand(object parameter) => true;
        public LimitWindowsViewModel()
        {
            AddLimitCommand = new LambdaCommand(AddLimitCommandExecute, CanAddLimitCommand);
            DeleteLimitCommand = new LambdaCommand(DeleteLimitCommandExecute, CanDeleteLimitCommand);

            LoadFromFileCommand();
        }
    }
}
