using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityCollection.UserControls
{
    public partial class UserControl_Regulation : UserControl__Data
    {

        #region Construction
        public UserControl_Regulation(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this._HelpNamespace = HelpNamespace;
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }

        #endregion

        #region Interface

        public override void SetPosition(int Position)
        {
            base.SetPosition(Position);
            //if (this.groupBoxTransaction.Text.Length == 0) this.groupBoxTransaction.Text = "Transaction";
            //this.groupBoxTransaction.ForeColor = System.Drawing.Color.Brown;
            //this.pictureBoxTransaction.Image = DiversityCollection.Specimen.getImage(Specimen.OverviewImage.Transaction);
            //if (this.groupBoxTransaction.Text.Length == 0)
            //{
            //    System.Collections.Generic.Dictionary<string, string> dict = DiversityWorkbench.Entity.EntityInformation("CollTransactionType_Enum.Code.loan");
            //    this.groupBoxTransaction.Text = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, dict);
            //}
            //bool IsManager = this.FormFunctions.getObjectPermissions("ManagerCollectionList", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Select);
            //bool IsManagerOfCurrentTransaction = false;
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                if (!R["TransactionID"].Equals(System.DBNull.Value))
                {
                    DocumentType type = DocumentType.Missing;
                    string SQL = "SELECT D.TransactionID, D.Date, D.TransactionText, D.TransactionDocument, D.DisplayText, D.DocumentURI FROM TransactionDocument D " +
                        "INNER JOIN [TransactionList] L ON L.TransactionID = D.TransactionID AND D.TransactionID = " + R["TransactionID"].ToString() + " AND L.[TransactionType] = 'regulation' ";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    _DtTransactionDocument = new DataTable("TransactionDocument");
                    ad.Fill(_DtTransactionDocument);
                    if(_DtTransactionDocument.Rows.Count == 0)
                    {
                        SQL = "SELECT D.TransactionID, D.Date, '<html><body><span style=\"color: #FF0000; font-weight: bold; font-size: large;\"><b>NO ACCESS</b></body></html>' AS TransactionText, cast(NULL as image) AS TransactionDocument, NULL AS DisplayText, NULL AS DocumentURI FROM TransactionDocument D " +
                                "INNER JOIN [Transaction] L ON L.TransactionID = D.TransactionID AND D.TransactionID = " + R["TransactionID"].ToString() + " AND L.[TransactionType] = 'regulation' ";
                        ad.SelectCommand.CommandText = SQL;
                        ad.Fill(_DtTransactionDocument);
                        if(_DtTransactionDocument.Rows.Count > 0)
                            type = DocumentType.NoPermission;
                    }
                    this.setDocumentControls();

                    int TransactionID;
                    if (int.TryParse(R["TransactionID"].ToString(), out TransactionID))
                        this.setTransaction(TransactionID);

                    this.setDocument(0, type);
                    this.dateTimePickerBeginDate.Visible = true;
                }
                else
                {
                    this.splitContainerTransactionDocuments.Panel2Collapsed = true;
                    this.dateTimePickerBeginDate.Visible = false;
                }
                //int TransationID = int.Parse(R["TransactionID"].ToString());
                //string TransactionType = DiversityCollection.LookupTable.TransactionType(TransationID);
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public System.Windows.Forms.TableLayoutPanel TableLayoutPanelMain
        {
            get { return this.tableLayoutPanelMain; }
        }

        #endregion


        #region Control

        private enum DocumentType { Undefined, Missing, NoPermission, Resource, Screenshot }

        private int _Position = 0;

        private System.Data.DataTable _DtTransactionDocument;

        private void initControl()
        {

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();
        }

        private void setDocument(int i, DocumentType type = DocumentType.Undefined)
        {
            this.dateTimePickerBeginDate.Enabled = false;
            this.dateTimePickerAgreedEndDate.Enabled = false;
            this.textBoxInternalNotes.ReadOnly = true;
            this.textBoxResponsibleName.ReadOnly = true;

            if (type == DocumentType.NoPermission)
            {
                setDocumentType(type); 
                _DtTransactionDocument.Clear();
                return;
            }

            if (_DtTransactionDocument.Rows.Count > 0 && this._DtTransactionDocument.Rows.Count > i)// && this.bindingNavigatorTransaction.Items.Count > 0)
            {
                // items present and selected
                System.Data.DataRow R = this._DtTransactionDocument.Rows[i];
                //System.DateTime date;
                //if (System.DateTime.TryParse(R["Date"].ToString(), out date))
                //    this.dateTimePickerBeginDate.Value = date;
                if ((!R["TransactionText"].Equals(System.DBNull.Value) && R["TransactionText"].ToString().Length > 0) ||
                    (!R["TransactionDocument"].Equals(System.DBNull.Value) && R["TransactionDocument"].ToString().Length > 0) ||
                    (!R["DocumentURI"].Equals(System.DBNull.Value) && R["DocumentURI"].ToString().Length > 0))
                {
                    type = DocumentType.Resource;
                    this.webBrowserTransactionDocuments.Visible = true;
                    type = DocumentType.Resource;
                    // text or image documents present
                    if (!R["TransactionText"].Equals(System.DBNull.Value) && R["TransactionText"].ToString().Length > 0)
                    {
                        // text document present
                        //this.splitContainerTransactionDocuments.Panel1Collapsed = false;
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
                                //this.buttonTransactionDocumentOpenPdf.Visible = true;
                                //this.buttonTransactionDocumentOpenPdf.Text = "      Open " + R["DocumentURI"].ToString();
                                this.webBrowserTransactionDocuments.Url = new Uri("about:blank");
                                this.webBrowserTransactionDocuments.Visible = false;
                                if (DiversityCollection.Forms.FormTransactionSettings.Default.PdfInBrowserControl)
                                {
                                    try
                                    {
                                        this.webBrowserTransactionDocuments.Navigate(R["DocumentURI"].ToString());
                                    }
                                    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
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
                        type = DocumentType.Missing;
                        //this.splitContainerTransactionDocuments.Panel1Collapsed = true;
                    }
                    if (!R["TransactionDocument"].Equals(System.DBNull.Value) && R["TransactionDocument"].ToString().Length > 0)
                    {
                        // image document present
                        try
                        {
                            // getting the image
                            System.Byte[] B = (System.Byte[])R["TransactionDocument"];
                            System.IO.MemoryStream ms = new System.IO.MemoryStream(B);
                            System.Drawing.Image I = System.Drawing.Image.FromStream(ms);
                            this.pictureBoxTransactionDocuments.Image = I;
                            this.splitContainerTransactionDocuments.Panel2Collapsed = false;
                            type = DocumentType.Screenshot;
                        }
                        catch(System.Exception ex)
                        {
                            this.splitContainerTransactionDocuments.Panel2Collapsed = true;
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                    else
                    {
                        // no image document
                        this.splitContainerTransactionDocuments.Panel2Collapsed = true;
                    }
                }
                else
                {
                    // no documents
                    type = DocumentType.Missing;
                    //this.splitContainerDocumentBrowser.Panel1Collapsed = true;
                    //this.splitContainerDocumentBrowser.Panel2Collapsed = false;
                }
            }
            else
            {
                // no items present or none selected
                this.pictureBoxTransactionDocuments.Image = null;
                type = DocumentType.Missing;
            }
            this.setDocumentType(type);
        }

        private int _TransactionID;
        private int _AdministratingCollectionID;

        private void setTransaction(int TransactionID)
        {
            if (TransactionID == _TransactionID)
                return;
            _TransactionID = TransactionID;
            try
            {
                System.Data.DataRow[] R = LookupTable.DtTransaction.Select("TransactionID = " + TransactionID.ToString());
                if (R.Length == 1)
                {
                    // TransactionTitle
                    if (R[0]["TransactionTitle"].Equals(System.DBNull.Value))
                        this.labelTransactionTitle.Text = "";
                    else
                        this.labelTransactionTitle.Text = R[0]["TransactionTitle"].ToString();

                    System.DateTime date;

                    // BeginDate
                    if (System.DateTime.TryParse(R[0]["BeginDate"].ToString(), out date))
                    {
                        this.dateTimePickerBeginDate.Value = date;
                        this.dateTimePickerBeginDate.CustomFormat = "yyyy-MM-dd";
                    }
                    else this.dateTimePickerBeginDate.CustomFormat = "--";

                    // AgreedEndDate
                    if (System.DateTime.TryParse(R[0]["AgreedEndDate"].ToString(), out date))
                    {
                        this.dateTimePickerAgreedEndDate.Value = date;
                        this.dateTimePickerAgreedEndDate.CustomFormat = "yyyy-MM-dd";
                    }
                    else this.dateTimePickerAgreedEndDate.CustomFormat = "--";

                    // ResponsibleName
                    if (R[0]["ResponsibleName"].Equals(System.DBNull.Value))
                        this.textBoxResponsibleName.Text = "";
                    else
                        this.textBoxResponsibleName.Text = R[0]["ResponsibleName"].ToString();

                    // InternalNotes
                    if (R[0]["InternalNotes"].Equals(System.DBNull.Value))
                        this.textBoxInternalNotes.Text = "";
                    else
                        this.textBoxInternalNotes.Text = R[0]["InternalNotes"].ToString();

                    // AdministratingCollectionID
                    if (int.TryParse(R[0]["AdministratingCollectionID"].ToString(), out _AdministratingCollectionID))
                    {
                        this.labelAdministratingCollectionID.Text = LookupTable.CollectionName(_AdministratingCollectionID);
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void setDocumentType(DocumentType type)
        {
            switch (type)
            {
                case DocumentType.Missing:
                    this.labelMessage.Text = "No documents available";
                    this.splitContainerTransactionDocuments.Panel1Collapsed = false;
                    this.splitContainerTransactionDocuments.Panel2Collapsed = true;
                    this.splitContainerDocumentBrowser.Panel2Collapsed = false;
                    this.splitContainerDocumentBrowser.Panel1Collapsed = true;
                    break;
                case DocumentType.NoPermission:
                    this.labelMessage.Text = "No permission";
                    this.splitContainerTransactionDocuments.Panel1Collapsed = false;
                    this.splitContainerTransactionDocuments.Panel2Collapsed = true;
                    this.splitContainerDocumentBrowser.Panel2Collapsed = false;
                    this.splitContainerDocumentBrowser.Panel1Collapsed = true;
                    break;
                case DocumentType.Resource:
                    this.splitContainerTransactionDocuments.Panel1Collapsed = false;
                    this.splitContainerTransactionDocuments.Panel2Collapsed = true;
                    this.splitContainerDocumentBrowser.Panel2Collapsed = true;
                    this.splitContainerDocumentBrowser.Panel1Collapsed = false;
                    break;
                case DocumentType.Screenshot:
                    this.splitContainerTransactionDocuments.Panel2Collapsed = false;
                    this.splitContainerTransactionDocuments.Panel1Collapsed = true;
                    break;
            }
        }

        //private System.Windows.Forms.Binding _bindingTransactionDocument;

        private void setDocumentControls()
        {
            try
            {
                if (this._DtTransactionDocument == null)
                    _DtTransactionDocument = new DataTable();
                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = _DtTransactionDocument;
                bindingNavigatorTransaction.BindingSource = bindingSource;
                this.bindingNavigatorDeleteItem.Visible = false;
                this.bindingNavigatorAddNewItem.Visible = false;
                this.bindingNavigatorMoveFirstItem.Visible = false;
                this.bindingNavigatorMoveLastItem.Visible = false;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            if (_DtTransactionDocument.Rows.Count > _Position + 1)
                _Position++;
            this.setDocument(_Position);
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            if (_DtTransactionDocument.Rows.Count > 0 && _Position > 0)
                _Position--;
            this.setDocument(_Position);
        }

        private void buttonAdministratingCollectionID_Click(object sender, EventArgs e)
        {
            if (DiversityWorkbench.Forms.FormFunctions.Permissions("CollectionManager", DiversityWorkbench.Forms.FormFunctions.Permission.DELETE))
            {
                DiversityCollection.Forms.FormManagers f = new Forms.FormManagers();
                f.ShowDialog();
            }
            else
                System.Windows.Forms.MessageBox.Show("You do not have sufficient permissions to edit the collection managers.\r\nPlease turn to your administrator", "Insufficient permissions", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        #endregion

    }
}
