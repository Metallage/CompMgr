using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using CompMgr.ViewModel;


namespace CompMgr
{
    /// <summary>
    /// Логика взаимодействия для UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        UpdateFormViewModel updModel;




        public UpdateWindow(UpdateFormViewModel updModel)
        {
            this.updModel = updModel;
            //gridSource = core.GetUpdates("Ордер");

            InitializeComponent();
            DataContext = updModel;
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Info.Text = "Обновления для Ордер";
            //UpdGrid.ItemsSource = gridSource;
        }
    }
}
