using System;
using Newtonsoft.Json;

namespace Memories.Model
{
    
    public class Post
    {
        [JsonProperty("user_id")]
        public String user_id { set; get; }
        [JsonProperty("status")]
        public String status { set; get; }
        [JsonProperty("image")]
        public String image { set; get; }
        [JsonProperty("time_created")]
        public String time_created { set; get; }
        [JsonProperty("amount_like")]
        public int amount_like  { set; get; }
    }
}
