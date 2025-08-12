using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using static DiversityCollection.Collection;

namespace DiversityCollection
{

    class SpecimenPart
    {
        public string AccessionNumber;
        public string AccessionNumberTo;
        public string Identification;
        public string LocalityDescription;
        public string TypeStatus;
        public string TypeIdentification;
        public string TransactionAccessionNumber;
        public string TransactionAccessionNumberTo;
        public string CollectorsName;
        public string CollectorsNumber;
        public int SpecimenPartID; //#127
        public string TaxonSinYear
        {
            get
            {
                string[] TT = Identification.Split(new char[]{' '});
                string Taxon = "";
                for (int i = 0; i < TT.Length; i++)
                {
                    if (Taxon.Length > 0) Taxon += " ";
                    int test;
                    if (i == TT.Length - 2 && TT[i + 1].Length == 4 && int.TryParse(TT[i + 1], out test))
                    {
                        if (TT[i].EndsWith(","))
                            Taxon += TT[i].Substring(0, TT[i].Length - 1);
                        else Taxon += Taxon[i];
                        break;
                    }
                    Taxon += TT[i];
                }
                return Taxon;
            }
        }
    }

    class Transaction : HierarchicalEntity
    {

        #region Parameter

        public enum TransactionSpecimenOrder { Taxa, AccessionNumber, SingleAccessionNumber, TaxaSingleAccessionNumber };
        //public enum TransactionCorrespondenceType { Sending, Confirmation, Reminder, PartialReturn, Return, Printing, Balance, Request, Forwarding, TransactionReturn };
        public enum TransactionCorrespondenceType { Sending, Confirmation, Reminder, Return, Printing, Balance, Request, Forwarding };
        public enum TransactionType {Borrow, Embargo, Exchange, Forwarding, Gift, Inventory, Loan, Permit, Purchase, Regulation, Removal, Request, Return, Visit, Group}
        
        public static string SqlFieldsTransaction = " TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialCategory, " +
	        "MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, ToCollectionID,  " +
            "ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, ToRecipient, NumberOfUnits, Investigator, TransactionComment, BeginDate,  " +
            "AgreedEndDate, ActualEndDate, DateSupplement, InternalNotes, ResponsibleName, ResponsibleAgentURI, MaterialSource ";

        public static string SqlFieldsPermit = " TransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, " +
            "MaterialDescription, MaterialCategory, MaterialCollectors, MaterialSource, FromTransactionPartnerName, FromTransactionNumber, " +
            "ToTransactionPartnerName, ToTransactionNumber, NumberOfUnits, Investigator, TransactionComment, " +
            "BeginDate, AgreedEndDate, DateSupplement, InternalNotes, ToRecipient, ResponsibleName ";

        public static string SqlFieldsCollectionSpecimenTransaction = " CollectionSpecimenID, TransactionID, SpecimenPartID, IsOnLoan, LogInsertedBy, LogInsertedWhen, LogUpdatedBy, LogUpdatedWhen, TransactionReturnID, AccessionNumber, TransactionTitle ";

        public static string SqlFieldsTransactionDocuments = " TransactionID, [Date], TransactionText, TransactionDocument, InternalNotes, DisplayText, DocumentURI, DocumentType ";

        public static string SqlFieldsTransactionAgents = " TransactionID, TransactionAgentID, AgentName, AgentURI, AgentRole, Notes ";

        public static string SqlFieldsTransactionPayments = " TransactionID, PaymentID, Amount, ForeignAmount, ForeignCurrency, Identifier, PaymentURI, PayerName, PayerAgentURI, RecipientName, RecipientAgentURI, " +
            "PaymentDate, PaymentDateSupplement, Notes ";

        public static string SqlCollectionSpecimenPartListTypeIncluded(int TransactionID, bool IncludeTypeOfReturn)
        {
            //string SQL = "/* Getting the rows of the specimen parts related to a transaction */" +
            //    "/* Temporary table */" +
            //    "DECLARE @FirstLines table " +
            //    "([CollectionSpecimenID] [int] Primary key, " +
            //    "[Taxonomic_group] [nvarchar](50) NULL, " +
            //    "[Order_of_taxon] [nvarchar](255) NULL, " +
            //    "[Family_of_taxon] [nvarchar](255) NULL, " +
            //    "[Taxonomic_name] [nvarchar](255) NULL, " +
            //    "[Identification_qualifier] [nvarchar](50) NULL,  " +
            //    "[Type_status] [nvarchar](50) NULL, " +
            //    "[LocalityDescription] [nvarchar](900) NULL, " +
            //    "[IdentificationUnitID] int) ; " +
            //    "/* Fill temporary table */" +
            //    "INSERT INTO @FirstLines(CollectionSpecimenID) " +
            //    "SELECT DISTINCT P.CollectionSpecimenID " +
            //    "FROM CollectionSpecimenPart AS P " +
            //    "INNER JOIN CollectionSpecimenTransaction AS T ON P.CollectionSpecimenID = T.CollectionSpecimenID AND P.SpecimenPartID = T.SpecimenPartID " +
            //    "INNER JOIN CollectionSpecimen AS S ON P.CollectionSpecimenID = S.CollectionSpecimenID " +
            //    "INNER JOIN CollectionSpecimenID_UserAvailable AS A ON S.CollectionSpecimenID = A.CollectionSpecimenID " +
            //    "GROUP BY P.CollectionSpecimenID, P.SpecimenPartID, P.PartSublabel, P.CollectionID, P.MaterialCategory, P.StorageLocation, P.Stock, P.Notes, " +
            //    "T.TransactionID, P.AccessionNumber, S.AccessionNumber, T.IsOnLoan, T.TransactionReturnID " +
            //    "HAVING  T.TransactionID = " + TransactionID.ToString() + "; " +
            //    "/* Set locality description in temporary table */" +
            //    "UPDATE F SET F.[LocalityDescription] = E.[LocalityDescription] " +
            //    "from @FirstLines F, CollectionEvent E, CollectionSpecimen S " +
            //    "WHERE F.CollectionSpecimenID = S.CollectionSpecimenID AND S.CollectionEventID = E.CollectionEventID" +
            //    "/* Set unit related information */" +
            //    "UPDATE F SET F.[Taxonomic_group] = U.TaxonomicGroup " +
            //    ", F.IdentificationUnitID = U.IdentificationUnitID " +
            //    ", F.Order_of_taxon = U.OrderCache " +
            //    ", F.Family_of_taxon = U.FamilyCache " +
            //    "from @FirstLines F Inner Join IdentificationUnit U ON F.CollectionSpecimenID = U.CollectionSpecimenID " +
            //    "WHERE EXISTS (select * from IdentificationUnit M GROUP BY M.CollectionSpecimenID HAVING min(M.IdentificationUnitID) = U.IdentificationUnitID) " +
            //    "/* Set identification related information */" +
            //    "UPDATE F SET F.[Taxonomic_name] = I.TaxonomicName " +
            //    ", F.[Identification_qualifier] = I.IdentificationQualifier " +
            //    ", F.[Type_status] = I.TypeStatus " +
            //    "from @FirstLines F Inner Join Identification I ON F.CollectionSpecimenID = I.CollectionSpecimenID and F.IdentificationUnitID = I.IdentificationUnitID " +
            //    "WHERE EXISTS (select * from Identification M GROUP BY M.CollectionSpecimenID, M.IdentificationUnitID HAVING max(M.IdentificationUnitID) = I.IdentificationSequence and M.IdentificationUnitID = I.IdentificationUnitID) " +
            //    "/* get final list */" +
            //    "SELECT P.CollectionSpecimenID, P.SpecimenPartID,  " +
            //    "CASE WHEN P.AccessionNumber IS NULL OR P.AccessionNumber = ''  " +
            //    "THEN CASE WHEN S.AccessionNumber IS NULL OR LEN(S.AccessionNumber) = 0  " +
            //    "THEN '[ID: ' + CAST(P.CollectionSpecimenID AS VARCHAR)  + ']' ELSE S.AccessionNumber END ELSE P.AccessionNumber END AS AccessionNumber,  " +
            //    "P.PartSublabel, P.CollectionID, P.MaterialCategory, P.StorageLocation,  P.Stock, P.Notes, T.TransactionID, T.IsOnLoan,  " +
            //    "F.Taxonomic_name AS LastIdentificationCache,  " +
            //    "dbo.TaxonWithQualifier(F.Taxonomic_name, F.Identification_qualifier) AS LastIdentificationCache,  " +
            //    "F.Family_of_taxon, F.Order_of_taxon, F.Taxonomic_group,   " +
            //    "CASE WHEN P.AccessionNumber IS NULL OR LEN(RTRIM(P.AccessionNumber)) = 0 THEN CASE WHEN S.AccessionNumber IS NULL OR LEN(RTRIM(S.AccessionNumber)) = 0  " +
            //    "THEN '[ID: ' + CAST(P.CollectionSpecimenID AS VARCHAR)  + ']' ELSE S.AccessionNumber END ELSE P.AccessionNumber END +  " +
            //    "CASE WHEN P.PartSublabel IS NULL OR RTRIM(P.PartSublabel) = '' THEN '' ELSE ' - ' + P.PartSublabel END + ' - ' + P.MaterialCategory  " +
            //    "AS DisplayText,  " +
            //    "CASE WHEN NOT F.Type_status IS NULL  THEN F.Type_status /*+ ' of ' + F.Taxonomic_name*/ ELSE NULL END AS TypeOf,  " +
            //    "F.LocalityDescription, T.TransactionReturnID, ";

            // Markus 18.12.2018 - optimized query avoiding EXISTS clause
            string SQL = "/* Getting the rows of the specimen parts related to a transaction */" +
                "/* Temporary table */" +
                "DECLARE @FirstLines table " +
                "([SpecimenPartID] [int] Primary key, " +
                "[CollectionSpecimenID] [int], " +
                "[Taxonomic_group] [nvarchar](50) NULL, " +
                "[Order_of_taxon] [nvarchar](255) NULL, " +
                "[Family_of_taxon] [nvarchar](255) NULL, " +
                "[Taxonomic_name] [nvarchar](255) NULL, " +
                "[Identification_qualifier] [nvarchar](50) NULL,  " +
                "[Type_status] [nvarchar](50) NULL, " +
                "[LocalityDescription] [nvarchar](900) NULL, " +
                "[IdentificationUnitID] int NULL, " +
                "[IdentificationSequence] int NULL, " +
                "[Country] [nvarchar](50) NULL, " +
                "CollectionYear smallint NULL, " +
                "Permit [nvarchar](200) NULL, " +
                "PermitID int NULL); " +
                "/* Fill temporary table */" +
                "INSERT INTO @FirstLines(SpecimenPartID, CollectionSpecimenID) " +
                "SELECT DISTINCT P.SpecimenPartID, P.CollectionSpecimenID " +
                "FROM CollectionSpecimenPart AS P " +
                "INNER JOIN CollectionSpecimenTransaction AS T ON P.CollectionSpecimenID = T.CollectionSpecimenID AND P.SpecimenPartID = T.SpecimenPartID " +
                "INNER JOIN CollectionSpecimen AS S ON P.CollectionSpecimenID = S.CollectionSpecimenID " +
                "INNER JOIN CollectionSpecimenID_UserAvailable AS A ON S.CollectionSpecimenID = A.CollectionSpecimenID " +
                "GROUP BY P.CollectionSpecimenID, P.SpecimenPartID, P.PartSublabel, P.CollectionID, P.MaterialCategory, P.StorageLocation, P.Stock, P.Notes, " +
                "T.TransactionID, P.AccessionNumber, S.AccessionNumber, T.IsOnLoan, T.TransactionReturnID " +
                "HAVING  T.TransactionID = " + TransactionID.ToString() + "; " +
                "/* Set locality description in temporary table */" +
                "UPDATE F SET F.[LocalityDescription] = E.[LocalityDescription], F.Country = E.CountryCache, F.CollectionYear = E.CollectionYear " +
                "from @FirstLines F, CollectionEvent E, CollectionSpecimen S " +
                "WHERE F.CollectionSpecimenID = S.CollectionSpecimenID AND S.CollectionEventID = E.CollectionEventID; " +
                "/* Set permit in temporary table */" +
                "UPDATE F SET F.[Permit] = T.[TransactionTitle], F.[PermitID] = T.[TransactionID] " +
                "from @FirstLines F, [Transaction] T, CollectionSpecimenTransaction S " +
                "WHERE F.SpecimenPartID = S.SpecimenPartID AND S.TransactionID = T.TransactionID AND T.TransactionType = 'permit'; " +
                "/* Set unit related information */" +
                "UPDATE F SET F.IdentificationUnitID = U.IdentificationUnitID " +
                "from @FirstLines F Inner Join IdentificationUnit U ON F.CollectionSpecimenID = U.CollectionSpecimenID and u.DisplayOrder = 1; " +
                "UPDATE F SET F.IdentificationUnitID = U.IdentificationUnitID " +
                "from @FirstLines F Inner Join IdentificationUnit U ON F.CollectionSpecimenID = U.CollectionSpecimenID and F.IdentificationUnitID is null and u.DisplayOrder > 1; " +
                "UPDATE F SET F.IdentificationUnitID = U.IdentificationUnitID " +
                "from @FirstLines F Inner Join IdentificationUnit U ON F.CollectionSpecimenID = U.CollectionSpecimenID and F.IdentificationUnitID is null and u.DisplayOrder < 1; " +
                "UPDATE F SET F.[Taxonomic_group] = U.TaxonomicGroup , F.Order_of_taxon = U.OrderCache, F.Family_of_taxon = U.FamilyCache  " +
                "from @FirstLines F Inner Join IdentificationUnit U ON F.CollectionSpecimenID = U.CollectionSpecimenID and F.IdentificationUnitID = U.IdentificationUnitID; " +
                "/* Set identification related information */" +
                "UPDATE F SET F.IdentificationSequence = I.IdentificationSequence " +
                "from @FirstLines F Inner Join Identification I ON F.CollectionSpecimenID = I.CollectionSpecimenID " +
                "and F.IdentificationUnitID = I.IdentificationUnitID " +
                "and exists (select * from Identification M where M.CollectionSpecimenID = I.CollectionSpecimenID " +
                "and M.IdentificationUnitID = I.IdentificationUnitID " +
                "group by M.CollectionSpecimenID, M.IdentificationUnitID having i.IdentificationSequence = Max(M.IdentificationSequence)) " +
                "/* declare @Sequence int; " + // unklar warum das hier so organisiert war - falls -1 als IdentificationSequence drin steht klappt es nicht
                "set @Sequence = 0; " +
                "declare @i int; " +
                "set @i = (select count(*) from @FirstLines F where F.IdentificationSequence is null); " +
                "while @i > 0 and @Sequence < 10 " +
                "begin " +
                "UPDATE F SET F.IdentificationSequence = I.IdentificationSequence " +
                "from @FirstLines F Inner Join Identification I ON F.CollectionSpecimenID = I.CollectionSpecimenID and F.IdentificationUnitID = I.IdentificationUnitID  " +
                "and I.IdentificationSequence = @Sequence and F.IdentificationSequence is null " +
                "and exists (select * from Identification M group by M.CollectionSpecimenID, M.IdentificationUnitID " +
                "having M.CollectionSpecimenID = I.CollectionSpecimenID and M.IdentificationUnitID = I.IdentificationUnitID and I.IdentificationSequence = max(M.IdentificationSequence)); " +
                "set @i = (select count(*) from @FirstLines F where F.IdentificationSequence is null); " +
                "set @Sequence = @Sequence + 1; " +
                "end;*/ " +
                "UPDATE F SET F.IdentificationSequence = I.TaxonomicName , F.[Identification_qualifier] = I.IdentificationQualifier , F.[Type_status] = I.TypeStatus  " +
                "from @FirstLines F Inner Join Identification I ON F.CollectionSpecimenID = I.CollectionSpecimenID and F.IdentificationUnitID = I.IdentificationUnitID  " +
                "and I.IdentificationSequence is null; " +
                "UPDATE F SET F.[Taxonomic_name] = I.TaxonomicName , F.[Identification_qualifier] = I.IdentificationQualifier , F.[Type_status] = I.TypeStatus  " +
                "from @FirstLines F Inner Join Identification I ON F.CollectionSpecimenID = I.CollectionSpecimenID " +
                "and F.IdentificationUnitID = I.IdentificationUnitID and F.IdentificationSequence = I.IdentificationSequence; " +
                "/* get final list */" +
                "SELECT P.CollectionSpecimenID, P.SpecimenPartID,  " +
                "CASE WHEN P.AccessionNumber IS NULL OR P.AccessionNumber = ''  " +
                "THEN CASE WHEN S.AccessionNumber IS NULL OR LEN(S.AccessionNumber) = 0  " +
                "THEN '[ID: ' + CAST(P.CollectionSpecimenID AS VARCHAR)  + ']' ELSE S.AccessionNumber END ELSE P.AccessionNumber END AS AccessionNumber,  " +
                "P.PartSublabel, P.CollectionID, P.MaterialCategory, P.StorageLocation,  P.Stock, P.Notes, T.TransactionID, T.IsOnLoan,  " +
                "F.Taxonomic_name AS LastIdentificationCache,  " +
                "dbo.TaxonWithQualifier(F.Taxonomic_name, F.Identification_qualifier) AS TaxonWithQualifier,  " +
                "F.Family_of_taxon, F.Order_of_taxon, F.Taxonomic_group,   " +
                "CASE WHEN P.AccessionNumber IS NULL OR LEN(RTRIM(P.AccessionNumber)) = 0 THEN CASE WHEN S.AccessionNumber IS NULL OR LEN(RTRIM(S.AccessionNumber)) = 0  " +
                "THEN '[ID: ' + CAST(P.CollectionSpecimenID AS VARCHAR)  + ']' ELSE S.AccessionNumber END ELSE P.AccessionNumber END +  " +
                "CASE WHEN P.PartSublabel IS NULL OR RTRIM(P.PartSublabel) = '' THEN '' ELSE ' - ' + P.PartSublabel END + ' - ' + P.MaterialCategory + " +
                "' - ' + CASE WHEN F.Country is null OR LEN(RTRIM(F.Country)) = 0 then '?' else F.Country end " +
                " + ' - ' + CASE WHEN F.CollectionYear is null then '?' else cast(F.CollectionYear as varchar) end " +
                " + CASE WHEN F.Permit <> '' then ' - Permit: ' + F.Permit else '' end " +
                "AS DisplayText, F.PermitID, " +
                "CASE WHEN NOT F.Type_status IS NULL  THEN F.Type_status /*+ ' of ' + F.Taxonomic_name*/ ELSE NULL END AS TypeOf,  " +
                "F.LocalityDescription, T.TransactionReturnID, ";

            if (IncludeTypeOfReturn)
                SQL += "TR.TransactionType, ";
            else SQL += "'', ";
            SQL += "T.AccessionNumber AS TransactionAccessionNumber    " +
                "FROM  @FirstLines F INNER JOIN CollectionSpecimenPart AS P ON F.CollectionSpecimenID = P.CollectionSpecimenID AND F.SpecimenPartID = P.SpecimenPartID " +
                "INNER JOIN CollectionSpecimenTransaction AS T ON P.CollectionSpecimenID = T.CollectionSpecimenID AND P.SpecimenPartID = T.SpecimenPartID  ";
            if (IncludeTypeOfReturn)
                SQL += "INNER JOIN [Transaction] AS TR ON TR.TransactionID = T.TransactionReturnID ";
            SQL += "INNER JOIN CollectionSpecimen AS S ON P.CollectionSpecimenID = S.CollectionSpecimenID  " +
                "GROUP BY P.CollectionSpecimenID, P.SpecimenPartID, P.PartSublabel, P.CollectionID, P.MaterialCategory, P.StorageLocation, P.Stock, P.Notes,  " +
                "T.TransactionID,   " +
                "P.AccessionNumber, S.AccessionNumber,  " +
                "T.IsOnLoan, F.LocalityDescription, T.TransactionReturnID, T.AccessionNumber, ";
            if (IncludeTypeOfReturn) SQL += " TR.TransactionType,  ";
            SQL += " F.CollectionSpecimenID, F.Family_of_taxon, F.Identification_qualifier, F.Order_of_taxon, F.Taxonomic_group, F.Taxonomic_name, F.Type_status, F.CollectionYear, F.Country, F.Permit, F.PermitID " +
            "HAVING  T.TransactionID = " + TransactionID.ToString();
            return SQL;
        }

        private DiversityCollection.Datasets.DataSetTransaction _dsTransactionForReturn;

        private static System.Collections.Generic.Dictionary<Transaction.ConversionType, string> _ConversionDictionary;
        private static System.Collections.Generic.Dictionary<string, string> _ConversionDescription;
        public enum ConversionType { No_Conversion, Numeric_to_roman };

        private System.Collections.Generic.Dictionary<string, string> _TransactionParterAddressValues;
        private DiversityWorkbench.Agent _Agent;
        private System.Windows.Forms.TabControl _TabControl;
        private int _TransactionID;

        private int _ParentTransactionID;

        private int _InitialNumberOfSpecimen = 0;
        private int _DatasetOnLoan = 0;
        private int _DatasetReturnedSinceDate = 0;
        //private DateTime _StartDate;

        private int _PartsInTransaction = 0;
        private int _PartsReturned = 0;
        private int _PartsShipped = 0;
        private int _PartsOnLoan = 0;
        private string _CurrentShippedSpecimenParts = "";

        private string _AdminAgentURI = "";

        private DiversityCollection.Transaction.TransactionCorrespondenceType _TransactionCorrespondenceType = TransactionCorrespondenceType.Sending;
        private DiversityCollection.Transaction.TransactionType _TransactionType = TransactionType.Loan;

        protected Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterTransactionDocuments;
        protected Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterTransactionAgents;
        protected Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterTransactionPayments;
        protected Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterCollectionSpecimenTransaction;
        protected Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterExternalIdentifier;

        private bool _IncludeTypeInformation = true;
        private bool _IncludeChildTransactions = false;
        private bool _IncludeSubcollections = false;
        private bool _IncludeAllCollections = false;
        private bool _AllSpecimenOnLoan = false;
        private bool _GroupByMaterial = true;

        public bool GroupByMaterial
        {
            get { return _GroupByMaterial; }
            set { _GroupByMaterial = value; }
        }

        private string _RestrictToTaxonomicGroup = "";

        public string RestrictToTaxonomicGroup
        {
            get { return _RestrictToTaxonomicGroup; }
            set 
            {
                _RestrictToTaxonomicGroup = value;
                this._DataSet.Tables["TaxonomicGroups"].Clear();
                this.fillTaxonomicGroups();
            }
        }

        private Transaction.ConversionType _ConversionType = ConversionType.No_Conversion;
        private TransactionSpecimenOrder _SpecimenOrder = TransactionSpecimenOrder.Taxa;

        private System.Collections.Generic.Dictionary<string, System.Windows.Forms.BindingSource> _DependentTablesBindingSources;

        public System.Collections.Generic.Dictionary<string, System.Windows.Forms.BindingSource> DependentTablesBindingSources
        {
            get
            {
                if (this._DependentTablesBindingSources == null)
                    this._DependentTablesBindingSources = new Dictionary<string, System.Windows.Forms.BindingSource>();
                return _DependentTablesBindingSources; 
            }
            set { _DependentTablesBindingSources = value; }
        }

        private System.Data.DataTable _DtTransactionAccess;

        public System.Data.DataTable DtTransactionAccess
        {
            get 
            {
                if (this._DtTransactionAccess == null)
                {
                    this._DtTransactionAccess = new System.Data.DataTable();
                    string SQL = "SELECT * FROM dbo.TransactionHierarchyAccess(" + this.ID.ToString() + ")";
                    string Message = "";
                    if (!DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._DtTransactionAccess, ref Message))
                        System.Windows.Forms.MessageBox.Show(Message);
                    //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    //ad.Fill(this._DtTransactionAccess);
                }
                return _DtTransactionAccess; 
            }
            set { _DtTransactionAccess = value; }
        }

        private System.Collections.Generic.List<int> _NonAccessibleIDs;

        public System.Collections.Generic.List<int> NonAccessibleIDs
        {
            get 
            {
                if (this._NonAccessibleIDs == null)
                {
                    this._NonAccessibleIDs = new List<int>();
                    foreach (System.Data.DataRow R in this.DtTransactionAccess.Rows)
                    {
                        if (R["Accessible"].ToString() == "0")
                            this.NonAccessibleIDs.Add(int.Parse(R["TransactionID"].ToString()));
                    }
                }
                return _NonAccessibleIDs; 
            }
            set 
            { 
                _NonAccessibleIDs = value;
                if (value == null && this._DtTransactionAccess != null)
                    this._DtTransactionAccess = null;
            }
        }

        #endregion

        #region Construction

