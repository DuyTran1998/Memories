using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
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
    public partial class DashBoard : INotifyPropertyChanged
    {

        public ObservableCollection<Post> ListViewItems { get; set; }
        public DashBoard()
        {
            InitializeComponent();
            DataModelList.IsRefreshing = true;
            initDashBoard();
            DataModelList.IsRefreshing = false;

            DataModelList.RefreshCommand = new Command(async () => {
                DataModelList.IsRefreshing = true;
                await initDashBoard();
                DataModelList.IsRefreshing = false;
            });
            
            //DataModelList.ItemsSource = postCollection;
            //Take Photo When Tapping Image
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (sender, args) =>
            {
                await CrossMedia.Current.Initialize();
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
            };
            takePhoto.GestureRecognizers.Add(tapGestureRecognizer);

            var tapPost = new TapGestureRecognizer();
            tapPost.Tapped += async (sender, args) =>
            {
                await Navigation.PushAsync(new Posts());
            };
            writePost.GestureRecognizers.Add(tapPost);

            //DataModelList.ItemSelected += async (object sender, SelectedItemChangedEventArgs e) =>
            //{
            //    var MyListView = (ListView)sender;
            //    var item = (Post) e.SelectedItem;
            //    if (item.isLiked) {
            //        item.amount_like -= 1;
            //        item.isLiked = false;
            //    }
            //    else
            //    {
            //        item.amount_like += 1;
            //        item.isLiked = true;
            //    }
            //    //DataModelList ;
            //    //this.BindingContext = item;
            //    //OnPropertyChanged("postCollection");
            //};
        }

        public string formatTime(string time)
        {
            string[] splited = Regex.Split(time, "[T:-]"); 
            return splited[3] +":" +splited[4] + " " + splited[2] + "-" + splited[1] + "-" + splited[0];
        }

        public async Task<List<Post>> initDashBoard()
        {
            DataService service = new DataService();
            List<Post> posts = await service.GetPost();
            ObservableCollection<Post> postCollection = new ObservableCollection<Post>();
            if (posts != null)
            {
                foreach (Post x in posts)
                {
                    x.time_created = formatTime(x.time_created);
                    x.image = Global.IMAGE + "/" + x.image;
                    x.user.avatar = Global.IMAGE + "/" + x.user.avatar;
                    postCollection.Add(x);
                }
            }
            DataModelList.ItemsSource = postCollection;

            return posts;
        }
 
        public void OnItemSelected(object sender, EventArgs e)
        {
            
        }
    }
}