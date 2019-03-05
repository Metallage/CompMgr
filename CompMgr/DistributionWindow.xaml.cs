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

        DistributionViewModel dvm;

        public DistributionWindow(Logic core)
        {
            this.core = core;
            InitializeComponent();
             dvm = new DistributionViewModel();
            DistributedView distributed = new DistributedView();
            distributed.InputMe(core.GetDistribution());
            dvm.SourceDistr = distributed;
            dvm.UserSource = core.GetUsersNoComp();
            dvm.CompSource = core.GetComputersNoUser();
            DataContext = dvm;
            
        }

        private void Bind()
        {
            SelectUser.ItemsSource = userSource;
            SelectComp.ItemsSource = computerSource;
            DistributionView.ItemsSource = source;
        }

        private void SaveAndExit_Click(object sender, RoutedEventArgs e)
        {
            core.SaveDistribution(source);
            Close();
        }

        private void ExitNoSave_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            if ((SelectComp.SelectedItem != null) && (SelectUser.SelectedItem != null))
            {
                User currentUser = (User)SelectUser.SelectedItem;
                Computer currentComp = (Computer)SelectComp.SelectedItem;
                Distribution newDistribution = new Distribution();
                newDistribution.Id = -1;
                newDistribution.ComputerID = currentComp.Id;
                newDistribution.NsName = currentComp.NsName;
                newDistribution.UserFio = currentUser.UserFio;
                newDistribution.UserID = currentUser.Id;

                dvm.SourceDistr.Add(newDistribution);


                dvm.UserSource.Remove(currentUser);
                dvm.CompSource.Remove(currentComp);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Bind();
        }

        private void DeleteItem_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }
    }
}
