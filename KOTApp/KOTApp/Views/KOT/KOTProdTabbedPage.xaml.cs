﻿using KOTApp.Helpers;
using KOTApp.Interfaces;
using KOTApp.ViewModels.KOT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace KOTApp.Views.KOT
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class KOTProdTabbedPage : MasterDetailPage
	{
        public KOTProdTabbedPageVM viewModel { get; set; }
        public bool BExit { get; set; }


        //private bool _BExit;
        //public bool BExit
        //{
        //    get { return _BExit; }
        //    set
        //    {
        //        _BExit = value;
        //        OnBackButtonPressed();
        //    }
        //}

		public KOTProdTabbedPage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new KOTProdTabbedPageVM();
            NavigationPage.SetHasNavigationBar(this,false);
            BExit = true;
        }

        public async void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var listView = sender as ListView;
            IsPresented = false;
        }

        public void MenuItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var listview = sender as ListView;
            listview.SelectedItem = null;            
        }

        protected override bool OnBackButtonPressed()
        {
            var res = Services.OnBackCheck(viewModel.IsLoading, viewModel.IsCancel, viewModel.IsPax);
            if (res == true)
            {
                viewModel.IsLoading = false;
                viewModel.IsCancel = false;
                viewModel.IsPax = false;
                return true;
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (await DisplayAlert("Alert", "Are you sure you want to go back?", "Yes", "No"))
                    {
                        base.OnBackButtonPressed();
                        if (Navigation.ModalStack.Count > 0)
                        {
                            await App.Current.MainPage.Navigation.PopModalAsync();
                        }
                        else if (Navigation.NavigationStack.Count > 1)
                        {
                            await App.Current.MainPage.Navigation.PopAsync();
                        }
                        else
                        {
                            base.OnBackButtonPressed();
                        }
                    }
                }
                );
                return true;
            }              

        }    

    }
}