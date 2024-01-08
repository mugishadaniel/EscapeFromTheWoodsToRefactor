using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
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

        public DBLogs(ObjectId recordId, int woodID, int monkeyID, string message)
        {
            RecordId = recordId;
            WoodID = woodID;
            MonkeyID = monkeyID;
            Message = message;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public ObjectId RecordId { get; set; }
        public int WoodID { get; set; }
        public int MonkeyID { get; set; }
        public string Message { get; set; }
    }
}
