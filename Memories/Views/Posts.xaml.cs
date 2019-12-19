using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public Posts()
        {
            InitializeComponent();
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
                Image.Source = file.Path;
            };
            pickPhoto.GestureRecognizers.Add(tappedToPickPhoto);

            EditorText.TextChanged += (sender, e) =>
            {
                EditorPlaceholder.IsVisible = e.NewTextValue.Length <= 0;
            };
        }
    }
}
