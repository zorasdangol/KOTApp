using KOTApp.DataAccessLayer;
using KOTApp.Interfaces;
using KOTAppClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace KOTApp.ViewModels.KitchenDispatch
{
    public class DispatchSplitPageVM : BaseViewModel
    {
        private List<KOTProd> _SplitList;
        public List<KOTProd> SplitList
        {
            get { return _SplitList; }
            set
            {
                if (value == null)
                    return;
                _SplitList = value;
                OnPropertyChanged("SplitList");
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

        private double _FirstQuantity;
        public double FirstQuantity
        {
            get { return _FirstQuantity; }
            set
            {
                _FirstQuantity = value;
                OnPropertyChanged("FirstQuantity");
            }
        }

        private double _SecondQuantity;
        public double SecondQuantity
        {
            get { return _SecondQuantity; }
            set
            {
                _SecondQuantity = value;
                OnPropertyChanged("SecondQuantity");
            }
        }

        private string _FirstRemarks;
        public string FirstRemarks
        {
            get { return _FirstRemarks; }
            set { _FirstRemarks = value; OnPropertyChanged("FirstRemarks"); }
        }

        private string _SecondRemarks;
        public string SecondRemarks
        {
            get { return _SecondRemarks; }
            set { _SecondRemarks = value; OnPropertyChanged("SecondRemarks"); }
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

        public Command SaveCommand { get; set; }
        public Command BackCommand { get; set; }
        public Command CancelCommand { get; set; }

        public DispatchSplitPageVM()
        {
            try
            {
                IsLoading = false;
                SelectedOrder = Helpers.Data.SelectedSplitOrder;
                BackCommand = new Command(ExecuteBackCommand);
                SaveCommand = new Command(ExecuteSaveCommand);

            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
        }

        public async void ExecuteSaveCommand()
        {
            try
            {               
                if (((FirstQuantity + SecondQuantity) != SelectedOrder.Quantity) || FirstQuantity == 0 || SecondQuantity == 0)
                {
                    IsLoading = false;
                    DependencyService.Get<IMessage>().ShortAlert("Quantity Entry Incorrect..");
                    return;
                }

                var result = await App.Current.MainPage.DisplayAlert("Confirm", "Are you sure to save Split?", "Yes", "No");
                if (result)
                {
                    LoadingMessage = "Loading Please Wait!!!";
                    IsLoading = true;

                    SplitList = new List<KOTProd>();
                    SplitList.Add(new KOTProd(SelectedOrder) { Quantity = FirstQuantity, Remarks = FirstRemarks });
                    SplitList.Add(new KOTProd(SelectedOrder) { Quantity = SecondQuantity, Remarks = SecondRemarks });

                    var items = Helpers.Data.OrderItemsList.Where( x => x.TABLENO == SelectedOrder.TABLENO && x.MCODE != SelectedOrder.MCODE).ToList();

                    SplitList.AddRange(items);

                    SplitTransfer SplitTransfer = new SplitTransfer();

                    SplitTransfer = new SplitTransfer()
                    {
                        TableNo = SelectedOrder.TABLENO,
                        transferData = SplitList,
                        TRNUSER = Helpers.Constants.User.UserName
                    };

                    SplitTransfer.transferData.ForEach(x => x.DispatchTime = null);
                    SplitTransfer.transferData.ForEach(x => x.TRNDATE = null);

                    var res = await TableTransferAccess.GetSplitTableAsync(SplitTransfer);
                    if (res.ToLower() == "success")
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Split Successful");
                        
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
                        }
                        else
                        {
                            IsLoading = false;
                            DependencyService.Get<IMessage>().ShortAlert("Error loading orders:" + functionResponse.Message);
                        }
                        await App.Current.MainPage.Navigation.PopModalAsync();
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().ShortAlert(res);
                    }                    
                }
            }
            catch (Exception e)
            {
                IsLoading = false;
                DependencyService.Get<IMessage>().ShortAlert(e.Message);

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
