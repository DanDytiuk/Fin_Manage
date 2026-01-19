using FinManage.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
