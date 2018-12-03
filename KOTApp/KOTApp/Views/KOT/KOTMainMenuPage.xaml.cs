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
	public partial class KOTMainMenuPage : ContentPage
	{
        public KOTProdTabbedPageVM viewModel { get; set; }
		public KOTMainMenuPage ()
		{
			InitializeComponent ();
            //BindingContext = viewModel = new KOTProdTabbedPageVM();
            
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

        public void Focused_Handler(object sender, Xamarin.Forms.FocusEventArgs e)
        {
            PaxEntry.Text = "";
        }


        public void Tapped_Handler(object sender, Xamarin.Forms.TappedEventArgs e)
        {
            SearchEntry.Unfocus();
        }


    }
}