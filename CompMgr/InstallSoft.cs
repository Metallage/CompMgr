using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CompMgr
{
    public class InstallSoft : INotifyPropertyChanged
    {
        private bool installed = false;

        public long SoftId { get; set; }

        public string SoftName { get; set; }
        public string SoftVersion { get; set; }
        public long ComputerId { get; set; }

        public bool Installed
        {
            get
            {
                return installed;
            }
            set
            {
                if (value != installed)
                {
                    installed = value;
                    OnPropertyChanged(this, "Installed");
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
