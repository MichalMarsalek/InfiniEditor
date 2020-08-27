using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace InfiniEditor
{
    public class BlockInfo
    {
        public int Type { get; private set; }
        public string Name { get; private set; }
        public string Group { get; private set; }
        private Image image;
        public Image Image
        {
            get
            {
                if (image == null)
                {
                    try
                    {
                        image = Image.FromFile(ImagePath);
                    }
                    catch
                    {
                        image = Properties.Resources.BlockImageNotFound;
                    }
                }
                return image;
            }
        }

        public string ImagePath
        {
            get
            {
                return @"blocks\images\" + BlockID + ".jpg";
            }
        }

        public string BlockID
        {
            get
            {
                return (Decal ? "d" : "") + Type;
            }
        }
        public Dictionary<string, Symmetry> Symmetries { get; private set; }
        public bool Decal { get; private set; }
        public string LongID
        {
            get
            {
                return (Decal ? "Decal " : "Block ") + Type;
            }
        }

        public HashSet<string> ObviousFlags { get; private set; }
        public HashSet<string> OtherFlags { get; private set; } //Stair, Transparent, Light, Structure...
        public HashSet<string> AllFlags
        {
            get
            {
                return new HashSet<string>(ObviousFlags.Concat(OtherFlags));
            }
        }

        public BoundingBox BoundingBox { get; private set; }

        public BlockInfo(XElement block, string groupName)
        {
            Group = groupName;
            Decal = block.Name == "Decal";
            Type = (int)block.Attribute("Type");
            Name = (string)block.Attribute("Name");
            ObviousFlags = new HashSet<string>(Name.ToLower().Replace("(", ", ").Replace(")", ", ").Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries));
            foreach(string flag in new HashSet<string>(ObviousFlags))
            {
                foreach(string subflag in flag.Split(' '))
                {
                    ObviousFlags.Add(subflag);
                }
            }
            ObviousFlags.Add(Decal ? "decal" : "block");
            OtherFlags = new HashSet<string>();
            Symmetries = new Dictionary<string, Symmetry>();
            foreach(XElement flag in block.Elements("Flag"))
            {
                OtherFlags.Add(flag.Value);
            }
            foreach (XElement sym in block.Elements("Symmetry"))
            {
                Symmetry s = sym.Attribute("With") == null ? new Symmetry(sym.Value) : new Symmetry(sym.Value, (int)sym.Attribute("With"));
                Symmetries.Add(sym.Value, s);
            }
            var bbox = block.Element("BoundingBox");
            if (bbox == null)
            {
                BoundingBox = new BoundingBox();
            }
            else
            {
                BoundingBox = new BoundingBox(new Vec((int)bbox.Attribute("MinX"), (int)bbox.Attribute("MinY"), (int)bbox.Attribute("MinZ")),
                                              new Vec((int)bbox.Attribute("MaxX"), (int)bbox.Attribute("MaxY"), (int)bbox.Attribute("MaxZ")));
            }
        }

        private ListViewItem lvi;
        public ListViewItem ListViewItem
        {
            get
            {
                if (lvi == null)
                {
                    lvi = new ListViewItem(new string[] { Name, LongID, SymmetriesToString(false), BoundingBox.SizeToString(), String.Join(", ", OtherFlags) });
                    lvi.Tag = this;
                    lvi.ImageKey = BlockID;
                }
                return lvi;
            }
        }

        private ListViewItem lvi2;
        public ListViewItem ListViewItem2
        {
            get
            {
                if (lvi2 == null)
                {
                    lvi2 = new ListViewItem(new string[] { Name, Group });
                    lvi2.Tag = this;
                    lvi2.ImageKey = BlockID;
                }
                return lvi2;
            }
        }

        public string SymmetriesToString(bool loong)
        {
            if (loong)
            {
                return String.Join(", ", Symmetries.Values.Select(i => i.ToString(Decal)));
            }
            return String.Join(", ", Symmetries.Values);
        }

        public int SameFlags(BlockInfo other)
        {
            return AllFlags.Intersect(other.AllFlags).Count();
        }

        public int SameObviousFlags(BlockInfo other)
        {
            return ObviousFlags.Intersect(other.ObviousFlags).Count();
        }

        public int SameOtherFlags(BlockInfo other)
        {
            return OtherFlags.Intersect(other.OtherFlags).Count();
        }

        public static bool operator ==(BlockInfo a, BlockInfo b)
        {
            return a.Type == b.Type && a.Decal == b.Decal;
        }

        public static bool operator !=(BlockInfo a, BlockInfo b)
        {
            return !(a == b);
        }

        public bool FlagsCondition(string cond)
        {
            return cond.Split(';').Any(i => FlagsAnd(i));
        }

        private bool FlagsAnd(string cond)
        {
            return cond.ToLower().Split(',').All(i => FlagsSingle(i));
        }

        private bool FlagsSingle(string flag)
        {
            flag = flag.Trim();
            if(flag == "")
            {
                return true;
            }
            if(flag[0] == '!')
            {
                return !AllFlags.Contains(flag.Substring(1));
            }
            return AllFlags.Contains(flag);
        }
    }
}
