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

        private void CommandBinding_CanAddExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if ((sender != null) && (sender.GetType() == typeof(Button)))
            {
                if (NewDivisionTB.Text.Count() > 0)
                {
                    string newDivisionName = NewDivisionTB.Text;
                    e.CanExecute = model.IsDivisionUniq(newDivisionName);
                }
                else
                    e.CanExecute = false;
                
            }
            else
                e.CanExecute = false;

            
        }

        private void CommandBinding_AddExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            string divisionName = NewDivisionTB.Text;
            model.AddDivision(divisionName);
        }
    }
}
