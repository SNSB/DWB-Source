namespace DiversityCollection.UserControls
{
    partial class UserControl_PartDescription
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_PartDescription));
            this.groupBoxPartDescription = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelPartDescription = new System.Windows.Forms.TableLayoutPanel();
            this.listBoxPartDescription = new System.Windows.Forms.ListBox();
            this.toolStripPartDescription = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonPartDescriptionAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPartDescriptionDelete = new System.Windows.Forms.ToolStripButton();
            this.textBoxPartDescriptionNotes = new System.Windows.Forms.TextBox();
            this.labelPartDescriptionNotes = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntryPartDescription = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.labelIdentificationUnitID = new System.Windows.Forms.Label();
            this.comboBoxIdentificationUnitID = new System.Windows.Forms.ComboBox();
            this.labelDescriptionHierarchyCache = new System.Windows.Forms.Label();
            this.textBoxDescriptionHierarchyCache = new System.Windows.Forms.TextBox();
            this.groupBoxPartDescription.SuspendLayout();
            this.tableLayoutPanelPartDescription.SuspendLayout();
            this.toolStripPartDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // groupBoxPartDescription
            // 
            this.groupBoxPartDescription.Controls.Add(this.tableLayoutPanelPartDescription);
            this.groupBoxPartDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxPartDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxPartDescription.Location = new System.Drawing.Point(0, 0);
            this.groupBoxPartDescription.Name = "groupBoxPartDescription";
            this.groupBoxPartDescription.Size = new System.Drawing.Size(717, 150);
            this.groupBoxPartDescription.TabIndex = 50;
            this.groupBoxPartDescription.TabStop = false;
            this.groupBoxPartDescription.Text = "Descriptions";
            // 
            // tableLayoutPanelPartDescription
            // 
            this.tableLayoutPanelPartDescription.ColumnCount = 4;
            this.tableLayoutPanelPartDescription.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPartDescription.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPartDescription.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelPartDescription.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelPartDescription.Controls.Add(this.listBoxPartDescription, 0, 0);
            this.tableLayoutPanelPartDescription.Controls.Add(this.toolStripPartDescription, 0, 3);
            this.tableLayoutPanelPartDescription.Controls.Add(this.textBoxPartDescriptionNotes, 1, 2);
            this.tableLayoutPanelPartDescription.Controls.Add(this.labelPartDescriptionNotes, 1, 1);
            this.tableLayoutPanelPartDescription.Controls.Add(this.userControlModuleRelatedEntryPartDescription, 1, 0);
            this.tableLayoutPanelPartDescription.Controls.Add(this.labelIdentificationUnitID, 1, 3);
            this.tableLayoutPanelPartDescription.Controls.Add(this.comboBoxIdentificationUnitID, 2, 3);
            this.tableLayoutPanelPartDescription.Controls.Add(this.labelDescriptionHierarchyCache, 3, 1);
            this.tableLayoutPanelPartDescription.Controls.Add(this.textBoxDescriptionHierarchyCache, 3, 2);
            this.tableLayoutPanelPartDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelPartDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelPartDescription.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelPartDescription.Name = "tableLayoutPanelPartDescription";
            this.tableLayoutPanelPartDescription.RowCount = 4;
            this.tableLayoutPanelPartDescription.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPartDescription.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPartDescription.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPartDescription.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPartDescription.Size = new System.Drawing.Size(711, 131);
            this.tableLayoutPanelPartDescription.TabIndex = 0;
            // 
            // listBoxPartDescription
            // 
            this.listBoxPartDescription.DisplayMember = "Description";
            this.listBoxPartDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxPartDescription.FormattingEnabled = true;
            this.listBoxPartDescription.IntegralHeight = false;
            this.listBoxPartDescription.Location = new System.Drawing.Point(0, 0);
            this.listBoxPartDescription.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxPartDescription.Name = "listBoxPartDescription";
            this.tableLayoutPanelPartDescription.SetRowSpan(this.listBoxPartDescription, 3);
            this.listBoxPartDescription.Size = new System.Drawing.Size(120, 106);
            this.listBoxPartDescription.TabIndex = 0;
            this.listBoxPartDescription.ValueMember = "PartDescriptionID";
            this.listBoxPartDescription.SelectedIndexChanged += new System.EventHandler(this.listBoxPartDescription_SelectedIndexChanged);
            this.listBoxPartDescription.DataSourceChanged += new System.EventHandler(this.listBoxPartDescription_DataSourceChanged);
            // 
            // toolStripPartDescription
            // 
            this.toolStripPartDescription.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripPartDescription.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonPartDescriptionAdd,
            this.toolStripButtonPartDescriptionDelete});
            this.toolStripPartDescription.Location = new System.Drawing.Point(0, 106);
            this.toolStripPartDescription.Name = "toolStripPartDescription";
            this.toolStripPartDescription.Size = new System.Drawing.Size(120, 25);
            this.toolStripPartDescription.TabIndex = 1;
            this.toolStripPartDescription.Text = "toolStrip1";
            // 
            // toolStripButtonPartDescriptionAdd
            // 
            this.toolStripButtonPartDescriptionAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPartDescriptionAdd.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonPartDescriptionAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPartDescriptionAdd.Name = "toolStripButtonPartDescriptionAdd";
            this.toolStripButtonPartDescriptionAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPartDescriptionAdd.Text = "Add a new description for the part";
            this.toolStripButtonPartDescriptionAdd.Click += new System.EventHandler(this.toolStripButtonPartDescriptionAdd_Click);
            // 
            // toolStripButtonPartDescriptionDelete
            // 
            this.toolStripButtonPartDescriptionDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPartDescriptionDelete.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonPartDescriptionDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPartDescriptionDelete.Name = "toolStripButtonPartDescriptionDelete";
            this.toolStripButtonPartDescriptionDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPartDescriptionDelete.Text = "Remove the selected description";
            this.toolStripButtonPartDescriptionDelete.Click += new System.EventHandler(this.toolStripButtonPartDescriptionDelete_Click);
            // 
            // textBoxPartDescriptionNotes
            // 
            this.tableLayoutPanelPartDescription.SetColumnSpan(this.textBoxPartDescriptionNotes, 2);
            this.textBoxPartDescriptionNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPartDescriptionNotes.Location = new System.Drawing.Point(120, 37);
            this.textBoxPartDescriptionNotes.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxPartDescriptionNotes.Multiline = true;
            this.textBoxPartDescriptionNotes.Name = "textBoxPartDescriptionNotes";
            this.textBoxPartDescriptionNotes.Size = new System.Drawing.Size(311, 69);
            this.textBoxPartDescriptionNotes.TabIndex = 2;
            // 
            // labelPartDescriptionNotes
            // 
            this.labelPartDescriptionNotes.AutoSize = true;
            this.tableLayoutPanelPartDescription.SetColumnSpan(this.labelPartDescriptionNotes, 2);
            this.labelPartDescriptionNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPartDescriptionNotes.Location = new System.Drawing.Point(123, 24);
            this.labelPartDescriptionNotes.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelPartDescriptionNotes.Name = "labelPartDescriptionNotes";
            this.labelPartDescriptionNotes.Size = new System.Drawing.Size(308, 13);
            this.labelPartDescriptionNotes.TabIndex = 3;
            this.labelPartDescriptionNotes.Text = "Notes:";
            this.labelPartDescriptionNotes.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // userControlModuleRelatedEntryPartDescription
            // 
            this.userControlModuleRelatedEntryPartDescription.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelPartDescription.SetColumnSpan(this.userControlModuleRelatedEntryPartDescription, 3);
            this.userControlModuleRelatedEntryPartDescription.DependsOnUri = "";
            this.userControlModuleRelatedEntryPartDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryPartDescription.Domain = "";
            this.userControlModuleRelatedEntryPartDescription.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryPartDescription.Location = new System.Drawing.Point(120, 0);
            this.userControlModuleRelatedEntryPartDescription.Margin = new System.Windows.Forms.Padding(0);
            this.userControlModuleRelatedEntryPartDescription.Module = null;
            this.userControlModuleRelatedEntryPartDescription.Name = "userControlModuleRelatedEntryPartDescription";
            this.userControlModuleRelatedEntryPartDescription.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryPartDescription.ShowInfo = false;
            this.userControlModuleRelatedEntryPartDescription.Size = new System.Drawing.Size(591, 24);
            this.userControlModuleRelatedEntryPartDescription.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryPartDescription.TabIndex = 4;
            // 
            // labelIdentificationUnitID
            // 
            this.labelIdentificationUnitID.AutoSize = true;
            this.labelIdentificationUnitID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelIdentificationUnitID.Location = new System.Drawing.Point(123, 106);
            this.labelIdentificationUnitID.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelIdentificationUnitID.Name = "labelIdentificationUnitID";
            this.labelIdentificationUnitID.Size = new System.Drawing.Size(29, 25);
            this.labelIdentificationUnitID.TabIndex = 5;
            this.labelIdentificationUnitID.Text = "Unit:";
            this.labelIdentificationUnitID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxIdentificationUnitID
            // 
            this.tableLayoutPanelPartDescription.SetColumnSpan(this.comboBoxIdentificationUnitID, 2);
            this.comboBoxIdentificationUnitID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxIdentificationUnitID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxIdentificationUnitID.FormattingEnabled = true;
            this.comboBoxIdentificationUnitID.Location = new System.Drawing.Point(152, 109);
            this.comboBoxIdentificationUnitID.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.comboBoxIdentificationUnitID.Name = "comboBoxIdentificationUnitID";
            this.comboBoxIdentificationUnitID.Size = new System.Drawing.Size(559, 21);
            this.comboBoxIdentificationUnitID.TabIndex = 6;
            // 
            // labelDescriptionHierarchyCache
            // 
            this.labelDescriptionHierarchyCache.AutoSize = true;
            this.labelDescriptionHierarchyCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDescriptionHierarchyCache.Location = new System.Drawing.Point(431, 24);
            this.labelDescriptionHierarchyCache.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescriptionHierarchyCache.Name = "labelDescriptionHierarchyCache";
            this.labelDescriptionHierarchyCache.Size = new System.Drawing.Size(280, 13);
            this.labelDescriptionHierarchyCache.TabIndex = 7;
            this.labelDescriptionHierarchyCache.Text = "Hierarchy:";
            this.labelDescriptionHierarchyCache.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxDescriptionHierarchyCache
            // 
            this.textBoxDescriptionHierarchyCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDescriptionHierarchyCache.Location = new System.Drawing.Point(434, 37);
            this.textBoxDescriptionHierarchyCache.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.textBoxDescriptionHierarchyCache.Multiline = true;
            this.textBoxDescriptionHierarchyCache.Name = "textBoxDescriptionHierarchyCache";
            this.textBoxDescriptionHierarchyCache.ReadOnly = true;
            this.textBoxDescriptionHierarchyCache.Size = new System.Drawing.Size(274, 69);
            this.textBoxDescriptionHierarchyCache.TabIndex = 8;
            // 
            // UserControl_PartDescription
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxPartDescription);
            this.Name = "UserControl_PartDescription";
            this.Size = new System.Drawing.Size(717, 150);
            this.groupBoxPartDescription.ResumeLayout(false);
            this.tableLayoutPanelPartDescription.ResumeLayout(false);
            this.tableLayoutPanelPartDescription.PerformLayout();
            this.toolStripPartDescription.ResumeLayout(false);
            this.toolStripPartDescription.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxPartDescription;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPartDescription;
        private System.Windows.Forms.ListBox listBoxPartDescription;
        private System.Windows.Forms.ToolStrip toolStripPartDescription;
        private System.Windows.Forms.ToolStripButton toolStripButtonPartDescriptionAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonPartDescriptionDelete;
        private System.Windows.Forms.TextBox textBoxPartDescriptionNotes;
        private System.Windows.Forms.Label labelPartDescriptionNotes;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryPartDescription;
        private System.Windows.Forms.Label labelIdentificationUnitID;
        private System.Windows.Forms.ComboBox comboBoxIdentificationUnitID;
        private System.Windows.Forms.Label labelDescriptionHierarchyCache;
        private System.Windows.Forms.TextBox textBoxDescriptionHierarchyCache;
    }
}
