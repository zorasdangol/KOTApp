using M = KOTAppClassLibrary.Models;
using KOTAppClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using KOTApp.Interfaces;
using KOTApp.DataAccessLayer;
using Newtonsoft.Json;
using KOTApp.Views;

namespace KOTApp.ViewModels.KOT
{
    public class KOTProdTabbedPageVM:BaseViewModel
    {
        private List<M.MenuItem> _MenuItemsList;
        public List<M.MenuItem> MenuItemsList
        {
            get { return _MenuItemsList; }
            set
            {
                if (value == null)
                    return;
                _MenuItemsList = value;
                OnPropertyChanged("MenuItemsList");
            }
        }

        private List<M.MenuItem> _SubMenuList;
        public List<M.MenuItem> SubMenuList
        {
            get { return _SubMenuList; }
            set
            {
                if (value == null)
                    return;
                _SubMenuList = value;

                OnPropertyChanged("SubMenuList");
            }
        }

        private List<M.MenuItem> _SelectedItemsList;
        public List<M.MenuItem> SelectedItemsList
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

        private List<M.MenuItem> _MasterMenuList;
        public List<M.MenuItem> MasterMenuList
        {
            get { return _MasterMenuList; }
            set
            {
                if (value == null)
                    return;
                _MasterMenuList = value;
                OnPropertyChanged("MasterMenuList");
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

        private M.MenuItem _SelectedMasterMenu;
        public M.MenuItem SelectedMasterMenu
        {
            get { return _SelectedMasterMenu; }
            set
            {
                if (value == null)
                    return;
                if(_SelectedMasterMenu != value)
                {
                    _SelectedMasterMenu = value;
                    ViewSelectedMenu(value);
                }
                OnPropertyChanged("SelectedMasterMenu");
            }
        }

        private string _PAX;
        public string PAX
        {
            get { return _PAX; }
            set
            {
                if (value == null)
                    return;
                _PAX = value;
                Helpers.Data.PAX = value;
                OnPropertyChanged("PAX");
            }
        }

        private bool _IsCode;
        public bool IsCode
        {
            get { return _IsCode; }
            set
            {
                if (value == _IsCode)
                    return;
                _IsCode = value;
                SearchWord = "";
                IsName = !value;
                OnPropertyChanged("IsCode");
            }
        }

        private bool _IsName;
        public bool IsName
        {
            get { return _IsName; }
            set
            {
                if (value == _IsName)
                    return;
                _IsName = value;
                IsCode = !value;
                OnPropertyChanged("IsName");
            }
        }

        private string _SearchWord;
        public string SearchWord
        {
            get { return _SearchWord; }
            set
            {
                if (value == null)
                    return;
                _SearchWord = value;
                if (!String.IsNullOrEmpty(value))
                {
                    if (IsCode == true)
                    {
                        SelectedItemsList = SubMenuList.Where(x => x.MCODE.ToLower().Contains(value.ToLower())).ToList();
                    }
                    else if (IsCode == false)
                    {
                        SelectedItemsList = SubMenuList.Where(x => x.DESCA.ToLower().Contains(value.ToLower())).ToList();
                    }
                    CountOrderQuantity();
                }
                OnPropertyChanged("SearchWord");                
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

        private bool _IsCancel;
        public bool IsCancel
        {
            get { return _IsCancel; }
            set { _IsCancel = value; OnPropertyChanged("IsCancel"); }
        }

        private bool _IsPax;
        public bool IsPax
        {
            get { return _IsPax; }
            set { _IsPax = value; OnPropertyChanged("IsPax"); }
        }

        private bool _IsRemarks;
        public bool IsRemarks
        {
            get { return _IsRemarks; }
            set { _IsRemarks = value; OnPropertyChanged("IsRemarks"); }
        }

        private KOTProd _SelectedOrderItem;
        public KOTProd SelectedOrderItem
        {
            get { return _SelectedOrderItem; }
            set
            {
                if (value == null)
                    return;
                if(value != null && !string.IsNullOrEmpty(value.MCODE))
                {
                    IsRemarks = true;
                }
                _SelectedOrderItem = value;               
                OnPropertyChanged("SelectedOrderItem");
            }
        }

        public Command OrderCommand { get; set; }
        public Command IncreaseOrderCommand { get; set; }
        public Command DecreaseOrderCommand { get; set; }
        public Command CancelCommand { get; set; }
        public Command SpecialItemCommand { get; set; }
        public Command KOTCommand { get; set; }
        public Command BackCommand { get; set; }

        public Command IncCommand { get; set; }
        public Command DecCommand { get; set; }
        public Command RemarksOkCommand { get; set; }

        public KOTProdTabbedPageVM()
        {
            try
            {
                IsCode = true;
                IsLoading = false;
                IsCancel = false;
                IsPax = false;
                IsRemarks = false;
                SelectedOrderItem = new KOTProd();
                if(Helpers.Data.SelectedTable.IsPacked == false)
                {
                    IsPax = true;
                }
                PAX = "";
                OrderCommand = new Command<M.MenuItem>(ExecuteOrderCommand);
                CancelCommand = new Command<KOTProd>(ExecuteCancelCommand);
                IncreaseOrderCommand = new Command<M.MenuItem>(ExecuteIncreaseCommand);
                DecreaseOrderCommand = new Command<KOTProd>(ExecuteDecreaseCommand);
                KOTCommand = new Command<string>(ExecuteKOTCommand);
                SpecialItemCommand = new Command(ExecuteSpecialItemCommand);
                BackCommand = new Command(ExecuteBackCommand);
                IncCommand = new Command<KOTProd>(ExecuteIncCommand);
                DecCommand = new Command<KOTProd>(ExecuteDecCommand);
                RemarksOkCommand = new Command(ExecuteRemarksOkCommand);

                SelectedTable = new TableDetail();
                OrderItemsList = new List<KOTProd>();
                SelectedItemsList = new List<M.MenuItem>();                
                MenuItemsList = new List<M.MenuItem>();
                MasterMenuList = new List<M.MenuItem>();
                SubMenuList = new List<M.MenuItem>();

                if (Helpers.Data.SelectedTable != null)
                    SelectedTable = Helpers.Data.SelectedTable;
                if (Helpers.Data.OrderItemsList != null)
                {

                    OrderItemsList = Helpers.Data.OrderItemsList.OrderBy( x => x.SNO).ToList();
                }
                if (Helpers.Data.MenuItemsList != null)
                {
                    MenuItemsList = Helpers.Data.MenuItemsList;
                    MenuItemsList.ForEach( x=> x.QUANTITY = 0 );
                    MasterMenuList = new List<M.MenuItem>(Helpers.Data.MenuItemsList.Where(x => x.TYPE == "G").ToList());

                    SubMenuList = new List<M.MenuItem>(Helpers.Data.MenuItemsList.Where(x => x.TYPE == "A").ToList());
                    SubMenuList.ForEach(x => x.QUANTITY = 0);
                    SelectedItemsList = new List<M.MenuItem>(Helpers.Data.MenuItemsList.Where(x => x.TYPE == "A").ToList());
                    CountOrderQuantity();
                }

                MasterMenuList.ForEach(x => x.ItemCount = 0);
                foreach (var item in SubMenuList)
                {
                    var groupItem = MasterMenuList.Find(x => x.MCODE == item.PARENT);
                    if(groupItem != null)
                    {
                        groupItem.ItemCount += 1;
                    }
                }
            }
            catch (Exception e)
            {
                DependencyService.Get<IMessage>().ShortAlert(e.Message);
            }
        }

        public void ExecuteRemarksOkCommand()
        {
            var found = OrderItemsList.Where(x => x.OrderSNO == SelectedOrderItem.OrderSNO).ToList();
            found.ForEach( x => x.Remarks = SelectedOrderItem.Remarks);
            IsRemarks = false;
        }

        public void ExecuteIncCommand(KOTProd obj)
        {
            obj.OrderSNO += 1;
        }

        public void ExecuteDecCommand(KOTProd obj)
        {
            if(obj.OrderSNO != 0)
            {
                obj.OrderSNO -= 1;
            }
        }

        public void ExecuteCancelCommand(KOTProd obj)
        {
            if(obj.SNO == 0)
            {
                OrderItemsList.Remove(obj);
                OrderItemsList = new List<KOTProd>(OrderItemsList);
                OrderItemsList.OrderBy(x => x.SNO);
                //OnPropertyChanged("OrderItemsList");
                var list = Helpers.Data.OrderItemsList;

                M.MenuItem selectedObj = new M.MenuItem() { MCODE = obj.MCODE};
                SelectedItemCount(selectedObj);
            }
            else
            {
                IsCancel = true;
            }
        }

        public void ExecuteBackCommand()
        {
            IsCancel = false;
            IsPax = false;
            IsRemarks = false;
        }

        public void ExecuteDecreaseCommand(KOTProd obj)
        {
            //new KOTProd (SNO = 0 )
            if (obj.SNO == 0)
            {
                if(obj.Quantity > 1)
                { 
                    var item = OrderItemsList.Find( x => ( x.MCODE == obj.MCODE ) && ( x.SNO == 0 ));
                    item.Quantity -= 1;
                    OrderItemsList = new List<KOTProd>(OrderItemsList);
                    OrderItemsList.OrderBy(x => x.SNO);

                    M.MenuItem selectedObj = new M.MenuItem() { MCODE = obj.MCODE };
                    SelectedItemCount(selectedObj);
                }
            }

            //old saved KOTProd ( SNO > 0 )
            else
            {
                var items = OrderItemsList.Where(x => x.MCODE == obj.MCODE).ToList();
                decimal count = 0;
                foreach (var i in items)
                {
                    count = count + (decimal)i.Quantity;
                }
                
                if (count > 1)
                {
                    var item = SubMenuList.Find(x => x.MCODE == obj.MCODE);
                    var KOTItem = M.MenuItem.MenuItemsToKOTProd(item);
                    //item.SNO = 0;
                    KOTItem.Quantity = -1;
                    //newKOT.KOTTIME = "not set";
                    //OrderItemsList.Add(newKOT);

                    OrderItemsList.Add(KOTItem);
                    DependencyService.Get<IMessage>().ShortAlert("Item added to Order List");

                    OrderItemsList = new List<KOTProd>(OrderItemsList);
                    OrderItemsList.OrderBy(x => x.SNO);

                    M.MenuItem selectedObj = new M.MenuItem() { MCODE = obj.MCODE };
                    SelectedItemCount(selectedObj);
                }
            }
        }

        //Command Execution for SpecialItem button
        public void ExecuteSpecialItemCommand()
        {
            SelectedItemsList = new List<M.MenuItem>(Helpers.Data.MenuItemsList.Where(x => x.TYPE == "A").ToList());
            CountOrderQuantity();
        }


        //function to count the ordered quantity for viewing
        public void CountOrderQuantity()
        {
            try
            {
                SelectedItemsList.ForEach(x => x.QUANTITY = 0);
                SelectedItemsList.ForEach(x => x.SetQuantity = 0);
                if (OrderItemsList == null || OrderItemsList.Count == 0)
                    return;
                foreach(var item in OrderItemsList)
                {
                    var selected = SelectedItemsList.Find(x => x.MCODE == item.MCODE);
                    selected.QUANTITY += (decimal)item.Quantity;
                }                
            }
            catch { }
        }


        //Command Execution for New and Save KOT Button
        public async void ExecuteKOTCommand(string val)
        {
            var res = await App.Current.MainPage.DisplayAlert("Confirm", "Are you sure?", "Yes", "No");
            if(res)
            { 
                //New KOT
                if (val == "1")
                {
                    await App.Current.MainPage.Navigation.PopAsync();
                }

                //Save KOT
                else if (val == "2")
                {
                    //if(string.IsNullOrEmpty(PAX) || (PAX == "0"))
                    //{
                    //    if(SelectedTable.IsPacked == false)
                    //    {
                    //        DependencyService.Get<IMessage>().ShortAlert("Enter Number of PAX");
                    //        return;
                    //    }
                    //}

                    if (OrderItemsList == null || OrderItemsList.Count == 0)
                    {
                        DependencyService.Get<IMessage>().ShortAlert("No Order Items");
                        return;
                    }

                    var items = OrderItemsList;
                    //var Hitems = Helpers.Data.OrderItemsList;
                    //var KOTList = M.MenuItem.MenuItemsToKOTProd(items);
                    if (items != null)
                    {
                        KOTListTransfer KOTData = new KOTListTransfer();
                        KOTData.TABLENO = Helpers.Data.SelectedTable.TableNo;
                        KOTData.TRNUSER = Helpers.Constants.User.UserName;
                        KOTData.PAX = Helpers.Data.PAX;

                        KOTData.KOTProdList = OrderItemsList;

                        LoadingMessage = "Please Wait! Saving KOT Items";
                        IsLoading = true;

                        //var result = await TableDataAccess.SaveKOTProdListAsync(Helpers.Data.SelectedTable.TableNo, datastring, Helpers.Constants.User.UserName , PAX );
                        var result = await TableDataAccess.SaveKOTProdListAsync(KOTData);

                        if (result == "Success")
                        {
                            DependencyService.Get<IMessage>().ShortAlert("Order Saved Successfully");
                            App.Current.MainPage = new NavigationPage(new HomePage());
                        }
                        else
                        {
                            DependencyService.Get<IMessage>().ShortAlert(result);
                        }
                        IsLoading = false;
                    }
                }

            }
           
        }
    

        //Command Execution for Increasing Quantity
        public void ExecuteIncreaseCommand(M.MenuItem obj)
        {
            try
            {
                var selected = SelectedItemsList.Find(x => x.MCODE == obj.MCODE);
                var item = OrderItemsList.Find(x => ((x.MCODE == obj.MCODE) && (x.SNO == 0)));

                var KOTItem = M.MenuItem.MenuItemsToKOTProd(obj);
                if (item == null)
                {                    
                    KOTItem.Quantity = 1;
                    OrderItemsList.Add(KOTItem);
                }
                else
                {
                    item.Quantity += 1;
                }
                //OrderItemsList = new List<KOTProd>(Helpers.Data.OrderItemsList);
                OrderItemsList = new List<KOTProd>(OrderItemsList);
                OrderItemsList.OrderBy(x => x.SNO);
                //OnPropertyChanged("OrderItemsList");

                SelectedItemCount(obj);
            }
            catch
            {

            }
        }


        //Command Execution for Order Clicked in KOTMainMenuPage
        public void ExecuteOrderCommand(M.MenuItem obj)
        {
            try
            {
                if(obj.SetQuantity == 0)
                {
                    DependencyService.Get<IMessage>().ShortAlert("Enter Quantity to Order");
                    return;
                }
                else
                {
                    var selected = SelectedItemsList.Find(x => x.MCODE == obj.MCODE);

                    var item = OrderItemsList.Find(x => ((x.MCODE == obj.MCODE) && (x.SNO == 0)));
                    var KOTItem = M.MenuItem.MenuItemsToKOTProd(obj);

                    KOTItem.Quantity = (double)obj.SetQuantity;
                    OrderItemsList.Add(KOTItem);
                    DependencyService.Get<IMessage>().ShortAlert("Item added to Order List");

                    //if (item == null)
                    //{
                    //    KOTItem.Quantity = (double)obj.SetQuantity;
                    //    OrderItemsList.Add(KOTItem);
                    //    DependencyService.Get<IMessage>().ShortAlert("Item added to Order List");                        
                    //}
                    //else
                    //{
                    //    if ((double)obj.SetQuantity != item.Quantity)
                    //    {
                    //        item.Quantity = (double)obj.SetQuantity;
                    //        DependencyService.Get<IMessage>().ShortAlert("Item added to Order List");
                    //    }
                    //    else
                    //    {
                    //        DependencyService.Get<IMessage>().ShortAlert("Already Added to Order List");
                    //    }
                    //}
                    selected.SetQuantity = 0;

                    SelectedItemCount(obj);

                    //selected.QUANTITY = (decimal)item.Quantity;
                    //OrderItemsList = new List<KOTProd>(Helpers.Data.OrderItemsList);
                    OrderItemsList = new List<KOTProd>(OrderItemsList);
                    OrderItemsList.OrderBy(x => x.SNO);
                    //OnPropertyChanged("OrderItemsList");

                }


            }
            catch { }
        }


        //Function for Viewing Items of Selected Group 
        private void ViewSelectedMenu(M.MenuItem value)
        {
            try
            {
                SelectedItemsList = MenuItemsList.Where(x => x.PARENT == value.MCODE).ToList();
                CountOrderQuantity();
            }
            catch (Exception e)
            {
               
            }
        }


        //Function for viewing the change in KOTMainMenuPage
        public void SelectedItemCount(M.MenuItem obj)
        {
            try
            {
                var selected = SelectedItemsList.Find(x => x.MCODE == obj.MCODE);
                var items = OrderItemsList.Where(x => x.MCODE == selected.MCODE).ToList();
                decimal count = 0;
                foreach (var i in items)
                {
                    count = count + (decimal)i.Quantity;
                }
                selected.QUANTITY = count;
            }
            catch { }
        }
        
    }
}
