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
            DataContext = this.dvm;
            InitializeComponent();
            this.dvm.UpdateData += Dvm_UpdateData;
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
            dvm.AllSaved += Dvm_AllSaved;
            dvm.SaveDistribution();

        }

        private void Dvm_AllSaved()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
             {
                 DialogResult = true;
             });
        }

        private void ExitNoSave_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            if ((SelectComp.SelectedItem != null) && (SelectUser.SelectedItem != null))
            {
                object currentUser = SelectUser.SelectedItem;
                object currentComp = SelectComp.SelectedItem;
                DistributionVM newDistribution = new DistributionVM();
   

                dvm.AddDistribution(newDistribution, currentComp, currentUser);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Bind();
        }

        private void DeleteItem_Executed(object sender, ExecutedRoutedEventArgs e)
        {
          dvm.ExecuteDeleteCommand(e.Parameter.ToString());
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
            DistributionVM newDistribution = e.Parameter as DistributionVM;
            if ((SelectComp.SelectedItem != null) && (SelectUser.SelectedItem != null))
            {
                object currentUser = (object)SelectUser.SelectedItem;
                object currentComp = (object)SelectComp.SelectedItem;

                dvm.AddDistribution(newDistribution, currentComp, currentUser);
            }

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

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            DistributionVM deletedItem = ((Button)sender).DataContext as DistributionVM;
            dvm.DeleteDistribution(deletedItem);
        }
    }
}
