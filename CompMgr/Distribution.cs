using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CompMgr
{
    public class Distribution:INotifyPropertyChanged
    {
        private string userFio = String.Empty;
        private string nsName = String.Empty;
        private long computerID;
        private long userID;

        public long Id { get; set; }

        public string UserFio
        {
            get
            {
                return userFio;
            }
            set
            {
                if(value != userFio)
                {
                    userFio = value;
                    OnPropertyChanged(this, "UserFio");
                }
            }
        }

        public string NsName
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

        public long ComputerID
        {
            get
            {
                return computerID;
            }
            set
            {
                if (value != computerID)
                {
                    computerID = value;
                    OnPropertyChanged(this, "ComputerID");
                }
            }
        }

        public long UserID
        {
            get
            {
                return userID;
            }
            set
            {
                if (value != userID)
                {
                    userID = value;
                    OnPropertyChanged(this, "UserID");
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
