using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CompMgr
{
    public class Updates : INotifyPropertyChanged
    {
        private bool isUp;

        public long Id { get; set; }
        public string NsName { get; }
        public string Ip { get; }
        public string UserFio { get; }
        public string OldVersion { get; set; }
        public string NewVersion { get; set; }

        public Updates(string nsName, string ip, string user)
        {
            NsName = nsName;
            Ip = ip;
            UserFio = user;
            isUp = false;
        }

        public bool IsUp { get
            {
                return isUp;
            }
            set
            {
                if (value != isUp)
                {
                    isUp = value;
                    OnPropertyChanged(this, "IsUp");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
