using KOTAppClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace KOTApp.Models
{
    public class ItemDetail:MenuItem
    {
        private string _QUANTITY;
        public string TotalQuantity
        {
            get { return _QUANTITY; }
            set
            {
                _QUANTITY = value;               
            }
        }
        
    }
}
