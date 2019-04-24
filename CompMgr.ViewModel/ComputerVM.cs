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
       

        public override string NsName
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

        public override string Ip
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
                if (division == null)
                    return "Не распределён";
                else
                    return division.DivisionName;
            }
        }


        public override string UserFio
        {
            get
            {
                if (user == null)
                    return "Не распределён";
                else
                    return user.UserFio;
            }
        }


        public override Division CurrentDivision
        {
            get
            {
                return division;
            }
            set
            {
                if(value != division)
                {
                    division = value;
                    OnPropertyChanged(this, "CurrentDivision");
                }
            }
        }

        public override User CurrentUser
        {
            get
            {
                return user;
            }
            set
            {
                if(value!=user)
                {
                    user = value;
                    OnPropertyChanged(this, "CurrentUser");
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
