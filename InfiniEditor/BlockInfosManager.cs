using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NLua;

namespace InfiniEditor
{
    public class BlockInfosManager
    {
        public List<BlockInfo> BlockInfosList { get; private set; }
        public List<string> Groups { get; private set; }
        public HashSet<string> AllFlags { get; private set; }

        public BlockInfosManager()
        {
            Groups = new List<string>();
            BlockInfosList = new List<BlockInfo>();
            AllFlags = new HashSet<string>();
            try
            {
                XDocument xml = XDocument.Load(@"blocks\BlockInfos.xml");

                foreach (XElement group in xml.Root.Elements("Group"))
                {
                    string groupName = (string)group.Attribute("Name");
                    Groups.Add(groupName);
                    foreach (XElement block in group.Elements())
                    {
                        BlockInfo blockInfo = new BlockInfo(block, groupName);
                        AllFlags.UnionWith(blockInfo.AllFlags);
                        BlockInfosList.Add(blockInfo);
                    }
                }
            }
            catch { }
        }

        public IEnumerable<BlockInfo> SimilarTo(BlockInfo block)
        {
            return BlockInfosList.Where(i => block != i).OrderByDescending(i => i.SameObviousFlags(block) * 100 + i.SameOtherFlags(block) * 10 + (i.Group == block.Group  ? 1 : 0));
        }

        public BlockInfo BlockInfo(int type)
        {
            return BlockInfosList.FirstOrDefault(i => i.Type == type && !i.Decal);
        }

        public BlockInfo DecalInfo(int type)
        {
            return BlockInfosList.FirstOrDefault(i => i.Type == type && i.Decal);
        }

        public Dictionary<int,BlockInfo> BlockInfos(string cond)
        {
            return BlockInfosList.Where(i => !i.Decal).Where(i => i.FlagsCondition(cond)).ToDictionary(i => i.Type, i => i);
        }
        public Dictionary<int, BlockInfo> BlockInfos()
        {
            return BlockInfos("");
        }

        public Dictionary<int, BlockInfo> DecalInfos(string cond)
        {
            return BlockInfosList.Where(i => i.Decal).Where(i => i.FlagsCondition(cond)).ToDictionary(i => i.Type, i => i);
        }
        public Dictionary<int, BlockInfo> DecalInfos()
        {
            return DecalInfos("");
        }

    }
}
