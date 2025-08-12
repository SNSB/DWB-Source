namespace DiversityCollection.Forms
{
    partial class FormLabelEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLabelEditor));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelLabel = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainerDesigner = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelDesigner = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelRowColumnEditor = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripRows = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonRowAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRowRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripColumns = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonColumnAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonColumnRemove = new System.Windows.Forms.ToolStripButton();
            this.panelRows = new System.Windows.Forms.Panel();
            this.panelColumns = new System.Windows.Forms.Panel();
            this.tabControlDesigner = new System.Windows.Forms.TabControl();
            this.tabPageDetail = new System.Windows.Forms.TabPage();
            this.tabPageFonts = new System.Windows.Forms.TabPage();
            this.splitContainerFonts = new System.Windows.Forms.SplitContainer();
            this.listBoxFonts = new System.Windows.Forms.ListBox();
            this.tabPagePage = new System.Windows.Forms.TabPage();
            this.tabPageTemplates = new System.Windows.Forms.TabPage();
            this.splitContainerTemplates = new System.Windows.Forms.SplitContainer();
            this.listBoxTemplates = new System.Windows.Forms.ListBox();
            this.tabPageRows = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelRowOptions = new System.Windows.Forms.TableLayoutPanel();
            this.labelRowOptionsHeader = new System.Windows.Forms.Label();
            this.labelRowOptionsAttributes = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.toolStripLabel = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNewSchemaFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonOpenSchemaFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorLabel = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonLabelPreview = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLabelMulti = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBoxPrintDuplicates = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparatorPrint = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonLabelPrintBrowserOptions = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPageSetup = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLabelExport = new System.Windows.Forms.ToolStripButton();
            this.textBoxSchemaFile = new System.Windows.Forms.TextBox();
            this.labelSchemaFile = new System.Windows.Forms.Label();
            this.textBoxReportTitle = new System.Windows.Forms.TextBox();
            this.labelReportTitle = new System.Windows.Forms.Label();
            this.checkBoxUseStockForLabelDuplicates = new System.Windows.Forms.CheckBox();
            this.checkBoxPrintRestrictToMaterial = new System.Windows.Forms.CheckBox();
            this.checkBoxPrintRestrictToCollection = new System.Windows.Forms.CheckBox();
            this.pictureBoxPrintRestrictToMaterial = new System.Windows.Forms.PictureBox();
            this.labelLabelConversion = new System.Windows.Forms.Label();
            this.comboBoxLabelConversion = new System.Windows.Forms.ComboBox();
            this.labelHeader = new System.Windows.Forms.Label();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.webBrowserLabel = new System.Windows.Forms.WebBrowser();
            this.openFileDialogLabelSchema = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tableLayoutPanelLabel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDesigner)).BeginInit();
            this.splitContainerDesigner.Panel1.SuspendLayout();
            this.splitContainerDesigner.Panel2.SuspendLayout();
            this.splitContainerDesigner.SuspendLayout();
            this.tableLayoutPanelDesigner.SuspendLayout();
            this.toolStripRows.SuspendLayout();
            this.toolStripColumns.SuspendLayout();
            this.tabControlDesigner.SuspendLayout();
            this.tabPageFonts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerFonts)).BeginInit();
            this.splitContainerFonts.Panel1.SuspendLayout();
            this.splitContainerFonts.SuspendLayout();
            this.tabPageTemplates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTemplates)).BeginInit();
            this.splitContainerTemplates.Panel1.SuspendLayout();
            this.splitContainerTemplates.SuspendLayout();
            this.tabPageRows.SuspendLayout();
            this.tableLayoutPanelRowOptions.SuspendLayout();
            this.toolStripLabel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPrintRestrictToMaterial)).BeginInit();
            this.SuspendLayout();
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
            this.splitContainerMain.Panel1.Controls.Add(this.tableLayoutPanelLabel);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.webBrowserLabel);
            this.splitContainerMain.Size = new System.Drawing.Size(864, 589);
            this.splitContainerMain.SplitterDistance = 381;
            this.splitContainerMain.SplitterWidth = 8;
            this.splitContainerMain.TabIndex = 0;
            // 
            // tableLayoutPanelLabel
            // 
            this.tableLayoutPanelLabel.ColumnCount = 5;
            this.tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanelLabel.Controls.Add(this.splitContainerDesigner, 0, 1);
            this.tableLayoutPanelLabel.Controls.Add(this.toolStripLabel, 3, 2);
            this.tableLayoutPanelLabel.Controls.Add(this.textBoxSchemaFile, 1, 2);
            this.tableLayoutPanelLabel.Controls.Add(this.labelSchemaFile, 0, 2);
            this.tableLayoutPanelLabel.Controls.Add(this.textBoxReportTitle, 1, 3);
            this.tableLayoutPanelLabel.Controls.Add(this.labelReportTitle, 0, 3);
            this.tableLayoutPanelLabel.Controls.Add(this.checkBoxUseStockForLabelDuplicates, 3, 3);
            this.tableLayoutPanelLabel.Controls.Add(this.checkBoxPrintRestrictToMaterial, 3, 4);
            this.tableLayoutPanelLabel.Controls.Add(this.checkBoxPrintRestrictToCollection, 2, 4);
            this.tableLayoutPanelLabel.Controls.Add(this.pictureBoxPrintRestrictToMaterial, 4, 4);
            this.tableLayoutPanelLabel.Controls.Add(this.labelLabelConversion, 0, 4);
            this.tableLayoutPanelLabel.Controls.Add(this.comboBoxLabelConversion, 1, 4);
            this.tableLayoutPanelLabel.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelLabel.Controls.Add(this.buttonRefresh, 3, 0);
            this.tableLayoutPanelLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelLabel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelLabel.Name = "tableLayoutPanelLabel";
            this.tableLayoutPanelLabel.RowCount = 5;
            this.tableLayoutPanelLabel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLabel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLabel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanelLabel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLabel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLabel.Size = new System.Drawing.Size(864, 381);
            this.tableLayoutPanelLabel.TabIndex = 2;
            // 
            // splitContainerDesigner
            // 
            this.tableLayoutPanelLabel.SetColumnSpan(this.splitContainerDesigner, 5);
            this.splitContainerDesigner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerDesigner.Location = new System.Drawing.Point(3, 27);
            this.splitContainerDesigner.Name = "splitContainerDesigner";
            // 
            // splitContainerDesigner.Panel1
            // 
            this.splitContainerDesigner.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerDesigner.Panel1.Controls.Add(this.tableLayoutPanelDesigner);
            // 
            // splitContainerDesigner.Panel2
            // 
            this.splitContainerDesigner.Panel2.Controls.Add(this.tabControlDesigner);
            this.splitContainerDesigner.Panel2.Controls.Add(this.button1);
            this.splitContainerDesigner.Size = new System.Drawing.Size(858, 272);
            this.splitContainerDesigner.SplitterDistance = 571;
            this.splitContainerDesigner.TabIndex = 0;
            // 
            // tableLayoutPanelDesigner
            // 
            this.tableLayoutPanelDesigner.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelDesigner.ColumnCount = 3;
            this.tableLayoutPanelDesigner.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDesigner.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanelDesigner.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDesigner.Controls.Add(this.tableLayoutPanelRowColumnEditor, 2, 2);
            this.tableLayoutPanelDesigner.Controls.Add(this.toolStripRows, 0, 2);
            this.tableLayoutPanelDesigner.Controls.Add(this.toolStripColumns, 2, 0);
            this.tableLayoutPanelDesigner.Controls.Add(this.panelRows, 1, 2);
            this.tableLayoutPanelDesigner.Controls.Add(this.panelColumns, 2, 1);
            this.tableLayoutPanelDesigner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDesigner.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelDesigner.Name = "tableLayoutPanelDesigner";
            this.tableLayoutPanelDesigner.RowCount = 3;
            this.tableLayoutPanelDesigner.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDesigner.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelDesigner.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDesigner.Size = new System.Drawing.Size(571, 272);
            this.tableLayoutPanelDesigner.TabIndex = 0;
            // 
            // tableLayoutPanelRowColumnEditor
            // 
            this.tableLayoutPanelRowColumnEditor.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanelRowColumnEditor.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanelRowColumnEditor.ColumnCount = 3;
            this.tableLayoutPanelRowColumnEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRowColumnEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRowColumnEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRowColumnEditor.Location = new System.Drawing.Point(64, 45);
            this.tableLayoutPanelRowColumnEditor.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelRowColumnEditor.Name = "tableLayoutPanelRowColumnEditor";
            this.tableLayoutPanelRowColumnEditor.RowCount = 3;
            this.tableLayoutPanelRowColumnEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRowColumnEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRowColumnEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRowColumnEditor.Size = new System.Drawing.Size(178, 111);
            this.tableLayoutPanelRowColumnEditor.TabIndex = 3;
            // 
            // toolStripRows
            // 
            this.toolStripRows.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStripRows.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripRows.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonRowAdd,
            this.toolStripButtonRowRemove});
            this.toolStripRows.Location = new System.Drawing.Point(0, 45);
            this.toolStripRows.Name = "toolStripRows";
            this.toolStripRows.Size = new System.Drawing.Size(24, 227);
            this.toolStripRows.TabIndex = 2;
            this.toolStripRows.Text = "toolStrip1";
            // 
            // toolStripButtonRowAdd
            // 
            this.toolStripButtonRowAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRowAdd.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonRowAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRowAdd.Name = "toolStripButtonRowAdd";
            this.toolStripButtonRowAdd.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonRowAdd.Text = "Add a new row";
            this.toolStripButtonRowAdd.Click += new System.EventHandler(this.toolStripButtonRowAdd_Click);
            // 
            // toolStripButtonRowRemove
            // 
            this.toolStripButtonRowRemove.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonRowRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRowRemove.Image = global::DiversityCollection.Resource.Remove;
            this.toolStripButtonRowRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRowRemove.Name = "toolStripButtonRowRemove";
            this.toolStripButtonRowRemove.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonRowRemove.Text = "Remove row";
            this.toolStripButtonRowRemove.Click += new System.EventHandler(this.toolStripButtonRowRemove_Click);
            // 
            // toolStripColumns
            // 
            this.toolStripColumns.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripColumns.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonColumnAdd,
            this.toolStripButtonColumnRemove});
            this.toolStripColumns.Location = new System.Drawing.Point(64, 0);
            this.toolStripColumns.Name = "toolStripColumns";
            this.toolStripColumns.Size = new System.Drawing.Size(507, 25);
            this.toolStripColumns.TabIndex = 1;
            this.toolStripColumns.Text = "toolStripColumns";
            // 
            // toolStripButtonColumnAdd
            // 
            this.toolStripButtonColumnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonColumnAdd.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonColumnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonColumnAdd.Name = "toolStripButtonColumnAdd";
            this.toolStripButtonColumnAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonColumnAdd.Text = "Add column";
            this.toolStripButtonColumnAdd.Click += new System.EventHandler(this.toolStripButtonColumnAdd_Click);
            // 
            // toolStripButtonColumnRemove
            // 
            this.toolStripButtonColumnRemove.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonColumnRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonColumnRemove.Image = global::DiversityCollection.Resource.Remove;
            this.toolStripButtonColumnRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonColumnRemove.Name = "toolStripButtonColumnRemove";
            this.toolStripButtonColumnRemove.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonColumnRemove.Text = "Remove column";
            this.toolStripButtonColumnRemove.Click += new System.EventHandler(this.toolStripButtonColumnRemove_Click);
            // 
            // panelRows
            // 
            this.panelRows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRows.Location = new System.Drawing.Point(24, 45);
            this.panelRows.Margin = new System.Windows.Forms.Padding(0);
            this.panelRows.Name = "panelRows";
            this.panelRows.Size = new System.Drawing.Size(40, 227);
            this.panelRows.TabIndex = 5;
            // 
            // panelColumns
            // 
            this.panelColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelColumns.Location = new System.Drawing.Point(64, 25);
            this.panelColumns.Margin = new System.Windows.Forms.Padding(0);
            this.panelColumns.Name = "panelColumns";
            this.panelColumns.Size = new System.Drawing.Size(507, 20);
            this.panelColumns.TabIndex = 6;
            // 
            // tabControlDesigner
            // 
            this.tabControlDesigner.Controls.Add(this.tabPageDetail);
            this.tabControlDesigner.Controls.Add(this.tabPageFonts);
            this.tabControlDesigner.Controls.Add(this.tabPagePage);
            this.tabControlDesigner.Controls.Add(this.tabPageTemplates);
            this.tabControlDesigner.Controls.Add(this.tabPageRows);
            this.tabControlDesigner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlDesigner.Location = new System.Drawing.Point(0, 0);
            this.tabControlDesigner.Name = "tabControlDesigner";
            this.tabControlDesigner.SelectedIndex = 0;
            this.tabControlDesigner.Size = new System.Drawing.Size(283, 272);
            this.tabControlDesigner.TabIndex = 10;
            // 
            // tabPageDetail
            // 
            this.tabPageDetail.Location = new System.Drawing.Point(4, 22);
            this.tabPageDetail.Name = "tabPageDetail";
            this.tabPageDetail.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDetail.Size = new System.Drawing.Size(275, 246);
            this.tabPageDetail.TabIndex = 1;
            this.tabPageDetail.Text = "Details";
            this.tabPageDetail.UseVisualStyleBackColor = true;
            // 
            // tabPageFonts
            // 
            this.tabPageFonts.Controls.Add(this.splitContainerFonts);
            this.tabPageFonts.Location = new System.Drawing.Point(4, 22);
            this.tabPageFonts.Name = "tabPageFonts";
            this.tabPageFonts.Size = new System.Drawing.Size(275, 246);
            this.tabPageFonts.TabIndex = 2;
            this.tabPageFonts.Text = "Fonts";
            this.tabPageFonts.UseVisualStyleBackColor = true;
            // 
            // splitContainerFonts
            // 
            this.splitContainerFonts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerFonts.Location = new System.Drawing.Point(0, 0);
            this.splitContainerFonts.Name = "splitContainerFonts";
            // 
            // splitContainerFonts.Panel1
            // 
            this.splitContainerFonts.Panel1.Controls.Add(this.listBoxFonts);
            this.splitContainerFonts.Size = new System.Drawing.Size(275, 246);
            this.splitContainerFonts.SplitterDistance = 91;
            this.splitContainerFonts.TabIndex = 0;
            // 
            // listBoxFonts
            // 
            this.listBoxFonts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxFonts.FormattingEnabled = true;
            this.listBoxFonts.Location = new System.Drawing.Point(0, 0);
            this.listBoxFonts.Name = "listBoxFonts";
            this.listBoxFonts.Size = new System.Drawing.Size(91, 246);
            this.listBoxFonts.TabIndex = 0;
            // 
            // tabPagePage
            // 
            this.tabPagePage.Location = new System.Drawing.Point(4, 22);
            this.tabPagePage.Name = "tabPagePage";
            this.tabPagePage.Size = new System.Drawing.Size(275, 246);
            this.tabPagePage.TabIndex = 3;
            this.tabPagePage.Text = "Page";
            this.tabPagePage.UseVisualStyleBackColor = true;
            // 
            // tabPageTemplates
            // 
            this.tabPageTemplates.Controls.Add(this.splitContainerTemplates);
            this.tabPageTemplates.Location = new System.Drawing.Point(4, 22);
            this.tabPageTemplates.Name = "tabPageTemplates";
            this.tabPageTemplates.Size = new System.Drawing.Size(275, 246);
            this.tabPageTemplates.TabIndex = 4;
            this.tabPageTemplates.Text = "Templates";
            this.tabPageTemplates.UseVisualStyleBackColor = true;
            // 
            // splitContainerTemplates
            // 
            this.splitContainerTemplates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTemplates.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTemplates.Name = "splitContainerTemplates";
            // 
            // splitContainerTemplates.Panel1
            // 
            this.splitContainerTemplates.Panel1.Controls.Add(this.listBoxTemplates);
            this.splitContainerTemplates.Size = new System.Drawing.Size(275, 246);
            this.splitContainerTemplates.SplitterDistance = 91;
            this.splitContainerTemplates.TabIndex = 0;
            // 
            // listBoxTemplates
            // 
            this.listBoxTemplates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxTemplates.FormattingEnabled = true;
            this.listBoxTemplates.Location = new System.Drawing.Point(0, 0);
            this.listBoxTemplates.Name = "listBoxTemplates";
            this.listBoxTemplates.Size = new System.Drawing.Size(91, 246);
            this.listBoxTemplates.TabIndex = 0;
            // 
            // tabPageRows
            // 
            this.tabPageRows.Controls.Add(this.tableLayoutPanelRowOptions);
            this.tabPageRows.Location = new System.Drawing.Point(4, 22);
            this.tabPageRows.Name = "tabPageRows";
            this.tabPageRows.Size = new System.Drawing.Size(275, 246);
            this.tabPageRows.TabIndex = 5;
            this.tabPageRows.Text = "Rows";
            this.tabPageRows.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelRowOptions
            // 
            this.tableLayoutPanelRowOptions.ColumnCount = 2;
            this.tableLayoutPanelRowOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRowOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRowOptions.Controls.Add(this.labelRowOptionsHeader, 0, 0);
            this.tableLayoutPanelRowOptions.Controls.Add(this.labelRowOptionsAttributes, 0, 1);
            this.tableLayoutPanelRowOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelRowOptions.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelRowOptions.Name = "tableLayoutPanelRowOptions";
            this.tableLayoutPanelRowOptions.RowCount = 2;
            this.tableLayoutPanelRowOptions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRowOptions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRowOptions.Size = new System.Drawing.Size(275, 246);
            this.tableLayoutPanelRowOptions.TabIndex = 0;
            // 
            // labelRowOptionsHeader
            // 
            this.labelRowOptionsHeader.AutoSize = true;
            this.labelRowOptionsHeader.Location = new System.Drawing.Point(3, 0);
            this.labelRowOptionsHeader.Name = "labelRowOptionsHeader";
            this.labelRowOptionsHeader.Size = new System.Drawing.Size(95, 13);
            this.labelRowOptionsHeader.TabIndex = 0;
            this.labelRowOptionsHeader.Text = "Set options for row";
            // 
            // labelRowOptionsAttributes
            // 
            this.labelRowOptionsAttributes.AutoSize = true;
            this.labelRowOptionsAttributes.Location = new System.Drawing.Point(3, 123);
            this.labelRowOptionsAttributes.Name = "labelRowOptionsAttributes";
            this.labelRowOptionsAttributes.Size = new System.Drawing.Size(19, 13);
            this.labelRowOptionsAttributes.TabIndex = 1;
            this.labelRowOptionsAttributes.Text = "??";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(113, 186);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(59, 25);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // toolStripLabel
            // 
            this.tableLayoutPanelLabel.SetColumnSpan(this.toolStripLabel, 2);
            this.toolStripLabel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNewSchemaFile,
            this.toolStripButtonOpenSchemaFile,
            this.toolStripSeparatorLabel,
            this.toolStripButtonLabelPreview,
            this.toolStripButtonLabelMulti,
            this.toolStripTextBoxPrintDuplicates,
            this.toolStripSeparatorPrint,
            this.toolStripButtonLabelPrintBrowserOptions,
            this.toolStripButtonPageSetup,
            this.toolStripButtonPrint,
            this.toolStripButtonLabelExport});
            this.toolStripLabel.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStripLabel.Location = new System.Drawing.Point(690, 302);
            this.toolStripLabel.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.toolStripLabel.Name = "toolStripLabel";
            this.toolStripLabel.Size = new System.Drawing.Size(171, 23);
            this.toolStripLabel.TabIndex = 0;
            // 
            // toolStripButtonNewSchemaFile
            // 
            this.toolStripButtonNewSchemaFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNewSchemaFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNewSchemaFile.Image")));
            this.toolStripButtonNewSchemaFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNewSchemaFile.Name = "toolStripButtonNewSchemaFile";
            this.toolStripButtonNewSchemaFile.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonNewSchemaFile.Text = "Create a new schema file";
            this.toolStripButtonNewSchemaFile.Visible = false;
            // 
            // toolStripButtonOpenSchemaFile
            // 
            this.toolStripButtonOpenSchemaFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonOpenSchemaFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOpenSchemaFile.Image")));
            this.toolStripButtonOpenSchemaFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpenSchemaFile.Name = "toolStripButtonOpenSchemaFile";
            this.toolStripButtonOpenSchemaFile.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonOpenSchemaFile.Text = "Select schema file (XSD)";
            this.toolStripButtonOpenSchemaFile.Click += new System.EventHandler(this.toolStripButtonOpenSchemaFile_Click);
            // 
            // toolStripSeparatorLabel
            // 
            this.toolStripSeparatorLabel.Name = "toolStripSeparatorLabel";
            this.toolStripSeparatorLabel.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonLabelPreview
            // 
            this.toolStripButtonLabelPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLabelPreview.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLabelPreview.Image")));
            this.toolStripButtonLabelPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLabelPreview.Name = "toolStripButtonLabelPreview";
            this.toolStripButtonLabelPreview.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonLabelPreview.Text = "Label preview";
            // 
            // toolStripButtonLabelMulti
            // 
            this.toolStripButtonLabelMulti.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLabelMulti.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLabelMulti.Image")));
            this.toolStripButtonLabelMulti.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLabelMulti.Name = "toolStripButtonLabelMulti";
            this.toolStripButtonLabelMulti.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonLabelMulti.Text = "Preview for all labels in query";
            // 
            // toolStripTextBoxPrintDuplicates
            // 
            this.toolStripTextBoxPrintDuplicates.Name = "toolStripTextBoxPrintDuplicates";
            this.toolStripTextBoxPrintDuplicates.Size = new System.Drawing.Size(18, 23);
            this.toolStripTextBoxPrintDuplicates.Text = "1";
            this.toolStripTextBoxPrintDuplicates.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolStripTextBoxPrintDuplicates.ToolTipText = "Number of duplicates that should be printed (1 - 99)";
            // 
            // toolStripSeparatorPrint
            // 
            this.toolStripSeparatorPrint.Name = "toolStripSeparatorPrint";
            this.toolStripSeparatorPrint.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonLabelPrintBrowserOptions
            // 
            this.toolStripButtonLabelPrintBrowserOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLabelPrintBrowserOptions.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLabelPrintBrowserOptions.Image")));
            this.toolStripButtonLabelPrintBrowserOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLabelPrintBrowserOptions.Name = "toolStripButtonLabelPrintBrowserOptions";
            this.toolStripButtonLabelPrintBrowserOptions.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonLabelPrintBrowserOptions.Text = "set browser options";
            this.toolStripButtonLabelPrintBrowserOptions.Visible = false;
            // 
            // toolStripButtonPageSetup
            // 
            this.toolStripButtonPageSetup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPageSetup.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPageSetup.Image")));
            this.toolStripButtonPageSetup.ImageTransparentColor = System.Drawing.Color.Purple;
            this.toolStripButtonPageSetup.Name = "toolStripButtonPageSetup";
            this.toolStripButtonPageSetup.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonPageSetup.Text = "Page setup";
            // 
            // toolStripButtonPrint
            // 
            this.toolStripButtonPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPrint.Image = global::DiversityCollection.Resource.Print1;
            this.toolStripButtonPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPrint.Name = "toolStripButtonPrint";
            this.toolStripButtonPrint.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonPrint.Text = "Print label";
            // 
            // toolStripButtonLabelExport
            // 
            this.toolStripButtonLabelExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLabelExport.Image = global::DiversityCollection.Resource.Save;
            this.toolStripButtonLabelExport.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonLabelExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLabelExport.Name = "toolStripButtonLabelExport";
            this.toolStripButtonLabelExport.Size = new System.Drawing.Size(23, 18);
            this.toolStripButtonLabelExport.Text = "save print files";
            // 
            // textBoxSchemaFile
            // 
            this.tableLayoutPanelLabel.SetColumnSpan(this.textBoxSchemaFile, 2);
            this.textBoxSchemaFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSchemaFile.Location = new System.Drawing.Point(68, 306);
            this.textBoxSchemaFile.Margin = new System.Windows.Forms.Padding(0, 4, 3, 0);
            this.textBoxSchemaFile.Name = "textBoxSchemaFile";
            this.textBoxSchemaFile.Size = new System.Drawing.Size(619, 20);
            this.textBoxSchemaFile.TabIndex = 1;
            // 
            // labelSchemaFile
            // 
            this.labelSchemaFile.AutoSize = true;
            this.labelSchemaFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSchemaFile.Location = new System.Drawing.Point(3, 302);
            this.labelSchemaFile.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelSchemaFile.Name = "labelSchemaFile";
            this.labelSchemaFile.Size = new System.Drawing.Size(65, 32);
            this.labelSchemaFile.TabIndex = 2;
            this.labelSchemaFile.Text = "Schema file:";
            this.labelSchemaFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxReportTitle
            // 
            this.tableLayoutPanelLabel.SetColumnSpan(this.textBoxReportTitle, 2);
            this.textBoxReportTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxReportTitle.Location = new System.Drawing.Point(68, 334);
            this.textBoxReportTitle.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.textBoxReportTitle.Name = "textBoxReportTitle";
            this.textBoxReportTitle.Size = new System.Drawing.Size(619, 20);
            this.textBoxReportTitle.TabIndex = 3;
            // 
            // labelReportTitle
            // 
            this.labelReportTitle.AutoSize = true;
            this.labelReportTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReportTitle.Location = new System.Drawing.Point(3, 334);
            this.labelReportTitle.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelReportTitle.Name = "labelReportTitle";
            this.labelReportTitle.Size = new System.Drawing.Size(65, 21);
            this.labelReportTitle.TabIndex = 4;
            this.labelReportTitle.Text = "Title:";
            this.labelReportTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBoxUseStockForLabelDuplicates
            // 
            this.checkBoxUseStockForLabelDuplicates.AutoSize = true;
            this.checkBoxUseStockForLabelDuplicates.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxUseStockForLabelDuplicates.Location = new System.Drawing.Point(709, 337);
            this.checkBoxUseStockForLabelDuplicates.Margin = new System.Windows.Forms.Padding(3, 3, 1, 1);
            this.checkBoxUseStockForLabelDuplicates.Name = "checkBoxUseStockForLabelDuplicates";
            this.checkBoxUseStockForLabelDuplicates.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxUseStockForLabelDuplicates.Size = new System.Drawing.Size(131, 17);
            this.checkBoxUseStockForLabelDuplicates.TabIndex = 7;
            this.checkBoxUseStockForLabelDuplicates.Text = ".Regard stock for dupl";
            this.checkBoxUseStockForLabelDuplicates.UseVisualStyleBackColor = true;
            // 
            // checkBoxPrintRestrictToMaterial
            // 
            this.checkBoxPrintRestrictToMaterial.AutoSize = true;
            this.checkBoxPrintRestrictToMaterial.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxPrintRestrictToMaterial.Location = new System.Drawing.Point(727, 355);
            this.checkBoxPrintRestrictToMaterial.Margin = new System.Windows.Forms.Padding(3, 0, 1, 3);
            this.checkBoxPrintRestrictToMaterial.Name = "checkBoxPrintRestrictToMaterial";
            this.checkBoxPrintRestrictToMaterial.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxPrintRestrictToMaterial.Size = new System.Drawing.Size(113, 23);
            this.checkBoxPrintRestrictToMaterial.TabIndex = 9;
            this.checkBoxPrintRestrictToMaterial.Text = "Restrict to material";
            this.checkBoxPrintRestrictToMaterial.UseVisualStyleBackColor = true;
            // 
            // checkBoxPrintRestrictToCollection
            // 
            this.checkBoxPrintRestrictToCollection.AutoSize = true;
            this.checkBoxPrintRestrictToCollection.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxPrintRestrictToCollection.Location = new System.Drawing.Point(565, 355);
            this.checkBoxPrintRestrictToCollection.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.checkBoxPrintRestrictToCollection.Name = "checkBoxPrintRestrictToCollection";
            this.checkBoxPrintRestrictToCollection.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxPrintRestrictToCollection.Size = new System.Drawing.Size(122, 23);
            this.checkBoxPrintRestrictToCollection.TabIndex = 10;
            this.checkBoxPrintRestrictToCollection.Text = "Restrict to collection";
            this.checkBoxPrintRestrictToCollection.UseVisualStyleBackColor = true;
            // 
            // pictureBoxPrintRestrictToMaterial
            // 
            this.pictureBoxPrintRestrictToMaterial.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBoxPrintRestrictToMaterial.Location = new System.Drawing.Point(841, 356);
            this.pictureBoxPrintRestrictToMaterial.Margin = new System.Windows.Forms.Padding(0, 1, 3, 3);
            this.pictureBoxPrintRestrictToMaterial.Name = "pictureBoxPrintRestrictToMaterial";
            this.pictureBoxPrintRestrictToMaterial.Size = new System.Drawing.Size(16, 22);
            this.pictureBoxPrintRestrictToMaterial.TabIndex = 11;
            this.pictureBoxPrintRestrictToMaterial.TabStop = false;
            // 
            // labelLabelConversion
            // 
            this.labelLabelConversion.AutoSize = true;
            this.labelLabelConversion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLabelConversion.Location = new System.Drawing.Point(3, 358);
            this.labelLabelConversion.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelLabelConversion.Name = "labelLabelConversion";
            this.labelLabelConversion.Size = new System.Drawing.Size(65, 23);
            this.labelLabelConversion.TabIndex = 12;
            this.labelLabelConversion.Text = "Conversion:";
            this.labelLabelConversion.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBoxLabelConversion
            // 
            this.comboBoxLabelConversion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxLabelConversion.FormattingEnabled = true;
            this.comboBoxLabelConversion.Location = new System.Drawing.Point(68, 355);
            this.comboBoxLabelConversion.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.comboBoxLabelConversion.Name = "comboBoxLabelConversion";
            this.comboBoxLabelConversion.Size = new System.Drawing.Size(494, 21);
            this.comboBoxLabelConversion.TabIndex = 13;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanelLabel.SetColumnSpan(this.labelHeader, 3);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(684, 24);
            this.labelHeader.TabIndex = 15;
            this.labelHeader.Text = "Editing the schemas for creating a label";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonRefresh
            // 
            this.tableLayoutPanelLabel.SetColumnSpan(this.buttonRefresh, 2);
            this.buttonRefresh.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonRefresh.Image = global::DiversityCollection.Resource.Reload;
            this.buttonRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRefresh.Location = new System.Drawing.Point(789, 0);
            this.buttonRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 24);
            this.buttonRefresh.TabIndex = 16;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // webBrowserLabel
            // 
            this.webBrowserLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserLabel.Location = new System.Drawing.Point(0, 0);
            this.webBrowserLabel.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserLabel.Name = "webBrowserLabel";
            this.webBrowserLabel.Size = new System.Drawing.Size(864, 200);
            this.webBrowserLabel.TabIndex = 0;
            // 
            // openFileDialogLabelSchema
            // 
            this.openFileDialogLabelSchema.FileName = "openFileDialogLabelSchema";
            // 
            // FormLabelEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 589);
            this.Controls.Add(this.splitContainerMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormLabelEditor";
            this.Text = "Label editor";
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.tableLayoutPanelLabel.ResumeLayout(false);
            this.tableLayoutPanelLabel.PerformLayout();
            this.splitContainerDesigner.Panel1.ResumeLayout(false);
            this.splitContainerDesigner.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDesigner)).EndInit();
            this.splitContainerDesigner.ResumeLayout(false);
            this.tableLayoutPanelDesigner.ResumeLayout(false);
            this.tableLayoutPanelDesigner.PerformLayout();
            this.toolStripRows.ResumeLayout(false);
            this.toolStripRows.PerformLayout();
            this.toolStripColumns.ResumeLayout(false);
            this.toolStripColumns.PerformLayout();
            this.tabControlDesigner.ResumeLayout(false);
            this.tabPageFonts.ResumeLayout(false);
            this.splitContainerFonts.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerFonts)).EndInit();
            this.splitContainerFonts.ResumeLayout(false);
            this.tabPageTemplates.ResumeLayout(false);
            this.splitContainerTemplates.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTemplates)).EndInit();
            this.splitContainerTemplates.ResumeLayout(false);
            this.tabPageRows.ResumeLayout(false);
            this.tableLayoutPanelRowOptions.ResumeLayout(false);
            this.tableLayoutPanelRowOptions.PerformLayout();
            this.toolStripLabel.ResumeLayout(false);
            this.toolStripLabel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPrintRestrictToMaterial)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLabel;
        private System.Windows.Forms.ToolStrip toolStripLabel;
        private System.Windows.Forms.ToolStripButton toolStripButtonNewSchemaFile;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpenSchemaFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorLabel;
        private System.Windows.Forms.ToolStripButton toolStripButtonLabelPreview;
        private System.Windows.Forms.ToolStripButton toolStripButtonLabelMulti;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxPrintDuplicates;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorPrint;
        private System.Windows.Forms.ToolStripButton toolStripButtonLabelPrintBrowserOptions;
        private System.Windows.Forms.ToolStripButton toolStripButtonPageSetup;
        private System.Windows.Forms.ToolStripButton toolStripButtonPrint;
        private System.Windows.Forms.ToolStripButton toolStripButtonLabelExport;
        private System.Windows.Forms.TextBox textBoxSchemaFile;
        private System.Windows.Forms.Label labelSchemaFile;
        private System.Windows.Forms.TextBox textBoxReportTitle;
        private System.Windows.Forms.Label labelReportTitle;
        private System.Windows.Forms.CheckBox checkBoxUseStockForLabelDuplicates;
        private System.Windows.Forms.CheckBox checkBoxPrintRestrictToMaterial;
        private System.Windows.Forms.CheckBox checkBoxPrintRestrictToCollection;
        private System.Windows.Forms.PictureBox pictureBoxPrintRestrictToMaterial;
        private System.Windows.Forms.Label labelLabelConversion;
        private System.Windows.Forms.ComboBox comboBoxLabelConversion;
        private System.Windows.Forms.WebBrowser webBrowserLabel;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.SplitContainer splitContainerDesigner;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDesigner;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabControlDesigner;
        private System.Windows.Forms.ToolStrip toolStripColumns;
        private System.Windows.Forms.ToolStripButton toolStripButtonColumnAdd;
        private System.Windows.Forms.ToolStrip toolStripRows;
        private System.Windows.Forms.ToolStripButton toolStripButtonRowAdd;
        private System.Windows.Forms.TabPage tabPageDetail;
        private System.Windows.Forms.TabPage tabPageFonts;
        private System.Windows.Forms.TabPage tabPagePage;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.ToolStripButton toolStripButtonColumnRemove;
        private System.Windows.Forms.ToolStripButton toolStripButtonRowRemove;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRowColumnEditor;
        private System.Windows.Forms.OpenFileDialog openFileDialogLabelSchema;
        private System.Windows.Forms.Panel panelRows;
        private System.Windows.Forms.Panel panelColumns;
        private System.Windows.Forms.TabPage tabPageTemplates;
        private System.Windows.Forms.TabPage tabPageRows;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRowOptions;
        private System.Windows.Forms.Label labelRowOptionsHeader;
        private System.Windows.Forms.Label labelRowOptionsAttributes;
        private System.Windows.Forms.SplitContainer splitContainerFonts;
        private System.Windows.Forms.ListBox listBoxFonts;
        private System.Windows.Forms.SplitContainer splitContainerTemplates;
        private System.Windows.Forms.ListBox listBoxTemplates;
    }
}