using System;
using System.IO;
using Memories.Data;
using Memories.iOS.Database;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_IOS))]
namespace Memories.iOS.Database
{
    public class SQLite_IOS: ISQLite
    {
        public SQLite_IOS()
        {
        }

        public SQLite.SQLiteConnection GetConnection()
        {
            var sqliteFileName = "Memories.db3";
            string documentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentPath, "..", "Library");
            var path = Path.Combine(libraryPath, sqliteFileName);
            var conn = new SQLite.SQLiteConnection(path);
            return conn;
        }

    }
}
