using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Memories.Model;
using Memories.Network;
using Memories.Views;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Memories
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Posts : ContentPage
    {
        ObservableCollection<MediaFile> files = new ObservableCollection<MediaFile>();
        DataService dataService = new DataService();
        MediaFile image;

        
        public Posts()
        {
            InitializeComponent();
            //post.IsEnabled = false;
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
                    Name = "test.jpg",
                    AllowCropping = true,
                    SaveMetaData = false
                });

                if (file == null)
                    return;
                image = file;
                Image.Source = file.Path;
                //await DisplayAlert("File Location", file.Path, "OK");

                files.Add(file);
            };

            takePhoto.GestureRecognizers.Add(tapGestureRecognizer);

            var tappedToPickPhoto = new TapGestureRecognizer();
            tappedToPickPhoto.Tapped += async (sender, args) =>
            {
                await CrossMedia.Current.Initialize();
                files.Clear();
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                    return;
                }
                var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Full,
                    SaveMetaData = true
                });


                if (file == null)
                    return;

                files.Add(file);
                image = file;
                Image.Source = file.Path;
            };
            pickPhoto.GestureRecognizers.Add(tappedToPickPhoto);

            //EditorText.TextChanged += (sender, e) =>
            //{
            //    EditorPlaceholder.IsVisible = true;
            //};
           

            post.Clicked += async (sender, args) =>
            {
                String status = EditorText.Text;
                bool flag = true;
                if(status == null && image == null)
                {
                    flag = false;
                }
                ResponseMessage response = await dataService.UploadPost(status,image);

                if (response.status)
                {
                    Application.Current.MainPage = new NavigationBar();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("ERROR!", response.message, null, "Cancel");
                }
            };
            cancel.Clicked += async (sender, args) =>
            {
                await Navigation.PushModalAsync(new NavigationBar());
            };
        }

        public async void TurnOffPlaceholder(object sender, EventArgs e)
        {
            EditorPlaceholder.IsVisible = EditorPlaceholder.Text.Length <= 0;
        }

    }
}
