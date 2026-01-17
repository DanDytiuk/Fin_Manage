using FinManage.Infrastructure.Commands;
using FinManage.Models;
using FinManage.Services;
using FinManage.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static FinManage.Infrastructure.EnumInfrastructure;

namespace FinManage.ViewModels
{
    internal class LimitWindowsViewModel : BaseViewModel
    {
        private readonly LimitService _limitService;
        /*public LimitModel LimitData { get; }
        public LimitService LimitService { get; }
        public Category SelectedCategory {  get; set; }*/
        #region Заполнение ComboBox
        public ObservableCollection<Category> Categories { get; }
        public ObservableCollection<LimitModel> Limits { get; } = new ObservableCollection<LimitModel>();

        #endregion
        public ICommand AddLimitCommand { get; }
        public ICommand CancelLimitCommand { get; }
       

        private void AddLimitCommandExecute()
        {
            
        } 
        private void CancelLimitCommandExecute()
        {

        }
        private bool CanAddLimitCommand(object parameter) => true;
        private bool CanCancelLimitCommand(object parameter) => true;
        public LimitWindowsViewModel()
        {
            _limitService = new LimitService();
            //LimitData = _limitService.Load();
        }
    }
}
