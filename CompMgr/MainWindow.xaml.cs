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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Threading;

namespace CompMgr
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // private SettingsHelper settings = new SettingsHelper();
        //private DataBaseHelper baseLogic;

        private CompleteData completeData = new CompleteData();

        Logic core = new Logic();

        public MainWindow()
        {
            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            RunCore();

            
        }

        private void RunCore()
        {
            ErrorMessageHelper start = core.Start();
            if (start.HasErrors)
            {
                MessageBox.Show(start.ErrorText);
            }
            completeData.GetData(core.GetCompleteTable());
            gridField.ItemsSource = completeData;

        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {


        }

        private void UpdButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateWindow upd = new UpdateWindow(core);
            upd.Show();
        }



        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

            EditTableWindow editTable = new EditTableWindow(core);
            editTable.Show();
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            InstallWindow installWindow = new InstallWindow(core);
            installWindow.Show();
        }
    }
}
