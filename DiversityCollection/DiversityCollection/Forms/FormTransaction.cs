using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormTransaction : Form, FormHierarchicalEntity
    {
        #region Parameter

        private DiversityCollection.Transaction _Transaction;
        private DiversityCollection.Datasets.DataSetTransaction _DataSetTransactionForReturn;
        private System.Windows.Forms.BindingSource _BindingSourceTransaction;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterCollectionManager;
        private System.Data.DataTable _DtBalance;
        private System.Data.DataTable _DtReportingCategory;
        private bool _ForMyRequests = false;

        private System.Data.DataTable _DtAdmin;
        private System.Data.DataTable _DtAdminRegulation;
        //private enum TransactionWorkStep {Sending, Confirmation, Reminder, PartialReturn, Return, Printing, Balance};

        public enum TransactionType { borrow, embargo, exchange, forwarding, gift, inventory, loan, permanentloan, permit, regulation, purchase, removal, request, returned, group, warning, chart };

        private string _XmlSending;

        #region Scanning
        private enum ScanStep { Idle, Scanning };
        private ScanStep _CurrentScanStepSending = ScanStep.Idle;
        private ScanStep _CurrentScanStepReturn = ScanStep.Idle;
        private ScanStep _CurrentScanStepPrinting = ScanStep.Idle;
        private ScanStep _CurrentScanStepRequest = ScanStep.Idle;
        #endregion

        #endregion

        #region Construction

        public FormTransaction()
        {
            try
            {
                InitializeComponent();
                this.splitContainerData.Panel2Collapsed = true;
                this.splitContainerMain.Panel2.Visible = false;
                this.initForm();
                this.userControlDialogPanel.Visible = false;
                // online manual
                this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
                // Address links
                this.userControlModuleRelatedEntryFromPartner.LocalValuesPossible = false;
                this.userControlModuleRelatedEntryToPartner.LocalValuesPossible = false;
            }
            catch(Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public FormTransaction(string SQL, string Description)
            : this()
        {
            try
            {
                string Table = "[Transaction]";
                this.Text += " - " + Description;
                if (SQL.Length > 0)
                {
                    this.userControlQueryList.IsPredefinedQuery = true;
                    this.userControlQueryList.setQueryConditions(Description, Table, SQL, Description);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public FormTransaction(string SQL, string Description, bool ForMyRequests)
            : this()
        {
            try
            {
                this._ForMyRequests = ForMyRequests;
                string Table = "[Transaction]";
                this.Text += " - " + Description;
                if (SQL.Length > 0)
                {
                    this.userControlQueryList.IsPredefinedQuery = true;
                    this.userControlQueryList.setQueryConditions(Description, Table, SQL, Description);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public FormTransaction(int? ItemID)
            : this()
        {
            if (ItemID != null)
            {
                this._Transaction.setItem((int)ItemID);
                this.initForm((int)ItemID);
                this.splitContainerMain.Panel1Collapsed = true;
                this.userControlDialogPanel.Visible = false;
            }
            else
                this.userControlDialogPanel.Visible = true;
            this.tableLayoutPanelHeader.Visible = true;
        }

        public FormTransaction(bool AsDialog, bool ReadOnly = false)
            : this()
        {
            try
            {
                this.userControlDialogPanel.Visible = AsDialog;
                this.tableLayoutPanelHeader.Visible = true;
                if (ReadOnly)
                {
                    this._Transaction.setReadOnly();
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        public FormTransaction(TransactionType Type)
        {
            InitializeComponent();
            try
            {
                switch (Type)
                {
                    case TransactionType.chart:
                        this.splitContainerMain.Panel1Collapsed = true;
                        this.tableLayoutPanelHeader.Visible = false;
                        this.tableLayoutPanelAdministratingAgent.Visible = false;
                        this.splitContainerTransaction.Panel1Collapsed = true;
                        this.tabControlTransaction.TabPages.Clear();
                        if (!this.tabControlTransaction.TabPages.Contains(this.tabPageChart))
                            this.tabControlTransaction.TabPages.Add(this.tabPageChart);
                        this.initChart();
                        break;
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Form

        private void initForm()
        {
            try
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxTransactionType, "CollTransactionType_Enum", con, false);
                DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxMaterialCategory, "CollMaterialCategory_Enum", con, true, true, false,"");
                System.Data.DataSet Dataset = this.dataSetTransaction;
                this._BindingSourceTransaction = new BindingSource(this.dataSetTransaction, "Transaction");
                if (this._Transaction == null)
                {
                    this._Transaction = new Transaction(ref Dataset, this.dataSetTransaction.Transaction,
                        ref this.treeViewTransaction, this, this.userControlQueryList, this.splitContainerMain,
                        this.splitContainerData, this.toolStripButtonSpecimenList, //this.imageListSpecimenList,
                        this.userControlSpecimenList, this.helpProvider, this.toolTip, ref this.transactionBindingSource,
                        this.tabControlTransaction);
                }
                this._Transaction.ShowHierarchy = DiversityCollection.Forms.FormTransactionSettings.Default.ShowHierarchy;
                this.setShowHierarchy();
                this._Transaction.ShowSpecimenLists = DiversityCollection.Forms.FormTransactionSettings.Default.ShowSpecimenLists;
                this.setShowSpecimenLists();
                this.splitContainerTransaction.Panel1Collapsed = !this._Transaction.ShowHierarchy;
                this._Transaction.initForm();
                this._Transaction.setToolStripButtonDeleteEvent(this.toolStripButtonDelete);
                this._Transaction.setToolStripButtonNewEvent(this.toolStripButtonNew);
                // ensure reset of transaction lookup table
                this.setToolStripButtonSaveTransaction(this.userControlQueryList.toolStripButtonSave);
                //this._Transaction.setToolStripButtonCopyEvent(this.toolStripButtonCopy);
                this._Transaction.setToolStripButtonCopyHierarchyEvent(this.toolStripButtonCopy);
                this._Transaction.setToolStripButtonSetParentWithHierarchyEvent(this.toolStripButtonSetParent);
                this._Transaction.setToolbarPermission(ref this.toolStripButtonDelete, "Transaction", "Delete");
                this._Transaction.setToolbarPermission(ref this.toolStripButtonNew, "Transaction", "Insert");

                this._Transaction.DependentTablesBindingSources.Add("TransactionDocument", this.transactionDocumentBindingSource);
                this._Transaction.DependentTablesBindingSources.Add("TransactionAgent", this.transactionAgentBindingSource);
                this._Transaction.DependentTablesBindingSources.Add("TransactionPayment", this.transactionPaymentBindingSource);
                this._Transaction.DependentTablesBindingSources.Add("ExternalIdentifier", this.externalIdentifierBindingSource);

                //this.initRemoteModules();
                this.initQueryOptimizingResetEvents();
                this.userControlSpecimenList.toolStripButtonDelete.Visible = false;
                this.setDocumentControls();
                this.readSettings();
                this.fillLookupTables();
                // setting hierarchy on demand for better performance, Markus 1.2.2019
                //this.setUserControlHierarchySelectors();
                this.initAutoCompletion();
                if (this.comboBoxPrintingConversion.Items.Count == 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<DiversityCollection.Transaction.ConversionType, string> KV in DiversityCollection.Transaction.ConversionDictionary)
                        this.comboBoxPrintingConversion.Items.Add(KV.Key.ToString().Replace("_", " "));
                }
                //this.startScannerSending();
                this.FillMonthIntervalls();
                this.tableLayoutPanelPrinting.Height = (int)(70 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);

                this._Transaction.FormFunctions.setDescriptions();
                //this.FormFunctions.setDescriptions();
                this.setTexts();

                this.userControlQueryList.RememberSettingIsAvailable(true);
                this.userControlQueryList.RememberQuerySettingsIdentifier = "Transaction";
                this.userControlQueryList.RememberQueryConditionSettings_ReadFromFile();

                //this.FormFunctions.setDescriptions();
                //this.UseSendingScanner = false;

                this.checkBoxTransactionDocumentPdf.Checked = DiversityCollection.Forms.FormTransactionSettings.Default.PdfInBrowserControl;

                this.initRemoteModules();

                this.initAdministration();

                this.initChart();
            }
            catch (System.Exception ex) 
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setTexts()
        {
            this.labelNoAccess.Text = DiversityCollection.Forms.FormCollectionSpecimenText.You_have_no_access_to_this_dataset;
            this.tabPageBalance.Text = DiversityCollection.Forms.FormTransactionText.Balance;
            this.tabPageConfirmation.Text = DiversityCollection.Forms.FormTransactionText.Confirmation;
            this.tabPageDocuments.Text = DiversityCollection.Forms.FormTransactionText.Documents;
            this.tabPageForwarding.Text = DiversityCollection.Forms.FormTransactionText.Forwarding;
            this.tabPageNoAccess.Text = DiversityCollection.Forms.FormTransactionText.NoAccess;
            this.tabPagePayment.Text = DiversityCollection.Forms.FormTransactionText.Payments;
            this.tabPagePrinting.Text = DiversityCollection.Forms.FormTransactionText.Printing;
            this.tabPageReminder.Text = DiversityCollection.Forms.FormTransactionText.Reminder;
            this.tabPageRequest.Text = DiversityCollection.Forms.FormTransactionText.Request;
            this.tabPageSending.Text = DiversityCollection.Forms.FormTransactionText.Sending;
            this.tabPageTransactionReturn.Text = DiversityCollection.Forms.FormTransactionText.Return;
        }

        private void initForm(int TransactionID)
        {
            string TransactionType = DiversityCollection.LookupTable.TransactionType(TransactionID);
            this.initForm(TransactionType);
        }

        private void initForm(string TransactionType)
        {
            if (this.treeViewTransaction.Visible)
                this._Transaction.buildHierarchy();
            this.setDetailControls(TransactionType);
            return;
        }

        public void setFormControls()
        {
            try
            {
                if (this.transactionBindingSource.Current != null)
                {
                    int iID = (int)this._Transaction.ID;
                    System.Data.DataRow[] rr = this._Transaction.DataSet.Tables["Transaction"].Select("TransactionID = " + iID.ToString());
                    if (rr.Length > 0)
                    {
                        string TT = rr[0]["TransactionType"].ToString();
                        this.initForm(TT);
                        return;
                    }


                    //int iT = this._Transaction.TransactionID;
                    System.Data.DataRowView RV = (System.Data.DataRowView)this.transactionBindingSource.Current;
                    string TransactionType = RV["TransactionType"].ToString();
                    string Type = "";
                    if (this.comboBoxTransactionType.SelectedItem != null)
                    {
                        System.Data.DataRowView RC = (System.Data.DataRowView)this.comboBoxTransactionType.SelectedItem;
                        Type = RC[0].ToString();// this.comboBoxTransactionType.Text.ToUpper();
                    }
                    else
                        Type = TransactionType;
                    if (TransactionType.ToLower() == Type.ToLower() || (TransactionType != "" && Type == ""))
                        this.initForm(Type);
                }
                else
                    this.initForm("");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void fillLookupTables()
        {
            // Markus 4.12.2023: Umbau Regulation
            string SQL = "SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, " +
                "AdministrativeContactName, AdministrativeContactAgentURI, Description,  " +
                "Location, CollectionOwner, DisplayText " +
                "FROM dbo.CollectionHierarchyAll() " + //WHERE Type IS NULL OR Type <> 'regulation' " +
                "ORDER BY DisplayText";
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(this.dataSetTransaction.Collection);
                this.comboBoxAdministratingCollection.Visible = true;
            }
            catch (System.Exception ex)
            {
                this.comboBoxAdministratingCollection.Visible = false;
            }
            // Markus 4.12.2023: Umbau Regulation
            SQL = "SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, " +
                "AdministrativeContactName, AdministrativeContactAgentURI, Description,  " +
                "Location, CollectionOwner, DisplayText " +
                "FROM dbo.CollectionHierarchyAll() " + // WHERE Type = 'regulation'" +
                "ORDER BY DisplayText";
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(this.dataSetTransaction.RegulationCollection);
                this.comboBoxAdministratingCollection.Visible = true;
            }
            catch (System.Exception ex)
            {
                this.comboBoxAdministratingCollection.Visible = false;
            }
            System.Data.DataTable dtCollFrom = this.dataSetTransaction.Collection.Copy();
            this.comboBoxFromCollection.DataSource = dtCollFrom;
            this.comboBoxFromCollection.DisplayMember = "DisplayText";
            this.comboBoxFromCollection.ValueMember = "CollectionID";
            System.Data.DataTable dtCollTo = this.dataSetTransaction.Collection.Copy();
            this.comboBoxToCollection.DataSource = dtCollTo;
            this.comboBoxToCollection.DisplayMember = "DisplayText";
            this.comboBoxToCollection.ValueMember = "CollectionID";
            DiversityWorkbench.Forms.FormFunctions FF = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
            bool OK = FF.getObjectPermissions("ManagerCollectionList", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Select);
            if (OK)
            {
                // Markus 4.12.2023: Umbau Regulation
                SQL = "SELECT C.CollectionID, C.CollectionParentID, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, " +
                    "C.Location, C.CollectionOwner, C.DisplayOrder, C.DisplayText " +
                    "FROM CollectionManager INNER JOIN dbo.CollectionHierarchyAll() AS C ON CollectionManager.AdministratingCollectionID = C.CollectionID " +
                    "WHERE (CollectionManager.LoginName = USER_NAME()) /*AND (C.Type IS NULL OR C.Type <> 'regulation'  )*/ ORDER BY DisplayText ";
                Microsoft.Data.SqlClient.SqlDataAdapter adAdmin = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                this._DtAdmin = new DataTable();
                adAdmin.Fill(this._DtAdmin);
                if (this._DtAdmin.Rows.Count > 0)
                {
                    this.comboBoxAdministratingCollection.DataSource = this._DtAdmin;
                    this.comboBoxAdministratingCollection.DisplayMember = "DisplayText";
                    this.comboBoxAdministratingCollection.ValueMember = "CollectionID";
                    this.comboBoxAdministratingCollection.Visible = true;
                }
                else
                    this.comboBoxAdministratingCollection.Visible = false;
                // Markus 4.12.2023: Umbau Regulation
                SQL = "SELECT C.CollectionID, C.CollectionParentID, C.CollectionName, C.CollectionAcronym, C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, " +
                    "C.Location, C.CollectionOwner, C.DisplayOrder, C.DisplayText " +
                    "FROM CollectionManager INNER JOIN dbo.CollectionHierarchyAll() AS C ON CollectionManager.AdministratingCollectionID = C.CollectionID " +
                    "WHERE (CollectionManager.LoginName = USER_NAME()) /*AND (C.Type = 'regulation')*/ ORDER BY DisplayText ";
                adAdmin.SelectCommand.CommandText = SQL;
                this._DtAdminRegulation = new DataTable();
                adAdmin.Fill(this._DtAdminRegulation);
            }
            else
            {
                this.comboBoxAdministratingCollection.Visible = false;
            }

            OK = FF.getObjectPermissions("RequesterCollectionList", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Select);
            if (OK)
            {
                SQL = "SELECT C.CollectionID, C.CollectionParentID, C.CollectionName, C.CollectionAcronym, " +
                    "C.AdministrativeContactName, C.AdministrativeContactAgentURI, C.Description, C.Location, " +
                    "C.CollectionOwner, C.DisplayOrder, C.DisplayText " +
                    "FROM dbo.CollectionManager INNER JOIN " +
                    "dbo.CollectionHierarchyAll() AS C ON dbo.CollectionManager.AdministratingCollectionID = C.CollectionID " +
                    "WHERE (dbo.CollectionManager.LoginName = USER_NAME()) ORDER BY DisplayText ";
                Microsoft.Data.SqlClient.SqlDataAdapter adAdminRequester = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                System.Data.DataTable dtAdminRequester = new DataTable();
                adAdminRequester.Fill(dtAdminRequester);

                this.comboBoxRequestTo.DataSource = dtAdminRequester;
                this.comboBoxRequestTo.DisplayMember = "DisplayText";
                this.comboBoxRequestTo.ValueMember = "CollectionID";

                SQL = "SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, " +
                    "AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, " +
                    "CollectionOwner, DisplayOrder " +
                    "FROM dbo.RequesterCollectionList()";
                System.Data.DataTable dtCollectionsRequester = new DataTable();
                adAdminRequester.SelectCommand.CommandText = SQL;
                adAdminRequester.Fill(dtCollectionsRequester);

                this.comboBoxRequestFrom.DataSource = dtAdminRequester;
                this.comboBoxRequestFrom.DisplayMember = "CollectionName";
                this.comboBoxRequestFrom.ValueMember = "CollectionID";
            }
        }

        private void setReadOnly()
        {
        }

        #region Adding months
        
        private System.Collections.Generic.Dictionary<string, int> _MonthIntervalls;

        public System.Collections.Generic.Dictionary<string, int> MonthIntervalls
        {
            get
            {
                if (this._MonthIntervalls == null)
                {
                    this._MonthIntervalls = new Dictionary<string, int>();
                    this._MonthIntervalls.Add("3 months", 3);
                    this._MonthIntervalls.Add("6 months", 6);
                    this._MonthIntervalls.Add("9 months", 9);
                    this._MonthIntervalls.Add("1 year", 12);
                    this._MonthIntervalls.Add("2 years", 24);
                    this._MonthIntervalls.Add("3 years", 36);
                }
                return _MonthIntervalls;
            }
        }

        private void FillMonthIntervalls()
        {
            if (this.comboBoxAgreedEndDateAddMonths.Items.Count == 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, int> KV in this.MonthIntervalls)
                    this.comboBoxAgreedEndDateAddMonths.Items.Add(KV.Key);
            }
            if (this.comboBoxActualEndDateAddMonths.Items.Count == 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, int> KV in this.MonthIntervalls)
                    this.comboBoxActualEndDateAddMonths.Items.Add(KV.Key);
            }
        }
        
        #endregion

        /// <summary>
        /// read the settings for the schema files
        /// </summary>
        private void readSettings()
        {
            if (DiversityCollection.Forms.FormTransactionSettings.Default.SendingSchemaFile.Length > 0)
                this.textBoxSendingSchema.Text = DiversityCollection.Forms.FormTransactionSettings.Default.SendingSchemaFile;
            else this.textBoxSendingSchema.Text = this.SchemaFile(Transaction.TransactionCorrespondenceType.Sending);

            if (DiversityCollection.Forms.FormTransactionSettings.Default.ConfirmationSchemaFile.Length > 0)
                this.textBoxConfirmationSchema.Text = DiversityCollection.Forms.FormTransactionSettings.Default.ConfirmationSchemaFile;
            else this.textBoxConfirmationSchema.Text = this.SchemaFile(Transaction.TransactionCorrespondenceType.Confirmation);

            if (DiversityCollection.Forms.FormTransactionSettings.Default.ReminderSchemaFile.Length > 0)
                this.textBoxReminderSchema.Text = DiversityCollection.Forms.FormTransactionSettings.Default.ReminderSchemaFile;
            else this.textBoxReminderSchema.Text = this.SchemaFile(Transaction.TransactionCorrespondenceType.Reminder);

            if (DiversityCollection.Forms.FormTransactionSettings.Default.ReturnSchemaFile.Length > 0)
                this.textBoxTransactionReturnSchema.Text = DiversityCollection.Forms.FormTransactionSettings.Default.ReturnSchemaFile;
            else this.textBoxTransactionReturnSchema.Text = this.SchemaFile(Transaction.TransactionCorrespondenceType.Return);

            if (DiversityCollection.Forms.FormTransactionSettings.Default.ForwardingSchemaFile.Length > 0)
                this.textBoxForwardingSchema.Text = DiversityCollection.Forms.FormTransactionSettings.Default.ForwardingSchemaFile;
            else this.textBoxForwardingSchema.Text = this.SchemaFile(Transaction.TransactionCorrespondenceType.Forwarding);

            if (DiversityCollection.Forms.FormTransactionSettings.Default.BalanceSchemaFile.Length > 0)
                this.textBoxBalanceSchema.Text = DiversityCollection.Forms.FormTransactionSettings.Default.BalanceSchemaFile;
            else this.textBoxBalanceSchema.Text = this.SchemaFile(Transaction.TransactionCorrespondenceType.Balance);

            if (DiversityCollection.Forms.FormTransactionSettings.Default.PrintingSchemaFile.Length > 0)
                this.textBoxPrintingSchemaFile.Text = DiversityCollection.Forms.FormTransactionSettings.Default.PrintingSchemaFile;
            else this.textBoxPrintingSchemaFile.Text = this.SchemaFile(Transaction.TransactionCorrespondenceType.Printing);

            if (DiversityCollection.Forms.FormTransactionSettings.Default.RequestSchemaFile.Length > 0)
                this.textBoxRequestSchema.Text = DiversityCollection.Forms.FormTransactionSettings.Default.RequestSchemaFile;
            else this.textBoxRequestSchema.Text = this.SchemaFile(Transaction.TransactionCorrespondenceType.Request);

            this.timerReturn.Interval = DiversityCollection.Forms.FormTransactionSettings.Default.TimerIntervall;
            this.timerSending.Interval = DiversityCollection.Forms.FormTransactionSettings.Default.TimerIntervall;
        }

        private void initAutoCompletion()
        {
            try
            {
                string SQL = "SELECT distinct rtrim(ReportingCategory) AS ReportingCategory FROM dbo.[Transaction] WHERE rtrim(ReportingCategory) <> '' Order by rtrim(ReportingCategory)";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                System.Data.DataTable dt = new DataTable();
                ad.Fill(dt);
                this.comboBoxReportingCategory.DataSource = dt;
                this.comboBoxReportingCategory.DisplayMember = "ReportingCategory";
                this.comboBoxReportingCategory.ValueMember = "ReportingCategory";
                DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxReportingCategory, AutoCompleteMode.SuggestAppend);

                DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxInvestigator);

                DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxReportingCategory);
                DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxMaterialCategory);
                DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxTransactionType);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private string SchemaFile(Transaction.TransactionCorrespondenceType CorrespondenceType)
        {
            string File = "";
            System.IO.DirectoryInfo Dir = new System.IO.DirectoryInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\Transaction\\Schemas\\" + CorrespondenceType.ToString());
            if (Dir.Exists)
            {
                foreach (System.IO.FileInfo F in Dir.GetFiles("*.xslt"))
                {
                    if (F.Name.StartsWith(CorrespondenceType.ToString()))
                    {
                        File = Dir.FullName + "\\" + F.Name;
                        break;
                    }
                }
            }
            return File;
        }

        /// <summary>
        /// save schema files into the settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormTransaction_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.textBoxSendingSchema.Text.Length > 0)
                DiversityCollection.Forms.FormTransactionSettings.Default.SendingSchemaFile = this.textBoxSendingSchema.Text;
            if (this.textBoxConfirmationSchema.Text.Length > 0)
                DiversityCollection.Forms.FormTransactionSettings.Default.ConfirmationSchemaFile = this.textBoxConfirmationSchema.Text;
            if (this.textBoxReminderSchema.Text.Length > 0)
                DiversityCollection.Forms.FormTransactionSettings.Default.ReminderSchemaFile = this.textBoxReminderSchema.Text;
            //DiversityCollection.Forms.FormTransactionSettings.Default.PartialReturnSchemaFile = this.textBoxPartialReturnSchema.Text;
            if (this.textBoxTransactionReturnSchema.Text.Length > 0)
                DiversityCollection.Forms.FormTransactionSettings.Default.ReturnSchemaFile = this.textBoxTransactionReturnSchema.Text;
            if (this.textBoxBalanceSchema.Text.Length > 0)
                DiversityCollection.Forms.FormTransactionSettings.Default.BalanceSchemaFile = this.textBoxBalanceSchema.Text;
            if (this.timerReturn.Interval != DiversityCollection.Forms.FormTransactionSettings.Default.TimerIntervall)
                DiversityCollection.Forms.FormTransactionSettings.Default.TimerIntervall = this.timerReturn.Interval;
            if (this.textBoxPrintingSchemaFile.Text.Length > 0)
                DiversityCollection.Forms.FormTransactionSettings.Default.PrintingSchemaFile = this.textBoxPrintingSchemaFile.Text;
            if (this.textBoxRequestSchema.Text.Length > 0)
                DiversityCollection.Forms.FormTransactionSettings.Default.RequestSchemaFile = this.textBoxRequestSchema.Text;
            DiversityCollection.Forms.FormTransactionSettings.Default.Save();
            try
            {
                if (this._Transaction != null)
                this._Transaction.saveItem();
            }
            catch (System.Exception ex) 
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            if (this.userControlQueryList.RememberQuerySettings())
                this.userControlQueryList.RememberQueryConditionSettings_SaveToFile();
            else
                this.userControlQueryList.RememberQueryConditionSettings_RemoveFile();

            if (this._dtAdministration != null)
            {
                if (this._dtAdministration.Rows.Count > 0 && this._dtAdministration.Rows[0]["URI"].ToString().Length > 0)
                    DiversityCollection.Forms.FormTransactionSettings.Default.BalanceAdministrationAgentURI = this._dtAdministration.Rows[0]["URI"].ToString();
                else
                    DiversityCollection.Forms.FormTransactionSettings.Default.BalanceAdministrationAgentURI = "";
            }
        }

        private void toolStripButtonCopy_Click(object sender, EventArgs e)
        {
            this._Transaction.toolStripButtonCopy_Click(sender, e);
        }

        private void userControlHierarchySelectorFromCollection_Click(object sender, EventArgs e)
        {
            if (!userControlHierarchySelectorFromCollection.IsInitialized())
            {
                try
                {
                    if (DiversityCollection.LookupTable.DtCollection == null) return;
                    System.Data.DataTable DtCollectionFrom = DiversityCollection.LookupTable.DtCollection.Copy();
                    this.userControlHierarchySelectorFromCollection.initHierarchy(
                        DtCollectionFrom,
                        "CollectionID",
                        "CollectionParentID",
                        "CollectionName",
                        "CollectionName",
                        "CollectionID",
                        this.comboBoxFromCollection);
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void userControlHierarchySelectorToCollection_Click(object sender, EventArgs e)
        {
            if (!userControlHierarchySelectorFromCollection.IsInitialized())
            {
                try
                {
                    if (DiversityCollection.LookupTable.DtCollection == null) return;
                    System.Data.DataTable DtCollectionFrom = DiversityCollection.LookupTable.DtCollection.Copy();
                    System.Data.DataTable DtCollectionTo = DiversityCollection.LookupTable.DtCollection.Copy();
                    this.userControlHierarchySelectorToCollection.initHierarchy(
                        DtCollectionTo,
                        "CollectionID",
                        "CollectionParentID",
                        "CollectionName",
                        "CollectionName",
                        "CollectionID",
                        this.comboBoxToCollection);
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        //private void setUserControlHierarchySelectors()
        //{
        //    try
        //    {
        //        if (DiversityCollection.LookupTable.DtCollection == null) return;
        //        System.Data.DataTable DtCollectionFrom = DiversityCollection.LookupTable.DtCollection.Copy();
        //        this.userControlHierarchySelectorFromCollection.initHierarchy(
        //            DtCollectionFrom,
        //            "CollectionID",
        //            "CollectionParentID",
        //            "CollectionName",
        //            "CollectionName",
        //            "CollectionID",
        //            this.comboBoxFromCollection);


        //        System.Data.DataTable DtCollectionTo = DiversityCollection.LookupTable.DtCollection.Copy();
        //        this.userControlHierarchySelectorToCollection.initHierarchy(
        //            DtCollectionTo,
        //            "CollectionID",
        //            "CollectionParentID",
        //            "CollectionName",
        //            "CollectionName",
        //            "CollectionID",
        //            this.comboBoxToCollection);

        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        private void buttonHeaderFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                this.userControlQueryList.QueryString(),
                this.ID.ToString());
        }

        private void FormTransaction_Load_1(object sender, EventArgs e)
        {
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetTransaction.TransactionPayment". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.transactionPaymentTableAdapter.Fill(this.dataSetTransaction.TransactionPayment);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetTransaction.TransactionAgent". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.transactionAgentTableAdapter.Fill(this.dataSetTransaction.TransactionAgent);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetTransaction.Annotation". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.annotationTableAdapter.Fill(this.dataSetTransaction.Annotation);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetTransaction.CollectionSpecimenPartTransactionReturnList". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collectionSpecimenPartTransactionReturnListTableAdapter.Fill(this.dataSetTransaction.CollectionSpecimenPartTransactionReturnList);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetTransaction.CollectionSpecimenPartForwardingList". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collectionSpecimenPartForwardingListTableAdapter.Fill(this.dataSetTransaction.CollectionSpecimenPartForwardingList);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetTransaction.CollectionSpecimenPartSentList". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collectionSpecimenPartSentListTableAdapter.Fill(this.dataSetTransaction.CollectionSpecimenPartSentList);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetTransaction.CollectionSpecimenPartPartialReturnList". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collectionSpecimenPartPartialReturnListTableAdapter.Fill(this.dataSetTransaction.CollectionSpecimenPartPartialReturnList);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetTransaction.TransactionDocument". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.transactionDocumentTableAdapter.Fill(this.dataSetTransaction.TransactionDocument);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetTransaction.CollectionSpecimenPartList". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collectionSpecimenPartListTableAdapter.Fill(this.dataSetTransaction.CollectionSpecimenPartList);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetTransaction.CollectionSpecimenPartReturnList". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collectionSpecimenPartReturnListTableAdapter.Fill(this.dataSetTransaction.CollectionSpecimenPartReturnList);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetTransaction.CollectionSpecimenPartOnLoanList". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collectionSpecimenPartOnLoanListTableAdapter.Fill(this.dataSetTransaction.CollectionSpecimenPartOnLoanList);

        }

        private void setToolStripButtonSaveTransaction(System.Windows.Forms.ToolStripButton B)
        {
            try
            {
                B.Click += new System.EventHandler(this.toolStripButtonSaveTransaction_Click);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void toolStripButtonSaveTransaction_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityCollection.LookupTable.ResetTransaction();
                this._Transaction.setLookUpTableHierarchy(DiversityCollection.LookupTable.DtTransactionHierarchy);
                if (this._Transaction.ID != null)
                {
                    this._Transaction.fillDependentTables((int)this._Transaction.ID);
                    this._Transaction.buildHierarchy();
                    this.setDetailControls();
                }
                //this._Transaction.fillDependentTables(this._Transaction.TransactionID);
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonDisplaySettings_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormCustomizeDisplay f = new Forms.FormCustomizeDisplay(DiversityCollection.Specimen.ImageList, Forms.FormCustomizeDisplay.Customization.Transaction);
            f.ShowDialog();
        }

        private void buttonShowHierarchy_Click(object sender, EventArgs e)
        {
            this._Transaction.ShowHierarchy = !this._Transaction.ShowHierarchy;
            DiversityCollection.Forms.FormTransactionSettings.Default.ShowHierarchy = this._Transaction.ShowHierarchy;
            DiversityCollection.Forms.FormTransactionSettings.Default.Save();
            this.setShowHierarchy();
        }

        private void setShowHierarchy()
        {
            if (this._Transaction.ShowHierarchy)
            {
                this.buttonShowHierarchy.Image = DiversityCollection.Resource.Hierarchy;
                this.buttonShowHierarchy.BackColor = System.Drawing.SystemColors.Control;
                this.toolTip.SetToolTip(this.buttonShowHierarchy, "Hide the hierarchy");
            }
            else
            {
                this.buttonShowHierarchy.Image = DiversityCollection.Resource.HierarchyGrey;
                this.buttonShowHierarchy.BackColor = System.Drawing.Color.Yellow;
                this.toolTip.SetToolTip(this.buttonShowHierarchy, "Show the hierarchy");
            }
            this.splitContainerTransaction.Panel1Collapsed = !this._Transaction.ShowHierarchy;
        }

        private void buttonShowSpecimenLists_Click(object sender, EventArgs e)
        {
            this._Transaction.ShowSpecimenLists = !this._Transaction.ShowSpecimenLists;
            DiversityCollection.Forms.FormTransactionSettings.Default.ShowSpecimenLists = this._Transaction.ShowSpecimenLists;
            DiversityCollection.Forms.FormTransactionSettings.Default.Save();
            this.setShowSpecimenLists();
        }

        private void setShowSpecimenLists()
        {
            if (this._Transaction.ShowSpecimenLists)
            {
                this.buttonShowSpecimenLists.Image = DiversityCollection.Resource.List;
                this.buttonShowSpecimenLists.BackColor = System.Drawing.SystemColors.Control;
                this.toolTip.SetToolTip(this.buttonShowSpecimenLists, "Hide the specimen lists");
            }
            else
            {
                this.buttonShowSpecimenLists.Image = DiversityCollection.Resource.ListGrey;
                this.buttonShowSpecimenLists.BackColor = System.Drawing.Color.Yellow;
                this.toolTip.SetToolTip(this.buttonShowSpecimenLists, "Show the specimen lists");
            }

            this.tabPagePrinting.Enabled = this._Transaction.ShowSpecimenLists;
            this.tabPageReminder.Enabled = this._Transaction.ShowSpecimenLists;
            this.tabPageForwarding.Enabled = this._Transaction.ShowSpecimenLists;
            this.tabPageConfirmation.Enabled = this._Transaction.ShowSpecimenLists;
            this.tabPageRequest.Enabled = this._Transaction.ShowSpecimenLists;
            this.tabPageSending.Enabled = this._Transaction.ShowSpecimenLists;
            this.tabPageTransactionReturn.Enabled = this._Transaction.ShowSpecimenLists;
        }

        #region Query optimizing

        private void initQueryOptimizing()
        {
            DiversityWorkbench.UserControls.UserControlQueryList.QueryMainTable = "Transaction";
        }

        private void initQueryOptimizingResetEvents()
        {
            this.userControlModuleRelatedEntryAgent.buttonOpenModule.Click += new System.EventHandler(this.ResetOptimizing_Click);
            this.userControlModuleRelatedEntryFromPartner.buttonOpenModule.Click += new System.EventHandler(this.ResetOptimizing_Click);
            this.userControlModuleRelatedEntryPaymentPayer.buttonOpenModule.Click += new System.EventHandler(this.ResetOptimizing_Click);
            this.userControlModuleRelatedEntryPaymentRecipient.buttonOpenModule.Click += new System.EventHandler(this.ResetOptimizing_Click);
            this.userControlModuleRelatedEntryRequestFrom.buttonOpenModule.Click += new System.EventHandler(this.ResetOptimizing_Click);
            this.userControlModuleRelatedEntryRequestTo.buttonOpenModule.Click += new System.EventHandler(this.ResetOptimizing_Click);
            this.userControlModuleRelatedEntryResponsible.buttonOpenModule.Click += new System.EventHandler(this.ResetOptimizing_Click);
            this.userControlModuleRelatedEntryToPartner.buttonOpenModule.Click += new System.EventHandler(this.ResetOptimizing_Click);
        }

        private void ResetOptimizing_Click(object sender, EventArgs e)
        {
            this.initQueryOptimizing();
        }

        #endregion
        
        private void buttonMaintenance_Click(object sender, EventArgs e)
        {
            try
            {
                string SQL = "SELECT MIN(C.ProjectID) " +
                    " FROM CollectionProject C " +
                    " INNER JOIN CollectionSpecimenPart AS P ON C.CollectionSpecimenID = P.CollectionSpecimenID " +
                    " INNER JOIN IdentificationUnit AS U ON P.CollectionSpecimenID = U.CollectionSpecimenID " +
                    " INNER JOIN CollectionSpecimenTransaction AS T ON T.CollectionSpecimenID = P.CollectionSpecimenID AND T.SpecimenPartID = P.SpecimenPartID AND T.TransactionID = " + this._Transaction.ID.ToString() +
                    " LEFT OUTER JOIN IdentificationUnitInPart AS UP ON P.CollectionSpecimenID = UP.CollectionSpecimenID AND P.SpecimenPartID = UP.SpecimenPartID " +
                    " WHERE UP.IdentificationUnitID IS NULL ";
                string ProjectID = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                int iProjectID;
                if (int.TryParse(ProjectID, out iProjectID))
                {
                    DiversityCollection.Forms.FormMaintenance f = new FormMaintenance(FormMaintenance.Maintenance.StorageMissingUnit, iProjectID);
                    f.ShowDialog();
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Modules

        private void initRemoteModules()
        {
            try
            {
                // Agents
                DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
#if DEBUG
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryFromPartner, A, "Transaction", "FromTransactionPartnerName", "FromTransactionPartnerAgentURI", this.transactionBindingSource);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryToPartner, A, "Transaction", "ToTransactionPartnerName", "ToTransactionPartnerAgentURI", this.transactionBindingSource);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryResponsible, A, "Transaction", "ResponsibleName", "ResponsibleAgentURI", this.transactionBindingSource);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryRequestFrom, A, "Transaction", "FromTransactionPartnerName", "FromTransactionPartnerAgentURI", this.transactionBindingSource);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryRequestTo, A, "Transaction", "ToTransactionPartnerName", "ToTransactionPartnerAgentURI", this.transactionBindingSource);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryPaymentPayer, A, "TransactionPayment", "PayerName", "PayerAgentURI", this.transactionPaymentBindingSource);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryPaymentRecipient, A, "TransactionPayment", "RecipientName", "RecipientAgentURI", this.transactionPaymentBindingSource);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryAgent, A, "TransactionAgent", "AgentName", "AgentURI", this.transactionAgentBindingSource);
#else


                this.userControlModuleRelatedEntryFromPartner.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryToPartner.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryResponsible.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryRequestFrom.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryRequestTo.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryPaymentPayer.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryPaymentRecipient.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryAgent.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;


                this.userControlModuleRelatedEntryFromPartner.bindToData("[Transaction]", "FromTransactionPartnerName", "FromTransactionPartnerAgentURI", this.transactionBindingSource);
                this.userControlModuleRelatedEntryToPartner.bindToData("[Transaction]", "ToTransactionPartnerName", "ToTransactionPartnerAgentURI", this.transactionBindingSource);
                this.userControlModuleRelatedEntryResponsible.bindToData("[Transaction]", "ResponsibleName", "ResponsibleAgentURI", this.transactionBindingSource);
                this.userControlModuleRelatedEntryRequestFrom.bindToData("[Transaction]", "FromTransactionPartnerName", "FromTransactionPartnerAgentURI", this.transactionBindingSource);
                this.userControlModuleRelatedEntryRequestTo.bindToData("[Transaction]", "ToTransactionPartnerName", "ToTransactionPartnerAgentURI", this.transactionBindingSource);
                this.userControlModuleRelatedEntryPaymentPayer.bindToData("[TransactionPayment]", "PayerName", "PayerAgentURI", this.transactionPaymentBindingSource);
                this.userControlModuleRelatedEntryPaymentRecipient.bindToData("[TransactionPayment]", "RecipientName", "RecipientAgentURI", this.transactionPaymentBindingSource);
                this.userControlModuleRelatedEntryAgent.bindToData("[TransactionAgent]", "AgentName", "AgentURI", this.transactionAgentBindingSource);
#endif

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void inituserControlModuleRelatedEntry(ref DiversityWorkbench.UserControls.UserControlModuleRelatedEntry UC, DiversityWorkbench.IWorkbenchUnit iWorkbenchUnit, string TableName, string DisplayColumn, string ValueColumn, System.Windows.Forms.BindingSource BindingSource)
        {
            try
            {
                UC.FixingOfSourceEnabled = true;
                UC.buttonFixSource.Click += new System.EventHandler(this.FixSource_Click);

                //UC.IWorkbenchUnit = iWorkbenchUnit;
                UC.bindToData(TableName, DisplayColumn, ValueColumn, this.transactionBindingSource);
                UC.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace;
                UC.IWorkbenchUnit = iWorkbenchUnit;
                if (iWorkbenchUnit.getServerConnection().ModuleName.ToLower() == "diversityscientificterms")
                    UC.IsListInDatabase = true;

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        //private void setUserControlSourceFixing(ref DiversityWorkbench.UserControls.UserControlModuleRelatedEntry UC, string ValueColumn)
        //{
        //    System.Collections.Generic.List<string> Settings = new List<string>();
        //    Settings.Add("ModuleSource");
        //    Settings.Add(TableName);
        //    Settings.Add(ValueColumn);
        //    this.setUserControlModuleRelatedEntrySources(Settings, ref UC);
        //    //UC.FixingOfSourceEnabled = true;
        //}

        protected void FixSource_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Button B = (System.Windows.Forms.Button)sender;
                System.Windows.Forms.Panel P = (System.Windows.Forms.Panel)B.Parent;
                DiversityWorkbench.UserControls.UserControlModuleRelatedEntry UCMRE = (DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)P.Parent;
                DiversityWorkbench.UserSettings U = new DiversityWorkbench.UserSettings();
                System.Collections.Generic.List<string> Setting = new List<string>();
                Setting.Add(DiversityWorkbench.UserSettings.SettingGroups.ModuleSource.ToString());
                Setting.Add(UCMRE.TableName);
                switch (UCMRE.TableName)
                {
                    case "Transaction":
                        switch (UCMRE.ValueColumn)
                        {
                            default:
                                Setting.Add(UCMRE.ValueColumn);
                                break;
                        }
                        break;
                    default:
                        Setting.Add(UCMRE.ValueColumn);
                        break;
                }
                if (!DiversityWorkbench.WorkbenchUnit.SaveSetting(Setting, UCMRE.SourceServerConnection(), UCMRE.SourceWebservice))
                    System.Windows.Forms.MessageBox.Show("Saving of setting failed");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        #endregion

        #region Properties

        public int ID
        {
            get
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
                return int.Parse(R["TransactionID"].ToString());// this.dataSetTransaction.Transaction.Rows[this.transactionBindingSource.Position][0].ToString());
                //return int.Parse(this.dataSetTransaction.Transaction.Rows[this.transactionBindingSource.Position][0].ToString());
            }
        }

        public int? ParentID
        {
            get
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
                if (R["ParentTransactionID"].Equals(System.DBNull.Value))
                    return null;
                else
                    return int.Parse(R["ParentTransactionID"].ToString());
            }
        }

        public string TransactionTitle
        {
            get
            {
                return this.dataSetTransaction.Transaction.Rows[this.transactionBindingSource.Position]["TransactionTitle"].ToString();
            }
        }

        public string TypeOfTransaction
        {
            get
            {
                return this.dataSetTransaction.Transaction.Rows[this.transactionBindingSource.Position]["TransactionType"].ToString();
            }
        }

        public string DisplayText
        {
            get
            {
                return this.dataSetTransaction.Transaction.Rows[this.transactionBindingSource.Position][2].ToString();
            }
        }

        public bool ChangeToSpecimen { get { return this.userControlSpecimenList.ChangeToSpecimen; } }

        public int CollectionSpecimenID { get { return this.userControlSpecimenList.CollectionSpecimenID; } }

        public System.Windows.Forms.BindingSource BindingSourceTransaction
        {
            get 
            {
                if (this._BindingSourceTransaction == null) this._BindingSourceTransaction = new BindingSource(this.dataSetTransaction, "Transaction");
                return _BindingSourceTransaction; 
            }
            //set { _BindingSourceTransaction = value; }
        }

        public System.Data.DataTable DtReportingCategory
        {
            get 
            {
                if (this._DtReportingCategory == null)
                {
                    this._DtReportingCategory = new DataTable();
                }
                return _DtReportingCategory; 
            }
            //set { _DtReportingCategory = value; }
        }

        public int AdministrativeCollectionID
        {
            get 
            {
                int CollID = 0;
                System.Data.DataRowView RV = (System.Data.DataRowView)transactionBindingSource.Current;
                int.TryParse(RV["AdministratingCollectionID"].ToString(), out CollID);
                return CollID;
                //bool OK = true;
                //System.Data.DataRowView R = (System.Data.DataRowView)transactionBindingSource.Current;
                //if (R["AdministratingCollectionID"].Equals(System.DBNull.Value))
                //{
                //    System.Windows.Forms.MessageBox.Show("Please select an administrating collection");
                //    this.comboBoxPrintingAccessionNumber.Text = "";
                //    OK = false;
                //}
                //else
                //{
                //    if (!int.TryParse(R["AdministratingCollectionID"].ToString(), out CollID))
                //        OK = false;
                //}
                //if (OK)
                //    return CollID;
                //else
                //{
                //    this.tabControlTransaction.SelectedTab = this.tabPageDataEntry;
                //    this.comboBoxAdministratingCollection.Focus();
                //    return null;
                //}
            }
        }

#endregion

#region Showing, hiding and setting controls

        public void setDetailControls()
        {
            System.Data.DataRow[] rr = this._Transaction.DataSet.Tables["Transaction"].Select("TransactionID = " + this._Transaction.ID.ToString());
            if (rr.Length > 0)
            {
                string Type = rr[0]["TransactionType"].ToString();
                this.setDetailControls(Type);
            }
            string SQL = "SELECT count(*) " +
                " FROM " +
                " CollectionSpecimenPart AS P " +
                " INNER JOIN IdentificationUnit AS U ON P.CollectionSpecimenID = U.CollectionSpecimenID " +
                " INNER JOIN CollectionSpecimenTransaction AS T ON T.CollectionSpecimenID = P.CollectionSpecimenID AND T.SpecimenPartID = P.SpecimenPartID AND T.TransactionID = " + this._Transaction.ID.ToString() + 
                " LEFT OUTER JOIN IdentificationUnitInPart AS UP ON P.CollectionSpecimenID = UP.CollectionSpecimenID AND P.SpecimenPartID = UP.SpecimenPartID " +
                " WHERE UP.IdentificationUnitID IS NULL ";
            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (Result != "0")
            {
                this.toolTip.SetToolTip(this.buttonMaintenance, "For " + Result + " datasets the units within the parts are missing");
                this.buttonMaintenance.Visible = true;
            }
            else
                this.buttonMaintenance.Visible = false;
        }

        private void setDetailControls(string Type)
        {
            switch(Type.ToLower())
            {
                case "borrow":
                    this.setDetailControls(TransactionType.borrow);
                    break;
                case "embargo":
                    this.setDetailControls(TransactionType.embargo);
                    break;
                case "exchange":
                    this.setDetailControls(TransactionType.exchange);
                    break;
                case "forwarding":
                    this.setDetailControls(TransactionType.forwarding);
                    break;
                case "permanent loan":
                case "gift":
                    this.setDetailControls(TransactionType.gift);
                    break;
                case "inventory":
                    this.setDetailControls(TransactionType.inventory);
                    break;
                case "loan":
                    this.setDetailControls(TransactionType.loan);
                    break;
                case "permit":
                    this.setDetailControls(TransactionType.permit);
                    break;
                case "regulation":
                    this.setDetailControls(TransactionType.regulation);
                    break;
                case "purchase":
                    this.setDetailControls(TransactionType.purchase);
                    break;
                case "removal":
                    this.setDetailControls(TransactionType.removal);
                    break;
                case "request":
                    this.setDetailControls(TransactionType.request);
                    break;
                case "return":
                    this.setDetailControls(TransactionType.returned);
                    break;
                case "transaction group":
                    this.setDetailControls(TransactionType.group);
                    break;
                case "warning":
                    this.setDetailControls(TransactionType.warning);
                    break;
                default:
                    this.SuspendLayout();
                    this.tabControlTransaction.TabPages.Clear();
                    if (!this.tabControlTransaction.TabPages.Contains(this.tabPageNoAccess)) // #113
                        this.tabControlTransaction.TabPages.Add(this.tabPageNoAccess);
                    this.ResumeLayout();
                    break;
            }
        }

        private void setDetailControls(TransactionType Type)
        {
            this.SuspendLayout();
            try
            {
                this.setTabPages(Type);

                // Administration
                if (Type == TransactionType.regulation)
                {
                    this.comboBoxAdministratingCollection.DataSource = this._DtAdminRegulation;

                }
                else
                {
                    this.comboBoxAdministratingCollection.DataSource = this._DtAdmin;

                }

                // Material controls
                bool ShowMaterial = true;
                if (Type == TransactionType.embargo || Type == TransactionType.returned || Type == TransactionType.group || Type == TransactionType.warning || Type == TransactionType.forwarding)
                    ShowMaterial = false;
                this.labelMaterialCategory.Visible = ShowMaterial;
                this.comboBoxMaterialCategory.Visible = ShowMaterial;
                this.labelMaterialDescription.Visible = ShowMaterial;
                this.textBoxMaterialDescription.Visible = ShowMaterial;
                this.labelMaterialCollectors.Visible = ShowMaterial;
                this.textBoxMaterialCollectors.Visible = ShowMaterial;
                this.labelMaterialSource.Visible = ShowMaterial;
                this.comboBoxMaterialSource.Visible = ShowMaterial;

                // From controls
                bool ShowFromControls = true;
                if (Type == TransactionType.embargo || Type == TransactionType.returned || Type == TransactionType.group || Type == TransactionType.inventory || 
                    Type == TransactionType.forwarding || Type == TransactionType.removal || Type == TransactionType.warning)
                    ShowFromControls = false;

                // set controls on default state
                this.groupBoxFrom.Visible = ShowFromControls;
                this.labelFromCollection.Text = "Collection:";
                this.userControlHierarchySelectorFromCollection.Visible = true;
                this.comboBoxFromCollection.Visible = true;
                this.groupBoxFrom.Text = DiversityCollection.Forms.FormTransactionText.From;// "From";
                this.labelFromTransactionNumber.Text = "Number:";
                this.labelFromTransactionNumber.Visible = true;
                this.textBoxFromTransactionNumber.Visible = true;
                this.userControlModuleRelatedEntryFromPartner.Visible = true;
                this.labelFromCollection.Visible = true;

                // get state of available data
                bool FromCollectionPresent = false;
                bool FromNumberPresent = false;
                bool FromAddressPresent = false;
                bool ToCollectionPresent = false;
                bool ToNumberPresent = false;
                bool ToAddressPresent = false;
                bool ToRecipientPresent = false;
                if (this.transactionBindingSource.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
                    if (!R["FromCollectionID"].Equals(System.DBNull.Value) && R["FromCollectionID"].ToString().Length > 0)
                        FromCollectionPresent = true;
                    if (!R["FromTransactionNumber"].Equals(System.DBNull.Value) && R["FromTransactionNumber"].ToString().Length > 0)
                        FromNumberPresent = true;
                    if (!R["FromTransactionPartnerName"].Equals(System.DBNull.Value) && R["FromTransactionPartnerName"].ToString().Length > 0)
                        FromAddressPresent = true;
                    if (!R["ToCollectionID"].Equals(System.DBNull.Value) && R["ToCollectionID"].ToString().Length > 0)
                        ToCollectionPresent = true;
                    if (!R["ToTransactionNumber"].Equals(System.DBNull.Value) && R["ToTransactionNumber"].ToString().Length > 0)
                        ToNumberPresent = true;
                    if (!R["ToTransactionPartnerName"].Equals(System.DBNull.Value) && R["ToTransactionPartnerName"].ToString().Length > 0)
                        ToAddressPresent = true;
                    if (!R["ToRecipient"].Equals(System.DBNull.Value) && R["ToRecipient"].ToString().Length > 0)
                        ToRecipientPresent = true;
                }

                // Collection is not used any more, keep only for compatibility to older versions
                if (!DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionShowAllAddressFields
                    && !FromCollectionPresent)
                {
                    this.comboBoxFromCollection.Visible = false;
                    this.userControlHierarchySelectorFromCollection.Visible = false;
                    this.labelFromCollection.Visible = false;
                    this.groupBoxFrom.Height = (int)(DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor * 64);
                }

                switch (Type)
                {
                    case TransactionType.loan:
                        if (FromCollectionPresent)
                        {
                            this.comboBoxFromCollection.Visible = true;
                            this.userControlHierarchySelectorFromCollection.Visible = true;
                            this.labelFromCollection.Visible = true;
                            this.groupBoxFrom.Height = (int)(DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor * 88);
                        }
                        break;
                    case TransactionType.permanentloan:
                    case TransactionType.gift:
                        if (!DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionShowAllAddressFields)
                        {
                            if (!FromNumberPresent)
                            {
                                this.labelFromTransactionNumber.Visible = false;
                                this.textBoxFromTransactionNumber.Visible = false;
                            }
                            this.groupBoxFrom.Height = (int)(DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor * 40);
                            if (FromNumberPresent && FromCollectionPresent)
                                this.groupBoxFrom.Height = 88;
                            else if (FromNumberPresent || FromCollectionPresent)
                                this.groupBoxFrom.Height = (int)(DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor * 64);
                        }
                        break;
                    case TransactionType.permit:
                        this.groupBoxFrom.Text = DiversityCollection.Forms.FormTransactionText.Permitting_institution;// "Permitting institution";
                        this.labelFromCollection.Text = "Address:";
                        this.labelFromTransactionNumber.Text = "No of permission";
                        break;
                    //case TransactionType.regulation:
                    //    this.groupBoxFrom.Visible = false;
                    //    this.groupBoxTo.Visible = false;

                    //    break;
                    case TransactionType.exchange:
                        if (!DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionShowAllAddressFields)
                        {
                            if (!FromNumberPresent)
                            {
                                this.labelFromTransactionNumber.Visible = false;
                                this.textBoxFromTransactionNumber.Visible = false;
                            }
                            this.groupBoxFrom.Height = (int)(DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor * 40);
                            if (FromNumberPresent && FromAddressPresent)
                                this.groupBoxFrom.Height = (int)(DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor * 88);
                            else if (FromNumberPresent && FromAddressPresent)
                                this.groupBoxFrom.Height = (int)(DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor * 64);
                        }
                        break;
                    default:
                        break;
                }

                // To controls
                bool ShowToControls = true;
                if (Type == TransactionType.embargo || Type == TransactionType.group || Type == TransactionType.inventory || Type == TransactionType.removal || Type == TransactionType.warning)// || Type == TransactionType.returned)
                    ShowToControls = false;

                // Show field for inventory number (is as copy contained in TO controls)
                bool ShowInventoryNumber = !ShowToControls;
                if (ShowInventoryNumber && (Type == TransactionType.gift || Type == TransactionType.exchange || Type == TransactionType.permanentloan || Type == TransactionType.warning))
                    ShowInventoryNumber = false;

                this.labelToTransactionInventoryNumber.Visible = ShowInventoryNumber;
                this.textBoxToTransactionInventoryNumber.Visible = ShowInventoryNumber;


                // set control on default state
                this.groupBoxTo.Visible = ShowToControls;
                this.comboBoxToCollection.Visible = true;
                this.userControlHierarchySelectorToCollection.Visible = true;
                this.labelToCollection.Visible = true;
                this.labelToTransactionNumber.Visible = true;
                this.textBoxToTransactionNumber.Visible = true;
                this.groupBoxTo.Text = DiversityCollection.Forms.FormTransactionText.To;// "To";
                this.labelToTransactionNumber.Visible = true;
                this.textBoxToTransactionNumber.Visible = true;
                this.labelToTransactionNumber.Visible = true;
                this.textBoxToTransactionNumber.Visible = true;
                this.userControlModuleRelatedEntryToPartner.Visible = true;
                this.labelToCollection.Visible = true;
                this.labelToRecipient.Visible = true;
                this.comboBoxToRecipient.Visible = true;

                this.groupBoxTo.Height = (int)(DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor * 88);

                // Collection is not used any more, keep only for compatibility to older versions
                if (!DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionShowAllAddressFields
                    && !ToCollectionPresent)
                {
                    this.comboBoxToCollection.Visible = false;
                    this.userControlHierarchySelectorToCollection.Visible = false;
                    this.labelToCollection.Visible = false;
                    this.groupBoxTo.Height = (int)(DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor * 64);
                }

                switch (Type)
                {
                    case TransactionType.returned:
                        if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionShowAllAddressFields ||
                            ToRecipientPresent)
                        {
                            this.comboBoxToCollection.Visible = false;
                            this.userControlHierarchySelectorToCollection.Visible = false;
                            this.labelToCollection.Visible = false;

                            this.labelToTransactionNumber.Visible = false;
                            this.textBoxToTransactionNumber.Visible = false;

                            this.userControlModuleRelatedEntryToPartner.Visible = false;

                            this.groupBoxTo.Height = (int)(DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor * 40);
                        }
                        else
                            this.groupBoxTo.Visible = false;
                        break;
                    case TransactionType.loan:
                        if (ToCollectionPresent)
                        {
                            this.comboBoxToCollection.Visible = true;
                            this.userControlHierarchySelectorToCollection.Visible = true;
                            this.labelToCollection.Visible = true;
                            this.groupBoxTo.Height = (int)(DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor * 88);
                        }
                        if (!ToNumberPresent && !DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionShowAllAddressFields)
                        {
                            this.labelToTransactionNumber.Visible = false;
                            this.textBoxToTransactionNumber.Visible = false;
                        }
                        break;
                    case TransactionType.permit:
                        this.groupBoxTo.Text = DiversityCollection.Forms.FormTransactionText.Permission_granted_to;// "Permission granted to";
                        if (!DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionShowAllAddressFields)
                        {
                            this.labelToRecipient.Visible = false;
                            this.comboBoxToRecipient.Visible = false;
                            if (!ToNumberPresent)
                            {
                                this.labelToTransactionNumber.Visible = false;
                                this.textBoxToTransactionNumber.Visible = false;
                            }
                            this.groupBoxTo.Height = (int)(DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor * 40);
                            if (ToNumberPresent && ToCollectionPresent)
                                this.groupBoxTo.Height = (int)(DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor * 88);
                            else if (ToNumberPresent || ToCollectionPresent)
                                this.groupBoxTo.Height = (int)(DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor * 64);
                        }
                        break;
                    case TransactionType.regulation:
                        this.labelToRecipient.Visible = false;
                        this.comboBoxToRecipient.Visible = false;

                        this.labelToTransactionNumber.Visible = false;
                        this.textBoxToTransactionNumber.Visible = false;

                        this.groupBoxTo.Visible = false;
                        this.groupBoxFrom.Visible = false;

                        this.labelToRecipient.Visible = false;
                        this.comboBoxToRecipient.Visible = false;

                        // Markus 4.12.2023: Umbau Regulation
                        //this.labelActualEndDate.Visible = false;
                        //this.dateTimePickerActualEndDate.Visible = false;
                        //this.buttonActualEndDateEdit.Visible = false;

                        //this.labelAgreedEndDate.Visible = false;
                        //this.dateTimePickerAgreedEndDate.Visible = false;
                        //this.buttonAgreedEndDateEdit.Visible = false;

                        //this.labelBeginDate.Visible = false;
                        //this.dateTimePickerBeginDate.Visible = false;
                        //this.buttonBeginDateEdit.Visible = false;

                        this.labelDateSupplement.Visible = false;
                        this.textBoxDateSupplement.Visible = false;

                        this.labelMaterialCategory.Visible = false;
                        this.labelMaterialCollectors.Visible = false;
                        this.labelMaterialDescription.Visible = false;
                        this.labelMaterialSource.Visible = false;
                        this.textBoxMaterialCollectors.Visible = false;
                        this.textBoxMaterialDescription.Visible = false;
                        this.comboBoxMaterialCategory.Visible = false;
                        this.comboBoxMaterialSource.Visible = false;

                        break;
                    case TransactionType.permanentloan:
                    case TransactionType.gift:
                        if (!DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionShowAllAddressFields)
                        {
                            this.groupBoxTo.Height = (int)(DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor * 40);
                            this.labelToRecipient.Visible = false;
                            this.comboBoxToRecipient.Visible = false;
                            if (!ToNumberPresent)
                            {
                                this.labelToTransactionNumber.Visible = false;
                                this.textBoxToTransactionNumber.Visible = false;
                            }
                            if (ToNumberPresent && ToCollectionPresent)
                                this.groupBoxTo.Height = (int)(DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor * 88);
                            else if (ToNumberPresent || ToCollectionPresent)
                                this.groupBoxTo.Height = (int)(DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor * 64);
                        }
                        break;
                    case TransactionType.exchange:
                        System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
                        if (R["FromTransactionPartnerAgentURI"].Equals(System.DBNull.Value) ||
                            R["FromTransactionPartnerAgentURI"].ToString().Length == 0 ||
                            R["ToTransactionPartnerAgentURI"].Equals(System.DBNull.Value) ||
                            R["ToTransactionPartnerAgentURI"].ToString().Length == 0)
                        {
                            if (R["TransactionType"].ToString() == "exchange")
                                this.tableLayoutPanelBalance.Enabled = false;
                        }
                        else
                            this.tableLayoutPanelBalance.Enabled = true;
                        if (!DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionShowAllAddressFields)
                        {
                            if (!ToNumberPresent)
                            {
                                this.labelToTransactionNumber.Visible = false;
                                this.textBoxToTransactionNumber.Visible = false;
                            }
                            this.groupBoxTo.Height = (int)(DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor * 40);
                            if (ToNumberPresent && ToCollectionPresent)
                                this.groupBoxTo.Height = (int)(DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor * 88);
                            else if (ToNumberPresent || ToCollectionPresent)
                                this.groupBoxTo.Height = (int)(DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor * 64);
                        }
                        break;
                    default:
                        break;
                }

                // Unit contols
                bool ShowUnitControls = true;
                if (Type == TransactionType.embargo || Type == TransactionType.returned || Type == TransactionType.group || Type == TransactionType.regulation || Type == TransactionType.permit || Type == TransactionType.removal || Type == TransactionType.warning)
                    ShowUnitControls = false;
                this.labelNumberOfUnits.Visible = ShowUnitControls;
                this.textBoxNumberOfUnits.Visible = ShowUnitControls;

                // Category controls
                bool ShowCategoryControls = false;
                if (Type == TransactionType.gift || Type == TransactionType.permanentloan || Type == TransactionType.purchase || Type == TransactionType.exchange || Type == TransactionType.inventory || Type == TransactionType.loan)
                    ShowCategoryControls = true;
                this.labelReportingCategory.Visible = ShowCategoryControls;
                this.comboBoxReportingCategory.Visible = ShowCategoryControls;

                // Investigator
                bool ShowInvestigatorControls = true;
                if (Type == TransactionType.embargo || Type == TransactionType.returned || Type == TransactionType.group || Type == TransactionType.inventory || Type == TransactionType.regulation ||
                    Type == TransactionType.gift || Type == TransactionType.permanentloan || Type == TransactionType.purchase || Type == TransactionType.exchange || Type == TransactionType.removal || Type == TransactionType.warning)
                    ShowInvestigatorControls = false;
                this.labelInvestigator.Visible = ShowInvestigatorControls;
                this.textBoxInvestigator.Visible = ShowInvestigatorControls;

                // Date controls
                // Begin
                bool ShowBeginControls = true;
                // Markus 4.12.2023: Umbau Regulation
                if (Type == TransactionType.group) // || Type == TransactionType.regulation)
                    ShowBeginControls = false;
                this.setDateContolsVisibility(DateControls.Begin, ShowBeginControls);

                // AgreedEnd
                // Markus 4.12.2023: Umbau Regulation
                bool ShowAgreedEndControls = true;
                if (Type == TransactionType.returned || Type == TransactionType.group || Type == TransactionType.gift || Type == TransactionType.permanentloan 
                    || /*Type == TransactionType.regulation ||*/ Type == TransactionType.purchase || Type == TransactionType.exchange || Type == TransactionType.removal)
                    ShowAgreedEndControls = false;
                this.setDateContolsVisibility(DateControls.AgreedEnd, ShowAgreedEndControls);

                // ActualEnd
                bool ShowActualEndControls = false;
                if (Type == TransactionType.loan || Type == TransactionType.forwarding)
                    ShowActualEndControls = true;
                this.setDateContolsVisibility(DateControls.ActualEnd, ShowActualEndControls);

                if (ShowBeginControls && !ShowAgreedEndControls && !ShowActualEndControls)
                    this.labelBeginDate.Text = "Date:";
                else
                    this.labelBeginDate.Text = "Begin:";

                // DateSupplement
                bool ShowDateSupplement = true;
                if (Type == TransactionType.group || Type == TransactionType.regulation || Type == TransactionType.warning)
                    ShowDateSupplement = false;
                this.labelDateSupplement.Visible = ShowDateSupplement;
                this.textBoxDateSupplement.Visible = ShowDateSupplement;

                // Comment
                bool ShowCommentControls = true;
                if (Type == TransactionType.embargo || Type == TransactionType.group || Type == TransactionType.inventory || Type == TransactionType.regulation || Type == TransactionType.permit || Type == TransactionType.removal || Type == TransactionType.warning)
                    ShowCommentControls = false;
                this.labelTransactionComment.Visible = ShowCommentControls;
                this.textBoxTransactionComment.Visible = ShowCommentControls;
                this.buttonTransactionCommentInsertSelected.Visible = ShowCommentControls;
                this.comboBoxTransactionCommentAdd.Visible = ShowCommentControls;
                this.labelCommentPhrases.Visible = ShowCommentControls;

                // Internal notes
                bool ShowIntNotesControls = true;
                // Markus 4.12.2023: Umbau Regulation
                //if (Type == TransactionType.regulation)
                //    ShowIntNotesControls = false;
                //this.labelInternalNotes.Visible = ShowIntNotesControls;
                this.textBoxInternalNotes.Visible = ShowIntNotesControls;

                // Responsible
                // Markus 4.12.2023: Umbau Regulation
                bool ShowResponsibleControls = true;
                if (Type == TransactionType.embargo || Type == TransactionType.returned || Type == TransactionType.group)// || Type == TransactionType.regulation)
                    ShowResponsibleControls = false;
                this.labelResponsible.Visible = ShowResponsibleControls;
                this.userControlModuleRelatedEntryResponsible.Visible = ShowResponsibleControls;

                // Request tab
                if (this._ForMyRequests)
                {
                    if (Type == TransactionType.request)
                    {
                        this.toolStripSpecimenListRequest.Visible = true;
                        this.labelRequestSpecimenList.Text = "Specimen";
                        this.labelRequestSpecimenOnLoan.Visible = false;
                        this.tableLayoutPanelRequest.Enabled = true;
                        this.listBoxRequestOnLoan.Visible = false;
                    }
                    else
                    {
                        this.toolStripSpecimenListRequest.Visible = false;
                        this.labelRequestSpecimenList.Text = "Specimen returned";
                        this.labelRequestSpecimenOnLoan.Visible = true;
                        this.tableLayoutPanelRequest.Enabled = false;
                        this.listBoxRequestOnLoan.Visible = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            this.ResumeLayout();
        }

        private void setTabPages(TransactionType Type)
        {
            try
            {
                if (!this.tabControlTransaction.TabPages.Contains(this.tabPageDataEntry))
                    this.tabControlTransaction.TabPages.Add(this.tabPageDataEntry);

                this.tabControlTransaction.TabPages.Remove(this.tabPageBalance);
                this.tabControlTransaction.TabPages.Remove(this.tabPageConfirmation);
                this.tabControlTransaction.TabPages.Remove(this.tabPageDocuments);
                //this.tabControlTransaction.TabPages.Remove(this.tabPagePartialReturn);
                this.tabControlTransaction.TabPages.Remove(this.tabPagePrinting);
                this.tabControlTransaction.TabPages.Remove(this.tabPageReminder);
                //this.tabControlTransaction.TabPages.Remove(this.tabPageReturn);
                this.tabControlTransaction.TabPages.Remove(this.tabPageSending);
                this.tabControlTransaction.TabPages.Remove(this.tabPageRequest);
                this.tabControlTransaction.TabPages.Remove(this.tabPageForwarding);
                this.tabControlTransaction.TabPages.Remove(this.tabPageTransactionReturn);
                this.tabControlTransaction.TabPages.Remove(this.tabPageNoAccess);
                this.tabControlTransaction.TabPages.Remove(this.tabPagePayment);
                this.tabControlTransaction.TabPages.Remove(this.tabPageAgents);
                this.tabControlTransaction.TabPages.Remove(this.tabPageIdentifier);
                // #113
                this.tabControlTransaction.TabPages.Remove(this.tabPageChart);

#if !DEBUG
                //this.tabControlTransaction.TabPages.Remove(this.tabPageChart);

#endif

                int CurrentID = -1;
                if (this._Transaction.ID != null)
                    CurrentID = (int)this._Transaction.ID;
                if (this.treeViewTransaction.SelectedNode != null &&
                    this.treeViewTransaction.SelectedNode.Tag != null)
                {
                    System.Data.DataRow R = (System.Data.DataRow)this.treeViewTransaction.SelectedNode.Tag;
                    if (R.RowState != DataRowState.Detached && R.RowState != DataRowState.Deleted)
                        CurrentID = int.Parse(R["TransactionID"].ToString());
                }

                if (this._Transaction.NonAccessibleIDs.Contains(CurrentID))
                {
                    this.tabControlTransaction.TabPages.Clear();
                    this.tabControlTransaction.TabPages.Add(this.tabPageNoAccess);
                    this.toolStripTransaction.Enabled = false;
                    this.buttonHistory.Enabled = false;
                    this.buttonTableEditor.Enabled = false;
                    this.buttonDisplaySettings.Enabled = false;
                }
                else
                {
                    this.toolStripTransaction.Enabled = true;
                    this.buttonHistory.Enabled = true;
                    this.buttonTableEditor.Enabled = true;
                    this.buttonDisplaySettings.Enabled = true;

                    if (this._ForMyRequests)
                    {
                        switch (Type)
                        {
                            case TransactionType.forwarding:
                                break;
                            case TransactionType.returned:// "RETURN":
                            case TransactionType.loan:// "LOAN":
                            case TransactionType.request:// "REQUEST":
                                goto default;
                            default:
                                if (this.tabControlTransaction.TabPages.Contains(this.tabPageDataEntry)) // #113
                                    this.tabControlTransaction.TabPages.Remove(this.tabPageDataEntry);
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPageRequest)) // #113
                                    this.tabControlTransaction.TabPages.Insert(1, this.tabPageRequest);
                                break;
                        }
                    }
                    else
                    {
                        switch (Type)
                        {
                            case TransactionType.loan:
                                if(!this.tabControlTransaction.TabPages.Contains(this.tabPageSending)) // #113
                                    this.tabControlTransaction.TabPages.Insert(1, this.tabPageSending);
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPageConfirmation))
                                    this.tabControlTransaction.TabPages.Insert(2, this.tabPageConfirmation);
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPageReminder))
                                    this.tabControlTransaction.TabPages.Insert(3, this.tabPageReminder);
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPageChart))
                                    this.tabControlTransaction.TabPages.Insert(4, this.tabPageChart);
                                break;
                            case TransactionType.returned:
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPageTransactionReturn)) // #113
                                    this.tabControlTransaction.TabPages.Insert(1, this.tabPageTransactionReturn);
                                break;
                            case TransactionType.forwarding:
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPageForwarding)) // #113
                                    this.tabControlTransaction.TabPages.Insert(1, this.tabPageForwarding);
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPageReminder)) // #113
                                    this.tabControlTransaction.TabPages.Insert(2, this.tabPageReminder);
                                break;
                            case TransactionType.borrow:
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPageConfirmation)) // #113
                                    this.tabControlTransaction.TabPages.Insert(1, this.tabPageConfirmation);
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPagePrinting)) // #113
                                    this.tabControlTransaction.TabPages.Insert(2, this.tabPagePrinting);
                                break;
                            case TransactionType.permanentloan:
                            case TransactionType.gift:
                            case TransactionType.purchase:
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPageConfirmation)) // #113
                                    this.tabControlTransaction.TabPages.Insert(1, this.tabPageConfirmation);
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPagePrinting)) // #113
                                    this.tabControlTransaction.TabPages.Insert(2, this.tabPagePrinting);
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPagePayment)) // #113
                                    this.tabControlTransaction.TabPages.Insert(3, this.tabPagePayment);
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPageAgents)) // #113
                                    this.tabControlTransaction.TabPages.Insert(4, this.tabPageAgents);
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPageIdentifier)) // #113
                                    this.tabControlTransaction.TabPages.Insert(5,this.tabPageIdentifier);
                                break;
                            case TransactionType.exchange:
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPageSending)) // #113
                                    this.tabControlTransaction.TabPages.Insert(1, this.tabPageSending);
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPageConfirmation)) // #113
                                    this.tabControlTransaction.TabPages.Insert(2, this.tabPageConfirmation);
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPageBalance)) // #113
                                    this.tabControlTransaction.TabPages.Insert(3, this.tabPageBalance);
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPagePrinting)) // #113
                                    this.tabControlTransaction.TabPages.Insert(4, this.tabPagePrinting);
                                break;
                            case TransactionType.permit:
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPageIdentifier)) // #113
                                    this.tabControlTransaction.TabPages.Insert(1, this.tabPageIdentifier);
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPageAgents)) // #113
                                    this.tabControlTransaction.TabPages.Insert(2, this.tabPageAgents);
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPagePrinting)) // #113
                                    this.tabControlTransaction.TabPages.Insert(3, this.tabPagePrinting);
                                break;
                            case TransactionType.regulation:
                                break;
                            case TransactionType.inventory:
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPageIdentifier)) // #113
                                    this.tabControlTransaction.TabPages.Insert(1, this.tabPageIdentifier);
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPageAgents)) // #113
                                    this.tabControlTransaction.TabPages.Insert(2, this.tabPageAgents);
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPagePrinting)) // #113
                                    this.tabControlTransaction.TabPages.Insert(3, this.tabPagePrinting);
                                break;
                            case TransactionType.request:
                            case TransactionType.embargo:
                            case TransactionType.removal:
                            case TransactionType.warning:
                                if (!this.tabControlTransaction.TabPages.Contains(this.tabPagePrinting)) // #113
                                    this.tabControlTransaction.TabPages.Insert(this.tabControlTransaction.TabPages.Count, this.tabPagePrinting);
                                break;
                            default:
                                //this.tabPageConfirmation.Hide();
                                break;
                        }

                        // Markus 4.12.2023: Umbau Regulation
                        //if (Type != TransactionType.group && Type != TransactionType.regulation)
                        if (!this.tabControlTransaction.TabPages.Contains(this.tabPageDocuments)) // #113
                            this.tabControlTransaction.TabPages.Add(this.tabPageDocuments);

                        if (Type == TransactionType.forwarding)
                        {
                            this.listBoxReminderSpecimenOnLoan.DataSource = this.dataSetTransaction.CollectionSpecimenPartForwardingOnLoanList;
                            this.listBoxReminderSpecimenReturned.DataSource = this.dataSetTransaction.CollectionSpecimenPartForwardingReturnedList;
                        }
                        else
                        {
                            this.listBoxReminderSpecimenOnLoan.DataSource = this.dataSetTransaction.CollectionSpecimenPartOnLoanList;
                            this.listBoxReminderSpecimenReturned.DataSource = this.dataSetTransaction.CollectionSpecimenPartTransactionReturnList;
                        }

                        if (Type == TransactionType.loan)
                        {
                            this.labelSendingSpecimenReturned.Visible = true;
                            this.listBoxSendingSpecimenReturned.Visible = true;
                            this.listBoxSendingSpecimen.BackColor = System.Drawing.Color.Pink;
                            if (this.dataSetTransaction.CollectionSpecimenPartForwardingList.Rows.Count > 0)
                            {
                                this.labelSendingSpecimenForwarded.Visible = true;
                                this.listBoxSendingSpecimenForwarded.Visible = true;
                            }
                            else
                            {
                                this.labelSendingSpecimenForwarded.Visible = false;
                                this.listBoxSendingSpecimenForwarded.Visible = false;
                            }
                        }
                        else
                        {
                            this.labelSendingSpecimenReturned.Visible = false;
                            this.listBoxSendingSpecimenReturned.Visible = false;
                            this.listBoxSendingSpecimen.BackColor = System.Drawing.Color.White;
                            this.labelSendingSpecimenForwarded.Visible = false;
                            this.listBoxSendingSpecimenForwarded.Visible = false;
                        }

                        // setting the background for the confirmation specimen list
                        switch (Type)
                        {
                            case TransactionType.purchase:
                            case TransactionType.permanentloan:
                            case TransactionType.gift:
                                this.listBoxConfirmationSpecimenList.BackColor = System.Drawing.Color.LightGreen;
                                break;
                            default:
                                this.listBoxConfirmationSpecimenList.BackColor = System.Drawing.Color.Pink;
                                break;
                        }

                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

