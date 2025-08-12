namespace DiversityCollection.CacheDatabase
{
    partial class UserControlLookupSource
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonTransferToPostgresFilter = new System.Windows.Forms.Button();
            this.textBoxSource = new System.Windows.Forms.TextBox();
            this.textBoxServer = new System.Windows.Forms.TextBox();
            this.labelCountCacheDB = new System.Windows.Forms.Label();
            this.contextMenuStripCompetingTransfer = new System.Windows.Forms.ContextMenuStrip();
            this.clearCompetingTransferToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxView = new System.Windows.Forms.TextBox();
            this.buttonTransferToCache = new System.Windows.Forms.Button();
            this.buttonViewCacheDB = new System.Windows.Forms.Button();
            this.buttonTransferToPostgres = new System.Windows.Forms.Button();
            this.buttonViewPostgres = new System.Windows.Forms.Button();
            this.labelCountPostgres = new System.Windows.Forms.Label();
            this.buttonDeleteSource = new System.Windows.Forms.Button();
            this.contextMenuStripRecreate = new System.Windows.Forms.ContextMenuStrip();
            this.recreateSourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonTestView = new System.Windows.Forms.Button();
            this.textBoxDatabase = new System.Windows.Forms.TextBox();
            this.labelDatabase = new System.Windows.Forms.Label();
            this.labelServer = new System.Windows.Forms.Label();
            this.labelView = new System.Windows.Forms.Label();
            this.labelSource = new System.Windows.Forms.Label();
            this.buttonSetSubsets = new System.Windows.Forms.Button();
            this.buttonTransferToCacheFilter = new System.Windows.Forms.Button();
            this.checkBoxIncludeInTransfer = new System.Windows.Forms.CheckBox();
            this.buttonTransferSettings = new System.Windows.Forms.Button();
            this.buttonTransferProtocol = new System.Windows.Forms.Button();
            this.buttonTransferErrors = new System.Windows.Forms.Button();
            this.checkBoxPostgresIncludeInTransfer = new System.Windows.Forms.CheckBox();
            this.panelPostgresTargets = new System.Windows.Forms.Panel();
            this.buttonPostgresTransferSettings = new System.Windows.Forms.Button();
            this.buttonPostgresTransferProtocol = new System.Windows.Forms.Button();
            this.buttonPostgresTransferErrors = new System.Windows.Forms.Button();
            this.buttonViewPostgresCompetingSources = new System.Windows.Forms.Button();
            this.buttonHistoryInCacheDB = new System.Windows.Forms.Button();
            this.buttonHistoryInPostgres = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.buttonRecreateView = new System.Windows.Forms.Button();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.toolTip = new System.Windows.Forms.ToolTip();
            this.tableLayoutPanel.SuspendLayout();
            this.contextMenuStripCompetingTransfer.SuspendLayout();
            this.contextMenuStripRecreate.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 16;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel.Controls.Add(this.buttonTransferToPostgresFilter, 11, 1);
            this.tableLayoutPanel.Controls.Add(this.textBoxSource, 3, 3);
            this.tableLayoutPanel.Controls.Add(this.textBoxServer, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.labelCountCacheDB, 7, 1);
            this.tableLayoutPanel.Controls.Add(this.textBoxView, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonTransferToCache, 6, 2);
            this.tableLayoutPanel.Controls.Add(this.buttonViewCacheDB, 8, 2);
            this.tableLayoutPanel.Controls.Add(this.buttonTransferToPostgres, 11, 2);
            this.tableLayoutPanel.Controls.Add(this.buttonViewPostgres, 14, 2);
            this.tableLayoutPanel.Controls.Add(this.labelCountPostgres, 12, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonDeleteSource, 4, 3);
            this.tableLayoutPanel.Controls.Add(this.buttonTestView, 4, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxDatabase, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.labelDatabase, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelServer, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.labelView, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.labelSource, 3, 2);
            this.tableLayoutPanel.Controls.Add(this.buttonSetSubsets, 6, 4);
            this.tableLayoutPanel.Controls.Add(this.buttonTransferToCacheFilter, 6, 1);
            this.tableLayoutPanel.Controls.Add(this.checkBoxIncludeInTransfer, 6, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonTransferSettings, 7, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonTransferProtocol, 8, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonTransferErrors, 9, 0);
            this.tableLayoutPanel.Controls.Add(this.checkBoxPostgresIncludeInTransfer, 11, 0);
            this.tableLayoutPanel.Controls.Add(this.panelPostgresTargets, 11, 5);
            this.tableLayoutPanel.Controls.Add(this.buttonPostgresTransferSettings, 12, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonPostgresTransferProtocol, 14, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonPostgresTransferErrors, 15, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonViewPostgresCompetingSources, 14, 4);
            this.tableLayoutPanel.Controls.Add(this.buttonHistoryInCacheDB, 8, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonHistoryInPostgres, 14, 1);
            this.tableLayoutPanel.Controls.Add(this.progressBar, 0, 6);
            this.tableLayoutPanel.Controls.Add(this.buttonRecreateView, 4, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 7;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(669, 90);
            this.tableLayoutPanel.TabIndex = 2;
            // 
            // buttonTransferToPostgresFilter
            // 
            this.buttonTransferToPostgresFilter.BackColor = System.Drawing.Color.LightSteelBlue;
            this.buttonTransferToPostgresFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTransferToPostgresFilter.FlatAppearance.BorderSize = 0;
            this.buttonTransferToPostgresFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransferToPostgresFilter.Image = global::DiversityCollection.Resource.FilterClear;
            this.buttonTransferToPostgresFilter.Location = new System.Drawing.Point(512, 17);
            this.buttonTransferToPostgresFilter.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTransferToPostgresFilter.Name = "buttonTransferToPostgresFilter";
            this.buttonTransferToPostgresFilter.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.buttonTransferToPostgresFilter.Size = new System.Drawing.Size(20, 20);
            this.buttonTransferToPostgresFilter.TabIndex = 21;
            this.toolTip.SetToolTip(this.buttonTransferToPostgresFilter, "Data not filtered for updates");
            this.buttonTransferToPostgresFilter.UseVisualStyleBackColor = false;
            this.buttonTransferToPostgresFilter.Click += new System.EventHandler(this.buttonTransferToPostgresFilter_Click);
            // 
            // textBoxSource
            // 
            this.textBoxSource.BackColor = System.Drawing.Color.Khaki;
            this.textBoxSource.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSource.Location = new System.Drawing.Point(181, 50);
            this.textBoxSource.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxSource.Multiline = true;
            this.textBoxSource.Name = "textBoxSource";
            this.textBoxSource.ReadOnly = true;
            this.tableLayoutPanel.SetRowSpan(this.textBoxSource, 2);
            this.textBoxSource.Size = new System.Drawing.Size(152, 30);
            this.textBoxSource.TabIndex = 12;
            this.textBoxSource.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxServer
            // 
            this.textBoxServer.BackColor = System.Drawing.Color.Khaki;
            this.textBoxServer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxServer.Location = new System.Drawing.Point(0, 50);
            this.textBoxServer.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxServer.Multiline = true;
            this.textBoxServer.Name = "textBoxServer";
            this.textBoxServer.ReadOnly = true;
            this.tableLayoutPanel.SetRowSpan(this.textBoxServer, 2);
            this.textBoxServer.Size = new System.Drawing.Size(180, 30);
            this.textBoxServer.TabIndex = 11;
            this.textBoxServer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip.SetToolTip(this.textBoxServer, "The server where the database is located");
            // 
            // labelCountCacheDB
            // 
            this.labelCountCacheDB.AutoSize = true;
            this.labelCountCacheDB.BackColor = System.Drawing.Color.Thistle;
            this.labelCountCacheDB.ContextMenuStrip = this.contextMenuStripCompetingTransfer;
            this.labelCountCacheDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCountCacheDB.Location = new System.Drawing.Point(379, 17);
            this.labelCountCacheDB.Margin = new System.Windows.Forms.Padding(0);
            this.labelCountCacheDB.Name = "labelCountCacheDB";
            this.tableLayoutPanel.SetRowSpan(this.labelCountCacheDB, 4);
            this.labelCountCacheDB.Size = new System.Drawing.Size(101, 63);
            this.labelCountCacheDB.TabIndex = 3;
            this.labelCountCacheDB.Text = "label1";
            this.labelCountCacheDB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // contextMenuStripCompetingTransfer
            // 
            this.contextMenuStripCompetingTransfer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearCompetingTransferToolStripMenuItem});
            this.contextMenuStripCompetingTransfer.Name = "contextMenuStripCompetingTransfer";
            this.contextMenuStripCompetingTransfer.Size = new System.Drawing.Size(206, 26);
            // 
            // clearCompetingTransferToolStripMenuItem
            // 
            this.clearCompetingTransferToolStripMenuItem.ForeColor = System.Drawing.Color.Red;
            this.clearCompetingTransferToolStripMenuItem.Image = global::DiversityCollection.Resource.Delete;
            this.clearCompetingTransferToolStripMenuItem.Name = "clearCompetingTransferToolStripMenuItem";
            this.clearCompetingTransferToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.clearCompetingTransferToolStripMenuItem.Text = "Clear competing transfer";
            this.clearCompetingTransferToolStripMenuItem.Click += new System.EventHandler(this.clearCompetingTransferToolStripMenuItem_Click);
            // 
            // textBoxView
            // 
            this.textBoxView.BackColor = System.Drawing.Color.Khaki;
            this.textBoxView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxView.Location = new System.Drawing.Point(181, 17);
            this.textBoxView.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxView.Multiline = true;
            this.textBoxView.Name = "textBoxView";
            this.textBoxView.ReadOnly = true;
            this.textBoxView.Size = new System.Drawing.Size(152, 20);
            this.textBoxView.TabIndex = 2;
            this.textBoxView.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip.SetToolTip(this.textBoxView, "The view for selecting the data");
            // 
            // buttonTransferToCache
            // 
            this.buttonTransferToCache.BackColor = System.Drawing.Color.Thistle;
            this.buttonTransferToCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTransferToCache.FlatAppearance.BorderSize = 0;
            this.buttonTransferToCache.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransferToCache.Image = global::DiversityCollection.Resource.ArrowNext1;
            this.buttonTransferToCache.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonTransferToCache.Location = new System.Drawing.Point(359, 37);
            this.buttonTransferToCache.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTransferToCache.Name = "buttonTransferToCache";
            this.buttonTransferToCache.Padding = new System.Windows.Forms.Padding(0, 14, 0, 0);
            this.tableLayoutPanel.SetRowSpan(this.buttonTransferToCache, 2);
            this.buttonTransferToCache.Size = new System.Drawing.Size(20, 30);
            this.buttonTransferToCache.TabIndex = 6;
            this.toolTip.SetToolTip(this.buttonTransferToCache, "Transfer the data into the cache database");
            this.buttonTransferToCache.UseVisualStyleBackColor = false;
            this.buttonTransferToCache.Click += new System.EventHandler(this.buttonTransferToCache_Click);
            // 
            // buttonViewCacheDB
            // 
            this.buttonViewCacheDB.BackColor = System.Drawing.Color.Thistle;
            this.tableLayoutPanel.SetColumnSpan(this.buttonViewCacheDB, 2);
            this.buttonViewCacheDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonViewCacheDB.FlatAppearance.BorderSize = 0;
            this.buttonViewCacheDB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonViewCacheDB.Image = global::DiversityCollection.Resource.Lupe;
            this.buttonViewCacheDB.Location = new System.Drawing.Point(480, 37);
            this.buttonViewCacheDB.Margin = new System.Windows.Forms.Padding(0);
            this.buttonViewCacheDB.Name = "buttonViewCacheDB";
            this.tableLayoutPanel.SetRowSpan(this.buttonViewCacheDB, 3);
            this.buttonViewCacheDB.Size = new System.Drawing.Size(26, 43);
            this.buttonViewCacheDB.TabIndex = 7;
            this.toolTip.SetToolTip(this.buttonViewCacheDB, "Inspect the data transferred into the cache database");
            this.buttonViewCacheDB.UseVisualStyleBackColor = false;
            this.buttonViewCacheDB.Click += new System.EventHandler(this.buttonViewCacheDB_Click);
            // 
            // buttonTransferToPostgres
            // 
            this.buttonTransferToPostgres.BackColor = System.Drawing.Color.LightSteelBlue;
            this.buttonTransferToPostgres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTransferToPostgres.FlatAppearance.BorderSize = 0;
            this.buttonTransferToPostgres.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransferToPostgres.Image = global::DiversityCollection.Resource.ArrowNext1;
            this.buttonTransferToPostgres.Location = new System.Drawing.Point(512, 37);
            this.buttonTransferToPostgres.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTransferToPostgres.Name = "buttonTransferToPostgres";
            this.tableLayoutPanel.SetRowSpan(this.buttonTransferToPostgres, 3);
            this.buttonTransferToPostgres.Size = new System.Drawing.Size(20, 43);
            this.buttonTransferToPostgres.TabIndex = 8;
            this.toolTip.SetToolTip(this.buttonTransferToPostgres, "Transfer the data into the Postgres database");
            this.buttonTransferToPostgres.UseVisualStyleBackColor = false;
            this.buttonTransferToPostgres.Click += new System.EventHandler(this.buttonTransferToPostgres_Click);
            // 
            // buttonViewPostgres
            // 
            this.buttonViewPostgres.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tableLayoutPanel.SetColumnSpan(this.buttonViewPostgres, 2);
            this.buttonViewPostgres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonViewPostgres.FlatAppearance.BorderSize = 0;
            this.buttonViewPostgres.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonViewPostgres.Image = global::DiversityCollection.Resource.Lupe;
            this.buttonViewPostgres.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonViewPostgres.Location = new System.Drawing.Point(616, 37);
            this.buttonViewPostgres.Margin = new System.Windows.Forms.Padding(0);
            this.buttonViewPostgres.Name = "buttonViewPostgres";
            this.tableLayoutPanel.SetRowSpan(this.buttonViewPostgres, 2);
            this.buttonViewPostgres.Size = new System.Drawing.Size(53, 30);
            this.buttonViewPostgres.TabIndex = 9;
            this.toolTip.SetToolTip(this.buttonViewPostgres, "Inspect the data in the Postgres database");
            this.buttonViewPostgres.UseVisualStyleBackColor = false;
            this.buttonViewPostgres.Click += new System.EventHandler(this.buttonViewPostgres_Click);
            // 
            // labelCountPostgres
            // 
            this.labelCountPostgres.AutoSize = true;
            this.labelCountPostgres.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tableLayoutPanel.SetColumnSpan(this.labelCountPostgres, 2);
            this.labelCountPostgres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCountPostgres.Location = new System.Drawing.Point(532, 17);
            this.labelCountPostgres.Margin = new System.Windows.Forms.Padding(0);
            this.labelCountPostgres.Name = "labelCountPostgres";
            this.tableLayoutPanel.SetRowSpan(this.labelCountPostgres, 4);
            this.labelCountPostgres.Size = new System.Drawing.Size(84, 63);
            this.labelCountPostgres.TabIndex = 10;
            this.labelCountPostgres.Text = "label1";
            this.labelCountPostgres.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonDeleteSource
            // 
            this.buttonDeleteSource.BackColor = System.Drawing.Color.Khaki;
            this.buttonDeleteSource.ContextMenuStrip = this.contextMenuStripRecreate;
            this.buttonDeleteSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDeleteSource.FlatAppearance.BorderSize = 0;
            this.buttonDeleteSource.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDeleteSource.Image = global::DiversityCollection.Resource.Delete;
            this.buttonDeleteSource.Location = new System.Drawing.Point(333, 50);
            this.buttonDeleteSource.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDeleteSource.Name = "buttonDeleteSource";
            this.tableLayoutPanel.SetRowSpan(this.buttonDeleteSource, 2);
            this.buttonDeleteSource.Size = new System.Drawing.Size(20, 30);
            this.buttonDeleteSource.TabIndex = 5;
            this.toolTip.SetToolTip(this.buttonDeleteSource, "Remove this datasource");
            this.buttonDeleteSource.UseVisualStyleBackColor = false;
            this.buttonDeleteSource.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // contextMenuStripRecreate
            // 
            this.contextMenuStripRecreate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recreateSourceToolStripMenuItem});
            this.contextMenuStripRecreate.Name = "contextMenuStripRecreate";
            this.contextMenuStripRecreate.Size = new System.Drawing.Size(158, 26);
            // 
            // recreateSourceToolStripMenuItem
            // 
            this.recreateSourceToolStripMenuItem.Image = global::DiversityCollection.Resource.Update;
            this.recreateSourceToolStripMenuItem.Name = "recreateSourceToolStripMenuItem";
            this.recreateSourceToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.recreateSourceToolStripMenuItem.Text = "Recreate source";
            this.recreateSourceToolStripMenuItem.Click += new System.EventHandler(this.recreateSourceToolStripMenuItem_Click);
            // 
            // buttonTestView
            // 
            this.buttonTestView.BackColor = System.Drawing.Color.Khaki;
            this.buttonTestView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTestView.FlatAppearance.BorderSize = 0;
            this.buttonTestView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTestView.Image = global::DiversityCollection.Resource.Lupe;
            this.buttonTestView.Location = new System.Drawing.Point(333, 0);
            this.buttonTestView.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTestView.Name = "buttonTestView";
            this.buttonTestView.Size = new System.Drawing.Size(20, 17);
            this.buttonTestView.TabIndex = 4;
            this.toolTip.SetToolTip(this.buttonTestView, "Inspect the data provided for transfer");
            this.buttonTestView.UseVisualStyleBackColor = false;
            this.buttonTestView.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // textBoxDatabase
            // 
            this.textBoxDatabase.BackColor = System.Drawing.Color.Khaki;
            this.textBoxDatabase.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDatabase.Location = new System.Drawing.Point(0, 17);
            this.textBoxDatabase.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxDatabase.Multiline = true;
            this.textBoxDatabase.Name = "textBoxDatabase";
            this.textBoxDatabase.ReadOnly = true;
            this.textBoxDatabase.Size = new System.Drawing.Size(180, 20);
            this.textBoxDatabase.TabIndex = 0;
            this.textBoxDatabase.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip.SetToolTip(this.textBoxDatabase, "The source database for the data");
            // 
            // labelDatabase
            // 
            this.labelDatabase.BackColor = System.Drawing.Color.Khaki;
            this.labelDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDatabase.ForeColor = System.Drawing.Color.Gray;
            this.labelDatabase.Location = new System.Drawing.Point(0, 0);
            this.labelDatabase.Margin = new System.Windows.Forms.Padding(0);
            this.labelDatabase.Name = "labelDatabase";
            this.labelDatabase.Size = new System.Drawing.Size(180, 17);
            this.labelDatabase.TabIndex = 13;
            this.labelDatabase.Text = "Database";
            // 
            // labelServer
            // 
            this.labelServer.AutoSize = true;
            this.labelServer.BackColor = System.Drawing.Color.Khaki;
            this.labelServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelServer.ForeColor = System.Drawing.Color.Gray;
            this.labelServer.Location = new System.Drawing.Point(0, 37);
            this.labelServer.Margin = new System.Windows.Forms.Padding(0);
            this.labelServer.Name = "labelServer";
            this.labelServer.Size = new System.Drawing.Size(180, 13);
            this.labelServer.TabIndex = 14;
            this.labelServer.Text = "Server";
            // 
            // labelView
            // 
            this.labelView.AutoSize = true;
            this.labelView.BackColor = System.Drawing.Color.Khaki;
            this.labelView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelView.ForeColor = System.Drawing.Color.Gray;
            this.labelView.Location = new System.Drawing.Point(181, 0);
            this.labelView.Margin = new System.Windows.Forms.Padding(0);
            this.labelView.Name = "labelView";
            this.labelView.Size = new System.Drawing.Size(152, 17);
            this.labelView.TabIndex = 15;
            this.labelView.Text = "View";
            // 
            // labelSource
            // 
            this.labelSource.AutoSize = true;
            this.labelSource.BackColor = System.Drawing.Color.Khaki;
            this.labelSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSource.ForeColor = System.Drawing.Color.Gray;
            this.labelSource.Location = new System.Drawing.Point(181, 37);
            this.labelSource.Margin = new System.Windows.Forms.Padding(0);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(152, 13);
            this.labelSource.TabIndex = 16;
            this.labelSource.Text = "Source";
            // 
            // buttonSetSubsets
            // 
            this.buttonSetSubsets.BackColor = System.Drawing.Color.Thistle;
            this.buttonSetSubsets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSetSubsets.FlatAppearance.BorderSize = 0;
            this.buttonSetSubsets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSetSubsets.Image = global::DiversityCollection.Resource.SettingsNarrow;
            this.buttonSetSubsets.Location = new System.Drawing.Point(359, 67);
            this.buttonSetSubsets.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSetSubsets.Name = "buttonSetSubsets";
            this.buttonSetSubsets.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.buttonSetSubsets.Size = new System.Drawing.Size(20, 13);
            this.buttonSetSubsets.TabIndex = 17;
            this.toolTip.SetToolTip(this.buttonSetSubsets, "Define the subsets that should be transferred");
            this.buttonSetSubsets.UseVisualStyleBackColor = false;
            this.buttonSetSubsets.Click += new System.EventHandler(this.buttonSetSubsets_Click);
            // 
            // buttonTransferToCacheFilter
            // 
            this.buttonTransferToCacheFilter.BackColor = System.Drawing.Color.Thistle;
            this.buttonTransferToCacheFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTransferToCacheFilter.FlatAppearance.BorderSize = 0;
            this.buttonTransferToCacheFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransferToCacheFilter.Image = global::DiversityCollection.Resource.FilterClear;
            this.buttonTransferToCacheFilter.Location = new System.Drawing.Point(359, 17);
            this.buttonTransferToCacheFilter.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTransferToCacheFilter.Name = "buttonTransferToCacheFilter";
            this.buttonTransferToCacheFilter.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.buttonTransferToCacheFilter.Size = new System.Drawing.Size(20, 20);
            this.buttonTransferToCacheFilter.TabIndex = 20;
            this.toolTip.SetToolTip(this.buttonTransferToCacheFilter, "Data not filtered for updates");
            this.buttonTransferToCacheFilter.UseVisualStyleBackColor = false;
            this.buttonTransferToCacheFilter.Click += new System.EventHandler(this.buttonTransferToCacheFilter_Click);
            // 
            // checkBoxIncludeInTransfer
            // 
            this.checkBoxIncludeInTransfer.AutoSize = true;
            this.checkBoxIncludeInTransfer.BackColor = System.Drawing.Color.Thistle;
            this.checkBoxIncludeInTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxIncludeInTransfer.Location = new System.Drawing.Point(359, 0);
            this.checkBoxIncludeInTransfer.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxIncludeInTransfer.Name = "checkBoxIncludeInTransfer";
            this.checkBoxIncludeInTransfer.Padding = new System.Windows.Forms.Padding(2, 3, 0, 0);
            this.checkBoxIncludeInTransfer.Size = new System.Drawing.Size(20, 17);
            this.checkBoxIncludeInTransfer.TabIndex = 18;
            this.toolTip.SetToolTip(this.checkBoxIncludeInTransfer, "If this source should be included in the schedule based transfer of the data");
            this.checkBoxIncludeInTransfer.UseVisualStyleBackColor = false;
            this.checkBoxIncludeInTransfer.Click += new System.EventHandler(this.checkBoxIncludeInTransfer_Click);
            // 
            // buttonTransferSettings
            // 
            this.buttonTransferSettings.BackColor = System.Drawing.Color.Thistle;
            this.buttonTransferSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTransferSettings.FlatAppearance.BorderSize = 0;
            this.buttonTransferSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransferSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTransferSettings.ForeColor = System.Drawing.SystemColors.WindowText;
            this.buttonTransferSettings.Image = global::DiversityCollection.Resource.Time;
            this.buttonTransferSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonTransferSettings.Location = new System.Drawing.Point(379, 0);
            this.buttonTransferSettings.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTransferSettings.Name = "buttonTransferSettings";
            this.buttonTransferSettings.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.buttonTransferSettings.Size = new System.Drawing.Size(101, 17);
            this.buttonTransferSettings.TabIndex = 23;
            this.buttonTransferSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonTransferSettings, "The setting for the scheduled transfer");
            this.buttonTransferSettings.UseVisualStyleBackColor = false;
            this.buttonTransferSettings.Click += new System.EventHandler(this.buttonTransferSettings_Click);
            // 
            // buttonTransferProtocol
            // 
            this.buttonTransferProtocol.BackColor = System.Drawing.Color.Thistle;
            this.buttonTransferProtocol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTransferProtocol.FlatAppearance.BorderSize = 0;
            this.buttonTransferProtocol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransferProtocol.Image = global::DiversityCollection.Resource.List;
            this.buttonTransferProtocol.Location = new System.Drawing.Point(480, 0);
            this.buttonTransferProtocol.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTransferProtocol.Name = "buttonTransferProtocol";
            this.buttonTransferProtocol.Size = new System.Drawing.Size(13, 17);
            this.buttonTransferProtocol.TabIndex = 19;
            this.toolTip.SetToolTip(this.buttonTransferProtocol, "The protocol of the schedule based transfer of the data");
            this.buttonTransferProtocol.UseVisualStyleBackColor = false;
            this.buttonTransferProtocol.Click += new System.EventHandler(this.buttonTransferProtocol_Click);
            // 
            // buttonTransferErrors
            // 
            this.buttonTransferErrors.BackColor = System.Drawing.Color.Thistle;
            this.buttonTransferErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTransferErrors.FlatAppearance.BorderSize = 0;
            this.buttonTransferErrors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransferErrors.Image = global::DiversityCollection.Resource.ListRed;
            this.buttonTransferErrors.Location = new System.Drawing.Point(493, 0);
            this.buttonTransferErrors.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTransferErrors.Name = "buttonTransferErrors";
            this.buttonTransferErrors.Size = new System.Drawing.Size(13, 17);
            this.buttonTransferErrors.TabIndex = 22;
            this.toolTip.SetToolTip(this.buttonTransferErrors, "The errors that occured during the schedule based transfer of the data");
            this.buttonTransferErrors.UseVisualStyleBackColor = false;
            this.buttonTransferErrors.Click += new System.EventHandler(this.buttonTransferErrors_Click);
            // 
            // checkBoxPostgresIncludeInTransfer
            // 
            this.checkBoxPostgresIncludeInTransfer.AutoSize = true;
            this.checkBoxPostgresIncludeInTransfer.BackColor = System.Drawing.Color.LightSteelBlue;
            this.checkBoxPostgresIncludeInTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxPostgresIncludeInTransfer.Enabled = false;
            this.checkBoxPostgresIncludeInTransfer.Location = new System.Drawing.Point(512, 0);
            this.checkBoxPostgresIncludeInTransfer.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxPostgresIncludeInTransfer.Name = "checkBoxPostgresIncludeInTransfer";
            this.checkBoxPostgresIncludeInTransfer.Padding = new System.Windows.Forms.Padding(3, 2, 0, 0);
            this.checkBoxPostgresIncludeInTransfer.Size = new System.Drawing.Size(20, 17);
            this.checkBoxPostgresIncludeInTransfer.TabIndex = 24;
            this.toolTip.SetToolTip(this.checkBoxPostgresIncludeInTransfer, "If the data should be included in the scheduled transfer");
            this.checkBoxPostgresIncludeInTransfer.UseVisualStyleBackColor = false;
            this.checkBoxPostgresIncludeInTransfer.Click += new System.EventHandler(this.checkBoxPostgresIncludeInTransfer_Click);
            // 
            // panelPostgresTargets
            // 
            this.tableLayoutPanel.SetColumnSpan(this.panelPostgresTargets, 5);
            this.panelPostgresTargets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPostgresTargets.Location = new System.Drawing.Point(512, 80);
            this.panelPostgresTargets.Margin = new System.Windows.Forms.Padding(0);
            this.panelPostgresTargets.Name = "panelPostgresTargets";
            this.panelPostgresTargets.Size = new System.Drawing.Size(157, 1);
            this.panelPostgresTargets.TabIndex = 25;
            // 
            // buttonPostgresTransferSettings
            // 
            this.buttonPostgresTransferSettings.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tableLayoutPanel.SetColumnSpan(this.buttonPostgresTransferSettings, 2);
            this.buttonPostgresTransferSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPostgresTransferSettings.Enabled = false;
            this.buttonPostgresTransferSettings.FlatAppearance.BorderSize = 0;
            this.buttonPostgresTransferSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPostgresTransferSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPostgresTransferSettings.ForeColor = System.Drawing.SystemColors.WindowText;
            this.buttonPostgresTransferSettings.Image = global::DiversityCollection.Resource.Time;
            this.buttonPostgresTransferSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonPostgresTransferSettings.Location = new System.Drawing.Point(532, 0);
            this.buttonPostgresTransferSettings.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPostgresTransferSettings.Name = "buttonPostgresTransferSettings";
            this.buttonPostgresTransferSettings.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.buttonPostgresTransferSettings.Size = new System.Drawing.Size(84, 17);
            this.buttonPostgresTransferSettings.TabIndex = 26;
            this.buttonPostgresTransferSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonPostgresTransferSettings, "The setting for the scheduled transfer");
            this.buttonPostgresTransferSettings.UseVisualStyleBackColor = false;
            this.buttonPostgresTransferSettings.Click += new System.EventHandler(this.buttonPostgresTransferSettings_Click);
            // 
            // buttonPostgresTransferProtocol
            // 
            this.buttonPostgresTransferProtocol.BackColor = System.Drawing.Color.LightSteelBlue;
            this.buttonPostgresTransferProtocol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPostgresTransferProtocol.Enabled = false;
            this.buttonPostgresTransferProtocol.FlatAppearance.BorderSize = 0;
            this.buttonPostgresTransferProtocol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPostgresTransferProtocol.Image = global::DiversityCollection.Resource.List;
            this.buttonPostgresTransferProtocol.Location = new System.Drawing.Point(616, 0);
            this.buttonPostgresTransferProtocol.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPostgresTransferProtocol.Name = "buttonPostgresTransferProtocol";
            this.buttonPostgresTransferProtocol.Size = new System.Drawing.Size(13, 17);
            this.buttonPostgresTransferProtocol.TabIndex = 27;
            this.toolTip.SetToolTip(this.buttonPostgresTransferProtocol, "The protocol of the schedule based transfer of the data");
            this.buttonPostgresTransferProtocol.UseVisualStyleBackColor = false;
            this.buttonPostgresTransferProtocol.Click += new System.EventHandler(this.buttonPostgresTransferProtocol_Click);
            // 
            // buttonPostgresTransferErrors
            // 
            this.buttonPostgresTransferErrors.BackColor = System.Drawing.Color.LightSteelBlue;
            this.buttonPostgresTransferErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPostgresTransferErrors.Enabled = false;
            this.buttonPostgresTransferErrors.FlatAppearance.BorderSize = 0;
            this.buttonPostgresTransferErrors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPostgresTransferErrors.Image = global::DiversityCollection.Resource.ListRed;
            this.buttonPostgresTransferErrors.Location = new System.Drawing.Point(629, 0);
            this.buttonPostgresTransferErrors.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPostgresTransferErrors.Name = "buttonPostgresTransferErrors";
            this.buttonPostgresTransferErrors.Size = new System.Drawing.Size(40, 17);
            this.buttonPostgresTransferErrors.TabIndex = 28;
            this.toolTip.SetToolTip(this.buttonPostgresTransferErrors, "The errors that occured during the schedule based transfer of the data");
            this.buttonPostgresTransferErrors.UseVisualStyleBackColor = false;
            this.buttonPostgresTransferErrors.Click += new System.EventHandler(this.buttonPostgresTransferErrors_Click);
            // 
            // buttonViewPostgresCompetingSources
            // 
            this.buttonViewPostgresCompetingSources.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tableLayoutPanel.SetColumnSpan(this.buttonViewPostgresCompetingSources, 2);
            this.buttonViewPostgresCompetingSources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonViewPostgresCompetingSources.Enabled = false;
            this.buttonViewPostgresCompetingSources.FlatAppearance.BorderSize = 0;
            this.buttonViewPostgresCompetingSources.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonViewPostgresCompetingSources.ForeColor = System.Drawing.Color.Red;
            this.buttonViewPostgresCompetingSources.Image = global::DiversityCollection.Resource.Remove;
            this.buttonViewPostgresCompetingSources.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonViewPostgresCompetingSources.Location = new System.Drawing.Point(616, 67);
            this.buttonViewPostgresCompetingSources.Margin = new System.Windows.Forms.Padding(0);
            this.buttonViewPostgresCompetingSources.Name = "buttonViewPostgresCompetingSources";
            this.buttonViewPostgresCompetingSources.Size = new System.Drawing.Size(53, 13);
            this.buttonViewPostgresCompetingSources.TabIndex = 29;
            this.toolTip.SetToolTip(this.buttonViewPostgresCompetingSources, "Show overview for number of other sources");
            this.buttonViewPostgresCompetingSources.UseVisualStyleBackColor = false;
            this.buttonViewPostgresCompetingSources.Click += new System.EventHandler(this.buttonViewPostgresCompetingSources_Click);
            // 
            // buttonHistoryInCacheDB
            // 
            this.buttonHistoryInCacheDB.BackColor = System.Drawing.Color.Thistle;
            this.tableLayoutPanel.SetColumnSpan(this.buttonHistoryInCacheDB, 2);
            this.buttonHistoryInCacheDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonHistoryInCacheDB.FlatAppearance.BorderSize = 0;
            this.buttonHistoryInCacheDB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHistoryInCacheDB.Image = global::DiversityCollection.Resource.History;
            this.buttonHistoryInCacheDB.Location = new System.Drawing.Point(480, 17);
            this.buttonHistoryInCacheDB.Margin = new System.Windows.Forms.Padding(0);
            this.buttonHistoryInCacheDB.Name = "buttonHistoryInCacheDB";
            this.buttonHistoryInCacheDB.Size = new System.Drawing.Size(26, 20);
            this.buttonHistoryInCacheDB.TabIndex = 30;
            this.buttonHistoryInCacheDB.UseVisualStyleBackColor = false;
            this.buttonHistoryInCacheDB.Click += new System.EventHandler(this.buttonHistoryInCacheDB_Click);
            // 
            // buttonHistoryInPostgres
            // 
            this.buttonHistoryInPostgres.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tableLayoutPanel.SetColumnSpan(this.buttonHistoryInPostgres, 2);
            this.buttonHistoryInPostgres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonHistoryInPostgres.FlatAppearance.BorderSize = 0;
            this.buttonHistoryInPostgres.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHistoryInPostgres.Image = global::DiversityCollection.Resource.History;
            this.buttonHistoryInPostgres.Location = new System.Drawing.Point(616, 17);
            this.buttonHistoryInPostgres.Margin = new System.Windows.Forms.Padding(0);
            this.buttonHistoryInPostgres.Name = "buttonHistoryInPostgres";
            this.buttonHistoryInPostgres.Size = new System.Drawing.Size(53, 20);
            this.buttonHistoryInPostgres.TabIndex = 31;
            this.buttonHistoryInPostgres.UseVisualStyleBackColor = false;
            this.buttonHistoryInPostgres.Click += new System.EventHandler(this.buttonHistoryInPostgres_Click);
            // 
            // progressBar
            // 
            this.tableLayoutPanel.SetColumnSpan(this.progressBar, 16);
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar.Location = new System.Drawing.Point(1, 82);
            this.progressBar.Margin = new System.Windows.Forms.Padding(1, 1, 1, 3);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(667, 5);
            this.progressBar.TabIndex = 32;
            this.progressBar.Visible = false;
            // 
            // buttonRecreateView
            // 
            this.buttonRecreateView.BackColor = System.Drawing.Color.Khaki;
            this.buttonRecreateView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRecreateView.FlatAppearance.BorderSize = 0;
            this.buttonRecreateView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRecreateView.Location = new System.Drawing.Point(333, 17);
            this.buttonRecreateView.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRecreateView.Name = "buttonRecreateView";
            this.tableLayoutPanel.SetRowSpan(this.buttonRecreateView, 2);
            this.buttonRecreateView.Size = new System.Drawing.Size(20, 33);
            this.buttonRecreateView.TabIndex = 33;
            this.toolTip.SetToolTip(this.buttonRecreateView, "Recreate source");
            this.buttonRecreateView.UseVisualStyleBackColor = false;
            this.buttonRecreateView.Click += new System.EventHandler(this.buttonRecreateView_Click);
            // 
            // UserControlLookupSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlLookupSource";
            this.Size = new System.Drawing.Size(669, 90);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.contextMenuStripCompetingTransfer.ResumeLayout(false);
            this.contextMenuStripRecreate.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TextBox textBoxDatabase;
        private System.Windows.Forms.Label labelCountCacheDB;
        private System.Windows.Forms.TextBox textBoxView;
        private System.Windows.Forms.Button buttonTestView;
        private System.Windows.Forms.Button buttonTransferToCache;
        private System.Windows.Forms.Button buttonViewCacheDB;
        private System.Windows.Forms.Button buttonTransferToPostgres;
        private System.Windows.Forms.Button buttonViewPostgres;
        private System.Windows.Forms.Label labelCountPostgres;
        private System.Windows.Forms.Button buttonDeleteSource;
        private System.Windows.Forms.TextBox textBoxSource;
        private System.Windows.Forms.TextBox textBoxServer;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Label labelDatabase;
        private System.Windows.Forms.Label labelServer;
        private System.Windows.Forms.Label labelView;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.Button buttonSetSubsets;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox checkBoxIncludeInTransfer;
        private System.Windows.Forms.Button buttonTransferProtocol;
        private System.Windows.Forms.Button buttonTransferToCacheFilter;
        private System.Windows.Forms.Button buttonTransferToPostgresFilter;
        private System.Windows.Forms.Button buttonTransferErrors;
        private System.Windows.Forms.Button buttonTransferSettings;
        private System.Windows.Forms.CheckBox checkBoxPostgresIncludeInTransfer;
        private System.Windows.Forms.Button buttonPostgresTransferSettings;
        private System.Windows.Forms.Button buttonPostgresTransferProtocol;
        private System.Windows.Forms.Button buttonPostgresTransferErrors;
        private System.Windows.Forms.Button buttonViewPostgresCompetingSources;
        private System.Windows.Forms.Button buttonHistoryInCacheDB;
        private System.Windows.Forms.Button buttonHistoryInPostgres;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripRecreate;
        private System.Windows.Forms.ToolStripMenuItem recreateSourceToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripCompetingTransfer;
        private System.Windows.Forms.ToolStripMenuItem clearCompetingTransferToolStripMenuItem;
        private System.Windows.Forms.Button buttonRecreateView;
        private System.Windows.Forms.Panel panelPostgresTargets;
    }
}
