using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Memories.Model;
using Newtonsoft.Json;

namespace Memories.Network
{
    public class DataService
    {
        public async Task<bool> PostLogin(string username, string password)
        {
            HttpClient client = new HttpClient();
            Account account = new Account(username, password);
            var jsonObject = JsonConvert.SerializeObject(account);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("http://localhost:8080/quizs/login", content);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Check()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:8080/quizs/login");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
