namespace DiversityCollection.CacheDatabase
{
    partial class FormCacheDB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCacheDB));
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageTransfer = new System.Windows.Forms.TabPage();
            this.tabControlTransfer = new System.Windows.Forms.TabControl();
            this.tabPageDBtoCacheDB = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelTransferSteps = new System.Windows.Forms.TableLayoutPanel();
            this.panelTransferSteps = new System.Windows.Forms.Panel();
            this.buttonStartTransfer = new System.Windows.Forms.Button();
            this.labelWarningNoGazetteer = new System.Windows.Forms.Label();
            this.labelLastTransfer = new System.Windows.Forms.Label();
            this.tableLayoutPanelVersion1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonV1StartCollectionTransfer = new System.Windows.Forms.Button();
            this.buttonV1StartTaxonTransfer = new System.Windows.Forms.Button();
            this.listBoxV1ProjectPublished = new System.Windows.Forms.ListBox();
            this.projectPublishedBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetCacheDB_1 = new DiversityCollection.CacheDatabase.DataSetCacheDB_1();
            this.listBoxV1ProjectsUnpublished = new System.Windows.Forms.ListBox();
            this.buttonV1PublishProject = new System.Windows.Forms.Button();
            this.buttonV1UnpublishProject = new System.Windows.Forms.Button();
            this.labelProjectsUnpublished = new System.Windows.Forms.Label();
            this.labelProjectsPublished = new System.Windows.Forms.Label();
            this.labelV1LastSpecimenTransfer = new System.Windows.Forms.Label();
            this.labelV1CurrentSpecimenNumber = new System.Windows.Forms.Label();
            this.labelV1LastTaxonTransfer = new System.Windows.Forms.Label();
            this.labelV1CurrentTaxa = new System.Windows.Forms.Label();
            this.buttonV1CheckSpecimen = new System.Windows.Forms.Button();
            this.buttonV1CheckTaxa = new System.Windows.Forms.Button();
            this.buttonTransferCountryIcoCode = new System.Windows.Forms.Button();
            this.tabPageCacheDBToPostGres = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelPostgres = new System.Windows.Forms.TableLayoutPanel();
            this.listBoxPostgresTables = new System.Windows.Forms.ListBox();
            this.labelPostgresTables = new System.Windows.Forms.Label();
            this.groupBoxPostgresTable = new System.Windows.Forms.GroupBox();
            this.dataGridViewPostgresTable = new System.Windows.Forms.DataGridView();
            this.labelPostgresProject = new System.Windows.Forms.Label();
            this.buttonPostgresTransferProject = new System.Windows.Forms.Button();
            this.comboBoxPostgresProject = new System.Windows.Forms.ComboBox();
            this.buttonPostgresConnect = new System.Windows.Forms.Button();
            this.labelPostgresConnection = new System.Windows.Forms.Label();
            this.progressBarPostgresTransfer = new System.Windows.Forms.ProgressBar();
            this.labelPostgresTransferMessage = new System.Windows.Forms.Label();
            this.tabPageBioCASE = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelBioCASE = new System.Windows.Forms.TableLayoutPanel();
            this.labelBioCASEsources = new System.Windows.Forms.Label();
            this.textBoxBioCASEsources = new System.Windows.Forms.TextBox();
            this.buttonBioCASErefresh = new System.Windows.Forms.Button();
            this.webBrowserBioCASE = new System.Windows.Forms.WebBrowser();
            this.buttonBioCASEback = new System.Windows.Forms.Button();
            this.imageListTabControl = new System.Windows.Forms.ImageList(this.components);
            this.tabPageAdminCacheDB = new System.Windows.Forms.TabPage();
            this.tabControlDatabase = new System.Windows.Forms.TabControl();
            this.tabPageUpdate = new System.Windows.Forms.TabPage();
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
            this.buttonCheckProjectsDatabase = new System.Windows.Forms.Button();
            this.tabPageAnonymCollectors = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelAnonymCollector = new System.Windows.Forms.TableLayoutPanel();
            this.labelCollectorNotAnomym = new System.Windows.Forms.Label();
            this.labelCollectorAnonym = new System.Windows.Forms.Label();
            this.dataGridViewAnonymAgent = new System.Windows.Forms.DataGridView();
            this.listBoxAnonymCollector = new System.Windows.Forms.ListBox();
            this.buttonCollectorIsAnonym = new System.Windows.Forms.Button();
            this.buttonCollectorNotAnonym = new System.Windows.Forms.Button();
            this.tabPageProjects = new System.Windows.Forms.TabPage();
            this.splitContainerProjects = new System.Windows.Forms.SplitContainer();
            this.listBoxProjects = new System.Windows.Forms.ListBox();
            this.toolStripAdministrationProjects = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonProjectAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonProjectRemove = new System.Windows.Forms.ToolStripButton();
            this.tabControlProjectAdminTaxMat = new System.Windows.Forms.TabControl();
            this.tabPageProjectTaxonomicGroup = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelTaxonomicGroups = new System.Windows.Forms.TableLayoutPanel();
            this.labelProjectTaxonomicGroup = new System.Windows.Forms.Label();
            this.labelProjectTaxonomicGroupNotPublished = new System.Windows.Forms.Label();
            this.labelProjectTaxonomicGroupPublished = new System.Windows.Forms.Label();
            this.listBoxProjectTaxonomicGroupNotPublished = new System.Windows.Forms.ListBox();
            this.listBoxProjectTaxonomicGroupPublished = new System.Windows.Forms.ListBox();
            this.taxonomicGroupInProjectBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.buttonProjectTaxonomicGroupPublished = new System.Windows.Forms.Button();
            this.buttonProjectTaxonomicGroupNotPublished = new System.Windows.Forms.Button();
            this.buttonProjectTaxonomicGroupTransferExisting = new System.Windows.Forms.Button();
            this.tabPageProjectMaterialCategory = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelProjectMaterialCategory = new System.Windows.Forms.TableLayoutPanel();
            this.labelProjectMaterialCategoryHeader = new System.Windows.Forms.Label();
            this.labelProjectMaterialCategoryNotPublished = new System.Windows.Forms.Label();
            this.labelProjectMaterialCategoryPublished = new System.Windows.Forms.Label();
            this.listBoxProjectMaterialCategoryNotPublished = new System.Windows.Forms.ListBox();
            this.listBoxProjectMaterialCategoryPublished = new System.Windows.Forms.ListBox();
            this.buttonProjectMaterialCategoryPublishe = new System.Windows.Forms.Button();
            this.buttonProjectMaterialCategoryWithhold = new System.Windows.Forms.Button();
            this.buttonProjectMaterialCategoryTransferExisting = new System.Windows.Forms.Button();
            this.tabPageProjectLocalisationsystem = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelProjectLocalisation = new System.Windows.Forms.TableLayoutPanel();
            this.labelProjectLocalisationNotPublished = new System.Windows.Forms.Label();
            this.listBoxProjectLocalisationNotPublished = new System.Windows.Forms.ListBox();
            this.labelProjectLocalisationPublished = new System.Windows.Forms.Label();
            this.listBoxProjectLocalisationPubished = new System.Windows.Forms.ListBox();
            this.enumLocalisationSystemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.buttonProjectLocalisationPublish = new System.Windows.Forms.Button();
            this.buttonProjectLocalisationHide = new System.Windows.Forms.Button();
            this.buttonProjectLocalisationUp = new System.Windows.Forms.Button();
            this.buttonProjectLocalisationDown = new System.Windows.Forms.Button();
            this.labelProjectLocalisationHeader = new System.Windows.Forms.Label();
            this.groupBoxProjectSettings = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelProjectSettings = new System.Windows.Forms.TableLayoutPanel();
            this.buttonProjectSettings = new System.Windows.Forms.Button();
            this.buttonProjectRecordURI = new System.Windows.Forms.Button();
            this.groupBoxProjectCoordinates = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelLocalisations = new System.Windows.Forms.TableLayoutPanel();
            this.numericUpDownCoordinatePrecision = new System.Windows.Forms.NumericUpDown();
            this.checkBoxCoordinatePrecision = new System.Windows.Forms.CheckBox();
            this.labelCoordinatePrecision = new System.Windows.Forms.Label();
            this.pictureBoxProjectCoordiates = new System.Windows.Forms.PictureBox();
            this.groupBoxProjectImages = new System.Windows.Forms.GroupBox();
            this.checkBoxHighResolutionImage = new System.Windows.Forms.CheckBox();
            this.buttonHighResolutionImages = new System.Windows.Forms.Button();
            this.pictureBoxProjectImages = new System.Windows.Forms.PictureBox();
            this.tabPageLocalisation = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelLocalisation = new System.Windows.Forms.TableLayoutPanel();
            this.labelLocalisationNotPublished = new System.Windows.Forms.Label();
            this.listBoxLocalisationNotPublished = new System.Windows.Forms.ListBox();
            this.labelLocalisationPublished = new System.Windows.Forms.Label();
            this.listBoxLocalisationPublished = new System.Windows.Forms.ListBox();
            this.buttonLocalisationPublished = new System.Windows.Forms.Button();
            this.buttonLocalisationNotPublished = new System.Windows.Forms.Button();
            this.buttonLocalisationPublishedUp = new System.Windows.Forms.Button();
            this.buttonLocalisationPublishedDown = new System.Windows.Forms.Button();
            this.labelLocalisation = new System.Windows.Forms.Label();
            this.tabPageTaxonSynonymy = new System.Windows.Forms.TabPage();
            this.panelTaxonSources = new System.Windows.Forms.Panel();
            this.tableLayoutPanelTaxonomySources = new System.Windows.Forms.TableLayoutPanel();
            this.labelTaxonSourceDB = new System.Windows.Forms.Label();
            this.labelTaxonSourceView = new System.Windows.Forms.Label();
            this.toolStripTaxonSources = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelTaxonSources = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButtonTaxonSourceAdd = new System.Windows.Forms.ToolStripButton();
            this.tabPageMaterialCategory = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelMaterialCategories = new System.Windows.Forms.TableLayoutPanel();
            this.labelMaterialCategories = new System.Windows.Forms.Label();
            this.dataGridViewMaterialCategory = new System.Windows.Forms.DataGridView();
            this.codeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.displayTextDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recordBasisDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.enumRecordBasisBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.preparationTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.enumPreparationTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.categoryOrderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.enumMaterialCategoryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.labelMaterialCategoryMissing = new System.Windows.Forms.Label();
            this.buttonMaterialAdd = new System.Windows.Forms.Button();
            this.buttonMaterialRemove = new System.Windows.Forms.Button();
            this.listBoxMaterialNotTransferred = new System.Windows.Forms.ListBox();
            this.buttonSaveMaterial = new System.Windows.Forms.Button();
            this.labelMaterialPublished = new System.Windows.Forms.Label();
            this.tabPageKingdom = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelKingdom = new System.Windows.Forms.TableLayoutPanel();
            this.labelKingdomNotPublished = new System.Windows.Forms.Label();
            this.dataGridViewKingdoms = new System.Windows.Forms.DataGridView();
            this.codeDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.displayTextDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kingdomDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.enumKingdomBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.enumTaxonomicGroupBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.listBoxKingdomNotPublished = new System.Windows.Forms.ListBox();
            this.buttonKingdomNotPublished = new System.Windows.Forms.Button();
            this.buttonKingdomPublished = new System.Windows.Forms.Button();
            this.labelKingdomPublished = new System.Windows.Forms.Label();
            this.buttonSaveKingdoms = new System.Windows.Forms.Button();
            this.labelKingdoms = new System.Windows.Forms.Label();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelSettings = new System.Windows.Forms.TableLayoutPanel();
            this.labelSettingsHeader = new System.Windows.Forms.Label();
            this.tabPageAdminPostgres = new System.Windows.Forms.TabPage();
            this.splitContainerAdminPostgres = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelAdminPostgresList = new System.Windows.Forms.TableLayoutPanel();
            this.labelPostgresDBlist = new System.Windows.Forms.Label();
            this.listBoxPostgresDBs = new System.Windows.Forms.ListBox();
            this.tableLayoutPanelPostgresAdmin = new System.Windows.Forms.TableLayoutPanel();
            this.labelPostgresVersion = new System.Windows.Forms.Label();
            this.buttonPostgresUpdate = new System.Windows.Forms.Button();
            this.tabControlPostgresAdmin = new System.Windows.Forms.TabControl();
            this.tabPagePostgresRoles = new System.Windows.Forms.TabPage();
            this.splitContainerPgAdminRoles = new System.Windows.Forms.SplitContainer();
            this.listBoxPgAdminRoles = new System.Windows.Forms.ListBox();
            this.toolStripPgAdminRoles = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonPgAdminRoleAdministration = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanelPgAdminRoles = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridViewPgAdminRoleGrants = new System.Windows.Forms.DataGridView();
            this.labelPgAdminRoleGrants = new System.Windows.Forms.Label();
            this.tabPagePostgresGroups = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelPostgresUser = new System.Windows.Forms.TableLayoutPanel();
            this.listBoxPostgresUser = new System.Windows.Forms.ListBox();
            this.toolStripPostgresUser = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonPostgresUserAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPostgresUserDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPostgresUserPassword = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPostgresRoleAdministration = new System.Windows.Forms.ToolStripButton();
            this.dataGridViewPostgresUser = new System.Windows.Forms.DataGridView();
            this.tabPagePostgresTables = new System.Windows.Forms.TabPage();
            this.splitContainerPostgresTables = new System.Windows.Forms.SplitContainer();
            this.listBoxPGtables = new System.Windows.Forms.ListBox();
            this.dataGridViewPGtable = new System.Windows.Forms.DataGridView();
            this.tabPagePostgresProjects = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelPostgresAdminProjects = new System.Windows.Forms.TableLayoutPanel();
            this.labelPostgresProjectsAvailable = new System.Windows.Forms.Label();
            this.listBoxPostgresProjectsAvailable = new System.Windows.Forms.ListBox();
            this.buttonPostgresProjectsEstablish = new System.Windows.Forms.Button();
            this.labelPostgresProjectsEstablished = new System.Windows.Forms.Label();
            this.panelPostgresEstablishedProjects = new System.Windows.Forms.Panel();
            this.tabControlPgProjects = new System.Windows.Forms.TabControl();
            this.tabPagePgProjectPackages = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelPostgresPackages = new System.Windows.Forms.TableLayoutPanel();
            this.panelPostgresProjectPackages = new System.Windows.Forms.Panel();
            this.labelPostgresPackagesAvailable = new System.Windows.Forms.Label();
            this.labelPostgresPackagesEstablished = new System.Windows.Forms.Label();
            this.listBoxPostgresPackagesAvailable = new System.Windows.Forms.ListBox();
            this.buttonPostgresPackageEstablish = new System.Windows.Forms.Button();
            this.tabPagePgProjectPermissions = new System.Windows.Forms.TabPage();
            this.dataGridViewPostgresProjectPermissions = new System.Windows.Forms.DataGridView();
            this.labelProjectPackages = new System.Windows.Forms.Label();
            this.tableLayoutPanelPostgresAdminLogin = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripPostgresDBs = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonPostgresConnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPostgresNewDB = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPostgresDeleteDB = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorPgAdmin = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonPgAdminRoles = new System.Windows.Forms.ToolStripButton();
            this.labelPgAdminConnection = new System.Windows.Forms.Label();
            this.tabPageOverview = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelOverview = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxOverviewSource = new System.Windows.Forms.GroupBox();
            this.panelOverviewSource = new System.Windows.Forms.Panel();
            this.groupBoxOverviewCache = new System.Windows.Forms.GroupBox();
            this.panelOverviewCache = new System.Windows.Forms.Panel();
            this.groupBoxOverviewPostgres = new System.Windows.Forms.GroupBox();
            this.panelOverviewPostgres = new System.Windows.Forms.Panel();
            this.labelOverviewSourceDB = new System.Windows.Forms.Label();
            this.pictureBoxOverviewSourceDB = new System.Windows.Forms.PictureBox();
            this.pictureBoxOverviewCacheDB = new System.Windows.Forms.PictureBox();
            this.labelOverviewCacheDB = new System.Windows.Forms.Label();
            this.pictureBoxOverviewPostgresDB = new System.Windows.Forms.PictureBox();
            this.labelOverviewPostgresDB = new System.Windows.Forms.Label();
            this.buttonOverviewPostgresDBConnect = new System.Windows.Forms.Button();
            this.buttonOverviewTransferToCacheDB = new System.Windows.Forms.Button();
            this.buttonOverviewTransferToPostgresDB = new System.Windows.Forms.Button();
            this.projectPublishedTableAdapter = new DiversityCollection.CacheDatabase.DataSetCacheDB_1TableAdapters.ProjectPublishedTableAdapter();
            this.enumMaterialCategoryTableAdapter = new DiversityCollection.CacheDatabase.DataSetCacheDB_1TableAdapters.EnumMaterialCategoryTableAdapter();
            this.enumRecordBasisTableAdapter = new DiversityCollection.CacheDatabase.DataSetCacheDB_1TableAdapters.EnumRecordBasisTableAdapter();
            this.enumPreparationTypeTableAdapter = new DiversityCollection.CacheDatabase.DataSetCacheDB_1TableAdapters.EnumPreparationTypeTableAdapter();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.enumTaxonomicGroupTableAdapter = new DiversityCollection.CacheDatabase.DataSetCacheDB_1TableAdapters.EnumTaxonomicGroupTableAdapter();
            this.enumKingdomTableAdapter = new DiversityCollection.CacheDatabase.DataSetCacheDB_1TableAdapters.EnumKingdomTableAdapter();
            this.taxonomicGroupInProjectTableAdapter = new DiversityCollection.CacheDatabase.DataSetCacheDB_1TableAdapters.TaxonomicGroupInProjectTableAdapter();
            this.enumLocalisationSystemTableAdapter = new DiversityCollection.CacheDatabase.DataSetCacheDB_1TableAdapters.EnumLocalisationSystemTableAdapter();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.fKTaxonomicGroupInProjectProjectPublishedBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.imageListTransferSteps = new System.Windows.Forms.ImageList(this.components);
            this.tabControlMain.SuspendLayout();
            this.tabPageTransfer.SuspendLayout();
            this.tabControlTransfer.SuspendLayout();
            this.tabPageDBtoCacheDB.SuspendLayout();
            this.tableLayoutPanelTransferSteps.SuspendLayout();
            this.tableLayoutPanelVersion1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.projectPublishedBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCacheDB_1)).BeginInit();
            this.tabPageCacheDBToPostGres.SuspendLayout();
            this.tableLayoutPanelPostgres.SuspendLayout();
            this.groupBoxPostgresTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPostgresTable)).BeginInit();
            this.tabPageBioCASE.SuspendLayout();
            this.tableLayoutPanelBioCASE.SuspendLayout();
            this.tabPageAdminCacheDB.SuspendLayout();
            this.tabControlDatabase.SuspendLayout();
            this.tabPageUpdate.SuspendLayout();
            this.tableLayoutPanelDatabase.SuspendLayout();
            this.tabPageAnonymCollectors.SuspendLayout();
            this.tableLayoutPanelAnonymCollector.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAnonymAgent)).BeginInit();
            this.tabPageProjects.SuspendLayout();
            this.splitContainerProjects.Panel1.SuspendLayout();
            this.splitContainerProjects.Panel2.SuspendLayout();
            this.splitContainerProjects.SuspendLayout();
            this.toolStripAdministrationProjects.SuspendLayout();
            this.tabControlProjectAdminTaxMat.SuspendLayout();
            this.tabPageProjectTaxonomicGroup.SuspendLayout();
            this.tableLayoutPanelTaxonomicGroups.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.taxonomicGroupInProjectBindingSource)).BeginInit();
            this.tabPageProjectMaterialCategory.SuspendLayout();
            this.tableLayoutPanelProjectMaterialCategory.SuspendLayout();
            this.tabPageProjectLocalisationsystem.SuspendLayout();
            this.tableLayoutPanelProjectLocalisation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.enumLocalisationSystemBindingSource)).BeginInit();
            this.groupBoxProjectSettings.SuspendLayout();
            this.tableLayoutPanelProjectSettings.SuspendLayout();
            this.groupBoxProjectCoordinates.SuspendLayout();
            this.tableLayoutPanelLocalisations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCoordinatePrecision)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProjectCoordiates)).BeginInit();
            this.groupBoxProjectImages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProjectImages)).BeginInit();
            this.tabPageLocalisation.SuspendLayout();
            this.tableLayoutPanelLocalisation.SuspendLayout();
            this.tabPageTaxonSynonymy.SuspendLayout();
            this.tableLayoutPanelTaxonomySources.SuspendLayout();
            this.toolStripTaxonSources.SuspendLayout();
            this.tabPageMaterialCategory.SuspendLayout();
            this.tableLayoutPanelMaterialCategories.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMaterialCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.enumRecordBasisBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.enumPreparationTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.enumMaterialCategoryBindingSource)).BeginInit();
            this.tabPageKingdom.SuspendLayout();
            this.tableLayoutPanelKingdom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewKingdoms)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.enumKingdomBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.enumTaxonomicGroupBindingSource)).BeginInit();
            this.tabPageSettings.SuspendLayout();
            this.tableLayoutPanelSettings.SuspendLayout();
            this.tabPageAdminPostgres.SuspendLayout();
            this.splitContainerAdminPostgres.Panel1.SuspendLayout();
            this.splitContainerAdminPostgres.Panel2.SuspendLayout();
            this.splitContainerAdminPostgres.SuspendLayout();
            this.tableLayoutPanelAdminPostgresList.SuspendLayout();
            this.tableLayoutPanelPostgresAdmin.SuspendLayout();
            this.tabControlPostgresAdmin.SuspendLayout();
            this.tabPagePostgresRoles.SuspendLayout();
            this.splitContainerPgAdminRoles.Panel1.SuspendLayout();
            this.splitContainerPgAdminRoles.Panel2.SuspendLayout();
            this.splitContainerPgAdminRoles.SuspendLayout();
            this.toolStripPgAdminRoles.SuspendLayout();
            this.tableLayoutPanelPgAdminRoles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPgAdminRoleGrants)).BeginInit();
            this.tabPagePostgresGroups.SuspendLayout();
            this.tableLayoutPanelPostgresUser.SuspendLayout();
            this.toolStripPostgresUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPostgresUser)).BeginInit();
            this.tabPagePostgresTables.SuspendLayout();
            this.splitContainerPostgresTables.Panel1.SuspendLayout();
            this.splitContainerPostgresTables.Panel2.SuspendLayout();
            this.splitContainerPostgresTables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPGtable)).BeginInit();
            this.tabPagePostgresProjects.SuspendLayout();
            this.tableLayoutPanelPostgresAdminProjects.SuspendLayout();
            this.tabControlPgProjects.SuspendLayout();
            this.tabPagePgProjectPackages.SuspendLayout();
            this.tableLayoutPanelPostgresPackages.SuspendLayout();
            this.tabPagePgProjectPermissions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPostgresProjectPermissions)).BeginInit();
            this.tableLayoutPanelPostgresAdminLogin.SuspendLayout();
            this.toolStripPostgresDBs.SuspendLayout();
            this.tabPageOverview.SuspendLayout();
            this.tableLayoutPanelOverview.SuspendLayout();
            this.groupBoxOverviewSource.SuspendLayout();
            this.groupBoxOverviewCache.SuspendLayout();
            this.groupBoxOverviewPostgres.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOverviewSourceDB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOverviewCacheDB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOverviewPostgresDB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fKTaxonomicGroupInProjectProjectPublishedBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageTransfer);
            this.tabControlMain.Controls.Add(this.tabPageAdminCacheDB);
            this.tabControlMain.Controls.Add(this.tabPageAdminPostgres);
            this.tabControlMain.Controls.Add(this.tabPageOverview);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.ImageList = this.imageListTabControl;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(772, 616);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageTransfer
            // 
            this.tabPageTransfer.Controls.Add(this.tabControlTransfer);
            this.helpProvider.SetHelpKeyword(this.tabPageTransfer, "Cache database");
            this.helpProvider.SetHelpNavigator(this.tabPageTransfer, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.helpProvider.SetHelpString(this.tabPageTransfer, "Cache database");
            this.tabPageTransfer.Location = new System.Drawing.Point(4, 23);
            this.tabPageTransfer.Name = "tabPageTransfer";
            this.tabPageTransfer.Padding = new System.Windows.Forms.Padding(3);
            this.helpProvider.SetShowHelp(this.tabPageTransfer, true);
            this.tabPageTransfer.Size = new System.Drawing.Size(764, 589);
            this.tabPageTransfer.TabIndex = 0;
            this.tabPageTransfer.Text = "Transfer data";
            this.tabPageTransfer.UseVisualStyleBackColor = true;
            // 
            // tabControlTransfer
            // 
            this.tabControlTransfer.Controls.Add(this.tabPageDBtoCacheDB);
            this.tabControlTransfer.Controls.Add(this.tabPageCacheDBToPostGres);
            this.tabControlTransfer.Controls.Add(this.tabPageBioCASE);
            this.tabControlTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlTransfer.ImageList = this.imageListTabControl;
            this.tabControlTransfer.Location = new System.Drawing.Point(3, 3);
            this.tabControlTransfer.Name = "tabControlTransfer";
            this.tabControlTransfer.SelectedIndex = 0;
            this.tabControlTransfer.Size = new System.Drawing.Size(758, 583);
            this.tabControlTransfer.TabIndex = 3;
            // 
            // tabPageDBtoCacheDB
            // 
            this.tabPageDBtoCacheDB.Controls.Add(this.tableLayoutPanelTransferSteps);
            this.tabPageDBtoCacheDB.Controls.Add(this.tableLayoutPanelVersion1);
            this.tabPageDBtoCacheDB.ImageIndex = 9;
            this.tabPageDBtoCacheDB.Location = new System.Drawing.Point(4, 23);
            this.tabPageDBtoCacheDB.Name = "tabPageDBtoCacheDB";
            this.tabPageDBtoCacheDB.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDBtoCacheDB.Size = new System.Drawing.Size(750, 556);
            this.tabPageDBtoCacheDB.TabIndex = 0;
            this.tabPageDBtoCacheDB.Text = "DB -> CacheDB";
            this.tabPageDBtoCacheDB.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelTransferSteps
            // 
            this.tableLayoutPanelTransferSteps.ColumnCount = 2;
            this.tableLayoutPanelTransferSteps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTransferSteps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTransferSteps.Controls.Add(this.panelTransferSteps, 1, 0);
            this.tableLayoutPanelTransferSteps.Controls.Add(this.buttonStartTransfer, 0, 0);
            this.tableLayoutPanelTransferSteps.Controls.Add(this.labelWarningNoGazetteer, 0, 2);
            this.tableLayoutPanelTransferSteps.Controls.Add(this.labelLastTransfer, 0, 1);
            this.tableLayoutPanelTransferSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTransferSteps.Location = new System.Drawing.Point(3, 252);
            this.tableLayoutPanelTransferSteps.Name = "tableLayoutPanelTransferSteps";
            this.tableLayoutPanelTransferSteps.RowCount = 3;
            this.tableLayoutPanelTransferSteps.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTransferSteps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelTransferSteps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTransferSteps.Size = new System.Drawing.Size(744, 301);
            this.tableLayoutPanelTransferSteps.TabIndex = 2;
            // 
            // panelTransferSteps
            // 
            this.panelTransferSteps.AutoScroll = true;
            this.panelTransferSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTransferSteps.Location = new System.Drawing.Point(108, 3);
            this.panelTransferSteps.Name = "panelTransferSteps";
            this.tableLayoutPanelTransferSteps.SetRowSpan(this.panelTransferSteps, 3);
            this.panelTransferSteps.Size = new System.Drawing.Size(633, 295);
            this.panelTransferSteps.TabIndex = 0;
            // 
            // buttonStartTransfer
            // 
            this.buttonStartTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonStartTransfer.Location = new System.Drawing.Point(3, 3);
            this.buttonStartTransfer.Name = "buttonStartTransfer";
            this.buttonStartTransfer.Size = new System.Drawing.Size(99, 50);
            this.buttonStartTransfer.TabIndex = 1;
            this.buttonStartTransfer.Text = "Start transfer to cache database";
            this.buttonStartTransfer.UseVisualStyleBackColor = true;
            this.buttonStartTransfer.Click += new System.EventHandler(this.buttonStartTransfer_Click);
            // 
            // labelWarningNoGazetteer
            // 
            this.labelWarningNoGazetteer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWarningNoGazetteer.ForeColor = System.Drawing.Color.Red;
            this.labelWarningNoGazetteer.Location = new System.Drawing.Point(3, 86);
            this.labelWarningNoGazetteer.Name = "labelWarningNoGazetteer";
            this.labelWarningNoGazetteer.Size = new System.Drawing.Size(99, 215);
            this.labelWarningNoGazetteer.TabIndex = 15;
            this.labelWarningNoGazetteer.Text = "No connection to a DiversityGazetter database as source of the ISO-codes for the " +
    "countries is established ";
            this.labelWarningNoGazetteer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelWarningNoGazetteer.Visible = false;
            // 
            // labelLastTransfer
            // 
            this.labelLastTransfer.AutoSize = true;
            this.labelLastTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLastTransfer.Location = new System.Drawing.Point(3, 56);
            this.labelLastTransfer.Name = "labelLastTransfer";
            this.labelLastTransfer.Size = new System.Drawing.Size(99, 30);
            this.labelLastTransfer.TabIndex = 16;
            this.labelLastTransfer.Text = "Last transfer:";
            this.labelLastTransfer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanelVersion1
            // 
            this.tableLayoutPanelVersion1.ColumnCount = 5;
            this.tableLayoutPanelVersion1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.77778F));
            this.tableLayoutPanelVersion1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.22222F));
            this.tableLayoutPanelVersion1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelVersion1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.22222F));
            this.tableLayoutPanelVersion1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.77778F));
            this.tableLayoutPanelVersion1.Controls.Add(this.buttonV1StartCollectionTransfer, 1, 3);
            this.tableLayoutPanelVersion1.Controls.Add(this.buttonV1StartTaxonTransfer, 1, 5);
            this.tableLayoutPanelVersion1.Controls.Add(this.listBoxV1ProjectPublished, 3, 1);
            this.tableLayoutPanelVersion1.Controls.Add(this.listBoxV1ProjectsUnpublished, 0, 1);
            this.tableLayoutPanelVersion1.Controls.Add(this.buttonV1PublishProject, 2, 1);
            this.tableLayoutPanelVersion1.Controls.Add(this.buttonV1UnpublishProject, 2, 2);
            this.tableLayoutPanelVersion1.Controls.Add(this.labelProjectsUnpublished, 0, 0);
            this.tableLayoutPanelVersion1.Controls.Add(this.labelProjectsPublished, 3, 0);
            this.tableLayoutPanelVersion1.Controls.Add(this.labelV1LastSpecimenTransfer, 0, 4);
            this.tableLayoutPanelVersion1.Controls.Add(this.labelV1CurrentSpecimenNumber, 3, 4);
            this.tableLayoutPanelVersion1.Controls.Add(this.labelV1LastTaxonTransfer, 0, 6);
            this.tableLayoutPanelVersion1.Controls.Add(this.labelV1CurrentTaxa, 3, 6);
            this.tableLayoutPanelVersion1.Controls.Add(this.buttonV1CheckSpecimen, 2, 4);
            this.tableLayoutPanelVersion1.Controls.Add(this.buttonV1CheckTaxa, 2, 6);
            this.tableLayoutPanelVersion1.Controls.Add(this.buttonTransferCountryIcoCode, 4, 3);
            this.tableLayoutPanelVersion1.Dock = System.Windows.Forms.DockStyle.Top;
            this.helpProvider.SetHelpKeyword(this.tableLayoutPanelVersion1, "Cache database import");
            this.helpProvider.SetHelpNavigator(this.tableLayoutPanelVersion1, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.tableLayoutPanelVersion1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelVersion1.Name = "tableLayoutPanelVersion1";
            this.tableLayoutPanelVersion1.RowCount = 7;
            this.tableLayoutPanelVersion1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelVersion1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41.77216F));
            this.tableLayoutPanelVersion1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58.22785F));
            this.tableLayoutPanelVersion1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanelVersion1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanelVersion1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanelVersion1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.helpProvider.SetShowHelp(this.tableLayoutPanelVersion1, true);
            this.tableLayoutPanelVersion1.Size = new System.Drawing.Size(744, 249);
            this.tableLayoutPanelVersion1.TabIndex = 1;
            // 
            // buttonV1StartCollectionTransfer
            // 
            this.tableLayoutPanelVersion1.SetColumnSpan(this.buttonV1StartCollectionTransfer, 3);
            this.buttonV1StartCollectionTransfer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonV1StartCollectionTransfer.Image = global::DiversityCollection.Resource.CollectionSpecimen;
            this.buttonV1StartCollectionTransfer.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonV1StartCollectionTransfer.Location = new System.Drawing.Point(203, 251);
            this.buttonV1StartCollectionTransfer.Name = "buttonV1StartCollectionTransfer";
            this.buttonV1StartCollectionTransfer.Size = new System.Drawing.Size(336, 1);
            this.buttonV1StartCollectionTransfer.TabIndex = 0;
            this.buttonV1StartCollectionTransfer.Text = "Transfer collection data";
            this.buttonV1StartCollectionTransfer.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonV1StartCollectionTransfer.UseVisualStyleBackColor = true;
            this.buttonV1StartCollectionTransfer.Click += new System.EventHandler(this.buttonV1StartCollectionTransfer_Click);
            // 
            // buttonV1StartTaxonTransfer
            // 
            this.tableLayoutPanelVersion1.SetColumnSpan(this.buttonV1StartTaxonTransfer, 3);
            this.buttonV1StartTaxonTransfer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonV1StartTaxonTransfer.Image = global::DiversityCollection.Resource.Identification;
            this.buttonV1StartTaxonTransfer.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonV1StartTaxonTransfer.Location = new System.Drawing.Point(203, 251);
            this.buttonV1StartTaxonTransfer.Name = "buttonV1StartTaxonTransfer";
            this.buttonV1StartTaxonTransfer.Size = new System.Drawing.Size(336, 1);
            this.buttonV1StartTaxonTransfer.TabIndex = 1;
            this.buttonV1StartTaxonTransfer.Text = "Transfer taxonomic names";
            this.buttonV1StartTaxonTransfer.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonV1StartTaxonTransfer.UseVisualStyleBackColor = true;
            this.buttonV1StartTaxonTransfer.Click += new System.EventHandler(this.buttonV1StartTaxonTransfer_Click);
            // 
            // listBoxV1ProjectPublished
            // 
            this.listBoxV1ProjectPublished.BackColor = System.Drawing.Color.LightGreen;
            this.tableLayoutPanelVersion1.SetColumnSpan(this.listBoxV1ProjectPublished, 2);
            this.listBoxV1ProjectPublished.DataSource = this.projectPublishedBindingSource;
            this.listBoxV1ProjectPublished.DisplayMember = "Project";
            this.listBoxV1ProjectPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxV1ProjectPublished.FormattingEnabled = true;
            this.listBoxV1ProjectPublished.Location = new System.Drawing.Point(386, 23);
            this.listBoxV1ProjectPublished.Name = "listBoxV1ProjectPublished";
            this.tableLayoutPanelVersion1.SetRowSpan(this.listBoxV1ProjectPublished, 2);
            this.listBoxV1ProjectPublished.Size = new System.Drawing.Size(355, 222);
            this.listBoxV1ProjectPublished.TabIndex = 2;
            this.listBoxV1ProjectPublished.ValueMember = "ProjectID";
            // 
            // projectPublishedBindingSource
            // 
            this.projectPublishedBindingSource.DataMember = "ProjectPublished";
            this.projectPublishedBindingSource.DataSource = this.dataSetCacheDB_1;
            // 
            // dataSetCacheDB_1
            // 
            this.dataSetCacheDB_1.DataSetName = "DataSetCacheDB_1";
            this.dataSetCacheDB_1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // listBoxV1ProjectsUnpublished
            // 
            this.listBoxV1ProjectsUnpublished.BackColor = System.Drawing.Color.Pink;
            this.tableLayoutPanelVersion1.SetColumnSpan(this.listBoxV1ProjectsUnpublished, 2);
            this.listBoxV1ProjectsUnpublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxV1ProjectsUnpublished.FormattingEnabled = true;
            this.listBoxV1ProjectsUnpublished.Location = new System.Drawing.Point(3, 23);
            this.listBoxV1ProjectsUnpublished.Name = "listBoxV1ProjectsUnpublished";
            this.tableLayoutPanelVersion1.SetRowSpan(this.listBoxV1ProjectsUnpublished, 2);
            this.listBoxV1ProjectsUnpublished.Size = new System.Drawing.Size(353, 222);
            this.listBoxV1ProjectsUnpublished.TabIndex = 3;
            // 
            // buttonV1PublishProject
            // 
            this.buttonV1PublishProject.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonV1PublishProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonV1PublishProject.ForeColor = System.Drawing.Color.Green;
            this.buttonV1PublishProject.Location = new System.Drawing.Point(362, 89);
            this.buttonV1PublishProject.Name = "buttonV1PublishProject";
            this.buttonV1PublishProject.Size = new System.Drawing.Size(18, 23);
            this.buttonV1PublishProject.TabIndex = 4;
            this.buttonV1PublishProject.Text = ">";
            this.buttonV1PublishProject.UseVisualStyleBackColor = true;
            this.buttonV1PublishProject.Click += new System.EventHandler(this.buttonV1PublishProject_Click);
            // 
            // buttonV1UnpublishProject
            // 
            this.buttonV1UnpublishProject.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonV1UnpublishProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonV1UnpublishProject.ForeColor = System.Drawing.Color.Red;
            this.buttonV1UnpublishProject.Location = new System.Drawing.Point(362, 118);
            this.buttonV1UnpublishProject.Name = "buttonV1UnpublishProject";
            this.buttonV1UnpublishProject.Size = new System.Drawing.Size(18, 23);
            this.buttonV1UnpublishProject.TabIndex = 5;
            this.buttonV1UnpublishProject.Text = "<";
            this.buttonV1UnpublishProject.UseVisualStyleBackColor = true;
            this.buttonV1UnpublishProject.Click += new System.EventHandler(this.buttonV1UnpublishProject_Click);
            // 
            // labelProjectsUnpublished
            // 
            this.labelProjectsUnpublished.AutoSize = true;
            this.tableLayoutPanelVersion1.SetColumnSpan(this.labelProjectsUnpublished, 2);
            this.labelProjectsUnpublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProjectsUnpublished.Location = new System.Drawing.Point(3, 0);
            this.labelProjectsUnpublished.Name = "labelProjectsUnpublished";
            this.labelProjectsUnpublished.Size = new System.Drawing.Size(353, 20);
            this.labelProjectsUnpublished.TabIndex = 6;
            this.labelProjectsUnpublished.Text = "Unpublished projects";
            this.labelProjectsUnpublished.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProjectsPublished
            // 
            this.labelProjectsPublished.AutoSize = true;
            this.tableLayoutPanelVersion1.SetColumnSpan(this.labelProjectsPublished, 2);
            this.labelProjectsPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProjectsPublished.Location = new System.Drawing.Point(386, 0);
            this.labelProjectsPublished.Name = "labelProjectsPublished";
            this.labelProjectsPublished.Size = new System.Drawing.Size(355, 20);
            this.labelProjectsPublished.TabIndex = 7;
            this.labelProjectsPublished.Text = "Published projects";
            this.labelProjectsPublished.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelV1LastSpecimenTransfer
            // 
            this.labelV1LastSpecimenTransfer.AutoSize = true;
            this.tableLayoutPanelVersion1.SetColumnSpan(this.labelV1LastSpecimenTransfer, 2);
            this.labelV1LastSpecimenTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelV1LastSpecimenTransfer.Location = new System.Drawing.Point(3, 248);
            this.labelV1LastSpecimenTransfer.Name = "labelV1LastSpecimenTransfer";
            this.labelV1LastSpecimenTransfer.Size = new System.Drawing.Size(353, 1);
            this.labelV1LastSpecimenTransfer.TabIndex = 8;
            this.labelV1LastSpecimenTransfer.Text = "Last transfer:";
            this.labelV1LastSpecimenTransfer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelV1CurrentSpecimenNumber
            // 
            this.labelV1CurrentSpecimenNumber.AutoSize = true;
            this.tableLayoutPanelVersion1.SetColumnSpan(this.labelV1CurrentSpecimenNumber, 2);
            this.labelV1CurrentSpecimenNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelV1CurrentSpecimenNumber.Location = new System.Drawing.Point(386, 248);
            this.labelV1CurrentSpecimenNumber.Name = "labelV1CurrentSpecimenNumber";
            this.labelV1CurrentSpecimenNumber.Size = new System.Drawing.Size(355, 1);
            this.labelV1CurrentSpecimenNumber.TabIndex = 9;
            this.labelV1CurrentSpecimenNumber.Text = "Current specimens:";
            this.labelV1CurrentSpecimenNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelV1LastTaxonTransfer
            // 
            this.labelV1LastTaxonTransfer.AutoSize = true;
            this.tableLayoutPanelVersion1.SetColumnSpan(this.labelV1LastTaxonTransfer, 2);
            this.labelV1LastTaxonTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelV1LastTaxonTransfer.Location = new System.Drawing.Point(3, 248);
            this.labelV1LastTaxonTransfer.Name = "labelV1LastTaxonTransfer";
            this.labelV1LastTaxonTransfer.Size = new System.Drawing.Size(353, 1);
            this.labelV1LastTaxonTransfer.TabIndex = 10;
            this.labelV1LastTaxonTransfer.Text = "Last transfer:";
            this.labelV1LastTaxonTransfer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelV1CurrentTaxa
            // 
            this.labelV1CurrentTaxa.AutoSize = true;
            this.tableLayoutPanelVersion1.SetColumnSpan(this.labelV1CurrentTaxa, 2);
            this.labelV1CurrentTaxa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelV1CurrentTaxa.Location = new System.Drawing.Point(386, 248);
            this.labelV1CurrentTaxa.Name = "labelV1CurrentTaxa";
            this.labelV1CurrentTaxa.Size = new System.Drawing.Size(355, 1);
            this.labelV1CurrentTaxa.TabIndex = 11;
            this.labelV1CurrentTaxa.Text = "Current taxa:";
            this.labelV1CurrentTaxa.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonV1CheckSpecimen
            // 
            this.buttonV1CheckSpecimen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonV1CheckSpecimen.Image = global::DiversityCollection.Resource.Lupe;
            this.buttonV1CheckSpecimen.Location = new System.Drawing.Point(359, 248);
            this.buttonV1CheckSpecimen.Margin = new System.Windows.Forms.Padding(0);
            this.buttonV1CheckSpecimen.Name = "buttonV1CheckSpecimen";
            this.buttonV1CheckSpecimen.Size = new System.Drawing.Size(24, 1);
            this.buttonV1CheckSpecimen.TabIndex = 12;
            this.buttonV1CheckSpecimen.UseVisualStyleBackColor = true;
            this.buttonV1CheckSpecimen.Visible = false;
            this.buttonV1CheckSpecimen.Click += new System.EventHandler(this.buttonV1CheckSpecimen_Click);
            // 
            // buttonV1CheckTaxa
            // 
            this.buttonV1CheckTaxa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonV1CheckTaxa.Image = global::DiversityCollection.Resource.Lupe;
            this.buttonV1CheckTaxa.Location = new System.Drawing.Point(359, 248);
            this.buttonV1CheckTaxa.Margin = new System.Windows.Forms.Padding(0);
            this.buttonV1CheckTaxa.Name = "buttonV1CheckTaxa";
            this.buttonV1CheckTaxa.Size = new System.Drawing.Size(24, 1);
            this.buttonV1CheckTaxa.TabIndex = 13;
            this.buttonV1CheckTaxa.UseVisualStyleBackColor = true;
            this.buttonV1CheckTaxa.Visible = false;
            this.buttonV1CheckTaxa.Click += new System.EventHandler(this.buttonV1CheckTaxa_Click);
            // 
            // buttonTransferCountryIcoCode
            // 
            this.buttonTransferCountryIcoCode.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonTransferCountryIcoCode.Location = new System.Drawing.Point(545, 251);
            this.buttonTransferCountryIcoCode.Name = "buttonTransferCountryIcoCode";
            this.buttonTransferCountryIcoCode.Size = new System.Drawing.Size(196, 1);
            this.buttonTransferCountryIcoCode.TabIndex = 14;
            this.buttonTransferCountryIcoCode.Text = "Transfer IsoCode for countries";
            this.buttonTransferCountryIcoCode.UseVisualStyleBackColor = true;
            this.buttonTransferCountryIcoCode.Click += new System.EventHandler(this.buttonTransferCountryIcoCode_Click);
            // 
            // tabPageCacheDBToPostGres
            // 
            this.tabPageCacheDBToPostGres.Controls.Add(this.tableLayoutPanelPostgres);
            this.tabPageCacheDBToPostGres.ImageIndex = 10;
            this.tabPageCacheDBToPostGres.Location = new System.Drawing.Point(4, 23);
            this.tabPageCacheDBToPostGres.Name = "tabPageCacheDBToPostGres";
            this.tabPageCacheDBToPostGres.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCacheDBToPostGres.Size = new System.Drawing.Size(750, 556);
            this.tabPageCacheDBToPostGres.TabIndex = 1;
            this.tabPageCacheDBToPostGres.Text = "CacheDB -> PostgreSQL";
            this.tabPageCacheDBToPostGres.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelPostgres
            // 
            this.tableLayoutPanelPostgres.ColumnCount = 4;
            this.tableLayoutPanelPostgres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPostgres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPostgres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPostgres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPostgres.Controls.Add(this.listBoxPostgresTables, 0, 7);
            this.tableLayoutPanelPostgres.Controls.Add(this.labelPostgresTables, 0, 6);
            this.tableLayoutPanelPostgres.Controls.Add(this.groupBoxPostgresTable, 3, 1);
            this.tableLayoutPanelPostgres.Controls.Add(this.labelPostgresProject, 0, 2);
            this.tableLayoutPanelPostgres.Controls.Add(this.buttonPostgresTransferProject, 0, 3);
            this.tableLayoutPanelPostgres.Controls.Add(this.comboBoxPostgresProject, 2, 2);
            this.tableLayoutPanelPostgres.Controls.Add(this.buttonPostgresConnect, 0, 0);
            this.tableLayoutPanelPostgres.Controls.Add(this.labelPostgresConnection, 1, 0);
            this.tableLayoutPanelPostgres.Controls.Add(this.progressBarPostgresTransfer, 0, 5);
            this.tableLayoutPanelPostgres.Controls.Add(this.labelPostgresTransferMessage, 0, 4);
            this.tableLayoutPanelPostgres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpProvider.SetHelpKeyword(this.tableLayoutPanelPostgres, "Postgres database import");
            this.helpProvider.SetHelpNavigator(this.tableLayoutPanelPostgres, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.tableLayoutPanelPostgres.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelPostgres.Name = "tableLayoutPanelPostgres";
            this.tableLayoutPanelPostgres.RowCount = 8;
            this.tableLayoutPanelPostgres.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPostgres.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPostgres.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPostgres.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPostgres.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPostgres.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPostgres.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelPostgres.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.helpProvider.SetShowHelp(this.tableLayoutPanelPostgres, true);
            this.tableLayoutPanelPostgres.Size = new System.Drawing.Size(744, 550);
            this.tableLayoutPanelPostgres.TabIndex = 0;
            // 
            // listBoxPostgresTables
            // 
            this.tableLayoutPanelPostgres.SetColumnSpan(this.listBoxPostgresTables, 3);
            this.listBoxPostgresTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxPostgresTables.FormattingEnabled = true;
            this.listBoxPostgresTables.Location = new System.Drawing.Point(3, 131);
            this.listBoxPostgresTables.Name = "listBoxPostgresTables";
            this.listBoxPostgresTables.Size = new System.Drawing.Size(186, 416);
            this.listBoxPostgresTables.TabIndex = 15;
            this.listBoxPostgresTables.SelectedIndexChanged += new System.EventHandler(this.listBoxPostgresTables_SelectedIndexChanged);
            // 
            // labelPostgresTables
            // 
            this.labelPostgresTables.AutoSize = true;
            this.tableLayoutPanelPostgres.SetColumnSpan(this.labelPostgresTables, 3);
            this.labelPostgresTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPostgresTables.Location = new System.Drawing.Point(3, 111);
            this.labelPostgresTables.Margin = new System.Windows.Forms.Padding(3);
            this.labelPostgresTables.Name = "labelPostgresTables";
            this.labelPostgresTables.Size = new System.Drawing.Size(186, 14);
            this.labelPostgresTables.TabIndex = 16;
            this.labelPostgresTables.Text = "Tables in PostgreSQL database";
            // 
            // groupBoxPostgresTable
            // 
            this.groupBoxPostgresTable.Controls.Add(this.dataGridViewPostgresTable);
            this.groupBoxPostgresTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxPostgresTable.Location = new System.Drawing.Point(195, 16);
            this.groupBoxPostgresTable.Name = "groupBoxPostgresTable";
            this.tableLayoutPanelPostgres.SetRowSpan(this.groupBoxPostgresTable, 7);
            this.groupBoxPostgresTable.Size = new System.Drawing.Size(546, 531);
            this.groupBoxPostgresTable.TabIndex = 17;
            this.groupBoxPostgresTable.TabStop = false;
            // 
            // dataGridViewPostgresTable
            // 
            this.dataGridViewPostgresTable.AllowUserToAddRows = false;
            this.dataGridViewPostgresTable.AllowUserToDeleteRows = false;
            this.dataGridViewPostgresTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridViewPostgresTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPostgresTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPostgresTable.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewPostgresTable.Name = "dataGridViewPostgresTable";
            this.dataGridViewPostgresTable.ReadOnly = true;
            this.dataGridViewPostgresTable.RowHeadersVisible = false;
            this.dataGridViewPostgresTable.Size = new System.Drawing.Size(540, 512);
            this.dataGridViewPostgresTable.TabIndex = 0;
            // 
            // labelPostgresProject
            // 
            this.labelPostgresProject.AutoSize = true;
            this.tableLayoutPanelPostgres.SetColumnSpan(this.labelPostgresProject, 2);
            this.labelPostgresProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPostgresProject.Location = new System.Drawing.Point(3, 23);
            this.labelPostgresProject.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelPostgresProject.Name = "labelPostgresProject";
            this.labelPostgresProject.Size = new System.Drawing.Size(43, 27);
            this.labelPostgresProject.TabIndex = 18;
            this.labelPostgresProject.Text = "Project:";
            this.labelPostgresProject.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonPostgresTransferProject
            // 
            this.tableLayoutPanelPostgres.SetColumnSpan(this.buttonPostgresTransferProject, 3);
            this.buttonPostgresTransferProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPostgresTransferProject.Location = new System.Drawing.Point(3, 53);
            this.buttonPostgresTransferProject.Name = "buttonPostgresTransferProject";
            this.buttonPostgresTransferProject.Size = new System.Drawing.Size(186, 23);
            this.buttonPostgresTransferProject.TabIndex = 19;
            this.buttonPostgresTransferProject.Text = "Start Transfer of project";
            this.buttonPostgresTransferProject.UseVisualStyleBackColor = true;
            this.buttonPostgresTransferProject.Click += new System.EventHandler(this.buttonPostgresTransferProject_Click);
            // 
            // comboBoxPostgresProject
            // 
            this.comboBoxPostgresProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxPostgresProject.FormattingEnabled = true;
            this.comboBoxPostgresProject.Location = new System.Drawing.Point(46, 26);
            this.comboBoxPostgresProject.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxPostgresProject.Name = "comboBoxPostgresProject";
            this.comboBoxPostgresProject.Size = new System.Drawing.Size(143, 21);
            this.comboBoxPostgresProject.TabIndex = 20;
            this.comboBoxPostgresProject.SelectedIndexChanged += new System.EventHandler(this.comboBoxPostgresProject_SelectedIndexChanged);
            // 
            // buttonPostgresConnect
            // 
            this.buttonPostgresConnect.BackColor = System.Drawing.Color.Red;
            this.buttonPostgresConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPostgresConnect.ForeColor = System.Drawing.Color.Transparent;
            this.buttonPostgresConnect.Image = global::DiversityCollection.Resource.NoPostgres;
            this.buttonPostgresConnect.Location = new System.Drawing.Point(3, 0);
            this.buttonPostgresConnect.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonPostgresConnect.Name = "buttonPostgresConnect";
            this.tableLayoutPanelPostgres.SetRowSpan(this.buttonPostgresConnect, 2);
            this.buttonPostgresConnect.Size = new System.Drawing.Size(24, 23);
            this.buttonPostgresConnect.TabIndex = 22;
            this.toolTip.SetToolTip(this.buttonPostgresConnect, "Connect to postgres database");
            this.buttonPostgresConnect.UseVisualStyleBackColor = false;
            this.buttonPostgresConnect.Click += new System.EventHandler(this.buttonPostgresConnect_Click);
            // 
            // labelPostgresConnection
            // 
            this.labelPostgresConnection.AutoSize = true;
            this.tableLayoutPanelPostgres.SetColumnSpan(this.labelPostgresConnection, 3);
            this.labelPostgresConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPostgresConnection.ForeColor = System.Drawing.Color.Red;
            this.labelPostgresConnection.Location = new System.Drawing.Point(30, 0);
            this.labelPostgresConnection.Margin = new System.Windows.Forms.Padding(0);
            this.labelPostgresConnection.Name = "labelPostgresConnection";
            this.labelPostgresConnection.Size = new System.Drawing.Size(714, 13);
            this.labelPostgresConnection.TabIndex = 23;
            this.labelPostgresConnection.Text = "Not connected";
            this.labelPostgresConnection.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // progressBarPostgresTransfer
            // 
            this.tableLayoutPanelPostgres.SetColumnSpan(this.progressBarPostgresTransfer, 3);
            this.progressBarPostgresTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBarPostgresTransfer.Location = new System.Drawing.Point(3, 92);
            this.progressBarPostgresTransfer.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.progressBarPostgresTransfer.Name = "progressBarPostgresTransfer";
            this.progressBarPostgresTransfer.Size = new System.Drawing.Size(186, 13);
            this.progressBarPostgresTransfer.TabIndex = 24;
            // 
            // labelPostgresTransferMessage
            // 
            this.tableLayoutPanelPostgres.SetColumnSpan(this.labelPostgresTransferMessage, 3);
            this.labelPostgresTransferMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPostgresTransferMessage.Location = new System.Drawing.Point(3, 79);
            this.labelPostgresTransferMessage.Name = "labelPostgresTransferMessage";
            this.labelPostgresTransferMessage.Size = new System.Drawing.Size(186, 13);
            this.labelPostgresTransferMessage.TabIndex = 25;
            this.labelPostgresTransferMessage.Text = " ";
            // 
            // tabPageBioCASE
            // 
            this.tabPageBioCASE.Controls.Add(this.tableLayoutPanelBioCASE);
            this.tabPageBioCASE.ImageIndex = 11;
            this.tabPageBioCASE.Location = new System.Drawing.Point(4, 23);
            this.tabPageBioCASE.Name = "tabPageBioCASE";
            this.tabPageBioCASE.Size = new System.Drawing.Size(750, 556);
            this.tabPageBioCASE.TabIndex = 2;
            this.tabPageBioCASE.Text = "BioCASe";
            this.tabPageBioCASE.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelBioCASE
            // 
            this.tableLayoutPanelBioCASE.ColumnCount = 4;
            this.tableLayoutPanelBioCASE.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelBioCASE.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBioCASE.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelBioCASE.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelBioCASE.Controls.Add(this.labelBioCASEsources, 0, 0);
            this.tableLayoutPanelBioCASE.Controls.Add(this.textBoxBioCASEsources, 1, 0);
            this.tableLayoutPanelBioCASE.Controls.Add(this.buttonBioCASErefresh, 2, 0);
            this.tableLayoutPanelBioCASE.Controls.Add(this.webBrowserBioCASE, 0, 1);
            this.tableLayoutPanelBioCASE.Controls.Add(this.buttonBioCASEback, 3, 0);
            this.tableLayoutPanelBioCASE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBioCASE.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelBioCASE.Name = "tableLayoutPanelBioCASE";
            this.tableLayoutPanelBioCASE.RowCount = 2;
            this.tableLayoutPanelBioCASE.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBioCASE.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBioCASE.Size = new System.Drawing.Size(750, 556);
            this.tableLayoutPanelBioCASE.TabIndex = 0;
            // 
            // labelBioCASEsources
            // 
            this.labelBioCASEsources.AutoSize = true;
            this.labelBioCASEsources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelBioCASEsources.Location = new System.Drawing.Point(3, 0);
            this.labelBioCASEsources.Name = "labelBioCASEsources";
            this.labelBioCASEsources.Size = new System.Drawing.Size(92, 26);
            this.labelBioCASEsources.TabIndex = 0;
            this.labelBioCASEsources.Text = "BioCASe sources:";
            this.labelBioCASEsources.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxBioCASEsources
            // 
            this.textBoxBioCASEsources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxBioCASEsources.Location = new System.Drawing.Point(101, 3);
            this.textBoxBioCASEsources.Name = "textBoxBioCASEsources";
            this.textBoxBioCASEsources.Size = new System.Drawing.Size(602, 20);
            this.textBoxBioCASEsources.TabIndex = 1;
            this.toolTip.SetToolTip(this.textBoxBioCASEsources, "Please enter the path of the BioCASe sources");
            // 
            // buttonBioCASErefresh
            // 
            this.buttonBioCASErefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonBioCASErefresh.Image = global::DiversityCollection.Resource.Transfrom;
            this.buttonBioCASErefresh.Location = new System.Drawing.Point(706, 0);
            this.buttonBioCASErefresh.Margin = new System.Windows.Forms.Padding(0);
            this.buttonBioCASErefresh.Name = "buttonBioCASErefresh";
            this.buttonBioCASErefresh.Size = new System.Drawing.Size(24, 26);
            this.buttonBioCASErefresh.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonBioCASErefresh, "Reset to BioCASe sources and save path");
            this.buttonBioCASErefresh.UseVisualStyleBackColor = true;
            this.buttonBioCASErefresh.Click += new System.EventHandler(this.buttonBioCASErefresh_Click);
            // 
            // webBrowserBioCASE
            // 
            this.tableLayoutPanelBioCASE.SetColumnSpan(this.webBrowserBioCASE, 4);
            this.webBrowserBioCASE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserBioCASE.Location = new System.Drawing.Point(3, 29);
            this.webBrowserBioCASE.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserBioCASE.Name = "webBrowserBioCASE";
            this.webBrowserBioCASE.Size = new System.Drawing.Size(744, 524);
            this.webBrowserBioCASE.TabIndex = 3;
            this.webBrowserBioCASE.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowserBioCASE_Navigated);
            // 
            // buttonBioCASEback
            // 
            this.buttonBioCASEback.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonBioCASEback.Enabled = false;
            this.buttonBioCASEback.Image = global::DiversityCollection.Resource.ArrowPrevious1;
            this.buttonBioCASEback.Location = new System.Drawing.Point(730, 0);
            this.buttonBioCASEback.Margin = new System.Windows.Forms.Padding(0);
            this.buttonBioCASEback.Name = "buttonBioCASEback";
            this.buttonBioCASEback.Size = new System.Drawing.Size(20, 26);
            this.buttonBioCASEback.TabIndex = 4;
            this.toolTip.SetToolTip(this.buttonBioCASEback, "Back");
            this.buttonBioCASEback.UseVisualStyleBackColor = true;
            this.buttonBioCASEback.Click += new System.EventHandler(this.buttonBioCASEback_Click);
            // 
            // imageListTabControl
            // 
            this.imageListTabControl.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTabControl.ImageStream")));
            this.imageListTabControl.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTabControl.Images.SetKeyName(0, "Database.ico");
            this.imageListTabControl.Images.SetKeyName(1, "MergeUpdate2.ico");
            this.imageListTabControl.Images.SetKeyName(2, "Plant.ico");
            this.imageListTabControl.Images.SetKeyName(3, "Project.ico");
            this.imageListTabControl.Images.SetKeyName(4, "Specimen.ico");
            this.imageListTabControl.Images.SetKeyName(5, "Localisation.ico");
            this.imageListTabControl.Images.SetKeyName(6, "Hierarchy.ico");
            this.imageListTabControl.Images.SetKeyName(7, "NameAccepted.ico");
            this.imageListTabControl.Images.SetKeyName(8, "Settings.ico");
            this.imageListTabControl.Images.SetKeyName(9, "CacheDB.ico");
            this.imageListTabControl.Images.SetKeyName(10, "Postgres.ico");
            this.imageListTabControl.Images.SetKeyName(11, "BioCASE.ico");
            this.imageListTabControl.Images.SetKeyName(12, "Group.ico");
            this.imageListTabControl.Images.SetKeyName(13, "Speadsheet.ico");
            this.imageListTabControl.Images.SetKeyName(14, "Login.ico");
            this.imageListTabControl.Images.SetKeyName(15, "Package.ico");
            this.imageListTabControl.Images.SetKeyName(16, "Permission.ico");
            this.imageListTabControl.Images.SetKeyName(17, "AgentGrey.ico");
            // 
            // tabPageAdminCacheDB
            // 
            this.tabPageAdminCacheDB.Controls.Add(this.tabControlDatabase);
            this.helpProvider.SetHelpKeyword(this.tabPageAdminCacheDB, "Cache database configuration");
            this.helpProvider.SetHelpNavigator(this.tabPageAdminCacheDB, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.helpProvider.SetHelpString(this.tabPageAdminCacheDB, "Cache database configuration");
            this.tabPageAdminCacheDB.ImageIndex = 9;
            this.tabPageAdminCacheDB.Location = new System.Drawing.Point(4, 23);
            this.tabPageAdminCacheDB.Name = "tabPageAdminCacheDB";
            this.tabPageAdminCacheDB.Padding = new System.Windows.Forms.Padding(3);
            this.helpProvider.SetShowHelp(this.tabPageAdminCacheDB, true);
            this.tabPageAdminCacheDB.Size = new System.Drawing.Size(764, 589);
            this.tabPageAdminCacheDB.TabIndex = 1;
            this.tabPageAdminCacheDB.Text = "Administration";
            this.tabPageAdminCacheDB.UseVisualStyleBackColor = true;
            // 
            // tabControlDatabase
            // 
            this.tabControlDatabase.Controls.Add(this.tabPageUpdate);
            this.tabControlDatabase.Controls.Add(this.tabPageAnonymCollectors);
            this.tabControlDatabase.Controls.Add(this.tabPageProjects);
            this.tabControlDatabase.Controls.Add(this.tabPageLocalisation);
            this.tabControlDatabase.Controls.Add(this.tabPageTaxonSynonymy);
            this.tabControlDatabase.Controls.Add(this.tabPageMaterialCategory);
            this.tabControlDatabase.Controls.Add(this.tabPageKingdom);
            this.tabControlDatabase.Controls.Add(this.tabPageSettings);
            this.tabControlDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpProvider.SetHelpKeyword(this.tabControlDatabase, "Cache database configuration");
            this.helpProvider.SetHelpNavigator(this.tabControlDatabase, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.tabControlDatabase.ImageList = this.imageListTabControl;
            this.tabControlDatabase.Location = new System.Drawing.Point(3, 3);
            this.tabControlDatabase.Name = "tabControlDatabase";
            this.tabControlDatabase.SelectedIndex = 0;
            this.helpProvider.SetShowHelp(this.tabControlDatabase, true);
            this.tabControlDatabase.Size = new System.Drawing.Size(758, 583);
            this.tabControlDatabase.TabIndex = 2;
            // 
            // tabPageUpdate
            // 
            this.tabPageUpdate.Controls.Add(this.tableLayoutPanelDatabase);
            this.tabPageUpdate.ImageIndex = 1;
            this.tabPageUpdate.Location = new System.Drawing.Point(4, 23);
            this.tabPageUpdate.Name = "tabPageUpdate";
            this.tabPageUpdate.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUpdate.Size = new System.Drawing.Size(750, 556);
            this.tabPageUpdate.TabIndex = 0;
            this.tabPageUpdate.Text = "Version & Update";
            this.tabPageUpdate.UseVisualStyleBackColor = true;
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
            this.tableLayoutPanelDatabase.Controls.Add(this.buttonCheckProjectsDatabase, 0, 8);
            this.tableLayoutPanelDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpProvider.SetHelpKeyword(this.tableLayoutPanelDatabase, "Cache database configration");
            this.helpProvider.SetHelpNavigator(this.tableLayoutPanelDatabase, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.tableLayoutPanelDatabase.Location = new System.Drawing.Point(3, 3);
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
            this.helpProvider.SetShowHelp(this.tableLayoutPanelDatabase, true);
            this.tableLayoutPanelDatabase.Size = new System.Drawing.Size(744, 550);
            this.tableLayoutPanelDatabase.TabIndex = 1;
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
            this.textBoxServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxServer.Location = new System.Drawing.Point(172, 3);
            this.textBoxServer.Name = "textBoxServer";
            this.textBoxServer.ReadOnly = true;
            this.textBoxServer.Size = new System.Drawing.Size(569, 20);
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
            this.textBoxPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPort.Location = new System.Drawing.Point(172, 29);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.ReadOnly = true;
            this.textBoxPort.Size = new System.Drawing.Size(569, 20);
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
            this.textBoxDatabaseName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDatabaseName.Location = new System.Drawing.Point(172, 55);
            this.textBoxDatabaseName.Name = "textBoxDatabaseName";
            this.textBoxDatabaseName.ReadOnly = true;
            this.textBoxDatabaseName.Size = new System.Drawing.Size(569, 20);
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
            this.textBoxProjectsDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProjectsDatabase.Location = new System.Drawing.Point(172, 107);
            this.textBoxProjectsDatabase.Name = "textBoxProjectsDatabase";
            this.textBoxProjectsDatabase.ReadOnly = true;
            this.textBoxProjectsDatabase.Size = new System.Drawing.Size(569, 20);
            this.textBoxProjectsDatabase.TabIndex = 12;
            // 
            // textBoxCurrentDatabaseVersion
            // 
            this.tableLayoutPanelDatabase.SetColumnSpan(this.textBoxCurrentDatabaseVersion, 2);
            this.textBoxCurrentDatabaseVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCurrentDatabaseVersion.Location = new System.Drawing.Point(172, 81);
            this.textBoxCurrentDatabaseVersion.Name = "textBoxCurrentDatabaseVersion";
            this.textBoxCurrentDatabaseVersion.ReadOnly = true;
            this.textBoxCurrentDatabaseVersion.Size = new System.Drawing.Size(569, 20);
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
            this.textBoxAvailableDatabaseVersion.Size = new System.Drawing.Size(569, 20);
            this.textBoxAvailableDatabaseVersion.TabIndex = 15;
            // 
            // buttonCheckProjectsDatabase
            // 
            this.buttonCheckProjectsDatabase.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonCheckProjectsDatabase.ForeColor = System.Drawing.Color.Red;
            this.buttonCheckProjectsDatabase.Image = global::DiversityCollection.Resource.Project1;
            this.buttonCheckProjectsDatabase.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonCheckProjectsDatabase.Location = new System.Drawing.Point(3, 193);
            this.buttonCheckProjectsDatabase.Name = "buttonCheckProjectsDatabase";
            this.buttonCheckProjectsDatabase.Size = new System.Drawing.Size(163, 41);
            this.buttonCheckProjectsDatabase.TabIndex = 16;
            this.buttonCheckProjectsDatabase.Text = "Please check function ProjectsDatabase";
            this.buttonCheckProjectsDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonCheckProjectsDatabase, "Function retrieving the name of the project database may be wrong");
            this.buttonCheckProjectsDatabase.UseVisualStyleBackColor = true;
            this.buttonCheckProjectsDatabase.Visible = false;
            this.buttonCheckProjectsDatabase.Click += new System.EventHandler(this.buttonCheckProjectsDatabase_Click);
            // 
            // tabPageAnonymCollectors
            // 
            this.tabPageAnonymCollectors.Controls.Add(this.tableLayoutPanelAnonymCollector);
            this.tabPageAnonymCollectors.ImageIndex = 17;
            this.tabPageAnonymCollectors.Location = new System.Drawing.Point(4, 23);
            this.tabPageAnonymCollectors.Name = "tabPageAnonymCollectors";
            this.tabPageAnonymCollectors.Size = new System.Drawing.Size(750, 556);
            this.tabPageAnonymCollectors.TabIndex = 7;
            this.tabPageAnonymCollectors.Text = "Anonym collectors";
            this.tabPageAnonymCollectors.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelAnonymCollector
            // 
            this.tableLayoutPanelAnonymCollector.ColumnCount = 3;
            this.tableLayoutPanelAnonymCollector.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAnonymCollector.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAnonymCollector.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAnonymCollector.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelAnonymCollector.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelAnonymCollector.Controls.Add(this.labelCollectorNotAnomym, 0, 0);
            this.tableLayoutPanelAnonymCollector.Controls.Add(this.labelCollectorAnonym, 2, 0);
            this.tableLayoutPanelAnonymCollector.Controls.Add(this.dataGridViewAnonymAgent, 2, 1);
            this.tableLayoutPanelAnonymCollector.Controls.Add(this.listBoxAnonymCollector, 0, 1);
            this.tableLayoutPanelAnonymCollector.Controls.Add(this.buttonCollectorIsAnonym, 1, 1);
            this.tableLayoutPanelAnonymCollector.Controls.Add(this.buttonCollectorNotAnonym, 1, 2);
            this.tableLayoutPanelAnonymCollector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelAnonymCollector.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelAnonymCollector.Name = "tableLayoutPanelAnonymCollector";
            this.tableLayoutPanelAnonymCollector.RowCount = 3;
            this.tableLayoutPanelAnonymCollector.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAnonymCollector.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelAnonymCollector.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelAnonymCollector.Size = new System.Drawing.Size(750, 556);
            this.tableLayoutPanelAnonymCollector.TabIndex = 0;
            // 
            // labelCollectorNotAnomym
            // 
            this.labelCollectorNotAnomym.AutoSize = true;
            this.labelCollectorNotAnomym.Location = new System.Drawing.Point(3, 0);
            this.labelCollectorNotAnomym.Name = "labelCollectorNotAnomym";
            this.labelCollectorNotAnomym.Size = new System.Drawing.Size(112, 13);
            this.labelCollectorNotAnomym.TabIndex = 0;
            this.labelCollectorNotAnomym.Text = "Not anonym collectors";
            // 
            // labelCollectorAnonym
            // 
            this.labelCollectorAnonym.AutoSize = true;
            this.labelCollectorAnonym.Location = new System.Drawing.Point(160, 0);
            this.labelCollectorAnonym.Name = "labelCollectorAnonym";
            this.labelCollectorAnonym.Size = new System.Drawing.Size(93, 13);
            this.labelCollectorAnonym.TabIndex = 1;
            this.labelCollectorAnonym.Text = "Anonym collectors";
            // 
            // dataGridViewAnonymAgent
            // 
            this.dataGridViewAnonymAgent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAnonymAgent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewAnonymAgent.Location = new System.Drawing.Point(160, 16);
            this.dataGridViewAnonymAgent.Name = "dataGridViewAnonymAgent";
            this.tableLayoutPanelAnonymCollector.SetRowSpan(this.dataGridViewAnonymAgent, 2);
            this.dataGridViewAnonymAgent.Size = new System.Drawing.Size(587, 537);
            this.dataGridViewAnonymAgent.TabIndex = 2;
            // 
            // listBoxAnonymCollector
            // 
            this.listBoxAnonymCollector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxAnonymCollector.FormattingEnabled = true;
            this.listBoxAnonymCollector.Location = new System.Drawing.Point(3, 16);
            this.listBoxAnonymCollector.Name = "listBoxAnonymCollector";
            this.tableLayoutPanelAnonymCollector.SetRowSpan(this.listBoxAnonymCollector, 2);
            this.listBoxAnonymCollector.Size = new System.Drawing.Size(120, 537);
            this.listBoxAnonymCollector.TabIndex = 3;
            // 
            // buttonCollectorIsAnonym
            // 
            this.buttonCollectorIsAnonym.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonCollectorIsAnonym.Location = new System.Drawing.Point(129, 258);
            this.buttonCollectorIsAnonym.Name = "buttonCollectorIsAnonym";
            this.buttonCollectorIsAnonym.Size = new System.Drawing.Size(25, 23);
            this.buttonCollectorIsAnonym.TabIndex = 4;
            this.buttonCollectorIsAnonym.Text = ">";
            this.buttonCollectorIsAnonym.UseVisualStyleBackColor = true;
            this.buttonCollectorIsAnonym.Click += new System.EventHandler(this.buttonCollectorIsAnonym_Click);
            // 
            // buttonCollectorNotAnonym
            // 
            this.buttonCollectorNotAnonym.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonCollectorNotAnonym.Location = new System.Drawing.Point(129, 287);
            this.buttonCollectorNotAnonym.Name = "buttonCollectorNotAnonym";
            this.buttonCollectorNotAnonym.Size = new System.Drawing.Size(25, 23);
            this.buttonCollectorNotAnonym.TabIndex = 5;
            this.buttonCollectorNotAnonym.Text = "<";
            this.buttonCollectorNotAnonym.UseVisualStyleBackColor = true;
            this.buttonCollectorNotAnonym.Click += new System.EventHandler(this.buttonCollectorNotAnonym_Click);
            // 
            // tabPageProjects
            // 
            this.tabPageProjects.Controls.Add(this.splitContainerProjects);
            this.tabPageProjects.ImageIndex = 3;
            this.tabPageProjects.Location = new System.Drawing.Point(4, 23);
            this.tabPageProjects.Name = "tabPageProjects";
            this.tabPageProjects.Size = new System.Drawing.Size(750, 556);
            this.tabPageProjects.TabIndex = 6;
            this.tabPageProjects.Text = "Projects";
            this.tabPageProjects.UseVisualStyleBackColor = true;
            // 
            // splitContainerProjects
            // 
            this.splitContainerProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpProvider.SetHelpKeyword(this.splitContainerProjects, "Cache database configuration");
            this.helpProvider.SetHelpNavigator(this.splitContainerProjects, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.splitContainerProjects.Location = new System.Drawing.Point(0, 0);
            this.splitContainerProjects.Name = "splitContainerProjects";
            // 
            // splitContainerProjects.Panel1
            // 
            this.splitContainerProjects.Panel1.Controls.Add(this.listBoxProjects);
            this.splitContainerProjects.Panel1.Controls.Add(this.toolStripAdministrationProjects);
            // 
            // splitContainerProjects.Panel2
            // 
            this.splitContainerProjects.Panel2.Controls.Add(this.tabControlProjectAdminTaxMat);
            this.splitContainerProjects.Panel2.Controls.Add(this.groupBoxProjectSettings);
            this.splitContainerProjects.Panel2.Controls.Add(this.groupBoxProjectCoordinates);
            this.splitContainerProjects.Panel2.Controls.Add(this.groupBoxProjectImages);
            this.helpProvider.SetShowHelp(this.splitContainerProjects, true);
            this.splitContainerProjects.Size = new System.Drawing.Size(750, 556);
            this.splitContainerProjects.SplitterDistance = 190;
            this.splitContainerProjects.TabIndex = 1;
            // 
            // listBoxProjects
            // 
            this.listBoxProjects.DataSource = this.projectPublishedBindingSource;
            this.listBoxProjects.DisplayMember = "Project";
            this.listBoxProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjects.FormattingEnabled = true;
            this.listBoxProjects.Location = new System.Drawing.Point(0, 0);
            this.listBoxProjects.Name = "listBoxProjects";
            this.listBoxProjects.Size = new System.Drawing.Size(190, 531);
            this.listBoxProjects.TabIndex = 1;
            this.listBoxProjects.ValueMember = "ProjectID";
            this.listBoxProjects.SelectedIndexChanged += new System.EventHandler(this.listBoxProjects_SelectedIndexChanged);
            // 
            // toolStripAdministrationProjects
            // 
            this.toolStripAdministrationProjects.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripAdministrationProjects.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripAdministrationProjects.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonProjectAdd,
            this.toolStripButtonProjectRemove});
            this.toolStripAdministrationProjects.Location = new System.Drawing.Point(0, 531);
            this.toolStripAdministrationProjects.Name = "toolStripAdministrationProjects";
            this.toolStripAdministrationProjects.Size = new System.Drawing.Size(190, 25);
            this.toolStripAdministrationProjects.TabIndex = 2;
            this.toolStripAdministrationProjects.Text = "toolStrip1";
            // 
            // toolStripButtonProjectAdd
            // 
            this.toolStripButtonProjectAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonProjectAdd.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonProjectAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonProjectAdd.Name = "toolStripButtonProjectAdd";
            this.toolStripButtonProjectAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonProjectAdd.Text = "toolStripButton1";
            this.toolStripButtonProjectAdd.Click += new System.EventHandler(this.toolStripButtonProjectAdd_Click);
            // 
            // toolStripButtonProjectRemove
            // 
            this.toolStripButtonProjectRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonProjectRemove.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonProjectRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonProjectRemove.Name = "toolStripButtonProjectRemove";
            this.toolStripButtonProjectRemove.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonProjectRemove.Text = "toolStripButton1";
            this.toolStripButtonProjectRemove.Click += new System.EventHandler(this.toolStripButtonProjectRemove_Click);
            // 
            // tabControlProjectAdminTaxMat
            // 
            this.tabControlProjectAdminTaxMat.Controls.Add(this.tabPageProjectTaxonomicGroup);
            this.tabControlProjectAdminTaxMat.Controls.Add(this.tabPageProjectMaterialCategory);
            this.tabControlProjectAdminTaxMat.Controls.Add(this.tabPageProjectLocalisationsystem);
            this.tabControlProjectAdminTaxMat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlProjectAdminTaxMat.ImageList = this.imageListTabControl;
            this.tabControlProjectAdminTaxMat.Location = new System.Drawing.Point(0, 135);
            this.tabControlProjectAdminTaxMat.Name = "tabControlProjectAdminTaxMat";
            this.tabControlProjectAdminTaxMat.SelectedIndex = 0;
            this.tabControlProjectAdminTaxMat.Size = new System.Drawing.Size(556, 421);
            this.tabControlProjectAdminTaxMat.TabIndex = 10;
            // 
            // tabPageProjectTaxonomicGroup
            // 
            this.tabPageProjectTaxonomicGroup.Controls.Add(this.tableLayoutPanelTaxonomicGroups);
            this.tabPageProjectTaxonomicGroup.ImageIndex = 2;
            this.tabPageProjectTaxonomicGroup.Location = new System.Drawing.Point(4, 23);
            this.tabPageProjectTaxonomicGroup.Name = "tabPageProjectTaxonomicGroup";
            this.tabPageProjectTaxonomicGroup.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProjectTaxonomicGroup.Size = new System.Drawing.Size(548, 394);
            this.tabPageProjectTaxonomicGroup.TabIndex = 0;
            this.tabPageProjectTaxonomicGroup.Text = "Taxonomic groups";
            this.tabPageProjectTaxonomicGroup.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelTaxonomicGroups
            // 
            this.tableLayoutPanelTaxonomicGroups.ColumnCount = 4;
            this.tableLayoutPanelTaxonomicGroups.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTaxonomicGroups.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelTaxonomicGroups.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelTaxonomicGroups.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelTaxonomicGroups.Controls.Add(this.labelProjectTaxonomicGroup, 0, 0);
            this.tableLayoutPanelTaxonomicGroups.Controls.Add(this.labelProjectTaxonomicGroupNotPublished, 0, 1);
            this.tableLayoutPanelTaxonomicGroups.Controls.Add(this.labelProjectTaxonomicGroupPublished, 3, 1);
            this.tableLayoutPanelTaxonomicGroups.Controls.Add(this.listBoxProjectTaxonomicGroupNotPublished, 0, 2);
            this.tableLayoutPanelTaxonomicGroups.Controls.Add(this.listBoxProjectTaxonomicGroupPublished, 3, 2);
            this.tableLayoutPanelTaxonomicGroups.Controls.Add(this.buttonProjectTaxonomicGroupPublished, 2, 2);
            this.tableLayoutPanelTaxonomicGroups.Controls.Add(this.buttonProjectTaxonomicGroupNotPublished, 2, 3);
            this.tableLayoutPanelTaxonomicGroups.Controls.Add(this.buttonProjectTaxonomicGroupTransferExisting, 2, 0);
            this.tableLayoutPanelTaxonomicGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTaxonomicGroups.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelTaxonomicGroups.Name = "tableLayoutPanelTaxonomicGroups";
            this.tableLayoutPanelTaxonomicGroups.RowCount = 4;
            this.tableLayoutPanelTaxonomicGroups.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTaxonomicGroups.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTaxonomicGroups.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelTaxonomicGroups.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelTaxonomicGroups.Size = new System.Drawing.Size(542, 388);
            this.tableLayoutPanelTaxonomicGroups.TabIndex = 2;
            // 
            // labelProjectTaxonomicGroup
            // 
            this.labelProjectTaxonomicGroup.AutoSize = true;
            this.tableLayoutPanelTaxonomicGroups.SetColumnSpan(this.labelProjectTaxonomicGroup, 2);
            this.labelProjectTaxonomicGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProjectTaxonomicGroup.Location = new System.Drawing.Point(3, 3);
            this.labelProjectTaxonomicGroup.Margin = new System.Windows.Forms.Padding(3);
            this.labelProjectTaxonomicGroup.Name = "labelProjectTaxonomicGroup";
            this.labelProjectTaxonomicGroup.Size = new System.Drawing.Size(322, 23);
            this.labelProjectTaxonomicGroup.TabIndex = 1;
            this.labelProjectTaxonomicGroup.Text = "Taxonomic groups transfered into the cache database";
            this.labelProjectTaxonomicGroup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelProjectTaxonomicGroupNotPublished
            // 
            this.labelProjectTaxonomicGroupNotPublished.AutoSize = true;
            this.tableLayoutPanelTaxonomicGroups.SetColumnSpan(this.labelProjectTaxonomicGroupNotPublished, 2);
            this.labelProjectTaxonomicGroupNotPublished.Location = new System.Drawing.Point(3, 29);
            this.labelProjectTaxonomicGroupNotPublished.Name = "labelProjectTaxonomicGroupNotPublished";
            this.labelProjectTaxonomicGroupNotPublished.Size = new System.Drawing.Size(72, 13);
            this.labelProjectTaxonomicGroupNotPublished.TabIndex = 5;
            this.labelProjectTaxonomicGroupNotPublished.Text = "Not published";
            // 
            // labelProjectTaxonomicGroupPublished
            // 
            this.labelProjectTaxonomicGroupPublished.AutoSize = true;
            this.labelProjectTaxonomicGroupPublished.Location = new System.Drawing.Point(351, 29);
            this.labelProjectTaxonomicGroupPublished.Name = "labelProjectTaxonomicGroupPublished";
            this.labelProjectTaxonomicGroupPublished.Size = new System.Drawing.Size(53, 13);
            this.labelProjectTaxonomicGroupPublished.TabIndex = 6;
            this.labelProjectTaxonomicGroupPublished.Text = "Published";
            // 
            // listBoxProjectTaxonomicGroupNotPublished
            // 
            this.listBoxProjectTaxonomicGroupNotPublished.BackColor = System.Drawing.Color.Pink;
            this.tableLayoutPanelTaxonomicGroups.SetColumnSpan(this.listBoxProjectTaxonomicGroupNotPublished, 2);
            this.listBoxProjectTaxonomicGroupNotPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjectTaxonomicGroupNotPublished.FormattingEnabled = true;
            this.listBoxProjectTaxonomicGroupNotPublished.Location = new System.Drawing.Point(3, 45);
            this.listBoxProjectTaxonomicGroupNotPublished.Name = "listBoxProjectTaxonomicGroupNotPublished";
            this.tableLayoutPanelTaxonomicGroups.SetRowSpan(this.listBoxProjectTaxonomicGroupNotPublished, 2);
            this.listBoxProjectTaxonomicGroupNotPublished.Size = new System.Drawing.Size(322, 340);
            this.listBoxProjectTaxonomicGroupNotPublished.Sorted = true;
            this.listBoxProjectTaxonomicGroupNotPublished.TabIndex = 7;
            // 
            // listBoxProjectTaxonomicGroupPublished
            // 
            this.listBoxProjectTaxonomicGroupPublished.BackColor = System.Drawing.Color.LightGreen;
            this.listBoxProjectTaxonomicGroupPublished.DataSource = this.taxonomicGroupInProjectBindingSource;
            this.listBoxProjectTaxonomicGroupPublished.DisplayMember = "TaxonomicGroup";
            this.listBoxProjectTaxonomicGroupPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjectTaxonomicGroupPublished.FormattingEnabled = true;
            this.listBoxProjectTaxonomicGroupPublished.Location = new System.Drawing.Point(351, 45);
            this.listBoxProjectTaxonomicGroupPublished.Name = "listBoxProjectTaxonomicGroupPublished";
            this.tableLayoutPanelTaxonomicGroups.SetRowSpan(this.listBoxProjectTaxonomicGroupPublished, 2);
            this.listBoxProjectTaxonomicGroupPublished.Size = new System.Drawing.Size(188, 340);
            this.listBoxProjectTaxonomicGroupPublished.Sorted = true;
            this.listBoxProjectTaxonomicGroupPublished.TabIndex = 8;
            // 
            // taxonomicGroupInProjectBindingSource
            // 
            this.taxonomicGroupInProjectBindingSource.DataMember = "TaxonomicGroupInProject";
            this.taxonomicGroupInProjectBindingSource.DataSource = this.dataSetCacheDB_1;
            // 
            // buttonProjectTaxonomicGroupPublished
            // 
            this.buttonProjectTaxonomicGroupPublished.BackColor = System.Drawing.Color.LightGreen;
            this.buttonProjectTaxonomicGroupPublished.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonProjectTaxonomicGroupPublished.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonProjectTaxonomicGroupPublished.ForeColor = System.Drawing.Color.Green;
            this.buttonProjectTaxonomicGroupPublished.Location = new System.Drawing.Point(328, 192);
            this.buttonProjectTaxonomicGroupPublished.Margin = new System.Windows.Forms.Padding(0);
            this.buttonProjectTaxonomicGroupPublished.Name = "buttonProjectTaxonomicGroupPublished";
            this.buttonProjectTaxonomicGroupPublished.Size = new System.Drawing.Size(20, 23);
            this.buttonProjectTaxonomicGroupPublished.TabIndex = 9;
            this.buttonProjectTaxonomicGroupPublished.Text = ">";
            this.toolTip.SetToolTip(this.buttonProjectTaxonomicGroupPublished, "Publish the selected taxonomic group");
            this.buttonProjectTaxonomicGroupPublished.UseVisualStyleBackColor = false;
            this.buttonProjectTaxonomicGroupPublished.Click += new System.EventHandler(this.buttonProjectTaxonomicGroupPublished_Click);
            // 
            // buttonProjectTaxonomicGroupNotPublished
            // 
            this.buttonProjectTaxonomicGroupNotPublished.BackColor = System.Drawing.Color.Pink;
            this.buttonProjectTaxonomicGroupNotPublished.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonProjectTaxonomicGroupNotPublished.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonProjectTaxonomicGroupNotPublished.ForeColor = System.Drawing.Color.Red;
            this.buttonProjectTaxonomicGroupNotPublished.Location = new System.Drawing.Point(328, 215);
            this.buttonProjectTaxonomicGroupNotPublished.Margin = new System.Windows.Forms.Padding(0);
            this.buttonProjectTaxonomicGroupNotPublished.Name = "buttonProjectTaxonomicGroupNotPublished";
            this.buttonProjectTaxonomicGroupNotPublished.Size = new System.Drawing.Size(20, 23);
            this.buttonProjectTaxonomicGroupNotPublished.TabIndex = 10;
            this.buttonProjectTaxonomicGroupNotPublished.Text = "<";
            this.toolTip.SetToolTip(this.buttonProjectTaxonomicGroupNotPublished, "Hide the selected taxonomic grroup");
            this.buttonProjectTaxonomicGroupNotPublished.UseVisualStyleBackColor = false;
            this.buttonProjectTaxonomicGroupNotPublished.Click += new System.EventHandler(this.buttonProjectTaxonomicGroupNotPublished_Click);
            // 
            // buttonProjectTaxonomicGroupTransferExisting
            // 
            this.tableLayoutPanelTaxonomicGroups.SetColumnSpan(this.buttonProjectTaxonomicGroupTransferExisting, 2);
            this.buttonProjectTaxonomicGroupTransferExisting.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonProjectTaxonomicGroupTransferExisting.Image = global::DiversityCollection.Resource.ArrowNext1;
            this.buttonProjectTaxonomicGroupTransferExisting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonProjectTaxonomicGroupTransferExisting.Location = new System.Drawing.Point(331, 3);
            this.buttonProjectTaxonomicGroupTransferExisting.Name = "buttonProjectTaxonomicGroupTransferExisting";
            this.buttonProjectTaxonomicGroupTransferExisting.Size = new System.Drawing.Size(109, 23);
            this.buttonProjectTaxonomicGroupTransferExisting.TabIndex = 11;
            this.buttonProjectTaxonomicGroupTransferExisting.Text = "Transfer existing";
            this.buttonProjectTaxonomicGroupTransferExisting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.buttonProjectTaxonomicGroupTransferExisting, "Transfers all taxonomic groups that exist in the current project and are publishe" +
        "d in the cache database");
            this.buttonProjectTaxonomicGroupTransferExisting.UseVisualStyleBackColor = true;
            this.buttonProjectTaxonomicGroupTransferExisting.Click += new System.EventHandler(this.buttonProjectTaxonomicGroupTransferExisting_Click);
            // 
            // tabPageProjectMaterialCategory
            // 
            this.tabPageProjectMaterialCategory.Controls.Add(this.tableLayoutPanelProjectMaterialCategory);
            this.tabPageProjectMaterialCategory.ImageIndex = 4;
            this.tabPageProjectMaterialCategory.Location = new System.Drawing.Point(4, 23);
            this.tabPageProjectMaterialCategory.Name = "tabPageProjectMaterialCategory";
            this.tabPageProjectMaterialCategory.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProjectMaterialCategory.Size = new System.Drawing.Size(548, 394);
            this.tabPageProjectMaterialCategory.TabIndex = 1;
            this.tabPageProjectMaterialCategory.Text = "Material category";
            this.tabPageProjectMaterialCategory.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelProjectMaterialCategory
            // 
            this.tableLayoutPanelProjectMaterialCategory.ColumnCount = 4;
            this.tableLayoutPanelProjectMaterialCategory.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelProjectMaterialCategory.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelProjectMaterialCategory.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelProjectMaterialCategory.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelProjectMaterialCategory.Controls.Add(this.labelProjectMaterialCategoryHeader, 0, 0);
            this.tableLayoutPanelProjectMaterialCategory.Controls.Add(this.labelProjectMaterialCategoryNotPublished, 0, 1);
            this.tableLayoutPanelProjectMaterialCategory.Controls.Add(this.labelProjectMaterialCategoryPublished, 3, 1);
            this.tableLayoutPanelProjectMaterialCategory.Controls.Add(this.listBoxProjectMaterialCategoryNotPublished, 0, 2);
            this.tableLayoutPanelProjectMaterialCategory.Controls.Add(this.listBoxProjectMaterialCategoryPublished, 3, 2);
            this.tableLayoutPanelProjectMaterialCategory.Controls.Add(this.buttonProjectMaterialCategoryPublishe, 2, 2);
            this.tableLayoutPanelProjectMaterialCategory.Controls.Add(this.buttonProjectMaterialCategoryWithhold, 2, 3);
            this.tableLayoutPanelProjectMaterialCategory.Controls.Add(this.buttonProjectMaterialCategoryTransferExisting, 2, 0);
            this.tableLayoutPanelProjectMaterialCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelProjectMaterialCategory.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelProjectMaterialCategory.Name = "tableLayoutPanelProjectMaterialCategory";
            this.tableLayoutPanelProjectMaterialCategory.RowCount = 4;
            this.tableLayoutPanelProjectMaterialCategory.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProjectMaterialCategory.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProjectMaterialCategory.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelProjectMaterialCategory.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelProjectMaterialCategory.Size = new System.Drawing.Size(542, 388);
            this.tableLayoutPanelProjectMaterialCategory.TabIndex = 3;
            // 
            // labelProjectMaterialCategoryHeader
            // 
            this.labelProjectMaterialCategoryHeader.AutoSize = true;
            this.tableLayoutPanelProjectMaterialCategory.SetColumnSpan(this.labelProjectMaterialCategoryHeader, 2);
            this.labelProjectMaterialCategoryHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProjectMaterialCategoryHeader.Location = new System.Drawing.Point(3, 3);
            this.labelProjectMaterialCategoryHeader.Margin = new System.Windows.Forms.Padding(3);
            this.labelProjectMaterialCategoryHeader.Name = "labelProjectMaterialCategoryHeader";
            this.labelProjectMaterialCategoryHeader.Size = new System.Drawing.Size(322, 23);
            this.labelProjectMaterialCategoryHeader.TabIndex = 1;
            this.labelProjectMaterialCategoryHeader.Text = "Material categories transfered into the cache database";
            this.labelProjectMaterialCategoryHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelProjectMaterialCategoryNotPublished
            // 
            this.labelProjectMaterialCategoryNotPublished.AutoSize = true;
            this.tableLayoutPanelProjectMaterialCategory.SetColumnSpan(this.labelProjectMaterialCategoryNotPublished, 2);
            this.labelProjectMaterialCategoryNotPublished.Location = new System.Drawing.Point(3, 29);
            this.labelProjectMaterialCategoryNotPublished.Name = "labelProjectMaterialCategoryNotPublished";
            this.labelProjectMaterialCategoryNotPublished.Size = new System.Drawing.Size(72, 13);
            this.labelProjectMaterialCategoryNotPublished.TabIndex = 5;
            this.labelProjectMaterialCategoryNotPublished.Text = "Not published";
            // 
            // labelProjectMaterialCategoryPublished
            // 
            this.labelProjectMaterialCategoryPublished.AutoSize = true;
            this.labelProjectMaterialCategoryPublished.Location = new System.Drawing.Point(351, 29);
            this.labelProjectMaterialCategoryPublished.Name = "labelProjectMaterialCategoryPublished";
            this.labelProjectMaterialCategoryPublished.Size = new System.Drawing.Size(53, 13);
            this.labelProjectMaterialCategoryPublished.TabIndex = 6;
            this.labelProjectMaterialCategoryPublished.Text = "Published";
            // 
            // listBoxProjectMaterialCategoryNotPublished
            // 
            this.listBoxProjectMaterialCategoryNotPublished.BackColor = System.Drawing.Color.Pink;
            this.tableLayoutPanelProjectMaterialCategory.SetColumnSpan(this.listBoxProjectMaterialCategoryNotPublished, 2);
            this.listBoxProjectMaterialCategoryNotPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjectMaterialCategoryNotPublished.FormattingEnabled = true;
            this.listBoxProjectMaterialCategoryNotPublished.Location = new System.Drawing.Point(3, 45);
            this.listBoxProjectMaterialCategoryNotPublished.Name = "listBoxProjectMaterialCategoryNotPublished";
            this.tableLayoutPanelProjectMaterialCategory.SetRowSpan(this.listBoxProjectMaterialCategoryNotPublished, 2);
            this.listBoxProjectMaterialCategoryNotPublished.Size = new System.Drawing.Size(322, 340);
            this.listBoxProjectMaterialCategoryNotPublished.Sorted = true;
            this.listBoxProjectMaterialCategoryNotPublished.TabIndex = 7;
            // 
            // listBoxProjectMaterialCategoryPublished
            // 
            this.listBoxProjectMaterialCategoryPublished.BackColor = System.Drawing.Color.LightGreen;
            this.listBoxProjectMaterialCategoryPublished.DataSource = this.taxonomicGroupInProjectBindingSource;
            this.listBoxProjectMaterialCategoryPublished.DisplayMember = "TaxonomicGroup";
            this.listBoxProjectMaterialCategoryPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjectMaterialCategoryPublished.FormattingEnabled = true;
            this.listBoxProjectMaterialCategoryPublished.Location = new System.Drawing.Point(351, 45);
            this.listBoxProjectMaterialCategoryPublished.Name = "listBoxProjectMaterialCategoryPublished";
            this.tableLayoutPanelProjectMaterialCategory.SetRowSpan(this.listBoxProjectMaterialCategoryPublished, 2);
            this.listBoxProjectMaterialCategoryPublished.Size = new System.Drawing.Size(188, 340);
            this.listBoxProjectMaterialCategoryPublished.Sorted = true;
            this.listBoxProjectMaterialCategoryPublished.TabIndex = 8;
            // 
            // buttonProjectMaterialCategoryPublishe
            // 
            this.buttonProjectMaterialCategoryPublishe.BackColor = System.Drawing.Color.LightGreen;
            this.buttonProjectMaterialCategoryPublishe.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonProjectMaterialCategoryPublishe.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonProjectMaterialCategoryPublishe.ForeColor = System.Drawing.Color.Green;
            this.buttonProjectMaterialCategoryPublishe.Location = new System.Drawing.Point(328, 192);
            this.buttonProjectMaterialCategoryPublishe.Margin = new System.Windows.Forms.Padding(0);
            this.buttonProjectMaterialCategoryPublishe.Name = "buttonProjectMaterialCategoryPublishe";
            this.buttonProjectMaterialCategoryPublishe.Size = new System.Drawing.Size(20, 23);
            this.buttonProjectMaterialCategoryPublishe.TabIndex = 9;
            this.buttonProjectMaterialCategoryPublishe.Text = ">";
            this.toolTip.SetToolTip(this.buttonProjectMaterialCategoryPublishe, "Publish the selected material category");
            this.buttonProjectMaterialCategoryPublishe.UseVisualStyleBackColor = false;
            this.buttonProjectMaterialCategoryPublishe.Click += new System.EventHandler(this.buttonProjectMaterialCategoryPublishe_Click);
            // 
            // buttonProjectMaterialCategoryWithhold
            // 
            this.buttonProjectMaterialCategoryWithhold.BackColor = System.Drawing.Color.Pink;
            this.buttonProjectMaterialCategoryWithhold.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonProjectMaterialCategoryWithhold.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonProjectMaterialCategoryWithhold.ForeColor = System.Drawing.Color.Red;
            this.buttonProjectMaterialCategoryWithhold.Location = new System.Drawing.Point(328, 215);
            this.buttonProjectMaterialCategoryWithhold.Margin = new System.Windows.Forms.Padding(0);
            this.buttonProjectMaterialCategoryWithhold.Name = "buttonProjectMaterialCategoryWithhold";
            this.buttonProjectMaterialCategoryWithhold.Size = new System.Drawing.Size(20, 23);
            this.buttonProjectMaterialCategoryWithhold.TabIndex = 10;
            this.buttonProjectMaterialCategoryWithhold.Text = "<";
            this.toolTip.SetToolTip(this.buttonProjectMaterialCategoryWithhold, "Hide the selected material category");
            this.buttonProjectMaterialCategoryWithhold.UseVisualStyleBackColor = false;
            this.buttonProjectMaterialCategoryWithhold.Click += new System.EventHandler(this.buttonProjectMaterialCategoryWithhold_Click);
            // 
            // buttonProjectMaterialCategoryTransferExisting
            // 
            this.tableLayoutPanelProjectMaterialCategory.SetColumnSpan(this.buttonProjectMaterialCategoryTransferExisting, 2);
            this.buttonProjectMaterialCategoryTransferExisting.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonProjectMaterialCategoryTransferExisting.Image = global::DiversityCollection.Resource.ArrowNext1;
            this.buttonProjectMaterialCategoryTransferExisting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonProjectMaterialCategoryTransferExisting.Location = new System.Drawing.Point(331, 3);
            this.buttonProjectMaterialCategoryTransferExisting.Name = "buttonProjectMaterialCategoryTransferExisting";
            this.buttonProjectMaterialCategoryTransferExisting.Size = new System.Drawing.Size(109, 23);
            this.buttonProjectMaterialCategoryTransferExisting.TabIndex = 11;
            this.buttonProjectMaterialCategoryTransferExisting.Text = "Transfer existing";
            this.buttonProjectMaterialCategoryTransferExisting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.buttonProjectMaterialCategoryTransferExisting, "Transfers all mateial categories that exist in the current project and are publis" +
        "hed in the cache database");
            this.buttonProjectMaterialCategoryTransferExisting.UseVisualStyleBackColor = true;
            this.buttonProjectMaterialCategoryTransferExisting.Click += new System.EventHandler(this.buttonProjectMaterialCategoryTransferExisting_Click);
            // 
            // tabPageProjectLocalisationsystem
            // 
            this.tabPageProjectLocalisationsystem.Controls.Add(this.tableLayoutPanelProjectLocalisation);
            this.tabPageProjectLocalisationsystem.ImageIndex = 5;
            this.tabPageProjectLocalisationsystem.Location = new System.Drawing.Point(4, 23);
            this.tabPageProjectLocalisationsystem.Name = "tabPageProjectLocalisationsystem";
            this.tabPageProjectLocalisationsystem.Size = new System.Drawing.Size(548, 394);
            this.tabPageProjectLocalisationsystem.TabIndex = 2;
            this.tabPageProjectLocalisationsystem.Text = "Localisation";
            this.tabPageProjectLocalisationsystem.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelProjectLocalisation
            // 
            this.tableLayoutPanelProjectLocalisation.ColumnCount = 4;
            this.tableLayoutPanelProjectLocalisation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelProjectLocalisation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelProjectLocalisation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelProjectLocalisation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelProjectLocalisation.Controls.Add(this.labelProjectLocalisationNotPublished, 0, 1);
            this.tableLayoutPanelProjectLocalisation.Controls.Add(this.listBoxProjectLocalisationNotPublished, 0, 2);
            this.tableLayoutPanelProjectLocalisation.Controls.Add(this.labelProjectLocalisationPublished, 2, 1);
            this.tableLayoutPanelProjectLocalisation.Controls.Add(this.listBoxProjectLocalisationPubished, 2, 2);
            this.tableLayoutPanelProjectLocalisation.Controls.Add(this.buttonProjectLocalisationPublish, 1, 2);
            this.tableLayoutPanelProjectLocalisation.Controls.Add(this.buttonProjectLocalisationHide, 1, 3);
            this.tableLayoutPanelProjectLocalisation.Controls.Add(this.buttonProjectLocalisationUp, 2, 4);
            this.tableLayoutPanelProjectLocalisation.Controls.Add(this.buttonProjectLocalisationDown, 3, 4);
            this.tableLayoutPanelProjectLocalisation.Controls.Add(this.labelProjectLocalisationHeader, 0, 0);
            this.tableLayoutPanelProjectLocalisation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelProjectLocalisation.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelProjectLocalisation.Name = "tableLayoutPanelProjectLocalisation";
            this.tableLayoutPanelProjectLocalisation.RowCount = 5;
            this.tableLayoutPanelProjectLocalisation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelProjectLocalisation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProjectLocalisation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelProjectLocalisation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelProjectLocalisation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelProjectLocalisation.Size = new System.Drawing.Size(548, 394);
            this.tableLayoutPanelProjectLocalisation.TabIndex = 1;
            // 
            // labelProjectLocalisationNotPublished
            // 
            this.labelProjectLocalisationNotPublished.AutoSize = true;
            this.labelProjectLocalisationNotPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProjectLocalisationNotPublished.Location = new System.Drawing.Point(3, 23);
            this.labelProjectLocalisationNotPublished.Margin = new System.Windows.Forms.Padding(3);
            this.labelProjectLocalisationNotPublished.Name = "labelProjectLocalisationNotPublished";
            this.labelProjectLocalisationNotPublished.Size = new System.Drawing.Size(258, 13);
            this.labelProjectLocalisationNotPublished.TabIndex = 0;
            this.labelProjectLocalisationNotPublished.Text = "Not published";
            // 
            // listBoxProjectLocalisationNotPublished
            // 
            this.listBoxProjectLocalisationNotPublished.BackColor = System.Drawing.Color.Pink;
            this.listBoxProjectLocalisationNotPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjectLocalisationNotPublished.FormattingEnabled = true;
            this.listBoxProjectLocalisationNotPublished.Location = new System.Drawing.Point(3, 42);
            this.listBoxProjectLocalisationNotPublished.Name = "listBoxProjectLocalisationNotPublished";
            this.tableLayoutPanelProjectLocalisation.SetRowSpan(this.listBoxProjectLocalisationNotPublished, 3);
            this.listBoxProjectLocalisationNotPublished.Size = new System.Drawing.Size(258, 349);
            this.listBoxProjectLocalisationNotPublished.TabIndex = 2;
            // 
            // labelProjectLocalisationPublished
            // 
            this.labelProjectLocalisationPublished.AutoSize = true;
            this.labelProjectLocalisationPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProjectLocalisationPublished.Location = new System.Drawing.Point(287, 23);
            this.labelProjectLocalisationPublished.Margin = new System.Windows.Forms.Padding(3);
            this.labelProjectLocalisationPublished.Name = "labelProjectLocalisationPublished";
            this.labelProjectLocalisationPublished.Size = new System.Drawing.Size(126, 13);
            this.labelProjectLocalisationPublished.TabIndex = 1;
            this.labelProjectLocalisationPublished.Text = "Published";
            // 
            // listBoxProjectLocalisationPubished
            // 
            this.listBoxProjectLocalisationPubished.BackColor = System.Drawing.Color.LightGreen;
            this.tableLayoutPanelProjectLocalisation.SetColumnSpan(this.listBoxProjectLocalisationPubished, 2);
            this.listBoxProjectLocalisationPubished.DataSource = this.enumLocalisationSystemBindingSource;
            this.listBoxProjectLocalisationPubished.DisplayMember = "DisplayText";
            this.listBoxProjectLocalisationPubished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjectLocalisationPubished.FormattingEnabled = true;
            this.listBoxProjectLocalisationPubished.Location = new System.Drawing.Point(287, 42);
            this.listBoxProjectLocalisationPubished.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.listBoxProjectLocalisationPubished.Name = "listBoxProjectLocalisationPubished";
            this.tableLayoutPanelProjectLocalisation.SetRowSpan(this.listBoxProjectLocalisationPubished, 2);
            this.listBoxProjectLocalisationPubished.Size = new System.Drawing.Size(258, 331);
            this.listBoxProjectLocalisationPubished.TabIndex = 3;
            this.listBoxProjectLocalisationPubished.ValueMember = "LocalisationSystemID";
            // 
            // enumLocalisationSystemBindingSource
            // 
            this.enumLocalisationSystemBindingSource.DataMember = "EnumLocalisationSystem";
            this.enumLocalisationSystemBindingSource.DataSource = this.dataSetCacheDB_1;
            // 
            // buttonProjectLocalisationPublish
            // 
            this.buttonProjectLocalisationPublish.BackColor = System.Drawing.Color.LightGreen;
            this.buttonProjectLocalisationPublish.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonProjectLocalisationPublish.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonProjectLocalisationPublish.ForeColor = System.Drawing.Color.Green;
            this.buttonProjectLocalisationPublish.Location = new System.Drawing.Point(264, 183);
            this.buttonProjectLocalisationPublish.Margin = new System.Windows.Forms.Padding(0);
            this.buttonProjectLocalisationPublish.Name = "buttonProjectLocalisationPublish";
            this.buttonProjectLocalisationPublish.Size = new System.Drawing.Size(20, 23);
            this.buttonProjectLocalisationPublish.TabIndex = 4;
            this.buttonProjectLocalisationPublish.Text = ">";
            this.toolTip.SetToolTip(this.buttonProjectLocalisationPublish, "Include the selected localisation system in the coordinate retrieval");
            this.buttonProjectLocalisationPublish.UseVisualStyleBackColor = false;
            // 
            // buttonProjectLocalisationHide
            // 
            this.buttonProjectLocalisationHide.BackColor = System.Drawing.Color.Pink;
            this.buttonProjectLocalisationHide.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonProjectLocalisationHide.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonProjectLocalisationHide.ForeColor = System.Drawing.Color.Red;
            this.buttonProjectLocalisationHide.Location = new System.Drawing.Point(264, 206);
            this.buttonProjectLocalisationHide.Margin = new System.Windows.Forms.Padding(0);
            this.buttonProjectLocalisationHide.Name = "buttonProjectLocalisationHide";
            this.buttonProjectLocalisationHide.Size = new System.Drawing.Size(20, 23);
            this.buttonProjectLocalisationHide.TabIndex = 5;
            this.buttonProjectLocalisationHide.Text = "<";
            this.toolTip.SetToolTip(this.buttonProjectLocalisationHide, "Remove the selected localisation system from the coordinate retrieval");
            this.buttonProjectLocalisationHide.UseVisualStyleBackColor = false;
            // 
            // buttonProjectLocalisationUp
            // 
            this.buttonProjectLocalisationUp.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonProjectLocalisationUp.Image = global::DiversityCollection.Resource.ArrowUp;
            this.buttonProjectLocalisationUp.Location = new System.Drawing.Point(389, 373);
            this.buttonProjectLocalisationUp.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.buttonProjectLocalisationUp.Name = "buttonProjectLocalisationUp";
            this.buttonProjectLocalisationUp.Size = new System.Drawing.Size(24, 18);
            this.buttonProjectLocalisationUp.TabIndex = 6;
            this.toolTip.SetToolTip(this.buttonProjectLocalisationUp, "Move the selected localisation system to the top of the coordinate retrieval");
            this.buttonProjectLocalisationUp.UseVisualStyleBackColor = true;
            // 
            // buttonProjectLocalisationDown
            // 
            this.buttonProjectLocalisationDown.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonProjectLocalisationDown.Image = global::DiversityCollection.Resource.ArrowDown;
            this.buttonProjectLocalisationDown.Location = new System.Drawing.Point(419, 373);
            this.buttonProjectLocalisationDown.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.buttonProjectLocalisationDown.Name = "buttonProjectLocalisationDown";
            this.buttonProjectLocalisationDown.Size = new System.Drawing.Size(24, 18);
            this.buttonProjectLocalisationDown.TabIndex = 7;
            this.toolTip.SetToolTip(this.buttonProjectLocalisationDown, "Move the selected localisation system to the base of the coordinate retrieval");
            this.buttonProjectLocalisationDown.UseVisualStyleBackColor = true;
            // 
            // labelProjectLocalisationHeader
            // 
            this.labelProjectLocalisationHeader.AutoSize = true;
            this.tableLayoutPanelProjectLocalisation.SetColumnSpan(this.labelProjectLocalisationHeader, 4);
            this.labelProjectLocalisationHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProjectLocalisationHeader.Location = new System.Drawing.Point(3, 0);
            this.labelProjectLocalisationHeader.Name = "labelProjectLocalisationHeader";
            this.labelProjectLocalisationHeader.Size = new System.Drawing.Size(542, 20);
            this.labelProjectLocalisationHeader.TabIndex = 8;
            this.labelProjectLocalisationHeader.Text = "The localisation systems used to retrieve the coordinates and their sequence of r" +
    "etrieval";
            this.labelProjectLocalisationHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBoxProjectSettings
            // 
            this.groupBoxProjectSettings.Controls.Add(this.tableLayoutPanelProjectSettings);
            this.groupBoxProjectSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxProjectSettings.Location = new System.Drawing.Point(0, 86);
            this.groupBoxProjectSettings.Name = "groupBoxProjectSettings";
            this.groupBoxProjectSettings.Size = new System.Drawing.Size(556, 49);
            this.groupBoxProjectSettings.TabIndex = 9;
            this.groupBoxProjectSettings.TabStop = false;
            this.groupBoxProjectSettings.Text = "Settings (retrieved from DiversityProjects)";
            // 
            // tableLayoutPanelProjectSettings
            // 
            this.tableLayoutPanelProjectSettings.ColumnCount = 2;
            this.tableLayoutPanelProjectSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelProjectSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelProjectSettings.Controls.Add(this.buttonProjectSettings, 0, 0);
            this.tableLayoutPanelProjectSettings.Controls.Add(this.buttonProjectRecordURI, 1, 0);
            this.tableLayoutPanelProjectSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelProjectSettings.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelProjectSettings.Name = "tableLayoutPanelProjectSettings";
            this.tableLayoutPanelProjectSettings.RowCount = 1;
            this.tableLayoutPanelProjectSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelProjectSettings.Size = new System.Drawing.Size(550, 30);
            this.tableLayoutPanelProjectSettings.TabIndex = 0;
            // 
            // buttonProjectSettings
            // 
            this.buttonProjectSettings.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonProjectSettings.Image = global::DiversityCollection.Resource.Settings;
            this.buttonProjectSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonProjectSettings.Location = new System.Drawing.Point(3, 3);
            this.buttonProjectSettings.Name = "buttonProjectSettings";
            this.buttonProjectSettings.Size = new System.Drawing.Size(131, 24);
            this.buttonProjectSettings.TabIndex = 2;
            this.buttonProjectSettings.Text = "View project settings";
            this.buttonProjectSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonProjectSettings.UseVisualStyleBackColor = true;
            this.buttonProjectSettings.Click += new System.EventHandler(this.buttonProjectSettings_Click);
            // 
            // buttonProjectRecordURI
            // 
            this.buttonProjectRecordURI.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonProjectRecordURI.Image = global::DiversityCollection.Resource.Browse;
            this.buttonProjectRecordURI.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonProjectRecordURI.Location = new System.Drawing.Point(387, 3);
            this.buttonProjectRecordURI.Name = "buttonProjectRecordURI";
            this.buttonProjectRecordURI.Size = new System.Drawing.Size(160, 24);
            this.buttonProjectRecordURI.TabIndex = 3;
            this.buttonProjectRecordURI.Text = "View record URI of project";
            this.buttonProjectRecordURI.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonProjectRecordURI.UseVisualStyleBackColor = true;
            this.buttonProjectRecordURI.Click += new System.EventHandler(this.buttonProjectRecordURI_Click);
            // 
            // groupBoxProjectCoordinates
            // 
            this.groupBoxProjectCoordinates.Controls.Add(this.tableLayoutPanelLocalisations);
            this.groupBoxProjectCoordinates.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxProjectCoordinates.Location = new System.Drawing.Point(0, 42);
            this.groupBoxProjectCoordinates.Name = "groupBoxProjectCoordinates";
            this.groupBoxProjectCoordinates.Size = new System.Drawing.Size(556, 44);
            this.groupBoxProjectCoordinates.TabIndex = 7;
            this.groupBoxProjectCoordinates.TabStop = false;
            this.groupBoxProjectCoordinates.Text = "Coordinates";
            // 
            // tableLayoutPanelLocalisations
            // 
            this.tableLayoutPanelLocalisations.ColumnCount = 4;
            this.tableLayoutPanelLocalisations.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelLocalisations.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLocalisations.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLocalisations.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLocalisations.Controls.Add(this.numericUpDownCoordinatePrecision, 2, 0);
            this.tableLayoutPanelLocalisations.Controls.Add(this.checkBoxCoordinatePrecision, 1, 0);
            this.tableLayoutPanelLocalisations.Controls.Add(this.labelCoordinatePrecision, 3, 0);
            this.tableLayoutPanelLocalisations.Controls.Add(this.pictureBoxProjectCoordiates, 0, 0);
            this.tableLayoutPanelLocalisations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelLocalisations.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelLocalisations.Name = "tableLayoutPanelLocalisations";
            this.tableLayoutPanelLocalisations.RowCount = 1;
            this.tableLayoutPanelLocalisations.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLocalisations.Size = new System.Drawing.Size(550, 25);
            this.tableLayoutPanelLocalisations.TabIndex = 5;
            // 
            // numericUpDownCoordinatePrecision
            // 
            this.numericUpDownCoordinatePrecision.Dock = System.Windows.Forms.DockStyle.Left;
            this.numericUpDownCoordinatePrecision.Location = new System.Drawing.Point(204, 3);
            this.numericUpDownCoordinatePrecision.Name = "numericUpDownCoordinatePrecision";
            this.numericUpDownCoordinatePrecision.Size = new System.Drawing.Size(26, 20);
            this.numericUpDownCoordinatePrecision.TabIndex = 13;
            this.numericUpDownCoordinatePrecision.Click += new System.EventHandler(this.numericUpDownCoordinatePrecision_Click);
            // 
            // checkBoxCoordinatePrecision
            // 
            this.checkBoxCoordinatePrecision.AutoSize = true;
            this.checkBoxCoordinatePrecision.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxCoordinatePrecision.Location = new System.Drawing.Point(23, 3);
            this.checkBoxCoordinatePrecision.Name = "checkBoxCoordinatePrecision";
            this.checkBoxCoordinatePrecision.Size = new System.Drawing.Size(175, 19);
            this.checkBoxCoordinatePrecision.TabIndex = 5;
            this.checkBoxCoordinatePrecision.Text = "Restrict coordinate precision to:";
            this.checkBoxCoordinatePrecision.UseVisualStyleBackColor = true;
            this.checkBoxCoordinatePrecision.Click += new System.EventHandler(this.checkBoxCoordinatePrecision_Click);
            // 
            // labelCoordinatePrecision
            // 
            this.labelCoordinatePrecision.AutoSize = true;
            this.labelCoordinatePrecision.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelCoordinatePrecision.Location = new System.Drawing.Point(236, 0);
            this.labelCoordinatePrecision.Name = "labelCoordinatePrecision";
            this.labelCoordinatePrecision.Size = new System.Drawing.Size(77, 25);
            this.labelCoordinatePrecision.TabIndex = 12;
            this.labelCoordinatePrecision.Text = "decimal places";
            this.labelCoordinatePrecision.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBoxProjectCoordiates
            // 
            this.pictureBoxProjectCoordiates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxProjectCoordiates.Image = global::DiversityCollection.Resource.Localisation;
            this.pictureBoxProjectCoordiates.Location = new System.Drawing.Point(0, 3);
            this.pictureBoxProjectCoordiates.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.pictureBoxProjectCoordiates.Name = "pictureBoxProjectCoordiates";
            this.pictureBoxProjectCoordiates.Size = new System.Drawing.Size(17, 19);
            this.pictureBoxProjectCoordiates.TabIndex = 14;
            this.pictureBoxProjectCoordiates.TabStop = false;
            // 
            // groupBoxProjectImages
            // 
            this.groupBoxProjectImages.Controls.Add(this.checkBoxHighResolutionImage);
            this.groupBoxProjectImages.Controls.Add(this.buttonHighResolutionImages);
            this.groupBoxProjectImages.Controls.Add(this.pictureBoxProjectImages);
            this.groupBoxProjectImages.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxProjectImages.Location = new System.Drawing.Point(0, 0);
            this.groupBoxProjectImages.Name = "groupBoxProjectImages";
            this.groupBoxProjectImages.Size = new System.Drawing.Size(556, 42);
            this.groupBoxProjectImages.TabIndex = 6;
            this.groupBoxProjectImages.TabStop = false;
            this.groupBoxProjectImages.Text = "Images";
            // 
            // checkBoxHighResolutionImage
            // 
            this.checkBoxHighResolutionImage.AutoSize = true;
            this.checkBoxHighResolutionImage.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.projectPublishedBindingSource, "HtmlAvailable", true));
            this.checkBoxHighResolutionImage.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxHighResolutionImage.Location = new System.Drawing.Point(25, 16);
            this.checkBoxHighResolutionImage.Name = "checkBoxHighResolutionImage";
            this.checkBoxHighResolutionImage.Size = new System.Drawing.Size(209, 23);
            this.checkBoxHighResolutionImage.TabIndex = 0;
            this.checkBoxHighResolutionImage.Text = "Project contains high resolution images";
            this.toolTip.SetToolTip(this.checkBoxHighResolutionImage, "availablity of high resolution versions of the images");
            this.checkBoxHighResolutionImage.UseVisualStyleBackColor = true;
            this.checkBoxHighResolutionImage.Visible = false;
            this.checkBoxHighResolutionImage.Click += new System.EventHandler(this.checkBoxHighResolutionImage_Click);
            // 
            // buttonHighResolutionImages
            // 
            this.buttonHighResolutionImages.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonHighResolutionImages.Image = global::DiversityCollection.Resource.Icones;
            this.buttonHighResolutionImages.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonHighResolutionImages.Location = new System.Drawing.Point(390, 16);
            this.buttonHighResolutionImages.Margin = new System.Windows.Forms.Padding(0);
            this.buttonHighResolutionImages.Name = "buttonHighResolutionImages";
            this.buttonHighResolutionImages.Size = new System.Drawing.Size(163, 23);
            this.buttonHighResolutionImages.TabIndex = 16;
            this.buttonHighResolutionImages.Text = "Source for high res. images";
            this.buttonHighResolutionImages.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonHighResolutionImages, "Inspect the function for retrieval of the path for the high resolution images - m" +
        "ay be adapted to your server settings");
            this.buttonHighResolutionImages.UseVisualStyleBackColor = true;
            this.buttonHighResolutionImages.Click += new System.EventHandler(this.buttonHighResolutionImages_Click);
            // 
            // pictureBoxProjectImages
            // 
            this.pictureBoxProjectImages.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBoxProjectImages.Image = global::DiversityCollection.Resource.Icones;
            this.pictureBoxProjectImages.Location = new System.Drawing.Point(3, 16);
            this.pictureBoxProjectImages.Name = "pictureBoxProjectImages";
            this.pictureBoxProjectImages.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.pictureBoxProjectImages.Size = new System.Drawing.Size(22, 23);
            this.pictureBoxProjectImages.TabIndex = 1;
            this.pictureBoxProjectImages.TabStop = false;
            // 
            // tabPageLocalisation
            // 
            this.tabPageLocalisation.Controls.Add(this.tableLayoutPanelLocalisation);
            this.tabPageLocalisation.ImageIndex = 5;
            this.tabPageLocalisation.Location = new System.Drawing.Point(4, 23);
            this.tabPageLocalisation.Name = "tabPageLocalisation";
            this.tabPageLocalisation.Size = new System.Drawing.Size(750, 556);
            this.tabPageLocalisation.TabIndex = 3;
            this.tabPageLocalisation.Text = "Localisation systems";
            this.tabPageLocalisation.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelLocalisation
            // 
            this.tableLayoutPanelLocalisation.ColumnCount = 4;
            this.tableLayoutPanelLocalisation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelLocalisation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelLocalisation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelLocalisation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelLocalisation.Controls.Add(this.labelLocalisationNotPublished, 0, 1);
            this.tableLayoutPanelLocalisation.Controls.Add(this.listBoxLocalisationNotPublished, 0, 2);
            this.tableLayoutPanelLocalisation.Controls.Add(this.labelLocalisationPublished, 2, 1);
            this.tableLayoutPanelLocalisation.Controls.Add(this.listBoxLocalisationPublished, 2, 2);
            this.tableLayoutPanelLocalisation.Controls.Add(this.buttonLocalisationPublished, 1, 2);
            this.tableLayoutPanelLocalisation.Controls.Add(this.buttonLocalisationNotPublished, 1, 3);
            this.tableLayoutPanelLocalisation.Controls.Add(this.buttonLocalisationPublishedUp, 2, 4);
            this.tableLayoutPanelLocalisation.Controls.Add(this.buttonLocalisationPublishedDown, 3, 4);
            this.tableLayoutPanelLocalisation.Controls.Add(this.labelLocalisation, 0, 0);
            this.tableLayoutPanelLocalisation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelLocalisation.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelLocalisation.Name = "tableLayoutPanelLocalisation";
            this.tableLayoutPanelLocalisation.RowCount = 5;
            this.tableLayoutPanelLocalisation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelLocalisation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLocalisation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelLocalisation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelLocalisation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelLocalisation.Size = new System.Drawing.Size(750, 556);
            this.tableLayoutPanelLocalisation.TabIndex = 0;
            // 
            // labelLocalisationNotPublished
            // 
            this.labelLocalisationNotPublished.AutoSize = true;
            this.labelLocalisationNotPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLocalisationNotPublished.Location = new System.Drawing.Point(3, 23);
            this.labelLocalisationNotPublished.Margin = new System.Windows.Forms.Padding(3);
            this.labelLocalisationNotPublished.Name = "labelLocalisationNotPublished";
            this.labelLocalisationNotPublished.Size = new System.Drawing.Size(359, 13);
            this.labelLocalisationNotPublished.TabIndex = 0;
            this.labelLocalisationNotPublished.Text = "Not published";
            // 
            // listBoxLocalisationNotPublished
            // 
            this.listBoxLocalisationNotPublished.BackColor = System.Drawing.Color.Pink;
            this.listBoxLocalisationNotPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxLocalisationNotPublished.FormattingEnabled = true;
            this.listBoxLocalisationNotPublished.Location = new System.Drawing.Point(3, 42);
            this.listBoxLocalisationNotPublished.Name = "listBoxLocalisationNotPublished";
            this.tableLayoutPanelLocalisation.SetRowSpan(this.listBoxLocalisationNotPublished, 3);
            this.listBoxLocalisationNotPublished.Size = new System.Drawing.Size(359, 511);
            this.listBoxLocalisationNotPublished.TabIndex = 2;
            // 
            // labelLocalisationPublished
            // 
            this.labelLocalisationPublished.AutoSize = true;
            this.labelLocalisationPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLocalisationPublished.Location = new System.Drawing.Point(388, 23);
            this.labelLocalisationPublished.Margin = new System.Windows.Forms.Padding(3);
            this.labelLocalisationPublished.Name = "labelLocalisationPublished";
            this.labelLocalisationPublished.Size = new System.Drawing.Size(176, 13);
            this.labelLocalisationPublished.TabIndex = 1;
            this.labelLocalisationPublished.Text = "Published";
            // 
            // listBoxLocalisationPublished
            // 
            this.listBoxLocalisationPublished.BackColor = System.Drawing.Color.LightGreen;
            this.tableLayoutPanelLocalisation.SetColumnSpan(this.listBoxLocalisationPublished, 2);
            this.listBoxLocalisationPublished.DataSource = this.enumLocalisationSystemBindingSource;
            this.listBoxLocalisationPublished.DisplayMember = "DisplayText";
            this.listBoxLocalisationPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxLocalisationPublished.FormattingEnabled = true;
            this.listBoxLocalisationPublished.Location = new System.Drawing.Point(388, 42);
            this.listBoxLocalisationPublished.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.listBoxLocalisationPublished.Name = "listBoxLocalisationPublished";
            this.tableLayoutPanelLocalisation.SetRowSpan(this.listBoxLocalisationPublished, 2);
            this.listBoxLocalisationPublished.Size = new System.Drawing.Size(359, 493);
            this.listBoxLocalisationPublished.TabIndex = 3;
            this.listBoxLocalisationPublished.ValueMember = "LocalisationSystemID";
            // 
            // buttonLocalisationPublished
            // 
            this.buttonLocalisationPublished.BackColor = System.Drawing.Color.LightGreen;
            this.buttonLocalisationPublished.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonLocalisationPublished.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLocalisationPublished.ForeColor = System.Drawing.Color.Green;
            this.buttonLocalisationPublished.Location = new System.Drawing.Point(365, 264);
            this.buttonLocalisationPublished.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLocalisationPublished.Name = "buttonLocalisationPublished";
            this.buttonLocalisationPublished.Size = new System.Drawing.Size(20, 23);
            this.buttonLocalisationPublished.TabIndex = 4;
            this.buttonLocalisationPublished.Text = ">";
            this.toolTip.SetToolTip(this.buttonLocalisationPublished, "Include the selected localisation system in the coordinate retrieval");
            this.buttonLocalisationPublished.UseVisualStyleBackColor = false;
            this.buttonLocalisationPublished.Click += new System.EventHandler(this.buttonLocalisationPublished_Click);
            // 
            // buttonLocalisationNotPublished
            // 
            this.buttonLocalisationNotPublished.BackColor = System.Drawing.Color.Pink;
            this.buttonLocalisationNotPublished.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonLocalisationNotPublished.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLocalisationNotPublished.ForeColor = System.Drawing.Color.Red;
            this.buttonLocalisationNotPublished.Location = new System.Drawing.Point(365, 287);
            this.buttonLocalisationNotPublished.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLocalisationNotPublished.Name = "buttonLocalisationNotPublished";
            this.buttonLocalisationNotPublished.Size = new System.Drawing.Size(20, 23);
            this.buttonLocalisationNotPublished.TabIndex = 5;
            this.buttonLocalisationNotPublished.Text = "<";
            this.toolTip.SetToolTip(this.buttonLocalisationNotPublished, "Remove the selected localisation system from the coordinate retrieval");
            this.buttonLocalisationNotPublished.UseVisualStyleBackColor = false;
            this.buttonLocalisationNotPublished.Click += new System.EventHandler(this.buttonLocalisationNotPublished_Click);
            // 
            // buttonLocalisationPublishedUp
            // 
            this.buttonLocalisationPublishedUp.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonLocalisationPublishedUp.Image = global::DiversityCollection.Resource.ArrowUp;
            this.buttonLocalisationPublishedUp.Location = new System.Drawing.Point(540, 535);
            this.buttonLocalisationPublishedUp.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.buttonLocalisationPublishedUp.Name = "buttonLocalisationPublishedUp";
            this.buttonLocalisationPublishedUp.Size = new System.Drawing.Size(24, 18);
            this.buttonLocalisationPublishedUp.TabIndex = 6;
            this.toolTip.SetToolTip(this.buttonLocalisationPublishedUp, "Move the selected localisation system to the top of the coordinate retrieval");
            this.buttonLocalisationPublishedUp.UseVisualStyleBackColor = true;
            this.buttonLocalisationPublishedUp.Click += new System.EventHandler(this.buttonLocalisationPublishedUp_Click);
            // 
            // buttonLocalisationPublishedDown
            // 
            this.buttonLocalisationPublishedDown.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonLocalisationPublishedDown.Image = global::DiversityCollection.Resource.ArrowDown;
            this.buttonLocalisationPublishedDown.Location = new System.Drawing.Point(570, 535);
            this.buttonLocalisationPublishedDown.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.buttonLocalisationPublishedDown.Name = "buttonLocalisationPublishedDown";
            this.buttonLocalisationPublishedDown.Size = new System.Drawing.Size(24, 18);
            this.buttonLocalisationPublishedDown.TabIndex = 7;
            this.toolTip.SetToolTip(this.buttonLocalisationPublishedDown, "Move the selected localisation system to the base of the coordinate retrieval");
            this.buttonLocalisationPublishedDown.UseVisualStyleBackColor = true;
            this.buttonLocalisationPublishedDown.Click += new System.EventHandler(this.buttonLocalisationPublishedDown_Click);
            // 
            // labelLocalisation
            // 
            this.labelLocalisation.AutoSize = true;
            this.tableLayoutPanelLocalisation.SetColumnSpan(this.labelLocalisation, 4);
            this.labelLocalisation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLocalisation.Location = new System.Drawing.Point(3, 0);
            this.labelLocalisation.Name = "labelLocalisation";
            this.labelLocalisation.Size = new System.Drawing.Size(744, 20);
            this.labelLocalisation.TabIndex = 8;
            this.labelLocalisation.Text = "The localisation systems used to retrieve the coordinates and their sequence of r" +
    "etrieval";
            this.labelLocalisation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPageTaxonSynonymy
            // 
            this.tabPageTaxonSynonymy.Controls.Add(this.panelTaxonSources);
            this.tabPageTaxonSynonymy.Controls.Add(this.tableLayoutPanelTaxonomySources);
            this.tabPageTaxonSynonymy.Controls.Add(this.toolStripTaxonSources);
            this.tabPageTaxonSynonymy.ImageIndex = 7;
            this.tabPageTaxonSynonymy.Location = new System.Drawing.Point(4, 23);
            this.tabPageTaxonSynonymy.Name = "tabPageTaxonSynonymy";
            this.tabPageTaxonSynonymy.Size = new System.Drawing.Size(750, 556);
            this.tabPageTaxonSynonymy.TabIndex = 4;
            this.tabPageTaxonSynonymy.Text = "Sources for taxonomy";
            this.tabPageTaxonSynonymy.UseVisualStyleBackColor = true;
            // 
            // panelTaxonSources
            // 
            this.panelTaxonSources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTaxonSources.Location = new System.Drawing.Point(0, 49);
            this.panelTaxonSources.Name = "panelTaxonSources";
            this.panelTaxonSources.Size = new System.Drawing.Size(750, 507);
            this.panelTaxonSources.TabIndex = 0;
            // 
            // tableLayoutPanelTaxonomySources
            // 
            this.tableLayoutPanelTaxonomySources.ColumnCount = 3;
            this.tableLayoutPanelTaxonomySources.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260F));
            this.tableLayoutPanelTaxonomySources.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.tableLayoutPanelTaxonomySources.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaxonomySources.Controls.Add(this.labelTaxonSourceDB, 0, 0);
            this.tableLayoutPanelTaxonomySources.Controls.Add(this.labelTaxonSourceView, 1, 0);
            this.tableLayoutPanelTaxonomySources.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelTaxonomySources.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanelTaxonomySources.Name = "tableLayoutPanelTaxonomySources";
            this.tableLayoutPanelTaxonomySources.RowCount = 1;
            this.tableLayoutPanelTaxonomySources.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaxonomySources.Size = new System.Drawing.Size(750, 24);
            this.tableLayoutPanelTaxonomySources.TabIndex = 0;
            // 
            // labelTaxonSourceDB
            // 
            this.labelTaxonSourceDB.AutoSize = true;
            this.labelTaxonSourceDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTaxonSourceDB.Location = new System.Drawing.Point(3, 0);
            this.labelTaxonSourceDB.Name = "labelTaxonSourceDB";
            this.labelTaxonSourceDB.Size = new System.Drawing.Size(254, 24);
            this.labelTaxonSourceDB.TabIndex = 0;
            this.labelTaxonSourceDB.Text = "Database";
            this.labelTaxonSourceDB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTaxonSourceView
            // 
            this.labelTaxonSourceView.AutoSize = true;
            this.labelTaxonSourceView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTaxonSourceView.Location = new System.Drawing.Point(263, 0);
            this.labelTaxonSourceView.Name = "labelTaxonSourceView";
            this.labelTaxonSourceView.Size = new System.Drawing.Size(224, 24);
            this.labelTaxonSourceView.TabIndex = 1;
            this.labelTaxonSourceView.Text = "Source within the database";
            this.labelTaxonSourceView.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toolStripTaxonSources
            // 
            this.toolStripTaxonSources.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripTaxonSources.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelTaxonSources,
            this.toolStripButtonTaxonSourceAdd});
            this.toolStripTaxonSources.Location = new System.Drawing.Point(0, 0);
            this.toolStripTaxonSources.Name = "toolStripTaxonSources";
            this.toolStripTaxonSources.Size = new System.Drawing.Size(750, 25);
            this.toolStripTaxonSources.TabIndex = 1;
            this.toolStripTaxonSources.Text = "toolStrip1";
            // 
            // toolStripLabelTaxonSources
            // 
            this.toolStripLabelTaxonSources.Name = "toolStripLabelTaxonSources";
            this.toolStripLabelTaxonSources.Size = new System.Drawing.Size(319, 22);
            this.toolStripLabelTaxonSources.Text = "Sources for the taxonomy (accepted names and synonyms)";
            // 
            // toolStripButtonTaxonSourceAdd
            // 
            this.toolStripButtonTaxonSourceAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTaxonSourceAdd.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonTaxonSourceAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTaxonSourceAdd.Name = "toolStripButtonTaxonSourceAdd";
            this.toolStripButtonTaxonSourceAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonTaxonSourceAdd.Text = "Add a source for the taxa";
            this.toolStripButtonTaxonSourceAdd.Click += new System.EventHandler(this.toolStripButtonTaxonSourceAdd_Click);
            // 
            // tabPageMaterialCategory
            // 
            this.tabPageMaterialCategory.Controls.Add(this.tableLayoutPanelMaterialCategories);
            this.tabPageMaterialCategory.ImageIndex = 4;
            this.tabPageMaterialCategory.Location = new System.Drawing.Point(4, 23);
            this.tabPageMaterialCategory.Name = "tabPageMaterialCategory";
            this.tabPageMaterialCategory.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMaterialCategory.Size = new System.Drawing.Size(750, 556);
            this.tabPageMaterialCategory.TabIndex = 1;
            this.tabPageMaterialCategory.Text = "Material category";
            this.tabPageMaterialCategory.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelMaterialCategories
            // 
            this.tableLayoutPanelMaterialCategories.ColumnCount = 5;
            this.tableLayoutPanelMaterialCategories.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMaterialCategories.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMaterialCategories.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMaterialCategories.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMaterialCategories.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanelMaterialCategories.Controls.Add(this.labelMaterialCategories, 0, 0);
            this.tableLayoutPanelMaterialCategories.Controls.Add(this.dataGridViewMaterialCategory, 2, 2);
            this.tableLayoutPanelMaterialCategories.Controls.Add(this.labelMaterialCategoryMissing, 0, 1);
            this.tableLayoutPanelMaterialCategories.Controls.Add(this.buttonMaterialAdd, 1, 2);
            this.tableLayoutPanelMaterialCategories.Controls.Add(this.buttonMaterialRemove, 1, 3);
            this.tableLayoutPanelMaterialCategories.Controls.Add(this.listBoxMaterialNotTransferred, 0, 2);
            this.tableLayoutPanelMaterialCategories.Controls.Add(this.buttonSaveMaterial, 4, 1);
            this.tableLayoutPanelMaterialCategories.Controls.Add(this.labelMaterialPublished, 2, 1);
            this.tableLayoutPanelMaterialCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMaterialCategories.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelMaterialCategories.Name = "tableLayoutPanelMaterialCategories";
            this.tableLayoutPanelMaterialCategories.RowCount = 4;
            this.tableLayoutPanelMaterialCategories.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMaterialCategories.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMaterialCategories.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMaterialCategories.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMaterialCategories.Size = new System.Drawing.Size(744, 550);
            this.tableLayoutPanelMaterialCategories.TabIndex = 4;
            // 
            // labelMaterialCategories
            // 
            this.labelMaterialCategories.AutoSize = true;
            this.tableLayoutPanelMaterialCategories.SetColumnSpan(this.labelMaterialCategories, 5);
            this.labelMaterialCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaterialCategories.Location = new System.Drawing.Point(3, 3);
            this.labelMaterialCategories.Margin = new System.Windows.Forms.Padding(3);
            this.labelMaterialCategories.Name = "labelMaterialCategories";
            this.labelMaterialCategories.Size = new System.Drawing.Size(738, 14);
            this.labelMaterialCategories.TabIndex = 2;
            this.labelMaterialCategories.Text = "Material categories transferred into the cache database and their translation int" +
    "o GBIF categories";
            this.labelMaterialCategories.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridViewMaterialCategory
            // 
            this.dataGridViewMaterialCategory.AllowUserToAddRows = false;
            this.dataGridViewMaterialCategory.AllowUserToDeleteRows = false;
            this.dataGridViewMaterialCategory.AutoGenerateColumns = false;
            this.dataGridViewMaterialCategory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMaterialCategory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.codeDataGridViewTextBoxColumn,
            this.displayTextDataGridViewTextBoxColumn,
            this.recordBasisDataGridViewTextBoxColumn,
            this.preparationTypeDataGridViewTextBoxColumn,
            this.categoryOrderDataGridViewTextBoxColumn});
            this.tableLayoutPanelMaterialCategories.SetColumnSpan(this.dataGridViewMaterialCategory, 3);
            this.dataGridViewMaterialCategory.DataSource = this.enumMaterialCategoryBindingSource;
            this.dataGridViewMaterialCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewMaterialCategory.Location = new System.Drawing.Point(199, 49);
            this.dataGridViewMaterialCategory.Name = "dataGridViewMaterialCategory";
            this.dataGridViewMaterialCategory.RowHeadersVisible = false;
            this.tableLayoutPanelMaterialCategories.SetRowSpan(this.dataGridViewMaterialCategory, 2);
            this.dataGridViewMaterialCategory.Size = new System.Drawing.Size(542, 498);
            this.dataGridViewMaterialCategory.TabIndex = 3;
            // 
            // codeDataGridViewTextBoxColumn
            // 
            this.codeDataGridViewTextBoxColumn.DataPropertyName = "Code";
            this.codeDataGridViewTextBoxColumn.HeaderText = "Code";
            this.codeDataGridViewTextBoxColumn.Name = "codeDataGridViewTextBoxColumn";
            this.codeDataGridViewTextBoxColumn.Visible = false;
            // 
            // displayTextDataGridViewTextBoxColumn
            // 
            this.displayTextDataGridViewTextBoxColumn.DataPropertyName = "DisplayText";
            this.displayTextDataGridViewTextBoxColumn.HeaderText = "Material category";
            this.displayTextDataGridViewTextBoxColumn.Name = "displayTextDataGridViewTextBoxColumn";
            this.displayTextDataGridViewTextBoxColumn.ReadOnly = true;
            this.displayTextDataGridViewTextBoxColumn.Width = 200;
            // 
            // recordBasisDataGridViewTextBoxColumn
            // 
            this.recordBasisDataGridViewTextBoxColumn.DataPropertyName = "RecordBasis";
            this.recordBasisDataGridViewTextBoxColumn.DataSource = this.enumRecordBasisBindingSource;
            this.recordBasisDataGridViewTextBoxColumn.DisplayMember = "RecordBasis";
            this.recordBasisDataGridViewTextBoxColumn.HeaderText = "Recordbasis";
            this.recordBasisDataGridViewTextBoxColumn.Name = "recordBasisDataGridViewTextBoxColumn";
            this.recordBasisDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.recordBasisDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.recordBasisDataGridViewTextBoxColumn.ValueMember = "RecordBasis";
            this.recordBasisDataGridViewTextBoxColumn.Width = 150;
            // 
            // enumRecordBasisBindingSource
            // 
            this.enumRecordBasisBindingSource.DataMember = "EnumRecordBasis";
            this.enumRecordBasisBindingSource.DataSource = this.dataSetCacheDB_1;
            // 
            // preparationTypeDataGridViewTextBoxColumn
            // 
            this.preparationTypeDataGridViewTextBoxColumn.DataPropertyName = "PreparationType";
            this.preparationTypeDataGridViewTextBoxColumn.DataSource = this.enumPreparationTypeBindingSource;
            this.preparationTypeDataGridViewTextBoxColumn.DisplayMember = "PreparationType";
            this.preparationTypeDataGridViewTextBoxColumn.HeaderText = "Type of preparation";
            this.preparationTypeDataGridViewTextBoxColumn.Name = "preparationTypeDataGridViewTextBoxColumn";
            this.preparationTypeDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.preparationTypeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.preparationTypeDataGridViewTextBoxColumn.ValueMember = "PreparationType";
            this.preparationTypeDataGridViewTextBoxColumn.Width = 150;
            // 
            // enumPreparationTypeBindingSource
            // 
            this.enumPreparationTypeBindingSource.DataMember = "EnumPreparationType";
            this.enumPreparationTypeBindingSource.DataSource = this.dataSetCacheDB_1;
            // 
            // categoryOrderDataGridViewTextBoxColumn
            // 
            this.categoryOrderDataGridViewTextBoxColumn.DataPropertyName = "CategoryOrder";
            this.categoryOrderDataGridViewTextBoxColumn.HeaderText = "Display order";
            this.categoryOrderDataGridViewTextBoxColumn.Name = "categoryOrderDataGridViewTextBoxColumn";
            this.categoryOrderDataGridViewTextBoxColumn.Visible = false;
            this.categoryOrderDataGridViewTextBoxColumn.Width = 50;
            // 
            // enumMaterialCategoryBindingSource
            // 
            this.enumMaterialCategoryBindingSource.DataMember = "EnumMaterialCategory";
            this.enumMaterialCategoryBindingSource.DataSource = this.dataSetCacheDB_1;
            // 
            // labelMaterialCategoryMissing
            // 
            this.labelMaterialCategoryMissing.AutoSize = true;
            this.labelMaterialCategoryMissing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaterialCategoryMissing.Location = new System.Drawing.Point(3, 20);
            this.labelMaterialCategoryMissing.Name = "labelMaterialCategoryMissing";
            this.labelMaterialCategoryMissing.Size = new System.Drawing.Size(170, 26);
            this.labelMaterialCategoryMissing.TabIndex = 5;
            this.labelMaterialCategoryMissing.Text = "Not published";
            this.labelMaterialCategoryMissing.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonMaterialAdd
            // 
            this.buttonMaterialAdd.BackColor = System.Drawing.Color.PaleGreen;
            this.buttonMaterialAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonMaterialAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonMaterialAdd.ForeColor = System.Drawing.Color.Green;
            this.buttonMaterialAdd.Location = new System.Drawing.Point(176, 275);
            this.buttonMaterialAdd.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMaterialAdd.Name = "buttonMaterialAdd";
            this.buttonMaterialAdd.Size = new System.Drawing.Size(20, 23);
            this.buttonMaterialAdd.TabIndex = 6;
            this.buttonMaterialAdd.Text = ">";
            this.toolTip.SetToolTip(this.buttonMaterialAdd, "publish the selected material category");
            this.buttonMaterialAdd.UseVisualStyleBackColor = false;
            this.buttonMaterialAdd.Click += new System.EventHandler(this.buttonMaterialAdd_Click);
            // 
            // buttonMaterialRemove
            // 
            this.buttonMaterialRemove.BackColor = System.Drawing.Color.Pink;
            this.buttonMaterialRemove.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonMaterialRemove.ForeColor = System.Drawing.Color.Red;
            this.buttonMaterialRemove.Location = new System.Drawing.Point(176, 298);
            this.buttonMaterialRemove.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMaterialRemove.Name = "buttonMaterialRemove";
            this.buttonMaterialRemove.Size = new System.Drawing.Size(20, 23);
            this.buttonMaterialRemove.TabIndex = 7;
            this.buttonMaterialRemove.Text = "<";
            this.toolTip.SetToolTip(this.buttonMaterialRemove, "Hide the selected material category");
            this.buttonMaterialRemove.UseVisualStyleBackColor = false;
            this.buttonMaterialRemove.Click += new System.EventHandler(this.buttonMaterialRemove_Click);
            // 
            // listBoxMaterialNotTransferred
            // 
            this.listBoxMaterialNotTransferred.BackColor = System.Drawing.Color.Pink;
            this.listBoxMaterialNotTransferred.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxMaterialNotTransferred.FormattingEnabled = true;
            this.listBoxMaterialNotTransferred.Location = new System.Drawing.Point(3, 49);
            this.listBoxMaterialNotTransferred.Name = "listBoxMaterialNotTransferred";
            this.tableLayoutPanelMaterialCategories.SetRowSpan(this.listBoxMaterialNotTransferred, 2);
            this.listBoxMaterialNotTransferred.Size = new System.Drawing.Size(170, 498);
            this.listBoxMaterialNotTransferred.TabIndex = 8;
            // 
            // buttonSaveMaterial
            // 
            this.buttonSaveMaterial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSaveMaterial.Image = global::DiversityCollection.Resource.Save;
            this.buttonSaveMaterial.Location = new System.Drawing.Point(721, 20);
            this.buttonSaveMaterial.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSaveMaterial.Name = "buttonSaveMaterial";
            this.buttonSaveMaterial.Size = new System.Drawing.Size(23, 26);
            this.buttonSaveMaterial.TabIndex = 9;
            this.toolTip.SetToolTip(this.buttonSaveMaterial, "Save changes to the material categories");
            this.buttonSaveMaterial.UseVisualStyleBackColor = true;
            this.buttonSaveMaterial.Click += new System.EventHandler(this.buttonSaveMaterial_Click);
            // 
            // labelMaterialPublished
            // 
            this.labelMaterialPublished.AutoSize = true;
            this.labelMaterialPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaterialPublished.Location = new System.Drawing.Point(199, 20);
            this.labelMaterialPublished.Name = "labelMaterialPublished";
            this.labelMaterialPublished.Size = new System.Drawing.Size(53, 26);
            this.labelMaterialPublished.TabIndex = 10;
            this.labelMaterialPublished.Text = "Published";
            this.labelMaterialPublished.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageKingdom
            // 
            this.tabPageKingdom.Controls.Add(this.tableLayoutPanelKingdom);
            this.tabPageKingdom.ImageIndex = 6;
            this.tabPageKingdom.Location = new System.Drawing.Point(4, 23);
            this.tabPageKingdom.Name = "tabPageKingdom";
            this.tabPageKingdom.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageKingdom.Size = new System.Drawing.Size(750, 556);
            this.tabPageKingdom.TabIndex = 2;
            this.tabPageKingdom.Text = "Kingdoms of taxonomic groups";
            this.tabPageKingdom.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelKingdom
            // 
            this.tableLayoutPanelKingdom.ColumnCount = 4;
            this.tableLayoutPanelKingdom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelKingdom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelKingdom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanelKingdom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanelKingdom.Controls.Add(this.labelKingdomNotPublished, 0, 1);
            this.tableLayoutPanelKingdom.Controls.Add(this.dataGridViewKingdoms, 2, 2);
            this.tableLayoutPanelKingdom.Controls.Add(this.listBoxKingdomNotPublished, 0, 2);
            this.tableLayoutPanelKingdom.Controls.Add(this.buttonKingdomNotPublished, 1, 3);
            this.tableLayoutPanelKingdom.Controls.Add(this.buttonKingdomPublished, 1, 2);
            this.tableLayoutPanelKingdom.Controls.Add(this.labelKingdomPublished, 2, 1);
            this.tableLayoutPanelKingdom.Controls.Add(this.buttonSaveKingdoms, 3, 1);
            this.tableLayoutPanelKingdom.Controls.Add(this.labelKingdoms, 0, 0);
            this.tableLayoutPanelKingdom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelKingdom.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelKingdom.Name = "tableLayoutPanelKingdom";
            this.tableLayoutPanelKingdom.RowCount = 4;
            this.tableLayoutPanelKingdom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelKingdom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelKingdom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelKingdom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelKingdom.Size = new System.Drawing.Size(744, 550);
            this.tableLayoutPanelKingdom.TabIndex = 1;
            // 
            // labelKingdomNotPublished
            // 
            this.labelKingdomNotPublished.AutoSize = true;
            this.labelKingdomNotPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelKingdomNotPublished.Location = new System.Drawing.Point(3, 23);
            this.labelKingdomNotPublished.Margin = new System.Windows.Forms.Padding(3);
            this.labelKingdomNotPublished.Name = "labelKingdomNotPublished";
            this.labelKingdomNotPublished.Size = new System.Drawing.Size(198, 17);
            this.labelKingdomNotPublished.TabIndex = 0;
            this.labelKingdomNotPublished.Text = "Not published";
            // 
            // dataGridViewKingdoms
            // 
            this.dataGridViewKingdoms.AllowUserToAddRows = false;
            this.dataGridViewKingdoms.AllowUserToDeleteRows = false;
            this.dataGridViewKingdoms.AllowUserToResizeRows = false;
            this.dataGridViewKingdoms.AutoGenerateColumns = false;
            this.dataGridViewKingdoms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewKingdoms.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.codeDataGridViewTextBoxColumn1,
            this.displayTextDataGridViewTextBoxColumn1,
            this.kingdomDataGridViewTextBoxColumn});
            this.tableLayoutPanelKingdom.SetColumnSpan(this.dataGridViewKingdoms, 2);
            this.dataGridViewKingdoms.DataSource = this.enumTaxonomicGroupBindingSource;
            this.dataGridViewKingdoms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewKingdoms.Location = new System.Drawing.Point(227, 46);
            this.dataGridViewKingdoms.Name = "dataGridViewKingdoms";
            this.dataGridViewKingdoms.RowHeadersVisible = false;
            this.tableLayoutPanelKingdom.SetRowSpan(this.dataGridViewKingdoms, 2);
            this.dataGridViewKingdoms.Size = new System.Drawing.Size(514, 501);
            this.dataGridViewKingdoms.TabIndex = 0;
            this.dataGridViewKingdoms.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewKingdoms_DataError);
            // 
            // codeDataGridViewTextBoxColumn1
            // 
            this.codeDataGridViewTextBoxColumn1.DataPropertyName = "Code";
            this.codeDataGridViewTextBoxColumn1.HeaderText = "Code";
            this.codeDataGridViewTextBoxColumn1.Name = "codeDataGridViewTextBoxColumn1";
            this.codeDataGridViewTextBoxColumn1.Visible = false;
            // 
            // displayTextDataGridViewTextBoxColumn1
            // 
            this.displayTextDataGridViewTextBoxColumn1.DataPropertyName = "DisplayText";
            this.displayTextDataGridViewTextBoxColumn1.HeaderText = "Taxonomic group";
            this.displayTextDataGridViewTextBoxColumn1.Name = "displayTextDataGridViewTextBoxColumn1";
            this.displayTextDataGridViewTextBoxColumn1.ReadOnly = true;
            this.displayTextDataGridViewTextBoxColumn1.Width = 200;
            // 
            // kingdomDataGridViewTextBoxColumn
            // 
            this.kingdomDataGridViewTextBoxColumn.DataPropertyName = "Kingdom";
            this.kingdomDataGridViewTextBoxColumn.DataSource = this.enumKingdomBindingSource;
            this.kingdomDataGridViewTextBoxColumn.DisplayMember = "Kingdom";
            this.kingdomDataGridViewTextBoxColumn.HeaderText = "Kingdom";
            this.kingdomDataGridViewTextBoxColumn.Name = "kingdomDataGridViewTextBoxColumn";
            this.kingdomDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.kingdomDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.kingdomDataGridViewTextBoxColumn.ValueMember = "Kingdom";
            this.kingdomDataGridViewTextBoxColumn.Width = 200;
            // 
            // enumKingdomBindingSource
            // 
            this.enumKingdomBindingSource.DataMember = "EnumKingdom";
            this.enumKingdomBindingSource.DataSource = this.dataSetCacheDB_1;
            // 
            // enumTaxonomicGroupBindingSource
            // 
            this.enumTaxonomicGroupBindingSource.DataMember = "EnumTaxonomicGroup";
            this.enumTaxonomicGroupBindingSource.DataSource = this.dataSetCacheDB_1;
            // 
            // listBoxKingdomNotPublished
            // 
            this.listBoxKingdomNotPublished.BackColor = System.Drawing.Color.Pink;
            this.listBoxKingdomNotPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxKingdomNotPublished.FormattingEnabled = true;
            this.listBoxKingdomNotPublished.Location = new System.Drawing.Point(3, 46);
            this.listBoxKingdomNotPublished.Name = "listBoxKingdomNotPublished";
            this.tableLayoutPanelKingdom.SetRowSpan(this.listBoxKingdomNotPublished, 2);
            this.listBoxKingdomNotPublished.Size = new System.Drawing.Size(198, 501);
            this.listBoxKingdomNotPublished.Sorted = true;
            this.listBoxKingdomNotPublished.TabIndex = 1;
            // 
            // buttonKingdomNotPublished
            // 
            this.buttonKingdomNotPublished.BackColor = System.Drawing.Color.Pink;
            this.buttonKingdomNotPublished.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonKingdomNotPublished.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonKingdomNotPublished.ForeColor = System.Drawing.Color.Red;
            this.buttonKingdomNotPublished.Location = new System.Drawing.Point(204, 296);
            this.buttonKingdomNotPublished.Margin = new System.Windows.Forms.Padding(0);
            this.buttonKingdomNotPublished.Name = "buttonKingdomNotPublished";
            this.buttonKingdomNotPublished.Size = new System.Drawing.Size(20, 23);
            this.buttonKingdomNotPublished.TabIndex = 2;
            this.buttonKingdomNotPublished.Text = "<";
            this.toolTip.SetToolTip(this.buttonKingdomNotPublished, "Hide the selected taxonomic group");
            this.buttonKingdomNotPublished.UseVisualStyleBackColor = false;
            this.buttonKingdomNotPublished.Click += new System.EventHandler(this.buttonKingdomNotPublished_Click);
            // 
            // buttonKingdomPublished
            // 
            this.buttonKingdomPublished.BackColor = System.Drawing.Color.LightGreen;
            this.buttonKingdomPublished.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonKingdomPublished.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonKingdomPublished.ForeColor = System.Drawing.Color.Green;
            this.buttonKingdomPublished.Location = new System.Drawing.Point(204, 273);
            this.buttonKingdomPublished.Margin = new System.Windows.Forms.Padding(0);
            this.buttonKingdomPublished.Name = "buttonKingdomPublished";
            this.buttonKingdomPublished.Size = new System.Drawing.Size(20, 23);
            this.buttonKingdomPublished.TabIndex = 3;
            this.buttonKingdomPublished.Text = ">";
            this.toolTip.SetToolTip(this.buttonKingdomPublished, "Publish the selected taxonomic group and set the kingdom");
            this.buttonKingdomPublished.UseVisualStyleBackColor = false;
            this.buttonKingdomPublished.Click += new System.EventHandler(this.buttonKingdomPublished_Click);
            // 
            // labelKingdomPublished
            // 
            this.labelKingdomPublished.AutoSize = true;
            this.labelKingdomPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelKingdomPublished.Location = new System.Drawing.Point(227, 23);
            this.labelKingdomPublished.Margin = new System.Windows.Forms.Padding(3);
            this.labelKingdomPublished.Name = "labelKingdomPublished";
            this.labelKingdomPublished.Size = new System.Drawing.Size(471, 17);
            this.labelKingdomPublished.TabIndex = 4;
            this.labelKingdomPublished.Text = "Published";
            // 
            // buttonSaveKingdoms
            // 
            this.buttonSaveKingdoms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSaveKingdoms.Image = global::DiversityCollection.Resource.Save;
            this.buttonSaveKingdoms.Location = new System.Drawing.Point(701, 20);
            this.buttonSaveKingdoms.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSaveKingdoms.Name = "buttonSaveKingdoms";
            this.buttonSaveKingdoms.Size = new System.Drawing.Size(43, 23);
            this.buttonSaveKingdoms.TabIndex = 5;
            this.toolTip.SetToolTip(this.buttonSaveKingdoms, "Save the changes");
            this.buttonSaveKingdoms.UseVisualStyleBackColor = true;
            this.buttonSaveKingdoms.Click += new System.EventHandler(this.buttonSaveKingdoms_Click);
            // 
            // labelKingdoms
            // 
            this.labelKingdoms.AutoSize = true;
            this.tableLayoutPanelKingdom.SetColumnSpan(this.labelKingdoms, 4);
            this.labelKingdoms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelKingdoms.Location = new System.Drawing.Point(3, 0);
            this.labelKingdoms.Name = "labelKingdoms";
            this.labelKingdoms.Size = new System.Drawing.Size(738, 20);
            this.labelKingdoms.TabIndex = 6;
            this.labelKingdoms.Text = "Taxonomic groups published and their translation into kingdoms";
            this.labelKingdoms.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.tableLayoutPanelSettings);
            this.tabPageSettings.ImageIndex = 8;
            this.tabPageSettings.Location = new System.Drawing.Point(4, 23);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Size = new System.Drawing.Size(750, 556);
            this.tabPageSettings.TabIndex = 5;
            this.tabPageSettings.Text = "Settings";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelSettings
            // 
            this.tableLayoutPanelSettings.ColumnCount = 2;
            this.tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSettings.Controls.Add(this.labelSettingsHeader, 0, 0);
            this.tableLayoutPanelSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSettings.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSettings.Name = "tableLayoutPanelSettings";
            this.tableLayoutPanelSettings.RowCount = 2;
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSettings.Size = new System.Drawing.Size(750, 556);
            this.tableLayoutPanelSettings.TabIndex = 0;
            // 
            // labelSettingsHeader
            // 
            this.labelSettingsHeader.AutoSize = true;
            this.labelSettingsHeader.Location = new System.Drawing.Point(3, 0);
            this.labelSettingsHeader.Name = "labelSettingsHeader";
            this.labelSettingsHeader.Size = new System.Drawing.Size(273, 13);
            this.labelSettingsHeader.TabIndex = 0;
            this.labelSettingsHeader.Text = "Settings retrieved from DiversityProjects - Define retrieval";
            // 
            // tabPageAdminPostgres
            // 
            this.tabPageAdminPostgres.Controls.Add(this.splitContainerAdminPostgres);
            this.tabPageAdminPostgres.ImageIndex = 10;
            this.tabPageAdminPostgres.Location = new System.Drawing.Point(4, 23);
            this.tabPageAdminPostgres.Name = "tabPageAdminPostgres";
            this.tabPageAdminPostgres.Size = new System.Drawing.Size(764, 589);
            this.tabPageAdminPostgres.TabIndex = 3;
            this.tabPageAdminPostgres.Text = "Postgres";
            this.tabPageAdminPostgres.UseVisualStyleBackColor = true;
            // 
            // splitContainerAdminPostgres
            // 
            this.splitContainerAdminPostgres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpProvider.SetHelpKeyword(this.splitContainerAdminPostgres, "Postgres database administration");
            this.helpProvider.SetHelpNavigator(this.splitContainerAdminPostgres, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.splitContainerAdminPostgres.Location = new System.Drawing.Point(0, 0);
            this.splitContainerAdminPostgres.Name = "splitContainerAdminPostgres";
            // 
            // splitContainerAdminPostgres.Panel1
            // 
            this.splitContainerAdminPostgres.Panel1.Controls.Add(this.tableLayoutPanelAdminPostgresList);
            this.splitContainerAdminPostgres.Panel1Collapsed = true;
            // 
            // splitContainerAdminPostgres.Panel2
            // 
            this.splitContainerAdminPostgres.Panel2.Controls.Add(this.tableLayoutPanelPostgresAdmin);
            this.splitContainerAdminPostgres.Panel2.Controls.Add(this.tableLayoutPanelPostgresAdminLogin);
            this.helpProvider.SetShowHelp(this.splitContainerAdminPostgres, true);
            this.splitContainerAdminPostgres.Size = new System.Drawing.Size(764, 589);
            this.splitContainerAdminPostgres.SplitterDistance = 198;
            this.splitContainerAdminPostgres.TabIndex = 0;
            // 
            // tableLayoutPanelAdminPostgresList
            // 
            this.tableLayoutPanelAdminPostgresList.ColumnCount = 3;
            this.tableLayoutPanelAdminPostgresList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAdminPostgresList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAdminPostgresList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAdminPostgresList.Controls.Add(this.labelPostgresDBlist, 0, 0);
            this.tableLayoutPanelAdminPostgresList.Controls.Add(this.listBoxPostgresDBs, 0, 1);
            this.tableLayoutPanelAdminPostgresList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelAdminPostgresList.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelAdminPostgresList.Name = "tableLayoutPanelAdminPostgresList";
            this.tableLayoutPanelAdminPostgresList.RowCount = 4;
            this.tableLayoutPanelAdminPostgresList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAdminPostgresList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAdminPostgresList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAdminPostgresList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanelAdminPostgresList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelAdminPostgresList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelAdminPostgresList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelAdminPostgresList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelAdminPostgresList.Size = new System.Drawing.Size(198, 100);
            this.tableLayoutPanelAdminPostgresList.TabIndex = 0;
            // 
            // labelPostgresDBlist
            // 
            this.labelPostgresDBlist.AutoSize = true;
            this.tableLayoutPanelAdminPostgresList.SetColumnSpan(this.labelPostgresDBlist, 3);
            this.labelPostgresDBlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPostgresDBlist.Location = new System.Drawing.Point(3, 3);
            this.labelPostgresDBlist.Margin = new System.Windows.Forms.Padding(3);
            this.labelPostgresDBlist.Name = "labelPostgresDBlist";
            this.labelPostgresDBlist.Size = new System.Drawing.Size(192, 13);
            this.labelPostgresDBlist.TabIndex = 0;
            this.labelPostgresDBlist.Text = "List of PostgreSQL databases";
            this.labelPostgresDBlist.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listBoxPostgresDBs
            // 
            this.tableLayoutPanelAdminPostgresList.SetColumnSpan(this.listBoxPostgresDBs, 3);
            this.listBoxPostgresDBs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxPostgresDBs.FormattingEnabled = true;
            this.listBoxPostgresDBs.Location = new System.Drawing.Point(3, 22);
            this.listBoxPostgresDBs.Name = "listBoxPostgresDBs";
            this.listBoxPostgresDBs.Size = new System.Drawing.Size(192, 35);
            this.listBoxPostgresDBs.TabIndex = 1;
            this.listBoxPostgresDBs.SelectedIndexChanged += new System.EventHandler(this.listBoxPostgresDBs_SelectedIndexChanged);
            // 
            // tableLayoutPanelPostgresAdmin
            // 
            this.tableLayoutPanelPostgresAdmin.ColumnCount = 2;
            this.tableLayoutPanelPostgresAdmin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelPostgresAdmin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelPostgresAdmin.Controls.Add(this.labelPostgresVersion, 0, 0);
            this.tableLayoutPanelPostgresAdmin.Controls.Add(this.buttonPostgresUpdate, 1, 0);
            this.tableLayoutPanelPostgresAdmin.Controls.Add(this.tabControlPostgresAdmin, 0, 1);
            this.tableLayoutPanelPostgresAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelPostgresAdmin.Enabled = false;
            this.tableLayoutPanelPostgresAdmin.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanelPostgresAdmin.Name = "tableLayoutPanelPostgresAdmin";
            this.tableLayoutPanelPostgresAdmin.RowCount = 3;
            this.tableLayoutPanelPostgresAdmin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelPostgresAdmin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPostgresAdmin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelPostgresAdmin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelPostgresAdmin.Size = new System.Drawing.Size(764, 565);
            this.tableLayoutPanelPostgresAdmin.TabIndex = 0;
            // 
            // labelPostgresVersion
            // 
            this.labelPostgresVersion.AutoSize = true;
            this.labelPostgresVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPostgresVersion.Location = new System.Drawing.Point(3, 0);
            this.labelPostgresVersion.Name = "labelPostgresVersion";
            this.labelPostgresVersion.Size = new System.Drawing.Size(376, 24);
            this.labelPostgresVersion.TabIndex = 0;
            this.labelPostgresVersion.Text = "Version";
            this.labelPostgresVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonPostgresUpdate
            // 
            this.buttonPostgresUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPostgresUpdate.Image = global::DiversityCollection.Resource.UpdateDatabase;
            this.buttonPostgresUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonPostgresUpdate.Location = new System.Drawing.Point(385, 0);
            this.buttonPostgresUpdate.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonPostgresUpdate.Name = "buttonPostgresUpdate";
            this.buttonPostgresUpdate.Size = new System.Drawing.Size(376, 24);
            this.buttonPostgresUpdate.TabIndex = 1;
            this.buttonPostgresUpdate.Text = "Update";
            this.buttonPostgresUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonPostgresUpdate.UseVisualStyleBackColor = true;
            this.buttonPostgresUpdate.Visible = false;
            this.buttonPostgresUpdate.Click += new System.EventHandler(this.buttonPostgresUpdate_Click);
            // 
            // tabControlPostgresAdmin
            // 
            this.tableLayoutPanelPostgresAdmin.SetColumnSpan(this.tabControlPostgresAdmin, 2);
            this.tabControlPostgresAdmin.Controls.Add(this.tabPagePostgresRoles);
            this.tabControlPostgresAdmin.Controls.Add(this.tabPagePostgresGroups);
            this.tabControlPostgresAdmin.Controls.Add(this.tabPagePostgresTables);
            this.tabControlPostgresAdmin.Controls.Add(this.tabPagePostgresProjects);
            this.tabControlPostgresAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPostgresAdmin.ImageList = this.imageListTabControl;
            this.tabControlPostgresAdmin.Location = new System.Drawing.Point(3, 27);
            this.tabControlPostgresAdmin.Name = "tabControlPostgresAdmin";
            this.tableLayoutPanelPostgresAdmin.SetRowSpan(this.tabControlPostgresAdmin, 2);
            this.tabControlPostgresAdmin.SelectedIndex = 0;
            this.tabControlPostgresAdmin.Size = new System.Drawing.Size(758, 535);
            this.tabControlPostgresAdmin.TabIndex = 4;
            // 
            // tabPagePostgresRoles
            // 
            this.tabPagePostgresRoles.Controls.Add(this.splitContainerPgAdminRoles);
            this.tabPagePostgresRoles.ImageIndex = 14;
            this.tabPagePostgresRoles.Location = new System.Drawing.Point(4, 23);
            this.tabPagePostgresRoles.Name = "tabPagePostgresRoles";
            this.tabPagePostgresRoles.Size = new System.Drawing.Size(750, 508);
            this.tabPagePostgresRoles.TabIndex = 3;
            this.tabPagePostgresRoles.Text = "Roles";
            this.tabPagePostgresRoles.UseVisualStyleBackColor = true;
            // 
            // splitContainerPgAdminRoles
            // 
            this.splitContainerPgAdminRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerPgAdminRoles.Location = new System.Drawing.Point(0, 0);
            this.splitContainerPgAdminRoles.Name = "splitContainerPgAdminRoles";
            // 
            // splitContainerPgAdminRoles.Panel1
            // 
            this.splitContainerPgAdminRoles.Panel1.Controls.Add(this.listBoxPgAdminRoles);
            this.splitContainerPgAdminRoles.Panel1.Controls.Add(this.toolStripPgAdminRoles);
            // 
            // splitContainerPgAdminRoles.Panel2
            // 
            this.splitContainerPgAdminRoles.Panel2.Controls.Add(this.tableLayoutPanelPgAdminRoles);
            this.splitContainerPgAdminRoles.Size = new System.Drawing.Size(750, 508);
            this.splitContainerPgAdminRoles.SplitterDistance = 250;
            this.splitContainerPgAdminRoles.TabIndex = 0;
            // 
            // listBoxPgAdminRoles
            // 
            this.listBoxPgAdminRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxPgAdminRoles.FormattingEnabled = true;
            this.listBoxPgAdminRoles.IntegralHeight = false;
            this.listBoxPgAdminRoles.Location = new System.Drawing.Point(0, 0);
            this.listBoxPgAdminRoles.Name = "listBoxPgAdminRoles";
            this.listBoxPgAdminRoles.Size = new System.Drawing.Size(250, 483);
            this.listBoxPgAdminRoles.TabIndex = 0;
            this.listBoxPgAdminRoles.SelectedIndexChanged += new System.EventHandler(this.listBoxPgAdminRoles_SelectedIndexChanged);
            // 
            // toolStripPgAdminRoles
            // 
            this.toolStripPgAdminRoles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripPgAdminRoles.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripPgAdminRoles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonPgAdminRoleAdministration});
            this.toolStripPgAdminRoles.Location = new System.Drawing.Point(0, 483);
            this.toolStripPgAdminRoles.Name = "toolStripPgAdminRoles";
            this.toolStripPgAdminRoles.Size = new System.Drawing.Size(250, 25);
            this.toolStripPgAdminRoles.TabIndex = 1;
            this.toolStripPgAdminRoles.Text = "toolStrip1";
            // 
            // toolStripButtonPgAdminRoleAdministration
            // 
            this.toolStripButtonPgAdminRoleAdministration.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonPgAdminRoleAdministration.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPgAdminRoleAdministration.Image")));
            this.toolStripButtonPgAdminRoleAdministration.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPgAdminRoleAdministration.Name = "toolStripButtonPgAdminRoleAdministration";
            this.toolStripButtonPgAdminRoleAdministration.Size = new System.Drawing.Size(90, 22);
            this.toolStripButtonPgAdminRoleAdministration.Text = "Administration";
            this.toolStripButtonPgAdminRoleAdministration.ToolTipText = "Administration of roles";
            this.toolStripButtonPgAdminRoleAdministration.Click += new System.EventHandler(this.toolStripButtonPgAdminRoleAdministration_Click);
            // 
            // tableLayoutPanelPgAdminRoles
            // 
            this.tableLayoutPanelPgAdminRoles.ColumnCount = 1;
            this.tableLayoutPanelPgAdminRoles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPgAdminRoles.Controls.Add(this.dataGridViewPgAdminRoleGrants, 0, 1);
            this.tableLayoutPanelPgAdminRoles.Controls.Add(this.labelPgAdminRoleGrants, 0, 0);
            this.tableLayoutPanelPgAdminRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelPgAdminRoles.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelPgAdminRoles.Name = "tableLayoutPanelPgAdminRoles";
            this.tableLayoutPanelPgAdminRoles.RowCount = 2;
            this.tableLayoutPanelPgAdminRoles.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPgAdminRoles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPgAdminRoles.Size = new System.Drawing.Size(496, 508);
            this.tableLayoutPanelPgAdminRoles.TabIndex = 0;
            // 
            // dataGridViewPgAdminRoleGrants
            // 
            this.dataGridViewPgAdminRoleGrants.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPgAdminRoleGrants.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPgAdminRoleGrants.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewPgAdminRoleGrants.Name = "dataGridViewPgAdminRoleGrants";
            this.dataGridViewPgAdminRoleGrants.ReadOnly = true;
            this.dataGridViewPgAdminRoleGrants.RowHeadersVisible = false;
            this.dataGridViewPgAdminRoleGrants.Size = new System.Drawing.Size(490, 489);
            this.dataGridViewPgAdminRoleGrants.TabIndex = 0;
            // 
            // labelPgAdminRoleGrants
            // 
            this.labelPgAdminRoleGrants.AutoSize = true;
            this.labelPgAdminRoleGrants.Location = new System.Drawing.Point(3, 0);
            this.labelPgAdminRoleGrants.Name = "labelPgAdminRoleGrants";
            this.labelPgAdminRoleGrants.Size = new System.Drawing.Size(38, 13);
            this.labelPgAdminRoleGrants.TabIndex = 1;
            this.labelPgAdminRoleGrants.Text = "Grants";
            // 
            // tabPagePostgresGroups
            // 
            this.tabPagePostgresGroups.Controls.Add(this.tableLayoutPanelPostgresUser);
            this.tabPagePostgresGroups.ImageIndex = 12;
            this.tabPagePostgresGroups.Location = new System.Drawing.Point(4, 23);
            this.tabPagePostgresGroups.Name = "tabPagePostgresGroups";
            this.tabPagePostgresGroups.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePostgresGroups.Size = new System.Drawing.Size(750, 508);
            this.tabPagePostgresGroups.TabIndex = 0;
            this.tabPagePostgresGroups.Text = "User groups";
            this.tabPagePostgresGroups.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelPostgresUser
            // 
            this.tableLayoutPanelPostgresUser.ColumnCount = 2;
            this.tableLayoutPanelPostgresUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPostgresUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPostgresUser.Controls.Add(this.listBoxPostgresUser, 0, 0);
            this.tableLayoutPanelPostgresUser.Controls.Add(this.toolStripPostgresUser, 0, 1);
            this.tableLayoutPanelPostgresUser.Controls.Add(this.dataGridViewPostgresUser, 1, 0);
            this.tableLayoutPanelPostgresUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelPostgresUser.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelPostgresUser.Name = "tableLayoutPanelPostgresUser";
            this.tableLayoutPanelPostgresUser.RowCount = 2;
            this.tableLayoutPanelPostgresUser.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPostgresUser.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPostgresUser.Size = new System.Drawing.Size(744, 502);
            this.tableLayoutPanelPostgresUser.TabIndex = 0;
            // 
            // listBoxPostgresUser
            // 
            this.listBoxPostgresUser.FormattingEnabled = true;
            this.listBoxPostgresUser.Location = new System.Drawing.Point(3, 3);
            this.listBoxPostgresUser.Name = "listBoxPostgresUser";
            this.listBoxPostgresUser.Size = new System.Drawing.Size(150, 355);
            this.listBoxPostgresUser.TabIndex = 2;
            this.listBoxPostgresUser.SelectedIndexChanged += new System.EventHandler(this.listBoxPostgresUser_SelectedIndexChanged);
            // 
            // toolStripPostgresUser
            // 
            this.toolStripPostgresUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripPostgresUser.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripPostgresUser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonPostgresUserAdd,
            this.toolStripButtonPostgresUserDelete,
            this.toolStripButtonPostgresUserPassword,
            this.toolStripButtonPostgresRoleAdministration});
            this.toolStripPostgresUser.Location = new System.Drawing.Point(0, 477);
            this.toolStripPostgresUser.Name = "toolStripPostgresUser";
            this.toolStripPostgresUser.Size = new System.Drawing.Size(156, 25);
            this.toolStripPostgresUser.TabIndex = 3;
            this.toolStripPostgresUser.Text = "toolStrip1";
            // 
            // toolStripButtonPostgresUserAdd
            // 
            this.toolStripButtonPostgresUserAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPostgresUserAdd.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonPostgresUserAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPostgresUserAdd.Name = "toolStripButtonPostgresUserAdd";
            this.toolStripButtonPostgresUserAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPostgresUserAdd.Text = "Insert a new user";
            this.toolStripButtonPostgresUserAdd.Click += new System.EventHandler(this.toolStripButtonPostgresUserAdd_Click);
            // 
            // toolStripButtonPostgresUserDelete
            // 
            this.toolStripButtonPostgresUserDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPostgresUserDelete.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonPostgresUserDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPostgresUserDelete.Name = "toolStripButtonPostgresUserDelete";
            this.toolStripButtonPostgresUserDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPostgresUserDelete.Text = "Delete the selected user";
            this.toolStripButtonPostgresUserDelete.Click += new System.EventHandler(this.toolStripButtonPostgresUserDelete_Click);
            // 
            // toolStripButtonPostgresUserPassword
            // 
            this.toolStripButtonPostgresUserPassword.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPostgresUserPassword.Image = global::DiversityCollection.Resource.Key_2;
            this.toolStripButtonPostgresUserPassword.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPostgresUserPassword.Name = "toolStripButtonPostgresUserPassword";
            this.toolStripButtonPostgresUserPassword.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPostgresUserPassword.Text = "Set the password for the selected role";
            this.toolStripButtonPostgresUserPassword.Click += new System.EventHandler(this.toolStripButtonPostgresUserPassword_Click);
            // 
            // toolStripButtonPostgresRoleAdministration
            // 
            this.toolStripButtonPostgresRoleAdministration.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPostgresRoleAdministration.Image = global::DiversityCollection.Resource.Postgres;
            this.toolStripButtonPostgresRoleAdministration.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPostgresRoleAdministration.Name = "toolStripButtonPostgresRoleAdministration";
            this.toolStripButtonPostgresRoleAdministration.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPostgresRoleAdministration.Text = "Administrate roles in the database";
            this.toolStripButtonPostgresRoleAdministration.Click += new System.EventHandler(this.toolStripButtonPostgresRoleAdministration_Click);
            // 
            // dataGridViewPostgresUser
            // 
            this.dataGridViewPostgresUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPostgresUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPostgresUser.Location = new System.Drawing.Point(159, 3);
            this.dataGridViewPostgresUser.Name = "dataGridViewPostgresUser";
            this.dataGridViewPostgresUser.Size = new System.Drawing.Size(582, 471);
            this.dataGridViewPostgresUser.TabIndex = 4;
            // 
            // tabPagePostgresTables
            // 
            this.tabPagePostgresTables.Controls.Add(this.splitContainerPostgresTables);
            this.tabPagePostgresTables.ImageIndex = 13;
            this.tabPagePostgresTables.Location = new System.Drawing.Point(4, 23);
            this.tabPagePostgresTables.Name = "tabPagePostgresTables";
            this.tabPagePostgresTables.Size = new System.Drawing.Size(750, 508);
            this.tabPagePostgresTables.TabIndex = 2;
            this.tabPagePostgresTables.Text = "Tables";
            this.tabPagePostgresTables.UseVisualStyleBackColor = true;
            // 
            // splitContainerPostgresTables
            // 
            this.splitContainerPostgresTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerPostgresTables.Location = new System.Drawing.Point(0, 0);
            this.splitContainerPostgresTables.Name = "splitContainerPostgresTables";
            // 
            // splitContainerPostgresTables.Panel1
            // 
            this.splitContainerPostgresTables.Panel1.Controls.Add(this.listBoxPGtables);
            // 
            // splitContainerPostgresTables.Panel2
            // 
            this.splitContainerPostgresTables.Panel2.Controls.Add(this.dataGridViewPGtable);
            this.splitContainerPostgresTables.Size = new System.Drawing.Size(750, 508);
            this.splitContainerPostgresTables.SplitterDistance = 250;
            this.splitContainerPostgresTables.TabIndex = 0;
            // 
            // listBoxPGtables
            // 
            this.listBoxPGtables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxPGtables.FormattingEnabled = true;
            this.listBoxPGtables.Location = new System.Drawing.Point(0, 0);
            this.listBoxPGtables.Name = "listBoxPGtables";
            this.listBoxPGtables.Size = new System.Drawing.Size(250, 508);
            this.listBoxPGtables.TabIndex = 0;
            this.listBoxPGtables.SelectedIndexChanged += new System.EventHandler(this.listBoxPGtables_SelectedIndexChanged);
            // 
            // dataGridViewPGtable
            // 
            this.dataGridViewPGtable.AllowUserToAddRows = false;
            this.dataGridViewPGtable.AllowUserToDeleteRows = false;
            this.dataGridViewPGtable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPGtable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPGtable.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewPGtable.Name = "dataGridViewPGtable";
            this.dataGridViewPGtable.ReadOnly = true;
            this.dataGridViewPGtable.Size = new System.Drawing.Size(496, 508);
            this.dataGridViewPGtable.TabIndex = 0;
            // 
            // tabPagePostgresProjects
            // 
            this.tabPagePostgresProjects.Controls.Add(this.tableLayoutPanelPostgresAdminProjects);
            this.tabPagePostgresProjects.ImageIndex = 3;
            this.tabPagePostgresProjects.Location = new System.Drawing.Point(4, 23);
            this.tabPagePostgresProjects.Name = "tabPagePostgresProjects";
            this.tabPagePostgresProjects.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePostgresProjects.Size = new System.Drawing.Size(750, 508);
            this.tabPagePostgresProjects.TabIndex = 1;
            this.tabPagePostgresProjects.Text = "Projects";
            this.tabPagePostgresProjects.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelPostgresAdminProjects
            // 
            this.tableLayoutPanelPostgresAdminProjects.ColumnCount = 3;
            this.tableLayoutPanelPostgresAdminProjects.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPostgresAdminProjects.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPostgresAdminProjects.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPostgresAdminProjects.Controls.Add(this.labelPostgresProjectsAvailable, 0, 0);
            this.tableLayoutPanelPostgresAdminProjects.Controls.Add(this.listBoxPostgresProjectsAvailable, 0, 1);
            this.tableLayoutPanelPostgresAdminProjects.Controls.Add(this.buttonPostgresProjectsEstablish, 1, 2);
            this.tableLayoutPanelPostgresAdminProjects.Controls.Add(this.labelPostgresProjectsEstablished, 2, 0);
            this.tableLayoutPanelPostgresAdminProjects.Controls.Add(this.panelPostgresEstablishedProjects, 2, 1);
            this.tableLayoutPanelPostgresAdminProjects.Controls.Add(this.tabControlPgProjects, 0, 5);
            this.tableLayoutPanelPostgresAdminProjects.Controls.Add(this.labelProjectPackages, 0, 4);
            this.tableLayoutPanelPostgresAdminProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelPostgresAdminProjects.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelPostgresAdminProjects.Name = "tableLayoutPanelPostgresAdminProjects";
            this.tableLayoutPanelPostgresAdminProjects.RowCount = 6;
            this.tableLayoutPanelPostgresAdminProjects.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPostgresAdminProjects.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelPostgresAdminProjects.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPostgresAdminProjects.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelPostgresAdminProjects.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelPostgresAdminProjects.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPostgresAdminProjects.Size = new System.Drawing.Size(744, 502);
            this.tableLayoutPanelPostgresAdminProjects.TabIndex = 0;
            // 
            // labelPostgresProjectsAvailable
            // 
            this.labelPostgresProjectsAvailable.AutoSize = true;
            this.labelPostgresProjectsAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPostgresProjectsAvailable.Location = new System.Drawing.Point(3, 0);
            this.labelPostgresProjectsAvailable.Name = "labelPostgresProjectsAvailable";
            this.labelPostgresProjectsAvailable.Size = new System.Drawing.Size(100, 13);
            this.labelPostgresProjectsAvailable.TabIndex = 0;
            this.labelPostgresProjectsAvailable.Text = "Available projects";
            // 
            // listBoxPostgresProjectsAvailable
            // 
            this.listBoxPostgresProjectsAvailable.BackColor = System.Drawing.Color.Pink;
            this.listBoxPostgresProjectsAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxPostgresProjectsAvailable.FormattingEnabled = true;
            this.listBoxPostgresProjectsAvailable.IntegralHeight = false;
            this.listBoxPostgresProjectsAvailable.Location = new System.Drawing.Point(3, 16);
            this.listBoxPostgresProjectsAvailable.Name = "listBoxPostgresProjectsAvailable";
            this.tableLayoutPanelPostgresAdminProjects.SetRowSpan(this.listBoxPostgresProjectsAvailable, 3);
            this.listBoxPostgresProjectsAvailable.Size = new System.Drawing.Size(100, 325);
            this.listBoxPostgresProjectsAvailable.TabIndex = 1;
            // 
            // buttonPostgresProjectsEstablish
            // 
            this.buttonPostgresProjectsEstablish.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPostgresProjectsEstablish.Image = global::DiversityCollection.Resource.ArrowNext1;
            this.buttonPostgresProjectsEstablish.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonPostgresProjectsEstablish.Location = new System.Drawing.Point(106, 167);
            this.buttonPostgresProjectsEstablish.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPostgresProjectsEstablish.Name = "buttonPostgresProjectsEstablish";
            this.buttonPostgresProjectsEstablish.Size = new System.Drawing.Size(23, 23);
            this.buttonPostgresProjectsEstablish.TabIndex = 2;
            this.buttonPostgresProjectsEstablish.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonPostgresProjectsEstablish, "Establish the selected project in the postgres database");
            this.buttonPostgresProjectsEstablish.UseVisualStyleBackColor = true;
            this.buttonPostgresProjectsEstablish.Click += new System.EventHandler(this.buttonPostgresProjectsEstablish_Click);
            // 
            // labelPostgresProjectsEstablished
            // 
            this.labelPostgresProjectsEstablished.AutoSize = true;
            this.labelPostgresProjectsEstablished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPostgresProjectsEstablished.Location = new System.Drawing.Point(132, 0);
            this.labelPostgresProjectsEstablished.Name = "labelPostgresProjectsEstablished";
            this.labelPostgresProjectsEstablished.Size = new System.Drawing.Size(609, 13);
            this.labelPostgresProjectsEstablished.TabIndex = 3;
            this.labelPostgresProjectsEstablished.Text = "Established projects";
            // 
            // panelPostgresEstablishedProjects
            // 
            this.panelPostgresEstablishedProjects.AutoScroll = true;
            this.panelPostgresEstablishedProjects.BackColor = System.Drawing.Color.LightGreen;
            this.panelPostgresEstablishedProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPostgresEstablishedProjects.Location = new System.Drawing.Point(132, 16);
            this.panelPostgresEstablishedProjects.Name = "panelPostgresEstablishedProjects";
            this.tableLayoutPanelPostgresAdminProjects.SetRowSpan(this.panelPostgresEstablishedProjects, 3);
            this.panelPostgresEstablishedProjects.Size = new System.Drawing.Size(609, 325);
            this.panelPostgresEstablishedProjects.TabIndex = 8;
            // 
            // tabControlPgProjects
            // 
            this.tableLayoutPanelPostgresAdminProjects.SetColumnSpan(this.tabControlPgProjects, 3);
            this.tabControlPgProjects.Controls.Add(this.tabPagePgProjectPackages);
            this.tabControlPgProjects.Controls.Add(this.tabPagePgProjectPermissions);
            this.tabControlPgProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPgProjects.Enabled = false;
            this.tabControlPgProjects.ImageList = this.imageListTabControl;
            this.tabControlPgProjects.Location = new System.Drawing.Point(3, 367);
            this.tabControlPgProjects.Name = "tabControlPgProjects";
            this.tabControlPgProjects.SelectedIndex = 0;
            this.tabControlPgProjects.Size = new System.Drawing.Size(738, 132);
            this.tabControlPgProjects.TabIndex = 9;
            // 
            // tabPagePgProjectPackages
            // 
            this.tabPagePgProjectPackages.Controls.Add(this.tableLayoutPanelPostgresPackages);
            this.tabPagePgProjectPackages.ImageIndex = 15;
            this.tabPagePgProjectPackages.Location = new System.Drawing.Point(4, 23);
            this.tabPagePgProjectPackages.Name = "tabPagePgProjectPackages";
            this.tabPagePgProjectPackages.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePgProjectPackages.Size = new System.Drawing.Size(730, 105);
            this.tabPagePgProjectPackages.TabIndex = 0;
            this.tabPagePgProjectPackages.Text = "Packages";
            this.tabPagePgProjectPackages.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelPostgresPackages
            // 
            this.tableLayoutPanelPostgresPackages.ColumnCount = 3;
            this.tableLayoutPanelPostgresPackages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPostgresPackages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPostgresPackages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPostgresPackages.Controls.Add(this.panelPostgresProjectPackages, 2, 1);
            this.tableLayoutPanelPostgresPackages.Controls.Add(this.labelPostgresPackagesAvailable, 0, 0);
            this.tableLayoutPanelPostgresPackages.Controls.Add(this.labelPostgresPackagesEstablished, 2, 0);
            this.tableLayoutPanelPostgresPackages.Controls.Add(this.listBoxPostgresPackagesAvailable, 0, 1);
            this.tableLayoutPanelPostgresPackages.Controls.Add(this.buttonPostgresPackageEstablish, 1, 1);
            this.tableLayoutPanelPostgresPackages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelPostgresPackages.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelPostgresPackages.Name = "tableLayoutPanelPostgresPackages";
            this.tableLayoutPanelPostgresPackages.RowCount = 2;
            this.tableLayoutPanelPostgresPackages.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPostgresPackages.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPostgresPackages.Size = new System.Drawing.Size(724, 99);
            this.tableLayoutPanelPostgresPackages.TabIndex = 1;
            // 
            // panelPostgresProjectPackages
            // 
            this.panelPostgresProjectPackages.AutoScroll = true;
            this.panelPostgresProjectPackages.BackColor = System.Drawing.Color.LightGreen;
            this.panelPostgresProjectPackages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPostgresProjectPackages.Location = new System.Drawing.Point(132, 16);
            this.panelPostgresProjectPackages.Name = "panelPostgresProjectPackages";
            this.panelPostgresProjectPackages.Size = new System.Drawing.Size(589, 80);
            this.panelPostgresProjectPackages.TabIndex = 0;
            // 
            // labelPostgresPackagesAvailable
            // 
            this.labelPostgresPackagesAvailable.AutoSize = true;
            this.labelPostgresPackagesAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPostgresPackagesAvailable.Location = new System.Drawing.Point(3, 0);
            this.labelPostgresPackagesAvailable.Name = "labelPostgresPackagesAvailable";
            this.labelPostgresPackagesAvailable.Size = new System.Drawing.Size(100, 13);
            this.labelPostgresPackagesAvailable.TabIndex = 1;
            this.labelPostgresPackagesAvailable.Text = "Available";
            // 
            // labelPostgresPackagesEstablished
            // 
            this.labelPostgresPackagesEstablished.AutoSize = true;
            this.labelPostgresPackagesEstablished.Location = new System.Drawing.Point(132, 0);
            this.labelPostgresPackagesEstablished.Name = "labelPostgresPackagesEstablished";
            this.labelPostgresPackagesEstablished.Size = new System.Drawing.Size(111, 13);
            this.labelPostgresPackagesEstablished.TabIndex = 2;
            this.labelPostgresPackagesEstablished.Text = "Established packages";
            // 
            // listBoxPostgresPackagesAvailable
            // 
            this.listBoxPostgresPackagesAvailable.BackColor = System.Drawing.Color.Pink;
            this.listBoxPostgresPackagesAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxPostgresPackagesAvailable.FormattingEnabled = true;
            this.listBoxPostgresPackagesAvailable.IntegralHeight = false;
            this.listBoxPostgresPackagesAvailable.Location = new System.Drawing.Point(3, 16);
            this.listBoxPostgresPackagesAvailable.Name = "listBoxPostgresPackagesAvailable";
            this.listBoxPostgresPackagesAvailable.Size = new System.Drawing.Size(100, 80);
            this.listBoxPostgresPackagesAvailable.TabIndex = 3;
            // 
            // buttonPostgresPackageEstablish
            // 
            this.buttonPostgresPackageEstablish.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonPostgresPackageEstablish.Image = global::DiversityCollection.Resource.ArrowNext1;
            this.buttonPostgresPackageEstablish.Location = new System.Drawing.Point(106, 16);
            this.buttonPostgresPackageEstablish.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.buttonPostgresPackageEstablish.Name = "buttonPostgresPackageEstablish";
            this.buttonPostgresPackageEstablish.Size = new System.Drawing.Size(23, 23);
            this.buttonPostgresPackageEstablish.TabIndex = 4;
            this.buttonPostgresPackageEstablish.UseVisualStyleBackColor = true;
            this.buttonPostgresPackageEstablish.Click += new System.EventHandler(this.buttonPostgresPackageEstablish_Click);
            // 
            // tabPagePgProjectPermissions
            // 
            this.tabPagePgProjectPermissions.Controls.Add(this.dataGridViewPostgresProjectPermissions);
            this.tabPagePgProjectPermissions.ImageIndex = 16;
            this.tabPagePgProjectPermissions.Location = new System.Drawing.Point(4, 23);
            this.tabPagePgProjectPermissions.Name = "tabPagePgProjectPermissions";
            this.tabPagePgProjectPermissions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePgProjectPermissions.Size = new System.Drawing.Size(730, 105);
            this.tabPagePgProjectPermissions.TabIndex = 1;
            this.tabPagePgProjectPermissions.Text = "Permissions";
            this.tabPagePgProjectPermissions.UseVisualStyleBackColor = true;
            // 
            // dataGridViewPostgresProjectPermissions
            // 
            this.dataGridViewPostgresProjectPermissions.AllowUserToAddRows = false;
            this.dataGridViewPostgresProjectPermissions.AllowUserToDeleteRows = false;
            this.dataGridViewPostgresProjectPermissions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPostgresProjectPermissions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPostgresProjectPermissions.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewPostgresProjectPermissions.Name = "dataGridViewPostgresProjectPermissions";
            this.dataGridViewPostgresProjectPermissions.ReadOnly = true;
            this.dataGridViewPostgresProjectPermissions.Size = new System.Drawing.Size(724, 99);
            this.dataGridViewPostgresProjectPermissions.TabIndex = 6;
            // 
            // labelProjectPackages
            // 
            this.labelProjectPackages.AutoSize = true;
            this.labelProjectPackages.BackColor = System.Drawing.Color.LightGreen;
            this.tableLayoutPanelPostgresAdminProjects.SetColumnSpan(this.labelProjectPackages, 3);
            this.labelProjectPackages.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelProjectPackages.Location = new System.Drawing.Point(3, 351);
            this.labelProjectPackages.Name = "labelProjectPackages";
            this.labelProjectPackages.Size = new System.Drawing.Size(738, 13);
            this.labelProjectPackages.TabIndex = 10;
            this.labelProjectPackages.Text = "Project";
            this.labelProjectPackages.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.labelProjectPackages.Visible = false;
            // 
            // tableLayoutPanelPostgresAdminLogin
            // 
            this.tableLayoutPanelPostgresAdminLogin.ColumnCount = 2;
            this.tableLayoutPanelPostgresAdminLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPostgresAdminLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPostgresAdminLogin.Controls.Add(this.toolStripPostgresDBs, 0, 0);
            this.tableLayoutPanelPostgresAdminLogin.Controls.Add(this.labelPgAdminConnection, 1, 0);
            this.tableLayoutPanelPostgresAdminLogin.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelPostgresAdminLogin.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelPostgresAdminLogin.Name = "tableLayoutPanelPostgresAdminLogin";
            this.tableLayoutPanelPostgresAdminLogin.RowCount = 1;
            this.tableLayoutPanelPostgresAdminLogin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPostgresAdminLogin.Size = new System.Drawing.Size(764, 24);
            this.tableLayoutPanelPostgresAdminLogin.TabIndex = 1;
            // 
            // toolStripPostgresDBs
            // 
            this.toolStripPostgresDBs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripPostgresDBs.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripPostgresDBs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonPostgresConnect,
            this.toolStripButtonPostgresNewDB,
            this.toolStripButtonPostgresDeleteDB,
            this.toolStripSeparatorPgAdmin,
            this.toolStripButtonPgAdminRoles});
            this.toolStripPostgresDBs.Location = new System.Drawing.Point(0, 0);
            this.toolStripPostgresDBs.Name = "toolStripPostgresDBs";
            this.toolStripPostgresDBs.Size = new System.Drawing.Size(101, 24);
            this.toolStripPostgresDBs.TabIndex = 2;
            this.toolStripPostgresDBs.Text = "toolStrip1";
            // 
            // toolStripButtonPostgresConnect
            // 
            this.toolStripButtonPostgresConnect.BackColor = System.Drawing.Color.Transparent;
            this.toolStripButtonPostgresConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPostgresConnect.Image = global::DiversityCollection.Resource.NoPostgres;
            this.toolStripButtonPostgresConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPostgresConnect.Name = "toolStripButtonPostgresConnect";
            this.toolStripButtonPostgresConnect.Size = new System.Drawing.Size(23, 21);
            this.toolStripButtonPostgresConnect.Text = "Connect to Postgres database server";
            this.toolStripButtonPostgresConnect.Click += new System.EventHandler(this.toolStripButtonPostgresConnect_Click);
            // 
            // toolStripButtonPostgresNewDB
            // 
            this.toolStripButtonPostgresNewDB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPostgresNewDB.Enabled = false;
            this.toolStripButtonPostgresNewDB.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonPostgresNewDB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPostgresNewDB.Name = "toolStripButtonPostgresNewDB";
            this.toolStripButtonPostgresNewDB.Size = new System.Drawing.Size(23, 21);
            this.toolStripButtonPostgresNewDB.Text = "Create a new database";
            this.toolStripButtonPostgresNewDB.Click += new System.EventHandler(this.toolStripButtonPostgresNewDB_Click);
            // 
            // toolStripButtonPostgresDeleteDB
            // 
            this.toolStripButtonPostgresDeleteDB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPostgresDeleteDB.Enabled = false;
            this.toolStripButtonPostgresDeleteDB.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonPostgresDeleteDB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPostgresDeleteDB.Name = "toolStripButtonPostgresDeleteDB";
            this.toolStripButtonPostgresDeleteDB.Size = new System.Drawing.Size(23, 21);
            this.toolStripButtonPostgresDeleteDB.Text = "Delete the selected database";
            this.toolStripButtonPostgresDeleteDB.Click += new System.EventHandler(this.toolStripButtonPostgresDeleteDB_Click);
            // 
            // toolStripSeparatorPgAdmin
            // 
            this.toolStripSeparatorPgAdmin.Name = "toolStripSeparatorPgAdmin";
            this.toolStripSeparatorPgAdmin.Size = new System.Drawing.Size(6, 24);
            // 
            // toolStripButtonPgAdminRoles
            // 
            this.toolStripButtonPgAdminRoles.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonPgAdminRoles.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPgAdminRoles.Enabled = false;
            this.toolStripButtonPgAdminRoles.Image = global::DiversityCollection.Resource.Login;
            this.toolStripButtonPgAdminRoles.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPgAdminRoles.Name = "toolStripButtonPgAdminRoles";
            this.toolStripButtonPgAdminRoles.Size = new System.Drawing.Size(23, 21);
            this.toolStripButtonPgAdminRoles.Text = "Administration of user roles";
            this.toolStripButtonPgAdminRoles.Click += new System.EventHandler(this.toolStripButtonPgAdminRoles_Click);
            // 
            // labelPgAdminConnection
            // 
            this.labelPgAdminConnection.AutoSize = true;
            this.labelPgAdminConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPgAdminConnection.ForeColor = System.Drawing.Color.Red;
            this.labelPgAdminConnection.Location = new System.Drawing.Point(104, 0);
            this.labelPgAdminConnection.Name = "labelPgAdminConnection";
            this.labelPgAdminConnection.Size = new System.Drawing.Size(657, 24);
            this.labelPgAdminConnection.TabIndex = 13;
            this.labelPgAdminConnection.Text = "Not connected";
            this.labelPgAdminConnection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageOverview
            // 
            this.tabPageOverview.Controls.Add(this.tableLayoutPanelOverview);
            this.tabPageOverview.Location = new System.Drawing.Point(4, 23);
            this.tabPageOverview.Name = "tabPageOverview";
            this.tabPageOverview.Size = new System.Drawing.Size(764, 589);
            this.tabPageOverview.TabIndex = 2;
            this.tabPageOverview.Text = "Overview";
            this.tabPageOverview.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelOverview
            // 
            this.tableLayoutPanelOverview.ColumnCount = 9;
            this.tableLayoutPanelOverview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelOverview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanelOverview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelOverview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelOverview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanelOverview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelOverview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelOverview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.tableLayoutPanelOverview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanelOverview.Controls.Add(this.groupBoxOverviewSource, 0, 1);
            this.tableLayoutPanelOverview.Controls.Add(this.groupBoxOverviewCache, 3, 1);
            this.tableLayoutPanelOverview.Controls.Add(this.groupBoxOverviewPostgres, 6, 1);
            this.tableLayoutPanelOverview.Controls.Add(this.labelOverviewSourceDB, 1, 0);
            this.tableLayoutPanelOverview.Controls.Add(this.pictureBoxOverviewSourceDB, 0, 0);
            this.tableLayoutPanelOverview.Controls.Add(this.pictureBoxOverviewCacheDB, 3, 0);
            this.tableLayoutPanelOverview.Controls.Add(this.labelOverviewCacheDB, 4, 0);
            this.tableLayoutPanelOverview.Controls.Add(this.pictureBoxOverviewPostgresDB, 6, 0);
            this.tableLayoutPanelOverview.Controls.Add(this.labelOverviewPostgresDB, 7, 0);
            this.tableLayoutPanelOverview.Controls.Add(this.buttonOverviewPostgresDBConnect, 8, 0);
            this.tableLayoutPanelOverview.Controls.Add(this.buttonOverviewTransferToCacheDB, 2, 1);
            this.tableLayoutPanelOverview.Controls.Add(this.buttonOverviewTransferToPostgresDB, 5, 1);
            this.tableLayoutPanelOverview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelOverview.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelOverview.Name = "tableLayoutPanelOverview";
            this.tableLayoutPanelOverview.RowCount = 3;
            this.tableLayoutPanelOverview.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelOverview.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelOverview.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanelOverview.Size = new System.Drawing.Size(764, 589);
            this.tableLayoutPanelOverview.TabIndex = 0;
            // 
            // groupBoxOverviewSource
            // 
            this.tableLayoutPanelOverview.SetColumnSpan(this.groupBoxOverviewSource, 2);
            this.groupBoxOverviewSource.Controls.Add(this.panelOverviewSource);
            this.groupBoxOverviewSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxOverviewSource.Location = new System.Drawing.Point(3, 27);
            this.groupBoxOverviewSource.Name = "groupBoxOverviewSource";
            this.tableLayoutPanelOverview.SetRowSpan(this.groupBoxOverviewSource, 2);
            this.groupBoxOverviewSource.Size = new System.Drawing.Size(218, 559);
            this.groupBoxOverviewSource.TabIndex = 0;
            this.groupBoxOverviewSource.TabStop = false;
            this.groupBoxOverviewSource.Text = "Source database";
            // 
            // panelOverviewSource
            // 
            this.panelOverviewSource.AutoScroll = true;
            this.panelOverviewSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOverviewSource.Location = new System.Drawing.Point(3, 16);
            this.panelOverviewSource.Name = "panelOverviewSource";
            this.panelOverviewSource.Size = new System.Drawing.Size(212, 540);
            this.panelOverviewSource.TabIndex = 0;
            // 
            // groupBoxOverviewCache
            // 
            this.tableLayoutPanelOverview.SetColumnSpan(this.groupBoxOverviewCache, 2);
            this.groupBoxOverviewCache.Controls.Add(this.panelOverviewCache);
            this.groupBoxOverviewCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxOverviewCache.Location = new System.Drawing.Point(247, 27);
            this.groupBoxOverviewCache.Name = "groupBoxOverviewCache";
            this.tableLayoutPanelOverview.SetRowSpan(this.groupBoxOverviewCache, 2);
            this.groupBoxOverviewCache.Size = new System.Drawing.Size(218, 559);
            this.groupBoxOverviewCache.TabIndex = 1;
            this.groupBoxOverviewCache.TabStop = false;
            this.groupBoxOverviewCache.Text = "Cache database";
            // 
            // panelOverviewCache
            // 
            this.panelOverviewCache.AutoScroll = true;
            this.panelOverviewCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOverviewCache.Location = new System.Drawing.Point(3, 16);
            this.panelOverviewCache.Name = "panelOverviewCache";
            this.panelOverviewCache.Size = new System.Drawing.Size(212, 540);
            this.panelOverviewCache.TabIndex = 0;
            // 
            // groupBoxOverviewPostgres
            // 
            this.tableLayoutPanelOverview.SetColumnSpan(this.groupBoxOverviewPostgres, 3);
            this.groupBoxOverviewPostgres.Controls.Add(this.panelOverviewPostgres);
            this.groupBoxOverviewPostgres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxOverviewPostgres.Location = new System.Drawing.Point(491, 27);
            this.groupBoxOverviewPostgres.Name = "groupBoxOverviewPostgres";
            this.tableLayoutPanelOverview.SetRowSpan(this.groupBoxOverviewPostgres, 2);
            this.groupBoxOverviewPostgres.Size = new System.Drawing.Size(270, 559);
            this.groupBoxOverviewPostgres.TabIndex = 2;
            this.groupBoxOverviewPostgres.TabStop = false;
            this.groupBoxOverviewPostgres.Text = "Postgres database";
            // 
            // panelOverviewPostgres
            // 
            this.panelOverviewPostgres.AutoScroll = true;
            this.panelOverviewPostgres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOverviewPostgres.Location = new System.Drawing.Point(3, 16);
            this.panelOverviewPostgres.Name = "panelOverviewPostgres";
            this.panelOverviewPostgres.Size = new System.Drawing.Size(264, 540);
            this.panelOverviewPostgres.TabIndex = 0;
            // 
            // labelOverviewSourceDB
            // 
            this.labelOverviewSourceDB.AutoSize = true;
            this.labelOverviewSourceDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOverviewSourceDB.Location = new System.Drawing.Point(20, 0);
            this.labelOverviewSourceDB.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelOverviewSourceDB.Name = "labelOverviewSourceDB";
            this.labelOverviewSourceDB.Size = new System.Drawing.Size(201, 24);
            this.labelOverviewSourceDB.TabIndex = 3;
            this.labelOverviewSourceDB.Text = "label1";
            this.labelOverviewSourceDB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBoxOverviewSourceDB
            // 
            this.pictureBoxOverviewSourceDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxOverviewSourceDB.Image = global::DiversityCollection.Resource.Database;
            this.pictureBoxOverviewSourceDB.Location = new System.Drawing.Point(4, 4);
            this.pictureBoxOverviewSourceDB.Margin = new System.Windows.Forms.Padding(4, 4, 0, 2);
            this.pictureBoxOverviewSourceDB.Name = "pictureBoxOverviewSourceDB";
            this.pictureBoxOverviewSourceDB.Size = new System.Drawing.Size(16, 18);
            this.pictureBoxOverviewSourceDB.TabIndex = 4;
            this.pictureBoxOverviewSourceDB.TabStop = false;
            // 
            // pictureBoxOverviewCacheDB
            // 
            this.pictureBoxOverviewCacheDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxOverviewCacheDB.Image = global::DiversityCollection.Resource.CacheDB;
            this.pictureBoxOverviewCacheDB.Location = new System.Drawing.Point(248, 4);
            this.pictureBoxOverviewCacheDB.Margin = new System.Windows.Forms.Padding(4, 4, 0, 2);
            this.pictureBoxOverviewCacheDB.Name = "pictureBoxOverviewCacheDB";
            this.pictureBoxOverviewCacheDB.Size = new System.Drawing.Size(16, 18);
            this.pictureBoxOverviewCacheDB.TabIndex = 5;
            this.pictureBoxOverviewCacheDB.TabStop = false;
            // 
            // labelOverviewCacheDB
            // 
            this.labelOverviewCacheDB.AutoSize = true;
            this.labelOverviewCacheDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOverviewCacheDB.Location = new System.Drawing.Point(264, 0);
            this.labelOverviewCacheDB.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelOverviewCacheDB.Name = "labelOverviewCacheDB";
            this.labelOverviewCacheDB.Size = new System.Drawing.Size(201, 24);
            this.labelOverviewCacheDB.TabIndex = 6;
            this.labelOverviewCacheDB.Text = "label1";
            this.labelOverviewCacheDB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBoxOverviewPostgresDB
            // 
            this.pictureBoxOverviewPostgresDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxOverviewPostgresDB.Image = global::DiversityCollection.Resource.Postgres;
            this.pictureBoxOverviewPostgresDB.Location = new System.Drawing.Point(492, 4);
            this.pictureBoxOverviewPostgresDB.Margin = new System.Windows.Forms.Padding(4, 4, 0, 2);
            this.pictureBoxOverviewPostgresDB.Name = "pictureBoxOverviewPostgresDB";
            this.pictureBoxOverviewPostgresDB.Size = new System.Drawing.Size(16, 18);
            this.pictureBoxOverviewPostgresDB.TabIndex = 7;
            this.pictureBoxOverviewPostgresDB.TabStop = false;
            // 
            // labelOverviewPostgresDB
            // 
            this.labelOverviewPostgresDB.AutoSize = true;
            this.labelOverviewPostgresDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOverviewPostgresDB.Location = new System.Drawing.Point(508, 0);
            this.labelOverviewPostgresDB.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelOverviewPostgresDB.Name = "labelOverviewPostgresDB";
            this.labelOverviewPostgresDB.Size = new System.Drawing.Size(189, 24);
            this.labelOverviewPostgresDB.TabIndex = 8;
            this.labelOverviewPostgresDB.Text = "Not connected";
            this.labelOverviewPostgresDB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonOverviewPostgresDBConnect
            // 
            this.buttonOverviewPostgresDBConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOverviewPostgresDBConnect.Image = global::DiversityCollection.Resource.NoPostgres;
            this.buttonOverviewPostgresDBConnect.Location = new System.Drawing.Point(700, 0);
            this.buttonOverviewPostgresDBConnect.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOverviewPostgresDBConnect.Name = "buttonOverviewPostgresDBConnect";
            this.buttonOverviewPostgresDBConnect.Size = new System.Drawing.Size(64, 24);
            this.buttonOverviewPostgresDBConnect.TabIndex = 9;
            this.toolTip.SetToolTip(this.buttonOverviewPostgresDBConnect, "Connect to database");
            this.buttonOverviewPostgresDBConnect.UseVisualStyleBackColor = true;
            this.buttonOverviewPostgresDBConnect.Click += new System.EventHandler(this.buttonOverviewPostgresDBConnect_Click);
            // 
            // buttonOverviewTransferToCacheDB
            // 
            this.buttonOverviewTransferToCacheDB.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonOverviewTransferToCacheDB.Image = global::DiversityCollection.Resource.ArrowNextNext;
            this.buttonOverviewTransferToCacheDB.Location = new System.Drawing.Point(224, 170);
            this.buttonOverviewTransferToCacheDB.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOverviewTransferToCacheDB.Name = "buttonOverviewTransferToCacheDB";
            this.buttonOverviewTransferToCacheDB.Size = new System.Drawing.Size(20, 23);
            this.buttonOverviewTransferToCacheDB.TabIndex = 10;
            this.toolTip.SetToolTip(this.buttonOverviewTransferToCacheDB, "Transfer data to cache database");
            this.buttonOverviewTransferToCacheDB.UseVisualStyleBackColor = true;
            this.buttonOverviewTransferToCacheDB.Click += new System.EventHandler(this.buttonOverviewTransferToCacheDB_Click);
            // 
            // buttonOverviewTransferToPostgresDB
            // 
            this.buttonOverviewTransferToPostgresDB.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonOverviewTransferToPostgresDB.Image = global::DiversityCollection.Resource.ArrowNextNext;
            this.buttonOverviewTransferToPostgresDB.Location = new System.Drawing.Point(468, 170);
            this.buttonOverviewTransferToPostgresDB.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOverviewTransferToPostgresDB.Name = "buttonOverviewTransferToPostgresDB";
            this.buttonOverviewTransferToPostgresDB.Size = new System.Drawing.Size(20, 23);
            this.buttonOverviewTransferToPostgresDB.TabIndex = 11;
            this.toolTip.SetToolTip(this.buttonOverviewTransferToPostgresDB, "Transfer data for all projects");
            this.buttonOverviewTransferToPostgresDB.UseVisualStyleBackColor = true;
            this.buttonOverviewTransferToPostgresDB.Click += new System.EventHandler(this.buttonOverviewTransferToPostgresDB_Click);
            // 
            // projectPublishedTableAdapter
            // 
            this.projectPublishedTableAdapter.ClearBeforeFill = true;
            // 
            // enumMaterialCategoryTableAdapter
            // 
            this.enumMaterialCategoryTableAdapter.ClearBeforeFill = true;
            // 
            // enumRecordBasisTableAdapter
            // 
            this.enumRecordBasisTableAdapter.ClearBeforeFill = true;
            // 
            // enumPreparationTypeTableAdapter
            // 
            this.enumPreparationTypeTableAdapter.ClearBeforeFill = true;
            // 
            // enumTaxonomicGroupTableAdapter
            // 
            this.enumTaxonomicGroupTableAdapter.ClearBeforeFill = true;
            // 
            // enumKingdomTableAdapter
            // 
            this.enumKingdomTableAdapter.ClearBeforeFill = true;
            // 
            // taxonomicGroupInProjectTableAdapter
            // 
            this.taxonomicGroupInProjectTableAdapter.ClearBeforeFill = true;
            // 
            // enumLocalisationSystemTableAdapter
            // 
            this.enumLocalisationSystemTableAdapter.ClearBeforeFill = true;
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFeedback.Image = global::DiversityCollection.Resource.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(745, 0);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(25, 23);
            this.buttonFeedback.TabIndex = 1;
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // fKTaxonomicGroupInProjectProjectPublishedBindingSource
            // 
            this.fKTaxonomicGroupInProjectProjectPublishedBindingSource.DataMember = "FK_TaxonomicGroupInProject_ProjectPublished";
            this.fKTaxonomicGroupInProjectProjectPublishedBindingSource.DataSource = this.projectPublishedBindingSource;
            // 
            // imageListTransferSteps
            // 
            this.imageListTransferSteps.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTransferSteps.ImageStream")));
            this.imageListTransferSteps.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTransferSteps.Images.SetKeyName(0, "CollectionSpecimen.ico");
            this.imageListTransferSteps.Images.SetKeyName(1, "Localisation.ico");
            this.imageListTransferSteps.Images.SetKeyName(2, "Country.ico");
            this.imageListTransferSteps.Images.SetKeyName(3, "Plant.ico");
            this.imageListTransferSteps.Images.SetKeyName(4, "NameAccepted.ico");
            this.imageListTransferSteps.Images.SetKeyName(5, "Browse.ico");
            this.imageListTransferSteps.Images.SetKeyName(6, "Project.ico");
            this.imageListTransferSteps.Images.SetKeyName(7, "Agent.ico");
            this.imageListTransferSteps.Images.SetKeyName(8, "Icones.ico");
            this.imageListTransferSteps.Images.SetKeyName(9, "Specimen.ico");
            this.imageListTransferSteps.Images.SetKeyName(10, "info.ico");
            this.imageListTransferSteps.Images.SetKeyName(11, "Analysis.ico");
            this.imageListTransferSteps.Images.SetKeyName(12, "CacheCollectionSpecimen.ico");
            this.imageListTransferSteps.Images.SetKeyName(13, "CacheImages.ico");
            this.imageListTransferSteps.Images.SetKeyName(14, "CacheOrganisms.ico");
            this.imageListTransferSteps.Images.SetKeyName(15, "CachePart.ico");
            // 
            // FormCacheDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 616);
            this.Controls.Add(this.buttonFeedback);
            this.Controls.Add(this.tabControlMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCacheDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormCacheDB";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCacheDB_FormClosing);
            this.Load += new System.EventHandler(this.FormCacheDB_Load);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageTransfer.ResumeLayout(false);
            this.tabControlTransfer.ResumeLayout(false);
            this.tabPageDBtoCacheDB.ResumeLayout(false);
            this.tableLayoutPanelTransferSteps.ResumeLayout(false);
            this.tableLayoutPanelTransferSteps.PerformLayout();
            this.tableLayoutPanelVersion1.ResumeLayout(false);
            this.tableLayoutPanelVersion1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.projectPublishedBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCacheDB_1)).EndInit();
            this.tabPageCacheDBToPostGres.ResumeLayout(false);
            this.tableLayoutPanelPostgres.ResumeLayout(false);
            this.tableLayoutPanelPostgres.PerformLayout();
            this.groupBoxPostgresTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPostgresTable)).EndInit();
            this.tabPageBioCASE.ResumeLayout(false);
            this.tableLayoutPanelBioCASE.ResumeLayout(false);
            this.tableLayoutPanelBioCASE.PerformLayout();
            this.tabPageAdminCacheDB.ResumeLayout(false);
            this.tabControlDatabase.ResumeLayout(false);
            this.tabPageUpdate.ResumeLayout(false);
            this.tableLayoutPanelDatabase.ResumeLayout(false);
            this.tableLayoutPanelDatabase.PerformLayout();
            this.tabPageAnonymCollectors.ResumeLayout(false);
            this.tableLayoutPanelAnonymCollector.ResumeLayout(false);
            this.tableLayoutPanelAnonymCollector.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAnonymAgent)).EndInit();
            this.tabPageProjects.ResumeLayout(false);
            this.splitContainerProjects.Panel1.ResumeLayout(false);
            this.splitContainerProjects.Panel1.PerformLayout();
            this.splitContainerProjects.Panel2.ResumeLayout(false);
            this.splitContainerProjects.ResumeLayout(false);
            this.toolStripAdministrationProjects.ResumeLayout(false);
            this.toolStripAdministrationProjects.PerformLayout();
            this.tabControlProjectAdminTaxMat.ResumeLayout(false);
            this.tabPageProjectTaxonomicGroup.ResumeLayout(false);
            this.tableLayoutPanelTaxonomicGroups.ResumeLayout(false);
            this.tableLayoutPanelTaxonomicGroups.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.taxonomicGroupInProjectBindingSource)).EndInit();
            this.tabPageProjectMaterialCategory.ResumeLayout(false);
            this.tableLayoutPanelProjectMaterialCategory.ResumeLayout(false);
            this.tableLayoutPanelProjectMaterialCategory.PerformLayout();
            this.tabPageProjectLocalisationsystem.ResumeLayout(false);
            this.tableLayoutPanelProjectLocalisation.ResumeLayout(false);
            this.tableLayoutPanelProjectLocalisation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.enumLocalisationSystemBindingSource)).EndInit();
            this.groupBoxProjectSettings.ResumeLayout(false);
            this.tableLayoutPanelProjectSettings.ResumeLayout(false);
            this.groupBoxProjectCoordinates.ResumeLayout(false);
            this.tableLayoutPanelLocalisations.ResumeLayout(false);
            this.tableLayoutPanelLocalisations.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCoordinatePrecision)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProjectCoordiates)).EndInit();
            this.groupBoxProjectImages.ResumeLayout(false);
            this.groupBoxProjectImages.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProjectImages)).EndInit();
            this.tabPageLocalisation.ResumeLayout(false);
            this.tableLayoutPanelLocalisation.ResumeLayout(false);
            this.tableLayoutPanelLocalisation.PerformLayout();
            this.tabPageTaxonSynonymy.ResumeLayout(false);
            this.tabPageTaxonSynonymy.PerformLayout();
            this.tableLayoutPanelTaxonomySources.ResumeLayout(false);
            this.tableLayoutPanelTaxonomySources.PerformLayout();
            this.toolStripTaxonSources.ResumeLayout(false);
            this.toolStripTaxonSources.PerformLayout();
            this.tabPageMaterialCategory.ResumeLayout(false);
            this.tableLayoutPanelMaterialCategories.ResumeLayout(false);
            this.tableLayoutPanelMaterialCategories.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMaterialCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.enumRecordBasisBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.enumPreparationTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.enumMaterialCategoryBindingSource)).EndInit();
            this.tabPageKingdom.ResumeLayout(false);
            this.tableLayoutPanelKingdom.ResumeLayout(false);
            this.tableLayoutPanelKingdom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewKingdoms)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.enumKingdomBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.enumTaxonomicGroupBindingSource)).EndInit();
            this.tabPageSettings.ResumeLayout(false);
            this.tableLayoutPanelSettings.ResumeLayout(false);
            this.tableLayoutPanelSettings.PerformLayout();
            this.tabPageAdminPostgres.ResumeLayout(false);
            this.splitContainerAdminPostgres.Panel1.ResumeLayout(false);
            this.splitContainerAdminPostgres.Panel2.ResumeLayout(false);
            this.splitContainerAdminPostgres.ResumeLayout(false);
            this.tableLayoutPanelAdminPostgresList.ResumeLayout(false);
            this.tableLayoutPanelAdminPostgresList.PerformLayout();
            this.tableLayoutPanelPostgresAdmin.ResumeLayout(false);
            this.tableLayoutPanelPostgresAdmin.PerformLayout();
            this.tabControlPostgresAdmin.ResumeLayout(false);
            this.tabPagePostgresRoles.ResumeLayout(false);
            this.splitContainerPgAdminRoles.Panel1.ResumeLayout(false);
            this.splitContainerPgAdminRoles.Panel1.PerformLayout();
            this.splitContainerPgAdminRoles.Panel2.ResumeLayout(false);
            this.splitContainerPgAdminRoles.ResumeLayout(false);
            this.toolStripPgAdminRoles.ResumeLayout(false);
            this.toolStripPgAdminRoles.PerformLayout();
            this.tableLayoutPanelPgAdminRoles.ResumeLayout(false);
            this.tableLayoutPanelPgAdminRoles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPgAdminRoleGrants)).EndInit();
            this.tabPagePostgresGroups.ResumeLayout(false);
            this.tableLayoutPanelPostgresUser.ResumeLayout(false);
            this.tableLayoutPanelPostgresUser.PerformLayout();
            this.toolStripPostgresUser.ResumeLayout(false);
            this.toolStripPostgresUser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPostgresUser)).EndInit();
            this.tabPagePostgresTables.ResumeLayout(false);
            this.splitContainerPostgresTables.Panel1.ResumeLayout(false);
            this.splitContainerPostgresTables.Panel2.ResumeLayout(false);
            this.splitContainerPostgresTables.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPGtable)).EndInit();
            this.tabPagePostgresProjects.ResumeLayout(false);
            this.tableLayoutPanelPostgresAdminProjects.ResumeLayout(false);
            this.tableLayoutPanelPostgresAdminProjects.PerformLayout();
            this.tabControlPgProjects.ResumeLayout(false);
            this.tabPagePgProjectPackages.ResumeLayout(false);
            this.tableLayoutPanelPostgresPackages.ResumeLayout(false);
            this.tableLayoutPanelPostgresPackages.PerformLayout();
            this.tabPagePgProjectPermissions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPostgresProjectPermissions)).EndInit();
            this.tableLayoutPanelPostgresAdminLogin.ResumeLayout(false);
            this.tableLayoutPanelPostgresAdminLogin.PerformLayout();
            this.toolStripPostgresDBs.ResumeLayout(false);
            this.toolStripPostgresDBs.PerformLayout();
            this.tabPageOverview.ResumeLayout(false);
            this.tableLayoutPanelOverview.ResumeLayout(false);
            this.tableLayoutPanelOverview.PerformLayout();
            this.groupBoxOverviewSource.ResumeLayout(false);
            this.groupBoxOverviewCache.ResumeLayout(false);
            this.groupBoxOverviewPostgres.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOverviewSourceDB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOverviewCacheDB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOverviewPostgresDB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fKTaxonomicGroupInProjectProjectPublishedBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageTransfer;
        private System.Windows.Forms.TabPage tabPageAdminCacheDB;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelVersion1;
        private System.Windows.Forms.Button buttonV1StartCollectionTransfer;
        private System.Windows.Forms.Button buttonV1StartTaxonTransfer;
        private System.Windows.Forms.ListBox listBoxV1ProjectPublished;
        private System.Windows.Forms.ListBox listBoxV1ProjectsUnpublished;
        private System.Windows.Forms.Button buttonV1PublishProject;
        private System.Windows.Forms.Button buttonV1UnpublishProject;
        private System.Windows.Forms.Label labelProjectsUnpublished;
        private System.Windows.Forms.Label labelProjectsPublished;
        private System.Windows.Forms.Label labelV1LastSpecimenTransfer;
        private System.Windows.Forms.Label labelV1CurrentSpecimenNumber;
        private System.Windows.Forms.Label labelV1LastTaxonTransfer;
        private System.Windows.Forms.Label labelV1CurrentTaxa;
        private System.Windows.Forms.Button buttonV1CheckSpecimen;
        private System.Windows.Forms.Button buttonV1CheckTaxa;
        private System.Windows.Forms.SplitContainer splitContainerProjects;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTaxonomicGroups;
        private System.Windows.Forms.Label labelProjectTaxonomicGroup;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMaterialCategories;
        private System.Windows.Forms.Label labelMaterialCategories;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLocalisations;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDatabase;
        private System.Windows.Forms.Label labelDatabaseName;
        private System.Windows.Forms.Label labelServer;
        private System.Windows.Forms.TextBox textBoxServer;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label labelProjectsDatabase;
        private System.Windows.Forms.TextBox textBoxDatabaseName;
        private System.Windows.Forms.Button buttonLoginAdministration;
        private System.Windows.Forms.Button buttonUpdateDatabase;
        private System.Windows.Forms.TextBox textBoxProjectsDatabase;
        private System.Windows.Forms.TextBox textBoxCurrentDatabaseVersion;
        private System.Windows.Forms.Label labelCurrentDatabaseVersion;
        private System.Windows.Forms.TextBox textBoxAvailableDatabaseVersion;
        private System.Windows.Forms.NumericUpDown numericUpDownCoordinatePrecision;
        private System.Windows.Forms.CheckBox checkBoxCoordinatePrecision;
        private System.Windows.Forms.Label labelCoordinatePrecision;
        private System.Windows.Forms.Button buttonHighResolutionImages;
        private DataSetCacheDB_1 dataSetCacheDB_1;
        private System.Windows.Forms.BindingSource projectPublishedBindingSource;
        private DataSetCacheDB_1TableAdapters.ProjectPublishedTableAdapter projectPublishedTableAdapter;
        private System.Windows.Forms.DataGridView dataGridViewMaterialCategory;
        private System.Windows.Forms.BindingSource enumMaterialCategoryBindingSource;
        private DataSetCacheDB_1TableAdapters.EnumMaterialCategoryTableAdapter enumMaterialCategoryTableAdapter;
        private System.Windows.Forms.BindingSource enumRecordBasisBindingSource;
        private DataSetCacheDB_1TableAdapters.EnumRecordBasisTableAdapter enumRecordBasisTableAdapter;
        private System.Windows.Forms.BindingSource enumPreparationTypeBindingSource;
        private DataSetCacheDB_1TableAdapters.EnumPreparationTypeTableAdapter enumPreparationTypeTableAdapter;
        private System.Windows.Forms.TabControl tabControlDatabase;
        private System.Windows.Forms.TabPage tabPageUpdate;
        private System.Windows.Forms.TabPage tabPageMaterialCategory;
        private System.Windows.Forms.Label labelMaterialCategoryMissing;
        private System.Windows.Forms.Button buttonMaterialAdd;
        private System.Windows.Forms.Button buttonMaterialRemove;
        private System.Windows.Forms.ListBox listBoxMaterialNotTransferred;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TabPage tabPageKingdom;
        private System.Windows.Forms.ImageList imageListTabControl;
        private System.Windows.Forms.TabPage tabPageLocalisation;
        private System.Windows.Forms.ListBox listBoxProjects;
        private System.Windows.Forms.Label labelProjectTaxonomicGroupNotPublished;
        private System.Windows.Forms.Label labelProjectTaxonomicGroupPublished;
        private System.Windows.Forms.ListBox listBoxProjectTaxonomicGroupNotPublished;
        private System.Windows.Forms.ListBox listBoxProjectTaxonomicGroupPublished;
        private System.Windows.Forms.Button buttonProjectTaxonomicGroupPublished;
        private System.Windows.Forms.Button buttonProjectTaxonomicGroupNotPublished;
        private System.Windows.Forms.GroupBox groupBoxProjectCoordinates;
        private System.Windows.Forms.PictureBox pictureBoxProjectCoordiates;
        private System.Windows.Forms.GroupBox groupBoxProjectImages;
        private System.Windows.Forms.CheckBox checkBoxHighResolutionImage;
        private System.Windows.Forms.PictureBox pictureBoxProjectImages;
        private System.Windows.Forms.DataGridView dataGridViewKingdoms;
        private System.Windows.Forms.BindingSource enumTaxonomicGroupBindingSource;
        private DataSetCacheDB_1TableAdapters.EnumTaxonomicGroupTableAdapter enumTaxonomicGroupTableAdapter;
        private System.Windows.Forms.BindingSource enumKingdomBindingSource;
        private DataSetCacheDB_1TableAdapters.EnumKingdomTableAdapter enumKingdomTableAdapter;
        private System.Windows.Forms.BindingSource taxonomicGroupInProjectBindingSource;
        private DataSetCacheDB_1TableAdapters.TaxonomicGroupInProjectTableAdapter taxonomicGroupInProjectTableAdapter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLocalisation;
        private System.Windows.Forms.Label labelLocalisationNotPublished;
        private System.Windows.Forms.ListBox listBoxLocalisationNotPublished;
        private System.Windows.Forms.Label labelLocalisationPublished;
        private System.Windows.Forms.ListBox listBoxLocalisationPublished;
        private System.Windows.Forms.Button buttonLocalisationPublished;
        private System.Windows.Forms.Button buttonLocalisationNotPublished;
        private System.Windows.Forms.Button buttonLocalisationPublishedUp;
        private System.Windows.Forms.Button buttonLocalisationPublishedDown;
        private System.Windows.Forms.BindingSource enumLocalisationSystemBindingSource;
        private DataSetCacheDB_1TableAdapters.EnumLocalisationSystemTableAdapter enumLocalisationSystemTableAdapter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelKingdom;
        private System.Windows.Forms.Label labelKingdomNotPublished;
        private System.Windows.Forms.ListBox listBoxKingdomNotPublished;
        private System.Windows.Forms.Button buttonKingdomNotPublished;
        private System.Windows.Forms.Button buttonKingdomPublished;
        private System.Windows.Forms.Label labelKingdomPublished;
        private System.Windows.Forms.DataGridViewTextBoxColumn codeDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayTextDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewComboBoxColumn kingdomDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button buttonSaveKingdoms;
        private System.Windows.Forms.Button buttonSaveMaterial;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Label labelLocalisation;
        private System.Windows.Forms.Label labelMaterialPublished;
        private System.Windows.Forms.Label labelKingdoms;
        private System.Windows.Forms.Button buttonTransferCountryIcoCode;
        private System.Windows.Forms.Label labelWarningNoGazetteer;
        private System.Windows.Forms.BindingSource fKTaxonomicGroupInProjectProjectPublishedBindingSource;
        private System.Windows.Forms.TabPage tabPageTaxonSynonymy;
        private System.Windows.Forms.Panel panelTaxonSources;
        private System.Windows.Forms.ToolStrip toolStripTaxonSources;
        private System.Windows.Forms.ToolStripLabel toolStripLabelTaxonSources;
        private System.Windows.Forms.ToolStripButton toolStripButtonTaxonSourceAdd;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTaxonomySources;
        private System.Windows.Forms.Label labelTaxonSourceDB;
        private System.Windows.Forms.Label labelTaxonSourceView;
        private System.Windows.Forms.ImageList imageListTransferSteps;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTransferSteps;
        private System.Windows.Forms.Panel panelTransferSteps;
        private System.Windows.Forms.Button buttonStartTransfer;
        private System.Windows.Forms.Label labelLastTransfer;
        private System.Windows.Forms.DataGridViewTextBoxColumn codeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayTextDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn recordBasisDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn preparationTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn categoryOrderDataGridViewTextBoxColumn;
        private System.Windows.Forms.GroupBox groupBoxProjectSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProjectSettings;
        private System.Windows.Forms.Button buttonProjectSettings;
        private System.Windows.Forms.Button buttonProjectRecordURI;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSettings;
        private System.Windows.Forms.Label labelSettingsHeader;
        private System.Windows.Forms.Button buttonProjectTaxonomicGroupTransferExisting;
        private System.Windows.Forms.TabControl tabControlTransfer;
        private System.Windows.Forms.TabPage tabPageDBtoCacheDB;
        private System.Windows.Forms.TabPage tabPageCacheDBToPostGres;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPostgres;
        private System.Windows.Forms.TabPage tabPageBioCASE;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBioCASE;
        private System.Windows.Forms.Label labelBioCASEsources;
        private System.Windows.Forms.TextBox textBoxBioCASEsources;
        private System.Windows.Forms.Button buttonBioCASErefresh;
        private System.Windows.Forms.WebBrowser webBrowserBioCASE;
        private System.Windows.Forms.Button buttonBioCASEback;
        private System.Windows.Forms.ListBox listBoxPostgresTables;
        private System.Windows.Forms.Label labelPostgresTables;
        private System.Windows.Forms.GroupBox groupBoxPostgresTable;
        private System.Windows.Forms.DataGridView dataGridViewPostgresTable;
        private System.Windows.Forms.SplitContainer splitContainerAdminPostgres;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAdminPostgresList;
        private System.Windows.Forms.Label labelPostgresDBlist;
        private System.Windows.Forms.ListBox listBoxPostgresDBs;
        private System.Windows.Forms.ToolStrip toolStripPostgresDBs;
        private System.Windows.Forms.ToolStripButton toolStripButtonPostgresConnect;
        private System.Windows.Forms.ToolStripButton toolStripButtonPostgresNewDB;
        private System.Windows.Forms.ToolStripButton toolStripButtonPostgresDeleteDB;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPostgresAdmin;
        private System.Windows.Forms.Label labelPostgresVersion;
        private System.Windows.Forms.Button buttonPostgresUpdate;
        private System.Windows.Forms.ListBox listBoxPostgresUser;
        private System.Windows.Forms.ToolStrip toolStripPostgresUser;
        private System.Windows.Forms.ToolStripButton toolStripButtonPostgresUserAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonPostgresUserDelete;
        private System.Windows.Forms.Label labelPostgresProject;
        private System.Windows.Forms.Button buttonPostgresTransferProject;
        private System.Windows.Forms.ComboBox comboBoxPostgresProject;
        private System.Windows.Forms.TabControl tabControlPostgresAdmin;
        private System.Windows.Forms.TabPage tabPagePostgresGroups;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPostgresUser;
        private System.Windows.Forms.TabPage tabPagePostgresProjects;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPostgresAdminProjects;
        private System.Windows.Forms.Label labelPostgresProjectsAvailable;
        private System.Windows.Forms.ListBox listBoxPostgresProjectsAvailable;
        private System.Windows.Forms.Button buttonPostgresProjectsEstablish;
        private System.Windows.Forms.Label labelPostgresProjectsEstablished;
        private System.Windows.Forms.DataGridView dataGridViewPostgresUser;
        private System.Windows.Forms.DataGridView dataGridViewPostgresProjectPermissions;
        private System.Windows.Forms.ToolStripButton toolStripButtonPostgresUserPassword;
        private System.Windows.Forms.Panel panelPostgresEstablishedProjects;
        private System.Windows.Forms.ToolStrip toolStripAdministrationProjects;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectRemove;
        private System.Windows.Forms.TabPage tabPagePostgresTables;
        private System.Windows.Forms.SplitContainer splitContainerPostgresTables;
        private System.Windows.Forms.ListBox listBoxPGtables;
        private System.Windows.Forms.DataGridView dataGridViewPGtable;
        private System.Windows.Forms.ToolStripButton toolStripButtonPostgresRoleAdministration;
        private System.Windows.Forms.TabPage tabPageOverview;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelOverview;
        private System.Windows.Forms.GroupBox groupBoxOverviewSource;
        private System.Windows.Forms.GroupBox groupBoxOverviewCache;
        private System.Windows.Forms.GroupBox groupBoxOverviewPostgres;
        private System.Windows.Forms.Panel panelOverviewSource;
        private System.Windows.Forms.Panel panelOverviewCache;
        private System.Windows.Forms.Panel panelOverviewPostgres;
        private System.Windows.Forms.TabPage tabPagePostgresRoles;
        private System.Windows.Forms.Button buttonPostgresConnect;
        private System.Windows.Forms.Label labelPostgresConnection;
        private System.Windows.Forms.Label labelPgAdminConnection;
        private System.Windows.Forms.SplitContainer splitContainerPgAdminRoles;
        private System.Windows.Forms.ListBox listBoxPgAdminRoles;
        private System.Windows.Forms.ToolStrip toolStripPgAdminRoles;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPgAdminRoles;
        private System.Windows.Forms.DataGridView dataGridViewPgAdminRoleGrants;
        private System.Windows.Forms.ToolStripButton toolStripButtonPgAdminRoleAdministration;
        private System.Windows.Forms.Label labelPgAdminRoleGrants;
        private System.Windows.Forms.ToolStripButton toolStripButtonPgAdminRoles;
        private System.Windows.Forms.ProgressBar progressBarPostgresTransfer;
        private System.Windows.Forms.Label labelPostgresTransferMessage;
        private System.Windows.Forms.TabControl tabControlPgProjects;
        private System.Windows.Forms.TabPage tabPagePgProjectPackages;
        private System.Windows.Forms.TabPage tabPagePgProjectPermissions;
        private System.Windows.Forms.Panel panelPostgresProjectPackages;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPostgresPackages;
        private System.Windows.Forms.Label labelPostgresPackagesAvailable;
        private System.Windows.Forms.Label labelPostgresPackagesEstablished;
        private System.Windows.Forms.ListBox listBoxPostgresPackagesAvailable;
        private System.Windows.Forms.Button buttonPostgresPackageEstablish;
        private System.Windows.Forms.Label labelOverviewSourceDB;
        private System.Windows.Forms.PictureBox pictureBoxOverviewSourceDB;
        private System.Windows.Forms.PictureBox pictureBoxOverviewCacheDB;
        private System.Windows.Forms.Label labelOverviewCacheDB;
        private System.Windows.Forms.PictureBox pictureBoxOverviewPostgresDB;
        private System.Windows.Forms.Label labelOverviewPostgresDB;
        private System.Windows.Forms.Button buttonOverviewPostgresDBConnect;
        private System.Windows.Forms.Button buttonOverviewTransferToCacheDB;
        private System.Windows.Forms.Button buttonOverviewTransferToPostgresDB;
        private System.Windows.Forms.TabPage tabPageProjects;
        private System.Windows.Forms.TabPage tabPageAdminPostgres;
        private System.Windows.Forms.Label labelProjectPackages;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPostgresAdminLogin;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorPgAdmin;
        private System.Windows.Forms.TabPage tabPageAnonymCollectors;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAnonymCollector;
        private System.Windows.Forms.Label labelCollectorNotAnomym;
        private System.Windows.Forms.Label labelCollectorAnonym;
        private System.Windows.Forms.DataGridView dataGridViewAnonymAgent;
        private System.Windows.Forms.ListBox listBoxAnonymCollector;
        private System.Windows.Forms.Button buttonCollectorIsAnonym;
        private System.Windows.Forms.Button buttonCollectorNotAnonym;
        private System.Windows.Forms.TabControl tabControlProjectAdminTaxMat;
        private System.Windows.Forms.TabPage tabPageProjectTaxonomicGroup;
        private System.Windows.Forms.TabPage tabPageProjectMaterialCategory;
        private System.Windows.Forms.Button buttonCheckProjectsDatabase;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProjectMaterialCategory;
        private System.Windows.Forms.Label labelProjectMaterialCategoryHeader;
        private System.Windows.Forms.Label labelProjectMaterialCategoryNotPublished;
        private System.Windows.Forms.Label labelProjectMaterialCategoryPublished;
        private System.Windows.Forms.ListBox listBoxProjectMaterialCategoryNotPublished;
        private System.Windows.Forms.ListBox listBoxProjectMaterialCategoryPublished;
        private System.Windows.Forms.Button buttonProjectMaterialCategoryPublishe;
        private System.Windows.Forms.Button buttonProjectMaterialCategoryWithhold;
        private System.Windows.Forms.Button buttonProjectMaterialCategoryTransferExisting;
        private System.Windows.Forms.TabPage tabPageProjectLocalisationsystem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProjectLocalisation;
        private System.Windows.Forms.Label labelProjectLocalisationNotPublished;
        private System.Windows.Forms.ListBox listBoxProjectLocalisationNotPublished;
        private System.Windows.Forms.Label labelProjectLocalisationPublished;
        private System.Windows.Forms.ListBox listBoxProjectLocalisationPubished;
        private System.Windows.Forms.Button buttonProjectLocalisationPublish;
        private System.Windows.Forms.Button buttonProjectLocalisationHide;
        private System.Windows.Forms.Button buttonProjectLocalisationUp;
        private System.Windows.Forms.Button buttonProjectLocalisationDown;
        private System.Windows.Forms.Label labelProjectLocalisationHeader;
    }
}