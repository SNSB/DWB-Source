using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Spreadsheet
{
    public struct LinkedModuleColumn
    {
        string Module;
        string BaseURL;
        string Table;
        string Column;
    }

    public class DataColumn : Data.Column
    {
        //public enum RetrievalTypes { Data, ViewOnly, Function };
        //private RetrievalTypes _RetrievalType = RetrievalTypes.Data;

        //#region ViewOnly

        //private bool _ViewOnly = false;

        //public bool ViewOnly
        //{
        //    get { return _ViewOnly; }
        //    set { _ViewOnly = value; }
        //}

        //#endregion

        #region Column

        private Data.Column _Column;

        public Data.Column Column
        {
            get { return _Column; }
            set { _Column = value; }
        }

        #endregion

        #region Visibility

        private bool _IsOutdated;
        /// <summary>
        /// If a column has been used in a former version of the database and is not supported any more
        /// </summary>
        public bool IsOutdated 
        { 
            get { return _IsOutdated; }
            set { _IsOutdated = value; }
        }

        private bool _IsVisible;

        public bool IsVisible
        {
            get 
            {
                // Markus 20.5.2019 - ensure that PK of Root etc. are included
                //if (!_IsVisible 
                //    && 
                //    (
                //    this._DataTable.Type() == Spreadsheet.DataTable.TableType.Root
                //    || this._DataTable.Type() == Spreadsheet.DataTable.TableType.Target
                //    )
                //    && this._DataTable.PrimaryKeyColumnList.Contains(this.Name)
                //    && !_IsHidden)
                //{
                //    _IsHidden = true;
                //    _IsVisible = true;
                //}
                if (_IsOutdated)
                    return false;
                else
                    return _IsVisible; 
            }
            set 
            { 
                _IsVisible = value;
                if (!value)
                    this.IsHidden = value;
                if (this._IsRequired)
                {
                    if (!this._IsVisible)
                    {
                        this._IsVisible = true;
                        this._IsHidden = true;
                    }
                }
            }
        }

        private bool _IsHidden = false;

        public bool IsHidden
        {
            get 
            {
                if (_IsOutdated)
                    return true;
                else
                    return _IsHidden; 
            }
            set { _IsHidden = value; }
        }

        private bool _IsRequired = false;

        public bool IsRequired // e.g. for display columns for linkd. These can not be removed from the displayed columns as an update would fail
        {
            get { return _IsRequired; }
            //set { _IsRequired = value; }
        }
        
        #endregion

        #region Retrieval

        public enum RetrievalType { Default, FunctionInDatabase, ViewOnly } //, SqlFromInsert }
        private RetrievalType _DataRetrievalType = RetrievalType.Default;
        /// <summary>
        /// The way data are retrieved during the import
        /// </summary>
        public RetrievalType DataRetrievalType
        {
            get { return _DataRetrievalType; }
            set
            {
                _DataRetrievalType = value;
            }
        }
        
        #endregion

        #region OrdinalPosition

        private int _OrdinalPosition;

        public int OrdinalPosition
        {
            get { return _OrdinalPosition; }
            set { _OrdinalPosition = value; }
        }
        
        #endregion

        private DataTable _DataTable;

        #region Display text

        private string _DisplayText;
        public string DisplayText
        {
            get
            {
                if (this._DisplayText == null)
                {
                    if (this._DisplayText == null || this._DisplayText.Length == 0)
                        this._DisplayText = this.DisplayTextUnique(this.Name);
                }
                return _DisplayText;
            }
            set
            {
                _DisplayText = this.DisplayTextUnique(value);
            }
        }

        private string _DisplayTextOriginal;
        /// <summary>
        /// The original display text before the mechanism of DisplayTextUnique was applied
        /// </summary>
        public string DisplayTextOriginal
        {
            get { return _DisplayTextOriginal; }
            set { _DisplayTextOriginal = value; }
        }
        
        private string _InternalRelationDisplay;
        /// <summary>
        /// The column(s) displayed for the selection of an internal relation, 
        /// e.g. within IdentificationUnit where RelatedUnitID refers to IdentificationUnitID in the same table
        /// </summary>
        public string InternalRelationDisplay
        {
            get { return _InternalRelationDisplay; }
            set { _InternalRelationDisplay = value; }
        }

        private string DisplayTextUnique(string DisplayText)
        {
            bool IsUnique = false;
            int i = 0;
            string OriginalDisplayText = DisplayText;
            while (!IsUnique)
            {
                IsUnique = true;
                foreach (System.Collections.Generic.KeyValuePair<string, DataTable> DT in this._DataTable.Sheet().DataTables())
                {
                    if (DT.Key == this._DataTable.Alias())
                        continue;
                    foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in DT.Value.DataColumns())
                    {
                        if (DC.Value._DisplayText == null)
                            continue;
                        if (DC.Value._DisplayText == DisplayText)
                        {
                            IsUnique = false;
                            break;
                        }
                    }
                    if (!IsUnique)
                        break;
                }
                if (!IsUnique)
                {
                    i++;
                    DisplayText = OriginalDisplayText + i.ToString();
                    this._DisplayTextOriginal = OriginalDisplayText;
                }
            }
            return DisplayText;
        }

        #endregion

        #region Width

        private static int _OperationWith = 14;
        public static int OperationWidth() { return _OperationWith; }

        private static int _SpacerWith = 2;
        public static int SpacerWidth() { return _SpacerWith; }

        private int _Width = 70;
        public int Width
        {
            get
            {
                if (this.Type() == ColumnType.Operation)
                    return _OperationWith;
                return _Width;
            }
            set
            {
                if (this.Type() == ColumnType.Data)
                    _Width = value;
            }
        }
        
        #endregion

        #region Type

        public enum ColumnType { Data, Operation, Link, Spacer }
        private ColumnType _ColumnType = ColumnType.Data;
        public void setColumnType(ColumnType Type)
        {
            this._ColumnType = Type;
            switch (this._ColumnType)
            {
                case ColumnType.Spacer:
                case ColumnType.Operation:
                    this.IsVisible = true;
                    break;
            }
        }
        
        public ColumnType Type()
        {
            return this._ColumnType;
        }

        #endregion

        #region Default content for adding

        private string _DefaultForAdding;

        public string DefaultForAdding
        {
            get 
            { 
                return _DefaultForAdding; 
            }
            set 
            {
                if (value.GetType() == typeof(string))
                {
                    if (value.Length == 0)
                        _DefaultForAdding = "";
                    else
                    {
                        int i;
                        if (this._Column.DataType == "int" ||
                            this._Column.DataType == "tinyint" ||
                            this._Column.DataType == "smallint")
                        {
                            if (int.TryParse(value, out i))
                                _DefaultForAdding = value;
                            else
                                System.Windows.Forms.MessageBox.Show(value + " is not a numeric value");
                        }
                        else if (this._Column.DataType == "float")
                        {
                            double d;
                            if (double.TryParse(value, out d))
                                _DefaultForAdding = value;
                            else
                                System.Windows.Forms.MessageBox.Show(value + " is not a numeric value");
                        }
                        else if (this._Column.DataType == "datetime" ||
                            this._Column.DataType == "date")
                        {
                            DateTime dt;
                            float f;
                            if (DateTime.TryParse(value, out dt))
                                _DefaultForAdding = value;
                            else if (value.Length > 4)
                                System.Windows.Forms.MessageBox.Show(value + " is not a date value");
                            else if (float.TryParse(value, out f))
                                _DefaultForAdding = value;
                        }
                        else
                            _DefaultForAdding = value;
                    }
                }
            }
        }

        private string _SqlQueryForDefaultForAdding;
        /// <summary>
        /// A query to get an valid default for a field. 
        /// The alias used for the table is T.
        /// Has to be replenished by a where clause containing the available part of the primary key
        /// e.g. ... + " WHERE T.CollectionSpecimenID = 234"
        /// </summary>
        public string SqlQueryForDefaultForAdding
        {
            get 
            {
                if (_SqlQueryForDefaultForAdding == null)
                    _SqlQueryForDefaultForAdding = "";
                return _SqlQueryForDefaultForAdding; 
            }
            set { _SqlQueryForDefaultForAdding = value; }
        }

        #endregion

        #region Read only

        private bool? _IsReadOnly = null;

        public bool IsReadOnly()
        {
            if (this.Type() == ColumnType.Data)
            {
                if (this._IsReadOnly == null)
                {
                    if (this.Name.ToLower() == "logcreatedwhen" ||
                        this.Name.ToLower() == "logcreatedby" ||
                        this.Name.ToLower() == "logupdatedwhen" ||
                        this.Name.ToLower() == "logupdatedby" ||
                        this.Name.ToLower() == "loginsertedwhen" ||
                        this.Name.ToLower() == "loginsertedby" ||
                        this.Name.ToLower() == "version" ||
                        this.Name.ToLower() == "rowguid")
                        this._IsReadOnly = true;
                    if (this.Column.IsIdentity)
                        this._IsReadOnly = true;
                    if (this.DataTable().ParentTable() != null &&
                        this.Column.ForeignRelationColumn != null &&
                        this.Column.ForeignRelationColumn.Length > 0 &&
                        this.DataTable().ParentTable().DataColumns().ContainsKey(this.Column.ForeignRelationColumn) &&
                        this.DataTable().ParentTable().DataColumns()[this.Column.ForeignRelationColumn].Column.IsIdentity)
                        this._IsReadOnly = true;
                    if (this.DataTable().ParentTable() != null &&
                        this.Column.ForeignRelationColumn != null &&
                        this.Column.ForeignRelationColumn.Length > 0 &&
                        this.DataTable().ParentTable().Name == this.Column.ForeignRelationTable)
                        this._IsReadOnly = true;
                    if (this._IsReadOnly == null)
                        this._IsReadOnly = false;
                }
                return (bool)this._IsReadOnly;
            }
            else
                return true;
        }

        public void setReadOnly(bool ReadOnly) { this._IsReadOnly = ReadOnly; }
        
        #endregion

        #region Order
        
        public enum OrderByDirection { none, ascending, descending }
        private OrderByDirection _OrderDirection = OrderByDirection.none;

        public OrderByDirection OrderDirection
        {
            get { return _OrderDirection; }
            set 
            {
                _OrderDirection = value;
                if (this.DisplayText.Length > 0)
                    this._DataTable.Sheet().SetOrderColumn("[" + this.DisplayText + "]", value);
                else
                    this._DataTable.Sheet().SetOrderColumn("[" + this._DataTable.Alias() + "].[" + this.Name + "]", value);
            }
        }

        private int? _OrderSequence = null;

        public int? OrderSequence
        {
            get { return _OrderSequence; }
            set 
            {
                if (this._IsVisible)
                {
                    if (this.DisplayText.Length > 0)
                        _OrderSequence = this._DataTable.Sheet().SetOrderSequence(value, "[" + this.DisplayText + "]");
                    else
                        _OrderSequence = this._DataTable.Sheet().SetOrderSequence(value, "[" + this._DataTable.Alias() + "].[" + this.Name + "]");

                }
                else
                    _OrderSequence = null;
            }
        }
        
        #endregion        

        #region Lookup source

        private string _SqlLookupSource;

        /// <summary>
        /// SQL query for a table with 2 columns: Value and Display
        /// </summary>
        public string SqlLookupSource
        {
            get { return _SqlLookupSource; }
            set { _SqlLookupSource = value; }
        }

        private System.Data.DataTable _LookupSource;
        /// <summary>
        /// Table with 2 columns: Value and Display
        /// </summary>
        public System.Data.DataTable LookupSource
        {
            get
            {
                if (this._LookupSource == null)
                {
                    if (this._SqlLookupSource != null && this._SqlLookupSource.Length > 0)
                    {
                        string SQL = this._SqlLookupSource;
                        if (this._SqlLookupSource.IndexOf("#") > -1)
                        {
                            if (this.DataTable().DataColumns()[this.DataTable().RestrictionColumns[0]].RestrictionValue == null)
                                return null;
                            else
                            {
                                SQL = "";
                                string[] ssQL = this._SqlLookupSource.Split(new char[] { '#' });
                                for (int i = 0; i < ssQL.Length; i++)
                                {
                                    if (ssQL[i] == this.DataTable().RestrictionColumns[0])
                                    {
                                        if (this.DataTable().DataColumns()[this.DataTable().RestrictionColumns[0]].Column.DataTypeBasicType != DataTypeBase.numeric)
                                            SQL += "'";
                                        SQL += this.DataTable().DataColumns()[this.DataTable().RestrictionColumns[0]].RestrictionValue;
                                        if (this.DataTable().DataColumns()[this.DataTable().RestrictionColumns[0]].Column.DataTypeBasicType != DataTypeBase.numeric)
                                            SQL += "'";
                                    }
                                    else
                                        SQL += ssQL[i];
                                }
                            }
                        }
                        this._LookupSource = new System.Data.DataTable();
                        try
                        {
                            // Timeout restricted to 1 second for lookup tables
                            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(1));
                            ad.SelectCommand.CommandTimeout = 1;
                            ad.Fill(this._LookupSource);
                        }
                        catch (System.Exception ex)
                        {
                            if (ex.Message.ToLower().IndexOf("timeout") == -1)
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
                        }
                    }
                }
                return _LookupSource;
            }
            //set { _LookupSource = value; }
        }
        
        #endregion

        #region Restriction

        private bool _IsRestrictionColumn;

        public bool IsRestrictionColumn
        {
            get { return _IsRestrictionColumn; }
            set { _IsRestrictionColumn = value; }
        }

        private string _RestrictionValue;

        public string RestrictionValue
        {
            get { return _RestrictionValue; }
            set { _RestrictionValue = value; }
        }
        
        #endregion

        #region Filter

        private bool _FilterExclude = false;

        // if this column is excluded from a filter set by the user e.g. if this is done by a SQL restriction like for the column IdentificationSequence in the table first identification
        public bool FilterExclude
        {
            get { return _FilterExclude; }
            set { _FilterExclude = value; }
        }

        private string _FilterOperator = "=";

        public string FilterOperator
        {
            get { return _FilterOperator; }
            set { _FilterOperator = value; }
        }

        private string _FilterValue;

        public string FilterValue
        {
            get { return _FilterValue; }
            set 
            {
                if (value == null || value.Length == 0)
                    _FilterValue = "";
                else
                {
                    if (this._Column.Table == null)
                    {
                        _FilterValue = value;
                    }
                    else if (this._Column.DataTypeBasicType == DataTypeBase.numeric)
                    {
                        if (this._FilterOperator != "|" && this._FilterOperator != "‡" && this._FilterOperator != "∈" && this._FilterOperator != "∉")
                        {
                            if (this._Column.DataType == "float")
                            {
                                double d;
                                if (double.TryParse(value, out d))
                                    _FilterValue = value;
                                else
                                    System.Windows.Forms.MessageBox.Show(value + " is not a numeric value");
                            }
                            else
                            {
                                int i;
                                if (int.TryParse(value, out i))
                                    _FilterValue = value;
                                else
                                    System.Windows.Forms.MessageBox.Show(value + " is not a numeric value");
                            }
                        }
                        else
                            _FilterValue = value;
                    }
                    else if (this._Column.DataTypeBasicType == DataTypeBase.date)
                    {
                        DateTime dt;
                        float f;
                        if (DateTime.TryParse(value, out dt))
                            _FilterValue = value;
                        else if (value.Length > 4)
                            System.Windows.Forms.MessageBox.Show(value + " is not a date value");
                        else if (float.TryParse(value, out f))
                            _DefaultForAdding = value;
                    }
                    else
                        _FilterValue = value;
                }
            }
        }

        public string[] FilterValueArray()
        {
            string[] FF = this.FilterValue.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (FF.Length == 1 && this.FilterValue.IndexOf("\n") > -1)
            {
                FF = this.FilterValue.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            }
            return FF;
        }

        
        private string _FilterModuleLinkRoot = "";

        public string FilterModuleLinkRoot
        {
            get { return _FilterModuleLinkRoot; }
            set { _FilterModuleLinkRoot = value; }
        }

        private string _SqlForColumn = "";
        /// <summary>
        /// If a column should be retrieved via a alternative statement, e.g. for Geography - ...EnvelopeCenter().ToString() etc.
        /// </summary>
        public string SqlForColumn
        {
            get { return _SqlForColumn; }
            set 
            { 
                _SqlForColumn = value; 
            }
        }

        public string WhereClause()
        {
            string WhereClause = "";
            if (this._Column.Table == null && this.SqlForColumn.Length > 0)
            {
                WhereClause = this.SqlForColumn.Replace("#TableAlias#", this.DataTable().Alias()) + this.FilterValueForSql();
            }
            else
            {
                if (this.Column.DataType == "geography")
                    WhereClause = this.DataTable().Alias() + ".[" + this.Name + "].ToString()" + this.FilterValueForSql();
                else
                    WhereClause = this.DataTable().Alias() + ".[" + this.Name + "]" + this.FilterValueForSql();
            }
            return WhereClause;
        }

        private string FilterValueForSql()
        {
            string Filter = "";
            if (this.FilterOperator.Length == 1)
            {
                switch (this.FilterOperator) // Change Filteroperator where needed
                {
                    case "≠":
                        Filter = " <> ";
                        break;
                    case "Ø":
                        Filter = " IS ";
                        break;
                    case "•":
                        Filter = " <> ";
                        break;
                    case "~":
                        Filter = " LIKE ";
                        break;
                    case "¬":
                        Filter = " NOT LIKE ";
                        break;
                    default:
                        Filter = " " + this._FilterOperator + " ";
                        break;
                }
            }
            if (this._Column.Table == null)
            {
                switch (this.FilterOperator)
                {
                    case "+H":
                    case "+S":
                    case "+H+S":
                    case "∈":
                    case "|":
                        string[] FF = this.FilterValueArray();// .Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        if (FF.Length > 0)
                        {
                            Filter = " IN (";
                            for (int iF = 0; iF < FF.Length; iF++)
                            {
                                if (iF > 0) Filter += ", ";
                                if (!FF[iF].StartsWith("'"))
                                    Filter += "'";
                                Filter += FF[iF];
                                if (!FF[iF].EndsWith("'"))
                                    Filter += "'";
                            }
                            Filter += ") ";
                        }
                        break;
                    case "∉":
                        string[] NonFilter = this.FilterValueArray();// .FilterValue.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        if (NonFilter.Length > 0)
                        {
                            Filter = " NOT IN (";
                            for (int iF = 0; iF < NonFilter.Length; iF++)
                            {
                                if (iF > 0) Filter += ", ";
                                if (!NonFilter[iF].StartsWith("'"))
                                    Filter += "'";
                                Filter += "'" + NonFilter[iF] + "'";
                                if (!NonFilter[iF].EndsWith("'"))
                                    Filter += "'";
                            }
                            Filter += ") ";
                        }
                        break;
                    default:
                        if (!this._FilterValue.StartsWith("'"))
                            Filter += "'";
                        Filter += this._FilterValue;
                        if (!this._FilterValue.EndsWith("'"))
                            Filter += "'";
                        break;
                }
            }
            else
            {
                switch (this.Column.DataTypeBasicType)
                {
                    case DataTypeBase.date:
                        System.DateTime DT;
                        if (System.DateTime.TryParse(this._FilterValue, out DT))
                        {
                            if (this.FilterOperator == "=")
                            {
                                Filter = " BETWEEN CONVERT(DATETIME, '"
                                    + DT.Year.ToString() + "-"
                                    + DT.Month.ToString() + "-"
                                    + DT.Day.ToString() + " 00:00:00', 102) AND CONVERT(DATETIME, '"
                                    + DT.Year.ToString() + "-"
                                    + DT.Month.ToString() + "-"
                                    + DT.Day.ToString() + " 23:59:59', 102)";
                            }
                            else
                                Filter = " " + this.FilterOperator + " CONVERT(DATETIME, '" + DT.Year.ToString() + "-" + DT.Month.ToString() + "-" + DT.Day.ToString() + " 00:00:00', 102)";
                        }
                        return Filter;
                    default:
                        switch (this.FilterOperator)
                        {
                            case "+H":
                            case "+S":
                            case "+H+S":
                            case "∈":
                            case "|":
                                string[] FF = this.FilterValueArray();// .FilterValue.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                                //if (FF.Length == 1 && this.FilterValue.IndexOf("\n") > -1)
                                //{
                                //    FF = this.FilterValue.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                                //}
                                if (FF.Length > 0)
                                {
                                    Filter = " IN (";
                                    for (int iF = 0; iF < FF.Length; iF++)
                                    {
                                        if (iF > 0) Filter += ", ";
                                        if (this.Column.DataTypeBasicType == DataTypeBase.text)
                                            Filter += "'";
                                        Filter += FF[iF];
                                        if (this.Column.DataTypeBasicType == DataTypeBase.text)
                                            Filter += "'";
                                    }
                                    Filter += ") ";
                                }
                                break;
                            case "∉":
                                string[] NonFilter = this.FilterValue.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                                if (NonFilter.Length == 1 && this.FilterValue.IndexOf("\n") > -1)
                                {
                                    NonFilter = this.FilterValue.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                                }
                                if (NonFilter.Length > 0)
                                {
                                    Filter = " NOT IN (";
                                    for (int iF = 0; iF < NonFilter.Length; iF++)
                                    {
                                        if (iF > 0) Filter += ", ";
                                        if (this.Column.DataTypeBasicType == DataTypeBase.text)
                                            Filter += "'";
                                        Filter += NonFilter[iF];
                                        if (this.Column.DataTypeBasicType == DataTypeBase.text)
                                            Filter += "'";
                                    }
                                    Filter += ") ";
                                }
                                break;
                            case "Ø":
                            case "•":
                                Filter += this._FilterValue;
                                break;
                            default:
                                if (this.Column.DataTypeBasicType == DataTypeBase.text)
                                    Filter += "'";
                                Filter += this._FilterValue;
                                if (this.Column.DataTypeBasicType == DataTypeBase.text)
                                    Filter += "'";
                                break;
                        }
                        break;
                }
            }
            return Filter;
        }

        #endregion

        #region Link
        /// <summary>
        /// if the data column represents a link, e.g. a link to a module or a resource
        /// </summary>
        private bool? _IsLinkColumn = null;

        public bool? IsLinkColumn
        {
            get 
            {
                if (this._IsLinkColumn == null)
                {
                    if (this._TypeOfLink == LinkType.None)
                    {
                        this._IsLinkColumn = false;
                    }
                    else if (this._TypeOfLink != LinkType.OptionalLinkToDiversityWorkbenchModule)
                        this._IsLinkColumn = true;
                    else
                    {
                    }
                }
                return this._IsLinkColumn;
            }
            //set { _IsLinkColumn = value; }
        }

        public bool IsLinkedColumn()
        {
            if (this._IsLinkColumn != null && (bool)this._IsLinkColumn)
                return true;
            else
                return false;
        }

        public void SetIsLinkColumn(bool IsLink)
        {
            if (this._TypeOfLink == LinkType.OptionalLinkToDiversityWorkbenchModule)
                this._IsLinkColumn = IsLink;
        }


        private DataColumn _LinkedToColumn = null;
        public DataColumn LinkedToColumn
        {
            get 
            { 
                return _LinkedToColumn; 
            }
            set
            {
                _LinkedToColumn = value;
                if (_LinkedToColumn != null) // e.g. if the column Family should be linked to a taxonomic source, it can not be removed from the selected columns a an update would fail
                    this._IsRequired = true;
            }
        }

        /// <summary>
        /// the list of column display texts linked to this column, e.g. FamilyCache, OrderCache, ... to NameURI
        /// </summary>
        private System.Collections.Generic.List<string> _LinkedColumns;

        public System.Collections.Generic.List<string> LinkedColumns()
        {
            if (this._LinkedColumns == null)
            {
                this._LinkedColumns = new List<string>();
                if (this._RemoteLinks != null)
                {
                    foreach (RemoteLink RL in this._RemoteLinks)
                    {
                        foreach (RemoteColumnBinding RCB in RL.RemoteColumnBindings)
                        {
                            if (!this._LinkedColumns.Contains(RCB.Column.DisplayText))
                                this._LinkedColumns.Add(RCB.Column.DisplayText);
                        }
                    }
                }
            }
            return this._LinkedColumns;
        }

        public enum LinkType { None, DiversityWorkbenchModule, OptionalLinkToDiversityWorkbenchModule, Web, Resource }
        private LinkType _TypeOfLink = LinkType.None;
        public LinkType TypeOfLink
        {
            get { return _TypeOfLink; }
            set
            {
                _TypeOfLink = value;
                //this._IsLinkColumn = true;
            }
        }

        private DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule _LinkedModule = RemoteLink.LinkedModule.None;

        public DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule LinkedModule
        {
            get { return _LinkedModule; }
            set { _LinkedModule = value; }
        }

        private bool _RemoteLinkIsOptional;

        public bool RemoteLinkIsOptional
        {
            get { return _RemoteLinkIsOptional; }
            //set { _RemoteLinkIsOptional = value; }
        }
        private string _RemoteLinkDecisionColmn;

        public string RemoteLinkDecisionColmn
        {
            get { return _RemoteLinkDecisionColmn; }
            //set { _RemoteLinkDecisionColmn = value; }
        }
        private string _RemoteLinkDisplayColumn;

        public string RemoteLinkDisplayColumn
        {
            get 
            {
                if (_RemoteLinkDisplayColumn == null)
                    _RemoteLinkDisplayColumn = "";
                return _RemoteLinkDisplayColumn; 
            }
            //set { _RemoteLinkDisplayColumn = value; }
        }
        private System.Collections.Generic.List<RemoteLink> _RemoteLinks;

        public System.Collections.Generic.List<RemoteLink> RemoteLinks
        {
            get { return _RemoteLinks; }
            //set { _RemoteLinks = value; }
        }

        public void setRemoteLinks(
            bool IsOptional,
            string RemoteLinkDecisionColmn,
            string RemoteLinkDisplayColumn,
            System.Collections.Generic.List<RemoteLink> RemoteLinks,
            LinkType Type)
        {
            this._RemoteLinkIsOptional = IsOptional;
            this._RemoteLinkDecisionColmn = RemoteLinkDecisionColmn;
            this._RemoteLinkDisplayColumn = RemoteLinkDisplayColumn;
            this.setRemoteLinks(RemoteLinks);
            this._TypeOfLink = Type;
        }

        public void setRemoteLinks(
            string RemoteLinkDisplayColumn,
            System.Collections.Generic.List<RemoteLink> RemoteLinks)
        {
            try
            {
                this._RemoteLinkIsOptional = false;
                this._IsRequired = true;
                this.DataTable().DataColumns()[RemoteLinkDisplayColumn]._IsRequired = true;
                this._RemoteLinkDecisionColmn = "";
                this._RemoteLinkDisplayColumn = RemoteLinkDisplayColumn;
                this.setRemoteLinks(RemoteLinks);
                this._TypeOfLink = LinkType.DiversityWorkbenchModule;
                this._LinkedModule = RemoteLinks[0].LinkedToModule;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void setRemoteLinks(
            string RemoteLinkDisplayColumn, DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule Module)
        {
            try
            {
                this._RemoteLinkIsOptional = false;
                this._RemoteLinkDecisionColmn = "";
                this._RemoteLinkDisplayColumn = RemoteLinkDisplayColumn;
                this._RemoteLinks = null;
                this._TypeOfLink = LinkType.DiversityWorkbenchModule;
                this._LinkedModule = Module;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void setRemoteLinks(System.Collections.Generic.List<RemoteLink> RemoteLinks)
        {
            this._RemoteLinks = RemoteLinks;
            if (this._RemoteLinks != null)
            {
                foreach (RemoteLink RL in this._RemoteLinks)
                {
                    if (RL.RemoteColumnBindings != null)
                    {
                        foreach (RemoteColumnBinding RCB in RL.RemoteColumnBindings)
                        {
                            RCB.Column.LinkedToColumn = this;
                        }
                    }
                }
            }
        }

        private bool _FixedSourceNeedsValueFromData = false;
        public bool FixedSourceNeedsValueFromData() { return this._FixedSourceNeedsValueFromData; }
        private System.Collections.Generic.List<string> _FixedSourceSetting = null;
        /// <summary>
        /// The settings for a column. Only needed if non standard e.g. Link for Identification depending on TaxonomicGroup
        /// otherwise the Table and column are used for retrieval of settings
        /// </summary>
        /// <param name="Setting">The list of the settings</param>
        /// <param name="NeedsValuesFromData">If any of the values are retrieved from the data. In this case the value are composed as follows: alias of the table + . + name of the column, e.g. the column TaxonomicGroup in table IdentificationUnit for identfications </param>
        public void FixedSourceSetSetting(System.Collections.Generic.List<string> Setting, bool NeedsValuesFromData)
        {
            this._FixedSourceSetting = Setting;
            this._FixedSourceNeedsValueFromData = NeedsValuesFromData;
        }

        public string FixedSourceValueFromDataTableAlias = "";
        public string FixedSourceValueFromDataColumnName = "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RowIndex"></param>
        /// <returns></returns>
        public System.Collections.Generic.List<string> FixedSourceGetSetting(string Value = "")
        {
            if (this._FixedSourceSetting == null 
                && this._IsLinkColumn != null && (bool)this._IsLinkColumn
                && this._RemoteLinkDisplayColumn != null && this._RemoteLinkDisplayColumn.Length > 0)
            {
                this._FixedSourceSetting = new List<string>();
                this._FixedSourceSetting.Add("ModuleSource");
                this._FixedSourceSetting.Add(this.DataTable().Name);
                if (this._FixedSourceNeedsValueFromData)
                {
                }
                else
                {
                    this._FixedSourceSetting.Add(this.Name);
                }
            }
            if (Value.Length > 0)
            {
                if (this._FixedSourceSetting.Last().IndexOf(".") > -1)
                {
                    System.Collections.Generic.List<string> L = new List<string>();
                    for (int i = 0; i < this._FixedSourceSetting.Count - 1; i++)
                        L.Add(this._FixedSourceSetting[i]);
                    L.Add(Value);
                    return L;
                }
                else
                    return _FixedSourceSetting;
            }
            return this._FixedSourceSetting;
        }

        /// <summary>
        /// If the column is a link column and settings are available
        /// </summary>
        /// <returns></returns>
        public bool FixedSourceIsFixed(string Value = "")
        {
            if (this._IsLinkColumn != null && (bool)this._IsLinkColumn && this._LinkedModule != null && this._RemoteLinkDisplayColumn != null && this._RemoteLinkDisplayColumn.Length > 0)
            {
                DiversityWorkbench.UserSettings US = new UserSettings();
                string Settings = US.GetSetting(this.FixedSourceGetSetting(Value), "Database");
                if (Settings.Length > 0)
                {
                    return true;
                }
                else
                {
                    Settings = US.GetSetting(this.FixedSourceGetSetting(Value), "Webservice");
                    if (Settings.Length > 0)
                        return true;
                }
            }
            return false;
        }
        #endregion

        #region Construction
        
        public DataColumn(DataTable DataTable, Data.Column Column) :
            base(Column.Name, (Data.Table)DataTable)
        {
            if (DataTable == null)
                this.DataRetrievalType = RetrievalType.ViewOnly;
            else
                this._DataTable = DataTable;
            this._Column = Column;
        }
        
        #endregion

        public DataTable DataTable() { return this._DataTable; }

    }
}
