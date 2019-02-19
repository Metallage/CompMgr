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
            //baseLogic = new Logica(settings);

            //gridField.ItemsSource = baseLogic.LogicDataSet.Tables["Software"].DefaultView;
            //settings.AddBase("РТП(ЦЭД)","RTPCED.sqlite");
            //settings.AddBase("МТП", "MTP.sqlite");
            //settings.AddBase("РТП", "RTP.sqlite");
            //settings.WriteSettings();
            //SettingsWindow settings = new SettingsWindow();
            //settings.CurrentSettings = this.settings;
            //settings.Show();

            baseLogic = new Logica();
            ErrorMessageHelper initialDB = baseLogic.InitialDB();
            if(initialDB.HasErrors)
            {
                MessageBox.Show(initialDB.ErrorText);
            }
            gridField.ItemsSource = baseLogic.LogicDataSet.Tables["Software"].DefaultView;

            EditTableWindow editSoft = new EditTableWindow(this.baseLogic);
            editSoft.FormDataTables(baseLogic.LogicDataSet);
            editSoft.Show();
        }
    }
}
