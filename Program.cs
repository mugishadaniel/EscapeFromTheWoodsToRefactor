using Amazon.Runtime.Internal.Util;
using EscapeFromTheWoods.Logging;
using EscapeFromTheWoods.Objects;
using MongoDBManager;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EscapeFromTheWoods
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine("Hello World!");
            string connectionString = @"mongodb://localhost:27017/";
            DBRepository db = new DBRepository(connectionString);
            
            string path = @"C:\Hogent\programmeren specialisatie\EscapeWoods\EscapeFromTheWoodsToRefactor\Woodsmap";

            Map m1 = new Map(0, 500, 0, 500);
            Wood w1 = WoodBuilder.GetWood(500, m1, path,db);
            w1.PlaceMonkey("Alice", IDgenerator.GetMonkeyID());
            w1.PlaceMonkey("Janice", IDgenerator.GetMonkeyID());
            w1.PlaceMonkey("Toby", IDgenerator.GetMonkeyID());
            w1.PlaceMonkey("Mindy", IDgenerator.GetMonkeyID());
            w1.PlaceMonkey("Jos", IDgenerator.GetMonkeyID());
            
            Map m2 = new Map(0, 200, 0, 400);
            Wood w2 = WoodBuilder.GetWood(2500, m2, path,db);
            w2.PlaceMonkey("Tom", IDgenerator.GetMonkeyID());
            w2.PlaceMonkey("Jerry", IDgenerator.GetMonkeyID());
            w2.PlaceMonkey("Tiffany", IDgenerator.GetMonkeyID());
            w2.PlaceMonkey("Mozes", IDgenerator.GetMonkeyID());
            w2.PlaceMonkey("Jebus", IDgenerator.GetMonkeyID());

            Map m3 = new Map(0, 400, 0, 400);
            Wood w3 = WoodBuilder.GetWood(2000, m3, path,db);
            w3.PlaceMonkey("Kelly", IDgenerator.GetMonkeyID());
            w3.PlaceMonkey("Kenji", IDgenerator.GetMonkeyID());
            w3.PlaceMonkey("Kobe", IDgenerator.GetMonkeyID());
            w3.PlaceMonkey("Kendra", IDgenerator.GetMonkeyID());

            await w1.WriteWoodToDBAsync();
            await w2.WriteWoodToDBAsync();
            await w3.WriteWoodToDBAsync();

            await w1.EscapeAsync();
            LogWriter logWriter = new LogWriter(w1.MonkeyRecords);

            await w2.EscapeAsync();
            LogWriter logWriter2 = new LogWriter(w2.MonkeyRecords);

            await w1.EscapeAsync();
            LogWriter logWriter1 = new LogWriter(w3.MonkeyRecords);

            stopwatch.Stop();
            // Write result.
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            Console.WriteLine("end");
        }
    }
}
