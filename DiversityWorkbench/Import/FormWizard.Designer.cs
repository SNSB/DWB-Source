namespace DiversityWorkbench.Import
{
    partial class FormWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWizard));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonFeedback = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLogging = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabelColumnHeaders = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxColumnHeaders = new System.Windows.Forms.ToolStripComboBox();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.splitContainerData = new System.Windows.Forms.SplitContainer();
            this.panelStepSelection = new System.Windows.Forms.Panel();
            this.splitContainerColumnsAndFile = new System.Windows.Forms.SplitContainer();
            this.panelData = new System.Windows.Forms.Panel();
            this.panelDataHeader = new System.Windows.Forms.Panel();
            this.labelDataHeaderMergeHandling = new System.Windows.Forms.Label();
            this.pictureBoxDataHeaderMergeHandling = new System.Windows.Forms.PictureBox();
            this.labelDataHeaderGroup = new System.Windows.Forms.Label();
            this.pictureBoxDataHeaderGroup = new System.Windows.Forms.PictureBox();
            this.labelDataHeaderStep = new System.Windows.Forms.Label();
            this.pictureBoxDataHeaderStep = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanelFile = new System.Windows.Forms.TableLayoutPanel();
            this.panelFileColumn = new System.Windows.Forms.Panel();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.panelSteps = new System.Windows.Forms.Panel();
            this.panelFile = new System.Windows.Forms.Panel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.imageListSteps = new System.Windows.Forms.ImageList(this.components);
            this.imageListLogging = new System.Windows.Forms.ImageList(this.components);
            this.imageListMergeHandling = new System.Windows.Forms.ImageList(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerData)).BeginInit();
            this.splitContainerData.Panel1.SuspendLayout();
            this.splitContainerData.Panel2.SuspendLayout();
            this.splitContainerData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerColumnsAndFile)).BeginInit();
            this.splitContainerColumnsAndFile.Panel1.SuspendLayout();
            this.splitContainerColumnsAndFile.Panel2.SuspendLayout();
            this.splitContainerColumnsAndFile.SuspendLayout();
            this.panelDataHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDataHeaderMergeHandling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDataHeaderGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDataHeaderStep)).BeginInit();
            this.tableLayoutPanelFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonFeedback,
            this.toolStripButtonLogging,
            this.toolStripLabelColumnHeaders,
            this.toolStripComboBoxColumnHeaders});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1134, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripButtonFeedback
            // 
            this.toolStripButtonFeedback.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonFeedback.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFeedback.Image = global::DiversityWorkbench.Properties.Resources.Feedback;
            this.toolStripButtonFeedback.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFeedback.Name = "toolStripButtonFeedback";
            this.toolStripButtonFeedback.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonFeedback.Text = "Send feedback";
            this.toolStripButtonFeedback.Click += new System.EventHandler(this.toolStripButtonFeedback_Click);
            // 
            // toolStripButtonLogging
            // 
            this.toolStripButtonLogging.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonLogging.Image = global::DiversityWorkbench.Properties.Resources.FingerprintGray;
            this.toolStripButtonLogging.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButtonLogging.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLogging.Name = "toolStripButtonLogging";
            this.toolStripButtonLogging.Size = new System.Drawing.Size(145, 22);
            this.toolStripButtonLogging.Tag = "0";
            this.toolStripButtonLogging.Text = "Hide logging columns";
            this.toolStripButtonLogging.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripButtonLogging.Click += new System.EventHandler(this.toolStripButtonLogging_Click);
            // 
            // toolStripLabelColumnHeaders
            // 
            this.toolStripLabelColumnHeaders.Name = "toolStripLabelColumnHeaders";
            this.toolStripLabelColumnHeaders.Size = new System.Drawing.Size(94, 22);
            this.toolStripLabelColumnHeaders.Text = "Column headers";
            // 
            // toolStripComboBoxColumnHeaders
            // 
            this.toolStripComboBoxColumnHeaders.Items.AddRange(new object[] {
            "visible",
            "hidden"});
            this.toolStripComboBoxColumnHeaders.Name = "toolStripComboBoxColumnHeaders";
            this.toolStripComboBoxColumnHeaders.Size = new System.Drawing.Size(75, 25);
            this.toolStripComboBoxColumnHeaders.Text = "visible";
            this.toolStripComboBoxColumnHeaders.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxColumnHeaders_SelectedIndexChanged);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 25);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerData);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.panelSteps);
            this.splitContainerMain.Panel2.Controls.Add(this.panelFile);
            this.splitContainerMain.Size = new System.Drawing.Size(1134, 549);
            this.splitContainerMain.SplitterDistance = 928;
            this.splitContainerMain.TabIndex = 1;
            // 
            // splitContainerData
            // 
            this.splitContainerData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerData.Location = new System.Drawing.Point(0, 0);
            this.splitContainerData.Name = "splitContainerData";
            // 
            // splitContainerData.Panel1
            // 
            this.splitContainerData.Panel1.Controls.Add(this.panelStepSelection);
            // 
            // splitContainerData.Panel2
            // 
            this.splitContainerData.Panel2.Controls.Add(this.splitContainerColumnsAndFile);
            this.splitContainerData.Size = new System.Drawing.Size(928, 549);
            this.splitContainerData.SplitterDistance = 210;
            this.splitContainerData.TabIndex = 0;
            // 
            // panelStepSelection
            // 
            this.panelStepSelection.AutoScroll = true;
            this.panelStepSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStepSelection.Location = new System.Drawing.Point(0, 0);
            this.panelStepSelection.Name = "panelStepSelection";
            this.panelStepSelection.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelStepSelection.Size = new System.Drawing.Size(210, 549);
            this.panelStepSelection.TabIndex = 0;
            // 
            // splitContainerColumnsAndFile
            // 
            this.splitContainerColumnsAndFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerColumnsAndFile.Location = new System.Drawing.Point(0, 0);
            this.splitContainerColumnsAndFile.Name = "splitContainerColumnsAndFile";
            this.splitContainerColumnsAndFile.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerColumnsAndFile.Panel1
            // 
            this.splitContainerColumnsAndFile.Panel1.Controls.Add(this.panelData);
            this.splitContainerColumnsAndFile.Panel1.Controls.Add(this.panelDataHeader);
            // 
            // splitContainerColumnsAndFile.Panel2
            // 
            this.splitContainerColumnsAndFile.Panel2.Controls.Add(this.tableLayoutPanelFile);
            this.splitContainerColumnsAndFile.Size = new System.Drawing.Size(714, 549);
            this.splitContainerColumnsAndFile.SplitterDistance = 274;
            this.splitContainerColumnsAndFile.TabIndex = 1;
            // 
            // panelData
            // 
            this.panelData.AutoScroll = true;
            this.panelData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelData.Location = new System.Drawing.Point(0, 28);
            this.panelData.Name = "panelData";
            this.panelData.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelData.Size = new System.Drawing.Size(714, 246);
            this.panelData.TabIndex = 0;
            // 
            // panelDataHeader
            // 
            this.panelDataHeader.Controls.Add(this.labelDataHeaderMergeHandling);
            this.panelDataHeader.Controls.Add(this.pictureBoxDataHeaderMergeHandling);
            this.panelDataHeader.Controls.Add(this.labelDataHeaderGroup);
            this.panelDataHeader.Controls.Add(this.pictureBoxDataHeaderGroup);
            this.panelDataHeader.Controls.Add(this.labelDataHeaderStep);
            this.panelDataHeader.Controls.Add(this.pictureBoxDataHeaderStep);
            this.panelDataHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDataHeader.Location = new System.Drawing.Point(0, 0);
            this.panelDataHeader.Name = "panelDataHeader";
            this.panelDataHeader.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.panelDataHeader.Size = new System.Drawing.Size(714, 28);
            this.panelDataHeader.TabIndex = 1;
            // 
            // labelDataHeaderMergeHandling
            // 
            this.labelDataHeaderMergeHandling.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelDataHeaderMergeHandling.Location = new System.Drawing.Point(640, 0);
            this.labelDataHeaderMergeHandling.Name = "labelDataHeaderMergeHandling";
            this.labelDataHeaderMergeHandling.Size = new System.Drawing.Size(50, 28);
            this.labelDataHeaderMergeHandling.TabIndex = 5;
            this.labelDataHeaderMergeHandling.Text = "Insert";
            this.labelDataHeaderMergeHandling.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pictureBoxDataHeaderMergeHandling
            // 
            this.pictureBoxDataHeaderMergeHandling.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBoxDataHeaderMergeHandling.Location = new System.Drawing.Point(690, 0);
            this.pictureBoxDataHeaderMergeHandling.Name = "pictureBoxDataHeaderMergeHandling";
            this.pictureBoxDataHeaderMergeHandling.Padding = new System.Windows.Forms.Padding(0, 6, 6, 6);
            this.pictureBoxDataHeaderMergeHandling.Size = new System.Drawing.Size(22, 28);
            this.pictureBoxDataHeaderMergeHandling.TabIndex = 4;
            this.pictureBoxDataHeaderMergeHandling.TabStop = false;
            // 
            // labelDataHeaderGroup
            // 
            this.labelDataHeaderGroup.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelDataHeaderGroup.Location = new System.Drawing.Point(140, 0);
            this.labelDataHeaderGroup.Name = "labelDataHeaderGroup";
            this.labelDataHeaderGroup.Size = new System.Drawing.Size(100, 28);
            this.labelDataHeaderGroup.TabIndex = 3;
            this.labelDataHeaderGroup.Text = "Group";
            this.labelDataHeaderGroup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelDataHeaderGroup.Visible = false;
            // 
            // pictureBoxDataHeaderGroup
            // 
            this.pictureBoxDataHeaderGroup.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBoxDataHeaderGroup.Location = new System.Drawing.Point(118, 0);
            this.pictureBoxDataHeaderGroup.Name = "pictureBoxDataHeaderGroup";
            this.pictureBoxDataHeaderGroup.Padding = new System.Windows.Forms.Padding(6, 6, 0, 6);
            this.pictureBoxDataHeaderGroup.Size = new System.Drawing.Size(22, 28);
            this.pictureBoxDataHeaderGroup.TabIndex = 2;
            this.pictureBoxDataHeaderGroup.TabStop = false;
            this.pictureBoxDataHeaderGroup.Visible = false;
            // 
            // labelDataHeaderStep
            // 
            this.labelDataHeaderStep.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelDataHeaderStep.Location = new System.Drawing.Point(18, 0);
            this.labelDataHeaderStep.Name = "labelDataHeaderStep";
            this.labelDataHeaderStep.Size = new System.Drawing.Size(100, 28);
            this.labelDataHeaderStep.TabIndex = 1;
            this.labelDataHeaderStep.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBoxDataHeaderStep
            // 
            this.pictureBoxDataHeaderStep.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBoxDataHeaderStep.Location = new System.Drawing.Point(2, 0);
            this.pictureBoxDataHeaderStep.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.pictureBoxDataHeaderStep.Name = "pictureBoxDataHeaderStep";
            this.pictureBoxDataHeaderStep.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this.pictureBoxDataHeaderStep.Size = new System.Drawing.Size(16, 28);
            this.pictureBoxDataHeaderStep.TabIndex = 0;
            this.pictureBoxDataHeaderStep.TabStop = false;
            // 
            // tableLayoutPanelFile
            // 
            this.tableLayoutPanelFile.ColumnCount = 1;
            this.tableLayoutPanelFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelFile.Controls.Add(this.panelFileColumn, 0, 0);
            this.tableLayoutPanelFile.Controls.Add(this.dataGridView, 0, 1);
            this.tableLayoutPanelFile.Controls.Add(this.progressBar, 0, 2);
            this.tableLayoutPanelFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelFile.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelFile.Name = "tableLayoutPanelFile";
            this.tableLayoutPanelFile.RowCount = 3;
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelFile.Size = new System.Drawing.Size(714, 271);
            this.tableLayoutPanelFile.TabIndex = 0;
            // 
            // panelFileColumn
            // 
            this.panelFileColumn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFileColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFileColumn.Location = new System.Drawing.Point(0, 0);
            this.panelFileColumn.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.panelFileColumn.Name = "panelFileColumn";
            this.panelFileColumn.Size = new System.Drawing.Size(714, 48);
            this.panelFileColumn.TabIndex = 0;
            this.panelFileColumn.Visible = false;
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 51);
            this.dataGridView.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersWidth = 21;
            this.dataGridView.Size = new System.Drawing.Size(714, 207);
            this.dataGridView.TabIndex = 1;
            this.dataGridView.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_RowPostPaint);
            this.dataGridView.Paint += new System.Windows.Forms.PaintEventHandler(this.dataGridView_Paint);
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar.Location = new System.Drawing.Point(0, 258);
            this.progressBar.Margin = new System.Windows.Forms.Padding(0);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(714, 13);
            this.progressBar.TabIndex = 2;
            // 
            // panelSteps
            // 
            this.panelSteps.AutoScroll = true;
            this.panelSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSteps.Location = new System.Drawing.Point(0, 26);
            this.panelSteps.Name = "panelSteps";
            this.panelSteps.Size = new System.Drawing.Size(202, 523);
            this.panelSteps.TabIndex = 0;
            // 
            // panelFile
            // 
            this.panelFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFile.Location = new System.Drawing.Point(0, 0);
            this.panelFile.Name = "panelFile";
            this.panelFile.Size = new System.Drawing.Size(202, 26);
            this.panelFile.TabIndex = 1;
            // 
            // imageListSteps
            // 
            this.imageListSteps.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSteps.ImageStream")));
            this.imageListSteps.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSteps.Images.SetKeyName(0, "OpenFolder.ico");
            this.imageListSteps.Images.SetKeyName(1, "Attach.ico");
            this.imageListSteps.Images.SetKeyName(2, "Merge.ico");
            this.imageListSteps.Images.SetKeyName(3, "ImportAnalysis.ico");
            this.imageListSteps.Images.SetKeyName(4, "Import.ico");
            this.imageListSteps.Images.SetKeyName(5, "Export.ico");
            // 
            // imageListLogging
            // 
            this.imageListLogging.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListLogging.ImageStream")));
            this.imageListLogging.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListLogging.Images.SetKeyName(0, "FingerprintGray.ico");
            this.imageListLogging.Images.SetKeyName(1, "Fingerprint.ico");
            // 
            // imageListMergeHandling
            // 
            this.imageListMergeHandling.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMergeHandling.ImageStream")));
            this.imageListMergeHandling.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListMergeHandling.Images.SetKeyName(0, "MergeInsert.ico");
            this.imageListMergeHandling.Images.SetKeyName(1, "MergeMerge.ico");
            this.imageListMergeHandling.Images.SetKeyName(2, "MergeUpdate.ico");
            this.imageListMergeHandling.Images.SetKeyName(3, "Attach.ico");
            // 
            // FormWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 574);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.toolStrip);
            this.helpProvider.SetHelpKeyword(this, "Wizard");
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormWizard";
            this.helpProvider.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormWizard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormWizard_FormClosing);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerData.Panel1.ResumeLayout(false);
            this.splitContainerData.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerData)).EndInit();
            this.splitContainerData.ResumeLayout(false);
            this.splitContainerColumnsAndFile.Panel1.ResumeLayout(false);
            this.splitContainerColumnsAndFile.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerColumnsAndFile)).EndInit();
            this.splitContainerColumnsAndFile.ResumeLayout(false);
            this.panelDataHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDataHeaderMergeHandling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDataHeaderGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDataHeaderStep)).EndInit();
            this.tableLayoutPanelFile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.ToolStripButton toolStripButtonFeedback;
        private System.Windows.Forms.Panel panelSteps;
        private System.Windows.Forms.SplitContainer splitContainerData;
        private System.Windows.Forms.Panel panelStepSelection;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelFile;
        private System.Windows.Forms.Panel panelFileColumn;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Panel panelData;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ImageList imageListSteps;
        private System.Windows.Forms.SplitContainer splitContainerColumnsAndFile;
        private System.Windows.Forms.Panel panelDataHeader;
        private System.Windows.Forms.Label labelDataHeaderGroup;
        private System.Windows.Forms.PictureBox pictureBoxDataHeaderGroup;
        private System.Windows.Forms.Label labelDataHeaderStep;
        private System.Windows.Forms.PictureBox pictureBoxDataHeaderStep;
        private System.Windows.Forms.Panel panelFile;
        private System.Windows.Forms.ToolStripButton toolStripButtonLogging;
        private System.Windows.Forms.ImageList imageListLogging;
        private System.Windows.Forms.Label labelDataHeaderMergeHandling;
        private System.Windows.Forms.PictureBox pictureBoxDataHeaderMergeHandling;
        private System.Windows.Forms.ImageList imageListMergeHandling;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ToolStripLabel toolStripLabelColumnHeaders;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxColumnHeaders;

    }
}