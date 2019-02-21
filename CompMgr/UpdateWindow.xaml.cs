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


namespace CompMgr
{
    /// <summary>
    /// Логика взаимодействия для UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        private Logica logic;

        public List<Updates> gridSource;

        public UpdateWindow(Logica logic)
        {
            this.logic = logic;
            gridSource = logic.GetUpdates();

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            

            DataGridTextColumn nsName = new DataGridTextColumn();
            nsName.Header = "Имя Компьютера";
            Binding nsBind = new Binding("NsName");
            nsName.Binding = nsBind;
            nsName.IsReadOnly = true;
            UpdGrid.Columns.Add(nsName);

            DataGridTextColumn ip = new DataGridTextColumn();
            ip.Header = "IP адрес";
            Binding ipBind = new Binding("Ip");
            ip.IsReadOnly = true;
            ip.Binding = ipBind;
            UpdGrid.Columns.Add(ip);

            DataGridTextColumn user = new DataGridTextColumn();
            user.Header = "Пользователь";
            Binding usrBind = new Binding("User");
            user.Binding = usrBind;
            user.IsReadOnly = true;
            UpdGrid.Columns.Add(user);

            DataGridCheckBoxColumn isUp = new DataGridCheckBoxColumn();
            isUp.Header = "Обновлено";
            Binding upBind = new Binding("IsUp");
            isUp.Binding = upBind;
            UpdGrid.Columns.Add(isUp);

            Updates up = new Updates();
            up.NsName = "localhost";
            up.Ip = "127.0.0.100";
            up.User = "Админов А.А.";
            gridSource.Add(up);
            UpdGrid.ItemsSource = gridSource;
        }
    }
}
