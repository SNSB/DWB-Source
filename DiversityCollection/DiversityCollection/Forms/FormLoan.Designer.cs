namespace DiversityCollection
{
    partial class FormLoan
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLoan));
            this.panelHeader = new System.Windows.Forms.Panel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.userControlQueryList = new DiversityWorkbench.UserControlQueryList();
            this.splitContainerData = new System.Windows.Forms.SplitContainer();
            this.groupBoxData = new System.Windows.Forms.GroupBox();
            this.splitContainerLoan = new System.Windows.Forms.SplitContainer();
            this.treeViewLoan = new System.Windows.Forms.TreeView();
            this.toolStripLoan = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSpecimenList = new System.Windows.Forms.ToolStripButton();
            this.tabControlLoan = new System.Windows.Forms.TabControl();
            this.tabPageDataEntry = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelCollection = new System.Windows.Forms.TableLayoutPanel();
            this.labelLoanTitle = new System.Windows.Forms.Label();
            this.textBoxLoanTitle = new System.Windows.Forms.TextBox();
            this.loanBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetLoan = new DiversityCollection.Datasets.DataSetLoan();
            this.labelBegin = new System.Windows.Forms.Label();
            this.textBoxLoanBegin = new System.Windows.Forms.TextBox();
            this.labelLoanNumber = new System.Windows.Forms.Label();
            this.textBoxLoanNumber = new System.Windows.Forms.TextBox();
            this.labelLoanPartnerName = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntryLoanPartner = new DiversityWorkbench.UserControlModuleRelatedEntry();
            this.labelLoanEnd = new System.Windows.Forms.Label();
            this.dateTimePickerLoanBegin = new System.Windows.Forms.DateTimePicker();
            this.textBoxLoanEnd = new System.Windows.Forms.TextBox();
            this.dateTimePickerLoanEnd = new System.Windows.Forms.DateTimePicker();
            this.labelLoanComment = new System.Windows.Forms.Label();
            this.textBoxLoanComment = new System.Windows.Forms.TextBox();
            this.labelInternalNotes = new System.Windows.Forms.Label();
            this.textBoxInternalNotes = new System.Windows.Forms.TextBox();
            this.labelResponsible = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntryResponsible = new DiversityWorkbench.UserControlModuleRelatedEntry();
            this.labelInvestigator = new System.Windows.Forms.Label();
            this.textBoxInvestigator = new System.Windows.Forms.TextBox();
            this.comboBoxLoanCommentAdd = new System.Windows.Forms.ComboBox();
            this.buttonLoanCommentInsertSelected = new System.Windows.Forms.Button();
            this.labelLoanPartnerAddress = new System.Windows.Forms.Label();
            this.labelInitialNumberOfSpecimen = new System.Windows.Forms.Label();
            this.textBoxInitialNumberOfSpecimen = new System.Windows.Forms.TextBox();
            this.tabPageSending = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelSending = new System.Windows.Forms.TableLayoutPanel();
            this.panelWebbrowserSending = new System.Windows.Forms.Panel();
            this.webBrowserSending = new System.Windows.Forms.WebBrowser();
            this.labelSendingSchema = new System.Windows.Forms.Label();
            this.textBoxSendingSchema = new System.Windows.Forms.TextBox();
            this.toolStripSending = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonOpenSendingSchemaFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorSending1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSendingPreview = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSendingPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSendingSave = new System.Windows.Forms.ToolStripButton();
            this.buttonSendingScanner = new System.Windows.Forms.Button();
            this.textBoxSendingAccessionNumber = new System.Windows.Forms.TextBox();
            this.tabPageConfirmation = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelReminder = new System.Windows.Forms.TableLayoutPanel();
            this.panelReminderWebbrowser = new System.Windows.Forms.Panel();
            this.webBrowserConfirmation = new System.Windows.Forms.WebBrowser();
            this.labelConfirmationSchema = new System.Windows.Forms.Label();
            this.textBoxConfirmationSchema = new System.Windows.Forms.TextBox();
            this.toolStripConfirmation = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonConfirmationOpenSchema = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorConfirmation = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonConfirmationPreview = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonConfirmationPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonConfirmationSave = new System.Windows.Forms.ToolStripButton();
            this.tabPageReminder = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelAdmonition = new System.Windows.Forms.TableLayoutPanel();
            this.panelAdmonitionWebbrowser = new System.Windows.Forms.Panel();
            this.webBrowserReminder = new System.Windows.Forms.WebBrowser();
            this.labelReminderSchema = new System.Windows.Forms.Label();
            this.textBoxReminderSchema = new System.Windows.Forms.TextBox();
            this.toolStripReminder = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonReminderOpenSchema = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorReminder = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonReminderPreview = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonReminderPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonReminderSave = new System.Windows.Forms.ToolStripButton();
            this.tabPagePartialReturn = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelPartialReturn = new System.Windows.Forms.TableLayoutPanel();
            this.panelPartialReturnWebbrowser = new System.Windows.Forms.Panel();
            this.webBrowserPartialReturn = new System.Windows.Forms.WebBrowser();
            this.labelPartialReturnSchema = new System.Windows.Forms.Label();
            this.textBoxPartialReturnSchema = new System.Windows.Forms.TextBox();
            this.toolStripPartialReturn = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonPartialReturnOpenSchema = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorPartialReturn1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonPartialReturnPreview = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPartialReturnPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPartialReturnSave = new System.Windows.Forms.ToolStripButton();
            this.buttonPartialReturnScanner = new System.Windows.Forms.Button();
            this.textBoxPartialReturnAccessionNumber = new System.Windows.Forms.TextBox();
            this.labelPartialReturnDate = new System.Windows.Forms.Label();
            this.dateTimePickerPartialReturn = new System.Windows.Forms.DateTimePicker();
            this.labelPartialReturnReturnedSpecimen = new System.Windows.Forms.Label();
            this.textBoxPartialReturnReturnedSpecimen = new System.Windows.Forms.TextBox();
            this.listBoxPartialReturnSpecimen = new System.Windows.Forms.ListBox();
            this.comboBoxPartialReturn = new System.Windows.Forms.ComboBox();
            this.tabPageReturn = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelReturn = new System.Windows.Forms.TableLayoutPanel();
            this.panelReturnWebbrowser = new System.Windows.Forms.Panel();
            this.webBrowserReturn = new System.Windows.Forms.WebBrowser();
            this.labelReturnSchema = new System.Windows.Forms.Label();
            this.textBoxReturnSchema = new System.Windows.Forms.TextBox();
            this.toolStripReturn = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonReturnOpenSchema = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorReturn1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonReturnOK = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonReturnPreview = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonReturnPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonReturnSave = new System.Windows.Forms.ToolStripButton();
            this.tabPageHistory = new System.Windows.Forms.TabPage();
            this.splitContainerLoanHistory = new System.Windows.Forms.SplitContainer();
            this.listBoxLoanHistory = new System.Windows.Forms.ListBox();
            this.loanHistoryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStripLoanHistory = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonLoanHistoryNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLoanHistoryDelete = new System.Windows.Forms.ToolStripButton();
            this.splitContainerLoanHistoryData = new System.Windows.Forms.SplitContainer();
            this.splitContainerLoanHistoryDocuments = new System.Windows.Forms.SplitContainer();
            this.panelHistoryWebbrowser = new System.Windows.Forms.Panel();
            this.webBrowserLoanHistory = new System.Windows.Forms.WebBrowser();
            this.panelHistoryImage = new System.Windows.Forms.Panel();
            this.pictureBoxLoanHistory = new System.Windows.Forms.PictureBox();
            this.toolStripHistoryImage = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonHistoryZoomAdapt = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonHistoryZoom100Percent = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorHistory = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonHistoryRemove = new System.Windows.Forms.ToolStripButton();
            this.textBoxLoanHistoryInternalNotes = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelLoanHistoryButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buttonLoanHistoryInsertDocument = new System.Windows.Forms.Button();
            this.imageListTabpages = new System.Windows.Forms.ImageList(this.components);
            this.userControlSpecimenList = new DiversityCollection.UserControlSpecimenList();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.imageListSpecimenList = new System.Windows.Forms.ImageList(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.timerSending = new System.Windows.Forms.Timer(this.components);
            this.timerReturn = new System.Windows.Forms.Timer(this.components);
            this.userControlDialogPanel = new DiversityWorkbench.UserControlDialogPanel();
            this.openFileDialogSendingSchema = new System.Windows.Forms.OpenFileDialog();
            this.loanTableAdapter = new DiversityCollection.Datasets.DataSetLoanTableAdapters.LoanTableAdapter();
            this.loanHistoryTableAdapter = new DiversityCollection.Datasets.DataSetLoanTableAdapters.LoanHistoryTableAdapter();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerData)).BeginInit();
            this.splitContainerData.Panel1.SuspendLayout();
            this.splitContainerData.Panel2.SuspendLayout();
            this.splitContainerData.SuspendLayout();
            this.groupBoxData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLoan)).BeginInit();
            this.splitContainerLoan.Panel1.SuspendLayout();
            this.splitContainerLoan.Panel2.SuspendLayout();
            this.splitContainerLoan.SuspendLayout();
            this.toolStripLoan.SuspendLayout();
            this.tabControlLoan.SuspendLayout();
            this.tabPageDataEntry.SuspendLayout();
            this.tableLayoutPanelCollection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loanBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetLoan)).BeginInit();
            this.tabPageSending.SuspendLayout();
            this.tableLayoutPanelSending.SuspendLayout();
            this.panelWebbrowserSending.SuspendLayout();
            this.toolStripSending.SuspendLayout();
            this.tabPageConfirmation.SuspendLayout();
            this.tableLayoutPanelReminder.SuspendLayout();
            this.panelReminderWebbrowser.SuspendLayout();
            this.toolStripConfirmation.SuspendLayout();
            this.tabPageReminder.SuspendLayout();
            this.tableLayoutPanelAdmonition.SuspendLayout();
            this.panelAdmonitionWebbrowser.SuspendLayout();
            this.toolStripReminder.SuspendLayout();
            this.tabPagePartialReturn.SuspendLayout();
            this.tableLayoutPanelPartialReturn.SuspendLayout();
            this.panelPartialReturnWebbrowser.SuspendLayout();
            this.toolStripPartialReturn.SuspendLayout();
            this.tabPageReturn.SuspendLayout();
            this.tableLayoutPanelReturn.SuspendLayout();
            this.panelReturnWebbrowser.SuspendLayout();
            this.toolStripReturn.SuspendLayout();
            this.tabPageHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLoanHistory)).BeginInit();
            this.splitContainerLoanHistory.Panel1.SuspendLayout();
            this.splitContainerLoanHistory.Panel2.SuspendLayout();
            this.splitContainerLoanHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loanHistoryBindingSource)).BeginInit();
            this.toolStripLoanHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLoanHistoryData)).BeginInit();
            this.splitContainerLoanHistoryData.Panel1.SuspendLayout();
            this.splitContainerLoanHistoryData.Panel2.SuspendLayout();
            this.splitContainerLoanHistoryData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLoanHistoryDocuments)).BeginInit();
            this.splitContainerLoanHistoryDocuments.Panel1.SuspendLayout();
            this.splitContainerLoanHistoryDocuments.Panel2.SuspendLayout();
            this.splitContainerLoanHistoryDocuments.SuspendLayout();
            this.panelHistoryWebbrowser.SuspendLayout();
            this.panelHistoryImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoanHistory)).BeginInit();
            this.toolStripHistoryImage.SuspendLayout();
            this.tableLayoutPanelLoanHistoryButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.labelHeader);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1016, 24);
            this.panelHeader.TabIndex = 9;
            this.panelHeader.Visible = false;
            // 
            // labelHeader
            // 
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(0, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(1016, 24);
            this.labelHeader.TabIndex = 3;
            this.labelHeader.Text = "Select a loan";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerData);
            this.splitContainerMain.Size = new System.Drawing.Size(1016, 666);
            this.splitContainerMain.SplitterDistance = 201;
            this.splitContainerMain.TabIndex = 8;
            // 
            // userControlQueryList
            // 
            this.userControlQueryList.Connection = null;
            this.userControlQueryList.DisplayTextSelectedItem = "";
            this.userControlQueryList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpProvider.SetHelpKeyword(this.userControlQueryList, "Query");
            this.helpProvider.SetHelpNavigator(this.userControlQueryList, System.Windows.Forms.HelpNavigator.KeywordIndex);
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
            this.helpProvider.SetShowHelp(this.userControlQueryList, true);
            this.userControlQueryList.Size = new System.Drawing.Size(201, 666);
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
            this.splitContainerData.Panel1.Controls.Add(this.groupBoxData);
            // 
            // splitContainerData.Panel2
            // 
            this.splitContainerData.Panel2.Controls.Add(this.userControlSpecimenList);
            this.splitContainerData.Size = new System.Drawing.Size(811, 666);
            this.splitContainerData.SplitterDistance = 670;
            this.splitContainerData.TabIndex = 2;
            // 
            // groupBoxData
            // 
            this.groupBoxData.Controls.Add(this.splitContainerLoan);
            this.groupBoxData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxData.Location = new System.Drawing.Point(0, 0);
            this.groupBoxData.Name = "groupBoxData";
            this.groupBoxData.Size = new System.Drawing.Size(670, 666);
            this.groupBoxData.TabIndex = 2;
            this.groupBoxData.TabStop = false;
            this.groupBoxData.Text = "Loan";
            // 
            // splitContainerLoan
            // 
            this.splitContainerLoan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLoan.Location = new System.Drawing.Point(3, 16);
            this.splitContainerLoan.Name = "splitContainerLoan";
            this.splitContainerLoan.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerLoan.Panel1
            // 
            this.splitContainerLoan.Panel1.Controls.Add(this.treeViewLoan);
            this.splitContainerLoan.Panel1.Controls.Add(this.toolStripLoan);
            // 
            // splitContainerLoan.Panel2
            // 
            this.splitContainerLoan.Panel2.Controls.Add(this.tabControlLoan);
            this.splitContainerLoan.Size = new System.Drawing.Size(664, 647);
            this.splitContainerLoan.SplitterDistance = 167;
            this.splitContainerLoan.TabIndex = 1;
            // 
            // treeViewLoan
            // 
            this.treeViewLoan.AllowDrop = true;
            this.treeViewLoan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewLoan.Location = new System.Drawing.Point(0, 0);
            this.treeViewLoan.Name = "treeViewLoan";
            this.treeViewLoan.Size = new System.Drawing.Size(640, 167);
            this.treeViewLoan.TabIndex = 0;
            // 
            // toolStripLoan
            // 
            this.toolStripLoan.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStripLoan.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNew,
            this.toolStripButtonDelete,
            this.toolStripButtonSpecimenList});
            this.toolStripLoan.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.toolStripLoan.Location = new System.Drawing.Point(640, 0);
            this.toolStripLoan.Name = "toolStripLoan";
            this.toolStripLoan.Size = new System.Drawing.Size(24, 167);
            this.toolStripLoan.TabIndex = 1;
            this.toolStripLoan.Text = "toolStrip1";
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
            this.toolStripButtonSpecimenList.Text = "Show specimen list";
            // 
            // tabControlLoan
            // 
            this.tabControlLoan.Controls.Add(this.tabPageDataEntry);
            this.tabControlLoan.Controls.Add(this.tabPageSending);
            this.tabControlLoan.Controls.Add(this.tabPageConfirmation);
            this.tabControlLoan.Controls.Add(this.tabPageReminder);
            this.tabControlLoan.Controls.Add(this.tabPagePartialReturn);
            this.tabControlLoan.Controls.Add(this.tabPageReturn);
            this.tabControlLoan.Controls.Add(this.tabPageHistory);
            this.tabControlLoan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlLoan.ImageList = this.imageListTabpages;
            this.tabControlLoan.Location = new System.Drawing.Point(0, 0);
            this.tabControlLoan.Name = "tabControlLoan";
            this.tabControlLoan.SelectedIndex = 0;
            this.tabControlLoan.Size = new System.Drawing.Size(664, 476);
            this.tabControlLoan.TabIndex = 0;
            // 
            // tabPageDataEntry
            // 
            this.tabPageDataEntry.Controls.Add(this.tableLayoutPanelCollection);
            this.tabPageDataEntry.Location = new System.Drawing.Point(4, 23);
            this.tabPageDataEntry.Name = "tabPageDataEntry";
            this.tabPageDataEntry.Size = new System.Drawing.Size(656, 449);
            this.tabPageDataEntry.TabIndex = 0;
            this.tabPageDataEntry.Text = "Data entry";
            this.tabPageDataEntry.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelCollection
            // 
            this.tableLayoutPanelCollection.ColumnCount = 7;
            this.tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tableLayoutPanelCollection.Controls.Add(this.labelLoanTitle, 0, 0);
            this.tableLayoutPanelCollection.Controls.Add(this.textBoxLoanTitle, 1, 0);
            this.tableLayoutPanelCollection.Controls.Add(this.labelBegin, 0, 3);
            this.tableLayoutPanelCollection.Controls.Add(this.textBoxLoanBegin, 1, 3);
            this.tableLayoutPanelCollection.Controls.Add(this.labelLoanNumber, 0, 4);
            this.tableLayoutPanelCollection.Controls.Add(this.textBoxLoanNumber, 1, 4);
            this.tableLayoutPanelCollection.Controls.Add(this.labelLoanPartnerName, 0, 1);
            this.tableLayoutPanelCollection.Controls.Add(this.userControlModuleRelatedEntryLoanPartner, 1, 1);
            this.tableLayoutPanelCollection.Controls.Add(this.labelLoanEnd, 4, 3);
            this.tableLayoutPanelCollection.Controls.Add(this.dateTimePickerLoanBegin, 3, 3);
            this.tableLayoutPanelCollection.Controls.Add(this.textBoxLoanEnd, 5, 3);
            this.tableLayoutPanelCollection.Controls.Add(this.dateTimePickerLoanEnd, 6, 3);
            this.tableLayoutPanelCollection.Controls.Add(this.labelLoanComment, 0, 6);
            this.tableLayoutPanelCollection.Controls.Add(this.textBoxLoanComment, 1, 6);
            this.tableLayoutPanelCollection.Controls.Add(this.labelInternalNotes, 0, 8);
            this.tableLayoutPanelCollection.Controls.Add(this.textBoxInternalNotes, 1, 8);
            this.tableLayoutPanelCollection.Controls.Add(this.labelResponsible, 0, 9);
            this.tableLayoutPanelCollection.Controls.Add(this.userControlModuleRelatedEntryResponsible, 1, 9);
            this.tableLayoutPanelCollection.Controls.Add(this.labelInvestigator, 0, 5);
            this.tableLayoutPanelCollection.Controls.Add(this.textBoxInvestigator, 1, 5);
            this.tableLayoutPanelCollection.Controls.Add(this.comboBoxLoanCommentAdd, 2, 7);
            this.tableLayoutPanelCollection.Controls.Add(this.buttonLoanCommentInsertSelected, 1, 7);
            this.tableLayoutPanelCollection.Controls.Add(this.labelLoanPartnerAddress, 1, 2);
            this.tableLayoutPanelCollection.Controls.Add(this.labelInitialNumberOfSpecimen, 3, 4);
            this.tableLayoutPanelCollection.Controls.Add(this.textBoxInitialNumberOfSpecimen, 5, 4);
            this.tableLayoutPanelCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelCollection.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelCollection.Name = "tableLayoutPanelCollection";
            this.tableLayoutPanelCollection.RowCount = 10;
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollection.Size = new System.Drawing.Size(656, 449);
            this.tableLayoutPanelCollection.TabIndex = 0;
            // 
            // labelLoanTitle
            // 
            this.labelLoanTitle.AutoSize = true;
            this.labelLoanTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLoanTitle.Location = new System.Drawing.Point(3, 6);
            this.labelLoanTitle.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelLoanTitle.Name = "labelLoanTitle";
            this.labelLoanTitle.Size = new System.Drawing.Size(55, 83);
            this.labelLoanTitle.TabIndex = 2;
            this.labelLoanTitle.Text = "Title:";
            this.labelLoanTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxLoanTitle
            // 
            this.tableLayoutPanelCollection.SetColumnSpan(this.textBoxLoanTitle, 6);
            this.textBoxLoanTitle.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.loanBindingSource, "LoanTitle", true));
            this.textBoxLoanTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLoanTitle.Location = new System.Drawing.Point(64, 3);
            this.textBoxLoanTitle.Multiline = true;
            this.textBoxLoanTitle.Name = "textBoxLoanTitle";
            this.textBoxLoanTitle.Size = new System.Drawing.Size(589, 83);
            this.textBoxLoanTitle.TabIndex = 3;
            // 
            // loanBindingSource
            // 
            this.loanBindingSource.DataMember = "Loan";
            this.loanBindingSource.DataSource = this.dataSetLoan;
            // 
            // dataSetLoan
            // 
            this.dataSetLoan.DataSetName = "DataSetLoan";
            this.dataSetLoan.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // labelBegin
            // 
            this.labelBegin.AutoSize = true;
            this.labelBegin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelBegin.Location = new System.Drawing.Point(3, 129);
            this.labelBegin.Name = "labelBegin";
            this.labelBegin.Size = new System.Drawing.Size(55, 26);
            this.labelBegin.TabIndex = 4;
            this.labelBegin.Text = "Begin:";
            this.labelBegin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxLoanBegin
            // 
            this.tableLayoutPanelCollection.SetColumnSpan(this.textBoxLoanBegin, 2);
            this.textBoxLoanBegin.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.loanBindingSource, "LoanBegin", true));
            this.textBoxLoanBegin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLoanBegin.Location = new System.Drawing.Point(64, 132);
            this.textBoxLoanBegin.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.textBoxLoanBegin.Name = "textBoxLoanBegin";
            this.textBoxLoanBegin.Size = new System.Drawing.Size(273, 20);
            this.textBoxLoanBegin.TabIndex = 5;
            // 
            // labelLoanNumber
            // 
            this.labelLoanNumber.AutoSize = true;
            this.labelLoanNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLoanNumber.Location = new System.Drawing.Point(3, 155);
            this.labelLoanNumber.Name = "labelLoanNumber";
            this.labelLoanNumber.Size = new System.Drawing.Size(55, 26);
            this.labelLoanNumber.TabIndex = 6;
            this.labelLoanNumber.Text = "Loan Nr.:";
            this.labelLoanNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxLoanNumber
            // 
            this.tableLayoutPanelCollection.SetColumnSpan(this.textBoxLoanNumber, 2);
            this.textBoxLoanNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.loanBindingSource, "LoanNumber", true));
            this.textBoxLoanNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLoanNumber.Location = new System.Drawing.Point(64, 158);
            this.textBoxLoanNumber.Name = "textBoxLoanNumber";
            this.textBoxLoanNumber.Size = new System.Drawing.Size(270, 20);
            this.textBoxLoanNumber.TabIndex = 7;
            // 
            // labelLoanPartnerName
            // 
            this.labelLoanPartnerName.AutoSize = true;
            this.labelLoanPartnerName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLoanPartnerName.Location = new System.Drawing.Point(3, 89);
            this.labelLoanPartnerName.Name = "labelLoanPartnerName";
            this.labelLoanPartnerName.Size = new System.Drawing.Size(55, 27);
            this.labelLoanPartnerName.TabIndex = 11;
            this.labelLoanPartnerName.Text = "Contact:";
            this.labelLoanPartnerName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlModuleRelatedEntryLoanPartner
            // 
            this.userControlModuleRelatedEntryLoanPartner.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelCollection.SetColumnSpan(this.userControlModuleRelatedEntryLoanPartner, 6);
            this.userControlModuleRelatedEntryLoanPartner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryLoanPartner.Domain = "";
            this.helpProvider.SetHelpKeyword(this.userControlModuleRelatedEntryLoanPartner, "Module related entry");
            this.helpProvider.SetHelpNavigator(this.userControlModuleRelatedEntryLoanPartner, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.userControlModuleRelatedEntryLoanPartner.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryLoanPartner.Location = new System.Drawing.Point(64, 92);
            this.userControlModuleRelatedEntryLoanPartner.Module = null;
            this.userControlModuleRelatedEntryLoanPartner.Name = "userControlModuleRelatedEntryLoanPartner";
            this.helpProvider.SetShowHelp(this.userControlModuleRelatedEntryLoanPartner, true);
            this.userControlModuleRelatedEntryLoanPartner.ShowInfo = false;
            this.userControlModuleRelatedEntryLoanPartner.Size = new System.Drawing.Size(589, 21);
            this.userControlModuleRelatedEntryLoanPartner.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryLoanPartner.TabIndex = 12;
            // 
            // labelLoanEnd
            // 
            this.labelLoanEnd.AutoSize = true;
            this.labelLoanEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLoanEnd.Location = new System.Drawing.Point(358, 129);
            this.labelLoanEnd.Name = "labelLoanEnd";
            this.labelLoanEnd.Size = new System.Drawing.Size(30, 26);
            this.labelLoanEnd.TabIndex = 13;
            this.labelLoanEnd.Text = "End:";
            this.labelLoanEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePickerLoanBegin
            // 
            this.dateTimePickerLoanBegin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerLoanBegin.Location = new System.Drawing.Point(337, 132);
            this.dateTimePickerLoanBegin.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.dateTimePickerLoanBegin.Name = "dateTimePickerLoanBegin";
            this.dateTimePickerLoanBegin.Size = new System.Drawing.Size(15, 20);
            this.dateTimePickerLoanBegin.TabIndex = 14;
            this.dateTimePickerLoanBegin.CloseUp += new System.EventHandler(this.dateTimePickerLoanBegin_CloseUp);
            // 
            // textBoxLoanEnd
            // 
            this.textBoxLoanEnd.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.loanBindingSource, "LoanEnd", true));
            this.textBoxLoanEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLoanEnd.Location = new System.Drawing.Point(394, 132);
            this.textBoxLoanEnd.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.textBoxLoanEnd.Name = "textBoxLoanEnd";
            this.textBoxLoanEnd.Size = new System.Drawing.Size(243, 20);
            this.textBoxLoanEnd.TabIndex = 15;
            // 
            // dateTimePickerLoanEnd
            // 
            this.dateTimePickerLoanEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerLoanEnd.Location = new System.Drawing.Point(637, 132);
            this.dateTimePickerLoanEnd.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.dateTimePickerLoanEnd.Name = "dateTimePickerLoanEnd";
            this.dateTimePickerLoanEnd.Size = new System.Drawing.Size(16, 20);
            this.dateTimePickerLoanEnd.TabIndex = 16;
            this.dateTimePickerLoanEnd.CloseUp += new System.EventHandler(this.dateTimePickerLoanEnd_CloseUp);
            // 
            // labelLoanComment
            // 
            this.labelLoanComment.AutoSize = true;
            this.labelLoanComment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLoanComment.Location = new System.Drawing.Point(3, 213);
            this.labelLoanComment.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelLoanComment.Name = "labelLoanComment";
            this.labelLoanComment.Size = new System.Drawing.Size(55, 90);
            this.labelLoanComment.TabIndex = 17;
            this.labelLoanComment.Text = "Comment:";
            this.labelLoanComment.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxLoanComment
            // 
            this.tableLayoutPanelCollection.SetColumnSpan(this.textBoxLoanComment, 6);
            this.textBoxLoanComment.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.loanBindingSource, "LoanComment", true));
            this.textBoxLoanComment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLoanComment.Location = new System.Drawing.Point(64, 210);
            this.textBoxLoanComment.Multiline = true;
            this.textBoxLoanComment.Name = "textBoxLoanComment";
            this.textBoxLoanComment.Size = new System.Drawing.Size(589, 90);
            this.textBoxLoanComment.TabIndex = 18;
            // 
            // labelInternalNotes
            // 
            this.labelInternalNotes.AutoSize = true;
            this.labelInternalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInternalNotes.Location = new System.Drawing.Point(3, 336);
            this.labelInternalNotes.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelInternalNotes.Name = "labelInternalNotes";
            this.labelInternalNotes.Size = new System.Drawing.Size(55, 83);
            this.labelInternalNotes.TabIndex = 19;
            this.labelInternalNotes.Text = "Int. notes:";
            this.labelInternalNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxInternalNotes
            // 
            this.tableLayoutPanelCollection.SetColumnSpan(this.textBoxInternalNotes, 6);
            this.textBoxInternalNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.loanBindingSource, "InternalNotes", true));
            this.textBoxInternalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxInternalNotes.Location = new System.Drawing.Point(64, 333);
            this.textBoxInternalNotes.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.textBoxInternalNotes.Multiline = true;
            this.textBoxInternalNotes.Name = "textBoxInternalNotes";
            this.textBoxInternalNotes.Size = new System.Drawing.Size(592, 83);
            this.textBoxInternalNotes.TabIndex = 20;
            // 
            // labelResponsible
            // 
            this.labelResponsible.AutoSize = true;
            this.labelResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelResponsible.Location = new System.Drawing.Point(3, 419);
            this.labelResponsible.Name = "labelResponsible";
            this.labelResponsible.Size = new System.Drawing.Size(55, 30);
            this.labelResponsible.TabIndex = 21;
            this.labelResponsible.Text = "Respons.:";
            this.labelResponsible.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlModuleRelatedEntryResponsible
            // 
            this.userControlModuleRelatedEntryResponsible.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelCollection.SetColumnSpan(this.userControlModuleRelatedEntryResponsible, 6);
            this.userControlModuleRelatedEntryResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryResponsible.Domain = "";
            this.helpProvider.SetHelpKeyword(this.userControlModuleRelatedEntryResponsible, "Module related entry");
            this.helpProvider.SetHelpNavigator(this.userControlModuleRelatedEntryResponsible, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.userControlModuleRelatedEntryResponsible.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryResponsible.Location = new System.Drawing.Point(64, 422);
            this.userControlModuleRelatedEntryResponsible.Module = null;
            this.userControlModuleRelatedEntryResponsible.Name = "userControlModuleRelatedEntryResponsible";
            this.helpProvider.SetShowHelp(this.userControlModuleRelatedEntryResponsible, true);
            this.userControlModuleRelatedEntryResponsible.ShowInfo = false;
            this.userControlModuleRelatedEntryResponsible.Size = new System.Drawing.Size(589, 24);
            this.userControlModuleRelatedEntryResponsible.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryResponsible.TabIndex = 22;
            // 
            // labelInvestigator
            // 
            this.labelInvestigator.AutoSize = true;
            this.labelInvestigator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInvestigator.Location = new System.Drawing.Point(3, 181);
            this.labelInvestigator.Name = "labelInvestigator";
            this.labelInvestigator.Size = new System.Drawing.Size(55, 26);
            this.labelInvestigator.TabIndex = 24;
            this.labelInvestigator.Text = "Invest.:";
            this.labelInvestigator.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxInvestigator
            // 
            this.tableLayoutPanelCollection.SetColumnSpan(this.textBoxInvestigator, 6);
            this.textBoxInvestigator.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.loanBindingSource, "Investigator", true));
            this.textBoxInvestigator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxInvestigator.Location = new System.Drawing.Point(64, 184);
            this.textBoxInvestigator.Name = "textBoxInvestigator";
            this.textBoxInvestigator.Size = new System.Drawing.Size(589, 20);
            this.textBoxInvestigator.TabIndex = 25;
            this.textBoxInvestigator.Text = "For study by";
            // 
            // comboBoxLoanCommentAdd
            // 
            this.tableLayoutPanelCollection.SetColumnSpan(this.comboBoxLoanCommentAdd, 5);
            this.comboBoxLoanCommentAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxLoanCommentAdd.FormattingEnabled = true;
            this.comboBoxLoanCommentAdd.Location = new System.Drawing.Point(94, 306);
            this.comboBoxLoanCommentAdd.Name = "comboBoxLoanCommentAdd";
            this.comboBoxLoanCommentAdd.Size = new System.Drawing.Size(559, 21);
            this.comboBoxLoanCommentAdd.TabIndex = 26;
            // 
            // buttonLoanCommentInsertSelected
            // 
            this.buttonLoanCommentInsertSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLoanCommentInsertSelected.Image = global::DiversityCollection.Resource.ArrowUp;
            this.buttonLoanCommentInsertSelected.Location = new System.Drawing.Point(64, 306);
            this.buttonLoanCommentInsertSelected.Name = "buttonLoanCommentInsertSelected";
            this.buttonLoanCommentInsertSelected.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.buttonLoanCommentInsertSelected.Size = new System.Drawing.Size(24, 21);
            this.buttonLoanCommentInsertSelected.TabIndex = 27;
            this.buttonLoanCommentInsertSelected.UseVisualStyleBackColor = true;
            // 
            // labelLoanPartnerAddress
            // 
            this.labelLoanPartnerAddress.AutoSize = true;
            this.tableLayoutPanelCollection.SetColumnSpan(this.labelLoanPartnerAddress, 6);
            this.labelLoanPartnerAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLoanPartnerAddress.Location = new System.Drawing.Point(64, 116);
            this.labelLoanPartnerAddress.Name = "labelLoanPartnerAddress";
            this.labelLoanPartnerAddress.Size = new System.Drawing.Size(589, 13);
            this.labelLoanPartnerAddress.TabIndex = 28;
            this.labelLoanPartnerAddress.Visible = false;
            // 
            // labelInitialNumberOfSpecimen
            // 
            this.labelInitialNumberOfSpecimen.AutoSize = true;
            this.tableLayoutPanelCollection.SetColumnSpan(this.labelInitialNumberOfSpecimen, 2);
            this.labelInitialNumberOfSpecimen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInitialNumberOfSpecimen.Location = new System.Drawing.Point(340, 155);
            this.labelInitialNumberOfSpecimen.Name = "labelInitialNumberOfSpecimen";
            this.labelInitialNumberOfSpecimen.Size = new System.Drawing.Size(48, 26);
            this.labelInitialNumberOfSpecimen.TabIndex = 29;
            this.labelInitialNumberOfSpecimen.Text = "Spec.:";
            this.labelInitialNumberOfSpecimen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxInitialNumberOfSpecimen
            // 
            this.tableLayoutPanelCollection.SetColumnSpan(this.textBoxInitialNumberOfSpecimen, 2);
            this.textBoxInitialNumberOfSpecimen.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.loanBindingSource, "InitialNumberOfSpecimen", true));
            this.textBoxInitialNumberOfSpecimen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxInitialNumberOfSpecimen.Location = new System.Drawing.Point(394, 158);
            this.textBoxInitialNumberOfSpecimen.Name = "textBoxInitialNumberOfSpecimen";
            this.textBoxInitialNumberOfSpecimen.ReadOnly = true;
            this.textBoxInitialNumberOfSpecimen.Size = new System.Drawing.Size(259, 20);
            this.textBoxInitialNumberOfSpecimen.TabIndex = 30;
            // 
            // tabPageSending
            // 
            this.tabPageSending.Controls.Add(this.tableLayoutPanelSending);
            this.tabPageSending.ImageKey = "LoanOut.ico";
            this.tabPageSending.Location = new System.Drawing.Point(4, 23);
            this.tabPageSending.Name = "tabPageSending";
            this.tabPageSending.Size = new System.Drawing.Size(656, 449);
            this.tabPageSending.TabIndex = 1;
            this.tabPageSending.Text = "Sending";
            this.tabPageSending.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelSending
            // 
            this.tableLayoutPanelSending.ColumnCount = 5;
            this.tableLayoutPanelSending.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSending.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSending.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSending.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSending.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanelSending.Controls.Add(this.panelWebbrowserSending, 0, 1);
            this.tableLayoutPanelSending.Controls.Add(this.labelSendingSchema, 0, 0);
            this.tableLayoutPanelSending.Controls.Add(this.textBoxSendingSchema, 1, 0);
            this.tableLayoutPanelSending.Controls.Add(this.toolStripSending, 2, 0);
            this.tableLayoutPanelSending.Controls.Add(this.buttonSendingScanner, 3, 0);
            this.tableLayoutPanelSending.Controls.Add(this.textBoxSendingAccessionNumber, 4, 0);
            this.tableLayoutPanelSending.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSending.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSending.Name = "tableLayoutPanelSending";
            this.tableLayoutPanelSending.RowCount = 2;
            this.tableLayoutPanelSending.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSending.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSending.Size = new System.Drawing.Size(656, 449);
            this.tableLayoutPanelSending.TabIndex = 0;
            // 
            // panelWebbrowserSending
            // 
            this.panelWebbrowserSending.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanelSending.SetColumnSpan(this.panelWebbrowserSending, 5);
            this.panelWebbrowserSending.Controls.Add(this.webBrowserSending);
            this.panelWebbrowserSending.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWebbrowserSending.Location = new System.Drawing.Point(3, 29);
            this.panelWebbrowserSending.Name = "panelWebbrowserSending";
            this.panelWebbrowserSending.Padding = new System.Windows.Forms.Padding(3);
            this.panelWebbrowserSending.Size = new System.Drawing.Size(650, 417);
            this.panelWebbrowserSending.TabIndex = 3;
            // 
            // webBrowserSending
            // 
            this.webBrowserSending.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserSending.Location = new System.Drawing.Point(3, 3);
            this.webBrowserSending.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserSending.Name = "webBrowserSending";
            this.webBrowserSending.Size = new System.Drawing.Size(640, 407);
            this.webBrowserSending.TabIndex = 1;
            // 
            // labelSendingSchema
            // 
            this.labelSendingSchema.AutoSize = true;
            this.labelSendingSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSendingSchema.Location = new System.Drawing.Point(3, 0);
            this.labelSendingSchema.Name = "labelSendingSchema";
            this.labelSendingSchema.Size = new System.Drawing.Size(49, 26);
            this.labelSendingSchema.TabIndex = 4;
            this.labelSendingSchema.Text = "Schema:";
            this.labelSendingSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxSendingSchema
            // 
            this.textBoxSendingSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSendingSchema.Location = new System.Drawing.Point(58, 3);
            this.textBoxSendingSchema.Name = "textBoxSendingSchema";
            this.textBoxSendingSchema.Size = new System.Drawing.Size(391, 20);
            this.textBoxSendingSchema.TabIndex = 5;
            // 
            // toolStripSending
            // 
            this.toolStripSending.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripSending.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripSending.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonOpenSendingSchemaFile,
            this.toolStripSeparatorSending1,
            this.toolStripButtonSendingPreview,
            this.toolStripButtonSendingPrint,
            this.toolStripButtonSendingSave});
            this.toolStripSending.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStripSending.Location = new System.Drawing.Point(452, 0);
            this.toolStripSending.Name = "toolStripSending";
            this.toolStripSending.Size = new System.Drawing.Size(99, 26);
            this.toolStripSending.TabIndex = 6;
            this.toolStripSending.Text = "toolStrip1";
            // 
            // toolStripButtonOpenSendingSchemaFile
            // 
            this.toolStripButtonOpenSendingSchemaFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonOpenSendingSchemaFile.Image = global::DiversityCollection.Resource.Open;
            this.toolStripButtonOpenSendingSchemaFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpenSendingSchemaFile.Name = "toolStripButtonOpenSendingSchemaFile";
            this.toolStripButtonOpenSendingSchemaFile.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonOpenSendingSchemaFile.Text = "Open schema file";
            this.toolStripButtonOpenSendingSchemaFile.Click += new System.EventHandler(this.toolStripButtonOpenSendingSchemaFile_Click);
            // 
            // toolStripSeparatorSending1
            // 
            this.toolStripSeparatorSending1.Name = "toolStripSeparatorSending1";
            this.toolStripSeparatorSending1.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonSendingPreview
            // 
            this.toolStripButtonSendingPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSendingPreview.Image = global::DiversityCollection.Resource.List;
            this.toolStripButtonSendingPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSendingPreview.Name = "toolStripButtonSendingPreview";
            this.toolStripButtonSendingPreview.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonSendingPreview.Text = "Create preview";
            this.toolStripButtonSendingPreview.Click += new System.EventHandler(this.toolStripButtonSendingPreview_Click);
            // 
            // toolStripButtonSendingPrint
            // 
            this.toolStripButtonSendingPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSendingPrint.Image = global::DiversityCollection.Resource.Print;
            this.toolStripButtonSendingPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSendingPrint.Name = "toolStripButtonSendingPrint";
            this.toolStripButtonSendingPrint.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonSendingPrint.Text = "Print form";
            this.toolStripButtonSendingPrint.Click += new System.EventHandler(this.toolStripButtonSendingPrint_Click);
            // 
            // toolStripButtonSendingSave
            // 
            this.toolStripButtonSendingSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSendingSave.Image = global::DiversityCollection.Resource.Save;
            this.toolStripButtonSendingSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSendingSave.Name = "toolStripButtonSendingSave";
            this.toolStripButtonSendingSave.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonSendingSave.Text = "Save form";
            this.toolStripButtonSendingSave.Click += new System.EventHandler(this.toolStripButtonSendingSave_Click);
            // 
            // buttonSendingScanner
            // 
            this.buttonSendingScanner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSendingScanner.FlatAppearance.BorderSize = 0;
            this.buttonSendingScanner.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSendingScanner.Image = global::DiversityCollection.Resource.ScannerBarcode;
            this.buttonSendingScanner.Location = new System.Drawing.Point(551, 0);
            this.buttonSendingScanner.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSendingScanner.Name = "buttonSendingScanner";
            this.buttonSendingScanner.Size = new System.Drawing.Size(25, 26);
            this.buttonSendingScanner.TabIndex = 7;
            this.buttonSendingScanner.UseVisualStyleBackColor = true;
            // 
            // textBoxSendingAccessionNumber
            // 
            this.textBoxSendingAccessionNumber.BackColor = System.Drawing.Color.Pink;
            this.textBoxSendingAccessionNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSendingAccessionNumber.Location = new System.Drawing.Point(579, 3);
            this.textBoxSendingAccessionNumber.Name = "textBoxSendingAccessionNumber";
            this.textBoxSendingAccessionNumber.Size = new System.Drawing.Size(74, 20);
            this.textBoxSendingAccessionNumber.TabIndex = 8;
            this.textBoxSendingAccessionNumber.TextChanged += new System.EventHandler(this.textBoxSendingAccessionNumber_TextChanged);
            this.textBoxSendingAccessionNumber.MouseEnter += new System.EventHandler(this.textBoxSendingAccessionNumber_MouseEnter);
            // 
            // tabPageConfirmation
            // 
            this.tabPageConfirmation.Controls.Add(this.tableLayoutPanelReminder);
            this.tabPageConfirmation.ImageKey = "LoanQuery.ico";
            this.tabPageConfirmation.Location = new System.Drawing.Point(4, 23);
            this.tabPageConfirmation.Name = "tabPageConfirmation";
            this.tabPageConfirmation.Size = new System.Drawing.Size(656, 449);
            this.tabPageConfirmation.TabIndex = 6;
            this.tabPageConfirmation.Text = "Confirmation";
            this.tabPageConfirmation.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelReminder
            // 
            this.tableLayoutPanelReminder.ColumnCount = 3;
            this.tableLayoutPanelReminder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelReminder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelReminder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelReminder.Controls.Add(this.panelReminderWebbrowser, 0, 1);
            this.tableLayoutPanelReminder.Controls.Add(this.labelConfirmationSchema, 0, 0);
            this.tableLayoutPanelReminder.Controls.Add(this.textBoxConfirmationSchema, 1, 0);
            this.tableLayoutPanelReminder.Controls.Add(this.toolStripConfirmation, 2, 0);
            this.tableLayoutPanelReminder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelReminder.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelReminder.Name = "tableLayoutPanelReminder";
            this.tableLayoutPanelReminder.RowCount = 2;
            this.tableLayoutPanelReminder.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelReminder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelReminder.Size = new System.Drawing.Size(656, 449);
            this.tableLayoutPanelReminder.TabIndex = 2;
            // 
            // panelReminderWebbrowser
            // 
            this.panelReminderWebbrowser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanelReminder.SetColumnSpan(this.panelReminderWebbrowser, 3);
            this.panelReminderWebbrowser.Controls.Add(this.webBrowserConfirmation);
            this.panelReminderWebbrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelReminderWebbrowser.Location = new System.Drawing.Point(3, 29);
            this.panelReminderWebbrowser.Name = "panelReminderWebbrowser";
            this.panelReminderWebbrowser.Padding = new System.Windows.Forms.Padding(3);
            this.panelReminderWebbrowser.Size = new System.Drawing.Size(650, 417);
            this.panelReminderWebbrowser.TabIndex = 3;
            // 
            // webBrowserConfirmation
            // 
            this.webBrowserConfirmation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserConfirmation.Location = new System.Drawing.Point(3, 3);
            this.webBrowserConfirmation.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserConfirmation.Name = "webBrowserConfirmation";
            this.webBrowserConfirmation.Size = new System.Drawing.Size(640, 407);
            this.webBrowserConfirmation.TabIndex = 1;
            // 
            // labelConfirmationSchema
            // 
            this.labelConfirmationSchema.AutoSize = true;
            this.labelConfirmationSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelConfirmationSchema.Location = new System.Drawing.Point(3, 0);
            this.labelConfirmationSchema.Name = "labelConfirmationSchema";
            this.labelConfirmationSchema.Size = new System.Drawing.Size(49, 26);
            this.labelConfirmationSchema.TabIndex = 4;
            this.labelConfirmationSchema.Text = "Schema:";
            this.labelConfirmationSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxConfirmationSchema
            // 
            this.textBoxConfirmationSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxConfirmationSchema.Location = new System.Drawing.Point(58, 3);
            this.textBoxConfirmationSchema.Name = "textBoxConfirmationSchema";
            this.textBoxConfirmationSchema.Size = new System.Drawing.Size(496, 20);
            this.textBoxConfirmationSchema.TabIndex = 5;
            // 
            // toolStripConfirmation
            // 
            this.toolStripConfirmation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripConfirmation.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripConfirmation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonConfirmationOpenSchema,
            this.toolStripSeparatorConfirmation,
            this.toolStripButtonConfirmationPreview,
            this.toolStripButtonConfirmationPrint,
            this.toolStripButtonConfirmationSave});
            this.toolStripConfirmation.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStripConfirmation.Location = new System.Drawing.Point(557, 0);
            this.toolStripConfirmation.Name = "toolStripConfirmation";
            this.toolStripConfirmation.Size = new System.Drawing.Size(99, 26);
            this.toolStripConfirmation.TabIndex = 6;
            this.toolStripConfirmation.Text = "toolStrip1";
            // 
            // toolStripButtonConfirmationOpenSchema
            // 
            this.toolStripButtonConfirmationOpenSchema.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonConfirmationOpenSchema.Image = global::DiversityCollection.Resource.Open;
            this.toolStripButtonConfirmationOpenSchema.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonConfirmationOpenSchema.Name = "toolStripButtonConfirmationOpenSchema";
            this.toolStripButtonConfirmationOpenSchema.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonConfirmationOpenSchema.Text = "Open schema file";
            this.toolStripButtonConfirmationOpenSchema.Click += new System.EventHandler(this.toolStripButtonConfirmationOpenSchema_Click);
            // 
            // toolStripSeparatorConfirmation
            // 
            this.toolStripSeparatorConfirmation.Name = "toolStripSeparatorConfirmation";
            this.toolStripSeparatorConfirmation.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonConfirmationPreview
            // 
            this.toolStripButtonConfirmationPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonConfirmationPreview.Image = global::DiversityCollection.Resource.List;
            this.toolStripButtonConfirmationPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonConfirmationPreview.Name = "toolStripButtonConfirmationPreview";
            this.toolStripButtonConfirmationPreview.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonConfirmationPreview.Text = "Create preview";
            this.toolStripButtonConfirmationPreview.Click += new System.EventHandler(this.toolStripButtonConfirmationPreview_Click);
            // 
            // toolStripButtonConfirmationPrint
            // 
            this.toolStripButtonConfirmationPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonConfirmationPrint.Image = global::DiversityCollection.Resource.Print;
            this.toolStripButtonConfirmationPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonConfirmationPrint.Name = "toolStripButtonConfirmationPrint";
            this.toolStripButtonConfirmationPrint.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonConfirmationPrint.Text = "Print form";
            this.toolStripButtonConfirmationPrint.Click += new System.EventHandler(this.toolStripButtonConfirmationPrint_Click);
            // 
            // toolStripButtonConfirmationSave
            // 
            this.toolStripButtonConfirmationSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonConfirmationSave.Image = global::DiversityCollection.Resource.Save;
            this.toolStripButtonConfirmationSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonConfirmationSave.Name = "toolStripButtonConfirmationSave";
            this.toolStripButtonConfirmationSave.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonConfirmationSave.Text = "Save form";
            this.toolStripButtonConfirmationSave.Click += new System.EventHandler(this.toolStripButtonConfirmationSave_Click);
            // 
            // tabPageReminder
            // 
            this.tabPageReminder.Controls.Add(this.tableLayoutPanelAdmonition);
            this.tabPageReminder.ImageKey = "LoanReminder.ico";
            this.tabPageReminder.Location = new System.Drawing.Point(4, 23);
            this.tabPageReminder.Name = "tabPageReminder";
            this.tabPageReminder.Size = new System.Drawing.Size(656, 449);
            this.tabPageReminder.TabIndex = 2;
            this.tabPageReminder.Text = "Reminder";
            this.tabPageReminder.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelAdmonition
            // 
            this.tableLayoutPanelAdmonition.ColumnCount = 3;
            this.tableLayoutPanelAdmonition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAdmonition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAdmonition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAdmonition.Controls.Add(this.panelAdmonitionWebbrowser, 0, 1);
            this.tableLayoutPanelAdmonition.Controls.Add(this.labelReminderSchema, 0, 0);
            this.tableLayoutPanelAdmonition.Controls.Add(this.textBoxReminderSchema, 1, 0);
            this.tableLayoutPanelAdmonition.Controls.Add(this.toolStripReminder, 2, 0);
            this.tableLayoutPanelAdmonition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelAdmonition.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelAdmonition.Name = "tableLayoutPanelAdmonition";
            this.tableLayoutPanelAdmonition.RowCount = 2;
            this.tableLayoutPanelAdmonition.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAdmonition.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAdmonition.Size = new System.Drawing.Size(656, 449);
            this.tableLayoutPanelAdmonition.TabIndex = 3;
            // 
            // panelAdmonitionWebbrowser
            // 
            this.panelAdmonitionWebbrowser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanelAdmonition.SetColumnSpan(this.panelAdmonitionWebbrowser, 3);
            this.panelAdmonitionWebbrowser.Controls.Add(this.webBrowserReminder);
            this.panelAdmonitionWebbrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAdmonitionWebbrowser.Location = new System.Drawing.Point(3, 29);
            this.panelAdmonitionWebbrowser.Name = "panelAdmonitionWebbrowser";
            this.panelAdmonitionWebbrowser.Padding = new System.Windows.Forms.Padding(3);
            this.panelAdmonitionWebbrowser.Size = new System.Drawing.Size(650, 417);
            this.panelAdmonitionWebbrowser.TabIndex = 3;
            // 
            // webBrowserReminder
            // 
            this.webBrowserReminder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserReminder.Location = new System.Drawing.Point(3, 3);
            this.webBrowserReminder.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserReminder.Name = "webBrowserReminder";
            this.webBrowserReminder.Size = new System.Drawing.Size(640, 407);
            this.webBrowserReminder.TabIndex = 1;
            // 
            // labelReminderSchema
            // 
            this.labelReminderSchema.AutoSize = true;
            this.labelReminderSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReminderSchema.Location = new System.Drawing.Point(3, 0);
            this.labelReminderSchema.Name = "labelReminderSchema";
            this.labelReminderSchema.Size = new System.Drawing.Size(49, 26);
            this.labelReminderSchema.TabIndex = 4;
            this.labelReminderSchema.Text = "Schema:";
            this.labelReminderSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxReminderSchema
            // 
            this.textBoxReminderSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxReminderSchema.Location = new System.Drawing.Point(58, 3);
            this.textBoxReminderSchema.Name = "textBoxReminderSchema";
            this.textBoxReminderSchema.Size = new System.Drawing.Size(496, 20);
            this.textBoxReminderSchema.TabIndex = 5;
            // 
            // toolStripReminder
            // 
            this.toolStripReminder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripReminder.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripReminder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonReminderOpenSchema,
            this.toolStripSeparatorReminder,
            this.toolStripButtonReminderPreview,
            this.toolStripButtonReminderPrint,
            this.toolStripButtonReminderSave});
            this.toolStripReminder.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStripReminder.Location = new System.Drawing.Point(557, 0);
            this.toolStripReminder.Name = "toolStripReminder";
            this.toolStripReminder.Size = new System.Drawing.Size(99, 26);
            this.toolStripReminder.TabIndex = 6;
            this.toolStripReminder.Text = "toolStrip1";
            // 
            // toolStripButtonReminderOpenSchema
            // 
            this.toolStripButtonReminderOpenSchema.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonReminderOpenSchema.Image = global::DiversityCollection.Resource.Open;
            this.toolStripButtonReminderOpenSchema.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReminderOpenSchema.Name = "toolStripButtonReminderOpenSchema";
            this.toolStripButtonReminderOpenSchema.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonReminderOpenSchema.Text = "Open schema file";
            this.toolStripButtonReminderOpenSchema.Click += new System.EventHandler(this.toolStripButtonReminderOpenSchema_Click);
            // 
            // toolStripSeparatorReminder
            // 
            this.toolStripSeparatorReminder.Name = "toolStripSeparatorReminder";
            this.toolStripSeparatorReminder.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonReminderPreview
            // 
            this.toolStripButtonReminderPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonReminderPreview.Image = global::DiversityCollection.Resource.List;
            this.toolStripButtonReminderPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReminderPreview.Name = "toolStripButtonReminderPreview";
            this.toolStripButtonReminderPreview.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonReminderPreview.Text = "Create preview";
            this.toolStripButtonReminderPreview.Click += new System.EventHandler(this.toolStripButtonReminderPreview_Click);
            // 
            // toolStripButtonReminderPrint
            // 
            this.toolStripButtonReminderPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonReminderPrint.Image = global::DiversityCollection.Resource.Print;
            this.toolStripButtonReminderPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReminderPrint.Name = "toolStripButtonReminderPrint";
            this.toolStripButtonReminderPrint.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonReminderPrint.Text = "Print form";
            this.toolStripButtonReminderPrint.Click += new System.EventHandler(this.toolStripButtonReminderPrint_Click);
            // 
            // toolStripButtonReminderSave
            // 
            this.toolStripButtonReminderSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonReminderSave.Image = global::DiversityCollection.Resource.Save;
            this.toolStripButtonReminderSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReminderSave.Name = "toolStripButtonReminderSave";
            this.toolStripButtonReminderSave.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonReminderSave.Text = "Save form";
            this.toolStripButtonReminderSave.Click += new System.EventHandler(this.toolStripButtonReminderSave_Click);
            // 
            // tabPagePartialReturn
            // 
            this.tabPagePartialReturn.Controls.Add(this.tableLayoutPanelPartialReturn);
            this.tabPagePartialReturn.ImageKey = "LoanPartIn.ico";
            this.tabPagePartialReturn.Location = new System.Drawing.Point(4, 23);
            this.tabPagePartialReturn.Name = "tabPagePartialReturn";
            this.tabPagePartialReturn.Size = new System.Drawing.Size(656, 449);
            this.tabPagePartialReturn.TabIndex = 4;
            this.tabPagePartialReturn.Text = "Partial return";
            this.tabPagePartialReturn.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelPartialReturn
            // 
            this.tableLayoutPanelPartialReturn.ColumnCount = 7;
            this.tableLayoutPanelPartialReturn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPartialReturn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPartialReturn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPartialReturn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPartialReturn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPartialReturn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPartialReturn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPartialReturn.Controls.Add(this.panelPartialReturnWebbrowser, 0, 2);
            this.tableLayoutPanelPartialReturn.Controls.Add(this.labelPartialReturnSchema, 0, 0);
            this.tableLayoutPanelPartialReturn.Controls.Add(this.textBoxPartialReturnSchema, 1, 0);
            this.tableLayoutPanelPartialReturn.Controls.Add(this.toolStripPartialReturn, 6, 0);
            this.tableLayoutPanelPartialReturn.Controls.Add(this.buttonPartialReturnScanner, 5, 1);
            this.tableLayoutPanelPartialReturn.Controls.Add(this.textBoxPartialReturnAccessionNumber, 6, 1);
            this.tableLayoutPanelPartialReturn.Controls.Add(this.labelPartialReturnDate, 0, 1);
            this.tableLayoutPanelPartialReturn.Controls.Add(this.dateTimePickerPartialReturn, 1, 1);
            this.tableLayoutPanelPartialReturn.Controls.Add(this.labelPartialReturnReturnedSpecimen, 2, 1);
            this.tableLayoutPanelPartialReturn.Controls.Add(this.textBoxPartialReturnReturnedSpecimen, 3, 1);
            this.tableLayoutPanelPartialReturn.Controls.Add(this.listBoxPartialReturnSpecimen, 6, 2);
            this.tableLayoutPanelPartialReturn.Controls.Add(this.comboBoxPartialReturn, 4, 1);
            this.tableLayoutPanelPartialReturn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelPartialReturn.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelPartialReturn.Name = "tableLayoutPanelPartialReturn";
            this.tableLayoutPanelPartialReturn.RowCount = 3;
            this.tableLayoutPanelPartialReturn.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPartialReturn.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPartialReturn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPartialReturn.Size = new System.Drawing.Size(656, 449);
            this.tableLayoutPanelPartialReturn.TabIndex = 1;
            // 
            // panelPartialReturnWebbrowser
            // 
            this.panelPartialReturnWebbrowser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanelPartialReturn.SetColumnSpan(this.panelPartialReturnWebbrowser, 6);
            this.panelPartialReturnWebbrowser.Controls.Add(this.webBrowserPartialReturn);
            this.panelPartialReturnWebbrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPartialReturnWebbrowser.Location = new System.Drawing.Point(3, 56);
            this.panelPartialReturnWebbrowser.Name = "panelPartialReturnWebbrowser";
            this.panelPartialReturnWebbrowser.Padding = new System.Windows.Forms.Padding(3);
            this.panelPartialReturnWebbrowser.Size = new System.Drawing.Size(524, 390);
            this.panelPartialReturnWebbrowser.TabIndex = 3;
            // 
            // webBrowserPartialReturn
            // 
            this.webBrowserPartialReturn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserPartialReturn.Location = new System.Drawing.Point(3, 3);
            this.webBrowserPartialReturn.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserPartialReturn.Name = "webBrowserPartialReturn";
            this.webBrowserPartialReturn.Size = new System.Drawing.Size(514, 380);
            this.webBrowserPartialReturn.TabIndex = 1;
            // 
            // labelPartialReturnSchema
            // 
            this.labelPartialReturnSchema.AutoSize = true;
            this.labelPartialReturnSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPartialReturnSchema.Location = new System.Drawing.Point(3, 0);
            this.labelPartialReturnSchema.Name = "labelPartialReturnSchema";
            this.labelPartialReturnSchema.Size = new System.Drawing.Size(49, 26);
            this.labelPartialReturnSchema.TabIndex = 4;
            this.labelPartialReturnSchema.Text = "Schema:";
            this.labelPartialReturnSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPartialReturnSchema
            // 
            this.tableLayoutPanelPartialReturn.SetColumnSpan(this.textBoxPartialReturnSchema, 5);
            this.textBoxPartialReturnSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPartialReturnSchema.Location = new System.Drawing.Point(58, 3);
            this.textBoxPartialReturnSchema.Name = "textBoxPartialReturnSchema";
            this.textBoxPartialReturnSchema.Size = new System.Drawing.Size(469, 20);
            this.textBoxPartialReturnSchema.TabIndex = 5;
            // 
            // toolStripPartialReturn
            // 
            this.toolStripPartialReturn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripPartialReturn.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripPartialReturn.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonPartialReturnOpenSchema,
            this.toolStripSeparatorPartialReturn1,
            this.toolStripButtonPartialReturnPreview,
            this.toolStripButtonPartialReturnPrint,
            this.toolStripButtonPartialReturnSave});
            this.toolStripPartialReturn.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStripPartialReturn.Location = new System.Drawing.Point(530, 0);
            this.toolStripPartialReturn.Name = "toolStripPartialReturn";
            this.toolStripPartialReturn.Size = new System.Drawing.Size(126, 26);
            this.toolStripPartialReturn.TabIndex = 6;
            this.toolStripPartialReturn.Text = "toolStrip1";
            // 
            // toolStripButtonPartialReturnOpenSchema
            // 
            this.toolStripButtonPartialReturnOpenSchema.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPartialReturnOpenSchema.Image = global::DiversityCollection.Resource.Open;
            this.toolStripButtonPartialReturnOpenSchema.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPartialReturnOpenSchema.Name = "toolStripButtonPartialReturnOpenSchema";
            this.toolStripButtonPartialReturnOpenSchema.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonPartialReturnOpenSchema.Text = "Open schema file";
            this.toolStripButtonPartialReturnOpenSchema.Click += new System.EventHandler(this.toolStripButtonPartialReturnOpenSchema_Click);
            // 
            // toolStripSeparatorPartialReturn1
            // 
            this.toolStripSeparatorPartialReturn1.Name = "toolStripSeparatorPartialReturn1";
            this.toolStripSeparatorPartialReturn1.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonPartialReturnPreview
            // 
            this.toolStripButtonPartialReturnPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPartialReturnPreview.Image = global::DiversityCollection.Resource.List;
            this.toolStripButtonPartialReturnPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPartialReturnPreview.Name = "toolStripButtonPartialReturnPreview";
            this.toolStripButtonPartialReturnPreview.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonPartialReturnPreview.Text = "Create preview";
            this.toolStripButtonPartialReturnPreview.Click += new System.EventHandler(this.toolStripButtonPartialReturnPreview_Click);
            // 
            // toolStripButtonPartialReturnPrint
            // 
            this.toolStripButtonPartialReturnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPartialReturnPrint.Image = global::DiversityCollection.Resource.Print;
            this.toolStripButtonPartialReturnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPartialReturnPrint.Name = "toolStripButtonPartialReturnPrint";
            this.toolStripButtonPartialReturnPrint.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonPartialReturnPrint.Text = "Print form";
            this.toolStripButtonPartialReturnPrint.Click += new System.EventHandler(this.toolStripButtonPartialReturnPrint_Click);
            // 
            // toolStripButtonPartialReturnSave
            // 
            this.toolStripButtonPartialReturnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPartialReturnSave.Image = global::DiversityCollection.Resource.Save;
            this.toolStripButtonPartialReturnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPartialReturnSave.Name = "toolStripButtonPartialReturnSave";
            this.toolStripButtonPartialReturnSave.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonPartialReturnSave.Text = "Save form";
            this.toolStripButtonPartialReturnSave.Click += new System.EventHandler(this.toolStripButtonPartialReturnSave_Click);
            // 
            // buttonPartialReturnScanner
            // 
            this.buttonPartialReturnScanner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPartialReturnScanner.FlatAppearance.BorderSize = 0;
            this.buttonPartialReturnScanner.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPartialReturnScanner.Image = global::DiversityCollection.Resource.ScannerBarcode;
            this.buttonPartialReturnScanner.Location = new System.Drawing.Point(505, 26);
            this.buttonPartialReturnScanner.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPartialReturnScanner.Name = "buttonPartialReturnScanner";
            this.buttonPartialReturnScanner.Size = new System.Drawing.Size(25, 27);
            this.buttonPartialReturnScanner.TabIndex = 7;
            this.buttonPartialReturnScanner.UseVisualStyleBackColor = true;
            // 
            // textBoxPartialReturnAccessionNumber
            // 
            this.textBoxPartialReturnAccessionNumber.BackColor = System.Drawing.Color.Pink;
            this.textBoxPartialReturnAccessionNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxPartialReturnAccessionNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPartialReturnAccessionNumber.Location = new System.Drawing.Point(533, 29);
            this.textBoxPartialReturnAccessionNumber.Name = "textBoxPartialReturnAccessionNumber";
            this.textBoxPartialReturnAccessionNumber.Size = new System.Drawing.Size(120, 20);
            this.textBoxPartialReturnAccessionNumber.TabIndex = 8;
            this.textBoxPartialReturnAccessionNumber.TextChanged += new System.EventHandler(this.textBoxPartialReturnAccessionNumber_TextChanged);
            this.textBoxPartialReturnAccessionNumber.MouseEnter += new System.EventHandler(this.textBoxPartialReturnAccessionNumber_MouseEnter);
            // 
            // labelPartialReturnDate
            // 
            this.labelPartialReturnDate.AutoSize = true;
            this.labelPartialReturnDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPartialReturnDate.Location = new System.Drawing.Point(3, 26);
            this.labelPartialReturnDate.Name = "labelPartialReturnDate";
            this.labelPartialReturnDate.Size = new System.Drawing.Size(49, 27);
            this.labelPartialReturnDate.TabIndex = 9;
            this.labelPartialReturnDate.Text = "Date:";
            this.labelPartialReturnDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePickerPartialReturn
            // 
            this.dateTimePickerPartialReturn.CustomFormat = "";
            this.dateTimePickerPartialReturn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerPartialReturn.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerPartialReturn.Location = new System.Drawing.Point(58, 29);
            this.dateTimePickerPartialReturn.Name = "dateTimePickerPartialReturn";
            this.dateTimePickerPartialReturn.Size = new System.Drawing.Size(116, 20);
            this.dateTimePickerPartialReturn.TabIndex = 10;
            // 
            // labelPartialReturnReturnedSpecimen
            // 
            this.labelPartialReturnReturnedSpecimen.AutoSize = true;
            this.labelPartialReturnReturnedSpecimen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPartialReturnReturnedSpecimen.Location = new System.Drawing.Point(180, 26);
            this.labelPartialReturnReturnedSpecimen.Name = "labelPartialReturnReturnedSpecimen";
            this.labelPartialReturnReturnedSpecimen.Size = new System.Drawing.Size(149, 27);
            this.labelPartialReturnReturnedSpecimen.TabIndex = 11;
            this.labelPartialReturnReturnedSpecimen.Text = "Number of returned specimen:";
            this.labelPartialReturnReturnedSpecimen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPartialReturnReturnedSpecimen
            // 
            this.textBoxPartialReturnReturnedSpecimen.Location = new System.Drawing.Point(335, 29);
            this.textBoxPartialReturnReturnedSpecimen.Name = "textBoxPartialReturnReturnedSpecimen";
            this.textBoxPartialReturnReturnedSpecimen.ReadOnly = true;
            this.textBoxPartialReturnReturnedSpecimen.Size = new System.Drawing.Size(40, 20);
            this.textBoxPartialReturnReturnedSpecimen.TabIndex = 12;
            // 
            // listBoxPartialReturnSpecimen
            // 
            this.listBoxPartialReturnSpecimen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxPartialReturnSpecimen.FormattingEnabled = true;
            this.listBoxPartialReturnSpecimen.IntegralHeight = false;
            this.listBoxPartialReturnSpecimen.Location = new System.Drawing.Point(533, 56);
            this.listBoxPartialReturnSpecimen.Name = "listBoxPartialReturnSpecimen";
            this.listBoxPartialReturnSpecimen.Size = new System.Drawing.Size(120, 390);
            this.listBoxPartialReturnSpecimen.TabIndex = 13;
            // 
            // comboBoxPartialReturn
            // 
            this.comboBoxPartialReturn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxPartialReturn.DropDownWidth = 300;
            this.comboBoxPartialReturn.FormattingEnabled = true;
            this.comboBoxPartialReturn.Location = new System.Drawing.Point(381, 29);
            this.comboBoxPartialReturn.Name = "comboBoxPartialReturn";
            this.comboBoxPartialReturn.Size = new System.Drawing.Size(121, 21);
            this.comboBoxPartialReturn.TabIndex = 14;
            this.comboBoxPartialReturn.DropDown += new System.EventHandler(this.comboBoxPartialReturn_DropDown);
            this.comboBoxPartialReturn.SelectionChangeCommitted += new System.EventHandler(this.comboBoxPartialReturn_SelectionChangeCommitted);
            // 
            // tabPageReturn
            // 
            this.tabPageReturn.Controls.Add(this.tableLayoutPanelReturn);
            this.tabPageReturn.ImageKey = "LoanIn.ico";
            this.tabPageReturn.Location = new System.Drawing.Point(4, 23);
            this.tabPageReturn.Name = "tabPageReturn";
            this.tabPageReturn.Size = new System.Drawing.Size(656, 449);
            this.tabPageReturn.TabIndex = 3;
            this.tabPageReturn.Text = "Return";
            this.tabPageReturn.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelReturn
            // 
            this.tableLayoutPanelReturn.ColumnCount = 3;
            this.tableLayoutPanelReturn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelReturn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelReturn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelReturn.Controls.Add(this.panelReturnWebbrowser, 0, 1);
            this.tableLayoutPanelReturn.Controls.Add(this.labelReturnSchema, 0, 0);
            this.tableLayoutPanelReturn.Controls.Add(this.textBoxReturnSchema, 1, 0);
            this.tableLayoutPanelReturn.Controls.Add(this.toolStripReturn, 2, 0);
            this.tableLayoutPanelReturn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelReturn.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelReturn.Name = "tableLayoutPanelReturn";
            this.tableLayoutPanelReturn.RowCount = 2;
            this.tableLayoutPanelReturn.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelReturn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelReturn.Size = new System.Drawing.Size(656, 449);
            this.tableLayoutPanelReturn.TabIndex = 3;
            // 
            // panelReturnWebbrowser
            // 
            this.panelReturnWebbrowser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanelReturn.SetColumnSpan(this.panelReturnWebbrowser, 3);
            this.panelReturnWebbrowser.Controls.Add(this.webBrowserReturn);
            this.panelReturnWebbrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelReturnWebbrowser.Location = new System.Drawing.Point(3, 29);
            this.panelReturnWebbrowser.Name = "panelReturnWebbrowser";
            this.panelReturnWebbrowser.Padding = new System.Windows.Forms.Padding(3);
            this.panelReturnWebbrowser.Size = new System.Drawing.Size(650, 417);
            this.panelReturnWebbrowser.TabIndex = 3;
            // 
            // webBrowserReturn
            // 
            this.webBrowserReturn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserReturn.Location = new System.Drawing.Point(3, 3);
            this.webBrowserReturn.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserReturn.Name = "webBrowserReturn";
            this.webBrowserReturn.Size = new System.Drawing.Size(640, 407);
            this.webBrowserReturn.TabIndex = 1;
            // 
            // labelReturnSchema
            // 
            this.labelReturnSchema.AutoSize = true;
            this.labelReturnSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReturnSchema.Location = new System.Drawing.Point(3, 0);
            this.labelReturnSchema.Name = "labelReturnSchema";
            this.labelReturnSchema.Size = new System.Drawing.Size(49, 26);
            this.labelReturnSchema.TabIndex = 4;
            this.labelReturnSchema.Text = "Schema:";
            this.labelReturnSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxReturnSchema
            // 
            this.textBoxReturnSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxReturnSchema.Location = new System.Drawing.Point(58, 3);
            this.textBoxReturnSchema.Name = "textBoxReturnSchema";
            this.textBoxReturnSchema.Size = new System.Drawing.Size(473, 20);
            this.textBoxReturnSchema.TabIndex = 5;
            // 
            // toolStripReturn
            // 
            this.toolStripReturn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripReturn.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripReturn.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonReturnOpenSchema,
            this.toolStripSeparatorReturn1,
            this.toolStripButtonReturnOK,
            this.toolStripButtonReturnPreview,
            this.toolStripButtonReturnPrint,
            this.toolStripButtonReturnSave});
            this.toolStripReturn.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStripReturn.Location = new System.Drawing.Point(534, 0);
            this.toolStripReturn.Name = "toolStripReturn";
            this.toolStripReturn.Size = new System.Drawing.Size(122, 26);
            this.toolStripReturn.TabIndex = 6;
            this.toolStripReturn.Text = "toolStrip1";
            // 
            // toolStripButtonReturnOpenSchema
            // 
            this.toolStripButtonReturnOpenSchema.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonReturnOpenSchema.Image = global::DiversityCollection.Resource.Open;
            this.toolStripButtonReturnOpenSchema.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReturnOpenSchema.Name = "toolStripButtonReturnOpenSchema";
            this.toolStripButtonReturnOpenSchema.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonReturnOpenSchema.Text = "Open schema file";
            this.toolStripButtonReturnOpenSchema.Click += new System.EventHandler(this.toolStripButtonReturnOpenSchema_Click);
            // 
            // toolStripSeparatorReturn1
            // 
            this.toolStripSeparatorReturn1.Name = "toolStripSeparatorReturn1";
            this.toolStripSeparatorReturn1.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonReturnOK
            // 
            this.toolStripButtonReturnOK.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonReturnOK.Image = global::DiversityCollection.Resource.OK;
            this.toolStripButtonReturnOK.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReturnOK.Name = "toolStripButtonReturnOK";
            this.toolStripButtonReturnOK.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonReturnOK.Text = "All specimen were returned";
            this.toolStripButtonReturnOK.ToolTipText = "All specimen were returned";
            this.toolStripButtonReturnOK.Click += new System.EventHandler(this.toolStripButtonReturnOK_Click);
            // 
            // toolStripButtonReturnPreview
            // 
            this.toolStripButtonReturnPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonReturnPreview.Image = global::DiversityCollection.Resource.List;
            this.toolStripButtonReturnPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReturnPreview.Name = "toolStripButtonReturnPreview";
            this.toolStripButtonReturnPreview.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonReturnPreview.Text = "Create preview";
            this.toolStripButtonReturnPreview.Click += new System.EventHandler(this.toolStripButtonReturnPreview_Click);
            // 
            // toolStripButtonReturnPrint
            // 
            this.toolStripButtonReturnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonReturnPrint.Image = global::DiversityCollection.Resource.Print;
            this.toolStripButtonReturnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReturnPrint.Name = "toolStripButtonReturnPrint";
            this.toolStripButtonReturnPrint.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonReturnPrint.Text = "Print form";
            this.toolStripButtonReturnPrint.Click += new System.EventHandler(this.toolStripButtonReturnPrint_Click);
            // 
            // toolStripButtonReturnSave
            // 
            this.toolStripButtonReturnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonReturnSave.Image = global::DiversityCollection.Resource.Save;
            this.toolStripButtonReturnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReturnSave.Name = "toolStripButtonReturnSave";
            this.toolStripButtonReturnSave.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonReturnSave.Text = "Save form";
            this.toolStripButtonReturnSave.Click += new System.EventHandler(this.toolStripButtonReturnSave_Click);
            // 
            // tabPageHistory
            // 
            this.tabPageHistory.Controls.Add(this.splitContainerLoanHistory);
            this.tabPageHistory.ImageKey = "History.ico";
            this.tabPageHistory.Location = new System.Drawing.Point(4, 23);
            this.tabPageHistory.Name = "tabPageHistory";
            this.tabPageHistory.Size = new System.Drawing.Size(656, 449);
            this.tabPageHistory.TabIndex = 5;
            this.tabPageHistory.Text = "History";
            this.tabPageHistory.UseVisualStyleBackColor = true;
            // 
            // splitContainerLoanHistory
            // 
            this.splitContainerLoanHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLoanHistory.Location = new System.Drawing.Point(0, 0);
            this.splitContainerLoanHistory.Name = "splitContainerLoanHistory";
            // 
            // splitContainerLoanHistory.Panel1
            // 
            this.splitContainerLoanHistory.Panel1.Controls.Add(this.listBoxLoanHistory);
            this.splitContainerLoanHistory.Panel1.Controls.Add(this.toolStripLoanHistory);
            // 
            // splitContainerLoanHistory.Panel2
            // 
            this.splitContainerLoanHistory.Panel2.Controls.Add(this.splitContainerLoanHistoryData);
            this.splitContainerLoanHistory.Size = new System.Drawing.Size(656, 449);
            this.splitContainerLoanHistory.SplitterDistance = 113;
            this.splitContainerLoanHistory.TabIndex = 1;
            // 
            // listBoxLoanHistory
            // 
            this.listBoxLoanHistory.DataSource = this.loanHistoryBindingSource;
            this.listBoxLoanHistory.DisplayMember = "Date";
            this.listBoxLoanHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxLoanHistory.FormattingEnabled = true;
            this.listBoxLoanHistory.IntegralHeight = false;
            this.listBoxLoanHistory.Location = new System.Drawing.Point(0, 0);
            this.listBoxLoanHistory.Name = "listBoxLoanHistory";
            this.listBoxLoanHistory.Size = new System.Drawing.Size(113, 424);
            this.listBoxLoanHistory.TabIndex = 0;
            this.listBoxLoanHistory.SelectedIndexChanged += new System.EventHandler(this.listBoxLoanHistory_SelectedIndexChanged);
            // 
            // loanHistoryBindingSource
            // 
            this.loanHistoryBindingSource.DataMember = "LoanHistory";
            this.loanHistoryBindingSource.DataSource = this.dataSetLoan;
            // 
            // toolStripLoanHistory
            // 
            this.toolStripLoanHistory.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripLoanHistory.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripLoanHistory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonLoanHistoryNew,
            this.toolStripButtonLoanHistoryDelete});
            this.toolStripLoanHistory.Location = new System.Drawing.Point(0, 424);
            this.toolStripLoanHistory.Name = "toolStripLoanHistory";
            this.toolStripLoanHistory.Size = new System.Drawing.Size(113, 25);
            this.toolStripLoanHistory.TabIndex = 1;
            this.toolStripLoanHistory.Text = "toolStrip1";
            // 
            // toolStripButtonLoanHistoryNew
            // 
            this.toolStripButtonLoanHistoryNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLoanHistoryNew.Image = global::DiversityCollection.Resource.New1;
            this.toolStripButtonLoanHistoryNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLoanHistoryNew.Name = "toolStripButtonLoanHistoryNew";
            this.toolStripButtonLoanHistoryNew.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonLoanHistoryNew.Text = "Add a new event to this loan";
            this.toolStripButtonLoanHistoryNew.Click += new System.EventHandler(this.toolStripButtonLoanHistoryNew_Click);
            // 
            // toolStripButtonLoanHistoryDelete
            // 
            this.toolStripButtonLoanHistoryDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLoanHistoryDelete.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonLoanHistoryDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLoanHistoryDelete.Name = "toolStripButtonLoanHistoryDelete";
            this.toolStripButtonLoanHistoryDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonLoanHistoryDelete.Text = "Delete the selected entry from the history";
            this.toolStripButtonLoanHistoryDelete.Click += new System.EventHandler(this.toolStripButtonLoanHistoryDelete_Click);
            // 
            // splitContainerLoanHistoryData
            // 
            this.splitContainerLoanHistoryData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLoanHistoryData.Location = new System.Drawing.Point(0, 0);
            this.splitContainerLoanHistoryData.Name = "splitContainerLoanHistoryData";
            this.splitContainerLoanHistoryData.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerLoanHistoryData.Panel1
            // 
            this.splitContainerLoanHistoryData.Panel1.Controls.Add(this.splitContainerLoanHistoryDocuments);
            // 
            // splitContainerLoanHistoryData.Panel2
            // 
            this.splitContainerLoanHistoryData.Panel2.Controls.Add(this.textBoxLoanHistoryInternalNotes);
            this.splitContainerLoanHistoryData.Panel2.Controls.Add(this.tableLayoutPanelLoanHistoryButtons);
            this.splitContainerLoanHistoryData.Size = new System.Drawing.Size(539, 449);
            this.splitContainerLoanHistoryData.SplitterDistance = 224;
            this.splitContainerLoanHistoryData.TabIndex = 1;
            // 
            // splitContainerLoanHistoryDocuments
            // 
            this.splitContainerLoanHistoryDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLoanHistoryDocuments.Location = new System.Drawing.Point(0, 0);
            this.splitContainerLoanHistoryDocuments.Name = "splitContainerLoanHistoryDocuments";
            // 
            // splitContainerLoanHistoryDocuments.Panel1
            // 
            this.splitContainerLoanHistoryDocuments.Panel1.Controls.Add(this.panelHistoryWebbrowser);
            // 
            // splitContainerLoanHistoryDocuments.Panel2
            // 
            this.splitContainerLoanHistoryDocuments.Panel2.Controls.Add(this.panelHistoryImage);
            this.splitContainerLoanHistoryDocuments.Panel2.Controls.Add(this.toolStripHistoryImage);
            this.splitContainerLoanHistoryDocuments.Size = new System.Drawing.Size(539, 224);
            this.splitContainerLoanHistoryDocuments.SplitterDistance = 269;
            this.splitContainerLoanHistoryDocuments.TabIndex = 0;
            // 
            // panelHistoryWebbrowser
            // 
            this.panelHistoryWebbrowser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelHistoryWebbrowser.Controls.Add(this.webBrowserLoanHistory);
            this.panelHistoryWebbrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHistoryWebbrowser.Location = new System.Drawing.Point(0, 0);
            this.panelHistoryWebbrowser.Name = "panelHistoryWebbrowser";
            this.panelHistoryWebbrowser.Size = new System.Drawing.Size(269, 224);
            this.panelHistoryWebbrowser.TabIndex = 1;
            // 
            // webBrowserLoanHistory
            // 
            this.webBrowserLoanHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserLoanHistory.Location = new System.Drawing.Point(0, 0);
            this.webBrowserLoanHistory.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserLoanHistory.Name = "webBrowserLoanHistory";
            this.webBrowserLoanHistory.Size = new System.Drawing.Size(265, 220);
            this.webBrowserLoanHistory.TabIndex = 0;
            // 
            // panelHistoryImage
            // 
            this.panelHistoryImage.AutoScroll = true;
            this.panelHistoryImage.Controls.Add(this.pictureBoxLoanHistory);
            this.panelHistoryImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHistoryImage.Location = new System.Drawing.Point(0, 0);
            this.panelHistoryImage.Name = "panelHistoryImage";
            this.panelHistoryImage.Size = new System.Drawing.Size(266, 199);
            this.panelHistoryImage.TabIndex = 1;
            // 
            // pictureBoxLoanHistory
            // 
            this.pictureBoxLoanHistory.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxLoanHistory.Name = "pictureBoxLoanHistory";
            this.pictureBoxLoanHistory.Size = new System.Drawing.Size(2000, 2000);
            this.pictureBoxLoanHistory.TabIndex = 0;
            this.pictureBoxLoanHistory.TabStop = false;
            // 
            // toolStripHistoryImage
            // 
            this.toolStripHistoryImage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripHistoryImage.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripHistoryImage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonHistoryZoomAdapt,
            this.toolStripButtonHistoryZoom100Percent,
            this.toolStripSeparatorHistory,
            this.toolStripButtonHistoryRemove});
            this.toolStripHistoryImage.Location = new System.Drawing.Point(0, 199);
            this.toolStripHistoryImage.Name = "toolStripHistoryImage";
            this.toolStripHistoryImage.Size = new System.Drawing.Size(266, 25);
            this.toolStripHistoryImage.TabIndex = 2;
            this.toolStripHistoryImage.Text = "toolStrip1";
            // 
            // toolStripButtonHistoryZoomAdapt
            // 
            this.toolStripButtonHistoryZoomAdapt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHistoryZoomAdapt.Image = global::DiversityCollection.Resource.ZoomAdapt;
            this.toolStripButtonHistoryZoomAdapt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHistoryZoomAdapt.Name = "toolStripButtonHistoryZoomAdapt";
            this.toolStripButtonHistoryZoomAdapt.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonHistoryZoomAdapt.Text = "Adapt size of image to available space";
            this.toolStripButtonHistoryZoomAdapt.Click += new System.EventHandler(this.toolStripButtonHistoryZoomAdapt_Click);
            // 
            // toolStripButtonHistoryZoom100Percent
            // 
            this.toolStripButtonHistoryZoom100Percent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHistoryZoom100Percent.Image = global::DiversityCollection.Resource.Zoom100;
            this.toolStripButtonHistoryZoom100Percent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHistoryZoom100Percent.Name = "toolStripButtonHistoryZoom100Percent";
            this.toolStripButtonHistoryZoom100Percent.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonHistoryZoom100Percent.Text = "toolStripButton2";
            this.toolStripButtonHistoryZoom100Percent.Click += new System.EventHandler(this.toolStripButtonHistoryZoom100Percent_Click);
            // 
            // toolStripSeparatorHistory
            // 
            this.toolStripSeparatorHistory.Name = "toolStripSeparatorHistory";
            this.toolStripSeparatorHistory.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonHistoryRemove
            // 
            this.toolStripButtonHistoryRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHistoryRemove.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonHistoryRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHistoryRemove.Name = "toolStripButtonHistoryRemove";
            this.toolStripButtonHistoryRemove.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonHistoryRemove.Text = "toolStripButton1";
            this.toolStripButtonHistoryRemove.Click += new System.EventHandler(this.toolStripButtonHistoryRemove_Click);
            // 
            // textBoxLoanHistoryInternalNotes
            // 
            this.textBoxLoanHistoryInternalNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.loanHistoryBindingSource, "InternalNotes", true));
            this.textBoxLoanHistoryInternalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLoanHistoryInternalNotes.Location = new System.Drawing.Point(0, 46);
            this.textBoxLoanHistoryInternalNotes.Multiline = true;
            this.textBoxLoanHistoryInternalNotes.Name = "textBoxLoanHistoryInternalNotes";
            this.textBoxLoanHistoryInternalNotes.Size = new System.Drawing.Size(539, 175);
            this.textBoxLoanHistoryInternalNotes.TabIndex = 1;
            // 
            // tableLayoutPanelLoanHistoryButtons
            // 
            this.tableLayoutPanelLoanHistoryButtons.ColumnCount = 3;
            this.tableLayoutPanelLoanHistoryButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelLoanHistoryButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLoanHistoryButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelLoanHistoryButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelLoanHistoryButtons.Controls.Add(this.buttonLoanHistoryInsertDocument, 1, 0);
            this.tableLayoutPanelLoanHistoryButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelLoanHistoryButtons.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelLoanHistoryButtons.Name = "tableLayoutPanelLoanHistoryButtons";
            this.tableLayoutPanelLoanHistoryButtons.RowCount = 1;
            this.tableLayoutPanelLoanHistoryButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLoanHistoryButtons.Size = new System.Drawing.Size(539, 46);
            this.tableLayoutPanelLoanHistoryButtons.TabIndex = 4;
            // 
            // buttonLoanHistoryInsertDocument
            // 
            this.buttonLoanHistoryInsertDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLoanHistoryInsertDocument.Location = new System.Drawing.Point(194, 10);
            this.buttonLoanHistoryInsertDocument.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.buttonLoanHistoryInsertDocument.Name = "buttonLoanHistoryInsertDocument";
            this.buttonLoanHistoryInsertDocument.Size = new System.Drawing.Size(150, 26);
            this.buttonLoanHistoryInsertDocument.TabIndex = 2;
            this.buttonLoanHistoryInsertDocument.Text = "Add image from document";
            this.toolTip.SetToolTip(this.buttonLoanHistoryInsertDocument, "Copy an image of a document (activate image and press alt-print), then click on t" +
        "his button");
            this.buttonLoanHistoryInsertDocument.UseVisualStyleBackColor = true;
            this.buttonLoanHistoryInsertDocument.Click += new System.EventHandler(this.buttonLoanHistoryInsertDocument_Click);
            // 
            // imageListTabpages
            // 
            this.imageListTabpages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTabpages.ImageStream")));
            this.imageListTabpages.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTabpages.Images.SetKeyName(0, "History.ico");
            this.imageListTabpages.Images.SetKeyName(1, "LoanReminder.ico");
            this.imageListTabpages.Images.SetKeyName(2, "LoanOut.ico");
            this.imageListTabpages.Images.SetKeyName(3, "LoanPartIn.ico");
            this.imageListTabpages.Images.SetKeyName(4, "LoanQuery.ico");
            this.imageListTabpages.Images.SetKeyName(5, "LoanIn.ico");
            this.imageListTabpages.Images.SetKeyName(6, "Note.ICO");
            // 
            // userControlSpecimenList
            // 
            this.userControlSpecimenList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlSpecimenList.Location = new System.Drawing.Point(0, 0);
            this.userControlSpecimenList.Name = "userControlSpecimenList";
            this.userControlSpecimenList.Size = new System.Drawing.Size(137, 666);
            this.userControlSpecimenList.TabIndex = 0;
            // 
            // imageListSpecimenList
            // 
            this.imageListSpecimenList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSpecimenList.ImageStream")));
            this.imageListSpecimenList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSpecimenList.Images.SetKeyName(0, "List.ico");
            this.imageListSpecimenList.Images.SetKeyName(1, "ListNot.ico");
            // 
            // timerSending
            // 
            this.timerSending.Interval = 200;
            this.timerSending.Tick += new System.EventHandler(this.timerScanning_Tick);
            // 
            // timerReturn
            // 
            this.timerReturn.Tick += new System.EventHandler(this.timerReturn_Tick);
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 690);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(1016, 27);
            this.userControlDialogPanel.TabIndex = 7;
            this.userControlDialogPanel.Visible = false;
            // 
            // loanTableAdapter
            // 
            this.loanTableAdapter.ClearBeforeFill = true;
            // 
            // loanHistoryTableAdapter
            // 
            this.loanHistoryTableAdapter.ClearBeforeFill = true;
            // 
            // FormLoan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 717);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.userControlDialogPanel);
            this.helpProvider.SetHelpKeyword(this, "Loan");
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormLoan";
            this.helpProvider.SetShowHelp(this, true);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Loan";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLoan_FormClosing);
            this.panelHeader.ResumeLayout(false);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerData.Panel1.ResumeLayout(false);
            this.splitContainerData.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerData)).EndInit();
            this.splitContainerData.ResumeLayout(false);
            this.groupBoxData.ResumeLayout(false);
            this.splitContainerLoan.Panel1.ResumeLayout(false);
            this.splitContainerLoan.Panel1.PerformLayout();
            this.splitContainerLoan.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLoan)).EndInit();
            this.splitContainerLoan.ResumeLayout(false);
            this.toolStripLoan.ResumeLayout(false);
            this.toolStripLoan.PerformLayout();
            this.tabControlLoan.ResumeLayout(false);
            this.tabPageDataEntry.ResumeLayout(false);
            this.tableLayoutPanelCollection.ResumeLayout(false);
            this.tableLayoutPanelCollection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loanBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetLoan)).EndInit();
            this.tabPageSending.ResumeLayout(false);
            this.tableLayoutPanelSending.ResumeLayout(false);
            this.tableLayoutPanelSending.PerformLayout();
            this.panelWebbrowserSending.ResumeLayout(false);
            this.toolStripSending.ResumeLayout(false);
            this.toolStripSending.PerformLayout();
            this.tabPageConfirmation.ResumeLayout(false);
            this.tableLayoutPanelReminder.ResumeLayout(false);
            this.tableLayoutPanelReminder.PerformLayout();
            this.panelReminderWebbrowser.ResumeLayout(false);
            this.toolStripConfirmation.ResumeLayout(false);
            this.toolStripConfirmation.PerformLayout();
            this.tabPageReminder.ResumeLayout(false);
            this.tableLayoutPanelAdmonition.ResumeLayout(false);
            this.tableLayoutPanelAdmonition.PerformLayout();
            this.panelAdmonitionWebbrowser.ResumeLayout(false);
            this.toolStripReminder.ResumeLayout(false);
            this.toolStripReminder.PerformLayout();
            this.tabPagePartialReturn.ResumeLayout(false);
            this.tableLayoutPanelPartialReturn.ResumeLayout(false);
            this.tableLayoutPanelPartialReturn.PerformLayout();
            this.panelPartialReturnWebbrowser.ResumeLayout(false);
            this.toolStripPartialReturn.ResumeLayout(false);
            this.toolStripPartialReturn.PerformLayout();
            this.tabPageReturn.ResumeLayout(false);
            this.tableLayoutPanelReturn.ResumeLayout(false);
            this.tableLayoutPanelReturn.PerformLayout();
            this.panelReturnWebbrowser.ResumeLayout(false);
            this.toolStripReturn.ResumeLayout(false);
            this.toolStripReturn.PerformLayout();
            this.tabPageHistory.ResumeLayout(false);
            this.splitContainerLoanHistory.Panel1.ResumeLayout(false);
            this.splitContainerLoanHistory.Panel1.PerformLayout();
            this.splitContainerLoanHistory.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLoanHistory)).EndInit();
            this.splitContainerLoanHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.loanHistoryBindingSource)).EndInit();
            this.toolStripLoanHistory.ResumeLayout(false);
            this.toolStripLoanHistory.PerformLayout();
            this.splitContainerLoanHistoryData.Panel1.ResumeLayout(false);
            this.splitContainerLoanHistoryData.Panel2.ResumeLayout(false);
            this.splitContainerLoanHistoryData.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLoanHistoryData)).EndInit();
            this.splitContainerLoanHistoryData.ResumeLayout(false);
            this.splitContainerLoanHistoryDocuments.Panel1.ResumeLayout(false);
            this.splitContainerLoanHistoryDocuments.Panel2.ResumeLayout(false);
            this.splitContainerLoanHistoryDocuments.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLoanHistoryDocuments)).EndInit();
            this.splitContainerLoanHistoryDocuments.ResumeLayout(false);
            this.panelHistoryWebbrowser.ResumeLayout(false);
            this.panelHistoryImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoanHistory)).EndInit();
            this.toolStripHistoryImage.ResumeLayout(false);
            this.toolStripHistoryImage.PerformLayout();
            this.tableLayoutPanelLoanHistoryButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private DiversityWorkbench.UserControlQueryList userControlQueryList;
        private DiversityWorkbench.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.ToolTip toolTip;
        private Datasets.DataSetLoan dataSetLoan;
        private System.Windows.Forms.SplitContainer splitContainerData;
        private System.Windows.Forms.GroupBox groupBoxData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCollection;
        private System.Windows.Forms.TreeView treeViewLoan;
        private System.Windows.Forms.ToolStrip toolStripLoan;
        private System.Windows.Forms.ToolStripButton toolStripButtonNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.Label labelLoanTitle;
        private System.Windows.Forms.TextBox textBoxLoanTitle;
        private System.Windows.Forms.Label labelBegin;
        private System.Windows.Forms.TextBox textBoxLoanBegin;
        private System.Windows.Forms.Label labelLoanNumber;
        private System.Windows.Forms.TextBox textBoxLoanNumber;
        private System.Windows.Forms.Label labelLoanPartnerName;
        private DiversityWorkbench.UserControlModuleRelatedEntry userControlModuleRelatedEntryLoanPartner;
        private System.Windows.Forms.ImageList imageListSpecimenList;
        private System.Windows.Forms.Label labelLoanEnd;
        private System.Windows.Forms.DateTimePicker dateTimePickerLoanBegin;
        private System.Windows.Forms.TextBox textBoxLoanEnd;
        private System.Windows.Forms.DateTimePicker dateTimePickerLoanEnd;
        private System.Windows.Forms.Label labelLoanComment;
        private System.Windows.Forms.TextBox textBoxLoanComment;
        private System.Windows.Forms.Label labelInternalNotes;
        private System.Windows.Forms.TextBox textBoxInternalNotes;
        private System.Windows.Forms.Label labelResponsible;
        private DiversityWorkbench.UserControlModuleRelatedEntry userControlModuleRelatedEntryResponsible;
        private UserControlSpecimenList userControlSpecimenList;
        private System.Windows.Forms.ToolStripButton toolStripButtonSpecimenList;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.SplitContainer splitContainerLoan;
        private System.Windows.Forms.TabControl tabControlLoan;
        private System.Windows.Forms.TabPage tabPageDataEntry;
        private System.Windows.Forms.TabPage tabPageSending;
        private System.Windows.Forms.TabPage tabPageReminder;
        private System.Windows.Forms.TabPage tabPageReturn;
        private System.Windows.Forms.TabPage tabPagePartialReturn;
        private System.Windows.Forms.TabPage tabPageHistory;
        private System.Windows.Forms.ImageList imageListTabpages;
        private System.Windows.Forms.TabPage tabPageConfirmation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSending;
        private System.Windows.Forms.Panel panelWebbrowserSending;
        private System.Windows.Forms.WebBrowser webBrowserSending;
        private System.Windows.Forms.Label labelSendingSchema;
        private System.Windows.Forms.TextBox textBoxSendingSchema;
        private System.Windows.Forms.ToolStrip toolStripSending;
        private System.Windows.Forms.Button buttonSendingScanner;
        private System.Windows.Forms.TextBox textBoxSendingAccessionNumber;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpenSendingSchemaFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorSending1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSendingPreview;
        private System.Windows.Forms.ToolStripButton toolStripButtonSendingPrint;
        private System.Windows.Forms.ToolStripButton toolStripButtonSendingSave;
        private System.Windows.Forms.Timer timerSending;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPartialReturn;
        private System.Windows.Forms.Panel panelPartialReturnWebbrowser;
        private System.Windows.Forms.WebBrowser webBrowserPartialReturn;
        private System.Windows.Forms.Label labelPartialReturnSchema;
        private System.Windows.Forms.TextBox textBoxPartialReturnSchema;
        private System.Windows.Forms.ToolStrip toolStripPartialReturn;
        private System.Windows.Forms.ToolStripButton toolStripButtonPartialReturnOpenSchema;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorPartialReturn1;
        private System.Windows.Forms.ToolStripButton toolStripButtonPartialReturnPreview;
        private System.Windows.Forms.ToolStripButton toolStripButtonPartialReturnPrint;
        private System.Windows.Forms.ToolStripButton toolStripButtonPartialReturnSave;
        private System.Windows.Forms.Button buttonPartialReturnScanner;
        private System.Windows.Forms.TextBox textBoxPartialReturnAccessionNumber;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelReminder;
        private System.Windows.Forms.Panel panelReminderWebbrowser;
        private System.Windows.Forms.WebBrowser webBrowserConfirmation;
        private System.Windows.Forms.Label labelConfirmationSchema;
        private System.Windows.Forms.TextBox textBoxConfirmationSchema;
        private System.Windows.Forms.ToolStrip toolStripConfirmation;
        private System.Windows.Forms.ToolStripButton toolStripButtonConfirmationOpenSchema;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorConfirmation;
        private System.Windows.Forms.ToolStripButton toolStripButtonConfirmationPreview;
        private System.Windows.Forms.ToolStripButton toolStripButtonConfirmationPrint;
        private System.Windows.Forms.ToolStripButton toolStripButtonConfirmationSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelReturn;
        private System.Windows.Forms.Panel panelReturnWebbrowser;
        private System.Windows.Forms.WebBrowser webBrowserReturn;
        private System.Windows.Forms.Label labelReturnSchema;
        private System.Windows.Forms.TextBox textBoxReturnSchema;
        private System.Windows.Forms.ToolStrip toolStripReturn;
        private System.Windows.Forms.ToolStripButton toolStripButtonReturnOpenSchema;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorReturn1;
        private System.Windows.Forms.ToolStripButton toolStripButtonReturnPreview;
        private System.Windows.Forms.ToolStripButton toolStripButtonReturnPrint;
        private System.Windows.Forms.ToolStripButton toolStripButtonReturnSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAdmonition;
        private System.Windows.Forms.Panel panelAdmonitionWebbrowser;
        private System.Windows.Forms.WebBrowser webBrowserReminder;
        private System.Windows.Forms.Label labelReminderSchema;
        private System.Windows.Forms.TextBox textBoxReminderSchema;
        private System.Windows.Forms.ToolStrip toolStripReminder;
        private System.Windows.Forms.ToolStripButton toolStripButtonReminderOpenSchema;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorReminder;
        private System.Windows.Forms.ToolStripButton toolStripButtonReminderPreview;
        private System.Windows.Forms.ToolStripButton toolStripButtonReminderPrint;
        private System.Windows.Forms.ToolStripButton toolStripButtonReminderSave;
        private System.Windows.Forms.ToolStripButton toolStripButtonReturnOK;
        private System.Windows.Forms.Timer timerReturn;
        private System.Windows.Forms.Label labelPartialReturnDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerPartialReturn;
        private System.Windows.Forms.Label labelPartialReturnReturnedSpecimen;
        private System.Windows.Forms.TextBox textBoxPartialReturnReturnedSpecimen;
        private System.Windows.Forms.SplitContainer splitContainerLoanHistory;
        private System.Windows.Forms.ListBox listBoxLoanHistory;
        private System.Windows.Forms.ToolStrip toolStripLoanHistory;
        private System.Windows.Forms.ToolStripButton toolStripButtonLoanHistoryNew;
        private System.Windows.Forms.SplitContainer splitContainerLoanHistoryDocuments;
        private System.Windows.Forms.WebBrowser webBrowserLoanHistory;
        private System.Windows.Forms.SplitContainer splitContainerLoanHistoryData;
        private System.Windows.Forms.PictureBox pictureBoxLoanHistory;
        private System.Windows.Forms.TextBox textBoxLoanHistoryInternalNotes;
        private System.Windows.Forms.Label labelInvestigator;
        private System.Windows.Forms.TextBox textBoxInvestigator;
        private System.Windows.Forms.ComboBox comboBoxLoanCommentAdd;
        private System.Windows.Forms.Button buttonLoanCommentInsertSelected;
        private System.Windows.Forms.Button buttonLoanHistoryInsertDocument;
        private System.Windows.Forms.ToolStripButton toolStripButtonLoanHistoryDelete;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLoanHistoryButtons;
        private System.Windows.Forms.OpenFileDialog openFileDialogSendingSchema;
        private System.Windows.Forms.Label labelLoanPartnerAddress;
        private System.Windows.Forms.Label labelInitialNumberOfSpecimen;
        private System.Windows.Forms.TextBox textBoxInitialNumberOfSpecimen;
        private System.Windows.Forms.BindingSource loanBindingSource;
        private DiversityCollection.Datasets.DataSetLoanTableAdapters.LoanTableAdapter loanTableAdapter;
        private System.Windows.Forms.BindingSource loanHistoryBindingSource;
        private DiversityCollection.Datasets.DataSetLoanTableAdapters.LoanHistoryTableAdapter loanHistoryTableAdapter;
        private System.Windows.Forms.ListBox listBoxPartialReturnSpecimen;
        private System.Windows.Forms.ComboBox comboBoxPartialReturn;
        private System.Windows.Forms.Panel panelHistoryWebbrowser;
        private System.Windows.Forms.ToolStrip toolStripHistoryImage;
        private System.Windows.Forms.Panel panelHistoryImage;
        private System.Windows.Forms.ToolStripButton toolStripButtonHistoryZoomAdapt;
        private System.Windows.Forms.ToolStripButton toolStripButtonHistoryZoom100Percent;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorHistory;
        private System.Windows.Forms.ToolStripButton toolStripButtonHistoryRemove;
    }
}