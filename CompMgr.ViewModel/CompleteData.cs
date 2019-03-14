using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CompMgr.Model;

namespace CompMgr.ViewModel
{
    public class CompleteData:ObservableCollection<CompleteTableHelper>
    {

        public CompleteData()
        { }

        public CompleteData(List<CompleteTableHelper> data)
        {
            foreach (CompleteTableHelper cth in data)
                this.Add(cth);
        }

        public CompleteData(ObservableCollection<CompleteTableHelper> data)
        {
            foreach (CompleteTableHelper cth in data)
                this.Add(cth);
        }

        public void InputData(List<CompleteTableHelper> data)
        {
            foreach (CompleteTableHelper cth in data)
                this.Add(cth);
        }

    }
}
