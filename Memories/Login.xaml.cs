using System;
using Xamarin.Forms;
using Memories.Network;

namespace Memories
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }
        public void LoginEvent(object sender, EventArgs e)
        {
            //DataService service = new DataService();
            //var callback = service.PostLogin(username.Text, password.Text);
            //var check = service.Check().Result;
            bool a = true;
            if (a)
            {
                Application.Current.MainPage = new Navigation();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("ERROR!", "Username or Password is wrong!", "Cancel");
            }
        }
    }
}
