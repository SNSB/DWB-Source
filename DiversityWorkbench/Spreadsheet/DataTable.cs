using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Spreadsheet
{
    public class DataTable : Data.Table
    {

        public enum SqlCommandTypeExecuted { None, Insert, Update, Delete, PKcollision, MissingRowGUID, NoMatch }

        #region Properties

        #region Display text

        private string _DisplayText;

        /// <summary>
        /// Display text for the table corresponds to Entity value if Entity exists and if table is Root or Target
        /// otherwise its kept separate as it may vary e.g. for parallel tables like WGS84, MTB which refer to the same table
        /// </summary>
        public string DisplayText
        {
            get
            {
                if (this._DisplayText == null)
                {
                    //if (DiversityWorkbench.Entity.EntityTablesExist)
                    //{
                    //    System.Collections.Generic.Dictionary<string, string> Entity = new Dictionary<string, string>();
                    //    Entity = DiversityWorkbench.Entity.EntityInformation(this.Name);
                    //    if (Entity["DisplayTextOK"] == "True")
                    //        this._DisplayText = Entity["DisplayText"];
                    //    else
                    //        this._DisplayText = this._Name;
                    //}
                    //else
                    return this._Name;
                }
                return _DisplayText;
            }
            set
            {
                _DisplayText = value;
                //if (this._Type == TableType.Root || this._Type == TableType.Target)
                //{
                //    if (DiversityWorkbench.Entity.EntityTablesExist)
                //        DiversityWorkbench.Entity.setEntityRepresentation(this.Name, value, Entity.EntityInformationField.DisplayText, true);
                //}
            }
        }

        public void setDisplayTextByRestriction()
        {
            this.DisplayText = "";
            foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in this.DataColumns())
            {
                if (DC.Value.RestrictionValue != null && DC.Value.RestrictionValue.Length > 0)
                {
                    if (this.DisplayText.Length > 0) this.DisplayText += " - ";
                    if (DC.Value.LookupSource != null)
                    {
                        System.Data.DataRow[] R = DC.Value.LookupSource.Select("Value = '" + DC.Value.RestrictionValue + "'", "");
                        this.DisplayText += R[0]["Display"].ToString();
                    }
                    else
                        this.DisplayText += DC.Value.RestrictionValue;
                }
            }
            Spreadsheet.Sheet.RebuildNeeded = true;
        }

        #endregion

        #region Description

        /// <summary>
        /// A text describing the table, not changed after construction
        /// </summary>
        private string _Description;
        public string Description()
        {
            if (this._Description == null || this._Description.Length == 0)
            {
                if (this._DisplayText != null && this._DisplayText.Length > 0)
                    this._Description = this._DisplayText;
                else
                    this._Description = this.Name;
            }
            return this._Description;
        }
        public void SetDescription(string Description) { this._Description = Description; }

        #endregion

        #region Images

        System.Collections.Generic.Dictionary<string, System.Drawing.Image> _Images;
        public void AddImage(string RestrictionValue, System.Drawing.Image Image)
        {
            if (this._Images == null)
                this._Images = new Dictionary<string, System.Drawing.Image>();
            if (!this._Images.ContainsKey(RestrictionValue))
                this._Images.Add(RestrictionValue, Image);
        }
        public System.Drawing.Image TableImage()
        {
            if (this.RestrictionColumns.Count == 1)
            {
                string Restriction = this.DataColumns()[this.RestrictionColumns[0]].RestrictionValue;
                if (Restriction != null && this._Images != null && this._Images.ContainsKey(Restriction))
                    return this._Images[Restriction];
                else
                {
                    if (this._TemplateAlias != null && this._TemplateAlias.Length > 0 && Restriction != null)
                        return this._Sheet.DataTables()[this._TemplateAlias].TableImage(Restriction);
                    else if (this._TemplateAlias != null && this._TemplateAlias.Length > 0 && this._Images.Count == 1 && this._Images.ContainsKey(""))
                        return this._Images[""];
                    else
                        return (System.Drawing.Image)DiversityWorkbench.Properties.Resources.NULL;
                }
            }
            else
            {
                if (this._Images != null && this._Images.Count == 1 && this._Images.ContainsKey(""))
                    return this._Images[""];
                else
                    return (System.Drawing.Image)DiversityWorkbench.Properties.Resources.NULL;
            }
        }

        public System.Drawing.Image TableImage(string Restriction)
        {
            if (this._Images != null && this._Images.ContainsKey(Restriction))
                return this._Images[Restriction];
            else
            {
                if (this._Images != null && this._Images.Count == 1)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, System.Drawing.Image> KV in this._Images)
                    {
                        return KV.Value;
                    }
                }
                else
                    return (System.Drawing.Image)DiversityWorkbench.Properties.Resources.NULL;
            }
            return (System.Drawing.Image)DiversityWorkbench.Properties.Resources.NULL;
            //if (this.RestrictionColumns.Count == 1)
            //{
            //    if (this._TemplateAlias != null && this._TemplateAlias.Length > 0)
            //    {
            //        return this._Sheet.DataTables()[this._TemplateAlias].TableImage(Restriction);
            //    }
            //    else
            //        return (System.Drawing.Image)DiversityWorkbench.Properties.Resources.NULL;
            //}
            //    else
            //        return (System.Drawing.Image)DiversityWorkbench.Properties.Resources.NULL;
        }

        #endregion

        #region Alias

        private string _Alias;
        public string Alias()
        {
            return this._Alias;
        }

        private string _TemplateAlias;

        public string TemplateAlias
        {
            get { return _TemplateAlias; }
            set
            {
                _TemplateAlias = value;
            }
        }

        #endregion

        private bool _IsRequired;
        public bool IsRequired
        {
            get { return _IsRequired; }
            set { _IsRequired = value; }
        }

        private string _View;
        public string View
        {
            get { return _View; }
            set { _View = value; }
        }

        private string _SqlRestrictionClause;

        public string SqlRestrictionClause
        {
            get { return _SqlRestrictionClause; }
            set { _SqlRestrictionClause = value; }
        }

        #region Columns

        //private System.Collections.Generic.List<string> _DisplayedColumns;
        public void setDisplayedColumns(System.Collections.Generic.List<string> Columns)
        {
            foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in this.DataColumns())
            {
                if (Columns.Contains(DC.Key))
                    DC.Value.IsVisible = true;
                else DC.Value.IsVisible = false;
            }
            //this._DisplayedColumns = Columns; 
        }

        public System.Collections.Generic.List<string> DisplayedColumns()
        {
            System.Collections.Generic.List<string> _DisplayedColumns = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in this.DataColumns())
                if (DC.Value.IsVisible)
                    _DisplayedColumns.Add(DC.Key);
            return _DisplayedColumns;
        }

        private System.Collections.Generic.Dictionary<string, DataColumn> _DataColumns;
        public System.Collections.Generic.Dictionary<string, DataColumn> DataColumns()
        {
            if (this._DataColumns == null)
            {
                this._DataColumns = new Dictionary<string, DataColumn>();
                int i = 1;
                foreach (System.Collections.Generic.KeyValuePair<string, Data.Column> KV in this.Columns)
                {
                    DataColumn C = new DataColumn(this, KV.Value);
                    // setting the default ordinal position
                    C.OrdinalPosition = i;
                    i++;

                    this._DataColumns.Add(KV.Key, C);
                    if (this._TemplateAlias != null && this._TemplateAlias != this._Alias)
                    {
                        if (this._Sheet.DataTables()[this._TemplateAlias].DataColumns()[C.Name].SqlLookupSource != null
                            && this._Sheet.DataTables()[this._TemplateAlias].DataColumns()[C.Name].SqlLookupSource.Length > 0)
                            C.SqlLookupSource = this._Sheet.DataTables()[this._TemplateAlias].DataColumns()[C.Name].SqlLookupSource;
                    }
                }
            }
            return this._DataColumns;
        }

        public void AddColumn(string ColumnName, string SQL, DataColumn.RetrievalType RetrievalType)
        {
            if (!this.DataColumns().ContainsKey(ColumnName))
            {
                DiversityWorkbench.Data.Column C = new Data.Column(ColumnName, null);
                DiversityWorkbench.Spreadsheet.DataColumn DC = new DataColumn(this, C);
                DC.SqlForColumn = SQL;
                DC.DataRetrievalType = RetrievalType;
                System.Collections.Generic.SortedDictionary<int, string> ColumnDict = new SortedDictionary<int, string>();
                foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> KV in this.DataColumns())
                {
                    ColumnDict.Add(KV.Value.OrdinalPosition, KV.Key);
                }
                DC.OrdinalPosition = ColumnDict.First().Key - 1;
                this._DataColumns.Add(ColumnName, DC);
            }
        }

        public void setColumnOrdinalPosition(string Column, int Position)
        {
            System.Collections.Generic.SortedDictionary<int, string> ColumnDict = new SortedDictionary<int, string>();
            ColumnDict.Add(Position, Column);
            this.DataColumns()[Column].OrdinalPosition = Position;
            foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> KV in this.DataColumns())
            {
                if (KV.Key == Column)
                    continue;
                if (!ColumnDict.ContainsKey(KV.Value.OrdinalPosition))
                    ColumnDict.Add(KV.Value.OrdinalPosition, KV.Key);
                else
                {
                }
            }
        }

        #endregion

        #region Parent

        /// <summary>
        /// if available, the name of the parent table
        /// </summary>
        private DataTable _ParentTable;
        public void setParentTable(DataTable DT) { this._ParentTable = DT; }
        public void setParentTable(string TableAlias)
        {
            if (this._Sheet.DataTables().ContainsKey(TableAlias))
            {
                this._ParentTable = this._Sheet.DataTables()[TableAlias];
                if (this.Type() == TableType.Parallel)
                    this.setColorBack(this._ParentTable._ColorBack);
            }
        }
        public DataTable ParentTable() { return this._ParentTable; }

        #endregion

        /// <summary>
        /// the list of columns used to order the data
        /// </summary>
        private System.Collections.Generic.List<string> _OrderColumns;
        ///// <summary>
        ///// if the data should be restricted to the first dataset
        ///// </summary>
        //private bool _RestrictToTop1;
        ///// <summary>
        ///// The colums used to select the first row
        ///// </summary>
        //private System.Collections.Generic.List<string> _OrderColumnsForRestrictionToTop1;

        ///// <summary>
        ///// if the table can have multiple lines within the parent table, e.g. many specimen in an event
        ///// </summary>
        //private bool _EnableAdding = false;
        //public void EnableAdding(bool Enable)
        //{ this._EnableAdding = Enable; }

        /// <summary>
        /// Project: the table containing the project infos for the restriction to the project, hidden, e.g. CollectionProject
        /// Root: Starting points that are needed and always present and used for sorting the data, e.g. CollectionSpecimen
        /// Required: A table that must be included, e.g. TK25
        /// RootHidden: Starting points that are needed and always present but hidden, e.g. CollectionSpecimen for observations
        /// Target: the table where the lines are unique and further adding for dependent tables occur via parallelity, e.g. IdentificationUnit
        /// Parallel: Tables for which more than 1 dataset are displayed parallel, e.g.  CollectionEventLocalisation, Analysis
        /// Single: Data restricted to one line, e.g. first CollectionAgent, last identification
        /// </summary>
        public enum TableType
        {
            Root, RootHidden, Parallel, Target, Project, Single, Referencing, Lookup, InsertOnly
        }

        private TableType _Type = TableType.Parallel;
        public TableType Type() { return this._Type; }

        private string _FilterOperator = "";

        public string FilterOperator
        {
            get { return _FilterOperator; }
            set { _FilterOperator = value; }
        }

        private Sheet _Sheet;
        public Sheet Sheet() { return this._Sheet; }

        #region Permissions

        private bool? _AllowAdding;
        private bool? _AllowDelete;
        private bool? _AllowUpdate;

        public bool AllowAdding()
        {
            if (this._AllowAdding == null)
            {
                this._AllowAdding = DiversityWorkbench.Forms.FormFunctions.Permissions(this.Name, "INSERT");
            }
            return (bool)_AllowAdding;
        }

        public bool AllowDelete()
        {
            if (this._AllowDelete == null)
            {
                this._AllowDelete = DiversityWorkbench.Forms.FormFunctions.Permissions(this.Name, "DELETE");
            }
            return (bool)_AllowDelete;
        }

        public bool AllowUpdate()
        {
            if (this._AllowUpdate == null)
            {
                this._AllowUpdate = DiversityWorkbench.Forms.FormFunctions.Permissions(this.Name, "UPDATE");
            }
            return (bool)_AllowUpdate;
        }

        #endregion

        #endregion

        #region Construction

        public DataTable(string Alias, string Name, string Display, TableType Type, ref Sheet Sheet)
        {
            this._Alias = Alias;
            this._Name = Name;
            if (Display.Length > 0)
            {
                this._DisplayText = Display;
                this._Description = this._DisplayText;
            }
            this._Type = Type;
            if (this.Type() == TableType.Lookup)
                this._ColorFont = ColorLookup();
            //else if (this.Type() == TableType.Parallel)
            //{

            //}
            this._ConnectionString = DiversityWorkbench.Settings.ConnectionString;
            this.FindColumnsWithForeignRelations();
            this.SetLookupListsForEnums();
            //this.SetLookupLists();
            this._Sheet = Sheet;
        }

        public string GetDataTableParallel(DataTable OriginalTable)
        {
            string Alias = OriginalTable.Alias();
            if (OriginalTable.TemplateAlias != null && OriginalTable.TemplateAlias.Length > 0)
                Alias = OriginalTable.TemplateAlias;
            bool AliasFound = false;
            int i = 1;
            while (!AliasFound)
            {
                if (!_Sheet.DataTables().ContainsKey(Alias + "_" + i.ToString()))
                {
                    Alias = Alias + "_" + i.ToString();
                    AliasFound = true;
                }
                i++;
            }
            DiversityWorkbench.Spreadsheet.DataTable T = new DiversityWorkbench.Spreadsheet.DataTable(Alias, OriginalTable.Name, OriginalTable.DisplayText, OriginalTable.Type(), ref _Sheet);
            T._ColorBack = OriginalTable._ColorBack;
            T._ConnectionString = OriginalTable._ConnectionString;
            T._ParentTable = OriginalTable._ParentTable;
            T._Description = OriginalTable.Description();
            if (OriginalTable._SqlRestrictionClause != null)
                T._SqlRestrictionClause = OriginalTable._SqlRestrictionClause.Replace(OriginalTable.TemplateAlias, Alias);
            if (OriginalTable._TemplateAlias != null)
                T._TemplateAlias = OriginalTable._TemplateAlias;
            if (OriginalTable._Images != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, System.Drawing.Image> KV in OriginalTable._Images)
                {
                    T.AddImage(KV.Key, KV.Value);
                }
            }
            else if (OriginalTable.TemplateAlias != null && OriginalTable.TemplateAlias.Length > 0 && this._Sheet.DataTables()[OriginalTable.TemplateAlias]._Images != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, System.Drawing.Image> KV in this._Sheet.DataTables()[OriginalTable.TemplateAlias]._Images)
                {
                    T.AddImage(KV.Key, KV.Value);
                }
            }
            foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in T.DataColumns())
            {
                DC.Value.IsRestrictionColumn = OriginalTable.DataColumns()[DC.Key].IsRestrictionColumn;
                DC.Value.setRemoteLinks(
                    OriginalTable.DataColumns()[DC.Key].RemoteLinkIsOptional,
                    OriginalTable.DataColumns()[DC.Key].RemoteLinkDecisionColmn,
                    OriginalTable.DataColumns()[DC.Key].RemoteLinkDisplayColumn,
                    OriginalTable.DataColumns()[DC.Key].RemoteLinks,
                    OriginalTable.DataColumns()[DC.Key].TypeOfLink);
                DC.Value.SqlLookupSource = OriginalTable.DataColumns()[DC.Key].SqlLookupSource;
                // Exclusion of already available restrictions
                if (DC.Value.SqlLookupSource != null)
                {
                    string WhereClause = "";
                    foreach (System.Collections.Generic.KeyValuePair<string, DataTable> KV in this.Sheet().DataTables())
                    {
                        if (KV.Value.DisplayedColumns().Count == 0)
                            continue;
                        if (KV.Value.Name == this.Name)
                        {
                            foreach (string RC in KV.Value.RestrictionColumns)
                            {
                                if (KV.Value.DataColumns()[RC].RestrictionValue == null || KV.Value.DataColumns()[RC].RestrictionValue.Length == 0)
                                    continue;
                                if (WhereClause.Length > 0)
                                    WhereClause += " AND ";
                                WhereClause += RC + " <> '" + KV.Value.DataColumns()[RC].RestrictionValue + "' ";
                            }
                        }
                    }
                    if (WhereClause.Length > 0)
                    {
                        string SQL = DC.Value.SqlLookupSource;
                        if (SQL.IndexOf(" WHERE ") > -1)
                        {
                            SQL = SQL.Substring(0, SQL.IndexOf(" WHERE ")) + " WHERE " + WhereClause + " AND " + SQL.Substring(SQL.IndexOf(" WHERE ") + " WHERE ".Length);
                        }
                        else if (SQL.IndexOf(" ORDER BY ") > -1)
                        {
                            SQL = SQL.Substring(0, SQL.IndexOf(" ORDER BY ")) + " WHERE " + WhereClause + " " + SQL.Substring(SQL.IndexOf(" ORDER BY "));
                        }
                        else
                            SQL += " WHERE " + WhereClause;
                        DC.Value.SqlLookupSource = SQL;
                    }
                }
                DC.Value.TypeOfLink = OriginalTable.DataColumns()[DC.Key].TypeOfLink;
                DC.Value.FilterExclude = OriginalTable.DataColumns()[DC.Key].FilterExclude;
                DC.Value.SqlQueryForDefaultForAdding = OriginalTable.DataColumns()[DC.Key].SqlQueryForDefaultForAdding;
                DC.Value.DataRetrievalType = OriginalTable.DataColumns()[DC.Key].DataRetrievalType;
            }
            T.FindColumnsWithForeignRelations();
            _Sheet.AddDataTable(T);
            return Alias;
        }

        #endregion

        private void SetLookupListsForEnums()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in this.DataColumns())
            {
                if (DC.Value.Column.ForeignRelations.Count == 1 &&
                    DC.Value.Column.ForeignRelationTable.EndsWith("_Enum"))
                {
                    string SQL = "SELECT NULL AS Value, NULL AS Display  " +
                    "UNION " +
                    "SELECT Code AS Value, DisplayText AS Display " +
                    "FROM " + DC.Value.Column.ForeignRelationTable + " WHERE DisplayEnable = 1 ORDER BY Display";
                    DC.Value.SqlLookupSource = SQL;
                }
            }
        }

        #region Restriction

        //private System.Collections.Generic.List<string> _RestrictionColumns;

        public System.Collections.Generic.List<string> RestrictionColumns
        {
            get
            {
                System.Collections.Generic.List<string> _RestrictionColumns = new List<string>();
                foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in this.DataColumns())
                {
                    if (DC.Value.IsRestrictionColumn)
                        _RestrictionColumns.Add(DC.Key);
                }
                return _RestrictionColumns;
            }
            set
            {
                foreach (string s in value)
                {
                    this.DataColumns()[s].IsRestrictionColumn = true;
                }
                //_RestrictionColumns = value; 
            }
        }

        //private string _RestrictionColumn;

        //public string RestrictionColumn
        //{
        //    get { return _RestrictionColumn; }
        //    set { _RestrictionColumn = value; }
        //}

        //private System.Data.DataTable _RestrictionColumnSource;
        ///// <summary>
        ///// Table with 2 columns: Value and Display
        ///// </summary>
        //public System.Data.DataTable RestrictionColumnSource
        //{
        //    get { return _RestrictionColumnSource; }
        //    set { _RestrictionColumnSource = value; }
        //}

        //private System.Collections.Generic.Dictionary<string, string> _RestrictionColumns;
        //public void setRestrictions(System.Collections.Generic.Dictionary<string, string> Restrictions)
        //{
        //    this._RestrictionColumns = Restrictions;
        //}
        //public System.Collections.Generic.Dictionary<string, string> Restrictions()
        //{ return this._RestrictionColumns; }

        #endregion

        #region Filter

        public System.Collections.Generic.List<string> FilterColumns
        {
            get
            {
                System.Collections.Generic.List<string> _FilterColumns = new List<string>();
                foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in this.DataColumns())
                {
                    if (DC.Value.FilterValue != null && DC.Value.FilterValue.Length > 0)
                    {
                        bool ColumnIsShown = false;
                        foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> SC in this._Sheet.SelectedColumns())
                        {
                            if (SC.Value.DisplayText == DC.Value.DisplayText)
                            {
                                ColumnIsShown = true;
                                break;
                            }
                        }
                        if (ColumnIsShown)
                            _FilterColumns.Add(DC.Key);
                        else
                        {
                        }
                    }
                }
                return _FilterColumns;
            }
        }

        #endregion

        #region Handling data

        #region Adding

        public bool CheckAddingPrerequisite(ref string Message)
        {
            bool OK = true;
            if (this.AllowAdding())
            {
                bool AnyValuesAreGiven = false;
                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                {
                    if (DC.Value.DataTable().Alias() == this.Alias()
                        && !this._Sheet.DtAdding.Rows[0][DC.Key].Equals(System.DBNull.Value)
                        && this._Sheet.DtAdding.Rows[0][DC.Key].ToString().Length > 0)
                    {
                        AnyValuesAreGiven = true;
                        break;
                    }
                }
                if (AnyValuesAreGiven)
                {
                    if (!this.AllNotNullValuesForAddingProvided(ref Message))
                        return false;
                    foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                    {
                        if (DC.Value.DataTable().ParentTable() != null
                            && DC.Value.DataTable().ParentTable().Alias() == this.Alias())
                        {
                            DC.Value.DataTable().CheckAddingPrerequisite(ref Message);
                        }
                    }
                }
            }
            else
            {
                OK = false;
                Message = "You do not have the permission to add data to the table " + this.Name;
            }
            return OK;
        }

        public bool CheckAddingPrerequisite(int? SelectedRowIndex, ref string Message)
        {
            bool OK = true;
            if (this.AllowAdding())
            {

                bool AnyValuesAreGiven = false;
                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                {
                    if (DC.Value.DataTable().Alias() == this.Alias()
                        && !this._Sheet.DtAdding.Rows[0][DC.Key].Equals(System.DBNull.Value)
                        && this._Sheet.DtAdding.Rows[0][DC.Key].ToString().Length > 0)
                    {
                        AnyValuesAreGiven = true;
                        break;
                    }
                }
                if (AnyValuesAreGiven)
                {
                    if (!this.AllNotNullValuesForAddingProvided(SelectedRowIndex, ref Message))
                        return false;
                    foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                    {
                        if (DC.Value.DataTable().ParentTable() != null
                            && DC.Value.DataTable().ParentTable().Alias() == this.Alias())
                        {
                            DC.Value.DataTable().CheckAddingPrerequisite(SelectedRowIndex, ref Message);
                        }
                    }
                }
            }
            else
            {
                OK = false;
                Message = "You do not have the permission to add data to the table " + this.Name;
            }
            return OK;
        }

        /// <summary>
        /// Replenish the data in a row from defaults, parent data etc.
        /// </summary>
        /// <param name="Row">The row that should be replenished</param>
        private void ReplenishDataInRow(ref System.Data.DataRow Row)
        {
        }

        private bool AllNotNullValuesForAddingProvided(ref string Message)
        {
            bool OK = true;
            foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in this.DataColumns())
            {
                if (!DC.Value.Column.IsNullable)
                {
                    if (DC.Value.Column.ColumnDefault != null)
                        continue;
                    if (DC.Value.DefaultForAdding != null)
                        continue;
                    if (DC.Value.Column.IsIdentity)
                        continue;
                    if (DC.Value.Column.ForeignRelations.ContainsValue(DC.Value.Name) // Has a foreign relation
                        && this.PrimaryKeyColumnList.Contains(DC.Value.Name) // Part of the PK
                        && DC.Value.Column.ForeignRelations.ContainsKey(this.ParentTable().Name))
                    {
                        if (this.ParentTable() != null &&
                            this.ParentTable().DataColumns().ContainsKey(DC.Key) &&
                            this.ParentTable().DataColumns()[DC.Key].Column.IsIdentity)
                            continue;
                        if (this.ParentTable() != null &&
                            this.ParentTable().ParentTable() != null &&
                            this.ParentTable().ParentTable().DataColumns().ContainsKey(DC.Key) &&
                            this.ParentTable().ParentTable().DataColumns()[DC.Key].Column.IsIdentity)
                            continue;
                        bool ValuePresent = false;
                        foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DCparent in this.ParentTable().DataColumns())
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, string> FK in DC.Value.Column.ForeignRelations)
                            {
                                if (FK.Key == DC.Value.DataTable().Name &&
                                    FK.Value == DCparent.Key &&
                                    DCparent.Value.IsIdentity)
                                {
                                    ValuePresent = true;
                                    break;
                                }
                            }
                            if (ValuePresent)
                                break;
                        }
                        if (ValuePresent)
                            continue;
                    }
                    if (Message.Length > 0) Message += ". ";
                    Message += "No value for the column " + DC.Value.DisplayText + " is provided";
                    OK = false;
                    break;
                }
            }
            return OK;
        }

        private bool AllNotNullValuesForAddingProvided(int? SelectedRowIndex, ref string Message)
        {
            bool OK = true;
            foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in this.DataColumns())
            {
                if (!DC.Value.Column.IsNullable)
                {
                    if (DC.Value.Column.ColumnDefault != null)
                        continue;
                    if (DC.Value.DefaultForAdding != null)
                        continue;
                    if (DC.Value.Column.IsIdentity)
                        continue;
                    if (DC.Value.Column.ForeignRelations.ContainsValue(DC.Value.Name) // Has a foreign relation
                        && this.PrimaryKeyColumnList.Contains(DC.Value.Name) // Part of the PK
                        && DC.Value.Column.ForeignRelations.ContainsKey(this.ParentTable().Name))
                    {
                        if (this.ParentTable() != null &&
                            this.ParentTable().DataColumns().ContainsKey(DC.Key) &&
                            this.ParentTable().DataColumns()[DC.Key].Column.IsIdentity)
                            continue;
                        if (this.ParentTable() != null &&
                            this.ParentTable().ParentTable() != null &&
                            this.ParentTable().ParentTable().DataColumns().ContainsKey(DC.Key) &&
                            this.ParentTable().ParentTable().DataColumns()[DC.Key].Column.IsIdentity)
                            continue;
                        string DisplayTextOfParentColumn = "";
                        bool ValuePresent = false;
                        foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DCparent in this.ParentTable().DataColumns())
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, string> FK in DC.Value.Column.ForeignRelations)
                            {
                                if (FK.Key == DC.Value.DataTable().Name && FK.Value == DCparent.Key)
                                {
                                    DisplayTextOfParentColumn = DCparent.Value.DisplayText;
                                    if (SelectedRowIndex != null &&
                                        !this._Sheet.DT().Rows[(int)SelectedRowIndex][DisplayTextOfParentColumn].Equals(System.DBNull.Value))
                                    {
                                        ValuePresent = true;
                                        break;
                                    }
                                }
                            }
                            if (ValuePresent)
                                break;
                        }
                        if (ValuePresent)
                            continue;
                    }
                    if (Message.Length > 0) Message += ". ";
                    Message += "No value for the column " + DC.Value.DisplayText + " is provided";
                    OK = false;
                    break;
                }
            }
            return OK;
        }

        public bool AddData(int? SelectedRow, ref System.Data.DataRow AddedRow, ref string Message)//
        {
            bool OK = true;
            string ColumnClause = "";
            string ValueClause = "";
            //System.Data.DataRow Rselected = this._Sheet.DT().Rows[SelectedRow];
            System.Data.DataRow AddingDefaultRow = this._Sheet.DtAdding.Rows[0];
            System.Collections.Generic.Dictionary<string, string> PKvalues = new Dictionary<string, string>();
            string SQL = "";
            // Check if any values are given
            bool AnyValuesAreGiven = false;
            // providing all column values except for Identity columns
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (DC.Value.DataTable().Alias() == this.Alias()
                    && DC.Value.Type() == DataColumn.ColumnType.Data
                    && !this.DataColumns()[DC.Value.Name].Column.IsIdentity)
                {
                    if (this.PrimaryKeyColumnList.Contains(DC.Value.Name)
                        && this.ParentTable() != null
                        && this.ParentTable().DataColumns().ContainsKey(DC.Value.Name)
                        && SelectedRow != null
                        && !AddedRow[this.ParentTable().DataColumns()[DC.Value.Name].DisplayText].Equals(System.DBNull.Value)
                        && AddedRow[this.ParentTable().DataColumns()[DC.Value.Name].DisplayText].ToString().Length > 0)
                    {
                        string ParentValue = AddedRow[this.ParentTable().DataColumns()[DC.Value.Name].DisplayText].ToString();
                        if (ParentValue.Length > 0)
                        {
                            if (ColumnClause.Length > 0)
                                ColumnClause += ", ";
                            if (ValueClause.Length > 0)
                                ValueClause += ", ";
                            ColumnClause += DC.Value.Name;
                            ValueClause += "'" + ParentValue.Replace("'", "''") + "'";
                        }
                        AddedRow[DC.Value.DisplayText] = ParentValue;
                        if (!PKvalues.ContainsKey(DC.Value.Name) && this.PrimaryKeyColumnList.Contains(DC.Value.Name))
                            PKvalues.Add(DC.Value.Name, ParentValue);
                    }
                    else if (SelectedRow == null &&
                        this.PrimaryKeyColumnList.Contains(DC.Value.Column.Name) &&
                        this.ParentTable() != null &&
                        this.DataColumns()[DC.Value.Column.Name].Column.ForeignRelations.ContainsKey(this.ParentTable().Name) &&
                        this.DataColumns()[DC.Value.Column.Name].Column.ForeignRelations[this.ParentTable().Name] == DC.Value.Name)
                    {
                        string ParentColumn = this._Sheet.DataTables()[this.ParentTable().Alias()].DataColumns()[DC.Value.Name].DisplayText;
                        string ParentValue = AddedRow[ParentColumn].ToString();
                        AddedRow[DC.Value.DisplayText] = ParentValue;
                        if (ColumnClause.Length > 0)
                            ColumnClause += ", ";
                        if (ValueClause.Length > 0)
                            ValueClause += ", ";
                        ColumnClause += DC.Value.Name;
                        ValueClause += "'" + ParentValue.Replace("'", "''") + "'";
                    }

                    // Adding defaults
                    else if (!AddingDefaultRow[DC.Value.DisplayText].Equals(System.DBNull.Value)
                       && AddingDefaultRow[DC.Value.DisplayText].ToString().Length > 0)
                    {
                        if (ColumnClause.Length > 0)
                            ColumnClause += ", ";
                        if (ValueClause.Length > 0)
                            ValueClause += ", ";
                        ColumnClause += DC.Value.Name;
                        ValueClause += "'" + AddingDefaultRow[DC.Value.DisplayText].ToString().Replace("'", "''") + "'";
                        AddedRow[DC.Value.DisplayText] = AddingDefaultRow[DC.Value.DisplayText].ToString();
                        if (!PKvalues.ContainsKey(DC.Value.Name) && this.PrimaryKeyColumnList.Contains(DC.Value.Name))
                            PKvalues.Add(DC.Value.Name, AddingDefaultRow[DC.Value.DisplayText].ToString());
                        AnyValuesAreGiven = true;
                    }
                    // Default defined for the column if none is given by the user
                    else if (DC.Value.DefaultForAdding != null && DC.Value.DefaultForAdding.Length > 0)
                    {
                        if (ColumnClause.Length > 0)
                            ColumnClause += ", ";
                        if (ValueClause.Length > 0)
                            ValueClause += ", ";
                        ColumnClause += DC.Value.Name;
                        ValueClause += "'" + DC.Value.DefaultForAdding.Replace("'", "''") + "'";
                        AddedRow[DC.Value.DisplayText] = DC.Value.DefaultForAdding;
                        if (!PKvalues.ContainsKey(DC.Value.Name) && this.PrimaryKeyColumnList.Contains(DC.Value.Name))
                            PKvalues.Add(DC.Value.Name, DC.Value.DefaultForAdding);
                    }
                    // Preset values
                    else if (DC.Value.RestrictionValue != null
                        && DC.Value.RestrictionValue.Length > 0)
                    {
                        if (ColumnClause.Length > 0)
                            ColumnClause += ", ";
                        if (ValueClause.Length > 0)
                            ValueClause += ", ";
                        ColumnClause += DC.Value.Name;
                        ValueClause += "'" + DC.Value.RestrictionValue.Replace("'", "''") + "'";
                        AddedRow[DC.Value.DisplayText] = DC.Value.RestrictionValue;
                        if (!PKvalues.ContainsKey(DC.Value.Name) && this.PrimaryKeyColumnList.Contains(DC.Value.Name))
                            PKvalues.Add(DC.Value.Name, DC.Value.RestrictionValue);
                    }
                    // Default via SQL if none is given so far
                    else if (DC.Value.SqlQueryForDefaultForAdding.Length > 0)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in PKvalues)
                        {
                            if (SQL.Length > 0) SQL += " AND ";
                            SQL += "T." + KV.Key + " = '" + KV.Value + "' ";
                        }
                        SQL = DC.Value.SqlQueryForDefaultForAdding + " WHERE " + SQL;
                        string M = "";
                        string iPK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref M);
                        if (M.Length == 0)
                        {
                            if (ColumnClause.Length > 0)
                                ColumnClause += ", ";
                            if (ValueClause.Length > 0)
                                ValueClause += ", ";
                            ColumnClause += DC.Value.Name;
                            ValueClause += "'" + iPK.Replace("'", "''") + "'";
                            AddedRow[DC.Value.DisplayText] = iPK;
                            if (!PKvalues.ContainsKey(DC.Value.Name) && this.PrimaryKeyColumnList.Contains(DC.Value.Name))
                                PKvalues.Add(DC.Value.Name, iPK);
                        }
                    }
                }
            }
            // getting defaults for not null columns
            foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in this.DataColumns())
            {
                if (DC.Value.DefaultForAdding != null
                    && DC.Value.DefaultForAdding.Length > 0
                    && ColumnClause.IndexOf(DC.Key) == -1)
                {
                    if (ColumnClause.Length > 0)
                        ColumnClause += ", ";
                    if (ValueClause.Length > 0)
                        ValueClause += ", ";
                    ColumnClause += DC.Value.Name;
                    ValueClause += "'" + DC.Value.DefaultForAdding.Replace("'", "''") + "'";
                    if (AddedRow.Table.Columns.Contains(DC.Value.DisplayText))
                        AddedRow[DC.Value.DisplayText] = DC.Value.DefaultForAdding;
                    if (!PKvalues.ContainsKey(DC.Value.Name) && this.PrimaryKeyColumnList.Contains(DC.Value.Name))
                        PKvalues.Add(DC.Value.Name, DC.Value.DefaultForAdding);
                }
            }

            // getting parent table informations
            if ((ColumnClause.Length > 0
                && ValueClause.Length > 0
                && this.ParentTable() != null) ||
                this.Type() == TableType.Project)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in this.DataColumns())
                {
                    if (AddedRow.Table.Columns.Contains(DC.Value.DisplayText)
                        && !AddedRow[DC.Value.DisplayText].Equals(DBNull.Value))
                        continue;
                    if (ColumnClause.IndexOf(DC.Key) > -1)
                        continue;
                    if (DC.Value.Column.ForeignRelationTable != null
                        && DC.Value.Column.ForeignRelationTable == this.ParentTable().Name)
                    {
                        if (AddedRow.Table.Columns.Contains(this.ParentTable().DataColumns()[DC.Value.Column.ForeignRelationColumn].DisplayText)
                            && !AddedRow[this.ParentTable().DataColumns()[DC.Value.Column.ForeignRelationColumn].DisplayText].Equals(System.DBNull.Value))
                        {
                            if (ColumnClause.Length > 0)
                                ColumnClause += ", ";
                            if (ValueClause.Length > 0)
                                ValueClause += ", ";
                            ColumnClause += DC.Value.Name;
                            ValueClause += "'" + AddedRow[this.ParentTable().DataColumns()[DC.Value.Column.ForeignRelationColumn].DisplayText].ToString().Replace("'", "''") + "'";
                            if (AddedRow.Table.Columns.Contains(DC.Value.DisplayText))
                                AddedRow[DC.Value.DisplayText] = AddedRow[this.ParentTable().DataColumns()[DC.Value.Column.ForeignRelationColumn].DisplayText];
                            if (!PKvalues.ContainsKey(DC.Value.Name) && this.PrimaryKeyColumnList.Contains(DC.Value.Name))
                                PKvalues.Add(DC.Value.Name, AddedRow[this.ParentTable().DataColumns()[DC.Value.Column.ForeignRelationColumn].DisplayText].ToString());
                        }
                        else if (SelectedRow != null &&
                            this.Type() == TableType.Root &&
                            !this._Sheet.DT().Rows[(int)SelectedRow][this.ParentTable().DataColumns()[DC.Value.Column.ForeignRelationColumn].DisplayText].Equals(System.DBNull.Value))
                        {
                            if (ColumnClause.Length > 0)
                                ColumnClause += ", ";
                            if (ValueClause.Length > 0)
                                ValueClause += ", ";
                            ColumnClause += DC.Value.Name;
                            ValueClause += "'" + this._Sheet.DT().Rows[(int)SelectedRow][this.ParentTable().DataColumns()[DC.Value.Column.ForeignRelationColumn].DisplayText].ToString().Replace("'", "''") + "'";
                            if (AddedRow.Table.Columns.Contains(DC.Value.DisplayText))
                                AddedRow[DC.Value.DisplayText] = this._Sheet.DT().Rows[(int)SelectedRow][this.ParentTable().DataColumns()[DC.Value.Column.ForeignRelationColumn].DisplayText];
                            if (!PKvalues.ContainsKey(DC.Value.Name) && this.PrimaryKeyColumnList.Contains(DC.Value.Name))
                                PKvalues.Add(DC.Value.Name, this._Sheet.DT().Rows[(int)SelectedRow][this.ParentTable().DataColumns()[DC.Value.Column.ForeignRelationColumn].DisplayText].ToString());
                        }
                    }
                }
            }
            if (this.Type() == TableType.Project
                && this.Columns.ContainsKey("ProjectID"))
            {
                if (ColumnClause.Length > 0)
                    ColumnClause += ", ";
                if (ValueClause.Length > 0)
                    ValueClause += ", ";
                ColumnClause += "ProjectID";
                ValueClause += this._Sheet.ProjectID().ToString();
                AnyValuesAreGiven = true;
            }
            if (AnyValuesAreGiven && ColumnClause.Length > 0 && ValueClause.Length > 0)
            {
                SQL = "INSERT INTO " + this.Name + " (" + ColumnClause + ") VALUES (" + ValueClause + ")";
                if (this.IdentityColumn.Length > 0)
                {
                    SQL += " SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY] ";
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
                    if (Message.Length > 0)
                        OK = false;
                    else
                    {
                        // Check Result for plausibility
                        SQL = "SELECT MAX(" + this.IdentityColumn + ") FROM " + this.Name + " WHERE LogUpdatedBy = SUSER_SNAME()";
                        string iPK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
                        if (iPK != Result && Result.Length < iPK.Length - 1)
                            Result = iPK;
                        foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                        {
                            if (DC.Value.DataTable().Alias() == this.Alias() &&
                                DC.Value.Column.Name == this.IdentityColumn)
                                AddedRow[DC.Value.DisplayText] = Result;
                        }
                        if (!PKvalues.ContainsKey(this.IdentityColumn))
                            PKvalues.Add(this.IdentityColumn, Result);
                    }
                }
                else
                    OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message);
            }
            if (!OK)
            {
                return OK;
            }
            if (OK && AnyValuesAreGiven)
            {
                // getting the PK
                try
                {
                    foreach (string PK in this.PrimaryKeyColumnList)
                    {
                        if (AddedRow.Table.Columns.Contains(this.DataColumns()[PK].DisplayText)
                            && !AddedRow[this.DataColumns()[PK].DisplayText].Equals(System.DBNull.Value))
                        {
                            if (!PKvalues.ContainsKey(PK))
                                PKvalues.Add(PK, AddedRow[this.DataColumns()[PK].DisplayText].ToString());
                        }
                        else
                        {
                            if (this.DataColumns()[PK].Column.IsIdentity)
                            {
                                SQL = "SELECT MAX(" + PK + ") FROM " + this.Name + " WHERE LogUpdatedBy = SUSER_SNAME()";
                                string iPK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
                                if (!PKvalues.ContainsKey(PK))
                                    PKvalues.Add(PK, iPK);
                                AddedRow[this.DataColumns()[PK].DisplayText] = iPK;
                            }
                            else if (AddedRow.Table.Columns.Contains(this.DataColumns()[PK].DisplayText))
                            {
                                SQL = "";
                                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in PKvalues)
                                {
                                    if (SQL.Length > 0) SQL += " AND ";
                                    SQL += KV.Key + " = '" + KV.Value + "' ";
                                }
                                if (SQL.Length > 0)
                                {
                                    SQL = "SELECT MAX(" + PK + ") FROM " + this.Name + " WHERE " + SQL;
                                    string iPK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
                                    if (!PKvalues.ContainsKey(PK))
                                        PKvalues.Add(PK, iPK);
                                    AddedRow[this.DataColumns()[PK].DisplayText] = iPK;
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                }
                if (AnyValuesAreGiven)
                    OK = this.AddDependingData(SelectedRow, this, ref AddedRow, ref Message);
            }
            return OK;
        }

        public bool AddData(ref System.Data.DataRow AddedRow, ref string Message, ref System.Collections.Generic.Dictionary<string, string> PKvalues)//
        {
            bool OK = true;
            try
            {
                string ColumnClause = "";
                string ValueClause = "";
                System.Data.DataRow AddingDefaultRow = this._Sheet.DtAdding.Rows[0];
                string SQL = "";
                // Check if any values are given
                bool AnyValuesAreGiven = false;
                // providing all column values except for Identity columns
                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                {
                    if (DC.Value.DataTable().Alias() == this.Alias()
                        && DC.Value.Type() == DataColumn.ColumnType.Data
                        && !this.DataColumns()[DC.Value.Name].Column.IsIdentity)
                    {
                        // PK derived from the parent table
                        if (this.PrimaryKeyColumnList.Contains(DC.Value.Name)
                            && this.ParentTable() != null
                            && this.ParentTable().DataColumns().ContainsKey(DC.Value.Name)
                            && !AddedRow[this.ParentTable().DataColumns()[DC.Value.Name].DisplayText].Equals(System.DBNull.Value)
                            && AddedRow[this.ParentTable().DataColumns()[DC.Value.Name].DisplayText].ToString().Length > 0)
                        {
                            string ParentValue = AddedRow[this.ParentTable().DataColumns()[DC.Value.Name].DisplayText].ToString();
                            if (ParentValue.Length > 0)
                            {
                                if (ColumnClause.Length > 0)
                                    ColumnClause += ", ";
                                if (ValueClause.Length > 0)
                                    ValueClause += ", ";
                                ColumnClause += DC.Value.Name;
                                ValueClause += "'" + ParentValue.Replace("'", "''") + "'";
                            }
                            AddedRow[DC.Value.DisplayText] = ParentValue;
                            if (!PKvalues.ContainsKey(DC.Value.Name) && this.PrimaryKeyColumnList.Contains(DC.Value.Name))
                                PKvalues.Add(DC.Value.Name, ParentValue);
                        }
                        else if (this.PrimaryKeyColumnList.Contains(DC.Value.Column.Name) &&
                            this.ParentTable() != null &&
                            this.DataColumns()[DC.Value.Column.Name].Column.ForeignRelations.ContainsKey(this.ParentTable().Name) &&
                            this.DataColumns()[DC.Value.Column.Name].Column.ForeignRelations[this.ParentTable().Name] == DC.Value.Name)
                        {
                            string ParentColumn = this._Sheet.DataTables()[this.ParentTable().Alias()].DataColumns()[DC.Value.Name].DisplayText;
                            string ParentValue = AddedRow[ParentColumn].ToString();
                            if (ParentValue.Length > 0)
                            {
                                AddedRow[DC.Value.DisplayText] = ParentValue;
                                if (ColumnClause.Length > 0)
                                    ColumnClause += ", ";
                                if (ValueClause.Length > 0)
                                    ValueClause += ", ";
                                ColumnClause += DC.Value.Name;
                                ValueClause += "'" + ParentValue.Replace("'", "''") + "'";
                            }
                            else
                            {
                            }
                        }

                        // Adding defaults
                        else if (!AddingDefaultRow[DC.Value.DisplayText].Equals(System.DBNull.Value)
                           && AddingDefaultRow[DC.Value.DisplayText].ToString().Length > 0)
                        {
                            if (ColumnClause.Length > 0)
                                ColumnClause += ", ";
                            if (ValueClause.Length > 0)
                                ValueClause += ", ";
                            ColumnClause += DC.Value.Name;
                            ValueClause += "'" + AddingDefaultRow[DC.Value.DisplayText].ToString().Replace("'", "''") + "'";
                            AddedRow[DC.Value.DisplayText] = AddingDefaultRow[DC.Value.DisplayText].ToString();
                            if (!PKvalues.ContainsKey(DC.Value.Name) && this.PrimaryKeyColumnList.Contains(DC.Value.Name))
                                PKvalues.Add(DC.Value.Name, AddingDefaultRow[DC.Value.DisplayText].ToString());
                            AnyValuesAreGiven = true;
                        }
                        // Default defined for the column if none is given by the user
                        else if (DC.Value.DefaultForAdding != null && DC.Value.DefaultForAdding.Length > 0)
                        {
                            if (ColumnClause.Length > 0)
                                ColumnClause += ", ";
                            if (ValueClause.Length > 0)
                                ValueClause += ", ";
                            ColumnClause += DC.Value.Name;
                            ValueClause += "'" + DC.Value.DefaultForAdding.Replace("'", "''") + "'";
                            AddedRow[DC.Value.DisplayText] = DC.Value.DefaultForAdding;
                            if (!PKvalues.ContainsKey(DC.Value.Name) && this.PrimaryKeyColumnList.Contains(DC.Value.Name))
                                PKvalues.Add(DC.Value.Name, DC.Value.DefaultForAdding);
                        }
                        // Preset values
                        else if (DC.Value.RestrictionValue != null
                            && DC.Value.RestrictionValue.Length > 0)
                        {
                            if (ColumnClause.Length > 0)
                                ColumnClause += ", ";
                            if (ValueClause.Length > 0)
                                ValueClause += ", ";
                            ColumnClause += DC.Value.Name;
                            ValueClause += "'" + DC.Value.RestrictionValue.Replace("'", "''") + "'";
                            AddedRow[DC.Value.DisplayText] = DC.Value.RestrictionValue;
                            if (!PKvalues.ContainsKey(DC.Value.Name) && this.PrimaryKeyColumnList.Contains(DC.Value.Name))
                                PKvalues.Add(DC.Value.Name, DC.Value.RestrictionValue);
                        }
                        // Default via SQL if none is given so far
                        else if (DC.Value.SqlQueryForDefaultForAdding.Length > 0)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in PKvalues)
                            {
                                if (SQL.Length > 0) SQL += " AND ";
                                SQL += "T." + KV.Key + " = '" + KV.Value + "' ";
                            }
                            SQL = DC.Value.SqlQueryForDefaultForAdding + " WHERE " + SQL;
                            string M = "";
                            string iPK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref M);
                            if (M.Length == 0)
                            {
                                if (ColumnClause.Length > 0)
                                    ColumnClause += ", ";
                                if (ValueClause.Length > 0)
                                    ValueClause += ", ";
                                ColumnClause += DC.Value.Name;
                                ValueClause += "'" + iPK.Replace("'", "''") + "'";
                                AddedRow[DC.Value.DisplayText] = iPK;
                                if (!PKvalues.ContainsKey(DC.Value.Name) && this.PrimaryKeyColumnList.Contains(DC.Value.Name))
                                    PKvalues.Add(DC.Value.Name, iPK);
                            }
                        }
                    }
                }
                // getting defaults for not null columns
                foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in this.DataColumns())
                {
                    if (DC.Value.DefaultForAdding != null
                        && DC.Value.DefaultForAdding.Length > 0
                        && ColumnClause.IndexOf(DC.Key) == -1)
                    {
                        if (ColumnClause.Length > 0)
                            ColumnClause += ", ";
                        if (ValueClause.Length > 0)
                            ValueClause += ", ";
                        ColumnClause += DC.Value.Name;
                        ValueClause += "'" + DC.Value.DefaultForAdding.Replace("'", "''") + "'";
                        if (AddedRow.Table.Columns.Contains(DC.Value.DisplayText))
                            AddedRow[DC.Value.DisplayText] = DC.Value.DefaultForAdding;
                        if (!PKvalues.ContainsKey(DC.Value.Name) && this.PrimaryKeyColumnList.Contains(DC.Value.Name))
                            PKvalues.Add(DC.Value.Name, DC.Value.DefaultForAdding);
                    }
                }

                // getting parent table informations
                if ((ColumnClause.Length > 0
                    && ValueClause.Length > 0
                    && this.ParentTable() != null) ||
                    this.Type() == TableType.Project)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in this.DataColumns())
                    {
                        if (ColumnClause.IndexOf(DC.Key) > -1)
                            continue;
                        if (DC.Value.Column.ForeignRelationTable != null
                            && DC.Value.Column.ForeignRelationTable == this.ParentTable().Name)
                        {
                            if (AddedRow.Table.Columns.Contains(this.ParentTable().DataColumns()[DC.Value.Column.ForeignRelationColumn].DisplayText)
                                && !AddedRow[this.ParentTable().DataColumns()[DC.Value.Column.ForeignRelationColumn].DisplayText].Equals(System.DBNull.Value))
                            {
                                if (ColumnClause.Length > 0)
                                    ColumnClause += ", ";
                                if (ValueClause.Length > 0)
                                    ValueClause += ", ";
                                ColumnClause += DC.Value.Name;
                                ValueClause += "'" + AddedRow[this.ParentTable().DataColumns()[DC.Value.Column.ForeignRelationColumn].DisplayText].ToString().Replace("'", "''") + "'";
                                if (AddedRow.Table.Columns.Contains(DC.Value.DisplayText))
                                    AddedRow[DC.Value.DisplayText] = AddedRow[this.ParentTable().DataColumns()[DC.Value.Column.ForeignRelationColumn].DisplayText];
                                if (!PKvalues.ContainsKey(DC.Value.Name) && this.PrimaryKeyColumnList.Contains(DC.Value.Name))
                                    PKvalues.Add(DC.Value.Name, AddedRow[this.ParentTable().DataColumns()[DC.Value.Column.ForeignRelationColumn].DisplayText].ToString());
                            }
                        }
                    }
                }
                if (this.Type() == TableType.Project
                    && this.Columns.ContainsKey("ProjectID"))
                {
                    if (ColumnClause.Length > 0)
                        ColumnClause += ", ";
                    if (ValueClause.Length > 0)
                        ValueClause += ", ";
                    ColumnClause += "ProjectID";
                    ValueClause += this._Sheet.ProjectID().ToString();
                    AnyValuesAreGiven = true;
                }
                if (AnyValuesAreGiven && ColumnClause.Length > 0 && ValueClause.Length > 0)
                {
                    SQL = "INSERT INTO " + this.Name + " (" + ColumnClause + ") VALUES (" + ValueClause + ")";
                    if (this.IdentityColumn.Length > 0)
                    {
                        SQL += " SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY] ";
                        string Error = "";
                        string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Error);
                        if (Error.Length > 0)
                        {
                            Message += "\r\n" + Error;
                            OK = false;
                        }
                        else
                        {
                            // Check Result for plausibility
                            SQL = "SELECT MAX(" + this.IdentityColumn + ") FROM " + this.Name + " WHERE LogUpdatedBy = SUSER_SNAME()";
                            string iPK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
                            if (iPK != Result && Result.Length < iPK.Length - 1)
                                Result = iPK;
                            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                            {
                                if (DC.Value.DataTable().Alias() == this.Alias() &&
                                    DC.Value.Column.Name == this.IdentityColumn)
                                    AddedRow[DC.Value.DisplayText] = Result;
                                if (DC.Value.DataTable().ParentTable() != null &&
                                    DC.Value.DataTable().ParentTable().Alias() == this.Alias() &&
                                    DC.Value.Column.Name == this.IdentityColumn)
                                    AddedRow[DC.Value.DisplayText] = Result;
                            }
                            if (!PKvalues.ContainsKey(this.IdentityColumn))
                                PKvalues.Add(this.IdentityColumn, Result);
                        }
                    }
                    else
                    {
                        string Error = "";
                        OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
                        if (Error.Length > 0)
                            Message += Error;
                    }
                }
                if (!OK)
                {
                    return OK;
                }
                if (OK && AnyValuesAreGiven)
                {
                    // getting the PK
                    try
                    {
                        foreach (string PK in this.PrimaryKeyColumnList)
                        {
                            if (AddedRow.Table.Columns.Contains(this.DataColumns()[PK].DisplayText)
                                && !AddedRow[this.DataColumns()[PK].DisplayText].Equals(System.DBNull.Value))
                            {
                                if (!PKvalues.ContainsKey(PK))
                                    PKvalues.Add(PK, AddedRow[this.DataColumns()[PK].DisplayText].ToString());
                            }
                            else
                            {
                                if (this.DataColumns()[PK].Column.IsIdentity)
                                {
                                    SQL = "SELECT MAX(" + PK + ") FROM " + this.Name + " WHERE LogUpdatedBy = SUSER_SNAME()";
                                    string iPK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
                                    if (!PKvalues.ContainsKey(PK))
                                        PKvalues.Add(PK, iPK);
                                    AddedRow[this.DataColumns()[PK].DisplayText] = iPK;
                                }
                                else if (AddedRow.Table.Columns.Contains(this.DataColumns()[PK].DisplayText))
                                {
                                    SQL = "";
                                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in PKvalues)
                                    {
                                        if (SQL.Length > 0) SQL += " AND ";
                                        SQL += KV.Key + " = '" + KV.Value + "' ";
                                    }
                                    if (SQL.Length > 0)
                                    {
                                        SQL = "SELECT MAX(" + PK + ") FROM " + this.Name + " WHERE " + SQL;
                                        string iPK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
                                        if (!PKvalues.ContainsKey(PK))
                                            PKvalues.Add(PK, iPK);
                                        AddedRow[this.DataColumns()[PK].DisplayText] = iPK;
                                    }
                                }
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                    }
                    if (AnyValuesAreGiven)
                        OK = this.AddDependingData(this, ref AddedRow, ref Message);
                }
                else if (!AnyValuesAreGiven)
                {
                    Message = "No values for the new dataset were given";
                    OK = false;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        private bool AddProjectData()
        {
            bool OK = true;
            string PK = "";
            if (OK && this._Sheet.MasterQueryColumn.DataTable().Alias() == this.Alias())
            {
                string SQL = "SELECT MAX(" + this._Sheet.MasterQueryColumn.Name + ") FROM " + this._Sheet.MasterQueryColumn.DataTable().Name;
                PK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                string ProjectTable = "";
                foreach (System.Collections.Generic.KeyValuePair<string, DataTable> DT in this._Sheet.DataTables())
                {
                    if (DT.Value.Type() == TableType.Project)
                    {
                        ProjectTable = DT.Value.Name;
                        break;
                    }
                }
                SQL = "INSERT INTO " + ProjectTable + " (ProjectID, " + this._Sheet.MasterQueryColumn.Name + ") VALUES (" + this._Sheet.ProjectID().ToString() + ", " + PK + ")";
                OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            }
            return OK;
        }

        /// <summary>
        /// Adding data to tables depending on the current table
        /// </summary>
        /// <param name="SelectedRow">The row selected in the interface as starting row</param>
        /// <param name="Table">The parent table of the dependent tables</param>
        /// <param name="AddedRow">The added row</param>
        /// <param name="Message">String for error messages</param>
        /// <returns>If adding to data was successful</returns>
        private bool AddDependingData(int? SelectedRow, DataTable Table, ref System.Data.DataRow AddedRow, ref string Message)
        {
            bool OK = true;
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DataTable> DT in this._Sheet.DataTables())
                {
                    if (DT.Value.ParentTable() != null
                        && DT.Value.ParentTable().Alias() == Table.Alias()
                        && DT.Value.Name != Table.Name)
                    {
                        DT.Value.AddData(SelectedRow, ref AddedRow, ref Message);
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return OK;
        }

        private bool AddDependingData(DataTable Table, ref System.Data.DataRow AddedRow, ref string Message)
        {
            bool OK = true;
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DataTable> DT in this._Sheet.DataTables())
                {
                    if (DT.Value.ParentTable() != null
                        && DT.Value.ParentTable().Alias() == Table.Alias()
                        && DT.Value.Name != Table.Name)
                    {
                        System.Collections.Generic.Dictionary<string, string> PKvalues = new Dictionary<string, string>();
                        DT.Value.AddData(ref AddedRow, ref Message, ref PKvalues);
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return OK;
        }

        #region Old version

        public bool InsertData(int Row, System.Data.DataRow DefaultRow, int ProjectID)
        {
            bool OK = true;
            try
            {
                System.Data.DataRow R = this._Sheet.DT().Rows[Row];
                if (DefaultRow != null)
                {
                    string ColumnClause = "";
                    string ValueClause = "";
                    foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                    {
                        if (DC.Value.DataTable().Alias() == this.Alias() && DC.Value.Type() == DataColumn.ColumnType.Data)
                        {
                            if (this.PrimaryKeyColumnList.Contains(DC.Value.Name) && R[DC.Value.DisplayText].Equals(System.DBNull.Value))
                            {
                                if (this.ParentTable() != null && this.ParentTable().DataColumns().ContainsKey(DC.Value.Name))
                                {
                                    string ParentValue = R[this.ParentTable().DataColumns()[DC.Value.Name].DisplayText].ToString();
                                    if (ParentValue.Length > 0)
                                    {
                                        if (ColumnClause.Length > 0)
                                            ColumnClause += ", ";
                                        if (ValueClause.Length > 0)
                                            ValueClause += ", ";
                                        ColumnClause += DC.Value.Name;
                                        ValueClause += "'" + ParentValue.Replace("'", "''") + "'";
                                    }
                                }
                            }
                            else if (!DefaultRow[DC.Value.DisplayText].Equals(System.DBNull.Value))
                            {
                                if (ColumnClause.Length > 0)
                                    ColumnClause += ", ";
                                if (ValueClause.Length > 0)
                                    ValueClause += ", ";
                                ColumnClause += DC.Value.Name;
                                ValueClause += "'" + DefaultRow[DC.Value.DisplayText].ToString().Replace("'", "''") + "'";
                            }
                        }
                    }
                    string SQL = "INSERT INTO " + this.Name + " (" + ColumnClause + ") VALUES (" + ValueClause + ")";
                    OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                    string PK = "";
                    if (OK && this._Sheet.MasterQueryColumn.DataTable().Alias() == this.Alias())
                    {
                        SQL = "SELECT MAX(" + this._Sheet.MasterQueryColumn.Name + ") FROM " + this._Sheet.MasterQueryColumn.DataTable().Name;
                        PK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                        string ProjectTable = "";
                        foreach (System.Collections.Generic.KeyValuePair<string, DataTable> DT in this._Sheet.DataTables())
                        {
                            if (DT.Value.Type() == TableType.Project)
                            {
                                ProjectTable = DT.Value.Name;
                                break;
                            }
                        }
                        SQL = "INSERT INTO " + ProjectTable + " (ProjectID, " + this._Sheet.MasterQueryColumn.Name + ") VALUES (" + ProjectID.ToString() + ", " + PK + ")";
                        OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                    }
                    if (OK)
                    {
                        System.Collections.Generic.Dictionary<string, string> PKvalues = new Dictionary<string, string>();
                        if (PK.Length > 0)
                            PKvalues.Add(this._Sheet.MasterQueryColumn.Name, PK);
                        else if (this.PrimaryKeyColumnList.Count == 1)
                        {
                            SQL = "SELECT MAX(" + this.PrimaryKeyColumnList[0] + ") FROM " + this.Name;
                            PK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                            PKvalues.Add(this.PrimaryKeyColumnList[0], PK);
                        }
                        else
                        {
                            foreach (string P in this.PrimaryKeyColumnList)
                            {
                                if (this.DataColumns()[P].IsIdentity)
                                {
                                    SQL = "SELECT MAX(" + this.PrimaryKeyColumnList[0] + ") FROM " + this.Name;
                                    PK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                                    PKvalues.Add(this.PrimaryKeyColumnList[0], PK);
                                }
                                else if (this.ParentTable() != null)
                                {
                                    PK = R[this.ParentTable().DataColumns()[P].DisplayText].ToString();
                                    PKvalues.Add(this.PrimaryKeyColumnList[0], PK);
                                }
                            }
                        }
                        this.InsertDependingData(this, PKvalues, DefaultRow);
                    }
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        private bool InsertDependingData(DataTable Table, System.Collections.Generic.Dictionary<string, string> PK, System.Data.DataRow DefaultRow)
        {
            bool OK = true;
            foreach (System.Collections.Generic.KeyValuePair<string, DataTable> DT in this._Sheet.DataTables())
            {
                if (DT.Value.ParentTable().Alias() == Table.Alias())
                {
                    string ColumnClause = "";
                    string ValueClause = "";
                    foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                    {
                        if (!DefaultRow[DC.Value.DisplayText].Equals(System.DBNull.Value) &&
                            DefaultRow[DC.Value.DisplayText].ToString().Length > 0)
                        {
                            if (ColumnClause.Length > 0)
                                ColumnClause += ", ";
                            if (ValueClause.Length > 0)
                                ValueClause += ", ";
                            ColumnClause += DC.Value.Name;
                            ValueClause += "'" + DefaultRow[DC.Value.DisplayText].ToString().Replace("'", "''") + "'";
                        }
                    }
                    if (ColumnClause.Length > 0)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in PK)
                        {
                            ColumnClause += ", " + KV.Key;
                            ValueClause += ", '" + KV.Value.Replace("'", "''") + "'";
                        }
                        foreach (string R in DT.Value.RestrictionColumns)
                        {
                            ColumnClause += ", " + R;
                            ValueClause += ", '" + DT.Value.DataColumns()[R].RestrictionValue.Replace("'", "''") + "'";
                        }
                    }
                }
            }
            return OK;
        }

        public System.Collections.Generic.List<string> InsertDefaultsMissing()
        {
            System.Collections.Generic.List<string> L = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in this.DataColumns())
            {
                if (!DC.Value.IsNullable && DC.Value.ColumnDefault == null && DC.Value.DefaultForAdding == null)
                {
                    L.Add(DC.Key);
                }
            }
            return L;
        }

        #endregion

        #endregion

        public bool SaveData(int Row, ref string Message, ref SqlCommandTypeExecuted SqlType)
        {
            bool OK = true;
            if (this.AllowUpdate())
            {
                try
                {
                    System.Data.DataRow R = this._Sheet.DT().Rows[Row];
                    bool PKcomplete = this.PKcomplete(R);

                    #region obs
                    //string RowGUID = this.RowGUIDvalue(R);
                    //bool RowGuidIncluded = this.ColumnRowGUIDisIncluded();
                    //bool PKmayChange = this.YouTryToChangeThePK(R, RowGuidIncluded);

                    //System.Collections.Generic.Dictionary<string, string> WhereConditions = this.WhereConditions(R);
                    //if (PKcomplete && PKmayChange && !RowGuidIncluded)
                    //{
                    //    System.Windows.Forms.MessageBox.Show("Can not change PK.\r\nTry including RowGUID", "Can't change PK", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    //    return false;

                    //    //string WhereClause = "";
                    //    //foreach (string P in this.PrimaryKeyColumnList)
                    //    //{
                    //    //    foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                    //    //    {
                    //    //        if (DC.Value.DataTable().Alias() == this.Alias() && DC.Value.Type() == DataColumn.ColumnType.Data)
                    //    //        {
                    //    //            if (DC.Value.Name == P)
                    //    //            {
                    //    //                if (R[DC.Value.DisplayText].Equals(System.DBNull.Value))
                    //    //                {
                    //    //                    PKcomplete = false;
                    //    //                    break;
                    //    //                }
                    //    //                else
                    //    //                {
                    //    //                    string Val = R[DC.Value.DisplayText].ToString();
                    //    //                    if (DC.Value.DataType != "int" && DC.Value.DataType != "smallint" && DC.Value.DataType != "float" && DC.Value.DataType != "real" && DC.Value.DataType != "bit" && DC.Value.DataType != "bigint" && DC.Value.DataType != "tinyint")
                    //    //                        Val = "'" + Val + "'";
                    //    //                    if (!DC.Value.IsHidden && !DC.Value.IsReadOnly() && !RowGuidIncluded)
                    //    //                    {
                    //    //                        System.Windows.Forms.MessageBox.Show("Can not change PK.\r\nTry including RowGUID", "Can't change PK", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    //    //                        return false;
                    //    //                    }
                    //    //                    if (RowGuidIncluded && DC.Value.Name == "RowGUID")
                    //    //                    {
                    //    //                        //if (WhereClause.Length > 0)
                    //    //                        //    WhereClause += " AND ";
                    //    //                        //WhereClause += DC.Value.Name + " = '" + R[DC.Value.DisplayText].ToString() + "'";
                    //    //                        WhereConditions.Add(DC.Value.Name, Val);
                    //    //                    }
                    //    //                    else if (DC.Value.IsHidden || DC.Value.IsReadOnly())
                    //    //                    {
                    //    //                        //if (WhereClause.Length > 0)
                    //    //                        //    WhereClause += " AND ";
                    //    //                        //WhereClause += DC.Value.Name + " = '" + R[DC.Value.DisplayText].ToString() + "'";
                    //    //                        WhereConditions.Add(DC.Value.Name, Val);
                    //    //                    }
                    //    //                    else
                    //    //                    {
                    //    //                        if (!RowGuidIncluded) PKcomplete = false;
                    //    //                    }
                    //    //                }
                    //    //            }
                    //    //        }
                    //    //    }
                    //    //    if (!PKcomplete)
                    //    //        break;
                    //    //}
                    //}

                    ////if (RowGuidIncluded && RowGUID.Length > 0)
                    ////{
                    ////    WhereConditions.Add("RowGUID", RowGUID);
                    ////    //if (WhereClause.Length > 0)
                    ////    //    WhereClause += " AND ";
                    ////    //WhereClause += RowGuidRestriction;
                    ////}
                    //string SetClause = "";
                    //bool Exists = false;
                    //if (PKcomplete)
                    //{
                    //    bool CanUpdate = true;
                    //    // if the PK is complete Update is possible unless the PK is changed data
                    //    if (PKmayChange)
                    //    {
                    //        // Check if dataset exists with same PK but different RowGUID
                    //        CanUpdate = !this.DataExist(this.WhereConditions(R, false), this.WhereConditions(R, true));
                    //    }
                    //        if (!CanUpdate)
                    //        {
                    //            Message = "A dataset with the same key is present in the table " + this.Name + ":\r\n";
                    //            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.WhereConditions(R, false))
                    //                Message += "\r\n" + KV.Key + ": " + KV.Value;
                    //        }
                    //    else { }
                    //    //string Sqlexists = "select count(*) from [" + this.Name + "] where " + WhereClause;
                    //    //string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(Sqlexists, true);
                    //    Exists = this.DataExist(WhereConditions);// Result.Length > 0 && Result != "0";
                    //    // Check if PK has been changed
                    //    if (!Exists)
                    //    {
                    //        bool RowGUIDincluded = false;
                    //        foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                    //        {
                    //            if (DC.Value.DataTable().Alias() == this.Alias() && DC.Value.Type() == DataColumn.ColumnType.Data)
                    //            {
                    //                if (DC.Value.Name.ToLower() == "rowguid")
                    //                {
                    //                    RowGUIDincluded = true;
                    //                    break;
                    //                }
                    //            }
                    //        }
                    //        if (RowGUIDincluded)
                    //        {
                    //            if(!WhereConditions.ContainsKey("RowGUID"))
                    //                WhereConditions.Add("RowGUID", "'" + R["RowGUID"].ToString() + "'");
                    //            //Sqlexists = "select count(*) from [" + this.Name + "] where RowGUID = '" + R["RowGUID"].ToString() + "'";
                    //            //Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(Sqlexists, true);
                    //            Exists = this.DataExist(WhereConditions);// Result.Length > 0 && Result != "0";
                    //            if (Exists)
                    //                this.UpdateData(R, "RowGUID = '" + R["RowGUID"].ToString() + "'", ref Message, ref SqlType, true);
                    //        }
                    //    }
                    //}
                    #endregion

                    if (PKcomplete)
                    {
                        OK = this.UpdateData(R, ref Message, ref SqlType);
                    }
                    else
                    {
                        OK = this.InsertData(R, WhereClause(this.WhereConditions(R, true)), ref Message, ref SqlType);
                    }
                }
                catch (System.Exception ex)
                {
                    OK = false;
                }
            }
            else
            {
                OK = false;
                Message = "You do not have the permission to change data in the table " + this.Name;
            }
            return OK;
        }

        #region Where
        private System.Collections.Generic.Dictionary<string, string> WhereConditions(System.Data.DataRow R)
        {
            System.Collections.Generic.Dictionary<string, string> Conditions = new Dictionary<string, string>();
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (DC.Value.DataTable().Alias() == this.Alias() && DC.Value.Type() == DataColumn.ColumnType.Data && !R[DC.Value.DisplayText].Equals(System.DBNull.Value))
                {
                    string Val = R[DC.Value.DisplayText].ToString();
                    if (DC.Value.DataType != "int" && DC.Value.DataType != "smallint" && DC.Value.DataType != "float" && DC.Value.DataType != "real" && DC.Value.DataType != "bit" && DC.Value.DataType != "bigint" && DC.Value.DataType != "tinyint")
                        Val = "'" + Val + "'";
                    // Get all parts of the PK
                    foreach (string P in this.PrimaryKeyColumnList)
                    {
                        if (DC.Value.Name == P)
                        {
                            Conditions.Add(DC.Value.Name, Val);
                        }
                    }
                    // Take RowGUID if included
                    if (DC.Value.Name == "RowGUID")
                    {
                        Conditions.Add(DC.Value.Name, Val);
                    }
                }
            }
            return Conditions;
        }

        private System.Collections.Generic.Dictionary<string, string> WhereConditionsHiddenPK(System.Data.DataRow R)
        {
            System.Collections.Generic.Dictionary<string, string> Conditions = new Dictionary<string, string>();
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (DC.Value.DataTable().Alias() == this.Alias() && DC.Value.Type() == DataColumn.ColumnType.Data && !R[DC.Value.DisplayText].Equals(System.DBNull.Value))
                {
                    if (this.PrimaryKeyColumnList.Contains(DC.Value.Name) && DC.Value.IsHidden)
                    {
                        string Val = R[DC.Value.DisplayText].ToString();
                        if (DC.Value.DataType != "int" && DC.Value.DataType != "smallint" && DC.Value.DataType != "float" && DC.Value.DataType != "real" && DC.Value.DataType != "bit" && DC.Value.DataType != "bigint" && DC.Value.DataType != "tinyint")
                            Val = "'" + Val + "'";
                        Conditions.Add(DC.Value.Name, Val);
                    }
                }
            }
            return Conditions;
        }



        private System.Collections.Generic.Dictionary<string, string> WhereConditions(System.Data.DataRow R, bool UseRowGUID)
        {
            System.Collections.Generic.Dictionary<string, string> Conditions = new Dictionary<string, string>();
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.WhereConditions(R))
            {
                if (UseRowGUID)
                {
                    if (KV.Key == "RowGUID")
                        Conditions.Add(KV.Key, KV.Value);
                }
                else
                {
                    if (KV.Key != "RowGUID")
                        Conditions.Add(KV.Key, KV.Value);
                }
            }
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.WhereConditionsHiddenPK(R))
            {
                Conditions.Add(KV.Key, KV.Value);
            }
            return Conditions;
        }


        private string WhereClause(System.Collections.Generic.Dictionary<string, string> WhereConditions)
        {
            return this.WhereClause(WhereConditions, false);
        }

        private string WhereClause(System.Collections.Generic.Dictionary<string, string> WhereConditions, bool ForInsert)
        {
            string WhereClause = "";
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in WhereConditions)
            {
                if (ForInsert && !this.PrimaryKeyColumnList.Contains(KV.Key)) continue; // Markus 24.7.23: Ob daten vorhanden darf nur PK geprüft werden
                if (WhereClause.Length > 0) WhereClause += " AND ";
                WhereClause += KV.Key + " = " + KV.Value;
            }
            return WhereClause;
        }


        private string WhereClause(System.Collections.Generic.Dictionary<string, string> WhereConditions, System.Collections.Generic.Dictionary<string, string> NotConditions)
        {
            string WhereClause = this.WhereClause(WhereConditions);
            if (NotConditions != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in NotConditions)
                {
                    if (WhereClause.Length > 0) WhereClause += " AND ";
                    if (KV.Value.ToLower() == "null")
                        WhereClause += KV.Key + " is not " + KV.Value;
                    else
                        WhereClause += KV.Key + " <> " + KV.Value;
                }
            }
            return WhereClause;
        }

        #endregion

        #region Exist, PK, ...
        private bool DataExist(System.Collections.Generic.Dictionary<string, string> WhereConditions, bool ForInsert)
        {
            try
            {
                string WhereClause = this.WhereClause(WhereConditions, ForInsert);
                string Sqlexists = "select count(*) from [" + this.Name + "] where " + WhereClause;
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(Sqlexists, true);
                return Result.Length > 0 && Result != "0";
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return false;
        }

        private bool DataExist(System.Collections.Generic.Dictionary<string, string> WhereConditions, System.Collections.Generic.Dictionary<string, string> NotConditions = null)
        {
            try
            {
                string WhereClause = this.WhereClause(WhereConditions, NotConditions);
                string Sqlexists = "select count(*) from [" + this.Name + "] where " + WhereClause;
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(Sqlexists, true);
                return Result.Length > 0 && Result != "0";
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return false;
        }

        private bool PKcomplete(System.Data.DataRow R)
        {
            bool PKcomplete = true;
            try
            {
                System.Collections.Generic.Dictionary<string, bool> PK = new Dictionary<string, bool>();
                foreach (string P in this.PrimaryKeyColumnList)
                {
                    PK.Add(P, false);
                }
                foreach (string P in this.PrimaryKeyColumnList)
                {
                    foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                    {
                        if (DC.Value.DataTable().Alias() == this.Alias() && DC.Value.Type() == DataColumn.ColumnType.Data)
                        {
                            if (DC.Value.Name == P)
                            {
                                if (R[DC.Value.DisplayText].Equals(System.DBNull.Value))
                                {
                                    PKcomplete = false;
                                    break;
                                }
                                else
                                {
                                    PK[P] = true;
                                }
                            }
                        }
                    }
                    if (!PKcomplete)
                        break;
                }
                PKcomplete = !PK.ContainsValue(false);
            }
            catch (System.Exception ex)
            {
                PKcomplete = false;
            }
            return PKcomplete;
        }

        private bool YouTryToChangeThePK(System.Data.DataRow R, bool RowGUIDincluded = false)
        {
            bool Included = false;
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in SetClauseColumns(R, RowGUIDincluded))
            {
                if (this.PrimaryKeyColumnList.Contains(KV.Key))
                {
                    Included = true;
                    break;
                }
            }
            return Included;
        }

        #endregion

        #region RowGUID

        private bool ColumnRowGUIDisIncluded()
        {
            bool RowGuidIncluded = false;
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (DC.Value.DataTable().Alias() == this.Alias() && DC.Value.Type() == DataColumn.ColumnType.Data)
                {
                    if (DC.Value.Name == "RowGUID")
                    {
                        RowGuidIncluded = true;
                        break;
                    }
                }
            }
            return RowGuidIncluded;
        }

        private string RowGUIDvalue(System.Data.DataRow R)
        {
            string RowGUID = "";
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (DC.Value.DataTable().Alias() == this.Alias() && DC.Value.Type() == DataColumn.ColumnType.Data)
                {
                    if (DC.Value.Name == "RowGUID")
                    {
                        RowGUID = " '" + R[DC.Value.DisplayText].ToString() + "'";
                        break;
                    }
                }
            }
            return RowGUID;
        }

        #endregion

        private bool UpdateData(System.Data.DataRow R, ref string Message, ref SqlCommandTypeExecuted SqlType)
        {
            bool OK = true;
            bool RowGuidIncluded = this.ColumnRowGUIDisIncluded();
            bool PKcomplete = this.PKcomplete(R);
            string RowGUID = this.RowGUIDvalue(R);

            // PK change without RowGUID
            bool PKmayChange = this.YouTryToChangeThePK(R, RowGuidIncluded);
            if (PKmayChange && !RowGuidIncluded)
            {
                Message = "Can not change PK.\r\nTry including RowGUID";
                SqlType = SqlCommandTypeExecuted.MissingRowGUID;
                return false;
            }

            // PKcollision
            if (PKmayChange && RowGuidIncluded)
            {
                if (this.DataExist(this.WhereConditions(R, false), this.WhereConditions(R, true)))
                {
                    Message = "Can not perform update.\r\nA dataset with the same key is present in the table " + this.Name + ":\r\n";
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.WhereConditions(R, false))
                        Message += "\r\n" + KV.Key + ": " + KV.Value;
                    SqlType = SqlCommandTypeExecuted.PKcollision;
                    return false;
                }
            }

            // no corresponding dataset found
            if (RowGuidIncluded && !this.DataExist(this.WhereConditions(R, true)))
            {
                Message = "No dataset with the given restriction is present in the table " + this.Name + ":\r\n";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.WhereConditions(R, true))
                    Message += "\r\n" + KV.Key + ": " + KV.Value;
                SqlType = SqlCommandTypeExecuted.NoMatch;
                return false;
            }

            if (!RowGuidIncluded && !this.DataExist(this.WhereConditions(R, false)))
            {
                Message = "No dataset with the given key is present in the table " + this.Name + ":\r\n";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.WhereConditions(R, false))
                    Message += "\r\n" + KV.Key + ": " + KV.Value;
                SqlType = SqlCommandTypeExecuted.NoMatch;
                return false;
            }

            System.Collections.Generic.Dictionary<string, string> WhereConditions = this.WhereConditions(R, PKmayChange); //new Dictionary<string, string>();

            string SetClause = this.SetClause(this.SetClauseColumns(R));
            //bool PKinvolved = false;

            //foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            //{
            //    if (DC.Value.DataTable().Alias() == this.Alias() && DC.Value.Type() == DataColumn.ColumnType.Data)
            //    {
            //        string Value = R[DC.Value.DisplayText].ToString();
            //        string columnValue = this.ColumnValue(DC.Value, Value);
            //        if (columnValue.Length > 0)
            //        {
            //            if (SetClause.Length > 0)
            //                SetClause += ", ";
            //            SetClause += columnValue;
            //        }
            //        if (DC.Value.IsLinkColumn != null && (bool)DC.Value.IsLinkColumn)
            //        {
            //            // Check if the linked column is part of the PK and if so, check if it is unique
            //            string LinkedColumn = DC.Value.RemoteLinkDisplayColumn;
            //            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> dc in this._Sheet.SelectedColumns())
            //            {
            //                if (DC.Value.DataTable().Alias() == this.Alias() && dc.Value.Type() == DataColumn.ColumnType.Data && dc.Value.Name == LinkedColumn)
            //                {
            //                    if (DC.Value.Table.PrimaryKeyColumnList.Contains(dc.Value.Name))
            //                    {
            //                        System.Collections.Generic.Dictionary<string, string> whereConditions = new Dictionary<string, string>();
            //                        System.Collections.Generic.Dictionary<string, string> notConditions = new Dictionary<string, string>();
            //                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in WhereConditions) whereConditions.Add(KV.Key, KV.Value);
            //                        string LinkedColumnValue = R[dc.Value.DisplayText].ToString();
            //                        whereConditions.Add(LinkedColumn, "'" + LinkedColumnValue + "'");
            //                        if (whereConditions.ContainsKey("RowGUID"))
            //                        {
            //                            notConditions.Add("RowGUID", whereConditions["RowGUID"]);
            //                            whereConditions.Remove("RowGUID");
            //                        }
            //                        bool Exists = DataExist(whereConditions, notConditions);
            //                        if (Exists)
            //                        {
            //                            Message = "Another dataset with the key " + WhereClause(whereConditions) + " exists. Can not perform update";
            //                            return false;
            //                        }
            //                        if (SetClause.Length > 0)
            //                            SetClause += ", ";
            //                        SetClause += dc.Value.Name + " = '" + LinkedColumnValue + "'"; // this.ColumnValue(dc.Value, LinkedColumnValue);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}


            //string SqlExists = "select count(*) from [" + this.Name + "] WHERE " + WhereClause(WhereConditions);
            //string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlExists, true);
            //if (Result.Length == 0 || Result == "0")
            //{
            //    OK = false;
            //    Message = "No data corresponding to " + WhereClause(WhereConditions) + " found.\r\nTry to include RowGUID";
            //    return OK;
            //}
            string SQL = "UPDATE " + this.Name + " SET " + SetClause + " FROM " + this.Name + " WHERE " + WhereClause(WhereConditions);
            OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message);
            if (OK)
            {
                Message = "";//Data updated";
                SqlType = SqlCommandTypeExecuted.Update;
            }
            return OK;
        }


        private bool UpdateData(System.Data.DataRow R, string WhereClause, ref string Message, ref SqlCommandTypeExecuted SqlType, bool ViaRowGUID = false)
        {
            bool OK = true;
            string SetClause = "";
            //bool PKinvolved = false;
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (DC.Value.DataTable().Alias() == this.Alias() && DC.Value.Type() == DataColumn.ColumnType.Data)
                {
                    string Value = R[DC.Value.DisplayText].ToString();
                    string columnValue = this.ColumnValue(DC.Value, Value);
                    if (columnValue.Length > 0)
                    {
                        if (SetClause.Length > 0)
                            SetClause += ", ";
                        SetClause += columnValue;
                    }
                    if (DC.Value.IsLinkColumn != null && (bool)DC.Value.IsLinkColumn)
                    {
                        // Check if the linked column is part of the PK and if so, check if it is unique
                        string LinkedColumn = DC.Value.RemoteLinkDisplayColumn;
                        foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> dc in this._Sheet.SelectedColumns())
                        {
                            if (DC.Value.DataTable().Alias() == this.Alias() && dc.Value.Type() == DataColumn.ColumnType.Data && dc.Value.Name == LinkedColumn)
                            {
                                if (DC.Value.Table.PrimaryKeyColumnList.Contains(dc.Value.Name))
                                {
                                    string LinkedColumnValue = R[dc.Value.DisplayText].ToString();
                                    string sql = "";
                                }
                            }
                        }
                    }
                    //if (this.PrimaryKeyColumnList.Contains(DC.Value.Name))
                    //{
                    //    PKinvolved = true;
                    //    //if (!ViaRowGUID)
                    //    //{
                    //    //    // do not update primary key
                    //    //    continue;
                    //    //}
                    //}
                    //else
                    //{
                    //}
                    //else if (DC.Value.Column.Table == null)
                    //    continue;
                    //else if (DC.Value.Column.ForeignRelations.Count > 0 &&
                    //    R[DC.Value.DisplayText].ToString().Length == 0 &&
                    //    this.ParentTable() != null &&
                    //    DC.Value.Column.ForeignRelationTable == this.ParentTable().Name)
                    //{
                    //    // do not try to set relations to parent tables no '' if there is no relation
                    //    continue;
                    //}
                    //else if (DC.Value.IsReadOnly())
                    //    continue;
                    //else
                    //{
                    //    if (SetClause.Length > 0)
                    //        SetClause += ", ";
                    //    if (DC.Value.Column.DataType == "geography")
                    //    {
                    //        //string Value = R[DC.Value.DisplayText].ToString();
                    //        if (Value.Length > 0)
                    //            SetClause += DC.Value.Name + " = geography::STGeomFromText('" + R[DC.Value.DisplayText].ToString() + "', 4326)";
                    //        else
                    //            SetClause += DC.Value.Name + " = NULL";
                    //    }
                    //    else if (DC.Value.Column.DataType == "bit")
                    //    {
                    //        //string Value = R[DC.Value.DisplayText].ToString();
                    //        bool bVal;
                    //        if (bool.TryParse(Value, out bVal))
                    //        {
                    //            SetClause += DC.Value.Name + " = ";
                    //            if (bVal)
                    //                SetClause += "1";
                    //            else
                    //                SetClause += "0";
                    //        }
                    //        else
                    //        {
                    //            SetClause += DC.Value.Name + " = NULL";
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (DC.Value.Column.ForeignRelations.Count > 0 && R[DC.Value.DisplayText].ToString().Length == 0)
                    //            SetClause += DC.Value.Name + " = NULL ";
                    //        else if (DC.Value.RemoteLinkDisplayColumn.Length > 0 && R[DC.Value.DisplayText].ToString().Length == 0)
                    //            SetClause += DC.Value.Name + " = NULL ";
                    //        else if ((DC.Value.Column.DataType == "tinyint"
                    //            || DC.Value.Column.DataType == "int"
                    //            || DC.Value.Column.DataType == "smallint"
                    //            || DC.Value.Column.DataType == "bigint"
                    //            || DC.Value.Column.DataType == "float"
                    //            || DC.Value.Column.DataType == "real"
                    //            || DC.Value.Column.DataType == "numeric"
                    //            || DC.Value.Column.DataType == "decimal"
                    //            || DC.Value.Column.DataType == "smallmoney"
                    //            || DC.Value.Column.DataType == "money")
                    //            && R[DC.Value.DisplayText].ToString().Length == 0)
                    //            SetClause += DC.Value.Name + " = NULL ";
                    //        else
                    //            SetClause += DC.Value.Name + " = '" + R[DC.Value.DisplayText].ToString() + "'";
                    //    }
                    //}
                }
            }
            string SqlExists = "select count(*) from [" + this.Name + "] WHERE " + WhereClause;
            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlExists, true);
            if (Result.Length == 0 || Result == "0")
            {
                OK = false;
                Message = "No data corresponding to " + WhereClause + " found.\r\nTry to include RowGUID";
                return OK;
            }
            string SQL = "UPDATE " + this.Name + " SET " + SetClause + " FROM " + this.Name + " WHERE " + WhereClause;
            OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message);
            if (OK)
            {
                Message = "";//Data updated";
                SqlType = SqlCommandTypeExecuted.Update;
            }
            return OK;
        }

        private bool UpdateData(System.Data.DataRow R, System.Collections.Generic.Dictionary<string, string> WhereConditions, ref string Message, ref SqlCommandTypeExecuted SqlType, bool ViaRowGUID = false)
        {
            bool OK = true;
            string SetClause = "";
            //bool PKinvolved = false;
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (DC.Value.DataTable().Alias() == this.Alias() && DC.Value.Type() == DataColumn.ColumnType.Data)
                {
                    string Value = R[DC.Value.DisplayText].ToString();
                    string columnValue = this.ColumnValue(DC.Value, Value);
                    if (columnValue.Length > 0)
                    {
                        if (SetClause.Length > 0)
                            SetClause += ", ";
                        SetClause += columnValue;
                    }
                    if (DC.Value.IsLinkColumn != null && (bool)DC.Value.IsLinkColumn)
                    {
                        // Check if the linked column is part of the PK and if so, check if it is unique
                        string LinkedColumn = DC.Value.RemoteLinkDisplayColumn;
                        foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> dc in this._Sheet.SelectedColumns())
                        {
                            if (DC.Value.DataTable().Alias() == this.Alias() && dc.Value.Type() == DataColumn.ColumnType.Data && dc.Value.Name == LinkedColumn)
                            {
                                if (DC.Value.Table.PrimaryKeyColumnList.Contains(dc.Value.Name))
                                {
                                    System.Collections.Generic.Dictionary<string, string> whereConditions = new Dictionary<string, string>();
                                    System.Collections.Generic.Dictionary<string, string> notConditions = new Dictionary<string, string>();
                                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in WhereConditions) whereConditions.Add(KV.Key, KV.Value);
                                    string LinkedColumnValue = R[dc.Value.DisplayText].ToString();
                                    whereConditions.Add(LinkedColumn, "'" + LinkedColumnValue + "'");
                                    if (whereConditions.ContainsKey("RowGUID"))
                                    {
                                        notConditions.Add("RowGUID", whereConditions["RowGUID"]);
                                        whereConditions.Remove("RowGUID");
                                    }
                                    bool Exists = DataExist(whereConditions, notConditions);
                                    if (Exists)
                                    {
                                        Message = "Another dataset with the key " + WhereClause(whereConditions) + " exists. Can not perform update";
                                        return false;
                                    }
                                    if (SetClause.Length > 0)
                                        SetClause += ", ";
                                    SetClause += dc.Value.Name + " = '" + LinkedColumnValue + "'"; // this.ColumnValue(dc.Value, LinkedColumnValue);
                                }
                            }
                        }
                    }
                }
            }
            string SqlExists = "select count(*) from [" + this.Name + "] WHERE " + WhereClause(WhereConditions);
            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlExists, true);
            if (Result.Length == 0 || Result == "0")
            {
                OK = false;
                Message = "No data corresponding to " + WhereClause(WhereConditions) + " found.\r\nTry to include RowGUID";
                return OK;
            }
            string SQL = "UPDATE " + this.Name + " SET " + SetClause + " FROM " + this.Name + " WHERE " + WhereClause(WhereConditions);
            OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message);
            if (OK)
            {
                Message = "";//Data updated";
                SqlType = SqlCommandTypeExecuted.Update;
            }
            return OK;
        }

        private string SetClause(System.Collections.Generic.Dictionary<string, string> SetClauseColumns)
        {
            string Set = "";
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in SetClauseColumns)
            {
                if (Set.Length > 0) Set += ", ";
                Set += KV.Key + " = " + KV.Value;
            }
            return Set;
        }

        private System.Collections.Generic.Dictionary<string, string> SetClauseColumns(System.Data.DataRow R, bool RowGUIDincluded = false)
        {
            System.Collections.Generic.Dictionary<string, string> setClause = new Dictionary<string, string>();
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                {
                    if (DC.Value.DataTable().Alias() == this.Alias() && DC.Value.Type() == DataColumn.ColumnType.Data)
                    {
                        string Value = R[DC.Value.DisplayText].ToString();
                        bool OK = true;
                        string columnValue = this.ColumnValue(DC.Value, Value, ref OK, RowGUIDincluded);
                        if (OK)
                        {
                            setClause.Add(DC.Value.Name, columnValue);
                            if (DC.Value.IsLinkColumn != null && (bool)DC.Value.IsLinkColumn)
                            {
                                string LinkedColumn = DC.Value.RemoteLinkDisplayColumn;
                                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> dc in this._Sheet.SelectedColumns())
                                {
                                    if (DC.Value.DataTable().Alias() == this.Alias() && dc.Value.Type() == DataColumn.ColumnType.Data && dc.Value.Name == LinkedColumn)
                                    {
                                        string LinkedColumnValue = R[dc.Value.DisplayText].ToString();
                                        if (dc.Value.Column.DataTypeBasicType != Data.Column.DataTypeBase.numeric)
                                            LinkedColumnValue = "'" + LinkedColumnValue + "'";
                                        if (!setClause.ContainsKey(dc.Value.Name))
                                            setClause.Add(dc.Value.Name, LinkedColumnValue); // this.ColumnValue(dc.Value, LinkedColumnValue);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }

            return setClause;
        }

        #region Column value
        private string ColumnValue(DataColumn DC, string Value)
        {
            string SetClause = "";
            if (DC.DataTable().Alias() == this.Alias() && DC.Type() == DataColumn.ColumnType.Data)
            {
                if (this.PrimaryKeyColumnList.Contains(DC.Name))
                {
                    //PKinvolved = true;
                    //if (!ViaRowGUID)
                    //{
                    //    // do not update primary keys
                    //    continue;
                    //}
                }
                else if (DC.Column.Table == null) { }
                //continue;
                else if (DC.Column.ForeignRelations.Count > 0 &&
                    Value.Length == 0 &&
                    this.ParentTable() != null &&
                    DC.Column.ForeignRelationTable == this.ParentTable().Name)
                {
                    // do not try to set relations to parent tables no '' if there is no relation
                    //continue;
                }
                else if (DC.IsReadOnly()) { }
                //continue;
                else
                {
                    if (SetClause.Length > 0)
                        SetClause += ", ";
                    if (DC.Column.DataType == "geography")
                    {
                        if (Value.Length > 0)
                            SetClause += DC.Name + " = geography::STGeomFromText('" + Value + "', 4326)";
                        else
                            SetClause += DC.Name + " = NULL";
                    }
                    else if (DC.Column.DataType == "bit")
                    {
                        bool bVal;
                        if (bool.TryParse(Value, out bVal))
                        {
                            SetClause += DC.Name + " = ";
                            if (bVal)
                                SetClause += "1";
                            else
                                SetClause += "0";
                        }
                        else
                        {
                            SetClause += DC.Name + " = NULL";
                        }
                    }
                    else
                    {
                        if (DC.Column.ForeignRelations.Count > 0 && Value.Length == 0)
                            SetClause += DC.Name + " = NULL ";
                        else if (DC.RemoteLinkDisplayColumn.Length > 0 && Value.Length == 0)
                            SetClause += DC.Name + " = NULL ";
                        else if ((DC.Column.DataType == "tinyint"
                            || DC.Column.DataType == "int"
                            || DC.Column.DataType == "smallint"
                            || DC.Column.DataType == "bigint"
                            || DC.Column.DataType == "float"
                            || DC.Column.DataType == "real"
                            || DC.Column.DataType == "numeric"
                            || DC.Column.DataType == "decimal"
                            || DC.Column.DataType == "smallmoney"
                            || DC.Column.DataType == "money")
                            && Value.Length == 0)
                            SetClause += DC.Name + " = NULL ";
                        else
                            SetClause += DC.Name + " = '" + Value + "'";
                    }
                }
            }
            return SetClause;
        }

        /// <summary>
        /// Returns values for column only if it is allowed to change the content
        /// </summary>
        /// <param name="DC"></param>
        /// <param name="Value"></param>
        /// <param name="OK">If it is OK to change the content</param>
        /// <param name="RowGuidIncluded">If the column RowGUID is included, enabling changes to the primary key</param>
        /// <returns></returns>
        private string ColumnValue(DataColumn DC, string Value, ref bool OK, bool RowGuidIncluded = false)
        {
            string SetClause = "";
            if (DC.DataTable().Alias() == this.Alias() && DC.Type() == DataColumn.ColumnType.Data)
            {
                // NO change to PK unless RowGUID is included
                if (this.PrimaryKeyColumnList.Contains(DC.Name) && !RowGuidIncluded)
                {
                    OK = false;
                    return "";
                }
                // dont get table
                if (DC.Column.Table == null)
                {
                    OK = false;
                    return "";
                }
                // do not try to set relations to parent tables no '' if there is no relation
                if (DC.Column.ForeignRelations.Count > 0 &&
                    Value.Length == 0 &&
                    this.ParentTable() != null &&
                    DC.Column.ForeignRelationTable == this.ParentTable().Name)
                {
                    OK = false;
                    return "";
                }
                // NO change to readonly columns
                if (DC.IsReadOnly())
                {
                    OK = false;
                    return "";
                }

                // getting the set clause value
                if (DC.Column.DataType == "geography")
                {
                    if (Value.Length > 0)
                        SetClause = "geography::STGeomFromText('" + Value + "', 4326)";
                    else
                        SetClause = "NULL";
                }
                else if (DC.Column.DataType == "bit")
                {
                    bool bVal;
                    if (bool.TryParse(Value, out bVal))
                    {
                        if (bVal)
                            SetClause = "1";
                        else
                            SetClause = "0";
                    }
                    else
                    {
                        SetClause = "NULL";
                    }
                }
                else
                {
                    if (DC.Column.ForeignRelations.Count > 0 && Value.Length == 0)
                        SetClause = "NULL ";
                    else if (DC.RemoteLinkDisplayColumn.Length > 0 && Value.Length == 0)
                        SetClause = "NULL ";
                    else if ((DC.Column.DataType == "tinyint"
                        || DC.Column.DataType == "int"
                        || DC.Column.DataType == "smallint"
                        || DC.Column.DataType == "bigint"
                        || DC.Column.DataType == "float"
                        || DC.Column.DataType == "real"
                        || DC.Column.DataType == "numeric"
                        || DC.Column.DataType == "decimal"
                        || DC.Column.DataType == "smallmoney"
                        || DC.Column.DataType == "money")
                        && Value.Length == 0)
                        SetClause = "NULL ";
                    else
                        SetClause = "'" + Value + "'";
                }
                OK = true;
            }
            return SetClause;
        }

        #endregion

        private bool InsertData(System.Data.DataRow R, string WhereClause, ref string Message, ref SqlCommandTypeExecuted SqlType)
        {
            bool OK = true;
            bool LinkedInViaParentTable = false;
            System.Collections.Generic.Dictionary<string, string> InsertValues = new Dictionary<string, string>();
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (DC.Value.DataTable().Alias() == this.Alias() && DC.Value.Type() == DataColumn.ColumnType.Data)
                {
                    if (this.PrimaryKeyColumnList.Contains(DC.Value.Name) && R[DC.Value.DisplayText].Equals(System.DBNull.Value))
                    {
                        if (this.ParentTable() != null
                            && this.ParentTable().DataColumns().ContainsKey(DC.Value.Name) &&
                            DC.Value.Column.ForeignRelations.ContainsKey(this.ParentTable().Name))
                        {
                            if (this.ParentTable().Type() == TableType.Project &&
                                DC.Value.Column.Name == "ProjectID" &&
                                DC.Value.Column.ForeignRelationColumn == "ProjectID")// && this._Sheet.ProjectID() != null)
                            {
                                InsertValues.Add(DC.Value.Name, this._Sheet.ProjectID().ToString());
                            }
                            else
                            {
                                string ColumnOfParentValue = this.ParentTable().DataColumns()[DC.Value.Column.ForeignRelationColumn].DisplayText;
                                if (!R.Table.Columns.Contains(ColumnOfParentValue))
                                    ColumnOfParentValue = DC.Value.Column.ForeignRelationColumn;
                                if (R.Table.Columns.Contains(ColumnOfParentValue))
                                {
                                    string ParentValue = R[ColumnOfParentValue].ToString();
                                    if (ParentValue.Length > 0)
                                    {
                                        InsertValues.Add(DC.Value.Name, "'" + ParentValue.Replace("'", "''") + "'");
                                    }
                                }
                            }
                        }
                        else if (R[DC.Value.DisplayText].Equals(System.DBNull.Value) && DC.Value.SqlQueryForDefaultForAdding != null)
                        {
                            string Result = "";
                            string Error = "";
                            if (DC.Value.SqlQueryForDefaultForAdding.IndexOf("#") > -1)
                            {
                                string SqlNew = "";
                                string[] ss = DC.Value.SqlQueryForDefaultForAdding.Split(new char[] { '#' }, StringSplitOptions.None);
                                for (int i = 0; i < ss.Length; i++)
                                {
                                    if (InsertValues.ContainsKey(ss[i]))
                                        SqlNew += InsertValues[ss[i]];
                                    else if (this.PrimaryKeyColumnList.Contains(ss[i]))
                                    {
                                        //this.DataColumns()[ss[i]].v
                                    }
                                    else
                                        SqlNew += ss[i];
                                }
                                Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlNew, ref Error);
                            }
                            else
                            {
                                Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(DC.Value.SqlQueryForDefaultForAdding, ref Error);
                            }
                            if (Error.Length == 0)
                            {
                                InsertValues.Add(DC.Value.Name, "'" + Result.Replace("'", "''") + "'");
                            }
                        }
                    }
                    else if (!R[DC.Value.DisplayText].Equals(System.DBNull.Value))
                    {
                        string Value = "";
                        if (DC.Value.Column.DataType == "geography")
                            Value = " geography::STGeomFromText('" + R[DC.Value.DisplayText].ToString() + "', 4326) ";
                        else
                            Value = "'" + R[DC.Value.DisplayText].ToString().Replace("'", "''") + "'";
                        InsertValues.Add(DC.Value.Name, Value);
                    }
                    else if ((DC.Value.SqlQueryForDefaultForAdding == null || DC.Value.SqlQueryForDefaultForAdding.Length == 0)
                        && DC.Value.DataTable().ParentTable() != null && DC.Value.DataTable().ParentTable().Name == DC.Value.Column.ForeignRelationTable)
                    {
                        // inserting the relation to a parent table if existing, e.g. if an image in the table CollectionSpecimenImage relates to the table CollectionSpecimenPart via the column SpecimenPartID
                        string ParentColumn = DC.Value.DataTable().ParentTable().DataColumns()[DC.Value.Column.ForeignRelationColumn].DisplayText;
                        string Value = "'" + R[ParentColumn].ToString().Replace("'", "''") + "'";
                        if (Value.Length > 0 && !InsertValues.ContainsKey(DC.Value.Name))
                        {
                            InsertValues.Add(DC.Value.Name, Value);
                            LinkedInViaParentTable = true;
                        }
                    }
                    else if (R[DC.Value.DisplayText].Equals(System.DBNull.Value) && DC.Value.SqlQueryForDefaultForAdding != null)
                    {
                        string Error = "";
                        string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(DC.Value.SqlQueryForDefaultForAdding, ref Error);
                    }
                }
            }
            foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in this.DataColumns())
            {
                if (!InsertValues.ContainsKey(DC.Key))
                {
                    if (DC.Value.DefaultForAdding != null && DC.Value.DefaultForAdding.Length > 0)
                    {
                        InsertValues.Add(DC.Value.Name, "'" + DC.Value.DefaultForAdding.Replace("'", "''") + "'");
                    }
                    else if (DC.Value.RestrictionValue != null && DC.Value.RestrictionValue.Length > 0)
                    {
                        InsertValues.Add(DC.Value.Name, "'" + DC.Value.RestrictionValue.Replace("'", "''") + "'");
                    }
                    else if (DC.Value.Column.IsNullable == false && DC.Value.Column.ColumnDefault == null)
                    {
                    }
                }
            }
            string ColumnClause = "";
            string ValueClause = "";
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in InsertValues)
            {
                if (ColumnClause.Length > 0)
                    ColumnClause += ", ";
                if (ValueClause.Length > 0)
                    ValueClause += ", ";
                ColumnClause += KV.Key;
                ValueClause += KV.Value;
            }
            if (LinkedInViaParentTable)
            {
                // check if the dataset is there
                string whereclause = "";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in InsertValues)
                {
                    if (this.PrimaryKeyColumnList.Contains(KV.Key))
                    {
                        if (whereclause.Length > 0) whereclause += " AND ";
                        whereclause += KV.Key + " = " + KV.Value;
                    }
                }
                string sql = "SELECT COUNT(*) FROM " + this.Name + " WHERE " + whereclause;
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(sql);
                if (Result == "1")
                {
                    sql = "";
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in InsertValues)
                    {
                        if (!this.PrimaryKeyColumnList.Contains(KV.Key))
                        {
                            if (sql.Length > 0) sql += ", ";
                            sql += KV.Key + " = " + KV.Value;
                        }
                    }
                    sql = "UPDATE T SET " + sql + " FROM " + this.Name + " AS T WHERE " + whereclause;
                    OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(sql, ref Message);
                    if (OK)
                    {
                        Message = "";//Data inserted";
                        SqlType = SqlCommandTypeExecuted.Insert;
                        return OK;
                    }
                }
            }
            string SQL = "INSERT INTO " + this.Name + " (" + ColumnClause + ") VALUES (" + ValueClause + ")";
            if (this.IdentityColumn != null && this.IdentityColumn.Length > 0)
            {
                SQL += " SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
                string ID = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
                if (Message.Length == 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                    {
                        if (DC.Value.DataTable().ParentTable() != null &&
                            DC.Value.DataTable().ParentTable().Alias() == this.Alias() &&
                            DC.Value.Type() == DataColumn.ColumnType.Data &&
                            DC.Value.Column.ForeignRelationTable != null &&
                            DC.Value.Column.ForeignRelationTable == this.Name)
                        {
                            SQL = "UPDATE T SET " + DC.Value.Name + " = " + ID + " FROM " + DC.Value.DataTable().Name + " AS T WHERE " + DC.Value.Name + " IS NULL ";
                            string AliasOfDependentTable = DC.Value.DataTable().Alias();
                            bool DependentTableContainsData = true;
                            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> PK in this._Sheet.SelectedColumns())
                            {
                                if (PK.Value.DataTable().Alias() == AliasOfDependentTable &&
                                    PK.Value.Type() == DataColumn.ColumnType.Data &&
                                    PK.Value.DataTable().PrimaryKeyColumnList.Contains(PK.Value.Name))
                                {
                                    if (R[PK.Key].Equals(System.DBNull.Value))
                                    {
                                        DependentTableContainsData = false;
                                        break;
                                    }
                                    else
                                    {
                                        SQL += " AND " + PK.Value.Name + " = '" + R[PK.Key].ToString() + "'";
                                    }
                                }
                            }
                            if (DependentTableContainsData)
                            {
                                OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message);
                            }
                        }
                    }
                }
                else
                    OK = false;
            }
            else
            {
                OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message);
            }
            if (OK)
            {
                Message = "";//Data inserted";
                SqlType = SqlCommandTypeExecuted.Insert;
            }
            else
            {
            }
            return OK;
        }

        public bool DeleteData(int Row, ref string Message)
        {
            bool OK = true;
            if (this.AllowDelete())
            {
                try
                {
                    bool PKcomplete = true;
                    System.Data.DataRow R = this._Sheet.DT().Rows[Row];
                    string WhereClause = "";
                    foreach (string P in this.PrimaryKeyColumnList)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                        {
                            if (DC.Value.DataTable().Alias() == this.Alias() && DC.Value.Type() == DataColumn.ColumnType.Data)
                            {
                                if (DC.Value.Name == P)
                                {
                                    if (R[DC.Value.DisplayText].Equals(System.DBNull.Value))
                                    {
                                        PKcomplete = false;
                                        break;
                                    }
                                    else
                                    {
                                        if (WhereClause.Length > 0)
                                            WhereClause += " AND ";
                                        if (DC.Value.Column.DataTypeBasicType == Data.Column.DataTypeBase.date)
                                        {
                                            string Date = R[DC.Value.DisplayText].ToString();
                                            System.DateTime DT;
                                            if (System.DateTime.TryParse(Date, out DT))
                                            {
                                                WhereClause += "(cast((CONVERT(DATETIME," + DC.Value.Name + ", 102)) as varchar(20)) = cast(CONVERT(DATETIME, '" + DT.Year.ToString() + "-";
                                                if (DT.Month < 10) WhereClause += "0";
                                                WhereClause += DT.Month.ToString() + "-";
                                                if (DT.Day < 10) WhereClause += "0";
                                                WhereClause += DT.Day.ToString() + " ";
                                                if (DT.Hour < 10) WhereClause += "0";
                                                WhereClause += DT.Hour.ToString() + ":";
                                                if (DT.Minute < 10) WhereClause += "0";
                                                WhereClause += DT.Minute.ToString() + ":";
                                                if (DT.Second < 10) WhereClause += "0";
                                                WhereClause += DT.Second.ToString() + "', 102)as varchar(20)))";
                                            }
                                            else
                                            {
                                                string StackMessage = "";
                                                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(new System.Diagnostics.StackFrame(true));
                                                foreach (System.Diagnostics.StackFrame sf in st.GetFrames())
                                                {
                                                    StackMessage += sf.ToString() + "\r\n";
                                                }
                                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("DiversityWorkbench.Spreadsheet.Data.Table", "DeleteData(int Row, ref string Message)", "could not convert " + Date + " into valid datetime:\r\n" + StackMessage);
                                            }
                                        }
                                        else
                                        {
                                            WhereClause += DC.Value.Name + " = '" + R[DC.Value.DisplayText].ToString() + "'";
                                        }
                                    }
                                }
                            }
                        }
                        if (!PKcomplete)
                            break;
                    }
                    if (PKcomplete)
                    {
                        string SQL = "DELETE " + this.Name + " FROM " + this.Name + " WHERE " + WhereClause;
                        OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message);
                    }
                    else
                    {
                    }
                    //System.Collections.Generic.Dictionary<string, string>
                }
                catch (System.Exception ex)
                {
                    OK = false;
                }
            }
            else
            {
                OK = false;
                Message = "You do not have the permission to delete data from the table " + this.Name;
            }
            return OK;
        }

        #endregion

        #region Color

        /// <summary>
        /// the background color for the table data
        /// </summary>
        private System.Drawing.Color _ColorBack = System.Drawing.Color.White;
        public void setColorBack(System.Drawing.Color Color)
        {
            this._ColorBack = Color;
        }
        public System.Drawing.Color ColorBack() { return this._ColorBack; }
        /// <summary>
        /// the font color of the table data
        /// </summary>
        private System.Drawing.Color _ColorFont = System.Drawing.Color.Black;
        public void setColorFont(System.Drawing.Color Color)
        {
            this._ColorFont = Color;
        }
        public System.Drawing.Color ColorFont() { return this._ColorFont; }

        public static System.Drawing.Color ColorLookup()
        {
            System.Drawing.Color C = System.Drawing.Color.FromArgb(90, 90, 90);
            return C;
        }

        public System.Drawing.Color paleColor()
        {
            float red = (255 - this._ColorBack.R) * 0.5f + this._ColorBack.R;
            float green = (255 - this._ColorBack.G) * 0.5f + this._ColorBack.G;
            float blue = (255 - this._ColorBack.B) * 0.5f + this._ColorBack.B;
            System.Drawing.Color lighterColor = System.Drawing.Color.FromArgb(this._ColorBack.A, (int)red, (int)green, (int)blue);
            return lighterColor;
        }

        #endregion

    }
}
