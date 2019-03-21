﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CompMgr.Model;

namespace CompMgr.ViewModel
{
    public class InstallVM: Install, INotifyPropertyChanged, IComparable
    {
        private ObservableCollection<InstalledSoftVM> installedSoft = new ObservableCollection<InstalledSoftVM>();

        //public string NsName { get; set; }
        //public string UserFio { get; set; }
       
        public InstallVM ()
        {

        }
        
        public InstallVM(Install inst, List<string> allSoft)
        {
            this.NsName = inst.NsName;
            this.UserFio = inst.UserFio;
            foreach(string softName in allSoft)
            {
                InstalledSoftVM isvm = new InstalledSoftVM(softName);
                string installedSoft = inst.InstalledSoft.Find(x => x == softName);
                
                if(installedSoft!=null)
                {
                    isvm.IsInstalled = true;
                }
                InstalledSoft.Add(isvm);
            }


        }

        public void FlushNotInstalled()
        {
            base.InstalledSoft = new List<string>();

            foreach (InstalledSoftVM isvm in InstalledSoft)
                if (isvm.IsInstalled)
                    base.InstalledSoft.Add(isvm.SoftName);

            

        }

        new public ObservableCollection<InstalledSoftVM> InstalledSoft
        {
            get
            {
                return installedSoft;
            }
            set
            {
                if(value!=installedSoft)
                {
                    installedSoft = value;
                    OnPropertyChanged(this, "InstalledSoft");
                }
            }

        }

       

        public int CompareTo(object obj) // не факт , что надо...
        {
            InstallVM iVM = obj as InstallVM;
            if (iVM != null)
                return this.NsName.CompareTo(iVM.NsName);
            else
                throw new Exception("Невозможно сравнить 2 объекта");

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(object sender , string propertyName)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}