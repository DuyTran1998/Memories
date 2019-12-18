using System;
namespace Memories.Model
{
    public class Account
    {
        public String username { set; get; }
        public String password { set; get; }
        public String email { set; get; }

        public Account(String email, String username, String password)
        {
            this.username = username;
            this.password = password;
            this.email = email;
        }

        public Account(String email, String password)
        {
            this.email = email;
            this.password = password;
        }
    }
}
