using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr
{
    public class User
    {
        public string UserFio { get; set; }
        public string UserTel { get; set; }

        public override string ToString()
        {
            return UserFio;
        }

        public override int GetHashCode()
        {
            if (UserFio != null)
                return UserFio.ToLower().GetHashCode();
            else
                return 0;
        }

        public override bool Equals(object obj)
        {
            if((obj!=null)&&(obj.GetType()==typeof(User)))
            {
                User objAsUser = (User)obj;
                if ((objAsUser != null) && (objAsUser.UserFio.ToLower() == this.UserFio.ToLower()))
                    return true;
            }
            return false;
        }
    }
}
