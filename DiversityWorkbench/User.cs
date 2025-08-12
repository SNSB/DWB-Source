using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityWorkbench
{
    public class User : DiversityWorkbench.WorkbenchUnit, DiversityWorkbench.IWorkbenchUnit
    {
        #region Construction
        public User(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
        }
        
        #endregion

        #region Interface

        public override string ServiceName() { return "DiversityUsers"; }

        public System.Collections.Generic.Dictionary<string, string> UnitValues(int ID)
        {
            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                string SQL = "select UserID AS _URI, CombinedNameCache AS _DisplayText, " +
                    " CombinedNameCache " +
                    "from UserInfo " +
                    "where UserInfo.UserID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                //SQL = "SELECT Name AS Author " +
                //    "FROM ReferenceRelator " +
                //    "WHERE RefID = " + ID.ToString() +
                //    "AND Role = 'Aut' ORDER BY Sequence";
                //this.getDataFromTable(SQL, ref Values);

                //SQL = "SELECT Name AS Editor " +
                //    "FROM ReferenceRelator " +
                //    "WHERE RefID = " + ID.ToString() +
                //    "AND Role = 'Edt' ORDER BY Sequence";
                //this.getDataFromTable(SQL, ref Values);

                //SQL = "SELECT Name AS Serieseditor " +
                //    "FROM ReferenceRelator " +
                //    "WHERE RefID = " + ID.ToString() +
                //    "AND Role = 'Sed' ORDER BY Sequence";
                //this.getDataFromTable(SQL, ref Values);

                //SQL = "SELECT Content AS Keyword " +
                //    "FROM ReferenceDescriptor " +
                //    "WHERE RefID = " + ID.ToString();
                //this.getDataFromTable(SQL, ref Values);

            }
            return Values;
        }

        public string MainTable() { return "UserInfo"; }

        public DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns()
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[1];
            QueryDisplayColumns[0].DisplayText = "Name";
            QueryDisplayColumns[0].DisplayColumn = "CombinedNameCache";
            QueryDisplayColumns[0].OrderColumn = "CombinedNameCache";
            QueryDisplayColumns[0].IdentityColumn = "UserID";
            QueryDisplayColumns[0].TableName = "UserInfo";
            //QueryDisplayColumns[1].DisplayText = "Title";
            //QueryDisplayColumns[1].DisplayColumn = "Title";
            //QueryDisplayColumns[1].OrderColumn = "Title";
            //QueryDisplayColumns[1].IdentityColumn = "RefID";
            //QueryDisplayColumns[1].TableName = "ReferenceTitle";
            //QueryDisplayColumns[2].DisplayText = "Author";
            //QueryDisplayColumns[2].DisplayColumn = "Name";
            //QueryDisplayColumns[2].OrderColumn = "Name";
            //QueryDisplayColumns[2].IdentityColumn = "RefID";
            //QueryDisplayColumns[2].TableName = "ReferenceRelator";
            return QueryDisplayColumns;
        }

        public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions()
        {
            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

            string Description = DiversityWorkbench.Functions.ColumnDescription("UserInfo", "CombinedNameCache");
            DiversityWorkbench.QueryCondition qName = new DiversityWorkbench.QueryCondition(true, "UserInfo", "UserID", "CombinedNameCache", "Name", "Name", "Name", Description);
            QueryConditions.Add(qName);

            //Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "RefDescription");
            //DiversityWorkbench.QueryCondition qDescription = new DiversityWorkbench.QueryCondition(true, "ReferenceTitle", "RefID", "RefDescription", "Reference", "Description", "Description", Description);
            //QueryConditions.Add(qDescription);

            //System.Data.DataTable dtSource = new System.Data.DataTable();
            //string SQL = "SELECT NULL  AS [Value], NULL AS Display UNION " +
            //    "SELECT DISTINCT ImportedFrom  AS [Value], ImportedFrom AS Display " +
            //    "FROM ReferenceTitle " +
            //    "ORDER BY Display";
            //Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //try { a.Fill(dtSource); }
            //catch { }
            //Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "ImportedFrom");
            //DiversityWorkbench.QueryCondition qImportedFrom = new DiversityWorkbench.QueryCondition(true, "ReferenceDescriptor", "RefID", "ImportedFrom", "Source", "Source", "Source", Description, dtSource, false);
            //QueryConditions.Add(qImportedFrom);

            //Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "ImportedID");
            //DiversityWorkbench.QueryCondition qImportedID = new DiversityWorkbench.QueryCondition(false, "ReferenceTitle", "RefID", "ImportedID", "Source", "Source ID", "Source ID", Description);
            //QueryConditions.Add(qImportedID);

            //Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "DateYear");
            //DiversityWorkbench.QueryCondition qDate = new DiversityWorkbench.QueryCondition(true, "ReferenceTitle", "RefID", "DateYear", "Reference", "Date", "Date", Description, "DateDay", "DateMonth", "DateYear");
            //QueryConditions.Add(qDate);

            //Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceRelator", "Name");
            //DiversityWorkbench.QueryCondition qAuthor = new DiversityWorkbench.QueryCondition(true, "ReferenceRelator", "RefID", "Name", "Authors", "Author", "Author", Description);
            //QueryConditions.Add(qAuthor);

            //System.Data.DataTable dtReferenceCollection = new System.Data.DataTable();
            //SQL = "SELECT NULL  AS [Value], NULL AS Display UNION " +
            //    "SELECT DISTINCT ElementID  AS [Value], [Content] AS Display " +
            //    "FROM ReferenceDescriptor " +
            //    "WHERE (ElementID = 22) " +
            //    "ORDER BY Display";
            //a.SelectCommand.CommandText = SQL;
            //try { a.Fill(dtReferenceCollection); }
            //catch { }
            //Description = "Collection of reference titels";
            //DiversityWorkbench.QueryCondition qReferenceCollection = new DiversityWorkbench.QueryCondition(true, "ReferenceDescriptor", "RefID", "ElementID", "Keywords and marker", "Collection", "Reference collection", Description, dtReferenceCollection, false);
            //QueryConditions.Add(qReferenceCollection);

            //Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceDescriptor", "Content");
            //DiversityWorkbench.QueryCondition qKeyword = new DiversityWorkbench.QueryCondition(true, "ReferenceDescriptor", "RefID", "Content", "Keywords and marker", "Keyword", "Keyword", Description);
            //QueryConditions.Add(qKeyword);
            //DiversityWorkbench.QueryCondition qKeyword2 = new DiversityWorkbench.QueryCondition(true, "ReferenceDescriptor", "RefID", "Content", "Keywords and marker", "Keyword", "Keyword", Description);
            //QueryConditions.Add(qKeyword2);
            //DiversityWorkbench.QueryCondition qKeyword3 = new DiversityWorkbench.QueryCondition(false, "ReferenceDescriptor", "RefID", "Content", "Keywords and marker", "Keyword", "Keyword", Description);
            //QueryConditions.Add(qKeyword3);
            //DiversityWorkbench.QueryCondition qKeyword4 = new DiversityWorkbench.QueryCondition(false, "ReferenceDescriptor", "RefID", "Content", "Keywords and marker", "Keyword", "Keyword", Description);
            //QueryConditions.Add(qKeyword4);

            //Description = DiversityWorkbench.Functions.ColumnDescription("ReferencePrivateDescriptor", "Content");
            //DiversityWorkbench.QueryCondition qMarker = new DiversityWorkbench.QueryCondition(true, "ReferencePrivateDescriptor", "RefID", "Content", "Keywords and marker", "Marker", "Marker", Description);
            //QueryConditions.Add(qMarker);

            //Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceNote", "Content");
            //DiversityWorkbench.QueryCondition qAbstract = new DiversityWorkbench.QueryCondition(true, "ReferenceNote", "RefID", "Content", "Abstract and notes", "Abstract", "Abstract", Description);
            //QueryConditions.Add(qAbstract);

            //Description = DiversityWorkbench.Functions.ColumnDescription("ReferencePrivateNote", "Content");
            //DiversityWorkbench.QueryCondition qNotes = new DiversityWorkbench.QueryCondition(true, "ReferencePrivateNote", "RefID", "Content", "Abstract and notes", "Notes", "Notes", Description);
            //QueryConditions.Add(qNotes);

            //Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceAvailability", "FilingCode");
            //DiversityWorkbench.QueryCondition qFilingCode = new DiversityWorkbench.QueryCondition(true, "ReferenceAvailability", "RefID", "FilingCode", "Availability", "Code", "Filing code", Description);
            //QueryConditions.Add(qFilingCode);

            //Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceAvailability", "RequestDate");
            //DiversityWorkbench.QueryCondition qRequestDate = new DiversityWorkbench.QueryCondition(false, "ReferenceAvailability", "RefID", "RequestDate", "Availability", "Date", "Request date", Description);
            //QueryConditions.Add(qRequestDate);

            return QueryConditions;
        }

        public static string CurrentUserName
        {
            get
            {
                string Name = "";
                string SQL = "SELECT CombinedNameCache FROM UserProxy WHERE (LoginName = USER_NAME())";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    Name = C.ExecuteScalar()?.ToString() ?? string.Empty;
                    con.Close();
                    return Name;
                }
                catch { }
                C.CommandText = "SELECT USER_NAME()";
                try
                {
                    con.Open();
                    Name = C.ExecuteScalar()?.ToString() ?? string.Empty;
                    con.Close();
                    return Name;
                }
                catch { }
                return Name;
            }
        }

        //public static int? CurrentProjectID
        //{
        //    get
        //    {
        //        int? ProjectID = null;
        //        string SQL = "SELECT CurrentProjectID FROM UserProxy WHERE (LoginName = USER_NAME())";
        //        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
        //        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
        //        try
        //        {
        //            con.Open();
        //            ProjectID = int.Parse(C.ExecuteScalar()?.ToString());
        //            con.Close();
        //            return ProjectID;
        //        }
        //        catch { }
        //        return ProjectID;
        //    }
        //    set
        //    {
        //        string SQL = "UPDATE UserProxy SET CurrentProjectID = " + value + " WHERE (LoginName = USER_NAME())";
        //        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
        //        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
        //        try
        //        {
        //            con.Open();
        //            C.ExecuteNonQuery();
        //            con.Close();
        //        }
        //        catch { }
        //    }
        //}

        #endregion

        #region Properties

        //public override DiversityWorkbench.ServerConnection ServerConnection
        //{
        //    get { return _ServerConnection; }
        //    set
        //    {
        //        if (value != null)
        //            this._ServerConnection = value;
        //        else
        //        {
        //            this._ServerConnection = new ServerConnection();
        //            this._ServerConnection.DatabaseServer = "127.0.0.1";
        //            this._ServerConnection.IsTrustedConnection = true;
        //        }
        //        this._ServerConnection.ModuleName = "DiversityUsers";
        //        this._ServerConnection.DatabaseName = "DiversityUsers";
        //    }
        //}

        #endregion    

        #region Secure Password

        private static System.Security.SecureString _securePW = new System.Security.SecureString();
        public static string Password
        {
            get
            {
                if (DiversityWorkbench.User._securePW.Length > 0)
                {
                    IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(DiversityWorkbench.User._securePW);
                    return System.Runtime.InteropServices.Marshal.PtrToStringUni(ptr);
                }
                else
                    return "";
            }
            set
            {
                string PWklar = value;
                DiversityWorkbench.User._securePW.Clear();
                for (int i = 0; i < PWklar.Length; i++)
                    DiversityWorkbench.User._securePW.AppendChar(PWklar[i]);
            }
        }
        
        #endregion    
    }
}
