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
	public partial class PreBillPage : ContentPage
	{
        public PreBillPageVM viewModel { get; set; }
		public PreBillPage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new PreBillPageVM();
		}

        public void MenuItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var listview = sender as ListView;
            listview.SelectedItem = null;
        }
    }
}