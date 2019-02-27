using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr
{
    public class Computer
    {

        public string NsName { get; set; }
        public string Ip { get; set; }

        public string UserFio { get; set; }
        public string DivisionName { get; set; }



        public Computer(string nsName, string ip)
        {
            NsName = nsName;
            Ip = ip;
            
        }

        public override int GetHashCode()
        {
            if (NsName != null)
                return NsName.GetHashCode();
            else
                return 0;
        }

        public override bool Equals(object obj)
        {

            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() == typeof(Computer))
            {
                Computer objAsComp = (Computer)obj;
                if (objAsComp == null)
                {
                    return false;
                }
                else if (objAsComp.NsName == this.NsName)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return NsName;
        }
    }
}
