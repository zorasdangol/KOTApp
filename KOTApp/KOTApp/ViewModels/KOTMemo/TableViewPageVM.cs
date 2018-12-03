using KOTApp.DataAccessLayer;
using KOTApp.Interfaces;
using KOTApp.Views.KOTMemo;
using KOTAppClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace KOTApp.ViewModels.KOTMemo
{
    public class TableViewPageVM : BaseViewModel
    {
        private List<TableDetail> _TableList { get; set; }
        public List<TableDetail> TableList
        {
            get { return _TableList; }
            set
            {
                _TableList = value;
                OnPropertyChanged("TableList");
            }
        }

        private List<TableDetail> _TableListRight { get; set; }
        public List<TableDetail> TableListRight
        {
            get { return _TableListRight; }
            set
            {
                _TableListRight = value;
                OnPropertyChanged("TableListRight");
            }
        }

        private List<TableDetail> _TableListLeft { get; set; }
        public List<TableDetail> TableListLeft
        {
            get { return _TableListLeft; }
            set
            {
                _TableListLeft = value;
                OnPropertyChanged("TableListLeft");
            }
        }

        private bool _IsLoading;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set { _IsLoading = value; OnPropertyChanged("IsLoading"); }
        }

        private bool _IsRemarks;
        public bool IsRemarks
        {
            get { return _IsRemarks; }
            set { _IsRemarks = value; OnPropertyChanged("IsRemarks"); }
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
            set { _Remarks = value; OnPropertyChanged("Remarks"); }
        }


        public Command TableBTCommand { get; set; }
        public Command RefreshCommand { get; set; }
        public Command RemarksOkCommand { get; set; }
        public Command BackCommand { get; set; }

        public string SelectedTableNo { get; private set; }

        public TableViewPageVM()
        {
            IsLoading = false;
            TableList = Helpers.Data.TableList.Where( x => x.IsPacked == true).ToList();
            DivideTableView(TableList);
            LoadingMessage = "";
            Remarks = "";
            TableBTCommand = new Command<string>(ExecuteTableBTCommand);
            RefreshCommand = new Command(ExecuteRefreshCommand);
            RemarksOkCommand = new Command(ExecuteRemarksOkCommand);
            BackCommand = new Command(ExecuteBackCommand);
        }

        private void ExecuteBackCommand(object obj)
        {
            IsRemarks = false;
            Remarks = "";
        }

        private async void ExecuteRemarksOkCommand(object obj)
        {
            try
            {
                LoadingMessage = "Please Wait, Loading Table Data";
                IsLoading = true;
                Helpers.Data.SelectedTable = TableList.Find(x => x.TableNo == SelectedTableNo);
                var functionResponse = await TableDataAccess.CancelOrdersAsync(SelectedTableNo, Helpers.Constants.User.UserName, Remarks);
                if (functionResponse.ToLower() == "success")              
                    DependencyService.Get<IMessage>().ShortAlert("Table order canceled successfully");                
                else
                    DependencyService.Get<IMessage>().ShortAlert(functionResponse);
                IsLoading = false;
                IsRemarks = false;
                Remarks = "";
                ExecuteRefreshCommand();
            }catch(Exception ex)
            {
                IsLoading = false;
                IsRemarks = false;
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
        }

        public void DivideTableView(List<TableDetail> TableList)
        {
            try
            {
                TableListLeft = new List<TableDetail>();
                TableListRight = new List<TableDetail>();
                var flag = 0;
                foreach (var table in TableList)
                {
                    if (flag == 0)
                    {
                        TableListLeft.Add(table);
                        flag = 1;
                    }
                    else if (flag == 1)
                    {
                        TableListRight.Add(table);
                        flag = 0;
                    }
                }
            }
            catch (Exception e)
            {
                DependencyService.Get<IMessage>().ShortAlert(e.Message);
            }
        }

        public async void ExecuteTableBTCommand(string TableNo)
        {
            try
            {
                var res = await App.Current.MainPage.DisplayActionSheet("Select any options", "Cancel", "", "Cancel Order", "View PreBill");
                SelectedTableNo = TableNo;
                if (res == "Cancel Order")
                {
                    Remarks = "";
                    IsRemarks = true;                    
                }
                else if(res == "View PreBill")
                {
                    LoadingMessage = "Please Wait, Loading Table Data";
                    IsLoading = true;
                    Helpers.Data.SelectedTable = TableList.Find(x => x.TableNo == TableNo);
                    var functionResponse = await TableDataAccess.GetPreBillAsync(TableNo);
                    if (functionResponse.status == "ok")
                    {
                        var result = JsonConvert.DeserializeObject<BillMain>(functionResponse.result.ToString());
                        Helpers.Data.BillMain = result;
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().ShortAlert(functionResponse.Message);
                    }
                    IsLoading = false;
                    await App.Current.MainPage.Navigation.PushAsync(new PreBillPage());

                }
                    
            }
            catch(Exception e)
            {
                IsLoading = false;
                DependencyService.Get<IMessage>().ShortAlert(e.Message);
                //await App.Current.MainPage.Navigation.PushAsync(new PreBillPage());
            }
        }

        public async void ExecuteRefreshCommand()
        {
            try
            {
                LoadingMessage = "Please Wait, Checking pending KOT";
                IsLoading = true;
                {
                    var functionResponse = await TableDataAccess.GetTableAsync();
                    if (functionResponse.status == "ok")
                    {
                        var list = JsonConvert.DeserializeObject<List<TableDetail>>(functionResponse.result.ToString());
                        Helpers.Data.TableList = JsonConvert.DeserializeObject<List<TableDetail>>(functionResponse.result.ToString());
                    }
                    else
                    {
                        Helpers.Data.TableList = new List<TableDetail>();

                    }
                    IsLoading = false;
                    if (Helpers.Data.TableList.Count > 0)
                    {
                        TableList = Helpers.Data.TableList.Where(x => x.IsPacked == true).ToList();
                        DivideTableView(TableList);
                    }
                    else
                    {
                        TableList = new List<TableDetail>();
                        DependencyService.Get<IMessage>().ShortAlert("No Tables available.");
                    }
                }
            }
            catch(Exception e)
            {
                DependencyService.Get<IMessage>().ShortAlert(e.Message);
            }
        }

    }
}
