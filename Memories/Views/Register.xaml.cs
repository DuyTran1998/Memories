using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Memories.Network;
using Xamarin.Forms;

namespace Memories.Views
{
    public partial class Register : ContentPage
    {
        Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        public Register()
        {
            InitializeComponent();
        }
        private void Sign_In(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Login();
        }
        private async void Submit(object sender, EventArgs e)
        {

            DataService service = new DataService();
            var response = await service.Register(email.Text, username.Text, password.Text);
            if (response)
            {
                Application.Current.MainPage = new NavigationBar();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("ERROR!", "Register is fail!!!", "Cancel");
            }
        }

        public bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            return EmailRegex.IsMatch(email);
        }
    }
}
