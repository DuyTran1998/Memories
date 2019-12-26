using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Memories.Database;
using Memories.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Media.Abstractions;
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
                HttpResponseMessage response = await client.PostAsync(Global.URL_API + "/auth/register", content);
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
            catch (Exception ex)
            {
                //await Application.Current.MainPage.DisplayAlert("ERROR!", "Hmm...you're not connected to the internet", "Cancel");
                return new ResponseMessage(false, ex.Message);

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
                HttpResponseMessage response = await client.PostAsync(Global.URL_API + "/auth/login", content);
                var result = await response.Content.ReadAsStringAsync();
                var raw = JObject.Parse(result);
                bool status = false;
                String token = Convert.ToString(raw["token"]);
                String message = "Username or password wrong!";
                if (response.IsSuccessStatusCode)
                {
                    App.Database.SaveToken(token);
                    status = true;
                }
                ResponseMessage responseMessage = new ResponseMessage(status, message);
                return responseMessage;
            }
            catch (Exception ex)
            {
                //await Application.Current.MainPage.DisplayAlert("ERROR!", "Something was wrong", "Cancel");
                return new ResponseMessage(false, ex.Message);
            }
        }

        public async Task<ResponseMessage> UploadPost(String status, MediaFile _mediaFile)
        {
            try
            {
                var token = App.Database.GetToken();

                if (status == null && _mediaFile == null)
                {
                    throw new Exception("Require status or image");
                }
                string image_name = null;
                if (_mediaFile != null)
                {
                    var content = new MultipartFormDataContent();
                    content.Add(new StreamContent(_mediaFile.GetStream()),
                        "\"image\"",
                        $"\"{_mediaFile.Path}\"");
                    var httpClient = new HttpClient();
                    var uploadServiceBaseAddress = Global.URL_API + "/upload";
                    httpClient.DefaultRequestHeaders.Add("authorization", token);
                    var httpResponseMessage = await httpClient.PostAsync(uploadServiceBaseAddress, content);
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    var raw = JObject.Parse(result);
                    image_name = Convert.ToString(raw["image_name"]);
                }
                Post post = new Post(status, image_name);
                var jsonObject = JsonConvert.SerializeObject(post);
                var contentPost = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("authorization", token);
                HttpResponseMessage response = await client.PostAsync(Global.URL_API + "/post", contentPost);
                var contentResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return new ResponseMessage(true, "Successfully!");
                }
                return new ResponseMessage(false, "Server not response!");
            }
            catch (Exception ex)
            {
                return new ResponseMessage(false, ex.Message);
            }

        }

        public async Task<List<Post>> GetPost()
        {
            HttpClient client = new HttpClient();
            //var contentType = "application/json";
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(Global.URL_API + "/post"),
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

        public async Task<ResponseMessage> UpdateProfile(UserDetail userDetail)
        {
            try
            {
                var token = App.Database.GetToken();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("authorization", token);
                var jsonObject = JsonConvert.SerializeObject(userDetail);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(Global.URL_API + "/auth/update", content);
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
            catch (Exception ex)
            {
                //await Application.Current.MainPage.DisplayAlert("ERROR!", "Hmm...you're not connected to the internet", "Cancel");
                return new ResponseMessage(false, ex.Message);
            }

        }
        public async Task<UserDetail> GetUser()
        {
            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(Global.URL_API + "/auth/profile"),
                Method = HttpMethod.Get

            };
            var token = App.Database.GetToken();
            request.Headers.Add("authorization", token);
            HttpResponseMessage response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            UserDetail userDetail = null;
            userDetail = JsonConvert.DeserializeObject<UserDetail>(result);
            return userDetail;
        }

        public async Task<String> Like(String id_post)
        {
            try
            {
                HttpClient client = new HttpClient();
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(Global.URL_API + "/post/" + id_post),
                    Method = HttpMethod.Put

                };
                var token = App.Database.GetToken();
                request.Headers.Add("authorization", token);
                HttpResponseMessage response = await client.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch(Exception e)
            {
                return e.Message;
            }
            
        }

    }
}
