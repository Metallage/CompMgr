using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr
{
    public class Software
    {
        public string SoftwareName { get; }
        public string SoftwareVersion { get; set; }

        public Software(string name)
        {
            SoftwareName = name;
        }

        public override int GetHashCode()
        {
            if (SoftwareName != null)
                return SoftwareName.ToLower().GetHashCode();
            else
                return 0;
        }

        public override bool Equals(object obj)
        {
            if ((obj != null) && (obj.GetType() == typeof(Software)))
            {
                Software objAsSoft = (Software)obj;
                if ((objAsSoft != null) && (objAsSoft.SoftwareName.ToLower() == this.SoftwareName.ToLower()))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public override string ToString()
        {
            return SoftwareName;
        }
    }
}
