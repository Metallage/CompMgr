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
using System.Data;

namespace CompMgr
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class EditTableWindow : Window
    {
        enum Tables : int
        {
            Software = 0,
            Users = 1,
            Division = 2,
            Computer = 3,
            Install = 4
        }

        private bool changed = false;

        public EditTableWindow()
        {
            InitializeComponent();
        }


        private void BaseSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (changed)
            {

            }
            else
            {
                switch (BaseSelect.SelectedIndex)
                {
                    case 0:
                        EditDG.ItemsSource = TableSet.Tables["Software"].DefaultView;
                        EditDG.AutoGenerateColumns = true;
                        EditDG.IsEnabled = true;
                        break;
                    case 1:
                        EditDG.ItemsSource = TableSet.Tables["Users"].DefaultView;
                        EditDG.AutoGenerateColumns = true;
                        EditDG.IsEnabled = true;
                        break;
                    case 2:
                        EditDG.ItemsSource = TableSet.Tables["Division"].DefaultView;
                        EditDG.AutoGenerateColumns = true;
                        EditDG.IsEnabled = true;
                        break;
                    case 3:
                        EditDG.ItemsSource = TableSet.Tables["Computer"].DefaultView;
                        EditDG.AutoGenerateColumns = true;
                        EditDG.IsEnabled = true;
                        break;
                    case 4:
                        EditDG.ItemsSource = TableSet.Tables["Install"].DefaultView;
                        EditDG.AutoGenerateColumns = true;
                        EditDG.IsEnabled = true;
                        break;
                }
            }
        }


        public DataSet TableSet
        { get; set; }



    }
}
