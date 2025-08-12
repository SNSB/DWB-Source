namespace DiversityCollection
{
    partial class FormImportList
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Collection specimen");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Collection event", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Collection specimen");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Collection event", new System.Windows.Forms.TreeNode[] {
            treeNode3});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImportList));
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Collection specimen");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Collection specimen");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Collection event", new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode6});
            this.tableLayoutPanelImport = new System.Windows.Forms.TableLayoutPanel();
            this.buttonStartImport = new System.Windows.Forms.Button();
            this.buttonAnalyse = new System.Windows.Forms.Button();
            this.treeViewAnalysis = new System.Windows.Forms.TreeView();
            this.toolStripNavigation = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonFirst = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPrevious = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabelCurrent = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButtonNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLast = new System.Windows.Forms.ToolStripButton();
            this.labelUpTo = new System.Windows.Forms.Label();
            this.numericUpDownUpTo = new System.Windows.Forms.NumericUpDown();
            this.labelImport = new System.Windows.Forms.Label();
            this.radioButtonImportAll = new System.Windows.Forms.RadioButton();
            this.radioButtonImportFirstLines = new System.Windows.Forms.RadioButton();
            this.numericUpDownImport = new System.Windows.Forms.NumericUpDown();
            this.labelImportUpTo = new System.Windows.Forms.Label();
            this.progressBarImport = new System.Windows.Forms.ProgressBar();
            this.checkBoxImportEmptyData = new System.Windows.Forms.CheckBox();
            this.labelTotalLines = new System.Windows.Forms.Label();
            this.textBoxTotalLines = new System.Windows.Forms.TextBox();
            this.groupBoxPresettings = new System.Windows.Forms.GroupBox();
            this.buttonLoadSettings = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.tabControlPresettings = new System.Windows.Forms.TabControl();
            this.tabPageColumnMapping = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelColumnMapping = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridViewColumnMappingSource = new System.Windows.Forms.DataGridView();
            this.labelColumnMappingTable = new System.Windows.Forms.Label();
            this.comboBoxColumnMappingTable = new System.Windows.Forms.ComboBox();
            this.comboBoxColumnMappingColumn = new System.Windows.Forms.ComboBox();
            this.labelColumnMappingColumn = new System.Windows.Forms.Label();
            this.labelColumnMappingAlias = new System.Windows.Forms.Label();
            this.buttonColumnMappingSave = new System.Windows.Forms.Button();
            this.dataGridViewColumnMapping = new System.Windows.Forms.DataGridView();
            this.labelDataStartInLine = new System.Windows.Forms.Label();
            this.numericUpDownDataStartInLine = new System.Windows.Forms.NumericUpDown();
            this.labelColumnGroup = new System.Windows.Forms.Label();
            this.comboBoxColumnGroup = new System.Windows.Forms.ComboBox();
            this.comboBoxColumnMappingAlias = new System.Windows.Forms.ComboBox();
            this.buttonColumnMappingSaveToFile = new System.Windows.Forms.Button();
            this.buttonColumnMappingImportFromFile = new System.Windows.Forms.Button();
            this.buttonIgnoreColumn = new System.Windows.Forms.Button();
            this.tabPageEvent = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelEventHierarchy = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonEventsSeparate = new System.Windows.Forms.RadioButton();
            this.treeViewEventsSeparate = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.radioButtonEventsInGroups = new System.Windows.Forms.RadioButton();
            this.treeViewEventsInGroups = new System.Windows.Forms.TreeView();
            this.imageListGrey = new System.Windows.Forms.ImageList(this.components);
            this.checkBoxEventSeries = new System.Windows.Forms.CheckBox();
            this.labelEventSeriesCode = new System.Windows.Forms.Label();
            this.labelEventSeriesDescription = new System.Windows.Forms.Label();
            this.labelEventSeriesNotes = new System.Windows.Forms.Label();
            this.textBoxEventSeriesCode = new System.Windows.Forms.TextBox();
            this.textBoxEventSeriesDescription = new System.Windows.Forms.TextBox();
            this.textBoxEventSeriesNotes = new System.Windows.Forms.TextBox();
            this.buttonGetEventSeries = new System.Windows.Forms.Button();
            this.tableLayoutPanelEventDate = new System.Windows.Forms.TableLayoutPanel();
            this.labelCollectionDate = new System.Windows.Forms.Label();
            this.userControlDatePanelCollectionDate = new DiversityWorkbench.UserControlDatePanel();
            this.labelCollectionTime = new System.Windows.Forms.Label();
            this.textBoxCollectionTime = new System.Windows.Forms.TextBox();
            this.labelCollectionTimeSpan = new System.Windows.Forms.Label();
            this.textBoxCollectionTimeSpan = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanelCollector = new System.Windows.Forms.TableLayoutPanel();
            this.labelCollector = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntryCollector = new DiversityWorkbench.UserControlModuleRelatedEntry();
            this.tableLayoutPanelPlaceAndCountry = new System.Windows.Forms.TableLayoutPanel();
            this.labelGazetteer = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntryGazetteer = new DiversityWorkbench.UserControlModuleRelatedEntry();
            this.labelCountry = new System.Windows.Forms.Label();
            this.comboBoxCountry = new System.Windows.Forms.ComboBox();
            this.tabPageSpecimen = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelPresetSpecimen = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxLabelType = new System.Windows.Forms.ComboBox();
            this.userControlModuleRelatedEntryExsiccate = new DiversityWorkbench.UserControlModuleRelatedEntry();
            this.labelLabelType = new System.Windows.Forms.Label();
            this.comboBoxImageType = new System.Windows.Forms.ComboBox();
            this.labelImageType = new System.Windows.Forms.Label();
            this.userControlDatePanelAccessionDate = new DiversityWorkbench.UserControlDatePanel();
            this.labelExsiccate = new System.Windows.Forms.Label();
            this.labelAccessionDate = new System.Windows.Forms.Label();
            this.labelProject = new System.Windows.Forms.Label();
            this.comboBoxProject = new System.Windows.Forms.ComboBox();
            this.labelDepositor = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntryDepositor = new DiversityWorkbench.UserControlModuleRelatedEntry();
            this.tabPageUnits = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelPresetUnits = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelUnit1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelTaxonomicGroup = new System.Windows.Forms.Label();
            this.radioButtonUnit1isHost = new System.Windows.Forms.RadioButton();
            this.comboBoxTaxonomicGroup = new System.Windows.Forms.ComboBox();
            this.labelIdentificationListUnit1 = new System.Windows.Forms.Label();
            this.listBoxIdentificationsUnit1 = new System.Windows.Forms.ListBox();
            this.buttonIdentification1Down = new System.Windows.Forms.Button();
            this.textBoxUnitTable1 = new System.Windows.Forms.TextBox();
            this.checkBox2Units = new System.Windows.Forms.CheckBox();
            this.labelIdentificationExampleFor1 = new System.Windows.Forms.Label();
            this.tableLayoutPanelUnit2 = new System.Windows.Forms.TableLayoutPanel();
            this.labelIdentificationListUnit2 = new System.Windows.Forms.Label();
            this.listBoxIdentificationsUnit2 = new System.Windows.Forms.ListBox();
            this.labelTaxonomicGroupUnit2 = new System.Windows.Forms.Label();
            this.comboBoxTaxonomicGroupUnit2 = new System.Windows.Forms.ComboBox();
            this.labelHost = new System.Windows.Forms.Label();
            this.radioButtonUnit2isHost = new System.Windows.Forms.RadioButton();
            this.buttonMoveToUnit2 = new System.Windows.Forms.Button();
            this.buttonMoveToUnit1 = new System.Windows.Forms.Button();
            this.buttonIdentification2Down = new System.Windows.Forms.Button();
            this.buttonSwitchUnitTables = new System.Windows.Forms.Button();
            this.textBoxUnitTable2 = new System.Windows.Forms.TextBox();
            this.labelIdentificationExampleFor2 = new System.Windows.Forms.Label();
            this.checkBoxTaxonAnywhere = new System.Windows.Forms.CheckBox();
            this.checkBoxInsertRelation = new System.Windows.Forms.CheckBox();
            this.radioButtonMain1 = new System.Windows.Forms.RadioButton();
            this.radioButtonMain2 = new System.Windows.Forms.RadioButton();
            this.labelMain = new System.Windows.Forms.Label();
            this.comboBoxRelationType = new System.Windows.Forms.ComboBox();
            this.tabPageStorage = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelStorage = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxDerivedFrom2 = new System.Windows.Forms.ComboBox();
            this.comboBoxDerivedFrom = new System.Windows.Forms.ComboBox();
            this.labelDerivedFrom2 = new System.Windows.Forms.Label();
            this.labelDerivedFrom = new System.Windows.Forms.Label();
            this.comboBoxMaterialCategory2 = new System.Windows.Forms.ComboBox();
            this.comboBoxMaterialCategory = new System.Windows.Forms.ComboBox();
            this.labelMaterialCategory2 = new System.Windows.Forms.Label();
            this.labelMaterial = new System.Windows.Forms.Label();
            this.userControlHierarchySelectorCollection2 = new DiversityWorkbench.UserControlHierarchySelector();
            this.comboBoxCollection2 = new System.Windows.Forms.ComboBox();
            this.userControlHierarchySelectorCollection = new DiversityWorkbench.UserControlHierarchySelector();
            this.comboBoxCollection = new System.Windows.Forms.ComboBox();
            this.labelCollection2 = new System.Windows.Forms.Label();
            this.labelCollection = new System.Windows.Forms.Label();
            this.labelPart2 = new System.Windows.Forms.Label();
            this.labelPart1 = new System.Windows.Forms.Label();
            this.panelPartsContained = new System.Windows.Forms.Panel();
            this.radioButtonManyParts = new System.Windows.Forms.RadioButton();
            this.radioButtonTwoParts = new System.Windows.Forms.RadioButton();
            this.radioButtonOnePart = new System.Windows.Forms.RadioButton();
            this.radioButtonNoPart = new System.Windows.Forms.RadioButton();
            this.tabPageAdding = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelAdding = new System.Windows.Forms.TableLayoutPanel();
            this.labelToPartMatch = new System.Windows.Forms.Label();
            this.pictureBoxAddPartToPart = new System.Windows.Forms.PictureBox();
            this.pictureBoxAddToPart = new System.Windows.Forms.PictureBox();
            this.pictureBoxArrowAddToPart = new System.Windows.Forms.PictureBox();
            this.radioButtonAddPartsToSpecimen = new System.Windows.Forms.RadioButton();
            this.radioButtonAddPartsToPart = new System.Windows.Forms.RadioButton();
            this.pictureBoxAddToSpecimen = new System.Windows.Forms.PictureBox();
            this.pictureBoxArrowAdd = new System.Windows.Forms.PictureBox();
            this.pictureBoxAddPartToSpecimen = new System.Windows.Forms.PictureBox();
            this.labelAddPartToSpecimenMatchColumn = new System.Windows.Forms.Label();
            this.comboBoxAddPartToPartMatchingColumn = new System.Windows.Forms.ComboBox();
            this.labelAddToSpecimenMatch = new System.Windows.Forms.Label();
            this.tabPageHidden = new System.Windows.Forms.TabPage();
            this.userControlModuleRelatedEntryIdentifiedBy = new DiversityWorkbench.UserControlModuleRelatedEntry();
            this.labelIdentifiedBy = new System.Windows.Forms.Label();
            this.labelIdentification = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntryIdentification = new DiversityWorkbench.UserControlModuleRelatedEntry();
            this.tabPageFile = new System.Windows.Forms.TabPage();
            this.dataGridViewFile = new System.Windows.Forms.DataGridView();
            this.textBoxImportFile = new System.Windows.Forms.TextBox();
            this.labelImportFile = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buttonReload = new System.Windows.Forms.Button();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanelHeader = new System.Windows.Forms.TableLayoutPanel();
            this.buttonHeaderFeedback = new System.Windows.Forms.Button();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.labelEncoding = new System.Windows.Forms.Label();
            this.comboBoxEncoding = new System.Windows.Forms.ComboBox();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.dataSetCollectionSpecimen = new DiversityCollection.Datasets.DataSetCollectionSpecimen();
            this.tableLayoutPanelImport.SuspendLayout();
            this.toolStripNavigation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownUpTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownImport)).BeginInit();
            this.groupBoxPresettings.SuspendLayout();
            this.tabControlPresettings.SuspendLayout();
            this.tabPageColumnMapping.SuspendLayout();
            this.tableLayoutPanelColumnMapping.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewColumnMappingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewColumnMapping)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDataStartInLine)).BeginInit();
            this.tabPageEvent.SuspendLayout();
            this.tableLayoutPanelEventHierarchy.SuspendLayout();
            this.tableLayoutPanelEventDate.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanelCollector.SuspendLayout();
            this.tableLayoutPanelPlaceAndCountry.SuspendLayout();
            this.tabPageSpecimen.SuspendLayout();
            this.tableLayoutPanelPresetSpecimen.SuspendLayout();
            this.tabPageUnits.SuspendLayout();
            this.tableLayoutPanelPresetUnits.SuspendLayout();
            this.tableLayoutPanelUnit1.SuspendLayout();
            this.tableLayoutPanelUnit2.SuspendLayout();
            this.tabPageStorage.SuspendLayout();
            this.tableLayoutPanelStorage.SuspendLayout();
            this.panelPartsContained.SuspendLayout();
            this.tabPageAdding.SuspendLayout();
            this.tableLayoutPanelAdding.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAddPartToPart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAddToPart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxArrowAddToPart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAddToSpecimen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxArrowAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAddPartToSpecimen)).BeginInit();
            this.tabPageHidden.SuspendLayout();
            this.tabPageFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFile)).BeginInit();
            this.tableLayoutPanelHeader.SuspendLayout();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCollectionSpecimen)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelImport
            // 
            this.tableLayoutPanelImport.ColumnCount = 10;
            this.tableLayoutPanelImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanelImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanelImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelImport.Controls.Add(this.buttonStartImport, 0, 5);
            this.tableLayoutPanelImport.Controls.Add(this.buttonAnalyse, 0, 0);
            this.tableLayoutPanelImport.Controls.Add(this.treeViewAnalysis, 2, 0);
            this.tableLayoutPanelImport.Controls.Add(this.toolStripNavigation, 0, 1);
            this.tableLayoutPanelImport.Controls.Add(this.labelUpTo, 0, 2);
            this.tableLayoutPanelImport.Controls.Add(this.numericUpDownUpTo, 1, 2);
            this.tableLayoutPanelImport.Controls.Add(this.labelImport, 2, 5);
            this.tableLayoutPanelImport.Controls.Add(this.radioButtonImportAll, 3, 5);
            this.tableLayoutPanelImport.Controls.Add(this.radioButtonImportFirstLines, 4, 5);
            this.tableLayoutPanelImport.Controls.Add(this.numericUpDownImport, 5, 5);
            this.tableLayoutPanelImport.Controls.Add(this.labelImportUpTo, 6, 5);
            this.tableLayoutPanelImport.Controls.Add(this.progressBarImport, 8, 5);
            this.tableLayoutPanelImport.Controls.Add(this.checkBoxImportEmptyData, 7, 5);
            this.tableLayoutPanelImport.Controls.Add(this.labelTotalLines, 0, 3);
            this.tableLayoutPanelImport.Controls.Add(this.textBoxTotalLines, 1, 3);
            this.tableLayoutPanelImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelImport.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelImport.Name = "tableLayoutPanelImport";
            this.tableLayoutPanelImport.RowCount = 6;
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelImport.Size = new System.Drawing.Size(992, 349);
            this.tableLayoutPanelImport.TabIndex = 0;
            // 
            // buttonStartImport
            // 
            this.tableLayoutPanelImport.SetColumnSpan(this.buttonStartImport, 2);
            this.buttonStartImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonStartImport.Enabled = false;
            this.buttonStartImport.Location = new System.Drawing.Point(3, 319);
            this.buttonStartImport.Name = "buttonStartImport";
            this.buttonStartImport.Size = new System.Drawing.Size(109, 27);
            this.buttonStartImport.TabIndex = 2;
            this.buttonStartImport.Text = "Start import";
            this.buttonStartImport.UseVisualStyleBackColor = true;
            this.buttonStartImport.Click += new System.EventHandler(this.buttonStartImport_Click);
            // 
            // buttonAnalyse
            // 
            this.tableLayoutPanelImport.SetColumnSpan(this.buttonAnalyse, 2);
            this.buttonAnalyse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAnalyse.Enabled = false;
            this.buttonAnalyse.Location = new System.Drawing.Point(3, 3);
            this.buttonAnalyse.Name = "buttonAnalyse";
            this.buttonAnalyse.Size = new System.Drawing.Size(109, 37);
            this.buttonAnalyse.TabIndex = 5;
            this.buttonAnalyse.Text = "Analyse data";
            this.buttonAnalyse.UseVisualStyleBackColor = true;
            this.buttonAnalyse.Click += new System.EventHandler(this.buttonAnalyse_Click);
            // 
            // treeViewAnalysis
            // 
            this.tableLayoutPanelImport.SetColumnSpan(this.treeViewAnalysis, 8);
            this.treeViewAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewAnalysis.Location = new System.Drawing.Point(118, 3);
            this.treeViewAnalysis.Name = "treeViewAnalysis";
            this.tableLayoutPanelImport.SetRowSpan(this.treeViewAnalysis, 5);
            this.treeViewAnalysis.Size = new System.Drawing.Size(871, 310);
            this.treeViewAnalysis.TabIndex = 6;
            // 
            // toolStripNavigation
            // 
            this.toolStripNavigation.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanelImport.SetColumnSpan(this.toolStripNavigation, 2);
            this.toolStripNavigation.Enabled = false;
            this.toolStripNavigation.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripNavigation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonFirst,
            this.toolStripButtonPrevious,
            this.toolStripLabelCurrent,
            this.toolStripButtonNext,
            this.toolStripButtonLast});
            this.toolStripNavigation.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStripNavigation.Location = new System.Drawing.Point(0, 43);
            this.toolStripNavigation.Name = "toolStripNavigation";
            this.toolStripNavigation.Size = new System.Drawing.Size(115, 25);
            this.toolStripNavigation.TabIndex = 7;
            this.toolStripNavigation.Text = "toolStrip1";
            // 
            // toolStripButtonFirst
            // 
            this.toolStripButtonFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFirst.Image = global::DiversityCollection.Resource.ArrowFirst;
            this.toolStripButtonFirst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFirst.Name = "toolStripButtonFirst";
            this.toolStripButtonFirst.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonFirst.Text = "Move to first dataset";
            this.toolStripButtonFirst.Click += new System.EventHandler(this.toolStripButtonFirst_Click);
            // 
            // toolStripButtonPrevious
            // 
            this.toolStripButtonPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPrevious.Image = global::DiversityCollection.Resource.ArrowPrevious1;
            this.toolStripButtonPrevious.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPrevious.Name = "toolStripButtonPrevious";
            this.toolStripButtonPrevious.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPrevious.Text = "Move to previous dataset";
            this.toolStripButtonPrevious.Click += new System.EventHandler(this.toolStripButtonPrevious_Click);
            // 
            // toolStripLabelCurrent
            // 
            this.toolStripLabelCurrent.Name = "toolStripLabelCurrent";
            this.toolStripLabelCurrent.Size = new System.Drawing.Size(13, 22);
            this.toolStripLabelCurrent.Text = "1";
            this.toolStripLabelCurrent.ToolTipText = "Current dataset";
            // 
            // toolStripButtonNext
            // 
            this.toolStripButtonNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNext.Image = global::DiversityCollection.Resource.ArrowNext1;
            this.toolStripButtonNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNext.Name = "toolStripButtonNext";
            this.toolStripButtonNext.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNext.Text = "Move to next dataset";
            this.toolStripButtonNext.Click += new System.EventHandler(this.toolStripButtonNext_Click);
            // 
            // toolStripButtonLast
            // 
            this.toolStripButtonLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLast.Image = global::DiversityCollection.Resource.ArrowLast;
            this.toolStripButtonLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLast.Name = "toolStripButtonLast";
            this.toolStripButtonLast.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonLast.Text = "Move to last dataset";
            this.toolStripButtonLast.Click += new System.EventHandler(this.toolStripButtonLast_Click);
            // 
            // labelUpTo
            // 
            this.labelUpTo.AutoSize = true;
            this.labelUpTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUpTo.Location = new System.Drawing.Point(3, 74);
            this.labelUpTo.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelUpTo.Name = "labelUpTo";
            this.labelUpTo.Size = new System.Drawing.Size(42, 20);
            this.labelUpTo.TabIndex = 77;
            this.labelUpTo.Text = "Up to:";
            this.labelUpTo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // numericUpDownUpTo
            // 
            this.numericUpDownUpTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDownUpTo.Location = new System.Drawing.Point(45, 71);
            this.numericUpDownUpTo.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.numericUpDownUpTo.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownUpTo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownUpTo.Name = "numericUpDownUpTo";
            this.numericUpDownUpTo.Size = new System.Drawing.Size(67, 20);
            this.numericUpDownUpTo.TabIndex = 78;
            this.numericUpDownUpTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownUpTo.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // labelImport
            // 
            this.labelImport.AutoSize = true;
            this.labelImport.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelImport.Location = new System.Drawing.Point(118, 316);
            this.labelImport.Name = "labelImport";
            this.labelImport.Size = new System.Drawing.Size(63, 33);
            this.labelImport.TabIndex = 80;
            this.labelImport.Text = "Import data:";
            this.labelImport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radioButtonImportAll
            // 
            this.radioButtonImportAll.AutoSize = true;
            this.radioButtonImportAll.Checked = true;
            this.radioButtonImportAll.Dock = System.Windows.Forms.DockStyle.Right;
            this.radioButtonImportAll.Location = new System.Drawing.Point(187, 319);
            this.radioButtonImportAll.Name = "radioButtonImportAll";
            this.radioButtonImportAll.Size = new System.Drawing.Size(115, 27);
            this.radioButtonImportAll.TabIndex = 81;
            this.radioButtonImportAll.TabStop = true;
            this.radioButtonImportAll.Text = "Import all data lines";
            this.radioButtonImportAll.UseVisualStyleBackColor = true;
            // 
            // radioButtonImportFirstLines
            // 
            this.radioButtonImportFirstLines.AutoSize = true;
            this.radioButtonImportFirstLines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonImportFirstLines.Location = new System.Drawing.Point(308, 319);
            this.radioButtonImportFirstLines.Name = "radioButtonImportFirstLines";
            this.radioButtonImportFirstLines.Size = new System.Drawing.Size(73, 27);
            this.radioButtonImportFirstLines.TabIndex = 82;
            this.radioButtonImportFirstLines.Text = "Import first";
            this.radioButtonImportFirstLines.UseVisualStyleBackColor = true;
            // 
            // numericUpDownImport
            // 
            this.numericUpDownImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDownImport.Location = new System.Drawing.Point(387, 322);
            this.numericUpDownImport.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.numericUpDownImport.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownImport.Name = "numericUpDownImport";
            this.numericUpDownImport.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownImport.TabIndex = 83;
            this.numericUpDownImport.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelImportUpTo
            // 
            this.labelImportUpTo.AutoSize = true;
            this.labelImportUpTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelImportUpTo.Location = new System.Drawing.Point(433, 316);
            this.labelImportUpTo.Name = "labelImportUpTo";
            this.labelImportUpTo.Size = new System.Drawing.Size(28, 33);
            this.labelImportUpTo.TabIndex = 84;
            this.labelImportUpTo.Text = "lines";
            this.labelImportUpTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBarImport
            // 
            this.tableLayoutPanelImport.SetColumnSpan(this.progressBarImport, 2);
            this.progressBarImport.Dock = System.Windows.Forms.DockStyle.Right;
            this.progressBarImport.Location = new System.Drawing.Point(727, 319);
            this.progressBarImport.Name = "progressBarImport";
            this.progressBarImport.Size = new System.Drawing.Size(262, 27);
            this.progressBarImport.TabIndex = 86;
            this.progressBarImport.Visible = false;
            // 
            // checkBoxImportEmptyData
            // 
            this.checkBoxImportEmptyData.AutoSize = true;
            this.checkBoxImportEmptyData.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxImportEmptyData.Location = new System.Drawing.Point(467, 319);
            this.checkBoxImportEmptyData.Name = "checkBoxImportEmptyData";
            this.checkBoxImportEmptyData.Size = new System.Drawing.Size(110, 27);
            this.checkBoxImportEmptyData.TabIndex = 87;
            this.checkBoxImportEmptyData.Text = "Import empty data";
            this.checkBoxImportEmptyData.UseVisualStyleBackColor = true;
            this.checkBoxImportEmptyData.CheckedChanged += new System.EventHandler(this.checkBoxImportEmptyData_CheckedChanged);
            // 
            // labelTotalLines
            // 
            this.labelTotalLines.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTotalLines.Location = new System.Drawing.Point(3, 94);
            this.labelTotalLines.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelTotalLines.Name = "labelTotalLines";
            this.labelTotalLines.Size = new System.Drawing.Size(42, 23);
            this.labelTotalLines.TabIndex = 89;
            this.labelTotalLines.Text = "Total:";
            this.labelTotalLines.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxTotalLines
            // 
            this.textBoxTotalLines.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxTotalLines.Location = new System.Drawing.Point(45, 97);
            this.textBoxTotalLines.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxTotalLines.Name = "textBoxTotalLines";
            this.textBoxTotalLines.ReadOnly = true;
            this.textBoxTotalLines.Size = new System.Drawing.Size(67, 20);
            this.textBoxTotalLines.TabIndex = 90;
            this.textBoxTotalLines.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBoxPresettings
            // 
            this.groupBoxPresettings.Controls.Add(this.buttonLoadSettings);
            this.groupBoxPresettings.Controls.Add(this.buttonSave);
            this.groupBoxPresettings.Controls.Add(this.tabControlPresettings);
            this.groupBoxPresettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxPresettings.Enabled = false;
            this.groupBoxPresettings.Location = new System.Drawing.Point(0, 0);
            this.groupBoxPresettings.Name = "groupBoxPresettings";
            this.groupBoxPresettings.Size = new System.Drawing.Size(992, 314);
            this.groupBoxPresettings.TabIndex = 76;
            this.groupBoxPresettings.TabStop = false;
            this.groupBoxPresettings.Text = "Presetting parameters for the import";
            // 
            // buttonLoadSettings
            // 
            this.buttonLoadSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLoadSettings.Image = global::DiversityCollection.Resource.Open;
            this.buttonLoadSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonLoadSettings.Location = new System.Drawing.Point(803, 9);
            this.buttonLoadSettings.Name = "buttonLoadSettings";
            this.buttonLoadSettings.Size = new System.Drawing.Size(93, 22);
            this.buttonLoadSettings.TabIndex = 100;
            this.buttonLoadSettings.Text = "Load settings";
            this.buttonLoadSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonLoadSettings, "Load column mapping and preset parameters from a file");
            this.buttonLoadSettings.UseVisualStyleBackColor = true;
            this.buttonLoadSettings.Click += new System.EventHandler(this.buttonLoadSettings_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Image = global::DiversityCollection.Resource.Save;
            this.buttonSave.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSave.Location = new System.Drawing.Point(896, 9);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(93, 22);
            this.buttonSave.TabIndex = 99;
            this.buttonSave.Text = "Save settings";
            this.buttonSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonSave, "Save preset parameters and column mapping");
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // tabControlPresettings
            // 
            this.tabControlPresettings.Controls.Add(this.tabPageColumnMapping);
            this.tabControlPresettings.Controls.Add(this.tabPageEvent);
            this.tabControlPresettings.Controls.Add(this.tabPageSpecimen);
            this.tabControlPresettings.Controls.Add(this.tabPageUnits);
            this.tabControlPresettings.Controls.Add(this.tabPageStorage);
            this.tabControlPresettings.Controls.Add(this.tabPageAdding);
            this.tabControlPresettings.Controls.Add(this.tabPageHidden);
            this.tabControlPresettings.Controls.Add(this.tabPageFile);
            this.tabControlPresettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPresettings.ImageList = this.imageList;
            this.tabControlPresettings.Location = new System.Drawing.Point(3, 16);
            this.tabControlPresettings.Name = "tabControlPresettings";
            this.tabControlPresettings.SelectedIndex = 0;
            this.tabControlPresettings.Size = new System.Drawing.Size(986, 295);
            this.tabControlPresettings.TabIndex = 98;
            // 
            // tabPageColumnMapping
            // 
            this.tabPageColumnMapping.Controls.Add(this.tableLayoutPanelColumnMapping);
            this.helpProvider.SetHelpKeyword(this.tabPageColumnMapping, "Import lists");
            this.helpProvider.SetHelpNavigator(this.tabPageColumnMapping, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.tabPageColumnMapping.ImageIndex = 3;
            this.tabPageColumnMapping.Location = new System.Drawing.Point(4, 23);
            this.tabPageColumnMapping.Name = "tabPageColumnMapping";
            this.tabPageColumnMapping.Padding = new System.Windows.Forms.Padding(3);
            this.helpProvider.SetShowHelp(this.tabPageColumnMapping, true);
            this.tabPageColumnMapping.Size = new System.Drawing.Size(978, 268);
            this.tabPageColumnMapping.TabIndex = 4;
            this.tabPageColumnMapping.Text = "Column mapping";
            this.tabPageColumnMapping.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelColumnMapping
            // 
            this.tableLayoutPanelColumnMapping.ColumnCount = 14;
            this.tableLayoutPanelColumnMapping.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelColumnMapping.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelColumnMapping.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelColumnMapping.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelColumnMapping.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelColumnMapping.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelColumnMapping.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelColumnMapping.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelColumnMapping.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelColumnMapping.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelColumnMapping.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelColumnMapping.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelColumnMapping.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tableLayoutPanelColumnMapping.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.tableLayoutPanelColumnMapping.Controls.Add(this.dataGridViewColumnMappingSource, 0, 0);
            this.tableLayoutPanelColumnMapping.Controls.Add(this.labelColumnMappingTable, 3, 1);
            this.tableLayoutPanelColumnMapping.Controls.Add(this.comboBoxColumnMappingTable, 4, 1);
            this.tableLayoutPanelColumnMapping.Controls.Add(this.comboBoxColumnMappingColumn, 8, 1);
            this.tableLayoutPanelColumnMapping.Controls.Add(this.labelColumnMappingColumn, 7, 1);
            this.tableLayoutPanelColumnMapping.Controls.Add(this.labelColumnMappingAlias, 5, 1);
            this.tableLayoutPanelColumnMapping.Controls.Add(this.buttonColumnMappingSave, 2, 1);
            this.tableLayoutPanelColumnMapping.Controls.Add(this.dataGridViewColumnMapping, 0, 2);
            this.tableLayoutPanelColumnMapping.Controls.Add(this.labelDataStartInLine, 0, 1);
            this.tableLayoutPanelColumnMapping.Controls.Add(this.numericUpDownDataStartInLine, 1, 1);
            this.tableLayoutPanelColumnMapping.Controls.Add(this.labelColumnGroup, 9, 1);
            this.tableLayoutPanelColumnMapping.Controls.Add(this.comboBoxColumnGroup, 10, 1);
            this.tableLayoutPanelColumnMapping.Controls.Add(this.comboBoxColumnMappingAlias, 6, 1);
            this.tableLayoutPanelColumnMapping.Controls.Add(this.buttonColumnMappingSaveToFile, 13, 1);
            this.tableLayoutPanelColumnMapping.Controls.Add(this.buttonColumnMappingImportFromFile, 12, 1);
            this.tableLayoutPanelColumnMapping.Controls.Add(this.buttonIgnoreColumn, 11, 1);
            this.tableLayoutPanelColumnMapping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelColumnMapping.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelColumnMapping.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.tableLayoutPanelColumnMapping.Name = "tableLayoutPanelColumnMapping";
            this.tableLayoutPanelColumnMapping.RowCount = 3;
            this.tableLayoutPanelColumnMapping.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelColumnMapping.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelColumnMapping.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.tableLayoutPanelColumnMapping.Size = new System.Drawing.Size(972, 262);
            this.tableLayoutPanelColumnMapping.TabIndex = 0;
            // 
            // dataGridViewColumnMappingSource
            // 
            this.dataGridViewColumnMappingSource.AllowUserToAddRows = false;
            this.dataGridViewColumnMappingSource.AllowUserToDeleteRows = false;
            this.dataGridViewColumnMappingSource.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewColumnMappingSource.ColumnHeadersVisible = false;
            this.tableLayoutPanelColumnMapping.SetColumnSpan(this.dataGridViewColumnMappingSource, 14);
            this.dataGridViewColumnMappingSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewColumnMappingSource.Location = new System.Drawing.Point(3, 0);
            this.dataGridViewColumnMappingSource.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.dataGridViewColumnMappingSource.MultiSelect = false;
            this.dataGridViewColumnMappingSource.Name = "dataGridViewColumnMappingSource";
            this.dataGridViewColumnMappingSource.ReadOnly = true;
            this.dataGridViewColumnMappingSource.RowHeadersVisible = false;
            this.dataGridViewColumnMappingSource.Size = new System.Drawing.Size(966, 147);
            this.dataGridViewColumnMappingSource.TabIndex = 1;
            this.dataGridViewColumnMappingSource.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewColumnMappingSource_CellClick);
            this.dataGridViewColumnMappingSource.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewColumnMappingSource_CellContentClick);
            // 
            // labelColumnMappingTable
            // 
            this.labelColumnMappingTable.AutoSize = true;
            this.labelColumnMappingTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelColumnMappingTable.Location = new System.Drawing.Point(179, 147);
            this.labelColumnMappingTable.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelColumnMappingTable.Name = "labelColumnMappingTable";
            this.labelColumnMappingTable.Size = new System.Drawing.Size(37, 29);
            this.labelColumnMappingTable.TabIndex = 2;
            this.labelColumnMappingTable.Text = "Table:";
            this.labelColumnMappingTable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxColumnMappingTable
            // 
            this.comboBoxColumnMappingTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxColumnMappingTable.FormattingEnabled = true;
            this.comboBoxColumnMappingTable.Location = new System.Drawing.Point(216, 150);
            this.comboBoxColumnMappingTable.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxColumnMappingTable.MaxDropDownItems = 20;
            this.comboBoxColumnMappingTable.Name = "comboBoxColumnMappingTable";
            this.comboBoxColumnMappingTable.Size = new System.Drawing.Size(156, 21);
            this.comboBoxColumnMappingTable.TabIndex = 3;
            this.toolTip.SetToolTip(this.comboBoxColumnMappingTable, "The table into which the data of the selected field should be imported");
            this.comboBoxColumnMappingTable.DropDown += new System.EventHandler(this.comboBoxColumnMappingTable_DropDown);
            this.comboBoxColumnMappingTable.SelectionChangeCommitted += new System.EventHandler(this.comboBoxColumnMappingTable_SelectionChangeCommitted);
            // 
            // comboBoxColumnMappingColumn
            // 
            this.comboBoxColumnMappingColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxColumnMappingColumn.FormattingEnabled = true;
            this.comboBoxColumnMappingColumn.Location = new System.Drawing.Point(658, 150);
            this.comboBoxColumnMappingColumn.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxColumnMappingColumn.MaxDropDownItems = 20;
            this.comboBoxColumnMappingColumn.Name = "comboBoxColumnMappingColumn";
            this.comboBoxColumnMappingColumn.Size = new System.Drawing.Size(156, 21);
            this.comboBoxColumnMappingColumn.TabIndex = 5;
            this.toolTip.SetToolTip(this.comboBoxColumnMappingColumn, "The column of the table into which the data should be imported");
            this.comboBoxColumnMappingColumn.SelectionChangeCommitted += new System.EventHandler(this.comboBoxColumnMappingColumn_SelectionChangeCommitted);
            // 
            // labelColumnMappingColumn
            // 
            this.labelColumnMappingColumn.AutoSize = true;
            this.labelColumnMappingColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelColumnMappingColumn.Location = new System.Drawing.Point(613, 147);
            this.labelColumnMappingColumn.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelColumnMappingColumn.Name = "labelColumnMappingColumn";
            this.labelColumnMappingColumn.Size = new System.Drawing.Size(45, 29);
            this.labelColumnMappingColumn.TabIndex = 4;
            this.labelColumnMappingColumn.Text = "Column:";
            this.labelColumnMappingColumn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelColumnMappingAlias
            // 
            this.labelColumnMappingAlias.AutoSize = true;
            this.labelColumnMappingAlias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelColumnMappingAlias.Location = new System.Drawing.Point(378, 147);
            this.labelColumnMappingAlias.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelColumnMappingAlias.Name = "labelColumnMappingAlias";
            this.labelColumnMappingAlias.Size = new System.Drawing.Size(73, 29);
            this.labelColumnMappingAlias.TabIndex = 7;
            this.labelColumnMappingAlias.Text = "Alias for table:";
            this.labelColumnMappingAlias.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonColumnMappingSave
            // 
            this.buttonColumnMappingSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonColumnMappingSave.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonColumnMappingSave.Location = new System.Drawing.Point(128, 147);
            this.buttonColumnMappingSave.Margin = new System.Windows.Forms.Padding(3, 0, 3, 2);
            this.buttonColumnMappingSave.Name = "buttonColumnMappingSave";
            this.buttonColumnMappingSave.Size = new System.Drawing.Size(45, 27);
            this.buttonColumnMappingSave.TabIndex = 8;
            this.buttonColumnMappingSave.Text = "Save";
            this.toolTip.SetToolTip(this.buttonColumnMappingSave, "Save the mapping for the current column");
            this.buttonColumnMappingSave.UseVisualStyleBackColor = true;
            this.buttonColumnMappingSave.Click += new System.EventHandler(this.buttonColumnMappingSave_Click);
            // 
            // dataGridViewColumnMapping
            // 
            this.dataGridViewColumnMapping.AllowUserToAddRows = false;
            this.dataGridViewColumnMapping.AllowUserToDeleteRows = false;
            this.dataGridViewColumnMapping.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewColumnMapping.ColumnHeadersVisible = false;
            this.tableLayoutPanelColumnMapping.SetColumnSpan(this.dataGridViewColumnMapping, 14);
            this.dataGridViewColumnMapping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewColumnMapping.Location = new System.Drawing.Point(3, 176);
            this.dataGridViewColumnMapping.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.dataGridViewColumnMapping.MultiSelect = false;
            this.dataGridViewColumnMapping.Name = "dataGridViewColumnMapping";
            this.dataGridViewColumnMapping.ReadOnly = true;
            this.dataGridViewColumnMapping.RowHeadersVisible = false;
            this.dataGridViewColumnMapping.Size = new System.Drawing.Size(966, 86);
            this.dataGridViewColumnMapping.TabIndex = 9;
            this.dataGridViewColumnMapping.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewColumnMapping_CellClick);
            // 
            // labelDataStartInLine
            // 
            this.labelDataStartInLine.AutoSize = true;
            this.labelDataStartInLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDataStartInLine.Location = new System.Drawing.Point(3, 147);
            this.labelDataStartInLine.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelDataStartInLine.Name = "labelDataStartInLine";
            this.labelDataStartInLine.Size = new System.Drawing.Size(86, 29);
            this.labelDataStartInLine.TabIndex = 10;
            this.labelDataStartInLine.Text = "Data start in line:";
            this.labelDataStartInLine.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownDataStartInLine
            // 
            this.numericUpDownDataStartInLine.Location = new System.Drawing.Point(89, 150);
            this.numericUpDownDataStartInLine.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.numericUpDownDataStartInLine.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownDataStartInLine.Name = "numericUpDownDataStartInLine";
            this.numericUpDownDataStartInLine.Size = new System.Drawing.Size(33, 20);
            this.numericUpDownDataStartInLine.TabIndex = 11;
            this.numericUpDownDataStartInLine.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.numericUpDownDataStartInLine, "The first line in the file with data that should be imported ");
            this.numericUpDownDataStartInLine.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownDataStartInLine.ValueChanged += new System.EventHandler(this.numericUpDownDataStartInLine_ValueChanged);
            // 
            // labelColumnGroup
            // 
            this.labelColumnGroup.AutoSize = true;
            this.labelColumnGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelColumnGroup.Location = new System.Drawing.Point(820, 147);
            this.labelColumnGroup.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelColumnGroup.Name = "labelColumnGroup";
            this.labelColumnGroup.Size = new System.Drawing.Size(39, 29);
            this.labelColumnGroup.TabIndex = 12;
            this.labelColumnGroup.Text = "Group:";
            this.labelColumnGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxColumnGroup
            // 
            this.comboBoxColumnGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxColumnGroup.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxColumnGroup.ForeColor = System.Drawing.SystemColors.Window;
            this.comboBoxColumnGroup.FormattingEnabled = true;
            this.comboBoxColumnGroup.Location = new System.Drawing.Point(859, 150);
            this.comboBoxColumnGroup.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxColumnGroup.Name = "comboBoxColumnGroup";
            this.comboBoxColumnGroup.Size = new System.Drawing.Size(50, 21);
            this.comboBoxColumnGroup.TabIndex = 13;
            this.comboBoxColumnGroup.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBoxColumnGroup_DrawItem);
            this.comboBoxColumnGroup.DropDown += new System.EventHandler(this.comboBoxColumnGroup_DropDown);
            this.comboBoxColumnGroup.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.comboBoxColumnGroup_MeasureItem);
            this.comboBoxColumnGroup.SelectionChangeCommitted += new System.EventHandler(this.comboBoxColumnGroup_SelectionChangeCommitted);
            // 
            // comboBoxColumnMappingAlias
            // 
            this.comboBoxColumnMappingAlias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxColumnMappingAlias.FormattingEnabled = true;
            this.comboBoxColumnMappingAlias.Location = new System.Drawing.Point(451, 150);
            this.comboBoxColumnMappingAlias.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxColumnMappingAlias.Name = "comboBoxColumnMappingAlias";
            this.comboBoxColumnMappingAlias.Size = new System.Drawing.Size(156, 21);
            this.comboBoxColumnMappingAlias.TabIndex = 14;
            this.comboBoxColumnMappingAlias.DropDown += new System.EventHandler(this.comboBoxColumnMappingAlias_DropDown);
            // 
            // buttonColumnMappingSaveToFile
            // 
            this.buttonColumnMappingSaveToFile.Image = global::DiversityCollection.Resource.Save;
            this.buttonColumnMappingSaveToFile.Location = new System.Drawing.Point(963, 150);
            this.buttonColumnMappingSaveToFile.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.buttonColumnMappingSaveToFile.Name = "buttonColumnMappingSaveToFile";
            this.buttonColumnMappingSaveToFile.Size = new System.Drawing.Size(1, 23);
            this.buttonColumnMappingSaveToFile.TabIndex = 15;
            this.toolTip.SetToolTip(this.buttonColumnMappingSaveToFile, "Save column mapping to file");
            this.buttonColumnMappingSaveToFile.UseVisualStyleBackColor = true;
            this.buttonColumnMappingSaveToFile.Visible = false;
            this.buttonColumnMappingSaveToFile.Click += new System.EventHandler(this.buttonColumnMappingSaveToFile_Click);
            // 
            // buttonColumnMappingImportFromFile
            // 
            this.buttonColumnMappingImportFromFile.Image = global::DiversityCollection.Resource.Open;
            this.buttonColumnMappingImportFromFile.Location = new System.Drawing.Point(962, 150);
            this.buttonColumnMappingImportFromFile.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.buttonColumnMappingImportFromFile.Name = "buttonColumnMappingImportFromFile";
            this.buttonColumnMappingImportFromFile.Size = new System.Drawing.Size(1, 23);
            this.buttonColumnMappingImportFromFile.TabIndex = 16;
            this.toolTip.SetToolTip(this.buttonColumnMappingImportFromFile, "Import a column mapping from a file");
            this.buttonColumnMappingImportFromFile.UseVisualStyleBackColor = true;
            this.buttonColumnMappingImportFromFile.Visible = false;
            this.buttonColumnMappingImportFromFile.Click += new System.EventHandler(this.buttonColumnMappingImportFromFile_Click);
            // 
            // buttonIgnoreColumn
            // 
            this.buttonIgnoreColumn.BackColor = System.Drawing.Color.Black;
            this.buttonIgnoreColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonIgnoreColumn.ForeColor = System.Drawing.SystemColors.Window;
            this.buttonIgnoreColumn.Location = new System.Drawing.Point(912, 147);
            this.buttonIgnoreColumn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonIgnoreColumn.Name = "buttonIgnoreColumn";
            this.buttonIgnoreColumn.Size = new System.Drawing.Size(50, 29);
            this.buttonIgnoreColumn.TabIndex = 17;
            this.buttonIgnoreColumn.Text = "Ignore";
            this.toolTip.SetToolTip(this.buttonIgnoreColumn, "Ignore the selected column");
            this.buttonIgnoreColumn.UseVisualStyleBackColor = false;
            this.buttonIgnoreColumn.Click += new System.EventHandler(this.buttonIgnoreColumn_Click);
            // 
            // tabPageEvent
            // 
            this.tabPageEvent.Controls.Add(this.tableLayoutPanelEventHierarchy);
            this.tabPageEvent.Controls.Add(this.tableLayoutPanelEventDate);
            this.tabPageEvent.Controls.Add(this.panel1);
            this.tabPageEvent.Controls.Add(this.tableLayoutPanelCollector);
            this.tabPageEvent.Controls.Add(this.tableLayoutPanelPlaceAndCountry);
            this.helpProvider.SetHelpKeyword(this.tabPageEvent, "Import lists");
            this.helpProvider.SetHelpNavigator(this.tabPageEvent, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.tabPageEvent.ImageIndex = 1;
            this.tabPageEvent.Location = new System.Drawing.Point(4, 23);
            this.tabPageEvent.Name = "tabPageEvent";
            this.tabPageEvent.Padding = new System.Windows.Forms.Padding(3);
            this.helpProvider.SetShowHelp(this.tabPageEvent, true);
            this.tabPageEvent.Size = new System.Drawing.Size(978, 268);
            this.tabPageEvent.TabIndex = 0;
            this.tabPageEvent.Text = "Collection event";
            this.tabPageEvent.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelEventHierarchy
            // 
            this.tableLayoutPanelEventHierarchy.ColumnCount = 5;
            this.tableLayoutPanelEventHierarchy.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelEventHierarchy.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelEventHierarchy.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelEventHierarchy.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelEventHierarchy.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelEventHierarchy.Controls.Add(this.radioButtonEventsSeparate, 0, 0);
            this.tableLayoutPanelEventHierarchy.Controls.Add(this.treeViewEventsSeparate, 0, 1);
            this.tableLayoutPanelEventHierarchy.Controls.Add(this.radioButtonEventsInGroups, 1, 0);
            this.tableLayoutPanelEventHierarchy.Controls.Add(this.treeViewEventsInGroups, 1, 1);
            this.tableLayoutPanelEventHierarchy.Controls.Add(this.checkBoxEventSeries, 2, 0);
            this.tableLayoutPanelEventHierarchy.Controls.Add(this.labelEventSeriesCode, 2, 1);
            this.tableLayoutPanelEventHierarchy.Controls.Add(this.labelEventSeriesDescription, 3, 1);
            this.tableLayoutPanelEventHierarchy.Controls.Add(this.labelEventSeriesNotes, 4, 1);
            this.tableLayoutPanelEventHierarchy.Controls.Add(this.textBoxEventSeriesCode, 2, 2);
            this.tableLayoutPanelEventHierarchy.Controls.Add(this.textBoxEventSeriesDescription, 3, 2);
            this.tableLayoutPanelEventHierarchy.Controls.Add(this.textBoxEventSeriesNotes, 4, 2);
            this.tableLayoutPanelEventHierarchy.Controls.Add(this.buttonGetEventSeries, 4, 0);
            this.tableLayoutPanelEventHierarchy.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelEventHierarchy.Location = new System.Drawing.Point(3, 102);
            this.tableLayoutPanelEventHierarchy.Name = "tableLayoutPanelEventHierarchy";
            this.tableLayoutPanelEventHierarchy.RowCount = 3;
            this.tableLayoutPanelEventHierarchy.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEventHierarchy.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEventHierarchy.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEventHierarchy.Size = new System.Drawing.Size(972, 95);
            this.tableLayoutPanelEventHierarchy.TabIndex = 4;
            // 
            // radioButtonEventsSeparate
            // 
            this.radioButtonEventsSeparate.AutoSize = true;
            this.radioButtonEventsSeparate.Checked = true;
            this.radioButtonEventsSeparate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonEventsSeparate.Location = new System.Drawing.Point(3, 3);
            this.radioButtonEventsSeparate.Name = "radioButtonEventsSeparate";
            this.radioButtonEventsSeparate.Size = new System.Drawing.Size(188, 18);
            this.radioButtonEventsSeparate.TabIndex = 0;
            this.radioButtonEventsSeparate.TabStop = true;
            this.radioButtonEventsSeparate.Text = "Separate collection events";
            this.toolTip.SetToolTip(this.radioButtonEventsSeparate, "Will create a collection event for each specimen");
            this.radioButtonEventsSeparate.UseVisualStyleBackColor = true;
            this.radioButtonEventsSeparate.CheckedChanged += new System.EventHandler(this.radioButtonEventsSeparate_CheckedChanged);
            // 
            // treeViewEventsSeparate
            // 
            this.treeViewEventsSeparate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewEventsSeparate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewEventsSeparate.ImageIndex = 0;
            this.treeViewEventsSeparate.ImageList = this.imageList;
            this.treeViewEventsSeparate.Location = new System.Drawing.Point(3, 27);
            this.treeViewEventsSeparate.Name = "treeViewEventsSeparate";
            treeNode1.Name = "Knoten1";
            treeNode1.Text = "Collection specimen";
            treeNode2.ImageKey = "Event.ico";
            treeNode2.Name = "Knoten0";
            treeNode2.SelectedImageKey = "Event.ico";
            treeNode2.Text = "Collection event";
            treeNode3.Name = "Knoten3";
            treeNode3.Text = "Collection specimen";
            treeNode4.ImageKey = "Event.ico";
            treeNode4.Name = "Knoten2";
            treeNode4.SelectedImageKey = "Event.ico";
            treeNode4.Text = "Collection event";
            this.treeViewEventsSeparate.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode4});
            this.tableLayoutPanelEventHierarchy.SetRowSpan(this.treeViewEventsSeparate, 2);
            this.treeViewEventsSeparate.SelectedImageIndex = 0;
            this.treeViewEventsSeparate.Size = new System.Drawing.Size(188, 65);
            this.treeViewEventsSeparate.TabIndex = 2;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "CollectionSpecimen.ico");
            this.imageList.Images.SetKeyName(1, "Event.ico");
            this.imageList.Images.SetKeyName(2, "Plant.ico");
            this.imageList.Images.SetKeyName(3, "ColumnMapping.ico");
            this.imageList.Images.SetKeyName(4, "Collection.ico");
            this.imageList.Images.SetKeyName(5, "Add.ico");
            // 
            // radioButtonEventsInGroups
            // 
            this.radioButtonEventsInGroups.AutoSize = true;
            this.radioButtonEventsInGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonEventsInGroups.Location = new System.Drawing.Point(197, 3);
            this.radioButtonEventsInGroups.Name = "radioButtonEventsInGroups";
            this.radioButtonEventsInGroups.Size = new System.Drawing.Size(188, 18);
            this.radioButtonEventsInGroups.TabIndex = 1;
            this.radioButtonEventsInGroups.Text = "Group collection events";
            this.toolTip.SetToolTip(this.radioButtonEventsInGroups, "Will search the data related to the collection event and place all specimen with " +
        "equal collecting data in one collection event");
            this.radioButtonEventsInGroups.UseVisualStyleBackColor = true;
            // 
            // treeViewEventsInGroups
            // 
            this.treeViewEventsInGroups.BackColor = System.Drawing.SystemColors.Control;
            this.treeViewEventsInGroups.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewEventsInGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewEventsInGroups.ImageIndex = 0;
            this.treeViewEventsInGroups.ImageList = this.imageListGrey;
            this.treeViewEventsInGroups.Location = new System.Drawing.Point(197, 27);
            this.treeViewEventsInGroups.Name = "treeViewEventsInGroups";
            treeNode5.Name = "Knoten1";
            treeNode5.Text = "Collection specimen";
            treeNode6.Name = "Knoten2";
            treeNode6.Text = "Collection specimen";
            treeNode7.ImageKey = "EventGrey.ico";
            treeNode7.Name = "Knoten0";
            treeNode7.SelectedImageKey = "EventGrey.ico";
            treeNode7.Text = "Collection event";
            this.treeViewEventsInGroups.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode7});
            this.tableLayoutPanelEventHierarchy.SetRowSpan(this.treeViewEventsInGroups, 2);
            this.treeViewEventsInGroups.SelectedImageIndex = 0;
            this.treeViewEventsInGroups.Size = new System.Drawing.Size(188, 65);
            this.treeViewEventsInGroups.TabIndex = 3;
            // 
            // imageListGrey
            // 
            this.imageListGrey.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListGrey.ImageStream")));
            this.imageListGrey.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListGrey.Images.SetKeyName(0, "CollectionSpecimenGrey.ico");
            this.imageListGrey.Images.SetKeyName(1, "EventGrey.ico");
            // 
            // checkBoxEventSeries
            // 
            this.checkBoxEventSeries.AutoSize = true;
            this.tableLayoutPanelEventHierarchy.SetColumnSpan(this.checkBoxEventSeries, 2);
            this.checkBoxEventSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxEventSeries.Location = new System.Drawing.Point(391, 3);
            this.checkBoxEventSeries.Name = "checkBoxEventSeries";
            this.checkBoxEventSeries.Size = new System.Drawing.Size(382, 18);
            this.checkBoxEventSeries.TabIndex = 4;
            this.checkBoxEventSeries.Text = "All collection events belong to an event series, e.g. an expedition";
            this.checkBoxEventSeries.UseVisualStyleBackColor = true;
            this.checkBoxEventSeries.CheckedChanged += new System.EventHandler(this.checkBoxEventSeries_CheckedChanged);
            // 
            // labelEventSeriesCode
            // 
            this.labelEventSeriesCode.AutoSize = true;
            this.labelEventSeriesCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEventSeriesCode.Location = new System.Drawing.Point(391, 24);
            this.labelEventSeriesCode.Name = "labelEventSeriesCode";
            this.labelEventSeriesCode.Size = new System.Drawing.Size(91, 13);
            this.labelEventSeriesCode.TabIndex = 5;
            this.labelEventSeriesCode.Text = "Code";
            // 
            // labelEventSeriesDescription
            // 
            this.labelEventSeriesDescription.AutoSize = true;
            this.labelEventSeriesDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEventSeriesDescription.Location = new System.Drawing.Point(488, 24);
            this.labelEventSeriesDescription.Name = "labelEventSeriesDescription";
            this.labelEventSeriesDescription.Size = new System.Drawing.Size(285, 13);
            this.labelEventSeriesDescription.TabIndex = 6;
            this.labelEventSeriesDescription.Text = "Description";
            // 
            // labelEventSeriesNotes
            // 
            this.labelEventSeriesNotes.AutoSize = true;
            this.labelEventSeriesNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEventSeriesNotes.Location = new System.Drawing.Point(779, 24);
            this.labelEventSeriesNotes.Name = "labelEventSeriesNotes";
            this.labelEventSeriesNotes.Size = new System.Drawing.Size(190, 13);
            this.labelEventSeriesNotes.TabIndex = 7;
            this.labelEventSeriesNotes.Text = "Notes";
            // 
            // textBoxEventSeriesCode
            // 
            this.textBoxEventSeriesCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEventSeriesCode.Enabled = false;
            this.textBoxEventSeriesCode.Location = new System.Drawing.Point(391, 40);
            this.textBoxEventSeriesCode.Multiline = true;
            this.textBoxEventSeriesCode.Name = "textBoxEventSeriesCode";
            this.textBoxEventSeriesCode.Size = new System.Drawing.Size(91, 52);
            this.textBoxEventSeriesCode.TabIndex = 8;
            // 
            // textBoxEventSeriesDescription
            // 
            this.textBoxEventSeriesDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEventSeriesDescription.Enabled = false;
            this.textBoxEventSeriesDescription.Location = new System.Drawing.Point(488, 40);
            this.textBoxEventSeriesDescription.Multiline = true;
            this.textBoxEventSeriesDescription.Name = "textBoxEventSeriesDescription";
            this.textBoxEventSeriesDescription.Size = new System.Drawing.Size(285, 52);
            this.textBoxEventSeriesDescription.TabIndex = 9;
            // 
            // textBoxEventSeriesNotes
            // 
            this.textBoxEventSeriesNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEventSeriesNotes.Enabled = false;
            this.textBoxEventSeriesNotes.Location = new System.Drawing.Point(779, 40);
            this.textBoxEventSeriesNotes.Multiline = true;
            this.textBoxEventSeriesNotes.Name = "textBoxEventSeriesNotes";
            this.textBoxEventSeriesNotes.Size = new System.Drawing.Size(190, 52);
            this.textBoxEventSeriesNotes.TabIndex = 10;
            // 
            // buttonGetEventSeries
            // 
            this.buttonGetEventSeries.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonGetEventSeries.Image = global::DiversityCollection.Resource.EventSeriesAssign;
            this.buttonGetEventSeries.Location = new System.Drawing.Point(944, 0);
            this.buttonGetEventSeries.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonGetEventSeries.Name = "buttonGetEventSeries";
            this.buttonGetEventSeries.Size = new System.Drawing.Size(25, 24);
            this.buttonGetEventSeries.TabIndex = 11;
            this.toolTip.SetToolTip(this.buttonGetEventSeries, "Assign to an existing event series");
            this.buttonGetEventSeries.UseVisualStyleBackColor = true;
            this.buttonGetEventSeries.Click += new System.EventHandler(this.buttonGetEventSeries_Click);
            // 
            // tableLayoutPanelEventDate
            // 
            this.tableLayoutPanelEventDate.ColumnCount = 6;
            this.tableLayoutPanelEventDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEventDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEventDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEventDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEventDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEventDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEventDate.Controls.Add(this.labelCollectionDate, 0, 0);
            this.tableLayoutPanelEventDate.Controls.Add(this.userControlDatePanelCollectionDate, 1, 0);
            this.tableLayoutPanelEventDate.Controls.Add(this.labelCollectionTime, 2, 0);
            this.tableLayoutPanelEventDate.Controls.Add(this.textBoxCollectionTime, 3, 0);
            this.tableLayoutPanelEventDate.Controls.Add(this.labelCollectionTimeSpan, 4, 0);
            this.tableLayoutPanelEventDate.Controls.Add(this.textBoxCollectionTimeSpan, 5, 0);
            this.tableLayoutPanelEventDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelEventDate.Location = new System.Drawing.Point(3, 78);
            this.tableLayoutPanelEventDate.Name = "tableLayoutPanelEventDate";
            this.tableLayoutPanelEventDate.RowCount = 1;
            this.tableLayoutPanelEventDate.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEventDate.Size = new System.Drawing.Size(972, 24);
            this.tableLayoutPanelEventDate.TabIndex = 3;
            // 
            // labelCollectionDate
            // 
            this.labelCollectionDate.AutoSize = true;
            this.labelCollectionDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionDate.Location = new System.Drawing.Point(3, 0);
            this.labelCollectionDate.Name = "labelCollectionDate";
            this.labelCollectionDate.Size = new System.Drawing.Size(80, 24);
            this.labelCollectionDate.TabIndex = 82;
            this.labelCollectionDate.Text = "Collection date:";
            this.labelCollectionDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlDatePanelCollectionDate
            // 
            this.userControlDatePanelCollectionDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDatePanelCollectionDate.Location = new System.Drawing.Point(89, 3);
            this.userControlDatePanelCollectionDate.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.userControlDatePanelCollectionDate.Name = "userControlDatePanelCollectionDate";
            this.userControlDatePanelCollectionDate.Size = new System.Drawing.Size(519, 20);
            this.userControlDatePanelCollectionDate.TabIndex = 81;
            // 
            // labelCollectionTime
            // 
            this.labelCollectionTime.AutoSize = true;
            this.labelCollectionTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionTime.Location = new System.Drawing.Point(614, 0);
            this.labelCollectionTime.Name = "labelCollectionTime";
            this.labelCollectionTime.Size = new System.Drawing.Size(78, 24);
            this.labelCollectionTime.TabIndex = 83;
            this.labelCollectionTime.Text = "Collection time:";
            this.labelCollectionTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCollectionTime
            // 
            this.textBoxCollectionTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCollectionTime.Location = new System.Drawing.Point(698, 3);
            this.textBoxCollectionTime.Name = "textBoxCollectionTime";
            this.textBoxCollectionTime.Size = new System.Drawing.Size(100, 20);
            this.textBoxCollectionTime.TabIndex = 84;
            // 
            // labelCollectionTimeSpan
            // 
            this.labelCollectionTimeSpan.AutoSize = true;
            this.labelCollectionTimeSpan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionTimeSpan.Location = new System.Drawing.Point(804, 0);
            this.labelCollectionTimeSpan.Name = "labelCollectionTimeSpan";
            this.labelCollectionTimeSpan.Size = new System.Drawing.Size(59, 24);
            this.labelCollectionTimeSpan.TabIndex = 85;
            this.labelCollectionTimeSpan.Text = "Time span:";
            this.labelCollectionTimeSpan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCollectionTimeSpan
            // 
            this.textBoxCollectionTimeSpan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCollectionTimeSpan.Location = new System.Drawing.Point(869, 3);
            this.textBoxCollectionTimeSpan.Name = "textBoxCollectionTimeSpan";
            this.textBoxCollectionTimeSpan.Size = new System.Drawing.Size(100, 20);
            this.textBoxCollectionTimeSpan.TabIndex = 86;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.comboBox2);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 56);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(972, 22);
            this.panel1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Location = new System.Drawing.Point(776, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 22);
            this.label2.TabIndex = 2;
            this.label2.Text = "Localisation 1:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox2
            // 
            this.comboBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(851, 0);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 3;
            // 
            // comboBox1
            // 
            this.comboBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(222, 0);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(225, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectionChangeCommitted += new System.EventHandler(this.comboBox1_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(222, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Localisation T1:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanelCollector
            // 
            this.tableLayoutPanelCollector.ColumnCount = 4;
            this.tableLayoutPanelCollector.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelCollector.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelCollector.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelCollector.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelCollector.Controls.Add(this.labelCollector, 0, 0);
            this.tableLayoutPanelCollector.Controls.Add(this.userControlModuleRelatedEntryCollector, 1, 0);
            this.tableLayoutPanelCollector.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelCollector.Location = new System.Drawing.Point(3, 30);
            this.tableLayoutPanelCollector.Name = "tableLayoutPanelCollector";
            this.tableLayoutPanelCollector.RowCount = 1;
            this.tableLayoutPanelCollector.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollector.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanelCollector.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanelCollector.Size = new System.Drawing.Size(972, 26);
            this.tableLayoutPanelCollector.TabIndex = 0;
            // 
            // labelCollector
            // 
            this.labelCollector.AutoSize = true;
            this.labelCollector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollector.Location = new System.Drawing.Point(3, 0);
            this.labelCollector.Name = "labelCollector";
            this.labelCollector.Size = new System.Drawing.Size(51, 26);
            this.labelCollector.TabIndex = 86;
            this.labelCollector.Text = "Collector:";
            this.labelCollector.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlModuleRelatedEntryCollector
            // 
            this.userControlModuleRelatedEntryCollector.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelCollector.SetColumnSpan(this.userControlModuleRelatedEntryCollector, 3);
            this.userControlModuleRelatedEntryCollector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryCollector.Domain = "";
            this.userControlModuleRelatedEntryCollector.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryCollector.Location = new System.Drawing.Point(60, 3);
            this.userControlModuleRelatedEntryCollector.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.userControlModuleRelatedEntryCollector.Module = null;
            this.userControlModuleRelatedEntryCollector.Name = "userControlModuleRelatedEntryCollector";
            this.userControlModuleRelatedEntryCollector.ShowInfo = false;
            this.userControlModuleRelatedEntryCollector.Size = new System.Drawing.Size(909, 23);
            this.userControlModuleRelatedEntryCollector.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryCollector.TabIndex = 85;
            // 
            // tableLayoutPanelPlaceAndCountry
            // 
            this.tableLayoutPanelPlaceAndCountry.ColumnCount = 4;
            this.tableLayoutPanelPlaceAndCountry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPlaceAndCountry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPlaceAndCountry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPlaceAndCountry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPlaceAndCountry.Controls.Add(this.labelGazetteer, 0, 0);
            this.tableLayoutPanelPlaceAndCountry.Controls.Add(this.userControlModuleRelatedEntryGazetteer, 1, 0);
            this.tableLayoutPanelPlaceAndCountry.Controls.Add(this.labelCountry, 2, 0);
            this.tableLayoutPanelPlaceAndCountry.Controls.Add(this.comboBoxCountry, 3, 0);
            this.tableLayoutPanelPlaceAndCountry.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelPlaceAndCountry.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelPlaceAndCountry.Name = "tableLayoutPanelPlaceAndCountry";
            this.tableLayoutPanelPlaceAndCountry.RowCount = 1;
            this.tableLayoutPanelPlaceAndCountry.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPlaceAndCountry.Size = new System.Drawing.Size(972, 27);
            this.tableLayoutPanelPlaceAndCountry.TabIndex = 2;
            // 
            // labelGazetteer
            // 
            this.labelGazetteer.AutoSize = true;
            this.labelGazetteer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGazetteer.Location = new System.Drawing.Point(3, 0);
            this.labelGazetteer.Name = "labelGazetteer";
            this.labelGazetteer.Size = new System.Drawing.Size(87, 27);
            this.labelGazetteer.TabIndex = 92;
            this.labelGazetteer.Text = "Locality or place:";
            this.labelGazetteer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlModuleRelatedEntryGazetteer
            // 
            this.userControlModuleRelatedEntryGazetteer.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryGazetteer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryGazetteer.Domain = "";
            this.userControlModuleRelatedEntryGazetteer.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryGazetteer.Location = new System.Drawing.Point(96, 3);
            this.userControlModuleRelatedEntryGazetteer.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.userControlModuleRelatedEntryGazetteer.Module = null;
            this.userControlModuleRelatedEntryGazetteer.Name = "userControlModuleRelatedEntryGazetteer";
            this.userControlModuleRelatedEntryGazetteer.ShowInfo = false;
            this.userControlModuleRelatedEntryGazetteer.Size = new System.Drawing.Size(618, 24);
            this.userControlModuleRelatedEntryGazetteer.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryGazetteer.TabIndex = 91;
            // 
            // labelCountry
            // 
            this.labelCountry.AutoSize = true;
            this.labelCountry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCountry.Location = new System.Drawing.Point(720, 0);
            this.labelCountry.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelCountry.Name = "labelCountry";
            this.labelCountry.Size = new System.Drawing.Size(46, 27);
            this.labelCountry.TabIndex = 93;
            this.labelCountry.Text = "Country:";
            this.labelCountry.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxCountry
            // 
            this.comboBoxCountry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxCountry.FormattingEnabled = true;
            this.comboBoxCountry.Location = new System.Drawing.Point(769, 3);
            this.comboBoxCountry.Name = "comboBoxCountry";
            this.comboBoxCountry.Size = new System.Drawing.Size(200, 21);
            this.comboBoxCountry.TabIndex = 94;
            // 
            // tabPageSpecimen
            // 
            this.tabPageSpecimen.Controls.Add(this.tableLayoutPanelPresetSpecimen);
            this.helpProvider.SetHelpKeyword(this.tabPageSpecimen, "Import lists");
            this.helpProvider.SetHelpNavigator(this.tabPageSpecimen, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.tabPageSpecimen.ImageIndex = 0;
            this.tabPageSpecimen.Location = new System.Drawing.Point(4, 23);
            this.tabPageSpecimen.Name = "tabPageSpecimen";
            this.tabPageSpecimen.Padding = new System.Windows.Forms.Padding(3);
            this.helpProvider.SetShowHelp(this.tabPageSpecimen, true);
            this.tabPageSpecimen.Size = new System.Drawing.Size(978, 268);
            this.tabPageSpecimen.TabIndex = 1;
            this.tabPageSpecimen.Text = "Specimen";
            this.tabPageSpecimen.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelPresetSpecimen
            // 
            this.tableLayoutPanelPresetSpecimen.ColumnCount = 8;
            this.tableLayoutPanelPresetSpecimen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPresetSpecimen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPresetSpecimen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelPresetSpecimen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelPresetSpecimen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPresetSpecimen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelPresetSpecimen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPresetSpecimen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPresetSpecimen.Controls.Add(this.comboBoxLabelType, 5, 2);
            this.tableLayoutPanelPresetSpecimen.Controls.Add(this.userControlModuleRelatedEntryExsiccate, 1, 1);
            this.tableLayoutPanelPresetSpecimen.Controls.Add(this.labelLabelType, 4, 2);
            this.tableLayoutPanelPresetSpecimen.Controls.Add(this.comboBoxImageType, 1, 3);
            this.tableLayoutPanelPresetSpecimen.Controls.Add(this.labelImageType, 0, 3);
            this.tableLayoutPanelPresetSpecimen.Controls.Add(this.userControlDatePanelAccessionDate, 1, 0);
            this.tableLayoutPanelPresetSpecimen.Controls.Add(this.labelExsiccate, 0, 1);
            this.tableLayoutPanelPresetSpecimen.Controls.Add(this.labelAccessionDate, 0, 0);
            this.tableLayoutPanelPresetSpecimen.Controls.Add(this.labelProject, 4, 0);
            this.tableLayoutPanelPresetSpecimen.Controls.Add(this.comboBoxProject, 5, 0);
            this.tableLayoutPanelPresetSpecimen.Controls.Add(this.labelDepositor, 0, 2);
            this.tableLayoutPanelPresetSpecimen.Controls.Add(this.userControlModuleRelatedEntryDepositor, 1, 2);
            this.tableLayoutPanelPresetSpecimen.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelPresetSpecimen.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelPresetSpecimen.Name = "tableLayoutPanelPresetSpecimen";
            this.tableLayoutPanelPresetSpecimen.RowCount = 8;
            this.tableLayoutPanelPresetSpecimen.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPresetSpecimen.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPresetSpecimen.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPresetSpecimen.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPresetSpecimen.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPresetSpecimen.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPresetSpecimen.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPresetSpecimen.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPresetSpecimen.Size = new System.Drawing.Size(972, 238);
            this.tableLayoutPanelPresetSpecimen.TabIndex = 0;
            // 
            // comboBoxLabelType
            // 
            this.comboBoxLabelType.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanelPresetSpecimen.SetColumnSpan(this.comboBoxLabelType, 3);
            this.comboBoxLabelType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxLabelType.FormattingEnabled = true;
            this.comboBoxLabelType.Location = new System.Drawing.Point(589, 57);
            this.comboBoxLabelType.Name = "comboBoxLabelType";
            this.comboBoxLabelType.Size = new System.Drawing.Size(380, 21);
            this.comboBoxLabelType.TabIndex = 87;
            // 
            // userControlModuleRelatedEntryExsiccate
            // 
            this.userControlModuleRelatedEntryExsiccate.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelPresetSpecimen.SetColumnSpan(this.userControlModuleRelatedEntryExsiccate, 5);
            this.userControlModuleRelatedEntryExsiccate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryExsiccate.Domain = "";
            this.userControlModuleRelatedEntryExsiccate.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryExsiccate.Location = new System.Drawing.Point(96, 30);
            this.userControlModuleRelatedEntryExsiccate.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.userControlModuleRelatedEntryExsiccate.Module = null;
            this.userControlModuleRelatedEntryExsiccate.Name = "userControlModuleRelatedEntryExsiccate";
            this.userControlModuleRelatedEntryExsiccate.ShowInfo = false;
            this.userControlModuleRelatedEntryExsiccate.Size = new System.Drawing.Size(867, 24);
            this.userControlModuleRelatedEntryExsiccate.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryExsiccate.TabIndex = 90;
            // 
            // labelLabelType
            // 
            this.labelLabelType.AutoSize = true;
            this.labelLabelType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLabelType.Location = new System.Drawing.Point(496, 54);
            this.labelLabelType.Name = "labelLabelType";
            this.labelLabelType.Size = new System.Drawing.Size(87, 28);
            this.labelLabelType.TabIndex = 88;
            this.labelLabelType.Text = "Label type:";
            this.labelLabelType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxImageType
            // 
            this.comboBoxImageType.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanelPresetSpecimen.SetColumnSpan(this.comboBoxImageType, 3);
            this.comboBoxImageType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxImageType.Location = new System.Drawing.Point(96, 85);
            this.comboBoxImageType.MaxDropDownItems = 20;
            this.comboBoxImageType.Name = "comboBoxImageType";
            this.comboBoxImageType.Size = new System.Drawing.Size(394, 21);
            this.comboBoxImageType.TabIndex = 74;
            // 
            // labelImageType
            // 
            this.labelImageType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelImageType.Location = new System.Drawing.Point(3, 82);
            this.labelImageType.Name = "labelImageType";
            this.labelImageType.Size = new System.Drawing.Size(87, 27);
            this.labelImageType.TabIndex = 75;
            this.labelImageType.Text = "Image type:";
            this.labelImageType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlDatePanelAccessionDate
            // 
            this.tableLayoutPanelPresetSpecimen.SetColumnSpan(this.userControlDatePanelAccessionDate, 3);
            this.userControlDatePanelAccessionDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDatePanelAccessionDate.Location = new System.Drawing.Point(96, 3);
            this.userControlDatePanelAccessionDate.Margin = new System.Windows.Forms.Padding(3, 3, 1, 0);
            this.userControlDatePanelAccessionDate.Name = "userControlDatePanelAccessionDate";
            this.userControlDatePanelAccessionDate.Size = new System.Drawing.Size(396, 24);
            this.userControlDatePanelAccessionDate.TabIndex = 83;
            // 
            // labelExsiccate
            // 
            this.labelExsiccate.AutoSize = true;
            this.labelExsiccate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelExsiccate.Location = new System.Drawing.Point(3, 27);
            this.labelExsiccate.Name = "labelExsiccate";
            this.labelExsiccate.Size = new System.Drawing.Size(87, 27);
            this.labelExsiccate.TabIndex = 89;
            this.labelExsiccate.Text = "Exsiccate:";
            this.labelExsiccate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelAccessionDate
            // 
            this.labelAccessionDate.AutoSize = true;
            this.labelAccessionDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAccessionDate.Location = new System.Drawing.Point(3, 0);
            this.labelAccessionDate.Name = "labelAccessionDate";
            this.labelAccessionDate.Size = new System.Drawing.Size(87, 27);
            this.labelAccessionDate.TabIndex = 84;
            this.labelAccessionDate.Text = "Accession date:";
            this.labelAccessionDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelProject
            // 
            this.labelProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProject.Location = new System.Drawing.Point(496, 0);
            this.labelProject.Name = "labelProject";
            this.labelProject.Size = new System.Drawing.Size(87, 27);
            this.labelProject.TabIndex = 41;
            this.labelProject.Text = "Project:";
            this.labelProject.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxProject
            // 
            this.comboBoxProject.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanelPresetSpecimen.SetColumnSpan(this.comboBoxProject, 3);
            this.comboBoxProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxProject.Location = new System.Drawing.Point(589, 3);
            this.comboBoxProject.MaxDropDownItems = 20;
            this.comboBoxProject.Name = "comboBoxProject";
            this.comboBoxProject.Size = new System.Drawing.Size(380, 21);
            this.comboBoxProject.TabIndex = 40;
            // 
            // labelDepositor
            // 
            this.labelDepositor.AutoSize = true;
            this.labelDepositor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDepositor.Location = new System.Drawing.Point(3, 54);
            this.labelDepositor.Name = "labelDepositor";
            this.labelDepositor.Size = new System.Drawing.Size(87, 28);
            this.labelDepositor.TabIndex = 91;
            this.labelDepositor.Text = "Depositor:";
            this.labelDepositor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlModuleRelatedEntryDepositor
            // 
            this.userControlModuleRelatedEntryDepositor.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelPresetSpecimen.SetColumnSpan(this.userControlModuleRelatedEntryDepositor, 3);
            this.userControlModuleRelatedEntryDepositor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryDepositor.Domain = "";
            this.userControlModuleRelatedEntryDepositor.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryDepositor.Location = new System.Drawing.Point(96, 57);
            this.userControlModuleRelatedEntryDepositor.Module = null;
            this.userControlModuleRelatedEntryDepositor.Name = "userControlModuleRelatedEntryDepositor";
            this.userControlModuleRelatedEntryDepositor.ShowInfo = false;
            this.userControlModuleRelatedEntryDepositor.Size = new System.Drawing.Size(394, 22);
            this.userControlModuleRelatedEntryDepositor.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryDepositor.TabIndex = 92;
            // 
            // tabPageUnits
            // 
            this.tabPageUnits.Controls.Add(this.tableLayoutPanelPresetUnits);
            this.helpProvider.SetHelpKeyword(this.tabPageUnits, "Import lists");
            this.helpProvider.SetHelpNavigator(this.tabPageUnits, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.tabPageUnits.ImageIndex = 2;
            this.tabPageUnits.Location = new System.Drawing.Point(4, 23);
            this.tabPageUnits.Name = "tabPageUnits";
            this.helpProvider.SetShowHelp(this.tabPageUnits, true);
            this.tabPageUnits.Size = new System.Drawing.Size(978, 268);
            this.tabPageUnits.TabIndex = 2;
            this.tabPageUnits.Text = "Organisms";
            this.tabPageUnits.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelPresetUnits
            // 
            this.tableLayoutPanelPresetUnits.ColumnCount = 4;
            this.tableLayoutPanelPresetUnits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanelPresetUnits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelPresetUnits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPresetUnits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPresetUnits.Controls.Add(this.tableLayoutPanelUnit1, 0, 0);
            this.tableLayoutPanelPresetUnits.Controls.Add(this.tableLayoutPanelUnit2, 2, 0);
            this.tableLayoutPanelPresetUnits.Controls.Add(this.checkBoxTaxonAnywhere, 0, 2);
            this.tableLayoutPanelPresetUnits.Controls.Add(this.checkBoxInsertRelation, 0, 1);
            this.tableLayoutPanelPresetUnits.Controls.Add(this.radioButtonMain1, 1, 2);
            this.tableLayoutPanelPresetUnits.Controls.Add(this.radioButtonMain2, 3, 2);
            this.tableLayoutPanelPresetUnits.Controls.Add(this.labelMain, 2, 2);
            this.tableLayoutPanelPresetUnits.Controls.Add(this.comboBoxRelationType, 1, 1);
            this.tableLayoutPanelPresetUnits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelPresetUnits.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelPresetUnits.Name = "tableLayoutPanelPresetUnits";
            this.tableLayoutPanelPresetUnits.RowCount = 3;
            this.tableLayoutPanelPresetUnits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPresetUnits.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPresetUnits.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPresetUnits.Size = new System.Drawing.Size(978, 268);
            this.tableLayoutPanelPresetUnits.TabIndex = 97;
            // 
            // tableLayoutPanelUnit1
            // 
            this.tableLayoutPanelUnit1.ColumnCount = 3;
            this.tableLayoutPanelPresetUnits.SetColumnSpan(this.tableLayoutPanelUnit1, 2);
            this.tableLayoutPanelUnit1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelUnit1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelUnit1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelUnit1.Controls.Add(this.labelTaxonomicGroup, 0, 1);
            this.tableLayoutPanelUnit1.Controls.Add(this.radioButtonUnit1isHost, 2, 5);
            this.tableLayoutPanelUnit1.Controls.Add(this.comboBoxTaxonomicGroup, 0, 2);
            this.tableLayoutPanelUnit1.Controls.Add(this.labelIdentificationListUnit1, 1, 1);
            this.tableLayoutPanelUnit1.Controls.Add(this.listBoxIdentificationsUnit1, 1, 2);
            this.tableLayoutPanelUnit1.Controls.Add(this.buttonIdentification1Down, 1, 5);
            this.tableLayoutPanelUnit1.Controls.Add(this.textBoxUnitTable1, 1, 0);
            this.tableLayoutPanelUnit1.Controls.Add(this.checkBox2Units, 0, 0);
            this.tableLayoutPanelUnit1.Controls.Add(this.labelIdentificationExampleFor1, 0, 3);
            this.tableLayoutPanelUnit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelUnit1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelUnit1.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.tableLayoutPanelUnit1.Name = "tableLayoutPanelUnit1";
            this.tableLayoutPanelUnit1.RowCount = 6;
            this.tableLayoutPanelUnit1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelUnit1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelUnit1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelUnit1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelUnit1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelUnit1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelUnit1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelUnit1.Size = new System.Drawing.Size(467, 212);
            this.tableLayoutPanelUnit1.TabIndex = 98;
            // 
            // labelTaxonomicGroup
            // 
            this.labelTaxonomicGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTaxonomicGroup.Location = new System.Drawing.Point(3, 26);
            this.labelTaxonomicGroup.Name = "labelTaxonomicGroup";
            this.labelTaxonomicGroup.Size = new System.Drawing.Size(227, 20);
            this.labelTaxonomicGroup.TabIndex = 43;
            this.labelTaxonomicGroup.Text = "Taxon. group:";
            this.labelTaxonomicGroup.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // radioButtonUnit1isHost
            // 
            this.radioButtonUnit1isHost.AutoSize = true;
            this.radioButtonUnit1isHost.Dock = System.Windows.Forms.DockStyle.Right;
            this.radioButtonUnit1isHost.Location = new System.Drawing.Point(450, 191);
            this.radioButtonUnit1isHost.Name = "radioButtonUnit1isHost";
            this.radioButtonUnit1isHost.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radioButtonUnit1isHost.Size = new System.Drawing.Size(14, 18);
            this.radioButtonUnit1isHost.TabIndex = 103;
            this.radioButtonUnit1isHost.UseVisualStyleBackColor = true;
            this.radioButtonUnit1isHost.Click += new System.EventHandler(this.radioButtonUnit1isHost_Click);
            // 
            // comboBoxTaxonomicGroup
            // 
            this.comboBoxTaxonomicGroup.BackColor = System.Drawing.SystemColors.Window;
            this.comboBoxTaxonomicGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxTaxonomicGroup.Location = new System.Drawing.Point(3, 49);
            this.comboBoxTaxonomicGroup.MaxDropDownItems = 20;
            this.comboBoxTaxonomicGroup.Name = "comboBoxTaxonomicGroup";
            this.comboBoxTaxonomicGroup.Size = new System.Drawing.Size(227, 21);
            this.comboBoxTaxonomicGroup.TabIndex = 42;
            // 
            // labelIdentificationListUnit1
            // 
            this.labelIdentificationListUnit1.AutoSize = true;
            this.tableLayoutPanelUnit1.SetColumnSpan(this.labelIdentificationListUnit1, 2);
            this.labelIdentificationListUnit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelIdentificationListUnit1.Location = new System.Drawing.Point(236, 26);
            this.labelIdentificationListUnit1.Name = "labelIdentificationListUnit1";
            this.labelIdentificationListUnit1.Size = new System.Drawing.Size(228, 20);
            this.labelIdentificationListUnit1.TabIndex = 97;
            this.labelIdentificationListUnit1.Text = "Identification tables";
            this.labelIdentificationListUnit1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // listBoxIdentificationsUnit1
            // 
            this.tableLayoutPanelUnit1.SetColumnSpan(this.listBoxIdentificationsUnit1, 2);
            this.listBoxIdentificationsUnit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxIdentificationsUnit1.FormattingEnabled = true;
            this.listBoxIdentificationsUnit1.IntegralHeight = false;
            this.listBoxIdentificationsUnit1.Location = new System.Drawing.Point(236, 49);
            this.listBoxIdentificationsUnit1.Name = "listBoxIdentificationsUnit1";
            this.tableLayoutPanelUnit1.SetRowSpan(this.listBoxIdentificationsUnit1, 3);
            this.listBoxIdentificationsUnit1.Size = new System.Drawing.Size(228, 136);
            this.listBoxIdentificationsUnit1.TabIndex = 98;
            // 
            // buttonIdentification1Down
            // 
            this.buttonIdentification1Down.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonIdentification1Down.Image = global::DiversityCollection.Resource.ArrowDown;
            this.buttonIdentification1Down.Location = new System.Drawing.Point(348, 191);
            this.buttonIdentification1Down.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.buttonIdentification1Down.Name = "buttonIdentification1Down";
            this.buttonIdentification1Down.Size = new System.Drawing.Size(1, 18);
            this.buttonIdentification1Down.TabIndex = 100;
            this.toolTip.SetToolTip(this.buttonIdentification1Down, "move selected table to buttom of list");
            this.buttonIdentification1Down.UseVisualStyleBackColor = true;
            this.buttonIdentification1Down.Click += new System.EventHandler(this.buttonIdentification1Down_Click);
            // 
            // textBoxUnitTable1
            // 
            this.tableLayoutPanelUnit1.SetColumnSpan(this.textBoxUnitTable1, 2);
            this.textBoxUnitTable1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxUnitTable1.Location = new System.Drawing.Point(236, 3);
            this.textBoxUnitTable1.Name = "textBoxUnitTable1";
            this.textBoxUnitTable1.ReadOnly = true;
            this.textBoxUnitTable1.Size = new System.Drawing.Size(228, 20);
            this.textBoxUnitTable1.TabIndex = 102;
            // 
            // checkBox2Units
            // 
            this.checkBox2Units.AutoSize = true;
            this.checkBox2Units.Checked = true;
            this.checkBox2Units.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2Units.Location = new System.Drawing.Point(3, 3);
            this.checkBox2Units.Name = "checkBox2Units";
            this.checkBox2Units.Size = new System.Drawing.Size(206, 17);
            this.checkBox2Units.TabIndex = 103;
            this.checkBox2Units.Text = "Datasets contain 2 organisms. Tables:";
            this.checkBox2Units.UseVisualStyleBackColor = true;
            this.checkBox2Units.CheckedChanged += new System.EventHandler(this.checkBox2Units_CheckedChanged);
            // 
            // labelIdentificationExampleFor1
            // 
            this.labelIdentificationExampleFor1.AutoSize = true;
            this.labelIdentificationExampleFor1.Location = new System.Drawing.Point(3, 73);
            this.labelIdentificationExampleFor1.Name = "labelIdentificationExampleFor1";
            this.labelIdentificationExampleFor1.Size = new System.Drawing.Size(13, 13);
            this.labelIdentificationExampleFor1.TabIndex = 104;
            this.labelIdentificationExampleFor1.Text = "?";
            this.labelIdentificationExampleFor1.Visible = false;
            // 
            // tableLayoutPanelUnit2
            // 
            this.tableLayoutPanelUnit2.ColumnCount = 4;
            this.tableLayoutPanelPresetUnits.SetColumnSpan(this.tableLayoutPanelUnit2, 2);
            this.tableLayoutPanelUnit2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelUnit2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelUnit2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelUnit2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelUnit2.Controls.Add(this.labelIdentificationListUnit2, 1, 1);
            this.tableLayoutPanelUnit2.Controls.Add(this.listBoxIdentificationsUnit2, 1, 2);
            this.tableLayoutPanelUnit2.Controls.Add(this.labelTaxonomicGroupUnit2, 3, 1);
            this.tableLayoutPanelUnit2.Controls.Add(this.comboBoxTaxonomicGroupUnit2, 3, 2);
            this.tableLayoutPanelUnit2.Controls.Add(this.labelHost, 0, 4);
            this.tableLayoutPanelUnit2.Controls.Add(this.radioButtonUnit2isHost, 1, 4);
            this.tableLayoutPanelUnit2.Controls.Add(this.buttonMoveToUnit2, 0, 2);
            this.tableLayoutPanelUnit2.Controls.Add(this.buttonMoveToUnit1, 0, 3);
            this.tableLayoutPanelUnit2.Controls.Add(this.buttonIdentification2Down, 2, 4);
            this.tableLayoutPanelUnit2.Controls.Add(this.buttonSwitchUnitTables, 0, 0);
            this.tableLayoutPanelUnit2.Controls.Add(this.textBoxUnitTable2, 1, 0);
            this.tableLayoutPanelUnit2.Controls.Add(this.labelIdentificationExampleFor2, 3, 3);
            this.tableLayoutPanelUnit2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelUnit2.Location = new System.Drawing.Point(470, 3);
            this.tableLayoutPanelUnit2.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.tableLayoutPanelUnit2.Name = "tableLayoutPanelUnit2";
            this.tableLayoutPanelUnit2.RowCount = 5;
            this.tableLayoutPanelUnit2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelUnit2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelUnit2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelUnit2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelUnit2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelUnit2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelUnit2.Size = new System.Drawing.Size(505, 212);
            this.tableLayoutPanelUnit2.TabIndex = 99;
            // 
            // labelIdentificationListUnit2
            // 
            this.labelIdentificationListUnit2.AutoSize = true;
            this.tableLayoutPanelUnit2.SetColumnSpan(this.labelIdentificationListUnit2, 2);
            this.labelIdentificationListUnit2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelIdentificationListUnit2.Location = new System.Drawing.Point(33, 29);
            this.labelIdentificationListUnit2.Name = "labelIdentificationListUnit2";
            this.labelIdentificationListUnit2.Size = new System.Drawing.Size(230, 17);
            this.labelIdentificationListUnit2.TabIndex = 0;
            this.labelIdentificationListUnit2.Text = "Identification tables";
            this.labelIdentificationListUnit2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // listBoxIdentificationsUnit2
            // 
            this.tableLayoutPanelUnit2.SetColumnSpan(this.listBoxIdentificationsUnit2, 2);
            this.listBoxIdentificationsUnit2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxIdentificationsUnit2.FormattingEnabled = true;
            this.listBoxIdentificationsUnit2.IntegralHeight = false;
            this.listBoxIdentificationsUnit2.Location = new System.Drawing.Point(33, 49);
            this.listBoxIdentificationsUnit2.Name = "listBoxIdentificationsUnit2";
            this.tableLayoutPanelUnit2.SetRowSpan(this.listBoxIdentificationsUnit2, 2);
            this.listBoxIdentificationsUnit2.Size = new System.Drawing.Size(230, 136);
            this.listBoxIdentificationsUnit2.TabIndex = 1;
            // 
            // labelTaxonomicGroupUnit2
            // 
            this.labelTaxonomicGroupUnit2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTaxonomicGroupUnit2.Location = new System.Drawing.Point(269, 29);
            this.labelTaxonomicGroupUnit2.Name = "labelTaxonomicGroupUnit2";
            this.labelTaxonomicGroupUnit2.Size = new System.Drawing.Size(233, 17);
            this.labelTaxonomicGroupUnit2.TabIndex = 44;
            this.labelTaxonomicGroupUnit2.Text = "Taxon. group:";
            this.labelTaxonomicGroupUnit2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // comboBoxTaxonomicGroupUnit2
            // 
            this.comboBoxTaxonomicGroupUnit2.BackColor = System.Drawing.SystemColors.Window;
            this.comboBoxTaxonomicGroupUnit2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxTaxonomicGroupUnit2.Location = new System.Drawing.Point(269, 49);
            this.comboBoxTaxonomicGroupUnit2.MaxDropDownItems = 20;
            this.comboBoxTaxonomicGroupUnit2.Name = "comboBoxTaxonomicGroupUnit2";
            this.comboBoxTaxonomicGroupUnit2.Size = new System.Drawing.Size(233, 21);
            this.comboBoxTaxonomicGroupUnit2.TabIndex = 45;
            // 
            // labelHost
            // 
            this.labelHost.AutoSize = true;
            this.labelHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHost.Location = new System.Drawing.Point(0, 188);
            this.labelHost.Margin = new System.Windows.Forms.Padding(0);
            this.labelHost.Name = "labelHost";
            this.labelHost.Size = new System.Drawing.Size(30, 24);
            this.labelHost.TabIndex = 105;
            this.labelHost.Text = "Host";
            this.labelHost.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radioButtonUnit2isHost
            // 
            this.radioButtonUnit2isHost.AutoSize = true;
            this.radioButtonUnit2isHost.Checked = true;
            this.radioButtonUnit2isHost.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonUnit2isHost.Location = new System.Drawing.Point(33, 191);
            this.radioButtonUnit2isHost.Name = "radioButtonUnit2isHost";
            this.radioButtonUnit2isHost.Size = new System.Drawing.Size(14, 18);
            this.radioButtonUnit2isHost.TabIndex = 104;
            this.radioButtonUnit2isHost.TabStop = true;
            this.radioButtonUnit2isHost.UseVisualStyleBackColor = true;
            this.radioButtonUnit2isHost.CheckedChanged += new System.EventHandler(this.radioButtonUnit2isHost_CheckedChanged);
            // 
            // buttonMoveToUnit2
            // 
            this.buttonMoveToUnit2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonMoveToUnit2.Image = global::DiversityCollection.Resource.ArrowNext;
            this.buttonMoveToUnit2.Location = new System.Drawing.Point(3, 54);
            this.buttonMoveToUnit2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.buttonMoveToUnit2.Name = "buttonMoveToUnit2";
            this.buttonMoveToUnit2.Size = new System.Drawing.Size(24, 19);
            this.buttonMoveToUnit2.TabIndex = 46;
            this.toolTip.SetToolTip(this.buttonMoveToUnit2, "move selected table form left to right list");
            this.buttonMoveToUnit2.UseVisualStyleBackColor = true;
            this.buttonMoveToUnit2.Click += new System.EventHandler(this.buttonMoveToUnit2_Click);
            // 
            // buttonMoveToUnit1
            // 
            this.buttonMoveToUnit1.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonMoveToUnit1.Image = global::DiversityCollection.Resource.ArrowPrevious;
            this.buttonMoveToUnit1.Location = new System.Drawing.Point(3, 76);
            this.buttonMoveToUnit1.Name = "buttonMoveToUnit1";
            this.buttonMoveToUnit1.Size = new System.Drawing.Size(24, 1);
            this.buttonMoveToUnit1.TabIndex = 47;
            this.toolTip.SetToolTip(this.buttonMoveToUnit1, "move selected table form right to left list");
            this.buttonMoveToUnit1.UseVisualStyleBackColor = true;
            this.buttonMoveToUnit1.Click += new System.EventHandler(this.buttonMoveToUnit1_Click);
            // 
            // buttonIdentification2Down
            // 
            this.buttonIdentification2Down.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonIdentification2Down.Image = global::DiversityCollection.Resource.ArrowDown;
            this.buttonIdentification2Down.Location = new System.Drawing.Point(148, 191);
            this.buttonIdentification2Down.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.buttonIdentification2Down.Name = "buttonIdentification2Down";
            this.buttonIdentification2Down.Size = new System.Drawing.Size(22, 18);
            this.buttonIdentification2Down.TabIndex = 49;
            this.toolTip.SetToolTip(this.buttonIdentification2Down, "Move selected table to buttom of list");
            this.buttonIdentification2Down.UseVisualStyleBackColor = true;
            this.buttonIdentification2Down.Click += new System.EventHandler(this.buttonIdentification2Down_Click);
            // 
            // buttonSwitchUnitTables
            // 
            this.buttonSwitchUnitTables.Location = new System.Drawing.Point(0, 3);
            this.buttonSwitchUnitTables.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.buttonSwitchUnitTables.Name = "buttonSwitchUnitTables";
            this.buttonSwitchUnitTables.Size = new System.Drawing.Size(30, 23);
            this.buttonSwitchUnitTables.TabIndex = 50;
            this.buttonSwitchUnitTables.Text = "<>";
            this.toolTip.SetToolTip(this.buttonSwitchUnitTables, "Switch tables");
            this.buttonSwitchUnitTables.UseVisualStyleBackColor = true;
            this.buttonSwitchUnitTables.Click += new System.EventHandler(this.buttonSwitchUnitTables_Click);
            // 
            // textBoxUnitTable2
            // 
            this.tableLayoutPanelUnit2.SetColumnSpan(this.textBoxUnitTable2, 2);
            this.textBoxUnitTable2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxUnitTable2.Location = new System.Drawing.Point(33, 3);
            this.textBoxUnitTable2.Name = "textBoxUnitTable2";
            this.textBoxUnitTable2.ReadOnly = true;
            this.textBoxUnitTable2.Size = new System.Drawing.Size(230, 20);
            this.textBoxUnitTable2.TabIndex = 51;
            // 
            // labelIdentificationExampleFor2
            // 
            this.labelIdentificationExampleFor2.AutoSize = true;
            this.labelIdentificationExampleFor2.Location = new System.Drawing.Point(269, 73);
            this.labelIdentificationExampleFor2.Name = "labelIdentificationExampleFor2";
            this.labelIdentificationExampleFor2.Size = new System.Drawing.Size(13, 13);
            this.labelIdentificationExampleFor2.TabIndex = 106;
            this.labelIdentificationExampleFor2.Text = "?";
            this.labelIdentificationExampleFor2.Visible = false;
            // 
            // checkBoxTaxonAnywhere
            // 
            this.checkBoxTaxonAnywhere.AutoSize = true;
            this.checkBoxTaxonAnywhere.Checked = true;
            this.checkBoxTaxonAnywhere.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTaxonAnywhere.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxTaxonAnywhere.Location = new System.Drawing.Point(3, 248);
            this.checkBoxTaxonAnywhere.Name = "checkBoxTaxonAnywhere";
            this.checkBoxTaxonAnywhere.Size = new System.Drawing.Size(370, 17);
            this.checkBoxTaxonAnywhere.TabIndex = 97;
            this.checkBoxTaxonAnywhere.Text = "Taxon name is storage location.     Main organism:";
            this.checkBoxTaxonAnywhere.UseVisualStyleBackColor = true;
            // 
            // checkBoxInsertRelation
            // 
            this.checkBoxInsertRelation.AutoSize = true;
            this.checkBoxInsertRelation.Checked = true;
            this.checkBoxInsertRelation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxInsertRelation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxInsertRelation.Location = new System.Drawing.Point(3, 224);
            this.checkBoxInsertRelation.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.checkBoxInsertRelation.Name = "checkBoxInsertRelation";
            this.checkBoxInsertRelation.Size = new System.Drawing.Size(373, 21);
            this.checkBoxInsertRelation.TabIndex = 102;
            this.checkBoxInsertRelation.Text = "Host-parasite or corresponding relation between organisms resp. units:";
            this.checkBoxInsertRelation.UseVisualStyleBackColor = true;
            // 
            // radioButtonMain1
            // 
            this.radioButtonMain1.AutoSize = true;
            this.radioButtonMain1.Checked = true;
            this.radioButtonMain1.Dock = System.Windows.Forms.DockStyle.Right;
            this.radioButtonMain1.Location = new System.Drawing.Point(453, 248);
            this.radioButtonMain1.Name = "radioButtonMain1";
            this.radioButtonMain1.Size = new System.Drawing.Size(14, 17);
            this.radioButtonMain1.TabIndex = 103;
            this.radioButtonMain1.TabStop = true;
            this.radioButtonMain1.UseVisualStyleBackColor = true;
            // 
            // radioButtonMain2
            // 
            this.radioButtonMain2.AutoSize = true;
            this.radioButtonMain2.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonMain2.Location = new System.Drawing.Point(503, 248);
            this.radioButtonMain2.Name = "radioButtonMain2";
            this.radioButtonMain2.Size = new System.Drawing.Size(14, 17);
            this.radioButtonMain2.TabIndex = 104;
            this.radioButtonMain2.UseVisualStyleBackColor = true;
            // 
            // labelMain
            // 
            this.labelMain.AutoSize = true;
            this.labelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMain.Location = new System.Drawing.Point(470, 245);
            this.labelMain.Margin = new System.Windows.Forms.Padding(0);
            this.labelMain.Name = "labelMain";
            this.labelMain.Size = new System.Drawing.Size(30, 23);
            this.labelMain.TabIndex = 105;
            this.labelMain.Text = "Main";
            this.labelMain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBoxRelationType
            // 
            this.tableLayoutPanelPresetUnits.SetColumnSpan(this.comboBoxRelationType, 2);
            this.comboBoxRelationType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxRelationType.FormattingEnabled = true;
            this.comboBoxRelationType.Location = new System.Drawing.Point(379, 221);
            this.comboBoxRelationType.Name = "comboBoxRelationType";
            this.comboBoxRelationType.Size = new System.Drawing.Size(118, 21);
            this.comboBoxRelationType.TabIndex = 106;
            // 
            // tabPageStorage
            // 
            this.tabPageStorage.Controls.Add(this.tableLayoutPanelStorage);
            this.tabPageStorage.ImageIndex = 4;
            this.tabPageStorage.Location = new System.Drawing.Point(4, 23);
            this.tabPageStorage.Name = "tabPageStorage";
            this.tabPageStorage.Size = new System.Drawing.Size(978, 268);
            this.tabPageStorage.TabIndex = 6;
            this.tabPageStorage.Text = "Storage";
            this.tabPageStorage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelStorage
            // 
            this.tableLayoutPanelStorage.ColumnCount = 8;
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelStorage.Controls.Add(this.comboBoxDerivedFrom2, 7, 2);
            this.tableLayoutPanelStorage.Controls.Add(this.comboBoxDerivedFrom, 7, 1);
            this.tableLayoutPanelStorage.Controls.Add(this.labelDerivedFrom2, 6, 2);
            this.tableLayoutPanelStorage.Controls.Add(this.labelDerivedFrom, 6, 1);
            this.tableLayoutPanelStorage.Controls.Add(this.comboBoxMaterialCategory2, 5, 2);
            this.tableLayoutPanelStorage.Controls.Add(this.comboBoxMaterialCategory, 5, 1);
            this.tableLayoutPanelStorage.Controls.Add(this.labelMaterialCategory2, 4, 2);
            this.tableLayoutPanelStorage.Controls.Add(this.labelMaterial, 4, 1);
            this.tableLayoutPanelStorage.Controls.Add(this.userControlHierarchySelectorCollection2, 3, 2);
            this.tableLayoutPanelStorage.Controls.Add(this.comboBoxCollection2, 2, 2);
            this.tableLayoutPanelStorage.Controls.Add(this.userControlHierarchySelectorCollection, 3, 1);
            this.tableLayoutPanelStorage.Controls.Add(this.comboBoxCollection, 2, 1);
            this.tableLayoutPanelStorage.Controls.Add(this.labelCollection2, 1, 2);
            this.tableLayoutPanelStorage.Controls.Add(this.labelCollection, 1, 1);
            this.tableLayoutPanelStorage.Controls.Add(this.labelPart2, 0, 2);
            this.tableLayoutPanelStorage.Controls.Add(this.labelPart1, 0, 1);
            this.tableLayoutPanelStorage.Controls.Add(this.panelPartsContained, 0, 0);
            this.tableLayoutPanelStorage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelStorage.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelStorage.Name = "tableLayoutPanelStorage";
            this.tableLayoutPanelStorage.RowCount = 4;
            this.tableLayoutPanelStorage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStorage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStorage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStorage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelStorage.Size = new System.Drawing.Size(978, 268);
            this.tableLayoutPanelStorage.TabIndex = 0;
            // 
            // comboBoxDerivedFrom2
            // 
            this.comboBoxDerivedFrom2.FormattingEnabled = true;
            this.comboBoxDerivedFrom2.Location = new System.Drawing.Point(853, 64);
            this.comboBoxDerivedFrom2.Name = "comboBoxDerivedFrom2";
            this.comboBoxDerivedFrom2.Size = new System.Drawing.Size(121, 21);
            this.comboBoxDerivedFrom2.TabIndex = 126;
            this.comboBoxDerivedFrom2.Visible = false;
            // 
            // comboBoxDerivedFrom
            // 
            this.comboBoxDerivedFrom.FormattingEnabled = true;
            this.comboBoxDerivedFrom.Location = new System.Drawing.Point(853, 37);
            this.comboBoxDerivedFrom.Name = "comboBoxDerivedFrom";
            this.comboBoxDerivedFrom.Size = new System.Drawing.Size(121, 21);
            this.comboBoxDerivedFrom.TabIndex = 125;
            this.comboBoxDerivedFrom.Visible = false;
            // 
            // labelDerivedFrom2
            // 
            this.labelDerivedFrom2.AutoSize = true;
            this.labelDerivedFrom2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDerivedFrom2.Location = new System.Drawing.Point(797, 67);
            this.labelDerivedFrom2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelDerivedFrom2.Name = "labelDerivedFrom2";
            this.labelDerivedFrom2.Size = new System.Drawing.Size(50, 21);
            this.labelDerivedFrom2.TabIndex = 124;
            this.labelDerivedFrom2.Text = "Der.from:";
            this.labelDerivedFrom2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelDerivedFrom2.Visible = false;
            // 
            // labelDerivedFrom
            // 
            this.labelDerivedFrom.AutoSize = true;
            this.labelDerivedFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDerivedFrom.Location = new System.Drawing.Point(797, 34);
            this.labelDerivedFrom.Name = "labelDerivedFrom";
            this.labelDerivedFrom.Size = new System.Drawing.Size(50, 27);
            this.labelDerivedFrom.TabIndex = 123;
            this.labelDerivedFrom.Text = "Der.from:";
            this.labelDerivedFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelDerivedFrom.Visible = false;
            // 
            // comboBoxMaterialCategory2
            // 
            this.comboBoxMaterialCategory2.BackColor = System.Drawing.SystemColors.Window;
            this.comboBoxMaterialCategory2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxMaterialCategory2.Location = new System.Drawing.Point(513, 64);
            this.comboBoxMaterialCategory2.MaxDropDownItems = 20;
            this.comboBoxMaterialCategory2.Name = "comboBoxMaterialCategory2";
            this.comboBoxMaterialCategory2.Size = new System.Drawing.Size(278, 21);
            this.comboBoxMaterialCategory2.TabIndex = 122;
            this.comboBoxMaterialCategory2.Visible = false;
            // 
            // comboBoxMaterialCategory
            // 
            this.comboBoxMaterialCategory.BackColor = System.Drawing.SystemColors.Window;
            this.comboBoxMaterialCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxMaterialCategory.Location = new System.Drawing.Point(513, 37);
            this.comboBoxMaterialCategory.MaxDropDownItems = 20;
            this.comboBoxMaterialCategory.Name = "comboBoxMaterialCategory";
            this.comboBoxMaterialCategory.Size = new System.Drawing.Size(278, 21);
            this.comboBoxMaterialCategory.TabIndex = 121;
            // 
            // labelMaterialCategory2
            // 
            this.labelMaterialCategory2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaterialCategory2.Location = new System.Drawing.Point(420, 67);
            this.labelMaterialCategory2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelMaterialCategory2.Name = "labelMaterialCategory2";
            this.labelMaterialCategory2.Size = new System.Drawing.Size(87, 21);
            this.labelMaterialCategory2.TabIndex = 120;
            this.labelMaterialCategory2.Text = "Material:";
            this.labelMaterialCategory2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelMaterialCategory2.Visible = false;
            // 
            // labelMaterial
            // 
            this.labelMaterial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaterial.Location = new System.Drawing.Point(420, 34);
            this.labelMaterial.Name = "labelMaterial";
            this.labelMaterial.Size = new System.Drawing.Size(87, 27);
            this.labelMaterial.TabIndex = 119;
            this.labelMaterial.Text = "Material:";
            this.labelMaterial.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlHierarchySelectorCollection2
            // 
            this.userControlHierarchySelectorCollection2.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlHierarchySelectorCollection2.Location = new System.Drawing.Point(397, 64);
            this.userControlHierarchySelectorCollection2.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.userControlHierarchySelectorCollection2.Name = "userControlHierarchySelectorCollection2";
            this.userControlHierarchySelectorCollection2.Size = new System.Drawing.Size(17, 20);
            this.userControlHierarchySelectorCollection2.TabIndex = 118;
            this.userControlHierarchySelectorCollection2.Visible = false;
            // 
            // comboBoxCollection2
            // 
            this.comboBoxCollection2.BackColor = System.Drawing.SystemColors.Window;
            this.comboBoxCollection2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxCollection2.Location = new System.Drawing.Point(116, 64);
            this.comboBoxCollection2.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.comboBoxCollection2.MaxDropDownItems = 20;
            this.comboBoxCollection2.Name = "comboBoxCollection2";
            this.comboBoxCollection2.Size = new System.Drawing.Size(281, 21);
            this.comboBoxCollection2.TabIndex = 117;
            this.comboBoxCollection2.Visible = false;
            // 
            // userControlHierarchySelectorCollection
            // 
            this.userControlHierarchySelectorCollection.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlHierarchySelectorCollection.Location = new System.Drawing.Point(397, 37);
            this.userControlHierarchySelectorCollection.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.userControlHierarchySelectorCollection.Name = "userControlHierarchySelectorCollection";
            this.userControlHierarchySelectorCollection.Size = new System.Drawing.Size(17, 20);
            this.userControlHierarchySelectorCollection.TabIndex = 116;
            // 
            // comboBoxCollection
            // 
            this.comboBoxCollection.BackColor = System.Drawing.SystemColors.Window;
            this.comboBoxCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxCollection.Location = new System.Drawing.Point(116, 37);
            this.comboBoxCollection.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.comboBoxCollection.MaxDropDownItems = 20;
            this.comboBoxCollection.Name = "comboBoxCollection";
            this.comboBoxCollection.Size = new System.Drawing.Size(281, 21);
            this.comboBoxCollection.TabIndex = 115;
            // 
            // labelCollection2
            // 
            this.labelCollection2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollection2.Location = new System.Drawing.Point(50, 67);
            this.labelCollection2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelCollection2.Name = "labelCollection2";
            this.labelCollection2.Size = new System.Drawing.Size(60, 21);
            this.labelCollection2.TabIndex = 114;
            this.labelCollection2.Text = "Collection:";
            this.labelCollection2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelCollection2.Visible = false;
            // 
            // labelCollection
            // 
            this.labelCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollection.Location = new System.Drawing.Point(50, 34);
            this.labelCollection.Name = "labelCollection";
            this.labelCollection.Size = new System.Drawing.Size(60, 27);
            this.labelCollection.TabIndex = 113;
            this.labelCollection.Text = "Collection:";
            this.labelCollection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelPart2
            // 
            this.labelPart2.AutoSize = true;
            this.labelPart2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPart2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPart2.Location = new System.Drawing.Point(3, 67);
            this.labelPart2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelPart2.Name = "labelPart2";
            this.labelPart2.Size = new System.Drawing.Size(41, 21);
            this.labelPart2.TabIndex = 112;
            this.labelPart2.Text = "Part 2";
            this.labelPart2.Visible = false;
            // 
            // labelPart1
            // 
            this.labelPart1.AutoSize = true;
            this.labelPart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPart1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPart1.Location = new System.Drawing.Point(3, 34);
            this.labelPart1.Name = "labelPart1";
            this.labelPart1.Size = new System.Drawing.Size(41, 27);
            this.labelPart1.TabIndex = 111;
            this.labelPart1.Text = "Part 1";
            this.labelPart1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelPartsContained
            // 
            this.tableLayoutPanelStorage.SetColumnSpan(this.panelPartsContained, 8);
            this.panelPartsContained.Controls.Add(this.radioButtonManyParts);
            this.panelPartsContained.Controls.Add(this.radioButtonTwoParts);
            this.panelPartsContained.Controls.Add(this.radioButtonOnePart);
            this.panelPartsContained.Controls.Add(this.radioButtonNoPart);
            this.panelPartsContained.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPartsContained.Location = new System.Drawing.Point(3, 8);
            this.panelPartsContained.Margin = new System.Windows.Forms.Padding(3, 8, 0, 0);
            this.panelPartsContained.Name = "panelPartsContained";
            this.panelPartsContained.Size = new System.Drawing.Size(975, 26);
            this.panelPartsContained.TabIndex = 110;
            // 
            // radioButtonManyParts
            // 
            this.radioButtonManyParts.AutoSize = true;
            this.radioButtonManyParts.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonManyParts.Location = new System.Drawing.Point(453, 0);
            this.radioButtonManyParts.Name = "radioButtonManyParts";
            this.radioButtonManyParts.Size = new System.Drawing.Size(144, 26);
            this.radioButtonManyParts.TabIndex = 109;
            this.radioButtonManyParts.TabStop = true;
            this.radioButtonManyParts.Text = "many parts in a collection";
            this.radioButtonManyParts.UseVisualStyleBackColor = true;
            // 
            // radioButtonTwoParts
            // 
            this.radioButtonTwoParts.AutoSize = true;
            this.radioButtonTwoParts.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonTwoParts.Location = new System.Drawing.Point(328, 0);
            this.radioButtonTwoParts.Name = "radioButtonTwoParts";
            this.radioButtonTwoParts.Size = new System.Drawing.Size(125, 26);
            this.radioButtonTwoParts.TabIndex = 108;
            this.radioButtonTwoParts.TabStop = true;
            this.radioButtonTwoParts.Text = "2 parts in a collection";
            this.radioButtonTwoParts.UseVisualStyleBackColor = true;
            this.radioButtonTwoParts.CheckedChanged += new System.EventHandler(this.radioButtonTwoParts_CheckedChanged);
            // 
            // radioButtonOnePart
            // 
            this.radioButtonOnePart.AutoSize = true;
            this.radioButtonOnePart.Checked = true;
            this.radioButtonOnePart.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonOnePart.Location = new System.Drawing.Point(199, 0);
            this.radioButtonOnePart.Name = "radioButtonOnePart";
            this.radioButtonOnePart.Size = new System.Drawing.Size(129, 26);
            this.radioButtonOnePart.TabIndex = 107;
            this.radioButtonOnePart.TabStop = true;
            this.radioButtonOnePart.Text = "1 part in a collection   ";
            this.radioButtonOnePart.UseVisualStyleBackColor = true;
            this.radioButtonOnePart.CheckedChanged += new System.EventHandler(this.radioButtonOnePart_CheckedChanged);
            // 
            // radioButtonNoPart
            // 
            this.radioButtonNoPart.AutoSize = true;
            this.radioButtonNoPart.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonNoPart.Location = new System.Drawing.Point(0, 0);
            this.radioButtonNoPart.Name = "radioButtonNoPart";
            this.radioButtonNoPart.Size = new System.Drawing.Size(199, 26);
            this.radioButtonNoPart.TabIndex = 106;
            this.radioButtonNoPart.Text = "Data contain no part in a collection   ";
            this.radioButtonNoPart.UseVisualStyleBackColor = true;
            this.radioButtonNoPart.CheckedChanged += new System.EventHandler(this.radioButtonNoPart_CheckedChanged);
            // 
            // tabPageAdding
            // 
            this.tabPageAdding.Controls.Add(this.tableLayoutPanelAdding);
            this.tabPageAdding.ImageIndex = 5;
            this.tabPageAdding.Location = new System.Drawing.Point(4, 23);
            this.tabPageAdding.Name = "tabPageAdding";
            this.tabPageAdding.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAdding.Size = new System.Drawing.Size(978, 268);
            this.tabPageAdding.TabIndex = 7;
            this.tabPageAdding.Text = "Adding data";
            this.tabPageAdding.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelAdding
            // 
            this.tableLayoutPanelAdding.ColumnCount = 6;
            this.tableLayoutPanelAdding.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAdding.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAdding.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAdding.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAdding.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAdding.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAdding.Controls.Add(this.labelToPartMatch, 4, 1);
            this.tableLayoutPanelAdding.Controls.Add(this.pictureBoxAddPartToPart, 3, 1);
            this.tableLayoutPanelAdding.Controls.Add(this.pictureBoxAddToPart, 1, 1);
            this.tableLayoutPanelAdding.Controls.Add(this.pictureBoxArrowAddToPart, 2, 1);
            this.tableLayoutPanelAdding.Controls.Add(this.radioButtonAddPartsToSpecimen, 0, 0);
            this.tableLayoutPanelAdding.Controls.Add(this.radioButtonAddPartsToPart, 0, 1);
            this.tableLayoutPanelAdding.Controls.Add(this.pictureBoxAddToSpecimen, 1, 0);
            this.tableLayoutPanelAdding.Controls.Add(this.pictureBoxArrowAdd, 2, 0);
            this.tableLayoutPanelAdding.Controls.Add(this.pictureBoxAddPartToSpecimen, 3, 0);
            this.tableLayoutPanelAdding.Controls.Add(this.labelAddPartToSpecimenMatchColumn, 5, 0);
            this.tableLayoutPanelAdding.Controls.Add(this.comboBoxAddPartToPartMatchingColumn, 5, 1);
            this.tableLayoutPanelAdding.Controls.Add(this.labelAddToSpecimenMatch, 4, 0);
            this.tableLayoutPanelAdding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelAdding.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelAdding.Name = "tableLayoutPanelAdding";
            this.tableLayoutPanelAdding.RowCount = 3;
            this.tableLayoutPanelAdding.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAdding.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAdding.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAdding.Size = new System.Drawing.Size(972, 262);
            this.tableLayoutPanelAdding.TabIndex = 0;
            // 
            // labelToPartMatch
            // 
            this.labelToPartMatch.AutoSize = true;
            this.labelToPartMatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelToPartMatch.Location = new System.Drawing.Point(205, 29);
            this.labelToPartMatch.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelToPartMatch.Name = "labelToPartMatch";
            this.labelToPartMatch.Size = new System.Drawing.Size(53, 21);
            this.labelToPartMatch.TabIndex = 11;
            this.labelToPartMatch.Text = "match by:";
            // 
            // pictureBoxAddPartToPart
            // 
            this.pictureBoxAddPartToPart.Image = global::DiversityCollection.Resource.Specimen;
            this.pictureBoxAddPartToPart.Location = new System.Drawing.Point(183, 26);
            this.pictureBoxAddPartToPart.Name = "pictureBoxAddPartToPart";
            this.pictureBoxAddPartToPart.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxAddPartToPart.TabIndex = 8;
            this.pictureBoxAddPartToPart.TabStop = false;
            // 
            // pictureBoxAddToPart
            // 
            this.pictureBoxAddToPart.Image = global::DiversityCollection.Resource.Specimen;
            this.pictureBoxAddToPart.Location = new System.Drawing.Point(139, 26);
            this.pictureBoxAddToPart.Name = "pictureBoxAddToPart";
            this.pictureBoxAddToPart.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxAddToPart.TabIndex = 7;
            this.pictureBoxAddToPart.TabStop = false;
            // 
            // pictureBoxArrowAddToPart
            // 
            this.pictureBoxArrowAddToPart.Image = global::DiversityCollection.Resource.ArrowPrevious;
            this.pictureBoxArrowAddToPart.Location = new System.Drawing.Point(161, 26);
            this.pictureBoxArrowAddToPart.Name = "pictureBoxArrowAddToPart";
            this.pictureBoxArrowAddToPart.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxArrowAddToPart.TabIndex = 6;
            this.pictureBoxArrowAddToPart.TabStop = false;
            // 
            // radioButtonAddPartsToSpecimen
            // 
            this.radioButtonAddPartsToSpecimen.AutoSize = true;
            this.radioButtonAddPartsToSpecimen.Checked = true;
            this.radioButtonAddPartsToSpecimen.Location = new System.Drawing.Point(3, 3);
            this.radioButtonAddPartsToSpecimen.Name = "radioButtonAddPartsToSpecimen";
            this.radioButtonAddPartsToSpecimen.Size = new System.Drawing.Size(130, 17);
            this.radioButtonAddPartsToSpecimen.TabIndex = 0;
            this.radioButtonAddPartsToSpecimen.TabStop = true;
            this.radioButtonAddPartsToSpecimen.Text = "Add parts to specimen";
            this.radioButtonAddPartsToSpecimen.UseVisualStyleBackColor = true;
            // 
            // radioButtonAddPartsToPart
            // 
            this.radioButtonAddPartsToPart.AutoSize = true;
            this.radioButtonAddPartsToPart.Location = new System.Drawing.Point(3, 26);
            this.radioButtonAddPartsToPart.Name = "radioButtonAddPartsToPart";
            this.radioButtonAddPartsToPart.Size = new System.Drawing.Size(103, 17);
            this.radioButtonAddPartsToPart.TabIndex = 1;
            this.radioButtonAddPartsToPart.TabStop = true;
            this.radioButtonAddPartsToPart.Text = "Add parts to part";
            this.radioButtonAddPartsToPart.UseVisualStyleBackColor = true;
            // 
            // pictureBoxAddToSpecimen
            // 
            this.pictureBoxAddToSpecimen.Image = global::DiversityCollection.Resource.CollectionSpecimen;
            this.pictureBoxAddToSpecimen.Location = new System.Drawing.Point(139, 3);
            this.pictureBoxAddToSpecimen.Name = "pictureBoxAddToSpecimen";
            this.pictureBoxAddToSpecimen.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxAddToSpecimen.TabIndex = 2;
            this.pictureBoxAddToSpecimen.TabStop = false;
            // 
            // pictureBoxArrowAdd
            // 
            this.pictureBoxArrowAdd.Image = global::DiversityCollection.Resource.ArrowPrevious;
            this.pictureBoxArrowAdd.Location = new System.Drawing.Point(161, 3);
            this.pictureBoxArrowAdd.Name = "pictureBoxArrowAdd";
            this.pictureBoxArrowAdd.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxArrowAdd.TabIndex = 3;
            this.pictureBoxArrowAdd.TabStop = false;
            // 
            // pictureBoxAddPartToSpecimen
            // 
            this.pictureBoxAddPartToSpecimen.Image = global::DiversityCollection.Resource.Specimen;
            this.pictureBoxAddPartToSpecimen.Location = new System.Drawing.Point(183, 3);
            this.pictureBoxAddPartToSpecimen.Name = "pictureBoxAddPartToSpecimen";
            this.pictureBoxAddPartToSpecimen.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxAddPartToSpecimen.TabIndex = 4;
            this.pictureBoxAddPartToSpecimen.TabStop = false;
            // 
            // labelAddPartToSpecimenMatchColumn
            // 
            this.labelAddPartToSpecimenMatchColumn.AutoSize = true;
            this.labelAddPartToSpecimenMatchColumn.Location = new System.Drawing.Point(264, 6);
            this.labelAddPartToSpecimenMatchColumn.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelAddPartToSpecimenMatchColumn.Name = "labelAddPartToSpecimenMatchColumn";
            this.labelAddPartToSpecimenMatchColumn.Size = new System.Drawing.Size(94, 13);
            this.labelAddPartToSpecimenMatchColumn.TabIndex = 5;
            this.labelAddPartToSpecimenMatchColumn.Text = "Accession number";
            // 
            // comboBoxAddPartToPartMatchingColumn
            // 
            this.comboBoxAddPartToPartMatchingColumn.FormattingEnabled = true;
            this.comboBoxAddPartToPartMatchingColumn.Location = new System.Drawing.Point(264, 26);
            this.comboBoxAddPartToPartMatchingColumn.Name = "comboBoxAddPartToPartMatchingColumn";
            this.comboBoxAddPartToPartMatchingColumn.Size = new System.Drawing.Size(1, 21);
            this.comboBoxAddPartToPartMatchingColumn.TabIndex = 9;
            // 
            // labelAddToSpecimenMatch
            // 
            this.labelAddToSpecimenMatch.AutoSize = true;
            this.labelAddToSpecimenMatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAddToSpecimenMatch.Location = new System.Drawing.Point(205, 6);
            this.labelAddToSpecimenMatch.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelAddToSpecimenMatch.Name = "labelAddToSpecimenMatch";
            this.labelAddToSpecimenMatch.Size = new System.Drawing.Size(53, 17);
            this.labelAddToSpecimenMatch.TabIndex = 10;
            this.labelAddToSpecimenMatch.Text = "match by:";
            // 
            // tabPageHidden
            // 
            this.tabPageHidden.Controls.Add(this.userControlModuleRelatedEntryIdentifiedBy);
            this.tabPageHidden.Controls.Add(this.labelIdentifiedBy);
            this.tabPageHidden.Controls.Add(this.labelIdentification);
            this.tabPageHidden.Controls.Add(this.userControlModuleRelatedEntryIdentification);
            this.tabPageHidden.Location = new System.Drawing.Point(4, 23);
            this.tabPageHidden.Name = "tabPageHidden";
            this.tabPageHidden.Size = new System.Drawing.Size(978, 268);
            this.tabPageHidden.TabIndex = 3;
            this.tabPageHidden.Text = "[Hidden]";
            this.tabPageHidden.UseVisualStyleBackColor = true;
            // 
            // userControlModuleRelatedEntryIdentifiedBy
            // 
            this.userControlModuleRelatedEntryIdentifiedBy.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryIdentifiedBy.Domain = "";
            this.userControlModuleRelatedEntryIdentifiedBy.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryIdentifiedBy.Location = new System.Drawing.Point(3, 58);
            this.userControlModuleRelatedEntryIdentifiedBy.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.userControlModuleRelatedEntryIdentifiedBy.Module = null;
            this.userControlModuleRelatedEntryIdentifiedBy.Name = "userControlModuleRelatedEntryIdentifiedBy";
            this.userControlModuleRelatedEntryIdentifiedBy.ShowInfo = false;
            this.userControlModuleRelatedEntryIdentifiedBy.Size = new System.Drawing.Size(225, 24);
            this.userControlModuleRelatedEntryIdentifiedBy.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryIdentifiedBy.TabIndex = 98;
            // 
            // labelIdentifiedBy
            // 
            this.labelIdentifiedBy.AutoSize = true;
            this.labelIdentifiedBy.Location = new System.Drawing.Point(5, 44);
            this.labelIdentifiedBy.Name = "labelIdentifiedBy";
            this.labelIdentifiedBy.Size = new System.Drawing.Size(48, 13);
            this.labelIdentifiedBy.TabIndex = 97;
            this.labelIdentifiedBy.Text = "Ident.by:";
            this.labelIdentifiedBy.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelIdentification
            // 
            this.labelIdentification.AutoSize = true;
            this.labelIdentification.Location = new System.Drawing.Point(5, 10);
            this.labelIdentification.Name = "labelIdentification";
            this.labelIdentification.Size = new System.Drawing.Size(70, 13);
            this.labelIdentification.TabIndex = 95;
            this.labelIdentification.Text = "Identification:";
            this.labelIdentification.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // userControlModuleRelatedEntryIdentification
            // 
            this.userControlModuleRelatedEntryIdentification.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryIdentification.Domain = "";
            this.userControlModuleRelatedEntryIdentification.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryIdentification.Location = new System.Drawing.Point(3, 24);
            this.userControlModuleRelatedEntryIdentification.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.userControlModuleRelatedEntryIdentification.Module = null;
            this.userControlModuleRelatedEntryIdentification.Name = "userControlModuleRelatedEntryIdentification";
            this.userControlModuleRelatedEntryIdentification.ShowInfo = false;
            this.userControlModuleRelatedEntryIdentification.Size = new System.Drawing.Size(225, 20);
            this.userControlModuleRelatedEntryIdentification.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryIdentification.TabIndex = 96;
            // 
            // tabPageFile
            // 
            this.tabPageFile.Controls.Add(this.dataGridViewFile);
            this.helpProvider.SetHelpKeyword(this.tabPageFile, "Reimport");
            this.helpProvider.SetHelpNavigator(this.tabPageFile, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.tabPageFile.Location = new System.Drawing.Point(4, 23);
            this.tabPageFile.Name = "tabPageFile";
            this.tabPageFile.Padding = new System.Windows.Forms.Padding(3);
            this.helpProvider.SetShowHelp(this.tabPageFile, true);
            this.tabPageFile.Size = new System.Drawing.Size(978, 268);
            this.tabPageFile.TabIndex = 5;
            this.tabPageFile.Text = "File";
            this.tabPageFile.UseVisualStyleBackColor = true;
            // 
            // dataGridViewFile
            // 
            this.dataGridViewFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFile.ColumnHeadersVisible = false;
            this.dataGridViewFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewFile.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewFile.Name = "dataGridViewFile";
            this.dataGridViewFile.ReadOnly = true;
            this.dataGridViewFile.RowHeadersVisible = false;
            this.dataGridViewFile.Size = new System.Drawing.Size(972, 262);
            this.dataGridViewFile.TabIndex = 0;
            // 
            // textBoxImportFile
            // 
            this.textBoxImportFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxImportFile.Location = new System.Drawing.Point(237, 6);
            this.textBoxImportFile.Margin = new System.Windows.Forms.Padding(0, 6, 3, 3);
            this.textBoxImportFile.Name = "textBoxImportFile";
            this.textBoxImportFile.ReadOnly = true;
            this.textBoxImportFile.Size = new System.Drawing.Size(663, 20);
            this.textBoxImportFile.TabIndex = 1;
            this.textBoxImportFile.TextChanged += new System.EventHandler(this.textBoxImportFile_TextChanged);
            // 
            // labelImportFile
            // 
            this.labelImportFile.AutoSize = true;
            this.labelImportFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelImportFile.Location = new System.Drawing.Point(185, 0);
            this.labelImportFile.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelImportFile.Name = "labelImportFile";
            this.labelImportFile.Size = new System.Drawing.Size(52, 29);
            this.labelImportFile.TabIndex = 79;
            this.labelImportFile.Text = "Filename:";
            this.labelImportFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            this.openFileDialog.RestoreDirectory = true;
            // 
            // buttonReload
            // 
            this.buttonReload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonReload.Image = global::DiversityCollection.Resource.Reload;
            this.buttonReload.Location = new System.Drawing.Point(933, 3);
            this.buttonReload.Margin = new System.Windows.Forms.Padding(0, 3, 3, 2);
            this.buttonReload.Name = "buttonReload";
            this.buttonReload.Padding = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.buttonReload.Size = new System.Drawing.Size(23, 24);
            this.buttonReload.TabIndex = 82;
            this.toolTip.SetToolTip(this.buttonReload, "Reload the current file");
            this.buttonReload.UseVisualStyleBackColor = true;
            this.buttonReload.Click += new System.EventHandler(this.buttonReload_Click);
            // 
            // tableLayoutPanelHeader
            // 
            this.tableLayoutPanelHeader.ColumnCount = 7;
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeader.Controls.Add(this.buttonHeaderFeedback, 6, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.labelImportFile, 2, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.buttonOpenFile, 4, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.textBoxImportFile, 3, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.labelEncoding, 0, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.comboBoxEncoding, 1, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.buttonReload, 5, 0);
            this.tableLayoutPanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelHeader.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelHeader.Name = "tableLayoutPanelHeader";
            this.tableLayoutPanelHeader.RowCount = 1;
            this.tableLayoutPanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelHeader.Size = new System.Drawing.Size(992, 29);
            this.tableLayoutPanelHeader.TabIndex = 1;
            // 
            // buttonHeaderFeedback
            // 
            this.buttonHeaderFeedback.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonHeaderFeedback.Image = global::DiversityCollection.Resource.Feedback;
            this.buttonHeaderFeedback.Location = new System.Drawing.Point(962, 3);
            this.buttonHeaderFeedback.Name = "buttonHeaderFeedback";
            this.buttonHeaderFeedback.Size = new System.Drawing.Size(27, 23);
            this.buttonHeaderFeedback.TabIndex = 83;
            this.buttonHeaderFeedback.UseVisualStyleBackColor = true;
            this.buttonHeaderFeedback.Click += new System.EventHandler(this.buttonHeaderFeedback_Click);
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOpenFile.Image = global::DiversityCollection.Resource.Open;
            this.buttonOpenFile.Location = new System.Drawing.Point(903, 3);
            this.buttonOpenFile.Margin = new System.Windows.Forms.Padding(0, 3, 3, 2);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(27, 24);
            this.buttonOpenFile.TabIndex = 0;
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // labelEncoding
            // 
            this.labelEncoding.AutoSize = true;
            this.labelEncoding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEncoding.Location = new System.Drawing.Point(3, 0);
            this.labelEncoding.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelEncoding.Name = "labelEncoding";
            this.labelEncoding.Size = new System.Drawing.Size(55, 29);
            this.labelEncoding.TabIndex = 80;
            this.labelEncoding.Text = "Encoding:";
            this.labelEncoding.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxEncoding
            // 
            this.comboBoxEncoding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxEncoding.FormattingEnabled = true;
            this.comboBoxEncoding.Location = new System.Drawing.Point(58, 5);
            this.comboBoxEncoding.Margin = new System.Windows.Forms.Padding(0, 5, 3, 3);
            this.comboBoxEncoding.Name = "comboBoxEncoding";
            this.comboBoxEncoding.Size = new System.Drawing.Size(121, 21);
            this.comboBoxEncoding.TabIndex = 81;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 29);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.groupBoxPresettings);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tableLayoutPanelImport);
            this.splitContainerMain.Size = new System.Drawing.Size(992, 667);
            this.splitContainerMain.SplitterDistance = 314;
            this.splitContainerMain.TabIndex = 2;
            // 
            // dataSetCollectionSpecimen
            // 
            this.dataSetCollectionSpecimen.DataSetName = "DataSetCollectionSpecimen";
            this.dataSetCollectionSpecimen.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // FormImportList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 696);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.tableLayoutPanelHeader);
            this.helpProvider.SetHelpKeyword(this, "Import lists");
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormImportList";
            this.helpProvider.SetShowHelp(this, true);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Import list";
            this.tableLayoutPanelImport.ResumeLayout(false);
            this.tableLayoutPanelImport.PerformLayout();
            this.toolStripNavigation.ResumeLayout(false);
            this.toolStripNavigation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownUpTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownImport)).EndInit();
            this.groupBoxPresettings.ResumeLayout(false);
            this.tabControlPresettings.ResumeLayout(false);
            this.tabPageColumnMapping.ResumeLayout(false);
            this.tableLayoutPanelColumnMapping.ResumeLayout(false);
            this.tableLayoutPanelColumnMapping.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewColumnMappingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewColumnMapping)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDataStartInLine)).EndInit();
            this.tabPageEvent.ResumeLayout(false);
            this.tableLayoutPanelEventHierarchy.ResumeLayout(false);
            this.tableLayoutPanelEventHierarchy.PerformLayout();
            this.tableLayoutPanelEventDate.ResumeLayout(false);
            this.tableLayoutPanelEventDate.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanelCollector.ResumeLayout(false);
            this.tableLayoutPanelCollector.PerformLayout();
            this.tableLayoutPanelPlaceAndCountry.ResumeLayout(false);
            this.tableLayoutPanelPlaceAndCountry.PerformLayout();
            this.tabPageSpecimen.ResumeLayout(false);
            this.tableLayoutPanelPresetSpecimen.ResumeLayout(false);
            this.tableLayoutPanelPresetSpecimen.PerformLayout();
            this.tabPageUnits.ResumeLayout(false);
            this.tableLayoutPanelPresetUnits.ResumeLayout(false);
            this.tableLayoutPanelPresetUnits.PerformLayout();
            this.tableLayoutPanelUnit1.ResumeLayout(false);
            this.tableLayoutPanelUnit1.PerformLayout();
            this.tableLayoutPanelUnit2.ResumeLayout(false);
            this.tableLayoutPanelUnit2.PerformLayout();
            this.tabPageStorage.ResumeLayout(false);
            this.tableLayoutPanelStorage.ResumeLayout(false);
            this.tableLayoutPanelStorage.PerformLayout();
            this.panelPartsContained.ResumeLayout(false);
            this.panelPartsContained.PerformLayout();
            this.tabPageAdding.ResumeLayout(false);
            this.tableLayoutPanelAdding.ResumeLayout(false);
            this.tableLayoutPanelAdding.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAddPartToPart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAddToPart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxArrowAddToPart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAddToSpecimen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxArrowAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAddPartToSpecimen)).EndInit();
            this.tabPageHidden.ResumeLayout(false);
            this.tabPageHidden.PerformLayout();
            this.tabPageFile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFile)).EndInit();
            this.tableLayoutPanelHeader.ResumeLayout(false);
            this.tableLayoutPanelHeader.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCollectionSpecimen)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelImport;
        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.TextBox textBoxImportFile;
        private System.Windows.Forms.Button buttonStartImport;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button buttonAnalyse;
        private System.Windows.Forms.TreeView treeViewAnalysis;
        private System.Windows.Forms.ToolStrip toolStripNavigation;
        private System.Windows.Forms.ToolStripButton toolStripButtonPrevious;
        private System.Windows.Forms.ToolStripLabel toolStripLabelCurrent;
        private System.Windows.Forms.ToolStripButton toolStripButtonNext;
        private System.Windows.Forms.GroupBox groupBoxPresettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPresetUnits;
        private System.Windows.Forms.Label labelGazetteer;
        private System.Windows.Forms.Label labelCollector;
        private DiversityWorkbench.UserControlModuleRelatedEntry userControlModuleRelatedEntryGazetteer;
        private DiversityWorkbench.UserControlModuleRelatedEntry userControlModuleRelatedEntryCollector;
        private System.Windows.Forms.Label labelCollectionDate;
        private System.Windows.Forms.Label labelAccessionDate;
        private DiversityWorkbench.UserControlDatePanel userControlDatePanelCollectionDate;
        private DiversityWorkbench.UserControlModuleRelatedEntry userControlModuleRelatedEntryExsiccate;
        private System.Windows.Forms.Label labelExsiccate;
        private System.Windows.Forms.Label labelLabelType;
        private System.Windows.Forms.ComboBox comboBoxLabelType;
        private DiversityWorkbench.UserControlDatePanel userControlDatePanelAccessionDate;
        private System.Windows.Forms.Label labelImageType;
        private System.Windows.Forms.ComboBox comboBoxImageType;
        private System.Windows.Forms.Label labelProject;
        private System.Windows.Forms.ComboBox comboBoxProject;
        private System.Windows.Forms.Label labelTaxonomicGroup;
        private System.Windows.Forms.ComboBox comboBoxTaxonomicGroup;
        private System.Windows.Forms.CheckBox checkBoxTaxonAnywhere;
        private System.Windows.Forms.CheckBox checkBoxInsertRelation;
        private System.Windows.Forms.Label labelUpTo;
        private System.Windows.Forms.NumericUpDown numericUpDownUpTo;
        private Datasets.DataSetCollectionSpecimen dataSetCollectionSpecimen;
        private System.Windows.Forms.Label labelImportFile;
        private System.Windows.Forms.Label labelImport;
        private System.Windows.Forms.RadioButton radioButtonImportAll;
        private System.Windows.Forms.RadioButton radioButtonImportFirstLines;
        private System.Windows.Forms.NumericUpDown numericUpDownImport;
        private System.Windows.Forms.Label labelImportUpTo;
        private System.Windows.Forms.TabControl tabControlPresettings;
        private System.Windows.Forms.TabPage tabPageEvent;
        private System.Windows.Forms.TabPage tabPageSpecimen;
        private System.Windows.Forms.TabPage tabPageUnits;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCollector;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPresetSpecimen;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelUnit2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelUnit1;
        private System.Windows.Forms.Label labelIdentificationListUnit1;
        private System.Windows.Forms.Label labelIdentificationListUnit2;
        private System.Windows.Forms.ListBox listBoxIdentificationsUnit1;
        private System.Windows.Forms.Button buttonIdentification1Down;
        private System.Windows.Forms.ListBox listBoxIdentificationsUnit2;
        private System.Windows.Forms.Label labelTaxonomicGroupUnit2;
        private System.Windows.Forms.ComboBox comboBoxTaxonomicGroupUnit2;
        private System.Windows.Forms.Button buttonMoveToUnit2;
        private System.Windows.Forms.Button buttonMoveToUnit1;
        private System.Windows.Forms.Button buttonIdentification2Down;
        private System.Windows.Forms.RadioButton radioButtonUnit1isHost;
        private System.Windows.Forms.RadioButton radioButtonUnit2isHost;
        private System.Windows.Forms.Label labelHost;
        private System.Windows.Forms.TextBox textBoxUnitTable1;
        private System.Windows.Forms.Button buttonSwitchUnitTables;
        private System.Windows.Forms.TextBox textBoxUnitTable2;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox checkBox2Units;
        private System.Windows.Forms.RadioButton radioButtonMain1;
        private System.Windows.Forms.RadioButton radioButtonMain2;
        private System.Windows.Forms.Label labelMain;
        private System.Windows.Forms.Label labelCountry;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBoxRelationType;
        private System.Windows.Forms.TabPage tabPageHidden;
        private DiversityWorkbench.UserControlModuleRelatedEntry userControlModuleRelatedEntryIdentifiedBy;
        private System.Windows.Forms.Label labelIdentifiedBy;
        private DiversityWorkbench.UserControlModuleRelatedEntry userControlModuleRelatedEntryIdentification;
        private System.Windows.Forms.Label labelIdentification;
        private System.Windows.Forms.ComboBox comboBoxCountry;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPlaceAndCountry;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEventDate;
        private System.Windows.Forms.Label labelCollectionTime;
        private System.Windows.Forms.TextBox textBoxCollectionTime;
        private System.Windows.Forms.Label labelCollectionTimeSpan;
        private System.Windows.Forms.TextBox textBoxCollectionTimeSpan;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Label labelDepositor;
        private DiversityWorkbench.UserControlModuleRelatedEntry userControlModuleRelatedEntryDepositor;
        private System.Windows.Forms.ProgressBar progressBarImport;
        private System.Windows.Forms.CheckBox checkBoxImportEmptyData;
        private System.Windows.Forms.TabPage tabPageColumnMapping;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelColumnMapping;
        private System.Windows.Forms.DataGridView dataGridViewColumnMappingSource;
        private System.Windows.Forms.Label labelColumnMappingTable;
        private System.Windows.Forms.ComboBox comboBoxColumnMappingTable;
        private System.Windows.Forms.ComboBox comboBoxColumnMappingColumn;
        private System.Windows.Forms.Label labelColumnMappingColumn;
        private System.Windows.Forms.Label labelColumnMappingAlias;
        private System.Windows.Forms.Button buttonColumnMappingSave;
        private System.Windows.Forms.DataGridView dataGridViewColumnMapping;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelHeader;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Label labelDataStartInLine;
        private System.Windows.Forms.NumericUpDown numericUpDownDataStartInLine;
        private System.Windows.Forms.TabPage tabPageFile;
        private System.Windows.Forms.DataGridView dataGridViewFile;
        private System.Windows.Forms.Label labelIdentificationExampleFor1;
        private System.Windows.Forms.Label labelIdentificationExampleFor2;
        private System.Windows.Forms.Label labelEncoding;
        private System.Windows.Forms.ComboBox comboBoxEncoding;
        private System.Windows.Forms.Button buttonReload;
        private System.Windows.Forms.Label labelTotalLines;
        private System.Windows.Forms.TextBox textBoxTotalLines;
        private System.Windows.Forms.ToolStripButton toolStripButtonFirst;
        private System.Windows.Forms.ToolStripButton toolStripButtonLast;
        private System.Windows.Forms.Label labelColumnGroup;
        private System.Windows.Forms.ComboBox comboBoxColumnGroup;
        private System.Windows.Forms.ComboBox comboBoxColumnMappingAlias;
        private System.Windows.Forms.Button buttonColumnMappingSaveToFile;
        private System.Windows.Forms.Button buttonColumnMappingImportFromFile;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEventHierarchy;
        private System.Windows.Forms.RadioButton radioButtonEventsSeparate;
        private System.Windows.Forms.RadioButton radioButtonEventsInGroups;
        private System.Windows.Forms.TreeView treeViewEventsSeparate;
        private System.Windows.Forms.TreeView treeViewEventsInGroups;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.CheckBox checkBoxEventSeries;
        private System.Windows.Forms.Label labelEventSeriesCode;
        private System.Windows.Forms.Label labelEventSeriesDescription;
        private System.Windows.Forms.Label labelEventSeriesNotes;
        private System.Windows.Forms.TextBox textBoxEventSeriesCode;
        private System.Windows.Forms.TextBox textBoxEventSeriesDescription;
        private System.Windows.Forms.TextBox textBoxEventSeriesNotes;
        private System.Windows.Forms.ImageList imageListGrey;
        private System.Windows.Forms.Button buttonGetEventSeries;
        private System.Windows.Forms.Button buttonHeaderFeedback;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonLoadSettings;
        private System.Windows.Forms.Button buttonIgnoreColumn;
        private System.Windows.Forms.TabPage tabPageStorage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelStorage;
        private System.Windows.Forms.Label labelCollection2;
        private System.Windows.Forms.Label labelCollection;
        private System.Windows.Forms.Label labelPart2;
        private System.Windows.Forms.Label labelPart1;
        private System.Windows.Forms.Panel panelPartsContained;
        private System.Windows.Forms.RadioButton radioButtonTwoParts;
        private System.Windows.Forms.RadioButton radioButtonOnePart;
        private System.Windows.Forms.RadioButton radioButtonNoPart;
        private System.Windows.Forms.Label labelDerivedFrom;
        private System.Windows.Forms.ComboBox comboBoxMaterialCategory2;
        private System.Windows.Forms.ComboBox comboBoxMaterialCategory;
        private System.Windows.Forms.Label labelMaterialCategory2;
        private System.Windows.Forms.Label labelMaterial;
        private DiversityWorkbench.UserControlHierarchySelector userControlHierarchySelectorCollection2;
        private System.Windows.Forms.ComboBox comboBoxCollection2;
        private DiversityWorkbench.UserControlHierarchySelector userControlHierarchySelectorCollection;
        private System.Windows.Forms.ComboBox comboBoxCollection;
        private System.Windows.Forms.Label labelDerivedFrom2;
        private System.Windows.Forms.ComboBox comboBoxDerivedFrom2;
        private System.Windows.Forms.ComboBox comboBoxDerivedFrom;
        private System.Windows.Forms.RadioButton radioButtonManyParts;
        private System.Windows.Forms.TabPage tabPageAdding;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAdding;
        private System.Windows.Forms.RadioButton radioButtonAddPartsToSpecimen;
        private System.Windows.Forms.RadioButton radioButtonAddPartsToPart;
        private System.Windows.Forms.PictureBox pictureBoxAddToSpecimen;
        private System.Windows.Forms.PictureBox pictureBoxArrowAdd;
        private System.Windows.Forms.PictureBox pictureBoxAddPartToSpecimen;
        private System.Windows.Forms.Label labelAddPartToSpecimenMatchColumn;
        private System.Windows.Forms.PictureBox pictureBoxArrowAddToPart;
        private System.Windows.Forms.PictureBox pictureBoxAddToPart;
        private System.Windows.Forms.PictureBox pictureBoxAddPartToPart;
        private System.Windows.Forms.ComboBox comboBoxAddPartToPartMatchingColumn;
        private System.Windows.Forms.Label labelAddToSpecimenMatch;
        private System.Windows.Forms.Label labelToPartMatch;
    }
}