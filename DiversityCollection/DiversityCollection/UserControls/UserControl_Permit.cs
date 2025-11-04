using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.UserControls
{
    public partial class UserControl_Permit : UserControl__Data
    {

        #region Parameter

        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterTransaction;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterTransactionDocument;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterTransactionAgent;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterTransactionIdentifier;

        #endregion

        #region Construction

        public UserControl_Permit(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this._Source = Source;
            this._HelpNamespace = HelpNamespace;
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }

        #endregion

        #region Interface

        public void setTransaction(System.Data.DataRow R)// int TransactionID)
        {
            try
            {
                string SQL = "SELECT " + DiversityCollection.Transaction.SqlFieldsPermit + " FROM TransactionPermit " +
                    "WHERE TransactionID = " + R["TransactionID"].ToString();
                this._SqlDataAdapterTransaction = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                this._SqlDataAdapterTransaction.Fill(this.dataSetTransaction.Transaction);
                if (this.dataSetTransaction.Transaction.Rows.Count == 1)
                {
                    SQL = "SELECT " + DiversityCollection.Transaction.SqlFieldsTransactionDocuments + " FROM TransactionDocument " +
                    "WHERE TransactionID = " + R["TransactionID"].ToString();
                    this._SqlDataAdapterTransactionDocument = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    this._SqlDataAdapterTransactionDocument.Fill(this.dataSetTransaction.TransactionDocument);
                    if (this.dataSetTransaction.TransactionDocument.Rows.Count == 0 && this.tabControlTransaction.TabPages.Contains(this.tabPageDocuments))
                        this.tabControlTransaction.TabPages.Remove(this.tabPageDocuments);
                    else
                    {
                        if (!this.tabControlTransaction.TabPages.Contains(this.tabPageDocuments))
                            this.tabControlTransaction.TabPages.Add(this.tabPageDocuments);
                    }

                    this.tabControlTransaction.TabPages.Remove(this.tabPageIdentifier);
                    this.tabControlTransaction.TabPages.Remove(this.tabPagePayment);
                    this.tabControlTransaction.TabPages.Remove(this.tabPageAgents);
                    this.tabControlTransaction.TabPages.Remove(this.tabPageDocuments);

                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public System.Data.DataSet DataSet() { return this.dataSetTransaction; }

        public System.Windows.Forms.TabControl TabControl() { return this.tabControlTransaction; }

        public System.Windows.Forms.BindingSource BindingSource() { return this.transactionBindingSource; }

        public System.Windows.Forms.TableLayoutPanel TableLayoutPanelAgents { get { return this.tableLayoutPanelAgents; } }

        public System.Windows.Forms.TableLayoutPanel TableLayoutPanelPayment { get { return this.tableLayoutPanelPayment; } }

        public System.Windows.Forms.TableLayoutPanel TableLayoutPanelIdentifier { get { return this.tableLayoutPanelIdentifier; } }

        public System.Windows.Forms.TableLayoutPanel TableLayoutPanelTransactionDocument { get { return this.tableLayoutPanelTransactionDocument; } }

        #endregion

        #region Private events

        private void initControl()
        {
            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();
        }

        private void listBoxTransactionDocuments_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxTransactionDocuments.SelectedIndex > -1)
            {
                this.splitContainerDocuments.Visible = true;
                this.tableLayoutPanelTransactionDocument.Visible = true;
                this.setDocumentControls();
            }
        }

        private void setDocumentControls()
        {
            try
            {
                if (this.listBoxTransactionDocuments.Items.Count > 0 && this.listBoxTransactionDocuments.SelectedIndex > -1)
                {
                    // items present and selected
                    this.splitContainerTransactionDocumentData.Enabled = true;
                    this.splitContainerTransactionDocuments.Visible = true;
                    System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxTransactionDocuments.SelectedItem;
                    if ((!R["TransactionText"].Equals(System.DBNull.Value) && R["TransactionText"].ToString().Length > 0) ||
                        (!R["TransactionDocument"].Equals(System.DBNull.Value) && R["TransactionDocument"].ToString().Length > 0) ||
                        (!R["DocumentURI"].Equals(System.DBNull.Value) && R["DocumentURI"].ToString().Length > 0))
                    {
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
                            this.webBrowserTransactionDocuments.Url = U;
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
                        this.splitContainerTransactionDocumentData.Panel1Collapsed = true;
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

        #endregion


    }
}
