using System;
using Newtonsoft.Json;

namespace Memories.Model
{
    
    public class Post
    {
        [JsonProperty("_id")]
        public String id { set; get; }
        [JsonProperty("status")]
        public String status { set; get; }
        [JsonProperty("user_id")]
        public UserModel user { set; get; }
        [JsonProperty("image")]
        public String image { set; get; }
        [JsonProperty("time_created")]
        public String time_created { set; get; }
        [JsonProperty("amount_like")]
        public int amount_like  { set; get; }
        [JsonProperty("liked")]
        public bool isLiked { set; get; }

        public Post(String status, String image)
        {
            this.status = status;
            this.image = image;
        }

        public class UserModel
        {
            public String _id { set; get; }
            public String avatar { set; get; }
            public String username { set; get; }
        }

    }

   
}
