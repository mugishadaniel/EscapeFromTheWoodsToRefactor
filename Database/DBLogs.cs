using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBManager
{
    public class DBLogs
    {
        public DBLogs(int woodID, int monkeyID, string message)
        { 
            WoodID = woodID;
            MonkeyID = monkeyID;
            Message = message;
        }

        public ObjectId RecordId { get; set; }
        public int WoodID { get; set; }
        public int MonkeyID { get; set; }
        public string Message { get; set; }
    }
}
