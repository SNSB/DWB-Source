namespace DiversityWorkbench.Import
{
    partial class UserControlImport
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
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.buttonStartImport = new System.Windows.Forms.Button();
            this.buttonShowSchema = new System.Windows.Forms.Button();
            this.textBoxSchemaFile = new System.Windows.Forms.TextBox();
            this.checkBoxSaveFailedLines = new System.Windows.Forms.CheckBox();
            this.buttonOpenErrorFile = new System.Windows.Forms.Button();
            this.textBoxFailedLinesFileName = new System.Windows.Forms.TextBox();
            this.buttonCreateSchemaDescription = new System.Windows.Forms.Button();
            this.checkBoxIncludeDescription = new System.Windows.Forms.CheckBox();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 6;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.webBrowser, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonStartImport, 4, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonShowSchema, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxSchemaFile, 2, 0);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxSaveFailedLines, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.buttonOpenErrorFile, 5, 3);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxFailedLinesFileName, 2, 3);
            this.tableLayoutPanelMain.Controls.Add(this.buttonCreateSchemaDescription, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxIncludeDescription, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxDescription, 2, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 4;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(570, 420);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // webBrowser
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.webBrowser, 6);
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(3, 58);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(564, 330);
            this.webBrowser.TabIndex = 1;
            // 
            // buttonStartImport
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonStartImport, 2);
            this.buttonStartImport.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonStartImport.Image = global::DiversityWorkbench.Properties.Resources.ImportWizard2;
            this.buttonStartImport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonStartImport.Location = new System.Drawing.Point(480, 3);
            this.buttonStartImport.Name = "buttonStartImport";
            this.buttonStartImport.Size = new System.Drawing.Size(87, 23);
            this.buttonStartImport.TabIndex = 0;
            this.buttonStartImport.Text = "Start import";
            this.buttonStartImport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.buttonStartImport, "Start the import of the data into the database");
            this.buttonStartImport.UseVisualStyleBackColor = true;
            this.buttonStartImport.Click += new System.EventHandler(this.buttonStartImport_Click);
            // 
            // buttonShowSchema
            // 
            this.buttonShowSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShowSchema.Image = global::DiversityWorkbench.Properties.Resources.Save;
            this.buttonShowSchema.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonShowSchema.Location = new System.Drawing.Point(29, 3);
            this.buttonShowSchema.Margin = new System.Windows.Forms.Padding(0, 3, 0, 2);
            this.buttonShowSchema.Name = "buttonShowSchema";
            this.buttonShowSchema.Size = new System.Drawing.Size(94, 24);
            this.buttonShowSchema.TabIndex = 2;
            this.buttonShowSchema.Text = "Save schema";
            this.buttonShowSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.buttonShowSchema, "Save the current schema for the import for later reuse of data with identical str" +
        "ucture");
            this.buttonShowSchema.UseVisualStyleBackColor = true;
            this.buttonShowSchema.Click += new System.EventHandler(this.buttonShowSchema_Click);
            // 
            // textBoxSchemaFile
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxSchemaFile, 2);
            this.textBoxSchemaFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSchemaFile.Location = new System.Drawing.Point(126, 3);
            this.textBoxSchemaFile.Name = "textBoxSchemaFile";
            this.textBoxSchemaFile.Size = new System.Drawing.Size(348, 20);
            this.textBoxSchemaFile.TabIndex = 3;
            this.toolTip.SetToolTip(this.textBoxSchemaFile, "The path and file name of the import schema");
            this.textBoxSchemaFile.TextChanged += new System.EventHandler(this.textBoxSchemaFile_TextChanged);
            // 
            // checkBoxSaveFailedLines
            // 
            this.checkBoxSaveFailedLines.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.checkBoxSaveFailedLines, 2);
            this.checkBoxSaveFailedLines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxSaveFailedLines.Location = new System.Drawing.Point(3, 394);
            this.checkBoxSaveFailedLines.Name = "checkBoxSaveFailedLines";
            this.checkBoxSaveFailedLines.Size = new System.Drawing.Size(117, 23);
            this.checkBoxSaveFailedLines.TabIndex = 4;
            this.checkBoxSaveFailedLines.Text = "Save failed lines";
            this.toolTip.SetToolTip(this.checkBoxSaveFailedLines, "If lines where the import failed should be written into a separate file for a ret" +
        "rial of the import");
            this.checkBoxSaveFailedLines.UseVisualStyleBackColor = true;
            this.checkBoxSaveFailedLines.Click += new System.EventHandler(this.checkBoxSaveFailedLines_Click);
            // 
            // buttonOpenErrorFile
            // 
            this.buttonOpenErrorFile.Image = global::DiversityWorkbench.ResourceWorkbench.Open;
            this.buttonOpenErrorFile.Location = new System.Drawing.Point(543, 394);
            this.buttonOpenErrorFile.Name = "buttonOpenErrorFile";
            this.buttonOpenErrorFile.Size = new System.Drawing.Size(24, 23);
            this.buttonOpenErrorFile.TabIndex = 5;
            this.toolTip.SetToolTip(this.buttonOpenErrorFile, "Open the file with the failed lines");
            this.buttonOpenErrorFile.UseVisualStyleBackColor = true;
            this.buttonOpenErrorFile.Click += new System.EventHandler(this.buttonOpenErrorFile_Click);
            // 
            // textBoxFailedLinesFileName
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxFailedLinesFileName, 3);
            this.textBoxFailedLinesFileName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxFailedLinesFileName.Location = new System.Drawing.Point(126, 394);
            this.textBoxFailedLinesFileName.Name = "textBoxFailedLinesFileName";
            this.textBoxFailedLinesFileName.ReadOnly = true;
            this.textBoxFailedLinesFileName.Size = new System.Drawing.Size(411, 20);
            this.textBoxFailedLinesFileName.TabIndex = 6;
            // 
            // buttonCreateSchemaDescription
            // 
            this.buttonCreateSchemaDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCreateSchemaDescription.Image = global::DiversityWorkbench.Properties.Resources.Manual;
            this.buttonCreateSchemaDescription.Location = new System.Drawing.Point(3, 3);
            this.buttonCreateSchemaDescription.Margin = new System.Windows.Forms.Padding(3, 3, 3, 2);
            this.buttonCreateSchemaDescription.Name = "buttonCreateSchemaDescription";
            this.buttonCreateSchemaDescription.Size = new System.Drawing.Size(23, 24);
            this.buttonCreateSchemaDescription.TabIndex = 7;
            this.toolTip.SetToolTip(this.buttonCreateSchemaDescription, "Create a description of the schema");
            this.buttonCreateSchemaDescription.UseVisualStyleBackColor = true;
            this.buttonCreateSchemaDescription.Click += new System.EventHandler(this.buttonCreateSchemaDescription_Click);
            // 
            // checkBoxIncludeDescription
            // 
            this.checkBoxIncludeDescription.AutoSize = true;
            this.checkBoxIncludeDescription.Checked = true;
            this.checkBoxIncludeDescription.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanelMain.SetColumnSpan(this.checkBoxIncludeDescription, 2);
            this.checkBoxIncludeDescription.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxIncludeDescription.Location = new System.Drawing.Point(5, 32);
            this.checkBoxIncludeDescription.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.checkBoxIncludeDescription.Name = "checkBoxIncludeDescription";
            this.checkBoxIncludeDescription.Size = new System.Drawing.Size(118, 20);
            this.checkBoxIncludeDescription.TabIndex = 8;
            this.checkBoxIncludeDescription.Text = "Include description:";
            this.toolTip.SetToolTip(this.checkBoxIncludeDescription, "Include a description in the schema");
            this.checkBoxIncludeDescription.UseVisualStyleBackColor = true;
            // 
            // textBoxDescription
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxDescription, 4);
            this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDescription.Location = new System.Drawing.Point(126, 32);
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(441, 20);
            this.textBoxDescription.TabIndex = 10;
            this.toolTip.SetToolTip(this.textBoxDescription, "A short description of the schema");
            this.textBoxDescription.Leave += new System.EventHandler(this.textBoxDescription_Leave);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // UserControlImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Name = "UserControlImport";
            this.Size = new System.Drawing.Size(570, 420);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.Button buttonStartImport;
        private System.Windows.Forms.Button buttonShowSchema;
        private System.Windows.Forms.TextBox textBoxSchemaFile;
        private System.Windows.Forms.CheckBox checkBoxSaveFailedLines;
        private System.Windows.Forms.Button buttonOpenErrorFile;
        private System.Windows.Forms.TextBox textBoxFailedLinesFileName;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonCreateSchemaDescription;
        private System.Windows.Forms.CheckBox checkBoxIncludeDescription;
        private System.Windows.Forms.TextBox textBoxDescription;
    }
}