#region Date controls

        private enum DateControls { Begin, AgreedEnd, ActualEnd };

        private void setDateContolsVisibility(DateControls DC, bool Show)
        {
            System.Collections.Generic.List<System.Windows.Forms.Control> Controls = new List<Control>();
            switch (DC)
            {
                case DateControls.Begin:
                    Controls.Add(this.dateTimePickerBeginDate);
                    Controls.Add(this.buttonBeginDateEdit);
                    Controls.Add(this.labelBeginDate);
                    break;
                case DateControls.AgreedEnd:
                    Controls.Add(this.dateTimePickerAgreedEndDate);
                    Controls.Add(this.buttonAgreedEndDateEdit);
                    Controls.Add(this.labelAgreedEndDate);
                    Controls.Add(this.comboBoxAgreedEndDateAddMonths);
                    break;
                case DateControls.ActualEnd:
                    Controls.Add(this.dateTimePickerActualEndDate);
                    Controls.Add(this.buttonActualEndDateEdit);
                    Controls.Add(this.labelActualEndDate);
                    Controls.Add(this.comboBoxActualEndDateAddMonths);
                    break;
            }
            foreach (System.Windows.Forms.Control C in Controls)
            {
                if (!Show)
                {
                    C.Tag = "hide";
                    C.Visible = false;
                }
                else
                {
                    C.Tag = "show";
                    // Markus 1.9.23: unklar warum das ausgeblendet wurde
                    //if (C.GetType() != typeof(System.Windows.Forms.DateTimePicker))
                        C.Visible = true;
                }
            }
        }

        private void setDateControls(System.Data.DataRow R)
        {
            try
            {
                setBeginDateControls(R);
                setAgreedEndDateControls(R);
                setActualEndDateControls(R);
            }
            catch (System.Exception ex) { }
        }

        private void setBeginDateControls(System.Data.DataRow R)
        {
            if (R["BeginDate"].Equals(System.DBNull.Value))
            {
                this.dateTimePickerBeginDate.DataBindings.Clear();
                this.dateTimePickerBeginDate.Visible = false;
                this.buttonBeginDateEdit.Image = this.imageListSetDate.Images[0];
            }
            else
            {
                if (this.dateTimePickerBeginDate.DataBindings.Count == 0)
                {
                    this.dateTimePickerBeginDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.transactionBindingSource, "BeginDate", true));
                }
                this.dateTimePickerBeginDate.Visible = true;
                this.buttonBeginDateEdit.Image = this.imageListSetDate.Images[1];
            }
        }

        private void setAgreedEndDateControls(System.Data.DataRow R)
        {
            try
            {
                if (R["AgreedEndDate"].Equals(System.DBNull.Value))
                {
                    this.dateTimePickerAgreedEndDate.DataBindings.Clear();
                    this.dateTimePickerAgreedEndDate.Visible = false;
                    this.buttonAgreedEndDateEdit.Image = this.imageListSetDate.Images[0];
                }
                else
                {
                    if (this.dateTimePickerAgreedEndDate.DataBindings.Count == 0)
                    {
                        this.dateTimePickerAgreedEndDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.transactionBindingSource, "AgreedEndDate", true));
                    }
                    else
                    {
                    }
                    this.dateTimePickerAgreedEndDate.Visible = true;
                    this.buttonAgreedEndDateEdit.Image = this.imageListSetDate.Images[1];
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void setActualEndDateControls(System.Data.DataRow R)
        {
            try
            {
                if (this.dateTimePickerActualEndDate.Tag != null && this.dateTimePickerActualEndDate.Tag.ToString() == "show")
                {
                    if (R["ActualEndDate"].Equals(System.DBNull.Value))
                    {
                        this.dateTimePickerActualEndDate.DataBindings.Clear();
                        this.dateTimePickerActualEndDate.Visible = false;
                        this.buttonActualEndDateEdit.Image = this.imageListSetDate.Images[0];

                    }
                    else
                    {
                        if (this.dateTimePickerActualEndDate.DataBindings.Count == 0)
                        {
                            this.dateTimePickerActualEndDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.transactionBindingSource, "ActualEndDate", true));
                        }
                        else
                        {

                        }
                        this.dateTimePickerActualEndDate.Visible = true;
                        this.buttonActualEndDateEdit.Image = this.imageListSetDate.Images[1];
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

#endregion

#region alte loesungen - obsolet

        private void showInvestigator(bool ShowFields)
        {
            //System.Collections.Generic.List<System.Windows.Forms.Control> Controls = new List<Control>();
            //Controls.Add(this.labelInvestigator);
            //Controls.Add(this.textBoxInvestigator);
            //foreach (System.Windows.Forms.Control C in Controls)
            //    if (ShowFields != C.Visible) C.Visible = ShowFields;
        }

        private void showTransactionToFields(bool ShowFields)
        {
            //System.Collections.Generic.List<System.Windows.Forms.Control> Controls = new List<Control>();
            //Controls.Add(this.labelTo);
            //Controls.Add(this.textBoxToTransactionNumber);
            //Controls.Add(this.comboBoxToCollection);
            //Controls.Add(this.userControlModuleRelatedEntryToPartner);
            //Controls.Add(this.userControlHierarchySelectorToCollection);
            //foreach (System.Windows.Forms.Control C in Controls)
            //    if (ShowFields != C.Visible) C.Visible = ShowFields;
        }

        private void showTransactionFromFields(bool ShowFields)
        {
            //System.Collections.Generic.List<System.Windows.Forms.Control> Controls = new List<Control>();
            //Controls.Add(this.labelFrom);
            //Controls.Add(this.textBoxFromTransactionNumber);
            //Controls.Add(this.comboBoxFromCollection);
            //Controls.Add(this.userControlModuleRelatedEntryFromPartner);
            //Controls.Add(this.userControlHierarchySelectorFromCollection);
            //foreach (System.Windows.Forms.Control C in Controls)
            //    if (ShowFields != C.Visible) C.Visible = ShowFields;
        }

        private void showNonEmbargoFields(bool IsEmbargo)
        {
            //this.labelMaterialCategory.Visible = !IsEmbargo;
            //this.comboBoxMaterialCategory.Visible = !IsEmbargo;
            //this.labelMaterialDescription.Visible = !IsEmbargo;
            //this.textBoxMaterialDescription.Visible = !IsEmbargo;
            //this.labelMaterialCollectors.Visible = !IsEmbargo;
            //this.textBoxMaterialCollectors.Visible = !IsEmbargo;
            //this.labelCollection.Visible = !IsEmbargo;
            //this.labelTransactionNumber.Visible = !IsEmbargo;
            //this.labelPartner.Visible = !IsEmbargo;
            //this.labelFrom.Visible = !IsEmbargo;
            //this.comboBoxFromCollection.Visible = !IsEmbargo;
            //this.userControlHierarchySelectorFromCollection.Visible = !IsEmbargo;
            //this.textBoxFromTransactionNumber.Visible = !IsEmbargo;
            //this.userControlModuleRelatedEntryFromPartner.Visible = !IsEmbargo;
            //this.labelTo.Visible = !IsEmbargo;
            //this.comboBoxToCollection.Visible = !IsEmbargo;
            //this.userControlHierarchySelectorToCollection.Visible = !IsEmbargo;
            //this.textBoxToTransactionNumber.Visible = !IsEmbargo;
            //this.userControlModuleRelatedEntryToPartner.Visible = !IsEmbargo;
            //this.labelNumberOfUnits.Visible = !IsEmbargo;
            //this.textBoxNumberOfUnits.Visible = !IsEmbargo;
            //this.labelReportingCategory.Visible = !IsEmbargo;
            //this.comboBoxReportingCategory.Visible = !IsEmbargo;
            //this.labelActualEndDate.Visible = !IsEmbargo;
            ////this.textBoxActualEndDate.Visible = !IsEmbargo;
            ////this.buttonActualEndDateEdit.Visible = !IsEmbargo;
            //this.setDateContolsVisibility(DateControls.ActualEnd, IsEmbargo);
            ////if (IsEmbargo)
            ////{
            ////    this.setDateContolsVisibility(DateControls.ActualEnd, false);
            ////    //this.dateTimePickerActualEndDate.Tag = "hide";
            ////    //this.dateTimePickerActualEndDate.Visible = false;
            ////    //this.buttonActualEndDateEdit.Tag = "hide";
            ////    //this.buttonActualEndDateEdit.Visible = false;
            ////}
            ////else
            ////{
            ////    this.setDateContolsVisibility(DateControls.ActualEnd, true);
            ////    //this.dateTimePickerActualEndDate.Tag = "show";
            ////    //this.buttonActualEndDateEdit.Tag = "show";
            ////}
            ////this.dateTimePickerActualEndDate.Visible = !IsEmbargo;
            //this.labelInvestigator.Visible = !IsEmbargo;
            //this.textBoxInvestigator.Visible = !IsEmbargo;
        }

        private void showNonPermitFields(bool IsPermit)
        {
            //this.textBoxToTransactionNumber.Visible = !IsPermit;

            //this.labelNumberOfUnits.Visible = !IsPermit;
            //this.textBoxNumberOfUnits.Visible = !IsPermit;

            //this.labelReportingCategory.Visible = !IsPermit;
            //this.comboBoxReportingCategory.Visible = !IsPermit;

            //this.labelActualEndDate.Visible = !IsPermit;
            //this.setDateContolsVisibility(DateControls.ActualEnd, IsPermit);



            //this.labelInvestigator.Visible = !IsPermit;
            //this.textBoxInvestigator.Visible = !IsPermit;

            //this.comboBoxMaterialCategory.Visible = !IsPermit;
            //this.labelMaterialDescription.Visible = !IsPermit;
            //this.textBoxMaterialDescription.Visible = !IsPermit;
            //this.labelMaterialCollectors.Visible = !IsPermit;
            //this.textBoxMaterialCollectors.Visible = !IsPermit;
            //this.labelCollection.Visible = !IsPermit;
            //this.labelTransactionNumber.Visible = !IsPermit;
            //this.labelPartner.Visible = !IsPermit;
            //this.labelFrom.Visible = !IsPermit;
            //this.comboBoxFromCollection.Visible = !IsPermit;
            //this.userControlHierarchySelectorFromCollection.Visible = !IsPermit;
            //this.textBoxFromTransactionNumber.Visible = !IsPermit;
            //this.userControlModuleRelatedEntryFromPartner.Visible = !IsPermit;
            //this.labelTo.Visible = !IsPermit;
            //this.comboBoxToCollection.Visible = !IsPermit;
            //this.userControlHierarchySelectorToCollection.Visible = !IsPermit;
            //this.textBoxToTransactionNumber.Visible = !IsPermit;
            //this.userControlModuleRelatedEntryToPartner.Visible = !IsPermit;
        }
        
        //private void showEndDateAndInvestigator(bool ShowFields)
        //{
        //    System.Collections.Generic.List<System.Windows.Forms.Control> Controls = new List<Control>();
        //    Controls.Add(this.labelActualEndDate);
        //    Controls.Add(this.labelAgreedEndDate);
        //    Controls.Add(this.buttonActualEndDateEdit);
        //    Controls.Add(this.buttonAgreedEndDateEdit);
        //    Controls.Add(this.dateTimePickerActualEndDate);
        //    Controls.Add(this.dateTimePickerAgreedEndDate);
        //    Controls.Add(this.labelInvestigator);
        //    Controls.Add(this.textBoxInvestigator);
        //    Controls.Add(this.comboBoxAgreedEndDateAddMonths);
        //    foreach (System.Windows.Forms.Control C in Controls)
        //        if (ShowFields != C.Visible) C.Visible = ShowFields;
        //    if (ShowFields) this.labelBeginDate.Text = "Begin:";
        //    else this.labelBeginDate.Text = "Date:";
        //}

#endregion
       
#endregion

#region DataEditing

#region Date controls

#region Datetimepicker Events

        private void dateTimePickerBeginDate_CloseUp(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
            R["BeginDate"] = this.dateTimePickerBeginDate.Value.ToShortDateString();
            //this.textBoxBeginDate.Text = R["BeginDate"].ToString();
        }

        private void dateTimePickerAgreedEndDate_CloseUp(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
            R["AgreedEndDate"] = this.dateTimePickerAgreedEndDate.Value.ToShortDateString();
            //this.textBoxAgreedEndDate.Text = R["AgreedEndDate"].ToString();
        }

        private void dateTimePickerActualEndDate_CloseUp(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
            R["ActualEndDate"] = this.dateTimePickerActualEndDate.Value.ToShortDateString();
            //this.textBoxActualEndDate.Text = R["ActualEndDate"].ToString();
        }

        private void comboBoxAgreedEndDateAddMonths_SelectionChangeCommitted(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
            if (R["BeginDate"].Equals(System.DBNull.Value))
            {
                System.Windows.Forms.MessageBox.Show("No start date is given");
                return;
            }
            System.DateTime Begin;
            if (System.DateTime.TryParse(R["BeginDate"].ToString(), out Begin))
            {
                string Intervall = this.comboBoxAgreedEndDateAddMonths.SelectedItem.ToString();
                System.DateTime End = Begin.AddMonths(this.MonthIntervalls[Intervall]);
                R["AgreedEndDate"] = End.ToShortDateString();
                this.dateTimePickerAgreedEndDate.Value = End;
                //this.textBoxAgreedEndDate.Text = R["AgreedEndDate"].ToString();
                this.setAgreedEndDateControls(R.Row);
            }
        }

        private void comboBoxActualEndDateAddMonths_SelectionChangeCommitted(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
            if (R["AgreedEndDate"].Equals(System.DBNull.Value))
            {
                System.Windows.Forms.MessageBox.Show("No end date is given");
                return;
            }
            System.DateTime Begin;
            if (System.DateTime.TryParse(R["AgreedEndDate"].ToString(), out Begin))
            {
                string Intervall = this.comboBoxActualEndDateAddMonths.SelectedItem.ToString();
                System.DateTime End = Begin.AddMonths(this.MonthIntervalls[Intervall]);
                R["ActualEndDate"] = End.ToShortDateString();
                this.dateTimePickerActualEndDate.Value = End;
                //this.textBoxAgreedEndDate.Text = R["AgreedEndDate"].ToString();
                this.setActualEndDateControls(R.Row);
            }
        }

#endregion

#region DateButtons

        private void buttonBeginDateEdit_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
            if (R["BeginDate"].Equals(System.DBNull.Value))
            {
                R["BeginDate"] = System.DateTime.Now;
            }
            else
            {
                R["BeginDate"] = System.DBNull.Value;
            }
            this.setBeginDateControls(R.Row);
        }

        private void buttonAgreedEndDateEdit_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
            if (R["AgreedEndDate"].Equals(System.DBNull.Value))
            {
                R["AgreedEndDate"] = System.DateTime.Now;
            }
            else
            {
                R["AgreedEndDate"] = System.DBNull.Value;
            }
            this.setAgreedEndDateControls(R.Row);
        }

        private void buttonActualEndDateEdit_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
            if (R["ActualEndDate"].Equals(System.DBNull.Value))
            {
                R["ActualEndDate"] = System.DateTime.Now;
            }
            else
            {
                R["ActualEndDate"] = System.DBNull.Value;
            }
            this.setActualEndDateControls(R.Row);
        }


