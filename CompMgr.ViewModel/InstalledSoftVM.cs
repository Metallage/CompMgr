using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CompMgr.ViewModel
{
    public class InstalledSoftVM : IComparable, INotifyPropertyChanged
    {
        private bool isInstalled;
        public string SoftName { get; set; }
        public bool IsInstalled
        {
            get
            {
                return isInstalled;
            }
            set
            {
                if(value!=isInstalled)
                {
                    isInstalled = value;
                    OnPropertyChanged(this, "IsInstalled");
                }
            }
        }

        /// <summary>
        /// Реализация метода сравнения для интерфейса IComparable
        /// </summary>
        /// <param name="obj">Объектк типа InstalledSoftVM с которым надо сравнить</param>
        /// <returns>если меньше нуля то меньше, если 0 то одинаковы, если больше 0 то больше</returns>
        public int CompareTo(object obj)
        {
            InstalledSoftVM isoft = obj as InstalledSoftVM;
            if (isoft != null)
                return this.SoftName.CompareTo(isoft.SoftName);
            else
                throw new Exception("Не возможно сравнить 2 обьекта");
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}
