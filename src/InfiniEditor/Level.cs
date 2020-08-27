using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace InfiniEditor
{
    public class Level
    {
        #region attributes definitions
        private bool solvedLocally;
        private long steamUser;
        private Dictionary<string, string> keyVals;
        public string Path { get; private set; }
        public bool Saved { get; private set; }

        public string FileName
        {
            get
            {
                return System.IO.Path.GetFileName(Path);
            }
        }

        private string title;
        public string Title
        {
            get
            {
                if (title == null)
                    title = Value<string>("Title", "New custom level");
                return title;
            }
            set
            {
                if (title != value)
                {
                    title = value;
                    Saved = false;
                }
            }
        }

        public DateTime LastChange { get; private set; }

        public Sources Source { get; private set; }
        public enum Sources { Game, Custom, Workshop }

        private long author = -1;
        public long Author {
            get
            {
                if (author == -1)
                {
                    author = Source == Sources.Custom ? steamUser : Value<long>("Creator", 0);
                }
                return author;
            }
        }

        public string AuthorLink
        {
            get
            {
                if (Source == Sources.Game)
                    return "http://www.zachtronics.com/";
                return "http://steamcommunity.com/profiles/" + Author;
            }
        }

        private string authorName;
        public string AuthorName
        {
            get
            {
                if (Source == Sources.Game)
                {
                    authorName = "Zachtronics";
                }
                if (authorName == null)
                {
                    try {
                        authorName = LevelsManager.GetSteamName(Author);
                    }
                    catch
                    {
                        return Author.ToString();
                    }
                }
                return authorName;
            }
        }

        private int workshopID = -1;
        public int WorkshopID
        {
            get
            {
                if (workshopID == -1)
                    workshopID = Value<int>("WorkshopID", 0);
                return workshopID;
            }
        }

        public string WorkshopLink
        {
            get
            {
                return WorkshopID == 0 ? "" : "http://steamcommunity.com/sharedfiles/filedetails/?id=" + WorkshopID;
            }
        }

        private bool? advanced;
        public bool Advanced
        {
            get
            {
                if (advanced == null)
                    advanced = Value<bool>("IsAdvanced", false);
                return (bool)advanced;
            }
            set
            {
                if (advanced != value)
                {
                    advanced = value;
                    Saved = false;
                }
            }
        }

        private bool? solved;
        public bool Solved
        {
            get
            {
                if (solved == null)
                {
                    if (Source != Sources.Custom)
                    {
                        solved = solvedLocally;
                    }
                    else
                    {
                        solved = Value<bool>("Solved", false);
                    }
                }
                return (bool)solved;
            }
        }

        private IEnumerable<int> allowedBlocks;
        public IEnumerable<int> AllowedBlocks
        {
            get
            {
                if (allowedBlocks == null)
                    allowedBlocks = Value<List<int>>("AllowedBlockTypes", new List<int> { 1, 0, 18, 8, 131, 5, 149, 4, 35, 2, 39, 25, 17, 138, 9, 15, 16 });
                return allowedBlocks.ToList().AsReadOnly();
            }
            set
            {
                if (allowedBlocks != value)
                {
                    allowedBlocks = value;
                    Saved = false;
                }
            }
        }

        private BlocksCollection blocks;
        public BlocksCollection Blocks
        {
            get
            {
                if (blocks == null)
                    blocks = BlocksCollection.FromBase64(Value<string>("Inputs", ""), Value<string>("Outputs", ""), Value<string>("WorldBlocks", ""));
                return blocks;
            }
            set
            {
                if (blocks != value)
                {
                    blocks = value;
                    Saved = false;
                }
            }
        }

        private Bitmap preview;
        public Bitmap Preview
        {
            get
            {
                if (preview == null)
                    preview = Value<Bitmap>("PreviewImage", new Bitmap(1, 1));
                return preview;
            }
            set
            {
                if (preview != value)
                {
                    preview = value;
                    Saved = false;
                    try
                    {
                        keyVals.Add("PreviewImage", "_");
                    }
                    catch { }
                }
            }
        }

        private Environments? environment;
        public Environments Environment
        {
            get
            {
                if (environment == null)
                    environment = Value<Environments>("AdvEnvironment", Environments.ProvingGrounds);
                return (Environments)environment;
            }
            set
            {
                if (environment != value)
                {
                    environment = value;
                    Saved = false;
                }
            }
        }

        private int inputDelay = -1;
        public int InputDelay
        {
            get
            {
                if (inputDelay == -1)
                    inputDelay = Value<int>("AdvInputRate", 3);
                return inputDelay;
            }
            set
            {
                if (inputDelay != value)
                {
                    inputDelay = value;
                    Saved = false;
                }
            }
        }

        private bool? constantInputRatio;
        public bool ConstantInputRatio
        {
            get
            {
                if (constantInputRatio == null)
                    constantInputRatio = Value<bool>("AdvForceConstantInputRatio", false);
                return (bool)constantInputRatio;
            }
            set
            {
                if (constantInputRatio != value)
                {
                    constantInputRatio = value;
                    Saved = false;
                }
            }
        }

        private bool? inputTops;
        public bool InputTops
        {
            get
            {
                if (inputTops == null)
                    inputTops = Value<bool>("AdvCreateInputTops", true);
                return (bool)inputTops;
            }
            set
            {
                if (inputTops != value)
                {
                    inputTops = value;
                    Saved = false;
                }
            }
        }

        private bool? noOneBlockTops;
        public bool NoOneBlockTops
        {
            get
            {
                if (noOneBlockTops == null)
                    noOneBlockTops = Value<bool>("AdvNoTopsForOneBlockInputs", false);
                return (bool)noOneBlockTops;
            }
            set
            {
                if (noOneBlockTops != value)
                {
                    noOneBlockTops = value;
                    Saved = false;
                }
            }
        }

        private Bitmap screenshot;
        public Bitmap Screenshot
        {
            get
            {
                if (screenshot == null)
                    screenshot = Value<Bitmap>("AdvPreviewImage", new Bitmap(1, 1));
                return screenshot;
            }
            set
            {
                if (screenshot != value)
                {
                    screenshot = value;
                    Saved = false;
                    try
                    {
                        keyVals.Add("AdvPreviewImage", "_");
                    }
                    catch { }
                }
            }
        }

        public static List<ListItem<Environments>> EnvironmentsNames = new List<ListItem<Environments>>{
            new ListItem<Environments>(Environments.ProvingGrounds, "Proving grounds" ),
            new ListItem<Environments>(Environments.Skydock19, "Skydock 19" ),
            new ListItem<Environments>(Environments.ResSite52681, "Res. site 526.81" ),
            new ListItem<Environments>(Environments.ProductionZone2, "Production zone 2" ),
            new ListItem<Environments>(Environments.ResSite33811, "Res. site 338.11" ),
            new ListItem<Environments>(Environments.ResSite90242, "Res. site 902.42" ),
            new ListItem<Environments>(Environments.AsteroidFieldA, "Asteroid field A" ),
            new ListItem<Environments>(Environments.AsteroidFieldB, "Asteroid field B" ),
            new ListItem<Environments>(Environments.ProductionZone1, "Production zone 1" ),
            new ListItem<Environments>(Environments.AtroposStation, "Atropos station" ),
        };

        public ListViewItem ListViewItem
        {
            get
            {
                ListViewItem lvi = new ListViewItem(new string[] { Title, Advanced ? Level.EnvironmentsNames.First(i => i.Value == Environment).Text : "Classic", Solved ? "Yes" : "No", AuthorName, FileName });
                lvi.Tag = this;
                return lvi;
            }
        }

        #endregion

        public enum Environments
        {
            ProvingGrounds = 1, Skydock19 = 2, ResSite52681 = 3, ProductionZone2 = 4, ResSite33811 = 5,
            ResSite90242 = 6, AsteroidFieldA = 7, AsteroidFieldB = 8, ProductionZone1 = 9, AtroposStation = 10
        }

        public Level(Sources source, string path, bool solvedLocally, long steamUser)
        {
            Path = path;
            Source = source;
            LastChange = File.GetLastWriteTime(path);            
            keyVals = ParseFile(path);
            Saved = true;
            this.solvedLocally = solvedLocally;
            this.steamUser = steamUser;
        }

        #region FileParsing
        private T Value<T>(string key, T def)
        {
            if (!keyVals.ContainsKey(key))
            {
                return def;
            }
            if (typeof(T) == typeof(string))
            {
                return ConvertTo<T>(keyVals[key]);
            }
            if (typeof(T) == typeof(bool))
            {
                return ConvertTo<T>(keyVals[key] == "True");
            }
            if (typeof(T) == typeof(List<int>))
            {
                List<int> output = new List<int>();
                foreach (string block in keyVals[key].Split(new string[] { "," }, StringSplitOptions.None))
                {
                    int v;
                    if (Int32.TryParse(block, out v))
                    {
                        output.Add(v);
                    }
                }
                return ConvertTo<T>(output);
            }
            if (typeof(T) == typeof(Environments))
            {
                int v = 1;
                Int32.TryParse(keyVals[key], out v);
                v = Math.Max(1, v);
                return ConvertTo<T>((Environments)v);
            }
            if (typeof(T) == typeof(int))
            {
                int v = 1;
                Int32.TryParse(keyVals[key], out v);
                return ConvertTo<T>(v);
            }
            if (typeof(T) == typeof(long))
            {
                long v = 1;
                Int64.TryParse(keyVals[key], out v);
                return ConvertTo<T>(v);
            }
            if (typeof(T) == typeof(Bitmap))
            {
                return ConvertTo<T>(Base64ToBitmap(keyVals[key]));
            }
            return default(T);
        }
        private T ConvertTo<T>(Object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        private Dictionary<string, string> ParseFile(string path)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string line in File.ReadLines(path))
            {
                string[] keyVal = line.Split(new string[] { " = " }, 2, StringSplitOptions.None);
                string key = keyVal[0];
                string val = keyVal.Count() == 2 ? keyVal[1] : "";
                dict.Add(key, val);
            }
            return dict;
        }
        #endregion

        private Bitmap Base64ToBitmap(string base64)
        {
            if(base64 == "")
            {
                return null;
            }
            return new Bitmap(new MemoryStream(Convert.FromBase64String(base64)));
        }

        private string BitmapToBase64(Bitmap bmp, ImageFormat imgF)
        {
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, imgF);
            return Convert.ToBase64String(ms.ToArray());
        }

        public bool Save()
        {
            ImageFormat imgF = Source == Sources.Game ? ImageFormat.Jpeg : ImageFormat.Png;
            Blocks.UpdateBlocksGroups();
            try {
                Dictionary<string, string> dict = new Dictionary<string, string>
                {
                    {"Title", Title }
                };
                if(Source == Sources.Workshop)
                {
                    dict.Add("Creator", Author.ToString());
                    dict.Add("WorkhopID", WorkshopID.ToString());
                }
                dict.Add("IsAdvanced", Advanced.ToString());
                dict.Add("Solved", Solved.ToString());
                dict.Add("AllowedBlockTypes", String.Join(",", AllowedBlocks.Select<int, string>(i => i.ToString())));
                dict.Add("AdvEnvironment", ((int)Environment).ToString());
                dict.Add("AdvInputRate", InputDelay.ToString());
                dict.Add("AdvForceConstantInputRatio", ConstantInputRatio.ToString());
                dict.Add("AdvCreateInputTops", InputTops.ToString());
                dict.Add("AdvNoTopsForOneBlockInputs", NoOneBlockTops.ToString());
                dict.Add("Inputs", Blocks.ToBase64(Block.Roles.In));
                dict.Add("Outputs", Blocks.ToBase64(Block.Roles.Out));
                dict.Add("WorldBlocks", Blocks.ToBase64(Block.Roles.World));
                if (keyVals.ContainsKey("PreviewImage"))
                {
                    dict.Add("PreviewImage", BitmapToBase64(Preview, imgF));
                }
                if (keyVals.ContainsKey("AdvPreviewImage"))
                {
                    dict.Add("AdvPreviewImage", BitmapToBase64(Screenshot, imgF));
                }
                using (StreamWriter sr = new StreamWriter(Path, false))
                {
                    foreach (KeyValuePair<string, string> keyval in dict)
                    {
                        sr.WriteLine(keyval.Key + " = " + keyval.Value);
                    }
                }
            }
            catch
            {
                return false;
            }
            Saved = true;
            return true;
        }

        public void SaveWithoutSaving()
        {
            Saved = true;
        }
    }
}
