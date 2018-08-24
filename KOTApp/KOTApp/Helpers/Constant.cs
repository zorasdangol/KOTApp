using KOTAppClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KOTApp.Helpers
{
    public class Constants
    {
        public static User User = new User()
        {
            UserName = "admin",
            Password = "16877",
            ip1 = "192",
            ip2 = "168",
            ip3 = "125",
            ip4 = "173",
            Port = "80",
        };

        //public static string ip1 = "192";
        //public static string ip2 = "168";
        //public static string ip3 = "125";
        //public static string ip4 = "173";
        //public static string Port = "80";

        public static string IPAddress = User.ip1 + "." + User.ip2 + "." + User.ip3 + "." + User.ip4 + ":" + User.Port ;
      
        public static string MainURL = "http://" + IPAddress + "/ImsRms/api/";

        public static string LoginURL = "userVerification";

        public static List<MenuItem> MenuItemsList { get; set; }
        public static List<TableDetail> TableList { get; set; }
        public static List<TableDetail> PackedTableList { get; set; }

        public static string MenuItemsURL = "getMenuItems";
        public static string DayCloseTableURL = "GetTableDetails?DayCloseTable=1";
        public static string GetTableURL = "GetTableDetails?GetTable=1";
        public static string GetTableNoURL = "GetTableDetails?GetTableNo=1";

        public static void SetMainURL(User User)
        {
            IPAddress = User.ip1 + "." + User.ip2 + "." + User.ip3 + "." + User.ip4 + ":" + User.Port;

            MainURL = "http://" + IPAddress + "/ImsRms/api/";
        }

        public static string SetApiURL(string apiUrl)
        {
            return (MainURL + apiUrl);
        }
    }
}