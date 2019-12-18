using System;
using Xamarin.Forms;
using Memories.Network;
using Memories.Views;

namespace Memories
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }
        public async void Submit(object sender, EventArgs e)
        {
            DataService service = new DataService();
            var response = await service.Login(email.Text, password.Text);
            if (response)
            {
                Application.Current.MainPage = new NavigationBar();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("ERROR!", "Username or Password is wrong!", "Cancel");
            }
        }
        private void Sign_Up(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Register();
        }
    }
}
