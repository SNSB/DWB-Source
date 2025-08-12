namespace DiversityWorkbench.Forms
{
    partial class FormFeedback
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFeedback));
            this.splitContainerLogData = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelLogData = new System.Windows.Forms.TableLayoutPanel();
            this.buttonFilterForDatabase = new System.Windows.Forms.Button();
            this.buttonFilterForUser = new System.Windows.Forms.Button();
            this.labelModule = new System.Windows.Forms.Label();
            this.textBoxModule = new System.Windows.Forms.TextBox();
            this.feedbackBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetFeedback = new DiversityWorkbench.Datasets.DataSetFeedback();
            this.labelUser = new System.Windows.Forms.Label();
            this.textBoxUser = new System.Windows.Forms.TextBox();
            this.textBoxDate = new System.Windows.Forms.TextBox();
            this.labelDate = new System.Windows.Forms.Label();
            this.labelDatabase = new System.Windows.Forms.Label();
            this.textBoxDatabase = new System.Windows.Forms.TextBox();
            this.labelID = new System.Windows.Forms.Label();
            this.textBoxID = new System.Windows.Forms.TextBox();
            this.buttonFilterForModule = new System.Windows.Forms.Button();
            this.labelServer = new System.Windows.Forms.Label();
            this.textBoxServer = new System.Windows.Forms.TextBox();
            this.labelVersion = new System.Windows.Forms.Label();
            this.textBoxVersion = new System.Windows.Forms.TextBox();
            this.buttonSearchForModule = new System.Windows.Forms.Button();
            this.splitContainerQuery = new System.Windows.Forms.SplitContainer();
            this.textBoxLogFile = new System.Windows.Forms.TextBox();
            this.textBoxQuery = new System.Windows.Forms.TextBox();
            this.labelQuery = new System.Windows.Forms.Label();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.labelHeader = new System.Windows.Forms.Label();
            this.splitContainerUserData = new System.Windows.Forms.SplitContainer();
            this.splitContainerDescription = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelDescriptionPublic = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelPriority = new System.Windows.Forms.Label();
            this.comboBoxPriority = new System.Windows.Forms.ComboBox();
            this.priorityEnumBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.labelToDoUntil = new System.Windows.Forms.Label();
            this.dateTimePickerToDoUntil = new System.Windows.Forms.DateTimePicker();
            this.textBoxToDoUntil = new System.Windows.Forms.TextBox();
            this.labelTopic = new System.Windows.Forms.Label();
            this.comboBoxTopic = new System.Windows.Forms.ComboBox();
            this.buttonSortByPriority = new System.Windows.Forms.Button();
            this.buttonSortByDate = new System.Windows.Forms.Button();
            this.buttonFilterForTopic = new System.Windows.Forms.Button();
            this.tableLayoutPanelDescription = new System.Windows.Forms.TableLayoutPanel();
            this.labelState = new System.Windows.Forms.Label();
            this.comboBoxState = new System.Windows.Forms.ComboBox();
            this.progressStateEnumBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.labelProgress = new System.Windows.Forms.Label();
            this.textBoxProgress = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelEmailReply = new System.Windows.Forms.TableLayoutPanel();
            this.labelReply = new System.Windows.Forms.Label();
            this.buttonSendReply = new System.Windows.Forms.Button();
            this.labelReplyAddress = new System.Windows.Forms.Label();
            this.textBoxReplyAddress = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelImage = new System.Windows.Forms.TableLayoutPanel();
            this.labelImage = new System.Windows.Forms.Label();
            this.buttonInsertImage = new System.Windows.Forms.Button();
            this.labelImage2 = new System.Windows.Forms.Label();
            this.panelImage = new System.Windows.Forms.Panel();
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.labelDateForHistory = new System.Windows.Forms.Label();
            this.tableLayoutPanelDialog = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSendFeedback = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.bindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonNewEntry = new System.Windows.Forms.Button();
            this.buttonShowAll = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonRequery = new System.Windows.Forms.Button();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.feedbackTableAdapter = new DiversityWorkbench.Datasets.DataSetFeedbackTableAdapters.FeedbackTableAdapter();
            this.progressState_EnumTableAdapter = new DiversityWorkbench.Datasets.DataSetFeedbackTableAdapters.ProgressState_EnumTableAdapter();
            this.imageListFilter = new System.Windows.Forms.ImageList(this.components);
            this.imageListPriority = new System.Windows.Forms.ImageList(this.components);
            this.priority_EnumTableAdapter = new DiversityWorkbench.Datasets.DataSetFeedbackTableAdapters.Priority_EnumTableAdapter();
            this.imageListSortBy = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLogData)).BeginInit();
            this.splitContainerLogData.Panel1.SuspendLayout();
            this.splitContainerLogData.Panel2.SuspendLayout();
            this.splitContainerLogData.SuspendLayout();
            this.tableLayoutPanelLogData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.feedbackBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetFeedback)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerQuery)).BeginInit();
            this.splitContainerQuery.Panel1.SuspendLayout();
            this.splitContainerQuery.Panel2.SuspendLayout();
            this.splitContainerQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerUserData)).BeginInit();
            this.splitContainerUserData.Panel1.SuspendLayout();
            this.splitContainerUserData.Panel2.SuspendLayout();
            this.splitContainerUserData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDescription)).BeginInit();
            this.splitContainerDescription.Panel1.SuspendLayout();
            this.splitContainerDescription.Panel2.SuspendLayout();
            this.splitContainerDescription.SuspendLayout();
            this.tableLayoutPanelDescriptionPublic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.priorityEnumBindingSource)).BeginInit();
            this.tableLayoutPanelDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.progressStateEnumBindingSource)).BeginInit();
            this.tableLayoutPanelEmailReply.SuspendLayout();
            this.tableLayoutPanelImage.SuspendLayout();
            this.panelImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            this.tableLayoutPanelDialog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator)).BeginInit();
            this.bindingNavigator.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerLogData
            // 
            resources.ApplyResources(this.splitContainerLogData, "splitContainerLogData");
            this.splitContainerLogData.Name = "splitContainerLogData";
            // 
            // splitContainerLogData.Panel1
            // 
            this.splitContainerLogData.Panel1.Controls.Add(this.tableLayoutPanelLogData);
            this.helpProvider.SetShowHelp(this.splitContainerLogData.Panel1, ((bool)(resources.GetObject("splitContainerLogData.Panel1.ShowHelp"))));
            // 
            // splitContainerLogData.Panel2
            // 
            this.splitContainerLogData.Panel2.Controls.Add(this.splitContainerQuery);
            resources.ApplyResources(this.splitContainerLogData.Panel2, "splitContainerLogData.Panel2");
            this.helpProvider.SetShowHelp(this.splitContainerLogData.Panel2, ((bool)(resources.GetObject("splitContainerLogData.Panel2.ShowHelp"))));
            this.helpProvider.SetShowHelp(this.splitContainerLogData, ((bool)(resources.GetObject("splitContainerLogData.ShowHelp"))));
            // 
            // tableLayoutPanelLogData
            // 
            resources.ApplyResources(this.tableLayoutPanelLogData, "tableLayoutPanelLogData");
            this.tableLayoutPanelLogData.Controls.Add(this.buttonFilterForDatabase, 3, 4);
            this.tableLayoutPanelLogData.Controls.Add(this.buttonFilterForUser, 3, 2);
            this.tableLayoutPanelLogData.Controls.Add(this.labelModule, 0, 0);
            this.tableLayoutPanelLogData.Controls.Add(this.textBoxModule, 1, 0);
            this.tableLayoutPanelLogData.Controls.Add(this.labelUser, 0, 2);
            this.tableLayoutPanelLogData.Controls.Add(this.textBoxUser, 1, 2);
            this.tableLayoutPanelLogData.Controls.Add(this.textBoxDate, 1, 3);
            this.tableLayoutPanelLogData.Controls.Add(this.labelDate, 0, 3);
            this.tableLayoutPanelLogData.Controls.Add(this.labelDatabase, 0, 4);
            this.tableLayoutPanelLogData.Controls.Add(this.textBoxDatabase, 1, 4);
            this.tableLayoutPanelLogData.Controls.Add(this.labelID, 0, 6);
            this.tableLayoutPanelLogData.Controls.Add(this.textBoxID, 1, 6);
            this.tableLayoutPanelLogData.Controls.Add(this.buttonFilterForModule, 3, 0);
            this.tableLayoutPanelLogData.Controls.Add(this.labelServer, 0, 5);
            this.tableLayoutPanelLogData.Controls.Add(this.textBoxServer, 1, 5);
            this.tableLayoutPanelLogData.Controls.Add(this.labelVersion, 0, 1);
            this.tableLayoutPanelLogData.Controls.Add(this.textBoxVersion, 1, 1);
            this.tableLayoutPanelLogData.Controls.Add(this.buttonSearchForModule, 2, 0);
            this.tableLayoutPanelLogData.Name = "tableLayoutPanelLogData";
            this.helpProvider.SetShowHelp(this.tableLayoutPanelLogData, ((bool)(resources.GetObject("tableLayoutPanelLogData.ShowHelp"))));
            // 
            // buttonFilterForDatabase
            // 
            resources.ApplyResources(this.buttonFilterForDatabase, "buttonFilterForDatabase");
            this.buttonFilterForDatabase.Image = global::DiversityWorkbench.Properties.Resources.Filter;
            this.buttonFilterForDatabase.Name = "buttonFilterForDatabase";
            this.helpProvider.SetShowHelp(this.buttonFilterForDatabase, ((bool)(resources.GetObject("buttonFilterForDatabase.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonFilterForDatabase, resources.GetString("buttonFilterForDatabase.ToolTip"));
            this.buttonFilterForDatabase.UseVisualStyleBackColor = true;
            this.buttonFilterForDatabase.Click += new System.EventHandler(this.buttonFilterForDatabase_Click);
            // 
            // buttonFilterForUser
            // 
            resources.ApplyResources(this.buttonFilterForUser, "buttonFilterForUser");
            this.buttonFilterForUser.Image = global::DiversityWorkbench.Properties.Resources.Filter;
            this.buttonFilterForUser.Name = "buttonFilterForUser";
            this.helpProvider.SetShowHelp(this.buttonFilterForUser, ((bool)(resources.GetObject("buttonFilterForUser.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonFilterForUser, resources.GetString("buttonFilterForUser.ToolTip"));
            this.buttonFilterForUser.UseVisualStyleBackColor = true;
            this.buttonFilterForUser.Click += new System.EventHandler(this.buttonFilterForUser_Click);
            // 
            // labelModule
            // 
            resources.ApplyResources(this.labelModule, "labelModule");
            this.labelModule.Name = "labelModule";
            this.helpProvider.SetShowHelp(this.labelModule, ((bool)(resources.GetObject("labelModule.ShowHelp"))));
            // 
            // textBoxModule
            // 
            this.textBoxModule.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.feedbackBindingSource, "Module", true));
            resources.ApplyResources(this.textBoxModule, "textBoxModule");
            this.textBoxModule.Name = "textBoxModule";
            this.helpProvider.SetShowHelp(this.textBoxModule, ((bool)(resources.GetObject("textBoxModule.ShowHelp"))));
            // 
            // feedbackBindingSource
            // 
            this.feedbackBindingSource.DataMember = "Feedback";
            this.feedbackBindingSource.DataSource = this.dataSetFeedback;
            // 
            // dataSetFeedback
            // 
            this.dataSetFeedback.DataSetName = "DataSetFeedback";
            this.dataSetFeedback.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // labelUser
            // 
            resources.ApplyResources(this.labelUser, "labelUser");
            this.labelUser.Name = "labelUser";
            this.helpProvider.SetShowHelp(this.labelUser, ((bool)(resources.GetObject("labelUser.ShowHelp"))));
            // 
            // textBoxUser
            // 
            this.tableLayoutPanelLogData.SetColumnSpan(this.textBoxUser, 2);
            this.textBoxUser.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.feedbackBindingSource, "ReportedBy", true));
            resources.ApplyResources(this.textBoxUser, "textBoxUser");
            this.textBoxUser.Name = "textBoxUser";
            this.helpProvider.SetShowHelp(this.textBoxUser, ((bool)(resources.GetObject("textBoxUser.ShowHelp"))));
            // 
            // textBoxDate
            // 
            this.tableLayoutPanelLogData.SetColumnSpan(this.textBoxDate, 2);
            this.textBoxDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.feedbackBindingSource, "LogInsertedWhen", true));
            resources.ApplyResources(this.textBoxDate, "textBoxDate");
            this.textBoxDate.Name = "textBoxDate";
            this.helpProvider.SetShowHelp(this.textBoxDate, ((bool)(resources.GetObject("textBoxDate.ShowHelp"))));
            // 
            // labelDate
            // 
            resources.ApplyResources(this.labelDate, "labelDate");
            this.labelDate.Name = "labelDate";
            this.helpProvider.SetShowHelp(this.labelDate, ((bool)(resources.GetObject("labelDate.ShowHelp"))));
            // 
            // labelDatabase
            // 
            resources.ApplyResources(this.labelDatabase, "labelDatabase");
            this.labelDatabase.Name = "labelDatabase";
            this.helpProvider.SetShowHelp(this.labelDatabase, ((bool)(resources.GetObject("labelDatabase.ShowHelp"))));
            // 
            // textBoxDatabase
            // 
            this.tableLayoutPanelLogData.SetColumnSpan(this.textBoxDatabase, 2);
            this.textBoxDatabase.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.feedbackBindingSource, "DatabaseName", true));
            resources.ApplyResources(this.textBoxDatabase, "textBoxDatabase");
            this.textBoxDatabase.Name = "textBoxDatabase";
            this.helpProvider.SetShowHelp(this.textBoxDatabase, ((bool)(resources.GetObject("textBoxDatabase.ShowHelp"))));
            // 
            // labelID
            // 
            resources.ApplyResources(this.labelID, "labelID");
            this.labelID.Name = "labelID";
            this.helpProvider.SetShowHelp(this.labelID, ((bool)(resources.GetObject("labelID.ShowHelp"))));
            // 
            // textBoxID
            // 
            this.textBoxID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.feedbackBindingSource, "CurrentID", true));
            resources.ApplyResources(this.textBoxID, "textBoxID");
            this.textBoxID.Name = "textBoxID";
            this.helpProvider.SetShowHelp(this.textBoxID, ((bool)(resources.GetObject("textBoxID.ShowHelp"))));
            // 
            // buttonFilterForModule
            // 
            resources.ApplyResources(this.buttonFilterForModule, "buttonFilterForModule");
            this.buttonFilterForModule.Image = global::DiversityWorkbench.Properties.Resources.Filter;
            this.buttonFilterForModule.Name = "buttonFilterForModule";
            this.toolTip.SetToolTip(this.buttonFilterForModule, resources.GetString("buttonFilterForModule.ToolTip"));
            this.buttonFilterForModule.UseVisualStyleBackColor = true;
            this.buttonFilterForModule.Click += new System.EventHandler(this.buttonFilterForModule_Click);
            // 
            // labelServer
            // 
            resources.ApplyResources(this.labelServer, "labelServer");
            this.labelServer.Name = "labelServer";
            // 
            // textBoxServer
            // 
            this.tableLayoutPanelLogData.SetColumnSpan(this.textBoxServer, 3);
            this.textBoxServer.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.feedbackBindingSource, "Server", true));
            resources.ApplyResources(this.textBoxServer, "textBoxServer");
            this.textBoxServer.Name = "textBoxServer";
            // 
            // labelVersion
            // 
            resources.ApplyResources(this.labelVersion, "labelVersion");
            this.labelVersion.Name = "labelVersion";
            // 
            // textBoxVersion
            // 
            this.tableLayoutPanelLogData.SetColumnSpan(this.textBoxVersion, 2);
            this.textBoxVersion.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.feedbackBindingSource, "Version", true));
            resources.ApplyResources(this.textBoxVersion, "textBoxVersion");
            this.textBoxVersion.Name = "textBoxVersion";
            // 
            // buttonSearchForModule
            // 
            resources.ApplyResources(this.buttonSearchForModule, "buttonSearchForModule");
            this.buttonSearchForModule.Image = global::DiversityWorkbench.Properties.Resources.Find;
            this.buttonSearchForModule.Name = "buttonSearchForModule";
            this.buttonSearchForModule.UseVisualStyleBackColor = true;
            this.buttonSearchForModule.Click += new System.EventHandler(this.buttonSearchForModule_Click);
            // 
            // splitContainerQuery
            // 
            resources.ApplyResources(this.splitContainerQuery, "splitContainerQuery");
            this.splitContainerQuery.Name = "splitContainerQuery";
            // 
            // splitContainerQuery.Panel1
            // 
            this.splitContainerQuery.Panel1.Controls.Add(this.textBoxLogFile);
            this.helpProvider.SetShowHelp(this.splitContainerQuery.Panel1, ((bool)(resources.GetObject("splitContainerQuery.Panel1.ShowHelp"))));
            // 
            // splitContainerQuery.Panel2
            // 
            this.splitContainerQuery.Panel2.Controls.Add(this.textBoxQuery);
            this.splitContainerQuery.Panel2.Controls.Add(this.labelQuery);
            this.helpProvider.SetShowHelp(this.splitContainerQuery.Panel2, ((bool)(resources.GetObject("splitContainerQuery.Panel2.ShowHelp"))));
            this.helpProvider.SetShowHelp(this.splitContainerQuery, ((bool)(resources.GetObject("splitContainerQuery.ShowHelp"))));
            // 
            // textBoxLogFile
            // 
            this.textBoxLogFile.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.feedbackBindingSource, "LogFile", true));
            resources.ApplyResources(this.textBoxLogFile, "textBoxLogFile");
            this.textBoxLogFile.Name = "textBoxLogFile";
            this.helpProvider.SetShowHelp(this.textBoxLogFile, ((bool)(resources.GetObject("textBoxLogFile.ShowHelp"))));
            // 
            // textBoxQuery
            // 
            this.textBoxQuery.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.feedbackBindingSource, "QueryString", true));
            resources.ApplyResources(this.textBoxQuery, "textBoxQuery");
            this.textBoxQuery.Name = "textBoxQuery";
            this.helpProvider.SetShowHelp(this.textBoxQuery, ((bool)(resources.GetObject("textBoxQuery.ShowHelp"))));
            // 
            // labelQuery
            // 
            resources.ApplyResources(this.labelQuery, "labelQuery");
            this.labelQuery.Name = "labelQuery";
            this.helpProvider.SetShowHelp(this.labelQuery, ((bool)(resources.GetObject("labelQuery.ShowHelp"))));
            // 
            // splitContainerMain
            // 
            resources.ApplyResources(this.splitContainerMain, "splitContainerMain");
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerLogData);
            this.splitContainerMain.Panel1.Controls.Add(this.labelHeader);
            this.helpProvider.SetShowHelp(this.splitContainerMain.Panel1, ((bool)(resources.GetObject("splitContainerMain.Panel1.ShowHelp"))));
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerUserData);
            this.helpProvider.SetShowHelp(this.splitContainerMain.Panel2, ((bool)(resources.GetObject("splitContainerMain.Panel2.ShowHelp"))));
            this.helpProvider.SetShowHelp(this.splitContainerMain, ((bool)(resources.GetObject("splitContainerMain.ShowHelp"))));
            // 
            // labelHeader
            // 
            resources.ApplyResources(this.labelHeader, "labelHeader");
            this.labelHeader.Name = "labelHeader";
            this.helpProvider.SetShowHelp(this.labelHeader, ((bool)(resources.GetObject("labelHeader.ShowHelp"))));
            // 
            // splitContainerUserData
            // 
            resources.ApplyResources(this.splitContainerUserData, "splitContainerUserData");
            this.splitContainerUserData.Name = "splitContainerUserData";
            // 
            // splitContainerUserData.Panel1
            // 
            this.splitContainerUserData.Panel1.Controls.Add(this.splitContainerDescription);
            this.splitContainerUserData.Panel1.Controls.Add(this.tableLayoutPanelEmailReply);
            this.helpProvider.SetShowHelp(this.splitContainerUserData.Panel1, ((bool)(resources.GetObject("splitContainerUserData.Panel1.ShowHelp"))));
            // 
            // splitContainerUserData.Panel2
            // 
            this.splitContainerUserData.Panel2.Controls.Add(this.tableLayoutPanelImage);
            this.helpProvider.SetShowHelp(this.splitContainerUserData.Panel2, ((bool)(resources.GetObject("splitContainerUserData.Panel2.ShowHelp"))));
            this.helpProvider.SetShowHelp(this.splitContainerUserData, ((bool)(resources.GetObject("splitContainerUserData.ShowHelp"))));
            // 
            // splitContainerDescription
            // 
            resources.ApplyResources(this.splitContainerDescription, "splitContainerDescription");
            this.splitContainerDescription.Name = "splitContainerDescription";
            // 
            // splitContainerDescription.Panel1
            // 
            this.splitContainerDescription.Panel1.Controls.Add(this.tableLayoutPanelDescriptionPublic);
            this.helpProvider.SetShowHelp(this.splitContainerDescription.Panel1, ((bool)(resources.GetObject("splitContainerDescription.Panel1.ShowHelp"))));
            // 
            // splitContainerDescription.Panel2
            // 
            this.splitContainerDescription.Panel2.Controls.Add(this.tableLayoutPanelDescription);
            this.helpProvider.SetShowHelp(this.splitContainerDescription.Panel2, ((bool)(resources.GetObject("splitContainerDescription.Panel2.ShowHelp"))));
            this.helpProvider.SetShowHelp(this.splitContainerDescription, ((bool)(resources.GetObject("splitContainerDescription.ShowHelp"))));
            // 
            // tableLayoutPanelDescriptionPublic
            // 
            resources.ApplyResources(this.tableLayoutPanelDescriptionPublic, "tableLayoutPanelDescriptionPublic");
            this.tableLayoutPanelDescriptionPublic.Controls.Add(this.textBoxDescription, 0, 2);
            this.tableLayoutPanelDescriptionPublic.Controls.Add(this.labelDescription, 0, 1);
            this.tableLayoutPanelDescriptionPublic.Controls.Add(this.labelPriority, 0, 3);
            this.tableLayoutPanelDescriptionPublic.Controls.Add(this.comboBoxPriority, 2, 3);
            this.tableLayoutPanelDescriptionPublic.Controls.Add(this.labelToDoUntil, 3, 3);
            this.tableLayoutPanelDescriptionPublic.Controls.Add(this.dateTimePickerToDoUntil, 6, 3);
            this.tableLayoutPanelDescriptionPublic.Controls.Add(this.textBoxToDoUntil, 5, 3);
            this.tableLayoutPanelDescriptionPublic.Controls.Add(this.labelTopic, 0, 0);
            this.tableLayoutPanelDescriptionPublic.Controls.Add(this.comboBoxTopic, 1, 0);
            this.tableLayoutPanelDescriptionPublic.Controls.Add(this.buttonSortByPriority, 1, 3);
            this.tableLayoutPanelDescriptionPublic.Controls.Add(this.buttonSortByDate, 4, 3);
            this.tableLayoutPanelDescriptionPublic.Controls.Add(this.buttonFilterForTopic, 6, 0);
            this.tableLayoutPanelDescriptionPublic.Name = "tableLayoutPanelDescriptionPublic";
            // 
            // textBoxDescription
            // 
            this.tableLayoutPanelDescriptionPublic.SetColumnSpan(this.textBoxDescription, 7);
            this.textBoxDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.feedbackBindingSource, "Description", true));
            resources.ApplyResources(this.textBoxDescription, "textBoxDescription");
            this.textBoxDescription.Name = "textBoxDescription";
            this.helpProvider.SetShowHelp(this.textBoxDescription, ((bool)(resources.GetObject("textBoxDescription.ShowHelp"))));
            this.toolTip.SetToolTip(this.textBoxDescription, resources.GetString("textBoxDescription.ToolTip"));
            // 
            // labelDescription
            // 
            resources.ApplyResources(this.labelDescription, "labelDescription");
            this.tableLayoutPanelDescriptionPublic.SetColumnSpan(this.labelDescription, 7);
            this.labelDescription.Name = "labelDescription";
            this.helpProvider.SetShowHelp(this.labelDescription, ((bool)(resources.GetObject("labelDescription.ShowHelp"))));
            // 
            // labelPriority
            // 
            resources.ApplyResources(this.labelPriority, "labelPriority");
            this.labelPriority.Name = "labelPriority";
            // 
            // comboBoxPriority
            // 
            this.comboBoxPriority.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.feedbackBindingSource, "Priority", true));
            this.comboBoxPriority.DataSource = this.priorityEnumBindingSource;
            this.comboBoxPriority.DisplayMember = "DisplayText";
            resources.ApplyResources(this.comboBoxPriority, "comboBoxPriority");
            this.comboBoxPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPriority.FormattingEnabled = true;
            this.comboBoxPriority.Name = "comboBoxPriority";
            this.comboBoxPriority.ValueMember = "Code";
            this.comboBoxPriority.SelectedIndexChanged += new System.EventHandler(this.comboBoxPriority_SelectedIndexChanged);
            // 
            // priorityEnumBindingSource
            // 
            this.priorityEnumBindingSource.DataMember = "Priority_Enum";
            this.priorityEnumBindingSource.DataSource = this.dataSetFeedback;
            // 
            // labelToDoUntil
            // 
            resources.ApplyResources(this.labelToDoUntil, "labelToDoUntil");
            this.labelToDoUntil.Name = "labelToDoUntil";
            // 
            // dateTimePickerToDoUntil
            // 
            resources.ApplyResources(this.dateTimePickerToDoUntil, "dateTimePickerToDoUntil");
            this.dateTimePickerToDoUntil.Name = "dateTimePickerToDoUntil";
            this.dateTimePickerToDoUntil.CloseUp += new System.EventHandler(this.dateTimePickerToDoUntil_CloseUp);
            // 
            // textBoxToDoUntil
            // 
            this.textBoxToDoUntil.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.feedbackBindingSource, "ToDoUntil", true));
            resources.ApplyResources(this.textBoxToDoUntil, "textBoxToDoUntil");
            this.textBoxToDoUntil.Name = "textBoxToDoUntil";
            this.textBoxToDoUntil.ReadOnly = true;
            // 
            // labelTopic
            // 
            resources.ApplyResources(this.labelTopic, "labelTopic");
            this.labelTopic.Name = "labelTopic";
            // 
            // comboBoxTopic
            // 
            this.tableLayoutPanelDescriptionPublic.SetColumnSpan(this.comboBoxTopic, 5);
            this.comboBoxTopic.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.feedbackBindingSource, "Topic", true));
            resources.ApplyResources(this.comboBoxTopic, "comboBoxTopic");
            this.comboBoxTopic.FormattingEnabled = true;
            this.comboBoxTopic.Name = "comboBoxTopic";
            this.comboBoxTopic.DropDown += new System.EventHandler(this.comboBoxTopic_DropDown);
            // 
            // buttonSortByPriority
            // 
            this.buttonSortByPriority.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonSortByPriority.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.buttonSortByPriority, "buttonSortByPriority");
            this.buttonSortByPriority.Name = "buttonSortByPriority";
            this.toolTip.SetToolTip(this.buttonSortByPriority, resources.GetString("buttonSortByPriority.ToolTip"));
            this.buttonSortByPriority.UseVisualStyleBackColor = false;
            this.buttonSortByPriority.Click += new System.EventHandler(this.buttonSortByPriority_Click);
            // 
            // buttonSortByDate
            // 
            this.buttonSortByDate.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonSortByDate.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.buttonSortByDate, "buttonSortByDate");
            this.buttonSortByDate.Name = "buttonSortByDate";
            this.toolTip.SetToolTip(this.buttonSortByDate, resources.GetString("buttonSortByDate.ToolTip"));
            this.buttonSortByDate.UseVisualStyleBackColor = false;
            this.buttonSortByDate.Click += new System.EventHandler(this.buttonSortByDate_Click);
            // 
            // buttonFilterForTopic
            // 
            this.buttonFilterForTopic.Image = global::DiversityWorkbench.Properties.Resources.Filter;
            resources.ApplyResources(this.buttonFilterForTopic, "buttonFilterForTopic");
            this.buttonFilterForTopic.Name = "buttonFilterForTopic";
            this.buttonFilterForTopic.UseVisualStyleBackColor = true;
            this.buttonFilterForTopic.Click += new System.EventHandler(this.buttonFilterForTopic_Click);
            // 
            // tableLayoutPanelDescription
            // 
            resources.ApplyResources(this.tableLayoutPanelDescription, "tableLayoutPanelDescription");
            this.tableLayoutPanelDescription.Controls.Add(this.labelState, 0, 0);
            this.tableLayoutPanelDescription.Controls.Add(this.comboBoxState, 1, 0);
            this.tableLayoutPanelDescription.Controls.Add(this.labelProgress, 0, 1);
            this.tableLayoutPanelDescription.Controls.Add(this.textBoxProgress, 0, 2);
            this.tableLayoutPanelDescription.Name = "tableLayoutPanelDescription";
            this.helpProvider.SetShowHelp(this.tableLayoutPanelDescription, ((bool)(resources.GetObject("tableLayoutPanelDescription.ShowHelp"))));
            // 
            // labelState
            // 
            resources.ApplyResources(this.labelState, "labelState");
            this.labelState.Name = "labelState";
            this.helpProvider.SetShowHelp(this.labelState, ((bool)(resources.GetObject("labelState.ShowHelp"))));
            // 
            // comboBoxState
            // 
            this.tableLayoutPanelDescription.SetColumnSpan(this.comboBoxState, 2);
            this.comboBoxState.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.feedbackBindingSource, "ProgressState", true));
            this.comboBoxState.DataSource = this.progressStateEnumBindingSource;
            this.comboBoxState.DisplayMember = "DisplayText";
            resources.ApplyResources(this.comboBoxState, "comboBoxState");
            this.comboBoxState.FormattingEnabled = true;
            this.comboBoxState.Name = "comboBoxState";
            this.helpProvider.SetShowHelp(this.comboBoxState, ((bool)(resources.GetObject("comboBoxState.ShowHelp"))));
            this.comboBoxState.ValueMember = "Code";
            this.comboBoxState.SelectedIndexChanged += new System.EventHandler(this.comboBoxState_SelectedIndexChanged);
            // 
            // progressStateEnumBindingSource
            // 
            this.progressStateEnumBindingSource.DataMember = "ProgressState_Enum";
            this.progressStateEnumBindingSource.DataSource = this.dataSetFeedback;
            // 
            // labelProgress
            // 
            resources.ApplyResources(this.labelProgress, "labelProgress");
            this.labelProgress.Name = "labelProgress";
            this.helpProvider.SetShowHelp(this.labelProgress, ((bool)(resources.GetObject("labelProgress.ShowHelp"))));
            // 
            // textBoxProgress
            // 
            this.tableLayoutPanelDescription.SetColumnSpan(this.textBoxProgress, 3);
            this.textBoxProgress.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.feedbackBindingSource, "Progress", true));
            resources.ApplyResources(this.textBoxProgress, "textBoxProgress");
            this.textBoxProgress.Name = "textBoxProgress";
            this.helpProvider.SetShowHelp(this.textBoxProgress, ((bool)(resources.GetObject("textBoxProgress.ShowHelp"))));
            // 
            // tableLayoutPanelEmailReply
            // 
            resources.ApplyResources(this.tableLayoutPanelEmailReply, "tableLayoutPanelEmailReply");
            this.tableLayoutPanelEmailReply.Controls.Add(this.labelReply, 0, 0);
            this.tableLayoutPanelEmailReply.Controls.Add(this.buttonSendReply, 2, 1);
            this.tableLayoutPanelEmailReply.Controls.Add(this.labelReplyAddress, 0, 1);
            this.tableLayoutPanelEmailReply.Controls.Add(this.textBoxReplyAddress, 1, 1);
            this.tableLayoutPanelEmailReply.Name = "tableLayoutPanelEmailReply";
            this.helpProvider.SetShowHelp(this.tableLayoutPanelEmailReply, ((bool)(resources.GetObject("tableLayoutPanelEmailReply.ShowHelp"))));
            // 
            // labelReply
            // 
            this.tableLayoutPanelEmailReply.SetColumnSpan(this.labelReply, 3);
            resources.ApplyResources(this.labelReply, "labelReply");
            this.labelReply.Name = "labelReply";
            this.helpProvider.SetShowHelp(this.labelReply, ((bool)(resources.GetObject("labelReply.ShowHelp"))));
            // 
            // buttonSendReply
            // 
            resources.ApplyResources(this.buttonSendReply, "buttonSendReply");
            this.buttonSendReply.Name = "buttonSendReply";
            this.helpProvider.SetShowHelp(this.buttonSendReply, ((bool)(resources.GetObject("buttonSendReply.ShowHelp"))));
            this.buttonSendReply.UseVisualStyleBackColor = true;
            this.buttonSendReply.Click += new System.EventHandler(this.buttonSendReply_Click);
            // 
            // labelReplyAddress
            // 
            resources.ApplyResources(this.labelReplyAddress, "labelReplyAddress");
            this.labelReplyAddress.Name = "labelReplyAddress";
            this.helpProvider.SetShowHelp(this.labelReplyAddress, ((bool)(resources.GetObject("labelReplyAddress.ShowHelp"))));
            // 
            // textBoxReplyAddress
            // 
            this.textBoxReplyAddress.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.feedbackBindingSource, "ReplyAddress", true));
            resources.ApplyResources(this.textBoxReplyAddress, "textBoxReplyAddress");
            this.textBoxReplyAddress.Name = "textBoxReplyAddress";
            this.helpProvider.SetShowHelp(this.textBoxReplyAddress, ((bool)(resources.GetObject("textBoxReplyAddress.ShowHelp"))));
            this.toolTip.SetToolTip(this.textBoxReplyAddress, resources.GetString("textBoxReplyAddress.ToolTip"));
            this.textBoxReplyAddress.TextChanged += new System.EventHandler(this.textBoxReplyAddress_TextChanged);
            // 
            // tableLayoutPanelImage
            // 
            resources.ApplyResources(this.tableLayoutPanelImage, "tableLayoutPanelImage");
            this.tableLayoutPanelImage.Controls.Add(this.labelImage, 0, 1);
            this.tableLayoutPanelImage.Controls.Add(this.buttonInsertImage, 1, 2);
            this.tableLayoutPanelImage.Controls.Add(this.labelImage2, 0, 3);
            this.tableLayoutPanelImage.Controls.Add(this.panelImage, 0, 4);
            this.tableLayoutPanelImage.Controls.Add(this.labelDateForHistory, 2, 0);
            this.tableLayoutPanelImage.Name = "tableLayoutPanelImage";
            this.helpProvider.SetShowHelp(this.tableLayoutPanelImage, ((bool)(resources.GetObject("tableLayoutPanelImage.ShowHelp"))));
            // 
            // labelImage
            // 
            resources.ApplyResources(this.labelImage, "labelImage");
            this.tableLayoutPanelImage.SetColumnSpan(this.labelImage, 3);
            this.labelImage.Name = "labelImage";
            this.helpProvider.SetShowHelp(this.labelImage, ((bool)(resources.GetObject("labelImage.ShowHelp"))));
            // 
            // buttonInsertImage
            // 
            resources.ApplyResources(this.buttonInsertImage, "buttonInsertImage");
            this.buttonInsertImage.Name = "buttonInsertImage";
            this.helpProvider.SetShowHelp(this.buttonInsertImage, ((bool)(resources.GetObject("buttonInsertImage.ShowHelp"))));
            this.buttonInsertImage.UseVisualStyleBackColor = true;
            this.buttonInsertImage.Click += new System.EventHandler(this.buttonInsertImage_Click);
            // 
            // labelImage2
            // 
            resources.ApplyResources(this.labelImage2, "labelImage2");
            this.tableLayoutPanelImage.SetColumnSpan(this.labelImage2, 3);
            this.labelImage2.Name = "labelImage2";
            this.helpProvider.SetShowHelp(this.labelImage2, ((bool)(resources.GetObject("labelImage2.ShowHelp"))));
            // 
            // panelImage
            // 
            resources.ApplyResources(this.panelImage, "panelImage");
            this.tableLayoutPanelImage.SetColumnSpan(this.panelImage, 3);
            this.panelImage.Controls.Add(this.pictureBoxImage);
            this.panelImage.Name = "panelImage";
            this.helpProvider.SetShowHelp(this.panelImage, ((bool)(resources.GetObject("panelImage.ShowHelp"))));
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.DataBindings.Add(new System.Windows.Forms.Binding("Image", this.feedbackBindingSource, "Image", true));
            resources.ApplyResources(this.pictureBoxImage, "pictureBoxImage");
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.helpProvider.SetShowHelp(this.pictureBoxImage, ((bool)(resources.GetObject("pictureBoxImage.ShowHelp"))));
            this.pictureBoxImage.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxImage, resources.GetString("pictureBoxImage.ToolTip"));
            // 
            // labelDateForHistory
            // 
            resources.ApplyResources(this.labelDateForHistory, "labelDateForHistory");
            this.labelDateForHistory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.feedbackBindingSource, "LogInsertedWhen", true));
            this.labelDateForHistory.Name = "labelDateForHistory";
            // 
            // tableLayoutPanelDialog
            // 
            resources.ApplyResources(this.tableLayoutPanelDialog, "tableLayoutPanelDialog");
            this.tableLayoutPanelDialog.Controls.Add(this.buttonSendFeedback, 8, 0);
            this.tableLayoutPanelDialog.Controls.Add(this.buttonCancel, 7, 0);
            this.tableLayoutPanelDialog.Controls.Add(this.bindingNavigator, 0, 0);
            this.tableLayoutPanelDialog.Controls.Add(this.buttonNewEntry, 1, 0);
            this.tableLayoutPanelDialog.Controls.Add(this.buttonShowAll, 2, 0);
            this.tableLayoutPanelDialog.Controls.Add(this.buttonSave, 4, 0);
            this.tableLayoutPanelDialog.Controls.Add(this.buttonRequery, 5, 0);
            this.tableLayoutPanelDialog.Name = "tableLayoutPanelDialog";
            this.helpProvider.SetShowHelp(this.tableLayoutPanelDialog, ((bool)(resources.GetObject("tableLayoutPanelDialog.ShowHelp"))));
            // 
            // buttonSendFeedback
            // 
            resources.ApplyResources(this.buttonSendFeedback, "buttonSendFeedback");
            this.buttonSendFeedback.Name = "buttonSendFeedback";
            this.helpProvider.SetShowHelp(this.buttonSendFeedback, ((bool)(resources.GetObject("buttonSendFeedback.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonSendFeedback, resources.GetString("buttonSendFeedback.ToolTip"));
            this.buttonSendFeedback.UseVisualStyleBackColor = true;
            this.buttonSendFeedback.Click += new System.EventHandler(this.buttonSendFeedback_Click);
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.Name = "buttonCancel";
            this.helpProvider.SetShowHelp(this.buttonCancel, ((bool)(resources.GetObject("buttonCancel.ShowHelp"))));
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // bindingNavigator
            // 
            this.bindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.bindingNavigator.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            resources.ApplyResources(this.bindingNavigator, "bindingNavigator");
            this.bindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem});
            this.bindingNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator.Name = "bindingNavigator";
            this.bindingNavigator.PositionItem = this.bindingNavigatorPositionItem;
            this.helpProvider.SetShowHelp(this.bindingNavigator, ((bool)(resources.GetObject("bindingNavigator.ShowHelp"))));
            this.bindingNavigator.RefreshItems += new System.EventHandler(this.bindingNavigator_RefreshItems);
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.bindingNavigatorAddNewItem, "bindingNavigatorAddNewItem");
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            resources.ApplyResources(this.bindingNavigatorCountItem, "bindingNavigatorCountItem");
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.bindingNavigatorDeleteItem, "bindingNavigatorDeleteItem");
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.bindingNavigatorMoveFirstItem, "bindingNavigatorMoveFirstItem");
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.bindingNavigatorMovePreviousItem, "bindingNavigatorMovePreviousItem");
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            resources.ApplyResources(this.bindingNavigatorSeparator, "bindingNavigatorSeparator");
            // 
            // bindingNavigatorPositionItem
            // 
            resources.ApplyResources(this.bindingNavigatorPositionItem, "bindingNavigatorPositionItem");
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            resources.ApplyResources(this.bindingNavigatorSeparator1, "bindingNavigatorSeparator1");
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.bindingNavigatorMoveNextItem, "bindingNavigatorMoveNextItem");
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.bindingNavigatorMoveLastItem, "bindingNavigatorMoveLastItem");
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            resources.ApplyResources(this.bindingNavigatorSeparator2, "bindingNavigatorSeparator2");
            // 
            // buttonNewEntry
            // 
            resources.ApplyResources(this.buttonNewEntry, "buttonNewEntry");
            this.buttonNewEntry.Name = "buttonNewEntry";
            this.helpProvider.SetShowHelp(this.buttonNewEntry, ((bool)(resources.GetObject("buttonNewEntry.ShowHelp"))));
            this.buttonNewEntry.UseVisualStyleBackColor = true;
            this.buttonNewEntry.Click += new System.EventHandler(this.buttonNewEntry_Click);
            // 
            // buttonShowAll
            // 
            resources.ApplyResources(this.buttonShowAll, "buttonShowAll");
            this.buttonShowAll.Image = global::DiversityWorkbench.Properties.Resources.FilterClear;
            this.buttonShowAll.Name = "buttonShowAll";
            this.helpProvider.SetShowHelp(this.buttonShowAll, ((bool)(resources.GetObject("buttonShowAll.ShowHelp"))));
            this.buttonShowAll.Tag = "";
            this.toolTip.SetToolTip(this.buttonShowAll, resources.GetString("buttonShowAll.ToolTip"));
            this.buttonShowAll.UseVisualStyleBackColor = true;
            this.buttonShowAll.Click += new System.EventHandler(this.buttonShowAll_Click);
            // 
            // buttonSave
            // 
            resources.ApplyResources(this.buttonSave, "buttonSave");
            this.buttonSave.Image = global::DiversityWorkbench.Properties.Resources.SaveSmall;
            this.buttonSave.Name = "buttonSave";
            this.toolTip.SetToolTip(this.buttonSave, resources.GetString("buttonSave.ToolTip"));
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonRequery
            // 
            resources.ApplyResources(this.buttonRequery, "buttonRequery");
            this.buttonRequery.Image = global::DiversityWorkbench.Properties.Resources.UpdateDatabase;
            this.buttonRequery.Name = "buttonRequery";
            this.toolTip.SetToolTip(this.buttonRequery, resources.GetString("buttonRequery.ToolTip"));
            this.buttonRequery.UseVisualStyleBackColor = true;
            this.buttonRequery.UseWaitCursor = true;
            this.buttonRequery.Click += new System.EventHandler(this.buttonRequery_Click);
            // 
            // feedbackTableAdapter
            // 
            this.feedbackTableAdapter.ClearBeforeFill = true;
            // 
            // progressState_EnumTableAdapter
            // 
            this.progressState_EnumTableAdapter.ClearBeforeFill = true;
            // 
            // imageListFilter
            // 
            this.imageListFilter.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListFilter.ImageStream")));
            this.imageListFilter.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListFilter.Images.SetKeyName(0, "Filter.ico");
            this.imageListFilter.Images.SetKeyName(1, "FilterClear.ico");
            // 
            // imageListPriority
            // 
            this.imageListPriority.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.imageListPriority, "imageListPriority");
            this.imageListPriority.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // priority_EnumTableAdapter
            // 
            this.priority_EnumTableAdapter.ClearBeforeFill = true;
            // 
            // imageListSortBy
            // 
            this.imageListSortBy.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSortBy.ImageStream")));
            this.imageListSortBy.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSortBy.Images.SetKeyName(0, "ArrowDown.ico");
            this.imageListSortBy.Images.SetKeyName(1, "ArrowUp.ico");
            // 
            // FormFeedback
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.tableLayoutPanelDialog);
            this.Name = "FormFeedback";
            this.helpProvider.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormFeedback_FormClosing);
            this.Load += new System.EventHandler(this.FormFeedback_Load);
            this.splitContainerLogData.Panel1.ResumeLayout(false);
            this.splitContainerLogData.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLogData)).EndInit();
            this.splitContainerLogData.ResumeLayout(false);
            this.tableLayoutPanelLogData.ResumeLayout(false);
            this.tableLayoutPanelLogData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.feedbackBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetFeedback)).EndInit();
            this.splitContainerQuery.Panel1.ResumeLayout(false);
            this.splitContainerQuery.Panel1.PerformLayout();
            this.splitContainerQuery.Panel2.ResumeLayout(false);
            this.splitContainerQuery.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerQuery)).EndInit();
            this.splitContainerQuery.ResumeLayout(false);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerUserData.Panel1.ResumeLayout(false);
            this.splitContainerUserData.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerUserData)).EndInit();
            this.splitContainerUserData.ResumeLayout(false);
            this.splitContainerDescription.Panel1.ResumeLayout(false);
            this.splitContainerDescription.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDescription)).EndInit();
            this.splitContainerDescription.ResumeLayout(false);
            this.tableLayoutPanelDescriptionPublic.ResumeLayout(false);
            this.tableLayoutPanelDescriptionPublic.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.priorityEnumBindingSource)).EndInit();
            this.tableLayoutPanelDescription.ResumeLayout(false);
            this.tableLayoutPanelDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.progressStateEnumBindingSource)).EndInit();
            this.tableLayoutPanelEmailReply.ResumeLayout(false);
            this.tableLayoutPanelEmailReply.PerformLayout();
            this.tableLayoutPanelImage.ResumeLayout(false);
            this.tableLayoutPanelImage.PerformLayout();
            this.panelImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            this.tableLayoutPanelDialog.ResumeLayout(false);
            this.tableLayoutPanelDialog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator)).EndInit();
            this.bindingNavigator.ResumeLayout(false);
            this.bindingNavigator.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDialog;
        private System.Windows.Forms.Button buttonSendFeedback;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.BindingNavigator bindingNavigator;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private Datasets.DataSetFeedback dataSetFeedback;
        private System.Windows.Forms.SplitContainer splitContainerLogData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLogData;
        private System.Windows.Forms.Label labelModule;
        private System.Windows.Forms.TextBox textBoxLogFile;
        private System.Windows.Forms.TextBox textBoxModule;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.TextBox textBoxUser;
        private System.Windows.Forms.TextBox textBoxDate;
        private System.Windows.Forms.Label labelDate;
        private System.Windows.Forms.SplitContainer splitContainerUserData;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelImage;
        private System.Windows.Forms.Label labelImage;
        private System.Windows.Forms.Button buttonInsertImage;
        private System.Windows.Forms.Label labelImage2;
        private System.Windows.Forms.PictureBox pictureBoxImage;
        private System.Windows.Forms.BindingSource feedbackBindingSource;
        private DiversityWorkbench.Datasets.DataSetFeedbackTableAdapters.FeedbackTableAdapter feedbackTableAdapter;
        private System.Windows.Forms.Panel panelImage;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Label labelDatabase;
        private System.Windows.Forms.TextBox textBoxDatabase;
        private System.Windows.Forms.Button buttonSendReply;
        private System.Windows.Forms.TextBox textBoxReplyAddress;
        private System.Windows.Forms.Label labelReply;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDescription;
        private System.Windows.Forms.Label labelReplyAddress;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelQuery;
        private System.Windows.Forms.Label labelID;
        private System.Windows.Forms.TextBox textBoxID;
        private System.Windows.Forms.TextBox textBoxQuery;
        private System.Windows.Forms.SplitContainer splitContainerQuery;
        private System.Windows.Forms.Button buttonNewEntry;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.ComboBox comboBoxState;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.TextBox textBoxProgress;
        private System.Windows.Forms.BindingSource progressStateEnumBindingSource;
        private DiversityWorkbench.Datasets.DataSetFeedbackTableAdapters.ProgressState_EnumTableAdapter progressState_EnumTableAdapter;
        private System.Windows.Forms.Button buttonShowAll;
        private System.Windows.Forms.SplitContainer splitContainerDescription;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEmailReply;
        private System.Windows.Forms.Button buttonFilterForDatabase;
        private System.Windows.Forms.Button buttonFilterForUser;
        private System.Windows.Forms.Button buttonFilterForModule;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonRequery;
        private System.Windows.Forms.Label labelServer;
        private System.Windows.Forms.TextBox textBoxServer;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.TextBox textBoxVersion;
        private System.Windows.Forms.ImageList imageListFilter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDescriptionPublic;
        private System.Windows.Forms.Label labelPriority;
        private System.Windows.Forms.ComboBox comboBoxPriority;
        private System.Windows.Forms.Label labelToDoUntil;
        private System.Windows.Forms.DateTimePicker dateTimePickerToDoUntil;
        private System.Windows.Forms.ImageList imageListPriority;
        private System.Windows.Forms.BindingSource priorityEnumBindingSource;
        private Datasets.DataSetFeedbackTableAdapters.Priority_EnumTableAdapter priority_EnumTableAdapter;
        private System.Windows.Forms.TextBox textBoxToDoUntil;
        private System.Windows.Forms.Label labelTopic;
        private System.Windows.Forms.Button buttonSearchForModule;
        private System.Windows.Forms.ComboBox comboBoxTopic;
        private System.Windows.Forms.Button buttonSortByPriority;
        private System.Windows.Forms.Button buttonSortByDate;
        private System.Windows.Forms.ImageList imageListSortBy;
        private System.Windows.Forms.Button buttonFilterForTopic;
        private System.Windows.Forms.Label labelDateForHistory;
    }
}