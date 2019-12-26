using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoreBluetooth;
using Memories.Model;
using Memories.Network;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Memories.Views.Template
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostViewCell : ViewCell
    {
        private DataService dataService = new DataService();
        private Post PostObject;
        public int amount_like = 0;
        ICommand LikeCommand;


        public PostViewCell()
        {
            InitializeComponent();

        }
        public PostViewCell(Post thePost)
        {
            PostObject = thePost;
            LikeCommand = new Command(Like);
        }

        

        void Like()
        {
            PostObject.amount_like += 1;
            BindingContext = PostObject;
        }

        public void Button_Cliked(object sender, EventArgs e)
        {
            var post = (Post)((Button)sender).CommandParameter;
            if (post.isLiked)
            {
                post.amount_like -= 1;
                post.isLiked = false;
            }
            else
            {
                post.amount_like += 1;
                post.isLiked = true;
            }
            numberofLike.Text = post.amount_like.ToString();
            this.BindingContext = post;
            dataService.Like(post.id);

        }

    }
}
