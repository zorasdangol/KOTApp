using KOTApp.ViewModels.KitchenDispatch;
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
	public partial class DispatchSplitPage : ContentPage
	{
        public DispatchSplitPageVM viewModel { get; set; }
		public DispatchSplitPage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new DispatchSplitPageVM();
		}

        public void MenuItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var listview = sender as ListView;
            listview.SelectedItem = null;
        }

        public void FR_Focused_Handler(object sender, FocusEventArgs e)
        {
            FREntry.Text = "";
        }
        public void SR_Focused_Handler(object sender, FocusEventArgs e)
        {
            SREntry.Text = "";
        }


        public void FQ_Focused_Handler(object sender, FocusEventArgs e)
        {
            FQEntry.Text = "";
        }

        public void SQ_Focused_Handler(object sender, FocusEventArgs e)
        {
            SQEntry.Text = "";
        }
    }
}