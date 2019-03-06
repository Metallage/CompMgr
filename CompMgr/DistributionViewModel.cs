using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Controls;

namespace CompMgr
{
    public class DistributionViewModel:INotifyPropertyChanged
    {
        private DistributedView sourceDistr = new DistributedView();

        public DistributedView SourceDistr {
            get
            {
                return sourceDistr;
            }

            set
            {
                if(value != sourceDistr)
                {
                    sourceDistr = value;
                    OnPropertyChanged(this, "SourceDistr");
                }
            }
        }
        public ObservableCollection<User> UserSource { get; set; }
        public ObservableCollection<Computer> CompSource { get; set; }

        public static RoutedCommand DeleteItemCommand = new RoutedCommand();

        public static RoutedCommand AddItemCommand = new RoutedCommand();




        public void ExecuteDeleteCommand(object sender, ExecutedRoutedEventArgs e)
        {
            Distribution delDistribution = FindByNsName(e.Parameter.ToString());

            UserSource.Add(new User(delDistribution.UserID,delDistribution.UserFio));
            CompSource.Add(new Computer(delDistribution.ComputerID, delDistribution.NsName));
            sourceDistr.Remove(delDistribution);
        }

        public void CanExecuteDeleteCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            Control target = e.Source as Control;

            if (target != null)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        public void Save()
        {
            
        }

        //private static ItemCommand deleteItem;
        //private ItemCommand addItem;

        //public ItemCommand DeleteItem
        //{
        //    get
        //    {
        //        return deleteItem ??
        //            (deleteItem = new ItemCommand(obj =>
        //            {
        //                Distribution distr = this.FindByNsName((string)obj);
        //                if (distr != null)
        //                    SourceDistr.Remove(distr);
        //            },
        //            (obj)=>SourceDistr.Count()>0));
        //    }
        //}

        //public ItemCommand AddItem
        //{
        //    get
        //    {
        //        return addItem ??
        //            (addItem = new ItemCommand(obj =>
        //            {
        //                Distribution distr = new Distribution();
        //                string param = obj as string;
        //                string[] paramPam = param.Split(',');
        //                distr.Id = -1;
        //                distr.NsName = paramPam[0];
        //                distr.UserFio = paramPam[1];
        //                SourceDistr.Add(distr);
        //            },(obj)=>(UserSource.Count>0)&&(CompSource.Count>0)
        //        ));
        //    }
        //}


        private Distribution FindByNsName(string nsName)
        {
            foreach (Distribution distr in SourceDistr)
                if (distr.NsName == nsName)
                {
                    return distr;

                }
            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
