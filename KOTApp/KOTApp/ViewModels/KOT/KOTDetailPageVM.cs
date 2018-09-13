using KOTAppClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using M = KOTAppClassLibrary.Models;

namespace KOTApp.ViewModels.KOT
{
    public class KOTDetailPageVM:BaseViewModel
    {
        private List<KOTProd> _OrderItemsList;        

        public List<KOTProd> OrderItemsList
        {
            get { return _OrderItemsList; }
            set
            {
                if (value == null)
                    return;
                _OrderItemsList = value;
                OnPropertyChanged("OrderItemsList");
            }
        }
        

        public KOTDetailPageVM()
        {
            
            OrderItemsList = new List<M.MenuItem>();
            RefreshOrderList();
        }

        public void RefreshOrderList()
        {
            //OrderItemsList = (Helpers.Data.OrderItemsList);

            if (Helpers.Data.OrderItemsList != null)
                OrderItemsList = new List<KOTProd>(Helpers.Data.OrderItemsList);
            //OnPropertyChanged("OrderItemsList");
        }
    }
}
