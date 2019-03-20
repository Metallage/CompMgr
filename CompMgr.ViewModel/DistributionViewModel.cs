using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using CompMgr.Model;


namespace CompMgr.ViewModel
{
    public class DistributionViewModel:INotifyPropertyChanged
    {


        private DistributedView sourceDistr = new DistributedView();
        private ObservableCollection<User> userSource = new ObservableCollection<User>();

        private ModelCore core;

        public delegate void UpdateEventHandler();
        public event UpdateEventHandler UpdateData;
        public event UpdateEventHandler AllSaved;

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
        public ObservableCollection<User> UserSource { get
            {
                return userSource;
            }
                set
            {
                if(value!=userSource)
                {
                    userSource = value;
                    OnPropertyChanged(this, "UserSource");
                }
            }
                }
        public ObservableCollection<Computer> CompSource { get; set; }



        public void ExecuteDeleteCommand(string nsName)
        {
            DistributionVM delDistribution = FindByNsName(nsName);

            UserSource.Add(new User(delDistribution.UserID,delDistribution.UserFio));
            CompSource.Add(new Computer(delDistribution.ComputerID, delDistribution.NsName));
            sourceDistr.Remove(delDistribution);
        }


        public void AddDistribution(DistributionVM newDistribution, object currentComp, object currentUser)
        {

            User cu = currentUser as User;
            Computer cc = currentComp as Computer;

            newDistribution.Id = -1;
            newDistribution.ComputerID = cc.Id;
            newDistribution.NsName = cc.NsName;
            newDistribution.UserFio = cu.UserFio;
            newDistribution.UserID = cu.Id;

            SourceDistr.Add(newDistribution);


            UserSource.Remove(cu);
            CompSource.Remove(cc);
        }

        public void SaveDistribution()
        {
            List<Distribution> saveDistr = new List<Distribution>();

            foreach (DistributionVM dvm in sourceDistr)
            {
                Distribution dst = new Distribution(dvm.Id, dvm.ComputerID, dvm.NsName, dvm.UserID, dvm.UserFio);
                saveDistr.Add(dst);
            }

            var saveThread = Task.Factory.StartNew(()=>
                {



                    core.SaveDistribution(saveDistr);
                    AllSaved?.Invoke();
                });
          
        }

        public void ImportData()
        {
            List<Distribution> distributions = core.GetDistribution();

            sourceDistr = new DistributedView(distributions);
            UserSource = core.GetUsersNoComp();
            CompSource = core.GetComputersNoUser();
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
