using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CompMgr.ViewModel
{
    public class InstallVM: INotifyPropertyChanged, IComparable
    {
        private ObservableCollection<InstalledSoftVM> installedSoft = new ObservableCollection<InstalledSoftVM>();

        public string NsName { get; set; }
        public string UserFio { get; set; }
        
        public ObservableCollection<InstalledSoftVM> InstalledSoft
        {
            get
            {
                return installedSoft;
            }
            set
            {
                if(value!=installedSoft)
                {
                    installedSoft = value;
                    OnPropertyChanged(this, "InstalledSoft");
                }
            }

        }

        public int CompareTo(object obj) // не факт , что надо...
        {
            InstallVM iVM = obj as InstallVM;
            if (iVM != null)
                return this.NsName.CompareTo(iVM.NsName);
            else
                throw new Exception("Невозможно сравнить 2 объекта");

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(object sender , string propertyName)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}
