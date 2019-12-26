using System;

using Xamarin.Forms;

namespace Memories.Views
{
    public class PostItemView : ContentPage
    {
        public PostItemView()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

