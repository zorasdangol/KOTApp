﻿using KOTApp.ViewModels.TableTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KOTApp.Views.TableTransfer
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MergeTablePage : ContentPage
	{
        public MergeTablePageVM viewModel { get; set; }
		public MergeTablePage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new MergeTablePageVM();
		}

        public void MenuItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var listview = sender as ListView;
            listview.SelectedItem = null;
        }

        protected override bool OnBackButtonPressed()
        {
            //var res = Services.OnBackCheck(viewModel.IsLoading, viewModel.IsCancel, viewModel.IsPax);
            //if (res == true)
            //{
            //    viewModel.IsLoading = false;
            //    viewModel.IsCancel = false;
            //    viewModel.IsPax = false;
            //    return true;
            //}
            //else
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