
using KOTAppClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms.PlatformConfiguration;


namespace KOTApp.DataAccessLayer
{
    public class LoginConnection
    {
        public static string GenerateLoginURL()
        {
            return string.Format(Helpers.Constants.MainURL + Helpers.Constants.LoginURL);
        }

        public static async Task<FunctionResponse> CheckAccessAsync(User User)
        {
            try
            {
                FunctionResponse functionResponse = new FunctionResponse();
               
                User.UniqueID = "12345";

                var JsonObject = JsonConvert.SerializeObject(User);
                string ContentType = "application/json"; // or application/xml
                
                String url = GenerateLoginURL();
               
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsync(url, new StringContent(JsonObject.ToString(), Encoding.UTF8,ContentType));

                    var json = await response.Content.ReadAsStringAsync();
                    
                    var result = JsonConvert.DeserializeObject<string>(json);
                    if (result == "1")
                    {
                        functionResponse.status = "ok";
                    }
                    else if(result == "0")
                    {
                        functionResponse.status = "error";
                        functionResponse.Message = "InCorrect Password or UserName";
                    }
                    else
                    {
                        functionResponse.status = "error";
                        functionResponse.Message = result;
                    }
                    return functionResponse;
                }               
            }catch(Exception e)
            {
                return new FunctionResponse() { status = "error", Message = "Cannot connect to Server"  };
            }

        }

    }
}
