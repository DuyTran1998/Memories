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
                DeleteToken();
                database.Insert(item);
                Console.WriteLine(token);
                Console.WriteLine("Save New Token");
            }
        }
        public void DeleteToken()
        {
            lock (locker)
             {
                    database.Query<Token>("delete from Token");
                    Console.WriteLine("Delete Token Sussessfull");
             }   
        }
        public String GetToken()
        { 
            lock (locker)
            {
                var token = database.Query<Token>("select * from Token limit 1");
                return token[0].token;
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
