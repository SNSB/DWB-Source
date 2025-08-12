namespace DiversityWorkbench.Archive
{
    partial class FormRestoreArchive
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRestoreArchive));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.panelTables = new System.Windows.Forms.Panel();
            this.buttonRestoreArchive = new System.Windows.Forms.Button();
            this.buttonLoadData = new System.Windows.Forms.Button();
            this.textBoxArchiveFolder = new System.Windows.Forms.TextBox();
            this.buttonReadData = new System.Windows.Forms.Button();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.checkBoxIncludeLog = new System.Windows.Forms.CheckBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelState = new System.Windows.Forms.Label();
            this.checkBoxAskForStopOnError = new System.Windows.Forms.CheckBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 5;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.panelTables, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonRestoreArchive, 4, 4);
            this.tableLayoutPanelMain.Controls.Add(this.buttonLoadData, 3, 0);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxArchiveFolder, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonReadData, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.buttonFeedback, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxIncludeLog, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.progressBar, 0, 5);
            this.tableLayoutPanelMain.Controls.Add(this.labelState, 2, 4);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxAskForStopOnError, 2, 3);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 6;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(369, 498);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelHeader, 2);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(32, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(135, 29);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "label1";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTables
            // 
            this.panelTables.AutoScroll = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.panelTables, 5);
            this.panelTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTables.Location = new System.Drawing.Point(3, 58);
            this.panelTables.Name = "panelTables";
            this.panelTables.Size = new System.Drawing.Size(363, 378);
            this.panelTables.TabIndex = 3;
            // 
            // buttonRestoreArchive
            // 
            this.buttonRestoreArchive.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonRestoreArchive.Enabled = false;
            this.buttonRestoreArchive.Image = global::DiversityWorkbench.Properties.Resources.ArchivRestore;
            this.buttonRestoreArchive.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRestoreArchive.Location = new System.Drawing.Point(243, 466);
            this.buttonRestoreArchive.Name = "buttonRestoreArchive";
            this.buttonRestoreArchive.Size = new System.Drawing.Size(123, 23);
            this.buttonRestoreArchive.TabIndex = 4;
            this.buttonRestoreArchive.Text = "Restore the archive";
            this.buttonRestoreArchive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonRestoreArchive, "Restore the archive into the database");
            this.buttonRestoreArchive.UseVisualStyleBackColor = true;
            this.buttonRestoreArchive.Click += new System.EventHandler(this.buttonRestoreArchive_Click);
            // 
            // buttonLoadData
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonLoadData, 2);
            this.buttonLoadData.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonLoadData.Image = global::DiversityWorkbench.Properties.Resources.OpenFolder;
            this.buttonLoadData.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonLoadData.Location = new System.Drawing.Point(216, 3);
            this.buttonLoadData.Name = "buttonLoadData";
            this.buttonLoadData.Size = new System.Drawing.Size(150, 23);
            this.buttonLoadData.TabIndex = 2;
            this.buttonLoadData.Text = "Choose archive directory";
            this.buttonLoadData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonLoadData, "Choose the directory from which the archive should be restored");
            this.buttonLoadData.UseVisualStyleBackColor = true;
            this.buttonLoadData.Click += new System.EventHandler(this.buttonLoadData_Click);
            // 
            // textBoxArchiveFolder
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxArchiveFolder, 5);
            this.textBoxArchiveFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxArchiveFolder.Location = new System.Drawing.Point(3, 32);
            this.textBoxArchiveFolder.Name = "textBoxArchiveFolder";
            this.textBoxArchiveFolder.ReadOnly = true;
            this.textBoxArchiveFolder.Size = new System.Drawing.Size(363, 20);
            this.textBoxArchiveFolder.TabIndex = 6;
            // 
            // buttonReadData
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonReadData, 2);
            this.buttonReadData.Enabled = false;
            this.buttonReadData.Image = global::DiversityWorkbench.Properties.Resources.Import;
            this.buttonReadData.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonReadData.Location = new System.Drawing.Point(3, 466);
            this.buttonReadData.Name = "buttonReadData";
            this.buttonReadData.Size = new System.Drawing.Size(80, 23);
            this.buttonReadData.TabIndex = 7;
            this.buttonReadData.Text = "Read data";
            this.buttonReadData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonReadData, "Read the data into intermediary cache tables");
            this.buttonReadData.UseVisualStyleBackColor = true;
            this.buttonReadData.Click += new System.EventHandler(this.buttonReadData_Click);
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFeedback.FlatAppearance.BorderSize = 0;
            this.buttonFeedback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFeedback.Image = global::DiversityWorkbench.Properties.Resources.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(3, 3);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(23, 23);
            this.buttonFeedback.TabIndex = 8;
            this.toolTip.SetToolTip(this.buttonFeedback, "Send a feedback");
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // checkBoxIncludeLog
            // 
            this.checkBoxIncludeLog.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.checkBoxIncludeLog, 2);
            this.checkBoxIncludeLog.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxIncludeLog.Image = global::DiversityWorkbench.Properties.Resources.LogRestore;
            this.checkBoxIncludeLog.Location = new System.Drawing.Point(3, 439);
            this.checkBoxIncludeLog.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.checkBoxIncludeLog.Name = "checkBoxIncludeLog";
            this.checkBoxIncludeLog.Size = new System.Drawing.Size(94, 24);
            this.checkBoxIncludeLog.TabIndex = 9;
            this.checkBoxIncludeLog.Text = "Include log";
            this.checkBoxIncludeLog.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.checkBoxIncludeLog.UseVisualStyleBackColor = true;
            this.checkBoxIncludeLog.Click += new System.EventHandler(this.checkBoxIncludeLog_Click);
            // 
            // progressBar
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.progressBar, 5);
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar.Location = new System.Drawing.Point(3, 492);
            this.progressBar.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(363, 3);
            this.progressBar.TabIndex = 10;
            this.progressBar.Visible = false;
            // 
            // labelState
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.labelState, 2);
            this.labelState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelState.Location = new System.Drawing.Point(100, 463);
            this.labelState.Margin = new System.Windows.Forms.Padding(0);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(140, 29);
            this.labelState.TabIndex = 11;
            this.labelState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkBoxAskForStopOnError
            // 
            this.checkBoxAskForStopOnError.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.checkBoxAskForStopOnError, 3);
            this.checkBoxAskForStopOnError.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxAskForStopOnError.Image = global::DiversityWorkbench.Properties.Resources.Stop3;
            this.checkBoxAskForStopOnError.Location = new System.Drawing.Point(229, 439);
            this.checkBoxAskForStopOnError.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.checkBoxAskForStopOnError.Name = "checkBoxAskForStopOnError";
            this.checkBoxAskForStopOnError.Size = new System.Drawing.Size(137, 24);
            this.checkBoxAskForStopOnError.TabIndex = 12;
            this.checkBoxAskForStopOnError.Text = "Ask for stop on error";
            this.checkBoxAskForStopOnError.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.checkBoxAskForStopOnError.UseVisualStyleBackColor = true;
            // 
            // FormRestoreArchive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 498);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormRestoreArchive";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Restore archive";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Panel panelTables;
        private System.Windows.Forms.Button buttonRestoreArchive;
        private System.Windows.Forms.Button buttonLoadData;
        private System.Windows.Forms.TextBox textBoxArchiveFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button buttonReadData;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.CheckBox checkBoxIncludeLog;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.CheckBox checkBoxAskForStopOnError;
    }
}