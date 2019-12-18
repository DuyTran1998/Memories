using System;
using Memories.Database;
using Memories.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Memories
{
    public partial class App : Application
    {
        static AccountDatabaseController database;

        public static AccountDatabaseController Database
        {
            get
            {
                if(database == null)
                {
                    database = new AccountDatabaseController();
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();
            MainPage = new Login();
            //var remember = App.Database.CheckExistToken();
            //if (remember)
            //{
            //    MainPage = new NavigationBar();
            //}
            //else
            //{
            //    MainPage = new Login();
            //}

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
