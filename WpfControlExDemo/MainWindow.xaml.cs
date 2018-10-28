﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControlEx.Controls;

namespace WpfControlExDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : WindowEx
    {

        public MainWindow()
        {
            InitializeComponent();
        }
        

        private void btnChildWindow_Click(object sender, RoutedEventArgs e)
        {
            ChildWindow childWindow = new ChildWindow();
            childWindow.Owner = this;
            childWindow.ShowInTaskbar = false;
            childWindow.ShowDialog();
        }

        private void btnMessageBoxEx_Click(object sender, RoutedEventArgs e)
        {
            string msg = "WindowStartupLocation = WindowStartupLocation.CenterOwner;";
            msg += "WindowStartupLocation = WindowStartupLocation.CenterOwner;";
            msg += "WindowStartupLocation = WindowStartupLocation.CenterOwner;";
            msg += "WindowStartupLocation = WindowStartupLocation.CenterOwner;";
            msg += "WindowStartupLocation = WindowStartupLocation.CenterOwner;";
            MessageBoxEx.Show(this, msg, "TestTitle");
        }
    }
}
