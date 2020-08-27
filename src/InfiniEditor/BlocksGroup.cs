using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InfiniEditor
{
    public class BlocksGroup
    {
        public List<Block> Blocks { get; private set; }
        public int Group { get; private set; }
        public int Version { get; set; }
        public Block.Roles Role { get; private set; }
        public int Count { get
            {
                return Blocks.Count();
            }
        }
        private static readonly int CurrentVersion = 3;

        public BlocksGroup(Block.Roles role, int group, int version = -1, List<Block> blocks = null)
        {
            Role = role;
            Group = group;
            Version = version == -1 ? CurrentVersion : version;
            Blocks = blocks == null ? new List<Block>() : blocks;
        }

        public BlocksGroup(BlocksGroup old)
        {
            Blocks = old.Blocks.ConvertAll(i => new Block(i));
            Version = old.Version;
            Role = old.Role;
            Group = old.Group;
        }

        public string ToBase64()
        {
            if (!Blocks.Any())
            {
                return "";
            }
            MemoryStream stream = new MemoryStream();
            using (BinaryWriter streamWriter = new BinaryWriter(stream))
            {
                streamWriter.Write(Version);
                int count = Blocks.Count();
                streamWriter.Write(count);
                foreach (Block block in Blocks)
                {
                    block.SaveToStream(streamWriter);
                }
            }
            return Convert.ToBase64String(stream.ToArray());
        }

        public XElement ToXElement()
        {
            string elName = new string[] { "World", "Input", "Output" }[(int)Role];
            XElement ret = new XElement(elName,
                        new XAttribute("Version", Version.ToString()),
                        new XAttribute("Group", Group.ToString()),
                        from block in Blocks select
                            block.ToXElement()
                    );
            if (ret.IsEmpty)
            {
                ret.Value = string.Empty;
            }
            return ret;
        }
    }
}
