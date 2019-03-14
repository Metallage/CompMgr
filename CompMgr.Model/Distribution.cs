using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr.Model
{
    public struct Distribution
    {
        public long Id { get; set; }
        public string UserFio { get; set; }
        public string NsName { get; set; }
        public long ComputerID { get; set; }
        public long UserID { get; set; }

        public Distribution(long id, long computerID, string nsName, long userID, string userFio)
        {
            Id = id;
            ComputerID = computerID;
            NsName = nsName;
            UserID = userID;
            UserFio = userFio;
        }
    }
}
