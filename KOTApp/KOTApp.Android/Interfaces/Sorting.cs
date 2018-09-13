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
using Xamarin.Forms;

namespace KOTApp.Droid.Interfaces
{
    public static class Sorting
    {
        public static readonly BindableProperty IsSortableProperty =
            BindableProperty.CreateAttached(
                            "IsSortabble", typeof(bool),
                            typeof(ListViewSortableEffect), false,
                            propertyChanged: OnIsSortabbleChanged);

        public static bool GetIsSortable(BindableObject view)
        {
            return (bool)view.GetValue(IsSortableProperty);
        }

        public static void SetIsSortable(BindableObject view, bool value)
        {
            view.SetValue(IsSortableProperty, value);
        }

        static void OnIsSortabbleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as Xamarin.Forms.ListView;
            if (view == null)
            {
                return;
            }

            if (!view.Effects.Any(item => item is ListViewSortableEffect))
            {
                view.Effects.Add(new ListViewSortableEffect());
            }
        }

        class ListViewSortableEffect : RoutingEffect
        {
            public ListViewSortableEffect() : base("CubiSoft.ListViewSortableEffect")
            {

            }
        }
    }
}