using System;
namespace Memories.Model
{
    public class Account
    {
        public String username { set; get; }
        public String password { set; get; }

        public Account(String username, String password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
