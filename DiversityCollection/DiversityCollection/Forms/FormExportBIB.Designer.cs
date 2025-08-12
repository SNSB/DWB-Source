namespace DiversityCollection.Forms
{
    partial class FormExportBIB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExportBIB));
            tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            textBoxUnit = new System.Windows.Forms.TextBox();
            buttonOpenFileUnit = new System.Windows.Forms.Button();
            textBoxExportFileUnits = new System.Windows.Forms.TextBox();
            buttonOpenFile = new System.Windows.Forms.Button();
            webBrowserBIB = new System.Windows.Forms.WebBrowser();
            textBox = new System.Windows.Forms.TextBox();
            buttonStartExport = new System.Windows.Forms.Button();
            textBoxExportFile = new System.Windows.Forms.TextBox();
            progressBar = new System.Windows.Forms.ProgressBar();
            labelHeader = new System.Windows.Forms.Label();
            openFileDialog = new System.Windows.Forms.OpenFileDialog();
            helpProvider = new System.Windows.Forms.HelpProvider();
            tableLayoutPanelMain.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.ColumnCount = 3;
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.Controls.Add(textBoxUnit, 0, 4);
            tableLayoutPanelMain.Controls.Add(buttonOpenFileUnit, 2, 3);
            tableLayoutPanelMain.Controls.Add(textBoxExportFileUnits, 0, 3);
            tableLayoutPanelMain.Controls.Add(buttonOpenFile, 2, 1);
            tableLayoutPanelMain.Controls.Add(webBrowserBIB, 0, 6);
            tableLayoutPanelMain.Controls.Add(textBox, 0, 2);
            tableLayoutPanelMain.Controls.Add(buttonStartExport, 1, 5);
            tableLayoutPanelMain.Controls.Add(textBoxExportFile, 0, 1);
            tableLayoutPanelMain.Controls.Add(progressBar, 0, 5);
            tableLayoutPanelMain.Controls.Add(labelHeader, 0, 0);
            tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.RowCount = 7;
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanelMain.Size = new System.Drawing.Size(867, 837);
            tableLayoutPanelMain.TabIndex = 0;
            // 
            // textBoxUnit
            // 
            tableLayoutPanelMain.SetColumnSpan(textBoxUnit, 3);
            textBoxUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxUnit.Location = new System.Drawing.Point(4, 378);
            textBoxUnit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxUnit.Multiline = true;
            textBoxUnit.Name = "textBoxUnit";
            textBoxUnit.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            textBoxUnit.Size = new System.Drawing.Size(859, 280);
            textBoxUnit.TabIndex = 8;
            // 
            // buttonOpenFileUnit
            // 
            buttonOpenFileUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonOpenFileUnit.Image = Resource.Open;
            buttonOpenFileUnit.Location = new System.Drawing.Point(833, 345);
            buttonOpenFileUnit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonOpenFileUnit.Name = "buttonOpenFileUnit";
            buttonOpenFileUnit.Size = new System.Drawing.Size(30, 27);
            buttonOpenFileUnit.TabIndex = 7;
            buttonOpenFileUnit.UseVisualStyleBackColor = true;
            buttonOpenFileUnit.Click += buttonOpenFileUnit_Click;
            // 
            // textBoxExportFileUnits
            // 
            tableLayoutPanelMain.SetColumnSpan(textBoxExportFileUnits, 2);
            textBoxExportFileUnits.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxExportFileUnits.Location = new System.Drawing.Point(4, 345);
            textBoxExportFileUnits.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxExportFileUnits.Name = "textBoxExportFileUnits";
            textBoxExportFileUnits.Size = new System.Drawing.Size(821, 23);
            textBoxExportFileUnits.TabIndex = 6;
            // 
            // buttonOpenFile
            // 
            buttonOpenFile.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonOpenFile.Image = Resource.Open;
            buttonOpenFile.Location = new System.Drawing.Point(833, 26);
            buttonOpenFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonOpenFile.Name = "buttonOpenFile";
            buttonOpenFile.Size = new System.Drawing.Size(30, 27);
            buttonOpenFile.TabIndex = 5;
            buttonOpenFile.UseVisualStyleBackColor = true;
            buttonOpenFile.Click += buttonOpenFile_Click;
            // 
            // webBrowserBIB
            // 
            tableLayoutPanelMain.SetColumnSpan(webBrowserBIB, 3);
            webBrowserBIB.Dock = System.Windows.Forms.DockStyle.Fill;
            webBrowserBIB.Location = new System.Drawing.Point(4, 697);
            webBrowserBIB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            webBrowserBIB.MinimumSize = new System.Drawing.Size(23, 23);
            webBrowserBIB.Name = "webBrowserBIB";
            webBrowserBIB.Size = new System.Drawing.Size(859, 137);
            webBrowserBIB.TabIndex = 0;
            // 
            // textBox
            // 
            tableLayoutPanelMain.SetColumnSpan(textBox, 3);
            textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            textBox.Location = new System.Drawing.Point(4, 59);
            textBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox.Multiline = true;
            textBox.Name = "textBox";
            textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            textBox.Size = new System.Drawing.Size(859, 280);
            textBox.TabIndex = 1;
            // 
            // buttonStartExport
            // 
            tableLayoutPanelMain.SetColumnSpan(buttonStartExport, 2);
            buttonStartExport.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonStartExport.Location = new System.Drawing.Point(746, 664);
            buttonStartExport.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonStartExport.Name = "buttonStartExport";
            buttonStartExport.Size = new System.Drawing.Size(117, 27);
            buttonStartExport.TabIndex = 2;
            buttonStartExport.Text = "Start export";
            buttonStartExport.UseVisualStyleBackColor = true;
            buttonStartExport.Click += buttonStartExport_Click;
            // 
            // textBoxExportFile
            // 
            tableLayoutPanelMain.SetColumnSpan(textBoxExportFile, 2);
            textBoxExportFile.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxExportFile.Location = new System.Drawing.Point(4, 26);
            textBoxExportFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxExportFile.Name = "textBoxExportFile";
            textBoxExportFile.Size = new System.Drawing.Size(821, 23);
            textBoxExportFile.TabIndex = 3;
            // 
            // progressBar
            // 
            progressBar.Dock = System.Windows.Forms.DockStyle.Top;
            progressBar.Location = new System.Drawing.Point(4, 664);
            progressBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            progressBar.Name = "progressBar";
            progressBar.Size = new System.Drawing.Size(734, 27);
            progressBar.TabIndex = 4;
            // 
            // labelHeader
            // 
            labelHeader.AutoSize = true;
            tableLayoutPanelMain.SetColumnSpan(labelHeader, 3);
            labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            labelHeader.Location = new System.Drawing.Point(4, 0);
            labelHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new System.Drawing.Size(859, 23);
            labelHeader.TabIndex = 9;
            labelHeader.Text = "Click on the Start export button to create the export files";
            labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog";
            // 
            // FormExportBIB
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(867, 837);
            Controls.Add(tableLayoutPanelMain);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormExportBIB";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Export für den Botanischen Informationsknoten Bayern";
            KeyDown += Form_KeyDown;
            tableLayoutPanelMain.ResumeLayout(false);
            tableLayoutPanelMain.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.WebBrowser webBrowserBIB;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Button buttonStartExport;
        private System.Windows.Forms.TextBox textBoxExportFile;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox textBoxUnit;
        private System.Windows.Forms.Button buttonOpenFileUnit;
        private System.Windows.Forms.TextBox textBoxExportFileUnits;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}