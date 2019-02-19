using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompMgr
{
    public class ErrorMessageHelper
    {
        public ErrorMessageHelper(string error)
        {
            HasErrors = true;
            ErrorText = error;
        }

        public ErrorMessageHelper()
        {
            HasErrors = false;
            ErrorText = "Ошибок нет";
        }

        public bool HasErrors { get; }

        public string ErrorText { get; }
    }
}
