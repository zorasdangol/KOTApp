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
	public partial class ChooseTablePage : ContentPage
	{
        public ChooseTablePageVM viewModel { get; set; }
		public ChooseTablePage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new ChooseTablePageVM();
		}

        public void MenuItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var listview = sender as ListView;
            listview.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            viewModel.ExecuteRefreshCommand();
            base.OnAppearing();
        }

    }
}