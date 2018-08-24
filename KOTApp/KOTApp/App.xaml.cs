
using KOTApp.SQLiteAccess;
using KOTApp.Views;
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

            var res = LoginUser.LoadUserAndIP(App.DatabaseLocation);

            if (res)
                MainPage = new NavigationPage(new HomePage());
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
