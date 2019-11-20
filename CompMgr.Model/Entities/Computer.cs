using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompMgr.Model.Entities.Abstract;

namespace CompMgr.Model.Entities
{
    class Computer:IComputer
    {
        public int Id { get; set; }
        public string NsName { get; set; }
        public string Ip { get; set; }
        public int? UserId { get; set; }
        public int? DivisionId { get; set; }
    }
}
