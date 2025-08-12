namespace DiversityWorkbench.Import
{
    partial class UserControlTesting
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.buttonTestData = new System.Windows.Forms.Button();
            this.numericUpDownAnalyseData = new System.Windows.Forms.NumericUpDown();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.buttonCheckForPresentData = new System.Windows.Forms.Button();
            this.comboBoxCheckForPresenceColumn = new System.Windows.Forms.ComboBox();
            this.buttonExportMissingData = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAnalyseData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 6;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.treeView, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.buttonTestData, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.numericUpDownAnalyseData, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.pictureBox1, 5, 1);
            this.tableLayoutPanel.Controls.Add(this.textBoxMessage, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonSearch, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonCheckForPresentData, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBoxCheckForPresenceColumn, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonExportMissingData, 4, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(680, 434);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // treeView
            // 
            this.tableLayoutPanel.SetColumnSpan(this.treeView, 6);
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(3, 61);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(674, 370);
            this.treeView.TabIndex = 0;
            this.toolTip.SetToolTip(this.treeView, "The result of the import simulation");
            // 
            // buttonTestData
            // 
            this.buttonTestData.Location = new System.Drawing.Point(3, 32);
            this.buttonTestData.Name = "buttonTestData";
            this.buttonTestData.Size = new System.Drawing.Size(93, 23);
            this.buttonTestData.TabIndex = 1;
            this.buttonTestData.Text = "Test data in line:";
            this.buttonTestData.UseVisualStyleBackColor = true;
            this.buttonTestData.Click += new System.EventHandler(this.buttonTestData_Click);
            // 
            // numericUpDownAnalyseData
            // 
            this.numericUpDownAnalyseData.Dock = System.Windows.Forms.DockStyle.Left;
            this.numericUpDownAnalyseData.Location = new System.Drawing.Point(99, 33);
            this.numericUpDownAnalyseData.Margin = new System.Windows.Forms.Padding(0, 4, 3, 3);
            this.numericUpDownAnalyseData.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDownAnalyseData.Name = "numericUpDownAnalyseData";
            this.numericUpDownAnalyseData.Size = new System.Drawing.Size(54, 20);
            this.numericUpDownAnalyseData.TabIndex = 2;
            this.toolTip.SetToolTip(this.numericUpDownAnalyseData, "The line that should be tested");
            this.numericUpDownAnalyseData.ValueChanged += new System.EventHandler(this.numericUpDownAnalyseData_ValueChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::DiversityWorkbench.Properties.Resources.NULL;
            this.pictureBox1.Location = new System.Drawing.Point(679, 29);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1, 29);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel.SetColumnSpan(this.textBoxMessage, 2);
            this.textBoxMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMessage.ForeColor = System.Drawing.Color.Red;
            this.textBoxMessage.Location = new System.Drawing.Point(178, 29);
            this.textBoxMessage.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxMessage.Multiline = true;
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.ReadOnly = true;
            this.textBoxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMessage.Size = new System.Drawing.Size(501, 29);
            this.textBoxMessage.TabIndex = 4;
            this.toolTip.SetToolTip(this.textBoxMessage, "The errors that occured during the import simulation");
            this.textBoxMessage.DoubleClick += new System.EventHandler(this.textBoxMessage_DoubleClick);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSearch.Image = global::DiversityWorkbench.Properties.Resources.Find;
            this.buttonSearch.Location = new System.Drawing.Point(156, 32);
            this.buttonSearch.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(22, 23);
            this.buttonSearch.TabIndex = 5;
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // buttonCheckForPresentData
            // 
            this.tableLayoutPanel.SetColumnSpan(this.buttonCheckForPresentData, 3);
            this.buttonCheckForPresentData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCheckForPresentData.Location = new System.Drawing.Point(3, 3);
            this.buttonCheckForPresentData.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.buttonCheckForPresentData.Name = "buttonCheckForPresentData";
            this.buttonCheckForPresentData.Size = new System.Drawing.Size(175, 23);
            this.buttonCheckForPresentData.TabIndex = 6;
            this.buttonCheckForPresentData.Text = "Check for allready present data";
            this.buttonCheckForPresentData.UseVisualStyleBackColor = true;
            this.buttonCheckForPresentData.Click += new System.EventHandler(this.buttonCheckForPresentData_Click);
            // 
            // comboBoxCheckForPresenceColumn
            // 
            this.comboBoxCheckForPresenceColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxCheckForPresenceColumn.FormattingEnabled = true;
            this.comboBoxCheckForPresenceColumn.Location = new System.Drawing.Point(181, 3);
            this.comboBoxCheckForPresenceColumn.Name = "comboBoxCheckForPresenceColumn";
            this.comboBoxCheckForPresenceColumn.Size = new System.Drawing.Size(328, 21);
            this.comboBoxCheckForPresenceColumn.TabIndex = 7;
            this.comboBoxCheckForPresenceColumn.DropDown += new System.EventHandler(this.comboBoxCheckForPresenceColumn_DropDown);
            this.comboBoxCheckForPresenceColumn.SelectionChangeCommitted += new System.EventHandler(this.comboBoxCheckForPresenceColumn_SelectionChangeCommitted);
            // 
            // buttonExportMissingData
            // 
            this.buttonExportMissingData.Image = global::DiversityWorkbench.Properties.Resources.SaveSmall;
            this.buttonExportMissingData.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonExportMissingData.Location = new System.Drawing.Point(512, 3);
            this.buttonExportMissingData.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.buttonExportMissingData.Name = "buttonExportMissingData";
            this.buttonExportMissingData.Size = new System.Drawing.Size(164, 23);
            this.buttonExportMissingData.TabIndex = 8;
            this.buttonExportMissingData.Text = "Save missing data as text file";
            this.buttonExportMissingData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonExportMissingData.UseVisualStyleBackColor = true;
            this.buttonExportMissingData.Click += new System.EventHandler(this.buttonExportMissingData_Click);
            // 
            // UserControlTesting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlTesting";
            this.Size = new System.Drawing.Size(680, 434);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAnalyseData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Button buttonTestData;
        private System.Windows.Forms.NumericUpDown numericUpDownAnalyseData;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Button buttonCheckForPresentData;
        private System.Windows.Forms.ComboBox comboBoxCheckForPresenceColumn;
        private System.Windows.Forms.Button buttonExportMissingData;
    }
}
