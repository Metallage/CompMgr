using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompMgr.Model;

namespace CompMgr.ViewModel
{
    public class MainWindowViewModel
    {
        private ModelCore core = new ModelCore();

        public MainWindowViewModel()
        {
            core.Start();
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
    }
}
