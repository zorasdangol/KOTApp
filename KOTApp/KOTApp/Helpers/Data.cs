using KOTAppClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KOTApp.Helpers
{
    public class Data
    {
        public static TableDetail SelectedTable { get; set; }

        public static List<MenuItem> MenuItemsList { get; set; }
        public static List<TableDetail> TableList { get; set; }
        public static List<TableDetail> PackedTableList { get; set; }
        public static List<KOTProd> OrderItemsList { get; set; }

        public static string PAX { get; set; }        
    }
}
