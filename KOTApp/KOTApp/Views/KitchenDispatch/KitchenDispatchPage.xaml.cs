using KOTApp.ViewModels.KitchenDispatch;
using KOTAppClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KOTApp.Views.KitchenDispatch
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class KitchenDispatchPage : ContentPage
	{
        public KitchenDispatchPageVM viewModel { get; set; }
		public KitchenDispatchPage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new KitchenDispatchPageVM();
		}

        protected override void OnAppearing()
        {
            viewModel.ExecuteRefreshCommand();
            base.OnAppearing();
        }
        public void MenuItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var listview = sender as ListView;
            listview.SelectedItem = null;
        }

        public void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as KOTProd;
            viewModel.HideOrShowButton(item);
        }

        public void RE_Focused_Handler(object sender, FocusEventArgs e)
        {
            viewModel.Remarks = "";
        }


        public void RE_Unfocused_Handler(object sender, FocusEventArgs e)
        {
            viewModel.Remarks = "Order Dispatched";
        }
    }
}