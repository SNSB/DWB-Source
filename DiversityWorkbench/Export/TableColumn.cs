using DWBServices.WebServices.TaxonomicServices;
using DWBServices.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DWBServices;
using System.Windows.Forms;

namespace DiversityWorkbench.Export
{

    //public interface iTableColumn
    //{

    //}

    public class TableColumn// : Export.iTableColumn
    {
        #region Properties

        #region Name & Table

        private string _ColumnName;

        public string ColumnName
        {
            get { return _ColumnName; }
            //set { _ColumnName = value; }
        }

        private string _SQL;
        public string SQL { get { return this._SQL; } }

        private string _DisplayText;

        public string DisplayText
        {
            get { return _DisplayText; }
            set { _DisplayText = value; }
        }

        private DiversityWorkbench.Export.Table _Table;
        public DiversityWorkbench.Export.Table Table
        {
            get { return _Table; }
            //set { _Table = value; }
        }

        private string _DataType;
        /// <summary>
        /// the data type of the column as defined in the database
        /// </summary>
        public string DataType
        {
            get { return _DataType; }
            set { _DataType = value; }
        }

        private int _DataTypeLength;
        /// <summary>
        /// The length in the definition of the datatype for e.g. character columns as defined in the database
        /// </summary>
        public int DataTypeLength
        {
            get { return _DataTypeLength; }
            set { _DataTypeLength = value; }
        }

        #endregion

        #region Grouping

        public enum Grouping { None, GroupBy, Maximum, Minimum }
        private Grouping _ColumnGrouping;
        public Grouping ColumnGrouping
        {
            get { return _ColumnGrouping; }
            set { _ColumnGrouping = value; }
        }

        #endregion

        #region Sorting

        public enum Sorting { notsorted, descending, ascending }
        private Sorting _SortingType;
        private int _SortingSequence;

        public int SortingSequence
        {
            get { return _SortingSequence; }
            set { _SortingSequence = value; }
        }

        public Sorting SortingType
        {
            get { return _SortingType; }
            set { _SortingType = value; }
        }

        public string SqlSortingTyp
        {
            get
            {
                if (this.SortingType == Sorting.ascending) return "ASC";
                else if (this.SortingType == Sorting.descending) return "DESC";
                else return "";
            }
        }

        #endregion

        #region Relations to Workbench module or table or sebservice

        private DiversityWorkbench.IWorkbenchUnit _IWorkbenchUnit;
        /// <summary>
        /// The Workbench unit as derived from the string DiversityWorkbenchModule
        /// </summary>
        public DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit
        {
            get
            {
                if (_IWorkbenchUnit == null)
                {
                    switch (this.DiversityWorkbenchModule)
                    {
                        case "DiversityAgents":
                            Agent A = new Agent(DiversityWorkbench.Settings.ServerConnection);
                            _IWorkbenchUnit = A;
                            break;
                        case "DiversityCollection":
                            CollectionSpecimen C = new CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
                            _IWorkbenchUnit = C;
                            break;
                        case "DiversityDescriptions":
                            Description D = new Description(DiversityWorkbench.Settings.ServerConnection);
                            _IWorkbenchUnit = D;
                            break;
                        case "DiversityExsiccatae":
                            Exsiccate E = new Exsiccate(DiversityWorkbench.Settings.ServerConnection);
                            _IWorkbenchUnit = E;
                            break;
                        case "DiversityGazetteer":
                            DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].getServerConnection();
                            Gazetteer G = new Gazetteer(SC);
                            _IWorkbenchUnit = G;
                            break;
                        case "DiversityProjects":
                            Project P = new Project(DiversityWorkbench.Settings.ServerConnection);
                            _IWorkbenchUnit = P;
                            break;
                        case "DiversityReferences":
                            Reference R = new Reference(DiversityWorkbench.Settings.ServerConnection);
                            _IWorkbenchUnit = R;
                            break;
                        case "DiversitySamplingPlots":
                            SamplingPlot SP = new SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                            _IWorkbenchUnit = SP;
                            break;
                        case "DiversityScientificTerms":
                            ScientificTerm ST = new ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                            _IWorkbenchUnit = ST;
                            break;
                        case "DiversityTaxonNames":
                            //if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this.DiversityWorkbenchModule].ServerConnectionList().ContainsKey(
                            TaxonName T = new TaxonName(DiversityWorkbench.Settings.ServerConnection);
                            _IWorkbenchUnit = T;
                            break;
                    }
                }
                return _IWorkbenchUnit;
            }
            //set { _IWorkbenchUnit = value; }
        }

