using KOTApp.DataAccessLayer;
using KOTAppClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public ChooseTablePageVM()
        {
            try
            {
                CheckPackedTable();
                LayoutList = new List<string>();
                var filter = TableList.GroupBy(x => x.LayoutName).Select(y => y.FirstOrDefault()).ToList();
                LayoutList.Insert(0, "All");

                foreach (var item in filter)
                {
                    LayoutList.Add(item.LayoutName);
                }
            }
            catch
            {

            }
        }

        public void CheckPackedTable()
        {
            try
            {
                TableList = Helpers.Constants.TableList;
                if (Helpers.Constants.PackedTableList.Count > 0)
                {
                    foreach (var table in Helpers.Constants.PackedTableList)
                    {
                        TableList.Find(x => x.TableNo == table.TableNo).IsPacked = true;
                    }
                }
                OnPropertyChanged("TableList");
            }catch(Exception e)
            {

            }
        }
    }
}
