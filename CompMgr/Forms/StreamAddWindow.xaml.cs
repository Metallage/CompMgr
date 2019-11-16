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
    /// Логика взаимодействия для StreamAddWindow.xaml
    /// </summary>
    public partial class StreamAddWindow : Window
    {
        private string tableName;

        public string InputData {
            get
            {
                return InputTextBox.Text;
            }
        }

        public StreamAddWindow(string tableName)
        {
            this.tableName = tableName;
            InitializeComponent();
            HeaderBinding();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }



        private void HeaderBinding()
        {
            string headertext = "";

            switch(tableName)
            {
                case "computer":
                    headertext = "Введите информацию о компьютерах";
                    break;
                case "user":
                    headertext = "Введите информацию по пользователях";
                    break;
                case "software":
                    headertext = "введите информацию о ПО";
                    break;
            }
            HeaderTextBlock.Text = headertext;
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
