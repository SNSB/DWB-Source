using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench
{
    public class Transaction : DiversityWorkbench.WorkbenchUnit, DiversityWorkbench.IWorkbenchUnit
    {

        #region Construction

        public Transaction(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
        }
        
        #endregion

        #region Interface

        public override string ServiceName() { return "DiversityCollection"; }

        public System.Collections.Generic.Dictionary<string, string> UnitValues(int ID)
        {
            this._UnitValues = new Dictionary<string, string>();

            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                string SQL = "SELECT dbo.BaseURL() + 'Transaction/' + CAST(TransactionID AS varchar) AS _URI, TransactionTitle AS _DisplayText, " +
                    "TransactionID, TransactionTitle, TransactionType, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, " +
                    "FromTransactionPartnerAgentURI, FromTransactionNumber, ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, NumberOfUnits, Investigator, TransactionComment, " +
                    "BeginDate, AgreedEndDate, ActualEndDate, DateSupplement, InternalNotes, ToRecipient, ResponsibleName, ResponsibleAgentURI, MaterialSource " +
                    "FROM   [Transaction] AS T " +
                    "WHERE   TransactionID = " + ID.ToString();
                this.getDataFromTable(SQL, ref this._UnitValues);

                SQL = "SELECT AgentName, AgentURI, AgentRole, Notes " +
                    "FROM TransactionAgent AS A " +
                    "WHERE TransactionID = " + ID.ToString();
                this.getDataFromTable(SQL, ref this._UnitValues);

                SQL = "SELECT Date, TransactionText, InternalNotes, DisplayText, DocumentURI, DocumentType " +
                    "FROM  TransactionDocument AS D " +
                    "WHERE TransactionID = " + ID.ToString();
                this.getDataFromTable(SQL, ref this._UnitValues);

                SQL = "SELECT Amount, ForeignAmount, ForeignCurrency, Identifier, PaymentURI, PayerName, PayerAgentURI, RecipientName, RecipientAgentURI, PaymentDate, PaymentDateSupplement, Notes " +
                    "FROM TransactionPayment AS P " +
                    "WHERE TransactionID = " + ID.ToString();
                this.getDataFromTable(SQL, ref this._UnitValues);
            }
            return this._UnitValues;
        }

        public string MainTable() { return "Transaction"; }

        public DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns()
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[1];
            QueryDisplayColumns[0].DisplayText = "TransactionTitle";
            QueryDisplayColumns[0].DisplayColumn = "TransactionTitle";
            QueryDisplayColumns[0].OrderColumn = "TransactionTitle";
            QueryDisplayColumns[0].IdentityColumn = "TransactionID";
            QueryDisplayColumns[0].TableName = "Transaction";
            return QueryDisplayColumns;
        }

        public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions()
        {
            string Database = "DiversityCollection";
            try
            {
                Database = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityCollection"].ServerConnection.DatabaseName;
            }
            catch { }

            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

            string Description = "Transaction";
            DiversityWorkbench.QueryCondition qTransaction = new DiversityWorkbench.QueryCondition(true, "Transaction", "TransactionID", "TransactionTitle", "Transaction", "Transaction", "Transaction", Description);
            QueryConditions.Add(qTransaction);

            //Description = DiversityWorkbench.Functions.ColumnDescription("Terminology", "ExternalDatabase");
            //DiversityWorkbench.QueryCondition qExternalDatabase = new DiversityWorkbench.QueryCondition(true, "Terminology", "TerminologyID", "ExternalDatabase", "Terminology", "Ext. Database", "External database", Description);
            //QueryConditions.Add(qExternalDatabase);

            //Description = DiversityWorkbench.Functions.ColumnDescription("Terminology", "DefaultLanguageCode");
            //DiversityWorkbench.QueryCondition qLanguageCode = new DiversityWorkbench.QueryCondition(true, "Terminology", "TerminologyID", "DefaultLanguageCode", "Terminology", "Language", "2-letter ISO code of the language", Description, "LanguageCode_Enum", Database);
            //QueryConditions.Add(qLanguageCode);

            // DATABASE
            //System.Data.DataTable dtTerminology = new System.Data.DataTable();
            //string SQL = "SELECT TerminologyID AS [Value], Terminology.DisplayText AS Display " +
            //        "FROM Terminology " +
            //        "ORDER BY DisplayText";
            //if (this._ServerConnection.ConnectionString.Length > 0)
            //{
            //    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
            //    try { a.Fill(dtTerminology); }
            //    catch { }
            //}
            //if (dtTerminology.Columns.Count == 0)
            //{
            //    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
            //    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
            //    dtTerminology.Columns.Add(Value);
            //    dtTerminology.Columns.Add(Display);
            //}
            //SQL = "FROM Terminology INNER JOIN " +
            //    "Term ON Terminology.TerminologyID = Term.TerminologyID ";
            //Description = DiversityWorkbench.Functions.ColumnDescription("Terminology", "DisplayText");
            //DiversityWorkbench.QueryCondition qTerminology = new DiversityWorkbench.QueryCondition(true, "Terminology", "TerminologyID", true, SQL, "Terminology.TerminologyID", "Terminology", "Terminology", "Terminology", Description, dtTerminology, false);
            //QueryConditions.Add(qTerminology);

            return QueryConditions;
        }

        public override System.Collections.Generic.Dictionary<string, string> UnitValues(string Domain, int ID)
        {
            return this.UnitValues(ID);
        }

        public override DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns(string Domain)
        {
            return this.QueryDisplayColumns();
        }

        public override System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions(string Domain)
        {
            return this.QueryConditions();
        }

        #endregion

    }
}
