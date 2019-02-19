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

        private DataTable software = new DataTable("Software");
        private DataTable users = new DataTable("Users");
        private DataTable division = new DataTable("Division");
        private DataTable computer = new DataTable("Computer");
        private DataTable install = new DataTable("Install");
        private DataSet localDS = new DataSet();

        private bool changed = false;

        private Logica logic;

        public EditTableWindow(Logica logic)
        {
            this.logic = logic;
            InitializeComponent();
        }

        /// <summary>
        /// Заполняем таблички из переданного DataSet
        /// </summary>
        /// <param name="source">Источник для заполнения</param>
        public void FormDataTables(DataSet source)
        {
            using (DataTableReader softReader = source.Tables["Software"].CreateDataReader())
            {
                software.Load(softReader);
                software.AcceptChanges();
                localDS.Tables.Add(software);
            }

            using (DataTableReader userReader = source.Tables["Users"].CreateDataReader())
            {
                users.Load(userReader);
                users.AcceptChanges();
                localDS.Tables.Add(users);
            }

            using (DataTableReader divReader = source.Tables["Division"].CreateDataReader())
            {
                division.Load(divReader);
                division.AcceptChanges();
                localDS.Tables.Add(division);
            }


            using (DataTableReader compReader = source.Tables["Computer"].CreateDataReader())
            {
                computer.Load(compReader);
                computer.AcceptChanges();
                localDS.Tables.Add(computer);
            }

            using (DataTableReader instReader = source.Tables["Install"].CreateDataReader())
            {
                install.Load(instReader);
                install.AcceptChanges();
                localDS.Tables.Add(install);
            }
        }

        /// <summary>
        /// Сохраняет изменения в тавлице
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        private void SaveChanges(string tableName)
        {
            changed = false;
            localDS.Tables[$"{tableName}"].AcceptChanges(); //Сохраняем локальную копию формы
            logic.UpdateTable(tableName, localDS.Tables[$"{tableName}"]); //Сохраняем в основной базе
        }

        private void Update()
        {
            
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
                        EditDG.ItemsSource = software.DefaultView;
                        EditDG.AutoGenerateColumns = true;
                        EditDG.IsEnabled = true;
                        break;
                    case 1:
                        EditDG.ItemsSource = users.DefaultView;
                        EditDG.AutoGenerateColumns = true;
                        EditDG.IsEnabled = true;
                        break;
                    case 2:
                        EditDG.ItemsSource = division.DefaultView;
                        EditDG.AutoGenerateColumns = true;
                        EditDG.IsEnabled = true;
                        break;
                    case 3:
                        EditDG.ItemsSource = computer.DefaultView;
                        EditDG.AutoGenerateColumns = true;
                        EditDG.IsEnabled = true;
                        break;
                    case 4:
                        EditDG.ItemsSource = install.DefaultView;
                        EditDG.AutoGenerateColumns = true;
                        EditDG.IsEnabled = true;
                        break;
                }
            }
        }


        public DataSet TableSet
        { get; set; }

        public Logica logica
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
    }
}
