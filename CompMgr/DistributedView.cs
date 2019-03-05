using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;


namespace CompMgr
{
    public class DistributedView: ObservableCollection<Distribution>
    {

        public void InputMe(ObservableCollection<Distribution> input)
        {
            foreach (Distribution distr in input)
                Add(distr);
        }

        public ObservableCollection<Distribution> GetMe()
        {
            ObservableCollection<Distribution> output = new ObservableCollection<Distribution>();

            foreach (Distribution dist in this)
                output.Add(dist);

            return output;
        }

        private ItemCommand deleteItem;
       

        public ItemCommand DeleteItem
        {
            get
            {
                return deleteItem ??
                    (deleteItem = new ItemCommand(obj =>
                    {
                        Distribution distr = this.FindByNsName((string)obj);
                        if (distr != null)
                            this.Remove(distr);
                    },
                    (obj) => this.Count() > 0));
            }
        }

        private Distribution FindByNsName(string nsName)
        {
            foreach (Distribution distr in this)
                if (distr.NsName == nsName)
                {
                    return distr;

                }
            return null;
        }
    }
}
