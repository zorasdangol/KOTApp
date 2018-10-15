using KOTAppClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KOTApp.DataAccessLayer
{
    public class TableTransferAccess
    {

        public static string GenerateTransferAllTableURL(string tableNew,string tableOld)
        {
            return string.Format(Helpers.Constants.MainURL + Helpers.Constants.TransferAllTableURL,tableNew,tableOld);
        }

        public static string GenerateMergeTableURL()
        {
            return string.Format(Helpers.Constants.MainURL + Helpers.Constants.MergeTableURL);
        }

        public static string GenerateSplitTableURL()
        {
            return string.Format(Helpers.Constants.MainURL + Helpers.Constants.SplitTableURL);
        }


        public static async Task<string> GetTransferAllTableAsync(string tableNew, string tableOld)
        {
            try
            {
                String url = GenerateTransferAllTableURL(tableNew,tableOld);

                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url);

                    var json = await response.Content.ReadAsStringAsync();                   

                    return json;
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static async Task<string> GetMergeTableAsync(string Destination, List<string> Mergings)
        {
            try
            {
                MergeTransfer mergeTransfer = new MergeTransfer() { DestinationTableNo = Destination, MergingTables = Mergings };
                String url = GenerateMergeTableURL();

                var JsonObject = JsonConvert.SerializeObject(mergeTransfer);
                string ContentType = "application/json"; // or application/xml


                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsync(url, new StringContent(JsonObject.ToString(), Encoding.UTF8, ContentType));
                    
                    var json = await response.Content.ReadAsStringAsync();

                    return json;
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static async Task<string> GetSplitTableAsync(SplitTransfer SplitTransfer)
        {
            try
            {
                String url = GenerateSplitTableURL();
                
                var JsonObject = JsonConvert.SerializeObject(SplitTransfer);
                string ContentType = "application/json"; // or application/xml

                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsync(url, new StringContent(JsonObject.ToString(), Encoding.UTF8, ContentType));

                    var json = await response.Content.ReadAsStringAsync();

                    return json;
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
