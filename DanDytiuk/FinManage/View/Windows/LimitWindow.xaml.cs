using FinManage.ViewModels;
using System.Windows;

namespace FinManage.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для LimitWindow.xaml
    /// </summary>
    public partial class LimitWindow : Window
    {
        public LimitWindow()
        {
            InitializeComponent();
            var vm = new LimitWindowsViewModel
            {
                CloseAction = Close
            };

            DataContext = vm;
        }
    }
}
