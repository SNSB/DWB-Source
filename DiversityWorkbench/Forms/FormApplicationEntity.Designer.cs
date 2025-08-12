namespace DiversityWorkbench.Forms
{
    partial class FormApplicationEntity
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApplicationEntity));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.userControlQueryList = new DiversityWorkbench.UserControls.UserControlQueryList();
            this.splitContainerData = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelEntity = new System.Windows.Forms.TableLayoutPanel();
            this.labelEntityGroup = new System.Windows.Forms.Label();
            this.labelEntityNotes = new System.Windows.Forms.Label();
            this.textBoxEntityNotes = new System.Windows.Forms.TextBox();
            this.entityBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetEntity = new DiversityWorkbench.Datasets.DataSetEntity();
            this.checkBoxObsolete = new System.Windows.Forms.CheckBox();
            this.comboBoxDisplayGroup = new System.Windows.Forms.ComboBox();
            this.entityListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.textBoxEntity = new System.Windows.Forms.TextBox();
            this.splitContainerDataDetails = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelUsage = new System.Windows.Forms.TableLayoutPanel();
            this.labelUsage = new System.Windows.Forms.Label();
            this.buttonUsageNew = new System.Windows.Forms.Button();
            this.dataGridViewUsage = new System.Windows.Forms.DataGridView();
            this.entityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entityContextDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.entityContextEnumBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.accessibilityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.entityAccessibilityEnumBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.determinationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.entityDeterminationEnumBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.visibilityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.entityVisibilityEnumBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.presetValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entityUsageBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tableLayoutPanelRepresentation = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridViewRepresentation = new System.Windows.Forms.DataGridView();
            this.entityDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.entityLanguageCodeEnumBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.entityContextDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entityRepresentationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.buttonRepresentationNew = new System.Windows.Forms.Button();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertAllMissingTablesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertMissingColumnsForAllTablesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertMissingColumnsForSelectedTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertPKForSelectedTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.representationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertMissingLanguageEntriesForAllEntitiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateDescriptionsAccordingToDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateDatabaseAccordingToDescriptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usageToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.insertUsageForTablesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertUsageForColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.administrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accessibilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.determinationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visibilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.feedbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.entityTableAdapter = new DiversityWorkbench.Datasets.DataSetEntityTableAdapters.EntityTableAdapter();
            this.entityContext_EnumTableAdapter = new DiversityWorkbench.Datasets.DataSetEntityTableAdapters.EntityContext_EnumTableAdapter();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.entityLanguageCode_EnumTableAdapter = new DiversityWorkbench.Datasets.DataSetEntityTableAdapters.EntityLanguageCode_EnumTableAdapter();
            this.entityListTableAdapter = new DiversityWorkbench.Datasets.DataSetEntityTableAdapters.EntityListTableAdapter();
            this.entityUsageTableAdapter = new DiversityWorkbench.Datasets.DataSetEntityTableAdapters.EntityUsageTableAdapter();
            this.entityRepresentationTableAdapter = new DiversityWorkbench.Datasets.DataSetEntityTableAdapters.EntityRepresentationTableAdapter();
            this.entityAccessibility_EnumTableAdapter = new DiversityWorkbench.Datasets.DataSetEntityTableAdapters.EntityAccessibility_EnumTableAdapter();
            this.entityDetermination_EnumTableAdapter = new DiversityWorkbench.Datasets.DataSetEntityTableAdapters.EntityDetermination_EnumTableAdapter();
            this.entityVisibility_EnumTableAdapter = new DiversityWorkbench.Datasets.DataSetEntityTableAdapters.EntityVisibility_EnumTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerData)).BeginInit();
            this.splitContainerData.Panel1.SuspendLayout();
            this.splitContainerData.Panel2.SuspendLayout();
            this.splitContainerData.SuspendLayout();
            this.tableLayoutPanelEntity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.entityBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetEntity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.entityListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDataDetails)).BeginInit();
            this.splitContainerDataDetails.Panel1.SuspendLayout();
            this.splitContainerDataDetails.Panel2.SuspendLayout();
            this.splitContainerDataDetails.SuspendLayout();
            this.tableLayoutPanelUsage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.entityContextEnumBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.entityAccessibilityEnumBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.entityDeterminationEnumBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.entityVisibilityEnumBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.entityUsageBindingSource)).BeginInit();
            this.tableLayoutPanelRepresentation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRepresentation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.entityLanguageCodeEnumBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.entityRepresentationBindingSource)).BeginInit();
            this.menuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 24);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.userControlQueryList);
            this.splitContainerMain.Panel1.Padding = new System.Windows.Forms.Padding(4, 4, 0, 4);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerData);
            this.splitContainerMain.Panel2.Padding = new System.Windows.Forms.Padding(0, 4, 4, 4);
            this.splitContainerMain.Size = new System.Drawing.Size(901, 571);
            this.splitContainerMain.SplitterDistance = 215;
            this.splitContainerMain.TabIndex = 0;
            // 
            // userControlQueryList
            // 
            this.userControlQueryList.BacklinkUpdateEnabled = false;
            this.userControlQueryList.Connection = null;
            this.userControlQueryList.DisplayTextSelectedItem = "";
            this.userControlQueryList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlQueryList.IDisNumeric = true;
            this.userControlQueryList.ImageList = null;
            this.userControlQueryList.IsPredefinedQuery = false;
            this.userControlQueryList.Location = new System.Drawing.Point(4, 4);
            this.userControlQueryList.MaximalNumberOfResults = 100;
            this.userControlQueryList.Name = "userControlQueryList";
            this.userControlQueryList.Optimizing_UsedForQueryList = false;
            this.userControlQueryList.ProjectID = -1;
            this.userControlQueryList.QueryConditionVisiblity = "";
            this.userControlQueryList.QueryDisplayColumns = null;
            this.userControlQueryList.QueryMainTableLocal = null;
            this.userControlQueryList.QueryRestriction = "";
            this.userControlQueryList.RememberQuerySettingsIdentifier = "QueryList";
            this.userControlQueryList.SelectedProjectID = null;
            this.userControlQueryList.Size = new System.Drawing.Size(211, 563);
            this.userControlQueryList.TabIndex = 0;
            this.userControlQueryList.TableColors = null;
            this.userControlQueryList.TableImageIndex = null;
            this.userControlQueryList.WhereClause = null;
            // 
            // splitContainerData
            // 
            this.splitContainerData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerData.Location = new System.Drawing.Point(0, 4);
            this.splitContainerData.Name = "splitContainerData";
            this.splitContainerData.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerData.Panel1
            // 
            this.splitContainerData.Panel1.Controls.Add(this.tableLayoutPanelEntity);
            this.splitContainerData.Panel1MinSize = 66;
            // 
            // splitContainerData.Panel2
            // 
            this.splitContainerData.Panel2.Controls.Add(this.splitContainerDataDetails);
            this.splitContainerData.Size = new System.Drawing.Size(678, 563);
            this.splitContainerData.SplitterDistance = 68;
            this.splitContainerData.TabIndex = 0;
            // 
            // tableLayoutPanelEntity
            // 
            this.tableLayoutPanelEntity.ColumnCount = 3;
            this.tableLayoutPanelEntity.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEntity.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEntity.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEntity.Controls.Add(this.labelEntityGroup, 0, 1);
            this.tableLayoutPanelEntity.Controls.Add(this.labelEntityNotes, 1, 1);
            this.tableLayoutPanelEntity.Controls.Add(this.textBoxEntityNotes, 1, 2);
            this.tableLayoutPanelEntity.Controls.Add(this.checkBoxObsolete, 2, 1);
            this.tableLayoutPanelEntity.Controls.Add(this.comboBoxDisplayGroup, 0, 2);
            this.tableLayoutPanelEntity.Controls.Add(this.textBoxEntity, 0, 0);
            this.tableLayoutPanelEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelEntity.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelEntity.Name = "tableLayoutPanelEntity";
            this.tableLayoutPanelEntity.RowCount = 3;
            this.tableLayoutPanelEntity.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelEntity.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEntity.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelEntity.Size = new System.Drawing.Size(678, 68);
            this.tableLayoutPanelEntity.TabIndex = 0;
            // 
            // labelEntityGroup
            // 
            this.labelEntityGroup.AutoSize = true;
            this.labelEntityGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelEntityGroup.Location = new System.Drawing.Point(3, 25);
            this.labelEntityGroup.Name = "labelEntityGroup";
            this.labelEntityGroup.Size = new System.Drawing.Size(240, 13);
            this.labelEntityGroup.TabIndex = 1;
            this.labelEntityGroup.Text = "Group for the display of the entity";
            // 
            // labelEntityNotes
            // 
            this.labelEntityNotes.AutoSize = true;
            this.labelEntityNotes.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelEntityNotes.Location = new System.Drawing.Point(249, 25);
            this.labelEntityNotes.Name = "labelEntityNotes";
            this.labelEntityNotes.Size = new System.Drawing.Size(355, 13);
            this.labelEntityNotes.TabIndex = 2;
            this.labelEntityNotes.Text = "Notes about the entity";
            // 
            // textBoxEntityNotes
            // 
            this.tableLayoutPanelEntity.SetColumnSpan(this.textBoxEntityNotes, 2);
            this.textBoxEntityNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.entityBindingSource, "Notes", true));
            this.textBoxEntityNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEntityNotes.Location = new System.Drawing.Point(249, 42);
            this.textBoxEntityNotes.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxEntityNotes.Multiline = true;
            this.textBoxEntityNotes.Name = "textBoxEntityNotes";
            this.textBoxEntityNotes.Size = new System.Drawing.Size(426, 23);
            this.textBoxEntityNotes.TabIndex = 4;
            // 
            // entityBindingSource
            // 
            this.entityBindingSource.DataMember = "Entity";
            this.entityBindingSource.DataSource = this.dataSetEntity;
            // 
            // dataSetEntity
            // 
            this.dataSetEntity.DataSetName = "DataSetEntity";
            this.dataSetEntity.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // checkBoxObsolete
            // 
            this.checkBoxObsolete.AutoSize = true;
            this.checkBoxObsolete.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.entityBindingSource, "Obsolete", true));
            this.checkBoxObsolete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxObsolete.Location = new System.Drawing.Point(610, 25);
            this.checkBoxObsolete.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.checkBoxObsolete.Name = "checkBoxObsolete";
            this.checkBoxObsolete.Size = new System.Drawing.Size(68, 17);
            this.checkBoxObsolete.TabIndex = 5;
            this.checkBoxObsolete.Text = "Obsolete";
            this.checkBoxObsolete.UseVisualStyleBackColor = true;
            // 
            // comboBoxDisplayGroup
            // 
            this.comboBoxDisplayGroup.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.entityBindingSource, "DisplayGroup", true));
            this.comboBoxDisplayGroup.DataSource = this.entityListBindingSource;
            this.comboBoxDisplayGroup.DisplayMember = "Entity";
            this.comboBoxDisplayGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxDisplayGroup.FormattingEnabled = true;
            this.comboBoxDisplayGroup.Location = new System.Drawing.Point(3, 42);
            this.comboBoxDisplayGroup.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.comboBoxDisplayGroup.Name = "comboBoxDisplayGroup";
            this.helpProvider.SetShowHelp(this.comboBoxDisplayGroup, true);
            this.comboBoxDisplayGroup.Size = new System.Drawing.Size(240, 21);
            this.comboBoxDisplayGroup.TabIndex = 6;
            this.comboBoxDisplayGroup.ValueMember = "Entity";
            // 
            // textBoxEntity
            // 
            this.textBoxEntity.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelEntity.SetColumnSpan(this.textBoxEntity, 3);
            this.textBoxEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEntity.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxEntity.Location = new System.Drawing.Point(3, 3);
            this.textBoxEntity.Name = "textBoxEntity";
            this.textBoxEntity.ReadOnly = true;
            this.textBoxEntity.Size = new System.Drawing.Size(672, 17);
            this.textBoxEntity.TabIndex = 7;
            this.textBoxEntity.Text = "Entity";
            // 
            // splitContainerDataDetails
            // 
            this.splitContainerDataDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerDataDetails.Location = new System.Drawing.Point(0, 0);
            this.splitContainerDataDetails.Name = "splitContainerDataDetails";
            this.splitContainerDataDetails.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerDataDetails.Panel1
            // 
            this.splitContainerDataDetails.Panel1.Controls.Add(this.tableLayoutPanelUsage);
            // 
            // splitContainerDataDetails.Panel2
            // 
            this.splitContainerDataDetails.Panel2.Controls.Add(this.tableLayoutPanelRepresentation);
            this.splitContainerDataDetails.Size = new System.Drawing.Size(678, 491);
            this.splitContainerDataDetails.SplitterDistance = 236;
            this.splitContainerDataDetails.TabIndex = 0;
            // 
            // tableLayoutPanelUsage
            // 
            this.tableLayoutPanelUsage.ColumnCount = 2;
            this.tableLayoutPanelUsage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelUsage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelUsage.Controls.Add(this.labelUsage, 0, 0);
            this.tableLayoutPanelUsage.Controls.Add(this.buttonUsageNew, 1, 0);
            this.tableLayoutPanelUsage.Controls.Add(this.dataGridViewUsage, 0, 1);
            this.tableLayoutPanelUsage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelUsage.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelUsage.Name = "tableLayoutPanelUsage";
            this.tableLayoutPanelUsage.RowCount = 2;
            this.tableLayoutPanelUsage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelUsage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelUsage.Size = new System.Drawing.Size(678, 236);
            this.tableLayoutPanelUsage.TabIndex = 3;
            // 
            // labelUsage
            // 
            this.labelUsage.AutoSize = true;
            this.labelUsage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUsage.Location = new System.Drawing.Point(3, 0);
            this.labelUsage.Name = "labelUsage";
            this.labelUsage.Size = new System.Drawing.Size(560, 26);
            this.labelUsage.TabIndex = 1;
            this.labelUsage.Text = "Usage: How the entity should be used within a certain context, e.g. hidden, reado" +
    "nly";
            this.labelUsage.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // buttonUsageNew
            // 
            this.buttonUsageNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonUsageNew.Location = new System.Drawing.Point(569, 3);
            this.buttonUsageNew.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.buttonUsageNew.Name = "buttonUsageNew";
            this.buttonUsageNew.Size = new System.Drawing.Size(106, 23);
            this.buttonUsageNew.TabIndex = 2;
            this.buttonUsageNew.Text = "Insert new usage";
            this.buttonUsageNew.UseVisualStyleBackColor = true;
            this.buttonUsageNew.Click += new System.EventHandler(this.buttonUsageNew_Click);
            // 
            // dataGridViewUsage
            // 
            this.dataGridViewUsage.AutoGenerateColumns = false;
            this.dataGridViewUsage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewUsage.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.entityDataGridViewTextBoxColumn,
            this.entityContextDataGridViewTextBoxColumn,
            this.accessibilityDataGridViewTextBoxColumn,
            this.determinationDataGridViewTextBoxColumn,
            this.visibilityDataGridViewTextBoxColumn,
            this.presetValueDataGridViewTextBoxColumn,
            this.dataGridViewTextBoxColumn1});
            this.tableLayoutPanelUsage.SetColumnSpan(this.dataGridViewUsage, 2);
            this.dataGridViewUsage.DataSource = this.entityUsageBindingSource;
            this.dataGridViewUsage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewUsage.Location = new System.Drawing.Point(3, 29);
            this.dataGridViewUsage.Name = "dataGridViewUsage";
            this.dataGridViewUsage.RowHeadersWidth = 24;
            this.dataGridViewUsage.Size = new System.Drawing.Size(672, 204);
            this.dataGridViewUsage.TabIndex = 3;
            this.dataGridViewUsage.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewUsage_DataError);
            // 
            // entityDataGridViewTextBoxColumn
            // 
            this.entityDataGridViewTextBoxColumn.DataPropertyName = "Entity";
            this.entityDataGridViewTextBoxColumn.HeaderText = "Entity";
            this.entityDataGridViewTextBoxColumn.Name = "entityDataGridViewTextBoxColumn";
            this.entityDataGridViewTextBoxColumn.Visible = false;
            this.entityDataGridViewTextBoxColumn.Width = 5;
            // 
            // entityContextDataGridViewTextBoxColumn
            // 
            this.entityContextDataGridViewTextBoxColumn.DataPropertyName = "EntityContext";
            this.entityContextDataGridViewTextBoxColumn.DataSource = this.entityContextEnumBindingSource;
            this.entityContextDataGridViewTextBoxColumn.DisplayMember = "DisplayText";
            this.entityContextDataGridViewTextBoxColumn.HeaderText = "EntityContext";
            this.entityContextDataGridViewTextBoxColumn.Name = "entityContextDataGridViewTextBoxColumn";
            this.entityContextDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.entityContextDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.entityContextDataGridViewTextBoxColumn.ValueMember = "Code";
            // 
            // entityContextEnumBindingSource
            // 
            this.entityContextEnumBindingSource.DataMember = "EntityContext_Enum";
            this.entityContextEnumBindingSource.DataSource = this.dataSetEntity;
            // 
            // accessibilityDataGridViewTextBoxColumn
            // 
            this.accessibilityDataGridViewTextBoxColumn.DataPropertyName = "Accessibility";
            this.accessibilityDataGridViewTextBoxColumn.DataSource = this.entityAccessibilityEnumBindingSource;
            this.accessibilityDataGridViewTextBoxColumn.DisplayMember = "DisplayText";
            this.accessibilityDataGridViewTextBoxColumn.HeaderText = "Accessibility";
            this.accessibilityDataGridViewTextBoxColumn.Name = "accessibilityDataGridViewTextBoxColumn";
            this.accessibilityDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.accessibilityDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.accessibilityDataGridViewTextBoxColumn.ValueMember = "Code";
            // 
            // entityAccessibilityEnumBindingSource
            // 
            this.entityAccessibilityEnumBindingSource.DataMember = "EntityAccessibility_Enum";
            this.entityAccessibilityEnumBindingSource.DataSource = this.dataSetEntity;
            // 
            // determinationDataGridViewTextBoxColumn
            // 
            this.determinationDataGridViewTextBoxColumn.DataPropertyName = "Determination";
            this.determinationDataGridViewTextBoxColumn.DataSource = this.entityDeterminationEnumBindingSource;
            this.determinationDataGridViewTextBoxColumn.DisplayMember = "DisplayText";
            this.determinationDataGridViewTextBoxColumn.HeaderText = "Determination";
            this.determinationDataGridViewTextBoxColumn.Name = "determinationDataGridViewTextBoxColumn";
            this.determinationDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.determinationDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.determinationDataGridViewTextBoxColumn.ValueMember = "Code";
            // 
            // entityDeterminationEnumBindingSource
            // 
            this.entityDeterminationEnumBindingSource.DataMember = "EntityDetermination_Enum";
            this.entityDeterminationEnumBindingSource.DataSource = this.dataSetEntity;
            // 
            // visibilityDataGridViewTextBoxColumn
            // 
            this.visibilityDataGridViewTextBoxColumn.DataPropertyName = "Visibility";
            this.visibilityDataGridViewTextBoxColumn.DataSource = this.entityVisibilityEnumBindingSource;
            this.visibilityDataGridViewTextBoxColumn.DisplayMember = "DisplayText";
            this.visibilityDataGridViewTextBoxColumn.HeaderText = "Visibility";
            this.visibilityDataGridViewTextBoxColumn.Name = "visibilityDataGridViewTextBoxColumn";
            this.visibilityDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.visibilityDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.visibilityDataGridViewTextBoxColumn.ValueMember = "Code";
            // 
            // entityVisibilityEnumBindingSource
            // 
            this.entityVisibilityEnumBindingSource.DataMember = "EntityVisibility_Enum";
            this.entityVisibilityEnumBindingSource.DataSource = this.dataSetEntity;
            // 
            // presetValueDataGridViewTextBoxColumn
            // 
            this.presetValueDataGridViewTextBoxColumn.DataPropertyName = "PresetValue";
            this.presetValueDataGridViewTextBoxColumn.HeaderText = "PresetValue";
            this.presetValueDataGridViewTextBoxColumn.Name = "presetValueDataGridViewTextBoxColumn";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Notes";
            this.dataGridViewTextBoxColumn1.HeaderText = "Notes";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // entityUsageBindingSource
            // 
            this.entityUsageBindingSource.DataMember = "EntityUsage";
            this.entityUsageBindingSource.DataSource = this.dataSetEntity;
            // 
            // tableLayoutPanelRepresentation
            // 
            this.tableLayoutPanelRepresentation.ColumnCount = 2;
            this.tableLayoutPanelRepresentation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRepresentation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelRepresentation.Controls.Add(this.dataGridViewRepresentation, 0, 1);
            this.tableLayoutPanelRepresentation.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanelRepresentation.Controls.Add(this.buttonRepresentationNew, 1, 0);
            this.tableLayoutPanelRepresentation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelRepresentation.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelRepresentation.Name = "tableLayoutPanelRepresentation";
            this.tableLayoutPanelRepresentation.RowCount = 2;
            this.tableLayoutPanelRepresentation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRepresentation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRepresentation.Size = new System.Drawing.Size(678, 251);
            this.tableLayoutPanelRepresentation.TabIndex = 1;
            // 
            // dataGridViewRepresentation
            // 
            this.dataGridViewRepresentation.AutoGenerateColumns = false;
            this.dataGridViewRepresentation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRepresentation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.entityDataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.entityContextDataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6});
            this.tableLayoutPanelRepresentation.SetColumnSpan(this.dataGridViewRepresentation, 2);
            this.dataGridViewRepresentation.DataSource = this.entityRepresentationBindingSource;
            this.dataGridViewRepresentation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewRepresentation.Location = new System.Drawing.Point(3, 29);
            this.dataGridViewRepresentation.Name = "dataGridViewRepresentation";
            this.dataGridViewRepresentation.RowHeadersWidth = 25;
            this.dataGridViewRepresentation.Size = new System.Drawing.Size(672, 219);
            this.dataGridViewRepresentation.TabIndex = 0;
            this.dataGridViewRepresentation.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewRepresentation_DataError);
            // 
            // entityDataGridViewTextBoxColumn1
            // 
            this.entityDataGridViewTextBoxColumn1.DataPropertyName = "Entity";
            this.entityDataGridViewTextBoxColumn1.HeaderText = "Entity";
            this.entityDataGridViewTextBoxColumn1.Name = "entityDataGridViewTextBoxColumn1";
            this.entityDataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "LanguageCode";
            this.dataGridViewTextBoxColumn2.DataSource = this.entityLanguageCodeEnumBindingSource;
            this.dataGridViewTextBoxColumn2.DisplayMember = "DisplayText";
            this.dataGridViewTextBoxColumn2.HeaderText = "LanguageCode";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewTextBoxColumn2.ValueMember = "Code";
            // 
            // entityLanguageCodeEnumBindingSource
            // 
            this.entityLanguageCodeEnumBindingSource.DataMember = "EntityLanguageCode_Enum";
            this.entityLanguageCodeEnumBindingSource.DataSource = this.dataSetEntity;
            // 
            // entityContextDataGridViewTextBoxColumn1
            // 
            this.entityContextDataGridViewTextBoxColumn1.DataPropertyName = "EntityContext";
            this.entityContextDataGridViewTextBoxColumn1.DataSource = this.entityContextEnumBindingSource;
            this.entityContextDataGridViewTextBoxColumn1.DisplayMember = "DisplayText";
            this.entityContextDataGridViewTextBoxColumn1.HeaderText = "EntityContext";
            this.entityContextDataGridViewTextBoxColumn1.Name = "entityContextDataGridViewTextBoxColumn1";
            this.entityContextDataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.entityContextDataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.entityContextDataGridViewTextBoxColumn1.ValueMember = "Code";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "DisplayText";
            this.dataGridViewTextBoxColumn3.HeaderText = "DisplayText";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Abbreviation";
            this.dataGridViewTextBoxColumn4.HeaderText = "Abbreviation";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "Description";
            this.dataGridViewTextBoxColumn5.HeaderText = "Description";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "Notes";
            this.dataGridViewTextBoxColumn6.HeaderText = "Notes";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // entityRepresentationBindingSource
            // 
            this.entityRepresentationBindingSource.DataMember = "EntityRepresentation";
            this.entityRepresentationBindingSource.DataSource = this.dataSetEntity;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(526, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "Representation of an entity in a certain context in the selected language";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // buttonRepresentationNew
            // 
            this.buttonRepresentationNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRepresentationNew.Location = new System.Drawing.Point(535, 3);
            this.buttonRepresentationNew.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.buttonRepresentationNew.Name = "buttonRepresentationNew";
            this.buttonRepresentationNew.Size = new System.Drawing.Size(140, 23);
            this.buttonRepresentationNew.TabIndex = 2;
            this.buttonRepresentationNew.Text = "Insert new representation";
            this.buttonRepresentationNew.UseVisualStyleBackColor = true;
            this.buttonRepresentationNew.Click += new System.EventHandler(this.buttonRepresentationNew_Click);
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataToolStripMenuItem,
            this.representationToolStripMenuItem,
            this.usageToolStripMenuItem1,
            this.administrationToolStripMenuItem,
            this.feedbackToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(901, 24);
            this.menuStripMain.TabIndex = 1;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // dataToolStripMenuItem
            // 
            this.dataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertAllMissingTablesToolStripMenuItem,
            this.insertMissingColumnsForAllTablesToolStripMenuItem,
            this.insertMissingColumnsForSelectedTableToolStripMenuItem,
            this.insertPKForSelectedTableToolStripMenuItem});
            this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            this.dataToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.dataToolStripMenuItem.Text = "Entity";
            // 
            // insertAllMissingTablesToolStripMenuItem
            // 
            this.insertAllMissingTablesToolStripMenuItem.Name = "insertAllMissingTablesToolStripMenuItem";
            this.insertAllMissingTablesToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.insertAllMissingTablesToolStripMenuItem.Text = "Insert all missing tables";
            this.insertAllMissingTablesToolStripMenuItem.Click += new System.EventHandler(this.insertAllMissingTablesToolStripMenuItem_Click);
            // 
            // insertMissingColumnsForAllTablesToolStripMenuItem
            // 
            this.insertMissingColumnsForAllTablesToolStripMenuItem.Name = "insertMissingColumnsForAllTablesToolStripMenuItem";
            this.insertMissingColumnsForAllTablesToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.insertMissingColumnsForAllTablesToolStripMenuItem.Text = "Insert missing columns for all tables";
            this.insertMissingColumnsForAllTablesToolStripMenuItem.Click += new System.EventHandler(this.insertMissingColumnsForAllTablesToolStripMenuItem_Click);
            // 
            // insertMissingColumnsForSelectedTableToolStripMenuItem
            // 
            this.insertMissingColumnsForSelectedTableToolStripMenuItem.Name = "insertMissingColumnsForSelectedTableToolStripMenuItem";
            this.insertMissingColumnsForSelectedTableToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.insertMissingColumnsForSelectedTableToolStripMenuItem.Text = "Insert missing columns for selected table";
            this.insertMissingColumnsForSelectedTableToolStripMenuItem.Click += new System.EventHandler(this.insertMissingColumnsForSelectedTableToolStripMenuItem_Click);
            // 
            // insertPKForSelectedTableToolStripMenuItem
            // 
            this.insertPKForSelectedTableToolStripMenuItem.Name = "insertPKForSelectedTableToolStripMenuItem";
            this.insertPKForSelectedTableToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.insertPKForSelectedTableToolStripMenuItem.Text = "Insert PK for selected table";
            this.insertPKForSelectedTableToolStripMenuItem.Click += new System.EventHandler(this.insertPKForSelectedTableToolStripMenuItem_Click);
            // 
            // representationToolStripMenuItem
            // 
            this.representationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertMissingLanguageEntriesForAllEntitiesToolStripMenuItem,
            this.updateDescriptionsAccordingToDatabaseToolStripMenuItem,
            this.updateDatabaseAccordingToDescriptionToolStripMenuItem});
            this.representationToolStripMenuItem.Name = "representationToolStripMenuItem";
            this.representationToolStripMenuItem.Size = new System.Drawing.Size(98, 20);
            this.representationToolStripMenuItem.Text = "Representation";
            // 
            // insertMissingLanguageEntriesForAllEntitiesToolStripMenuItem
            // 
            this.insertMissingLanguageEntriesForAllEntitiesToolStripMenuItem.Name = "insertMissingLanguageEntriesForAllEntitiesToolStripMenuItem";
            this.insertMissingLanguageEntriesForAllEntitiesToolStripMenuItem.Size = new System.Drawing.Size(311, 22);
            this.insertMissingLanguageEntriesForAllEntitiesToolStripMenuItem.Text = "Insert missing language entries for all entities";
            this.insertMissingLanguageEntriesForAllEntitiesToolStripMenuItem.Click += new System.EventHandler(this.insertMissingLanguageEntriesForAllEntitiesToolStripMenuItem_Click);
            // 
            // updateDescriptionsAccordingToDatabaseToolStripMenuItem
            // 
            this.updateDescriptionsAccordingToDatabaseToolStripMenuItem.Name = "updateDescriptionsAccordingToDatabaseToolStripMenuItem";
            this.updateDescriptionsAccordingToDatabaseToolStripMenuItem.Size = new System.Drawing.Size(311, 22);
            this.updateDescriptionsAccordingToDatabaseToolStripMenuItem.Text = "Update descriptions according to database";
            this.updateDescriptionsAccordingToDatabaseToolStripMenuItem.ToolTipText = "This will transfer the descriptions found in the database to the entries for the " +
    "context GENERAL and the language ENGLISH";
            this.updateDescriptionsAccordingToDatabaseToolStripMenuItem.Click += new System.EventHandler(this.updateDescriptionsAccordingToDatabaseToolStripMenuItem_Click);
            // 
            // updateDatabaseAccordingToDescriptionToolStripMenuItem
            // 
            this.updateDatabaseAccordingToDescriptionToolStripMenuItem.Name = "updateDatabaseAccordingToDescriptionToolStripMenuItem";
            this.updateDatabaseAccordingToDescriptionToolStripMenuItem.Size = new System.Drawing.Size(311, 22);
            this.updateDatabaseAccordingToDescriptionToolStripMenuItem.Text = "Update database according to description";
            this.updateDatabaseAccordingToDescriptionToolStripMenuItem.ToolTipText = "This will write the current entry for the context GENERAL and the language ENGLIS" +
    "H into the database";
            this.updateDatabaseAccordingToDescriptionToolStripMenuItem.Click += new System.EventHandler(this.updateDatabaseAccordingToDescriptionToolStripMenuItem_Click);
            // 
            // usageToolStripMenuItem1
            // 
            this.usageToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertUsageForTablesToolStripMenuItem,
            this.insertUsageForColumnsToolStripMenuItem});
            this.usageToolStripMenuItem1.Name = "usageToolStripMenuItem1";
            this.usageToolStripMenuItem1.Size = new System.Drawing.Size(51, 20);
            this.usageToolStripMenuItem1.Text = "Usage";
            // 
            // insertUsageForTablesToolStripMenuItem
            // 
            this.insertUsageForTablesToolStripMenuItem.Name = "insertUsageForTablesToolStripMenuItem";
            this.insertUsageForTablesToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.insertUsageForTablesToolStripMenuItem.Text = "Insert usage for tables ...";
            this.insertUsageForTablesToolStripMenuItem.Click += new System.EventHandler(this.insertUsageForTablesToolStripMenuItem_Click);
            // 
            // insertUsageForColumnsToolStripMenuItem
            // 
            this.insertUsageForColumnsToolStripMenuItem.Name = "insertUsageForColumnsToolStripMenuItem";
            this.insertUsageForColumnsToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.insertUsageForColumnsToolStripMenuItem.Text = "Insert usage for columns ...";
            this.insertUsageForColumnsToolStripMenuItem.Click += new System.EventHandler(this.insertUsageForColumnsToolStripMenuItem_Click);
            // 
            // administrationToolStripMenuItem
            // 
            this.administrationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextToolStripMenuItem,
            this.usageToolStripMenuItem,
            this.accessibilityToolStripMenuItem,
            this.determinationToolStripMenuItem,
            this.visibilityToolStripMenuItem});
            this.administrationToolStripMenuItem.Name = "administrationToolStripMenuItem";
            this.administrationToolStripMenuItem.Size = new System.Drawing.Size(98, 20);
            this.administrationToolStripMenuItem.Text = "Administration";
            // 
            // contextToolStripMenuItem
            // 
            this.contextToolStripMenuItem.Name = "contextToolStripMenuItem";
            this.contextToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.contextToolStripMenuItem.Text = "Context ...";
            this.contextToolStripMenuItem.Click += new System.EventHandler(this.contextToolStripMenuItem_Click);
            // 
            // usageToolStripMenuItem
            // 
            this.usageToolStripMenuItem.Name = "usageToolStripMenuItem";
            this.usageToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.usageToolStripMenuItem.Text = "Usage ...";
            this.usageToolStripMenuItem.Visible = false;
            this.usageToolStripMenuItem.Click += new System.EventHandler(this.usageToolStripMenuItem_Click);
            // 
            // accessibilityToolStripMenuItem
            // 
            this.accessibilityToolStripMenuItem.Name = "accessibilityToolStripMenuItem";
            this.accessibilityToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.accessibilityToolStripMenuItem.Text = "Accessibility ...";
            this.accessibilityToolStripMenuItem.Click += new System.EventHandler(this.accessibilityToolStripMenuItem_Click);
            // 
            // determinationToolStripMenuItem
            // 
            this.determinationToolStripMenuItem.Name = "determinationToolStripMenuItem";
            this.determinationToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.determinationToolStripMenuItem.Text = "Determination ...";
            this.determinationToolStripMenuItem.Click += new System.EventHandler(this.determinationToolStripMenuItem_Click);
            // 
            // visibilityToolStripMenuItem
            // 
            this.visibilityToolStripMenuItem.Name = "visibilityToolStripMenuItem";
            this.visibilityToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.visibilityToolStripMenuItem.Text = "Visibility ...";
            this.visibilityToolStripMenuItem.Click += new System.EventHandler(this.visibilityToolStripMenuItem_Click);
            // 
            // feedbackToolStripMenuItem
            // 
            this.feedbackToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.feedbackToolStripMenuItem.Image = global::DiversityWorkbench.Properties.Resources.Feedback;
            this.feedbackToolStripMenuItem.Name = "feedbackToolStripMenuItem";
            this.feedbackToolStripMenuItem.Size = new System.Drawing.Size(28, 20);
            this.feedbackToolStripMenuItem.ToolTipText = "Send a feedback to the developer";
            this.feedbackToolStripMenuItem.Click += new System.EventHandler(this.feedbackToolStripMenuItem_Click);
            // 
            // entityTableAdapter
            // 
            this.entityTableAdapter.ClearBeforeFill = true;
            // 
            // entityContext_EnumTableAdapter
            // 
            this.entityContext_EnumTableAdapter.ClearBeforeFill = true;
            // 
            // entityLanguageCode_EnumTableAdapter
            // 
            this.entityLanguageCode_EnumTableAdapter.ClearBeforeFill = true;
            // 
            // entityListTableAdapter
            // 
            this.entityListTableAdapter.ClearBeforeFill = true;
            // 
            // entityUsageTableAdapter
            // 
            this.entityUsageTableAdapter.ClearBeforeFill = true;
            // 
            // entityRepresentationTableAdapter
            // 
            this.entityRepresentationTableAdapter.ClearBeforeFill = true;
            // 
            // entityAccessibility_EnumTableAdapter
            // 
            this.entityAccessibility_EnumTableAdapter.ClearBeforeFill = true;
            // 
            // entityDetermination_EnumTableAdapter
            // 
            this.entityDetermination_EnumTableAdapter.ClearBeforeFill = true;
            // 
            // entityVisibility_EnumTableAdapter
            // 
            this.entityVisibility_EnumTableAdapter.ClearBeforeFill = true;
            // 
            // FormApplicationEntity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(901, 595);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.menuStripMain);
            this.helpProvider.SetHelpKeyword(this, "Entity");
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "FormApplicationEntity";
            this.helpProvider.SetShowHelp(this, true);
            this.Text = "Descriptions of the entities in the application";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormApplicationEntity_FormClosing);
            this.Load += new System.EventHandler(this.FormApplicationEntity_Load);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerData.Panel1.ResumeLayout(false);
            this.splitContainerData.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerData)).EndInit();
            this.splitContainerData.ResumeLayout(false);
            this.tableLayoutPanelEntity.ResumeLayout(false);
            this.tableLayoutPanelEntity.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.entityBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetEntity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.entityListBindingSource)).EndInit();
            this.splitContainerDataDetails.Panel1.ResumeLayout(false);
            this.splitContainerDataDetails.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDataDetails)).EndInit();
            this.splitContainerDataDetails.ResumeLayout(false);
            this.tableLayoutPanelUsage.ResumeLayout(false);
            this.tableLayoutPanelUsage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.entityContextEnumBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.entityAccessibilityEnumBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.entityDeterminationEnumBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.entityVisibilityEnumBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.entityUsageBindingSource)).EndInit();
            this.tableLayoutPanelRepresentation.ResumeLayout(false);
            this.tableLayoutPanelRepresentation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRepresentation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.entityLanguageCodeEnumBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.entityRepresentationBindingSource)).EndInit();
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.SplitContainer splitContainerData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEntity;
        private System.Windows.Forms.SplitContainer splitContainerDataDetails;
        private System.Windows.Forms.Label labelEntityGroup;
        private System.Windows.Forms.Label labelEntityNotes;
        private System.Windows.Forms.TextBox textBoxEntityNotes;
        private System.Windows.Forms.DataGridView dataGridViewRepresentation;
        private System.Windows.Forms.DataGridViewComboBoxColumn workbenchContextDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn workbenchEntityUsageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn notesDataGridViewTextBoxColumn;
        //private DiversityWorkbench.DataSetEntityTableAdapters.WorkbenchISO_Language_EnumTableAdapter workbenchISO_Language_EnumTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn workbenchEntityDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewComboBoxColumn languageCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn workbenchContextDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayTextDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn abbreviationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn notesDataGridViewTextBoxColumn1;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonUsageNew;
        private System.Windows.Forms.Label labelUsage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelUsage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRepresentation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonRepresentationNew;
        private UserControls.UserControlQueryList userControlQueryList;
        private System.Windows.Forms.BindingSource entityBindingSource;
        private DiversityWorkbench.Datasets.DataSetEntityTableAdapters.EntityTableAdapter entityTableAdapter;
        private System.Windows.Forms.BindingSource entityContextEnumBindingSource;
        private DiversityWorkbench.Datasets.DataSetEntityTableAdapters.EntityContext_EnumTableAdapter entityContext_EnumTableAdapter;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertAllMissingTablesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertMissingColumnsForAllTablesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertMissingColumnsForSelectedTableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertPKForSelectedTableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem representationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertMissingLanguageEntriesForAllEntitiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateDescriptionsAccordingToDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem administrationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usageToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBoxObsolete;
        private System.Windows.Forms.DataGridViewComboBoxColumn entityUsageDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource entityLanguageCodeEnumBindingSource;
        private DiversityWorkbench.Datasets.DataSetEntityTableAdapters.EntityLanguageCode_EnumTableAdapter entityLanguageCode_EnumTableAdapter;
        private System.Windows.Forms.ToolStripMenuItem usageToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem insertUsageForTablesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertUsageForColumnsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateDatabaseAccordingToDescriptionToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBoxDisplayGroup;
        private System.Windows.Forms.BindingSource entityListBindingSource;
        private DiversityWorkbench.Datasets.DataSetEntityTableAdapters.EntityListTableAdapter entityListTableAdapter;
        private System.Windows.Forms.TextBox textBoxEntity;
        private DiversityWorkbench.Datasets.DataSetEntity dataSetEntity;
        private System.Windows.Forms.DataGridView dataGridViewUsage;
        private System.Windows.Forms.BindingSource entityUsageBindingSource;
        private DiversityWorkbench.Datasets.DataSetEntityTableAdapters.EntityUsageTableAdapter entityUsageTableAdapter;
        private System.Windows.Forms.BindingSource entityRepresentationBindingSource;
        private DiversityWorkbench.Datasets.DataSetEntityTableAdapters.EntityRepresentationTableAdapter entityRepresentationTableAdapter;
        private System.Windows.Forms.BindingSource entityAccessibilityEnumBindingSource;
        private DiversityWorkbench.Datasets.DataSetEntityTableAdapters.EntityAccessibility_EnumTableAdapter entityAccessibility_EnumTableAdapter;
        private System.Windows.Forms.BindingSource entityDeterminationEnumBindingSource;
        private DiversityWorkbench.Datasets.DataSetEntityTableAdapters.EntityDetermination_EnumTableAdapter entityDetermination_EnumTableAdapter;
        private System.Windows.Forms.BindingSource entityVisibilityEnumBindingSource;
        private DiversityWorkbench.Datasets.DataSetEntityTableAdapters.EntityVisibility_EnumTableAdapter entityVisibility_EnumTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn entityDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewComboBoxColumn entityContextDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.ToolStripMenuItem accessibilityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem determinationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem visibilityToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn entityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn entityContextDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn accessibilityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn determinationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn visibilityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn presetValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.ToolStripMenuItem feedbackToolStripMenuItem;
    }
}