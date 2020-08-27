using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Drawing.Imaging;

namespace InfiniEditor
{
    public partial class FormMain : Form
    {
        public Level OpenedLevel { get; private set; }
        public BlocksCollection ClipboardBlocks { get; private set; }
        public LevelsManager LevelsManager { get; private set; }
        public BlockInfosManager BlockInfosManager { get; private set; }
        FormBlockReference formBlockReference;
        FormAxisHelp formAxisHelp;
        FormColors formImageConverter;
        FormScripting formScripting;
        enum BlocksSource { Clipboard, Puzzle };

        public FormMain()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            comboBoxRotation.SelectedIndex = 0;
            comboBoxFlipping.SelectedIndex = 0;
            comboBoxEnvironment.DataSource = Level.EnvironmentsNames;

            ClipboardNotification.ClipboardUpdate += ClipboardNotification_ClipboardUpdate;
            ClipboardNotification_ClipboardUpdate(null, EventArgs.Empty);

            foreach(Control ctrl in panelAllowedBlocks.Controls)
            {
                ((CheckBox)ctrl).CheckedChanged += new EventHandler(checkBoxBlocks_CheckedChanged);
            }

            LevelsManager = new LevelsManager(LevelsManager.SteamUsers().FirstOrDefault());
            LoadLevels();

            BlockInfosManager = new BlockInfosManager();
            formBlockReference = new FormBlockReference();
            formBlockReference.Owner = this;
            formAxisHelp = new FormAxisHelp();
            formImageConverter = new FormColors();
            formImageConverter.Owner = this;
            formScripting = new FormScripting();
            formScripting.Owner = this;
        }

        private void ClipboardNotification_ClipboardUpdate(object sender, EventArgs e)
        {
            buttonFromClipboardPreview.Enabled = buttonFromClipboardScreenshot.Enabled = Clipboard.ContainsImage() && OpenedLevel != null;
            ClipboardBlocks = BlocksCollection.FromClipboard();
            if (Clipboard.ContainsText() && Clipboard.GetText() != "")
            {
                UpdateBlockCount();
            }
        }

        public void UpdateBlockCount()
        {
            buttonRotateClipboard.Enabled = buttonShiftClipboard.Enabled = buttonReplaceClipboard.Enabled = buttonFlipClipboard.Enabled = ClipboardBlocks.Valid;
            buttonPasteBlocks.Enabled = buttonReplaceWithClipboard.Enabled = ClipboardBlocks.Valid && OpenedLevel != null && OpenedLevel.Source == Level.Sources.Custom;
            labelCount.Text = OpenedLevel == null ? "" : OpenedLevel.Blocks.Count + " (" + (ClipboardBlocks.Valid ? ClipboardBlocks.Count.ToString() : "invalid ") + " in clipboard)";
        }

        private void labelSolved_Click(object sender, EventArgs e)
        {
            if(checkBoxSolved.Enabled)
                checkBoxSolved.Checked = !checkBoxSolved.Checked;
        }

        private void labelSolved_DoubleClick(object sender, EventArgs e)
        {
            if (checkBoxSolved.Enabled)
                checkBoxSolved.Checked = !checkBoxSolved.Checked;
        }
        
        private void panelParameters_Resize(object sender, EventArgs e)
        {
            panelScrollEnabler.Size = panelParameters.Size - new Size(40, 70);
        }

