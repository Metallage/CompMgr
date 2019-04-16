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

        private void ClearNewTB()
        {
            NewSoftNameTB.Text = String.Empty;
            NewSoftVerTB.Text = String.Empty;

            NewUserFioTB.Text = String.Empty;
            NewUserTelTB.Text = String.Empty;
        }

        private void CollapseAll()
        {
            ClearNewTB();


            SoftListBox.Visibility = Visibility.Collapsed;
            NewSOftGrid.Visibility = Visibility.Collapsed;

            UserListBox.Visibility = Visibility.Collapsed;
            NewUserGrid.Visibility = Visibility.Collapsed;
        }

        private void SelectUser()
        {
            CollapseAll();
            NewUserGrid.Visibility = Visibility.Visible;
            UserListBox.Visibility = Visibility.Visible;
        }

        private void SelectSoft()
        {

            CollapseAll();

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






        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox tableSelector = sender as ComboBox;
            string selected = tableSelector.SelectedValue.ToString();

            switch(selected)
            {
                case "ПО":
                    SelectSoft();
                    break;
                case "Пользователи":
                    SelectUser();
                    break;
                default:
                    CollapseAll();
                    break;


            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetStarted();
        }



        #region Выполнение комманд


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
            model.RemoveSoft(delSoft);
        }

        private void CommandBinding_UserDelExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            UserVM delUser = ((Button)sender).DataContext as UserVM;
            model.RemoveUser(delUser);
        }



        private void CommandBinding_NewSoftExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            string softName = NewSoftNameTB.Text;
            string softVersion = NewSoftVerTB.Text;

            model.AddSoft(softName, softVersion);

            ClearNewTB();

        }



        private void CommandBinding_NewUserExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            string userFio = NewUserFioTB.Text;
            string userTel = NewUserTelTB.Text;

            model.AddUser(userFio, userTel);

            ClearNewTB();
        }

        #endregion


        #region Проверка выполнения комманд

        /// <summary>
        /// Проверка на возможность удаления по
        /// </summary>
        /// <param name="sender">Вызывающая кнопка удаления</param>
        /// <param name="e">Аргументы</param>
        private void CommandBinding_CanSoftDelExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //Если вызывающий контрол кнопка и не пустой
            if ((sender != null) && (sender.GetType() == typeof(Button)))
            {
                //Проверка есть ли связанный с кнопкой объект типа SoftVM
                if ((((Button)sender).DataContext != null) && ((((Button)sender).DataContext.GetType() == typeof(SoftVM))))
                    e.CanExecute = true; //Команду можно выполнить
                else
                    e.CanExecute = false; //Команду нельзя выполнить
            }
            else
                e.CanExecute = false; //Команду нельзя выполнить

        }

        /// <summary>
        /// Проверка на возможность удаления пользователя
        /// </summary>
        /// <param name="sender">Вызывающая кнопка удаления</param>
        /// <param name="e">Аргументы</param>
        private void CommandBinding_CanUserDelExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //Если вызывающий контрол кнопка и не пустой
            if ((sender != null) && (sender.GetType() == typeof(Button)))
            {
                //Проверка есть ли связанный с кнопкой объект типа UserVM
                if ((((Button)sender).DataContext != null) && ((((Button)sender).DataContext.GetType() == typeof(UserVM))))
                    e.CanExecute = true; //Команду можно выполнить
                else
                    e.CanExecute = false; //Команду нельзя выполнить
            }
            else
                e.CanExecute = false; //Команду нельзя выполнить
        }


        /// <summary>
        /// Проверка на возможность выполнения команды добавления нового ПО
        /// </summary>
        /// <param name="sender">Вызывающий контрол (должен быть Button)</param>
        /// <param name="e">Аргументы</param>
        private void CommandBinding_CanNewSoftExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //Если тип вызывающего контрола кнопка и он существует
            if ((sender != null) && (sender.GetType() == typeof(Button)))
            {
                //Проверяем, что поля с версией и названием ПО не пустые 
                if ((NewSoftNameTB.Text.Count() > 0) && (NewSoftVerTB.Text.Count() > 0))
                    e.CanExecute = true; //Команду можно выполнить
                else
                    e.CanExecute = false; //Команду нельзя выполнить
            }
            else
                e.CanExecute = false; //Команду нельзя выполнить
        }


        /// <summary>
        /// Проверка на возможность выполнения команды добавления нового пользователя
        /// </summary>
        /// <param name="sender">Вызывающий контрол (должен быть Button)</param>
        /// <param name="e">Аргументы</param>
        private void CommandBinding_CanNewUserExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //Если тип вызывающего контрола кнопка и он существует
            if ((sender != null) && (sender.GetType() == typeof(Button)))
            {
                //Проверяем, что поля с версией и названием ПО не пустые 
                if ((NewUserFioTB.Text.Count() > 0) && (NewUserTelTB.Text.Count() > 0))
                    e.CanExecute = true; //Команду можно выполнить
                else
                    e.CanExecute = false; //Команду нельзя выполнить
            }
            else
                e.CanExecute = false; //Команду нельзя выполнить
        }


        #endregion


    }
}
