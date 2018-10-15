using KOTAppClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms.Internals;
using System.Linq;
using Xamarin.Forms;
using KOTApp.Interfaces;
using KOTApp.DataAccessLayer;
using Newtonsoft.Json;

namespace KOTApp.ViewModels.TableTransfer
{
    public class TransferSplitPageVM:BaseViewModel
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

        private bool _IsSplit;
        public bool IsSplit
        {
            get { return _IsSplit; }
            set
            {
                _IsSplit = value;
                OnPropertyChanged("IsSplit");                
            }
        }

        private List<TableDetail> _TableList;
        public List<TableDetail> TableList
        {
            get { return _TableList; }
            set
            {
                _TableList = value;
                OnPropertyChanged("TableList");
            }
        }

        private List<string> _TableStrList;
        public List<string> TableStrList
        {
            get { return _TableStrList; }
            set
            {
                if (value == null)
                    return;
                _TableStrList = value;
                OnPropertyChanged("TableStrList");
            }
        }


        private TableDetail _SelectedTableList;
        public TableDetail SelectedTableList
        {
            get { return _SelectedTableList; }
            set
            {
                _SelectedTableList = value;
                OnPropertyChanged("SelectedTableList");
            }
        }

        private ObservableCollection<KOTProd> _TrnasferItemsList;
        public ObservableCollection<KOTProd> TransferItemsList
        {
            get { return _TrnasferItemsList; }
            set
            {
                if (value == null)
                    return;
                _TrnasferItemsList = value;
                OnPropertyChanged("TransferItemsList");
            }
        }

        public Command TransferCommand { get; set; }
        public Command SaveCommand { get; set; }
        public Command BackCommand { get; set; }
        public Command CancelCommand { get; set; }
        public Command SplitOkCommand { get; set; }
        public string TransferType { get; set; }

        public TransferSplitPageVM()
        {
            try
            {
                IsSplit = false;
                TransferType = "";
                TransferItemsList = new ObservableCollection<KOTProd>();
                TableStrList = new List<string>();
                SelectedTableList = new TableDetail();
                TransferCommand = new Command<KOTProd>(ExecuteTransferCommand);
                CancelCommand = new Command(ExecuteCancelCommand);
                BackCommand = new Command(ExecuteBackCommand);
                SplitOkCommand = new Command(ExecuteSplitOkCommand);
                SaveCommand = new Command(ExecuteSaveCommand);
                OrderItemsList = new ObservableCollection<KOTProd>();
                
                if(Helpers.Data.OrderItemsList != null)
                {
                    foreach (var i in Helpers.Data.OrderItemsList)
                    {
                        var found = OrderItemsList.ToList().Find(x => x.MCODE == i.MCODE);
                        if (found != null)
                        {
                            found.Quantity += i.Quantity;
                        }
                        else
                        {
                            OrderItemsList.Add(i);
                        }
                    }
                    OrderItemsList.ForEach(x => x.TABLENO = Helpers.Data.SelectedTable.TableNo);
                }

                if(Helpers.Data.TableList != null)
                {
                    foreach(var i in Helpers.Data.TableList)
                    {
                        TableStrList.Add(i.TableNo);
                    }
                }                

            }
            catch(Exception ex)
            {

            }
        }

        public async void ExecuteTransferCommand(KOTProd SelectedKOTProd)
        {
            try
            {
                TransferItemsList = new ObservableCollection<KOTProd>();
                var res = await App.Current.MainPage.DisplayActionSheet("Choose any one Option", "Cancel", "", "Table", "Item");
                if (res == "Table")
                {
                    TransferType = "Table";
                    var newItem = new KOTProd(SelectedKOTProd);
                    newItem.Quantity = SelectedKOTProd.Quantity;
                    TransferItemsList.Add(newItem);
                    IsSplit = true;
                }
                else if (res == "Item")
                {
                    TransferType = "Item";
                    var count = SelectedKOTProd.Quantity;
                    for (var i = 0; i < count; i++)
                    {
                        var newItem = new KOTProd(SelectedKOTProd);
                        TransferItemsList.Add(newItem);
                    }
                    TransferItemsList.ForEach(x => x.Quantity = 1);
                    IsSplit = true;
                }
                //TransferItemsList = new List<KOTProd>(TransferItemsList);
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
        }

        public void ExecuteSplitOkCommand()
        {
            try
            {
                var sno = TransferItemsList.FirstOrDefault().SNO;
                var found = OrderItemsList.ToList().Find(x => x.SNO == sno);
                if (found != null)
                    OrderItemsList.Remove(found);

                //var count = OrderItemsList.ToList().MaxBy(x => x.SNO);
                var items = OrderItemsList.Select(i => i.SNO).ToList();
                var count = 0;
                if(items.Count > 0)
                    count = items.Max();

                foreach ( var i in TransferItemsList)
                {
                    count++;
                    i.SNO = count;
                }

                foreach (var i in TransferItemsList)
                {
                    var newItem = new KOTProd(i);
                    newItem.Quantity = i.Quantity;
                    OrderItemsList.Add(newItem);
                }
                IsSplit = false;
            }catch(Exception ex)
            {
                IsSplit = false;
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
        }

        public void ExecuteCancelCommand()
        {
            IsSplit = false;
        }

        public async void ExecuteSaveCommand()
        {
            if(Helpers.Data.SelectedTable == null)
            {
                DependencyService.Get<IMessage>().ShortAlert("Data mismatch Error.");
                return;
            }

            // var transferData = JsonConvert.SerializeObject(OrderItemsList.ToList()).ToString();
            SplitTransfer SplitTransfer = new SplitTransfer()
            {
                TableNo = Helpers.Data.SelectedTable.TableNo,
                transferData = OrderItemsList.ToList(),
                TRNUSER = Helpers.Constants.User.UserName
            };

            SplitTransfer.transferData.ForEach(x => x.DispatchTime = null);
            SplitTransfer.transferData.ForEach(x => x.TRNDATE = null);

            var res = await TableTransferAccess.GetSplitTableAsync(SplitTransfer);
            if(res.ToLower() == "success")
            {
                DependencyService.Get<IMessage>().ShortAlert("Split Successful");
                await App.Current.MainPage.Navigation.PopModalAsync();
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert(res);
            }
        }

        public async void ExecuteBackCommand()
        {
            try
            {
                var res = await App.Current.MainPage.DisplayAlert("Confirm", "Are you sure to return Back?", "Yes", "No");
                if (res)
                    await App.Current.MainPage.Navigation.PopModalAsync();
            }
            catch { }
        }

    }
}
