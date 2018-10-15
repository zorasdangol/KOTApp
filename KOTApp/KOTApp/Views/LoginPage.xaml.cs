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
	public partial class LoginPage : ContentPage
	{
        public LoginPageVM viewModel { get; set; }
        public bool BExit { get; set; }
        public LoginPage ()
		{
			InitializeComponent ();
            BExit = true;
            BindingContext = viewModel = new LoginPageVM();
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