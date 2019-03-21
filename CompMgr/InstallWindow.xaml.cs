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
using System.Threading;
using CompMgr.ViewModel;


namespace CompMgr
{
    /// <summary>
    /// Логика взаимодействия для InstallWindow.xaml
    /// </summary>
    public partial class InstallWindow : Window
    {

        InstallWindowVM iwvm;


        public InstallWindow(InstallWindowVM iwvm)
        {

            this.iwvm = iwvm;
            InitializeComponent();
            this.iwvm.DataUpdate += Iwvm_DataUpdate;
            this.iwvm.RunModel();
        }

        private void Iwvm_DataUpdate()
        {
            Dispatcher.BeginInvoke((ThreadStart)delegate ()
            {
                DataContext = this.iwvm;
                MainInstallView.Items.Refresh();
            });
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            
            //core.SaveInstall(source);
            ////core.Save();
            //Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void AcceptBut_Click(object sender, RoutedEventArgs e)
        {
            iwvm.Save();
        }

        private void RollbackBut_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}
