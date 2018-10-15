using KOTApp.ViewModels.KOT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KOTApp.Views.KOT
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class KOTDetailPage : ContentPage
	{
        //public KOTDetailPageVM viewModel { get; set; }
		public KOTDetailPage ()
		{
			InitializeComponent ();
           // BindingContext = viewModel = new KOTDetailPageVM();
		}

        protected override void OnAppearing()
        {
            var vm = BindingContext as KOTProdTabbedPageVM;
            vm.RefreshOrderItemsList();
            //viewModel.RefreshOrderList();
        }

        public void MenuItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var listview = sender as ListView;
            listview.SelectedItem = null;
        }

        protected override bool OnBackButtonPressed()
        {
           return true;
        }
    }

    
}