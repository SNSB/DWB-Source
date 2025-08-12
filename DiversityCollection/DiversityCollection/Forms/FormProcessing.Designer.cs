namespace DiversityCollection.Forms
{
    partial class FormProcessing
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProcessing));
            this.labelDescription = new System.Windows.Forms.Label();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.processingBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetProcessing = new DiversityCollection.Datasets.DataSetProcessing();
            this.labelNotes = new System.Windows.Forms.Label();
            this.textBoxNotes = new System.Windows.Forms.TextBox();
            this.labelURI = new System.Windows.Forms.Label();
            this.textBoxURI = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.labelHeader = new System.Windows.Forms.Label();
            this.groupBoxProcessing = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelAnalysis = new System.Windows.Forms.TableLayoutPanel();
            this.labelMaterialCategory = new System.Windows.Forms.Label();
            this.treeViewProcessing = new System.Windows.Forms.TreeView();
            this.toolStripProcessing = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSpecimenList = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonIncludeID = new System.Windows.Forms.ToolStripButton();
            this.labelDisplayText = new System.Windows.Forms.Label();
            this.textBoxDisplayText = new System.Windows.Forms.TextBox();
            this.buttonUriOpen = new System.Windows.Forms.Button();
            this.listBoxMaterialCategory = new System.Windows.Forms.ListBox();
            this.processingMaterialCategoryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStripMaterialCategory = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonMaterialCategoryNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMaterialCategoryDelete = new System.Windows.Forms.ToolStripButton();
            this.labelProject = new System.Windows.Forms.Label();
            this.listBoxProjects = new System.Windows.Forms.ListBox();
            this.projectProcessingListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStripProjects = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonProjectNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonProjectRemove = new System.Windows.Forms.ToolStripButton();
            this.checkBoxOnlyHierarchy = new System.Windows.Forms.CheckBox();
            this.labelMethods = new System.Windows.Forms.Label();
            this.listBoxMethods = new System.Windows.Forms.ListBox();
            this.methodForProcessingListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStripMethods = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonMethodsAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMethodsDelete = new System.Windows.Forms.ToolStripButton();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.userControlQueryList = new DiversityWorkbench.UserControls.UserControlQueryList();
            this.splitContainerData = new System.Windows.Forms.SplitContainer();
            this.splitContainerURI = new System.Windows.Forms.SplitContainer();
            this.panelURI = new System.Windows.Forms.Panel();
            this.webBrowserURI = new System.Windows.Forms.WebBrowser();
            this.userControlSpecimenList = new DiversityCollection.UserControls.UserControlSpecimenList();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.imageListSpecimenList = new System.Windows.Forms.ImageList(this.components);
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showURIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.historyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFeedback = new System.Windows.Forms.ToolStripMenuItem();
            this.tableEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectProcessingListTableAdapter = new DiversityCollection.Datasets.DataSetProcessingTableAdapters.ProjectProcessingListTableAdapter();
            this.methodForProcessingListTableAdapter = new DiversityCollection.Datasets.DataSetProcessingTableAdapters.MethodForProcessingListTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.processingBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetProcessing)).BeginInit();
            this.groupBoxProcessing.SuspendLayout();
            this.tableLayoutPanelAnalysis.SuspendLayout();
            this.toolStripProcessing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.processingMaterialCategoryBindingSource)).BeginInit();
            this.toolStripMaterialCategory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.projectProcessingListBindingSource)).BeginInit();
            this.toolStripProjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.methodForProcessingListBindingSource)).BeginInit();
            this.toolStripMethods.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerData)).BeginInit();
            this.splitContainerData.Panel1.SuspendLayout();
            this.splitContainerData.Panel2.SuspendLayout();
            this.splitContainerData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerURI)).BeginInit();
            this.splitContainerURI.Panel1.SuspendLayout();
            this.splitContainerURI.Panel2.SuspendLayout();
            this.splitContainerURI.SuspendLayout();
            this.panelURI.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.menuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDescription.Location = new System.Drawing.Point(3, 114);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(64, 49);
            this.labelDescription.TabIndex = 4;
            this.labelDescription.Text = "Description:";
            this.labelDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxDescription
            // 
            this.tableLayoutPanelAnalysis.SetColumnSpan(this.textBoxDescription, 3);
            this.textBoxDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.processingBindingSource, "Description", true));
            this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDescription.Location = new System.Drawing.Point(73, 111);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(351, 49);
            this.textBoxDescription.TabIndex = 5;
            // 
            // processingBindingSource
            // 
            this.processingBindingSource.DataMember = "Processing";
            this.processingBindingSource.DataSource = this.dataSetProcessing;
            // 
            // dataSetProcessing
            // 
            this.dataSetProcessing.DataSetName = "DataSetProcessing";
            this.dataSetProcessing.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // labelNotes
            // 
            this.labelNotes.AutoSize = true;
            this.labelNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNotes.Location = new System.Drawing.Point(3, 169);
            this.labelNotes.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelNotes.Name = "labelNotes";
            this.labelNotes.Size = new System.Drawing.Size(64, 21);
            this.labelNotes.TabIndex = 6;
            this.labelNotes.Text = "Notes:";
            this.labelNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxNotes
            // 
            this.textBoxNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.processingBindingSource, "Notes", true));
            this.textBoxNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNotes.Location = new System.Drawing.Point(73, 166);
            this.textBoxNotes.Multiline = true;
            this.textBoxNotes.Name = "textBoxNotes";
            this.textBoxNotes.Size = new System.Drawing.Size(237, 21);
            this.textBoxNotes.TabIndex = 7;
            // 
            // labelURI
            // 
            this.labelURI.AutoSize = true;
            this.labelURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelURI.Location = new System.Drawing.Point(3, 319);
            this.labelURI.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelURI.Name = "labelURI";
            this.labelURI.Size = new System.Drawing.Size(64, 22);
            this.labelURI.TabIndex = 8;
            this.labelURI.Text = "URI:";
            this.labelURI.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxURI
            // 
            this.tableLayoutPanelAnalysis.SetColumnSpan(this.textBoxURI, 2);
            this.textBoxURI.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.processingBindingSource, "ProcessingURI", true));
            this.textBoxURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxURI.Location = new System.Drawing.Point(73, 316);
            this.textBoxURI.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.textBoxURI.Name = "textBoxURI";
            this.textBoxURI.Size = new System.Drawing.Size(330, 20);
            this.textBoxURI.TabIndex = 9;
            this.textBoxURI.TextChanged += new System.EventHandler(this.textBoxURI_TextChanged);
            // 
            // labelHeader
            // 
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(0, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(810, 24);
            this.labelHeader.TabIndex = 3;
            this.labelHeader.Text = "Select a processing";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBoxProcessing
            // 
            this.groupBoxProcessing.Controls.Add(this.tableLayoutPanelAnalysis);
            this.groupBoxProcessing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxProcessing.Location = new System.Drawing.Point(0, 0);
            this.groupBoxProcessing.Name = "groupBoxProcessing";
            this.groupBoxProcessing.Size = new System.Drawing.Size(433, 360);
            this.groupBoxProcessing.TabIndex = 1;
            this.groupBoxProcessing.TabStop = false;
            this.groupBoxProcessing.Text = "Processing";
            // 
            // tableLayoutPanelAnalysis
            // 
            this.tableLayoutPanelAnalysis.ColumnCount = 4;
            this.tableLayoutPanelAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelAnalysis.Controls.Add(this.labelMaterialCategory, 0, 4);
            this.tableLayoutPanelAnalysis.Controls.Add(this.treeViewProcessing, 0, 0);
            this.tableLayoutPanelAnalysis.Controls.Add(this.toolStripProcessing, 3, 0);
            this.tableLayoutPanelAnalysis.Controls.Add(this.labelDisplayText, 0, 1);
            this.tableLayoutPanelAnalysis.Controls.Add(this.textBoxDisplayText, 1, 1);
            this.tableLayoutPanelAnalysis.Controls.Add(this.labelDescription, 0, 2);
            this.tableLayoutPanelAnalysis.Controls.Add(this.textBoxDescription, 1, 2);
            this.tableLayoutPanelAnalysis.Controls.Add(this.labelNotes, 0, 3);
            this.tableLayoutPanelAnalysis.Controls.Add(this.textBoxNotes, 1, 3);
            this.tableLayoutPanelAnalysis.Controls.Add(this.labelURI, 0, 7);
            this.tableLayoutPanelAnalysis.Controls.Add(this.textBoxURI, 1, 7);
            this.tableLayoutPanelAnalysis.Controls.Add(this.buttonUriOpen, 3, 7);
            this.tableLayoutPanelAnalysis.Controls.Add(this.listBoxMaterialCategory, 1, 4);
            this.tableLayoutPanelAnalysis.Controls.Add(this.toolStripMaterialCategory, 3, 4);
            this.tableLayoutPanelAnalysis.Controls.Add(this.labelProject, 0, 5);
            this.tableLayoutPanelAnalysis.Controls.Add(this.listBoxProjects, 1, 5);
            this.tableLayoutPanelAnalysis.Controls.Add(this.toolStripProjects, 3, 5);
            this.tableLayoutPanelAnalysis.Controls.Add(this.checkBoxOnlyHierarchy, 2, 3);
            this.tableLayoutPanelAnalysis.Controls.Add(this.labelMethods, 0, 6);
            this.tableLayoutPanelAnalysis.Controls.Add(this.listBoxMethods, 1, 6);
            this.tableLayoutPanelAnalysis.Controls.Add(this.toolStripMethods, 3, 6);
            this.tableLayoutPanelAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelAnalysis.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelAnalysis.Name = "tableLayoutPanelAnalysis";
            this.tableLayoutPanelAnalysis.RowCount = 8;
            this.tableLayoutPanelAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 28.57143F));
            this.tableLayoutPanelAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.04762F));
            this.tableLayoutPanelAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.523809F));
            this.tableLayoutPanelAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanelAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanelAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanelAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAnalysis.Size = new System.Drawing.Size(427, 341);
            this.tableLayoutPanelAnalysis.TabIndex = 0;
            // 
            // labelMaterialCategory
            // 
            this.labelMaterialCategory.AutoSize = true;
            this.labelMaterialCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaterialCategory.Location = new System.Drawing.Point(3, 196);
            this.labelMaterialCategory.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelMaterialCategory.Name = "labelMaterialCategory";
            this.labelMaterialCategory.Size = new System.Drawing.Size(64, 35);
            this.labelMaterialCategory.TabIndex = 13;
            this.labelMaterialCategory.Text = "Mat.cat.:";
            this.labelMaterialCategory.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // treeViewProcessing
            // 
            this.treeViewProcessing.AllowDrop = true;
            this.tableLayoutPanelAnalysis.SetColumnSpan(this.treeViewProcessing, 3);
            this.treeViewProcessing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewProcessing.Location = new System.Drawing.Point(3, 3);
            this.treeViewProcessing.Name = "treeViewProcessing";
            this.treeViewProcessing.Size = new System.Drawing.Size(397, 76);
            this.treeViewProcessing.TabIndex = 0;
            // 
            // toolStripProcessing
            // 
            this.toolStripProcessing.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStripProcessing.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNew,
            this.toolStripButtonDelete,
            this.toolStripButtonSpecimenList,
            this.toolStripButtonIncludeID});
            this.toolStripProcessing.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.toolStripProcessing.Location = new System.Drawing.Point(403, 0);
            this.toolStripProcessing.Name = "toolStripProcessing";
            this.toolStripProcessing.Size = new System.Drawing.Size(24, 82);
            this.toolStripProcessing.TabIndex = 1;
            this.toolStripProcessing.Text = "toolStrip1";
            // 
            // toolStripButtonNew
            // 
            this.toolStripButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNew.Image = global::DiversityCollection.Resource.New1;
            this.toolStripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNew.Name = "toolStripButtonNew";
            this.toolStripButtonNew.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonNew.Text = "Insert a new analysis dependent on the selected analysis";
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDelete.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonDelete.Text = "delete the selected analysis";
            // 
            // toolStripButtonSpecimenList
            // 
            this.toolStripButtonSpecimenList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSpecimenList.Image = global::DiversityCollection.Resource.List;
            this.toolStripButtonSpecimenList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSpecimenList.Name = "toolStripButtonSpecimenList";
            this.toolStripButtonSpecimenList.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonSpecimenList.Text = "Hide specimen list";
            // 
            // toolStripButtonIncludeID
            // 
            this.toolStripButtonIncludeID.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonIncludeID.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.toolStripButtonIncludeID.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripButtonIncludeID.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonIncludeID.Image")));
            this.toolStripButtonIncludeID.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonIncludeID.Name = "toolStripButtonIncludeID";
            this.toolStripButtonIncludeID.Size = new System.Drawing.Size(24, 17);
            this.toolStripButtonIncludeID.Text = "ID";
            this.toolStripButtonIncludeID.ToolTipText = "Display the ID of the datasets in the tree";
            // 
            // labelDisplayText
            // 
            this.labelDisplayText.AutoSize = true;
            this.labelDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDisplayText.Location = new System.Drawing.Point(3, 88);
            this.labelDisplayText.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelDisplayText.Name = "labelDisplayText";
            this.labelDisplayText.Size = new System.Drawing.Size(64, 20);
            this.labelDisplayText.TabIndex = 2;
            this.labelDisplayText.Text = "Display text:";
            this.labelDisplayText.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxDisplayText
            // 
            this.tableLayoutPanelAnalysis.SetColumnSpan(this.textBoxDisplayText, 3);
            this.textBoxDisplayText.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.processingBindingSource, "DisplayText", true));
            this.textBoxDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDisplayText.Location = new System.Drawing.Point(73, 85);
            this.textBoxDisplayText.Name = "textBoxDisplayText";
            this.textBoxDisplayText.Size = new System.Drawing.Size(351, 20);
            this.textBoxDisplayText.TabIndex = 3;
            // 
            // buttonUriOpen
            // 
            this.buttonUriOpen.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonUriOpen.Image = global::DiversityCollection.Resource.Browse;
            this.buttonUriOpen.Location = new System.Drawing.Point(403, 313);
            this.buttonUriOpen.Margin = new System.Windows.Forms.Padding(0);
            this.buttonUriOpen.Name = "buttonUriOpen";
            this.buttonUriOpen.Size = new System.Drawing.Size(24, 24);
            this.buttonUriOpen.TabIndex = 10;
            this.buttonUriOpen.UseVisualStyleBackColor = true;
            this.buttonUriOpen.Click += new System.EventHandler(this.buttonUriOpen_Click);
            // 
            // listBoxMaterialCategory
            // 
            this.tableLayoutPanelAnalysis.SetColumnSpan(this.listBoxMaterialCategory, 2);
            this.listBoxMaterialCategory.DataSource = this.processingMaterialCategoryBindingSource;
            this.listBoxMaterialCategory.DisplayMember = "MaterialCategory";
            this.listBoxMaterialCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxMaterialCategory.FormattingEnabled = true;
            this.listBoxMaterialCategory.IntegralHeight = false;
            this.listBoxMaterialCategory.Location = new System.Drawing.Point(73, 190);
            this.listBoxMaterialCategory.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.listBoxMaterialCategory.Name = "listBoxMaterialCategory";
            this.listBoxMaterialCategory.Size = new System.Drawing.Size(330, 38);
            this.listBoxMaterialCategory.TabIndex = 14;
            this.listBoxMaterialCategory.ValueMember = "MaterialCategory";
            // 
            // processingMaterialCategoryBindingSource
            // 
            this.processingMaterialCategoryBindingSource.DataMember = "ProcessingMaterialCategory";
            this.processingMaterialCategoryBindingSource.DataSource = this.dataSetProcessing;
            // 
            // toolStripMaterialCategory
            // 
            this.toolStripMaterialCategory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonMaterialCategoryNew,
            this.toolStripButtonMaterialCategoryDelete});
            this.toolStripMaterialCategory.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.toolStripMaterialCategory.Location = new System.Drawing.Point(403, 190);
            this.toolStripMaterialCategory.Name = "toolStripMaterialCategory";
            this.toolStripMaterialCategory.Size = new System.Drawing.Size(24, 41);
            this.toolStripMaterialCategory.TabIndex = 15;
            this.toolStripMaterialCategory.Text = "toolStrip1";
            // 
            // toolStripButtonMaterialCategoryNew
            // 
            this.toolStripButtonMaterialCategoryNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMaterialCategoryNew.Image = global::DiversityCollection.Resource.New1;
            this.toolStripButtonMaterialCategoryNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMaterialCategoryNew.Name = "toolStripButtonMaterialCategoryNew";
            this.toolStripButtonMaterialCategoryNew.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonMaterialCategoryNew.Text = "Add a new material cateogory";
            this.toolStripButtonMaterialCategoryNew.Click += new System.EventHandler(this.toolStripButtonMaterialCategoryNew_Click);
            // 
            // toolStripButtonMaterialCategoryDelete
            // 
            this.toolStripButtonMaterialCategoryDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMaterialCategoryDelete.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonMaterialCategoryDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMaterialCategoryDelete.Name = "toolStripButtonMaterialCategoryDelete";
            this.toolStripButtonMaterialCategoryDelete.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonMaterialCategoryDelete.Text = "Delete the selected material category";
            this.toolStripButtonMaterialCategoryDelete.Click += new System.EventHandler(this.toolStripButtonMaterialCategoryDelete_Click);
            // 
            // labelProject
            // 
            this.labelProject.AutoSize = true;
            this.labelProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProject.Location = new System.Drawing.Point(3, 237);
            this.labelProject.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelProject.Name = "labelProject";
            this.labelProject.Size = new System.Drawing.Size(64, 35);
            this.labelProject.TabIndex = 16;
            this.labelProject.Text = "Projects:";
            this.labelProject.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // listBoxProjects
            // 
            this.tableLayoutPanelAnalysis.SetColumnSpan(this.listBoxProjects, 2);
            this.listBoxProjects.DataSource = this.projectProcessingListBindingSource;
            this.listBoxProjects.DisplayMember = "Project";
            this.listBoxProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjects.FormattingEnabled = true;
            this.listBoxProjects.IntegralHeight = false;
            this.listBoxProjects.Location = new System.Drawing.Point(73, 231);
            this.listBoxProjects.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.listBoxProjects.Name = "listBoxProjects";
            this.listBoxProjects.Size = new System.Drawing.Size(330, 38);
            this.listBoxProjects.TabIndex = 17;
            this.listBoxProjects.ValueMember = "ProjectID";
            // 
            // projectProcessingListBindingSource
            // 
            this.projectProcessingListBindingSource.DataMember = "ProjectProcessingList";
            this.projectProcessingListBindingSource.DataSource = this.dataSetProcessing;
            // 
            // toolStripProjects
            // 
            this.toolStripProjects.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripProjects.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonProjectNew,
            this.toolStripButtonProjectRemove});
            this.toolStripProjects.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.toolStripProjects.Location = new System.Drawing.Point(403, 231);
            this.toolStripProjects.Name = "toolStripProjects";
            this.toolStripProjects.Size = new System.Drawing.Size(24, 41);
            this.toolStripProjects.TabIndex = 18;
            this.toolStripProjects.Text = "toolStrip1";
            // 
            // toolStripButtonProjectNew
            // 
            this.toolStripButtonProjectNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonProjectNew.Image = global::DiversityCollection.Resource.New1;
            this.toolStripButtonProjectNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonProjectNew.Name = "toolStripButtonProjectNew";
            this.toolStripButtonProjectNew.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonProjectNew.Text = "Add a new project";
            this.toolStripButtonProjectNew.Click += new System.EventHandler(this.toolStripButtonProjectNew_Click);
            // 
            // toolStripButtonProjectRemove
            // 
            this.toolStripButtonProjectRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonProjectRemove.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonProjectRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonProjectRemove.Name = "toolStripButtonProjectRemove";
            this.toolStripButtonProjectRemove.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonProjectRemove.Text = "Remove the selected project from the list";
            this.toolStripButtonProjectRemove.Click += new System.EventHandler(this.toolStripButtonProjectRemove_Click);
            // 
            // checkBoxOnlyHierarchy
            // 
            this.checkBoxOnlyHierarchy.AutoSize = true;
            this.tableLayoutPanelAnalysis.SetColumnSpan(this.checkBoxOnlyHierarchy, 2);
            this.checkBoxOnlyHierarchy.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.processingBindingSource, "OnlyHierarchy", true));
            this.checkBoxOnlyHierarchy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxOnlyHierarchy.Location = new System.Drawing.Point(316, 166);
            this.checkBoxOnlyHierarchy.Name = "checkBoxOnlyHierarchy";
            this.checkBoxOnlyHierarchy.Size = new System.Drawing.Size(108, 21);
            this.checkBoxOnlyHierarchy.TabIndex = 19;
            this.checkBoxOnlyHierarchy.Text = "Only for hierarchy";
            this.checkBoxOnlyHierarchy.UseVisualStyleBackColor = true;
            // 
            // labelMethods
            // 
            this.labelMethods.AutoSize = true;
            this.labelMethods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMethods.Location = new System.Drawing.Point(3, 272);
            this.labelMethods.Name = "labelMethods";
            this.labelMethods.Size = new System.Drawing.Size(64, 41);
            this.labelMethods.TabIndex = 20;
            this.labelMethods.Text = "Methods:";
            this.labelMethods.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // listBoxMethods
            // 
            this.tableLayoutPanelAnalysis.SetColumnSpan(this.listBoxMethods, 2);
            this.listBoxMethods.DataSource = this.methodForProcessingListBindingSource;
            this.listBoxMethods.DisplayMember = "DisplayText";
            this.listBoxMethods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxMethods.FormattingEnabled = true;
            this.listBoxMethods.Location = new System.Drawing.Point(73, 272);
            this.listBoxMethods.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.listBoxMethods.Name = "listBoxMethods";
            this.listBoxMethods.Size = new System.Drawing.Size(330, 38);
            this.listBoxMethods.TabIndex = 21;
            this.listBoxMethods.ValueMember = "MethodID";
            // 
            // methodForProcessingListBindingSource
            // 
            this.methodForProcessingListBindingSource.DataMember = "MethodForProcessingList";
            this.methodForProcessingListBindingSource.DataSource = this.dataSetProcessing;
            // 
            // toolStripMethods
            // 
            this.toolStripMethods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripMethods.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMethods.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonMethodsAdd,
            this.toolStripButtonMethodsDelete});
            this.toolStripMethods.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.toolStripMethods.Location = new System.Drawing.Point(403, 272);
            this.toolStripMethods.Name = "toolStripMethods";
            this.toolStripMethods.Size = new System.Drawing.Size(24, 41);
            this.toolStripMethods.TabIndex = 22;
            this.toolStripMethods.Text = "toolStrip1";
            // 
            // toolStripButtonMethodsAdd
            // 
            this.toolStripButtonMethodsAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMethodsAdd.Image = global::DiversityCollection.Resource.New;
            this.toolStripButtonMethodsAdd.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonMethodsAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMethodsAdd.Name = "toolStripButtonMethodsAdd";
            this.toolStripButtonMethodsAdd.Size = new System.Drawing.Size(23, 17);
            this.toolStripButtonMethodsAdd.Text = "Add a new method";
            this.toolStripButtonMethodsAdd.Click += new System.EventHandler(this.toolStripButtonMethodsAdd_Click);
            // 
            // toolStripButtonMethodsDelete
            // 
            this.toolStripButtonMethodsDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMethodsDelete.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonMethodsDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMethodsDelete.Name = "toolStripButtonMethodsDelete";
            this.toolStripButtonMethodsDelete.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonMethodsDelete.Text = "Remove the selected method";
            this.toolStripButtonMethodsDelete.Click += new System.EventHandler(this.toolStripButtonMethodsDelete_Click);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpProvider.SetHelpKeyword(this.splitContainerMain, "Processing");
            this.helpProvider.SetHelpNavigator(this.splitContainerMain, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.splitContainerMain.Location = new System.Drawing.Point(0, 48);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.userControlQueryList);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerData);
            this.helpProvider.SetShowHelp(this.splitContainerMain, true);
            this.splitContainerMain.Size = new System.Drawing.Size(810, 520);
            this.splitContainerMain.SplitterDistance = 211;
            this.splitContainerMain.TabIndex = 5;
            // 
            // userControlQueryList
            // 
            this.userControlQueryList.Connection = null;
            this.userControlQueryList.DisplayTextSelectedItem = "";
            this.userControlQueryList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlQueryList.IDisNumeric = true;
            this.userControlQueryList.ImageList = null;
            this.userControlQueryList.IsPredefinedQuery = false;
            this.userControlQueryList.Location = new System.Drawing.Point(0, 0);
            this.userControlQueryList.MaximalNumberOfResults = 100;
            this.userControlQueryList.Name = "userControlQueryList";
            this.userControlQueryList.ProjectID = -1;
            this.userControlQueryList.QueryConditionVisiblity = "";
            this.userControlQueryList.QueryDisplayColumns = null;
            this.userControlQueryList.QueryMainTableLocal = null;
            this.userControlQueryList.QueryRestriction = "";
            this.userControlQueryList.RememberQuerySettingsIdentifier = "QueryList";
            this.userControlQueryList.SelectedProjectID = null;
            this.userControlQueryList.Size = new System.Drawing.Size(211, 520);
            this.userControlQueryList.TabIndex = 2;
            this.userControlQueryList.TableColors = null;
            this.userControlQueryList.TableImageIndex = null;
            this.userControlQueryList.WhereClause = null;
            // 
            // splitContainerData
            // 
            this.splitContainerData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerData.Location = new System.Drawing.Point(0, 0);
            this.splitContainerData.Name = "splitContainerData";
            // 
            // splitContainerData.Panel1
            // 
            this.splitContainerData.Panel1.Controls.Add(this.splitContainerURI);
            // 
            // splitContainerData.Panel2
            // 
            this.splitContainerData.Panel2.Controls.Add(this.userControlSpecimenList);
            this.splitContainerData.Size = new System.Drawing.Size(595, 520);
            this.splitContainerData.SplitterDistance = 433;
            this.splitContainerData.TabIndex = 2;
            // 
            // splitContainerURI
            // 
            this.splitContainerURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerURI.Location = new System.Drawing.Point(0, 0);
            this.splitContainerURI.Name = "splitContainerURI";
            this.splitContainerURI.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerURI.Panel1
            // 
            this.splitContainerURI.Panel1.Controls.Add(this.groupBoxProcessing);
            // 
            // splitContainerURI.Panel2
            // 
            this.splitContainerURI.Panel2.Controls.Add(this.panelURI);
            this.splitContainerURI.Size = new System.Drawing.Size(433, 520);
            this.splitContainerURI.SplitterDistance = 360;
            this.splitContainerURI.TabIndex = 2;
            // 
            // panelURI
            // 
            this.panelURI.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelURI.Controls.Add(this.webBrowserURI);
            this.panelURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelURI.Location = new System.Drawing.Point(0, 0);
            this.panelURI.Name = "panelURI";
            this.panelURI.Size = new System.Drawing.Size(433, 156);
            this.panelURI.TabIndex = 12;
            // 
            // webBrowserURI
            // 
            this.webBrowserURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserURI.Location = new System.Drawing.Point(0, 0);
            this.webBrowserURI.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserURI.Name = "webBrowserURI";
            this.webBrowserURI.Size = new System.Drawing.Size(429, 152);
            this.webBrowserURI.TabIndex = 0;
            // 
            // userControlSpecimenList
            // 
            this.userControlSpecimenList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlSpecimenList.Location = new System.Drawing.Point(0, 0);
            this.userControlSpecimenList.Name = "userControlSpecimenList";
            this.userControlSpecimenList.Size = new System.Drawing.Size(158, 520);
            this.userControlSpecimenList.TabIndex = 0;
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.labelHeader);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 24);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(810, 24);
            this.panelHeader.TabIndex = 6;
            this.panelHeader.Visible = false;
            // 
            // imageListSpecimenList
            // 
            this.imageListSpecimenList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSpecimenList.ImageStream")));
            this.imageListSpecimenList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSpecimenList.Images.SetKeyName(0, "List.ico");
            this.imageListSpecimenList.Images.SetKeyName(1, "ListNot.ico");
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 568);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(810, 27);
            this.userControlDialogPanel.TabIndex = 4;
            this.userControlDialogPanel.Visible = false;
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem,
            this.historyToolStripMenuItem,
            this.toolStripMenuItemFeedback,
            this.tableEditorToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(810, 24);
            this.menuStripMain.TabIndex = 7;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showURIToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // showURIToolStripMenuItem
            // 
            this.showURIToolStripMenuItem.Checked = true;
            this.showURIToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showURIToolStripMenuItem.Name = "showURIToolStripMenuItem";
            this.showURIToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.showURIToolStripMenuItem.Text = "Show URI";
            this.showURIToolStripMenuItem.Click += new System.EventHandler(this.showURIToolStripMenuItem_Click);
            // 
            // historyToolStripMenuItem
            // 
            this.historyToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.historyToolStripMenuItem.Image = global::DiversityCollection.Resource.History;
            this.historyToolStripMenuItem.Name = "historyToolStripMenuItem";
            this.historyToolStripMenuItem.Size = new System.Drawing.Size(28, 20);
            this.historyToolStripMenuItem.ToolTipText = "Show the history of the current dataset";
            this.historyToolStripMenuItem.Click += new System.EventHandler(this.historyToolStripMenuItem_Click);
            // 
            // toolStripMenuItemFeedback
            // 
            this.toolStripMenuItemFeedback.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripMenuItemFeedback.Image = global::DiversityCollection.Resource.Feedback;
            this.toolStripMenuItemFeedback.Name = "toolStripMenuItemFeedback";
            this.toolStripMenuItemFeedback.Size = new System.Drawing.Size(28, 20);
            this.toolStripMenuItemFeedback.ToolTipText = "Send a feedback to the administrator";
            this.toolStripMenuItemFeedback.Click += new System.EventHandler(this.toolStripMenuItemFeedback_Click);
            // 
            // tableEditorToolStripMenuItem
            // 
            this.tableEditorToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tableEditorToolStripMenuItem.Image = global::DiversityCollection.Resource.EditInSpeadsheet;
            this.tableEditorToolStripMenuItem.Name = "tableEditorToolStripMenuItem";
            this.tableEditorToolStripMenuItem.Size = new System.Drawing.Size(28, 20);
            this.tableEditorToolStripMenuItem.ToolTipText = "Edit selected processings in table";
            this.tableEditorToolStripMenuItem.Click += new System.EventHandler(this.tableEditorToolStripMenuItem_Click);
            // 
            // projectProcessingListTableAdapter
            // 
            this.projectProcessingListTableAdapter.ClearBeforeFill = true;
            // 
            // methodForProcessingListTableAdapter
            // 
            this.methodForProcessingListTableAdapter.ClearBeforeFill = true;
            // 
            // FormProcessing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 595);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.userControlDialogPanel);
            this.Controls.Add(this.menuStripMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "FormProcessing";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Processing";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormProcessing_FormClosing);
            this.Load += new System.EventHandler(this.FormProcessing_Load);
            ((System.ComponentModel.ISupportInitialize)(this.processingBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetProcessing)).EndInit();
            this.groupBoxProcessing.ResumeLayout(false);
            this.tableLayoutPanelAnalysis.ResumeLayout(false);
            this.tableLayoutPanelAnalysis.PerformLayout();
            this.toolStripProcessing.ResumeLayout(false);
            this.toolStripProcessing.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.processingMaterialCategoryBindingSource)).EndInit();
            this.toolStripMaterialCategory.ResumeLayout(false);
            this.toolStripMaterialCategory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.projectProcessingListBindingSource)).EndInit();
            this.toolStripProjects.ResumeLayout(false);
            this.toolStripProjects.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.methodForProcessingListBindingSource)).EndInit();
            this.toolStripMethods.ResumeLayout(false);
            this.toolStripMethods.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerData.Panel1.ResumeLayout(false);
            this.splitContainerData.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerData)).EndInit();
            this.splitContainerData.ResumeLayout(false);
            this.splitContainerURI.Panel1.ResumeLayout(false);
            this.splitContainerURI.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerURI)).EndInit();
            this.splitContainerURI.ResumeLayout(false);
            this.panelURI.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label labelNotes;
        private System.Windows.Forms.TextBox textBoxNotes;
        private System.Windows.Forms.Label labelURI;
        private System.Windows.Forms.TextBox textBoxURI;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Button buttonUriOpen;
        private System.Windows.Forms.GroupBox groupBoxProcessing;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAnalysis;
        private System.Windows.Forms.TreeView treeViewProcessing;
        private System.Windows.Forms.ToolStrip toolStripProcessing;
        private System.Windows.Forms.ToolStripButton toolStripButtonNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.Label labelDisplayText;
        private System.Windows.Forms.TextBox textBoxDisplayText;
        private DiversityWorkbench.UserControls.UserControlQueryList userControlQueryList;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Panel panelHeader;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.BindingSource processingBindingSource;
        private Datasets.DataSetProcessing dataSetProcessing;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.SplitContainer splitContainerData;
        private DiversityCollection.UserControls.UserControlSpecimenList userControlSpecimenList;
        private System.Windows.Forms.ToolStripButton toolStripButtonSpecimenList;
        private System.Windows.Forms.ImageList imageListSpecimenList;
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainerURI;
        private System.Windows.Forms.Panel panelURI;
        private System.Windows.Forms.WebBrowser webBrowserURI;
        private System.Windows.Forms.Label labelMaterialCategory;
        private System.Windows.Forms.ListBox listBoxMaterialCategory;
        private System.Windows.Forms.ToolStrip toolStripMaterialCategory;
        private System.Windows.Forms.ToolStripButton toolStripButtonMaterialCategoryNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonMaterialCategoryDelete;
        private System.Windows.Forms.ToolStripMenuItem showURIToolStripMenuItem;
        private System.Windows.Forms.BindingSource processingMaterialCategoryBindingSource;
        private System.Windows.Forms.ToolStripButton toolStripButtonIncludeID;
        private System.Windows.Forms.Label labelProject;
        private System.Windows.Forms.ListBox listBoxProjects;
        private System.Windows.Forms.ToolStrip toolStripProjects;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectRemove;
        private System.Windows.Forms.BindingSource projectProcessingListBindingSource;
        private DiversityCollection.Datasets.DataSetProcessingTableAdapters.ProjectProcessingListTableAdapter projectProcessingListTableAdapter;
        private System.Windows.Forms.CheckBox checkBoxOnlyHierarchy;
        private System.Windows.Forms.ToolStripMenuItem historyToolStripMenuItem;
        private System.Windows.Forms.Label labelMethods;
        private System.Windows.Forms.ListBox listBoxMethods;
        private System.Windows.Forms.ToolStrip toolStripMethods;
        private System.Windows.Forms.ToolStripButton toolStripButtonMethodsAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonMethodsDelete;
        private System.Windows.Forms.BindingSource methodForProcessingListBindingSource;
        private Datasets.DataSetProcessingTableAdapters.MethodForProcessingListTableAdapter methodForProcessingListTableAdapter;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFeedback;
        private System.Windows.Forms.ToolStripMenuItem tableEditorToolStripMenuItem;
    }
}