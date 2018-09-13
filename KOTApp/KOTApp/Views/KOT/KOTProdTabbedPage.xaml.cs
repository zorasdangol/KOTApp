using KOTApp.Helpers;
using KOTApp.Interfaces;
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
	public partial class KOTProdTabbedPage : MasterDetailPage
	{
        public KOTProdTabbedPageVM viewModel { get; set; }
        public bool BExit { get; set; }


        //private bool _BExit;
        //public bool BExit
        //{
        //    get { return _BExit; }
        //    set
        //    {
        //        _BExit = value;
        //        OnBackButtonPressed();
        //    }
        //}

		public KOTProdTabbedPage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new KOTProdTabbedPageVM();
            NavigationPage.SetHasNavigationBar(this,false);
            BExit = true;
        }

        public async void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var listView = sender as ListView;
            IsPresented = false;
        }

        public void MenuItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var listview = sender as ListView;
            listview.SelectedItem = null;            
        }

        protected override bool OnBackButtonPressed()
        {
            var res =  Services.OnBackCheck(viewModel.IsLoading, viewModel.IsCancel, viewModel.IsPax);
            if(res == true)
            {
                viewModel.IsLoading = false;
                viewModel.IsCancel = false;
                viewModel.IsPax = false;
                return true;
            }
            else
            {
                return false;

                //if (BExit)
                //{
                //if (!BExit)
                //    return BExit;
                //AskFunction();
                //var task = Task.Run(async () => await AskFunction());
                //task.Wait();
                    //async(() => await DependencyService.Get<IMessage>().ShortAlert("Press again to Return"));
                //return BExit;
                //}               
            }            
        }

        //public async void AskFunction()
        //{
        //    var res =await  DisplayAlert("Confirm","Are you sure to exit?","Yes","No");
        //    BExit = !res;
        //}

    }
}