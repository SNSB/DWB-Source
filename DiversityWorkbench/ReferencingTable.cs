using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench
{

    public interface ReferencingTableProvider
    {
        System.Data.DataTable ReferencingTable();
    }

    public struct ReferencingTableLink
    {
        /// <summary>
        /// the name of the datacolumn on which the referencing table refers
        /// </summary>
        public string ReferencedTable;
        /// <summary>
        /// the name of the column containing the ID of the dataset on which the referencing table refers
        /// </summary>
        public string ReferencedColumn;
        /// <summary>
        /// the name of the table containing the referenced column and the linked column
        /// </summary>
        public string LinkTable;
        /// <summary>
        ///// the name of the column in the main table that is used for the query
        /// </summary>
        public string LinkedColumn;
        /// <summary>
        /// the display text shown in a user interface, e.g. the name of the table
        /// </summary>
        public string DisplayText;
    }

    public class ReferencingTable
    {

        #region Parameter

        public static string SqlAnnotation = "SELECT AnnotationID, ReferencedAnnotationID, AnnotationType, Title, Annotation, URI, ReferenceDisplayText, ReferenceURI, SourceDisplayText, SourceURI, IsInternal, " +
            "ReferencedID, ReferencedTable, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy " +
            "FROM Annotation";

        public static string SqlIdentifier = "SELECT ID, ReferencedTable, ReferencedID, Type, Identifier, URL, Notes, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID " +
            "FROM ExternalIdentifier";

        public enum IdentifierType { ID }//, Regulation }

        private System.Data.DataTable _dtReferencedTables;
        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.ReferencingTableLink> _ReferencingTableLinks;
        private string _ReferencingTableName;

        #endregion

        #region Construction

        public ReferencingTable()
        {
        }

        public ReferencingTable(string ReferencingTable,
            System.Collections.Generic.Dictionary<string, DiversityWorkbench.ReferencingTableLink> ReferencingTableLinks)
        {
            this._ReferencingTableName = ReferencingTable;
            this._ReferencingTableLinks = ReferencingTableLinks;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Where clause for query
        /// </summary>
        /// <param name="ReferencingTable">The name of the referencing table</param>
        /// <param name="ReferencedTable">The name of the refered table</param>
        /// <param name="Restriction">The restriction of the query</param>
        /// <param name="Type">The selected type of the annotation</param>
        /// <returns></returns>
        public string WhereClause(string ReferencingTable, string ReferencedTable, string Column, string QueryOperator, string Restriction, string Type)
        {
            string SQL = "";
            try
            {
                if (Restriction.Length == 0 && QueryOperator != "•" && QueryOperator != "Ø")
                    return "";

                if (ReferencedTable.Length > 0)
                {
                    SQL = "SELECT T." + this._ReferencingTableLinks[ReferencedTable].LinkedColumn +
                    " FROM [" + this._ReferencingTableName + "] AS R, " +
                    " [" + this._ReferencingTableLinks[ReferencedTable].LinkTable + "] AS T " +
                    "WHERE R.ReferencedID = T." + this._ReferencingTableLinks[ReferencedTable].ReferencedColumn +
                    " AND R.ReferencedTable = N'" + this._ReferencingTableLinks[ReferencedTable].ReferencedTable + "'";
                    if (Type.Length > 0)
                    {
                        SQL += " AND R.Type = '" + Type + "' AND R." + this.Restriction(Column, QueryOperator, Restriction);
                    }
                    else
                        SQL += " AND R." + this.Restriction(Column, QueryOperator, Restriction);
                }
                else
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, ReferencingTableLink> KV in this._ReferencingTableLinks)
                    {
                        if (SQL.Length > 0)
                            SQL += " UNION ";
                        SQL += "SELECT T." + KV.Value.LinkedColumn +
                        " FROM [" + this._ReferencingTableName + "] AS R, " +
                        " [" + KV.Value.LinkTable + "] AS T " +
                        "WHERE R.ReferencedID = T." + KV.Value.ReferencedColumn +
                        " AND R.ReferencedTable = N'" + KV.Value.ReferencedTable + "'";
                        if (Type.Length > 0)
                        {
                            SQL += " AND R.Type = '" + Type + "' AND R." + this.Restriction(Column, QueryOperator, Restriction);
                        }
                        else
                            SQL += " AND R." + this.Restriction(Column, QueryOperator, Restriction);
                    }
                }
            }
            catch(System.Exception ex) { }
            return SQL;
        }

        private string Restriction(string Column, string QueryOperator, string Restriction)
        {
            string SQL = Column;
            switch (QueryOperator)
            {
                case "~":
                    SQL += " LIKE N'" + Restriction.Replace("'", "''") + "%'";
                    break;
                case "=":
                    SQL += " = N'" + Restriction.Replace("'", "''") + "'";
                    break;
                case "Ø":
                case "•":
                    SQL += " <> ''";
                    break;
            }
            return SQL;
        }

        public System.Data.DataTable DtReferencedTables
        {
            get
            {
                if (this._dtReferencedTables == null)
                {
                    this._dtReferencedTables = new System.Data.DataTable();
                    System.Data.DataColumn CTable = new System.Data.DataColumn("Table", typeof(System.String));
                    this._dtReferencedTables.Columns.Add(CTable);
                    System.Data.DataColumn CDisplayText = new System.Data.DataColumn("DisplayText", typeof(System.String));
                    this._dtReferencedTables.Columns.Add(CDisplayText);
                    System.Data.DataRow Rnull = this._dtReferencedTables.NewRow();
                    Rnull["Table"] = "";
                    Rnull["DisplayText"] = "";
                    this._dtReferencedTables.Rows.Add(Rnull);
                    if (this._ReferencingTableLinks != null)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, ReferencingTableLink> KV in this._ReferencingTableLinks)
                        {
                            System.Data.DataRow R = this._dtReferencedTables.NewRow();
                            R["Table"] = KV.Key;
                            R["DisplayText"] = KV.Value.DisplayText;
                            this._dtReferencedTables.Rows.Add(R);
                        }
                    }
                }
                return this._dtReferencedTables;
            }
        }

        public string TableName() { return this._ReferencingTableName; }

        #region Type lists

        private System.Collections.Generic.List<string> _TypeList;

        public System.Collections.Generic.List<string> TypeList()
        {
            if (this._TypeList == null || this._TypeList.Count == 0)
            {
                this._TypeList = new List<string>();
                string SQL = "SELECT Type " +
                    "FROM " + this.TableName() + "Type " +
                    "ORDER BY Type";
                System.Data.DataTable dt = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                this._TypeList.Add("");
                if (dt.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow R in dt.Rows)
                        this._TypeList.Add(R[0].ToString());
                }
            }
            return this._TypeList;
        }

        //private static System.Collections.Generic.List<string> _Regulations;
        //public static System.Collections.Generic.List<string> Regulations()
        //{
        //    if (_Regulations == null)
        //    {
        //        string SQL = SqlTypeList("Regulation");
        //        System.Data.DataTable dt = new System.Data.DataTable();
        //        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //        ad.Fill(dt);
        //        if (dt.Rows.Count > 0)
        //        {
        //            _Regulations = new List<string>();
        //            foreach (System.Data.DataRow R in dt.Rows)
        //                _Regulations.Add(R[0].ToString());
        //        }
        //    }
        //    return _Regulations;
        //}

        //public static void ResetRegulations()
        //{
        //    _Regulations = null;
        //}

        private static System.Collections.Generic.List<string> _IDs;
        public static System.Collections.Generic.List<string> IDs()
        {
            if (_IDs == null)
            {
                string SQL = SqlTypeList("ID");
                System.Data.DataTable dt = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    _IDs = new List<string>();
                    foreach (System.Data.DataRow R in dt.Rows)
                        _IDs.Add(R[0].ToString());
                }
            }
            return _IDs;
        }

        public static void ResetIDs()
        {
            _IDs = null;
        }

        private static string SqlTypeList(string ParentType)
        {
            string SQL = "DECLARE @ExtID TABLE ([Type] nvarchar(50) Primary key, [ParentType] nvarchar(50)) " +
                "INSERT INTO @ExtID( Type, ParentType) " +
                "SELECT [Type] " +
                ",[ParentType] " +
                "FROM [dbo].[ExternalIdentifierType] T " +
                "WHERE T.ParentType = '" + ParentType + "' " +
                "WHILE (SELECT COUNT(*) FROM [dbo].[ExternalIdentifierType] T, @ExtID R  " +
                "WHERE T.ParentType = R.Type " +
                "AND T.Type NOT IN (SELECT R.Type FROM @ExtID R) ) > 0 " +
                "BEGIN " +
                "INSERT INTO @ExtID( Type, ParentType) " +
                "SELECT T.[Type] " +
                ",T.[ParentType] " +
                "FROM [dbo].[ExternalIdentifierType] T, @ExtID R  " +
                "WHERE T.ParentType = R.Type " +
                "AND T.Type NOT IN (SELECT R.Type FROM @ExtID R) " +
                "END " +
                "SELECT Type FROM @ExtID R " +
                "ORDER BY R.TYPE ";
            //string SQL = "DECLARE @Regulation TABLE ([Type] nvarchar(50) Primary key, [ParentType] nvarchar(50)) " +
            //    "INSERT INTO @Regulation( Type, ParentType) " +
            //    "SELECT [Type] " +
            //    ",[ParentType] " +
            //    "FROM [dbo].[ExternalIdentifierType] T " +
            //    "WHERE T.ParentType = '" + ParentType + "' " +
            //    "WHILE (SELECT COUNT(*) FROM [dbo].[ExternalIdentifierType] T, @Regulation R  " +
            //    "WHERE T.ParentType = R.Type " +
            //    "AND T.Type NOT IN (SELECT R.Type FROM @Regulation R) ) > 0 " +
            //    "BEGIN " +
            //    "INSERT INTO @Regulation( Type, ParentType) " +
            //    "SELECT T.[Type] " +
            //    ",T.[ParentType] " +
            //    "FROM [dbo].[ExternalIdentifierType] T, @Regulation R  " +
            //    "WHERE T.ParentType = R.Type " +
            //    "AND T.Type NOT IN (SELECT R.Type FROM @Regulation R) " +
            //    "END " +
            //    "SELECT Type FROM @Regulation R " +
            //    "ORDER BY R.TYPE ";
            return SQL;
        }

        #endregion

        #endregion

    }

}
