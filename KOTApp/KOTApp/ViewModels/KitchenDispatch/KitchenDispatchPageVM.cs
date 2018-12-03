using KOTApp.DataAccessLayer;
using KOTApp.Interfaces;
using KOTApp.Views.KitchenDispatch;
using KOTAppClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace KOTApp.ViewModels.KitchenDispatch
{
    public class KitchenDispatchPageVM : BaseViewModel
    {
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

        private ObservableCollection<KOTProd> _DisplayList;
        public ObservableCollection<KOTProd> DisplayList
        {
            get { return _DisplayList; }
            set
            {
                if (value == null)
                    return;
                _DisplayList = value;
                OnPropertyChanged("DisplayList");
            }
        }

        private List<TableDetail> _PackedTableList;
        public List<TableDetail> PackedTableList
        {
            get { return _PackedTableList; }
            set
            {
                if (value == null)
                    return;
                _PackedTableList = value;
                OnPropertyChanged("PackedTableList");
            }
        }

        private List<string> _ItemsList;
        public List<string> ItemsList
        {
            get { return _ItemsList; }
            set
            {
                if (value == null)
                    return;
                _ItemsList = value;
                OnPropertyChanged("ItemsList");
            }
        }

        private bool _IsDispatch;
        public bool IsDispatch
        {
            get { return _IsDispatch; }
            set
            {
                _IsDispatch = value;
                OnPropertyChanged("IsDispatch");
            }
        }

        private bool _IsLoading;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }

        private bool _IsTable;
        public bool IsTable
        {
            get { return _IsTable; }
            set
            {
                _IsTable = value;
                if (IsItem == value)
                    IsItem = !value;
                OnPropertyChanged("IsTable");                
            }
        }


        private bool _IsItem;
        public bool IsItem
        {
            get { return _IsItem; }
            set
            {
                _IsItem = value;
                if (IsTable == value)
                    IsTable = !value;
                OnPropertyChanged("IsItem");
            }
        }

        private KOTProd _SelectedOrder;
        public KOTProd SelectedOrder
        {
            get { return _SelectedOrder; }
            set
            {
                if (value == null)
                    return;
                _SelectedOrder = value;
                OnPropertyChanged("SelectedOrder");
            }
        }

        public Command CallCommand { get; set; }
        public Command DispatchCommand { get; set; }
        public Command SplitCommand { get; set; }
        public Command BackCommand { get; set; }
        public Command RefreshCommand { get; set; }
                
        private KOTProd _oldItem  { get; set; }

        private TableDetail _SelectedTable;
        public TableDetail SelectedTable
        {
            get { return _SelectedTable; }
            set
            {
                if (value == null)
                    return;
                _SelectedTable = value;
                ViewTableItems(value);
                OnPropertyChanged("SelectedTable");
            }
        }

        private string _SelectedItem;
        public string SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                if (value == null)
                    return;
                _SelectedItem = value;
                ViewItems(value);
                OnPropertyChanged("SelectedItem");
            }
        }

        private string _LoadingMessage;
        public string LoadingMessage
        {
            get { return _LoadingMessage; }
            set { _LoadingMessage = value; OnPropertyChanged("LoadingMessage"); }
        }

        private string _Remarks;
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                _Remarks = value;
                OnPropertyChanged("Remarks");
            }
        }


        public KitchenDispatchPageVM()
        {
            try
            {
                DisplayList = new ObservableCollection<KOTProd>(FilterOrderList());
                SelectedTable = new TableDetail() { TableNo = "All" };
                PackedTableList = Helpers.Data.TableList.Where(x => x.IsPacked == true).ToList();
                PackedTableList.Insert(0, new TableDetail() { TableNo = "All" });
                PackedTableList = new List<TableDetail>(PackedTableList);                

                ItemsList = new List<string>(DisplayList.Select(x => x.ItemDesc).Distinct().ToList());
                ItemsList.Insert(0, "All");
                IsDispatch = false;
                IsLoading = false;
                IsTable = true;
                Remarks = "Order Dispatched";
                CallCommand = new Command(ExecuteCallCommand);
                DispatchCommand = new Command(ExecuteDispatchCommand);
                SplitCommand = new Command(ExecuteSplitCommand);
                BackCommand = new Command(ExecuteBackCommand);
                RefreshCommand = new Command(ExecuteRefreshCommand);
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
                SelectedOrder = item;
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
        }

        private void UpdateOrder(KOTProd item)
        {
            var index = OrderItemsList.IndexOf(item);
            OrderItemsList.Remove(item);
            OrderItemsList.Insert(index, item);
        }


        private void ExecuteBackCommand(object obj)
        {
            IsDispatch = false;
            IsLoading = false;
        }

        private void ExecuteCallCommand(object obj)
        {            
            DependencyService.Get<ICallMessage>().SendMessage("Dispatch Call", SelectedOrder.TABLENO + ": Item ready for Dispatch. " + Remarks);
        }

        private async void ExecuteDispatchCommand()
        {
            try
            {

                var res = await App.Current.MainPage.DisplayAlert("Confirm", "Are you sure to Dispatch Order Items?", "Yes", "No");
                if (res)
                {
                    LoadingMessage = "Please Wait! Tables Loading...";
                    IsLoading = true;
                    var item = SelectedOrder;
                    if (item != null)
                    {
                        item.KitchenDispatch = 1;
                        item.DispatchTime = DateTime.Today.ToLocalTime();
                        item.DispatchUser = Helpers.Constants.User.UserName;
                        item.Remarks = Remarks;

                        var result = await TableDataAccess.SaveKitchenDispatch(item);
                        if (result == "success")
                        {
                            DependencyService.Get<IMessage>().ShortAlert("Order Items Dispatched successfully");
                            
                            ExecuteRefreshCommand();
                        }
                        else
                        {
                            DependencyService.Get<IMessage>().ShortAlert(result);
                        }
                    }
                    Remarks = "Order Dispatched";
                    IsLoading = false;

                }
            }catch(Exception ex)
            {
                IsLoading = false;
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
        }

        public  async void ExecuteRefreshCommand()
        {
            try
            {                
                SelectedOrder = new KOTProd();
                LoadingMessage = "Please Wait! Tables Loading...";
                IsLoading = true;
                var functionResponse = await TableDataAccess.GetTableAsync();
                if (functionResponse.status == "ok")
                {
                    var list = JsonConvert.DeserializeObject<List<TableDetail>>(functionResponse.result.ToString());
                    Helpers.Data.TableList = JsonConvert.DeserializeObject<List<TableDetail>>(functionResponse.result.ToString());
                }
                else
                {
                    Helpers.Data.TableList = new List<TableDetail>();
                    IsLoading = false;
                    DependencyService.Get<IMessage>().ShortAlert(functionResponse.Message);
                }

                LoadingMessage = "Please Wait! Order Items  Loading...";
                IsLoading = true;
                functionResponse = await TableDataAccess.GetAllKOTProdAsync(Helpers.Constants.User.UserName);
                if (functionResponse.status == "ok")
                {
                    IsLoading = false;
                    DependencyService.Get<IMessage>().ShortAlert("Order Items loaded successfully");
                    Helpers.Data.OrderItemsList = JsonConvert.DeserializeObject<List<KOTAppClassLibrary.Models.KOTProd>>(functionResponse.result.ToString());
                    IsTable = true;


                    SelectedTable = new TableDetail() { TableNo = "All" };
                    PackedTableList = Helpers.Data.TableList.Where(x => x.IsPacked == true).ToList();
                    PackedTableList.Insert(0, new TableDetail() { TableNo = "All" });
                    PackedTableList = new List<TableDetail>(PackedTableList);
                    DisplayList = new ObservableCollection<KOTProd>(FilterOrderList());
                    OrderItemsList = new ObservableCollection<KOTProd>(DisplayList);

                    ItemsList = new List<string>(DisplayList.Select(x => x.ItemDesc).Distinct().ToList());
                    ItemsList.Insert(0, "All");
                    
                }
                else
                {
                    IsLoading = false;
                    DependencyService.Get<IMessage>().ShortAlert("Error loading orders:" + functionResponse.Message);

                }
            }catch(Exception ex)
            {
                IsLoading = false;
                 DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
        }

        private async void ExecuteSplitCommand()
        {
            try
            {
                if (SelectedOrder.Quantity == 1)
                {
                    DependencyService.Get<IMessage>().ShortAlert("No need to split this item");
                    return;
                }

                Helpers.Data.SelectedSplitOrder = SelectedOrder;
                await App.Current.MainPage.Navigation.PushModalAsync(new DispatchSplitPage());
                
            }
            catch (Exception e)
            {
                DependencyService.Get<IMessage>().ShortAlert(e.Message);

            }
        }

        public void ViewTableItems(TableDetail tableDetail)
        {
            SelectedOrder = null;
            _oldItem = null;
            try
            {
                if (tableDetail.TableNo == "All")
                {
                    OrderItemsList = new ObservableCollection<KOTProd>(DisplayList);
                }
                else
                {
                    OrderItemsList = new ObservableCollection<KOTProd>(DisplayList.Where(x => x.TABLENO == tableDetail.TableNo ));
                }
                OrderItemsList.ForEach(x=>x.IsVisible = false);
            }catch(Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
        }

        public void ViewItems(string ItemDesc)
        {
            SelectedOrder = null;
            _oldItem = null;
            try
            {
                if (ItemDesc == "All")
                {
                    OrderItemsList = new ObservableCollection<KOTProd>(DisplayList);
                }
                else
                {
                    OrderItemsList = new ObservableCollection<KOTProd>(DisplayList.Where(x => x.ItemDesc == ItemDesc));
                }
                OrderItemsList.ForEach(x => x.IsVisible = false);
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
        }


        public List<KOTProd> FilterOrderList()
        {
            try
            {
                if (Helpers.Data.OrderItemsList == null)
                    return new List<KOTProd>();
                List<KOTProd> list = Helpers.Data.OrderItemsList.Where( x => x.Quantity > 0).ToList();

                if (list == null)
                    return new List<KOTProd>();

                foreach (var item in Helpers.Data.OrderItemsList)
                {
                    if(item.Quantity <= 0)
                    {
                        var i = list.Find(x=> x.SNO == item.REFSNO && x.TABLENO == item.TABLENO);    
                        if(i != null)
                        {
                            i.Quantity = i.Quantity + item.Quantity;                            
                        }
                    }
                }
                var items = list.Where(x => x.Quantity > 0).ToList();                
                return items;

            }catch(Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
                if (Helpers.Data.OrderItemsList == null)
                    return new List<KOTProd>();
                return Helpers.Data.OrderItemsList;
            }
        }
    }
}
