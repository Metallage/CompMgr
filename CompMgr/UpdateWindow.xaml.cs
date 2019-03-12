using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CompMgr.ViewModel;
using System.Threading;


namespace CompMgr
{
    /// <summary>
    /// Логика взаимодействия для UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        UpdateFormViewModel updModel;


        public static RoutedCommand UpChanged = new RoutedCommand();

        public UpdateWindow(UpdateFormViewModel updModel)
        {
            this.updModel = updModel;
            //gridSource = core.GetUpdates("Ордер");

            InitializeComponent();
            DataContext = this.updModel;
            this.updModel.CoreReady += UpdModel_CoreReady;
            //this.updModel.onRefresh += UpdModel_onRefresh;
        }

        private void UpdModel_CoreReady()
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {

                updModel.Bind();
               // UpdatesList.Items.Refresh();
            }
);
        }

        private void UpdModel_onRefresh()
        {


        }

        private void ExecuteUpChange(object sender, ExecutedRoutedEventArgs e)
        {
            updModel.Changed(e.Parameter.ToString());
        }

        private void CanExecuteUpChange(object sender, CanExecuteRoutedEventArgs e)
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Info.Text = "Обновления для Ордер";
            //UpdGrid.ItemsSource = gridSource;
        }

        private void SaveBt_Click(object sender, RoutedEventArgs e)
        {
            updModel.SaveChanges();
        }
    }
}
