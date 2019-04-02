using Common.Api.Dtos;
using MonkeyCache.SQLite;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TD.Api.Dtos;

namespace FourPlaces
{
    public static class Service
    {
        private static string URL = "https://td-api.julienmialon.com/";
        
        
        public static async Task<bool> RegistrationService(string email,string password,string lastname, string firstname)
        {
            
            try
            {
                HttpClient client = new HttpClient();
                var uri = new Uri(string.Format(URL + "auth/register", string.Empty));
                var json = JsonConvert.SerializeObject(new RegisterRequest(email, password, lastname, firstname));
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    var reponseContent = await response.Content.ReadAsStringAsync();
                    Response<LoginResult> result = JsonConvert.DeserializeObject<Response<LoginResult>>(reponseContent);
                    Barrel.Current.Add(key: "Login", data: result.Data, expireIn: TimeSpan.FromDays(1));
                    return true;

                }


                return false;
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return false;
            }    

        }



    

    public static async Task<bool> LoginService(string email, string password)
    {
             
                try {
                    HttpClient client = new HttpClient();
                    var uri = new Uri(string.Format(URL + "auth/login", string.Empty));
                    var json = JsonConvert.SerializeObject(new LoginRequest(email, password));
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = null;
                    response = await client.PostAsync(uri, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var reponseContent = await response.Content.ReadAsStringAsync();
                        Response<LoginResult> result = JsonConvert.DeserializeObject<Response<LoginResult>>(reponseContent);
                        Barrel.Current.Add(key: "Login", data: result.Data, expireIn: TimeSpan.FromDays(1));
                        return true;

                    }
                    return false;
                } catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            
               
            
    }

    }
}
