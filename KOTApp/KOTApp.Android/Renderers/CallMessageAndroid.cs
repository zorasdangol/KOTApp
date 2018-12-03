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
using KOTApp.Droid.Renderers;
using KOTApp.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(CallMessageAndroid))]
namespace KOTApp.Droid.Renderers
{
    public class CallMessageAndroid : ICallMessage
    {
        public async void SendMessage(string title, string Message)
        {
            try
            {
                Toast.MakeText(Application.Context, title + ": " + Message, ToastLength.Short).Show();
                await MainActivity.mhubProxy.Invoke("Send", new object[] {
                    title, Message
                });
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, ex.Message, ToastLength.Short).Show();
                //await MainActivity.hubConnection.Start();
            }
        }
    }
}