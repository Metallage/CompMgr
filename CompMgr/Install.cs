using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace CompMgr
{
    public class Install : INotifyPropertyChanged
    {

        private ObservableCollection<InstallSoft> isInstalled = new ObservableCollection<InstallSoft>();
        //private long computerId; //ID компа куда будем устанавливать софт
        //private string nsName = String.Empty; //Имя компа, куда будем устанавливать софт
        //private string ip = String.Empty; //ip компа куда будем устанавливать софт

        //ID записи об установке
        //public long Id { get; set; }
        public long ComputerId { get; set; }
        public string NsName { get; set; }
        public string Ip { get; set; }

        
        ////ID компьютера
        //public long ComputerId
        //{
        //    get
        //    {
        //        return computerId;
        //    }
        //    set
        //    {
        //        if (value != this.computerId)
        //        {
        //            computerId = value;
        //            OnPropertyChanged(this, "ComputerId");
        //        }
        //    }
        //}

        ////Имя компьютера
        //public string NsName
        //{
        //    get
        //    {
        //        return nsName;
        //    }
        //    set
        //    {
        //        if (value != this.nsName)
        //        {
        //            nsName = value;
        //            OnPropertyChanged(this, "NsName");
        //        }
        //    }
        //}

        //public string Ip
        //{
        //    get
        //    {
        //        return ip;
        //    }
        //    set
        //    {
        //        if (value != this.ip)
        //        {
        //            ip = value;
        //            OnPropertyChanged(this, "Ip");
        //        }
        //    }
        //}

        public ObservableCollection<InstallSoft> IsInstalled
        {
            get
            {
                return isInstalled;
            }
            set
            {
                if(value != isInstalled)
                {
                    isInstalled = value;
                    OnPropertyChanged(this, "IsInstalled");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
