using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabs
{
    public class SQLiteDataAccess
    {
        //get connection string
        private static string GetConnectionString(string id ="Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        } 


        //manage Items
        public static List<Item> GetItems()
        {
            using (var cnn = new SQLiteConnection(GetConnectionString()))
            {
                var output = cnn.Query<Item>("Select * From items", new DynamicParameters());

                return output.ToList();
            }
        }
        public static void SaveItem(Item item)
        {
            using (var cnn = new SQLiteConnection(GetConnectionString()))
            {
                cnn.Execute("INSERT INTO items(ItemName,ItemPrice,Date,Category) VALUES(@ItemName,@ItemPrice,@Date,@Category)", item);
            }
        }

        //Manage Balance
        public static List<Finances> GetBalance()
        {
            using (var cnn = new SQLiteConnection(GetConnectionString()))
            {
                var output = cnn.Query<Finances>("Select * From finances", new DynamicParameters());

                return output.ToList();
            }
        }

        public static void LoadSalary(Finances finances)
        {
            using (var cnn = new SQLiteConnection(GetConnectionString()))
            {
                cnn.Execute("UPDATE  finances SET Balance=@Balance WHERE Id = 1", new {Balance = finances.Balance });
            }
        }
       
    }
}
