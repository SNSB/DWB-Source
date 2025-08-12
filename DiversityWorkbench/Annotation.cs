using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench
{

    public interface AnnotationProvider
    {
        System.Data.DataTable AnnotationTable();
        //void UpdateAnnotationTable();
    }

    public struct AnnotationLink
    {
        /// <summary>
        /// the name of the datacolumn on which the annotation refers
        /// </summary>
        public string ReferencedTable;
        /// <summary>
        /// the name of the column containing the ID of the dataset on which the annotation refers
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

    public class Annotation
    {

        #region Parameter

        public static string SqlAnnotation = "SELECT AnnotationID, ReferencedAnnotationID, AnnotationType, Title, Annotation, URI, ReferenceDisplayText, ReferenceURI, SourceDisplayText, SourceURI, IsInternal, " +
            "ReferencedID, ReferencedTable, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy " +
            "FROM Annotation";

        private System.Data.DataTable _dtReferencedTables;
        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.AnnotationLink> _AnnotationLinks;
        ///// <summary>
        ///// the name of the datacolumn on which the annotation refers
        ///// </summary>
        //private string _ReferencedTable;
        ///// <summary>
        ///// the name of the column containing the ID of the dataset on which the annotation refers
        ///// </summary>
        //private string _ReferencedColumn;
        ///// <summary>
        ///// the name of the table containing the referenced column and the linked column
        ///// </summary>
        //private string _LinkTable;
        ///// <summary>
        /////// the name of the column in the main table that is used for the query
        ///// </summary>
        //private string _LinkedColumn;
        ///// <summary>
        ///// the display text shown in a user interface, e.g. the name of the table
        ///// </summary>
        //private string _DisplayText;
        
        #endregion

        #region Construction

        public Annotation()
        {
        }

        public Annotation(
            //string ReferencedTable, 
            //string ReferencedColumn, 
            //string LinkTable, 
            //string LinkedColumn, 
            //string DisplayText,
            System.Collections.Generic.Dictionary<string, DiversityWorkbench.AnnotationLink> AnnotationLinks)
        {
            //this._DisplayText = DisplayText;
            //this._LinkedColumn = LinkedColumn;
            //this._LinkTable = LinkTable;
            //this._ReferencedColumn = ReferencedColumn;
            //this._ReferencedTable = ReferencedTable;
            this._AnnotationLinks = AnnotationLinks;
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Where clause for query
        /// </summary>
        /// <param name="ReferencedTable">The name of the refered table</param>
        /// <param name="Restriction">The restriction of the query</param>
        /// <param name="AnnotationType">The selected type of the annotation</param>
        /// <returns></returns>
        public string WhereClause(string ReferencedTable, string Column, string QueryOperator, string Restriction, string AnnotationType)
        {
            string SQL = "";
            try
            {
                if (Restriction.Length == 0 && QueryOperator != "•")
                    return "";

                if (ReferencedTable.Length > 0)
                {
                    SQL = "SELECT T." + this._AnnotationLinks[ReferencedTable].LinkedColumn +
                    " FROM Annotation AS A, " +
                    this._AnnotationLinks[ReferencedTable].LinkTable + " AS T " +
                    "WHERE A.ReferencedID = T." + this._AnnotationLinks[ReferencedTable].ReferencedColumn +
                    " AND A.ReferencedTable = N'" + this._AnnotationLinks[ReferencedTable].ReferencedTable + "'";
                    if (AnnotationType.Length > 0)
                    {
                        SQL += " AND A.AnnotationType = '" + AnnotationType + "'";
                        if (AnnotationType == "Reference")
                            SQL += " AND A." + this.Restriction("ReferenceDisplayText", QueryOperator, Restriction);
                        else
                            SQL += " AND A." + this.Restriction(Column, QueryOperator, Restriction);
                    }
                    else
                        SQL += " AND A." + this.Restriction(Column, QueryOperator, Restriction);
                }
                else
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, AnnotationLink> KV in this._AnnotationLinks)
                    {
                        if (SQL.Length > 0)
                            SQL += " UNION ";
                        SQL += "SELECT T." + KV.Value.LinkedColumn +
                        " FROM Annotation AS A, " +
                        KV.Value.LinkTable + " AS T " +
                        "WHERE A.ReferencedID = T." + KV.Value.ReferencedColumn +
                        " AND A.ReferencedTable = N'" + KV.Value.ReferencedTable + "'";
                        if (AnnotationType.Length > 0)
                        {
                            SQL += " AND A.AnnotationType = '" + AnnotationType + "'";
                            if (AnnotationType == "Reference")
                                SQL += " AND A." + this.Restriction("ReferenceDisplayText", QueryOperator, Restriction);
                            else
                                SQL += " AND A." + this.Restriction(Column, QueryOperator, Restriction);
                        }
                        else
                            SQL += " AND A." + this.Restriction(Column, QueryOperator, Restriction);
                    }
                }
            }
            catch { }
            return SQL;
        }

        private string Restriction(string Column, string QueryOperator, string Restriction)
        {
            string SQL = Column;
            switch (QueryOperator)
            {
                case "~":
                    SQL += " LIKE '" + Restriction + "%'";
                    break;
                case "=":
                    SQL += " = '" + Restriction + "'";
                    break;
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
                    if (this._AnnotationLinks != null)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, AnnotationLink> KV in this._AnnotationLinks)
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
        
        #endregion

    }
}
