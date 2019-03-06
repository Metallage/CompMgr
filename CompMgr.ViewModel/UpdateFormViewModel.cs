using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CompMgr.Model;

namespace CompMgr.ViewModel
{
    public class UpdateFormViewModel
    {
        private Logic core;
        private ObservableCollection<ViewUpdate> updates = new ObservableCollection<ViewUpdate>();
        private string softName;

        public UpdateFormViewModel(Logic core, string softName)
        {
            this.core = core;
            this.softName = softName;
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
    }
}
