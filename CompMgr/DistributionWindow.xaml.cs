using System;
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
using System.Data;

namespace CompMgr
{
    /// <summary>
    /// Логика взаимодействия для DistributionWindow.xaml
    /// </summary>
    public partial class DistributionWindow : Window
    {
        private ObservableCollection<Distribution> source;
        private Logic core;
        private ObservableCollection<User> userSource;
        private ObservableCollection<Computer> computerSource;

        public DistributionWindow(Logic core)
        {
            this.core = core;
            InitializeComponent();
            source = core.GetDistribution();
            userSource = core.GetUsers();
            computerSource = core.GetComputers();
        }

        private void Bind()
        {
            SelectUser.ItemsSource = userSource;
            SelectComp.ItemsSource = computerSource;
            DistributionView.ItemsSource = source;
        }

        private void SaveAndExit_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ExitNoSave_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Bind();
        }
    }
}
