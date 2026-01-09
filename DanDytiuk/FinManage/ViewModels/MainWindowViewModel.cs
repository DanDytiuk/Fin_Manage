using FinManage.Infrastructure.Commands;
using FinManage.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace FinManage.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        #region Команды
        public ICommand CloseAppCommand { get; }
        
        private void CloseAppCommandExecute(object p)
        {
            Application.Current.Shutdown();
        }

        private bool CanCloseAppCommandExecute(object p) => true;
        

        #endregion
        public MainWindowViewModel() 
        {
            CloseAppCommand = new LambdaCommand(CloseAppCommandExecute, CanCloseAppCommandExecute);
        
        }
    }
}
