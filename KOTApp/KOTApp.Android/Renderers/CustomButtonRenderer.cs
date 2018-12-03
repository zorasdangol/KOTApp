using System;
using System.Collections.Generic;
using System.ComponentModel;
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

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace KOTApp.Droid.Renderers
{
    public class CustomButtonRenderer : Xamarin.Forms.Platform.Android.ButtonRenderer
    {
        public CustomButtonRenderer(Context context) : base(context)
        {
        }
        
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            var view = e as CustomButton;

            this.Control.LongClick += (s, args) =>
            {
                view.LongPressCommand.Execute(new object());
            };
        }

        //protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{

        //    var view = sender as CustomButton;

        //    this.Control.LongClick += (s, args) =>
        //    {
        //        view.LongPressCommand.Execute(new object());
        //    };

        //    base.OnElementPropertyChanged(sender, e);
        //}
    }
}