        public Transaction(
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
            : base(ref Dataset, DataTable, ref TreeView, Form, UserControlQueryList, SplitContainerMain,
            SplitContainerData, ToolStripButtonSpecimenList, /*ImageListSpecimenList,*/ UserControlSpecimenList,
            HelpProvider, ToolTip, ref BindingSource, DiversityCollection.LookupTable.DtTransaction, DiversityCollection.LookupTable.DtTransactionHierarchy)
        {
            try
            {
                this._sqlItemFieldList = DiversityCollection.Transaction.SqlFieldsTransaction;
                this._SpecimenTable = "CollectionSpecimenTransaction";
                this._MainTable = "[Transaction]";
                this._TabControl = TabControl;
                this._OrderColumns.Add("BeginDate");
                this._OrderColumns.Add("TransactionTitle");
            }
            catch (System.Exception ex)
            {
            }
        }
        
        #endregion

        #region Functions and properties

        protected override string SqlSpecimenCount(int ID)
        {
            return "SELECT COUNT(*) FROM CollectionSpecimenTransaction WHERE TransactionID = " + ID.ToString();
        }

        //public DateTime StartDate
        //{
        //    get 
        //    {
        //        if (_StartDate == null || _StartDate.ToString() == "01.01.0001 00:00:00")
        //            _StartDate = System.DateTime.Parse(System.DateTime.Now.ToShortDateString());
        //        return _StartDate; 
        //    }
        //    set { _StartDate = value; }
        //}

        #endregion

        #region Interface

        public override System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions
        {
            get
            {
                System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

                #region Transaction

                string Description = this.FormFunctions.ColumnDescription("[Transaction]", "TransactionTitle");
                DiversityWorkbench.QueryCondition qTransactionTitle = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "TransactionTitle", "Transaction", "Name", "Name", Description);
                QueryConditions.Add(qTransactionTitle);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "TransactionID");
                DiversityWorkbench.QueryCondition qTransactionID = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "TransactionID", "Transaction", "ID", "Transaction ID", Description);
                QueryConditions.Add(qTransactionID);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "ParentTransactionID");
                DiversityWorkbench.QueryCondition qParentTransactionID = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "ParentTransactionID", "Transaction", "ParentID", "Parent ID", Description);
                QueryConditions.Add(qParentTransactionID);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "TransactionType");
                DiversityWorkbench.QueryCondition qTransactionType = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "TransactionType", "Transaction", "Type", "Transaction type", Description, "CollTransactionType_Enum", DiversityWorkbench.Settings.DatabaseName);
                QueryConditions.Add(qTransactionType);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "BeginDate");
                DiversityWorkbench.QueryCondition qBeginDate = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "BeginDate", "Transaction", "Begin", "Begin", Description, true);
                QueryConditions.Add(qBeginDate);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "AgreedEndDate");
                DiversityWorkbench.QueryCondition qAgreedEndDate = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "AgreedEndDate", "Transaction", "End", "End", Description, true);
                QueryConditions.Add(qAgreedEndDate);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "TransactionComment");
                DiversityWorkbench.QueryCondition qTransactionComment = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "TransactionComment", "Transaction", "Comment", "Comment", Description);
                QueryConditions.Add(qTransactionComment);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "InternalNotes");
                DiversityWorkbench.QueryCondition qInternalNotes = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "InternalNotes", "Transaction", "Notes", "Notes", Description);
                QueryConditions.Add(qInternalNotes);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "Investigator");
                DiversityWorkbench.QueryCondition qInvestigator = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "Investigator", "Transaction", "Investigator", "Investigator", Description);
                QueryConditions.Add(qInvestigator);

                #endregion

                #region From

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "FromTransactionNumber");
                DiversityWorkbench.QueryCondition qFromTransactionNumber = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "FromTransactionNumber", "From", "Number", "Number", Description);
                QueryConditions.Add(qFromTransactionNumber);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "FromTransactionPartnerName");
                DiversityWorkbench.QueryCondition qFromTransactionPartnerName = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "FromTransactionPartnerName", "From", "Partner", "Transaction partner", Description);
                QueryConditions.Add(qFromTransactionPartnerName);

                System.Data.DataTable dtCollectionFrom = new System.Data.DataTable();
                System.Data.DataTable dtCollectionTo = new System.Data.DataTable();
                string SQL = "SELECT NULL AS [Value], NULL AS Display UNION " +
                    "SELECT CollectionID AS [Value], CollectionName AS Display " +
                    "FROM Collection " +
                    "ORDER BY Display ";
                Microsoft.Data.SqlClient.SqlDataAdapter aColl = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { 
                        aColl.Fill(dtCollectionFrom);
                        aColl.Fill(dtCollectionTo);
                    }
                    catch { }
                }
                if (dtCollectionFrom.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtCollectionFrom.Columns.Add(Value);
                    dtCollectionFrom.Columns.Add(Display);
                }
                Description = DiversityWorkbench.Functions.ColumnDescription("Collection", "CollectionName");

                DiversityWorkbench.QueryCondition qCollectionFrom = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "FromCollectionID", "From", "Collection", "Collection", Description, dtCollectionFrom, true);
                QueryConditions.Add(qCollectionFrom);

                #endregion

                #region To

                DiversityWorkbench.QueryCondition qCollectionTo = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "ToCollectionID", "To", "Collection", "Collection", Description, dtCollectionTo, true);
                QueryConditions.Add(qCollectionTo);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "ToTransactionNumber");
                DiversityWorkbench.QueryCondition qToTransactionNumber = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "ToTransactionNumber", "To", "Number", "Number", Description);
                QueryConditions.Add(qToTransactionNumber);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "ToTransactionPartnerName");
                DiversityWorkbench.QueryCondition qToTransactionPartnerName = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "ToTransactionPartnerName", "To", "Partner", "Transaction partner", Description);
                QueryConditions.Add(qToTransactionPartnerName);

                #endregion

                #region Material

                DiversityWorkbench.QueryCondition qCollectionAdmin = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "AdministratingCollectionID", "Material", "Administrating collection", "Collection", Description, dtCollectionTo, true);
                QueryConditions.Add(qCollectionAdmin);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "MaterialDescription");
                DiversityWorkbench.QueryCondition qMaterialDescription = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "MaterialDescription", "Material", "Description", "Material description", Description);
                QueryConditions.Add(qMaterialDescription);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "MaterialSource");
                DiversityWorkbench.QueryCondition qMaterialSource = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "MaterialSource", "Material", "Source", "Material source", Description);
                QueryConditions.Add(qMaterialSource);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "MaterialCategory");
                DiversityWorkbench.QueryCondition qMaterialCategory = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "MaterialCategory", "Material", "Category", "Material category", Description);
                QueryConditions.Add(qMaterialCategory);

                #endregion

                #region Logging

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "LogCreatedBy");
                DiversityWorkbench.QueryCondition qTransactionLogCreatedBy = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "LogCreatedBy", "Logging", "Creator", "Log created by", Description);
                QueryConditions.Add(qTransactionLogCreatedBy);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "LogCreatedWhen");
                DiversityWorkbench.QueryCondition qTransactionLogCreatedWhen = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "LogCreatedWhen", "Logging", "Cre.date", "Log created when", Description, true);
                QueryConditions.Add(qTransactionLogCreatedWhen);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "LogUpdatedBy");
                DiversityWorkbench.QueryCondition qTransactionLogUpdatedBy = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "LogUpdatedBy", "Logging", "Updator", "Log Updated by", Description);
                QueryConditions.Add(qTransactionLogUpdatedBy);

                Description = this.FormFunctions.ColumnDescription("[Transaction]", "LogUpdatedWhen");
                DiversityWorkbench.QueryCondition qTransactionLogUpdatedWhen = new DiversityWorkbench.QueryCondition(true, "[Transaction]", "TransactionID", "LogUpdatedWhen", "Logging", "Upd.date", "Log Updated when", Description, true);
                QueryConditions.Add(qTransactionLogUpdatedWhen);

                #endregion

                #region Agent

                Description = this.FormFunctions.ColumnDescription("[TransactionAgent]", "AgentName");
                DiversityWorkbench.QueryCondition qTransactionAgentName = new DiversityWorkbench.QueryCondition(true, "[TransactionAgent]", "TransactionID", "AgentName", "Agent", "Agent", "Name of the agent", Description);
                QueryConditions.Add(qTransactionAgentName);

                Description = this.FormFunctions.ColumnDescription("[TransactionAgent]", "AgentRole");
                DiversityWorkbench.QueryCondition qTransactionAgentRole = new DiversityWorkbench.QueryCondition(true, "[TransactionAgent]", "TransactionID", "AgentRole", "Agent", "Role", "Role of the agent", Description);
                QueryConditions.Add(qTransactionAgentRole);

                #endregion

                #region Payment

                Description = this.FormFunctions.ColumnDescription("[TransactionPayment]", "RecipientName");
                DiversityWorkbench.QueryCondition qTransactionPaymentRecipientName = new DiversityWorkbench.QueryCondition(true, "[TransactionPayment]", "TransactionID", "RecipientName", "Payment", "Recipient", "Recipien of the paymentt", Description);
                QueryConditions.Add(qTransactionPaymentRecipientName);

                Description = this.FormFunctions.ColumnDescription("[TransactionPayment]", "PayerName");
                DiversityWorkbench.QueryCondition qTransactionPaymentPayerName = new DiversityWorkbench.QueryCondition(true, "[TransactionPayment]", "TransactionID", "PayerName", "Payment", "Payer", "Payer of the payment", Description);
                QueryConditions.Add(qTransactionPaymentPayerName);

                Description = this.FormFunctions.ColumnDescription("[TransactionPayment]", "Amount");
                DiversityWorkbench.QueryCondition qTransactionPaymentAmount = new DiversityWorkbench.QueryCondition(true, "[TransactionPayment]", "TransactionID", "Amount", "Payment", "Amount", "Amount of the payment", Description);
                QueryConditions.Add(qTransactionPaymentAmount);

                Description = this.FormFunctions.ColumnDescription("[TransactionPayment]", "ForeignCurrency");
                DiversityWorkbench.QueryCondition qTransactionPaymentForeignCurrency = new DiversityWorkbench.QueryCondition(true, "[TransactionPayment]", "TransactionID", "ForeignCurrency", "Payment", "Curreny", "Foreign currency", Description);
                QueryConditions.Add(qTransactionPaymentForeignCurrency);

                Description = this.FormFunctions.ColumnDescription("[TransactionPayment]", "Identifier");
                DiversityWorkbench.QueryCondition qTransactionPaymentIdentifier = new DiversityWorkbench.QueryCondition(true, "[TransactionPayment]", "TransactionID", "Identifier", "Payment", "Identifier", "Identifier", Description);
                QueryConditions.Add(qTransactionPaymentIdentifier);

                #endregion

                System.Collections.Generic.Dictionary<string, DiversityWorkbench.ReferencingTableLink> IdentifierLinks = new Dictionary<string, DiversityWorkbench.ReferencingTableLink>();
                DiversityWorkbench.ReferencingTableLink IDReference = new DiversityWorkbench.ReferencingTableLink();
                IDReference.ReferencedTable = "Transaction";
                IDReference.ReferencedColumn = "TransactionID";
                IDReference.LinkTable = "Transaction";
                IDReference.LinkedColumn = "TransactionID";
                System.Collections.Generic.Dictionary<string, string> EntityDictTransactionID = DiversityWorkbench.Entity.EntityInformation("Transaction");
                IDReference.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, EntityDictTransactionID);
                if (IDReference.DisplayText.Length == 0)
                    IDReference.DisplayText = "Transaction";
                if (IDReference.DisplayText.Length == 0)
                    IDReference.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, EntityDictTransactionID);
                IdentifierLinks.Add("Transaction", IDReference);

                DiversityWorkbench.ReferencingTable ID = new DiversityWorkbench.ReferencingTable("ExternalIdentifier", IdentifierLinks);

                Description = DiversityWorkbench.Functions.ColumnDescription("ExternalIdentifier", "Identifier");
                DiversityWorkbench.QueryCondition qIdentifier = new DiversityWorkbench.QueryCondition(false, "Identifier", "External identifier", "Identifier", "Identifier", Description, ID);
                QueryConditions.Add(qIdentifier);

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
                //string Table = "sp_TransactionHierarchyAll 1";// "TransactionList";
                if (!this._MainTableContainsHierarchy)
                    Table = "TransactionList";
