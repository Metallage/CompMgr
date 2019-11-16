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
using CompMgr.ViewModel;

namespace CompMgr
{
    /// <summary>
    /// Логика взаимодействия для SelectSoftWindow.xaml
    /// </summary>
    public partial class SelectSoftWindow : Window
    {
        SelectSoftViewModel myViewModel;
        public SelectSoftWindow(SelectSoftViewModel vm)
        {
            InitializeComponent();
            if(vm!=null)
            {
                myViewModel = vm;
                DataContext = myViewModel;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void SoftSelectorCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string newSoftName = SoftSelectorCB.SelectedItem.ToString();
            myViewModel.SelectedSoftName = newSoftName;
        }
    }
}
