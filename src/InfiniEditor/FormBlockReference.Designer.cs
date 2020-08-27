namespace InfiniEditor
{
    partial class FormBlockReference
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBlockReference));
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.panelBlockProperties = new System.Windows.Forms.Panel();
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.labelType = new System.Windows.Forms.Label();
            this.labelGroup = new System.Windows.Forms.Label();
            this.labelBlockName = new System.Windows.Forms.Label();
            this.flowLayoutPanelGroupBoxes = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBoxFlags = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanelFlags = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBoxSymmetries = new System.Windows.Forms.GroupBox();
            this.labelSymmetries = new System.Windows.Forms.Label();
            this.groupBoxBoundingBox = new System.Windows.Forms.GroupBox();
            this.labelBoundingBox = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxSimilarBlocks = new System.Windows.Forms.GroupBox();
            this.listViewNFSimilarBlocks = new InfiniEditor.ListViewNF();
            this.columnHeaderSimName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSimGroup = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listViewNFAllBlocks = new InfiniEditor.ListViewNF();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLongID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSymmetries = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderBoundingBox = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderFlags = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonClearFilter = new System.Windows.Forms.Button();
            this.panelBlockProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            this.flowLayoutPanelGroupBoxes.SuspendLayout();
            this.groupBoxFlags.SuspendLayout();
            this.groupBoxSymmetries.SuspendLayout();
            this.groupBoxBoundingBox.SuspendLayout();
            this.groupBoxSimilarBlocks.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // panelBlockProperties
            // 
            this.panelBlockProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBlockProperties.AutoScroll = true;
            this.panelBlockProperties.Controls.Add(this.pictureBoxImage);
            this.panelBlockProperties.Controls.Add(this.labelType);
            this.panelBlockProperties.Controls.Add(this.labelGroup);
            this.panelBlockProperties.Controls.Add(this.labelBlockName);
            this.panelBlockProperties.Controls.Add(this.flowLayoutPanelGroupBoxes);
            this.panelBlockProperties.Location = new System.Drawing.Point(826, 13);
            this.panelBlockProperties.Name = "panelBlockProperties";
            this.panelBlockProperties.Size = new System.Drawing.Size(401, 493);
            this.panelBlockProperties.TabIndex = 24;
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxImage.Location = new System.Drawing.Point(0, 91);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new System.Drawing.Size(383, 400);
            this.pictureBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxImage.TabIndex = 1;
            this.pictureBoxImage.TabStop = false;
            // 
            // labelType
            // 
            this.labelType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelType.Location = new System.Drawing.Point(0, 62);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(384, 23);
            this.labelType.TabIndex = 0;
            this.labelType.Text = "Block 58";
            this.labelType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelGroup
            // 
            this.labelGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGroup.Location = new System.Drawing.Point(0, 39);
            this.labelGroup.Name = "labelGroup";
            this.labelGroup.Size = new System.Drawing.Size(384, 23);
            this.labelGroup.TabIndex = 0;
            this.labelGroup.Text = "Environment - Atropos station";
            this.labelGroup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelBlockName
            // 
            this.labelBlockName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBlockName.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBlockName.Location = new System.Drawing.Point(0, 0);
            this.labelBlockName.Name = "labelBlockName";
            this.labelBlockName.Size = new System.Drawing.Size(384, 39);
            this.labelBlockName.TabIndex = 0;
            this.labelBlockName.Text = "Hangar Door, Teeth";
            this.labelBlockName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanelGroupBoxes
            // 
            this.flowLayoutPanelGroupBoxes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelGroupBoxes.AutoSize = true;
            this.flowLayoutPanelGroupBoxes.Controls.Add(this.groupBoxFlags);
            this.flowLayoutPanelGroupBoxes.Controls.Add(this.groupBoxSymmetries);
            this.flowLayoutPanelGroupBoxes.Controls.Add(this.groupBoxBoundingBox);
            this.flowLayoutPanelGroupBoxes.Controls.Add(this.groupBoxSimilarBlocks);
            this.flowLayoutPanelGroupBoxes.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelGroupBoxes.Location = new System.Drawing.Point(2, 491);
            this.flowLayoutPanelGroupBoxes.Name = "flowLayoutPanelGroupBoxes";
            this.flowLayoutPanelGroupBoxes.Size = new System.Drawing.Size(399, 283);
            this.flowLayoutPanelGroupBoxes.TabIndex = 3;
            // 
            // groupBoxFlags
            // 
            this.groupBoxFlags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFlags.AutoSize = true;
            this.groupBoxFlags.Controls.Add(this.flowLayoutPanelFlags);
            this.groupBoxFlags.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxFlags.Location = new System.Drawing.Point(3, 3);
            this.groupBoxFlags.MaximumSize = new System.Drawing.Size(399, 0);
            this.groupBoxFlags.MinimumSize = new System.Drawing.Size(378, 0);
            this.groupBoxFlags.Name = "groupBoxFlags";
            this.groupBoxFlags.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.groupBoxFlags.Size = new System.Drawing.Size(393, 46);
            this.groupBoxFlags.TabIndex = 2;
            this.groupBoxFlags.TabStop = false;
            this.groupBoxFlags.Text = "Flags";
            // 
            // flowLayoutPanelFlags
            // 
            this.flowLayoutPanelFlags.AutoSize = true;
            this.flowLayoutPanelFlags.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flowLayoutPanelFlags.Location = new System.Drawing.Point(7, 20);
            this.flowLayoutPanelFlags.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.flowLayoutPanelFlags.MaximumSize = new System.Drawing.Size(380, 0);
            this.flowLayoutPanelFlags.MinimumSize = new System.Drawing.Size(10, 10);
            this.flowLayoutPanelFlags.Name = "flowLayoutPanelFlags";
            this.flowLayoutPanelFlags.Size = new System.Drawing.Size(380, 10);
            this.flowLayoutPanelFlags.TabIndex = 0;
            // 
            // groupBoxSymmetries
            // 
            this.groupBoxSymmetries.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSymmetries.Controls.Add(this.labelSymmetries);
            this.groupBoxSymmetries.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxSymmetries.Location = new System.Drawing.Point(3, 55);
            this.groupBoxSymmetries.Name = "groupBoxSymmetries";
            this.groupBoxSymmetries.Size = new System.Drawing.Size(393, 42);
            this.groupBoxSymmetries.TabIndex = 3;
            this.groupBoxSymmetries.TabStop = false;
            this.groupBoxSymmetries.Text = "Symmetries";
            // 
            // labelSymmetries
            // 
            this.labelSymmetries.AutoSize = true;
            this.labelSymmetries.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSymmetries.Location = new System.Drawing.Point(7, 20);
            this.labelSymmetries.MinimumSize = new System.Drawing.Size(20, 2);
            this.labelSymmetries.Name = "labelSymmetries";
            this.labelSymmetries.Size = new System.Drawing.Size(20, 13);
            this.labelSymmetries.TabIndex = 0;
            // 
            // groupBoxBoundingBox
            // 
            this.groupBoxBoundingBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxBoundingBox.Controls.Add(this.labelBoundingBox);
            this.groupBoxBoundingBox.Controls.Add(this.label1);
            this.groupBoxBoundingBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxBoundingBox.Location = new System.Drawing.Point(3, 103);
            this.groupBoxBoundingBox.Name = "groupBoxBoundingBox";
            this.groupBoxBoundingBox.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.groupBoxBoundingBox.Size = new System.Drawing.Size(393, 42);
            this.groupBoxBoundingBox.TabIndex = 3;
            this.groupBoxBoundingBox.TabStop = false;
            this.groupBoxBoundingBox.Text = "Bounding box";
            // 
            // labelBoundingBox
            // 
            this.labelBoundingBox.AutoSize = true;
            this.labelBoundingBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBoundingBox.Location = new System.Drawing.Point(7, 20);
            this.labelBoundingBox.MinimumSize = new System.Drawing.Size(20, 2);
            this.labelBoundingBox.Name = "labelBoundingBox";
            this.labelBoundingBox.Size = new System.Drawing.Size(20, 13);
            this.labelBoundingBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.MinimumSize = new System.Drawing.Size(20, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 17);
            this.label1.TabIndex = 0;
            // 
            // groupBoxSimilarBlocks
            // 
            this.groupBoxSimilarBlocks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSimilarBlocks.AutoSize = true;
            this.groupBoxSimilarBlocks.Controls.Add(this.listViewNFSimilarBlocks);
            this.groupBoxSimilarBlocks.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxSimilarBlocks.Location = new System.Drawing.Point(3, 151);
            this.groupBoxSimilarBlocks.MaximumSize = new System.Drawing.Size(393, 10000);
            this.groupBoxSimilarBlocks.Name = "groupBoxSimilarBlocks";
            this.groupBoxSimilarBlocks.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.groupBoxSimilarBlocks.Size = new System.Drawing.Size(393, 116);
            this.groupBoxSimilarBlocks.TabIndex = 3;
            this.groupBoxSimilarBlocks.TabStop = false;
            this.groupBoxSimilarBlocks.Text = "Similar blocks";
            // 
            // listViewNFSimilarBlocks
            // 
            this.listViewNFSimilarBlocks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(237)))), ((int)(((byte)(231)))));
            this.listViewNFSimilarBlocks.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewNFSimilarBlocks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderSimName,
            this.columnHeaderSimGroup});
            this.listViewNFSimilarBlocks.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewNFSimilarBlocks.Location = new System.Drawing.Point(2, 14);
            this.listViewNFSimilarBlocks.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.listViewNFSimilarBlocks.MultiSelect = false;
            this.listViewNFSimilarBlocks.Name = "listViewNFSimilarBlocks";
            this.listViewNFSimilarBlocks.Size = new System.Drawing.Size(390, 86);
            this.listViewNFSimilarBlocks.TabIndex = 0;
            this.listViewNFSimilarBlocks.TileSize = new System.Drawing.Size(185, 60);
            this.listViewNFSimilarBlocks.UseCompatibleStateImageBehavior = false;
            this.listViewNFSimilarBlocks.View = System.Windows.Forms.View.Tile;
            this.listViewNFSimilarBlocks.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewNFSimilarBlocks_MouseClick);
            // 
            // columnHeaderSimName
            // 
            this.columnHeaderSimName.Text = "Name";
            // 
            // columnHeaderSimGroup
            // 
            this.columnHeaderSimGroup.Text = "Group";
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFilter.Location = new System.Drawing.Point(691, 5);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(113, 20);
            this.textBoxFilter.TabIndex = 25;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(588, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "Filter flags / search";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // listViewNFAllBlocks
            // 
            this.listViewNFAllBlocks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewNFAllBlocks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderLongID,
            this.columnHeaderSymmetries,
            this.columnHeaderBoundingBox,
            this.columnHeaderFlags});
            this.listViewNFAllBlocks.Location = new System.Drawing.Point(6, 30);
            this.listViewNFAllBlocks.MultiSelect = false;
            this.listViewNFAllBlocks.Name = "listViewNFAllBlocks";
            this.listViewNFAllBlocks.Size = new System.Drawing.Size(818, 476);
            this.listViewNFAllBlocks.TabIndex = 0;
            this.listViewNFAllBlocks.TileSize = new System.Drawing.Size(245, 100);
            this.listViewNFAllBlocks.UseCompatibleStateImageBehavior = false;
            this.listViewNFAllBlocks.View = System.Windows.Forms.View.Tile;
            this.listViewNFAllBlocks.SelectedIndexChanged += new System.EventHandler(this.listViewNF_SelectedIndexChanged);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            // 
            // columnHeaderLongID
            // 
            this.columnHeaderLongID.Text = "Long ID";
            // 
            // columnHeaderSymmetries
            // 
            this.columnHeaderSymmetries.DisplayIndex = 3;
            this.columnHeaderSymmetries.Text = "Symmetries";
            // 
            // columnHeaderBoundingBox
            // 
            this.columnHeaderBoundingBox.DisplayIndex = 4;
            this.columnHeaderBoundingBox.Text = "BoundingBox";
            // 
            // columnHeaderFlags
            // 
            this.columnHeaderFlags.DisplayIndex = 2;
            this.columnHeaderFlags.Text = "Flags";
            // 
            // buttonClearFilter
            // 
            this.buttonClearFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearFilter.Location = new System.Drawing.Point(807, 4);
            this.buttonClearFilter.Name = "buttonClearFilter";
            this.buttonClearFilter.Size = new System.Drawing.Size(17, 21);
            this.buttonClearFilter.TabIndex = 27;
            this.buttonClearFilter.Text = "×";
            this.buttonClearFilter.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonClearFilter.UseVisualStyleBackColor = true;
            this.buttonClearFilter.Click += new System.EventHandler(this.buttonClearFilter_Click);
            // 
            // FormBlockReference
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(237)))), ((int)(((byte)(231)))));
            this.ClientSize = new System.Drawing.Size(1231, 513);
            this.Controls.Add(this.buttonClearFilter);
            this.Controls.Add(this.listViewNFAllBlocks);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxFilter);
            this.Controls.Add(this.panelBlockProperties);
            this.Icon = InfiniEditor.Properties.Resources.icon;
            this.Name = "FormBlockReference";
            this.Text = "Block reference";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBlockReference_FormClosing);
            this.Load += new System.EventHandler(this.FormBlockReference_Load);
            this.Resize += new System.EventHandler(this.FormBlockReference_Resize);
            this.panelBlockProperties.ResumeLayout(false);
            this.panelBlockProperties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            this.flowLayoutPanelGroupBoxes.ResumeLayout(false);
            this.flowLayoutPanelGroupBoxes.PerformLayout();
            this.groupBoxFlags.ResumeLayout(false);
            this.groupBoxFlags.PerformLayout();
            this.groupBoxSymmetries.ResumeLayout(false);
            this.groupBoxSymmetries.PerformLayout();
            this.groupBoxBoundingBox.ResumeLayout(false);
            this.groupBoxBoundingBox.PerformLayout();
            this.groupBoxSimilarBlocks.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListViewNF listViewNFAllBlocks;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderLongID;
        private System.Windows.Forms.ColumnHeader columnHeaderFlags;
        private System.Windows.Forms.ColumnHeader columnHeaderSymmetries;
        private System.Windows.Forms.ColumnHeader columnHeaderBoundingBox;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Panel panelBlockProperties;
        private System.Windows.Forms.Label labelBlockName;
        private System.Windows.Forms.Label labelGroup;
        private System.Windows.Forms.PictureBox pictureBoxImage;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.GroupBox groupBoxFlags;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelFlags;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelGroupBoxes;
        private System.Windows.Forms.GroupBox groupBoxSymmetries;
        private System.Windows.Forms.Label labelSymmetries;
        private System.Windows.Forms.GroupBox groupBoxBoundingBox;
        private System.Windows.Forms.Label labelBoundingBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonClearFilter;
        private System.Windows.Forms.GroupBox groupBoxSimilarBlocks;
        private ListViewNF listViewNFSimilarBlocks;
        private System.Windows.Forms.ColumnHeader columnHeaderSimName;
        private System.Windows.Forms.ColumnHeader columnHeaderSimGroup;
    }
}