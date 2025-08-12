namespace DiversityCollection.Forms
{
    partial class FormMethod
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMethod));
            this.labelHeader = new System.Windows.Forms.Label();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.userControlQueryList = new DiversityWorkbench.UserControls.UserControlQueryList();
            this.splitContainerData = new System.Windows.Forms.SplitContainer();
            this.splitContainerURI = new System.Windows.Forms.SplitContainer();
            this.groupBoxMethod = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelTool = new System.Windows.Forms.TableLayoutPanel();
            this.treeViewMethod = new System.Windows.Forms.TreeView();
            this.toolStripTool = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonIncludeID = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSetParent = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRemoveParent = new System.Windows.Forms.ToolStripButton();
            this.labelDisplayText = new System.Windows.Forms.Label();
            this.textBoxDisplayText = new System.Windows.Forms.TextBox();
            this.methodBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetMethod = new DiversityCollection.Datasets.DataSetMethod();
            this.labelDescription = new System.Windows.Forms.Label();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelNotes = new System.Windows.Forms.Label();
            this.textBoxNotes = new System.Windows.Forms.TextBox();
            this.labelURI = new System.Windows.Forms.Label();
            this.textBoxURI = new System.Windows.Forms.TextBox();
            this.buttonUriOpen = new System.Windows.Forms.Button();
            this.checkBoxOnlyHierarchy = new System.Windows.Forms.CheckBox();
            this.checkBoxForCollectionEvent = new System.Windows.Forms.CheckBox();
            this.groupBoxAnalysis = new System.Windows.Forms.GroupBox();
            this.listBoxAnalysis = new System.Windows.Forms.ListBox();
            this.methodForAnalysisListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStripAnalysis = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAnalysisAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAnalysisRemove = new System.Windows.Forms.ToolStripButton();
            this.groupBoxProcessing = new System.Windows.Forms.GroupBox();
            this.listBoxProcessing = new System.Windows.Forms.ListBox();
            this.methodForProcessingListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStripProcessing = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonProcessingAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonProcessingRemove = new System.Windows.Forms.ToolStripButton();
            this.groupBoxParameter = new System.Windows.Forms.GroupBox();
            this.splitContainerParameter = new System.Windows.Forms.SplitContainer();
            this.listBoxParameter = new System.Windows.Forms.ListBox();
            this.parameterBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStripParameter = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonParameterAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonParameterDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonParameterSave = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanelParameter = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxParameterDisplayText = new System.Windows.Forms.TextBox();
            this.labelParameterDescription = new System.Windows.Forms.Label();
            this.textBoxParameterDescription = new System.Windows.Forms.TextBox();
            this.labelParameterURI = new System.Windows.Forms.Label();
            this.buttonParameterURI = new System.Windows.Forms.Button();
            this.textBoxParameterURI = new System.Windows.Forms.TextBox();
            this.labelParameterDefaultValue = new System.Windows.Forms.Label();
            this.labelParameterNotes = new System.Windows.Forms.Label();
            this.textBoxParameterNotes = new System.Windows.Forms.TextBox();
            this.checkBoxUseDedicatedParameterValues = new System.Windows.Forms.CheckBox();
            this.groupBoxParameterValues = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelParameterValues = new System.Windows.Forms.TableLayoutPanel();
            this.listBoxParameterValue = new System.Windows.Forms.ListBox();
            this.parameterValueEnumBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStripParameterValues = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonParameterValueAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonParameterValueRemove = new System.Windows.Forms.ToolStripButton();
            this.labelParameterValueDisplayText = new System.Windows.Forms.Label();
            this.textBoxParameterValueDisplayText = new System.Windows.Forms.TextBox();
            this.labelParameterValue = new System.Windows.Forms.Label();
            this.textBoxParameterValue = new System.Windows.Forms.TextBox();
            this.labelParameterValueUri = new System.Windows.Forms.Label();
            this.textBoxParameterValueUri = new System.Windows.Forms.TextBox();
            this.buttonParameterValueUri = new System.Windows.Forms.Button();
            this.labelParameterValueDescription = new System.Windows.Forms.Label();
            this.textBoxParameterValueDescription = new System.Windows.Forms.TextBox();
            this.panelDefaultValue = new System.Windows.Forms.Panel();
            this.textBoxParameterDefaultValue = new System.Windows.Forms.TextBox();
            this.comboBoxParameterDefaultValue = new System.Windows.Forms.ComboBox();
            this.labelParameterID = new System.Windows.Forms.Label();
            this.textBoxParameterID = new System.Windows.Forms.TextBox();
            this.panelURI = new System.Windows.Forms.Panel();
            this.webBrowserURI = new System.Windows.Forms.WebBrowser();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showURIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.feedbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.methodTableAdapter = new DiversityCollection.Datasets.DataSetMethodTableAdapters.MethodTableAdapter();
            this.parameterTableAdapter = new DiversityCollection.Datasets.DataSetMethodTableAdapters.ParameterTableAdapter();
            this.parameterValue_EnumTableAdapter = new DiversityCollection.Datasets.DataSetMethodTableAdapters.ParameterValue_EnumTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerData)).BeginInit();
            this.splitContainerData.Panel1.SuspendLayout();
            this.splitContainerData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerURI)).BeginInit();
            this.splitContainerURI.Panel1.SuspendLayout();
            this.splitContainerURI.Panel2.SuspendLayout();
            this.splitContainerURI.SuspendLayout();
            this.groupBoxMethod.SuspendLayout();
            this.tableLayoutPanelTool.SuspendLayout();
            this.toolStripTool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.methodBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetMethod)).BeginInit();
            this.groupBoxAnalysis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.methodForAnalysisListBindingSource)).BeginInit();
            this.toolStripAnalysis.SuspendLayout();
            this.groupBoxProcessing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.methodForProcessingListBindingSource)).BeginInit();
            this.toolStripProcessing.SuspendLayout();
            this.groupBoxParameter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerParameter)).BeginInit();
            this.splitContainerParameter.Panel1.SuspendLayout();
            this.splitContainerParameter.Panel2.SuspendLayout();
            this.splitContainerParameter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parameterBindingSource)).BeginInit();
            this.toolStripParameter.SuspendLayout();
            this.tableLayoutPanelParameter.SuspendLayout();
            this.groupBoxParameterValues.SuspendLayout();
            this.tableLayoutPanelParameterValues.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parameterValueEnumBindingSource)).BeginInit();
            this.toolStripParameterValues.SuspendLayout();
            this.panelDefaultValue.SuspendLayout();
            this.panelURI.SuspendLayout();
            this.menuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelHeader
            // 
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelHeader.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelHeader.Location = new System.Drawing.Point(0, 24);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(906, 24);
            this.labelHeader.TabIndex = 7;
            this.labelHeader.Text = "Select a method";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelHeader.Visible = false;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 651);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(906, 27);
            this.userControlDialogPanel.TabIndex = 8;
            this.userControlDialogPanel.Visible = false;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpProvider.SetHelpNavigator(this.splitContainerMain, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.helpProvider.SetHelpString(this.splitContainerMain, "Method");
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
            this.splitContainerMain.Size = new System.Drawing.Size(906, 603);
            this.splitContainerMain.SplitterDistance = 251;
            this.splitContainerMain.TabIndex = 9;
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
            this.userControlQueryList.Size = new System.Drawing.Size(251, 603);
            this.userControlQueryList.TabIndex = 0;
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
            this.splitContainerData.Panel2Collapsed = true;
            this.splitContainerData.Size = new System.Drawing.Size(651, 603);
            this.splitContainerData.SplitterDistance = 361;
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
            this.splitContainerURI.Panel1.Controls.Add(this.groupBoxMethod);
            // 
            // splitContainerURI.Panel2
            // 
            this.splitContainerURI.Panel2.Controls.Add(this.panelURI);
            this.splitContainerURI.Size = new System.Drawing.Size(651, 603);
            this.splitContainerURI.SplitterDistance = 517;
            this.splitContainerURI.TabIndex = 2;
            // 
            // groupBoxMethod
            // 
            this.groupBoxMethod.Controls.Add(this.tableLayoutPanelTool);
            this.groupBoxMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxMethod.Location = new System.Drawing.Point(0, 0);
            this.groupBoxMethod.Name = "groupBoxMethod";
            this.groupBoxMethod.Size = new System.Drawing.Size(651, 517);
            this.groupBoxMethod.TabIndex = 1;
            this.groupBoxMethod.TabStop = false;
            this.groupBoxMethod.Text = "Method";
            // 
            // tableLayoutPanelTool
            // 
            this.tableLayoutPanelTool.ColumnCount = 6;
            this.tableLayoutPanelTool.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTool.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanelTool.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelTool.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTool.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTool.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTool.Controls.Add(this.treeViewMethod, 0, 0);
            this.tableLayoutPanelTool.Controls.Add(this.toolStripTool, 5, 0);
            this.tableLayoutPanelTool.Controls.Add(this.labelDisplayText, 0, 1);
            this.tableLayoutPanelTool.Controls.Add(this.textBoxDisplayText, 1, 1);
            this.tableLayoutPanelTool.Controls.Add(this.labelDescription, 0, 2);
            this.tableLayoutPanelTool.Controls.Add(this.textBoxDescription, 1, 2);
            this.tableLayoutPanelTool.Controls.Add(this.labelNotes, 0, 3);
            this.tableLayoutPanelTool.Controls.Add(this.textBoxNotes, 1, 3);
            this.tableLayoutPanelTool.Controls.Add(this.labelURI, 0, 6);
            this.tableLayoutPanelTool.Controls.Add(this.textBoxURI, 1, 6);
            this.tableLayoutPanelTool.Controls.Add(this.buttonUriOpen, 5, 6);
            this.tableLayoutPanelTool.Controls.Add(this.checkBoxOnlyHierarchy, 4, 1);
            this.tableLayoutPanelTool.Controls.Add(this.checkBoxForCollectionEvent, 3, 1);
            this.tableLayoutPanelTool.Controls.Add(this.groupBoxAnalysis, 0, 5);
            this.tableLayoutPanelTool.Controls.Add(this.groupBoxProcessing, 2, 5);
            this.tableLayoutPanelTool.Controls.Add(this.groupBoxParameter, 0, 4);
            this.tableLayoutPanelTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpProvider.SetHelpNavigator(this.tableLayoutPanelTool, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.helpProvider.SetHelpString(this.tableLayoutPanelTool, "Method");
            this.tableLayoutPanelTool.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelTool.Name = "tableLayoutPanelTool";
            this.tableLayoutPanelTool.RowCount = 7;
            this.tableLayoutPanelTool.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanelTool.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTool.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTool.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTool.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanelTool.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelTool.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.helpProvider.SetShowHelp(this.tableLayoutPanelTool, true);
            this.tableLayoutPanelTool.Size = new System.Drawing.Size(645, 498);
            this.tableLayoutPanelTool.TabIndex = 0;
            // 
            // treeViewMethod
            // 
            this.treeViewMethod.AllowDrop = true;
            this.tableLayoutPanelTool.SetColumnSpan(this.treeViewMethod, 5);
            this.treeViewMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewMethod.Location = new System.Drawing.Point(3, 3);
            this.treeViewMethod.Name = "treeViewMethod";
            this.treeViewMethod.Size = new System.Drawing.Size(583, 148);
            this.treeViewMethod.TabIndex = 0;
            // 
            // toolStripTool
            // 
            this.toolStripTool.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStripTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNew,
            this.toolStripButtonDelete,
            this.toolStripButtonIncludeID,
            this.toolStripButtonSetParent,
            this.toolStripButtonRemoveParent});
            this.toolStripTool.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.toolStripTool.Location = new System.Drawing.Point(620, 0);
            this.toolStripTool.Name = "toolStripTool";
            this.toolStripTool.Size = new System.Drawing.Size(25, 154);
            this.toolStripTool.TabIndex = 1;
            this.toolStripTool.Text = "toolStrip1";
            // 
            // toolStripButtonNew
            // 
            this.toolStripButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNew.Image = global::DiversityCollection.Resource.New1;
            this.toolStripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNew.Name = "toolStripButtonNew";
            this.toolStripButtonNew.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonNew.Text = "Insert a new method dependent on the selected method";
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDelete.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonDelete.Text = "Delete the selected method";
            // 
            // toolStripButtonIncludeID
            // 
            this.toolStripButtonIncludeID.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonIncludeID.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.toolStripButtonIncludeID.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripButtonIncludeID.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonIncludeID.Name = "toolStripButtonIncludeID";
            this.toolStripButtonIncludeID.Size = new System.Drawing.Size(24, 17);
            this.toolStripButtonIncludeID.Text = "ID";
            this.toolStripButtonIncludeID.ToolTipText = "Display the ID of the datasets in the tree";
            // 
            // toolStripButtonSetParent
            // 
            this.toolStripButtonSetParent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSetParent.Image = global::DiversityCollection.Resource.SetParent;
            this.toolStripButtonSetParent.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonSetParent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSetParent.Name = "toolStripButtonSetParent";
            this.toolStripButtonSetParent.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonSetParent.Text = "Set the parent for the selected method";
            // 
            // toolStripButtonRemoveParent
            // 
            this.toolStripButtonRemoveParent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRemoveParent.Image = global::DiversityCollection.Resource.RemoveParent;
            this.toolStripButtonRemoveParent.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonRemoveParent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemoveParent.Name = "toolStripButtonRemoveParent";
            this.toolStripButtonRemoveParent.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonRemoveParent.Text = "Remove the parent of the selected method";
            // 
            // labelDisplayText
            // 
            this.labelDisplayText.AutoSize = true;
            this.labelDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDisplayText.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelDisplayText.Location = new System.Drawing.Point(3, 160);
            this.labelDisplayText.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelDisplayText.Name = "labelDisplayText";
            this.labelDisplayText.Size = new System.Drawing.Size(65, 20);
            this.labelDisplayText.TabIndex = 2;
            this.labelDisplayText.Text = "DisplayText:";
            this.labelDisplayText.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxDisplayText
            // 
            this.tableLayoutPanelTool.SetColumnSpan(this.textBoxDisplayText, 2);
            this.textBoxDisplayText.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.methodBindingSource, "DisplayText", true));
            this.textBoxDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDisplayText.Location = new System.Drawing.Point(68, 157);
            this.textBoxDisplayText.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxDisplayText.Name = "textBoxDisplayText";
            this.textBoxDisplayText.Size = new System.Drawing.Size(334, 20);
            this.textBoxDisplayText.TabIndex = 3;
            // 
            // methodBindingSource
            // 
            this.methodBindingSource.DataMember = "Method";
            this.methodBindingSource.DataSource = this.dataSetMethod;
            // 
            // dataSetMethod
            // 
            this.dataSetMethod.DataSetName = "DataSetMethod";
            this.dataSetMethod.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDescription.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelDescription.Location = new System.Drawing.Point(3, 186);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(65, 23);
            this.labelDescription.TabIndex = 4;
            this.labelDescription.Text = "Description:";
            this.labelDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxDescription
            // 
            this.tableLayoutPanelTool.SetColumnSpan(this.textBoxDescription, 5);
            this.textBoxDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.methodBindingSource, "Description", true));
            this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDescription.Location = new System.Drawing.Point(68, 183);
            this.textBoxDescription.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(574, 23);
            this.textBoxDescription.TabIndex = 5;
            // 
            // labelNotes
            // 
            this.labelNotes.AutoSize = true;
            this.labelNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNotes.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelNotes.Location = new System.Drawing.Point(3, 215);
            this.labelNotes.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelNotes.Name = "labelNotes";
            this.labelNotes.Size = new System.Drawing.Size(65, 23);
            this.labelNotes.TabIndex = 6;
            this.labelNotes.Text = "Notes:";
            this.labelNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxNotes
            // 
            this.tableLayoutPanelTool.SetColumnSpan(this.textBoxNotes, 5);
            this.textBoxNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.methodBindingSource, "Notes", true));
            this.textBoxNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNotes.Location = new System.Drawing.Point(68, 212);
            this.textBoxNotes.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxNotes.Multiline = true;
            this.textBoxNotes.Name = "textBoxNotes";
            this.textBoxNotes.Size = new System.Drawing.Size(574, 23);
            this.textBoxNotes.TabIndex = 7;
            // 
            // labelURI
            // 
            this.labelURI.AutoSize = true;
            this.labelURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelURI.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelURI.Location = new System.Drawing.Point(3, 469);
            this.labelURI.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelURI.Name = "labelURI";
            this.labelURI.Size = new System.Drawing.Size(65, 29);
            this.labelURI.TabIndex = 8;
            this.labelURI.Text = "URI:";
            this.labelURI.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxURI
            // 
            this.tableLayoutPanelTool.SetColumnSpan(this.textBoxURI, 4);
            this.textBoxURI.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.methodBindingSource, "MethodURI", true));
            this.textBoxURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxURI.Location = new System.Drawing.Point(68, 472);
            this.textBoxURI.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.textBoxURI.Name = "textBoxURI";
            this.textBoxURI.Size = new System.Drawing.Size(521, 20);
            this.textBoxURI.TabIndex = 9;
            this.textBoxURI.TextChanged += new System.EventHandler(this.textBoxURI_TextChanged);
            // 
            // buttonUriOpen
            // 
            this.buttonUriOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonUriOpen.Image = global::DiversityCollection.Resource.Browse;
            this.buttonUriOpen.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonUriOpen.Location = new System.Drawing.Point(589, 469);
            this.buttonUriOpen.Margin = new System.Windows.Forms.Padding(0);
            this.buttonUriOpen.Name = "buttonUriOpen";
            this.buttonUriOpen.Size = new System.Drawing.Size(56, 29);
            this.buttonUriOpen.TabIndex = 10;
            this.buttonUriOpen.UseVisualStyleBackColor = true;
            this.buttonUriOpen.Click += new System.EventHandler(this.buttonUriOpen_Click);
            // 
            // checkBoxOnlyHierarchy
            // 
            this.checkBoxOnlyHierarchy.AutoSize = true;
            this.tableLayoutPanelTool.SetColumnSpan(this.checkBoxOnlyHierarchy, 2);
            this.checkBoxOnlyHierarchy.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.methodBindingSource, "OnlyHierarchy", true));
            this.checkBoxOnlyHierarchy.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxOnlyHierarchy.Location = new System.Drawing.Point(534, 157);
            this.checkBoxOnlyHierarchy.Name = "checkBoxOnlyHierarchy";
            this.checkBoxOnlyHierarchy.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxOnlyHierarchy.Size = new System.Drawing.Size(108, 20);
            this.checkBoxOnlyHierarchy.TabIndex = 23;
            this.checkBoxOnlyHierarchy.Text = "Only for hierarchy";
            this.checkBoxOnlyHierarchy.UseVisualStyleBackColor = true;
            // 
            // checkBoxForCollectionEvent
            // 
            this.checkBoxForCollectionEvent.AutoSize = true;
            this.checkBoxForCollectionEvent.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.methodBindingSource, "ForCollectionEvent", true));
            this.checkBoxForCollectionEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxForCollectionEvent.Location = new System.Drawing.Point(408, 157);
            this.checkBoxForCollectionEvent.Name = "checkBoxForCollectionEvent";
            this.checkBoxForCollectionEvent.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxForCollectionEvent.Size = new System.Drawing.Size(119, 20);
            this.checkBoxForCollectionEvent.TabIndex = 28;
            this.checkBoxForCollectionEvent.Text = "For collection event";
            this.checkBoxForCollectionEvent.UseVisualStyleBackColor = true;
            // 
            // groupBoxAnalysis
            // 
            this.tableLayoutPanelTool.SetColumnSpan(this.groupBoxAnalysis, 2);
            this.groupBoxAnalysis.Controls.Add(this.listBoxAnalysis);
            this.groupBoxAnalysis.Controls.Add(this.toolStripAnalysis);
            this.groupBoxAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxAnalysis.Location = new System.Drawing.Point(3, 395);
            this.groupBoxAnalysis.Name = "groupBoxAnalysis";
            this.groupBoxAnalysis.Size = new System.Drawing.Size(315, 71);
            this.groupBoxAnalysis.TabIndex = 29;
            this.groupBoxAnalysis.TabStop = false;
            this.groupBoxAnalysis.Text = "Analysis";
            // 
            // listBoxAnalysis
            // 
            this.listBoxAnalysis.DataSource = this.methodForAnalysisListBindingSource;
            this.listBoxAnalysis.DisplayMember = "DisplayText";
            this.listBoxAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxAnalysis.FormattingEnabled = true;
            this.listBoxAnalysis.IntegralHeight = false;
            this.listBoxAnalysis.Location = new System.Drawing.Point(27, 16);
            this.listBoxAnalysis.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.listBoxAnalysis.Name = "listBoxAnalysis";
            this.listBoxAnalysis.Size = new System.Drawing.Size(285, 52);
            this.listBoxAnalysis.TabIndex = 1;
            this.listBoxAnalysis.ValueMember = "AnalysisID";
            // 
            // methodForAnalysisListBindingSource
            // 
            this.methodForAnalysisListBindingSource.DataMember = "MethodForAnalysisList";
            this.methodForAnalysisListBindingSource.DataSource = this.dataSetMethod;
            // 
            // toolStripAnalysis
            // 
            this.toolStripAnalysis.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStripAnalysis.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripAnalysis.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAnalysisAdd,
            this.toolStripButtonAnalysisRemove});
            this.toolStripAnalysis.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.toolStripAnalysis.Location = new System.Drawing.Point(3, 16);
            this.toolStripAnalysis.Name = "toolStripAnalysis";
            this.toolStripAnalysis.Size = new System.Drawing.Size(24, 52);
            this.toolStripAnalysis.TabIndex = 0;
            this.toolStripAnalysis.Text = "toolStrip1";
            // 
            // toolStripButtonAnalysisAdd
            // 
            this.toolStripButtonAnalysisAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAnalysisAdd.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonAnalysisAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAnalysisAdd.Name = "toolStripButtonAnalysisAdd";
            this.toolStripButtonAnalysisAdd.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonAnalysisAdd.Text = "Add an analysis";
            this.toolStripButtonAnalysisAdd.Click += new System.EventHandler(this.toolStripButtonAnalysisAdd_Click);
            // 
            // toolStripButtonAnalysisRemove
            // 
            this.toolStripButtonAnalysisRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAnalysisRemove.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonAnalysisRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAnalysisRemove.Name = "toolStripButtonAnalysisRemove";
            this.toolStripButtonAnalysisRemove.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonAnalysisRemove.Text = "Remove the selected analysis";
            this.toolStripButtonAnalysisRemove.Click += new System.EventHandler(this.toolStripButtonAnalysisRemove_Click);
            // 
            // groupBoxProcessing
            // 
            this.tableLayoutPanelTool.SetColumnSpan(this.groupBoxProcessing, 4);
            this.groupBoxProcessing.Controls.Add(this.listBoxProcessing);
            this.groupBoxProcessing.Controls.Add(this.toolStripProcessing);
            this.groupBoxProcessing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxProcessing.Location = new System.Drawing.Point(324, 395);
            this.groupBoxProcessing.Name = "groupBoxProcessing";
            this.groupBoxProcessing.Size = new System.Drawing.Size(318, 71);
            this.groupBoxProcessing.TabIndex = 30;
            this.groupBoxProcessing.TabStop = false;
            this.groupBoxProcessing.Text = "Processing";
            // 
            // listBoxProcessing
            // 
            this.listBoxProcessing.DataSource = this.methodForProcessingListBindingSource;
            this.listBoxProcessing.DisplayMember = "DisplayText";
            this.listBoxProcessing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProcessing.FormattingEnabled = true;
            this.listBoxProcessing.IntegralHeight = false;
            this.listBoxProcessing.Location = new System.Drawing.Point(27, 16);
            this.listBoxProcessing.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.listBoxProcessing.Name = "listBoxProcessing";
            this.listBoxProcessing.Size = new System.Drawing.Size(288, 52);
            this.listBoxProcessing.TabIndex = 2;
            this.listBoxProcessing.ValueMember = "ProcessingID";
            // 
            // methodForProcessingListBindingSource
            // 
            this.methodForProcessingListBindingSource.DataMember = "MethodForProcessingList";
            this.methodForProcessingListBindingSource.DataSource = this.dataSetMethod;
            // 
            // toolStripProcessing
            // 
            this.toolStripProcessing.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStripProcessing.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripProcessing.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonProcessingAdd,
            this.toolStripButtonProcessingRemove});
            this.toolStripProcessing.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.toolStripProcessing.Location = new System.Drawing.Point(3, 16);
            this.toolStripProcessing.Name = "toolStripProcessing";
            this.toolStripProcessing.Size = new System.Drawing.Size(24, 52);
            this.toolStripProcessing.TabIndex = 1;
            this.toolStripProcessing.Text = "toolStrip1";
            // 
            // toolStripButtonProcessingAdd
            // 
            this.toolStripButtonProcessingAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonProcessingAdd.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonProcessingAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonProcessingAdd.Name = "toolStripButtonProcessingAdd";
            this.toolStripButtonProcessingAdd.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonProcessingAdd.Text = "Add a processing";
            this.toolStripButtonProcessingAdd.Click += new System.EventHandler(this.toolStripButtonProcessingAdd_Click);
            // 
            // toolStripButtonProcessingRemove
            // 
            this.toolStripButtonProcessingRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonProcessingRemove.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonProcessingRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonProcessingRemove.Name = "toolStripButtonProcessingRemove";
            this.toolStripButtonProcessingRemove.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonProcessingRemove.Text = "Remove the selected processing";
            this.toolStripButtonProcessingRemove.Click += new System.EventHandler(this.toolStripButtonProcessingRemove_Click);
            // 
            // groupBoxParameter
            // 
            this.tableLayoutPanelTool.SetColumnSpan(this.groupBoxParameter, 6);
            this.groupBoxParameter.Controls.Add(this.splitContainerParameter);
            this.groupBoxParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxParameter.Location = new System.Drawing.Point(3, 241);
            this.groupBoxParameter.Name = "groupBoxParameter";
            this.groupBoxParameter.Size = new System.Drawing.Size(639, 148);
            this.groupBoxParameter.TabIndex = 32;
            this.groupBoxParameter.TabStop = false;
            this.groupBoxParameter.Text = "Parameter";
            // 
            // splitContainerParameter
            // 
            this.splitContainerParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerParameter.Location = new System.Drawing.Point(3, 16);
            this.splitContainerParameter.Name = "splitContainerParameter";
            // 
            // splitContainerParameter.Panel1
            // 
            this.splitContainerParameter.Panel1.Controls.Add(this.listBoxParameter);
            this.splitContainerParameter.Panel1.Controls.Add(this.toolStripParameter);
            // 
            // splitContainerParameter.Panel2
            // 
            this.splitContainerParameter.Panel2.Controls.Add(this.tableLayoutPanelParameter);
            this.splitContainerParameter.Size = new System.Drawing.Size(633, 129);
            this.splitContainerParameter.SplitterDistance = 211;
            this.splitContainerParameter.TabIndex = 1;
            // 
            // listBoxParameter
            // 
            this.listBoxParameter.DataSource = this.parameterBindingSource;
            this.listBoxParameter.DisplayMember = "DisplayText";
            this.listBoxParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxParameter.FormattingEnabled = true;
            this.listBoxParameter.IntegralHeight = false;
            this.listBoxParameter.Location = new System.Drawing.Point(0, 0);
            this.listBoxParameter.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.listBoxParameter.Name = "listBoxParameter";
            this.listBoxParameter.Size = new System.Drawing.Size(211, 104);
            this.listBoxParameter.TabIndex = 25;
            this.listBoxParameter.ValueMember = "ParameterID";
            this.listBoxParameter.SelectedIndexChanged += new System.EventHandler(this.listBoxParameter_SelectedIndexChanged);
            this.listBoxParameter.DataSourceChanged += new System.EventHandler(this.listBoxParameter_DataSourceChanged);
            // 
            // parameterBindingSource
            // 
            this.parameterBindingSource.DataMember = "Parameter";
            this.parameterBindingSource.DataSource = this.dataSetMethod;
            // 
            // toolStripParameter
            // 
            this.toolStripParameter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripParameter.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripParameter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonParameterAdd,
            this.toolStripButtonParameterDelete,
            this.toolStripButtonParameterSave});
            this.toolStripParameter.Location = new System.Drawing.Point(0, 104);
            this.toolStripParameter.Name = "toolStripParameter";
            this.toolStripParameter.Size = new System.Drawing.Size(211, 25);
            this.toolStripParameter.TabIndex = 26;
            this.toolStripParameter.Text = "toolStrip1";
            // 
            // toolStripButtonParameterAdd
            // 
            this.toolStripButtonParameterAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonParameterAdd.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonParameterAdd.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonParameterAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonParameterAdd.Name = "toolStripButtonParameterAdd";
            this.toolStripButtonParameterAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonParameterAdd.Text = "Add a new parameter";
            this.toolStripButtonParameterAdd.Click += new System.EventHandler(this.toolStripButtonParameterAdd_Click);
            // 
            // toolStripButtonParameterDelete
            // 
            this.toolStripButtonParameterDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonParameterDelete.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonParameterDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonParameterDelete.Name = "toolStripButtonParameterDelete";
            this.toolStripButtonParameterDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonParameterDelete.Text = "Delete the selected parameter";
            this.toolStripButtonParameterDelete.Click += new System.EventHandler(this.toolStripButtonParameterDelete_Click);
            // 
            // toolStripButtonParameterSave
            // 
            this.toolStripButtonParameterSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonParameterSave.Image = global::DiversityCollection.Resource.Save;
            this.toolStripButtonParameterSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonParameterSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonParameterSave.Name = "toolStripButtonParameterSave";
            this.toolStripButtonParameterSave.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonParameterSave.Text = "Save the changes to the parameter";
            this.toolStripButtonParameterSave.Click += new System.EventHandler(this.toolStripButtonParameterSave_Click);
            // 
            // tableLayoutPanelParameter
            // 
            this.tableLayoutPanelParameter.ColumnCount = 7;
            this.tableLayoutPanelParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanelParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanelParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanelParameter.Controls.Add(this.textBoxParameterDisplayText, 1, 0);
            this.tableLayoutPanelParameter.Controls.Add(this.labelParameterDescription, 1, 1);
            this.tableLayoutPanelParameter.Controls.Add(this.textBoxParameterDescription, 3, 1);
            this.tableLayoutPanelParameter.Controls.Add(this.labelParameterURI, 1, 2);
            this.tableLayoutPanelParameter.Controls.Add(this.buttonParameterURI, 2, 2);
            this.tableLayoutPanelParameter.Controls.Add(this.textBoxParameterURI, 3, 2);
            this.tableLayoutPanelParameter.Controls.Add(this.labelParameterDefaultValue, 1, 3);
            this.tableLayoutPanelParameter.Controls.Add(this.labelParameterNotes, 1, 4);
            this.tableLayoutPanelParameter.Controls.Add(this.textBoxParameterNotes, 3, 4);
            this.tableLayoutPanelParameter.Controls.Add(this.checkBoxUseDedicatedParameterValues, 4, 0);
            this.tableLayoutPanelParameter.Controls.Add(this.groupBoxParameterValues, 4, 1);
            this.tableLayoutPanelParameter.Controls.Add(this.panelDefaultValue, 3, 3);
            this.tableLayoutPanelParameter.Controls.Add(this.labelParameterID, 5, 0);
            this.tableLayoutPanelParameter.Controls.Add(this.textBoxParameterID, 6, 0);
            this.tableLayoutPanelParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelParameter.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelParameter.Name = "tableLayoutPanelParameter";
            this.tableLayoutPanelParameter.RowCount = 5;
            this.tableLayoutPanelParameter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelParameter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelParameter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelParameter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelParameter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelParameter.Size = new System.Drawing.Size(418, 129);
            this.tableLayoutPanelParameter.TabIndex = 0;
            // 
            // textBoxParameterDisplayText
            // 
            this.tableLayoutPanelParameter.SetColumnSpan(this.textBoxParameterDisplayText, 3);
            this.textBoxParameterDisplayText.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.parameterBindingSource, "DisplayText", true));
            this.textBoxParameterDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxParameterDisplayText.Location = new System.Drawing.Point(3, 3);
            this.textBoxParameterDisplayText.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.textBoxParameterDisplayText.Name = "textBoxParameterDisplayText";
            this.textBoxParameterDisplayText.Size = new System.Drawing.Size(174, 20);
            this.textBoxParameterDisplayText.TabIndex = 28;
            // 
            // labelParameterDescription
            // 
            this.labelParameterDescription.AutoSize = true;
            this.tableLayoutPanelParameter.SetColumnSpan(this.labelParameterDescription, 2);
            this.labelParameterDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelParameterDescription.Location = new System.Drawing.Point(3, 29);
            this.labelParameterDescription.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelParameterDescription.Name = "labelParameterDescription";
            this.labelParameterDescription.Size = new System.Drawing.Size(63, 32);
            this.labelParameterDescription.TabIndex = 29;
            this.labelParameterDescription.Text = "Description:";
            this.labelParameterDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxParameterDescription
            // 
            this.textBoxParameterDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.parameterBindingSource, "Description", true));
            this.textBoxParameterDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxParameterDescription.Location = new System.Drawing.Point(69, 26);
            this.textBoxParameterDescription.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.textBoxParameterDescription.Multiline = true;
            this.textBoxParameterDescription.Name = "textBoxParameterDescription";
            this.textBoxParameterDescription.Size = new System.Drawing.Size(108, 35);
            this.textBoxParameterDescription.TabIndex = 30;
            // 
            // labelParameterURI
            // 
            this.labelParameterURI.AutoSize = true;
            this.labelParameterURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelParameterURI.Location = new System.Drawing.Point(3, 61);
            this.labelParameterURI.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelParameterURI.Name = "labelParameterURI";
            this.labelParameterURI.Size = new System.Drawing.Size(39, 24);
            this.labelParameterURI.TabIndex = 31;
            this.labelParameterURI.Text = "URI:";
            this.labelParameterURI.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonParameterURI
            // 
            this.buttonParameterURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonParameterURI.Image = global::DiversityCollection.Resource.Browse;
            this.buttonParameterURI.Location = new System.Drawing.Point(42, 61);
            this.buttonParameterURI.Margin = new System.Windows.Forms.Padding(0);
            this.buttonParameterURI.Name = "buttonParameterURI";
            this.buttonParameterURI.Size = new System.Drawing.Size(24, 24);
            this.buttonParameterURI.TabIndex = 32;
            this.buttonParameterURI.UseVisualStyleBackColor = true;
            this.buttonParameterURI.Click += new System.EventHandler(this.buttonParameterURI_Click);
            // 
            // textBoxParameterURI
            // 
            this.textBoxParameterURI.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.parameterBindingSource, "ParameterURI", true));
            this.textBoxParameterURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxParameterURI.Location = new System.Drawing.Point(69, 64);
            this.textBoxParameterURI.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.textBoxParameterURI.Name = "textBoxParameterURI";
            this.textBoxParameterURI.Size = new System.Drawing.Size(108, 20);
            this.textBoxParameterURI.TabIndex = 33;
            // 
            // labelParameterDefaultValue
            // 
            this.labelParameterDefaultValue.AutoSize = true;
            this.tableLayoutPanelParameter.SetColumnSpan(this.labelParameterDefaultValue, 2);
            this.labelParameterDefaultValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelParameterDefaultValue.Location = new System.Drawing.Point(3, 85);
            this.labelParameterDefaultValue.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelParameterDefaultValue.Name = "labelParameterDefaultValue";
            this.labelParameterDefaultValue.Size = new System.Drawing.Size(63, 21);
            this.labelParameterDefaultValue.TabIndex = 34;
            this.labelParameterDefaultValue.Text = "Default:";
            this.labelParameterDefaultValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelParameterNotes
            // 
            this.labelParameterNotes.AutoSize = true;
            this.tableLayoutPanelParameter.SetColumnSpan(this.labelParameterNotes, 2);
            this.labelParameterNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelParameterNotes.Location = new System.Drawing.Point(3, 106);
            this.labelParameterNotes.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelParameterNotes.Name = "labelParameterNotes";
            this.labelParameterNotes.Size = new System.Drawing.Size(63, 23);
            this.labelParameterNotes.TabIndex = 36;
            this.labelParameterNotes.Text = "Notes:";
            this.labelParameterNotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxParameterNotes
            // 
            this.textBoxParameterNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.parameterBindingSource, "Notes", true));
            this.textBoxParameterNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxParameterNotes.Location = new System.Drawing.Point(69, 109);
            this.textBoxParameterNotes.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.textBoxParameterNotes.Name = "textBoxParameterNotes";
            this.textBoxParameterNotes.Size = new System.Drawing.Size(108, 20);
            this.textBoxParameterNotes.TabIndex = 37;
            // 
            // checkBoxUseDedicatedParameterValues
            // 
            this.checkBoxUseDedicatedParameterValues.AutoSize = true;
            this.checkBoxUseDedicatedParameterValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxUseDedicatedParameterValues.Location = new System.Drawing.Point(183, 3);
            this.checkBoxUseDedicatedParameterValues.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.checkBoxUseDedicatedParameterValues.Name = "checkBoxUseDedicatedParameterValues";
            this.checkBoxUseDedicatedParameterValues.Size = new System.Drawing.Size(165, 20);
            this.checkBoxUseDedicatedParameterValues.TabIndex = 27;
            this.checkBoxUseDedicatedParameterValues.Text = "Use dedicated values for a parameter";
            this.checkBoxUseDedicatedParameterValues.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.checkBoxUseDedicatedParameterValues.UseVisualStyleBackColor = true;
            this.checkBoxUseDedicatedParameterValues.CheckedChanged += new System.EventHandler(this.checkBoxUseDedicatedParameterValues_CheckedChanged);
            this.checkBoxUseDedicatedParameterValues.Click += new System.EventHandler(this.checkBoxUseDedicatedParameterValues_Click);
            // 
            // groupBoxParameterValues
            // 
            this.tableLayoutPanelParameter.SetColumnSpan(this.groupBoxParameterValues, 3);
            this.groupBoxParameterValues.Controls.Add(this.tableLayoutPanelParameterValues);
            this.groupBoxParameterValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxParameterValues.Location = new System.Drawing.Point(180, 23);
            this.groupBoxParameterValues.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxParameterValues.Name = "groupBoxParameterValues";
            this.tableLayoutPanelParameter.SetRowSpan(this.groupBoxParameterValues, 4);
            this.groupBoxParameterValues.Size = new System.Drawing.Size(238, 106);
            this.groupBoxParameterValues.TabIndex = 38;
            this.groupBoxParameterValues.TabStop = false;
            this.groupBoxParameterValues.Text = "Dedicated parameter values";
            // 
            // tableLayoutPanelParameterValues
            // 
            this.tableLayoutPanelParameterValues.ColumnCount = 4;
            this.tableLayoutPanelParameterValues.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelParameterValues.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelParameterValues.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelParameterValues.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelParameterValues.Controls.Add(this.listBoxParameterValue, 0, 0);
            this.tableLayoutPanelParameterValues.Controls.Add(this.toolStripParameterValues, 0, 3);
            this.tableLayoutPanelParameterValues.Controls.Add(this.labelParameterValueDisplayText, 3, 0);
            this.tableLayoutPanelParameterValues.Controls.Add(this.textBoxParameterValueDisplayText, 3, 1);
            this.tableLayoutPanelParameterValues.Controls.Add(this.labelParameterValue, 1, 0);
            this.tableLayoutPanelParameterValues.Controls.Add(this.textBoxParameterValue, 1, 1);
            this.tableLayoutPanelParameterValues.Controls.Add(this.labelParameterValueUri, 1, 3);
            this.tableLayoutPanelParameterValues.Controls.Add(this.textBoxParameterValueUri, 3, 3);
            this.tableLayoutPanelParameterValues.Controls.Add(this.buttonParameterValueUri, 2, 3);
            this.tableLayoutPanelParameterValues.Controls.Add(this.labelParameterValueDescription, 1, 2);
            this.tableLayoutPanelParameterValues.Controls.Add(this.textBoxParameterValueDescription, 3, 2);
            this.tableLayoutPanelParameterValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelParameterValues.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelParameterValues.Name = "tableLayoutPanelParameterValues";
            this.tableLayoutPanelParameterValues.RowCount = 4;
            this.tableLayoutPanelParameterValues.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelParameterValues.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelParameterValues.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelParameterValues.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelParameterValues.Size = new System.Drawing.Size(232, 87);
            this.tableLayoutPanelParameterValues.TabIndex = 32;
            // 
            // listBoxParameterValue
            // 
            this.listBoxParameterValue.DataSource = this.parameterValueEnumBindingSource;
            this.listBoxParameterValue.DisplayMember = "DisplayText";
            this.listBoxParameterValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxParameterValue.FormattingEnabled = true;
            this.listBoxParameterValue.IntegralHeight = false;
            this.listBoxParameterValue.Location = new System.Drawing.Point(0, 0);
            this.listBoxParameterValue.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxParameterValue.Name = "listBoxParameterValue";
            this.tableLayoutPanelParameterValues.SetRowSpan(this.listBoxParameterValue, 3);
            this.listBoxParameterValue.Size = new System.Drawing.Size(83, 62);
            this.listBoxParameterValue.TabIndex = 31;
            this.listBoxParameterValue.ValueMember = "Value";
            this.listBoxParameterValue.SelectedIndexChanged += new System.EventHandler(this.listBoxParameterValue_SelectedIndexChanged);
            // 
            // parameterValueEnumBindingSource
            // 
            this.parameterValueEnumBindingSource.DataMember = "ParameterValue_Enum";
            this.parameterValueEnumBindingSource.DataSource = this.dataSetMethod;
            // 
            // toolStripParameterValues
            // 
            this.toolStripParameterValues.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStripParameterValues.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripParameterValues.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonParameterValueAdd,
            this.toolStripButtonParameterValueRemove});
            this.toolStripParameterValues.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStripParameterValues.Location = new System.Drawing.Point(0, 62);
            this.toolStripParameterValues.Name = "toolStripParameterValues";
            this.toolStripParameterValues.Size = new System.Drawing.Size(49, 25);
            this.toolStripParameterValues.TabIndex = 22;
            this.toolStripParameterValues.Text = "toolStrip1";
            // 
            // toolStripButtonParameterValueAdd
            // 
            this.toolStripButtonParameterValueAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonParameterValueAdd.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonParameterValueAdd.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonParameterValueAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonParameterValueAdd.Name = "toolStripButtonParameterValueAdd";
            this.toolStripButtonParameterValueAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonParameterValueAdd.Text = "Add a dedicated value to the list";
            this.toolStripButtonParameterValueAdd.Click += new System.EventHandler(this.toolStripButtonParameterValueAdd_Click);
            // 
            // toolStripButtonParameterValueRemove
            // 
            this.toolStripButtonParameterValueRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonParameterValueRemove.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonParameterValueRemove.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonParameterValueRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonParameterValueRemove.Name = "toolStripButtonParameterValueRemove";
            this.toolStripButtonParameterValueRemove.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonParameterValueRemove.Text = "Remove the selected value from the list";
            this.toolStripButtonParameterValueRemove.Click += new System.EventHandler(this.toolStripButtonParameterValueRemove_Click);
            // 
            // labelParameterValueDisplayText
            // 
            this.labelParameterValueDisplayText.AutoSize = true;
            this.labelParameterValueDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelParameterValueDisplayText.Location = new System.Drawing.Point(152, 0);
            this.labelParameterValueDisplayText.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelParameterValueDisplayText.Name = "labelParameterValueDisplayText";
            this.labelParameterValueDisplayText.Size = new System.Drawing.Size(80, 16);
            this.labelParameterValueDisplayText.TabIndex = 32;
            this.labelParameterValueDisplayText.Text = "Display text:";
            this.labelParameterValueDisplayText.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxParameterValueDisplayText
            // 
            this.textBoxParameterValueDisplayText.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.parameterValueEnumBindingSource, "DisplayText", true));
            this.textBoxParameterValueDisplayText.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxParameterValueDisplayText.Location = new System.Drawing.Point(152, 16);
            this.textBoxParameterValueDisplayText.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.textBoxParameterValueDisplayText.Name = "textBoxParameterValueDisplayText";
            this.textBoxParameterValueDisplayText.Size = new System.Drawing.Size(77, 20);
            this.textBoxParameterValueDisplayText.TabIndex = 33;
            // 
            // labelParameterValue
            // 
            this.labelParameterValue.AutoSize = true;
            this.tableLayoutPanelParameterValues.SetColumnSpan(this.labelParameterValue, 2);
            this.labelParameterValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelParameterValue.Location = new System.Drawing.Point(86, 0);
            this.labelParameterValue.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelParameterValue.Name = "labelParameterValue";
            this.labelParameterValue.Size = new System.Drawing.Size(63, 16);
            this.labelParameterValue.TabIndex = 34;
            this.labelParameterValue.Text = "Value:";
            this.labelParameterValue.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxParameterValue
            // 
            this.tableLayoutPanelParameterValues.SetColumnSpan(this.textBoxParameterValue, 2);
            this.textBoxParameterValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.parameterValueEnumBindingSource, "Value", true));
            this.textBoxParameterValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxParameterValue.Location = new System.Drawing.Point(86, 16);
            this.textBoxParameterValue.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.textBoxParameterValue.Name = "textBoxParameterValue";
            this.textBoxParameterValue.Size = new System.Drawing.Size(60, 20);
            this.textBoxParameterValue.TabIndex = 35;
            // 
            // labelParameterValueUri
            // 
            this.labelParameterValueUri.AutoSize = true;
            this.labelParameterValueUri.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelParameterValueUri.Location = new System.Drawing.Point(86, 62);
            this.labelParameterValueUri.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelParameterValueUri.Name = "labelParameterValueUri";
            this.labelParameterValueUri.Size = new System.Drawing.Size(39, 25);
            this.labelParameterValueUri.TabIndex = 36;
            this.labelParameterValueUri.Text = "Uri:";
            this.labelParameterValueUri.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxParameterValueUri
            // 
            this.textBoxParameterValueUri.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.parameterValueEnumBindingSource, "URI", true));
            this.textBoxParameterValueUri.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxParameterValueUri.Location = new System.Drawing.Point(152, 62);
            this.textBoxParameterValueUri.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxParameterValueUri.Name = "textBoxParameterValueUri";
            this.textBoxParameterValueUri.Size = new System.Drawing.Size(77, 20);
            this.textBoxParameterValueUri.TabIndex = 37;
            // 
            // buttonParameterValueUri
            // 
            this.buttonParameterValueUri.Image = global::DiversityCollection.Resource.Browse;
            this.buttonParameterValueUri.Location = new System.Drawing.Point(125, 62);
            this.buttonParameterValueUri.Margin = new System.Windows.Forms.Padding(0);
            this.buttonParameterValueUri.Name = "buttonParameterValueUri";
            this.buttonParameterValueUri.Size = new System.Drawing.Size(24, 24);
            this.buttonParameterValueUri.TabIndex = 38;
            this.buttonParameterValueUri.UseVisualStyleBackColor = true;
            this.buttonParameterValueUri.Click += new System.EventHandler(this.buttonParameterValueUri_Click);
            // 
            // labelParameterValueDescription
            // 
            this.labelParameterValueDescription.AutoSize = true;
            this.tableLayoutPanelParameterValues.SetColumnSpan(this.labelParameterValueDescription, 2);
            this.labelParameterValueDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelParameterValueDescription.Location = new System.Drawing.Point(86, 36);
            this.labelParameterValueDescription.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelParameterValueDescription.Name = "labelParameterValueDescription";
            this.labelParameterValueDescription.Size = new System.Drawing.Size(63, 26);
            this.labelParameterValueDescription.TabIndex = 39;
            this.labelParameterValueDescription.Text = "Description:";
            this.labelParameterValueDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxParameterValueDescription
            // 
            this.textBoxParameterValueDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.parameterValueEnumBindingSource, "Description", true));
            this.textBoxParameterValueDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxParameterValueDescription.Location = new System.Drawing.Point(152, 39);
            this.textBoxParameterValueDescription.Name = "textBoxParameterValueDescription";
            this.textBoxParameterValueDescription.Size = new System.Drawing.Size(77, 20);
            this.textBoxParameterValueDescription.TabIndex = 40;
            // 
            // panelDefaultValue
            // 
            this.panelDefaultValue.Controls.Add(this.textBoxParameterDefaultValue);
            this.panelDefaultValue.Controls.Add(this.comboBoxParameterDefaultValue);
            this.panelDefaultValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDefaultValue.Location = new System.Drawing.Point(69, 85);
            this.panelDefaultValue.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.panelDefaultValue.Name = "panelDefaultValue";
            this.panelDefaultValue.Size = new System.Drawing.Size(108, 21);
            this.panelDefaultValue.TabIndex = 39;
            // 
            // textBoxParameterDefaultValue
            // 
            this.textBoxParameterDefaultValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.parameterBindingSource, "DefaultValue", true));
            this.textBoxParameterDefaultValue.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxParameterDefaultValue.Location = new System.Drawing.Point(0, 0);
            this.textBoxParameterDefaultValue.Name = "textBoxParameterDefaultValue";
            this.textBoxParameterDefaultValue.Size = new System.Drawing.Size(36, 20);
            this.textBoxParameterDefaultValue.TabIndex = 36;
            // 
            // comboBoxParameterDefaultValue
            // 
            this.comboBoxParameterDefaultValue.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.parameterBindingSource, "DefaultValue", true));
            this.comboBoxParameterDefaultValue.Dock = System.Windows.Forms.DockStyle.Right;
            this.comboBoxParameterDefaultValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxParameterDefaultValue.FormattingEnabled = true;
            this.comboBoxParameterDefaultValue.Location = new System.Drawing.Point(63, 0);
            this.comboBoxParameterDefaultValue.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.comboBoxParameterDefaultValue.Name = "comboBoxParameterDefaultValue";
            this.comboBoxParameterDefaultValue.Size = new System.Drawing.Size(45, 21);
            this.comboBoxParameterDefaultValue.TabIndex = 35;
            this.comboBoxParameterDefaultValue.KeyUp += new System.Windows.Forms.KeyEventHandler(this.comboBoxParameterDefaultValue_KeyUp);
            // 
            // labelParameterID
            // 
            this.labelParameterID.AutoSize = true;
            this.labelParameterID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelParameterID.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelParameterID.Location = new System.Drawing.Point(354, 0);
            this.labelParameterID.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelParameterID.Name = "labelParameterID";
            this.labelParameterID.Size = new System.Drawing.Size(21, 23);
            this.labelParameterID.TabIndex = 40;
            this.labelParameterID.Text = "ID:";
            this.labelParameterID.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxParameterID
            // 
            this.textBoxParameterID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxParameterID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.parameterBindingSource, "ParameterID", true));
            this.textBoxParameterID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxParameterID.Location = new System.Drawing.Point(375, 0);
            this.textBoxParameterID.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxParameterID.Name = "textBoxParameterID";
            this.textBoxParameterID.ReadOnly = true;
            this.textBoxParameterID.Size = new System.Drawing.Size(40, 13);
            this.textBoxParameterID.TabIndex = 41;
            // 
            // panelURI
            // 
            this.panelURI.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelURI.Controls.Add(this.webBrowserURI);
            this.panelURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelURI.Location = new System.Drawing.Point(0, 0);
            this.panelURI.Name = "panelURI";
            this.panelURI.Size = new System.Drawing.Size(651, 82);
            this.panelURI.TabIndex = 11;
            // 
            // webBrowserURI
            // 
            this.webBrowserURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserURI.Location = new System.Drawing.Point(0, 0);
            this.webBrowserURI.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserURI.Name = "webBrowserURI";
            this.webBrowserURI.Size = new System.Drawing.Size(647, 78);
            this.webBrowserURI.TabIndex = 0;
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem,
            this.toolStripMenuItemHistory,
            this.feedbackToolStripMenuItem,
            this.tableEditorToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(906, 24);
            this.menuStripMain.TabIndex = 10;
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
            // 
            // toolStripMenuItemHistory
            // 
            this.toolStripMenuItemHistory.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripMenuItemHistory.Image = global::DiversityCollection.Resource.History;
            this.toolStripMenuItemHistory.Name = "toolStripMenuItemHistory";
            this.toolStripMenuItemHistory.Size = new System.Drawing.Size(28, 20);
            this.toolStripMenuItemHistory.ToolTipText = "View the history of the current dataset";
            this.toolStripMenuItemHistory.Click += new System.EventHandler(this.toolStripMenuItemHistory_Click);
            // 
            // feedbackToolStripMenuItem
            // 
            this.feedbackToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.feedbackToolStripMenuItem.Image = global::DiversityCollection.Resource.Feedback;
            this.feedbackToolStripMenuItem.Name = "feedbackToolStripMenuItem";
            this.feedbackToolStripMenuItem.Size = new System.Drawing.Size(28, 20);
            this.feedbackToolStripMenuItem.ToolTipText = "Send feedback";
            this.feedbackToolStripMenuItem.Click += new System.EventHandler(this.feedbackToolStripMenuItem_Click);
            // 
            // tableEditorToolStripMenuItem
            // 
            this.tableEditorToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tableEditorToolStripMenuItem.Image = global::DiversityCollection.Resource.EditInSpeadsheet;
            this.tableEditorToolStripMenuItem.Name = "tableEditorToolStripMenuItem";
            this.tableEditorToolStripMenuItem.Size = new System.Drawing.Size(28, 20);
            this.tableEditorToolStripMenuItem.ToolTipText = "Edit methods in table";
            this.tableEditorToolStripMenuItem.Click += new System.EventHandler(this.tableEditorToolStripMenuItem_Click);
            // 
            // methodTableAdapter
            // 
            this.methodTableAdapter.ClearBeforeFill = true;
            // 
            // parameterTableAdapter
            // 
            this.parameterTableAdapter.ClearBeforeFill = true;
            // 
            // parameterValue_EnumTableAdapter
            // 
            this.parameterValue_EnumTableAdapter.ClearBeforeFill = true;
            // 
            // FormMethod
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(906, 678);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.userControlDialogPanel);
            this.Controls.Add(this.labelHeader);
            this.Controls.Add(this.menuStripMain);
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.helpProvider.SetHelpString(this, "Method");
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMethod";
            this.helpProvider.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Administration of methods";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMethod_FormClosing);
            this.Load += new System.EventHandler(this.FormMethod_Load);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerData.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerData)).EndInit();
            this.splitContainerData.ResumeLayout(false);
            this.splitContainerURI.Panel1.ResumeLayout(false);
            this.splitContainerURI.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerURI)).EndInit();
            this.splitContainerURI.ResumeLayout(false);
            this.groupBoxMethod.ResumeLayout(false);
            this.tableLayoutPanelTool.ResumeLayout(false);
            this.tableLayoutPanelTool.PerformLayout();
            this.toolStripTool.ResumeLayout(false);
            this.toolStripTool.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.methodBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetMethod)).EndInit();
            this.groupBoxAnalysis.ResumeLayout(false);
            this.groupBoxAnalysis.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.methodForAnalysisListBindingSource)).EndInit();
            this.toolStripAnalysis.ResumeLayout(false);
            this.toolStripAnalysis.PerformLayout();
            this.groupBoxProcessing.ResumeLayout(false);
            this.groupBoxProcessing.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.methodForProcessingListBindingSource)).EndInit();
            this.toolStripProcessing.ResumeLayout(false);
            this.toolStripProcessing.PerformLayout();
            this.groupBoxParameter.ResumeLayout(false);
            this.splitContainerParameter.Panel1.ResumeLayout(false);
            this.splitContainerParameter.Panel1.PerformLayout();
            this.splitContainerParameter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerParameter)).EndInit();
            this.splitContainerParameter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.parameterBindingSource)).EndInit();
            this.toolStripParameter.ResumeLayout(false);
            this.toolStripParameter.PerformLayout();
            this.tableLayoutPanelParameter.ResumeLayout(false);
            this.tableLayoutPanelParameter.PerformLayout();
            this.groupBoxParameterValues.ResumeLayout(false);
            this.tableLayoutPanelParameterValues.ResumeLayout(false);
            this.tableLayoutPanelParameterValues.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parameterValueEnumBindingSource)).EndInit();
            this.toolStripParameterValues.ResumeLayout(false);
            this.toolStripParameterValues.PerformLayout();
            this.panelDefaultValue.ResumeLayout(false);
            this.panelDefaultValue.PerformLayout();
            this.panelURI.ResumeLayout(false);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelHeader;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private DiversityWorkbench.UserControls.UserControlQueryList userControlQueryList;
        private System.Windows.Forms.SplitContainer splitContainerData;
        private System.Windows.Forms.SplitContainer splitContainerURI;
        private System.Windows.Forms.GroupBox groupBoxMethod;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTool;
        private System.Windows.Forms.TreeView treeViewMethod;
        private System.Windows.Forms.ToolStrip toolStripTool;
        private System.Windows.Forms.ToolStripButton toolStripButtonNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonIncludeID;
        private System.Windows.Forms.ToolStripButton toolStripButtonSetParent;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemoveParent;
        private System.Windows.Forms.Label labelDisplayText;
        private System.Windows.Forms.TextBox textBoxDisplayText;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label labelNotes;
        private System.Windows.Forms.TextBox textBoxNotes;
        private System.Windows.Forms.Label labelURI;
        private System.Windows.Forms.TextBox textBoxURI;
        private System.Windows.Forms.Button buttonUriOpen;
        private System.Windows.Forms.ToolStrip toolStripParameterValues;
        private System.Windows.Forms.ToolStripButton toolStripButtonParameterValueAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonParameterValueRemove;
        private System.Windows.Forms.CheckBox checkBoxOnlyHierarchy;
        private System.Windows.Forms.Panel panelURI;
        private System.Windows.Forms.WebBrowser webBrowserURI;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showURIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemHistory;
        private System.Windows.Forms.ToolStripMenuItem feedbackToolStripMenuItem;
        private Datasets.DataSetMethod dataSetMethod;
        private System.Windows.Forms.BindingSource methodBindingSource;
        private Datasets.DataSetMethodTableAdapters.MethodTableAdapter methodTableAdapter;
        private System.Windows.Forms.ListBox listBoxParameter;
        private System.Windows.Forms.ToolStrip toolStripParameter;
        private System.Windows.Forms.ToolStripButton toolStripButtonParameterAdd;
        private System.Windows.Forms.CheckBox checkBoxUseDedicatedParameterValues;
        private System.Windows.Forms.ToolStripButton toolStripButtonParameterDelete;
        private System.Windows.Forms.CheckBox checkBoxForCollectionEvent;
        private System.Windows.Forms.GroupBox groupBoxAnalysis;
        private System.Windows.Forms.GroupBox groupBoxProcessing;
        private System.Windows.Forms.ListBox listBoxParameterValue;
        private System.Windows.Forms.ToolStrip toolStripAnalysis;
        private System.Windows.Forms.ToolStripButton toolStripButtonAnalysisAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonAnalysisRemove;
        private System.Windows.Forms.ListBox listBoxAnalysis;
        private System.Windows.Forms.ListBox listBoxProcessing;
        private System.Windows.Forms.ToolStrip toolStripProcessing;
        private System.Windows.Forms.ToolStripButton toolStripButtonProcessingAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonProcessingRemove;
        private System.Windows.Forms.GroupBox groupBoxParameter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelParameter;
        private System.Windows.Forms.TextBox textBoxParameterDisplayText;
        private System.Windows.Forms.Label labelParameterDescription;
        private System.Windows.Forms.TextBox textBoxParameterDescription;
        private System.Windows.Forms.Label labelParameterURI;
        private System.Windows.Forms.Button buttonParameterURI;
        private System.Windows.Forms.TextBox textBoxParameterURI;
        private System.Windows.Forms.Label labelParameterDefaultValue;
        private System.Windows.Forms.ComboBox comboBoxParameterDefaultValue;
        private System.Windows.Forms.Label labelParameterNotes;
        private System.Windows.Forms.TextBox textBoxParameterNotes;
        private System.Windows.Forms.GroupBox groupBoxParameterValues;
        private System.Windows.Forms.BindingSource methodForAnalysisListBindingSource;
        private System.Windows.Forms.BindingSource methodForProcessingListBindingSource;
        private System.Windows.Forms.BindingSource parameterBindingSource;
        private Datasets.DataSetMethodTableAdapters.ParameterTableAdapter parameterTableAdapter;
        private System.Windows.Forms.BindingSource parameterValueEnumBindingSource;
        private Datasets.DataSetMethodTableAdapters.ParameterValue_EnumTableAdapter parameterValue_EnumTableAdapter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelParameterValues;
        private System.Windows.Forms.Label labelParameterValueDisplayText;
        private System.Windows.Forms.TextBox textBoxParameterValueDisplayText;
        private System.Windows.Forms.Label labelParameterValue;
        private System.Windows.Forms.TextBox textBoxParameterValue;
        private System.Windows.Forms.Label labelParameterValueUri;
        private System.Windows.Forms.TextBox textBoxParameterValueUri;
        private System.Windows.Forms.Button buttonParameterValueUri;
        private System.Windows.Forms.Label labelParameterValueDescription;
        private System.Windows.Forms.TextBox textBoxParameterValueDescription;
        private System.Windows.Forms.Panel panelDefaultValue;
        private System.Windows.Forms.TextBox textBoxParameterDefaultValue;
        private System.Windows.Forms.ToolStripButton toolStripButtonParameterSave;
        private System.Windows.Forms.Label labelParameterID;
        private System.Windows.Forms.TextBox textBoxParameterID;
        private System.Windows.Forms.SplitContainer splitContainerParameter;
        private System.Windows.Forms.ToolStripMenuItem tableEditorToolStripMenuItem;
    }
}