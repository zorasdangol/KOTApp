using KOTApp.DataAccessLayer;
using KOTApp.Views.KOT;
using KOTAppClassLibrary.Models;
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

        public Command TableBTCommand { get; set; }


        public ChooseTablePageVM()
        {
            try
            {
                SetLayout();
                SelectedLayout = "All";
                TableBTCommand = new Command<string>(ExecuteTableBTCommand);
            }
            catch
            {

            }
        }

        public void SetLayout()
        {
            var filter = Helpers.Constants.TableList.GroupBy(x => x.LayoutName).Select(y => y.FirstOrDefault()).ToList();

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
                    TableList = Helpers.Constants.TableList;
                    DivideTableView(TableList);
                }
                else
                {
                    TableList = Helpers.Constants.TableList.Where(x => x.LayoutName == value).ToList();
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

                if (Helpers.Constants.PackedTableList.Count > 0)
                {
                    foreach (var table in Helpers.Constants.PackedTableList)
                    {
                        TableList.Find(x => x.TableNo == table.TableNo).IsPacked = true;
                    }
                }

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

        public void ExecuteTableBTCommand(string TableNo)
        {
            App.Current.MainPage = new KOTProdPage();

        }
      
    }
}
