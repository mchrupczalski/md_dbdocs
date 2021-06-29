﻿using md_dbdocs.app.Views;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace md_dbdocs.app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Services.ViewNavigationService.ViewNav = this;
            Services.ViewNavigationService.Switch(new ConfigView());
        }

        public void Navigate(UserControl newView)
        {
            this.Content = newView;
        }
    }
}
