using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using KOTApp.Controls;
using KOTApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GenericButton), typeof(GenericButtonRenderer))]
namespace KOTApp.Droid.Renderers
{
    public class GenericButtonRenderer : ButtonRenderer
    {
        public GenericButtonRenderer(Context context) : base(context)
        {
        }

        private Xamarin.Forms.Color StartColor { get; set; }
        private Xamarin.Forms.Color EndColor { get; set; }

        protected override void DispatchDraw(Canvas canvas)
        {

            #region for Vertical Gradient  
            var gradient = new Android.Graphics.LinearGradient(0, 0, 0, Height,      
            #endregion
            //var gradient = new Android.Graphics.LinearGradient(0, 0, Width, Height,
                this.StartColor.ToAndroid(),
                this.EndColor.ToAndroid(),
                Android.Graphics.Shader.TileMode.Clamp);
            var paint = new Android.Graphics.Paint()
            {
                Dither = true,
            };
            paint.SetShader(gradient);
            canvas.DrawPaint(paint);
            base.DispatchDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || Element == null)
            {
                return;
            }
            try
            {
                var btn = e.NewElement as GenericButton;
                this.StartColor = btn.StartColor;
                this.EndColor = btn.EndColor;             
               
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"ERROR: ", ex.Message);
            }
        }
    }
}