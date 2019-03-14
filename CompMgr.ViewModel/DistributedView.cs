using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using CompMgr.Model;


namespace CompMgr.ViewModel
{
    public class DistributedView: ObservableCollection<DistributionVM>
    {

        public DistributedView()
        { }

        public DistributedView(List<Distribution> distributions)
        {
            foreach(Distribution dst in distributions)
            {
                DistributionVM dvm = new DistributionVM();
                dvm.Id = dst.Id;
                dvm.ComputerID = dst.ComputerID;
                dvm.NsName = dst.NsName;
                dvm.UserID = dst.UserID;
                dvm.UserFio = dst.UserFio;
                this.Add(dvm);
            }
        }

        public void InputMe(ObservableCollection<DistributionVM> input)
        {
            foreach (DistributionVM distr in input)
                Add(distr);
        }

        public ObservableCollection<DistributionVM> GetMe()
        {
            ObservableCollection<DistributionVM> output = new ObservableCollection<DistributionVM>();

            foreach (DistributionVM dist in this)
                output.Add(dist);

            return output;
        }

        //private ItemCommand deleteItem;
       

        //public ItemCommand DeleteItem
        //{
        //    get
        //    {
        //        return deleteItem ??
        //            (deleteItem = new ItemCommand(obj =>
        //            {
        //                Distribution distr = this.FindByNsName((string)obj);
        //                if (distr != null)
        //                    this.Remove(distr);
        //            },
        //            (obj) => this.Count() > 0));
        //    }
        //}

        private DistributionVM FindByNsName(string nsName)
        {
            foreach (DistributionVM distr in this)
                if (distr.NsName == nsName)
                {
                    return distr;

                }
            return null;
        }
    }
}
