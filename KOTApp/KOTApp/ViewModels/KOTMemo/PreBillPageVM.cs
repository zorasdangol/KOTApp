using KOTAppClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KOTApp.ViewModels.KOTMemo
{
    public class PreBillPageVM:BaseViewModel
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


        private List<MenuItem> _SelectedItemsList;
        public List<MenuItem> SelectedItemsList
        {
            get { return _SelectedItemsList; }
            set
            {
                if (value == null)
                    return;
                _SelectedItemsList = value;
                OnPropertyChanged("SelectedItemsList");
            }
        }


        private TableDetail _SelectedTable;
        public TableDetail SelectedTable
        {
            get { return _SelectedTable; }
            set
            {
                if (value == null)
                    return;
                _SelectedTable = value;
                OnPropertyChanged("SelectedTable");
            }
        }

        private bool _IsLoading;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set { _IsLoading = value; OnPropertyChanged("IsLoading"); }
        }

        private string _LoadingMessage;
        public string LoadingMessage
        {
            get { return _LoadingMessage; }
            set { _LoadingMessage = value; OnPropertyChanged("LoadingMessage"); }
        }

        private decimal _BeverageDiscount;
        public decimal BeverageDiscount
        {
            get { return _BeverageDiscount; }
            set
            {
                _BeverageDiscount = value;
                OnPropertyChanged("BeverageDiscount");
            }
        }

        private decimal _FoodDiscount;
        public decimal FoodDiscount
        {
            get
            {
                return _FoodDiscount;
            }
            set
            {
                _FoodDiscount = value;
                OnPropertyChanged("FoodDiscount");
            }
        }

        private TableDetail _TableDetail;
        public TableDetail TableDetail
        {
            get { return _TableDetail; }
            set
            {
                _TableDetail = value;
                OnPropertyChanged("TableDetail");
            }
        }

        public PreBillPageVM()
        {
            try
            {
                IsLoading = false;
                LoadingMessage = "";
                OrderItemsList = new List<KOTProd>();
                SelectedItemsList = new List<MenuItem>();
                SelectedTable = Helpers.Data.SelectedTable;
               

                if (Helpers.Data.MenuItemsList != null)
                {
                    SelectedItemsList = new List<MenuItem>(Helpers.Data.MenuItemsList.Where(x => x.TYPE == "A").ToList());
                }

                if (Helpers.Data.OrderItemsList != null)
                {
                    //OrderItemsList = Helpers.Data.OrderItemsList.OrderBy(x=>x.MCODE).ToList();

                    OrderItemsList = new List<KOTProd>();
                    foreach (var item in Helpers.Data.OrderItemsList)
                    {
                        var found = OrderItemsList.Find(x => x.MCODE == item.MCODE);
                        if (found == null)
                        {
                            var newItem = new KOTProd(item);
                            newItem.SNO = OrderItemsList.Count + 1;
                            OrderItemsList.Add(newItem);
                        }
                        else
                        {
                            found.Quantity += item.Quantity;
                        }
                    }

                    OrderItemsList.ForEach(x => x.DisMode = (SelectedItemsList.Find(y => y.MCODE == x.MCODE).DisMode));
                    OrderItemsList.ForEach(x => x.RATE = (SelectedItemsList.Find(y => y.MCODE == x.MCODE).RATE_A));

                    foreach(var item in OrderItemsList)
                    {
                        if(item.DisMode == "DISCOUNTABLE")
                        {
                            if(item.ISBOT == 0)
                            {
                                item.AMOUNT = (item.RATE * item.Quantity);
                                item.DISCOUNT = ((double)(FoodDiscount / 100) * item.AMOUNT);
                                item.NAMNT = item.AMOUNT - item.DISCOUNT;
                            }
                            else
                            {
                                item.AMOUNT = (item.RATE * item.Quantity);
                                item.DISCOUNT = ((double)(BeverageDiscount / 100) * item.AMOUNT);
                                item.NAMNT = item.AMOUNT - item.DISCOUNT;
                            }
                        }
                        else
                        {
                            item.AMOUNT = (item.RATE * item.Quantity);
                            item.NAMNT = item.AMOUNT;
                            item.DISCOUNT = 0;
                        }
                    }
                }      
                    
            }catch
            {

            }
        }

        public void SetSelectedTableValues()
        {
            SelectedTable.GrossAmount = 0;
            SelectedTable.DisAmount = 0;
            SelectedTable.VATAmount = 0;
            SelectedTable.ServiceCharge = 0;
            SelectedTable.NetAmount = 0;

            foreach(var item in OrderItemsList)
            {
                SelectedTable.GrossAmount += item.AMOUNT;
                SelectedTable.DisAmount += item.DISCOUNT;
               
            }

        }
    }
}
