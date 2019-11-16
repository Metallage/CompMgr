using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr.Model.Entities.Abstract
{
    interface IInstall
    {
        int Id { get; set; }
        int ComputerId { get; set; }
        int SoftwareId { get; set; }
        string Version { get; set; }
    }
}
