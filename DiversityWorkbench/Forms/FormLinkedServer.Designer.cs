namespace DiversityWorkbench.Forms
{
    partial class FormLinkedServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLinkedServer));
            this.toolStripServer = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAddServer = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDeleteServer = new System.Windows.Forms.ToolStripButton();
            this.tabControlLinkedServer = new System.Windows.Forms.TabControl();
            this.tabPageTables = new System.Windows.Forms.TabPage();
            this.splitContainerTables = new System.Windows.Forms.SplitContainer();
            this.listBoxTables = new System.Windows.Forms.ListBox();
            this.dataGridViewTable = new System.Windows.Forms.DataGridView();
            this.tabPageViews = new System.Windows.Forms.TabPage();
            this.splitContainerViews = new System.Windows.Forms.SplitContainer();
            this.listBoxViews = new System.Windows.Forms.ListBox();
            this.dataGridViewView = new System.Windows.Forms.DataGridView();
            this.labelViewError = new System.Windows.Forms.Label();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanelTablesAndViews = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxRestrictToNumber = new System.Windows.Forms.CheckBox();
            this.numericUpDownRestriction = new System.Windows.Forms.NumericUpDown();
            this.labelRestriction = new System.Windows.Forms.Label();
            this.labelDatabase = new System.Windows.Forms.Label();
            this.pictureBoxDatabase = new System.Windows.Forms.PictureBox();
            this.splitContainerServer = new System.Windows.Forms.SplitContainer();
            this.treeViewServer = new System.Windows.Forms.TreeView();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.toolStripServer.SuspendLayout();
            this.tabControlLinkedServer.SuspendLayout();
            this.tabPageTables.SuspendLayout();
            this.splitContainerTables.Panel1.SuspendLayout();
            this.splitContainerTables.Panel2.SuspendLayout();
            this.splitContainerTables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTable)).BeginInit();
            this.tabPageViews.SuspendLayout();
            this.splitContainerViews.Panel1.SuspendLayout();
            this.splitContainerViews.Panel2.SuspendLayout();
            this.splitContainerViews.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewView)).BeginInit();
            this.tableLayoutPanelTablesAndViews.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRestriction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDatabase)).BeginInit();
            this.splitContainerServer.Panel1.SuspendLayout();
            this.splitContainerServer.Panel2.SuspendLayout();
            this.splitContainerServer.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripServer
            // 
            this.toolStripServer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripServer.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripServer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAddServer,
            this.toolStripButtonDeleteServer});
            this.toolStripServer.Location = new System.Drawing.Point(0, 462);
            this.toolStripServer.Name = "toolStripServer";
            this.toolStripServer.Size = new System.Drawing.Size(209, 25);
            this.toolStripServer.TabIndex = 1;
            this.toolStripServer.Text = "toolStrip1";
            // 
            // toolStripButtonAddServer
            // 
            this.toolStripButtonAddServer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAddServer.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.toolStripButtonAddServer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddServer.Name = "toolStripButtonAddServer";
            this.toolStripButtonAddServer.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonAddServer.Text = "Add a new linked server";
            this.toolStripButtonAddServer.Click += new System.EventHandler(this.toolStripButtonAddServer_Click);
            // 
            // toolStripButtonDeleteServer
            // 
            this.toolStripButtonDeleteServer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDeleteServer.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.toolStripButtonDeleteServer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDeleteServer.Name = "toolStripButtonDeleteServer";
            this.toolStripButtonDeleteServer.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDeleteServer.Text = "Delete the selected linked server";
            this.toolStripButtonDeleteServer.Click += new System.EventHandler(this.toolStripButtonDeleteServer_Click);
            // 
            // tabControlLinkedServer
            // 
            this.tableLayoutPanelTablesAndViews.SetColumnSpan(this.tabControlLinkedServer, 4);
            this.tabControlLinkedServer.Controls.Add(this.tabPageTables);
            this.tabControlLinkedServer.Controls.Add(this.tabPageViews);
            this.tabControlLinkedServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlLinkedServer.ImageList = this.imageList;
            this.tabControlLinkedServer.Location = new System.Drawing.Point(3, 23);
            this.tabControlLinkedServer.Name = "tabControlLinkedServer";
            this.tabControlLinkedServer.SelectedIndex = 0;
            this.tabControlLinkedServer.Size = new System.Drawing.Size(476, 435);
            this.tabControlLinkedServer.TabIndex = 0;
            // 
            // tabPageTables
            // 
            this.tabPageTables.Controls.Add(this.splitContainerTables);
            this.tabPageTables.Location = new System.Drawing.Point(4, 23);
            this.tabPageTables.Name = "tabPageTables";
            this.tabPageTables.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTables.Size = new System.Drawing.Size(468, 408);
            this.tabPageTables.TabIndex = 0;
            this.tabPageTables.Text = "Tables";
            this.tabPageTables.UseVisualStyleBackColor = true;
            // 
            // splitContainerTables
            // 
            this.splitContainerTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTables.Location = new System.Drawing.Point(3, 3);
            this.splitContainerTables.Name = "splitContainerTables";
            // 
            // splitContainerTables.Panel1
            // 
            this.splitContainerTables.Panel1.Controls.Add(this.listBoxTables);
            // 
            // splitContainerTables.Panel2
            // 
            this.splitContainerTables.Panel2.Controls.Add(this.dataGridViewTable);
            this.splitContainerTables.Size = new System.Drawing.Size(462, 402);
            this.splitContainerTables.SplitterDistance = 153;
            this.splitContainerTables.TabIndex = 0;
            // 
            // listBoxTables
            // 
            this.listBoxTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxTables.FormattingEnabled = true;
            this.listBoxTables.IntegralHeight = false;
            this.listBoxTables.Location = new System.Drawing.Point(0, 0);
            this.listBoxTables.Name = "listBoxTables";
            this.listBoxTables.Size = new System.Drawing.Size(153, 402);
            this.listBoxTables.TabIndex = 0;
            this.listBoxTables.SelectedIndexChanged += new System.EventHandler(this.listBoxTables_SelectedIndexChanged);
            // 
            // dataGridViewTable
            // 
            this.dataGridViewTable.AllowUserToAddRows = false;
            this.dataGridViewTable.AllowUserToDeleteRows = false;
            this.dataGridViewTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTable.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewTable.Name = "dataGridViewTable";
            this.dataGridViewTable.ReadOnly = true;
            this.dataGridViewTable.RowHeadersVisible = false;
            this.dataGridViewTable.Size = new System.Drawing.Size(305, 402);
            this.dataGridViewTable.TabIndex = 0;
            // 
            // tabPageViews
            // 
            this.tabPageViews.Controls.Add(this.splitContainerViews);
            this.tabPageViews.Controls.Add(this.labelViewError);
            this.tabPageViews.Location = new System.Drawing.Point(4, 23);
            this.tabPageViews.Name = "tabPageViews";
            this.tabPageViews.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageViews.Size = new System.Drawing.Size(468, 408);
            this.tabPageViews.TabIndex = 1;
            this.tabPageViews.Text = "Views";
            this.tabPageViews.UseVisualStyleBackColor = true;
            // 
            // splitContainerViews
            // 
            this.splitContainerViews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerViews.Location = new System.Drawing.Point(3, 3);
            this.splitContainerViews.Name = "splitContainerViews";
            // 
            // splitContainerViews.Panel1
            // 
            this.splitContainerViews.Panel1.Controls.Add(this.listBoxViews);
            // 
            // splitContainerViews.Panel2
            // 
            this.splitContainerViews.Panel2.Controls.Add(this.dataGridViewView);
            this.splitContainerViews.Size = new System.Drawing.Size(462, 389);
            this.splitContainerViews.SplitterDistance = 152;
            this.splitContainerViews.TabIndex = 0;
            // 
            // listBoxViews
            // 
            this.listBoxViews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxViews.FormattingEnabled = true;
            this.listBoxViews.IntegralHeight = false;
            this.listBoxViews.Location = new System.Drawing.Point(0, 0);
            this.listBoxViews.Name = "listBoxViews";
            this.listBoxViews.Size = new System.Drawing.Size(152, 389);
            this.listBoxViews.TabIndex = 0;
            this.listBoxViews.SelectedIndexChanged += new System.EventHandler(this.listBoxViews_SelectedIndexChanged);
            // 
            // dataGridViewView
            // 
            this.dataGridViewView.AllowUserToAddRows = false;
            this.dataGridViewView.AllowUserToDeleteRows = false;
            this.dataGridViewView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewView.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewView.Name = "dataGridViewView";
            this.dataGridViewView.ReadOnly = true;
            this.dataGridViewView.RowHeadersVisible = false;
            this.dataGridViewView.Size = new System.Drawing.Size(306, 389);
            this.dataGridViewView.TabIndex = 0;
            // 
            // labelViewError
            // 
            this.labelViewError.AutoSize = true;
            this.labelViewError.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelViewError.ForeColor = System.Drawing.Color.Red;
            this.labelViewError.Location = new System.Drawing.Point(3, 392);
            this.labelViewError.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.labelViewError.Name = "labelViewError";
            this.labelViewError.Size = new System.Drawing.Size(65, 13);
            this.labelViewError.TabIndex = 1;
            this.labelViewError.Text = "Error in view";
            this.labelViewError.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "ServerLinked.ico");
            this.imageList.Images.SetKeyName(1, "DatabaseGrey.ico");
            this.imageList.Images.SetKeyName(2, "Speadsheet.ico");
            this.imageList.Images.SetKeyName(3, "Lupe.ico");
            // 
            // tableLayoutPanelTablesAndViews
            // 
            this.tableLayoutPanelTablesAndViews.ColumnCount = 4;
            this.tableLayoutPanelTablesAndViews.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelTablesAndViews.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTablesAndViews.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTablesAndViews.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTablesAndViews.Controls.Add(this.checkBoxRestrictToNumber, 0, 2);
            this.tableLayoutPanelTablesAndViews.Controls.Add(this.tabControlLinkedServer, 0, 1);
            this.tableLayoutPanelTablesAndViews.Controls.Add(this.numericUpDownRestriction, 2, 2);
            this.tableLayoutPanelTablesAndViews.Controls.Add(this.labelRestriction, 3, 2);
            this.tableLayoutPanelTablesAndViews.Controls.Add(this.labelDatabase, 1, 0);
            this.tableLayoutPanelTablesAndViews.Controls.Add(this.pictureBoxDatabase, 0, 0);
            this.tableLayoutPanelTablesAndViews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTablesAndViews.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelTablesAndViews.Name = "tableLayoutPanelTablesAndViews";
            this.tableLayoutPanelTablesAndViews.RowCount = 3;
            this.tableLayoutPanelTablesAndViews.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelTablesAndViews.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTablesAndViews.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTablesAndViews.Size = new System.Drawing.Size(482, 487);
            this.tableLayoutPanelTablesAndViews.TabIndex = 1;
            // 
            // checkBoxRestrictToNumber
            // 
            this.checkBoxRestrictToNumber.AutoSize = true;
            this.checkBoxRestrictToNumber.Checked = true;
            this.checkBoxRestrictToNumber.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanelTablesAndViews.SetColumnSpan(this.checkBoxRestrictToNumber, 2);
            this.checkBoxRestrictToNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxRestrictToNumber.Location = new System.Drawing.Point(3, 464);
            this.checkBoxRestrictToNumber.Name = "checkBoxRestrictToNumber";
            this.checkBoxRestrictToNumber.Size = new System.Drawing.Size(92, 20);
            this.checkBoxRestrictToNumber.TabIndex = 0;
            this.checkBoxRestrictToNumber.Text = "Restrict to top";
            this.checkBoxRestrictToNumber.UseVisualStyleBackColor = true;
            // 
            // numericUpDownRestriction
            // 
            this.numericUpDownRestriction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDownRestriction.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownRestriction.Location = new System.Drawing.Point(101, 464);
            this.numericUpDownRestriction.Name = "numericUpDownRestriction";
            this.numericUpDownRestriction.Size = new System.Drawing.Size(60, 20);
            this.numericUpDownRestriction.TabIndex = 1;
            this.numericUpDownRestriction.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownRestriction.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // labelRestriction
            // 
            this.labelRestriction.AutoSize = true;
            this.labelRestriction.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelRestriction.Location = new System.Drawing.Point(167, 461);
            this.labelRestriction.Name = "labelRestriction";
            this.labelRestriction.Size = new System.Drawing.Size(47, 26);
            this.labelRestriction.TabIndex = 2;
            this.labelRestriction.Text = "datasets";
            this.labelRestriction.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDatabase
            // 
            this.labelDatabase.AutoSize = true;
            this.tableLayoutPanelTablesAndViews.SetColumnSpan(this.labelDatabase, 3);
            this.labelDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDatabase.ForeColor = System.Drawing.Color.Gray;
            this.labelDatabase.Location = new System.Drawing.Point(23, 0);
            this.labelDatabase.Name = "labelDatabase";
            this.labelDatabase.Size = new System.Drawing.Size(456, 20);
            this.labelDatabase.TabIndex = 3;
            this.labelDatabase.Text = "Database";
            this.labelDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBoxDatabase
            // 
            this.pictureBoxDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxDatabase.Image = global::DiversityWorkbench.ResourceWorkbench.DatabaseGrey;
            this.pictureBoxDatabase.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxDatabase.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.pictureBoxDatabase.Name = "pictureBoxDatabase";
            this.pictureBoxDatabase.Size = new System.Drawing.Size(17, 17);
            this.pictureBoxDatabase.TabIndex = 4;
            this.pictureBoxDatabase.TabStop = false;
            // 
            // splitContainerServer
            // 
            this.splitContainerServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerServer.Location = new System.Drawing.Point(0, 0);
            this.splitContainerServer.Name = "splitContainerServer";
            // 
            // splitContainerServer.Panel1
            // 
            this.splitContainerServer.Panel1.Controls.Add(this.treeViewServer);
            this.splitContainerServer.Panel1.Controls.Add(this.toolStripServer);
            // 
            // splitContainerServer.Panel2
            // 
            this.splitContainerServer.Panel2.Controls.Add(this.tableLayoutPanelTablesAndViews);
            this.splitContainerServer.Size = new System.Drawing.Size(695, 487);
            this.splitContainerServer.SplitterDistance = 209;
            this.splitContainerServer.TabIndex = 1;
            // 
            // treeViewServer
            // 
            this.treeViewServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewServer.ImageIndex = 0;
            this.treeViewServer.ImageList = this.imageList;
            this.treeViewServer.Location = new System.Drawing.Point(0, 0);
            this.treeViewServer.Name = "treeViewServer";
            this.treeViewServer.SelectedImageIndex = 0;
            this.treeViewServer.Size = new System.Drawing.Size(209, 462);
            this.treeViewServer.TabIndex = 2;
            this.treeViewServer.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewServer_AfterSelect);
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFeedback.Image = global::DiversityWorkbench.Properties.Resources.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(670, 0);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(24, 23);
            this.buttonFeedback.TabIndex = 2;
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // FormLinkedServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 487);
            this.Controls.Add(this.buttonFeedback);
            this.Controls.Add(this.splitContainerServer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormLinkedServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Linked server";
            this.toolStripServer.ResumeLayout(false);
            this.toolStripServer.PerformLayout();
            this.tabControlLinkedServer.ResumeLayout(false);
            this.tabPageTables.ResumeLayout(false);
            this.splitContainerTables.Panel1.ResumeLayout(false);
            this.splitContainerTables.Panel2.ResumeLayout(false);
            this.splitContainerTables.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTable)).EndInit();
            this.tabPageViews.ResumeLayout(false);
            this.tabPageViews.PerformLayout();
            this.splitContainerViews.Panel1.ResumeLayout(false);
            this.splitContainerViews.Panel2.ResumeLayout(false);
            this.splitContainerViews.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewView)).EndInit();
            this.tableLayoutPanelTablesAndViews.ResumeLayout(false);
            this.tableLayoutPanelTablesAndViews.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRestriction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDatabase)).EndInit();
            this.splitContainerServer.Panel1.ResumeLayout(false);
            this.splitContainerServer.Panel1.PerformLayout();
            this.splitContainerServer.Panel2.ResumeLayout(false);
            this.splitContainerServer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripServer;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddServer;
        private System.Windows.Forms.ToolStripButton toolStripButtonDeleteServer;
        private System.Windows.Forms.TabControl tabControlLinkedServer;
        private System.Windows.Forms.TabPage tabPageTables;
        private System.Windows.Forms.SplitContainer splitContainerTables;
        private System.Windows.Forms.TabPage tabPageViews;
        private System.Windows.Forms.SplitContainer splitContainerViews;
        private System.Windows.Forms.ListBox listBoxTables;
        private System.Windows.Forms.DataGridView dataGridViewTable;
        private System.Windows.Forms.ListBox listBoxViews;
        private System.Windows.Forms.DataGridView dataGridViewView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTablesAndViews;
        private System.Windows.Forms.CheckBox checkBoxRestrictToNumber;
        private System.Windows.Forms.NumericUpDown numericUpDownRestriction;
        private System.Windows.Forms.Label labelRestriction;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.SplitContainer splitContainerServer;
        private System.Windows.Forms.TreeView treeViewServer;
        private System.Windows.Forms.Label labelDatabase;
        private System.Windows.Forms.PictureBox pictureBoxDatabase;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.Label labelViewError;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}