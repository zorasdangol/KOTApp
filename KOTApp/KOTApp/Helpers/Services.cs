using System;
using System.Collections.Generic;
using System.Text;

namespace KOTApp.Helpers
{
    public class Services
    {
        public static bool OnBackCheck(bool IsLoading=false, bool IsCancel=false, bool IsPax=false)
        {
            try
            {
                if(IsLoading == true || IsCancel == true || IsPax == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch { return false; }

        }
    }
}