#endregion
        
#endregion
    
        private void comboBoxAdministratingCollection_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxAdministratingCollection_TextChanged(object sender, EventArgs e)
        {
            if (this.comboBoxAdministratingCollection.Text.Length > 0)
            {
                this.checkBoxSendingRestrictToCollection.Text = "Restrict to " + this.comboBoxAdministratingCollection.Text;
                this.checkBoxSendingRestrictToCollection.Visible = true;
                this.pictureBoxSendingRestrictToCollection.Visible = true;
                this.checkBoxPrintingIncludeSubcollections.Text = "subcollections of " + this.comboBoxAdministratingCollection.Text; ;
            }
            else
            {
                this.checkBoxSendingRestrictToCollection.Visible = false;
                this.pictureBoxSendingRestrictToCollection.Visible = false;
            }
        }

        private void comboBoxTransactionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.transactionBindingSource.Current != null)
            //{
            //    this._Transaction.ItemChanged();
            //    System.Data.DataRowView RV = (System.Data.DataRowView)this.transactionBindingSource.Current;
            //    string Type = this.comboBoxTransactionType.Text.ToUpper();
            //    if (RV["TransactionType"].ToString() == Type)
            //        this.initForm(Type);
            //}
            //string Type = this.comboBoxTransactionType.Text.ToUpper();
            //System.Data.DataRowView RV = (System.Data.DataRowView)this.transactionBindingSource.Current;
            //this.tabControlTransaction.TabPages.Remove(this.tabPageBalance);
            //this.tabControlTransaction.TabPages.Remove(this.tabPageConfirmation);
            //this.tabControlTransaction.TabPages.Remove(this.tabPageDocuments);
            //this.tabControlTransaction.TabPages.Remove(this.tabPagePrinting);
            //this.tabControlTransaction.TabPages.Remove(this.tabPageReminder);
            //this.tabControlTransaction.TabPages.Remove(this.tabPageSending);
            //this.tabControlTransaction.TabPages.Remove(this.tabPageForwarding);
            //this.tabControlTransaction.TabPages.Remove(this.tabPageTransactionReturn);
            //this.setTitle();
            //if (this.transactionBindingSource.Current != null)
            //{
            //    if (Type == "FORWARDING" || Type == "RETURN")
            //    {
            //        try
            //        {
            //            bool ResetType = false;
            //            System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
            //            if (R["ParentTransactionID"].Equals(System.DBNull.Value))
            //            {
            //                System.Windows.Forms.MessageBox.Show("The type " + Type.ToLower() + " must be part of a loan");
            //                ResetType = true;
            //            }
            //            else
            //            {
            //                System.Windows.Forms.TreeNode TP = this.treeViewTransaction.SelectedNode.Parent;
            //                System.Data.DataRow RP = (System.Data.DataRow)TP.Tag;
            //                string ParentType = RP["TransactionType"].ToString();
            //                if (ParentType.ToLower() != "loan" && ParentType.ToLower() != "forwarding")
            //                {
            //                    System.Windows.Forms.MessageBox.Show("The type " + Type.ToLower() + " must be part of a loan");
            //                    ResetType = true;
            //                }
            //            }
            //            if (ResetType)
            //            {
            //                string TransactionType = R["TransactionType"].ToString().ToLower();
            //                int i = 0;
            //                if (TransactionType == "forwarding" || TransactionType == "return")
            //                {
            //                    TransactionType = "loan";
            //                    R["TransactionType"] = "loan";
            //                    this._Transaction.saveItem();
            //                    int ID = int.Parse(R["TransactionID"].ToString());
            //                    this._Transaction.setItem(ID);
            //                }
            //                foreach (System.Object o in this.comboBoxTransactionType.Items)
            //                {
            //                    System.Data.DataRowView rv = (System.Data.DataRowView)o;
            //                    if (rv["Code"].ToString().ToLower() == TransactionType.ToLower())
            //                        break;
            //                    i++;
            //                }
            //                this.comboBoxTransactionType.SelectedIndex = i;
            //                return;
            //            }
            //        }
            //        catch { };
            //    }
            //    this.initForm(Type);
            //}
        }

        private void comboBoxTransactionType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            System.Data.DataRowView RS = (System.Data.DataRowView)this.comboBoxTransactionType.SelectedItem;
            string Type = RS[0].ToString().ToUpper();// this.comboBoxTransactionType.Text.ToUpper();
            //System.Data.DataRowView RV = (System.Data.DataRowView)this.transactionBindingSource.Current;
            //this.tabControlTransaction.TabPages.Remove(this.tabPageBalance);
            //this.tabControlTransaction.TabPages.Remove(this.tabPageConfirmation);
            //this.tabControlTransaction.TabPages.Remove(this.tabPageDocuments);
            //this.tabControlTransaction.TabPages.Remove(this.tabPagePrinting);
            //this.tabControlTransaction.TabPages.Remove(this.tabPageReminder);
            //this.tabControlTransaction.TabPages.Remove(this.tabPageSending);
            //this.tabControlTransaction.TabPages.Remove(this.tabPageForwarding);
            //this.tabControlTransaction.TabPages.Remove(this.tabPageTransactionReturn);
            this.setTitle();
            if (this.transactionBindingSource.Current != null)
            {
                if (Type == "FORWARDING" || Type == "RETURN")
                {
                    try
                    {
                        bool ResetType = false;
                        System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
                        if (R["ParentTransactionID"].Equals(System.DBNull.Value))
                        {
                            System.Windows.Forms.MessageBox.Show("The type " + Type.ToLower() + " must be part of a loan");
                            ResetType = true;
                        }
                        else
                        {
                            System.Windows.Forms.TreeNode TP = this.treeViewTransaction.SelectedNode.Parent;
                            System.Data.DataRow RP = (System.Data.DataRow)TP.Tag;
                            string ParentType = RP["TransactionType"].ToString();
                            if (ParentType.ToLower() != "loan" && ParentType.ToLower() != "forwarding")
                            {
                                System.Windows.Forms.MessageBox.Show("The type " + Type.ToLower() + " must be part of a loan");
                                ResetType = true;
                            }
                        }
                        if (ResetType)
                        {
                            string TransactionType = R["TransactionType"].ToString().ToLower();
                            int i = 0;
                            if (TransactionType == "forwarding" || TransactionType == "return")
                            {
                                TransactionType = "loan";
                                R["TransactionType"] = "loan";
                                this._Transaction.saveItem();
                                int ID = int.Parse(R["TransactionID"].ToString());
                                this._Transaction.setItem(ID);
                            }
                            foreach (System.Object o in this.comboBoxTransactionType.Items)
                            {
                                System.Data.DataRowView rv = (System.Data.DataRowView)o;
                                if (rv["Code"].ToString().ToLower() == TransactionType.ToLower())
                                    break;
                                i++;
                            }
                            this.comboBoxTransactionType.SelectedIndex = i;
                            return;
                        }
                    }
                    catch { };
                }
                else
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
                    R["TransactionType"] = Type;
                }
                this.initForm(Type);
            }
        }

        private void comboBoxReportingCategory_DropDown(object sender, EventArgs e)
        {
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter("", DiversityWorkbench.Settings.ConnectionString);
            System.Data.DataTable dt = new DataTable();
            System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
            string ReportingCategory = "";
            if (!R["ReportingCategory"].Equals(System.DBNull.Value) && R["ReportingCategory"].ToString().Length > 0)
                ReportingCategory = R["ReportingCategory"].ToString();
            if (ReportingCategory.Length > 0)
            {
                ad.SelectCommand.CommandText = "SELECT '" + ReportingCategory + "' AS ReportingCategory";
                ad.Fill(dt);
            }
            ad.SelectCommand.CommandText = "SELECT distinct rtrim(ReportingCategory) AS ReportingCategory FROM dbo.[Transaction] WHERE rtrim(ReportingCategory) <> '' ORDER BY rtrim(ReportingCategory)";
            ad.Fill(dt);
            this.comboBoxReportingCategory.DataSource = dt;
            this.comboBoxReportingCategory.DisplayMember = "ReportingCategory";
            this.comboBoxReportingCategory.ValueMember = "ReportingCategory";
        }

        private void comboBoxReportingCategory_SelectionChangeCommitted(object sender, EventArgs e)
        {
            System.Data.DataRowView RV = (System.Data.DataRowView)this.comboBoxReportingCategory.SelectedItem;
            string Category = RV[0].ToString();// this.comboBoxReportingCategory.Text;
            System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
            R.BeginEdit();
            R["ReportingCategory"] = Category;
            R.EndEdit();
        }

        private void comboBoxTransactionCommentAdd_DropDown(object sender, EventArgs e)
        {
            string SQL = "SELECT Comment " +
                " FROM         TransactionComment " +
                " WHERE 1 = 1 ";
            //if (this.comboBoxAdministratingCollection.SelectedItem != null)
            //{
            //    System.Data.DataRowView RV = (System.Data.DataRowView)this.comboBoxAdministratingCollection.SelectedItem;
            //    SQL += " AND AdministratingCollectionID = " + RV["CollectionID"].ToString();
            //}
            if (this.comboBoxTransactionCommentAdd.Text.Length > 0)
            {
                SQL += " AND Comment LIKE '" + this.comboBoxTransactionCommentAdd.Text + "%'";
            }
            SQL += "ORDER BY Comment";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                ad.Fill(dt);
                this.comboBoxTransactionCommentAdd.DataSource = dt;
                this.comboBoxTransactionCommentAdd.DisplayMember = "Comment";
                this.comboBoxTransactionCommentAdd.ValueMember = "Comment";
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonTransactionCommentInsertSelected_Click(object sender, EventArgs e)
        {
            if (comboBoxTransactionCommentAdd.Text.Length > 0)
            {
                if (this.textBoxTransactionComment.Text.Trim().Length > 0)
                    this.textBoxTransactionComment.Text += " ";
                this.textBoxTransactionComment.Text += this.comboBoxTransactionCommentAdd.Text;
                System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
                R.BeginEdit();
                R["TransactionComment"] = this.textBoxTransactionComment.Text;
                R.EndEdit();
            }
            else
                System.Windows.Forms.MessageBox.Show("Nothing selected");
        }

        private void comboBoxFromCollection_TextChanged(object sender, EventArgs e)
        {
            // not used any more - linked to new field in settings

            // restricted to the balance - the rest of the functionallity is connected to the administrating collection
            //if (this.comboBoxFromCollection.Text.Length > 0 
            //    && this.comboBoxFromCollection.SelectedIndex > -1
            //    && this.comboBoxFromCollection.Text != "System.Data.DataRowView")
            //{
            //    this.checkBoxBalanceIncludeFromSubcollections.Text = "Include subcollections of " + this.comboBoxFromCollection.Text;
            //    this.checkBoxBalanceFromIncludeAllCollections.Text = "Include all collections related to " + this.comboBoxFromCollection.Text;
            //    this.checkBoxBalanceIncludeFromSubcollections.Visible = true;
            //    this.checkBoxBalanceFromIncludeAllCollections.Visible = true;
            //}
            //else
            //{
            //    this.checkBoxBalanceFromIncludeAllCollections.Visible = false;
            //    this.checkBoxBalanceIncludeFromSubcollections.Visible = false;
            //}
        }

        private void comboBoxToCollection_TextChanged(object sender, EventArgs e)
        {
            // not used any more - linked to new field in settings
            
            //if (this.comboBoxToCollection.Text.Length > 0
            //    && this.comboBoxToCollection.SelectedIndex > -1
            //    && this.comboBoxToCollection.Text != "System.Data.DataRowView")
            //{
            //    this.checkBoxBalanceIncludeToSubcollections.Text = "Include subcollections of " + this.comboBoxToCollection.Text;
            //    this.checkBoxBalanceToIncludeAllCollections.Text = "Include all collections related to " + this.comboBoxToCollection.Text;
            //    this.checkBoxBalanceIncludeToSubcollections.Visible = true;
            //    this.checkBoxBalanceToIncludeAllCollections.Visible = true;
            //}
            //else
            //{
            //    this.checkBoxBalanceIncludeToSubcollections.Visible = false;
            //    this.checkBoxBalanceToIncludeAllCollections.Visible = false;
            //}
        }

        private void comboBoxMaterialCategory_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.comboBoxMaterialCategory.Text.Length > 0 && this.comboBoxMaterialCategory.Text != "System.Data.DataRowView")
                {
                    if (this.comboBoxMaterialCategory.SelectedItem != null)
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxMaterialCategory.SelectedItem;
                        this.checkBoxSendingRestrictToMaterial.Text = "Restrict to " + R["DisplayText"].ToString();
                        this.setMaterialCategoryImage(R["Code"].ToString());
                        this.checkBoxSendingRestrictToMaterial.Visible = true;
                        this.pictureBoxSendingRestrictToMaterial.Visible = true;
                    }
                }
                else
                {
                    this.checkBoxSendingRestrictToMaterial.Visible = false;
                    this.pictureBoxSendingRestrictToMaterial.Visible = false;
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void comboBoxAdministratingCollection_SelectionChangeCommitted(object sender, EventArgs e)
        {
            System.Data.DataRowView RT = (System.Data.DataRowView)this.transactionBindingSource.Current;
            System.Data.DataRowView RC = (System.Data.DataRowView)this.comboBoxAdministratingCollection.SelectedItem;
            RT.BeginEdit();
            RT["AdministratingCollectionID"] = int.Parse(RC["CollectionID"].ToString());
            RT.EndEdit();
        }

        private void checkBoxIncludeTypeInformation_CheckedChanged(object sender, EventArgs e)
        {
            this._Transaction.IncludeTypeInformation = this.checkBoxIncludeTypeInformation.Checked;
        }

        private void dateTimePickerBeginDate_ValueChanged(object sender, EventArgs e)
        {
            //this.setbuttonBeginDate();
        }

        private void dateTimePickerBeginDate_Validated(object sender, EventArgs e)
        {
            //this.setbuttonBeginDate();
        }

        private void textBoxNumberOfUnits_TextChanged(object sender, EventArgs e)
        {
            if (this.textBoxNumberOfUnits.Text.Length == 0 && this.transactionBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
                    R.BeginEdit();
                    R["NumberOfUnits"] = System.DBNull.Value;
                    R.EndEdit();
                    R.Row.AcceptChanges();
            }
        }

        private void textBoxNumberOfUnits_Validating(object sender, CancelEventArgs e)
        {
            int i = 0;
            if (!int.TryParse(this.textBoxNumberOfUnits.Text, out i))
                System.Windows.Forms.MessageBox.Show("only numbers are allowed here");
        }

        private void treeViewTransaction_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewTransaction.SelectedNode.Tag;
                this.setDateControls(R);
            }
            catch (System.Exception ex)
            {
            }
        }

        private void comboBoxToRecipient_DropDown(object sender, EventArgs e)
        {
            string SQL = "SELECT DISTINCT ToRecipient FROM [Transaction] WHERE ToRecipient <> '' ";
            if (this.transactionBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
                if (R["ToTransactionPartnerName"].Equals(System.DBNull.Value) && R["ToTransactionPartnerName"].ToString().Length > 0)
                {
                    SQL += " AND ToTransactionPartnerName = '" + R["ToTransactionPartnerName"].ToString() + "'";
                }
            }
            SQL += " ORDER BY ToRecipient";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            this.comboBoxToRecipient.DataSource = dt;
            this.comboBoxToRecipient.DisplayMember = "ToRecipient";
            this.comboBoxToRecipient.ValueMember = "ToRecipient";
        }

        private void comboBoxMaterialSource_DropDown(object sender, EventArgs e)
        {
            string SQL = "SELECT DISTINCT MaterialSource FROM [Transaction] WHERE MaterialSource <> '' ORDER BY MaterialSource";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            this.comboBoxMaterialSource.DataSource = dt;
            this.comboBoxMaterialSource.DisplayMember = "MaterialSource";
            this.comboBoxMaterialSource.ValueMember = "MaterialSource";
        }

