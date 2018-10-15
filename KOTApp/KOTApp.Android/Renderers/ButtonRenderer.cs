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
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(BorderButtonRenderer))]
namespace KOTApp.Droid.Renderers
{
    public class BorderButtonRenderer : ButtonRenderer
    {
        public BorderButtonRenderer(Context context) : base(context)
        {
        }
    }
}