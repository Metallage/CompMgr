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
using System.Windows.Threading;
using System.Threading;
using System.Data;


using CompMgr.ViewModel;


namespace CompMgr
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class EditTableWindow : Window
    {


        private DataTable software;
        private DataTable user;
        private DataTable computer;



        private bool changed = false;

        private EditWindowVM ewvm;

        private Logic core;

        public EditTableWindow(Logic logic)
        {
            this.core = logic;
            InitializeComponent();
            software = core.GetSoftware();
            user = core.GetUserDT();
            computer = core.GetComputerDT();
            EditDG.Visibility = Visibility.Visible;

        }





        public EditTableWindow(EditWindowVM ewvm)
        {
            this.ewvm = ewvm;
            InitializeComponent();
        }


        private void Launch()
        {
            ewvm.DataUpdate += Ewvm_DataUpdate;
            ewvm.Start();
        }

        private void Ewvm_DataUpdate()
        {
           Dispatcher.BeginInvoke(DispatcherPriority.Normal,(ThreadStart)delegate() 
           {
               DataContext = null;
               DataContext = ewvm;
               EditListView.Visibility = Visibility.Visible;
           });
        }

        /// <summary>
        /// Устанавливает вид и источник данных для ListView и настравивает на просмотр компьютеров
        /// </summary>
        private void SetCompView()
        {
            GridView compView = EditListView.FindResource("CompView") as GridView;
            Binding compBinding = new Binding("CompList");
            EditListView.View = compView;
            EditListView.SetBinding(ListView.ItemsSourceProperty, compBinding);
        }

        /// <summary>
        /// Устанавливает вид и источник данных для ListView и настравивает на просмотр пользователей
        /// </summary>
        private void SetUserView()
        {
            GridView userView = EditListView.FindResource("UserView") as GridView;
            Binding userBinding = new Binding("UserList");
            EditListView.View = userView;
            EditListView.SetBinding(ListView.ItemsSourceProperty, userBinding);
        }


        /// <summary>
        /// Устанавливает вид и источник данных для ListView и настравивает на просмотр ПО
        /// </summary>
        private void SetSoftView()
        {
            GridView softView = EditListView.FindResource("SoftView") as GridView;
            Binding softBinding = new Binding("SoftwareList");
            EditListView.View = softView;
            EditListView.SetBinding(ListView.ItemsSourceProperty, softBinding);
        }

        #region Привязываем таблицы к DataGrid

        /// <summary>
        /// Настраивает DataGrid на отображение таблицы Software 
        /// </summary>
        private void BindSoft()
        {
            foreach (DataGridColumn dc in EditDG.Columns)
            {
                switch (dc.Header.ToString().ToLower())
                {
                    case "наименование по":
                        dc.Visibility = Visibility.Visible;
                        continue;
                    case "актуальная версия":
                        dc.Visibility = Visibility.Visible;
                        continue;
                    default:
                        dc.Visibility = Visibility.Collapsed;
                        continue;
                }
            }
            

            EditDG.ItemsSource = software.DefaultView;

        }


        private void BindUsers()
        {
            foreach (DataGridColumn dc in EditDG.Columns)
            {
                switch (dc.Header.ToString())
                {
                    case "Пользователь":
                        dc.Visibility = Visibility.Visible;
                        continue;
                    case "Номер телефона":
                        dc.Visibility = Visibility.Visible;
                        continue;
                    default:
                        dc.Visibility = Visibility.Collapsed;
                        continue;
                }
            }


            EditDG.ItemsSource = user.DefaultView;
        }

        //private void BindDivision()
        //{
        //    foreach (DataGridColumn dc in EditDG.Columns)
        //    {
        //        switch (dc.Header.ToString())
        //        {
                    
        //            case "Подразделение":
        //                dc.Visibility = Visibility.Visible;
        //                continue;
                  
        //            default:
        //                dc.Visibility = Visibility.Collapsed;
        //                continue;
        //        }
        //    }
        //    //divisionList = core.GetDivision();
        //    EditDG.ItemsSource = divisionList;
        //}


        private void BindComp()
        {
            foreach (DataGridColumn dc in EditDG.Columns)
            {
                switch(dc.Header.ToString().ToLower())
                {
                    case "имя компьютера":
                        dc.Visibility = Visibility.Visible;
                        continue;
                    case "ip адрес":
                        dc.Visibility = Visibility.Visible;
                        continue;
                    default:
                        dc.Visibility = Visibility.Collapsed;
                        continue;
                }
            }

            EditDG.ItemsSource = computer.DefaultView;
        }

      

        #endregion


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
                       // BindSoft();
                        SetSoftView();
                        break;
                    case 1:
                        // BindUsers();
                        SetUserView();
                        break;
                    case 2:
                        //BindComp();
                        SetCompView();
                        break;

                }
            }
        }


        public DataSet TableSet
        { get; set; }

        public DataBaseHelper logica
        { get; set; }

        private void EditDG_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            changed = true;
            RollbackButton.IsEnabled = true;
            SaveButton.IsEnabled = true;


        }

        private void RollbackButton_Click(object sender, RoutedEventArgs e)
        {
            EditDG.ItemsSource = null;
            switch (BaseSelect.SelectedIndex)
            {
                case 0:
                    software.RejectChanges();
                    BindSoft();
                    changed = false;
                    RollbackButton.IsEnabled = false;
                    SaveButton.IsEnabled = false;
                    break;
                case 1:
                    user.RejectChanges();
                    BindUsers();
                    changed = false;
                    RollbackButton.IsEnabled = false;
                    SaveButton.IsEnabled = false;
                    break;
                case 2:
                    computer.RejectChanges();
                    BindComp();
                    changed = false;
                    RollbackButton.IsEnabled = false;
                    SaveButton.IsEnabled = false;
                    break;
            }
            DgRefreshImensSourse();
        }

        private void DgRefreshImensSourse()
        {

        }

        private void BaseSelect_DropDownOpened(object sender, EventArgs e)
        {
            if (changed)
            {

                MessageBox.Show("Необходимо сохранить или откатить изменения", "Таблица изменена");

            }
        }

        private void EditDG_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            changed = true;
            RollbackButton.IsEnabled = true;
            SaveButton.IsEnabled = true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            core.Save();
            changed = false;
            RollbackButton.IsEnabled = false;
            SaveButton.IsEnabled = false;


        }

        private void StreamInputButton_Click(object sender, RoutedEventArgs e)
        {
            string tableToInput = String.Empty;
            switch (BaseSelect.SelectedIndex)
            {
                case 0:
                    tableToInput = "software";
                    break;

                case 1:
                    tableToInput = "user";
                    break;

                case 2:
                    tableToInput = "computer";
                    break;
            }

            StreamAddWindow inputWindow = new StreamAddWindow(tableToInput);


            if (inputWindow.ShowDialog()==true)
            {
                changed = true;
                RollbackButton.IsEnabled = true;
                SaveButton.IsEnabled = true;

                switch(tableToInput)
                {
                    case "software":
                        core.ParseSoftware(inputWindow.InputData);
                        
                        break;
                    case "user":
                        core.ParseUser(inputWindow.InputData);

                        break;
                    case "computer":
                        core.ParseComputer(inputWindow.InputData);
                        break;
                }
                EditDG.Items.Refresh();

            }
        }



        private void EditDG_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Сложная магия по поиску нужной строки
            DependencyObject obj = (DependencyObject)e.OriginalSource; //Получаем элемент куда ткнули мышкой
            while((obj!=null)&&!(obj is DataGridCell)) //Перебираем последовательно его родителей пока не упрёмся в ячейку датагрида (ну или в нулл)
            {
                obj = VisualTreeHelper.GetParent(obj);
            }

            if (obj == null) //Мы за безопасный нулл
                return;

            if(obj is DataGridCell) //Если яччейка найдена
            {
                while((obj!=null)&&!(obj is DataGridRow)) //Перебираем всех родителей пока не найдём строку
                {
                    obj = VisualTreeHelper.GetParent(obj);
                }

                if (obj == null)
                    return;

                DataGridRow dr = obj as DataGridRow; //искомая строка
                string ert = dr.Item.ToString(); //Тут можно обратиться непосредсвенно к итему.
                EditDG.SelectedItem = dr.Item;

                ContextMenu rowMenu = this.FindResource("RowContextMenu") as ContextMenu;
                dr.ContextMenu = rowMenu;
                rowMenu.IsOpen = true;
                

            }
            //ContextMenu rowMenu = EditDG.ContextMenu;
            //string src = e.OriginalSource.ToString();
            //string tp = sender.GetType().ToString();
            //DataGrid send = sender as DataGrid;
            //string itm = send.ToString();
            //rowMenu.PlacementTarget = send;
            //rowMenu.IsOpen = true;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            string src = sender.GetType().ToString();

        }

        private void EditDG_BeginningEdit_1(object sender, DataGridBeginningEditEventArgs e)
        {
            changed = true;
            SaveButton.IsEnabled = true;
            RollbackButton.IsEnabled = true;
        }

        private void EditDG_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete)
            {
                changed = true;
                SaveButton.IsEnabled = true;
                RollbackButton.IsEnabled = true;
            }

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            if (changed)
            {
                MessageBox.Show("Необходимо сохранить или откатить изменения", "Таблица изменена");
            }
            else
                this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Launch();
        }
    }
}
