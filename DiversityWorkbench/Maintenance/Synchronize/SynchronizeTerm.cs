using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityWorkbench.Maintenance.Synchronize
{
    public class SynchronizeTerm
    {
        #region Parameter

        protected System.Windows.Forms.DataGridView _DataGridView;
        protected System.Windows.Forms.ProgressBar _ProgressBar;
        protected System.Collections.Generic.Dictionary<string, System.Data.DataTable> _SynColTaxTextNameLists;
        protected DiversityWorkbench.ServerConnection _ServerConnection;
        //protected System.Windows.Forms.Label _RestrictionTableName;
        protected System.Windows.Forms.ComboBox _ComboboxRestrictionTable;
        protected System.Drawing.Image _RestrictionImage;
        protected string _RestrictionText;
        protected iMaintenanceCheckSingle _FormCheckSingle;

        protected string _MainTableName;
        protected string _MainTablePKcolumnName;
        protected string _MainTableDisplayColumnName;

        protected string _RestrictionTableName;

        protected string _ProjectTableName;

        protected string _TargetTableName;
        protected string _TargetTablePKColumnName;
        protected string _TargetDisplayColumnName;
        protected string _TargetLinkColumnName;
        protected string _TargetHierarchyColumnName;

        private string _SqlTableRestriction;
        protected string _GroupsTableName;
        protected string _GroupsPKcolumnName;

        #endregion

        #region Construction

        public SynchronizeTerm()
        {
        }

        //public SynchronizeTerm(TargetTable Table, bool Linked)
        //{
        //    this._TargetTable = Table;
        //    this._Linked = Linked;
        //}


        public SynchronizeTerm(//TargetTable Table,
            bool Linked,
            //System.Windows.Forms.DataGridView dataGridView,
            //System.Data.DataTable dataTable,
            //System.Windows.Forms.Label ResultLabel,
            //System.Windows.Forms.Button UpdateButton,
            System.Drawing.Image RestrictionImage,
            string RestrictionText,
            string RestrictionTable,
            iMaintenanceCheckSingle FormCheckSingle,
            string MainTablePKcolumnName,
            string MainTableName,
            string MainTableDisplayColumnName,
            string TargetTableName,
            string TargetDisplayColumnName,
            string TargetTablePKColumnName,
            string TargetHierarchyColumnName,
            string TargetLinkColumnName,
            string ProjectTableName,
            string SqlTableRestriction = "",
            string GroupsTable = "",
            string GroupsPK = "",
            System.Windows.Forms.ProgressBar progressBar = null, 
            System.Collections.Generic.Dictionary<string, System.Data.DataTable> SynColTaxTextNameLists = null
            )
        {
            //this._TargetTable = Table;
            this._Linked = Linked;
            //this._DataGridView = dataGridView;
            //this._DataTable = dataTable;
            this._RestrictionTableName = RestrictionTable;
            this._RestrictionImage = RestrictionImage;
            this._RestrictionText = RestrictionText;
            this._FormCheckSingle = FormCheckSingle;
            this._MainTablePKcolumnName = MainTablePKcolumnName;
            this._MainTableDisplayColumnName = MainTableDisplayColumnName;
            this._MainTableName = MainTableName;
            this._TargetTableName = TargetTableName;
            this._TargetDisplayColumnName = TargetDisplayColumnName;
            this._TargetTablePKColumnName = TargetTablePKColumnName;
            this._TargetHierarchyColumnName = TargetHierarchyColumnName;
            this._TargetLinkColumnName = TargetLinkColumnName;
            this._ProjectTableName = ProjectTableName;
            this._SqlTableRestriction = SqlTableRestriction;
            this._GroupsPKcolumnName = GroupsPK;
            this._GroupsTableName = GroupsTable;

            if (progressBar != null) _ProgressBar = progressBar;
            this._SynColTaxTextNameLists = SynColTaxTextNameLists;
        }

        #endregion

        #region Interface

        #region Settings

        public string MainTableName { get { return _MainTableName; } }
        public string MainTablePKcolumnName { get { return _MainTablePKcolumnName; } }
        public string MainTableDisplayColumnName { get { return _MainTableDisplayColumnName; } }

        public string RestrictionTableName { get { return _RestrictionTableName; } }

        public string ProjectTableName { get { return _ProjectTableName; } }

        public string TargetTableName { get { return _TargetTableName; } }
        public string TargetTablePKColumnName { get { return _TargetTablePKColumnName; } }
        public string TargetDisplayColumnName { get { return _TargetDisplayColumnName; } }
        public string TargetLinkColumnName { get { return _TargetLinkColumnName; } }
        public string TargetHierarchyColumnName { get { return _TargetHierarchyColumnName; } }

        #endregion

        #region UserControl

        public void setUserControl(DiversityWorkbench.Maintenance.Synchronize.UserControlSynchronizeTerm userControlSynchronizeTerm)
        {
            this._DataGridView = userControlSynchronizeTerm.DataGridView;
            this._ProgressBar = userControlSynchronizeTerm.ProgressBar;
        }

        public System.Drawing.Image RestrictionImage { get { return _RestrictionImage; } }
        public string RestrictionText { get { return _RestrictionText; } }

        public iMaintenanceCheckSingle FormCheckSingle { get { return _FormCheckSingle; } }

        #endregion

        #region Target

        //public enum TargetTable { CollectionEventProperty, CollectionSpecimenPartDescription, Identification }

        //protected TargetTable _TargetTable;
        protected bool _Linked;

        //public TargetTable Target { get { return _TargetTable; } }


        #endregion

        #region Linked

        public bool Linked { get => _Linked; }

        #endregion

        #region Table restriction

        //protected string RestrictionTableName
        //{
        //    get
        //    {
        //        return _RestrictionTableName;
        //        //switch (Target)
        //        //{
        //        //    case SynchronizeTerm.TargetTable.CollectionEventProperty:
        //        //        return "Property";
        //        //    case SynchronizeTerm.TargetTable.CollectionSpecimenPartDescription:
        //        //        return "CollMaterialCategory_Enum";
        //        //    case SynchronizeTerm.TargetTable.Identification:
        //        //        return "CollTaxonomicGroup_Enum";
        //        //}
        //        //return "";
        //    }
        //}

        public System.Data.DataTable TableRestriction()
        {
            string SQL = "SELECT " + _TargetTablePKColumnName;
            //switch (Target)
            //{
            //    case SynchronizeTerm.TargetTable.CollectionEventProperty:
            //        SQL += "PropertyID";
            //        break;
            //    case SynchronizeTerm.TargetTable.Identification:
            //    case SynchronizeTerm.TargetTable.CollectionSpecimenPartDescription:
            //        SQL += "Code";
            //        break;
            //}
            SQL += " AS Value, DisplayText AS Display FROM " + RestrictionTableName;
            //if (Target == TargetTable.Identification)
            //{
            //    string Restriction = "";
            //    foreach (string C in DiversityWorkbench.CollectionSpecimen.TermRelatedTaxonomicGroups)
            //    {
            //        if (Restriction.Length > 0)
            //            Restriction += ", ";
            //        Restriction += "'" + C + "'";
            //    }
            //    SQL += " WHERE Code IN (" + Restriction + ") ";
            //}
            if (this._Linked)// || _TargetTable == TargetTable.CollectionSpecimenPartDescription)
            {
                SQL += " UNION SELECT NULL AS Value, NULL AS Display";
            }
            SQL += " ORDER BY Display";
            System.Data.DataTable dt = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            return dt;
        }

        public void setRestrictionTable(System.Windows.Forms.ComboBox comboBox, System.Windows.Forms.Label label)
        {
            string SQL = "SELECT " + _TargetTablePKColumnName;
            //switch (Target)
            //{
            //    case SynchronizeTerm.TargetTable.CollectionEventProperty:
            //        SQL += "PropertyID";
            //        break;
            //    case SynchronizeTerm.TargetTable.Identification:
            //    case SynchronizeTerm.TargetTable.CollectionSpecimenPartDescription:
            //        SQL += "Code";
            //        break;
            //}
            SQL += " AS Value, DisplayText AS Display FROM " + RestrictionTableName + " ORDER BY Display";
            System.Data.DataTable dt = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            comboBox.DataSource = dt;
            comboBox.DisplayMember = "Display";
            comboBox.ValueMember = "Value";

        }

        public void setTableRestriction(string Restriction)
        {
            _TableRestriction = Restriction;
        }

        private string _TableRestriction;

        #endregion

        #region DataGrid

        public void setDataGridView(System.Windows.Forms.DataGridView dataGridView) { this._DataGridView = dataGridView; }

        #endregion

        #region DST database

        public void setServerConnection(DiversityWorkbench.ServerConnection serverConnection)
        {
            _ServerConnection = serverConnection;
        }

        public DiversityWorkbench.ServerConnection ServerConnection
        {
            get { return _ServerConnection; }
        }

        public void setDatabase(string Database)
        {
            if (Database.Length > 0)
            {
                if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList().ContainsKey("DiversityScientificTerms"))
                {
                    if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityScientificTerms"].ServerConnectionList().ContainsKey(Database))
                    {
                        _ServerConnection = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityScientificTerms"].ServerConnectionList()[Database];
                    }
                }
            }
        }

        public string DatabaseNameDST()
        {
            return _ServerConnection.DatabaseName;
        }


        public string ServerConnection_DisplayText()
        {
            return "      " + _ServerConnection.DatabaseName;
        }


        public void setDatabase(System.Windows.Forms.Label label, System.Windows.Forms.ComboBox comboBoxDatabase, System.Windows.Forms.ComboBox comboBoxTerminology)
        {
            label.Text = "      Database:";
            this.setServerConnection(DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityScientificTerms"].ServerConnectionList()[comboBoxDatabase.SelectedItem.ToString()]);
            if (this.ServerConnection.LinkedServer.Length > 0)
                label.Text = "      " + this.ServerConnection.DatabaseName;
            comboBoxTerminology.DataSource = this.getTerminologies();
            comboBoxTerminology.DisplayMember = "DisplayText";
            comboBoxTerminology.ValueMember = "TerminologyID";

        }
        #endregion

        #region Hierarchy

        public enum Hierarchy { None, TopDetail, DetailTop, SelectionMissing }

        protected Hierarchy _Hierarchy;// = Hierarchy.None;

        public void setHierarchy(Hierarchy hierarchy) { this._Hierarchy = hierarchy; }

        #endregion

        #region Project

        protected int? _ProjectID;

        public void setProjectID(int ProjectID) { this._ProjectID = ProjectID; }

        public System.Data.DataTable ProjectList(bool IncludeNull = false)
        {
            string SQL = "";
            if (IncludeNull)
                SQL = "SELECT NULL AS [ProjectID], NULL AS [Project] UNION ";
            SQL += "SELECT [ProjectID], [Project] " +
                "FROM [" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[ProjectList] ORDER BY Project";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            System.Data.DataTable dt = new System.Data.DataTable();
            ad.Fill(dt);
            return dt;
        }

        #endregion

        #region Term restriction

        protected string _TermRestriction = "";

        public void setTermRestriction(string Term)
        {
            _TermRestriction = Term;
            if (!_TermRestriction.EndsWith("%"))
                _TermRestriction += "%";
        }

        #endregion

        #region Max results

        protected int? _MaxResults;
        public void setMaxResults(int? Max) { this._MaxResults = Max; }

        #endregion


        #region Include main table

        protected bool _IncludeMainTable = false;

        public void IncludeMainTable(bool include) { this._IncludeMainTable = include; }

        #endregion

        public void ShowDetails(int ID, int? width, int? height)
        {
            iMaintenanceCheckSingle checkSingle = this._FormCheckSingle;
            this._FormCheckSingle.ShowInMainForm(ID, width, height);
        }

        public void ShowDetails(int Width, int Height, System.Drawing.Point Location)
        {
            if (this._DataGridView.SelectedCells.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a dataset");
                return;
            }
            int ID = 0;
            int Position = this._DataGridView.SelectedCells[0].RowIndex;
            if (int.TryParse(this._DataGridView.Rows[Position].Cells[this._MainTablePKcolumnName].Value.ToString(), out ID))
            {
                iMaintenanceCheckSingle checkSingle = this._FormCheckSingle;
                this._FormCheckSingle.ShowInMainForm(ID, null, null);
                //DiversityCollection.FormCollectionSpecimen f = new FormCollectionSpecimen(CollectionSpecimenID, false, false);
                //f.setViewMode(FormCollectionSpecimen.ViewMode.SingleInspectionMode);
                //f.Location = Location;
                //f.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                //f.Width = Width - 10;
                //f.Height = Height - 10;
                //f.ShowDialog();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Dataset in database can only be checked if you include the accession number before checking for differences");
            }
        }

        protected bool ValuesSet()
        {
            if (this._ServerConnection == null || this._ServerConnection.ConnectionString.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a source database containing the terms");
                return false;
            }
            if (this._ProjectID == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a project");
                return false;
            }
            if (this._TerminologyID == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a terminology");
                return false;
            }
            if (!Linked && (this._Language == null || this._Language.Length == 0))
            {
                System.Windows.Forms.MessageBox.Show("Please select a language");
                return false;
            }
            if (_Hierarchy == Hierarchy.SelectionMissing)
            {
                System.Windows.Forms.MessageBox.Show("Please select the type of the hierarchy");
                return false;
            }
            return true;
        }

        #region SQL, Database


        protected System.Data.DataTable _DataTable;

        public int StartSearch()
        {
            try
            {
                this._DataGridView.DataSource = null;
                this._DataTable = new System.Data.DataTable();
                if (!this.ValuesSet()) return 0;

                try
                {
                    //SynTermTargetTable Table = SynTermTextTargetSelectedTable;
                    string SQL = "SELECT ";
                    // Max
                    if (_MaxResults != null && _MaxResults > 0)
                    {
                        SQL += "TOP " + _MaxResults.ToString();
                    }

                    // OK columns [0]
                    SQL += " CASE WHEN COUNT(DISTINCT R.RepresentationID) = 1 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS OK, ";

                    // Column in DT for the term [1]
                    SQL += "T." + TargetTermColumn;

                    SQL += " AS [Term in " + _TargetTableName + "] ";

                    // Column in DST for the term [2]
                    SQL += ",  MIN(R.DisplayText) AS [Term in DiversityScientificTerms], ";

                    // Hierarchy column in DC [3]
                    if (_Hierarchy != Hierarchy.None)
                    {
                        SQL += " T." + TargetHierarchyColumn + " ";
                        SQL += " AS [Hierarchy in " + _TargetTableName + "], ";
                    }

                    // Hierarchy column in DST [4]
                    switch (_Hierarchy)
                    {
                        case Hierarchy.DetailTop:
                            SQL += "MIN(R.HierarchyCache) AS [Hierarchy in DiversityScientificTerms], ";
                            break;
                        case Hierarchy.TopDetail:
                            SQL += "MIN(R.HierarchyCacheDown) AS [Hierarchy in DiversityScientificTerms], ";
                            break;
                    }

                    // Rest of the columns [3 - ...] or [5 - ...] max 9
                    SQL += "COUNT(DISTINCT R.RepresentationID) AS Count, MIN(B.[BaseURL]) + CAST(MIN(R.RepresentationID) AS varchar) AS URL ";
                    if (_IncludeMainTable)
                        SQL += ", S." + this._MainTableDisplayColumnName + " AS [" + this._TargetDisplayColumnName + "], S." + this._MainTablePKcolumnName + " ";
                    SQL += " " + this.FromClause();

                    this._DataTable = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                    ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                    try
                    {
                        ad.Fill(this._DataTable);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                    this._DataGridView.DataSource = this._DataTable;
                    for (int i = 1; i < this._DataGridView.Columns.Count; i++)
                    {
                        this._DataGridView.Columns[i].ReadOnly = true;
                    }

                    System.Collections.Generic.List<int> Positions = new List<int>();
                    Positions.Add(2);
                    if (_Hierarchy == Hierarchy.None)
                        for (int i = 3; i < 5; i++) Positions.Add(i);
                    else
                        for (int i = 4; i < 7; i++) Positions.Add(i);
                    this.SynTermSetCellStyle(this._DataGridView, Positions);

                    this.setColumnWidth();

                    this.setColumnVisibilty();

                    this._ProgressBar.Visible = false;
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }



            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return this._DataTable.Rows.Count;
        }

        protected void SynTermSetCellStyle(System.Windows.Forms.DataGridView D, int From, int? To = null)
        {
            try
            {
                if (D.Columns.Count > From)
                {
                    System.Windows.Forms.DataGridViewCellStyle S = new System.Windows.Forms.DataGridViewCellStyle(D.DefaultCellStyle);
                    S.ForeColor = System.Drawing.Color.Blue;
                    int Max = D.ColumnCount;
                    if (To != null)
                        Max = (int)To;
                    for (int i = From; i < Max; i++)
                    {
                        if (D.ColumnCount > i)
                        {
                            D.Columns[i].DefaultCellStyle = S;
                            D.Columns[i].HeaderCell.Style.ForeColor = System.Drawing.Color.Blue;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        protected void SynTermSetCellStyle(System.Windows.Forms.DataGridView D, System.Collections.Generic.List<int> ColumnPositions)
        {
            try
            {
                foreach (int i in ColumnPositions)
                {
                    if (D.Columns.Count > i)
                    {
                        System.Windows.Forms.DataGridViewCellStyle S = new System.Windows.Forms.DataGridViewCellStyle(D.DefaultCellStyle);
                        S.ForeColor = System.Drawing.Color.Blue;
                        D.Columns[i].DefaultCellStyle = S;
                        D.Columns[i].HeaderCell.Style.ForeColor = System.Drawing.Color.Blue;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setColumnVisibilty()
        {
            System.Collections.Generic.List<int> Hide = new List<int>();
            // Count
            if (_Hierarchy == Hierarchy.None) Hide.Add(3);
            else Hide.Add(5);

            // CollectionspecimenID
            if (_IncludeMainTable)
            {
                if (_Hierarchy == Hierarchy.None) Hide.Add(6);
                else Hide.Add(8);
            }
            foreach (int i in Hide)
            {
                if (_DataGridView.Columns.Count > i)
                    _DataGridView.Columns[i].Visible = false;
            }
        }

        private void setColumnWidth()
        {
            for (int i = 0; i < this._DataTable.Columns.Count; i++)
            {
                if (this._DataTable.Columns[i].ColumnName == "URL")
                    this._DataGridView.Columns[i].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
                else this._DataGridView.Columns[i].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            }
        }


        protected string FromClause(System.Data.DataRow Row = null, bool ForUpdate = false)
        {
            // Tests
            string SQL = " FROM ";
            try
            {
                SQL += _TargetTableName + " AS T INNER JOIN " + this._MainTableName + " S ON T." + TargetIdentityColumn + " = S." + TargetIdentityColumn + " ";
                if (Linked)
                {
                    SQL += " AND (T." + TargetLinkColumn + " <> '') ";
                }
                else
                {
                    SQL += " AND (T." + TargetLinkColumn + " IS NULL OR T." + TargetLinkColumn + " = '') ";
                }
                if (Row != null && Row.Table.Columns.Contains(_MainTablePKcolumnName))
                {
                    SQL += " AND S." + _MainTablePKcolumnName + " = " + Row[_MainTablePKcolumnName].ToString();
                }

                // getting the project
                if (this._ProjectID != null)
                    SQL += " INNER JOIN " + _ProjectTableName + " P ON S." + _MainTablePKcolumnName + " = P." + _MainTablePKcolumnName + " AND P.ProjectID = " + _ProjectID.ToString();

                // BaseURL
                SQL += " CROSS JOIN " + _ServerConnection.Prefix() + "ViewBaseURL AS B ";

                // Table Restriction
                if (_Linked)
                {
                    SQL += " INNER JOIN " + _ServerConnection.Prefix() + "TermRepresentation R ON T." + TargetLinkColumn;
                    SQL += " = B.BaseURL + CAST(R.RepresentationID AS varchar) ";
                }
                else
                {
                    SQL += " INNER JOIN " + _ServerConnection.Prefix() + "TermRepresentation R ON T." + TargetTermColumn;
                    SQL += " collate database_default = R.DisplayText collate database_default ";
                }

                // TermRestriction
                if (this._TermRestriction != null && this._TermRestriction.Length > 0)
                {
                    SQL += " AND T." + _TargetDisplayColumnName;
                    //switch (_TargetTable)
                    //{
                    //    case TargetTable.CollectionEventProperty:
                    //        SQL += "DisplayText";
                    //        break;
                    //    case TargetTable.CollectionSpecimenPartDescription:
                    //        SQL += "Description";
                    //        break;
                    //    case TargetTable.Identification:
                    //        SQL += "VernacularTerm";
                    //        break;
                    //}
                    SQL += " LIKE '" + _TermRestriction + "' ";
                }

                // Languange
                if (this._Language != null && this._Language.Length > 0)
                    SQL += "AND R.LanguageCode = '" + this._Language + "' ";

                // Terminology
                SQL += " AND R.TerminologyID = " + this._TerminologyID.ToString();

                // No Ranking terms
                SQL += " INNER JOIN " + _ServerConnection.Prefix() + "Term DST ON R.TermID = DST.TermID AND DST.IsRankingTerm = 0 ";

                // Table restriction
                if (this._SqlTableRestriction.Length > 0 && this._TableRestriction.Length > 0)
                    SQL += this._SqlTableRestriction + this._TableRestriction + "'";
                //switch (_TargetTable)
                //{
                //    case TargetTable.CollectionSpecimenPartDescription:
                //        SQL += " INNER JOIN CollectionSpecimenPart CSP ON CSP.CollectionSpecimenID = T.CollectionSpecimenID AND CSP.SpecimenPartID = T.SpecimenPartID AND CSP.MaterialCategory = '" + this._TableRestriction + "'";
                //        break;
                //    case TargetTable.Identification:
                //        SQL += " INNER JOIN IdentificationUnit U ON U.CollectionSpecimenID = T.CollectionSpecimenID AND U.IdentificationUnitID = T.IdentificationUnitID AND U.TaxonomicGroup = '" + this._TableRestriction + "'";
                //        break;
                //}
                SQL += WhereClause(ForUpdate);
                SQL += GroupByClause(ForUpdate);

                return SQL;
            }

            catch (System.Exception ex)
            {
                SQL = "";
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return SQL;
        }

        protected string WhereClause(bool ForUpdate = false)
        {
            // Tests
            string SQL = "";
            try
            {
                if (_Linked)// || _TargetTable == TargetTable.CollectionEventProperty)
                    SQL += " WHERE ";
                //if (_TargetTable == TargetTable.CollectionEventProperty)
                //{
                //    SQL += " T.PropertyID = " + this._TableRestriction.ToString() + " ";
                //    if (_Linked) SQL += " AND ";
                //}
                if (_Linked)
                {
                    SQL += " (R.DisplayText <> T." + this._TargetDisplayColumnName;
                    //switch (_TargetTable)
                    //{
                    //    case TargetTable.CollectionEventProperty:
                    //        SQL += "DisplayText";
                    //        break;
                    //    case TargetTable.CollectionSpecimenPartDescription:
                    //        SQL += "Description";
                    //        break;
                    //    case TargetTable.Identification:
                    //        SQL += "VernacularTerm";
                    //        break;
                    //}
                    if (_Hierarchy != Hierarchy.None)
                    {
                        SQL += " OR R.";
                        switch (_Hierarchy)
                        {
                            case Hierarchy.DetailTop:
                                SQL += "HierarchyCache";
                                break;
                            case Hierarchy.TopDetail:
                                SQL += "HierarchyCacheDown";
                                break;
                        }
                        SQL += " <> T." + _TargetHierarchyColumnName;
                        SQL += " OR (R.";
                        switch (_Hierarchy)
                        {
                            case Hierarchy.DetailTop:
                                SQL += "HierarchyCache";
                                break;
                            case Hierarchy.TopDetail:
                                SQL += "HierarchyCacheDown";
                                break;
                        }
                        SQL += " <> '' AND T." + _TargetHierarchyColumnName + " IS NULL)";
                        //switch (_TargetTable)
                        //{
                        //    case TargetTable.CollectionEventProperty:
                        //        SQL += " T.PropertyHierarchyCache ";
                        //        break;
                        //    case TargetTable.CollectionSpecimenPartDescription:
                        //        SQL += " T.DescriptionHierarchyCache ";
                        //        break;
                        //    case TargetTable.Identification:
                        //        SQL += " U.HierarchyCache ";
                        //        break;
                        //}
                    }
                    SQL += ") ";
                }

                return SQL;
            }

            catch (System.Exception ex)
            {
                SQL = "";
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return SQL;
        }

        protected string GroupByClause(bool ForUpdate = false)
        {
            // Tests
            string SQL = "";
            try
            {
                if (!ForUpdate)
                {
                    SQL += " GROUP BY T." + _TargetDisplayColumnName;
                    //switch (_TargetTable)
                    //{
                    //    case TargetTable.CollectionEventProperty:
                    //        SQL += "DisplayText";
                    //        break;
                    //    case TargetTable.CollectionSpecimenPartDescription:
                    //        SQL += "Description";
                    //        break;
                    //    case TargetTable.Identification:
                    //        SQL += "VernacularTerm";
                    //        break;
                    //}
                    if (_Hierarchy != Hierarchy.None)
                    {
                        SQL += ", T." + _TargetHierarchyColumnName;
                        //switch (_TargetTable)
                        //{
                        //    case TargetTable.CollectionEventProperty:
                        //        SQL += ", T.PropertyHierarchyCache ";
                        //        break;
                        //    case TargetTable.CollectionSpecimenPartDescription:
                        //        SQL += ", T.DescriptionHierarchyCache ";
                        //        break;
                        //    case TargetTable.Identification:
                        //        SQL += ", U.HierarchyCache ";
                        //        break;
                        //}
                    }
                    if (this._IncludeMainTable)
                        SQL += ", S." + _MainTableDisplayColumnName + ", S." + _MainTablePKcolumnName + " ";
                }

                return SQL;
            }

            catch (System.Exception ex)
            {
                SQL = "";
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return SQL;
        }


        public string Update()
        {
            string Message = "";
            this._ProgressBar.Visible = true;
            this._ProgressBar.Maximum = this._DataTable.Rows.Count;
            foreach (System.Data.DataRow R in this._DataTable.Rows)
            {
                if (R[0].ToString() == "True")
                {
                    try
                    {
                        string Term = R[2].ToString();
                        string Link = R["URL"].ToString();
                        string Hierarchy = "";
                        string HierarchyColumn = "";
                        switch (_Hierarchy)
                        {
                            case SynchronizeTerm.Hierarchy.DetailTop:
                                HierarchyColumn = "HierarchyCache";
                                break;
                            case SynchronizeTerm.Hierarchy.TopDetail:
                                HierarchyColumn = "HierarchyCacheDown";
                                break;
                        }
                        if (_Hierarchy != SynchronizeTerm.Hierarchy.None && R.Table.Columns.Contains(HierarchyColumn))
                            Hierarchy = R[HierarchyColumn].ToString();
                        if (Hierarchy.Length == 0 && R.Table.Columns.Contains("Hierarchy in DiversityScientificTerms"))
                            Hierarchy = R["Hierarchy in DiversityScientificTerms"].ToString();

                        string SQL = "UPDATE T SET T." + TargetTermColumn + " = '" + Term + "' ";
                        // Link
                        SQL += ", T." + TargetLinkColumn + " = '" + Link + "' ";
                        if (_Hierarchy != SynchronizeTerm.Hierarchy.None)// && _TargetTable != TargetTable.Identification)
                        {
                            SQL += ", T." + TargetHierarchyColumn + " = '" + Hierarchy + "' ";
                        }
                        SQL += this.FromClause(R, true);
                        if (SQL.IndexOf(" WHERE ") == 0)
                            SQL += " WHERE T.";
                        else
                            SQL += " AND T." + TargetTermColumn;
                        SQL += " = '" + R[1].ToString() + "'";
                        //if (_TargetTable == TargetTable.Identification && _Hierarchy != SynchronizeTerm.Hierarchy.None)
                        //{
                        //    string SqlUnit = "UPDATE U SET U.HierarchyCache = '" + R["Hierarchy"].ToString() + "'";
                        //    SqlUnit += this.FromClause(R, true);
                        //    SqlUnit += " WHERE T.VernacularTerm = '" + R[1].ToString() + "'";
                        //    DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SqlUnit, ref Message);
                        //}
                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message);
                    }
                    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                }

                if (this._ProgressBar.Value < this._ProgressBar.Maximum) this._ProgressBar.Value++;
            }
            this._ProgressBar.Visible = false;
            this.StartSearch();
            return "";
        }

        private string TargetTermColumn
        {
            get
            {
                return _TargetDisplayColumnName;
                //switch (_TargetTable)
                //{
                //    case TargetTable.CollectionEventProperty:
                //        return "DisplayText";
                //    case TargetTable.CollectionSpecimenPartDescription:
                //        return "Description";
                //    case TargetTable.Identification:
                //        return "VernacularTerm";
                //}
                //return "";
            }
        }

        private string TargetLinkColumn
        {
            get
            {
                return _TargetLinkColumnName;
                //    switch (_TargetTable)
                //    {
                //        case TargetTable.CollectionEventProperty:
                //            return "PropertyURI";
                //        case TargetTable.CollectionSpecimenPartDescription:
                //            return "DescriptionTermURI";
                //        case TargetTable.Identification:
                //            return "TermURI";
                //    }
                //    return "";
            }
        }

        private string TargetHierarchyColumn
        {
            get
            {
                return _TargetHierarchyColumnName;
                //switch (_TargetTable)
                //{
                //    case TargetTable.CollectionEventProperty:
                //        return "PropertyHierarchyCache";
                //    case TargetTable.CollectionSpecimenPartDescription:
                //        return "DescriptionHierarchyCache";
                //    case TargetTable.Identification:
                //        return "HierarchyCache";
                //}
                //return "";
            }
        }

        private string TargetIdentityColumn
        {
            get
            {
                return _MainTablePKcolumnName;
                //switch (_TargetTable)
                //{
                //    case TargetTable.CollectionEventProperty:
                //        return "CollectionEventID";
                //    case TargetTable.CollectionSpecimenPartDescription:
                //    case TargetTable.Identification:
                //        return "CollectionSpecimenID";
                //}
                //return "";
            }
        }


        #endregion

        #region Selecting the rows
        public void SelectAllRows()
        {
            if (System.Windows.Forms.MessageBox.Show("Are you sure that you want to select all rows?", "Select all?", System.Windows.Forms.MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                return;
            foreach (System.Data.DataRow R in this._DataTable.Rows)
                R[0] = true;
        }

        public void DeselectAllRows()
        {
            if (System.Windows.Forms.MessageBox.Show("Are you sure that you want to deselect all rows?", "Deselect all?", System.Windows.Forms.MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                return;
            foreach (System.Data.DataRow R in this._DataTable.Rows)
                R[0] = false;
        }
        public void SelectValidRow(int RowIndex, int Height) //, string TaxonDatabase, bool IncludeHierarchy, System.Data.DataTable dataTable, System.Collections.Generic.Dictionary<string, System.Data.DataTable> SynColTaxTextNameLists)
        {
            int i = RowIndex;
            string Name = this._DataTable.Rows[i][1].ToString();
            if (_SynColTaxTextNameLists.ContainsKey(Name))
            {
                string Message = "Please select the valid name from the list:\r\n" + Name;
                DiversityWorkbench.Forms.FormSelectTableRow f = new DiversityWorkbench.Forms.FormSelectTableRow(_SynColTaxTextNameLists[Name], Message);//, 2, "1", System.Drawing.Color.LightGreen, System.Drawing.Color.LightPink);
                f.dataGridView.Columns[0].Visible = false;
                f.dataGridView.Columns[1].HeaderText = "Name in " + this._ServerConnection.DatabaseName; // TaxonDatabase;
                if (f.dataGridView.ColumnCount > 2)
                    f.dataGridView.Columns[2].Width = 70;
                f.dataGridView.AutoResizeColumns(System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells);
                f.Width = 570;
                f.dataGridView.Columns[1].Width = f.Width - 140;
                f.Height = Height - 10;
                f.ForeColor = System.Drawing.Color.DarkBlue;
                f.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    System.Data.DataRow Rs = f.SelectedRow;
                    System.Data.DataRow Rd = this._DataTable.Rows[i];
                    if (Rd[2].ToString() != Rs[1].ToString())
                    {
                        Rd[2] = Rs[1];
                        string URI = Rd[3].ToString().Substring(0, Rd[3].ToString().IndexOf("=") + 1) + Rs[0].ToString();
                        bool AdaptHierarchy = true;
                        if (Rd[3].ToString() == URI
                            || _Hierarchy == Hierarchy.None)
                            AdaptHierarchy = false;
                        Rd[3] = URI;
                    }
                    Rd[0] = true;
                }
            }

        }

        #endregion

        #region Terminology

        protected int? _TerminologyID;

        public System.Data.DataTable Terminologies()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                string SQL = "SELECT NULL AS ProjectID, NULL AS Project " +
                    "UNION SELECT TerminologyID AS ProjectID, DisplayText AS Project " +
                    "FROM " + this._ServerConnection.Prefix() + "Terminology " +
                    "ORDER BY Project";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _ServerConnection.ConnectionString);
                ad.Fill(dt);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return dt;
        }

        public System.Data.DataTable getTerminologies()
        {
            string Prefix = "dbo.";
            if (this._ServerConnection.LinkedServer != null && this._ServerConnection.LinkedServer.Length > 0)
                Prefix = "[" + this._ServerConnection.LinkedServer + "].[" + this._ServerConnection.DatabaseName + "].dbo.";
            string SQL = "SELECT TerminologyID, DisplayText FROM " + Prefix + "Terminology ORDER BY DisplayText";
            System.Data.DataTable dt = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, this._ServerConnection.ConnectionString);
            return dt;
        }



        public void setTerminologySource(System.Windows.Forms.ComboBox comboBox)
        {
            comboBox.DataSource = this.Terminologies();
            comboBox.DisplayMember = "Project";
            comboBox.ValueMember = "ProjectID";
        }

        public void setTerminology(int ID) { this._TerminologyID = ID; }

        #endregion

        #region Groups

        protected string _Group;

        public void SetGroup(string group) { _Group = group; }

        private System.Data.DataTable _DtGroups;

        public System.Data.DataTable Groups()
        {
            if (_DtGroups == null)
            {
                try
                {
                    _DtGroups = new System.Data.DataTable();
                    string SQL = "";
                    if (_GroupsTableName.Length > 0 && _GroupsPKcolumnName.Length > 0)
                    {
                        SQL =  "SELECT " + _GroupsPKcolumnName + " AS Value, DisplayText AS Display FROM " + _GroupsTableName;
                        //switch (this._TargetTable)
                        //{
                        //    case TargetTable.CollectionEventProperty:
                        //        SQL = "SELECT PropertyID AS Value, DisplayText AS Display FROM Property";
                        //        goto default;
                        //    case TargetTable.CollectionSpecimenPartDescription:
                        //        SQL = "SELECT Code AS Value, DisplayText AS Display FROM CollMaterialCategory_Enum";
                        //        goto default;
                        //    case TargetTable.Identification:
                        //        SQL = "SELECT Code AS Value, DisplayText AS Display FROM CollTaxonomicGroup_Enum";
                        //        goto default;
                        //    default:
                        //        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref _DtGroups);
                        //        break;
                        //}
                        if (this._Linked) // || _TargetTable == TargetTable.CollectionSpecimenPartDescription)
                        {
                            //SQL += " UNION SELECT NULL AS Value, NULL AS Display";
                            SQL += " UNION SELECT '' AS Value, '' AS Display";
                        }
                        SQL += " ORDER BY Display";
                    }
                    else
                        SQL = "SELECT '' AS Value, '' AS Display";
                        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref _DtGroups);
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            return _DtGroups;
        }

        public void setGroupsSource(System.Windows.Forms.ComboBox comboBox)
        {
            comboBox.DataSource = this.Groups();
            comboBox.DisplayMember = "Display";
            comboBox.ValueMember = "Value";
        }

        #endregion

        #region Language

        protected string _Language;

        public System.Data.DataTable DtLanguages()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                string SQL = "SELECT DISTINCT R.LanguageCode AS Value, L.DisplayText AS Display " +
                    "FROM " + this._ServerConnection.Prefix() + "TermRepresentation AS R " +
                    "INNER JOIN " + this._ServerConnection.Prefix() + "LanguageCode_Enum AS L ON R.LanguageCode = L.Code " +
                    "WHERE R.TerminologyID = " + _TerminologyID.ToString();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return dt;
        }

        public void setLanguage(string language) { this._Language = language; }

        public void setLanguageSource(System.Windows.Forms.ComboBox comboBox)
        {
            comboBox.DataSource = this.DtLanguages();
            comboBox.DisplayMember = "Display";
            comboBox.ValueMember = "Value";
        }

        #endregion

        #endregion

    }
}
