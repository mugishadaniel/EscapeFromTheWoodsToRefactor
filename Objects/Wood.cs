using EscapeFromTheWoods.Database;
using MongoDBManager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EscapeFromTheWoods.Objects
{
    public class Wood
    {
        private const int drawingFactor = 8;
        private string path;
        private DBRepository db;
        private Random r = new Random(1);
        public int woodID { get; set; }
        public List<Tree> trees { get; set; }
        public List<Monkey> monkeys { get; private set; }
        public List<DBMonkeyRecords> MonkeyRecords { get; set; }
        private GridDataSet gridDataSet;

        private Map map;
        public Wood(int woodID, List<Tree> trees, Map map, string path, DBRepository db)
        {
            this.woodID = woodID;
            this.trees = trees;
            monkeys = new List<Monkey>();
            this.map = map;
            this.path = path;
            this.db = db;
            MonkeyRecords = new List<DBMonkeyRecords>();
            this.gridDataSet = new GridDataSet(map, 10, trees);
        }



        public void PlaceMonkey(string monkeyName, int monkeyID)
        {
            int treeNr;
            do
            {
                treeNr = r.Next(0, trees.Count - 1);
            }
            while (trees[treeNr].hasMonkey);
            Monkey m = new Monkey(monkeyID, monkeyName, trees[treeNr]);
            monkeys.Add(m);
            trees[treeNr].hasMonkey = true;
        }


        public async Task EscapeAsync()
        {
            Dictionary<Monkey, Task<List<Tree>>> routes = new Dictionary<Monkey, Task<List<Tree>>>();
            foreach (Monkey m in monkeys)
            {
                routes[m] = EscapeMonkeyAsync(m);
            }
            await WriteEscaperoutesToBitmapAsync((await Task.WhenAll(routes.Values)).ToList());
        }


        private async Task writeRouteToDBAsync(Monkey monkey, List<Tree> route)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"{woodID}:write db routes {woodID},{monkey.name} start");
                List<DBRouteRecords> routeRecords = new List<DBRouteRecords>();
                List<DBLogs> logs = new List<DBLogs>();
                for (int j = 0; j < route.Count; j++)
                {
                    routeRecords.Add(new DBRouteRecords(j, route[j].treeID, route[j].x, route[j].y));
                    logs.Add(new DBLogs(woodID, monkey.monkeyID, $"{monkey.name} is now in tree {route[j].treeID} at location ({route[j].x},{route[j].y})"));
                }
                DBMonkeyRecords monkeyRecord = new DBMonkeyRecords(monkey.monkeyID, monkey.name, woodID, routeRecords);
                await db.InsertMonkeyRecordsAsync(monkeyRecord);
                MonkeyRecords.Add(monkeyRecord);
                await db.InsertLogsAsync(logs);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"{woodID}:write db routes {woodID},{monkey.name} end");
            }
            catch (Exception ex)
            {

                throw new Exception("Error in writeRouteToDB",ex);
            }
        }


        public async Task WriteEscaperoutesToBitmapAsync(List<List<Tree>> routes)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{woodID}:write bitmap routes {woodID} start");
                Color[] cvalues = new Color[] { Color.Red, Color.Yellow, Color.Blue, Color.Cyan, Color.GreenYellow };
                Bitmap bm = new Bitmap((map.xmax - map.xmin) * drawingFactor, (map.ymax - map.ymin) * drawingFactor);
                Graphics g = Graphics.FromImage(bm);
                int delta = drawingFactor / 2;
                Pen p = new Pen(Color.Green, 1);
                foreach (Tree t in trees)
                {
                    g.DrawEllipse(p, t.x * drawingFactor, t.y * drawingFactor, drawingFactor, drawingFactor);
                }
                int colorN = 0;
                foreach (List<Tree> route in routes)
                {
                    int p1x = route[0].x * drawingFactor + delta;
                    int p1y = route[0].y * drawingFactor + delta;
                    Color color = cvalues[colorN % cvalues.Length];
                    Pen pen = new Pen(color, 1);
                    g.DrawEllipse(pen, p1x - delta, p1y - delta, drawingFactor, drawingFactor);
                    g.FillEllipse(new SolidBrush(color), p1x - delta, p1y - delta, drawingFactor, drawingFactor);
                    for (int i = 1; i < route.Count; i++)
                    {
                        g.DrawLine(pen, p1x, p1y, route[i].x * drawingFactor + delta, route[i].y * drawingFactor + delta);
                        p1x = route[i].x * drawingFactor + delta;
                        p1y = route[i].y * drawingFactor + delta;
                    }
                    colorN++;
                }
                bm.Save(Path.Combine(path, woodID.ToString() + "_escapeRoutes.jpg"), ImageFormat.Jpeg);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{woodID}:write bitmap routes {woodID} end");
            }
            catch (Exception ex)
            {

                throw new Exception("Error in WriteEscaperoutesToBitmap", ex);
            }
        }


        public async Task WriteWoodToDBAsync()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{woodID}:write db wood {woodID} start");
                await db.InsertWoodRecordsAsync(new DBWoodRecords(woodID, trees));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{woodID}:write db wood {woodID} end");
            }
            catch (Exception ex)
            {

                throw new Exception("Error in WriteWoodToDB", ex);
            }
        }
        public async Task<List<Tree>> EscapeMonkeyAsync(Monkey monkey)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{woodID}:start {woodID},{monkey.name}");
                Dictionary<int, bool> visited = new Dictionary<int, bool>();
                trees.ForEach(x => visited.Add(x.treeID, false));
                List<Tree> route = new List<Tree>() { monkey.tree };
                do
                {
                    visited[monkey.tree.treeID] = true;
                    SortedList<double, List<Tree>> distanceToMonkey = new SortedList<double, List<Tree>>();

                    // Get the cell of the current tree
                    int x = (int)Math.Floor((monkey.tree.x - map.xmin) / gridDataSet.Delta);
                    int y = (int)Math.Floor((monkey.tree.y - map.ymin) / gridDataSet.Delta);

                    // Get the neighboring cells
                    var neighboringCells = new List<List<Tree>>();
                    for (int i = Math.Max(0, x - 1); i <= Math.Min(gridDataSet.NX - 1, x + 1); i++)
                    {
                        for (int j = Math.Max(0, y - 1); j <= Math.Min(gridDataSet.NY - 1, y + 1); j++)
                        {
                            neighboringCells.Add(gridDataSet.GridData[i][j]);
                        }
                    }

                    // Search for the closest tree in the neighboring cells
                    foreach (var cell in neighboringCells)
                    {
                        foreach (Tree t in cell)
                        {
                            if (!visited[t.treeID] && !t.hasMonkey)
                            {
                                double d = Math.Sqrt(Math.Pow(t.x - monkey.tree.x, 2) + Math.Pow(t.y - monkey.tree.y, 2));
                                if (distanceToMonkey.ContainsKey(d)) distanceToMonkey[d].Add(t);
                                else distanceToMonkey.Add(d, new List<Tree>() { t });
                            }
                        }
                    }

                    //distance to border            
                    //noord oost zuid west
                    double distanceToBorder = new List<double>(){ map.ymax - monkey.tree.y,
                map.xmax - monkey.tree.x,monkey.tree.y-map.ymin,monkey.tree.x-map.xmin }.Min();

                    if (distanceToMonkey.Any() && distanceToBorder < distanceToMonkey.First().Key)
                    {
                        await writeRouteToDBAsync(monkey, route);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"{woodID}:end {woodID},{monkey.name}");
                        return route;
                    }

                    if (distanceToMonkey.Count == 0)
                    {
                        await writeRouteToDBAsync(monkey, route);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"{woodID}:end {woodID},{monkey.name}");
                        return route;
                    }

                    route.Add(distanceToMonkey.First().Value.First());
                    monkey.tree = distanceToMonkey.First().Value.First();
                }
                while (true);
            }
            catch (Exception ex)
            {

                throw new Exception("Error in EscapeMonkey", ex);
            }
        }
    }
}
