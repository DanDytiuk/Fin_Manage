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
            CloseAppCommand = new LambdaCommand(CloseAppCommandExecute, CanCloseAppCommandExecute);
            OpenSettingsCommand = new LambdaCommand(OpenSettingsCommandExecute, CanOpenSettingsCommandExecute);
            OpenLimitsCommand = new LambdaCommand(OpenLimitsCommandExecute, CanOpenLimitsCommandExecute);
        }

    }
}
