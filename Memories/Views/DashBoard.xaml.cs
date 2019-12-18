using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Memories.Model;
using Memories.Network;
using Xamarin.Forms;

namespace Memories
{
    public partial class DashBoard : ContentPage
    {
        public DashBoard()
        {
            InitializeComponent();
            getAll();

        }
        public async void getAll()
        {
            DataService service = new DataService();
            List<Post> posts = await service.GetPost();
            foreach(Post x in posts)
            {
                x.image = "http://localhost:8000/images/" + x.image;
            }
            DataModelList.ItemsSource = posts;
        }
    }
}