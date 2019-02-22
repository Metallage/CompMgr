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

namespace CompMgr
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // private SettingsHelper settings = new SettingsHelper();
        //private DataBaseHelper baseLogic;

        Logic core = new Logic();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            ErrorMessageHelper start = core.Start();
            if (start.HasErrors)
            {
                MessageBox.Show(start.ErrorText);
            }
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {


            gridField.ItemsSource = core.GetDiv().DefaultView;

            //EditTableWindow editSoft = new EditTableWindow(this.baseLogic);

            //editSoft.Show();
        }

        private void UpdButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateWindow upd = new UpdateWindow(core);
            upd.Show();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
           // baseLogic.AddSomeData();
        }
    }
}
