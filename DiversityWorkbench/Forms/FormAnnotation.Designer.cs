namespace DiversityWorkbench.Forms
{
    partial class FormAnnotation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAnnotation));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.treeViewAnnotation = new System.Windows.Forms.TreeView();
            this.imageListAnnotationType = new System.Windows.Forms.ImageList();
            this.toolStripAnnotation = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNewRootAnnotation = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNewAnnotation = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFeedback = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanelData = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxAnnotation = new System.Windows.Forms.TextBox();
            this.userControlModuleRelatedEntryReference = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.linkLabelURL = new System.Windows.Forms.LinkLabel();
            this.buttonSetURL = new System.Windows.Forms.Button();
            this.labelCreatedBy = new System.Windows.Forms.Label();
            this.textBoxCreatedBy = new System.Windows.Forms.TextBox();
            this.labelCreatedWhen = new System.Windows.Forms.Label();
            this.textBoxCreatedWhen = new System.Windows.Forms.TextBox();
            this.panelAnnotationType = new System.Windows.Forms.Panel();
            this.labelDataRow = new System.Windows.Forms.Label();
            this.labelAnnotationType = new System.Windows.Forms.Label();
            this.pictureBoxAnnotationType = new System.Windows.Forms.PictureBox();
            this.labelAnnotationTitle = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntrySource = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.labelSource = new System.Windows.Forms.Label();
            this.labelReference = new System.Windows.Forms.Label();
            this.comboBoxTitle = new System.Windows.Forms.ComboBox();
            this.labelReferencedTable = new System.Windows.Forms.Label();
            this.textBoxReferencedTable = new System.Windows.Forms.TextBox();
            this.labelURL = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip();
            this.labelHeader = new System.Windows.Forms.Label();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.toolStripAnnotation.SuspendLayout();
            this.tableLayoutPanelData.SuspendLayout();
            this.panelAnnotationType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAnnotationType)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 13);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.treeViewAnnotation);
            this.splitContainerMain.Panel1.Controls.Add(this.toolStripAnnotation);
            this.splitContainerMain.Panel1.Padding = new System.Windows.Forms.Padding(3);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tableLayoutPanelData);
            this.splitContainerMain.Size = new System.Drawing.Size(468, 467);
            this.splitContainerMain.SplitterDistance = 252;
            this.splitContainerMain.TabIndex = 0;
            // 
            // treeViewAnnotation
            // 
            this.treeViewAnnotation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewAnnotation.ImageIndex = 0;
            this.treeViewAnnotation.ImageList = this.imageListAnnotationType;
            this.treeViewAnnotation.Location = new System.Drawing.Point(3, 28);
            this.treeViewAnnotation.Name = "treeViewAnnotation";
            this.treeViewAnnotation.SelectedImageIndex = 0;
            this.treeViewAnnotation.Size = new System.Drawing.Size(462, 221);
            this.treeViewAnnotation.TabIndex = 1;
            this.treeViewAnnotation.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewAnnotation_AfterSelect);
            this.treeViewAnnotation.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewAnnotation_NodeMouseClick);
            // 
            // imageListAnnotationType
            // 
            this.imageListAnnotationType.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListAnnotationType.ImageStream")));
            this.imageListAnnotationType.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListAnnotationType.Images.SetKeyName(0, "Note.ico");
            this.imageListAnnotationType.Images.SetKeyName(1, "References.ico");
            this.imageListAnnotationType.Images.SetKeyName(2, "Problem.ico");
            this.imageListAnnotationType.Images.SetKeyName(3, "Identifier.ico");
            // 
            // toolStripAnnotation
            // 
            this.toolStripAnnotation.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripAnnotation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNewRootAnnotation,
            this.toolStripButtonNewAnnotation,
            this.toolStripButtonDelete,
            this.toolStripButtonFeedback});
            this.toolStripAnnotation.Location = new System.Drawing.Point(3, 3);
            this.toolStripAnnotation.Name = "toolStripAnnotation";
            this.toolStripAnnotation.Size = new System.Drawing.Size(462, 25);
            this.toolStripAnnotation.TabIndex = 0;
            this.toolStripAnnotation.Text = "toolStrip1";
            // 
            // toolStripButtonNewRootAnnotation
            // 
            this.toolStripButtonNewRootAnnotation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNewRootAnnotation.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.toolStripButtonNewRootAnnotation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNewRootAnnotation.Name = "toolStripButtonNewRootAnnotation";
            this.toolStripButtonNewRootAnnotation.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNewRootAnnotation.Text = "Add a new root annotation";
            this.toolStripButtonNewRootAnnotation.Click += new System.EventHandler(this.toolStripButtonNewRootAnnotation_Click);
            // 
            // toolStripButtonNewAnnotation
            // 
            this.toolStripButtonNewAnnotation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNewAnnotation.Image = global::DiversityWorkbench.Properties.Resources.AddChild;
            this.toolStripButtonNewAnnotation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNewAnnotation.Name = "toolStripButtonNewAnnotation";
            this.toolStripButtonNewAnnotation.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNewAnnotation.Text = "Add a new annotation as child of the selected annotation";
            this.toolStripButtonNewAnnotation.Click += new System.EventHandler(this.toolStripButtonNewAnnotation_Click);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDelete.Enabled = false;
            this.toolStripButtonDelete.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDelete.Text = "Delete the selected annotation";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripButtonFeedback
            // 
            this.toolStripButtonFeedback.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonFeedback.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFeedback.Image = global::DiversityWorkbench.Properties.Resources.Feedback;
            this.toolStripButtonFeedback.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFeedback.Name = "toolStripButtonFeedback";
            this.toolStripButtonFeedback.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonFeedback.Text = "Send a feedback";
            this.toolStripButtonFeedback.Click += new System.EventHandler(this.toolStripButtonFeedback_Click);
            // 
            // tableLayoutPanelData
            // 
            this.tableLayoutPanelData.ColumnCount = 5;
            this.tableLayoutPanelData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelData.Controls.Add(this.textBoxAnnotation, 0, 4);
            this.tableLayoutPanelData.Controls.Add(this.userControlModuleRelatedEntryReference, 1, 3);
            this.tableLayoutPanelData.Controls.Add(this.linkLabelURL, 1, 5);
            this.tableLayoutPanelData.Controls.Add(this.buttonSetURL, 4, 5);
            this.tableLayoutPanelData.Controls.Add(this.labelCreatedBy, 0, 7);
            this.tableLayoutPanelData.Controls.Add(this.textBoxCreatedBy, 1, 7);
            this.tableLayoutPanelData.Controls.Add(this.labelCreatedWhen, 2, 7);
            this.tableLayoutPanelData.Controls.Add(this.textBoxCreatedWhen, 3, 7);
            this.tableLayoutPanelData.Controls.Add(this.panelAnnotationType, 0, 0);
            this.tableLayoutPanelData.Controls.Add(this.labelAnnotationTitle, 0, 2);
            this.tableLayoutPanelData.Controls.Add(this.userControlModuleRelatedEntrySource, 1, 6);
            this.tableLayoutPanelData.Controls.Add(this.labelSource, 0, 6);
            this.tableLayoutPanelData.Controls.Add(this.labelReference, 0, 3);
            this.tableLayoutPanelData.Controls.Add(this.comboBoxTitle, 1, 2);
            this.tableLayoutPanelData.Controls.Add(this.labelReferencedTable, 0, 1);
            this.tableLayoutPanelData.Controls.Add(this.textBoxReferencedTable, 1, 1);
            this.tableLayoutPanelData.Controls.Add(this.labelURL, 0, 5);
            this.tableLayoutPanelData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelData.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelData.Name = "tableLayoutPanelData";
            this.tableLayoutPanelData.RowCount = 8;
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelData.Size = new System.Drawing.Size(468, 211);
            this.tableLayoutPanelData.TabIndex = 0;
            // 
            // textBoxAnnotation
            // 
            this.tableLayoutPanelData.SetColumnSpan(this.textBoxAnnotation, 5);
            this.textBoxAnnotation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAnnotation.Location = new System.Drawing.Point(3, 102);
            this.textBoxAnnotation.Multiline = true;
            this.textBoxAnnotation.Name = "textBoxAnnotation";
            this.textBoxAnnotation.Size = new System.Drawing.Size(462, 31);
            this.textBoxAnnotation.TabIndex = 0;
            this.textBoxAnnotation.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxAnnotation_KeyUp);
            // 
            // userControlModuleRelatedEntryReference
            // 
            this.userControlModuleRelatedEntryReference.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelData.SetColumnSpan(this.userControlModuleRelatedEntryReference, 4);
            this.userControlModuleRelatedEntryReference.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryReference.Domain = "";
            this.userControlModuleRelatedEntryReference.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryReference.Location = new System.Drawing.Point(67, 74);
            this.userControlModuleRelatedEntryReference.Module = null;
            this.userControlModuleRelatedEntryReference.Name = "userControlModuleRelatedEntryReference";
            this.userControlModuleRelatedEntryReference.ShowInfo = false;
            this.userControlModuleRelatedEntryReference.Size = new System.Drawing.Size(398, 22);
            this.userControlModuleRelatedEntryReference.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryReference.TabIndex = 1;
            // 
            // linkLabelURL
            // 
            this.linkLabelURL.AutoSize = true;
            this.tableLayoutPanelData.SetColumnSpan(this.linkLabelURL, 3);
            this.linkLabelURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkLabelURL.Location = new System.Drawing.Point(67, 136);
            this.linkLabelURL.Name = "linkLabelURL";
            this.linkLabelURL.Size = new System.Drawing.Size(371, 27);
            this.linkLabelURL.TabIndex = 3;
            this.linkLabelURL.TabStop = true;
            this.linkLabelURL.Text = "http://...";
            this.linkLabelURL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabelURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelURL_LinkClicked);
            // 
            // buttonSetURL
            // 
            this.buttonSetURL.Image = global::DiversityWorkbench.Properties.Resources.Browse;
            this.buttonSetURL.Location = new System.Drawing.Point(441, 136);
            this.buttonSetURL.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.buttonSetURL.Name = "buttonSetURL";
            this.buttonSetURL.Size = new System.Drawing.Size(24, 24);
            this.buttonSetURL.TabIndex = 4;
            this.buttonSetURL.UseVisualStyleBackColor = true;
            this.buttonSetURL.Click += new System.EventHandler(this.buttonSetURL_Click);
            // 
            // labelCreatedBy
            // 
            this.labelCreatedBy.AutoSize = true;
            this.labelCreatedBy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCreatedBy.Location = new System.Drawing.Point(3, 191);
            this.labelCreatedBy.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelCreatedBy.Name = "labelCreatedBy";
            this.labelCreatedBy.Size = new System.Drawing.Size(61, 20);
            this.labelCreatedBy.TabIndex = 6;
            this.labelCreatedBy.Text = "Created by:";
            this.labelCreatedBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCreatedBy
            // 
            this.textBoxCreatedBy.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxCreatedBy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCreatedBy.Enabled = false;
            this.textBoxCreatedBy.Location = new System.Drawing.Point(67, 194);
            this.textBoxCreatedBy.Name = "textBoxCreatedBy";
            this.textBoxCreatedBy.Size = new System.Drawing.Size(138, 13);
            this.textBoxCreatedBy.TabIndex = 7;
            // 
            // labelCreatedWhen
            // 
            this.labelCreatedWhen.AutoSize = true;
            this.labelCreatedWhen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCreatedWhen.Location = new System.Drawing.Point(211, 191);
            this.labelCreatedWhen.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelCreatedWhen.Name = "labelCreatedWhen";
            this.labelCreatedWhen.Size = new System.Drawing.Size(86, 20);
            this.labelCreatedWhen.TabIndex = 8;
            this.labelCreatedWhen.Text = "Date of creation:";
            this.labelCreatedWhen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCreatedWhen
            // 
            this.textBoxCreatedWhen.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelData.SetColumnSpan(this.textBoxCreatedWhen, 2);
            this.textBoxCreatedWhen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCreatedWhen.Enabled = false;
            this.textBoxCreatedWhen.Location = new System.Drawing.Point(300, 194);
            this.textBoxCreatedWhen.Name = "textBoxCreatedWhen";
            this.textBoxCreatedWhen.Size = new System.Drawing.Size(165, 13);
            this.textBoxCreatedWhen.TabIndex = 9;
            // 
            // panelAnnotationType
            // 
            this.tableLayoutPanelData.SetColumnSpan(this.panelAnnotationType, 5);
            this.panelAnnotationType.Controls.Add(this.labelDataRow);
            this.panelAnnotationType.Controls.Add(this.labelAnnotationType);
            this.panelAnnotationType.Controls.Add(this.pictureBoxAnnotationType);
            this.panelAnnotationType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAnnotationType.Location = new System.Drawing.Point(0, 0);
            this.panelAnnotationType.Margin = new System.Windows.Forms.Padding(0);
            this.panelAnnotationType.Name = "panelAnnotationType";
            this.panelAnnotationType.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panelAnnotationType.Size = new System.Drawing.Size(468, 18);
            this.panelAnnotationType.TabIndex = 11;
            // 
            // labelDataRow
            // 
            this.labelDataRow.AutoSize = true;
            this.labelDataRow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDataRow.Location = new System.Drawing.Point(54, 0);
            this.labelDataRow.Name = "labelDataRow";
            this.labelDataRow.Size = new System.Drawing.Size(31, 13);
            this.labelDataRow.TabIndex = 11;
            this.labelDataRow.Text = "for ...";
            // 
            // labelAnnotationType
            // 
            this.labelAnnotationType.AutoSize = true;
            this.labelAnnotationType.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelAnnotationType.Location = new System.Drawing.Point(23, 0);
            this.labelAnnotationType.Name = "labelAnnotationType";
            this.labelAnnotationType.Size = new System.Drawing.Size(31, 13);
            this.labelAnnotationType.TabIndex = 10;
            this.labelAnnotationType.Text = "Type";
            // 
            // pictureBoxAnnotationType
            // 
            this.pictureBoxAnnotationType.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBoxAnnotationType.Location = new System.Drawing.Point(3, 0);
            this.pictureBoxAnnotationType.Name = "pictureBoxAnnotationType";
            this.pictureBoxAnnotationType.Size = new System.Drawing.Size(20, 18);
            this.pictureBoxAnnotationType.TabIndex = 12;
            this.pictureBoxAnnotationType.TabStop = false;
            // 
            // labelAnnotationTitle
            // 
            this.labelAnnotationTitle.AutoSize = true;
            this.labelAnnotationTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAnnotationTitle.Location = new System.Drawing.Point(3, 44);
            this.labelAnnotationTitle.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelAnnotationTitle.Name = "labelAnnotationTitle";
            this.labelAnnotationTitle.Size = new System.Drawing.Size(61, 27);
            this.labelAnnotationTitle.TabIndex = 12;
            this.labelAnnotationTitle.Text = "Topic:";
            this.labelAnnotationTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlModuleRelatedEntrySource
            // 
            this.userControlModuleRelatedEntrySource.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelData.SetColumnSpan(this.userControlModuleRelatedEntrySource, 4);
            this.userControlModuleRelatedEntrySource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntrySource.Domain = "";
            this.userControlModuleRelatedEntrySource.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntrySource.Location = new System.Drawing.Point(67, 166);
            this.userControlModuleRelatedEntrySource.Module = null;
            this.userControlModuleRelatedEntrySource.Name = "userControlModuleRelatedEntrySource";
            this.userControlModuleRelatedEntrySource.ShowInfo = false;
            this.userControlModuleRelatedEntrySource.Size = new System.Drawing.Size(398, 22);
            this.userControlModuleRelatedEntrySource.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntrySource.TabIndex = 14;
            // 
            // labelSource
            // 
            this.labelSource.AutoSize = true;
            this.labelSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSource.Location = new System.Drawing.Point(3, 163);
            this.labelSource.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(61, 28);
            this.labelSource.TabIndex = 15;
            this.labelSource.Text = "Source:";
            this.labelSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelReference
            // 
            this.labelReference.AutoSize = true;
            this.labelReference.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReference.Location = new System.Drawing.Point(3, 71);
            this.labelReference.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelReference.Name = "labelReference";
            this.labelReference.Size = new System.Drawing.Size(61, 28);
            this.labelReference.TabIndex = 16;
            this.labelReference.Text = "Reference:";
            this.labelReference.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxTitle
            // 
            this.tableLayoutPanelData.SetColumnSpan(this.comboBoxTitle, 4);
            this.comboBoxTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxTitle.FormattingEnabled = true;
            this.comboBoxTitle.Location = new System.Drawing.Point(67, 47);
            this.comboBoxTitle.Name = "comboBoxTitle";
            this.comboBoxTitle.Size = new System.Drawing.Size(398, 21);
            this.comboBoxTitle.TabIndex = 17;
            this.comboBoxTitle.DropDown += new System.EventHandler(this.comboBoxTitle_DropDown);
            // 
            // labelReferencedTable
            // 
            this.labelReferencedTable.AutoSize = true;
            this.labelReferencedTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReferencedTable.Location = new System.Drawing.Point(3, 18);
            this.labelReferencedTable.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelReferencedTable.Name = "labelReferencedTable";
            this.labelReferencedTable.Size = new System.Drawing.Size(61, 26);
            this.labelReferencedTable.TabIndex = 18;
            this.labelReferencedTable.Text = "Table:";
            this.labelReferencedTable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxReferencedTable
            // 
            this.tableLayoutPanelData.SetColumnSpan(this.textBoxReferencedTable, 4);
            this.textBoxReferencedTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxReferencedTable.Location = new System.Drawing.Point(67, 21);
            this.textBoxReferencedTable.Name = "textBoxReferencedTable";
            this.textBoxReferencedTable.ReadOnly = true;
            this.textBoxReferencedTable.Size = new System.Drawing.Size(398, 20);
            this.textBoxReferencedTable.TabIndex = 19;
            // 
            // labelURL
            // 
            this.labelURL.AutoSize = true;
            this.labelURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelURL.Location = new System.Drawing.Point(3, 136);
            this.labelURL.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelURL.Name = "labelURL";
            this.labelURL.Size = new System.Drawing.Size(61, 27);
            this.labelURL.TabIndex = 20;
            this.labelURL.Text = "URL:";
            this.labelURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelHeader.Location = new System.Drawing.Point(0, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(111, 13);
            this.labelHeader.TabIndex = 1;
            this.labelHeader.Text = "Add or edit annotation";
            // 
            // FormAnnotation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 480);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.labelHeader);
            this.helpProvider.SetHelpKeyword(this, "Annotation");
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormAnnotation";
            this.helpProvider.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Annotation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAnnotation_FormClosing);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel1.PerformLayout();
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.toolStripAnnotation.ResumeLayout(false);
            this.toolStripAnnotation.PerformLayout();
            this.tableLayoutPanelData.ResumeLayout(false);
            this.tableLayoutPanelData.PerformLayout();
            this.panelAnnotationType.ResumeLayout(false);
            this.panelAnnotationType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAnnotationType)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.ToolStrip toolStripAnnotation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelData;
        private System.Windows.Forms.ToolStripButton toolStripButtonNewAnnotation;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TreeView treeViewAnnotation;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.TextBox textBoxAnnotation;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryReference;
        private System.Windows.Forms.LinkLabel linkLabelURL;
        private System.Windows.Forms.Button buttonSetURL;
        private System.Windows.Forms.Label labelCreatedBy;
        private System.Windows.Forms.TextBox textBoxCreatedBy;
        private System.Windows.Forms.Label labelCreatedWhen;
        private System.Windows.Forms.TextBox textBoxCreatedWhen;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Panel panelAnnotationType;
        private System.Windows.Forms.Label labelAnnotationType;
        private System.Windows.Forms.Label labelDataRow;
        private System.Windows.Forms.PictureBox pictureBoxAnnotationType;
        private System.Windows.Forms.ImageList imageListAnnotationType;
        private System.Windows.Forms.Label labelAnnotationTitle;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntrySource;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.Label labelReference;
        private System.Windows.Forms.ComboBox comboBoxTitle;
        private System.Windows.Forms.Label labelReferencedTable;
        private System.Windows.Forms.TextBox textBoxReferencedTable;
        private System.Windows.Forms.Label labelURL;
        private System.Windows.Forms.ToolStripButton toolStripButtonNewRootAnnotation;
        private System.Windows.Forms.ToolStripButton toolStripButtonFeedback;
    }
}