#if DEBUG
#endif

                Table = "TransactionList_H7";// "TransactionList";

                string IdentityColumn = "TransactionID";

                DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns; 
                if (this._MainTableContainsHierarchy)
                    QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[7];
                else
                    QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[6];

                QueryDisplayColumns[0].DisplayText = "Transaction";
                QueryDisplayColumns[0].DisplayColumn = "TransactionTitle";

                QueryDisplayColumns[1].DisplayText = "Trans.partner";
                QueryDisplayColumns[1].DisplayColumn = "ToTransactionPartnerName";

                QueryDisplayColumns[2].DisplayText = "Trans.type";
                QueryDisplayColumns[2].DisplayColumn = "TransactionType";

                QueryDisplayColumns[3].DisplayText = "Date";
                QueryDisplayColumns[3].DisplayColumn = "BeginDate";

                QueryDisplayColumns[4].DisplayText = "Trans.category";
                QueryDisplayColumns[4].DisplayColumn = "ReportingCategory";

                QueryDisplayColumns[5].DisplayText = "Trans.number";
                QueryDisplayColumns[5].DisplayColumn = "ToTransactionNumber";

                if (this._MainTableContainsHierarchy)
                {
                    QueryDisplayColumns[6].DisplayText = "Trans.Hierarchy";
                    QueryDisplayColumns[6].DisplayColumn = "HierarchyDisplayText";
                }
                for (int i = 0; i < 7; i++)
                {
                    if (!this._MainTableContainsHierarchy && i == 6)
                        break;
                    QueryDisplayColumns[i].OrderColumn = QueryDisplayColumns[i].DisplayColumn;
                    QueryDisplayColumns[i].IdentityColumn = IdentityColumn;
                    QueryDisplayColumns[i].TableName = Table;
                }
                return QueryDisplayColumns;
            }
        }

        public int InitialNumberOfSpecimen { set { this._InitialNumberOfSpecimen = value; } }

        public System.Data.DataSet DataSet { get { return this._DataSet; } }

        public override System.Collections.Generic.Dictionary<string, string> AdditionalNotNullColumnsForNewItem(int? ParentTransactionID)
        {
            System.Collections.Generic.Dictionary<string, string> Dict = new Dictionary<string, string>();
            int AdminColl = 0;
            try
            {
                if (ParentTransactionID == null)
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    //string SQL = "SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, " +
                    //    "AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder " +
                    //    "FROM ManagerCollectionList() ORDER BY CollectionName";
                    string SQL = "SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, " +
                        "AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder " +
                        "FROM CollectionManager M INNER JOIN Collection C ON M.AdministratingCollectionID = C.CollectionID AND M.LoginName = USER_NAME() ORDER BY CollectionName";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    DiversityWorkbench.Forms.FormGetItemFromHierarchy f = new DiversityWorkbench.Forms.FormGetItemFromHierarchy(dt, "CollectionID", "CollectionParentID", "CollectionName", "CollectionID", "Select a collection from the list or via the hierarchy", "Find administrating collection");
                    f.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        if (int.TryParse(f.SelectedValue.ToString(), out AdminColl))
                            Dict.Add("AdministratingCollectionID", AdminColl.ToString());
                    }
                    else
                    {

                    }
                    SQL = "SELECT Code, DisplayText FROM CollTransactionType_Enum WHERE Code NOT IN ('forwarding', 'return') ORDER BY DisplayText ";// ";
                    dt = new System.Data.DataTable();
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(dt);
                    DiversityWorkbench.Forms.FormGetStringFromList Ftype = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "DisplayText", "Code", "Type of the transaction", "Please select the type of the new transaction");
                    Ftype.ShowDialog();
                    if (Ftype.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        Dict.Add("TransactionType", "'" + Ftype.SelectedValue + "'");
                    }
                }
                else
                {
                    string SQL = "SELECT AdministratingCollectionID FROM [Transaction] WHERE TransactionID = " + ParentTransactionID.ToString();
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    con.Open();
                    if (int.TryParse(C.ExecuteScalar()?.ToString(), out AdminColl))
                        Dict.Add("AdministratingCollectionID", AdminColl.ToString());
                    con.Close();
                    SQL = "SELECT TransactionType FROM [Transaction] WHERE TransactionID = " + ParentTransactionID.ToString();
                    string Type = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    SQL = "SELECT Code, DisplayText FROM CollTransactionType_Enum ";
                    switch (Type.ToLower())
                    {
                        case "loan":
                            SQL += " WHERE Code IN ('forwarding', 'return')";
                            break;
                        case "transaction group":
                            SQL += " WHERE Code NOT IN ('forwarding', 'return')";
                            break;
                        case "forwarding": 
                            SQL += " WHERE Code IN ('return')";
                            break;
                        case "request":
                            SQL += " WHERE Code IN ('loan')";
                            break;
                        case "return": 
                            SQL += " WHERE 1 = 0";
                            break;
                        case "borrow": 
                        case "embargo": 
                        case "exchange": 
                        case "permit": 
                        case "gift": 
                        case "inventory": 
                        case "purchase": 
                        default:
                            SQL += " WHERE Code = '" + Type + "'";
                            break;
                    }
                    // Markus 1.7.24: Order by an Ende von SQL
                    SQL += " ORDER BY DisplayText";
                    System.Data.DataTable dt = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    if (dt.Rows.Count > 1)
                    {
                        DiversityWorkbench.Forms.FormGetStringFromList Ftype = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "DisplayText", "Code", "Type of the transaction", "Please select the type of the new transaction");
                        Ftype.ShowDialog();
                        if (Ftype.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            Dict.Add("TransactionType", "'" + Ftype.SelectedValue + "'");
                        }
                    }
                    else if (dt.Rows.Count == 1) Dict.Add("TransactionType", "'" + dt.Rows[0][0].ToString() + "'");
                }
            }
            catch(System.Exception ex)  { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return Dict;
        }

        public override void markHierarchyNodes()
        {
            //base.markHierarchyNodes();
            if (this._TreeView.ImageList == null)
                this._TreeView.ImageList = DiversityCollection.Specimen.ImageListTransactionType();

            foreach (System.Windows.Forms.TreeNode N in this._TreeView.Nodes)
            {
                this.markHierarchyNode(N);
                this.markHierachyChildNodes(N);
            }
        }

        public override void ItemChanged() 
        {
            //this.
        }

        private void markHierachyChildNodes(System.Windows.Forms.TreeNode N)
        {
            this.markHierarchyNode(N);
            foreach (System.Windows.Forms.TreeNode NC in N.Nodes)
                this.markHierachyChildNodes(NC);
        }

        private void markHierarchyNode(System.Windows.Forms.TreeNode N)
        {
            if (N.Tag != null && (N.Tag.GetType().BaseType == typeof(System.Data.DataRow) || N.Tag.GetType() == typeof(System.Data.DataRow)))
            {
                System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                string Type = R["TransactionType"].ToString();
                int ID = int.Parse(R["TransactionID"].ToString());
                bool NoAccess = false;
                //if (this.DtTransactionAccess.Select("TransactionID = " + ID.ToString()).Length > 0)
                if(this.NonAccessibleIDs.Contains(ID))
                    NoAccess = true;
                bool? IsInactive = null;
                bool? HasStarted = null;
                  bool? HasEnded = null;
                System.DateTime Begin;
                System.DateTime AgreedEnd;
                System.DateTime ActualEnd;
                if ((Type == "loan" || Type == "embargo"))// && !NoAccess)
                {
                    if (!R["BeginDate"].Equals(System.DBNull.Value)
                        && System.DateTime.TryParse(R["BeginDate"].ToString(), out Begin))
                    {
                        if (Begin < System.DateTime.Now)
                            HasStarted = true;
                    }
                    if (Type == "loan")
                    {
                        if (!R["ActualEndDate"].Equals(System.DBNull.Value)
                            && System.DateTime.TryParse(R["ActualEndDate"].ToString(), out ActualEnd))
                        {
                            if (ActualEnd < System.DateTime.Now)
                                HasEnded = true;
                        }
                        if (HasEnded != null)
                        {
                            if ((bool)HasEnded)
                                IsInactive = true;
                            else if (!(bool)HasEnded)
                                IsInactive = false;
                        }
                        else if (HasStarted != null)
                        {
                            if ((bool)HasStarted)
                                IsInactive = false;
                            else
                                IsInactive = true;
                        }
                        else
                            IsInactive = true;
                    }
                    else if (Type == "embargo")
                    {
                        if (!R["AgreedEndDate"].Equals(System.DBNull.Value)
                            && System.DateTime.TryParse(R["AgreedEndDate"].ToString(), out AgreedEnd))
                        {
                            if (AgreedEnd < System.DateTime.Now)
                                HasEnded = true;
                        }
                        if (HasEnded != null)
                        {
                            if ((bool)HasEnded)
                                IsInactive = true;
                            else IsInactive = false;
                        }
                        else if (HasStarted != null)
                        {
                            if ((bool)HasStarted)
                                IsInactive = false;
                            else IsInactive = true;
                        }
                        else IsInactive = true;
                    }
                }
                //if (NoAccess)
                //    IsInactive = true;
                N.ImageIndex = DiversityCollection.Specimen.IndexImageTransactionType(Type, IsInactive);
                N.SelectedImageIndex = DiversityCollection.Specimen.IndexImageTransactionType(Type, IsInactive);
            }
        }

        //private System.Collections.Generic.SortedList<string, System.Data.DataRow> _TransactionRows;

        #region Tasks

        private bool _ShowTasks = false;
        private TaskDisplayStyle _taskDisplayStyle = TaskDisplayStyle.Default;
        private System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<System.Windows.Forms.TreeNode>> _TaskNodes;
        private System.Data.DataTable _dtTask;

        public void SetTaskVisibility(bool ShowTasks, TaskDisplayStyle taskDisplay = TaskDisplayStyle.Default)
        {
            this._ShowTasks = ShowTasks;
            this._taskDisplayStyle = taskDisplay;
            this._TaskNodes = null;
        }

        private System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<System.Windows.Forms.TreeNode>> TaskNodes()
        {
            if (_TaskNodes == null)
            {
                _TaskNodes = new Dictionary<int, List<System.Windows.Forms.TreeNode>>();
                string SQL = "SELECT case when H.CollectionHierarchyDisplayText like '%:%' then ltrim(rtrim(substring(H.CollectionHierarchyDisplayText, charindex(':', H.CollectionHierarchyDisplayText) + 1, 255))) " +
                    "else ltrim(rtrim(substring(H.CollectionHierarchyDisplayText, charindex('—', H.CollectionHierarchyDisplayText) + 1, 255))) " +
                    "end AS DisplayText, H.CollectionID, H.CollectionTaskID, T.Type " +
                    " FROM [dbo].[CollectionTaskHierarchyAll] () H  INNER JOIN Task T ON H.TaskID = T.TaskID " +
                    " ORDER BY H.CollectionHierarchyDisplayText";
                System.Data.DataTable dt = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    string TaskType = R["Type"].ToString();
                    System.Windows.Forms.TreeNode NodeTask = new System.Windows.Forms.TreeNode(R["DisplayText"].ToString());
                    NodeTask.ImageIndex = DiversityCollection.Specimen.TaskTypeImage(TaskType, true);
                    NodeTask.SelectedImageIndex = DiversityCollection.Specimen.TaskTypeImage(TaskType, true);
                    NodeTask.ForeColor = System.Drawing.Color.Gray;
                    NodeTask.Tag = R["CollectionTaskID"].ToString();

                    int TransactionID = int.Parse(R["TransactionID"].ToString());
                    if (_TaskNodes.ContainsKey(TransactionID))
                    {
                        _TaskNodes[TransactionID].Add(NodeTask);
                    }
                    else
                    {
                        System.Collections.Generic.List<System.Windows.Forms.TreeNode> nodes = new List<System.Windows.Forms.TreeNode>();
                        nodes.Add(NodeTask);
                        _TaskNodes.Add(TransactionID, nodes);
                    }
                }
            }
            return _TaskNodes;
        }


        private void addHierarchyTaskNodes(int TransactionID, System.Windows.Forms.TreeNode pNode) //, string Type)
        {
            if (this._taskDisplayStyle == TaskDisplayStyle.Default)
            {
                //string Type = r["Type"].ToString();
                string SQL = "SELECT T.Type + ':' " +
                    " + CASE WHEN C.NumberValue IS NULL OR C.NumberValue = '' THEN '' ELSE ' ' + CAST(C.NumberValue AS varchar) END " +
                    " + CASE WHEN C.DisplayText IS NULL OR C.DisplayText = '' THEN '' ELSE ' ' + C.DisplayText END " +
                    " + CASE WHEN C.TaskStart IS NULL THEN '' " +
                    " ELSE ' (' + CONVERT(varchar(10), C.TaskStart, 120) " +
                    " + CASE WHEN C.TaskEnd IS NULL THEN '' ELSE ' - ' + CONVERT(varchar(10), C.TaskEnd, 120)  END + ')' " +
                    " END  " +
                    "AS DisplayText, " +
                    "C.CollectionTaskID, T.Type " +
                    ", CASE WHEN C.TaskEnd < GetDate() THEN 1 ELSE 0 END AS TrapEnded, CASE WHEN C.BoolValue = 1 THEN 1 ELSE 0 END AS Trapped, C.MetricUnit, T.Type " +
                    "FROM [dbo].[CollectionTask] C INNER JOIN Task T ON C.TaskID = T.TaskID " +
                    "AND C.TransactionID = " + TransactionID.ToString() +
                    "  AND C.ModuleUri <> '' "; // AND T.Type IN ('" + Type + "'";
                //foreach (string T in DiversityCollection.LookupTable.TaskTypes(Type))
                //{
                //    if (T.ToLower() != Type.ToLower())
                //        SQL += ", '" + T + "'";
                //}
                //SQL += ") ORDER BY DisplayText";
                SQL += " ORDER BY DisplayText";

                this._dtTask = new System.Data.DataTable();
                string Message = "";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._dtTask, ref Message);
                if (this._dtTask.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow R in this._dtTask.Rows)
                    {
                        string Task = R[0].ToString(); // r[this.ColumnDisplayText].ToString();
                        if (Task.Length == 0)
                        { }
                        System.Windows.Forms.TreeNode NodeTask = new System.Windows.Forms.TreeNode(Task);
                        string TypeOfTask = R["Type"].ToString().ToLower();
                        switch (TypeOfTask)
                        {
                            case "pest":
                                NodeTask.ImageIndex = DiversityCollection.Specimen.IndexImageTransactionType("IPM", false);
                                NodeTask.SelectedImageIndex = DiversityCollection.Specimen.IndexImageTransactionType("IPM", false);
                                break;
                            default:
                                NodeTask.ImageIndex = DiversityCollection.Specimen.IndexImageTransactionType("Task", false);
                                NodeTask.SelectedImageIndex = DiversityCollection.Specimen.IndexImageTransactionType("Task", false);
                                //NodeTask.ForeColor = System.Drawing.Color.Gray;
                                break;
                        }
                        NodeTask.ForeColor = System.Drawing.Color.Gray;
                        NodeTask.Tag = R[1].ToString();
                        pNode.Nodes.Add(NodeTask);
                    }
                }

            }
            //else
            //{
            //    if (this.TaskNodes().ContainsKey(TransactionID))
            //    {
            //        foreach (System.Windows.Forms.TreeNode node in this.TaskNodes()[TransactionID])
            //        {
            //            pNode.Nodes.Add(node);
            //        }
            //    }
            //}
        }




        #endregion

        public override void buildHierarchy()
        {
            #region Old version
            
            //this._TreeView.Nodes.Clear();
            //// getting the column names
            //string ColumnID = this.ColumnID;
            //string ColumnParentID = this.ColumnParentID;
            //string ColumnDisplayText = this.ColumnDisplayText;
            //if (this._DataTable.Rows.Count > 0)
            //{
            //    System.Windows.Forms.TreeNode rootNode = new System.Windows.Forms.TreeNode(this._DataTable.Rows[0][ColumnDisplayText].ToString());
            //    System.Data.DataRow[] rrAll = this._DataTable.Select("", this.ColumnDisplayOrder + ", " + this.ColumnDisplayText);
            //    foreach (System.Data.DataRow r in rrAll)// this._DataTable.Rows)
            //    {
            //        try
            //        {
            //            if (r.RowState != System.Data.DataRowState.Deleted)
            //            {
            //                if (!r[this.ColumnParentID].Equals(System.DBNull.Value))
            //                {
            //                    int ID = System.Int32.Parse(r[this.ColumnParentID].ToString());
            //                    System.Data.DataRow[] rr = this._DataTable.Select(this.ColumnID + " = " + ID.ToString());
            //                    if (rr.Length == 0)
            //                        r[this.ColumnParentID] = System.DBNull.Value;
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //        }
            //    }
            //    // prepare SubstrateID: set SubstrateID at max DisplayOrder to NULL if no starting point can be found
            //    System.Data.DataRow[] rrS = this._DataTable.Select(this.ColumnParentID + " IS NULL", this.ColumnDisplayOrder + ", " + this.ColumnDisplayText, System.Data.DataViewRowState.CurrentRows);
            //    if (rrS.Length == 0)
            //    {
            //        System.Data.DataRow[] rrD = this._DataTable.Select("", this.ColumnDisplayOrder + " DESC", System.Data.DataViewRowState.CurrentRows);
            //        if (rrD.Length > 0)
            //        {
            //            System.Data.DataRow rrDMax = rrD[0];
            //            rrDMax[this.ColumnParentID] = System.DBNull.Value;
            //        }
            //    }
            //    foreach (System.Data.DataRow r in rrAll)// this._DataTable.Rows)
            //    {
            //        try
            //        {
            //            if (r.RowState != System.Data.DataRowState.Deleted)
            //            {
            //                if (r[this.ColumnParentID].Equals(System.DBNull.Value))
            //                {
            //                    string NodeText = r[this.ColumnDisplayText].ToString();
            //                    if (this._IncludeIDinTreeview) NodeText += "     [" + r[this.ColumnID].ToString() + "]";
            //                    NodeText += "      ";
            //                    System.Windows.Forms.TreeNode Node = new System.Windows.Forms.TreeNode(NodeText);
            //                    Node.Tag = r;
            //                    System.Drawing.Font F = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //                    Node.NodeFont = F;
            //                    //rootNode.Nodes.Add(Node);
            //                    this._TreeView.Nodes.Add(Node);
            //                    this.addHierarchyNodes(System.Int32.Parse(r[this.ColumnID].ToString()), Node);
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //        }
            //    }
            //    this.markHierarchyNodes();
            //    this._TreeView.ExpandAll();
            //}
            
            #endregion

            /// TODO: muss noch getestet werden - evtl. aendert sich die Datengrundlage, dann passen die DataRows nicht mehr die an den nodes haengen
            //if (this.ID != null && this.HierarchyContainsCurrent((int)this.ID))
            //    return;
            this._TreeView.SuspendLayout();
            this._TreeView.Nodes.Clear();

            // transferred to fillDependentTables as hierarchy may not be accessed
            // this.NonAccessibleIDs = null;  

            if (this._DataTable.Rows.Count == 0)
                return;

            // Getting the root rows sorted according to the definition in TransactionDisplaySorter
            System.Collections.Generic.SortedList<string, System.Data.DataRow> _RootRows = new SortedList<string, System.Data.DataRow>();
            foreach (System.Data.DataRow RT in this._DataTable.Rows) //Test 2020-07-15: 4067
            {
                if (RT["ParentTransactionID"].Equals(System.DBNull.Value)) //Test 2020-07-15: ID = 3918
                {
                    string Type = RT["TransactionType"].ToString();
                    int TransactionID = int.Parse(RT["TransactionID"].ToString());
                    string Title = RT["TransactionTitle"].ToString();
                    string Date = DiversityCollection.LookupTable.TransactionDateAsString(TransactionID, "BeginDate", false);
                    if (Date.Length == 0)
                    {
                        string EndDate = DiversityCollection.LookupTable.TransactionDateAsString(TransactionID, "AgreedEndDate", false);
                        if (EndDate.Length > 0)
                            Date = "- " + EndDate;
                        else
                            Date = "0000-00-00";
                    }
                    string Counter = _RootRows.Count.ToString();
                    if (_RootRows.Count < 10)
                        Counter = "00" + Counter;
                    else if (_RootRows.Count < 100)
                        Counter = "0" + Counter;
                    Counter = "     [" + Counter + "]";
                    if (DiversityCollection.LookupTable.TransactionDisplaySorting().ContainsKey(Type))
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.LookupTable.TransactionDisplaySorter> TS in DiversityCollection.LookupTable.TransactionDisplaySorting())
                        {
                            try
                            {
                                if (TS.Key == Type)
                                {
                                    switch (TS.Value)
                                    {
                                        case LookupTable.TransactionDisplaySorter.BeginDate:
                                            if (!_RootRows.ContainsKey(Date + "; " + Title))
                                                _RootRows.Add(Date + "; " + Title, RT);
                                            else
                                                _RootRows.Add(Date + " " + _RootRows.Count.ToString() + "; " + Title, RT);
                                            break;
                                        case LookupTable.TransactionDisplaySorter.TransactionTitle:
                                            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionIncludeDateInTitle)
                                            {
                                                if (!_RootRows.ContainsKey(DiversityCollection.LookupTable.TransactionTitle(TransactionID) + "; " + Date))
                                                    _RootRows.Add(DiversityCollection.LookupTable.TransactionTitle(TransactionID) + "; " + Date, RT);
                                                else
                                                {
                                                    _RootRows.Add(DiversityCollection.LookupTable.TransactionTitle(TransactionID) + "; " + Date + Counter, RT);
                                                }
                                            }
                                            else
                                            {
                                                if (!_RootRows.ContainsKey(DiversityCollection.LookupTable.TransactionTitle(TransactionID)))
                                                    _RootRows.Add(DiversityCollection.LookupTable.TransactionTitle(TransactionID), RT);
                                                else
                                                {
                                                    _RootRows.Add(DiversityCollection.LookupTable.TransactionTitle(TransactionID) + Counter, RT);
                                                }
                                            }
                                            break;
                                        default:
                                            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionIncludeDateInTitle)
                                                _RootRows.Add(RT["TransactionTitle"].ToString() + "; " + Date + Counter, RT);
                                            else
                                                _RootRows.Add(RT["TransactionTitle"].ToString() + ";" + Counter, RT);
                                            break;
                                    }
                                    break;
                                }
                            }
                            catch (System.Exception ex) { }
                        }
                    }
                    else
                    {
                        if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionIncludeDateInTitle)
                        {
                            if (!_RootRows.ContainsKey(RT["TransactionTitle"].ToString() + "; " + Date))
                                _RootRows.Add(RT["TransactionTitle"].ToString() + "; " + Date , RT);
                            else if (!_RootRows.ContainsKey(RT["TransactionTitle"].ToString() + "; " + Date + Counter))
                                _RootRows.Add(RT["TransactionTitle"].ToString() + "; " + Date + Counter, RT);
                            else
                                _RootRows.Add(_RootRows.Count.ToString() + ". " + RT["TransactionTitle"].ToString() + "; " + Date + " ", RT);
                        }
                        else
                        {
                            if (!_RootRows.ContainsKey(RT["TransactionTitle"].ToString()))
                                _RootRows.Add(RT["TransactionTitle"].ToString(), RT);
                            else if (!_RootRows.ContainsKey(RT["TransactionTitle"].ToString() + Counter))
                                _RootRows.Add(RT["TransactionTitle"].ToString() + " " + _RootRows.Count.ToString(), RT);
                            else
                                _RootRows.Add(_RootRows.Count.ToString() + ". " + RT["TransactionTitle"].ToString(), RT);
                        }
                    }
                }
            }

            if (_RootRows.Count == 0 && this._DataTable.Rows.Count > 0)
            {
                System.Data.DataRow[] rr = this._DataTable.Select("TransactionID = " + this._ID.ToString());
                if (rr.Length == 1)
                {
                    _RootRows.Add(rr[0]["TransactionTitle"].ToString(), rr[0]);
                }
            }

            if (_RootRows.Count > 0)
            {
                // for every root row, create a node in the tree and attach its children
                foreach (System.Collections.Generic.KeyValuePair<string, System.Data.DataRow> KV in _RootRows) //Test 2020-07-15: 1
                {
                    try
                    {
                        if (KV.Value.RowState != System.Data.DataRowState.Deleted)
                        {
                            string NodeText = KV.Value[this.ColumnDisplayText].ToString();
                            if (this._IncludeIDinTreeview)
                                NodeText += "     [" + KV.Value[this.ColumnID].ToString() + "]";
                            //NodeText += "      ";
                            for (int i = 0; i < NodeText.Length; i++)
                            {
                                if (i % 4 == 0) NodeText += " ";
                            }
                            if (this.NonAccessibleIDs.Contains(int.Parse(KV.Value[this.ColumnID].ToString())))// _RootRowsNoAccess.ContainsKey(KV.Key))
                                NodeText = KV.Key;
                            System.Windows.Forms.TreeNode rootNode = new System.Windows.Forms.TreeNode(NodeText);
                            rootNode.Tag = KV.Value;
                            if (!this.NonAccessibleIDs.Contains(int.Parse(KV.Value[this.ColumnID].ToString())))//_RootRowsNoAccess.ContainsKey(KV.Key))
                            {
                                System.Drawing.Font F = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                                rootNode.NodeFont = F;
                            }
                            else
                                rootNode.ForeColor = System.Drawing.Color.Gray;
                            this._TreeView.Nodes.Add(rootNode);
                            int ParentID = int.Parse(KV.Value["TransactionID"].ToString());
                            this.addHierarchyNodes(ParentID, rootNode);
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }

                this.markHierarchyNodes();
                this._TreeView.ExpandAll();
                this._TreeView.ResumeLayout();
            }
            else if (this.ShowHierarchy)
            {
                ////System.Windows.Forms.MessageBox.Show("You have no access to the basic transaction. Please turn to your administrator for access");
            }
        }

        private bool HierarchyContainsCurrent(int ID)
        {
            bool HierarchyContainsNode = false;
            foreach (System.Windows.Forms.TreeNode N in this._TreeView.Nodes)
            {
                if (N.Tag != null)
                {
                    try
                    {
                        System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                        if (R["TransactionID"].ToString() == ID.ToString())
                        {
                            this._TreeView.SelectedNode = N;
                            HierarchyContainsNode = true;
                            break;
                        }
                    }
                    catch { }
                }
                foreach (System.Windows.Forms.TreeNode NN in N.Nodes)
                {
                    HierarchyContainsNode = this.HierarchyContainsCurrent(ID);
                    if (HierarchyContainsNode)
                        break;
                }
            }
            return HierarchyContainsNode;
        }

        protected override void addHierarchyNodes(int ParentID, System.Windows.Forms.TreeNode pNode, bool IncludeAllChildren = true)
        {
            System.Data.DataRow[] RR = this._DataTable.Select(this.ColumnParentID + " = " + ParentID.ToString());
            System.Collections.Generic.List<System.Data.DataRow> rr = new List<System.Data.DataRow>();
            foreach(System.Data.DataRow R in RR)
                rr.Add(R);
            try
            {
                // Getting the rows sorted according to the definition in TransactionDisplaySorter
                System.Collections.Generic.SortedList<string, System.Data.DataRow> _CurrentRows = new SortedList<string, System.Data.DataRow>();
                foreach (System.Collections.Generic.KeyValuePair<string, LookupTable.TransactionDisplaySorter> KVSorter in DiversityCollection.LookupTable.TransactionDisplaySorting())
                {
                    _CurrentRows = new SortedList<string, System.Data.DataRow>();
                    foreach (System.Data.DataRow r in rr)
                    {
                        string Type = r["TransactionType"].ToString();
                        if (Type != KVSorter.Key)// || DiversityCollection.LookupTable.TransactionDisplaySorting().ContainsKey(Type))
                            continue;

                        int TransactionID = int.Parse(r["TransactionID"].ToString());
                        string Title = r["TransactionTitle"].ToString().Trim();
                        string Date = DiversityCollection.LookupTable.TransactionDateAsString(TransactionID, "BeginDate", false);
                        if (Date.Length == 0)
                        {
                            string EndDate = DiversityCollection.LookupTable.TransactionDateAsString(TransactionID, "AgreedEndDate", false);
                            if (EndDate.Length > 0)
                                Date = "- " + EndDate;
                            else
                                Date = "0000-00-00";
                        }

                        string Counter = _CurrentRows.Count.ToString();
                        if (_CurrentRows.Count < 10)
                            Counter = "00" + Counter;
                        else if (_CurrentRows.Count < 100)
                            Counter = "0" + Counter;
                        Counter = "          [" + Counter + "]";

                        switch (KVSorter.Value)
                        {
                            case LookupTable.TransactionDisplaySorter.BeginDate:
                                if (!_CurrentRows.ContainsKey(Date + "; " + Title))
                                    _CurrentRows.Add(Date + "; " + Title, r);
                                else
                                    _CurrentRows.Add(Date + Counter + "; " + Title, r);
                                break;
                            case LookupTable.TransactionDisplaySorter.TransactionTitle:
                                if (!_CurrentRows.ContainsKey(DiversityCollection.LookupTable.TransactionTitle(TransactionID)))
                                    _CurrentRows.Add(DiversityCollection.LookupTable.TransactionTitle(TransactionID), r);
                                else
                                    _CurrentRows.Add(DiversityCollection.LookupTable.TransactionTitle(TransactionID) + Counter, r);
                                break;
                            default:
                                _CurrentRows.Add(DiversityCollection.LookupTable.TransactionTitle(TransactionID), r);
                                break;
                        }
                    }

                    foreach (System.Collections.Generic.KeyValuePair<string, System.Data.DataRow> KV in _CurrentRows)
                    {
                        int CurrentID = System.Int32.Parse(KV.Value[this.ColumnID].ToString());
                        bool NoAccess = false;
                        //if (this.DtTransactionAccess.Select("TransactionID = " + CurrentID.ToString()).Length > 0)
                        if (this.NonAccessibleIDs.Contains(CurrentID))
                            NoAccess = true;
                        string NodeText = KV.Key;
                        if (NoAccess) NodeText = "Ø " + NodeText;
                        if (this._IncludeIDinTreeview) NodeText += "     [" + KV.Value[this.ColumnID].ToString() + "]";
                         System.Windows.Forms.TreeNode Node = new System.Windows.Forms.TreeNode(NodeText);
                        if (NoAccess)
                        {
                            Node.ForeColor = System.Drawing.Color.Gray;
                        }
                        else
                        {
                        }
                        Node.Tag = KV.Value;
                        pNode.Nodes.Add(Node);


                        this.addHierarchyNodes(CurrentID, Node);
                    }


                }

                if (this._ShowTasks)
                {
                    this.addHierarchyTaskNodes((int)this.ID, pNode); //, r["Type"].ToString());
                }

                // getting the rows NOT contained in the TransactionDisplaySorter
                _CurrentRows = new SortedList<string, System.Data.DataRow>();
                foreach (System.Data.DataRow r in rr)
                {
                    string Type = r["TransactionType"].ToString();
                    if (DiversityCollection.LookupTable.TransactionDisplaySorting().ContainsKey(Type))
                        continue;

                    string Date = DiversityCollection.LookupTable.TransactionDateAsString(TransactionID, "BeginDate", false);
                    if (Date.Length == 0)
                    {
                        string EndDate = DiversityCollection.LookupTable.TransactionDateAsString(TransactionID, "AgreedEndDate", false);
                        if (EndDate.Length > 0)
                            Date = "- " + EndDate;
                        else
                            Date = "0000-00-00";
                    }

                    string Counter = _CurrentRows.Count.ToString();
                    if (_CurrentRows.Count < 10)
                        Counter = "00" + Counter;
                    else if (_CurrentRows.Count < 100)
                        Counter = "0" + Counter;
                    Counter = "          [" + Counter + "]";

                    if (!_CurrentRows.ContainsKey(r["TransactionTitle"].ToString().Trim()))
                        _CurrentRows.Add(r["TransactionTitle"].ToString(), r);
                    else if (!_CurrentRows.ContainsKey(r["TransactionTitle"].ToString().Trim() + "; " + Date))
                        _CurrentRows.Add(r["TransactionTitle"].ToString().Trim() + "; " + Date, r);
                    else if (!_CurrentRows.ContainsKey(r["TransactionTitle"].ToString().Trim() + "; " + Date + Counter))
                        _CurrentRows.Add(r["TransactionTitle"].ToString().Trim() + "; " + Date + Counter, r);
                    else
                        _CurrentRows.Add(_CurrentRows.Count.ToString() + ". " + r["TransactionTitle"].ToString().Trim() + "; " + Date + " ", r);
                }
                foreach (System.Collections.Generic.KeyValuePair<string, System.Data.DataRow> KV in _CurrentRows)
                {
                    int CurrentID = System.Int32.Parse(KV.Value[this.ColumnID].ToString());
                    bool NoAccess = false;
                    //if (this.DtTransactionAccess.Select("TransactionID = " + CurrentID.ToString()).Length > 0)
                    if (this.NonAccessibleIDs.Contains(CurrentID))
                        NoAccess = true;
                    string NodeText = KV.Key;
                    if (NoAccess) NodeText = "Ø " + NodeText;
                    if (this._IncludeIDinTreeview) NodeText += "     [" + KV.Value[this.ColumnID].ToString() + "]";
                    System.Windows.Forms.TreeNode Node = new System.Windows.Forms.TreeNode(NodeText);
                    if (NoAccess)
                    {
                        Node.ForeColor = System.Drawing.Color.Gray;
                    }
                    else
                    {
                    }
                    Node.Tag = KV.Value;
                    pNode.Nodes.Add(Node);
                    this.addHierarchyNodes(CurrentID, Node);
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        public override void treeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            if (this._TreeView.SelectedNode != null && this._TreeView.SelectedNode.Tag != null)
            {
                System.Data.DataRow rn = (System.Data.DataRow)this._TreeView.SelectedNode.Tag;
                int AdminID = System.Int32.Parse(rn["AdministratingCollectionID"].ToString());
                int ID = System.Int32.Parse(rn[this.ColumnID].ToString());
                string SQL = "SELECT * FROM TransactionList WHERE " + this.ColumnID + " = " + ID.ToString();
                System.Data.DataTable dt = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    this._TabControl.Visible = false;
                }
                else
                {
                    this._TabControl.Visible = true;
                    if (this._ID != ID)
                    {
                        this._ID = ID;
                        this.setDependentTable(ID);
                        this.RequerySpecimenLists();
                        this.markSelectedUnitNode((int)this._ID);
                    }
                    //else
                    //{
                    //    this._ID = ID;
                    //}
                    this.setFormControls();
                    int i = 0;
                    if (this._DataTable.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow r in this._DataTable.Rows)
                        {
                            if (r.RowState != System.Data.DataRowState.Deleted)
                            {
                                if (r[this.ColumnID].ToString() == ID.ToString()) break;
                                i++;
                            }
                        }
                    }
                    try
                    {
                        this._BindingSource.Position = i;
                        if (!this._SplitContainerData.Panel2Collapsed)
                        {
                            this.setSpecimenList(ID);
                            this.ItemChanged();
                        }
                        if (this.ChildControls.Count > 0)
                        {
                            this.hideChildControls();
                        }
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
            else
                this.hideChildControls();
        }

        public bool ShowHierarchy
        {
            get
            {
                return this._MainTableContainsHierarchy;
            }
            set
            {
                this._MainTableContainsHierarchy = value;
            }
        }

        public bool ShowSpecimenLists
        {
            get
            {
                return this._ShowSpecimenLists;
            }
            set
            {
                this._ShowSpecimenLists = value;
            }
        }

        #endregion

        #region Datahandling

        public override void fillDependentTables(int ID) //Test 2020-07-15: 91, für Konvolut 3918
        {
            string SQL = "";
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

                if (this._DataSet == null) this._DataSet = new System.Data.DataSet();

                // Check Access
                SQL = "SELECT COUNT(*) FROM [TransactionList] WHERE TransactionID = " + ID.ToString();
                bool HasAccess = true;
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL) == "0")
                {
                    HasAccess = false;
                    SQL = "SELECT C.CollectionName " +
                        "FROM [Transaction] AS T INNER JOIN " +
                        "Collection AS C ON T.AdministratingCollectionID = C.CollectionID " +
                        "WHERE T.TransactionID = " + ID.ToString();
                    string AdminColl = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (AdminColl.Length > 0)
                        System.Windows.Forms.MessageBox.Show("NO ACCESS\r\nAdministrating collection:\r\n\t" + AdminColl + "\r\nYou need to be a manager of this collection");
                }

                // Documents
                SQL = "SELECT TransactionID, Date, TransactionText, TransactionDocument, InternalNotes, DisplayText, DocumentURI, DocumentType " +
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
                SQL = "SELECT TransactionID, PaymentID, Amount, ForeignAmount, ForeignCurrency, Identifier, PaymentURI, PayerName, PayerAgentURI, RecipientName, RecipientAgentURI, " +
                    "PaymentDate, PaymentDateSupplement, Notes " +
                    "FROM TransactionPayment " +
                    "WHERE TransactionID = " + ID.ToString();
                this.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterTransactionPayments, SQL, this._DataSet.Tables["TransactionPayment"]);

                // Specimen list
                SQL = "";
                if (ID == 0 && this.ID != null)
                    ID = (int)this.ID;
                System.Data.DataRow[] rr = this._DataSet.Tables["Transaction"].Select(" TransactionID = " + ID.ToString());
                if (rr.Length > 0)
                {
                    string TransactionType = rr[0]["TransactionType"].ToString().ToLower();
                    if (TransactionType == "return" || TransactionType == "forwarding")
                    {
                        System.Data.DataRow[] RR = this._DataSet.Tables["Transaction"].Select("TransactionType = '" + TransactionType + "'", "");
                        string ParentID = RR[0]["ParentTransactionID"].ToString(); // this._DataSet.Tables["Transaction"].Rows[0]["ParentTransactionID"].ToString();
                        SQL = "SELECT " + DiversityCollection.Transaction.SqlFieldsCollectionSpecimenTransaction + " FROM " + this._SpecimenTable + " WHERE TransactionID = " + ParentID;
                    }
                    else
                    {
                        SQL = "SELECT " + DiversityCollection.Transaction.SqlFieldsCollectionSpecimenTransaction + " FROM " + this._SpecimenTable + " WHERE TransactionID = " + ID.ToString();
                    }
                    this.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterCollectionSpecimenTransaction, SQL, this._DataSet.Tables["CollectionSpecimenTransaction"]);
                    this.RequerySpecimenLists();

                    if (this._DataSet.Tables["CollectionSpecimenPartList"].Rows.Count > 0)
                    {
                        System.Data.DataRow R = this._DataSet.Tables["CollectionSpecimenPartList"].Rows[0];
                        if (this._DataSet.Tables["Collection"].Rows.Count == 0)
                        {
                            ad.SelectCommand.CommandText = "select * from dbo.CollectionHierarchyAll()";
                            try
                            {
                                ad.Fill(this._DataSet.Tables["Collection"]);
                            }
                            catch { }
                        }
                    }
                    this.fillTaxonomicGroups();

                    /// ExternalIdentifier
                    SQL = "SELECT ID, ReferencedTable, ReferencedID, Type, Identifier, URL, Notes " +
                        "FROM  ExternalIdentifier " +
                        " WHERE (ReferencedID = " + this.ID.ToString() + " AND ReferencedTable = 'Transaction')";
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(this._DataSet.Tables["ExternalIdentifier"]);
                    this.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterExternalIdentifier, SQL, this._DataSet.Tables["ExternalIdentifier"]);
                }
                else
                {
                }
                this.clearBrowser();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
        }

        private void fillTaxonomicGroups()
        {
            try
            {
                string SQL = "SELECT count(distinct U.CollectionSpecimenID) AS NumberOfSpecimen, U.TaxonomicGroup  " +
                    "FROM CollectionSpecimenTransaction T INNER JOIN " +
                    "IdentificationUnit U ON T.CollectionSpecimenID = U.CollectionSpecimenID " +
                    "INNER JOIN IdentificationUnitInPart UP ON UP.IdentificationUnitID = U.IdentificationUnitID AND UP.SpecimenPartID = T.SpecimenPartID " +
                    "WHERE T.TransactionID = " + ID.ToString() + " AND UP.DisplayOrder > 0 " +
                    "AND EXISTS(SELECT CollectionSpecimenID FROM IdentificationUnitInPart L WHERE L.CollectionSpecimenID = UP.CollectionSpecimenID AND L.DisplayOrder > 0 GROUP BY L.CollectionSpecimenID HAVING MIN(L.DisplayOrder) = UP.DisplayOrder) ";
                if (this.RestrictToTaxonomicGroup.Length > 0)
                    SQL += " AND U.TaxonomicGroup = '" + this.RestrictToTaxonomicGroup + "' ";
                SQL += "GROUP BY U.TaxonomicGroup";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(this._DataSet.Tables["TaxonomicGroups"]);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void MoveParts(int? PartID, string SourceTable, string TargetTable)
        {
            try
            {
                int ss = this._DataSet.Tables[SourceTable].Rows.Count;
                int tt = this._DataSet.Tables[TargetTable].Rows.Count;
                System.Data.DataRow[] RR;
                if (PartID != null)
                    RR = this._DataSet.Tables[SourceTable].Select("SpecimenPartID = " + PartID.ToString());
                else RR = this._DataSet.Tables[SourceTable].Select("");
                for (int i = 0; i < RR.Length; i++)
                {
                    System.Data.DataRow Rtarget = this._DataSet.Tables[TargetTable].NewRow();
                    System.Data.DataRow Rsource = RR[i];
                    foreach (System.Data.DataColumn C in this._DataSet.Tables[SourceTable].Columns)
                    {
                        Rtarget[C.ColumnName] = Rsource[C.ColumnName];
                    }
                    this._DataSet.Tables[TargetTable].Rows.Add(Rtarget);
                }
                for (int i = 0; i < RR.Length; i++)
                {
                    RR[i].Delete();
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        public void SaveSentListToDatabase()
        {
            string SQL = "UPDATE CollectionSpecimenTransaction SET IsOnLoan = 0 WHERE TransactionID = " + this.ID.ToString() + 
                " AND SpecimenPartID = ";
            foreach (System.Data.DataRow R in this._DataSet.Tables["CollectionSpecimenPartSentList"].Rows)
            {
                int PartID;
                if (int.TryParse(R["SpecimenPartID"].ToString(), out PartID))
                {
                    DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL + PartID.ToString()); 
                }
            }
            this.RequerySpecimenLists();
        }

        public void RequerySpecimenLists()
        {

            this._DataSet.Tables["CollectionSpecimenPartList"].Clear();

            if (this.ShowSpecimenLists)
            {
                System.Data.DataRow[] rr = this._DataSet.Tables["Transaction"].Select("TransactionID = " + this.ID.ToString());
                string TransactionType = rr[0]["TransactionType"].ToString().ToLower();
                string ParentID = rr[0]["ParentTransactionID"].ToString();
                if (TransactionType == "return" || TransactionType == "forwarding")
                {
                    if (ParentID.Length == 0)
                    {
                        string SqlParent = "SELECT ParentTransactionID " +
                            "FROM [Transaction] AS T " +
                            "WHERE TransactionID = " + this.ID.ToString();
                        ParentID = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlParent);
                        int P;
                        if (ParentID.Length == 0 || !int.TryParse(ParentID, out P))
                            return;
                    }
                }
                else
                    ParentID = this.ID.ToString();
                string SQL = DiversityCollection.Transaction.SqlCollectionSpecimenPartListTypeIncluded(int.Parse(ParentID), false);

                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));// DiversityWorkbench.Settings.ConnectionString);
                try
                {
                    ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                    ad.Fill(this._DataSet.Tables["CollectionSpecimenPartList"]);
                }
                catch (System.Exception ex) { }
            }
            this.RequeryLoanAndReturnList();
        }

        private void RequeryLoanAndReturnList()
        {
            string SQL = "";
            try
            {
                this._DataSet.Tables["CollectionSpecimenPartOnLoanList"].Clear();
                this._DataSet.Tables["CollectionSpecimenPartReturnList"].Clear();
                this._DataSet.Tables["CollectionSpecimenPartPartialReturnList"].Clear();
                this._DataSet.Tables["CollectionSpecimenPartSentList"].Clear();
                this._DataSet.Tables["CollectionSpecimenPartForwardingList"].Clear();
                this._DataSet.Tables["CollectionSpecimenPartTransactionReturnList"].Clear();
                this._DataSet.Tables["CollectionSpecimenPartForwardingOnLoanList"].Clear();
                this._DataSet.Tables["CollectionSpecimenPartForwardingReturnedList"].Clear();

                // NEU
                if (this.ShowSpecimenLists)
                {
                    // Filling the main list
                    SQL = DiversityCollection.Transaction.SqlCollectionSpecimenPartListTypeIncluded((int)this.ID, false)
                        + " AND T.TransactionReturnID IS NULL";

                    System.Data.DataRow[] RRParent = this._DataSet.Tables["Transaction"].Select("TransactionID = " + this.ID.ToString());
                    string TransactionType = RRParent[0]["TransactionType"].ToString().ToLower();
                    string ParentID = RRParent[0]["ParentTransactionID"].ToString();
                    if (TransactionType == "return" || TransactionType == "forwarding")
                    {
                        SQL = DiversityCollection.Transaction.SqlCollectionSpecimenPartListTypeIncluded(int.Parse(ParentID), false)
                            + " AND T.TransactionReturnID IS NULL";
                    }
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    try
                    {
                        if (TransactionType != "inventory" && 
                            TransactionType != "gift" &&
                            TransactionType != "purchase")
                            ad.Fill(this._DataSet.Tables["CollectionSpecimenPartOnLoanList"]);
                    }
                    catch (System.Exception ex) { }

                    // Filling the list with returned parts
                    //CollectionSpecimenPartTransactionReturnList
                    SQL = DiversityCollection.Transaction.SqlCollectionSpecimenPartListTypeIncluded((int)this.ID, true) +
                        " AND NOT T.TransactionReturnID IS NULL";
                    if (TransactionType == "return" || TransactionType == "forwarding")
                    {
                        SQL = DiversityCollection.Transaction.SqlCollectionSpecimenPartListTypeIncluded((int)this.ID, false);
                    }
                    else if (TransactionType == "loan")
                        SQL += " AND TR.TransactionType <> 'forwarding'";

                    ad.SelectCommand.CommandText = SQL;
                    try
                    {
                        if (TransactionType == "forwarding")
                            ad.Fill(this._DataSet.Tables["CollectionSpecimenPartForwardingList"]);
                        else if (TransactionType != "inventory" && 
                            TransactionType != "gift" &&
                            TransactionType != "purchase")
                            ad.Fill(this._DataSet.Tables["CollectionSpecimenPartTransactionReturnList"]);
                    }
                    catch (System.Exception Exception) { }

                    System.Data.DataRow[] rr = this._DataSet.Tables["Transaction"].Select("TransactionID = " + this.ID.ToString());
                    //string TransactionType = rr[0]["TransactionType"].ToString().ToLower();
                    if (TransactionType.ToLower() == "forwarding")
                    {
                        foreach (System.Data.DataRow R in this._DataSet.Tables["CollectionSpecimenPartForwardingList"].Rows) //CollectionSpecimenPartReturnList"].Rows)
                        {
                            if (R["TransactionReturnID"].Equals(System.DBNull.Value))
                            {
                                System.Data.DataRow RForwardingOnLoan = this._DataSet.Tables["CollectionSpecimenPartForwardingOnLoanList"].NewRow();
                                foreach (System.Data.DataColumn C in this._DataSet.Tables["CollectionSpecimenPartForwardingOnLoanList"].Columns)
                                {
                                    RForwardingOnLoan[C.ColumnName] = R[C.ColumnName];
                                }
                                this._DataSet.Tables["CollectionSpecimenPartForwardingOnLoanList"].Rows.Add(RForwardingOnLoan);
                            }
                            else
                            {
                                System.Data.DataRow RForwardingReturned = this._DataSet.Tables["CollectionSpecimenPartForwardingReturnedList"].NewRow();
                                foreach (System.Data.DataColumn C in this._DataSet.Tables["CollectionSpecimenPartForwardingReturnedList"].Columns)
                                {
                                    RForwardingReturned[C.ColumnName] = R[C.ColumnName];
                                }
                                this._DataSet.Tables["CollectionSpecimenPartForwardingReturnedList"].Rows.Add(RForwardingReturned);
                            }
                        }
                    }
                    else if (TransactionType == "loan")
                    {
                        SQL = DiversityCollection.Transaction.SqlCollectionSpecimenPartListTypeIncluded((int)this.ID, true) +
                            " AND NOT T.TransactionReturnID IS NULL AND TR.TransactionType = 'forwarding'";
                        ad.SelectCommand.CommandText = SQL;
                        ad.Fill(this._DataSet.Tables["CollectionSpecimenPartForwardingList"]);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
        }

        protected void clearBrowser()
        {
            foreach (System.Windows.Forms.Control C in this._TabControl.Controls)
                this.clearBrowser(C);
        }

        private void clearBrowser(System.Windows.Forms.Control C)
        {
            if (C.GetType() == typeof(System.Windows.Forms.WebBrowser))
            {
                System.Windows.Forms.WebBrowser W = (System.Windows.Forms.WebBrowser)C;
                W.DocumentText = "";
            }
            if (C.Controls.Count > 0)
            {
                foreach (System.Windows.Forms.Control CC in C.Controls)
                    this.clearBrowser(CC);
            }
        }

        public override void saveDependentTables() 
        {
            try
            {
                this.FormFunctions.updateTable(this._DataSet, "TransactionDocument", this._SqlDataAdapterTransactionDocuments, this.DependentTablesBindingSources["TransactionDocument"]);
                this.FormFunctions.updateTable(this._DataSet, "TransactionAgent", this._SqlDataAdapterTransactionAgents, this.DependentTablesBindingSources["TransactionAgent"]);
                this.FormFunctions.updateTable(this._DataSet, "TransactionPayment", this._SqlDataAdapterTransactionPayments, this.DependentTablesBindingSources["TransactionPayment"]);
                this.FormFunctions.updateTable(this._DataSet, "CollectionSpecimenTransaction", this._SqlDataAdapterCollectionSpecimenTransaction, this._BindingSource);
                this._DataSet.Tables["ExternalIdentifier"].Rows[0].BeginEdit();
                this._DataSet.Tables["ExternalIdentifier"].Rows[0].EndEdit();
                this.FormFunctions.updateTable(this._DataSet, "ExternalIdentifier", this._SqlDataAdapterExternalIdentifier, this._BindingSource);
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Form functions
        /// <summary>
        /// nicht mehr verwendete Funktion - hat alle Tabs disabled wenn keine Eintraege in Documents vorhanden waren
        /// </summary>
        public void setTabControl()
        {
            if (this._TabControl != null)
            {
                //if (this._dsTransaction.TransactionHistory.Rows.Count > 0)
                if (this._DataSet.Tables["TransactionDocument"].Rows.Count > 0)
                {
                    foreach (System.Windows.Forms.Control C in this._TabControl.TabPages[1].Controls)
                        C.Enabled = false;
                    for (int i = 2; i < this._TabControl.TabPages.Count; i++)
                    {
                        foreach (System.Windows.Forms.Control C in this._TabControl.TabPages[i].Controls)
                            C.Enabled = true;
                    }
                }
                else
                {
                    foreach (System.Windows.Forms.Control C in this._TabControl.TabPages[0].Controls)
                        C.Enabled = true;
                    for (int i = 2; i < this._TabControl.TabPages.Count; i++)
                    {
                        foreach (System.Windows.Forms.Control C in this._TabControl.TabPages[i].Controls)
                            C.Enabled = false;
                    }
                }
            }
        }

        public override void setFormControls() 
        {
            DiversityCollection.Forms.FormTransaction f = (DiversityCollection.Forms.FormTransaction)this._Form;
            f.setDetailControls();
        }
        
        #endregion

        #region Properties

        public int TransactionID
        {
            get { return _TransactionID; }
            //set { _TransactionID = value; }
        }
        
        public bool IncludeTypeInformation
        {
            get { return _IncludeTypeInformation; }
            set { _IncludeTypeInformation = value; }
        }

        public System.Collections.Generic.Dictionary<string, string> TransactionParterAddressValues
        {
            get 
            {
                this._TransactionParterAddressValues = this.Agent.UnitValues((int)this.ID);
                return _TransactionParterAddressValues; 
            }
            //set { _TransactionParterAddressValues = value; }
        }

        public DiversityWorkbench.Agent Agent
        {
            get 
            {
                if (this._Agent == null)
                    this._Agent = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                return _Agent; 
            }
            //set { _Agent = value; }
        }

        public override string ColumnParentID
        {
            get
            {
                return "ParentTransactionID";
            }
        }

        public override string ColumnID
        {
            get
            {
                return "TransactionID";
            }
        }

        public override string MainTableHierarchy
        {
            get
            {
                return "TransactionHierarchy";
            }
        }

        #endregion


        #region XML

        public void writeDefaultXslt(System.IO.FileInfo File)
        {
            System.Xml.XmlWriter W;
            try
            {
                System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                settings.Encoding = System.Text.Encoding.UTF8;
                W = System.Xml.XmlWriter.Create(File.FullName, settings);
                W.WriteRaw("<?xml version=\"1.0\" encoding=\"utf-16\" ?> " +
                    "<xsl:stylesheet version=\"1.0\"  " +
                    "xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\"> " +
                    "<xsl:output method=\"xml\" encoding=\"utf-16\"/> ");
                W.WriteRaw("<xsl:template match=\"text\"></xsl:template> " +
                    "   </xsl:stylesheet>)");
                W.Close();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// If all units of a transaction should be included in the XML correspondence.
        /// #127
        /// </summary>
        private bool _AllUnits = false;

        public string XmlCorrespondence(
            System.IO.FileInfo XmlFile,
            string XsltStilesheetPath,
            DiversityCollection.Transaction.TransactionCorrespondenceType Type,
            int? NumberOfReturnedSpecimen,
            int TransactionID,
            string AdminAgentURI,
            System.DateTime StartDate,
            bool IncludeChildTransactions,
            bool IncludeSubcollections,
            bool IncludeTypeInformation,
            bool AllSpecimenOnLoan,
            Transaction.ConversionType ConversionType,
            Transaction.TransactionSpecimenOrder SpecimenOrder,
            bool AllUnits = false) // #127
        {
            this._IncludeChildTransactions = IncludeChildTransactions;
            this._IncludeSubcollections = IncludeSubcollections;
            this._IncludeTypeInformation = IncludeTypeInformation;
            this._AllSpecimenOnLoan = AllSpecimenOnLoan;
            this._ConversionType = ConversionType;
            this._SpecimenOrder = SpecimenOrder;
            this._TransactionCorrespondenceType = Type;
            this._TransactionID = TransactionID;
            this._AdminAgentURI = AdminAgentURI;
            string XML = "";
            this._AllUnits = AllUnits; // #127

            System.Xml.XmlWriter W;
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            W = System.Xml.XmlWriter.Create(XmlFile.FullName, settings);
            try
            {
                W.WriteStartDocument();
                W.WriteStartElement("TransactionList");
                this.writeXmlAdministration(ref W);
                System.Collections.Generic.List<string> HierarchyList = new List<string>();
                this.writeXmlTransaction(ref W, this._TransactionID, HierarchyList);
                W.WriteEndElement();//TransactionList
                W.WriteEndDocument();
                W.Flush();
                W.Close();
                if (XsltStilesheetPath.Length > 0)
                {
                    System.IO.FileInfo FI = new System.IO.FileInfo(XsltStilesheetPath);
                    if (FI.Exists)
                    {
                        //#127
                        System.Xml.Xsl.XslCompiledTransform XSLT = new System.Xml.Xsl.XslCompiledTransform();
                        System.Xml.Xsl.XsltSettings XsltSettings = new System.Xml.Xsl.XsltSettings(true, true);
                        System.Xml.XmlResolver resolver = new System.Xml.XmlUrlResolver();
                        XSLT.Load(XsltStilesheetPath, XsltSettings, resolver);

                        // Load the file to transform.
                        System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(XmlFile.FullName);

                        // The output file:
                        string OutputFile = XmlFile.FullName.Substring(0, XmlFile.FullName.Length
                            - XmlFile.Extension.Length) + ".htm";

                        // Create the writer.             
                        System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(OutputFile, XSLT.OutputSettings);

                        // Transform the file and send the output to the console.
                        XSLT.Transform(doc, writer);
                        writer.Close();
                        return OutputFile;
                    }
                }
                else
                {
                    XML = XmlFile.FullName;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                if (W != null)
                {
                    W.Flush();
                    W.Close();
                }
            }
            return XML;
        }

        private void writeXmlTransactionsDependingHierarchy(ref System.Xml.XmlWriter W, int TransactionID)
        {
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtTransaction.Select("ParentTransactionID = " + TransactionID.ToString(), "BeginDate, TransactionTitle"); //
            if (RR.Length > 0)
            {
                System.Collections.Generic.List<string> HierarchyList = new List<string>();
                for (int i = 0; i < RR.Length; i++)
                {
                    int ChildID;
                    if (int.TryParse(RR[i]["TransactionID"].ToString(), out ChildID))
                    {
                        HierarchyList.Clear();
                        HierarchyList.Add(RR[i]["TransactionTitle"].ToString());
                        this.writeXmlTransaction(ref W, ChildID, HierarchyList);
                    }
                }
            }
        }

        private void writeXslt(ref System.Xml.XmlWriter W, System.IO.FileInfo XsltStilesheet)
        {
            if (XsltStilesheet != null)
            {
                System.Xml.XmlReader R;
                R = System.Xml.XmlReader.Create(XsltStilesheet.FullName);
                //R.Read();
                //string Content = R.ReadInnerXml();// +"</" + NodeName + ">";
                //W.WriteRaw(Content);
                while (R.ReadState != System.Xml.ReadState.EndOfFile)
                {
                    R.Read();
                    if (R.NodeType != System.Xml.XmlNodeType.XmlDeclaration &&
                        //R.NodeType != System.Xml.XmlNodeType.ProcessingInstruction &&
                        R.NodeType != System.Xml.XmlNodeType.Whitespace)
                    {
                        if (R.NodeType == System.Xml.XmlNodeType.Element)
                        {
                            string Content = "";
                            //string NodeName = R.Name;
                            //string lineEnd = ">";
                            //string line = "<" + R.Name;
                            if (!R.IsEmptyElement)
                            {
                                Content = R.ReadInnerXml();// +"</" + NodeName + ">";
                                Content = "<xsl:stylesheet id='style_Transaction' version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>" + Content + "</xsl:stylesheet>";
                            }
                            W.WriteRaw(Content);

                            //if (R.HasAttributes)
                            //{
                            //    int AC = R.AttributeCount;
                            //    for (int i = 0; i < AC; i++)
                            //    {
                            //        R.MoveToNextAttribute();
                            //        line += " " + R.Name + "='" + R.Value + "'";
                            //    }
                            //}
                            //line += lineEnd;
                            //W.WriteRaw(line);
                            //if (R.LocalName == "output") W.WriteRaw("<" + R.Name + "id=\"style_Transaction\" version=\"1.0\" xmlns:xsl=\"" + R.NamespaceURI + "\">");
                            //if (R.LocalName == "stylesheet") W.WriteRaw("<" + R.Name + "id=\"style_Transaction\" version=\"1.0\" xmlns:xsl=\"" + R.NamespaceURI + "\">");
                            //R.GetAttribute(
                            //W.WriteAttributes(R, true);
                        }
                    }
                    //break;
                }
                //W.WriteDocType("Transaction", "xsl:stylesheet", "ID", "#IMPLIED");
            }
        }

        private void writeXmlAdministration(ref System.Xml.XmlWriter W)
        {
            if (this._AdminAgentURI == null || this._AdminAgentURI.Length == 0)
                return;
            W.WriteStartElement("Administration");
            this.writeXmlAddress(ref W, this._AdminAgentURI);
            W.WriteEndElement();//Administration

        }

        private void writeXmlTransaction(ref System.Xml.XmlWriter W, int TransactionID, System.Collections.Generic.List<string> HierarchyList)
        {
            try
            {
                if (this._DataSet.Tables["Transaction"].Rows.Count > 0)
                {
                    System.Data.DataRow[] rr = this._DataSet.Tables["Transaction"].Select("TransactionID = " + TransactionID.ToString());
                    if (rr.Length > 0)
                    {
                        W.WriteStartElement("Transaction");

                        #region Header

                        if (HierarchyList.Count == 0)
                            HierarchyList.Add(rr[0]["TransactionTitle"].ToString());
                        if (HierarchyList.Count > 0)
                        {
                            W.WriteStartElement("Hierarchy");
                            for (int i = 0; i < HierarchyList.Count; i++)
                            {
                                W.WriteStartElement("Level_" + i.ToString());
                                W.WriteElementString("TransactionTitle", HierarchyList[i]);
                                W.WriteEndElement();//Level_
                            }
                            W.WriteEndElement();//Hierarchy
                        }
                        W.WriteElementString("HierarchyLevel", HierarchyList.Count.ToString());

                        string stringTransactionType = rr[0]["TransactionType"].ToString();
                        switch (stringTransactionType.ToUpper())
                        {
                            case "BORROW":
                                this._TransactionType = Transaction.TransactionType.Borrow;
                                break;
                            case "EMBARGO":
                                this._TransactionType = Transaction.TransactionType.Embargo;
                                break;
                            case "EXCHANGE":
                                this._TransactionType = Transaction.TransactionType.Exchange;
                                break;
                            case "FORWARDING":
                                this._TransactionType = Transaction.TransactionType.Forwarding;
                                break;
                            case "GIFT":
                                this._TransactionType = Transaction.TransactionType.Gift;
                                break;
                            case "INVENTORY":
                                this._TransactionType = Transaction.TransactionType.Inventory;
                                break;
                            case "LOAN":
                                this._TransactionType = Transaction.TransactionType.Loan;
                                break;
                            case "PERMIT":
                                this._TransactionType = Transaction.TransactionType.Permit;
                                break;
                            case "PURCHASE":
                                this._TransactionType = Transaction.TransactionType.Purchase;
                                break;
                            case "REMOVAL":
                                this._TransactionType = Transaction.TransactionType.Removal;
                                break;
                            case "REQUEST":
                                this._TransactionType = Transaction.TransactionType.Request;
                                break;
                            case "RETURN":
                                this._TransactionType = Transaction.TransactionType.Return;
                                break;
                            case "GROUP":
                                this._TransactionType = Transaction.TransactionType.Group;
                                break;
                            case "VISIT":
                                this._TransactionType = Transaction.TransactionType.Visit;
                                break;
                        }
                        if (!rr[0]["ParentTransactionID"].Equals(System.DBNull.Value) && (this._TransactionType == TransactionType.Forwarding || this._TransactionType == TransactionType.Return)) // TransactionType.ToLower() == "forwarding" || TransactionType.ToLower() == "return"))
                        {
                            int ParentTransactionID = int.Parse(rr[0]["ParentTransactionID"].ToString());
                            this.writeXmlTransaction(ref W, ParentTransactionID, HierarchyList);
                        }

                        W.WriteElementString("Date", this.getDateInEnglish(System.DateTime.Now));
                        System.DateTime Begin = System.DateTime.Now;
                        System.DateTime End = System.DateTime.Now;
                        System.DateTime ActEnd = System.DateTime.Now;
                        bool DatePeriodOK = true;
                        bool DateProlongOK = true;
                        System.Data.DataRow R = rr[0];
                        // Data besides From and To
                        foreach (System.Data.DataColumn C in R.Table.Columns)
                        {
                            if (!R[C.ColumnName].Equals(System.DBNull.Value) &&
                                R[C.ColumnName].ToString().Length > 0 &&
                                C.ColumnName != "TransactionID" &&
                                C.ColumnName != "PreceedingTransactionID" &&
                                C.ColumnName != "FromCollectionID" &&
                                C.ColumnName != "FromTransactionPartnerAgentURI" &&
                                C.ColumnName != "ToCollectionID" &&
                                C.ColumnName != "ToTransactionPartnerAgentURI" &&
                                C.ColumnName != "ResponsibleAgentURI" &&
                                !C.ColumnName.StartsWith("From") &&
                                !C.ColumnName.StartsWith("To"))
                            {
                                if (C.ColumnName == "BeginDate" || C.ColumnName == "AgreedEndDate" || C.ColumnName == "ActualEndDate")
                                {
                                    System.DateTime d;
                                    if (System.DateTime.TryParse(R[C.ColumnName].ToString(), out d))
                                        W.WriteElementString(C.ColumnName, this.getDateInEnglish(d));
                                    if (C.ColumnName == "BeginDate")
                                    {
                                        if (!System.DateTime.TryParse(R[C.ColumnName].ToString(), out Begin))
                                            DatePeriodOK = false;
                                    }
                                    if (C.ColumnName == "AgreedEndDate")
                                    {
                                        if (!System.DateTime.TryParse(R[C.ColumnName].ToString(), out End))
                                        {
                                            DatePeriodOK = false;
                                            DateProlongOK = false;
                                        }
                                    }
                                    if (C.ColumnName == "ActualEndDate")
                                    {
                                        if (!System.DateTime.TryParse(R[C.ColumnName].ToString(), out ActEnd))
                                        {
                                            DatePeriodOK = false;
                                            DateProlongOK = false;
                                        }
                                    }
                                }
                                else
                                    W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                            }
                        }
                        if (DatePeriodOK)
                        {
                            System.DateTime dtBegin = Begin;
                            System.DateTime dtEnd = End;
                            // Test for Years
                            int iYears = 0;
                            int iMonth = 0;
                            bool PeriodIsInYears = false;
                            while (dtBegin < dtEnd)
                            {
                                iYears++;
                                dtBegin = dtBegin.AddYears(1);
                                if (dtBegin == dtEnd)
                                {
                                    PeriodIsInYears = true;
                                    break;
                                }
                            }
                            if (PeriodIsInYears)
                            {
                                iMonth = iYears * 12;
                            }
                            else
                            {
                                // Calculate months
                                // Markus 29.1.2018 - Neue Version - Start: {1/29/2018 8:04:07 AM} End: {7/29/2018 12:00:00 AM} - andere Berechnung lieferte 7 Monate
                                dtBegin = Begin; 
                                int iD = dtEnd.Day - Begin.Day;
                                int iM = dtEnd.Month - Begin.Month;
                                int iY = dtEnd.Year - Begin.Year;
                                iMonth = iM + iY * 12;

                                if (iD > 28)
                                    iMonth++;
                            }
                            W.WriteElementString("DurationInMonths", iMonth.ToString());
                        }
                        if (DateProlongOK)
                        {
                            System.TimeSpan P = ActEnd - End;
                            int Months = P.Days / 30;
                            if (Months > 0)
                                W.WriteElementString("ProlongationInMonths", Months.ToString());
                        }

                        #endregion

                        #region From

                        if (this._TransactionType == TransactionType.Return)// TransactionType.ToLower() == "return")
                        {
                            int ParentID;
                            if (!rr[0]["ParentTransactionID"].Equals(System.DBNull.Value) && int.TryParse(rr[0]["ParentTransactionID"].ToString(), out ParentID))
                            {
                                System.Data.DataRow[] rrP = this._DataSet.Tables["Transaction"].Select("TransactionID = " + ParentID.ToString());
                                if (rrP.Length > 0)
                                {
                                    W.WriteStartElement("From");
                                    //string FromAgentURI = this.AddressURI(null, "From");
                                    string FromAgentURI = this.AddressURI(ParentID, "From");
                                    if (FromAgentURI.Length > 0)
                                    {
                                        W.WriteStartElement("Address");
                                        this.writeXmlAddress(ref W, FromAgentURI);
                                        W.WriteEndElement();//Address
                                    }
                                    else if (this.AddressText("From").Length > 0)
                                    {
                                        W.WriteStartElement("Address");
                                        W.WriteElementString("Address", this.AddressText("From"));
                                        W.WriteEndElement();//Address
                                    }
                                    foreach (System.Data.DataColumn C in R.Table.Columns)
                                    {
                                        if (C.ColumnName.StartsWith("From")
                                            && C.ColumnName != "FromCollectionID"
                                            && C.ColumnName != "FromTransactionPartnerAgentURI"
                                            && C.ColumnName != "FromTransactionPartnerName"
                                            && R[C.ColumnName].ToString().Trim().Length > 0)
                                        {
                                            W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                                        }
                                        if (!R[C.ColumnName].Equals(System.DBNull.Value) &&
                                           R[C.ColumnName].ToString().Length > 0 &&
                                           (C.ColumnName == "FromCollectionID"))
                                        {
                                            int CollectionID = 0;
                                            if (int.TryParse(R[C.ColumnName].ToString(), out CollectionID))
                                            {
                                                this.writeXmlCollection(ref W, CollectionID);
                                            }
                                        }
                                    }
                                    W.WriteEndElement();//From
                                }
                            }
                        }
                        else if (this._TransactionType != TransactionType.Inventory && this._TransactionType != TransactionType.Visit)// TransactionType.ToLower() != "inventory" && TransactionType.ToLower() != "visit")
                        {
                            W.WriteStartElement("From");
                            string FromAgentURI = this.AddressURI(null, "From");
                            if (FromAgentURI.Length > 0)
                            {
                                W.WriteStartElement("Address");
                                this.writeXmlAddress(ref W, FromAgentURI);
                                W.WriteEndElement();//Address
                            }
                            else if (this.AddressText("From").Length > 0)
                            {
                                W.WriteStartElement("Address");
                                W.WriteElementString("Address", this.AddressText("From"));
                                W.WriteEndElement();//Address
                                //this.writeXmlAddress(ref W, this.AddressText("From"));
                            }
                            foreach (System.Data.DataColumn C in R.Table.Columns)
                            {
                                if (C.ColumnName.StartsWith("From")
                                    && C.ColumnName != "FromCollectionID"
                                    && C.ColumnName != "FromTransactionPartnerAgentURI"
                                    && C.ColumnName != "FromTransactionPartnerName"
                                    && R[C.ColumnName].ToString().Trim().Length > 0)
                                {
                                    W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                                }
                                if (!R[C.ColumnName].Equals(System.DBNull.Value) &&
                                   R[C.ColumnName].ToString().Length > 0 &&
                                   (C.ColumnName == "FromCollectionID"))
                                {
                                    int CollectionID = 0;
                                    if (int.TryParse(R[C.ColumnName].ToString(), out CollectionID))
                                    {
                                        this.writeXmlCollection(ref W, CollectionID);
                                    }
                                }
                            }
                            W.WriteEndElement();//From
                        }

                        #endregion

                        #region To

                        // TransactionType may have been changed in forwarding due to generation of parent address - so check again
                        if (rr[0]["TransactionType"].ToString().ToLower() != this._TransactionType.ToString().ToLower() 
                            && rr[0]["TransactionType"].ToString().ToLower() == "forwarding"
                            && this._TransactionType == TransactionType.Loan)
                            this._TransactionType = TransactionType.Forwarding;

                        W.WriteStartElement("To");
                        string ToAgentURI = "";
                        if ((this._TransactionType != TransactionType.Forwarding && this._TransactionType != TransactionType.Return) && TransactionID != this.TransactionID)//((TransactionType.ToLower() != "forwarding" && TransactionType.ToLower() != "return") && TransactionID != this.TransactionID)
                        {
                            ToAgentURI = this.AddressURI(TransactionID, "To");
                        }
                        else if (!rr[0]["ParentTransactionID"].Equals(System.DBNull.Value) && (_TransactionType != TransactionType.Forwarding && this._TransactionType != TransactionType.Return)) //(!rr[0]["ParentTransactionID"].Equals(System.DBNull.Value) && (TransactionType.ToLower() != "forwarding" && TransactionType.ToLower() != "return"))
                        {
                            int ParentTransactionID = int.Parse(rr[0]["ParentTransactionID"].ToString());
                            ToAgentURI = this.AddressURI(ParentTransactionID, "To");
                            if (_TransactionType == TransactionType.Exchange)
                            {

                            }
                        }
                        else if (_TransactionType == TransactionType.Forwarding)
                        {
                            ToAgentURI = this.AddressURI(TransactionID, "To");
                        }

                        if (ToAgentURI.Length == 0)
                            ToAgentURI = this.AddressURI(null, "To");
                        if (ToAgentURI.Length > 0)
                        {
                            W.WriteStartElement("Address");
                            this.writeXmlAddress(ref W, ToAgentURI);
                            W.WriteEndElement();//Address
                        }
                        else if (this.AddressText("To").Length > 0)
                        {
                            W.WriteStartElement("Address");
                            W.WriteElementString("Address", this.AddressText("To"));
                            W.WriteEndElement();//Address
                            //W.WriteStartElement("Address");
                            //this.writeXmlAddress(ref W, this.AddressText("To"));
                            //W.WriteEndElement();
                        }
                        foreach (System.Data.DataColumn C in R.Table.Columns)
                        {
                            if (C.ColumnName.StartsWith("To")
                                && C.ColumnName != "ToCollectionID"
                                && C.ColumnName != "ToTransactionPartnerAgentURI"
                                && C.ColumnName != "ToTransactionPartnerName"
                                && R[C.ColumnName].ToString().Trim().Length > 0)
                            {
                                W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                            }
                            if (!R[C.ColumnName].Equals(System.DBNull.Value) &&
                               R[C.ColumnName].ToString().Length > 0 &&
                               (C.ColumnName == "ToCollectionID"))
                            {
                                int CollectionID = 0;
                                if (int.TryParse(R[C.ColumnName].ToString(), out CollectionID))
                                {
                                    this.writeXmlCollection(ref W, CollectionID);
                                }
                            }
                            else if (R[C.ColumnName].Equals(System.DBNull.Value) &&
                               R[C.ColumnName].ToString().Length == 0 &&
                               (C.ColumnName == "ToCollectionID"))
                            {
                                int? CollID = DiversityCollection.LookupTable.TransactionCollectionID(TransactionID, false);
                                if (CollID != null)
                                    this.writeXmlCollection(ref W, (int)CollID);
                            }
                        }
                        W.WriteEndElement();//To

                        #endregion

                        #region Administration

                        W.WriteStartElement("Administration");
                        if (DiversityCollection.LookupTable.TransactionAdministratingCollectionID(TransactionID) != null)
                        {
                            int TransactionAdministrativeCollectionID = (int)DiversityCollection.LookupTable.TransactionAdministratingCollectionID(TransactionID);
                            this.writeXmlCollection(ref W, TransactionAdministrativeCollectionID);
                        }
                        W.WriteEndElement();//Administration
                        #endregion

                        #region Numbers

                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                        string SQL = "";
                        int CurrentCount = 0;
                        int CurrentStock = 0;
                        SQL = "SELECT COUNT(*) AS Count, SUM(CASE WHEN P.Stock IS NULL THEN 1 ELSE Stock END) AS Stock " +
                            "FROM CollectionSpecimenTransaction T INNER JOIN " +
                            "CollectionSpecimenPart P ON T.CollectionSpecimenID = P.CollectionSpecimenID AND " +
                            "T.SpecimenPartID = P.SpecimenPartID " +
                            "WHERE  T.TransactionID = " + TransactionID.ToString();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        System.Data.DataTable dt = new System.Data.DataTable();
                        ad.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (int.TryParse(dt.Rows[0][0].ToString(), out CurrentCount))
                            {
                                if (this._TransactionType == TransactionType.Exchange && CurrentCount == 0)
                                {
                                    W.WriteElementString("NumberOfSpecimenInitial", R["NumberOfUnits"].ToString());
                                }
                                else
                                    W.WriteElementString("NumberOfSpecimenInitial", CurrentCount.ToString());
                            }
                            if (int.TryParse(dt.Rows[0][1].ToString(), out CurrentStock))
                                W.WriteElementString("NumberOfStockInitial", CurrentStock.ToString());
                        }

                        int StockOnLoan = 0;
                        SQL = "SELECT COUNT(*) AS Count, SUM(CASE WHEN P.Stock IS NULL THEN 1 ELSE Stock END) AS Stock " +
                            "FROM CollectionSpecimenTransaction T INNER JOIN " +
                            "CollectionSpecimenPart P ON T.CollectionSpecimenID = P.CollectionSpecimenID AND " +
                            "T.SpecimenPartID = P.SpecimenPartID ";
                        if (this._AllSpecimenOnLoan)
                            SQL += "WHERE T.TransactionID = " + TransactionID.ToString();
                        else
                            SQL += "WHERE T.TransactionReturnID IS NULL AND T.TransactionID = " + TransactionID.ToString();

                        if (this._CurrentShippedSpecimenParts.Length > 0 && !this._AllSpecimenOnLoan)
                        {
                            if (_TransactionType != TransactionType.Forwarding)//(TransactionType.ToLower() != "forwarding")
                                SQL += " AND P.SpecimenPartID NOT IN (" + this._CurrentShippedSpecimenParts + ")";
                            else
                            {
                                SQL += " AND P.SpecimenPartID IN (" + this._CurrentShippedSpecimenParts + ")";
                            }
                        }
                        ad.SelectCommand.CommandText = SQL;
                        dt.Clear();
                        ad.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (int.TryParse(dt.Rows[0][0].ToString(), out this._PartsOnLoan))
                                W.WriteElementString("NumberOfSpecimenOnLoan", this._PartsOnLoan.ToString());
                            if (int.TryParse(dt.Rows[0][1].ToString(), out StockOnLoan))
                                 W.WriteElementString("NumberOfStockOnLoan", StockOnLoan.ToString());
                        }

                        if (this._TransactionType == TransactionType.Forwarding)
                        {
                            SQL = "SELECT COUNT(*) AS Count FROM CollectionSpecimenTransaction T INNER JOIN CollectionSpecimenPart P ON T.CollectionSpecimenID = P.CollectionSpecimenID AND T.SpecimenPartID = P.SpecimenPartID " +
                                "WHERE T.TransactionID = " + TransactionID.ToString();
                            int iFor;
                            if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out iFor))
                            {
                                W.WriteElementString("NumberOfStockForwarded", iFor.ToString());
                            }
                        }

                        if (!this._AllSpecimenOnLoan)
                        {
                            int NumberReturned = 0;
                            int StockReturned = 0;
                            SQL = "SELECT COUNT(*) AS Count, SUM(CASE WHEN P.Stock IS NULL THEN 1 ELSE Stock END) AS Stock " +
                               " FROM CollectionSpecimenTransaction T INNER JOIN " +
                               " CollectionSpecimenPart P ON T.CollectionSpecimenID = P.CollectionSpecimenID AND " +
                               " T.SpecimenPartID = P.SpecimenPartID " +
                               " WHERE T.TransactionID = " + TransactionID.ToString() +
                               " AND (NOT T.TransactionReturnID IS NULL ";
                            if (this._CurrentShippedSpecimenParts.Length > 0 && _TransactionType != TransactionType.Forwarding)// TransactionType.ToLower() != "forwarding")
                                SQL += " OR T.SpecimenPartID IN (" + this._CurrentShippedSpecimenParts + ")";
                            SQL += ")";
                            ad.SelectCommand.CommandText = SQL;
                            dt.Clear();
                            ad.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                if (int.TryParse(dt.Rows[0][0].ToString(), out this._PartsReturned))
                                    W.WriteElementString("NumberOfSpecimenReturned", this._PartsReturned.ToString());
                                if (int.TryParse(dt.Rows[0][1].ToString(), out StockReturned))
                                    W.WriteElementString("NumberOfStockReturned", StockReturned.ToString());
                            }

                            Microsoft.Data.SqlClient.SqlCommand COM = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                            con.Open();
                            SQL = "SELECT COUNT(*) FROM [CollectionSpecimenTransaction] WHERE TransactionID = " + TransactionID.ToString();
                            if (this._CurrentShippedSpecimenParts.Length > 0)
                                SQL += " AND SpecimenPartID IN (" + this._CurrentShippedSpecimenParts + ")";
                            if (_TransactionType == TransactionType.Forwarding)// TransactionType.ToLower() == "forwarding")
                                SQL += " AND NOT TransactionReturnID IS NULL ";
                            COM.CommandText = SQL;
                            if (int.TryParse(COM.ExecuteScalar().ToString(), out this._PartsShipped))
                                W.WriteElementString("NumberOfSpecimenReturnedNow", this._PartsShipped.ToString());
                        }
                        con.Close();

                        #endregion

                        this.writeXmlTaxonomicGroups(ref W);

                        if (this._SpecimenOrder == TransactionSpecimenOrder.Taxa)
                        {
                            this.writeXmlSpecimenParts(ref W, TransactionID, false, SpecimenSorting.Identification);
                            //this.writeXmlSpecimenPartsByTaxa(ref W, TransactionID);
                        }
                        else if (this._SpecimenOrder == TransactionSpecimenOrder.AccessionNumber)
                        {
                            //this.writeXmlSpecimenParts(ref W, TransactionID);
                            this.writeXmlSpecimenParts(ref W, TransactionID, false, SpecimenSorting.AccessionNumber);
                        }
                        else if (this._SpecimenOrder == TransactionSpecimenOrder.TaxaSingleAccessionNumber)
                        {
                            this.writeXmlSpecimenParts(ref W, TransactionID, true, SpecimenSorting.Identification);
                            //this.writeXmlSpecimenPartsSingleNumbers(ref W, TransactionID, "LastIdentificationCache, MaterialCategory, AccessionNumber");
                        }
                        else
                        {
                            this.writeXmlSpecimenParts(ref W, TransactionID, true, SpecimenSorting.AccessionNumber);
                            //this.writeXmlSpecimenPartsSingleNumbers(ref W, TransactionID);
                        }

                        // Hierarchy
                        if (this._IncludeChildTransactions)
                        {
                            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtTransaction.Select("ParentTransactionID = " + TransactionID.ToString(), "BeginDate, TransactionTitle"); //
                            if (RR.Length > 0)
                            {
                                //if (HierarchyList.Count == 0)
                                //    HierarchyList.Add(rr[0]["TransactionTitle"].ToString());
                                System.Collections.Generic.List<string> NextHierarchy = new List<string>();
                                for (int i = 0; i < RR.Length; i++)
                                {
                                    NextHierarchy.Clear();
                                    foreach (string s in HierarchyList)
                                        NextHierarchy.Add(s);
                                    NextHierarchy.Add(RR[i]["TransactionTitle"].ToString());
                                    int ChildID;
                                    if (int.TryParse(RR[i]["TransactionID"].ToString(), out ChildID))
                                    {
                                        this.writeXmlTransaction(ref W, ChildID, NextHierarchy);
                                    }
                                }
                            }
                        }

                        W.WriteEndElement();//Transaction

                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void writeXmlTransaction(ref System.Xml.XmlWriter W, int TransactionID)
        {
            if (this._DataSet.Tables["Transaction"].Rows.Count > 0)
            {
                System.Data.DataRow[] rr = this._DataSet.Tables["Transaction"].Select("TransactionID = " + TransactionID.ToString());
                if (rr.Length > 0)
                {
                    W.WriteElementString("Date", this.getDateInEnglish(System.DateTime.Now));
                    System.DateTime Begin = System.DateTime.Now;
                    System.DateTime End = System.DateTime.Now;
                    bool DatePeriodOK = true;
                    System.Data.DataRow R = rr[0];
                    // Data besides From and To
                    foreach (System.Data.DataColumn C in R.Table.Columns)
                    {
                        if (!R[C.ColumnName].Equals(System.DBNull.Value) &&
                            R[C.ColumnName].ToString().Length > 0 &&
                            C.ColumnName != "TransactionID" &&
                            C.ColumnName != "PreceedingTransactionID" &&
                            C.ColumnName != "FromCollectionID" &&
                            C.ColumnName != "FromTransactionPartnerAgentURI" &&
                            C.ColumnName != "ToCollectionID" &&
                            C.ColumnName != "ToTransactionPartnerAgentURI" &&
                            C.ColumnName != "ResponsibleAgentURI" &&
                            !C.ColumnName.StartsWith("From") &&
                            !C.ColumnName.StartsWith("To"))
                        {
                            if (C.ColumnName == "BeginDate" || C.ColumnName == "AgreedEndDate" || C.ColumnName == "ActualEndDate")
                            {
                                System.DateTime d;
                                if (System.DateTime.TryParse(R[C.ColumnName].ToString(), out d))
                                    W.WriteElementString(C.ColumnName, this.getDateInEnglish(d));
                                if (C.ColumnName == "BeginDate")
                                {
                                    if (!System.DateTime.TryParse(R[C.ColumnName].ToString(), out Begin))
                                        DatePeriodOK = false;
                                }
                                if (C.ColumnName == "AgreedEndDate")
                                {
                                    if (!System.DateTime.TryParse(R[C.ColumnName].ToString(), out End))
                                        DatePeriodOK = false;
                                }
                                if (C.ColumnName == "ActualEndDate")
                                {
                                    if (!System.DateTime.TryParse(R[C.ColumnName].ToString(), out End))
                                        DatePeriodOK = false;
                                }
                            }
                            else
                                W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                        }
                    }
                    if (DatePeriodOK)
                    {
                        System.TimeSpan T = End - Begin;
                        int Months = T.Days / 30;
                        W.WriteElementString("DurationInMonths", Months.ToString());
                    }

                    // From
                    W.WriteStartElement("From");
                    foreach (System.Data.DataColumn C in R.Table.Columns)
                    {
                        if (C.ColumnName.StartsWith("From") && C.ColumnName != "FromCollectionID" && C.ColumnName != "FromTransactionPartnerAgentURI")
                        {
                            W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                        }
                        if (!R[C.ColumnName].Equals(System.DBNull.Value) &&
                           R[C.ColumnName].ToString().Length > 0 &&
                           (C.ColumnName == "FromCollectionID"))
                        {
                            int CollectionID = 0;
                            if (int.TryParse(R[C.ColumnName].ToString(), out CollectionID))
                            {
                                this.writeXmlCollection(ref W, CollectionID);
                            }

                        }
                        if (C.ColumnName == "FromTransactionPartnerAgentURI") // && !R[C.ColumnName].Equals(System.DBNull.Value))
                        {
                            W.WriteStartElement("FromTransactionPartnerAddress");
                            this.writeXmlAddress(ref W, R["FromTransactionPartnerAgentURI"].ToString());
                            W.WriteEndElement();//FromTransactionPartnerAddress
                        }
                    }
                    W.WriteEndElement();//From

                    // To
                    W.WriteStartElement("To");
                    foreach (System.Data.DataColumn C in R.Table.Columns)
                    {
                        if (C.ColumnName.StartsWith("To") && C.ColumnName != "ToCollectionID" && C.ColumnName != "ToTransactionPartnerAgentURI")
                        {
                            W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                        }
                        if (!R[C.ColumnName].Equals(System.DBNull.Value) &&
                           R[C.ColumnName].ToString().Length > 0 &&
                           (C.ColumnName == "ToCollectionID"))
                        {
                            int CollectionID = 0;
                            if (int.TryParse(R[C.ColumnName].ToString(), out CollectionID))
                            {
                                this.writeXmlCollection(ref W, CollectionID);
                            }
                        }
                        else if (R[C.ColumnName].Equals(System.DBNull.Value) &&
                           R[C.ColumnName].ToString().Length == 0 &&
                           (C.ColumnName == "ToCollectionID"))
                        {
                            int? CollID = DiversityCollection.LookupTable.TransactionCollectionID(TransactionID, false);
                            if (CollID != null)
                                this.writeXmlCollection(ref W, (int)CollID);
                        }
                        if (C.ColumnName == "ToTransactionPartnerAgentURI" && !R[C.ColumnName].Equals(System.DBNull.Value))
                        {
                            W.WriteStartElement("ToTransactionPartnerAddress");
                            this.writeXmlAddress(ref W, R["ToTransactionPartnerAgentURI"].ToString());
                            W.WriteEndElement();
                        }
                        else if (C.ColumnName == "ToTransactionPartnerAgentURI" && R[C.ColumnName].Equals(System.DBNull.Value))
                        {
                            string URI = DiversityCollection.LookupTable.TransactionAgentURI(TransactionID, false);
                            W.WriteStartElement("ToTransactionPartnerAddress");
                            this.writeXmlAddress(ref W, URI);
                            W.WriteEndElement();
                        }
                    }
                    W.WriteEndElement();//To

                    // Administration
                    W.WriteStartElement("Administration");
                    if (DiversityCollection.LookupTable.TransactionAdministratingCollectionID(TransactionID) != null)
                    {
                        int TransactionAdministrativeCollectionID = (int)DiversityCollection.LookupTable.TransactionAdministratingCollectionID(TransactionID);
                        this.writeXmlCollection(ref W, TransactionAdministrativeCollectionID);
                    }
                    W.WriteEndElement();//Administration

                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    string SQL = "";
                    int CurrentCount = 0;
                    SQL = "SELECT COUNT(*) AS Count FROM CollectionSpecimenTransaction WHERE TransactionID = " + TransactionID.ToString();
                    Microsoft.Data.SqlClient.SqlCommand COM = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    con.Open();
                    if (int.TryParse(COM.ExecuteScalar().ToString(), out CurrentCount))
                        W.WriteElementString("NumberOfSpecimenInitial", CurrentCount.ToString());

                    SQL = "SELECT COUNT(*) FROM [CollectionSpecimenTransaction] WHERE TransactionID = " + TransactionID.ToString() +
                       " AND TransactionReturnID IS NULL ";
                    //SQL = "SELECT COUNT(*) FROM [CollectionSpecimenTransaction] WHERE TransactionID = " + TransactionID.ToString() +
                    //   " AND IsOnLoan = 1 ";

                    COM.CommandText = SQL;
                    if (int.TryParse(COM.ExecuteScalar().ToString(), out this._DatasetOnLoan))
                        W.WriteElementString("NumberOfSpecimenOnLoan", this._DatasetOnLoan.ToString());

                    SQL = "SELECT COUNT(*) FROM [CollectionSpecimenTransaction] WHERE TransactionID = " + TransactionID.ToString() +
                        " AND NOT TransactionReturnID IS NULL ";
                    //SQL = "SELECT COUNT(*) FROM [CollectionSpecimenTransaction] WHERE TransactionID = " + TransactionID.ToString() +
                    //    " AND IsOnLoan = 0";

                    COM.CommandText = SQL;
                    int NumberReturned = 0;
                    if (int.TryParse(COM.ExecuteScalar().ToString(), out NumberReturned))
                        W.WriteElementString("NumberOfSpecimenReturned", NumberReturned.ToString());

                    SQL = "SELECT COUNT(*) FROM [CollectionSpecimenTransaction] WHERE TransactionID = " + TransactionID.ToString() +
                        " AND NOT TransactionReturnID IS NULL";
                    //SQL = "SELECT COUNT(*) FROM [CollectionSpecimenTransaction] WHERE TransactionID = " + TransactionID.ToString() +
                    //    " AND NOT TransactionReturnID IS NULL AND LogUpdatedWhen > '" + this._StartDate.ToShortDateString() + "'";
                    //SQL = "SELECT COUNT(*) FROM [CollectionSpecimenTransaction] WHERE TransactionID = " + TransactionID.ToString() +
                    //    " AND IsOnLoan = 0 AND LogUpdatedWhen > '" + this._StartDate.ToShortDateString() + "'";

                    COM.CommandText = SQL;
                    if (int.TryParse(COM.ExecuteScalar().ToString(), out this._DatasetReturnedSinceDate))
                        W.WriteElementString("NumberOfSpecimenReturnedNow", this._DatasetReturnedSinceDate.ToString());
                    con.Close();
                    this.writeXmlTaxonomicGroups(ref W);

                    //this.writeXmlSpecimenParts(ref W, TransactionID);
                    this.writeXmlSpecimenParts(ref W, TransactionID, false, SpecimenSorting.AccessionNumber);
                }
            }
        }

        private void writeXmlTransaction(ref System.Xml.XmlWriter W, int? NumberOfReturnedSpecimen)
        {
            //if (this._dsTransaction.Transaction.Rows.Count > 0)
            if (this._DataSet.Tables["Transaction"].Rows.Count > 0)
            {
                //int TransactionCount = 0;
                //System.Data.DataRow[] rr = this._dsTransaction.Transaction.Select("TransactionID = " + this._TransactionID.ToString());
                System.Data.DataRow[] rr = this._DataSet.Tables["Transaction"].Select("TransactionID = " + this._TransactionID.ToString());
                if (rr.Length > 0)
                {
                    W.WriteElementString("Date", this.getDateInEnglish(System.DateTime.Now));
                    System.DateTime Begin = System.DateTime.Now;
                    System.DateTime End = System.DateTime.Now;
                    bool DatePeriodOK = true;
                    System.Data.DataRow R = rr[0];
                    // Data besides From and To
                    foreach (System.Data.DataColumn C in R.Table.Columns)
                    {
                        if (!R[C.ColumnName].Equals(System.DBNull.Value) &&
                            R[C.ColumnName].ToString().Length > 0 &&
                            C.ColumnName != "TransactionID" &&
                            C.ColumnName != "PreceedingTransactionID" &&
                            C.ColumnName != "FromCollectionID" &&
                            C.ColumnName != "FromTransactionPartnerAgentURI" &&
                            C.ColumnName != "ToCollectionID" &&
                            C.ColumnName != "ToTransactionPartnerAgentURI" &&
                            C.ColumnName != "ResponsibleAgentURI" &&
                            !C.ColumnName.StartsWith("From") &&
                            !C.ColumnName.StartsWith("To"))
                        {
                            if (C.ColumnName == "BeginDate" || C.ColumnName == "AgreedEndDate" || C.ColumnName == "ActualEndDate")
                            {
                                System.DateTime d;
                                if ( System.DateTime.TryParse(R[C.ColumnName].ToString(), out d))
                                    W.WriteElementString(C.ColumnName, this.getDateInEnglish(d));
                                if (C.ColumnName == "BeginDate")
                                {
                                    if (!System.DateTime.TryParse(R[C.ColumnName].ToString(), out Begin)) 
                                        DatePeriodOK = false;
                                }
                                if (C.ColumnName == "AgreedEndDate")
                                {
                                    if (!System.DateTime.TryParse(R[C.ColumnName].ToString(), out End))
                                        DatePeriodOK = false;
                                }
                                if (C.ColumnName == "ActualEndDate")
                                {
                                    if (!System.DateTime.TryParse(R[C.ColumnName].ToString(), out End))
                                        DatePeriodOK = false;
                                }
                            }
                            else
                            W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                        }
                    }
                    if (DatePeriodOK)
                    {
                        System.TimeSpan T = End - Begin;
                        int Months = T.Days / 30;
                        W.WriteElementString("DurationInMonths", Months.ToString());
                    }

                    // From
                    W.WriteStartElement("From");
                    foreach (System.Data.DataColumn C in R.Table.Columns)
                    {
                        if (C.ColumnName.StartsWith("From") && C.ColumnName != "FromCollectionID" && C.ColumnName != "FromTransactionPartnerAgentURI")
                        {
                            W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                        }
                        if (!R[C.ColumnName].Equals(System.DBNull.Value) &&
                           R[C.ColumnName].ToString().Length > 0 &&
                           (C.ColumnName == "FromCollectionID"))
                        {
                            int CollectionID = 0;
                            if (int.TryParse(R[C.ColumnName].ToString(), out CollectionID))
                            {
                                this.writeXmlCollection(ref W, CollectionID);
                            }

                        }
                        if (C.ColumnName == "FromTransactionPartnerAgentURI") // && !R[C.ColumnName].Equals(System.DBNull.Value))
                        {
                            W.WriteStartElement("FromTransactionPartnerAddress");
                            this.writeXmlAddress(ref W, R["FromTransactionPartnerAgentURI"].ToString());
                            W.WriteEndElement();//FromTransactionPartnerAddress
                        }
                    }
                    W.WriteEndElement();//From

                    // To
                    W.WriteStartElement("To");
                    foreach (System.Data.DataColumn C in R.Table.Columns)
                    {
                        if (C.ColumnName.StartsWith("To") && C.ColumnName != "ToCollectionID" && C.ColumnName != "ToTransactionPartnerAgentURI")
                        {
                            W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                        }
                        if (!R[C.ColumnName].Equals(System.DBNull.Value) &&
                           R[C.ColumnName].ToString().Length > 0 &&
                           (C.ColumnName == "ToCollectionID"))
                        {
                            int CollectionID = 0;
                            if (int.TryParse(R[C.ColumnName].ToString(), out CollectionID))
                            {
                                this.writeXmlCollection(ref W, CollectionID);
                            }
                        }
                        else if (R[C.ColumnName].Equals(System.DBNull.Value) &&
                           R[C.ColumnName].ToString().Length == 0 &&
                           (C.ColumnName == "ToCollectionID"))
                        {
                            int? CollID = DiversityCollection.LookupTable.TransactionCollectionID(this._TransactionID, false);
                            if (CollID != null)
                                this.writeXmlCollection(ref W, (int)CollID);
                        }
                        if (C.ColumnName == "ToTransactionPartnerAgentURI" && !R[C.ColumnName].Equals(System.DBNull.Value))
                        {
                            W.WriteStartElement("ToTransactionPartnerAddress");
                            this.writeXmlAddress(ref W, R["ToTransactionPartnerAgentURI"].ToString());
                            W.WriteEndElement();//ToTransactionPartnerAddress
                        }
                        else if (C.ColumnName == "ToTransactionPartnerAgentURI" && R[C.ColumnName].Equals(System.DBNull.Value))
                        {
                            string URI = DiversityCollection.LookupTable.TransactionAgentURI(this._TransactionID, false);
                            W.WriteStartElement("ToTransactionPartnerAddress");
                            this.writeXmlAddress(ref W, URI);
                            W.WriteEndElement();//ToTransactionPartnerAddress
                        }
                    }
                    W.WriteEndElement();//To

                    // Administration
                    W.WriteStartElement("Administration");
                    if (DiversityCollection.LookupTable.TransactionAdministratingCollectionID(this._TransactionID) != null)
                    {
                        int TransactionAdministrativeCollectionID = (int)DiversityCollection.LookupTable.TransactionAdministratingCollectionID(this._TransactionID);
                        this.writeXmlCollection(ref W, TransactionAdministrativeCollectionID);
                    }
                    W.WriteEndElement();//Administration

                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    string SQL = "";
                    int CurrentCount = 0;
                    SQL = "SELECT COUNT(*) AS Count FROM CollectionSpecimenTransaction WHERE TransactionID = " + this._TransactionID.ToString();
                    Microsoft.Data.SqlClient.SqlCommand COM = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    con.Open();
                    if (int.TryParse(COM.ExecuteScalar().ToString(), out CurrentCount))
                        W.WriteElementString("NumberOfSpecimenInitial", CurrentCount.ToString());

                    SQL = "SELECT COUNT(*) FROM [CollectionSpecimenTransaction] WHERE TransactionID = " + this._TransactionID.ToString() +
                       " AND IsOnLoan = 1 ";
                    COM.CommandText = SQL;
                    if (int.TryParse(COM.ExecuteScalar().ToString(), out this._DatasetOnLoan))
                        W.WriteElementString("NumberOfSpecimenOnLoan", this._DatasetOnLoan.ToString());

                    SQL = "SELECT COUNT(*) FROM [CollectionSpecimenTransaction] WHERE TransactionID = " + this._TransactionID.ToString() +
                        " AND IsOnLoan = 0";
                    COM.CommandText = SQL;
                    int NumberReturned = 0;
                    if (int.TryParse(COM.ExecuteScalar().ToString(), out NumberReturned))
                        W.WriteElementString("NumberOfSpecimenReturned", NumberReturned.ToString());

                    //SQL = "SELECT COUNT(*) FROM [CollectionSpecimenTransaction] WHERE TransactionID = " + this._TransactionID.ToString() +
                    //    " AND IsOnLoan = 0 AND LogUpdatedWhen > '" + this._StartDate.ToShortDateString() + "'";
                    SQL = "SELECT COUNT(*) FROM [CollectionSpecimenTransaction] WHERE TransactionID = " + this._TransactionID.ToString();
                    COM.CommandText = SQL;
                    if (int.TryParse(COM.ExecuteScalar().ToString(), out this._DatasetReturnedSinceDate))
                        W.WriteElementString("NumberOfSpecimenReturnedNow", this._DatasetReturnedSinceDate.ToString());
                    con.Close();
                    
                    this.writeXmlTaxonomicGroups(ref W);
                    //this.writeXmlSpecimenParts(ref W);
                    this.writeXmlSpecimenParts(ref W, null, false, SpecimenSorting.AccessionNumber);
                }
            }
        }

        private string getDateInEnglish(System.DateTime Date)
        {
            string D = Date.Day.ToString() + ". ";
            int Month = Date.Month;
            switch (Month)
            {
                case 1:
                    D += "Jan.";
                    break;
                case 2:
                    D += "Feb.";
                    break;
                case 3:
                    D += "Mar.";
                    break;
                case 4:
                    D += "Apr.";
                    break;
                case 5:
                    D += "May";
                    break;
                case 6:
                    D += "Jun.";
                    break;
                case 7:
                    D += "Jul.";
                    break;
                case 8:
                    D += "Aug.";
                    break;
                case 9:
                    D += "Sep.";
                    break;
                case 10:
                    D += "Oct.";
                    break;
                case 11:
                    D += "Nov.";
                    break;
                case 12:
                    D += "Dec.";
                    break;
            }
            D += " " + Date.Year;
            return D;
        }

        private string AddressURI(int? ParentTransactionID, string Direction)
        {
            string URI = "";
            if (this._TransactionCorrespondenceType == TransactionCorrespondenceType.Confirmation && 
                this._TransactionType == TransactionType.Exchange)
            {
                System.Data.DataRow[] rr = this._DataSet.Tables["Transaction"].Select("TransactionID = " + this.ID.ToString());
                System.Data.DataRow r = rr[0];
                if (Direction == "From")
                    return this._AdminAgentURI;
                else
                {
                    if (r["ToTransactionPartnerAgentURI"].ToString() == this._AdminAgentURI)
                        return r["FromTransactionPartnerAgentURI"].ToString();
                    else return _AdminAgentURI;
                }
            }
            if (ParentTransactionID == null)
            {
                if (this._DataSet.Tables["Transaction"].Rows.Count > 0)
                {
                    System.Data.DataRow[] rr = this._DataSet.Tables["Transaction"].Select("TransactionID = " + this.ID.ToString());
                    if (rr.Length > 0)
                    {
                        System.Data.DataRow R = rr[0];
                        if (Direction.ToUpper() == "FROM" && !R["FromTransactionPartnerAgentURI"].Equals(System.DBNull.Value))
                            return R["FromTransactionPartnerAgentURI"].ToString();
                        if (Direction.ToUpper() == "TO" && !R["ToTransactionPartnerAgentURI"].Equals(System.DBNull.Value))
                            return R["ToTransactionPartnerAgentURI"].ToString();
                        if (!R["ParentTransactionID"].Equals(System.DBNull.Value))
                        {
                            URI = this.AddressURI(int.Parse(R["ParentTransactionID"].ToString()), Direction);
                            if (URI.Length > 0) 
                                return URI;
                        }
                    }
                }
            }
            else
            {
                if (this._DataSet.Tables["Transaction"].Rows.Count > 0)
                {
                    System.Data.DataRow[] rr = this._DataSet.Tables["Transaction"].Select("TransactionID = " + ParentTransactionID.ToString());
                    if (rr.Length > 0)
                    {
                        System.Data.DataRow R = rr[0];
                        if (Direction.ToUpper() == "FROM" && !R["FromTransactionPartnerAgentURI"].Equals(System.DBNull.Value))
                            return R["FromTransactionPartnerAgentURI"].ToString();
                        if (Direction.ToUpper() == "TO" && !R["ToTransactionPartnerAgentURI"].Equals(System.DBNull.Value))
                            return R["ToTransactionPartnerAgentURI"].ToString();
                        if (!R["ParentTransactionID"].Equals(System.DBNull.Value))
                        {
                            URI = this.AddressURI(int.Parse(R["ParentTransactionID"].ToString()), Direction);
                            if (URI.Length > 0)
                                return URI;
                        }
                    }
                }
            }
            System.Data.DataRow[] RR = this._DataSet.Tables["Transaction"].Select("TransactionID = " + this.ID.ToString());
            if (RR.Length > 0)
            {
                System.Data.DataRow R = RR[0];
                if (R["TransactionType"].ToString().ToLower() == "return")
                {
                    if (Direction.ToUpper() == "FROM" && !R["FromCollectionID"].Equals(System.DBNull.Value))
                        URI = DiversityCollection.LookupTable.CollectionAdministrativeContactAgentURI(int.Parse(R["FromCollectionID"].ToString()));
                    if (Direction.ToUpper() == "TO" && !R["ToCollectionID"].Equals(System.DBNull.Value))
                        URI = DiversityCollection.LookupTable.CollectionAdministrativeContactAgentURI(int.Parse(R["ToCollectionID"].ToString()));
                    if (URI.Length == 0 && ParentTransactionID != null)
                    {
                        System.Data.DataRow[] rrP = this._DataSet.Tables["Transaction"].Select("TransactionID = " + ParentTransactionID.ToString());
                        if (rrP.Length > 0)
                        {
                            System.Data.DataRow rP = rrP[0];
                            if (Direction.ToUpper() == "FROM" && !rP["FromCollectionID"].Equals(System.DBNull.Value))
                                URI = DiversityCollection.LookupTable.CollectionAdministrativeContactAgentURI(int.Parse(rP["FromCollectionID"].ToString()));
                            if (Direction.ToUpper() == "TO" && !rP["ToCollectionID"].Equals(System.DBNull.Value))
                                URI = DiversityCollection.LookupTable.CollectionAdministrativeContactAgentURI(int.Parse(rP["ToCollectionID"].ToString()));
                        }
                    }
                }
                else
                {
                    if (Direction.ToUpper() == "FROM" && !R["FromCollectionID"].Equals(System.DBNull.Value))
                        return DiversityCollection.LookupTable.CollectionAdministrativeContactAgentURI(int.Parse(R["FromCollectionID"].ToString()));
                    if (Direction.ToUpper() == "TO" && !R["ToCollectionID"].Equals(System.DBNull.Value))
                        return DiversityCollection.LookupTable.CollectionAdministrativeContactAgentURI(int.Parse(R["ToCollectionID"].ToString()));
                }
            }
            return URI;
        }

        private string AddressText(string Direction)
        {
            string Address = "";
            if (this._DataSet.Tables["Transaction"].Rows.Count > 0)
            {
                System.Data.DataRow[] rr = this._DataSet.Tables["Transaction"].Select("TransactionID = " + this.ID.ToString());
                if (rr.Length > 0)
                {
                    System.Data.DataRow R = rr[0];
                    if (Direction.ToUpper() == "FROM" && !R["FromTransactionPartnerName"].Equals(System.DBNull.Value))
                        return R["FromTransactionPartnerName"].ToString();
                    if (Direction.ToUpper() == "TO" && !R["ToTransactionPartnerName"].Equals(System.DBNull.Value))
                        return R["ToTransactionPartnerName"].ToString();
                }
            }
            return Address;
        }

        private void writeXmlAddress(ref System.Xml.XmlWriter W, string AgentURI)
        {
            System.Collections.Generic.Dictionary<string, string> DictAddress = this.Agent.UnitValues(AgentURI);
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DictAddress)
            {
                try
                {
                    if (KV.Key == "ParentName" ||
                        KV.Key == "Country" ||
                        KV.Key == "City" ||
                        KV.Key == "Abbreviation" ||
                        KV.Key == "PostalCode" ||
                        KV.Key == "Streetaddress" ||
                        KV.Key == "CellularPhone" ||
                        KV.Key == "Telefax" ||
                        KV.Key == "Telephone" ||
                        KV.Key == "Email" ||
                        KV.Key == "URI" ||
                        KV.Key == "Address" ||
                        KV.Key == "PersonName" ||
                        KV.Key == "AgentName" ||
                        KV.Key == "SuperiorAgents"
                         || KV.Key == "Parent Abbreviation"
                        )
                    {
                        if (KV.Key == "Country")
                            W.WriteElementString(KV.Key, KV.Value.ToUpper());
                        else if (KV.Key == "SuperiorAgents")
                        {
                            W.WriteStartElement("SuperiorAgents");
                            string SA = KV.Value;
                            int i = 0;
                            while (i > -1 && SA.Length > 0)
                            {
                                string S = "";
                                if (i == 0)
                                    S = SA.Substring(i, SA.IndexOf("\r\n", i) - i);
                                else
                                {
                                    int e = SA.IndexOf("\r\n", i + 1);
                                    int l = e - i;
                                    if (l > 0)
                                        S = SA.Substring(i + 2, l - 2);
                                }
                                if (S.Length > 0)
                                {
                                    W.WriteStartElement("SuperiorAgent");
                                    W.WriteElementString("AgentName", S);
                                    W.WriteEndElement();//SuperiorAgent
                                }
                                i = SA.IndexOf("\r\n", i + 1);
                            }
                            W.WriteEndElement();//SuperiorAgents
                        }
                        //else if (KV.Key == "Parent Abbreviation")
                        //{
                        //    if (KV.Value.Length > 0)
                        //    {
                        //        W.WriteElementString(KV.Key.Replace(" ", "_"), KV.Value);
                        //    }
                        //}
                        else
                        {
                            if (KV.Value.Length > 0)
                            {
                                W.WriteElementString(KV.Key.Replace(" ", "_"), KV.Value);
                            }
                        }
                    }
                    else
                    {

                    }
                }
                catch { }
            }
        }

        private void writeXmlTaxonomicGroups(ref System.Xml.XmlWriter W)
        {
            W.WriteStartElement("TaxonomicGroups");
            foreach (System.Data.DataRow R in this._DataSet.Tables["TaxonomicGroups"].Rows)
            {
                W.WriteStartElement("TaxonomicGroup");
                W.WriteElementString("NumberOfSpecimen", R["NumberOfSpecimen"].ToString());
                W.WriteElementString("Group", R["TaxonomicGroup"].ToString());
                W.WriteEndElement();//TaxonomicGroup
            }
            W.WriteEndElement();//TaxonomicGroups
        }

        private void writeXmlCollection(ref System.Xml.XmlWriter W, int CollectionID)
        {
            //string CollectionOwner = DiversityCollection.LookupTable.CollectionOwner(CollectionID);
            string URI = DiversityCollection.LookupTable.CollectionAdministrativeContactAgentURI(CollectionID);
            W.WriteStartElement("CollectionAdministrativeContact");
            W.WriteStartElement("Address");
            this.writeXmlAddress(ref W, URI);
            W.WriteEndElement(); //Address
            W.WriteEndElement();//CollectionAdministrativeContact
            W.WriteStartElement("CollectionOwner");
            string Name = DiversityCollection.LookupTable.CollectionOwner(CollectionID);
            if (Name.Length > 0)
                W.WriteElementString("Name", Name);
            string Acronym = DiversityCollection.LookupTable.CollectionOwnerAcronym(CollectionID);
            if (Acronym.Length > 0)
                W.WriteElementString("Acronym", Acronym);
            W.WriteElementString("CollectionName", DiversityCollection.LookupTable.CollectionName(CollectionID));
            //W.WriteStartElement("Address");
            URI = DiversityCollection.LookupTable.CollectionOwnerAgentURI(CollectionID);
            if (URI.Length > 0)
            {
                W.WriteStartElement("Address");
                this.writeXmlAddress(ref W, URI);
                W.WriteEndElement();//Address
            }
            W.WriteEndElement();//CollectionOwner
        }

        public enum SpecimenSorting { Identification, AccessionNumber }

        private void writeXmlSpecimenParts(ref System.Xml.XmlWriter W, int? TransactionID, bool EveryLine, SpecimenSorting Sorter)
        {
            try
            {

                if (Sorter == SpecimenSorting.Identification)
                {
                    this.writeXmlSpecimenPartsByTaxon(ref W, TransactionID);
                }
                else
                {
                    string Material = this.DataSet.Tables["Transaction"].Rows[0]["MaterialCategory"].ToString();
                    System.Collections.Generic.List<string> AccNrList = new List<string>();
                    if (TransactionID == null)
                        TransactionID = this._TransactionID;
                    string OrderColumns = "MaterialCategory, LastIdentificationCache, AccessionNumber";
                    if (Sorter == SpecimenSorting.AccessionNumber)
                        OrderColumns = "MaterialCategory, AccessionNumber, LastIdentificationCache";
                    System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.SortedDictionary<string, SpecimenPart>> MaterialCategoryDict = new SortedDictionary<string, SortedDictionary<string, SpecimenPart>>();
                    System.Data.DataRow[] rr;
                    if (this._TransactionCorrespondenceType == TransactionCorrespondenceType.Forwarding && (int)TransactionID == this.ID)
                        rr = this._DataSet.Tables["CollectionSpecimenPartForwardingList"].Select("TransactionID = " + TransactionID.ToString(), OrderColumns);
                    else if (this._TransactionCorrespondenceType == TransactionCorrespondenceType.Return && (int)TransactionID == this.ID)
                        rr = this._DataSet.Tables["CollectionSpecimenPartTransactionReturnList"].Select("TransactionID = " + TransactionID.ToString(), OrderColumns);
                    else
                        rr = this._DataSet.Tables["CollectionSpecimenPartList"].Select("TransactionID = " + TransactionID.ToString(), OrderColumns);

                    for (int i = 0; i < rr.Length; i++)
                    {
                        if (this.GroupByMaterial)
                            Material = rr[i]["MaterialCategory"].ToString();
                        string Sort = rr[i]["AccessionNumber"].ToString();
                        SpecimenPart SP = new SpecimenPart();
                        SP.AccessionNumber = rr[i]["AccessionNumber"].ToString();
                        SP.AccessionNumberTo = "";
                        SP.Identification = rr[i]["LastIdentificationCache"].ToString();
                        SP.LocalityDescription = rr[i]["LocalityDescription"].ToString();
                        // #127
                        int PartID;
                        if (int.TryParse(rr[i]["SpecimenPartID"].ToString(), out PartID))
                        {
                            SP.SpecimenPartID = PartID;
                        }
                        else
                        {
                            SP.SpecimenPartID = 0;
                        }
                        string Collector = "";
                        string CollectorsNumber = "";
                        int ID;
                        if (int.TryParse(rr[i]["CollectionSpecimenID"].ToString(), out ID))
                        {
                            this.getCollectorAndNumber(ID, ref Collector, ref CollectorsNumber);
                            SP.CollectorsName = Collector;
                            SP.CollectorsNumber = CollectorsNumber;
                        }
                        SP.TypeStatus = "";
                        if (rr[i]["TypeOf"].Equals(System.DBNull.Value) || rr[i]["TypeOf"].ToString().ToLower() == "not a type")
                        {
                            string SQL = "SELECT TOP (1) TaxonomicName, TypeStatus " +
                                "FROM  Identification AS I " +
                                "WHERE (CollectionSpecimenID = " + rr[i]["CollectionSpecimenID"].ToString() + ") AND (TypeStatus <> '') AND (TypeStatus <> 'not a type')";
                            System.Data.DataTable dt = new System.Data.DataTable();
                            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                            ad.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["TaxonomicName"].ToString() != SP.Identification)
                                    SP.TypeIdentification = dt.Rows[0]["TaxonomicName"].ToString();
                                SP.TypeStatus = dt.Rows[0]["TypeStatus"].ToString();
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                            SP.TypeStatus = rr[i]["TypeOf"].ToString();
                        }
                        if (rr[i]["TransactionAccessionNumber"].Equals(System.DBNull.Value))
                            SP.TransactionAccessionNumber = "";
                        else
                            SP.TransactionAccessionNumber = rr[i]["TransactionAccessionNumber"].ToString();
                        SP.TransactionAccessionNumberTo = "";
                        if (!MaterialCategoryDict.ContainsKey(Material))//rr[i]["MaterialCategory"].ToString()))
                        {
                            System.Collections.Generic.SortedDictionary<string, SpecimenPart> SpecimenPartDict = new SortedDictionary<string, SpecimenPart>();
                            if (!SpecimenPartDict.ContainsKey(Sort))
                                SpecimenPartDict.Add(Sort, SP);
                            else
                            {
                            }
                            if (!MaterialCategoryDict.ContainsKey(Material))//rr[i]["MaterialCategory"].ToString()))
                                MaterialCategoryDict.Add(Material/*rr[i]["MaterialCategory"].ToString()*/, SpecimenPartDict);
                            else
                            { }
                        }
                        else
                        {
                            if (EveryLine)
                            {
                                if (!MaterialCategoryDict[Material/*rr[i]["MaterialCategory"].ToString()*/].ContainsKey(Sort))
                                    MaterialCategoryDict[Material/*rr[i]["MaterialCategory"].ToString()*/].Add(Sort, SP);
                                else if (Sorter == SpecimenSorting.Identification)
                                {
                                    MaterialCategoryDict[Material/*rr[i]["MaterialCategory"].ToString()*/].Add(Sort + " [" + SP.AccessionNumber + "]", SP);
                                }
                            }
                            else
                            {
                                if (MaterialCategoryDict[Material/*rr[i]["MaterialCategory"].ToString()*/].ContainsKey(Sort))
                                {
                                }
                                else
                                {
                                    System.Collections.Generic.SortedDictionary<string, SpecimenPart> SpecimenPartDict = new SortedDictionary<string, SpecimenPart>();
                                    SpecimenPartDict.Add(Sort, SP);
                                    MaterialCategoryDict[Material/*rr[i]["MaterialCategory"].ToString()*/].Add(Sort, SP);
                                }
                            }
                        }
                    }

                    W.WriteStartElement("SpecimenParts");
                    W.WriteStartElement("MaterialCategories");
                    int Counter = 1;
                    foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.SortedDictionary<string, SpecimenPart>> KV in MaterialCategoryDict)
                    {
                        W.WriteStartElement("MaterialCategory");
                        W.WriteElementString("Material", KV.Key);
                        W.WriteElementString("Counter", Counter.ToString());
                        W.WriteStartElement("Specimens");
                        foreach (System.Collections.Generic.KeyValuePair<string, SpecimenPart> kv in KV.Value)
                        {
                            W.WriteStartElement("Specimen");
                            if (EveryLine)
                            {
                                W.WriteElementString("Counter", Counter.ToString());
                                Counter++;
                                W.WriteElementString("AccessionNumber", kv.Value.AccessionNumber);
                                W.WriteElementString("Collector", kv.Value.CollectorsName);
                                W.WriteElementString("CollectorsNumber", kv.Value.CollectorsNumber);
                            }
                            else
                            {
                                string AN = kv.Value.AccessionNumber;
                                if (kv.Value.AccessionNumberTo.Length > 0)
                                    AN += " - " + kv.Value.AccessionNumberTo;
                                W.WriteElementString("AccessionNumber", AN);
                                W.WriteElementString("Counter", Counter.ToString());
                                Counter++;
                            }
                            W.WriteElementString("Identification", kv.Value.TaxonSinYear);//.Identification);
                            // #127
                            if (this._AllUnits)
                            {
                                this.writeXmlAllUnits(ref W, kv.Value);
                            }
                            W.WriteElementString("LocalityDescription", kv.Value.LocalityDescription);

                            string TAN = kv.Value.TransactionAccessionNumber;
                            if (kv.Value.TransactionAccessionNumberTo.Length > 0)
                                TAN += " - " + kv.Value.TransactionAccessionNumberTo;
                            if (TAN.Length > 0)
                                W.WriteElementString("TransactionAccessionNumber", TAN);

                            if (kv.Value.TypeStatus.Length > 0 && this._IncludeTypeInformation)
                                W.WriteElementString("TypeStatus", kv.Value.TypeStatus);
                            if (kv.Value.TypeIdentification != null && kv.Value.TypeIdentification.Length > 0 && this._IncludeTypeInformation)
                            {
                                W.WriteElementString("TypeIdentification", kv.Value.TypeIdentification);
                                Counter++;
                                string IdentSinType = kv.Value.TaxonSinYear;//.Identification;// "";// KVtax.Key.Substring(0, KVtax.Key.IndexOf(", " + Status));
                                W.WriteElementString("IdentificationSinType", IdentSinType);

                            }
                            W.WriteEndElement(); // Specimen
                        }
                        W.WriteEndElement(); // Specimens
                        Counter++;
                        W.WriteEndElement(); // MaterialCategory
                    }
                    W.WriteEndElement(); // MaterialCategories
                    W.WriteEndElement(); // SpecimenParts
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        /// <summary>
        /// writing the XML for all units of a specimen part
        /// #127
        /// </summary>
        /// <param name="W"></param>
        /// <param name="specimenPart"></param>
        private void writeXmlAllUnits(ref System.Xml.XmlWriter W, SpecimenPart specimenPart)
        {
            int TransactionID = this._TransactionID;
            System.Data.DataRow[] rr = this._DataSet.Tables["CollectionSpecimenPartList"].Select("TransactionID = " + TransactionID.ToString() + " AND SpecimenPartID = " + specimenPart.SpecimenPartID.ToString(), "AccessionNumber");
            W.WriteStartElement("AllUnits");
            string SQL = "SELECT U.LastIdentificationCache, U.TaxonomicGroup FROM IdentificationUnit U " +
                //"INNER JOIN Identification I ON U.IdentificationUnitID = I.IdentificationUnitID AND (U.LastIdentificationCache = I.TaxonomicName OR U.LastIdentificationCache = I.VernacularTerm) " +
                "INNER JOIN IdentificationUnitInPart P ON U.IdentificationUnitID = P.IdentificationUnitID AND P.DisplayOrder > 0 AND P.SpecimenPartID = " + specimenPart.SpecimenPartID.ToString();
            System.Data.DataTable dt = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            foreach (System.Data.DataRow R in dt.Rows)
            {
                W.WriteStartElement("Unit");
                W.WriteElementString("Identification", R["LastIdentificationCache"].ToString());
                W.WriteElementString("TaxonomicGroup", R["TaxonomicGroup"].ToString());
                W.WriteEndElement(); // Unit
            }
            W.WriteEndElement(); // AllUnits
        }

        /// <summary>
        /// writing the list grouped by the taxa
        /// </summary>
        /// <param name="W">The writer</param>
        /// <param name="TransactionID">The ID of the transaction</param>
        private void writeXmlSpecimenPartsByTaxon(ref System.Xml.XmlWriter W, int? TransactionID)
        {
            try
            {
                System.Collections.Generic.List<string> AccNrList = new List<string>();
                if (TransactionID == null)
                    TransactionID = this._TransactionID;
                string OrderColumns = "MaterialCategory, LastIdentificationCache, AccessionNumber";
                if (!this.GroupByMaterial)
                    OrderColumns = "LastIdentificationCache, AccessionNumber";
                System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.SortedDictionary<string, SpecimenPart>>> MaterialTaxonAccessionDict = new SortedDictionary<string, SortedDictionary<string, SortedDictionary<string, SpecimenPart>>>();
                System.Data.DataRow[] rr;
                if (this._TransactionCorrespondenceType == TransactionCorrespondenceType.Forwarding && (int)TransactionID == this.ID)
                    rr = this._DataSet.Tables["CollectionSpecimenPartForwardingList"].Select("TransactionID = " + TransactionID.ToString(), OrderColumns);
                else if (this._TransactionCorrespondenceType == TransactionCorrespondenceType.Return && (int)TransactionID == this.ID)
                    rr = this._DataSet.Tables["CollectionSpecimenPartTransactionReturnList"].Select("TransactionID = " + TransactionID.ToString(), OrderColumns);
                else
                    rr = this._DataSet.Tables["CollectionSpecimenPartList"].Select("TransactionID = " + TransactionID.ToString(), OrderColumns);

                // Collecting all AccNrs for the taxa
                string Material = this.DataSet.Tables["Transaction"].Rows[0]["MaterialCategory"].ToString();
                for (int i = 0; i < rr.Length; i++)
                {
                    if (this.GroupByMaterial)
                        Material = rr[i]["MaterialCategory"].ToString();
                    string Taxon = rr[i]["LastIdentificationCache"].ToString();
                    string AccNr = rr[i]["AccessionNumber"].ToString();
                    SpecimenPart SP = new SpecimenPart();
                    SP.TypeStatus = "";
                    if (this._IncludeTypeInformation)
                    {
                        // last identification missing type information - try to find type information in older identifications
                        if (rr[i]["TypeOf"].Equals(System.DBNull.Value) || rr[i]["TypeOf"].ToString().ToLower() == "not a type")
                        {
                            string SQL = "SELECT TOP (1) TaxonomicName, TypeStatus " +
                                "FROM  Identification AS I " +
                                "WHERE (CollectionSpecimenID = " + rr[i]["CollectionSpecimenID"].ToString() + ") AND (TypeStatus <> '') AND (TypeStatus <> 'not a type')";
                            System.Data.DataTable dt = new System.Data.DataTable();
                            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                            ad.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["TaxonomicName"].ToString() != SP.Identification)
                                    SP.TypeIdentification = dt.Rows[0]["TaxonomicName"].ToString();
                                SP.TypeStatus = dt.Rows[0]["TypeStatus"].ToString();
                            }
                        }
                        else // type information linked to last identification
                        {
                            SP.TypeStatus = rr[i]["TypeOf"].ToString();
                            SP.TypeIdentification = rr[i]["LastIdentificationCache"].ToString();
                        }
                        // writing identification if different to type identification
                        //if (!rr[i]["LastIdentificationCache"].Equals(System.DBNull.Value) &&
                        //    SP.TypeIdentification != null &&
                        //    rr[i]["LastIdentificationCache"].ToString() != SP.TypeIdentification)
                        //    SP.Identification = rr[i]["LastIdentificationCache"].ToString();
                        if (SP.TypeStatus.Length > 0)
                            Taxon += ", " + SP.TypeStatus;
                    }
                    if (!MaterialTaxonAccessionDict.ContainsKey(Material))
                    {
                        System.Collections.Generic.SortedDictionary<string, SpecimenPart> AccNrDict = new SortedDictionary<string,SpecimenPart>();
                        AccNrDict.Add(AccNr, SP);
                        System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.SortedDictionary<string, SpecimenPart>> TaxonDict = new SortedDictionary<string,SortedDictionary<string,SpecimenPart>>();
                        TaxonDict.Add(Taxon, AccNrDict);
                        MaterialTaxonAccessionDict.Add(Material, TaxonDict);
                    }
                    else
                    {
                        if (!MaterialTaxonAccessionDict[Material].ContainsKey(Taxon))
                        {
                            System.Collections.Generic.SortedDictionary<string, SpecimenPart> AccNrDict = new SortedDictionary<string, SpecimenPart>();
                            AccNrDict.Add(AccNr, SP);
                            System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.SortedDictionary<string, SpecimenPart>> TaxonDict = new SortedDictionary<string, SortedDictionary<string, SpecimenPart>>();
                            MaterialTaxonAccessionDict[Material].Add(Taxon, AccNrDict);
                        }
                        else
                        {
                            MaterialTaxonAccessionDict[Material][Taxon].Add(AccNr, SP);
                        }
                    }
                    string Collector = "";
                    string CollectorsNumber = "";
                    int ID;
                    if (int.TryParse(rr[i]["CollectionSpecimenID"].ToString(), out ID))
                    {
                        this.getCollectorAndNumber(ID, ref Collector, ref CollectorsNumber);
                        SP.CollectorsName = Collector;
                        SP.CollectorsNumber = CollectorsNumber;
                    }
                }
                W.WriteStartElement("SpecimenParts");
                W.WriteStartElement("MaterialCategories");
                int Counter = 1;
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.SortedDictionary<string, SpecimenPart>>> KVmat in MaterialTaxonAccessionDict)
                {
                    W.WriteStartElement("MaterialCategory");
                    if (this._GroupByMaterial)
                        W.WriteElementString("Material", KVmat.Key);
                    W.WriteStartElement("Specimens");
                    foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.SortedDictionary<string, SpecimenPart>> KVtax in KVmat.Value)
                    {
                        if (this._SpecimenOrder == TransactionSpecimenOrder.TaxaSingleAccessionNumber)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.SpecimenPart> KVPart in KVtax.Value)
                            {
                                W.WriteStartElement("Specimen");
                                W.WriteElementString("Identification", KVtax.Key);
                                W.WriteElementString("AccessionNumber", KVPart.Key);
                                if (KVPart.Value.CollectorsName.Length > 0)
                                {
                                    W.WriteElementString("Collector", KVPart.Value.CollectorsName);
                                    if (KVPart.Value.CollectorsNumber.Length > 0)
                                        W.WriteElementString("CollectorsNumber", KVPart.Value.CollectorsNumber);
                                }
                                W.WriteElementString("Counter", Counter.ToString());
                                string Status = "";
                                string Ident = "";
                                    if (KVPart.Value.TypeStatus != null && KVPart.Value.TypeStatus.Length > 0)
                                        Status = KVPart.Value.TypeStatus;
                                    if (KVPart.Value.TypeIdentification != null && KVPart.Value.TypeIdentification.Length > 0)
                                        Ident = KVPart.Value.TypeIdentification;
                                if (Status.Length > 0 && Ident.Length > 0)
                                {
                                    W.WriteElementString("TypeIdentification", Ident);
                                    W.WriteElementString("TypeStatus", Status);
                                    string IdentSinType = KVtax.Key.Substring(0, KVtax.Key.IndexOf(", " + Status));
                                    W.WriteElementString("IdentificationSinType", IdentSinType);
                                }
                                W.WriteEndElement(); // Specimen
                                Counter++;
                            }
                        }
                        else
                        {
                            string Status = "";
                            string Ident = "";
                            bool TypeIsDifferent = false;
                            string Type = "";
                            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.SpecimenPart> KVPart in KVtax.Value)
                            {
                                if (KVPart.Value.TypeStatus != null && KVPart.Value.TypeStatus.Length > 0)
                                    Status = KVPart.Value.TypeStatus;
                                if (KVPart.Value.TypeIdentification != null && KVPart.Value.TypeIdentification.Length > 0)
                                    Ident = KVPart.Value.TypeIdentification;
                                if (Ident != Type && Type.Length > 0)
                                    TypeIsDifferent = true;
                                else Type = Ident;
                            }
                            if (TypeIsDifferent)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.SpecimenPart> KVPart in KVtax.Value)
                                {
                                    W.WriteStartElement("Specimen");
                                    W.WriteElementString("AccessionNumber", KVPart.Key);
                                    W.WriteElementString("Identification", KVtax.Key);
                                    W.WriteElementString("Number", KVtax.Value.Count.ToString());
                                    W.WriteElementString("Counter", Counter.ToString());
                                    W.WriteElementString("TypeIdentification", KVPart.Value.TypeIdentification);
                                    W.WriteElementString("TypeStatus", KVPart.Value.TypeStatus);
                                    string IdentSinType = KVtax.Key.Substring(0, KVtax.Key.IndexOf(", " + Status));
                                    W.WriteElementString("IdentificationSinType", IdentSinType);
                                    W.WriteEndElement(); // Specimen
                                    Counter++;
                                }
                            }
                            else
                            {
                                W.WriteStartElement("Specimen");
                                string AccNrRange = this.AccessionNumberStringForIdentification(KVtax.Value);
                                W.WriteElementString("AccessionNumber", AccNrRange);
                                W.WriteElementString("Identification", KVtax.Key);
                                W.WriteElementString("Number", KVtax.Value.Count.ToString());
                                W.WriteElementString("Counter", Counter.ToString());
                                if (Status.Length > 0 && Ident.Length > 0)
                                {
                                        // TODO: may interfere with other reports
                                        W.WriteElementString("TypeIdentification", Ident);
                                        W.WriteElementString("TypeStatus", Status);
                                        string IdentSinType = KVtax.Key.Substring(0, KVtax.Key.IndexOf(", " + Status));
                                        W.WriteElementString("IdentificationSinType", IdentSinType);
                                }
                                W.WriteEndElement(); // Specimen
                                Counter++;
                            }
                        }
                    }
                    W.WriteEndElement(); // Specimens
                    W.WriteEndElement(); // MaterialCategory
                }
                W.WriteEndElement(); // MaterialCategories
                W.WriteEndElement(); // SpecimenParts
            }
            catch (System.Exception ex)
            {
            }
        }

        private void getCollectorAndNumber(int CollectionSpecimenID, ref string CollectorsName, ref string CollectorsNumber)
        {
            string SQL = "SELECT TOP (1) CollectorsName, CollectorsNumber " +
                "FROM  CollectionAgent " +
                "WHERE (CollectionSpecimenID = " + CollectionSpecimenID.ToString() + ") " +
                "ORDER BY CollectorsNumber DESC, CollectorsName DESC ";
            System.Data.DataTable dt = new System.Data.DataTable();
            string Message = "";
            if (DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message) && dt.Rows.Count == 1)
            {
                CollectorsName = dt.Rows[0]["CollectorsName"].ToString();
                if(!dt.Rows[0]["CollectorsNumber"].Equals(System.DBNull.Value))
                    CollectorsNumber = dt.Rows[0]["CollectorsNumber"].ToString();
            }
        }

        private string AccessionNumberStringForIdentification(System.Collections.Generic.SortedDictionary<string, SpecimenPart> AccNrDict)
        {
            // extracting the numbers from the dictionary
            System.Collections.Generic.List<string> AccNrList = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<string, SpecimenPart> KV in AccNrDict)
                AccNrList.Add(KV.Key);

            bool ContinuousAccNrRange = false;
            string AccNrRange = "";
            for (int a = 0; a < AccNrList.Count; a++)
            {
                string aTo = AccNrList[a];
                int iPos = aTo.Length - 1;
                while (iPos > 0)
                {
                    int iTest;
                    if (!int.TryParse(aTo[iPos].ToString(), out iTest))
                        break;
                    iPos--;
                }
                int iTo;
                int.TryParse(aTo.Substring(iPos), out iTo);
                iTo = System.Math.Abs(iTo);
                if (a == 0)
                {
                    AccNrRange = AccNrList[a];
                }
                else
                {
                    string aFrom = AccNrList[a - 1];
                    int iPosFrom = aFrom.Length - 1;
                    while (iPosFrom > 0)
                    {
                        int iTest;
                        if (!int.TryParse(aFrom[iPosFrom].ToString(), out iTest))
                            break;
                        iPosFrom--;
                    }
                    int iFrom;
                    int.TryParse(aFrom.Substring(iPosFrom), out iFrom);
                    iFrom = System.Math.Abs(iFrom);
                    if (iTo - iFrom == 1)
                    {
                        ContinuousAccNrRange = true;
                    }
                    else
                    {
                        if (ContinuousAccNrRange)
                        {
                            AccNrRange += " - " + aFrom.Substring(aFrom.Length - 2);
                        }
                        AccNrRange += ",\r\n" + aTo;
                        ContinuousAccNrRange = false;
                    }
                    if (ContinuousAccNrRange && a == AccNrList.Count - 1)
                    {
                        AccNrRange += " - " + aTo.Substring(aTo.Length - 2);
                    }
                }
            }
            return AccNrRange;
        }

        /// <summary>
        /// Try to get a number out of an AccessionNumber
        /// </summary>
        /// <param name="AccessionNumber"></param>
        /// <returns></returns>
        private int? NumberFromAccessionNumber(string AccessionNumber)
        {
            int? Nr = null;
            string sNr = "";
            for (int i = AccessionNumber.Length - 1; i > 0; i--)
            {
                int TestNr;
                sNr = AccessionNumber.Substring(i);
                if (!int.TryParse(sNr, out TestNr))
                    break;
                else
                    Nr = TestNr;
            }
            return Nr;
        }

        public string XmlBalance(
        int TransactionID,
        string AdminAgentURI,
        System.IO.FileInfo XmlFile,
        System.IO.FileInfo XsltStilesheet,
        System.Data.DataTable dtBalance)
        {
            string XML = "";
            this._TransactionID = TransactionID;
            System.Data.DataTable dtSummary = new System.Data.DataTable();
            System.Data.DataRow[] rr = this._DataSet.Tables["Transaction"].Select("TransactionID = " + this._TransactionID.ToString());

            System.Xml.XmlWriter W;
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            W = System.Xml.XmlWriter.Create(XmlFile.FullName, settings);
            try
            {
                W.WriteStartDocument();
                W.WriteStartElement("Transaction");
                W.WriteElementString("Date", this.getDateInEnglish(System.DateTime.Now));
                System.DateTime Begin = System.DateTime.Now;
                System.DateTime End = System.DateTime.Now;
                bool DatePeriodOK = true;
                System.Data.DataRow R = rr[0];

                // FROM
                W.WriteStartElement("From");
                W.WriteStartElement("FromTransactionPartnerAddress");
                this.writeXmlAddress(ref W, AdminAgentURI);
                W.WriteEndElement();
                W.WriteEndElement();

                // TO
                W.WriteStartElement("To");
                W.WriteStartElement("ToTransactionPartnerAddress");
                if (R["FromTransactionPartnerAgentURI"].ToString() == AdminAgentURI)
                    this.writeXmlAddress(ref W, R["ToTransactionPartnerAgentURI"].ToString());
                else
                    this.writeXmlAddress(ref W, R["FromTransactionPartnerAgentURI"].ToString());
                W.WriteEndElement();
                W.WriteEndElement();

                // Obsolet - wird ueber AgentURI geregelt
                //foreach (System.Data.DataColumn C in R.Table.Columns)
                //{
                //    if (C.ColumnName.StartsWith("From") && C.ColumnName != "FromCollectionID" && C.ColumnName != "FromTransactionPartnerAgentURI")
                //    {
                //        W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                //    }
                //    if (!R[C.ColumnName].Equals(System.DBNull.Value) &&
                //       R[C.ColumnName].ToString().Length > 0 &&
                //       (C.ColumnName == "FromCollectionID"))
                //    {
                //        int CollectionID = 0;
                //        if (int.TryParse(R[C.ColumnName].ToString(), out CollectionID))
                //        {
                //            this.writeXmlCollection(ref W, CollectionID);
                //        }

                //    }
                //    if (C.ColumnName == "FromTransactionPartnerAgentURI" && !R[C.ColumnName].Equals(System.DBNull.Value))
                //    {
                //        W.WriteStartElement("FromTransactionPartnerAddress");
                //        this.writeXmlAddress(ref W, R["FromTransactionPartnerAgentURI"].ToString());
                //        W.WriteEndElement();
                //    }
                //}
                //W.WriteEndElement();

                // To
                //W.WriteStartElement("To");
                //foreach (System.Data.DataColumn C in R.Table.Columns)
                //{
                //    if (C.ColumnName.StartsWith("To") && C.ColumnName != "ToCollectionID" && C.ColumnName != "ToTransactionPartnerAgentURI")
                //    {
                //        W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                //    }
                //    if (!R[C.ColumnName].Equals(System.DBNull.Value) &&
                //       R[C.ColumnName].ToString().Length > 0 &&
                //       (C.ColumnName == "ToCollectionID"))
                //    {
                //        int CollectionID = 0;
                //        if (int.TryParse(R[C.ColumnName].ToString(), out CollectionID))
                //        {
                //            this.writeXmlCollection(ref W, CollectionID);
                //        }

                //    }
                //    if (C.ColumnName == "ToTransactionPartnerAgentURI" && !R[C.ColumnName].Equals(System.DBNull.Value))
                //    {
                //        W.WriteStartElement("ToTransactionPartnerAddress");
                //        this.writeXmlAddress(ref W, R["ToTransactionPartnerAgentURI"].ToString());
                //        W.WriteEndElement();
                //    }
                //}
                //W.WriteEndElement();

                System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> BalanceDictionary = new Dictionary<string, List<int>>();
                foreach (System.Data.DataRow RBalance in dtBalance.Rows)
                {
                    System.Collections.Generic.List<int> List = new List<int>();
                    List.Add(0);
                    List.Add(0);
                    List.Add(0);
                    if (!BalanceDictionary.ContainsKey(RBalance["ReportingCategory"].ToString()))
                        BalanceDictionary.Add(RBalance["ReportingCategory"].ToString(), List);
                }
                foreach (System.Data.DataRow RBalance in dtBalance.Rows)
                {
                    int Sent = 0;
                    int Received = 0;
                    if (RBalance["Direction"].ToString() == "sent") int.TryParse(RBalance["NumberOfUnits"].ToString(), out Sent);
                    else int.TryParse(RBalance["NumberOfUnits"].ToString(), out Received);
                    Sent = Sent + BalanceDictionary[RBalance["ReportingCategory"].ToString()][1];
                    Received = Received + BalanceDictionary[RBalance["ReportingCategory"].ToString()][0];
                    int Balance = Sent - Received;
                    BalanceDictionary[RBalance["ReportingCategory"].ToString()][0] = Received;
                    BalanceDictionary[RBalance["ReportingCategory"].ToString()][1] = Sent;
                    BalanceDictionary[RBalance["ReportingCategory"].ToString()][2] = Balance;
                }
                W.WriteStartElement("Balance");
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<int>> KV in BalanceDictionary)
                {
                    W.WriteStartElement("Detail");
                    W.WriteElementString("ReportingCategory", KV.Key);
                    W.WriteElementString("Received", KV.Value[0].ToString());
                    W.WriteElementString("Sent", KV.Value[1].ToString());
                    W.WriteElementString("Balance", KV.Value[2].ToString());
                    W.WriteEndElement();
                }
                W.WriteEndElement();
                int TotalBalance = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<int>> KV in BalanceDictionary)
                {
                    TotalBalance += KV.Value[2];
                }
                W.WriteElementString("TotalBalance", TotalBalance.ToString());
                W.WriteStartElement("Details");
                foreach (System.Data.DataRow RB in dtBalance.Rows)
                {
                    W.WriteStartElement("Detail");
                    W.WriteElementString("Direction", RB["Direction"].ToString());
                    W.WriteElementString("ReportingCategory", RB["ReportingCategory"].ToString());
                    System.DateTime dt;
                    string BeginDate = "";
                    if (System.DateTime.TryParse(RB["BeginDate"].ToString(), out dt))
                    {
                        BeginDate = dt.ToString("yyyy/MM/dd");//.Year.ToString() + "/";
                        //if (dt.Month < 10)
                        //    BeginDate += "0";
                        //BeginDate += dt.Month.ToString() + "/";
                        //if (dt.Day < 10)
                        //    BeginDate += "0";
                        //BeginDate += dt.Day.ToString();
                    }
                    W.WriteElementString("BeginDate", BeginDate);
                    W.WriteElementString("NumberOfUnits", RB["NumberOfUnits"].ToString());
                    W.WriteElementString("ToTransactionNumber", RB["ToTransactionNumber"].ToString());
                    W.WriteElementString("MaterialDescription", RB["MaterialDescription"].ToString());
                    W.WriteEndElement();
                }
                W.WriteEndElement();
                W.WriteFullEndElement();
                W.WriteEndDocument();
                W.Flush();
                W.Close();
                if (XsltStilesheet != null && XsltStilesheet.Exists)
                {
                    //#127
                    System.Xml.Xsl.XslCompiledTransform XSLT = new System.Xml.Xsl.XslCompiledTransform();
                    System.Xml.Xsl.XsltSettings XsltSettings = new System.Xml.Xsl.XsltSettings(true, true);
                    System.Xml.XmlResolver resolver = new System.Xml.XmlUrlResolver();
                    XSLT.Load(XsltStilesheet.FullName);

                    // Load the file to transform.
                    System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(XmlFile.FullName);

                    // The output file:
                    string OutputFile = XmlFile.FullName.Substring(0, XmlFile.FullName.Length
                        - XmlFile.Extension.Length) + ".htm";

                    // Create the writer.             
                    System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(OutputFile, XSLT.OutputSettings);

                    // Transform the file and send the output to the console.
                    XSLT.Transform(doc, writer);
                    writer.Close();
                    return OutputFile;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                if (W != null)
                {
                    W.Flush();
                    W.Close();
                }
            }
            return XML;
        }

        private int FromCollectionID(int TransactionID)
        {
            int CollectionID = 0;
            return CollectionID;
        }

        private int AdministratingCollectionID(int TransactionID)
        {
            int CollectionID = 0;
            return CollectionID;
        }

        private int ToCollectionID(int TransactionID)
        {
            int CollectionID = 0;
            return CollectionID;
        }

        private string TransactionPartnerAgentURI(int TransactionID, DiversityCollection.LookupTable.TransactionDirection Direction)
        {
            string URI = "";
            URI = DiversityCollection.LookupTable.TransactionAgentURI(TransactionID, Direction);
            return URI;
        }

        private string CollectionAgentURI(int CollectionID)
        {
            string URI = "";
            URI = DiversityCollection.LookupTable.CollectionAdministrativeContactAgentURI(CollectionID);
            return URI;
        }

        #endregion

        #region Conversion for Accession number

        public static System.Collections.Generic.Dictionary<Transaction.ConversionType, string> ConversionDictionary
        {
            get
            {
                if (Transaction._ConversionDictionary == null)
                {
                    Transaction._ConversionDictionary = new Dictionary<Transaction.ConversionType, string>();
                    Transaction._ConversionDictionary.Add(Transaction.ConversionType.No_Conversion, "No conversion");
                    Transaction._ConversionDictionary.Add(Transaction.ConversionType.Numeric_to_roman, "In Accession number: convert 3rd part to roman number (e.g.: BSPG-2000-VIX-001355)");
                }
                return Transaction._ConversionDictionary;
            }
        }

        public static string AccessionNumberConverted(Transaction.ConversionType ConversionType, string AccessionNumberOriginal)
        {
            if (ConversionType == ConversionType.No_Conversion) return AccessionNumberOriginal;
            char Separator = '-';
            if (AccessionNumberOriginal.IndexOf('-') == -1)
            {
                if (AccessionNumberOriginal.IndexOf(' ') > -1) Separator = ' ';
                else if (AccessionNumberOriginal.IndexOf('_') > -1) Separator = '_';
            }
            if (ConversionType == Transaction.ConversionType.Numeric_to_roman)
            {
                string[] Parts = AccessionNumberOriginal.Split(new char[] { Separator });
                if (Parts.Length > 2)
                {
                    string AccNrNew = Parts[0] + Separator + Parts[1] + Separator + Transaction.TranslateToRoman(Parts[2]);
                    if (Parts.Length > 3)
                        AccNrNew += Separator + Parts[3];
                    return AccNrNew;
                }
                else return AccessionNumberOriginal;
            }
            else
                return AccessionNumberOriginal;
        }

        public static string ConversionDescription(string Conversion)
        {
            string Description = "";
            if (Transaction._ConversionDescription == null)
            {
                Transaction._ConversionDescription = new Dictionary<string, string>();
                foreach (string str in Enum.GetNames(typeof(DiversityCollection.Transaction.ConversionType)))
                {
                    foreach (System.Collections.Generic.KeyValuePair<Transaction.ConversionType, string> CT in Transaction._ConversionDictionary)
                    {
                        if (CT.Key.ToString() == str)
                        {
                            Description = CT.Value;
                            break;
                        }
                    }
                    Transaction._ConversionDescription.Add(str.Replace("_", " "), Description);
                }
            }
            Description = Transaction._ConversionDescription[Conversion];
            return Description;
        }

        private static string TranslateToRoman(string Original) 
        {
            string NumericPart = "";
            string NoNumPart = "";
            int PositionOfNoNumPart = 0;
            for (int i = 0; i < Original.Length; i++)
            {
                int iTest = 0;
                if (int.TryParse(Original[i].ToString(), out iTest) && NoNumPart.Length == 0) NumericPart += Original[i];
                else
                {
                    NoNumPart += Original[i].ToString().ToLower();
                    PositionOfNoNumPart = i;
                }
            }
            string Roman1 = "";
            string Roman10 = "";
            string Roman100 = "";
            string Roman1000 = "";
            int Number = 0;
            if (int.TryParse(NumericPart, out Number))
            {
                int iEiner = Number % 10;
                switch (iEiner)
                {
                    case 1:
                        Roman1 = "I";
                        break;
                    case 2:
                        Roman1 = "II";
                        break;
                    case 3:
                        Roman1 = "III";
                        break;
                    case 4:
                        Roman1 = "IV";
                        break;
                    case 5:
                        Roman1 = "V";
                        break;
                    case 6:
                        Roman1 = "VI";
                        break;
                    case 7:
                        Roman1 = "VII";
                        break;
                    case 8:
                        Roman1 = "VIII";
                        break;
                    case 9:
                        Roman1 = "IX";
                        break;
                }
                Number = (Number - iEiner)/10;
                int iZehner = Number % 10;
                switch (iZehner)
                {
                    case 1:
                        Roman10 = "X";
                        break;
                    case 2:
                        Roman10 = "XX";
                        break;
                    case 3:
                        Roman10 = "XXX";
                        break;
                    case 4:
                        Roman10 = "XL";
                        break;
                    case 5:
                        Roman10 = "L";
                        break;
                    case 6:
                        Roman10 = "LX";
                        break;
                    case 7:
                        Roman10 = "LXX";
                        break;
                    case 8:
                        Roman10 = "LXXX";
                        break;
                    case 9:
                        Roman10 = "XC";
                        break;
                }
                Number = (Number - iZehner) / 10;
                int iHunderter = Number % 10;
                switch (iHunderter)
                {
                    case 1:
                        Roman100 = "C";
                        break;
                    case 2:
                        Roman100 = "CC";
                        break;
                    case 3:
                        Roman100 = "CCC";
                        break;
                    case 4:
                        Roman100 = "CD";
                        break;
                    case 5:
                        Roman100 = "D";
                        break;
                    case 6:
                        Roman100 = "DC";
                        break;
                    case 7:
                        Roman100 = "DCC";
                        break;
                    case 8:
                        Roman100 = "DCCC";
                        break;
                    case 9:
                        Roman100 = "CC";
                        break;
                }
                Number = (Number - iHunderter) / 10;
                int iTausender = Number % 10;
                switch (iTausender)
                {
                    case 1:
                        Roman1000 = "M";
                        break;
                    case 2:
                        Roman1000 = "MM";
                        break;
                    case 3:
                        Roman1000 = "MMM";
                        break;
                }
            }
            string Roman = Roman1000 + Roman100 + Roman10 + Roman1;
            if (NoNumPart.Length > 0)
            {
                if (PositionOfNoNumPart == 0) Roman = NoNumPart + Roman;
                else Roman = Roman + NoNumPart;
            }
            return Roman;
        }

        public static Transaction.ConversionType ConversionTypeFormString(string Conversion)
        {
            foreach (System.Collections.Generic.KeyValuePair<Transaction.ConversionType, string> KV in Transaction.ConversionDictionary)
                if (KV.Key.ToString() == Conversion || KV.Key.ToString().Replace("_", " ") == Conversion) return KV.Key;
            return Transaction.ConversionType.No_Conversion;
        }
        
        #endregion

        #region Statistics

        public static string XmlStatistics(string Agent, string AgentUri, System.IO.FileInfo XmlFile, System.IO.FileInfo SchemaFile, System.DateTime From, System.DateTime Until)
        {
            string XML = "";

            System.Xml.XmlWriter W;
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            W = System.Xml.XmlWriter.Create(XmlFile.FullName, settings);

            try
            {
                W.WriteStartDocument();
                W.WriteStartElement("Transaction");
                W.WriteElementString("TransactionAgent", Agent);
                W.WriteElementString("From", From.Year.ToString() + "-" + From.Month.ToString() + "-" + From.Day.ToString());
                W.WriteElementString("Until", Until.Year.ToString() + "-" + Until.Month.ToString() + "-" + Until.Day.ToString());

                string SQL = "SELECT COUNT(*) AS NumberOfLoans " +
                    "FROM  [Transaction] AS T " +
                    "WHERE  (FromTransactionPartnerAgentURI = '" + AgentUri + "') " +
                    "AND (BeginDate BETWEEN CONVERT(DATETIME, '" + From.Year.ToString() + "-" + From.Month.ToString() + "-" + From.Day.ToString() + "', 102) " +
                    "AND CONVERT(DATETIME,  '" + Until.Year.ToString() + "-" + Until.Month.ToString() + "-" + Until.Day.ToString() + "', 102)) " +
                    "AND (TransactionType = 'loan')";

                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);

                W.WriteElementString("NumberOfLoans", Result);

                SQL = "declare  @Stat table ([CollectionSpecimenID] [int] , [IsType] nvarchar(50), [ReportingCategory] nvarchar(50)) " +
                    "insert into @Stat " +
                    "SELECT S.CollectionSpecimenID, '' AS IsType, " +
                    "CASE WHEN T.ReportingCategory IS NULL OR LEN(RTRIM(T.ReportingCategory)) = 0 THEN '' ELSE T.ReportingCategory END AS ReportingCategory " +
                    "FROM  [Transaction] AS T INNER JOIN CollectionSpecimenTransaction AS S ON T.TransactionID = S.TransactionID  " +
                    "where  (T.FromTransactionPartnerAgentURI = '" + AgentUri + "') " +
                    "AND T.BeginDate BETWEEN CONVERT(DATETIME, '" + From.Year.ToString() + "-" + From.Month.ToString() + "-" + From.Day.ToString() + "', 102) " +
                    "and CONVERT(DATETIME, '" + Until.Year.ToString() + "-" + Until.Month.ToString() + "-" + Until.Day.ToString() + "', 102) " +
                    "AND T.TransactionType = 'loan' " +
                    "; " +
                    //"GROUP BY S.CollectionSpecimenID, CASE WHEN T.ReportingCategory IS NULL OR LEN(RTRIM(T.ReportingCategory)) = 0 THEN '' ELSE T.ReportingCategory END; " +
                    "UPDATE S SET S.IsType = 'Type' " +
                    "from @Stat S, Identification AS I " +
                    "WHERE S.CollectionSpecimenID = I.CollectionSpecimenID " +
                    "and I.TypeStatus <> 'not a type'; " +
                    "select count(*)  AS SpecimenOnLoan, IsType, ReportingCategory " +
                    "from @Stat " +
                    "group by IsType, ReportingCategory";

                //SQL = "SELECT CASE WHEN I.TypeStatus IS NULL OR " +
                //    "I.TypeStatus = 'not a type' THEN NULL ELSE 'Type' END AS IsType, CASE WHEN T.ReportingCategory IS NULL OR LEN(RTRIM(T.ReportingCategory)) = 0 THEN '' ELSE T.ReportingCategory END AS ReportingCategory, COUNT(DISTINCT S.CollectionSpecimenID) AS SpecimenOnLoan " +
                //    "FROM  [Transaction] AS T INNER JOIN " +
                //    "CollectionSpecimenTransaction AS S ON T.TransactionID = S.TransactionID INNER JOIN " +
                //    "Identification AS I ON S.CollectionSpecimenID = I.CollectionSpecimenID " +
                //    "where  (T.FromTransactionPartnerAgentURI = '" + AgentUri + "') " +
                //    "AND T.BeginDate BETWEEN CONVERT(DATETIME, '" + From.Year.ToString() + "-" + From.Month.ToString() + "-" + From.Day.ToString() + "', 102) " +
                //    "and CONVERT(DATETIME, '" + Until.Year.ToString() + "-" + Until.Month.ToString() + "-" + Until.Day.ToString() + "', 102) " +
                //    "AND T.TransactionType = 'loan' " +
                //    "GROUP BY CASE WHEN T.ReportingCategory IS NULL OR LEN(RTRIM(T.ReportingCategory)) = 0 THEN '' ELSE T.ReportingCategory END, CASE WHEN I.TypeStatus IS NULL OR " +
                //    "I.TypeStatus = 'not a type' THEN NULL ELSE 'Type' END";
                System.Data.DataTable dt = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    W.WriteStartElement("Groups");
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        W.WriteStartElement("Group");
                        W.WriteElementString("ReportingCategory", R["ReportingCategory"].ToString());
                        string IsType = "NoType";
                        if (R["IsType"].ToString().ToLower() == "type")
                            IsType = "Type";
                        W.WriteElementString(IsType, R["SpecimenOnLoan"].ToString());
                        W.WriteEndElement();//Group
                    }
                    W.WriteEndElement();//Groups
                }

                //SQL = "SELECT CASE WHEN I.TypeStatus IS NULL OR " +
                //    "I.TypeStatus = 'not a type' THEN NULL ELSE 'Type' END AS IsType, COUNT(DISTINCT S.CollectionSpecimenID) AS SpecimenOnLoan " +
                //    "FROM  [Transaction] AS T INNER JOIN " +
                //    "CollectionSpecimenTransaction AS S ON T.TransactionID = S.TransactionID INNER JOIN " +
                //    "Identification AS I ON S.CollectionSpecimenID = I.CollectionSpecimenID " +
                //    "where  (T.FromTransactionPartnerAgentURI = '" + AgentUri + "') " +
                //    "AND T.BeginDate BETWEEN CONVERT(DATETIME, '" + From.Year.ToString() + "-" + From.Month.ToString() + "-" + From.Day.ToString() + "', 102) and CONVERT(DATETIME, '" + Until.Year.ToString() + "-" + Until.Month.ToString() + "-" + Until.Day.ToString() + "', 102) " +
                //    "AND T.TransactionType = 'loan' " +
                //    "GROUP BY CASE WHEN I.TypeStatus IS NULL OR I.TypeStatus = 'not a type' THEN NULL ELSE 'Type' END";
                SQL = "declare  @Stat table ([CollectionSpecimenID] [int] , [IsType] nvarchar(50)) " +
                    "insert into @Stat " +
                    "SELECT S.CollectionSpecimenID, '' AS IsType " +
                    "FROM  [Transaction] AS T INNER JOIN CollectionSpecimenTransaction AS S ON T.TransactionID = S.TransactionID  " +
                    "where  (T.FromTransactionPartnerAgentURI = '" + AgentUri + "') " +
                    "AND T.BeginDate BETWEEN CONVERT(DATETIME, '" + From.Year.ToString() + "-" + From.Month.ToString() + "-" + From.Day.ToString() + "', 102) " +
                    "and CONVERT(DATETIME, '" + Until.Year.ToString() + "-" + Until.Month.ToString() + "-" + Until.Day.ToString() + "', 102) " +
                    "AND T.TransactionType = 'loan' " +
                    //"GROUP BY S.CollectionSpecimenID; " +
                    "; " +
                    "UPDATE S SET S.IsType = 'Type' " +
                    "from @Stat S, Identification AS I " +
                    "WHERE S.CollectionSpecimenID = I.CollectionSpecimenID " +
                    "and I.TypeStatus <> 'not a type'; " +
                    "select count(*)  AS SpecimenOnLoan, IsType " +
                    "from @Stat " +
                    "group by IsType";
                dt.Rows.Clear();
                ad.SelectCommand.CommandText = SQL;
                ad.Fill(dt);
                W.WriteStartElement("Total");
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    string Type = "NoType";
                    if (R["IsType"].ToString().ToLower() == "type")
                        Type = "Type";
                    W.WriteElementString(Type, R["SpecimenOnLoan"].ToString());
                }
                W.WriteEndElement();//Total

                W.WriteEndElement();//Statistics
                W.WriteEndDocument();
                W.Flush();
                W.Close();
                if (SchemaFile.FullName.Length > 0)
                {
                    if (SchemaFile.Exists)
                    {
                        //#127
                        System.Xml.Xsl.XslCompiledTransform XSLT = new System.Xml.Xsl.XslCompiledTransform();
                        System.Xml.Xsl.XsltSettings XsltSettings = new System.Xml.Xsl.XsltSettings(true, true);
                        System.Xml.XmlResolver resolver = new System.Xml.XmlUrlResolver();
                        XSLT.Load(SchemaFile.FullName);

                        // Load the file to transform.
                        System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(XmlFile.FullName);

                        // The output file:
                        string OutputFile = XmlFile.FullName.Substring(0, XmlFile.FullName.Length
                            - XmlFile.Extension.Length) + ".htm";

                        // Create the writer.             
                        System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(OutputFile, XSLT.OutputSettings);

                        // Transform the file and send the output to the console.
                        XSLT.Transform(doc, writer);
                        writer.Close();
                        return OutputFile;
                    }
                }
                else
                {
                    XML = XmlFile.FullName;
                }
            }
            catch (System.Exception ex)
            {
            }

            return XML;
        }

        public static string XmlStatistics(string Agent, string AddressDatabase, string AgentUri, System.IO.FileInfo XmlFile, System.IO.FileInfo SchemaFile, System.DateTime From, System.DateTime Until)
        {
            string XML = "";

            System.Xml.XmlWriter W;
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            W = System.Xml.XmlWriter.Create(XmlFile.FullName, settings);

            try
            {
                W.WriteStartDocument();
                W.WriteStartElement("Transaction");
                W.WriteElementString("TransactionAgent", Agent);
                W.WriteElementString("From", From.Year.ToString() + "-" + From.Month.ToString() + "-" + From.Day.ToString());
                W.WriteElementString("Until", Until.Year.ToString() + "-" + Until.Month.ToString() + "-" + Until.Day.ToString());

                string SQL = "";

                SQL = "declare  @Stat table ([TransactionID] [int] , [Country] nvarchar(50)); " +
                    "insert into @Stat " +
                    "SELECT T.TransactionID, " +
                    "CASE WHEN A.Country IS NULL OR LEN(RTRIM(A.Country)) = 0 THEN '' ELSE A.Country END AS Country " +
                    "FROM " + AddressDatabase + ".dbo.AgentContactInformation A,  [Transaction] AS T INNER JOIN CollectionSpecimenTransaction AS S ON T.TransactionID = S.TransactionID  " +
                    "where  (T.FromTransactionPartnerAgentURI = '" + AgentUri + "') " +
                    "AND T.BeginDate BETWEEN CONVERT(DATETIME, '" + From.Year.ToString() + "-" + From.Month.ToString() + "-" + From.Day.ToString() + "', 102) " +
                    "and CONVERT(DATETIME, '" + Until.Year.ToString() + "-" + Until.Month.ToString() + "-" + Until.Day.ToString() + "', 102) " +
                    "AND T.TransactionType = 'loan' " +
                    "AND " + AddressDatabase + ".dbo.BaseURL() + CAST(A.AgentID as varchar) = T.ToTransactionPartnerAgentURI " +
                    "AND EXISTS(SELECT * from " + AddressDatabase + ".dbo.AgentContactInformation A1 WHERE A.AgentID = A1.AgentID GROUP BY A1.AgentID HAVING A.DisplayOrder = MIN(A1.DisplayOrder)) " +
                    "GROUP BY T.TransactionID, A.Country; " +
                    "select count(*) AS Loans, Country " +
                    "from @Stat " +
                    "group by Country";

                System.Data.DataTable dt = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    W.WriteStartElement("Groups");
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        W.WriteStartElement("Group");
                        W.WriteElementString("Country", R["Country"].ToString());
                        W.WriteElementString("Loans", R["Loans"].ToString());
                        W.WriteEndElement();//Group
                    }
                    W.WriteEndElement();//Groups
                }

                W.WriteEndElement();//Statistics
                W.WriteEndDocument();
                W.Flush();
                W.Close();
                if (SchemaFile.FullName.Length > 0)
                {
                    if (SchemaFile.Exists)
                    {
                        try
                        {
                            //#127
                            System.Xml.Xsl.XslCompiledTransform XSLT = new System.Xml.Xsl.XslCompiledTransform();
                            System.Xml.Xsl.XsltSettings XsltSettings = new System.Xml.Xsl.XsltSettings(true, true);
                            System.Xml.XmlResolver resolver = new System.Xml.XmlUrlResolver();
                            XSLT.Load(SchemaFile.FullName);

                            // Load the file to transform.
                            System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(XmlFile.FullName);

                            // The output file:
                            string OutputFile = XmlFile.FullName.Substring(0, XmlFile.FullName.Length
                                - XmlFile.Extension.Length) + ".htm";

                            // Create the writer.             
                            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(OutputFile, XSLT.OutputSettings);

                            // Transform the file and send the output to the console.
                            XSLT.Transform(doc, writer);
                            writer.Close();
                            writer.Flush();
                            return OutputFile;
                        }
                        catch (System.Exception ex)
                        {
                        }
                    }
                }
                else
                {
                    XML = XmlFile.FullName;
                }
            }
            catch (System.Exception ex)
            {
            }
            finally
            {
            }

            return XML;
        }

        #endregion  
 
    }
}
