﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Memories.Model;
using Memories.Network;
using Memories.Views.Template;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Memories
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashBoard : ContentPage
    {
        ObservableCollection<MediaFile> files = new ObservableCollection<MediaFile>();
        public DashBoard()
        {
            InitializeComponent();
            initDashBoard();

            //Take Photo When Tapping Image;
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (sender, args) =>
            {
                await CrossMedia.Current.Initialize();
                files.Clear();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                    Directory = "Sample",
                    Name = "test.jpg"
                });

                if (file == null)
                    return;

                await DisplayAlert("File Location", file.Path, "OK");

                files.Add(file);
            };
            takePhoto.GestureRecognizers.Add(tapGestureRecognizer);
        }

        public async void initDashBoard()
        {
            DataService service = new DataService();
            List<Post> posts = await service.GetPost();
            if(posts != null)
            {
                foreach (Post x in posts)
                {
                    x.image = Global.IMAGE_POST + x.image;
                }
            }
            DataModelList.ItemsSource = posts;
        }

        public void Camera(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MyPage();
        }
    }
}