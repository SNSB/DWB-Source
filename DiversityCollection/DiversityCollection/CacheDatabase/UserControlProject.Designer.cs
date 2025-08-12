namespace DiversityCollection.CacheDatabase
{
    partial class UserControlProject
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlProject));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.buttonProjectTransferToPostgresFilter = new System.Windows.Forms.Button();
            this.buttonProjectTransferToCacheDBFilter = new System.Windows.Forms.Button();
            this.contextMenuStripFilter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewDifferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelProject = new System.Windows.Forms.Label();
            this.labelNumberInDatabase = new System.Windows.Forms.Label();
            this.labelNumberInCacheDB = new System.Windows.Forms.Label();
            this.labelNumberInPostgresDB = new System.Windows.Forms.Label();
            this.labelLastUpdateInDatabase = new System.Windows.Forms.Label();
            this.labelLastUpdateInCacheDatabase = new System.Windows.Forms.Label();
            this.labelLastUpdateInPostgresDatabase = new System.Windows.Forms.Label();
            this.buttonSetTransferFilter = new System.Windows.Forms.Button();
            this.buttonViewCacheContent = new System.Windows.Forms.Button();
            this.buttonViewPostgresContent = new System.Windows.Forms.Button();
            this.buttonAdministratePackages = new System.Windows.Forms.Button();
            this.listBoxPackages = new System.Windows.Forms.ListBox();
            this.buttonTransferDatabaseToCache = new System.Windows.Forms.Button();
            this.buttonRemoveProject = new System.Windows.Forms.Button();
            this.buttonPostgresEstablishProject = new System.Windows.Forms.Button();
            this.buttonUpdateCache = new System.Windows.Forms.Button();
            this.buttonUpdatePostgres = new System.Windows.Forms.Button();
            this.checkBoxPostgresIncludeInTransfer = new System.Windows.Forms.CheckBox();
            this.buttonPostgresTransferSettings = new System.Windows.Forms.Button();
            this.buttonPostgresTransferErrors = new System.Windows.Forms.Button();
            this.buttonPostgresTransferProtocol = new System.Windows.Forms.Button();
            this.buttonTransferErrors = new System.Windows.Forms.Button();
            this.buttonTransferProtocol = new System.Windows.Forms.Button();
            this.buttonIncludeInTransfer = new System.Windows.Forms.Button();
            this.checkBoxIncludeInTransfer = new System.Windows.Forms.CheckBox();
            this.buttonTransferPackageData = new System.Windows.Forms.Button();
            this.buttonDatawithholding = new System.Windows.Forms.Button();
            this.labelDatawithholding = new System.Windows.Forms.Label();
            this.buttonProjectTargets = new System.Windows.Forms.Button();
            this.buttonHistoryInCacheDB = new System.Windows.Forms.Button();
            this.buttonHistoryInPostgres = new System.Windows.Forms.Button();
            this.buttonCreateArchive = new System.Windows.Forms.Button();
            this.buttonGetDoi = new System.Windows.Forms.Button();
            this.labelDoiInCacheDB = new System.Windows.Forms.Label();
            this.labelDoiInPostgres = new System.Windows.Forms.Label();
            this.labelDoiInPackage = new System.Windows.Forms.Label();
            this.panelTransferCacheToPostgres = new System.Windows.Forms.Panel();
            this.buttonTransferCacheToPostgresFile = new System.Windows.Forms.Button();
            this.buttonTransferCacheToPostgres = new System.Windows.Forms.Button();
            this.checkBoxPostgresUseBulkTransfer = new System.Windows.Forms.CheckBox();
            this.panelPostgresOtherTargets = new System.Windows.Forms.Panel();
            this.buttonProjectDifferingName = new System.Windows.Forms.Button();
            this.labelProjectID = new System.Windows.Forms.Label();
            this.textBoxProjectID = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.imageListButtonIcons = new System.Windows.Forms.ImageList(this.components);
            this.imageListTransferSteps = new System.Windows.Forms.ImageList(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanelMain.SuspendLayout();
            this.contextMenuStripFilter.SuspendLayout();
            this.panelTransferCacheToPostgres.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 23;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanelMain.Controls.Add(this.buttonProjectTransferToPostgresFilter, 14, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonProjectTransferToCacheDBFilter, 6, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelProject, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelNumberInDatabase, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelNumberInCacheDB, 7, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelNumberInPostgresDB, 15, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelLastUpdateInDatabase, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelLastUpdateInCacheDatabase, 7, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelLastUpdateInPostgresDatabase, 15, 3);
            this.tableLayoutPanelMain.Controls.Add(this.buttonSetTransferFilter, 6, 4);
            this.tableLayoutPanelMain.Controls.Add(this.buttonViewCacheContent, 9, 4);
            this.tableLayoutPanelMain.Controls.Add(this.buttonViewPostgresContent, 17, 4);
            this.tableLayoutPanelMain.Controls.Add(this.buttonAdministratePackages, 20, 4);
            this.tableLayoutPanelMain.Controls.Add(this.listBoxPackages, 21, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonTransferDatabaseToCache, 6, 3);
            this.tableLayoutPanelMain.Controls.Add(this.buttonRemoveProject, 4, 4);
            this.tableLayoutPanelMain.Controls.Add(this.buttonPostgresEstablishProject, 14, 4);
            this.tableLayoutPanelMain.Controls.Add(this.buttonUpdateCache, 7, 4);
            this.tableLayoutPanelMain.Controls.Add(this.buttonUpdatePostgres, 15, 4);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxPostgresIncludeInTransfer, 14, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonPostgresTransferSettings, 15, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonPostgresTransferErrors, 19, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonPostgresTransferProtocol, 18, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonTransferErrors, 12, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonTransferProtocol, 11, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonIncludeInTransfer, 7, 1);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxIncludeInTransfer, 6, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonTransferPackageData, 20, 3);
            this.tableLayoutPanelMain.Controls.Add(this.buttonDatawithholding, 2, 4);
            this.tableLayoutPanelMain.Controls.Add(this.labelDatawithholding, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.buttonProjectTargets, 11, 3);
            this.tableLayoutPanelMain.Controls.Add(this.buttonHistoryInCacheDB, 11, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonHistoryInPostgres, 18, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonCreateArchive, 22, 4);
            this.tableLayoutPanelMain.Controls.Add(this.buttonGetDoi, 4, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelDoiInCacheDB, 10, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelDoiInPostgres, 18, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelDoiInPackage, 22, 3);
            this.tableLayoutPanelMain.Controls.Add(this.panelTransferCacheToPostgres, 14, 3);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxPostgresUseBulkTransfer, 17, 1);
            this.tableLayoutPanelMain.Controls.Add(this.panelPostgresOtherTargets, 14, 5);
            this.tableLayoutPanelMain.Controls.Add(this.buttonProjectDifferingName, 4, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelProjectID, 2, 2);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxProjectID, 3, 2);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 6;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(822, 88);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // buttonProjectTransferToPostgresFilter
            // 
            this.buttonProjectTransferToPostgresFilter.BackColor = System.Drawing.Color.LightSteelBlue;
            this.buttonProjectTransferToPostgresFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonProjectTransferToPostgresFilter.FlatAppearance.BorderSize = 0;
            this.buttonProjectTransferToPostgresFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonProjectTransferToPostgresFilter.Image = global::DiversityCollection.Resource.FilterClear;
            this.buttonProjectTransferToPostgresFilter.Location = new System.Drawing.Point(405, 25);
            this.buttonProjectTransferToPostgresFilter.Margin = new System.Windows.Forms.Padding(0);
            this.buttonProjectTransferToPostgresFilter.Name = "buttonProjectTransferToPostgresFilter";
            this.buttonProjectTransferToPostgresFilter.Size = new System.Drawing.Size(24, 13);
            this.buttonProjectTransferToPostgresFilter.TabIndex = 22;
            this.toolTip.SetToolTip(this.buttonProjectTransferToPostgresFilter, "Data not filtered for updates");
            this.buttonProjectTransferToPostgresFilter.UseVisualStyleBackColor = false;
            this.buttonProjectTransferToPostgresFilter.Click += new System.EventHandler(this.buttonProjectTransferToPostgresFilter_Click);
            // 
            // buttonProjectTransferToCacheDBFilter
            // 
            this.buttonProjectTransferToCacheDBFilter.BackColor = System.Drawing.Color.Thistle;
            this.buttonProjectTransferToCacheDBFilter.ContextMenuStrip = this.contextMenuStripFilter;
            this.buttonProjectTransferToCacheDBFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonProjectTransferToCacheDBFilter.FlatAppearance.BorderSize = 0;
            this.buttonProjectTransferToCacheDBFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonProjectTransferToCacheDBFilter.Image = global::DiversityCollection.Resource.FilterClear;
            this.buttonProjectTransferToCacheDBFilter.Location = new System.Drawing.Point(183, 25);
            this.buttonProjectTransferToCacheDBFilter.Margin = new System.Windows.Forms.Padding(0);
            this.buttonProjectTransferToCacheDBFilter.Name = "buttonProjectTransferToCacheDBFilter";
            this.buttonProjectTransferToCacheDBFilter.Size = new System.Drawing.Size(24, 13);
            this.buttonProjectTransferToCacheDBFilter.TabIndex = 21;
            this.toolTip.SetToolTip(this.buttonProjectTransferToCacheDBFilter, "Data not filtered for updates");
            this.buttonProjectTransferToCacheDBFilter.UseVisualStyleBackColor = false;
            this.buttonProjectTransferToCacheDBFilter.Click += new System.EventHandler(this.buttonProjectTransferToCacheDBFilter_Click);
            // 
            // contextMenuStripFilter
            // 
            this.contextMenuStripFilter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewDifferencesToolStripMenuItem});
            this.contextMenuStripFilter.Name = "contextMenuStripFilter";
            this.contextMenuStripFilter.Size = new System.Drawing.Size(161, 26);
            // 
            // viewDifferencesToolStripMenuItem
            // 
            this.viewDifferencesToolStripMenuItem.Image = global::DiversityCollection.Resource.Lupe;
            this.viewDifferencesToolStripMenuItem.Name = "viewDifferencesToolStripMenuItem";
            this.viewDifferencesToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.viewDifferencesToolStripMenuItem.Text = "View differences";
            this.viewDifferencesToolStripMenuItem.Click += new System.EventHandler(this.viewDifferencesToolStripMenuItem_Click);
            // 
            // labelProject
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.labelProject, 4);
            this.labelProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProject.Location = new System.Drawing.Point(3, 6);
            this.labelProject.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.labelProject.Name = "labelProject";
            this.labelProject.Size = new System.Drawing.Size(144, 16);
            this.labelProject.TabIndex = 0;
            this.labelProject.Text = "label1";
            this.labelProject.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelNumberInDatabase
            // 
            this.labelNumberInDatabase.AutoSize = true;
            this.labelNumberInDatabase.BackColor = System.Drawing.Color.Khaki;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelNumberInDatabase, 2);
            this.labelNumberInDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNumberInDatabase.Location = new System.Drawing.Point(3, 25);
            this.labelNumberInDatabase.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelNumberInDatabase.Name = "labelNumberInDatabase";
            this.labelNumberInDatabase.Size = new System.Drawing.Size(123, 13);
            this.labelNumberInDatabase.TabIndex = 1;
            this.labelNumberInDatabase.Text = "label1";
            // 
            // labelNumberInCacheDB
            // 
            this.labelNumberInCacheDB.AutoSize = true;
            this.labelNumberInCacheDB.BackColor = System.Drawing.Color.Thistle;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelNumberInCacheDB, 4);
            this.labelNumberInCacheDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNumberInCacheDB.Location = new System.Drawing.Point(207, 25);
            this.labelNumberInCacheDB.Margin = new System.Windows.Forms.Padding(0);
            this.labelNumberInCacheDB.Name = "labelNumberInCacheDB";
            this.labelNumberInCacheDB.Size = new System.Drawing.Size(168, 13);
            this.labelNumberInCacheDB.TabIndex = 2;
            this.labelNumberInCacheDB.Text = "label1";
            // 
            // labelNumberInPostgresDB
            // 
            this.labelNumberInPostgresDB.AutoSize = true;
            this.labelNumberInPostgresDB.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelNumberInPostgresDB, 3);
            this.labelNumberInPostgresDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNumberInPostgresDB.Location = new System.Drawing.Point(429, 25);
            this.labelNumberInPostgresDB.Margin = new System.Windows.Forms.Padding(0);
            this.labelNumberInPostgresDB.Name = "labelNumberInPostgresDB";
            this.labelNumberInPostgresDB.Size = new System.Drawing.Size(158, 13);
            this.labelNumberInPostgresDB.TabIndex = 3;
            this.labelNumberInPostgresDB.Text = "label1";
            // 
            // labelLastUpdateInDatabase
            // 
            this.labelLastUpdateInDatabase.BackColor = System.Drawing.Color.Khaki;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelLastUpdateInDatabase, 4);
            this.labelLastUpdateInDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLastUpdateInDatabase.Location = new System.Drawing.Point(3, 38);
            this.labelLastUpdateInDatabase.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelLastUpdateInDatabase.Name = "labelLastUpdateInDatabase";
            this.labelLastUpdateInDatabase.Size = new System.Drawing.Size(147, 25);
            this.labelLastUpdateInDatabase.TabIndex = 4;
            this.labelLastUpdateInDatabase.Text = "label1";
            // 
            // labelLastUpdateInCacheDatabase
            // 
            this.labelLastUpdateInCacheDatabase.AutoSize = true;
            this.labelLastUpdateInCacheDatabase.BackColor = System.Drawing.Color.Thistle;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelLastUpdateInCacheDatabase, 3);
            this.labelLastUpdateInCacheDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLastUpdateInCacheDatabase.Location = new System.Drawing.Point(207, 38);
            this.labelLastUpdateInCacheDatabase.Margin = new System.Windows.Forms.Padding(0);
            this.labelLastUpdateInCacheDatabase.Name = "labelLastUpdateInCacheDatabase";
            this.labelLastUpdateInCacheDatabase.Size = new System.Drawing.Size(154, 25);
            this.labelLastUpdateInCacheDatabase.TabIndex = 5;
            this.labelLastUpdateInCacheDatabase.Text = "label1";
            // 
            // labelLastUpdateInPostgresDatabase
            // 
            this.labelLastUpdateInPostgresDatabase.AutoSize = true;
            this.labelLastUpdateInPostgresDatabase.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelLastUpdateInPostgresDatabase, 3);
            this.labelLastUpdateInPostgresDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLastUpdateInPostgresDatabase.Location = new System.Drawing.Point(429, 38);
            this.labelLastUpdateInPostgresDatabase.Margin = new System.Windows.Forms.Padding(0);
            this.labelLastUpdateInPostgresDatabase.Name = "labelLastUpdateInPostgresDatabase";
            this.labelLastUpdateInPostgresDatabase.Size = new System.Drawing.Size(158, 25);
            this.labelLastUpdateInPostgresDatabase.TabIndex = 6;
            this.labelLastUpdateInPostgresDatabase.Text = "label1";
            // 
            // buttonSetTransferFilter
            // 
            this.buttonSetTransferFilter.BackColor = System.Drawing.Color.Thistle;
            this.buttonSetTransferFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSetTransferFilter.FlatAppearance.BorderSize = 0;
            this.buttonSetTransferFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSetTransferFilter.Image = global::DiversityCollection.Resource.CacheDBTransferFilter;
            this.buttonSetTransferFilter.Location = new System.Drawing.Point(183, 63);
            this.buttonSetTransferFilter.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSetTransferFilter.Name = "buttonSetTransferFilter";
            this.buttonSetTransferFilter.Size = new System.Drawing.Size(24, 24);
            this.buttonSetTransferFilter.TabIndex = 9;
            this.toolTip.SetToolTip(this.buttonSetTransferFilter, "Administrate filters for data transfer");
            this.buttonSetTransferFilter.UseVisualStyleBackColor = false;
            this.buttonSetTransferFilter.Click += new System.EventHandler(this.buttonSetTransferFilter_Click);
            // 
            // buttonViewCacheContent
            // 
            this.buttonViewCacheContent.BackColor = System.Drawing.Color.Thistle;
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonViewCacheContent, 4);
            this.buttonViewCacheContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonViewCacheContent.FlatAppearance.BorderSize = 0;
            this.buttonViewCacheContent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonViewCacheContent.Image = global::DiversityCollection.Resource.Lupe;
            this.buttonViewCacheContent.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonViewCacheContent.Location = new System.Drawing.Point(296, 63);
            this.buttonViewCacheContent.Margin = new System.Windows.Forms.Padding(0);
            this.buttonViewCacheContent.Name = "buttonViewCacheContent";
            this.buttonViewCacheContent.Size = new System.Drawing.Size(103, 24);
            this.buttonViewCacheContent.TabIndex = 10;
            this.buttonViewCacheContent.Text = "View content";
            this.buttonViewCacheContent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonViewCacheContent, "View data of the project");
            this.buttonViewCacheContent.UseVisualStyleBackColor = false;
            this.buttonViewCacheContent.Click += new System.EventHandler(this.buttonViewCacheContent_Click);
            // 
            // buttonViewPostgresContent
            // 
            this.buttonViewPostgresContent.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonViewPostgresContent, 3);
            this.buttonViewPostgresContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonViewPostgresContent.FlatAppearance.BorderSize = 0;
            this.buttonViewPostgresContent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonViewPostgresContent.Image = global::DiversityCollection.Resource.Lupe;
            this.buttonViewPostgresContent.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonViewPostgresContent.Location = new System.Drawing.Point(522, 63);
            this.buttonViewPostgresContent.Margin = new System.Windows.Forms.Padding(0);
            this.buttonViewPostgresContent.Name = "buttonViewPostgresContent";
            this.buttonViewPostgresContent.Size = new System.Drawing.Size(89, 24);
            this.buttonViewPostgresContent.TabIndex = 11;
            this.buttonViewPostgresContent.Text = "View content";
            this.buttonViewPostgresContent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonViewPostgresContent, "View project data");
            this.buttonViewPostgresContent.UseVisualStyleBackColor = false;
            this.buttonViewPostgresContent.Click += new System.EventHandler(this.buttonViewPostgresContent_Click);
            // 
            // buttonAdministratePackages
            // 
            this.buttonAdministratePackages.BackColor = System.Drawing.Color.PaleTurquoise;
            this.buttonAdministratePackages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAdministratePackages.FlatAppearance.BorderSize = 0;
            this.buttonAdministratePackages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAdministratePackages.Image = global::DiversityCollection.Resource.Package;
            this.buttonAdministratePackages.Location = new System.Drawing.Point(611, 63);
            this.buttonAdministratePackages.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAdministratePackages.Name = "buttonAdministratePackages";
            this.buttonAdministratePackages.Size = new System.Drawing.Size(24, 24);
            this.buttonAdministratePackages.TabIndex = 12;
            this.toolTip.SetToolTip(this.buttonAdministratePackages, "Administrate packages");
            this.buttonAdministratePackages.UseVisualStyleBackColor = false;
            this.buttonAdministratePackages.Click += new System.EventHandler(this.buttonAdministratePackages_Click);
            // 
            // listBoxPackages
            // 
            this.listBoxPackages.BackColor = System.Drawing.Color.PaleTurquoise;
            this.listBoxPackages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxPackages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxPackages.FormattingEnabled = true;
            this.listBoxPackages.IntegralHeight = false;
            this.listBoxPackages.Location = new System.Drawing.Point(635, 6);
            this.listBoxPackages.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxPackages.Name = "listBoxPackages";
            this.tableLayoutPanelMain.SetRowSpan(this.listBoxPackages, 4);
            this.listBoxPackages.Size = new System.Drawing.Size(135, 81);
            this.listBoxPackages.TabIndex = 13;
            this.listBoxPackages.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listBoxPackages_MouseClick);
            // 
            // buttonTransferDatabaseToCache
            // 
            this.buttonTransferDatabaseToCache.BackColor = System.Drawing.Color.Thistle;
            this.buttonTransferDatabaseToCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTransferDatabaseToCache.Enabled = false;
            this.buttonTransferDatabaseToCache.FlatAppearance.BorderSize = 0;
            this.buttonTransferDatabaseToCache.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransferDatabaseToCache.Image = global::DiversityCollection.Resource.ArrowNext1;
            this.buttonTransferDatabaseToCache.Location = new System.Drawing.Point(183, 38);
            this.buttonTransferDatabaseToCache.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTransferDatabaseToCache.Name = "buttonTransferDatabaseToCache";
            this.buttonTransferDatabaseToCache.Size = new System.Drawing.Size(24, 25);
            this.buttonTransferDatabaseToCache.TabIndex = 7;
            this.buttonTransferDatabaseToCache.UseVisualStyleBackColor = false;
            this.buttonTransferDatabaseToCache.Click += new System.EventHandler(this.buttonTransferDatabaseToCache_Click);
            // 
            // buttonRemoveProject
            // 
            this.buttonRemoveProject.BackColor = System.Drawing.Color.Thistle;
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonRemoveProject, 2);
            this.buttonRemoveProject.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonRemoveProject.FlatAppearance.BorderSize = 0;
            this.buttonRemoveProject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRemoveProject.Image = global::DiversityCollection.Resource.Delete;
            this.buttonRemoveProject.Location = new System.Drawing.Point(160, 63);
            this.buttonRemoveProject.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.buttonRemoveProject.Name = "buttonRemoveProject";
            this.buttonRemoveProject.Size = new System.Drawing.Size(23, 24);
            this.buttonRemoveProject.TabIndex = 14;
            this.toolTip.SetToolTip(this.buttonRemoveProject, "Remove project from cache database");
            this.buttonRemoveProject.UseVisualStyleBackColor = false;
            this.buttonRemoveProject.Click += new System.EventHandler(this.buttonRemoveProject_Click);
            // 
            // buttonPostgresEstablishProject
            // 
            this.buttonPostgresEstablishProject.BackColor = System.Drawing.Color.LightSteelBlue;
            this.buttonPostgresEstablishProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPostgresEstablishProject.FlatAppearance.BorderSize = 0;
            this.buttonPostgresEstablishProject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPostgresEstablishProject.Image = global::DiversityCollection.Resource.EstablishProject;
            this.buttonPostgresEstablishProject.Location = new System.Drawing.Point(405, 63);
            this.buttonPostgresEstablishProject.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPostgresEstablishProject.Name = "buttonPostgresEstablishProject";
            this.buttonPostgresEstablishProject.Size = new System.Drawing.Size(24, 24);
            this.buttonPostgresEstablishProject.TabIndex = 15;
            this.toolTip.SetToolTip(this.buttonPostgresEstablishProject, "Establish project in postgres database");
            this.buttonPostgresEstablishProject.UseVisualStyleBackColor = false;
            this.buttonPostgresEstablishProject.Click += new System.EventHandler(this.buttonPostgresEstablishProject_Click);
            // 
            // buttonUpdateCache
            // 
            this.buttonUpdateCache.BackColor = System.Drawing.Color.Thistle;
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonUpdateCache, 2);
            this.buttonUpdateCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonUpdateCache.FlatAppearance.BorderSize = 0;
            this.buttonUpdateCache.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUpdateCache.ForeColor = System.Drawing.Color.Red;
            this.buttonUpdateCache.Image = global::DiversityCollection.Resource.Update;
            this.buttonUpdateCache.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonUpdateCache.Location = new System.Drawing.Point(207, 63);
            this.buttonUpdateCache.Margin = new System.Windows.Forms.Padding(0);
            this.buttonUpdateCache.Name = "buttonUpdateCache";
            this.buttonUpdateCache.Size = new System.Drawing.Size(89, 24);
            this.buttonUpdateCache.TabIndex = 16;
            this.buttonUpdateCache.Text = "Update";
            this.buttonUpdateCache.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonUpdateCache, "Update project to current version");
            this.buttonUpdateCache.UseVisualStyleBackColor = false;
            this.buttonUpdateCache.Click += new System.EventHandler(this.buttonUpdateCache_Click);
            // 
            // buttonUpdatePostgres
            // 
            this.buttonUpdatePostgres.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonUpdatePostgres, 2);
            this.buttonUpdatePostgres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonUpdatePostgres.FlatAppearance.BorderSize = 0;
            this.buttonUpdatePostgres.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUpdatePostgres.ForeColor = System.Drawing.Color.Red;
            this.buttonUpdatePostgres.Image = global::DiversityCollection.Resource.Update;
            this.buttonUpdatePostgres.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonUpdatePostgres.Location = new System.Drawing.Point(429, 63);
            this.buttonUpdatePostgres.Margin = new System.Windows.Forms.Padding(0);
            this.buttonUpdatePostgres.Name = "buttonUpdatePostgres";
            this.buttonUpdatePostgres.Size = new System.Drawing.Size(93, 24);
            this.buttonUpdatePostgres.TabIndex = 17;
            this.buttonUpdatePostgres.Text = "Update";
            this.buttonUpdatePostgres.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonUpdatePostgres, "Update project to current version");
            this.buttonUpdatePostgres.UseVisualStyleBackColor = false;
            this.buttonUpdatePostgres.Click += new System.EventHandler(this.buttonUpdatePostgres_Click);
            // 
            // checkBoxPostgresIncludeInTransfer
            // 
            this.checkBoxPostgresIncludeInTransfer.AutoSize = true;
            this.checkBoxPostgresIncludeInTransfer.BackColor = System.Drawing.Color.LightSteelBlue;
            this.checkBoxPostgresIncludeInTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxPostgresIncludeInTransfer.Location = new System.Drawing.Point(405, 6);
            this.checkBoxPostgresIncludeInTransfer.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxPostgresIncludeInTransfer.Name = "checkBoxPostgresIncludeInTransfer";
            this.checkBoxPostgresIncludeInTransfer.Padding = new System.Windows.Forms.Padding(6, 1, 0, 0);
            this.checkBoxPostgresIncludeInTransfer.Size = new System.Drawing.Size(24, 19);
            this.checkBoxPostgresIncludeInTransfer.TabIndex = 23;
            this.toolTip.SetToolTip(this.checkBoxPostgresIncludeInTransfer, "If the project should be included in the scheduled transfer");
            this.checkBoxPostgresIncludeInTransfer.UseVisualStyleBackColor = false;
            this.checkBoxPostgresIncludeInTransfer.Click += new System.EventHandler(this.checkBoxPostgresIncludeInTransfer_Click);
            // 
            // buttonPostgresTransferSettings
            // 
            this.buttonPostgresTransferSettings.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonPostgresTransferSettings, 2);
            this.buttonPostgresTransferSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPostgresTransferSettings.FlatAppearance.BorderSize = 0;
            this.buttonPostgresTransferSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPostgresTransferSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPostgresTransferSettings.Image = global::DiversityCollection.Resource.Time;
            this.buttonPostgresTransferSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonPostgresTransferSettings.Location = new System.Drawing.Point(429, 6);
            this.buttonPostgresTransferSettings.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPostgresTransferSettings.Name = "buttonPostgresTransferSettings";
            this.buttonPostgresTransferSettings.Size = new System.Drawing.Size(93, 19);
            this.buttonPostgresTransferSettings.TabIndex = 24;
            this.buttonPostgresTransferSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonPostgresTransferSettings, "Set the days and time for the scheduled transfer");
            this.buttonPostgresTransferSettings.UseVisualStyleBackColor = false;
            this.buttonPostgresTransferSettings.Click += new System.EventHandler(this.buttonPostgresTransferSettings_Click);
            // 
            // buttonPostgresTransferErrors
            // 
            this.buttonPostgresTransferErrors.BackColor = System.Drawing.Color.LightSteelBlue;
            this.buttonPostgresTransferErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPostgresTransferErrors.FlatAppearance.BorderSize = 0;
            this.buttonPostgresTransferErrors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPostgresTransferErrors.Image = global::DiversityCollection.Resource.ListRed;
            this.buttonPostgresTransferErrors.Location = new System.Drawing.Point(599, 6);
            this.buttonPostgresTransferErrors.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPostgresTransferErrors.Name = "buttonPostgresTransferErrors";
            this.buttonPostgresTransferErrors.Size = new System.Drawing.Size(12, 19);
            this.buttonPostgresTransferErrors.TabIndex = 28;
            this.toolTip.SetToolTip(this.buttonPostgresTransferErrors, "Errors that occurred during the last transfer");
            this.buttonPostgresTransferErrors.UseVisualStyleBackColor = false;
            this.buttonPostgresTransferErrors.Click += new System.EventHandler(this.buttonPostgresTransferErrors_Click);
            // 
            // buttonPostgresTransferProtocol
            // 
            this.buttonPostgresTransferProtocol.BackColor = System.Drawing.Color.LightSteelBlue;
            this.buttonPostgresTransferProtocol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPostgresTransferProtocol.FlatAppearance.BorderSize = 0;
            this.buttonPostgresTransferProtocol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPostgresTransferProtocol.Image = global::DiversityCollection.Resource.List;
            this.buttonPostgresTransferProtocol.Location = new System.Drawing.Point(587, 6);
            this.buttonPostgresTransferProtocol.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPostgresTransferProtocol.Name = "buttonPostgresTransferProtocol";
            this.buttonPostgresTransferProtocol.Size = new System.Drawing.Size(12, 19);
            this.buttonPostgresTransferProtocol.TabIndex = 29;
            this.toolTip.SetToolTip(this.buttonPostgresTransferProtocol, "Open the protocol of the last transfer");
            this.buttonPostgresTransferProtocol.UseVisualStyleBackColor = false;
            this.buttonPostgresTransferProtocol.Click += new System.EventHandler(this.buttonPostgresTransferProtocol_Click);
            // 
            // buttonTransferErrors
            // 
            this.buttonTransferErrors.BackColor = System.Drawing.Color.Thistle;
            this.buttonTransferErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTransferErrors.FlatAppearance.BorderSize = 0;
            this.buttonTransferErrors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransferErrors.Image = global::DiversityCollection.Resource.ListRed;
            this.buttonTransferErrors.Location = new System.Drawing.Point(387, 6);
            this.buttonTransferErrors.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTransferErrors.Name = "buttonTransferErrors";
            this.buttonTransferErrors.Size = new System.Drawing.Size(12, 19);
            this.buttonTransferErrors.TabIndex = 27;
            this.toolTip.SetToolTip(this.buttonTransferErrors, "Errors that occurred during the last transfer");
            this.buttonTransferErrors.UseVisualStyleBackColor = false;
            this.buttonTransferErrors.Click += new System.EventHandler(this.buttonTransferErrors_Click);
            // 
            // buttonTransferProtocol
            // 
            this.buttonTransferProtocol.BackColor = System.Drawing.Color.Thistle;
            this.buttonTransferProtocol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTransferProtocol.FlatAppearance.BorderSize = 0;
            this.buttonTransferProtocol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransferProtocol.Image = global::DiversityCollection.Resource.List;
            this.buttonTransferProtocol.Location = new System.Drawing.Point(375, 6);
            this.buttonTransferProtocol.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTransferProtocol.Name = "buttonTransferProtocol";
            this.buttonTransferProtocol.Size = new System.Drawing.Size(12, 19);
            this.buttonTransferProtocol.TabIndex = 20;
            this.toolTip.SetToolTip(this.buttonTransferProtocol, "Open the protocol of the last transfer ");
            this.buttonTransferProtocol.UseVisualStyleBackColor = false;
            this.buttonTransferProtocol.Click += new System.EventHandler(this.buttonTransferProtocol_Click);
            // 
            // buttonIncludeInTransfer
            // 
            this.buttonIncludeInTransfer.BackColor = System.Drawing.Color.Thistle;
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonIncludeInTransfer, 4);
            this.buttonIncludeInTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonIncludeInTransfer.FlatAppearance.BorderSize = 0;
            this.buttonIncludeInTransfer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonIncludeInTransfer.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonIncludeInTransfer.Image = global::DiversityCollection.Resource.Time;
            this.buttonIncludeInTransfer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonIncludeInTransfer.Location = new System.Drawing.Point(207, 6);
            this.buttonIncludeInTransfer.Margin = new System.Windows.Forms.Padding(0);
            this.buttonIncludeInTransfer.Name = "buttonIncludeInTransfer";
            this.buttonIncludeInTransfer.Size = new System.Drawing.Size(168, 19);
            this.buttonIncludeInTransfer.TabIndex = 26;
            this.buttonIncludeInTransfer.Text = "        ";
            this.buttonIncludeInTransfer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonIncludeInTransfer, "Set the days and time for the scheduled transfer");
            this.buttonIncludeInTransfer.UseVisualStyleBackColor = false;
            this.buttonIncludeInTransfer.Click += new System.EventHandler(this.buttonIncludeInTransfer_Click);
            // 
            // checkBoxIncludeInTransfer
            // 
            this.checkBoxIncludeInTransfer.AutoSize = true;
            this.checkBoxIncludeInTransfer.BackColor = System.Drawing.Color.Thistle;
            this.checkBoxIncludeInTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxIncludeInTransfer.Location = new System.Drawing.Point(183, 6);
            this.checkBoxIncludeInTransfer.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxIncludeInTransfer.Name = "checkBoxIncludeInTransfer";
            this.checkBoxIncludeInTransfer.Padding = new System.Windows.Forms.Padding(6, 1, 0, 0);
            this.checkBoxIncludeInTransfer.Size = new System.Drawing.Size(24, 19);
            this.checkBoxIncludeInTransfer.TabIndex = 18;
            this.toolTip.SetToolTip(this.checkBoxIncludeInTransfer, "select if the project should be included in the schedule based transfer of the da" +
        "ta");
            this.checkBoxIncludeInTransfer.UseVisualStyleBackColor = false;
            this.checkBoxIncludeInTransfer.Click += new System.EventHandler(this.checkBoxIncludeInTransfer_Click);
            // 
            // buttonTransferPackageData
            // 
            this.buttonTransferPackageData.BackColor = System.Drawing.Color.PaleTurquoise;
            this.buttonTransferPackageData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTransferPackageData.FlatAppearance.BorderSize = 0;
            this.buttonTransferPackageData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransferPackageData.Image = global::DiversityCollection.Resource.ArrowNext1;
            this.buttonTransferPackageData.Location = new System.Drawing.Point(611, 38);
            this.buttonTransferPackageData.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTransferPackageData.Name = "buttonTransferPackageData";
            this.buttonTransferPackageData.Size = new System.Drawing.Size(24, 25);
            this.buttonTransferPackageData.TabIndex = 31;
            this.toolTip.SetToolTip(this.buttonTransferPackageData, "Transfer data for all packages");
            this.buttonTransferPackageData.UseVisualStyleBackColor = false;
            this.buttonTransferPackageData.Visible = false;
            this.buttonTransferPackageData.Click += new System.EventHandler(this.buttonTransferPackageData_Click);
            // 
            // buttonDatawithholding
            // 
            this.buttonDatawithholding.BackColor = System.Drawing.Color.Khaki;
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonDatawithholding, 2);
            this.buttonDatawithholding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDatawithholding.FlatAppearance.BorderSize = 0;
            this.buttonDatawithholding.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDatawithholding.Image = global::DiversityCollection.Resource.Stop3;
            this.buttonDatawithholding.Location = new System.Drawing.Point(126, 63);
            this.buttonDatawithholding.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDatawithholding.Name = "buttonDatawithholding";
            this.buttonDatawithholding.Size = new System.Drawing.Size(24, 24);
            this.buttonDatawithholding.TabIndex = 32;
            this.toolTip.SetToolTip(this.buttonDatawithholding, "Edit datawithholding reasons for the project");
            this.buttonDatawithholding.UseVisualStyleBackColor = false;
            this.buttonDatawithholding.Click += new System.EventHandler(this.buttonDatawithholding_Click);
            // 
            // labelDatawithholding
            // 
            this.labelDatawithholding.AutoSize = true;
            this.labelDatawithholding.BackColor = System.Drawing.Color.Khaki;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelDatawithholding, 2);
            this.labelDatawithholding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDatawithholding.Location = new System.Drawing.Point(3, 63);
            this.labelDatawithholding.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelDatawithholding.Name = "labelDatawithholding";
            this.labelDatawithholding.Size = new System.Drawing.Size(123, 24);
            this.labelDatawithholding.TabIndex = 33;
            this.labelDatawithholding.Text = "label1";
            this.labelDatawithholding.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonProjectTargets
            // 
            this.buttonProjectTargets.BackColor = System.Drawing.Color.Thistle;
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonProjectTargets, 2);
            this.buttonProjectTargets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonProjectTargets.FlatAppearance.BorderSize = 0;
            this.buttonProjectTargets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonProjectTargets.Image = global::DiversityCollection.Resource.PostgresTarget;
            this.buttonProjectTargets.Location = new System.Drawing.Point(375, 38);
            this.buttonProjectTargets.Margin = new System.Windows.Forms.Padding(0);
            this.buttonProjectTargets.Name = "buttonProjectTargets";
            this.buttonProjectTargets.Size = new System.Drawing.Size(24, 25);
            this.buttonProjectTargets.TabIndex = 34;
            this.toolTip.SetToolTip(this.buttonProjectTargets, "Show postgres target databases of this project");
            this.buttonProjectTargets.UseVisualStyleBackColor = false;
            this.buttonProjectTargets.Click += new System.EventHandler(this.buttonProjectTargets_Click);
            // 
            // buttonHistoryInCacheDB
            // 
            this.buttonHistoryInCacheDB.BackColor = System.Drawing.Color.Thistle;
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonHistoryInCacheDB, 2);
            this.buttonHistoryInCacheDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonHistoryInCacheDB.FlatAppearance.BorderSize = 0;
            this.buttonHistoryInCacheDB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHistoryInCacheDB.Image = global::DiversityCollection.Resource.History;
            this.buttonHistoryInCacheDB.Location = new System.Drawing.Point(375, 25);
            this.buttonHistoryInCacheDB.Margin = new System.Windows.Forms.Padding(0);
            this.buttonHistoryInCacheDB.Name = "buttonHistoryInCacheDB";
            this.buttonHistoryInCacheDB.Size = new System.Drawing.Size(24, 13);
            this.buttonHistoryInCacheDB.TabIndex = 35;
            this.buttonHistoryInCacheDB.UseVisualStyleBackColor = false;
            this.buttonHistoryInCacheDB.Click += new System.EventHandler(this.buttonHistoryInCacheDB_Click);
            // 
            // buttonHistoryInPostgres
            // 
            this.buttonHistoryInPostgres.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonHistoryInPostgres, 2);
            this.buttonHistoryInPostgres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonHistoryInPostgres.FlatAppearance.BorderSize = 0;
            this.buttonHistoryInPostgres.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHistoryInPostgres.Image = global::DiversityCollection.Resource.History;
            this.buttonHistoryInPostgres.Location = new System.Drawing.Point(587, 25);
            this.buttonHistoryInPostgres.Margin = new System.Windows.Forms.Padding(0);
            this.buttonHistoryInPostgres.Name = "buttonHistoryInPostgres";
            this.buttonHistoryInPostgres.Size = new System.Drawing.Size(24, 13);
            this.buttonHistoryInPostgres.TabIndex = 36;
            this.buttonHistoryInPostgres.UseVisualStyleBackColor = false;
            this.buttonHistoryInPostgres.Click += new System.EventHandler(this.buttonHistoryInPostgres_Click);
            // 
            // buttonCreateArchive
            // 
            this.buttonCreateArchive.BackColor = System.Drawing.Color.Silver;
            this.buttonCreateArchive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCreateArchive.FlatAppearance.BorderSize = 0;
            this.buttonCreateArchive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCreateArchive.Image = global::DiversityCollection.Resource.OAIS_16;
            this.buttonCreateArchive.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonCreateArchive.Location = new System.Drawing.Point(770, 63);
            this.buttonCreateArchive.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.buttonCreateArchive.Name = "buttonCreateArchive";
            this.buttonCreateArchive.Size = new System.Drawing.Size(49, 24);
            this.buttonCreateArchive.TabIndex = 37;
            this.toolTip.SetToolTip(this.buttonCreateArchive, "Create Archive");
            this.buttonCreateArchive.UseVisualStyleBackColor = false;
            this.buttonCreateArchive.Click += new System.EventHandler(this.buttonCreateArchive_Click);
            // 
            // buttonGetDoi
            // 
            this.buttonGetDoi.BackColor = System.Drawing.Color.Khaki;
            this.buttonGetDoi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonGetDoi.FlatAppearance.BorderSize = 0;
            this.buttonGetDoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGetDoi.Image = global::DiversityCollection.Resource.DOIGrey;
            this.buttonGetDoi.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonGetDoi.Location = new System.Drawing.Point(150, 38);
            this.buttonGetDoi.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.buttonGetDoi.Name = "buttonGetDoi";
            this.buttonGetDoi.Size = new System.Drawing.Size(24, 25);
            this.buttonGetDoi.TabIndex = 39;
            this.toolTip.SetToolTip(this.buttonGetDoi, "No DOI for the archive. Click here to retrieve a DOI together with the transfer");
            this.buttonGetDoi.UseVisualStyleBackColor = false;
            this.buttonGetDoi.Click += new System.EventHandler(this.buttonGetDoi_Click);
            // 
            // labelDoiInCacheDB
            // 
            this.labelDoiInCacheDB.AutoSize = true;
            this.labelDoiInCacheDB.BackColor = System.Drawing.Color.Thistle;
            this.labelDoiInCacheDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDoiInCacheDB.Image = global::DiversityCollection.Resource.DOI;
            this.labelDoiInCacheDB.Location = new System.Drawing.Point(361, 38);
            this.labelDoiInCacheDB.Margin = new System.Windows.Forms.Padding(0);
            this.labelDoiInCacheDB.Name = "labelDoiInCacheDB";
            this.labelDoiInCacheDB.Size = new System.Drawing.Size(14, 25);
            this.labelDoiInCacheDB.TabIndex = 40;
            // 
            // labelDoiInPostgres
            // 
            this.labelDoiInPostgres.AutoSize = true;
            this.labelDoiInPostgres.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelDoiInPostgres, 2);
            this.labelDoiInPostgres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDoiInPostgres.Image = global::DiversityCollection.Resource.DOI;
            this.labelDoiInPostgres.Location = new System.Drawing.Point(587, 38);
            this.labelDoiInPostgres.Margin = new System.Windows.Forms.Padding(0);
            this.labelDoiInPostgres.Name = "labelDoiInPostgres";
            this.labelDoiInPostgres.Size = new System.Drawing.Size(24, 25);
            this.labelDoiInPostgres.TabIndex = 41;
            // 
            // labelDoiInPackage
            // 
            this.labelDoiInPackage.AutoSize = true;
            this.labelDoiInPackage.BackColor = System.Drawing.Color.PaleTurquoise;
            this.labelDoiInPackage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDoiInPackage.Image = global::DiversityCollection.Resource.DOI;
            this.labelDoiInPackage.Location = new System.Drawing.Point(770, 38);
            this.labelDoiInPackage.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelDoiInPackage.Name = "labelDoiInPackage";
            this.labelDoiInPackage.Size = new System.Drawing.Size(49, 25);
            this.labelDoiInPackage.TabIndex = 42;
            // 
            // panelTransferCacheToPostgres
            // 
            this.panelTransferCacheToPostgres.Controls.Add(this.buttonTransferCacheToPostgresFile);
            this.panelTransferCacheToPostgres.Controls.Add(this.buttonTransferCacheToPostgres);
            this.panelTransferCacheToPostgres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTransferCacheToPostgres.Location = new System.Drawing.Point(405, 38);
            this.panelTransferCacheToPostgres.Margin = new System.Windows.Forms.Padding(0);
            this.panelTransferCacheToPostgres.Name = "panelTransferCacheToPostgres";
            this.panelTransferCacheToPostgres.Size = new System.Drawing.Size(24, 25);
            this.panelTransferCacheToPostgres.TabIndex = 43;
            // 
            // buttonTransferCacheToPostgresFile
            // 
            this.buttonTransferCacheToPostgresFile.BackColor = System.Drawing.Color.LightSteelBlue;
            this.buttonTransferCacheToPostgresFile.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonTransferCacheToPostgresFile.FlatAppearance.BorderSize = 0;
            this.buttonTransferCacheToPostgresFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransferCacheToPostgresFile.Image = global::DiversityCollection.Resource.TransferToFile;
            this.buttonTransferCacheToPostgresFile.Location = new System.Drawing.Point(14, 0);
            this.buttonTransferCacheToPostgresFile.Name = "buttonTransferCacheToPostgresFile";
            this.buttonTransferCacheToPostgresFile.Size = new System.Drawing.Size(10, 25);
            this.buttonTransferCacheToPostgresFile.TabIndex = 9;
            this.buttonTransferCacheToPostgresFile.UseVisualStyleBackColor = false;
            this.buttonTransferCacheToPostgresFile.Visible = false;
            this.buttonTransferCacheToPostgresFile.Click += new System.EventHandler(this.buttonTransferCacheToPostgresFile_Click);
            // 
            // buttonTransferCacheToPostgres
            // 
            this.buttonTransferCacheToPostgres.BackColor = System.Drawing.Color.LightSteelBlue;
            this.buttonTransferCacheToPostgres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTransferCacheToPostgres.Enabled = false;
            this.buttonTransferCacheToPostgres.FlatAppearance.BorderSize = 0;
            this.buttonTransferCacheToPostgres.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransferCacheToPostgres.Image = global::DiversityCollection.Resource.ArrowNext1;
            this.buttonTransferCacheToPostgres.Location = new System.Drawing.Point(0, 0);
            this.buttonTransferCacheToPostgres.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTransferCacheToPostgres.Name = "buttonTransferCacheToPostgres";
            this.buttonTransferCacheToPostgres.Size = new System.Drawing.Size(24, 25);
            this.buttonTransferCacheToPostgres.TabIndex = 8;
            this.toolTip.SetToolTip(this.buttonTransferCacheToPostgres, "Transfer project data to postgres database");
            this.buttonTransferCacheToPostgres.UseVisualStyleBackColor = false;
            this.buttonTransferCacheToPostgres.Click += new System.EventHandler(this.buttonTransferCacheToPostgres_Click);
            // 
            // checkBoxPostgresUseBulkTransfer
            // 
            this.checkBoxPostgresUseBulkTransfer.AutoSize = true;
            this.checkBoxPostgresUseBulkTransfer.BackColor = System.Drawing.Color.LightSteelBlue;
            this.checkBoxPostgresUseBulkTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxPostgresUseBulkTransfer.Enabled = false;
            this.checkBoxPostgresUseBulkTransfer.Image = global::DiversityCollection.Resource.ToFile;
            this.checkBoxPostgresUseBulkTransfer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkBoxPostgresUseBulkTransfer.Location = new System.Drawing.Point(522, 6);
            this.checkBoxPostgresUseBulkTransfer.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxPostgresUseBulkTransfer.Name = "checkBoxPostgresUseBulkTransfer";
            this.checkBoxPostgresUseBulkTransfer.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxPostgresUseBulkTransfer.Size = new System.Drawing.Size(65, 19);
            this.checkBoxPostgresUseBulkTransfer.TabIndex = 44;
            this.checkBoxPostgresUseBulkTransfer.UseVisualStyleBackColor = false;
            this.checkBoxPostgresUseBulkTransfer.Click += new System.EventHandler(this.checkBoxPostgresUseBulkTransfer_Click);
            // 
            // panelPostgresOtherTargets
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.panelPostgresOtherTargets, 9);
            this.panelPostgresOtherTargets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPostgresOtherTargets.Location = new System.Drawing.Point(405, 87);
            this.panelPostgresOtherTargets.Margin = new System.Windows.Forms.Padding(0);
            this.panelPostgresOtherTargets.Name = "panelPostgresOtherTargets";
            this.panelPostgresOtherTargets.Size = new System.Drawing.Size(417, 1);
            this.panelPostgresOtherTargets.TabIndex = 45;
            // 
            // buttonProjectDifferingName
            // 
            this.buttonProjectDifferingName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonProjectDifferingName.FlatAppearance.BorderSize = 0;
            this.buttonProjectDifferingName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonProjectDifferingName.Image = global::DiversityCollection.Resource.Synonyms;
            this.buttonProjectDifferingName.Location = new System.Drawing.Point(153, 9);
            this.buttonProjectDifferingName.Name = "buttonProjectDifferingName";
            this.buttonProjectDifferingName.Size = new System.Drawing.Size(21, 13);
            this.buttonProjectDifferingName.TabIndex = 46;
            this.buttonProjectDifferingName.UseVisualStyleBackColor = true;
            this.buttonProjectDifferingName.Visible = false;
            this.buttonProjectDifferingName.Click += new System.EventHandler(this.buttonProjectDifferingName_Click);
            // 
            // labelProjectID
            // 
            this.labelProjectID.BackColor = System.Drawing.Color.Khaki;
            this.labelProjectID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProjectID.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProjectID.ForeColor = System.Drawing.Color.Gray;
            this.labelProjectID.Location = new System.Drawing.Point(126, 25);
            this.labelProjectID.Margin = new System.Windows.Forms.Padding(0);
            this.labelProjectID.Name = "labelProjectID";
            this.labelProjectID.Size = new System.Drawing.Size(12, 13);
            this.labelProjectID.TabIndex = 47;
            this.labelProjectID.Text = "ID";
            this.labelProjectID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxProjectID
            // 
            this.textBoxProjectID.BackColor = System.Drawing.Color.Khaki;
            this.textBoxProjectID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxProjectID, 2);
            this.textBoxProjectID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProjectID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxProjectID.ForeColor = System.Drawing.Color.Gray;
            this.textBoxProjectID.Location = new System.Drawing.Point(138, 25);
            this.textBoxProjectID.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.textBoxProjectID.Name = "textBoxProjectID";
            this.textBoxProjectID.ReadOnly = true;
            this.textBoxProjectID.Size = new System.Drawing.Size(36, 13);
            this.textBoxProjectID.TabIndex = 48;
            this.textBoxProjectID.Text = "32132";
            // 
            // imageListButtonIcons
            // 
            this.imageListButtonIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListButtonIcons.ImageStream")));
            this.imageListButtonIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListButtonIcons.Images.SetKeyName(0, "EstablishProject.ico");
            this.imageListButtonIcons.Images.SetKeyName(1, "Delete.ico");
            // 
            // imageListTransferSteps
            // 
            this.imageListTransferSteps.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTransferSteps.ImageStream")));
            this.imageListTransferSteps.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTransferSteps.Images.SetKeyName(0, "info.ico");
            this.imageListTransferSteps.Images.SetKeyName(1, "Event.ico");
            this.imageListTransferSteps.Images.SetKeyName(2, "Localisation.ico");
            this.imageListTransferSteps.Images.SetKeyName(3, "Habitat.ico");
            this.imageListTransferSteps.Images.SetKeyName(4, "CollectionSpecimen.ico");
            this.imageListTransferSteps.Images.SetKeyName(5, "Agent.ico");
            this.imageListTransferSteps.Images.SetKeyName(6, "Relation.ico");
            this.imageListTransferSteps.Images.SetKeyName(7, "Icones.ico");
            this.imageListTransferSteps.Images.SetKeyName(8, "Specimen.ico");
            this.imageListTransferSteps.Images.SetKeyName(9, "Processing.ico");
            this.imageListTransferSteps.Images.SetKeyName(10, "Plant.ico");
            this.imageListTransferSteps.Images.SetKeyName(11, "UnitInPart.ico");
            this.imageListTransferSteps.Images.SetKeyName(12, "NameAccepted.ico");
            this.imageListTransferSteps.Images.SetKeyName(13, "Analysis.ico");
            this.imageListTransferSteps.Images.SetKeyName(14, "Collection.ico");
            this.imageListTransferSteps.Images.SetKeyName(15, "Note.ico");
            this.imageListTransferSteps.Images.SetKeyName(16, "References.ico");
            this.imageListTransferSteps.Images.SetKeyName(17, "Identifier.ico");
            this.imageListTransferSteps.Images.SetKeyName(18, "Import.ico");
            this.imageListTransferSteps.Images.SetKeyName(19, "Tools.ico");
            this.imageListTransferSteps.Images.SetKeyName(20, "Parameter.ico");
            this.imageListTransferSteps.Images.SetKeyName(21, "InLines.ico");
            this.imageListTransferSteps.Images.SetKeyName(22, "Project.ico");
            this.imageListTransferSteps.Images.SetKeyName(23, "PartDescription.ico");
            this.imageListTransferSteps.Images.SetKeyName(24, "KeyBlue.ico");
            // 
            // UserControlProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Name = "UserControlProject";
            this.Size = new System.Drawing.Size(822, 88);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.contextMenuStripFilter.ResumeLayout(false);
            this.panelTransferCacheToPostgres.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelProject;
        private System.Windows.Forms.Label labelNumberInDatabase;
        private System.Windows.Forms.Label labelNumberInCacheDB;
        private System.Windows.Forms.Label labelNumberInPostgresDB;
        private System.Windows.Forms.Label labelLastUpdateInDatabase;
        private System.Windows.Forms.Label labelLastUpdateInCacheDatabase;
        private System.Windows.Forms.Label labelLastUpdateInPostgresDatabase;
        private System.Windows.Forms.Button buttonTransferDatabaseToCache;
        private System.Windows.Forms.Button buttonTransferCacheToPostgres;
        private System.Windows.Forms.Button buttonSetTransferFilter;
        private System.Windows.Forms.Button buttonViewCacheContent;
        private System.Windows.Forms.Button buttonViewPostgresContent;
        private System.Windows.Forms.Button buttonAdministratePackages;
        private System.Windows.Forms.ListBox listBoxPackages;
        private System.Windows.Forms.Button buttonRemoveProject;
        private System.Windows.Forms.Button buttonPostgresEstablishProject;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonUpdateCache;
        private System.Windows.Forms.Button buttonUpdatePostgres;
        private System.Windows.Forms.ImageList imageListButtonIcons;
        private System.Windows.Forms.ImageList imageListTransferSteps;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.CheckBox checkBoxIncludeInTransfer;
        private System.Windows.Forms.Button buttonTransferProtocol;
        private System.Windows.Forms.Button buttonProjectTransferToCacheDBFilter;
        private System.Windows.Forms.Button buttonProjectTransferToPostgresFilter;
        private System.Windows.Forms.CheckBox checkBoxPostgresIncludeInTransfer;
        private System.Windows.Forms.Button buttonPostgresTransferSettings;
        private System.Windows.Forms.Button buttonIncludeInTransfer;
        private System.Windows.Forms.Button buttonTransferErrors;
        private System.Windows.Forms.Button buttonPostgresTransferErrors;
        private System.Windows.Forms.Button buttonPostgresTransferProtocol;
        private System.Windows.Forms.Button buttonTransferPackageData;
        private System.Windows.Forms.Button buttonDatawithholding;
        private System.Windows.Forms.Label labelDatawithholding;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFilter;
        private System.Windows.Forms.ToolStripMenuItem viewDifferencesToolStripMenuItem;
        private System.Windows.Forms.Button buttonProjectTargets;
        private System.Windows.Forms.Button buttonHistoryInCacheDB;
        private System.Windows.Forms.Button buttonHistoryInPostgres;
        private System.Windows.Forms.Button buttonCreateArchive;
        private System.Windows.Forms.Button buttonGetDoi;
        private System.Windows.Forms.Label labelDoiInCacheDB;
        private System.Windows.Forms.Label labelDoiInPostgres;
        private System.Windows.Forms.Label labelDoiInPackage;
        private System.Windows.Forms.Panel panelTransferCacheToPostgres;
        private System.Windows.Forms.Button buttonTransferCacheToPostgresFile;
        private System.Windows.Forms.CheckBox checkBoxPostgresUseBulkTransfer;
        private System.Windows.Forms.Panel panelPostgresOtherTargets;
        private System.Windows.Forms.Button buttonProjectDifferingName;
        private System.Windows.Forms.Label labelProjectID;
        private System.Windows.Forms.TextBox textBoxProjectID;
    }
}
