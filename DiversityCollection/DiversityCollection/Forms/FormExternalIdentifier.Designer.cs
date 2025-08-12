namespace DiversityCollection.Forms
{
    partial class FormExternalIdentifier
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExternalIdentifier));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.labelType = new System.Windows.Forms.Label();
            this.textBoxID = new System.Windows.Forms.TextBox();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonSetURL = new System.Windows.Forms.Button();
            this.textBoxURL = new System.Windows.Forms.TextBox();
            this.labelNotes = new System.Windows.Forms.Label();
            this.textBoxNotes = new System.Windows.Forms.TextBox();
            this.labelURL = new System.Windows.Forms.Label();
            this.pictureBoxID = new System.Windows.Forms.PictureBox();
            this.userControlModuleRelatedEntryRestriction = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.treeViewIdentifier = new System.Windows.Forms.TreeView();
            this.imageListTree = new System.Windows.Forms.ImageList(this.components);
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxID)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.comboBoxType, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labelType, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxID, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonDelete, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonSetURL, 3, 2);
            this.tableLayoutPanel.Controls.Add(this.textBoxURL, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.labelNotes, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.textBoxNotes, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.labelURL, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxID, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.userControlModuleRelatedEntryRestriction, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.buttonFeedback, 3, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 6;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(294, 122);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // comboBoxType
            // 
            this.comboBoxType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Location = new System.Drawing.Point(41, 3);
            this.comboBoxType.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(196, 21);
            this.comboBoxType.TabIndex = 0;
            this.toolTip.SetToolTip(this.comboBoxType, "Type of the external identifier");
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelType.Location = new System.Drawing.Point(3, 0);
            this.labelType.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(38, 27);
            this.labelType.TabIndex = 1;
            this.labelType.Text = "Type:";
            this.labelType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxID
            // 
            this.tableLayoutPanel.SetColumnSpan(this.textBoxID, 3);
            this.textBoxID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxID.Location = new System.Drawing.Point(41, 27);
            this.textBoxID.Margin = new System.Windows.Forms.Padding(0, 0, 3, 2);
            this.textBoxID.Name = "textBoxID";
            this.textBoxID.Size = new System.Drawing.Size(250, 20);
            this.textBoxID.TabIndex = 2;
            this.toolTip.SetToolTip(this.textBoxID, "The external identifier");
            // 
            // buttonDelete
            // 
            this.buttonDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDelete.Image = global::DiversityCollection.Resource.Delete;
            this.buttonDelete.Location = new System.Drawing.Point(240, 2);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(0, 2, 3, 0);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(24, 25);
            this.buttonDelete.TabIndex = 3;
            this.toolTip.SetToolTip(this.buttonDelete, "Delete the external idenifier");
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonSetURL
            // 
            this.buttonSetURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSetURL.Image = global::DiversityCollection.Resource.Browse;
            this.buttonSetURL.Location = new System.Drawing.Point(267, 49);
            this.buttonSetURL.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.buttonSetURL.Name = "buttonSetURL";
            this.buttonSetURL.Size = new System.Drawing.Size(24, 24);
            this.buttonSetURL.TabIndex = 4;
            this.toolTip.SetToolTip(this.buttonSetURL, "Set the URL resp. open the given URL");
            this.buttonSetURL.UseVisualStyleBackColor = true;
            this.buttonSetURL.Click += new System.EventHandler(this.buttonSetURL_Click);
            // 
            // textBoxURL
            // 
            this.tableLayoutPanel.SetColumnSpan(this.textBoxURL, 2);
            this.textBoxURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxURL.Location = new System.Drawing.Point(41, 50);
            this.textBoxURL.Margin = new System.Windows.Forms.Padding(0, 1, 3, 3);
            this.textBoxURL.Name = "textBoxURL";
            this.textBoxURL.Size = new System.Drawing.Size(223, 20);
            this.textBoxURL.TabIndex = 5;
            this.toolTip.SetToolTip(this.textBoxURL, "The URL of the identifier");
            // 
            // labelNotes
            // 
            this.labelNotes.AutoSize = true;
            this.labelNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNotes.Location = new System.Drawing.Point(3, 104);
            this.labelNotes.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelNotes.Name = "labelNotes";
            this.labelNotes.Size = new System.Drawing.Size(38, 13);
            this.labelNotes.TabIndex = 6;
            this.labelNotes.Text = "Notes:";
            this.labelNotes.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxNotes
            // 
            this.tableLayoutPanel.SetColumnSpan(this.textBoxNotes, 3);
            this.textBoxNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNotes.Location = new System.Drawing.Point(41, 101);
            this.textBoxNotes.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxNotes.Multiline = true;
            this.textBoxNotes.Name = "textBoxNotes";
            this.tableLayoutPanel.SetRowSpan(this.textBoxNotes, 2);
            this.textBoxNotes.Size = new System.Drawing.Size(250, 18);
            this.textBoxNotes.TabIndex = 7;
            this.toolTip.SetToolTip(this.textBoxNotes, "Notes about the identifier");
            // 
            // labelURL
            // 
            this.labelURL.AutoSize = true;
            this.labelURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelURL.Location = new System.Drawing.Point(3, 49);
            this.labelURL.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelURL.Name = "labelURL";
            this.labelURL.Size = new System.Drawing.Size(38, 24);
            this.labelURL.TabIndex = 8;
            this.labelURL.Text = "URL:";
            this.labelURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pictureBoxID
            // 
            this.pictureBoxID.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBoxID.Image = global::DiversityCollection.Resource.Identifier;
            this.pictureBoxID.InitialImage = global::DiversityCollection.Resource.Identifier;
            this.pictureBoxID.Location = new System.Drawing.Point(23, 29);
            this.pictureBoxID.Margin = new System.Windows.Forms.Padding(3, 2, 2, 3);
            this.pictureBoxID.Name = "pictureBoxID";
            this.pictureBoxID.Size = new System.Drawing.Size(16, 17);
            this.pictureBoxID.TabIndex = 9;
            this.pictureBoxID.TabStop = false;
            // 
            // userControlModuleRelatedEntryRestriction
            // 
            this.userControlModuleRelatedEntryRestriction.CanDeleteConnectionToModule = true;
            this.tableLayoutPanel.SetColumnSpan(this.userControlModuleRelatedEntryRestriction, 4);
            this.userControlModuleRelatedEntryRestriction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryRestriction.Domain = "";
            this.userControlModuleRelatedEntryRestriction.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryRestriction.Location = new System.Drawing.Point(3, 76);
            this.userControlModuleRelatedEntryRestriction.Module = null;
            this.userControlModuleRelatedEntryRestriction.Name = "userControlModuleRelatedEntryRestriction";
            this.userControlModuleRelatedEntryRestriction.Size = new System.Drawing.Size(288, 22);
            this.userControlModuleRelatedEntryRestriction.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryRestriction.TabIndex = 10;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.tableLayoutPanel);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.treeViewIdentifier);
            this.splitContainerMain.Size = new System.Drawing.Size(294, 310);
            this.splitContainerMain.SplitterDistance = 122;
            this.splitContainerMain.TabIndex = 6;
            // 
            // treeViewIdentifier
            // 
            this.treeViewIdentifier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewIdentifier.ImageIndex = 0;
            this.treeViewIdentifier.ImageList = this.imageListTree;
            this.treeViewIdentifier.Location = new System.Drawing.Point(0, 0);
            this.treeViewIdentifier.Name = "treeViewIdentifier";
            this.treeViewIdentifier.SelectedImageIndex = 0;
            this.treeViewIdentifier.Size = new System.Drawing.Size(294, 184);
            this.treeViewIdentifier.TabIndex = 0;
            // 
            // imageListTree
            // 
            this.imageListTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTree.ImageStream")));
            this.imageListTree.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTree.Images.SetKeyName(0, "Identifier.ico");
            this.imageListTree.Images.SetKeyName(1, "Event.ico");
            this.imageListTree.Images.SetKeyName(2, "CollectionSpecimen.ico");
            this.imageListTree.Images.SetKeyName(3, "Plant.ico");
            this.imageListTree.Images.SetKeyName(4, "Specimen.ico");
            this.imageListTree.Images.SetKeyName(5, "Paragraph.ico");
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 310);
            this.userControlDialogPanel.Margin = new System.Windows.Forms.Padding(0);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(294, 27);
            this.userControlDialogPanel.TabIndex = 5;
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFeedback.Image = global::DiversityCollection.Resource.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(267, 0);
            this.buttonFeedback.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(27, 27);
            this.buttonFeedback.TabIndex = 11;
            this.toolTip.SetToolTip(this.buttonFeedback, "Send a feedback");
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // FormExternalIdentifier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 337);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.userControlDialogPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.helpProvider.SetHelpKeyword(this, "externalidentifier_dc");
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormExternalIdentifier";
            this.helpProvider.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "External Identifier";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxID)).EndInit();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.TextBox textBoxID;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonDelete;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.TreeView treeViewIdentifier;
        private System.Windows.Forms.ImageList imageListTree;
        private System.Windows.Forms.Button buttonSetURL;
        private System.Windows.Forms.TextBox textBoxURL;
        private System.Windows.Forms.Label labelNotes;
        private System.Windows.Forms.TextBox textBoxNotes;
        private System.Windows.Forms.Label labelURL;
        private System.Windows.Forms.PictureBox pictureBoxID;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryRestriction;
        private System.Windows.Forms.Button buttonFeedback;
    }
}