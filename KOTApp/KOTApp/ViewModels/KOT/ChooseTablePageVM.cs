using KOTApp.DataAccessLayer;
using KOTApp.Interfaces;
using KOTApp.Views.KOT;
using KOTAppClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace KOTApp.ViewModels.KOT
{
    public class ChooseTablePageVM:BaseViewModel
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


        private List<String> _LayoutList;
        public List<String> LayoutList
        {
            get { return _LayoutList; }
            set
            {
                _LayoutList = value;
                OnPropertyChanged("LayoutList");
            }
        }

        private String _SelectedLayout;
        public String SelectedLayout
        {
            get { return _SelectedLayout; }
            set
            {
                if(value == null)                
                    return;
                if(_SelectedLayout != value)
                {
                    _SelectedLayout = value;
                    ViewSelectedTable(value);
                }               
                OnPropertyChanged("SelectedLayout");  
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


        public ChooseTablePageVM()
        {
            try
            {
                SetLayout();
                SelectedLayout = "All";
                IsLoading = false;
                LoadingMessage = "";
                TableBTCommand = new Command<string>(ExecuteTableBTCommand);
                RefreshCommand = new Command(ExecuteRefreshCommand);
            }
            catch
            {

            }
        }

        public void SetLayout()
        {
            var filter = Helpers.Data.TableList.GroupBy(x => x.LayoutName).Select(y => y.FirstOrDefault()).ToList();

            LayoutList = new List<string>();
            LayoutList.Insert(0, "All");
            foreach (var item in filter)
            {
                LayoutList.Add(item.LayoutName);
            }
        }

        public void ViewSelectedTable(String value)
        {
            try
            {
                TableList = new List<TableDetail>();
                if (value == "All")
                {
                    TableList = Helpers.Data.TableList;
                    DivideTableView(TableList);
                }
                else
                {
                    TableList = Helpers.Data.TableList.Where(x => x.LayoutName == value).ToList();
                    DivideTableView(TableList);
                }
            }
            catch (Exception e)
            {

            }
        }

        public void DivideTableView(List<TableDetail> TableList)
        {
            try
            {              
                //if (Helpers.Data.PackedTableList.Count > 0)
                //{
                //    foreach (var table in Helpers.Data.PackedTableList)
                //    {
                //        TableList.Find(x => x.TableNo == table.TableNo).IsPacked = true;
                //    }
                //}

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
            }catch(Exception e)
            {

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
                await App.Current.MainPage.Navigation.PushAsync(new KOTProdTabbedPage());
            }
            catch
            {
                await App.Current.MainPage.Navigation.PushAsync(new KOTProdTabbedPage());
            }
        }

        public async void ExecuteRefreshCommand()
        {
            LoadingMessage = "Please Wait, Checking pending KOT";
            IsLoading = true;
            // var res = await TableDataAccess.CheckPendingKOTAsync();

            //if (res == "0")
            {
                var functionResponse = await TableDataAccess.GetTableAsync();
                if (functionResponse.status == "ok")
                {
                    var list = JsonConvert.DeserializeObject<List<TableDetail>>(functionResponse.result.ToString());
                    Helpers.Data.TableList = JsonConvert.DeserializeObject<List<TableDetail>>(functionResponse.result.ToString());
                    TableList = Helpers.Data.TableList;
                }
                else
                {
                    Helpers.Data.TableList = new List<TableDetail>();
                    TableList = Helpers.Data.TableList;
                }
                IsLoading = false;
                if (Helpers.Data.TableList.Count > 0)
                {
                    SetLayout();
                    ViewSelectedTable("All");
                    SelectedLayout = "All";
                }
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert("No Tables available.");
                }
            }
        }
    }
}
