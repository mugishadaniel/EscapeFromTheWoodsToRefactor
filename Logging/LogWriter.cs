using MongoDBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EscapeFromTheWoods.Logging
{
    public class LogWriter
    {
        public List<DBMonkeyRecords> MonkeyRecords { get; set; }

        public LogWriter(List<DBMonkeyRecords> monkeyRecords)
        {
            MonkeyRecords = monkeyRecords;
        }

        public Task StartLoggingAsync()
        {
            return Task.Run(() => LogOnTxtFileAsync());
        }   

        private async Task LogOnTxtFileAsync()
        {
            Console.WriteLine("Starting logging");
            string path = @"C:\Hogent\programmeren specialisatie\EscapeWoods\EscapeFromTheWoodsToRefactor\Logging\Logs";
            string fileName = $"Log_{DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss")}_Wood{MonkeyRecords[0].WoodID}.txt";
            string fullPath = Path.Combine(path, fileName);
            List<(string MonkeyName, int SeqNr, int TreeID, int X, int Y)> logEntries = new List<(string, int, int, int, int)>();

            foreach (var monkey in MonkeyRecords)
            {
                foreach (var tree in monkey.Route)
                {
                    logEntries.Add((monkey.MonkeyName, tree.SeqNr, tree.TreeID, tree.X, tree.Y));
                }
            }

            var sortedLogEntries = logEntries
                .OrderBy(entry => entry.SeqNr)
                .ThenBy(entry => entry.MonkeyName)
                .ToList();

            using (StreamWriter sw = File.CreateText(fullPath))
            {
                foreach (var entry in sortedLogEntries)
                {
                   await sw.WriteLineAsync($"{entry.MonkeyName} is in tree {entry.TreeID} at ({entry.X},{entry.Y})");
                }
            }
            Console.WriteLine("Logging ended");
        }
    }
}
