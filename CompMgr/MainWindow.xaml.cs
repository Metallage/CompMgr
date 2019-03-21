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
using System.Windows.Threading;
using System.Data;
using System.Threading;
using CompMgr.ViewModel;

namespace CompMgr
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //Визуальная модель главного окна
        private MainWindowViewModel mwvm = new MainWindowViewModel();

        private CompleteData completeData = new CompleteData();



        public MainWindow()
        {
            InitializeComponent();

            mwvm.CoreStarted += Mwvm_CoreStarted;
            mwvm.DataUpdate += Mwvm_DataUpdate;

        }

        private void Mwvm_DataUpdate()
        {
            Bind();
        }

        private void Mwvm_CoreStarted()
        {
            Bind();
        }

        private void Bind()
        {
            //Перенаправления потока для исключения кросстрединга
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate() 
                {
                    UpdButton.IsEnabled = true;
                    DistributeButon.IsEnabled = true;
                    mwvm.GetCompData();
                    DataContext = mwvm;
                    gridField.Items.Refresh();
                });
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            //RunOldCore();

            
        }

        //private void RunOldCore()
        //{
        //    ErrorMessageHelper start = core.Start();
        //    if (start.HasErrors)
        //    {
        //        MessageBox.Show(start.ErrorText);
        //    }
        //    completeData.GetData(core.GetCompleteTable());
        //    gridField.ItemsSource = completeData;

        //}

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {

            //UpdateFormViewModel uvm = new UpdateFormViewModel();
            //UpdateWindow UPD = new UpdateWindow(uvm);
            //UPD.Show();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Обновление"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdButton_Click(object sender, RoutedEventArgs e)
        {
            SelectSoftViewModel ssvm = mwvm.SelectSoftToUpdate(); //Формируем визуальную модель для окна выбора ПО
            SelectSoftWindow ssw = new SelectSoftWindow(ssvm); //Создаём окно выбора ПО
            if(ssw.ShowDialog()==true) //Если нажал ок
            {
                string softName = ssvm.SelectedSoftName; //Выдираем имя ПО для обновления
                string newVer = ssvm.NewVersion; //Выдираем новую версию
                mwvm.UpdateSoft(softName, newVer); //Обновляем По в таблице software
                UpdateFormViewModel uwvm = mwvm.StartUpdate(softName); //Формируем визуальную модель для окна обновдлений
                UpdateWindow upWin = new UpdateWindow(uwvm); //создаём окно обновлений
                upWin.ShowDialog(); //показываем
            }
        }



        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

            Logic core = new Logic();
            ErrorMessageHelper start = core.Start();
            if (start.HasErrors)
            {
                MessageBox.Show(start.ErrorText);
            }

            EditTableWindow editTable = new EditTableWindow(core);
            editTable.Show();
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {

            InstallWindowVM iwvm = mwvm.StartInstall();

            InstallWindow installWindow = new InstallWindow(iwvm);
            installWindow.Show();
        }



        private void DistributeButon_Click(object sender, RoutedEventArgs e)
        {
            DistributionViewModel dvm = mwvm.StartDistribute();

            DistributionWindow distribute = new DistributionWindow(dvm);
            distribute.ShowDialog();
           
        }
    }
}
