using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;
using Android.Content;
using Microsoft.AspNet.SignalR.Client;
using Plugin.Notifications;
using Android.Media;
using System.Security.Policy;

namespace KOTApp.Droid
{
    [Activity(Label = "KOTApp", Icon = "@drawable/rms", Theme = "@style/MainTheme",  ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        public static IHubProxy mhubProxy;
        public static HubConnection hubConnection;

        protected override async void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            string dbName = "KOTApp.sqlite";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            string fullPath = Path.Combine(folderPath, dbName);

            LoadApplication(new App(fullPath));


            //hubConnection = new HubConnection(Helpers.Constants.MainURL);
            hubConnection = new HubConnection("http://"+ Helpers.Constants.IPAddress +"/SignalRWebApi");
            mhubProxy = hubConnection.CreateHubProxy("NewHub");
            try
            {
                await hubConnection.Start();
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, ex.Message, ToastLength.Long).Show();
            }

            mhubProxy.On<string, string>("broadcastMessage", (name, Message) => {
                RunOnUiThread(async () => {
                    await CrossNotifications.Current.Send(
                        new Plugin.Notifications.Notification()
                        {               
                            Title = name,
                            Message = Message,
                            Vibrate = true,
                            Sound = "notificationTone"         
                        });                    
                    Toast.MakeText(Application.Context, name + ":" + Message, ToastLength.Short).Show();
                });
            }); 
            
            //FirebasePushNotificationManager.ProcessIntent(this.Intent);
                     

        }


        
        //protected override void OnNewIntent(Intent intent)
        //{
        //    base.OnNewIntent(intent);
        //    //FirebasePushNotificationManager.ProcessIntent(intent);
        //}

    }
}

