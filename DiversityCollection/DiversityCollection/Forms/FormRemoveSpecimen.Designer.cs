namespace DiversityCollection.Forms
{
    partial class FormRemoveSpecimen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRemoveSpecimen));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.checkBoxRemoveLog = new System.Windows.Forms.CheckBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelErrorReport = new System.Windows.Forms.Label();
            this.pictureBoxLogFiles = new System.Windows.Forms.PictureBox();
            this.pictureBoxEvents = new System.Windows.Forms.PictureBox();
            this.checkBoxRemoveEvents = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEvents)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 3;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxRemoveLog, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonStart, 2, 2);
            this.tableLayoutPanelMain.Controls.Add(this.progressBar, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.labelErrorReport, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.pictureBoxLogFiles, 1, 2);
            this.tableLayoutPanelMain.Controls.Add(this.pictureBoxEvents, 1, 3);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxRemoveEvents, 0, 3);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 5;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(382, 139);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelHeader, 3);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeader.Location = new System.Drawing.Point(3, 9);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(3, 9, 3, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(376, 24);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "Remove all selected specimen";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkBoxRemoveLog
            // 
            this.checkBoxRemoveLog.AutoSize = true;
            this.checkBoxRemoveLog.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxRemoveLog.Location = new System.Drawing.Point(3, 80);
            this.checkBoxRemoveLog.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.checkBoxRemoveLog.Name = "checkBoxRemoveLog";
            this.checkBoxRemoveLog.Size = new System.Drawing.Size(152, 19);
            this.checkBoxRemoveLog.TabIndex = 1;
            this.checkBoxRemoveLog.Text = "Remove entries in  log files";
            this.checkBoxRemoveLog.UseVisualStyleBackColor = true;
            this.checkBoxRemoveLog.CheckedChanged += new System.EventHandler(this.checkBoxRemoveLog_CheckedChanged);
            // 
            // buttonStart
            // 
            this.buttonStart.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonStart.Image = global::DiversityCollection.Resource.Delete;
            this.buttonStart.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonStart.Location = new System.Drawing.Point(283, 80);
            this.buttonStart.Name = "buttonStart";
            this.tableLayoutPanelMain.SetRowSpan(this.buttonStart, 2);
            this.buttonStart.Size = new System.Drawing.Size(96, 36);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start removal";
            this.buttonStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // progressBar
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.progressBar, 3);
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar.Location = new System.Drawing.Point(3, 122);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(376, 14);
            this.progressBar.TabIndex = 3;
            // 
            // labelErrorReport
            // 
            this.labelErrorReport.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelErrorReport, 3);
            this.labelErrorReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelErrorReport.Location = new System.Drawing.Point(3, 33);
            this.labelErrorReport.Name = "labelErrorReport";
            this.labelErrorReport.Size = new System.Drawing.Size(376, 44);
            this.labelErrorReport.TabIndex = 4;
            // 
            // pictureBoxLogFiles
            // 
            this.pictureBoxLogFiles.Image = global::DiversityCollection.Resource.History;
            this.pictureBoxLogFiles.Location = new System.Drawing.Point(155, 80);
            this.pictureBoxLogFiles.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.pictureBoxLogFiles.Name = "pictureBoxLogFiles";
            this.pictureBoxLogFiles.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxLogFiles.TabIndex = 5;
            this.pictureBoxLogFiles.TabStop = false;
            // 
            // pictureBoxEvents
            // 
            this.pictureBoxEvents.Image = global::DiversityCollection.Resource.Event;
            this.pictureBoxEvents.Location = new System.Drawing.Point(155, 100);
            this.pictureBoxEvents.Margin = new System.Windows.Forms.Padding(0, 1, 3, 3);
            this.pictureBoxEvents.Name = "pictureBoxEvents";
            this.pictureBoxEvents.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxEvents.TabIndex = 6;
            this.pictureBoxEvents.TabStop = false;
            // 
            // checkBoxRemoveEvents
            // 
            this.checkBoxRemoveEvents.AutoSize = true;
            this.checkBoxRemoveEvents.Checked = true;
            this.checkBoxRemoveEvents.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRemoveEvents.Location = new System.Drawing.Point(3, 99);
            this.checkBoxRemoveEvents.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.checkBoxRemoveEvents.Name = "checkBoxRemoveEvents";
            this.checkBoxRemoveEvents.Size = new System.Drawing.Size(149, 17);
            this.checkBoxRemoveEvents.TabIndex = 7;
            this.checkBoxRemoveEvents.Text = "Remove collection events";
            this.checkBoxRemoveEvents.UseVisualStyleBackColor = true;
            this.checkBoxRemoveEvents.CheckedChanged += new System.EventHandler(this.checkBoxRemoveEvents_CheckedChanged);
            // 
            // FormRemoveSpecimen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 139);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.ForeColor = System.Drawing.Color.Red;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormRemoveSpecimen";
            this.Text = "Remove selected specimen";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEvents)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.CheckBox checkBoxRemoveLog;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelErrorReport;
        private System.Windows.Forms.PictureBox pictureBoxLogFiles;
        private System.Windows.Forms.PictureBox pictureBoxEvents;
        private System.Windows.Forms.CheckBox checkBoxRemoveEvents;
        private System.Windows.Forms.ToolTip toolTip;
    }
}