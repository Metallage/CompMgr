using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using CompMgr.Model;


namespace CompMgr.ViewModel
{
    public class MainWindowViewModel:INotifyPropertyChanged
    {
        private ModelCore core = new ModelCore();

        private CompleteData dataList = new CompleteData();

        public delegate void RefreshEventHadnler();
        public event RefreshEventHadnler CoreStarted;
        public event RefreshEventHadnler DataUpdate;


        public CompleteData DataList
        {
            get
            {
                return dataList;
            }
            set
            {
                if(dataList!=value)
                {
                    dataList = value;
                    OnPropertyChanged(this, "DataList");
                }
            }
        }

        public MainWindowViewModel()
        {
            FirstLoad();
        }

        public UpdateFormViewModel StartUpdate(string softName)
        {
            UpdateFormViewModel updateViewModel = new UpdateFormViewModel(core, softName);

            return updateViewModel;
        }

        public SelectSoftViewModel SelectSoftToUpdate()
        {
            SelectSoftViewModel ssvm = new SelectSoftViewModel(core);

            return ssvm;
        }


        public DistributionViewModel StartDistribute()
        {
            DistributionViewModel dvm = new DistributionViewModel(core);

            return dvm;
        }

        public InstallWindowVM StartInstall()
        {
            InstallWindowVM iwvm = new InstallWindowVM(core);

            return iwvm;
        }

        public void UpdateSoft(string softName, string version)
        {
            core.UpdateSoft(softName, version);
        }


        public void GetCompData()
        {
            List<CompleteTableHelper> dataList = core.GetCompleteTable();
            CompleteData newData = new CompleteData(dataList);
            this.dataList = newData;
        }

        private void FirstLoad()
        {
            core.DataUpdate += Core_DataUpdate;
            var firstLuanch = Task.Factory.StartNew(() => 
            {
                core.Start();
                CoreStarted?.Invoke();
            });


        }

        private void Core_DataUpdate()
        {
            DataUpdate?.Invoke();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }

    }
}
