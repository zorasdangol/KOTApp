using KOTApp.ViewModels.KOTMemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KOTApp.Views.KOTMemo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TableViewPage : ContentPage
	{
        public TableViewPageVM viewModel { get; set; }
		public TableViewPage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new TableViewPageVM();
		}

        public void MenuItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var listview = sender as ListView;
            listview.SelectedItem = null;
        }

        protected override bool OnBackButtonPressed()
        {
            if(viewModel.IsLoading || viewModel.IsRemarks)
            {
                viewModel.IsLoading = false;
                viewModel.IsRemarks = false;                     
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}