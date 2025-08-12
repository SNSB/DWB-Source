using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DWBServices.WebServices;

namespace DiversityWorkbench.Spreadsheet
{
    public class Sheet
    {

        #region Properties

        #region Master

        private Spreadsheet.DataColumn _MasterQueryColumn;

        public Spreadsheet.DataColumn MasterQueryColumn
        {
            get { return _MasterQueryColumn; }
            set
            {
                _MasterQueryColumn = value;
                this._MasterTableAlias = _MasterQueryColumn.DataTable().Alias();
            }
        }

        private string _MasterTableAlias;

        private DiversityWorkbench.Spreadsheet.iMainForm _iMainForm;
        public void setInterfaceMainForm(DiversityWorkbench.Spreadsheet.iMainForm iMainForm)
        {
            this._iMainForm = iMainForm;
        }

        public DiversityWorkbench.Spreadsheet.iMainForm iMainForm
        {
            get
            {
                //DiversityWorkbench.Spreadsheet.iMainForm Interface = this._iMainForm;
                return this._iMainForm;
            }
        }


        #region Static version to support threading

        private static DiversityWorkbench.Spreadsheet.iMainForm _iStaticMainForm;
        public static void setStaticInterfaceMainForm(DiversityWorkbench.Spreadsheet.iMainForm iMainForm)
        {
            _iStaticMainForm = iMainForm;
        }

        public static DiversityWorkbench.Spreadsheet.iMainForm iStaticMainForm
        {
            get
            {
                return _iStaticMainForm;
            }
        }

        #endregion

        #endregion

        #region Connection

        private Microsoft.Data.SqlClient.SqlConnection _Connection;
        private Microsoft.Data.SqlClient.SqlConnection Connection()
        {
            if (this._Connection == null)
                this._Connection = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
            return this._Connection;
        }
        public void ResetConnection()
        {
            this._Connection = null;
        }

        #endregion

        public static bool RebuildNeeded = false;

        private string _SQL = "";

        public string SQL(QueryType Type)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            bool OK = true;
            this.getData(QueryType.SQL, ref dt, ref OK);
            return this._SQL;
        }

        #region DataTables

        private System.Collections.Generic.SortedDictionary<string, DataTable> _DataTables;
        public System.Collections.Generic.SortedDictionary<string, DataTable> DataTables()
        {
            if (this._DataTables == null)
                this._DataTables = new SortedDictionary<string, DataTable>();
            return this._DataTables;
        }

        public void AddDataTable(DataTable Table)
        {
            if (this._DataTables == null)
                this._DataTables = new SortedDictionary<string, DataTable>();
            if (!this._DataTables.ContainsKey(Table.Alias()))
            {
                if (Table.Alias().Length == 0)
                { }
                else
                    this._DataTables.Add(Table.Alias(), Table);
            }
        }

        public void RemoveDataTable(string Alias)
        {
            if (this._DataTables.ContainsKey(Alias) && this._DataTables[Alias].Type() == DataTable.TableType.Parallel)
            {
                this._DataTables.Remove(Alias);
            }
        }

