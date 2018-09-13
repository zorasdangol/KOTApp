
using KOTApp.DataAccessLayer;
using KOTApp.Interfaces;
using KOTApp.SQLiteAccess;
using KOTApp.Views;
using KOTAppClassLibrary.DataValidationLayer;
using KOTAppClassLibrary.Models;
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
            var res = LoginUser.LoadUserAndIP(App.DatabaseLocation);
            if (res)
            {
                var User = Helpers.Constants.User;
                var functionResponse = UserValidator.CheckUser(User);
                if (functionResponse.status == "error")
                {
                    DependencyService.Get<IMessage>().ShortAlert(functionResponse.Message);
                }
                else
                {
                    //functionResponse = await LoginConnection.CheckAccessAsync(User);
                    //if (functionResponse.status == "ok")
                    //{
                    //    Helpers.Data.MenuItemsList = MenuItemsAccess.LoadList(App.DatabaseLocation);
                    //    MainPage = new NavigationPage(new HomePage());
                    //}
                    //else
                    //{
                    //    DependencyService.Get<IMessage>().ShortAlert(functionResponse.Message);
                    //    MainPage = new LoginPage();
                    //}

                    MainPage = new LoginPage();

                }                
            }
            else
                MainPage = new LoginPage();

        }

        protected override void OnStart ()
		{
			// Handle when your app starts
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
