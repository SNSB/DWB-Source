namespace DiversityCollection.UserControls
{
    partial class UserControlImportTable
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
            this.labelTableName = new System.Windows.Forms.Label();
            this.radioButtonInsert = new System.Windows.Forms.RadioButton();
            this.radioButtonUpdate = new System.Windows.Forms.RadioButton();
            this.radioButtonMerge = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanelErrorHandling = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonErrorHandlingAutomaticNone = new System.Windows.Forms.RadioButton();
            this.labelErrorHandlingAutomaticNone = new System.Windows.Forms.Label();
            this.labelErrorHandling = new System.Windows.Forms.Label();
            this.radioButtonErrorHandlingManual = new System.Windows.Forms.RadioButton();
            this.radioButtonErrorHandlingAutomaticFirst = new System.Windows.Forms.RadioButton();
            this.labelErrorHandlingAutomaticFirst = new System.Windows.Forms.Label();
            this.radioButtonErrorHandlingAutomaticLast = new System.Windows.Forms.RadioButton();
            this.labelErrorTreatmentAutomatic = new System.Windows.Forms.Label();
            this.labelErrorHandlingAutomaticLast = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanel.SuspendLayout();
            this.tableLayoutPanelErrorHandling.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 5;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Controls.Add(this.labelTableName, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.radioButtonInsert, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.radioButtonUpdate, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.radioButtonMerge, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanelErrorHandling, 4, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(498, 32);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelTableName
            // 
            this.labelTableName.AutoSize = true;
            this.labelTableName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTableName.Location = new System.Drawing.Point(3, 0);
            this.labelTableName.Name = "labelTableName";
            this.labelTableName.Size = new System.Drawing.Size(128, 32);
            this.labelTableName.TabIndex = 0;
            this.labelTableName.Text = "Table";
            this.labelTableName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // radioButtonInsert
            // 
            this.radioButtonInsert.AutoSize = true;
            this.radioButtonInsert.Checked = true;
            this.radioButtonInsert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonInsert.Location = new System.Drawing.Point(134, 3);
            this.radioButtonInsert.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.radioButtonInsert.Name = "radioButtonInsert";
            this.radioButtonInsert.Size = new System.Drawing.Size(60, 26);
            this.radioButtonInsert.TabIndex = 1;
            this.radioButtonInsert.TabStop = true;
            this.radioButtonInsert.Text = "Insert";
            this.toolTip.SetToolTip(this.radioButtonInsert, "Insert as new data");
            this.radioButtonInsert.UseVisualStyleBackColor = true;
            this.radioButtonInsert.Click += new System.EventHandler(this.radioButtonInsert_Click);
            // 
            // radioButtonUpdate
            // 
            this.radioButtonUpdate.AutoSize = true;
            this.radioButtonUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonUpdate.Location = new System.Drawing.Point(254, 3);
            this.radioButtonUpdate.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.radioButtonUpdate.Name = "radioButtonUpdate";
            this.radioButtonUpdate.Size = new System.Drawing.Size(60, 26);
            this.radioButtonUpdate.TabIndex = 2;
            this.radioButtonUpdate.Text = "Update";
            this.toolTip.SetToolTip(this.radioButtonUpdate, "Update existing data. Insert only if no data exist");
            this.radioButtonUpdate.UseVisualStyleBackColor = true;
            this.radioButtonUpdate.Click += new System.EventHandler(this.radioButtonUpdate_Click);
            // 
            // radioButtonMerge
            // 
            this.radioButtonMerge.AutoSize = true;
            this.radioButtonMerge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonMerge.Location = new System.Drawing.Point(194, 3);
            this.radioButtonMerge.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.radioButtonMerge.Name = "radioButtonMerge";
            this.radioButtonMerge.Size = new System.Drawing.Size(60, 26);
            this.radioButtonMerge.TabIndex = 3;
            this.radioButtonMerge.Text = "Merge";
            this.toolTip.SetToolTip(this.radioButtonMerge, "Insert data if missing, otherwise update data");
            this.radioButtonMerge.UseVisualStyleBackColor = true;
            this.radioButtonMerge.Click += new System.EventHandler(this.radioButtonMerge_Click);
            // 
            // tableLayoutPanelErrorHandling
            // 
            this.tableLayoutPanelErrorHandling.ColumnCount = 5;
            this.tableLayoutPanelErrorHandling.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelErrorHandling.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelErrorHandling.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanelErrorHandling.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanelErrorHandling.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanelErrorHandling.Controls.Add(this.radioButtonErrorHandlingAutomaticNone, 2, 1);
            this.tableLayoutPanelErrorHandling.Controls.Add(this.labelErrorHandlingAutomaticNone, 2, 0);
            this.tableLayoutPanelErrorHandling.Controls.Add(this.labelErrorHandling, 0, 0);
            this.tableLayoutPanelErrorHandling.Controls.Add(this.radioButtonErrorHandlingManual, 0, 1);
            this.tableLayoutPanelErrorHandling.Controls.Add(this.radioButtonErrorHandlingAutomaticFirst, 3, 1);
            this.tableLayoutPanelErrorHandling.Controls.Add(this.labelErrorHandlingAutomaticFirst, 3, 0);
            this.tableLayoutPanelErrorHandling.Controls.Add(this.radioButtonErrorHandlingAutomaticLast, 4, 1);
            this.tableLayoutPanelErrorHandling.Controls.Add(this.labelErrorTreatmentAutomatic, 1, 1);
            this.tableLayoutPanelErrorHandling.Controls.Add(this.labelErrorHandlingAutomaticLast, 4, 0);
            this.tableLayoutPanelErrorHandling.Location = new System.Drawing.Point(314, 0);
            this.tableLayoutPanelErrorHandling.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelErrorHandling.Name = "tableLayoutPanelErrorHandling";
            this.tableLayoutPanelErrorHandling.RowCount = 2;
            this.tableLayoutPanelErrorHandling.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelErrorHandling.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelErrorHandling.Size = new System.Drawing.Size(184, 32);
            this.tableLayoutPanelErrorHandling.TabIndex = 5;
            // 
            // radioButtonErrorHandlingAutomaticNone
            // 
            this.radioButtonErrorHandlingAutomaticNone.AutoSize = true;
            this.radioButtonErrorHandlingAutomaticNone.Checked = true;
            this.radioButtonErrorHandlingAutomaticNone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonErrorHandlingAutomaticNone.Location = new System.Drawing.Point(94, 13);
            this.radioButtonErrorHandlingAutomaticNone.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.radioButtonErrorHandlingAutomaticNone.Name = "radioButtonErrorHandlingAutomaticNone";
            this.radioButtonErrorHandlingAutomaticNone.Size = new System.Drawing.Size(26, 19);
            this.radioButtonErrorHandlingAutomaticNone.TabIndex = 1;
            this.radioButtonErrorHandlingAutomaticNone.TabStop = true;
            this.toolTip.SetToolTip(this.radioButtonErrorHandlingAutomaticNone, "Autonom error handling: Ignore data in file if there are several matching entries" +
        "");
            this.radioButtonErrorHandlingAutomaticNone.UseVisualStyleBackColor = true;
            this.radioButtonErrorHandlingAutomaticNone.Click += new System.EventHandler(this.radioButtonErrorHandlingAutomaticNone_Click);
            // 
            // labelErrorHandlingAutomaticNone
            // 
            this.labelErrorHandlingAutomaticNone.AutoSize = true;
            this.labelErrorHandlingAutomaticNone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelErrorHandlingAutomaticNone.ForeColor = System.Drawing.Color.OrangeRed;
            this.labelErrorHandlingAutomaticNone.Location = new System.Drawing.Point(88, 0);
            this.labelErrorHandlingAutomaticNone.Margin = new System.Windows.Forms.Padding(0);
            this.labelErrorHandlingAutomaticNone.Name = "labelErrorHandlingAutomaticNone";
            this.labelErrorHandlingAutomaticNone.Size = new System.Drawing.Size(32, 13);
            this.labelErrorHandlingAutomaticNone.TabIndex = 0;
            this.labelErrorHandlingAutomaticNone.Text = "none";
            // 
            // labelErrorHandling
            // 
            this.labelErrorHandling.AutoSize = true;
            this.tableLayoutPanelErrorHandling.SetColumnSpan(this.labelErrorHandling, 2);
            this.labelErrorHandling.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelErrorHandling.ForeColor = System.Drawing.Color.Red;
            this.labelErrorHandling.Location = new System.Drawing.Point(0, 0);
            this.labelErrorHandling.Margin = new System.Windows.Forms.Padding(0);
            this.labelErrorHandling.Name = "labelErrorHandling";
            this.labelErrorHandling.Size = new System.Drawing.Size(88, 13);
            this.labelErrorHandling.TabIndex = 6;
            this.labelErrorHandling.Text = "Error handling";
            this.labelErrorHandling.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // radioButtonErrorHandlingManual
            // 
            this.radioButtonErrorHandlingManual.AutoSize = true;
            this.radioButtonErrorHandlingManual.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonErrorHandlingManual.ForeColor = System.Drawing.Color.Red;
            this.radioButtonErrorHandlingManual.Location = new System.Drawing.Point(0, 13);
            this.radioButtonErrorHandlingManual.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonErrorHandlingManual.Name = "radioButtonErrorHandlingManual";
            this.radioButtonErrorHandlingManual.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radioButtonErrorHandlingManual.Size = new System.Drawing.Size(48, 19);
            this.radioButtonErrorHandlingManual.TabIndex = 7;
            this.radioButtonErrorHandlingManual.Text = ".man";
            this.toolTip.SetToolTip(this.radioButtonErrorHandlingManual, "Manual error handling. Decide for every error, what should be done");
            this.radioButtonErrorHandlingManual.UseVisualStyleBackColor = true;
            this.radioButtonErrorHandlingManual.Click += new System.EventHandler(this.radioButtonErrorHandlingManual_Click);
            // 
            // radioButtonErrorHandlingAutomaticFirst
            // 
            this.radioButtonErrorHandlingAutomaticFirst.AutoSize = true;
            this.radioButtonErrorHandlingAutomaticFirst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonErrorHandlingAutomaticFirst.Location = new System.Drawing.Point(126, 16);
            this.radioButtonErrorHandlingAutomaticFirst.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.radioButtonErrorHandlingAutomaticFirst.Name = "radioButtonErrorHandlingAutomaticFirst";
            this.radioButtonErrorHandlingAutomaticFirst.Size = new System.Drawing.Size(23, 13);
            this.radioButtonErrorHandlingAutomaticFirst.TabIndex = 3;
            this.toolTip.SetToolTip(this.radioButtonErrorHandlingAutomaticFirst, "Autonom error handling: Choose the first entry if there are several matching entr" +
        "ies");
            this.radioButtonErrorHandlingAutomaticFirst.UseVisualStyleBackColor = true;
            this.radioButtonErrorHandlingAutomaticFirst.Click += new System.EventHandler(this.radioButtonErrorHandlingAutomaticFirst_Click);
            // 
            // labelErrorHandlingAutomaticFirst
            // 
            this.labelErrorHandlingAutomaticFirst.AutoSize = true;
            this.labelErrorHandlingAutomaticFirst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelErrorHandlingAutomaticFirst.ForeColor = System.Drawing.Color.OrangeRed;
            this.labelErrorHandlingAutomaticFirst.Location = new System.Drawing.Point(120, 0);
            this.labelErrorHandlingAutomaticFirst.Margin = new System.Windows.Forms.Padding(0);
            this.labelErrorHandlingAutomaticFirst.Name = "labelErrorHandlingAutomaticFirst";
            this.labelErrorHandlingAutomaticFirst.Size = new System.Drawing.Size(32, 13);
            this.labelErrorHandlingAutomaticFirst.TabIndex = 2;
            this.labelErrorHandlingAutomaticFirst.Text = "first";
            // 
            // radioButtonErrorHandlingAutomaticLast
            // 
            this.radioButtonErrorHandlingAutomaticLast.AutoSize = true;
            this.radioButtonErrorHandlingAutomaticLast.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonErrorHandlingAutomaticLast.Location = new System.Drawing.Point(158, 16);
            this.radioButtonErrorHandlingAutomaticLast.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.radioButtonErrorHandlingAutomaticLast.Name = "radioButtonErrorHandlingAutomaticLast";
            this.radioButtonErrorHandlingAutomaticLast.Size = new System.Drawing.Size(23, 13);
            this.radioButtonErrorHandlingAutomaticLast.TabIndex = 5;
            this.toolTip.SetToolTip(this.radioButtonErrorHandlingAutomaticLast, "Autonom error handling: Choose the last entry if there are several matching entri" +
        "es");
            this.radioButtonErrorHandlingAutomaticLast.UseVisualStyleBackColor = true;
            this.radioButtonErrorHandlingAutomaticLast.Click += new System.EventHandler(this.radioButtonErrorHandlingAutomaticLast_Click);
            // 
            // labelErrorTreatmentAutomatic
            // 
            this.labelErrorTreatmentAutomatic.AutoSize = true;
            this.labelErrorTreatmentAutomatic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelErrorTreatmentAutomatic.ForeColor = System.Drawing.Color.OrangeRed;
            this.labelErrorTreatmentAutomatic.Location = new System.Drawing.Point(57, 13);
            this.labelErrorTreatmentAutomatic.Margin = new System.Windows.Forms.Padding(0);
            this.labelErrorTreatmentAutomatic.Name = "labelErrorTreatmentAutomatic";
            this.labelErrorTreatmentAutomatic.Size = new System.Drawing.Size(31, 19);
            this.labelErrorTreatmentAutomatic.TabIndex = 10;
            this.labelErrorTreatmentAutomatic.Text = "auto:";
            this.labelErrorTreatmentAutomatic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelErrorHandlingAutomaticLast
            // 
            this.labelErrorHandlingAutomaticLast.AutoSize = true;
            this.labelErrorHandlingAutomaticLast.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelErrorHandlingAutomaticLast.ForeColor = System.Drawing.Color.OrangeRed;
            this.labelErrorHandlingAutomaticLast.Location = new System.Drawing.Point(152, 0);
            this.labelErrorHandlingAutomaticLast.Margin = new System.Windows.Forms.Padding(0);
            this.labelErrorHandlingAutomaticLast.Name = "labelErrorHandlingAutomaticLast";
            this.labelErrorHandlingAutomaticLast.Size = new System.Drawing.Size(32, 13);
            this.labelErrorHandlingAutomaticLast.TabIndex = 4;
            this.labelErrorHandlingAutomaticLast.Text = "last";
            // 
            // UserControlImportTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.helpProvider.SetHelpString(this, "Import wizard");
            this.Name = "UserControlImportTable";
            this.helpProvider.SetShowHelp(this, true);
            this.Size = new System.Drawing.Size(498, 32);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.tableLayoutPanelErrorHandling.ResumeLayout(false);
            this.tableLayoutPanelErrorHandling.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelTableName;
        private System.Windows.Forms.RadioButton radioButtonInsert;
        private System.Windows.Forms.RadioButton radioButtonUpdate;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.RadioButton radioButtonMerge;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelErrorHandling;
        private System.Windows.Forms.Label labelErrorHandling;
        private System.Windows.Forms.RadioButton radioButtonErrorHandlingManual;
        private System.Windows.Forms.Label labelErrorHandlingAutomaticNone;
        private System.Windows.Forms.RadioButton radioButtonErrorHandlingAutomaticNone;
        private System.Windows.Forms.Label labelErrorHandlingAutomaticFirst;
        private System.Windows.Forms.RadioButton radioButtonErrorHandlingAutomaticFirst;
        private System.Windows.Forms.Label labelErrorHandlingAutomaticLast;
        private System.Windows.Forms.RadioButton radioButtonErrorHandlingAutomaticLast;
        private System.Windows.Forms.Label labelErrorTreatmentAutomatic;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}
