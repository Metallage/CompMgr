using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;
using CompMgr.Model;

namespace CompMgr.ViewModel
{
    public class SelectSoftViewModel: INotifyPropertyChanged
    {
        private List<ViewSoft> softList = new List<ViewSoft>();
        private ViewSoft selectedSoft = new ViewSoft();
        public List<string> SoftNames { get; set; } = new List<string>();

        public string NewVersion { get; set; }

        public SelectSoftViewModel(ModelCore core)
        {
            ParseSoft(core.GetSoftwareDT());
        }


        public SelectSoftViewModel()
        {
            ModelCore core = new ModelCore();
            core.Start();
            ParseSoft(core.GetSoftwareDT());
        }

        public string SelectedSoftName
        {
            get
            {
                return selectedSoft.SoftName;
            }
            set
            {
                if (FindByName(value) != null)
                {
                    selectedSoft = FindByName(value);
                    OnPropertyChanged(this, "SelectedSoftName");
                    OnPropertyChanged(this, "SelectedSoftVersion");
                }
            }
        }

        public string SelectedSoftVersion
        {
            get
            {
                return selectedSoft.SoftVersion;
            }
            //set
            //{
            //    selectedSoft.SoftVersion = value;
            //}
        }

        public List<ViewSoft> SoftList
        {
            get
            {
                return softList;
            }
        }

        private ViewSoft FindByName(string softName)
        {
            foreach (ViewSoft vs in softList)
                if (vs.SoftName == softName)
                    return vs;

            return null;
        }

        private void ParseSoft(DataTable soft)
        {
            if(soft!=null)
                foreach(DataRow dr in soft.Rows)
                {
                    ViewSoft newSoft = new ViewSoft();
                    newSoft.Id = dr.Field<long>("id");
                    newSoft.SoftName = dr.Field<string>("softName");
                    newSoft.SoftVersion = dr.Field<string>("version");
                    softList.Add(newSoft);
                    SoftNames.Add(newSoft.SoftName);
                }
            if (softList.Count > 0)
            {
                selectedSoft = softList[0];
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
