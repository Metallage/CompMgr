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
        private SettingsHelper settings = new SettingsHelper();
        private Logica baseLogic;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            baseLogic = new Logica();
            ErrorMessageHelper initialDB = baseLogic.InitialDB();
            if(initialDB.HasErrors)
            {
                MessageBox.Show(initialDB.ErrorText);
            }
            gridField.ItemsSource = baseLogic.LogicDataSet.Tables["Software"].DefaultView;

            EditTableWindow editSoft = new EditTableWindow(this.baseLogic);

            editSoft.Show();
        }
    }
}
