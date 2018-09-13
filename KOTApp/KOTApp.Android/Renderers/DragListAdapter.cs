using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AViews = Android.Views;
using AWidget = Android.Widget;
using Java.Lang;
using System.Collections;
using KOTApp.Droid.Interfaces;

namespace KOTApp.Droid.Renderers
{
    public class DragListAdapter :
    Android.Widget.BaseAdapter,
    Android.Widget.IWrapperListAdapter,
    Android.Views.View.IOnDragListener,
    Android.Widget.AdapterView.IOnItemLongClickListener
    {
        private Android.Widget.IListAdapter _listAdapter;

        private Android.Widget.ListView _listView;

        private Xamarin.Forms.ListView _element;

        public DragListAdapter(Android.Widget.ListView listView, Xamarin.Forms.ListView element)
        {
            _listView = listView;
            // NOTE: careful, the listAdapter might not always be an IWrapperListAdapter
            _listAdapter = ((Android.Widget.IWrapperListAdapter)_listView.Adapter).WrappedAdapter;
            _element = element;
        }

        public bool DragDropEnabled { get; set; } = true;

        #region IWrapperListAdapter Members

        public Android.Widget.IListAdapter WrappedAdapter => _listAdapter;

        public override int Count => WrappedAdapter.Count;

        public override bool HasStableIds => WrappedAdapter.HasStableIds;

        public override bool IsEmpty => WrappedAdapter.IsEmpty;

        public override int ViewTypeCount => WrappedAdapter.ViewTypeCount;

        public override bool AreAllItemsEnabled() => WrappedAdapter.AreAllItemsEnabled();

        public override Java.Lang.Object GetItem(int position)
        {
            return WrappedAdapter.GetItem(position);
        }

        public override long GetItemId(int position)
        {
            return WrappedAdapter.GetItemId(position);
        }

        public override int GetItemViewType(int position)
        {
            return WrappedAdapter.GetItemViewType(position);
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
        {
            var view = WrappedAdapter.GetView(position, convertView, parent);
            view.SetOnDragListener(this);
            return view;
        }

        public override bool IsEnabled(int position)
        {
            return WrappedAdapter.IsEnabled(position);
        }

        public bool OnDrag(AViews.View v, DragEvent e)
        {
            switch (e.Action)
            {
                case DragAction.Started:
                    break;
                case DragAction.Entered:
                    System.Diagnostics.Debug.WriteLine($"DragAction.Entered from {v.GetType()}");
                    break;
                case DragAction.Location:
                    break;
                case DragAction.Exited:
                    System.Diagnostics.Debug.WriteLine($"DragAction.Entered from {v.GetType()}");

                    if (!(v is AWidget.ListView))
                    {
                        var positionEntered = GetListPositionForView(v);

                        System.Diagnostics.Debug.WriteLine($"DragAction.Exited index {positionEntered}");
                    }
                    break;
                case DragAction.Drop:
                    System.Diagnostics.Debug.WriteLine($"DragAction.Drop from {v.GetType()}");

                    var mobileItem = (DragItem)e.LocalState;

                    if (!(v is AWidget.ListView) && v != mobileItem.View)
                    {
                        mobileItem.Index = GetListPositionForView(v);

                        mobileItem.View.Visibility = ViewStates.Visible;

                        if (_element.ItemsSource is IOrderable orderable)
                        {
                            orderable.ChangeOrdinal(mobileItem.OriginalIndex, mobileItem.Index);
                        }
                    }
                    break;
                case DragAction.Ended:
                    System.Diagnostics.Debug.WriteLine($"DragAction.Drop from {v.GetType()}");
                    break;
            }

            return true;
        }


        public bool OnItemLongClick(Android.Widget.AdapterView parent, Android.Views.View view, int position, long id)
        {
            var selectedItem = ((IList)_element.ItemsSource)[(int)id];

            // Creating drag state
            DragItem dragItem = new DragItem(NormalizeListPosition(position), view, selectedItem);

            // Creating a blank clip data object (we won't depend on this) 
            var data = ClipData.NewPlainText(string.Empty, string.Empty);

            // Creating the default drag shadow for the item (the translucent version of the view)
            // NOTE: Can create a custom view in order to change the dragged item view
            AViews.View.DragShadowBuilder shadowBuilder = new AViews.View.DragShadowBuilder(view);

            // Setting the original view cell to be invisible
            view.Visibility = ViewStates.Invisible;

            // NOTE: this method is introduced in Android 24, for earlier versions the StartDrag method should be used
            view.StartDragAndDrop(data, shadowBuilder, dragItem, 0);

            return true;
        }

        public override void RegisterDataSetObserver(DataSetObserver observer)
        {
            base.RegisterDataSetObserver(observer);
            WrappedAdapter.RegisterDataSetObserver(observer);
        }

        public override void UnregisterDataSetObserver(DataSetObserver observer)
        {
            base.UnregisterDataSetObserver(observer);
            WrappedAdapter.UnregisterDataSetObserver(observer);
        }

        #endregion


        private int GetListPositionForView(AViews.View view)
        {
            return NormalizeListPosition(_listView.GetPositionForView(view));
        }

        private int NormalizeListPosition(int position)
        {
            // We do not want to count the headers into the item source index
            return position - _listView.HeaderViewsCount;
        }

        //... removed for brevity
    }
}