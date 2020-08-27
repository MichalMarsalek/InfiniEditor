using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InfiniEditor
{
    public partial class FormBlockReference : Form
    {
        public FormBlockReference()
        {
            InitializeComponent();
        }

        BlockInfosManager bim;
        List<ListViewItem> lvis;

        private void FormBlockReference_Load(object sender, EventArgs e)
        {
            bim = ((FormMain)Owner).BlockInfosManager;
            lvis = new List<ListViewItem>();
            backgroundWorker.RunWorkerAsync();
            ResetViewer();
        }

        private void buttonResetViewer_Click(object sender, EventArgs e)
        {
            ResetViewer();
        }

        private void ResetViewer()
        {
            listViewNFAllBlocks.Groups.Clear();
            lvis.Clear();
            foreach (string group in bim.Groups)
            {
                listViewNFAllBlocks.Groups.Add(group, group);
            }
            foreach (BlockInfo block in bim.BlockInfosList)
            {
                ListViewItem item = block.ListViewItem;
                item.Group = listViewNFAllBlocks.Groups[block.Group];
                lvis.Add(item);
            }
            Filter("");
            if (listViewNFAllBlocks.Items.Count > 0)
            {
                listViewNFAllBlocks.Items[0].Selected = true;
            }
        }

        private void FormBlockReference_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ImageList images = new ImageList();
            images.ImageSize = new Size(100, 100);
            images.ColorDepth = ColorDepth.Depth32Bit;
            foreach (BlockInfo block in bim.BlockInfosList)
            {
                Image img = block.Image;
                images.Images.Add(block.BlockID, img);
            }
            e.Result = images;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            listViewNFAllBlocks.LargeImageList = (ImageList)e.Result;
            foreach (ListViewItem lvi in listViewNFAllBlocks.Items)
            {
                lvi.ImageKey = lvi.ImageKey + "";
            }
        }

        private void listViewNF_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewNFAllBlocks.SelectedItems.Count > 0)
            {
                OpenBlock((BlockInfo)listViewNFAllBlocks.SelectedItems[0].Tag);
            }
        }

        private void OpenBlock(BlockInfo block)
        {
            labelBlockName.Text = block.Name;
            labelGroup.Text = block.Group;
            pictureBoxImage.Image = block.Image;
            labelType.Text = block.LongID;
            flowLayoutPanelFlags.Controls.Clear();
            foreach(string flag in block.AllFlags)
            {
                LinkLabel llFlag = new LinkLabel();
                llFlag.Text = flag;
                llFlag.AutoSize = true;
                llFlag.Click += llFlag_Click;
                flowLayoutPanelFlags.Controls.Add(llFlag);
            }
            labelSymmetries.Text = block.SymmetriesToString(true);
            labelBoundingBox.Text = block.BoundingBox + " (" + block.BoundingBox.SizeToString() + ")";

            listViewNFSimilarBlocks.Items.Clear();
            ImageList images = new ImageList();
            images.ImageSize = new Size(60, 60);
            images.ColorDepth = ColorDepth.Depth32Bit;
            foreach (BlockInfo similar in bim.SimilarTo(block).Take(10))
            {
                listViewNFSimilarBlocks.Items.Add(similar.ListViewItem2);
                Image img = similar.Image;
                images.Images.Add(similar.BlockID, img);
            }
            listViewNFSimilarBlocks.LargeImageList = images;
            foreach(ListViewItem lvi in listViewNFSimilarBlocks.Items)
            {
                lvi.ImageKey = ((BlockInfo)lvi.Tag).BlockID;
            }
            listViewNFSimilarBlocks.Height = 5 + listViewNFSimilarBlocks.Items.Count / 2 * listViewNFSimilarBlocks.TileSize.Height;
        }

        private void LlSimilar_Click(object sender, EventArgs e)
        {
            ((BlockInfo)((Label)sender).Tag).ListViewItem.Selected = true;
            listViewNFAllBlocks.Select();
        }

        private void llFlag_Click(object sender, EventArgs e)
        {
            textBoxFilter.Text = ((Label)sender).Text;
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            Filter(textBoxFilter.Text);
        }

        private void Filter(string filter)
        {
            listViewNFAllBlocks.Items.Clear();
            foreach(ListViewItem lvi in lvis)
            {
                lvi.Group = listViewNFAllBlocks.Groups[((BlockInfo)lvi.Tag).Group];
            }
            listViewNFAllBlocks.Items.AddRange(lvis.Where(i => ((BlockInfo)i.Tag).FlagsCondition(filter)).ToArray());
        }

        private void buttonClearFilter_Click(object sender, EventArgs e)
        {
            textBoxFilter.Text = "";
        }

        private void FormBlockReference_Resize(object sender, EventArgs e)
        {
            panelBlockProperties.Height = listViewNFAllBlocks.Height + 17;
        }

        private void listViewNFSimilarBlocks_MouseClick(object sender, MouseEventArgs e)
        {
            if (((ListViewNF)sender).HitTest(e.Location).Item != null)
            {
                ((BlockInfo)listViewNFSimilarBlocks.SelectedItems[0].Tag).ListViewItem.Selected = true;
            }
        }
    }
}
