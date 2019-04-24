using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CompMgr.Model;

namespace CompMgr.ViewModel
{
    /// <summary>
    /// Класс для отображения пользователя
    /// </summary>
    public class UserVM: User, INotifyPropertyChanged
    {
        /// <summary>
        /// Устанавливает или возвращает ФИО пользователя
        /// </summary>
        public override string UserFio
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

        /// <summary>
        /// Устанавливает или возвращает телефон пользователя
        /// </summary>
        public override string UserTel
        {
            get
            {
                return userTel;
            }
            set
            {
                if(value != userTel)
                {
                    userTel = value;
                    OnPropertyChanged(this, "UserTel");
                }
            }
        }

        public override string ToString()
        {
            return userFio;
        }

        //Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}
