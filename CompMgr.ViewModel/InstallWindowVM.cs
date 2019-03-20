using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using CompMgr.Model;

namespace CompMgr.ViewModel
{
    public class InstallWindowVM:INotifyPropertyChanged
    {
        private ObservableCollection<InstallVM> installs = new ObservableCollection<InstallVM>();

        private ModelCore core;

        public delegate void UpdateHandler();
        public event UpdateHandler DataUpdate;

        public InstallWindowVM (ModelCore core)
        {
            this.core = core;
            this.core.DataUpdate += Core_DataUpdate;
        }

        private void Core_DataUpdate()
        {
            DataUpdate?.Invoke();
        }

        public ObservableCollection<InstallVM> Installs
        {
            get
            {
                return installs;
            }
            set
            {
                if(value!= installs)
                {
                    installs = value;
                    OnPropertyChanged(this, "Installs");
                }
            }
        }

        public void RunModel()
        {
            var importData = Task.Factory.StartNew( ()=>
                {
                    GetInstall();
                    DataUpdate?.Invoke();
                });

        }

        private void GetInstall()
        {
            List<string> allsoft = core.GetSoftNames();
            List<Install> modelList = core.GetInstalled();
            foreach(Install inst in modelList)
            {
                InstallVM newComp = new InstallVM(inst, allsoft);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}
