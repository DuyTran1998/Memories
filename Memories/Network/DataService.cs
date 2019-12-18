using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Memories.Database;
using Memories.Model;
using Newtonsoft.Json;

namespace Memories.Network
{
    public class DataService
    {
        public async Task<bool> Login(string email, string password)
        {
            HttpClient client = new HttpClient();
            Account account = new Account(email, password);
            var jsonObject = JsonConvert.SerializeObject(account);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("http://localhost:8000/auth/login", content);
            var token = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                App.Database.SaveToken(token);
                return true;
            }
            return false;
        }

        //public async Task<bool> Check()
        //{
        //    HttpClient client = new HttpClient();
        //    HttpResponseMessage response = await client.GetAsync("https://localhost:3000/auth/login");
        //    var result = await response.Content.ReadAsStringAsync();
        //    if (response.IsSuccessStatusCode)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        public async Task<bool> Register(String email, String username, String password)
        {
            HttpClient client = new HttpClient();
            Account accRegister = new Account(email, username, password);
            var jsonObject = JsonConvert.SerializeObject(accRegister);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("http://localhost:8000/auth/register", content);
            //var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<List<Post>> GetPost()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:8000/post");
            var result = await response.Content.ReadAsStringAsync();
            List<Post> resultData = null;
            resultData = JsonConvert.DeserializeObject<List<Post>>(result);
            return resultData;
        }

    }
}
