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
using Xamarin.Forms;
using KOTApp.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(CustomButton))]
namespace KOTApp.Droid.Renderers
{
    [ContentProperty("CustomButton")]
    public class CustomButton : Xamarin.Forms.Button, ICustomButton
    {
        public CustomButton()
        {
        }

        public Command LongPressCommand { get; set; }
    }
}