        private void OpenLevel(Level lvl)
        {
            try
            {
                if (OpenedLevel == null || OpenedLevel.Saved || ProceedClosingLevel())
                {
                    OpenedLevel = lvl;
                    buttonCopyBlocks.Enabled = buttonToFilePreview.Enabled = buttonToFileScreenshot.Enabled = buttonToClipboardPreview.Enabled = buttonToClipboardScreenshot.Enabled = true;
                    panelScroll.Visible = true;
                    linkLabelPath.Text = lvl.Path;
                    labelSource.Text = lvl.Source.ToString();
                    linkLabelAuthor.Text = lvl.AuthorName.ToString() == "-" ? "" : lvl.AuthorName.ToString();
                    linkLabelWorkshop.Text = lvl.WorkshopLink;
                    textBoxTitle.Text = lvl.Title;
                    radioButtonClassic.Checked = !lvl.Advanced;
                    radioButtonAdvanced.Checked = lvl.Advanced;
                    checkBoxSolved.Checked = lvl.Solved;
                    foreach (Control control in panelAllowedBlocks.Controls)
                    {
                        CheckBox checkbox = (CheckBox)control;
                        checkbox.Checked = lvl.AllowedBlocks.Contains((int)checkbox.Tag);
                    }
                    labelCount.Text = lvl.Blocks.Count + " (" + ClipboardBlocks.Count + " in clipboard)";
                    comboBoxEnvironment.SelectedIndex = (int)lvl.Environment - 1;
                    numericUpDownInputRate.Value = (decimal)lvl.InputDelay;
                    checkBoxConstantInputRatio.Checked = lvl.ConstantInputRatio;
                    checkBoxInputTops.Checked = lvl.InputTops;
                    checkBoxNoOneBlockTops.Checked = lvl.NoOneBlockTops;
                    pictureBoxPreview.Image = lvl.Preview;
                    pictureBoxScreenshot.Image = lvl.Screenshot;

                    groupBoxAdvancedProperties.Enabled = panelScreenshot.Enabled = lvl.Advanced;
                    List<Control> dis = new List<Control> { textBoxTitle, radioButtonClassic, radioButtonAdvanced, panelAllowedBlocks, buttonDeleteBlocks, buttonRotatePuzzle, buttonShiftPuzzle, buttonFlipPuzzle, buttonReplacePuzzle, comboBoxEnvironment, numericUpDownInputRate, panelAdvancedCheckboxes, buttonFromFilePreview, buttonFromFileScreenshot };
                    bool editable = lvl.Source == Level.Sources.Custom;
                    foreach (Control ctrl in dis)
                    {
                        ctrl.Enabled = editable;
                    }
                    buttonFromClipboardPreview.Enabled = buttonFromClipboardScreenshot.Enabled = editable;
                    buttonPasteBlocks.Enabled = buttonReplaceWithClipboard.Enabled = ClipboardBlocks.Valid && OpenedLevel.Source == Level.Sources.Custom;
                    buttonSaveLevel.Enabled = false;
                    lvl.SaveWithoutSaving();
                    Status(lvl.Title + " opened.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured trying to open " + lvl.Title + "!\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ProceedClosingLevel()
        {
            DialogResult res = MessageBox.Show("Do you want to save changes to " + OpenedLevel.Title + "?", "Unsaved level", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                SaveLevel();
            }
            return res != DialogResult.Cancel;
        }

        private void SaveLevel()
        {
            buttonSaveLevel.Enabled = false;
            if (OpenedLevel.Save())
            {
                Status("{0} was saved successfully.", OpenedLevel.Title);
            }
            else
            {
                Status("There was a problem saving {0}.", OpenedLevel.Title);
            }
        }

        private void backgroundWorkerLoadLevels_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (Level lvl in LevelsManager.GetLevels())
            {
                ListViewItem item = lvl.ListViewItem;
                item.Group = listViewLevels.Groups[(int)lvl.Source];
                backgroundWorkerLoadLevels.ReportProgress(0, item);
            }
        }

        private void backgroundWorkerLoadLevels_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            listViewLevels.Items.Add((ListViewItem)e.UserState);
            statusProgressbar.PerformStep();
            Status("Loading levels {0}/{1} {2}", statusProgressbar.Value, statusProgressbar.Maximum, new string('.', 1 + statusProgressbar.Value % 5));
        }

        private void buttonReloadLevels_Click(object sender, EventArgs e)
        {
            buttonReloadLevels.Enabled = false;
            LevelsManager.ClearLoadedLevels();
            LoadLevels();
        }

        private void LoadLevels()
        {
            listViewLevels.Items.Clear();
            statusProgressbar.Maximum = LevelsManager.CountLevels();
            statusProgressbar.Value = 0;
            backgroundWorkerLoadLevels.RunWorkerAsync();
        }

        private void buttonEditSelectedLevel_Click(object sender, EventArgs e)
        {
            OpenLevel((Level)(listViewLevels.SelectedItems[0].Tag));
        }

        private void linkLabelPath_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("explorer.exe", "/select," + OpenedLevel.Path);
        }

        private void listViewLevels_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (((ListViewNF)sender).HitTest(e.Location).Item != null)
            {
                OpenLevel((Level)(listViewLevels.SelectedItems[0].Tag));
            }
        }

