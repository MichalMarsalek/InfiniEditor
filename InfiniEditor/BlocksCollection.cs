using NLua;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace InfiniEditor
{
    public class BlocksCollection
    {
        public int Version { get; set; }
        public Dictionary<Vec, Block> blocksDict; //TODO make this private / add function for iterating through this
        private static readonly int CurrentVersion = 3;
        private List<BlocksGroup> blocksGroups;
        public bool ClipboardReady { get; protected set; }
        public Block GetBlock(Vec pos)
        {
            if (!blocksDict.ContainsKey(pos))
            {
                return null;
            }
            return blocksDict[pos];
        }
        public void SetBlock(Vec pos, Block value)
        {
            if (value == null)
            {
                blocksDict.Remove(pos);
            }
            else
            {
                blocksDict[pos] = value;
            }
        }
        public Block this[Object xyz]
        {
            get
            {
                if(xyz is LuaTable)
                {
                    return GetBlock(LuaTableToVec((LuaTable)(xyz)));
                }
                return GetBlock((Vec)xyz);

            }
            set
            {
                if (xyz is LuaTable)
                {
                    SetBlock(LuaTableToVec((LuaTable)(xyz)), value);
                }
                else
                {
                    SetBlock((Vec)xyz, value);
                }
            }
        }
        private Vec LuaTableToVec(LuaTable t)
        {
            var p = ((LuaTable)t);
            return new Vec((int)(double)p[1], (int)(double)p[2], (int)(double)p[3]);
        }

        public IEnumerable<Vec> OccupiedCoordinates
        {
            get
            {
                return blocksDict.Keys;
            }
        }
        public bool Valid { get; private set; }
        
        public BlocksCollection()
        {            
            Valid = true;
            blocksDict = new Dictionary<Vec, Block>();
        }

        public BlocksCollection(BlocksCollection old)
        {
            Valid = old.Valid;
            blocksDict = new Dictionary<Vec, Block>(old.blocksDict);
            Version = old.Version;
        }

        public int Count
        {
            get
            {
                return blocksDict.Count;
            }
        }

        #region BASE64 CONVERSIONS
        public void UpdateBlocksGroups()
        {
            blocksGroups = new List<BlocksGroup>();
            foreach (var pair in blocksDict)
            {
                Block b = pair.Value;
                if (b.Type >= 0)
                {
                    b.Position = pair.Key;
                    BlocksGroup group = blocksGroups.FirstOrDefault(i => i.Role == b.Role && i.Group == b.Group);
                    if (group == null)
                    {
                        group = new BlocksGroup(b.Role, b.Group);
                        blocksGroups.Add(group);
                    }
                    group.Blocks.Add(b);
                }
            }
        }

        public static BlocksCollection FromBase64(string inputs, string outputs, string world)
        {
            BlocksCollection blocks = new BlocksCollection();
            blocks.AddFromBase64(world, Block.Roles.World, 0);
            int group = 1;
            foreach (string input in inputs.Split('#'))
            {
                blocks.AddFromBase64(input, Block.Roles.In, group++);
            }
            foreach (string output in outputs.Split('#'))
            {
                blocks.AddFromBase64(output, Block.Roles.Out, group++);
            }
            return blocks;
        }

        private void AddFromBase64(string base64, Block.Roles role, int group)
        {
            Version = CurrentVersion;
            if (base64 == "")
            {
                return;
            }
            try
            {
                using (BinaryReader streamReader = new BinaryReader(new MemoryStream(Convert.FromBase64String(base64))))
                {
                    Version = streamReader.ReadInt32();
                    int blocks_count = streamReader.ReadInt32();
                    for (int i = 0; i < blocks_count; i++)
                    {
                        Block b = new Block(streamReader, role, group);
                        blocksDict.Add(b.Position, b);
                    }
                }
            }
            catch { }
        }

        public string ToBase64(Block.Roles role)
        {
            return String.Join("#", blocksGroups.Where(i => i.Role == role).Select(i => i.ToBase64()));
        }
        #endregion

        #region XML CONVERSIONS
        public string ToXML()
        {
            XDocument xml = new XDocument(
                new XElement("Infiniblocks",
                    new XAttribute("Origin", "InfiniEditor"),
                    from bgroup in blocksGroups select
                        bgroup.ToXElement()
                )
            );
            return xml.ToString();
        }
        
        public static BlocksCollection FromXML(string source)
        {
            BlocksCollection blocks = new BlocksCollection();
            try {
                XDocument xml = XDocument.Parse(source);
                blocks.AddFromXML(xml.Root.Element("World"), Block.Roles.World);
                foreach (XElement input in xml.Root.Elements("Input"))
                {
                    blocks.AddFromXML(input, Block.Roles.In);
                }
                foreach (XElement output in xml.Root.Elements("Output"))
                {
                    blocks.AddFromXML(output, Block.Roles.Out);
                }
            }
            catch
            {
                blocks.Valid = false;
            }
            return blocks;
        }
        private void AddFromXML(XElement blocks, Block.Roles role)
        {
            Version = (int)blocks.Attribute("Version");
            int group = (int)blocks.Attribute("Group");
            foreach (var b in blocks.Elements("Block")) {
                Block block = new Block(b, role, group);
                blocksDict.Add(block.Position, block);
            }
        }

        #endregion

        #region Clipboard operations
        public void ToClipboard()
        {
            ClipboardNotification.Suspend();
            UpdateBlocksGroups();
            Clipboard.SetText(ToXML());
        }

        public static BlocksCollection FromClipboard()
        {
            BlocksCollection blocks = BlocksCollection.FromXML(Clipboard.GetText());
            blocks.ClipboardReady = true;
            return blocks;
        }
        #endregion

        #region BLOCKS ACTIONS
        public void AddBlocks(BlocksCollection b2, bool overide=false)
        {
            foreach(var pair in b2.blocksDict)
            {
                if (blocksDict.ContainsKey(pair.Key))
                {
                    if (overide)
                    {
                        blocksDict[pair.Key] = pair.Value;
                    }
                }
                else
                {
                    blocksDict.Add(pair.Key, pair.Value);
                }
            }            
            if (ClipboardReady)
            {
                ToClipboard();
            }
        }

        public void Shift(Vec vec)
        {
            var blocks = new Dictionary<Vec, Block>();
            foreach (var block in blocksDict)
            {
                blocks.Add(block.Key + vec, block.Value);
            }
            blocksDict = blocks;
            if (ClipboardReady)
            {
                ToClipboard();
            }
        }

        public void Rotate(int rot)
        {
            var blocks = new Dictionary<Vec, Block>();
            foreach (var block in blocksDict)
            {
                block.Value.RotateWithDecals(rot);
                blocks.Add(block.Key ^ rot, block.Value);
            }
            blocksDict = blocks;
            if (ClipboardReady)
            {
                ToClipboard();
            }
        }

        public void Replace(int from, int to)
        {
            foreach (var block in blocksDict)
            {
                block.Value.Replace(from, to);
            }
            if (ClipboardReady)
            {
                ToClipboard();
            }
        }

        public void Flip(Axis axis, BlockInfosManager bim)
        {
            var blocks = new Dictionary<Vec, Block>();
            foreach (var block in blocksDict)
            {
                block.Value.Flip(axis, bim.BlockInfosList.FirstOrDefault(i => i.Type == block.Value.Type));
                blocks.Add(block.Key | axis, block.Value);
            }
            blocksDict = blocks;
            if (ClipboardReady)
            {
                ToClipboard();
            }
        }

        public void DeleteAll()
        {
            this.blocksDict.Clear();
        }

        public int CountOverlaps(BlocksCollection b)
        {
            List<Vec> aOcc = OccupiedCoordinates.ToList();
            List<Vec> bOcc = b.OccupiedCoordinates.ToList();
            if (aOcc.Count() > bOcc.Count())
            {
                List<Vec> cOcc = aOcc;
                aOcc = bOcc;
                bOcc = cOcc;
            }
            HashSet<Vec> aSet = new HashSet<Vec>(aOcc);
            return bOcc.Count(i => aSet.Contains(i));
        }
        #endregion
    }
}