        public System.Collections.Generic.Dictionary<string, string> MissingTables(bool IncludeProject)
        {
            System.Collections.Generic.Dictionary<string, string> Missing = new Dictionary<string, string>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Spreadsheet.DataTable> KV in this.DataTables())
            {
                if (!IncludeProject && KV.Value.Type() == DataTable.TableType.Project)
                    continue;
                Missing.Add(KV.Key, KV.Value.DisplayText);
            }
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this.SelectedColumns())
            {
                if (Missing.ContainsKey(DC.Value.DataTable().Alias()))
                    Missing.Remove(DC.Value.DataTable().Alias());
            }
            return Missing;
        }

        public System.Collections.Generic.Dictionary<string, string> TablesToAdd()
        {
            System.Collections.Generic.Dictionary<string, string> ToAdd = new Dictionary<string, string>();
            try
            {
                // getting all possible tables that may be added
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Spreadsheet.DataTable> KV in this.DataTables())
                {
                    if (KV.Value.Type() == DataTable.TableType.Project)
                        continue;
                    if (KV.Value.Type() == DataTable.TableType.Parallel)
                    {
                        if (KV.Value.Alias() == KV.Value.TemplateAlias)
                            ToAdd.Add(KV.Key, KV.Value.Description());
                    }
                    else
                    {
                        ToAdd.Add(KV.Key, KV.Value.Description());
                    }
                }
                // removing those that are already present and not parallel
                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this.SelectedColumns())
                {
                    if (ToAdd.ContainsKey(DC.Value.DataTable().Alias())
                        && DC.Value.DataTable().Type() != DataTable.TableType.Parallel
                        && DC.Value.Type() == DataColumn.ColumnType.Data
                        && DC.Value.IsVisible)
                    {
                        string Alias = DC.Value.DataTable().Alias();
                        ToAdd.Remove(DC.Value.DataTable().Alias());
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return ToAdd;
        }


        #endregion

        #region SelectedColumns

        private System.Collections.Generic.Dictionary<int, DataColumn> _SelectedColumns;
        /// <summary>
        /// The colums selected in a SQL query including spacers and operation columns
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.Dictionary<int, DataColumn> SelectedColumns()
        {
            if (this._SelectedColumns == null)
            {
                this._SelectedColumns = new Dictionary<int, DataColumn>();

                // the position of the selected column including spacers and operation columns
                int iPosition = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, DataTable> KV in this._DataTables)
                {
                    try
                    {
                        if (KV.Value.Type() != DataTable.TableType.Root && KV.Value.ParentTable() == null)
                        {
                            continue;
                        }
                        if (KV.Value.DisplayedColumns().Count == 0
                            && KV.Value.Type() != DataTable.TableType.Root
                            && KV.Value.Type() != DataTable.TableType.Target
                            //&& KV.Value.Type() != DataTable.TableType.Project
                            )
                            continue;
                        bool AnyColumnVisible = false;
                        foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in KV.Value.DataColumns())
                        {
                            if (DC.Value.IsVisible && !DC.Value.IsHidden)
                            {
                                AnyColumnVisible = true;
                                break;
                            }
                            // Markus 20.5.2019 - ensure inclusion of PK of Root etc. table
                            //else if ((KV.Value.Type() == DataTable.TableType.Root
                            //    || KV.Value.Type() == DataTable.TableType.Target)
                            //    && DC.Value.IsVisible && DC.Value.IsHidden
                            //    && DC.Value.DataTable().PrimaryKeyColumnList.Contains(DC.Key))
                            //{
                            //    AnyColumnVisible = true;
                            //    break;
                            //}

                        }
                        if (!AnyColumnVisible)
                        {
                            continue;
                        }
                        if (KV.Value.Type() != DataTable.TableType.Project)
                        {
                            // spacer
                            Data.Column Cspacer = new Data.Column("|", (Data.Table)KV.Value);
                            DataColumn DCspacer = new DataColumn(KV.Value, Cspacer);
                            DCspacer.setColumnType(DataColumn.ColumnType.Spacer);
                            if (KV.Value.DisplayedColumns().Count == 0)
                                DCspacer.IsVisible = false;
                            this.AddSelectedColumn(iPosition, DCspacer);
                            iPosition++;

                            // Operation Delete
                            Data.Column Cdel = new Data.Column("x", (Data.Table)KV.Value);
                            DataColumn DCdel = new DataColumn(KV.Value, Cdel);
                            DCdel.setColumnType(DataColumn.ColumnType.Operation);
                            if (KV.Value.DisplayedColumns().Count == 0)
                                DCdel.IsVisible = false;
                            this.AddSelectedColumn(iPosition, DCdel);
                            iPosition++;

                            // PK
                            foreach (string C in KV.Value.PrimaryKeyColumnList)
                            {
                                if (!KV.Value.DisplayedColumns().Contains(C))
                                    KV.Value.DataColumns()[C].IsVisible = false;
                                this.AddSelectedColumn(iPosition, KV.Value.DataColumns()[C]);
                                iPosition++;
                            }

                            // Relations to parent table
                            if (KV.Value.ParentTable() != null)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in KV.Value.DataColumns())
                                {
                                    if (!KV.Value.PrimaryKeyColumnList.Contains(DC.Key) &&
                                        DC.Value.Column.ForeignRelations.ContainsKey(KV.Value.ParentTable().Name))
                                    {
                                        if (!KV.Value.DisplayedColumns().Contains(DC.Key))
                                            KV.Value.DataColumns()[DC.Key].IsVisible = false;
                                        this.AddSelectedColumn(iPosition, KV.Value.DataColumns()[DC.Key]);
                                        iPosition++;
                                    }
                                }
                            }
                        }
                        else if (KV.Value.Type() == DataTable.TableType.Project)
                        {
                        }

                        if (KV.Value.DisplayedColumns() != null)
                        {
                            foreach (string C in KV.Value.DisplayedColumns())
                            {
                                if (KV.Value.PrimaryKeyColumnList.Contains(C))
                                {
                                    continue;
                                }
                                KV.Value.DataColumns()[C].IsVisible = true;
                                if (KV.Value.DataColumns()[C].Column.ForeignRelations != null &&
                                    KV.Value.DataColumns()[C].Column.ForeignRelations.Count > 0 &&
                                    KV.Value.ParentTable() != null &&
                                    KV.Value.DataColumns()[C].Column.ForeignRelations.ContainsKey(KV.Value.ParentTable().Name))
                                    continue;
                                this.AddSelectedColumn(iPosition, KV.Value.DataColumns()[C]);
                                iPosition++;
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }

            }
            return this._SelectedColumns;
        }

        public void ResetSelectedColumns()
        {
            this._SelectedColumns = null;
            this.resetOrderColumn();
        }

        private void AddSelectedColumn(int Position, DataColumn Column)
        {
            if (!this._SelectedColumns.ContainsKey(Position))
                this._SelectedColumns.Add(Position, Column);
            else
            {
            }
        }

        #endregion

        #region MaxColumnWidth

        private int _MaxColumnWidth = 140;
        public int MaxColumnWidth() { return this._MaxColumnWidth; }
        public void setMaxColumnWidth(int Max)
        {
            if (Max > DataColumn.OperationWidth())
                this._MaxColumnWidth = Max;
        }

        #endregion

        #region Display text

        private string _DisplayText;
        public string DisplayText()
        {
            return DiversityWorkbench.Settings.DatabaseName + ": " + this.TargetDisplayText();
        }

        #endregion

        #region Target

        private string _Target;
        public string Target() { return this._Target; }
        public string TargetDisplayText()
        {
            string Target = this.Target();
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DataTable> DT in this.DataTables())
                {
                    if (DT.Value.Type() == DataTable.TableType.Target)
                    {
                        Target = DT.Value.DisplayText;
                        break;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return Target;
        }

        #endregion

        #region Project

        private System.Data.DataTable _DtProjects;
        private string _SqlProjectSource;
        public void setProjectSqlSoure(string SQL)
        {
            this._SqlProjectSource = SQL;
        }

        public System.Data.DataTable DtProjects()
        {
            if (this._DtProjects == null && this._SqlProjectSource != null)
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(this._SqlProjectSource, DiversityWorkbench.Settings.ConnectionString);
                this._DtProjects = new System.Data.DataTable();
                ad.Fill(this._DtProjects);
            }
            return this._DtProjects;
        }

        private System.Data.DataTable _DtProjectsReadOnly;
        private string _SqlProjectReadOnlySource;
        public void setProjectReadOnlySqlSoure(string SQL)
        {
            this._SqlProjectReadOnlySource = SQL;
        }

        public bool HasProjects()
        {
            if (this.DtProjects() != null && this.DtProjects().Rows.Count > 0)
                return true;
            else
                return false;
        }

        private bool _ProjectReadOnly = false;
        public bool ProjectReadOnly
        {
            get
            {
                return this._ProjectReadOnly;
            }
            set
            {
                this._ProjectReadOnly = value;
                if (value)
                    this._ReadOnly = true;
            }
        }

        public System.Data.DataTable DtProjectsReadOnly()
        {
            if (this._DtProjectsReadOnly == null)
            {
                this._DtProjectsReadOnly = new System.Data.DataTable();
                if (this._SqlProjectReadOnlySource != null && this._SqlProjectReadOnlySource.Length > 0)
                {
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(this._SqlProjectReadOnlySource, DiversityWorkbench.Settings.ConnectionString);
                    try
                    {
                        ad.Fill(this._DtProjectsReadOnly);
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
            return this._DtProjectsReadOnly;
        }

        public bool HasReadOnlyProjects()
        {
            if (this.DtProjectsReadOnly().Rows.Count > 0)
                return true;
            else
                return false;
        }

        public void ResetProjects()//System.Data.DataTable DT)
        {
            try
            {
                this._DtProjects = null;// DT;
                this._DtProjectsReadOnly = null;
            }
            catch (System.Exception ex)
            {
            }
        }


        private int _ProjectID = 0;
        public int ProjectID() { return this._ProjectID; }
        public void setProjectID(int ID) { this._ProjectID = ID; }

        #endregion

        #region Max results

        private int _MaxResults = 50;
        public int MaxResult() { return this._MaxResults; }
        public void setMaxResult(int Max)
        {
            if (Max > -1)
                this._MaxResults = Max;
            this._Offset = 0;
        }

        #endregion

        #region TotalCount

        private int? _TotalCount;
        public int TotalCount()
        {
            if (this._TotalCount == null)
            {
                try
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    bool OK = true;
                    this._TotalCount = this.getData(QueryType.TotalCount, ref dt, ref OK);
#if DEBUG
                    //int CheckCount = this.getData(QueryType.CheckForDuplication, ref dt, ref OK);
                    //if (this._TotalCount != CheckCount)
                    //{
                    //    this.getData(QueryType.SQL, ref dt, ref OK);
                    //    DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Query result is not unique - SQL:", this._SQL, true);
                    //}
#endif
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            return (int)this._TotalCount;
        }

        public void ResetTotalCount()
        {
            this._TotalCount = null;
        }

        #endregion

        #region Read only

        private bool _ReadOnly = false;

        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set { _ReadOnly = value; }
        }

        #endregion

        #endregion

        #region Construction

        public Sheet(string DisplayText, string Target)
        {
            this._DisplayText = DisplayText;
            this._Target = Target;
            this._MasterQueryColumn = MasterQueryColumn;

            if (DiversityWorkbench.Settings.ConnectionString.Length == 0)
            {
                DiversityWorkbench.Forms.FormConnectToDatabase f = new DiversityWorkbench.Forms.FormConnectToDatabase();
                f.setHelpProviderNameSpace(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace(), "Login");
                f.ShowDialog();
            }

        }
        //private Sheet()
        //{
        //}

        //private static Sheet _Sheet;
        //public static Sheet GetSheet(string DisplayText, string Target)
        //{
        //    if (Sheet._Sheet == null)
        //    {
        //        Sheet._Sheet = new Sheet();
        //        Sheet._Sheet._DisplayText = DisplayText;
        //        Sheet._Sheet._Target = Target;
        //    }
        //    return Sheet._Sheet;
        //}

        //public static void ResetSheet() { Sheet._Sheet = null; }

        #endregion

        #region Data

        #region Offset

        private int _Offset = 0;
        public void ResetOffset() { this._Offset = 0; }
        public void IncreaseOffset()
        {
            if (this._Offset + this._MaxResults < this._TotalCount)
                this._Offset += this._MaxResults;
        }
        public void ReduceOffset()
        {
            this._Offset -= this._MaxResults;
            if (this._Offset < 0)
                this._Offset = 0;
        }
        public int Offset() { return this._Offset; }

        #endregion

        private System.Data.DataTable _DT;
        public System.Data.DataTable DT()
        {
            if (this._DT == null)
                this._DT = new System.Data.DataTable(this._Target);
            return this._DT;
        }
        public void SetDT(System.Data.DataTable DT)
        {
            this._DT = DT;
        }

        #region Order by

        private System.Collections.Generic.Dictionary<string, DataColumn.OrderByDirection> _OrderDictionary;
        private System.Collections.Generic.SortedDictionary<int, string> _OrderColumnSequence;

        public void SetOrderColumn(string ColumnDisplayText, DataColumn.OrderByDirection Direction)
        {
            if (this._OrderDictionary == null)
            {
                this._OrderDictionary = new Dictionary<string, DataColumn.OrderByDirection>();
            }
            if (!this._OrderDictionary.ContainsKey(ColumnDisplayText))
            {
                if (Direction != DataColumn.OrderByDirection.none)
                {
                    this._OrderDictionary.Add(ColumnDisplayText, Direction);
                    //this.SetOrderSequence(this._OrderDictionary.Count, TableAliasColumnName);
                }
            }
            else
            {
                if (Direction == DataColumn.OrderByDirection.none)
                {
                    this._OrderDictionary.Remove(ColumnDisplayText);
                    //this.SetOrderSequence(null, TableAliasColumnName);
                }
                else
                {
                    this._OrderDictionary[ColumnDisplayText] = Direction;
                    //this.SetOrderSequence(this._OrderDictionary.Count, TableAliasColumnName);
                }
            }
        }

        public int? SetOrderSequence(int? Sequence, string ColumnDisplayText)
        {
            if (this._OrderColumnSequence == null)
            {
                this._OrderColumnSequence = new SortedDictionary<int, string>();
            }

            // Remove previous entries for this column
            System.Collections.Generic.List<int> iRemove = new List<int>();
            foreach (System.Collections.Generic.KeyValuePair<int, string> KV in this._OrderColumnSequence)
            {
                if (ColumnDisplayText == KV.Value)
                {
                    iRemove.Add(KV.Key);
                }
            }
            foreach (int ii in iRemove)
                this._OrderColumnSequence.Remove(ii);

            // insert new sequence
            if (Sequence != null)
            {
                int i = (int)Sequence;
                while (this._OrderColumnSequence.ContainsKey(i))
                    i++;
                this._OrderColumnSequence.Add(i, ColumnDisplayText);
                return i;
            }
            else return null;
        }


        private string _OrderColumn;
        public void setOrderColumn(string ColumnName)
        {
            this._OrderColumn = ColumnName;
        }
        public string getOrderColumn() { return this._OrderColumn; }

        public void resetOrderColumn() { this._OrderColumn = null; }

        public enum OrderDirection { Desc, Asc }
        private OrderDirection _OrderDirection = OrderDirection.Asc;
        public void setOrderDirection(OrderDirection Direction)
        {
            this._OrderDirection = Direction;
        }
        public OrderDirection getOrderDirection() { return this._OrderDirection; }

        #endregion

        public bool getDataStructure(ref System.Data.DataTable T, QueryType Type)
        {
            bool OK = true;
            this.getData(Type, ref T, ref OK);
            return OK;// dt;
        }

        public bool getDataDistinctContentForMapSymbols(ref System.Data.DataTable T)
        {
            bool OK = true;
            if (this.GeographySymbolSourceColumn.Length > 0 && this.GeographySymbolSourceTable.Length > 0)
            {
                string SQL = "SELECT DISTINCT " + this.GeographySymbolSourceColumn + " FROM " + this.GeographySymbolSourceTable;
                if (this.GeographySymbolSourceRestriction.Length > 0)
                {
                    SQL += " WHERE " + this.GeographySymbolSourceRestriction;
                }
                SQL += " ORDER BY [" + this.GeographySymbolSourceColumn + "]";
                string Message = "";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref T, ref Message);
            }
            else
                this.getData(QueryType.DistinctContentForMapIcons, ref T, ref OK);
            return OK;// dt;
        }

        public bool getDataGeoObjectsForMapSymbols(ref System.Data.DataTable T, string SqlRestriction)
        {
            bool OK = true;
            this._SqlRestriction = SqlRestriction;
            this.getData(QueryType.GeoObjects, ref T, ref OK);
            return OK;// dt;
        }

        private string _SqlRestriction = "";

        private void DeselectUnconnectedLookupTables()
        {
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DataTable> DT in this.DataTables())
                {
                    if (DT.Value.Type() == DataTable.TableType.Lookup)
                    {
                        // Markus 20.4.2019 ensure inclusion of root tables
                        bool ParentPresent = this.ParentPresent(DT.Value);// = false;
                        //foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this.SelectedColumns())
                        //{
                        //    if (DC.Value.DataTable().Alias() == DT.Value.ParentTable().Alias())
                        //    {
                        //        ParentPresent = true;
                        //        break;
                        //    }
                        //}
                        if (!ParentPresent)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in DT.Value.DataColumns())
                                DC.Value.IsVisible = false;
                            this.ResetSelectedColumns();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public System.Data.DataTable getData()
        {
            bool OK = true;
            this._DT = new System.Data.DataTable();
            this.getData(QueryType.Data, ref this._DT, ref OK);
            if (!this.ReadOnly)
                this.ResetTableReadOnlyRows();
            return this._DT;
        }

        public enum QueryType { Data, Adding, Filter, TotalCount, DistinctContentForMapIcons, GeoObjects, SQL, CheckForDuplication }

        private int getData(QueryType Type, ref System.Data.DataTable T, ref bool OK)
        {
            try
            {
                this.DeselectUnconnectedLookupTables();
                if (this.getColumnClause(Type).Length == 0)
                {
                    if (Type == QueryType.Data)// Count > 0)
                        System.Windows.Forms.MessageBox.Show("No columns selected");
                    return 0;
                }

                this._TableNullRows = null;
                string SqlTop = "SELECT "; //TOP ";
                if (Type != QueryType.CheckForDuplication)
                    SqlTop += " DISTINCT ";
                switch (Type)
                {
                    case QueryType.Data:
                        SqlTop += " TOP ";
                        if (this._Offset == 0)
                            SqlTop += this._MaxResults.ToString() + " ";
                        else
                            SqlTop += (this._Offset + this._MaxResults).ToString() + " ";
                        break;
                    case QueryType.Filter:
                    case QueryType.Adding:
                        SqlTop += " TOP 0 ";
                        break;
                    case QueryType.TotalCount:
                    case QueryType.CheckForDuplication:
                        SqlTop = "SELECT COUNT(*) ";
                        break;
                }

                string SQL = "";

                string WhereClause = this.WhereClause();// "";
                if (Type == QueryType.GeoObjects && this._SqlRestriction.Length > 0)
                {
                    if (WhereClause.Length > 0) WhereClause += " AND ";
                    else WhereClause = " WHERE ";
                    WhereClause += this._SqlRestriction;
                }

                string ColumnClause = "";
                if (Type != QueryType.TotalCount && Type != QueryType.CheckForDuplication)
                {
                    ColumnClause = this.getColumnClause(Type);
                    if (ColumnClause.Length > 0)
                    {
                        if (ColumnClause.Trim().ToUpper().StartsWith("DISTINCT ") && SQL.ToUpper().IndexOf("DISTINCT") > -1)
                            ColumnClause = ColumnClause.Replace("DISTINCT", "");
                        if (!ColumnClause.StartsWith(" "))
                            ColumnClause = " " + ColumnClause;
                        SQL += ColumnClause;
                    }
                    else
                        return 0;
                }
                if (Type == QueryType.SQL)
                    SQL += "\r\n";
                SQL += " FROM ";
                string SqlFrom = "";

                foreach (System.Collections.Generic.KeyValuePair<string, DataTable> KVtab in this._DataTables)
                {
                    try
                    {
                        if (KVtab.Value.Type() == DataTable.TableType.Lookup)
                        {
                            // Markus 20.4.2019 ensure inclusion of root tables
                            bool ParentPresent = this.ParentPresent(KVtab.Value);// false;
                                                                                 //foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this.SelectedColumns())
                                                                                 //{
                                                                                 //    if (KVtab.Value.ParentTable() != null && DC.Value.DataTable().Alias() == KVtab.Value.ParentTable().Alias())
                                                                                 //    {
                                                                                 //        ParentPresent = true;
                                                                                 //        break;
                                                                                 //    }
                                                                                 //}
                            if (!ParentPresent)
                                continue;
                        }

                        if (KVtab.Value.Type() != DataTable.TableType.Root && KVtab.Value.ParentTable() == null)
                        {
                            continue;
                        }
                        if ((KVtab.Value.Type() == DataTable.TableType.Single
                            || KVtab.Value.Type() == DataTable.TableType.InsertOnly
                            || KVtab.Value.Type() == DataTable.TableType.Parallel
                            || KVtab.Value.Type() == DataTable.TableType.Lookup) &&
                            KVtab.Value.DisplayedColumns().Count == 0)
                        {
                            continue;
                        }

                        if (Type == QueryType.TotalCount && !WhereClause.Contains(KVtab.Key + "."))
                        {
                            if ((KVtab.Value.Type() == DataTable.TableType.Parallel ||
                                 KVtab.Value.Type() == DataTable.TableType.Single ||
                                 KVtab.Value.Type() == DataTable.TableType.InsertOnly ||
                                 KVtab.Value.Type() == DataTable.TableType.Referencing ||
                                 KVtab.Value.Type() == DataTable.TableType.Lookup)
                                 && !KVtab.Value.IsRequired)
                            {
                                continue;
                            }
                            if (this._MasterTableAlias != null &&
                                 KVtab.Key == this._MasterTableAlias &&
                                 KVtab.Value.ParentTable() != null &&
                                 !KVtab.Value.ParentTable().IsRequired
                                 && this._MasterTableAlias != KVtab.Key)
                            {
                                continue;
                            }
                        }

                        if (SqlFrom.Length > 0)
                        {
                            if (Type == QueryType.SQL)
                                SqlFrom += "\r\n";
                            if ((KVtab.Value.Type() == DataTable.TableType.Parallel ||
                                KVtab.Value.Type() == DataTable.TableType.Single ||
                                KVtab.Value.Type() == DataTable.TableType.InsertOnly ||
                                KVtab.Value.Type() == DataTable.TableType.Referencing ||
                                KVtab.Value.Type() == DataTable.TableType.Lookup)
                                && !KVtab.Value.IsRequired)
                                SqlFrom += " LEFT OUTER JOIN ";
                            else if (this._MasterTableAlias != null && KVtab.Key == this._MasterTableAlias && KVtab.Value.ParentTable() != null && !KVtab.Value.ParentTable().IsRequired)
                            {
                                if (this._MasterTableAlias == KVtab.Key)
                                    SqlFrom += " INNER JOIN ";
                                else
                                    SqlFrom += " RIGHT OUTER JOIN ";
                            }
                            else
                                SqlFrom += " INNER JOIN ";
                        }
                        if (KVtab.Value.View != null && KVtab.Value.View.Length > 0)
                            SqlFrom += "[" + KVtab.Value.View + "] AS " + KVtab.Key;
                        else
                            SqlFrom += "[" + KVtab.Value.Name + "] AS " + KVtab.Key;
                        if (KVtab.Value.ParentTable() != null)
                        {
                            try
                            {
                                SqlFrom += " ON ";
                                string SqlLink = "";
                                if (KVtab.Value.Type() != DataTable.TableType.Referencing)
                                {
                                    if (KVtab.Value.ForeignRelationColumns(KVtab.Value.ParentTable().Name).Count > 0)
                                    {
                                        foreach (System.Collections.Generic.KeyValuePair<string, string> KVcol in KVtab.Value.ForeignRelationColumns(KVtab.Value.ParentTable().Name))
                                        {
                                            if (SqlLink.Length > 0) SqlLink += " AND ";
                                            SqlLink += KVtab.Key + "." + KVcol.Key + " = " + KVtab.Value.ParentTable().Alias() + "." + KVcol.Value;
                                        }
                                    }
                                    else
                                    {
                                        if (KVtab.Value.ParentTable().ForeignRelationColumns(KVtab.Value.Name).Count > 0 && KVtab.Value.Type() == DataTable.TableType.Lookup)
                                        {
                                            foreach (System.Collections.Generic.KeyValuePair<string, string> KVcol in KVtab.Value.ParentTable().ForeignRelationColumns(KVtab.Value.Name))
                                            {
                                                if (SqlLink.Length > 0) SqlLink += " AND ";
                                                SqlLink += KVtab.Key + "." + KVcol.Key + " = " + KVtab.Value.ParentTable().Alias() + "." + KVcol.Value;
                                            }
                                        }
                                    }
                                }
                                if (KVtab.Value.SqlRestrictionClause != null && KVtab.Value.SqlRestrictionClause.Length > 0)
                                {
                                    if (KVtab.Value.Type() != DataTable.TableType.Project)
                                    {
                                        if (SqlLink.Length > 0) SqlLink += " AND ";
                                        SqlLink += KVtab.Value.SqlRestrictionClause;
                                    }
                                    else
                                    {
                                        if (!this.ProjectReadOnly)
                                        {
                                            if (SqlLink.Length > 0) SqlLink += " AND ";
                                            SqlLink += KVtab.Value.SqlRestrictionClause;
                                        }
                                    }
                                }
                                if (KVtab.Value.RestrictionColumns != null)
                                {
                                    foreach (string R in KVtab.Value.RestrictionColumns)
                                    {
                                        if (KVtab.Value.DataColumns()[R].RestrictionValue != null && KVtab.Value.DataColumns()[R].RestrictionValue.Length > 0)
                                        {
                                            if (SqlLink.Length > 0) SqlLink += " AND ";
                                            SqlLink += KVtab.Key + "." + R + " = '" + KVtab.Value.DataColumns()[R].RestrictionValue + "' ";
                                        }
                                    }
                                }
                                SqlFrom += SqlLink;
                            }
                            catch (System.Exception ex)
                            {
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }

                SQL += SqlFrom;
                if (WhereClause.Length > 0)
                {
                    if (Type == QueryType.SQL)
                        SQL += "\r\n";
                    SQL += " WHERE " + WhereClause;
                }

                if (Type != QueryType.TotalCount && Type != QueryType.CheckForDuplication)
                {
                    string OrderByClause = "";

                    if (Type == QueryType.DistinctContentForMapIcons)
                    {
                        if (this.GeographySymbolTableAlias.Length > 0 && this.GeographySymbolColumn.Length > 0)
                        {
                            if (this.DataTables()[this.GeographySymbolTableAlias].DataColumns()[this.GeographySymbolColumn].SqlForColumn.Length > 0)
                                OrderByClause = " [" + this.GeographySymbolColumn + "] ";
                            else
                                OrderByClause = " [" + this.GeographySymbolTableAlias + "].[" + this.GeographySymbolColumn + "] ";
                        }
                    }
                    else
                    {
                        if (this._OrderDictionary != null &&
                        this._OrderDictionary.Count > 0 &&
                        this._OrderColumnSequence != null &&
                        this._OrderColumnSequence.Count == this._OrderDictionary.Count)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<int, string> KV in this._OrderColumnSequence)
                            {
                                // with distinct the columns for ordering must be listed in the displayed columns
                                if (ColumnClause.IndexOf(KV.Value) == -1)
                                    continue;
                                if (OrderByClause.Length > 0)
                                    OrderByClause += ", ";
                                OrderByClause += KV.Value;
                                if (this._OrderDictionary[KV.Value] == DataColumn.OrderByDirection.descending)
                                    OrderByClause += " DESC ";
                            }
                        }
                        else if (this._OrderDictionary != null &&
                            this._OrderDictionary.Count > 0 &&
                            this._OrderColumnSequence != null &&
                            this._OrderColumnSequence.Count != this._OrderDictionary.Count)
                        {
                        }

                        // MasterQueryColumn
                        if (OrderByClause.Length == 0 ||
                            OrderByClause.IndexOf("[" + this.MasterQueryColumn.DataTable().Alias() + "].[" + this.MasterQueryColumn.Name + "]") == 0)
                        {
                            string MTA = this.MasterQueryColumn.DataTable().Alias();
                            string MCN = this.MasterQueryColumn.Name;
                            string MCA = this.MasterQueryColumn.DisplayText;

                            // if Master has parent and columns of parent are shown - use PK of parent as first order column
                            if (this.MasterQueryColumn.DataTable().ParentTable() != null)
                            {
                                bool ParentIsShow = false;
                                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this.SelectedColumns())
                                {
                                    if (DC.Value.DataTable().Alias() == this.MasterQueryColumn.DataTable().ParentTable().Alias())
                                    {
                                        ParentIsShow = true;
                                        break;
                                    }
                                }
                                if (ParentIsShow)
                                {
                                    foreach (string PK in this.MasterQueryColumn.DataTable().ParentTable().PrimaryKeyColumnList)
                                    {
                                        if (Type == QueryType.SQL && !this.MasterQueryColumn.DataTable().ParentTable().DataColumns()[PK].IsVisible)
                                            continue;
                                        if (OrderByClause.Length > 0)
                                            OrderByClause += ", ";
                                        OrderByClause += "[" + this.MasterQueryColumn.DataTable().ParentTable().DataColumns()[PK].DisplayText + "]";// this.MasterQueryColumn.DataTable().ParentTable().Alias() + "." + PK;// this.MasterQueryColumn.DataTable().ParentTable().DataColumns()[PK].di;
                                    }
                                }
                            }

                            if (OrderByClause.Length > 0)
                                OrderByClause += ", ";
                            if (SqlTop.IndexOf(" DISTINCT ") > -1
                                && this.MasterQueryColumn.DisplayText.Length > 0
                                && ColumnClause.IndexOf("[" + this.MasterQueryColumn.DisplayText + "]") > -1)
                                OrderByClause += "[" + this.MasterQueryColumn.DisplayText + "]";
                            else if (ColumnClause.IndexOf("[" + this.MasterQueryColumn.DataTable().Alias() + "].[" + this.MasterQueryColumn.Name + "]") > -1)
                                OrderByClause += "[" + this.MasterQueryColumn.DataTable().Alias() + "].[" + this.MasterQueryColumn.Name + "]";
                            else if (ColumnClause.IndexOf(this.MasterQueryColumn.DataTable().Alias() + "." + this.MasterQueryColumn.Name) > -1)
                                OrderByClause += this.MasterQueryColumn.DataTable().Alias() + "." + this.MasterQueryColumn.Name;
                            else if (OrderByClause.EndsWith(", "))
                                OrderByClause = OrderByClause.Substring(0, OrderByClause.Length - 2);
                        }
                        //System.Collections.Generic.SortedDictionary<int, string> OrderDict = new SortedDictionary<int, string>();
                        foreach (System.Collections.Generic.KeyValuePair<string, DataTable> DT in this.DataTables())
                        {
                            if (DT.Value != null)
                            {
                                if (DT.Value.Type() == DataTable.TableType.Root || DT.Value.Type() == DataTable.TableType.Target)
                                {
                                    foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in DT.Value.DataColumns())
                                    {
                                        try
                                        {
                                            if (DT.Value.PrimaryKeyColumnList.Contains(DC.Key))
                                            {
                                                // with distinct the columns for ordering must be listed in the displayed columns
                                                if (ColumnClause.IndexOf("[" + DT.Key + "].[" + DC.Key + "]") == -1
                                                    && ColumnClause.IndexOf(DT.Key + "." + DC.Key) == -1
                                                    && SqlTop.IndexOf(" DISTINCT ") > -1)
                                                    continue;
                                                // allready contained in the order clause
                                                if (OrderByClause.IndexOf("[" + DT.Key + "].[" + DC.Key + "]") > -1
                                                    || OrderByClause.IndexOf(DT.Key + "." + DC.Key) > -1)
                                                    continue;
                                                if (SqlTop.IndexOf(" DISTINCT ") > -1
                                                    && DC.Value.DisplayText.Length > 0
                                                    && OrderByClause.IndexOf("[" + DC.Value.DisplayText + "]") > -1
                                                    && ColumnClause.IndexOf(" AS [" + DC.Value.DisplayText + "]") > -1)
                                                    continue;
                                                if (SqlTop.IndexOf(" DISTINCT ") > -1
                                                    && Type == QueryType.SQL
                                                    && !DC.Value.IsVisible)
                                                    continue;

                                                if (OrderByClause.Length > 0)
                                                    OrderByClause += ", ";
                                                if (SqlTop.IndexOf(" DISTINCT ") > -1
                                                    && DC.Value.DisplayText.Length > 0
                                                    && ColumnClause.IndexOf(" AS [" + DC.Value.DisplayText + "]") > -1)
                                                    OrderByClause += "[" + DC.Value.DisplayText + "]";
                                                else
                                                    OrderByClause += "[" + DT.Key + "].[" + DC.Key + "]";
                                            }
                                        }
                                        catch (System.Exception ex)
                                        {
                                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (OrderByClause.Length > 0)
                    {
                        if (Type == QueryType.SQL)
                            SQL += "\r\n";
                        SQL += " ORDER BY " + OrderByClause;
                    }
                    if (this._Offset == 0)
                    {
                        if (SqlTop.IndexOf("DISTINCT") > -1 && SQL.IndexOf("DISTINCT") > -1)
                            SqlTop = SqlTop.Replace("DISTINCT", "");
                        SQL = SqlTop + SQL;
                    }
                    else
                    {
                        string SqlEx = " except select * from (SELECT TOP " + this._Offset.ToString() + " " + SQL + ") ex";
                        SQL = "select * from (" + SqlTop + SQL + SqlEx + ") d";
                    }
                }
                if (this.Connection().State == System.Data.ConnectionState.Closed)
                    this.Connection().Open();
                if (Type != QueryType.TotalCount && Type != QueryType.CheckForDuplication)
                {
                    if (Type == QueryType.SQL)
                        this._SQL = SQL;
                    else
                    {
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.Connection());
                        C.ResetCommandTimeout();
                        C.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(C);// = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.Connection());
                        ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                        if (ad.InsertCommand != null)
                            ad.InsertCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                        try
                        {
                            if (T == null)
                                T = new System.Data.DataTable();
                            ad.Fill(T);
                            OK = true;
                        }
                        catch (System.Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
                            //System.Windows.Forms.MessageBox.Show(ex.Message + ".\r\nSettings may be corrupted.\r\nPlease try to reset to factory settings");// + ": " + SQL);
                            OK = false;
                            if (ex.Message.ToLower().IndexOf("timeout") > -1 && DiversityWorkbench.Settings.TimeoutDatabase > 0)
                            {
                                System.Windows.Forms.MessageBox.Show("The search for the data was not successful due to a\r\n\r\n\tTIMEOUT.\r\n\r\nPlease increase the timeout for database queries or set it to 0\r\n(see manual: Spreadsheet - Timeout)", "Timeout", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                            }
                            // ensure table is created
                            SQL = SQL.Replace(" WHERE ", " WHERE 1 = 0 AND ");
                            try
                            {
                                ad.SelectCommand.CommandText = SQL;
                                ad.Fill(T);
                            }
                            catch (System.Exception ex2)
                            {
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex2);
                            }
                        }
                    }
                    return 0;// dt;
                }
                else
                {
                    SQL = SqlTop + SQL;
                    //this._SQL = SQL;
                    int i = 0;
                    try
                    {
                        Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.Connection());
                        if (int.TryParse(Com.ExecuteScalar().ToString(), out i))
                            return i;
                        else
                            return 0;
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                    return i;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return 0;
        }

        /// <summary>
        /// If the table has a parent and this parent is inculded in the SQL statement
        /// </summary>
        /// <param name="Table">The table that should be checked</param>
        /// <returns>Parent included</returns>
        private bool ParentPresent(DataTable Table)
        {
            bool ParentPresent = false;
            // Checking standard tables
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this.SelectedColumns())
            {
                if (Table.ParentTable() != null && DC.Value.DataTable().Alias() == Table.ParentTable().Alias())
                {
                    ParentPresent = true;
                    break;
                }
            }
            // Markus 20.4.2019 ensure inclusion of root tables
            if (!ParentPresent)
            {
                // checking tables that may not be included in the selected columns, but are present in the FROM clause
                foreach (System.Collections.Generic.KeyValuePair<string, DataTable> DT in this.DataTables())
                {
                    if (Table.ParentTable() != null
                        && DT.Value.Alias() == Table.ParentTable().Alias()
                        && DT.Value.IsRequired
                        && DT.Value.Type() == DataTable.TableType.Root)
                    {
                        ParentPresent = true;
                        break;
                    }
                }
            }
            return ParentPresent;
        }

        public string WhereClause()
        {
            string WhereClause = "";
            foreach (System.Collections.Generic.KeyValuePair<string, DataTable> KV in this._DataTables)
            {
                if (KV.Value.Type() == DataTable.TableType.Lookup)
                {
                    // Markus 20.4.2019 ensure inclusion of root tables
                    bool ParentPresent = this.ParentPresent(KV.Value);// false;
                    //foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this.SelectedColumns())
                    //{
                    //    if (DC.Value.DataTable().Alias() == KV.Value.ParentTable().Alias())
                    //    {
                    //        ParentPresent = true;
                    //        break;
                    //    }
                    //}
                    if (!ParentPresent)
                        continue;
                }

                if (KV.Value.Type() != DataTable.TableType.Root && KV.Value.ParentTable() == null)
                {
                    continue;
                }
                if (KV.Value.DisplayedColumns().Count == 0)
                    continue;
                if (KV.Value.Type() != DataTable.TableType.Project)
                {
                    this.getWhereClause(ref WhereClause, KV.Value);
                }

                if (KV.Value.FilterOperator == "•")
                {
                    if (WhereClause.Length > 0)
                        WhereClause += " AND ";
                    WhereClause += " NOT " + KV.Key + "." + KV.Value.PrimaryKeyColumnList[0] + " IS NULL";
                }
                else if (KV.Value.FilterOperator == "Ø")
                {
                    if (WhereClause.Length > 0)
                        WhereClause += " AND ";
                    WhereClause += KV.Key + "." + KV.Value.PrimaryKeyColumnList[0] + " IS NULL";
                }
            }
            foreach (System.Collections.Generic.KeyValuePair<string, DataTable> KVtab in this._DataTables)
            {
                if (KVtab.Value.Type() == DataTable.TableType.Project)
                {
                    if (WhereClause.Length > 0)
                        WhereClause += " AND ";
                    WhereClause += KVtab.Key + ".ProjectID = " + this._ProjectID.ToString();
                }
            }
            return WhereClause;
        }

        public string FromClause(System.Collections.Generic.List<string> IncludedAliases)
        {
            string SqlFrom = "";
            foreach (System.Collections.Generic.KeyValuePair<string, DataTable> KVtab in this._DataTables)
            {
                if (KVtab.Value.Type() == DataTable.TableType.Lookup)
                {
                    // Markus 20.4.2019 ensure inclusion of root tables
                    bool ParentPresent = this.ParentPresent(KVtab.Value);// false;
                    //foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this.SelectedColumns())
                    //{
                    //    if (DC.Value.DataTable().Alias() == KVtab.Value.ParentTable().Alias())
                    //    {
                    //        ParentPresent = true;
                    //        break;
                    //    }
                    //}
                    if (!ParentPresent)
                        continue;
                }

                if (KVtab.Value.Type() != DataTable.TableType.Root && KVtab.Value.ParentTable() == null)
                {
                    continue;
                }
                if ((KVtab.Value.Type() == DataTable.TableType.Single
                    || KVtab.Value.Type() == DataTable.TableType.Parallel
                    || KVtab.Value.Type() == DataTable.TableType.InsertOnly
                    || KVtab.Value.Type() == DataTable.TableType.Lookup) &&
                    KVtab.Value.DisplayedColumns().Count == 0)
                {
                    continue;
                }
                // Checking the WhereClause
                string WhereClause = "";
                if (KVtab.Value.Type() != DataTable.TableType.Project &&
                    KVtab.Value.Type() != DataTable.TableType.Target &&
                    KVtab.Value.Type() != DataTable.TableType.Root &&
                    !IncludedAliases.Contains(KVtab.Key))
                {
                    this.getWhereClause(ref WhereClause, KVtab.Value);
                    if (WhereClause.Length == 0)
                        continue;
                }

                if (SqlFrom.Length > 0)
                {
                    SqlFrom += " INNER JOIN ";
                }
                if (KVtab.Value.View != null && KVtab.Value.View.Length > 0)
                    SqlFrom += KVtab.Value.View + " AS " + KVtab.Key;
                else
                    SqlFrom += KVtab.Value.Name + " AS " + KVtab.Key;
                if (KVtab.Value.ParentTable() != null)
                {
                    SqlFrom += " ON ";
                    string SqlLink = "";
                    if (KVtab.Value.Type() != DataTable.TableType.Referencing)
                    {
                        if (KVtab.Value.ForeignRelationColumns(KVtab.Value.ParentTable().Name).Count > 0)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KVcol in KVtab.Value.ForeignRelationColumns(KVtab.Value.ParentTable().Name))
                            {
                                if (SqlLink.Length > 0) SqlLink += " AND ";
                                SqlLink += KVtab.Key + "." + KVcol.Key + " = " + KVtab.Value.ParentTable().Alias() + "." + KVcol.Value;
                            }
                        }
                        else
                        {
                            if (KVtab.Value.ParentTable().ForeignRelationColumns(KVtab.Value.Name).Count > 0 && KVtab.Value.Type() == DataTable.TableType.Lookup)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<string, string> KVcol in KVtab.Value.ParentTable().ForeignRelationColumns(KVtab.Value.Name))
                                {
                                    if (SqlLink.Length > 0) SqlLink += " AND ";
                                    SqlLink += KVtab.Key + "." + KVcol.Key + " = " + KVtab.Value.ParentTable().Alias() + "." + KVcol.Value;
                                }
                            }
                        }
                    }
                    if (KVtab.Value.SqlRestrictionClause != null && KVtab.Value.SqlRestrictionClause.Length > 0 && KVtab.Value.Type() != DataTable.TableType.Project)
                    {
                        if (SqlLink.Length > 0) SqlLink += " AND ";
                        SqlLink += KVtab.Value.SqlRestrictionClause;
                    }
                    if (KVtab.Value.RestrictionColumns != null && IncludedAliases.Contains(KVtab.Key))
                    {
                        foreach (string R in KVtab.Value.RestrictionColumns)
                        {
                            if (KVtab.Value.DataColumns()[R].RestrictionValue != null && KVtab.Value.DataColumns()[R].RestrictionValue.Length > 0)
                            {
                                if (SqlLink.Length > 0) SqlLink += " AND ";
                                SqlLink += KVtab.Key + "." + R + " = '" + KVtab.Value.DataColumns()[R].RestrictionValue + "' ";
                            }
                        }
                    }

                    SqlFrom += SqlLink;
                }
            }
            return SqlFrom;
        }

        public string FromClause(string ResultTableAlias)
        {
            System.Collections.Generic.List<string> Aliases = new List<string>();
            Aliases.Add(ResultTableAlias);
            return this.FromClause(Aliases);


            string SqlFrom = "";
            foreach (System.Collections.Generic.KeyValuePair<string, DataTable> KVtab in this._DataTables)
            {
                if (KVtab.Value.Type() == DataTable.TableType.Lookup)
                {
                    bool ParentPresent = false;
                    foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this.SelectedColumns())
                    {
                        if (DC.Value.DataTable().Alias() == KVtab.Value.ParentTable().Alias())
                        {
                            ParentPresent = true;
                            break;
                        }
                    }
                    if (!ParentPresent)
                        continue;
                }

                if (KVtab.Value.Type() != DataTable.TableType.Root && KVtab.Value.ParentTable() == null)
                {
                    continue;
                }
                if ((KVtab.Value.Type() == DataTable.TableType.Single
                    || KVtab.Value.Type() == DataTable.TableType.InsertOnly
                    || KVtab.Value.Type() == DataTable.TableType.Parallel
                    || KVtab.Value.Type() == DataTable.TableType.Lookup) &&
                    KVtab.Value.DisplayedColumns().Count == 0)
                {
                    continue;
                }
                // Checking the WhereClause
                string WhereClause = "";
                if (KVtab.Value.Type() != DataTable.TableType.Project &&
                    KVtab.Value.Type() != DataTable.TableType.Target &&
                    KVtab.Value.Type() != DataTable.TableType.Root &&
                    KVtab.Key != ResultTableAlias)
                {
                    this.getWhereClause(ref WhereClause, KVtab.Value);
                    if (WhereClause.Length == 0)
                        continue;
                }

                if (SqlFrom.Length > 0)
                {
                    SqlFrom += " INNER JOIN ";
                }
                if (KVtab.Value.View != null && KVtab.Value.View.Length > 0)
                    SqlFrom += KVtab.Value.View + " AS " + KVtab.Key;
                else
                    SqlFrom += KVtab.Value.Name + " AS " + KVtab.Key;
                if (KVtab.Value.ParentTable() != null)
                {
                    SqlFrom += " ON ";
                    string SqlLink = "";
                    if (KVtab.Value.Type() != DataTable.TableType.Referencing)
                    {
                        if (KVtab.Value.ForeignRelationColumns(KVtab.Value.ParentTable().Name).Count > 0)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KVcol in KVtab.Value.ForeignRelationColumns(KVtab.Value.ParentTable().Name))
                            {
                                if (SqlLink.Length > 0) SqlLink += " AND ";
                                SqlLink += KVtab.Key + "." + KVcol.Key + " = " + KVtab.Value.ParentTable().Alias() + "." + KVcol.Value;
                            }
                        }
                        else
                        {
                            if (KVtab.Value.ParentTable().ForeignRelationColumns(KVtab.Value.Name).Count > 0 && KVtab.Value.Type() == DataTable.TableType.Lookup)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<string, string> KVcol in KVtab.Value.ParentTable().ForeignRelationColumns(KVtab.Value.Name))
                                {
                                    if (SqlLink.Length > 0) SqlLink += " AND ";
                                    SqlLink += KVtab.Key + "." + KVcol.Key + " = " + KVtab.Value.ParentTable().Alias() + "." + KVcol.Value;
                                }
                            }
                        }
                    }
                    if (KVtab.Value.SqlRestrictionClause != null && KVtab.Value.SqlRestrictionClause.Length > 0 && KVtab.Value.Type() != DataTable.TableType.Project)
                    {
                        if (SqlLink.Length > 0) SqlLink += " AND ";
                        SqlLink += KVtab.Value.SqlRestrictionClause;
                    }
                    if (KVtab.Value.RestrictionColumns != null && KVtab.Key != ResultTableAlias)
                    {
                        foreach (string R in KVtab.Value.RestrictionColumns)
                        {
                            if (KVtab.Value.DataColumns()[R].RestrictionValue != null && KVtab.Value.DataColumns()[R].RestrictionValue.Length > 0)
                            {
                                if (SqlLink.Length > 0) SqlLink += " AND ";
                                SqlLink += KVtab.Key + "." + R + " = '" + KVtab.Value.DataColumns()[R].RestrictionValue + "' ";
                            }
                        }
                    }

                    SqlFrom += SqlLink;
                }
            }
            return SqlFrom;
        }

        //private System.Collections.Generic.Dictionary<string, string> _ColumnClauseAliases;

        private string getColumnClause(QueryType Type)
        {
            string SQL = "";
            {
                if (Type == QueryType.DistinctContentForMapIcons)
                {
                    if (this.GeographySymbolTableAlias.Length > 0 && this.GeographySymbolColumn.Length > 0)
                    {
                        if (this.DataTables()[this.GeographySymbolTableAlias].DataColumns()[this.GeographySymbolColumn].SqlForColumn.Length > 0)
                        {
                            string SqlCol = this.DataTables()[this.GeographySymbolTableAlias].DataColumns()[this.GeographySymbolColumn].SqlForColumn;
                            SqlCol = SqlCol.Replace("#TableAlias#", this.GeographySymbolTableAlias);
                            SQL = " DISTINCT " + SqlCol + " AS [" + this.GeographySymbolColumn + "] ";
                        }
                        else
                            SQL = " DISTINCT [" + this.GeographySymbolTableAlias + "].[" + this.GeographySymbolColumn + "] ";
                    }
                }
                else if (Type == QueryType.GeoObjects)
                {
                    if (this.GeographyKeyTableAlias.Length > 0 && this.GeographyKeyColumn.Length > 0)
                        SQL = " DISTINCT [" + this.GeographyKeyTableAlias + "].[" + this.GeographyKeyColumn + "] ";
                }
                else
                {
                    foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this.SelectedColumns())
                    {
                        if (DC.Value.DataTable().Type() == DataTable.TableType.Lookup)
                        {
                            // Markus 20.4.2019 ensure inclusion of root tables
                            bool ParentPresent = this.ParentPresent(DC.Value.DataTable());// false;
                            //foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> dc in this.SelectedColumns())
                            //{
                            //    if (dc.Value.DataTable().Alias() == DC.Value.DataTable().ParentTable().Alias())
                            //    {
                            //        ParentPresent = true;
                            //        break;
                            //    }
                            //}
                            if (!ParentPresent)
                                continue;
                        }

                        if (DC.Value.Type() == DataColumn.ColumnType.Spacer && Type != QueryType.SQL)
                        {
                            if (SQL.Length > 0)
                                SQL += ", ";
                            SQL += "'' AS [|" + DC.Key.ToString() + "]";
                        }
                        else if (DC.Value.Type() == DataColumn.ColumnType.Operation && Type != QueryType.SQL)
                        {
                            if (SQL.Length > 0)
                                SQL += ", ";
                            if (ReadOnly) SQL += "''";
                            else SQL += "'x'";
                            SQL += " AS [x" + DC.Key.ToString() + "]";
                        }
                        else if (DC.Value.SqlForColumn.Length > 0)
                        {
                            if (SQL.Length > 0)
                                SQL += ", ";
                            if (DC.Value.SqlForColumn.IndexOf("#TableAlias#") > -1)
                                SQL += DC.Value.SqlForColumn.Replace("#TableAlias#", DC.Value.DataTable().Alias()) + " AS [" + DC.Value.DisplayText + "]";
                            else
                                SQL += DC.Value.DataTable().Alias() + "." + DC.Value.SqlForColumn + " AS [" + DC.Value.DisplayText + "]";
                        }
                        else if (DC.Value.Type() == DataColumn.ColumnType.Data)
                        {
                            if (Type == QueryType.SQL && !DC.Value.IsVisible)
                                continue;
                            if (SQL.Length > 0)
                                SQL += ", ";
                            if (DC.Value.Column.DataType == "geography")
                            {
                                SQL += DC.Value.DataTable().Alias() + "." + DC.Value.Name + ".ToString() AS [" + DC.Value.DisplayText + "]";
                            }
                            else
                            {
                                switch (Type)
                                {
                                    case QueryType.Filter:
                                        SQL += "CAST(" + DC.Value.DataTable().Alias() + "." + DC.Value.Name + " AS nvarchar(4000)) AS [" + DC.Value.DisplayText + "]";
                                        break;
                                    default:
                                        SQL += DC.Value.DataTable().Alias() + "." + DC.Value.Name + " AS [" + DC.Value.DisplayText + "]";
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            return SQL;
        }

        private bool getWhereClause(ref string WhereClause, DataTable DT)
        {
            try
            {
                if (DT.FilterOperator == "◊")
                {
                    foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this.SelectedColumns())
                    {
                        if (DC.Value.IsVisible && DC.Value.Type() == DataColumn.ColumnType.Data && DC.Value.DataTable().Alias() == DT.Alias())
                        {
                            if (WhereClause.Length > 0)
                                WhereClause += " AND ";
                            WhereClause += "(" + DT.Alias() + "." + DC.Value.Name + " IS NULL OR " + DT.Alias() + "." + DC.Value.Name + " = '')";
                        }
                    }
                }
                else
                {
                    if (DT.FilterOperator == "♦")
                    {
                        foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this.SelectedColumns())
                        {
                            if (DC.Value.IsVisible && DC.Value.Type() == DataColumn.ColumnType.Data && DC.Value.DataTable().Alias() == DT.Alias())
                            {
                                if (DC.Value.DataRetrievalType == DataColumn.RetrievalType.ViewOnly)
                                    continue;
                                if (WhereClause.Length > 0)
                                    WhereClause += " AND ";
                                WhereClause += DT.Alias() + "." + DC.Value.Name + " <> ''";
                            }
                        }
                    }
                    //else
                    {
                        foreach (string Column in DT.FilterColumns)
                        {
                            if (DT.DataColumns()[Column].Column.Table == null)
                            {
                                if (WhereClause.Length > 0)
                                    WhereClause += " AND ";
                                WhereClause += DT.DataColumns()[Column].WhereClause();
                            }
                            else
                            {
                                if (DT.DataColumns()[Column].Column.DataType == "bit")
                                    continue;
                                if (/*DT.FilterOperator == "♦" || DT.FilterOperator == "•" ||*/ DT.FilterOperator == "◊" || DT.FilterOperator == "Ø")
                                    continue;
                                if (DT.RestrictionColumns.Contains(Column))
                                    continue;
                                if (WhereClause.Length > 0)
                                    WhereClause += " AND ";
                                WhereClause += DT.DataColumns()[Column].WhereClause();
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool SaveData(int Row, string TableAlias, ref string Message, ref DataTable.SqlCommandTypeExecuted SqlType)
        {
            return this.DataTables()[TableAlias].SaveData(Row, ref Message, ref SqlType);
        }

        #endregion

        #region Read only

        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> _TableReadOnlyRows;

        private void ResetTableReadOnlyRows()
        {
            this._TableReadOnlyRows = new Dictionary<string, List<int>>();
            this._TableWhiteRows = null;
            try
            {
                if (this._DT != null)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DataTable> KV in this._DataTables)
                    {
                        if (KV.Value.Type() == DataTable.TableType.Root)
                        {
                            int PK = 0;
                            int PKcurrent = 0;
                            for (int i = 0; i < this._DT.Rows.Count; i++)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._SelectedColumns)
                                {
                                    if (DC.Value.DataTable().Alias() != KV.Key)
                                        continue;
                                    if (KV.Value.PrimaryKeyColumnList.Contains(DC.Value.Name))
                                    {
                                        if (this._DT.Rows[i][DC.Value.DisplayText].Equals(System.DBNull.Value))
                                        {
                                            if (!this.getTableNullRows(KV.Key).Contains(i))
                                                this.getTableNullRows(KV.Key).Add(i);
                                        }
                                        else if (int.TryParse(this._DT.Rows[i][DC.Value.DisplayText].ToString(), out PKcurrent))
                                        {
                                            if (PK == PKcurrent)
                                            {
                                                if (!this.getTableReadOnlyRows(KV.Key).Contains(i))
                                                    this.getTableReadOnlyRows(KV.Key).Add(i);
                                            }
                                        }
                                    }
                                }
                                if (PKcurrent != PK)
                                    PK = PKcurrent;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < this._DT.Rows.Count; i++)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._SelectedColumns)
                                {
                                    if (DC.Value.DataTable().Type() == DataTable.TableType.Project)
                                        continue;
                                    if (DC.Value.DataTable().Alias() != KV.Key)
                                        continue;
                                    if (DC.Value.DataTable().Type() == DataTable.TableType.Single ||
                                        DC.Value.DataTable().Type() == DataTable.TableType.InsertOnly ||
                                        DC.Value.DataTable().Type() == DataTable.TableType.Parallel ||
                                        DC.Value.DataTable().Type() == DataTable.TableType.Lookup)
                                    {
                                        if (!this.getTableReadOnlyRows(KV.Key).Contains(i) && this.getTableReadOnlyRows(KV.Value.ParentTable().Alias()).Contains(i))
                                            this.getTableReadOnlyRows(KV.Key).Add(i);
                                    }
                                    if (KV.Value.PrimaryKeyColumnList.Contains(DC.Value.Name))
                                    {
                                        if (this._DT.Columns.Contains(DC.Value.DisplayText) && this._DT.Rows[i][DC.Value.DisplayText].Equals(System.DBNull.Value))
                                        {
                                            this.getTableNullRows(KV.Key).Add(i);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        public System.Collections.Generic.List<int> getTableReadOnlyRows(string Table)
        {
            if (this._TableReadOnlyRows != null && this._TableReadOnlyRows.ContainsKey(Table))
                return this._TableReadOnlyRows[Table];
            else
            {
                if (this._TableReadOnlyRows == null)
                    this._TableReadOnlyRows = new Dictionary<string, List<int>>();
                if (!this._TableReadOnlyRows.ContainsKey(Table))
                {
                    System.Collections.Generic.List<int> L = new List<int>();
                    this._TableReadOnlyRows.Add(Table, L);
                }
                return this._TableReadOnlyRows[Table];
            }
        }

        #endregion

        #region Null rows

        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> _TableNullRows;
        public System.Collections.Generic.List<int> getTableNullRows(string Table)
        {
            if (this._TableNullRows == null)
                this._TableNullRows = new Dictionary<string, List<int>>();
            if (!this._TableNullRows.ContainsKey(Table))
            {
                System.Collections.Generic.List<int> L = new List<int>();
                this._TableNullRows.Add(Table, L);
                this.setTableNullRows(Table);
            }
            return this._TableNullRows[Table];
        }

        private void setTableNullRows(string TableAlias)
        {
            try
            {
                if (this._DataTables[TableAlias].Type() == DataTable.TableType.Root)
                {
                    int PK = 0;
                    int PKcurrent = 0;
                    for (int i = 0; i < this._DT.Rows.Count; i++)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._SelectedColumns)
                        {
                            if (DC.Value.DataTable().Alias() != TableAlias)
                                continue;
                            if (this._DataTables[TableAlias].PrimaryKeyColumnList.Contains(DC.Value.Name))
                            {
                                if (this._DT.Rows[i][DC.Value.DisplayText].Equals(System.DBNull.Value))
                                {
                                    if (!this.getTableNullRows(TableAlias).Contains(i))
                                        this.getTableNullRows(TableAlias).Add(i);
                                }
                                else if (int.TryParse(this._DT.Rows[i][DC.Value.DisplayText].ToString(), out PKcurrent))
                                {
                                    if (PK == PKcurrent)
                                    {
                                        if (!this.getTableReadOnlyRows(TableAlias).Contains(i))
                                            this.getTableReadOnlyRows(TableAlias).Add(i);
                                    }
                                }
                            }
                        }
                        if (PKcurrent != PK)
                            PK = PKcurrent;
                    }
                }
                else
                {
                    for (int i = 0; i < this._DT.Rows.Count; i++)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._SelectedColumns)
                        {
                            if (DC.Value.DataTable().Type() == DataTable.TableType.Project)
                                continue;
                            if (DC.Value.DataTable().Alias() != TableAlias)
                                continue;
                            if (DC.Value.DataTable().Type() == DataTable.TableType.Single ||
                                DC.Value.DataTable().Type() == DataTable.TableType.InsertOnly ||
                                DC.Value.DataTable().Type() == DataTable.TableType.Parallel ||
                                DC.Value.DataTable().Type() == DataTable.TableType.Lookup)
                            {
                                if (!this.getTableReadOnlyRows(TableAlias).Contains(i) && this.getTableReadOnlyRows(this._DataTables[TableAlias].ParentTable().Alias()).Contains(i))
                                    this.getTableReadOnlyRows(TableAlias).Add(i);
                            }
                            if (this._DataTables[TableAlias].PrimaryKeyColumnList.Contains(DC.Value.Name))
                            {
                                if (this._DT.Columns.Contains(DC.Value.DisplayText) && this._DT.Rows[i][DC.Value.DisplayText].Equals(System.DBNull.Value))
                                {
                                    this.getTableNullRows(TableAlias).Add(i);
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region White rows - those rows that do not have the color of the table to display changes between content

        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> _TableWhiteRows;
        public System.Collections.Generic.List<int> getTableWhiteRows(string Table)
        {
            if (this._TableWhiteRows == null)
            {
                this._TableWhiteRows = new Dictionary<string, List<int>>();
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<int>> KV in this._TableReadOnlyRows)
                {
                    try
                    {
                        System.Collections.Generic.List<int> W = new List<int>();
                        bool IsWhite = false;
                        for (int i = 0; i < this._DT.Rows.Count; i++)
                        {
                            if (i > 0 && !KV.Value.Contains(i))
                                IsWhite = !IsWhite;
                            if (IsWhite) W.Add(i);
                        }
                        int w = 0;
                        w++;
                        this._TableWhiteRows.Add(KV.Key, W);
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
            }
            if (this._TableWhiteRows.ContainsKey(Table))
                return this._TableWhiteRows[Table];
            else
            {
                System.Collections.Generic.List<int> L = new List<int>();
                return L;
            }
        }

        #endregion

        #region Filter

        private System.Data.DataTable _DtFilter;

        public System.Data.DataTable DtFilter
        {
            get { return _DtFilter; }
            set { _DtFilter = value; }
        }

        private System.Collections.Generic.Dictionary<string, string> _FilterOperators;

        public System.Collections.Generic.Dictionary<string, string> FilterOperators
        {
            get
            {
                if (_FilterOperators == null)
                {
                    this._FilterOperators = new Dictionary<string, string>();
                    //this._FilterOperators.Add("", "No filter");
                    this._FilterOperators.Add("~", "~  Similar to (use wildcards % _)");
                    this._FilterOperators.Add("¬", "¬  Not similar to (use wildcards % _)");
                    this._FilterOperators.Add("=", "=  Equal to");
                    this._FilterOperators.Add("≠", "≠  Not equal to");
                    this._FilterOperators.Add("Ø", "Ø  Empty");
                    this._FilterOperators.Add("•", "•  Not empty");
                    this._FilterOperators.Add("|", "|   Within a list of values");
                    this._FilterOperators.Add("∉", "∉  Not within a list of values");
                    this._FilterOperators.Add("<", "<  Smaller than");
                    this._FilterOperators.Add(">", ">  Bigger than");
                    this._FilterOperators.Add("+H", "+H    Include lower Hierarchy");
                    this._FilterOperators.Add("+S", "+S    Include Synonyms");
                    this._FilterOperators.Add("+H+S", "+H+S  Include lower Hierarchy and Synonyms");
                }
                return _FilterOperators;
            }
            //set { _FilterOperators = value; }
        }

        private System.Collections.Generic.Dictionary<string, string> _FilterOperatorListColumn;
        public System.Collections.Generic.Dictionary<string, string> FilterOperatorListColumn()
        {
            if (this._FilterOperatorListColumn == null)
            {
                this._FilterOperatorListColumn = new Dictionary<string, string>();
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in FilterOperators)
                {
                    if (KV.Key.Length == 1)
                        this._FilterOperatorListColumn.Add(KV.Value, KV.Key);
                }
            }
            return this._FilterOperatorListColumn;
        }

        public System.Collections.Generic.Dictionary<string, string> FilterOperatorListColumn(DataColumn DC)
        {
            //this._FilterOperatorListColumn.Add(FilterOperators[""], "");//"+H    Include lower Hierarchy", "+H");
            if (DC.Column.DataType != null)
            {
                switch (DC.Column.DataType.ToLower())
                {
                    case "int":
                    case "smallint":
                    case "bigint":
                    case "tinyint":
                    case "float":
                    case "real":
                    case "numeric":
                    case "bit":
                    case "decimal":
                    case "smallmoney":
                    case "money":
                    case "datetime":
                    case "datetime2":
                    case "smalldatetime":
                    case "date":
                    case "datetimeoffset":
                    case "time":
                    case "binary":
                    case "varbinary":
                    case "image":
                        if (this.FilterOperatorListColumn().ContainsKey(FilterOperators["~"]))//"Similar to (use wildcards % _)"))
                            this._FilterOperatorListColumn.Remove(FilterOperators["~"]);//"Similar to (use wildcards % _)");
                        if (this.FilterOperatorListColumn().ContainsKey(FilterOperators["¬"]))//"Not similar to (use wildcards % _)"))
                            this._FilterOperatorListColumn.Remove(FilterOperators["¬"]);//"Not similar to (use wildcards % _)");
                        break;
                }
                switch (DC.Column.DataType.ToLower())
                {
                    case "datetime":
                    case "datetime2":
                    case "smalldatetime":
                    case "date":
                    case "datetimeoffset":
                    case "time":
                    case "binary":
                    case "varbinary":
                    case "image":
                        if (this.FilterOperatorListColumn().ContainsKey(FilterOperators["|"]))//"Within a list of values"))
                            this._FilterOperatorListColumn.Remove(FilterOperators["|"]);//"Within a list of values");
                        if (this.FilterOperatorListColumn().ContainsKey(FilterOperators["∉"]))//"Not within a list of values"))
                            this._FilterOperatorListColumn.Remove(FilterOperators["∉"]);//"Not within a list of values");
                        break;
                }
            }
            else
            {
                if (DC.DataRetrievalType == DataColumn.RetrievalType.ViewOnly)
                {
                }
            }
            if (DC.LinkedModule != RemoteLink.LinkedModule.None)
            {
                switch (DC.LinkedModule)
                {
                    case RemoteLink.LinkedModule.DiversityTaxonNames:
                        if (!this._FilterOperatorListColumn.ContainsKey(FilterOperators["+H"]))//"Include lower Hierarchy"))
                            this._FilterOperatorListColumn.Add(FilterOperators["+H"], "+H");//"+H    Include lower Hierarchy", "+H");
                        if (!this._FilterOperatorListColumn.ContainsKey(FilterOperators["+S"]))//"Include Synonyms"))
                            this._FilterOperatorListColumn.Add(FilterOperators["+S"], "+S");//"+S    Include Synonyms", "+S");
                        if (!this._FilterOperatorListColumn.ContainsKey(FilterOperators["+H+S"]))//"Include lower Hierarchy and Synonyms"))
                            this._FilterOperatorListColumn.Add(FilterOperators["+H+S"], "+H+S");//"+H+S  Include lower Hierarchy and Synonyms", "+H+S");
                        break;
                }
            }
            return this._FilterOperatorListColumn;
        }

        public System.Collections.Generic.Dictionary<string, string> FilterOperatorColumnDictionary()
        {
            System.Collections.Generic.Dictionary<string, string> Dict = new Dictionary<string, string>();
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.FilterOperatorListColumn())
            {
                if (!Dict.ContainsKey(KV.Value))
                    Dict.Add(KV.Value, KV.Key);
            }
            return Dict;
        }


        private System.Collections.Generic.Dictionary<string, string> _FilterOperatorListTable;
        public System.Collections.Generic.Dictionary<string, string> FilterOperatorListTable()
        {
            if (this._FilterOperatorListTable == null)
            {
                this._FilterOperatorListTable = new Dictionary<string, string>();
                this._FilterOperatorListTable.Add("No filter", "");
                this._FilterOperatorListTable.Add("♦  Filled (all visible columns)", "♦");
                this._FilterOperatorListTable.Add("◊  Empty (all visible columns)", "◊");
                this._FilterOperatorListTable.Add("•  Data in table do exist", "•");
                this._FilterOperatorListTable.Add("Ø  Data in table are missing", "Ø");
            }
            return this._FilterOperatorListTable;
        }
        public System.Collections.Generic.Dictionary<string, string> FilterOperatorTableDictionary()
        {
            System.Collections.Generic.Dictionary<string, string> Dict = new Dictionary<string, string>();
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.FilterOperatorListTable())
            {
                if (!Dict.ContainsKey(KV.Value))
                    Dict.Add(KV.Value, KV.Key);
            }
            return Dict;
        }

        public string FilterOperatorToolTip(string Operator)
        {
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.FilterOperatorListTable())
            {
                if (KV.Value == Operator)
                {
                    return KV.Key;
                }
            }
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.FilterOperatorListColumn())
            {
                if (KV.Value == Operator)
                {
                    return KV.Key;
                }
            }
            return "";
        }

        System.Collections.Generic.Dictionary<int, string> _Filter;
        public void ResetFilter()
        {
            this._Filter = null;
        }
        public System.Collections.Generic.Dictionary<int, string> Filter()
        {
            if (this._Filter == null)
            {
                this._Filter = new Dictionary<int, string>();
                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this.SelectedColumns())
                {
                    try
                    {
                        if (DC.Value.DataTable().DataColumns().ContainsKey(DC.Value.Name) &&
                            DC.Value.DataTable().DataColumns()[DC.Value.Name].FilterValue != null &&
                            DC.Value.DataTable().DataColumns()[DC.Value.Name].FilterValue.Length > 0)
                        {
                            this._Filter.Add(DC.Key, DC.Value.DataTable().DataColumns()[DC.Value.Name].FilterValue);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
            return this._Filter;
        }

        public void SetFilterValue(int Position, string Filter)
        {
            this.SelectedColumns()[Position].FilterValue = Filter;
            this.ResetFilter();
        }

        //public void SetFilterOperator(int Position, string FilterOperator)
        //{
        //    this.DataColumns()[Position].FilterOperator = FilterOperator;
        //    this.ResetFilter();
        //}

        #endregion

        #region Adding

        private System.Data.DataTable _DtAdding;

        public System.Data.DataTable DtAdding
        {
            get { return _DtAdding; }
            set { _DtAdding = value; }
        }

        //public System.Data.DataTable DtAdding()
        //{
        //    if (this._DtAdding == null)
        //        this._DtAdding = new System.Data.DataTable("Adding" + this._Target);
        //    return this._DtAdding;
        //}

        private int _AddingSelectedRowIndex = -1;
        private string _AddingTargetTableAlias;

        /// <summary>
        /// Insert new data. All tables depending on the selected table and displayed in the interface will be filled with data.
        /// The informations for the required parent tables, if available, will be taken from the selected row.
        /// If informations are missing the user will be informed and asked for the creation of parent data
        /// The new data will be inserted in the table and no requery will happen as the new data may not fulfill the filter criteria
        /// </summary>
        /// <param name="TableAlias">The alias for the table where the insert should happen</param>
        /// <param name="SelectedRowIndex">The index of the row selected for the defaults for the insert</param>
        /// <returns></returns>
        //        public bool AddingData(string TableAlias, int? SelectedRowIndex, ref string Message)
        //        {
        //            bool OK = true;
        //            //if (SelectedRowIndex == -1)
        //            //    SelectedRowIndex = this.DT().Rows.Count - 1;
        //            if (this.DataTables()[TableAlias].CheckAddingPrerequisite(SelectedRowIndex, ref Message))
        //            {
        //                //this.DataTables()[TableAlias].AddData(SelectedRowIndex, ref Message);

        ////#hier weiter


        //                for (int i = 0; i < this.DT().Columns.Count; i++)
        //                {
        //                    if (this.SelectedColumns()[i].DataTable().Alias() == TableAlias)
        //                    {
        //                        System.Data.DataRow AddedRow = this.DT().NewRow();
        //                        if (this.DataTables()[TableAlias].AddData(SelectedRowIndex, ref AddedRow, ref Message))
        //                        {
        //                            this.getData();
        //                        }
        //                        else if (Message.Length > 0)
        //                        {
        //                            System.Windows.Forms.MessageBox.Show("Adding failed:\r\n" + Message);
        //                        }
        //                        break;
        //                    }
        //                }



        //                //if (SelectedRowIndex != null)
        //                //{
        //                //    System.Data.DataRow Rnew = this.DT().NewRow();
        //                //    for (int i = 0; i < this.DT().Columns.Count; i++)
        //                //    {
        //                //        Rnew[this.DT().Columns[i]] = this.DT().Rows[(int)SelectedRowIndex][i];
        //                //        if (this.SelectedColumns()[i].DataTable().Alias() == TableAlias)
        //                //        {
        //                //            if (this.DataTables()[TableAlias].AddData((int)SelectedRowIndex, ref Rnew, ref Message))
        //                //            {
        //                //                this.DT().Rows.Add(Rnew);
        //                //            }
        //                //            else if (Message.Length > 0)
        //                //            {
        //                //                System.Windows.Forms.MessageBox.Show("Adding failed:\r\n" + Message);
        //                //            }
        //                //            break;
        //                //        }
        //                //    }
        //                //}
        //                //else
        //                //{
        //                //    this.DataTables()[TableAlias].AddData(SelectedRowIndex, ref Message);
        //                //}
        //            }
        //            else if (Message.Length > 0)
        //                System.Windows.Forms.MessageBox.Show("Adding failed:\r\n" + Message);

        //            //this._AddingTargetTableAlias = TableAlias;
        //            //this._AddingTableList = null;
        //            //this._AddingSelectedRowIndex = SelectedRowIndex;
        //            //foreach(string T in this.AddingTableList())
        //            //{
        //            //}
        //            return OK;
        //        }


        public bool AddingData(string TableAlias, System.Data.DataRow AddedRow, ref string Message, ref System.Collections.Generic.Dictionary<string, string> PKvalues)
        {
            bool OK = true;
            try
            {
                if (this.DataTables()[TableAlias].CheckAddingPrerequisite(ref Message))
                {
                    for (int i = 0; i < this.DT().Columns.Count; i++)
                    {
                        if (this.SelectedColumns()[i].DataTable().Alias() == TableAlias)
                        {
                            //System.Data.DataRow AddedRow = this.DT().NewRow();
                            if (this.DataTables()[TableAlias].AddData(ref AddedRow, ref Message, ref PKvalues))
                            {
                                this.getData();
                            }
                            else if (Message.Length > 0)
                            {
                                System.Windows.Forms.MessageBox.Show("Adding failed:\r\n" + Message);
                            }
                            break;
                        }
                    }
                }
                else if (Message.Length > 0)
                    System.Windows.Forms.MessageBox.Show("Adding failed:\r\n" + Message);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }


        private bool CheckAddingPrerequisite(string TableAlias, int SelectedRowIndex)
        {
            bool OK = true;

            return OK;
        }


        #region Table list for the adding

        //private System.Collections.Generic.List<string> _AddingTableList = null;
        ///// <summary>
        ///// The list of tables where data will be added
        ///// </summary>
        ///// <returns></returns>
        //private System.Collections.Generic.List<string> AddingTableList()
        //{
        //    if (this._AddingTableList == null)
        //    {
        //        this._AddingTableList = new List<string>();
        //        string Message = "";
        //        if (this.DataTables()[this._AddingTargetTableAlias].AllNotNullValuesForAddingProvided(this._AddingSelectedRowIndex, ref Message))
        //        {
        //            this._AddingTableList.Add(this._AddingTargetTableAlias);
        //            //this.AddingAddDependentTables(this._AddingTargetTableAlias);
        //        }
        //        if (Message.Length > 0)
        //            System.Windows.Forms.MessageBox.Show(Message);
        //    }
        //    return this._AddingTableList;
        //}

        /// <summary>
        /// Adding the tables dependent on the inserted table 
        /// assuming that all visible tables should be filled provided that enough informations are available
        /// </summary>
        /// <param name="TableAlias">The alias of the parent table</param>
        //private void AddingAddDependentTables(string TableAlias)
        //{
        //    foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this.SelectedColumns())
        //    {
        //        if (DC.Value.DataTable().PrimaryKeyColumnList.Contains(DC.Value.Name) &&
        //            DC.Value.DataTable().ParentTable() != null &&
        //            DC.Value.DataTable().ParentTable().Alias() == TableAlias &&
        //            !this._AddingTableList.Contains(DC.Value.DataTable().Alias()))
        //        {
        //            string Message = "";
        //            if (DC.Value.DataTable().AllNotNullValuesForAddingProvided(this._AddingDefaultRowIndex, ref Message))
        //            {
        //                this._AddingTableList.Add(DC.Value.DataTable().Alias());
        //                this.AddingAddDependentTables(DC.Value.DataTable().Alias());
        //            }
        //        }
        //    }
        //}

        #endregion

        //private bool CheckAddingPrerequisite()
        //{
        //    bool OK = true;
        //    foreach (string T in this._AddingTableList)
        //    {

        //    }
        //    return OK;
        //}

        #endregion

        #region Lookup lists for columns

        private System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<string>> _LookupLists;

        public System.Collections.Generic.List<string> LookupList(int ColumnIndex)
        {
            if (this._LookupLists == null)
                this._LookupLists = new Dictionary<int, List<string>>();
            if (!this._LookupLists.ContainsKey(ColumnIndex))
            {
                System.Collections.Generic.List<string> L = new List<string>();
                if (this._SelectedColumns[ColumnIndex].LookupSource != null)
                {
                    L.Add("");
                    foreach (System.Data.DataRow R in this._SelectedColumns[ColumnIndex].LookupSource.Rows)
                    {
                        L.Add(R["Value"].ToString());
                    }
                }
                else
                {
                    L.Add("");
                    foreach (System.Data.DataRow R in this.DT().Rows)
                    {
                        if (!R[ColumnIndex].Equals(System.DBNull.Value))
                        {
                            if (this.SelectedColumns()[ColumnIndex].LookupSource != null)
                            {
                                if (!L.Contains(R[ColumnIndex].ToString()))
                                    L.Add(R[ColumnIndex].ToString());
                            }
                            else
                            {
                                string[] Value = R[ColumnIndex].ToString().Split(new char[] { ' ' });
                                if (Value.Length > 0)
                                {
                                    if (!L.Contains(Value[0]))
                                        L.Add(Value[0]);
                                    if (Value.Length > 1)
                                    {
                                        if (!L.Contains(Value[1]))
                                            L.Add(Value[1]);
                                    }
                                }
                            }
                        }
                    }
                    L.Sort();
                }
                this._LookupLists.Add(ColumnIndex, L);
            }
            return this._LookupLists[ColumnIndex];
        }

        public void ResetLookupLists()
        {
            this._LookupLists = null;
            this._LookupDictionaries = null;
        }

        private System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, string>> _LookupDictionaries;

        public System.Collections.Generic.Dictionary<string, string> LookupDictionary(int ColumnIndex)
        {
            if (this._LookupDictionaries == null)
                this._LookupDictionaries = new Dictionary<int, Dictionary<string, string>>();
            if (!this._LookupDictionaries.ContainsKey(ColumnIndex))
            {
                System.Collections.Generic.Dictionary<string, string> D = new Dictionary<string, string>();
                D.Add("", "");
                if (this.SelectedColumns()[ColumnIndex].LookupSource != null)
                {
                    foreach (System.Data.DataRow R in this.SelectedColumns()[ColumnIndex].LookupSource.Rows)
                    {
                        if (!D.ContainsKey(R["Value"].ToString()))
                            D.Add(R["Value"].ToString(), R["Display"].ToString());
                    }
                }
                this._LookupDictionaries.Add(ColumnIndex, D);
            }
            return this._LookupDictionaries[ColumnIndex];
        }

        #endregion

        #region Setting linked values

        public enum Grid { Filter, Data, Adding }

        // Parameters to get local values back to spreadsheet
        private DiversityWorkbench.ServerConnection _FixedSourceServerConnection;
        public DiversityWorkbench.ServerConnection FixedSourceServerConnection() { return this._FixedSourceServerConnection; }

        private DWBServices.WebServices.DwbServiceEnums.DwbService _FixedSourceWebservice;
        public DWBServices.WebServices.DwbServiceEnums.DwbService FixedSourceWebservice() { return this._FixedSourceWebservice; }

        public void setLinkedColumnValues(System.Collections.Generic.List<string> Settings
            , DiversityWorkbench.ServerConnection FixSourceServerConnection
            , DWBServices.WebServices.DwbServiceEnums.DwbService FixSourceWebservice
            , DiversityWorkbench.IWorkbenchUnit FixSourceIWorkbenchUnit
            , System.Windows.Forms.DataGridViewCell Cell
            , Grid SourceGrid, ref string Error)
        {
            try
            {
                bool PKinvolved = false; // Markus 18.7.23 - PK may be involved in update
                System.Collections.Generic.Dictionary<string, string> PK = new Dictionary<string, string>();
                if ((bool)this.SelectedColumns()[Cell.ColumnIndex].IsLinkColumn)
                {
                    if (Settings != null && Settings.Count > 0)
                    {
                        string DisplayColum = this.SelectedColumns()[Cell.ColumnIndex].RemoteLinkDisplayColumn;
                        string LinkedColumnValue = "";
                        foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this.SelectedColumns())
                        {
                            if (DC.Value.Column.Name == DisplayColum && DC.Value.DataTable().Alias() == this.SelectedColumns()[Cell.ColumnIndex].DataTable().Alias())
                            {
                                LinkedColumnValue = this.DT().Rows[Cell.RowIndex][DC.Key].ToString();
                                if (DC.Value.DataTable().PrimaryKeyColumnList.Contains(DisplayColum))
                                {
                                    PKinvolved = true;
                                }
                                break;
                            }
                        }
                        FormFixedSourceQuery f = new FormFixedSourceQuery(Settings, FixSourceServerConnection, FixSourceWebservice, FixSourceIWorkbenchUnit, Cell, this);
                        f.ShowDialog();
                        if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            System.Collections.Generic.Dictionary<string, string> UnitValues = f.FixedSourceValues();
                            if (UnitValues.Count > 0)
                                this.setLinkedColumnValues(Cell, UnitValues, SourceGrid, ref Error);
                            else if (f.SelectedIndex() == -1 && f.EnteredText().Length > 0)
                                System.Windows.Forms.MessageBox.Show("Please select a value from the list.\r\nThe value\r\n\t" + f.EnteredText() + "\r\nis not in the list");

                            // Do not touch connection parameters if dialog = Cancel
                            this._FixedSourceServerConnection = f.FixSourceServerConnection();
                            this._FixedSourceWebservice = f.FixSourceWebservice();
                            if (Settings != null)
                            {
                                DiversityWorkbench.WorkbenchUnit.SaveSetting(Settings,
                                    this._FixedSourceServerConnection, this._FixedSourceWebservice);
                            }
                        }
                        else if (this._FixedSourceServerConnection == null && this._FixedSourceWebservice == DwbServiceEnums.DwbService.None)
                        {
                            // set connection parameters if they are missing so far
                            this._FixedSourceServerConnection = f.FixSourceServerConnection();
                            this._FixedSourceWebservice = f.FixSourceWebservice();
                        }
                    }
                    else if (this.SelectedColumns()[Cell.ColumnIndex].TypeOfLink == DataColumn.LinkType.DiversityWorkbenchModule ||
                       this.SelectedColumns()[Cell.ColumnIndex].TypeOfLink == DataColumn.LinkType.OptionalLinkToDiversityWorkbenchModule)
                    {
                        DiversityWorkbench.IWorkbenchUnit iWU = null;
                        switch (this.SelectedColumns()[Cell.ColumnIndex].LinkedModule)
                        {
                            case RemoteLink.LinkedModule.DiversityAgents:
                                DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                                iWU = (DiversityWorkbench.IWorkbenchUnit)A;
                                break;
                            case RemoteLink.LinkedModule.DiversityGazetteer:
                                DiversityWorkbench.Gazetteer G = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                                iWU = (DiversityWorkbench.IWorkbenchUnit)G;
                                break;
                            case RemoteLink.LinkedModule.DiversityProjects:
                                DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
                                iWU = (DiversityWorkbench.IWorkbenchUnit)P;
                                break;
                            case RemoteLink.LinkedModule.DiversityReferences:
                                DiversityWorkbench.Reference R = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
                                iWU = (DiversityWorkbench.IWorkbenchUnit)R;
                                break;
                            case RemoteLink.LinkedModule.DiversitySamplingPlots:
                                DiversityWorkbench.SamplingPlot SP = new DiversityWorkbench.SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                                iWU = (DiversityWorkbench.IWorkbenchUnit)SP;
                                break;
                            case RemoteLink.LinkedModule.DiversityScientificTerms:
                                DiversityWorkbench.ScientificTerm ST = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                                iWU = (DiversityWorkbench.IWorkbenchUnit)ST;
                                break;
                            case RemoteLink.LinkedModule.DiversityTaxonNames:
                                DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                                iWU = (DiversityWorkbench.IWorkbenchUnit)T;
                                break;
                            case RemoteLink.LinkedModule.None:
                                if (this.SelectedColumns()[Cell.ColumnIndex].RemoteLinkIsOptional && this.SelectedColumns()[Cell.ColumnIndex].RemoteLinks.Count > 0)
                                {
                                    if (true)
                                    {
                                    }
                                }
                                break;
                        }
                        this.setLinkedColumnValues(Cell, iWU, SourceGrid, ref Error);
                    }
                    else
                    {
                        switch (this.SelectedColumns()[Cell.ColumnIndex].TypeOfLink)
                        {
                            case DataColumn.LinkType.Resource:
                                if (Cell.Value.ToString().Length > 0)
                                {
                                    DiversityWorkbench.Forms.FormImage fI = new DiversityWorkbench.Forms.FormImage(Cell.Value.ToString());
                                    fI.ShowDialog();
                                }
                                else
                                {
                                    DiversityWorkbench.Forms.FormGetImage fGI = new DiversityWorkbench.Forms.FormGetImage();
                                    fGI.ShowDialog();
                                    if (fGI.DialogResult == System.Windows.Forms.DialogResult.OK)
                                    {
                                        Cell.Value = fGI.URIImage;
                                    }
                                }
                                break;
                            case DataColumn.LinkType.Web:
                                DiversityWorkbench.Forms.FormWebBrowser fW = new DiversityWorkbench.Forms.FormWebBrowser(Cell.Value.ToString());
                                fW.ShowDialog();
                                break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void setLinkedColumnValues(System.Windows.Forms.DataGridViewCell Cell, DiversityWorkbench.IWorkbenchUnit I, Grid SourceGrid, ref string Error)
        {
            try
            {
                if (Cell.Value.ToString().Length > 0 && SourceGrid != Grid.Filter) // A filter value should allways be changed as it may contain a list
                {
                    DiversityWorkbench.Forms.FormRemoteQuery fT = new DiversityWorkbench.Forms.FormRemoteQuery(Cell.Value.ToString(), I);
                    fT.ShowDialog();
                }
                else
                {
                    DiversityWorkbench.Forms.FormRemoteQuery fT;
                    ServerConnection SC = this.getFixSourceConnection(this.SelectedColumns()[Cell.ColumnIndex].DataTable().Alias(), this.SelectedColumns()[Cell.ColumnIndex].Column.Name);
                    if (SC != null)
                    {
                        //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(SC.ConnectionStringForDB(SC.DatabaseName, SC.ModuleName));
                        fT = new DiversityWorkbench.Forms.FormRemoteQuery(I, SC);
                    }
                    else
                        fT = new DiversityWorkbench.Forms.FormRemoteQuery(I);
                    //fT.SetAcceptAndCancelButton();
                    fT.ShowDialog();
                    if (fT.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        this.setFixSourceConnection(this.SelectedColumns()[Cell.ColumnIndex].DataTable().Alias(), this.SelectedColumns()[Cell.ColumnIndex].Column.Name, fT.ServerConnection);
                        if (this.SelectedColumns()[Cell.ColumnIndex].RemoteLinks != null &&
                            this.SelectedColumns()[Cell.ColumnIndex].RemoteLinks.Count > 0)
                        {
                            System.Collections.Generic.List<string> TablesToUpdate = new List<string>();
                            this.DT().Rows[Cell.RowIndex].BeginEdit();
                            System.Collections.Generic.Dictionary<string, string> UnitValues = I.UnitValues();
                            foreach (DiversityWorkbench.Spreadsheet.RemoteLink RL in this.SelectedColumns()[Cell.ColumnIndex].RemoteLinks)
                            {
                                if (RL.LinkedToModule == this.SelectedColumns()[Cell.ColumnIndex].LinkedModule)
                                {
                                    foreach (DiversityWorkbench.Spreadsheet.RemoteColumnBinding RCB in RL.RemoteColumnBindings)
                                    {
                                        if (UnitValues.ContainsKey(RCB.RemoteParameter) &&
                                            UnitValues[RCB.RemoteParameter] != null &&
                                            UnitValues[RCB.RemoteParameter].Length > 0 &&
                                            this.DT().Columns.Contains(RCB.Column.DisplayText))
                                        {
                                            if (SourceGrid == Grid.Data)
                                            {
                                                switch (RCB.ModeOfUpdate)
                                                {
                                                    case RemoteColumnBinding.UpdateMode.Allways:
                                                        this.DT().Rows[Cell.RowIndex][RCB.Column.DisplayText] = UnitValues[RCB.RemoteParameter];
                                                        foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._SelectedColumns)
                                                        {
                                                            if (DC.Value.DisplayText == RCB.Column.DisplayText)
                                                            {
                                                                if (!TablesToUpdate.Contains(DC.Value.DataTable().Alias()))
                                                                    TablesToUpdate.Add(DC.Value.DataTable().Alias());
                                                                break;
                                                            }
                                                        }
                                                        break;
                                                    case RemoteColumnBinding.UpdateMode.IfNotEmptyAskUser:
                                                        if (this.DT().Rows[Cell.RowIndex][RCB.Column.DisplayText].Equals(System.DBNull.Value) ||
                                                            this.DT().Rows[Cell.RowIndex][RCB.Column.DisplayText].ToString().Length == 0)
                                                        {
                                                            this.DT().Rows[Cell.RowIndex][RCB.Column.DisplayText] = UnitValues[RCB.RemoteParameter];
                                                            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._SelectedColumns)
                                                            {
                                                                if (DC.Value.DisplayText == RCB.Column.DisplayText)
                                                                {
                                                                    if (!TablesToUpdate.Contains(DC.Value.DataTable().Alias()))
                                                                        TablesToUpdate.Add(DC.Value.DataTable().Alias());
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            string OldValue = this.DT().Rows[Cell.RowIndex][RCB.Column.DisplayText].ToString();
                                                            string NewValue = UnitValues[RCB.RemoteParameter];
                                                            if (OldValue != NewValue)
                                                            {
                                                                string Message = "Change value for " + RCB.Column.Name + "\r\nfrom " + OldValue + "\r\nto " + NewValue + "?";
                                                                if (System.Windows.Forms.MessageBox.Show(Message, "Update " + RCB.Column.Name + "?", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                                                                {
                                                                    this.DT().Rows[Cell.RowIndex][RCB.Column.DisplayText] = UnitValues[RCB.RemoteParameter];
                                                                    foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._SelectedColumns)
                                                                    {
                                                                        if (DC.Value.DisplayText == RCB.Column.DisplayText)
                                                                        {
                                                                            if (!TablesToUpdate.Contains(DC.Value.DataTable().Alias()))
                                                                                TablesToUpdate.Add(DC.Value.DataTable().Alias());
                                                                            break;
                                                                        }
                                                                    }

                                                                }
                                                            }
                                                        }
                                                        break;
                                                    case RemoteColumnBinding.UpdateMode.OnlyEmpty:
                                                        if (this.DT().Rows[Cell.RowIndex][RCB.Column.DisplayText].Equals(System.DBNull.Value) ||
                                                            this.DT().Rows[Cell.RowIndex][RCB.Column.DisplayText].ToString().Length == 0)
                                                        {
                                                            this.DT().Rows[Cell.RowIndex][RCB.Column.DisplayText] = UnitValues[RCB.RemoteParameter];
                                                            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._SelectedColumns)
                                                            {
                                                                if (DC.Value.DisplayText == RCB.Column.DisplayText)
                                                                {
                                                                    if (!TablesToUpdate.Contains(DC.Value.DataTable().Alias()))
                                                                        TablesToUpdate.Add(DC.Value.DataTable().Alias());
                                                                    break;
                                                                }
                                                            }

                                                        }
                                                        break;
                                                }
                                            }
                                            else if (SourceGrid == Grid.Adding)
                                            {
                                                this.DtAdding.Rows[0][RCB.Column.DisplayText] = UnitValues[RCB.RemoteParameter];
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                            foreach (string DT in TablesToUpdate)
                            {
                                DataTable.SqlCommandTypeExecuted SqlType = DataTable.SqlCommandTypeExecuted.None;
                                this._DataTables[DT].SaveData(Cell.RowIndex, ref Error, ref SqlType);
                            }
                        }
                        else if (this.SelectedColumns()[Cell.ColumnIndex].RemoteLinkDisplayColumn != null && SourceGrid != Grid.Filter)
                        {
                            this.DT().Rows[Cell.RowIndex].BeginEdit();
                        }
                        if (SourceGrid == Grid.Data)
                        {
                            this.DT().Rows[Cell.RowIndex][this.SelectedColumns()[Cell.ColumnIndex].DataTable().DataColumns()[this.SelectedColumns()[Cell.ColumnIndex].RemoteLinkDisplayColumn].DisplayText] = fT.DisplayText;
                            this.DT().Rows[Cell.RowIndex].EndEdit();
                            this.DT().Rows[Cell.RowIndex].AcceptChanges();
                        }
                        else if (SourceGrid == Grid.Adding)
                        {
                            this.DtAdding.Rows[Cell.RowIndex][this.SelectedColumns()[Cell.ColumnIndex].DataTable().DataColumns()[this.SelectedColumns()[Cell.ColumnIndex].RemoteLinkDisplayColumn].DisplayText] = fT.DisplayText;
                            this.DtAdding.Rows[Cell.RowIndex].EndEdit();
                            this.DtAdding.Rows[Cell.RowIndex].AcceptChanges();
                        }
                        Cell.Value = fT.URI;

                        return;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void setLinkedColumnValues(System.Windows.Forms.DataGridViewCell Cell, System.Collections.Generic.Dictionary<string, string> UnitValues, Grid SourceGrid, ref string Error)
        {
            try
            {
                if (this.SelectedColumns()[Cell.ColumnIndex].RemoteLinks != null &&
                    this.SelectedColumns()[Cell.ColumnIndex].RemoteLinks.Count > 0)
                {
                    System.Collections.Generic.List<string> TablesToUpdate = new List<string>();
                    if (SourceGrid == Grid.Data)
                        this.DT().Rows[Cell.RowIndex].BeginEdit();
                    else if (SourceGrid == Grid.Adding)
                        this.DtAdding.Rows[0].BeginEdit();
                    foreach (DiversityWorkbench.Spreadsheet.RemoteLink RL in this.SelectedColumns()[Cell.ColumnIndex].RemoteLinks)
                    {
                        if (RL.LinkedToModule == this.SelectedColumns()[Cell.ColumnIndex].LinkedModule)
                        {
                            // setting other linked columns e.g. Family when retrieved form a source for taxonomic names
                            foreach (DiversityWorkbench.Spreadsheet.RemoteColumnBinding RCB in RL.RemoteColumnBindings)
                            {
                                if (UnitValues.ContainsKey(RCB.RemoteParameter) &&
                                    UnitValues[RCB.RemoteParameter] != null &&
                                    UnitValues[RCB.RemoteParameter].Length > 0)
                                {
                                    if (!this.DT().Columns.Contains(RCB.Column.DisplayText))
                                    {
                                    }
                                    if (SourceGrid == Grid.Data)
                                    {
                                        switch (RCB.ModeOfUpdate)
                                        {
                                            case RemoteColumnBinding.UpdateMode.Allways:
                                                this.DT().Rows[Cell.RowIndex][RCB.Column.DisplayText] = UnitValues[RCB.RemoteParameter];
                                                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._SelectedColumns)
                                                {
                                                    if (DC.Value.DisplayText == RCB.Column.DisplayText)
                                                    {
                                                        if (!TablesToUpdate.Contains(DC.Value.DataTable().Alias()))
                                                            TablesToUpdate.Add(DC.Value.DataTable().Alias());
                                                        break;
                                                    }
                                                }
                                                break;
                                            case RemoteColumnBinding.UpdateMode.IfNotEmptyAskUser:
                                                if (this.DT().Rows[Cell.RowIndex][RCB.Column.DisplayText].Equals(System.DBNull.Value) ||
                                                    this.DT().Rows[Cell.RowIndex][RCB.Column.DisplayText].ToString().Length == 0)
                                                {
                                                    this.DT().Rows[Cell.RowIndex][RCB.Column.DisplayText] = UnitValues[RCB.RemoteParameter];
                                                    foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._SelectedColumns)
                                                    {
                                                        if (DC.Value.DisplayText == RCB.Column.DisplayText)
                                                        {
                                                            if (!TablesToUpdate.Contains(DC.Value.DataTable().Alias()))
                                                                TablesToUpdate.Add(DC.Value.DataTable().Alias());
                                                            break;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    string OldValue = this.DT().Rows[Cell.RowIndex][RCB.Column.DisplayText].ToString();
                                                    string NewValue = UnitValues[RCB.RemoteParameter];
                                                    if (OldValue != NewValue)
                                                    {
                                                        string Message = "Change value for " + RCB.Column.Name + "\r\nfrom " + OldValue + "\r\nto " + NewValue + "?";
                                                        if (System.Windows.Forms.MessageBox.Show(Message, "Update " + RCB.Column.Name + "?", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                                                        {
                                                            this.DT().Rows[Cell.RowIndex][RCB.Column.DisplayText] = UnitValues[RCB.RemoteParameter];
                                                            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._SelectedColumns)
                                                            {
                                                                if (DC.Value.DisplayText == RCB.Column.DisplayText)
                                                                {
                                                                    if (!TablesToUpdate.Contains(DC.Value.DataTable().Alias()))
                                                                        TablesToUpdate.Add(DC.Value.DataTable().Alias());
                                                                    break;
                                                                }
                                                            }

                                                        }
                                                    }
                                                }
                                                break;
                                            case RemoteColumnBinding.UpdateMode.OnlyEmpty:
                                                if (this.DT().Rows[Cell.RowIndex][RCB.Column.DisplayText].Equals(System.DBNull.Value) ||
                                                    this.DT().Rows[Cell.RowIndex][RCB.Column.DisplayText].ToString().Length == 0)
                                                {
                                                    this.DT().Rows[Cell.RowIndex][RCB.Column.DisplayText] = UnitValues[RCB.RemoteParameter];
                                                    foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._SelectedColumns)
                                                    {
                                                        if (DC.Value.DisplayText == RCB.Column.DisplayText)
                                                        {
                                                            if (!TablesToUpdate.Contains(DC.Value.DataTable().Alias()))
                                                                TablesToUpdate.Add(DC.Value.DataTable().Alias());
                                                            break;
                                                        }
                                                    }

                                                }
                                                break;
                                        }
                                    }
                                    else if (SourceGrid == Grid.Adding)
                                    {
                                        this.DtAdding.Rows[0][RCB.Column.DisplayText] = UnitValues[RCB.RemoteParameter];
                                        foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this.SelectedColumns())
                                        {
                                            if (DC.Value.Type() == DataColumn.ColumnType.Data
                                                && DC.Value.DisplayText == RCB.Column.DisplayText)
                                            {
                                                DC.Value.DefaultForAdding = UnitValues[RCB.RemoteParameter];
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                    foreach (string DT in TablesToUpdate)
                    {
                        DataTable.SqlCommandTypeExecuted SqlType = DataTable.SqlCommandTypeExecuted.None;
                        this._DataTables[DT].SaveData(Cell.RowIndex, ref Error, ref SqlType);
                    }
                }
                if (UnitValues.ContainsKey("DisplayText") && UnitValues.ContainsKey("URI"))
                {
                    string DisplayText = UnitValues["DisplayText"];
                    string URI = UnitValues["URI"];
                    // setting the values according to the selected link


                    string RemoteLinkDisplayColumn = this.SelectedColumns()[Cell.ColumnIndex].DataTable().DataColumns()[this.SelectedColumns()[Cell.ColumnIndex].RemoteLinkDisplayColumn].DisplayText;
                    if (!this.DT().Columns.Contains(RemoteLinkDisplayColumn))
                        RemoteLinkDisplayColumn = this.SelectedColumns()[Cell.ColumnIndex].RemoteLinkDisplayColumn;
                    if (SourceGrid == Grid.Data)
                    {
                        if (this.DT().Columns.Contains(RemoteLinkDisplayColumn))
                            this.DT().Rows[Cell.RowIndex][RemoteLinkDisplayColumn] = UnitValues["DisplayText"];// fT.DisplayText;

                        this.DT().Rows[Cell.RowIndex].EndEdit();
                        this.DT().Rows[Cell.RowIndex].AcceptChanges();
                    }
                    else if (SourceGrid == Grid.Adding)
                    {
                        if (this.DtAdding.Columns.Contains(RemoteLinkDisplayColumn))
                        {
                            this.DtAdding.Rows[0][RemoteLinkDisplayColumn] = UnitValues["DisplayText"];// fT.DisplayText;
                        }
                        else
                            this.DtAdding.Rows[0][this.SelectedColumns()[Cell.ColumnIndex].DataTable().DataColumns()[this.SelectedColumns()[Cell.ColumnIndex].RemoteLinkDisplayColumn].DisplayText] = UnitValues["DisplayText"];// fT.DisplayText;
                        this.DtAdding.Rows[0].EndEdit();
                        this.DtAdding.Rows[0].AcceptChanges();
                        foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this.SelectedColumns())
                        {
                            if (DC.Value.Type() == DataColumn.ColumnType.Data
                                && DC.Value.DisplayText == RemoteLinkDisplayColumn)
                            {
                                DC.Value.DefaultForAdding = UnitValues["DisplayText"];
                            }
                        }

                    }
                }
                if (UnitValues.ContainsKey("URI"))
                    Cell.Value = UnitValues["URI"];// fT.URI;

                return;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setLinkedColumnValue(System.Windows.Forms.DataGridViewCell Cell, string ColumnDisplayText, string Value, Grid SourceGrid)
        {
            switch (SourceGrid)
            {
                case Grid.Adding:
                    this.DtAdding.Rows[0][ColumnDisplayText] = Value;
                    break;
                case Grid.Data:
                    this.DT().Rows[Cell.RowIndex][ColumnDisplayText] = Value;
                    break;
                case Grid.Filter:
                    this.DtFilter.Rows[0][ColumnDisplayText] = Value;
                    break;
            }
        }

        /// <summary>
        /// SQL for the retrieval of the data for known sources from module databases either on the local server or a linked server
        /// </summary>
        /// <param name="Restriction">The restriction for the query, e.g. a value in a combobox entered by a user</param>
        /// <param name="SC">The connection to the source</param>
        /// <param name="iWU"></param>
        /// <param name="ListInDatabase">If the source is a list in a module database e.g. in ScientificTerms</param>
        /// <returns></returns>
        public static string SqlForLinkedSource(string Restriction, ServerConnection SC, IWorkbenchUnit iWU, string ListInDatabase)
        {
            string SQL = "";
            try
            {
                if (SC != null)
                {
                    string Prefix = SC.DatabaseName + ".dbo.";
                    if ((SC.DatabaseName.StartsWith("DiversityCollectionCache") || SC.ModuleName == "DiversityCollectionCache") && SC.ProjectID != null)
                    {
                        string Domain = SC.Project.Substring(0, 5);
                        switch (Domain)
                        {
                            case "Names":
                                SQL = "SELECT T.TaxonName AS DisplayText, " +
                                    " T.NameURI AS URI, T.BaseURL, T.NameID AS ID " +
                                    " FROM " + Prefix + "TaxonSynonymy T  " +
                                    " WHERE  T.SourceView = '" + SC.Project + "' AND T.ProjectID = " + SC.ProjectID.ToString();
                                if (Restriction.Length > 0)
                                {
                                    SQL += " AND T.TaxonName LIKE '" + Restriction + "%'";
                                }
                                break;
                        }
                    }
                    else
                    {
                        if (SC.LinkedServer.Length > 0)
                            Prefix = "[" + SC.LinkedServer + "]." + Prefix;
                        SQL = "SELECT T." + iWU.QueryDisplayColumns()[0].DisplayColumn + " AS DisplayText, U.BaseURL + CAST(T." + iWU.QueryDisplayColumns()[0].IdentityColumn +
                            " AS VARCHAR(500)) AS URI, U.BaseURL, T." + iWU.QueryDisplayColumns()[0].IdentityColumn + " AS ID " +
                            " FROM " + Prefix + "VIEWBASEURL U, " + Prefix + iWU.QueryDisplayColumns()[0].TableName + " T ";
                        if (SC.ProjectID != null)
                        {
                            switch (SC.ModuleName)
                            {
                                case "DiversityAgents":
                                    SQL += " , " + Prefix + "AgentProject P WHERE P.AgentID = T.AgentID AND P.ProjectID = " + SC.ProjectID.ToString();
                                    break;
                                case "DiversityCollection":
                                    SQL += " , " + Prefix + "CollectionProject P WHERE P.CollectionSpecimenID = T.CollectionSpecimenID AND P.ProjectID = " + SC.ProjectID.ToString();
                                    break;
                                case "DiversityGazetteer":
                                    SQL += " , " + Prefix + "GeoProject P WHERE P.NameID = T.NameID AND P.ProjectID = " + SC.ProjectID.ToString();
                                    break;
                                case "DiversityReferences":
                                    SQL += " , " + Prefix + "ReferenceProject P WHERE P.RefID = T.RefID AND P.ProjectID = " + SC.ProjectID.ToString();
                                    break;
                                case "DiversitySamplingPlots":
                                    SQL += " , " + Prefix + "SamplingProject P WHERE P.PlotID = T.PlotID AND P.ProjectID = " + SC.ProjectID.ToString();
                                    break;
                                case "DiversityScientificTerms":
                                    SQL = "SELECT CASE WHEN TP.DisplayText IS NULL THEN '' ELSE TP.DisplayText + ' | ' END + T.DisplayText AS DisplayText, " +
                                        "U.BaseURL + CAST(T." + iWU.QueryDisplayColumns()[0].IdentityColumn +
                                        " AS VARCHAR(500)) AS URI, U.BaseURL, T." + iWU.QueryDisplayColumns()[0].IdentityColumn + " AS ID " +
                                        " FROM " + Prefix + "VIEWBASEURL U, " + Prefix + iWU.QueryDisplayColumns()[0].TableName + " T " +
                                        " INNER JOIN " + Prefix + "Term AS ET ON T.TermID = ET.TermID AND T.TerminologyID = ET.TerminologyID " +
                                        " LEFT OUTER JOIN " + Prefix + "Term AS EP " +
                                        " INNER JOIN " + Prefix + "TermRepresentation AS TP ON EP.TerminologyID = TP.TerminologyID AND EP.TermID = TP.TermID AND EP.PreferredRepresentationID = TP.RepresentationID " +
                                        " ON ET.TerminologyID = EP.TerminologyID AND ET.BroaderTermID = EP.TermID " +
                                        " WHERE  ET.IsRankingTerm = 0  AND T.TerminologyID = " + SC.ProjectID.ToString();
                                    break;
                                case "DiversityTaxonNames":
                                    SQL += " , " + Prefix + "TaxonNameProject P WHERE P.NameID = T.NameID AND P.ProjectID = " + SC.ProjectID.ToString();
                                    break;
                                case "DiversityCollectionCache":
                                    string Domain = SC.Project.Substring(0, 5);
                                    switch (Domain)
                                    {
                                        case "Names":
                                            SQL = "SELECT T.TaxonName AS DisplayText, " +
                                                " T.NameURI AS URI, T.BaseURL, T.NameID AS ID " +
                                                " FROM " + Prefix + "TaxonSynonymy T  " +
                                                " WHERE  T.SourceView = '" + SC.Project + "' AND T.ProjectID = " + SC.ProjectID.ToString();
                                            if (Restriction.Length > 0)
                                            {
                                                SQL += " AND T.TaxonName LIKE '" + Restriction + "%'";
                                            }
                                            break;
                                    }
                                    break;
                            }
                            if (SC.ModuleName != "DiversityCollectionCache")
                                SQL += " AND ";
                        }
                        else
                            SQL += " WHERE ";
                        if (SC.ModuleName != "DiversityCollectionCache")
                        {
                            SQL += " T." + iWU.QueryDisplayColumns()[0].DisplayColumn +
                                " LIKE '" + Restriction + "%'";
                        }
                        if (ListInDatabase.Length > 0)
                        {
                            foreach (DiversityWorkbench.DatabaseService D in iWU.DatabaseServices())
                            {
                                if (D.ListName == ListInDatabase)
                                {
                                    SQL += " AND " + D.RestrictionForListInDatabase;
                                    break;
                                }
                            }
                        }
                        switch (SC.ModuleName)
                        {
                            case "DiversityScientificTerms":
                                SQL += " ORDER BY TP.DisplayText, T.DisplayText";
                                break;
                            case "DiversityCollectionCache":
                                SQL += " ORDER BY DisplayText";
                                break;
                            default:
                                SQL += " ORDER BY " + iWU.QueryDisplayColumns()[0].DisplayColumn;
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return SQL;
        }

        private System.Collections.Generic.Dictionary<string, ServerConnection> _FixSourceConnections;
        private ServerConnection getFixSourceConnection(string TableAlias, string ColumnName)
        {
            try
            {
                if (this._FixSourceConnections != null && this._FixSourceConnections.ContainsKey(TableAlias + "." + ColumnName))
                    return this._FixSourceConnections[TableAlias + "." + ColumnName];
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return null;
        }

        public System.Collections.Generic.Dictionary<string, ServerConnection> FixSourceConnections()
        {
            return this._FixSourceConnections;
        }

        public void setFixSourceConnection(string TableAlias, string ColumnName, ServerConnection SC)
        {
            try
            {
                if (this._FixSourceConnections == null)
                    this._FixSourceConnections = new Dictionary<string, ServerConnection>();
                if (this._FixSourceConnections.ContainsKey(TableAlias + "." + ColumnName))
                    this._FixSourceConnections[TableAlias + "." + ColumnName] = SC;
                else
                    this._FixSourceConnections.Add(TableAlias + "." + ColumnName, SC);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Export

        public bool ExportToTextFile(string ExportFile, bool IncludeHiddenColumns)
        {
            bool OK = true;
            System.IO.StreamWriter sw = null;
            try
            {
                if (!ExportFile.EndsWith(".txt"))
                    ExportFile += ".txt";
                sw = new System.IO.StreamWriter(ExportFile, false, System.Text.Encoding.UTF8);
                string Line = "";
                // Header
                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this.SelectedColumns())
                {
                    if (!this.IncludeColumn(DC.Value, IncludeHiddenColumns))
                        continue;
                    //if (DC.Value.Type() != DataColumn.ColumnType.Data)
                    //    continue;
                    //if (!DC.Value.IsVisible && !IncludeHiddenColumns)
                    //    continue;
                    //if (Line.Length > 0) 
                    //    Line += "\t";
                    Line += DC.Value.DisplayText.Replace("\t", " ").Replace("\r\n", " ") + "\t";
                }
                sw.WriteLine(Line);
                //Data
                foreach (System.Data.DataRow R in this.DT().Rows)
                {
                    Line = "";
                    foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this.SelectedColumns())
                    {
                        if (!this.IncludeColumn(DC.Value, IncludeHiddenColumns))
                            continue;
                        //if (DC.Value.Type() != DataColumn.ColumnType.Data)
                        //    continue;
                        //if (!DC.Value.IsVisible && !IncludeHiddenColumns)
                        //    continue;
                        ////if (Line.Length > 0) 
                        //    Line += "\t";
                        Line += R[DC.Key].ToString().Replace("\t", " ").Replace("\r\n", " ") + "\t";
                    }
                    sw.WriteLine(Line);
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                OK = false;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
            return OK;
        }

        private bool IncludeColumn(DataColumn DC, bool IncludeHiddenColumns)
        {
            bool Include = true;
            if (DC.Type() == DataColumn.ColumnType.Operation || DC.Type() == DataColumn.ColumnType.Spacer)
                Include = false;
            if (!DC.IsVisible && !IncludeHiddenColumns)
                Include = false;
            if (DC.IsVisible && DC.IsHidden && !IncludeHiddenColumns)
                Include = false;
            return Include;
        }

        #endregion

        #region Map

        public bool ShowDetailsInMap = true;

        public bool ShowAllDetailsInMap = false;

        public bool KeepLastSymbol = true;

        #region Geography

        private string _GeographyColumn = "";
        private string _GeographyTableAlias = "";

        #endregion

        #region KeyFilter resp. Identifier

        private string _GeographyKeyColumn = "";
        private string _GeographyKeyTableAlias = "";

        private bool _GeographyUseKeyFilter = false;

        #region WGS84

        private string _GeographyWGS84TableAlias = "";
        public string GeographyWGS84TableAlias
        {
            get { return _GeographyWGS84TableAlias; }
            set { _GeographyWGS84TableAlias = value; }
        }

        private string _GeographyWGS84Column = "";
        public string GeographyWGS84Column
        {
            get { return _GeographyWGS84Column; }
            set { _GeographyWGS84Column = value; }
        }

        #endregion

        #region UnitGeo

        private string _GeographyUnitGeoTableAlias = "";
        public string GeographyUnitGeoTableAlias
        {
            get { return _GeographyUnitGeoTableAlias; }
            set { _GeographyUnitGeoTableAlias = value; }
        }

        private string _GeographyUnitGeoColumn = "";
        public string GeographyUnitGeoColumn
        {
            get { return _GeographyUnitGeoColumn; }
            set { _GeographyUnitGeoColumn = value; }
        }

        #endregion

        public bool GeographyUseKeyFilter
        {
            get { return _GeographyUseKeyFilter; }
            set { _GeographyUseKeyFilter = value; }
        }

        private System.Collections.Generic.SortedDictionary<int, MapFilter> _MapFilterList;

        public System.Collections.Generic.SortedDictionary<int, MapFilter> MapFilterList
        {
            get
            {
                if (this._MapFilterList == null)
                    this._MapFilterList = new SortedDictionary<int, MapFilter>();
                return _MapFilterList;
            }
            //set { _MapFilterList = value; }
        }

        public void AddMapFilter(MapFilter Filter)
        {
            int Position = this.MapFilterList.Count + 1;
            Filter.setPosition(Position);
            this.MapFilterList.Add(Position, Filter);
        }

        public void RemoveMapFilter(int Position)
        {
            this.MapFilterList.Remove(Position);
        }

        public void MoveFilterUp(int Position)
        {
            System.Collections.Generic.SortedDictionary<int, MapFilter> TempMapFilterList = new SortedDictionary<int, MapFilter>();
            foreach (System.Collections.Generic.KeyValuePair<int, MapFilter> KV in this.MapFilterList)
            {
                TempMapFilterList.Add(KV.Key, KV.Value);
            }
            System.Collections.Generic.Dictionary<int, int> D = new Dictionary<int, int>();
            foreach (System.Collections.Generic.KeyValuePair<int, MapFilter> KV in this.MapFilterList)
            {
                D.Add(KV.Key, KV.Key);
                if (KV.Key == Position)
                {
                    int NewPosition = Position - 1;
                    while (!D.ContainsKey(NewPosition) && NewPosition > 1)
                    {
                        NewPosition--;
                    }
                    D[Position] = NewPosition;
                    D[NewPosition] = Position;
                }
            }
            this.MapFilterList.Clear();
            foreach (System.Collections.Generic.KeyValuePair<int, int> KV in D)
            {
                MapFilterList.Add(KV.Value, TempMapFilterList[KV.Key]);
            }
        }

        public void MoveFilterDown(int Position)
        {
        }

        public int? GeographyKeyColumnPosition()
        {
            int i = this.GeographyObjectPosition(this.GeographyKeyColumn, this.GeographyKeyTableAlias);
            if (i > -1)
                return i;
            else
                return null;
        }

        public string GeographyKeyColumn
        {
            get { return _GeographyKeyColumn; }
            set { _GeographyKeyColumn = value; }
        }

        public string GeographyKeyTableAlias
        {
            get { return _GeographyKeyTableAlias; }
            set
            {
                _GeographyKeyTableAlias = value;
                if (_GeographyKeyTableAlias.Length == 0)
                    _GeographyKeyColumn = "";
            }
        }

        #endregion

        #region Symbol

        /// Replaced by a value of "" resp. NULL which appears anyway
        //public bool UserMapSymbolForMissing = false;
        //private double _MapSymbolDefaultSize = 1.0;

        private MapSymbol _MapSymbolForMissing;
        public MapSymbol MapSymbolForMissing
        {
            get
            {
                if (_MapSymbolForMissing == null)
                    this._MapSymbolForMissing = new MapSymbol("", 1.0, "Circle");
                return _MapSymbolForMissing;
            }
            set { _MapSymbolForMissing = value; }
        }

        private string _GeographySymbolColumn = "";
        private string _GeographySymbolTableAlias = "";

        private string _GeographySymbolSizeColumn = "";
        private string _GeographySymbolSizeTableAlias = "";
        //private double _GeographySymbolSize = 0.0;
        private double _GeographySymbolSize = 1.0;
        private bool _GeographySymbolSizeLinkedToColumn = false;

        private string _GeographySymbolSourceTable = "";
        private string _GeographySymbolSourceColumn = "";
        private string _GeographySymbolSourceRestriction = "";


        private System.Collections.Generic.Dictionary<string, MapSymbol> _MapSymbols;
        public System.Collections.Generic.Dictionary<string, MapSymbol> MapSymbols()
        {
            if (this._MapSymbols == null)
                this._MapSymbols = new Dictionary<string, MapSymbol>();
            return this._MapSymbols;
        }

        public void ResetMapSymbols() { this._MapSymbols = null; }

        public bool GeographySymbolSizeLinkedToColumn
        {
            get { return _GeographySymbolSizeLinkedToColumn; }
            set { _GeographySymbolSizeLinkedToColumn = value; }
        }

        public double GeographySymbolSize
        {
            get { return _GeographySymbolSize; }
            set { _GeographySymbolSize = value; }
        }

        public double GeographyStrokeThickness
        {
            get
            {
                if (_GeographySymbolSize < 1)
                    return 1;
                else return _GeographySymbolSize;
            }
        }

        public string GeographySymbolSizeTableAlias
        {
            get { return _GeographySymbolSizeTableAlias; }
            set { _GeographySymbolSizeTableAlias = value; }
        }

        public string GeographySymbolSizeColumn
        {
            get { return _GeographySymbolSizeColumn; }
            set { _GeographySymbolSizeColumn = value; }
        }

        public bool GeographySymbolSizeCanBeLinkedToColumnValue()
        {
            if (GeographySymbolSize > 0 && this.GeographySymbolSizeColumn.Length > 0 && this.GeographySymbolSizeTableAlias.Length > 0)
                return true;
            else return false;
        }

        public int? GeographySymbolColumnPosition()
        {
            int i = this.GeographyObjectPosition(this.GeographySymbolColumn, this.GeographySymbolTableAlias);
            if (i > -1)
                return i;
            else
                return null;
        }

        public string GeographySymbolColumn
        {
            get { return _GeographySymbolColumn; }
            set { _GeographySymbolColumn = value; }
        }

        public string GeographySymbolTableAlias
        {
            get { return _GeographySymbolTableAlias; }
            set
            {
                _GeographySymbolTableAlias = value;
                if (_GeographySymbolTableAlias.Length == 0)
                    _GeographySymbolColumn = "";
            }
        }

        public string GeographySymbolSourceTable
        {
            get { return _GeographySymbolSourceTable; }
            set { _GeographySymbolSourceTable = value; }
        }

        public string GeographySymbolSourceColumn
        {
            get { return _GeographySymbolSourceColumn; }
            set { _GeographySymbolSourceColumn = value; }
        }

        public string GeographySymbolSourceRestriction
        {
            get { return _GeographySymbolSourceRestriction; }
            set { _GeographySymbolSourceRestriction = value; }
        }

        #endregion

        #region Color

        private string _GeographyColorColumn = "";
        private string _GeographyColorTableAlias = "";
        private byte _GeographyTransparency = 255;
        private string _GeographyMap = "";

        public string GeographyMap
        {
            get { return _GeographyMap; }
            set { _GeographyMap = value; }
        }

        private System.Collections.Generic.List<MapColor> _MapColors;
        public System.Collections.Generic.List<MapColor> MapColors()
        {
            if (this._MapColors == null)
                this._MapColors = new List<MapColor>();
            return this._MapColors;
        }

        public MapColor getMapColor(string Value)
        {
            double dVal;
            double dLow;
            double dUpp;
            foreach (MapColor MC in this.MapColors())
            {
                switch (MC.Operator)
                {
                    case "=":
                        if (Value == MC.LowerValue)
                            return MC;
                        break;
                    case "<>":
                        if (Value != MC.LowerValue)
                            return MC;
                        break;
                    case "∅":
                        if (Value.Length == 0)
                            return MC;
                        break;
                    default:
                        if (double.TryParse(Value, out dVal) && double.TryParse(MC.LowerValue, out dLow))
                        {
                            switch (MC.Operator)
                            {
                                case "<":
                                    if (dVal < dLow)
                                        return MC;
                                    break;
                                case "<=":
                                    if (dVal <= dLow)
                                        return MC;
                                    break;
                                case ">":
                                    if (dVal > dLow)
                                        return MC;
                                    break;
                                case ">=":
                                    if (dVal >= dLow)
                                        return MC;
                                    break;
                                case "-":
                                    if (double.TryParse(MC.UpperValue, out dUpp) && dVal >= dLow && dVal <= dUpp)
                                        return MC;
                                    break;
                            }
                        }
                        break;
                }
            }
            //MapColor Mwhite = new MapColor("#FFFFFFFF", "=", "", "");
            MapColor M = new MapColor("#FF000000", "=", "", "");
            return M;
        }

        public string GeographyColorColumn
        {
            get { return _GeographyColorColumn; }
            set { _GeographyColorColumn = value; }
        }

        public string GeographyColorTableAlias
        {
            get { return _GeographyColorTableAlias; }
            set
            {
                _GeographyColorTableAlias = value;
                if (_GeographyColorTableAlias.Length == 0)
                    _GeographyColorColumn = "";
            }
        }

        public int? GeographyColorColumnPosition()
        {
            int i = this.GeographyObjectPosition(this.GeographyColorColumn, this.GeographyColorTableAlias);
            if (i > -1)
                return i;
            else
                return null;
        }

        #endregion

        #region Transparency

        public byte GeographyTransparency
        {
            get { return _GeographyTransparency; }
            set
            {
                if ((int)value > -1 && (int)value < 256)
                    _GeographyTransparency = value;
            }
        }

        #endregion

        #region Geography

        public string GeographyColumn
        {
            get { return _GeographyColumn; }
            set { _GeographyColumn = value; }
        }

        public string GeographyTableAlias
        {
            get { return _GeographyTableAlias; }
            set
            {
                _GeographyTableAlias = value;
                if (_GeographyTableAlias.Length == 0)
                    _GeographyColumn = "";
            }
        }

        public int? GeographyColumnPosition()
        {
            int i = this.GeographyObjectPosition(this._GeographyColorColumn, this._GeographyColorTableAlias);
            if (i > -1)
                return i;
            else
                return null;
        }

        #endregion

        #region Common

        private int GeographyObjectPosition(string Column, string TableAlias)
        {
            int i = -1;
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this.SelectedColumns())
            {
                if (DC.Value.DataTable().Alias() == TableAlias &&
                    DC.Value.Column.Name == Column)
                {
                    i = DC.Key;
                    break;
                }
            }
            return i;
        }

        #endregion

        #region Evaluation

        private string _EvaluationGazetteer = "";
        public string EvaluationGazetteer() { return this._EvaluationGazetteer; }
        public void EvaluationSetGazetteer(string Gazetteer) { this._EvaluationGazetteer = Gazetteer; }

        private System.Collections.Generic.Dictionary<string, int> _EvaluationSymbolValueSequence;
        public System.Collections.Generic.Dictionary<string, int> EvaluationSymbolValueSequence()
        {
            if (this._EvaluationSymbolValueSequence == null)
                this._EvaluationSymbolValueSequence = new Dictionary<string, int>();
            return this._EvaluationSymbolValueSequence;
        }

        public void EvaluationClearSymbols()
        {
            if (this._EvaluationSymbolValueSequence != null)
                this._EvaluationSymbolValueSequence.Clear();
        }

        #endregion

        #endregion

        #region Geometry

        private string _GeometryColumn = "";
        private string _GeometryTableAlias = "";
        private string _GeometryyPlan = "";

        public string GeometryPlan
        {
            get { return _GeometryyPlan; }
            set { _GeometryyPlan = value; }
        }

        #endregion

    }
}
