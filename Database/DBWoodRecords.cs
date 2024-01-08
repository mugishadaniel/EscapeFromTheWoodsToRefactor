using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBManager
{
    public class DBWoodRecords
    {
        public DBWoodRecords(int woodID, int treeID, int x, int y)
        {
            WoodID = woodID;
            TreeID = treeID;
            X = x;
            Y = y;
        }

        public ObjectId RecordId { get; set; }
        public int WoodID { get; set; }
        public int TreeID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

    }
}
