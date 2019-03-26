using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CompMgr.ViewModel
{
    class EditWindowVM:INotifyPropertyChanged
    {
        #region Поля и свойства
        //Здесь хранится список ПО
        private ObservableCollection<SoftVM> softwareList = new ObservableCollection<SoftVM>();

        //Здесь хранится список компьютеров
        private ObservableCollection<ComputerVM> compList = new ObservableCollection<ComputerVM>();

        /// <summary>
        /// Здесь хранится список пользователей
        /// </summary>
        private ObservableCollection<UserVM> userList = new ObservableCollection<UserVM>();


        /// <summary>
        /// Поле для хранения выбранного ПО
        /// </summary>
        private SoftVM selectedSoft = new SoftVM();

        /// <summary>
        /// Поле для хранения выделенного компьютера
        /// </summary>
        private ComputerVM selectedComp = new ComputerVM();

        /// <summary>
        /// Поле для хранения выбранного пользователя
        /// </summary>
        private UserVM selectedUser = new UserVM();

        /// <summary>
        /// Свойство на доступ к списку ПО softwareList
        /// </summary>
        public ObservableCollection<SoftVM> SoftwareList
        {
            get
            {
                return softwareList;
            }
            set
            {
                if(value!=softwareList)
                {
                    softwareList = value;
                    OnPropertyChanged(this, "SoftwarwList");
                }
            }
        }

        /// <summary>
        /// Свойство на доступ к списку компьютеров compList
        /// </summary>
        public ObservableCollection<ComputerVM> CompList
        {
            get
            {
                return compList;
            }
            set
            {
                if(value != compList)
                {
                    compList = value;
                    OnPropertyChanged(this, "CompList");
                }
            }
        }

        /// <summary>
        /// Устанавливает или возвращает список пользователей
        /// </summary>
        public ObservableCollection<UserVM> UserList
        {
            get
            {
                return userList;
            }
            set
            {
                if(value != userList)
                {
                    userList = value;
                    OnPropertyChanged(this, "UserList");
                }
            }
        }

        /// <summary>
        /// Устанавливает или возвращает выбранное ПО
        /// </summary>
        public SoftVM SelectedSoft
        {
            get
            {
                return selectedSoft;
            }
            set
            {
                if(value != selectedSoft)
                {
                    selectedSoft = value;
                    OnPropertyChanged(this, "SelectedSoft");
                }
            }
        }

        /// <summary>
        /// Устанавливает или возвращает выделенный компьютер
        /// </summary>
        public ComputerVM SelectedComp
        {
            get
            {
                return selectedComp;
            }
            set
            {
                if(value!= selectedComp)
                {
                    selectedComp = value;
                    OnPropertyChanged(this, "SelectedComp");
                }
            }
        }

        #endregion



        #region реализация интерфейса INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