        public void setWorkbenchUnitConnection(string SelectedService)
        {
            DiversityWorkbench.ServerConnection S;
            if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this.DiversityWorkbenchModule].ServerConnectionList().ContainsKey(SelectedService))
            {
                S = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this.DiversityWorkbenchModule].ServerConnectionList()[SelectedService];
                this.IWorkbenchUnit.setServerConnection(S);
            }
        }

        private string _DiversityWorkbenchModule;
        /// <summary>
        /// If the column is designed as a relation to a remote module, the name of the module
        /// </summary>
        public string DiversityWorkbenchModule
        {
            get { return _DiversityWorkbenchModule; }
            set { _DiversityWorkbenchModule = value; }
        }

        private System.Collections.Generic.Dictionary<string, string> _DiversityWorkbenchModuleSources;
        /// <summary>
        ///  the possible sources for the module values in the current data
        /// </summary>
        public System.Collections.Generic.Dictionary<string, string> DiversityWorkbenchModuleSources
        {
            get
            {
                if (_DiversityWorkbenchModuleSources == null || _DiversityWorkbenchModuleSources.Count == 0)
                {
                    this._DiversityWorkbenchModuleSources = new Dictionary<string, string>();
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.WorkbenchUnit> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList())
                    {
                        if (KV.Key == this.DiversityWorkbenchModule)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KVconn in KV.Value.ServerConnectionList())
                            {
                                if (KVconn.Key.Length > 0 && KVconn.Value.BaseURL != null && !_DiversityWorkbenchModuleSources.ContainsKey(KVconn.Value.BaseURL))
                                {
                                    _DiversityWorkbenchModuleSources.Add(KVconn.Value.BaseURL, KVconn.Key);
                                }
                            }
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KVservice in KV.Value.AccessibleDatabasesAndServicesOfModule())
                            {
                                if (KV.Key != DiversityWorkbench.Settings.ServerConnection.ModuleName)
                                {
                                    if (!KV.Value.ServerConnectionList().ContainsKey(KVservice.Key))
                                    {
                                        string key = "";
                                        if (KV.Value.DatabaseAndServiceURIs().TryGetValue(KVservice.Key, out key))
                                            _DiversityWorkbenchModuleSources.Add(key, KVservice.Key);
                                    }
                                }
                            }
                        }
                    }
                }
                return _DiversityWorkbenchModuleSources;
            }
            //set { _DiversityWorkbenchModuleSources = value; }
        }

        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> _DiversityWorkbenchModuleSelectedUnitValues;
        /// <summary>
        /// The list of selected unit values received from a remote module
        /// </summary>
        public System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> DiversityWorkbenchModuleSelectedUnitValues
        {
            get
            {
                if (this._DiversityWorkbenchModuleSelectedUnitValues == null)
                    this._DiversityWorkbenchModuleSelectedUnitValues = new Dictionary<string, List<string>>();
                return _DiversityWorkbenchModuleSelectedUnitValues;
            }
            //set { _DiversityWorkbenchModuleUnitValues = value; }
        }

        public void addDiversityWorkbenchModuleUnitValue(string Service, string UnitValue)
        {
            if (!this._DiversityWorkbenchModuleSelectedUnitValues.ContainsKey(Service))
            {
                System.Collections.Generic.List<string> L = new List<string>();
                L.Add(UnitValue);
                this._DiversityWorkbenchModuleSelectedUnitValues.Add(Service, L);
            }
            else
            {
                if (!this._DiversityWorkbenchModuleSelectedUnitValues[Service].Contains(UnitValue))
                    this._DiversityWorkbenchModuleSelectedUnitValues[Service].Add(UnitValue);
            }
        }

        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> _DiversityWorkbenchModulePossibleUnitValues;

        public System.Collections.Generic.List<string> getDiversityWorkbenchModulePossibleUnitValues(string Service)
        {
            if (this._DiversityWorkbenchModulePossibleUnitValues == null)
                this._DiversityWorkbenchModulePossibleUnitValues = new Dictionary<string, List<string>>();
            if (!this._DiversityWorkbenchModulePossibleUnitValues.ContainsKey(Service))
            {
                DiversityWorkbench.DatabaseService S;
                foreach (DiversityWorkbench.DatabaseService d in this.IWorkbenchUnit.DatabaseServices())
                {
                    if (d.DisplayText == Service)
                    {
                        S = d;
                        if (!this._DiversityWorkbenchModulePossibleUnitValues.ContainsKey(Service))
                        {
                            if (d.IsWebservice)
                            {
                                System.Collections.Generic.List<string> PossibleValues = new List<string>();
                                PossibleValues = GetDwbServicePossibleEntityValues(d.WebService);
                                this._DiversityWorkbenchModulePossibleUnitValues.Add(Service, PossibleValues);
                            }
                            else
                            {
                                System.Collections.Generic.Dictionary<string, string> Values = IWorkbenchUnit.UnitValues(-1);
                                System.Collections.Generic.List<string> ValueList = new List<string>();
                                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Values)
                                {
                                    if (!ValueList.Contains(KV.Key))
                                        ValueList.Add(KV.Key);
                                }
                                this._DiversityWorkbenchModulePossibleUnitValues.Add(Service, ValueList);
                            }
                        }
                        break;
                    }
                }
            }
            if (!this._DiversityWorkbenchModulePossibleUnitValues.ContainsKey(Service) &&
                DiversityWorkbench.LinkedServer.LinkedServers().Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.LinkedServer> KV in DiversityWorkbench.LinkedServer.LinkedServers())
                {
                    if (Service.StartsWith("[" + KV.Key + "]."))
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, LinkedServerDatabase> KVDB in KV.Value.Databases())
                        {
                            if (Service.EndsWith(KVDB.Key))
                            {
                                System.Collections.Generic.Dictionary<string, string> Values = IWorkbenchUnit.UnitValues(-1);
                                System.Collections.Generic.List<string> ValueList = new List<string>();
                                foreach (System.Collections.Generic.KeyValuePair<string, string> kv in Values)
                                {
                                    if (!ValueList.Contains(kv.Key))
                                        ValueList.Add(kv.Key);
                                }
                                this._DiversityWorkbenchModulePossibleUnitValues.Add(Service, ValueList);
                            }
                        }
                    }
                }
            }
            return this._DiversityWorkbenchModulePossibleUnitValues[Service];
        }

        private List<string> GetDwbServicePossibleEntityValues(DwbServiceEnums.DwbService service)
        {
            try
            {
                System.Collections.Generic.List<string> PossibleValues = new List<string>();
                IDwbWebservice<DwbSearchResult, DwbSearchResultItem, DwbEntity> _api =
                    DwbServiceProviderAccessor.GetDwbWebservice(service);
                if (_api == null)
                    return PossibleValues;
                DwbEntity clientEntity = _api.GetEmptyDwbApiDetailModel();
                System.Data.DataTable dtQuery = new System.Data.DataTable();
                ReadDwbDetailModelInQueryTable(clientEntity, ref dtQuery);
                
                foreach (System.Data.DataColumn C in dtQuery.Columns)
                {
                    // TODO Ariane at the moment only _DisplayTest is filled in TableColumnUnitValue. If we want to select all possible values, like _URI.. as well we need to adjust this in TableColumnUnitValue
                    if (C.ColumnName == "_DisplayText")
                        PossibleValues.Add(C.ColumnName);
                }
                return PossibleValues;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("An error occurred when trying to synchronize with the webservice: " + ex.Message, "SynchronizeError", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return new List<string>();
            }
        }

        private void ReadDwbDetailModelInQueryTable(DwbEntity dwbEntity, ref System.Data.DataTable dtQuery)
        {
            try
            {
                if (dwbEntity is null)
                    return;
                if (dwbEntity is TaxonomicEntity)
                {
                    TaxonomicEntity taxonomicEntity = (TaxonomicEntity)dwbEntity;

                    dtQuery.Columns.Clear();
                    var columns = new[]
                    {
                        new { Name = "_URI", Type = "System.String" },
                        new { Name = "_DisplayText", Type = "System.String" },
                        new { Name = "Family", Type = "System.String" },
                        new { Name = "Order", Type = "System.String" },
                        new { Name = "Hierarchy", Type = "System.String" },
                    };
                    foreach (var column in columns)
                    {
                        dtQuery.Columns.Add(new System.Data.DataColumn(column.Name, Type.GetType(column.Type)));
                    }

                    var row = dtQuery.NewRow();
                    row["_URI"] = taxonomicEntity._URL;
                    row["_DisplayText"] = taxonomicEntity._DisplayText;
                    row["Family"] = taxonomicEntity.Family;
                    row["Order"] = taxonomicEntity.Order;
                    row["Hierarchy"] = taxonomicEntity.Hierarchy;
                    dtQuery.Rows.Add(row);
                }
                else
                {
                    // TODO implement other service mappings
                }
            }
            catch (Exception ex)
            {
#if DEBUG

                Console.WriteLine(ex.StackTrace);
#endif 
            }
        }

        private System.Collections.Generic.List<string> GetCurrentDiversityWorkbenchModuleSources()
        {
            System.Collections.Generic.List<string> Sources = new List<string>();
            string SQL = "SELECT distinct reverse(SUBSTRING(reverse([" + this._ColumnName + "]), CHARINDEX('/', reverse([" + this._ColumnName + "])) , 255)) " +
                "FROM [" + this._Table.TableName + "] " +
                "where Not [" + this._ColumnName + "] is null " +
                "and [" + this._ColumnName + "] like 'http://%' ";
            System.Data.DataTable dt = new System.Data.DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            foreach (System.Data.DataRow R in dt.Rows)
            {
                Sources.Add(R[0].ToString());
            }
            return Sources;
        }

        #endregion

        #region Relations

        private System.Collections.Generic.Dictionary<string, string> _ForeignRelations;
        /// <summary>
        /// The foreign relations for the column as defined in the database: Key = ForeignTable, Value = ForeignColumn
        /// </summary>
        public System.Collections.Generic.Dictionary<string, string> ForeignRelations
        {
            get
            {
                if (this._ForeignRelations == null)
                {
                    this._ForeignRelations = new Dictionary<string, string>();
                    //System.Data.DataTable dt = new System.Data.DataTable();
                    //string SQL = "SELECT COLUMN_NAME " +
                    //    "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                    //    "WHERE (TABLE_NAME = '" + this._ForeignRelationTable + "') " +
                    //    "ORDER BY ORDINAL_POSITION";
                    //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    //ad.Fill(dt);
                }
                return this._ForeignRelations;
            }
        }

        private string _ForeignRelationTable;
        /// <summary>
        /// The name of the parent table of a foreign relation as defined in the database and choosed by the user
        /// </summary>
        public string ForeignRelationTable
        {
            get
            {
                if (this._ForeignRelationTable == null)
                {
                    if (this.ForeignRelations.Count > 0)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ForeignRelations)
                            this._ForeignRelationTable = KV.Key;
                    }
                }
                return _ForeignRelationTable;
            }
            set { _ForeignRelationTable = value; }
        }

        private string _ForeignRelationColumn;
        /// <summary>
        /// The name of the column in the foreign relation table as defined in the database and choosed by the user
        /// </summary>
        public string ForeignRelationColumn
        {
            get
            {
                if (this._ForeignRelationColumn == null)
                {
                    if (this.ForeignRelations.Count > 0)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ForeignRelations)
                            this._ForeignRelationColumn = KV.Value;
                    }
                    else
                    {
                        this._ForeignRelationColumn = "";
                    }
                }
                return _ForeignRelationColumn;
            }
            set { _ForeignRelationColumn = value; }
        }

        private string _ParentColumn;

        public string ParentColumn
        {
            get { return _ParentColumn; }
            set { _ParentColumn = value; }
        }

        private System.Data.DataTable _DtForeignRelationTableColumns;
        public System.Data.DataTable getForeignRelationTableColumns()
        {
            if (this._DtForeignRelationTableColumns == null)
            {
                this._DtForeignRelationTableColumns = new System.Data.DataTable();
                string SQL = "SELECT COLUMN_NAME " +
                    "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                    "WHERE (TABLE_NAME = '" + this.ForeignRelationTable + "') " +
                    "ORDER BY ORDINAL_POSITION";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(this._DtForeignRelationTableColumns);
            }
            return this._DtForeignRelationTableColumns;
        }

        #endregion

        #region Unit values

        private System.Collections.Generic.List<Export.TableColumnUnitValue> _UnitValues;

        public System.Collections.Generic.List<Export.TableColumnUnitValue> UnitValues
        {
            get
            {
                if (_UnitValues == null)
                    _UnitValues = new List<TableColumnUnitValue>();
                return _UnitValues;
            }
            //set { _UnitValues = value; }
        }

        #endregion

        #region Filter

        private string _Filter;

        public string Filter
        {
            get
            {
                if (_Filter == null)
                    _Filter = "";
                return _Filter;
            }
            set
            {
                _Filter = value;
            }
        }

        private string _RowFilter;

        public string RowFilter
        {
            get
            {
                if (_RowFilter == null)
                    _RowFilter = "";
                return _RowFilter;
            }
            set { _RowFilter = value; }
        }

        #endregion

        //private bool _IsSelected;

        //public bool IsSelected
        //{
        //    get { return _IsSelected; }
        //    set { _IsSelected = value; }
        //}

        #endregion

        #region Informations derived from database

        public string SqlForDataColumn(string TableAlias)
        {
            string SQL = this.ColumnName;
            bool IsAddedColumn = false;
            if (this.Table.AddedColumns() != null && this.Table.AddedColumns().ContainsKey(this.DisplayText))
                IsAddedColumn = true;
            if (TableAlias.Length > 0 && !IsAddedColumn)
                SQL = TableAlias + ".[" + SQL + "]";
            else if (IsAddedColumn && this._SQL != null && this._SQL.Length > 0)
            {
                SQL = this._SQL;
            }
            switch (this.DataType)
            {
                case "datetime":
                    SQL = "convert(varchar(20)," + SQL + ", 120) AS " + this.ColumnName;
                    break;
                case "datetime2":
                    SQL = "convert(varchar(30)," + SQL + ", 121) AS " + this.ColumnName;
                    break;
                case "geometry":
                case "geography":
                    SQL += ".ToString() AS " + this.ColumnName;
                    break;
            }
            return SQL;
        }

        public bool AllowOrderBy
        {
            get
            {
                if (this.DataType == "geography") return false;
                return true;
            }
        }

        private bool? _IsIdentity;
        /// <summary>
        /// If the column is an identity column where the value will be automatically generated by the database
        /// </summary>
        public bool IsIdentity
        {
            get
            {
                if (this._IsIdentity == null)
                {
                    if (this._DataType != "int")
                        this._IsIdentity = false;
                    else
                    {
                        try
                        {
                            string SQL = "select case when min(c.name) is null then '' else min(c.name) end from sys.columns c, sys.tables t where c.is_identity = 1 " +
                            "and c.object_id = t.object_id and t.name = '" + this._Table.TableName + "'";
                            string IdentityColumn = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                            if (IdentityColumn.Length > 0 && IdentityColumn == this._ColumnName)
                                this.IsIdentity = true;
                            else
                                this._IsIdentity = false;
                        }
                        catch (System.Exception ex)
                        {
                            this._IsIdentity = false;
                        }
                    }
                }
                return (bool)_IsIdentity;
            }
            set { _IsIdentity = value; }
        }

        private System.Data.DataTable _DtSource;
        public System.Data.DataTable DtSource()
        {
            if (this.ForeignRelations.Count == 1)
            {
                if (this._DtSource == null)
                {
                    this._DtSource = new System.Data.DataTable();
                    string SQL = "SELECT DISTINCT [" + this.ForeignRelationColumn + "] FROM [" + this.ForeignRelationTable + "] ORDER BY [" + this.ForeignRelationColumn + "]";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(this._DtSource);
                }
                return this._DtSource;
            }
            else return null;
        }

        #endregion

        #region Construction

        public TableColumn(string DisplayText, string ColumnName, DiversityWorkbench.Export.Table Table, string SQL = "")
        {
            if (SQL.Length > 0)
            {
                this._SQL = SQL;
            }
            this._ColumnName = ColumnName;
            if (DisplayText.Length > 0)
                this._DisplayText = DisplayText;
            else this._DisplayText = ColumnName;
            this._Table = Table;
        }

        #endregion

    }
}
