using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CompMgr.Model;

namespace CompMgr.ViewModel
{
    public class ComputerVM: Computer, INotifyPropertyChanged
    {
       

        public new string NsName
        {
            get
            {
                return nsName;
            }
            set
            {
                if (value != nsName)
                {
                    nsName = value;
                    OnPropertyChanged(this, "NsName");
                }
            }
        }

        public new string Ip
        {
            get
            {
                return ip;
            }
            set
            {
                if(value != ip)
                {
                    ip = value;
                    OnPropertyChanged(this, "Ip");
                }
            }
        }


        public override string DivisionName
        {
            get
            {
                return divisionName;
            }
            set
            {
                if (value != divisionName)
                {
                    userFio = value;
                    OnPropertyChanged(this, "DivisionName");
                }
            }
        }


        public override string UserFio
        {
            get
            {
                return userFio;
            }

            set
            {
                if (value != userFio)
                {
                    userFio = value;
                    OnPropertyChanged(this, "UserFio");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}
