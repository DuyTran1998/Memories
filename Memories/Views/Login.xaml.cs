using System;
using Xamarin.Forms;
using Memories.Network;
using Memories.Views;
using Memories.Model;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Foundation;
using Newtonsoft.Json;
using System.Text;

namespace Memories
{
    public partial class Login : ContentPage
    {
        private String ClientId = "748301458984718";
        private String SECRET = "1122334455";
        DataService service = new DataService();
        public Login()
        {
            InitializeComponent();
        }
        public async void Submit(object sender, EventArgs e)
        {
            ResponseMessage response = await service.Login(email.Text, password.Text);
            if (response.status)
            {
                Application.Current.MainPage = new NavigationBar();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("ERROR!", response.message,null, "Cancel");
            }
        }
        private void Register(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Register();
        }

        private async void LoginWithFacebook_Clicked(object sender, EventArgs e)
        {
            var apiRequest = "https://www.facebook.com/v5.0/dialog/oauth?client_id="
              + ClientId
              + "&display=popup&response_type=token&redirect_uri=https://www.facebook.com/connect/login_success.html";

            var webView = new WebView
            {
                Source = apiRequest,
                HeightRequest = 1
            };
            webView.Navigated += WebViewOnNavigated;
            Content = webView;
            
            //delete cache of webview
            NSHttpCookieStorage CookieStorage = NSHttpCookieStorage.SharedStorage;
            foreach (var cookie in CookieStorage.Cookies)
                CookieStorage.DeleteCookie(cookie);
            //Application.Current.MainPage = new NavigationBar();
        }

        private async void WebViewOnNavigated(object sender, WebNavigatedEventArgs e)
        {
            var access_token = ExtraTokenFromUrl(e.Url);
            var result = "";
            if (access_token != "")
            {
                result = await GetFacebookProfileAsysc(access_token);
            }
            if (result != "")
            {
                await Navigation.PushModalAsync(new NavigationBar());
            }
        }

        private string ExtraTokenFromUrl(string url)
        {
            if (url.Contains("access_token") && url.Contains("&expires_in="))
            {
                var at = url.Replace("https://www.facebook.com/connect/login_success.html#access_token=", "");

                var accessToken = at.Remove(at.IndexOf("&expires_in="));
                return accessToken;
            }
            return String.Empty;
        }

        public async Task<String> GetFacebookProfileAsysc(string accessToken)
        {
            var requestUrl = "https://graph.facebook.com/v2.7/me/"
              + "?fields=name,id,address,birthday,email,first_name,last_name"
              + "&access_token=" + accessToken;
            var httpClient = new HttpClient();
            var userString = await httpClient.GetStringAsync(requestUrl);
            JObject userObject = JObject.Parse(userString);
            var username = userObject.GetValue("name");
            var id = userObject.GetValue("id");
            string path_avt = "http://graph.facebook.com/" + id + "/picture?type=large&width=720&height=720";
            userObject.Add("imagePath", path_avt);
            userObject.Add("password", id + SECRET);
            userObject.Add("username", username);
            HttpClient httpClientRequest = new HttpClient();
            var json = JsonConvert.SerializeObject(userObject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(Global.URL_API + "/auth/login_fb"),
                Method = HttpMethod.Post,
                Content = content
            };
            HttpResponseMessage response = await httpClientRequest.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            var raw = JObject.Parse(result);
            String token = Convert.ToString(raw["token"]);
            App.Database.SaveToken(token);
            return userString;
            //return a;
        }
    }
}
