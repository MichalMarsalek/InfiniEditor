using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using AForge.Imaging.ColorReduction;

namespace InfiniEditor
{
    public partial class FormColors : Form
    {
        BlockInfosManager bim;
        bool registerSizeChanges = false;
        Color[] colorPalette;
        Dictionary<Color, BlockInfo> colorTranslation;

        public FormColors()
        {
            InitializeComponent();
        }

        private void FormColors_Load(object sender, EventArgs e)
        {
            bim = ((FormMain)Owner).BlockInfosManager;
            IEnumerable<XElement> colors = XDocument.Load(@"blocks\BlockColors.xml").Root.Elements();
            colorTranslation = new Dictionary<Color, BlockInfo>();
            foreach(XElement color in colors)
            {
                BlockColor bc = new BlockColor();
                bc.BlockInfo = bim.BlockInfosList.First(i => !i.Decal && i.Type == (int)color.Attribute("Type"));
                bc.Color = ColorTranslator.FromHtml((string)color.Attribute("Color"));
                bc.Click += Bc_Click;
                flowLayoutPanel.Controls.Add(bc);
                colorTranslation.Add(bc.Color, bc.BlockInfo);
            }
            Bc_Click(null, EventArgs.Empty);
        }

        private void Bc_Click(object sender, EventArgs e)
        {
            colorPalette = flowLayoutPanel.Controls.Cast<BlockColor>().Where(i => i.Use).Select(i => i.Color).ToArray();
            ReduceImage();
        }

        private void FormColors_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void numericUpDownWidth_ValueChanged(object sender, EventArgs e)
        {
            if (registerSizeChanges)
            {
                try
                {
                    registerSizeChanges = false;
                    decimal perc = numericUpDownWidth.Value / pictureBoxOriginal.Image.Width;
                    numericUpDownHeight.Value = pictureBoxOriginal.Image.Height * perc;
                    numericUpDownPercentage.Value = perc * 100;
                    registerSizeChanges = true;
                    ReduceImage();
                }
                catch
                {
                    registerSizeChanges = true;
                }
            }
        }

        private void numericUpDownHeight_ValueChanged(object sender, EventArgs e)
        {
            if (registerSizeChanges)
            {
                try
                {
                    registerSizeChanges = false;
                    decimal perc = numericUpDownHeight.Value / pictureBoxOriginal.Image.Height;
                    numericUpDownWidth.Value = pictureBoxOriginal.Image.Width * perc;
                    numericUpDownPercentage.Value = perc * 100;
                    registerSizeChanges = true;
                    ReduceImage();
                }
                catch
                {
                    registerSizeChanges = true;
                }
            }
        }

        private void numericUpDownPercentage_ValueChanged(object sender, EventArgs e)
        {
            if (registerSizeChanges)
            {
                try
                {
                    registerSizeChanges = false;
                    decimal perc = numericUpDownPercentage.Value / 100;
                    if (pictureBoxOriginal.Image.Width > pictureBoxOriginal.Image.Height)
                    {
                        numericUpDownWidth.Value = pictureBoxOriginal.Image.Width * perc;
                        numericUpDownHeight.Value = pictureBoxOriginal.Image.Height * perc;
                    }
                    else
                    {
                        numericUpDownHeight.Value = pictureBoxOriginal.Image.Height * perc;
                        numericUpDownWidth.Value = pictureBoxOriginal.Image.Width * perc;
                    }
                    registerSizeChanges = true;
                    ReduceImage();
                }
                catch
                {
                    registerSizeChanges = true;
                }
            }
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBoxOriginal.Image = Image.FromFile(openFileDialog.FileName);
                registerSizeChanges = true;
                numericUpDownPercentage.Value = Math.Min(15000M / (decimal)Math.Max(pictureBoxOriginal.Image.Width, pictureBoxOriginal.Image.Height), 100M);
                ReduceImage();
            }
        }

        private void ReduceImage()
        {
            if (pictureBoxOriginal.Image != null)
            {
                Image img = new Bitmap(pictureBoxOriginal.Image, new Size((int)numericUpDownWidth.Value, (int)numericUpDownHeight.Value));
                ColorImageQuantizer ciq = new ColorImageQuantizer(new MedianCutQuantizer());
                img = ciq.ReduceColors((Bitmap)img, colorPalette);
                HashSet<Color> colorsUsed = new HashSet<Color>();
                for (int y = 0; y < img.Height; y++)
                {
                    for (int x = 0; x < img.Width; x++)
                    {
                        colorsUsed.Add(((Bitmap)img).GetPixel(x, y));
                    }
                }
                labelColorsCount.Text = String.Format("Using {0}/{1}/{2} colors", colorsUsed.Count(), colorPalette.Count(), 100);
                pictureBoxReduced.Image = img;
                pictureBoxReduced.Refresh();
            }
        }

        private void buttonConvertToClipboard_Click(object sender, EventArgs e)
        {
            BlocksCollection blocks = new BlocksCollection();
            for (int y = 0; y < pictureBoxReduced.Image.Height; y++)
            {
                for (int x = 0; x < pictureBoxReduced.Image.Width; x++)
                {
                    Vec pos = new Vec(-pictureBoxReduced.Image.Width / 2 + x, 100 - y, 180);
                    blocks[pos] = new Block(
                                             colorTranslation[((Bitmap)pictureBoxReduced.Image).GetPixel(x, y)].Type,
                                             Direction.NegZ
                                             );
                }
            }
            blocks.ToClipboard();
        }
    }
}
