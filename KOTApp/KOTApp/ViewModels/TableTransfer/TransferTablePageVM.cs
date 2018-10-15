using KOTApp.DataAccessLayer;
using KOTApp.Interfaces;
using KOTApp.Views.TableTransfer;
using KOTAppClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace KOTApp.ViewModels.TableTransfer
{
    public class TransferTablePageVM:BaseViewModel
    {
        private List<TableDetail> _TableList { get; set; }
        public List<TableDetail> TableList
        {
            get { return _TableList; }
            set
            {
                if (value == null)
                    return;
                _TableList = value;
                OnPropertyChanged("TableList");
            }
        }

        private List<TableDetail> _AllTableList { get; set; }
        public List<TableDetail> AllTableList
        {
            get { return _AllTableList; }
            set
            {
                if (value == null)
                    return;
                _AllTableList = value;
                OnPropertyChanged("AllTableList");
            }
        }

        private List<TableDetail> _EmptyTableList { get; set; }
        public List<TableDetail> EmptyTableList
        {
            get { return _EmptyTableList; }
            set
            {
                if (value == null)
                    return;
                _EmptyTableList = value;
                OnPropertyChanged("EmptyTableList");
            }
        }

        private List<TableDetail> _TableListRight { get; set; }
        public List<TableDetail> TableListRight
        {
            get { return _TableListRight; }
            set
            {
                if (value == null)
                    return;
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
                if (value == null)
                    return;
                _TableListLeft = value;
                OnPropertyChanged("TableListLeft");
            }
        }

        private string _SelectedTableNo;
        public string SelectedTableNo
        {
            get { return _SelectedTableNo; }
            set
            {
                if (value == null)
                    return;
                _SelectedTableNo = value;
                OnPropertyChanged("SelectedTableNo");
            }
        }

        private TableDetail _SelectedTransferFrom;
        public TableDetail SelectedTransferFrom
        {
            get { return _SelectedTransferFrom; }
            set
            {
                if (value == null)
                    return;
                _SelectedTransferFrom = value;
                OnPropertyChanged("SelectedTransferFrom");
            }
        }

        private TableDetail _SelectedTransferTo;
        public TableDetail SelectedTransferTo
        {
            get { return _SelectedTransferTo; }
            set
            {
                if (value == null)
                    return;
                _SelectedTransferTo = value;
                OnPropertyChanged("SelectedTransferTo");
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

        private bool _IsTransfer;
        public bool IsTransfer
        {
            get { return _IsTransfer; }
            set
            {
                _IsTransfer = value;
                OnPropertyChanged("IsTransfer");
            }
        }

        private bool _IsMerge;
        public bool IsMerge
        {
            get { return _IsMerge; }
            set
            {
                _IsMerge = value;
                OnPropertyChanged("IsMerge");
            }
        }

        public Command TableBTCommand { get; set; }
        public Command RefreshCommand { get; set; }

        public Command OkCommand { get; set; }
        public Command CancelCommand { get; set; }

        public TransferTablePageVM()
        {
            IsLoading = false;
            IsTransfer = false;
            IsMerge = false;
            TableList = Helpers.Data.TableList.Where(x => x.IsPacked == true).ToList();
            EmptyTableList = Helpers.Data.TableList.Where( x=> x.IsPacked == false).ToList();
            AllTableList = new List<TableDetail>(Helpers.Data.TableList);

            DivideTableView(TableList);
            LoadingMessage = "";
            TableBTCommand = new Command<string>(ExecuteTableBTCommand);
            RefreshCommand = new Command(ExecuteRefreshCommand);
            OkCommand = new Command<string>(ExecuteOkCommand);
            CancelCommand = new Command(ExecuteBackCommand);

            SelectedTransferFrom = new TableDetail();
            SelectedTransferTo = new TableDetail();
        }

        public void ExecuteBackCommand()
        {
            IsTransfer = false;
            IsMerge = false;
        }

        public async void ExecuteOkCommand(string option)
        {
            try
            {
                LoadingMessage = "Loading Please Wait!!!";
                IsLoading = true;
                if (string.IsNullOrEmpty(SelectedTransferTo.TableNo))
                {
                    DependencyService.Get<IMessage>().ShortAlert("Select correct table to Transfer");
                }
                else
                {
                    if (option == "Transfer")
                    {
                        var res = await TableTransferAccess.GetTransferAllTableAsync(SelectedTransferTo.TableNo, SelectedTransferFrom.TableNo);
                        if (res.ToLower() == "success")
                        {
                            DependencyService.Get<IMessage>().ShortAlert("Table Transfered successfully");
                            IsTransfer = false;
                            ExecuteRefreshCommand();
                        }
                        else if (res.ToLower() == "no")
                        {
                            DependencyService.Get<IMessage>().ShortAlert("Selected Table not empty to be transfered.");
                        }
                        else if (string.IsNullOrEmpty(res))
                        {
                            DependencyService.Get<IMessage>().ShortAlert("Cannot connect to Server");
                        }
                        else
                        {
                            DependencyService.Get<IMessage>().ShortAlert(res);
                        }
                    }
                    
                }
                IsLoading = false;
            }
            catch (Exception e)
            {
                IsLoading = false;
                IsTransfer = false;
                IsMerge = false;
                DependencyService.Get<IMessage>().ShortAlert(e.Message);
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

            }
        }

        public async void ExecuteTableBTCommand(string TableNo)
        {
            try
            {
                var res = await App.Current.MainPage.DisplayActionSheet("Select any options","Cancel","","Merge","Split","Transfer");
                SelectedTransferFrom.TableNo = TableNo;
                if(res == "Transfer")
                {
                    SelectedTransferTo = new TableDetail();
                    IsTransfer = true;
                }
                else if(res == "Split")
                {
                    LoadingMessage = "Loading.. Please wait!!";
                    IsLoading = true;

                    Helpers.Data.SelectedTable = TableList.Find(x => x.TableNo == TableNo);
                    var functionResponse = await TableDataAccess.GetTableDetailsAsync(TableNo);
                    IsLoading = false;
                    if (functionResponse.status == "ok")
                    {
                        var result = JsonConvert.DeserializeObject<List<KOTProd>>(functionResponse.result.ToString());
                        Helpers.Data.OrderItemsList = result;
                        await App.Current.MainPage.Navigation.PushModalAsync(new TransferSplitPage());
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().ShortAlert(functionResponse.Message);
                    }
                }
                else if (res == "Merge")
                {
                    Helpers.Data.SelectedTable = SelectedTransferFrom;
                    await App.Current.MainPage.Navigation.PushModalAsync(new MergeTablePage());
                }
            }
            catch(Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
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
                        DependencyService.Get<IMessage>().ShortAlert("No Tables available.");
                    }
                }
            }
            catch (Exception e)
            {
                DependencyService.Get<IMessage>().ShortAlert(e.Message);
            }
        }

    
    }
}
