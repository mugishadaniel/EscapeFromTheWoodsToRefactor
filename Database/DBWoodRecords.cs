using EscapeFromTheWoods.Objects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBManager
{
    public class DBWoodRecords
    {
        public DBWoodRecords(int woodID, List<Tree> trees)
        {
            WoodID = woodID;
            Trees = trees;
        }

        public DBWoodRecords(ObjectId recordId, int woodID, List<Tree> trees)
        {
            RecordId = recordId;
            WoodID = woodID;
            Trees = trees;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public ObjectId RecordId { get; set; }
        public int WoodID { get; set; }     
        public List<Tree> Trees { get; set; }

    }
}