#endregion

#region Sending

#region Toolstrip

        private void toolStripButtonOpenSendingSchemaFile_Click(object sender, EventArgs e)
        {
            this.setSchemaFile(this.textBoxSendingSchema, Transaction.TransactionCorrespondenceType.Sending);
            if (this.textBoxSendingSchema.Text.Length > 0)
                DiversityCollection.Forms.FormTransactionSettings.Default.SendingSchemaFile = this.textBoxSendingSchema.Text;
        }

        private void SendingSnsbToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // System.Diagnostics.Process.Start("https://github.com/SNSB/DWB-Contrib/tree/master/DiversityCollection/Transaction/Schemas/Sending");
            string Link = "https://github.com/SNSB/DWB-Contrib/tree/master/DiversityCollection/Transaction/Schemas/Sending";
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(Link);
            info.UseShellExecute = true;
            System.Diagnostics.Process.Start(info);
        }

        private void toolStripButtonSendingShowList_Click(object sender, EventArgs e)
        {
            this.EnableSendingList(true);
        }

        private void toolStripButtonSendingPreview_Click(object sender, EventArgs e)
        {
            bool SchemaOK = this.XsltFileOK(this.textBoxSendingSchema.Text);
            if (this.textBoxSendingSchema.Text.Length > 0 && !SchemaOK)
                return;

            string Type = "";
            System.Data.DataRow[] rr = this._Transaction.DataSet.Tables["Transaction"].Select("TransactionID = " + this._Transaction.ID.ToString());
            if (rr.Length > 0)
            {
                Type = rr[0]["TransactionType"].ToString();
            }

            if (this._Transaction.DataSet.Tables["CollectionSpecimenPartList"].Rows.Count == 0 && Type != "exchange")
            {
                System.Windows.Forms.MessageBox.Show("No specimen where selected");
                return;
            }
            if (this.transactionBindingSource.Current != null)
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                try
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
                    int ID = int.Parse(R[0].ToString());
                    //int Count = 0;
                    string SQL = "SELECT COUNT(*) AS Count FROM CollectionSpecimenTransaction WHERE TransactionID = " + ID.ToString();
                    string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\Transaction\\Sending.XML";
                    System.IO.FileInfo XML = new System.IO.FileInfo(Path);
                    Transaction.TransactionSpecimenOrder SO = Transaction.TransactionSpecimenOrder.Taxa;
                    if (this.checkBoxSendingSingleLines.Checked && this.checkBoxSendingOrderByTaxa.Checked)
                        SO = Transaction.TransactionSpecimenOrder.TaxaSingleAccessionNumber;
                    else if (this.checkBoxSendingSingleLines.Checked && !this.checkBoxSendingOrderByTaxa.Checked)
                        SO = Transaction.TransactionSpecimenOrder.SingleAccessionNumber;
                    else if (!this.checkBoxSendingSingleLines.Checked && this.checkBoxSendingOrderByTaxa.Checked)
                        SO = Transaction.TransactionSpecimenOrder.Taxa;
                    else
                        SO = Transaction.TransactionSpecimenOrder.AccessionNumber;
                    this._XmlSending = this._Transaction.XmlCorrespondence(
                        XML,
                        this.textBoxSendingSchema.Text,
                        Transaction.TransactionCorrespondenceType.Sending,
                        null,
                        ID,
                        this.AdministrationAgentURI(),
                        this.dateTimePickerBeginDate.Value,
                        false, false, this.checkBoxSendingIncludeType.Checked, false, Transaction.ConversionType.No_Conversion, SO, 
                        this.checkBoxSendingAllUnits.Checked); // #127
                    System.Uri U = new Uri(this._XmlSending);
                    this.webBrowserSending.Url = U;

                    if (SchemaOK && System.Windows.Forms.MessageBox.Show("Do you want to save the document", "Save?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        this.toolStripButtonSendingSave_Click(null, null);
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void toolStripButtonSendingPreviewAllOnLoan_Click(object sender, EventArgs e)
        {
            string Type = "";
            System.Data.DataRow[] rr = this._Transaction.DataSet.Tables["Transaction"].Select("TransactionID = " + this._Transaction.ID.ToString());
            if (rr.Length > 0)
            {
                Type = rr[0]["TransactionType"].ToString();
            }

            if (this._Transaction.DataSet.Tables["CollectionSpecimenPartList"].Rows.Count == 0 && Type != "exchange")
            {
                System.Windows.Forms.MessageBox.Show("No specimen where selected");
                return;
            }
            if (this.transactionBindingSource.Current != null)
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                try
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
                    int ID = int.Parse(R[0].ToString());

                    string SQL = "SELECT COUNT(*) AS Count FROM CollectionSpecimenTransaction WHERE TransactionID = " + ID.ToString();
                    string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\Transaction\\Sending.XML";
                    System.IO.FileInfo XML = new System.IO.FileInfo(Path);
                    Transaction.TransactionSpecimenOrder SO = Transaction.TransactionSpecimenOrder.Taxa;
                    if (this.checkBoxSendingSingleLines.Checked && this.checkBoxSendingOrderByTaxa.Checked)
                        SO = Transaction.TransactionSpecimenOrder.TaxaSingleAccessionNumber;
                    else if (this.checkBoxSendingSingleLines.Checked && !this.checkBoxSendingOrderByTaxa.Checked)
                        SO = Transaction.TransactionSpecimenOrder.SingleAccessionNumber;
                    else if (!this.checkBoxSendingSingleLines.Checked && this.checkBoxSendingOrderByTaxa.Checked)
                        SO = Transaction.TransactionSpecimenOrder.Taxa;
                    else
                        SO = Transaction.TransactionSpecimenOrder.AccessionNumber;
                    this._XmlSending = this._Transaction.XmlCorrespondence(
                        XML,
                        this.textBoxSendingSchema.Text,
                        Transaction.TransactionCorrespondenceType.Sending,
                        null,
                        ID,
                        this.AdministrationAgentURI(),
                        this.dateTimePickerBeginDate.Value,
                        false, false, this.checkBoxSendingIncludeType.Checked, true, Transaction.ConversionType.No_Conversion, SO, this.checkBoxSendingAllUnits.Checked); // #127
                    System.Uri U = new Uri(this._XmlSending);
                    this.webBrowserSending.Url = U;
                    if (this.textBoxSendingSchema.Text.Length > 0) // #127
                    {
                        if (System.Windows.Forms.MessageBox.Show("Do you want to save the document", "Save?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            this.toolStripButtonSendingSave_Click(null, null);
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void toolStripButtonSendingXslEditor_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            return;
            try
            {
                DiversityWorkbench.XslEditor.FormXslEditor F = new DiversityWorkbench.XslEditor.FormXslEditor(this.webBrowserSending.Url.ToString(), false);
                F.StartPosition = FormStartPosition.CenterParent;
                F.Width = this.Width - 10;
                F.Height = this.Height - 10;
                F.ShowDialog();
            }
            catch (System.Exception ex) { }
        }

        private void toolStripButtonSendingSave_Click(object sender, EventArgs e)
        {
            string Type = "";
            System.Data.DataRow[] rr = this._Transaction.DataSet.Tables["Transaction"].Select("TransactionID = " + this._Transaction.ID.ToString());
            if (rr.Length > 0)
            {
                Type = rr[0]["TransactionType"].ToString();
            }
            if (Type == TransactionType.loan.ToString() ||
                Type == TransactionType.forwarding.ToString())
            {
                if (this._Transaction.DataSet.Tables["CollectionSpecimenPartList"] == null || this._Transaction.DataSet.Tables["CollectionSpecimenPartList"].Rows.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("No specimen selected");
                    return;
                }
            }
            if (this.webBrowserSending.DocumentText.Length < 15)
            {
                System.Windows.Forms.MessageBox.Show("No document created");
                return;
            }
            this.saveDocumentEntry(this.webBrowserSending, "Sending");
        }

        private void toolStripButtonSendingPrint_Click(object sender, EventArgs e)
        {
            string Type = "";
            System.Data.DataRow[] rr = this._Transaction.DataSet.Tables["Transaction"].Select("TransactionID = " + this._Transaction.ID.ToString());
            if (rr.Length > 0)
            {
                Type = rr[0]["TransactionType"].ToString();
            }
            if (this._Transaction.DataSet.Tables["CollectionSpecimenPartList"] == null || this._Transaction.DataSet.Tables["CollectionSpecimenPartList"].Rows.Count == 0)
            {
                if (Type == TransactionType.loan.ToString() ||
                    Type == TransactionType.forwarding.ToString())
                {
                    System.Windows.Forms.MessageBox.Show("No specimen selected");
                    return;
                }
            }
            if (this.webBrowserSending.DocumentText.Length < 15)
            {
                System.Windows.Forms.MessageBox.Show("No document created");
                return;
            }
            this.webBrowserSending.ShowPrintPreviewDialog();
        }

#endregion

#region Scanner

        //private bool _UseSendingScanner = false;

        //private bool UseSendingScanner
        //{
        //    //get { return _UseSendingScanner; }
        //    set 
        //    { 
        //        _UseSendingScanner = value;
        //        if (value)
        //        {
        //            this.toolStripButtonSendingScanner.BackColor = System.Drawing.Color.Pink;
        //            this.textBoxSendingAccessionNumber.BackColor = System.Drawing.Color.Pink;
        //            this.textBoxSendingAccessionNumber.Enabled = true;
        //            this.startScannerSending();
        //        }
        //        else
        //        {
        //            this.toolStripButtonSendingScanner.BackColor = System.Drawing.SystemColors.Control;
        //            this.textBoxSendingAccessionNumber.BackColor = System.Drawing.Color.White;
        //            this.textBoxSendingAccessionNumber.Enabled = false;
        //            this.stopScannerSending();
        //        }
        //    }
        //}

        private void toolStripButtonSendingScanner_Click(object sender, EventArgs e)
        {
            this.setTimerIntervall();

            //this.setSendingTimerIntervall();
            //this.UseSendingScanner = !this._UseSendingScanner;
        }

        private void toolStripButtonSendingScanner_DoubleClick(object sender, EventArgs e)
        {
            //this.setSendingTimerIntervall();
        }

        private void buttonSendingScanner_Click(object sender, EventArgs e)
        {
        }

        private void textBoxSendingAccessionNumber_TextChanged(object sender, EventArgs e)
        {
            if (this.textBoxSendingAccessionNumber.Text.Length > 0)
                this.startScannerSending();
        }

        private void textBoxSendingAccessionNumber_MouseEnter(object sender, EventArgs e)
        {
            this.textBoxSendingAccessionNumber.Focus();
            this.textBoxSendingAccessionNumber.SelectAll();
        }

        private void timerSending_Tick(object sender, EventArgs e)
        {
            //if (this._CurrentScanStepSending == ScanStep.Idle)
            //    return;

            if (this.textBoxSendingAccessionNumber.Text.Length > 0)
            {
                try
                {
                    int? CollectionID = null;
                    if (this.checkBoxSendingRestrictToCollection.Visible && this.checkBoxSendingRestrictToCollection.Checked)
                    {
                        int CollID;

                        System.Data.DataRowView RV = (System.Data.DataRowView)this.comboBoxAdministratingCollection.SelectedItem;
                        if (int.TryParse(RV["CollectionID"].ToString(), out CollID))
                            CollectionID = CollID;
                    }
                    string MaterialCategory = "";
                    if (this.checkBoxSendingRestrictToMaterial.Visible && this.checkBoxSendingRestrictToMaterial.Checked)
                        MaterialCategory = this.comboBoxMaterialCategory.Text;

                    if (!this.InsertSpecimenTransaction(this.ID, this.textBoxSendingAccessionNumber.Text, true, CollectionID, MaterialCategory, false))
                    {
                        this.timerSending.Stop();
                        System.Windows.Forms.MessageBox.Show("Either the accession number " + this.textBoxSendingAccessionNumber.Text + " is already in the list " +
                            "\r\n\tor\r\n no dataset could be found for the accession number " + this.textBoxSendingAccessionNumber.Text +
                            "\r\n\tor\r\n you have no access to these data ", "No data found");
                        this.textBoxSendingAccessionNumber.Text = "";
                    }

                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            this._CurrentScanStepSending = ScanStep.Idle;
            this.textBoxSendingAccessionNumber.SelectAll();
            this.timerSending.Stop();
        }

        private void startScannerSending()
        {
            if (this._CurrentScanStepSending == ScanStep.Idle)
            {
                this._CurrentScanStepSending = ScanStep.Scanning;
                this.timerSending.Start();
            }
        }

        private void stopScannerSending()
        {
            if (this._CurrentScanStepSending == ScanStep.Scanning)
            {
                this._CurrentScanStepSending = ScanStep.Idle;
                this.timerSending.Stop();
            }
        }

        /// <summary>
        /// Setting the timer intervall for the Scanner
        /// </summary>
        //private void setSendingTimerIntervall()
        //{
        //    DiversityWorkbench.Forms.FormGetInteger f = new DiversityWorkbench.Forms.FormGetInteger(DiversityCollection.Forms.FormTransactionSettings.Default.TimerIntervall,
        //        "Timer intervall", "Please give a value for the timer intervall of the scanner in milliseconds");
        //    f.ShowDialog();
        //    if (f.DialogResult == DialogResult.OK)
        //    {
        //        if (f.Integer != null)
        //        {
        //            int i = (int)f.Integer;
        //            this.timerSending.Interval = i;
        //            DiversityCollection.Forms.FormTransactionSettings.Default.TimerIntervall = i;
        //        }
        //    }
        //}

#endregion

        private void setMaterialCategoryImage(string MaterialCategory)
        {
            this.pictureBoxSendingRestrictToMaterial.Image = DiversityCollection.Specimen.MaterialCategoryImage(false, MaterialCategory);
        }

        private void comboBoxSendingRestrictToTaxonomicGroup_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxSendingRestrictToTaxonomicGroup.DataSource == null)
            {
                string SQL = "SELECT NULL AS [Code] UNION SELECT Code FROM [dbo].[CollTaxonomicGroup_Enum] ORDER BY CODE";
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                System.Data.DataTable dt = new DataTable();
                a.Fill(dt);
                this.comboBoxSendingRestrictToTaxonomicGroup.DataSource = dt;
                this.comboBoxSendingRestrictToTaxonomicGroup.DisplayMember = "Code";
                this.comboBoxSendingRestrictToTaxonomicGroup.ValueMember = "Code";
            }
        }

        private void comboBoxSendingRestrictToTaxonomicGroup_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string TaxGroup = this.comboBoxSendingRestrictToTaxonomicGroup.SelectedValue.ToString();
            //if(this.comboBoxSendingRestrictToTaxonomicGroup.eq
            this._Transaction.RestrictToTaxonomicGroup = TaxGroup;
        }

#region Manuell handling of list items

        private void comboBoxSendingAccessionNumber_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.comboBoxSendingAccessionNumber.SelectedItem != null)
                {
                    int? CollectionID = null;
                    if (this.checkBoxSendingRestrictToCollection.Visible && this.checkBoxSendingRestrictToCollection.Checked && this.comboBoxFromCollection.SelectedItem != null)
                    {
                        int CollID;
                        System.Data.DataRowView RVColl = (System.Data.DataRowView)this.comboBoxFromCollection.SelectedItem;
                        if (int.TryParse(RVColl["CollectionID"].ToString(), out CollID))
                            CollectionID = CollID;
                    }
                    string MaterialCategory = "";
                    if (this.checkBoxSendingRestrictToMaterial.Visible && this.checkBoxSendingRestrictToMaterial.Checked)
                        MaterialCategory = this.comboBoxMaterialCategory.Text;

                    System.Data.DataRowView RV = (System.Data.DataRowView)this.comboBoxSendingAccessionNumber.SelectedItem;
                    string AccNr = RV["AccessionNumber"].ToString();
                    this.InsertSpecimenTransaction(this.ID, AccNr, true, CollectionID, MaterialCategory, false);
                }
            }
            catch (System.Exception ex) { }
        }

        private void comboBoxSendingAccessionNumber_DropDown(object sender, EventArgs e)
        {
            try
            {
                int CollectionID = this.AdministrativeCollectionID;
                string SQL = "SELECT DISTINCT AccessionNumber " +
                    "FROM ManagerSpecimenPartList " +
                    "WHERE AccessionNumber LIKE '" + this.comboBoxSendingAccessionNumber.Text + "%' ";
                if (this.checkBoxSendingRestrictToCollection.Visible && this.checkBoxSendingRestrictToCollection.Checked && this.comboBoxFromCollection.SelectedItem != null)
                {
                    System.Data.DataRowView RV = (System.Data.DataRowView)this.comboBoxFromCollection.SelectedItem;
                    if (int.TryParse(RV["CollectionID"].ToString(), out CollectionID))
                        SQL += " AND CollectionID = " + CollectionID.ToString();
                }
                if (this.checkBoxSendingRestrictToMaterial.Visible && this.checkBoxSendingRestrictToMaterial.Checked)
                {
                    SQL += " AND MaterialCategory = N'" + this.comboBoxMaterialCategory.Text + "'";
                }
                if (this.dataSetTransaction.CollectionSpecimenPartOnLoanList.Rows.Count > 0)
                {
                    SQL += " AND AccessionNumber NOT IN (";
                    foreach (DiversityCollection.Datasets.DataSetTransaction.CollectionSpecimenPartOnLoanListRow R in this.dataSetTransaction.CollectionSpecimenPartOnLoanList.Rows)
                    {
                        if (!R.AccessionNumber.Equals(System.DBNull.Value))
                            SQL += "'" + R.AccessionNumber.ToString() + "', ";
                    }
                    SQL = SQL.Substring(0, SQL.Length - 2) + ") ";
                }
                SQL += " ORDER BY AccessionNumber";
                try
                {
                    System.Data.DataTable dt = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    this.comboBoxSendingAccessionNumber.DataSource = dt;
                    this.comboBoxSendingAccessionNumber.DisplayMember = "AccessionNumber";
                    this.comboBoxSendingAccessionNumber.ValueMember = "AccessionNumber";
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            catch (System.Exception ex)
            {
            }
        }
        
#endregion
     
#region Toolstrip List

        private void toolStripButtonSendingListDelete_Click(object sender, EventArgs e)
        {
            if (this.listBoxSendingSpecimen.SelectedIndex > -1)
            {
                System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxSendingSpecimen.SelectedItem;
                string SQL = "DELETE FROM CollectionSpecimenTransaction " +
                    "WHERE (TransactionID = " + this.ID.ToString() + ")" +
                    " AND CollectionSpecimenID = " + RV["CollectionSpecimenID"].ToString() +
                    " AND SpecimenPartID = " + RV["SpecimenPartID"].ToString();
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    C.ExecuteNonQuery();
                    con.Close();
                }
                catch { }
                RV.Delete();
                this._Transaction.saveDependentTables();
                this._Transaction.RequerySpecimenLists();
            }
        }

        private void toolStripButtonSendingListFind_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxSendingSpecimen.SelectedItem;
                int ID;
                if (int.TryParse(R["CollectionSpecimenID"].ToString(), out ID))
                    this.OpenSpecimen(ID);
            }
            catch { }
        }
        
#endregion

#region Permit

        private DiversityCollection.UserControls.UserControlPermit _UCP;

        private void listBoxSendingSpecimen_Click(object sender, EventArgs e)
        {
            bool HasPermit = false;
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxSendingSpecimen.SelectedItem;
                int PermitID;
                if (!R["PermitID"].Equals(System.DBNull.Value) && int.TryParse(R["PermitID"].ToString(), out PermitID))
                {
                    if (this._UCP == null)
                    {
                        this._UCP = new UserControls.UserControlPermit();
                        this._UCP.Dock = DockStyle.Fill;
                        this.splitContainerWebbrowserSending.Panel2.Controls.Add(this._UCP);
                    }
                    this.splitContainerWebbrowserSending.Panel1Collapsed = true;
                    this.splitContainerWebbrowserSending.Panel2Collapsed = false;
                    HasPermit = true;
                    this._UCP.setTransaction(PermitID);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            if (!HasPermit)
            {
                this.ResetPermitDisplay();
            }
        }

        private void listBoxSendingSpecimen_Leave(object sender, EventArgs e)
        {
            this.ResetPermitDisplay();
        }

        private void ResetPermitDisplay()
        {
            this.splitContainerWebbrowserSending.Panel2Collapsed = true;
            this.splitContainerWebbrowserSending.Panel1Collapsed = false;
            //foreach (System.Windows.Forms.Control C in this.splitContainerWebbrowserSending.Panel2.Controls)
            //    C.Dispose();
            //this.splitContainerWebbrowserSending.Panel2.Controls.Clear();
        }
        
#endregion

#endregion

#region Confirmation

#region Toolstrip

        private void toolStripButtonConfirmationOpenSchema_Click(object sender, EventArgs e)
        {
            this.setSchemaFile(this.textBoxConfirmationSchema, Transaction.TransactionCorrespondenceType.Confirmation);
            if (this.textBoxConfirmationSchema.Text.Length > 0)
            {
                DiversityCollection.Forms.FormTransactionSettings.Default.ConfirmationSchemaFile = this.textBoxConfirmationSchema.Text;
            }
        }

        private void toolStripButtonConfirmationShowList_Click(object sender, EventArgs e)
        {
            this.EnableConfirmationList(true);
        }

        private void toolStripButtonConfirmationPreview_Click(object sender, EventArgs e)
        {
            bool SchemaOK = this.XsltFileOK(this.textBoxConfirmationSchema.Text);
            if (this.textBoxConfirmationSchema.Text.Length > 0 && !SchemaOK)
                return;

            if (this.transactionBindingSource.Current != null)
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                try
                {
                    this.setListForConfirmation();
                    string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\Transaction\\Confirmation.XML";
                    System.IO.FileInfo XML = new System.IO.FileInfo(Path);

                    this._XmlSending = this._Transaction.XmlCorrespondence(
                        XML,
                        this.textBoxConfirmationSchema.Text,
                        Transaction.TransactionCorrespondenceType.Confirmation,
                        null,
                        ID,
                        this.AdministrationAgentURI(),
                        this.dateTimePickerBeginDate.Value,
                        false, false, false, false, Transaction.ConversionType.No_Conversion, Transaction.TransactionSpecimenOrder.Taxa);

                    System.Uri U = new Uri(this._XmlSending);
                    this.webBrowserConfirmation.Url = U;

                    if (SchemaOK && System.Windows.Forms.MessageBox.Show("Do you want to save the document", "Save?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        this.toolStripButtonConfirmationSave_Click(null, null);
                }
                catch { }
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void toolStripButtonConfirmationPrint_Click(object sender, EventArgs e)
        {
            this.webBrowserConfirmation.ShowPrintPreviewDialog();
        }

        private void toolStripButtonConfirmationSave_Click(object sender, EventArgs e)
        {
            this.saveDocumentEntry(this.webBrowserConfirmation, "Confirmation");
        }

        private void setListForConfirmation()
        {
            string SQL = DiversityCollection.Transaction.SqlCollectionSpecimenPartListTypeIncluded((int)this.ID, false) +
                "AND T.TransactionReturnID IS NULL " + 
                "ORDER BY DisplayText";
            //string SQL = DiversityCollection.Transaction.SqlCollectionSpecimenPartListTypeIncluded +
            //    "HAVING IsOnLoan = 1 AND CollectionSpecimenTransaction.TransactionID = " + this.ID +
            //    "ORDER BY DisplayText";
            //string SQL = DiversityCollection.Transaction.SqlCollectionSpecimenPartList +
            //    "WHERE CollectionSpecimenTransaction.TransactionID = " + this.ID.ToString() +
            //    "ORDER BY DisplayText";
            this.dataSetTransaction.CollectionSpecimenPartList.Clear();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                ad.Fill(this.dataSetTransaction.CollectionSpecimenPartList);
                //this.listBoxConfirmationSpecimenList.DataSource = this.dataSetTransaction.CollectionSpecimenPartList;
                //this.listBoxConfirmationSpecimenList.DisplayMember = "DisplayText";

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
#endregion

        //private void tabPageConfirmation_Enter(object sender, EventArgs e)
        //{
        //    //this.setListForConfirmation();
        //}

#endregion

#region Reminder

#region Toolstrip

        private void toolStripButtonReminderOpenSchema_Click(object sender, EventArgs e)
        {
            this.setSchemaFile(this.textBoxReminderSchema, Transaction.TransactionCorrespondenceType.Reminder);
            if (this.textBoxReminderSchema.Text.Length > 0)
                DiversityCollection.Forms.FormTransactionSettings.Default.ReminderSchemaFile = this.textBoxReminderSchema.Text;
        }

        private void toolStripButtonReminderShowList_Click(object sender, EventArgs e)
        {
            this.EnableReminderList(true);
        }

        private void toolStripButtonReminderPreview_Click(object sender, EventArgs e)
        {
            bool SchemaOK = this.XsltFileOK(this.textBoxReminderSchema.Text);
            if (this.textBoxReminderSchema.Text.Length > 0 && !SchemaOK)
                return;

            System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\Transaction\\Reminder.XML";
                System.IO.FileInfo XML = new System.IO.FileInfo(Path);
                string Type = R["TransactionType"].ToString();
                if (this.transactionBindingSource.Current != null)
                {
                    try
                    {
                        this._XmlSending = this._Transaction.XmlCorrespondence(
                            XML,
                            this.textBoxReminderSchema.Text,
                            Transaction.TransactionCorrespondenceType.Reminder,
                            null,
                            ID,
                            this.AdministrationAgentURI(),
                            this.dateTimePickerBeginDate.Value,
                            false, false, false, false, Transaction.ConversionType.No_Conversion,
                            Transaction.TransactionSpecimenOrder.Taxa);
                        System.Uri U = new Uri(this._XmlSending);
                        this.webBrowserReminder.Url = U;
                        if (SchemaOK && System.Windows.Forms.MessageBox.Show("Do you want to save the document", "Save?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            this.toolStripButtonReminderSave_Click(null, null);
                            //this.toolStripButtonSendingSave_Click(null, null);
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                else
                {
                    try
                    {
                        this._XmlSending = this._Transaction.XmlCorrespondence(
                            XML,
                            this.textBoxReminderSchema.Text,
                            Transaction.TransactionCorrespondenceType.Reminder,
                            null,
                            ID,
                            this.AdministrationAgentURI(),
                            this.dateTimePickerBeginDate.Value,
                            false, false, false, false, Transaction.ConversionType.No_Conversion,
                            Transaction.TransactionSpecimenOrder.Taxa);
                        System.Uri U = new Uri(this._XmlSending);
                        this.webBrowserReminder.Url = U;
                        if (SchemaOK && System.Windows.Forms.MessageBox.Show("Do you want to save the document", "Save?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            this.toolStripButtonReminderSave_Click(null, null);
                        //this.toolStripButtonSendingSave_Click(null, null);
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void toolStripButtonReminderPrint_Click(object sender, EventArgs e)
        {
            this.webBrowserReminder.ShowPrintPreviewDialog();
        }

        private void toolStripButtonReminderSave_Click(object sender, EventArgs e)
        {
            this.saveDocumentEntry(this.webBrowserReminder, "Reminder");
        }

#endregion

#endregion

#region Printing

#region Toolstrip

        private void toolStripButtonOpenSchemaFile_Click(object sender, EventArgs e)
        {
            this.setSchemaFile(this.textBoxPrintingSchemaFile, Transaction.TransactionCorrespondenceType.Printing);
        }

        private void toolStripButtonPrintingShowList_Click(object sender, EventArgs e)
        {
            this.EnablePrintingList(true);
        }

        private void toolStripButtonPrintingPreview_Click(object sender, EventArgs e)
        {
            bool SchemaOK = this.XsltFileOK(this.textBoxPrintingSchemaFile.Text);
            if (this.textBoxPrintingSchemaFile.Text.Length > 0 && !SchemaOK)
                return;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            if (this.transactionBindingSource.Current != null)
            {
                try
                {
                    //System.DateTime date = System.DateTime.Now;
                    //System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
                    //if (!R["BeginDate"].Equals(System.DBNull.Value))
                    //    System.DateTime.TryParse(R["BeginDate"].ToString(), out date);

                    System.DateTime date = System.DateTime.Now;
                    System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
                    if (!R["BeginDate"].Equals(System.DBNull.Value) && !System.DateTime.TryParse(R["BeginDate"].ToString(), out date))
                        date = System.DateTime.Now;
                    //if (!System.DateTime.TryParse(this.textBoxBeginDate.Text, out date)) date = System.DateTime.Now;

                    string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\Transaction\\Printing.XML";
                    System.IO.FileInfo XML = new System.IO.FileInfo(Path);
                    Transaction.TransactionSpecimenOrder Order = Transaction.TransactionSpecimenOrder.Taxa;
                    if (this.radioButtonPrintingGroupedByNumber.Checked)
                        Order = Transaction.TransactionSpecimenOrder.AccessionNumber;
                    else if (this.radioButtonPrintingSingleNumbers.Checked)
                        Order = Transaction.TransactionSpecimenOrder.SingleAccessionNumber;
                    this._XmlSending = this._Transaction.XmlCorrespondence(
                        XML, 
                        this.textBoxPrintingSchemaFile.Text, 
                        Transaction.TransactionCorrespondenceType.Printing, 
                        null, 
                        ID, 
                        this.AdministrationAgentURI(),
                        date,
                        this.checkBoxPrintingIncludeChildTransactions.Checked,
                        this.checkBoxPrintingIncludeSubcollections.Checked,
                        this.checkBoxIncludeTypeInformation.Checked, false, Transaction.ConversionTypeFormString(this.comboBoxPrintingConversion.Text), Order);
                    System.Uri U = new Uri(this._XmlSending);
                    this.webBrowserPrint.Url = U;

                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void toolStripButtonPrintingPrint_Click(object sender, EventArgs e)
        {
            this.webBrowserPrint.ShowPrintPreviewDialog();
        }

        private void toolStripButtonPrintingSave_Click(object sender, EventArgs e)
        {
            try
            {
                string Type = "";
                System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
                Type = R["TransactionType"].ToString();
                this.saveDocumentEntry(this.webBrowserPrint, Type);
            }
            catch (System.Exception ex) { }
        }
        
        private void checkBoxPrintingOrderByMaterial_CheckedChanged(object sender, EventArgs e)
        {
            this._Transaction.GroupByMaterial = this.checkBoxPrintingOrderByMaterial.Checked;
        }

#endregion

#region Scanner

        private void toolStripButtonScannerPrinting_Click(object sender, EventArgs e)
        {
            this.setTimerIntervall();
        }

        //private void buttonScannerPrinting_Click(object sender, EventArgs e)
        //{
        //}

        private void textBoxPrintingAccessionNumber_TextChanged(object sender, EventArgs e)
        {
            if ((this.comboBoxToCollection.SelectedIndex == -1 || this.comboBoxToCollection.Text.Length == 0) && this.comboBoxToCollection.Visible)
            {
                this.timerPrinting.Stop();
                this.textBoxPrintingAccessionNumber.Text = "";
                System.Windows.Forms.MessageBox.Show("Please select the collection of the specimen");
                return;
            }
            if (this.textBoxPrintingAccessionNumber.Text.Length > 0)
                this.startScannerPrinting();
        }

        private void textBoxPrintingAccessionNumber_MouseEnter(object sender, EventArgs e)
        {
            this.textBoxPrintingAccessionNumber.Focus();
            this.textBoxPrintingAccessionNumber.SelectAll();
        }

        private void timerPrinting_Tick(object sender, EventArgs e)
        {
            if (this.textBoxPrintingAccessionNumber.Text.Length > 0)
            {
                //int? CollectionID = null;
                //if (this.checkBoxSendingRestrictToCollection.Visible && this.checkBoxSendingRestrictToCollection.Checked)
                //{
                //    System.Data.DataRowView RV = (System.Data.DataRowView)this.comboBoxFromCollection.SelectedItem;
                //    int.TryParse(RV["CollectionID"].ToString(), out CollectionID);
                //}
                //string MaterialCategory = "";
                //if (this.checkBoxSendingRestrictToMaterial.Visible && this.checkBoxSendingRestrictToMaterial.Checked)
                //    MaterialCategory = this.comboBoxMaterialCategory.Text;

                if (!this.InsertSpecimenTransaction(this.ID, this.textBoxPrintingAccessionNumber.Text, false, null, "", false))
                {
                    this.timerPrinting.Stop();
                    System.Windows.Forms.MessageBox.Show("Either the accession number " + this.textBoxPrintingAccessionNumber.Text + " is already in the list " +
                        "\r\n\tor\r\n no dataset could be found for the accession number " + this.textBoxPrintingAccessionNumber.Text +
                        "\r\n\tor\r\n you have no access to these data ", "No data found");
                    this.textBoxPrintingAccessionNumber.Text = "";
                }
            }
            this._CurrentScanStepPrinting = ScanStep.Idle;
            this.textBoxPrintingAccessionNumber.SelectAll();
            this.timerPrinting.Stop();
        }
       
        private void startScannerPrinting()
        {
            if (this._CurrentScanStepPrinting == ScanStep.Idle)
            {
                this._CurrentScanStepPrinting = ScanStep.Scanning;
                this.timerPrinting.Start();
            }
        }

#endregion

#region Manuell handling of list items

        //private void buttonPrintingRemoveFromList_Click(object sender, EventArgs e)
        //{
        //    if (this.listBoxPrintingList.SelectedIndex > -1)
        //    {
        //        System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxPrintingList.SelectedItem;
        //        string SQL = "DELETE FROM CollectionSpecimenTransaction " +
        //            "WHERE (TransactionID = " + this.ID.ToString() + ")" +
        //            " AND CollectionSpecimenID = " + RV["CollectionSpecimenID"].ToString() +
        //            " AND SpecimenPartID = " + RV["SpecimenPartID"].ToString();
        //        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
        //        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
        //        try
        //        {
        //            con.Open();
        //            C.ExecuteNonQuery();
        //            con.Close();
        //        }
        //        catch { }
        //        RV.Delete();
        //        this.savePrintingList();
        //    }
        //}

        private void comboBoxPrintingAccessionNumber_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.comboBoxPrintingAccessionNumber.SelectedItem != null)
                {
                    System.Data.DataRowView RV = (System.Data.DataRowView)this.comboBoxPrintingAccessionNumber.SelectedItem;
                    string AccNr = RV["AccessionNumber"].ToString();
                    this.InsertSpecimenTransaction(this.ID, AccNr, true, null, "", false);
                }
            }
            catch { }
            //try
            //{
            //    if (this.comboBoxPrintingAccessionNumber.SelectedItem != null)
            //    {
            //        System.Data.DataRowView RV = (System.Data.DataRowView)this.comboBoxPrintingAccessionNumber.SelectedItem;
            //        bool OK = true;
            //        int SpecimenID = 0;
            //        int PartID = 0;
            //        string AccNr = RV["AccessionNumber"].ToString();
            //        if (!int.TryParse(RV["CollectionSpecimenID"].ToString(), out SpecimenID)) OK = false;
            //        if (!int.TryParse(RV["SpecimenPartID"].ToString(), out PartID)) OK = false;
            //        if (OK) this.insertItemForPrinting(AccNr, SpecimenID, PartID);
            //        else this.insertItemForPrinting(AccNr, null, null);
            //    }
            //}
            //catch { }
        }

        private void comboBoxPrintingAccessionNumber_DropDown(object sender, EventArgs e)
        {
            int CollectionID = this.AdministrativeCollectionID;
            //System.Data.DataRowView R = (System.Data.DataRowView)transactionBindingSource.Current;
            //if (R["AdministratingCollectionID"].Equals(System.DBNull.Value))
            //{
            //    System.Windows.Forms.MessageBox.Show("Please select an administrating collection");
            //    this.comboBoxPrintingAccessionNumber.Text = "";
            //    return;
            //}
            //else
            //{
            //    if (!int.TryParse(R["AdministratingCollectionID"].ToString(), out CollectionID))
            //        return;
            //}
            //if (this.comboBoxToCollection.SelectedIndex == -1)
            //{
            //    System.Windows.Forms.MessageBox.Show("Please select a collection");
            //    return;
            //}
            //System.Data.DataRowView RV = (System.Data.DataRowView)this.comboBoxToCollection.SelectedItem;
            //if (!int.TryParse(RV["CollectionID"].ToString(), out CollectionID))
            //{
            //    System.Windows.Forms.MessageBox.Show("Please select a collection");
            //    return;
            //}
            //string SQL = "SELECT CASE WHEN CollectionSpecimenPart.AccessionNumber IS NULL  OR CollectionSpecimenPart.AccessionNumber NOT LIKE '" + this.comboBoxPrintingAccessionNumber.Text + "%' " +
            //    "THEN CollectionSpecimen.AccessionNumber ELSE CollectionSpecimenPart.AccessionNumber END + CASE WHEN CollectionSpecimenPart.PartSublabel " +
            //    "IS NULL THEN '' ELSE ' ' + CollectionSpecimenPart.PartSublabel END AS AccessionNumber , " +
            //    "CollectionSpecimenPart.CollectionSpecimenID, CollectionSpecimenPart.SpecimenPartID " +
            //    "FROM CollectionSpecimenPart INNER JOIN " +
            //    "CollectionSpecimen ON CollectionSpecimenPart.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID " +
            //    "WHERE (CollectionSpecimenPart.AccessionNumber LIKE '" + this.comboBoxPrintingAccessionNumber.Text + "%' " +
            //    "OR CollectionSpecimen.AccessionNumber LIKE '" + this.comboBoxPrintingAccessionNumber.Text + "%') " +
            //    "AND (LEN(RTRIM(CollectionSpecimenPart.AccessionNumber)) > 0 OR LEN(RTRIM(CollectionSpecimen.AccessionNumber)) > 0) " +
            //    "AND CollectionSpecimen.CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionSpecimenID_UserAvailable) ";
            //if (this.checkBoxPrintingIncludeAnyCollection.Checked)
            //{
            //    SQL += " AND CollectionSpecimenPart.CollectionID IN (SELECT CollectionID FROM dbo.CollectionHierarchy (" + CollectionID.ToString() + ")) ";
            //}
            string SQL = "SELECT DISTINCT AccessionNumber " +
                "FROM ManagerSpecimenPartList  " +
                "WHERE AccessionNumber LIKE '" + this.comboBoxPrintingAccessionNumber.Text + "%' ";
            if (this.checkBoxPrintingIncludeSubcollections.Checked)
            {
                SQL += " AND CollectionID IN (SELECT " + CollectionID.ToString() + " UNION SELECT CollectionID FROM dbo.CollectionChildNodes (" + CollectionID.ToString() + ")) ";
            }
            else
            {
                SQL += " AND CollectionID = " + CollectionID.ToString();
            }
            SQL += " ORDER BY AccessionNumber";
            try
            {
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                this.comboBoxPrintingAccessionNumber.DataSource = dt;
                this.comboBoxPrintingAccessionNumber.DisplayMember = "AccessionNumber";
                this.comboBoxPrintingAccessionNumber.ValueMember = "AccessionNumber";

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

#endregion

#region Toolstrip List

        private void toolStripButtonSpecimenListDelete_Click(object sender, EventArgs e)
        {
            if (this.listBoxPrintingList.SelectedIndex > -1)
            {
                System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxPrintingList.SelectedItem;
                string SQL = "DELETE FROM CollectionSpecimenTransaction " +
                    "WHERE (TransactionID = " + this.ID.ToString() + ")" +
                    " AND CollectionSpecimenID = " + RV["CollectionSpecimenID"].ToString() +
                    " AND SpecimenPartID = " + RV["SpecimenPartID"].ToString();
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    C.ExecuteNonQuery();
                    con.Close();
                }
                catch { }
                RV.Delete();
                this._Transaction.saveDependentTables();
                this._Transaction.RequerySpecimenLists();
            }
        }

        private void toolStripButtonSpecimenListSearch_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxPrintingList.SelectedItem;
            int ID;
            if (int.TryParse(R["CollectionSpecimenID"].ToString(), out ID))
                this.OpenSpecimen(ID);
        }
        
#endregion

#endregion

#region Request

        private void textBoxRequestType_TextChanged(object sender, EventArgs e)
        {
            this.initForm(this.textBoxRequestType.Text);
        }

#region Toolstrip

        private void toolStripButtonRequestOpenSchema_Click(object sender, EventArgs e)
        {
            this.setSchemaFile(this.textBoxRequestSchema, Transaction.TransactionCorrespondenceType.Request);
        }

        private void toolStripButtonRequestShowList_Click(object sender, EventArgs e)
        {
            this.EnableRequestList(true);
        }
        
        private void toolStripButtonRequestPreview_Click(object sender, EventArgs e)
        {
            bool SchemaOK = this.XsltFileOK(this.textBoxRequestSchema.Text);
            if (this.textBoxRequestSchema.Text.Length > 0 && !SchemaOK)
                return;

            if (this.transactionBindingSource.Current != null)
            {
                try
                {
                    System.DateTime date = System.DateTime.Now;// ;


                    //if (!System.DateTime.TryParse(this.textBoxBeginDate.Text, out date)) date = System.DateTime.Now;

                    System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
                    if (!R["BeginDate"].Equals(System.DBNull.Value))
                        System.DateTime.TryParse(R["BeginDate"].ToString(), out date);
                    //if (!System.DateTime.TryParse(this.dateTimePickerBeginDate.Value.ToShortDateString(), out date)) date = System.DateTime.Now;

                    string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\Transaction\\Request.XML";
                    System.IO.FileInfo XML = new System.IO.FileInfo(Path);
                    this._XmlSending = this._Transaction.XmlCorrespondence(
                        XML, 
                        this.textBoxRequestSchema.Text, 
                        Transaction.TransactionCorrespondenceType.Request, 
                        null, 
                        ID, 
                        this.AdministrationAgentURI(),
                        date,
                        false,
                        false,
                        false, false, Transaction.ConversionType.No_Conversion,
                        Transaction.TransactionSpecimenOrder.Taxa);
                    System.Uri U = new Uri(XML.FullName);
                    this.webBrowserRequest.Url = U;

                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void toolStripButtonRequestPrint_Click(object sender, EventArgs e)
        {
            this.webBrowserRequest.ShowPrintPreviewDialog();
        }

        private void toolStripButtonRequestSave_Click(object sender, EventArgs e)
        {
            this.saveDocumentEntry(this.webBrowserRequest, "Request");
        }

#endregion

#region Scanner

        private void toolStripButtonRequestScanner_Click(object sender, EventArgs e)
        {
            this.setTimerIntervall();
        }

        private void textBoxRequestAccessionNumber_TextChanged(object sender, EventArgs e)
        {
            if (this.comboBoxAdministratingCollection.SelectedIndex == -1 || this.comboBoxAdministratingCollection.Text.Length == 0)
            {
                this.timerRequest.Stop();
                this.textBoxRequestAccessionNumber.Text = "";
                System.Windows.Forms.MessageBox.Show("Please select the collection of the specimen");
                return;
            }
            if (this.textBoxRequestAccessionNumber.Text.Length > 0)
                this.startScannerRequest();
        }

        private void textBoxRequestAccessionNumber_MouseEnter(object sender, EventArgs e)
        {
            this.textBoxRequestAccessionNumber.Focus();
            this.textBoxRequestAccessionNumber.SelectAll();
        }

        private void timerRequest_Tick(object sender, EventArgs e)
        {
            if (this.textBoxRequestAccessionNumber.Text.Length > 0)
            {
                if (!this.InsertSpecimenTransaction(this.ID, this.textBoxRequestAccessionNumber.Text, false, null, "", true))
                {
                    this.timerRequest.Stop();
                    System.Windows.Forms.MessageBox.Show("Either the accession number " + this.textBoxRequestAccessionNumber.Text + " is already in the list " +
                        "\r\n\tor\r\n no dataset could be found for the accession number " + this.textBoxRequestAccessionNumber.Text +
                        "\r\n\tor\r\n you have no access to these data ", "No data found");
                    this.textBoxRequestAccessionNumber.Text = "";
                }
            }
            this._CurrentScanStepRequest = ScanStep.Idle;
            this.textBoxRequestAccessionNumber.SelectAll();
            this.timerRequest.Stop();
        }

        private void startScannerRequest()
        {
            if (this._CurrentScanStepRequest == ScanStep.Idle)
            {
                this._CurrentScanStepRequest = ScanStep.Scanning;
                this.timerRequest.Start();
            }
        }

#endregion

#region Manuell handling of list items

        private void comboBoxRequestAccessionNumber_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.comboBoxRequestAccessionNumber.SelectedItem != null)
                {
                    System.Data.DataRowView RV = (System.Data.DataRowView)this.comboBoxRequestAccessionNumber.SelectedItem;
                    string AccNr = RV["AccessionNumber"].ToString();
                    this.InsertSpecimenTransaction(this.ID, AccNr, false, null, "", true);
                }
            }
            catch { }
        }

        private void comboBoxRequestAccessionNumber_DropDown(object sender, EventArgs e)
        {
            int CollectionID = this.AdministrativeCollectionID;
            string SQL = "SELECT DISTINCT AccessionNumber " +
                "FROM RequesterSpecimenPartList  " +
                "WHERE AccessionNumber LIKE '" + this.comboBoxRequestAccessionNumber.Text + "%' ";
            SQL += " ORDER BY AccessionNumber";
            try
            {
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                this.comboBoxRequestAccessionNumber.DataSource = dt;
                this.comboBoxRequestAccessionNumber.DisplayMember = "AccessionNumber";
                this.comboBoxRequestAccessionNumber.ValueMember = "AccessionNumber";

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

#endregion

#region Toolstrip List
        private void toolStripButtonRequestListDelete_Click(object sender, EventArgs e)
        {
            if (this.listBoxRequest.SelectedIndex > -1)
            {
                System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxRequest.SelectedItem;
                string SQL = "DELETE FROM CollectionSpecimenTransactionRequest " +
                    "WHERE (TransactionID = " + this.ID.ToString() + ")" +
                    " AND CollectionSpecimenID = " + RV["CollectionSpecimenID"].ToString() +
                    " AND SpecimenPartID = " + RV["SpecimenPartID"].ToString();
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    C.ExecuteNonQuery();
                    con.Close();
                }
                catch { }
                RV.Delete();
                this._Transaction.saveDependentTables();
                this._Transaction.RequerySpecimenLists();
            }
        }

        private void toolStripButtonRequestListFind_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxRequest.SelectedItem;
            int ID;
            if (int.TryParse(R["CollectionSpecimenID"].ToString(), out ID))
                this.OpenSpecimen(ID);
        }

#endregion

#endregion

#region Documents

        private void listBoxTransactionDocuments_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.transactionDocumentBindingSource.Position = this.listBoxTransactionDocuments.SelectedIndex;
            this.setDocumentControls();
        }

        private void buttonTransactionDocumentFindUri_Click(object sender, EventArgs e)
        {
            if (this.transactionDocumentBindingSource.Current == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select or create a new document");
                return;
            }
            System.Data.DataRowView R = (System.Data.DataRowView)this.transactionDocumentBindingSource.Current;
            try
            {
                if (!R["DocumentURI"].Equals(System.DBNull.Value))
                {
                    System.Windows.Forms.MessageBox.Show("There is allready a link present. Please create a new entry to insert the link");
                    return;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            string URL = "";
            try
            {
                string SQL = "SELECT TOP (1) DocumentURI " +
                    "FROM            TransactionDocument AS T " +
                    "WHERE        (DocumentURI LIKE 'http%') " +
                    "ORDER BY Date DESC";
                URL = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (URL.Length > 0)
                {
                    System.Uri U = new Uri(URL);
                    string Res = U.Segments[U.Segments.Length - 1];
                    URL = U.OriginalString.Substring(0, U.OriginalString.IndexOf(Res));
                }
                else
                    URL = "https://diversityworkbench.net/Portal/DiversityWorkbench";
            }
            catch (System.Exception ex)
            {
                URL = "https://diversityworkbench.net/Portal/DiversityWorkbench";
            }
            // System.Diagnostics.Process.Start(URL);
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(URL);
            info.UseShellExecute = true;
            System.Diagnostics.Process.Start(info);
            
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New document", "Please copy path from browser here", "");
            f.ShowInTaskbar = true;
            f.TopMost = true;
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                URL = f.String;
                if (URL.Length > 0)
                {
                    System.Uri U = new Uri(URL);
                    if (U.IsWellFormedOriginalString())
                    {
                        try
                        {
                            this.splitContainerTransactionDocumentData.Panel1Collapsed = false;
                            R.BeginEdit();
                            R["DocumentURI"] = URL;
                            R.EndEdit();
                        }
                        catch (Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("No valid URL");
                    }
                }
                else
                {
                }
            }
        }

        private void buttonTransactionDocumentsInsertDocument_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Clipboard.ContainsImage())
            {
                this.splitContainerTransactionDocumentData.Panel1Collapsed = false;
                System.Data.DataRowView R = (System.Data.DataRowView)this.transactionDocumentBindingSource.Current;
                try
                {
                    if (!R["TransactionDocument"].Equals(System.DBNull.Value))
                    {
                        System.Windows.Forms.MessageBox.Show("There is already an image present. Please create a new entry to insert the image");
                        return;
                    }
                    System.Drawing.Image I = System.Windows.Forms.Clipboard.GetImage();
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    I.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    System.Byte[] bb = (System.Byte[])ms.ToArray();
                    R.BeginEdit();
                    R["TransactionDocument"] = bb;
                    R.EndEdit();
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("No image in clipboard");
        }

        private void buttonTransactionDocumentInsertUri_Click(object sender, EventArgs e)
        {
            this.splitContainerTransactionDocumentData.Panel1Collapsed = false;
            System.Data.DataRowView R = (System.Data.DataRowView)this.transactionDocumentBindingSource.Current;
            try
            {
                if (!R["DocumentURI"].Equals(System.DBNull.Value))
                {
                    System.Windows.Forms.MessageBox.Show("There is already a link present. Please create a new entry to insert the link");
                    return;
                }
                DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser();
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    R.BeginEdit();
                    R["DocumentURI"] = f.URL;
                    R.EndEdit();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonTransactionDocumentInsertFile_Click(object sender, EventArgs e)
        {
            this.splitContainerTransactionDocumentData.Panel1Collapsed = false;
            System.Data.DataRowView R = (System.Data.DataRowView)this.transactionDocumentBindingSource.Current;
            try
            {
                if (!R["DocumentURI"].Equals(System.DBNull.Value))
                {
                    System.Windows.Forms.MessageBox.Show("There is already a file present. Please create a new entry to insert the file");
                    return;
                }
                this.openFileDialogDocument.ShowDialog();
                if (this.openFileDialogDocument.FileName.Length > 0)
                {
                    R.BeginEdit();
                    R["DocumentURI"] = this.openFileDialogDocument.FileName;
                    R.EndEdit();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void saveDocumentEntry(System.Windows.Forms.WebBrowser WebBrowser)
        {
            try
            {
                System.Data.DataRowView RL = (System.Data.DataRowView)this.transactionBindingSource.Current;
                int ID = int.Parse(RL[0].ToString());
                System.Data.DataRow R = this._Transaction.DataSet.Tables["TransactionDocument"].NewRow();
                R["TransactionID"] = ID;
                R["Date"] = System.DateTime.Now;
                R["TransactionText"] = WebBrowser.DocumentText;
                string DisplayText = System.DateTime.Now.Year.ToString()
                    + "-" + System.DateTime.Now.Month.ToString()
                    + "-" + System.DateTime.Now.Day.ToString()
                    + " " + System.DateTime.Now.Hour.ToString()
                    + ":" + System.DateTime.Now.Minute.ToString()
                    + ":" + System.DateTime.Now.Second.ToString();
                R["DisplayText"] = DisplayText;
                this._Transaction.DataSet.Tables["TransactionDocument"].Rows.Add(R);
                this.setDocumentControls();

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void saveDocumentEntry(System.Windows.Forms.WebBrowser WebBrowser, string DisplayText)
        {
            try
            {
                System.Data.DataRowView RL = (System.Data.DataRowView)this.transactionBindingSource.Current;
                int ID = int.Parse(RL[0].ToString());
                System.Data.DataRow R = this._Transaction.DataSet.Tables["TransactionDocument"].NewRow();
                R["TransactionID"] = ID;
                R["Date"] = System.DateTime.Now;
                R["TransactionText"] = WebBrowser.DocumentText;
                DisplayText += " " + System.DateTime.Now.Year.ToString()
                    + "-" + System.DateTime.Now.Month.ToString()
                    + "-" + System.DateTime.Now.Day.ToString()
                    + " " + System.DateTime.Now.Hour.ToString()
                    + ":" + System.DateTime.Now.Minute.ToString()
                    + ":" + System.DateTime.Now.Second.ToString();
                R["DisplayText"] = DisplayText;
                this._Transaction.DataSet.Tables["TransactionDocument"].Rows.Add(R);
                this.setDocumentControls();

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setDocumentControls()
        {
            try
            {
                this.buttonTransactionDocumentOpenPdf.Visible = false;
                if (this.listBoxTransactionDocuments.Items.Count > 0 && this.listBoxTransactionDocuments.SelectedIndex > -1)
                {
                    // items present and selected
                    this.splitContainerTransactionDocumentData.Enabled = true;
                    System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxTransactionDocuments.SelectedItem;
                    if ((!R["TransactionText"].Equals(System.DBNull.Value) && R["TransactionText"].ToString().Length > 0) ||
                        (!R["TransactionDocument"].Equals(System.DBNull.Value) && R["TransactionDocument"].ToString().Length > 0)  ||
                        (!R["DocumentURI"].Equals(System.DBNull.Value) && R["DocumentURI"].ToString().Length > 0))
                    {
                        this.webBrowserTransactionDocuments.Visible = true;
                        // text or image documents present
                        this.splitContainerTransactionDocumentData.Panel1Collapsed = false;
                        if (!R["TransactionText"].Equals(System.DBNull.Value) && R["TransactionText"].ToString().Length > 0)
                        {
                            // text document present
                            this.splitContainerTransactionDocuments.Panel1Collapsed = false;
                            this.webBrowserTransactionDocuments.DocumentText = R["TransactionText"].ToString();
                        }
                        else if (!R["DocumentURI"].Equals(System.DBNull.Value) && R["DocumentURI"].ToString().Length > 0)
                        {
                            System.Uri U = new Uri(R["DocumentURI"].ToString());
                            if (U.IsFile)
                            {
                                System.IO.FileInfo F = new System.IO.FileInfo(R["DocumentURI"].ToString());
                                if (F.Extension.ToLower() == ".pdf")
                                {
                                    this.buttonTransactionDocumentOpenPdf.Visible = true;
                                    this.buttonTransactionDocumentOpenPdf.Text = "      Open " + R["DocumentURI"].ToString();
                                    this.webBrowserTransactionDocuments.Url = new Uri("about:blank");
                                    this.webBrowserTransactionDocuments.Visible = false;
                                    if (DiversityCollection.Forms.FormTransactionSettings.Default.PdfInBrowserControl)
                                    {
                                        try
                                        {
                                            this.webBrowserTransactionDocuments.Navigate(R["DocumentURI"].ToString());
                                        }
                                        catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                                    }
                                }
                                else
                                {
                                    this.webBrowserTransactionDocuments.Url = U;
                                }
                            }
                            else
                            {
                                this.webBrowserTransactionDocuments.Url = U;
                            }
                        }
                        else
                        {
                            // no text document
                            this.splitContainerTransactionDocuments.Panel1Collapsed = true;
                        }
                        if (!R["TransactionDocument"].Equals(System.DBNull.Value) && R["TransactionDocument"].ToString().Length > 0)
                        {
                            // image document present
                            this.splitContainerTransactionDocuments.Panel2Collapsed = false;
                            // getting the image
                            System.Byte[] B = (System.Byte[])R["TransactionDocument"];
                            System.IO.MemoryStream ms = new System.IO.MemoryStream(B);
                            System.Drawing.Image I = System.Drawing.Image.FromStream(ms);
                            this.pictureBoxTransactionDocuments.Image = I;
                            this.buttonTransactionDocumentInsertDocument.Visible = false;
                            this.buttonTransactionDocumentInsertUri.Visible = false;
                        }
                        else
                        {
                            // no image document
                            this.splitContainerTransactionDocuments.Panel2Collapsed = true;
                            this.buttonTransactionDocumentInsertDocument.Visible = true;
                            this.buttonTransactionDocumentInsertUri.Visible = true;
                        }
                        this.tableLayoutPanelTransactionDocumentButtons.Visible = false;
                    }
                    else
                    {
                        // no documents
                        this.splitContainerTransactionDocumentData.Panel1Collapsed = true;
                        this.buttonTransactionDocumentInsertDocument.Visible = true;
                        this.buttonTransactionDocumentInsertUri.Visible = true;
                        this.tableLayoutPanelTransactionDocumentButtons.Visible = true;
                    }
                    this.splitContainerTransactionDocumentData.Panel2Collapsed = false;
                }
                else
                {
                    // no items present or none selected
                    this.splitContainerTransactionDocumentData.Enabled = false;
                    this.pictureBoxTransactionDocuments.Image = null;
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void textBoxTransactionDocumentDisplayText_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.transactionDocumentBindingSource.Current == null)
                    return;

                System.Data.DataRowView R = (System.Data.DataRowView)this.transactionDocumentBindingSource.Current;
                if (R.Row["DisplayText"].ToString() != this.textBoxTransactionDocumentDisplayText.Text)
                {
                    R.Row["DisplayText"] = this.textBoxTransactionDocumentDisplayText.Text;
                    R.BeginEdit();
                    R.EndEdit();
                    this._Transaction.saveDependentTables();
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void textBoxTransactionDocumentInternalNotes_Leave(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.transactionDocumentBindingSource.Current;
            R.BeginEdit();
            R.EndEdit();
        }

        private void comboBoxDocumentType_DropDown(object sender, EventArgs e)
        {
            string SQL = "SELECT NULL AS DocumentType ";
            if (this.comboBoxDocumentType.Text.Length > 0)
                SQL += "UNION SELECT '" + this.comboBoxDocumentType.Text + "' ";
            SQL += "UNION SELECT DISTINCT DocumentType FROM TransactionDocument WHERE RTRIM(DocumentType) <> '' ORDER BY DocumentType ";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            this.comboBoxDocumentType.DataSource = dt;
            this.comboBoxDocumentType.ValueMember = "DocumentType";
            this.comboBoxDocumentType.DisplayMember = "DocumentType";
        }

#region PDF
        
        private void checkBoxTransactionDocumentPdf_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormTransactionSettings.Default.PdfInBrowserControl = !DiversityCollection.Forms.FormTransactionSettings.Default.PdfInBrowserControl;
            this.checkBoxTransactionDocumentPdf.Checked = DiversityCollection.Forms.FormTransactionSettings.Default.PdfInBrowserControl;
        }

        private void buttonTransactionDocumentOpenPdf_Click(object sender, EventArgs e)
        {
            if (this.listBoxTransactionDocuments.SelectedItem != null)
            {
                try
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxTransactionDocuments.SelectedItem;
                    if (!R["DocumentURI"].Equals(System.DBNull.Value))
                    {
                        string URI = R["DocumentURI"].ToString();
                        if (URI.Length > 0)
                        {
                            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(URI);
                            info.UseShellExecute = true;
                            System.Diagnostics.Process.Start(info);
                        }
                    }
                }
                catch(System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
        }

#endregion

#region Toolstrip

        private void toolStripButtonTransactionDocumentNew_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView RT = (System.Data.DataRowView)this.transactionBindingSource.Current;
                System.Data.DataRow R = this._Transaction.DataSet.Tables["TransactionDocument"].NewRow();
                R["TransactionID"] = ID;
                R["Date"] = System.DateTime.Now;
                R["InternalNotes"] = " ";
                R["DisplayText"] = RT["TransactionType"].ToString() + " " + System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
                this._Transaction.DataSet.Tables["TransactionDocument"].Rows.Add(R);
                this.setDocumentControls();
                this.listBoxTransactionDocuments.SelectedIndex = this.listBoxTransactionDocuments.Items.Count - 1;

                //Sending 2014-7-8 17:44:19
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonTransactionDocumentDelete_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.transactionDocumentBindingSource.Current;
                if (R["TransactionText"].Equals(System.DBNull.Value) && R["TransactionDocument"].Equals(System.DBNull.Value))
                    R.Delete();
                else
                {
                    if (System.Windows.Forms.MessageBox.Show("Do you really want to delete this entry?", "Delete?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        R.Delete();
                    }
                }
                if (R.Row.RowState == DataRowState.Deleted || R.Row.RowState == DataRowState.Detached)
                {
                    this.webBrowserTransactionDocuments.Url = new Uri("about:blank");
                    this.setDocumentControls();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonDocumentZoomAdapt_Click(object sender, EventArgs e)
        {
            this.pictureBoxTransactionDocuments.Dock = DockStyle.Fill;
            this.pictureBoxTransactionDocuments.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void toolStripButtonDocumentZoom100Percent_Click(object sender, EventArgs e)
        {
            this.pictureBoxTransactionDocuments.Dock = DockStyle.None;
            this.pictureBoxTransactionDocuments.SizeMode = PictureBoxSizeMode.Normal;
            this.pictureBoxTransactionDocuments.Width = this.pictureBoxTransactionDocuments.Image.Width;
            this.pictureBoxTransactionDocuments.Height = this.pictureBoxTransactionDocuments.Image.Height;
        }

        private void toolStripButtonDocumentRemove_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
            R["TransactionDocument"] = System.DBNull.Value;
        }

        private void pictureBoxTransactionDocuments_DoubleClick(object sender, EventArgs e)
        {
            System.Drawing.Image I = this.pictureBoxTransactionDocuments.Image;
            DiversityWorkbench.Forms.FormPrintImage f = new DiversityWorkbench.Forms.FormPrintImage(I, this.Width, this.Height);
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
        }

#endregion

#endregion

#region Administration
        
        private System.Data.DataTable _dtAdministration;
        private System.Data.DataSet _dsAdministration;
        private System.Windows.Forms.BindingSource _bsAdministration;

        private void initAdministration()
        {
            try
            {
                this._dtAdministration = new DataTable("BalanceAdministration");
                System.Data.DataColumn dcAgent = new DataColumn("Agent", System.Type.GetType("System.String"));
                System.Data.DataColumn dcURI = new DataColumn("URI", System.Type.GetType("System.String"));
                this._dtAdministration.Columns.Add(dcAgent);
                this._dtAdministration.Columns.Add(dcURI);
                this._dsAdministration = new DataSet();
                this._dsAdministration.Tables.Add(this._dtAdministration);
                this._bsAdministration = new BindingSource(this.components);
                ((System.ComponentModel.ISupportInitialize)(this._bsAdministration)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this._dsAdministration)).BeginInit();
                this._bsAdministration.DataMember = "BalanceAdministration";
                this._bsAdministration.DataSource = this._dsAdministration;
                ((System.ComponentModel.ISupportInitialize)(this._bsAdministration)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this._dsAdministration)).EndInit();
                this.userControlModuleRelatedEntryBalanceAdministration.buttonOpenModule.Click += new System.EventHandler(this.ResetOptimizing_Click);
                DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryBalanceAdministration.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryBalanceAdministration.bindToData("BalanceAdministration", "Agent", "URI", this._bsAdministration);

                string URI = DiversityCollection.Forms.FormTransactionSettings.Default.BalanceAdministrationAgentURI;
                string Agent = "";
                if (DiversityCollection.Forms.FormTransactionSettings.Default.BalanceAdministrationAgentURI != null &&
                    DiversityCollection.Forms.FormTransactionSettings.Default.BalanceAdministrationAgentURI.Length > 0)
                {
                    Agent = A.AgentName(DiversityCollection.Forms.FormTransactionSettings.Default.BalanceAdministrationAgentURI, "Tt. Gg Ii");
                    if (Agent.Length == 0)
                        URI = "";
                }
                System.Data.DataRow R = this._dtAdministration.NewRow();
                R["Agent"] = Agent;
                R["URI"] = URI;
                this._dtAdministration.Rows.Add(R);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private string AdministrationAgentURI()
        {
            // Bugfix in case of missing data
            if (this._dtAdministration.Rows.Count > 0 && this._dtAdministration.Rows[0]["URI"].ToString().Length == 0 && this._Transaction.ID > -1)
            {
                string SQL = "SELECT C.AdministrativeContactName, C.AdministrativeContactAgentURI " +
                    " FROM [Transaction] AS T INNER JOIN" +
                    " Collection C ON T.AdministratingCollectionID = C.CollectionID" +
                    " WHERE T.TransactionID = " + this._Transaction.ID.ToString();
                System.Data.DataTable dt = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                if (dt != null && dt.Rows.Count == 1)
                {
                    this._dtAdministration.Rows[0]["Agent"] = dt.Rows[0]["AdministrativeContactName"].ToString();
                    this._dtAdministration.Rows[0]["URI"] = dt.Rows[0]["AdministrativeContactAgentURI"].ToString();
                }
            }

            if (this._dtAdministration.Rows.Count > 0 && this._dtAdministration.Rows[0]["URI"].ToString().Length > 0)
            {
                return this._dtAdministration.Rows[0]["URI"].ToString();
            }
            //else if (DiversityCollection.Forms.FormTransactionSettings.Default.BalanceAdministrationAgentURI.Length > 0)
            //    return DiversityCollection.Forms.FormTransactionSettings.Default.BalanceAdministrationAgentURI;
            else
            {
                ///ToDo: da muss noch geprüft werden warum die leer ist. Evtl. Moeglichkeit schaffen die hier zu setzen
                return DiversityCollection.Forms.FormTransactionSettings.Default.BalanceAdministrationAgentURI;
            }
        }
        
#endregion

#region Balance

        private string fillBalanceTableByPartnerAgentURI()
        {
            string ErrorMessage = "";
            if (this._DtBalance == null)
                this._DtBalance = new DataTable();
            System.Data.DataRowView RV = (System.Data.DataRowView)this.transactionBindingSource.Current;
            if (this.AdministrationAgentURI().Length == 0)
            {
                ErrorMessage = "Please set the administrative partner (Link to DiversityAgents needed)";
            }
            else if (this.AdministrationAgentURI() == RV["FromTransactionPartnerAgentURI"].ToString() ||
                this.AdministrationAgentURI() == RV["ToTransactionPartnerAgentURI"].ToString())
            {
                string ExchangePartnerURI = RV["ToTransactionPartnerAgentURI"].ToString();
                if (this.AdministrationAgentURI() == ExchangePartnerURI)
                {
                    ExchangePartnerURI = RV["FromTransactionPartnerAgentURI"].ToString();
                }
                if (this.AdministrationAgentURI().Length == 0)
                {
                    ErrorMessage = "Please set the administrative partner (Link to DiversityAgents needed)";
                }
                else if (this.AdministrationAgentURI().Length == 0 || ExchangePartnerURI.Length == 0)
                {
                    ErrorMessage = "Both partners must be available";
                }
                else if (this.AdministrationAgentURI() == ExchangePartnerURI)
                {
                    ErrorMessage = "Exchange partners must be different";
                }
                if (ErrorMessage.Length == 0)
                {
                    string SQL = "SELECT     " +
                        "'sent' AS Direction, " +
                        "TransactionTitle, ReportingCategory, MaterialDescription,  " +
                        "MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber,  " +
                        "ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, NumberOfUnits, Investigator, TransactionComment,  " +
                        "BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI " +
                        "FROM [Transaction] " +
                        "WHERE FromTransactionPartnerAgentURI = '" + this.AdministrationAgentURI() + "' AND ToTransactionPartnerAgentURI = '" + ExchangePartnerURI + "' AND TransactionType = 'exchange' " +
                        "UNION " +
                        "SELECT " +
                        "'received' AS Direction, " +
                        "TransactionTitle, ReportingCategory, MaterialDescription,  " +
                        "MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber,  " +
                        "ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, NumberOfUnits, Investigator, TransactionComment,  " +
                        "BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI " +
                        "FROM [Transaction] " +
                        "WHERE FromTransactionPartnerAgentURI = '" + ExchangePartnerURI + "' AND ToTransactionPartnerAgentURI = '" + this.AdministrationAgentURI() + "' AND TransactionType = 'exchange' ORDER BY BeginDate";
                    try
                    {
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        this._DtBalance.Clear();
                        ad.Fill(this._DtBalance);
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        ErrorMessage = ex.Message;
                    }
                }
            }
            else
            {
                ErrorMessage = "The administrative partner does not match with any of the exchange partners: " +
                    "\r\n\r\nAdministration:\r\n" + this.userControlModuleRelatedEntryBalanceAdministration.textBoxValue.Text +
                    "\r\n\r\nFrom:\r\n" + RV["FromTransactionPartnerName"].ToString() +
                    "\r\n\r\nTo:\r\n" + RV["ToTransactionPartnerName"].ToString();
            }
            return ErrorMessage;
        }

        private void createBalanceXmlOutput()
        {
            try
            {
                bool SchemaOK = this.XsltFileOK(this.textBoxBalanceSchema.Text);
                if (this.textBoxBalanceSchema.Text.Length > 0 && !SchemaOK)
                    return;

                string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\Transaction\\Balance.XML";
                System.IO.FileInfo XML = new System.IO.FileInfo(Path);
                if (this.textBoxBalanceSchema.Text.Length > 0)
                {
                    System.IO.FileInfo XSLT = new System.IO.FileInfo(this.textBoxBalanceSchema.Text);
                    this._XmlSending = this._Transaction.XmlBalance(ID, DiversityCollection.Forms.FormTransactionSettings.Default.BalanceAdministrationAgentURI, XML, XSLT, this._DtBalance);
                }
                else
                    this._XmlSending = this._Transaction.XmlBalance(ID, DiversityCollection.Forms.FormTransactionSettings.Default.BalanceAdministrationAgentURI, XML, null, this._DtBalance);
                System.Uri U = new Uri(this._XmlSending);
                this.webBrowserBalance.Url = U;

                if (SchemaOK && System.Windows.Forms.MessageBox.Show("Do you want to save the document", "Save?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    this.toolStripButtonBalanceSave_Click(null, null);

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

#region ToolStrip

        private void toolStripButtonBalanceOpenSchema_Click(object sender, EventArgs e)
        {
            this.setSchemaFile(this.textBoxBalanceSchema, Transaction.TransactionCorrespondenceType.Balance);
            if (this.textBoxBalanceSchema.Text.Length > 0)
                DiversityCollection.Forms.FormTransactionSettings.Default.BalanceSchemaFile = this.textBoxBalanceSchema.Text;
        }

        private void toolStripButtonBalanceCreatePreview_Click(object sender, EventArgs e)
        {
            string Message = this.fillBalanceTableByPartnerAgentURI();
            if (Message.Length == 0)
                this.createBalanceXmlOutput();
            else
                System.Windows.Forms.MessageBox.Show(Message);
        }

        private void toolStripButtonBalancePrint_Click(object sender, EventArgs e)
        {
            this.webBrowserBalance.ShowPrintPreviewDialog();
        }

        private void toolStripButtonBalanceSave_Click(object sender, EventArgs e)
        {
            this.saveDocumentEntry(this.webBrowserBalance, "Balance");
        }
        
#endregion

#endregion

#region Transaction RETURN
        
        private void toolStripButtonTransactionReturnOpenSchema_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");

            this.setSchemaFile(this.textBoxTransactionReturnSchema, Transaction.TransactionCorrespondenceType.Return);
            if (this.textBoxTransactionReturnSchema.Text.Length > 0)
            {
                DiversityCollection.Forms.FormTransactionSettings.Default.ReturnSchemaFile = this.textBoxTransactionReturnSchema.Text;
                this.EnableTransactionReturnList(true);
            }
            else
                this.EnableTransactionReturnList(false);
        }

        private void toolStripButtonTransactionReturnShowLists_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            this.EnableTransactionReturnList(true);
        }

        private void toolStripButtonTransactionReturnPreview_Click(object sender, EventArgs e)
        {
            bool SchemaOK = false;
            if (this.textBoxTransactionReturnSchema.Text.Length > 0)
                SchemaOK = this.XsltFileOK(this.textBoxTransactionReturnSchema.Text);
            if (this.textBoxTransactionReturnSchema.Text.Length > 0 && !SchemaOK)
                return;

            if (this.dataSetTransaction.CollectionSpecimenPartTransactionReturnList.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("No items are selected");
                return;
            }
            if (this.transactionBindingSource.Current != null)
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                try
                {
                    string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\Transaction\\Return.XML";
                    System.IO.FileInfo XML = new System.IO.FileInfo(Path);

                    Transaction.TransactionSpecimenOrder SO = Transaction.TransactionSpecimenOrder.Taxa;
                    if (this.checkBoxReturnSingleLines.Checked && this.checkBoxReturnOrderByTaxa.Checked)
                        SO = Transaction.TransactionSpecimenOrder.TaxaSingleAccessionNumber;
                    else if (this.checkBoxReturnSingleLines.Checked && !this.checkBoxReturnOrderByTaxa.Checked)
                        SO = Transaction.TransactionSpecimenOrder.SingleAccessionNumber;
                    else if (!this.checkBoxReturnSingleLines.Checked && this.checkBoxReturnOrderByTaxa.Checked)
                        SO = Transaction.TransactionSpecimenOrder.Taxa;
                    else
                        SO = Transaction.TransactionSpecimenOrder.AccessionNumber;

                    this._XmlSending = this._Transaction.XmlCorrespondence(
                        XML,
                        this.textBoxTransactionReturnSchema.Text,
                        Transaction.TransactionCorrespondenceType.Return,
                        null,
                        ID, 
                        this.AdministrationAgentURI(),
                        System.DateTime.Now,
                        false, false, this.checkBoxReturnIncludeType.Checked, false, Transaction.ConversionType.No_Conversion,
                        SO);
                    System.Uri U = new Uri(this._XmlSending);
                    this.webBrowserTransactionReturn.Url = U;

                    if (SchemaOK &&
                        System.Windows.Forms.MessageBox.Show("Do you want to save the document", "Save?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        this.toolStripButtonTransactionReturnSave_Click(null, null);

                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void toolStripButtonTransactionReturnPrint_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            this.webBrowserTransactionReturn.ShowPrintPreviewDialog();
        }

        private void toolStripButtonTransactionReturnSave_Click(object sender, EventArgs e)
        {
            this.saveDocumentEntry(this.webBrowserTransactionReturn, "Return");
        }

        private void toolStripButtonTransactionReturnSpecimen_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            string SQL = "";
            try
            {
                string ParentTransactionID = "";
                foreach (System.Object O in this.listBoxTransactionReturnOnLoan.SelectedItems)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)O;
                    if (SQL.Length > 0)
                        SQL += ", ";
                    SQL += R["SpecimenPartID"].ToString();
                    if (ParentTransactionID.Length == 0)
                        ParentTransactionID = R["TransactionID"].ToString();
                }
                //SQL = "UPDATE T SET TransactionReturnID = " + this.ID.ToString() +
                //    " FROM CollectionSpecimenTransaction AS T WHERE T.SpecimenPartID IN (" + SQL + ") " +
                //    " AND TransactionID = " + ParentTransactionID;
                SQL = "BEGIN TRY " +
                    "BEGIN TRANSACTION TR " +
                    "UPDATE T SET TransactionReturnID = " + this._Transaction.ID.ToString() + " FROM CollectionSpecimenTransaction AS T WHERE T.SpecimenPartID IN (" + SQL + ")  AND TransactionID = " + ParentTransactionID + "; " +
                    "INSERT INTO CollectionSpecimenTransaction  " +
                    "(CollectionSpecimenID, TransactionID, SpecimenPartID, AccessionNumber) " +
                    "SELECT CollectionSpecimenID, " + this._Transaction.ID.ToString() + " AS TransactionID, SpecimenPartID, AccessionNumber " +
                    "FROM CollectionSpecimenTransaction AS T " +
                    "WHERE (SpecimenPartID IN (" + SQL + ")) AND (TransactionID = " + ParentTransactionID + ") " +
                    "COMMIT TRANSACTION TR; " +
                    "END TRY " +
                    "BEGIN CATCH  " +
                    "ROLLBACK TRANSACTION TR; " +
                    "END CATCH";
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                    this._Transaction.RequerySpecimenLists();
            }
            catch (System.Exception ex)
            {
            }
        }
        
        private void toolStripButtonTransactionReturnRemove_Click(object sender, EventArgs e)
        {
            string SQL = "";
            try
            {
                System.Data.DataRow[] rr = this.dataSetTransaction.Transaction.Select("TransactionID = " + this._Transaction.ID.ToString());
                string ParentTransactionID = rr[0]["ParentTransactionID"].ToString();
                foreach (System.Object O in this.listBoxTransactionReturnReturned.SelectedItems)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)O;
                    if (SQL.Length > 0)
                        SQL += ", ";
                    SQL += R["SpecimenPartID"].ToString();
                }

                SQL = "BEGIN TRY " +
                    "BEGIN TRANSACTION TR " +
                    "UPDATE T SET TransactionReturnID = NULL  FROM CollectionSpecimenTransaction AS T WHERE T.SpecimenPartID IN (" + SQL + ")  AND TransactionID = " + ParentTransactionID + "; " +
                    "DELETE T FROM CollectionSpecimenTransaction AS T WHERE T.SpecimenPartID IN (" + SQL + ") AND TransactionID = " + this._Transaction.ID.ToString() + "; " +
                    "COMMIT TRANSACTION TR; " +
                    "END TRY " +
                    "BEGIN CATCH  " +
                    "ROLLBACK TRANSACTION TR; " +
                    "END CATCH";
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                    this._Transaction.RequerySpecimenLists();
            }
            catch (System.Exception ex)
            {
            }
        }

#region Scanner
        
        private void toolStripButtonReturnScanner_Click(object sender, EventArgs e)
        {
            this.setTimerIntervall();
        }

        private void toolStripTextBoxReturnScanner_MouseEnter(object sender, EventArgs e)
        {
            this.toolStripTextBoxReturnScanner.Focus();
            this.toolStripTextBoxReturnScanner.SelectAll();
        }

        private void toolStripTextBoxReturnScanner_TextChanged(object sender, EventArgs e)
        {
            this.startScannerReturn();
        }

        private void startScannerReturn()
        {
            if (this._CurrentScanStepReturn == ScanStep.Idle)
            {
                this._CurrentScanStepReturn = ScanStep.Scanning;
                this.timerReturn.Start();
            }
        }

        private void timerReturn_Tick(object sender, EventArgs e)
        {

            if (this.toolStripTextBoxReturnScanner.Text.Length > 0)
            {
                if (!this.setCollectionSpecimenTransactionReturnID(this.toolStripTextBoxReturnScanner.Text, (int)this._Transaction.ID))
                {
                    this.timerReturn.Stop();
                    System.Windows.Forms.MessageBox.Show("No dataset could be found for the accession number " + this.toolStripTextBoxReturnScanner.Text, "No data found");
                    this.toolStripTextBoxReturnScanner.Text = "";
                }
            }
            this._CurrentScanStepReturn = ScanStep.Idle;
            this.toolStripTextBoxReturnScanner.SelectAll();
            this.timerReturn.Stop();
        }

        private bool setCollectionSpecimenTransactionReturnID(string AccessionNumber, int TransactionReturnID)
        {
            System.Data.DataRow[] rP = this.dataSetTransaction.Transaction.Select("TransactionID = " + this._Transaction.ID.ToString());
            string ParentTransactionID = rP[0]["ParentTransactionID"].ToString();

            //System.Collections.Generic.List<System.Data.DataRow> RR = new List<DataRow>();
            string SQL = "";
            foreach (System.Data.DataRow R in this.dataSetTransaction.CollectionSpecimenPartOnLoanList.Rows)
            {
                if (R["AccessionNumber"].ToString() == AccessionNumber)
                {
                    if (SQL.Length > 0)
                        SQL += ", ";
                    SQL += R["SpecimenPartID"].ToString();
                    //RR.Add(R);
                }
            }
            SQL = "BEGIN TRY " +
                "BEGIN TRANSACTION TR " +
                "UPDATE T SET TransactionReturnID = " + this._Transaction.ID.ToString() + 
                " FROM CollectionSpecimenTransaction AS T WHERE T.SpecimenPartID IN (" + SQL + ")  AND TransactionID = " + ParentTransactionID + "; " +
                "INSERT INTO CollectionSpecimenTransaction " +
                "(CollectionSpecimenID, TransactionID, SpecimenPartID) " +
                "SELECT     CollectionSpecimenID, " + this._Transaction.ID.ToString() + ", SpecimenPartID " +
                "FROM         CollectionSpecimenTransaction T " +
                "WHERE T.SpecimenPartID IN (" + SQL + ") AND TransactionID = " + ParentTransactionID + "; " +
                "COMMIT TRANSACTION TR; " +
                "END TRY " +
                "BEGIN CATCH  " +
                "ROLLBACK TRANSACTION TR; " +
                "END CATCH";
            try
            {
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                    this._Transaction.RequerySpecimenLists();
            }
            catch(System.Exception ex)
            { return false; }


            //SQL = " SELECT CollectionSpecimenTransaction.CollectionSpecimenID, CollectionSpecimenTransaction.TransactionID, " +
            //    "CollectionSpecimenTransaction.SpecimenPartID, CollectionSpecimenTransaction.IsOnLoan " +
            //    "FROM CollectionSpecimenTransaction INNER JOIN " +
            //    "CollectionSpecimenPart " +
            //    "ON CollectionSpecimenTransaction.CollectionSpecimenID = CollectionSpecimenPart.CollectionSpecimenID " +
            //    "AND CollectionSpecimenTransaction.SpecimenPartID = CollectionSpecimenPart.SpecimenPartID " +
            //    "INNER JOIN CollectionSpecimen " +
            //    "ON CollectionSpecimenPart.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID " +
            //    "WHERE (CollectionSpecimen.AccessionNumber = N'" + AccessionNumber + "' " +
            //    "OR CollectionSpecimenPart.AccessionNumber = N'" + AccessionNumber + "') " +
            //    "AND CollectionSpecimenTransaction.TransactionReturnID IS NULL ";

            //System.Data.DataTable dt = new DataTable();
            //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //ad.Fill(dt);
            //if (dt.Rows.Count == 1)
            //{
            //    SQL = "CollectionSpecimenID = " + dt.Rows[0]["CollectionSpecimenID"].ToString() +
            //          " AND SpecimenPartID = " + dt.Rows[0]["SpecimenPartID"].ToString() +
            //          " AND TransactionID = " + dt.Rows[0]["TransactionID"].ToString() +
            //          " AND TransactionReturnID <> " + dt.Rows[0]["TransactionReturnID"].ToString();
            //    System.Data.DataRow[] rr = this.dataSetTransaction.Tables["CollectionSpecimenTransaction"].Select(SQL);
            //    rr[0]["TransactionReturnID"] = TransactionReturnID;
            //}
            //else if (dt.Rows.Count > 1)
            //{
            //    DiversityCollection.Forms.FormTransactionChooseParts f = new FormTransactionChooseParts(this.dataSetTransaction.Tables["CollectionSpecimenTransaction"]);
            //    f.ShowDialog();
            //    if (f.DialogResult == DialogResult.OK)
            //    {
            //        foreach (System.Data.DataRow R in f.CollectionSpecimenPart.Rows)
            //        {
            //            SQL = "CollectionSpecimenID = " + dt.Rows[0]["CollectionSpecimenID"].ToString() +
            //                  " AND SpecimenPartID = " + dt.Rows[0]["SpecimenPartID"].ToString() +
            //                  " AND TransactionID = " + dt.Rows[0]["TransactionID"].ToString() +
            //                  " AND TransactionReturnID <> " + dt.Rows[0]["TransactionReturnID"].ToString();
            //            System.Data.DataRow[] rr = this.dataSetTransaction.Tables["CollectionSpecimenTransaction"].Select(SQL);
            //            rr[0]["TransactionReturnID"] = TransactionReturnID;
            //        }
            //    }
            //}
            //else
            //{
            //    System.Windows.Forms.MessageBox.Show("");
            //    return false;
            //}
            //this._Transaction.saveDependentTables();
            //this._Transaction.RequerySpecimenLists();
            return true;
        }

#endregion

#endregion

#region Forwarding
        /*
         * Parts werden aus altem Loan ausgetragen (IsOnLoan = false) und in neues Forwarding eingetragen
         * Forwarding wird dann gleich behandelt wie Loan
         * */
        
        private void fillForwardingTable()
        {
            if (this._DtBalance == null) this._DtBalance = new DataTable();
            int FromCollectionID;
            int ToCollectionID;
            bool FromAnyCollections = false;
            bool FromChildCollections = false;
            bool ToAnyCollections = false;
            bool ToChildCollections = false;
            if (this.checkBoxBalanceFromIncludeAllCollections.Checked) FromAnyCollections = true;
            if (this.checkBoxBalanceIncludeFromSubcollections.Checked) FromChildCollections = true;
            if (this.checkBoxBalanceIncludeToSubcollections.Checked) ToChildCollections = true;
            if (this.checkBoxBalanceToIncludeAllCollections.Checked) ToAnyCollections = true;
            System.Data.DataRowView RV = (System.Data.DataRowView)this.transactionBindingSource.Current;
            if (!int.TryParse(RV["FromCollectionID"].ToString(), out FromCollectionID))
                return;
            if (!int.TryParse(RV["ToCollectionID"].ToString(), out ToCollectionID))
                return;
            string SQL = "SELECT     " +
                "'sent' AS Direction, " +
                "TransactionTitle, ReportingCategory, MaterialDescription,  " +
                "MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber,  " +
                "ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, NumberOfUnits, Investigator, TransactionComment,  " +
                "BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI " +
                "FROM [Transaction] " +
                "WHERE FromCollectionID IN (";
            if (FromAnyCollections)
            {
                SQL += "SELECT CollectionID " +
                "FROM dbo.CollectionHierarchy(" + FromCollectionID.ToString() + ") ";
            }
            else if (FromChildCollections)
            {
                SQL += "SELECT " + FromCollectionID.ToString() + " UNION SELECT CollectionID " +
                "FROM dbo.CollectionChildNodes(" + FromCollectionID.ToString() + ") ";
            }
            else
            {
                SQL += " " + FromCollectionID.ToString() + " ";
            }
            SQL += ") AND ToCollectionID IN (";
            if (ToAnyCollections)
            {
                SQL += "SELECT CollectionID " +
                "FROM dbo.CollectionHierarchy(" + ToCollectionID.ToString() + ") ";
            }
            else if (ToChildCollections)
            {
                SQL += "SELECT " + ToCollectionID.ToString() + " UNION SELECT CollectionID " +
                "FROM dbo.CollectionChildNodes(" + ToCollectionID.ToString() + ") ";
            }
            else
            {
                SQL += " " + ToCollectionID.ToString() + " ";
            }
            SQL += ") AND TransactionType = 'exchange' " +
                "UNION " +
                "SELECT " +
                "'received' AS Direction, " +
                "TransactionTitle, ReportingCategory, MaterialDescription,  " +
                "MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber,  " +
                "ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, NumberOfUnits, Investigator, TransactionComment,  " +
                "BeginDate, AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI " +
                "FROM [Transaction] " +
                "WHERE FromCollectionID IN (";
            if (ToAnyCollections)
            {
                SQL += "SELECT CollectionID " +
                "FROM dbo.CollectionHierarchy(" + ToCollectionID.ToString() + ") ";
            }
            else if (ToChildCollections)
            {
                SQL += "SELECT " + ToCollectionID.ToString() + " UNION SELECT CollectionID " +
                "FROM dbo.CollectionChildNodes(" + ToCollectionID.ToString() + ") ";
            }
            else
            {
                SQL += " " + ToCollectionID.ToString() + " ";
            }
            SQL += ") AND ToCollectionID IN (";
            if (ToAnyCollections)
            {
                SQL += "SELECT CollectionID " +
                "FROM dbo.CollectionHierarchy(" + FromCollectionID.ToString() + ") ";
            }
            else if (ToChildCollections)
            {
                SQL += "SELECT " + FromCollectionID.ToString() + " UNION SELECT CollectionID " +
                "FROM dbo.CollectionChildNodes(" + FromCollectionID.ToString() + ") ";
            }
            else
            {
                SQL += " " + FromCollectionID.ToString() + " ";
            }
            SQL += ") AND TransactionType = 'exchange' ORDER BY BeginDate";
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                this._DtBalance.Clear();
                ad.Fill(this._DtBalance);

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonForwardingOpenSchema_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");

            this.setSchemaFile(this.textBoxForwardingSchema, Transaction.TransactionCorrespondenceType.Forwarding);
            if (this.textBoxForwardingSchema.Text.Length > 0)
            {
                DiversityCollection.Forms.FormTransactionSettings.Default.ForwardingSchemaFile = this.textBoxForwardingSchema.Text;
                //this.EnableTransactionReturnList(true);
            }
            else
            {
                //this.EnableTransactionReturnList(false);
            }
        }

        private void toolStripButtonForwardingShowLists_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            this.EnableForwardingList(true);
        }

        private void toolStripButtonForwardingPreview_Click(object sender, EventArgs e)
        {
            bool SchemaOK = this.XsltFileOK(this.textBoxForwardingSchema.Text);
            if (this.textBoxForwardingSchema.Text.Length > 0 && !SchemaOK)
                return;

            if (this.dataSetTransaction.CollectionSpecimenPartForwardingList.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("No items are selected");
                return;
            }
            if (this.transactionBindingSource.Current != null)
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                try
                {
                    string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\Transaction\\Forwarding.XML";
                    System.IO.FileInfo XML = new System.IO.FileInfo(Path);

                    Transaction.TransactionSpecimenOrder SO = Transaction.TransactionSpecimenOrder.Taxa;
                    if (this.checkBoxForwardingSingleLines.Checked && this.checkBoxForwardingOrderByTaxa.Checked)
                        SO = Transaction.TransactionSpecimenOrder.TaxaSingleAccessionNumber;
                    else if (this.checkBoxForwardingSingleLines.Checked && !this.checkBoxForwardingOrderByTaxa.Checked)
                        SO = Transaction.TransactionSpecimenOrder.SingleAccessionNumber;
                    else if (!this.checkBoxForwardingSingleLines.Checked && this.checkBoxForwardingOrderByTaxa.Checked)
                        SO = Transaction.TransactionSpecimenOrder.Taxa;
                    else
                        SO = Transaction.TransactionSpecimenOrder.AccessionNumber;

                    this._XmlSending = this._Transaction.XmlCorrespondence(
                        XML,
                        this.textBoxForwardingSchema.Text,
                        Transaction.TransactionCorrespondenceType.Forwarding,
                        null,
                        ID, 
                        this.AdministrationAgentURI(),
                        System.DateTime.Now,
                        false, false, this.checkBoxForwardingIncludeType.Checked, false, Transaction.ConversionType.No_Conversion,
                        SO);
                    System.Uri U = new Uri(this._XmlSending);
                    this.webBrowserForwarding.Url = U;

                    if (SchemaOK && System.Windows.Forms.MessageBox.Show("Do you want to save the document", "Save?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        this.toolStripButtonForwardingSave_Click(null, null);

                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void toolStripButtonForwardingPrint_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            string Type = "";
            System.Data.DataRow[] rr = this._Transaction.DataSet.Tables["Transaction"].Select("TransactionID = " + this._Transaction.ID.ToString());
            if (rr.Length > 0)
            {
                Type = rr[0]["TransactionType"].ToString();
            }
            if (this._Transaction.DataSet.Tables["CollectionSpecimenPartList"] == null || this._Transaction.DataSet.Tables["CollectionSpecimenPartList"].Rows.Count == 0)
            {
                if (Type == TransactionType.loan.ToString() ||
                    Type == TransactionType.forwarding.ToString())
                {
                    System.Windows.Forms.MessageBox.Show("No specimen selected");
                    return;
                }
            }
            if (this.webBrowserForwarding.DocumentText.Length < 15)
            {
                System.Windows.Forms.MessageBox.Show("No document created");
                return;
            }
            this.webBrowserForwarding.ShowPrintPreviewDialog();
        }

        private void toolStripButtonForwardingSave_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            string Type = "";
            System.Data.DataRow[] rr = this._Transaction.DataSet.Tables["Transaction"].Select("TransactionID = " + this._Transaction.ID.ToString());
            if (rr.Length > 0)
            {
                Type = rr[0]["TransactionType"].ToString();
            }
            if (Type == TransactionType.loan.ToString() ||
                Type == TransactionType.forwarding.ToString())
            {
                if (this._Transaction.DataSet.Tables["CollectionSpecimenPartList"] == null || this._Transaction.DataSet.Tables["CollectionSpecimenPartList"].Rows.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("No specimen selected");
                    return;
                }
            }
            if (this.webBrowserForwarding.DocumentText.Length < 15)
            {
                System.Windows.Forms.MessageBox.Show("No document created");
                return;
            }
            this.saveDocumentEntry(this.webBrowserForwarding, "Forwarding");
        }

        private void toolStripButtonForwardingForwarded_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            //return;

            string SQL = "";
            try
            {
                string ParentTransactionID = "";
                foreach (System.Object O in this.listBoxForwardingSpecimenOnLoan.SelectedItems)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)O;
                    if (SQL.Length > 0)
                        SQL += ", ";
                    SQL += R["SpecimenPartID"].ToString();
                    if (ParentTransactionID.Length == 0)
                        ParentTransactionID = R["TransactionID"].ToString();
                }
                SQL = "BEGIN TRY " +
                    "BEGIN TRANSACTION TR " +
                    "UPDATE T SET TransactionReturnID = " + this._Transaction.ID.ToString() + " FROM CollectionSpecimenTransaction AS T WHERE T.SpecimenPartID IN (" + SQL + ")  AND TransactionID = " + ParentTransactionID + "; " +
                    "INSERT INTO CollectionSpecimenTransaction  " +
                    "(CollectionSpecimenID, TransactionID, SpecimenPartID, AccessionNumber) " +
                    "SELECT CollectionSpecimenID, " + this._Transaction.ID.ToString() + " AS TransactionID, SpecimenPartID, AccessionNumber " +
                    "FROM CollectionSpecimenTransaction AS T " +
                    "WHERE (SpecimenPartID IN (" + SQL + ")) AND (TransactionID = " + ParentTransactionID + ") " +
                    "COMMIT TRANSACTION TR; " +
                    "END TRY " +
                    "BEGIN CATCH  " +
                    "ROLLBACK TRANSACTION TR; " +
                    "END CATCH";
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                    this._Transaction.RequerySpecimenLists();
            }
            catch (System.Exception ex)
            {
            }
        }
        
        private void toolStripButtonForwardingRemove_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            //return;

            string SQL = "";
            try
            {
                System.Data.DataRow[] rr = this.dataSetTransaction.Transaction.Select("TransactionID = " + this._Transaction.ID.ToString());
                string ParentTransactionID = rr[0]["ParentTransactionID"].ToString();
                foreach (System.Object O in this.listBoxForwardingSpecimenForwarded.SelectedItems)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)O;
                    if (SQL.Length > 0)
                        SQL += ", ";
                    SQL += R["SpecimenPartID"].ToString();
                }

                SQL = "BEGIN TRY " +
                    "BEGIN TRANSACTION TR " +
                    "UPDATE T SET TransactionReturnID = NULL  FROM CollectionSpecimenTransaction AS T WHERE T.SpecimenPartID IN (" + SQL + ")  AND TransactionID = " + ParentTransactionID + "; " +
                    "DELETE T FROM CollectionSpecimenTransaction AS T WHERE T.SpecimenPartID IN (" + SQL + ") AND TransactionID = " + this._Transaction.ID.ToString() + "; " +
                    "COMMIT TRANSACTION TR; " +
                    "END TRY " +
                    "BEGIN CATCH  " +
                    "ROLLBACK TRANSACTION TR; " +
                    "END CATCH";
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                    this._Transaction.RequerySpecimenLists();
            }
            catch (System.Exception ex)
            {
            }


            //string SQL = "";
            //try
            //{
            //    string ParentTransactionID = "";
            //    foreach (System.Object O in this.listBoxForwardingSpecimenForwarded.SelectedItems)
            //    {
            //        System.Data.DataRowView R = (System.Data.DataRowView)O;
            //        if (SQL.Length > 0)
            //            SQL += ", ";
            //        SQL += R["SpecimenPartID"].ToString();
            //        if (ParentTransactionID.Length == 0)
            //            ParentTransactionID = R["TransactionID"].ToString();
            //    }
            //    string SqlDel = "DELETE T " +
            //        "FROM CollectionSpecimenTransaction AS T WHERE  T.SpecimenPartID IN (" + SQL + ") " +
            //        " AND TransactionID = " + this.ID.ToString();
            //    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SqlDel))
            //    {
            //        SQL = "UPDATE T SET IsOnLoan = 1 " +
            //        " FROM CollectionSpecimenTransaction AS T WHERE T.SpecimenPartID IN (" + SQL + ") " +
            //        " AND TransactionID = " + ParentTransactionID;
            //        if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
            //            this._Transaction.RequerySpecimenLists();
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //}
        }

#endregion

#region Auxilliary functions

        private bool XsltFileOK(string Path)
        {
            if (Path.Length == 0)
                return false;
            bool OK = true;
            try
            {
                System.IO.FileInfo FI = new System.IO.FileInfo(Path);
                if (!FI.Exists)
                {
                    System.Windows.Forms.MessageBox.Show("Please select a valid schema file");
                    OK = false;
                }
                else
                {
                    if (FI.Extension.ToLower() != ".xslt")
                    {
                        System.Windows.Forms.MessageBox.Show("Please select a valid schema file");
                        OK = false;
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        private bool InsertSpecimenTransaction(int TransactionID, string AccessionNumber, bool IsOnLoan, int? CollectionID, string MaterialCategory, bool AsRequest)
        {
            int? CollID = null;
            string MatCat = "";
            string SQL = "SELECT AccessionNumber, CollectionSpecimenID, SpecimenPartID, AccessionNumberSpecimen, " +
                "AccessionNumberPart, MaterialCategory, CollectionID ";
            if (AsRequest) SQL += "FROM RequesterSpecimenPartList";
            else SQL += "FROM ManagerSpecimenPartList";
            SQL += " WHERE AccessionNumber = '" + AccessionNumber + "' ";
            if (CollectionID != null)
            {
                    SQL += " AND CollectionID = " + CollectionID.ToString();
                    CollID = CollectionID;
            }

            if (MaterialCategory.Length > 0)
            {
                SQL += " AND MaterialCategory = N'" + MaterialCategory + "'";
                MatCat = MaterialCategory;
            }

            SQL += " ORDER BY AccessionNumber";
            try
            {
                this.timerSending.Stop();
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    if (AsRequest)
                    {
                        string SQLInsert = "INSERT INTO CollectionSpecimenTransactionRequest " +
                            " (CollectionSpecimenID, TransactionID, SpecimenPartID, IsOnLoan) " +
                            " VALUES (" + dt.Rows[0]["CollectionSpecimenID"].ToString() +
                            ", " + dt.Rows[0]["SpecimenPartID"].ToString() +
                            ", " + TransactionID.ToString() +
                            ", 0) ";
                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQLInsert, con);
                        con.Open();
                        C.ExecuteNonQuery();
                        con.Close();
                    }
                    System.Data.DataRow R = this._Transaction.DataSet.Tables["CollectionSpecimenTransaction"].NewRow();
                    R["CollectionSpecimenID"] = dt.Rows[0]["CollectionSpecimenID"];
                    R["SpecimenPartID"] = dt.Rows[0]["SpecimenPartID"];
                    R["TransactionID"] = TransactionID;
                    R["IsOnLoan"] = IsOnLoan;
                    System.Data.DataRow[] rr = this._Transaction.DataSet.Tables["CollectionSpecimenTransaction"].Select("CollectionSpecimenID = " + R["CollectionSpecimenID"].ToString() +
                        " AND SpecimenPartID = " + R["SpecimenPartID"].ToString() +
                        " AND TransactionID = " + R["TransactionID"].ToString());
                    if (rr.Length == 0)
                        this._Transaction.DataSet.Tables["CollectionSpecimenTransaction"].Rows.Add(R);
                    else
                        System.Windows.Forms.MessageBox.Show("This item is allready in the list");
                }
                else if (dt.Rows.Count > 1)
                {
                    DiversityCollection.Forms.FormTransactionChooseParts f = new FormTransactionChooseParts(AccessionNumber, CollID, MatCat);
                    f.TopMost = true;
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        foreach (System.Data.DataRow R in f.CollectionSpecimenPart.Rows)
                        {
                            if (AsRequest)
                            {
                                string SQLInsert = "INSERT INTO CollectionSpecimenTransactionRequest " +
                                   " (CollectionSpecimenID, TransactionID, SpecimenPartID, IsOnLoan) " +
                                   " VALUES (" + R["CollectionSpecimenID"].ToString() +
                                   ", " + R["SpecimenPartID"].ToString() +
                                   ", " + TransactionID.ToString() +
                                   ", 0) ";
                                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQLInsert, con);
                                con.Open();
                                C.ExecuteNonQuery();
                                con.Close();
                            }
                            System.Data.DataRow RN = this._Transaction.DataSet.Tables["CollectionSpecimenTransaction"].NewRow();
                            RN["CollectionSpecimenID"] = R["CollectionSpecimenID"];
                            RN["SpecimenPartID"] = R["SpecimenPartID"];
                            RN["TransactionID"] = TransactionID;
                            RN["IsOnLoan"] = IsOnLoan;
                            this._Transaction.DataSet.Tables["CollectionSpecimenTransaction"].Rows.Add(RN);
                        }
                    }
                    else return false;
                    this.timerSending.Start();
                }
                else
                {
                    bool ReasonUnclear = true;
                    this.timerPrinting.Stop();
                    SQL = "SELECT P.MaterialCategory, P.CollectionID, M.CollectionName " +
                        "FROM CollectionSpecimenPart AS P INNER JOIN " +
                        "CollectionSpecimen AS S ON P.CollectionSpecimenID = S.CollectionSpecimenID LEFT OUTER JOIN " +
                        "dbo.ManagerCollectionList() AS M ON P.CollectionID = M.CollectionID " +
                        "WHERE (P.AccessionNumber = N'" + AccessionNumber + "') OR " +
                        "(S.AccessionNumber = N'" + AccessionNumber + "') ";
                    dt.Rows.Clear();
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["CollectionID"].ToString() != CollID.ToString())
                        {
                            string Message = "The collection where the specimen is located is " + dt.Rows[0]["CollectionName"].ToString();
                            System.Windows.Forms.MessageBox.Show("Error", Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            ReasonUnclear = false;
                        }
                        else if (MaterialCategory.Length > 0 && dt.Rows[0]["MaterialCategory"].ToString() != MaterialCategory)
                        {
                            string Message = "The material of the specimen is " + dt.Rows[0]["MaterialCategory"].ToString();
                            System.Windows.Forms.MessageBox.Show("Error", Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            ReasonUnclear = false;
                        }
                    }
                    else
                    {
                    }
                    if (ReasonUnclear)
                        System.Windows.Forms.MessageBox.Show("No datasets correspond to the criteria");
                    return false;
                }
            }
            catch (System.Exception ex) { return false; }
            finally { this.timerSending.Start(); }
            if(!AsRequest) 
                this._Transaction.saveDependentTables();
            this._Transaction.RequerySpecimenLists();
            return true;
        }

        /// <summary>
        /// Setting the timer intervall for the Scanner
        /// </summary>
        private void setTimerIntervall()
        {
            DiversityWorkbench.Forms.FormGetInteger f = new DiversityWorkbench.Forms.FormGetInteger(DiversityCollection.Forms.FormTransactionSettings.Default.TimerIntervall,
                "Timer intervall", "Please give a value for the timer intervall of the scanner in milliseconds");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                if (f.Integer != null)
                {
                    int i =  (int)f.Integer;
                    this.timerSending.Interval = i;
                    this.timerReturn.Interval = i;
                    this.timerPrinting.Interval = i;
                    DiversityCollection.Forms.FormTransactionSettings.Default.TimerIntervall = i;
                }
            }
        }

        /// <summary>
        /// setting the schema file for the documents
        /// </summary>
        /// <param name="TextBox"></param>
        private void setSchemaFile(System.Windows.Forms.TextBox TextBox, Transaction.TransactionCorrespondenceType Type)
        {
            string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\Transaction\\Schemas\\" + Type.ToString();
            if (!System.IO.File.Exists(Path))
            {
                System.IO.Directory.CreateDirectory(Path);
            }
            string[] Schemas = System.IO.Directory.GetFiles(Path, "*.xslt");
            if (Schemas.Length == 0)
            {
                Path = Path + "\\Transaction.xslt";
                if (!System.IO.File.Exists(Path))
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(Path);
                    this._Transaction.writeDefaultXslt(f);
                }
            }
            this.openFileDialogSendingSchema = new OpenFileDialog();
            this.openFileDialogSendingSchema.RestoreDirectory = true;
            this.openFileDialogSendingSchema.Multiselect = false;
            this.openFileDialogSendingSchema.InitialDirectory = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\Transaction\\Schemas\\" + Type.ToString();
            this.openFileDialogSendingSchema.Filter = "XSLT Files|*.xslt";
            try
            {
                this.openFileDialogSendingSchema.ShowDialog();
                if (this.openFileDialogSendingSchema.FileName.Length > 0)
                {
                    TextBox.Tag = this.openFileDialogSendingSchema.FileName;
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialogSendingSchema.FileName);
                    TextBox.Text = f.FullName;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


#region Setting the title

        private void comboBoxFromCollection_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.setTitle();
        }

        private void comboBoxToCollection_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.setTitle();
        }

        private void textBoxBeginDate_TextChanged(object sender, EventArgs e)
        {
            this.setTitle();
        }

        private void textBoxAgreedEndDate_TextChanged(object sender, EventArgs e)
        {
            this.setTitle();
        }

        private void textBoxFromTransactionNumber_TextChanged(object sender, EventArgs e)
        {
            this.setTitle();
        }

        private void setTitle()
        {
            try
            {
                //if (this.checkBoxAutoTitle.Checked)
                //{
                //    string Title = this.comboBoxTransactionType.Text;
                //    if (this.textBoxFromTransactionNumber.Text.Length > 0) Title += " " + this.textBoxFromTransactionNumber.Text;
                //    if (this.comboBoxFromCollection.Text.Length > 0) Title += " from " + this.comboBoxFromCollection.Text;
                //    if (this.comboBoxToCollection.Text.Length == 0) Title += " to " + this.comboBoxToCollection.Text;
                //    System.DateTime date = System.DateTime.Now;

                //    System.Data.DataRowView R = (System.Data.DataRowView)this.transactionBindingSource.Current;
                //    if (!R["BeginDate"].Equals(System.DBNull.Value) || !R["AgreedEndDate"].Equals(System.DBNull.Value))
                //    {
                //        Title += " (";
                //        if (!R["BeginDate"].Equals(System.DBNull.Value)) Title += this.dateTimePickerBeginDate.Text;
                //        if (!R["AgreedEndDate"].Equals(System.DBNull.Value)) Title += this.dateTimePickerAgreedEndDate.Text;
                //        Title += ")";
                //    }
                //    if (this.textBoxTransactionTitle.Text != Title) this.textBoxTransactionTitle.Text = Title;
                //}
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

#endregion

        private void OpenSpecimen(int CollectionSpecimenID)
        {
            try
            {
                DiversityCollection.Forms.FormCollectionSpecimen f = new Forms.FormCollectionSpecimen(CollectionSpecimenID, false, false, Forms.FormCollectionSpecimen.ViewMode.SingleInspectionMode);
                f.Width = this.Width - 20;
                f.Height = this.Height - 20;
                
                f.ShowDialog();
            }
            catch { }
        }

#region Enabling lists
        
        private void EnableRequestList(bool enable)
        {
            this.toolStripButtonRequestPreview.Visible = enable;
            this.toolStripButtonRequestPrint.Visible = enable;
            this.toolStripButtonRequestSave.Visible = enable;

            this.splitContainerRequest.Panel2.Enabled = enable;
        }

        private void EnablePrintingList(bool enable)
        {
            this.toolStripButtonPrintingSave.Visible = enable;
            this.toolStripButtonPrintingPrint.Visible = enable;
            this.toolStripButtonPrintingPreview.Visible = enable;
            this.toolStripButtonScannerPrinting.Visible = enable;

            this.splitContainerPrinting.Panel2.Enabled = enable;
        }

        private void EnableReminderList(bool enable)
        {
            this.toolStripButtonReminderPreview.Visible = enable;
            this.toolStripButtonReminderPrint.Visible = enable;
            this.toolStripButtonReminderSave.Visible = enable;

            this.splitContainerReminder.Panel2.Enabled = enable;
        }

        private void EnableSendingList(bool enable)
        {
            this.toolStripButtonSendingPreview.Visible = enable;
            this.toolStripButtonSendingPrint.Visible = enable;
            this.toolStripButtonSendingSave.Visible = enable;
            this.toolStripButtonSendingScanner.Visible = enable;

            this.splitContainerSending.Enabled = enable;
        }

        private void EnableConfirmationList(bool enable)
        {
            this.toolStripButtonConfirmationPreview.Visible = enable;
            this.toolStripButtonConfirmationPrint.Visible = enable;
            this.toolStripButtonConfirmationSave.Visible = enable;

            this.splitContainerConfirmation.Panel2.Enabled = enable;
        }

        private void EnableTransactionReturnList(bool enable)
        {
            this.toolStripButtonTransactionReturnPreview.Visible = enable;
            this.toolStripButtonTransactionReturnPrint.Visible = enable;
            this.toolStripButtonTransactionReturnSave.Visible = enable;
            if (enable)
            {
                this.toolStripButtonTransactionReturnPreview.Enabled = enable;
                this.toolStripButtonTransactionReturnPrint.Enabled = enable;
                this.toolStripButtonTransactionReturnSave.Enabled = enable;
            }

            this.splitContainerTransactionReturn.Panel2.Enabled = enable;
        }

        private void EnableForwardingList(bool enable)
        {
            this.toolStripButtonForwardingPreview.Visible = enable;
            this.toolStripButtonForwardingPrint.Visible = enable;
            this.toolStripButtonForwardingSave.Visible = enable;

            this.splitContainerForwarding.Panel2.Enabled = enable;
        }
        
#endregion

#endregion

#region History

        private void buttonHistory_Click(object sender, EventArgs e)
        {
            // Markus, 25.07.2018: Exclusion of non accessible IDs
            if (this._Transaction.NonAccessibleIDs.Contains(this.ID))
                return;

            string Title = "History of " + this.dataSetTransaction.Transaction.Rows[0]["TransactionTitle"].ToString() + " (TransactionID: " + this.ID.ToString() + ")";
            try
            {
                bool HistoryPresent = false;
                System.Collections.Generic.List<System.Data.DataTable> LogTables = new List<DataTable>();

                if (this.dataSetTransaction.Transaction.Rows.Count > 0)
                {
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "TransactionID", this.dataSetTransaction.Transaction.TableName, ""));
                    HistoryPresent = true;
                }
                if (this.dataSetTransaction.TransactionAgent.Rows.Count > 0)
                {
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "TransactionID", this.dataSetTransaction.TransactionAgent.TableName, ""));
                    HistoryPresent = true;
                }
                if (this.dataSetTransaction.TransactionDocument.Rows.Count > 0)
                {
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "TransactionID", this.dataSetTransaction.TransactionDocument.TableName, ""));
                    HistoryPresent = true;
                }
                if (this.dataSetTransaction.TransactionPayment.Rows.Count > 0)
                {
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "TransactionID", this.dataSetTransaction.TransactionPayment.TableName, ""));
                    HistoryPresent = true;
                }
                if (this.dataSetTransaction.ExternalIdentifier.Rows.Count > 0)
                {
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "ReferencedID", "ReferencedTable = 'Transaction'", this.dataSetTransaction.ExternalIdentifier.TableName, ""));
                    HistoryPresent = true;
                }
                if (this.dataSetTransaction.CollectionSpecimenTransaction.Rows.Count > 0)
                {
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "TransactionID", this.dataSetTransaction.CollectionSpecimenTransaction.TableName, ""));
                }
                if (HistoryPresent)
                {
                    DiversityWorkbench.Forms.FormHistory f = new DiversityWorkbench.Forms.FormHistory(Title, LogTables, DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace());
                    f.setHelpProviderNameSpace(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace(), "History");
                    f.ShowDialog();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("No history data found");
                }

            }

            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
#endregion

#region Payment

#region List and toolstrip

        private void toolStripButtonPaymentAdd_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRow R = this._Transaction.DataSet.Tables["TransactionPayment"].NewRow();
                R["TransactionID"] = ID;
                R["PaymentDate"] = System.DateTime.Now;
                R["Identifier"] = "New payment";
                this._Transaction.DataSet.Tables["TransactionPayment"].Rows.Add(R);
                //this.setDocumentControls();
                this.listBoxPayment.SelectedIndex = this.listBoxPayment.Items.Count - 1;
                this.listBoxPayment_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonPaymentDelete_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.transactionPaymentBindingSource.Current;
                if (System.Windows.Forms.MessageBox.Show("Do you really want to delete this entry?", "Delete?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    R.Delete();
                }
                if (R.Row.RowState == DataRowState.Deleted || R.Row.RowState == DataRowState.Detached)
                {
                    //this.webBrowserTransactionDocuments.Url = new Uri("about:blank");
                    //this.setDocumentControls();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void listBoxPayment_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.transactionPaymentBindingSource.Position = this.listBoxPayment.SelectedIndex;
            if (this.listBoxPayment.SelectedIndex > -1)
            {
                this.splitContainerPayment.Panel2.Enabled = true;
                System.Data.DataRowView R = (System.Data.DataRowView)this.transactionPaymentBindingSource.Current;
                this.setPaymentDateControls(R.Row);
            }
            else this.splitContainerPayment.Panel2.Enabled = false;
            //this.setDocumentControls();
        }
        
#endregion

#region Date

        private void buttonPaymentDate_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.transactionPaymentBindingSource.Current;
            if (R["PaymentDate"].Equals(System.DBNull.Value))
            {
                R["PaymentDate"] = System.DateTime.Now;
            }
            else
            {
                R["PaymentDate"] = System.DBNull.Value;
            }
            this.setPaymentDateControls(R.Row);
        }

        private void dateTimePickerPaymentDate_CloseUp(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.transactionPaymentBindingSource.Current;
            R["PaymentDate"] = this.dateTimePickerPaymentDate.Value.ToShortDateString();
        }

        private void setPaymentDateControls(System.Data.DataRow R)
        {
            if (R["PaymentDate"].Equals(System.DBNull.Value))
            {
                this.dateTimePickerPaymentDate.DataBindings.Clear();
                this.dateTimePickerPaymentDate.Visible = false;
                this.buttonPaymentDate.Image = this.imageListSetDate.Images[0];
            }
            else
            {
                if (this.dateTimePickerPaymentDate.DataBindings.Count == 0)
                {
                    this.dateTimePickerPaymentDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.transactionPaymentBindingSource, "PaymentDate", true));
                }
                this.dateTimePickerPaymentDate.Visible = true;
                this.buttonPaymentDate.Image = this.imageListSetDate.Images[1];
            }
        }
        
#endregion

        private void buttonPaymentURI_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.transactionPaymentBindingSource.Current;
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(R["PaymentURI"].ToString());
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                R.BeginEdit();
                R["PaymentURI"] = f.URL;
                R.EndEdit();
            }
        }

        private void textBoxPaymentAmount_Validating(object sender, CancelEventArgs e)
        {
            double d;
            if (!double.TryParse(this.textBoxPaymentAmount.Text, out d) && this.textBoxPaymentAmount.Text.Length > 0)
            {
                System.Windows.Forms.MessageBox.Show(this.textBoxPaymentAmount.Text + " is not a valid entry for a payment.\r\nOnly numeric values are allowed here");
            }
            else if (this.textBoxPaymentAmount.Text.Length == 0)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.transactionPaymentBindingSource.Current;
                R.BeginEdit();
                R["Amount"] = System.DBNull.Value;
                R.EndEdit();
            }
        }

        private void textBoxPaymentForeignAmount_Validating(object sender, CancelEventArgs e)
        {
            double d;
            if (this.textBoxPaymentForeignAmount.Text.Length > 0  && !double.TryParse(this.textBoxPaymentForeignAmount.Text, out d))
            {
                System.Windows.Forms.MessageBox.Show(this.textBoxPaymentForeignAmount.Text + " is not a valid entry for a payment.\r\nOnly numeric values are allowed here");
            }
            else if (this.textBoxPaymentForeignAmount.Text.Length == 0)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.transactionPaymentBindingSource.Current;
                R.BeginEdit();
                R["ForeignAmount"] = System.DBNull.Value;
                R.EndEdit();
            }
        }

        private void buttonSavePayment_Click(object sender, EventArgs e)
        {
            this._Transaction.saveDependentTables();
        }

#endregion

#region Agent

        private void toolStripButtonAgentsAdd_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRow R = this._Transaction.DataSet.Tables["TransactionAgent"].NewRow();
                R["TransactionID"] = ID;
                R["AgentName"] = "New agent";
                this._Transaction.DataSet.Tables["TransactionAgent"].Rows.Add(R);
                this.listBoxAgents.SelectedIndex = this.listBoxAgents.Items.Count - 1;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonAgentsRemove_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.transactionAgentBindingSource.Current;
                if (System.Windows.Forms.MessageBox.Show("Do you really want to delete this entry?", "Delete?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    R.Delete();
                }
                if (R.Row.RowState == DataRowState.Deleted || R.Row.RowState == DataRowState.Detached)
                {
                    //this.setDocumentControls();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        private void comboBoxAgentRole_DropDown(object sender, EventArgs e)
        {
            string SQL = "SELECT AgentRole " +
                "FROM TransactionAgent " +
                "GROUP BY AgentRole " +
                "HAVING (AgentRole <> N'') " +
                "ORDER BY AgentRole";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            this.comboBoxAgentRole.DataSource = dt;
            this.comboBoxAgentRole.DisplayMember = "AgentRole";
            this.comboBoxAgentRole.ValueMember = "AgentRole";
        }

        private void listBoxAgents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxAgents.SelectedIndex > -1)
                this.splitContainerAgents.Panel2.Enabled = true;
            else this.splitContainerAgents.Panel2.Enabled = false;
        }

#endregion

#region Identifier

        private System.Data.DataTable _dtIdentifier;

        private void initIdentifier()
        {
            this.listBoxIdentifier.DataSource = this.dataSetTransaction.Tables["Identifier"];//._dtIdentifier;
            this.listBoxIdentifier.DisplayMember = "Identifier";
            this.listBoxIdentifier.ValueMember = "ID";
            this.tableLayoutPanelIdentifier.Enabled = false;
        }

        private void toolStripButtonIdentifierAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string ReferencedTable = "Transaction";
                System.Drawing.Image Image = DiversityCollection.Resource.Transaction;
                int ReferencedID = this.ID;

                DiversityCollection.Forms.FormExternalIdentifier f = new Forms.FormExternalIdentifier(DiversityWorkbench.ReferencingTable.IdentifierType.ID);
                f.ShowDialog();
                if (f.IdentifierType().Length > 0 && f.Identifier().Length > 0)
                {
                    System.Data.DataRow R = this.dataSetTransaction.ExternalIdentifier.NewRow();
                    R["Type"] = f.IdentifierType();
                    R["Identifier"] = f.Identifier();
                    R["Notes"] = f.Notes();
                    R["URL"] = f.URL();
                    R["ReferencedID"] = ReferencedID;
                    R["ReferencedTable"] = ReferencedTable;
                    this.dataSetTransaction.ExternalIdentifier.Rows.Add(R);
                }
            }
            catch (System.Exception ex) { }
        }

        private void toolStripButtonIdentifierRemove_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.externalIdentifierBindingSource.Current;
            R.Delete();
        }
        
        private void listBoxIdentifier_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxIdentifier.SelectedIndex > -1)
                this.tableLayoutPanelIdentifier.Enabled = true;
            else
                this.tableLayoutPanelIdentifier.Enabled = false;
        }

#endregion

#region TableEditor

        private void buttonTableEditor_Click(object sender, EventArgs e)
        {
            this.TableEditing("Transaction", DiversityCollection.Resource.Transaction);
        }

        private void TableEditing(string Table, System.Drawing.Image Icon)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                if (this.userControlQueryList.ListOfIDs.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Nothing selected");
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    return;
                }
                System.Collections.Generic.List<string> ReadOnlyColumns = new List<string>();
                string SqlCol = "SELECT C.COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = '" + Table + "'";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter adCol = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCol, DiversityWorkbench.Settings.ConnectionString);
                adCol.Fill(dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    if (R[0].ToString().EndsWith("ID") || R[0].ToString().StartsWith("Log"))
                        ReadOnlyColumns.Add(R[0].ToString());
                }

                string IDs = "";
                foreach (int i in this.userControlQueryList.ListOfIDs)
                {
                    // Markus, 25.07.2018: Exclusion of non accessible IDs
                    if (this._Transaction.NonAccessibleIDs.Contains(i))
                        continue;
                    if (IDs.Length > 0) IDs += ",";
                    IDs += i.ToString();
                }
                string SQL = "SELECT * FROM [" + Table + "] T WHERE T.TransactionID IN (" + IDs + ")";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout * 1000));
                ad.SelectCommand.CommandTimeout = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout * 1000;
                DiversityWorkbench.Forms.FormTableEditor f = new DiversityWorkbench.Forms.FormTableEditor(Icon, ad, ReadOnlyColumns, DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout);
                f.StartPosition = FormStartPosition.CenterParent;
                f.Width = this.Width - 10;
                f.Height = this.Height - 10;
                //f.setHelpProvider(this.HelpProvider.HelpNamespace, "Table editor");
                bool SetTimeout = false;
                try
                {
                    f.ShowDialog();
                    //this.setSpecimen(this.ID);
                }
                catch (System.Exception ex)
                {
                    SetTimeout = true;
                }
                if (SetTimeout)
                {
                    int? Timeout = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout;
                    DiversityWorkbench.Forms.FormGetInteger ftimeout = new DiversityWorkbench.Forms.FormGetInteger(Timeout, "Set timeout", "A timeout occured. Please set the seconds you are prepared to wait");
                    ftimeout.ShowDialog();
                    if (ftimeout.DialogResult == System.Windows.Forms.DialogResult.OK && ftimeout.Integer != null)
                    {
                        DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout = (int)ftimeout.Integer;
                        DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "TableEditing(string Table, System.Drawing.Image Icon)");
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private System.Collections.Generic.List<string> TableEditingReadOnlyColumns(string Table)
        {
            System.Collections.Generic.List<string> ReadOnlyColumns = new List<string>();
            string SqlCol = "SELECT C.COLUMN_NAME, C.DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = '" + Table + "'";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter adCol = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCol, DiversityWorkbench.Settings.ConnectionString);
            adCol.Fill(dt);

            System.Collections.Generic.List<string> LookUpColumns = new List<string>();
            LookUpColumns.Add("ParameterID");
            LookUpColumns.Add("MethodID");
            LookUpColumns.Add("ProcessingID");
            LookUpColumns.Add("ExternalDatasourceID");
            LookUpColumns.Add("AnalysisID");
            LookUpColumns.Add("CollectionID");
            //LookUpColumns.Add("LocalisationSystemID");
            LookUpColumns.Add("ParentTransactionID");

            foreach (System.Data.DataRow R in dt.Rows)
            {
                if (LookUpColumns.Contains(R[0].ToString()))
                    continue;
                if (R[0].ToString().EndsWith("ID")
                    || R[0].ToString().StartsWith("Log")
                    || R[1].ToString() == "uniqueidentifier"
                    || R[1].ToString() == "geography"
                    || R[1].ToString() == "geometry")
                    ReadOnlyColumns.Add(R[0].ToString());
            }
            return ReadOnlyColumns;
        }

        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.Forms.FormTableEditor.ComboBoxValues> TableEditingLookupValues(string TableName)
        {
            System.Collections.Generic.Dictionary<string, DiversityWorkbench.Forms.FormTableEditor.ComboBoxValues> DD = new Dictionary<string, DiversityWorkbench.Forms.FormTableEditor.ComboBoxValues>();
            DiversityWorkbench.Data.Table T = new DiversityWorkbench.Data.Table(TableName, DiversityWorkbench.Settings.ConnectionString);
            T.FindColumnsWithForeignRelations();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Data.Column> C in T.Columns)
            {
                if (C.Value.ForeignRelations.Count == 1
                    && C.Value.ForeignRelationColumn.Length > 0
                    && C.Value.ForeignRelationTable.Length > 0
                    && C.Value.ForeignRelationTable.ToLower().EndsWith("_enum"))
                {
                    System.Data.DataTable dt = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter("SELECT NULL AS Code, NULL AS DisplayText UNION SELECT Code, DisplayText FROM " + C.Value.ForeignRelationTable + " WHERE DisplayEnable = 1 ORDER BY DisplayText", DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    DiversityWorkbench.Forms.FormTableEditor.ComboBoxValues CBV = new DiversityWorkbench.Forms.FormTableEditor.ComboBoxValues(dt, "DisplayText", "Code");
                    DD.Add(C.Key, CBV);
                }
                else if (C.Value.ForeignRelations.Count > 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in C.Value.ForeignRelations)
                    {
                        if (!DD.ContainsKey(C.Key))
                        {
                            System.Data.DataTable dt = new DataTable();
                            string SQL = "";
                            string DisplayColumn = "DisplayText";
                            string ColumnHeader = "";
                            string Union = "SELECT NULL AS " + KV.Value + ", NULL AS DisplayText UNION ";
                            switch (KV.Value)
                            {
                                case "ParameterID":
                                    SQL = Union + "SELECT ParameterID, DisplayText FROM Parameter ORDER BY DisplayText";
                                    ColumnHeader = "Parameter";
                                    break;
                                case "SeriesID":
                                    SQL = Union + "SELECT SeriesID, case when SeriesCode is null or SeriesCode = '' then Description else SeriesCode end AS DisplayText FROM CollectionEventSeries ORDER BY DisplayText";
                                    ColumnHeader = "EventSeries";
                                    break;
                                case "MethodID":
                                    SQL = Union + "SELECT MethodID, DisplayText FROM Method ORDER BY DisplayText";
                                    ColumnHeader = "Method";
                                    break;
                                case "ProjectID":
                                    SQL = Union + "SELECT ProjectID, Project FROM ProjectProxy ORDER BY DisplayText";
                                    ColumnHeader = "Project";
                                    break;
                                case "ProcessingID":
                                    SQL = Union + "SELECT ProcessingID, DisplayText FROM Processing ORDER BY DisplayText";
                                    ColumnHeader = "Processing";
                                    break;
                                case "ExternalDatasourceID":
                                    SQL = Union + "SELECT ExternalDatasourceID, ExternalDatasourceName FROM CollectionExternalDatasource ORDER BY DisplayText";
                                    ColumnHeader = "ExternalDatasource";
                                    break;
                                case "AnalysisID":
                                    SQL = Union + "SELECT AnalysisID, DisplayText FROM Analysis ORDER BY DisplayText";
                                    ColumnHeader = "Analysis";
                                    break;
                                case "CollectionID":
                                    SQL = Union + "SELECT CollectionID, CollectionName AS DisplayText FROM Collection ORDER BY DisplayText";
                                    ColumnHeader = C.Value.Name;
                                    if (ColumnHeader.EndsWith("ID"))
                                        ColumnHeader = ColumnHeader.Substring(0, ColumnHeader.Length - 2);
                                    break;
                                case "PropertyID":
                                    SQL = Union + "SELECT PropertyID, DisplayText FROM Property ORDER BY DisplayText";
                                    ColumnHeader = "Property";
                                    break;
                                case "LocalisationSystemID":
                                    SQL = Union + "SELECT LocalisationSystemID, DisplayText FROM LocalisationSystem ORDER BY DisplayText";
                                    ColumnHeader = "LocalisationSystem";
                                    break;
                                //case "TransactionID":
                                //    SQL = Union + "SELECT TransactionID, TransactionTitle FROM [Transaction] ORDER BY DisplayText";
                                //    ColumnHeader = C.Value.Name;
                                //    if (ColumnHeader.EndsWith("ID"))
                                //        ColumnHeader = ColumnHeader.Substring(0, ColumnHeader.Length - 2);
                                //    break;
                            }
                            if (SQL.Length > 0)
                            {
                                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                                ad.Fill(dt);
                                DiversityWorkbench.Forms.FormTableEditor.ComboBoxValues CBV = new DiversityWorkbench.Forms.FormTableEditor.ComboBoxValues(dt, DisplayColumn, KV.Value, ColumnHeader);
                                DD.Add(C.Key, CBV);
                            }
                        }
                    }
                }
            }
            return DD;
        }



        #endregion

        #region Chart

        /*
         * gewünschte Ausbage war ab 2020, Land, Stadt, Name, Jahr, Anzahl, Typ ...
         * 
        SELECT CASE WHEN T.FromTransactionPartnerName LIKE '%Botanische Staatssammlung M%nchen%' THEN Ato.Country ELSE Afrom.Country END AS Country,
                  CASE WHEN T.FromTransactionPartnerName LIKE '%Botanische Staatssammlung M%nchen%' THEN Ato.City ELSE Afrom.City END AS City,
                  CASE WHEN T.FromTransactionPartnerName LIKE '%Botanische Staatssammlung M%nchen%' THEN T .ToTransactionPartnerName ELSE T.FromTransactionPartnerName END AS TransactionPartner, YEAR(T.BeginDate)
                  AS Jahr, T.TransactionType, Count(*) AS Transaktionen, SUM(T.NumberOfUnits) AS NumberOfUnits
        FROM            TransactionList_H7 AS T INNER JOIN
                          DiversityAgents.dbo.AgentContactInformation AS Afrom ON T.FromTransactionPartnerAgentURI = DiversityAgents.dbo.BaseURL() + CAST(Afrom.AgentID AS varchar) INNER JOIN

                          DiversityAgents.dbo.AgentContactInformation AS Ato ON T.ToTransactionPartnerAgentURI = DiversityAgents.dbo.BaseURL() + CAST(Ato.AgentID AS varchar)
        WHERE        ((T.FromTransactionPartnerName LIKE '%Botanische Staatssammlung M%nchen%') OR
                          (T.ToTransactionPartnerName LIKE '%Botanische Staatssammlung M%nchen%'))
                                 AND YEAR(T.BeginDate) >= 2020
        GROUP BY YEAR(T.BeginDate), T.TransactionType, T.FromTransactionPartnerName, T.ToTransactionPartnerName, Ato.Country, Afrom.Country, Ato.City, Afrom.City
        HAVING(T.TransactionType IN (N'loan', N'return', N'exchange'))

        UNION

        SELECT        CASE WHEN T.FromTransactionPartnerName LIKE '%Botanische Staatssammlung M%nchen%' THEN Ato.Country ELSE Afrom.Country END AS Country,
                                CASE WHEN T.FromTransactionPartnerName LIKE '%Botanische Staatssammlung M%nchen%' THEN Ato.City ELSE Afrom.City END AS City,
                                CASE WHEN T.FromTransactionPartnerName LIKE '%Botanische Staatssammlung M%nchen%' THEN T .ToTransactionPartnerName ELSE T.FromTransactionPartnerName END AS TransactionPartner, YEAR(T.BeginDate)
                                AS Jahr, R.TransactionType, COUNT(*) AS Transaktionen, SUM(T.NumberOfUnits) AS NumberOfUnits
        FROM            TransactionList_H7 AS T INNER JOIN
                                DiversityAgents.dbo.AgentContactInformation AS Afrom ON T.FromTransactionPartnerAgentURI = DiversityAgents.dbo.BaseURL() + CAST(Afrom.AgentID AS varchar) INNER JOIN

                                DiversityAgents.dbo.AgentContactInformation AS Ato ON T.ToTransactionPartnerAgentURI = DiversityAgents.dbo.BaseURL() + CAST(Ato.AgentID AS varchar) INNER JOIN

                                [Transaction] AS R ON T.TransactionID = R.ParentTransactionID
        WHERE        (T.FromTransactionPartnerName LIKE '%Botanische Staatssammlung M%nchen%') AND(YEAR(T.BeginDate) >= 2020) OR
                               (YEAR(T.BeginDate) >= 2020) AND(T.ToTransactionPartnerName LIKE '%Botanische Staatssammlung M%nchen%')
        GROUP BY YEAR(T.BeginDate), T.TransactionType, T.FromTransactionPartnerName, T.ToTransactionPartnerName, Ato.Country, Afrom.Country, Ato.City, Afrom.City, R.TransactionType
        HAVING(R.TransactionType = N'return') AND(T.TransactionType = N'loan')
        ORDER BY Country, City, TransactionPartner, Jahr
        */


        #region Parameter

        private System.Data.DataTable _DtChartCollections;
        private System.Data.DataTable _DtChartTransactionTypes;
        private System.Data.DataTable _DtChartData;

        #endregion

        #region init

        private void initChart()
        {
            try
            {
                if (this.chartChart != null) // #114
                    this.chartChart.Series.Clear();
                this.initChartCollections();
                this.initChartTransactionTypes();
                //this.tabControlChart.TabPages.Remove(this.tabPageChartData);
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void initChartCollections()
        {
            this.checkedListBoxChartCollections.Items.Clear();
            string SQL = "SELECT C.CollectionName, c.CollectionID " +
                "FROM [Transaction] AS T INNER JOIN " +
                "Collection C ON T.AdministratingCollectionID = C.CollectionID " +
                "GROUP BY C.CollectionName, c.CollectionID ORDER BY C.CollectionName";
            _DtChartCollections = new DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref _DtChartCollections);
            int i = 0;
            foreach(System.Data.DataRow R in _DtChartCollections.Rows)
            {
                this.checkedListBoxChartCollections.Items.Add(R[0].ToString());
            }

        }

        private void initChartTransactionTypes()
        {
            this.checkedListBoxChartTransactionType.Items.Clear();
            string SQL = "SELECT T.TransactionType " +
                "FROM CollectionSpecimenTransaction AS S INNER JOIN " +
                "[Transaction] AS T ON S.TransactionID = T.TransactionID " +
                "WHERE T.TransactionType <> 'transaction group' " +
                "GROUP BY T.TransactionType " +
                "ORDER BY TransactionType";
            _DtChartTransactionTypes = new DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref _DtChartTransactionTypes);
            int i = 0;
            foreach (System.Data.DataRow R in _DtChartTransactionTypes.Rows)
            {
                this.checkedListBoxChartTransactionType.Items.Add(R[0].ToString());
            }
        }

        #endregion

        #region Chart

        private void buttonChartGenerate_Click(object sender, EventArgs e)
        {
            if (this.ChartSelectedCollectionIDs().Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select the administrating collections that should be included");
                return;
            }
            if (this.chartSeparation == ChartSeparation.Type && this.ChartSelectedTransactionTypes().Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select the transaction types that should be included");
                return;
            }
            // #114
            if (this.chartChart != null)
            {
                this.chartChart.Series.Clear();
                this.chartChart.ChartAreas.Clear();
                this.chartChart.ChartAreas.Add("Transactions");
            }
            this._DtChartData = null;
            this.CreateChart();
            if (this.radioButtonChartDataChart.Checked)
            {
                this.dataGridViewChartData.DataSource = this._DtChartData;
            }
        }

        private void CreateChart()
        {
            try
            {
                switch(this.chartSeparation)
                {
                    case ChartSeparation.None:
                        this.AddChartSeries(this.ChartDataSoure, "Transaction");
                        //this.chartChart.DataSource = this.ChartDataSoure;
                        //this.chartChart.Series.Add("Transaction");
                        //foreach (System.Data.DataRow R in ChartDataSoure.Rows)
                        //{
                        //    double Jahr;
                        //    double Anzahl;
                        //    if (double.TryParse(R[0].ToString(), out Jahr) && double.TryParse(R[1].ToString(), out Anzahl))
                        //        this.chartChart.Series["Transaction"].Points.AddXY(Jahr, Anzahl);
                        //}
                        break;
                    case ChartSeparation.Collection:
                        foreach(string C in this.ChartSelectedCollections())
                        {
                            this.AddChartSeries(this.chartDataSoure(this.ChartSelectedCollectionID(C)), C);
                        }
                        break;
                    case ChartSeparation.Type:
                        foreach (string T in this.ChartSelectedTransactionTypes())
                        {
                            this.AddChartSeries(this.chartDataSoure(null, T), T);
                        }
                        break;
                }
                //foreach(System.Windows.Forms.DataVisualization.Charting.Series S in this.chartChart.Series)
                //{
                //    S.
                //}
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void AddChartSeries(System.Data.DataTable dataTable, string Title)
        {
            try
            {
                // #114
                if (this.chartChart != null)
                {
                    this.chartChart.Series.Add(Title);
                    foreach (System.Data.DataRow R in dataTable.Rows)
                    {
                        double Jahr;
                        double Anzahl;
                        if (double.TryParse(R[0].ToString(), out Jahr) && double.TryParse(R[1].ToString(), out Anzahl))
                            this.chartChart.Series[Title].Points.AddXY(Jahr, Anzahl);
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private System.Data.DataTable chartDataSoure(int? CollectionID = null, string TransactionType = "")
        {
            string Source = "";
            if (CollectionID != null) Source = DiversityCollection.LookupTable.CollectionName((int)CollectionID);
            else if (TransactionType.Length > 0) Source = TransactionType;
            System.Data.DataTable dtChartData = new DataTable();
            string SQL = "SELECT CASE WHEN T.BeginDate IS NULL THEN year(T.LogCreatedWhen) ELSE year(T.BeginDate) END AS Jahr, COUNT(*) ";
            if (this.ChartAddUnits)
                SQL += " + CASE WHEN SUM(T.NumberOfUnits) IS NULL THEN 0 ELSE SUM(T.NumberOfUnits) END ";
            SQL += " AS Anzahl # " + //, C.CollectionName, T.TransactionType " +
                "FROM CollectionSpecimenTransaction AS S INNER JOIN " +
                "[Transaction] AS T ON S.TransactionID = T.TransactionID INNER JOIN " +
                "Collection C ON T.AdministratingCollectionID = C.CollectionID " +
                "WHERE 1 = 1 ";
            if (CollectionID != null)
                SQL += " AND C.CollectionID = " + CollectionID.ToString() + " ";
            if (TransactionType.Length > 0)
                SQL += " AND T.TransactionType = '" + TransactionType + "' ";
            if (ChartStartYear().Length > 0)
                SQL += " AND CASE WHEN T .BeginDate IS NULL THEN year(T.LogCreatedWhen) ELSE year(T.BeginDate) END >= " + ChartStartYear();
            SQL += " GROUP BY CASE WHEN T.BeginDate IS NULL THEN year(T.LogCreatedWhen) ELSE year(T.BeginDate) END " + //, T.TransactionType, C.CollectionName " +
                " ORDER BY Jahr";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL.Replace("#", ""), ref dtChartData);
            if (_DtChartData == null) _DtChartData = new DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL.Replace("#", ", '" + Source + "' AS Source"), ref _DtChartData);
            return dtChartData;
        }
        

        private System.Data.DataTable ChartDataSoure
        {
            get
            {
                if (this._DtChartData == null)
                {
                    this._DtChartData = new DataTable();
                    string SQL = "SELECT CASE WHEN T.BeginDate IS NULL THEN year(T.LogCreatedWhen) ELSE year(T.BeginDate) END AS Jahr, COUNT(*) ";
                    if (this.ChartAddUnits)
                        SQL += " + CASE WHEN SUM(T.NumberOfUnits) IS NULL THEN 0 ELSE SUM(T.NumberOfUnits) END ";
                    SQL += " AS Anzahl " + //, C.CollectionName, T.TransactionType " +
                        "FROM CollectionSpecimenTransaction AS S INNER JOIN " +
                        "[Transaction] AS T ON S.TransactionID = T.TransactionID INNER JOIN " +
                        "Collection C ON T.AdministratingCollectionID = C.CollectionID " +
                        "WHERE C.CollectionID IN (" + chartCollectionIDs() + ") ";
                    if (chartTransactionTypes().Length > 0)
                        SQL += " AND T.TransactionType IN (" + chartTransactionTypes() + ") ";
                    if (ChartStartYear().Length > 0)
                        SQL += " AND CASE WHEN T.BeginDate IS NULL THEN year(T .LogCreatedWhen) ELSE year(T.BeginDate) END >= " + ChartStartYear();
                    SQL += " GROUP BY CASE WHEN T.BeginDate IS NULL THEN year(T.LogCreatedWhen) ELSE year(T.BeginDate) END " + //, T.TransactionType, C.CollectionName " +
                        " ORDER BY Jahr";
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._DtChartData);
                }
                return this._DtChartData;
            }
        }

        #region Start, Collections, Units and types

        private string ChartStartYear()
        {
            if (this.checkBoxChartStart.Checked)
                return this.numericUpDownChartStart.Value.ToString();
            return "";
        }
        private string chartCollectionIDs()
        {
            //get
            {
                string IDs = "";
                foreach(int id in this.ChartSelectedCollectionIDs())
                {
                    if (IDs.Length > 0) IDs += ", ";
                    IDs += id.ToString();
                }
                return IDs;
            }
        }

        private bool ChartAddUnits { get { return this.checkBoxChartAddNumberOfUnits.Checked; } }

        private System.Collections.Generic.List<string> ChartSelectedCollections()
        {
            System.Collections.Generic.List<string> list = new List<string>();
            try
            {
                foreach (System.Object o in this.checkedListBoxChartCollections.CheckedItems)
                {
                    list.Add(o.ToString());
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return list;
        }

        private int ChartSelectedCollectionID(string Collection)
        {
            int ID  = -1;
            System.Data.DataRow[] rr = this._DtChartCollections.Select("CollectionName = '" + Collection + "'");
            if (rr.Length == 1)
            {
                int.TryParse(rr[0][1].ToString(), out ID);
            }
            return ID;
        }

        private System.Collections.Generic.List<int> ChartSelectedCollectionIDs()
        {
            //get
            {
                System.Collections.Generic.List<int> list = new List<int>();
                try
                {
                    foreach (System.Object o in this.checkedListBoxChartCollections.CheckedItems)
                    {
                        int ID = this.ChartSelectedCollectionID(o.ToString());
                        if (ID > -1)
                            list.Add(ID);
                        //System.Data.DataRow[] rr = this._DtChartCollections.Select("CollectionName = '" + o.ToString() + "'");
                        //if (rr.Length == 1)
                        //{
                        //    int ID;
                        //    if (int.TryParse(rr[0][1].ToString(), out ID))
                        //    {
                        //        list.Add(ID);
                        //    }
                        //}
                    }
                }
                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                return list;
            }
        }

        private System.Collections.Generic.List<string> ChartSelectedTransactionTypes()
        {
            System.Collections.Generic.List<string> list = new List<string>();
            try
            {
                foreach (System.Object o in this.checkedListBoxChartTransactionType.CheckedItems)
                {
                    list.Add(o.ToString());
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return list;
        }

        private string chartTransactionTypes()
        {
            //get
            {
                string Types = "";
                foreach(System.Object o in this.checkedListBoxChartTransactionType.CheckedItems)
                {
                    if (Types.Length > 0) Types += ", ";
                    Types += "'" + o.ToString() + "'";
                }
                return Types;
            }
        }

        #endregion

        #endregion

        #region Separation
        private enum ChartSeparation { None, Collection, Type }

        private ChartSeparation chartSeparation
        {
            get
            {
                if (this.radioButtonChartGroupingCollection.Checked) return ChartSeparation.Collection;
                if (this.radioButtonChartGroupingTransactionType.Checked) return ChartSeparation.Type;
                return ChartSeparation.None;
            }
        }

        #endregion


        #region Data

        private void buttonChartDataExport_Click(object sender, EventArgs e)
        {

        }

        private void buttonChartDataGet_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonChartAddressSelfAdd_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonChartAddressSelfRemove_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #endregion

        #region Tasks
        private void toolStripButtonShowTasks_Click(object sender, EventArgs e)
        {
#if !DEBUG
            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            return;
#endif
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            bool showTask = true;
            if (this.toolStripButtonShowTasks.BackColor == System.Drawing.Color.Yellow)
                showTask = false;
            showTask = !showTask;
            this._Transaction.SetTaskVisibility(showTask);
            if (showTask)
            {
                this.toolStripButtonShowTasks.BackColor = System.Drawing.SystemColors.Control;
                this.toolStripButtonShowTasks.Image = DiversityCollection.Resource.Task;
                //this.toolStripButtonTaskOpen.Visible = true;
            }
            else
            {
                this.toolStripButtonShowTasks.BackColor = System.Drawing.Color.Yellow;
                this.toolStripButtonShowTasks.Image = DiversityCollection.Resource.TaskGrey;
                //this.toolStripButtonTaskOpen.Visible = false;
            }
            this._Transaction.buildHierarchy();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        #endregion

        #region Manual

        /// <summary>
        /// Adding event deletates to form and controls
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task InitManual()
        {
            try
            {

                DiversityWorkbench.DwbManual.Hugo manual = new Hugo(this.helpProvider, this);
                if (manual != null)
                {
                    await manual.addKeyDownF1ToForm();
                }
            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// ensure that init is only done once
        /// </summary>
        private bool _InitManualDone = false;


        /// <summary>
        /// KeyDown of the form adding event deletates to form and controls within the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (!_InitManualDone)
                {
                    await this.InitManual();
                    _InitManualDone = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion

    }
}