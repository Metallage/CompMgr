using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompMgr.Model.Entities.Abstract;

namespace CompMgr.Model.Entities
{
    class Division : IDivision
    {
        public int Id { get; set; }

        public string DivisionName { get; set; }
    }
}
