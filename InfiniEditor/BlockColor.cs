using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InfiniEditor
{
    public partial class BlockColor : UserControl
    {
        public bool Use
        {
            get
            {
                return checkBox.Checked;
            }
            set
            {
                checkBox.Checked = value;
            }
        }

        public Color Color
        {
            get
            {
                return checkBox.BackColor;
            }
            set
            {
                checkBox.BackColor = BackColor = value;
            }
        }
        
        private BlockInfo blockInfo;
        public BlockInfo BlockInfo
        {
            get
            {
                return blockInfo;
            }
            set
            {
                blockInfo = value;
                pictureBox.Image = value.Image;
            }
        }

        public BlockColor()
        {
            InitializeComponent();
        }
        
        private void checkBox_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }
    }
}