        private void linkLabelWorkshopID_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(OpenedLevel.WorkshopLink);
        }

        private void backgroundWorkerLoadLevels_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonReloadLevels.Enabled = true;
            Status(statusProgressbar.Maximum + " levels loaded.");
            statusProgressbar.Value = 0;
        }

        private void listViewLevels_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.listViewLevels.ListViewItemSorter = new ListViewItemComparer(e.Column);
        }

        #region CHANGE VALUES
        private void buttonsFromFile_Click(object sender, EventArgs e)
        {
            string tag = ((Button)sender).Parent.Tag.ToString();
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(openFileDialog.FileName);
                ImageIn(sender, img);
            }
            SaveButtonUpdate();
        }

        private void buttonsFromClipboard_Click(object sender, EventArgs e)
        {
            Image img = Clipboard.GetImage();
            ImageIn(sender, img);
            SaveButtonUpdate();
        }

        private void ImageIn(object sender, Image img)
        {
            if (img == null)
            {
                return;
            }
            string tag = ((Button)sender).Parent.Tag.ToString();
            Bitmap bmp = (Bitmap)img;
            if (tag == "preview")
            {
                OpenedLevel.Preview = bmp;
                pictureBoxPreview.Image = bmp;
            }
            else
            {
                OpenedLevel.Screenshot = bmp;
                pictureBoxScreenshot.Image = bmp;
            }
        }

        private void buttonsToFile_Click(object sender, EventArgs e)
        {
            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ImageOut(sender).Save(saveFileDialog.FileName, Ext2ImageFormat(Path.GetExtension(saveFileDialog.FileName)));
                }
                catch { }
            }
            SaveButtonUpdate();
        }

        private ImageFormat Ext2ImageFormat(string ext)
        {
            var dict = new Dictionary<string, ImageFormat> { { ".jpg", ImageFormat.Jpeg }, { ".bmp", ImageFormat.Bmp }, { ".png", ImageFormat.Png } };
            return dict[ext];
        }

        private void buttonsToClipboard_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetImage(ImageOut(sender));
            }
            catch { }
            SaveButtonUpdate();
        }

        private Image ImageOut(object sender)
        {
            string tag = ((Button)sender).Parent.Tag.ToString();
            if (tag == "preview")
            {
                return pictureBoxPreview.Image;
            }
            return pictureBoxScreenshot.Image;
        }

        private void textBoxTitle_TextChanged(object sender, EventArgs e)
        {
            OpenedLevel.Title = textBoxTitle.Text;
            SaveButtonUpdate();
        }
        
        private void radioButtonAdvanced_CheckedChanged(object sender, EventArgs e)
        {
            OpenedLevel.Advanced = radioButtonAdvanced.Checked;
            SaveButtonUpdate();
        }

        private void checkBoxBlocks_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            List<int> newAllowed = new List<int>(OpenedLevel.AllowedBlocks);
            if (checkBox.Checked)
            {
                newAllowed.Add((int)checkBox.Tag);
            }
            else
            {
                newAllowed.Remove((int)checkBox.Tag);
            }
            OpenedLevel.AllowedBlocks = (IEnumerable<int>)newAllowed;
            SaveButtonUpdate();
        }

        private void comboBoxEnvironment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OpenedLevel == null)
            {
                return;
            }
            OpenedLevel.Environment = (Level.Environments)(comboBoxEnvironment.SelectedIndex + 1);
            SaveButtonUpdate();
        }

        private void numericUpDownInputRate_ValueChanged(object sender, EventArgs e)
        {
            OpenedLevel.InputDelay = (int)numericUpDownInputRate.Value;
            SaveButtonUpdate();
        }

        public void SaveButtonUpdate(bool overrideStatus = false)
        {
            buttonSaveLevel.Enabled = overrideStatus || !OpenedLevel.Saved;
        }

        private void checkBoxConstantInputRatio_CheckedChanged(object sender, EventArgs e)
        {
            OpenedLevel.ConstantInputRatio = checkBoxConstantInputRatio.Checked;
            SaveButtonUpdate();
        }

        private void checkBoxNoOneBlockTops_CheckedChanged(object sender, EventArgs e)
        {
            OpenedLevel.NoOneBlockTops = checkBoxNoOneBlockTops.Checked;
            SaveButtonUpdate();
        }

        private void checkBoxInputTops_CheckedChanged(object sender, EventArgs e)
        {
            OpenedLevel.InputTops = checkBoxInputTops.Checked;
            SaveButtonUpdate();
        }
        #endregion

        private void linkLabelAuthor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(OpenedLevel.AuthorLink);
        }

        private void buttonSaveLevel_Click(object sender, EventArgs e)
        {
            SaveLevel();
        }

        #region BLOCKS ACTIONS

        public void Status(string status)
        {
            statusLabel.Text = status;
        }
        public void Status(string status, params object[] objects)
        {
            statusLabel.Text = String.Format(status, objects);
        }
        public string Status()
        {
            return statusLabel.Text;
        }

        private void buttonCopyBlocks_Click(object sender, EventArgs e)
        {
            OpenedLevel.Blocks.ToClipboard();
            UpdateBlockCount();
            Status("{0} blocks copied to clipboard.", OpenedLevel.Blocks.Count);
        }

        private void buttonDeleteBlocks_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Do you really wish to delete all blocks in this level?", "Delete all blocks?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes){
                OpenedLevel.Blocks.DeleteAll();
                Status("All blocks from this level were deleted.");
                UpdateBlockCount();
                SaveButtonUpdate(true);
            }
        }

        private void buttonReplaceWithClipboard_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really wish to replace all blocks in this level?", "Replace all blocks?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                OpenedLevel.Blocks.DeleteAll();
                OpenedLevel.Blocks.AddBlocks(ClipboardBlocks);
                Status("All blocks in this level were replaced with those in the clipboard.");
                UpdateBlockCount();
                SaveButtonUpdate(true);
            }
        }

        private void buttonPasteBlocks_Click(object sender, EventArgs e)
        {
            int overlaps = OpenedLevel.Blocks.CountOverlaps(ClipboardBlocks);
            if(overlaps == 0)
            {
                OpenedLevel.Blocks.AddBlocks(ClipboardBlocks);
                Status("{0} blocks from clipboard were copied to the level.", ClipboardBlocks.Count);
                UpdateBlockCount();
                SaveButtonUpdate(true);
            }
            else
            {
                var res = MessageBox.Show(overlaps + " blocks are overlaping, do you want to override blocks from the level with those in the clipboard? You might as well consider cancelling this operation, shifting the blocks in the clipboard so that they wouldn't overlap and paste them then.", "Override blocks?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if(res == DialogResult.Yes)
                {
                    OpenedLevel.Blocks.AddBlocks(ClipboardBlocks, true);
                    Status("{0} blocks were added to the level. {1} of them overrided the previous ones.", ClipboardBlocks.Count, overlaps);
                    UpdateBlockCount();
                    SaveButtonUpdate();
                    SaveButtonUpdate(true);
                }
                else if(res == DialogResult.No)
                {
                    OpenedLevel.Blocks.AddBlocks(ClipboardBlocks);
                    Status("{0} / {1} blocks were added to the level.", ClipboardBlocks.Count - overlaps, ClipboardBlocks.Count);
                    UpdateBlockCount();
                    SaveButtonUpdate();
                    SaveButtonUpdate(true);
                }
            }
        }

        private void buttonRotateBlocks_Click(object sender, EventArgs e)
        {
            BlocksSource src = GetBlocksSource(sender);
            BlocksCollection blocks = GetRightBlocks(src);
            blocks.Rotate(comboBoxRotation.SelectedIndex + 1);
            Status("{0} blocks were rotated by {1}.", src, comboBoxRotation.Text);
            SaveButtonUpdate(src == BlocksSource.Puzzle);
        }

        private void buttonFlipBlocks_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature doesn't really flip the level entirely: while blocks positions will flip, their orientation won't change accordingly.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            BlocksSource src = GetBlocksSource(sender);
            BlocksCollection blocks = GetRightBlocks(src);
            blocks.Flip((Axis)comboBoxFlipping.SelectedIndex, BlockInfosManager);
            Status("{0} blocks were flipped along the {1}.", src, comboBoxFlipping.Text);
            SaveButtonUpdate(src == BlocksSource.Puzzle);
        }

        private void buttonShiftBlocks_Click(object sender, EventArgs e)
        {
            BlocksSource src = GetBlocksSource(sender);
            BlocksCollection blocks = GetRightBlocks(src);
            Vec vec = new Vec((int)numericUpDownXShift.Value, (int)numericUpDownYShift.Value, (int)numericUpDownZShift.Value);
            blocks.Shift(vec);
            Status("{0} blocks were shifted by {1} vector.", src, vec);
            SaveButtonUpdate(src == BlocksSource.Puzzle);
        }

        private void buttonReplaceBlocks_Click(object sender, EventArgs e)
        {
            BlocksSource src = GetBlocksSource(sender);
            BlocksCollection blocks = GetRightBlocks(src);
            int what = (int)numericUpDownReplaceWhat.Value;
            int with = (int)numericUpDownReplaceWith.Value;
            string msg = "There is no block " + with + " in the Block Reference. Do you want to continue with the replacement?";
            if (BlockInfosManager.BlockInfosList.Any(i => !i.Decal && i.Type == with) || MessageBox.Show(msg, "Replace blocks?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                blocks.Replace(what, with);
                Status("{0} blocks {1} were replaced with blocks {2}.", src, what, with);
                SaveButtonUpdate(src == BlocksSource.Puzzle);
            }
        }

        private BlocksSource GetBlocksSource(object ctrl)
        {
            return ((Control)ctrl).Text.Contains("puzzle") ? BlocksSource.Puzzle : BlocksSource.Clipboard;
        }
        private BlocksCollection GetRightBlocks(BlocksSource src)
        {
            return src == BlocksSource.Puzzle ? OpenedLevel.Blocks : ClipboardBlocks;
        }

        #endregion

        private void buttonAxisReference_Click(object sender, EventArgs e)
        {
            formAxisHelp.Show();            
        }

        private void buttonBlockReference_Click(object sender, EventArgs e)
        {
            formBlockReference.Show();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && OpenedLevel != null && !OpenedLevel.Saved && !ProceedClosingLevel())
            {
                e.Cancel = true;
            }
        }

        private void buttonImageConverter_Click(object sender, EventArgs e)
        {
            formImageConverter.Show();
        }

        private void buttonCode_Click(object sender, EventArgs e)
        {
            formScripting.Show();
        }
    }
}
