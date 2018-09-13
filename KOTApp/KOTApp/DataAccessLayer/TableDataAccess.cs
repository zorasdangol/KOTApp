using KOTAppClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KOTApp.DataAccessLayer
{
    public class TableDataAccess
    {

        public static string GenerateDayCloseTableURL()
        {
            return string.Format(Helpers.Constants.MainURL + Helpers.Constants.DayCloseTableURL);
        }

        public static string GenerateGetTableURL()
        {
            return string.Format(Helpers.Constants.MainURL + Helpers.Constants.GetTableURL);
        }

        public static string GeneratePackedGetTableURL()
        {
            return string.Format(Helpers.Constants.MainURL + Helpers.Constants.GetPackedTableURL);
        }

        public static string GenerateTableDetailsURL(string TableNo)
        {
            return string.Format(Helpers.Constants.MainURL + Helpers.Constants.GetTableItemsDetailsURL, TableNo);
        }

        public static string GenerateSaveKOTProdListURL()
        {
            return string.Format(Helpers.Constants.MainURL + Helpers.Constants.SaveKOTProdListURL);
        }

        public static async Task<string> CheckPendingKOTAsync()
        {
            try
            {
                
                String url = GenerateDayCloseTableURL();

                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url);

                    var json = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<string>(json);
                   
                    return result;
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static async Task<FunctionResponse> GetTableAsync()
        {
            try
            {               

                String url = GenerateGetTableURL();

                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url);

                    var json = await response.Content.ReadAsStringAsync();
                    
                    var functionResponse = JsonConvert.DeserializeObject<FunctionResponse>(json);
                    
                    return functionResponse;
                }
            }
            catch (Exception e)
            {
                return new FunctionResponse() { status = "error", Message = "" };
            }

        }

        public static async Task<FunctionResponse> GetPackedTableAsync()
        {
            try
            {            
                String url = GeneratePackedGetTableURL();

                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url);

                    var json = await response.Content.ReadAsStringAsync();
                    
                    var functionResponse = JsonConvert.DeserializeObject<FunctionResponse>(json);
                    
                    return functionResponse;
                }
            }
            catch (Exception e)
            {
                return new FunctionResponse() { status = "error", Message = "" };
            }
        }

        public static async Task<FunctionResponse> GetTableDetailsAsync(string TableNo)
        {
            try
            {
                String url = GenerateTableDetailsURL(TableNo);

                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url);

                    var json = await response.Content.ReadAsStringAsync();

                    var functionResponse = JsonConvert.DeserializeObject<FunctionResponse>(json);

                    return functionResponse;
                }
            }
            catch (Exception e)
            {
                return new FunctionResponse() { status = "error", Message = e.Message };
            }
        }

        public static async Task<string> SaveKOTProdListAsync(KOTListTransfer KOTData)
        {
            try
            {
                String url = GenerateSaveKOTProdListURL();

                var JsonObject = JsonConvert.SerializeObject(KOTData);
                string ContentType = "application/json"; // or application/xml

                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsync(url, new StringContent(JsonObject.ToString(), Encoding.UTF8, ContentType));

                    var json = await response.Content.ReadAsStringAsync();

                    //var stringRes = JsonConvert.DeserializeObject<string>(json);

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
