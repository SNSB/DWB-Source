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
            groupBoxDisplayOrderPart = new System.Windows.Forms.GroupBox();
            tableLayoutPanelDisplayOrderPart = new System.Windows.Forms.TableLayoutPanel();
            labelUnitsNotInPart = new System.Windows.Forms.Label();
            listBoxUnitsNotInPart = new System.Windows.Forms.ListBox();
            toolStripUnitInPart = new System.Windows.Forms.ToolStrip();
            toolStripButtonUnitRemoveFromPart = new System.Windows.Forms.ToolStripButton();
            toolStripButtonUnitMoveInPart = new System.Windows.Forms.ToolStripButton();
            toolStripButtonUnitMoveInPartAll = new System.Windows.Forms.ToolStripButton();
            toolStripButtonUnitRemoveFromPartAll = new System.Windows.Forms.ToolStripButton();
            listBoxPartHide = new System.Windows.Forms.ListBox();
            listBoxPartShowInLabel = new System.Windows.Forms.ListBox();
            labelPartShowInLabel = new System.Windows.Forms.Label();
            toolStripPartDisplayOrderUpDown = new System.Windows.Forms.ToolStrip();
            toolStripButtonPartLabelMoveUp = new System.Windows.Forms.ToolStripButton();
            toolStripButtonPartLabelMoveDown = new System.Windows.Forms.ToolStripButton();
            toolStripDropDownButtonPartLabelSort = new System.Windows.Forms.ToolStripDropDownButton();
            toolStripMenuItemPartLabelSortByName = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItemPartLabelSortByIdentifier = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItemPartLabelSortByID = new System.Windows.Forms.ToolStripMenuItem();
            labelPartHide = new System.Windows.Forms.Label();
            toolStripUnitInPartHide = new System.Windows.Forms.ToolStrip();
            toolStripButtonShowUnitInPartLabel = new System.Windows.Forms.ToolStripButton();
            toolStripButtonHideUnitFromPartLabel = new System.Windows.Forms.ToolStripButton();
            groupBoxDisplayOrderPart.SuspendLayout();
            tableLayoutPanelDisplayOrderPart.SuspendLayout();
            toolStripUnitInPart.SuspendLayout();
            toolStripPartDisplayOrderUpDown.SuspendLayout();
            toolStripUnitInPartHide.SuspendLayout();
            SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            imageListDataWithholding.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListDataWithholding.ImageStream");
            imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // groupBoxDisplayOrderPart
            // 
            groupBoxDisplayOrderPart.AccessibleName = "IdentificationUnitInPart.DisplayOrder";
            groupBoxDisplayOrderPart.Controls.Add(tableLayoutPanelDisplayOrderPart);
            groupBoxDisplayOrderPart.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxDisplayOrderPart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            groupBoxDisplayOrderPart.ForeColor = System.Drawing.Color.Black;
            groupBoxDisplayOrderPart.Location = new System.Drawing.Point(0, 0);
            groupBoxDisplayOrderPart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxDisplayOrderPart.MinimumSize = new System.Drawing.Size(0, 87);
            groupBoxDisplayOrderPart.Name = "groupBoxDisplayOrderPart";
            groupBoxDisplayOrderPart.Padding = new System.Windows.Forms.Padding(4, 0, 4, 3);
            groupBoxDisplayOrderPart.Size = new System.Drawing.Size(889, 231);
            groupBoxDisplayOrderPart.TabIndex = 2;
            groupBoxDisplayOrderPart.TabStop = false;
            groupBoxDisplayOrderPart.Text = "Display order of parts";
            // 
            // tableLayoutPanelDisplayOrderPart
            // 
            tableLayoutPanelDisplayOrderPart.ColumnCount = 5;
            tableLayoutPanelDisplayOrderPart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tableLayoutPanelDisplayOrderPart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelDisplayOrderPart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tableLayoutPanelDisplayOrderPart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            tableLayoutPanelDisplayOrderPart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tableLayoutPanelDisplayOrderPart.Controls.Add(labelUnitsNotInPart, 0, 0);
            tableLayoutPanelDisplayOrderPart.Controls.Add(listBoxUnitsNotInPart, 0, 1);
            tableLayoutPanelDisplayOrderPart.Controls.Add(toolStripUnitInPart, 1, 1);
            tableLayoutPanelDisplayOrderPart.Controls.Add(listBoxPartHide, 4, 1);
            tableLayoutPanelDisplayOrderPart.Controls.Add(listBoxPartShowInLabel, 2, 1);
            tableLayoutPanelDisplayOrderPart.Controls.Add(labelPartShowInLabel, 2, 0);
            tableLayoutPanelDisplayOrderPart.Controls.Add(toolStripPartDisplayOrderUpDown, 2, 2);
            tableLayoutPanelDisplayOrderPart.Controls.Add(labelPartHide, 4, 0);
            tableLayoutPanelDisplayOrderPart.Controls.Add(toolStripUnitInPartHide, 3, 1);
            tableLayoutPanelDisplayOrderPart.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelDisplayOrderPart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            tableLayoutPanelDisplayOrderPart.Location = new System.Drawing.Point(4, 13);
            tableLayoutPanelDisplayOrderPart.Margin = new System.Windows.Forms.Padding(4, 0, 4, 3);
            tableLayoutPanelDisplayOrderPart.Name = "tableLayoutPanelDisplayOrderPart";
            tableLayoutPanelDisplayOrderPart.RowCount = 3;
            tableLayoutPanelDisplayOrderPart.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelDisplayOrderPart.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelDisplayOrderPart.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelDisplayOrderPart.Size = new System.Drawing.Size(881, 215);
            tableLayoutPanelDisplayOrderPart.TabIndex = 0;
            // 
            // labelUnitsNotInPart
            // 
            labelUnitsNotInPart.Dock = System.Windows.Forms.DockStyle.Fill;
            labelUnitsNotInPart.Location = new System.Drawing.Point(4, 0);
            labelUnitsNotInPart.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelUnitsNotInPart.Name = "labelUnitsNotInPart";
            labelUnitsNotInPart.Size = new System.Drawing.Size(268, 18);
            labelUnitsNotInPart.TabIndex = 11;
            labelUnitsNotInPart.Text = "Units not in part:";
            labelUnitsNotInPart.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // listBoxUnitsNotInPart
            // 
            listBoxUnitsNotInPart.DisplayMember = "DisplayText";
            listBoxUnitsNotInPart.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxUnitsNotInPart.IntegralHeight = false;
            listBoxUnitsNotInPart.ItemHeight = 13;
            listBoxUnitsNotInPart.Location = new System.Drawing.Point(0, 18);
            listBoxUnitsNotInPart.Margin = new System.Windows.Forms.Padding(0);
            listBoxUnitsNotInPart.Name = "listBoxUnitsNotInPart";
            tableLayoutPanelDisplayOrderPart.SetRowSpan(listBoxUnitsNotInPart, 2);
            listBoxUnitsNotInPart.Size = new System.Drawing.Size(276, 197);
            listBoxUnitsNotInPart.TabIndex = 10;
            // 
            // toolStripUnitInPart
            // 
            toolStripUnitInPart.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonUnitRemoveFromPart, toolStripButtonUnitMoveInPart, toolStripButtonUnitMoveInPartAll, toolStripButtonUnitRemoveFromPartAll });
            toolStripUnitInPart.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            toolStripUnitInPart.Location = new System.Drawing.Point(276, 18);
            toolStripUnitInPart.Name = "toolStripUnitInPart";
            tableLayoutPanelDisplayOrderPart.SetRowSpan(toolStripUnitInPart, 2);
            toolStripUnitInPart.Size = new System.Drawing.Size(23, 111);
            toolStripUnitInPart.TabIndex = 9;
            toolStripUnitInPart.Text = "toolStrip1";
            // 
            // toolStripButtonUnitRemoveFromPart
            // 
            toolStripButtonUnitRemoveFromPart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonUnitRemoveFromPart.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            toolStripButtonUnitRemoveFromPart.Image = Resource.ArrowPrevious;
            toolStripButtonUnitRemoveFromPart.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonUnitRemoveFromPart.Name = "toolStripButtonUnitRemoveFromPart";
            toolStripButtonUnitRemoveFromPart.Size = new System.Drawing.Size(23, 20);
            toolStripButtonUnitRemoveFromPart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            toolStripButtonUnitRemoveFromPart.ToolTipText = "The selected unit is not present in the specimen part";
            toolStripButtonUnitRemoveFromPart.Click += toolStripButtonUnitRemoveFromPart_Click;
            // 
            // toolStripButtonUnitMoveInPart
            // 
            toolStripButtonUnitMoveInPart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonUnitMoveInPart.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            toolStripButtonUnitMoveInPart.Image = Resource.ArrowNext;
            toolStripButtonUnitMoveInPart.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonUnitMoveInPart.Name = "toolStripButtonUnitMoveInPart";
            toolStripButtonUnitMoveInPart.Size = new System.Drawing.Size(23, 20);
            toolStripButtonUnitMoveInPart.Text = ">";
            toolStripButtonUnitMoveInPart.ToolTipText = "The selected unit is present in the specimen part";
            toolStripButtonUnitMoveInPart.Click += toolStripButtonUnitMoveInPart_Click;
            // 
            // toolStripButtonUnitMoveInPartAll
            // 
            toolStripButtonUnitMoveInPartAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonUnitMoveInPartAll.Font = new System.Drawing.Font("Segoe UI", 12F);
            toolStripButtonUnitMoveInPartAll.Image = Resource.ArrowNextNextSmall;
            toolStripButtonUnitMoveInPartAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonUnitMoveInPartAll.Name = "toolStripButtonUnitMoveInPartAll";
            toolStripButtonUnitMoveInPartAll.Size = new System.Drawing.Size(23, 20);
            toolStripButtonUnitMoveInPartAll.Text = "»";
            toolStripButtonUnitMoveInPartAll.ToolTipText = "All units are present in the part";
            toolStripButtonUnitMoveInPartAll.Click += toolStripButtonUnitMoveInPartAll_Click;
            // 
            // toolStripButtonUnitRemoveFromPartAll
            // 
            toolStripButtonUnitRemoveFromPartAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonUnitRemoveFromPartAll.Image = Resource.ArrowLeftLeftBlack;
            toolStripButtonUnitRemoveFromPartAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonUnitRemoveFromPartAll.Name = "toolStripButtonUnitRemoveFromPartAll";
            toolStripButtonUnitRemoveFromPartAll.Size = new System.Drawing.Size(23, 20);
            toolStripButtonUnitRemoveFromPartAll.Text = "<<";
            toolStripButtonUnitRemoveFromPartAll.ToolTipText = "Remove all units from the part";
            toolStripButtonUnitRemoveFromPartAll.Click += toolStripButtonUnitRemoveFromPartAll_Click;
            // 
            // listBoxPartHide
            // 
            listBoxPartHide.DisplayMember = "DisplayText";
            listBoxPartHide.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxPartHide.IntegralHeight = false;
            listBoxPartHide.ItemHeight = 13;
            listBoxPartHide.Location = new System.Drawing.Point(603, 18);
            listBoxPartHide.Margin = new System.Windows.Forms.Padding(0);
            listBoxPartHide.Name = "listBoxPartHide";
            tableLayoutPanelDisplayOrderPart.SetRowSpan(listBoxPartHide, 2);
            listBoxPartHide.Size = new System.Drawing.Size(278, 197);
            listBoxPartHide.TabIndex = 7;
            // 
            // listBoxPartShowInLabel
            // 
            listBoxPartShowInLabel.DisplayMember = "DisplayText";
            listBoxPartShowInLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxPartShowInLabel.IntegralHeight = false;
            listBoxPartShowInLabel.ItemHeight = 13;
            listBoxPartShowInLabel.Location = new System.Drawing.Point(299, 18);
            listBoxPartShowInLabel.Margin = new System.Windows.Forms.Padding(0);
            listBoxPartShowInLabel.Name = "listBoxPartShowInLabel";
            listBoxPartShowInLabel.Size = new System.Drawing.Size(276, 172);
            listBoxPartShowInLabel.TabIndex = 3;
            // 
            // labelPartShowInLabel
            // 
            labelPartShowInLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            labelPartShowInLabel.Location = new System.Drawing.Point(303, 0);
            labelPartShowInLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelPartShowInLabel.Name = "labelPartShowInLabel";
            labelPartShowInLabel.Size = new System.Drawing.Size(268, 18);
            labelPartShowInLabel.TabIndex = 1;
            labelPartShowInLabel.Text = "Show in label:";
            labelPartShowInLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // toolStripPartDisplayOrderUpDown
            // 
            toolStripPartDisplayOrderUpDown.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripPartDisplayOrderUpDown.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonPartLabelMoveUp, toolStripButtonPartLabelMoveDown, toolStripDropDownButtonPartLabelSort });
            toolStripPartDisplayOrderUpDown.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            toolStripPartDisplayOrderUpDown.Location = new System.Drawing.Point(311, 190);
            toolStripPartDisplayOrderUpDown.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            toolStripPartDisplayOrderUpDown.Name = "toolStripPartDisplayOrderUpDown";
            toolStripPartDisplayOrderUpDown.Size = new System.Drawing.Size(252, 25);
            toolStripPartDisplayOrderUpDown.TabIndex = 0;
            toolStripPartDisplayOrderUpDown.Text = "toolStrip1";
            // 
            // toolStripButtonPartLabelMoveUp
            // 
            toolStripButtonPartLabelMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonPartLabelMoveUp.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonPartLabelMoveUp.Image");
            toolStripButtonPartLabelMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonPartLabelMoveUp.Name = "toolStripButtonPartLabelMoveUp";
            toolStripButtonPartLabelMoveUp.Size = new System.Drawing.Size(23, 22);
            toolStripButtonPartLabelMoveUp.Text = "move selected unit to a higher display order";
            toolStripButtonPartLabelMoveUp.Click += toolStripButtonPartLabelMoveUp_Click;
            // 
            // toolStripButtonPartLabelMoveDown
            // 
            toolStripButtonPartLabelMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonPartLabelMoveDown.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonPartLabelMoveDown.Image");
            toolStripButtonPartLabelMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonPartLabelMoveDown.Name = "toolStripButtonPartLabelMoveDown";
            toolStripButtonPartLabelMoveDown.Size = new System.Drawing.Size(23, 22);
            toolStripButtonPartLabelMoveDown.Text = "move selected unit to a lower display order";
            toolStripButtonPartLabelMoveDown.Click += toolStripButtonPartLabelMoveDown_Click;
            // 
            // toolStripDropDownButtonPartLabelSort
            // 
            toolStripDropDownButtonPartLabelSort.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripDropDownButtonPartLabelSort.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripDropDownButtonPartLabelSort.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItemPartLabelSortByName, toolStripMenuItemPartLabelSortByIdentifier, toolStripMenuItemPartLabelSortByID });
            toolStripDropDownButtonPartLabelSort.Image = Resource.Sort;
            toolStripDropDownButtonPartLabelSort.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButtonPartLabelSort.Name = "toolStripDropDownButtonPartLabelSort";
            toolStripDropDownButtonPartLabelSort.Size = new System.Drawing.Size(29, 22);
            toolStripDropDownButtonPartLabelSort.Text = "Sort parts according to ...";
            // 
            // toolStripMenuItemPartLabelSortByName
            // 
            toolStripMenuItemPartLabelSortByName.Name = "toolStripMenuItemPartLabelSortByName";
            toolStripMenuItemPartLabelSortByName.Size = new System.Drawing.Size(121, 22);
            toolStripMenuItemPartLabelSortByName.Text = "Name";
            toolStripMenuItemPartLabelSortByName.ToolTipText = "Sort by last identification";
            toolStripMenuItemPartLabelSortByName.Click += toolStripMenuItemPartLabelSortByName_Click;
            // 
            // toolStripMenuItemPartLabelSortByIdentifier
            // 
            toolStripMenuItemPartLabelSortByIdentifier.Name = "toolStripMenuItemPartLabelSortByIdentifier";
            toolStripMenuItemPartLabelSortByIdentifier.Size = new System.Drawing.Size(121, 22);
            toolStripMenuItemPartLabelSortByIdentifier.Text = "Identifier";
            toolStripMenuItemPartLabelSortByIdentifier.ToolTipText = "Sort by identifier";
            toolStripMenuItemPartLabelSortByIdentifier.Click += toolStripMenuItemPartLabelSortByIdentifier_Click;
            // 
            // toolStripMenuItemPartLabelSortByID
            // 
            toolStripMenuItemPartLabelSortByID.Name = "toolStripMenuItemPartLabelSortByID";
            toolStripMenuItemPartLabelSortByID.Size = new System.Drawing.Size(121, 22);
            toolStripMenuItemPartLabelSortByID.Text = "ID";
            toolStripMenuItemPartLabelSortByID.ToolTipText = "Sort by ID (column IdentificationUnitID)";
            toolStripMenuItemPartLabelSortByID.Click += toolStripMenuItemPartLabelSortByID_Click;
            // 
            // labelPartHide
            // 
            labelPartHide.Dock = System.Windows.Forms.DockStyle.Fill;
            labelPartHide.Location = new System.Drawing.Point(607, 0);
            labelPartHide.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelPartHide.Name = "labelPartHide";
            labelPartHide.Size = new System.Drawing.Size(270, 18);
            labelPartHide.TabIndex = 6;
            labelPartHide.Text = "Hide:";
            labelPartHide.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // toolStripUnitInPartHide
            // 
            toolStripUnitInPartHide.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonShowUnitInPartLabel, toolStripButtonHideUnitFromPartLabel });
            toolStripUnitInPartHide.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            toolStripUnitInPartHide.Location = new System.Drawing.Point(575, 18);
            toolStripUnitInPartHide.Name = "toolStripUnitInPartHide";
            toolStripUnitInPartHide.Size = new System.Drawing.Size(28, 46);
            toolStripUnitInPartHide.TabIndex = 8;
            toolStripUnitInPartHide.Text = "toolStrip1";
            // 
            // toolStripButtonShowUnitInPartLabel
            // 
            toolStripButtonShowUnitInPartLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonShowUnitInPartLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            toolStripButtonShowUnitInPartLabel.Image = Resource.ArrowPrevious;
            toolStripButtonShowUnitInPartLabel.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonShowUnitInPartLabel.Name = "toolStripButtonShowUnitInPartLabel";
            toolStripButtonShowUnitInPartLabel.Size = new System.Drawing.Size(23, 20);
            toolStripButtonShowUnitInPartLabel.Text = "<";
            toolStripButtonShowUnitInPartLabel.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            toolStripButtonShowUnitInPartLabel.ToolTipText = "show the selected unit in the label";
            toolStripButtonShowUnitInPartLabel.Click += toolStripButtonShowUnitInPartLabel_Click;
            // 
            // toolStripButtonHideUnitFromPartLabel
            // 
            toolStripButtonHideUnitFromPartLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonHideUnitFromPartLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            toolStripButtonHideUnitFromPartLabel.Image = Resource.ArrowNext;
            toolStripButtonHideUnitFromPartLabel.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonHideUnitFromPartLabel.Name = "toolStripButtonHideUnitFromPartLabel";
            toolStripButtonHideUnitFromPartLabel.Size = new System.Drawing.Size(23, 20);
            toolStripButtonHideUnitFromPartLabel.Text = ">";
            toolStripButtonHideUnitFromPartLabel.ToolTipText = "Hide the selected unit";
            toolStripButtonHideUnitFromPartLabel.Click += toolStripButtonHideUnitFromPartLabel_Click;
            // 
            // UserControl_DisplayOrder
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(groupBoxDisplayOrderPart);
            Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            Name = "UserControl_DisplayOrder";
            Size = new System.Drawing.Size(889, 231);
            groupBoxDisplayOrderPart.ResumeLayout(false);
            tableLayoutPanelDisplayOrderPart.ResumeLayout(false);
            tableLayoutPanelDisplayOrderPart.PerformLayout();
            toolStripUnitInPart.ResumeLayout(false);
            toolStripUnitInPart.PerformLayout();
            toolStripPartDisplayOrderUpDown.ResumeLayout(false);
            toolStripPartDisplayOrderUpDown.PerformLayout();
            toolStripUnitInPartHide.ResumeLayout(false);
            toolStripUnitInPartHide.PerformLayout();
            ResumeLayout(false);

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
        private System.Windows.Forms.ToolStripButton toolStripButtonUnitRemoveFromPartAll;
    }
}
