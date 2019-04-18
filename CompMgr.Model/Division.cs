using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr.Model
{
    public class Division
    {
        protected string divisionName = String.Empty;

        public Division()
        {

        }

        public Division(string divisionName)
        {
            this.divisionName = divisionName;
        }

        public virtual string DivisionName
        {
            get
            {
                return divisionName;
            }
            set
            {
                divisionName = value;
            }
        }

    }
}
