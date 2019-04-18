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
using CompMgr.ViewModel;

namespace CompMgr
{
    /// <summary>
    /// Логика взаимодействия для AddDivisionUserControl.xaml
    /// </summary>
    public partial class AddDivisionUserControl : UserControl
    {
        private EditWindowVM model;

        public EditWindowVM Model
        {
            get
            {
                return model;
            }
            set
            {
                model = value;
            }
        }
        public AddDivisionUserControl()
        {
            //this.model = model;
            InitializeComponent();
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }
    }
}
