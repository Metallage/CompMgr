using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CompMgr.Model;

namespace CompMgr.ViewModel
{
    public class InstalledSoftVM : InstalledSoft, IComparable, INotifyPropertyChanged
    {
        private bool isInstalled;
        //public string SoftName { get; set; }
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

        public InstalledSoftVM()
        {

        }

        public InstalledSoftVM(string softName)
        {
            SoftName = softName;
            IsInstalled = false;
        }

        public InstalledSoftVM(InstalledSoft inss)
        {
            SoftName = inss.SoftName;
            IsInstalled = true;
        }

        public InstalledSoftVM(string softName, string version)
        {
            SoftName = softName;
            Version = version;
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


        public override string ToString()
        {
            return SoftName;
        }
    }
}
