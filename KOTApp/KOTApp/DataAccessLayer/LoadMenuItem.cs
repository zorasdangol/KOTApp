using KOTAppClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KOTApp.DataAccessLayer
{
    public class LoadMenuItem
    {
        public static async Task<FunctionResponse> GetMenuItemAsync()
        {
            try
            {
              //  FunctionResponse<List<MenuItem>> functionResponse = new FunctionResponse<List<MenuItem>>();
                
                string url = Helpers.Constants.SetApiURL(Helpers.Constants.MenuItemsURL);
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url);

                    var json = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<FunctionResponse>(json);
                    //if (result == "1")
                    //{
                    //    functionResponse.status = "ok";
                    //}
                    //else if (result == "0")
                    //{
                    //    functionResponse.status = "error";
                    //    functionResponse.Message = "InCorrect Password or UserName";
                    //}
                    //else
                    //{
                    //    functionResponse.status = "error";
                    //    functionResponse.Message = result;
                    //}
                    return result;
                }                
            }catch(Exception e)
            {
                return new FunctionResponse() { status = "error", Message = e.Message };
            }
        }
    }
}
