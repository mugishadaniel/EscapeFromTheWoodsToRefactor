using EscapeFromTheWoods.Database;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MongoDBManager
{
    public class DBMonkeyRecords
    {
        public DBMonkeyRecords(int monkeyID, string monkeyName, int woodID, List<DBRouteRecords> route)
        {
            MonkeyID = monkeyID;
            MonkeyName = monkeyName;
            WoodID = woodID;
            Route = route;
        }

        public DBMonkeyRecords(ObjectId recordId, int monkeyID, string monkeyName, int woodID, List<DBRouteRecords> route)
        {
            RecordId = recordId;
            MonkeyID = monkeyID;
            MonkeyName = monkeyName;
            WoodID = woodID;
            Route = route;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public ObjectId RecordId { get; set; }
        public int MonkeyID { get; set; }
        public string MonkeyName { get; set; }
        public int WoodID { get; set; }
        public List<DBRouteRecords> Route { get; set; }
    }
}
