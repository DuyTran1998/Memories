using System;
using Newtonsoft.Json;

namespace Memories.Model
{
    public class UserDetail
    {
        public String avatar { set; get; }
        public String username { set; get; }
        [JsonProperty("first_name")]
        public String firstName { set; get; }
        [JsonProperty("last_name")]
        public String lastName { set; get; }
        [JsonProperty("birthday")]
        public String dateOfBirth { set; get; }
        public String gender { set; get; }
        public String relationship { set; get; }
        public String phone { set; get; }
        public String address { set; get; }
        public UserDetail(String avatar, String username, String firstName, String lastName,
             String dateofBirth, String gender, String relationship, String phone,
             String address)
        {
            this.avatar = avatar;
            this.username = username;
            this.firstName = firstName;
            this.lastName = lastName;
            this.dateOfBirth = dateofBirth;
            this.gender = gender;
            this.relationship = relationship;
            this.phone = phone;
            this.address = address;
        }
    }
}
