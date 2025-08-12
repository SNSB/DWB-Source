namespace DiversityWorkbench.Archive
{
    partial class FormCreateArchive
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCreateArchive));
            tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            labelHeader = new System.Windows.Forms.Label();
            comboBoxRootSelection = new System.Windows.Forms.ComboBox();
            panelTables = new System.Windows.Forms.Panel();
            buttonCreateArchive = new System.Windows.Forms.Button();
            buttonQueryData = new System.Windows.Forms.Button();
            buttonDirectory = new System.Windows.Forms.Button();
            textBoxArchiveFolder = new System.Windows.Forms.TextBox();
            buttonFeedback = new System.Windows.Forms.Button();
            checkBoxIncludeLog = new System.Windows.Forms.CheckBox();
            progressBar = new System.Windows.Forms.ProgressBar();
            toolTip = new System.Windows.Forms.ToolTip(components);
            folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            helpProvider = new System.Windows.Forms.HelpProvider();
            tableLayoutPanelMain.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.ColumnCount = 3;
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.Controls.Add(labelHeader, 1, 0);
            tableLayoutPanelMain.Controls.Add(comboBoxRootSelection, 0, 1);
            tableLayoutPanelMain.Controls.Add(panelTables, 0, 2);
            tableLayoutPanelMain.Controls.Add(buttonCreateArchive, 2, 4);
            tableLayoutPanelMain.Controls.Add(buttonQueryData, 2, 1);
            tableLayoutPanelMain.Controls.Add(buttonDirectory, 0, 3);
            tableLayoutPanelMain.Controls.Add(textBoxArchiveFolder, 1, 3);
            tableLayoutPanelMain.Controls.Add(buttonFeedback, 0, 0);
            tableLayoutPanelMain.Controls.Add(checkBoxIncludeLog, 0, 4);
            tableLayoutPanelMain.Controls.Add(progressBar, 0, 5);
            tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.RowCount = 6;
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            tableLayoutPanelMain.Size = new System.Drawing.Size(476, 571);
            tableLayoutPanelMain.TabIndex = 0;
            // 
            // labelHeader
            // 
            labelHeader.AutoSize = true;
            tableLayoutPanelMain.SetColumnSpan(labelHeader, 2);
            labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            labelHeader.Location = new System.Drawing.Point(42, 0);
            labelHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new System.Drawing.Size(430, 23);
            labelHeader.TabIndex = 0;
            labelHeader.Text = "label1";
            labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBoxRootSelection
            // 
            tableLayoutPanelMain.SetColumnSpan(comboBoxRootSelection, 2);
            comboBoxRootSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxRootSelection.FormattingEnabled = true;
            comboBoxRootSelection.Location = new System.Drawing.Point(4, 26);
            comboBoxRootSelection.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxRootSelection.Name = "comboBoxRootSelection";
            comboBoxRootSelection.Size = new System.Drawing.Size(325, 23);
            comboBoxRootSelection.TabIndex = 1;
            toolTip.SetToolTip(comboBoxRootSelection, "The project of which an archive should be created");
            comboBoxRootSelection.SelectedIndexChanged += comboBoxRootSelection_SelectedIndexChanged;
            // 
            // panelTables
            // 
            panelTables.AutoScroll = true;
            tableLayoutPanelMain.SetColumnSpan(panelTables, 3);
            panelTables.Dock = System.Windows.Forms.DockStyle.Fill;
            panelTables.Location = new System.Drawing.Point(4, 59);
            panelTables.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panelTables.Name = "panelTables";
            panelTables.Size = new System.Drawing.Size(468, 436);
            panelTables.TabIndex = 3;
            // 
            // buttonCreateArchive
            // 
            buttonCreateArchive.Dock = System.Windows.Forms.DockStyle.Right;
            buttonCreateArchive.Enabled = false;
            buttonCreateArchive.Image = Properties.Resources.ArchivCreate;
            buttonCreateArchive.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            buttonCreateArchive.Location = new System.Drawing.Point(337, 534);
            buttonCreateArchive.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonCreateArchive.Name = "buttonCreateArchive";
            buttonCreateArchive.Size = new System.Drawing.Size(135, 27);
            buttonCreateArchive.TabIndex = 4;
            buttonCreateArchive.Text = "Create the archive";
            buttonCreateArchive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            toolTip.SetToolTip(buttonCreateArchive, "Create the archive of the project");
            buttonCreateArchive.UseVisualStyleBackColor = true;
            buttonCreateArchive.Click += buttonCreateArchive_Click;
            // 
            // buttonQueryData
            // 
            buttonQueryData.Dock = System.Windows.Forms.DockStyle.Right;
            buttonQueryData.Image = Properties.Resources.Find;
            buttonQueryData.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            buttonQueryData.Location = new System.Drawing.Point(337, 26);
            buttonQueryData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonQueryData.Name = "buttonQueryData";
            buttonQueryData.Size = new System.Drawing.Size(135, 27);
            buttonQueryData.TabIndex = 2;
            buttonQueryData.Text = "Find the data";
            buttonQueryData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            toolTip.SetToolTip(buttonQueryData, "Find all data related to the project");
            buttonQueryData.UseVisualStyleBackColor = true;
            buttonQueryData.Click += buttonQueryData_Click;
            // 
            // buttonDirectory
            // 
            buttonDirectory.Dock = System.Windows.Forms.DockStyle.Right;
            buttonDirectory.Image = Properties.Resources.OpenFolder;
            buttonDirectory.Location = new System.Drawing.Point(4, 501);
            buttonDirectory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonDirectory.Name = "buttonDirectory";
            buttonDirectory.Size = new System.Drawing.Size(30, 27);
            buttonDirectory.TabIndex = 5;
            toolTip.SetToolTip(buttonDirectory, "Open archive directory");
            buttonDirectory.UseVisualStyleBackColor = true;
            buttonDirectory.Click += buttonDirectory_Click;
            // 
            // textBoxArchiveFolder
            // 
            tableLayoutPanelMain.SetColumnSpan(textBoxArchiveFolder, 2);
            textBoxArchiveFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxArchiveFolder.Location = new System.Drawing.Point(42, 501);
            textBoxArchiveFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxArchiveFolder.Name = "textBoxArchiveFolder";
            textBoxArchiveFolder.ReadOnly = true;
            textBoxArchiveFolder.Size = new System.Drawing.Size(430, 23);
            textBoxArchiveFolder.TabIndex = 6;
            // 
            // buttonFeedback
            // 
            buttonFeedback.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonFeedback.FlatAppearance.BorderSize = 0;
            buttonFeedback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonFeedback.Image = Properties.Resources.Feedback;
            buttonFeedback.Location = new System.Drawing.Point(0, 0);
            buttonFeedback.Margin = new System.Windows.Forms.Padding(0);
            buttonFeedback.Name = "buttonFeedback";
            buttonFeedback.Size = new System.Drawing.Size(38, 23);
            buttonFeedback.TabIndex = 7;
            toolTip.SetToolTip(buttonFeedback, "Send a feedback");
            buttonFeedback.UseVisualStyleBackColor = true;
            buttonFeedback.Click += buttonFeedback_Click;
            // 
            // checkBoxIncludeLog
            // 
            checkBoxIncludeLog.AutoSize = true;
            tableLayoutPanelMain.SetColumnSpan(checkBoxIncludeLog, 2);
            checkBoxIncludeLog.Dock = System.Windows.Forms.DockStyle.Left;
            checkBoxIncludeLog.Image = Properties.Resources.LogSave;
            checkBoxIncludeLog.Location = new System.Drawing.Point(4, 531);
            checkBoxIncludeLog.Margin = new System.Windows.Forms.Padding(4, 0, 4, 3);
            checkBoxIncludeLog.Name = "checkBoxIncludeLog";
            checkBoxIncludeLog.Size = new System.Drawing.Size(101, 30);
            checkBoxIncludeLog.TabIndex = 8;
            checkBoxIncludeLog.Text = "Include log";
            checkBoxIncludeLog.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            checkBoxIncludeLog.UseVisualStyleBackColor = true;
            checkBoxIncludeLog.Click += checkBoxIncludeLog_Click;
            // 
            // progressBar
            // 
            tableLayoutPanelMain.SetColumnSpan(progressBar, 3);
            progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            progressBar.Location = new System.Drawing.Point(4, 564);
            progressBar.Margin = new System.Windows.Forms.Padding(4, 0, 4, 3);
            progressBar.Name = "progressBar";
            progressBar.Size = new System.Drawing.Size(468, 4);
            progressBar.TabIndex = 9;
            progressBar.Visible = false;
            // 
            // FormCreateArchive
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(476, 571);
            Controls.Add(tableLayoutPanelMain);
            helpProvider.SetHelpKeyword(this, "archive");
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormCreateArchive";
            helpProvider.SetShowHelp(this, true);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Create an archive";
            KeyDown += Form_KeyDown;
            tableLayoutPanelMain.ResumeLayout(false);
            tableLayoutPanelMain.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.ComboBox comboBoxRootSelection;
        private System.Windows.Forms.Panel panelTables;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonDirectory;
        private System.Windows.Forms.TextBox textBoxArchiveFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button buttonCreateArchive;
        private System.Windows.Forms.Button buttonQueryData;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.CheckBox checkBoxIncludeLog;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}