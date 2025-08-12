namespace DiversityWorkbench.Forms
{
    partial class FormLoginAdminGetProjects
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLoginAdminGetProjects));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxDatabase = new System.Windows.Forms.ComboBox();
            this.labelDatabase = new System.Windows.Forms.Label();
            this.labelHeader = new System.Windows.Forms.Label();
            this.labelProjectsMissing = new System.Windows.Forms.Label();
            this.buttonStartDownload = new System.Windows.Forms.Button();
            this.treeViewProjects = new System.Windows.Forms.TreeView();
            this.imageListTreeView = new System.Windows.Forms.ImageList(this.components);
            this.buttonFilter = new System.Windows.Forms.Button();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.labelProjectsPresent = new System.Windows.Forms.Label();
            this.labelProjectsDiffering = new System.Windows.Forms.Label();
            this.checkBoxIncludeAll = new System.Windows.Forms.CheckBox();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buttonProjectProxyRefresh = new System.Windows.Forms.Button();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageDownload = new System.Windows.Forms.TabPage();
            this.tabPageProjectProxy = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelProjectProxy = new System.Windows.Forms.TableLayoutPanel();
            this.labelProjectProxy = new System.Windows.Forms.Label();
            this.dataGridViewProjectProxy = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanelMain.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageDownload.SuspendLayout();
            this.tabPageProjectProxy.SuspendLayout();
            this.tableLayoutPanelProjectProxy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProjectProxy)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 4;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.comboBoxDatabase, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelDatabase, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.labelProjectsMissing, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonStartDownload, 2, 6);
            this.tableLayoutPanelMain.Controls.Add(this.treeViewProjects, 0, 5);
            this.tableLayoutPanelMain.Controls.Add(this.buttonFilter, 0, 6);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxFilter, 1, 6);
            this.tableLayoutPanelMain.Controls.Add(this.labelProjectsPresent, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelProjectsDiffering, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxIncludeAll, 2, 2);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpProvider.SetHelpNavigator(this.tableLayoutPanelMain, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.helpProvider.SetHelpString(this.tableLayoutPanelMain, "Projects");
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 7;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.helpProvider.SetShowHelp(this.tableLayoutPanelMain, true);
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(388, 367);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // comboBoxDatabase
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.comboBoxDatabase, 3);
            this.comboBoxDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxDatabase.FormattingEnabled = true;
            this.comboBoxDatabase.Location = new System.Drawing.Point(62, 28);
            this.comboBoxDatabase.Name = "comboBoxDatabase";
            this.comboBoxDatabase.Size = new System.Drawing.Size(323, 21);
            this.comboBoxDatabase.TabIndex = 1;
            this.toolTip.SetToolTip(this.comboBoxDatabase, "The project database from where the projects should be downloaded");
            this.comboBoxDatabase.SelectedIndexChanged += new System.EventHandler(this.comboBoxDatabase_SelectedIndexChanged);
            this.comboBoxDatabase.SelectionChangeCommitted += new System.EventHandler(this.comboBoxDatabase_SelectionChangeCommitted);
            // 
            // labelDatabase
            // 
            this.labelDatabase.AutoSize = true;
            this.labelDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDatabase.Location = new System.Drawing.Point(3, 25);
            this.labelDatabase.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelDatabase.Name = "labelDatabase";
            this.labelDatabase.Size = new System.Drawing.Size(56, 27);
            this.labelDatabase.TabIndex = 2;
            this.labelDatabase.Text = "Database:";
            this.labelDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelHeader, 4);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 6);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(382, 13);
            this.labelHeader.TabIndex = 3;
            this.labelHeader.Text = "Synchronize and download projects in your local database";
            // 
            // labelProjectsMissing
            // 
            this.labelProjectsMissing.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelProjectsMissing, 2);
            this.labelProjectsMissing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProjectsMissing.ForeColor = System.Drawing.Color.Red;
            this.labelProjectsMissing.Location = new System.Drawing.Point(3, 52);
            this.labelProjectsMissing.Name = "labelProjectsMissing";
            this.labelProjectsMissing.Size = new System.Drawing.Size(269, 13);
            this.labelProjectsMissing.TabIndex = 4;
            this.labelProjectsMissing.Text = "Projects missing in database";
            this.labelProjectsMissing.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // buttonStartDownload
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonStartDownload, 2);
            this.buttonStartDownload.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonStartDownload.Image = global::DiversityWorkbench.ResourceWorkbench.Download;
            this.buttonStartDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonStartDownload.Location = new System.Drawing.Point(278, 341);
            this.buttonStartDownload.Name = "buttonStartDownload";
            this.buttonStartDownload.Size = new System.Drawing.Size(107, 23);
            this.buttonStartDownload.TabIndex = 7;
            this.buttonStartDownload.Text = "Start download";
            this.buttonStartDownload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonStartDownload, "Start the download of the selected projects");
            this.buttonStartDownload.UseVisualStyleBackColor = true;
            this.buttonStartDownload.Click += new System.EventHandler(this.buttonStartDownload_Click);
            // 
            // treeViewProjects
            // 
            this.treeViewProjects.CheckBoxes = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.treeViewProjects, 4);
            this.treeViewProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewProjects.ImageIndex = 0;
            this.treeViewProjects.ImageList = this.imageListTreeView;
            this.treeViewProjects.Location = new System.Drawing.Point(3, 94);
            this.treeViewProjects.Name = "treeViewProjects";
            this.treeViewProjects.SelectedImageIndex = 0;
            this.treeViewProjects.Size = new System.Drawing.Size(382, 241);
            this.treeViewProjects.TabIndex = 8;
            this.treeViewProjects.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewProjects_AfterCheck);
            this.treeViewProjects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewProjects_AfterSelect);
            // 
            // imageListTreeView
            // 
            this.imageListTreeView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTreeView.ImageStream")));
            this.imageListTreeView.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTreeView.Images.SetKeyName(0, "Project.ico");
            this.imageListTreeView.Images.SetKeyName(1, "Checklist.ico");
            this.imageListTreeView.Images.SetKeyName(2, "DiversityAgents.ico");
            this.imageListTreeView.Images.SetKeyName(3, "DiversityCollection.ico");
            this.imageListTreeView.Images.SetKeyName(4, "DiversityGazetteer.ico");
            this.imageListTreeView.Images.SetKeyName(5, "DiversityProjects.ico");
            this.imageListTreeView.Images.SetKeyName(6, "DiversityReferences.ico");
            this.imageListTreeView.Images.SetKeyName(7, "DiversitySamplingPlots.ico");
            this.imageListTreeView.Images.SetKeyName(8, "DiversityScientificTerms.ico");
            this.imageListTreeView.Images.SetKeyName(9, "DiversityTaxonNames.ico");
            this.imageListTreeView.Images.SetKeyName(10, "Paragraph.ico");
            this.imageListTreeView.Images.SetKeyName(11, "NULL.ico");
            this.imageListTreeView.Images.SetKeyName(12, "DiversityDescriptions.ico");
            // 
            // buttonFilter
            // 
            this.buttonFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFilter.Image = global::DiversityWorkbench.Properties.Resources.Filter;
            this.buttonFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonFilter.Location = new System.Drawing.Point(3, 341);
            this.buttonFilter.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(56, 23);
            this.buttonFilter.TabIndex = 9;
            this.buttonFilter.Text = "Filter:";
            this.buttonFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.buttonFilter, "Filter the projects according to the given text");
            this.buttonFilter.UseVisualStyleBackColor = true;
            this.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxFilter.Location = new System.Drawing.Point(62, 341);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(86, 20);
            this.textBoxFilter.TabIndex = 10;
            this.toolTip.SetToolTip(this.textBoxFilter, "The text for the filter of the projects");
            // 
            // labelProjectsPresent
            // 
            this.labelProjectsPresent.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelProjectsPresent, 2);
            this.labelProjectsPresent.ForeColor = System.Drawing.Color.Green;
            this.labelProjectsPresent.Location = new System.Drawing.Point(3, 65);
            this.labelProjectsPresent.Name = "labelProjectsPresent";
            this.labelProjectsPresent.Size = new System.Drawing.Size(141, 13);
            this.labelProjectsPresent.TabIndex = 11;
            this.labelProjectsPresent.Text = "Projects present in database";
            // 
            // labelProjectsDiffering
            // 
            this.labelProjectsDiffering.AutoSize = true;
            this.labelProjectsDiffering.BackColor = System.Drawing.Color.Yellow;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelProjectsDiffering, 2);
            this.labelProjectsDiffering.ForeColor = System.Drawing.Color.Green;
            this.labelProjectsDiffering.Location = new System.Drawing.Point(3, 78);
            this.labelProjectsDiffering.Name = "labelProjectsDiffering";
            this.labelProjectsDiffering.Size = new System.Drawing.Size(136, 13);
            this.labelProjectsDiffering.TabIndex = 12;
            this.labelProjectsDiffering.Text = "Projects with differing name";
            // 
            // checkBoxIncludeAll
            // 
            this.checkBoxIncludeAll.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.checkBoxIncludeAll, 2);
            this.checkBoxIncludeAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxIncludeAll.Image = global::DiversityWorkbench.Properties.Resources.Project;
            this.checkBoxIncludeAll.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxIncludeAll.Location = new System.Drawing.Point(278, 55);
            this.checkBoxIncludeAll.Name = "checkBoxIncludeAll";
            this.tableLayoutPanelMain.SetRowSpan(this.checkBoxIncludeAll, 3);
            this.checkBoxIncludeAll.Size = new System.Drawing.Size(107, 33);
            this.checkBoxIncludeAll.TabIndex = 13;
            this.checkBoxIncludeAll.Text = "Include all";
            this.checkBoxIncludeAll.UseVisualStyleBackColor = true;
            this.checkBoxIncludeAll.Visible = false;
            this.checkBoxIncludeAll.Click += new System.EventHandler(this.checkBoxIncludeAll_Click);
            // 
            // buttonProjectProxyRefresh
            // 
            this.buttonProjectProxyRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonProjectProxyRefresh.FlatAppearance.BorderSize = 0;
            this.buttonProjectProxyRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonProjectProxyRefresh.Image = global::DiversityWorkbench.Properties.Resources.Transfrom;
            this.buttonProjectProxyRefresh.Location = new System.Drawing.Point(363, 0);
            this.buttonProjectProxyRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.buttonProjectProxyRefresh.Name = "buttonProjectProxyRefresh";
            this.buttonProjectProxyRefresh.Size = new System.Drawing.Size(25, 20);
            this.buttonProjectProxyRefresh.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonProjectProxyRefresh, "Requery project table");
            this.buttonProjectProxyRefresh.UseVisualStyleBackColor = true;
            this.buttonProjectProxyRefresh.Click += new System.EventHandler(this.buttonProjectProxyRefresh_Click);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageDownload);
            this.tabControlMain.Controls.Add(this.tabPageProjectProxy);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(402, 399);
            this.tabControlMain.TabIndex = 1;
            // 
            // tabPageDownload
            // 
            this.tabPageDownload.Controls.Add(this.tableLayoutPanelMain);
            this.tabPageDownload.Location = new System.Drawing.Point(4, 22);
            this.tabPageDownload.Name = "tabPageDownload";
            this.tabPageDownload.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDownload.Size = new System.Drawing.Size(394, 373);
            this.tabPageDownload.TabIndex = 0;
            this.tabPageDownload.Text = "Download/Synchronize";
            this.tabPageDownload.UseVisualStyleBackColor = true;
            // 
            // tabPageProjectProxy
            // 
            this.tabPageProjectProxy.Controls.Add(this.tableLayoutPanelProjectProxy);
            this.tabPageProjectProxy.Location = new System.Drawing.Point(4, 22);
            this.tabPageProjectProxy.Name = "tabPageProjectProxy";
            this.tabPageProjectProxy.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProjectProxy.Size = new System.Drawing.Size(394, 373);
            this.tabPageProjectProxy.TabIndex = 1;
            this.tabPageProjectProxy.Text = "Present projects";
            this.tabPageProjectProxy.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelProjectProxy
            // 
            this.tableLayoutPanelProjectProxy.ColumnCount = 2;
            this.tableLayoutPanelProjectProxy.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelProjectProxy.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelProjectProxy.Controls.Add(this.labelProjectProxy, 0, 0);
            this.tableLayoutPanelProjectProxy.Controls.Add(this.dataGridViewProjectProxy, 0, 1);
            this.tableLayoutPanelProjectProxy.Controls.Add(this.buttonProjectProxyRefresh, 1, 0);
            this.tableLayoutPanelProjectProxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelProjectProxy.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelProjectProxy.Name = "tableLayoutPanelProjectProxy";
            this.tableLayoutPanelProjectProxy.RowCount = 2;
            this.tableLayoutPanelProjectProxy.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProjectProxy.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelProjectProxy.Size = new System.Drawing.Size(388, 367);
            this.tableLayoutPanelProjectProxy.TabIndex = 0;
            // 
            // labelProjectProxy
            // 
            this.labelProjectProxy.AutoSize = true;
            this.labelProjectProxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProjectProxy.Location = new System.Drawing.Point(3, 0);
            this.labelProjectProxy.Name = "labelProjectProxy";
            this.labelProjectProxy.Size = new System.Drawing.Size(357, 20);
            this.labelProjectProxy.TabIndex = 0;
            this.labelProjectProxy.Text = "Projects already present in the database";
            this.labelProjectProxy.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // dataGridViewProjectProxy
            // 
            this.dataGridViewProjectProxy.AllowUserToAddRows = false;
            this.dataGridViewProjectProxy.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewProjectProxy.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanelProjectProxy.SetColumnSpan(this.dataGridViewProjectProxy, 2);
            this.dataGridViewProjectProxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewProjectProxy.Location = new System.Drawing.Point(3, 23);
            this.dataGridViewProjectProxy.Name = "dataGridViewProjectProxy";
            this.dataGridViewProjectProxy.ReadOnly = true;
            this.dataGridViewProjectProxy.RowHeadersVisible = false;
            this.dataGridViewProjectProxy.Size = new System.Drawing.Size(382, 341);
            this.dataGridViewProjectProxy.TabIndex = 1;
            // 
            // FormLoginAdminGetProjects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 399);
            this.Controls.Add(this.tabControlMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormLoginAdminGetProjects";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Get Projects";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageDownload.ResumeLayout(false);
            this.tabPageProjectProxy.ResumeLayout(false);
            this.tableLayoutPanelProjectProxy.ResumeLayout(false);
            this.tableLayoutPanelProjectProxy.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProjectProxy)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.ComboBox comboBoxDatabase;
        private System.Windows.Forms.Label labelDatabase;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Label labelProjectsMissing;
        private System.Windows.Forms.Button buttonStartDownload;
        private System.Windows.Forms.TreeView treeViewProjects;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonFilter;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.Label labelProjectsPresent;
        private System.Windows.Forms.Label labelProjectsDiffering;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageDownload;
        private System.Windows.Forms.TabPage tabPageProjectProxy;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProjectProxy;
        private System.Windows.Forms.Label labelProjectProxy;
        private System.Windows.Forms.DataGridView dataGridViewProjectProxy;
        private System.Windows.Forms.Button buttonProjectProxyRefresh;
        private System.Windows.Forms.CheckBox checkBoxIncludeAll;
        private System.Windows.Forms.ImageList imageListTreeView;
    }
}