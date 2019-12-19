using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Memories.Database;
using Memories.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
//using Memories.Data;

namespace Memories.Network
{
    public class DataService
    {
        public async Task<ResponseMessage> Register(String email, String username, String password)
        {
            try
            {
                HttpClient client = new HttpClient();
                Account accRegister = new Account(email, username, password);
                var jsonObject = JsonConvert.SerializeObject(accRegister);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(Global.URI_API + "/auth/register", content);
                var result = await response.Content.ReadAsStringAsync();
                var raw = JObject.Parse(result);
                bool status = false;
                String message = Convert.ToString(raw["message"]);
                if (response.IsSuccessStatusCode)
                {
                    status = true;
                }
                ResponseMessage responseMessage = new ResponseMessage(status, message);
                return responseMessage;
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("ERROR!", "Something was wrong", "Cancel");
                return new ResponseMessage(false, "The App don't connect the Intenet");
            }

        }

        public async Task<ResponseMessage> Login(string email, string password)
        {
            try
            {
                HttpClient client = new HttpClient();
                Account account = new Account(email, password);
                var jsonObject = JsonConvert.SerializeObject(account);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(Global.URI_API + "/auth/login", content);
                var result = await response.Content.ReadAsStringAsync();
                var raw = JObject.Parse(result);
                bool status = false;
                String message = Convert.ToString(raw["token"]);
                if (response.IsSuccessStatusCode)
                {
                    App.Database.SaveToken(message);
                    status = true;
                }
                ResponseMessage responseMessage = new ResponseMessage(status, message);
                return responseMessage;
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("ERROR!", "Something was wrong", "Cancel");
                return new ResponseMessage(false, "The App don't connect the Intenet");
            }
        }

        //public async Task<bool> Check()
        //{
        //    HttpClient client = new HttpClient();
        //    HttpResponseMessage response = await client.GetAsync("http://xamarin-api-group9.herokuapp.com/auth/login");
        //    var result = await response.Content.ReadAsStringAsync();
        //    if (response.IsSuccessStatusCode)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        public async Task<List<Post>> GetPost()
        {
            HttpClient client = new HttpClient();
            var contentType = "application/json";
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(Global.URI_API + "/post"),
                Method = HttpMethod.Get

            };
            var token = App.Database.GetToken();
            request.Headers.Add("authorization", token);
            HttpResponseMessage response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            List<Post> resultData = null;
            resultData = JsonConvert.DeserializeObject<List<Post>>(result);
            return resultData;
        }

    }
}
