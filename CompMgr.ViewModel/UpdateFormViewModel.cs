using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

using CompMgr.Model;


namespace CompMgr.ViewModel
{
    public class UpdateFormViewModel : INotifyPropertyChanged
    {
        private ModelCore core;
        private ObservableCollection<ViewUpdate> updates = new ObservableCollection<ViewUpdate>();
        private string softName;

        private List<long> upFinish = new List<long>();

        public delegate void CoreReadyEventHandler();

        public delegate void RefreshEventHandler();


        public event RefreshEventHandler onRefresh;
        public event CoreReadyEventHandler CoreReady;
        



        public UpdateFormViewModel(ModelCore core, string softName)
        {
            this.core = core;
            this.softName = softName;


        }


        public UpdateFormViewModel()
        {
            softName = "Ордер";

            core = new ModelCore();
            //core.onReady += Core_onReady;

            var coreStart = Task.Factory.StartNew(() =>
            {
                core.Start();
                CoreReady?.Invoke();
            }
            );

        }

        public void Bind()
        {
            updates.Clear();
            ImportUpdates();
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

        public void Changed(string nsName)
        {
            ViewUpdate vu = FindUpdateByNsName(nsName);
            if (vu != null)
            {
                if (vu.IsUp)
                    upFinish.Add(vu.Id);
                else
                    upFinish.Remove(vu.Id);
            }
        }

        public void SaveChanges()
        {
            var SaveUp = Task.Factory.StartNew(() =>
            {
                core.SaveUpdate(upFinish);
                CoreReady?.Invoke();
            });

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
                //???
                Dispatcher ds = Dispatcher.CurrentDispatcher;
                ds.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    this.updates.Add(iUpd);
                });

            }
            onRefresh?.Invoke();
  
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ViewUpdate FindUpdateByNsName(string nsName)
        {
            foreach (ViewUpdate vu in updates)
                if (vu.NsName == nsName)
                    return vu;
            return null;
        }
    }
}
