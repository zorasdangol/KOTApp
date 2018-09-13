using KOTApp.DataAccessLayer;
using KOTApp.Interfaces;
using KOTApp.SQLiteAccess;
using KOTApp.Views;
using KOTApp.Views.KOT;
using KOTApp.Views.KOTMemo;
using KOTApp.Views.TableTransfer;
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
            try
            {
                if (index == "1")
                {
                    CommonForMemoAndTransfer(1);
                }

                else if (index == "2")
                {
                    CommonForMemoAndTransfer(2);
                }

                else if (index == "3")
                {
                    CommonForMemoAndTransfer(3);
                }

                else if (index == "4")
                {
                    var res = await App.Current.MainPage.DisplayAlert("Confirm", "Are you sure to Sync MenuItems?", "Yes", "No");
                    if (res)
                    {
                        LoadingMessage = "Please Wait, MenuItems Loading";
                        IsLoading = true; 
                        var functionResponse = await LoadMenuItem.GetMenuItemAsync();
                        if (functionResponse.status == "ok")
                        {
                            DependencyService.Get<IMessage>().ShortAlert("MenuItems synced successfully");
                            MenuItemsList = JsonConvert.DeserializeObject<List<KOTAppClassLibrary.Models.MenuItem>>(functionResponse.result.ToString());
                            Helpers.Data.MenuItemsList = MenuItemsList;
                            MenuItemsAccess.SaveList(App.DatabaseLocation, MenuItemsList);
                        }
                        else
                        {
                            DependencyService.Get<IMessage>().ShortAlert("Couldnot sync: " + functionResponse.Message);
                        }
                        IsLoading = false;

                    }
                }

                else if (index == "5")
                {
                    var res = await App.Current.MainPage.DisplayAlert("Confirm", "Are you sure to log Out?", "Yes", "No");
                    if (res)
                    {
                        LoginUser.DeleteUserAndIP(App.DatabaseLocation);
                        App.Current.MainPage = new LoginPage();
                    }

                }
            }catch(Exception e)
            {
                IsLoading = false;
                DependencyService.Get<IMessage>().ShortAlert("Error: " + e.Message);                
            }

        }
        
        public async void  CommonForMemoAndTransfer(int index)
        {
            LoadingMessage = "Please Wait, Checking pending KOT";
            IsLoading = true;
            var res = await TableDataAccess.CheckPendingKOTAsync();
            if (res == "0")
            {
                var functionResponse = await TableDataAccess.GetTableAsync();
                if (functionResponse.status == "ok")
                {
                    var list = JsonConvert.DeserializeObject<List<TableDetail>>(functionResponse.result.ToString());
                    Helpers.Data.TableList = JsonConvert.DeserializeObject<List<TableDetail>>(functionResponse.result.ToString());
                }
                else
                {
                    Helpers.Data.TableList = new List<TableDetail>();
                }
                IsLoading = false;
                if (Helpers.Data.TableList.Count > 0)
                {
                    if(index == 1)
                    {
                        await App.Current.MainPage.Navigation.PushAsync(new ChooseTablePage());
                    }
                    else if(index == 2)
                    {
                        await App.Current.MainPage.Navigation.PushAsync(new TableViewPage());
                    }
                    else if(index == 3)
                    {
                        await App.Current.MainPage.Navigation.PushAsync(new TransferTablePage());
                    }
                }
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert("No Tables available.");
                }
            }
            else if (res == "1")
            {
                await App.Current.MainPage.DisplayAlert("KOT Pending", "Clear all the pending to start new KOT", "Ok");
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Cannot Connect to Server.");
            }
            IsLoading = false;
        }
    }
}
