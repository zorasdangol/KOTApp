
using KOTAppClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Linq;
using Xamarin.Forms;
using KOTApp.Interfaces;

namespace KOTApp.SQLiteAccess
{
    public class LoginUser
    {
        public static bool LoadUserAndIP(string DatabaseLocation)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(DatabaseLocation))
                {
                    conn.CreateTable<User>();
                    var apiRow = conn.Table<User>().ToList();
                    if (apiRow.Count > 0)
                    {
                        Helpers.Constants.User = apiRow.FirstOrDefault();
                  
                        Helpers.Constants.User.ip1 = apiRow.FirstOrDefault().ip1;
                        Helpers.Constants.User.ip2 = apiRow.FirstOrDefault().ip2;
                        Helpers.Constants.User.ip3 = apiRow.FirstOrDefault().ip3;
                        Helpers.Constants.User.ip4 = apiRow.FirstOrDefault().ip4;
                        Helpers.Constants.User.Port = apiRow.FirstOrDefault().Port;
                        
                        Helpers.Constants.SetMainURL(Helpers.Constants.User);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                DependencyService.Get<IMessage>().ShortAlert(e.Message);
                return false;
            }
        }

        public static bool SetUserAndIP(string DatabaseLocation, User User)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(DatabaseLocation))
                {
                    conn.CreateTable<User>();
                    
                    conn.DeleteAll<User>();
                    int rows = conn.Insert(User);
                    return true;
                }
            }
            catch (Exception e)
            {
                DependencyService.Get<IMessage>().ShortAlert(e.Message);
                return false;
            }
        }

        public static bool DeleteUserAndIP(string DatabaseLocation)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(DatabaseLocation))
                {
                    conn.CreateTable<User>();

                    conn.DeleteAll<User>();
                    return true;
                }
            }
            catch (Exception e)
            {
                DependencyService.Get<IMessage>().ShortAlert(e.Message);
                return false;
            }
        }

    }
}
