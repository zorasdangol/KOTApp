using KOTApp.DataAccessLayer;
using KOTApp.Interfaces;
using KOTApp.SQLiteAccess;
using KOTApp.Views;
using KOTAppClassLibrary.DataValidationLayer;
using KOTAppClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace KOTApp.ViewModels
{
    public class LoginPageVM:BaseViewModel
    {
        private bool _IsLoading;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set { _IsLoading = value; OnPropertyChanged("IsLoading"); }
        }

        private string _ip1;
        public string ip1
        {
            get { return _ip1; }
            set
            {
                _ip1 = value;
                OnPropertyChanged("ip1");
            }
        }

        private string _ip2;
        public string ip2
        {
            get { return _ip2; }
            set
            {
                _ip2 = value;
                OnPropertyChanged("ip2");
            }
        }

        private string _ip3;
        public string ip3
        {
            get { return _ip3; }
            set
            {
                _ip3 = value;
                OnPropertyChanged("ip3");
            }
        }
        private string _ip4;
        public string ip4
        {
            get { return _ip4; }
            set
            {
                _ip4 = value;
                OnPropertyChanged("ip4");
            }
        }

        private string _Port;
        public string Port
        {
            get { return _Port; }
            set
            {
                _Port = value;
                OnPropertyChanged("Port");
            }
        }

        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set
            {
                _UserName = value;
                OnPropertyChanged("UserName");
            }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                OnPropertyChanged("Password");
            }
        }

        //private string _PasswordKB;
        //public string PasswordKB
        //{
        //    get { return _PasswordKB; }
        //    set
        //    {
        //        if (value == null)
        //            return;
        //        _PasswordKB = value;
        //        OnPropertyChanged("PasswordKB");
        //    }
        //}

        private bool _HidePassword;
        public bool HidePassword
        {
            get { return _HidePassword; }
            set
            {
                _HidePassword = value;
                OnPropertyChanged("HidePassword");
            }
        }



        public User User { get; set; }

        public Command LoginCommand { get; set; }

        public LoginPageVM()
        {
            UserName = Helpers.Constants.User.UserName;
            Password = Helpers.Constants.User.Password;
            ip1 = Helpers.Constants.User.ip1;
            ip2 = Helpers.Constants.User.ip2;
            ip3 = Helpers.Constants.User.ip3;
            ip4 = Helpers.Constants.User.ip4;
            Port = Helpers.Constants.User.Port;

            HidePassword = true;
          
            LoginCommand = new Command(ExecuteLoginCommand);
            User = new User();
            IsLoading = false;
        }

        public async void ExecuteLoginCommand()
        {
            try
            {
                User.UserName = UserName;
                User.Password = Password;
                User.ip1 = ip1;
                User.ip2 = ip2;
                User.ip3 = ip3;
                User.ip4 = ip4;
                User.Port = Port;
                
                Helpers.Constants.SetMainURL(User);
                var functionResponse = UserValidator.CheckUser(User);                
                if (functionResponse.status == "error")
                {
                    DependencyService.Get<IMessage>().ShortAlert(functionResponse.Message);
                }
                else
                {
                    //DependencyService.Get<IMessage>().ShortAlert("Connecting to Server. Please Wait..");
                    IsLoading = true;
                    functionResponse = await LoginConnection.CheckAccess(User);
                    if (functionResponse.status == "ok")
                    {
                        Helpers.Constants.User = User;
                        Helpers.Constants.SetMainURL(User);
                        LoginUser.SetUserAndIP(App.DatabaseLocation, User);
                        DependencyService.Get<IMessage>().ShortAlert("Logged In Successfully");

                        var menuitemResponse = await LoadMenuItem.GetMenuItemAsync();
                        if(menuitemResponse.status == "ok")
                        {
                            Helpers.Constants.MenuItemsList = JsonConvert.DeserializeObject<List<KOTAppClassLibrary.Models.MenuItem>>(menuitemResponse.result.ToString());
                            SaveMenuItems.SaveList(App.DatabaseLocation, Helpers.Constants.MenuItemsList);
                        }

                        App.Current.MainPage = new NavigationPage(new HomePage());
                        //App.Current.MainPage = (new MasterPage());
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().ShortAlert(functionResponse.Message);
                    }
                    IsLoading = false;
                }
            }catch(Exception e)
            {
                DependencyService.Get<IMessage>().ShortAlert("Error::" + e.Message);
            }
        }
    }
}
