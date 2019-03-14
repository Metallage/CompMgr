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
    public class DistributionViewModel:INotifyPropertyChanged
    {


        private DistributedView sourceDistr = new DistributedView();

        private ModelCore core;

        public delegate void UpdateEventHandler();
        public event UpdateEventHandler UpdateData;

        public DistributionViewModel(ModelCore core)
        {
            this.core = core;
            this.core.DataUpdate += Core_DataUpdate;
            ImportData();
        }

        private void Core_DataUpdate()
        {
            UpdateData?.Invoke();
        }

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



        public void ExecuteDeleteCommand(string nsName)
        {
            DistributionVM delDistribution = FindByNsName(nsName);

            UserSource.Add(new User(delDistribution.UserID,delDistribution.UserFio));
            CompSource.Add(new Computer(delDistribution.ComputerID, delDistribution.NsName));
            sourceDistr.Remove(delDistribution);
        }



        public void ImportData()
        {
            List<Distribution> distributions = core.GetDistribution();

            sourceDistr = new DistributedView(distributions);
        }

        public void Save()
        {
            
        }


        private DistributionVM FindByNsName(string nsName)
        {
            foreach (DistributionVM distr in SourceDistr)
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
