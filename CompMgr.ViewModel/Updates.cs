using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CompMgr.ViewModel
{
    public class ViewUpdate : INotifyPropertyChanged
    {
        private bool isUp;

        public long Id { get; set; }
        public string NsName { get; set; }
        public string Ip { get; set; }
        public string UserFio { get; set; }
        public string OldVersion { get; set; }
        public string CurrentVersion { get; set; }

        public ViewUpdate(long id, string nsName, string ip, string userFio, string oldVersion, string currentVersion)
        {
            Id = id;
            NsName = nsName;
            Ip = ip;
            UserFio = userFio;
            OldVersion = oldVersion;
            CurrentVersion = currentVersion;
            isUp = false;
        }
        public ViewUpdate()
        {
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
