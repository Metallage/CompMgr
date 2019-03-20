﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using CompMgr.ViewModel;


namespace CompMgr
{
    /// <summary>
    /// Логика взаимодействия для InstallWindow.xaml
    /// </summary>
    public partial class InstallWindow : Window
    {
        ObservableCollection<Install> source;
        Logic core;

        InstallWindowVM iwvm;


        public InstallWindow(InstallWindowVM iwvm)
        {
            DataContext = this.iwvm;
            InitializeComponent();
            this.iwvm.DataUpdate += Iwvm_DataUpdate;
        }

        private void Iwvm_DataUpdate()
        {
            throw new NotImplementedException();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            core.SaveInstall(source);
            //core.Save();
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            source = core.GetInstall();
            MainInstallView.ItemsSource = source;
        }
    }
}
