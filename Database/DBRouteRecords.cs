using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeFromTheWoods.Database
{
    public class DBRouteRecords
    {
        public DBRouteRecords(int seqNr, int treeID, int x, int y)
        {
            SeqNr = seqNr;
            TreeID = treeID;
            X = x;
            Y = y;
        }
        public int SeqNr { get; set; }
        public int TreeID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
