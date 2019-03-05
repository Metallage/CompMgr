using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;

namespace CompMgr
{
    public class ListBoxCommands
    {
   
        static ListBoxCommands()
        {
            DeleteItem = new RoutedCommand("Delete_Item", typeof(ListBox));
        }

        public static RoutedCommand DeleteItem { get; set; }
    }
}
