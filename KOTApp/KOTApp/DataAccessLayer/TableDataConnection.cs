using KOTAppClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KOTApp.DataAccessLayer
{
    public class TableDataConnection
    {

        public static string GenerateDayCloseTableURL()
        {
            return string.Format(Helpers.Constants.MainURL + Helpers.Constants.DayCloseTableURL);
        }

        public static string GenerateGetTableURL()
        {
            return string.Format(Helpers.Constants.MainURL + Helpers.Constants.GetTableURL);
        }

        public static string GenerateGetTableNoURL()
        {
            return string.Format(Helpers.Constants.MainURL + Helpers.Constants.GetTableNoURL);
        }
        

        public static async Task<string> CheckPendingKOT()
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

        public static async Task<FunctionResponse> GetTable()
        {
            try
            {
                FunctionResponse functionResponse = new FunctionResponse();

                String url = GenerateGetTableURL();

                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url);

                    var json = await response.Content.ReadAsStringAsync();
                    if(json == null)
                    {
                        functionResponse.status = "ok";
                        functionResponse.result = null;
                        return functionResponse;
                    }
                    var result = JsonConvert.DeserializeObject<List<TableDetail>>(json);
                    
                    functionResponse.status = "ok";
                    functionResponse.result = result;

                    return functionResponse;
                }
            }
            catch (Exception e)
            {
                return new FunctionResponse() { status = "error", Message = "" };
            }

        }

        public static async Task<FunctionResponse> GetTableNo()
        {
            try
            {
                FunctionResponse functionResponse = new FunctionResponse();

                String url = GenerateGetTableNoURL();

                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url);

                    var json = await response.Content.ReadAsStringAsync();
                    if (json == null)
                    {
                        functionResponse.status = "ok";
                        functionResponse.result = new List<TableDetail>();
                        return functionResponse;
                    }
                    var result = JsonConvert.DeserializeObject<List<TableDetail>>(json);

                    functionResponse.status = "ok";
                    functionResponse.result = result;

                    return functionResponse;
                }
            }
            catch (Exception e)
            {
                return new FunctionResponse() { status = "error", Message = "" };
            }
        }

    }
}
