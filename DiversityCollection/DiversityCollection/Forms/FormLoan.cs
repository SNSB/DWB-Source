using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection
{
    public partial class FormLoan : Form
    {
        #region Parameter
        private DiversityCollection.Loan _Loan;
        private DiversityCollection.Datasets.DataSetLoan _DataSetLoanForReturn;

        private string _XmlSending;

        #region Scanning
        private enum ScanStep { Idle, Scanning };
        private ScanStep _CurrentScanStepSending = ScanStep.Idle;
        private ScanStep _CurrentScanStepReturn = ScanStep.Idle;
        #endregion
        
        #endregion

        #region Construction
        public FormLoan()
        {
            InitializeComponent();
            this.splitContainerData.Panel2Collapsed = true;
            this.splitContainerMain.Panel2.Visible = false;
            this.initForm();
            this.setChildControls();
        }

        public FormLoan(int? ItemID)
            : this()
        {
            if (ItemID != null)
                this._Loan.setItem((int)ItemID);
            this.userControlDialogPanel.Visible = true;
            this.panelHeader.Visible = true;
        }

        #endregion

        #region Form
        private void initForm()
        {
            System.Data.DataSet Dataset = this.dataSetLoan;
            //System.Windows.Forms.BindingSource B = new BindingSource(this.dataSetLoan, "Loan");
            if (this._Loan == null)
                this._Loan = new Loan(ref Dataset, this.dataSetLoan.Loan,
                    ref this.treeViewLoan, this, this.userControlQueryList, this.splitContainerMain,
                    this.splitContainerData, this.toolStripButtonSpecimenList, //this.imageListSpecimenList,
                    this.userControlSpecimenList, this.helpProvider, this.toolTip, ref this.loanBindingSource,
                    this.tabControlLoan);
            //if (this._Loan == null)
            //    this._Loan = new Loan(ref Dataset, this.dataSetLoan.Loan,
            //        ref this.treeViewLoan, this, this.userControlQueryList, this.splitContainerMain,
            //        this.splitContainerData, this.toolStripButtonSpecimenList, this.imageListSpecimenList,
            //        this.userControlSpecimenList, this.helpProvider, this.toolTip, ref B);
            this._Loan.initForm();
            this._Loan.setToolStripButtonDeleteEvent(this.toolStripButtonDelete);
            this._Loan.setToolStripButtonNewEvent(this.toolStripButtonNew);
            this._Loan.setToolbarPermission(ref this.toolStripButtonDelete, "Loan", "Delete");
            this._Loan.setToolbarPermission(ref this.toolStripButtonNew, "Loan", "Insert");
            this.initRemoteModules();
            this.userControlSpecimenList.toolStripButtonDelete.Visible = false;
            this.listBoxLoanHistory.DataSource = this._Loan.DataSet.Tables["LoanHistory"];
            this.listBoxLoanHistory.DisplayMember = "Date";
            this.setHistoryControls();
            this.readSettings();
        }

        /// <summary>
        /// adding the controls to the list of the ChildControls, that will only be shown 
        /// when a dataset is selected, that is not the root of the hierarchy
        /// </summary>
        private void setChildControls()
        {
            this._Loan.ChildControls.Add(this.labelBegin);
            this._Loan.ChildControls.Add(this.textBoxLoanBegin);
            this._Loan.ChildControls.Add(this.dateTimePickerLoanBegin);

            this._Loan.ChildControls.Add(this.labelLoanEnd);
            this._Loan.ChildControls.Add(this.textBoxLoanEnd);
            this._Loan.ChildControls.Add(this.dateTimePickerLoanEnd);

            this._Loan.ChildControls.Add(this.labelLoanComment);
            this._Loan.ChildControls.Add(this.textBoxLoanComment);
            this._Loan.ChildControls.Add(this.buttonLoanCommentInsertSelected);
            this._Loan.ChildControls.Add(this.comboBoxLoanCommentAdd);

            this._Loan.ChildControls.Add(this.labelLoanNumber);
            this._Loan.ChildControls.Add(this.textBoxLoanNumber);

            this._Loan.ChildControls.Add(this.labelInitialNumberOfSpecimen);
            this._Loan.ChildControls.Add(this.textBoxInitialNumberOfSpecimen);

            this._Loan.ChildControls.Add(this.labelInvestigator);
            this._Loan.ChildControls.Add(this.textBoxInvestigator);

            this._Loan.ChildControls.Add(this.labelResponsible);
            this._Loan.ChildControls.Add(this.userControlModuleRelatedEntryResponsible);
        }

        private void readSettings()
        {
            if (DiversityCollection.Forms.FormLoanSettings.Default.SendingSchemaFile.Length > 0)
                this.textBoxSendingSchema.Text = DiversityCollection.Forms.FormLoanSettings.Default.SendingSchemaFile;

            if (DiversityCollection.Forms.FormLoanSettings.Default.ConfirmationSchemaFile.Length > 0)
                this.textBoxConfirmationSchema.Text = DiversityCollection.Forms.FormLoanSettings.Default.ConfirmationSchemaFile;

            if (DiversityCollection.Forms.FormLoanSettings.Default.ReminderSchemaFile.Length > 0)
                this.textBoxReminderSchema.Text = DiversityCollection.Forms.FormLoanSettings.Default.ReminderSchemaFile;

            if (DiversityCollection.Forms.FormLoanSettings.Default.PartialReturnSchemaFile.Length > 0)
                this.textBoxPartialReturnSchema.Text = DiversityCollection.Forms.FormLoanSettings.Default.PartialReturnSchemaFile;

            if (DiversityCollection.Forms.FormLoanSettings.Default.ReturnSchemaFile.Length > 0)
                this.textBoxReturnSchema.Text = DiversityCollection.Forms.FormLoanSettings.Default.ReturnSchemaFile;
        }

        private void FormLoan_FormClosing(object sender, FormClosingEventArgs e)
        {
            DiversityCollection.Forms.FormLoanSettings.Default.SendingSchemaFile = this.textBoxSendingSchema.Text;
            DiversityCollection.Forms.FormLoanSettings.Default.ConfirmationSchemaFile = this.textBoxConfirmationSchema.Text;
            DiversityCollection.Forms.FormLoanSettings.Default.ReminderSchemaFile = this.textBoxReminderSchema.Text;
            DiversityCollection.Forms.FormLoanSettings.Default.PartialReturnSchemaFile = this.textBoxPartialReturnSchema.Text;
            DiversityCollection.Forms.FormLoanSettings.Default.ReturnSchemaFile = this.textBoxReturnSchema.Text;
            DiversityCollection.Forms.FormLoanSettings.Default.Save();
        }

        #endregion

        #region Modules

        private void initRemoteModules()
        {
            try
            {
                // Agents
                DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryLoanPartner.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryResponsible.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                //System.Windows.Forms.BindingSource B = new BindingSource(this.dataSetLoan, "Loan");
                //this.userControlModuleRelatedEntryLoanPartner.bindToData("Loan", "LoanPartnerName", "LoanPartnerAgentURI", B);
                //this.userControlModuleRelatedEntryResponsible.bindToData("Loan", "ResponsibleName", "ResponsibleAgentURI", B);
                this.userControlModuleRelatedEntryLoanPartner.bindToData("Loan", "LoanPartnerName", "LoanPartnerAgentURI", this.loanBindingSource);
                this.userControlModuleRelatedEntryResponsible.bindToData("Loan", "ResponsibleName", "ResponsibleAgentURI", this.loanBindingSource);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Events
        private void dateTimePickerLoanBegin_CloseUp(object sender, EventArgs e)
        {
            //System.Data.DataRow R = this.dataSetLoan.Loan.Rows[0];
            //R["LoanBegin"] = this.dateTimePickerLoanBegin.Value.ToShortDateString();
            //this.textBoxLoanBegin.Text = R["LoanBegin"].ToString();

            //System.Windows.Forms.BindingSource B = new BindingSource(this.dataSetLoan, "Loan");
            //System.Data.DataRowView R = (System.Data.DataRowView)B.Current;
            System.Data.DataRowView R = (System.Data.DataRowView)this.loanBindingSource.Current;
            R["LoanBegin"] = this.dateTimePickerLoanBegin.Value.ToShortDateString();
            this.textBoxLoanBegin.Text = R["LoanBegin"].ToString();
        }

        private void dateTimePickerLoanEnd_CloseUp(object sender, EventArgs e)
        {
            //System.Data.DataRow R = this.dataSetLoan.Loan.Rows[0];
            //R["LoanEnd"] = this.dateTimePickerLoanEnd.Value.ToShortDateString();
            //this.textBoxLoanEnd.Text = R["LoanEnd"].ToString();

            //System.Windows.Forms.BindingSource B = new BindingSource(this.dataSetLoan, "Loan");
            System.Data.DataRowView R = (System.Data.DataRowView)this.loanBindingSource.Current;
            R["LoanEnd"] = this.dateTimePickerLoanEnd.Value.ToShortDateString();
            this.textBoxLoanEnd.Text = R["LoanEnd"].ToString();
        }

        #endregion

        #region Properties
        public int ID 
        { 
            get 
            {
                //System.Windows.Forms.BindingSource B = new BindingSource(this.dataSetLoan, "Loan");
                //return int.Parse(this.dataSetLoan.Loan.Rows[B.Position][0].ToString());

                return int.Parse(this.dataSetLoan.Loan.Rows[this.loanBindingSource.Position][0].ToString());
            } 
        }
        public string DisplayText 
        { 
            get 
            {
                //System.Windows.Forms.BindingSource B = new BindingSource(this.dataSetLoan, "Loan");
                //return this.dataSetLoan.Loan.Rows[B.Position][2].ToString();

                return this.dataSetLoan.Loan.Rows[this.loanBindingSource.Position][2].ToString();
            } 
        }
        //public int ID { get { return int.Parse(this.dataSetLoan.Loan.Rows[this.loanBindingSource.Position][0].ToString()); } }
        //public string DisplayText { get { return this.dataSetLoan.Loan.Rows[this.loanBindingSource.Position][2].ToString(); } }
        public bool ChangeToSpecimen { get { return this.userControlSpecimenList.ChangeToSpecimen; } }
        public int CollectionSpecimenID { get { return this.userControlSpecimenList.CollectionSpecimenID; } }
        #endregion

        #region Sending

        #region Toolstrip
        private void toolStripButtonOpenSendingSchemaFile_Click(object sender, EventArgs e)
        {
            this.setSchemaFile(this.textBoxSendingSchema);
        }

        private void toolStripButtonSendingPreview_Click(object sender, EventArgs e)
        {
            if (this.loanBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.loanBindingSource.Current;
                int ID = int.Parse(R[0].ToString());
                int Count = 0;
                string SQL = "SELECT COUNT(*) AS Count FROM CollectionStorage WHERE LoanID = " + ID.ToString();
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    Count = int.Parse(C.ExecuteScalar().ToString());
                    con.Close();
                    R["InitialNumberOfSpecimen"] = Count;
                    this.textBoxInitialNumberOfSpecimen.Text = Count.ToString();
                    this._Loan.InitialNumberOfSpecimen = Count;
                }
                catch { }
                string P = Folder.Transaction(Folder.TransactionFolder.Loan);
                string Path = Folder.Transaction(Folder.TransactionFolder.Loan) +  "Sending.XML";
                //string Path = System.Windows.Forms.Application.StartupPath + "\\Loan\\Sending.XML";
                System.IO.FileInfo XML = new System.IO.FileInfo(Path);
                System.IO.FileInfo XSLT = new System.IO.FileInfo(this.textBoxSendingSchema.Text);
                this._XmlSending = this._Loan.XmlCorrespondence(XML, XSLT, Loan.LoanCorrespondenceType.Sending, null, ID);
                System.Uri U = new Uri(XML.FullName);
                this.webBrowserSending.Url = U;
            }
        }

        private void toolStripButtonSendingSave_Click(object sender, EventArgs e)
        {
            this.saveHistoryEvent(this.webBrowserSending);
        }

        private void toolStripButtonSendingPrint_Click(object sender, EventArgs e)
        {
            this.webBrowserSending.ShowPrintPreviewDialog();
        }

        #endregion

        #region Scanner

        private void textBoxSendingAccessionNumber_TextChanged(object sender, EventArgs e)
        {
            this.startScannerSending();
        }

        private void textBoxSendingAccessionNumber_MouseEnter(object sender, EventArgs e)
        {
            this.textBoxSendingAccessionNumber.Focus();
            this.textBoxSendingAccessionNumber.SelectAll();
        }

        private void timerScanning_Tick(object sender, EventArgs e)
        {
            if (this.textBoxSendingAccessionNumber.Text.Length > 0)
            {
                if (!this.setLoanSpecimen(this.textBoxSendingAccessionNumber.Text, this.ID))
                {
                    this.timerSending.Stop();
                    System.Windows.Forms.MessageBox.Show("No dataset could be found for the accession number " + this.textBoxSendingAccessionNumber.Text, "No data found");
                    this.textBoxSendingAccessionNumber.Text = "";
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

        #endregion

        #endregion

        #region Confirmation
        #region Toolstrip
        private void toolStripButtonConfirmationOpenSchema_Click(object sender, EventArgs e)
        {
            this.setSchemaFile(this.textBoxConfirmationSchema);
        }

        private void toolStripButtonConfirmationPreview_Click(object sender, EventArgs e)
        {
            if (this.loanBindingSource.Current != null)
            {
                string Path = Folder.Transaction(Folder.TransactionFolder.Loan) +  "Confirmation.XML";
                System.IO.FileInfo XML = new System.IO.FileInfo(Path);
                System.IO.FileInfo XSLT = new System.IO.FileInfo(this.textBoxSendingSchema.Text);
                this._XmlSending = this._Loan.XmlCorrespondence(XML, XSLT, Loan.LoanCorrespondenceType.Confirmation, null, ID);
                System.Uri U = new Uri(XML.FullName);
                this.webBrowserConfirmation.Url = U;
            }
        }

        private void toolStripButtonConfirmationPrint_Click(object sender, EventArgs e)
        {
            this.webBrowserConfirmation.ShowPrintPreviewDialog();
        }

        private void toolStripButtonConfirmationSave_Click(object sender, EventArgs e)
        {
            this.saveHistoryEvent(this.webBrowserConfirmation);
        }
        
        #endregion        
        #endregion

        #region Reminder
        #region Toolstrip
        private void toolStripButtonReminderOpenSchema_Click(object sender, EventArgs e)
        {
            this.setSchemaFile(this.textBoxReminderSchema);
        }

        private void toolStripButtonReminderPreview_Click(object sender, EventArgs e)
        {
            if (this.loanBindingSource.Current != null)
            {
                string Path = Folder.Transaction(Folder.TransactionFolder.Loan) +  "Reminder.XML";
                System.IO.FileInfo XML = new System.IO.FileInfo(Path);
                System.IO.FileInfo XSLT = new System.IO.FileInfo(this.textBoxSendingSchema.Text);
                this._XmlSending = this._Loan.XmlCorrespondence(XML, XSLT, Loan.LoanCorrespondenceType.Reminder, null, ID);
                System.Uri U = new Uri(XML.FullName);
                this.webBrowserReminder.Url = U;
            }
        }

        private void toolStripButtonReminderPrint_Click(object sender, EventArgs e)
        {
            this.webBrowserReminder.ShowPrintPreviewDialog();
        }

        private void toolStripButtonReminderSave_Click(object sender, EventArgs e)
        {
            this.saveHistoryEvent(this.webBrowserReminder);
        }
        
        #endregion        
        #endregion

        #region PartialReturn

        #region Scanner
        private void textBoxPartialReturnAccessionNumber_TextChanged(object sender, EventArgs e)
        {
            this.startScannerReturn();
        }

        private void textBoxPartialReturnAccessionNumber_MouseEnter(object sender, EventArgs e)
        {
            this.textBoxPartialReturnAccessionNumber.Focus();
            this.textBoxPartialReturnAccessionNumber.SelectAll();
        }

        private void timerReturn_Tick(object sender, EventArgs e)
        {
            if (this.textBoxPartialReturnAccessionNumber.Text.Length > 0)
            {
                if (!this.setLoanSpecimen(this.textBoxPartialReturnAccessionNumber.Text, null))
                {
                    this.timerSending.Stop();
                    System.Windows.Forms.MessageBox.Show("No dataset could be found for the accession number " + this.textBoxPartialReturnAccessionNumber.Text, "No data found");
                    this.textBoxPartialReturnAccessionNumber.Text = "";
                }
            }
            this._CurrentScanStepReturn = ScanStep.Idle;
            this.textBoxPartialReturnAccessionNumber.SelectAll();
            this.timerReturn.Stop();
        }

        private void startScannerReturn()
        {
            if (this._CurrentScanStepReturn == ScanStep.Idle)
            {
                this._CurrentScanStepReturn = ScanStep.Scanning;
                this.timerReturn.Start();
            }
        }
        
        #endregion

        #region Manuell input of specimen
        private void comboBoxPartialReturn_SelectionChangeCommitted(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxPartialReturn.SelectedItem;
            System.Data.DataRow RCS = this._DataSetLoanForReturn.CollectionStorage.NewCollectionStorageRow();
            RCS["CollectionSpecimenID"] = R["CollectionSpecimenID"];
            RCS["CollectionID"] = R["CollectionID"];
            RCS["MaterialCategory"] = R["MaterialCategory"];
            RCS["StorageLocation"] = R["StorageLocation"];
            this._DataSetLoanForReturn.CollectionStorage.Rows.Add(RCS);
            R.BeginEdit();
            R.Delete();
            R.EndEdit();
        }

        private void comboBoxPartialReturn_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxPartialReturn.DataSource == null)
            {
                this.comboBoxPartialReturn.DataSource = this._Loan.SpecimenListForPartialReturn;
                this.comboBoxPartialReturn.DisplayMember = "AccessionNumber";
            }
        }
        
        #endregion

        #region toolStrip
        private void toolStripButtonPartialReturnOpenSchema_Click(object sender, EventArgs e)
        {
            this.setSchemaFile(this.textBoxPartialReturnSchema);
        }

        private void toolStripButtonPartialReturnPreview_Click(object sender, EventArgs e)
        {
            if (this.loanBindingSource.Current != null)
            {
                string Path = Folder.Transaction(Folder.TransactionFolder.Loan) +  "PartialReturn.XML";
                System.IO.FileInfo XML = new System.IO.FileInfo(Path);
                System.IO.FileInfo XSLT = new System.IO.FileInfo(this.textBoxSendingSchema.Text);
                this._XmlSending = this._Loan.XmlCorrespondence(XML, XSLT, Loan.LoanCorrespondenceType.PartialReturn, null, ID);
                System.Uri U = new Uri(XML.FullName);
                this.webBrowserPartialReturn.Url = U;
            }
        }

        private void toolStripButtonPartialReturnPrint_Click(object sender, EventArgs e)
        {
            this.webBrowserPartialReturn.ShowPrintPreviewDialog();
        }

        private void toolStripButtonPartialReturnSave_Click(object sender, EventArgs e)
        {
            this.saveHistoryEvent(this.webBrowserPartialReturn);
        }
        
        #endregion   

        #endregion

        #region Return
        private void toolStripButtonReturnOpenSchema_Click(object sender, EventArgs e)
        {
            this.setSchemaFile(this.textBoxReturnSchema);
        }

        private void toolStripButtonReturnOK_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonReturnPreview_Click(object sender, EventArgs e)
        {
            if (this.loanBindingSource.Current != null)
            {
                string Path = Folder.Transaction(Folder.TransactionFolder.Loan) +  "Return.XML";
                System.IO.FileInfo XML = new System.IO.FileInfo(Path);
                System.IO.FileInfo XSLT = new System.IO.FileInfo(this.textBoxSendingSchema.Text);
                this._XmlSending = this._Loan.XmlCorrespondence(XML, XSLT, Loan.LoanCorrespondenceType.Return, null, ID);
                System.Uri U = new Uri(XML.FullName);
                this.webBrowserReturn.Url = U;
            }
        }

        private void toolStripButtonReturnPrint_Click(object sender, EventArgs e)
        {
            this.webBrowserReturn.ShowPrintPreviewDialog();
        }

        private void toolStripButtonReturnSave_Click(object sender, EventArgs e)
        {
            this.saveHistoryEvent(this.webBrowserReturn);
        }
        
        #endregion

        #region History
        private void listBoxLoanHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.loanHistoryBindingSource.Position = this.listBoxLoanHistory.SelectedIndex;
            this.setHistoryControls();
        }

        private void buttonLoanHistoryInsertDocument_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Clipboard.ContainsImage())
            {
                //System.Windows.Forms.BindingSource B = new BindingSource(this.dataSetLoan, "LoanHistory");
                this.splitContainerLoanHistoryData.Panel1Collapsed = false;
                System.Data.DataRowView R = (System.Data.DataRowView)this.loanHistoryBindingSource.Current;
                try
                {
                    System.Drawing.Image I = System.Windows.Forms.Clipboard.GetImage();
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    I.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    System.Byte[] B = (System.Byte[])ms.ToArray();
                    R["LoanDocument"] = B;
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("No image in clipboard");
        }

        private void buttonLoanHistoryDeleteDocument_Click(object sender, EventArgs e)
        {

        }

        private void setHistoryControls()
        {
            if (this.listBoxLoanHistory.Items.Count > 0 && this.listBoxLoanHistory.SelectedIndex > -1)
            {
                // items present and selected
                this.splitContainerLoanHistoryData.Enabled = true;
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxLoanHistory.SelectedItem;
                if ((!R["LoanText"].Equals(System.DBNull.Value) && R["LoanText"].ToString().Length > 0) ||
                    (!R["LoanDocument"].Equals(System.DBNull.Value) && R["LoanDocument"].ToString().Length > 0))
                {
                    // text or image documents present
                    this.splitContainerLoanHistoryData.Panel1Collapsed = false;
                    if (!R["LoanText"].Equals(System.DBNull.Value) && R["LoanText"].ToString().Length > 0)
                    {
                        // text document present
                        this.splitContainerLoanHistoryDocuments.Panel1Collapsed = false;
                        this.webBrowserLoanHistory.DocumentText = R["LoanText"].ToString();
                    }
                    else
                    {
                        // no text document
                        this.splitContainerLoanHistoryDocuments.Panel1Collapsed = true;
                    }
                    if (!R["LoanDocument"].Equals(System.DBNull.Value) && R["LoanDocument"].ToString().Length > 0)
                    {
                        // image document present
                        this.splitContainerLoanHistoryDocuments.Panel2Collapsed = false;
                        // getting the image
                        System.Byte[] B = (System.Byte[])R["LoanDocument"];
                        System.IO.MemoryStream ms = new System.IO.MemoryStream(B);
                        System.Drawing.Image I = System.Drawing.Image.FromStream(ms);
                        this.pictureBoxLoanHistory.Image = I;
                        this.buttonLoanHistoryInsertDocument.Visible = false;
                    }
                    else
                    {
                        // no image document
                        this.splitContainerLoanHistoryDocuments.Panel2Collapsed = true;
                        this.buttonLoanHistoryInsertDocument.Visible = true;
                    }
                }
                else
                {
                    // no documents
                    this.splitContainerLoanHistoryData.Panel1Collapsed = true;
                    this.buttonLoanHistoryInsertDocument.Visible = true;
                }
                this.splitContainerLoanHistoryData.Panel2Collapsed = false;
            }
            else
            {
                // no items present or none selected
                this.splitContainerLoanHistoryData.Enabled = false;
            }
        }

        #region Toolstrip
        private void toolStripButtonLoanHistoryNew_Click(object sender, EventArgs e)
        {
            System.Data.DataRow R = this._Loan.DataSet.Tables["LoanHistory"].NewRow();
            R["LoanID"] = ID;
            R["Date"] = System.DateTime.Now;
            R["InternalNotes"] = " ";
            this._Loan.DataSet.Tables["LoanHistory"].Rows.Add(R);
            this.setHistoryControls();
            //this._Loan.setTabControl();
        }

        private void toolStripButtonLoanHistoryDelete_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.loanHistoryBindingSource.Current;
            if (R["LoanText"].Equals(System.DBNull.Value) && R["LoanDocument"].Equals(System.DBNull.Value))
                R.Delete();
            else
                System.Windows.Forms.MessageBox.Show("You can not delete a entry with documents");
        }
        
        private void toolStripButtonHistoryZoomAdapt_Click(object sender, EventArgs e)
        {
            this.pictureBoxLoanHistory.Dock = DockStyle.Fill;
            this.pictureBoxLoanHistory.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void toolStripButtonHistoryZoom100Percent_Click(object sender, EventArgs e)
        {
            this.pictureBoxLoanHistory.Dock = DockStyle.None;
            this.pictureBoxLoanHistory.SizeMode = PictureBoxSizeMode.Normal;
            this.pictureBoxLoanHistory.Width = this.pictureBoxLoanHistory.Image.Width;
            this.pictureBoxLoanHistory.Height = this.pictureBoxLoanHistory.Image.Height;
        }

        private void toolStripButtonHistoryRemove_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.loanHistoryBindingSource.Current;
            R["LoanDocument"] = System.DBNull.Value;
        }


        #endregion
        #endregion

        #region Auxilliary functions

        private bool setLoanSpecimen(string AccessionNumber, int? LoanID)
        {
            bool OK = false;
            string SQL = "";
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                SQL = "SELECT CollectionSpecimenID FROM CollectionSpecimen WHERE AccessionNumber = '" + AccessionNumber + "'";
                con.Open();
                int CollectionSpecimenID = int.Parse(C.ExecuteScalar().ToString());
                SQL = "UPDATE CollectionStorage SET LoanID = ";
                if (LoanID == null) SQL += "NULL";
                else SQL += LoanID.ToString();
                SQL += " WHERE CollectionSpecimenID = " + CollectionSpecimenID.ToString();
                C.ExecuteNonQuery();
                con.Close();
                this._Loan.setSpecimenList(this.ID);
                OK = true;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        private void setSchemaFile(System.Windows.Forms.TextBox TextBox)
        {
            string Path = Folder.Transaction(Folder.TransactionFolder.LoanSchemas);
            //string Path = System.Windows.Forms.Application.StartupPath + "\\Loan\\Schemas";
            if (!System.IO.File.Exists(Path))
            {
                System.IO.Directory.CreateDirectory(Path);
            }
            Path = Path + "\\Loan.xslt";
            if (!System.IO.File.Exists(Path))
            {
                System.IO.FileInfo f = new System.IO.FileInfo(Path);
                this._Loan.writeDefaultXslt(f);
            }
            this.openFileDialogSendingSchema = new OpenFileDialog();
            this.openFileDialogSendingSchema.RestoreDirectory = true;
            this.openFileDialogSendingSchema.Multiselect = false;
            this.openFileDialogSendingSchema.InitialDirectory = Folder.Transaction(Folder.TransactionFolder.LoanSchemas);
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

        private void saveHistoryEvent(System.Windows.Forms.WebBrowser WebBrowser)
        {
            System.Data.DataRowView RL = (System.Data.DataRowView)this.loanBindingSource.Current;
            int ID = int.Parse(RL[0].ToString());
            System.Data.DataRow R = this._Loan.DataSet.Tables["LoanHistory"].NewRow();
            R["LoanID"] = ID;
            R["Date"] = System.DateTime.Now;
            R["LoanText"] = WebBrowser.DocumentText;
            this._Loan.DataSet.Tables["LoanHistory"].Rows.Add(R);
            this.setHistoryControls();
            this._Loan.setTabControl();
        }
 
	    #endregion    




   }
}