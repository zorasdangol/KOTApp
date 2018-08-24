using KOTApp.DataAccessLayer;
using KOTApp.Interfaces;
using KOTApp.SQLiteAccess;
using KOTApp.Views;
using KOTApp.Views.KOT;
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

        private string _LoadingMessage;
        public string LoadingMessage
        {
            get { return _LoadingMessage; }
            set { _LoadingMessage = value; OnPropertyChanged("LoadingMessage"); }
        }

        public Command MenuCommand { get; set; }

        public List<KOTAppClassLibrary.Models.MenuItem> MenuItemsList { get; set; }

        public HomePageVM()
        {
            User = Helpers.Constants.User;
            MenuCommand = new Command<string>(ExecuteMenuCommand);
            IsLoading = false;
            LoadingMessage = "";
            MenuItemsList = new List<KOTAppClassLibrary.Models.MenuItem>();
        }

        public async void ExecuteMenuCommand(string index)
        {
            if(index == "1")
            {
                IsLoading = true;
                LoadingMessage = "Please Wait, Checking pending KOT";
                var res = await TableDataConnection.CheckPendingKOT();
                LoadingMessage = "Please Wait, Checking Tables";
                if(res == "0")
                {

                    var functionResponse = await TableDataConnection.GetTable();
                    if(functionResponse.status == "ok")
                    {
                        Helpers.Constants.TableList = functionResponse.result as List<TableDetail>;
                    }
                    else
                    {
                        Helpers.Constants.TableList = new List<TableDetail>(); 
                    }

                    var packedResponse = await TableDataConnection.GetTableNo();
                    if(packedResponse.status == "ok")
                    {
                        Helpers.Constants.PackedTableList = packedResponse.result as List<TableDetail>;
                    }
                    else
                    {
                        Helpers.Constants.PackedTableList = new List<TableDetail>();
                    }

                    IsLoading = false;
                    if(Helpers.Constants.TableList.Count > 0)
                    {
                        await App.Current.MainPage.Navigation.PushAsync(new ChooseTablePage());
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().ShortAlert("No Tables available.");
                    }
                   
                }
                else if(res == "1")
                {
                    await App.Current.MainPage.DisplayAlert("KOT Pending", "Clear all the pending to start new KOT","Ok");
                }
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert("Cannot Connect to Server.");
                }
                IsLoading = false;
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
                {
                    LoginUser.DeleteUserAndIP(App.DatabaseLocation);
                    App.Current.MainPage = new LoginPage();
                }

            }

        }
    }
}
