using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CompMgr.Model;

namespace CompMgr.ViewModel
{
    public class UpdateFormViewModel : INotifyPropertyChanged
    {
        private ModelCore core;
        private ObservableCollection<ViewUpdate> updates = new ObservableCollection<ViewUpdate>();
        private string softName;

        public UpdateFormViewModel(ModelCore core, string softName)
        {
            this.core = core;
            this.softName = softName;
        }

        public ObservableCollection<ViewUpdate> Updates
        {
            get
            {
                return updates;
            }
            set
            {
                if (value != updates)
                {
                    updates = value;
                    OnPropertyChanged(this, "Updates");
                }
            }
        }

        public string UpdateBanner
        {
            get
            {
                return $"Обновления для {softName}";
            }
        }

        private void ImportUpdates()
        {
            List<Update> updates = core.GetUpdates(softName);
            foreach (Update upd in updates)
            {
                ViewUpdate iUpd = new ViewUpdate();
                iUpd.Id = upd.Id;
                iUpd.NsName = upd.NsName;
                iUpd.Ip = upd.Ip;
                iUpd.UserFio = upd.UserFio;
                iUpd.OldVersion = upd.OldVersion;
                iUpd.CurrentVersion = upd.CurrentVersion;
                this.updates.Add(iUpd);
            }
  
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
