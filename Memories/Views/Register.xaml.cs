using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Memories.Model;
using Memories.Network;
using Xamarin.Forms;

namespace Memories.Views
{
    public partial class Register : ContentPage
    {
        public Register()
        {
            InitializeComponent();
        }

        private void Login(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Login();
        }

        private async void Submit(object sender, EventArgs e)
        {

            DataService service = new DataService();
            ResponseMessage response = await service.Register(email.Text, username.Text, password.Text);
            if (response.status)
            {
                Application.Current.MainPage = new Login();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("ERROR!", response.message,null, "Cancel");
            }
        }
    }
}
