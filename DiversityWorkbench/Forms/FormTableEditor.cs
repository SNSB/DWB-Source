using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormTableEditor : Form
    {
        #region Auxilliary definition

        public struct ComboBoxValues
        {
            private DataTable _DataTable;
            public DataTable DataTable
            {
                get { return _DataTable; }
                //set { _DataTable = value; }
            }

            private string _DisplayMember;
            public string DisplayMember
            {
                get { return _DisplayMember; }
                //set { _DisplayMember = value; }
            }

            private string _ValueMember;
            public string ValueMember
            {
                get { return _ValueMember; }
                //set { _ValueMember = value; }
            }

            private string _ColumnTitle;
            public string ColumnTitle
            {
                get { return _ColumnTitle; }
                //set { _ColumnTitle = value; }
            }

            public ComboBoxValues(DataTable dataTable, string displayMember, string valueMember, string columnTitle = "")
            {
                _DataTable = dataTable;
                _DisplayMember = displayMember;
                _ValueMember = valueMember;
                _ColumnTitle = columnTitle;
            }
        }

        #endregion

        #region Parameter

        private Microsoft.Data.SqlClient.SqlDataAdapter _Adapter;
        private System.Data.DataTable _dt;
        private enum EditAction { Insert, Append, Replace, Clear, Transfer, Trim };
        private string _FilterColumn = "";
        private int _FilterColumnIndex = 0;
        private EditAction _EditingAction;
        private System.Collections.Generic.List<string> _ReadOnlyColumns;
        private System.Collections.Generic.List<string> _LookUpColumns;
        private string _TableName;
        private string _IDColumn;
        private string _TableAlias = "";
        private System.Collections.Generic.Dictionary<string, System.Data.DataTable> _LookupTables;
        private System.Collections.Generic.Dictionary<string, ComboBoxValues> _ComboBoxColumns;
        private System.Collections.Generic.Dictionary<string, ComboBoxValues> _UsedComboBoxColumns;
        private int? _TimeOut;
        private string _TableExportFile = "";
        private bool _DoSave = true;
        private System.Drawing.Image _Icon;
        private DiversityWorkbench.Data.Table _Table;
        private bool _UserComboboxValues = true;

        #endregion

        public static bool CanReadGeographyDataType = true;

        #region Properties

        public System.Collections.Generic.List<string> LookUpColumns
        {
            get
            {
                if (this._LookUpColumns == null)
                {
                    if (this._TableName != null)
                    {
                        this._LookUpColumns = new List<string>();
                        this._LookupTables = new Dictionary<string, DataTable>();
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
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        System.Data.DataTable dt = new DataTable();
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            try
                            {
                                if (_UsedComboBoxColumns != null && _UsedComboBoxColumns.ContainsKey(R[0].ToString()))
                                {
                                    this._LookUpColumns.Add(R[0].ToString());
                                    System.Data.DataTable dtLookup = new DataTable();
                                    dtLookup.Columns.Add(R[2].ToString());
                                    DataRow dr = dtLookup.NewRow();
                                    foreach (DataRow item in _UsedComboBoxColumns[R[0].ToString()].DataTable.Rows)
                                    {
                                        dr = dtLookup.NewRow();
                                        dr[R[2].ToString()] = item[_UsedComboBoxColumns[R[0].ToString()].ValueMember];
                                        dtLookup.Rows.Add(dr);
                                    }
                                    // Markus 20.8.24: Sorting the content
                                    System.Data.DataTable dtLookupSorted = new DataTable();
                                    dtLookupSorted.Columns.Add(R[2].ToString());
                                    foreach (System.Data.DataRow dataRow in dtLookup.Select("", R[2].ToString()))
                                    {
                                        System.Data.DataRow Rsorted = dtLookupSorted.NewRow();
                                        Rsorted[0] = dataRow[0];
                                        dtLookupSorted.Rows.Add(Rsorted);
                                    }
                                    this._LookupTables.Add(R[0].ToString(), dtLookupSorted);

                                    //this._LookupTables.Add(R[0].ToString(), dtLookup);
                                }
                                else if (R[1].ToString().EndsWith("_Enum"))
                                {
                                    this._LookUpColumns.Add(R[0].ToString());
                                    SQL = "SELECT NULL AS " + R[2].ToString() + " UNION SELECT " + R[2].ToString() + " FROM " + R[1].ToString() + " ORDER BY " + R[2].ToString();
                                    ad.SelectCommand.CommandText = SQL;
                                    System.Data.DataTable dtLookup = new DataTable();
                                    ad.Fill(dtLookup);
                                    this._LookupTables.Add(R[0].ToString(), dtLookup);
                                }
                            }
                            catch (System.Exception ex)
                            {
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            }
                        }
                        // Append combo box columns not based on relations
                        if (_UsedComboBoxColumns != null)
                        {
                            foreach (KeyValuePair<string, ComboBoxValues> column in _UsedComboBoxColumns)
                            {
                                if (!this._LookUpColumns.Contains(column.Key))
                                {
                                    this._LookUpColumns.Add(column.Key);
                                    System.Data.DataTable dtLookup = new DataTable();
                                    dtLookup.Columns.Add(column.Key);
                                    DataRow dr = dtLookup.NewRow();
                                    foreach (DataRow item in column.Value.DataTable.Rows)
                                    {
                                        dr = dtLookup.NewRow();
                                        dr[column.Key] = item[column.Value.ValueMember];
                                        dtLookup.Rows.Add(dr);
                                    }
                                    this._LookupTables.Add(column.Key, dtLookup);
                                }
                            }
                        }
                    }
                }
                return _LookUpColumns;
            }
            set { _LookUpColumns = value; }
        }

        public string FilterColumn
        {
            get { return _FilterColumn; }
            set
            {
                _FilterColumn = value;
                this.groupBoxFilter.Text = "Filter: " + _FilterColumn;
            }
        }

        private EditAction EditingAction
        {
            get { return _EditingAction; }
            set
            {
                _EditingAction = value;
                switch (this._EditingAction)
                {
                    case EditAction.Append:
                        this.buttonEdit.Image = this.imageListEditAction.Images[0];
                        break;
                    case EditAction.Insert:
                        this.buttonEdit.Image = this.imageListEditAction.Images[1];
                        break;
                    case EditAction.Replace:
                        this.buttonEdit.Image = this.imageListEditAction.Images[2];
                        break;
                    case EditAction.Clear:
                        this.buttonEdit.Image = this.imageListEditAction.Images[3];
                        break;
                    case EditAction.Transfer:
                        this.buttonEdit.Image = this.imageListEditAction.Images[4];
                        break;
                    case EditAction.Trim:
                        this.buttonEdit.Image = this.imageListEditAction.Images[5];
                        break;
                }
            }
        }

        private DiversityWorkbench.Data.Table Table()
        {
            if (this._Table == null)
            {
                this._Table = new Data.Table(this._TableName, DiversityWorkbench.Settings.ConnectionString);
            }
            return this._Table;
        }

        #endregion

        #region Construction

        public FormTableEditor(System.Drawing.Image Icon,
            string TableName,
            string IDColumn,
            bool trackUpdatedRows = false)
        {
            InitializeComponent();
            this._Icon = Icon;
            this._TableName = TableName;
            this._IDColumn = IDColumn;
        }

        public FormTableEditor(System.Drawing.Icon Icon,
            string TableName,
            string IDColumn)
        {
            InitializeComponent();
            this.Icon = Icon;
            this._TableName = TableName;
            this._IDColumn = IDColumn;
        }


        public FormTableEditor(System.Drawing.Image Icon,
            Microsoft.Data.SqlClient.SqlDataAdapter Adapter,
            System.Collections.Generic.List<string> ReadOnlyColumns,
            int Timeout,
            System.Collections.Generic.Dictionary<string, ComboBoxValues> ComboBoxColumns = null,
            string TableName = "")
        {
            InitializeComponent();
            this._TimeOut = Timeout;
            this._ComboBoxColumns = ComboBoxColumns;
            this._UsedComboBoxColumns = ComboBoxColumns;
            this.initForm(Icon, Adapter, ReadOnlyColumns, TableName);
        }

        public FormTableEditor(System.Drawing.Image Icon,
            Microsoft.Data.SqlClient.SqlDataAdapter Adapter,
            System.Collections.Generic.List<string> ReadOnlyColumns,
            System.Collections.Generic.Dictionary<string, ComboBoxValues> ComboBoxColumns = null)
        {
            InitializeComponent();
            this._ComboBoxColumns = ComboBoxColumns;
            this._UsedComboBoxColumns = ComboBoxColumns;
            this.initForm(Icon, Adapter, ReadOnlyColumns);
        }

        public FormTableEditor(System.Drawing.Image Icon,
            Microsoft.Data.SqlClient.SqlDataAdapter Adapter,
            string TableName,
            string TableAlias,
            System.Collections.Generic.List<string> ReadOnlyColumns,
            System.Collections.Generic.Dictionary<string, ComboBoxValues> ComboBoxColumns = null)
        {
            InitializeComponent();
            this._TableName = TableName;
            this._TableAlias = TableAlias;
            this._ComboBoxColumns = ComboBoxColumns;
            this._UsedComboBoxColumns = ComboBoxColumns;
            this.initForm(Icon, Adapter, ReadOnlyColumns);
        }

        #endregion

        #region Form and formatting

        private void initForm(System.Drawing.Image Icon,
            Microsoft.Data.SqlClient.SqlDataAdapter Adapter,
            System.Collections.Generic.List<string> ReadOnlyColumns,
            string TableName = "")
        {
            try
            {
                if (TableName.Length == 0)
                {
                    // Markus 16.6.2020: Zugriff auf _TableName statt beliebigen Tabellennamen. Grund fuer vorherige Loesung unklar
                    if (this._TableName == null || this._TableName.Length == 0)
                    {
                        // Markus 23.5.23: Anpassung an Modul
                        switch (DiversityWorkbench.Settings.ModuleName)
                        {
                            case "DiversityAgents":
                                TableName = "Agent";
                                break;
                            case "DiversityProjects":
                                TableName = "Project";
                                break;
                            case "DiversityTaxonNames":
                                TableName = "TaxonName";
                                break;
                            default:
                                TableName = "CollectionSpecimen";
                                break;
                        }
                    }
                    else
                        TableName = this._TableName;
                }
                else if ((this._TableName == null || this._TableName.Length == 0) && TableName.Length > 0)
                    this._TableName = TableName;
                DiversityWorkbench.Forms.FormFunctions FF = new FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                bool OK = FF.getObjectPermissions(TableName, "UPDATE");
                if (!OK)
                {
                    this.ReadOnly = false;
                }

                this._ReadOnlyColumns = ReadOnlyColumns;
                this.comboBoxAction.Visible = false;
                this.comboBoxAction.Items.Clear();
                this.comboBoxAction.Items.Add(EditAction.Insert.ToString());
                this.comboBoxAction.Items.Add(EditAction.Append.ToString());
                this.comboBoxAction.Items.Add(EditAction.Replace.ToString());
                this.comboBoxAction.Items.Add(EditAction.Clear.ToString());
                this.comboBoxAction.Items.Add(EditAction.Transfer.ToString());
                this.comboBoxAction.Items.Add(EditAction.Trim.ToString());
                this.comboBoxAction.SelectedIndex = 0;
                this.comboBoxFilter.SelectedIndex = 0;
                this._Icon = Icon;
                this.setIcon(Icon);
                string SQL = Adapter.SelectCommand.CommandText;
                SQL = SQL.Substring(SQL.IndexOf(" FROM ") + 6);
                if (SQL.IndexOf(" ") > -1) // Markus 17.7.23:  Bugfix if no space present
                    SQL = SQL.Substring(0, SQL.IndexOf(" ")).Trim();
                if (this._TableName == null || this._TableName.Length == 0)
                    this._TableName = SQL;
                this.Text = "Edit contents of table";
                if (this._TableName.Length > 0)
                    this.Text += " " + this._TableName;
                this.groupBoxEdit.Text = "Edit the contents of the table " + this._TableName;
                if (this._TableName == "Annotation" || this._TableName == "ExternalIdentifier")
                {
                    string ReferencedTable = Adapter.SelectCommand.CommandText.Substring(Adapter.SelectCommand.CommandText.IndexOf(" T.ReferencedTable = '") + " ReferencedTable = '".Length + 2);
                    ReferencedTable = ReferencedTable.Substring(0, ReferencedTable.IndexOf("'"));
                    this.Text += " for table " + ReferencedTable;
                    this.groupBoxEdit.Text += " for table " + ReferencedTable;
                }

                System.Data.DataTable dtColumns = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter("select C.COLUMN_NAME, C.DATA_TYPE from INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = '" + this._TableName + "'", DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dtColumns);

                /// init a sql adapter esp. for error ... Concurrency violation: the UpdateCommand affected 0 of the expected 1 records ...
                Microsoft.Data.SqlClient.SqlCommandBuilder cb = new Microsoft.Data.SqlClient.SqlCommandBuilder(ad);
                cb.ConflictOption = ConflictOption.OverwriteChanges;

                System.Data.DataRow[] RR = dtColumns.Select("DATA_TYPE = 'geography' OR DATA_TYPE = 'geometry'", "");
                if (RR.Length > 0)
                {
                    string Columns = "";
                    foreach (System.Data.DataRow R in dtColumns.Rows)
                    {
                        if (Columns.Length > 0)
                            Columns += ", ";
                        if (this._TableAlias.Length > 0)
                            Columns += this._TableAlias + ".";
                        Columns += R[0].ToString();
                        if (!CanReadGeographyDataType && (R[1].ToString().ToLower() == "geography" || R[1].ToString().ToLower() == "geometry"))
                        {
                            Columns += ".ToString() AS " + R[0].ToString();
                        }
                    }
                    if (this._TableAlias.Length > 0)
                        Adapter.SelectCommand.CommandText = Adapter.SelectCommand.CommandText.Replace(this._TableAlias + ".*", Columns);
                    else
                        Adapter.SelectCommand.CommandText = Adapter.SelectCommand.CommandText.Replace("*", Columns);
                }

                this.buttonDelete.Visible = FF.getObjectPermissions(this._TableName, "DELETE");

                if (this._ComboBoxColumns == null)
                {
                    // Check if ID according to DSGVO is present
                    SQL = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.COLUMN_NAME = 'ID' AND C.TABLE_NAME = 'UserProxy'";
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Result == "1" &&
                        (this.Table().Columns.ContainsKey("LogCreatedBy") ||
                        this.Table().Columns.ContainsKey("LogUpdatedBy") ||
                        this.Table().Columns.ContainsKey("LogInsertedBy")))
                    {
                        System.Data.DataTable dtLogByColumns = new DataTable();
                        string Message = "";
                        SQL = "SELECT C.COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.COLUMN_NAME LIKE 'Log%By' AND C.TABLE_NAME = '" + this._TableName + "'";
                        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtLogByColumns, ref Message);

                        System.Data.DataTable dtUser = new DataTable();
                        SQL = "SELECT cast(ID as varchar) AS ID, " +
                            "CASE WHEN [CombinedNameCache] IS NULL OR [CombinedNameCache] = '' THEN [LoginName] ELSE [CombinedNameCache] END AS UserName " +
                            "FROM UserProxy " +
                            "ORDER BY UserName ";
                        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtUser, ref Message);

                        this._ComboBoxColumns = new Dictionary<string, ComboBoxValues>();
                        foreach (System.Data.DataRow R in dtLogByColumns.Rows)
                        {
                            DiversityWorkbench.Forms.FormTableEditor.ComboBoxValues C = new ComboBoxValues(dtUser, "UserName", "ID", R[0].ToString());
                            this._ComboBoxColumns.Add(R[0].ToString(), C);
                        }
                        this._UsedComboBoxColumns = this._ComboBoxColumns;
                    }
                }

                this._Adapter = Adapter;
                if (this._TimeOut != null)
                    this._Adapter.SelectCommand.CommandTimeout = (int)this._TimeOut;
                this._dt = new DataTable();
                try
                {
                    this._Adapter.Fill(_dt);
                    //this.dataGridView.DataSource = _dt;
                    this.labelInfo.Text = this._dt.Rows.Count.ToString() + " data rows";
                    this.dataGridView.Columns.Clear();
                    foreach (DataColumn dataCol in _dt.Columns)
                    {
                        DataGridViewColumn columnView = null;
                        string columnHeader = dataCol.ColumnName;
                        if (this._UsedComboBoxColumns != null && this._UsedComboBoxColumns.ContainsKey(dataCol.ColumnName))
                        {
                            columnView = new DataGridViewComboBoxColumn();
                            columnView.ReadOnly = ReadOnlyColumns.Contains(dataCol.ColumnName);
                            ((DataGridViewComboBoxColumn)columnView).DataSource = this._UsedComboBoxColumns[dataCol.ColumnName].DataTable;
                            ((DataGridViewComboBoxColumn)columnView).ValueMember = this._UsedComboBoxColumns[dataCol.ColumnName].ValueMember;
                            ((DataGridViewComboBoxColumn)columnView).DisplayMember = this._UsedComboBoxColumns[dataCol.ColumnName].DisplayMember;
                            ((DataGridViewComboBoxColumn)columnView).DisplayStyle = ReadOnlyColumns.Contains(dataCol.ColumnName) ? DataGridViewComboBoxDisplayStyle.Nothing : DataGridViewComboBoxDisplayStyle.DropDownButton;
                            ((DataGridViewComboBoxColumn)columnView).FlatStyle = FlatStyle.Popup;
                            if (this._UsedComboBoxColumns[dataCol.ColumnName].ColumnTitle != "")
                                columnHeader = this._UsedComboBoxColumns[dataCol.ColumnName].ColumnTitle;
                        }
                        else
                        {
                            columnView = new DataGridViewTextBoxColumn();
                            columnView.ReadOnly = ReadOnlyColumns.Contains(dataCol.ColumnName) || LookUpColumns.Contains(dataCol.ColumnName);
                        }
                        columnView.DataPropertyName = dataCol.ColumnName;
                        columnView.HeaderText = columnHeader;
                        this.dataGridView.Columns.Add(columnView);
                    }
                    this.dataGridView.DataSource = _dt;
                    this.setDataGridColorRange();
                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    if (CanReadGeographyDataType && RR.Length > 0)
                        CanReadGeographyDataType = false;
                    this.Close();
                }
                // Check for log tables
                SQL = "SELECT COUNT(*) FROM [INFORMATION_SCHEMA].[TABLES] WHERE TABLE_NAME = '" + _TableName + "_log'";
                string result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (result != "" && result != "0")
                    this.buttonOpenLog.Visible = true;
                else
                    this.buttonOpenLog.Visible = false;
                this.progressBar.Visible = false;

                // ID - button
                this.checkBoxUseComboboxValues.Visible = _ComboBoxColumns != null;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        public void initTable(System.Collections.Generic.List<int> IDlist)
        {
            System.Collections.Generic.List<string> ReadOnlyColumns = new List<string>();
            string SqlCol = "SELECT C.COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = '" + this._TableName + "'";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter adCol = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCol, DiversityWorkbench.Settings.ConnectionString);
            adCol.Fill(dt);
            foreach (System.Data.DataRow R in dt.Rows)
            {
                if (R[0].ToString().EndsWith("ID") || R[0].ToString().StartsWith("Log") || R[0].ToString().ToLower().IndexOf("geometry") > -1 || R[0].ToString().ToLower().IndexOf("geography") > -1)
                    ReadOnlyColumns.Add(R[0].ToString());
            }

            string IDs = "";
            foreach (int i in IDlist)
            {
                if (IDs.Length > 0) IDs += ",";
                IDs += i.ToString();
            }
            string SQL = "SELECT * FROM [" + this._TableName + "] T WHERE T." + this._IDColumn + " IN (" + IDs + ")";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            this.initForm(this._Icon, ad, ReadOnlyColumns);
        }

        public void setColumWidth(DataGridViewAutoSizeColumnsMode mode)
        {
            this.dataGridView.AutoResizeColumns(mode);
        }

        public void setColumWidth(DataGridViewAutoSizeColumnMode mode)
        {
            foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
            {
                C.AutoSizeMode = mode;
            }
        }


        private void FormTableEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.dataGridView.EndEdit();
            try
            {
                bool HasChanges = this.dataGridView.IsCurrentRowDirty;
                foreach (System.Data.DataRow R in this._dt.Rows)
                {
                    if (R.RowState != DataRowState.Unchanged)
                    {
                        HasChanges = true;
                        break;
                    }
                }
                if (_DoSave && HasChanges && System.Windows.Forms.MessageBox.Show("Do you want to save the changes?", "Save changes?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    string Message = "";
                    if (!this.saveData(ref Message))
                    {
                        if (MessageBox.Show("Error during data saving.\r\n\r\nContinue editing?", "Continue editing?", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                            e.Cancel = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        public void setIcon(System.Drawing.Icon Icon)
        {
            this.Icon = Icon;
        }

        public void setIcon(System.Drawing.Image Image)
        {
            Bitmap theBitmap = new Bitmap(Image, new Size(16, 16));
            IntPtr Hicon = theBitmap.GetHicon();// Get an Hicon for myBitmap.
            Icon newIcon = Icon.FromHandle(Hicon);// Create a new icon from the handle.
            this.setIcon(newIcon);
        }

        private bool _ReadOnly;

        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set
            {

                _ReadOnly = value;
                if (_ReadOnly)
                {
                    this.buttonSave.Visible = false;
                    this.buttonDelete.Visible = false;
                    this.groupBoxEdit.Visible = false;
                    //this.progressBar.Visible = false;
                    this.buttonOpenLog.Visible = false;
                    this.buttonMarkColumn.Visible = false;
                    this.buttonClose.Visible = false;
                    this.dataGridView.ReadOnly = true;
                    this.dataGridView.AllowUserToAddRows = false;
                    this.dataGridView.AllowUserToDeleteRows = false;
                    foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                    {
                        C.ReadOnly = true;
                    }
                }
            }
        }

        public void setDataGridColorRange()
        {
            for (int i = 0; i < this.dataGridView.Columns.Count; i++)
            {
                if (this.LookUpColumns.Contains(this.dataGridView.Columns[i].DataPropertyName))
                    this.dataGridView.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
                else if (this._ReadOnlyColumns.Contains(this.dataGridView.Columns[i].DataPropertyName))
                    this.dataGridView.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.Silver;
            }
        }

        private void buttonSetColumnWidth_Click(object sender, EventArgs e)
        {
            //this.dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            if (this.dataGridView.AutoSizeColumnsMode != DataGridViewAutoSizeColumnsMode.AllCells)
            {
                this.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                this.buttonSetColumnWidth.BackColor = System.Drawing.Color.Yellow;
                this.buttonSetColumnWidthContent.BackColor = System.Drawing.SystemColors.Control;
            }
            else
            {
                this.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                this.buttonSetColumnWidth.BackColor = System.Drawing.SystemColors.Control;
            }
        }

        private void buttonSetColumnWidthContent_Click(object sender, EventArgs e)
        {
            //this.dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            if (this.dataGridView.AutoSizeColumnsMode != DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader)
            {
                this.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
                this.buttonSetColumnWidth.BackColor = System.Drawing.SystemColors.Control;
                this.buttonSetColumnWidthContent.BackColor = System.Drawing.Color.Yellow;
            }
            else
            {
                this.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                this.buttonSetColumnWidthContent.BackColor = System.Drawing.SystemColors.Control;
            }
        }

        public void setHelpProvider(string HelpNamespace, string Keyword)
        {
            this.helpProvider.HelpNamespace = HelpNamespace;
            this.helpProvider.SetHelpNavigator(this, HelpNavigator.KeywordIndex);
            this.helpProvider.SetHelpKeyword(this, Keyword);
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            string SQL = "";
            if (this._Adapter != null && this._Adapter.SelectCommand != null && this._Adapter.SelectCommand.CommandText.Length > 0)
                SQL = this._Adapter.SelectCommand.CommandText;
            string ID = "";
            if (this._Table != null)
            {
                try
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Data.Column> DC in this._Table.Columns)
                    {
                        if (DC.Value.IsIdentity)
                        {
                            ID = this._dt.Rows[this.dataGridView.SelectedCells[0].RowIndex][DC.Key].ToString();
                        }
                    }
                    if (ID.Length == 0)
                        ID = this._dt.Rows[this.dataGridView.SelectedCells[0].RowIndex][0].ToString();
                }
                catch (System.Exception ex) { }
            }
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), SQL, ID);
        }

        #region Rowheader with numbers

        public void ShowRowHeader(bool Show)
        {
            this.dataGridView.RowHeadersVisible = Show;
        }

        private void buttonShowRowNumber_Click(object sender, EventArgs e)
        {
            this.buttonShowRowNumber.Visible = false;
            this.setRowNumber();
        }

        private void setRowNumber()
        {
            this.dataGridView.SuspendLayout();
            foreach (DataGridViewRow row in this.dataGridView.Rows)
            {
                row.HeaderCell.Value = (row.Index + 1).ToString();
            }
            this.dataGridView.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
            this.dataGridView.ResumeLayout();
        }

        #endregion

        #endregion

        #region Filtering

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            this.dataGridView.SuspendLayout();
            System.Windows.Forms.DataGridViewAutoSizeColumnsMode M = this.dataGridView.AutoSizeColumnsMode;
            this.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            try
            {
                string Filter = this.textBoxFilter.Text;
                if (!this.checkBoxFilterCaseSensitiv.Checked)
                    Filter = Filter.ToLower();
                if (!this._dt.Columns.Contains(this.FilterColumn))
                    return;
                this.progressBar.Visible = true;
                this.progressBar.Value = 0;
                this.progressBar.Maximum = this._dt.Rows.Count;
                string DataType = this._dt.Columns[this.FilterColumn].DataType.Name.ToString().ToLower();
                if (this.comboBoxFilter.Text == "=" && !this.checkBoxFilterCaseSensitiv.Checked)
                {
                    string RowFilter = this.FilterColumn + " <> '" + Filter + "' OR " + this.FilterColumn + " = '' OR " + this.FilterColumn + " IS NULL";
                    if (DataType == "boolean")
                    {
                        RowFilter = this.FilterColumn + " <> " + this.textBoxFilter.Text + " OR " + this.FilterColumn + " IS NULL";
                    }
                    else if (DataType.StartsWith("int") || DataType.EndsWith("int") || DataType == "double" || DataType == "single")
                    {
                        if (Filter.Length == 0)
                            RowFilter = "NOT (" + this.FilterColumn + " IS NULL)";
                        else
                            RowFilter = this.FilterColumn + " <> " + Filter + " OR " + this.FilterColumn + " IS NULL";
                    }
                    System.Data.DataRow[] RR = this._dt.Select(RowFilter, "");
                    foreach (System.Data.DataRow R in RR)
                        R.Delete();
                }
                else if (this.comboBoxFilter.Text == "≠" && !this.checkBoxFilterCaseSensitiv.Checked)
                {
                    System.Data.DataRow[] RR = this._dt.Select(this.FilterColumn + " = '" + Filter + "'", "");
                    foreach (System.Data.DataRow R in RR)
                        R.Delete();
                }
                else if (this.comboBoxFilter.Text == "~" && !this.checkBoxFilterCaseSensitiv.Checked)
                {
                    System.Data.DataRow[] RR = this._dt.Select(this.FilterColumn + " NOT LIKE '%" + Filter + "%' OR " + this.FilterColumn + " = '' OR " + this.FilterColumn + " IS NULL", "");
                    foreach (System.Data.DataRow R in RR)
                        R.Delete();
                }
                else
                {
                    foreach (System.Data.DataRow R in this._dt.Rows)
                    {
                        string Content = R[this.FilterColumn].ToString();
                        if (!this.checkBoxFilterCaseSensitiv.Checked)
                            Content = Content.ToLower();
                        string Operator = this.comboBoxFilter.Text;
                        if (this.comboBoxFilter.SelectedItem != null)
                            Operator = this.comboBoxFilter.SelectedItem.ToString();
                        if (Operator == "=")
                        {
                            if (Content != Filter)
                                R.Delete();
                        }
                        else if (Operator == "~")
                        {
                            if (Content.IndexOf(Filter) == -1)
                                R.Delete();
                        }
                        else
                        {
                            if (Content == Filter)
                                R.Delete();
                        }
                        if (this.progressBar.Value < this.progressBar.Maximum)
                            this.progressBar.Value++;
                    }
                }
                Application.DoEvents();
                this._dt.AcceptChanges();
                this.labelInfo.Text = this._dt.Rows.Count.ToString() + " data rows filtered";
                this.buttonFilter.BackColor = System.Drawing.Color.Yellow;
                this.progressBar.Visible = false;
            }
            catch (System.Exception ex)
            {
            }
            this.dataGridView.AutoSizeColumnsMode = M;
            this.dataGridView.ResumeLayout();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            this._dt.Clear();
            this._Adapter.Fill(_dt);
            this.dataGridView.DataSource = _dt;
            this.labelInfo.Text = this._dt.Rows.Count.ToString() + " data rows";
            this.buttonFilter.BackColor = System.Drawing.Color.Transparent;
        }

        #endregion

        #region Datagrid events

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.dataGridView.SelectedCells.Count == 0)
                    return;
                if (this.dataGridView.SelectedCells[0].Value.ToString().Length > 0)
                    this.textBoxFilter.Text = this.dataGridView.SelectedCells[0].Value.ToString();
                else this.textBoxFilter.Text = "";
                this.FilterColumn = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName;
                this._FilterColumnIndex = this.dataGridView.SelectedCells[0].ColumnIndex;
                // Markus 23.5.23: Bugfix for missing content
                string Column = this.dataGridView.Columns[this._FilterColumnIndex].DataPropertyName;
                DiversityWorkbench.Data.Column.DataTypeBase Type = Data.Column.DataTypeBase.text;
                if (this.Table().Columns.ContainsKey(Column)) Type = this.Table().Columns[Column].DataTypeBasicType;
                this.comboBoxFilter.Items.Clear();
                this.comboBoxFilter.Items.Add("=");
                this.comboBoxFilter.Items.Add("≠");
                if (Type != Data.Column.DataTypeBase.numeric)
                    this.comboBoxFilter.Items.Add("~");
                this.comboBoxFilter.SelectedIndex = 0;
                if (this._ReadOnlyColumns.Contains(this.FilterColumn) || this.ReadOnly)
                {
                    this.comboBoxAction.SelectedIndex = -1;
                    this.groupBoxEdit.Visible = false;
                }
                else
                {
                    this.comboBoxAction.Visible = true;
                    this.buttonEdit.Visible = true;
                    this.comboBoxAction_SelectedIndexChanged(null, null);
                    this.groupBoxEdit.Visible = true;
                    this.groupBoxEdit.Text = "Edit " + this.FilterColumn;
                }
                if (this._ReadOnlyColumns.Contains(this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName))
                {
                    this.pasteToolStripMenuItem.Enabled = false;
                    this.clearSelectedCellsToolStripMenuItem.Enabled = false;
                }
                else
                {
                    this.pasteToolStripMenuItem.Enabled = true;
                    this.clearSelectedCellsToolStripMenuItem.Enabled = true;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dataGridView.SelectedRows == null || this.dataGridView.SelectedRows.Count == 0)
                this.buttonDelete.Enabled = false;
            else
                this.buttonDelete.Enabled = true;

            if (this.dataGridView.SelectedCells != null && this.dataGridView.SelectedCells.Count > 0 && this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].ReadOnly)
                this.dataGridView_CellClick(sender, null);
        }

        #endregion

        #region Handling the data

        private bool saveData(ref string Message)
        {
            string ErrorMessage = "";
            bool result = true;
            if (this.ReadOnly)
                return result;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            
            try
            {
                // Enforce update of data table (first selected cell is last entry in list... 
                if (this.dataGridView.SelectedCells.Count > 0)
                {
                    System.Data.DataRow R =
                        this._dt.Rows[
                            this.dataGridView.SelectedCells[this.dataGridView.SelectedCells.Count - 1].RowIndex];
                    R.BeginEdit();
                    R.EndEdit();
                }

                if (this.dataGridView.SelectedCells.Count > 1)
                {
                    foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridView.SelectedCells)
                    {
                        System.Data.DataRow R = this._dt.Rows[C.RowIndex];
                        R.BeginEdit();
                        R.EndEdit();
                    }
                }

                Microsoft.Data.SqlClient.SqlCommandBuilder B =
                    new Microsoft.Data.SqlClient.SqlCommandBuilder(this._Adapter);
                this._Adapter.Update(_dt);
            }
            catch (System.Exception ex)
            {
                result = false;
                ErrorMessage += ex.Message + "\r\n\r\n";
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
            ///Todo: Versuch das fehlgeschlagene Update durchzufuehren (Ursache unklar) - klappt nicht
            //if (!result)
            //{
            //    foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridView.SelectedCells)
            //    {
            //        System.Data.DataRow R = this._dt.Rows[C.RowIndex];
            //        R.BeginEdit();
            //        R.EndEdit();
            //        string Restriction = "";
            //        if (this._dt.PrimaryKey.Length > 0)
            //        {
            //            foreach (System.Data.DataColumn Col in this._dt.Columns)
            //            {
            //                if (_dt.PrimaryKey.Contains(Col))
            //                {
            //                    if (Restriction.Length > 0) Restriction += " AND ";
            //                    if (R[Col.ColumnName].Equals(System.DBNull.Value)) Restriction += Col.ColumnName + " IS NULL ";
            //                    else Restriction += Col.ColumnName + " = '" + R[Col.ColumnName].ToString() + "' ";
            //                }
            //            }
            //        }
            //        else
            //        {
            //            System.Data.DataTable dtPK = new DataTable();
            //            string SQL = "SELECT COLUMN_NAME " +
            //                "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
            //                "WHERE (TABLE_NAME = '" + this._TableName + "') AND (EXISTS " +
            //                "(SELECT * " +
            //                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
            //                "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS T ON K.CONSTRAINT_NAME = T.CONSTRAINT_NAME " +
            //                "WHERE (T.CONSTRAINT_TYPE = 'PRIMARY KEY') AND (K.COLUMN_NAME = C.COLUMN_NAME) AND (K.TABLE_NAME = C.TABLE_NAME)))";
            //            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //            ad.Fill(dtPK);
            //            foreach (System.Data.DataRow rPK in dtPK.Rows)
            //            {
            //                if (Restriction.Length > 0) Restriction += " AND ";
            //                if (R[rPK[0].ToString()].Equals(System.DBNull.Value)) Restriction += rPK[0].ToString() + " IS NULL ";
            //                else Restriction += rPK[0].ToString() + " = '" + R[rPK[0].ToString()].ToString() + "' ";
            //            }
            //        }
            //        System.Data.DataRow[] RR = this._dt.Select(Restriction, "");
            //        if (RR.Length == 1)
            //        {
            //            try
            //            {
            //                this._Adapter.Update(RR);
            //            }
            //            catch (System.Exception ex)
            //            {
            //                ErrorMessage += ex.Message + "\r\n\r\n";
            //                break;
            //            }
            //        }
            //    }
            //}


            if (this.dataGridView.SelectedCells.Count > 0)
            {
                // Enforce actualization of header field
                this.dataGridView.Rows[this.dataGridView.SelectedCells[0].RowIndex].HeaderCell.Value = "";
                this.dataGridView.Rows[this.dataGridView.SelectedCells[0].RowIndex].HeaderCell.Value = null;
            }
            Message = ErrorMessage;
            return result;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string Message = "";
            if (!this.saveData(ref Message) && Message.Length > 0)
                MessageBox.Show("Error during saving:\r\n" + Message);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this._DoSave = false;
            this.Close();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (this.dataGridView.SelectedRows == null || this.dataGridView.SelectedRows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("No rows selected");
                return;
            }
            int i = this.dataGridView.SelectedRows.Count;
            int iMax = i;
            if (System.Windows.Forms.MessageBox.Show("Are you sure that you want to delete the " + i.ToString() + " selected data from the database?", "Delete", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                i = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                try
                {
                    if (this._Adapter.DeleteCommand == null)
                    {
                        Microsoft.Data.SqlClient.SqlCommandBuilder CB = new Microsoft.Data.SqlClient.SqlCommandBuilder(this._Adapter);
                        Microsoft.Data.SqlClient.SqlCommand C = CB.GetDeleteCommand();
                        C.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                        //CB.GetDeleteCommand();
                    }
                    //this._Adapter.DeleteCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                    foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.SelectedRows)
                    {
                        try
                        {
                            System.Data.DataRowView Row = (System.Data.DataRowView)R.DataBoundItem;
                            Row.Delete();
                            this._Adapter.Update(this._dt);
                            i++;
                        }
                        catch (System.Exception ex)
                        {
                            if (System.Windows.Forms.MessageBox.Show("Deleting failed: " + ex.Message, "Cancel deleting?", MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop) == System.Windows.Forms.DialogResult.Cancel)
                                break;
                        }
                    }
                    this.buttonReset_Click(null, null);
                    System.Windows.Forms.MessageBox.Show(i.ToString() + " lines deleted");
                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Deleting failed: " + ex.Message);
                }
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        #endregion

        #region Editing

        private void buttonMarkColumn_Click(object sender, EventArgs e)
        {
            int iLines = this.dataGridView.Rows.Count;
            for (int i = 0; i < iLines; i++)
            {
                this.dataGridView.Rows[i].Cells[this.dataGridView.SelectedCells[0].ColumnIndex].Selected = true;
            }
        }

        private void comboBoxAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SuspendLayout();
            this.buttonEdit.Enabled = true;
            this.textBoxEditInsert.Visible = false;
            this.textBoxEditReplace.Visible = false;
            this.comboBoxEditInsert.Visible = false;
            this.comboBoxEditReplace.Visible = false;
            this.labelEditReplace.Visible = false;
            this.panelEditReplace.Visible = false;

            this.textBoxEditInsert.ContextMenuStrip = null;
            this.textBoxEditReplace.ContextMenuStrip = null;

            string Action = "";
            if (this.comboBoxAction.SelectedItem == null)
                this.comboBoxAction.SelectedIndex = 0;
            Action = this.comboBoxAction.SelectedItem.ToString();
#if !DEBUG
            if (Action == EditAction.Transfer.ToString())
            {
                System.Windows.Forms.MessageBox.Show("Available in upcoming version");
                this.comboBoxAction.SelectedIndex = 0;
            }
#endif
            if (this.LookUpColumns != null)
            {
                if (Action == EditAction.Append.ToString()
                    || Action == EditAction.Insert.ToString())
                {
                    if (this.LookUpColumns.Contains(this.FilterColumn))
                    {
                        this.comboBoxEditInsert.Visible = true;
                        this.comboBoxEditInsert.DataSource = this._LookupTables[this.FilterColumn];
                        this.comboBoxEditInsert.DisplayMember = this._LookupTables[this.FilterColumn].Columns[0].ColumnName;
                        this.comboBoxEditInsert.ValueMember = this._LookupTables[this.FilterColumn].Columns[0].ColumnName;
                        this.textBoxEditInsert.Dock = DockStyle.Right;
                        this.comboBoxEditInsert.Dock = DockStyle.Fill;
                    }
                    else
                    {
                        this.textBoxEditInsert.Visible = true;
                        this.textBoxEditInsert.Dock = DockStyle.Fill;
                        this.comboBoxEditInsert.Dock = DockStyle.Right;
                    }
                    if (Action == EditAction.Append.ToString())
                        this.EditingAction = EditAction.Append;
                    else
                        this.EditingAction = EditAction.Insert;
                }
                else if (Action == EditAction.Replace.ToString())
                {
                    this.textBoxEditReplace.ContextMenuStrip = this.contextMenuStripEditReplace;
                    this.textBoxEditInsert.ContextMenuStrip = this.contextMenuStripEditInsert;
                    this.panelEditReplace.Visible = true;
                    if (this.LookUpColumns.Contains(this.FilterColumn))
                    {
                        this.comboBoxEditReplace.Visible = true;
                        this.comboBoxEditInsert.Visible = true;
                        this.labelEditReplace.Visible = true;

                        this.comboBoxEditReplace.Dock = DockStyle.Fill;
                        this.textBoxEditReplace.Dock = DockStyle.Right;

                        this.comboBoxEditInsert.Dock = DockStyle.Fill;
                        this.textBoxEditInsert.Dock = DockStyle.Right;

                        this.comboBoxEditInsert.DataSource = this._LookupTables[this.FilterColumn];
                        this.comboBoxEditInsert.DisplayMember = this._LookupTables[this.FilterColumn].Columns[0].ColumnName;
                        this.comboBoxEditInsert.ValueMember = this._LookupTables[this.FilterColumn].Columns[0].ColumnName;
                        //this.comboBoxEditInsert.Sorted = true;

                        System.Data.DataTable dtReplace = this._LookupTables[this.FilterColumn].Copy();
                        this.comboBoxEditReplace.DataSource = dtReplace;
                        this.comboBoxEditReplace.DisplayMember = dtReplace.Columns[0].ColumnName;
                        this.comboBoxEditReplace.ValueMember = dtReplace.Columns[0].ColumnName;
                        //this.comboBoxEditReplace.Sorted  = true;
                    }
                    else
                    {
                        this.textBoxEditReplace.Visible = true;
                        this.labelEditReplace.Visible = true;
                        this.textBoxEditInsert.Visible = true;

                        this.comboBoxEditReplace.Dock = DockStyle.Right;
                        this.textBoxEditReplace.Dock = DockStyle.Fill;

                        this.comboBoxEditInsert.Dock = DockStyle.Right;
                        this.textBoxEditInsert.Dock = DockStyle.Fill;
                    }
                    this.EditingAction = EditAction.Replace;
                }
                else if (Action == EditAction.Clear.ToString()
                    || Action == EditAction.Trim.ToString())
                {
                    System.Windows.Forms.DataGridViewCell C = this.dataGridView.SelectedCells[0];
                    if (C.GetType() == typeof(System.Windows.Forms.DataGridViewComboBoxCell))
                        this.buttonEdit.Enabled = false;  // TODO: setzen auf NULL klappt nicht
                    else
                    {
                        if (this.LookUpColumns.Contains(this.FilterColumn)) // TODO: setzen auf NULL klappt nicht
                        {
                            this.buttonEdit.Enabled = true;// false;
                        }
                        else
                            this.buttonEdit.Enabled = true;
                    }
                    if (Action == EditAction.Clear.ToString())
                        this.EditingAction = EditAction.Clear;
                    else
                        this.EditingAction = EditAction.Trim;
                }
                else
                {
                }
            }
            if (Action == EditAction.Transfer.ToString())
            {
                System.Data.DataTable dt = (System.Data.DataTable)this.dataGridView.DataSource;
                string ColumnName = dt.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].ColumnName;
                this.EditingAction = EditAction.Transfer;
                this.comboBoxEditInsert.Visible = true;
                this.textBoxEditInsert.Dock = DockStyle.Right;
                this.comboBoxEditInsert.Dock = DockStyle.Fill;

                this.panelEditReplace.Visible = true;
                this.comboBoxEditReplace.Visible = true;
                this.comboBoxEditReplace.Dock = DockStyle.Fill;
                this.textBoxEditReplace.Dock = DockStyle.Right;
                this.comboBoxEditReplace.DataSource = null;
                this.comboBoxEditReplace.Items.Clear();
                this.comboBoxEditReplace.Items.Add("Append");
                this.comboBoxEditReplace.Items.Add("Overwrite");
                this.comboBoxEditReplace.Items.Add("Prepend");
                this.comboBoxEditReplace.SelectedIndex = 1;
            }
            this.ResumeLayout();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
#if !DEBUG
            if (this.EditingAction == EditAction.Transfer)
            {
                System.Windows.Forms.MessageBox.Show("available in upcoming version");
                return;
            }
#endif
            this.dataGridView.SuspendLayout();
            System.Windows.Forms.DataGridViewAutoSizeColumnsMode M = this.dataGridView.AutoSizeColumnsMode;
            this.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            this.EditContents();
            this.dataGridView.AutoSizeColumnsMode = M;
            this.dataGridView.ResumeLayout();
        }

        private void EditContents()
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                bool ShowProgress = false;
                int iFactor = 100;
                //int iSteps = 0;
                if (this.dataGridView.SelectedCells.Count > 1000)
                {
                    ShowProgress = true;
                    //iSteps = (int)(this.dataGridView.SelectedCells.Count / iFactor);
                    this.progressBar.Visible = true;
                    this.progressBar.Value = 0;
                    this.progressBar.Maximum = (int)(this.dataGridView.SelectedCells.Count / iFactor);//iSteps;
                }
                for (int i = 0; i < this.dataGridView.SelectedCells.Count; i++)
                {
                    switch (this.EditingAction)
                    {
                        case EditAction.Insert:
                            if (this.LookUpColumns.Contains(this.FilterColumn))
                                this.dataGridView.SelectedCells[i].Value = this.comboBoxEditInsert.SelectedValue.ToString();
                            else
                            {
                                if (this._dt.Columns[this.dataGridView.Columns[this.dataGridView.SelectedCells[i].ColumnIndex].DataPropertyName].DataType == typeof(System.Boolean))
                                {
                                    System.Boolean B;
                                    if (System.Boolean.TryParse(this.textBoxEditInsert.Text, out B))
                                        this.dataGridView.SelectedCells[i].Value = B;
                                    else
                                    {
                                        System.Windows.Forms.MessageBox.Show(this.textBoxEditInsert.Text + " could not be converted into a boolean value");
                                        return;
                                    }
                                }
                                else if (this._dt.Columns[this.dataGridView.Columns[this.dataGridView.SelectedCells[i].ColumnIndex].DataPropertyName].DataType == typeof(System.DateTime))
                                {
                                    System.DateTime DT;
                                    if (System.DateTime.TryParse(this.textBoxEditInsert.Text, out DT))
                                        this.dataGridView.SelectedCells[i].Value = DT;
                                    else
                                    {
                                        System.Windows.Forms.MessageBox.Show(this.textBoxEditInsert.Text + " could not be converted into a date time value");
                                        return;
                                    }
                                }
                                else
                                {
                                    if (this.dataGridView.SelectedCells[i].Value != System.DBNull.Value)
                                        this.dataGridView.SelectedCells[i].Value = this.textBoxEditInsert.Text + this.dataGridView.SelectedCells[i].Value.ToString();
                                    else
                                        this.dataGridView.SelectedCells[i].Value = this.textBoxEditInsert.Text;
                                }
                            }
                            break;
                        case EditAction.Append:
                            if (this.LookUpColumns.Contains(this.FilterColumn))
                                this.dataGridView.SelectedCells[i].Value = this.comboBoxEditInsert.SelectedValue.ToString();
                            else
                            {
                                if (this._dt.Columns[this.dataGridView.Columns[this.dataGridView.SelectedCells[i].ColumnIndex].DataPropertyName].DataType == typeof(System.Boolean))
                                {
                                    System.Boolean B;
                                    if (System.Boolean.TryParse(this.textBoxEditInsert.Text, out B))
                                        this.dataGridView.SelectedCells[i].Value = B;
                                    else
                                    {
                                        System.Windows.Forms.MessageBox.Show(this.textBoxEditInsert.Text + " could not be converted into a boolean value");
                                        return;
                                    }
                                }
                                else
                                    this.dataGridView.SelectedCells[i].Value = this.dataGridView.SelectedCells[i].Value.ToString() + this.textBoxEditInsert.Text;
                            }
                            break;
                        case EditAction.Trim:
                            if (this._dt.Columns[this.dataGridView.Columns[this.dataGridView.SelectedCells[i].ColumnIndex].DataPropertyName].DataType != typeof(System.Boolean))
                                this.dataGridView.SelectedCells[i].Value = this.dataGridView.SelectedCells[i].Value.ToString().Trim();
                            break;
                        case EditAction.Replace:
                            if (this.LookUpColumns.Contains(this.FilterColumn))
                            {
                                if (this.dataGridView.SelectedCells[i].Value.ToString() == this.comboBoxEditInsert.SelectedValue.ToString())
                                {
                                    if (this.comboBoxEditReplace.SelectedValue.Equals(System.DBNull.Value))
                                        this.dataGridView.SelectedCells[i].Value = null;// this.comboBoxEditInsert.SelectedValue;
                                    else
                                        this.dataGridView.SelectedCells[i].Value = this.comboBoxEditReplace.SelectedValue.ToString();
                                }
                            }
                            else
                            {
                                if (this._dt.Columns[this.dataGridView.Columns[this.dataGridView.SelectedCells[i].ColumnIndex].DataPropertyName].DataType == typeof(System.Boolean))
                                {
                                    System.Boolean Bori;
                                    if (!System.Boolean.TryParse(this.textBoxEditInsert.Text, out Bori))
                                    {
                                        System.Windows.Forms.MessageBox.Show(this.textBoxEditInsert.Text + " could not be converted into a boolean value");
                                        return;
                                    }
                                    System.Boolean Bnew;
                                    if (!System.Boolean.TryParse(this.textBoxEditReplace.Text, out Bnew))
                                    {
                                        System.Windows.Forms.MessageBox.Show(this.textBoxEditReplace.Text + " could not be converted into a boolean value");
                                        return;
                                    }
                                    else
                                    {
                                        if (System.Boolean.Parse(this.dataGridView.SelectedCells[i].Value.ToString()) == Bori)
                                            this.dataGridView.SelectedCells[i].Value = Bnew;
                                    }
                                }
                                else
                                {
                                    if (this._EditNew != EditSign.None || this._EditOri != EditSign.None)
                                    {
                                        string Ori = this.textBoxEditInsert.Text;
                                        string New = this.textBoxEditReplace.Text;
                                        if (this._EditOri != EditSign.None)
                                        {
                                            switch (_EditOri)
                                            {
                                                case EditSign.Return:
                                                    Ori = "\r\n";
                                                    break;
                                                case EditSign.Tab:
                                                    Ori = "\t";
                                                    break;
                                            }
                                        }
                                        if (this._EditNew != EditSign.None)
                                        {
                                            switch (_EditNew)
                                            {
                                                case EditSign.Return:
                                                    New = "\r\n";
                                                    break;
                                                case EditSign.Tab:
                                                    New = "\t";
                                                    break;
                                            }
                                        }
                                        this.dataGridView.SelectedCells[i].Value = this.dataGridView.SelectedCells[i].Value.ToString().Replace(Ori, New);
                                    }
                                    else
                                        this.dataGridView.SelectedCells[i].Value = this.dataGridView.SelectedCells[i].Value.ToString().Replace(this.textBoxEditInsert.Text, this.textBoxEditReplace.Text);

                                }
                            }
                            break;
                        case EditAction.Clear:
                            // Markus 29.8.2016 - Inhalt von Comboboxen wird auf "" statt auf NULL gesetzt. Unklar wie man Index hier auf erstes Element setzen kann
                            if (this.dataGridView.SelectedCells[i].GetType() == typeof(System.Windows.Forms.DataGridViewComboBoxCell))
                            {
                                System.Windows.Forms.DataGridViewComboBoxCell CBC = (System.Windows.Forms.DataGridViewComboBoxCell)this.dataGridView.SelectedCells[i];
                                if (CBC.DataSource.GetType() == typeof(System.Data.DataTable))
                                {
                                    System.Data.DataTable T = (System.Data.DataTable)CBC.DataSource;
                                    if (T.Rows[0][0].Equals(System.DBNull.Value))
                                    {
                                        CBC.Value = null;// System.DBNull.Value;
                                    }
                                }
                            }
                            //this.dataGridView.SelectedCells[i].Value = null;// System.DBNull.Value;
                            this.dataGridView.SelectedCells[i].Value = System.DBNull.Value;
                            if (this.dataGridView.SelectedCells[i].Value != null)
                            {
                                this.dataGridView.SelectedCells[i].Value = System.DBNull.Value;
                                if (this.dataGridView.SelectedCells[i].Value != null)
                                    this.dataGridView.SelectedCells[i].Value = null;
                            }
                            if (this.dataGridView.SelectedCells[i].ValueType.FullName == "System.DateTime")
                            {
                                this.dataGridView.SelectedCells[i].Value = System.DBNull.Value;
                            }
                            break;
                        case EditAction.Transfer:
                            string Value = "";

                            if (this.dataGridView.SelectedCells[i].GetType() == typeof(System.Windows.Forms.DataGridViewComboBoxCell))
                            {
                                System.Windows.Forms.DataGridViewComboBoxCell CBC = (System.Windows.Forms.DataGridViewComboBoxCell)this.dataGridView.SelectedCells[i];
                                if (CBC.DataSource.GetType() == typeof(System.Data.DataTable))
                                {
                                    System.Data.DataTable T = (System.Data.DataTable)CBC.DataSource;
                                    if (T.Rows[0][0].Equals(System.DBNull.Value))
                                    {
                                        CBC.Value = null;// System.DBNull.Value;
                                    }
                                }
                            }
                            //this.dataGridView.SelectedCells[i].Value = null;// System.DBNull.Value;
                            this.dataGridView.SelectedCells[i].Value = System.DBNull.Value;
                            if (this.dataGridView.SelectedCells[i].Value != null)
                            {
                                this.dataGridView.SelectedCells[i].Value = System.DBNull.Value;
                                if (this.dataGridView.SelectedCells[i].Value != null)
                                    this.dataGridView.SelectedCells[i].Value = null;
                            }
                            if (this.dataGridView.SelectedCells[i].ValueType.FullName == "System.DateTime")
                            {
                                this.dataGridView.SelectedCells[i].Value = System.DBNull.Value;
                            }
                            break;
                    }
                    if (ShowProgress && i % iFactor == 0)
                    {
                        //iSteps++;
                        if (this.progressBar.Value < this.progressBar.Maximum)
                        {
                            this.progressBar.Value++;
                            System.Windows.Forms.Application.DoEvents();
                        }
                    }
                }
                this.progressBar.Visible = false;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void comboBoxEditInsert_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.EditingAction == EditAction.Transfer)
            {
#if !DEBUG
                System.Windows.Forms.MessageBox.Show("available in upcoming version");
                return;
#endif
            }
        }

        private void comboBoxEditInsert_DropDown(object sender, EventArgs e)
        {
            if (this.EditingAction == EditAction.Transfer)
            {
#if !DEBUG
                System.Windows.Forms.MessageBox.Show("available in upcoming version");
                return;
#endif
                System.Data.DataTable dt = (System.Data.DataTable)this.dataGridView.DataSource;
                string ColumnName = dt.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].ColumnName;
                this.EditingAction = EditAction.Transfer;
                this.comboBoxEditInsert.Visible = true;
                System.Data.DataTable dtColumns = new DataTable();
                System.Data.DataColumn dc = new DataColumn("Column", typeof(string));
                dtColumns.Columns.Add(dc);
                foreach (System.Collections.Generic.KeyValuePair<string, Data.Column> DC in this.Table().Columns)
                {
                    if (DC.Key == ColumnName)
                        continue;
                    if (DC.Value.IsIdentity)
                        continue;
                    if (DC.Key.ToUpper() == "ROWGUID")
                        continue;
                    if ((DC.Value.ForeignRelations == null || DC.Value.ForeignRelations.Count == 0)
                        && DC.Value.DataType == this.Table().Columns[ColumnName].DataType ||
                        (DC.Value.DataTypeBasicType == Data.Column.DataTypeBase.text &&
                        DC.Value.ForeignRelations.Count == 0))
                    {
                        System.Data.DataRow R = dtColumns.NewRow();
                        R[0] = DC.Key;
                        dtColumns.Rows.Add(R);
                    }
                }
                this.comboBoxEditInsert.DataSource = dtColumns;
                this.comboBoxEditInsert.DisplayMember = "Column";
                this.comboBoxEditInsert.ValueMember = "Column";
                this.textBoxEditInsert.Dock = DockStyle.Right;
                this.comboBoxEditInsert.Dock = DockStyle.Fill;
                //this.comboBoxEditInsert.Sorted = true;
            }
        }

        private void comboBoxEditReplace_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.EditingAction == EditAction.Transfer)
            {
#if !DEBUG
                System.Windows.Forms.MessageBox.Show("available in upcoming version");
                return;
#endif
                if (this.comboBoxEditReplace.SelectedItem.ToString() == "Overwrite")
                {

                }
                else
                {
                }
            }
        }

        #endregion

        #region Export

        private void buttonExport_Click(object sender, EventArgs e)
        {
            bool mappedExport = false;
            if (this._UsedComboBoxColumns != null && this._UsedComboBoxColumns.Count > 0)
            {
                string message = "Do you want to export the mapped (display) values?";
                mappedExport = MessageBox.Show(message, "Mapping", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes;
            }
            System.IO.StreamWriter sw;
            DateTime expTim = DateTime.Now;
            this._TableExportFile = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Export) + this._TableName + "_Export_" + expTim.ToString("yyyyMMdd_hhmmss") + ".txt";
            try
            {
                if (System.IO.File.Exists(this._TableExportFile))
                    sw = new System.IO.StreamWriter(this._TableExportFile, true, System.Text.Encoding.UTF8);
                else
                    sw = new System.IO.StreamWriter(this._TableExportFile, false, System.Text.Encoding.UTF8);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return;
            }
            try
            {
                sw.WriteLine(this._TableName + " export from Table editor");
                sw.WriteLine();
                sw.WriteLine("User:\t" + System.Environment.UserName);
                sw.Write("Date:\t");
                sw.WriteLine(expTim);
                sw.WriteLine();
                foreach (System.Data.DataColumn C in this._dt.Columns)
                {
                    string value = C.ColumnName;
                    if (mappedExport && _UsedComboBoxColumns.ContainsKey(C.ColumnName) && _UsedComboBoxColumns[C.ColumnName].ColumnTitle != "")
                        value = _UsedComboBoxColumns[C.ColumnName].ColumnTitle;
                    sw.Write(value + "\t");
                }
                sw.WriteLine();
                this.progressBar.Visible = true;
                this.progressBar.Value = 0;
                this.progressBar.Maximum = this._dt.Rows.Count;

                // Process data grid view rows to preserve order
                for (int rowIdx = 0; rowIdx < this.dataGridView.RowCount; rowIdx++)
                {
                    // Get data row
                    System.Data.DataRow R = ((System.Data.DataRowView)this.dataGridView.Rows[rowIdx].DataBoundItem).Row;
                    foreach (System.Data.DataColumn C in this._dt.Columns)
                    {
                        object value = R[C.ColumnName];
                        if (mappedExport && _UsedComboBoxColumns.ContainsKey(C.ColumnName))
                        {
                            foreach (DataRow CR in _UsedComboBoxColumns[C.ColumnName].DataTable.Rows)
                            {
                                if (CR[_UsedComboBoxColumns[C.ColumnName].ValueMember].ToString() == R[C.ColumnName].ToString())
                                {
                                    value = CR[_UsedComboBoxColumns[C.ColumnName].DisplayMember];
                                    break;
                                }
                            }
                        }
                        sw.Write(value.ToString() + "\t");
                    }
                    sw.WriteLine();
                    if (this.progressBar.Value < this.progressBar.Maximum)
                        this.progressBar.Value++;
                }
                Application.DoEvents();
                System.Windows.Forms.MessageBox.Show("Data were exported to " + this._TableExportFile);
                this.labelInfo.Text = this._dt.Rows.Count.ToString() + " data rows exported to " + this._TableExportFile;
                this.progressBar.Visible = false;
            }
            catch
            {
            }
            finally
            {
                sw.Close();
            }
        }

        private void buttonOpenExportFile_Click(object sender, EventArgs e)
        {
            if (this._TableExportFile.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("So far nothing has been exported");
                return;
            }
            
            try
            {
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = this._TableExportFile,
                    UseShellExecute = true
                });
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);

            }
        }

        private void buttonExportSqlite_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.SQLiteDB.DatabaseName() == null)
                    System.Windows.Forms.MessageBox.Show("Available in upcoming version");
                return;
                int iRows = this.SQLiteDB.TableRowCount(this._TableName);
                if (iRows > 0)
                {
                    if (System.Windows.Forms.MessageBox.Show("There is a previous export of the table " + _TableName + " with " + iRows.ToString() + " rows\r\n   These will be removed.\r\nExport data anyway and remove previous data?", "Remove old data", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                        return;
                }
                //System.IO.DirectoryInfo DI = new System.IO.DirectoryInfo(...Windows.Forms.Application.StartupPath + "\\Export");
                //if (!DI.Exists)
                //    DI.Create();
                System.Collections.Generic.Dictionary<string, string> TableColumns = new Dictionary<string, string>();
                System.Collections.Generic.Dictionary<string, string> ColumnValues = new Dictionary<string, string>();
                DiversityWorkbench.Data.Table T = new Data.Table(this._TableName, DiversityWorkbench.Settings.ConnectionString);
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Data.Column> KV in T.Columns)
                {
                    string DataType = KV.Value.DataType;
                    if (KV.Value.DataTypeLength != 0)
                    {
                        DataType += "(";
                        if (KV.Value.DataTypeLength == -1)
                            DataType += "4000";
                        else DataType += KV.Value.DataTypeLength.ToString();
                        DataType += ")";
                    }
                    string ExportColumn = KV.Key.Replace(" ", "");
                    if (TableColumns.ContainsKey(ExportColumn))
                    {
                        System.Windows.Forms.MessageBox.Show("The column names must be unique.\r\nColumn\r\n" + ExportColumn + "\r\nis used more than once");
                        return;
                    }
                    TableColumns.Add(ExportColumn, DataType);
                    ColumnValues.Add(ExportColumn, "");
                }
                if (!this.SQLiteDB.AddTable(this._TableName, TableColumns))
                    return;
                this.progressBar.Value = 0;
                this.progressBar.Maximum = this._dt.Rows.Count;
                for (int l = 0; l < this._dt.Rows.Count; l++)
                {
                    for (int c = 0; c < this._dt.Columns.Count; c++)
                    {
                        ColumnValues[this._dt.Columns[c].ToString()] = this._dt.Rows[l][c].ToString();
                    }
                    this._SQLiteDB.InsertData(this._TableName, ColumnValues);
                    if (l <= this.progressBar.Maximum)
                        this.progressBar.Value = l;
                }
                this.progressBar.Value = this.progressBar.Maximum;
                System.Windows.Forms.MessageBox.Show("Data exported to " + DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Export) + DiversityWorkbench.Settings.ModuleName + "Tables.sqlite");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                System.Windows.Forms.MessageBox.Show("Exported failed");
            }
        }

        //private readonly string _SQLiteExportTableName = "ExportDWB";

        private DiversityWorkbench.SqlLite.Database _SQLiteDB;

        public DiversityWorkbench.SqlLite.Database SQLiteDB
        {
            get
            {
                try
                {
                    if (this._SQLiteDB == null)
                    {
                        //string Directory = ...Windows.Forms.Application.StartupPath + "\\Export";
                        //if (!System.IO.Directory.Exists(Directory))
                        //    System.IO.Directory.CreateDirectory(Directory);
                        string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Export) + DiversityWorkbench.Settings.ModuleName + "Tables.sqlite";
                        this._SQLiteDB = new SqlLite.Database(Path);
                        //{"Die Datei oder Assembly \"System.Data.SQLite, Version=1.0.111.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139\" oder eine Abhängigkeit davon wurde nicht gefunden. Die gefundene Manifestdefinition der Assembly stimmt nicht mit dem Assemblyverweis überein. (Ausnahme von HRESULT: 0x80131040)":"System.Data.SQLite, Version=1.0.111.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139"}
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    this._SQLiteDB = null;
                }
                return _SQLiteDB;
            }
        }

        #endregion

        #region Log table

        private void buttonOpenLog_Click(object sender, EventArgs e)
        {
            string LogTableName = this._TableName + "_log";
            string SQL = this._Adapter.SelectCommand.CommandText;
            if (SQL.IndexOf(" FROM " + this._TableName + " ") > -1)
                SQL = SQL.Replace(" FROM " + this._TableName + " ", " FROM " + LogTableName + " ");
            else if (SQL.IndexOf(" FROM [" + this._TableName + "] ") > -1)
                SQL = SQL.Replace(" FROM [" + this._TableName + "] ", " FROM [" + LogTableName + "] ");
            Microsoft.Data.SqlClient.SqlDataAdapter adLog = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._Adapter.SelectCommand.Connection);// DiversityWorkbench.Settings.ConnectionString);
            System.Collections.Generic.List<string> Columns = new List<string>();
            DiversityWorkbench.Forms.FormTableEditor f = new FormTableEditor(this._Icon, adLog, Columns);
            f.ReadOnly = true;
            f.ShowDialog();
        }

        #endregion

        #region Use combobox values instead of IDs

        private void checkBoxUseComboboxValues_Click(object sender, EventArgs e)
        {
            if (this.checkBoxUseComboboxValues.Checked)
            {
                this._UsedComboBoxColumns = null;
            }
            else if (this._ComboBoxColumns != null)
            {
                this._UsedComboBoxColumns = this._ComboBoxColumns;
            }
            this.initForm(this._Icon, this._Adapter, this._ReadOnlyColumns);
        }

        #endregion

        #region Context menu table

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormFunctions.WriteToClipboard(this.dataGridView);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Clipboard.ContainsData(System.Windows.Forms.DataFormats.CommaSeparatedValue)
                || System.Windows.Forms.Clipboard.ContainsData(System.Windows.Forms.DataFormats.Text))
            {
                // finding the top row
                int IndexTopRow = this.dataGridView.Rows.Count;
                if (this.dataGridView.SelectedCells.Count > 0)
                {
                    foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridView.SelectedCells)
                        if (IndexTopRow > C.RowIndex) IndexTopRow = C.RowIndex;
                }

                // parsing the content of the clipboard
                System.Collections.Generic.List<System.Collections.Generic.List<string>> ClipBoardValues = DiversityWorkbench.Forms.FormFunctions.ClipBoardValues;// this.ClipBoardValues;
                System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums = DiversityWorkbench.Forms.FormFunctions.GridColums(this.dataGridView);
                if (!DiversityWorkbench.Forms.FormFunctions.CanCopyClipboardInDataGrid(IndexTopRow, ClipBoardValues, GridColums, this.dataGridView))
                    return;
                try
                {
                    for (int ii = 0; ii < GridColums.Count; ii++) // the columns
                    {
                        for (int i = 0; i < ClipBoardValues.Count; i++) // the rows
                        {
                            if (this.dataGridView.Rows[IndexTopRow + i].Cells[GridColums[ii].Index].ReadOnly)
                                continue;
                            if (DiversityWorkbench.Forms.FormFunctions.ValueIsValid(this.dataGridView, GridColums[ii].Index, ClipBoardValues[i][ii]))
                            {
                                this.dataGridView.Rows[IndexTopRow + i].Cells[GridColums[ii].Index].Value = ClipBoardValues[i][ii];
                                //this.checkForMissingAndDefaultValues(this.dataGridView.Rows[IndexTopRow + i].Cells[GridColums[ii].Index], true);
                            }
                            else
                            {
                                string Message = ClipBoardValues[i][ii] + " is not a valid value for "
                                    + this.dataGridView.Columns[GridColums[ii].Index].DataPropertyName
                                    + "\r\n\r\nDo you want to try to insert the other values?";
                                if (System.Windows.Forms.MessageBox.Show(Message, "Invalid value", MessageBoxButtons.YesNo) == DialogResult.No)
                                    break;
                            }
                            if (i + IndexTopRow + 3 > this.dataGridView.Rows.Count)
                                continue;
                        }
                    }
                }
                catch { }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Only text and spreadsheet values can be copied");
                return;
            }
        }

        private void clearSelectedCellsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridView.SelectedCells)
            {
                if (this._ReadOnlyColumns.Contains(this.dataGridView.Columns[C.ColumnIndex].DataPropertyName))
                    continue;
                try
                {
                    C.Value = null;
                    if (C.Value != null)
                    {
                        C.Value = "";
                        if (C.Value != "")
                            C.Value = System.DBNull.Value;
                    }
                }
                catch (System.Exception ex) { }
            }
        }

        #endregion

        #region context menu edit

        private enum EditSign { Tab, Return, None }

        private void setEditContent(System.Windows.Forms.TextBox textBox, EditSign sign)
        {
            textBox.ReadOnly = true;
            switch (sign)
            {
                case EditSign.Return:
                    textBox.Text = "¶";
                    if (textBox == this.textBoxEditInsert)
                        this._EditOri = EditSign.Return;
                    else
                        this._EditNew = EditSign.Return;
                    break;
                case EditSign.Tab:
                    textBox.Text = "→";
                    if (textBox == this.textBoxEditInsert)
                        this._EditOri = EditSign.Tab;
                    else
                        this._EditNew = EditSign.Tab;
                    break;
            }
        }

        private EditSign _EditOri = EditSign.None;
        private EditSign _EditNew = EditSign.None;

        private void resetEditContent(System.Windows.Forms.TextBox textBox)
        {
            textBox.Text = "";
            textBox.ReadOnly = false;
            if (textBox == this.textBoxEditInsert)
                this._EditOri = EditSign.None;
            else
                this._EditNew = EditSign.None;
        }

        private void returnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.setEditContent(this.textBoxEditInsert, EditSign.Return);
        }

        private void returnReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.setEditContent(this.textBoxEditReplace, EditSign.Return);
        }


        private void tabulatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.setEditContent(this.textBoxEditInsert, EditSign.Tab);
        }

        private void tabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.setEditContent(this.textBoxEditReplace, EditSign.Tab);
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.resetEditContent(this.textBoxEditReplace);
        }

        private void resetInsertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.resetEditContent(this.textBoxEditInsert);
        }

        #endregion

        #region Autosuggest

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    string ColumnName = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
                    // getting Table and ColumnName
                    System.Windows.Forms.AutoCompleteStringCollection autoCompleteStringCollection = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionOnDemand(this._TableName, ColumnName);
                    textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    textBox.AutoCompleteCustomSource = autoCompleteStringCollection;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Manual

        /// <summary>
        /// Adding event deletates to form and controls
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task InitManual()
        {
            try
            {

                DiversityWorkbench.DwbManual.Hugo manual = new Hugo(this.helpProvider, this);
                if (manual != null)
                {
                    await manual.addKeyDownF1ToForm();
                }
            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// ensure that init is only done once
        /// </summary>
        private bool _InitManualDone = false;


        /// <summary>
        /// KeyDown of the form adding event deletates to form and controls within the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (!_InitManualDone)
                {
                    await this.InitManual();
                    _InitManualDone = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion
    }
}
