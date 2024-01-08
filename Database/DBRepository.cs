using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBManager
{
    public class DBRepository
    {
        private IMongoClient DbClient;
        private IMongoDatabase Database;
        private string ConnectionString;

        public DBRepository(string connectionString)
        {
            ConnectionString = connectionString;
            DbClient = new MongoClient(connectionString);
            Database = DbClient.GetDatabase("EscapeFromTheWoods");
        }

        public void InsertWoodRecords(List<DBWoodRecords> data)
        {
            var collection = Database.GetCollection<DBWoodRecords>("WoodRecords");
            collection.InsertMany(data);
        }

        public void InsertMonkeyRecords(List<DBMonkeyRecords> data)
        {
            var collection = Database.GetCollection<DBMonkeyRecords>("MonkeyRecords");
            collection.InsertMany(data);
        }

        public void InsertLogs(List<DBLogs> data)
        {
            var collection = Database.GetCollection<DBLogs>("Logs");
            collection.InsertMany(data);
        }

    }
}
