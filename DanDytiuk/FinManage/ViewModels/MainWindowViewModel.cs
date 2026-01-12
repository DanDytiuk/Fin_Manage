using FinManage.Infrastructure.Commands;
using FinManage.View.Windows;
using FinManage.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace FinManage.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        #region Команды
        public ICommand CloseAppCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        
        private void CloseAppCommandExecute(object p)
        {
            Application.Current.Shutdown();
        }
        private void OpenSettingsCommandExecute(object p) 
        {
            var window = new SettingsWindow();
            window.ShowDialog();
        }
        private bool CanCloseAppCommandExecute(object p) => true;
        private bool CanOpenSettingsCommandExecute(object p) => true;

        #endregion
        public MainWindowViewModel() 
        {
            CloseAppCommand = new LambdaCommand(CloseAppCommandExecute, CanCloseAppCommandExecute);
            OpenSettingsCommand = new LambdaCommand(OpenSettingsCommandExecute, CanOpenSettingsCommandExecute);

        }
    }
}
