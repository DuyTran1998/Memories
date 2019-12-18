using System;
using SQLite;

namespace Memories.Model
{
    public class Token
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public String token { set; get; }        
    }
}
