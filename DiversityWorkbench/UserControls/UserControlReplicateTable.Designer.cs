namespace DiversityWorkbench.UserControls
{
    partial class UserControlReplicateTable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlReplicateTable));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelConflict = new System.Windows.Forms.Label();
            this.checkBoxTableName = new System.Windows.Forms.CheckBox();
            this.labelNumberOfDatasets = new System.Windows.Forms.Label();
            this.labelNumberOfErrors = new System.Windows.Forms.Label();
            this.buttonInfo = new System.Windows.Forms.Button();
            this.labelUpdated = new System.Windows.Forms.Label();
            this.labelInserted = new System.Windows.Forms.Label();
            this.pictureBoxTable = new System.Windows.Forms.PictureBox();
            this.pictureBoxIndexRowGuidProvider = new System.Windows.Forms.PictureBox();
            this.pictureBoxIndexRowGuidSubscriber = new System.Windows.Forms.PictureBox();
            this.buttonFilter = new System.Windows.Forms.Button();
            this.panelProgress = new System.Windows.Forms.Panel();
            this.labelProgress = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.buttonFilterPropagate = new System.Windows.Forms.Button();
            this.buttonFilterClear = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.imageListState = new System.Windows.Forms.ImageList(this.components);
            this.imageListDirection = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIndexRowGuidProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIndexRowGuidSubscriber)).BeginInit();
            this.panelProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 10;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.labelConflict, 4, 0);
            this.tableLayoutPanel.Controls.Add(this.checkBoxTableName, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labelNumberOfDatasets, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.labelNumberOfErrors, 4, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonInfo, 8, 0);
            this.tableLayoutPanel.Controls.Add(this.labelUpdated, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.labelInserted, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxTable, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxIndexRowGuidProvider, 9, 0);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxIndexRowGuidSubscriber, 9, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonFilter, 6, 0);
            this.tableLayoutPanel.Controls.Add(this.panelProgress, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonFilterPropagate, 7, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonFilterClear, 7, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(646, 26);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelConflict
            // 
            this.labelConflict.AutoSize = true;
            this.labelConflict.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelConflict.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelConflict.ForeColor = System.Drawing.Color.Magenta;
            this.labelConflict.Location = new System.Drawing.Point(463, 0);
            this.labelConflict.Name = "labelConflict";
            this.labelConflict.Size = new System.Drawing.Size(94, 13);
            this.labelConflict.TabIndex = 7;
            this.labelConflict.Text = "Conflicts:";
            this.labelConflict.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.labelConflict, "Number of conflicts");
            this.labelConflict.Visible = false;
            // 
            // checkBoxTableName
            // 
            this.checkBoxTableName.AutoSize = true;
            this.checkBoxTableName.Checked = true;
            this.checkBoxTableName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTableName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxTableName.Location = new System.Drawing.Point(23, 3);
            this.checkBoxTableName.Name = "checkBoxTableName";
            this.tableLayoutPanel.SetRowSpan(this.checkBoxTableName, 2);
            this.checkBoxTableName.Size = new System.Drawing.Size(194, 20);
            this.checkBoxTableName.TabIndex = 0;
            this.checkBoxTableName.Text = "Table";
            this.checkBoxTableName.UseVisualStyleBackColor = true;
            this.checkBoxTableName.CheckedChanged += new System.EventHandler(this.checkBoxTableName_CheckedChanged);
            // 
            // labelNumberOfDatasets
            // 
            this.labelNumberOfDatasets.AutoSize = true;
            this.labelNumberOfDatasets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNumberOfDatasets.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNumberOfDatasets.ForeColor = System.Drawing.Color.Blue;
            this.labelNumberOfDatasets.Location = new System.Drawing.Point(223, 0);
            this.labelNumberOfDatasets.Name = "labelNumberOfDatasets";
            this.labelNumberOfDatasets.Size = new System.Drawing.Size(124, 13);
            this.labelNumberOfDatasets.TabIndex = 1;
            this.labelNumberOfDatasets.Text = "Total:";
            this.labelNumberOfDatasets.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.labelNumberOfDatasets, "Number of datasets in the source table");
            // 
            // labelNumberOfErrors
            // 
            this.labelNumberOfErrors.AutoSize = true;
            this.labelNumberOfErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNumberOfErrors.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNumberOfErrors.ForeColor = System.Drawing.Color.Red;
            this.labelNumberOfErrors.Location = new System.Drawing.Point(463, 13);
            this.labelNumberOfErrors.Name = "labelNumberOfErrors";
            this.labelNumberOfErrors.Size = new System.Drawing.Size(94, 13);
            this.labelNumberOfErrors.TabIndex = 5;
            this.labelNumberOfErrors.Text = "Errors:";
            this.labelNumberOfErrors.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.labelNumberOfErrors, "Number of errors");
            this.labelNumberOfErrors.Visible = false;
            // 
            // buttonInfo
            // 
            this.buttonInfo.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonInfo.Location = new System.Drawing.Point(606, 0);
            this.buttonInfo.Margin = new System.Windows.Forms.Padding(0);
            this.buttonInfo.Name = "buttonInfo";
            this.tableLayoutPanel.SetRowSpan(this.buttonInfo, 2);
            this.buttonInfo.Size = new System.Drawing.Size(26, 26);
            this.buttonInfo.TabIndex = 8;
            this.toolTip.SetToolTip(this.buttonInfo, "Solve the conflicts");
            this.buttonInfo.UseVisualStyleBackColor = true;
            this.buttonInfo.Click += new System.EventHandler(this.buttonInfo_Click);
            // 
            // labelUpdated
            // 
            this.labelUpdated.AutoSize = true;
            this.labelUpdated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUpdated.ForeColor = System.Drawing.Color.SaddleBrown;
            this.labelUpdated.Location = new System.Drawing.Point(350, 0);
            this.labelUpdated.Margin = new System.Windows.Forms.Padding(0);
            this.labelUpdated.Name = "labelUpdated";
            this.labelUpdated.Size = new System.Drawing.Size(110, 13);
            this.labelUpdated.TabIndex = 9;
            this.labelUpdated.Text = "Updated:";
            this.labelUpdated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.labelUpdated, "Number of datasets updated in the destination table");
            this.labelUpdated.Visible = false;
            // 
            // labelInserted
            // 
            this.labelInserted.AutoSize = true;
            this.labelInserted.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInserted.ForeColor = System.Drawing.Color.Black;
            this.labelInserted.Location = new System.Drawing.Point(350, 13);
            this.labelInserted.Margin = new System.Windows.Forms.Padding(0);
            this.labelInserted.Name = "labelInserted";
            this.labelInserted.Size = new System.Drawing.Size(110, 13);
            this.labelInserted.TabIndex = 10;
            this.labelInserted.Text = "Inserted:";
            this.labelInserted.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.labelInserted, "Number of datasets inserted in the destination table");
            this.labelInserted.Visible = false;
            // 
            // pictureBoxTable
            // 
            this.pictureBoxTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxTable.Location = new System.Drawing.Point(2, 5);
            this.pictureBoxTable.Margin = new System.Windows.Forms.Padding(2, 5, 2, 5);
            this.pictureBoxTable.Name = "pictureBoxTable";
            this.tableLayoutPanel.SetRowSpan(this.pictureBoxTable, 2);
            this.pictureBoxTable.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxTable.TabIndex = 12;
            this.pictureBoxTable.TabStop = false;
            // 
            // pictureBoxIndexRowGuidProvider
            // 
            this.pictureBoxIndexRowGuidProvider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxIndexRowGuidProvider.Image = global::DiversityWorkbench.Properties.Resources.Index;
            this.pictureBoxIndexRowGuidProvider.Location = new System.Drawing.Point(632, 0);
            this.pictureBoxIndexRowGuidProvider.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxIndexRowGuidProvider.Name = "pictureBoxIndexRowGuidProvider";
            this.pictureBoxIndexRowGuidProvider.Size = new System.Drawing.Size(14, 13);
            this.pictureBoxIndexRowGuidProvider.TabIndex = 13;
            this.pictureBoxIndexRowGuidProvider.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxIndexRowGuidProvider, "Create an index for the column RowGUID in the provider database");
            this.pictureBoxIndexRowGuidProvider.Visible = false;
            this.pictureBoxIndexRowGuidProvider.Click += new System.EventHandler(this.pictureBoxIndexRowGuidProvider_Click);
            // 
            // pictureBoxIndexRowGuidSubscriber
            // 
            this.pictureBoxIndexRowGuidSubscriber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxIndexRowGuidSubscriber.Image = global::DiversityWorkbench.Properties.Resources.Index;
            this.pictureBoxIndexRowGuidSubscriber.Location = new System.Drawing.Point(632, 13);
            this.pictureBoxIndexRowGuidSubscriber.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxIndexRowGuidSubscriber.Name = "pictureBoxIndexRowGuidSubscriber";
            this.pictureBoxIndexRowGuidSubscriber.Size = new System.Drawing.Size(14, 13);
            this.pictureBoxIndexRowGuidSubscriber.TabIndex = 14;
            this.pictureBoxIndexRowGuidSubscriber.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxIndexRowGuidSubscriber, "Create an index for the column RowGUID in the subscriber database");
            this.pictureBoxIndexRowGuidSubscriber.Visible = false;
            this.pictureBoxIndexRowGuidSubscriber.Click += new System.EventHandler(this.pictureBoxIndexRowGuidSubscriber_Click);
            // 
            // buttonFilter
            // 
            this.buttonFilter.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonFilter.FlatAppearance.BorderSize = 0;
            this.buttonFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFilter.Image = global::DiversityWorkbench.Properties.Resources.Filter;
            this.buttonFilter.Location = new System.Drawing.Point(574, 1);
            this.buttonFilter.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.buttonFilter.Name = "buttonFilter";
            this.tableLayoutPanel.SetRowSpan(this.buttonFilter, 2);
            this.buttonFilter.Size = new System.Drawing.Size(12, 24);
            this.buttonFilter.TabIndex = 15;
            this.toolTip.SetToolTip(this.buttonFilter, "Set a filter for the table");
            this.buttonFilter.UseVisualStyleBackColor = false;
            this.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
            // 
            // panelProgress
            // 
            this.panelProgress.Controls.Add(this.labelProgress);
            this.panelProgress.Controls.Add(this.progressBar);
            this.panelProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProgress.Location = new System.Drawing.Point(220, 13);
            this.panelProgress.Margin = new System.Windows.Forms.Padding(0);
            this.panelProgress.Name = "panelProgress";
            this.panelProgress.Size = new System.Drawing.Size(130, 13);
            this.panelProgress.TabIndex = 16;
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelProgress.Location = new System.Drawing.Point(0, 0);
            this.labelProgress.Margin = new System.Windows.Forms.Padding(0);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(0, 13);
            this.labelProgress.TabIndex = 12;
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar.Location = new System.Drawing.Point(0, 0);
            this.progressBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(130, 13);
            this.progressBar.TabIndex = 11;
            this.progressBar.Visible = false;
            // 
            // buttonFilterPropagate
            // 
            this.buttonFilterPropagate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFilterPropagate.FlatAppearance.BorderSize = 0;
            this.buttonFilterPropagate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFilterPropagate.Image = global::DiversityWorkbench.Properties.Resources.FilterSave;
            this.buttonFilterPropagate.Location = new System.Drawing.Point(586, 0);
            this.buttonFilterPropagate.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFilterPropagate.Name = "buttonFilterPropagate";
            this.buttonFilterPropagate.Size = new System.Drawing.Size(20, 13);
            this.buttonFilterPropagate.TabIndex = 17;
            this.toolTip.SetToolTip(this.buttonFilterPropagate, "Copy filter settings to all tables with corresponding columns");
            this.buttonFilterPropagate.UseVisualStyleBackColor = true;
            this.buttonFilterPropagate.Visible = false;
            this.buttonFilterPropagate.Click += new System.EventHandler(this.buttonFilterPropagate_Click);
            // 
            // buttonFilterClear
            // 
            this.buttonFilterClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFilterClear.FlatAppearance.BorderSize = 0;
            this.buttonFilterClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFilterClear.Image = global::DiversityWorkbench.Properties.Resources.FilterClear;
            this.buttonFilterClear.Location = new System.Drawing.Point(586, 13);
            this.buttonFilterClear.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFilterClear.Name = "buttonFilterClear";
            this.buttonFilterClear.Size = new System.Drawing.Size(20, 13);
            this.buttonFilterClear.TabIndex = 18;
            this.toolTip.SetToolTip(this.buttonFilterClear, "Clear filter");
            this.buttonFilterClear.UseVisualStyleBackColor = true;
            this.buttonFilterClear.Visible = false;
            this.buttonFilterClear.Click += new System.EventHandler(this.buttonFilterClear_Click);
            // 
            // imageListState
            // 
            this.imageListState.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListState.ImageStream")));
            this.imageListState.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListState.Images.SetKeyName(0, "OK.ico");
            this.imageListState.Images.SetKeyName(1, "Conflict.ico");
            this.imageListState.Images.SetKeyName(2, "Error.ico");
            this.imageListState.Images.SetKeyName(3, "wait_animation.gif");
            // 
            // imageListDirection
            // 
            this.imageListDirection.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDirection.ImageStream")));
            this.imageListDirection.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListDirection.Images.SetKeyName(0, "Download.ico");
            this.imageListDirection.Images.SetKeyName(1, "Upload.ico");
            this.imageListDirection.Images.SetKeyName(2, "CleanDatabase.ico");
            this.imageListDirection.Images.SetKeyName(3, "Merge.ico");
            // 
            // UserControlReplicateTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlReplicateTable";
            this.Size = new System.Drawing.Size(646, 26);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIndexRowGuidProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIndexRowGuidSubscriber)).EndInit();
            this.panelProgress.ResumeLayout(false);
            this.panelProgress.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.CheckBox checkBoxTableName;
        private System.Windows.Forms.Label labelNumberOfDatasets;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelNumberOfErrors;
        private System.Windows.Forms.Label labelConflict;
        private System.Windows.Forms.Button buttonInfo;
        private System.Windows.Forms.Label labelUpdated;
        private System.Windows.Forms.Label labelInserted;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.PictureBox pictureBoxTable;
        private System.Windows.Forms.ImageList imageListState;
        private System.Windows.Forms.ImageList imageListDirection;
        private System.Windows.Forms.PictureBox pictureBoxIndexRowGuidProvider;
        private System.Windows.Forms.PictureBox pictureBoxIndexRowGuidSubscriber;
        private System.Windows.Forms.Button buttonFilter;
        private System.Windows.Forms.Panel panelProgress;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.Button buttonFilterPropagate;
        private System.Windows.Forms.Button buttonFilterClear;
    }
}
