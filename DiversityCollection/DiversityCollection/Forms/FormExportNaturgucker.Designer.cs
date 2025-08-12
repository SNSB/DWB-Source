namespace DiversityCollection.Forms
{
    partial class FormExportNaturgucker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExportNaturgucker));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.textBox = new System.Windows.Forms.TextBox();
            this.buttonStartExport = new System.Windows.Forms.Button();
            this.textBoxExportFile = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelHeader = new System.Windows.Forms.Label();
            this.checkedListBoxTaxonomicGroups = new System.Windows.Forms.CheckedListBox();
            this.labelTaxonomicGroups = new System.Windows.Forms.Label();
            this.labelResult = new System.Windows.Forms.Label();
            this.labelAnalysis = new System.Windows.Forms.Label();
            this.checkedListBoxAnalysis = new System.Windows.Forms.CheckedListBox();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelMain.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 4;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.buttonOpenFile, 3, 1);
            this.tableLayoutPanelMain.Controls.Add(this.textBox, 1, 3);
            this.tableLayoutPanelMain.Controls.Add(this.buttonStartExport, 2, 6);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxExportFile, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.progressBar, 0, 6);
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.checkedListBoxTaxonomicGroups, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelTaxonomicGroups, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelResult, 1, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelAnalysis, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.checkedListBoxAnalysis, 0, 5);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 7;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(775, 454);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOpenFile.Image = global::DiversityCollection.Resource.Open;
            this.buttonOpenFile.Location = new System.Drawing.Point(747, 23);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(25, 21);
            this.buttonOpenFile.TabIndex = 5;
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // textBox
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBox, 3);
            this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox.Location = new System.Drawing.Point(129, 63);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.tableLayoutPanelMain.SetRowSpan(this.textBox, 3);
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox.Size = new System.Drawing.Size(643, 359);
            this.textBox.TabIndex = 1;
            // 
            // buttonStartExport
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonStartExport, 2);
            this.buttonStartExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonStartExport.Location = new System.Drawing.Point(672, 428);
            this.buttonStartExport.Name = "buttonStartExport";
            this.buttonStartExport.Size = new System.Drawing.Size(100, 23);
            this.buttonStartExport.TabIndex = 2;
            this.buttonStartExport.Text = "Start export";
            this.buttonStartExport.UseVisualStyleBackColor = true;
            this.buttonStartExport.Click += new System.EventHandler(this.buttonStartExport_Click);
            // 
            // textBoxExportFile
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxExportFile, 3);
            this.textBoxExportFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxExportFile.Location = new System.Drawing.Point(3, 23);
            this.textBoxExportFile.Name = "textBoxExportFile";
            this.textBoxExportFile.Size = new System.Drawing.Size(738, 20);
            this.textBoxExportFile.TabIndex = 3;
            // 
            // progressBar
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.progressBar, 2);
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBar.Location = new System.Drawing.Point(3, 428);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(663, 23);
            this.progressBar.TabIndex = 4;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelHeader, 4);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(769, 20);
            this.labelHeader.TabIndex = 9;
            this.labelHeader.Text = "Click on the Start export button to create the export files";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkedListBoxTaxonomicGroups
            // 
            this.checkedListBoxTaxonomicGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxTaxonomicGroups.FormattingEnabled = true;
            this.checkedListBoxTaxonomicGroups.IntegralHeight = false;
            this.checkedListBoxTaxonomicGroups.Location = new System.Drawing.Point(3, 63);
            this.checkedListBoxTaxonomicGroups.Name = "checkedListBoxTaxonomicGroups";
            this.checkedListBoxTaxonomicGroups.Size = new System.Drawing.Size(120, 170);
            this.checkedListBoxTaxonomicGroups.TabIndex = 12;
            // 
            // labelTaxonomicGroups
            // 
            this.labelTaxonomicGroups.AutoSize = true;
            this.labelTaxonomicGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTaxonomicGroups.Location = new System.Drawing.Point(3, 47);
            this.labelTaxonomicGroups.Name = "labelTaxonomicGroups";
            this.labelTaxonomicGroups.Size = new System.Drawing.Size(120, 13);
            this.labelTaxonomicGroups.TabIndex = 13;
            this.labelTaxonomicGroups.Text = "Taxonomic groups";
            this.labelTaxonomicGroups.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelResult
            // 
            this.labelResult.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelResult, 3);
            this.labelResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelResult.Location = new System.Drawing.Point(129, 47);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(643, 13);
            this.labelResult.TabIndex = 14;
            this.labelResult.Text = "Export result";
            this.labelResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelAnalysis
            // 
            this.labelAnalysis.AutoSize = true;
            this.labelAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAnalysis.Location = new System.Drawing.Point(3, 236);
            this.labelAnalysis.Name = "labelAnalysis";
            this.labelAnalysis.Size = new System.Drawing.Size(120, 13);
            this.labelAnalysis.TabIndex = 15;
            this.labelAnalysis.Text = "Analysis";
            this.labelAnalysis.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // checkedListBoxAnalysis
            // 
            this.checkedListBoxAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxAnalysis.FormattingEnabled = true;
            this.checkedListBoxAnalysis.IntegralHeight = false;
            this.checkedListBoxAnalysis.Location = new System.Drawing.Point(3, 252);
            this.checkedListBoxAnalysis.Name = "checkedListBoxAnalysis";
            this.checkedListBoxAnalysis.Size = new System.Drawing.Size(120, 170);
            this.checkedListBoxAnalysis.TabIndex = 16;
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(0, 0);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(775, 179);
            this.webBrowser.TabIndex = 0;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tableLayoutPanelMain);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.webBrowser);
            this.splitContainer.Size = new System.Drawing.Size(775, 637);
            this.splitContainer.SplitterDistance = 454;
            this.splitContainer.TabIndex = 2;
            // 
            // FormExportNaturgucker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 637);
            this.Controls.Add(this.splitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormExportNaturgucker";
            this.Text = "Export Naturgucker";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Button buttonStartExport;
        private System.Windows.Forms.TextBox textBoxExportFile;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.CheckedListBox checkedListBoxTaxonomicGroups;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label labelTaxonomicGroups;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.Label labelAnalysis;
        private System.Windows.Forms.CheckedListBox checkedListBoxAnalysis;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.SplitContainer splitContainer;
    }
}