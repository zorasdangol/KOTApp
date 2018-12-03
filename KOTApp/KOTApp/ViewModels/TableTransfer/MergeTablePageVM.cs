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

namespace KOTApp.ViewModels.TableTransfer
{
    public class MergeTablePageVM : BaseViewModel
    {
        private ObservableCollection<TableDetail> _TableList { get; set; }
        public ObservableCollection<TableDetail> TableList
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

        private ObservableCollection<TableDetail> _TableListRight { get; set; }
        public ObservableCollection<TableDetail> TableListRight
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

        private ObservableCollection<TableDetail> _TableListLeft { get; set; }
        public ObservableCollection<TableDetail> TableListLeft
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


        private List<TableDetail> _MergingTableList { get; set; }
        public List<TableDetail> MergingTableList
        {
            get { return _MergingTableList; }
            set
            {
                if (value == null)
                    return;
                _MergingTableList = value;
                OnPropertyChanged("MergingTableList");
            }
        }        

        private TableDetail _DestinationTable { get; set; }
        public TableDetail DestinationTable
        {
            get { return _DestinationTable; }
            set
            {
                if (value == null)
                    return;
                _DestinationTable = value;
                OnPropertyChanged("DestinationTable");
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

        public Command SaveCommand { get; set; }
        public Command BackCommand { get; set; }
        
        public MergeTablePageVM()
        {
            IsLoading = false;

            BackCommand = new Command(ExecuteBackCommand);
            SaveCommand = new Command(ExecuteSaveCommand);
            
            TableList = new ObservableCollection<TableDetail>(Helpers.Data.TableList);
            if(Helpers.Data.SelectedTable != null )
                DestinationTable = TableList.ToList().Find( x => x.TableNo == Helpers.Data.SelectedTable.TableNo) ;

            DivideTableView(TableList);
        }

        public void DivideTableView(ObservableCollection<TableDetail> TableList)
        {
            try
            {
                TableListLeft = new ObservableCollection<TableDetail>();
                TableListRight = new ObservableCollection<TableDetail>();
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
       
        public async void ExecuteSaveCommand()
        {
            try
            {
                LoadingMessage = "Loading Please Wait!!!";
                IsLoading = true;
                var res = await App.Current.MainPage.DisplayAlert("Confirm", "Are you sure to Merge tables?", "Yes", "No");
                if (res)
                {
                    MergingTableList = new List<TableDetail>();
                    var leftSelectedList = TableListLeft.Where( x=> x.IsSelected == true).ToList();
                    var rightSelectedList = TableListRight.Where( x=> x.IsSelected == true).ToList();
                    if (leftSelectedList != null)
                        MergingTableList.AddRange(leftSelectedList);
                    if (rightSelectedList != null)
                        MergingTableList.AddRange(rightSelectedList);

                    if(MergingTableList == null || MergingTableList.Count <= 0)
                    {
                        DependencyService.Get<IMessage>().ShortAlert("No Tables selected to Merge");
                        return;
                    }

                    var list = MergingTableList.Select( x=> x.TableNo).ToList();
                    list.Remove(DestinationTable.TableNo);

                    var result = await TableTransferAccess.GetMergeTableAsync(DestinationTable.TableNo, list);
                    if(result.ToLower() == "success")
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Tables Merged Successfully to " + DestinationTable.TableNo);
                        await App.Current.MainPage.Navigation.PopModalAsync();
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().ShortAlert(result);
                    }
                }
                IsLoading = false;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
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
            catch (Exception ex)
            {

            }
        }
    }
}
