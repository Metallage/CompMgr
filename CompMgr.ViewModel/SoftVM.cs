using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CompMgr.Model;

namespace CompMgr.ViewModel
{
    /// <summary>
    /// Класс визуальной модели для отображения ПО
    /// </summary>
    public class SoftVM:Soft,INotifyPropertyChanged
    {
        /// <summary>
        /// Свойство доступа к названию ПО
        /// </summary>
        public override string SoftName
        {
            get
            {
                return softName;
            }
            set
            {
                if(value!=softName)
                {
                    softName = value;
                    OnPropertyChanged(this, "SoftName");
                }
            }
        }

        /// <summary>
        /// Свойство доступа к версии ПО
        /// </summary>
        public override string SoftVersion
        {
            get
            {
                return softVersion;
            }
            set
            {
                if(value!=softVersion)
                {
                    softVersion = value;
                    OnPropertyChanged(this, "SoftVersion");
                }
            }
        }


        #region Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
