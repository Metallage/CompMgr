using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;

namespace CompMgr.ViewModel
{
    public class ViewSoft: DependencyObject, INotifyPropertyChanged
    {
        public static readonly DependencyProperty SoftNameProperty;
        public static readonly DependencyProperty SoftVersionProperty;

        static ViewSoft()
        {
            SoftNameProperty = DependencyProperty.Register("SoftName", typeof(string), typeof(ViewSoft));
            SoftVersionProperty = DependencyProperty.Register("SoftVersion", typeof(string), typeof(ViewSoft));
        }

        public long Id { get; set; }

        public string SoftName
        {
            get
            {
                return (string)GetValue(SoftNameProperty);
            }
            set
            {
                if (value != (string)GetValue(SoftNameProperty))
                {
                    SetValue(SoftNameProperty, value);
                    onPropertyChanged(this, "SoftName");
                    //onPropertyChanged(this, "SoftVersion");
                }
            }
        }

        public string SoftVersion
        {
            get
            {
                return (string)GetValue(SoftVersionProperty);
            }
            set
            {

                if (value != (string)GetValue(SoftVersionProperty))
                {
                    SetValue(SoftVersionProperty, value);
                    onPropertyChanged(this, "SoftVersion");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void onPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
