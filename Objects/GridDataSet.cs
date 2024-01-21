using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeFromTheWoods.Objects
{
    public class GridDataSet
    {
        public GridDataSet(Map map,double delta)
        {
            if(delta <= 0) { throw new Exception("Delta must be greater than 0"); } 

            this.Map = map;
            this.Delta = delta;
            this.NX = (int)Math.Ceiling((map.xmax - map.xmin) / delta);
            this.NY = (int)Math.Ceiling((map.ymax - map.ymin) / delta);
            GridData = new List<Tree>[NX][];
            for (int i = 0; i < NX; i++)
            {
                GridData[i] = new List<Tree>[NY];
                for (int j = 0; j < NY; j++)
                {
                    GridData[i][j] = new List<Tree>();
                }
            }
        }

        public GridDataSet(Map map, double delta, List<Tree> data) : this(map, delta)
        {
            foreach (Tree t in data)
            {
                AddTree(t);
            }
        }

        public int NX { get; private set; }
        public int NY { get; private set; }
        public double Delta { get;private set; }
        public void AddTree(Tree tree)
        { 
            if (tree.x < Map.xmin || tree.x > Map.xmax || tree.y < Map.ymin || tree.y > Map.ymax) { throw new Exception("Tree is out of bounds"); }
            int x = (int)Math.Floor((tree.x - Map.xmin) / Delta);
            int y = (int)Math.Floor((tree.y - Map.ymin) / Delta);
            GridData[x][y].Add(tree);
        }
        public Map Map { get; private set; }
        public List<Tree>[][] GridData { get; private set; }
    }
}
