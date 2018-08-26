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
	public partial class KOTProdPage : MasterDetailPage
	{
		public KOTProdPage ()
		{
			InitializeComponent ();
		}

        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new NavigationPage(new ChooseTablePage());
            return true;
        }
    }
}