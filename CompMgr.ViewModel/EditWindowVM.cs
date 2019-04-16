using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using CompMgr.Model;

namespace CompMgr.ViewModel
{
    /// <summary>
    /// ViewModel для EditWindow
    /// </summary>
    public class EditWindowVM:INotifyPropertyChanged
    {
        #region Поля и свойства
        /// <summary>
        /// Ядро модели
        /// </summary>
        private ModelCore core;

        public delegate void DataUpdateHandler();

        public event DataUpdateHandler DataUpdate;

        //Список талиц для редактирования
        public List<string> TableList { get; } = new List<string>() { "ПО", "Пользователи", "Компьютеры" };


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



        public EditWindowVM (ModelCore core)
        {
            this.core = core;

        }

        public void Start()
        {
            core.DataUpdate += Core_DataUpdate;
            var firstLaunch = Task.Factory.StartNew(() =>
            {
                GetUsers();
                GetSoftware();
                GetComputers();
                DataUpdate?.Invoke();
            });
        }

        private void Core_DataUpdate()
        {
            DataUpdate?.Invoke();
        }

        private void GetComputers()
        {
            List<Computer> compList = core.GetComputers();

            foreach (Computer comp in compList)
                CompList.Add(new ComputerVM
                {
                    NsName = comp.NsName,
                    Ip = comp.Ip
                });
        }

        private void GetUsers()
        {
            List<User> userList = core.GetUsers();

            foreach (User usr in userList)
                UserList.Add(new UserVM
                {
                    UserFio = usr.UserFio,
                    UserTel = usr.UserTel
                });
        }

        private void GetSoftware()
        {
            List<Soft> softList = core.GetSoftware();

            foreach (Soft sft in softList)
                SoftwareList.Add(new SoftVM
                {
                    SoftName = sft.SoftName,
                    SoftVersion = sft.SoftVersion
                });
        }

        public void RemoveSoft(SoftVM delSoft)
        {
            SoftwareList.Remove(delSoft);
        }

        public void AddSoft(string softName, string softVersion)
        {
            SoftwareList.Add(new SoftVM
            {
                SoftName = softName,
                SoftVersion = softVersion
            });
        }

        public void RemoveUser(UserVM delUser)
        {
            UserList.Remove(delUser);
        }

        public void AddUser(string userFio, string userTel)
        {
            UserList.Add(new UserVM
            {
                UserFio = userFio,
                UserTel = userTel
            });
        }


        #region реализация интерфейса INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
