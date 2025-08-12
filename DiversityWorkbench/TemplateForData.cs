using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench
{
    public class TemplateForDataSourceTable
    {
        private System.Data.DataTable _SourceTable;

        public System.Data.DataTable SourceTable
        {
            get { return _SourceTable; }
            //set { _SourceTable = value; }
        }
        private string _DisplayColumn;

        public string DisplayColumn
        {
            get { return _DisplayColumn; }
            //set { _DisplayColumn = value; }
        }
        private string _ValueColumn;

        public string ValueColumn
        {
            get { return _ValueColumn; }
            //set { _ValueColumn = value; }
        }

        public TemplateForDataSourceTable(System.Data.DataTable SourceTable, string DisplayColumn, string ValueColumn)
        {
            this._SourceTable = SourceTable;
            this._DisplayColumn = DisplayColumn;
            this._ValueColumn = ValueColumn;
        }

    }

    public class TemplateForData
    {
        #region Parameter

        private string _TableName;

        public string TableName
        {
            get { return _TableName; }
            //set { _TableName = value; }
        }
        
        #endregion

        #region Construction

        public TemplateForData(string TableName)
        {
            this._TableName = TableName;
            System.Collections.Generic.Dictionary<string, string> TemplateData = new Dictionary<string, string>();
            if (DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Template == null)
                DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Template = new System.Collections.Specialized.StringCollection();
            foreach (string s in DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Template)
            {
                string[] ss = s.Split(new char[] { '|' });
                if (ss[0] == this._TableName && !TemplateData.ContainsKey(ss[1]))
                {
                    TemplateData.Add(ss[1], ss[2]);
                }
            }
        }

        public TemplateForData(string TableName, System.Collections.Generic.List<string> SuppressedColumns) : this (TableName)
        {
            this._SuppressedColumns = SuppressedColumns;
        }

        public TemplateForData(string TableName, System.Collections.Generic.List<string> SuppressedColumns, System.Collections.Generic.Dictionary<string, DiversityWorkbench.TemplateForDataSourceTable> SourceTables)
            : this(TableName, SuppressedColumns)
        {
            this._SourceTables = SourceTables;
        }

        #endregion

        #region Filling option and user decision

        public enum FillingOption { OnlyEmpty, UserSelection, All }

        private static FillingOption _OptionForFilling;

        public static FillingOption OptionForFilling
        {
            get
            {
                if (DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.FillingOption == DiversityWorkbench.TemplateForData.FillingOption.UserSelection.ToString())
                {
                    _OptionForFilling = FillingOption.UserSelection;
                    return TemplateForData.FillingOption.UserSelection;
                }
                else if (DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.FillingOption == DiversityWorkbench.TemplateForData.FillingOption.OnlyEmpty.ToString())
                {
                    _OptionForFilling = FillingOption.OnlyEmpty;
                    return TemplateForData.FillingOption.OnlyEmpty;
                }
                else
                {
                    _OptionForFilling = FillingOption.All;
                    return TemplateForData.FillingOption.All;
                }

                //return TemplateForData._OptionForFilling; 
            }
            set
            {
                TemplateForData._OptionForFilling = value;
                DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.FillingOption = TemplateForData._OptionForFilling.ToString();
                DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Save();
            }
        }
        
        public bool UserDecisionNeeded(System.Data.DataRow R)
        {
            if (OptionForFilling != FillingOption.UserSelection)
                return false;

            bool UserMustDecide = false;
            foreach (System.Data.DataColumn C in R.Table.Columns)
            {
                if (this.ColumnTemplateValues().ContainsKey(C.ColumnName))
                {
                    if (R[C.ColumnName].ToString() != this.ColumnTemplateValues()[C.ColumnName])
                    {
                        UserMustDecide = true;
                        break;
                    }
                }
            }
            return UserMustDecide;
        }

        #endregion

        public void CopyTemplateToRow(System.Data.DataRow R)
        {
            if (this.UserDecisionNeeded(R))
            {
                DiversityWorkbench.Forms.FormTemplateEditor f = new Forms.FormTemplateEditor(this, R);
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    System.Collections.Generic.List<string> L = f.SelectedColumns();
                    R.BeginEdit();
                    foreach (string C in L)
                    {
                        string x = this.ColumnTemplateValues()[C];
                        R[C] = this.ColumnTemplateValues()[C];
                    }
                    R.EndEdit();
                }
            }
            else if (OptionForFilling == FillingOption.OnlyEmpty)
            {
                R.BeginEdit();
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ColumnTemplateValues())
                {
                    if (R[KV.Key].Equals(System.DBNull.Value) || R[KV.Key].ToString().Length == 0)
                    {
                        string x = R.Table.Columns[KV.Key].DataType.ToString();
                        if (R.Table.Columns[KV.Key].DataType.Name.ToLower() == "double"
                            || R.Table.Columns[KV.Key].DataType.Name.ToLower() == "single")
                        {
                            if (KV.Value.Trim().Replace(".", "").Length > 0)
                            {
                                switch (R.Table.Columns[KV.Key].DataType.Name.ToLower())
                                {
                                    case "double":
                                        Double D;
                                        if (Double.TryParse(KV.Value, out D))
                                            R[KV.Key] = D;
                                        else
                                        {
                                            System.Windows.Forms.MessageBox.Show("The template value\r\n" + KV.Value + "\r\nis not a valid " + KV.Key);
                                        }
                                        break;
                                    case "single":
                                        Single S;
                                        if (Single.TryParse(KV.Value, out S))
                                            R[KV.Key] = S;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            R[KV.Key] = KV.Value;
                        }
                    }
                }
                R.EndEdit();
            }
            else
            {
                R.BeginEdit();
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ColumnTemplateValues())
                {
                    R[KV.Key] = KV.Value;
                }
                R.EndEdit();
            }
        }

        private System.Collections.Generic.List<string> _SuppressedColumns;
        public void setSuppressedColumns(System.Collections.Generic.List<string> SuppressedColumns)
        {
            this._SuppressedColumns = SuppressedColumns;
        }

        private System.Collections.Generic.Dictionary<string, TemplateForDataSourceTable> _SourceTables;
        public void setSourceTables(System.Collections.Generic.Dictionary<string, TemplateForDataSourceTable> SourceTables)
        {
            this._SourceTables = SourceTables;
        }

        public System.Collections.Generic.Dictionary<string, TemplateForDataSourceTable> SourceTables()
        {
            return this._SourceTables;
        }

        private System.Collections.Generic.Dictionary<string, string> _ColumnTemplateValues;
        public System.Collections.Generic.Dictionary<string, string> ColumnTemplateValues()
        {
            if (this._ColumnTemplateValues == null)
            {
                this._ColumnTemplateValues = new Dictionary<string, string>();
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.TableColums())
                {
                    string TemplateColumn = this.TableName + "|" + KV.Key + "|";
                    foreach (string s in DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Template)
                    {
                        if (s.StartsWith(TemplateColumn) && !this._ColumnTemplateValues.ContainsKey(KV.Key))
                            this._ColumnTemplateValues.Add(KV.Key, s.Substring(TemplateColumn.Length));
                    }
                }

            }
            return this._ColumnTemplateValues;
        }

        #region Infos from the database
        
        private System.Collections.Generic.Dictionary<string, string> _TableColums;

        public System.Collections.Generic.Dictionary<string, string> TableColums()
        {
            if (this._TableColums == null)
            {
                this._TableColums = new Dictionary<string, string>();
                string SQL = "SELECT C.COLUMN_NAME, C.DATA_TYPE " +
                    "FROM INFORMATION_SCHEMA.COLUMNS C  " +
                    "WHERE C.TABLE_NAME = '" + this._TableName + "' " +
                    "AND C.COLUMN_NAME NOT LIKE 'Log%When' " +
                    "AND C.COLUMN_NAME NOT LIKE 'Log%By' " +
                    "AND C.COLUMN_NAME <> 'RowGUID' " +
                    "AND C.COLUMN_NAME NOT LIKE 'xx%' " +
                    "ORDER BY C.ORDINAL_POSITION";
                System.Data.DataTable dt = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    //if (this.PrimaryKeyColumnList.Contains(R[0].ToString()))
                    //    continue;
                    if (this._SuppressedColumns != null
                        && this._SuppressedColumns.Contains(R[0].ToString()))
                        continue;
                    this._TableColums.Add(R[0].ToString(), R[1].ToString());
                }
            }
            return this._TableColums;
        }

        private System.Collections.Generic.List<string> _PrimaryKeyColumns;
        /// <summary>
        /// the list of the primary key columns
        /// </summary>
        public System.Collections.Generic.List<string> PrimaryKeyColumnList
        {
            get
            {
                if (this._PrimaryKeyColumns == null)
                {
                    this._PrimaryKeyColumns = new List<string>();
                    try
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();
                        string SQL = "SELECT COLUMN_NAME " +
                            "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                            "WHERE (TABLE_NAME = '" + this._TableName + "') AND (EXISTS " +
                            "(SELECT * " +
                            "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                            "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS T ON K.CONSTRAINT_NAME = T.CONSTRAINT_NAME " +
                            "WHERE (T.CONSTRAINT_TYPE = 'PRIMARY KEY') AND (K.COLUMN_NAME = C.COLUMN_NAME) AND (K.TABLE_NAME = C.TABLE_NAME)))";
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                            this._PrimaryKeyColumns.Add(R[0].ToString());
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                return this._PrimaryKeyColumns;
            }
        }

        private System.Collections.Generic.Dictionary<string, string> _ColumsWithForeignRelations;
        private System.Collections.Generic.Dictionary<string, string> _ForeignColums;

        public System.Collections.Generic.Dictionary<string, string> ColumsWithForeignRelations
        {
            get
            {
                if (this._ColumsWithForeignRelations == null)
                {
                    this._ColumsWithForeignRelations = new Dictionary<string, string>();
                    string SQL = "SELECT DISTINCT FK.COLUMN_NAME AS ColumnName, P.TABLE_NAME AS ForeignTable, PP.COLUMN_NAME AS ForeignColumn " +
                    "FROM  INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PP INNER JOIN " +
                    "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF INNER JOIN " +
                    "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK ON TF.CONSTRAINT_NAME = FK.CONSTRAINT_NAME INNER JOIN " +
                    "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PK INNER JOIN " +
                    "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TPK ON PK.CONSTRAINT_NAME = TPK.CONSTRAINT_NAME ON " +
                    "FK.COLUMN_NAME = PK.COLUMN_NAME INNER JOIN " +
                    "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON FK.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
                    "INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME ON PP.TABLE_NAME = P.TABLE_NAME AND  " +
                    "PP.CONSTRAINT_NAME = P.CONSTRAINT_NAME AND PP.ORDINAL_POSITION = FK.ORDINAL_POSITION " +
                    "WHERE (TF.CONSTRAINT_TYPE = 'FOREIGN KEY') AND " +
                    "(TF.TABLE_NAME = '" + this._TableName + "') AND (TPK.TABLE_NAME = '" + this._TableName + "')";
                    try
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            try
                            {
                                this._ColumsWithForeignRelations.Add(R[0].ToString(), R[1].ToString());
                                if (this._ForeignColums == null)
                                    this._ForeignColums = new Dictionary<string, string>();
                                this._ForeignColums.Add(R[0].ToString(), R[2].ToString());
                            }
                            catch (System.Exception ex)
                            {
                            }
                            //string Column = R[0].ToString();
                            //if (this._ColumsWithForeignRelations[R[0].ToString()].ForeignRelationTable == this.ParentTableAlias)
                            //{
                            //    /// Werte bereits vergeben, e.g. bei IdentificationUnit wo es 2 Schluessel fuer die Spalte CollectionSpecimenID gibt. 
                            //    /// Hier soll nur dann was eingetragen werden, wenn die Tabelle dem Parent entspricht und dieses nicht mehr ueberschrieben werden
                            //}
                            //else if (this._DataColumns[R[0].ToString()].ForeignRelationColumn != null && this._DataColumns[R[0].ToString()].ForeignRelationTable != null)
                            //{
                            //    string CurrentForeignTable = this._DataColumns[R[0].ToString()].ForeignRelationTable;
                            //    string NewForeingTable = R["ForeignTable"].ToString();
                            //    ///TODO: Entscheiden war hier passieren soll. 2 Schluessel verweisen auf 2 Tabellen, welche nehmen?
                            //}
                            //else
                            //{
                            //    this._DataColumns[R[0].ToString()].ForeignRelationColumn = R["ForeignColumn"].ToString();
                            //    this._DataColumns[R[0].ToString()].ForeignRelationTable = R["ForeignTable"].ToString();
                            //}
                        }
                    }
                    catch (System.Exception ex) { }

                }
                return _ColumsWithForeignRelations;
            }
            //set { _ColumsWithForeignRelations = value; }
        }
        
        #endregion

        #region ForeignSources

        private System.Collections.Generic.Dictionary<string, System.Data.DataTable> _ForeignSources;

        private System.Collections.Generic.Dictionary<string, System.Data.DataTable> ForeignSources
        {
            get 
            {
                if (this._ForeignSources == null)
                {
                    this._ForeignSources = new Dictionary<string, System.Data.DataTable>();
                }
                return _ForeignSources; 
            }
            //set { _ForeignSources = value; }
        }

        public System.Data.DataTable ForeignSource(string TableName, ref string DisplayColumn)
        {
            return this.ForeignSources[TableName];
        }

        #endregion

    }
}
