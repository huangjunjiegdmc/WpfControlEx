using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using WpfControlEx.Controls;
using WpfControlEx.Controls.Helper;

namespace WpfControlExDemo
{
    /// <summary>
    /// ChildWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChildWindow : WindowEx
    {
        public ChildWindow()
        {
            InitializeComponent();
        }

        private void WindowEx_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

    }
}
