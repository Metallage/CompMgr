﻿using System;
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

        private DataTable software;
        private DataTable users;
        private DataTable division;
        private DataTable computer;
        private DataTable install;
        private List<Computer> compList = new List<Computer>();
        Dictionary<long, string> userNames = new Dictionary<long, string>();
        Dictionary<long, string> divNames = new Dictionary<long, string>();



        private bool changed = false;


        private Logic logic;

        public EditTableWindow(Logic logic)
        {
            this.logic = logic;
            InitializeComponent();
        }

       //private void FormComp()
       // {
       //     compList.Clear();
       //     userNames.Clear();
       //     divNames.Clear();


       //     var queryUser = from user in users.AsEnumerable()
       //                     select new { Id = user.Field<long>("id"), Fio = user.Field<string>("fio") };
       //     foreach(dynamic us in queryUser)
       //     {
       //         userNames.Add(us.Id, us.Fio);
       //     }

       //     userNames.Add(-1,"не закреплён");


       //     var queryDiv = from div in division.AsEnumerable()
       //                     select new { Id = div.Field<long>("id"), Name = div.Field<string>("name") };
       //     foreach (var div in queryDiv)
       //     {
       //         divNames.Add(div.Id, div.Name);
       //     }

       //     divNames.Add(-1, "не закреплён");

       //     foreach (DataRow dr in computer.Select())
       //     {
       //         Computer comp = new Computer();
       //         comp.NsName = dr.Field<string>("nsName");
       //         comp.Ip = dr.Field<string>("ip");
       //         comp.Id = dr.Field<long>("id");

       //         if (dr.Field<string>("userID") == null)
       //             comp.UserID = -1;
       //         else
       //             comp.UserID = dr.Field<long>("userID");
       //         if (dr.Field<string>("divID") == null)
       //             comp.DivisionId = -1;
       //         else
       //             comp.DivisionId = dr.Field<long>("divID");

       //         compList.Add(comp);

       //     }


       // }

        ///// <summary>
        ///// Сохраняет изменения в тавлице
        ///// </summary>
        ///// <param name="tableName">Имя таблицы</param>
        //private void SaveChanges(string tableName)
        //{
        //    changed = false;
        //    localDS.Tables[$"{tableName}"].AcceptChanges(); //Сохраняем локальную копию формы
        //    logic.UpdateTable(tableName, localDS.Tables[$"{tableName}"]); //Сохраняем в основной базе
        //}

        #region Привязываем таблицы к DataGrid

        private void CollapseColumns()
        {
            foreach (DataGridColumn dc in EditDG.Columns)
                dc.Visibility = Visibility.Collapsed;    
        }

        /// <summary>
        /// Настраивает DataGrid на отображение таблицы Software 
        /// </summary>
        private void BindSoft()
        {
            //удаляем все предыдущие столбцы и запрещаем из автогенерацию
            EditDG.Columns.Clear();
            EditDG.AutoGenerateColumns = false;

            //Создаём текстовый столбец с заголовком "Название ПО"
            DataGridTextColumn nameColumn = new DataGridTextColumn();
            nameColumn.Header = "Название ПО";
            //Привязываем его к источнику данных с полем name
            Binding softName = new Binding("name");
            nameColumn.Binding = softName;

            //Создаём текстовый столбец с заголовком "Текущая версия ПО"
            DataGridTextColumn verColumn = new DataGridTextColumn();
            verColumn.Header = "Текущая версия ПО";
            //Привязываем его к источнику данных с полем version
            Binding softVer = new Binding("version");
            verColumn.Binding = softVer;

            //Добавляем таблички к DataGrid
            EditDG.Columns.Add(nameColumn);
            EditDG.Columns.Add(verColumn);

            //Источник таблица Software
            EditDG.ItemsSource = software.DefaultView;

            EditDG.IsEnabled = true;
        }


        private void BindUsers()
        {
            EditDG.AutoGenerateColumns = false;
            EditDG.Columns.Clear();

            DataGridTextColumn fioCol = new DataGridTextColumn();
            fioCol.Header = "Фамилия и инициалы";
            Binding fioBind = new Binding("fio");
            fioCol.Binding = fioBind;

            DataGridTextColumn telCol = new DataGridTextColumn();
            telCol.Header = "Рабочий телефон";
            Binding telBind = new Binding("tel");
            telCol.Binding = telBind;

            EditDG.Columns.Add(fioCol);
            EditDG.Columns.Add(telCol);

            EditDG.ItemsSource = users.DefaultView;

            EditDG.IsEnabled = true;
        }



        private void BindComp()
        {
            CollapseColumns();

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
                }
            }

           
            
            compList = logic.GetComputers();
            EditDG.ItemsSource = compList;
           


        }

        private void BindDivision()
        {
            EditDG.AutoGenerateColumns = false;
            EditDG.Columns.Clear();

            DataGridTextColumn divNameCol = new DataGridTextColumn();
            divNameCol.Header = "Наименование подразделения";
            Binding nameBind = new Binding("name");
            divNameCol.Binding = nameBind;

            EditDG.Columns.Add(divNameCol);

            EditDG.ItemsSource = division.DefaultView;

            EditDG.IsEnabled = true;

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
                    case 4:
                        EditDG.Columns.Clear();
                        EditDG.ItemsSource = install.DefaultView;
                        EditDG.AutoGenerateColumns = true;
                        EditDG.IsEnabled = true;
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
            switch (BaseSelect.SelectedIndex)
            {
                case 0:
                    software.RejectChanges();
                    changed = false;
                    RollbackButton.IsEnabled = false;
                    SaveButton.IsEnabled = false;
                    break;
                case 1:
                    users.RejectChanges();
                    changed = false;
                    RollbackButton.IsEnabled = false;
                    SaveButton.IsEnabled = false;
                    break;
                case 2:
                    division.RejectChanges();
                    changed = false;
                    RollbackButton.IsEnabled = false;
                    SaveButton.IsEnabled = false;
                    break;
                case 3:
                    computer.RejectChanges();
                    changed = false;
                    RollbackButton.IsEnabled = false;
                    SaveButton.IsEnabled = false;
                    break;
                case 4:
                    install.RejectChanges();
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

    }
}
