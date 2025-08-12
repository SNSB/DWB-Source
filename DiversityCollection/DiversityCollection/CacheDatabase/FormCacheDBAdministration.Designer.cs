namespace DiversityCollection.Forms
{
    partial class FormCacheDBAdministration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCacheDBAdministration));
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageChooseDB = new System.Windows.Forms.TabPage();
            this.splitContainerSelectDatabase = new System.Windows.Forms.SplitContainer();
            this.listBoxDatabase = new System.Windows.Forms.ListBox();
            this.cacheDatabaseBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetCacheDB = new DiversityCollection.CacheDatabase.DataSetCacheDB();
            this.toolStripDatabase = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonDatabaseNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDatabaseDelete = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanelDatabase = new System.Windows.Forms.TableLayoutPanel();
            this.labelDatabaseName = new System.Windows.Forms.Label();
            this.labelServer = new System.Windows.Forms.Label();
            this.textBoxServer = new System.Windows.Forms.TextBox();
            this.labelPort = new System.Windows.Forms.Label();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.labelProjectsDatabase = new System.Windows.Forms.Label();
            this.textBoxDatabaseName = new System.Windows.Forms.TextBox();
            this.buttonLoginAdministration = new System.Windows.Forms.Button();
            this.buttonUpdateDatabase = new System.Windows.Forms.Button();
            this.textBoxProjectsDatabase = new System.Windows.Forms.TextBox();
            this.textBoxCurrentDatabaseVersion = new System.Windows.Forms.TextBox();
            this.labelCurrentDatabaseVersion = new System.Windows.Forms.Label();
            this.textBoxAvailableDatabaseVersion = new System.Windows.Forms.TextBox();
            this.tabPageProjects = new System.Windows.Forms.TabPage();
            this.splitContainerProjects = new System.Windows.Forms.SplitContainer();
            this.listBoxProjects = new System.Windows.Forms.ListBox();
            this.projectProxyBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStripProjects = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonProjectAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonProjectRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonProjectSave = new System.Windows.Forms.ToolStripButton();
            this.tabControlProject = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelGeneralProjectSettings = new System.Windows.Forms.TableLayoutPanel();
            this.labelProjectDataType = new System.Windows.Forms.Label();
            this.comboBoxProjectDataType = new System.Windows.Forms.ComboBox();
            this.checkBoxCoordinatePrecision = new System.Windows.Forms.CheckBox();
            this.numericUpDownCoordinatePrecision = new System.Windows.Forms.NumericUpDown();
            this.checkBoxRestrictTaxonomicGroups = new System.Windows.Forms.CheckBox();
            this.checkBoxRestrictMaterialCategories = new System.Windows.Forms.CheckBox();
            this.checkBoxRestrictLocalisations = new System.Windows.Forms.CheckBox();
            this.buttonShowProjectDataTypeDescription = new System.Windows.Forms.Button();
            this.checkBoxRestrictImages = new System.Windows.Forms.CheckBox();
            this.labelCoordinatePrecision = new System.Windows.Forms.Label();
            this.tabPageTaxonomicGroups = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelTaxonomicGroups = new System.Windows.Forms.TableLayoutPanel();
            this.labelProjectTaxonomicGroup = new System.Windows.Forms.Label();
            this.buttonRequeryTaxonomicGroups = new System.Windows.Forms.Button();
            this.panelTaxonomicGroups = new System.Windows.Forms.Panel();
            this.tabPageMaterialCategories = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelMaterialCategories = new System.Windows.Forms.TableLayoutPanel();
            this.buttonRequeryMaterialCategories = new System.Windows.Forms.Button();
            this.labelMaterialCategories = new System.Windows.Forms.Label();
            this.panelMaterialCategories = new System.Windows.Forms.Panel();
            this.tabPageLocalisations = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelLocalisations = new System.Windows.Forms.TableLayoutPanel();
            this.buttonRequeryLocalisations = new System.Windows.Forms.Button();
            this.labelLocalisations = new System.Windows.Forms.Label();
            this.panelLocalisations = new System.Windows.Forms.Panel();
            this.tabPageImages = new System.Windows.Forms.TabPage();
            this.tabControlImages = new System.Windows.Forms.TabControl();
            this.tabPageImagesSpecimen = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelImagesSpecimen = new System.Windows.Forms.TableLayoutPanel();
            this.buttonRequeryImagesSpecimen = new System.Windows.Forms.Button();
            this.labelImagesSpecimen = new System.Windows.Forms.Label();
            this.panelImagesSpecimen = new System.Windows.Forms.Panel();
            this.tabPageImagesEvent = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelImagesEvent = new System.Windows.Forms.TableLayoutPanel();
            this.buttonRequeryImagesEvent = new System.Windows.Forms.Button();
            this.labelImagesEvent = new System.Windows.Forms.Label();
            this.panelImagesEvent = new System.Windows.Forms.Panel();
            this.tabPageImagesSeries = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelImagesSeries = new System.Windows.Forms.TableLayoutPanel();
            this.buttonRequeryImagesSeries = new System.Windows.Forms.Button();
            this.labelImagesSeries = new System.Windows.Forms.Label();
            this.panelImagesSeries = new System.Windows.Forms.Panel();
            this.tabPageZoomView = new System.Windows.Forms.TabPage();
            this.imageListTabControl = new System.Windows.Forms.ImageList(this.components);
            this.tabPageTaxonomy = new System.Windows.Forms.TabPage();
            this.splitContainerTaxonomy = new System.Windows.Forms.SplitContainer();
            this.panelTaxonomy = new System.Windows.Forms.Panel();
            this.toolStripTaxonomy = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonTaxonomyAdd = new System.Windows.Forms.ToolStripButton();
            this.dataGridViewTaxonProject = new System.Windows.Forms.DataGridView();
            this.dataSourceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.databaseNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.projectIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sequenceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.projectDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.diversityTaxonNamesProjectSequenceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStripTaxonomyProjects = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonTaxonProjectAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonTaxonProjectRemove = new System.Windows.Forms.ToolStripButton();
            this.labelTaxonomyProjects = new System.Windows.Forms.Label();
            this.projectProxyTableAdapter = new DiversityCollection.CacheDatabase.DataSetCacheDBTableAdapters.ProjectProxyTableAdapter();
            this.diversityTaxonNamesProjectSequenceTableAdapter = new DiversityCollection.CacheDatabase.DataSetCacheDBTableAdapters.DiversityTaxonNamesProjectSequenceTableAdapter();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.cacheDatabaseTableAdapter = new DiversityCollection.CacheDatabase.DataSetCacheDBTableAdapters.CacheDatabaseTableAdapter();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.tabControlMain.SuspendLayout();
            this.tabPageChooseDB.SuspendLayout();
            this.splitContainerSelectDatabase.Panel1.SuspendLayout();
            this.splitContainerSelectDatabase.Panel2.SuspendLayout();
            this.splitContainerSelectDatabase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cacheDatabaseBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCacheDB)).BeginInit();
            this.toolStripDatabase.SuspendLayout();
            this.tableLayoutPanelDatabase.SuspendLayout();
            this.tabPageProjects.SuspendLayout();
            this.splitContainerProjects.Panel1.SuspendLayout();
            this.splitContainerProjects.Panel2.SuspendLayout();
            this.splitContainerProjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.projectProxyBindingSource)).BeginInit();
            this.toolStripProjects.SuspendLayout();
            this.tabControlProject.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.tableLayoutPanelGeneralProjectSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCoordinatePrecision)).BeginInit();
            this.tabPageTaxonomicGroups.SuspendLayout();
            this.tableLayoutPanelTaxonomicGroups.SuspendLayout();
            this.tabPageMaterialCategories.SuspendLayout();
            this.tableLayoutPanelMaterialCategories.SuspendLayout();
            this.tabPageLocalisations.SuspendLayout();
            this.tableLayoutPanelLocalisations.SuspendLayout();
            this.tabPageImages.SuspendLayout();
            this.tabControlImages.SuspendLayout();
            this.tabPageImagesSpecimen.SuspendLayout();
            this.tableLayoutPanelImagesSpecimen.SuspendLayout();
            this.tabPageImagesEvent.SuspendLayout();
            this.tableLayoutPanelImagesEvent.SuspendLayout();
            this.tabPageImagesSeries.SuspendLayout();
            this.tableLayoutPanelImagesSeries.SuspendLayout();
            this.tabPageTaxonomy.SuspendLayout();
            this.splitContainerTaxonomy.Panel1.SuspendLayout();
            this.splitContainerTaxonomy.Panel2.SuspendLayout();
            this.splitContainerTaxonomy.SuspendLayout();
            this.toolStripTaxonomy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTaxonProject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.diversityTaxonNamesProjectSequenceBindingSource)).BeginInit();
            this.toolStripTaxonomyProjects.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageChooseDB);
            this.tabControlMain.Controls.Add(this.tabPageProjects);
            this.tabControlMain.Controls.Add(this.tabPageTaxonomy);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.ImageList = this.imageListTabControl;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(804, 357);
            this.tabControlMain.TabIndex = 1;
            // 
            // tabPageChooseDB
            // 
            this.tabPageChooseDB.Controls.Add(this.splitContainerSelectDatabase);
            this.tabPageChooseDB.ImageIndex = 0;
            this.tabPageChooseDB.Location = new System.Drawing.Point(4, 23);
            this.tabPageChooseDB.Name = "tabPageChooseDB";
            this.tabPageChooseDB.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageChooseDB.Size = new System.Drawing.Size(796, 330);
            this.tabPageChooseDB.TabIndex = 0;
            this.tabPageChooseDB.Text = "Select database";
            this.tabPageChooseDB.UseVisualStyleBackColor = true;
            // 
            // splitContainerSelectDatabase
            // 
            this.splitContainerSelectDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerSelectDatabase.Location = new System.Drawing.Point(3, 3);
            this.splitContainerSelectDatabase.Name = "splitContainerSelectDatabase";
            // 
            // splitContainerSelectDatabase.Panel1
            // 
            this.splitContainerSelectDatabase.Panel1.Controls.Add(this.listBoxDatabase);
            this.splitContainerSelectDatabase.Panel1.Controls.Add(this.toolStripDatabase);
            // 
            // splitContainerSelectDatabase.Panel2
            // 
            this.splitContainerSelectDatabase.Panel2.Controls.Add(this.tableLayoutPanelDatabase);
            this.splitContainerSelectDatabase.Size = new System.Drawing.Size(790, 324);
            this.splitContainerSelectDatabase.SplitterDistance = 262;
            this.splitContainerSelectDatabase.TabIndex = 1;
            // 
            // listBoxDatabase
            // 
            this.listBoxDatabase.DataSource = this.cacheDatabaseBindingSource;
            this.listBoxDatabase.DisplayMember = "DatabaseName";
            this.listBoxDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxDatabase.FormattingEnabled = true;
            this.listBoxDatabase.IntegralHeight = false;
            this.listBoxDatabase.Location = new System.Drawing.Point(0, 0);
            this.listBoxDatabase.Name = "listBoxDatabase";
            this.listBoxDatabase.Size = new System.Drawing.Size(262, 299);
            this.listBoxDatabase.TabIndex = 0;
            this.listBoxDatabase.ValueMember = "Server";
            this.listBoxDatabase.SelectedIndexChanged += new System.EventHandler(this.listBoxDatabase_SelectedIndexChanged);
            // 
            // cacheDatabaseBindingSource
            // 
            this.cacheDatabaseBindingSource.DataMember = "CacheDatabase";
            this.cacheDatabaseBindingSource.DataSource = this.dataSetCacheDB;
            // 
            // dataSetCacheDB
            // 
            this.dataSetCacheDB.DataSetName = "DataSetCacheDB";
            this.dataSetCacheDB.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // toolStripDatabase
            // 
            this.toolStripDatabase.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripDatabase.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripDatabase.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonDatabaseNew,
            this.toolStripButtonDatabaseDelete});
            this.toolStripDatabase.Location = new System.Drawing.Point(0, 299);
            this.toolStripDatabase.Name = "toolStripDatabase";
            this.toolStripDatabase.Size = new System.Drawing.Size(262, 25);
            this.toolStripDatabase.TabIndex = 1;
            this.toolStripDatabase.Text = "toolStrip1";
            // 
            // toolStripButtonDatabaseNew
            // 
            this.toolStripButtonDatabaseNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDatabaseNew.Image = global::DiversityCollection.Resource.Database;
            this.toolStripButtonDatabaseNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDatabaseNew.Name = "toolStripButtonDatabaseNew";
            this.toolStripButtonDatabaseNew.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDatabaseNew.Text = "Create a new cache database";
            this.toolStripButtonDatabaseNew.Click += new System.EventHandler(this.toolStripButtonDatabaseNew_Click);
            // 
            // toolStripButtonDatabaseDelete
            // 
            this.toolStripButtonDatabaseDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDatabaseDelete.Image = global::DiversityCollection.Resource.NoDatabase;
            this.toolStripButtonDatabaseDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDatabaseDelete.Name = "toolStripButtonDatabaseDelete";
            this.toolStripButtonDatabaseDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDatabaseDelete.Text = "Delete the selected cache database";
            this.toolStripButtonDatabaseDelete.Click += new System.EventHandler(this.toolStripButtonDatabaseDelete_Click);
            // 
            // tableLayoutPanelDatabase
            // 
            this.tableLayoutPanelDatabase.ColumnCount = 3;
            this.tableLayoutPanelDatabase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDatabase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDatabase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDatabase.Controls.Add(this.labelDatabaseName, 0, 2);
            this.tableLayoutPanelDatabase.Controls.Add(this.labelServer, 0, 0);
            this.tableLayoutPanelDatabase.Controls.Add(this.textBoxServer, 1, 0);
            this.tableLayoutPanelDatabase.Controls.Add(this.labelPort, 0, 1);
            this.tableLayoutPanelDatabase.Controls.Add(this.textBoxPort, 1, 1);
            this.tableLayoutPanelDatabase.Controls.Add(this.labelProjectsDatabase, 0, 4);
            this.tableLayoutPanelDatabase.Controls.Add(this.textBoxDatabaseName, 1, 2);
            this.tableLayoutPanelDatabase.Controls.Add(this.buttonLoginAdministration, 0, 5);
            this.tableLayoutPanelDatabase.Controls.Add(this.buttonUpdateDatabase, 0, 6);
            this.tableLayoutPanelDatabase.Controls.Add(this.textBoxProjectsDatabase, 1, 4);
            this.tableLayoutPanelDatabase.Controls.Add(this.textBoxCurrentDatabaseVersion, 1, 3);
            this.tableLayoutPanelDatabase.Controls.Add(this.labelCurrentDatabaseVersion, 0, 3);
            this.tableLayoutPanelDatabase.Controls.Add(this.textBoxAvailableDatabaseVersion, 1, 6);
            this.tableLayoutPanelDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDatabase.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelDatabase.Name = "tableLayoutPanelDatabase";
            this.tableLayoutPanelDatabase.RowCount = 9;
            this.tableLayoutPanelDatabase.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDatabase.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDatabase.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDatabase.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDatabase.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDatabase.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDatabase.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDatabase.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDatabase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDatabase.Size = new System.Drawing.Size(524, 324);
            this.tableLayoutPanelDatabase.TabIndex = 0;
            // 
            // labelDatabaseName
            // 
            this.labelDatabaseName.AutoSize = true;
            this.labelDatabaseName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDatabaseName.Location = new System.Drawing.Point(3, 52);
            this.labelDatabaseName.Name = "labelDatabaseName";
            this.labelDatabaseName.Size = new System.Drawing.Size(163, 26);
            this.labelDatabaseName.TabIndex = 0;
            this.labelDatabaseName.Text = "Cache database:";
            this.labelDatabaseName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelServer
            // 
            this.labelServer.AutoSize = true;
            this.labelServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelServer.Location = new System.Drawing.Point(3, 0);
            this.labelServer.Name = "labelServer";
            this.labelServer.Size = new System.Drawing.Size(163, 26);
            this.labelServer.TabIndex = 2;
            this.labelServer.Text = "Server for the cache database:";
            this.labelServer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxServer
            // 
            this.tableLayoutPanelDatabase.SetColumnSpan(this.textBoxServer, 2);
            this.textBoxServer.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.cacheDatabaseBindingSource, "Server", true));
            this.textBoxServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxServer.Location = new System.Drawing.Point(172, 3);
            this.textBoxServer.Name = "textBoxServer";
            this.textBoxServer.ReadOnly = true;
            this.textBoxServer.Size = new System.Drawing.Size(349, 20);
            this.textBoxServer.TabIndex = 3;
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPort.Location = new System.Drawing.Point(3, 26);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(163, 26);
            this.labelPort.TabIndex = 4;
            this.labelPort.Text = "Port used by the server:";
            this.labelPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPort
            // 
            this.tableLayoutPanelDatabase.SetColumnSpan(this.textBoxPort, 2);
            this.textBoxPort.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.cacheDatabaseBindingSource, "Port", true));
            this.textBoxPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPort.Location = new System.Drawing.Point(172, 29);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.ReadOnly = true;
            this.textBoxPort.Size = new System.Drawing.Size(349, 20);
            this.textBoxPort.TabIndex = 5;
            // 
            // labelProjectsDatabase
            // 
            this.labelProjectsDatabase.AutoSize = true;
            this.labelProjectsDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProjectsDatabase.Location = new System.Drawing.Point(3, 104);
            this.labelProjectsDatabase.Name = "labelProjectsDatabase";
            this.labelProjectsDatabase.Size = new System.Drawing.Size(163, 26);
            this.labelProjectsDatabase.TabIndex = 8;
            this.labelProjectsDatabase.Text = "Database with project definitions:";
            this.labelProjectsDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxDatabaseName
            // 
            this.tableLayoutPanelDatabase.SetColumnSpan(this.textBoxDatabaseName, 2);
            this.textBoxDatabaseName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.cacheDatabaseBindingSource, "DatabaseName", true));
            this.textBoxDatabaseName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDatabaseName.Location = new System.Drawing.Point(172, 55);
            this.textBoxDatabaseName.Name = "textBoxDatabaseName";
            this.textBoxDatabaseName.ReadOnly = true;
            this.textBoxDatabaseName.Size = new System.Drawing.Size(349, 20);
            this.textBoxDatabaseName.TabIndex = 1;
            // 
            // buttonLoginAdministration
            // 
            this.buttonLoginAdministration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLoginAdministration.Image = global::DiversityCollection.Resource.Agent;
            this.buttonLoginAdministration.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonLoginAdministration.Location = new System.Drawing.Point(3, 133);
            this.buttonLoginAdministration.Name = "buttonLoginAdministration";
            this.buttonLoginAdministration.Size = new System.Drawing.Size(163, 24);
            this.buttonLoginAdministration.TabIndex = 10;
            this.buttonLoginAdministration.Text = "Login administration";
            this.buttonLoginAdministration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonLoginAdministration.UseVisualStyleBackColor = true;
            this.buttonLoginAdministration.Click += new System.EventHandler(this.buttonLoginAdministration_Click);
            // 
            // buttonUpdateDatabase
            // 
            this.buttonUpdateDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonUpdateDatabase.Image = global::DiversityCollection.Resource.UpdateDatabase;
            this.buttonUpdateDatabase.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonUpdateDatabase.Location = new System.Drawing.Point(3, 163);
            this.buttonUpdateDatabase.Name = "buttonUpdateDatabase";
            this.buttonUpdateDatabase.Size = new System.Drawing.Size(163, 24);
            this.buttonUpdateDatabase.TabIndex = 11;
            this.buttonUpdateDatabase.Text = "Update database";
            this.buttonUpdateDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonUpdateDatabase.UseVisualStyleBackColor = true;
            this.buttonUpdateDatabase.Click += new System.EventHandler(this.buttonUpdateDatabase_Click);
            // 
            // textBoxProjectsDatabase
            // 
            this.tableLayoutPanelDatabase.SetColumnSpan(this.textBoxProjectsDatabase, 2);
            this.textBoxProjectsDatabase.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.cacheDatabaseBindingSource, "ProjectsDatabaseName", true));
            this.textBoxProjectsDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProjectsDatabase.Location = new System.Drawing.Point(172, 107);
            this.textBoxProjectsDatabase.Name = "textBoxProjectsDatabase";
            this.textBoxProjectsDatabase.ReadOnly = true;
            this.textBoxProjectsDatabase.Size = new System.Drawing.Size(349, 20);
            this.textBoxProjectsDatabase.TabIndex = 12;
            // 
            // textBoxCurrentDatabaseVersion
            // 
            this.tableLayoutPanelDatabase.SetColumnSpan(this.textBoxCurrentDatabaseVersion, 2);
            this.textBoxCurrentDatabaseVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCurrentDatabaseVersion.Location = new System.Drawing.Point(172, 81);
            this.textBoxCurrentDatabaseVersion.Name = "textBoxCurrentDatabaseVersion";
            this.textBoxCurrentDatabaseVersion.ReadOnly = true;
            this.textBoxCurrentDatabaseVersion.Size = new System.Drawing.Size(349, 20);
            this.textBoxCurrentDatabaseVersion.TabIndex = 13;
            this.textBoxCurrentDatabaseVersion.Visible = false;
            // 
            // labelCurrentDatabaseVersion
            // 
            this.labelCurrentDatabaseVersion.AutoSize = true;
            this.labelCurrentDatabaseVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCurrentDatabaseVersion.Location = new System.Drawing.Point(3, 78);
            this.labelCurrentDatabaseVersion.Name = "labelCurrentDatabaseVersion";
            this.labelCurrentDatabaseVersion.Size = new System.Drawing.Size(163, 26);
            this.labelCurrentDatabaseVersion.TabIndex = 14;
            this.labelCurrentDatabaseVersion.Text = "Current database version:";
            this.labelCurrentDatabaseVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelCurrentDatabaseVersion.Visible = false;
            // 
            // textBoxAvailableDatabaseVersion
            // 
            this.tableLayoutPanelDatabase.SetColumnSpan(this.textBoxAvailableDatabaseVersion, 2);
            this.textBoxAvailableDatabaseVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAvailableDatabaseVersion.Location = new System.Drawing.Point(172, 163);
            this.textBoxAvailableDatabaseVersion.Name = "textBoxAvailableDatabaseVersion";
            this.textBoxAvailableDatabaseVersion.ReadOnly = true;
            this.textBoxAvailableDatabaseVersion.Size = new System.Drawing.Size(349, 20);
            this.textBoxAvailableDatabaseVersion.TabIndex = 15;
            // 
            // tabPageProjects
            // 
            this.tabPageProjects.Controls.Add(this.splitContainerProjects);
            this.tabPageProjects.ImageIndex = 2;
            this.tabPageProjects.Location = new System.Drawing.Point(4, 23);
            this.tabPageProjects.Name = "tabPageProjects";
            this.tabPageProjects.Size = new System.Drawing.Size(796, 330);
            this.tabPageProjects.TabIndex = 2;
            this.tabPageProjects.Text = "Projects";
            this.tabPageProjects.UseVisualStyleBackColor = true;
            // 
            // splitContainerProjects
            // 
            this.splitContainerProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerProjects.Location = new System.Drawing.Point(0, 0);
            this.splitContainerProjects.Name = "splitContainerProjects";
            // 
            // splitContainerProjects.Panel1
            // 
            this.splitContainerProjects.Panel1.Controls.Add(this.listBoxProjects);
            this.splitContainerProjects.Panel1.Controls.Add(this.toolStripProjects);
            // 
            // splitContainerProjects.Panel2
            // 
            this.splitContainerProjects.Panel2.Controls.Add(this.tabControlProject);
            this.splitContainerProjects.Size = new System.Drawing.Size(796, 330);
            this.splitContainerProjects.SplitterDistance = 205;
            this.splitContainerProjects.TabIndex = 0;
            // 
            // listBoxProjects
            // 
            this.listBoxProjects.DataSource = this.projectProxyBindingSource;
            this.listBoxProjects.DisplayMember = "Project";
            this.listBoxProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjects.FormattingEnabled = true;
            this.listBoxProjects.Location = new System.Drawing.Point(0, 0);
            this.listBoxProjects.Name = "listBoxProjects";
            this.listBoxProjects.Size = new System.Drawing.Size(205, 305);
            this.listBoxProjects.TabIndex = 1;
            this.listBoxProjects.ValueMember = "ProjectID";
            this.listBoxProjects.SelectedIndexChanged += new System.EventHandler(this.listBoxProjects_SelectedIndexChanged);
            // 
            // projectProxyBindingSource
            // 
            this.projectProxyBindingSource.DataMember = "ProjectProxy";
            this.projectProxyBindingSource.DataSource = this.dataSetCacheDB;
            // 
            // toolStripProjects
            // 
            this.toolStripProjects.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripProjects.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripProjects.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonProjectAdd,
            this.toolStripButtonProjectRemove,
            this.toolStripButtonProjectSave});
            this.toolStripProjects.Location = new System.Drawing.Point(0, 305);
            this.toolStripProjects.Name = "toolStripProjects";
            this.toolStripProjects.Size = new System.Drawing.Size(205, 25);
            this.toolStripProjects.TabIndex = 0;
            this.toolStripProjects.Text = "toolStrip1";
            // 
            // toolStripButtonProjectAdd
            // 
            this.toolStripButtonProjectAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonProjectAdd.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonProjectAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonProjectAdd.Name = "toolStripButtonProjectAdd";
            this.toolStripButtonProjectAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonProjectAdd.Text = "Add a new project";
            this.toolStripButtonProjectAdd.Click += new System.EventHandler(this.toolStripButtonProjectAdd_Click);
            // 
            // toolStripButtonProjectRemove
            // 
            this.toolStripButtonProjectRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonProjectRemove.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonProjectRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonProjectRemove.Name = "toolStripButtonProjectRemove";
            this.toolStripButtonProjectRemove.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonProjectRemove.Text = "Delete the selected project";
            this.toolStripButtonProjectRemove.Click += new System.EventHandler(this.toolStripButtonProjectRemove_Click);
            // 
            // toolStripButtonProjectSave
            // 
            this.toolStripButtonProjectSave.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonProjectSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonProjectSave.Enabled = false;
            this.toolStripButtonProjectSave.Image = global::DiversityCollection.Resource.Save;
            this.toolStripButtonProjectSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonProjectSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonProjectSave.Name = "toolStripButtonProjectSave";
            this.toolStripButtonProjectSave.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonProjectSave.Text = "Save changes in taxonomic groups";
            this.toolStripButtonProjectSave.Click += new System.EventHandler(this.toolStripButtonProjectSave_Click);
            // 
            // tabControlProject
            // 
            this.tabControlProject.Controls.Add(this.tabPageGeneral);
            this.tabControlProject.Controls.Add(this.tabPageTaxonomicGroups);
            this.tabControlProject.Controls.Add(this.tabPageMaterialCategories);
            this.tabControlProject.Controls.Add(this.tabPageLocalisations);
            this.tabControlProject.Controls.Add(this.tabPageImages);
            this.tabControlProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlProject.ImageList = this.imageListTabControl;
            this.tabControlProject.Location = new System.Drawing.Point(0, 0);
            this.tabControlProject.Name = "tabControlProject";
            this.tabControlProject.SelectedIndex = 0;
            this.tabControlProject.Size = new System.Drawing.Size(587, 330);
            this.tabControlProject.TabIndex = 4;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.tableLayoutPanelGeneralProjectSettings);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 23);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(579, 303);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General settings";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelGeneralProjectSettings
            // 
            this.tableLayoutPanelGeneralProjectSettings.ColumnCount = 4;
            this.tableLayoutPanelGeneralProjectSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeneralProjectSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeneralProjectSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelGeneralProjectSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeneralProjectSettings.Controls.Add(this.labelProjectDataType, 0, 0);
            this.tableLayoutPanelGeneralProjectSettings.Controls.Add(this.comboBoxProjectDataType, 1, 0);
            this.tableLayoutPanelGeneralProjectSettings.Controls.Add(this.checkBoxCoordinatePrecision, 0, 4);
            this.tableLayoutPanelGeneralProjectSettings.Controls.Add(this.numericUpDownCoordinatePrecision, 1, 4);
            this.tableLayoutPanelGeneralProjectSettings.Controls.Add(this.checkBoxRestrictTaxonomicGroups, 0, 1);
            this.tableLayoutPanelGeneralProjectSettings.Controls.Add(this.checkBoxRestrictMaterialCategories, 0, 2);
            this.tableLayoutPanelGeneralProjectSettings.Controls.Add(this.checkBoxRestrictLocalisations, 0, 3);
            this.tableLayoutPanelGeneralProjectSettings.Controls.Add(this.buttonShowProjectDataTypeDescription, 3, 0);
            this.tableLayoutPanelGeneralProjectSettings.Controls.Add(this.checkBoxRestrictImages, 0, 5);
            this.tableLayoutPanelGeneralProjectSettings.Controls.Add(this.labelCoordinatePrecision, 2, 4);
            this.tableLayoutPanelGeneralProjectSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelGeneralProjectSettings.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelGeneralProjectSettings.Name = "tableLayoutPanelGeneralProjectSettings";
            this.tableLayoutPanelGeneralProjectSettings.RowCount = 7;
            this.tableLayoutPanelGeneralProjectSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGeneralProjectSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGeneralProjectSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGeneralProjectSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGeneralProjectSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGeneralProjectSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGeneralProjectSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelGeneralProjectSettings.Size = new System.Drawing.Size(573, 297);
            this.tableLayoutPanelGeneralProjectSettings.TabIndex = 0;
            // 
            // labelProjectDataType
            // 
            this.labelProjectDataType.AutoSize = true;
            this.labelProjectDataType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProjectDataType.Location = new System.Drawing.Point(3, 6);
            this.labelProjectDataType.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelProjectDataType.Name = "labelProjectDataType";
            this.labelProjectDataType.Size = new System.Drawing.Size(175, 21);
            this.labelProjectDataType.TabIndex = 2;
            this.labelProjectDataType.Text = "Data type for transfer:";
            this.labelProjectDataType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelProjectDataType.Visible = false;
            // 
            // comboBoxProjectDataType
            // 
            this.tableLayoutPanelGeneralProjectSettings.SetColumnSpan(this.comboBoxProjectDataType, 2);
            this.comboBoxProjectDataType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.projectProxyBindingSource, "DataType", true));
            this.comboBoxProjectDataType.FormattingEnabled = true;
            this.comboBoxProjectDataType.Location = new System.Drawing.Point(184, 3);
            this.comboBoxProjectDataType.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.comboBoxProjectDataType.Name = "comboBoxProjectDataType";
            this.comboBoxProjectDataType.Size = new System.Drawing.Size(234, 21);
            this.comboBoxProjectDataType.TabIndex = 3;
            this.comboBoxProjectDataType.Visible = false;
            this.comboBoxProjectDataType.SelectionChangeCommitted += new System.EventHandler(this.comboBoxProjectDataType_SelectionChangeCommitted);
            // 
            // checkBoxCoordinatePrecision
            // 
            this.checkBoxCoordinatePrecision.AutoSize = true;
            this.checkBoxCoordinatePrecision.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxCoordinatePrecision.Location = new System.Drawing.Point(3, 99);
            this.checkBoxCoordinatePrecision.Name = "checkBoxCoordinatePrecision";
            this.checkBoxCoordinatePrecision.Size = new System.Drawing.Size(175, 20);
            this.checkBoxCoordinatePrecision.TabIndex = 4;
            this.checkBoxCoordinatePrecision.Text = "Restrict coordinate precision to:";
            this.checkBoxCoordinatePrecision.UseVisualStyleBackColor = true;
            this.checkBoxCoordinatePrecision.Click += new System.EventHandler(this.checkBoxCoordinatePrecision_Click);
            // 
            // numericUpDownCoordinatePrecision
            // 
            this.numericUpDownCoordinatePrecision.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.projectProxyBindingSource, "CoordinatePrecision", true));
            this.numericUpDownCoordinatePrecision.Dock = System.Windows.Forms.DockStyle.Left;
            this.numericUpDownCoordinatePrecision.Location = new System.Drawing.Point(184, 99);
            this.numericUpDownCoordinatePrecision.Name = "numericUpDownCoordinatePrecision";
            this.numericUpDownCoordinatePrecision.Size = new System.Drawing.Size(26, 20);
            this.numericUpDownCoordinatePrecision.TabIndex = 5;
            this.numericUpDownCoordinatePrecision.Click += new System.EventHandler(this.numericUpDownCoordinatePrecision_Click);
            // 
            // checkBoxRestrictTaxonomicGroups
            // 
            this.checkBoxRestrictTaxonomicGroups.AutoSize = true;
            this.checkBoxRestrictTaxonomicGroups.Location = new System.Drawing.Point(3, 30);
            this.checkBoxRestrictTaxonomicGroups.Name = "checkBoxRestrictTaxonomicGroups";
            this.checkBoxRestrictTaxonomicGroups.Size = new System.Drawing.Size(148, 17);
            this.checkBoxRestrictTaxonomicGroups.TabIndex = 6;
            this.checkBoxRestrictTaxonomicGroups.Text = "Restrict taxonomic groups";
            this.checkBoxRestrictTaxonomicGroups.UseVisualStyleBackColor = true;
            this.checkBoxRestrictTaxonomicGroups.Click += new System.EventHandler(this.checkBoxRestrictTaxonomicGroups_Click);
            // 
            // checkBoxRestrictMaterialCategories
            // 
            this.checkBoxRestrictMaterialCategories.AutoSize = true;
            this.checkBoxRestrictMaterialCategories.Location = new System.Drawing.Point(3, 53);
            this.checkBoxRestrictMaterialCategories.Name = "checkBoxRestrictMaterialCategories";
            this.checkBoxRestrictMaterialCategories.Size = new System.Drawing.Size(153, 17);
            this.checkBoxRestrictMaterialCategories.TabIndex = 7;
            this.checkBoxRestrictMaterialCategories.Text = "Restrict material categories";
            this.checkBoxRestrictMaterialCategories.UseVisualStyleBackColor = true;
            this.checkBoxRestrictMaterialCategories.Click += new System.EventHandler(this.checkBoxRestrictMaterialCategories_Click);
            // 
            // checkBoxRestrictLocalisations
            // 
            this.checkBoxRestrictLocalisations.AutoSize = true;
            this.checkBoxRestrictLocalisations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxRestrictLocalisations.Location = new System.Drawing.Point(3, 76);
            this.checkBoxRestrictLocalisations.Name = "checkBoxRestrictLocalisations";
            this.checkBoxRestrictLocalisations.Size = new System.Drawing.Size(175, 17);
            this.checkBoxRestrictLocalisations.TabIndex = 8;
            this.checkBoxRestrictLocalisations.Text = "Restrict localisation systems";
            this.checkBoxRestrictLocalisations.UseVisualStyleBackColor = true;
            this.checkBoxRestrictLocalisations.Click += new System.EventHandler(this.checkBoxRestrictLocalisations_Click);
            // 
            // buttonShowProjectDataTypeDescription
            // 
            this.buttonShowProjectDataTypeDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShowProjectDataTypeDescription.Image = global::DiversityCollection.Resource.Manual;
            this.buttonShowProjectDataTypeDescription.Location = new System.Drawing.Point(541, 1);
            this.buttonShowProjectDataTypeDescription.Margin = new System.Windows.Forms.Padding(0, 1, 3, 0);
            this.buttonShowProjectDataTypeDescription.Name = "buttonShowProjectDataTypeDescription";
            this.buttonShowProjectDataTypeDescription.Size = new System.Drawing.Size(29, 26);
            this.buttonShowProjectDataTypeDescription.TabIndex = 9;
            this.buttonShowProjectDataTypeDescription.UseVisualStyleBackColor = true;
            this.buttonShowProjectDataTypeDescription.Visible = false;
            this.buttonShowProjectDataTypeDescription.Click += new System.EventHandler(this.buttonShowProjectDataTypeDescription_Click);
            // 
            // checkBoxRestrictImages
            // 
            this.checkBoxRestrictImages.AutoSize = true;
            this.checkBoxRestrictImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxRestrictImages.Location = new System.Drawing.Point(3, 125);
            this.checkBoxRestrictImages.Name = "checkBoxRestrictImages";
            this.checkBoxRestrictImages.Size = new System.Drawing.Size(175, 17);
            this.checkBoxRestrictImages.TabIndex = 10;
            this.checkBoxRestrictImages.Text = "Restrict images";
            this.checkBoxRestrictImages.UseVisualStyleBackColor = true;
            this.checkBoxRestrictImages.Click += new System.EventHandler(this.checkBoxRestrictImages_Click);
            // 
            // labelCoordinatePrecision
            // 
            this.labelCoordinatePrecision.AutoSize = true;
            this.tableLayoutPanelGeneralProjectSettings.SetColumnSpan(this.labelCoordinatePrecision, 2);
            this.labelCoordinatePrecision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCoordinatePrecision.Location = new System.Drawing.Point(216, 96);
            this.labelCoordinatePrecision.Name = "labelCoordinatePrecision";
            this.labelCoordinatePrecision.Size = new System.Drawing.Size(354, 26);
            this.labelCoordinatePrecision.TabIndex = 11;
            this.labelCoordinatePrecision.Text = "decimal places";
            this.labelCoordinatePrecision.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageTaxonomicGroups
            // 
            this.tabPageTaxonomicGroups.Controls.Add(this.tableLayoutPanelTaxonomicGroups);
            this.tabPageTaxonomicGroups.ImageIndex = 6;
            this.tabPageTaxonomicGroups.Location = new System.Drawing.Point(4, 23);
            this.tabPageTaxonomicGroups.Name = "tabPageTaxonomicGroups";
            this.tabPageTaxonomicGroups.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTaxonomicGroups.Size = new System.Drawing.Size(579, 303);
            this.tabPageTaxonomicGroups.TabIndex = 1;
            this.tabPageTaxonomicGroups.Text = "Taxonomic groups";
            this.tabPageTaxonomicGroups.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelTaxonomicGroups
            // 
            this.tableLayoutPanelTaxonomicGroups.ColumnCount = 2;
            this.tableLayoutPanelTaxonomicGroups.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaxonomicGroups.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTaxonomicGroups.Controls.Add(this.labelProjectTaxonomicGroup, 0, 0);
            this.tableLayoutPanelTaxonomicGroups.Controls.Add(this.buttonRequeryTaxonomicGroups, 1, 0);
            this.tableLayoutPanelTaxonomicGroups.Controls.Add(this.panelTaxonomicGroups, 0, 1);
            this.tableLayoutPanelTaxonomicGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTaxonomicGroups.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelTaxonomicGroups.Name = "tableLayoutPanelTaxonomicGroups";
            this.tableLayoutPanelTaxonomicGroups.RowCount = 2;
            this.tableLayoutPanelTaxonomicGroups.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTaxonomicGroups.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaxonomicGroups.Size = new System.Drawing.Size(573, 297);
            this.tableLayoutPanelTaxonomicGroups.TabIndex = 2;
            // 
            // labelProjectTaxonomicGroup
            // 
            this.labelProjectTaxonomicGroup.AutoSize = true;
            this.labelProjectTaxonomicGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProjectTaxonomicGroup.Location = new System.Drawing.Point(3, 3);
            this.labelProjectTaxonomicGroup.Margin = new System.Windows.Forms.Padding(3);
            this.labelProjectTaxonomicGroup.Name = "labelProjectTaxonomicGroup";
            this.labelProjectTaxonomicGroup.Size = new System.Drawing.Size(492, 18);
            this.labelProjectTaxonomicGroup.TabIndex = 1;
            this.labelProjectTaxonomicGroup.Text = "Taxonomic groups transfered into the cache database";
            this.labelProjectTaxonomicGroup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonRequeryTaxonomicGroups
            // 
            this.buttonRequeryTaxonomicGroups.Image = global::DiversityCollection.Resource.Transfrom;
            this.buttonRequeryTaxonomicGroups.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRequeryTaxonomicGroups.Location = new System.Drawing.Point(498, 0);
            this.buttonRequeryTaxonomicGroups.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRequeryTaxonomicGroups.Name = "buttonRequeryTaxonomicGroups";
            this.buttonRequeryTaxonomicGroups.Size = new System.Drawing.Size(75, 24);
            this.buttonRequeryTaxonomicGroups.TabIndex = 2;
            this.buttonRequeryTaxonomicGroups.Text = "Requery";
            this.buttonRequeryTaxonomicGroups.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRequeryTaxonomicGroups.UseVisualStyleBackColor = true;
            this.buttonRequeryTaxonomicGroups.Click += new System.EventHandler(this.buttonRequeryTaxonomicGroups_Click);
            // 
            // panelTaxonomicGroups
            // 
            this.panelTaxonomicGroups.AutoScroll = true;
            this.tableLayoutPanelTaxonomicGroups.SetColumnSpan(this.panelTaxonomicGroups, 2);
            this.panelTaxonomicGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTaxonomicGroups.Location = new System.Drawing.Point(3, 27);
            this.panelTaxonomicGroups.Name = "panelTaxonomicGroups";
            this.panelTaxonomicGroups.Size = new System.Drawing.Size(567, 267);
            this.panelTaxonomicGroups.TabIndex = 3;
            // 
            // tabPageMaterialCategories
            // 
            this.tabPageMaterialCategories.Controls.Add(this.tableLayoutPanelMaterialCategories);
            this.tabPageMaterialCategories.ImageIndex = 7;
            this.tabPageMaterialCategories.Location = new System.Drawing.Point(4, 23);
            this.tabPageMaterialCategories.Name = "tabPageMaterialCategories";
            this.tabPageMaterialCategories.Size = new System.Drawing.Size(579, 303);
            this.tabPageMaterialCategories.TabIndex = 2;
            this.tabPageMaterialCategories.Text = "Material categories";
            this.tabPageMaterialCategories.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelMaterialCategories
            // 
            this.tableLayoutPanelMaterialCategories.ColumnCount = 2;
            this.tableLayoutPanelMaterialCategories.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMaterialCategories.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMaterialCategories.Controls.Add(this.buttonRequeryMaterialCategories, 1, 0);
            this.tableLayoutPanelMaterialCategories.Controls.Add(this.labelMaterialCategories, 0, 0);
            this.tableLayoutPanelMaterialCategories.Controls.Add(this.panelMaterialCategories, 0, 1);
            this.tableLayoutPanelMaterialCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMaterialCategories.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMaterialCategories.Name = "tableLayoutPanelMaterialCategories";
            this.tableLayoutPanelMaterialCategories.RowCount = 2;
            this.tableLayoutPanelMaterialCategories.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMaterialCategories.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMaterialCategories.Size = new System.Drawing.Size(579, 303);
            this.tableLayoutPanelMaterialCategories.TabIndex = 4;
            // 
            // buttonRequeryMaterialCategories
            // 
            this.buttonRequeryMaterialCategories.Image = global::DiversityCollection.Resource.Transfrom;
            this.buttonRequeryMaterialCategories.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRequeryMaterialCategories.Location = new System.Drawing.Point(504, 0);
            this.buttonRequeryMaterialCategories.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRequeryMaterialCategories.Name = "buttonRequeryMaterialCategories";
            this.buttonRequeryMaterialCategories.Size = new System.Drawing.Size(75, 24);
            this.buttonRequeryMaterialCategories.TabIndex = 2;
            this.buttonRequeryMaterialCategories.Text = "Requery";
            this.buttonRequeryMaterialCategories.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRequeryMaterialCategories.UseVisualStyleBackColor = true;
            this.buttonRequeryMaterialCategories.Click += new System.EventHandler(this.buttonRequeryMaterialCategories_Click);
            // 
            // labelMaterialCategories
            // 
            this.labelMaterialCategories.AutoSize = true;
            this.labelMaterialCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaterialCategories.Location = new System.Drawing.Point(3, 3);
            this.labelMaterialCategories.Margin = new System.Windows.Forms.Padding(3);
            this.labelMaterialCategories.Name = "labelMaterialCategories";
            this.labelMaterialCategories.Size = new System.Drawing.Size(498, 18);
            this.labelMaterialCategories.TabIndex = 2;
            this.labelMaterialCategories.Text = "Material categories transfered into the cache database";
            this.labelMaterialCategories.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelMaterialCategories
            // 
            this.panelMaterialCategories.AutoScroll = true;
            this.tableLayoutPanelMaterialCategories.SetColumnSpan(this.panelMaterialCategories, 2);
            this.panelMaterialCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMaterialCategories.Location = new System.Drawing.Point(3, 27);
            this.panelMaterialCategories.Name = "panelMaterialCategories";
            this.panelMaterialCategories.Size = new System.Drawing.Size(573, 273);
            this.panelMaterialCategories.TabIndex = 3;
            // 
            // tabPageLocalisations
            // 
            this.tabPageLocalisations.Controls.Add(this.tableLayoutPanelLocalisations);
            this.tabPageLocalisations.ImageIndex = 8;
            this.tabPageLocalisations.Location = new System.Drawing.Point(4, 23);
            this.tabPageLocalisations.Name = "tabPageLocalisations";
            this.tabPageLocalisations.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLocalisations.Size = new System.Drawing.Size(579, 303);
            this.tabPageLocalisations.TabIndex = 3;
            this.tabPageLocalisations.Text = "Localisations";
            this.tabPageLocalisations.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelLocalisations
            // 
            this.tableLayoutPanelLocalisations.ColumnCount = 2;
            this.tableLayoutPanelLocalisations.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLocalisations.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLocalisations.Controls.Add(this.buttonRequeryLocalisations, 1, 0);
            this.tableLayoutPanelLocalisations.Controls.Add(this.labelLocalisations, 0, 0);
            this.tableLayoutPanelLocalisations.Controls.Add(this.panelLocalisations, 0, 1);
            this.tableLayoutPanelLocalisations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelLocalisations.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelLocalisations.Name = "tableLayoutPanelLocalisations";
            this.tableLayoutPanelLocalisations.RowCount = 2;
            this.tableLayoutPanelLocalisations.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLocalisations.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLocalisations.Size = new System.Drawing.Size(573, 297);
            this.tableLayoutPanelLocalisations.TabIndex = 5;
            // 
            // buttonRequeryLocalisations
            // 
            this.buttonRequeryLocalisations.Image = global::DiversityCollection.Resource.Transfrom;
            this.buttonRequeryLocalisations.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRequeryLocalisations.Location = new System.Drawing.Point(498, 0);
            this.buttonRequeryLocalisations.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRequeryLocalisations.Name = "buttonRequeryLocalisations";
            this.buttonRequeryLocalisations.Size = new System.Drawing.Size(75, 24);
            this.buttonRequeryLocalisations.TabIndex = 2;
            this.buttonRequeryLocalisations.Text = "Requery";
            this.buttonRequeryLocalisations.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRequeryLocalisations.UseVisualStyleBackColor = true;
            this.buttonRequeryLocalisations.Click += new System.EventHandler(this.buttonRequeryLocalisations_Click);
            // 
            // labelLocalisations
            // 
            this.labelLocalisations.AutoSize = true;
            this.labelLocalisations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLocalisations.Location = new System.Drawing.Point(3, 3);
            this.labelLocalisations.Margin = new System.Windows.Forms.Padding(3);
            this.labelLocalisations.Name = "labelLocalisations";
            this.labelLocalisations.Size = new System.Drawing.Size(492, 18);
            this.labelLocalisations.TabIndex = 3;
            this.labelLocalisations.Text = "Localisations transfered into the cache database";
            this.labelLocalisations.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelLocalisations
            // 
            this.panelLocalisations.AutoScroll = true;
            this.tableLayoutPanelLocalisations.SetColumnSpan(this.panelLocalisations, 2);
            this.panelLocalisations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLocalisations.Location = new System.Drawing.Point(3, 27);
            this.panelLocalisations.Name = "panelLocalisations";
            this.panelLocalisations.Size = new System.Drawing.Size(567, 267);
            this.panelLocalisations.TabIndex = 3;
            // 
            // tabPageImages
            // 
            this.tabPageImages.Controls.Add(this.tabControlImages);
            this.tabPageImages.ImageIndex = 9;
            this.tabPageImages.Location = new System.Drawing.Point(4, 23);
            this.tabPageImages.Name = "tabPageImages";
            this.tabPageImages.Size = new System.Drawing.Size(579, 303);
            this.tabPageImages.TabIndex = 4;
            this.tabPageImages.Text = "Images";
            this.tabPageImages.UseVisualStyleBackColor = true;
            // 
            // tabControlImages
            // 
            this.tabControlImages.Controls.Add(this.tabPageImagesSpecimen);
            this.tabControlImages.Controls.Add(this.tabPageImagesEvent);
            this.tabControlImages.Controls.Add(this.tabPageImagesSeries);
            this.tabControlImages.Controls.Add(this.tabPageZoomView);
            this.tabControlImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlImages.ImageList = this.imageListTabControl;
            this.tabControlImages.Location = new System.Drawing.Point(0, 0);
            this.tabControlImages.Name = "tabControlImages";
            this.tabControlImages.SelectedIndex = 0;
            this.tabControlImages.Size = new System.Drawing.Size(579, 303);
            this.tabControlImages.TabIndex = 0;
            // 
            // tabPageImagesSpecimen
            // 
            this.tabPageImagesSpecimen.Controls.Add(this.tableLayoutPanelImagesSpecimen);
            this.tabPageImagesSpecimen.ImageIndex = 10;
            this.tabPageImagesSpecimen.Location = new System.Drawing.Point(4, 23);
            this.tabPageImagesSpecimen.Name = "tabPageImagesSpecimen";
            this.tabPageImagesSpecimen.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageImagesSpecimen.Size = new System.Drawing.Size(571, 276);
            this.tabPageImagesSpecimen.TabIndex = 0;
            this.tabPageImagesSpecimen.Text = "Specimen";
            this.tabPageImagesSpecimen.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelImagesSpecimen
            // 
            this.tableLayoutPanelImagesSpecimen.ColumnCount = 2;
            this.tableLayoutPanelImagesSpecimen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelImagesSpecimen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelImagesSpecimen.Controls.Add(this.buttonRequeryImagesSpecimen, 1, 0);
            this.tableLayoutPanelImagesSpecimen.Controls.Add(this.labelImagesSpecimen, 0, 0);
            this.tableLayoutPanelImagesSpecimen.Controls.Add(this.panelImagesSpecimen, 0, 1);
            this.tableLayoutPanelImagesSpecimen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelImagesSpecimen.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelImagesSpecimen.Name = "tableLayoutPanelImagesSpecimen";
            this.tableLayoutPanelImagesSpecimen.RowCount = 2;
            this.tableLayoutPanelImagesSpecimen.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImagesSpecimen.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelImagesSpecimen.Size = new System.Drawing.Size(565, 270);
            this.tableLayoutPanelImagesSpecimen.TabIndex = 6;
            // 
            // buttonRequeryImagesSpecimen
            // 
            this.buttonRequeryImagesSpecimen.Image = global::DiversityCollection.Resource.Transfrom;
            this.buttonRequeryImagesSpecimen.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRequeryImagesSpecimen.Location = new System.Drawing.Point(490, 0);
            this.buttonRequeryImagesSpecimen.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRequeryImagesSpecimen.Name = "buttonRequeryImagesSpecimen";
            this.buttonRequeryImagesSpecimen.Size = new System.Drawing.Size(75, 24);
            this.buttonRequeryImagesSpecimen.TabIndex = 2;
            this.buttonRequeryImagesSpecimen.Text = "Requery";
            this.buttonRequeryImagesSpecimen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRequeryImagesSpecimen.UseVisualStyleBackColor = true;
            this.buttonRequeryImagesSpecimen.Click += new System.EventHandler(this.buttonRequeryImagesSpecimen_Click);
            // 
            // labelImagesSpecimen
            // 
            this.labelImagesSpecimen.AutoSize = true;
            this.labelImagesSpecimen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelImagesSpecimen.Location = new System.Drawing.Point(3, 3);
            this.labelImagesSpecimen.Margin = new System.Windows.Forms.Padding(3);
            this.labelImagesSpecimen.Name = "labelImagesSpecimen";
            this.labelImagesSpecimen.Size = new System.Drawing.Size(484, 18);
            this.labelImagesSpecimen.TabIndex = 3;
            this.labelImagesSpecimen.Text = "Speicmen images transfered into the cache database";
            this.labelImagesSpecimen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelImagesSpecimen
            // 
            this.panelImagesSpecimen.AutoScroll = true;
            this.tableLayoutPanelImagesSpecimen.SetColumnSpan(this.panelImagesSpecimen, 2);
            this.panelImagesSpecimen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImagesSpecimen.Location = new System.Drawing.Point(3, 27);
            this.panelImagesSpecimen.Name = "panelImagesSpecimen";
            this.panelImagesSpecimen.Size = new System.Drawing.Size(559, 240);
            this.panelImagesSpecimen.TabIndex = 3;
            // 
            // tabPageImagesEvent
            // 
            this.tabPageImagesEvent.Controls.Add(this.tableLayoutPanelImagesEvent);
            this.tabPageImagesEvent.ImageIndex = 11;
            this.tabPageImagesEvent.Location = new System.Drawing.Point(4, 23);
            this.tabPageImagesEvent.Name = "tabPageImagesEvent";
            this.tabPageImagesEvent.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageImagesEvent.Size = new System.Drawing.Size(571, 276);
            this.tabPageImagesEvent.TabIndex = 1;
            this.tabPageImagesEvent.Text = "Event";
            this.tabPageImagesEvent.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelImagesEvent
            // 
            this.tableLayoutPanelImagesEvent.ColumnCount = 2;
            this.tableLayoutPanelImagesEvent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelImagesEvent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelImagesEvent.Controls.Add(this.buttonRequeryImagesEvent, 1, 0);
            this.tableLayoutPanelImagesEvent.Controls.Add(this.labelImagesEvent, 0, 0);
            this.tableLayoutPanelImagesEvent.Controls.Add(this.panelImagesEvent, 0, 1);
            this.tableLayoutPanelImagesEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelImagesEvent.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelImagesEvent.Name = "tableLayoutPanelImagesEvent";
            this.tableLayoutPanelImagesEvent.RowCount = 2;
            this.tableLayoutPanelImagesEvent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImagesEvent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelImagesEvent.Size = new System.Drawing.Size(565, 270);
            this.tableLayoutPanelImagesEvent.TabIndex = 7;
            // 
            // buttonRequeryImagesEvent
            // 
            this.buttonRequeryImagesEvent.Image = global::DiversityCollection.Resource.Transfrom;
            this.buttonRequeryImagesEvent.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRequeryImagesEvent.Location = new System.Drawing.Point(490, 0);
            this.buttonRequeryImagesEvent.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRequeryImagesEvent.Name = "buttonRequeryImagesEvent";
            this.buttonRequeryImagesEvent.Size = new System.Drawing.Size(75, 24);
            this.buttonRequeryImagesEvent.TabIndex = 2;
            this.buttonRequeryImagesEvent.Text = "Requery";
            this.buttonRequeryImagesEvent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRequeryImagesEvent.UseVisualStyleBackColor = true;
            this.buttonRequeryImagesEvent.Click += new System.EventHandler(this.buttonRequeryImagesEvent_Click);
            // 
            // labelImagesEvent
            // 
            this.labelImagesEvent.AutoSize = true;
            this.labelImagesEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelImagesEvent.Location = new System.Drawing.Point(3, 3);
            this.labelImagesEvent.Margin = new System.Windows.Forms.Padding(3);
            this.labelImagesEvent.Name = "labelImagesEvent";
            this.labelImagesEvent.Size = new System.Drawing.Size(484, 18);
            this.labelImagesEvent.TabIndex = 3;
            this.labelImagesEvent.Text = "Images of the collection event transfered into the cache database";
            this.labelImagesEvent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelImagesEvent
            // 
            this.panelImagesEvent.AutoScroll = true;
            this.tableLayoutPanelImagesEvent.SetColumnSpan(this.panelImagesEvent, 2);
            this.panelImagesEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImagesEvent.Location = new System.Drawing.Point(3, 27);
            this.panelImagesEvent.Name = "panelImagesEvent";
            this.panelImagesEvent.Size = new System.Drawing.Size(559, 240);
            this.panelImagesEvent.TabIndex = 3;
            // 
            // tabPageImagesSeries
            // 
            this.tabPageImagesSeries.Controls.Add(this.tableLayoutPanelImagesSeries);
            this.tabPageImagesSeries.ImageIndex = 12;
            this.tabPageImagesSeries.Location = new System.Drawing.Point(4, 23);
            this.tabPageImagesSeries.Name = "tabPageImagesSeries";
            this.tabPageImagesSeries.Size = new System.Drawing.Size(571, 276);
            this.tabPageImagesSeries.TabIndex = 2;
            this.tabPageImagesSeries.Text = "Series";
            this.tabPageImagesSeries.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelImagesSeries
            // 
            this.tableLayoutPanelImagesSeries.ColumnCount = 2;
            this.tableLayoutPanelImagesSeries.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelImagesSeries.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelImagesSeries.Controls.Add(this.buttonRequeryImagesSeries, 1, 0);
            this.tableLayoutPanelImagesSeries.Controls.Add(this.labelImagesSeries, 0, 0);
            this.tableLayoutPanelImagesSeries.Controls.Add(this.panelImagesSeries, 0, 1);
            this.tableLayoutPanelImagesSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelImagesSeries.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelImagesSeries.Name = "tableLayoutPanelImagesSeries";
            this.tableLayoutPanelImagesSeries.RowCount = 2;
            this.tableLayoutPanelImagesSeries.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImagesSeries.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelImagesSeries.Size = new System.Drawing.Size(571, 276);
            this.tableLayoutPanelImagesSeries.TabIndex = 8;
            // 
            // buttonRequeryImagesSeries
            // 
            this.buttonRequeryImagesSeries.Image = global::DiversityCollection.Resource.Transfrom;
            this.buttonRequeryImagesSeries.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRequeryImagesSeries.Location = new System.Drawing.Point(496, 0);
            this.buttonRequeryImagesSeries.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRequeryImagesSeries.Name = "buttonRequeryImagesSeries";
            this.buttonRequeryImagesSeries.Size = new System.Drawing.Size(75, 24);
            this.buttonRequeryImagesSeries.TabIndex = 2;
            this.buttonRequeryImagesSeries.Text = "Requery";
            this.buttonRequeryImagesSeries.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRequeryImagesSeries.UseVisualStyleBackColor = true;
            this.buttonRequeryImagesSeries.Click += new System.EventHandler(this.buttonRequeryImagesSeries_Click);
            // 
            // labelImagesSeries
            // 
            this.labelImagesSeries.AutoSize = true;
            this.labelImagesSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelImagesSeries.Location = new System.Drawing.Point(3, 3);
            this.labelImagesSeries.Margin = new System.Windows.Forms.Padding(3);
            this.labelImagesSeries.Name = "labelImagesSeries";
            this.labelImagesSeries.Size = new System.Drawing.Size(490, 18);
            this.labelImagesSeries.TabIndex = 3;
            this.labelImagesSeries.Text = "Images of the event series transfered into the cache database";
            this.labelImagesSeries.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelImagesSeries
            // 
            this.panelImagesSeries.AutoScroll = true;
            this.tableLayoutPanelImagesSeries.SetColumnSpan(this.panelImagesSeries, 2);
            this.panelImagesSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImagesSeries.Location = new System.Drawing.Point(3, 27);
            this.panelImagesSeries.Name = "panelImagesSeries";
            this.panelImagesSeries.Size = new System.Drawing.Size(565, 246);
            this.panelImagesSeries.TabIndex = 3;
            // 
            // tabPageZoomView
            // 
            this.tabPageZoomView.ImageIndex = 13;
            this.tabPageZoomView.Location = new System.Drawing.Point(4, 23);
            this.tabPageZoomView.Name = "tabPageZoomView";
            this.tabPageZoomView.Size = new System.Drawing.Size(571, 276);
            this.tabPageZoomView.TabIndex = 3;
            this.tabPageZoomView.Text = "Zoom view";
            this.tabPageZoomView.UseVisualStyleBackColor = true;
            // 
            // imageListTabControl
            // 
            this.imageListTabControl.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTabControl.ImageStream")));
            this.imageListTabControl.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTabControl.Images.SetKeyName(0, "Database.ico");
            this.imageListTabControl.Images.SetKeyName(1, "UpdateDatabase.ico");
            this.imageListTabControl.Images.SetKeyName(2, "Project.ico");
            this.imageListTabControl.Images.SetKeyName(3, "Taxon.ico");
            this.imageListTabControl.Images.SetKeyName(4, "Agent.ico");
            this.imageListTabControl.Images.SetKeyName(5, "UndoAll.ico");
            this.imageListTabControl.Images.SetKeyName(6, "Plant.ico");
            this.imageListTabControl.Images.SetKeyName(7, "Specimen.ico");
            this.imageListTabControl.Images.SetKeyName(8, "Localisation.ico");
            this.imageListTabControl.Images.SetKeyName(9, "Icones.ico");
            this.imageListTabControl.Images.SetKeyName(10, "CollectionSpecimen.ico");
            this.imageListTabControl.Images.SetKeyName(11, "Event.ico");
            this.imageListTabControl.Images.SetKeyName(12, "EventSeries.ico");
            this.imageListTabControl.Images.SetKeyName(13, "Zoom.ico");
            // 
            // tabPageTaxonomy
            // 
            this.tabPageTaxonomy.Controls.Add(this.splitContainerTaxonomy);
            this.tabPageTaxonomy.ImageIndex = 3;
            this.tabPageTaxonomy.Location = new System.Drawing.Point(4, 23);
            this.tabPageTaxonomy.Name = "tabPageTaxonomy";
            this.tabPageTaxonomy.Size = new System.Drawing.Size(796, 330);
            this.tabPageTaxonomy.TabIndex = 3;
            this.tabPageTaxonomy.Text = "Taxonomy";
            this.tabPageTaxonomy.UseVisualStyleBackColor = true;
            // 
            // splitContainerTaxonomy
            // 
            this.splitContainerTaxonomy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTaxonomy.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTaxonomy.Name = "splitContainerTaxonomy";
            this.splitContainerTaxonomy.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerTaxonomy.Panel1
            // 
            this.splitContainerTaxonomy.Panel1.Controls.Add(this.panelTaxonomy);
            this.splitContainerTaxonomy.Panel1.Controls.Add(this.toolStripTaxonomy);
            // 
            // splitContainerTaxonomy.Panel2
            // 
            this.splitContainerTaxonomy.Panel2.Controls.Add(this.dataGridViewTaxonProject);
            this.splitContainerTaxonomy.Panel2.Controls.Add(this.toolStripTaxonomyProjects);
            this.splitContainerTaxonomy.Panel2.Controls.Add(this.labelTaxonomyProjects);
            this.splitContainerTaxonomy.Size = new System.Drawing.Size(796, 330);
            this.splitContainerTaxonomy.SplitterDistance = 212;
            this.splitContainerTaxonomy.TabIndex = 3;
            // 
            // panelTaxonomy
            // 
            this.panelTaxonomy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTaxonomy.Location = new System.Drawing.Point(0, 0);
            this.panelTaxonomy.Name = "panelTaxonomy";
            this.panelTaxonomy.Size = new System.Drawing.Size(772, 212);
            this.panelTaxonomy.TabIndex = 2;
            // 
            // toolStripTaxonomy
            // 
            this.toolStripTaxonomy.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStripTaxonomy.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripTaxonomy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonTaxonomyAdd});
            this.toolStripTaxonomy.Location = new System.Drawing.Point(772, 0);
            this.toolStripTaxonomy.Name = "toolStripTaxonomy";
            this.toolStripTaxonomy.Size = new System.Drawing.Size(24, 212);
            this.toolStripTaxonomy.TabIndex = 1;
            this.toolStripTaxonomy.Text = "toolStrip1";
            // 
            // toolStripButtonTaxonomyAdd
            // 
            this.toolStripButtonTaxonomyAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTaxonomyAdd.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonTaxonomyAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTaxonomyAdd.Name = "toolStripButtonTaxonomyAdd";
            this.toolStripButtonTaxonomyAdd.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonTaxonomyAdd.Text = "Add a new DiversityTaxonNames database as source";
            this.toolStripButtonTaxonomyAdd.Click += new System.EventHandler(this.toolStripButtonTaxonomyAdd_Click);
            // 
            // dataGridViewTaxonProject
            // 
            this.dataGridViewTaxonProject.AllowUserToAddRows = false;
            this.dataGridViewTaxonProject.AllowUserToDeleteRows = false;
            this.dataGridViewTaxonProject.AutoGenerateColumns = false;
            this.dataGridViewTaxonProject.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTaxonProject.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataSourceDataGridViewTextBoxColumn,
            this.databaseNameDataGridViewTextBoxColumn,
            this.projectIDDataGridViewTextBoxColumn,
            this.sequenceDataGridViewTextBoxColumn,
            this.projectDataGridViewTextBoxColumn});
            this.dataGridViewTaxonProject.DataSource = this.diversityTaxonNamesProjectSequenceBindingSource;
            this.dataGridViewTaxonProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTaxonProject.Location = new System.Drawing.Point(0, 33);
            this.dataGridViewTaxonProject.Name = "dataGridViewTaxonProject";
            this.dataGridViewTaxonProject.RowHeadersVisible = false;
            this.dataGridViewTaxonProject.Size = new System.Drawing.Size(772, 81);
            this.dataGridViewTaxonProject.TabIndex = 1;
            this.dataGridViewTaxonProject.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTaxonProject_RowLeave);
            // 
            // dataSourceDataGridViewTextBoxColumn
            // 
            this.dataSourceDataGridViewTextBoxColumn.DataPropertyName = "DataSource";
            this.dataSourceDataGridViewTextBoxColumn.HeaderText = "DataSource";
            this.dataSourceDataGridViewTextBoxColumn.Name = "dataSourceDataGridViewTextBoxColumn";
            this.dataSourceDataGridViewTextBoxColumn.Visible = false;
            // 
            // databaseNameDataGridViewTextBoxColumn
            // 
            this.databaseNameDataGridViewTextBoxColumn.DataPropertyName = "DatabaseName";
            this.databaseNameDataGridViewTextBoxColumn.HeaderText = "DatabaseName";
            this.databaseNameDataGridViewTextBoxColumn.Name = "databaseNameDataGridViewTextBoxColumn";
            this.databaseNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // projectIDDataGridViewTextBoxColumn
            // 
            this.projectIDDataGridViewTextBoxColumn.DataPropertyName = "ProjectID";
            this.projectIDDataGridViewTextBoxColumn.HeaderText = "ProjectID";
            this.projectIDDataGridViewTextBoxColumn.Name = "projectIDDataGridViewTextBoxColumn";
            this.projectIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // sequenceDataGridViewTextBoxColumn
            // 
            this.sequenceDataGridViewTextBoxColumn.DataPropertyName = "Sequence";
            this.sequenceDataGridViewTextBoxColumn.HeaderText = "Sequence";
            this.sequenceDataGridViewTextBoxColumn.Name = "sequenceDataGridViewTextBoxColumn";
            this.sequenceDataGridViewTextBoxColumn.Width = 70;
            // 
            // projectDataGridViewTextBoxColumn
            // 
            this.projectDataGridViewTextBoxColumn.DataPropertyName = "Project";
            this.projectDataGridViewTextBoxColumn.HeaderText = "Project";
            this.projectDataGridViewTextBoxColumn.Name = "projectDataGridViewTextBoxColumn";
            this.projectDataGridViewTextBoxColumn.ReadOnly = true;
            this.projectDataGridViewTextBoxColumn.Width = 500;
            // 
            // diversityTaxonNamesProjectSequenceBindingSource
            // 
            this.diversityTaxonNamesProjectSequenceBindingSource.DataMember = "DiversityTaxonNamesProjectSequence";
            this.diversityTaxonNamesProjectSequenceBindingSource.DataSource = this.dataSetCacheDB;
            // 
            // toolStripTaxonomyProjects
            // 
            this.toolStripTaxonomyProjects.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStripTaxonomyProjects.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripTaxonomyProjects.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonTaxonProjectAdd,
            this.toolStripButtonTaxonProjectRemove});
            this.toolStripTaxonomyProjects.Location = new System.Drawing.Point(772, 33);
            this.toolStripTaxonomyProjects.Name = "toolStripTaxonomyProjects";
            this.toolStripTaxonomyProjects.Size = new System.Drawing.Size(24, 81);
            this.toolStripTaxonomyProjects.TabIndex = 0;
            this.toolStripTaxonomyProjects.Text = "toolStrip1";
            // 
            // toolStripButtonTaxonProjectAdd
            // 
            this.toolStripButtonTaxonProjectAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTaxonProjectAdd.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonTaxonProjectAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTaxonProjectAdd.Name = "toolStripButtonTaxonProjectAdd";
            this.toolStripButtonTaxonProjectAdd.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonTaxonProjectAdd.Text = "Add a new project to the list of projects that should be used in this source";
            this.toolStripButtonTaxonProjectAdd.Click += new System.EventHandler(this.toolStripButtonTaxonProjectAdd_Click);
            // 
            // toolStripButtonTaxonProjectRemove
            // 
            this.toolStripButtonTaxonProjectRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTaxonProjectRemove.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonTaxonProjectRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTaxonProjectRemove.Name = "toolStripButtonTaxonProjectRemove";
            this.toolStripButtonTaxonProjectRemove.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonTaxonProjectRemove.Text = "Remove the selected project";
            this.toolStripButtonTaxonProjectRemove.Click += new System.EventHandler(this.toolStripButtonTaxonProjectRemove_Click);
            // 
            // labelTaxonomyProjects
            // 
            this.labelTaxonomyProjects.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTaxonomyProjects.Location = new System.Drawing.Point(0, 0);
            this.labelTaxonomyProjects.Name = "labelTaxonomyProjects";
            this.labelTaxonomyProjects.Size = new System.Drawing.Size(796, 33);
            this.labelTaxonomyProjects.TabIndex = 2;
            this.labelTaxonomyProjects.Text = "Projects and their sequence used for the datasource";
            this.labelTaxonomyProjects.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // projectProxyTableAdapter
            // 
            this.projectProxyTableAdapter.ClearBeforeFill = true;
            // 
            // diversityTaxonNamesProjectSequenceTableAdapter
            // 
            this.diversityTaxonNamesProjectSequenceTableAdapter.ClearBeforeFill = true;
            // 
            // cacheDatabaseTableAdapter
            // 
            this.cacheDatabaseTableAdapter.ClearBeforeFill = true;
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFeedback.Image = global::DiversityCollection.Resource.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(779, 0);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(25, 23);
            this.buttonFeedback.TabIndex = 2;
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // FormCacheDBAdministration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 357);
            this.Controls.Add(this.buttonFeedback);
            this.Controls.Add(this.tabControlMain);
            this.helpProvider.SetHelpKeyword(this, "Cache database configuration");
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCacheDBAdministration";
            this.helpProvider.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Administration of the cache database";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCacheDBAdministration_FormClosing);
            this.Load += new System.EventHandler(this.FormCacheDBAdministration_Load);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageChooseDB.ResumeLayout(false);
            this.splitContainerSelectDatabase.Panel1.ResumeLayout(false);
            this.splitContainerSelectDatabase.Panel1.PerformLayout();
            this.splitContainerSelectDatabase.Panel2.ResumeLayout(false);
            this.splitContainerSelectDatabase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cacheDatabaseBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCacheDB)).EndInit();
            this.toolStripDatabase.ResumeLayout(false);
            this.toolStripDatabase.PerformLayout();
            this.tableLayoutPanelDatabase.ResumeLayout(false);
            this.tableLayoutPanelDatabase.PerformLayout();
            this.tabPageProjects.ResumeLayout(false);
            this.splitContainerProjects.Panel1.ResumeLayout(false);
            this.splitContainerProjects.Panel1.PerformLayout();
            this.splitContainerProjects.Panel2.ResumeLayout(false);
            this.splitContainerProjects.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.projectProxyBindingSource)).EndInit();
            this.toolStripProjects.ResumeLayout(false);
            this.toolStripProjects.PerformLayout();
            this.tabControlProject.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tableLayoutPanelGeneralProjectSettings.ResumeLayout(false);
            this.tableLayoutPanelGeneralProjectSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCoordinatePrecision)).EndInit();
            this.tabPageTaxonomicGroups.ResumeLayout(false);
            this.tableLayoutPanelTaxonomicGroups.ResumeLayout(false);
            this.tableLayoutPanelTaxonomicGroups.PerformLayout();
            this.tabPageMaterialCategories.ResumeLayout(false);
            this.tableLayoutPanelMaterialCategories.ResumeLayout(false);
            this.tableLayoutPanelMaterialCategories.PerformLayout();
            this.tabPageLocalisations.ResumeLayout(false);
            this.tableLayoutPanelLocalisations.ResumeLayout(false);
            this.tableLayoutPanelLocalisations.PerformLayout();
            this.tabPageImages.ResumeLayout(false);
            this.tabControlImages.ResumeLayout(false);
            this.tabPageImagesSpecimen.ResumeLayout(false);
            this.tableLayoutPanelImagesSpecimen.ResumeLayout(false);
            this.tableLayoutPanelImagesSpecimen.PerformLayout();
            this.tabPageImagesEvent.ResumeLayout(false);
            this.tableLayoutPanelImagesEvent.ResumeLayout(false);
            this.tableLayoutPanelImagesEvent.PerformLayout();
            this.tabPageImagesSeries.ResumeLayout(false);
            this.tableLayoutPanelImagesSeries.ResumeLayout(false);
            this.tableLayoutPanelImagesSeries.PerformLayout();
            this.tabPageTaxonomy.ResumeLayout(false);
            this.splitContainerTaxonomy.Panel1.ResumeLayout(false);
            this.splitContainerTaxonomy.Panel1.PerformLayout();
            this.splitContainerTaxonomy.Panel2.ResumeLayout(false);
            this.splitContainerTaxonomy.Panel2.PerformLayout();
            this.splitContainerTaxonomy.ResumeLayout(false);
            this.toolStripTaxonomy.ResumeLayout(false);
            this.toolStripTaxonomy.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTaxonProject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.diversityTaxonNamesProjectSequenceBindingSource)).EndInit();
            this.toolStripTaxonomyProjects.ResumeLayout(false);
            this.toolStripTaxonomyProjects.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageChooseDB;
        private System.Windows.Forms.ImageList imageListTabControl;
        private System.Windows.Forms.TabPage tabPageProjects;
        private System.Windows.Forms.SplitContainer splitContainerProjects;
        private System.Windows.Forms.ListBox listBoxProjects;
        private System.Windows.Forms.ToolStrip toolStripProjects;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectRemove;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectSave;
        private System.Windows.Forms.Label labelProjectTaxonomicGroup;
        private System.Windows.Forms.TabPage tabPageTaxonomy;
        private System.Windows.Forms.ToolStrip toolStripTaxonomy;
        private System.Windows.Forms.ToolStripButton toolStripButtonTaxonomyAdd;
        private System.Windows.Forms.Panel panelTaxonomy;
        private CacheDatabase.DataSetCacheDB dataSetCacheDB;
        private System.Windows.Forms.BindingSource projectProxyBindingSource;
        private CacheDatabase.DataSetCacheDBTableAdapters.ProjectProxyTableAdapter projectProxyTableAdapter;
        private System.Windows.Forms.SplitContainer splitContainerTaxonomy;
        private System.Windows.Forms.DataGridView dataGridViewTaxonProject;
        private System.Windows.Forms.ToolStrip toolStripTaxonomyProjects;
        private System.Windows.Forms.ToolStripButton toolStripButtonTaxonProjectAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonTaxonProjectRemove;
        private System.Windows.Forms.BindingSource diversityTaxonNamesProjectSequenceBindingSource;
        private CacheDatabase.DataSetCacheDBTableAdapters.DiversityTaxonNamesProjectSequenceTableAdapter diversityTaxonNamesProjectSequenceTableAdapter;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelTaxonomyProjects;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataSourceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn databaseNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn projectIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sequenceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn projectDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label labelProjectDataType;
        private System.Windows.Forms.ComboBox comboBoxProjectDataType;
        private System.Windows.Forms.SplitContainer splitContainerSelectDatabase;
        private System.Windows.Forms.ToolStrip toolStripDatabase;
        private System.Windows.Forms.ToolStripButton toolStripButtonDatabaseNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonDatabaseDelete;
        private System.Windows.Forms.ListBox listBoxDatabase;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDatabase;
        private System.Windows.Forms.Label labelDatabaseName;
        private System.Windows.Forms.TextBox textBoxDatabaseName;
        private System.Windows.Forms.Label labelServer;
        private System.Windows.Forms.TextBox textBoxServer;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label labelProjectsDatabase;
        private System.Windows.Forms.Button buttonLoginAdministration;
        private System.Windows.Forms.Button buttonUpdateDatabase;
        private System.Windows.Forms.TabControl tabControlProject;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelGeneralProjectSettings;
        private System.Windows.Forms.CheckBox checkBoxCoordinatePrecision;
        private System.Windows.Forms.NumericUpDown numericUpDownCoordinatePrecision;
        private System.Windows.Forms.TabPage tabPageTaxonomicGroups;
        private System.Windows.Forms.TabPage tabPageMaterialCategories;
        private System.Windows.Forms.Label labelMaterialCategories;
        private System.Windows.Forms.CheckBox checkBoxRestrictTaxonomicGroups;
        private System.Windows.Forms.CheckBox checkBoxRestrictMaterialCategories;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.BindingSource cacheDatabaseBindingSource;
        private CacheDatabase.DataSetCacheDBTableAdapters.CacheDatabaseTableAdapter cacheDatabaseTableAdapter;
        private System.Windows.Forms.TextBox textBoxProjectsDatabase;
        private System.Windows.Forms.CheckBox checkBoxRestrictLocalisations;
        private System.Windows.Forms.TabPage tabPageLocalisations;
        private System.Windows.Forms.Label labelLocalisations;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTaxonomicGroups;
        private System.Windows.Forms.Button buttonRequeryTaxonomicGroups;
        private System.Windows.Forms.Panel panelTaxonomicGroups;
        private System.Windows.Forms.TextBox textBoxCurrentDatabaseVersion;
        private System.Windows.Forms.Label labelCurrentDatabaseVersion;
        private System.Windows.Forms.TextBox textBoxAvailableDatabaseVersion;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMaterialCategories;
        private System.Windows.Forms.Button buttonRequeryMaterialCategories;
        private System.Windows.Forms.Panel panelMaterialCategories;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLocalisations;
        private System.Windows.Forms.Button buttonRequeryLocalisations;
        private System.Windows.Forms.Panel panelLocalisations;
        private System.Windows.Forms.Button buttonShowProjectDataTypeDescription;
        private System.Windows.Forms.CheckBox checkBoxRestrictImages;
        private System.Windows.Forms.TabPage tabPageImages;
        private System.Windows.Forms.TabControl tabControlImages;
        private System.Windows.Forms.TabPage tabPageImagesSpecimen;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelImagesSpecimen;
        private System.Windows.Forms.Button buttonRequeryImagesSpecimen;
        private System.Windows.Forms.Label labelImagesSpecimen;
        private System.Windows.Forms.Panel panelImagesSpecimen;
        private System.Windows.Forms.TabPage tabPageImagesEvent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelImagesEvent;
        private System.Windows.Forms.Button buttonRequeryImagesEvent;
        private System.Windows.Forms.Label labelImagesEvent;
        private System.Windows.Forms.Panel panelImagesEvent;
        private System.Windows.Forms.TabPage tabPageImagesSeries;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelImagesSeries;
        private System.Windows.Forms.Button buttonRequeryImagesSeries;
        private System.Windows.Forms.Label labelImagesSeries;
        private System.Windows.Forms.Panel panelImagesSeries;
        private System.Windows.Forms.Label labelCoordinatePrecision;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.TabPage tabPageZoomView;
    }
}