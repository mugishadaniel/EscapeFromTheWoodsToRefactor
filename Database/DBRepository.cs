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
        private readonly string ConnectionString;

        public DBRepository(string connectionString)
        {
            ConnectionString = connectionString;
            DbClient = new MongoClient(ConnectionString);
            Database = DbClient.GetDatabase("EscapeFromTheWoods");
        }

        public async Task InsertWoodRecordsAsync(DBWoodRecords data)
        {
            var collection = Database.GetCollection<DBWoodRecords>("WoodRecords");
            await collection.InsertOneAsync(data);
        }

        public async Task InsertMonkeyRecordsAsync(DBMonkeyRecords data)
        {
            var collection = Database.GetCollection<DBMonkeyRecords>("MonkeyRecords");
            await collection.InsertOneAsync(data);
        }

        public async Task InsertLogsAsync(List<DBLogs> data)
        {
            var collection = Database.GetCollection<DBLogs>("Logs");
            await collection.InsertManyAsync(data);
        }

    }
}
