using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InfiniEditor
{
    public class LevelsManager
    {
        private string path1;
        private long steamUser;
        private List<string> solvedW;
        private List<string> solvedG;
        private static Dictionary<long, string> steamNames = new Dictionary<long, string>();
        private static Dictionary<string, Level> loadedLevels = new Dictionary<string, Level>();

        public LevelsManager(long steamUser)
        {
            path1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", "Infinifactory", steamUser.ToString());
            this.steamUser = steamUser;
            int skip = 0;
            solvedW = new List<string>();
            solvedG = new List<string>();
            foreach (string line in File.ReadLines(Path.Combine(path1, "save.dat")))
            {
                if(skip > 0)
                {
                    skip--;
                    continue;
                }
                if (line.StartsWith("Best."))
                {
                    solvedG.Add(line.Split('.')[1]);
                    skip = 2;
                }
                else if (line.StartsWith("SolvedWorkshopPuzzles = "))
                {
                    solvedW = line.Split(',').Where(i => i != "SolvedWorkshopPuzzles = ").ToList();
                }
            }
        }

        public static IEnumerable<long> SteamUsers()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", "Infinifactory");
            foreach(string dir in Directory.EnumerateDirectories(path))
            {
                long id;
                if (Int64.TryParse(Path.GetFileName(dir), out id))
                {
                    yield return id;
                }
            }
        }

        public IEnumerable<Level> GetLevels(Level.Sources src)
        {
            foreach (string file in GetLevelsFiles(src))
            {
                yield return GetLevel(src, file);
            }
        }

        public Level GetLevel(Level.Sources src, string path)
        {
            if (loadedLevels.ContainsKey(path))
            {
                return loadedLevels[path];
            }
            Level lvl = new Level(src, path, (src == Level.Sources.Workshop ? solvedW : solvedG).Contains(Path.GetFileName(path)), steamUser);
            loadedLevels.Add(path, lvl);
            return lvl;
        }

        public IEnumerable<Level> GetLevels()
        {
            foreach (Level lvl in GetLevels(Level.Sources.Game))
            {
                yield return lvl;
            }
            foreach (Level lvl in GetLevels(Level.Sources.Custom))
            {
                yield return lvl;
            }
            foreach (Level lvl in GetLevels(Level.Sources.Workshop))
            {
                yield return lvl;
            }
        }

        public void ClearLoadedLevels()
        {
            loadedLevels.Clear();
        }

        public int CountLevels(Level.Sources src)
        {
            return GetLevelsFiles(src).Length;
        }

        private string[] GetLevelsFiles(Level.Sources src)
        {
            string path = LevelsDirectory(src);
            if (Directory.Exists(path))
            {
                return Directory.GetFiles(path);
            }
            return new string[0];
        }

        public int CountLevels()
        {
            return CountLevels(Level.Sources.Game) + CountLevels(Level.Sources.Custom) + CountLevels(Level.Sources.Workshop);
        }

        private string LevelsDirectory(Level.Sources src)
        {
            if(src == Level.Sources.Game)
            {
                return "game levels";
            }
            return Path.Combine(path1, src == Level.Sources.Custom ? "custom" : "workshop");
        }

        public static string GetSteamName(long steamID64)
        {
            if (!steamNames.ContainsKey(steamID64))
            {
                XDocument xml = XDocument.Load("http://steamcommunity.com/profiles/" + steamID64 + "?xml=1");
                steamNames[steamID64] = xml.Root.Element(XName.Get("steamID")).Value;
            }
            return steamNames[steamID64];
        }

    }
}
