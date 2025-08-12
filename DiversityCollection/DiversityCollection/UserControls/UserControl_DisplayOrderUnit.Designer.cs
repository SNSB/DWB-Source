namespace DiversityCollection.UserControls
{
    partial class UserControl_DisplayOrderUnit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_DisplayOrderUnit));
            this.groupBoxDisplayOrder = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelDisplayOrder = new System.Windows.Forms.TableLayoutPanel();
            this.listBoxDisplayOrderListHide = new System.Windows.Forms.ListBox();
            this.listBoxDisplayOrderListShow = new System.Windows.Forms.ListBox();
            this.labelDisplayOrderListShow = new System.Windows.Forms.Label();
            this.toolStripDisplayOrder = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonDisplayOrderUp = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDisplayOrderDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButtonDisplayOrderSort = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItemDisplayOrderSortByName = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDisplayOrderSortByIdentifier = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDisplayOrderSortByID = new System.Windows.Forms.ToolStripMenuItem();
            this.labelDisplayOrderListHide = new System.Windows.Forms.Label();
            this.toolStripDisplay = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonDisplayShow = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDisplayHide = new System.Windows.Forms.ToolStripButton();
            this.groupBoxDisplayOrder.SuspendLayout();
            this.tableLayoutPanelDisplayOrder.SuspendLayout();
            this.toolStripDisplayOrder.SuspendLayout();
            this.toolStripDisplay.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // groupBoxDisplayOrder
            // 
            this.groupBoxDisplayOrder.AccessibleName = "IdentificationUnit.DisplayOrder";
            this.groupBoxDisplayOrder.Controls.Add(this.tableLayoutPanelDisplayOrder);
            this.groupBoxDisplayOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDisplayOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxDisplayOrder.ForeColor = System.Drawing.Color.Black;
            this.groupBoxDisplayOrder.Location = new System.Drawing.Point(0, 0);
            this.groupBoxDisplayOrder.MinimumSize = new System.Drawing.Size(0, 75);
            this.groupBoxDisplayOrder.Name = "groupBoxDisplayOrder";
            this.groupBoxDisplayOrder.Size = new System.Drawing.Size(618, 260);
            this.groupBoxDisplayOrder.TabIndex = 1;
            this.groupBoxDisplayOrder.TabStop = false;
            this.groupBoxDisplayOrder.Text = "Display order unit";
            // 
            // tableLayoutPanelDisplayOrder
            // 
            this.tableLayoutPanelDisplayOrder.ColumnCount = 4;
            this.tableLayoutPanelDisplayOrder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelDisplayOrder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDisplayOrder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelDisplayOrder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDisplayOrder.Controls.Add(this.listBoxDisplayOrderListHide, 3, 1);
            this.tableLayoutPanelDisplayOrder.Controls.Add(this.listBoxDisplayOrderListShow, 1, 1);
            this.tableLayoutPanelDisplayOrder.Controls.Add(this.labelDisplayOrderListShow, 1, 0);
            this.tableLayoutPanelDisplayOrder.Controls.Add(this.toolStripDisplayOrder, 0, 1);
            this.tableLayoutPanelDisplayOrder.Controls.Add(this.labelDisplayOrderListHide, 3, 0);
            this.tableLayoutPanelDisplayOrder.Controls.Add(this.toolStripDisplay, 2, 1);
            this.tableLayoutPanelDisplayOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDisplayOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelDisplayOrder.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelDisplayOrder.Name = "tableLayoutPanelDisplayOrder";
            this.tableLayoutPanelDisplayOrder.RowCount = 2;
            this.tableLayoutPanelDisplayOrder.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDisplayOrder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDisplayOrder.Size = new System.Drawing.Size(612, 241);
            this.tableLayoutPanelDisplayOrder.TabIndex = 0;
            // 
            // listBoxDisplayOrderListHide
            // 
            this.listBoxDisplayOrderListHide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxDisplayOrderListHide.IntegralHeight = false;
            this.listBoxDisplayOrderListHide.Location = new System.Drawing.Point(328, 16);
            this.listBoxDisplayOrderListHide.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxDisplayOrderListHide.Name = "listBoxDisplayOrderListHide";
            this.listBoxDisplayOrderListHide.Size = new System.Drawing.Size(284, 225);
            this.listBoxDisplayOrderListHide.TabIndex = 7;
            // 
            // listBoxDisplayOrderListShow
            // 
            this.listBoxDisplayOrderListShow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxDisplayOrderListShow.IntegralHeight = false;
            this.listBoxDisplayOrderListShow.Location = new System.Drawing.Point(20, 16);
            this.listBoxDisplayOrderListShow.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxDisplayOrderListShow.Name = "listBoxDisplayOrderListShow";
            this.listBoxDisplayOrderListShow.Size = new System.Drawing.Size(284, 225);
            this.listBoxDisplayOrderListShow.TabIndex = 3;
            // 
            // labelDisplayOrderListShow
            // 
            this.labelDisplayOrderListShow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDisplayOrderListShow.Location = new System.Drawing.Point(23, 0);
            this.labelDisplayOrderListShow.Name = "labelDisplayOrderListShow";
            this.labelDisplayOrderListShow.Size = new System.Drawing.Size(278, 16);
            this.labelDisplayOrderListShow.TabIndex = 1;
            this.labelDisplayOrderListShow.Text = "Show in label:";
            this.labelDisplayOrderListShow.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // toolStripDisplayOrder
            // 
            this.toolStripDisplayOrder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonDisplayOrderUp,
            this.toolStripButtonDisplayOrderDown,
            this.toolStripDropDownButtonDisplayOrderSort});
            this.toolStripDisplayOrder.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.toolStripDisplayOrder.Location = new System.Drawing.Point(0, 16);
            this.toolStripDisplayOrder.Name = "toolStripDisplayOrder";
            this.toolStripDisplayOrder.Size = new System.Drawing.Size(20, 69);
            this.toolStripDisplayOrder.TabIndex = 0;
            this.toolStripDisplayOrder.Text = "toolStrip1";
            // 
            // toolStripButtonDisplayOrderUp
            // 
            this.toolStripButtonDisplayOrderUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDisplayOrderUp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDisplayOrderUp.Image")));
            this.toolStripButtonDisplayOrderUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDisplayOrderUp.Name = "toolStripButtonDisplayOrderUp";
            this.toolStripButtonDisplayOrderUp.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonDisplayOrderUp.Text = "move selected unit to a higher display order";
            this.toolStripButtonDisplayOrderUp.Click += new System.EventHandler(this.toolStripButtonDisplayOrderUp_Click);
            // 
            // toolStripButtonDisplayOrderDown
            // 
            this.toolStripButtonDisplayOrderDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDisplayOrderDown.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDisplayOrderDown.Image")));
            this.toolStripButtonDisplayOrderDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDisplayOrderDown.Name = "toolStripButtonDisplayOrderDown";
            this.toolStripButtonDisplayOrderDown.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonDisplayOrderDown.Text = "move selected unit to a lower display order";
            this.toolStripButtonDisplayOrderDown.Click += new System.EventHandler(this.toolStripButtonDisplayOrderDown_Click);
            // 
            // toolStripDropDownButtonDisplayOrderSort
            // 
            this.toolStripDropDownButtonDisplayOrderSort.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonDisplayOrderSort.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemDisplayOrderSortByName,
            this.toolStripMenuItemDisplayOrderSortByIdentifier,
            this.toolStripMenuItemDisplayOrderSortByID});
            this.toolStripDropDownButtonDisplayOrderSort.Image = global::DiversityCollection.Resource.Sort;
            this.toolStripDropDownButtonDisplayOrderSort.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonDisplayOrderSort.Name = "toolStripDropDownButtonDisplayOrderSort";
            this.toolStripDropDownButtonDisplayOrderSort.Size = new System.Drawing.Size(29, 20);
            this.toolStripDropDownButtonDisplayOrderSort.Text = "toolStripDropDownButton1";
            // 
            // toolStripMenuItemDisplayOrderSortByName
            // 
            this.toolStripMenuItemDisplayOrderSortByName.Name = "toolStripMenuItemDisplayOrderSortByName";
            this.toolStripMenuItemDisplayOrderSortByName.Size = new System.Drawing.Size(121, 22);
            this.toolStripMenuItemDisplayOrderSortByName.Text = "Name";
            this.toolStripMenuItemDisplayOrderSortByName.ToolTipText = "Sort by last identification";
            this.toolStripMenuItemDisplayOrderSortByName.Click += new System.EventHandler(this.toolStripMenuItemDisplayOrderSortByName_Click);
            // 
            // toolStripMenuItemDisplayOrderSortByIdentifier
            // 
            this.toolStripMenuItemDisplayOrderSortByIdentifier.Name = "toolStripMenuItemDisplayOrderSortByIdentifier";
            this.toolStripMenuItemDisplayOrderSortByIdentifier.Size = new System.Drawing.Size(121, 22);
            this.toolStripMenuItemDisplayOrderSortByIdentifier.Text = "Identifier";
            this.toolStripMenuItemDisplayOrderSortByIdentifier.ToolTipText = "Sort by identifier";
            this.toolStripMenuItemDisplayOrderSortByIdentifier.Click += new System.EventHandler(this.toolStripMenuItemDisplayOrderSortByIdentifier_Click);
            // 
            // toolStripMenuItemDisplayOrderSortByID
            // 
            this.toolStripMenuItemDisplayOrderSortByID.Name = "toolStripMenuItemDisplayOrderSortByID";
            this.toolStripMenuItemDisplayOrderSortByID.Size = new System.Drawing.Size(121, 22);
            this.toolStripMenuItemDisplayOrderSortByID.Text = "ID";
            this.toolStripMenuItemDisplayOrderSortByID.ToolTipText = "Sort by ID";
            this.toolStripMenuItemDisplayOrderSortByID.Click += new System.EventHandler(this.toolStripMenuItemDisplayOrderSortByID_Click);
            // 
            // labelDisplayOrderListHide
            // 
            this.labelDisplayOrderListHide.Location = new System.Drawing.Point(331, 0);
            this.labelDisplayOrderListHide.Name = "labelDisplayOrderListHide";
            this.labelDisplayOrderListHide.Size = new System.Drawing.Size(2, 16);
            this.labelDisplayOrderListHide.TabIndex = 6;
            this.labelDisplayOrderListHide.Text = "Hide:";
            this.labelDisplayOrderListHide.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // toolStripDisplay
            // 
            this.toolStripDisplay.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonDisplayShow,
            this.toolStripButtonDisplayHide});
            this.toolStripDisplay.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.toolStripDisplay.Location = new System.Drawing.Point(304, 16);
            this.toolStripDisplay.Name = "toolStripDisplay";
            this.toolStripDisplay.Size = new System.Drawing.Size(24, 40);
            this.toolStripDisplay.TabIndex = 8;
            this.toolStripDisplay.Text = "toolStrip1";
            // 
            // toolStripButtonDisplayShow
            // 
            this.toolStripButtonDisplayShow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonDisplayShow.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButtonDisplayShow.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDisplayShow.Image")));
            this.toolStripButtonDisplayShow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDisplayShow.Name = "toolStripButtonDisplayShow";
            this.toolStripButtonDisplayShow.Size = new System.Drawing.Size(23, 17);
            this.toolStripButtonDisplayShow.Text = "<";
            this.toolStripButtonDisplayShow.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.toolStripButtonDisplayShow.ToolTipText = "show the selected unit in the label";
            this.toolStripButtonDisplayShow.Click += new System.EventHandler(this.toolStripButtonDisplayShow_Click);
            // 
            // toolStripButtonDisplayHide
            // 
            this.toolStripButtonDisplayHide.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonDisplayHide.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButtonDisplayHide.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDisplayHide.Image")));
            this.toolStripButtonDisplayHide.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDisplayHide.Name = "toolStripButtonDisplayHide";
            this.toolStripButtonDisplayHide.Size = new System.Drawing.Size(23, 17);
            this.toolStripButtonDisplayHide.Text = ">";
            this.toolStripButtonDisplayHide.ToolTipText = "Hide the selected unit";
            this.toolStripButtonDisplayHide.Click += new System.EventHandler(this.toolStripButtonDisplayHide_Click);
            // 
            // UserControl_DisplayOrderUnit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxDisplayOrder);
            this.Name = "UserControl_DisplayOrderUnit";
            this.Size = new System.Drawing.Size(618, 260);
            this.groupBoxDisplayOrder.ResumeLayout(false);
            this.tableLayoutPanelDisplayOrder.ResumeLayout(false);
            this.tableLayoutPanelDisplayOrder.PerformLayout();
            this.toolStripDisplayOrder.ResumeLayout(false);
            this.toolStripDisplayOrder.PerformLayout();
            this.toolStripDisplay.ResumeLayout(false);
            this.toolStripDisplay.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxDisplayOrder;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDisplayOrder;
        private System.Windows.Forms.ListBox listBoxDisplayOrderListHide;
        private System.Windows.Forms.ListBox listBoxDisplayOrderListShow;
        private System.Windows.Forms.Label labelDisplayOrderListShow;
        private System.Windows.Forms.ToolStrip toolStripDisplayOrder;
        private System.Windows.Forms.ToolStripButton toolStripButtonDisplayOrderUp;
        private System.Windows.Forms.ToolStripButton toolStripButtonDisplayOrderDown;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonDisplayOrderSort;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDisplayOrderSortByName;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDisplayOrderSortByIdentifier;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDisplayOrderSortByID;
        private System.Windows.Forms.Label labelDisplayOrderListHide;
        private System.Windows.Forms.ToolStrip toolStripDisplay;
        private System.Windows.Forms.ToolStripButton toolStripButtonDisplayShow;
        private System.Windows.Forms.ToolStripButton toolStripButtonDisplayHide;
    }
}
