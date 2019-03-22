using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CompMgr.Model;

namespace CompMgr.ViewModel
{
    public class CompleteData:ObservableCollection<CompleteTableHelperVM>
    {

        public CompleteData()
        { }

        public CompleteData(List<CompleteTableHelper> data)
        {
            foreach (CompleteTableHelper cth in data)
            {
            CompleteTableHelperVM cthvm =   new CompleteTableHelperVM()
            {
                SoftName = cth.SoftName,
                SoftVersion = cth.SoftVersion,
            };
            foreach (KeyValuePair<string, bool> kvp in cth.CompNames)
                cthvm.CompNames.Add(new CompListHelper(kvp.Key, kvp.Value));
            this.Add(cthvm);

            }
        }

        public CompleteData(ObservableCollection<CompleteTableHelperVM> data)
        {
            foreach (CompleteTableHelperVM cth in data)
                this.Add(cth);
        }

        public void InputData(List<CompleteTableHelperVM> data)
        {
            foreach (CompleteTableHelperVM cth in data)
                this.Add(cth);
        }

    }
}
