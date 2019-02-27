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

       // private DataTable software;
        //private DataTable users;
        //private DataTable division;
        //private DataTable computer;
        //private DataTable install;

        private HashSet<Computer> compList = new HashSet<Computer>();
        private HashSet<Software> softList = new HashSet<Software>();
        private HashSet<User> userList = new HashSet<User>();
        private HashSet<Division> divisionList = new HashSet<Division>();

        //Dictionary<long, string> userNames = new Dictionary<long, string>();
        //Dictionary<long, string> divNames = new Dictionary<long, string>();



        private bool changed = false;


        private Logic core;

        public EditTableWindow(Logic logic)
        {
            this.core = logic;
            InitializeComponent();
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

            softList = core.GetSoftware();
            EditDG.ItemsSource = softList;

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
            userList = core.GetUsers();
            EditDG.ItemsSource = userList;
        }

        private void BindDivision()
        {
            foreach (DataGridColumn dc in EditDG.Columns)
            {
                switch (dc.Header.ToString())
                {
                    
                    case "Подразделение":
                        dc.Visibility = Visibility.Visible;
                        continue;
                  
                    default:
                        dc.Visibility = Visibility.Collapsed;
                        continue;
                }
            }
            divisionList = core.GetDivision();
            EditDG.ItemsSource = divisionList;
        }


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
                    case "подразделение":
                        dc.Visibility = Visibility.Visible;
                        continue;
                    case "пользователь":
                        dc.Visibility = Visibility.Visible;
                        continue;
                    default:
                        dc.Visibility = Visibility.Collapsed;
                        continue;
                }
            }

           
            
            compList = core.GetComputers();
            EditDG.ItemsSource = compList;
           


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
                        BindSoft();
                        break;
                    case 1:
                        BindUsers();
                        break;
                    case 2:
                        BindDivision();
                        break;
                    case 3:
                        BindComp();
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
                    softList = core.GetSoftware();
                    EditDG.ItemsSource = softList;
                    changed = false;
                    RollbackButton.IsEnabled = false;
                    SaveButton.IsEnabled = false;
                    break;
                case 1:
                    userList = core.GetUsers();
                    EditDG.ItemsSource = userList;
                    changed = false;
                    RollbackButton.IsEnabled = false;
                    SaveButton.IsEnabled = false;
                    break;
                case 2:
                    divisionList = core.GetDivision();
                    EditDG.ItemsSource = divisionList;
                    changed = false;
                    RollbackButton.IsEnabled = false;
                    SaveButton.IsEnabled = false;
                    break;
                case 3:
                    compList = core.GetComputers();
                    EditDG.ItemsSource = compList;
                    changed = false;
                    RollbackButton.IsEnabled = false;
                    SaveButton.IsEnabled = false;
                    break;
            }
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
            switch (BaseSelect.SelectedIndex)
            {
                case 3:
                    core.UpdateComp(compList);

                    compList = core.GetComputers();
                    EditDG.ItemsSource = null;
                    EditDG.ItemsSource = compList;

                    break;
            }
        }

        private void StreamInputButton_Click(object sender, RoutedEventArgs e)
        {
            StreamAddWindow compIn = new StreamAddWindow("computer");

            if(compIn.ShowDialog()==true)
            {
                changed = true;
                RollbackButton.IsEnabled = true;
                SaveButton.IsEnabled = true;
                HashSet<Computer> newData = core.ParseComp(compIn.InputData);
                newData.UnionWith(compList);
                compList=newData;
                EditDG.ItemsSource = null;
                EditDG.ItemsSource = compList;
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
    }
}
