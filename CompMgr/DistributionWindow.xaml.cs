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
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading;
using CompMgr.ViewModel;


namespace CompMgr
{
    /// <summary>
    /// Логика взаимодействия для DistributionWindow.xaml
    /// </summary>
    public partial class DistributionWindow : Window
    {

        DistributionViewModel dvm;

        public static RoutedCommand DeleteItemCommand = new RoutedCommand();

        public static RoutedCommand AddItemCommand = new RoutedCommand();


        public DistributionWindow(DistributionViewModel dvm)
        {
            this.dvm = dvm;
            InitializeComponent();
            this.dvm.UpdateData += Dvm_UpdateData;
            DataContext = dvm;

            //DistributedView distributed = new DistributedView();
            //distributed.InputMe(core.GetDistribution());
            //dvm.SourceDistr = distributed;
            //dvm.UserSource = core.GetUsersNoComp();
            //dvm.CompSource = core.GetComputersNoUser();
            //DataContext = dvm;


        }

        private void Dvm_UpdateData()
        {
            Bind();
        }

        private void Bind()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
             {
                 dvm.ImportData();
                 DataContext = dvm;

             });
        }

        private void SaveAndExit_Click(object sender, RoutedEventArgs e)
        {
           // core.SaveDistribution(dvm.SourceDistr);
            Close();
        }

        private void ExitNoSave_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            //if ((SelectComp.SelectedItem != null) && (SelectUser.SelectedItem != null))
            //{
            //    User currentUser = (User)SelectUser.SelectedItem;
            //    Computer currentComp = (Computer)SelectComp.SelectedItem;
            //    Distribution newDistribution = new Distribution();
            //    newDistribution.Id = -1;
            //    newDistribution.ComputerID = currentComp.Id;
            //    newDistribution.NsName = currentComp.NsName;
            //    newDistribution.UserFio = currentUser.UserFio;
            //    newDistribution.UserID = currentUser.Id;

            //    dvm.SourceDistr.Add(newDistribution);


            //    dvm.UserSource.Remove(currentUser);
            //    dvm.CompSource.Remove(currentComp);
            //}
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Bind();
        }

        private void DeleteItem_Executed(object sender, ExecutedRoutedEventArgs e)
        {
          //  dvm.ExecuteDeleteCommand(sender, e);
        }

        private void CanExecuteDeleteItem(object sender, CanExecuteRoutedEventArgs e)
        {
            Control target = e.Source as Control;

            if (target != null)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void AddItem_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //Distribution newDistribution = e.Parameter as Distribution;
            //if ((SelectComp.SelectedItem != null) && (SelectUser.SelectedItem != null))
            //{
            //    User currentUser = (User)SelectUser.SelectedItem;
            //    Computer currentComp = (Computer)SelectComp.SelectedItem;

            //    dvm.SourceDistr.Add(newDistribution);


            //    dvm.UserSource.Remove(currentUser);
            //    dvm.CompSource.Remove(currentComp);
            //}

        }

        private void CanExecuteAddCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            Control target = e.Source as Control;

            if (target != null)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

    }
}
