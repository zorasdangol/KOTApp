
using KOTApp.DataAccessLayer;
using KOTApp.Interfaces;
using KOTApp.SQLiteAccess;
using KOTApp.Views;
using KOTAppClassLibrary.DataValidationLayer;
using KOTAppClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace KOTApp
{
	public partial class App : Application
	{
        public static string DatabaseLocation = string.Empty;

        public App ()
		{
			InitializeComponent();

			MainPage = new LoginPage();
		}

        public App(string databaseLocation)
        {            

            InitializeComponent();           

            DatabaseLocation = databaseLocation;

            CheckLoggedInAsync();           
        }

        private async void CheckLoggedInAsync()
        {
            try
            {
                var res = LoginUser.LoadUserAndIP(App.DatabaseLocation);
                if (res)
                {
                    var User = Helpers.Constants.User;
                    var functionResponse = UserValidator.CheckUser(User);

                    //if (functionResponse.status == "error")
                    //{
                    //    DependencyService.Get<IMessage>().ShortAlert(functionResponse.Message);
                    //}
                    //else
                    //{
                    //    var response = await LoginConnection.CheckAccessAsync(User);
                    //    if (response.ToLower() == "success")
                    //    {
                    //        Helpers.Data.MenuItemsList = MenuItemsAccess.LoadList(App.DatabaseLocation);
                    //        MainPage = new NavigationPage(new HomePage());
                    //    }
                    //    else
                    //    {
                    //        DependencyService.Get<IMessage>().ShortAlert(functionResponse.Message);
                    //        MainPage = new LoginPage();
                    //    }

                    MainPage = new LoginPage();

                    //}                
                }
                else
                    MainPage = new LoginPage();
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
                MainPage = new LoginPage();
            }

        }

        protected override void OnStart ()
		{
            // Handle when your app starts
            //AppCenter.Start("6c8a11e0-b024-4752-a470-334a4a3ef7be", typeof(Push));        

        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
