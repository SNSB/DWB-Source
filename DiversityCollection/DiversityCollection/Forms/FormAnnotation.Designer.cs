namespace DiversityCollection.Forms
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAnnotation));
            splitContainerMain = new System.Windows.Forms.SplitContainer();
            treeViewAnnotation = new System.Windows.Forms.TreeView();
            imageListAnnotationType = new System.Windows.Forms.ImageList(components);
            toolStripAnnotation = new System.Windows.Forms.ToolStrip();
            toolStripButtonNewRoot = new System.Windows.Forms.ToolStripButton();
            toolStripButtonNewAnnotation = new System.Windows.Forms.ToolStripButton();
            toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            tableLayoutPanelData = new System.Windows.Forms.TableLayoutPanel();
            textBoxAnnotation = new System.Windows.Forms.TextBox();
            userControlModuleRelatedEntryReference = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            linkLabelURL = new System.Windows.Forms.LinkLabel();
            buttonSetURL = new System.Windows.Forms.Button();
            labelCreatedBy = new System.Windows.Forms.Label();
            textBoxCreatedBy = new System.Windows.Forms.TextBox();
            labelCreatedWhen = new System.Windows.Forms.Label();
            textBoxCreatedWhen = new System.Windows.Forms.TextBox();
            panelAnnotationType = new System.Windows.Forms.Panel();
            labelDataRow = new System.Windows.Forms.Label();
            labelAnnotationType = new System.Windows.Forms.Label();
            pictureBoxAnnotationType = new System.Windows.Forms.PictureBox();
            toolTip = new System.Windows.Forms.ToolTip(components);
            labelHeader = new System.Windows.Forms.Label();
            helpProvider = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            toolStripAnnotation.SuspendLayout();
            tableLayoutPanelData.SuspendLayout();
            panelAnnotationType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAnnotationType).BeginInit();
            SuspendLayout();
            // 
            // splitContainerMain
            // 
            splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(splitContainerMain, "annotation_dc ");
            splitContainerMain.Location = new System.Drawing.Point(0, 15);
            splitContainerMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerMain.Name = "splitContainerMain";
            splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(treeViewAnnotation);
            splitContainerMain.Panel1.Controls.Add(toolStripAnnotation);
            splitContainerMain.Panel1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(tableLayoutPanelData);
            helpProvider.SetShowHelp(splitContainerMain, true);
            splitContainerMain.Size = new System.Drawing.Size(546, 419);
            splitContainerMain.SplitterDistance = 226;
            splitContainerMain.SplitterWidth = 5;
            splitContainerMain.TabIndex = 0;
            // 
            // treeViewAnnotation
            // 
            treeViewAnnotation.Dock = System.Windows.Forms.DockStyle.Fill;
            treeViewAnnotation.ImageIndex = 0;
            treeViewAnnotation.ImageList = imageListAnnotationType;
            treeViewAnnotation.Location = new System.Drawing.Point(4, 28);
            treeViewAnnotation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            treeViewAnnotation.Name = "treeViewAnnotation";
            treeViewAnnotation.SelectedImageIndex = 0;
            treeViewAnnotation.Size = new System.Drawing.Size(538, 195);
            treeViewAnnotation.TabIndex = 1;
            treeViewAnnotation.AfterSelect += treeViewAnnotation_AfterSelect;
            // 
            // imageListAnnotationType
            // 
            imageListAnnotationType.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListAnnotationType.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListAnnotationType.ImageStream");
            imageListAnnotationType.TransparentColor = System.Drawing.Color.Transparent;
            imageListAnnotationType.Images.SetKeyName(0, "Note.ico");
            imageListAnnotationType.Images.SetKeyName(1, "References.ico");
            imageListAnnotationType.Images.SetKeyName(2, "Problem.ico");
            // 
            // toolStripAnnotation
            // 
            toolStripAnnotation.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripAnnotation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonNewRoot, toolStripButtonNewAnnotation, toolStripButtonDelete });
            toolStripAnnotation.Location = new System.Drawing.Point(4, 3);
            toolStripAnnotation.Name = "toolStripAnnotation";
            toolStripAnnotation.Size = new System.Drawing.Size(538, 25);
            toolStripAnnotation.TabIndex = 0;
            toolStripAnnotation.Text = "toolStrip1";
            // 
            // toolStripButtonNewRoot
            // 
            toolStripButtonNewRoot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonNewRoot.Image = Resource.New;
            toolStripButtonNewRoot.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButtonNewRoot.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonNewRoot.Name = "toolStripButtonNewRoot";
            toolStripButtonNewRoot.Size = new System.Drawing.Size(23, 22);
            toolStripButtonNewRoot.Text = "Add a new toppic annotation";
            toolStripButtonNewRoot.Click += toolStripButtonNewRoot_Click;
            // 
            // toolStripButtonNewAnnotation
            // 
            toolStripButtonNewAnnotation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonNewAnnotation.Image = Resource.Add1;
            toolStripButtonNewAnnotation.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonNewAnnotation.Name = "toolStripButtonNewAnnotation";
            toolStripButtonNewAnnotation.Size = new System.Drawing.Size(23, 22);
            toolStripButtonNewAnnotation.Text = "Add a new annotation";
            toolStripButtonNewAnnotation.Click += toolStripButtonNewAnnotation_Click;
            // 
            // toolStripButtonDelete
            // 
            toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonDelete.Enabled = false;
            toolStripButtonDelete.Image = Resource.Delete;
            toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonDelete.Name = "toolStripButtonDelete";
            toolStripButtonDelete.Size = new System.Drawing.Size(23, 22);
            toolStripButtonDelete.Text = "Delete the selected annotation";
            toolStripButtonDelete.Click += toolStripButtonDelete_Click;
            // 
            // tableLayoutPanelData
            // 
            tableLayoutPanelData.ColumnCount = 5;
            tableLayoutPanelData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelData.Controls.Add(textBoxAnnotation, 0, 2);
            tableLayoutPanelData.Controls.Add(userControlModuleRelatedEntryReference, 0, 1);
            tableLayoutPanelData.Controls.Add(linkLabelURL, 0, 3);
            tableLayoutPanelData.Controls.Add(buttonSetURL, 4, 3);
            tableLayoutPanelData.Controls.Add(labelCreatedBy, 0, 4);
            tableLayoutPanelData.Controls.Add(textBoxCreatedBy, 1, 4);
            tableLayoutPanelData.Controls.Add(labelCreatedWhen, 2, 4);
            tableLayoutPanelData.Controls.Add(textBoxCreatedWhen, 3, 4);
            tableLayoutPanelData.Controls.Add(panelAnnotationType, 0, 0);
            tableLayoutPanelData.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelData.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelData.Name = "tableLayoutPanelData";
            tableLayoutPanelData.RowCount = 5;
            tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelData.Size = new System.Drawing.Size(546, 188);
            tableLayoutPanelData.TabIndex = 0;
            // 
            // textBoxAnnotation
            // 
            tableLayoutPanelData.SetColumnSpan(textBoxAnnotation, 5);
            textBoxAnnotation.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxAnnotation.Location = new System.Drawing.Point(4, 55);
            textBoxAnnotation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxAnnotation.Multiline = true;
            textBoxAnnotation.Name = "textBoxAnnotation";
            textBoxAnnotation.Size = new System.Drawing.Size(538, 76);
            textBoxAnnotation.TabIndex = 0;
            textBoxAnnotation.KeyUp += textBoxAnnotation_KeyUp;
            // 
            // userControlModuleRelatedEntryReference
            // 
            userControlModuleRelatedEntryReference.CanDeleteConnectionToModule = true;
            tableLayoutPanelData.SetColumnSpan(userControlModuleRelatedEntryReference, 5);
            userControlModuleRelatedEntryReference.DependsOnUri = "";
            userControlModuleRelatedEntryReference.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleRelatedEntryReference.Domain = "";
            userControlModuleRelatedEntryReference.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryReference.Location = new System.Drawing.Point(5, 24);
            userControlModuleRelatedEntryReference.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlModuleRelatedEntryReference.Module = null;
            userControlModuleRelatedEntryReference.Name = "userControlModuleRelatedEntryReference";
            userControlModuleRelatedEntryReference.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryReference.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryReference.ShowInfo = false;
            userControlModuleRelatedEntryReference.Size = new System.Drawing.Size(536, 25);
            userControlModuleRelatedEntryReference.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryReference.TabIndex = 1;
            // 
            // linkLabelURL
            // 
            linkLabelURL.AutoSize = true;
            tableLayoutPanelData.SetColumnSpan(linkLabelURL, 4);
            linkLabelURL.Dock = System.Windows.Forms.DockStyle.Fill;
            linkLabelURL.Location = new System.Drawing.Point(4, 134);
            linkLabelURL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            linkLabelURL.Name = "linkLabelURL";
            linkLabelURL.Size = new System.Drawing.Size(505, 31);
            linkLabelURL.TabIndex = 3;
            linkLabelURL.TabStop = true;
            linkLabelURL.Text = "linkLabel1";
            linkLabelURL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonSetURL
            // 
            buttonSetURL.Image = Resource.Browse;
            buttonSetURL.Location = new System.Drawing.Point(513, 134);
            buttonSetURL.Margin = new System.Windows.Forms.Padding(0, 0, 4, 3);
            buttonSetURL.Name = "buttonSetURL";
            buttonSetURL.Size = new System.Drawing.Size(28, 28);
            buttonSetURL.TabIndex = 4;
            buttonSetURL.UseVisualStyleBackColor = true;
            buttonSetURL.Click += buttonSetURL_Click;
            // 
            // labelCreatedBy
            // 
            labelCreatedBy.AutoSize = true;
            labelCreatedBy.Dock = System.Windows.Forms.DockStyle.Fill;
            labelCreatedBy.Location = new System.Drawing.Point(4, 165);
            labelCreatedBy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelCreatedBy.Name = "labelCreatedBy";
            labelCreatedBy.Size = new System.Drawing.Size(67, 23);
            labelCreatedBy.TabIndex = 6;
            labelCreatedBy.Text = "Created by:";
            labelCreatedBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCreatedBy
            // 
            textBoxCreatedBy.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBoxCreatedBy.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxCreatedBy.Enabled = false;
            textBoxCreatedBy.Location = new System.Drawing.Point(79, 168);
            textBoxCreatedBy.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxCreatedBy.Name = "textBoxCreatedBy";
            textBoxCreatedBy.Size = new System.Drawing.Size(160, 16);
            textBoxCreatedBy.TabIndex = 7;
            // 
            // labelCreatedWhen
            // 
            labelCreatedWhen.AutoSize = true;
            labelCreatedWhen.Dock = System.Windows.Forms.DockStyle.Fill;
            labelCreatedWhen.Location = new System.Drawing.Point(247, 165);
            labelCreatedWhen.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelCreatedWhen.Name = "labelCreatedWhen";
            labelCreatedWhen.Size = new System.Drawing.Size(94, 23);
            labelCreatedWhen.TabIndex = 8;
            labelCreatedWhen.Text = "Date of creation:";
            labelCreatedWhen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCreatedWhen
            // 
            textBoxCreatedWhen.BorderStyle = System.Windows.Forms.BorderStyle.None;
            tableLayoutPanelData.SetColumnSpan(textBoxCreatedWhen, 2);
            textBoxCreatedWhen.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxCreatedWhen.Enabled = false;
            textBoxCreatedWhen.Location = new System.Drawing.Point(349, 168);
            textBoxCreatedWhen.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxCreatedWhen.Name = "textBoxCreatedWhen";
            textBoxCreatedWhen.Size = new System.Drawing.Size(193, 16);
            textBoxCreatedWhen.TabIndex = 9;
            // 
            // panelAnnotationType
            // 
            tableLayoutPanelData.SetColumnSpan(panelAnnotationType, 5);
            panelAnnotationType.Controls.Add(labelDataRow);
            panelAnnotationType.Controls.Add(labelAnnotationType);
            panelAnnotationType.Controls.Add(pictureBoxAnnotationType);
            panelAnnotationType.Dock = System.Windows.Forms.DockStyle.Fill;
            panelAnnotationType.Location = new System.Drawing.Point(0, 0);
            panelAnnotationType.Margin = new System.Windows.Forms.Padding(0);
            panelAnnotationType.Name = "panelAnnotationType";
            panelAnnotationType.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            panelAnnotationType.Size = new System.Drawing.Size(546, 21);
            panelAnnotationType.TabIndex = 11;
            // 
            // labelDataRow
            // 
            labelDataRow.AutoSize = true;
            labelDataRow.Dock = System.Windows.Forms.DockStyle.Fill;
            labelDataRow.Location = new System.Drawing.Point(58, 0);
            labelDataRow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelDataRow.Name = "labelDataRow";
            labelDataRow.Size = new System.Drawing.Size(34, 15);
            labelDataRow.TabIndex = 11;
            labelDataRow.Text = "for ...";
            // 
            // labelAnnotationType
            // 
            labelAnnotationType.AutoSize = true;
            labelAnnotationType.Dock = System.Windows.Forms.DockStyle.Left;
            labelAnnotationType.Location = new System.Drawing.Point(27, 0);
            labelAnnotationType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelAnnotationType.Name = "labelAnnotationType";
            labelAnnotationType.Size = new System.Drawing.Size(31, 15);
            labelAnnotationType.TabIndex = 10;
            labelAnnotationType.Text = "Type";
            // 
            // pictureBoxAnnotationType
            // 
            pictureBoxAnnotationType.Dock = System.Windows.Forms.DockStyle.Left;
            pictureBoxAnnotationType.Location = new System.Drawing.Point(4, 0);
            pictureBoxAnnotationType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBoxAnnotationType.Name = "pictureBoxAnnotationType";
            pictureBoxAnnotationType.Size = new System.Drawing.Size(23, 21);
            pictureBoxAnnotationType.TabIndex = 12;
            pictureBoxAnnotationType.TabStop = false;
            // 
            // labelHeader
            // 
            labelHeader.AutoSize = true;
            labelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            labelHeader.Location = new System.Drawing.Point(0, 0);
            labelHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new System.Drawing.Size(127, 15);
            labelHeader.TabIndex = 1;
            labelHeader.Text = "Add or edit annotation";
            // 
            // FormAnnotation
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(546, 434);
            Controls.Add(splitContainerMain);
            Controls.Add(labelHeader);
            helpProvider.SetHelpKeyword(this, "annotation_dc");
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormAnnotation";
            helpProvider.SetShowHelp(this, true);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Annotation";
            KeyDown += Form_KeyDown;
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel1.PerformLayout();
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            toolStripAnnotation.ResumeLayout(false);
            toolStripAnnotation.PerformLayout();
            tableLayoutPanelData.ResumeLayout(false);
            tableLayoutPanelData.PerformLayout();
            panelAnnotationType.ResumeLayout(false);
            panelAnnotationType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAnnotationType).EndInit();
            ResumeLayout(false);
            PerformLayout();

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
        private System.Windows.Forms.ToolStripButton toolStripButtonNewRoot;
    }
}