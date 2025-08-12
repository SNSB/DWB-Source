namespace DiversityCollection.Forms
{
    partial class FormTransaction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTransaction));
            splitContainerMain = new System.Windows.Forms.SplitContainer();
            userControlQueryList = new DiversityWorkbench.UserControls.UserControlQueryList();
            splitContainerData = new System.Windows.Forms.SplitContainer();
            groupBoxData = new System.Windows.Forms.GroupBox();
            splitContainerTransaction = new System.Windows.Forms.SplitContainer();
            treeViewTransaction = new System.Windows.Forms.TreeView();
            toolStripTransaction = new System.Windows.Forms.ToolStrip();
            toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
            toolStripButtonCopy = new System.Windows.Forms.ToolStripButton();
            toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            toolStripButtonSpecimenList = new System.Windows.Forms.ToolStripButton();
            toolStripButtonSetParent = new System.Windows.Forms.ToolStripButton();
            toolStripButtonShowTasks = new System.Windows.Forms.ToolStripButton();
            tabControlTransaction = new System.Windows.Forms.TabControl();
            tabPageDataEntry = new System.Windows.Forms.TabPage();
            tableLayoutPanelCollection = new System.Windows.Forms.TableLayoutPanel();
            labelTransactionTitle = new System.Windows.Forms.Label();
            textBoxTransactionTitle = new System.Windows.Forms.TextBox();
            transactionBindingSource = new System.Windows.Forms.BindingSource(components);
            dataSetTransaction = new DiversityCollection.Datasets.DataSetTransaction();
            labelBeginDate = new System.Windows.Forms.Label();
            dateTimePickerBeginDate = new System.Windows.Forms.DateTimePicker();
            labelTransactionComment = new System.Windows.Forms.Label();
            textBoxTransactionComment = new System.Windows.Forms.TextBox();
            labelInternalNotes = new System.Windows.Forms.Label();
            textBoxInternalNotes = new System.Windows.Forms.TextBox();
            labelResponsible = new System.Windows.Forms.Label();
            userControlModuleRelatedEntryResponsible = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            comboBoxTransactionCommentAdd = new System.Windows.Forms.ComboBox();
            buttonTransactionCommentInsertSelected = new System.Windows.Forms.Button();
            labelTransactionType = new System.Windows.Forms.Label();
            comboBoxTransactionType = new System.Windows.Forms.ComboBox();
            labelMaterialCategory = new System.Windows.Forms.Label();
            comboBoxMaterialCategory = new System.Windows.Forms.ComboBox();
            labelAgreedEndDate = new System.Windows.Forms.Label();
            dateTimePickerAgreedEndDate = new System.Windows.Forms.DateTimePicker();
            labelAdministratingCollection = new System.Windows.Forms.Label();
            comboBoxAdministratingCollection = new System.Windows.Forms.ComboBox();
            labelActualEndDate = new System.Windows.Forms.Label();
            dateTimePickerActualEndDate = new System.Windows.Forms.DateTimePicker();
            textBoxInvestigator = new System.Windows.Forms.TextBox();
            labelInvestigator = new System.Windows.Forms.Label();
            labelReportingCategory = new System.Windows.Forms.Label();
            comboBoxReportingCategory = new System.Windows.Forms.ComboBox();
            labelMaterialDescription = new System.Windows.Forms.Label();
            textBoxMaterialDescription = new System.Windows.Forms.TextBox();
            labelMaterialCollectors = new System.Windows.Forms.Label();
            textBoxMaterialCollectors = new System.Windows.Forms.TextBox();
            comboBoxAgreedEndDateAddMonths = new System.Windows.Forms.ComboBox();
            buttonBeginDateEdit = new System.Windows.Forms.Button();
            buttonActualEndDateEdit = new System.Windows.Forms.Button();
            buttonAgreedEndDateEdit = new System.Windows.Forms.Button();
            comboBoxActualEndDateAddMonths = new System.Windows.Forms.ComboBox();
            labelCommentPhrases = new System.Windows.Forms.Label();
            groupBoxFrom = new System.Windows.Forms.GroupBox();
            tableLayoutPanelFrom = new System.Windows.Forms.TableLayoutPanel();
            comboBoxFromCollection = new System.Windows.Forms.ComboBox();
            textBoxFromTransactionNumber = new System.Windows.Forms.TextBox();
            labelFromTransactionNumber = new System.Windows.Forms.Label();
            labelFromCollection = new System.Windows.Forms.Label();
            userControlHierarchySelectorFromCollection = new DiversityWorkbench.UserControls.UserControlHierarchySelector();
            userControlModuleRelatedEntryFromPartner = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            groupBoxTo = new System.Windows.Forms.GroupBox();
            tableLayoutPanelTo = new System.Windows.Forms.TableLayoutPanel();
            userControlModuleRelatedEntryToPartner = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            textBoxToTransactionNumber = new System.Windows.Forms.TextBox();
            labelToTransactionNumber = new System.Windows.Forms.Label();
            comboBoxToCollection = new System.Windows.Forms.ComboBox();
            userControlHierarchySelectorToCollection = new DiversityWorkbench.UserControls.UserControlHierarchySelector();
            labelToCollection = new System.Windows.Forms.Label();
            labelToRecipient = new System.Windows.Forms.Label();
            comboBoxToRecipient = new System.Windows.Forms.ComboBox();
            comboBoxMaterialSource = new System.Windows.Forms.ComboBox();
            labelMaterialSource = new System.Windows.Forms.Label();
            textBoxNumberOfUnits = new System.Windows.Forms.TextBox();
            labelNumberOfUnits = new System.Windows.Forms.Label();
            labelDateSupplement = new System.Windows.Forms.Label();
            textBoxDateSupplement = new System.Windows.Forms.TextBox();
            labelToTransactionInventoryNumber = new System.Windows.Forms.Label();
            textBoxToTransactionInventoryNumber = new System.Windows.Forms.TextBox();
            tabPageSending = new System.Windows.Forms.TabPage();
            splitContainerSending = new System.Windows.Forms.SplitContainer();
            tableLayoutPanelSending = new System.Windows.Forms.TableLayoutPanel();
            labelSendingSchema = new System.Windows.Forms.Label();
            textBoxSendingSchema = new System.Windows.Forms.TextBox();
            toolStripSending = new System.Windows.Forms.ToolStrip();
            toolStripButtonOpenSendingSchemaFile = new System.Windows.Forms.ToolStripButton();
            toolStripDropDownButtonSendingGitHub = new System.Windows.Forms.ToolStripDropDownButton();
            SendingSnsbToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparatorSending1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripButtonSendingShowList = new System.Windows.Forms.ToolStripButton();
            toolStripButtonSendingPreview = new System.Windows.Forms.ToolStripButton();
            toolStripButtonSendingPreviewAllOnLoan = new System.Windows.Forms.ToolStripButton();
            toolStripButtonSendingXslEditor = new System.Windows.Forms.ToolStripButton();
            toolStripButtonSendingPrint = new System.Windows.Forms.ToolStripButton();
            toolStripButtonSendingSave = new System.Windows.Forms.ToolStripButton();
            toolStripButtonSendingScanner = new System.Windows.Forms.ToolStripButton();
            checkBoxSendingRestrictToCollection = new System.Windows.Forms.CheckBox();
            checkBoxSendingRestrictToMaterial = new System.Windows.Forms.CheckBox();
            pictureBoxSendingRestrictToMaterial = new System.Windows.Forms.PictureBox();
            pictureBoxSendingRestrictToCollection = new System.Windows.Forms.PictureBox();
            checkBoxSendingOrderByTaxa = new System.Windows.Forms.CheckBox();
            checkBoxSendingSingleLines = new System.Windows.Forms.CheckBox();
            checkBoxSendingIncludeType = new System.Windows.Forms.CheckBox();
            labelSendingRestrictToTaxonomicGroup = new System.Windows.Forms.Label();
            comboBoxSendingRestrictToTaxonomicGroup = new System.Windows.Forms.ComboBox();
            splitContainerWebbrowserSending = new System.Windows.Forms.SplitContainer();
            panelWebbrowserSending = new System.Windows.Forms.Panel();
            webBrowserSending = new System.Windows.Forms.WebBrowser();
            tableLayoutPanelSendingList = new System.Windows.Forms.TableLayoutPanel();
            textBoxSendingAccessionNumber = new System.Windows.Forms.TextBox();
            comboBoxSendingAccessionNumber = new System.Windows.Forms.ComboBox();
            listBoxSendingSpecimen = new System.Windows.Forms.ListBox();
            collectionSpecimenPartOnLoanListBindingSource = new System.Windows.Forms.BindingSource(components);
            toolStripSendingList = new System.Windows.Forms.ToolStrip();
            toolStripButtonSendingListDelete = new System.Windows.Forms.ToolStripButton();
            toolStripButtonSendingListFind = new System.Windows.Forms.ToolStripButton();
            labelSendingSpecimenReturned = new System.Windows.Forms.Label();
            listBoxSendingSpecimenReturned = new System.Windows.Forms.ListBox();
            collectionSpecimenPartTransactionReturnListBindingSource = new System.Windows.Forms.BindingSource(components);
            labelSendingSpecimenForwarded = new System.Windows.Forms.Label();
            listBoxSendingSpecimenForwarded = new System.Windows.Forms.ListBox();
            collectionSpecimenPartForwardingListBindingSource = new System.Windows.Forms.BindingSource(components);
            tabPageForwarding = new System.Windows.Forms.TabPage();
            tableLayoutPanelForwarding = new System.Windows.Forms.TableLayoutPanel();
            labelForwardingSchema = new System.Windows.Forms.Label();
            textBoxForwardingSchema = new System.Windows.Forms.TextBox();
            toolStripForwarding = new System.Windows.Forms.ToolStrip();
            toolStripButtonForwardingOpenSchema = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            toolStripButtonForwardingShowLists = new System.Windows.Forms.ToolStripButton();
            toolStripButtonForwardingPreview = new System.Windows.Forms.ToolStripButton();
            toolStripButtonForwardingPrint = new System.Windows.Forms.ToolStripButton();
            toolStripButtonForwardingSave = new System.Windows.Forms.ToolStripButton();
            splitContainerForwarding = new System.Windows.Forms.SplitContainer();
            panel2 = new System.Windows.Forms.Panel();
            webBrowserForwarding = new System.Windows.Forms.WebBrowser();
            tableLayoutPanelForwardingLists = new System.Windows.Forms.TableLayoutPanel();
            toolStripForwardingSpecimenForwarded = new System.Windows.Forms.ToolStrip();
            toolStripLabelForwardingSpecimenForwarded = new System.Windows.Forms.ToolStripLabel();
            toolStripButtonForwardingForwarded = new System.Windows.Forms.ToolStripButton();
            toolStripButtonForwardingRemove = new System.Windows.Forms.ToolStripButton();
            labelForwardingSpecimenListOnLoan = new System.Windows.Forms.Label();
            listBoxForwardingSpecimenForwarded = new System.Windows.Forms.ListBox();
            listBoxForwardingSpecimenOnLoan = new System.Windows.Forms.ListBox();
            checkBoxForwardingSingleLines = new System.Windows.Forms.CheckBox();
            checkBoxForwardingOrderByTaxa = new System.Windows.Forms.CheckBox();
            checkBoxForwardingIncludeType = new System.Windows.Forms.CheckBox();
            tabPageConfirmation = new System.Windows.Forms.TabPage();
            tableLayoutPanelConfirmation = new System.Windows.Forms.TableLayoutPanel();
            labelConfirmationSchema = new System.Windows.Forms.Label();
            textBoxConfirmationSchema = new System.Windows.Forms.TextBox();
            toolStripConfirmation = new System.Windows.Forms.ToolStrip();
            toolStripButtonConfirmationOpenSchema = new System.Windows.Forms.ToolStripButton();
            toolStripSeparatorConfirmation = new System.Windows.Forms.ToolStripSeparator();
            toolStripButtonConfirmationShowList = new System.Windows.Forms.ToolStripButton();
            toolStripButtonConfirmationPreview = new System.Windows.Forms.ToolStripButton();
            toolStripButtonConfirmationPrint = new System.Windows.Forms.ToolStripButton();
            toolStripButtonConfirmationSave = new System.Windows.Forms.ToolStripButton();
            splitContainerConfirmation = new System.Windows.Forms.SplitContainer();
            panelReminderWebbrowser = new System.Windows.Forms.Panel();
            webBrowserConfirmation = new System.Windows.Forms.WebBrowser();
            listBoxConfirmationSpecimenList = new System.Windows.Forms.ListBox();
            labelConfirmationSpecimenList = new System.Windows.Forms.Label();
            tabPageReminder = new System.Windows.Forms.TabPage();
            tableLayoutPanelAdmonition = new System.Windows.Forms.TableLayoutPanel();
            labelReminderSchema = new System.Windows.Forms.Label();
            textBoxReminderSchema = new System.Windows.Forms.TextBox();
            toolStripReminder = new System.Windows.Forms.ToolStrip();
            toolStripButtonReminderOpenSchema = new System.Windows.Forms.ToolStripButton();
            toolStripSeparatorReminder = new System.Windows.Forms.ToolStripSeparator();
            toolStripButtonReminderShowList = new System.Windows.Forms.ToolStripButton();
            toolStripButtonReminderPreview = new System.Windows.Forms.ToolStripButton();
            toolStripButtonReminderPrint = new System.Windows.Forms.ToolStripButton();
            toolStripButtonReminderSave = new System.Windows.Forms.ToolStripButton();
            splitContainerReminder = new System.Windows.Forms.SplitContainer();
            panelAdmonitionWebbrowser = new System.Windows.Forms.Panel();
            webBrowserReminder = new System.Windows.Forms.WebBrowser();
            tableLayoutPanelReminderList = new System.Windows.Forms.TableLayoutPanel();
            labelReminderSpecimenOnLoan = new System.Windows.Forms.Label();
            listBoxReminderSpecimenOnLoan = new System.Windows.Forms.ListBox();
            labelReminderSpecimenReturned = new System.Windows.Forms.Label();
            listBoxReminderSpecimenReturned = new System.Windows.Forms.ListBox();
            tabPagePrinting = new System.Windows.Forms.TabPage();
            splitContainerPrinting = new System.Windows.Forms.SplitContainer();
            panelWebbrowserPrint = new System.Windows.Forms.Panel();
            webBrowserPrint = new System.Windows.Forms.WebBrowser();
            tableLayoutPanelPrinting = new System.Windows.Forms.TableLayoutPanel();
            pictureBoxIncludeSubCollections = new System.Windows.Forms.PictureBox();
            checkBoxPrintingIncludeSubcollections = new System.Windows.Forms.CheckBox();
            labelPrintingSchemaFile = new System.Windows.Forms.Label();
            textBoxPrintingSchemaFile = new System.Windows.Forms.TextBox();
            toolStripPrinting = new System.Windows.Forms.ToolStrip();
            toolStripButtonOpenSchemaFile = new System.Windows.Forms.ToolStripButton();
            toolStripSeparatorLabel = new System.Windows.Forms.ToolStripSeparator();
            toolStripButtonPrintingPreview = new System.Windows.Forms.ToolStripButton();
            toolStripButtonPrintingPrint = new System.Windows.Forms.ToolStripButton();
            toolStripButtonPrintingSave = new System.Windows.Forms.ToolStripButton();
            toolStripButtonScannerPrinting = new System.Windows.Forms.ToolStripButton();
            toolStripButtonPrintingShowList = new System.Windows.Forms.ToolStripButton();
            toolStripButtonPageSetup = new System.Windows.Forms.ToolStripButton();
            toolStripButtonNewSchemaFile = new System.Windows.Forms.ToolStripButton();
            checkBoxIncludeTypeInformation = new System.Windows.Forms.CheckBox();
            labelPrintingInclude = new System.Windows.Forms.Label();
            checkBoxPrintingIncludeChildTransactions = new System.Windows.Forms.CheckBox();
            pictureBoxIncludeSubTransactions = new System.Windows.Forms.PictureBox();
            labelPrintingConversion = new System.Windows.Forms.Label();
            comboBoxPrintingConversion = new System.Windows.Forms.ComboBox();
            radioButtonPrintingGroupedByTaxa = new System.Windows.Forms.RadioButton();
            radioButtonPrintingGroupedByNumber = new System.Windows.Forms.RadioButton();
            radioButtonPrintingSingleNumbers = new System.Windows.Forms.RadioButton();
            checkBoxPrintingOrderByMaterial = new System.Windows.Forms.CheckBox();
            tableLayoutPanelPrintingList = new System.Windows.Forms.TableLayoutPanel();
            comboBoxPrintingAccessionNumber = new System.Windows.Forms.ComboBox();
            toolStripSpecimenList = new System.Windows.Forms.ToolStrip();
            toolStripButtonSpecimenListDelete = new System.Windows.Forms.ToolStripButton();
            toolStripButtonSpecimenListSearch = new System.Windows.Forms.ToolStripButton();
            listBoxPrintingList = new System.Windows.Forms.ListBox();
            collectionSpecimenPartListBindingSource = new System.Windows.Forms.BindingSource(components);
            textBoxPrintingAccessionNumber = new System.Windows.Forms.TextBox();
            labelPrintingList = new System.Windows.Forms.Label();
            tabPageDocuments = new System.Windows.Forms.TabPage();
            splitContainerDocuments = new System.Windows.Forms.SplitContainer();
            listBoxTransactionDocuments = new System.Windows.Forms.ListBox();
            transactionDocumentBindingSource = new System.Windows.Forms.BindingSource(components);
            toolStripTransactionDocument = new System.Windows.Forms.ToolStrip();
            toolStripButtonTransactionDocumentNew = new System.Windows.Forms.ToolStripButton();
            toolStripButtonTransactionDocumentDelete = new System.Windows.Forms.ToolStripButton();
            splitContainerTransactionDocumentData = new System.Windows.Forms.SplitContainer();
            splitContainerTransactionDocuments = new System.Windows.Forms.SplitContainer();
            panelHistoryWebbrowser = new System.Windows.Forms.Panel();
            splitContainerDocumentBrowser = new System.Windows.Forms.SplitContainer();
            webBrowserTransactionDocuments = new System.Windows.Forms.WebBrowser();
            panelHistoryImage = new System.Windows.Forms.Panel();
            pictureBoxTransactionDocuments = new System.Windows.Forms.PictureBox();
            toolStripDocumentImage = new System.Windows.Forms.ToolStrip();
            toolStripButtonDocumentZoomAdapt = new System.Windows.Forms.ToolStripButton();
            toolStripButtonDocumentZoom100Percent = new System.Windows.Forms.ToolStripButton();
            toolStripSeparatorDocument = new System.Windows.Forms.ToolStripSeparator();
            toolStripButtonDocumentRemove = new System.Windows.Forms.ToolStripButton();
            tableLayoutPanelTransactionDocument = new System.Windows.Forms.TableLayoutPanel();
            labelTransactionDocumentDisplayText = new System.Windows.Forms.Label();
            textBoxTransactionDocumentInternalNotes = new System.Windows.Forms.TextBox();
            textBoxTransactionDocumentDisplayText = new System.Windows.Forms.TextBox();
            labelDocumentNotes = new System.Windows.Forms.Label();
            labelDocumentType = new System.Windows.Forms.Label();
            comboBoxDocumentType = new System.Windows.Forms.ComboBox();
            checkBoxTransactionDocumentPdf = new System.Windows.Forms.CheckBox();
            buttonTransactionDocumentOpenPdf = new System.Windows.Forms.Button();
            tableLayoutPanelTransactionDocumentButtons = new System.Windows.Forms.TableLayoutPanel();
            buttonTransactionDocumentInsertDocument = new System.Windows.Forms.Button();
            buttonTransactionDocumentInsertUri = new System.Windows.Forms.Button();
            buttonTransactionDocumentFindUri = new System.Windows.Forms.Button();
            buttonTransactionDocumentInsertFile = new System.Windows.Forms.Button();
            tabPageTransactionReturn = new System.Windows.Forms.TabPage();
            tableLayoutPanelTransactionReturn = new System.Windows.Forms.TableLayoutPanel();
            labelTransactionReturnSchema = new System.Windows.Forms.Label();
            textBoxTransactionReturnSchema = new System.Windows.Forms.TextBox();
            toolStripTransactionReturn = new System.Windows.Forms.ToolStrip();
            toolStripButtonTransactionReturnOpenSchema = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripButtonTransactionReturnShowLists = new System.Windows.Forms.ToolStripButton();
            toolStripButtonTransactionReturnPreview = new System.Windows.Forms.ToolStripButton();
            toolStripButtonTransactionReturnPrint = new System.Windows.Forms.ToolStripButton();
            toolStripButtonTransactionReturnSave = new System.Windows.Forms.ToolStripButton();
            splitContainerTransactionReturn = new System.Windows.Forms.SplitContainer();
            panel1 = new System.Windows.Forms.Panel();
            webBrowserTransactionReturn = new System.Windows.Forms.WebBrowser();
            tableLayoutPanelTransactionReturnLists = new System.Windows.Forms.TableLayoutPanel();
            toolStripTransactionReturnListReturned = new System.Windows.Forms.ToolStrip();
            toolStripLabelTransactionReturnListReturned = new System.Windows.Forms.ToolStripLabel();
            toolStripButtonTransactionReturnSpecimen = new System.Windows.Forms.ToolStripButton();
            toolStripButtonTransactionReturnRemove = new System.Windows.Forms.ToolStripButton();
            toolStripButtonReturnScanner = new System.Windows.Forms.ToolStripButton();
            toolStripTextBoxReturnScanner = new System.Windows.Forms.ToolStripTextBox();
            labelTransactionReturnListOnLoan = new System.Windows.Forms.Label();
            listBoxTransactionReturnReturned = new System.Windows.Forms.ListBox();
            listBoxTransactionReturnOnLoan = new System.Windows.Forms.ListBox();
            checkBoxReturnSingleLines = new System.Windows.Forms.CheckBox();
            checkBoxReturnOrderByTaxa = new System.Windows.Forms.CheckBox();
            checkBoxReturnIncludeType = new System.Windows.Forms.CheckBox();
            tabPageBalance = new System.Windows.Forms.TabPage();
            tableLayoutPanelBalance = new System.Windows.Forms.TableLayoutPanel();
            labelBalanceSchema = new System.Windows.Forms.Label();
            textBoxBalanceSchema = new System.Windows.Forms.TextBox();
            toolStripBalance = new System.Windows.Forms.ToolStrip();
            toolStripButtonBalanceNew = new System.Windows.Forms.ToolStripButton();
            toolStripButtonBalanceOpenSchema = new System.Windows.Forms.ToolStripButton();
            toolStripSeparatorBalance = new System.Windows.Forms.ToolStripSeparator();
            toolStripButtonBalanceCreatePreview = new System.Windows.Forms.ToolStripButton();
            toolStripButtonBalanceSetPage = new System.Windows.Forms.ToolStripButton();
            toolStripButtonBalancePrint = new System.Windows.Forms.ToolStripButton();
            toolStripButtonBalanceSave = new System.Windows.Forms.ToolStripButton();
            splitContainerBalance = new System.Windows.Forms.SplitContainer();
            panelBalance = new System.Windows.Forms.Panel();
            webBrowserBalance = new System.Windows.Forms.WebBrowser();
            checkBoxBalanceIncludeFromSubcollections = new System.Windows.Forms.CheckBox();
            checkBoxBalanceIncludeToSubcollections = new System.Windows.Forms.CheckBox();
            checkBoxBalanceFromIncludeAllCollections = new System.Windows.Forms.CheckBox();
            checkBoxBalanceToIncludeAllCollections = new System.Windows.Forms.CheckBox();
            tabPageRequest = new System.Windows.Forms.TabPage();
            splitContainerRequest = new System.Windows.Forms.SplitContainer();
            panelRequest = new System.Windows.Forms.Panel();
            webBrowserRequest = new System.Windows.Forms.WebBrowser();
            tableLayoutPanelRequest = new System.Windows.Forms.TableLayoutPanel();
            labelRequestSchema = new System.Windows.Forms.Label();
            textBoxRequestSchema = new System.Windows.Forms.TextBox();
            toolStripRequest = new System.Windows.Forms.ToolStrip();
            toolStripButtonRequestOpenSchema = new System.Windows.Forms.ToolStripButton();
            toolStripSeparatorRequest = new System.Windows.Forms.ToolStripSeparator();
            toolStripButtonRequestShowList = new System.Windows.Forms.ToolStripButton();
            toolStripButtonRequestPreview = new System.Windows.Forms.ToolStripButton();
            toolStripButtonRequestPrint = new System.Windows.Forms.ToolStripButton();
            toolStripButtonRequestSave = new System.Windows.Forms.ToolStripButton();
            toolStripButtonRequestScanner = new System.Windows.Forms.ToolStripButton();
            labelRequestFrom = new System.Windows.Forms.Label();
            labelRequestTo = new System.Windows.Forms.Label();
            comboBoxRequestFrom = new System.Windows.Forms.ComboBox();
            comboBoxRequestTo = new System.Windows.Forms.ComboBox();
            labelRequestNumber = new System.Windows.Forms.Label();
            textBoxRequestNumber = new System.Windows.Forms.TextBox();
            labelRequestCollection = new System.Windows.Forms.Label();
            userControlModuleRelatedEntryRequestTo = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            labelRequestAgents = new System.Windows.Forms.Label();
            userControlModuleRelatedEntryRequestFrom = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            tableLayoutPanelRequestList = new System.Windows.Forms.TableLayoutPanel();
            comboBoxRequestAccessionNumber = new System.Windows.Forms.ComboBox();
            toolStripSpecimenListRequest = new System.Windows.Forms.ToolStrip();
            toolStripButtonRequestListDelete = new System.Windows.Forms.ToolStripButton();
            toolStripButtonRequestListFind = new System.Windows.Forms.ToolStripButton();
            listBoxRequest = new System.Windows.Forms.ListBox();
            collectionSpecimenPartReturnListBindingSource = new System.Windows.Forms.BindingSource(components);
            textBoxRequestAccessionNumber = new System.Windows.Forms.TextBox();
            labelRequestSpecimenList = new System.Windows.Forms.Label();
            listBoxRequestOnLoan = new System.Windows.Forms.ListBox();
            labelRequestSpecimenOnLoan = new System.Windows.Forms.Label();
            textBoxRequestType = new System.Windows.Forms.TextBox();
            tabPageNoAccess = new System.Windows.Forms.TabPage();
            tableLayoutPanelNoAccess = new System.Windows.Forms.TableLayoutPanel();
            pictureBoxNoAccess = new System.Windows.Forms.PictureBox();
            labelNoAccess = new System.Windows.Forms.Label();
            tabPagePayment = new System.Windows.Forms.TabPage();
            splitContainerPayment = new System.Windows.Forms.SplitContainer();
            listBoxPayment = new System.Windows.Forms.ListBox();
            transactionPaymentBindingSource = new System.Windows.Forms.BindingSource(components);
            toolStripPayment = new System.Windows.Forms.ToolStrip();
            toolStripButtonPaymentAdd = new System.Windows.Forms.ToolStripButton();
            toolStripButtonPaymentDelete = new System.Windows.Forms.ToolStripButton();
            tableLayoutPanelPayment = new System.Windows.Forms.TableLayoutPanel();
            labelPaymentIdentifier = new System.Windows.Forms.Label();
            textBoxPaymentIdentifier = new System.Windows.Forms.TextBox();
            labelPaymentAmount = new System.Windows.Forms.Label();
            textBoxPaymentAmount = new System.Windows.Forms.TextBox();
            labelPaymentCurrency = new System.Windows.Forms.Label();
            labelPaymentForeignAmount = new System.Windows.Forms.Label();
            textBoxPaymentForeignAmount = new System.Windows.Forms.TextBox();
            textBoxPaymentForeignAmountCurrency = new System.Windows.Forms.TextBox();
            labelPaymentPayer = new System.Windows.Forms.Label();
            userControlModuleRelatedEntryPaymentPayer = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            userControlModuleRelatedEntryPaymentRecipient = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            labelPaymentRecipient = new System.Windows.Forms.Label();
            labelPaymentDate = new System.Windows.Forms.Label();
            dateTimePickerPaymentDate = new System.Windows.Forms.DateTimePicker();
            labelPaymentDateSupplement = new System.Windows.Forms.Label();
            textBoxPaymentDateSupplement = new System.Windows.Forms.TextBox();
            labelPaymentNotes = new System.Windows.Forms.Label();
            textBoxPaymentNotes = new System.Windows.Forms.TextBox();
            labelPaymentURI = new System.Windows.Forms.Label();
            textBoxPaymentURI = new System.Windows.Forms.TextBox();
            buttonPaymentURI = new System.Windows.Forms.Button();
            buttonPaymentDate = new System.Windows.Forms.Button();
            buttonSavePayment = new System.Windows.Forms.Button();
            tabPageAgents = new System.Windows.Forms.TabPage();
            splitContainerAgents = new System.Windows.Forms.SplitContainer();
            listBoxAgents = new System.Windows.Forms.ListBox();
            transactionAgentBindingSource = new System.Windows.Forms.BindingSource(components);
            toolStripAgents = new System.Windows.Forms.ToolStrip();
            toolStripButtonAgentsAdd = new System.Windows.Forms.ToolStripButton();
            toolStripButtonAgentsRemove = new System.Windows.Forms.ToolStripButton();
            tableLayoutPanelAgents = new System.Windows.Forms.TableLayoutPanel();
            userControlModuleRelatedEntryAgent = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            labelAgentRole = new System.Windows.Forms.Label();
            labelAgentNotes = new System.Windows.Forms.Label();
            textBoxAgentNotes = new System.Windows.Forms.TextBox();
            comboBoxAgentRole = new System.Windows.Forms.ComboBox();
            tabPageIdentifier = new System.Windows.Forms.TabPage();
            splitContainerIdentifier = new System.Windows.Forms.SplitContainer();
            listBoxIdentifier = new System.Windows.Forms.ListBox();
            externalIdentifierBindingSource = new System.Windows.Forms.BindingSource(components);
            toolStripIdentifier = new System.Windows.Forms.ToolStrip();
            toolStripButtonIdentifierAdd = new System.Windows.Forms.ToolStripButton();
            toolStripButtonIdentifierRemove = new System.Windows.Forms.ToolStripButton();
            tableLayoutPanelIdentifier = new System.Windows.Forms.TableLayoutPanel();
            labelIdentifierType = new System.Windows.Forms.Label();
            labelIdentifier = new System.Windows.Forms.Label();
            textBoxIdentifierType = new System.Windows.Forms.TextBox();
            textBoxIdentifier = new System.Windows.Forms.TextBox();
            buttonIdenitifierURL = new System.Windows.Forms.Button();
            labelIdenitifierURL = new System.Windows.Forms.Label();
            textBoxIdenitifierURL = new System.Windows.Forms.TextBox();
            labelIdenitifierNotes = new System.Windows.Forms.Label();
            textBoxIdenitifierNotes = new System.Windows.Forms.TextBox();
            tabPageChart = new System.Windows.Forms.TabPage();
            tabControlChart = new System.Windows.Forms.TabControl();
            tabPageChartResult = new System.Windows.Forms.TabPage();
            tableLayoutPanelChart = new System.Windows.Forms.TableLayoutPanel();
            labelChartHeader = new System.Windows.Forms.Label();
            buttonChartGenerate = new System.Windows.Forms.Button();
            groupBoxChartGrouping = new System.Windows.Forms.GroupBox();
            tableLayoutPanelChartGrouping = new System.Windows.Forms.TableLayoutPanel();
            radioButtonChartGroupingNone = new System.Windows.Forms.RadioButton();
            radioButtonChartGroupingCollection = new System.Windows.Forms.RadioButton();
            radioButtonChartGroupingTransactionType = new System.Windows.Forms.RadioButton();
            groupBoxChartStart = new System.Windows.Forms.GroupBox();
            numericUpDownChartStart = new System.Windows.Forms.NumericUpDown();
            checkBoxChartStart = new System.Windows.Forms.CheckBox();
            checkBoxChartAddNumberOfUnits = new System.Windows.Forms.CheckBox();
            tabControlChartSettingsForChart = new System.Windows.Forms.TabControl();
            tabPageChartAdminColl = new System.Windows.Forms.TabPage();
            checkedListBoxChartCollections = new System.Windows.Forms.CheckedListBox();
            imageListTab = new System.Windows.Forms.ImageList(components);
            tabPageChartData = new System.Windows.Forms.TabPage();
            tableLayoutPanelChartData = new System.Windows.Forms.TableLayoutPanel();
            dataGridViewChartData = new System.Windows.Forms.DataGridView();
            tabControlChartData = new System.Windows.Forms.TabControl();
            tabPageChartDataAddress = new System.Windows.Forms.TabPage();
            tableLayoutPanelChartDataAddress = new System.Windows.Forms.TableLayoutPanel();
            checkBoxChartAddressCity = new System.Windows.Forms.CheckBox();
            checkBoxChartAddressCountry = new System.Windows.Forms.CheckBox();
            listBoxChartAddressSelf = new System.Windows.Forms.ListBox();
            labelChartAddressSelf = new System.Windows.Forms.Label();
            labelChartAddressOutput = new System.Windows.Forms.Label();
            checkBoxChartAddressPartner = new System.Windows.Forms.CheckBox();
            toolStripChartAddressSelf = new System.Windows.Forms.ToolStrip();
            toolStripButtonChartAddressSelfAdd = new System.Windows.Forms.ToolStripButton();
            toolStripButtonChartAddressSelfRemove = new System.Windows.Forms.ToolStripButton();
            labelChartData = new System.Windows.Forms.Label();
            buttonChartDataExport = new System.Windows.Forms.Button();
            buttonChartDataGet = new System.Windows.Forms.Button();
            radioButtonChartDataChart = new System.Windows.Forms.RadioButton();
            radioButtonChartDataSettings = new System.Windows.Forms.RadioButton();
            tabControlChartSettings = new System.Windows.Forms.TabControl();
            tabPageChartTypes = new System.Windows.Forms.TabPage();
            checkedListBoxChartTransactionType = new System.Windows.Forms.CheckedListBox();
            tableLayoutPanelAdministratingAgent = new System.Windows.Forms.TableLayoutPanel();
            labelBalanceAdministration = new System.Windows.Forms.Label();
            userControlModuleRelatedEntryBalanceAdministration = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            tableLayoutPanelHeader = new System.Windows.Forms.TableLayoutPanel();
            buttonShowSpecimenLists = new System.Windows.Forms.Button();
            buttonShowHierarchy = new System.Windows.Forms.Button();
            buttonTableEditor = new System.Windows.Forms.Button();
            buttonDisplaySettings = new System.Windows.Forms.Button();
            buttonHistory = new System.Windows.Forms.Button();
            buttonHeaderFeedback = new System.Windows.Forms.Button();
            textBoxHeaderID = new System.Windows.Forms.TextBox();
            labelHeaderID = new System.Windows.Forms.Label();
            labelHeaderTitle = new System.Windows.Forms.Label();
            buttonMaintenance = new System.Windows.Forms.Button();
            userControlSpecimenList = new DiversityCollection.UserControls.UserControlSpecimenList();
            collectionSpecimenPartSentListBindingSource = new System.Windows.Forms.BindingSource(components);
            collectionSpecimenPartPartialReturnListBindingSource = new System.Windows.Forms.BindingSource(components);
            labelHeader = new System.Windows.Forms.Label();
            openFileDialogSendingSchema = new System.Windows.Forms.OpenFileDialog();
            timerSending = new System.Windows.Forms.Timer(components);
            timerReturn = new System.Windows.Forms.Timer(components);
            toolTip = new System.Windows.Forms.ToolTip(components);
            helpProvider = new System.Windows.Forms.HelpProvider();
            imageListSpecimenList = new System.Windows.Forms.ImageList(components);
            userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            timerPrinting = new System.Windows.Forms.Timer(components);
            transactionTableAdapter = new DiversityCollection.Datasets.DataSetTransactionTableAdapters.TransactionTableAdapter();
            collectionSpecimenPartOnLoanListTableAdapter = new DiversityCollection.Datasets.DataSetTransactionTableAdapters.CollectionSpecimenPartOnLoanListTableAdapter();
            collectionSpecimenPartReturnListTableAdapter = new DiversityCollection.Datasets.DataSetTransactionTableAdapters.CollectionSpecimenPartReturnListTableAdapter();
            collectionSpecimenPartListTableAdapter = new DiversityCollection.Datasets.DataSetTransactionTableAdapters.CollectionSpecimenPartListTableAdapter();
            transactionDocumentTableAdapter = new DiversityCollection.Datasets.DataSetTransactionTableAdapters.TransactionDocumentTableAdapter();
            collectionSpecimenPartPartialReturnListTableAdapter = new DiversityCollection.Datasets.DataSetTransactionTableAdapters.CollectionSpecimenPartPartialReturnListTableAdapter();
            timerRequest = new System.Windows.Forms.Timer(components);
            collectionSpecimenPartSentListTableAdapter = new DiversityCollection.Datasets.DataSetTransactionTableAdapters.CollectionSpecimenPartSentListTableAdapter();
            imageListSetDate = new System.Windows.Forms.ImageList(components);
            collectionSpecimenPartForwardingListTableAdapter = new DiversityCollection.Datasets.DataSetTransactionTableAdapters.CollectionSpecimenPartForwardingListTableAdapter();
            collectionSpecimenPartTransactionReturnListTableAdapter = new DiversityCollection.Datasets.DataSetTransactionTableAdapters.CollectionSpecimenPartTransactionReturnListTableAdapter();
            transactionAgentTableAdapter = new DiversityCollection.Datasets.DataSetTransactionTableAdapters.TransactionAgentTableAdapter();
            transactionPaymentTableAdapter = new DiversityCollection.Datasets.DataSetTransactionTableAdapters.TransactionPaymentTableAdapter();
            openFileDialogDocument = new System.Windows.Forms.OpenFileDialog();
            checkBoxSendingAllUnits = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerData).BeginInit();
            splitContainerData.Panel1.SuspendLayout();
            splitContainerData.Panel2.SuspendLayout();
            splitContainerData.SuspendLayout();
            groupBoxData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerTransaction).BeginInit();
            splitContainerTransaction.Panel1.SuspendLayout();
            splitContainerTransaction.Panel2.SuspendLayout();
            splitContainerTransaction.SuspendLayout();
            toolStripTransaction.SuspendLayout();
            tabControlTransaction.SuspendLayout();
            tabPageDataEntry.SuspendLayout();
            tableLayoutPanelCollection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)transactionBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataSetTransaction).BeginInit();
            groupBoxFrom.SuspendLayout();
            tableLayoutPanelFrom.SuspendLayout();
            groupBoxTo.SuspendLayout();
            tableLayoutPanelTo.SuspendLayout();
            tabPageSending.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerSending).BeginInit();
            splitContainerSending.Panel1.SuspendLayout();
            splitContainerSending.Panel2.SuspendLayout();
            splitContainerSending.SuspendLayout();
            tableLayoutPanelSending.SuspendLayout();
            toolStripSending.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSendingRestrictToMaterial).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSendingRestrictToCollection).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainerWebbrowserSending).BeginInit();
            splitContainerWebbrowserSending.Panel1.SuspendLayout();
            splitContainerWebbrowserSending.SuspendLayout();
            panelWebbrowserSending.SuspendLayout();
            tableLayoutPanelSendingList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)collectionSpecimenPartOnLoanListBindingSource).BeginInit();
            toolStripSendingList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)collectionSpecimenPartTransactionReturnListBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)collectionSpecimenPartForwardingListBindingSource).BeginInit();
            tabPageForwarding.SuspendLayout();
            tableLayoutPanelForwarding.SuspendLayout();
            toolStripForwarding.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerForwarding).BeginInit();
            splitContainerForwarding.Panel1.SuspendLayout();
            splitContainerForwarding.Panel2.SuspendLayout();
            splitContainerForwarding.SuspendLayout();
            panel2.SuspendLayout();
            tableLayoutPanelForwardingLists.SuspendLayout();
            toolStripForwardingSpecimenForwarded.SuspendLayout();
            tabPageConfirmation.SuspendLayout();
            tableLayoutPanelConfirmation.SuspendLayout();
            toolStripConfirmation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerConfirmation).BeginInit();
            splitContainerConfirmation.Panel1.SuspendLayout();
            splitContainerConfirmation.Panel2.SuspendLayout();
            splitContainerConfirmation.SuspendLayout();
            panelReminderWebbrowser.SuspendLayout();
            tabPageReminder.SuspendLayout();
            tableLayoutPanelAdmonition.SuspendLayout();
            toolStripReminder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerReminder).BeginInit();
            splitContainerReminder.Panel1.SuspendLayout();
            splitContainerReminder.Panel2.SuspendLayout();
            splitContainerReminder.SuspendLayout();
            panelAdmonitionWebbrowser.SuspendLayout();
            tableLayoutPanelReminderList.SuspendLayout();
            tabPagePrinting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerPrinting).BeginInit();
            splitContainerPrinting.Panel1.SuspendLayout();
            splitContainerPrinting.Panel2.SuspendLayout();
            splitContainerPrinting.SuspendLayout();
            panelWebbrowserPrint.SuspendLayout();
            tableLayoutPanelPrinting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIncludeSubCollections).BeginInit();
            toolStripPrinting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIncludeSubTransactions).BeginInit();
            tableLayoutPanelPrintingList.SuspendLayout();
            toolStripSpecimenList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)collectionSpecimenPartListBindingSource).BeginInit();
            tabPageDocuments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerDocuments).BeginInit();
            splitContainerDocuments.Panel1.SuspendLayout();
            splitContainerDocuments.Panel2.SuspendLayout();
            splitContainerDocuments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)transactionDocumentBindingSource).BeginInit();
            toolStripTransactionDocument.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerTransactionDocumentData).BeginInit();
            splitContainerTransactionDocumentData.Panel1.SuspendLayout();
            splitContainerTransactionDocumentData.Panel2.SuspendLayout();
            splitContainerTransactionDocumentData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerTransactionDocuments).BeginInit();
            splitContainerTransactionDocuments.Panel1.SuspendLayout();
            splitContainerTransactionDocuments.Panel2.SuspendLayout();
            splitContainerTransactionDocuments.SuspendLayout();
            panelHistoryWebbrowser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerDocumentBrowser).BeginInit();
            splitContainerDocumentBrowser.Panel1.SuspendLayout();
            splitContainerDocumentBrowser.SuspendLayout();
            panelHistoryImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxTransactionDocuments).BeginInit();
            toolStripDocumentImage.SuspendLayout();
            tableLayoutPanelTransactionDocument.SuspendLayout();
            tableLayoutPanelTransactionDocumentButtons.SuspendLayout();
            tabPageTransactionReturn.SuspendLayout();
            tableLayoutPanelTransactionReturn.SuspendLayout();
            toolStripTransactionReturn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerTransactionReturn).BeginInit();
            splitContainerTransactionReturn.Panel1.SuspendLayout();
            splitContainerTransactionReturn.Panel2.SuspendLayout();
            splitContainerTransactionReturn.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanelTransactionReturnLists.SuspendLayout();
            toolStripTransactionReturnListReturned.SuspendLayout();
            tabPageBalance.SuspendLayout();
            tableLayoutPanelBalance.SuspendLayout();
            toolStripBalance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerBalance).BeginInit();
            splitContainerBalance.Panel1.SuspendLayout();
            splitContainerBalance.SuspendLayout();
            panelBalance.SuspendLayout();
            tabPageRequest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerRequest).BeginInit();
            splitContainerRequest.Panel1.SuspendLayout();
            splitContainerRequest.Panel2.SuspendLayout();
            splitContainerRequest.SuspendLayout();
            panelRequest.SuspendLayout();
            tableLayoutPanelRequest.SuspendLayout();
            toolStripRequest.SuspendLayout();
            tableLayoutPanelRequestList.SuspendLayout();
            toolStripSpecimenListRequest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)collectionSpecimenPartReturnListBindingSource).BeginInit();
            tabPageNoAccess.SuspendLayout();
            tableLayoutPanelNoAccess.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxNoAccess).BeginInit();
            tabPagePayment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerPayment).BeginInit();
            splitContainerPayment.Panel1.SuspendLayout();
            splitContainerPayment.Panel2.SuspendLayout();
            splitContainerPayment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)transactionPaymentBindingSource).BeginInit();
            toolStripPayment.SuspendLayout();
            tableLayoutPanelPayment.SuspendLayout();
            tabPageAgents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerAgents).BeginInit();
            splitContainerAgents.Panel1.SuspendLayout();
            splitContainerAgents.Panel2.SuspendLayout();
            splitContainerAgents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)transactionAgentBindingSource).BeginInit();
            toolStripAgents.SuspendLayout();
            tableLayoutPanelAgents.SuspendLayout();
            tabPageIdentifier.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerIdentifier).BeginInit();
            splitContainerIdentifier.Panel1.SuspendLayout();
            splitContainerIdentifier.Panel2.SuspendLayout();
            splitContainerIdentifier.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)externalIdentifierBindingSource).BeginInit();
            toolStripIdentifier.SuspendLayout();
            tableLayoutPanelIdentifier.SuspendLayout();
            tabPageChart.SuspendLayout();
            tabControlChart.SuspendLayout();
            tabPageChartResult.SuspendLayout();
            tableLayoutPanelChart.SuspendLayout();
            groupBoxChartGrouping.SuspendLayout();
            tableLayoutPanelChartGrouping.SuspendLayout();
            groupBoxChartStart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownChartStart).BeginInit();
            tabControlChartSettingsForChart.SuspendLayout();
            tabPageChartAdminColl.SuspendLayout();
            tabPageChartData.SuspendLayout();
            tableLayoutPanelChartData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewChartData).BeginInit();
            tabControlChartData.SuspendLayout();
            tabPageChartDataAddress.SuspendLayout();
            tableLayoutPanelChartDataAddress.SuspendLayout();
            toolStripChartAddressSelf.SuspendLayout();
            tabControlChartSettings.SuspendLayout();
            tabPageChartTypes.SuspendLayout();
            tableLayoutPanelAdministratingAgent.SuspendLayout();
            tableLayoutPanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)collectionSpecimenPartSentListBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)collectionSpecimenPartPartialReturnListBindingSource).BeginInit();
            SuspendLayout();
            // 
            // splitContainerMain
            // 
            splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(splitContainerMain, "Transaction");
            helpProvider.SetHelpNavigator(splitContainerMain, System.Windows.Forms.HelpNavigator.KeywordIndex);
            splitContainerMain.Location = new System.Drawing.Point(0, 0);
            splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(userControlQueryList);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(splitContainerData);
            helpProvider.SetShowHelp(splitContainerMain, true);
            splitContainerMain.Size = new System.Drawing.Size(1070, 654);
            splitContainerMain.SplitterDistance = 213;
            splitContainerMain.TabIndex = 1;
            // 
            // userControlQueryList
            // 
            userControlQueryList.BacklinkUpdateEnabled = false;
            userControlQueryList.Connection = null;
            userControlQueryList.DisplayTextSelectedItem = "";
            userControlQueryList.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(userControlQueryList, "Query");
            helpProvider.SetHelpNavigator(userControlQueryList, System.Windows.Forms.HelpNavigator.KeywordIndex);
            userControlQueryList.IDisNumeric = true;
            userControlQueryList.ImageList = null;
            userControlQueryList.IsPredefinedQuery = false;
            userControlQueryList.Location = new System.Drawing.Point(0, 0);
            userControlQueryList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
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
            helpProvider.SetShowHelp(userControlQueryList, true);
            userControlQueryList.Size = new System.Drawing.Size(213, 654);
            userControlQueryList.TabIndex = 0;
            userControlQueryList.TableColors = null;
            userControlQueryList.TableImageIndex = null;
            userControlQueryList.WhereClause = null;
            // 
            // splitContainerData
            // 
            splitContainerData.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerData.Location = new System.Drawing.Point(0, 0);
            splitContainerData.Name = "splitContainerData";
            // 
            // splitContainerData.Panel1
            // 
            splitContainerData.Panel1.Controls.Add(groupBoxData);
            splitContainerData.Panel1.Controls.Add(tableLayoutPanelHeader);
            // 
            // splitContainerData.Panel2
            // 
            splitContainerData.Panel2.Controls.Add(userControlSpecimenList);
            splitContainerData.Panel2Collapsed = true;
            splitContainerData.Size = new System.Drawing.Size(853, 654);
            splitContainerData.SplitterDistance = 667;
            splitContainerData.TabIndex = 3;
            // 
            // groupBoxData
            // 
            groupBoxData.AccessibleName = "Transaction";
            groupBoxData.Controls.Add(splitContainerTransaction);
            groupBoxData.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxData.Location = new System.Drawing.Point(0, 26);
            groupBoxData.Name = "groupBoxData";
            groupBoxData.Size = new System.Drawing.Size(853, 628);
            groupBoxData.TabIndex = 2;
            groupBoxData.TabStop = false;
            groupBoxData.Text = "Transaction";
            // 
            // splitContainerTransaction
            // 
            splitContainerTransaction.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerTransaction.Location = new System.Drawing.Point(3, 19);
            splitContainerTransaction.Name = "splitContainerTransaction";
            splitContainerTransaction.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerTransaction.Panel1
            // 
            splitContainerTransaction.Panel1.Controls.Add(treeViewTransaction);
            splitContainerTransaction.Panel1.Controls.Add(toolStripTransaction);
            // 
            // splitContainerTransaction.Panel2
            // 
            splitContainerTransaction.Panel2.Controls.Add(tabControlTransaction);
            splitContainerTransaction.Panel2.Controls.Add(tableLayoutPanelAdministratingAgent);
            splitContainerTransaction.Size = new System.Drawing.Size(847, 606);
            splitContainerTransaction.SplitterDistance = 139;
            splitContainerTransaction.TabIndex = 1;
            // 
            // treeViewTransaction
            // 
            treeViewTransaction.AllowDrop = true;
            treeViewTransaction.Dock = System.Windows.Forms.DockStyle.Fill;
            treeViewTransaction.Location = new System.Drawing.Point(0, 0);
            treeViewTransaction.Name = "treeViewTransaction";
            treeViewTransaction.Size = new System.Drawing.Size(823, 139);
            treeViewTransaction.TabIndex = 0;
            treeViewTransaction.AfterSelect += treeViewTransaction_AfterSelect;
            // 
            // toolStripTransaction
            // 
            toolStripTransaction.Dock = System.Windows.Forms.DockStyle.Right;
            toolStripTransaction.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonNew, toolStripButtonCopy, toolStripButtonDelete, toolStripButtonSpecimenList, toolStripButtonSetParent, toolStripButtonShowTasks });
            toolStripTransaction.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            toolStripTransaction.Location = new System.Drawing.Point(823, 0);
            toolStripTransaction.Name = "toolStripTransaction";
            toolStripTransaction.Size = new System.Drawing.Size(24, 139);
            toolStripTransaction.TabIndex = 1;
            toolStripTransaction.Text = "toolStrip1";
            // 
            // toolStripButtonNew
            // 
            toolStripButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonNew.Image = Resource.New1;
            toolStripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonNew.Name = "toolStripButtonNew";
            toolStripButtonNew.Size = new System.Drawing.Size(23, 20);
            toolStripButtonNew.Text = "Insert a new item dependent on the selected item";
            // 
            // toolStripButtonCopy
            // 
            toolStripButtonCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonCopy.Image = Resource.Copy1;
            toolStripButtonCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonCopy.Name = "toolStripButtonCopy";
            toolStripButtonCopy.Size = new System.Drawing.Size(23, 20);
            toolStripButtonCopy.Text = "Create of copy of the selected item dependent on the selected item";
            // 
            // toolStripButtonDelete
            // 
            toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonDelete.Image = Resource.Delete;
            toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonDelete.Name = "toolStripButtonDelete";
            toolStripButtonDelete.Size = new System.Drawing.Size(23, 20);
            toolStripButtonDelete.Text = "delete the selected item";
            // 
            // toolStripButtonSpecimenList
            // 
            toolStripButtonSpecimenList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSpecimenList.Image = Resource.List;
            toolStripButtonSpecimenList.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSpecimenList.Name = "toolStripButtonSpecimenList";
            toolStripButtonSpecimenList.Size = new System.Drawing.Size(23, 20);
            toolStripButtonSpecimenList.Text = "Show specimen list";
            toolStripButtonSpecimenList.Visible = false;
            // 
            // toolStripButtonSetParent
            // 
            toolStripButtonSetParent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSetParent.Image = Resource.SetParent;
            toolStripButtonSetParent.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSetParent.Name = "toolStripButtonSetParent";
            toolStripButtonSetParent.Size = new System.Drawing.Size(23, 20);
            toolStripButtonSetParent.Text = "Set or change the superior transaction of the selected entry";
            // 
            // toolStripButtonShowTasks
            // 
            toolStripButtonShowTasks.BackColor = System.Drawing.Color.Yellow;
            toolStripButtonShowTasks.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonShowTasks.Image = Resource.TaskGrey;
            toolStripButtonShowTasks.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonShowTasks.Name = "toolStripButtonShowTasks";
            toolStripButtonShowTasks.Size = new System.Drawing.Size(23, 20);
            toolStripButtonShowTasks.Text = "Show tasks";
            toolStripButtonShowTasks.Click += toolStripButtonShowTasks_Click;
            // 
            // tabControlTransaction
            // 
            tabControlTransaction.Controls.Add(tabPageDataEntry);
            tabControlTransaction.Controls.Add(tabPageSending);
            tabControlTransaction.Controls.Add(tabPageForwarding);
            tabControlTransaction.Controls.Add(tabPageConfirmation);
            tabControlTransaction.Controls.Add(tabPageReminder);
            tabControlTransaction.Controls.Add(tabPagePrinting);
            tabControlTransaction.Controls.Add(tabPageDocuments);
            tabControlTransaction.Controls.Add(tabPageTransactionReturn);
            tabControlTransaction.Controls.Add(tabPageBalance);
            tabControlTransaction.Controls.Add(tabPageRequest);
            tabControlTransaction.Controls.Add(tabPageNoAccess);
            tabControlTransaction.Controls.Add(tabPagePayment);
            tabControlTransaction.Controls.Add(tabPageAgents);
            tabControlTransaction.Controls.Add(tabPageIdentifier);
            tabControlTransaction.Controls.Add(tabPageChart);
            tabControlTransaction.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(tabControlTransaction, "Transaction");
            helpProvider.SetHelpNavigator(tabControlTransaction, System.Windows.Forms.HelpNavigator.KeywordIndex);
            tabControlTransaction.ImageList = imageListTab;
            tabControlTransaction.Location = new System.Drawing.Point(0, 28);
            tabControlTransaction.Name = "tabControlTransaction";
            tabControlTransaction.SelectedIndex = 0;
            helpProvider.SetShowHelp(tabControlTransaction, true);
            tabControlTransaction.Size = new System.Drawing.Size(847, 435);
            tabControlTransaction.TabIndex = 0;
            // 
            // tabPageDataEntry
            // 
            tabPageDataEntry.Controls.Add(tableLayoutPanelCollection);
            tabPageDataEntry.ImageIndex = 0;
            tabPageDataEntry.Location = new System.Drawing.Point(4, 24);
            tabPageDataEntry.Name = "tabPageDataEntry";
            tabPageDataEntry.Size = new System.Drawing.Size(839, 407);
            tabPageDataEntry.TabIndex = 0;
            tabPageDataEntry.Text = "Details";
            tabPageDataEntry.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelCollection
            // 
            tableLayoutPanelCollection.ColumnCount = 13;
            tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelCollection.Controls.Add(labelTransactionTitle, 0, 1);
            tableLayoutPanelCollection.Controls.Add(textBoxTransactionTitle, 1, 1);
            tableLayoutPanelCollection.Controls.Add(labelBeginDate, 0, 5);
            tableLayoutPanelCollection.Controls.Add(dateTimePickerBeginDate, 3, 5);
            tableLayoutPanelCollection.Controls.Add(labelTransactionComment, 0, 7);
            tableLayoutPanelCollection.Controls.Add(textBoxTransactionComment, 1, 7);
            tableLayoutPanelCollection.Controls.Add(labelInternalNotes, 0, 11);
            tableLayoutPanelCollection.Controls.Add(textBoxInternalNotes, 1, 11);
            tableLayoutPanelCollection.Controls.Add(labelResponsible, 0, 10);
            tableLayoutPanelCollection.Controls.Add(userControlModuleRelatedEntryResponsible, 1, 10);
            tableLayoutPanelCollection.Controls.Add(comboBoxTransactionCommentAdd, 6, 8);
            tableLayoutPanelCollection.Controls.Add(buttonTransactionCommentInsertSelected, 5, 8);
            tableLayoutPanelCollection.Controls.Add(labelTransactionType, 0, 0);
            tableLayoutPanelCollection.Controls.Add(comboBoxTransactionType, 1, 0);
            tableLayoutPanelCollection.Controls.Add(labelMaterialCategory, 0, 2);
            tableLayoutPanelCollection.Controls.Add(comboBoxMaterialCategory, 1, 2);
            tableLayoutPanelCollection.Controls.Add(labelAgreedEndDate, 0, 6);
            tableLayoutPanelCollection.Controls.Add(dateTimePickerAgreedEndDate, 3, 6);
            tableLayoutPanelCollection.Controls.Add(labelAdministratingCollection, 9, 0);
            tableLayoutPanelCollection.Controls.Add(comboBoxAdministratingCollection, 10, 0);
            tableLayoutPanelCollection.Controls.Add(labelActualEndDate, 4, 6);
            tableLayoutPanelCollection.Controls.Add(dateTimePickerActualEndDate, 7, 6);
            tableLayoutPanelCollection.Controls.Add(textBoxInvestigator, 10, 6);
            tableLayoutPanelCollection.Controls.Add(labelInvestigator, 9, 6);
            tableLayoutPanelCollection.Controls.Add(labelReportingCategory, 9, 5);
            tableLayoutPanelCollection.Controls.Add(comboBoxReportingCategory, 10, 5);
            tableLayoutPanelCollection.Controls.Add(labelMaterialDescription, 9, 2);
            tableLayoutPanelCollection.Controls.Add(textBoxMaterialDescription, 10, 2);
            tableLayoutPanelCollection.Controls.Add(labelMaterialCollectors, 0, 3);
            tableLayoutPanelCollection.Controls.Add(textBoxMaterialCollectors, 1, 3);
            tableLayoutPanelCollection.Controls.Add(comboBoxAgreedEndDateAddMonths, 2, 6);
            tableLayoutPanelCollection.Controls.Add(buttonBeginDateEdit, 1, 5);
            tableLayoutPanelCollection.Controls.Add(buttonActualEndDateEdit, 5, 6);
            tableLayoutPanelCollection.Controls.Add(buttonAgreedEndDateEdit, 1, 6);
            tableLayoutPanelCollection.Controls.Add(comboBoxActualEndDateAddMonths, 6, 6);
            tableLayoutPanelCollection.Controls.Add(labelCommentPhrases, 0, 8);
            tableLayoutPanelCollection.Controls.Add(groupBoxFrom, 0, 4);
            tableLayoutPanelCollection.Controls.Add(groupBoxTo, 9, 4);
            tableLayoutPanelCollection.Controls.Add(comboBoxMaterialSource, 10, 3);
            tableLayoutPanelCollection.Controls.Add(labelMaterialSource, 9, 3);
            tableLayoutPanelCollection.Controls.Add(textBoxNumberOfUnits, 12, 3);
            tableLayoutPanelCollection.Controls.Add(labelNumberOfUnits, 11, 3);
            tableLayoutPanelCollection.Controls.Add(labelDateSupplement, 4, 5);
            tableLayoutPanelCollection.Controls.Add(textBoxDateSupplement, 5, 5);
            tableLayoutPanelCollection.Controls.Add(labelToTransactionInventoryNumber, 11, 1);
            tableLayoutPanelCollection.Controls.Add(textBoxToTransactionInventoryNumber, 12, 1);
            tableLayoutPanelCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(tableLayoutPanelCollection, "Transaction");
            helpProvider.SetHelpNavigator(tableLayoutPanelCollection, System.Windows.Forms.HelpNavigator.KeywordIndex);
            tableLayoutPanelCollection.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelCollection.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            tableLayoutPanelCollection.Name = "tableLayoutPanelCollection";
            tableLayoutPanelCollection.RowCount = 12;
            tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            helpProvider.SetShowHelp(tableLayoutPanelCollection, true);
            tableLayoutPanelCollection.Size = new System.Drawing.Size(839, 407);
            tableLayoutPanelCollection.TabIndex = 0;
            // 
            // labelTransactionTitle
            // 
            labelTransactionTitle.AccessibleName = "Transaction.TransactionTitle";
            labelTransactionTitle.AutoSize = true;
            labelTransactionTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            labelTransactionTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            labelTransactionTitle.Location = new System.Drawing.Point(3, 27);
            labelTransactionTitle.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            labelTransactionTitle.Name = "labelTransactionTitle";
            labelTransactionTitle.Size = new System.Drawing.Size(64, 26);
            labelTransactionTitle.TabIndex = 2;
            labelTransactionTitle.Text = "Title:";
            labelTransactionTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxTransactionTitle
            // 
            textBoxTransactionTitle.BackColor = System.Drawing.SystemColors.Window;
            tableLayoutPanelCollection.SetColumnSpan(textBoxTransactionTitle, 10);
            textBoxTransactionTitle.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionBindingSource, "TransactionTitle", true));
            textBoxTransactionTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxTransactionTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            textBoxTransactionTitle.Location = new System.Drawing.Point(67, 27);
            textBoxTransactionTitle.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            textBoxTransactionTitle.MaxLength = 200;
            textBoxTransactionTitle.Name = "textBoxTransactionTitle";
            textBoxTransactionTitle.Size = new System.Drawing.Size(663, 20);
            textBoxTransactionTitle.TabIndex = 3;
            // 
            // transactionBindingSource
            // 
            transactionBindingSource.DataMember = "Transaction";
            transactionBindingSource.DataSource = dataSetTransaction;
            // 
            // dataSetTransaction
            // 
            dataSetTransaction.DataSetName = "DataSetTransaction";
            dataSetTransaction.Namespace = "http://tempuri.org/DataSetTransaction.xsd";
            dataSetTransaction.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // labelBeginDate
            // 
            labelBeginDate.AccessibleName = "Transaction.BeginDate";
            labelBeginDate.AutoSize = true;
            labelBeginDate.Dock = System.Windows.Forms.DockStyle.Fill;
            labelBeginDate.Location = new System.Drawing.Point(3, 202);
            labelBeginDate.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            labelBeginDate.Name = "labelBeginDate";
            labelBeginDate.Size = new System.Drawing.Size(64, 29);
            labelBeginDate.TabIndex = 4;
            labelBeginDate.Text = "Begin:";
            labelBeginDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePickerBeginDate
            // 
            dateTimePickerBeginDate.CustomFormat = "yyyy.MM.dd";
            dateTimePickerBeginDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", transactionBindingSource, "BeginDate", true));
            dateTimePickerBeginDate.Dock = System.Windows.Forms.DockStyle.Fill;
            dateTimePickerBeginDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dateTimePickerBeginDate.Location = new System.Drawing.Point(103, 205);
            dateTimePickerBeginDate.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            dateTimePickerBeginDate.MinimumSize = new System.Drawing.Size(74, 0);
            dateTimePickerBeginDate.Name = "dateTimePickerBeginDate";
            dateTimePickerBeginDate.Size = new System.Drawing.Size(88, 23);
            dateTimePickerBeginDate.TabIndex = 14;
            dateTimePickerBeginDate.CloseUp += dateTimePickerBeginDate_CloseUp;
            dateTimePickerBeginDate.ValueChanged += dateTimePickerBeginDate_ValueChanged;
            dateTimePickerBeginDate.Validated += dateTimePickerBeginDate_Validated;
            // 
            // labelTransactionComment
            // 
            labelTransactionComment.AccessibleName = "Transaction.TransactionComment";
            labelTransactionComment.AutoSize = true;
            labelTransactionComment.Dock = System.Windows.Forms.DockStyle.Fill;
            labelTransactionComment.Location = new System.Drawing.Point(3, 261);
            labelTransactionComment.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            labelTransactionComment.Name = "labelTransactionComment";
            labelTransactionComment.Size = new System.Drawing.Size(64, 20);
            labelTransactionComment.TabIndex = 17;
            labelTransactionComment.Text = "Comment:";
            labelTransactionComment.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxTransactionComment
            // 
            tableLayoutPanelCollection.SetColumnSpan(textBoxTransactionComment, 12);
            textBoxTransactionComment.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionBindingSource, "TransactionComment", true));
            textBoxTransactionComment.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxTransactionComment.Location = new System.Drawing.Point(67, 258);
            textBoxTransactionComment.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            textBoxTransactionComment.Name = "textBoxTransactionComment";
            textBoxTransactionComment.Size = new System.Drawing.Size(769, 23);
            textBoxTransactionComment.TabIndex = 18;
            // 
            // labelInternalNotes
            // 
            labelInternalNotes.AccessibleName = "Transaction.InternalNotes";
            labelInternalNotes.AutoSize = true;
            labelInternalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            labelInternalNotes.Location = new System.Drawing.Point(3, 337);
            labelInternalNotes.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            labelInternalNotes.Name = "labelInternalNotes";
            labelInternalNotes.Size = new System.Drawing.Size(64, 70);
            labelInternalNotes.TabIndex = 19;
            labelInternalNotes.Text = "Int. notes:";
            labelInternalNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxInternalNotes
            // 
            tableLayoutPanelCollection.SetColumnSpan(textBoxInternalNotes, 12);
            textBoxInternalNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionBindingSource, "InternalNotes", true));
            textBoxInternalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxInternalNotes.Location = new System.Drawing.Point(67, 334);
            textBoxInternalNotes.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            textBoxInternalNotes.Multiline = true;
            textBoxInternalNotes.Name = "textBoxInternalNotes";
            textBoxInternalNotes.Size = new System.Drawing.Size(769, 70);
            textBoxInternalNotes.TabIndex = 20;
            // 
            // labelResponsible
            // 
            labelResponsible.AccessibleName = "Transaction.ResponsibleName";
            labelResponsible.AutoSize = true;
            labelResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            labelResponsible.Location = new System.Drawing.Point(3, 311);
            labelResponsible.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            labelResponsible.Name = "labelResponsible";
            labelResponsible.Size = new System.Drawing.Size(64, 20);
            labelResponsible.TabIndex = 21;
            labelResponsible.Text = "Respons.:";
            labelResponsible.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // userControlModuleRelatedEntryResponsible
            // 
            userControlModuleRelatedEntryResponsible.CanDeleteConnectionToModule = true;
            tableLayoutPanelCollection.SetColumnSpan(userControlModuleRelatedEntryResponsible, 12);
            userControlModuleRelatedEntryResponsible.DependsOnUri = "";
            userControlModuleRelatedEntryResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleRelatedEntryResponsible.Domain = "";
            userControlModuleRelatedEntryResponsible.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryResponsible.Location = new System.Drawing.Point(67, 308);
            userControlModuleRelatedEntryResponsible.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            userControlModuleRelatedEntryResponsible.Module = null;
            userControlModuleRelatedEntryResponsible.Name = "userControlModuleRelatedEntryResponsible";
            userControlModuleRelatedEntryResponsible.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryResponsible.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryResponsible.ShowInfo = false;
            userControlModuleRelatedEntryResponsible.Size = new System.Drawing.Size(769, 23);
            userControlModuleRelatedEntryResponsible.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryResponsible.TabIndex = 22;
            // 
            // comboBoxTransactionCommentAdd
            // 
            comboBoxTransactionCommentAdd.BackColor = System.Drawing.SystemColors.Control;
            tableLayoutPanelCollection.SetColumnSpan(comboBoxTransactionCommentAdd, 7);
            comboBoxTransactionCommentAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxTransactionCommentAdd.FormattingEnabled = true;
            comboBoxTransactionCommentAdd.Location = new System.Drawing.Point(263, 282);
            comboBoxTransactionCommentAdd.Margin = new System.Windows.Forms.Padding(0, 1, 3, 0);
            comboBoxTransactionCommentAdd.MaxDropDownItems = 20;
            comboBoxTransactionCommentAdd.Name = "comboBoxTransactionCommentAdd";
            comboBoxTransactionCommentAdd.Size = new System.Drawing.Size(573, 23);
            comboBoxTransactionCommentAdd.TabIndex = 26;
            toolTip.SetToolTip(comboBoxTransactionCommentAdd, "Select a comment from all comments in the administrated collection");
            comboBoxTransactionCommentAdd.DropDown += comboBoxTransactionCommentAdd_DropDown;
            // 
            // buttonTransactionCommentInsertSelected
            // 
            buttonTransactionCommentInsertSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonTransactionCommentInsertSelected.Image = Resource.ArrowUp;
            buttonTransactionCommentInsertSelected.Location = new System.Drawing.Point(239, 282);
            buttonTransactionCommentInsertSelected.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            buttonTransactionCommentInsertSelected.Name = "buttonTransactionCommentInsertSelected";
            buttonTransactionCommentInsertSelected.Padding = new System.Windows.Forms.Padding(0, 0, 1, 0);
            buttonTransactionCommentInsertSelected.Size = new System.Drawing.Size(24, 23);
            buttonTransactionCommentInsertSelected.TabIndex = 27;
            toolTip.SetToolTip(buttonTransactionCommentInsertSelected, "Copy the selected phrase into the comment");
            buttonTransactionCommentInsertSelected.UseVisualStyleBackColor = true;
            buttonTransactionCommentInsertSelected.Click += buttonTransactionCommentInsertSelected_Click;
            // 
            // labelTransactionType
            // 
            labelTransactionType.AccessibleName = "Transaction.TransactionType";
            labelTransactionType.AutoSize = true;
            labelTransactionType.Dock = System.Windows.Forms.DockStyle.Fill;
            labelTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            labelTransactionType.Location = new System.Drawing.Point(3, 0);
            labelTransactionType.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            labelTransactionType.Name = "labelTransactionType";
            labelTransactionType.Size = new System.Drawing.Size(64, 24);
            labelTransactionType.TabIndex = 31;
            labelTransactionType.Text = "Type:";
            labelTransactionType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxTransactionType
            // 
            comboBoxTransactionType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            comboBoxTransactionType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            comboBoxTransactionType.BackColor = System.Drawing.SystemColors.Window;
            tableLayoutPanelCollection.SetColumnSpan(comboBoxTransactionType, 8);
            comboBoxTransactionType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", transactionBindingSource, "TransactionType", true));
            comboBoxTransactionType.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxTransactionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            comboBoxTransactionType.FormattingEnabled = true;
            comboBoxTransactionType.Location = new System.Drawing.Point(67, 3);
            comboBoxTransactionType.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            comboBoxTransactionType.MaxDropDownItems = 20;
            comboBoxTransactionType.Name = "comboBoxTransactionType";
            comboBoxTransactionType.Size = new System.Drawing.Size(317, 21);
            comboBoxTransactionType.TabIndex = 32;
            comboBoxTransactionType.SelectedIndexChanged += comboBoxTransactionType_SelectedIndexChanged;
            comboBoxTransactionType.SelectionChangeCommitted += comboBoxTransactionType_SelectionChangeCommitted;
            // 
            // labelMaterialCategory
            // 
            labelMaterialCategory.AccessibleName = "Transaction.MaterialCategory";
            labelMaterialCategory.AutoSize = true;
            labelMaterialCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            labelMaterialCategory.Location = new System.Drawing.Point(3, 53);
            labelMaterialCategory.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            labelMaterialCategory.Name = "labelMaterialCategory";
            labelMaterialCategory.Size = new System.Drawing.Size(64, 29);
            labelMaterialCategory.TabIndex = 37;
            labelMaterialCategory.Text = "Material:";
            labelMaterialCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxMaterialCategory
            // 
            tableLayoutPanelCollection.SetColumnSpan(comboBoxMaterialCategory, 8);
            comboBoxMaterialCategory.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", transactionBindingSource, "MaterialCategory", true));
            comboBoxMaterialCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxMaterialCategory.FormattingEnabled = true;
            comboBoxMaterialCategory.Location = new System.Drawing.Point(67, 56);
            comboBoxMaterialCategory.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            comboBoxMaterialCategory.MaxDropDownItems = 20;
            comboBoxMaterialCategory.Name = "comboBoxMaterialCategory";
            comboBoxMaterialCategory.Size = new System.Drawing.Size(317, 23);
            comboBoxMaterialCategory.TabIndex = 38;
            comboBoxMaterialCategory.TextChanged += comboBoxMaterialCategory_TextChanged;
            // 
            // labelAgreedEndDate
            // 
            labelAgreedEndDate.AccessibleName = "Transaction.AgreedEndDate";
            labelAgreedEndDate.AutoSize = true;
            labelAgreedEndDate.Dock = System.Windows.Forms.DockStyle.Fill;
            labelAgreedEndDate.Location = new System.Drawing.Point(3, 231);
            labelAgreedEndDate.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            labelAgreedEndDate.Name = "labelAgreedEndDate";
            labelAgreedEndDate.Size = new System.Drawing.Size(64, 24);
            labelAgreedEndDate.TabIndex = 13;
            labelAgreedEndDate.Text = "End:";
            labelAgreedEndDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePickerAgreedEndDate
            // 
            dateTimePickerAgreedEndDate.CustomFormat = "yyyy.MM.dd";
            dateTimePickerAgreedEndDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", transactionBindingSource, "AgreedEndDate", true));
            dateTimePickerAgreedEndDate.Dock = System.Windows.Forms.DockStyle.Fill;
            dateTimePickerAgreedEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dateTimePickerAgreedEndDate.Location = new System.Drawing.Point(103, 231);
            dateTimePickerAgreedEndDate.Margin = new System.Windows.Forms.Padding(0);
            dateTimePickerAgreedEndDate.MinimumSize = new System.Drawing.Size(74, 0);
            dateTimePickerAgreedEndDate.Name = "dateTimePickerAgreedEndDate";
            dateTimePickerAgreedEndDate.Size = new System.Drawing.Size(88, 23);
            dateTimePickerAgreedEndDate.TabIndex = 16;
            dateTimePickerAgreedEndDate.CloseUp += dateTimePickerAgreedEndDate_CloseUp;
            // 
            // labelAdministratingCollection
            // 
            labelAdministratingCollection.AccessibleName = "Transaction.AdministratingCollectionID";
            labelAdministratingCollection.AutoSize = true;
            labelAdministratingCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            labelAdministratingCollection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            labelAdministratingCollection.Location = new System.Drawing.Point(390, 0);
            labelAdministratingCollection.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            labelAdministratingCollection.Name = "labelAdministratingCollection";
            labelAdministratingCollection.Size = new System.Drawing.Size(79, 24);
            labelAdministratingCollection.TabIndex = 49;
            labelAdministratingCollection.Text = "Admin. coll.:";
            labelAdministratingCollection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxAdministratingCollection
            // 
            comboBoxAdministratingCollection.BackColor = System.Drawing.SystemColors.Window;
            tableLayoutPanelCollection.SetColumnSpan(comboBoxAdministratingCollection, 3);
            comboBoxAdministratingCollection.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", transactionBindingSource, "AdministratingCollectionID", true));
            comboBoxAdministratingCollection.DataSource = dataSetTransaction;
            comboBoxAdministratingCollection.DisplayMember = "CuratorCollectionList.CollectionName";
            comboBoxAdministratingCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxAdministratingCollection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxAdministratingCollection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            comboBoxAdministratingCollection.FormattingEnabled = true;
            comboBoxAdministratingCollection.Location = new System.Drawing.Point(469, 3);
            comboBoxAdministratingCollection.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            comboBoxAdministratingCollection.MaxDropDownItems = 20;
            comboBoxAdministratingCollection.Name = "comboBoxAdministratingCollection";
            comboBoxAdministratingCollection.Size = new System.Drawing.Size(367, 21);
            comboBoxAdministratingCollection.TabIndex = 50;
            comboBoxAdministratingCollection.ValueMember = "CuratorCollectionList.CollectionID";
            comboBoxAdministratingCollection.SelectedIndexChanged += comboBoxAdministratingCollection_SelectedIndexChanged;
            comboBoxAdministratingCollection.SelectionChangeCommitted += comboBoxAdministratingCollection_SelectionChangeCommitted;
            comboBoxAdministratingCollection.TextChanged += comboBoxAdministratingCollection_TextChanged;
            // 
            // labelActualEndDate
            // 
            labelActualEndDate.AccessibleName = "Transaction.ActualEndDate";
            labelActualEndDate.AutoSize = true;
            labelActualEndDate.Dock = System.Windows.Forms.DockStyle.Fill;
            labelActualEndDate.Location = new System.Drawing.Point(191, 231);
            labelActualEndDate.Margin = new System.Windows.Forms.Padding(0);
            labelActualEndDate.Name = "labelActualEndDate";
            labelActualEndDate.Size = new System.Drawing.Size(48, 24);
            labelActualEndDate.TabIndex = 29;
            labelActualEndDate.Text = "Prol.:";
            labelActualEndDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePickerActualEndDate
            // 
            tableLayoutPanelCollection.SetColumnSpan(dateTimePickerActualEndDate, 2);
            dateTimePickerActualEndDate.CustomFormat = "yyyy.MM.dd";
            dateTimePickerActualEndDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", transactionBindingSource, "ActualEndDate", true));
            dateTimePickerActualEndDate.Dock = System.Windows.Forms.DockStyle.Fill;
            dateTimePickerActualEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dateTimePickerActualEndDate.Location = new System.Drawing.Point(275, 231);
            dateTimePickerActualEndDate.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            dateTimePickerActualEndDate.MinimumSize = new System.Drawing.Size(74, 0);
            dateTimePickerActualEndDate.Name = "dateTimePickerActualEndDate";
            dateTimePickerActualEndDate.Size = new System.Drawing.Size(109, 23);
            dateTimePickerActualEndDate.TabIndex = 48;
            dateTimePickerActualEndDate.CloseUp += dateTimePickerActualEndDate_CloseUp;
            // 
            // textBoxInvestigator
            // 
            tableLayoutPanelCollection.SetColumnSpan(textBoxInvestigator, 3);
            textBoxInvestigator.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionBindingSource, "Investigator", true));
            textBoxInvestigator.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxInvestigator.Location = new System.Drawing.Point(469, 232);
            textBoxInvestigator.Margin = new System.Windows.Forms.Padding(0, 1, 3, 0);
            textBoxInvestigator.MaxLength = 200;
            textBoxInvestigator.Name = "textBoxInvestigator";
            textBoxInvestigator.Size = new System.Drawing.Size(367, 23);
            textBoxInvestigator.TabIndex = 25;
            // 
            // labelInvestigator
            // 
            labelInvestigator.AccessibleName = "Transaction.Investigator";
            labelInvestigator.AutoSize = true;
            labelInvestigator.Dock = System.Windows.Forms.DockStyle.Fill;
            labelInvestigator.Location = new System.Drawing.Point(390, 231);
            labelInvestigator.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            labelInvestigator.Name = "labelInvestigator";
            labelInvestigator.Size = new System.Drawing.Size(79, 24);
            labelInvestigator.TabIndex = 24;
            labelInvestigator.Text = "Investigator:";
            labelInvestigator.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelReportingCategory
            // 
            labelReportingCategory.AutoSize = true;
            labelReportingCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            labelReportingCategory.Location = new System.Drawing.Point(390, 202);
            labelReportingCategory.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            labelReportingCategory.Name = "labelReportingCategory";
            labelReportingCategory.Size = new System.Drawing.Size(79, 29);
            labelReportingCategory.TabIndex = 33;
            labelReportingCategory.Text = "Categ.:";
            labelReportingCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxReportingCategory
            // 
            comboBoxReportingCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            comboBoxReportingCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            tableLayoutPanelCollection.SetColumnSpan(comboBoxReportingCategory, 3);
            comboBoxReportingCategory.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionBindingSource, "ReportingCategory", true));
            comboBoxReportingCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxReportingCategory.FormattingEnabled = true;
            comboBoxReportingCategory.Location = new System.Drawing.Point(469, 205);
            comboBoxReportingCategory.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            comboBoxReportingCategory.MaxDropDownItems = 20;
            comboBoxReportingCategory.Name = "comboBoxReportingCategory";
            comboBoxReportingCategory.Size = new System.Drawing.Size(367, 23);
            comboBoxReportingCategory.TabIndex = 34;
            comboBoxReportingCategory.DropDown += comboBoxReportingCategory_DropDown;
            comboBoxReportingCategory.SelectionChangeCommitted += comboBoxReportingCategory_SelectionChangeCommitted;
            // 
            // labelMaterialDescription
            // 
            labelMaterialDescription.AccessibleName = "Transaction.MaterialDescription";
            labelMaterialDescription.AutoSize = true;
            labelMaterialDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            labelMaterialDescription.Location = new System.Drawing.Point(387, 61);
            labelMaterialDescription.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            labelMaterialDescription.Name = "labelMaterialDescription";
            labelMaterialDescription.Size = new System.Drawing.Size(82, 21);
            labelMaterialDescription.TabIndex = 35;
            labelMaterialDescription.Text = "Mat. descript.:";
            labelMaterialDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxMaterialDescription
            // 
            tableLayoutPanelCollection.SetColumnSpan(textBoxMaterialDescription, 3);
            textBoxMaterialDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionBindingSource, "MaterialDescription", true));
            textBoxMaterialDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxMaterialDescription.Location = new System.Drawing.Point(469, 56);
            textBoxMaterialDescription.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            textBoxMaterialDescription.Name = "textBoxMaterialDescription";
            textBoxMaterialDescription.Size = new System.Drawing.Size(367, 23);
            textBoxMaterialDescription.TabIndex = 36;
            // 
            // labelMaterialCollectors
            // 
            labelMaterialCollectors.AccessibleName = "Transaction.MaterialCollectors";
            labelMaterialCollectors.AutoSize = true;
            labelMaterialCollectors.Dock = System.Windows.Forms.DockStyle.Fill;
            labelMaterialCollectors.Location = new System.Drawing.Point(3, 88);
            labelMaterialCollectors.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            labelMaterialCollectors.Name = "labelMaterialCollectors";
            labelMaterialCollectors.Size = new System.Drawing.Size(64, 20);
            labelMaterialCollectors.TabIndex = 53;
            labelMaterialCollectors.Text = "Mat. coll.:";
            labelMaterialCollectors.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxMaterialCollectors
            // 
            tableLayoutPanelCollection.SetColumnSpan(textBoxMaterialCollectors, 8);
            textBoxMaterialCollectors.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionBindingSource, "MaterialCollectors", true));
            textBoxMaterialCollectors.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxMaterialCollectors.Location = new System.Drawing.Point(67, 82);
            textBoxMaterialCollectors.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            textBoxMaterialCollectors.Name = "textBoxMaterialCollectors";
            textBoxMaterialCollectors.Size = new System.Drawing.Size(317, 23);
            textBoxMaterialCollectors.TabIndex = 54;
            // 
            // comboBoxAgreedEndDateAddMonths
            // 
            comboBoxAgreedEndDateAddMonths.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxAgreedEndDateAddMonths.DropDownWidth = 80;
            comboBoxAgreedEndDateAddMonths.FormattingEnabled = true;
            comboBoxAgreedEndDateAddMonths.Location = new System.Drawing.Point(91, 231);
            comboBoxAgreedEndDateAddMonths.Margin = new System.Windows.Forms.Padding(0);
            comboBoxAgreedEndDateAddMonths.Name = "comboBoxAgreedEndDateAddMonths";
            comboBoxAgreedEndDateAddMonths.Size = new System.Drawing.Size(12, 23);
            comboBoxAgreedEndDateAddMonths.TabIndex = 58;
            toolTip.SetToolTip(comboBoxAgreedEndDateAddMonths, "Add a distinctive time period");
            comboBoxAgreedEndDateAddMonths.SelectionChangeCommitted += comboBoxAgreedEndDateAddMonths_SelectionChangeCommitted;
            // 
            // buttonBeginDateEdit
            // 
            buttonBeginDateEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonBeginDateEdit.Image = Resource.CLOCK;
            buttonBeginDateEdit.Location = new System.Drawing.Point(67, 202);
            buttonBeginDateEdit.Margin = new System.Windows.Forms.Padding(0);
            buttonBeginDateEdit.Name = "buttonBeginDateEdit";
            buttonBeginDateEdit.Size = new System.Drawing.Size(24, 29);
            buttonBeginDateEdit.TabIndex = 59;
            toolTip.SetToolTip(buttonBeginDateEdit, "Set the date");
            buttonBeginDateEdit.UseVisualStyleBackColor = true;
            buttonBeginDateEdit.Click += buttonBeginDateEdit_Click;
            // 
            // buttonActualEndDateEdit
            // 
            buttonActualEndDateEdit.Image = Resource.CLOCK;
            buttonActualEndDateEdit.Location = new System.Drawing.Point(239, 231);
            buttonActualEndDateEdit.Margin = new System.Windows.Forms.Padding(0);
            buttonActualEndDateEdit.Name = "buttonActualEndDateEdit";
            buttonActualEndDateEdit.Size = new System.Drawing.Size(24, 23);
            buttonActualEndDateEdit.TabIndex = 60;
            toolTip.SetToolTip(buttonActualEndDateEdit, "Set the prolongation date");
            buttonActualEndDateEdit.UseVisualStyleBackColor = true;
            buttonActualEndDateEdit.Click += buttonActualEndDateEdit_Click;
            // 
            // buttonAgreedEndDateEdit
            // 
            buttonAgreedEndDateEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonAgreedEndDateEdit.Image = Resource.CLOCK;
            buttonAgreedEndDateEdit.Location = new System.Drawing.Point(67, 231);
            buttonAgreedEndDateEdit.Margin = new System.Windows.Forms.Padding(0);
            buttonAgreedEndDateEdit.Name = "buttonAgreedEndDateEdit";
            buttonAgreedEndDateEdit.Size = new System.Drawing.Size(24, 24);
            buttonAgreedEndDateEdit.TabIndex = 61;
            toolTip.SetToolTip(buttonAgreedEndDateEdit, "Set the end date");
            buttonAgreedEndDateEdit.UseVisualStyleBackColor = true;
            buttonAgreedEndDateEdit.Click += buttonAgreedEndDateEdit_Click;
            // 
            // comboBoxActualEndDateAddMonths
            // 
            comboBoxActualEndDateAddMonths.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxActualEndDateAddMonths.DropDownWidth = 80;
            comboBoxActualEndDateAddMonths.FormattingEnabled = true;
            comboBoxActualEndDateAddMonths.Location = new System.Drawing.Point(263, 231);
            comboBoxActualEndDateAddMonths.Margin = new System.Windows.Forms.Padding(0);
            comboBoxActualEndDateAddMonths.Name = "comboBoxActualEndDateAddMonths";
            comboBoxActualEndDateAddMonths.Size = new System.Drawing.Size(12, 23);
            comboBoxActualEndDateAddMonths.TabIndex = 62;
            toolTip.SetToolTip(comboBoxActualEndDateAddMonths, "Add a distinctive time period as prolongation");
            comboBoxActualEndDateAddMonths.SelectionChangeCommitted += comboBoxActualEndDateAddMonths_SelectionChangeCommitted;
            // 
            // labelCommentPhrases
            // 
            labelCommentPhrases.AccessibleName = "TransactionComment";
            labelCommentPhrases.AutoSize = true;
            tableLayoutPanelCollection.SetColumnSpan(labelCommentPhrases, 5);
            labelCommentPhrases.Dock = System.Windows.Forms.DockStyle.Right;
            labelCommentPhrases.Location = new System.Drawing.Point(29, 281);
            labelCommentPhrases.Name = "labelCommentPhrases";
            labelCommentPhrases.Size = new System.Drawing.Size(207, 24);
            labelCommentPhrases.TabIndex = 63;
            labelCommentPhrases.Text = "List of standard phrases for comment:";
            labelCommentPhrases.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBoxFrom
            // 
            tableLayoutPanelCollection.SetColumnSpan(groupBoxFrom, 9);
            groupBoxFrom.Controls.Add(tableLayoutPanelFrom);
            groupBoxFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            groupBoxFrom.Location = new System.Drawing.Point(3, 111);
            groupBoxFrom.Name = "groupBoxFrom";
            groupBoxFrom.Size = new System.Drawing.Size(381, 88);
            groupBoxFrom.TabIndex = 64;
            groupBoxFrom.TabStop = false;
            groupBoxFrom.Text = "From";
            // 
            // tableLayoutPanelFrom
            // 
            tableLayoutPanelFrom.ColumnCount = 3;
            tableLayoutPanelFrom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelFrom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelFrom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelFrom.Controls.Add(comboBoxFromCollection, 1, 2);
            tableLayoutPanelFrom.Controls.Add(textBoxFromTransactionNumber, 1, 1);
            tableLayoutPanelFrom.Controls.Add(labelFromTransactionNumber, 0, 1);
            tableLayoutPanelFrom.Controls.Add(labelFromCollection, 0, 2);
            tableLayoutPanelFrom.Controls.Add(userControlHierarchySelectorFromCollection, 2, 2);
            tableLayoutPanelFrom.Controls.Add(userControlModuleRelatedEntryFromPartner, 0, 0);
            tableLayoutPanelFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            tableLayoutPanelFrom.Location = new System.Drawing.Point(3, 16);
            tableLayoutPanelFrom.Name = "tableLayoutPanelFrom";
            tableLayoutPanelFrom.RowCount = 3;
            tableLayoutPanelFrom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelFrom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelFrom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelFrom.Size = new System.Drawing.Size(375, 69);
            tableLayoutPanelFrom.TabIndex = 0;
            // 
            // comboBoxFromCollection
            // 
            comboBoxFromCollection.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", transactionBindingSource, "FromCollectionID", true));
            comboBoxFromCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxFromCollection.FormattingEnabled = true;
            comboBoxFromCollection.Location = new System.Drawing.Point(62, 46);
            comboBoxFromCollection.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            comboBoxFromCollection.MaxDropDownItems = 20;
            comboBoxFromCollection.Name = "comboBoxFromCollection";
            comboBoxFromCollection.Size = new System.Drawing.Size(289, 21);
            comboBoxFromCollection.TabIndex = 41;
            comboBoxFromCollection.SelectionChangeCommitted += comboBoxFromCollection_SelectionChangeCommitted;
            comboBoxFromCollection.TextChanged += comboBoxFromCollection_TextChanged;
            // 
            // textBoxFromTransactionNumber
            // 
            tableLayoutPanelFrom.SetColumnSpan(textBoxFromTransactionNumber, 2);
            textBoxFromTransactionNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionBindingSource, "FromTransactionNumber", true));
            textBoxFromTransactionNumber.Dock = System.Windows.Forms.DockStyle.Left;
            textBoxFromTransactionNumber.Location = new System.Drawing.Point(62, 23);
            textBoxFromTransactionNumber.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            textBoxFromTransactionNumber.MaxLength = 50;
            textBoxFromTransactionNumber.Name = "textBoxFromTransactionNumber";
            textBoxFromTransactionNumber.Size = new System.Drawing.Size(101, 20);
            textBoxFromTransactionNumber.TabIndex = 45;
            textBoxFromTransactionNumber.TextChanged += textBoxFromTransactionNumber_TextChanged;
            // 
            // labelFromTransactionNumber
            // 
            labelFromTransactionNumber.AccessibleName = "Transaction.FromTransactionNumber";
            labelFromTransactionNumber.AutoSize = true;
            labelFromTransactionNumber.Dock = System.Windows.Forms.DockStyle.Top;
            labelFromTransactionNumber.Location = new System.Drawing.Point(3, 26);
            labelFromTransactionNumber.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            labelFromTransactionNumber.Name = "labelFromTransactionNumber";
            labelFromTransactionNumber.Size = new System.Drawing.Size(56, 13);
            labelFromTransactionNumber.TabIndex = 56;
            labelFromTransactionNumber.Text = "Number:";
            labelFromTransactionNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelFromCollection
            // 
            labelFromCollection.AccessibleName = "Transaction.FromCollectionID";
            labelFromCollection.AutoSize = true;
            labelFromCollection.Dock = System.Windows.Forms.DockStyle.Top;
            labelFromCollection.Location = new System.Drawing.Point(3, 49);
            labelFromCollection.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            labelFromCollection.Name = "labelFromCollection";
            labelFromCollection.Size = new System.Drawing.Size(56, 13);
            labelFromCollection.TabIndex = 57;
            labelFromCollection.Text = "Collection:";
            labelFromCollection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlHierarchySelectorFromCollection
            // 
            userControlHierarchySelectorFromCollection.Dock = System.Windows.Forms.DockStyle.Top;
            userControlHierarchySelectorFromCollection.Location = new System.Drawing.Point(351, 46);
            userControlHierarchySelectorFromCollection.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            userControlHierarchySelectorFromCollection.Name = "userControlHierarchySelectorFromCollection";
            userControlHierarchySelectorFromCollection.Size = new System.Drawing.Size(21, 20);
            userControlHierarchySelectorFromCollection.TabIndex = 55;
            userControlHierarchySelectorFromCollection.Click += userControlHierarchySelectorFromCollection_Click;
            // 
            // userControlModuleRelatedEntryFromPartner
            // 
            userControlModuleRelatedEntryFromPartner.CanDeleteConnectionToModule = true;
            tableLayoutPanelFrom.SetColumnSpan(userControlModuleRelatedEntryFromPartner, 3);
            userControlModuleRelatedEntryFromPartner.DependsOnUri = "";
            userControlModuleRelatedEntryFromPartner.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleRelatedEntryFromPartner.Domain = "";
            userControlModuleRelatedEntryFromPartner.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryFromPartner.Location = new System.Drawing.Point(0, 0);
            userControlModuleRelatedEntryFromPartner.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            userControlModuleRelatedEntryFromPartner.Module = null;
            userControlModuleRelatedEntryFromPartner.Name = "userControlModuleRelatedEntryFromPartner";
            userControlModuleRelatedEntryFromPartner.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryFromPartner.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryFromPartner.ShowInfo = false;
            userControlModuleRelatedEntryFromPartner.Size = new System.Drawing.Size(372, 23);
            userControlModuleRelatedEntryFromPartner.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryFromPartner.TabIndex = 12;
            // 
            // groupBoxTo
            // 
            tableLayoutPanelCollection.SetColumnSpan(groupBoxTo, 4);
            groupBoxTo.Controls.Add(tableLayoutPanelTo);
            groupBoxTo.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            groupBoxTo.Location = new System.Drawing.Point(390, 111);
            groupBoxTo.Name = "groupBoxTo";
            groupBoxTo.Size = new System.Drawing.Size(446, 88);
            groupBoxTo.TabIndex = 65;
            groupBoxTo.TabStop = false;
            groupBoxTo.Text = "To";
            // 
            // tableLayoutPanelTo
            // 
            tableLayoutPanelTo.ColumnCount = 5;
            tableLayoutPanelTo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelTo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelTo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelTo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelTo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelTo.Controls.Add(userControlModuleRelatedEntryToPartner, 0, 0);
            tableLayoutPanelTo.Controls.Add(textBoxToTransactionNumber, 3, 1);
            tableLayoutPanelTo.Controls.Add(labelToTransactionNumber, 2, 1);
            tableLayoutPanelTo.Controls.Add(comboBoxToCollection, 1, 2);
            tableLayoutPanelTo.Controls.Add(userControlHierarchySelectorToCollection, 4, 2);
            tableLayoutPanelTo.Controls.Add(labelToCollection, 0, 2);
            tableLayoutPanelTo.Controls.Add(labelToRecipient, 0, 1);
            tableLayoutPanelTo.Controls.Add(comboBoxToRecipient, 1, 1);
            tableLayoutPanelTo.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            tableLayoutPanelTo.Location = new System.Drawing.Point(3, 16);
            tableLayoutPanelTo.Name = "tableLayoutPanelTo";
            tableLayoutPanelTo.RowCount = 3;
            tableLayoutPanelTo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelTo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelTo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelTo.Size = new System.Drawing.Size(440, 69);
            tableLayoutPanelTo.TabIndex = 0;
            // 
            // userControlModuleRelatedEntryToPartner
            // 
            userControlModuleRelatedEntryToPartner.CanDeleteConnectionToModule = true;
            tableLayoutPanelTo.SetColumnSpan(userControlModuleRelatedEntryToPartner, 5);
            userControlModuleRelatedEntryToPartner.DependsOnUri = "";
            userControlModuleRelatedEntryToPartner.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleRelatedEntryToPartner.Domain = "";
            userControlModuleRelatedEntryToPartner.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryToPartner.Location = new System.Drawing.Point(0, 0);
            userControlModuleRelatedEntryToPartner.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            userControlModuleRelatedEntryToPartner.Module = null;
            userControlModuleRelatedEntryToPartner.Name = "userControlModuleRelatedEntryToPartner";
            userControlModuleRelatedEntryToPartner.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryToPartner.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryToPartner.ShowInfo = false;
            userControlModuleRelatedEntryToPartner.Size = new System.Drawing.Size(437, 23);
            userControlModuleRelatedEntryToPartner.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryToPartner.TabIndex = 39;
            // 
            // textBoxToTransactionNumber
            // 
            tableLayoutPanelTo.SetColumnSpan(textBoxToTransactionNumber, 2);
            textBoxToTransactionNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionBindingSource, "ToTransactionNumber", true));
            textBoxToTransactionNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxToTransactionNumber.Location = new System.Drawing.Point(335, 23);
            textBoxToTransactionNumber.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            textBoxToTransactionNumber.MaxLength = 50;
            textBoxToTransactionNumber.Name = "textBoxToTransactionNumber";
            textBoxToTransactionNumber.Size = new System.Drawing.Size(102, 20);
            textBoxToTransactionNumber.TabIndex = 46;
            // 
            // labelToTransactionNumber
            // 
            labelToTransactionNumber.AccessibleName = "Transaction.ToTransactionNumber";
            labelToTransactionNumber.AutoSize = true;
            labelToTransactionNumber.Dock = System.Windows.Forms.DockStyle.Top;
            labelToTransactionNumber.Location = new System.Drawing.Point(285, 26);
            labelToTransactionNumber.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            labelToTransactionNumber.Name = "labelToTransactionNumber";
            labelToTransactionNumber.Size = new System.Drawing.Size(47, 13);
            labelToTransactionNumber.TabIndex = 47;
            labelToTransactionNumber.Text = "Number:";
            labelToTransactionNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxToCollection
            // 
            tableLayoutPanelTo.SetColumnSpan(comboBoxToCollection, 3);
            comboBoxToCollection.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", transactionBindingSource, "ToCollectionID", true));
            comboBoxToCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxToCollection.FormattingEnabled = true;
            comboBoxToCollection.Location = new System.Drawing.Point(62, 46);
            comboBoxToCollection.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            comboBoxToCollection.MaxDropDownItems = 20;
            comboBoxToCollection.Name = "comboBoxToCollection";
            comboBoxToCollection.Size = new System.Drawing.Size(354, 21);
            comboBoxToCollection.TabIndex = 42;
            comboBoxToCollection.SelectionChangeCommitted += comboBoxToCollection_SelectionChangeCommitted;
            comboBoxToCollection.TextChanged += comboBoxToCollection_TextChanged;
            // 
            // userControlHierarchySelectorToCollection
            // 
            userControlHierarchySelectorToCollection.Dock = System.Windows.Forms.DockStyle.Top;
            userControlHierarchySelectorToCollection.Location = new System.Drawing.Point(416, 46);
            userControlHierarchySelectorToCollection.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            userControlHierarchySelectorToCollection.Name = "userControlHierarchySelectorToCollection";
            userControlHierarchySelectorToCollection.Size = new System.Drawing.Size(21, 18);
            userControlHierarchySelectorToCollection.TabIndex = 56;
            userControlHierarchySelectorToCollection.Click += userControlHierarchySelectorToCollection_Click;
            // 
            // labelToCollection
            // 
            labelToCollection.AccessibleName = "Transaction.ToCollectionID";
            labelToCollection.AutoSize = true;
            labelToCollection.Dock = System.Windows.Forms.DockStyle.Top;
            labelToCollection.Location = new System.Drawing.Point(3, 49);
            labelToCollection.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            labelToCollection.Name = "labelToCollection";
            labelToCollection.Size = new System.Drawing.Size(56, 13);
            labelToCollection.TabIndex = 57;
            labelToCollection.Text = "Collection:";
            labelToCollection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelToRecipient
            // 
            labelToRecipient.AccessibleName = "Transaction.ToRecipient";
            labelToRecipient.AutoSize = true;
            labelToRecipient.Dock = System.Windows.Forms.DockStyle.Top;
            labelToRecipient.Location = new System.Drawing.Point(3, 26);
            labelToRecipient.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            labelToRecipient.Name = "labelToRecipient";
            labelToRecipient.Size = new System.Drawing.Size(56, 13);
            labelToRecipient.TabIndex = 58;
            labelToRecipient.Text = "Recipient:";
            // 
            // comboBoxToRecipient
            // 
            comboBoxToRecipient.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionBindingSource, "ToRecipient", true));
            comboBoxToRecipient.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxToRecipient.FormattingEnabled = true;
            comboBoxToRecipient.Location = new System.Drawing.Point(62, 23);
            comboBoxToRecipient.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            comboBoxToRecipient.Name = "comboBoxToRecipient";
            comboBoxToRecipient.Size = new System.Drawing.Size(217, 21);
            comboBoxToRecipient.TabIndex = 59;
            comboBoxToRecipient.DropDown += comboBoxToRecipient_DropDown;
            // 
            // comboBoxMaterialSource
            // 
            comboBoxMaterialSource.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionBindingSource, "MaterialSource", true));
            comboBoxMaterialSource.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxMaterialSource.FormattingEnabled = true;
            comboBoxMaterialSource.Location = new System.Drawing.Point(469, 82);
            comboBoxMaterialSource.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            comboBoxMaterialSource.Name = "comboBoxMaterialSource";
            comboBoxMaterialSource.Size = new System.Drawing.Size(261, 23);
            comboBoxMaterialSource.TabIndex = 66;
            comboBoxMaterialSource.DropDown += comboBoxMaterialSource_DropDown;
            // 
            // labelMaterialSource
            // 
            labelMaterialSource.AutoSize = true;
            labelMaterialSource.Dock = System.Windows.Forms.DockStyle.Fill;
            labelMaterialSource.Location = new System.Drawing.Point(390, 82);
            labelMaterialSource.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            labelMaterialSource.Name = "labelMaterialSource";
            labelMaterialSource.Size = new System.Drawing.Size(79, 26);
            labelMaterialSource.TabIndex = 67;
            labelMaterialSource.Text = "Mat. source:";
            labelMaterialSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxNumberOfUnits
            // 
            textBoxNumberOfUnits.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionBindingSource, "NumberOfUnits", true));
            textBoxNumberOfUnits.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxNumberOfUnits.Location = new System.Drawing.Point(773, 82);
            textBoxNumberOfUnits.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            textBoxNumberOfUnits.Name = "textBoxNumberOfUnits";
            textBoxNumberOfUnits.Size = new System.Drawing.Size(63, 23);
            textBoxNumberOfUnits.TabIndex = 7;
            textBoxNumberOfUnits.TextChanged += textBoxNumberOfUnits_TextChanged;
            textBoxNumberOfUnits.Validating += textBoxNumberOfUnits_Validating;
            // 
            // labelNumberOfUnits
            // 
            labelNumberOfUnits.AccessibleName = "Transaction.NumberOfUnits";
            labelNumberOfUnits.AutoSize = true;
            labelNumberOfUnits.Dock = System.Windows.Forms.DockStyle.Fill;
            labelNumberOfUnits.Location = new System.Drawing.Point(736, 82);
            labelNumberOfUnits.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            labelNumberOfUnits.Name = "labelNumberOfUnits";
            labelNumberOfUnits.Size = new System.Drawing.Size(37, 26);
            labelNumberOfUnits.TabIndex = 6;
            labelNumberOfUnits.Text = "Units:";
            labelNumberOfUnits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelDateSupplement
            // 
            labelDateSupplement.AutoSize = true;
            labelDateSupplement.Dock = System.Windows.Forms.DockStyle.Fill;
            labelDateSupplement.Location = new System.Drawing.Point(194, 202);
            labelDateSupplement.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            labelDateSupplement.Name = "labelDateSupplement";
            labelDateSupplement.Size = new System.Drawing.Size(45, 29);
            labelDateSupplement.TabIndex = 68;
            labelDateSupplement.Text = "Suppl.:";
            labelDateSupplement.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxDateSupplement
            // 
            tableLayoutPanelCollection.SetColumnSpan(textBoxDateSupplement, 4);
            textBoxDateSupplement.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionBindingSource, "DateSupplement", true));
            textBoxDateSupplement.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxDateSupplement.Location = new System.Drawing.Point(239, 205);
            textBoxDateSupplement.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            textBoxDateSupplement.Name = "textBoxDateSupplement";
            textBoxDateSupplement.Size = new System.Drawing.Size(145, 23);
            textBoxDateSupplement.TabIndex = 69;
            // 
            // labelToTransactionInventoryNumber
            // 
            labelToTransactionInventoryNumber.AutoSize = true;
            labelToTransactionInventoryNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            labelToTransactionInventoryNumber.Location = new System.Drawing.Point(736, 24);
            labelToTransactionInventoryNumber.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            labelToTransactionInventoryNumber.Name = "labelToTransactionInventoryNumber";
            labelToTransactionInventoryNumber.Size = new System.Drawing.Size(37, 29);
            labelToTransactionInventoryNumber.TabIndex = 70;
            labelToTransactionInventoryNumber.Text = "No.:";
            labelToTransactionInventoryNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxToTransactionInventoryNumber
            // 
            textBoxToTransactionInventoryNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionBindingSource, "ToTransactionNumber", true));
            textBoxToTransactionInventoryNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxToTransactionInventoryNumber.Location = new System.Drawing.Point(773, 27);
            textBoxToTransactionInventoryNumber.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            textBoxToTransactionInventoryNumber.MaxLength = 50;
            textBoxToTransactionInventoryNumber.Name = "textBoxToTransactionInventoryNumber";
            textBoxToTransactionInventoryNumber.Size = new System.Drawing.Size(63, 23);
            textBoxToTransactionInventoryNumber.TabIndex = 71;
            // 
            // tabPageSending
            // 
            tabPageSending.Controls.Add(splitContainerSending);
            helpProvider.SetHelpKeyword(tabPageSending, "Transaction sending");
            helpProvider.SetHelpNavigator(tabPageSending, System.Windows.Forms.HelpNavigator.KeywordIndex);
            tabPageSending.ImageIndex = 1;
            tabPageSending.Location = new System.Drawing.Point(4, 24);
            tabPageSending.Name = "tabPageSending";
            helpProvider.SetShowHelp(tabPageSending, true);
            tabPageSending.Size = new System.Drawing.Size(839, 407);
            tabPageSending.TabIndex = 1;
            tabPageSending.Text = "Sending";
            tabPageSending.UseVisualStyleBackColor = true;
            // 
            // splitContainerSending
            // 
            splitContainerSending.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(splitContainerSending, "Transaction sending");
            helpProvider.SetHelpNavigator(splitContainerSending, System.Windows.Forms.HelpNavigator.KeywordIndex);
            splitContainerSending.Location = new System.Drawing.Point(0, 0);
            splitContainerSending.Name = "splitContainerSending";
            // 
            // splitContainerSending.Panel1
            // 
            splitContainerSending.Panel1.Controls.Add(tableLayoutPanelSending);
            // 
            // splitContainerSending.Panel2
            // 
            splitContainerSending.Panel2.Controls.Add(tableLayoutPanelSendingList);
            helpProvider.SetShowHelp(splitContainerSending, true);
            splitContainerSending.Size = new System.Drawing.Size(839, 407);
            splitContainerSending.SplitterDistance = 696;
            splitContainerSending.TabIndex = 1;
            // 
            // tableLayoutPanelSending
            // 
            tableLayoutPanelSending.ColumnCount = 9;
            tableLayoutPanelSending.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSending.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSending.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSending.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSending.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            tableLayoutPanelSending.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSending.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            tableLayoutPanelSending.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSending.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSending.Controls.Add(labelSendingSchema, 0, 0);
            tableLayoutPanelSending.Controls.Add(textBoxSendingSchema, 1, 0);
            tableLayoutPanelSending.Controls.Add(toolStripSending, 7, 0);
            tableLayoutPanelSending.Controls.Add(checkBoxSendingRestrictToCollection, 3, 1);
            tableLayoutPanelSending.Controls.Add(checkBoxSendingRestrictToMaterial, 6, 1);
            tableLayoutPanelSending.Controls.Add(pictureBoxSendingRestrictToMaterial, 8, 1);
            tableLayoutPanelSending.Controls.Add(pictureBoxSendingRestrictToCollection, 5, 1);
            tableLayoutPanelSending.Controls.Add(checkBoxSendingOrderByTaxa, 0, 1);
            tableLayoutPanelSending.Controls.Add(checkBoxSendingSingleLines, 2, 1);
            tableLayoutPanelSending.Controls.Add(checkBoxSendingIncludeType, 2, 2);
            tableLayoutPanelSending.Controls.Add(labelSendingRestrictToTaxonomicGroup, 4, 2);
            tableLayoutPanelSending.Controls.Add(comboBoxSendingRestrictToTaxonomicGroup, 5, 2);
            tableLayoutPanelSending.Controls.Add(splitContainerWebbrowserSending, 0, 3);
            tableLayoutPanelSending.Controls.Add(checkBoxSendingAllUnits, 0, 2);
            tableLayoutPanelSending.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(tableLayoutPanelSending, "Transaction sending");
            helpProvider.SetHelpNavigator(tableLayoutPanelSending, System.Windows.Forms.HelpNavigator.KeywordIndex);
            tableLayoutPanelSending.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelSending.Name = "tableLayoutPanelSending";
            tableLayoutPanelSending.RowCount = 4;
            tableLayoutPanelSending.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSending.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSending.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanelSending.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelSending.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            helpProvider.SetShowHelp(tableLayoutPanelSending, true);
            tableLayoutPanelSending.Size = new System.Drawing.Size(696, 407);
            tableLayoutPanelSending.TabIndex = 0;
            // 
            // labelSendingSchema
            // 
            labelSendingSchema.AutoSize = true;
            labelSendingSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            labelSendingSchema.Location = new System.Drawing.Point(3, 0);
            labelSendingSchema.Name = "labelSendingSchema";
            labelSendingSchema.Size = new System.Drawing.Size(52, 29);
            labelSendingSchema.TabIndex = 4;
            labelSendingSchema.Text = "Schema:";
            labelSendingSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxSendingSchema
            // 
            tableLayoutPanelSending.SetColumnSpan(textBoxSendingSchema, 6);
            textBoxSendingSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxSendingSchema.Location = new System.Drawing.Point(61, 3);
            textBoxSendingSchema.Name = "textBoxSendingSchema";
            textBoxSendingSchema.Size = new System.Drawing.Size(390, 23);
            textBoxSendingSchema.TabIndex = 5;
            // 
            // toolStripSending
            // 
            toolStripSending.BackColor = System.Drawing.SystemColors.Control;
            tableLayoutPanelSending.SetColumnSpan(toolStripSending, 2);
            toolStripSending.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStripSending.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripSending.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonOpenSendingSchemaFile, toolStripDropDownButtonSendingGitHub, toolStripSeparatorSending1, toolStripButtonSendingShowList, toolStripButtonSendingPreview, toolStripButtonSendingPreviewAllOnLoan, toolStripButtonSendingXslEditor, toolStripButtonSendingPrint, toolStripButtonSendingSave, toolStripButtonSendingScanner });
            toolStripSending.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            toolStripSending.Location = new System.Drawing.Point(454, 0);
            toolStripSending.Name = "toolStripSending";
            toolStripSending.Size = new System.Drawing.Size(242, 29);
            toolStripSending.TabIndex = 6;
            toolStripSending.Text = "toolStrip1";
            // 
            // toolStripButtonOpenSendingSchemaFile
            // 
            toolStripButtonOpenSendingSchemaFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonOpenSendingSchemaFile.Image = Resource.Open;
            toolStripButtonOpenSendingSchemaFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonOpenSendingSchemaFile.Name = "toolStripButtonOpenSendingSchemaFile";
            toolStripButtonOpenSendingSchemaFile.Size = new System.Drawing.Size(23, 20);
            toolStripButtonOpenSendingSchemaFile.Text = "Open the schema file for the covering letter";
            toolStripButtonOpenSendingSchemaFile.Click += toolStripButtonOpenSendingSchemaFile_Click;
            // 
            // toolStripDropDownButtonSendingGitHub
            // 
            toolStripDropDownButtonSendingGitHub.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripDropDownButtonSendingGitHub.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { SendingSnsbToolStripMenuItem });
            toolStripDropDownButtonSendingGitHub.Image = Resource.Github;
            toolStripDropDownButtonSendingGitHub.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButtonSendingGitHub.Name = "toolStripDropDownButtonSendingGitHub";
            toolStripDropDownButtonSendingGitHub.Size = new System.Drawing.Size(29, 20);
            toolStripDropDownButtonSendingGitHub.Text = "toolStripDropDownButton1";
            // 
            // SendingSnsbToolStripMenuItem
            // 
            SendingSnsbToolStripMenuItem.Image = Resource.SNSB;
            SendingSnsbToolStripMenuItem.Name = "SendingSnsbToolStripMenuItem";
            SendingSnsbToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            SendingSnsbToolStripMenuItem.Text = "SNSB";
            SendingSnsbToolStripMenuItem.Click += SendingSnsbToolStripMenuItem_Click;
            // 
            // toolStripSeparatorSending1
            // 
            toolStripSeparatorSending1.Name = "toolStripSeparatorSending1";
            toolStripSeparatorSending1.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonSendingShowList
            // 
            toolStripButtonSendingShowList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSendingShowList.Image = Resource.ShowList;
            toolStripButtonSendingShowList.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSendingShowList.Name = "toolStripButtonSendingShowList";
            toolStripButtonSendingShowList.Size = new System.Drawing.Size(23, 20);
            toolStripButtonSendingShowList.Text = "toolStripButton1";
            toolStripButtonSendingShowList.ToolTipText = "List the samples";
            toolStripButtonSendingShowList.Visible = false;
            toolStripButtonSendingShowList.Click += toolStripButtonSendingShowList_Click;
            // 
            // toolStripButtonSendingPreview
            // 
            toolStripButtonSendingPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSendingPreview.Image = Resource.List;
            toolStripButtonSendingPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSendingPreview.Name = "toolStripButtonSendingPreview";
            toolStripButtonSendingPreview.Size = new System.Drawing.Size(23, 20);
            toolStripButtonSendingPreview.Text = "Create a preview for the covering letter";
            toolStripButtonSendingPreview.Click += toolStripButtonSendingPreview_Click;
            // 
            // toolStripButtonSendingPreviewAllOnLoan
            // 
            toolStripButtonSendingPreviewAllOnLoan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSendingPreviewAllOnLoan.Image = Resource.ListRed;
            toolStripButtonSendingPreviewAllOnLoan.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSendingPreviewAllOnLoan.Name = "toolStripButtonSendingPreviewAllOnLoan";
            toolStripButtonSendingPreviewAllOnLoan.Size = new System.Drawing.Size(23, 20);
            toolStripButtonSendingPreviewAllOnLoan.Text = "Create a preview for the covering letter with all items on loan";
            toolStripButtonSendingPreviewAllOnLoan.Click += toolStripButtonSendingPreviewAllOnLoan_Click;
            // 
            // toolStripButtonSendingXslEditor
            // 
            toolStripButtonSendingXslEditor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSendingXslEditor.Image = Resource.LabelEditor;
            toolStripButtonSendingXslEditor.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSendingXslEditor.Name = "toolStripButtonSendingXslEditor";
            toolStripButtonSendingXslEditor.Size = new System.Drawing.Size(23, 20);
            toolStripButtonSendingXslEditor.Text = "Edit schema for document creation";
            toolStripButtonSendingXslEditor.Click += toolStripButtonSendingXslEditor_Click;
            // 
            // toolStripButtonSendingPrint
            // 
            toolStripButtonSendingPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSendingPrint.Image = Resource.Print;
            toolStripButtonSendingPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSendingPrint.Name = "toolStripButtonSendingPrint";
            toolStripButtonSendingPrint.Size = new System.Drawing.Size(23, 20);
            toolStripButtonSendingPrint.Text = "Print the covering letter";
            toolStripButtonSendingPrint.Click += toolStripButtonSendingPrint_Click;
            // 
            // toolStripButtonSendingSave
            // 
            toolStripButtonSendingSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSendingSave.Image = Resource.Save;
            toolStripButtonSendingSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButtonSendingSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSendingSave.Name = "toolStripButtonSendingSave";
            toolStripButtonSendingSave.Size = new System.Drawing.Size(23, 18);
            toolStripButtonSendingSave.Text = "Save the covering letter in the documents for this transaction";
            toolStripButtonSendingSave.Click += toolStripButtonSendingSave_Click;
            // 
            // toolStripButtonSendingScanner
            // 
            toolStripButtonSendingScanner.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSendingScanner.Image = Resource.ScannerBarcode;
            toolStripButtonSendingScanner.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSendingScanner.Name = "toolStripButtonSendingScanner";
            toolStripButtonSendingScanner.Size = new System.Drawing.Size(23, 20);
            toolStripButtonSendingScanner.Text = "Set timer intervall for scanner";
            toolStripButtonSendingScanner.Click += toolStripButtonSendingScanner_Click;
            toolStripButtonSendingScanner.DoubleClick += toolStripButtonSendingScanner_DoubleClick;
            // 
            // checkBoxSendingRestrictToCollection
            // 
            checkBoxSendingRestrictToCollection.AutoSize = true;
            checkBoxSendingRestrictToCollection.Checked = true;
            checkBoxSendingRestrictToCollection.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            tableLayoutPanelSending.SetColumnSpan(checkBoxSendingRestrictToCollection, 2);
            checkBoxSendingRestrictToCollection.Dock = System.Windows.Forms.DockStyle.Right;
            checkBoxSendingRestrictToCollection.Location = new System.Drawing.Point(256, 32);
            checkBoxSendingRestrictToCollection.Name = "checkBoxSendingRestrictToCollection";
            checkBoxSendingRestrictToCollection.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            checkBoxSendingRestrictToCollection.Size = new System.Drawing.Size(82, 23);
            checkBoxSendingRestrictToCollection.TabIndex = 11;
            checkBoxSendingRestrictToCollection.Text = "Restrict to ";
            checkBoxSendingRestrictToCollection.UseVisualStyleBackColor = true;
            checkBoxSendingRestrictToCollection.Visible = false;
            // 
            // checkBoxSendingRestrictToMaterial
            // 
            checkBoxSendingRestrictToMaterial.AutoSize = true;
            tableLayoutPanelSending.SetColumnSpan(checkBoxSendingRestrictToMaterial, 2);
            checkBoxSendingRestrictToMaterial.Dock = System.Windows.Forms.DockStyle.Right;
            checkBoxSendingRestrictToMaterial.Location = new System.Drawing.Point(413, 32);
            checkBoxSendingRestrictToMaterial.Name = "checkBoxSendingRestrictToMaterial";
            checkBoxSendingRestrictToMaterial.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            checkBoxSendingRestrictToMaterial.Size = new System.Drawing.Size(82, 23);
            checkBoxSendingRestrictToMaterial.TabIndex = 12;
            checkBoxSendingRestrictToMaterial.Text = "Restrict to ";
            checkBoxSendingRestrictToMaterial.UseVisualStyleBackColor = true;
            // 
            // pictureBoxSendingRestrictToMaterial
            // 
            pictureBoxSendingRestrictToMaterial.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxSendingRestrictToMaterial.Image = Resource.Specimen;
            pictureBoxSendingRestrictToMaterial.Location = new System.Drawing.Point(501, 32);
            pictureBoxSendingRestrictToMaterial.Name = "pictureBoxSendingRestrictToMaterial";
            pictureBoxSendingRestrictToMaterial.Padding = new System.Windows.Forms.Padding(2);
            pictureBoxSendingRestrictToMaterial.Size = new System.Drawing.Size(192, 23);
            pictureBoxSendingRestrictToMaterial.TabIndex = 13;
            pictureBoxSendingRestrictToMaterial.TabStop = false;
            // 
            // pictureBoxSendingRestrictToCollection
            // 
            pictureBoxSendingRestrictToCollection.Image = Resource.Collection;
            pictureBoxSendingRestrictToCollection.Location = new System.Drawing.Point(344, 32);
            pictureBoxSendingRestrictToCollection.Name = "pictureBoxSendingRestrictToCollection";
            pictureBoxSendingRestrictToCollection.Padding = new System.Windows.Forms.Padding(2);
            pictureBoxSendingRestrictToCollection.Size = new System.Drawing.Size(18, 23);
            pictureBoxSendingRestrictToCollection.TabIndex = 14;
            pictureBoxSendingRestrictToCollection.TabStop = false;
            pictureBoxSendingRestrictToCollection.Visible = false;
            // 
            // checkBoxSendingOrderByTaxa
            // 
            checkBoxSendingOrderByTaxa.AutoSize = true;
            tableLayoutPanelSending.SetColumnSpan(checkBoxSendingOrderByTaxa, 2);
            checkBoxSendingOrderByTaxa.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxSendingOrderByTaxa.Location = new System.Drawing.Point(3, 32);
            checkBoxSendingOrderByTaxa.Name = "checkBoxSendingOrderByTaxa";
            checkBoxSendingOrderByTaxa.Size = new System.Drawing.Size(97, 23);
            checkBoxSendingOrderByTaxa.TabIndex = 15;
            checkBoxSendingOrderByTaxa.Text = "Order by taxa";
            checkBoxSendingOrderByTaxa.UseVisualStyleBackColor = true;
            // 
            // checkBoxSendingSingleLines
            // 
            checkBoxSendingSingleLines.AutoSize = true;
            checkBoxSendingSingleLines.Checked = true;
            checkBoxSendingSingleLines.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxSendingSingleLines.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxSendingSingleLines.Location = new System.Drawing.Point(103, 29);
            checkBoxSendingSingleLines.Margin = new System.Windows.Forms.Padding(0);
            checkBoxSendingSingleLines.Name = "checkBoxSendingSingleLines";
            checkBoxSendingSingleLines.Size = new System.Drawing.Size(85, 29);
            checkBoxSendingSingleLines.TabIndex = 16;
            checkBoxSendingSingleLines.Text = "Single lines";
            checkBoxSendingSingleLines.UseVisualStyleBackColor = true;
            // 
            // checkBoxSendingIncludeType
            // 
            checkBoxSendingIncludeType.AutoSize = true;
            checkBoxSendingIncludeType.Checked = true;
            checkBoxSendingIncludeType.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxSendingIncludeType.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxSendingIncludeType.Location = new System.Drawing.Point(103, 58);
            checkBoxSendingIncludeType.Margin = new System.Windows.Forms.Padding(0);
            checkBoxSendingIncludeType.Name = "checkBoxSendingIncludeType";
            checkBoxSendingIncludeType.Size = new System.Drawing.Size(85, 20);
            checkBoxSendingIncludeType.TabIndex = 17;
            checkBoxSendingIncludeType.Text = "Type infos";
            checkBoxSendingIncludeType.UseVisualStyleBackColor = true;
            // 
            // labelSendingRestrictToTaxonomicGroup
            // 
            labelSendingRestrictToTaxonomicGroup.AutoSize = true;
            labelSendingRestrictToTaxonomicGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            labelSendingRestrictToTaxonomicGroup.Location = new System.Drawing.Point(210, 58);
            labelSendingRestrictToTaxonomicGroup.Name = "labelSendingRestrictToTaxonomicGroup";
            labelSendingRestrictToTaxonomicGroup.Size = new System.Drawing.Size(128, 20);
            labelSendingRestrictToTaxonomicGroup.TabIndex = 18;
            labelSendingRestrictToTaxonomicGroup.Text = "Restrict to:";
            labelSendingRestrictToTaxonomicGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            labelSendingRestrictToTaxonomicGroup.Visible = false;
            // 
            // comboBoxSendingRestrictToTaxonomicGroup
            // 
            tableLayoutPanelSending.SetColumnSpan(comboBoxSendingRestrictToTaxonomicGroup, 4);
            comboBoxSendingRestrictToTaxonomicGroup.Dock = System.Windows.Forms.DockStyle.Left;
            comboBoxSendingRestrictToTaxonomicGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxSendingRestrictToTaxonomicGroup.FormattingEnabled = true;
            comboBoxSendingRestrictToTaxonomicGroup.Location = new System.Drawing.Point(341, 58);
            comboBoxSendingRestrictToTaxonomicGroup.Margin = new System.Windows.Forms.Padding(0);
            comboBoxSendingRestrictToTaxonomicGroup.Name = "comboBoxSendingRestrictToTaxonomicGroup";
            comboBoxSendingRestrictToTaxonomicGroup.Size = new System.Drawing.Size(194, 23);
            comboBoxSendingRestrictToTaxonomicGroup.TabIndex = 19;
            comboBoxSendingRestrictToTaxonomicGroup.Visible = false;
            comboBoxSendingRestrictToTaxonomicGroup.DropDown += comboBoxSendingRestrictToTaxonomicGroup_DropDown;
            comboBoxSendingRestrictToTaxonomicGroup.SelectionChangeCommitted += comboBoxSendingRestrictToTaxonomicGroup_SelectionChangeCommitted;
            // 
            // splitContainerWebbrowserSending
            // 
            tableLayoutPanelSending.SetColumnSpan(splitContainerWebbrowserSending, 9);
            splitContainerWebbrowserSending.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerWebbrowserSending.Location = new System.Drawing.Point(3, 81);
            splitContainerWebbrowserSending.Name = "splitContainerWebbrowserSending";
            // 
            // splitContainerWebbrowserSending.Panel1
            // 
            splitContainerWebbrowserSending.Panel1.Controls.Add(panelWebbrowserSending);
            splitContainerWebbrowserSending.Panel2Collapsed = true;
            splitContainerWebbrowserSending.Size = new System.Drawing.Size(690, 323);
            splitContainerWebbrowserSending.SplitterDistance = 230;
            splitContainerWebbrowserSending.TabIndex = 20;
            // 
            // panelWebbrowserSending
            // 
            panelWebbrowserSending.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panelWebbrowserSending.Controls.Add(webBrowserSending);
            panelWebbrowserSending.Dock = System.Windows.Forms.DockStyle.Fill;
            panelWebbrowserSending.Location = new System.Drawing.Point(0, 0);
            panelWebbrowserSending.Name = "panelWebbrowserSending";
            panelWebbrowserSending.Padding = new System.Windows.Forms.Padding(3);
            panelWebbrowserSending.Size = new System.Drawing.Size(690, 323);
            panelWebbrowserSending.TabIndex = 3;
            // 
            // webBrowserSending
            // 
            webBrowserSending.Dock = System.Windows.Forms.DockStyle.Fill;
            webBrowserSending.Location = new System.Drawing.Point(3, 3);
            webBrowserSending.MinimumSize = new System.Drawing.Size(20, 20);
            webBrowserSending.Name = "webBrowserSending";
            webBrowserSending.Size = new System.Drawing.Size(680, 313);
            webBrowserSending.TabIndex = 1;
            // 
            // tableLayoutPanelSendingList
            // 
            tableLayoutPanelSendingList.ColumnCount = 1;
            tableLayoutPanelSendingList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelSendingList.Controls.Add(textBoxSendingAccessionNumber, 0, 0);
            tableLayoutPanelSendingList.Controls.Add(comboBoxSendingAccessionNumber, 0, 1);
            tableLayoutPanelSendingList.Controls.Add(listBoxSendingSpecimen, 0, 2);
            tableLayoutPanelSendingList.Controls.Add(toolStripSendingList, 0, 3);
            tableLayoutPanelSendingList.Controls.Add(labelSendingSpecimenReturned, 0, 6);
            tableLayoutPanelSendingList.Controls.Add(listBoxSendingSpecimenReturned, 0, 7);
            tableLayoutPanelSendingList.Controls.Add(labelSendingSpecimenForwarded, 0, 4);
            tableLayoutPanelSendingList.Controls.Add(listBoxSendingSpecimenForwarded, 0, 5);
            tableLayoutPanelSendingList.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(tableLayoutPanelSendingList, "Transaction sending");
            helpProvider.SetHelpNavigator(tableLayoutPanelSendingList, System.Windows.Forms.HelpNavigator.KeywordIndex);
            tableLayoutPanelSendingList.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelSendingList.Name = "tableLayoutPanelSendingList";
            tableLayoutPanelSendingList.RowCount = 8;
            tableLayoutPanelSendingList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSendingList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSendingList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelSendingList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSendingList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSendingList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSendingList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSendingList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            helpProvider.SetShowHelp(tableLayoutPanelSendingList, true);
            tableLayoutPanelSendingList.Size = new System.Drawing.Size(139, 407);
            tableLayoutPanelSendingList.TabIndex = 0;
            // 
            // textBoxSendingAccessionNumber
            // 
            textBoxSendingAccessionNumber.BackColor = System.Drawing.Color.Pink;
            textBoxSendingAccessionNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxSendingAccessionNumber.Location = new System.Drawing.Point(3, 3);
            textBoxSendingAccessionNumber.Name = "textBoxSendingAccessionNumber";
            textBoxSendingAccessionNumber.Size = new System.Drawing.Size(133, 23);
            textBoxSendingAccessionNumber.TabIndex = 8;
            textBoxSendingAccessionNumber.TextChanged += textBoxSendingAccessionNumber_TextChanged;
            textBoxSendingAccessionNumber.MouseEnter += textBoxSendingAccessionNumber_MouseEnter;
            // 
            // comboBoxSendingAccessionNumber
            // 
            comboBoxSendingAccessionNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxSendingAccessionNumber.FormattingEnabled = true;
            helpProvider.SetHelpKeyword(comboBoxSendingAccessionNumber, "Transaction sending");
            helpProvider.SetHelpNavigator(comboBoxSendingAccessionNumber, System.Windows.Forms.HelpNavigator.KeywordIndex);
            comboBoxSendingAccessionNumber.Location = new System.Drawing.Point(3, 32);
            comboBoxSendingAccessionNumber.Name = "comboBoxSendingAccessionNumber";
            helpProvider.SetShowHelp(comboBoxSendingAccessionNumber, true);
            comboBoxSendingAccessionNumber.Size = new System.Drawing.Size(133, 23);
            comboBoxSendingAccessionNumber.TabIndex = 10;
            toolTip.SetToolTip(comboBoxSendingAccessionNumber, "Select an accession number that should be included in the transaction");
            comboBoxSendingAccessionNumber.DropDown += comboBoxSendingAccessionNumber_DropDown;
            comboBoxSendingAccessionNumber.SelectionChangeCommitted += comboBoxSendingAccessionNumber_SelectionChangeCommitted;
            // 
            // listBoxSendingSpecimen
            // 
            listBoxSendingSpecimen.BackColor = System.Drawing.Color.Pink;
            listBoxSendingSpecimen.DataSource = collectionSpecimenPartOnLoanListBindingSource;
            listBoxSendingSpecimen.DisplayMember = "DisplayText";
            listBoxSendingSpecimen.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxSendingSpecimen.FormattingEnabled = true;
            listBoxSendingSpecimen.IntegralHeight = false;
            listBoxSendingSpecimen.ItemHeight = 15;
            listBoxSendingSpecimen.Location = new System.Drawing.Point(3, 61);
            listBoxSendingSpecimen.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            listBoxSendingSpecimen.Name = "listBoxSendingSpecimen";
            listBoxSendingSpecimen.Size = new System.Drawing.Size(133, 143);
            listBoxSendingSpecimen.Sorted = true;
            listBoxSendingSpecimen.TabIndex = 9;
            listBoxSendingSpecimen.ValueMember = "AccessionNumber";
            listBoxSendingSpecimen.Click += listBoxSendingSpecimen_Click;
            listBoxSendingSpecimen.Leave += listBoxSendingSpecimen_Leave;
            // 
            // collectionSpecimenPartOnLoanListBindingSource
            // 
            collectionSpecimenPartOnLoanListBindingSource.DataMember = "CollectionSpecimenPartOnLoanList";
            collectionSpecimenPartOnLoanListBindingSource.DataSource = dataSetTransaction;
            // 
            // toolStripSendingList
            // 
            toolStripSendingList.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripSendingList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonSendingListDelete, toolStripButtonSendingListFind });
            toolStripSendingList.Location = new System.Drawing.Point(3, 204);
            toolStripSendingList.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            toolStripSendingList.Name = "toolStripSendingList";
            toolStripSendingList.Size = new System.Drawing.Size(133, 25);
            toolStripSendingList.TabIndex = 11;
            toolStripSendingList.Text = "toolStrip1";
            // 
            // toolStripButtonSendingListDelete
            // 
            toolStripButtonSendingListDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSendingListDelete.Image = Resource.Delete;
            toolStripButtonSendingListDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSendingListDelete.Name = "toolStripButtonSendingListDelete";
            toolStripButtonSendingListDelete.Size = new System.Drawing.Size(23, 22);
            toolStripButtonSendingListDelete.Text = "Remove selected specimen part from the list";
            toolStripButtonSendingListDelete.Click += toolStripButtonSendingListDelete_Click;
            // 
            // toolStripButtonSendingListFind
            // 
            toolStripButtonSendingListFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSendingListFind.Image = Resource.Search;
            toolStripButtonSendingListFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSendingListFind.Name = "toolStripButtonSendingListFind";
            toolStripButtonSendingListFind.Size = new System.Drawing.Size(23, 22);
            toolStripButtonSendingListFind.Text = "Show selected specimen";
            toolStripButtonSendingListFind.Click += toolStripButtonSendingListFind_Click;
            // 
            // labelSendingSpecimenReturned
            // 
            labelSendingSpecimenReturned.AutoSize = true;
            labelSendingSpecimenReturned.Location = new System.Drawing.Point(3, 299);
            labelSendingSpecimenReturned.Name = "labelSendingSpecimenReturned";
            labelSendingSpecimenReturned.Size = new System.Drawing.Size(55, 15);
            labelSendingSpecimenReturned.TabIndex = 12;
            labelSendingSpecimenReturned.Text = "Returned";
            // 
            // listBoxSendingSpecimenReturned
            // 
            listBoxSendingSpecimenReturned.BackColor = System.Drawing.Color.LightGreen;
            listBoxSendingSpecimenReturned.DataSource = collectionSpecimenPartTransactionReturnListBindingSource;
            listBoxSendingSpecimenReturned.DisplayMember = "DisplayText";
            listBoxSendingSpecimenReturned.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxSendingSpecimenReturned.FormattingEnabled = true;
            listBoxSendingSpecimenReturned.ItemHeight = 15;
            listBoxSendingSpecimenReturned.Location = new System.Drawing.Point(3, 317);
            listBoxSendingSpecimenReturned.Name = "listBoxSendingSpecimenReturned";
            listBoxSendingSpecimenReturned.Size = new System.Drawing.Size(133, 87);
            listBoxSendingSpecimenReturned.TabIndex = 13;
            // 
            // collectionSpecimenPartTransactionReturnListBindingSource
            // 
            collectionSpecimenPartTransactionReturnListBindingSource.DataMember = "CollectionSpecimenPartTransactionReturnList";
            collectionSpecimenPartTransactionReturnListBindingSource.DataSource = dataSetTransaction;
            // 
            // labelSendingSpecimenForwarded
            // 
            labelSendingSpecimenForwarded.AutoSize = true;
            labelSendingSpecimenForwarded.Dock = System.Windows.Forms.DockStyle.Fill;
            labelSendingSpecimenForwarded.Location = new System.Drawing.Point(3, 232);
            labelSendingSpecimenForwarded.Name = "labelSendingSpecimenForwarded";
            labelSendingSpecimenForwarded.Size = new System.Drawing.Size(133, 15);
            labelSendingSpecimenForwarded.TabIndex = 14;
            labelSendingSpecimenForwarded.Text = "Forwarded";
            // 
            // listBoxSendingSpecimenForwarded
            // 
            listBoxSendingSpecimenForwarded.BackColor = System.Drawing.Color.SkyBlue;
            listBoxSendingSpecimenForwarded.DataSource = collectionSpecimenPartForwardingListBindingSource;
            listBoxSendingSpecimenForwarded.DisplayMember = "DisplayText";
            listBoxSendingSpecimenForwarded.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxSendingSpecimenForwarded.FormattingEnabled = true;
            listBoxSendingSpecimenForwarded.IntegralHeight = false;
            listBoxSendingSpecimenForwarded.ItemHeight = 15;
            listBoxSendingSpecimenForwarded.Location = new System.Drawing.Point(3, 250);
            listBoxSendingSpecimenForwarded.Name = "listBoxSendingSpecimenForwarded";
            listBoxSendingSpecimenForwarded.Size = new System.Drawing.Size(133, 46);
            listBoxSendingSpecimenForwarded.TabIndex = 15;
            // 
            // collectionSpecimenPartForwardingListBindingSource
            // 
            collectionSpecimenPartForwardingListBindingSource.DataMember = "CollectionSpecimenPartForwardingList";
            collectionSpecimenPartForwardingListBindingSource.DataSource = dataSetTransaction;
            // 
            // tabPageForwarding
            // 
            tabPageForwarding.Controls.Add(tableLayoutPanelForwarding);
            tabPageForwarding.ImageIndex = 11;
            tabPageForwarding.Location = new System.Drawing.Point(4, 24);
            tabPageForwarding.Name = "tabPageForwarding";
            tabPageForwarding.Size = new System.Drawing.Size(839, 409);
            tabPageForwarding.TabIndex = 11;
            tabPageForwarding.Text = "Forwarding";
            tabPageForwarding.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelForwarding
            // 
            tableLayoutPanelForwarding.ColumnCount = 5;
            tableLayoutPanelForwarding.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelForwarding.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelForwarding.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelForwarding.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelForwarding.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelForwarding.Controls.Add(labelForwardingSchema, 0, 0);
            tableLayoutPanelForwarding.Controls.Add(textBoxForwardingSchema, 1, 0);
            tableLayoutPanelForwarding.Controls.Add(toolStripForwarding, 4, 0);
            tableLayoutPanelForwarding.Controls.Add(splitContainerForwarding, 0, 2);
            tableLayoutPanelForwarding.Controls.Add(checkBoxForwardingSingleLines, 2, 1);
            tableLayoutPanelForwarding.Controls.Add(checkBoxForwardingOrderByTaxa, 0, 1);
            tableLayoutPanelForwarding.Controls.Add(checkBoxForwardingIncludeType, 3, 1);
            tableLayoutPanelForwarding.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(tableLayoutPanelForwarding, "Transaction forwarding");
            helpProvider.SetHelpNavigator(tableLayoutPanelForwarding, System.Windows.Forms.HelpNavigator.KeywordIndex);
            tableLayoutPanelForwarding.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelForwarding.Name = "tableLayoutPanelForwarding";
            tableLayoutPanelForwarding.RowCount = 3;
            tableLayoutPanelForwarding.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelForwarding.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelForwarding.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            helpProvider.SetShowHelp(tableLayoutPanelForwarding, true);
            tableLayoutPanelForwarding.Size = new System.Drawing.Size(839, 409);
            tableLayoutPanelForwarding.TabIndex = 5;
            // 
            // labelForwardingSchema
            // 
            labelForwardingSchema.AutoSize = true;
            labelForwardingSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            labelForwardingSchema.Location = new System.Drawing.Point(3, 0);
            labelForwardingSchema.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            labelForwardingSchema.Name = "labelForwardingSchema";
            labelForwardingSchema.Size = new System.Drawing.Size(52, 29);
            labelForwardingSchema.TabIndex = 4;
            labelForwardingSchema.Text = "Schema:";
            labelForwardingSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxForwardingSchema
            // 
            tableLayoutPanelForwarding.SetColumnSpan(textBoxForwardingSchema, 3);
            textBoxForwardingSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxForwardingSchema.Location = new System.Drawing.Point(55, 3);
            textBoxForwardingSchema.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            textBoxForwardingSchema.Name = "textBoxForwardingSchema";
            textBoxForwardingSchema.Size = new System.Drawing.Size(679, 23);
            textBoxForwardingSchema.TabIndex = 5;
            // 
            // toolStripForwarding
            // 
            toolStripForwarding.BackColor = System.Drawing.SystemColors.ControlLightLight;
            toolStripForwarding.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStripForwarding.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripForwarding.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonForwardingOpenSchema, toolStripSeparator2, toolStripButtonForwardingShowLists, toolStripButtonForwardingPreview, toolStripButtonForwardingPrint, toolStripButtonForwardingSave });
            toolStripForwarding.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            toolStripForwarding.Location = new System.Drawing.Point(737, 0);
            toolStripForwarding.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            toolStripForwarding.Name = "toolStripForwarding";
            toolStripForwarding.Size = new System.Drawing.Size(99, 29);
            toolStripForwarding.TabIndex = 6;
            toolStripForwarding.Text = "toolStrip1";
            // 
            // toolStripButtonForwardingOpenSchema
            // 
            toolStripButtonForwardingOpenSchema.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonForwardingOpenSchema.Image = Resource.Open;
            toolStripButtonForwardingOpenSchema.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonForwardingOpenSchema.Name = "toolStripButtonForwardingOpenSchema";
            toolStripButtonForwardingOpenSchema.Size = new System.Drawing.Size(23, 20);
            toolStripButtonForwardingOpenSchema.Text = "Open the schema file for the forwarding letters";
            toolStripButtonForwardingOpenSchema.Click += toolStripButtonForwardingOpenSchema_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonForwardingShowLists
            // 
            toolStripButtonForwardingShowLists.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonForwardingShowLists.Image = Resource.ShowList;
            toolStripButtonForwardingShowLists.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonForwardingShowLists.Name = "toolStripButtonForwardingShowLists";
            toolStripButtonForwardingShowLists.Size = new System.Drawing.Size(23, 20);
            toolStripButtonForwardingShowLists.Text = "toolStripButton1";
            toolStripButtonForwardingShowLists.ToolTipText = "List returned samples";
            toolStripButtonForwardingShowLists.Visible = false;
            toolStripButtonForwardingShowLists.Click += toolStripButtonForwardingShowLists_Click;
            // 
            // toolStripButtonForwardingPreview
            // 
            toolStripButtonForwardingPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonForwardingPreview.Image = Resource.List;
            toolStripButtonForwardingPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonForwardingPreview.Name = "toolStripButtonForwardingPreview";
            toolStripButtonForwardingPreview.Size = new System.Drawing.Size(23, 20);
            toolStripButtonForwardingPreview.Text = "Create a preview of the forwarding letters";
            toolStripButtonForwardingPreview.Click += toolStripButtonForwardingPreview_Click;
            // 
            // toolStripButtonForwardingPrint
            // 
            toolStripButtonForwardingPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonForwardingPrint.Image = Resource.Print;
            toolStripButtonForwardingPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonForwardingPrint.Name = "toolStripButtonForwardingPrint";
            toolStripButtonForwardingPrint.Size = new System.Drawing.Size(23, 20);
            toolStripButtonForwardingPrint.Text = "Print the forwarding letters";
            toolStripButtonForwardingPrint.Click += toolStripButtonForwardingPrint_Click;
            // 
            // toolStripButtonForwardingSave
            // 
            toolStripButtonForwardingSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonForwardingSave.Image = Resource.Save;
            toolStripButtonForwardingSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButtonForwardingSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonForwardingSave.Name = "toolStripButtonForwardingSave";
            toolStripButtonForwardingSave.Size = new System.Drawing.Size(23, 18);
            toolStripButtonForwardingSave.Text = "Save the forwarding letters in the documents";
            toolStripButtonForwardingSave.Click += toolStripButtonForwardingSave_Click;
            // 
            // splitContainerForwarding
            // 
            tableLayoutPanelForwarding.SetColumnSpan(splitContainerForwarding, 5);
            splitContainerForwarding.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerForwarding.Location = new System.Drawing.Point(3, 57);
            splitContainerForwarding.Name = "splitContainerForwarding";
            // 
            // splitContainerForwarding.Panel1
            // 
            splitContainerForwarding.Panel1.Controls.Add(panel2);
            // 
            // splitContainerForwarding.Panel2
            // 
            splitContainerForwarding.Panel2.Controls.Add(tableLayoutPanelForwardingLists);
            splitContainerForwarding.Size = new System.Drawing.Size(833, 349);
            splitContainerForwarding.SplitterDistance = 651;
            splitContainerForwarding.TabIndex = 13;
            // 
            // panel2
            // 
            panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panel2.Controls.Add(webBrowserForwarding);
            panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            panel2.Location = new System.Drawing.Point(0, 0);
            panel2.Name = "panel2";
            panel2.Padding = new System.Windows.Forms.Padding(3);
            panel2.Size = new System.Drawing.Size(651, 349);
            panel2.TabIndex = 3;
            // 
            // webBrowserForwarding
            // 
            webBrowserForwarding.Dock = System.Windows.Forms.DockStyle.Fill;
            webBrowserForwarding.Location = new System.Drawing.Point(3, 3);
            webBrowserForwarding.MinimumSize = new System.Drawing.Size(20, 20);
            webBrowserForwarding.Name = "webBrowserForwarding";
            webBrowserForwarding.Size = new System.Drawing.Size(641, 339);
            webBrowserForwarding.TabIndex = 1;
            // 
            // tableLayoutPanelForwardingLists
            // 
            tableLayoutPanelForwardingLists.ColumnCount = 1;
            tableLayoutPanelForwardingLists.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelForwardingLists.Controls.Add(toolStripForwardingSpecimenForwarded, 0, 2);
            tableLayoutPanelForwardingLists.Controls.Add(labelForwardingSpecimenListOnLoan, 0, 0);
            tableLayoutPanelForwardingLists.Controls.Add(listBoxForwardingSpecimenForwarded, 0, 3);
            tableLayoutPanelForwardingLists.Controls.Add(listBoxForwardingSpecimenOnLoan, 0, 1);
            tableLayoutPanelForwardingLists.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelForwardingLists.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelForwardingLists.Name = "tableLayoutPanelForwardingLists";
            tableLayoutPanelForwardingLists.RowCount = 4;
            tableLayoutPanelForwardingLists.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelForwardingLists.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelForwardingLists.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelForwardingLists.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelForwardingLists.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanelForwardingLists.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanelForwardingLists.Size = new System.Drawing.Size(178, 349);
            tableLayoutPanelForwardingLists.TabIndex = 13;
            // 
            // toolStripForwardingSpecimenForwarded
            // 
            toolStripForwardingSpecimenForwarded.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripForwardingSpecimenForwarded.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripLabelForwardingSpecimenForwarded, toolStripButtonForwardingForwarded, toolStripButtonForwardingRemove });
            toolStripForwardingSpecimenForwarded.Location = new System.Drawing.Point(3, 169);
            toolStripForwardingSpecimenForwarded.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            toolStripForwardingSpecimenForwarded.Name = "toolStripForwardingSpecimenForwarded";
            toolStripForwardingSpecimenForwarded.Size = new System.Drawing.Size(172, 25);
            toolStripForwardingSpecimenForwarded.TabIndex = 20;
            toolStripForwardingSpecimenForwarded.Text = "toolStrip1";
            // 
            // toolStripLabelForwardingSpecimenForwarded
            // 
            toolStripLabelForwardingSpecimenForwarded.Name = "toolStripLabelForwardingSpecimenForwarded";
            toolStripLabelForwardingSpecimenForwarded.Size = new System.Drawing.Size(63, 22);
            toolStripLabelForwardingSpecimenForwarded.Text = "Forwarded";
            toolStripLabelForwardingSpecimenForwarded.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // toolStripButtonForwardingForwarded
            // 
            toolStripButtonForwardingForwarded.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripButtonForwardingForwarded.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonForwardingForwarded.Image = Resource.ArrowDown;
            toolStripButtonForwardingForwarded.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonForwardingForwarded.Name = "toolStripButtonForwardingForwarded";
            toolStripButtonForwardingForwarded.Size = new System.Drawing.Size(23, 22);
            toolStripButtonForwardingForwarded.Text = "Move the selelcted item to the list of the returned specimen";
            toolStripButtonForwardingForwarded.Click += toolStripButtonForwardingForwarded_Click;
            // 
            // toolStripButtonForwardingRemove
            // 
            toolStripButtonForwardingRemove.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripButtonForwardingRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonForwardingRemove.Image = Resource.ArrowUp;
            toolStripButtonForwardingRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonForwardingRemove.Name = "toolStripButtonForwardingRemove";
            toolStripButtonForwardingRemove.Size = new System.Drawing.Size(23, 22);
            toolStripButtonForwardingRemove.Text = "Remove selected items from list of forwarded parts";
            toolStripButtonForwardingRemove.Click += toolStripButtonForwardingRemove_Click;
            // 
            // labelForwardingSpecimenListOnLoan
            // 
            labelForwardingSpecimenListOnLoan.AutoSize = true;
            labelForwardingSpecimenListOnLoan.Dock = System.Windows.Forms.DockStyle.Fill;
            labelForwardingSpecimenListOnLoan.Location = new System.Drawing.Point(3, 0);
            labelForwardingSpecimenListOnLoan.Name = "labelForwardingSpecimenListOnLoan";
            labelForwardingSpecimenListOnLoan.Size = new System.Drawing.Size(172, 15);
            labelForwardingSpecimenListOnLoan.TabIndex = 19;
            labelForwardingSpecimenListOnLoan.Text = "Specimen on loan";
            // 
            // listBoxForwardingSpecimenForwarded
            // 
            listBoxForwardingSpecimenForwarded.BackColor = System.Drawing.Color.SkyBlue;
            listBoxForwardingSpecimenForwarded.DataSource = collectionSpecimenPartForwardingListBindingSource;
            listBoxForwardingSpecimenForwarded.DisplayMember = "DisplayText";
            listBoxForwardingSpecimenForwarded.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxForwardingSpecimenForwarded.FormattingEnabled = true;
            listBoxForwardingSpecimenForwarded.IntegralHeight = false;
            listBoxForwardingSpecimenForwarded.ItemHeight = 15;
            listBoxForwardingSpecimenForwarded.Location = new System.Drawing.Point(3, 194);
            listBoxForwardingSpecimenForwarded.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            listBoxForwardingSpecimenForwarded.Name = "listBoxForwardingSpecimenForwarded";
            listBoxForwardingSpecimenForwarded.Size = new System.Drawing.Size(172, 152);
            listBoxForwardingSpecimenForwarded.Sorted = true;
            listBoxForwardingSpecimenForwarded.TabIndex = 11;
            listBoxForwardingSpecimenForwarded.ValueMember = "AccessionNumber";
            // 
            // listBoxForwardingSpecimenOnLoan
            // 
            listBoxForwardingSpecimenOnLoan.BackColor = System.Drawing.Color.Pink;
            listBoxForwardingSpecimenOnLoan.DataSource = collectionSpecimenPartOnLoanListBindingSource;
            listBoxForwardingSpecimenOnLoan.DisplayMember = "DisplayText";
            listBoxForwardingSpecimenOnLoan.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxForwardingSpecimenOnLoan.FormattingEnabled = true;
            listBoxForwardingSpecimenOnLoan.IntegralHeight = false;
            listBoxForwardingSpecimenOnLoan.ItemHeight = 15;
            listBoxForwardingSpecimenOnLoan.Location = new System.Drawing.Point(3, 18);
            listBoxForwardingSpecimenOnLoan.Name = "listBoxForwardingSpecimenOnLoan";
            listBoxForwardingSpecimenOnLoan.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            listBoxForwardingSpecimenOnLoan.Size = new System.Drawing.Size(172, 148);
            listBoxForwardingSpecimenOnLoan.TabIndex = 15;
            listBoxForwardingSpecimenOnLoan.ValueMember = "SpecimenPartID";
            // 
            // checkBoxForwardingSingleLines
            // 
            checkBoxForwardingSingleLines.AutoSize = true;
            checkBoxForwardingSingleLines.Checked = true;
            checkBoxForwardingSingleLines.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxForwardingSingleLines.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxForwardingSingleLines.Location = new System.Drawing.Point(106, 29);
            checkBoxForwardingSingleLines.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            checkBoxForwardingSingleLines.Name = "checkBoxForwardingSingleLines";
            checkBoxForwardingSingleLines.Size = new System.Drawing.Size(85, 25);
            checkBoxForwardingSingleLines.TabIndex = 14;
            checkBoxForwardingSingleLines.Text = "Single lines";
            checkBoxForwardingSingleLines.UseVisualStyleBackColor = true;
            // 
            // checkBoxForwardingOrderByTaxa
            // 
            checkBoxForwardingOrderByTaxa.AutoSize = true;
            tableLayoutPanelForwarding.SetColumnSpan(checkBoxForwardingOrderByTaxa, 2);
            checkBoxForwardingOrderByTaxa.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxForwardingOrderByTaxa.Location = new System.Drawing.Point(3, 29);
            checkBoxForwardingOrderByTaxa.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            checkBoxForwardingOrderByTaxa.Name = "checkBoxForwardingOrderByTaxa";
            checkBoxForwardingOrderByTaxa.Size = new System.Drawing.Size(97, 25);
            checkBoxForwardingOrderByTaxa.TabIndex = 15;
            checkBoxForwardingOrderByTaxa.Text = "Order by taxa";
            checkBoxForwardingOrderByTaxa.UseVisualStyleBackColor = true;
            // 
            // checkBoxForwardingIncludeType
            // 
            checkBoxForwardingIncludeType.AutoSize = true;
            checkBoxForwardingIncludeType.Checked = true;
            checkBoxForwardingIncludeType.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxForwardingIncludeType.Location = new System.Drawing.Point(194, 32);
            checkBoxForwardingIncludeType.Name = "checkBoxForwardingIncludeType";
            checkBoxForwardingIncludeType.Size = new System.Drawing.Size(79, 19);
            checkBoxForwardingIncludeType.TabIndex = 16;
            checkBoxForwardingIncludeType.Text = "Type infos";
            checkBoxForwardingIncludeType.UseVisualStyleBackColor = true;
            // 
            // tabPageConfirmation
            // 
            tabPageConfirmation.Controls.Add(tableLayoutPanelConfirmation);
            helpProvider.SetHelpKeyword(tabPageConfirmation, "Transaction confirmation");
            helpProvider.SetHelpNavigator(tabPageConfirmation, System.Windows.Forms.HelpNavigator.KeywordIndex);
            tabPageConfirmation.ImageIndex = 2;
            tabPageConfirmation.Location = new System.Drawing.Point(4, 24);
            tabPageConfirmation.Name = "tabPageConfirmation";
            helpProvider.SetShowHelp(tabPageConfirmation, true);
            tabPageConfirmation.Size = new System.Drawing.Size(839, 407);
            tabPageConfirmation.TabIndex = 6;
            tabPageConfirmation.Text = "Confirmation";
            tabPageConfirmation.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelConfirmation
            // 
            tableLayoutPanelConfirmation.ColumnCount = 3;
            tableLayoutPanelConfirmation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelConfirmation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelConfirmation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelConfirmation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanelConfirmation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanelConfirmation.Controls.Add(labelConfirmationSchema, 0, 0);
            tableLayoutPanelConfirmation.Controls.Add(textBoxConfirmationSchema, 1, 0);
            tableLayoutPanelConfirmation.Controls.Add(toolStripConfirmation, 2, 0);
            tableLayoutPanelConfirmation.Controls.Add(splitContainerConfirmation, 0, 1);
            tableLayoutPanelConfirmation.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(tableLayoutPanelConfirmation, "Transaction confirmation");
            helpProvider.SetHelpNavigator(tableLayoutPanelConfirmation, System.Windows.Forms.HelpNavigator.KeywordIndex);
            tableLayoutPanelConfirmation.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelConfirmation.Name = "tableLayoutPanelConfirmation";
            tableLayoutPanelConfirmation.RowCount = 2;
            tableLayoutPanelConfirmation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelConfirmation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            helpProvider.SetShowHelp(tableLayoutPanelConfirmation, true);
            tableLayoutPanelConfirmation.Size = new System.Drawing.Size(839, 407);
            tableLayoutPanelConfirmation.TabIndex = 2;
            // 
            // labelConfirmationSchema
            // 
            labelConfirmationSchema.AutoSize = true;
            labelConfirmationSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            labelConfirmationSchema.Location = new System.Drawing.Point(3, 0);
            labelConfirmationSchema.Name = "labelConfirmationSchema";
            labelConfirmationSchema.Size = new System.Drawing.Size(52, 29);
            labelConfirmationSchema.TabIndex = 4;
            labelConfirmationSchema.Text = "Schema:";
            labelConfirmationSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxConfirmationSchema
            // 
            textBoxConfirmationSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxConfirmationSchema.Location = new System.Drawing.Point(61, 3);
            textBoxConfirmationSchema.Name = "textBoxConfirmationSchema";
            textBoxConfirmationSchema.Size = new System.Drawing.Size(673, 23);
            textBoxConfirmationSchema.TabIndex = 5;
            // 
            // toolStripConfirmation
            // 
            toolStripConfirmation.BackColor = System.Drawing.SystemColors.ControlLightLight;
            toolStripConfirmation.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStripConfirmation.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripConfirmation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonConfirmationOpenSchema, toolStripSeparatorConfirmation, toolStripButtonConfirmationShowList, toolStripButtonConfirmationPreview, toolStripButtonConfirmationPrint, toolStripButtonConfirmationSave });
            toolStripConfirmation.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            toolStripConfirmation.Location = new System.Drawing.Point(737, 0);
            toolStripConfirmation.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            toolStripConfirmation.Name = "toolStripConfirmation";
            toolStripConfirmation.Size = new System.Drawing.Size(99, 29);
            toolStripConfirmation.TabIndex = 6;
            toolStripConfirmation.Text = "toolStrip1";
            // 
            // toolStripButtonConfirmationOpenSchema
            // 
            toolStripButtonConfirmationOpenSchema.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonConfirmationOpenSchema.Image = Resource.Open;
            toolStripButtonConfirmationOpenSchema.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonConfirmationOpenSchema.Name = "toolStripButtonConfirmationOpenSchema";
            toolStripButtonConfirmationOpenSchema.Size = new System.Drawing.Size(23, 20);
            toolStripButtonConfirmationOpenSchema.Text = "Open schema file";
            toolStripButtonConfirmationOpenSchema.Click += toolStripButtonConfirmationOpenSchema_Click;
            // 
            // toolStripSeparatorConfirmation
            // 
            toolStripSeparatorConfirmation.Name = "toolStripSeparatorConfirmation";
            toolStripSeparatorConfirmation.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonConfirmationShowList
            // 
            toolStripButtonConfirmationShowList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonConfirmationShowList.Image = Resource.ShowList;
            toolStripButtonConfirmationShowList.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonConfirmationShowList.Name = "toolStripButtonConfirmationShowList";
            toolStripButtonConfirmationShowList.Size = new System.Drawing.Size(23, 20);
            toolStripButtonConfirmationShowList.Text = "toolStripButton1";
            toolStripButtonConfirmationShowList.ToolTipText = "List the samples";
            toolStripButtonConfirmationShowList.Visible = false;
            toolStripButtonConfirmationShowList.Click += toolStripButtonConfirmationShowList_Click;
            // 
            // toolStripButtonConfirmationPreview
            // 
            toolStripButtonConfirmationPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonConfirmationPreview.Image = Resource.List;
            toolStripButtonConfirmationPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonConfirmationPreview.Name = "toolStripButtonConfirmationPreview";
            toolStripButtonConfirmationPreview.Size = new System.Drawing.Size(23, 20);
            toolStripButtonConfirmationPreview.Text = "Create preview";
            toolStripButtonConfirmationPreview.Click += toolStripButtonConfirmationPreview_Click;
            // 
            // toolStripButtonConfirmationPrint
            // 
            toolStripButtonConfirmationPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonConfirmationPrint.Image = Resource.Print;
            toolStripButtonConfirmationPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonConfirmationPrint.Name = "toolStripButtonConfirmationPrint";
            toolStripButtonConfirmationPrint.Size = new System.Drawing.Size(23, 20);
            toolStripButtonConfirmationPrint.Text = "Print document";
            toolStripButtonConfirmationPrint.Click += toolStripButtonConfirmationPrint_Click;
            // 
            // toolStripButtonConfirmationSave
            // 
            toolStripButtonConfirmationSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonConfirmationSave.Image = Resource.Save;
            toolStripButtonConfirmationSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButtonConfirmationSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonConfirmationSave.Name = "toolStripButtonConfirmationSave";
            toolStripButtonConfirmationSave.Size = new System.Drawing.Size(23, 18);
            toolStripButtonConfirmationSave.Text = "Save document";
            toolStripButtonConfirmationSave.Click += toolStripButtonConfirmationSave_Click;
            // 
            // splitContainerConfirmation
            // 
            tableLayoutPanelConfirmation.SetColumnSpan(splitContainerConfirmation, 3);
            splitContainerConfirmation.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerConfirmation.Location = new System.Drawing.Point(3, 32);
            splitContainerConfirmation.Name = "splitContainerConfirmation";
            // 
            // splitContainerConfirmation.Panel1
            // 
            splitContainerConfirmation.Panel1.Controls.Add(panelReminderWebbrowser);
            // 
            // splitContainerConfirmation.Panel2
            // 
            splitContainerConfirmation.Panel2.Controls.Add(listBoxConfirmationSpecimenList);
            splitContainerConfirmation.Panel2.Controls.Add(labelConfirmationSpecimenList);
            splitContainerConfirmation.Size = new System.Drawing.Size(833, 372);
            splitContainerConfirmation.SplitterDistance = 670;
            splitContainerConfirmation.TabIndex = 9;
            // 
            // panelReminderWebbrowser
            // 
            panelReminderWebbrowser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panelReminderWebbrowser.Controls.Add(webBrowserConfirmation);
            panelReminderWebbrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            panelReminderWebbrowser.Location = new System.Drawing.Point(0, 0);
            panelReminderWebbrowser.Name = "panelReminderWebbrowser";
            panelReminderWebbrowser.Padding = new System.Windows.Forms.Padding(3);
            panelReminderWebbrowser.Size = new System.Drawing.Size(670, 372);
            panelReminderWebbrowser.TabIndex = 3;
            // 
            // webBrowserConfirmation
            // 
            webBrowserConfirmation.Dock = System.Windows.Forms.DockStyle.Fill;
            webBrowserConfirmation.Location = new System.Drawing.Point(3, 3);
            webBrowserConfirmation.MinimumSize = new System.Drawing.Size(20, 20);
            webBrowserConfirmation.Name = "webBrowserConfirmation";
            webBrowserConfirmation.Size = new System.Drawing.Size(660, 362);
            webBrowserConfirmation.TabIndex = 1;
            // 
            // listBoxConfirmationSpecimenList
            // 
            listBoxConfirmationSpecimenList.BackColor = System.Drawing.Color.Pink;
            listBoxConfirmationSpecimenList.DataSource = collectionSpecimenPartOnLoanListBindingSource;
            listBoxConfirmationSpecimenList.DisplayMember = "DisplayText";
            listBoxConfirmationSpecimenList.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxConfirmationSpecimenList.FormattingEnabled = true;
            listBoxConfirmationSpecimenList.IntegralHeight = false;
            listBoxConfirmationSpecimenList.ItemHeight = 15;
            listBoxConfirmationSpecimenList.Location = new System.Drawing.Point(0, 15);
            listBoxConfirmationSpecimenList.Name = "listBoxConfirmationSpecimenList";
            listBoxConfirmationSpecimenList.Size = new System.Drawing.Size(159, 357);
            listBoxConfirmationSpecimenList.Sorted = true;
            listBoxConfirmationSpecimenList.TabIndex = 8;
            listBoxConfirmationSpecimenList.ValueMember = "AccessionNumber";
            // 
            // labelConfirmationSpecimenList
            // 
            labelConfirmationSpecimenList.AutoSize = true;
            labelConfirmationSpecimenList.Dock = System.Windows.Forms.DockStyle.Top;
            labelConfirmationSpecimenList.Location = new System.Drawing.Point(0, 0);
            labelConfirmationSpecimenList.Name = "labelConfirmationSpecimenList";
            labelConfirmationSpecimenList.Size = new System.Drawing.Size(59, 15);
            labelConfirmationSpecimenList.TabIndex = 7;
            labelConfirmationSpecimenList.Text = "Specimen";
            labelConfirmationSpecimenList.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // tabPageReminder
            // 
            tabPageReminder.Controls.Add(tableLayoutPanelAdmonition);
            helpProvider.SetHelpKeyword(tabPageReminder, "Transaction reminder");
            helpProvider.SetHelpNavigator(tabPageReminder, System.Windows.Forms.HelpNavigator.KeywordIndex);
            tabPageReminder.ImageIndex = 3;
            tabPageReminder.Location = new System.Drawing.Point(4, 24);
            tabPageReminder.Name = "tabPageReminder";
            helpProvider.SetShowHelp(tabPageReminder, true);
            tabPageReminder.Size = new System.Drawing.Size(839, 407);
            tabPageReminder.TabIndex = 2;
            tabPageReminder.Text = "Reminder";
            tabPageReminder.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelAdmonition
            // 
            tableLayoutPanelAdmonition.ColumnCount = 3;
            tableLayoutPanelAdmonition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelAdmonition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelAdmonition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelAdmonition.Controls.Add(labelReminderSchema, 0, 0);
            tableLayoutPanelAdmonition.Controls.Add(textBoxReminderSchema, 1, 0);
            tableLayoutPanelAdmonition.Controls.Add(toolStripReminder, 2, 0);
            tableLayoutPanelAdmonition.Controls.Add(splitContainerReminder, 0, 1);
            tableLayoutPanelAdmonition.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(tableLayoutPanelAdmonition, "Transaction reminder");
            helpProvider.SetHelpNavigator(tableLayoutPanelAdmonition, System.Windows.Forms.HelpNavigator.KeywordIndex);
            tableLayoutPanelAdmonition.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelAdmonition.Name = "tableLayoutPanelAdmonition";
            tableLayoutPanelAdmonition.RowCount = 2;
            tableLayoutPanelAdmonition.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelAdmonition.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelAdmonition.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanelAdmonition.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanelAdmonition.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanelAdmonition.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            helpProvider.SetShowHelp(tableLayoutPanelAdmonition, true);
            tableLayoutPanelAdmonition.Size = new System.Drawing.Size(839, 407);
            tableLayoutPanelAdmonition.TabIndex = 3;
            // 
            // labelReminderSchema
            // 
            labelReminderSchema.AutoSize = true;
            labelReminderSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            labelReminderSchema.Location = new System.Drawing.Point(3, 0);
            labelReminderSchema.Name = "labelReminderSchema";
            labelReminderSchema.Size = new System.Drawing.Size(52, 29);
            labelReminderSchema.TabIndex = 4;
            labelReminderSchema.Text = "Schema:";
            labelReminderSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxReminderSchema
            // 
            textBoxReminderSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxReminderSchema.Location = new System.Drawing.Point(61, 3);
            textBoxReminderSchema.Name = "textBoxReminderSchema";
            textBoxReminderSchema.Size = new System.Drawing.Size(673, 23);
            textBoxReminderSchema.TabIndex = 5;
            // 
            // toolStripReminder
            // 
            toolStripReminder.BackColor = System.Drawing.SystemColors.ControlLightLight;
            toolStripReminder.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStripReminder.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripReminder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonReminderOpenSchema, toolStripSeparatorReminder, toolStripButtonReminderShowList, toolStripButtonReminderPreview, toolStripButtonReminderPrint, toolStripButtonReminderSave });
            toolStripReminder.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            toolStripReminder.Location = new System.Drawing.Point(737, 0);
            toolStripReminder.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            toolStripReminder.Name = "toolStripReminder";
            toolStripReminder.Size = new System.Drawing.Size(99, 29);
            toolStripReminder.TabIndex = 6;
            toolStripReminder.Text = "toolStrip1";
            // 
            // toolStripButtonReminderOpenSchema
            // 
            toolStripButtonReminderOpenSchema.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonReminderOpenSchema.Image = Resource.Open;
            toolStripButtonReminderOpenSchema.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonReminderOpenSchema.Name = "toolStripButtonReminderOpenSchema";
            toolStripButtonReminderOpenSchema.Size = new System.Drawing.Size(23, 20);
            toolStripButtonReminderOpenSchema.Text = "Open schema file";
            toolStripButtonReminderOpenSchema.Click += toolStripButtonReminderOpenSchema_Click;
            // 
            // toolStripSeparatorReminder
            // 
            toolStripSeparatorReminder.Name = "toolStripSeparatorReminder";
            toolStripSeparatorReminder.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonReminderShowList
            // 
            toolStripButtonReminderShowList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonReminderShowList.Image = Resource.ShowList;
            toolStripButtonReminderShowList.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonReminderShowList.Name = "toolStripButtonReminderShowList";
            toolStripButtonReminderShowList.Size = new System.Drawing.Size(23, 20);
            toolStripButtonReminderShowList.Text = "toolStripButton1";
            toolStripButtonReminderShowList.ToolTipText = "List the samples";
            toolStripButtonReminderShowList.Visible = false;
            toolStripButtonReminderShowList.Click += toolStripButtonReminderShowList_Click;
            // 
            // toolStripButtonReminderPreview
            // 
            toolStripButtonReminderPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonReminderPreview.Image = Resource.List;
            toolStripButtonReminderPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonReminderPreview.Name = "toolStripButtonReminderPreview";
            toolStripButtonReminderPreview.Size = new System.Drawing.Size(23, 20);
            toolStripButtonReminderPreview.Text = "Create preview";
            toolStripButtonReminderPreview.Click += toolStripButtonReminderPreview_Click;
            // 
            // toolStripButtonReminderPrint
            // 
            toolStripButtonReminderPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonReminderPrint.Image = Resource.Print;
            toolStripButtonReminderPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonReminderPrint.Name = "toolStripButtonReminderPrint";
            toolStripButtonReminderPrint.Size = new System.Drawing.Size(23, 20);
            toolStripButtonReminderPrint.Text = "Print document";
            toolStripButtonReminderPrint.Click += toolStripButtonReminderPrint_Click;
            // 
            // toolStripButtonReminderSave
            // 
            toolStripButtonReminderSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonReminderSave.Image = Resource.Save;
            toolStripButtonReminderSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButtonReminderSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonReminderSave.Name = "toolStripButtonReminderSave";
            toolStripButtonReminderSave.Size = new System.Drawing.Size(23, 18);
            toolStripButtonReminderSave.Text = "Save document";
            toolStripButtonReminderSave.Click += toolStripButtonReminderSave_Click;
            // 
            // splitContainerReminder
            // 
            tableLayoutPanelAdmonition.SetColumnSpan(splitContainerReminder, 3);
            splitContainerReminder.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerReminder.Location = new System.Drawing.Point(3, 32);
            splitContainerReminder.Name = "splitContainerReminder";
            // 
            // splitContainerReminder.Panel1
            // 
            splitContainerReminder.Panel1.Controls.Add(panelAdmonitionWebbrowser);
            // 
            // splitContainerReminder.Panel2
            // 
            splitContainerReminder.Panel2.Controls.Add(tableLayoutPanelReminderList);
            splitContainerReminder.Size = new System.Drawing.Size(833, 372);
            splitContainerReminder.SplitterDistance = 636;
            splitContainerReminder.TabIndex = 11;
            // 
            // panelAdmonitionWebbrowser
            // 
            panelAdmonitionWebbrowser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panelAdmonitionWebbrowser.Controls.Add(webBrowserReminder);
            panelAdmonitionWebbrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            panelAdmonitionWebbrowser.Location = new System.Drawing.Point(0, 0);
            panelAdmonitionWebbrowser.Name = "panelAdmonitionWebbrowser";
            panelAdmonitionWebbrowser.Padding = new System.Windows.Forms.Padding(3);
            panelAdmonitionWebbrowser.Size = new System.Drawing.Size(636, 372);
            panelAdmonitionWebbrowser.TabIndex = 3;
            // 
            // webBrowserReminder
            // 
            webBrowserReminder.Dock = System.Windows.Forms.DockStyle.Fill;
            webBrowserReminder.Location = new System.Drawing.Point(3, 3);
            webBrowserReminder.MinimumSize = new System.Drawing.Size(20, 20);
            webBrowserReminder.Name = "webBrowserReminder";
            webBrowserReminder.Size = new System.Drawing.Size(626, 362);
            webBrowserReminder.TabIndex = 1;
            // 
            // tableLayoutPanelReminderList
            // 
            tableLayoutPanelReminderList.ColumnCount = 1;
            tableLayoutPanelReminderList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelReminderList.Controls.Add(labelReminderSpecimenOnLoan, 0, 0);
            tableLayoutPanelReminderList.Controls.Add(listBoxReminderSpecimenOnLoan, 0, 1);
            tableLayoutPanelReminderList.Controls.Add(labelReminderSpecimenReturned, 0, 2);
            tableLayoutPanelReminderList.Controls.Add(listBoxReminderSpecimenReturned, 0, 3);
            tableLayoutPanelReminderList.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelReminderList.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelReminderList.Name = "tableLayoutPanelReminderList";
            tableLayoutPanelReminderList.RowCount = 4;
            tableLayoutPanelReminderList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelReminderList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelReminderList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelReminderList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelReminderList.Size = new System.Drawing.Size(193, 372);
            tableLayoutPanelReminderList.TabIndex = 0;
            // 
            // labelReminderSpecimenOnLoan
            // 
            labelReminderSpecimenOnLoan.AutoSize = true;
            labelReminderSpecimenOnLoan.Dock = System.Windows.Forms.DockStyle.Fill;
            labelReminderSpecimenOnLoan.Location = new System.Drawing.Point(3, 0);
            labelReminderSpecimenOnLoan.Name = "labelReminderSpecimenOnLoan";
            labelReminderSpecimenOnLoan.Size = new System.Drawing.Size(187, 15);
            labelReminderSpecimenOnLoan.TabIndex = 7;
            labelReminderSpecimenOnLoan.Text = "Specimen on loan";
            labelReminderSpecimenOnLoan.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // listBoxReminderSpecimenOnLoan
            // 
            listBoxReminderSpecimenOnLoan.BackColor = System.Drawing.Color.Pink;
            listBoxReminderSpecimenOnLoan.DataSource = collectionSpecimenPartOnLoanListBindingSource;
            listBoxReminderSpecimenOnLoan.DisplayMember = "DisplayText";
            listBoxReminderSpecimenOnLoan.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxReminderSpecimenOnLoan.FormattingEnabled = true;
            listBoxReminderSpecimenOnLoan.IntegralHeight = false;
            listBoxReminderSpecimenOnLoan.ItemHeight = 15;
            listBoxReminderSpecimenOnLoan.Location = new System.Drawing.Point(3, 18);
            listBoxReminderSpecimenOnLoan.Name = "listBoxReminderSpecimenOnLoan";
            listBoxReminderSpecimenOnLoan.Size = new System.Drawing.Size(187, 165);
            listBoxReminderSpecimenOnLoan.Sorted = true;
            listBoxReminderSpecimenOnLoan.TabIndex = 8;
            listBoxReminderSpecimenOnLoan.ValueMember = "AccessionNumber";
            // 
            // labelReminderSpecimenReturned
            // 
            labelReminderSpecimenReturned.AutoSize = true;
            labelReminderSpecimenReturned.Dock = System.Windows.Forms.DockStyle.Fill;
            labelReminderSpecimenReturned.Location = new System.Drawing.Point(3, 186);
            labelReminderSpecimenReturned.Name = "labelReminderSpecimenReturned";
            labelReminderSpecimenReturned.Size = new System.Drawing.Size(187, 15);
            labelReminderSpecimenReturned.TabIndex = 9;
            labelReminderSpecimenReturned.Text = "Returned specimen";
            labelReminderSpecimenReturned.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // listBoxReminderSpecimenReturned
            // 
            listBoxReminderSpecimenReturned.BackColor = System.Drawing.Color.LightGreen;
            listBoxReminderSpecimenReturned.DataSource = collectionSpecimenPartTransactionReturnListBindingSource;
            listBoxReminderSpecimenReturned.DisplayMember = "DisplayText";
            listBoxReminderSpecimenReturned.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxReminderSpecimenReturned.FormattingEnabled = true;
            listBoxReminderSpecimenReturned.IntegralHeight = false;
            listBoxReminderSpecimenReturned.ItemHeight = 15;
            listBoxReminderSpecimenReturned.Location = new System.Drawing.Point(3, 204);
            listBoxReminderSpecimenReturned.Name = "listBoxReminderSpecimenReturned";
            listBoxReminderSpecimenReturned.Size = new System.Drawing.Size(187, 165);
            listBoxReminderSpecimenReturned.Sorted = true;
            listBoxReminderSpecimenReturned.TabIndex = 10;
            listBoxReminderSpecimenReturned.ValueMember = "AccessionNumber";
            // 
            // tabPagePrinting
            // 
            tabPagePrinting.Controls.Add(splitContainerPrinting);
            helpProvider.SetHelpKeyword(tabPagePrinting, "Transaction printing");
            helpProvider.SetHelpNavigator(tabPagePrinting, System.Windows.Forms.HelpNavigator.KeywordIndex);
            tabPagePrinting.ImageIndex = 6;
            tabPagePrinting.Location = new System.Drawing.Point(4, 24);
            tabPagePrinting.Name = "tabPagePrinting";
            helpProvider.SetShowHelp(tabPagePrinting, true);
            tabPagePrinting.Size = new System.Drawing.Size(839, 407);
            tabPagePrinting.TabIndex = 9;
            tabPagePrinting.Text = "Printing, specimen";
            tabPagePrinting.UseVisualStyleBackColor = true;
            // 
            // splitContainerPrinting
            // 
            splitContainerPrinting.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerPrinting.Location = new System.Drawing.Point(0, 0);
            splitContainerPrinting.Name = "splitContainerPrinting";
            // 
            // splitContainerPrinting.Panel1
            // 
            splitContainerPrinting.Panel1.Controls.Add(panelWebbrowserPrint);
            splitContainerPrinting.Panel1.Controls.Add(tableLayoutPanelPrinting);
            // 
            // splitContainerPrinting.Panel2
            // 
            splitContainerPrinting.Panel2.Controls.Add(tableLayoutPanelPrintingList);
            splitContainerPrinting.Size = new System.Drawing.Size(839, 407);
            splitContainerPrinting.SplitterDistance = 679;
            splitContainerPrinting.TabIndex = 4;
            // 
            // panelWebbrowserPrint
            // 
            panelWebbrowserPrint.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panelWebbrowserPrint.Controls.Add(webBrowserPrint);
            panelWebbrowserPrint.Dock = System.Windows.Forms.DockStyle.Fill;
            panelWebbrowserPrint.Location = new System.Drawing.Point(0, 70);
            panelWebbrowserPrint.Name = "panelWebbrowserPrint";
            panelWebbrowserPrint.Padding = new System.Windows.Forms.Padding(3);
            panelWebbrowserPrint.Size = new System.Drawing.Size(679, 337);
            panelWebbrowserPrint.TabIndex = 3;
            // 
            // webBrowserPrint
            // 
            webBrowserPrint.Dock = System.Windows.Forms.DockStyle.Fill;
            webBrowserPrint.Location = new System.Drawing.Point(3, 3);
            webBrowserPrint.MinimumSize = new System.Drawing.Size(20, 20);
            webBrowserPrint.Name = "webBrowserPrint";
            webBrowserPrint.Size = new System.Drawing.Size(669, 327);
            webBrowserPrint.TabIndex = 1;
            // 
            // tableLayoutPanelPrinting
            // 
            tableLayoutPanelPrinting.ColumnCount = 9;
            tableLayoutPanelPrinting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelPrinting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelPrinting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelPrinting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelPrinting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelPrinting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelPrinting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelPrinting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelPrinting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelPrinting.Controls.Add(pictureBoxIncludeSubCollections, 7, 1);
            tableLayoutPanelPrinting.Controls.Add(checkBoxPrintingIncludeSubcollections, 6, 1);
            tableLayoutPanelPrinting.Controls.Add(labelPrintingSchemaFile, 0, 0);
            tableLayoutPanelPrinting.Controls.Add(textBoxPrintingSchemaFile, 1, 0);
            tableLayoutPanelPrinting.Controls.Add(toolStripPrinting, 7, 0);
            tableLayoutPanelPrinting.Controls.Add(checkBoxIncludeTypeInformation, 8, 1);
            tableLayoutPanelPrinting.Controls.Add(labelPrintingInclude, 3, 1);
            tableLayoutPanelPrinting.Controls.Add(checkBoxPrintingIncludeChildTransactions, 4, 1);
            tableLayoutPanelPrinting.Controls.Add(pictureBoxIncludeSubTransactions, 5, 1);
            tableLayoutPanelPrinting.Controls.Add(labelPrintingConversion, 0, 1);
            tableLayoutPanelPrinting.Controls.Add(comboBoxPrintingConversion, 1, 1);
            tableLayoutPanelPrinting.Controls.Add(radioButtonPrintingGroupedByTaxa, 2, 2);
            tableLayoutPanelPrinting.Controls.Add(radioButtonPrintingGroupedByNumber, 3, 2);
            tableLayoutPanelPrinting.Controls.Add(radioButtonPrintingSingleNumbers, 5, 2);
            tableLayoutPanelPrinting.Controls.Add(checkBoxPrintingOrderByMaterial, 0, 2);
            tableLayoutPanelPrinting.Dock = System.Windows.Forms.DockStyle.Top;
            helpProvider.SetHelpKeyword(tableLayoutPanelPrinting, "Transaction printing");
            helpProvider.SetHelpNavigator(tableLayoutPanelPrinting, System.Windows.Forms.HelpNavigator.KeywordIndex);
            tableLayoutPanelPrinting.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelPrinting.MinimumSize = new System.Drawing.Size(100, 0);
            tableLayoutPanelPrinting.Name = "tableLayoutPanelPrinting";
            tableLayoutPanelPrinting.RowCount = 3;
            tableLayoutPanelPrinting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelPrinting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelPrinting.RowStyles.Add(new System.Windows.Forms.RowStyle());
            helpProvider.SetShowHelp(tableLayoutPanelPrinting, true);
            tableLayoutPanelPrinting.Size = new System.Drawing.Size(679, 70);
            tableLayoutPanelPrinting.TabIndex = 1;
            // 
            // pictureBoxIncludeSubCollections
            // 
            pictureBoxIncludeSubCollections.Dock = System.Windows.Forms.DockStyle.Left;
            pictureBoxIncludeSubCollections.Image = Resource.Collection;
            pictureBoxIncludeSubCollections.Location = new System.Drawing.Point(578, 25);
            pictureBoxIncludeSubCollections.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            pictureBoxIncludeSubCollections.Name = "pictureBoxIncludeSubCollections";
            pictureBoxIncludeSubCollections.Size = new System.Drawing.Size(16, 16);
            pictureBoxIncludeSubCollections.TabIndex = 57;
            pictureBoxIncludeSubCollections.TabStop = false;
            // 
            // checkBoxPrintingIncludeSubcollections
            // 
            checkBoxPrintingIncludeSubcollections.AutoSize = true;
            checkBoxPrintingIncludeSubcollections.Checked = true;
            checkBoxPrintingIncludeSubcollections.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxPrintingIncludeSubcollections.Dock = System.Windows.Forms.DockStyle.Left;
            checkBoxPrintingIncludeSubcollections.Location = new System.Drawing.Point(437, 25);
            checkBoxPrintingIncludeSubcollections.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            checkBoxPrintingIncludeSubcollections.Name = "checkBoxPrintingIncludeSubcollections";
            checkBoxPrintingIncludeSubcollections.Size = new System.Drawing.Size(102, 16);
            checkBoxPrintingIncludeSubcollections.TabIndex = 20;
            checkBoxPrintingIncludeSubcollections.Text = "subcollections";
            toolTip.SetToolTip(checkBoxPrintingIncludeSubcollections, "Include subcollections of the collections");
            checkBoxPrintingIncludeSubcollections.UseVisualStyleBackColor = true;
            // 
            // labelPrintingSchemaFile
            // 
            labelPrintingSchemaFile.AutoSize = true;
            labelPrintingSchemaFile.Dock = System.Windows.Forms.DockStyle.Fill;
            labelPrintingSchemaFile.Location = new System.Drawing.Point(3, 0);
            labelPrintingSchemaFile.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            labelPrintingSchemaFile.Name = "labelPrintingSchemaFile";
            labelPrintingSchemaFile.Size = new System.Drawing.Size(71, 22);
            labelPrintingSchemaFile.TabIndex = 2;
            labelPrintingSchemaFile.Text = "Schema file:";
            labelPrintingSchemaFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPrintingSchemaFile
            // 
            tableLayoutPanelPrinting.SetColumnSpan(textBoxPrintingSchemaFile, 6);
            textBoxPrintingSchemaFile.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxPrintingSchemaFile.Location = new System.Drawing.Point(74, 3);
            textBoxPrintingSchemaFile.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            textBoxPrintingSchemaFile.Name = "textBoxPrintingSchemaFile";
            textBoxPrintingSchemaFile.Size = new System.Drawing.Size(504, 23);
            textBoxPrintingSchemaFile.TabIndex = 1;
            // 
            // toolStripPrinting
            // 
            toolStripPrinting.BackColor = System.Drawing.SystemColors.Control;
            tableLayoutPanelPrinting.SetColumnSpan(toolStripPrinting, 2);
            toolStripPrinting.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStripPrinting.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonOpenSchemaFile, toolStripSeparatorLabel, toolStripButtonPrintingPreview, toolStripButtonPrintingPrint, toolStripButtonPrintingSave, toolStripButtonScannerPrinting, toolStripButtonPrintingShowList, toolStripButtonPageSetup, toolStripButtonNewSchemaFile });
            toolStripPrinting.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            toolStripPrinting.Location = new System.Drawing.Point(578, 0);
            toolStripPrinting.MinimumSize = new System.Drawing.Size(100, 0);
            toolStripPrinting.Name = "toolStripPrinting";
            toolStripPrinting.Size = new System.Drawing.Size(101, 22);
            toolStripPrinting.TabIndex = 0;
            toolStripPrinting.Text = "toolStrip1";
            // 
            // toolStripButtonOpenSchemaFile
            // 
            toolStripButtonOpenSchemaFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonOpenSchemaFile.Image = Resource.Open;
            toolStripButtonOpenSchemaFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonOpenSchemaFile.Name = "toolStripButtonOpenSchemaFile";
            toolStripButtonOpenSchemaFile.Size = new System.Drawing.Size(23, 20);
            toolStripButtonOpenSchemaFile.Text = "Select schema file (XSD)";
            toolStripButtonOpenSchemaFile.Click += toolStripButtonOpenSchemaFile_Click;
            // 
            // toolStripSeparatorLabel
            // 
            toolStripSeparatorLabel.Name = "toolStripSeparatorLabel";
            toolStripSeparatorLabel.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonPrintingPreview
            // 
            toolStripButtonPrintingPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonPrintingPreview.Image = Resource.List;
            toolStripButtonPrintingPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonPrintingPreview.Name = "toolStripButtonPrintingPreview";
            toolStripButtonPrintingPreview.Size = new System.Drawing.Size(23, 20);
            toolStripButtonPrintingPreview.Text = "Create a preview";
            toolStripButtonPrintingPreview.Click += toolStripButtonPrintingPreview_Click;
            // 
            // toolStripButtonPrintingPrint
            // 
            toolStripButtonPrintingPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonPrintingPrint.Image = Resource.Print;
            toolStripButtonPrintingPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonPrintingPrint.Name = "toolStripButtonPrintingPrint";
            toolStripButtonPrintingPrint.Size = new System.Drawing.Size(23, 20);
            toolStripButtonPrintingPrint.Text = "Print the created document";
            toolStripButtonPrintingPrint.Click += toolStripButtonPrintingPrint_Click;
            // 
            // toolStripButtonPrintingSave
            // 
            toolStripButtonPrintingSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonPrintingSave.Image = Resource.Save;
            toolStripButtonPrintingSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButtonPrintingSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonPrintingSave.Name = "toolStripButtonPrintingSave";
            toolStripButtonPrintingSave.Size = new System.Drawing.Size(23, 18);
            toolStripButtonPrintingSave.Text = "Save the document";
            toolStripButtonPrintingSave.Click += toolStripButtonPrintingSave_Click;
            // 
            // toolStripButtonScannerPrinting
            // 
            toolStripButtonScannerPrinting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonScannerPrinting.Image = Resource.ScannerBarcode;
            toolStripButtonScannerPrinting.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonScannerPrinting.Name = "toolStripButtonScannerPrinting";
            toolStripButtonScannerPrinting.Size = new System.Drawing.Size(23, 20);
            toolStripButtonScannerPrinting.Text = "toolStripButton1";
            toolStripButtonScannerPrinting.ToolTipText = "Set timer intervall for scanner";
            toolStripButtonScannerPrinting.Click += toolStripButtonScannerPrinting_Click;
            // 
            // toolStripButtonPrintingShowList
            // 
            toolStripButtonPrintingShowList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonPrintingShowList.Image = Resource.ShowList;
            toolStripButtonPrintingShowList.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonPrintingShowList.Name = "toolStripButtonPrintingShowList";
            toolStripButtonPrintingShowList.Size = new System.Drawing.Size(23, 20);
            toolStripButtonPrintingShowList.Text = "toolStripButton1";
            toolStripButtonPrintingShowList.ToolTipText = "List the samples that should be printed";
            toolStripButtonPrintingShowList.Visible = false;
            toolStripButtonPrintingShowList.Click += toolStripButtonPrintingShowList_Click;
            // 
            // toolStripButtonPageSetup
            // 
            toolStripButtonPageSetup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonPageSetup.Image = Resource.PageSetup;
            toolStripButtonPageSetup.ImageTransparentColor = System.Drawing.Color.Purple;
            toolStripButtonPageSetup.Name = "toolStripButtonPageSetup";
            toolStripButtonPageSetup.Size = new System.Drawing.Size(23, 20);
            toolStripButtonPageSetup.Text = "Page setup";
            toolStripButtonPageSetup.Visible = false;
            // 
            // toolStripButtonNewSchemaFile
            // 
            toolStripButtonNewSchemaFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonNewSchemaFile.Image = Resource.New1;
            toolStripButtonNewSchemaFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonNewSchemaFile.Name = "toolStripButtonNewSchemaFile";
            toolStripButtonNewSchemaFile.Size = new System.Drawing.Size(23, 20);
            toolStripButtonNewSchemaFile.Text = "Create a new schema file";
            toolStripButtonNewSchemaFile.Visible = false;
            // 
            // checkBoxIncludeTypeInformation
            // 
            checkBoxIncludeTypeInformation.AutoSize = true;
            checkBoxIncludeTypeInformation.Checked = true;
            checkBoxIncludeTypeInformation.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxIncludeTypeInformation.Dock = System.Windows.Forms.DockStyle.Right;
            checkBoxIncludeTypeInformation.Location = new System.Drawing.Point(600, 25);
            checkBoxIncludeTypeInformation.Name = "checkBoxIncludeTypeInformation";
            checkBoxIncludeTypeInformation.Size = new System.Drawing.Size(76, 16);
            checkBoxIncludeTypeInformation.TabIndex = 53;
            checkBoxIncludeTypeInformation.Text = "type info.";
            toolTip.SetToolTip(checkBoxIncludeTypeInformation, "Include the information about type specimen in the lists");
            checkBoxIncludeTypeInformation.UseVisualStyleBackColor = true;
            checkBoxIncludeTypeInformation.CheckedChanged += checkBoxIncludeTypeInformation_CheckedChanged;
            // 
            // labelPrintingInclude
            // 
            labelPrintingInclude.AutoSize = true;
            labelPrintingInclude.Dock = System.Windows.Forms.DockStyle.Fill;
            labelPrintingInclude.Location = new System.Drawing.Point(235, 22);
            labelPrintingInclude.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            labelPrintingInclude.Name = "labelPrintingInclude";
            labelPrintingInclude.Size = new System.Drawing.Size(46, 22);
            labelPrintingInclude.TabIndex = 54;
            labelPrintingInclude.Text = "Include";
            labelPrintingInclude.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBoxPrintingIncludeChildTransactions
            // 
            checkBoxPrintingIncludeChildTransactions.AutoSize = true;
            checkBoxPrintingIncludeChildTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxPrintingIncludeChildTransactions.Location = new System.Drawing.Point(284, 25);
            checkBoxPrintingIncludeChildTransactions.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            checkBoxPrintingIncludeChildTransactions.Name = "checkBoxPrintingIncludeChildTransactions";
            checkBoxPrintingIncludeChildTransactions.Size = new System.Drawing.Size(131, 16);
            checkBoxPrintingIncludeChildTransactions.TabIndex = 55;
            checkBoxPrintingIncludeChildTransactions.Text = "inferior transactions";
            toolTip.SetToolTip(checkBoxPrintingIncludeChildTransactions, "Include the transactions dependet  resp. inferior ot the current transaction");
            checkBoxPrintingIncludeChildTransactions.UseVisualStyleBackColor = true;
            // 
            // pictureBoxIncludeSubTransactions
            // 
            pictureBoxIncludeSubTransactions.Dock = System.Windows.Forms.DockStyle.Left;
            pictureBoxIncludeSubTransactions.Image = Resource.Transaction;
            pictureBoxIncludeSubTransactions.Location = new System.Drawing.Point(415, 25);
            pictureBoxIncludeSubTransactions.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            pictureBoxIncludeSubTransactions.Name = "pictureBoxIncludeSubTransactions";
            pictureBoxIncludeSubTransactions.Size = new System.Drawing.Size(16, 16);
            pictureBoxIncludeSubTransactions.TabIndex = 56;
            pictureBoxIncludeSubTransactions.TabStop = false;
            // 
            // labelPrintingConversion
            // 
            labelPrintingConversion.AutoSize = true;
            labelPrintingConversion.Dock = System.Windows.Forms.DockStyle.Fill;
            labelPrintingConversion.Location = new System.Drawing.Point(3, 22);
            labelPrintingConversion.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            labelPrintingConversion.Name = "labelPrintingConversion";
            labelPrintingConversion.Size = new System.Drawing.Size(71, 22);
            labelPrintingConversion.TabIndex = 58;
            labelPrintingConversion.Text = "Conversion:";
            labelPrintingConversion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxPrintingConversion
            // 
            tableLayoutPanelPrinting.SetColumnSpan(comboBoxPrintingConversion, 2);
            comboBoxPrintingConversion.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxPrintingConversion.FormattingEnabled = true;
            comboBoxPrintingConversion.Location = new System.Drawing.Point(74, 25);
            comboBoxPrintingConversion.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            comboBoxPrintingConversion.Name = "comboBoxPrintingConversion";
            comboBoxPrintingConversion.Size = new System.Drawing.Size(155, 23);
            comboBoxPrintingConversion.TabIndex = 59;
            // 
            // radioButtonPrintingGroupedByTaxa
            // 
            radioButtonPrintingGroupedByTaxa.AutoSize = true;
            radioButtonPrintingGroupedByTaxa.Checked = true;
            radioButtonPrintingGroupedByTaxa.Dock = System.Windows.Forms.DockStyle.Right;
            radioButtonPrintingGroupedByTaxa.Location = new System.Drawing.Point(130, 47);
            radioButtonPrintingGroupedByTaxa.Name = "radioButtonPrintingGroupedByTaxa";
            radioButtonPrintingGroupedByTaxa.Size = new System.Drawing.Size(99, 20);
            radioButtonPrintingGroupedByTaxa.TabIndex = 60;
            radioButtonPrintingGroupedByTaxa.TabStop = true;
            radioButtonPrintingGroupedByTaxa.Text = "Group by taxa";
            radioButtonPrintingGroupedByTaxa.UseVisualStyleBackColor = true;
            // 
            // radioButtonPrintingGroupedByNumber
            // 
            radioButtonPrintingGroupedByNumber.AutoSize = true;
            tableLayoutPanelPrinting.SetColumnSpan(radioButtonPrintingGroupedByNumber, 2);
            radioButtonPrintingGroupedByNumber.Location = new System.Drawing.Point(235, 47);
            radioButtonPrintingGroupedByNumber.Name = "radioButtonPrintingGroupedByNumber";
            radioButtonPrintingGroupedByNumber.Size = new System.Drawing.Size(173, 19);
            radioButtonPrintingGroupedByNumber.TabIndex = 61;
            radioButtonPrintingGroupedByNumber.Text = "Group by accession number";
            radioButtonPrintingGroupedByNumber.UseVisualStyleBackColor = true;
            // 
            // radioButtonPrintingSingleNumbers
            // 
            radioButtonPrintingSingleNumbers.AutoSize = true;
            tableLayoutPanelPrinting.SetColumnSpan(radioButtonPrintingSingleNumbers, 3);
            radioButtonPrintingSingleNumbers.Dock = System.Windows.Forms.DockStyle.Left;
            radioButtonPrintingSingleNumbers.Location = new System.Drawing.Point(418, 47);
            radioButtonPrintingSingleNumbers.Name = "radioButtonPrintingSingleNumbers";
            radioButtonPrintingSingleNumbers.Size = new System.Drawing.Size(135, 20);
            radioButtonPrintingSingleNumbers.TabIndex = 62;
            radioButtonPrintingSingleNumbers.TabStop = true;
            radioButtonPrintingSingleNumbers.Text = "Print every specimen";
            radioButtonPrintingSingleNumbers.UseVisualStyleBackColor = true;
            // 
            // checkBoxPrintingOrderByMaterial
            // 
            checkBoxPrintingOrderByMaterial.AutoSize = true;
            checkBoxPrintingOrderByMaterial.Checked = true;
            checkBoxPrintingOrderByMaterial.CheckState = System.Windows.Forms.CheckState.Checked;
            tableLayoutPanelPrinting.SetColumnSpan(checkBoxPrintingOrderByMaterial, 2);
            checkBoxPrintingOrderByMaterial.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxPrintingOrderByMaterial.Location = new System.Drawing.Point(3, 47);
            checkBoxPrintingOrderByMaterial.Name = "checkBoxPrintingOrderByMaterial";
            checkBoxPrintingOrderByMaterial.Size = new System.Drawing.Size(121, 20);
            checkBoxPrintingOrderByMaterial.TabIndex = 63;
            checkBoxPrintingOrderByMaterial.Text = "Group by material";
            checkBoxPrintingOrderByMaterial.UseVisualStyleBackColor = true;
            checkBoxPrintingOrderByMaterial.CheckedChanged += checkBoxPrintingOrderByMaterial_CheckedChanged;
            // 
            // tableLayoutPanelPrintingList
            // 
            tableLayoutPanelPrintingList.ColumnCount = 1;
            tableLayoutPanelPrintingList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelPrintingList.Controls.Add(comboBoxPrintingAccessionNumber, 0, 1);
            tableLayoutPanelPrintingList.Controls.Add(toolStripSpecimenList, 0, 4);
            tableLayoutPanelPrintingList.Controls.Add(listBoxPrintingList, 0, 3);
            tableLayoutPanelPrintingList.Controls.Add(textBoxPrintingAccessionNumber, 0, 0);
            tableLayoutPanelPrintingList.Controls.Add(labelPrintingList, 0, 2);
            tableLayoutPanelPrintingList.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelPrintingList.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelPrintingList.Name = "tableLayoutPanelPrintingList";
            tableLayoutPanelPrintingList.RowCount = 5;
            tableLayoutPanelPrintingList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelPrintingList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelPrintingList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelPrintingList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelPrintingList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelPrintingList.Size = new System.Drawing.Size(156, 407);
            tableLayoutPanelPrintingList.TabIndex = 13;
            // 
            // comboBoxPrintingAccessionNumber
            // 
            comboBoxPrintingAccessionNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxPrintingAccessionNumber.DropDownWidth = 300;
            comboBoxPrintingAccessionNumber.FormattingEnabled = true;
            comboBoxPrintingAccessionNumber.Location = new System.Drawing.Point(3, 32);
            comboBoxPrintingAccessionNumber.Name = "comboBoxPrintingAccessionNumber";
            comboBoxPrintingAccessionNumber.Size = new System.Drawing.Size(150, 23);
            comboBoxPrintingAccessionNumber.TabIndex = 17;
            comboBoxPrintingAccessionNumber.DropDown += comboBoxPrintingAccessionNumber_DropDown;
            comboBoxPrintingAccessionNumber.SelectionChangeCommitted += comboBoxPrintingAccessionNumber_SelectionChangeCommitted;
            // 
            // toolStripSpecimenList
            // 
            toolStripSpecimenList.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStripSpecimenList.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripSpecimenList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonSpecimenListDelete, toolStripButtonSpecimenListSearch });
            toolStripSpecimenList.Location = new System.Drawing.Point(3, 379);
            toolStripSpecimenList.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            toolStripSpecimenList.Name = "toolStripSpecimenList";
            toolStripSpecimenList.Size = new System.Drawing.Size(150, 25);
            toolStripSpecimenList.TabIndex = 12;
            toolStripSpecimenList.Text = "toolStrip1";
            // 
            // toolStripButtonSpecimenListDelete
            // 
            toolStripButtonSpecimenListDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSpecimenListDelete.Image = Resource.Delete;
            toolStripButtonSpecimenListDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSpecimenListDelete.Name = "toolStripButtonSpecimenListDelete";
            toolStripButtonSpecimenListDelete.Size = new System.Drawing.Size(23, 22);
            toolStripButtonSpecimenListDelete.Text = "Remove selected specimen part from the list";
            toolStripButtonSpecimenListDelete.Click += toolStripButtonSpecimenListDelete_Click;
            // 
            // toolStripButtonSpecimenListSearch
            // 
            toolStripButtonSpecimenListSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSpecimenListSearch.Image = Resource.Search;
            toolStripButtonSpecimenListSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSpecimenListSearch.Name = "toolStripButtonSpecimenListSearch";
            toolStripButtonSpecimenListSearch.Size = new System.Drawing.Size(23, 22);
            toolStripButtonSpecimenListSearch.Text = "Show selected specimen";
            toolStripButtonSpecimenListSearch.Click += toolStripButtonSpecimenListSearch_Click;
            // 
            // listBoxPrintingList
            // 
            listBoxPrintingList.DataSource = collectionSpecimenPartListBindingSource;
            listBoxPrintingList.DisplayMember = "DisplayText";
            listBoxPrintingList.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxPrintingList.FormattingEnabled = true;
            listBoxPrintingList.IntegralHeight = false;
            listBoxPrintingList.ItemHeight = 15;
            listBoxPrintingList.Location = new System.Drawing.Point(3, 76);
            listBoxPrintingList.Name = "listBoxPrintingList";
            listBoxPrintingList.Size = new System.Drawing.Size(150, 300);
            listBoxPrintingList.Sorted = true;
            listBoxPrintingList.TabIndex = 1;
            listBoxPrintingList.ValueMember = "AccessionNumber";
            // 
            // collectionSpecimenPartListBindingSource
            // 
            collectionSpecimenPartListBindingSource.DataMember = "CollectionSpecimenPartList";
            collectionSpecimenPartListBindingSource.DataSource = dataSetTransaction;
            // 
            // textBoxPrintingAccessionNumber
            // 
            textBoxPrintingAccessionNumber.BackColor = System.Drawing.Color.Pink;
            textBoxPrintingAccessionNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            textBoxPrintingAccessionNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxPrintingAccessionNumber.Location = new System.Drawing.Point(3, 3);
            textBoxPrintingAccessionNumber.Name = "textBoxPrintingAccessionNumber";
            textBoxPrintingAccessionNumber.Size = new System.Drawing.Size(150, 23);
            textBoxPrintingAccessionNumber.TabIndex = 9;
            textBoxPrintingAccessionNumber.TextChanged += textBoxPrintingAccessionNumber_TextChanged;
            textBoxPrintingAccessionNumber.MouseEnter += textBoxPrintingAccessionNumber_MouseEnter;
            // 
            // labelPrintingList
            // 
            labelPrintingList.AutoSize = true;
            labelPrintingList.Dock = System.Windows.Forms.DockStyle.Fill;
            labelPrintingList.Location = new System.Drawing.Point(3, 58);
            labelPrintingList.Name = "labelPrintingList";
            labelPrintingList.Size = new System.Drawing.Size(150, 15);
            labelPrintingList.TabIndex = 0;
            labelPrintingList.Text = "Specimen";
            // 
            // tabPageDocuments
            // 
            tabPageDocuments.Controls.Add(splitContainerDocuments);
            helpProvider.SetHelpKeyword(tabPageDocuments, "Transaction documents");
            helpProvider.SetHelpNavigator(tabPageDocuments, System.Windows.Forms.HelpNavigator.KeywordIndex);
            tabPageDocuments.ImageIndex = 7;
            tabPageDocuments.Location = new System.Drawing.Point(4, 24);
            tabPageDocuments.Name = "tabPageDocuments";
            helpProvider.SetShowHelp(tabPageDocuments, true);
            tabPageDocuments.Size = new System.Drawing.Size(839, 409);
            tabPageDocuments.TabIndex = 5;
            tabPageDocuments.Text = "Saved documents";
            tabPageDocuments.UseVisualStyleBackColor = true;
            // 
            // splitContainerDocuments
            // 
            splitContainerDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(splitContainerDocuments, "Transaction documents");
            helpProvider.SetHelpNavigator(splitContainerDocuments, System.Windows.Forms.HelpNavigator.KeywordIndex);
            splitContainerDocuments.Location = new System.Drawing.Point(0, 0);
            splitContainerDocuments.Name = "splitContainerDocuments";
            // 
            // splitContainerDocuments.Panel1
            // 
            splitContainerDocuments.Panel1.Controls.Add(listBoxTransactionDocuments);
            splitContainerDocuments.Panel1.Controls.Add(toolStripTransactionDocument);
            // 
            // splitContainerDocuments.Panel2
            // 
            splitContainerDocuments.Panel2.Controls.Add(splitContainerTransactionDocumentData);
            helpProvider.SetShowHelp(splitContainerDocuments, true);
            splitContainerDocuments.Size = new System.Drawing.Size(839, 409);
            splitContainerDocuments.SplitterDistance = 138;
            splitContainerDocuments.TabIndex = 1;
            // 
            // listBoxTransactionDocuments
            // 
            listBoxTransactionDocuments.DataSource = transactionDocumentBindingSource;
            listBoxTransactionDocuments.DisplayMember = "DisplayText";
            listBoxTransactionDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxTransactionDocuments.FormattingEnabled = true;
            listBoxTransactionDocuments.IntegralHeight = false;
            listBoxTransactionDocuments.ItemHeight = 15;
            listBoxTransactionDocuments.Location = new System.Drawing.Point(0, 0);
            listBoxTransactionDocuments.Name = "listBoxTransactionDocuments";
            listBoxTransactionDocuments.Size = new System.Drawing.Size(138, 384);
            listBoxTransactionDocuments.TabIndex = 0;
            listBoxTransactionDocuments.ValueMember = "Date";
            listBoxTransactionDocuments.SelectedIndexChanged += listBoxTransactionDocuments_SelectedIndexChanged;
            // 
            // transactionDocumentBindingSource
            // 
            transactionDocumentBindingSource.DataMember = "TransactionDocument";
            transactionDocumentBindingSource.DataSource = dataSetTransaction;
            // 
            // toolStripTransactionDocument
            // 
            toolStripTransactionDocument.Dock = System.Windows.Forms.DockStyle.Bottom;
            toolStripTransactionDocument.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripTransactionDocument.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonTransactionDocumentNew, toolStripButtonTransactionDocumentDelete });
            toolStripTransactionDocument.Location = new System.Drawing.Point(0, 384);
            toolStripTransactionDocument.Name = "toolStripTransactionDocument";
            toolStripTransactionDocument.Size = new System.Drawing.Size(138, 25);
            toolStripTransactionDocument.TabIndex = 1;
            toolStripTransactionDocument.Text = "toolStrip1";
            // 
            // toolStripButtonTransactionDocumentNew
            // 
            toolStripButtonTransactionDocumentNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonTransactionDocumentNew.Image = Resource.New1;
            toolStripButtonTransactionDocumentNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonTransactionDocumentNew.Name = "toolStripButtonTransactionDocumentNew";
            toolStripButtonTransactionDocumentNew.Size = new System.Drawing.Size(23, 22);
            toolStripButtonTransactionDocumentNew.Text = "Add a new document to this transaction";
            toolStripButtonTransactionDocumentNew.Click += toolStripButtonTransactionDocumentNew_Click;
            // 
            // toolStripButtonTransactionDocumentDelete
            // 
            toolStripButtonTransactionDocumentDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonTransactionDocumentDelete.Image = Resource.Delete;
            toolStripButtonTransactionDocumentDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonTransactionDocumentDelete.Name = "toolStripButtonTransactionDocumentDelete";
            toolStripButtonTransactionDocumentDelete.Size = new System.Drawing.Size(23, 22);
            toolStripButtonTransactionDocumentDelete.Text = "Delete the selected entry from the list";
            toolStripButtonTransactionDocumentDelete.Click += toolStripButtonTransactionDocumentDelete_Click;
            // 
            // splitContainerTransactionDocumentData
            // 
            splitContainerTransactionDocumentData.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerTransactionDocumentData.Location = new System.Drawing.Point(0, 0);
            splitContainerTransactionDocumentData.Name = "splitContainerTransactionDocumentData";
            splitContainerTransactionDocumentData.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerTransactionDocumentData.Panel1
            // 
            splitContainerTransactionDocumentData.Panel1.Controls.Add(splitContainerTransactionDocuments);
            // 
            // splitContainerTransactionDocumentData.Panel2
            // 
            splitContainerTransactionDocumentData.Panel2.Controls.Add(tableLayoutPanelTransactionDocument);
            splitContainerTransactionDocumentData.Panel2.Controls.Add(tableLayoutPanelTransactionDocumentButtons);
            splitContainerTransactionDocumentData.Size = new System.Drawing.Size(697, 409);
            splitContainerTransactionDocumentData.SplitterDistance = 275;
            splitContainerTransactionDocumentData.TabIndex = 1;
            // 
            // splitContainerTransactionDocuments
            // 
            splitContainerTransactionDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerTransactionDocuments.Location = new System.Drawing.Point(0, 0);
            splitContainerTransactionDocuments.Name = "splitContainerTransactionDocuments";
            // 
            // splitContainerTransactionDocuments.Panel1
            // 
            splitContainerTransactionDocuments.Panel1.Controls.Add(panelHistoryWebbrowser);
            // 
            // splitContainerTransactionDocuments.Panel2
            // 
            splitContainerTransactionDocuments.Panel2.Controls.Add(panelHistoryImage);
            splitContainerTransactionDocuments.Panel2.Controls.Add(toolStripDocumentImage);
            splitContainerTransactionDocuments.Size = new System.Drawing.Size(697, 275);
            splitContainerTransactionDocuments.SplitterDistance = 341;
            splitContainerTransactionDocuments.TabIndex = 0;
            // 
            // panelHistoryWebbrowser
            // 
            panelHistoryWebbrowser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panelHistoryWebbrowser.Controls.Add(splitContainerDocumentBrowser);
            panelHistoryWebbrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            panelHistoryWebbrowser.Location = new System.Drawing.Point(0, 0);
            panelHistoryWebbrowser.Name = "panelHistoryWebbrowser";
            panelHistoryWebbrowser.Size = new System.Drawing.Size(341, 275);
            panelHistoryWebbrowser.TabIndex = 1;
            // 
            // splitContainerDocumentBrowser
            // 
            splitContainerDocumentBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerDocumentBrowser.Location = new System.Drawing.Point(0, 0);
            splitContainerDocumentBrowser.Name = "splitContainerDocumentBrowser";
            splitContainerDocumentBrowser.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerDocumentBrowser.Panel1
            // 
            splitContainerDocumentBrowser.Panel1.Controls.Add(webBrowserTransactionDocuments);
            splitContainerDocumentBrowser.Panel2Collapsed = true;
            splitContainerDocumentBrowser.Size = new System.Drawing.Size(337, 271);
            splitContainerDocumentBrowser.SplitterDistance = 112;
            splitContainerDocumentBrowser.TabIndex = 1;
            // 
            // webBrowserTransactionDocuments
            // 
            webBrowserTransactionDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
            webBrowserTransactionDocuments.Location = new System.Drawing.Point(0, 0);
            webBrowserTransactionDocuments.MinimumSize = new System.Drawing.Size(20, 20);
            webBrowserTransactionDocuments.Name = "webBrowserTransactionDocuments";
            webBrowserTransactionDocuments.Size = new System.Drawing.Size(337, 271);
            webBrowserTransactionDocuments.TabIndex = 0;
            // 
            // panelHistoryImage
            // 
            panelHistoryImage.AutoScroll = true;
            panelHistoryImage.Controls.Add(pictureBoxTransactionDocuments);
            panelHistoryImage.Dock = System.Windows.Forms.DockStyle.Fill;
            panelHistoryImage.Location = new System.Drawing.Point(0, 0);
            panelHistoryImage.Name = "panelHistoryImage";
            panelHistoryImage.Size = new System.Drawing.Size(352, 275);
            panelHistoryImage.TabIndex = 1;
            // 
            // pictureBoxTransactionDocuments
            // 
            pictureBoxTransactionDocuments.Location = new System.Drawing.Point(0, 0);
            pictureBoxTransactionDocuments.Name = "pictureBoxTransactionDocuments";
            pictureBoxTransactionDocuments.Size = new System.Drawing.Size(2000, 2000);
            pictureBoxTransactionDocuments.TabIndex = 0;
            pictureBoxTransactionDocuments.TabStop = false;
            pictureBoxTransactionDocuments.DoubleClick += pictureBoxTransactionDocuments_DoubleClick;
            // 
            // toolStripDocumentImage
            // 
            toolStripDocumentImage.Dock = System.Windows.Forms.DockStyle.Bottom;
            toolStripDocumentImage.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripDocumentImage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonDocumentZoomAdapt, toolStripButtonDocumentZoom100Percent, toolStripSeparatorDocument, toolStripButtonDocumentRemove });
            toolStripDocumentImage.Location = new System.Drawing.Point(0, 206);
            toolStripDocumentImage.Name = "toolStripDocumentImage";
            toolStripDocumentImage.Size = new System.Drawing.Size(266, 25);
            toolStripDocumentImage.TabIndex = 2;
            toolStripDocumentImage.Text = "toolStrip1";
            toolStripDocumentImage.Visible = false;
            // 
            // toolStripButtonDocumentZoomAdapt
            // 
            toolStripButtonDocumentZoomAdapt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonDocumentZoomAdapt.Image = Resource.ZoomAdapt;
            toolStripButtonDocumentZoomAdapt.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonDocumentZoomAdapt.Name = "toolStripButtonDocumentZoomAdapt";
            toolStripButtonDocumentZoomAdapt.Size = new System.Drawing.Size(23, 22);
            toolStripButtonDocumentZoomAdapt.Text = "Adapt size of image to available space";
            // 
            // toolStripButtonDocumentZoom100Percent
            // 
            toolStripButtonDocumentZoom100Percent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonDocumentZoom100Percent.Image = Resource.Zoom100;
            toolStripButtonDocumentZoom100Percent.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonDocumentZoom100Percent.Name = "toolStripButtonDocumentZoom100Percent";
            toolStripButtonDocumentZoom100Percent.Size = new System.Drawing.Size(23, 22);
            toolStripButtonDocumentZoom100Percent.Text = "toolStripButton2";
            // 
            // toolStripSeparatorDocument
            // 
            toolStripSeparatorDocument.Name = "toolStripSeparatorDocument";
            toolStripSeparatorDocument.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonDocumentRemove
            // 
            toolStripButtonDocumentRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonDocumentRemove.Image = Resource.Delete;
            toolStripButtonDocumentRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonDocumentRemove.Name = "toolStripButtonDocumentRemove";
            toolStripButtonDocumentRemove.Size = new System.Drawing.Size(23, 22);
            toolStripButtonDocumentRemove.Text = "toolStripButton1";
            // 
            // tableLayoutPanelTransactionDocument
            // 
            tableLayoutPanelTransactionDocument.ColumnCount = 5;
            tableLayoutPanelTransactionDocument.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelTransactionDocument.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelTransactionDocument.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            tableLayoutPanelTransactionDocument.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelTransactionDocument.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            tableLayoutPanelTransactionDocument.Controls.Add(labelTransactionDocumentDisplayText, 0, 0);
            tableLayoutPanelTransactionDocument.Controls.Add(textBoxTransactionDocumentInternalNotes, 1, 1);
            tableLayoutPanelTransactionDocument.Controls.Add(textBoxTransactionDocumentDisplayText, 1, 0);
            tableLayoutPanelTransactionDocument.Controls.Add(labelDocumentNotes, 0, 1);
            tableLayoutPanelTransactionDocument.Controls.Add(labelDocumentType, 3, 0);
            tableLayoutPanelTransactionDocument.Controls.Add(comboBoxDocumentType, 4, 0);
            tableLayoutPanelTransactionDocument.Controls.Add(checkBoxTransactionDocumentPdf, 0, 2);
            tableLayoutPanelTransactionDocument.Controls.Add(buttonTransactionDocumentOpenPdf, 2, 2);
            tableLayoutPanelTransactionDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelTransactionDocument.Location = new System.Drawing.Point(0, 48);
            tableLayoutPanelTransactionDocument.Name = "tableLayoutPanelTransactionDocument";
            tableLayoutPanelTransactionDocument.RowCount = 3;
            tableLayoutPanelTransactionDocument.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelTransactionDocument.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelTransactionDocument.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelTransactionDocument.Size = new System.Drawing.Size(697, 82);
            tableLayoutPanelTransactionDocument.TabIndex = 5;
            // 
            // labelTransactionDocumentDisplayText
            // 
            labelTransactionDocumentDisplayText.AutoSize = true;
            labelTransactionDocumentDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            labelTransactionDocumentDisplayText.Location = new System.Drawing.Point(3, 0);
            labelTransactionDocumentDisplayText.Name = "labelTransactionDocumentDisplayText";
            labelTransactionDocumentDisplayText.Size = new System.Drawing.Size(71, 29);
            labelTransactionDocumentDisplayText.TabIndex = 0;
            labelTransactionDocumentDisplayText.Text = "Display text:";
            labelTransactionDocumentDisplayText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxTransactionDocumentInternalNotes
            // 
            tableLayoutPanelTransactionDocument.SetColumnSpan(textBoxTransactionDocumentInternalNotes, 4);
            textBoxTransactionDocumentInternalNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionDocumentBindingSource, "InternalNotes", true));
            textBoxTransactionDocumentInternalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxTransactionDocumentInternalNotes.Location = new System.Drawing.Point(80, 32);
            textBoxTransactionDocumentInternalNotes.Multiline = true;
            textBoxTransactionDocumentInternalNotes.Name = "textBoxTransactionDocumentInternalNotes";
            textBoxTransactionDocumentInternalNotes.Size = new System.Drawing.Size(614, 20);
            textBoxTransactionDocumentInternalNotes.TabIndex = 1;
            textBoxTransactionDocumentInternalNotes.Leave += textBoxTransactionDocumentInternalNotes_Leave;
            // 
            // textBoxTransactionDocumentDisplayText
            // 
            tableLayoutPanelTransactionDocument.SetColumnSpan(textBoxTransactionDocumentDisplayText, 2);
            textBoxTransactionDocumentDisplayText.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionDocumentBindingSource, "DisplayText", true));
            textBoxTransactionDocumentDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxTransactionDocumentDisplayText.Location = new System.Drawing.Point(80, 3);
            textBoxTransactionDocumentDisplayText.Name = "textBoxTransactionDocumentDisplayText";
            textBoxTransactionDocumentDisplayText.Size = new System.Drawing.Size(254, 23);
            textBoxTransactionDocumentDisplayText.TabIndex = 1;
            textBoxTransactionDocumentDisplayText.Leave += textBoxTransactionDocumentDisplayText_Leave;
            // 
            // labelDocumentNotes
            // 
            labelDocumentNotes.AutoSize = true;
            labelDocumentNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            labelDocumentNotes.Location = new System.Drawing.Point(3, 29);
            labelDocumentNotes.Name = "labelDocumentNotes";
            labelDocumentNotes.Size = new System.Drawing.Size(71, 26);
            labelDocumentNotes.TabIndex = 2;
            labelDocumentNotes.Text = "Notes:";
            labelDocumentNotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelDocumentType
            // 
            labelDocumentType.AutoSize = true;
            labelDocumentType.Dock = System.Windows.Forms.DockStyle.Fill;
            labelDocumentType.Location = new System.Drawing.Point(340, 0);
            labelDocumentType.Name = "labelDocumentType";
            labelDocumentType.Size = new System.Drawing.Size(34, 29);
            labelDocumentType.TabIndex = 3;
            labelDocumentType.Text = "Type:";
            labelDocumentType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxDocumentType
            // 
            comboBoxDocumentType.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionDocumentBindingSource, "DocumentType", true));
            comboBoxDocumentType.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxDocumentType.FormattingEnabled = true;
            comboBoxDocumentType.Location = new System.Drawing.Point(380, 3);
            comboBoxDocumentType.Name = "comboBoxDocumentType";
            comboBoxDocumentType.Size = new System.Drawing.Size(314, 23);
            comboBoxDocumentType.TabIndex = 4;
            comboBoxDocumentType.DropDown += comboBoxDocumentType_DropDown;
            // 
            // checkBoxTransactionDocumentPdf
            // 
            checkBoxTransactionDocumentPdf.AutoSize = true;
            tableLayoutPanelTransactionDocument.SetColumnSpan(checkBoxTransactionDocumentPdf, 2);
            checkBoxTransactionDocumentPdf.Dock = System.Windows.Forms.DockStyle.Left;
            checkBoxTransactionDocumentPdf.Image = Resource.PDF;
            checkBoxTransactionDocumentPdf.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            checkBoxTransactionDocumentPdf.Location = new System.Drawing.Point(3, 55);
            checkBoxTransactionDocumentPdf.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            checkBoxTransactionDocumentPdf.Name = "checkBoxTransactionDocumentPdf";
            checkBoxTransactionDocumentPdf.Size = new System.Drawing.Size(118, 27);
            checkBoxTransactionDocumentPdf.TabIndex = 5;
            checkBoxTransactionDocumentPdf.Text = "      Open pdf files";
            checkBoxTransactionDocumentPdf.UseVisualStyleBackColor = true;
            checkBoxTransactionDocumentPdf.Click += checkBoxTransactionDocumentPdf_Click;
            // 
            // buttonTransactionDocumentOpenPdf
            // 
            tableLayoutPanelTransactionDocument.SetColumnSpan(buttonTransactionDocumentOpenPdf, 3);
            buttonTransactionDocumentOpenPdf.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonTransactionDocumentOpenPdf.Image = Resource.PDF;
            buttonTransactionDocumentOpenPdf.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonTransactionDocumentOpenPdf.Location = new System.Drawing.Point(127, 55);
            buttonTransactionDocumentOpenPdf.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            buttonTransactionDocumentOpenPdf.Name = "buttonTransactionDocumentOpenPdf";
            buttonTransactionDocumentOpenPdf.Size = new System.Drawing.Size(567, 24);
            buttonTransactionDocumentOpenPdf.TabIndex = 6;
            buttonTransactionDocumentOpenPdf.Text = "      Open PDF in default browser";
            buttonTransactionDocumentOpenPdf.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonTransactionDocumentOpenPdf.UseVisualStyleBackColor = true;
            buttonTransactionDocumentOpenPdf.Click += buttonTransactionDocumentOpenPdf_Click;
            // 
            // tableLayoutPanelTransactionDocumentButtons
            // 
            tableLayoutPanelTransactionDocumentButtons.ColumnCount = 3;
            tableLayoutPanelTransactionDocumentButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            tableLayoutPanelTransactionDocumentButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            tableLayoutPanelTransactionDocumentButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelTransactionDocumentButtons.Controls.Add(buttonTransactionDocumentInsertDocument, 2, 0);
            tableLayoutPanelTransactionDocumentButtons.Controls.Add(buttonTransactionDocumentInsertUri, 1, 0);
            tableLayoutPanelTransactionDocumentButtons.Controls.Add(buttonTransactionDocumentFindUri, 0, 0);
            tableLayoutPanelTransactionDocumentButtons.Controls.Add(buttonTransactionDocumentInsertFile, 1, 1);
            tableLayoutPanelTransactionDocumentButtons.Dock = System.Windows.Forms.DockStyle.Top;
            tableLayoutPanelTransactionDocumentButtons.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelTransactionDocumentButtons.Name = "tableLayoutPanelTransactionDocumentButtons";
            tableLayoutPanelTransactionDocumentButtons.RowCount = 2;
            tableLayoutPanelTransactionDocumentButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelTransactionDocumentButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelTransactionDocumentButtons.Size = new System.Drawing.Size(697, 48);
            tableLayoutPanelTransactionDocumentButtons.TabIndex = 4;
            // 
            // buttonTransactionDocumentInsertDocument
            // 
            buttonTransactionDocumentInsertDocument.Dock = System.Windows.Forms.DockStyle.Left;
            buttonTransactionDocumentInsertDocument.Image = Resource.Media;
            buttonTransactionDocumentInsertDocument.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonTransactionDocumentInsertDocument.Location = new System.Drawing.Point(350, 10);
            buttonTransactionDocumentInsertDocument.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            buttonTransactionDocumentInsertDocument.Name = "buttonTransactionDocumentInsertDocument";
            tableLayoutPanelTransactionDocumentButtons.SetRowSpan(buttonTransactionDocumentInsertDocument, 2);
            buttonTransactionDocumentInsertDocument.Size = new System.Drawing.Size(207, 28);
            buttonTransactionDocumentInsertDocument.TabIndex = 2;
            buttonTransactionDocumentInsertDocument.Text = "Add image of document (screenshot)";
            buttonTransactionDocumentInsertDocument.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            toolTip.SetToolTip(buttonTransactionDocumentInsertDocument, "Add an image of a document from the clipboard");
            buttonTransactionDocumentInsertDocument.UseVisualStyleBackColor = true;
            buttonTransactionDocumentInsertDocument.Click += buttonTransactionDocumentsInsertDocument_Click;
            // 
            // buttonTransactionDocumentInsertUri
            // 
            buttonTransactionDocumentInsertUri.Dock = System.Windows.Forms.DockStyle.Right;
            buttonTransactionDocumentInsertUri.Image = Resource.Browse;
            buttonTransactionDocumentInsertUri.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            buttonTransactionDocumentInsertUri.Location = new System.Drawing.Point(153, 0);
            buttonTransactionDocumentInsertUri.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            buttonTransactionDocumentInsertUri.Name = "buttonTransactionDocumentInsertUri";
            buttonTransactionDocumentInsertUri.Size = new System.Drawing.Size(191, 24);
            buttonTransactionDocumentInsertUri.TabIndex = 3;
            buttonTransactionDocumentInsertUri.Text = "Add URI of document (webpage)";
            buttonTransactionDocumentInsertUri.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonTransactionDocumentInsertUri.UseVisualStyleBackColor = true;
            buttonTransactionDocumentInsertUri.Visible = false;
            buttonTransactionDocumentInsertUri.Click += buttonTransactionDocumentInsertUri_Click;
            // 
            // buttonTransactionDocumentFindUri
            // 
            buttonTransactionDocumentFindUri.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonTransactionDocumentFindUri.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, 0);
            buttonTransactionDocumentFindUri.ForeColor = System.Drawing.Color.Blue;
            buttonTransactionDocumentFindUri.Image = Resource.Browse;
            buttonTransactionDocumentFindUri.Location = new System.Drawing.Point(6, 10);
            buttonTransactionDocumentFindUri.Margin = new System.Windows.Forms.Padding(6, 10, 6, 10);
            buttonTransactionDocumentFindUri.Name = "buttonTransactionDocumentFindUri";
            tableLayoutPanelTransactionDocumentButtons.SetRowSpan(buttonTransactionDocumentFindUri, 2);
            buttonTransactionDocumentFindUri.Size = new System.Drawing.Size(57, 28);
            buttonTransactionDocumentFindUri.TabIndex = 4;
            buttonTransactionDocumentFindUri.Text = "https://...";
            toolTip.SetToolTip(buttonTransactionDocumentFindUri, "Browse for an URL and copy path into database");
            buttonTransactionDocumentFindUri.UseVisualStyleBackColor = true;
            buttonTransactionDocumentFindUri.Click += buttonTransactionDocumentFindUri_Click;
            // 
            // buttonTransactionDocumentInsertFile
            // 
            buttonTransactionDocumentInsertFile.Dock = System.Windows.Forms.DockStyle.Right;
            buttonTransactionDocumentInsertFile.Image = Resource.Folder;
            buttonTransactionDocumentInsertFile.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            buttonTransactionDocumentInsertFile.Location = new System.Drawing.Point(153, 24);
            buttonTransactionDocumentInsertFile.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            buttonTransactionDocumentInsertFile.Name = "buttonTransactionDocumentInsertFile";
            buttonTransactionDocumentInsertFile.Size = new System.Drawing.Size(191, 24);
            buttonTransactionDocumentInsertFile.TabIndex = 5;
            buttonTransactionDocumentInsertFile.Text = "Add path of the document (file)";
            buttonTransactionDocumentInsertFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonTransactionDocumentInsertFile.UseVisualStyleBackColor = true;
            buttonTransactionDocumentInsertFile.Click += buttonTransactionDocumentInsertFile_Click;
            // 
            // tabPageTransactionReturn
            // 
            tabPageTransactionReturn.Controls.Add(tableLayoutPanelTransactionReturn);
            tabPageTransactionReturn.ImageIndex = 5;
            tabPageTransactionReturn.Location = new System.Drawing.Point(4, 24);
            tabPageTransactionReturn.Name = "tabPageTransactionReturn";
            tabPageTransactionReturn.Size = new System.Drawing.Size(839, 409);
            tabPageTransactionReturn.TabIndex = 12;
            tabPageTransactionReturn.Text = "Return";
            tabPageTransactionReturn.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelTransactionReturn
            // 
            tableLayoutPanelTransactionReturn.ColumnCount = 6;
            tableLayoutPanelTransactionReturn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelTransactionReturn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelTransactionReturn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelTransactionReturn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelTransactionReturn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelTransactionReturn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelTransactionReturn.Controls.Add(labelTransactionReturnSchema, 0, 0);
            tableLayoutPanelTransactionReturn.Controls.Add(textBoxTransactionReturnSchema, 1, 0);
            tableLayoutPanelTransactionReturn.Controls.Add(toolStripTransactionReturn, 5, 0);
            tableLayoutPanelTransactionReturn.Controls.Add(splitContainerTransactionReturn, 0, 2);
            tableLayoutPanelTransactionReturn.Controls.Add(checkBoxReturnSingleLines, 2, 1);
            tableLayoutPanelTransactionReturn.Controls.Add(checkBoxReturnOrderByTaxa, 0, 1);
            tableLayoutPanelTransactionReturn.Controls.Add(checkBoxReturnIncludeType, 3, 1);
            tableLayoutPanelTransactionReturn.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(tableLayoutPanelTransactionReturn, "Transaction return");
            helpProvider.SetHelpNavigator(tableLayoutPanelTransactionReturn, System.Windows.Forms.HelpNavigator.KeywordIndex);
            tableLayoutPanelTransactionReturn.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelTransactionReturn.Name = "tableLayoutPanelTransactionReturn";
            tableLayoutPanelTransactionReturn.RowCount = 3;
            tableLayoutPanelTransactionReturn.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelTransactionReturn.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelTransactionReturn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            helpProvider.SetShowHelp(tableLayoutPanelTransactionReturn, true);
            tableLayoutPanelTransactionReturn.Size = new System.Drawing.Size(839, 409);
            tableLayoutPanelTransactionReturn.TabIndex = 4;
            // 
            // labelTransactionReturnSchema
            // 
            labelTransactionReturnSchema.AutoSize = true;
            labelTransactionReturnSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            labelTransactionReturnSchema.Location = new System.Drawing.Point(3, 0);
            labelTransactionReturnSchema.Name = "labelTransactionReturnSchema";
            labelTransactionReturnSchema.Size = new System.Drawing.Size(52, 29);
            labelTransactionReturnSchema.TabIndex = 4;
            labelTransactionReturnSchema.Text = "Schema:";
            labelTransactionReturnSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxTransactionReturnSchema
            // 
            tableLayoutPanelTransactionReturn.SetColumnSpan(textBoxTransactionReturnSchema, 3);
            textBoxTransactionReturnSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxTransactionReturnSchema.Location = new System.Drawing.Point(61, 3);
            textBoxTransactionReturnSchema.Name = "textBoxTransactionReturnSchema";
            textBoxTransactionReturnSchema.Size = new System.Drawing.Size(673, 23);
            textBoxTransactionReturnSchema.TabIndex = 5;
            // 
            // toolStripTransactionReturn
            // 
            toolStripTransactionReturn.BackColor = System.Drawing.SystemColors.ControlLightLight;
            toolStripTransactionReturn.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStripTransactionReturn.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripTransactionReturn.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonTransactionReturnOpenSchema, toolStripSeparator1, toolStripButtonTransactionReturnShowLists, toolStripButtonTransactionReturnPreview, toolStripButtonTransactionReturnPrint, toolStripButtonTransactionReturnSave });
            toolStripTransactionReturn.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            toolStripTransactionReturn.Location = new System.Drawing.Point(737, 0);
            toolStripTransactionReturn.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            toolStripTransactionReturn.Name = "toolStripTransactionReturn";
            toolStripTransactionReturn.Size = new System.Drawing.Size(99, 29);
            toolStripTransactionReturn.TabIndex = 6;
            toolStripTransactionReturn.Text = "toolStrip1";
            // 
            // toolStripButtonTransactionReturnOpenSchema
            // 
            toolStripButtonTransactionReturnOpenSchema.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonTransactionReturnOpenSchema.Image = Resource.Open;
            toolStripButtonTransactionReturnOpenSchema.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonTransactionReturnOpenSchema.Name = "toolStripButtonTransactionReturnOpenSchema";
            toolStripButtonTransactionReturnOpenSchema.Size = new System.Drawing.Size(23, 20);
            toolStripButtonTransactionReturnOpenSchema.Text = "Open the schema file for the return letter";
            toolStripButtonTransactionReturnOpenSchema.Click += toolStripButtonTransactionReturnOpenSchema_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonTransactionReturnShowLists
            // 
            toolStripButtonTransactionReturnShowLists.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonTransactionReturnShowLists.Image = Resource.ShowList;
            toolStripButtonTransactionReturnShowLists.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonTransactionReturnShowLists.Name = "toolStripButtonTransactionReturnShowLists";
            toolStripButtonTransactionReturnShowLists.Size = new System.Drawing.Size(23, 20);
            toolStripButtonTransactionReturnShowLists.Text = "toolStripButton1";
            toolStripButtonTransactionReturnShowLists.ToolTipText = "List returned samples";
            toolStripButtonTransactionReturnShowLists.Visible = false;
            toolStripButtonTransactionReturnShowLists.Click += toolStripButtonTransactionReturnShowLists_Click;
            // 
            // toolStripButtonTransactionReturnPreview
            // 
            toolStripButtonTransactionReturnPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonTransactionReturnPreview.Image = Resource.List;
            toolStripButtonTransactionReturnPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonTransactionReturnPreview.Name = "toolStripButtonTransactionReturnPreview";
            toolStripButtonTransactionReturnPreview.Size = new System.Drawing.Size(23, 20);
            toolStripButtonTransactionReturnPreview.Text = "Create a preview of the return letter";
            toolStripButtonTransactionReturnPreview.Click += toolStripButtonTransactionReturnPreview_Click;
            // 
            // toolStripButtonTransactionReturnPrint
            // 
            toolStripButtonTransactionReturnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonTransactionReturnPrint.Image = Resource.Print;
            toolStripButtonTransactionReturnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonTransactionReturnPrint.Name = "toolStripButtonTransactionReturnPrint";
            toolStripButtonTransactionReturnPrint.Size = new System.Drawing.Size(23, 20);
            toolStripButtonTransactionReturnPrint.Text = "Print the return letter";
            toolStripButtonTransactionReturnPrint.Click += toolStripButtonTransactionReturnPrint_Click;
            // 
            // toolStripButtonTransactionReturnSave
            // 
            toolStripButtonTransactionReturnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonTransactionReturnSave.Image = Resource.Save;
            toolStripButtonTransactionReturnSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButtonTransactionReturnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonTransactionReturnSave.Name = "toolStripButtonTransactionReturnSave";
            toolStripButtonTransactionReturnSave.Size = new System.Drawing.Size(23, 18);
            toolStripButtonTransactionReturnSave.Text = "Save the return letter in the documents";
            toolStripButtonTransactionReturnSave.Click += toolStripButtonTransactionReturnSave_Click;
            // 
            // splitContainerTransactionReturn
            // 
            tableLayoutPanelTransactionReturn.SetColumnSpan(splitContainerTransactionReturn, 6);
            splitContainerTransactionReturn.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerTransactionReturn.Location = new System.Drawing.Point(3, 57);
            splitContainerTransactionReturn.Name = "splitContainerTransactionReturn";
            // 
            // splitContainerTransactionReturn.Panel1
            // 
            splitContainerTransactionReturn.Panel1.Controls.Add(panel1);
            // 
            // splitContainerTransactionReturn.Panel2
            // 
            splitContainerTransactionReturn.Panel2.Controls.Add(tableLayoutPanelTransactionReturnLists);
            splitContainerTransactionReturn.Size = new System.Drawing.Size(833, 349);
            splitContainerTransactionReturn.SplitterDistance = 651;
            splitContainerTransactionReturn.TabIndex = 13;
            // 
            // panel1
            // 
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panel1.Controls.Add(webBrowserTransactionReturn);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Padding = new System.Windows.Forms.Padding(3);
            panel1.Size = new System.Drawing.Size(651, 349);
            panel1.TabIndex = 3;
            // 
            // webBrowserTransactionReturn
            // 
            webBrowserTransactionReturn.Dock = System.Windows.Forms.DockStyle.Fill;
            webBrowserTransactionReturn.Location = new System.Drawing.Point(3, 3);
            webBrowserTransactionReturn.MinimumSize = new System.Drawing.Size(20, 20);
            webBrowserTransactionReturn.Name = "webBrowserTransactionReturn";
            webBrowserTransactionReturn.Size = new System.Drawing.Size(641, 339);
            webBrowserTransactionReturn.TabIndex = 1;
            // 
            // tableLayoutPanelTransactionReturnLists
            // 
            tableLayoutPanelTransactionReturnLists.ColumnCount = 1;
            tableLayoutPanelTransactionReturnLists.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelTransactionReturnLists.Controls.Add(toolStripTransactionReturnListReturned, 0, 2);
            tableLayoutPanelTransactionReturnLists.Controls.Add(labelTransactionReturnListOnLoan, 0, 0);
            tableLayoutPanelTransactionReturnLists.Controls.Add(listBoxTransactionReturnReturned, 0, 3);
            tableLayoutPanelTransactionReturnLists.Controls.Add(listBoxTransactionReturnOnLoan, 0, 1);
            tableLayoutPanelTransactionReturnLists.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelTransactionReturnLists.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelTransactionReturnLists.Name = "tableLayoutPanelTransactionReturnLists";
            tableLayoutPanelTransactionReturnLists.RowCount = 4;
            tableLayoutPanelTransactionReturnLists.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelTransactionReturnLists.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelTransactionReturnLists.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelTransactionReturnLists.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelTransactionReturnLists.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanelTransactionReturnLists.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanelTransactionReturnLists.Size = new System.Drawing.Size(178, 349);
            tableLayoutPanelTransactionReturnLists.TabIndex = 13;
            // 
            // toolStripTransactionReturnListReturned
            // 
            toolStripTransactionReturnListReturned.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripTransactionReturnListReturned.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripLabelTransactionReturnListReturned, toolStripButtonTransactionReturnSpecimen, toolStripButtonTransactionReturnRemove, toolStripButtonReturnScanner, toolStripTextBoxReturnScanner });
            toolStripTransactionReturnListReturned.Location = new System.Drawing.Point(3, 169);
            toolStripTransactionReturnListReturned.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            toolStripTransactionReturnListReturned.Name = "toolStripTransactionReturnListReturned";
            toolStripTransactionReturnListReturned.Size = new System.Drawing.Size(172, 25);
            toolStripTransactionReturnListReturned.TabIndex = 20;
            toolStripTransactionReturnListReturned.Text = "toolStrip1";
            // 
            // toolStripLabelTransactionReturnListReturned
            // 
            toolStripLabelTransactionReturnListReturned.Name = "toolStripLabelTransactionReturnListReturned";
            toolStripLabelTransactionReturnListReturned.Size = new System.Drawing.Size(27, 22);
            toolStripLabelTransactionReturnListReturned.Text = "Ret.";
            toolStripLabelTransactionReturnListReturned.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // toolStripButtonTransactionReturnSpecimen
            // 
            toolStripButtonTransactionReturnSpecimen.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripButtonTransactionReturnSpecimen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonTransactionReturnSpecimen.Image = Resource.ArrowDown;
            toolStripButtonTransactionReturnSpecimen.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonTransactionReturnSpecimen.Name = "toolStripButtonTransactionReturnSpecimen";
            toolStripButtonTransactionReturnSpecimen.Size = new System.Drawing.Size(23, 22);
            toolStripButtonTransactionReturnSpecimen.Text = "Move the selelcted item to the list of the returned specimen";
            toolStripButtonTransactionReturnSpecimen.Click += toolStripButtonTransactionReturnSpecimen_Click;
            // 
            // toolStripButtonTransactionReturnRemove
            // 
            toolStripButtonTransactionReturnRemove.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripButtonTransactionReturnRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonTransactionReturnRemove.Image = Resource.ArrowUp;
            toolStripButtonTransactionReturnRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonTransactionReturnRemove.Name = "toolStripButtonTransactionReturnRemove";
            toolStripButtonTransactionReturnRemove.Size = new System.Drawing.Size(23, 22);
            toolStripButtonTransactionReturnRemove.Text = "Remove selected item from list of returned";
            toolStripButtonTransactionReturnRemove.Click += toolStripButtonTransactionReturnRemove_Click;
            // 
            // toolStripButtonReturnScanner
            // 
            toolStripButtonReturnScanner.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonReturnScanner.Image = Resource.ScannerBarcode;
            toolStripButtonReturnScanner.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonReturnScanner.Name = "toolStripButtonReturnScanner";
            toolStripButtonReturnScanner.Size = new System.Drawing.Size(23, 22);
            toolStripButtonReturnScanner.Text = "Set scanner intervall";
            toolStripButtonReturnScanner.Click += toolStripButtonReturnScanner_Click;
            // 
            // toolStripTextBoxReturnScanner
            // 
            toolStripTextBoxReturnScanner.BackColor = System.Drawing.Color.Pink;
            toolStripTextBoxReturnScanner.Name = "toolStripTextBoxReturnScanner";
            toolStripTextBoxReturnScanner.Size = new System.Drawing.Size(60, 25);
            toolStripTextBoxReturnScanner.MouseEnter += toolStripTextBoxReturnScanner_MouseEnter;
            toolStripTextBoxReturnScanner.TextChanged += toolStripTextBoxReturnScanner_TextChanged;
            // 
            // labelTransactionReturnListOnLoan
            // 
            labelTransactionReturnListOnLoan.AutoSize = true;
            labelTransactionReturnListOnLoan.Dock = System.Windows.Forms.DockStyle.Fill;
            labelTransactionReturnListOnLoan.Location = new System.Drawing.Point(3, 0);
            labelTransactionReturnListOnLoan.Name = "labelTransactionReturnListOnLoan";
            labelTransactionReturnListOnLoan.Size = new System.Drawing.Size(172, 15);
            labelTransactionReturnListOnLoan.TabIndex = 19;
            labelTransactionReturnListOnLoan.Text = "Specimen on loan";
            // 
            // listBoxTransactionReturnReturned
            // 
            listBoxTransactionReturnReturned.BackColor = System.Drawing.Color.LightGreen;
            listBoxTransactionReturnReturned.DataSource = collectionSpecimenPartTransactionReturnListBindingSource;
            listBoxTransactionReturnReturned.DisplayMember = "DisplayText";
            listBoxTransactionReturnReturned.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxTransactionReturnReturned.FormattingEnabled = true;
            listBoxTransactionReturnReturned.IntegralHeight = false;
            listBoxTransactionReturnReturned.ItemHeight = 15;
            listBoxTransactionReturnReturned.Location = new System.Drawing.Point(3, 194);
            listBoxTransactionReturnReturned.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            listBoxTransactionReturnReturned.Name = "listBoxTransactionReturnReturned";
            listBoxTransactionReturnReturned.Size = new System.Drawing.Size(172, 152);
            listBoxTransactionReturnReturned.Sorted = true;
            listBoxTransactionReturnReturned.TabIndex = 11;
            listBoxTransactionReturnReturned.ValueMember = "AccessionNumber";
            // 
            // listBoxTransactionReturnOnLoan
            // 
            listBoxTransactionReturnOnLoan.BackColor = System.Drawing.Color.Pink;
            listBoxTransactionReturnOnLoan.DataSource = collectionSpecimenPartOnLoanListBindingSource;
            listBoxTransactionReturnOnLoan.DisplayMember = "DisplayText";
            listBoxTransactionReturnOnLoan.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxTransactionReturnOnLoan.FormattingEnabled = true;
            listBoxTransactionReturnOnLoan.IntegralHeight = false;
            listBoxTransactionReturnOnLoan.ItemHeight = 15;
            listBoxTransactionReturnOnLoan.Location = new System.Drawing.Point(3, 18);
            listBoxTransactionReturnOnLoan.Name = "listBoxTransactionReturnOnLoan";
            listBoxTransactionReturnOnLoan.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            listBoxTransactionReturnOnLoan.Size = new System.Drawing.Size(172, 148);
            listBoxTransactionReturnOnLoan.TabIndex = 15;
            listBoxTransactionReturnOnLoan.ValueMember = "SpecimenPartID";
            // 
            // checkBoxReturnSingleLines
            // 
            checkBoxReturnSingleLines.AutoSize = true;
            checkBoxReturnSingleLines.Checked = true;
            checkBoxReturnSingleLines.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxReturnSingleLines.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxReturnSingleLines.Location = new System.Drawing.Point(106, 29);
            checkBoxReturnSingleLines.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            checkBoxReturnSingleLines.Name = "checkBoxReturnSingleLines";
            checkBoxReturnSingleLines.Size = new System.Drawing.Size(85, 25);
            checkBoxReturnSingleLines.TabIndex = 14;
            checkBoxReturnSingleLines.Text = "Single lines";
            checkBoxReturnSingleLines.UseVisualStyleBackColor = true;
            // 
            // checkBoxReturnOrderByTaxa
            // 
            checkBoxReturnOrderByTaxa.AutoSize = true;
            checkBoxReturnOrderByTaxa.Checked = true;
            checkBoxReturnOrderByTaxa.CheckState = System.Windows.Forms.CheckState.Checked;
            tableLayoutPanelTransactionReturn.SetColumnSpan(checkBoxReturnOrderByTaxa, 2);
            checkBoxReturnOrderByTaxa.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxReturnOrderByTaxa.Location = new System.Drawing.Point(3, 29);
            checkBoxReturnOrderByTaxa.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            checkBoxReturnOrderByTaxa.Name = "checkBoxReturnOrderByTaxa";
            checkBoxReturnOrderByTaxa.Size = new System.Drawing.Size(97, 25);
            checkBoxReturnOrderByTaxa.TabIndex = 15;
            checkBoxReturnOrderByTaxa.Text = "Order by taxa";
            checkBoxReturnOrderByTaxa.UseVisualStyleBackColor = true;
            // 
            // checkBoxReturnIncludeType
            // 
            checkBoxReturnIncludeType.AutoSize = true;
            checkBoxReturnIncludeType.Checked = true;
            checkBoxReturnIncludeType.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxReturnIncludeType.Dock = System.Windows.Forms.DockStyle.Left;
            checkBoxReturnIncludeType.Location = new System.Drawing.Point(197, 32);
            checkBoxReturnIncludeType.Name = "checkBoxReturnIncludeType";
            checkBoxReturnIncludeType.Size = new System.Drawing.Size(79, 19);
            checkBoxReturnIncludeType.TabIndex = 16;
            checkBoxReturnIncludeType.Text = "Type infos";
            checkBoxReturnIncludeType.UseVisualStyleBackColor = true;
            // 
            // tabPageBalance
            // 
            tabPageBalance.Controls.Add(tableLayoutPanelBalance);
            helpProvider.SetHelpKeyword(tabPageBalance, "Transaction balance");
            helpProvider.SetHelpNavigator(tabPageBalance, System.Windows.Forms.HelpNavigator.KeywordIndex);
            tabPageBalance.ImageIndex = 8;
            tabPageBalance.Location = new System.Drawing.Point(4, 24);
            tabPageBalance.Name = "tabPageBalance";
            helpProvider.SetShowHelp(tabPageBalance, true);
            tabPageBalance.Size = new System.Drawing.Size(839, 409);
            tabPageBalance.TabIndex = 8;
            tabPageBalance.Text = "Balance";
            tabPageBalance.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelBalance
            // 
            tableLayoutPanelBalance.ColumnCount = 4;
            tableLayoutPanelBalance.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelBalance.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelBalance.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelBalance.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelBalance.Controls.Add(labelBalanceSchema, 0, 0);
            tableLayoutPanelBalance.Controls.Add(textBoxBalanceSchema, 1, 0);
            tableLayoutPanelBalance.Controls.Add(toolStripBalance, 3, 0);
            tableLayoutPanelBalance.Controls.Add(splitContainerBalance, 0, 4);
            tableLayoutPanelBalance.Controls.Add(checkBoxBalanceIncludeFromSubcollections, 0, 2);
            tableLayoutPanelBalance.Controls.Add(checkBoxBalanceIncludeToSubcollections, 0, 3);
            tableLayoutPanelBalance.Controls.Add(checkBoxBalanceFromIncludeAllCollections, 2, 2);
            tableLayoutPanelBalance.Controls.Add(checkBoxBalanceToIncludeAllCollections, 2, 3);
            tableLayoutPanelBalance.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(tableLayoutPanelBalance, "Transaction balance");
            helpProvider.SetHelpNavigator(tableLayoutPanelBalance, System.Windows.Forms.HelpNavigator.KeywordIndex);
            tableLayoutPanelBalance.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelBalance.Name = "tableLayoutPanelBalance";
            tableLayoutPanelBalance.RowCount = 5;
            tableLayoutPanelBalance.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelBalance.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelBalance.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelBalance.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelBalance.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            helpProvider.SetShowHelp(tableLayoutPanelBalance, true);
            tableLayoutPanelBalance.Size = new System.Drawing.Size(839, 409);
            tableLayoutPanelBalance.TabIndex = 2;
            // 
            // labelBalanceSchema
            // 
            labelBalanceSchema.AutoSize = true;
            labelBalanceSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            labelBalanceSchema.Location = new System.Drawing.Point(3, 0);
            labelBalanceSchema.Name = "labelBalanceSchema";
            labelBalanceSchema.Size = new System.Drawing.Size(71, 29);
            labelBalanceSchema.TabIndex = 2;
            labelBalanceSchema.Text = "Schema file:";
            labelBalanceSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxBalanceSchema
            // 
            tableLayoutPanelBalance.SetColumnSpan(textBoxBalanceSchema, 2);
            textBoxBalanceSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxBalanceSchema.Location = new System.Drawing.Point(80, 3);
            textBoxBalanceSchema.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            textBoxBalanceSchema.Name = "textBoxBalanceSchema";
            textBoxBalanceSchema.Size = new System.Drawing.Size(657, 23);
            textBoxBalanceSchema.TabIndex = 1;
            // 
            // toolStripBalance
            // 
            toolStripBalance.BackColor = System.Drawing.SystemColors.ControlLightLight;
            toolStripBalance.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStripBalance.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonBalanceNew, toolStripButtonBalanceOpenSchema, toolStripSeparatorBalance, toolStripButtonBalanceCreatePreview, toolStripButtonBalanceSetPage, toolStripButtonBalancePrint, toolStripButtonBalanceSave });
            toolStripBalance.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            toolStripBalance.Location = new System.Drawing.Point(737, 0);
            toolStripBalance.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            toolStripBalance.Name = "toolStripBalance";
            toolStripBalance.Size = new System.Drawing.Size(99, 29);
            toolStripBalance.TabIndex = 0;
            toolStripBalance.Text = "toolStrip1";
            // 
            // toolStripButtonBalanceNew
            // 
            toolStripButtonBalanceNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonBalanceNew.Image = Resource.New1;
            toolStripButtonBalanceNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonBalanceNew.Name = "toolStripButtonBalanceNew";
            toolStripButtonBalanceNew.Size = new System.Drawing.Size(23, 20);
            toolStripButtonBalanceNew.Text = "Create a new schema file";
            toolStripButtonBalanceNew.Visible = false;
            // 
            // toolStripButtonBalanceOpenSchema
            // 
            toolStripButtonBalanceOpenSchema.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonBalanceOpenSchema.Image = Resource.Open;
            toolStripButtonBalanceOpenSchema.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonBalanceOpenSchema.Name = "toolStripButtonBalanceOpenSchema";
            toolStripButtonBalanceOpenSchema.Size = new System.Drawing.Size(23, 20);
            toolStripButtonBalanceOpenSchema.Text = "Select schema file (XSD)";
            toolStripButtonBalanceOpenSchema.Click += toolStripButtonBalanceOpenSchema_Click;
            // 
            // toolStripSeparatorBalance
            // 
            toolStripSeparatorBalance.Name = "toolStripSeparatorBalance";
            toolStripSeparatorBalance.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonBalanceCreatePreview
            // 
            toolStripButtonBalanceCreatePreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonBalanceCreatePreview.Image = Resource.List;
            toolStripButtonBalanceCreatePreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonBalanceCreatePreview.Name = "toolStripButtonBalanceCreatePreview";
            toolStripButtonBalanceCreatePreview.Size = new System.Drawing.Size(23, 20);
            toolStripButtonBalanceCreatePreview.Text = "Create a preview";
            toolStripButtonBalanceCreatePreview.Click += toolStripButtonBalanceCreatePreview_Click;
            // 
            // toolStripButtonBalanceSetPage
            // 
            toolStripButtonBalanceSetPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonBalanceSetPage.Image = Resource.PageSetup;
            toolStripButtonBalanceSetPage.ImageTransparentColor = System.Drawing.Color.Purple;
            toolStripButtonBalanceSetPage.Name = "toolStripButtonBalanceSetPage";
            toolStripButtonBalanceSetPage.Size = new System.Drawing.Size(23, 20);
            toolStripButtonBalanceSetPage.Text = "Page setup";
            toolStripButtonBalanceSetPage.Visible = false;
            // 
            // toolStripButtonBalancePrint
            // 
            toolStripButtonBalancePrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonBalancePrint.Image = Resource.Print;
            toolStripButtonBalancePrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonBalancePrint.Name = "toolStripButtonBalancePrint";
            toolStripButtonBalancePrint.Size = new System.Drawing.Size(23, 20);
            toolStripButtonBalancePrint.Text = "Print the created preview";
            toolStripButtonBalancePrint.Click += toolStripButtonBalancePrint_Click;
            // 
            // toolStripButtonBalanceSave
            // 
            toolStripButtonBalanceSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonBalanceSave.Image = Resource.Save;
            toolStripButtonBalanceSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButtonBalanceSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonBalanceSave.Name = "toolStripButtonBalanceSave";
            toolStripButtonBalanceSave.Size = new System.Drawing.Size(23, 18);
            toolStripButtonBalanceSave.Text = "Save document";
            toolStripButtonBalanceSave.Click += toolStripButtonBalanceSave_Click;
            // 
            // splitContainerBalance
            // 
            tableLayoutPanelBalance.SetColumnSpan(splitContainerBalance, 4);
            splitContainerBalance.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerBalance.Location = new System.Drawing.Point(3, 82);
            splitContainerBalance.Name = "splitContainerBalance";
            // 
            // splitContainerBalance.Panel1
            // 
            splitContainerBalance.Panel1.Controls.Add(panelBalance);
            splitContainerBalance.Panel2Collapsed = true;
            splitContainerBalance.Size = new System.Drawing.Size(833, 324);
            splitContainerBalance.SplitterDistance = 579;
            splitContainerBalance.TabIndex = 4;
            // 
            // panelBalance
            // 
            panelBalance.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panelBalance.Controls.Add(webBrowserBalance);
            panelBalance.Dock = System.Windows.Forms.DockStyle.Fill;
            panelBalance.Location = new System.Drawing.Point(0, 0);
            panelBalance.Name = "panelBalance";
            panelBalance.Padding = new System.Windows.Forms.Padding(3);
            panelBalance.Size = new System.Drawing.Size(833, 324);
            panelBalance.TabIndex = 3;
            // 
            // webBrowserBalance
            // 
            webBrowserBalance.Dock = System.Windows.Forms.DockStyle.Fill;
            webBrowserBalance.Location = new System.Drawing.Point(3, 3);
            webBrowserBalance.MinimumSize = new System.Drawing.Size(20, 20);
            webBrowserBalance.Name = "webBrowserBalance";
            webBrowserBalance.Size = new System.Drawing.Size(823, 314);
            webBrowserBalance.TabIndex = 1;
            // 
            // checkBoxBalanceIncludeFromSubcollections
            // 
            checkBoxBalanceIncludeFromSubcollections.AutoSize = true;
            tableLayoutPanelBalance.SetColumnSpan(checkBoxBalanceIncludeFromSubcollections, 2);
            checkBoxBalanceIncludeFromSubcollections.Dock = System.Windows.Forms.DockStyle.Left;
            checkBoxBalanceIncludeFromSubcollections.Location = new System.Drawing.Point(3, 32);
            checkBoxBalanceIncludeFromSubcollections.Name = "checkBoxBalanceIncludeFromSubcollections";
            checkBoxBalanceIncludeFromSubcollections.Size = new System.Drawing.Size(161, 19);
            checkBoxBalanceIncludeFromSubcollections.TabIndex = 5;
            checkBoxBalanceIncludeFromSubcollections.Text = "Include subcollections of ";
            checkBoxBalanceIncludeFromSubcollections.UseVisualStyleBackColor = true;
            checkBoxBalanceIncludeFromSubcollections.Visible = false;
            // 
            // checkBoxBalanceIncludeToSubcollections
            // 
            checkBoxBalanceIncludeToSubcollections.AutoSize = true;
            tableLayoutPanelBalance.SetColumnSpan(checkBoxBalanceIncludeToSubcollections, 2);
            checkBoxBalanceIncludeToSubcollections.Dock = System.Windows.Forms.DockStyle.Left;
            checkBoxBalanceIncludeToSubcollections.Location = new System.Drawing.Point(3, 57);
            checkBoxBalanceIncludeToSubcollections.Name = "checkBoxBalanceIncludeToSubcollections";
            checkBoxBalanceIncludeToSubcollections.Size = new System.Drawing.Size(158, 19);
            checkBoxBalanceIncludeToSubcollections.TabIndex = 7;
            checkBoxBalanceIncludeToSubcollections.Text = "Include subcollections of";
            checkBoxBalanceIncludeToSubcollections.UseVisualStyleBackColor = true;
            checkBoxBalanceIncludeToSubcollections.Visible = false;
            // 
            // checkBoxBalanceFromIncludeAllCollections
            // 
            checkBoxBalanceFromIncludeAllCollections.AutoSize = true;
            checkBoxBalanceFromIncludeAllCollections.Checked = true;
            checkBoxBalanceFromIncludeAllCollections.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxBalanceFromIncludeAllCollections.Location = new System.Drawing.Point(410, 32);
            checkBoxBalanceFromIncludeAllCollections.Name = "checkBoxBalanceFromIncludeAllCollections";
            checkBoxBalanceFromIncludeAllCollections.Size = new System.Drawing.Size(154, 19);
            checkBoxBalanceFromIncludeAllCollections.TabIndex = 8;
            checkBoxBalanceFromIncludeAllCollections.Text = "Include all collections of";
            checkBoxBalanceFromIncludeAllCollections.UseVisualStyleBackColor = true;
            checkBoxBalanceFromIncludeAllCollections.Visible = false;
            // 
            // checkBoxBalanceToIncludeAllCollections
            // 
            checkBoxBalanceToIncludeAllCollections.AutoSize = true;
            checkBoxBalanceToIncludeAllCollections.Checked = true;
            checkBoxBalanceToIncludeAllCollections.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxBalanceToIncludeAllCollections.Location = new System.Drawing.Point(410, 57);
            checkBoxBalanceToIncludeAllCollections.Name = "checkBoxBalanceToIncludeAllCollections";
            checkBoxBalanceToIncludeAllCollections.Size = new System.Drawing.Size(154, 19);
            checkBoxBalanceToIncludeAllCollections.TabIndex = 9;
            checkBoxBalanceToIncludeAllCollections.Text = "Include all collections of";
            checkBoxBalanceToIncludeAllCollections.UseVisualStyleBackColor = true;
            checkBoxBalanceToIncludeAllCollections.Visible = false;
            // 
            // tabPageRequest
            // 
            tabPageRequest.Controls.Add(splitContainerRequest);
            tabPageRequest.ImageIndex = 10;
            tabPageRequest.Location = new System.Drawing.Point(4, 24);
            tabPageRequest.Name = "tabPageRequest";
            tabPageRequest.Padding = new System.Windows.Forms.Padding(3);
            tabPageRequest.Size = new System.Drawing.Size(839, 409);
            tabPageRequest.TabIndex = 10;
            tabPageRequest.Text = "Request";
            tabPageRequest.UseVisualStyleBackColor = true;
            // 
            // splitContainerRequest
            // 
            splitContainerRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerRequest.Location = new System.Drawing.Point(3, 3);
            splitContainerRequest.Name = "splitContainerRequest";
            // 
            // splitContainerRequest.Panel1
            // 
            splitContainerRequest.Panel1.Controls.Add(panelRequest);
            splitContainerRequest.Panel1.Controls.Add(tableLayoutPanelRequest);
            // 
            // splitContainerRequest.Panel2
            // 
            splitContainerRequest.Panel2.Controls.Add(tableLayoutPanelRequestList);
            splitContainerRequest.Panel2.Enabled = false;
            splitContainerRequest.Size = new System.Drawing.Size(833, 403);
            splitContainerRequest.SplitterDistance = 674;
            splitContainerRequest.TabIndex = 5;
            // 
            // panelRequest
            // 
            panelRequest.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panelRequest.Controls.Add(webBrowserRequest);
            panelRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            panelRequest.Location = new System.Drawing.Point(0, 87);
            panelRequest.Name = "panelRequest";
            panelRequest.Padding = new System.Windows.Forms.Padding(3);
            panelRequest.Size = new System.Drawing.Size(674, 316);
            panelRequest.TabIndex = 3;
            // 
            // webBrowserRequest
            // 
            webBrowserRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            webBrowserRequest.Location = new System.Drawing.Point(3, 3);
            webBrowserRequest.MinimumSize = new System.Drawing.Size(20, 20);
            webBrowserRequest.Name = "webBrowserRequest";
            webBrowserRequest.Size = new System.Drawing.Size(664, 306);
            webBrowserRequest.TabIndex = 1;
            // 
            // tableLayoutPanelRequest
            // 
            tableLayoutPanelRequest.ColumnCount = 7;
            tableLayoutPanelRequest.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelRequest.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelRequest.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelRequest.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelRequest.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            tableLayoutPanelRequest.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelRequest.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelRequest.Controls.Add(labelRequestSchema, 0, 3);
            tableLayoutPanelRequest.Controls.Add(textBoxRequestSchema, 2, 3);
            tableLayoutPanelRequest.Controls.Add(toolStripRequest, 6, 3);
            tableLayoutPanelRequest.Controls.Add(labelRequestFrom, 0, 1);
            tableLayoutPanelRequest.Controls.Add(labelRequestTo, 0, 2);
            tableLayoutPanelRequest.Controls.Add(comboBoxRequestFrom, 1, 1);
            tableLayoutPanelRequest.Controls.Add(comboBoxRequestTo, 1, 2);
            tableLayoutPanelRequest.Controls.Add(labelRequestNumber, 4, 0);
            tableLayoutPanelRequest.Controls.Add(textBoxRequestNumber, 4, 2);
            tableLayoutPanelRequest.Controls.Add(labelRequestCollection, 1, 0);
            tableLayoutPanelRequest.Controls.Add(userControlModuleRelatedEntryRequestTo, 5, 2);
            tableLayoutPanelRequest.Controls.Add(labelRequestAgents, 5, 0);
            tableLayoutPanelRequest.Controls.Add(userControlModuleRelatedEntryRequestFrom, 5, 1);
            tableLayoutPanelRequest.Dock = System.Windows.Forms.DockStyle.Top;
            helpProvider.SetHelpKeyword(tableLayoutPanelRequest, "Transaction printing");
            helpProvider.SetHelpNavigator(tableLayoutPanelRequest, System.Windows.Forms.HelpNavigator.KeywordIndex);
            tableLayoutPanelRequest.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelRequest.Name = "tableLayoutPanelRequest";
            tableLayoutPanelRequest.RowCount = 4;
            tableLayoutPanelRequest.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelRequest.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelRequest.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelRequest.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            helpProvider.SetShowHelp(tableLayoutPanelRequest, true);
            tableLayoutPanelRequest.Size = new System.Drawing.Size(674, 87);
            tableLayoutPanelRequest.TabIndex = 1;
            // 
            // labelRequestSchema
            // 
            labelRequestSchema.AutoSize = true;
            tableLayoutPanelRequest.SetColumnSpan(labelRequestSchema, 2);
            labelRequestSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            labelRequestSchema.Location = new System.Drawing.Point(3, 71);
            labelRequestSchema.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            labelRequestSchema.Name = "labelRequestSchema";
            labelRequestSchema.Size = new System.Drawing.Size(71, 16);
            labelRequestSchema.TabIndex = 2;
            labelRequestSchema.Text = "Schema:";
            labelRequestSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxRequestSchema
            // 
            tableLayoutPanelRequest.SetColumnSpan(textBoxRequestSchema, 4);
            textBoxRequestSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxRequestSchema.Location = new System.Drawing.Point(74, 74);
            textBoxRequestSchema.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            textBoxRequestSchema.Name = "textBoxRequestSchema";
            textBoxRequestSchema.Size = new System.Drawing.Size(546, 23);
            textBoxRequestSchema.TabIndex = 1;
            // 
            // toolStripRequest
            // 
            toolStripRequest.BackColor = System.Drawing.SystemColors.Control;
            toolStripRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStripRequest.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonRequestOpenSchema, toolStripSeparatorRequest, toolStripButtonRequestShowList, toolStripButtonRequestPreview, toolStripButtonRequestPrint, toolStripButtonRequestSave, toolStripButtonRequestScanner });
            toolStripRequest.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            toolStripRequest.Location = new System.Drawing.Point(620, 71);
            toolStripRequest.Name = "toolStripRequest";
            toolStripRequest.Size = new System.Drawing.Size(54, 16);
            toolStripRequest.TabIndex = 0;
            toolStripRequest.Text = "toolStrip1";
            // 
            // toolStripButtonRequestOpenSchema
            // 
            toolStripButtonRequestOpenSchema.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonRequestOpenSchema.Image = Resource.Open;
            toolStripButtonRequestOpenSchema.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonRequestOpenSchema.Name = "toolStripButtonRequestOpenSchema";
            toolStripButtonRequestOpenSchema.Size = new System.Drawing.Size(23, 20);
            toolStripButtonRequestOpenSchema.Text = "Select schema file (XSD)";
            toolStripButtonRequestOpenSchema.Click += toolStripButtonRequestOpenSchema_Click;
            // 
            // toolStripSeparatorRequest
            // 
            toolStripSeparatorRequest.Name = "toolStripSeparatorRequest";
            toolStripSeparatorRequest.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonRequestShowList
            // 
            toolStripButtonRequestShowList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonRequestShowList.Image = Resource.ShowList;
            toolStripButtonRequestShowList.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonRequestShowList.Name = "toolStripButtonRequestShowList";
            toolStripButtonRequestShowList.Size = new System.Drawing.Size(23, 20);
            toolStripButtonRequestShowList.Text = "toolStripButton1";
            toolStripButtonRequestShowList.ToolTipText = "List the requested samples";
            toolStripButtonRequestShowList.Click += toolStripButtonRequestShowList_Click;
            // 
            // toolStripButtonRequestPreview
            // 
            toolStripButtonRequestPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonRequestPreview.Image = Resource.List;
            toolStripButtonRequestPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonRequestPreview.Name = "toolStripButtonRequestPreview";
            toolStripButtonRequestPreview.Size = new System.Drawing.Size(23, 20);
            toolStripButtonRequestPreview.Text = "Create a preview";
            toolStripButtonRequestPreview.Visible = false;
            toolStripButtonRequestPreview.Click += toolStripButtonRequestPreview_Click;
            // 
            // toolStripButtonRequestPrint
            // 
            toolStripButtonRequestPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonRequestPrint.Image = Resource.Print;
            toolStripButtonRequestPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonRequestPrint.Name = "toolStripButtonRequestPrint";
            toolStripButtonRequestPrint.Size = new System.Drawing.Size(23, 20);
            toolStripButtonRequestPrint.Text = "Print the created document";
            toolStripButtonRequestPrint.Visible = false;
            toolStripButtonRequestPrint.Click += toolStripButtonRequestPrint_Click;
            // 
            // toolStripButtonRequestSave
            // 
            toolStripButtonRequestSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonRequestSave.Image = Resource.Save;
            toolStripButtonRequestSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButtonRequestSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonRequestSave.Name = "toolStripButtonRequestSave";
            toolStripButtonRequestSave.Size = new System.Drawing.Size(23, 18);
            toolStripButtonRequestSave.Text = "Save the document";
            toolStripButtonRequestSave.Visible = false;
            toolStripButtonRequestSave.Click += toolStripButtonRequestSave_Click;
            // 
            // toolStripButtonRequestScanner
            // 
            toolStripButtonRequestScanner.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonRequestScanner.Image = Resource.ScannerBarcode;
            toolStripButtonRequestScanner.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonRequestScanner.Name = "toolStripButtonRequestScanner";
            toolStripButtonRequestScanner.Size = new System.Drawing.Size(23, 20);
            toolStripButtonRequestScanner.Text = "toolStripButton1";
            toolStripButtonRequestScanner.Visible = false;
            toolStripButtonRequestScanner.Click += toolStripButtonRequestScanner_Click;
            // 
            // labelRequestFrom
            // 
            labelRequestFrom.AutoSize = true;
            labelRequestFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            labelRequestFrom.Location = new System.Drawing.Point(3, 15);
            labelRequestFrom.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            labelRequestFrom.Name = "labelRequestFrom";
            labelRequestFrom.Size = new System.Drawing.Size(38, 30);
            labelRequestFrom.TabIndex = 3;
            labelRequestFrom.Text = "From:";
            labelRequestFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelRequestTo
            // 
            labelRequestTo.AutoSize = true;
            labelRequestTo.Dock = System.Windows.Forms.DockStyle.Fill;
            labelRequestTo.Location = new System.Drawing.Point(3, 45);
            labelRequestTo.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            labelRequestTo.Name = "labelRequestTo";
            labelRequestTo.Size = new System.Drawing.Size(38, 26);
            labelRequestTo.TabIndex = 4;
            labelRequestTo.Text = "To:";
            labelRequestTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxRequestFrom
            // 
            tableLayoutPanelRequest.SetColumnSpan(comboBoxRequestFrom, 2);
            comboBoxRequestFrom.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", transactionBindingSource, "FromCollectionID", true));
            comboBoxRequestFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxRequestFrom.FormattingEnabled = true;
            comboBoxRequestFrom.Location = new System.Drawing.Point(41, 18);
            comboBoxRequestFrom.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            comboBoxRequestFrom.Name = "comboBoxRequestFrom";
            comboBoxRequestFrom.Size = new System.Drawing.Size(264, 23);
            comboBoxRequestFrom.TabIndex = 5;
            // 
            // comboBoxRequestTo
            // 
            tableLayoutPanelRequest.SetColumnSpan(comboBoxRequestTo, 2);
            comboBoxRequestTo.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", transactionBindingSource, "ToCollectionID", true));
            comboBoxRequestTo.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxRequestTo.FormattingEnabled = true;
            comboBoxRequestTo.Location = new System.Drawing.Point(41, 48);
            comboBoxRequestTo.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            comboBoxRequestTo.Name = "comboBoxRequestTo";
            comboBoxRequestTo.Size = new System.Drawing.Size(264, 23);
            comboBoxRequestTo.TabIndex = 6;
            // 
            // labelRequestNumber
            // 
            labelRequestNumber.AutoSize = true;
            labelRequestNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            labelRequestNumber.Location = new System.Drawing.Point(312, 0);
            labelRequestNumber.Name = "labelRequestNumber";
            tableLayoutPanelRequest.SetRowSpan(labelRequestNumber, 2);
            labelRequestNumber.Size = new System.Drawing.Size(74, 45);
            labelRequestNumber.TabIndex = 10;
            labelRequestNumber.Text = "Loan number of requester";
            labelRequestNumber.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxRequestNumber
            // 
            textBoxRequestNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionBindingSource, "ToTransactionNumber", true));
            textBoxRequestNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxRequestNumber.Location = new System.Drawing.Point(312, 48);
            textBoxRequestNumber.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            textBoxRequestNumber.Name = "textBoxRequestNumber";
            textBoxRequestNumber.Size = new System.Drawing.Size(74, 23);
            textBoxRequestNumber.TabIndex = 11;
            // 
            // labelRequestCollection
            // 
            labelRequestCollection.AutoSize = true;
            tableLayoutPanelRequest.SetColumnSpan(labelRequestCollection, 2);
            labelRequestCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            labelRequestCollection.Location = new System.Drawing.Point(44, 0);
            labelRequestCollection.Name = "labelRequestCollection";
            labelRequestCollection.Size = new System.Drawing.Size(258, 15);
            labelRequestCollection.TabIndex = 12;
            labelRequestCollection.Text = "Collection";
            labelRequestCollection.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // userControlModuleRelatedEntryRequestTo
            // 
            userControlModuleRelatedEntryRequestTo.CanDeleteConnectionToModule = true;
            tableLayoutPanelRequest.SetColumnSpan(userControlModuleRelatedEntryRequestTo, 2);
            userControlModuleRelatedEntryRequestTo.DependsOnUri = "";
            userControlModuleRelatedEntryRequestTo.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleRelatedEntryRequestTo.Domain = "";
            userControlModuleRelatedEntryRequestTo.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryRequestTo.Location = new System.Drawing.Point(392, 48);
            userControlModuleRelatedEntryRequestTo.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            userControlModuleRelatedEntryRequestTo.Module = null;
            userControlModuleRelatedEntryRequestTo.Name = "userControlModuleRelatedEntryRequestTo";
            userControlModuleRelatedEntryRequestTo.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryRequestTo.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryRequestTo.ShowInfo = false;
            userControlModuleRelatedEntryRequestTo.Size = new System.Drawing.Size(279, 23);
            userControlModuleRelatedEntryRequestTo.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryRequestTo.TabIndex = 14;
            // 
            // labelRequestAgents
            // 
            labelRequestAgents.AutoSize = true;
            tableLayoutPanelRequest.SetColumnSpan(labelRequestAgents, 2);
            labelRequestAgents.Dock = System.Windows.Forms.DockStyle.Fill;
            labelRequestAgents.Location = new System.Drawing.Point(392, 0);
            labelRequestAgents.Name = "labelRequestAgents";
            labelRequestAgents.Size = new System.Drawing.Size(279, 15);
            labelRequestAgents.TabIndex = 15;
            labelRequestAgents.Text = "Address if different from collection address";
            labelRequestAgents.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // userControlModuleRelatedEntryRequestFrom
            // 
            userControlModuleRelatedEntryRequestFrom.CanDeleteConnectionToModule = true;
            tableLayoutPanelRequest.SetColumnSpan(userControlModuleRelatedEntryRequestFrom, 2);
            userControlModuleRelatedEntryRequestFrom.DependsOnUri = "";
            userControlModuleRelatedEntryRequestFrom.Domain = "";
            userControlModuleRelatedEntryRequestFrom.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryRequestFrom.Location = new System.Drawing.Point(392, 18);
            userControlModuleRelatedEntryRequestFrom.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            userControlModuleRelatedEntryRequestFrom.Module = null;
            userControlModuleRelatedEntryRequestFrom.Name = "userControlModuleRelatedEntryRequestFrom";
            userControlModuleRelatedEntryRequestFrom.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryRequestFrom.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryRequestFrom.ShowInfo = false;
            userControlModuleRelatedEntryRequestFrom.Size = new System.Drawing.Size(261, 22);
            userControlModuleRelatedEntryRequestFrom.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryRequestFrom.TabIndex = 16;
            // 
            // tableLayoutPanelRequestList
            // 
            tableLayoutPanelRequestList.ColumnCount = 1;
            tableLayoutPanelRequestList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelRequestList.Controls.Add(comboBoxRequestAccessionNumber, 0, 2);
            tableLayoutPanelRequestList.Controls.Add(toolStripSpecimenListRequest, 0, 7);
            tableLayoutPanelRequestList.Controls.Add(listBoxRequest, 0, 6);
            tableLayoutPanelRequestList.Controls.Add(textBoxRequestAccessionNumber, 0, 1);
            tableLayoutPanelRequestList.Controls.Add(labelRequestSpecimenList, 0, 5);
            tableLayoutPanelRequestList.Controls.Add(listBoxRequestOnLoan, 0, 4);
            tableLayoutPanelRequestList.Controls.Add(labelRequestSpecimenOnLoan, 0, 3);
            tableLayoutPanelRequestList.Controls.Add(textBoxRequestType, 0, 0);
            tableLayoutPanelRequestList.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelRequestList.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelRequestList.Name = "tableLayoutPanelRequestList";
            tableLayoutPanelRequestList.RowCount = 8;
            tableLayoutPanelRequestList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelRequestList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelRequestList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelRequestList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelRequestList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelRequestList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelRequestList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelRequestList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelRequestList.Size = new System.Drawing.Size(155, 403);
            tableLayoutPanelRequestList.TabIndex = 13;
            // 
            // comboBoxRequestAccessionNumber
            // 
            comboBoxRequestAccessionNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxRequestAccessionNumber.DropDownWidth = 300;
            comboBoxRequestAccessionNumber.FormattingEnabled = true;
            comboBoxRequestAccessionNumber.Location = new System.Drawing.Point(3, 61);
            comboBoxRequestAccessionNumber.Name = "comboBoxRequestAccessionNumber";
            comboBoxRequestAccessionNumber.Size = new System.Drawing.Size(149, 23);
            comboBoxRequestAccessionNumber.TabIndex = 17;
            comboBoxRequestAccessionNumber.Visible = false;
            comboBoxRequestAccessionNumber.DropDown += comboBoxRequestAccessionNumber_DropDown;
            comboBoxRequestAccessionNumber.SelectionChangeCommitted += comboBoxRequestAccessionNumber_SelectionChangeCommitted;
            // 
            // toolStripSpecimenListRequest
            // 
            toolStripSpecimenListRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStripSpecimenListRequest.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripSpecimenListRequest.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonRequestListDelete, toolStripButtonRequestListFind });
            toolStripSpecimenListRequest.Location = new System.Drawing.Point(3, 375);
            toolStripSpecimenListRequest.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            toolStripSpecimenListRequest.Name = "toolStripSpecimenListRequest";
            toolStripSpecimenListRequest.Size = new System.Drawing.Size(149, 25);
            toolStripSpecimenListRequest.TabIndex = 12;
            toolStripSpecimenListRequest.Text = "toolStrip1";
            // 
            // toolStripButtonRequestListDelete
            // 
            toolStripButtonRequestListDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonRequestListDelete.Image = Resource.Delete;
            toolStripButtonRequestListDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonRequestListDelete.Name = "toolStripButtonRequestListDelete";
            toolStripButtonRequestListDelete.Size = new System.Drawing.Size(23, 22);
            toolStripButtonRequestListDelete.Text = "Remove selected specimen part from the list";
            toolStripButtonRequestListDelete.Click += toolStripButtonRequestListDelete_Click;
            // 
            // toolStripButtonRequestListFind
            // 
            toolStripButtonRequestListFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonRequestListFind.Image = Resource.Search;
            toolStripButtonRequestListFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonRequestListFind.Name = "toolStripButtonRequestListFind";
            toolStripButtonRequestListFind.Size = new System.Drawing.Size(23, 22);
            toolStripButtonRequestListFind.Text = "Show selected specimen";
            toolStripButtonRequestListFind.Click += toolStripButtonRequestListFind_Click;
            // 
            // listBoxRequest
            // 
            listBoxRequest.BackColor = System.Drawing.Color.LightGreen;
            listBoxRequest.DataSource = collectionSpecimenPartReturnListBindingSource;
            listBoxRequest.DisplayMember = "DisplayText";
            listBoxRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxRequest.FormattingEnabled = true;
            listBoxRequest.IntegralHeight = false;
            listBoxRequest.ItemHeight = 15;
            listBoxRequest.Location = new System.Drawing.Point(3, 222);
            listBoxRequest.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            listBoxRequest.Name = "listBoxRequest";
            listBoxRequest.Size = new System.Drawing.Size(149, 153);
            listBoxRequest.Sorted = true;
            listBoxRequest.TabIndex = 1;
            listBoxRequest.ValueMember = "AccessionNumber";
            // 
            // collectionSpecimenPartReturnListBindingSource
            // 
            collectionSpecimenPartReturnListBindingSource.DataMember = "CollectionSpecimenPartReturnList";
            collectionSpecimenPartReturnListBindingSource.DataSource = dataSetTransaction;
            // 
            // textBoxRequestAccessionNumber
            // 
            textBoxRequestAccessionNumber.BackColor = System.Drawing.Color.Pink;
            textBoxRequestAccessionNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            textBoxRequestAccessionNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxRequestAccessionNumber.Location = new System.Drawing.Point(3, 32);
            textBoxRequestAccessionNumber.Name = "textBoxRequestAccessionNumber";
            textBoxRequestAccessionNumber.Size = new System.Drawing.Size(149, 23);
            textBoxRequestAccessionNumber.TabIndex = 9;
            textBoxRequestAccessionNumber.Visible = false;
            textBoxRequestAccessionNumber.TextChanged += textBoxRequestAccessionNumber_TextChanged;
            textBoxRequestAccessionNumber.MouseEnter += textBoxRequestAccessionNumber_MouseEnter;
            // 
            // labelRequestSpecimenList
            // 
            labelRequestSpecimenList.AutoSize = true;
            labelRequestSpecimenList.Dock = System.Windows.Forms.DockStyle.Fill;
            labelRequestSpecimenList.Location = new System.Drawing.Point(3, 204);
            labelRequestSpecimenList.Name = "labelRequestSpecimenList";
            labelRequestSpecimenList.Size = new System.Drawing.Size(149, 15);
            labelRequestSpecimenList.TabIndex = 0;
            labelRequestSpecimenList.Text = "Specimen";
            // 
            // listBoxRequestOnLoan
            // 
            listBoxRequestOnLoan.BackColor = System.Drawing.Color.Pink;
            listBoxRequestOnLoan.DataSource = collectionSpecimenPartOnLoanListBindingSource;
            listBoxRequestOnLoan.DisplayMember = "DisplayText";
            listBoxRequestOnLoan.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxRequestOnLoan.FormattingEnabled = true;
            listBoxRequestOnLoan.IntegralHeight = false;
            listBoxRequestOnLoan.ItemHeight = 15;
            listBoxRequestOnLoan.Location = new System.Drawing.Point(3, 105);
            listBoxRequestOnLoan.Name = "listBoxRequestOnLoan";
            listBoxRequestOnLoan.Size = new System.Drawing.Size(149, 96);
            listBoxRequestOnLoan.Sorted = true;
            listBoxRequestOnLoan.TabIndex = 18;
            listBoxRequestOnLoan.ValueMember = "AccessionNumber";
            listBoxRequestOnLoan.Visible = false;
            // 
            // labelRequestSpecimenOnLoan
            // 
            labelRequestSpecimenOnLoan.AutoSize = true;
            labelRequestSpecimenOnLoan.Dock = System.Windows.Forms.DockStyle.Fill;
            labelRequestSpecimenOnLoan.Location = new System.Drawing.Point(3, 87);
            labelRequestSpecimenOnLoan.Name = "labelRequestSpecimenOnLoan";
            labelRequestSpecimenOnLoan.Size = new System.Drawing.Size(149, 15);
            labelRequestSpecimenOnLoan.TabIndex = 19;
            labelRequestSpecimenOnLoan.Text = "Specimen on loan";
            labelRequestSpecimenOnLoan.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            labelRequestSpecimenOnLoan.Visible = false;
            // 
            // textBoxRequestType
            // 
            textBoxRequestType.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionBindingSource, "TransactionType", true));
            textBoxRequestType.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxRequestType.Location = new System.Drawing.Point(3, 3);
            textBoxRequestType.Name = "textBoxRequestType";
            textBoxRequestType.ReadOnly = true;
            textBoxRequestType.Size = new System.Drawing.Size(149, 23);
            textBoxRequestType.TabIndex = 21;
            textBoxRequestType.TextChanged += textBoxRequestType_TextChanged;
            // 
            // tabPageNoAccess
            // 
            tabPageNoAccess.Controls.Add(tableLayoutPanelNoAccess);
            tabPageNoAccess.ImageIndex = 12;
            tabPageNoAccess.Location = new System.Drawing.Point(4, 24);
            tabPageNoAccess.Name = "tabPageNoAccess";
            tabPageNoAccess.Size = new System.Drawing.Size(839, 409);
            tabPageNoAccess.TabIndex = 13;
            tabPageNoAccess.Text = "NoAccess";
            tabPageNoAccess.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelNoAccess
            // 
            tableLayoutPanelNoAccess.ColumnCount = 3;
            tableLayoutPanelNoAccess.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelNoAccess.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelNoAccess.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelNoAccess.Controls.Add(pictureBoxNoAccess, 1, 1);
            tableLayoutPanelNoAccess.Controls.Add(labelNoAccess, 0, 0);
            tableLayoutPanelNoAccess.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelNoAccess.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelNoAccess.Name = "tableLayoutPanelNoAccess";
            tableLayoutPanelNoAccess.RowCount = 3;
            tableLayoutPanelNoAccess.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelNoAccess.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelNoAccess.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelNoAccess.Size = new System.Drawing.Size(839, 409);
            tableLayoutPanelNoAccess.TabIndex = 0;
            // 
            // pictureBoxNoAccess
            // 
            pictureBoxNoAccess.Image = Resource.NoAccess128;
            pictureBoxNoAccess.Location = new System.Drawing.Point(355, 140);
            pictureBoxNoAccess.Name = "pictureBoxNoAccess";
            pictureBoxNoAccess.Size = new System.Drawing.Size(128, 128);
            pictureBoxNoAccess.TabIndex = 0;
            pictureBoxNoAccess.TabStop = false;
            // 
            // labelNoAccess
            // 
            labelNoAccess.AutoSize = true;
            tableLayoutPanelNoAccess.SetColumnSpan(labelNoAccess, 3);
            labelNoAccess.Dock = System.Windows.Forms.DockStyle.Fill;
            labelNoAccess.ForeColor = System.Drawing.Color.Red;
            labelNoAccess.Location = new System.Drawing.Point(3, 0);
            labelNoAccess.Name = "labelNoAccess";
            labelNoAccess.Size = new System.Drawing.Size(833, 137);
            labelNoAccess.TabIndex = 1;
            labelNoAccess.Text = "You have no access to this dataset";
            labelNoAccess.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPagePayment
            // 
            tabPagePayment.Controls.Add(splitContainerPayment);
            tabPagePayment.ImageIndex = 13;
            tabPagePayment.Location = new System.Drawing.Point(4, 24);
            tabPagePayment.Name = "tabPagePayment";
            tabPagePayment.Size = new System.Drawing.Size(839, 409);
            tabPagePayment.TabIndex = 14;
            tabPagePayment.Text = "Payments";
            tabPagePayment.UseVisualStyleBackColor = true;
            // 
            // splitContainerPayment
            // 
            splitContainerPayment.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(splitContainerPayment, "Transaction payment");
            helpProvider.SetHelpNavigator(splitContainerPayment, System.Windows.Forms.HelpNavigator.KeywordIndex);
            splitContainerPayment.Location = new System.Drawing.Point(0, 0);
            splitContainerPayment.Name = "splitContainerPayment";
            // 
            // splitContainerPayment.Panel1
            // 
            splitContainerPayment.Panel1.Controls.Add(listBoxPayment);
            splitContainerPayment.Panel1.Controls.Add(toolStripPayment);
            // 
            // splitContainerPayment.Panel2
            // 
            splitContainerPayment.Panel2.Controls.Add(tableLayoutPanelPayment);
            splitContainerPayment.Panel2.Enabled = false;
            helpProvider.SetShowHelp(splitContainerPayment, true);
            splitContainerPayment.Size = new System.Drawing.Size(839, 409);
            splitContainerPayment.SplitterDistance = 148;
            splitContainerPayment.TabIndex = 0;
            // 
            // listBoxPayment
            // 
            listBoxPayment.DataSource = transactionPaymentBindingSource;
            listBoxPayment.DisplayMember = "Identifier";
            listBoxPayment.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxPayment.FormattingEnabled = true;
            listBoxPayment.ItemHeight = 15;
            listBoxPayment.Location = new System.Drawing.Point(0, 0);
            listBoxPayment.Name = "listBoxPayment";
            listBoxPayment.Size = new System.Drawing.Size(148, 384);
            listBoxPayment.TabIndex = 1;
            listBoxPayment.ValueMember = "PaymentID";
            listBoxPayment.SelectedIndexChanged += listBoxPayment_SelectedIndexChanged;
            // 
            // transactionPaymentBindingSource
            // 
            transactionPaymentBindingSource.DataMember = "TransactionPayment";
            transactionPaymentBindingSource.DataSource = dataSetTransaction;
            // 
            // toolStripPayment
            // 
            toolStripPayment.Dock = System.Windows.Forms.DockStyle.Bottom;
            toolStripPayment.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripPayment.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonPaymentAdd, toolStripButtonPaymentDelete });
            toolStripPayment.Location = new System.Drawing.Point(0, 384);
            toolStripPayment.Name = "toolStripPayment";
            toolStripPayment.Size = new System.Drawing.Size(148, 25);
            toolStripPayment.TabIndex = 0;
            toolStripPayment.Text = "toolStrip1";
            // 
            // toolStripButtonPaymentAdd
            // 
            toolStripButtonPaymentAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonPaymentAdd.Image = Resource.Add1;
            toolStripButtonPaymentAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonPaymentAdd.Name = "toolStripButtonPaymentAdd";
            toolStripButtonPaymentAdd.Size = new System.Drawing.Size(23, 22);
            toolStripButtonPaymentAdd.Text = "Add a new payment";
            toolStripButtonPaymentAdd.Click += toolStripButtonPaymentAdd_Click;
            // 
            // toolStripButtonPaymentDelete
            // 
            toolStripButtonPaymentDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonPaymentDelete.Image = Resource.Delete;
            toolStripButtonPaymentDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonPaymentDelete.Name = "toolStripButtonPaymentDelete";
            toolStripButtonPaymentDelete.Size = new System.Drawing.Size(23, 22);
            toolStripButtonPaymentDelete.Text = "Delete the selected payment";
            toolStripButtonPaymentDelete.Click += toolStripButtonPaymentDelete_Click;
            // 
            // tableLayoutPanelPayment
            // 
            tableLayoutPanelPayment.ColumnCount = 6;
            tableLayoutPanelPayment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelPayment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelPayment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelPayment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelPayment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelPayment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelPayment.Controls.Add(labelPaymentIdentifier, 0, 0);
            tableLayoutPanelPayment.Controls.Add(textBoxPaymentIdentifier, 1, 0);
            tableLayoutPanelPayment.Controls.Add(labelPaymentAmount, 0, 2);
            tableLayoutPanelPayment.Controls.Add(textBoxPaymentAmount, 2, 2);
            tableLayoutPanelPayment.Controls.Add(labelPaymentCurrency, 3, 2);
            tableLayoutPanelPayment.Controls.Add(labelPaymentForeignAmount, 0, 3);
            tableLayoutPanelPayment.Controls.Add(textBoxPaymentForeignAmount, 2, 3);
            tableLayoutPanelPayment.Controls.Add(textBoxPaymentForeignAmountCurrency, 3, 3);
            tableLayoutPanelPayment.Controls.Add(labelPaymentPayer, 0, 4);
            tableLayoutPanelPayment.Controls.Add(userControlModuleRelatedEntryPaymentPayer, 1, 4);
            tableLayoutPanelPayment.Controls.Add(userControlModuleRelatedEntryPaymentRecipient, 1, 5);
            tableLayoutPanelPayment.Controls.Add(labelPaymentRecipient, 0, 5);
            tableLayoutPanelPayment.Controls.Add(labelPaymentDate, 0, 6);
            tableLayoutPanelPayment.Controls.Add(dateTimePickerPaymentDate, 2, 6);
            tableLayoutPanelPayment.Controls.Add(labelPaymentDateSupplement, 3, 6);
            tableLayoutPanelPayment.Controls.Add(textBoxPaymentDateSupplement, 4, 6);
            tableLayoutPanelPayment.Controls.Add(labelPaymentNotes, 0, 7);
            tableLayoutPanelPayment.Controls.Add(textBoxPaymentNotes, 1, 7);
            tableLayoutPanelPayment.Controls.Add(labelPaymentURI, 0, 1);
            tableLayoutPanelPayment.Controls.Add(textBoxPaymentURI, 1, 1);
            tableLayoutPanelPayment.Controls.Add(buttonPaymentURI, 5, 1);
            tableLayoutPanelPayment.Controls.Add(buttonPaymentDate, 1, 6);
            tableLayoutPanelPayment.Controls.Add(buttonSavePayment, 5, 0);
            tableLayoutPanelPayment.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelPayment.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelPayment.Name = "tableLayoutPanelPayment";
            tableLayoutPanelPayment.RowCount = 8;
            tableLayoutPanelPayment.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelPayment.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelPayment.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelPayment.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelPayment.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelPayment.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelPayment.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelPayment.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelPayment.Size = new System.Drawing.Size(687, 409);
            tableLayoutPanelPayment.TabIndex = 0;
            // 
            // labelPaymentIdentifier
            // 
            labelPaymentIdentifier.AutoSize = true;
            labelPaymentIdentifier.Dock = System.Windows.Forms.DockStyle.Fill;
            labelPaymentIdentifier.Location = new System.Drawing.Point(3, 0);
            labelPaymentIdentifier.Name = "labelPaymentIdentifier";
            labelPaymentIdentifier.Size = new System.Drawing.Size(73, 29);
            labelPaymentIdentifier.TabIndex = 0;
            labelPaymentIdentifier.Text = "Identifier:";
            labelPaymentIdentifier.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPaymentIdentifier
            // 
            tableLayoutPanelPayment.SetColumnSpan(textBoxPaymentIdentifier, 4);
            textBoxPaymentIdentifier.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionPaymentBindingSource, "Identifier", true));
            textBoxPaymentIdentifier.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxPaymentIdentifier.Location = new System.Drawing.Point(82, 3);
            textBoxPaymentIdentifier.Name = "textBoxPaymentIdentifier";
            textBoxPaymentIdentifier.Size = new System.Drawing.Size(573, 23);
            textBoxPaymentIdentifier.TabIndex = 0;
            // 
            // labelPaymentAmount
            // 
            labelPaymentAmount.AutoSize = true;
            tableLayoutPanelPayment.SetColumnSpan(labelPaymentAmount, 2);
            labelPaymentAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            labelPaymentAmount.Location = new System.Drawing.Point(3, 58);
            labelPaymentAmount.Name = "labelPaymentAmount";
            labelPaymentAmount.Size = new System.Drawing.Size(110, 29);
            labelPaymentAmount.TabIndex = 2;
            labelPaymentAmount.Text = "Amount:";
            labelPaymentAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPaymentAmount
            // 
            textBoxPaymentAmount.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionPaymentBindingSource, "Amount", true));
            textBoxPaymentAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxPaymentAmount.Location = new System.Drawing.Point(119, 61);
            textBoxPaymentAmount.Name = "textBoxPaymentAmount";
            textBoxPaymentAmount.Size = new System.Drawing.Size(200, 23);
            textBoxPaymentAmount.TabIndex = 2;
            textBoxPaymentAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            textBoxPaymentAmount.Validating += textBoxPaymentAmount_Validating;
            // 
            // labelPaymentCurrency
            // 
            labelPaymentCurrency.AutoSize = true;
            labelPaymentCurrency.Dock = System.Windows.Forms.DockStyle.Left;
            labelPaymentCurrency.Location = new System.Drawing.Point(325, 58);
            labelPaymentCurrency.Name = "labelPaymentCurrency";
            labelPaymentCurrency.Size = new System.Drawing.Size(13, 29);
            labelPaymentCurrency.TabIndex = 4;
            labelPaymentCurrency.Text = "€";
            labelPaymentCurrency.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelPaymentForeignAmount
            // 
            labelPaymentForeignAmount.AutoSize = true;
            tableLayoutPanelPayment.SetColumnSpan(labelPaymentForeignAmount, 2);
            labelPaymentForeignAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            labelPaymentForeignAmount.Location = new System.Drawing.Point(3, 87);
            labelPaymentForeignAmount.Name = "labelPaymentForeignAmount";
            labelPaymentForeignAmount.Size = new System.Drawing.Size(110, 29);
            labelPaymentForeignAmount.TabIndex = 5;
            labelPaymentForeignAmount.Text = "in foreign currency:";
            labelPaymentForeignAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPaymentForeignAmount
            // 
            textBoxPaymentForeignAmount.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionPaymentBindingSource, "ForeignAmount", true));
            textBoxPaymentForeignAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxPaymentForeignAmount.Location = new System.Drawing.Point(119, 90);
            textBoxPaymentForeignAmount.Name = "textBoxPaymentForeignAmount";
            textBoxPaymentForeignAmount.Size = new System.Drawing.Size(200, 23);
            textBoxPaymentForeignAmount.TabIndex = 3;
            textBoxPaymentForeignAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            textBoxPaymentForeignAmount.Validating += textBoxPaymentForeignAmount_Validating;
            // 
            // textBoxPaymentForeignAmountCurrency
            // 
            tableLayoutPanelPayment.SetColumnSpan(textBoxPaymentForeignAmountCurrency, 2);
            textBoxPaymentForeignAmountCurrency.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionPaymentBindingSource, "ForeignCurrency", true));
            textBoxPaymentForeignAmountCurrency.Dock = System.Windows.Forms.DockStyle.Left;
            textBoxPaymentForeignAmountCurrency.Location = new System.Drawing.Point(325, 90);
            textBoxPaymentForeignAmountCurrency.Name = "textBoxPaymentForeignAmountCurrency";
            textBoxPaymentForeignAmountCurrency.Size = new System.Drawing.Size(87, 23);
            textBoxPaymentForeignAmountCurrency.TabIndex = 4;
            // 
            // labelPaymentPayer
            // 
            labelPaymentPayer.AutoSize = true;
            labelPaymentPayer.Dock = System.Windows.Forms.DockStyle.Fill;
            labelPaymentPayer.Location = new System.Drawing.Point(3, 116);
            labelPaymentPayer.Name = "labelPaymentPayer";
            labelPaymentPayer.Size = new System.Drawing.Size(73, 28);
            labelPaymentPayer.TabIndex = 8;
            labelPaymentPayer.Text = "Payed by:";
            labelPaymentPayer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlModuleRelatedEntryPaymentPayer
            // 
            userControlModuleRelatedEntryPaymentPayer.CanDeleteConnectionToModule = true;
            tableLayoutPanelPayment.SetColumnSpan(userControlModuleRelatedEntryPaymentPayer, 5);
            userControlModuleRelatedEntryPaymentPayer.DependsOnUri = "";
            userControlModuleRelatedEntryPaymentPayer.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleRelatedEntryPaymentPayer.Domain = "";
            userControlModuleRelatedEntryPaymentPayer.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryPaymentPayer.Location = new System.Drawing.Point(83, 119);
            userControlModuleRelatedEntryPaymentPayer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            userControlModuleRelatedEntryPaymentPayer.Module = null;
            userControlModuleRelatedEntryPaymentPayer.Name = "userControlModuleRelatedEntryPaymentPayer";
            userControlModuleRelatedEntryPaymentPayer.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryPaymentPayer.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryPaymentPayer.ShowInfo = false;
            userControlModuleRelatedEntryPaymentPayer.Size = new System.Drawing.Size(600, 22);
            userControlModuleRelatedEntryPaymentPayer.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryPaymentPayer.TabIndex = 5;
            // 
            // userControlModuleRelatedEntryPaymentRecipient
            // 
            userControlModuleRelatedEntryPaymentRecipient.CanDeleteConnectionToModule = true;
            tableLayoutPanelPayment.SetColumnSpan(userControlModuleRelatedEntryPaymentRecipient, 5);
            userControlModuleRelatedEntryPaymentRecipient.DependsOnUri = "";
            userControlModuleRelatedEntryPaymentRecipient.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleRelatedEntryPaymentRecipient.Domain = "";
            userControlModuleRelatedEntryPaymentRecipient.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryPaymentRecipient.Location = new System.Drawing.Point(83, 147);
            userControlModuleRelatedEntryPaymentRecipient.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            userControlModuleRelatedEntryPaymentRecipient.Module = null;
            userControlModuleRelatedEntryPaymentRecipient.Name = "userControlModuleRelatedEntryPaymentRecipient";
            userControlModuleRelatedEntryPaymentRecipient.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryPaymentRecipient.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryPaymentRecipient.ShowInfo = false;
            userControlModuleRelatedEntryPaymentRecipient.Size = new System.Drawing.Size(600, 22);
            userControlModuleRelatedEntryPaymentRecipient.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryPaymentRecipient.TabIndex = 6;
            // 
            // labelPaymentRecipient
            // 
            labelPaymentRecipient.AutoSize = true;
            labelPaymentRecipient.Dock = System.Windows.Forms.DockStyle.Fill;
            labelPaymentRecipient.Location = new System.Drawing.Point(3, 144);
            labelPaymentRecipient.Name = "labelPaymentRecipient";
            labelPaymentRecipient.Size = new System.Drawing.Size(73, 28);
            labelPaymentRecipient.TabIndex = 11;
            labelPaymentRecipient.Text = "Received by:";
            labelPaymentRecipient.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelPaymentDate
            // 
            labelPaymentDate.AutoSize = true;
            labelPaymentDate.Dock = System.Windows.Forms.DockStyle.Fill;
            labelPaymentDate.Location = new System.Drawing.Point(3, 172);
            labelPaymentDate.Name = "labelPaymentDate";
            labelPaymentDate.Size = new System.Drawing.Size(73, 29);
            labelPaymentDate.TabIndex = 12;
            labelPaymentDate.Text = "Date:";
            labelPaymentDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePickerPaymentDate
            // 
            dateTimePickerPaymentDate.CustomFormat = "yyyy.MM.dd";
            dateTimePickerPaymentDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", transactionPaymentBindingSource, "PaymentDate", true));
            dateTimePickerPaymentDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dateTimePickerPaymentDate.Location = new System.Drawing.Point(119, 175);
            dateTimePickerPaymentDate.Name = "dateTimePickerPaymentDate";
            dateTimePickerPaymentDate.Size = new System.Drawing.Size(200, 23);
            dateTimePickerPaymentDate.TabIndex = 7;
            dateTimePickerPaymentDate.CloseUp += dateTimePickerPaymentDate_CloseUp;
            // 
            // labelPaymentDateSupplement
            // 
            labelPaymentDateSupplement.AutoSize = true;
            labelPaymentDateSupplement.Dock = System.Windows.Forms.DockStyle.Fill;
            labelPaymentDateSupplement.Location = new System.Drawing.Point(325, 172);
            labelPaymentDateSupplement.Name = "labelPaymentDateSupplement";
            labelPaymentDateSupplement.Size = new System.Drawing.Size(43, 29);
            labelPaymentDateSupplement.TabIndex = 8;
            labelPaymentDateSupplement.Text = "Suppl.:";
            labelPaymentDateSupplement.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPaymentDateSupplement
            // 
            tableLayoutPanelPayment.SetColumnSpan(textBoxPaymentDateSupplement, 2);
            textBoxPaymentDateSupplement.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionPaymentBindingSource, "PaymentDateSupplement", true));
            textBoxPaymentDateSupplement.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxPaymentDateSupplement.Location = new System.Drawing.Point(374, 175);
            textBoxPaymentDateSupplement.Name = "textBoxPaymentDateSupplement";
            textBoxPaymentDateSupplement.Size = new System.Drawing.Size(310, 23);
            textBoxPaymentDateSupplement.TabIndex = 7;
            // 
            // labelPaymentNotes
            // 
            labelPaymentNotes.AutoSize = true;
            labelPaymentNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            labelPaymentNotes.Location = new System.Drawing.Point(3, 207);
            labelPaymentNotes.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            labelPaymentNotes.Name = "labelPaymentNotes";
            labelPaymentNotes.Size = new System.Drawing.Size(73, 202);
            labelPaymentNotes.TabIndex = 16;
            labelPaymentNotes.Text = "Notes:";
            labelPaymentNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxPaymentNotes
            // 
            tableLayoutPanelPayment.SetColumnSpan(textBoxPaymentNotes, 5);
            textBoxPaymentNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionPaymentBindingSource, "Notes", true));
            textBoxPaymentNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxPaymentNotes.Location = new System.Drawing.Point(82, 204);
            textBoxPaymentNotes.Multiline = true;
            textBoxPaymentNotes.Name = "textBoxPaymentNotes";
            textBoxPaymentNotes.Size = new System.Drawing.Size(602, 202);
            textBoxPaymentNotes.TabIndex = 9;
            // 
            // labelPaymentURI
            // 
            labelPaymentURI.AutoSize = true;
            labelPaymentURI.Dock = System.Windows.Forms.DockStyle.Fill;
            labelPaymentURI.Location = new System.Drawing.Point(3, 29);
            labelPaymentURI.Name = "labelPaymentURI";
            labelPaymentURI.Size = new System.Drawing.Size(73, 29);
            labelPaymentURI.TabIndex = 18;
            labelPaymentURI.Text = "Ext. admin.:";
            labelPaymentURI.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPaymentURI
            // 
            tableLayoutPanelPayment.SetColumnSpan(textBoxPaymentURI, 4);
            textBoxPaymentURI.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionPaymentBindingSource, "PaymentURI", true));
            textBoxPaymentURI.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxPaymentURI.Location = new System.Drawing.Point(82, 32);
            textBoxPaymentURI.Name = "textBoxPaymentURI";
            textBoxPaymentURI.Size = new System.Drawing.Size(573, 23);
            textBoxPaymentURI.TabIndex = 1;
            // 
            // buttonPaymentURI
            // 
            buttonPaymentURI.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonPaymentURI.Image = Resource.Payment;
            buttonPaymentURI.Location = new System.Drawing.Point(661, 32);
            buttonPaymentURI.Name = "buttonPaymentURI";
            buttonPaymentURI.Size = new System.Drawing.Size(23, 23);
            buttonPaymentURI.TabIndex = 20;
            toolTip.SetToolTip(buttonPaymentURI, "Search for the link to an external administration of the payment");
            buttonPaymentURI.UseVisualStyleBackColor = true;
            buttonPaymentURI.Click += buttonPaymentURI_Click;
            // 
            // buttonPaymentDate
            // 
            buttonPaymentDate.Image = Resource.Time;
            buttonPaymentDate.Location = new System.Drawing.Point(82, 175);
            buttonPaymentDate.Name = "buttonPaymentDate";
            buttonPaymentDate.Size = new System.Drawing.Size(23, 23);
            buttonPaymentDate.TabIndex = 21;
            toolTip.SetToolTip(buttonPaymentDate, "Set the date for the payment");
            buttonPaymentDate.UseVisualStyleBackColor = true;
            buttonPaymentDate.Click += buttonPaymentDate_Click;
            // 
            // buttonSavePayment
            // 
            buttonSavePayment.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonSavePayment.Image = Resource.Save;
            buttonSavePayment.Location = new System.Drawing.Point(661, 3);
            buttonSavePayment.Name = "buttonSavePayment";
            buttonSavePayment.Size = new System.Drawing.Size(23, 23);
            buttonSavePayment.TabIndex = 22;
            toolTip.SetToolTip(buttonSavePayment, "Save the current changes");
            buttonSavePayment.UseVisualStyleBackColor = true;
            buttonSavePayment.Click += buttonSavePayment_Click;
            // 
            // tabPageAgents
            // 
            tabPageAgents.Controls.Add(splitContainerAgents);
            tabPageAgents.ImageIndex = 14;
            tabPageAgents.Location = new System.Drawing.Point(4, 24);
            tabPageAgents.Name = "tabPageAgents";
            tabPageAgents.Size = new System.Drawing.Size(839, 409);
            tabPageAgents.TabIndex = 15;
            tabPageAgents.Text = "Agents";
            tabPageAgents.UseVisualStyleBackColor = true;
            // 
            // splitContainerAgents
            // 
            splitContainerAgents.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(splitContainerAgents, "Transasction agent");
            helpProvider.SetHelpNavigator(splitContainerAgents, System.Windows.Forms.HelpNavigator.KeywordIndex);
            splitContainerAgents.Location = new System.Drawing.Point(0, 0);
            splitContainerAgents.Name = "splitContainerAgents";
            // 
            // splitContainerAgents.Panel1
            // 
            splitContainerAgents.Panel1.Controls.Add(listBoxAgents);
            splitContainerAgents.Panel1.Controls.Add(toolStripAgents);
            // 
            // splitContainerAgents.Panel2
            // 
            splitContainerAgents.Panel2.Controls.Add(tableLayoutPanelAgents);
            splitContainerAgents.Panel2.Enabled = false;
            helpProvider.SetShowHelp(splitContainerAgents, true);
            splitContainerAgents.Size = new System.Drawing.Size(839, 409);
            splitContainerAgents.SplitterDistance = 279;
            splitContainerAgents.TabIndex = 0;
            // 
            // listBoxAgents
            // 
            listBoxAgents.DataSource = transactionAgentBindingSource;
            listBoxAgents.DisplayMember = "AgentName";
            listBoxAgents.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxAgents.FormattingEnabled = true;
            listBoxAgents.ItemHeight = 15;
            listBoxAgents.Location = new System.Drawing.Point(0, 0);
            listBoxAgents.Name = "listBoxAgents";
            listBoxAgents.Size = new System.Drawing.Size(279, 384);
            listBoxAgents.TabIndex = 0;
            listBoxAgents.ValueMember = "TransactionAgentID";
            listBoxAgents.SelectedIndexChanged += listBoxAgents_SelectedIndexChanged;
            // 
            // transactionAgentBindingSource
            // 
            transactionAgentBindingSource.DataMember = "TransactionAgent";
            transactionAgentBindingSource.DataSource = dataSetTransaction;
            // 
            // toolStripAgents
            // 
            toolStripAgents.Dock = System.Windows.Forms.DockStyle.Bottom;
            toolStripAgents.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripAgents.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonAgentsAdd, toolStripButtonAgentsRemove });
            toolStripAgents.Location = new System.Drawing.Point(0, 384);
            toolStripAgents.Name = "toolStripAgents";
            toolStripAgents.Size = new System.Drawing.Size(279, 25);
            toolStripAgents.TabIndex = 1;
            toolStripAgents.Text = "toolStrip1";
            // 
            // toolStripButtonAgentsAdd
            // 
            toolStripButtonAgentsAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonAgentsAdd.Image = Resource.Add1;
            toolStripButtonAgentsAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonAgentsAdd.Name = "toolStripButtonAgentsAdd";
            toolStripButtonAgentsAdd.Size = new System.Drawing.Size(23, 22);
            toolStripButtonAgentsAdd.Text = "Add a new agent";
            toolStripButtonAgentsAdd.Click += toolStripButtonAgentsAdd_Click;
            // 
            // toolStripButtonAgentsRemove
            // 
            toolStripButtonAgentsRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonAgentsRemove.Image = Resource.Delete;
            toolStripButtonAgentsRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonAgentsRemove.Name = "toolStripButtonAgentsRemove";
            toolStripButtonAgentsRemove.Size = new System.Drawing.Size(23, 22);
            toolStripButtonAgentsRemove.Text = "Remove the selected agent";
            toolStripButtonAgentsRemove.Click += toolStripButtonAgentsRemove_Click;
            // 
            // tableLayoutPanelAgents
            // 
            tableLayoutPanelAgents.ColumnCount = 2;
            tableLayoutPanelAgents.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelAgents.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelAgents.Controls.Add(userControlModuleRelatedEntryAgent, 0, 0);
            tableLayoutPanelAgents.Controls.Add(labelAgentRole, 0, 1);
            tableLayoutPanelAgents.Controls.Add(labelAgentNotes, 0, 2);
            tableLayoutPanelAgents.Controls.Add(textBoxAgentNotes, 1, 2);
            tableLayoutPanelAgents.Controls.Add(comboBoxAgentRole, 1, 1);
            tableLayoutPanelAgents.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelAgents.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelAgents.Name = "tableLayoutPanelAgents";
            tableLayoutPanelAgents.RowCount = 3;
            tableLayoutPanelAgents.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelAgents.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelAgents.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelAgents.Size = new System.Drawing.Size(556, 409);
            tableLayoutPanelAgents.TabIndex = 0;
            // 
            // userControlModuleRelatedEntryAgent
            // 
            userControlModuleRelatedEntryAgent.CanDeleteConnectionToModule = true;
            tableLayoutPanelAgents.SetColumnSpan(userControlModuleRelatedEntryAgent, 2);
            userControlModuleRelatedEntryAgent.DependsOnUri = "";
            userControlModuleRelatedEntryAgent.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleRelatedEntryAgent.Domain = "";
            userControlModuleRelatedEntryAgent.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryAgent.Location = new System.Drawing.Point(4, 3);
            userControlModuleRelatedEntryAgent.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            userControlModuleRelatedEntryAgent.Module = null;
            userControlModuleRelatedEntryAgent.Name = "userControlModuleRelatedEntryAgent";
            userControlModuleRelatedEntryAgent.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryAgent.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryAgent.ShowInfo = false;
            userControlModuleRelatedEntryAgent.Size = new System.Drawing.Size(548, 22);
            userControlModuleRelatedEntryAgent.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryAgent.TabIndex = 0;
            // 
            // labelAgentRole
            // 
            labelAgentRole.AutoSize = true;
            labelAgentRole.Dock = System.Windows.Forms.DockStyle.Fill;
            labelAgentRole.Location = new System.Drawing.Point(3, 28);
            labelAgentRole.Name = "labelAgentRole";
            labelAgentRole.Size = new System.Drawing.Size(41, 29);
            labelAgentRole.TabIndex = 1;
            labelAgentRole.Text = "Role:";
            labelAgentRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelAgentNotes
            // 
            labelAgentNotes.AutoSize = true;
            labelAgentNotes.Dock = System.Windows.Forms.DockStyle.Top;
            labelAgentNotes.Location = new System.Drawing.Point(3, 63);
            labelAgentNotes.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            labelAgentNotes.Name = "labelAgentNotes";
            labelAgentNotes.Size = new System.Drawing.Size(41, 15);
            labelAgentNotes.TabIndex = 2;
            labelAgentNotes.Text = "Notes:";
            // 
            // textBoxAgentNotes
            // 
            textBoxAgentNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionAgentBindingSource, "Notes", true));
            textBoxAgentNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxAgentNotes.Location = new System.Drawing.Point(50, 60);
            textBoxAgentNotes.Multiline = true;
            textBoxAgentNotes.Name = "textBoxAgentNotes";
            textBoxAgentNotes.Size = new System.Drawing.Size(503, 346);
            textBoxAgentNotes.TabIndex = 3;
            // 
            // comboBoxAgentRole
            // 
            comboBoxAgentRole.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionAgentBindingSource, "AgentRole", true));
            comboBoxAgentRole.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxAgentRole.FormattingEnabled = true;
            comboBoxAgentRole.Location = new System.Drawing.Point(50, 31);
            comboBoxAgentRole.Name = "comboBoxAgentRole";
            comboBoxAgentRole.Size = new System.Drawing.Size(503, 23);
            comboBoxAgentRole.TabIndex = 4;
            comboBoxAgentRole.DropDown += comboBoxAgentRole_DropDown;
            // 
            // tabPageIdentifier
            // 
            tabPageIdentifier.Controls.Add(splitContainerIdentifier);
            tabPageIdentifier.ImageIndex = 15;
            tabPageIdentifier.Location = new System.Drawing.Point(4, 24);
            tabPageIdentifier.Name = "tabPageIdentifier";
            tabPageIdentifier.Size = new System.Drawing.Size(839, 409);
            tabPageIdentifier.TabIndex = 16;
            tabPageIdentifier.Text = "Identifier";
            tabPageIdentifier.UseVisualStyleBackColor = true;
            // 
            // splitContainerIdentifier
            // 
            splitContainerIdentifier.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(splitContainerIdentifier, "Transaction identifier");
            helpProvider.SetHelpNavigator(splitContainerIdentifier, System.Windows.Forms.HelpNavigator.KeywordIndex);
            splitContainerIdentifier.Location = new System.Drawing.Point(0, 0);
            splitContainerIdentifier.Name = "splitContainerIdentifier";
            // 
            // splitContainerIdentifier.Panel1
            // 
            splitContainerIdentifier.Panel1.Controls.Add(listBoxIdentifier);
            splitContainerIdentifier.Panel1.Controls.Add(toolStripIdentifier);
            // 
            // splitContainerIdentifier.Panel2
            // 
            splitContainerIdentifier.Panel2.Controls.Add(tableLayoutPanelIdentifier);
            helpProvider.SetShowHelp(splitContainerIdentifier, true);
            splitContainerIdentifier.Size = new System.Drawing.Size(839, 409);
            splitContainerIdentifier.SplitterDistance = 279;
            splitContainerIdentifier.TabIndex = 0;
            // 
            // listBoxIdentifier
            // 
            listBoxIdentifier.DataSource = externalIdentifierBindingSource;
            listBoxIdentifier.DisplayMember = "Identifier";
            listBoxIdentifier.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxIdentifier.FormattingEnabled = true;
            listBoxIdentifier.IntegralHeight = false;
            listBoxIdentifier.ItemHeight = 15;
            listBoxIdentifier.Location = new System.Drawing.Point(0, 0);
            listBoxIdentifier.Name = "listBoxIdentifier";
            listBoxIdentifier.Size = new System.Drawing.Size(279, 384);
            listBoxIdentifier.TabIndex = 0;
            listBoxIdentifier.ValueMember = "ID";
            listBoxIdentifier.SelectedIndexChanged += listBoxIdentifier_SelectedIndexChanged;
            // 
            // externalIdentifierBindingSource
            // 
            externalIdentifierBindingSource.DataMember = "ExternalIdentifier";
            externalIdentifierBindingSource.DataSource = dataSetTransaction;
            // 
            // toolStripIdentifier
            // 
            toolStripIdentifier.Dock = System.Windows.Forms.DockStyle.Bottom;
            toolStripIdentifier.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripIdentifier.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonIdentifierAdd, toolStripButtonIdentifierRemove });
            toolStripIdentifier.Location = new System.Drawing.Point(0, 384);
            toolStripIdentifier.Name = "toolStripIdentifier";
            toolStripIdentifier.Size = new System.Drawing.Size(279, 25);
            toolStripIdentifier.TabIndex = 1;
            toolStripIdentifier.Text = "toolStrip1";
            // 
            // toolStripButtonIdentifierAdd
            // 
            toolStripButtonIdentifierAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonIdentifierAdd.Image = Resource.Add1;
            toolStripButtonIdentifierAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonIdentifierAdd.Name = "toolStripButtonIdentifierAdd";
            toolStripButtonIdentifierAdd.Size = new System.Drawing.Size(23, 22);
            toolStripButtonIdentifierAdd.Text = "Add a new identifier";
            toolStripButtonIdentifierAdd.Click += toolStripButtonIdentifierAdd_Click;
            // 
            // toolStripButtonIdentifierRemove
            // 
            toolStripButtonIdentifierRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonIdentifierRemove.Image = Resource.Delete;
            toolStripButtonIdentifierRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonIdentifierRemove.Name = "toolStripButtonIdentifierRemove";
            toolStripButtonIdentifierRemove.Size = new System.Drawing.Size(23, 22);
            toolStripButtonIdentifierRemove.Text = "Remove the selected identifier";
            toolStripButtonIdentifierRemove.Click += toolStripButtonIdentifierRemove_Click;
            // 
            // tableLayoutPanelIdentifier
            // 
            tableLayoutPanelIdentifier.ColumnCount = 3;
            tableLayoutPanelIdentifier.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelIdentifier.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelIdentifier.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelIdentifier.Controls.Add(labelIdentifierType, 0, 0);
            tableLayoutPanelIdentifier.Controls.Add(labelIdentifier, 0, 1);
            tableLayoutPanelIdentifier.Controls.Add(textBoxIdentifierType, 1, 0);
            tableLayoutPanelIdentifier.Controls.Add(textBoxIdentifier, 1, 1);
            tableLayoutPanelIdentifier.Controls.Add(buttonIdenitifierURL, 2, 2);
            tableLayoutPanelIdentifier.Controls.Add(labelIdenitifierURL, 0, 2);
            tableLayoutPanelIdentifier.Controls.Add(textBoxIdenitifierURL, 1, 2);
            tableLayoutPanelIdentifier.Controls.Add(labelIdenitifierNotes, 0, 3);
            tableLayoutPanelIdentifier.Controls.Add(textBoxIdenitifierNotes, 1, 3);
            tableLayoutPanelIdentifier.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelIdentifier.Enabled = false;
            tableLayoutPanelIdentifier.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelIdentifier.Name = "tableLayoutPanelIdentifier";
            tableLayoutPanelIdentifier.RowCount = 5;
            tableLayoutPanelIdentifier.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelIdentifier.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelIdentifier.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelIdentifier.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelIdentifier.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelIdentifier.Size = new System.Drawing.Size(556, 409);
            tableLayoutPanelIdentifier.TabIndex = 0;
            // 
            // labelIdentifierType
            // 
            labelIdentifierType.AutoSize = true;
            labelIdentifierType.Dock = System.Windows.Forms.DockStyle.Fill;
            labelIdentifierType.Location = new System.Drawing.Point(3, 0);
            labelIdentifierType.Name = "labelIdentifierType";
            labelIdentifierType.Size = new System.Drawing.Size(57, 29);
            labelIdentifierType.TabIndex = 0;
            labelIdentifierType.Text = "Type:";
            labelIdentifierType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelIdentifier
            // 
            labelIdentifier.AutoSize = true;
            labelIdentifier.Dock = System.Windows.Forms.DockStyle.Top;
            labelIdentifier.Location = new System.Drawing.Point(3, 35);
            labelIdentifier.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            labelIdentifier.Name = "labelIdentifier";
            labelIdentifier.Size = new System.Drawing.Size(57, 15);
            labelIdentifier.TabIndex = 1;
            labelIdentifier.Text = "Identifier:";
            labelIdentifier.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxIdentifierType
            // 
            tableLayoutPanelIdentifier.SetColumnSpan(textBoxIdentifierType, 2);
            textBoxIdentifierType.DataBindings.Add(new System.Windows.Forms.Binding("Text", externalIdentifierBindingSource, "Type", true));
            textBoxIdentifierType.Dock = System.Windows.Forms.DockStyle.Top;
            textBoxIdentifierType.Location = new System.Drawing.Point(66, 3);
            textBoxIdentifierType.Name = "textBoxIdentifierType";
            textBoxIdentifierType.ReadOnly = true;
            textBoxIdentifierType.Size = new System.Drawing.Size(487, 23);
            textBoxIdentifierType.TabIndex = 2;
            // 
            // textBoxIdentifier
            // 
            tableLayoutPanelIdentifier.SetColumnSpan(textBoxIdentifier, 2);
            textBoxIdentifier.DataBindings.Add(new System.Windows.Forms.Binding("Text", externalIdentifierBindingSource, "Identifier", true));
            textBoxIdentifier.Dock = System.Windows.Forms.DockStyle.Top;
            textBoxIdentifier.Location = new System.Drawing.Point(66, 32);
            textBoxIdentifier.Name = "textBoxIdentifier";
            textBoxIdentifier.ReadOnly = true;
            textBoxIdentifier.Size = new System.Drawing.Size(487, 23);
            textBoxIdentifier.TabIndex = 3;
            // 
            // buttonIdenitifierURL
            // 
            buttonIdenitifierURL.Image = Resource.Browse;
            buttonIdenitifierURL.Location = new System.Drawing.Point(531, 59);
            buttonIdenitifierURL.Margin = new System.Windows.Forms.Padding(1);
            buttonIdenitifierURL.Name = "buttonIdenitifierURL";
            buttonIdenitifierURL.Size = new System.Drawing.Size(24, 24);
            buttonIdenitifierURL.TabIndex = 4;
            buttonIdenitifierURL.UseVisualStyleBackColor = true;
            // 
            // labelIdenitifierURL
            // 
            labelIdenitifierURL.AutoSize = true;
            labelIdenitifierURL.Dock = System.Windows.Forms.DockStyle.Fill;
            labelIdenitifierURL.Location = new System.Drawing.Point(3, 58);
            labelIdenitifierURL.Name = "labelIdenitifierURL";
            labelIdenitifierURL.Size = new System.Drawing.Size(57, 29);
            labelIdenitifierURL.TabIndex = 5;
            labelIdenitifierURL.Text = "URL:";
            labelIdenitifierURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxIdenitifierURL
            // 
            textBoxIdenitifierURL.DataBindings.Add(new System.Windows.Forms.Binding("Text", externalIdentifierBindingSource, "URL", true));
            textBoxIdenitifierURL.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxIdenitifierURL.Location = new System.Drawing.Point(66, 61);
            textBoxIdenitifierURL.Name = "textBoxIdenitifierURL";
            textBoxIdenitifierURL.Size = new System.Drawing.Size(461, 23);
            textBoxIdenitifierURL.TabIndex = 6;
            // 
            // labelIdenitifierNotes
            // 
            labelIdenitifierNotes.AutoSize = true;
            labelIdenitifierNotes.Dock = System.Windows.Forms.DockStyle.Top;
            labelIdenitifierNotes.Location = new System.Drawing.Point(3, 93);
            labelIdenitifierNotes.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            labelIdenitifierNotes.Name = "labelIdenitifierNotes";
            labelIdenitifierNotes.Size = new System.Drawing.Size(57, 15);
            labelIdenitifierNotes.TabIndex = 7;
            labelIdenitifierNotes.Text = "Notes:";
            labelIdenitifierNotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxIdenitifierNotes
            // 
            tableLayoutPanelIdentifier.SetColumnSpan(textBoxIdenitifierNotes, 2);
            textBoxIdenitifierNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", externalIdentifierBindingSource, "Notes", true));
            textBoxIdenitifierNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxIdenitifierNotes.Location = new System.Drawing.Point(66, 90);
            textBoxIdenitifierNotes.Multiline = true;
            textBoxIdenitifierNotes.Name = "textBoxIdenitifierNotes";
            tableLayoutPanelIdentifier.SetRowSpan(textBoxIdenitifierNotes, 2);
            textBoxIdenitifierNotes.Size = new System.Drawing.Size(487, 316);
            textBoxIdenitifierNotes.TabIndex = 8;
            // 
            // tabPageChart
            // 
            tabPageChart.Controls.Add(tabControlChart);
            tabPageChart.Controls.Add(tabControlChartSettings);
            tabPageChart.ImageIndex = 9;
            tabPageChart.Location = new System.Drawing.Point(4, 24);
            tabPageChart.Name = "tabPageChart";
            tabPageChart.Size = new System.Drawing.Size(839, 409);
            tabPageChart.TabIndex = 17;
            tabPageChart.Text = "Statistics";
            tabPageChart.UseVisualStyleBackColor = true;
            // 
            // tabControlChart
            // 
            tabControlChart.Controls.Add(tabPageChartResult);
            tabControlChart.Controls.Add(tabPageChartData);
            tabControlChart.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlChart.ImageList = imageListTab;
            tabControlChart.Location = new System.Drawing.Point(100, 0);
            tabControlChart.Name = "tabControlChart";
            tabControlChart.SelectedIndex = 0;
            tabControlChart.Size = new System.Drawing.Size(739, 409);
            tabControlChart.TabIndex = 13;
            // 
            // tabPageChartResult
            // 
            tabPageChartResult.Controls.Add(tableLayoutPanelChart);
            tabPageChartResult.ImageKey = "Graph.ico";
            tabPageChartResult.Location = new System.Drawing.Point(4, 24);
            tabPageChartResult.Name = "tabPageChartResult";
            tabPageChartResult.Padding = new System.Windows.Forms.Padding(3);
            tabPageChartResult.Size = new System.Drawing.Size(731, 381);
            tabPageChartResult.TabIndex = 0;
            tabPageChartResult.Text = "Chart";
            tabPageChartResult.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelChart
            // 
            tableLayoutPanelChart.ColumnCount = 6;
            tableLayoutPanelChart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelChart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelChart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelChart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            tableLayoutPanelChart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelChart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelChart.Controls.Add(labelChartHeader, 0, 0);
            tableLayoutPanelChart.Controls.Add(buttonChartGenerate, 5, 3);
            tableLayoutPanelChart.Controls.Add(groupBoxChartGrouping, 4, 3);
            tableLayoutPanelChart.Controls.Add(groupBoxChartStart, 2, 3);
            tableLayoutPanelChart.Controls.Add(checkBoxChartAddNumberOfUnits, 1, 3);
            tableLayoutPanelChart.Controls.Add(tabControlChartSettingsForChart, 0, 1);
            tableLayoutPanelChart.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelChart.Location = new System.Drawing.Point(3, 3);
            tableLayoutPanelChart.Name = "tableLayoutPanelChart";
            tableLayoutPanelChart.RowCount = 4;
            tableLayoutPanelChart.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelChart.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            tableLayoutPanelChart.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            tableLayoutPanelChart.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelChart.Size = new System.Drawing.Size(725, 375);
            tableLayoutPanelChart.TabIndex = 0;
            // 
            // labelChartHeader
            // 
            labelChartHeader.AutoSize = true;
            labelChartHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            labelChartHeader.Location = new System.Drawing.Point(6, 6);
            labelChartHeader.Margin = new System.Windows.Forms.Padding(6);
            labelChartHeader.Name = "labelChartHeader";
            labelChartHeader.Size = new System.Drawing.Size(221, 15);
            labelChartHeader.TabIndex = 0;
            labelChartHeader.Text = "Generate graph for transaction over time";
            // 
            // buttonChartGenerate
            // 
            buttonChartGenerate.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonChartGenerate.Image = Resource.Graph;
            buttonChartGenerate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonChartGenerate.Location = new System.Drawing.Point(626, 335);
            buttonChartGenerate.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            buttonChartGenerate.Name = "buttonChartGenerate";
            buttonChartGenerate.Size = new System.Drawing.Size(96, 37);
            buttonChartGenerate.TabIndex = 3;
            buttonChartGenerate.Text = "      Create chart";
            buttonChartGenerate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonChartGenerate.UseVisualStyleBackColor = true;
            buttonChartGenerate.Click += buttonChartGenerate_Click;
            // 
            // groupBoxChartGrouping
            // 
            groupBoxChartGrouping.Controls.Add(tableLayoutPanelChartGrouping);
            groupBoxChartGrouping.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxChartGrouping.Location = new System.Drawing.Point(386, 330);
            groupBoxChartGrouping.Name = "groupBoxChartGrouping";
            groupBoxChartGrouping.Padding = new System.Windows.Forms.Padding(0);
            groupBoxChartGrouping.Size = new System.Drawing.Size(234, 42);
            groupBoxChartGrouping.TabIndex = 7;
            groupBoxChartGrouping.TabStop = false;
            groupBoxChartGrouping.Text = "Separate in chart";
            // 
            // tableLayoutPanelChartGrouping
            // 
            tableLayoutPanelChartGrouping.ColumnCount = 3;
            tableLayoutPanelChartGrouping.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelChartGrouping.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelChartGrouping.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelChartGrouping.Controls.Add(radioButtonChartGroupingNone, 0, 0);
            tableLayoutPanelChartGrouping.Controls.Add(radioButtonChartGroupingCollection, 1, 0);
            tableLayoutPanelChartGrouping.Controls.Add(radioButtonChartGroupingTransactionType, 2, 0);
            tableLayoutPanelChartGrouping.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelChartGrouping.Location = new System.Drawing.Point(0, 16);
            tableLayoutPanelChartGrouping.Name = "tableLayoutPanelChartGrouping";
            tableLayoutPanelChartGrouping.RowCount = 1;
            tableLayoutPanelChartGrouping.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelChartGrouping.Size = new System.Drawing.Size(234, 26);
            tableLayoutPanelChartGrouping.TabIndex = 3;
            // 
            // radioButtonChartGroupingNone
            // 
            radioButtonChartGroupingNone.AutoSize = true;
            radioButtonChartGroupingNone.Checked = true;
            radioButtonChartGroupingNone.Location = new System.Drawing.Point(3, 0);
            radioButtonChartGroupingNone.Margin = new System.Windows.Forms.Padding(3, 0, 6, 0);
            radioButtonChartGroupingNone.Name = "radioButtonChartGroupingNone";
            radioButtonChartGroupingNone.Size = new System.Drawing.Size(99, 19);
            radioButtonChartGroupingNone.TabIndex = 0;
            radioButtonChartGroupingNone.TabStop = true;
            radioButtonChartGroupingNone.Text = "No separation";
            radioButtonChartGroupingNone.UseVisualStyleBackColor = true;
            // 
            // radioButtonChartGroupingCollection
            // 
            radioButtonChartGroupingCollection.AutoSize = true;
            radioButtonChartGroupingCollection.Location = new System.Drawing.Point(108, 0);
            radioButtonChartGroupingCollection.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            radioButtonChartGroupingCollection.Name = "radioButtonChartGroupingCollection";
            radioButtonChartGroupingCollection.Size = new System.Drawing.Size(170, 19);
            radioButtonChartGroupingCollection.TabIndex = 1;
            radioButtonChartGroupingCollection.Text = "Separate admin. collections";
            radioButtonChartGroupingCollection.UseVisualStyleBackColor = true;
            // 
            // radioButtonChartGroupingTransactionType
            // 
            radioButtonChartGroupingTransactionType.AutoSize = true;
            radioButtonChartGroupingTransactionType.Location = new System.Drawing.Point(284, 0);
            radioButtonChartGroupingTransactionType.Margin = new System.Windows.Forms.Padding(0);
            radioButtonChartGroupingTransactionType.Name = "radioButtonChartGroupingTransactionType";
            radioButtonChartGroupingTransactionType.Size = new System.Drawing.Size(1, 19);
            radioButtonChartGroupingTransactionType.TabIndex = 2;
            radioButtonChartGroupingTransactionType.Text = "Separate types";
            radioButtonChartGroupingTransactionType.UseVisualStyleBackColor = true;
            // 
            // groupBoxChartStart
            // 
            groupBoxChartStart.Controls.Add(numericUpDownChartStart);
            groupBoxChartStart.Controls.Add(checkBoxChartStart);
            groupBoxChartStart.Location = new System.Drawing.Point(310, 330);
            groupBoxChartStart.Name = "groupBoxChartStart";
            groupBoxChartStart.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            helpProvider.SetShowHelp(groupBoxChartStart, false);
            groupBoxChartStart.Size = new System.Drawing.Size(68, 34);
            groupBoxChartStart.TabIndex = 10;
            groupBoxChartStart.TabStop = false;
            groupBoxChartStart.Text = "Start";
            // 
            // numericUpDownChartStart
            // 
            numericUpDownChartStart.Dock = System.Windows.Forms.DockStyle.Fill;
            numericUpDownChartStart.Location = new System.Drawing.Point(18, 16);
            numericUpDownChartStart.Margin = new System.Windows.Forms.Padding(0, 9, 3, 3);
            numericUpDownChartStart.Maximum = new decimal(new int[] { 2100, 0, 0, 0 });
            numericUpDownChartStart.Minimum = new decimal(new int[] { 1900, 0, 0, 0 });
            numericUpDownChartStart.Name = "numericUpDownChartStart";
            helpProvider.SetShowHelp(numericUpDownChartStart, false);
            numericUpDownChartStart.Size = new System.Drawing.Size(47, 23);
            numericUpDownChartStart.TabIndex = 9;
            numericUpDownChartStart.Value = new decimal(new int[] { 1970, 0, 0, 0 });
            // 
            // checkBoxChartStart
            // 
            checkBoxChartStart.AutoSize = true;
            checkBoxChartStart.Dock = System.Windows.Forms.DockStyle.Left;
            checkBoxChartStart.Location = new System.Drawing.Point(3, 16);
            checkBoxChartStart.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            checkBoxChartStart.Name = "checkBoxChartStart";
            checkBoxChartStart.Size = new System.Drawing.Size(15, 18);
            checkBoxChartStart.TabIndex = 8;
            checkBoxChartStart.UseVisualStyleBackColor = true;
            // 
            // checkBoxChartAddNumberOfUnits
            // 
            checkBoxChartAddNumberOfUnits.AutoSize = true;
            checkBoxChartAddNumberOfUnits.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxChartAddNumberOfUnits.Image = Resource.AddAdd;
            checkBoxChartAddNumberOfUnits.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            checkBoxChartAddNumberOfUnits.Location = new System.Drawing.Point(236, 330);
            checkBoxChartAddNumberOfUnits.Name = "checkBoxChartAddNumberOfUnits";
            checkBoxChartAddNumberOfUnits.Size = new System.Drawing.Size(68, 42);
            checkBoxChartAddNumberOfUnits.TabIndex = 11;
            checkBoxChartAddNumberOfUnits.Text = "     Units";
            checkBoxChartAddNumberOfUnits.UseVisualStyleBackColor = true;
            // 
            // tabControlChartSettingsForChart
            // 
            tabControlChartSettingsForChart.Controls.Add(tabPageChartAdminColl);
            tabControlChartSettingsForChart.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlChartSettingsForChart.ImageList = imageListTab;
            tabControlChartSettingsForChart.Location = new System.Drawing.Point(3, 30);
            tabControlChartSettingsForChart.Name = "tabControlChartSettingsForChart";
            tableLayoutPanelChart.SetRowSpan(tabControlChartSettingsForChart, 3);
            tabControlChartSettingsForChart.SelectedIndex = 0;
            tabControlChartSettingsForChart.Size = new System.Drawing.Size(227, 342);
            tabControlChartSettingsForChart.TabIndex = 12;
            // 
            // tabPageChartAdminColl
            // 
            tabPageChartAdminColl.Controls.Add(checkedListBoxChartCollections);
            tabPageChartAdminColl.ImageKey = "CollectionManager.ico";
            tabPageChartAdminColl.Location = new System.Drawing.Point(4, 24);
            tabPageChartAdminColl.Name = "tabPageChartAdminColl";
            tabPageChartAdminColl.Padding = new System.Windows.Forms.Padding(3);
            tabPageChartAdminColl.Size = new System.Drawing.Size(219, 314);
            tabPageChartAdminColl.TabIndex = 1;
            tabPageChartAdminColl.Text = "Admin. coll.";
            tabPageChartAdminColl.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxChartCollections
            // 
            checkedListBoxChartCollections.CheckOnClick = true;
            checkedListBoxChartCollections.Dock = System.Windows.Forms.DockStyle.Fill;
            checkedListBoxChartCollections.FormattingEnabled = true;
            checkedListBoxChartCollections.IntegralHeight = false;
            checkedListBoxChartCollections.Location = new System.Drawing.Point(3, 3);
            checkedListBoxChartCollections.Name = "checkedListBoxChartCollections";
            checkedListBoxChartCollections.Size = new System.Drawing.Size(213, 308);
            checkedListBoxChartCollections.TabIndex = 1;
            // 
            // imageListTab
            // 
            imageListTab.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListTab.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListTab.ImageStream");
            imageListTab.TransparentColor = System.Drawing.Color.Transparent;
            imageListTab.Images.SetKeyName(0, "Edit.ico");
            imageListTab.Images.SetKeyName(1, "LoanOut.ico");
            imageListTab.Images.SetKeyName(2, "LoanQuery.ico");
            imageListTab.Images.SetKeyName(3, "TransactionOnLoan.ico");
            imageListTab.Images.SetKeyName(4, "LoanPartIn.ico");
            imageListTab.Images.SetKeyName(5, "LoanIn.ico");
            imageListTab.Images.SetKeyName(6, "Print.ico");
            imageListTab.Images.SetKeyName(7, "Document.ICO");
            imageListTab.Images.SetKeyName(8, "Balance.ico");
            imageListTab.Images.SetKeyName(9, "Graph.ico");
            imageListTab.Images.SetKeyName(10, "LoanRequest.ico");
            imageListTab.Images.SetKeyName(11, "TransactionForwarding.ico");
            imageListTab.Images.SetKeyName(12, "NoAccess.ico");
            imageListTab.Images.SetKeyName(13, "Payment.ico");
            imageListTab.Images.SetKeyName(14, "Agent.ico");
            imageListTab.Images.SetKeyName(15, "Identifier.ico");
            imageListTab.Images.SetKeyName(16, "Country.ico");
            imageListTab.Images.SetKeyName(17, "Institution.ico");
            imageListTab.Images.SetKeyName(18, "CollectionManager.ico");
            imageListTab.Images.SetKeyName(19, "Speadsheet.ico");
            // 
            // tabPageChartData
            // 
            tabPageChartData.Controls.Add(tableLayoutPanelChartData);
            tabPageChartData.ImageKey = "Speadsheet.ico";
            tabPageChartData.Location = new System.Drawing.Point(4, 24);
            tabPageChartData.Name = "tabPageChartData";
            tabPageChartData.Padding = new System.Windows.Forms.Padding(3);
            tabPageChartData.Size = new System.Drawing.Size(731, 382);
            tabPageChartData.TabIndex = 1;
            tabPageChartData.Text = "Data";
            tabPageChartData.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelChartData
            // 
            tableLayoutPanelChartData.ColumnCount = 3;
            tableLayoutPanelChartData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelChartData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelChartData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelChartData.Controls.Add(dataGridViewChartData, 1, 0);
            tableLayoutPanelChartData.Controls.Add(tabControlChartData, 0, 3);
            tableLayoutPanelChartData.Controls.Add(labelChartData, 0, 0);
            tableLayoutPanelChartData.Controls.Add(buttonChartDataExport, 2, 4);
            tableLayoutPanelChartData.Controls.Add(buttonChartDataGet, 1, 4);
            tableLayoutPanelChartData.Controls.Add(radioButtonChartDataChart, 0, 1);
            tableLayoutPanelChartData.Controls.Add(radioButtonChartDataSettings, 0, 2);
            tableLayoutPanelChartData.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelChartData.Location = new System.Drawing.Point(3, 3);
            tableLayoutPanelChartData.Name = "tableLayoutPanelChartData";
            tableLayoutPanelChartData.RowCount = 5;
            tableLayoutPanelChartData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelChartData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelChartData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelChartData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelChartData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelChartData.Size = new System.Drawing.Size(725, 376);
            tableLayoutPanelChartData.TabIndex = 1;
            // 
            // dataGridViewChartData
            // 
            dataGridViewChartData.AllowUserToAddRows = false;
            dataGridViewChartData.AllowUserToDeleteRows = false;
            dataGridViewChartData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableLayoutPanelChartData.SetColumnSpan(dataGridViewChartData, 2);
            dataGridViewChartData.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridViewChartData.Location = new System.Drawing.Point(227, 3);
            dataGridViewChartData.Name = "dataGridViewChartData";
            dataGridViewChartData.ReadOnly = true;
            tableLayoutPanelChartData.SetRowSpan(dataGridViewChartData, 4);
            dataGridViewChartData.Size = new System.Drawing.Size(495, 340);
            dataGridViewChartData.TabIndex = 0;
            // 
            // tabControlChartData
            // 
            tabControlChartData.Controls.Add(tabPageChartDataAddress);
            tabControlChartData.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlChartData.Enabled = false;
            tabControlChartData.ImageList = imageListTab;
            tabControlChartData.Location = new System.Drawing.Point(3, 64);
            tabControlChartData.Name = "tabControlChartData";
            tableLayoutPanelChartData.SetRowSpan(tabControlChartData, 2);
            tabControlChartData.SelectedIndex = 0;
            tabControlChartData.Size = new System.Drawing.Size(218, 309);
            tabControlChartData.TabIndex = 1;
            // 
            // tabPageChartDataAddress
            // 
            tabPageChartDataAddress.Controls.Add(tableLayoutPanelChartDataAddress);
            tabPageChartDataAddress.ImageKey = "Country.ico";
            tabPageChartDataAddress.Location = new System.Drawing.Point(4, 24);
            tabPageChartDataAddress.Name = "tabPageChartDataAddress";
            tabPageChartDataAddress.Size = new System.Drawing.Size(210, 281);
            tabPageChartDataAddress.TabIndex = 4;
            tabPageChartDataAddress.Text = "Address";
            tabPageChartDataAddress.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelChartDataAddress
            // 
            tableLayoutPanelChartDataAddress.ColumnCount = 1;
            tableLayoutPanelChartDataAddress.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelChartDataAddress.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanelChartDataAddress.Controls.Add(checkBoxChartAddressCity, 0, 5);
            tableLayoutPanelChartDataAddress.Controls.Add(checkBoxChartAddressCountry, 0, 4);
            tableLayoutPanelChartDataAddress.Controls.Add(listBoxChartAddressSelf, 0, 1);
            tableLayoutPanelChartDataAddress.Controls.Add(labelChartAddressSelf, 0, 0);
            tableLayoutPanelChartDataAddress.Controls.Add(labelChartAddressOutput, 0, 3);
            tableLayoutPanelChartDataAddress.Controls.Add(checkBoxChartAddressPartner, 0, 6);
            tableLayoutPanelChartDataAddress.Controls.Add(toolStripChartAddressSelf, 0, 2);
            tableLayoutPanelChartDataAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelChartDataAddress.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelChartDataAddress.Name = "tableLayoutPanelChartDataAddress";
            tableLayoutPanelChartDataAddress.RowCount = 8;
            tableLayoutPanelChartDataAddress.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelChartDataAddress.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelChartDataAddress.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelChartDataAddress.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanelChartDataAddress.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelChartDataAddress.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelChartDataAddress.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelChartDataAddress.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanelChartDataAddress.Size = new System.Drawing.Size(210, 281);
            tableLayoutPanelChartDataAddress.TabIndex = 0;
            // 
            // checkBoxChartAddressCity
            // 
            checkBoxChartAddressCity.AutoSize = true;
            checkBoxChartAddressCity.Location = new System.Drawing.Point(3, 214);
            checkBoxChartAddressCity.Name = "checkBoxChartAddressCity";
            checkBoxChartAddressCity.Size = new System.Drawing.Size(47, 19);
            checkBoxChartAddressCity.TabIndex = 1;
            checkBoxChartAddressCity.Text = "City";
            checkBoxChartAddressCity.UseVisualStyleBackColor = true;
            // 
            // checkBoxChartAddressCountry
            // 
            checkBoxChartAddressCountry.AutoSize = true;
            checkBoxChartAddressCountry.Location = new System.Drawing.Point(3, 189);
            checkBoxChartAddressCountry.Name = "checkBoxChartAddressCountry";
            checkBoxChartAddressCountry.Size = new System.Drawing.Size(69, 19);
            checkBoxChartAddressCountry.TabIndex = 0;
            checkBoxChartAddressCountry.Text = "Country";
            checkBoxChartAddressCountry.UseVisualStyleBackColor = true;
            // 
            // listBoxChartAddressSelf
            // 
            listBoxChartAddressSelf.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxChartAddressSelf.FormattingEnabled = true;
            listBoxChartAddressSelf.ItemHeight = 15;
            listBoxChartAddressSelf.Location = new System.Drawing.Point(3, 18);
            listBoxChartAddressSelf.Name = "listBoxChartAddressSelf";
            listBoxChartAddressSelf.Size = new System.Drawing.Size(204, 122);
            listBoxChartAddressSelf.TabIndex = 2;
            // 
            // labelChartAddressSelf
            // 
            labelChartAddressSelf.AutoSize = true;
            labelChartAddressSelf.Location = new System.Drawing.Point(3, 0);
            labelChartAddressSelf.Name = "labelChartAddressSelf";
            labelChartAddressSelf.Size = new System.Drawing.Size(151, 15);
            labelChartAddressSelf.TabIndex = 4;
            labelChartAddressSelf.Text = "Administrating institutions ";
            // 
            // labelChartAddressOutput
            // 
            labelChartAddressOutput.AutoSize = true;
            labelChartAddressOutput.Dock = System.Windows.Forms.DockStyle.Bottom;
            labelChartAddressOutput.Location = new System.Drawing.Point(3, 171);
            labelChartAddressOutput.Name = "labelChartAddressOutput";
            labelChartAddressOutput.Size = new System.Drawing.Size(204, 15);
            labelChartAddressOutput.TabIndex = 5;
            labelChartAddressOutput.Text = "Output";
            // 
            // checkBoxChartAddressPartner
            // 
            checkBoxChartAddressPartner.AutoSize = true;
            checkBoxChartAddressPartner.Location = new System.Drawing.Point(3, 239);
            checkBoxChartAddressPartner.Name = "checkBoxChartAddressPartner";
            checkBoxChartAddressPartner.Size = new System.Drawing.Size(121, 19);
            checkBoxChartAddressPartner.TabIndex = 6;
            checkBoxChartAddressPartner.Text = "Partner institution";
            checkBoxChartAddressPartner.UseVisualStyleBackColor = true;
            // 
            // toolStripChartAddressSelf
            // 
            toolStripChartAddressSelf.Dock = System.Windows.Forms.DockStyle.None;
            toolStripChartAddressSelf.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripChartAddressSelf.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonChartAddressSelfAdd, toolStripButtonChartAddressSelfRemove });
            toolStripChartAddressSelf.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            toolStripChartAddressSelf.Location = new System.Drawing.Point(6, 143);
            toolStripChartAddressSelf.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            toolStripChartAddressSelf.Name = "toolStripChartAddressSelf";
            toolStripChartAddressSelf.Size = new System.Drawing.Size(47, 23);
            toolStripChartAddressSelf.TabIndex = 3;
            toolStripChartAddressSelf.Text = "toolStrip1";
            // 
            // toolStripButtonChartAddressSelfAdd
            // 
            toolStripButtonChartAddressSelfAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonChartAddressSelfAdd.Image = Resource.Add1;
            toolStripButtonChartAddressSelfAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonChartAddressSelfAdd.Name = "toolStripButtonChartAddressSelfAdd";
            toolStripButtonChartAddressSelfAdd.Size = new System.Drawing.Size(23, 20);
            toolStripButtonChartAddressSelfAdd.Text = "toolStripButton1";
            toolStripButtonChartAddressSelfAdd.Click += toolStripButtonChartAddressSelfAdd_Click;
            // 
            // toolStripButtonChartAddressSelfRemove
            // 
            toolStripButtonChartAddressSelfRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonChartAddressSelfRemove.Image = Resource.Delete;
            toolStripButtonChartAddressSelfRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonChartAddressSelfRemove.Name = "toolStripButtonChartAddressSelfRemove";
            toolStripButtonChartAddressSelfRemove.Size = new System.Drawing.Size(23, 20);
            toolStripButtonChartAddressSelfRemove.Text = "toolStripButton1";
            toolStripButtonChartAddressSelfRemove.Click += toolStripButtonChartAddressSelfRemove_Click;
            // 
            // labelChartData
            // 
            labelChartData.AutoSize = true;
            labelChartData.Location = new System.Drawing.Point(3, 0);
            labelChartData.Name = "labelChartData";
            labelChartData.Size = new System.Drawing.Size(104, 15);
            labelChartData.TabIndex = 2;
            labelChartData.Text = "Data according to:";
            // 
            // buttonChartDataExport
            // 
            buttonChartDataExport.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonChartDataExport.Enabled = false;
            buttonChartDataExport.Image = Resource.Export;
            buttonChartDataExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonChartDataExport.Location = new System.Drawing.Point(647, 349);
            buttonChartDataExport.Name = "buttonChartDataExport";
            buttonChartDataExport.Size = new System.Drawing.Size(75, 24);
            buttonChartDataExport.TabIndex = 3;
            buttonChartDataExport.Text = "      Export";
            buttonChartDataExport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonChartDataExport.UseVisualStyleBackColor = true;
            buttonChartDataExport.Click += buttonChartDataExport_Click;
            // 
            // buttonChartDataGet
            // 
            buttonChartDataGet.Dock = System.Windows.Forms.DockStyle.Right;
            buttonChartDataGet.Enabled = false;
            buttonChartDataGet.Image = Resource.Transfrom;
            buttonChartDataGet.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonChartDataGet.Location = new System.Drawing.Point(566, 349);
            buttonChartDataGet.Name = "buttonChartDataGet";
            buttonChartDataGet.Size = new System.Drawing.Size(75, 24);
            buttonChartDataGet.TabIndex = 4;
            buttonChartDataGet.Text = "      Get data";
            buttonChartDataGet.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonChartDataGet.UseVisualStyleBackColor = true;
            buttonChartDataGet.Click += buttonChartDataGet_Click;
            // 
            // radioButtonChartDataChart
            // 
            radioButtonChartDataChart.AutoSize = true;
            radioButtonChartDataChart.Checked = true;
            radioButtonChartDataChart.Dock = System.Windows.Forms.DockStyle.Fill;
            radioButtonChartDataChart.Image = Resource.Graph;
            radioButtonChartDataChart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            radioButtonChartDataChart.Location = new System.Drawing.Point(3, 15);
            radioButtonChartDataChart.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            radioButtonChartDataChart.Name = "radioButtonChartDataChart";
            radioButtonChartDataChart.Size = new System.Drawing.Size(221, 23);
            radioButtonChartDataChart.TabIndex = 5;
            radioButtonChartDataChart.TabStop = true;
            radioButtonChartDataChart.Text = "      Chart";
            radioButtonChartDataChart.UseVisualStyleBackColor = true;
            // 
            // radioButtonChartDataSettings
            // 
            radioButtonChartDataSettings.AutoSize = true;
            radioButtonChartDataSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            radioButtonChartDataSettings.Enabled = false;
            radioButtonChartDataSettings.Image = Resource.Settings;
            radioButtonChartDataSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            radioButtonChartDataSettings.Location = new System.Drawing.Point(3, 38);
            radioButtonChartDataSettings.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            radioButtonChartDataSettings.Name = "radioButtonChartDataSettings";
            radioButtonChartDataSettings.Size = new System.Drawing.Size(221, 23);
            radioButtonChartDataSettings.TabIndex = 6;
            radioButtonChartDataSettings.Text = "      Settings";
            radioButtonChartDataSettings.UseVisualStyleBackColor = true;
            // 
            // tabControlChartSettings
            // 
            tabControlChartSettings.Controls.Add(tabPageChartTypes);
            tabControlChartSettings.Dock = System.Windows.Forms.DockStyle.Left;
            tabControlChartSettings.ImageList = imageListTab;
            tabControlChartSettings.Location = new System.Drawing.Point(0, 0);
            tabControlChartSettings.Name = "tabControlChartSettings";
            tabControlChartSettings.SelectedIndex = 0;
            tabControlChartSettings.Size = new System.Drawing.Size(100, 409);
            tabControlChartSettings.TabIndex = 14;
            // 
            // tabPageChartTypes
            // 
            tabPageChartTypes.Controls.Add(checkedListBoxChartTransactionType);
            tabPageChartTypes.ImageKey = "TransactionOnLoan.ico";
            tabPageChartTypes.Location = new System.Drawing.Point(4, 24);
            tabPageChartTypes.Name = "tabPageChartTypes";
            tabPageChartTypes.Padding = new System.Windows.Forms.Padding(3);
            tabPageChartTypes.Size = new System.Drawing.Size(92, 381);
            tabPageChartTypes.TabIndex = 0;
            tabPageChartTypes.Text = "Types";
            tabPageChartTypes.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxChartTransactionType
            // 
            checkedListBoxChartTransactionType.CheckOnClick = true;
            checkedListBoxChartTransactionType.Dock = System.Windows.Forms.DockStyle.Fill;
            checkedListBoxChartTransactionType.FormattingEnabled = true;
            checkedListBoxChartTransactionType.IntegralHeight = false;
            checkedListBoxChartTransactionType.Location = new System.Drawing.Point(3, 3);
            checkedListBoxChartTransactionType.Name = "checkedListBoxChartTransactionType";
            checkedListBoxChartTransactionType.Size = new System.Drawing.Size(86, 375);
            checkedListBoxChartTransactionType.TabIndex = 6;
            // 
            // tableLayoutPanelAdministratingAgent
            // 
            tableLayoutPanelAdministratingAgent.ColumnCount = 2;
            tableLayoutPanelAdministratingAgent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelAdministratingAgent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelAdministratingAgent.Controls.Add(labelBalanceAdministration, 0, 0);
            tableLayoutPanelAdministratingAgent.Controls.Add(userControlModuleRelatedEntryBalanceAdministration, 1, 0);
            tableLayoutPanelAdministratingAgent.Dock = System.Windows.Forms.DockStyle.Top;
            tableLayoutPanelAdministratingAgent.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelAdministratingAgent.Name = "tableLayoutPanelAdministratingAgent";
            tableLayoutPanelAdministratingAgent.RowCount = 1;
            tableLayoutPanelAdministratingAgent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelAdministratingAgent.Size = new System.Drawing.Size(847, 28);
            tableLayoutPanelAdministratingAgent.TabIndex = 1;
            // 
            // labelBalanceAdministration
            // 
            labelBalanceAdministration.AutoSize = true;
            labelBalanceAdministration.Dock = System.Windows.Forms.DockStyle.Fill;
            labelBalanceAdministration.Location = new System.Drawing.Point(3, 0);
            labelBalanceAdministration.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            labelBalanceAdministration.Name = "labelBalanceAdministration";
            labelBalanceAdministration.Size = new System.Drawing.Size(89, 25);
            labelBalanceAdministration.TabIndex = 11;
            labelBalanceAdministration.Text = "Administration:";
            labelBalanceAdministration.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlModuleRelatedEntryBalanceAdministration
            // 
            userControlModuleRelatedEntryBalanceAdministration.CanDeleteConnectionToModule = true;
            userControlModuleRelatedEntryBalanceAdministration.DependsOnUri = "";
            userControlModuleRelatedEntryBalanceAdministration.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleRelatedEntryBalanceAdministration.Domain = "";
            userControlModuleRelatedEntryBalanceAdministration.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryBalanceAdministration.Location = new System.Drawing.Point(92, 3);
            userControlModuleRelatedEntryBalanceAdministration.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            userControlModuleRelatedEntryBalanceAdministration.Module = null;
            userControlModuleRelatedEntryBalanceAdministration.Name = "userControlModuleRelatedEntryBalanceAdministration";
            userControlModuleRelatedEntryBalanceAdministration.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryBalanceAdministration.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryBalanceAdministration.ShowInfo = false;
            userControlModuleRelatedEntryBalanceAdministration.Size = new System.Drawing.Size(752, 22);
            userControlModuleRelatedEntryBalanceAdministration.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryBalanceAdministration.TabIndex = 10;
            // 
            // tableLayoutPanelHeader
            // 
            tableLayoutPanelHeader.ColumnCount = 10;
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.Controls.Add(buttonShowSpecimenLists, 9, 0);
            tableLayoutPanelHeader.Controls.Add(buttonShowHierarchy, 8, 0);
            tableLayoutPanelHeader.Controls.Add(buttonTableEditor, 7, 0);
            tableLayoutPanelHeader.Controls.Add(buttonDisplaySettings, 6, 0);
            tableLayoutPanelHeader.Controls.Add(buttonHistory, 5, 0);
            tableLayoutPanelHeader.Controls.Add(buttonHeaderFeedback, 4, 0);
            tableLayoutPanelHeader.Controls.Add(textBoxHeaderID, 2, 0);
            tableLayoutPanelHeader.Controls.Add(labelHeaderID, 1, 0);
            tableLayoutPanelHeader.Controls.Add(labelHeaderTitle, 0, 0);
            tableLayoutPanelHeader.Controls.Add(buttonMaintenance, 3, 0);
            tableLayoutPanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            tableLayoutPanelHeader.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelHeader.Name = "tableLayoutPanelHeader";
            tableLayoutPanelHeader.RowCount = 1;
            tableLayoutPanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelHeader.Size = new System.Drawing.Size(853, 26);
            tableLayoutPanelHeader.TabIndex = 11;
            // 
            // buttonShowSpecimenLists
            // 
            buttonShowSpecimenLists.FlatAppearance.BorderSize = 0;
            buttonShowSpecimenLists.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonShowSpecimenLists.Image = Resource.ListGrey;
            buttonShowSpecimenLists.Location = new System.Drawing.Point(833, 0);
            buttonShowSpecimenLists.Margin = new System.Windows.Forms.Padding(0);
            buttonShowSpecimenLists.Name = "buttonShowSpecimenLists";
            buttonShowSpecimenLists.Size = new System.Drawing.Size(20, 24);
            buttonShowSpecimenLists.TabIndex = 8;
            toolTip.SetToolTip(buttonShowSpecimenLists, "Hide or show the specimen lists");
            buttonShowSpecimenLists.UseVisualStyleBackColor = true;
            buttonShowSpecimenLists.Click += buttonShowSpecimenLists_Click;
            // 
            // buttonShowHierarchy
            // 
            buttonShowHierarchy.FlatAppearance.BorderSize = 0;
            buttonShowHierarchy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonShowHierarchy.Image = Resource.HierarchyGrey;
            buttonShowHierarchy.Location = new System.Drawing.Point(810, 3);
            buttonShowHierarchy.Name = "buttonShowHierarchy";
            buttonShowHierarchy.Size = new System.Drawing.Size(20, 20);
            buttonShowHierarchy.TabIndex = 7;
            toolTip.SetToolTip(buttonShowHierarchy, "Show or hide the hierarchy");
            buttonShowHierarchy.UseVisualStyleBackColor = true;
            buttonShowHierarchy.Click += buttonShowHierarchy_Click;
            // 
            // buttonTableEditor
            // 
            buttonTableEditor.FlatAppearance.BorderSize = 0;
            buttonTableEditor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonTableEditor.Image = Resource.EditInSpeadsheet;
            buttonTableEditor.Location = new System.Drawing.Point(784, 3);
            buttonTableEditor.Name = "buttonTableEditor";
            buttonTableEditor.Size = new System.Drawing.Size(20, 20);
            buttonTableEditor.TabIndex = 6;
            toolTip.SetToolTip(buttonTableEditor, "Edit data in a spreadsheet");
            buttonTableEditor.UseVisualStyleBackColor = true;
            buttonTableEditor.Click += buttonTableEditor_Click;
            // 
            // buttonDisplaySettings
            // 
            buttonDisplaySettings.FlatAppearance.BorderSize = 0;
            buttonDisplaySettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonDisplaySettings.Image = Resource.Settings;
            buttonDisplaySettings.Location = new System.Drawing.Point(758, 3);
            buttonDisplaySettings.Name = "buttonDisplaySettings";
            buttonDisplaySettings.Size = new System.Drawing.Size(20, 20);
            buttonDisplaySettings.TabIndex = 5;
            toolTip.SetToolTip(buttonDisplaySettings, "Edit display settings");
            buttonDisplaySettings.UseVisualStyleBackColor = true;
            buttonDisplaySettings.Click += buttonDisplaySettings_Click;
            // 
            // buttonHistory
            // 
            buttonHistory.FlatAppearance.BorderSize = 0;
            buttonHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonHistory.Image = Resource.History;
            buttonHistory.Location = new System.Drawing.Point(732, 3);
            buttonHistory.Name = "buttonHistory";
            buttonHistory.Size = new System.Drawing.Size(20, 20);
            buttonHistory.TabIndex = 4;
            toolTip.SetToolTip(buttonHistory, "Show the history of the current dataset");
            buttonHistory.UseVisualStyleBackColor = true;
            buttonHistory.Click += buttonHistory_Click;
            // 
            // buttonHeaderFeedback
            // 
            buttonHeaderFeedback.FlatAppearance.BorderSize = 0;
            buttonHeaderFeedback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonHeaderFeedback.Image = Resource.Feedback;
            buttonHeaderFeedback.Location = new System.Drawing.Point(706, 3);
            buttonHeaderFeedback.Name = "buttonHeaderFeedback";
            buttonHeaderFeedback.Size = new System.Drawing.Size(20, 20);
            buttonHeaderFeedback.TabIndex = 0;
            toolTip.SetToolTip(buttonHeaderFeedback, "Send a feedback");
            buttonHeaderFeedback.UseVisualStyleBackColor = true;
            buttonHeaderFeedback.Click += buttonHeaderFeedback_Click;
            // 
            // textBoxHeaderID
            // 
            textBoxHeaderID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            textBoxHeaderID.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionBindingSource, "TransactionID", true));
            textBoxHeaderID.Location = new System.Drawing.Point(627, 3);
            textBoxHeaderID.Name = "textBoxHeaderID";
            textBoxHeaderID.ReadOnly = true;
            textBoxHeaderID.Size = new System.Drawing.Size(55, 23);
            textBoxHeaderID.TabIndex = 3;
            // 
            // labelHeaderID
            // 
            labelHeaderID.Location = new System.Drawing.Point(603, 0);
            labelHeaderID.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            labelHeaderID.Name = "labelHeaderID";
            labelHeaderID.Size = new System.Drawing.Size(21, 24);
            labelHeaderID.TabIndex = 2;
            labelHeaderID.Text = "ID:";
            labelHeaderID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelHeaderTitle
            // 
            labelHeaderTitle.DataBindings.Add(new System.Windows.Forms.Binding("Text", transactionBindingSource, "TransactionTitle", true));
            labelHeaderTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            labelHeaderTitle.Location = new System.Drawing.Point(3, 0);
            labelHeaderTitle.Name = "labelHeaderTitle";
            labelHeaderTitle.Size = new System.Drawing.Size(509, 24);
            labelHeaderTitle.TabIndex = 1;
            labelHeaderTitle.Text = "Transaction";
            labelHeaderTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonMaintenance
            // 
            buttonMaintenance.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonMaintenance.FlatAppearance.BorderSize = 0;
            buttonMaintenance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonMaintenance.Image = Resource.DatabaseTools;
            buttonMaintenance.Location = new System.Drawing.Point(685, 0);
            buttonMaintenance.Margin = new System.Windows.Forms.Padding(0);
            buttonMaintenance.Name = "buttonMaintenance";
            buttonMaintenance.Size = new System.Drawing.Size(18, 26);
            buttonMaintenance.TabIndex = 9;
            toolTip.SetToolTip(buttonMaintenance, "Open maintenance");
            buttonMaintenance.UseVisualStyleBackColor = true;
            buttonMaintenance.Click += buttonMaintenance_Click;
            // 
            // userControlSpecimenList
            // 
            userControlSpecimenList.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlSpecimenList.Location = new System.Drawing.Point(0, 0);
            userControlSpecimenList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            userControlSpecimenList.Name = "userControlSpecimenList";
            userControlSpecimenList.Size = new System.Drawing.Size(96, 100);
            userControlSpecimenList.TabIndex = 0;
            // 
            // collectionSpecimenPartSentListBindingSource
            // 
            collectionSpecimenPartSentListBindingSource.DataMember = "CollectionSpecimenPartSentList";
            collectionSpecimenPartSentListBindingSource.DataSource = dataSetTransaction;
            // 
            // collectionSpecimenPartPartialReturnListBindingSource
            // 
            collectionSpecimenPartPartialReturnListBindingSource.DataMember = "CollectionSpecimenPartPartialReturnList";
            collectionSpecimenPartPartialReturnListBindingSource.DataSource = dataSetTransaction;
            // 
            // labelHeader
            // 
            labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            labelHeader.Location = new System.Drawing.Point(0, 0);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new System.Drawing.Size(1016, 720);
            labelHeader.TabIndex = 3;
            labelHeader.Text = "Select a transaction";
            labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timerSending
            // 
            timerSending.Interval = 200;
            timerSending.Tick += timerSending_Tick;
            // 
            // timerReturn
            // 
            timerReturn.Interval = 200;
            timerReturn.Tick += timerReturn_Tick;
            // 
            // imageListSpecimenList
            // 
            imageListSpecimenList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListSpecimenList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListSpecimenList.ImageStream");
            imageListSpecimenList.TransparentColor = System.Drawing.Color.Transparent;
            imageListSpecimenList.Images.SetKeyName(0, "List.ico");
            imageListSpecimenList.Images.SetKeyName(1, "ListNot.ico");
            // 
            // userControlDialogPanel
            // 
            userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            userControlDialogPanel.Location = new System.Drawing.Point(0, 654);
            userControlDialogPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            userControlDialogPanel.Name = "userControlDialogPanel";
            userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            userControlDialogPanel.Size = new System.Drawing.Size(1070, 26);
            userControlDialogPanel.TabIndex = 0;
            // 
            // timerPrinting
            // 
            timerPrinting.Interval = 200;
            timerPrinting.Tick += timerPrinting_Tick;
            // 
            // transactionTableAdapter
            // 
            transactionTableAdapter.ClearBeforeFill = true;
            // 
            // collectionSpecimenPartOnLoanListTableAdapter
            // 
            collectionSpecimenPartOnLoanListTableAdapter.ClearBeforeFill = true;
            // 
            // collectionSpecimenPartReturnListTableAdapter
            // 
            collectionSpecimenPartReturnListTableAdapter.ClearBeforeFill = true;
            // 
            // collectionSpecimenPartListTableAdapter
            // 
            collectionSpecimenPartListTableAdapter.ClearBeforeFill = true;
            // 
            // transactionDocumentTableAdapter
            // 
            transactionDocumentTableAdapter.ClearBeforeFill = true;
            // 
            // collectionSpecimenPartPartialReturnListTableAdapter
            // 
            collectionSpecimenPartPartialReturnListTableAdapter.ClearBeforeFill = true;
            // 
            // timerRequest
            // 
            timerRequest.Tick += timerRequest_Tick;
            // 
            // collectionSpecimenPartSentListTableAdapter
            // 
            collectionSpecimenPartSentListTableAdapter.ClearBeforeFill = true;
            // 
            // imageListSetDate
            // 
            imageListSetDate.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListSetDate.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListSetDate.ImageStream");
            imageListSetDate.TransparentColor = System.Drawing.Color.Transparent;
            imageListSetDate.Images.SetKeyName(0, "Time.ico");
            imageListSetDate.Images.SetKeyName(1, "Delete.ico");
            // 
            // collectionSpecimenPartForwardingListTableAdapter
            // 
            collectionSpecimenPartForwardingListTableAdapter.ClearBeforeFill = true;
            // 
            // collectionSpecimenPartTransactionReturnListTableAdapter
            // 
            collectionSpecimenPartTransactionReturnListTableAdapter.ClearBeforeFill = true;
            // 
            // transactionAgentTableAdapter
            // 
            transactionAgentTableAdapter.ClearBeforeFill = true;
            // 
            // transactionPaymentTableAdapter
            // 
            transactionPaymentTableAdapter.ClearBeforeFill = true;
            // 
            // openFileDialogDocument
            // 
            openFileDialogDocument.FileName = "openFileDialogDocument";
            // 
            // checkBoxSendingAllUnits
            // 
            checkBoxSendingAllUnits.AutoSize = true;
            tableLayoutPanelSending.SetColumnSpan(checkBoxSendingAllUnits, 2);
            checkBoxSendingAllUnits.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxSendingAllUnits.Location = new System.Drawing.Point(3, 58);
            checkBoxSendingAllUnits.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            checkBoxSendingAllUnits.Name = "checkBoxSendingAllUnits";
            checkBoxSendingAllUnits.Size = new System.Drawing.Size(100, 20);
            checkBoxSendingAllUnits.TabIndex = 21;
            checkBoxSendingAllUnits.Text = "All Units";
            checkBoxSendingAllUnits.UseVisualStyleBackColor = true;
            // 
            // FormTransaction
            // 
            ClientSize = new System.Drawing.Size(1070, 680);
            Controls.Add(splitContainerMain);
            Controls.Add(userControlDialogPanel);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "FormTransaction";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = " Administration of transactions";
            FormClosing += FormTransaction_FormClosing;
            Load += FormTransaction_Load_1;
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            splitContainerData.Panel1.ResumeLayout(false);
            splitContainerData.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerData).EndInit();
            splitContainerData.ResumeLayout(false);
            groupBoxData.ResumeLayout(false);
            splitContainerTransaction.Panel1.ResumeLayout(false);
            splitContainerTransaction.Panel1.PerformLayout();
            splitContainerTransaction.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerTransaction).EndInit();
            splitContainerTransaction.ResumeLayout(false);
            toolStripTransaction.ResumeLayout(false);
            toolStripTransaction.PerformLayout();
            tabControlTransaction.ResumeLayout(false);
            tabPageDataEntry.ResumeLayout(false);
            tableLayoutPanelCollection.ResumeLayout(false);
            tableLayoutPanelCollection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)transactionBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataSetTransaction).EndInit();
            groupBoxFrom.ResumeLayout(false);
            tableLayoutPanelFrom.ResumeLayout(false);
            tableLayoutPanelFrom.PerformLayout();
            groupBoxTo.ResumeLayout(false);
            tableLayoutPanelTo.ResumeLayout(false);
            tableLayoutPanelTo.PerformLayout();
            tabPageSending.ResumeLayout(false);
            splitContainerSending.Panel1.ResumeLayout(false);
            splitContainerSending.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerSending).EndInit();
            splitContainerSending.ResumeLayout(false);
            tableLayoutPanelSending.ResumeLayout(false);
            tableLayoutPanelSending.PerformLayout();
            toolStripSending.ResumeLayout(false);
            toolStripSending.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSendingRestrictToMaterial).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSendingRestrictToCollection).EndInit();
            splitContainerWebbrowserSending.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerWebbrowserSending).EndInit();
            splitContainerWebbrowserSending.ResumeLayout(false);
            panelWebbrowserSending.ResumeLayout(false);
            tableLayoutPanelSendingList.ResumeLayout(false);
            tableLayoutPanelSendingList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)collectionSpecimenPartOnLoanListBindingSource).EndInit();
            toolStripSendingList.ResumeLayout(false);
            toolStripSendingList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)collectionSpecimenPartTransactionReturnListBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)collectionSpecimenPartForwardingListBindingSource).EndInit();
            tabPageForwarding.ResumeLayout(false);
            tableLayoutPanelForwarding.ResumeLayout(false);
            tableLayoutPanelForwarding.PerformLayout();
            toolStripForwarding.ResumeLayout(false);
            toolStripForwarding.PerformLayout();
            splitContainerForwarding.Panel1.ResumeLayout(false);
            splitContainerForwarding.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerForwarding).EndInit();
            splitContainerForwarding.ResumeLayout(false);
            panel2.ResumeLayout(false);
            tableLayoutPanelForwardingLists.ResumeLayout(false);
            tableLayoutPanelForwardingLists.PerformLayout();
            toolStripForwardingSpecimenForwarded.ResumeLayout(false);
            toolStripForwardingSpecimenForwarded.PerformLayout();
            tabPageConfirmation.ResumeLayout(false);
            tableLayoutPanelConfirmation.ResumeLayout(false);
            tableLayoutPanelConfirmation.PerformLayout();
            toolStripConfirmation.ResumeLayout(false);
            toolStripConfirmation.PerformLayout();
            splitContainerConfirmation.Panel1.ResumeLayout(false);
            splitContainerConfirmation.Panel2.ResumeLayout(false);
            splitContainerConfirmation.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerConfirmation).EndInit();
            splitContainerConfirmation.ResumeLayout(false);
            panelReminderWebbrowser.ResumeLayout(false);
            tabPageReminder.ResumeLayout(false);
            tableLayoutPanelAdmonition.ResumeLayout(false);
            tableLayoutPanelAdmonition.PerformLayout();
            toolStripReminder.ResumeLayout(false);
            toolStripReminder.PerformLayout();
            splitContainerReminder.Panel1.ResumeLayout(false);
            splitContainerReminder.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerReminder).EndInit();
            splitContainerReminder.ResumeLayout(false);
            panelAdmonitionWebbrowser.ResumeLayout(false);
            tableLayoutPanelReminderList.ResumeLayout(false);
            tableLayoutPanelReminderList.PerformLayout();
            tabPagePrinting.ResumeLayout(false);
            splitContainerPrinting.Panel1.ResumeLayout(false);
            splitContainerPrinting.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerPrinting).EndInit();
            splitContainerPrinting.ResumeLayout(false);
            panelWebbrowserPrint.ResumeLayout(false);
            tableLayoutPanelPrinting.ResumeLayout(false);
            tableLayoutPanelPrinting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIncludeSubCollections).EndInit();
            toolStripPrinting.ResumeLayout(false);
            toolStripPrinting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIncludeSubTransactions).EndInit();
            tableLayoutPanelPrintingList.ResumeLayout(false);
            tableLayoutPanelPrintingList.PerformLayout();
            toolStripSpecimenList.ResumeLayout(false);
            toolStripSpecimenList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)collectionSpecimenPartListBindingSource).EndInit();
            tabPageDocuments.ResumeLayout(false);
            splitContainerDocuments.Panel1.ResumeLayout(false);
            splitContainerDocuments.Panel1.PerformLayout();
            splitContainerDocuments.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerDocuments).EndInit();
            splitContainerDocuments.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)transactionDocumentBindingSource).EndInit();
            toolStripTransactionDocument.ResumeLayout(false);
            toolStripTransactionDocument.PerformLayout();
            splitContainerTransactionDocumentData.Panel1.ResumeLayout(false);
            splitContainerTransactionDocumentData.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerTransactionDocumentData).EndInit();
            splitContainerTransactionDocumentData.ResumeLayout(false);
            splitContainerTransactionDocuments.Panel1.ResumeLayout(false);
            splitContainerTransactionDocuments.Panel2.ResumeLayout(false);
            splitContainerTransactionDocuments.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerTransactionDocuments).EndInit();
            splitContainerTransactionDocuments.ResumeLayout(false);
            panelHistoryWebbrowser.ResumeLayout(false);
            splitContainerDocumentBrowser.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerDocumentBrowser).EndInit();
            splitContainerDocumentBrowser.ResumeLayout(false);
            panelHistoryImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxTransactionDocuments).EndInit();
            toolStripDocumentImage.ResumeLayout(false);
            toolStripDocumentImage.PerformLayout();
            tableLayoutPanelTransactionDocument.ResumeLayout(false);
            tableLayoutPanelTransactionDocument.PerformLayout();
            tableLayoutPanelTransactionDocumentButtons.ResumeLayout(false);
            tabPageTransactionReturn.ResumeLayout(false);
            tableLayoutPanelTransactionReturn.ResumeLayout(false);
            tableLayoutPanelTransactionReturn.PerformLayout();
            toolStripTransactionReturn.ResumeLayout(false);
            toolStripTransactionReturn.PerformLayout();
            splitContainerTransactionReturn.Panel1.ResumeLayout(false);
            splitContainerTransactionReturn.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerTransactionReturn).EndInit();
            splitContainerTransactionReturn.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tableLayoutPanelTransactionReturnLists.ResumeLayout(false);
            tableLayoutPanelTransactionReturnLists.PerformLayout();
            toolStripTransactionReturnListReturned.ResumeLayout(false);
            toolStripTransactionReturnListReturned.PerformLayout();
            tabPageBalance.ResumeLayout(false);
            tableLayoutPanelBalance.ResumeLayout(false);
            tableLayoutPanelBalance.PerformLayout();
            toolStripBalance.ResumeLayout(false);
            toolStripBalance.PerformLayout();
            splitContainerBalance.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerBalance).EndInit();
            splitContainerBalance.ResumeLayout(false);
            panelBalance.ResumeLayout(false);
            tabPageRequest.ResumeLayout(false);
            splitContainerRequest.Panel1.ResumeLayout(false);
            splitContainerRequest.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerRequest).EndInit();
            splitContainerRequest.ResumeLayout(false);
            panelRequest.ResumeLayout(false);
            tableLayoutPanelRequest.ResumeLayout(false);
            tableLayoutPanelRequest.PerformLayout();
            toolStripRequest.ResumeLayout(false);
            toolStripRequest.PerformLayout();
            tableLayoutPanelRequestList.ResumeLayout(false);
            tableLayoutPanelRequestList.PerformLayout();
            toolStripSpecimenListRequest.ResumeLayout(false);
            toolStripSpecimenListRequest.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)collectionSpecimenPartReturnListBindingSource).EndInit();
            tabPageNoAccess.ResumeLayout(false);
            tableLayoutPanelNoAccess.ResumeLayout(false);
            tableLayoutPanelNoAccess.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxNoAccess).EndInit();
            tabPagePayment.ResumeLayout(false);
            splitContainerPayment.Panel1.ResumeLayout(false);
            splitContainerPayment.Panel1.PerformLayout();
            splitContainerPayment.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerPayment).EndInit();
            splitContainerPayment.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)transactionPaymentBindingSource).EndInit();
            toolStripPayment.ResumeLayout(false);
            toolStripPayment.PerformLayout();
            tableLayoutPanelPayment.ResumeLayout(false);
            tableLayoutPanelPayment.PerformLayout();
            tabPageAgents.ResumeLayout(false);
            splitContainerAgents.Panel1.ResumeLayout(false);
            splitContainerAgents.Panel1.PerformLayout();
            splitContainerAgents.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerAgents).EndInit();
            splitContainerAgents.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)transactionAgentBindingSource).EndInit();
            toolStripAgents.ResumeLayout(false);
            toolStripAgents.PerformLayout();
            tableLayoutPanelAgents.ResumeLayout(false);
            tableLayoutPanelAgents.PerformLayout();
            tabPageIdentifier.ResumeLayout(false);
            splitContainerIdentifier.Panel1.ResumeLayout(false);
            splitContainerIdentifier.Panel1.PerformLayout();
            splitContainerIdentifier.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerIdentifier).EndInit();
            splitContainerIdentifier.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)externalIdentifierBindingSource).EndInit();
            toolStripIdentifier.ResumeLayout(false);
            toolStripIdentifier.PerformLayout();
            tableLayoutPanelIdentifier.ResumeLayout(false);
            tableLayoutPanelIdentifier.PerformLayout();
            tabPageChart.ResumeLayout(false);
            tabControlChart.ResumeLayout(false);
            tabPageChartResult.ResumeLayout(false);
            tableLayoutPanelChart.ResumeLayout(false);
            tableLayoutPanelChart.PerformLayout();
            groupBoxChartGrouping.ResumeLayout(false);
            tableLayoutPanelChartGrouping.ResumeLayout(false);
            tableLayoutPanelChartGrouping.PerformLayout();
            groupBoxChartStart.ResumeLayout(false);
            groupBoxChartStart.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownChartStart).EndInit();
            tabControlChartSettingsForChart.ResumeLayout(false);
            tabPageChartAdminColl.ResumeLayout(false);
            tabPageChartData.ResumeLayout(false);
            tableLayoutPanelChartData.ResumeLayout(false);
            tableLayoutPanelChartData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewChartData).EndInit();
            tabControlChartData.ResumeLayout(false);
            tabPageChartDataAddress.ResumeLayout(false);
            tableLayoutPanelChartDataAddress.ResumeLayout(false);
            tableLayoutPanelChartDataAddress.PerformLayout();
            toolStripChartAddressSelf.ResumeLayout(false);
            toolStripChartAddressSelf.PerformLayout();
            tabControlChartSettings.ResumeLayout(false);
            tabPageChartTypes.ResumeLayout(false);
            tableLayoutPanelAdministratingAgent.ResumeLayout(false);
            tableLayoutPanelAdministratingAgent.PerformLayout();
            tableLayoutPanelHeader.ResumeLayout(false);
            tableLayoutPanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)collectionSpecimenPartSentListBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)collectionSpecimenPartPartialReturnListBindingSource).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private DiversityWorkbench.UserControls.UserControlQueryList userControlQueryList;
        private System.Windows.Forms.SplitContainer splitContainerData;
        private System.Windows.Forms.GroupBox groupBoxData;
        private System.Windows.Forms.SplitContainer splitContainerTransaction;
        private System.Windows.Forms.TreeView treeViewTransaction;
        private System.Windows.Forms.ToolStrip toolStripTransaction;
        private System.Windows.Forms.ToolStripButton toolStripButtonNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonSpecimenList;
        private DiversityCollection.UserControls.UserControlSpecimenList userControlSpecimenList;
        private Datasets.DataSetTransaction dataSetTransaction;
        //private System.Windows.Forms.ImageList imageListTabpages;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.OpenFileDialog openFileDialogSendingSchema;
        private System.Windows.Forms.Timer timerSending;
        private System.Windows.Forms.Timer timerReturn;
        //private System.Windows.Forms.ImageList imageListSpecimenList;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.BindingSource transactionBindingSource;
        private DiversityCollection.Datasets.DataSetTransactionTableAdapters.TransactionTableAdapter transactionTableAdapter;
        private System.Windows.Forms.TabControl tabControlTransaction;
        private System.Windows.Forms.TabPage tabPageDataEntry;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCollection;
        private System.Windows.Forms.Label labelTransactionTitle;
        private System.Windows.Forms.TextBox textBoxTransactionTitle;
        private System.Windows.Forms.Label labelBeginDate;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryFromPartner;
        private System.Windows.Forms.DateTimePicker dateTimePickerBeginDate;
        private System.Windows.Forms.Label labelTransactionComment;
        private System.Windows.Forms.TextBox textBoxTransactionComment;
        private System.Windows.Forms.Label labelInternalNotes;
        private System.Windows.Forms.TextBox textBoxInternalNotes;
        private System.Windows.Forms.Label labelResponsible;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryResponsible;
        private System.Windows.Forms.ComboBox comboBoxTransactionCommentAdd;
        private System.Windows.Forms.Button buttonTransactionCommentInsertSelected;
        private System.Windows.Forms.Label labelTransactionType;
        private System.Windows.Forms.ComboBox comboBoxTransactionType;
        private System.Windows.Forms.Label labelReportingCategory;
        private System.Windows.Forms.ComboBox comboBoxReportingCategory;
        private System.Windows.Forms.Label labelMaterialDescription;
        private System.Windows.Forms.TextBox textBoxMaterialDescription;
        private System.Windows.Forms.Label labelMaterialCategory;
        private System.Windows.Forms.ComboBox comboBoxMaterialCategory;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryToPartner;
        private System.Windows.Forms.ComboBox comboBoxFromCollection;
        private System.Windows.Forms.ComboBox comboBoxToCollection;
        private System.Windows.Forms.TextBox textBoxFromTransactionNumber;
        private System.Windows.Forms.TextBox textBoxToTransactionNumber;
        private System.Windows.Forms.TextBox textBoxNumberOfUnits;
        private System.Windows.Forms.Label labelNumberOfUnits;
        private System.Windows.Forms.Label labelAgreedEndDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerAgreedEndDate;
        private System.Windows.Forms.Label labelAdministratingCollection;
        private System.Windows.Forms.ComboBox comboBoxAdministratingCollection;
        private System.Windows.Forms.Label labelActualEndDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerActualEndDate;
        private System.Windows.Forms.TextBox textBoxInvestigator;
        private System.Windows.Forms.Label labelInvestigator;
        private System.Windows.Forms.TabPage tabPageSending;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSending;
        private System.Windows.Forms.Panel panelWebbrowserSending;
        private System.Windows.Forms.WebBrowser webBrowserSending;
        private System.Windows.Forms.Label labelSendingSchema;
        private System.Windows.Forms.TextBox textBoxSendingSchema;
        private System.Windows.Forms.TextBox textBoxSendingAccessionNumber;
        private System.Windows.Forms.ListBox listBoxSendingSpecimen;
        private System.Windows.Forms.ComboBox comboBoxSendingAccessionNumber;
        private System.Windows.Forms.CheckBox checkBoxSendingRestrictToCollection;
        private System.Windows.Forms.CheckBox checkBoxSendingRestrictToMaterial;
        private System.Windows.Forms.PictureBox pictureBoxSendingRestrictToMaterial;
        private System.Windows.Forms.PictureBox pictureBoxSendingRestrictToCollection;
        private System.Windows.Forms.TabPage tabPageConfirmation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelConfirmation;
        private System.Windows.Forms.Label labelConfirmationSchema;
        private System.Windows.Forms.TextBox textBoxConfirmationSchema;
        private System.Windows.Forms.ToolStrip toolStripConfirmation;
        private System.Windows.Forms.ToolStripButton toolStripButtonConfirmationOpenSchema;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorConfirmation;
        private System.Windows.Forms.ToolStripButton toolStripButtonConfirmationPreview;
        private System.Windows.Forms.ToolStripButton toolStripButtonConfirmationPrint;
        private System.Windows.Forms.ToolStripButton toolStripButtonConfirmationSave;
        private System.Windows.Forms.TabPage tabPageReminder;
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
        private System.Windows.Forms.TabPage tabPagePrinting;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPrinting;
        private System.Windows.Forms.ToolStrip toolStripPrinting;
        private System.Windows.Forms.ToolStripButton toolStripButtonNewSchemaFile;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpenSchemaFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorLabel;
        private System.Windows.Forms.ToolStripButton toolStripButtonPrintingPreview;
        private System.Windows.Forms.ToolStripButton toolStripButtonPageSetup;
        private System.Windows.Forms.ToolStripButton toolStripButtonPrintingPrint;
        private System.Windows.Forms.ToolStripButton toolStripButtonPrintingSave;
        private System.Windows.Forms.TextBox textBoxPrintingSchemaFile;
        private System.Windows.Forms.Label labelPrintingSchemaFile;
        private System.Windows.Forms.Panel panelWebbrowserPrint;
        private System.Windows.Forms.WebBrowser webBrowserPrint;
        private System.Windows.Forms.TabPage tabPageDocuments;
        private System.Windows.Forms.SplitContainer splitContainerDocuments;
        private System.Windows.Forms.ListBox listBoxTransactionDocuments;
        private System.Windows.Forms.ToolStrip toolStripTransactionDocument;
        private System.Windows.Forms.ToolStripButton toolStripButtonTransactionDocumentNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonTransactionDocumentDelete;
        private System.Windows.Forms.SplitContainer splitContainerTransactionDocumentData;
        private System.Windows.Forms.SplitContainer splitContainerTransactionDocuments;
        private System.Windows.Forms.Panel panelHistoryWebbrowser;
        private System.Windows.Forms.WebBrowser webBrowserTransactionDocuments;
        private System.Windows.Forms.Panel panelHistoryImage;
        private System.Windows.Forms.PictureBox pictureBoxTransactionDocuments;
        private System.Windows.Forms.ToolStrip toolStripDocumentImage;
        private System.Windows.Forms.ToolStripButton toolStripButtonDocumentZoomAdapt;
        private System.Windows.Forms.ToolStripButton toolStripButtonDocumentZoom100Percent;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorDocument;
        private System.Windows.Forms.ToolStripButton toolStripButtonDocumentRemove;
        private System.Windows.Forms.TextBox textBoxTransactionDocumentInternalNotes;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTransactionDocumentButtons;
        private System.Windows.Forms.Button buttonTransactionDocumentInsertDocument;
        private System.Windows.Forms.TabPage tabPageBalance;
        private System.Windows.Forms.Label labelReminderSpecimenOnLoan;
        private System.Windows.Forms.ListBox listBoxReminderSpecimenOnLoan;
        private System.Windows.Forms.Label labelReminderSpecimenReturned;
        private System.Windows.Forms.ListBox listBoxReminderSpecimenReturned;
        private System.Windows.Forms.ToolStripButton toolStripButtonCopy;
        private System.Windows.Forms.SplitContainer splitContainerSending;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSendingList;
        private System.Windows.Forms.SplitContainer splitContainerConfirmation;
        private System.Windows.Forms.Panel panelReminderWebbrowser;
        private System.Windows.Forms.WebBrowser webBrowserConfirmation;
        private System.Windows.Forms.ListBox listBoxConfirmationSpecimenList;
        private System.Windows.Forms.Label labelConfirmationSpecimenList;
        private System.Windows.Forms.SplitContainer splitContainerReminder;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelReminderList;
        private System.Windows.Forms.SplitContainer splitContainerPrinting;
        private System.Windows.Forms.ListBox listBoxPrintingList;
        private System.Windows.Forms.Label labelPrintingList;
        private System.Windows.Forms.ImageList imageListSpecimenList;
        private System.Windows.Forms.ImageList imageListTab;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBalance;
        private System.Windows.Forms.Label labelBalanceSchema;
        private System.Windows.Forms.TextBox textBoxBalanceSchema;
        private System.Windows.Forms.ToolStrip toolStripBalance;
        private System.Windows.Forms.ToolStripButton toolStripButtonBalanceNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonBalanceOpenSchema;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorBalance;
        private System.Windows.Forms.ToolStripButton toolStripButtonBalanceCreatePreview;
        private System.Windows.Forms.ToolStripButton toolStripButtonBalanceSetPage;
        private System.Windows.Forms.ToolStripButton toolStripButtonBalancePrint;
        private System.Windows.Forms.ToolStripButton toolStripButtonBalanceSave;
        private System.Windows.Forms.SplitContainer splitContainerBalance;
        private System.Windows.Forms.Panel panelBalance;
        private System.Windows.Forms.WebBrowser webBrowserBalance;
        private System.Windows.Forms.CheckBox checkBoxBalanceIncludeFromSubcollections;
        private System.Windows.Forms.CheckBox checkBoxBalanceIncludeToSubcollections;
        private System.Windows.Forms.CheckBox checkBoxBalanceFromIncludeAllCollections;
        private System.Windows.Forms.CheckBox checkBoxBalanceToIncludeAllCollections;
        private System.Windows.Forms.TextBox textBoxPrintingAccessionNumber;
        private System.Windows.Forms.ComboBox comboBoxPrintingAccessionNumber;
        private System.Windows.Forms.Timer timerPrinting;
        private System.Windows.Forms.CheckBox checkBoxPrintingIncludeSubcollections;
        private System.Windows.Forms.CheckBox checkBoxIncludeTypeInformation;
        private System.Windows.Forms.Label labelMaterialCollectors;
        private System.Windows.Forms.TextBox textBoxMaterialCollectors;
        private DiversityWorkbench.UserControls.UserControlHierarchySelector userControlHierarchySelectorFromCollection;
        private DiversityWorkbench.UserControls.UserControlHierarchySelector userControlHierarchySelectorToCollection;
        private System.Windows.Forms.Button buttonHeaderFeedback;
        private System.Windows.Forms.Label labelHeaderTitle;
        private System.Windows.Forms.Label labelHeaderID;
        private System.Windows.Forms.TextBox textBoxHeaderID;
        private System.Windows.Forms.Label labelPrintingInclude;
        private System.Windows.Forms.CheckBox checkBoxPrintingIncludeChildTransactions;
        private System.Windows.Forms.ToolStrip toolStripSending;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpenSendingSchemaFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorSending1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSendingPreview;
        private System.Windows.Forms.ToolStripButton toolStripButtonSendingPrint;
        private System.Windows.Forms.ToolStripButton toolStripButtonSendingSave;
        private System.Windows.Forms.BindingSource collectionSpecimenPartOnLoanListBindingSource;
        private DiversityCollection.Datasets.DataSetTransactionTableAdapters.CollectionSpecimenPartOnLoanListTableAdapter collectionSpecimenPartOnLoanListTableAdapter;
        private System.Windows.Forms.BindingSource collectionSpecimenPartReturnListBindingSource;
        private DiversityCollection.Datasets.DataSetTransactionTableAdapters.CollectionSpecimenPartReturnListTableAdapter collectionSpecimenPartReturnListTableAdapter;
        private System.Windows.Forms.BindingSource collectionSpecimenPartListBindingSource;
        private DiversityCollection.Datasets.DataSetTransactionTableAdapters.CollectionSpecimenPartListTableAdapter collectionSpecimenPartListTableAdapter;
        private System.Windows.Forms.BindingSource transactionDocumentBindingSource;
        private DiversityCollection.Datasets.DataSetTransactionTableAdapters.TransactionDocumentTableAdapter transactionDocumentTableAdapter;
        private System.Windows.Forms.ToolStrip toolStripSendingList;
        private System.Windows.Forms.ToolStripButton toolStripButtonSendingListDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonSendingListFind;
        private System.Windows.Forms.ToolStrip toolStripSpecimenList;
        private System.Windows.Forms.ToolStripButton toolStripButtonSpecimenListDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonSpecimenListSearch;
        private System.Windows.Forms.BindingSource collectionSpecimenPartPartialReturnListBindingSource;
        private DiversityCollection.Datasets.DataSetTransactionTableAdapters.CollectionSpecimenPartPartialReturnListTableAdapter collectionSpecimenPartPartialReturnListTableAdapter;
        private System.Windows.Forms.PictureBox pictureBoxIncludeSubCollections;
        private System.Windows.Forms.PictureBox pictureBoxIncludeSubTransactions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPrintingList;
        private System.Windows.Forms.ToolStripButton toolStripButtonSendingScanner;
        private System.Windows.Forms.ToolStripButton toolStripButtonScannerPrinting;
        private System.Windows.Forms.TabPage tabPageRequest;
        private System.Windows.Forms.SplitContainer splitContainerRequest;
        private System.Windows.Forms.Panel panelRequest;
        private System.Windows.Forms.WebBrowser webBrowserRequest;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRequest;
        private System.Windows.Forms.Label labelRequestSchema;
        private System.Windows.Forms.TextBox textBoxRequestSchema;
        private System.Windows.Forms.ToolStrip toolStripRequest;
        private System.Windows.Forms.ToolStripButton toolStripButtonRequestOpenSchema;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorRequest;
        private System.Windows.Forms.ToolStripButton toolStripButtonRequestPreview;
        private System.Windows.Forms.ToolStripButton toolStripButtonRequestPrint;
        private System.Windows.Forms.ToolStripButton toolStripButtonRequestSave;
        private System.Windows.Forms.ToolStripButton toolStripButtonRequestScanner;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRequestList;
        private System.Windows.Forms.ComboBox comboBoxRequestAccessionNumber;
        private System.Windows.Forms.ToolStrip toolStripSpecimenListRequest;
        private System.Windows.Forms.ToolStripButton toolStripButtonRequestListDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonRequestListFind;
        private System.Windows.Forms.ListBox listBoxRequest;
        private System.Windows.Forms.TextBox textBoxRequestAccessionNumber;
        private System.Windows.Forms.Label labelRequestSpecimenList;
        private System.Windows.Forms.Timer timerRequest;
        private System.Windows.Forms.Label labelRequestFrom;
        private System.Windows.Forms.Label labelRequestTo;
        private System.Windows.Forms.ComboBox comboBoxRequestFrom;
        private System.Windows.Forms.ComboBox comboBoxRequestTo;
        private System.Windows.Forms.Label labelRequestNumber;
        private System.Windows.Forms.TextBox textBoxRequestNumber;
        private System.Windows.Forms.Label labelRequestCollection;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryRequestTo;
        private System.Windows.Forms.Label labelRequestAgents;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryRequestFrom;
        private System.Windows.Forms.ListBox listBoxRequestOnLoan;
        private System.Windows.Forms.Label labelRequestSpecimenOnLoan;
        private System.Windows.Forms.TextBox textBoxRequestType;
        private System.Windows.Forms.Label labelPrintingConversion;
        private System.Windows.Forms.ComboBox comboBoxPrintingConversion;
        private System.Windows.Forms.CheckBox checkBoxSendingOrderByTaxa;
        private System.Windows.Forms.Button buttonHistory;
        private System.Windows.Forms.ToolStripButton toolStripButtonReminderShowList;
        private System.Windows.Forms.ToolStripButton toolStripButtonSendingShowList;
        private System.Windows.Forms.ToolStripButton toolStripButtonConfirmationShowList;
        private System.Windows.Forms.ToolStripButton toolStripButtonPrintingShowList;
        private System.Windows.Forms.ToolStripButton toolStripButtonRequestShowList;
        private System.Windows.Forms.BindingSource collectionSpecimenPartSentListBindingSource;
        private DiversityCollection.Datasets.DataSetTransactionTableAdapters.CollectionSpecimenPartSentListTableAdapter collectionSpecimenPartSentListTableAdapter;
        private System.Windows.Forms.ToolStripButton toolStripButtonSendingXslEditor;
        private System.Windows.Forms.RadioButton radioButtonPrintingGroupedByTaxa;
        private System.Windows.Forms.RadioButton radioButtonPrintingGroupedByNumber;
        private System.Windows.Forms.RadioButton radioButtonPrintingSingleNumbers;
        private System.Windows.Forms.ToolStripButton toolStripButtonSetParent;
        private System.Windows.Forms.TabPage tabPageForwarding;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelForwarding;
        private System.Windows.Forms.Label labelForwardingSchema;
        private System.Windows.Forms.TextBox textBoxForwardingSchema;
        private System.Windows.Forms.ToolStrip toolStripForwarding;
        private System.Windows.Forms.ToolStripButton toolStripButtonForwardingOpenSchema;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonForwardingShowLists;
        private System.Windows.Forms.ToolStripButton toolStripButtonForwardingPreview;
        private System.Windows.Forms.ToolStripButton toolStripButtonForwardingPrint;
        private System.Windows.Forms.ToolStripButton toolStripButtonForwardingSave;
        private System.Windows.Forms.SplitContainer splitContainerForwarding;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.WebBrowser webBrowserForwarding;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelForwardingLists;
        private System.Windows.Forms.ToolStrip toolStripForwardingSpecimenForwarded;
        private System.Windows.Forms.ToolStripLabel toolStripLabelForwardingSpecimenForwarded;
        private System.Windows.Forms.ToolStripButton toolStripButtonForwardingForwarded;
        private System.Windows.Forms.Label labelForwardingSpecimenListOnLoan;
        private System.Windows.Forms.ListBox listBoxForwardingSpecimenForwarded;
        private System.Windows.Forms.ListBox listBoxForwardingSpecimenOnLoan;
        private System.Windows.Forms.TabPage tabPageTransactionReturn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTransactionReturn;
        private System.Windows.Forms.Label labelTransactionReturnSchema;
        private System.Windows.Forms.TextBox textBoxTransactionReturnSchema;
        private System.Windows.Forms.ToolStrip toolStripTransactionReturn;
        private System.Windows.Forms.ToolStripButton toolStripButtonTransactionReturnOpenSchema;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonTransactionReturnShowLists;
        private System.Windows.Forms.ToolStripButton toolStripButtonTransactionReturnPreview;
        private System.Windows.Forms.ToolStripButton toolStripButtonTransactionReturnPrint;
        private System.Windows.Forms.ToolStripButton toolStripButtonTransactionReturnSave;
        private System.Windows.Forms.SplitContainer splitContainerTransactionReturn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.WebBrowser webBrowserTransactionReturn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTransactionReturnLists;
        private System.Windows.Forms.ToolStrip toolStripTransactionReturnListReturned;
        private System.Windows.Forms.ToolStripLabel toolStripLabelTransactionReturnListReturned;
        private System.Windows.Forms.ToolStripButton toolStripButtonTransactionReturnSpecimen;
        private System.Windows.Forms.Label labelTransactionReturnListOnLoan;
        private System.Windows.Forms.ListBox listBoxTransactionReturnReturned;
        private System.Windows.Forms.ListBox listBoxTransactionReturnOnLoan;
        private System.Windows.Forms.ImageList imageListSetDate;
        private System.Windows.Forms.ToolStripButton toolStripButtonForwardingRemove;
        private System.Windows.Forms.ToolStripButton toolStripButtonTransactionReturnRemove;
        private System.Windows.Forms.BindingSource collectionSpecimenPartForwardingListBindingSource;
        private Datasets.DataSetTransactionTableAdapters.CollectionSpecimenPartForwardingListTableAdapter collectionSpecimenPartForwardingListTableAdapter;
        private System.Windows.Forms.ComboBox comboBoxAgreedEndDateAddMonths;
        private System.Windows.Forms.CheckBox checkBoxSendingSingleLines;
        private System.Windows.Forms.Button buttonBeginDateEdit;
        private System.Windows.Forms.Button buttonActualEndDateEdit;
        private System.Windows.Forms.Button buttonAgreedEndDateEdit;
        private System.Windows.Forms.Label labelSendingSpecimenReturned;
        private System.Windows.Forms.ListBox listBoxSendingSpecimenReturned;
        private System.Windows.Forms.BindingSource collectionSpecimenPartTransactionReturnListBindingSource;
        private Datasets.DataSetTransactionTableAdapters.CollectionSpecimenPartTransactionReturnListTableAdapter collectionSpecimenPartTransactionReturnListTableAdapter;
        private System.Windows.Forms.ToolStripButton toolStripButtonReturnScanner;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxReturnScanner;
        private System.Windows.Forms.CheckBox checkBoxReturnSingleLines;
        private System.Windows.Forms.CheckBox checkBoxReturnOrderByTaxa;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTransactionDocument;
        private System.Windows.Forms.Label labelTransactionDocumentDisplayText;
        private System.Windows.Forms.TextBox textBoxTransactionDocumentDisplayText;
        private System.Windows.Forms.CheckBox checkBoxForwardingSingleLines;
        private System.Windows.Forms.CheckBox checkBoxForwardingOrderByTaxa;
        private System.Windows.Forms.ComboBox comboBoxActualEndDateAddMonths;
        private System.Windows.Forms.Label labelCommentPhrases;
        private System.Windows.Forms.GroupBox groupBoxFrom;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelFrom;
        private System.Windows.Forms.Label labelFromTransactionNumber;
        private System.Windows.Forms.Label labelFromCollection;
        private System.Windows.Forms.GroupBox groupBoxTo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTo;
        private System.Windows.Forms.Label labelToTransactionNumber;
        private System.Windows.Forms.Label labelToCollection;
        private System.Windows.Forms.Label labelToRecipient;
        private System.Windows.Forms.ComboBox comboBoxToRecipient;
        private System.Windows.Forms.CheckBox checkBoxSendingIncludeType;
        private System.Windows.Forms.CheckBox checkBoxForwardingIncludeType;
        private System.Windows.Forms.CheckBox checkBoxReturnIncludeType;
        private System.Windows.Forms.ToolStripButton toolStripButtonSendingPreviewAllOnLoan;
        private System.Windows.Forms.Label labelSendingSpecimenForwarded;
        private System.Windows.Forms.ListBox listBoxSendingSpecimenForwarded;
        private System.Windows.Forms.Button buttonTransactionDocumentInsertUri;
        private System.Windows.Forms.Button buttonDisplaySettings;
        private System.Windows.Forms.Label labelDocumentNotes;
        private System.Windows.Forms.TabPage tabPageNoAccess;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelNoAccess;
        private System.Windows.Forms.PictureBox pictureBoxNoAccess;
        private System.Windows.Forms.Label labelNoAccess;
        private System.Windows.Forms.CheckBox checkBoxPrintingOrderByMaterial;
        private System.Windows.Forms.TabPage tabPagePayment;
        private System.Windows.Forms.SplitContainer splitContainerPayment;
        private System.Windows.Forms.ListBox listBoxPayment;
        private System.Windows.Forms.ToolStrip toolStripPayment;
        private System.Windows.Forms.ToolStripButton toolStripButtonPaymentAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonPaymentDelete;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPayment;
        private System.Windows.Forms.Label labelPaymentIdentifier;
        private System.Windows.Forms.TextBox textBoxPaymentIdentifier;
        private System.Windows.Forms.Label labelPaymentAmount;
        private System.Windows.Forms.TextBox textBoxPaymentAmount;
        private System.Windows.Forms.Label labelPaymentCurrency;
        private System.Windows.Forms.Label labelPaymentForeignAmount;
        private System.Windows.Forms.TextBox textBoxPaymentForeignAmount;
        private System.Windows.Forms.TextBox textBoxPaymentForeignAmountCurrency;
        private System.Windows.Forms.Label labelPaymentPayer;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryPaymentPayer;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryPaymentRecipient;
        private System.Windows.Forms.Label labelPaymentRecipient;
        private System.Windows.Forms.Label labelPaymentDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerPaymentDate;
        private System.Windows.Forms.Label labelPaymentDateSupplement;
        private System.Windows.Forms.TextBox textBoxPaymentDateSupplement;
        private System.Windows.Forms.Label labelPaymentNotes;
        private System.Windows.Forms.TextBox textBoxPaymentNotes;
        private System.Windows.Forms.Label labelPaymentURI;
        private System.Windows.Forms.TextBox textBoxPaymentURI;
        private System.Windows.Forms.Button buttonPaymentURI;
        private System.Windows.Forms.Button buttonPaymentDate;
        private System.Windows.Forms.TabPage tabPageAgents;
        private System.Windows.Forms.SplitContainer splitContainerAgents;
        private System.Windows.Forms.ListBox listBoxAgents;
        private System.Windows.Forms.ToolStrip toolStripAgents;
        private System.Windows.Forms.ToolStripButton toolStripButtonAgentsAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonAgentsRemove;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAgents;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryAgent;
        private System.Windows.Forms.Label labelAgentRole;
        private System.Windows.Forms.Label labelAgentNotes;
        private System.Windows.Forms.TextBox textBoxAgentNotes;
        private System.Windows.Forms.ComboBox comboBoxAgentRole;
        private System.Windows.Forms.TabPage tabPageIdentifier;
        private System.Windows.Forms.SplitContainer splitContainerIdentifier;
        private System.Windows.Forms.ListBox listBoxIdentifier;
        private System.Windows.Forms.ToolStrip toolStripIdentifier;
        private System.Windows.Forms.ToolStripButton toolStripButtonIdentifierAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonIdentifierRemove;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelIdentifier;
        private System.Windows.Forms.Label labelIdentifierType;
        private System.Windows.Forms.Label labelIdentifier;
        private System.Windows.Forms.TextBox textBoxIdentifierType;
        private System.Windows.Forms.TextBox textBoxIdentifier;
        private System.Windows.Forms.BindingSource transactionAgentBindingSource;
        private Datasets.DataSetTransactionTableAdapters.TransactionAgentTableAdapter transactionAgentTableAdapter;
        private System.Windows.Forms.BindingSource transactionPaymentBindingSource;
        private Datasets.DataSetTransactionTableAdapters.TransactionPaymentTableAdapter transactionPaymentTableAdapter;
        private System.Windows.Forms.Button buttonSavePayment;
        private System.Windows.Forms.ComboBox comboBoxMaterialSource;
        private System.Windows.Forms.Label labelMaterialSource;
        private System.Windows.Forms.Label labelDocumentType;
        private System.Windows.Forms.ComboBox comboBoxDocumentType;
        private System.Windows.Forms.BindingSource externalIdentifierBindingSource;
        private System.Windows.Forms.Button buttonIdenitifierURL;
        private System.Windows.Forms.Label labelIdenitifierURL;
        private System.Windows.Forms.TextBox textBoxIdenitifierURL;
        private System.Windows.Forms.Label labelIdenitifierNotes;
        private System.Windows.Forms.TextBox textBoxIdenitifierNotes;
        private System.Windows.Forms.Label labelDateSupplement;
        private System.Windows.Forms.TextBox textBoxDateSupplement;
        private System.Windows.Forms.Label labelToTransactionInventoryNumber;
        private System.Windows.Forms.TextBox textBoxToTransactionInventoryNumber;
        private System.Windows.Forms.Label labelSendingRestrictToTaxonomicGroup;
        private System.Windows.Forms.ComboBox comboBoxSendingRestrictToTaxonomicGroup;
        private System.Windows.Forms.Button buttonTableEditor;
        private System.Windows.Forms.Button buttonShowHierarchy;
        private System.Windows.Forms.Button buttonShowSpecimenLists;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryBalanceAdministration;
        private System.Windows.Forms.Label labelBalanceAdministration;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAdministratingAgent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelHeader;
        private System.Windows.Forms.Button buttonTransactionDocumentFindUri;
        private System.Windows.Forms.SplitContainer splitContainerWebbrowserSending;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonSendingGitHub;
        private System.Windows.Forms.ToolStripMenuItem SendingSnsbToolStripMenuItem;
        private System.Windows.Forms.Button buttonMaintenance;
        private System.Windows.Forms.Button buttonTransactionDocumentInsertFile;
        private System.Windows.Forms.OpenFileDialog openFileDialogDocument;
        private System.Windows.Forms.SplitContainer splitContainerDocumentBrowser;
        private System.Windows.Forms.CheckBox checkBoxTransactionDocumentPdf;
        private System.Windows.Forms.Button buttonTransactionDocumentOpenPdf;
        private System.Windows.Forms.TabPage tabPageChart;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelChart;
        private System.Windows.Forms.Label labelChartHeader;
        private System.Windows.Forms.CheckedListBox checkedListBoxChartCollections;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartChart;
        private System.Windows.Forms.Button buttonChartGenerate;
        private System.Windows.Forms.CheckedListBox checkedListBoxChartTransactionType;
        private System.Windows.Forms.GroupBox groupBoxChartGrouping;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelChartGrouping;
        private System.Windows.Forms.RadioButton radioButtonChartGroupingNone;
        private System.Windows.Forms.RadioButton radioButtonChartGroupingCollection;
        private System.Windows.Forms.RadioButton radioButtonChartGroupingTransactionType;
        private System.Windows.Forms.CheckBox checkBoxChartStart;
        private System.Windows.Forms.NumericUpDown numericUpDownChartStart;
        private System.Windows.Forms.GroupBox groupBoxChartStart;
        private System.Windows.Forms.CheckBox checkBoxChartAddNumberOfUnits;
        private System.Windows.Forms.TabControl tabControlChartSettingsForChart;
        private System.Windows.Forms.TabPage tabPageChartAdminColl;
        private System.Windows.Forms.TabControl tabControlChart;
        private System.Windows.Forms.TabPage tabPageChartResult;
        private System.Windows.Forms.TabPage tabPageChartData;
        private System.Windows.Forms.DataGridView dataGridViewChartData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelChartData;
        private System.Windows.Forms.TabControl tabControlChartData;
        private System.Windows.Forms.TabPage tabPageChartDataAddress;
        private System.Windows.Forms.Label labelChartData;
        private System.Windows.Forms.Button buttonChartDataExport;
        private System.Windows.Forms.Button buttonChartDataGet;
        private System.Windows.Forms.RadioButton radioButtonChartDataChart;
        private System.Windows.Forms.RadioButton radioButtonChartDataSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelChartDataAddress;
        private System.Windows.Forms.CheckBox checkBoxChartAddressCity;
        private System.Windows.Forms.CheckBox checkBoxChartAddressCountry;
        private System.Windows.Forms.ListBox listBoxChartAddressSelf;
        private System.Windows.Forms.ToolStrip toolStripChartAddressSelf;
        private System.Windows.Forms.ToolStripButton toolStripButtonChartAddressSelfAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonChartAddressSelfRemove;
        private System.Windows.Forms.TabControl tabControlChartSettings;
        private System.Windows.Forms.TabPage tabPageChartTypes;
        private System.Windows.Forms.Label labelChartAddressSelf;
        private System.Windows.Forms.Label labelChartAddressOutput;
        private System.Windows.Forms.CheckBox checkBoxChartAddressPartner;
        private System.Windows.Forms.ToolStripButton toolStripButtonShowTasks;
        private System.Windows.Forms.CheckBox checkBoxSendingAllUnits;
    }
}