namespace DiversityCollection.Forms
{
    partial class FormAnalysis
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAnalysis));
            Microsoft.Web.WebView2.WinForms.CoreWebView2CreationProperties coreWebView2CreationProperties1 = new Microsoft.Web.WebView2.WinForms.CoreWebView2CreationProperties();
            splitContainerMain = new System.Windows.Forms.SplitContainer();
            userControlQueryList = new DiversityWorkbench.UserControls.UserControlQueryList();
            splitContainerData = new System.Windows.Forms.SplitContainer();
            splitContainerURI = new System.Windows.Forms.SplitContainer();
            groupBoxAnalysis = new System.Windows.Forms.GroupBox();
            tableLayoutPanelAnalysis = new System.Windows.Forms.TableLayoutPanel();
            labelMeasurementUnit = new System.Windows.Forms.Label();
            treeViewAnalysis = new System.Windows.Forms.TreeView();
            toolStripAnalyis = new System.Windows.Forms.ToolStrip();
            toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
            toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            toolStripButtonSpecimenList = new System.Windows.Forms.ToolStripButton();
            toolStripButtonIncludeID = new System.Windows.Forms.ToolStripButton();
            toolStripButtonSetParent = new System.Windows.Forms.ToolStripButton();
            toolStripButtonRemoveParent = new System.Windows.Forms.ToolStripButton();
            labelDisplayText = new System.Windows.Forms.Label();
            textBoxDisplayText = new System.Windows.Forms.TextBox();
            analysisBindingSource = new System.Windows.Forms.BindingSource(components);
            dataSetAnalysis1 = new DiversityCollection.Datasets.DataSetAnalysis();
            labelDescription = new System.Windows.Forms.Label();
            textBoxDescription = new System.Windows.Forms.TextBox();
            labelNotes = new System.Windows.Forms.Label();
            textBoxNotes = new System.Windows.Forms.TextBox();
            labelURI = new System.Windows.Forms.Label();
            textBoxURI = new System.Windows.Forms.TextBox();
            buttonUriOpen = new System.Windows.Forms.Button();
            labelTaxonomicGroup = new System.Windows.Forms.Label();
            listBoxTaxonomicGroup = new System.Windows.Forms.ListBox();
            analysisTaxonomicGroupBindingSource = new System.Windows.Forms.BindingSource(components);
            toolStripTaxonomicGroup = new System.Windows.Forms.ToolStrip();
            toolStripButtonTaxonomicGroupAddMany = new System.Windows.Forms.ToolStripButton();
            toolStripButtonTaxonomicGroupDelete = new System.Windows.Forms.ToolStripButton();
            toolStripButtonTaxonomicGroupAdd = new System.Windows.Forms.ToolStripButton();
            labelProjects = new System.Windows.Forms.Label();
            listBoxProjects = new System.Windows.Forms.ListBox();
            projectAnalysisListBindingSource = new System.Windows.Forms.BindingSource(components);
            toolStripProjects = new System.Windows.Forms.ToolStrip();
            toolStripButtonProjectAddMany = new System.Windows.Forms.ToolStripButton();
            toolStripButtonProjectsDelete = new System.Windows.Forms.ToolStripButton();
            toolStripButtonProjectsNew = new System.Windows.Forms.ToolStripButton();
            checkBoxOnlyHierarchy = new System.Windows.Forms.CheckBox();
            labelResult = new System.Windows.Forms.Label();
            toolStripResult = new System.Windows.Forms.ToolStrip();
            toolStripButtonResultNew = new System.Windows.Forms.ToolStripButton();
            toolStripButtonResultDelete = new System.Windows.Forms.ToolStripButton();
            toolStripButtonViewResultList = new System.Windows.Forms.ToolStripButton();
            dataGridViewResult = new System.Windows.Forms.DataGridView();
            analysisIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            analysisResultDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            displayTextDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            displayOrderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            notesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            analysisResultBindingSource = new System.Windows.Forms.BindingSource(components);
            labelMethods = new System.Windows.Forms.Label();
            listBoxMethods = new System.Windows.Forms.ListBox();
            methodForAnalysisListBindingSource = new System.Windows.Forms.BindingSource(components);
            toolStripMethods = new System.Windows.Forms.ToolStrip();
            toolStripButtonMethodsAdd = new System.Windows.Forms.ToolStripButton();
            toolStripButtonMethodsDelete = new System.Windows.Forms.ToolStripButton();
            buttonTaxonomicGroupAddExisting = new System.Windows.Forms.Button();
            buttonProjectsAddExisting = new System.Windows.Forms.Button();
            comboBoxMeasurementUnit = new System.Windows.Forms.ComboBox();
            panelURI = new System.Windows.Forms.Panel();
            userControlWebViewURI = new DiversityWorkbench.UserControls.UserControlWebView();
            userControlSpecimenList = new DiversityCollection.UserControls.UserControlSpecimenList();
            panelHeader = new System.Windows.Forms.Panel();
            labelHeader = new System.Windows.Forms.Label();
            toolTip = new System.Windows.Forms.ToolTip(components);
            menuStripMain = new System.Windows.Forms.MenuStrip();
            viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            showURIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItemHistory = new System.Windows.Forms.ToolStripMenuItem();
            feedbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItemTableEditor = new System.Windows.Forms.ToolStripMenuItem();
            userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            helpProvider = new System.Windows.Forms.HelpProvider();
            imageListSpecimenList = new System.Windows.Forms.ImageList(components);
            analysisListBindingSource = new System.Windows.Forms.BindingSource(components);
            projectAnalysisListTableAdapter = new DiversityCollection.DataSetAnalysisTableAdapters.ProjectAnalysisListTableAdapter();
            analysisResultTableAdapter = new DiversityCollection.DataSetAnalysisTableAdapters.AnalysisResultTableAdapter();
            analysisTableAdapter = new DiversityCollection.Datasets.DataSetAnalysisTableAdapters.AnalysisTableAdapter();
            projectAnalysisListTableAdapter1 = new DiversityCollection.Datasets.DataSetAnalysisTableAdapters.ProjectAnalysisListTableAdapter();
            analysisResultTableAdapter1 = new DiversityCollection.Datasets.DataSetAnalysisTableAdapters.AnalysisResultTableAdapter();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerData).BeginInit();
            splitContainerData.Panel1.SuspendLayout();
            splitContainerData.Panel2.SuspendLayout();
            splitContainerData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerURI).BeginInit();
            splitContainerURI.Panel1.SuspendLayout();
            splitContainerURI.Panel2.SuspendLayout();
            splitContainerURI.SuspendLayout();
            groupBoxAnalysis.SuspendLayout();
            tableLayoutPanelAnalysis.SuspendLayout();
            toolStripAnalyis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)analysisBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataSetAnalysis1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)analysisTaxonomicGroupBindingSource).BeginInit();
            toolStripTaxonomicGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)projectAnalysisListBindingSource).BeginInit();
            toolStripProjects.SuspendLayout();
            toolStripResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewResult).BeginInit();
            ((System.ComponentModel.ISupportInitialize)analysisResultBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)methodForAnalysisListBindingSource).BeginInit();
            toolStripMethods.SuspendLayout();
            panelURI.SuspendLayout();
            panelHeader.SuspendLayout();
            menuStripMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)analysisListBindingSource).BeginInit();
            SuspendLayout();
            // 
            // splitContainerMain
            // 
            resources.ApplyResources(splitContainerMain, "splitContainerMain");
            helpProvider.SetHelpKeyword(splitContainerMain, resources.GetString("splitContainerMain.HelpKeyword"));
            helpProvider.SetHelpNavigator(splitContainerMain, (System.Windows.Forms.HelpNavigator)resources.GetObject("splitContainerMain.HelpNavigator"));
            splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(userControlQueryList);
            helpProvider.SetShowHelp(splitContainerMain.Panel1, (bool)resources.GetObject("splitContainerMain.Panel1.ShowHelp"));
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(splitContainerData);
            helpProvider.SetShowHelp(splitContainerMain.Panel2, (bool)resources.GetObject("splitContainerMain.Panel2.ShowHelp"));
            helpProvider.SetShowHelp(splitContainerMain, (bool)resources.GetObject("splitContainerMain.ShowHelp"));
            // 
            // userControlQueryList
            // 
            userControlQueryList.BacklinkUpdateEnabled = false;
            userControlQueryList.Connection = null;
            userControlQueryList.DisplayTextSelectedItem = "";
            resources.ApplyResources(userControlQueryList, "userControlQueryList");
            userControlQueryList.IDisNumeric = true;
            userControlQueryList.ImageList = null;
            userControlQueryList.IsPredefinedQuery = false;
            userControlQueryList.MaximalNumberOfResults = 100;
            userControlQueryList.Name = "userControlQueryList";
            userControlQueryList.Optimizing_UsedForQueryList = false;
            userControlQueryList.ProjectID = -1;
            userControlQueryList.QueryConditionVisiblity = "";
            userControlQueryList.QueryDisplayColumns = null;
            userControlQueryList.QueryMainTableLocal = null;
            userControlQueryList.QueryRestriction = "";
            userControlQueryList.RememberQuerySettingsIdentifier = "QueryList";
            userControlQueryList.SelectedProjectID = null;
            helpProvider.SetShowHelp(userControlQueryList, (bool)resources.GetObject("userControlQueryList.ShowHelp"));
            userControlQueryList.TableColors = null;
            userControlQueryList.TableImageIndex = null;
            userControlQueryList.WhereClause = null;
            // 
            // splitContainerData
            // 
            resources.ApplyResources(splitContainerData, "splitContainerData");
            splitContainerData.Name = "splitContainerData";
            // 
            // splitContainerData.Panel1
            // 
            splitContainerData.Panel1.Controls.Add(splitContainerURI);
            helpProvider.SetShowHelp(splitContainerData.Panel1, (bool)resources.GetObject("splitContainerData.Panel1.ShowHelp"));
            // 
            // splitContainerData.Panel2
            // 
            splitContainerData.Panel2.Controls.Add(userControlSpecimenList);
            helpProvider.SetShowHelp(splitContainerData.Panel2, (bool)resources.GetObject("splitContainerData.Panel2.ShowHelp"));
            helpProvider.SetShowHelp(splitContainerData, (bool)resources.GetObject("splitContainerData.ShowHelp"));
            // 
            // splitContainerURI
            // 
            resources.ApplyResources(splitContainerURI, "splitContainerURI");
            splitContainerURI.Name = "splitContainerURI";
            // 
            // splitContainerURI.Panel1
            // 
            splitContainerURI.Panel1.Controls.Add(groupBoxAnalysis);
            helpProvider.SetShowHelp(splitContainerURI.Panel1, (bool)resources.GetObject("splitContainerURI.Panel1.ShowHelp"));
            // 
            // splitContainerURI.Panel2
            // 
            splitContainerURI.Panel2.Controls.Add(panelURI);
            helpProvider.SetShowHelp(splitContainerURI.Panel2, (bool)resources.GetObject("splitContainerURI.Panel2.ShowHelp"));
            helpProvider.SetShowHelp(splitContainerURI, (bool)resources.GetObject("splitContainerURI.ShowHelp"));
            // 
            // groupBoxAnalysis
            // 
            groupBoxAnalysis.Controls.Add(tableLayoutPanelAnalysis);
            resources.ApplyResources(groupBoxAnalysis, "groupBoxAnalysis");
            groupBoxAnalysis.Name = "groupBoxAnalysis";
            helpProvider.SetShowHelp(groupBoxAnalysis, (bool)resources.GetObject("groupBoxAnalysis.ShowHelp"));
            groupBoxAnalysis.TabStop = false;
            // 
            // tableLayoutPanelAnalysis
            // 
            resources.ApplyResources(tableLayoutPanelAnalysis, "tableLayoutPanelAnalysis");
            tableLayoutPanelAnalysis.Controls.Add(labelMeasurementUnit, 0, 3);
            tableLayoutPanelAnalysis.Controls.Add(treeViewAnalysis, 0, 0);
            tableLayoutPanelAnalysis.Controls.Add(toolStripAnalyis, 3, 0);
            tableLayoutPanelAnalysis.Controls.Add(labelDisplayText, 0, 1);
            tableLayoutPanelAnalysis.Controls.Add(textBoxDisplayText, 1, 1);
            tableLayoutPanelAnalysis.Controls.Add(labelDescription, 0, 2);
            tableLayoutPanelAnalysis.Controls.Add(textBoxDescription, 1, 2);
            tableLayoutPanelAnalysis.Controls.Add(labelNotes, 0, 4);
            tableLayoutPanelAnalysis.Controls.Add(textBoxNotes, 1, 4);
            tableLayoutPanelAnalysis.Controls.Add(labelURI, 0, 11);
            tableLayoutPanelAnalysis.Controls.Add(textBoxURI, 1, 11);
            tableLayoutPanelAnalysis.Controls.Add(buttonUriOpen, 3, 11);
            tableLayoutPanelAnalysis.Controls.Add(labelTaxonomicGroup, 0, 5);
            tableLayoutPanelAnalysis.Controls.Add(listBoxTaxonomicGroup, 1, 5);
            tableLayoutPanelAnalysis.Controls.Add(toolStripTaxonomicGroup, 3, 5);
            tableLayoutPanelAnalysis.Controls.Add(labelProjects, 0, 7);
            tableLayoutPanelAnalysis.Controls.Add(listBoxProjects, 1, 7);
            tableLayoutPanelAnalysis.Controls.Add(toolStripProjects, 3, 7);
            tableLayoutPanelAnalysis.Controls.Add(checkBoxOnlyHierarchy, 2, 3);
            tableLayoutPanelAnalysis.Controls.Add(labelResult, 0, 10);
            tableLayoutPanelAnalysis.Controls.Add(toolStripResult, 3, 10);
            tableLayoutPanelAnalysis.Controls.Add(dataGridViewResult, 1, 10);
            tableLayoutPanelAnalysis.Controls.Add(labelMethods, 0, 9);
            tableLayoutPanelAnalysis.Controls.Add(listBoxMethods, 1, 9);
            tableLayoutPanelAnalysis.Controls.Add(toolStripMethods, 3, 9);
            tableLayoutPanelAnalysis.Controls.Add(buttonTaxonomicGroupAddExisting, 0, 6);
            tableLayoutPanelAnalysis.Controls.Add(buttonProjectsAddExisting, 0, 8);
            tableLayoutPanelAnalysis.Controls.Add(comboBoxMeasurementUnit, 1, 3);
            tableLayoutPanelAnalysis.Name = "tableLayoutPanelAnalysis";
            helpProvider.SetShowHelp(tableLayoutPanelAnalysis, (bool)resources.GetObject("tableLayoutPanelAnalysis.ShowHelp"));
            // 
            // labelMeasurementUnit
            // 
            resources.ApplyResources(labelMeasurementUnit, "labelMeasurementUnit");
            labelMeasurementUnit.Name = "labelMeasurementUnit";
            helpProvider.SetShowHelp(labelMeasurementUnit, (bool)resources.GetObject("labelMeasurementUnit.ShowHelp"));
            // 
            // treeViewAnalysis
            // 
            treeViewAnalysis.AllowDrop = true;
            tableLayoutPanelAnalysis.SetColumnSpan(treeViewAnalysis, 3);
            resources.ApplyResources(treeViewAnalysis, "treeViewAnalysis");
            treeViewAnalysis.Name = "treeViewAnalysis";
            helpProvider.SetShowHelp(treeViewAnalysis, (bool)resources.GetObject("treeViewAnalysis.ShowHelp"));
            // 
            // toolStripAnalyis
            // 
            resources.ApplyResources(toolStripAnalyis, "toolStripAnalyis");
            toolStripAnalyis.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonNew, toolStripButtonDelete, toolStripButtonSpecimenList, toolStripButtonIncludeID, toolStripButtonSetParent, toolStripButtonRemoveParent });
            toolStripAnalyis.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            toolStripAnalyis.Name = "toolStripAnalyis";
            helpProvider.SetShowHelp(toolStripAnalyis, (bool)resources.GetObject("toolStripAnalyis.ShowHelp"));
            // 
            // toolStripButtonNew
            // 
            toolStripButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonNew.Image = Resource.New1;
            resources.ApplyResources(toolStripButtonNew, "toolStripButtonNew");
            toolStripButtonNew.Name = "toolStripButtonNew";
            // 
            // toolStripButtonDelete
            // 
            toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonDelete.Image = Resource.Delete;
            resources.ApplyResources(toolStripButtonDelete, "toolStripButtonDelete");
            toolStripButtonDelete.Name = "toolStripButtonDelete";
            // 
            // toolStripButtonSpecimenList
            // 
            toolStripButtonSpecimenList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSpecimenList.Image = Resource.List;
            resources.ApplyResources(toolStripButtonSpecimenList, "toolStripButtonSpecimenList");
            toolStripButtonSpecimenList.Name = "toolStripButtonSpecimenList";
            // 
            // toolStripButtonIncludeID
            // 
            toolStripButtonIncludeID.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(toolStripButtonIncludeID, "toolStripButtonIncludeID");
            toolStripButtonIncludeID.ForeColor = System.Drawing.Color.DarkGray;
            toolStripButtonIncludeID.Name = "toolStripButtonIncludeID";
            // 
            // toolStripButtonSetParent
            // 
            toolStripButtonSetParent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSetParent.Image = Resource.SetParent;
            resources.ApplyResources(toolStripButtonSetParent, "toolStripButtonSetParent");
            toolStripButtonSetParent.Name = "toolStripButtonSetParent";
            // 
            // toolStripButtonRemoveParent
            // 
            toolStripButtonRemoveParent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonRemoveParent.Image = Resource.RemoveParent;
            resources.ApplyResources(toolStripButtonRemoveParent, "toolStripButtonRemoveParent");
            toolStripButtonRemoveParent.Name = "toolStripButtonRemoveParent";
            // 
            // labelDisplayText
            // 
            resources.ApplyResources(labelDisplayText, "labelDisplayText");
            labelDisplayText.Name = "labelDisplayText";
            helpProvider.SetShowHelp(labelDisplayText, (bool)resources.GetObject("labelDisplayText.ShowHelp"));
            // 
            // textBoxDisplayText
            // 
            tableLayoutPanelAnalysis.SetColumnSpan(textBoxDisplayText, 3);
            textBoxDisplayText.DataBindings.Add(new System.Windows.Forms.Binding("Text", analysisBindingSource, "DisplayText", true));
            resources.ApplyResources(textBoxDisplayText, "textBoxDisplayText");
            textBoxDisplayText.Name = "textBoxDisplayText";
            helpProvider.SetShowHelp(textBoxDisplayText, (bool)resources.GetObject("textBoxDisplayText.ShowHelp"));
            // 
            // analysisBindingSource
            // 
            analysisBindingSource.DataMember = "Analysis";
            analysisBindingSource.DataSource = dataSetAnalysis1;
            // 
            // dataSetAnalysis1
            // 
            dataSetAnalysis1.DataSetName = "DataSetAnalysis";
            dataSetAnalysis1.Namespace = "http://tempuri.org/DataSetAnalysis.xsd";
            dataSetAnalysis1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // labelDescription
            // 
            resources.ApplyResources(labelDescription, "labelDescription");
            labelDescription.Name = "labelDescription";
            helpProvider.SetShowHelp(labelDescription, (bool)resources.GetObject("labelDescription.ShowHelp"));
            // 
            // textBoxDescription
            // 
            tableLayoutPanelAnalysis.SetColumnSpan(textBoxDescription, 3);
            textBoxDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", analysisBindingSource, "Description", true));
            resources.ApplyResources(textBoxDescription, "textBoxDescription");
            textBoxDescription.Name = "textBoxDescription";
            helpProvider.SetShowHelp(textBoxDescription, (bool)resources.GetObject("textBoxDescription.ShowHelp"));
            // 
            // labelNotes
            // 
            resources.ApplyResources(labelNotes, "labelNotes");
            labelNotes.Name = "labelNotes";
            helpProvider.SetShowHelp(labelNotes, (bool)resources.GetObject("labelNotes.ShowHelp"));
            // 
            // textBoxNotes
            // 
            tableLayoutPanelAnalysis.SetColumnSpan(textBoxNotes, 3);
            textBoxNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", analysisBindingSource, "Notes", true));
            resources.ApplyResources(textBoxNotes, "textBoxNotes");
            textBoxNotes.Name = "textBoxNotes";
            helpProvider.SetShowHelp(textBoxNotes, (bool)resources.GetObject("textBoxNotes.ShowHelp"));
            // 
            // labelURI
            // 
            resources.ApplyResources(labelURI, "labelURI");
            labelURI.Name = "labelURI";
            helpProvider.SetShowHelp(labelURI, (bool)resources.GetObject("labelURI.ShowHelp"));
            // 
            // textBoxURI
            // 
            tableLayoutPanelAnalysis.SetColumnSpan(textBoxURI, 2);
            textBoxURI.DataBindings.Add(new System.Windows.Forms.Binding("Text", analysisBindingSource, "AnalysisURI", true));
            resources.ApplyResources(textBoxURI, "textBoxURI");
            textBoxURI.Name = "textBoxURI";
            helpProvider.SetShowHelp(textBoxURI, (bool)resources.GetObject("textBoxURI.ShowHelp"));
            textBoxURI.TextChanged += textBoxURI_TextChanged;
            // 
            // buttonUriOpen
            // 
            resources.ApplyResources(buttonUriOpen, "buttonUriOpen");
            buttonUriOpen.Image = Resource.Browse;
            buttonUriOpen.Name = "buttonUriOpen";
            helpProvider.SetShowHelp(buttonUriOpen, (bool)resources.GetObject("buttonUriOpen.ShowHelp"));
            buttonUriOpen.UseVisualStyleBackColor = true;
            buttonUriOpen.Click += buttonUriOpen_Click;
            // 
            // labelTaxonomicGroup
            // 
            resources.ApplyResources(labelTaxonomicGroup, "labelTaxonomicGroup");
            labelTaxonomicGroup.Name = "labelTaxonomicGroup";
            helpProvider.SetShowHelp(labelTaxonomicGroup, (bool)resources.GetObject("labelTaxonomicGroup.ShowHelp"));
            // 
            // listBoxTaxonomicGroup
            // 
            tableLayoutPanelAnalysis.SetColumnSpan(listBoxTaxonomicGroup, 2);
            listBoxTaxonomicGroup.DataSource = analysisTaxonomicGroupBindingSource;
            listBoxTaxonomicGroup.DisplayMember = "TaxonomicGroup";
            resources.ApplyResources(listBoxTaxonomicGroup, "listBoxTaxonomicGroup");
            listBoxTaxonomicGroup.FormattingEnabled = true;
            listBoxTaxonomicGroup.Name = "listBoxTaxonomicGroup";
            tableLayoutPanelAnalysis.SetRowSpan(listBoxTaxonomicGroup, 2);
            helpProvider.SetShowHelp(listBoxTaxonomicGroup, (bool)resources.GetObject("listBoxTaxonomicGroup.ShowHelp"));
            listBoxTaxonomicGroup.ValueMember = "TaxonomicGroup";
            // 
            // analysisTaxonomicGroupBindingSource
            // 
            analysisTaxonomicGroupBindingSource.DataMember = "AnalysisTaxonomicGroup";
            analysisTaxonomicGroupBindingSource.DataSource = dataSetAnalysis1;
            // 
            // toolStripTaxonomicGroup
            // 
            toolStripTaxonomicGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonTaxonomicGroupAddMany, toolStripButtonTaxonomicGroupDelete, toolStripButtonTaxonomicGroupAdd });
            toolStripTaxonomicGroup.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            resources.ApplyResources(toolStripTaxonomicGroup, "toolStripTaxonomicGroup");
            toolStripTaxonomicGroup.Name = "toolStripTaxonomicGroup";
            tableLayoutPanelAnalysis.SetRowSpan(toolStripTaxonomicGroup, 2);
            helpProvider.SetShowHelp(toolStripTaxonomicGroup, (bool)resources.GetObject("toolStripTaxonomicGroup.ShowHelp"));
            // 
            // toolStripButtonTaxonomicGroupAddMany
            // 
            toolStripButtonTaxonomicGroupAddMany.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonTaxonomicGroupAddMany.Image = Resource.Add1;
            resources.ApplyResources(toolStripButtonTaxonomicGroupAddMany, "toolStripButtonTaxonomicGroupAddMany");
            toolStripButtonTaxonomicGroupAddMany.Name = "toolStripButtonTaxonomicGroupAddMany";
            toolStripButtonTaxonomicGroupAddMany.Click += toolStripButtonTaxonomicGroupAddMany_Click;
            // 
            // toolStripButtonTaxonomicGroupDelete
            // 
            toolStripButtonTaxonomicGroupDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonTaxonomicGroupDelete.Image = Resource.Delete;
            resources.ApplyResources(toolStripButtonTaxonomicGroupDelete, "toolStripButtonTaxonomicGroupDelete");
            toolStripButtonTaxonomicGroupDelete.Name = "toolStripButtonTaxonomicGroupDelete";
            toolStripButtonTaxonomicGroupDelete.Click += toolStripButtonTaxonomicGroupDelete_Click;
            // 
            // toolStripButtonTaxonomicGroupAdd
            // 
            toolStripButtonTaxonomicGroupAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonTaxonomicGroupAdd.Image = Resource.New1;
            resources.ApplyResources(toolStripButtonTaxonomicGroupAdd, "toolStripButtonTaxonomicGroupAdd");
            toolStripButtonTaxonomicGroupAdd.Name = "toolStripButtonTaxonomicGroupAdd";
            toolStripButtonTaxonomicGroupAdd.Click += toolStripButtonTaxonomicGroupAdd_Click;
            // 
            // labelProjects
            // 
            resources.ApplyResources(labelProjects, "labelProjects");
            labelProjects.Name = "labelProjects";
            helpProvider.SetShowHelp(labelProjects, (bool)resources.GetObject("labelProjects.ShowHelp"));
            // 
            // listBoxProjects
            // 
            tableLayoutPanelAnalysis.SetColumnSpan(listBoxProjects, 2);
            listBoxProjects.DataSource = projectAnalysisListBindingSource;
            listBoxProjects.DisplayMember = "Project";
            resources.ApplyResources(listBoxProjects, "listBoxProjects");
            listBoxProjects.FormattingEnabled = true;
            listBoxProjects.Name = "listBoxProjects";
            tableLayoutPanelAnalysis.SetRowSpan(listBoxProjects, 2);
            helpProvider.SetShowHelp(listBoxProjects, (bool)resources.GetObject("listBoxProjects.ShowHelp"));
            listBoxProjects.ValueMember = "ProjectID";
            // 
            // projectAnalysisListBindingSource
            // 
            projectAnalysisListBindingSource.DataMember = "ProjectAnalysisList";
            projectAnalysisListBindingSource.DataSource = dataSetAnalysis1;
            // 
            // toolStripProjects
            // 
            toolStripProjects.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripProjects.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonProjectAddMany, toolStripButtonProjectsDelete, toolStripButtonProjectsNew });
            toolStripProjects.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            resources.ApplyResources(toolStripProjects, "toolStripProjects");
            toolStripProjects.Name = "toolStripProjects";
            tableLayoutPanelAnalysis.SetRowSpan(toolStripProjects, 2);
            helpProvider.SetShowHelp(toolStripProjects, (bool)resources.GetObject("toolStripProjects.ShowHelp"));
            // 
            // toolStripButtonProjectAddMany
            // 
            toolStripButtonProjectAddMany.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonProjectAddMany.Image = Resource.Add1;
            resources.ApplyResources(toolStripButtonProjectAddMany, "toolStripButtonProjectAddMany");
            toolStripButtonProjectAddMany.Name = "toolStripButtonProjectAddMany";
            toolStripButtonProjectAddMany.Click += toolStripButtonProjectAddMany_Click;
            // 
            // toolStripButtonProjectsDelete
            // 
            toolStripButtonProjectsDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonProjectsDelete.Image = Resource.Delete;
            resources.ApplyResources(toolStripButtonProjectsDelete, "toolStripButtonProjectsDelete");
            toolStripButtonProjectsDelete.Name = "toolStripButtonProjectsDelete";
            toolStripButtonProjectsDelete.Click += toolStripButtonProjectsDelete_Click;
            // 
            // toolStripButtonProjectsNew
            // 
            toolStripButtonProjectsNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonProjectsNew.Image = Resource.New1;
            resources.ApplyResources(toolStripButtonProjectsNew, "toolStripButtonProjectsNew");
            toolStripButtonProjectsNew.Name = "toolStripButtonProjectsNew";
            toolStripButtonProjectsNew.Click += toolStripButtonProjectsNew_Click;
            // 
            // checkBoxOnlyHierarchy
            // 
            resources.ApplyResources(checkBoxOnlyHierarchy, "checkBoxOnlyHierarchy");
            tableLayoutPanelAnalysis.SetColumnSpan(checkBoxOnlyHierarchy, 2);
            checkBoxOnlyHierarchy.DataBindings.Add(new System.Windows.Forms.Binding("Checked", analysisBindingSource, "OnlyHierarchy", true));
            checkBoxOnlyHierarchy.Name = "checkBoxOnlyHierarchy";
            helpProvider.SetShowHelp(checkBoxOnlyHierarchy, (bool)resources.GetObject("checkBoxOnlyHierarchy.ShowHelp"));
            checkBoxOnlyHierarchy.ThreeState = true;
            checkBoxOnlyHierarchy.UseVisualStyleBackColor = true;
            // 
            // labelResult
            // 
            resources.ApplyResources(labelResult, "labelResult");
            labelResult.Name = "labelResult";
            helpProvider.SetShowHelp(labelResult, (bool)resources.GetObject("labelResult.ShowHelp"));
            // 
            // toolStripResult
            // 
            resources.ApplyResources(toolStripResult, "toolStripResult");
            toolStripResult.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripResult.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonResultNew, toolStripButtonResultDelete, toolStripButtonViewResultList });
            toolStripResult.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            toolStripResult.Name = "toolStripResult";
            helpProvider.SetShowHelp(toolStripResult, (bool)resources.GetObject("toolStripResult.ShowHelp"));
            // 
            // toolStripButtonResultNew
            // 
            toolStripButtonResultNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonResultNew.Image = Resource.Add1;
            resources.ApplyResources(toolStripButtonResultNew, "toolStripButtonResultNew");
            toolStripButtonResultNew.Name = "toolStripButtonResultNew";
            toolStripButtonResultNew.Click += toolStripButtonResultNew_Click;
            // 
            // toolStripButtonResultDelete
            // 
            toolStripButtonResultDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonResultDelete.Image = Resource.Delete;
            resources.ApplyResources(toolStripButtonResultDelete, "toolStripButtonResultDelete");
            toolStripButtonResultDelete.Name = "toolStripButtonResultDelete";
            toolStripButtonResultDelete.Click += toolStripButtonResultDelete_Click;
            // 
            // toolStripButtonViewResultList
            // 
            toolStripButtonViewResultList.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripButtonViewResultList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonViewResultList.Image = Resource.Lupe;
            resources.ApplyResources(toolStripButtonViewResultList, "toolStripButtonViewResultList");
            toolStripButtonViewResultList.Name = "toolStripButtonViewResultList";
            toolStripButtonViewResultList.Click += toolStripButtonViewResultList_Click;
            // 
            // dataGridViewResult
            // 
            dataGridViewResult.AllowUserToAddRows = false;
            dataGridViewResult.AllowUserToResizeRows = false;
            dataGridViewResult.AutoGenerateColumns = false;
            dataGridViewResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { analysisIDDataGridViewTextBoxColumn, analysisResultDataGridViewTextBoxColumn, descriptionDataGridViewTextBoxColumn, displayTextDataGridViewTextBoxColumn, displayOrderDataGridViewTextBoxColumn, notesDataGridViewTextBoxColumn });
            tableLayoutPanelAnalysis.SetColumnSpan(dataGridViewResult, 2);
            dataGridViewResult.DataSource = analysisResultBindingSource;
            resources.ApplyResources(dataGridViewResult, "dataGridViewResult");
            dataGridViewResult.MultiSelect = false;
            dataGridViewResult.Name = "dataGridViewResult";
            helpProvider.SetShowHelp(dataGridViewResult, (bool)resources.GetObject("dataGridViewResult.ShowHelp"));
            // 
            // analysisIDDataGridViewTextBoxColumn
            // 
            analysisIDDataGridViewTextBoxColumn.DataPropertyName = "AnalysisID";
            resources.ApplyResources(analysisIDDataGridViewTextBoxColumn, "analysisIDDataGridViewTextBoxColumn");
            analysisIDDataGridViewTextBoxColumn.Name = "analysisIDDataGridViewTextBoxColumn";
            // 
            // analysisResultDataGridViewTextBoxColumn
            // 
            analysisResultDataGridViewTextBoxColumn.DataPropertyName = "AnalysisResult";
            resources.ApplyResources(analysisResultDataGridViewTextBoxColumn, "analysisResultDataGridViewTextBoxColumn");
            analysisResultDataGridViewTextBoxColumn.Name = "analysisResultDataGridViewTextBoxColumn";
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            resources.ApplyResources(descriptionDataGridViewTextBoxColumn, "descriptionDataGridViewTextBoxColumn");
            descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // displayTextDataGridViewTextBoxColumn
            // 
            displayTextDataGridViewTextBoxColumn.DataPropertyName = "DisplayText";
            resources.ApplyResources(displayTextDataGridViewTextBoxColumn, "displayTextDataGridViewTextBoxColumn");
            displayTextDataGridViewTextBoxColumn.Name = "displayTextDataGridViewTextBoxColumn";
            // 
            // displayOrderDataGridViewTextBoxColumn
            // 
            displayOrderDataGridViewTextBoxColumn.DataPropertyName = "DisplayOrder";
            resources.ApplyResources(displayOrderDataGridViewTextBoxColumn, "displayOrderDataGridViewTextBoxColumn");
            displayOrderDataGridViewTextBoxColumn.Name = "displayOrderDataGridViewTextBoxColumn";
            // 
            // notesDataGridViewTextBoxColumn
            // 
            notesDataGridViewTextBoxColumn.DataPropertyName = "Notes";
            resources.ApplyResources(notesDataGridViewTextBoxColumn, "notesDataGridViewTextBoxColumn");
            notesDataGridViewTextBoxColumn.Name = "notesDataGridViewTextBoxColumn";
            // 
            // analysisResultBindingSource
            // 
            analysisResultBindingSource.DataMember = "AnalysisResult";
            analysisResultBindingSource.DataSource = dataSetAnalysis1;
            // 
            // labelMethods
            // 
            resources.ApplyResources(labelMethods, "labelMethods");
            labelMethods.Name = "labelMethods";
            // 
            // listBoxMethods
            // 
            tableLayoutPanelAnalysis.SetColumnSpan(listBoxMethods, 2);
            listBoxMethods.DataSource = methodForAnalysisListBindingSource;
            listBoxMethods.DisplayMember = "DisplayText";
            resources.ApplyResources(listBoxMethods, "listBoxMethods");
            listBoxMethods.FormattingEnabled = true;
            listBoxMethods.Name = "listBoxMethods";
            listBoxMethods.ValueMember = "MethodID";
            // 
            // methodForAnalysisListBindingSource
            // 
            methodForAnalysisListBindingSource.DataMember = "MethodForAnalysisList";
            methodForAnalysisListBindingSource.DataSource = dataSetAnalysis1;
            // 
            // toolStripMethods
            // 
            toolStripMethods.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripMethods.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonMethodsAdd, toolStripButtonMethodsDelete });
            toolStripMethods.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            resources.ApplyResources(toolStripMethods, "toolStripMethods");
            toolStripMethods.Name = "toolStripMethods";
            // 
            // toolStripButtonMethodsAdd
            // 
            toolStripButtonMethodsAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonMethodsAdd.Image = Resource.Add1;
            resources.ApplyResources(toolStripButtonMethodsAdd, "toolStripButtonMethodsAdd");
            toolStripButtonMethodsAdd.Name = "toolStripButtonMethodsAdd";
            toolStripButtonMethodsAdd.Click += toolStripButtonMethodsAdd_Click;
            // 
            // toolStripButtonMethodsDelete
            // 
            toolStripButtonMethodsDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonMethodsDelete.Image = Resource.Delete;
            resources.ApplyResources(toolStripButtonMethodsDelete, "toolStripButtonMethodsDelete");
            toolStripButtonMethodsDelete.Name = "toolStripButtonMethodsDelete";
            toolStripButtonMethodsDelete.Click += toolStripButtonMethodsDelete_Click;
            // 
            // buttonTaxonomicGroupAddExisting
            // 
            resources.ApplyResources(buttonTaxonomicGroupAddExisting, "buttonTaxonomicGroupAddExisting");
            buttonTaxonomicGroupAddExisting.FlatAppearance.BorderSize = 0;
            buttonTaxonomicGroupAddExisting.Image = Resource.AddAdd;
            buttonTaxonomicGroupAddExisting.Name = "buttonTaxonomicGroupAddExisting";
            toolTip.SetToolTip(buttonTaxonomicGroupAddExisting, resources.GetString("buttonTaxonomicGroupAddExisting.ToolTip"));
            buttonTaxonomicGroupAddExisting.UseVisualStyleBackColor = true;
            buttonTaxonomicGroupAddExisting.Click += buttonTaxonomicGroupAddExisting_Click;
            // 
            // buttonProjectsAddExisting
            // 
            resources.ApplyResources(buttonProjectsAddExisting, "buttonProjectsAddExisting");
            buttonProjectsAddExisting.FlatAppearance.BorderSize = 0;
            buttonProjectsAddExisting.Image = Resource.AddAdd;
            buttonProjectsAddExisting.Name = "buttonProjectsAddExisting";
            toolTip.SetToolTip(buttonProjectsAddExisting, resources.GetString("buttonProjectsAddExisting.ToolTip"));
            buttonProjectsAddExisting.UseVisualStyleBackColor = true;
            buttonProjectsAddExisting.Click += buttonProjectsAddExisting_Click;
            // 
            // comboBoxMeasurementUnit
            // 
            comboBoxMeasurementUnit.DataBindings.Add(new System.Windows.Forms.Binding("Text", analysisBindingSource, "MeasurementUnit", true));
            resources.ApplyResources(comboBoxMeasurementUnit, "comboBoxMeasurementUnit");
            comboBoxMeasurementUnit.FormattingEnabled = true;
            comboBoxMeasurementUnit.Name = "comboBoxMeasurementUnit";
            comboBoxMeasurementUnit.DropDown += comboBoxMeasurementUnit_DropDown;
            // 
            // panelURI
            // 
            panelURI.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panelURI.Controls.Add(userControlWebViewURI);
            resources.ApplyResources(panelURI, "panelURI");
            panelURI.Name = "panelURI";
            helpProvider.SetShowHelp(panelURI, (bool)resources.GetObject("panelURI.ShowHelp"));
            // 
            // userControlWebViewURI
            // 
            userControlWebViewURI.AllowScripting = false;
            coreWebView2CreationProperties1.AdditionalBrowserArguments = null;
            coreWebView2CreationProperties1.BrowserExecutableFolder = null;
            coreWebView2CreationProperties1.IsInPrivateModeEnabled = null;
            coreWebView2CreationProperties1.Language = null;
            coreWebView2CreationProperties1.ProfileName = null;
            coreWebView2CreationProperties1.UserDataFolder = "C:\\Users\\mweiss\\AppData\\Local\\DiversityWorkbench\\NETwebView";
            userControlWebViewURI.CreationProperties = coreWebView2CreationProperties1;
            resources.ApplyResources(userControlWebViewURI, "userControlWebViewURI");
            userControlWebViewURI.Name = "userControlWebViewURI";
            userControlWebViewURI.ScriptErrorsSuppressed = false;
            userControlWebViewURI.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            // 
            // userControlSpecimenList
            // 
            resources.ApplyResources(userControlSpecimenList, "userControlSpecimenList");
            userControlSpecimenList.Name = "userControlSpecimenList";
            helpProvider.SetShowHelp(userControlSpecimenList, (bool)resources.GetObject("userControlSpecimenList.ShowHelp"));
            // 
            // panelHeader
            // 
            panelHeader.Controls.Add(labelHeader);
            resources.ApplyResources(panelHeader, "panelHeader");
            panelHeader.Name = "panelHeader";
            helpProvider.SetShowHelp(panelHeader, (bool)resources.GetObject("panelHeader.ShowHelp"));
            // 
            // labelHeader
            // 
            resources.ApplyResources(labelHeader, "labelHeader");
            labelHeader.Name = "labelHeader";
            helpProvider.SetShowHelp(labelHeader, (bool)resources.GetObject("labelHeader.ShowHelp"));
            // 
            // menuStripMain
            // 
            menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { viewToolStripMenuItem, toolStripMenuItemHistory, feedbackToolStripMenuItem, toolStripMenuItemTableEditor });
            resources.ApplyResources(menuStripMain, "menuStripMain");
            menuStripMain.Name = "menuStripMain";
            helpProvider.SetShowHelp(menuStripMain, (bool)resources.GetObject("menuStripMain.ShowHelp"));
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { showURIToolStripMenuItem });
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            resources.ApplyResources(viewToolStripMenuItem, "viewToolStripMenuItem");
            // 
            // showURIToolStripMenuItem
            // 
            showURIToolStripMenuItem.Checked = true;
            showURIToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            showURIToolStripMenuItem.Name = "showURIToolStripMenuItem";
            resources.ApplyResources(showURIToolStripMenuItem, "showURIToolStripMenuItem");
            showURIToolStripMenuItem.Click += showURIToolStripMenuItem_Click;
            // 
            // toolStripMenuItemHistory
            // 
            toolStripMenuItemHistory.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripMenuItemHistory.Image = Resource.History;
            toolStripMenuItemHistory.Name = "toolStripMenuItemHistory";
            resources.ApplyResources(toolStripMenuItemHistory, "toolStripMenuItemHistory");
            toolStripMenuItemHistory.Click += toolStripMenuItemHistory_Click;
            // 
            // feedbackToolStripMenuItem
            // 
            feedbackToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            feedbackToolStripMenuItem.Image = Resource.Feedback;
            feedbackToolStripMenuItem.Name = "feedbackToolStripMenuItem";
            resources.ApplyResources(feedbackToolStripMenuItem, "feedbackToolStripMenuItem");
            feedbackToolStripMenuItem.Click += feedbackToolStripMenuItem_Click;
            // 
            // toolStripMenuItemTableEditor
            // 
            toolStripMenuItemTableEditor.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripMenuItemTableEditor.Image = Resource.EditInSpeadsheet;
            toolStripMenuItemTableEditor.Name = "toolStripMenuItemTableEditor";
            resources.ApplyResources(toolStripMenuItemTableEditor, "toolStripMenuItemTableEditor");
            toolStripMenuItemTableEditor.Click += toolStripMenuItemTableEditor_Click;
            // 
            // userControlDialogPanel
            // 
            resources.ApplyResources(userControlDialogPanel, "userControlDialogPanel");
            userControlDialogPanel.Name = "userControlDialogPanel";
            helpProvider.SetShowHelp(userControlDialogPanel, (bool)resources.GetObject("userControlDialogPanel.ShowHelp"));
            // 
            // imageListSpecimenList
            // 
            imageListSpecimenList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListSpecimenList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListSpecimenList.ImageStream");
            imageListSpecimenList.TransparentColor = System.Drawing.Color.Transparent;
            imageListSpecimenList.Images.SetKeyName(0, "List.ico");
            imageListSpecimenList.Images.SetKeyName(1, "ListNot.ico");
            // 
            // projectAnalysisListTableAdapter
            // 
            projectAnalysisListTableAdapter.ClearBeforeFill = true;
            // 
            // analysisResultTableAdapter
            // 
            analysisResultTableAdapter.ClearBeforeFill = true;
            // 
            // analysisTableAdapter
            // 
            analysisTableAdapter.ClearBeforeFill = true;
            // 
            // projectAnalysisListTableAdapter1
            // 
            projectAnalysisListTableAdapter1.ClearBeforeFill = true;
            // 
            // analysisResultTableAdapter1
            // 
            analysisResultTableAdapter1.ClearBeforeFill = true;
            // 
            // FormAnalysis
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(splitContainerMain);
            Controls.Add(panelHeader);
            Controls.Add(userControlDialogPanel);
            Controls.Add(menuStripMain);
            HelpButton = true;
            helpProvider.SetHelpKeyword(this, resources.GetString("$this.HelpKeyword"));
            helpProvider.SetHelpString(this, resources.GetString("$this.HelpString"));
            KeyPreview = true;
            Name = "FormAnalysis";
            helpProvider.SetShowHelp(this, (bool)resources.GetObject("$this.ShowHelp"));
            ShowInTaskbar = false;
            FormClosing += FormAnalysis_FormClosing;
            Load += FormAnalysis_Load;
            KeyDown += Form_KeyDown;
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            splitContainerData.Panel1.ResumeLayout(false);
            splitContainerData.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerData).EndInit();
            splitContainerData.ResumeLayout(false);
            splitContainerURI.Panel1.ResumeLayout(false);
            splitContainerURI.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerURI).EndInit();
            splitContainerURI.ResumeLayout(false);
            groupBoxAnalysis.ResumeLayout(false);
            tableLayoutPanelAnalysis.ResumeLayout(false);
            tableLayoutPanelAnalysis.PerformLayout();
            toolStripAnalyis.ResumeLayout(false);
            toolStripAnalyis.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)analysisBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataSetAnalysis1).EndInit();
            ((System.ComponentModel.ISupportInitialize)analysisTaxonomicGroupBindingSource).EndInit();
            toolStripTaxonomicGroup.ResumeLayout(false);
            toolStripTaxonomicGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)projectAnalysisListBindingSource).EndInit();
            toolStripProjects.ResumeLayout(false);
            toolStripProjects.PerformLayout();
            toolStripResult.ResumeLayout(false);
            toolStripResult.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewResult).EndInit();
            ((System.ComponentModel.ISupportInitialize)analysisResultBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)methodForAnalysisListBindingSource).EndInit();
            toolStripMethods.ResumeLayout(false);
            toolStripMethods.PerformLayout();
            panelURI.ResumeLayout(false);
            panelHeader.ResumeLayout(false);
            menuStripMain.ResumeLayout(false);
            menuStripMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)analysisListBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAnalysis;
        private System.Windows.Forms.TreeView treeViewAnalysis;
        private System.Windows.Forms.ToolStrip toolStripAnalyis;
        private System.Windows.Forms.ToolStripButton toolStripButtonNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.Label labelDisplayText;
        private System.Windows.Forms.TextBox textBoxDisplayText;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label labelNotes;
        private System.Windows.Forms.TextBox textBoxNotes;
        private System.Windows.Forms.Label labelURI;
        private System.Windows.Forms.TextBox textBoxURI;
        private System.Windows.Forms.Button buttonUriOpen;
        private System.Windows.Forms.BindingSource analysisBindingSource;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.BindingSource analysisListBindingSource;
        private System.Windows.Forms.GroupBox groupBoxAnalysis;
        //private DiversityWorkbench.UserControls.UserControlQueryList userControlQueryList;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.SplitContainer splitContainerData;
        private System.Windows.Forms.ToolStripButton toolStripButtonSpecimenList;
        private DiversityCollection.UserControls.UserControlSpecimenList userControlSpecimenList;
        private System.Windows.Forms.ImageList imageListSpecimenList;
        private System.Windows.Forms.Panel panelURI;
        private System.Windows.Forms.Label labelTaxonomicGroup;
        private System.Windows.Forms.ListBox listBoxTaxonomicGroup;
        private System.Windows.Forms.ToolStrip toolStripTaxonomicGroup;
        private System.Windows.Forms.ToolStripButton toolStripButtonTaxonomicGroupAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonTaxonomicGroupDelete;
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.SplitContainer splitContainerURI;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showURIToolStripMenuItem;
        private System.Windows.Forms.BindingSource analysisTaxonomicGroupBindingSource;
        private System.Windows.Forms.Label labelMeasurementUnit;
        private System.Windows.Forms.ToolStripButton toolStripButtonIncludeID;
        private DiversityWorkbench.UserControls.UserControlQueryList userControlQueryList;
        private System.Windows.Forms.Label labelProjects;
        private System.Windows.Forms.ListBox listBoxProjects;
        private System.Windows.Forms.ToolStrip toolStripProjects;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectsNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectsDelete;
        private System.Windows.Forms.BindingSource projectAnalysisListBindingSource;
        private DiversityCollection.DataSetAnalysisTableAdapters.ProjectAnalysisListTableAdapter projectAnalysisListTableAdapter;
        private System.Windows.Forms.CheckBox checkBoxOnlyHierarchy;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.ToolStrip toolStripResult;
        private System.Windows.Forms.ToolStripButton toolStripButtonResultNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonResultDelete;
        private System.Windows.Forms.DataGridView dataGridViewResult;
        private System.Windows.Forms.BindingSource analysisResultBindingSource;
        private DiversityCollection.DataSetAnalysisTableAdapters.AnalysisResultTableAdapter analysisResultTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn analysisIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn analysisResultDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayTextDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayOrderDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn notesDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemHistory;
        private System.Windows.Forms.ToolStripMenuItem feedbackToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonTaxonomicGroupAddMany;
        private System.Windows.Forms.ToolStripButton toolStripButtonProjectAddMany;
        private System.Windows.Forms.Label labelMethods;
        private System.Windows.Forms.ListBox listBoxMethods;
        private System.Windows.Forms.ToolStrip toolStripMethods;
        private System.Windows.Forms.ToolStripButton toolStripButtonMethodsAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonMethodsDelete;
        private System.Windows.Forms.BindingSource methodForAnalysisListBindingSource;
        private Datasets.DataSetAnalysis dataSetAnalysis1;
        private Datasets.DataSetAnalysisTableAdapters.AnalysisTableAdapter analysisTableAdapter;
        private Datasets.DataSetAnalysisTableAdapters.ProjectAnalysisListTableAdapter projectAnalysisListTableAdapter1;
        private Datasets.DataSetAnalysisTableAdapters.AnalysisResultTableAdapter analysisResultTableAdapter1;
        private System.Windows.Forms.Button buttonTaxonomicGroupAddExisting;
        private System.Windows.Forms.Button buttonProjectsAddExisting;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTableEditor;
        private System.Windows.Forms.ToolStripButton toolStripButtonSetParent;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemoveParent;
        private System.Windows.Forms.ToolStripButton toolStripButtonViewResultList;
        private System.Windows.Forms.ComboBox comboBoxMeasurementUnit;
        private DiversityWorkbench.UserControls.UserControlWebView userControlWebViewURI;
    }
}