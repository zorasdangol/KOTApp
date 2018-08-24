using KOTApp.Interfaces;
using KOTAppClassLibrary.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace KOTApp.SQLiteAccess
{
    public class SaveMenuItems
    {
        public static bool SaveList(string DatabaseLocation, List<KOTAppClassLibrary.Models.MenuItem> MenuItemsList)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(DatabaseLocation))
                {
                    conn.CreateTable<KOTAppClassLibrary.Models.MenuItem>();

                    conn.DeleteAll<KOTAppClassLibrary.Models.MenuItem>();
                    int rows = conn.InsertAll(MenuItemsList);
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
