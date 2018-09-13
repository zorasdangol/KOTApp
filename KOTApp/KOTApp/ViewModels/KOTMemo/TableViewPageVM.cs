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

        private string _LoadingMessage;
        public string LoadingMessage
        {
            get { return _LoadingMessage; }
            set { _LoadingMessage = value; OnPropertyChanged("LoadingMessage"); }
        }


        public Command TableBTCommand { get; set; }
        public Command RefreshCommand { get; set; }

        public TableViewPageVM()
        {
            IsLoading = false;
            TableList = Helpers.Data.TableList.Where( x => x.IsPacked == true).ToList();
            DivideTableView(TableList);
            LoadingMessage = "";
            TableBTCommand = new Command<string>(ExecuteTableBTCommand);
            RefreshCommand = new Command(ExecuteRefreshCommand);
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
                LoadingMessage = "Please Wait, Loading Table Data";
                IsLoading = true;
                Helpers.Data.SelectedTable = TableList.Find(x => x.TableNo == TableNo);
                var functionResponse = await TableDataAccess.GetTableDetailsAsync(TableNo);
                if (functionResponse.status == "ok")
                {
                    var result = JsonConvert.DeserializeObject<List<KOTProd>>(functionResponse.result.ToString());
                    Helpers.Data.OrderItemsList = result;
                }
                IsLoading = false;
                await App.Current.MainPage.Navigation.PushAsync(new PreBillPage());
            }
            catch
            {
                await App.Current.MainPage.Navigation.PushAsync(new PreBillPage());
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
            catch(Exception e)
            {
                DependencyService.Get<IMessage>().ShortAlert(e.Message);
            }
        }

    }
}
