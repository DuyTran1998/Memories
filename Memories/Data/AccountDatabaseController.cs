using System;
using System.Collections.Generic;
using Memories.Data;
using Memories.Model;
using SQLite;
using Xamarin.Forms;

namespace Memories.Database
{
    public class AccountDatabaseController
    {
        protected static object locker = new object();
        SQLiteConnection database;
        public AccountDatabaseController()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTable<Token>();
        }

        public void SaveToken(String token)
        {
            lock (locker)
            {
                var item = new Token
                {
                    id = 0,
                    token = token
                };
                database.Query<Token>("delete from Token");
                Console.WriteLine("Delete Token Sussessfull");
                database.Insert(item);
                Console.WriteLine(token);
                Console.WriteLine("Save New Token");
            }
        }

        public IEnumerable<Token> GetToken()
        { 
            lock (locker)
            {
                var Token = database.Query<Token>("select * from Token");
                return Token;
            }
        }

        public bool CheckExistToken()
        {
            lock (locker)
            {
                var number = database.Table<Token>().Count();
                Console.WriteLine("Number of record:" + number);
                if(number != 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
