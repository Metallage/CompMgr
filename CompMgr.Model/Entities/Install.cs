using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompMgr.Model.Entities.Abstract;

namespace CompMgr.Model.Entities
{
    class Install:IInstall
    {
        public int Id { get; set; }
        public int ComputerId { get; set; }
        public int SoftwareId { get; set; }
        public string Version { get; set; }
    }
}
