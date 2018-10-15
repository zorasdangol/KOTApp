using KOTAppClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KOTApp.Helpers
{
    public class Constants
    {
        public const decimal VAT = 13;
        public const decimal ServiceCharge = 10;

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
        public static string MenuItemsURL = "getMenuItems";

        //public static string DayCloseTableURL = "GetTableDetails?DayCloseTable=1";
        public static string DayCloseTableURL = "GetDayCloseTable?";

        //public static string GetTableURL = "GetTableDetails?GetTable=1";
        public static string GetTableURL = "GetTable?";

        //public static string GetPackedTableURL = "GetTableDetails?GetPackedTable=1";
        public static string GetPackedTableURL = "GetPackedTable?";

        //public static string GetTableDetailsURL = "GetTableDetails?TableNo={0}";
        public static string GetTableItemsDetailsURL = "GetTableItemsDetail?TableNo={0}";

        public static string CheckAccessURL = "CheckAccess";

        public static string TransferAllTableURL = "transferAllTable?tableNew={0}&tableOld={1}";
        public static string MergeTableURL = "MergeTable";
        public static string SplitTableURL = "SplitTable";

        //public static string SaveKOTProdListURL = "GetTableDetails?TableNo={0},transferData={1},TRNUSER={2},PAX={3}";

        public static string SaveKOTProdListURL = "GetTableDetails?";

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