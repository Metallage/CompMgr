using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr.Model
{
    /// <summary>
    /// Служит для передачи информации о необходимых обновления 
    /// </summary>
    public struct Update
    {
        public string NsName { get; }
        public string Ip { get; }
        public string UserFio { get; }
        public string OldVersion { get; }
        public string CurrentVersion { get; }
        public long Id { get; }

        public Update( long id, string nsName, string ip, string userFio, string oldVersion, string currentVersion)
        {
            NsName = nsName;
            Ip = ip;
            UserFio = userFio;
            OldVersion = oldVersion;
            Id = id;
            CurrentVersion = currentVersion;
        }
    }
}
