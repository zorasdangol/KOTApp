using KOTApp.Helpers;
using KOTApp.ViewModels.TableTransfer;
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
	public partial class TransferTablePage : ContentPage
	{
        public TransferTablePageVM viewModel { get; set; }
		public TransferTablePage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new TransferTablePageVM();
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
        protected override bool OnBackButtonPressed()
        {
            var res = Services.OnBackCheck(viewModel.IsLoading, viewModel.IsTransfer, viewModel.IsMerge);
            if (res == true)
            {
                viewModel.IsLoading = false;
                viewModel.IsTransfer = false;
                viewModel.IsMerge = false;
                return true;
            }
            return base.OnBackButtonPressed();

        }
    }
}