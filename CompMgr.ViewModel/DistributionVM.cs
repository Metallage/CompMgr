using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;

namespace CompMgr.ViewModel
{
    public class DistributionVM: DependencyObject, INotifyPropertyChanged
    {
        private string userFio = String.Empty;
        private string nsName = String.Empty;
        private long computerID;
        private long userID;

        public static readonly DependencyProperty IDProperty = DependencyProperty.Register("Id", typeof(long), typeof(DistributionVM));
        public static readonly DependencyProperty UserFioProperty = DependencyProperty.Register("UserFio", typeof(string), typeof(DistributionVM));
        public static readonly DependencyProperty NsNameProperty = DependencyProperty.Register("NsName", typeof(string), typeof(DistributionVM));

        public long Id { get; set; }

        public string UserFio
        {
            get
            {
                return (string)GetValue(UserFioProperty);
            }
            set
            {
                SetValue(UserFioProperty, value);
            }
        }

        public string NsName
        {
            get
            {
                 return (string)GetValue(NsNameProperty); 
            }
            set
            {
                SetValue(NsNameProperty,value);
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
