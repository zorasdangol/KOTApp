using KOTApp.ViewModels.KOT;
using KOTAppClassLibrary.Models;
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
	public partial class ListOrderPage : ContentPage
	{
		public ListOrderPage ()
		{
			InitializeComponent ();
		}

        public void MenuItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var listview = sender as ListView;
            listview.SelectedItem = null;
        }

        //public void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        //{
        //    var lv = sender as ListView;
        //    var order = e.Item as KOTProd;
            
        //}
    }
}