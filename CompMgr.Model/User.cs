using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace CompMgr.Model
{
    public class User:IComparable
    {
        protected string userFio = String.Empty;
        protected string userTel = String.Empty;

        public long Id { get; set; }


        public string UserFio {
            get
            {
                return userFio;
            }
            set
            {
                userFio = value;
            }
        }

        public string UserTel
        {
            get
            {
                return userTel;
            }
            set
            {
                userTel = value;
            }
        }

        public User()
        { }

        public User (long id, string fio)
        {
            Id = id;
            UserFio = fio;
        }

        public int CompareTo(object obj)
        {
            User u1 = obj as User;
            if(u1!=null)
            {
                return this.UserFio.CompareTo(u1.UserFio);
            }
            else
            {
                throw new Exception("Не возможно сравнить 2 объекта");
            }
        }

    }
}
