using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CompMgr.Model
{
    public class ErrorArgs : EventArgs
    {
        public string Stage { get; }
        public string Message { get; }

        public ErrorArgs(string stage, string message)
        {
            Stage = stage;
            Message = message;
        }
    }
}
