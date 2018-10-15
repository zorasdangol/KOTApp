using System;
using Android.App;
using Android.Content;
using KOTApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using System.ComponentModel;
using Android.Widget;
using KOTApp.Views;
using KOTApp.Views.KOT;
using KOTApp.Views.KOTMemo;
using KOTApp.Views.TableTransfer;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomNavigationRenderer))]
namespace KOTApp.Droid.Renderers
{
    public class CustomNavigationRenderer : NavigationPageRenderer
    {
        public CustomNavigationRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals("CurrentPage"))
                // If we're on MainPage show the icon, otherwise hide it
                //bool visible = (Element.CurrentPage is MainHomePage);
                // ChangeToolbarIconVisibility(Element.CurrentPage is HomePage);
                ChangeToolbarIconVisibility(Element.CurrentPage);
        }

        // private void ChangeToolbarIconVisibility(bool visible)
        private void ChangeToolbarIconVisibility(Page CurrentPage)
        {
            ImageView toolbarIcon = FindViewById<ImageView>(Resource.Id.toolbarIcon);
            TextView toolbar_title = FindViewById<TextView>(Resource.Id.toolbar_title);

            if (CurrentPage is HomePage)
            {
                toolbar_title.Text = "RMS";
            }
            else if (CurrentPage is ChooseTablePage)
            {
                toolbar_title.Text = "Choose Table View";
            }

            else if (CurrentPage is KOTProdTabbedPage)
            {
                toolbarIcon = null;
                toolbar_title.Text = "Items";
            }

            else if (CurrentPage is TableViewPage)
            {
                toolbar_title.Text = "Table View";
            }
            else if(CurrentPage is PreBillPage)
            {
                toolbar_title.Text = "Pre-Bill View";
            }
            else if(CurrentPage is TransferTablePage)
            {
                toolbar_title.Text = "Transfer Table";

            }


            //toolbarIcon.Visibility = visible ? Android.Views.ViewStates.Visible : Android.Views.ViewStates.Gone;
        }


        //protected virtual void SetupPageTransition(FragmentTransaction transaction, bool isPush)
        //{
        //    if (isPush)
        //        transaction.SetTransition((int)FragmentTransit.FragmentOpen);
        //    else
        //        transaction.SetTransition((int)FragmentTransit.FragmentClose);
        //}

    }
}