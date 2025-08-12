namespace DiversityCollection.UserControls
{
    partial class UserControl_DisplayOrder
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_DisplayOrder));
            this.groupBoxDisplayOrderPart = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelDisplayOrderPart = new System.Windows.Forms.TableLayoutPanel();
            this.labelUnitsNotInPart = new System.Windows.Forms.Label();
            this.listBoxUnitsNotInPart = new System.Windows.Forms.ListBox();
            this.toolStripUnitInPart = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonUnitRemoveFromPart = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonUnitMoveInPart = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonUnitMoveInPartAll = new System.Windows.Forms.ToolStripButton();
            this.listBoxPartHide = new System.Windows.Forms.ListBox();
            this.listBoxPartShowInLabel = new System.Windows.Forms.ListBox();
            this.labelPartShowInLabel = new System.Windows.Forms.Label();
            this.toolStripPartDisplayOrderUpDown = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonPartLabelMoveUp = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPartLabelMoveDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButtonPartLabelSort = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItemPartLabelSortByName = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPartLabelSortByIdentifier = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPartLabelSortByID = new System.Windows.Forms.ToolStripMenuItem();
            this.labelPartHide = new System.Windows.Forms.Label();
            this.toolStripUnitInPartHide = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonShowUnitInPartLabel = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonHideUnitFromPartLabel = new System.Windows.Forms.ToolStripButton();
            this.groupBoxDisplayOrderPart.SuspendLayout();
            this.tableLayoutPanelDisplayOrderPart.SuspendLayout();
            this.toolStripUnitInPart.SuspendLayout();
            this.toolStripPartDisplayOrderUpDown.SuspendLayout();
            this.toolStripUnitInPartHide.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // groupBoxDisplayOrderPart
            // 
            this.groupBoxDisplayOrderPart.AccessibleName = "IdentificationUnitInPart.DisplayOrder";
            this.groupBoxDisplayOrderPart.Controls.Add(this.tableLayoutPanelDisplayOrderPart);
            this.groupBoxDisplayOrderPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDisplayOrderPart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxDisplayOrderPart.ForeColor = System.Drawing.Color.Black;
            this.groupBoxDisplayOrderPart.Location = new System.Drawing.Point(0, 0);
            this.groupBoxDisplayOrderPart.MinimumSize = new System.Drawing.Size(0, 75);
            this.groupBoxDisplayOrderPart.Name = "groupBoxDisplayOrderPart";
            this.groupBoxDisplayOrderPart.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.groupBoxDisplayOrderPart.Size = new System.Drawing.Size(762, 200);
            this.groupBoxDisplayOrderPart.TabIndex = 2;
            this.groupBoxDisplayOrderPart.TabStop = false;
            this.groupBoxDisplayOrderPart.Text = "Display order of parts";
            // 
            // tableLayoutPanelDisplayOrderPart
            // 
            this.tableLayoutPanelDisplayOrderPart.ColumnCount = 5;
            this.tableLayoutPanelDisplayOrderPart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelDisplayOrderPart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelDisplayOrderPart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelDisplayOrderPart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelDisplayOrderPart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelDisplayOrderPart.Controls.Add(this.labelUnitsNotInPart, 0, 0);
            this.tableLayoutPanelDisplayOrderPart.Controls.Add(this.listBoxUnitsNotInPart, 0, 1);
            this.tableLayoutPanelDisplayOrderPart.Controls.Add(this.toolStripUnitInPart, 1, 1);
            this.tableLayoutPanelDisplayOrderPart.Controls.Add(this.listBoxPartHide, 4, 1);
            this.tableLayoutPanelDisplayOrderPart.Controls.Add(this.listBoxPartShowInLabel, 2, 1);
            this.tableLayoutPanelDisplayOrderPart.Controls.Add(this.labelPartShowInLabel, 2, 0);
            this.tableLayoutPanelDisplayOrderPart.Controls.Add(this.toolStripPartDisplayOrderUpDown, 2, 2);
            this.tableLayoutPanelDisplayOrderPart.Controls.Add(this.labelPartHide, 4, 0);
            this.tableLayoutPanelDisplayOrderPart.Controls.Add(this.toolStripUnitInPartHide, 3, 1);
            this.tableLayoutPanelDisplayOrderPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDisplayOrderPart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelDisplayOrderPart.Location = new System.Drawing.Point(3, 13);
            this.tableLayoutPanelDisplayOrderPart.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.tableLayoutPanelDisplayOrderPart.Name = "tableLayoutPanelDisplayOrderPart";
            this.tableLayoutPanelDisplayOrderPart.RowCount = 3;
            this.tableLayoutPanelDisplayOrderPart.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDisplayOrderPart.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDisplayOrderPart.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDisplayOrderPart.Size = new System.Drawing.Size(756, 184);
            this.tableLayoutPanelDisplayOrderPart.TabIndex = 0;
            // 
            // labelUnitsNotInPart
            // 
            this.labelUnitsNotInPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUnitsNotInPart.Location = new System.Drawing.Point(3, 0);
            this.labelUnitsNotInPart.Name = "labelUnitsNotInPart";
            this.labelUnitsNotInPart.Size = new System.Drawing.Size(231, 16);
            this.labelUnitsNotInPart.TabIndex = 11;
            this.labelUnitsNotInPart.Text = "Units not in part:";
            this.labelUnitsNotInPart.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // listBoxUnitsNotInPart
            // 
            this.listBoxUnitsNotInPart.DisplayMember = "DisplayText";
            this.listBoxUnitsNotInPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxUnitsNotInPart.IntegralHeight = false;
            this.listBoxUnitsNotInPart.Location = new System.Drawing.Point(0, 16);
            this.listBoxUnitsNotInPart.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxUnitsNotInPart.Name = "listBoxUnitsNotInPart";
            this.tableLayoutPanelDisplayOrderPart.SetRowSpan(this.listBoxUnitsNotInPart, 2);
            this.listBoxUnitsNotInPart.Size = new System.Drawing.Size(237, 168);
            this.listBoxUnitsNotInPart.TabIndex = 10;
            // 
            // toolStripUnitInPart
            // 
            this.toolStripUnitInPart.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonUnitRemoveFromPart,
            this.toolStripButtonUnitMoveInPart,
            this.toolStripButtonUnitMoveInPartAll});
            this.toolStripUnitInPart.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.toolStripUnitInPart.Location = new System.Drawing.Point(237, 16);
            this.toolStripUnitInPart.Name = "toolStripUnitInPart";
            this.tableLayoutPanelDisplayOrderPart.SetRowSpan(this.toolStripUnitInPart, 2);
            this.toolStripUnitInPart.Size = new System.Drawing.Size(20, 69);
            this.toolStripUnitInPart.TabIndex = 9;
            this.toolStripUnitInPart.Text = "toolStrip1";
            // 
            // toolStripButtonUnitRemoveFromPart
            // 
            this.toolStripButtonUnitRemoveFromPart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonUnitRemoveFromPart.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButtonUnitRemoveFromPart.Image = global::DiversityCollection.Resource.ArrowPrevious;
            this.toolStripButtonUnitRemoveFromPart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUnitRemoveFromPart.Name = "toolStripButtonUnitRemoveFromPart";
            this.toolStripButtonUnitRemoveFromPart.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonUnitRemoveFromPart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonUnitRemoveFromPart.ToolTipText = "The selected unit is not present in the specimen part";
            this.toolStripButtonUnitRemoveFromPart.Click += new System.EventHandler(this.toolStripButtonUnitRemoveFromPart_Click);
            // 
            // toolStripButtonUnitMoveInPart
            // 
            this.toolStripButtonUnitMoveInPart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonUnitMoveInPart.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButtonUnitMoveInPart.Image = global::DiversityCollection.Resource.ArrowNext;
            this.toolStripButtonUnitMoveInPart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUnitMoveInPart.Name = "toolStripButtonUnitMoveInPart";
            this.toolStripButtonUnitMoveInPart.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonUnitMoveInPart.Text = ">";
            this.toolStripButtonUnitMoveInPart.ToolTipText = "The selected unit is present in the specimen part";
            this.toolStripButtonUnitMoveInPart.Click += new System.EventHandler(this.toolStripButtonUnitMoveInPart_Click);
            // 
            // toolStripButtonUnitMoveInPartAll
            // 
            this.toolStripButtonUnitMoveInPartAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonUnitMoveInPartAll.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.toolStripButtonUnitMoveInPartAll.Image = global::DiversityCollection.Resource.ArrowNextNextSmall;
            this.toolStripButtonUnitMoveInPartAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUnitMoveInPartAll.Name = "toolStripButtonUnitMoveInPartAll";
            this.toolStripButtonUnitMoveInPartAll.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonUnitMoveInPartAll.Text = "»";
            this.toolStripButtonUnitMoveInPartAll.ToolTipText = "All units are present in the part";
            this.toolStripButtonUnitMoveInPartAll.Click += new System.EventHandler(this.toolStripButtonUnitMoveInPartAll_Click);
            // 
            // listBoxPartHide
            // 
            this.listBoxPartHide.DisplayMember = "DisplayText";
            this.listBoxPartHide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxPartHide.IntegralHeight = false;
            this.listBoxPartHide.Location = new System.Drawing.Point(518, 16);
            this.listBoxPartHide.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxPartHide.Name = "listBoxPartHide";
            this.tableLayoutPanelDisplayOrderPart.SetRowSpan(this.listBoxPartHide, 2);
            this.listBoxPartHide.Size = new System.Drawing.Size(238, 168);
            this.listBoxPartHide.TabIndex = 7;
            // 
            // listBoxPartShowInLabel
            // 
            this.listBoxPartShowInLabel.DisplayMember = "DisplayText";
            this.listBoxPartShowInLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxPartShowInLabel.IntegralHeight = false;
            this.listBoxPartShowInLabel.Location = new System.Drawing.Point(257, 16);
            this.listBoxPartShowInLabel.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxPartShowInLabel.Name = "listBoxPartShowInLabel";
            this.listBoxPartShowInLabel.Size = new System.Drawing.Size(237, 143);
            this.listBoxPartShowInLabel.TabIndex = 3;
            // 
            // labelPartShowInLabel
            // 
            this.labelPartShowInLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPartShowInLabel.Location = new System.Drawing.Point(260, 0);
            this.labelPartShowInLabel.Name = "labelPartShowInLabel";
            this.labelPartShowInLabel.Size = new System.Drawing.Size(231, 16);
            this.labelPartShowInLabel.TabIndex = 1;
            this.labelPartShowInLabel.Text = "Show in label:";
            this.labelPartShowInLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // toolStripPartDisplayOrderUpDown
            // 
            this.toolStripPartDisplayOrderUpDown.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripPartDisplayOrderUpDown.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonPartLabelMoveUp,
            this.toolStripButtonPartLabelMoveDown,
            this.toolStripDropDownButtonPartLabelSort});
            this.toolStripPartDisplayOrderUpDown.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStripPartDisplayOrderUpDown.Location = new System.Drawing.Point(267, 159);
            this.toolStripPartDisplayOrderUpDown.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.toolStripPartDisplayOrderUpDown.Name = "toolStripPartDisplayOrderUpDown";
            this.toolStripPartDisplayOrderUpDown.Size = new System.Drawing.Size(217, 25);
            this.toolStripPartDisplayOrderUpDown.TabIndex = 0;
            this.toolStripPartDisplayOrderUpDown.Text = "toolStrip1";
            // 
            // toolStripButtonPartLabelMoveUp
            // 
            this.toolStripButtonPartLabelMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPartLabelMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPartLabelMoveUp.Image")));
            this.toolStripButtonPartLabelMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPartLabelMoveUp.Name = "toolStripButtonPartLabelMoveUp";
            this.toolStripButtonPartLabelMoveUp.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPartLabelMoveUp.Text = "move selected unit to a higher display order";
            this.toolStripButtonPartLabelMoveUp.Click += new System.EventHandler(this.toolStripButtonPartLabelMoveUp_Click);
            // 
            // toolStripButtonPartLabelMoveDown
            // 
            this.toolStripButtonPartLabelMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPartLabelMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPartLabelMoveDown.Image")));
            this.toolStripButtonPartLabelMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPartLabelMoveDown.Name = "toolStripButtonPartLabelMoveDown";
            this.toolStripButtonPartLabelMoveDown.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPartLabelMoveDown.Text = "move selected unit to a lower display order";
            this.toolStripButtonPartLabelMoveDown.Click += new System.EventHandler(this.toolStripButtonPartLabelMoveDown_Click);
            // 
            // toolStripDropDownButtonPartLabelSort
            // 
            this.toolStripDropDownButtonPartLabelSort.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripDropDownButtonPartLabelSort.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonPartLabelSort.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemPartLabelSortByName,
            this.toolStripMenuItemPartLabelSortByIdentifier,
            this.toolStripMenuItemPartLabelSortByID});
            this.toolStripDropDownButtonPartLabelSort.Image = global::DiversityCollection.Resource.Sort;
            this.toolStripDropDownButtonPartLabelSort.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonPartLabelSort.Name = "toolStripDropDownButtonPartLabelSort";
            this.toolStripDropDownButtonPartLabelSort.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButtonPartLabelSort.Text = "Sort parts according to ...";
            // 
            // toolStripMenuItemPartLabelSortByName
            // 
            this.toolStripMenuItemPartLabelSortByName.Name = "toolStripMenuItemPartLabelSortByName";
            this.toolStripMenuItemPartLabelSortByName.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItemPartLabelSortByName.Text = "Name";
            this.toolStripMenuItemPartLabelSortByName.ToolTipText = "Sort by last identification";
            this.toolStripMenuItemPartLabelSortByName.Click += new System.EventHandler(this.toolStripMenuItemPartLabelSortByName_Click);
            // 
            // toolStripMenuItemPartLabelSortByIdentifier
            // 
            this.toolStripMenuItemPartLabelSortByIdentifier.Name = "toolStripMenuItemPartLabelSortByIdentifier";
            this.toolStripMenuItemPartLabelSortByIdentifier.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItemPartLabelSortByIdentifier.Text = "Identifier";
            this.toolStripMenuItemPartLabelSortByIdentifier.ToolTipText = "Sort by identifier";
            this.toolStripMenuItemPartLabelSortByIdentifier.Click += new System.EventHandler(this.toolStripMenuItemPartLabelSortByIdentifier_Click);
            // 
            // toolStripMenuItemPartLabelSortByID
            // 
            this.toolStripMenuItemPartLabelSortByID.Name = "toolStripMenuItemPartLabelSortByID";
            this.toolStripMenuItemPartLabelSortByID.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItemPartLabelSortByID.Text = "ID";
            this.toolStripMenuItemPartLabelSortByID.ToolTipText = "Sort by ID (column IdentificationUnitID)";
            this.toolStripMenuItemPartLabelSortByID.Click += new System.EventHandler(this.toolStripMenuItemPartLabelSortByID_Click);
            // 
            // labelPartHide
            // 
            this.labelPartHide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPartHide.Location = new System.Drawing.Point(521, 0);
            this.labelPartHide.Name = "labelPartHide";
            this.labelPartHide.Size = new System.Drawing.Size(232, 16);
            this.labelPartHide.TabIndex = 6;
            this.labelPartHide.Text = "Hide:";
            this.labelPartHide.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // toolStripUnitInPartHide
            // 
            this.toolStripUnitInPartHide.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonShowUnitInPartLabel,
            this.toolStripButtonHideUnitFromPartLabel});
            this.toolStripUnitInPartHide.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.toolStripUnitInPartHide.Location = new System.Drawing.Point(494, 16);
            this.toolStripUnitInPartHide.Name = "toolStripUnitInPartHide";
            this.toolStripUnitInPartHide.Size = new System.Drawing.Size(24, 46);
            this.toolStripUnitInPartHide.TabIndex = 8;
            this.toolStripUnitInPartHide.Text = "toolStrip1";
            // 
            // toolStripButtonShowUnitInPartLabel
            // 
            this.toolStripButtonShowUnitInPartLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonShowUnitInPartLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButtonShowUnitInPartLabel.Image = global::DiversityCollection.Resource.ArrowPrevious;
            this.toolStripButtonShowUnitInPartLabel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowUnitInPartLabel.Name = "toolStripButtonShowUnitInPartLabel";
            this.toolStripButtonShowUnitInPartLabel.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonShowUnitInPartLabel.Text = "<";
            this.toolStripButtonShowUnitInPartLabel.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.toolStripButtonShowUnitInPartLabel.ToolTipText = "show the selected unit in the label";
            this.toolStripButtonShowUnitInPartLabel.Click += new System.EventHandler(this.toolStripButtonShowUnitInPartLabel_Click);
            // 
            // toolStripButtonHideUnitFromPartLabel
            // 
            this.toolStripButtonHideUnitFromPartLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHideUnitFromPartLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButtonHideUnitFromPartLabel.Image = global::DiversityCollection.Resource.ArrowNext;
            this.toolStripButtonHideUnitFromPartLabel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHideUnitFromPartLabel.Name = "toolStripButtonHideUnitFromPartLabel";
            this.toolStripButtonHideUnitFromPartLabel.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonHideUnitFromPartLabel.Text = ">";
            this.toolStripButtonHideUnitFromPartLabel.ToolTipText = "Hide the selected unit";
            this.toolStripButtonHideUnitFromPartLabel.Click += new System.EventHandler(this.toolStripButtonHideUnitFromPartLabel_Click);
            // 
            // UserControl_DisplayOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxDisplayOrderPart);
            this.Name = "UserControl_DisplayOrder";
            this.Size = new System.Drawing.Size(762, 200);
            this.groupBoxDisplayOrderPart.ResumeLayout(false);
            this.tableLayoutPanelDisplayOrderPart.ResumeLayout(false);
            this.tableLayoutPanelDisplayOrderPart.PerformLayout();
            this.toolStripUnitInPart.ResumeLayout(false);
            this.toolStripUnitInPart.PerformLayout();
            this.toolStripPartDisplayOrderUpDown.ResumeLayout(false);
            this.toolStripPartDisplayOrderUpDown.PerformLayout();
            this.toolStripUnitInPartHide.ResumeLayout(false);
            this.toolStripUnitInPartHide.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxDisplayOrderPart;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDisplayOrderPart;
        private System.Windows.Forms.Label labelUnitsNotInPart;
        private System.Windows.Forms.ListBox listBoxUnitsNotInPart;
        private System.Windows.Forms.ToolStrip toolStripUnitInPart;
        private System.Windows.Forms.ToolStripButton toolStripButtonUnitRemoveFromPart;
        private System.Windows.Forms.ToolStripButton toolStripButtonUnitMoveInPart;
        private System.Windows.Forms.ToolStripButton toolStripButtonUnitMoveInPartAll;
        private System.Windows.Forms.ListBox listBoxPartHide;
        private System.Windows.Forms.ListBox listBoxPartShowInLabel;
        private System.Windows.Forms.Label labelPartShowInLabel;
        private System.Windows.Forms.ToolStrip toolStripPartDisplayOrderUpDown;
        private System.Windows.Forms.ToolStripButton toolStripButtonPartLabelMoveUp;
        private System.Windows.Forms.ToolStripButton toolStripButtonPartLabelMoveDown;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonPartLabelSort;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPartLabelSortByName;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPartLabelSortByIdentifier;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPartLabelSortByID;
        private System.Windows.Forms.Label labelPartHide;
        private System.Windows.Forms.ToolStrip toolStripUnitInPartHide;
        private System.Windows.Forms.ToolStripButton toolStripButtonShowUnitInPartLabel;
        private System.Windows.Forms.ToolStripButton toolStripButtonHideUnitFromPartLabel;
    }
}
