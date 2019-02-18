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

namespace CompMgr
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private SettingsHelper currentSettings;

        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(currentSettings!=null)
            {
                SoftwarePathInput.Text = currentSettings.SoftwareBase;
            }
            BasesDataGrid.ItemsSource = currentSettings.Bases;
        }

        public SettingsHelper CurrentSettings
        {
            get
            {
                return currentSettings;
            }
            set
            {
                currentSettings = value;
            }
        }
    }
}
