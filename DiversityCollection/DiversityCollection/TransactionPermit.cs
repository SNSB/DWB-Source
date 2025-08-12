using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection
{
    class TransactionPermit : Transaction
    {

        private System.Windows.Forms.TabControl _TabControl;
        private DiversityCollection.UserControls.UserControlPermit _UCPermit;

        #region Construction

        public TransactionPermit(
            ref System.Data.DataSet Dataset,
            System.Data.DataTable DataTable, 
            ref System.Windows.Forms.TreeView TreeView, 
            System.Windows.Forms.Form Form,
            DiversityWorkbench.UserControls.UserControlQueryList UserControlQueryList,
            System.Windows.Forms.SplitContainer SplitContainerMain,
            System.Windows.Forms.SplitContainer SplitContainerData,
            System.Windows.Forms.ToolStripButton ToolStripButtonSpecimenList,
            DiversityCollection.UserControls.UserControlSpecimenList UserControlSpecimenList,
            System.Windows.Forms.HelpProvider HelpProvider,
            System.Windows.Forms.ToolTip ToolTip, 
            ref System.Windows.Forms.BindingSource BindingSource,
            System.Windows.Forms.TabControl TabControl)
            : base(
            ref Dataset, 
            DataTable, 
            ref TreeView, 
            Form, 
            UserControlQueryList, 
            SplitContainerMain,
            SplitContainerData, 
            ToolStripButtonSpecimenList, 
            UserControlSpecimenList,
            HelpProvider, 
            ToolTip,
            ref BindingSource, 
            TabControl)
        {
            try
            {
                this._sqlItemFieldList = DiversityCollection.Transaction.SqlFieldsTransaction;
                this._SpecimenTable = "CollectionSpecimenTransaction";
                this._MainTable = "[Permit]";
                this._TabControl = TabControl;
            }
            catch (System.Exception ex)
            {
            }
        }
        
        #endregion

        #region Interface

        public void SetUserControlPermit(DiversityCollection.UserControls.UserControlPermit UCP)
        {
            this._UCPermit = UCP;
        }
        
        public override System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions
        {
            get
            {
                System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

                string Description = this.FormFunctions.ColumnDescription("[Transaction]", "TransactionTitle");
                DiversityWorkbench.QueryCondition qTransactionTitle = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "TransactionTitle", "Transaction", "Name", "Name", Description);
                QueryConditions.Add(qTransactionTitle);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "TransactionID");
                DiversityWorkbench.QueryCondition qTransactionID = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "TransactionID", "Transaction", "ID", "Transaction ID", Description);
                QueryConditions.Add(qTransactionID);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "ParentTransactionID");
                DiversityWorkbench.QueryCondition qParentTransactionID = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "ParentTransactionID", "Transaction", "ParentID", "Parent ID", Description);
                QueryConditions.Add(qParentTransactionID);

                //Description = this.FormFunctions.ColumnDescription("[Transaction]", "TransactionType");
                //DiversityWorkbench.QueryCondition qTransactionType = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "TransactionType", "Transaction", "Type", "Transaction type", Description, "CollTransactionType_Enum", DiversityWorkbench.Settings.DatabaseName);
                //QueryConditions.Add(qTransactionType);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "BeginDate");
                DiversityWorkbench.QueryCondition qBeginDate = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "BeginDate", "Transaction", "Begin", "Begin", Description, true);
                QueryConditions.Add(qBeginDate);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "AgreedEndDate");
                DiversityWorkbench.QueryCondition qAgreedEndDate = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "AgreedEndDate", "Transaction", "End", "End", Description, true);
                QueryConditions.Add(qAgreedEndDate);

                //Description = this.FormFunctions.ColumnDescription("[Transaction]", "TransactionComment");
                //DiversityWorkbench.QueryCondition qTransactionComment = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "TransactionComment", "Transaction", "Comment", "Comment", Description);
                //QueryConditions.Add(qTransactionComment);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "InternalNotes");
                DiversityWorkbench.QueryCondition qInternalNotes = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "InternalNotes", "Transaction", "Notes", "Notes", Description);
                QueryConditions.Add(qInternalNotes);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "FromTransactionNumber");
                DiversityWorkbench.QueryCondition qFromTransactionNumber = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "FromTransactionNumber", "From", "Number", "Number", Description);
                QueryConditions.Add(qFromTransactionNumber);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "FromTransactionPartnerName");
                DiversityWorkbench.QueryCondition qFromTransactionPartnerName = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "FromTransactionPartnerName", "From", "Partner", "Transaction partner", Description);
                QueryConditions.Add(qFromTransactionPartnerName);

                //System.Data.DataTable dtCollectionFrom = new System.Data.DataTable();
                //System.Data.DataTable dtCollectionTo = new System.Data.DataTable();
                //string SQL = "SELECT NULL AS [Value], NULL AS Display UNION " +
                //    "SELECT CollectionID AS [Value], CollectionName AS Display " +
                //    "FROM Collection " +
                //    "ORDER BY Display ";
                //Microsoft.Data.SqlClient.SqlDataAdapter aColl = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                //if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                //{
                //    try
                //    {
                //        aColl.Fill(dtCollectionFrom);
                //        aColl.Fill(dtCollectionTo);
                //    }
                //    catch { }
                //}
                //if (dtCollectionFrom.Columns.Count == 0)
                //{
                //    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                //    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                //    dtCollectionFrom.Columns.Add(Value);
                //    dtCollectionFrom.Columns.Add(Display);
                //}
                //Description = DiversityWorkbench.Functions.ColumnDescription("Collection", "CollectionName");
                //DiversityWorkbench.QueryCondition qCollectionFrom = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "FromCollectionID", "From", "Collection", "Collection", Description, dtCollectionFrom, true);
                //QueryConditions.Add(qCollectionFrom);
                //DiversityWorkbench.QueryCondition qCollectionTo = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "ToCollectionID", "To", "Collection", "Collection", Description, dtCollectionTo, true);
                //QueryConditions.Add(qCollectionTo);

                //Description = this.FormFunctions.ColumnDescription("[Transaction]", "ToTransactionNumber");
                //DiversityWorkbench.QueryCondition qToTransactionNumber = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "ToTransactionNumber", "To", "Number", "Number", Description);
                //QueryConditions.Add(qToTransactionNumber);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "ToTransactionPartnerName");
                DiversityWorkbench.QueryCondition qToTransactionPartnerName = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "ToTransactionPartnerName", "To", "Partner", "Transaction partner", Description);
                QueryConditions.Add(qToTransactionPartnerName);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "MaterialDescription");
                DiversityWorkbench.QueryCondition qMaterialDescription = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "MaterialDescription", "Material", "Description", "Material description", Description);
                QueryConditions.Add(qMaterialDescription);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "MaterialSource");
                DiversityWorkbench.QueryCondition qMaterialSource = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "MaterialSource", "Material", "Source", "Material source", Description);
                QueryConditions.Add(qMaterialSource);

                //Description = this.FormFunctions.ColumnDescription("[Transaction]", "MaterialCategory");
                //DiversityWorkbench.QueryCondition qMaterialCategory = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "MaterialCategory", "Material", "Category", "Material category", Description);
                //QueryConditions.Add(qMaterialCategory);

                //Description = this.FormFunctions.ColumnDescription("[Transaction]", "LogCreatedBy");
                //DiversityWorkbench.QueryCondition qTransactionLogCreatedBy = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "LogCreatedBy", "Logging", "Creator", "Log created by", Description);
                //QueryConditions.Add(qTransactionLogCreatedBy);

                //Description = this.FormFunctions.ColumnDescription("[Transaction]", "LogCreatedWhen");
                //DiversityWorkbench.QueryCondition qTransactionLogCreatedWhen = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "LogCreatedWhen", "Logging", "Cre.date", "Log created when", Description, true);
                //QueryConditions.Add(qTransactionLogCreatedWhen);

                //Description = this.FormFunctions.ColumnDescription("[Transaction]", "LogUpdatedBy");
                //DiversityWorkbench.QueryCondition qTransactionLogUpdatedBy = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "LogUpdatedBy", "Logging", "Updator", "Log Updated by", Description);
                //QueryConditions.Add(qTransactionLogUpdatedBy);

                //Description = this.FormFunctions.ColumnDescription("[Transaction]", "LogUpdatedWhen");
                //DiversityWorkbench.QueryCondition qTransactionLogUpdatedWhen = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "LogUpdatedWhen", "Logging", "Upd.date", "Log Updated when", Description, true);
                //QueryConditions.Add(qTransactionLogUpdatedWhen);

                Description = this.FormFunctions.ColumnDescription("[TransactionAgent]", "AgentName");
                DiversityWorkbench.QueryCondition qTransactionAgentName = new DiversityWorkbench.QueryCondition(true, "[TransactionAgent]", "TransactionID", "AgentName", "Agent", "Name of the agent", "Agent", Description);
                QueryConditions.Add(qTransactionAgentName);

                Description = this.FormFunctions.ColumnDescription("[TransactionAgent]", "AgentRole");
                DiversityWorkbench.QueryCondition qTransactionAgentRole = new DiversityWorkbench.QueryCondition(true, "[TransactionAgent]", "TransactionID", "AgentRole", "Agent", "Role of the agent", "Role", Description);
                QueryConditions.Add(qTransactionAgentRole);

                //Description = this.FormFunctions.ColumnDescription("[TransactionPayment]", "Amount");
                //DiversityWorkbench.QueryCondition qTransactionPaymentAmount = new DiversityWorkbench.QueryCondition(true, "[TransactionPayment]", "TransactionID", "Amount", "Payment", "Amount of the payment", "Amount", Description);
                //QueryConditions.Add(qTransactionPaymentAmount);

                //Description = this.FormFunctions.ColumnDescription("[TransactionPayment]", "ForeignCurrency");
                //DiversityWorkbench.QueryCondition qTransactionPaymentForeignCurrency = new DiversityWorkbench.QueryCondition(true, "[TransactionPayment]", "TransactionID", "ForeignCurrency", "Payment", "Foreign currency", "For.curr.", Description);
                //QueryConditions.Add(qTransactionPaymentForeignCurrency);

                //Description = this.FormFunctions.ColumnDescription("[TransactionPayment]", "Identifier");
                //DiversityWorkbench.QueryCondition qTransactionPaymentIdentifier = new DiversityWorkbench.QueryCondition(true, "[TransactionPayment]", "TransactionID", "Identifier", "Payment", "Identifier", "Identifier", Description);
                //QueryConditions.Add(qTransactionPaymentIdentifier);

                //System.Collections.Generic.Dictionary<string, DiversityWorkbench.ReferencingTableLink> IdentifierLinks = new Dictionary<string, DiversityWorkbench.ReferencingTableLink>();
                //DiversityWorkbench.ReferencingTableLink IDReference = new DiversityWorkbench.ReferencingTableLink();
                //IDReference.ReferencedTable = "Transaction";
                //IDReference.ReferencedColumn = "TransactionID";
                //IDReference.LinkTable = "Transaction";
                //IDReference.LinkedColumn = "TransactionID";
                //System.Collections.Generic.Dictionary<string, string> EntityDictTransactionID = DiversityWorkbench.Entity.EntityInformation("Transaction");
                //IDReference.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, EntityDictTransactionID);
                //if (IDReference.DisplayText.Length == 0)
                //    IDReference.DisplayText = "Transaction";
                //if (IDReference.DisplayText.Length == 0)
                //    IDReference.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, EntityDictTransactionID);
                //IdentifierLinks.Add("Transaction", IDReference);

                //DiversityWorkbench.ReferencingTable ID = new DiversityWorkbench.ReferencingTable("ExternalIdentifier", IdentifierLinks);

                //Description = DiversityWorkbench.Functions.ColumnDescription("ExternalIdentifier", "Identifier");
                //DiversityWorkbench.QueryCondition qIdentifier = new DiversityWorkbench.QueryCondition(false, "Identifier", "External identifier", "Identifier", "Identifier", Description, ID);
                //QueryConditions.Add(qIdentifier);

                //Description = DiversityWorkbench.Functions.ColumnDescription("ExternalIdentifier", "Type");
                //DiversityWorkbench.QueryCondition qIdentifierType = new DiversityWorkbench.QueryCondition(false, "Type", "External identifier", "Type", "Type of the identifier", Description, ID);
                //QueryConditions.Add(qIdentifierType);

                //Description = DiversityWorkbench.Functions.ColumnDescription("ExternalIdentifier", "Notes");
                //DiversityWorkbench.QueryCondition qIdentifierNotes = new DiversityWorkbench.QueryCondition(false, "Notes", "External identifier", "Notes", "Notes about the identifier", Description, ID);
                //QueryConditions.Add(qIdentifierNotes);

                return QueryConditions;
            }
        }

        public override DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns
        {
            get
            {
                string Table = "TransactionHierarchyAll()";// "TransactionList";
#if DEBUG
#endif
                Table = "TransactionPermit";// "TransactionList";
                //string Table = "sp_TransactionHierarchyAll 1";// "TransactionList";
                //if (!this._MainTableContainsHierarchy)
                //    Table = "TransactionList";

                string IdentityColumn = "TransactionID";

                DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns;
                //if (this._MainTableContainsHierarchy)
                //    QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[7];
                //else
                    QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[4];

                QueryDisplayColumns[0].DisplayText = "Transaction";
                QueryDisplayColumns[0].DisplayColumn = "TransactionTitle";
                QueryDisplayColumns[1].DisplayText = "Giver";
                QueryDisplayColumns[1].DisplayColumn = "FromTransactionPartnerName";
                QueryDisplayColumns[2].DisplayText = "Receiver";
                QueryDisplayColumns[2].DisplayColumn = "ToTransactionPartnerName";
                QueryDisplayColumns[3].DisplayText = "Date";
                QueryDisplayColumns[3].DisplayColumn = "BeginDate";
                //QueryDisplayColumns[4].DisplayText = "Trans.category";
                //QueryDisplayColumns[4].DisplayColumn = "ReportingCategory";
                //QueryDisplayColumns[5].DisplayText = "Trans.number";
                //QueryDisplayColumns[5].DisplayColumn = "ToTransactionNumber";
                //if (this._MainTableContainsHierarchy)
                //{
                //    QueryDisplayColumns[6].DisplayText = "Trans.Hierarchy";
                //    QueryDisplayColumns[6].DisplayColumn = "HierarchyDisplayText";
                //}
                //QueryDisplayColumns[4].DisplayText = "Hierarchy";
                //QueryDisplayColumns[4].DisplayColumn = "HierarchyDisplayText";
                try
                {
                    for (int i = 0; i < 4; i++)
                    {
                        //if (!this._MainTableContainsHierarchy && i == 6)
                        //    break;
                        QueryDisplayColumns[i].OrderColumn = QueryDisplayColumns[i].DisplayColumn;
                        QueryDisplayColumns[i].IdentityColumn = IdentityColumn;
                        QueryDisplayColumns[i].TableName = Table;
                    }
                }
                catch (System.Exception ex)
                { }
                return QueryDisplayColumns;
            }
        }

        public override void fillDependentTables(int ID)
        {
            try
            {
                // transferred from buildHierarchy as this event will always be accessed
                this.NonAccessibleIDs = null;

                this._DataSet.Tables["TransactionDocument"].Clear();
                this._DataSet.Tables["TransactionPayment"].Clear();
                this._DataSet.Tables["TransactionAgent"].Clear();
                this._DataSet.Tables["CollectionSpecimenPartList"].Clear();
                this._DataSet.Tables["CollectionSpecimenTransaction"].Clear();
                this._DataSet.Tables["Collection"].Clear();
                this._DataSet.Tables["TaxonomicGroups"].Clear();
                this._DataSet.Tables["ExternalIdentifier"].Clear();

                if (this._DataSet == null) 
                    this._DataSet = new System.Data.DataSet();

                if (this._UCPermit != null)
                    this._UCPermit.setTransaction(ID);

                // Check Access
                //string SQL = "SELECT COUNT(*) FROM [TransactionList] WHERE TransactionID = " + ID.ToString();
                //bool HasAccess = true;
                //if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL) == "0")
                //{
                //    HasAccess = false;
                //    SQL = "SELECT C.CollectionName " +
                //        "FROM [Transaction] AS T INNER JOIN " +
                //        "Collection AS C ON T.AdministratingCollectionID = C.CollectionID " +
                //        "WHERE T.TransactionID = " + ID.ToString();
                //    string AdminColl = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                //    if (AdminColl.Length > 0)
                //        System.Windows.Forms.MessageBox.Show("NO ACCESS\r\nAdministrating collection:\r\n\t" + AdminColl + "\r\nYou need to be a manager of this collection");
                //}

                // Documents
                string SQL = "SELECT TransactionID, Date, TransactionText, TransactionDocument, InternalNotes, DisplayText, DocumentURI, DocumentType " +
                    "FROM TransactionDocument " +
                    "WHERE TransactionID = " + ID.ToString();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                this.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterTransactionDocuments, SQL, this._DataSet.Tables["TransactionDocument"]);

                // Agents
                SQL = "SELECT TransactionID, TransactionAgentID, AgentName, AgentURI, AgentRole, Notes " +
                    "FROM TransactionAgent " +
                    "WHERE TransactionID = " + ID.ToString();
                this.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterTransactionAgents, SQL, this._DataSet.Tables["TransactionAgent"]);

                // Payments
                //SQL = "SELECT TransactionID, PaymentID, Amount, ForeignAmount, ForeignCurrency, Identifier, PaymentURI, PayerName, PayerAgentURI, RecipientName, RecipientAgentURI, " +
                //    "PaymentDate, PaymentDateSupplement, Notes " +
                //    "FROM TransactionPayment " +
                //    "WHERE TransactionID = " + ID.ToString();
                //this.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterTransactionPayments, SQL, this._DataSet.Tables["TransactionPayment"]);

                // Specimen list
                //SQL = "";
                //if (ID == 0 && this.ID != null)
                //    ID = (int)this.ID;
                //System.Data.DataRow[] rr = this._DataSet.Tables["Transaction"].Select(" TransactionID = " + ID.ToString());
                //if (rr.Length > 0)
                //{
                //    string TransactionType = rr[0]["TransactionType"].ToString().ToLower();
                //    if (TransactionType == "return" || TransactionType == "forwarding")
                //    {
                //        System.Data.DataRow[] RR = this._DataSet.Tables["Transaction"].Select("TransactionType = '" + TransactionType + "'", "");
                //        string ParentID = RR[0]["ParentTransactionID"].ToString(); // this._DataSet.Tables["Transaction"].Rows[0]["ParentTransactionID"].ToString();
                //        SQL = "SELECT " + DiversityCollection.Transaction.SqlFieldsCollectionSpecimenTransaction + " FROM " + this._SpecimenTable + " WHERE TransactionID = " + ParentID;
                //    }
                //    else
                //    {
                //        SQL = "SELECT " + DiversityCollection.Transaction.SqlFieldsCollectionSpecimenTransaction + " FROM " + this._SpecimenTable + " WHERE TransactionID = " + ID.ToString();
                //    }
                //    this.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterCollectionSpecimenTransaction, SQL, this._DataSet.Tables["CollectionSpecimenTransaction"]);
                //    this.RequerySpecimenLists();

                //    if (this._DataSet.Tables["CollectionSpecimenPartList"].Rows.Count > 0)
                //    {
                //        System.Data.DataRow R = this._DataSet.Tables["CollectionSpecimenPartList"].Rows[0];
                //        if (this._DataSet.Tables["Collection"].Rows.Count == 0)
                //        {
                //            ad.SelectCommand.CommandText = "select * from dbo.CollectionHierarchyAll()";
                //            try
                //            {
                //                ad.Fill(this._DataSet.Tables["Collection"]);
                //            }
                //            catch { }
                //        }
                //    }
                //    this.fillTaxonomicGroups();

                //}
                //else
                //{
                //}
                /// ExternalIdentifier
                SQL = "SELECT ID, ReferencedTable, ReferencedID, Type, Identifier, URL, Notes " +
                    "FROM  ExternalIdentifier " +
                    " WHERE (ReferencedID = " + this.ID.ToString() + " AND ReferencedTable = 'Transaction')";
                ad.SelectCommand.CommandText = SQL;
                ad.Fill(this._DataSet.Tables["ExternalIdentifier"]);
                this.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterExternalIdentifier, SQL, this._DataSet.Tables["ExternalIdentifier"]);
                this.clearBrowser();
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

    }
}
