using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
//using Plugin.FirebasePushNotification;

namespace KOTApp.Droid
{
    [Application]
    public class MainApplication:Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            ////Set the default notification channel for your app when running Android Oreo
            //if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Base)
            //{
            //    //Change for your default notification channel id here
            //    FirebasePushNotificationManager.n = "FirebasePushNotificationChannel";

            //    //Change for your default notification channel name here
            //    FirebasePushNotificationManager.DefaultNotificationChannelName = "General";
            //}


            //If debug you should reset the token each time.
#if DEBUG
            FirebasePushNotificationManager.Initialize(this, true);
#else
              FirebasePushNotificationManager.Initialize(this,false);
#endif

            ////Handle notification when app is closed here
            //CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            //{


            //};
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
                Helpers.Data.deviceToken = p.Token;
            };
            Helpers.Data.deviceToken =  CrossFirebasePushNotification.Current.Token;


        }
    }
}