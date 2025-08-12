namespace DiversityCollection.Forms
{
    partial class FormExportTaxa
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExportTaxa));
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageSource = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelSource = new System.Windows.Forms.TableLayoutPanel();
            this.labelSource = new System.Windows.Forms.Label();
            this.tabControlSource = new System.Windows.Forms.TabControl();
            this.tabPageSourceProjects = new System.Windows.Forms.TabPage();
            this.listBoxSourceProjects = new System.Windows.Forms.ListBox();
            this.toolStripSourceProjects = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSourceProjectAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSourceProjectRemove = new System.Windows.Forms.ToolStripButton();
            this.tabPageSourceFile = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelSourceFile = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSourceFileOpen = new System.Windows.Forms.Button();
            this.textBoxSourceFile = new System.Windows.Forms.TextBox();
            this.dataGridViewSourceFile = new System.Windows.Forms.DataGridView();
            this.toolStripSourceFile = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSourceFileAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButtonSourceFileTransferSetting = new System.Windows.Forms.ToolStripDropDownButton();
            this.SourceFileTransferLinkedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SourceFileTransferSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SourceFileTransferAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBoxSourceFileDataSource = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripComboBoxSourceFileProject = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripComboBoxSourceFileParser = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButtonSourceFileLinkToDiversityTaxonNames = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSourceFileEditLine = new System.Windows.Forms.ToolStripButton();
            this.comboBoxSourceFileEncoding = new System.Windows.Forms.ComboBox();
            this.labelSourceFileEncoding = new System.Windows.Forms.Label();
            this.checkBoxSourceFileFirstLine = new System.Windows.Forms.CheckBox();
            this.tabPageSourceTable = new System.Windows.Forms.TabPage();
            this.tabPageSourceTaxonNames = new System.Windows.Forms.TabPage();
            this.imageListTab = new System.Windows.Forms.ImageList(this.components);
            this.tabPageHierarchy = new System.Windows.Forms.TabPage();
            this.tabPageSynonyms = new System.Windows.Forms.TabPage();
            this.tabPageInfos = new System.Windows.Forms.TabPage();
            this.tabPageExport = new System.Windows.Forms.TabPage();
            this.listBoxTaxa = new System.Windows.Forms.ListBox();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.labelTaxonList = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buttonClearTaxonList = new System.Windows.Forms.Button();
            this.tabControlMain.SuspendLayout();
            this.tabPageSource.SuspendLayout();
            this.tableLayoutPanelSource.SuspendLayout();
            this.tabControlSource.SuspendLayout();
            this.tabPageSourceProjects.SuspendLayout();
            this.toolStripSourceProjects.SuspendLayout();
            this.tabPageSourceFile.SuspendLayout();
            this.tableLayoutPanelSourceFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSourceFile)).BeginInit();
            this.toolStripSourceFile.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageSource);
            this.tabControlMain.Controls.Add(this.tabPageHierarchy);
            this.tabControlMain.Controls.Add(this.tabPageSynonyms);
            this.tabControlMain.Controls.Add(this.tabPageInfos);
            this.tabControlMain.Controls.Add(this.tabPageExport);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.ImageList = this.imageListTab;
            this.tabControlMain.Location = new System.Drawing.Point(229, 34);
            this.tabControlMain.Name = "tabControlMain";
            this.tableLayoutPanelMain.SetRowSpan(this.tabControlMain, 2);
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(651, 537);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageSource
            // 
            this.tabPageSource.Controls.Add(this.tableLayoutPanelSource);
            this.tabPageSource.ImageKey = "Database.ico";
            this.tabPageSource.Location = new System.Drawing.Point(4, 23);
            this.tabPageSource.Name = "tabPageSource";
            this.tabPageSource.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSource.Size = new System.Drawing.Size(643, 510);
            this.tabPageSource.TabIndex = 0;
            this.tabPageSource.Text = "Source";
            this.tabPageSource.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelSource
            // 
            this.tableLayoutPanelSource.ColumnCount = 1;
            this.tableLayoutPanelSource.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSource.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelSource.Controls.Add(this.labelSource, 0, 0);
            this.tableLayoutPanelSource.Controls.Add(this.tabControlSource, 0, 1);
            this.tableLayoutPanelSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSource.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelSource.Name = "tableLayoutPanelSource";
            this.tableLayoutPanelSource.RowCount = 2;
            this.tableLayoutPanelSource.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSource.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSource.Size = new System.Drawing.Size(637, 504);
            this.tableLayoutPanelSource.TabIndex = 0;
            // 
            // labelSource
            // 
            this.labelSource.AutoSize = true;
            this.labelSource.Location = new System.Drawing.Point(9, 9);
            this.labelSource.Margin = new System.Windows.Forms.Padding(9);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(186, 13);
            this.labelSource.TabIndex = 0;
            this.labelSource.Text = "Please choose the source for the taxa";
            // 
            // tabControlSource
            // 
            this.tabControlSource.Controls.Add(this.tabPageSourceProjects);
            this.tabControlSource.Controls.Add(this.tabPageSourceFile);
            this.tabControlSource.Controls.Add(this.tabPageSourceTable);
            this.tabControlSource.Controls.Add(this.tabPageSourceTaxonNames);
            this.tabControlSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlSource.ImageList = this.imageListTab;
            this.tabControlSource.Location = new System.Drawing.Point(3, 34);
            this.tabControlSource.Name = "tabControlSource";
            this.tabControlSource.SelectedIndex = 0;
            this.tabControlSource.Size = new System.Drawing.Size(631, 467);
            this.tabControlSource.TabIndex = 1;
            // 
            // tabPageSourceProjects
            // 
            this.tabPageSourceProjects.Controls.Add(this.listBoxSourceProjects);
            this.tabPageSourceProjects.Controls.Add(this.toolStripSourceProjects);
            this.tabPageSourceProjects.ImageKey = "Project.ico";
            this.tabPageSourceProjects.Location = new System.Drawing.Point(4, 23);
            this.tabPageSourceProjects.Name = "tabPageSourceProjects";
            this.tabPageSourceProjects.Size = new System.Drawing.Size(623, 440);
            this.tabPageSourceProjects.TabIndex = 3;
            this.tabPageSourceProjects.Text = "Included projects";
            this.tabPageSourceProjects.UseVisualStyleBackColor = true;
            // 
            // listBoxSourceProjects
            // 
            this.listBoxSourceProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxSourceProjects.FormattingEnabled = true;
            this.listBoxSourceProjects.Location = new System.Drawing.Point(0, 0);
            this.listBoxSourceProjects.Name = "listBoxSourceProjects";
            this.listBoxSourceProjects.Size = new System.Drawing.Size(623, 415);
            this.listBoxSourceProjects.TabIndex = 1;
            // 
            // toolStripSourceProjects
            // 
            this.toolStripSourceProjects.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripSourceProjects.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripSourceProjects.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSourceProjectAdd,
            this.toolStripButtonSourceProjectRemove});
            this.toolStripSourceProjects.Location = new System.Drawing.Point(0, 415);
            this.toolStripSourceProjects.Name = "toolStripSourceProjects";
            this.toolStripSourceProjects.Size = new System.Drawing.Size(623, 25);
            this.toolStripSourceProjects.TabIndex = 0;
            this.toolStripSourceProjects.Text = "toolStrip1";
            // 
            // toolStripButtonSourceProjectAdd
            // 
            this.toolStripButtonSourceProjectAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSourceProjectAdd.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonSourceProjectAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSourceProjectAdd.Name = "toolStripButtonSourceProjectAdd";
            this.toolStripButtonSourceProjectAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSourceProjectAdd.Text = "Add a project";
            this.toolStripButtonSourceProjectAdd.Click += new System.EventHandler(this.toolStripButtonSourceProjectAdd_Click);
            // 
            // toolStripButtonSourceProjectRemove
            // 
            this.toolStripButtonSourceProjectRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSourceProjectRemove.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonSourceProjectRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSourceProjectRemove.Name = "toolStripButtonSourceProjectRemove";
            this.toolStripButtonSourceProjectRemove.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSourceProjectRemove.Text = "Delete the selected project";
            this.toolStripButtonSourceProjectRemove.Click += new System.EventHandler(this.toolStripButtonSourceProjectRemove_Click);
            // 
            // tabPageSourceFile
            // 
            this.tabPageSourceFile.Controls.Add(this.tableLayoutPanelSourceFile);
            this.tabPageSourceFile.ImageKey = "List.ico";
            this.tabPageSourceFile.Location = new System.Drawing.Point(4, 23);
            this.tabPageSourceFile.Name = "tabPageSourceFile";
            this.tabPageSourceFile.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSourceFile.Size = new System.Drawing.Size(623, 440);
            this.tabPageSourceFile.TabIndex = 0;
            this.tabPageSourceFile.Text = "List from file";
            this.tabPageSourceFile.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelSourceFile
            // 
            this.tableLayoutPanelSourceFile.ColumnCount = 4;
            this.tableLayoutPanelSourceFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSourceFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSourceFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSourceFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSourceFile.Controls.Add(this.buttonSourceFileOpen, 0, 0);
            this.tableLayoutPanelSourceFile.Controls.Add(this.textBoxSourceFile, 0, 1);
            this.tableLayoutPanelSourceFile.Controls.Add(this.dataGridViewSourceFile, 0, 2);
            this.tableLayoutPanelSourceFile.Controls.Add(this.toolStripSourceFile, 0, 3);
            this.tableLayoutPanelSourceFile.Controls.Add(this.comboBoxSourceFileEncoding, 3, 0);
            this.tableLayoutPanelSourceFile.Controls.Add(this.labelSourceFileEncoding, 2, 0);
            this.tableLayoutPanelSourceFile.Controls.Add(this.checkBoxSourceFileFirstLine, 1, 0);
            this.tableLayoutPanelSourceFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSourceFile.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelSourceFile.Name = "tableLayoutPanelSourceFile";
            this.tableLayoutPanelSourceFile.RowCount = 4;
            this.tableLayoutPanelSourceFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSourceFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSourceFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSourceFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSourceFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelSourceFile.Size = new System.Drawing.Size(617, 434);
            this.tableLayoutPanelSourceFile.TabIndex = 0;
            // 
            // buttonSourceFileOpen
            // 
            this.buttonSourceFileOpen.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonSourceFileOpen.Image = global::DiversityCollection.Resource.Folder;
            this.buttonSourceFileOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSourceFileOpen.Location = new System.Drawing.Point(3, 3);
            this.buttonSourceFileOpen.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonSourceFileOpen.Name = "buttonSourceFileOpen";
            this.buttonSourceFileOpen.Size = new System.Drawing.Size(115, 23);
            this.buttonSourceFileOpen.TabIndex = 0;
            this.buttonSourceFileOpen.Text = "Select source file";
            this.buttonSourceFileOpen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSourceFileOpen.UseVisualStyleBackColor = true;
            this.buttonSourceFileOpen.Click += new System.EventHandler(this.buttonSourceFileOpen_Click);
            // 
            // textBoxSourceFile
            // 
            this.tableLayoutPanelSourceFile.SetColumnSpan(this.textBoxSourceFile, 4);
            this.textBoxSourceFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSourceFile.Location = new System.Drawing.Point(3, 30);
            this.textBoxSourceFile.Name = "textBoxSourceFile";
            this.textBoxSourceFile.ReadOnly = true;
            this.textBoxSourceFile.Size = new System.Drawing.Size(611, 20);
            this.textBoxSourceFile.TabIndex = 1;
            // 
            // dataGridViewSourceFile
            // 
            this.dataGridViewSourceFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanelSourceFile.SetColumnSpan(this.dataGridViewSourceFile, 4);
            this.dataGridViewSourceFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSourceFile.Location = new System.Drawing.Point(3, 56);
            this.dataGridViewSourceFile.Name = "dataGridViewSourceFile";
            this.dataGridViewSourceFile.Size = new System.Drawing.Size(611, 350);
            this.dataGridViewSourceFile.TabIndex = 2;
            // 
            // toolStripSourceFile
            // 
            this.tableLayoutPanelSourceFile.SetColumnSpan(this.toolStripSourceFile, 4);
            this.toolStripSourceFile.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripSourceFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSourceFileAdd,
            this.toolStripDropDownButtonSourceFileTransferSetting,
            this.toolStripComboBoxSourceFileDataSource,
            this.toolStripComboBoxSourceFileProject,
            this.toolStripComboBoxSourceFileParser,
            this.toolStripButtonSourceFileLinkToDiversityTaxonNames,
            this.toolStripButtonSourceFileEditLine});
            this.toolStripSourceFile.Location = new System.Drawing.Point(0, 409);
            this.toolStripSourceFile.Name = "toolStripSourceFile";
            this.toolStripSourceFile.Size = new System.Drawing.Size(617, 25);
            this.toolStripSourceFile.TabIndex = 3;
            this.toolStripSourceFile.Text = "toolStrip1";
            // 
            // toolStripButtonSourceFileAdd
            // 
            this.toolStripButtonSourceFileAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSourceFileAdd.Image = global::DiversityCollection.Resource.ArrowPrevious1;
            this.toolStripButtonSourceFileAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSourceFileAdd.Name = "toolStripButtonSourceFileAdd";
            this.toolStripButtonSourceFileAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSourceFileAdd.Text = "Add taxa to list according to setting";
            this.toolStripButtonSourceFileAdd.Click += new System.EventHandler(this.toolStripButtonSourceFileAdd_Click);
            // 
            // toolStripDropDownButtonSourceFileTransferSetting
            // 
            this.toolStripDropDownButtonSourceFileTransferSetting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonSourceFileTransferSetting.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SourceFileTransferLinkedToolStripMenuItem,
            this.SourceFileTransferSelectedToolStripMenuItem,
            this.SourceFileTransferAllToolStripMenuItem});
            this.toolStripDropDownButtonSourceFileTransferSetting.Image = global::DiversityCollection.Resource.DiversityWorkbench;
            this.toolStripDropDownButtonSourceFileTransferSetting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonSourceFileTransferSetting.Name = "toolStripDropDownButtonSourceFileTransferSetting";
            this.toolStripDropDownButtonSourceFileTransferSetting.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButtonSourceFileTransferSetting.Text = "Transfer only names linked to DiversityTaxonNames";
            // 
            // SourceFileTransferLinkedToolStripMenuItem
            // 
            this.SourceFileTransferLinkedToolStripMenuItem.Image = global::DiversityCollection.Resource.DiversityWorkbench;
            this.SourceFileTransferLinkedToolStripMenuItem.Name = "SourceFileTransferLinkedToolStripMenuItem";
            this.SourceFileTransferLinkedToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.SourceFileTransferLinkedToolStripMenuItem.Text = "Linked";
            this.SourceFileTransferLinkedToolStripMenuItem.ToolTipText = "Transfer only names linked to DiversityTaxonNames";
            this.SourceFileTransferLinkedToolStripMenuItem.Click += new System.EventHandler(this.SourceFileTransferLinkedToolStripMenuItem_Click);
            // 
            // SourceFileTransferSelectedToolStripMenuItem
            // 
            this.SourceFileTransferSelectedToolStripMenuItem.Image = global::DiversityCollection.Resource.SelectRows;
            this.SourceFileTransferSelectedToolStripMenuItem.Name = "SourceFileTransferSelectedToolStripMenuItem";
            this.SourceFileTransferSelectedToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.SourceFileTransferSelectedToolStripMenuItem.Text = "Selected";
            this.SourceFileTransferSelectedToolStripMenuItem.ToolTipText = "Transfer only the selected names in the list";
            this.SourceFileTransferSelectedToolStripMenuItem.Click += new System.EventHandler(this.SourceFileTransferSelectedToolStripMenuItem_Click);
            // 
            // SourceFileTransferAllToolStripMenuItem
            // 
            this.SourceFileTransferAllToolStripMenuItem.Image = global::DiversityCollection.Resource.MarkColumn;
            this.SourceFileTransferAllToolStripMenuItem.Name = "SourceFileTransferAllToolStripMenuItem";
            this.SourceFileTransferAllToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.SourceFileTransferAllToolStripMenuItem.Text = "All";
            this.SourceFileTransferAllToolStripMenuItem.ToolTipText = "Transfer all names in the list";
            this.SourceFileTransferAllToolStripMenuItem.Click += new System.EventHandler(this.SourceFileTransferAllToolStripMenuItem_Click);
            // 
            // toolStripComboBoxSourceFileDataSource
            // 
            this.toolStripComboBoxSourceFileDataSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxSourceFileDataSource.DropDownWidth = 400;
            this.toolStripComboBoxSourceFileDataSource.MaxDropDownItems = 20;
            this.toolStripComboBoxSourceFileDataSource.Name = "toolStripComboBoxSourceFileDataSource";
            this.toolStripComboBoxSourceFileDataSource.Size = new System.Drawing.Size(221, 25);
            this.toolStripComboBoxSourceFileDataSource.ToolTipText = "DiversityTaxonNames Database for comparision";
            this.toolStripComboBoxSourceFileDataSource.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxSourceFileDataSource_SelectedIndexChanged);
            // 
            // toolStripComboBoxSourceFileProject
            // 
            this.toolStripComboBoxSourceFileProject.DropDownWidth = 400;
            this.toolStripComboBoxSourceFileProject.Name = "toolStripComboBoxSourceFileProject";
            this.toolStripComboBoxSourceFileProject.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBoxSourceFileProject.ToolTipText = "The project in the DivesityTaxonNames database";
            this.toolStripComboBoxSourceFileProject.DropDown += new System.EventHandler(this.toolStripComboBoxSourceFileProject_DropDown);
            // 
            // toolStripComboBoxSourceFileParser
            // 
            this.toolStripComboBoxSourceFileParser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxSourceFileParser.Name = "toolStripComboBoxSourceFileParser";
            this.toolStripComboBoxSourceFileParser.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBoxSourceFileParser.ToolTipText = "The type of comparision with the names in the database";
            // 
            // toolStripButtonSourceFileLinkToDiversityTaxonNames
            // 
            this.toolStripButtonSourceFileLinkToDiversityTaxonNames.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSourceFileLinkToDiversityTaxonNames.Image = global::DiversityCollection.Resource.Translation;
            this.toolStripButtonSourceFileLinkToDiversityTaxonNames.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSourceFileLinkToDiversityTaxonNames.Name = "toolStripButtonSourceFileLinkToDiversityTaxonNames";
            this.toolStripButtonSourceFileLinkToDiversityTaxonNames.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSourceFileLinkToDiversityTaxonNames.Text = "Link to DiversityTaxonNames";
            this.toolStripButtonSourceFileLinkToDiversityTaxonNames.Click += new System.EventHandler(this.toolStripButtonSourceFileLinkToDiversityTaxonNames_Click);
            // 
            // toolStripButtonSourceFileEditLine
            // 
            this.toolStripButtonSourceFileEditLine.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonSourceFileEditLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSourceFileEditLine.Image = global::DiversityCollection.Resource.SelectRow;
            this.toolStripButtonSourceFileEditLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSourceFileEditLine.Name = "toolStripButtonSourceFileEditLine";
            this.toolStripButtonSourceFileEditLine.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSourceFileEditLine.Text = "Edit selected taxon";
            this.toolStripButtonSourceFileEditLine.Click += new System.EventHandler(this.toolStripButtonSourceFileEditLine_Click);
            // 
            // comboBoxSourceFileEncoding
            // 
            this.comboBoxSourceFileEncoding.FormattingEnabled = true;
            this.comboBoxSourceFileEncoding.Location = new System.Drawing.Point(534, 3);
            this.comboBoxSourceFileEncoding.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxSourceFileEncoding.Name = "comboBoxSourceFileEncoding";
            this.comboBoxSourceFileEncoding.Size = new System.Drawing.Size(80, 21);
            this.comboBoxSourceFileEncoding.TabIndex = 4;
            // 
            // labelSourceFileEncoding
            // 
            this.labelSourceFileEncoding.AutoSize = true;
            this.labelSourceFileEncoding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSourceFileEncoding.Location = new System.Drawing.Point(479, 0);
            this.labelSourceFileEncoding.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelSourceFileEncoding.Name = "labelSourceFileEncoding";
            this.labelSourceFileEncoding.Size = new System.Drawing.Size(55, 27);
            this.labelSourceFileEncoding.TabIndex = 5;
            this.labelSourceFileEncoding.Text = "Encoding:";
            this.labelSourceFileEncoding.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBoxSourceFileFirstLine
            // 
            this.checkBoxSourceFileFirstLine.AutoSize = true;
            this.checkBoxSourceFileFirstLine.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxSourceFileFirstLine.Location = new System.Drawing.Point(284, 3);
            this.checkBoxSourceFileFirstLine.Name = "checkBoxSourceFileFirstLine";
            this.checkBoxSourceFileFirstLine.Size = new System.Drawing.Size(189, 21);
            this.checkBoxSourceFileFirstLine.TabIndex = 6;
            this.checkBoxSourceFileFirstLine.Text = "First line contains column definition";
            this.checkBoxSourceFileFirstLine.UseVisualStyleBackColor = true;
            this.checkBoxSourceFileFirstLine.Click += new System.EventHandler(this.checkBoxSourceFileFirstLine_Click);
            // 
            // tabPageSourceTable
            // 
            this.tabPageSourceTable.ImageKey = "Speadsheet.ico";
            this.tabPageSourceTable.Location = new System.Drawing.Point(4, 23);
            this.tabPageSourceTable.Name = "tabPageSourceTable";
            this.tabPageSourceTable.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSourceTable.Size = new System.Drawing.Size(623, 440);
            this.tabPageSourceTable.TabIndex = 1;
            this.tabPageSourceTable.Text = "Data in table Identification";
            this.tabPageSourceTable.UseVisualStyleBackColor = true;
            // 
            // tabPageSourceTaxonNames
            // 
            this.tabPageSourceTaxonNames.ImageKey = "Workbench.ico";
            this.tabPageSourceTaxonNames.Location = new System.Drawing.Point(4, 23);
            this.tabPageSourceTaxonNames.Name = "tabPageSourceTaxonNames";
            this.tabPageSourceTaxonNames.Size = new System.Drawing.Size(623, 440);
            this.tabPageSourceTaxonNames.TabIndex = 2;
            this.tabPageSourceTaxonNames.Text = "DiversityTaxonNames";
            this.tabPageSourceTaxonNames.UseVisualStyleBackColor = true;
            // 
            // imageListTab
            // 
            this.imageListTab.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTab.ImageStream")));
            this.imageListTab.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTab.Images.SetKeyName(0, "Database.ico");
            this.imageListTab.Images.SetKeyName(1, "Hierarchy.ico");
            this.imageListTab.Images.SetKeyName(2, "Synonyms.ico");
            this.imageListTab.Images.SetKeyName(3, "Schema.ico");
            this.imageListTab.Images.SetKeyName(4, "Export.ico");
            this.imageListTab.Images.SetKeyName(5, "List.ico");
            this.imageListTab.Images.SetKeyName(6, "Speadsheet.ico");
            this.imageListTab.Images.SetKeyName(7, "Workbench.ico");
            this.imageListTab.Images.SetKeyName(8, "Project.ico");
            // 
            // tabPageHierarchy
            // 
            this.tabPageHierarchy.ImageKey = "Hierarchy.ico";
            this.tabPageHierarchy.Location = new System.Drawing.Point(4, 23);
            this.tabPageHierarchy.Name = "tabPageHierarchy";
            this.tabPageHierarchy.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHierarchy.Size = new System.Drawing.Size(643, 510);
            this.tabPageHierarchy.TabIndex = 1;
            this.tabPageHierarchy.Text = "Hierarchy";
            this.tabPageHierarchy.UseVisualStyleBackColor = true;
            // 
            // tabPageSynonyms
            // 
            this.tabPageSynonyms.ImageKey = "Synonyms.ico";
            this.tabPageSynonyms.Location = new System.Drawing.Point(4, 23);
            this.tabPageSynonyms.Name = "tabPageSynonyms";
            this.tabPageSynonyms.Size = new System.Drawing.Size(643, 510);
            this.tabPageSynonyms.TabIndex = 2;
            this.tabPageSynonyms.Text = "Synonyms";
            this.tabPageSynonyms.UseVisualStyleBackColor = true;
            // 
            // tabPageInfos
            // 
            this.tabPageInfos.ImageKey = "Schema.ico";
            this.tabPageInfos.Location = new System.Drawing.Point(4, 23);
            this.tabPageInfos.Name = "tabPageInfos";
            this.tabPageInfos.Size = new System.Drawing.Size(643, 510);
            this.tabPageInfos.TabIndex = 3;
            this.tabPageInfos.Text = "Linked information";
            this.tabPageInfos.UseVisualStyleBackColor = true;
            // 
            // tabPageExport
            // 
            this.tabPageExport.ImageKey = "Export.ico";
            this.tabPageExport.Location = new System.Drawing.Point(4, 23);
            this.tabPageExport.Name = "tabPageExport";
            this.tabPageExport.Size = new System.Drawing.Size(643, 510);
            this.tabPageExport.TabIndex = 4;
            this.tabPageExport.Text = "Export";
            this.tabPageExport.UseVisualStyleBackColor = true;
            // 
            // listBoxTaxa
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.listBoxTaxa, 2);
            this.listBoxTaxa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxTaxa.FormattingEnabled = true;
            this.listBoxTaxa.IntegralHeight = false;
            this.listBoxTaxa.Location = new System.Drawing.Point(3, 50);
            this.listBoxTaxa.Name = "listBoxTaxa";
            this.listBoxTaxa.Size = new System.Drawing.Size(220, 521);
            this.listBoxTaxa.TabIndex = 2;
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 3;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.tabControlMain, 2, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.listBoxTaxa, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelTaxonList, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonClearTaxonList, 1, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(883, 574);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelHeader, 3);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(9, 9);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(9);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(865, 13);
            this.labelHeader.TabIndex = 1;
            this.labelHeader.Text = "Export of taxa and linked informations";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTaxonList
            // 
            this.labelTaxonList.AutoSize = true;
            this.labelTaxonList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTaxonList.Location = new System.Drawing.Point(3, 31);
            this.labelTaxonList.Name = "labelTaxonList";
            this.labelTaxonList.Size = new System.Drawing.Size(200, 16);
            this.labelTaxonList.TabIndex = 2;
            this.labelTaxonList.Text = "List of taxa";
            this.labelTaxonList.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // buttonClearTaxonList
            // 
            this.buttonClearTaxonList.FlatAppearance.BorderSize = 0;
            this.buttonClearTaxonList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClearTaxonList.Image = global::DiversityCollection.Resource.Delete;
            this.buttonClearTaxonList.Location = new System.Drawing.Point(206, 31);
            this.buttonClearTaxonList.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClearTaxonList.Name = "buttonClearTaxonList";
            this.buttonClearTaxonList.Size = new System.Drawing.Size(20, 16);
            this.buttonClearTaxonList.TabIndex = 3;
            this.toolTip.SetToolTip(this.buttonClearTaxonList, "Clear taxon list");
            this.buttonClearTaxonList.UseVisualStyleBackColor = true;
            // 
            // FormExportTaxa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 574);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormExportTaxa";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export Taxa";
            this.tabControlMain.ResumeLayout(false);
            this.tabPageSource.ResumeLayout(false);
            this.tableLayoutPanelSource.ResumeLayout(false);
            this.tableLayoutPanelSource.PerformLayout();
            this.tabControlSource.ResumeLayout(false);
            this.tabPageSourceProjects.ResumeLayout(false);
            this.tabPageSourceProjects.PerformLayout();
            this.toolStripSourceProjects.ResumeLayout(false);
            this.toolStripSourceProjects.PerformLayout();
            this.tabPageSourceFile.ResumeLayout(false);
            this.tableLayoutPanelSourceFile.ResumeLayout(false);
            this.tableLayoutPanelSourceFile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSourceFile)).EndInit();
            this.toolStripSourceFile.ResumeLayout(false);
            this.toolStripSourceFile.PerformLayout();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageSource;
        private System.Windows.Forms.TabPage tabPageHierarchy;
        private System.Windows.Forms.TabPage tabPageSynonyms;
        private System.Windows.Forms.TabPage tabPageInfos;
        private System.Windows.Forms.TabPage tabPageExport;
        private System.Windows.Forms.ImageList imageListTab;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSource;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.TabControl tabControlSource;
        private System.Windows.Forms.TabPage tabPageSourceFile;
        private System.Windows.Forms.TabPage tabPageSourceTable;
        private System.Windows.Forms.TabPage tabPageSourceTaxonNames;
        private System.Windows.Forms.ListBox listBoxTaxa;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSourceFile;
        private System.Windows.Forms.TabPage tabPageSourceProjects;
        private System.Windows.Forms.ListBox listBoxSourceProjects;
        private System.Windows.Forms.ToolStrip toolStripSourceProjects;
        private System.Windows.Forms.ToolStripButton toolStripButtonSourceProjectAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonSourceProjectRemove;
        private System.Windows.Forms.Button buttonSourceFileOpen;
        private System.Windows.Forms.TextBox textBoxSourceFile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Label labelTaxonList;
        private System.Windows.Forms.DataGridView dataGridViewSourceFile;
        private System.Windows.Forms.ToolStrip toolStripSourceFile;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxSourceFileDataSource;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxSourceFileProject;
        private System.Windows.Forms.ToolStripButton toolStripButtonSourceFileLinkToDiversityTaxonNames;
        private System.Windows.Forms.ToolStripButton toolStripButtonSourceFileEditLine;
        private System.Windows.Forms.ToolStripButton toolStripButtonSourceFileAdd;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonSourceFileTransferSetting;
        private System.Windows.Forms.ToolStripMenuItem SourceFileTransferAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SourceFileTransferSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SourceFileTransferLinkedToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ComboBox comboBoxSourceFileEncoding;
        private System.Windows.Forms.Label labelSourceFileEncoding;
        private System.Windows.Forms.CheckBox checkBoxSourceFileFirstLine;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxSourceFileParser;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonClearTaxonList;
    }
}