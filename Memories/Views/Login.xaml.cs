using System;
using Xamarin.Forms;
using Memories.Network;
using Memories.Views;
using Memories.Model;

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
            ResponseMessage response = await service.Login(email.Text, password.Text);
            if (response.status)
            {
                Application.Current.MainPage = new NavigationBar();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("ERROR!", "Username or Password is wrong!", "Cancel");
            }
        }
        private void Register(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Register();
        }
    }
}
