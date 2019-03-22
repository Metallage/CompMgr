using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CompMgr.Model;
using System.ComponentModel;

namespace CompMgr.ViewModel
{
    public class CompleteTableHelperVM : CompleteTableHelper, INotifyPropertyChanged
    {
        private ObservableCollection<CompListHelper> compNames = new ObservableCollection<CompListHelper>();


        public CompleteTableHelperVM()
        {
        }

        public new ObservableCollection<CompListHelper> CompNames
        {
            get
            {
                return compNames;
            }
            set
            {
                if(value!=compNames)
                {
                    compNames = value;
                    OnPropertyChanged(this, "CompNames");
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
