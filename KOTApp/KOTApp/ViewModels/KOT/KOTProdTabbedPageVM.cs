using KOTApp.DataAccessLayer;
using KOTApp.Interfaces;
using KOTApp.Views;
using KOTAppClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using M = KOTAppClassLibrary.Models;

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

        public KOTProd _oldItem { get; set; }

        internal void HideOrShowOrder(KOTProd order)
        {
            order.IsVisible = true;
            //UpdateOrderRemarks(order);
        }

   
        //private void UpdateOrderRemarks(KOTProd order)
        //{
        //    var index = 
        //}

        private ObservableCollection<M.MenuItem> _SelectedItemsList;
        public ObservableCollection<M.MenuItem> SelectedItemsList
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

        private ObservableCollection<KOTProd> _OrderItemsList;
        public ObservableCollection<KOTProd> OrderItemsList
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
                        SelectedItemsList = new ObservableCollection<M.MenuItem>( SubMenuList.Where(x => x.MCODE.ToLower().Contains(value.ToLower())));
                    }
                    else if (IsCode == false)
                    {
                        SelectedItemsList = new ObservableCollection<M.MenuItem>( SubMenuList.Where(x => x.DESCA.ToLower().Contains(value.ToLower())));
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
                //if (value != null && !string.IsNullOrEmpty(value.MCODE))
                //{
                //    IsRemarks = true;
                //}
                _SelectedOrderItem = value;               
                OnPropertyChanged("SelectedOrderItem");
            }
        }

        private User _CancelingUser;
        public User CancelingUser
        {
            get { return _CancelingUser; }
            set
            {
                if (value == null)
                    return;
                _CancelingUser = value;
                OnPropertyChanged("CancelingUser");
            }
        }
        
        public Command OrderCommand { get; set; }
        public Command IncreaseOrderCommand { get; set; }
        public Command DecreaseMenuItemCommand { get; set; }
        public Command DecreaseOrderCommand { get; set; }
        public Command CancelCommand { get; set; }
        public Command SpecialItemCommand { get; set; }
        public Command KOTCommand { get; set; }

        public Command LoginCheckCommand { get; set; }
        public Command BackCommand { get; set; }

        public Command IncCommand { get; set; }
        public Command DecCommand { get; set; }
        public Command RemarksOkCommand { get; set; }

        public Command TappedCommand { get; set; }

        public KOTProdTabbedPageVM()
        {
            try
            {
                IsName = true;
                IsLoading = false;
                IsCancel = false;
                IsPax = false;
                IsRemarks = false;
                SelectedOrderItem = new KOTProd();
                if(Helpers.Data.SelectedTable.IsPacked == false)
                {
                    IsPax = true;
                }
                PAX = Helpers.Data.PAX;
                OrderCommand = new Command<M.MenuItem>(ExecuteOrderCommand);
                CancelCommand = new Command<KOTProd>(ExecuteCancelCommand);
                IncreaseOrderCommand = new Command<M.MenuItem>(ExecuteIncreaseCommand);
                DecreaseMenuItemCommand = new Command<M.MenuItem>(ExecuteDecreaseMenuItemCommand);
                DecreaseOrderCommand = new Command<KOTProd>(ExecuteDecreaseOrderCommand);
                KOTCommand = new Command<string>(ExecuteKOTCommand);
                SpecialItemCommand = new Command(ExecuteSpecialItemCommand);
                BackCommand = new Command(ExecuteBackCommand);
                IncCommand = new Command<KOTProd>(ExecuteIncCommand);
                DecCommand = new Command<KOTProd>(ExecuteDecCommand);
                RemarksOkCommand = new Command(ExecuteRemarksOkCommand);
                LoginCheckCommand = new Command(ExecuteLoginCheckCommand);
                TappedCommand = new Command<object>(ExecuteTappedCommand);

                SelectedTable = new TableDetail();
                OrderItemsList = new ObservableCollection<KOTProd>();
                SelectedItemsList = new ObservableCollection<M.MenuItem>();                
                MenuItemsList = new List<M.MenuItem>();
                MasterMenuList = new List<M.MenuItem>();
                SubMenuList = new List<M.MenuItem>();
                CancelingUser = new User();

                if (Helpers.Data.SelectedTable != null)
                    SelectedTable = Helpers.Data.SelectedTable;
                if (Helpers.Data.OrderItemsList != null)
                {
                    var items = Helpers.Data.OrderItemsList;
                    OrderItemsList = new ObservableCollection<KOTProd>(Helpers.Data.OrderItemsList.OrderBy( x => x.SNO));
                    DecreaseItemCount();

                }
                if (Helpers.Data.MenuItemsList != null)
                {
                    MenuItemsList = Helpers.Data.MenuItemsList;
                    MenuItemsList.ForEach( x=> x.QUANTITY = 0 );
                    MasterMenuList = new List<M.MenuItem>(Helpers.Data.MenuItemsList.Where(x => x.TYPE == "G").ToList());

                    SubMenuList = new List<M.MenuItem>(Helpers.Data.MenuItemsList.Where(x => x.TYPE == "A").ToList());
                    SubMenuList.ForEach(x => x.QUANTITY = 0);
                    SelectedItemsList = new ObservableCollection<M.MenuItem>(Helpers.Data.MenuItemsList.Where(x => x.TYPE == "A").ToList());
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

        private void DecreaseItemCount()
        {
            try
            {
                OrderItemsList.ToList().ForEach(x => x.DecQuantity = 0);
                foreach (var item in OrderItemsList)
                {
                    if (item.Quantity < 0)
                    {
                        var found = OrderItemsList.ToList().Find(x => x.SNO == item.REFSNO);
                        if (found != null)
                        {
                            found.DecQuantity += item.Quantity;
                        }
                    }
                }
            }catch(Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
        }


        public void ExecuteTappedCommand(object item)
        {
            try
            {
                IsRemarks = true;
            }
            catch
            {

            }
        }

        public async void ExecuteLoginCheckCommand()
        {
            try
            {
                LoadingMessage = "Loading!! Please wait a while";
                IsLoading = true;
                if (CancelingUser == null)
                    return;
                if (string.IsNullOrEmpty(CancelingUser.UserName))
                {
                    DependencyService.Get<IMessage>().ShortAlert("UserName is empty");
                }
                else if (string.IsNullOrEmpty(CancelingUser.Password))
                {
                    DependencyService.Get<IMessage>().ShortAlert("Password is empty");
                }
                else
                {
                    var res = await LoginConnection.CheckAccessAsync(CancelingUser);
                    if (res.ToLower() == "success")
                    {
                        double qty = 0;
                        double decreased = 0;
                        double left = 0;
                        
                        if(SelectedOrderItem.Quantity > 0)
                        {
                            var items = OrderItemsList.Where(x=> x.REFSNO == SelectedOrderItem.SNO).ToList();
                            foreach(var i in items)
                            {
                                qty += i.Quantity;
                                if(i.SNO > 0)
                                {
                                    decreased += i.Quantity; 
                                }
                            }                             
                        }

                        var neg = -(SelectedOrderItem.Quantity + qty);                        
                        if (neg < 0)
                        {
                            var items = OrderItemsList.Where(x => x.REFSNO == SelectedOrderItem.SNO).ToList();
                            foreach (var i in items)
                            {
                                if(i.SNO == 0)
                                {
                                    i.Quantity = -(SelectedOrderItem.Quantity - decreased);
                                    i.Remarks = CancelingUser.Remarks;
                                    RefreshOrderItemsList();
                                    CancelingUser = new User();
                                    IsCancel = false;
                                    IsLoading = false;
                                    return;
                                }
                            }

                            var item = SubMenuList.Find(x => x.MCODE == SelectedOrderItem.MCODE);
                            var KOTItem = M.MenuItem.MenuItemsToKOTProd(item);
                            KOTItem.Remarks = CancelingUser.Remarks;
                            KOTItem.Quantity = neg;
                            KOTItem.REFSNO = SelectedOrderItem.SNO;
                            OrderItemsList.Add(KOTItem);   
                            
                        }
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().ShortAlert(res);
                    }
                }
                RefreshOrderItemsList();
                
                CancelingUser = new User();
                IsCancel = false;
                IsLoading = false;
            }catch(Exception ex)
            {
                IsCancel = false;
                IsLoading = false;
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
        }

        public  void RefreshOrderItemsList()
        {
            try
            {
                //OrderItemsList = new ObservableCollection<KOTProd>(OrderItemsList);
                //OrderItemsList.OrderBy(x => x.SNO);
                DecreaseItemCount();

                if (!string.IsNullOrEmpty(SelectedOrderItem.MCODE))
                {
                    M.MenuItem selectedObj = new M.MenuItem() { MCODE = SelectedOrderItem.MCODE };
                    SelectedItemCount(selectedObj);
                }               
                CheckSetQuantity();
            }catch(Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
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
            try
            {               
                if (obj.SNO == 0)
                {    
                    OrderItemsList.Remove(obj);
                    var item = SelectedItemsList.ToList().Find(x => x.MCODE == obj.MCODE);                    
                    if (item != null)
                    {
                        item.SetQuantity = 0;
                    }                    
                    RefreshOrderItemsList();
                }
                else
                {
                    if((obj.Quantity + obj.DecQuantity ) == 0)
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Order already Cancelled");
                        return;
                    }
                    if (obj.Quantity < 0)
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Cannot Cancel negative order");
                        return;
                    }
                    SelectedOrderItem = obj;
                    IsCancel = true;
                }
                CountOrderQuantity();
            }
            catch(Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
        }

        public void ExecuteBackCommand()
        {
            IsCancel = false;
            IsPax = false;
            IsRemarks = false;
        }

        public void ExecuteDecreaseMenuItemCommand(M.MenuItem menuItem)
        {
            if(menuItem.QUANTITY < 1)
            {
                DependencyService.Get<IMessage>().ShortAlert("Cannot decrease");
                return;
            }

            var items = OrderItemsList.ToList().Where(x=> (x.MCODE == menuItem.MCODE && x.Quantity > 0));
            var obj = items.ToList().Find(x=> x.SNO == 0 );
            if (obj == null)
            {
                DependencyService.Get<IMessage>().ShortAlert("Cannot decrease already Ordered Items from here");
                return;
                //decimal count = 0;
                //foreach (var item in items)
                //{
                //    count += (decimal)item.Quantity;
                //}
                //if (count > 1)
                //{
                //    obj = items.ToList().Find(x => (x.Quantity-x.DecQuantity) > 1);
                //}
                //else
                //{
                //    DependencyService.Get<IMessage>().ShortAlert("Cannot decrease");
                //    return;
                //}
                
            }
            ExecuteDecreaseOrderCommand(obj);
        }

        public void ExecuteDecreaseOrderCommand(KOTProd obj)
        {
            try
            {
                //new KOTProd (SNO = 0 )
                if (obj.SNO == 0)
                {
                    if (obj.Quantity > 1)
                    {
                        var item = OrderItemsList.ToList().Find(x => (x.MCODE == obj.MCODE) && (x.SNO == 0));
                        item.Quantity -= 1;
                        OrderItemsList = new ObservableCollection<KOTProd>(OrderItemsList);
                        OrderItemsList.OrderBy(x => x.SNO);

                        M.MenuItem selectedObj = new M.MenuItem() { MCODE = obj.MCODE };
                        SelectedItemCount(selectedObj);
                    }
                    else if(obj.Quantity == 1)
                    {
                        var item = OrderItemsList.ToList().Find(x => (x.MCODE == obj.MCODE) && (x.SNO == 0));
                        OrderItemsList.Remove(item);
                        M.MenuItem selectedObj = new M.MenuItem() { MCODE = obj.MCODE };
                        SelectedItemCount(selectedObj);
                    }
                    else if (obj.Quantity < 1)
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Cannot decrease");
                    }
                }

                //old saved KOTProd ( SNO > 0 )
                else
                {
                    //if quantity is negative return
                    if(obj.Quantity <= 1 || (obj.Quantity + obj.DecQuantity) <= 1)
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Cannot Decrease. Only one Item left.Click Cancel");
                        return;
                    }

                    var items = OrderItemsList.Where(x => x.MCODE == obj.MCODE).ToList();
                    
                    var negativeItems = items.Where(x => x.REFSNO == obj.SNO);
                    double qty = 0;
                    double negQty = 0;
                    foreach(var i in negativeItems)
                    {
                        negQty += i.Quantity;
                    }

                    var match = items.Find(x => x.REFSNO == obj.SNO && x.SNO == 0);
                    if (match != null)
                    {
                        qty = obj.Quantity + negQty;
                        if (qty > 1)
                        {
                            obj.DecQuantity -= 1;
                            match.Quantity -= 1;                                                    
                        }
                        else if (qty == 1)
                        {
                            SelectedOrderItem = obj;
                            IsCancel = true;
                        }
                    }
                    else
                    {
                        obj.DecQuantity -= 1;
                        var item = SubMenuList.Find(x => x.MCODE == obj.MCODE);
                        var KOTItem = M.MenuItem.MenuItemsToKOTProd(item);
                        KOTItem.Quantity = -1;
                        KOTItem.REFSNO = obj.SNO;
                        OrderItemsList.Add(KOTItem);
                        DependencyService.Get<IMessage>().ShortAlert("Item added to Order List");                        
                    }

                    RefreshOrderItemsList();
                    
                }
            }
            catch(Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
        }
        
        //Command Execution for SpecialItem button
        public void ExecuteSpecialItemCommand()
        {
            SelectedItemsList = new ObservableCollection<M.MenuItem>(Helpers.Data.MenuItemsList.Where(x => x.TYPE == "A").ToList());
            CountOrderQuantity();
        }
        
        //function to count the ordered quantity for viewing
        public void CountOrderQuantity()
        {
            try
            {
                SelectedItemsList.ToList().ForEach(x => x.QUANTITY = 0);
                SelectedItemsList.ToList().ForEach(x => x.SetQuantity = 0);
                if (OrderItemsList == null || OrderItemsList.Count == 0)
                    return;
                foreach(var item in OrderItemsList)
                {
                    var selected = SelectedItemsList.ToList().Find(x => x.MCODE == item.MCODE);
                    if(selected != null)
                        selected.QUANTITY += (decimal)item.Quantity;
                }                
            }
            catch (Exception e)
            { }
        }
        

        //Command Execution for New and Save KOT Button
        public async void ExecuteKOTCommand(string val)
        {
            try
            {
                var res = await App.Current.MainPage.DisplayAlert("Confirm", "Are you sure?", "Yes", "No");
                if (res)
                {
                    //New KOT
                    if (val == "1")
                    {
                        await App.Current.MainPage.Navigation.PopAsync();
                    }

                    //Save KOT
                    else if (val == "2")
                    {
                        if (OrderItemsList == null || OrderItemsList.Count == 0)
                        {
                            DependencyService.Get<IMessage>().ShortAlert("No Order Items");
                            return;
                        }

                        var items = OrderItemsList;
                        if (items != null)
                        {
                            KOTListTransfer KOTData = new KOTListTransfer();
                            KOTData.TABLENO = Helpers.Data.SelectedTable.TableNo;
                            KOTData.TRNUSER = Helpers.Constants.User.UserName;
                            KOTData.PAX = Helpers.Data.PAX;

                            KOTData.KOTProdList = OrderItemsList.ToList();

                            LoadingMessage = "Please Wait! Saving KOT Items";
                            IsLoading = true;

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
            }catch(Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
           
        }
    

        //Command Execution for Increasing Quantity
        public void ExecuteIncreaseCommand(M.MenuItem obj)
        {
            try
            {
                var selected = SelectedItemsList.ToList().Find(x => x.MCODE == obj.MCODE);
                selected.SetQuantity += 1;
                var item = OrderItemsList.ToList().Find(x => ((x.MCODE == obj.MCODE) && (x.SNO == 0)));

                var KOTItem = M.MenuItem.MenuItemsToKOTProd(obj);
                if (item == null)
                {                    
                    KOTItem.Quantity = 1;
                    OrderItemsList.Add(KOTItem);
                }
                else
                {
                    item.Quantity += 1;
                    if (item.Quantity > 0)
                        item.REFSNO = 0;
                    else if(item.Quantity == 0)
                    {
                        OrderItemsList.Remove(item);
                    }
                }

                SelectedItemCount(obj);

                //OrderItemsList = new List<KOTProd>(OrderItemsList);
                //OrderItemsList.OrderBy(x => x.SNO);
                DecreaseItemCount();

            }
            catch(Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
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
                    var selected = SelectedItemsList.ToList().Find(x => x.MCODE == obj.MCODE);

                    var item = OrderItemsList.ToList().Find(x => ((x.MCODE == obj.MCODE) && (x.SNO == 0)));
                    var KOTItem = M.MenuItem.MenuItemsToKOTProd(obj);

                    KOTItem.Quantity = (double)obj.SetQuantity;
                    OrderItemsList.Add(KOTItem);
                    DependencyService.Get<IMessage>().ShortAlert("Item added to Order List");
                                        
                    selected.SetQuantity = 0;

                    SelectedItemCount(obj);
                    
                    //OrderItemsList = new ObservableCollection<KOTProd>(OrderItemsList);
                    //OrderItemsList.OrderBy(x => x.SNO);
                    DecreaseItemCount();

                }
            }catch(Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
        }
        
        //Function for Viewing Items of Selected Group 
        private void ViewSelectedMenu(M.MenuItem value)
        {
            try
            {
                SelectedItemsList = new ObservableCollection<M.MenuItem>( MenuItemsList.Where(x => x.PARENT == value.MCODE));
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
                var selected = SelectedItemsList.ToList().Find(x => x.MCODE == obj.MCODE);
                var items = OrderItemsList.Where(x => x.MCODE == selected.MCODE).ToList();
                decimal count = 0;
                foreach (var i in items)
                {
                    count = count + (decimal)i.Quantity;
                }
                selected.QUANTITY = count;

                decimal setCount = 0;
                var newItems = items.Where(x => x.SNO == 0).ToList();
                if(newItems != null)
                {
                    foreach (var i in newItems)
                    {
                        setCount = setCount + (decimal)i.Quantity;
                    }
                    selected.SetQuantity = setCount;
                }
                
            }
            catch(Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
        }

        public void CheckSetQuantity()
        {
            try
            {
                var newitems = OrderItemsList.Where(x => x.SNO == 0).ToList();
                foreach (var i in OrderItemsList)
                {
                    var item = SelectedItemsList.ToList().Find(x => x.MCODE == i.MCODE);
                    if (i.SNO == 0)
                    {
                        if (item != null)
                        {
                            item.SetQuantity = (decimal)i.Quantity;
                        }
                    }
                    else
                    {
                        if (item != null)
                        {
                            item.SetQuantity = 0;
                        }
                    }

                }
            }catch(Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
        }


        public void HideOrShowButton(KOTProd item)
        {
            try
            {
                if (_oldItem == item)
                {
                    //click twice on the same item will hide it
                    item.IsVisible = !item.IsVisible;
                    UpdateOrder(item);
                }
                else
                {
                    if (_oldItem != null)
                    {
                        //hide previous selected item
                        _oldItem.IsVisible = false;
                        UpdateOrder(_oldItem);
                    }
                    //show selected item
                    item.IsVisible = true;
                    UpdateOrder(item);
                }
                _oldItem = item;
                //SelectedOrder = item;
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
        }

        public void UpdateOrder(KOTProd item)
        {
            var index = OrderItemsList.IndexOf(item);
            OrderItemsList.Remove(item);
            OrderItemsList.Insert(index, item);
        }

    }
}
