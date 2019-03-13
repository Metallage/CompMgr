using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompMgr.Model;


namespace CompMgr.ViewModel
{
    public class MainWindowViewModel
    {
        private ModelCore core = new ModelCore();

        public delegate void RefreshEventHadnler();
        public event RefreshEventHadnler CoreStarted;
        public event RefreshEventHadnler DataUpdate;


        public MainWindowViewModel()
        {
            FirstLoad();
        }

        public UpdateFormViewModel StartUpdate(string softName)
        {
            UpdateFormViewModel updateViewModel = new UpdateFormViewModel(core, softName);

            return updateViewModel;
        }

        public SelectSoftViewModel SelectSoftToUpdate()
        {
            SelectSoftViewModel ssvm = new SelectSoftViewModel(core);

            return ssvm;
        }

        public void UpdateSoft(string softName, string version)
        {
            core.UpdateSoft(softName, version);
        }

        private void FirstLoad()
        {
            core.DataUpdate += Core_DataUpdate;
            var firstLuanch = Task.Factory.StartNew(() => 
            {
                core.Start();
                CoreStarted?.Invoke();
            });


        }

        private void Core_DataUpdate()
        {
            DataUpdate?.Invoke();
        }
    }
}
