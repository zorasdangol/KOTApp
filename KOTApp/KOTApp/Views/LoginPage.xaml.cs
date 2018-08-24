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
		public LoginPage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new LoginPageVM();
		}
	}
}