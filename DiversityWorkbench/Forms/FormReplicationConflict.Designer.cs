namespace DiversityWorkbench.Forms
{
    partial class FormReplicationConflict
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReplicationConflict));
            this.labelHeader = new System.Windows.Forms.Label();
            this.dataGridViewPublisher = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanelData = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxPublisher = new System.Windows.Forms.PictureBox();
            this.pictureBoxSubscriber = new System.Windows.Forms.PictureBox();
            this.pictureBoxMerge = new System.Windows.Forms.PictureBox();
            this.radioButtonPublisher = new System.Windows.Forms.RadioButton();
            this.radioButtonMerge = new System.Windows.Forms.RadioButton();
            this.radioButtonSubscriber = new System.Windows.Forms.RadioButton();
            this.dataGridViewMerge = new System.Windows.Forms.DataGridView();
            this.dataGridViewSubscriber = new System.Windows.Forms.DataGridView();
            this.buttonDeletePublisher = new System.Windows.Forms.Button();
            this.buttonDeleteSubscriber = new System.Windows.Forms.Button();
            this.tableLayoutPanelHeader = new System.Windows.Forms.TableLayoutPanel();
            this.labelNumberOfConflicts = new System.Windows.Forms.Label();
            this.textBoxNumberOfConflicts = new System.Windows.Forms.TextBox();
            this.textBoxConflictsSolved = new System.Windows.Forms.TextBox();
            this.labelConflictsSolved = new System.Windows.Forms.Label();
            this.textBoxIgnoredConflicts = new System.Windows.Forms.TextBox();
            this.labelIgnoredConflicts = new System.Windows.Forms.Label();
            this.labelTable = new System.Windows.Forms.Label();
            this.buttonStopConflictResolution = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buttonSolveConflict = new System.Windows.Forms.Button();
            this.buttonNextConflict = new System.Windows.Forms.Button();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPublisher)).BeginInit();
            this.tableLayoutPanelData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPublisher)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSubscriber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMerge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMerge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSubscriber)).BeginInit();
            this.tableLayoutPanelHeader.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelHeader.Location = new System.Drawing.Point(41, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(151, 24);
            this.labelHeader.TabIndex = 1;
            this.labelHeader.Text = "Please select the correct data ";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridViewPublisher
            // 
            this.dataGridViewPublisher.AllowUserToAddRows = false;
            this.dataGridViewPublisher.AllowUserToDeleteRows = false;
            this.dataGridViewPublisher.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPublisher.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridViewPublisher.Location = new System.Drawing.Point(130, 3);
            this.dataGridViewPublisher.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.dataGridViewPublisher.Name = "dataGridViewPublisher";
            this.dataGridViewPublisher.ReadOnly = true;
            this.dataGridViewPublisher.RowHeadersVisible = false;
            this.dataGridViewPublisher.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewPublisher.Size = new System.Drawing.Size(984, 46);
            this.dataGridViewPublisher.TabIndex = 2;
            // 
            // tableLayoutPanelData
            // 
            this.tableLayoutPanelData.ColumnCount = 4;
            this.tableLayoutPanelData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelData.Controls.Add(this.pictureBoxPublisher, 1, 0);
            this.tableLayoutPanelData.Controls.Add(this.dataGridViewPublisher, 3, 0);
            this.tableLayoutPanelData.Controls.Add(this.pictureBoxSubscriber, 1, 2);
            this.tableLayoutPanelData.Controls.Add(this.pictureBoxMerge, 1, 1);
            this.tableLayoutPanelData.Controls.Add(this.radioButtonPublisher, 0, 0);
            this.tableLayoutPanelData.Controls.Add(this.radioButtonMerge, 0, 1);
            this.tableLayoutPanelData.Controls.Add(this.radioButtonSubscriber, 0, 2);
            this.tableLayoutPanelData.Controls.Add(this.dataGridViewMerge, 3, 1);
            this.tableLayoutPanelData.Controls.Add(this.dataGridViewSubscriber, 3, 2);
            this.tableLayoutPanelData.Controls.Add(this.buttonDeletePublisher, 2, 0);
            this.tableLayoutPanelData.Controls.Add(this.buttonDeleteSubscriber, 2, 2);
            this.tableLayoutPanelData.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelData.Location = new System.Drawing.Point(4, 28);
            this.tableLayoutPanelData.Name = "tableLayoutPanelData";
            this.tableLayoutPanelData.RowCount = 3;
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelData.Size = new System.Drawing.Size(1117, 103);
            this.tableLayoutPanelData.TabIndex = 3;
            // 
            // pictureBoxPublisher
            // 
            this.pictureBoxPublisher.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pictureBoxPublisher.Image = global::DiversityWorkbench.ResourceWorkbench.Download;
            this.pictureBoxPublisher.Location = new System.Drawing.Point(84, 30);
            this.pictureBoxPublisher.Name = "pictureBoxPublisher";
            this.pictureBoxPublisher.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxPublisher.TabIndex = 0;
            this.pictureBoxPublisher.TabStop = false;
            // 
            // pictureBoxSubscriber
            // 
            this.pictureBoxSubscriber.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBoxSubscriber.Image = global::DiversityWorkbench.ResourceWorkbench.Upload;
            this.pictureBoxSubscriber.Location = new System.Drawing.Point(84, 79);
            this.pictureBoxSubscriber.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.pictureBoxSubscriber.Name = "pictureBoxSubscriber";
            this.pictureBoxSubscriber.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxSubscriber.TabIndex = 3;
            this.pictureBoxSubscriber.TabStop = false;
            // 
            // pictureBoxMerge
            // 
            this.pictureBoxMerge.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pictureBoxMerge.Image = global::DiversityWorkbench.ResourceWorkbench.Merge;
            this.pictureBoxMerge.Location = new System.Drawing.Point(84, 55);
            this.pictureBoxMerge.Name = "pictureBoxMerge";
            this.pictureBoxMerge.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxMerge.TabIndex = 5;
            this.pictureBoxMerge.TabStop = false;
            // 
            // radioButtonPublisher
            // 
            this.radioButtonPublisher.AutoSize = true;
            this.radioButtonPublisher.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radioButtonPublisher.Location = new System.Drawing.Point(3, 29);
            this.radioButtonPublisher.Name = "radioButtonPublisher";
            this.radioButtonPublisher.Size = new System.Drawing.Size(75, 17);
            this.radioButtonPublisher.TabIndex = 6;
            this.radioButtonPublisher.TabStop = true;
            this.radioButtonPublisher.Text = "Publisher";
            this.radioButtonPublisher.UseVisualStyleBackColor = true;
            this.radioButtonPublisher.CheckedChanged += new System.EventHandler(this.radioButtonPublisher_CheckedChanged);
            // 
            // radioButtonMerge
            // 
            this.radioButtonMerge.AutoSize = true;
            this.radioButtonMerge.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radioButtonMerge.Location = new System.Drawing.Point(3, 54);
            this.radioButtonMerge.Name = "radioButtonMerge";
            this.radioButtonMerge.Size = new System.Drawing.Size(75, 17);
            this.radioButtonMerge.TabIndex = 7;
            this.radioButtonMerge.TabStop = true;
            this.radioButtonMerge.Text = "Merge";
            this.radioButtonMerge.UseVisualStyleBackColor = true;
            this.radioButtonMerge.CheckedChanged += new System.EventHandler(this.radioButtonMerge_CheckedChanged);
            // 
            // radioButtonSubscriber
            // 
            this.radioButtonSubscriber.AutoSize = true;
            this.radioButtonSubscriber.Dock = System.Windows.Forms.DockStyle.Top;
            this.radioButtonSubscriber.Location = new System.Drawing.Point(3, 79);
            this.radioButtonSubscriber.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.radioButtonSubscriber.Name = "radioButtonSubscriber";
            this.radioButtonSubscriber.Size = new System.Drawing.Size(75, 17);
            this.radioButtonSubscriber.TabIndex = 8;
            this.radioButtonSubscriber.TabStop = true;
            this.radioButtonSubscriber.Text = "Subscriber";
            this.radioButtonSubscriber.UseVisualStyleBackColor = true;
            this.radioButtonSubscriber.CheckedChanged += new System.EventHandler(this.radioButtonSubscriber_CheckedChanged);
            // 
            // dataGridViewMerge
            // 
            this.dataGridViewMerge.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMerge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewMerge.Location = new System.Drawing.Point(130, 52);
            this.dataGridViewMerge.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.dataGridViewMerge.Name = "dataGridViewMerge";
            this.dataGridViewMerge.RowHeadersVisible = false;
            this.dataGridViewMerge.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewMerge.Size = new System.Drawing.Size(984, 22);
            this.dataGridViewMerge.TabIndex = 9;
            // 
            // dataGridViewSubscriber
            // 
            this.dataGridViewSubscriber.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSubscriber.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridViewSubscriber.Location = new System.Drawing.Point(130, 77);
            this.dataGridViewSubscriber.Name = "dataGridViewSubscriber";
            this.dataGridViewSubscriber.ReadOnly = true;
            this.dataGridViewSubscriber.RowHeadersVisible = false;
            this.dataGridViewSubscriber.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewSubscriber.Size = new System.Drawing.Size(984, 21);
            this.dataGridViewSubscriber.TabIndex = 10;
            // 
            // buttonDeletePublisher
            // 
            this.buttonDeletePublisher.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonDeletePublisher.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.buttonDeletePublisher.Location = new System.Drawing.Point(103, 26);
            this.buttonDeletePublisher.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDeletePublisher.Name = "buttonDeletePublisher";
            this.buttonDeletePublisher.Size = new System.Drawing.Size(24, 23);
            this.buttonDeletePublisher.TabIndex = 11;
            this.toolTip.SetToolTip(this.buttonDeletePublisher, "Delete the datarow in the publisher database");
            this.buttonDeletePublisher.UseVisualStyleBackColor = true;
            this.buttonDeletePublisher.Visible = false;
            this.buttonDeletePublisher.Click += new System.EventHandler(this.buttonDeletePublisher_Click);
            // 
            // buttonDeleteSubscriber
            // 
            this.buttonDeleteSubscriber.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonDeleteSubscriber.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.buttonDeleteSubscriber.Location = new System.Drawing.Point(103, 76);
            this.buttonDeleteSubscriber.Margin = new System.Windows.Forms.Padding(0, 2, 0, 3);
            this.buttonDeleteSubscriber.Name = "buttonDeleteSubscriber";
            this.buttonDeleteSubscriber.Size = new System.Drawing.Size(24, 23);
            this.buttonDeleteSubscriber.TabIndex = 12;
            this.buttonDeleteSubscriber.UseVisualStyleBackColor = true;
            this.buttonDeleteSubscriber.Visible = false;
            this.buttonDeleteSubscriber.Click += new System.EventHandler(this.buttonDeleteSubscriber_Click);
            // 
            // tableLayoutPanelHeader
            // 
            this.tableLayoutPanelHeader.ColumnCount = 8;
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanelHeader.Controls.Add(this.labelHeader, 1, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.labelNumberOfConflicts, 6, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.textBoxNumberOfConflicts, 7, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.textBoxConflictsSolved, 5, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.labelConflictsSolved, 4, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.textBoxIgnoredConflicts, 3, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.labelIgnoredConflicts, 2, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.labelTable, 0, 0);
            this.tableLayoutPanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelHeader.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanelHeader.Name = "tableLayoutPanelHeader";
            this.tableLayoutPanelHeader.RowCount = 1;
            this.tableLayoutPanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelHeader.Size = new System.Drawing.Size(1117, 24);
            this.tableLayoutPanelHeader.TabIndex = 4;
            // 
            // labelNumberOfConflicts
            // 
            this.labelNumberOfConflicts.AutoSize = true;
            this.labelNumberOfConflicts.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelNumberOfConflicts.Location = new System.Drawing.Point(984, 0);
            this.labelNumberOfConflicts.Name = "labelNumberOfConflicts";
            this.labelNumberOfConflicts.Size = new System.Drawing.Size(90, 24);
            this.labelNumberOfConflicts.TabIndex = 2;
            this.labelNumberOfConflicts.Text = "Conflicts to solve:";
            this.labelNumberOfConflicts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxNumberOfConflicts
            // 
            this.textBoxNumberOfConflicts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxNumberOfConflicts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNumberOfConflicts.Location = new System.Drawing.Point(1080, 6);
            this.textBoxNumberOfConflicts.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.textBoxNumberOfConflicts.Name = "textBoxNumberOfConflicts";
            this.textBoxNumberOfConflicts.ReadOnly = true;
            this.textBoxNumberOfConflicts.Size = new System.Drawing.Size(34, 13);
            this.textBoxNumberOfConflicts.TabIndex = 3;
            // 
            // textBoxConflictsSolved
            // 
            this.textBoxConflictsSolved.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxConflictsSolved.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxConflictsSolved.Location = new System.Drawing.Point(944, 6);
            this.textBoxConflictsSolved.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.textBoxConflictsSolved.Name = "textBoxConflictsSolved";
            this.textBoxConflictsSolved.ReadOnly = true;
            this.textBoxConflictsSolved.Size = new System.Drawing.Size(34, 13);
            this.textBoxConflictsSolved.TabIndex = 4;
            this.textBoxConflictsSolved.Text = "0";
            // 
            // labelConflictsSolved
            // 
            this.labelConflictsSolved.AutoSize = true;
            this.labelConflictsSolved.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelConflictsSolved.Location = new System.Drawing.Point(853, 0);
            this.labelConflictsSolved.Name = "labelConflictsSolved";
            this.labelConflictsSolved.Size = new System.Drawing.Size(85, 24);
            this.labelConflictsSolved.TabIndex = 5;
            this.labelConflictsSolved.Text = "Solved conflicts:";
            this.labelConflictsSolved.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxIgnoredConflicts
            // 
            this.textBoxIgnoredConflicts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxIgnoredConflicts.Location = new System.Drawing.Point(813, 6);
            this.textBoxIgnoredConflicts.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.textBoxIgnoredConflicts.Name = "textBoxIgnoredConflicts";
            this.textBoxIgnoredConflicts.ReadOnly = true;
            this.textBoxIgnoredConflicts.Size = new System.Drawing.Size(34, 13);
            this.textBoxIgnoredConflicts.TabIndex = 6;
            this.textBoxIgnoredConflicts.Text = "0";
            // 
            // labelIgnoredConflicts
            // 
            this.labelIgnoredConflicts.AutoSize = true;
            this.labelIgnoredConflicts.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelIgnoredConflicts.Location = new System.Drawing.Point(719, 0);
            this.labelIgnoredConflicts.Name = "labelIgnoredConflicts";
            this.labelIgnoredConflicts.Size = new System.Drawing.Size(88, 24);
            this.labelIgnoredConflicts.TabIndex = 7;
            this.labelIgnoredConflicts.Text = "Ignored conflicts:";
            this.labelIgnoredConflicts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelTable
            // 
            this.labelTable.AutoSize = true;
            this.labelTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTable.Location = new System.Drawing.Point(3, 0);
            this.labelTable.Name = "labelTable";
            this.labelTable.Size = new System.Drawing.Size(32, 24);
            this.labelTable.TabIndex = 8;
            this.labelTable.Text = "Tab.:";
            this.labelTable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonStopConflictResolution
            // 
            this.buttonStopConflictResolution.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonStopConflictResolution.Image = global::DiversityWorkbench.Properties.Resources.error;
            this.buttonStopConflictResolution.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonStopConflictResolution.Location = new System.Drawing.Point(977, 0);
            this.buttonStopConflictResolution.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStopConflictResolution.Name = "buttonStopConflictResolution";
            this.buttonStopConflictResolution.Size = new System.Drawing.Size(140, 27);
            this.buttonStopConflictResolution.TabIndex = 2;
            this.buttonStopConflictResolution.Text = "Stop conflict resolution";
            this.buttonStopConflictResolution.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonStopConflictResolution, "Stop the conflict resolution");
            this.buttonStopConflictResolution.UseVisualStyleBackColor = true;
            this.buttonStopConflictResolution.Click += new System.EventHandler(this.buttonStopConflictResolution_Click);
            // 
            // buttonSolveConflict
            // 
            this.buttonSolveConflict.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonSolveConflict.Image = global::DiversityWorkbench.Properties.Resources.OK;
            this.buttonSolveConflict.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSolveConflict.Location = new System.Drawing.Point(881, 0);
            this.buttonSolveConflict.Name = "buttonSolveConflict";
            this.buttonSolveConflict.Size = new System.Drawing.Size(96, 27);
            this.buttonSolveConflict.TabIndex = 3;
            this.buttonSolveConflict.Text = "Solve conflict";
            this.buttonSolveConflict.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonSolveConflict, "Solve the current conflict");
            this.buttonSolveConflict.UseVisualStyleBackColor = true;
            this.buttonSolveConflict.Click += new System.EventHandler(this.buttonSolveConflict_Click);
            // 
            // buttonNextConflict
            // 
            this.buttonNextConflict.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonNextConflict.Image = global::DiversityWorkbench.Properties.Resources.ArrowNext;
            this.buttonNextConflict.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonNextConflict.Location = new System.Drawing.Point(787, 0);
            this.buttonNextConflict.Name = "buttonNextConflict";
            this.buttonNextConflict.Size = new System.Drawing.Size(94, 27);
            this.buttonNextConflict.TabIndex = 4;
            this.buttonNextConflict.Text = "Ignore conflict";
            this.buttonNextConflict.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonNextConflict, "Ignore the current conflict and change to next conflict");
            this.buttonNextConflict.UseVisualStyleBackColor = true;
            this.buttonNextConflict.Click += new System.EventHandler(this.buttonNextConflict_Click);
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.buttonNextConflict);
            this.panelButtons.Controls.Add(this.buttonSolveConflict);
            this.panelButtons.Controls.Add(this.buttonStopConflictResolution);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(4, 132);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1117, 27);
            this.panelButtons.TabIndex = 5;
            // 
            // FormReplicationConflict
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1125, 163);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.tableLayoutPanelData);
            this.Controls.Add(this.tableLayoutPanelHeader);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormReplicationConflict";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Replication conflict";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPublisher)).EndInit();
            this.tableLayoutPanelData.ResumeLayout(false);
            this.tableLayoutPanelData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPublisher)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSubscriber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMerge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMerge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSubscriber)).EndInit();
            this.tableLayoutPanelHeader.ResumeLayout(false);
            this.tableLayoutPanelHeader.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.DataGridView dataGridViewPublisher;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelData;
        private System.Windows.Forms.PictureBox pictureBoxPublisher;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelHeader;
        private System.Windows.Forms.PictureBox pictureBoxSubscriber;
        private System.Windows.Forms.PictureBox pictureBoxMerge;
        private System.Windows.Forms.RadioButton radioButtonPublisher;
        private System.Windows.Forms.RadioButton radioButtonMerge;
        private System.Windows.Forms.RadioButton radioButtonSubscriber;
        private System.Windows.Forms.DataGridView dataGridViewMerge;
        private System.Windows.Forms.DataGridView dataGridViewSubscriber;
        private System.Windows.Forms.Button buttonDeletePublisher;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonDeleteSubscriber;
        private System.Windows.Forms.Button buttonStopConflictResolution;
        private System.Windows.Forms.Label labelNumberOfConflicts;
        private System.Windows.Forms.TextBox textBoxNumberOfConflicts;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button buttonNextConflict;
        private System.Windows.Forms.Button buttonSolveConflict;
        private System.Windows.Forms.TextBox textBoxConflictsSolved;
        private System.Windows.Forms.Label labelConflictsSolved;
        private System.Windows.Forms.TextBox textBoxIgnoredConflicts;
        private System.Windows.Forms.Label labelIgnoredConflicts;
        private System.Windows.Forms.Label labelTable;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}