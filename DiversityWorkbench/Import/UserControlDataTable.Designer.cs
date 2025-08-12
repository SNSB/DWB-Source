namespace DiversityWorkbench.Import
{
    partial class UserControlDataTable
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlDataTable));
            this.radioButtonInsert = new System.Windows.Forms.RadioButton();
            this.radioButtonMerge = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelTable = new System.Windows.Forms.Label();
            this.radioButtonUpdate = new System.Windows.Forms.RadioButton();
            this.pictureBoxTable = new System.Windows.Forms.PictureBox();
            this.pictureBoxMergeHandling = new System.Windows.Forms.PictureBox();
            this.radioButtonAttach = new System.Windows.Forms.RadioButton();
            this.imageListMergeHandling = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMergeHandling)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButtonInsert
            // 
            this.radioButtonInsert.AutoSize = true;
            this.radioButtonInsert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonInsert.Location = new System.Drawing.Point(223, 3);
            this.radioButtonInsert.Name = "radioButtonInsert";
            this.radioButtonInsert.Size = new System.Drawing.Size(51, 17);
            this.radioButtonInsert.TabIndex = 1;
            this.radioButtonInsert.Text = "Insert";
            this.toolTip.SetToolTip(this.radioButtonInsert, "Insert the data");
            this.radioButtonInsert.UseVisualStyleBackColor = true;
            this.radioButtonInsert.Click += new System.EventHandler(this.radioButtonInsert_Click);
            // 
            // radioButtonMerge
            // 
            this.radioButtonMerge.AutoSize = true;
            this.radioButtonMerge.Checked = true;
            this.radioButtonMerge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonMerge.Location = new System.Drawing.Point(280, 3);
            this.radioButtonMerge.Name = "radioButtonMerge";
            this.radioButtonMerge.Size = new System.Drawing.Size(55, 17);
            this.radioButtonMerge.TabIndex = 2;
            this.radioButtonMerge.TabStop = true;
            this.radioButtonMerge.Text = "Merge";
            this.toolTip.SetToolTip(this.radioButtonMerge, "Compare database and file data according to keys. If data exist - update, otherwi" +
        "se insert");
            this.radioButtonMerge.UseVisualStyleBackColor = true;
            this.radioButtonMerge.Click += new System.EventHandler(this.radioButtonMerge_Click);
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 7;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Controls.Add(this.labelTable, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.radioButtonInsert, 2, 0);
            this.tableLayoutPanelMain.Controls.Add(this.radioButtonMerge, 3, 0);
            this.tableLayoutPanelMain.Controls.Add(this.radioButtonUpdate, 4, 0);
            this.tableLayoutPanelMain.Controls.Add(this.pictureBoxTable, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.pictureBoxMergeHandling, 6, 0);
            this.tableLayoutPanelMain.Controls.Add(this.radioButtonAttach, 5, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 1;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(493, 23);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // labelTable
            // 
            this.labelTable.AutoSize = true;
            this.labelTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTable.Location = new System.Drawing.Point(23, 0);
            this.labelTable.Name = "labelTable";
            this.labelTable.Size = new System.Drawing.Size(194, 23);
            this.labelTable.TabIndex = 0;
            this.labelTable.Text = "Table";
            this.labelTable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // radioButtonUpdate
            // 
            this.radioButtonUpdate.AutoSize = true;
            this.radioButtonUpdate.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonUpdate.Location = new System.Drawing.Point(341, 3);
            this.radioButtonUpdate.Name = "radioButtonUpdate";
            this.radioButtonUpdate.Size = new System.Drawing.Size(60, 17);
            this.radioButtonUpdate.TabIndex = 3;
            this.radioButtonUpdate.Text = "Update";
            this.toolTip.SetToolTip(this.radioButtonUpdate, "Search for data in the database according to key and update according to data in " +
        "the file");
            this.radioButtonUpdate.UseVisualStyleBackColor = true;
            this.radioButtonUpdate.Click += new System.EventHandler(this.radioButtonUpdate_Click);
            // 
            // pictureBoxTable
            // 
            this.pictureBoxTable.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxTable.Margin = new System.Windows.Forms.Padding(3, 3, 1, 3);
            this.pictureBoxTable.Name = "pictureBoxTable";
            this.pictureBoxTable.Size = new System.Drawing.Size(16, 17);
            this.pictureBoxTable.TabIndex = 4;
            this.pictureBoxTable.TabStop = false;
            // 
            // pictureBoxMergeHandling
            // 
            this.pictureBoxMergeHandling.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxMergeHandling.Location = new System.Drawing.Point(473, 4);
            this.pictureBoxMergeHandling.Margin = new System.Windows.Forms.Padding(0, 4, 4, 3);
            this.pictureBoxMergeHandling.Name = "pictureBoxMergeHandling";
            this.pictureBoxMergeHandling.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxMergeHandling.TabIndex = 5;
            this.pictureBoxMergeHandling.TabStop = false;
            // 
            // radioButtonAttach
            // 
            this.radioButtonAttach.AutoSize = true;
            this.radioButtonAttach.Location = new System.Drawing.Point(407, 3);
            this.radioButtonAttach.Name = "radioButtonAttach";
            this.radioButtonAttach.Size = new System.Drawing.Size(56, 17);
            this.radioButtonAttach.TabIndex = 6;
            this.radioButtonAttach.TabStop = true;
            this.radioButtonAttach.Text = "Attach";
            this.toolTip.SetToolTip(this.radioButtonAttach, "Search for data in the database according to key and use for attachment. Leave da" +
        "ta in database of this table unchainged");
            this.radioButtonAttach.UseVisualStyleBackColor = true;
            this.radioButtonAttach.Click += new System.EventHandler(this.radioButtonAttach_Click);
            // 
            // imageListMergeHandling
            // 
            this.imageListMergeHandling.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMergeHandling.ImageStream")));
            this.imageListMergeHandling.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListMergeHandling.Images.SetKeyName(0, "MergeInsert.ico");
            this.imageListMergeHandling.Images.SetKeyName(1, "MergeMerge.ico");
            this.imageListMergeHandling.Images.SetKeyName(2, "MergeUpdate.ico");
            this.imageListMergeHandling.Images.SetKeyName(3, "Attach.ico");
            // 
            // UserControlDataTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Name = "UserControlDataTable";
            this.Size = new System.Drawing.Size(493, 23);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMergeHandling)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonInsert;
        private System.Windows.Forms.RadioButton radioButtonMerge;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelTable;
        private System.Windows.Forms.RadioButton radioButtonUpdate;
        private System.Windows.Forms.PictureBox pictureBoxTable;
        private System.Windows.Forms.PictureBox pictureBoxMergeHandling;
        private System.Windows.Forms.ImageList imageListMergeHandling;
        private System.Windows.Forms.RadioButton radioButtonAttach;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
