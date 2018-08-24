using KOTApp.DataAccessLayer;
using KOTApp.Interfaces;
using KOTApp.SQLiteAccess;
using KOTApp.Views;
using KOTAppClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace KOTApp.ViewModels
{
    public class HomePageVM : BaseViewModel
    {
        private User _User;
        public User User
        {
            get { return _User; }
            set
            {
                if (value == null)
                    return;
                _User = value;
                OnPropertyChanged("User");
            }
        }

        private bool _IsLoading;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set { _IsLoading = value; OnPropertyChanged("IsLoading"); }
        }

        public Command MenuCommand { get; set; }

        public List<KOTAppClassLibrary.Models.MenuItem> MenuItemsList { get; set; }

        public HomePageVM()
        {
            User = Helpers.Constants.User;
            MenuCommand = new Command<string>(ExecuteMenuCommand);
            IsLoading = false;
            MenuItemsList = new List<KOTAppClassLibrary.Models.MenuItem>();
        }

        public async void ExecuteMenuCommand(string index)
        {
            if(index == "1")
            {

            }
            else if(index == "2")
            {

            }
            else if(index == "3")
            {

            }
            else if (index == "4")
            {
                var res = await App.Current.MainPage.DisplayAlert("Confirm", "Are you sure to Sync MenuItems?", "Yes", "No");
                if (res)
                {
                    IsLoading = true;
                    var functionResponse = await LoadMenuItem.GetMenuItemAsync();
                    if(functionResponse.status == "ok")
                    {
                        DependencyService.Get<IMessage>().ShortAlert("MenuItems synced successfully");
                        MenuItemsList = JsonConvert.DeserializeObject<List<KOTAppClassLibrary.Models.MenuItem>>(functionResponse.result.ToString());
                        Helpers.Constants.MenuItemsList = MenuItemsList;
                        SaveMenuItems.SaveList(App.DatabaseLocation, MenuItemsList);
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Couldnot sync: " + functionResponse.Message);
                    }
                    IsLoading = false;

                }
            }
            else if(index == "5")
            {
                var res = await App.Current.MainPage.DisplayAlert("Confirm","Are you sure to log Out?","Yes", "No");
                if(res)
                    App.Current.MainPage = new LoginPage();

            }

        }
    }
}
