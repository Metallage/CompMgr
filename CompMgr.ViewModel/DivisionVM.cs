using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CompMgr.Model;

namespace CompMgr.ViewModel
{
    public class DivisionVM:Division, INotifyPropertyChanged
    {
        public override string DivisionName
        {
            get
            {
                return divisionName;
            }
            set
            {
                if(value!=divisionName)
                {
                    divisionName = value;
                    OnPropertyChanged(this, "DivisionName");
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
