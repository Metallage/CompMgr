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
using System.Threading;
using CompMgr.ViewModel;

namespace CompMgr
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private EditWindowVM model;

        public EditWindow(EditWindowVM model)
        {
            this.model = model;
            InitializeComponent();
           // DataContext = this.model;
        }

        private void SelectSoft()
        {
            SoftListBox.Visibility = Visibility.Visible;
            NewSOftGrid.Visibility = Visibility.Visible;
        }

        private void GetStarted()
        {
            model.DataUpdate += Model_DataUpdate;
            model.Start();
        }

        private void Model_DataUpdate()
        {
            Dispatcher.BeginInvoke((ThreadStart)delegate ()
            {
                DataContext = null;
                DataContext = model;
            });
        }

        private void CommandBinding_CloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void CommandBinding_SaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CommandBinding_SoftDelExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SoftVM delSoft = ((Button)sender).DataContext as SoftVM;
        }

        /// <summary>
        /// Проверка на возможность удаления по
        /// </summary>
        /// <param name="sender">Вызывающая кнопка удаления</param>
        /// <param name="e">Аргументы</param>
        private void CommandBinding_CanSoftDelExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if ((sender != null) && (sender.GetType() == typeof(Button)))
            {
                if ((((Button)sender).DataContext != null) && ((((Button)sender).DataContext.GetType() == typeof(SoftVM))))
                    e.CanExecute = true;
                else
                    e.CanExecute = false;
            }
            else
                e.CanExecute = false;

        }



        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox tableSelector = sender as ComboBox;
            string selected = tableSelector.SelectedValue.ToString();

            if (tableSelector.SelectedItem.ToString() == "ПО")
            {
                SelectSoft();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetStarted();
        }

        private void CommandBinding_NewSoftExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CommandBinding_CanNewSoftExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if ((sender != null) && (sender.GetType() == typeof(EditWindow)))
            {
                if ((NewSoftNameTB.Text.Count() > 0) && (NewSoftVerTB.Text.Count() > 0))
                    e.CanExecute = true;
                else
                    e.CanExecute = false;
            }
            else
                e.CanExecute = false;
        }

        private void NewSoftNameTB_TextChanged(object sender, TextChangedEventArgs e)
        {
          //  CommandManager.InvalidateRequerySuggested();
        }

        private void NewSoftVerTB_TextChanged(object sender, TextChangedEventArgs e)
        {
           // CommandManager.InvalidateRequerySuggested();
        }
    }
}
