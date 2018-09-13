using KOTApp.Interfaces;
using KOTApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KOTApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
        public HomePageVM viewModel { get; set; }
        public bool BExit { get; set; }
		public HomePage ()
		{
			InitializeComponent ();
            BExit = true;
            BindingContext = viewModel = new HomePageVM();
		}

        protected override bool OnBackButtonPressed()
        {
            if (BExit)
            {
                DependencyService.Get<IMessage>().ShortAlert("Press again to exit");
                BExit = false;
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}