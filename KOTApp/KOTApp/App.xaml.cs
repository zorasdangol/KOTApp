
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

            MainPage = (new LoginPage());

            DatabaseLocation = databaseLocation;

            LoginUser.LoadUserAndIP(App.DatabaseLocation);
            
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
