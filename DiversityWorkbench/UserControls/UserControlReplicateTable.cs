using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.UserControls
{

    public partial class UserControlReplicateTable : UserControl
    {
        #region Parameter

        public enum ReplicationDirection { Download, Upload, Clean, Merge }
        private ReplicationDirection _ReplicationDirection;

        private System.Collections.Generic.List<string> _Messages;

        private int _MaxRowsForDirectDataLoad = 100;

        private enum _DataRelation { SourceUnchanged, SourceUpdated, SourceAndDestinationUpdated, ToBeChecked, NoInformation };
        private _DataRelation _RelationOfData;
        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<int, int>> _PKs;
        private string _TableName;

        public string TableName
        {
            get { return _TableName; }
            set { _TableName = value; }
        }

        private string _SqlRestrictionColumn;
        private string _SqlRestriction = "";
        private string _SqlRestrictionIDs;
        private int? _NumberOfSourceDatasets;
        private int _NumberOfConflicts;

        private int _NumberOfUpdatedRows = 0;
        private int _NumberOfInsertedRows = 0;
        private int _NumberOfDeletedRows = 0;
        private int _NumberOfErrors = 0;
        private int _NumberOfMissingRelations = 0;
        private int _NumberOfNoDifference = 0;
        private int _NumberOfForeignKeySameTableConflictsFound = 0;

        private int _NumberOfRows = 0;

        private System.Collections.Generic.List<string> _PrimaryKeyColumns;
        private System.Collections.Generic.List<string> _ForeignPrimaryKeyColumns;
        private System.Collections.Generic.List<DiversityWorkbench.UserControls.ForeignKey> _ForeignKeys;

        private string _IdentityColumn;
        //private string _ChildParentColumn;
        private string _ParentColumn;
        private System.Collections.Generic.List<string> _DataColumns;
        private System.Collections.Generic.Dictionary<string, string> _ColumnDictionary;
        private System.Data.DataTable _dtSource;
        private string _ColumnUpdateDateInSource = "";
        private string _ColumnInsertDateInSource = "";
        private string _ColumnUpdateDateInDestination = "";
        private string _ColumnInsertDateInDestination = "";
        private string _RowGUID = "";

        private bool _RowGUIDisPresent = false;
        private bool _PKisPresent = false;
        private bool _PKMatchesRowGUID = false;

        private System.Collections.Generic.List<ReplicationRow> _ReplicationRows;

        /// <summary>
        /// Dictionary for the identities in primary keys for the source and destination tables <name of the column><ID of source, ID of destination>>
        /// necessary as ID's might deviate in independet databases
        /// </summary>
        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<int, int>> _PrimaryKeys;// new Dictionary<string, Dictionary<int, int>>();

        public static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<int, int>> PrimaryKeys
        {
            get
            {
                if (DiversityWorkbench.UserControls.UserControlReplicateTable._PrimaryKeys == null)
                    DiversityWorkbench.UserControls.UserControlReplicateTable._PrimaryKeys = new Dictionary<string, Dictionary<int, int>>();
                return DiversityWorkbench.UserControls.UserControlReplicateTable._PrimaryKeys;
            }
        }

        private System.Drawing.Color _ColorInsert;
        private System.Drawing.Color _ColorUpdate;
        private System.Drawing.Color _ColorError;
        private System.Drawing.Color _ColorConlict;

        private DiversityWorkbench.ReplicationForm _replicationForm = null;

        #endregion

        #region Construction

        public UserControlReplicateTable(string TableName, ReplicationDirection Direction,
            string SqlRestrictionSourceColumn, string SqlRestrictionSourceIDs)
        {
            this._ReplicationDirection = Direction;

            InitializeComponent();

            this._ColorConlict = System.Drawing.Color.Magenta;
            this._ColorError = System.Drawing.Color.Red;
            this._ColorInsert = System.Drawing.Color.Black;
            this._ColorUpdate = System.Drawing.Color.SaddleBrown;

            this.checkBoxTableName.Text = TableName;

            this._TableName = TableName;
            //this._ConnectionStringPublisher = ConnectionStringPublisher;
            //this._ConnectionStringSubscriber = ConnectionStringSubscriber;
            this._SqlRestrictionColumn = SqlRestrictionSourceColumn;
            this._SqlRestrictionIDs = SqlRestrictionSourceIDs;

            switch (Direction)
            {
                case ReplicationDirection.Download:
                    this.buttonInfo.Image = this.imageListDirection.Images[0];
                    //this.labelNumberOfDatasets.Text = "Download ";
                    this.labelNumberOfDatasets.ForeColor = System.Drawing.Color.Blue;
                    break;
                case ReplicationDirection.Merge:
                    this.buttonInfo.Image = this.imageListDirection.Images[3];
                    //this.labelNumberOfDatasets.Text = "Merge ";
                    this.labelNumberOfDatasets.ForeColor = System.Drawing.Color.Blue;
                    break;
                case ReplicationDirection.Upload:
                    this.buttonInfo.Image = this.imageListDirection.Images[1];
                    //this.labelNumberOfDatasets.Text = "Upload ";
                    this.labelNumberOfDatasets.ForeColor = System.Drawing.Color.Blue;
                    break;
                case ReplicationDirection.Clean:
                    this.buttonInfo.Image = this.imageListDirection.Images[2];
                    //this.labelNumberOfDatasets.Text = "Delete ";
                    this.labelNumberOfDatasets.ForeColor = System.Drawing.Color.Red;
                    break;
            }

            this.InitNumberLabel(Direction);
            //int i = int.Parse(this.getNumberOfDatasets().ToString());
            //this.labelNumberOfDatasets.Text += i.ToString() + " row";
            //if (i != 1) this.labelNumberOfDatasets.Text += "s";
            //if (i == 0) this.checkBoxTableName.Checked = false;

            //this.toolTip.SetToolTip(this.buttonInfo, this.labelNumberOfDatasets.Text);
#if DEBUG
            if (Direction != ReplicationDirection.Clean)
            {
                this.buttonFilterClear.Visible = true;
                this.buttonFilterPropagate.Visible = true;
            }
#endif

        }

        public UserControlReplicateTable(string TableName, ReplicationDirection Direction,
            string SqlRestrictionSourceColumn, string SqlRestrictionSourceColumn2, string SqlRestrictionSourceIDs)
            : this(TableName, Direction, SqlRestrictionSourceColumn, SqlRestrictionSourceIDs)
        {
            this._SqlRestriction = SqlRestrictionSourceColumn2;
        }

        public UserControlReplicateTable(string TableName, ReplicationDirection Direction, string SqlRestriction)
        {
            this._ReplicationDirection = Direction;

            InitializeComponent();

            this._ColorConlict = System.Drawing.Color.Magenta;
            this._ColorError = System.Drawing.Color.Red;
            this._ColorInsert = System.Drawing.Color.Black;
            this._ColorUpdate = System.Drawing.Color.SaddleBrown;

            this.checkBoxTableName.Text = TableName;

            this._TableName = TableName;

            this._SqlRestriction = SqlRestriction;

            switch (Direction)
            {
                case ReplicationDirection.Download:
                    this.buttonInfo.Image = this.imageListDirection.Images[0];
                    //this.labelNumberOfDatasets.Text = "Download ";
                    this.labelNumberOfDatasets.ForeColor = System.Drawing.Color.Blue;
                    break;
                case ReplicationDirection.Merge:
                    this.buttonInfo.Image = this.imageListDirection.Images[3];
                    //this.labelNumberOfDatasets.Text = "Merge ";
                    this.labelNumberOfDatasets.ForeColor = System.Drawing.Color.Blue;
                    break;
                case ReplicationDirection.Upload:
                    this.buttonInfo.Image = this.imageListDirection.Images[1];
                    //this.labelNumberOfDatasets.Text = "Upload ";
                    this.labelNumberOfDatasets.ForeColor = System.Drawing.Color.Blue;
                    break;
                case ReplicationDirection.Clean:
                    this.buttonInfo.Image = this.imageListDirection.Images[2];
                    //this.labelNumberOfDatasets.Text = "Delete ";
                    this.labelNumberOfDatasets.ForeColor = System.Drawing.Color.Red;
                    break;
            }
            this.InitNumberLabel(Direction);
            //int i = int.Parse(this.getNumberOfDatasets().ToString());
            //this.labelNumberOfDatasets.Text += i.ToString() + " row";
            //if (i != 1) this.labelNumberOfDatasets.Text += "s";
            //if (i == 0) this.checkBoxTableName.Checked = false;

            //this.toolTip.SetToolTip(this.buttonInfo, this.labelNumberOfDatasets.Text);

        }

        private void InitNumberLabel(ReplicationDirection Direction)
        {
            switch (Direction)
            {
                case ReplicationDirection.Download:
                    this.labelNumberOfDatasets.Text = "Download ";
                    break;
                case ReplicationDirection.Merge:
                    this.labelNumberOfDatasets.Text = "Merge ";
                    break;
                case ReplicationDirection.Upload:
                    this.labelNumberOfDatasets.Text = "Upload ";
                    break;
                case ReplicationDirection.Clean:
                    this.labelNumberOfDatasets.Text = "Delete ";
                    break;
            }
            int i = int.Parse(this.getNumberOfDatasets().ToString());
            this.labelNumberOfDatasets.Text += i.ToString() + " row";
            if (i != 1) this.labelNumberOfDatasets.Text += "s";
            if (i == 0) this.checkBoxTableName.Checked = false;

            string Message = this.labelNumberOfDatasets.Text;
            if (this.Filter().Length > 0)
                Message += "\r\nFilter:\r\n" + this.Filter();

            this.toolTip.SetToolTip(this.buttonInfo, Message);
        }

        #endregion

        #region Interface

        public System.Drawing.Image TableImage
        {
            set
            {
                this.pictureBoxTable.Image = value;
            }
        }

        public void setReplicationForm(DiversityWorkbench.ReplicationForm replicationForm)
        {
            this._replicationForm = replicationForm;
        }

        public void setFilter(System.Collections.Generic.Dictionary<string, ReplicationFilter> Filters)
        {
            this.setReplicationFilters(Filters);
        }

        public void clearFilter()
        {
            this.clearReplicationFilter();
        }

        public int MaxRowsForDirectDataLoad { set => this._MaxRowsForDirectDataLoad = value; get => _MaxRowsForDirectDataLoad; }

        public void ReplicateTable()
        {
            bool OK = true;
            this.progressBar.Visible = false;
            this.labelProgress.Visible = true;
            if (this.checkBoxTableName.Checked)
            {
                try
                {
                    if (this.dtSource == null)
                    {
                        this.labelProgress.Text = "No source found";
                        return;
                    }

                    if (this.dtSource.Rows.Count == 0)
                    {
                        this.labelProgress.Text = "No data found";
                        return;
                    }

                    if (this._ReplicationDirection != ReplicationDirection.Clean)
                    {
                        this.labelProgress.Text = "Inserting missing parents";
                        Application.DoEvents();
                        this.InsertMissingParents();
                    }

                    this.labelProgress.Text = "Filling rows for replication";
                    Application.DoEvents();

                    this.fillRowsForReplication();

                    this.progressBar.Minimum = 0;
                    this.progressBar.Maximum = this._ReplicationRows.Count;
                    this.progressBar.Value = 0;

                    this._NumberOfConflicts = 0;
                    this._NumberOfUpdatedRows = 0;
                    this._NumberOfInsertedRows = 0;
                    this._NumberOfErrors = 0;
                    this._NumberOfMissingRelations = 0;
                    this._NumberOfNoDifference = 0;
                    this._NumberOfForeignKeySameTableConflictsFound = 0;

                    this._NumberOfRows = this._ReplicationRows.Count;

                    if (_NumberOfRows == 0)
                    {
                        System.Windows.Forms.MessageBox.Show("No valid rows detected");
                    }
                    bool ForeignKeySameTableConflictsFound = false;

                    if (this._ReplicationDirection == ReplicationDirection.Clean)
                    {
                        string SQL = "TRUNCATE TABLE " + this.TableName;
                        if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                        {
                            _NumberOfDeletedRows = _NumberOfRows;
                            this.progressBar.Value = this.progressBar.Maximum;
                            Application.DoEvents();
                            _NumberOfRows = 0;
                        }
                        else
                        {
                            SQL = "DELETE FROM " + this.TableName;
                            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                            {
                                _NumberOfDeletedRows = _NumberOfRows;
                                this.progressBar.Value = this.progressBar.Maximum;
                                Application.DoEvents();
                                _NumberOfRows = 0;
                            }
                        }

                    }

                    this.labelProgress.Text = "Starting replication";
                    Application.DoEvents();
                    this.progressBar.Visible = true;
                    this.labelProgress.Visible = false;
                    Application.DoEvents();

                    for (int i = 0; i < _NumberOfRows; i++)// int ReplicationRow R in this._ReplicationRows)
                    {
                        try
                        {
                            if (this.progressBar.Value < this.progressBar.Maximum) this.progressBar.Value++;

                            this._ReplicationRows[i].ReplicateDataRow();
                            if (this._ReplicationRows[i].RelationOfData == ReplicationRow.DataRelation.ForeignKeySameTableConflict)
                                ForeignKeySameTableConflictsFound = true;
                            switch (this._ReplicationRows[i].RelationOfData)
                            {
                                case ReplicationRow.DataRelation.Deleted:
                                    _NumberOfDeletedRows++;
                                    goto case ReplicationRow.DataRelation.Conflict;
                                case ReplicationRow.DataRelation.Conflict:
                                    if (!DiversityWorkbench.UserControls.ReplicationRow.IgnoreConflicts && this._ReplicationDirection != ReplicationDirection.Clean)
                                        _NumberOfConflicts++;
                                    break;
                                case ReplicationRow.DataRelation.Error:
                                    _NumberOfErrors++;
                                    break;
                                case ReplicationRow.DataRelation.Inserted:
                                    _NumberOfInsertedRows++;
                                    break;
                                case ReplicationRow.DataRelation.MissingKeyRelation:
                                    _NumberOfMissingRelations++;
                                    break;
                                case ReplicationRow.DataRelation.Updated:
                                    _NumberOfUpdatedRows++;
                                    break;
                                case ReplicationRow.DataRelation.NoDifference:
                                    _NumberOfNoDifference++;
                                    break;
                                case ReplicationRow.DataRelation.ForeignKeySameTableConflict:
                                    _NumberOfForeignKeySameTableConflictsFound++;
                                    break;
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                        try
                        {
                            if (i % 100 == 0)
                                System.Windows.Forms.Application.DoEvents();
                        }
                        catch (System.Exception ex)
                        { }
                    }
                    if (ForeignKeySameTableConflictsFound)
                    {
                        this.progressBar.Value = 0;
                        for (int i = 0; i < _NumberOfRows; i++)// int ReplicationRow R in this._ReplicationRows)
                        {
                            try
                            {
                                if (this.progressBar.Value < this.progressBar.Maximum) this.progressBar.Value++;
                                if (this._ReplicationRows[i].RelationOfData == ReplicationRow.DataRelation.ForeignKeySameTableConflict)
                                {
                                    if (this._ReplicationRows[i].SetForeignKeySameTableValues())
                                    {
                                        this._NumberOfForeignKeySameTableConflictsFound--;
                                        this._NumberOfInsertedRows++;
                                    }
                                    else
                                        this._NumberOfErrors++;
                                }
                            }
                            catch (System.Exception ex)
                            {
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            }
                        }
                    }

                    this.NumberOfConflicts = _NumberOfConflicts;
                    this.labelInserted.Text = "Inserted: " + _NumberOfInsertedRows.ToString();
                    if (_NumberOfInsertedRows == 0) this.labelInserted.ForeColor = System.Drawing.Color.Gray;
                    this.labelNumberOfErrors.Text = "Errors: " + (_NumberOfErrors + _NumberOfMissingRelations).ToString();
                    if (_NumberOfErrors == 0) this.labelNumberOfErrors.ForeColor = System.Drawing.Color.Gray;
                    this.labelUpdated.Text = "Updated: " + _NumberOfUpdatedRows.ToString();
                    if (_NumberOfUpdatedRows == 0) this.labelUpdated.ForeColor = System.Drawing.Color.Gray;

                    int rr = _NumberOfUpdatedRows + _NumberOfInsertedRows;

                    string Message = rr.ToString() + " row";
                    if (rr != 1) Message += "s";
                    Message += " transfered";

                    if (this._ReplicationDirection == ReplicationDirection.Clean)
                        Message = _NumberOfDeletedRows.ToString() + " rows deleted";

                    if (_NumberOfRows > 0)
                        this.Messages.Add(_NumberOfRows.ToString() + " datasets available");
                    if (_NumberOfNoDifference > 0)
                        this.Messages.Add(_NumberOfNoDifference.ToString() + " datasets showed no diffenence");
                    if (_NumberOfInsertedRows > 0)
                        this.Messages.Add(_NumberOfInsertedRows.ToString() + " datasets were inserted");
                    if (_NumberOfUpdatedRows > 0)
                        this.Messages.Add(_NumberOfUpdatedRows.ToString() + " datasets were updated");
                    if (_NumberOfErrors > 0)
                    {
                        this.Messages.Add(_NumberOfErrors.ToString() + " datasets producued errors");
                        foreach (DiversityWorkbench.UserControls.ReplicationRow R in this._ReplicationRows)
                        {
                            if (R.RelationOfData == ReplicationRow.DataRelation.Error && R.Message.Length > 0)
                                this.Messages.Add(R.Message);
                        }
                    }
                    if (_NumberOfConflicts > 0)
                    {
                        this.Messages.Add(_NumberOfConflicts.ToString() + " datasets producued conflicts");
                        foreach (DiversityWorkbench.UserControls.ReplicationRow R in this._ReplicationRows)
                        {
                            if (R.RelationOfData == ReplicationRow.DataRelation.Conflict && R.Message.Length > 0)
                                this.Messages.Add(R.Message);
                        }
                    }
                    if (_NumberOfMissingRelations > 0)
                    {
                        this.Messages.Add(_NumberOfMissingRelations.ToString() + " datasets missed a relation to other datasets");
                        foreach (DiversityWorkbench.UserControls.ReplicationRow R in this._ReplicationRows)
                        {
                            if (R.RelationOfData == ReplicationRow.DataRelation.MissingKeyRelation && R.Message.Length > 0)
                                this.Messages.Add(R.Message);
                        }
                    }


                    this.labelNumberOfDatasets.Text = Message;
                    this.labelNumberOfDatasets.ForeColor = System.Drawing.Color.Green;

                    this.labelUpdated.Visible = true;
                    this.labelConflict.Visible = true;
                    this.labelInserted.Visible = true;
                    this.labelNumberOfErrors.Visible = true;

                    this.checkBoxTableName.Checked = false;

                    if (_NumberOfErrors > 0 || _NumberOfMissingRelations > 0)
                    {
                        this.buttonInfo.Image = this.imageListState.Images[2];
                        this.buttonInfo.Tag = ReplicationRow.DataRelation.Error;
                        this.toolTip.SetToolTip(this.buttonInfo, _NumberOfErrors.ToString() + " errors found");
                    }

                    else if (_NumberOfConflicts == 0)
                    {
                        this.buttonInfo.Image = this.imageListState.Images[0];
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    OK = false;
                }
            }
            else
            {
                //this.setState(StateOfReplication.NotSelected);
            }
        }

        public void ClearLog()
        {
            if (this._ReplicationDirection != ReplicationDirection.Clean)
                return;
            string SQL = "IF (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = '" + this._TableName + "_log') = 1 " +
                " begin " +
                " TRUNCATE TABLE " + this._TableName + "_log " +
                " end";
            if (!DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
            {
                SQL = "IF (select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = '" + this._TableName + "_log') = 1 " +
                    " begin " +
                    " DELETE FROM " + this._TableName + "_log " +
                    " end";
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            }
        }

        public int DeletedData()
        {
            int DeletedRows = 0;
            //
            System.DateTime _DateLastReplication;
            string SQL = "";
            return DeletedRows;
        }

        public bool CleanTable()
        {
            bool OK = true;

            if (this.checkBoxTableName.Checked)
            {
                try
                {
                    this.ClearSubscriberTable();
                }
                catch (System.Exception ex)
                {
                    OK = false;
                }
            }
            else
            {
                OK = false;
            }
            return OK;
        }

        public bool TabelIsSelected
        {
            get { return this.checkBoxTableName.Checked; }
            set { this.checkBoxTableName.Checked = value; }
        }

        public int NumberOfDatasets
        {
            get
            {
                return getNumberOfDatasets();
            }
        }

        public int NumberOfConflicts
        {
            get
            { return _NumberOfConflicts; }
            set
            {
                _NumberOfConflicts = value;
                if (_NumberOfConflicts < 0)
                    _NumberOfConflicts = 0;
                this.labelConflict.Text = "Conflicts: " + value;
                if (_NumberOfConflicts == 0)
                    this.labelConflict.ForeColor = System.Drawing.Color.Gray;
                else if (_NumberOfConflicts > 0)
                {
                    this.buttonInfo.Image = this.imageListState.Images[1];
                    this.buttonInfo.Tag = ReplicationRow.DataRelation.Conflict;
                    this.toolTip.SetToolTip(this.buttonInfo, _NumberOfConflicts.ToString() + " conflicts found");
                }
                else
                {
                    this.buttonInfo.Image = this.imageListState.Images[0];
                }
            }
        }

        public System.Collections.Generic.List<string> Messages
        {
            get
            {
                if (this._Messages == null)
                    this._Messages = new List<string>();
                return _Messages;
            }
            //set { _Messages = value; }
        }

        #endregion

        #region Handling the rows

        /// <summary>
        /// for each row in the source table add a ReplicationRow to the list _ReplicationRows 
        /// for tables with internal relations start with the tops of the hierarchy
        /// </summary>
        private void fillRowsForReplication()
        {
            this._ReplicationRows = new List<ReplicationRow>();

            System.Collections.Generic.List<System.Data.DataRow> Rows = new List<DataRow>();
            try
            {
                foreach (System.Data.DataRow R in this.dtSource.Rows)
                {
                    bool IsTopRow = true;
                    foreach (string C in this.ChildParentColumns)
                    {
                        if (!R[C].Equals(System.DBNull.Value) &&
                        R[C].ToString() != R[this.ParentColumn].ToString())
                            IsTopRow = false;
                    }

                    //if (this.ChildParentColumn.Length == 0 ||
                    //    (R[this.ChildParentColumn].Equals(System.DBNull.Value) ||
                    //    R[this.ChildParentColumn].ToString() == R[this.ParentColumn].ToString()))
                    if (IsTopRow)
                    {
                        ReplicationRow RR = new ReplicationRow(
                            this._TableName, R, this._ReplicationDirection, this.IdentityColumn, this.PrimaryKeyColumns,
                            this.ForeignPrimaryKeyColumns, this.ForeignKeys, this.ColumnDictionary, this.DataColumns);//this._ConnectionStringPublisher, this._ConnectionStringSubscriber, 
                        this._ReplicationRows.Add(RR);
                    }
                    else
                        Rows.Add(R);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            if (Rows.Count > 0)
                this.fillRowsForReplication(Rows);
        }

        /// <summary>
        /// Adding the rows that had not been added so far
        /// </summary>
        /// <param name="RR">The rows that have to be added</param>
        private void fillRowsForReplication(System.Collections.Generic.List<System.Data.DataRow> RR)
        {
            System.Collections.Generic.List<System.Data.DataRow> Rows = new List<DataRow>();
            try
            {
                foreach (System.Data.DataRow R in RR)
                {
                    // if all keys of the childs for all child parent relations are available as parents
                    bool AllKeysPresent = true;
                    foreach (string C in this.ChildParentColumns)
                    {
                        bool? KeyPresent;// = false;
                        if (!R[C].Equals(System.DBNull.Value))
                        {
                            KeyPresent = false;
                            string ParentKey = R[C].ToString();
                            foreach (ReplicationRow RepRow in this._ReplicationRows)
                            {
                                if (RepRow.DataRow[this.ParentColumn].ToString() == ParentKey)
                                {
                                    KeyPresent = true;
                                    break;
                                }
                            }
                            if (KeyPresent != null && !(bool)KeyPresent)
                            {
                                AllKeysPresent = false;
                                break;
                            }
                        }
                    }
                    if (AllKeysPresent)
                    {
                        ReplicationRow Rnew = new ReplicationRow(
                                this._TableName, R, this._ReplicationDirection,
                                this.IdentityColumn, this.PrimaryKeyColumns,
                                this.ForeignPrimaryKeyColumns, this.ForeignKeys, this.ColumnDictionary, this.DataColumns);//, this._ConnectionStringSubscriber, this._ConnectionStringPublisher
                        Rnew.Message = "";
                        this._ReplicationRows.Add(Rnew);
                    }
                    else
                    {
                        Rows.Add(R);
                    }
                }
                if (Rows.Count > 0 && Rows.Count < RR.Count)
                    this.fillRowsForReplication(Rows);
                else if (Rows.Count > 0 && Rows.Count == RR.Count)
                {
                    foreach (System.Data.DataRow R in Rows)
                    {
                        ReplicationRow Rnew = new ReplicationRow(
                                this._TableName, R, this._ReplicationDirection,
                                this.IdentityColumn, this.PrimaryKeyColumns,
                                this.ForeignPrimaryKeyColumns, this.ForeignKeys, this.ColumnDictionary, this.DataColumns);//, this._ConnectionStringSubscriber, this._ConnectionStringPublisher
                        Rnew.RelationOfData = ReplicationRow.DataRelation.MissingKeyRelation;
                        Rnew.Message = "";
                        this._ReplicationRows.Add(Rnew);
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// For tables with an internal child-parent relation: insert parent datasets if these are missing
        /// </summary>
        private void InsertMissingParents()
        {
            if (!TableHasChildParentRelation)
                return;

            try
            {
                //System.Collections.Generic.Dictionary<string, string> ExistingParentChildValues = new Dictionary<string, string>();
                System.Collections.Generic.List<string> ExistingParentValues = new List<string>();
                System.Collections.Generic.List<string> NeededParentValues = new List<string>();
                // filling the basic data in the dictionary resp. the parent list
                foreach (System.Data.DataRow R in this.dtSource.Rows)
                {
                    //string Child = "";
                    string NeededParent = "";
                    string Parent = R[this.ParentColumn].ToString();
                    if (!ExistingParentValues.Contains(Parent))
                        ExistingParentValues.Add(Parent);
                    // for every possible child parent relation read the existing values into the lists
                    foreach (string C in this.ChildParentColumns)
                    {
                        if (!R[C].Equals(System.DBNull.Value))
                        {
                            NeededParent = R[C].ToString();
                            if (!NeededParentValues.Contains(NeededParent) && !ExistingParentValues.Contains(NeededParent))
                                NeededParentValues.Add(NeededParent);

                        }
                    }
                }
                this.InsertMissingParents(ref ExistingParentValues, ref NeededParentValues);
                //this.InsertMissingParents(ref ExistingParentChildValues, ref NeededParentValues);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void InsertMissingParents(
            ref System.Collections.Generic.List<string> ExistingParentValues,
            ref System.Collections.Generic.List<string> NeededParentValues)
        {
            try
            {
                if (ExistingParentValues.Count > 0)
                {
                    System.Collections.Generic.List<string> NeededParentValuesToAdd = new List<string>();
                    foreach (string P in NeededParentValues)
                    {
                        if (!ExistingParentValues.Contains(P))
                        {
                            System.Data.DataRow[] RRTest = this.dtSource.Select(this.ParentColumn + " = '" + P + "'");
                            if (RRTest.Length == 0)
                            {
                                string SQL = "SELECT " + this.AllColumns + " FROM [" + this._TableName + "]" +
                                    " WHERE " + this.ParentColumn + " = '" + P + "'";
                                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher);//, DiversityWorkbench.Settings.ConnectionString);
                                ad.Fill(this.dtSource);
                            }
                            System.Data.DataRow[] rr = this.dtSource.Select(this.ParentColumn + " = '" + P + "'");
                            foreach (string C in this.ChildParentColumns)
                            {
                                if (!rr[0][C].Equals(System.DBNull.Value))
                                {
                                    if (!NeededParentValuesToAdd.Contains(rr[0][C].ToString()))
                                        NeededParentValuesToAdd.Add(rr[0][C].ToString());
                                    //NeededParentValues.Add(rr[0][C].ToString());
                                }
                                else
                                {
                                }
                                if (!ExistingParentValues.Contains(P))
                                {
                                    //string Parent = rr[0][this.ParentColumn].ToString();
                                    ExistingParentValues.Add(P);
                                }
                            }
                        }
                        else
                        { }
                    }
                    foreach (string s in NeededParentValuesToAdd)
                    {
                        if (!NeededParentValues.Contains(s))
                            NeededParentValues.Add(s);
                    }
                    System.Collections.Generic.List<string> MissingParents = new List<string>();
                    foreach (string s in NeededParentValues)
                    {
                        if (!ExistingParentValues.Contains(s))
                            MissingParents.Add(s);
                    }
                    //foreach (string s in ExistingParentValues)
                    //{
                    //    if (!NeededParentValues.Contains(s))
                    //        MissingParents.Add(s);
                    //}
                    if (MissingParents.Count > 0)
                        this.InsertMissingParents(ref ExistingParentValues, ref NeededParentValues);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        //private void InsertMissingParents(
        //    ref System.Collections.Generic.Dictionary<string, string> ExistingParentChildValues,
        //    ref System.Collections.Generic.List<string> NeededParentValues)
        //{
        //    try
        //    {
        //        if (ExistingParentChildValues.Count > 0)
        //        {
        //            foreach (string P in NeededParentValues)
        //            {
        //                if (!ExistingParentChildValues.ContainsKey(P))
        //                {
        //                    //string Columns = "";
        //                    //foreach (System.Data.DataColumn C in this.dtSource.Columns)
        //                    //{
        //                    //    if (Columns.Length > 0) Columns += ", ";
        //                    //    Columns += C.ColumnName;
        //                    //}
        //                    string SQL = "SELECT " + this.AllColumns + " FROM [" + this._TableName + "]" +
        //                        " WHERE " + this.ParentColumn + " = '" + P + "'";
        //                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher);//, DiversityWorkbench.Settings.ConnectionString);
        //                    ad.Fill(this.dtSource);
        //                    //int i = dtSource.Rows.Count;
        //                    System.Data.DataRow[] rr = this.dtSource.Select(this.ParentColumn + " = '" + P + "'");
        //                    foreach (string C in this.ChildParentColumns)
        //                    {
        //                        if (!rr[0][C].Equals(System.DBNull.Value))
        //                        {
        //                            //if (!NeededParentValues.Contains(rr[0][this.ParentColumn].ToString()))
        //                            //    NeededParentValues.Add(rr[0][this.ParentColumn].ToString());
        //                            if (!NeededParentValues.Contains(rr[0][C].ToString()))
        //                                NeededParentValues.Add(rr[0][C].ToString());
        //                        }
        //                        else
        //                        {
        //                        }
        //                        if (!ExistingParentChildValues.ContainsKey(P))
        //                        {
        //                            string Parent = rr[0][this.ParentColumn].ToString();
        //                            ExistingParentChildValues.Add(P, Parent);
        //                        }
        //                    }

        //                    //if (rr[0][this.ChildParentColumn].Equals(System.DBNull.Value))
        //                    //{
        //                    //    if (!ParentValues.Contains(rr[0][this.ParentColumn].ToString()))
        //                    //        ParentValues.Add(rr[0][this.ParentColumn].ToString());
        //                    //}
        //                    //else
        //                    //{
        //                    //    if (!ParentChildValues.ContainsKey(P))
        //                    //    {
        //                    //        string Parent = rr[0][this.ParentColumn].ToString();
        //                    //        ParentChildValues.Add(P, Parent);
        //                    //    }
        //                    //}
        //                }
        //            }

        //            //foreach (System.Collections.Generic.KeyValuePair<string, string> KV in ParentChildValues)
        //            //{
        //            //    if (!ParentValues.Contains(KV.Value))
        //            //    {
        //            //        string SQL = "SELECT " + this.AllColumns + " FROM [" + this._TableName + "]" +
        //            //            " WHERE " + this.ParentColumn + " = '" + KV.Value + "'";
        //            //        //string SQL = "SELECT " + this.AllColumns + " FROM [" + this._TableName + "]" +
        //            //        //    " WHERE " + this.ChildParentColumn + " = '" + KV.Value + "'";
        //            //        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber);//, DiversityWorkbench.Settings.ConnectionString);
        //            //        ad.Fill(this.dtSource);

        //            //        System.Data.DataRow[] rr = this.dtSource.Select(this.ParentColumn + " = '" + KV.Value + "'");
        //            //        if (rr[0][this.ChildParentColumn].Equals(System.DBNull.Value))
        //            //        {
        //            //            if (!ParentValues.Contains(rr[0][this.ParentColumn].ToString()))
        //            //                ParentValues.Add(rr[0][this.ParentColumn].ToString());
        //            //        }
        //            //        else
        //            //        {
        //            //            if (!ParentChildValues.ContainsKey(KV.Value))
        //            //            {
        //            //                string Parent = rr[0][this.ParentColumn].ToString();
        //            //                ParentChildValues.Add(KV.Value, Parent);
        //            //            }
        //            //        }


        //            //        //System.Data.DataRow[] rr = this.dtSource.Select(this.ChildParentColumn + " = '" + KV.Value + "'");
        //            //        //if (rr[0][this.ParentColumn].Equals(System.DBNull.Value))
        //            //        //{
        //            //        //    if (!ParentValues.Contains(rr[0][this.ParentColumn].ToString()))
        //            //        //        ParentValues.Add(rr[0][this.ParentColumn].ToString());
        //            //        //}
        //            //        //else
        //            //        //{
        //            //        //    if (!ParentChildValues.ContainsKey(KV.Value))
        //            //        //    {
        //            //        //        string Parent = rr[0][this.ParentColumn].ToString();
        //            //        //        ParentChildValues.Add(KV.Value, Parent);
        //            //        //    }
        //            //        //}
        //            //    }
        //            //}
        //            System.Collections.Generic.List<string> MissingParents = new List<string>();
        //            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in ExistingParentChildValues)
        //            {
        //                if (!NeededParentValues.Contains(KV.Value))
        //                    MissingParents.Add(KV.Value);
        //            }
        //            if (MissingParents.Count > 0)
        //                this.InsertMissingParents(ref ExistingParentChildValues, ref NeededParentValues);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        private bool TableHasChildParentRelation
        {
            get
            {
                if (this.ChildParentColumns.Count > 0 && this.ParentColumn.Length > 0)
                    return true;
                else return false;
            }
        }

        public int getNumberOfDatasets()
        {
            if (this._NumberOfSourceDatasets != null)
                return (int)this._NumberOfSourceDatasets;
            else
            {
                string SQL = "SELECT COUNT(*) FROM [" + this._TableName + "]";
                if (this._SqlRestrictionColumn != null &&
                    this._SqlRestrictionIDs != null &&
                    this._SqlRestrictionColumn.Length > 0 &&
                    this._SqlRestrictionIDs.Length > 0 &&
                    this._ReplicationDirection != ReplicationDirection.Clean)
                    SQL += " WHERE " + this._SqlRestrictionColumn + " IN (" + this._SqlRestrictionIDs + ")";
                else if (this._SqlRestriction.Length > 0)
                    SQL += " WHERE " + this._SqlRestriction;
                if (this.Filter().Length > 0)
                {
                    if (SQL.IndexOf(" WHERE ") > -1) SQL += " AND ";
                    else SQL += " WHERE ";
                    SQL += this.Filter();
                }
                int i = 0;
                Microsoft.Data.SqlClient.SqlCommand C;// = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                if (this._ReplicationDirection == ReplicationDirection.Download)
                    C = new Microsoft.Data.SqlClient.SqlCommand(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher);
                else C = new Microsoft.Data.SqlClient.SqlCommand(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber);
                try
                {
                    int.TryParse(C.ExecuteScalar()?.ToString(), out i);
                    this._NumberOfSourceDatasets = i;
                    if (this._NumberOfSourceDatasets == 0 && this._ReplicationDirection == ReplicationDirection.Merge)
                    {
                        C = new Microsoft.Data.SqlClient.SqlCommand(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher);
                        if (int.TryParse(C.ExecuteScalar()?.ToString(), out i) && i > 0)
                            this._NumberOfSourceDatasets = i;
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    this._NumberOfSourceDatasets = 0;
                }
            }
            return (int)this._NumberOfSourceDatasets;
        }

        #endregion

        #region Clear table in subscriber

        private bool ClearSubscriberTable()
        {
            if (!this.checkBoxTableName.Checked)
                return false;
            bool OK = true;
            int iError = 0;
            string SqlDelete = "";
            try
            {
                //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlDelete, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber);//, con);
                string Message = "";
                string SQL = "SELECT COUNT(*) FROM [" + this._TableName + "]";
                int i;
                if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out i))
                {
                    if (i > 0)
                    {
                        this.progressBar.Value = 0;
                        this.progressBar.Maximum = i;
                        if (this.TableHasChildParentRelation)
                        {
                            Message = "\r\n\r\nErrors:\r\n";
                            this.fillRowsForReplication();
                            this.progressBar.Maximum = this._ReplicationRows.Count;
                            for (int rr = this._ReplicationRows.Count - 1; rr > -1; rr--)
                            {
                                this._ReplicationRows[rr].ReplicateDataRow();
                                if (this._ReplicationRows[rr].RelationOfData == ReplicationRow.DataRelation.Error)
                                {
                                    iError++;
                                    Message += "\r\n" + this._ReplicationRows[rr].Message;
                                    OK = false;
                                }
                                this.progressBar.Value = rr;
                            }
                        }
                        else
                        {
                            SqlDelete = "DELETE FROM [" + this._TableName + "]";
                            C.CommandText = SqlDelete;
                            try
                            {
                                //con.Open();
                                C.ExecuteNonQuery();
                            }
                            catch (System.Exception ex)
                            {
                                OK = false;
                                Message = ex.Message;
                            }
                            finally
                            {
                                //con.Close();
                                //con.Dispose();
                            }
                        }
                    }
                }
                SqlDelete = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + this._TableName + "_log]') AND type in (N'U')) " +
                    "DELETE FROM " + this._TableName + "_log";
                C.CommandText = SqlDelete;
                try
                {
                    //con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    //con.Open();
                    C = new Microsoft.Data.SqlClient.SqlCommand(SqlDelete, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber);//, con);
                }
                catch (System.Exception ex)
                {
                    Message = ex.Message;
                    OK = false;
                }
                finally
                {
                    //con.Close();
                    //con.Dispose();
                }
                if (OK)
                {
                    Message = i.ToString() + " datarow";
                    if (i != 1) Message += "s";
                    Message += " deleted";
                    this.toolTip.SetToolTip(this.buttonInfo, Message);
                    this.labelNumberOfDatasets.Text = Message;
                    this.labelNumberOfDatasets.ForeColor = System.Drawing.Color.Green;
                    this.buttonInfo.Image = this.imageListState.Images[0];
                    this.checkBoxTableName.Enabled = false;
                    this.checkBoxTableName.Checked = false;
                    this.progressBar.Value = this.progressBar.Maximum;
                }
                else
                {
                    this.buttonInfo.Image = this.imageListState.Images[2];
                    this.toolTip.SetToolTip(this.buttonInfo, Message + "Could not delete " + iError.ToString() + " datarows.\r\n");
                    this.labelNumberOfErrors.Text = "Errors: " + iError.ToString();
                    this.labelNumberOfErrors.Visible = true;
                }
                this.Messages.Add(Message);
            }
            catch (System.Exception ex) { OK = false; }
            return OK;
        }

        #endregion

        #region Updates and inserts
        /*
        Possible scenarios (simple version)
         * (1) RowGUID present
         *       -> Search foreign keys
         *       -> UPDATE excluding PK
         * (2) RowGUID missing
         *       -> INSERT, if PK relates to superior table first get values for PK
         *          For foreign keys get the corresponding values
         *      (3) Check parents in a Child-Parent relation       
        */

        private _DataRelation DataRelation(System.Data.DataRow R)
        {
            try
            {
                if (this._RelationOfData != _DataRelation.ToBeChecked)
                    return this._RelationOfData;
                string SQL = "";
                this._RowGUID = R["RowGUID"].ToString();
                string StateOfSource = _DataRelation.NoInformation.ToString();
                if (this.ColumnInsertDateInSource.Length > 0 && this.ColumnUpdateDateInSource.Length > 0)
                {
                    SQL = "SELECT CASE WHEN [" + this.ColumnInsertDateInSource + "] > [" + this.ColumnUpdateDateInSource + "] OR " +
                      "[" + this.ColumnInsertDateInSource + "] = [" + this.ColumnUpdateDateInSource + "] THEN '" + _DataRelation.SourceUnchanged.ToString() + "' ELSE '" +
                      _DataRelation.SourceUpdated.ToString() + "' END AS DataRelation " +
                      "FROM [" + this._TableName + "] AS T " +
                      "WHERE (RowGUID = '" + this._RowGUID + "')";
                    StateOfSource = this.SqlExecuteScalarInSource(SQL);
                }
                if (StateOfSource == _DataRelation.SourceUnchanged.ToString())
                    this._RelationOfData = _DataRelation.SourceUnchanged;
                else if (this.ColumnInsertDateInDestination.Length > 0 && this.ColumnUpdateDateInDestination.Length > 0)
                {
                    SQL = "SELECT CASE WHEN [" + this.ColumnInsertDateInDestination + "] > [" + this.ColumnUpdateDateInDestination + "] OR " +
                        "[" + this.ColumnInsertDateInDestination + "] = [" + this.ColumnUpdateDateInDestination + "] THEN '" + _DataRelation.SourceUnchanged.ToString() + "' ELSE '" +
                        _DataRelation.SourceUpdated.ToString() + "' END AS DataRelation " +
                        "FROM [" + this._TableName + "] AS T " +
                        "WHERE (RowGUID = '" + this._RowGUID + "')";
                    string StateOfDestination = this.SqlExecuteScalarInSource(SQL);
                    if (StateOfDestination == _DataRelation.SourceUnchanged.ToString())
                        this._RelationOfData = _DataRelation.SourceUpdated;
                    else this._RelationOfData = _DataRelation.SourceAndDestinationUpdated;
                }
                else
                    this._RelationOfData = _DataRelation.NoInformation;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return this._RelationOfData;
        }

        private System.Collections.Generic.Dictionary<string, string> GetForeignKeyValuesForNewDataset(System.Data.DataRow R, ref bool Keysfound, ref string Message)
        {
            Keysfound = true;
            System.Collections.Generic.Dictionary<string, string> PK = new Dictionary<string, string>();
            string SQL = "";
            try
            {
                System.Data.DataTable dtTables = new DataTable();
                //if (ForSqlCE)
                //    SQL = "SELECT UNIQUE_CONSTRAINT_TABLE_NAME FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS  K " +
                //        "WHERE (K.Constraint_TABLE_NAME = '" + this._TableName + "')";
                //else
                SQL = "SELECT DISTINCT C.TABLE_NAME " +
                    "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                    "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON K.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
                    "INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS C ON R.UNIQUE_CONSTRAINT_NAME = C.CONSTRAINT_NAME " +
                    "WHERE (K.TABLE_NAME = '" + this._TableName + "')";
                if (this.SqlFillTableFromDestination(SQL, ref dtTables))
                {
                    foreach (System.Data.DataRow Rtables in dtTables.Rows)
                    {
                        //if (ForSqlCE)
                        //    SQL = "SELECT KF.COLUMN_NAME AS TableColumn, KP.COLUMN_NAME AS RelatedColumn " +
                        //        "FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS  R " +
                        //        ", INFORMATION_SCHEMA.KEY_COLUMN_USAGE KF " +
                        //        ", INFORMATION_SCHEMA.KEY_COLUMN_USAGE KP " +
                        //        "WHERE R.CONSTRAINT_NAME = KF.CONSTRAINT_NAME " +
                        //        "AND KP.TABLE_NAME = R.UNIQUE_CONSTRAINT_TABLE_NAME " +
                        //        "AND KP.CONSTRAINT_NAME = R.UNIQUE_CONSTRAINT_NAME " +
                        //        "AND (R.CONSTRAINT_TABLE_NAME = '" + this._TableName + "') " +
                        //        "AND (R.UNIQUE_CONSTRAINT_TABLE_NAME = '" + Rtables[0].ToString() + "')";
                        //else
                        SQL = "SELECT DISTINCT K.COLUMN_NAME AS TableColumn, C.COLUMN_NAME AS RelatedColumn " +
                            "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kref INNER JOIN " +
                            "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                            "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON K.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
                            "INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS C ON R.UNIQUE_CONSTRAINT_NAME = C.CONSTRAINT_NAME ON  " +
                            "Kref.TABLE_NAME = C.TABLE_NAME AND Kref.ORDINAL_POSITION = K.ORDINAL_POSITION " +
                            "WHERE  Kref.CONSTRAINT_NAME = C.CONSTRAINT_NAME  " +
                            "AND C.COLUMN_NAME = Kref.COLUMN_NAME " +
                            "AND  (K.TABLE_NAME = '" + this._TableName + "') " +
                            "AND (C.TABLE_NAME = '" + Rtables[0].ToString() + "')";
                        System.Data.DataTable dtColumns = new DataTable();
                        if (this.SqlFillTableFromDestination(SQL, ref dtColumns))
                        {
                            SQL = "SELECT RowGUID " +
                                "FROM " + Rtables[0].ToString() + " AS T " +
                                "WHERE 1 = 1 ";
                            bool RelationIsPresent = true;
                            foreach (System.Data.DataRow Rcolumn in dtColumns.Rows)
                            {
                                if (R[Rcolumn[0].ToString()].Equals(System.DBNull.Value))
                                {
                                    RelationIsPresent = false;
                                    break;
                                }
                                SQL += " AND " + Rcolumn[1].ToString() + " = '" + R[Rcolumn[0].ToString()].ToString() + "'";
                            }
                            if (RelationIsPresent)
                            {
                                string RowGUID = this.SqlExecuteScalarInSource(SQL);
                                if (RowGUID.Length > 0)
                                {
                                    SQL = "";
                                    foreach (System.Data.DataRow Rcolumn in dtColumns.Rows)
                                    {
                                        if (SQL.Length > 0) SQL += ", ";
                                        SQL += " " + Rcolumn[1].ToString();
                                    }
                                    SQL = "SELECT " + SQL + " FROM " + Rtables[0].ToString() + " WHERE RowGUID = '" + RowGUID + "'";
                                    System.Data.DataTable dtPK = new DataTable();
                                    if (this.SqlFillTableFromDestination(SQL, ref dtPK))
                                    {
                                        if (dtPK.Rows.Count == 0)
                                        {
                                            SQL = "";
                                            foreach (System.Data.DataRow Rcolumn in dtColumns.Rows)
                                            {
                                                if (SQL.Length > 0) SQL += ", ";
                                                SQL += " " + Rcolumn[1].ToString();
                                            }
                                            SQL = "SELECT " + SQL + " FROM " + Rtables[0].ToString() + " WHERE  1 = 1 ";
                                            foreach (System.Data.DataRow Rcolumn in dtColumns.Rows)
                                            {
                                                SQL += " AND " + Rcolumn[1].ToString() + " = '" + R[Rcolumn[0].ToString()].ToString() + "'";
                                            }
                                            if (this.SqlFillTableFromDestination(SQL, ref dtPK))
                                            {
                                                if (dtPK.Rows.Count > 0)
                                                {
                                                    foreach (System.Data.DataRow Rcolumn in dtColumns.Rows)
                                                    {
                                                        if (!PK.ContainsKey(Rcolumn[0].ToString()))
                                                        {
                                                            string Column = Rcolumn[0].ToString();
                                                            string Value = dtPK.Rows[0][Rcolumn[1].ToString()].ToString();
                                                            PK.Add(Column, Value);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    Keysfound = false;
                                                    Message = SQL;
                                                    return PK;
                                                }
                                            }
                                            else
                                            {
                                                Keysfound = false;
                                                Message = SQL;
                                                return PK;
                                            }
                                        }
                                        else
                                        {
                                            foreach (System.Data.DataRow Rcolumn in dtColumns.Rows)
                                            {
                                                if (!PK.ContainsKey(Rcolumn[0].ToString()))
                                                {
                                                    string Column = Rcolumn[0].ToString();
                                                    string Value = dtPK.Rows[0][Rcolumn[0].ToString()].ToString();
                                                    PK.Add(Rcolumn[0].ToString(), dtPK.Rows[0][Rcolumn[0].ToString()].ToString());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //Keysfound = false;
                        }
                        else
                        {
                            Keysfound = false;
                            Message = SQL;
                            return PK;
                        }
                    }
                }
                else
                {
                    Keysfound = false;
                    Message = SQL;
                }
            }
            catch (System.Exception ex)
            {
                Keysfound = false;
                Message = SQL;
            }
            return PK;
        }

        #endregion

        #region Infos about the table

        private string AllColumns
        {
            get
            {
                string Columns = "";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ColumnDictionary)
                {
                    if (Columns.Length > 0) Columns += ", ";
                    Columns += KV.Key;
                }
                return Columns;
            }
        }

        private System.Collections.Generic.Dictionary<string, string> ColumnDictionary
        {
            get
            {
                if (this._ColumnDictionary != null &&
                    this._ColumnDictionary.Count > 0)
                    return this._ColumnDictionary;
                else
                {
                    this._ColumnDictionary = new Dictionary<string, string>();
                    System.Data.DataTable dtSubscriber = new DataTable();
                    string SQL = "SELECT C.COLUMN_NAME , C.DATA_TYPE " +
                    "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                    "WHERE TABLE_NAME = '" + this._TableName + "'";
                    try
                    {
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber);//, this._ConnectionStringSubscriber);
                        ad.Fill(dtSubscriber);
                        //ad.Dispose();
                        foreach (System.Data.DataRow R in dtSubscriber.Rows)
                        {
                            this._ColumnDictionary.Add(R[0].ToString(), R[1].ToString());
                        }

                        if (DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher.ConnectionString.Length > 0)// this._ConnectionStringPublisher.Length > 0)
                        {
                            // remove all columns that do not exist in the table of the publishing database
                            System.Data.DataTable dtCol = new DataTable();
                            Microsoft.Data.SqlClient.SqlDataAdapter adPublisher = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher);//, this._ConnectionStringPublisher);
                            adPublisher.Fill(dtCol);
                            //adPublisher.Dispose();
                            System.Collections.Generic.List<string> ColumnsToRemove = new List<string>();
                            foreach (string s in this._ColumnDictionary.Keys)
                            {
                                System.Data.DataRow[] rr = dtCol.Select("COLUMN_NAME = '" + s + "'");
                                if (rr.Length == 0)
                                    ColumnsToRemove.Add(s);
                            }
                            if (ColumnsToRemove.Count > 0)
                            {
                                foreach (string s in ColumnsToRemove)
                                    this._ColumnDictionary.Remove(s);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                return this._ColumnDictionary;
            }
        }

        private string IdentityColumn
        {
            get
            {
                if (this._IdentityColumn != null) return this._IdentityColumn;
                else
                {
                    this._IdentityColumn = "";
                    string SqlIdentiy = "select c.name from sys.columns c, sys.tables t where c.is_identity = 1 " +
                        "and c.object_id = t.object_id and t.name = '" + this._TableName + "'";
                    try
                    {
                        //if (!this._SourceIsCE)
                        //{
                        //    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this._ConnectionStringSource);
                        //    Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SqlIdentiy, con);
                        //    con.Open();
                        //    this._IdentityColumn = Com.ExecuteScalar().ToString();
                        //    con.Close();
                        //}
                        //else if (this._SourceIsCE && !this._DestinationIsCE)
                        {
                            //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this._ConnectionStringSubscriber);
                            Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SqlIdentiy, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber);//, con);
                            //con.Open();
                            this._IdentityColumn = Com.ExecuteScalar().ToString();
                            //con.Close();
                            //con.Dispose();
                        }
                    }
                    catch { }
                    return this._IdentityColumn;
                }
            }
        }

        private System.Collections.Generic.List<string> PrimaryKeyColumns
        {
            get
            {
                if (this._PrimaryKeyColumns != null) return this._PrimaryKeyColumns;
                else
                {
                    this._PrimaryKeyColumns = new List<string>();
                    string SqlPK = "SELECT COLUMN_NAME " +
                        "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                        "WHERE (TABLE_NAME = '" + this._TableName + "') AND (EXISTS " +
                        "(SELECT * " +
                        "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                        "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS T ON K.CONSTRAINT_NAME = T.CONSTRAINT_NAME " +
                        "WHERE (T.CONSTRAINT_TYPE = 'PRIMARY KEY') AND (K.COLUMN_NAME = C.COLUMN_NAME) AND (K.TABLE_NAME = C.TABLE_NAME)))";
                    try
                    {
                        System.Data.DataTable dtPK = new DataTable();
                        {
                            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlPK, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher);//, this._ConnectionStringPublisher);
                            ad.Fill(dtPK);
                        }
                        foreach (System.Data.DataRow R in dtPK.Rows)
                            this._PrimaryKeyColumns.Add(R[0].ToString());
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                    return this._PrimaryKeyColumns;
                }
            }
        }

        /// <summary>
        /// Columns that relate to another table
        /// </summary>
        public System.Collections.Generic.List<DiversityWorkbench.UserControls.ForeignKey> ForeignKeys
        {
            get
            {
                if (this._ForeignKeys == null)
                {
                    this._ForeignKeys = new List<DiversityWorkbench.UserControls.ForeignKey>();
                    string SQL = "SELECT DISTINCT K.COLUMN_NAME AS ColumnName, C.TABLE_NAME AS TableName " +
                        "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                        "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON K.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
                        "INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS C ON R.UNIQUE_CONSTRAINT_NAME = C.CONSTRAINT_NAME " +
                        "WHERE K.TABLE_NAME = '" + this._TableName + "'";

                    //string SQL = "SELECT COLUMN_NAME " +
                    //   "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                    //   "WHERE (TABLE_NAME = '" + this._TableName + "') AND (EXISTS " +
                    //   "(SELECT * " +
                    //   "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                    //   "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS T ON K.CONSTRAINT_NAME = T.CONSTRAINT_NAME " +
                    //   "WHERE (T.CONSTRAINT_TYPE = 'FOREIGN KEY') AND (K.COLUMN_NAME = C.COLUMN_NAME) AND (K.TABLE_NAME = C.TABLE_NAME)));";
                    try
                    {
                        System.Data.DataTable dt = new DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher);//, this._ConnectionStringPublisher);
                        ad.Fill(dt);
                        //ad.Dispose();
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            if (!this._ForeignPrimaryKeyColumns.Contains(R[0].ToString()))
                                this._ForeignPrimaryKeyColumns.Add(R[0].ToString());
                            DiversityWorkbench.UserControls.ForeignKey F = new ForeignKey(this._TableName, R[0].ToString(), R[1].ToString(), this.ConnectionSource, this.ConnectionDestination);
                            //F.ColumnName = R[0].ToString();
                            //F.ForeignTable = R[1].ToString();
                            this._ForeignKeys.Add(F);
                        }
                        int NumberOfForeignKeys = this._ForeignKeys.Count;
                        for (int i = 0; i < NumberOfForeignKeys; i++)
                        {
                            SQL = "SELECT MIN(C.COLUMN_NAME) AS RelatedColumn " +
                                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kref  " +
                                "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K " +
                                "INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON K.CONSTRAINT_NAME = R.CONSTRAINT_NAME " +
                                "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS C ON R.UNIQUE_CONSTRAINT_NAME = C.CONSTRAINT_NAME ON  " +
                                "Kref.TABLE_NAME = C.TABLE_NAME AND Kref.ORDINAL_POSITION = K.ORDINAL_POSITION " +
                                "WHERE  Kref.CONSTRAINT_NAME = C.CONSTRAINT_NAME  AND C.COLUMN_NAME = Kref.COLUMN_NAME " +
                                "AND  (K.TABLE_NAME = '" + this._TableName + "') " +
                                "AND K.COLUMN_NAME = '" + this._ForeignKeys[i].ColumnName + "'";
                            //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this._ConnectionStringPublisher);
                            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher);//, con);
                            //con.Open();
                            string testF = C.ExecuteScalar()?.ToString() ?? string.Empty;
                            if (testF != string.Empty)
                            {
                                this._ForeignKeys[i].ForeignColumnName = testF;
                            }
                            //con.Close();
                            //con.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                return _ForeignKeys;
            }
        }

        private System.Collections.Generic.List<string> DataColumns
        {
            get
            {
                if (this._DataColumns != null) return this._DataColumns;
                else
                {
                    this._DataColumns = new List<string>();
                    string SqlPK = "SELECT COLUMN_NAME " +
                        "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                        "WHERE (TABLE_NAME = '" + this._TableName + "') AND (NOT EXISTS " +
                        "(SELECT * " +
                        "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                        "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS T ON K.CONSTRAINT_NAME = T.CONSTRAINT_NAME " +
                        "WHERE (T.CONSTRAINT_TYPE = 'PRIMARY KEY') AND (K.COLUMN_NAME = C.COLUMN_NAME) AND (K.TABLE_NAME = C.TABLE_NAME)))";
                    try
                    {
                        System.Data.DataTable dtCol = new DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlPK, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher);//, this._ConnectionStringPublisher);
                        ad.Fill(dtCol);
                        System.Data.DataTable dtSubscriber = new DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter adSubscriber = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlPK, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber);//, this._ConnectionStringPublisher);
                        adSubscriber.Fill(dtSubscriber);
                        foreach (System.Data.DataRow R in dtCol.Rows)
                        {
                            System.Data.DataRow[] rr = dtSubscriber.Select("COLUMN_NAME = '" + R[0].ToString() + "'");
                            if (rr.Length > 0)
                                this._DataColumns.Add(R[0].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                    return this._DataColumns;
                }
            }
        }

        private System.Collections.Generic.List<string> _Columns;
        private System.Collections.Generic.List<string> Columns
        {
            get
            {
                if (this._Columns != null) return this._Columns;
                else
                {
                    this._Columns = new List<string>();
                    string SqlPK = "SELECT COLUMN_NAME " +
                        "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                        "WHERE (TABLE_NAME = '" + this._TableName + "')";
                    try
                    {
                        System.Data.DataTable dtCol = new DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlPK, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher);//, this._ConnectionStringPublisher);
                        ad.Fill(dtCol);
                        System.Data.DataTable dtSubscriber = new DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter adSubscriber = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlPK, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber);//, this._ConnectionStringPublisher);
                        adSubscriber.Fill(dtSubscriber);
                        foreach (System.Data.DataRow R in dtCol.Rows)
                        {
                            System.Data.DataRow[] rr = dtSubscriber.Select("COLUMN_NAME = '" + R[0].ToString() + "'");
                            if (rr.Length > 0)
                                this._Columns.Add(R[0].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                    return this._Columns;
                }
            }
        }

        private bool CheckPresenceOfDataInDestination(System.Data.DataRow R, ref bool RowGUIDIsPresent, ref bool PrimaryKeyIsPresent, ref bool PrimaryKeyMatchesRowGUID)
        {
            try
            {
                string SqlCount = "SELECT COUNT(*) FROM [" + this._TableName + "] WHERE RowGUID = '" + R["RowGUID"].ToString() + "'";
                RowGUIDIsPresent = false;
                int i;
                if (int.TryParse(this.SqlExecuteScalarInDestination(SqlCount), out i) && i == 1)
                    RowGUIDIsPresent = true;
                SqlCount = "SELECT COUNT(*) FROM [" + this._TableName + "] WHERE 1 = 1 ";
                if (this.PrimaryKeyColumns.Count > 0)
                {
                    foreach (string s in this.PrimaryKeyColumns)
                        SqlCount += " AND " + s + " = '" + R[s].ToString() + "'";
                }
                PrimaryKeyIsPresent = false;
                if (int.TryParse(this.SqlExecuteScalarInDestination(SqlCount), out i) && i == 1)
                    PrimaryKeyIsPresent = true;
                SqlCount += " AND RowGUID = '" + R["RowGUID"].ToString() + "'";
                PrimaryKeyMatchesRowGUID = false;
                if (RowGUIDIsPresent && PrimaryKeyIsPresent)
                {
                    if (int.TryParse(this.SqlExecuteScalarInDestination(SqlCount), out i) && i == 1)
                        PrimaryKeyMatchesRowGUID = true;
                }
                return true;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return false;
            }
        }

        private bool CheckPresenceOfDataInDestination(System.Data.DataRow R)
        {
            try
            {
                string SqlCount = "SELECT COUNT(*) FROM [" + this._TableName + "] WHERE RowGUID = '" + R["RowGUID"].ToString() + "'";
                this._RowGUIDisPresent = false;
                int i;
                if (int.TryParse(this.SqlExecuteScalarInDestination(SqlCount), out i) && i == 1)
                    this._RowGUIDisPresent = true;
                SqlCount = "SELECT COUNT(*) FROM [" + this._TableName + "] WHERE 1 = 1 ";
                if (this.PrimaryKeyColumns.Count > 0)
                {
                    foreach (string s in this.PrimaryKeyColumns)
                        SqlCount += " AND " + s + " = '" + R[s].ToString() + "'";
                }
                this._PKisPresent = false;
                if (int.TryParse(this.SqlExecuteScalarInDestination(SqlCount), out i) && i == 1)
                    this._PKisPresent = true;
                SqlCount += " AND RowGUID = '" + R["RowGUID"].ToString() + "'";
                this._PKMatchesRowGUID = false;
                if (this._RowGUIDisPresent && this._PKisPresent)
                {
                    if (int.TryParse(this.SqlExecuteScalarInDestination(SqlCount), out i) && i == 1)
                        this._PKMatchesRowGUID = true;
                }
                return true;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return false;
            }
        }

        /// <summary>
        /// Parts of the primary key, that relate to another table
        /// </summary>
        private System.Collections.Generic.List<string> ForeignPrimaryKeyColumns
        {
            get
            {
                if (this._ForeignPrimaryKeyColumns != null) return this._ForeignPrimaryKeyColumns;
                else
                {
                    this._ForeignPrimaryKeyColumns = new List<string>();
                    string SqlPK = "SELECT COLUMN_NAME " +
                        "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                        "WHERE (TABLE_NAME = '" + this._TableName + "') AND (EXISTS " +
                        "(SELECT * " +
                        "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                        "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS T ON K.CONSTRAINT_NAME = T.CONSTRAINT_NAME " +
                        "WHERE (T.CONSTRAINT_TYPE = 'PRIMARY KEY') AND (K.COLUMN_NAME = C.COLUMN_NAME) AND (K.TABLE_NAME = C.TABLE_NAME))) " +
                        "AND (EXISTS " +
                        "(SELECT * " +
                        "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                        "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS T ON K.CONSTRAINT_NAME = T.CONSTRAINT_NAME " +
                        "WHERE (T.CONSTRAINT_TYPE = 'FOREIGN KEY') AND (K.COLUMN_NAME = C.COLUMN_NAME) AND (K.TABLE_NAME = C.TABLE_NAME)));";
                    try
                    {
                        System.Data.DataTable dtPK = new DataTable();
                        //if (this._SourceIsCE)
                        //{
                        //    System.Data.SqlServerCe.SqlCeDataAdapter adCE = new System.Data.SqlServerCe.SqlCeDataAdapter(SqlPK, this._ConnectionStringSource);
                        //    adCE.Fill(dtPK);
                        //}
                        //else
                        {
                            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlPK, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher);//, this._ConnectionStringPublisher);
                            ad.Fill(dtPK);
                            //ad.Dispose();
                        }
                        foreach (System.Data.DataRow R in dtPK.Rows)
                            this._ForeignPrimaryKeyColumns.Add(R[0].ToString());
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                    return this._ForeignPrimaryKeyColumns;
                }
            }
        }

        private System.Collections.Generic.List<string> _ChildParentColumns;
        private System.Collections.Generic.List<string> ChildParentColumns
        {
            get
            {
                if (this._ChildParentColumns != null) return this._ChildParentColumns;
                else
                {
                    this._ChildParentColumns = new List<string>();
                    string SQL = "SELECT Kc.COLUMN_NAME AS ChildColumn " +
                        "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kc INNER JOIN " +
                        "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON Kc.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
                        "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kp ON R.UNIQUE_CONSTRAINT_NAME = Kp.CONSTRAINT_NAME " +
                        "WHERE (Kc.TABLE_NAME = '" + this._TableName + "') AND (Kp.TABLE_NAME = '" + this._TableName + "')" +
                        "AND  Kc.COLUMN_NAME <> Kp.COLUMN_NAME AND Kc.ORDINAL_POSITION = Kp.ORDINAL_POSITION";
                    System.Data.DataTable dt = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    foreach (System.Data.DataRow R in dt.Rows)
                        this._ChildParentColumns.Add(R[0].ToString());
                    return this._ChildParentColumns;
                }
            }
        }

        //private string ChildParentColumn
        //{
        //    get
        //    {
        //        if (this._ChildParentColumn != null) return this._ChildParentColumn;
        //        else
        //        {
        //            this._ChildParentColumn = "";
        //            string SQL = "SELECT Kc.COLUMN_NAME AS ChildColumn " +
        //                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kc INNER JOIN " +
        //                "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON Kc.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
        //                "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kp ON R.UNIQUE_CONSTRAINT_NAME = Kp.CONSTRAINT_NAME " +
        //                "WHERE (Kc.TABLE_NAME = '" + this._TableName + "') AND (Kp.TABLE_NAME = '" + this._TableName + "')" +
        //                "AND  Kc.COLUMN_NAME <> Kp.COLUMN_NAME AND Kc.ORDINAL_POSITION = Kp.ORDINAL_POSITION";
        //            this._ChildParentColumn = this.SqlExecuteScalarInSource(SQL).ToString();
        //            return this._ChildParentColumn;
        //        }
        //    }
        //}

        /// <summary>
        /// The parent column for the child parent relation, in most cases an identity or a unique ID and a possible primary key
        /// </summary>
        private string ParentColumn
        {
            get
            {
                if (this._ParentColumn != null) return this._ParentColumn;
                else
                {
                    this._ParentColumn = "";
                    string SQL = "SELECT DISTINCT Kp.COLUMN_NAME AS ChildColumn " +
                        "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kc INNER JOIN " +
                        "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON Kc.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
                        "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kp ON R.UNIQUE_CONSTRAINT_NAME = Kp.CONSTRAINT_NAME " +
                        "WHERE (Kc.TABLE_NAME = '" + this._TableName + "') AND (Kp.TABLE_NAME = '" + this._TableName + "')" +
                        "AND  Kc.COLUMN_NAME <> Kp.COLUMN_NAME AND Kc.ORDINAL_POSITION = Kp.ORDINAL_POSITION";
                    //string SQL = "SELECT Kp.COLUMN_NAME AS ParentColumn " + 
                    //    "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kc INNER JOIN " +
                    //    "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON Kc.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
                    //    "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kp ON R.UNIQUE_CONSTRAINT_NAME = Kp.CONSTRAINT_NAME " +
                    //    "WHERE (Kc.TABLE_NAME = '" + this._TableName + "') AND (Kp.TABLE_NAME = '" + this._TableName + "')";
                    this._ParentColumn = this.SqlExecuteScalarInSource(SQL);
                    return this._ParentColumn;
                }
            }
        }

        private string ColumnUpdateDateInSource
        {
            get
            {
                if (this._ColumnUpdateDateInSource.Length > 0) return this._ColumnUpdateDateInSource;
                if (this.ColumnDictionary.ContainsKey("LogUpdatedWhen")) this._ColumnUpdateDateInSource = "LogUpdatedWhen";
                return this._ColumnUpdateDateInSource;
            }
        }

        private string ColumnInsertDateInSource
        {
            get
            {
                if (this._ColumnInsertDateInSource.Length > 0) return this._ColumnInsertDateInSource;
                if (this.ColumnDictionary.ContainsKey("LogInsertedWhen")) this._ColumnInsertDateInSource = "LogInsertedWhen";
                else if (this.ColumnDictionary.ContainsKey("LogCreatedWhen")) this._ColumnInsertDateInSource = "LogCreatedWhen";
                else
                {
                    this._ColumnInsertDateInSource = this.ColumnInsertDate(true);
                }
                //if (this._ColumnInsertDateInSource.Length == 0) this._ColumnInsertDateInSource = "LogUpdatedWhen";
                return this._ColumnInsertDateInSource;
            }
        }

        private string ColumnUpdateDateInDestination
        {
            get
            {
                if (this._ColumnUpdateDateInDestination.Length > 0) return this._ColumnUpdateDateInDestination;
                if (this.ColumnDictionary.ContainsKey("LogUpdatedWhen")) this._ColumnUpdateDateInDestination = "LogUpdatedWhen";
                return this._ColumnUpdateDateInDestination;
            }
        }

        private string ColumnInsertDateInDestination
        {
            get
            {
                if (this._ColumnInsertDateInDestination.Length > 0) return this._ColumnInsertDateInDestination;
                if (this.ColumnDictionary.ContainsKey("LogInsertedWhen")) this._ColumnInsertDateInDestination = "LogInsertedWhen";
                else if (this.ColumnDictionary.ContainsKey("LogCreatedWhen")) this._ColumnInsertDateInDestination = "LogCreatedWhen";
                else
                {
                    try
                    {
                        this._ColumnInsertDateInDestination = this.ColumnInsertDate(false);
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                //if (this._ColumnInsertDateInDestination.Length == 0) this._ColumnInsertDateInDestination = "LogUpdatedWhen";
                return this._ColumnInsertDateInDestination;
            }
        }

        private string ColumnInsertDate(bool InSource)
        {
            string Column = "";
            string SQL = "SELECT COLUMN_NAME " +
                "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                "WHERE TABLE_NAME = '" + this._TableName + "' AND (COLUMN_NAME IN ('LogCreatedWhen', 'LogInsertedWhen'))";
            try
            {
                if (InSource)
                    Column = this.SqlExecuteScalarInSource(SQL);
                else
                    Column = this.SqlExecuteScalarInDestination(SQL);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Column;
        }

        #endregion

        private System.Data.DataTable dtSource
        {
            get
            {
                if (this._dtSource != null
                    && this._dtSource.Rows.Count > 0)
                    return this._dtSource;
                else
                {
                    this._dtSource = new DataTable(this._TableName);
                    string SQL = "";
                    string SqlColumns = "";
                    try
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ColumnDictionary)
                        {
                            if (SqlColumns.Length > 0)
                                SqlColumns += ", ";
                            if (KV.Value.ToLower() == "datetime" ||
                                KV.Value.ToLower() == "datetime2" ||
                                KV.Value.ToLower() == "smalldatetime")
                                SqlColumns += "CONVERT(varchar(23), " + KV.Key + ", 121) AS " + KV.Key;
                            else if (KV.Value.ToLower() == "geography" ||
                                KV.Value.ToLower() == "geometry")
                            {
                                SqlColumns += KV.Key + ".ToString() AS " + KV.Key;
                            }
                            else
                                SqlColumns += KV.Key;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "Reading the columns");
                    }

                    try
                    {
                        SQL = "SELECT " + SqlColumns + " FROM [" + this._TableName + "] ";
                        if (this._SqlRestrictionColumn != null &&
                            this._SqlRestrictionColumn.Length > 0 &&
                            this._SqlRestrictionIDs.Length > 0 &&
                            this._ReplicationDirection != ReplicationDirection.Clean)
                            SQL += " WHERE " + this._SqlRestrictionColumn + " IN (" + this._SqlRestrictionIDs + ")";
                        else if (this._SqlRestriction.Length > 0)
                            SQL += " WHERE " + this._SqlRestriction;
                        if (this.Filter().Length > 0)
                        {
                            if (SQL.IndexOf(" WHERE ") > -1) SQL += " AND ";
                            else SQL += " WHERE ";
                            SQL += this.Filter();
                        }

                        if (this.ChildParentColumns.Count > 0)
                        {
                            SQL += " ORDER BY ";
                            for (int i = 0; i < this.ChildParentColumns.Count; i++)
                            {
                                if (i > 0)
                                    SQL += ", ";
                                SQL += this.ChildParentColumns[i];
                            }
                        }

                        try
                        {
                            try
                            {
                                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ConnectionSource);//, this.ConnectionStringSource);
                                ad.Fill(this._dtSource);
                            }
                            catch (System.Exception ex)
                            {
                                this.labelProgress.Text = ex.Message;
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "Reading the source: " + SQL);
                            }
                            if (this._ReplicationDirection == ReplicationDirection.Merge)
                            {
                                System.Data.DataTable dtDestination = new DataTable();
                                try
                                {
                                    Microsoft.Data.SqlClient.SqlDataAdapter adDestination = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ConnectionDestination);
                                    adDestination.Fill(dtDestination);
                                }
                                catch (System.Exception ex)
                                {
                                    this.labelProgress.Text = ex.Message;
                                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "Reading the destination: " + SQL);
                                }
                                foreach (System.Data.DataRow R in dtDestination.Rows)
                                {
                                    try
                                    {
                                        System.Data.DataRow[] rr = this._dtSource.Select("RowGUID = '" + R["RowGUID"].ToString() + "'");
                                        if (rr.Length == 0)
                                        {
                                            System.Data.DataRow Rnew = this._dtSource.NewRow();
                                            foreach (System.Data.DataColumn C in this._dtSource.Columns)
                                                Rnew[C.ColumnName] = R[C.ColumnName];
                                            this._dtSource.Rows.Add(Rnew);
                                        }
                                    }
                                    catch (System.Exception ex)
                                    {
                                        this.labelProgress.Text = ex.Message;
                                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "Merging a single line");
                                    }
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            this.labelProgress.Text = ex.Message;
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "Merging");
                        }
                    }
                    catch (System.Exception ex)
                    {
                        this.labelProgress.Text = ex.Message;
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "Getting the source");
                    }
                    return this._dtSource;
                }
            }
        }

        #region SQL commands

        private Microsoft.Data.SqlClient.SqlConnection ConnectionDestination
        {
            get
            {
                if (this._ReplicationDirection == ReplicationDirection.Upload || this._ReplicationDirection == ReplicationDirection.Merge)
                    return DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher;
                else return DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber;
            }
        }

        private Microsoft.Data.SqlClient.SqlConnection ConnectionSource
        {
            get
            {
                if (this._ReplicationDirection == ReplicationDirection.Download)
                    return DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher;
                else return DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber;
            }
        }

        private bool SqlFillTableFromDestination(string SQL, ref System.Data.DataTable DT)
        {
            bool OK = true;
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ConnectionDestination);
                ad.Fill(DT);
                //ad.Dispose();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        private bool SqlFillTableFromSource(string SQL, ref System.Data.DataTable DT)
        {
            bool OK = true;
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ConnectionSource);
                ad.Fill(DT);
                //ad.Dispose();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        private string SqlExecuteScalarInDestination(string SQL)
        {
            string Result = "";
            //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this.ConnectionDestination);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.ConnectionDestination);// con);
            //con.Open();
            Result = C.ExecuteScalar()?.ToString() ?? string.Empty;
            //con.Close();
            //con.Dispose();
            return Result;
        }

        private bool SqlExecuteNonQueryInDestination(string SQL)
        {
            bool OK = true;
            try
            {
                //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this.ConnectionStringDestination);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.ConnectionDestination);//, con);
                //con.Open();
                C.ExecuteNonQuery();
                //con.Close();
                //con.Dispose();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        private string SqlExecuteScalarInSource(string SQL)
        {
            string Result = "";
            try
            {
                //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this.ConnectionStringSource);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.ConnectionSource);//, con);
                //con.Open();
                Result = C.ExecuteScalar()?.ToString() ?? string.Empty;
                //con.Close();
                //con.Dispose();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            return Result;
        }

        private bool SqlExecuteNonQueryInSource(string SQL)
        {
            bool OK = true;
            try
            {
                //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this.ConnectionStringSource);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.ConnectionSource);//, con);
                //con.Open();
                C.ExecuteNonQuery();
                //con.Close();
                //con.Dispose();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        #endregion

        #region Infos

        private void buttonInfo_Click(object sender, EventArgs e)
        {
            if (buttonInfo.Tag != null)
            {
                string State = buttonInfo.Tag.ToString();
                switch (State)
                {
                    case "Error":
                        string Message = this.labelNumberOfErrors.Text + ":\r\n\r\n";
                        foreach (DiversityWorkbench.UserControls.ReplicationRow R in this._ReplicationRows)
                        {
                            if (R.RelationOfData == ReplicationRow.DataRelation.Error || R.RelationOfData == ReplicationRow.DataRelation.MissingKeyRelation)
                            {
                                Message += R.Message;
                                if (R.Message.Trim().Length == 0)
                                {
                                    if (R.RelationOfData == ReplicationRow.DataRelation.MissingKeyRelation)
                                        Message += "Related key was missing\r\n";
                                }
                            }
                        }
                        DiversityWorkbench.Forms.FormEditText fError = new DiversityWorkbench.Forms.FormEditText(State, Message, true);
                        Bitmap bitmap = new Bitmap(this.buttonInfo.Image);
                        bitmap.SetResolution(16, 16);
                        Icon icon = System.Drawing.Icon.FromHandle(bitmap.GetHicon());
                        fError.Icon = icon;
                        fError.ShowDialog();
                        //System.Windows.Forms.MessageBox.Show(Message);
                        break;
                    case "Conflict":
                        DiversityWorkbench.Forms.FormReplicationConflict.NumberOfConflicts = this._NumberOfConflicts;
                        foreach (DiversityWorkbench.UserControls.ReplicationRow R in this._ReplicationRows)
                        {
                            if (R.RelationOfData == ReplicationRow.DataRelation.Conflict ||
                                R.RelationOfData == ReplicationRow.DataRelation.Deleted)
                            {
                                string SQL = "";
                                foreach (string s in this._PrimaryKeyColumns)
                                {
                                    if (SQL.Length > 0) SQL += ", ";
                                    SQL += s;
                                }
                                foreach (string s in this._DataColumns)
                                {
                                    if (SQL.Length > 0) SQL += ", ";
                                    SQL += s;
                                }
                                SQL = "SELECT " + SQL + " FROM " + this._TableName + " WHERE RowGUID = '" + R.RowGUID + "'";
                                System.Data.DataTable dtSubscriber = new DataTable(this._TableName);
                                System.Data.DataTable dtPublisher = new DataTable(this._TableName);
                                Microsoft.Data.SqlClient.SqlDataAdapter adSubscriber = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber);//, this._ConnectionStringSubscriber);
                                adSubscriber.Fill(dtSubscriber);


                                //adSubscriber.Dispose();
                                Microsoft.Data.SqlClient.SqlDataAdapter adPublisher = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher);//, this._ConnectionStringPublisher);
                                adPublisher.Fill(dtPublisher);

                                bool KeysFound = false;
                                string MessageKeys = "";
                                System.Collections.Generic.Dictionary<string, string> Keys = R.GetForeignKeyValuesForNewDataset(ref KeysFound, ref MessageKeys);
                                if (KeysFound && dtPublisher.Rows.Count == 1)
                                {
                                    foreach (System.Data.DataColumn C in dtPublisher.Columns)
                                    {
                                        if (Keys.ContainsKey(C.ColumnName))
                                            dtPublisher.Rows[0][C.ColumnName] = Keys[C.ColumnName];
                                    }
                                }

                                //adPublisher.Dispose();
                                DiversityWorkbench.Forms.FormReplicationConflict f =
                                    new DiversityWorkbench.Forms.FormReplicationConflict(
                                        this.PrimaryKeyColumns, this.ForeignKeys,
                                        this._ReplicationDirection,
                                        dtPublisher, dtSubscriber);
                                f.StartPosition = FormStartPosition.CenterParent;
                                f.ShowDialog();
                                if (f.DialogResult == DialogResult.OK || f.DialogResult == DialogResult.Yes)
                                {

                                    //neuer Code
                                    switch (f.ResolutionOfConflict)
                                    {
                                        //case DiversityWorkbench.Forms.FormReplicationConflict.ConflictResolution.Merge:
                                        //    break;
                                        case DiversityWorkbench.Forms.FormReplicationConflict.ConflictResolution.DeletePublisher:
                                            if (this._ReplicationDirection == ReplicationDirection.Download
                                               && (R.StateOfSubscriberRow == ReplicationRow.DataRowState.Deleted
                                               || R.StateOfSubscriberRow == ReplicationRow.DataRowState.ToBeDeleted))
                                                R.StateOfPublisherRow = ReplicationRow.DataRowState.ToBeDeleted;
                                            break;
                                        case DiversityWorkbench.Forms.FormReplicationConflict.ConflictResolution.DeleteSubscriber:
                                            if (this._ReplicationDirection == ReplicationDirection.Upload
                                            && (R.StateOfPublisherRow == ReplicationRow.DataRowState.Deleted
                                            || R.StateOfPublisherRow == ReplicationRow.DataRowState.ToBeDeleted))
                                                R.StateOfSubscriberRow = ReplicationRow.DataRowState.ToBeDeleted;
                                            break;
                                        case DiversityWorkbench.Forms.FormReplicationConflict.ConflictResolution.Download:
                                        case DiversityWorkbench.Forms.FormReplicationConflict.ConflictResolution.Upload:
                                            R.RestoreDeletedRow = true;
                                            R.ReplicateDataRow();
                                            break;
                                    }
                                    switch (R.RelationOfData)
                                    {
                                        case ReplicationRow.DataRelation.Inserted:
                                            this._NumberOfInsertedRows++;
                                            this.labelInserted.Text = "Inserted: " + _NumberOfInsertedRows.ToString();
                                            break;
                                        case ReplicationRow.DataRelation.Updated:
                                            this._NumberOfUpdatedRows++;
                                            this.labelUpdated.Text = "Updated: " + _NumberOfUpdatedRows.ToString();
                                            break;
                                        case ReplicationRow.DataRelation.Deleted:
                                            break;
                                        default:
                                            break;
                                    }

                                    // alter Code
                                    if (f.DialogResult == DialogResult.Yes)
                                    {
                                        if (this._ReplicationDirection == ReplicationDirection.Download
                                            && (R.StateOfSubscriberRow == ReplicationRow.DataRowState.Deleted
                                            || R.StateOfSubscriberRow == ReplicationRow.DataRowState.ToBeDeleted))
                                            R.StateOfPublisherRow = ReplicationRow.DataRowState.ToBeDeleted;
                                        else if (this._ReplicationDirection == ReplicationDirection.Upload
                                            && (R.StateOfPublisherRow == ReplicationRow.DataRowState.Deleted
                                            || R.StateOfPublisherRow == ReplicationRow.DataRowState.ToBeDeleted))
                                            R.StateOfSubscriberRow = ReplicationRow.DataRowState.ToBeDeleted;
                                        else if (this._ReplicationDirection == ReplicationDirection.Merge
                                            && R.RelationOfData == ReplicationRow.DataRelation.Deleted)
                                        {
                                            R.StateOfPublisherRow = ReplicationRow.DataRowState.ToBeDeleted;
                                            R.StateOfSubscriberRow = ReplicationRow.DataRowState.ToBeDeleted;
                                        }
                                    }

                                    R.MergeDataRow(f.ReplicationDirection, f.ConflictColumns);
                                    if (R.RelationOfData != ReplicationRow.DataRelation.Deleted)
                                        R.RelationOfData = ReplicationRow.DataRelation.NoDifference;

                                    this.NumberOfConflicts--;
                                }
                                if (f.StopConflictResolution)
                                    break;
                            }
                        }
                        break;
                }
            }
            else
            {
                if (this.toolTip.GetToolTip(this.buttonInfo) != null)
                {
                    string Message = this.toolTip.GetToolTip(this.buttonInfo);
                    if (Message.IndexOf("\r\n") > -1)
                        Message = Message.Substring(0, Message.IndexOf("\r\n"));
                    switch (this._ReplicationDirection)
                    {
                        case ReplicationDirection.Clean:
                            Message += " in ";
                            break;
                        case ReplicationDirection.Download:
                            Message += " from ";
                            break;
                        case ReplicationDirection.Merge:
                            Message += " in ";
                            break;
                        case ReplicationDirection.Upload:
                            Message += " of ";
                            break;
                    }
                    Message += "table " + this._TableName;
                    //if (this._ReplicationDirection == ReplicationDirection.Download || this._ReplicationDirection == ReplicationDirection.Upload)
                    //{
                    //    Message += "\r\n\r\nFilter data for replication:";
                    //    DiversityWorkbench.Forms.FormColumnFilter f = new Forms.FormColumnFilter(this.dtSource, Message, this.ConnectionSource.ConnectionString);
                    //    f.StartPosition = FormStartPosition.CenterParent;
                    //    f.ShowDialog();
                    //    if (f.DialogResult == DialogResult.OK)
                    //    {
                    //        this.SetFilter(f.Filter());
                    //        this._dtSource = null;
                    //        this._NumberOfSourceDatasets = null;
                    //        this.InitNumberLabel(this._ReplicationDirection);
                    //    }
                    //}
                    //else
                    //{
                    //    System.Windows.Forms.MessageBox.Show(Message);
                    //}
                }
            }
        }

        private void checkBoxTableName_CheckedChanged(object sender, EventArgs e)
        {
            if (this.buttonInfo.Tag == null)
            {
                if (this.checkBoxTableName.Checked)
                {
                    switch (this._ReplicationDirection)
                    {
                        case ReplicationDirection.Download:
                            this.buttonInfo.Image = this.imageListDirection.Images[0];
                            break;
                        case ReplicationDirection.Upload:
                            this.buttonInfo.Image = this.imageListDirection.Images[1];
                            break;
                        case ReplicationDirection.Clean:
                            this.buttonInfo.Image = this.imageListDirection.Images[2];
                            break;
                        case ReplicationDirection.Merge:
                            this.buttonInfo.Image = this.imageListDirection.Images[3];
                            break;
                    }
                }
                else
                    this.buttonInfo.Image = null;
            }
        }

        #endregion

        #region RowGUID

        private void SetRowGUIDOption(string TableName)
        {
            bool DestinationTableHasRowGuidIndex = DiversityWorkbench.Replication.IndexForRowGUID(TableName, this.ConnectionDestination);
            bool SourceTableHasRowGuidIndex = DiversityWorkbench.Replication.IndexForRowGUID(TableName, this.ConnectionSource);
            switch (this._ReplicationDirection)
            {
                case ReplicationDirection.Upload:
                case ReplicationDirection.Merge:
                    this.pictureBoxIndexRowGuidProvider.Visible = DestinationTableHasRowGuidIndex;
                    this.pictureBoxIndexRowGuidSubscriber.Visible = SourceTableHasRowGuidIndex;
                    break;
                case ReplicationDirection.Download:
                    this.pictureBoxIndexRowGuidProvider.Visible = SourceTableHasRowGuidIndex;
                    this.pictureBoxIndexRowGuidSubscriber.Visible = DestinationTableHasRowGuidIndex;
                    break;
                default:
                    this.pictureBoxIndexRowGuidProvider.Visible = false;
                    this.pictureBoxIndexRowGuidSubscriber.Visible = false;
                    break;
            }
        }

        private void pictureBoxIndexRowGuidProvider_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxIndexRowGuidSubscriber_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Filtering

        private void setReplicationFilters(System.Collections.Generic.Dictionary<string, ReplicationFilter> Filters)
        {
            try
            {
                System.Collections.Generic.Dictionary<string, ReplicationFilter> NewFilters = new Dictionary<string, ReplicationFilter>();
                foreach (string s in this.Columns)
                {
                    if (Filters.ContainsKey(s) && Filters[s].SQL.Length > 0)
                    {
                        NewFilters.Add(s, Filters[s]);
                        if (this.ReplicationFilters.ContainsKey(s))
                            this.ReplicationFilters.Remove(s);
                    }
                }
                foreach (System.Collections.Generic.KeyValuePair<string, ReplicationFilter> KV in NewFilters)
                {
                    this.ReplicationFilters.Add(KV.Key, KV.Value);
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void clearReplicationFilter()
        {
            this._ReplicationFilters = null;
            //foreach(System.Collections.Generic.KeyValuePair<string, ReplicationFilter> KV in this.ReplicationFilters)
            //{
            //    ReplicationFilter f = new ReplicationFilter("", "");
            //    KV.Value = f;
            //}
        }

        private System.Collections.Generic.Dictionary<string, ReplicationFilter> _ReplicationFilters;

        private System.Collections.Generic.Dictionary<string, ReplicationFilter> ReplicationFilters
        {
            get
            {
                if (this._ReplicationFilters == null)
                {
                    this._ReplicationFilters = new Dictionary<string, ReplicationFilter>();
                    foreach (string s in this.Columns)
                    {
                        ReplicationFilter filter = new ReplicationFilter("", "");
                        this._ReplicationFilters.Add(s, filter);
                    }
                }
                return _ReplicationFilters;
            }
        }

        private string _Filter;

        private void SetFilter(string Filter)
        {
            this._Filter = Filter;
            if (this._Filter.Length > 0) this.buttonFilter.BackColor = System.Drawing.Color.Pink;
            else this.buttonFilter.BackColor = System.Drawing.Color.Transparent;
        }

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            string Message = "";
            if (this._ReplicationDirection == ReplicationDirection.Download || this._ReplicationDirection == ReplicationDirection.Upload)
            {
                Message += "Filter data for replication in table\r\n" + this._TableName + ":";
                DiversityWorkbench.Forms.FormColumnFilter f = new Forms.FormColumnFilter(this.dtSource, Message, this.ConnectionSource.ConnectionString);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    this.SetFilter(f.Filter());
                    this._dtSource = null;
                    this._NumberOfSourceDatasets = null;
                    this.InitNumberLabel(this._ReplicationDirection);
                    this.setReplicationFilters(f.Filters());
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(Message);
            }
        }

        private System.Collections.Generic.Dictionary<string, string> _ColumnFilter;

        public System.Collections.Generic.Dictionary<string, string> ColumnFilter
        {
            get
            {
                if (this._ColumnFilter == null)
                {
                    this._ColumnFilter = new Dictionary<string, string>();
                    foreach (string s in this.DataColumns)
                        this._ColumnFilter.Add(s, "");
                }
                return _ColumnFilter;
            }
            set { _ColumnFilter = value; }
        }

        private string Filter()
        {
            if (this._Filter == null)
                this._Filter = "";
            return this._Filter;

            string Filter = "";
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ColumnFilter)
            {
                if (KV.Value.Length > 0)
                {
                    if (Filter.Length > 0) Filter += " AND ";
                    Filter += KV.Key + KV.Value;
                }
            }
            return Filter;
        }

        private void buttonFilterPropagate_Click(object sender, EventArgs e)
        {
            if (this._replicationForm != null)
                this._replicationForm.PropagateFilter(this.TableName, this.ReplicationFilters);
        }

        private void buttonFilterClear_Click(object sender, EventArgs e)
        {
            this._ReplicationFilters = null;
        }

        #endregion

    }

    public class ForeignKey
    {

        #region Parameter
        /// <summary>
        /// The name of the table
        /// </summary>
        public string TableName;
        /// <summary>
        /// The name of the column in the table
        /// </summary>
        public string ColumnName;
        /// <summary>
        /// the RowGUID of the referred value
        /// </summary>
        public string RowGUIDofReferredEntry;
        /// <summary>
        /// The name of the foreign table
        /// </summary>
        public string ForeignTable;
        /// <summary>
        /// The name of the column in the referred table
        /// </summary>
        public string ForeignColumnName;
        /// <summary>
        /// the RowGUID of the referred value in the referred table
        /// </summary>
        public string ForeignRowGUIDofReferredEntry;

        private Microsoft.Data.SqlClient.SqlConnection _ConnectionSource;
        private Microsoft.Data.SqlClient.SqlConnection _ConnectionDestination;

        #endregion

        #region Construction

        public ForeignKey(string TableName, string ColumnName, string ForeignTable, Microsoft.Data.SqlClient.SqlConnection ConnectionSource, Microsoft.Data.SqlClient.SqlConnection ConnectionDestination)
        {
            this.TableName = TableName;
            this.ColumnName = ColumnName;
            this.ForeignTable = ForeignTable;
            this._ConnectionDestination = ConnectionDestination;
            this._ConnectionSource = ConnectionSource;
        }

        #endregion

        //public void setForeignColumnName(string ColumnName)
        //{ this.ForeignColumnName = ColumnName; }
        //public void setRowGUIDofReferredEntry(string RowGUID)
        //{ this.RowGUIDofReferredEntry = RowGUID; }
        //public void setForeignRowGUIDofReferredEntry(string RowGUID)
        //{ this.ForeignRowGUIDofReferredEntry = RowGUID; }
    }

    public class ReplicationRow
    {

        #region Connections

        private static Microsoft.Data.SqlClient.SqlConnection _SqlConnectionPublisher;

        public static Microsoft.Data.SqlClient.SqlConnection SqlConnectionPublisher
        {
            get
            {
                if (ReplicationRow._SqlConnectionPublisher != null)
                {
                    if (ReplicationRow._SqlConnectionPublisher.State.ToString() == "Closed" &&
                        ReplicationRow._SqlConnectionPublisher.ConnectionString.Length > 0)
                        ReplicationRow._SqlConnectionPublisher.Open();
                }
                return ReplicationRow._SqlConnectionPublisher;
            }
            set { ReplicationRow._SqlConnectionPublisher = value; }
        }

        private static Microsoft.Data.SqlClient.SqlConnection _SqlConnectionSubscriber;

        public static Microsoft.Data.SqlClient.SqlConnection SqlConnectionSubscriber
        {
            get
            {
                if (ReplicationRow._SqlConnectionSubscriber == null)
                    ReplicationRow._SqlConnectionSubscriber = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                if (ReplicationRow._SqlConnectionSubscriber.State.ToString() == "Closed")
                    ReplicationRow._SqlConnectionSubscriber.Open();
                return ReplicationRow._SqlConnectionSubscriber;
            }
            set { ReplicationRow._SqlConnectionSubscriber = value; }
        }

        public static void ResetConnections()
        {
            DiversityWorkbench.UserControls.ReplicationRow._SqlConnectionPublisher = null;
            DiversityWorkbench.UserControls.ReplicationRow._SqlConnectionSubscriber = null;
        }

        #endregion

        #region Parameter and properties

        private static bool? _IgnoreConflicts;

        public static bool IgnoreConflicts
        {
            get
            {
                if (ReplicationRow._IgnoreConflicts == null)
                    ReplicationRow._IgnoreConflicts = false;
                return (bool)ReplicationRow._IgnoreConflicts;
            }
            set { ReplicationRow._IgnoreConflicts = value; }
        }

        private UserControlReplicateTable.ReplicationDirection _ReplicationDirection;

        private System.Data.DataRow _DataRow;
        private System.Data.DataRow _DataRowDestination;

        private System.DateTime _InsertDateInSubscriber;
        private System.DateTime _UpdateDateInSubscriber;

        private System.DateTime _InsertDateInPublisher;
        private System.DateTime _UpdateDateInPublisher;

        public enum DataRelation { Start, Conflict, Error, MissingKeyRelation, ForeignKeySameTableConflict, Inserted, Updated, Deleted, NoDifference };
        private DataRelation _RelationOfData;

        public enum DataRowState { NoChanges, Updated, Deleted, ToBeDeleted, ToBeChecked };
        private DataRowState _StateOfPublisherRow;
        private DataRowState _StateOfSubscriberRow;
        private string _RowGUID;

        private string _Message;
        public void setMessage(string Message) { _Message = Message; }

        private string _TableName;
        private System.Collections.Generic.List<string> _PrimaryKey;
        private System.Collections.Generic.List<string> _ForeignPrimaryKey;
        private System.Collections.Generic.List<string> _ForeignKey;
        private System.Collections.Generic.Dictionary<string, string> _ColumnDictionary;
        private System.Collections.Generic.List<string> _DataColumns;
        private System.Collections.Generic.List<DiversityWorkbench.UserControls.ForeignKey> _ForeignKeys;
        private string _IdentityColumn;
        private bool _RowGUIDisPresentInDestination;
        private bool? _RowGUIDhasBeenDeletedInDestination;

        private bool _PKisPresentInDestination = false;
        private bool _PKMatchesRowGUIDInDestination = false;

        // for Merge
        private bool _RowGUIDisPresentInSource;
        private bool? _RowGUIDhasBeenDeletedInSource;
        private bool _PKisPresentInSource = false;
        private bool _PKMatchesRowGUIDInSource = false;

        private string _ColumnUpdateDateInSubscriber = "";
        private string _ColumnInsertDateInSubscriber = "";
        System.DateTime _UpdateDateSubscriber;

        private string _ColumnUpdateDateInPublisher = "";
        private string _ColumnInsertDateInPublisher = "";
        System.DateTime _UpdateDatePublisher;

        // for Restore
        private bool _RestoreDeletedRow = false;

        public bool RestoreDeletedRow
        {
            get { return _RestoreDeletedRow; }
            set { _RestoreDeletedRow = value; }
        }

        public enum ConflictType { None, ForeignKeySameTable }

        private ConflictType _ConflictType = ConflictType.None;

        public ConflictType TypeOfConflict { get { return this._ConflictType; } }

        #endregion

        #region Construction

        public ReplicationRow(string Table,
            System.Data.DataRow Row,
            UserControlReplicateTable.ReplicationDirection Direction,
            string IdentityColumn,
            System.Collections.Generic.List<string> PrimaryKey,
            System.Collections.Generic.List<string> ForeignPrimaryKey,
            System.Collections.Generic.List<DiversityWorkbench.UserControls.ForeignKey> ForeignKeys,
            System.Collections.Generic.Dictionary<string, string> ColumnDictionary,
            System.Collections.Generic.List<string> DataColumns)//, string ConnectionPublisher,  string ConnectionSubscriber,
        {
            this._TableName = Table;
            this._DataRow = Row;
            this._ReplicationDirection = Direction;
            //this._ConnectionPublisher = ConnectionPublisher;
            //this._ConnectionSubscriber = ConnectionSubscriber;
            this._PrimaryKey = PrimaryKey;
            this._ForeignKeys = ForeignKeys;
            this._ForeignPrimaryKey = ForeignPrimaryKey;
            this._IdentityColumn = IdentityColumn;
            this._ColumnDictionary = ColumnDictionary;
            this._DataColumns = DataColumns;
            this._RelationOfData = DataRelation.Start;
            this._StateOfPublisherRow = DataRowState.ToBeChecked;
            this._StateOfSubscriberRow = DataRowState.ToBeChecked;
        }

        #endregion

        #region Interface

        public void ReplicateDataRow()
        {
            try
            {
                if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Clean)
                    this.Delete();
                else
                {
                    if (this.NoErrors)
                    {
                        if (this.RowGUIDhasBeenDeletedInDestination && !this.RestoreDeletedRow)
                            this._RelationOfData = DataRelation.Deleted;
                        else
                        {
                            if (this.CheckPresenceOfData())
                            {
                                if (!this._RowGUIDisPresentInDestination)
                                    this.Insert();
                                else
                                    this.Update();
                                if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Merge)
                                {
                                    if (!this._RowGUIDisPresentInSource
                                        && (bool)!this._RowGUIDhasBeenDeletedInSource)
                                    {
                                        this.Insert();
                                    }
                                    if ((bool)this._RowGUIDhasBeenDeletedInSource || (bool)this._RowGUIDhasBeenDeletedInDestination)
                                    {
                                        this._RelationOfData = DataRelation.Deleted;
                                    }
                                }
                            }
                        }
                    }
                    else this._RelationOfData = DataRelation.Error;
                }
            }
            catch (System.Exception ex)
            {
                this._RelationOfData = DataRelation.Error;
            }
        }

        public void MergeDataRow(DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection ReplicationDirection, System.Collections.Generic.Dictionary<string, string> ConflictColumns)
        {
            bool OK = true;
            string SQL = "";
            if (ConflictColumns != null && ConflictColumns.Count > 0
                && ReplicationDirection != UserControlReplicateTable.ReplicationDirection.Clean
                && this.StateOfPublisherRow != DataRowState.ToBeDeleted
                && this.StateOfSubscriberRow != DataRowState.ToBeDeleted)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in ConflictColumns)
                {
                    if (SQL.Length > 0) SQL += ", ";
                    if (this._DataRow.Table.Columns[KV.Key].DataType.Name != "String"
                        && KV.Value.Length == 0)
                        SQL += KV.Key + " = NULL";
                    else
                        SQL += KV.Key + " = '" + KV.Value + "'";
                }
                SQL = "Update T SET " + SQL + " FROM [" + this._TableName + "] AS T WHERE RowGUID = '" + this.RowGUID + "'";
                switch (ReplicationDirection)
                {
                    case UserControlReplicateTable.ReplicationDirection.Upload:
                        if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Upload)
                            OK = this.SqlExecuteNonQueryInDestination(SQL);
                        else if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Download)
                            OK = this.SqlExecuteNonQueryInSource(SQL);
                        break;
                    case UserControlReplicateTable.ReplicationDirection.Download:
                        if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Download)
                            OK = this.SqlExecuteNonQueryInDestination(SQL);
                        else if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Upload)
                            OK = this.SqlExecuteNonQueryInSource(SQL);
                        break;
                    case UserControlReplicateTable.ReplicationDirection.Merge:
                        OK = this.SqlExecuteNonQueryInDestination(SQL);
                        if (OK)
                        {
                            if (!this.SqlExecuteNonQueryInSource(SQL))
                                OK = false;
                        }
                        break;
                }
            }
            else
            {
                string SqlDelete = "DELETE [" + this._TableName + "] WHERE RowGUID = '" + this.RowGUID + "'";
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlDelete, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher);
                if ((this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Download || this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Merge) &&
                    this.StateOfPublisherRow == DataRowState.ToBeDeleted)
                {
                    try
                    {
                        C.ExecuteNonQuery();
                    }
                    catch (System.Exception ex) { System.Windows.Forms.MessageBox.Show(ex.Message); }
                }
                if ((this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Upload || this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Merge) &&
                    this.StateOfSubscriberRow == DataRowState.ToBeDeleted)
                {
                    this.Delete();
                }
            }
        }

        public DataRowState StateOfPublisherRow
        {
            get
            {
                try
                {
                    if (this._StateOfPublisherRow != DataRowState.ToBeChecked)
                        return this._StateOfPublisherRow;
                    if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Upload)
                    {
                        if (this.RowGUIDhasBeenDeletedInDestination)
                            this._StateOfPublisherRow = DataRowState.Deleted;
                        if (this._StateOfPublisherRow != DataRowState.ToBeChecked)
                            return this._StateOfPublisherRow;
                    }
                    if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Upload)
                    {
                        /// during an upload the state changed only if the row in the subscriber has been changed has been changed after the row in the publisher
                        if (this.UpdateDateInSubscriber < this.UpdateDateInPublisher
                            || this.UpdateDateInSubscriber < this.UpdateDateInPublisher)
                            this._StateOfPublisherRow = DataRowState.Updated;
                        else
                            this._StateOfPublisherRow = DataRowState.NoChanges;
                    }
                    else
                    {
                        if (this.InsertDateInSubscriber > this.UpdateDateInPublisher
                            || this.InsertDateInSubscriber == this.UpdateDateInPublisher)
                            this._StateOfPublisherRow = DataRowState.NoChanges;
                        else
                            this._StateOfPublisherRow = DataRowState.Updated;
                    }
                }
                catch (System.Exception ex) { }
                return this._StateOfPublisherRow;
            }
            set { this._StateOfPublisherRow = value; }
        }

        public DataRowState StateOfSubscriberRow
        {
            get
            {
                try
                {
                    if (this._StateOfSubscriberRow != DataRowState.ToBeChecked)
                        return this._StateOfSubscriberRow;
                    if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Download)
                    {
                        if (this.RowGUIDhasBeenDeletedInDestination)
                            this._StateOfSubscriberRow = DataRowState.Deleted;
                        if (this._StateOfSubscriberRow != DataRowState.ToBeChecked)
                            return this._StateOfSubscriberRow;
                    }
                    if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Upload)
                    {
                        /// the row in the subscriber has been updated if the last changes happened after those in the publisher
                        if (this.UpdateDateInSubscriber > this.UpdateDateInPublisher)
                            this._StateOfSubscriberRow = DataRowState.Updated;
                        else
                            this._StateOfSubscriberRow = DataRowState.NoChanges;
                    }
                    else
                    {
                        if (this.InsertDateInSubscriber == this.UpdateDateInSubscriber)
                            this._StateOfSubscriberRow = DataRowState.NoChanges;
                        else
                            this._StateOfSubscriberRow = DataRowState.Updated;
                    }
                }
                catch (System.Exception ex) { }
                return this._StateOfSubscriberRow;
            }
            set
            {
                this._StateOfSubscriberRow = value;
            }
        }

        /// <summary>
        /// the date when the data had been inserted into the subscriber table
        /// </summary>
        public System.DateTime InsertDateInSubscriber
        {
            get
            {
                if (this.DateIsValid(this._InsertDateInSubscriber.ToString())) // != "01.01.0001 00:00:00")
                    return this._InsertDateInSubscriber;
                try
                {
                    if (this.ColumnInsertDateInSubscriber.Length > 0)
                    {
                        string SQL = "SELECT CONVERT(DateTime, [" + this.ColumnInsertDateInSubscriber + "], 121)" +
                        "FROM [" + this._TableName + "] AS T " +
                        "WHERE (RowGUID = '" + this.RowGUID + "')";
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber);//, con);
                        System.DateTime.TryParse(C.ExecuteScalar()?.ToString(), out this._InsertDateInSubscriber);
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                return _InsertDateInSubscriber;
            }
        }

        /// <summary>
        /// the date when the data where last changed in the subscriber
        /// </summary>
        public System.DateTime UpdateDateInSubscriber
        {
            get
            {
                if (this.DateIsValid(this._UpdateDateInSubscriber.ToString())) // != "01.01.0001 00:00:00")
                    return this._UpdateDateInSubscriber;
                else
                    this._UpdateDateInSubscriber = this.UpdateDate(DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber);// this._ConnectionSubscriber);
                return _UpdateDateInSubscriber;
            }
        }

        /// <summary>
        /// the date when the data had been inserted into the publisher table
        /// </summary>
        public System.DateTime InsertDateInPublisher
        {
            get
            {
                if (this.DateIsValid(this._InsertDateInPublisher.ToString())) // != "01.01.0001 00:00:00")
                    return this._InsertDateInPublisher;
                try
                {
                    if (this.ColumnInsertDateInSubscriber.Length > 0)
                    {
                        string SQL = "SELECT CONVERT(DateTime, [" + this.ColumnInsertDateInSubscriber + "], 121)" +
                        "FROM [" + this._TableName + "] AS T " +
                        "WHERE (RowGUID = '" + this.RowGUID + "')";
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher);//, con);
                        System.DateTime.TryParse(C.ExecuteScalar()?.ToString(), out this._InsertDateInPublisher);
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                return _InsertDateInPublisher;
            }
        }

        /// <summary>
        /// the date when the data where last changed in the publisher
        /// </summary>
        public System.DateTime UpdateDateInPublisher
        {
            get
            {
                if (this.DateIsValid(this._UpdateDateInPublisher.ToString()))
                    return this._UpdateDateInPublisher;
                else
                    this._UpdateDateInPublisher = this.UpdateDate(DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher);// this._ConnectionPublisher);
                return _UpdateDateInPublisher;
            }
        }

        private bool DateIsValid(string Date)
        {
            bool ValidDate = true;
            System.DateTime D;
            if (System.DateTime.TryParse(Date, out D))
            {
                if (D.Day == 1 && D.Month == 1 && D.Year == 1)
                    ValidDate = false;
            }
            else
                ValidDate = false;
            return ValidDate;
        }

        private System.DateTime UpdateDate(Microsoft.Data.SqlClient.SqlConnection Connection)
        {
            System.DateTime Update = new DateTime();
            if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Clean && Connection.ConnectionString.Length == 0)
                return Update;
            try
            {
                string SQL = "SELECT CONVERT(DateTime, [LogUpdatedWhen], 121)" +
                "FROM [" + this._TableName + "] AS T " +
                "WHERE (RowGUID = '" + this.RowGUID + "')";
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, Connection);
                System.DateTime.TryParse(C.ExecuteScalar()?.ToString(), out Update);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Update;
        }

        private System.DateTime UpdateDate(string ConnectionString)
        {
            System.DateTime Update = new DateTime();
            if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Clean && ConnectionString.Length == 0)
                return Update;
            try
            {
                string SQL = "SELECT CONVERT(DateTime, [LogUpdatedWhen], 121)" +
                "FROM [" + this._TableName + "] AS T " +
                "WHERE (RowGUID = '" + this.RowGUID + "')";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                System.DateTime.TryParse(C.ExecuteScalar()?.ToString(), out Update);
                con.Close();
                //con.Dispose();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Update;
        }

        public string RowGUID
        {
            get
            {
                if (this._RowGUID == null)
                {
                    try
                    {
                        this._RowGUID = this._DataRow["RowGUID"].ToString();
                    }
                    catch (Exception ex)
                    {
                        this._RowGUID = "";
                    }
                }
                return _RowGUID;
            }
        }

        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        public DataRelation RelationOfData
        {
            get { return _RelationOfData; }
            set { _RelationOfData = value; }
        }

        public System.Data.DataRow DataRow
        {
            get { return _DataRow; }
            //set { _DataRow = value; }
        }

        public System.Data.DataRow DataRowDestination
        {
            get
            {
                if (this._DataRowDestination == null)
                {
                    string SQL = "SELECT * FROM [" + this._TableName + "] WHERE RowGUID = '" + this.RowGUID + "'";
                    System.Data.DataTable dt = new DataTable();
                    this.SqlFillTableFromDestination(SQL, ref dt);
                    if (dt.Rows.Count > 0)
                        this._DataRowDestination = dt.Rows[0];
                }
                return _DataRowDestination;
            }
        }

        #endregion

        #region Infos about the data

        private bool NoErrors
        {
            get
            {
                bool OK = true;
                try
                {
                    int i;
                    string SqlRowGUID = "SELECT COUNT(*) FROM [" + this._TableName + "] WHERE RowGUID = '" + this.RowGUID + "'";
                    if (int.TryParse(this.SqlExecuteScalarInSource(SqlRowGUID), out i))
                    {
                        if (i == 1)
                        {
                            return true;
                        }
                        else if (i == 0 && this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Merge)
                        {
                            int ii = 0;
                            if (int.TryParse(this.SqlExecuteScalarInDestination(SqlRowGUID), out ii))
                            {
                                if (ii == 1) return true;
                                else if (ii == 0)
                                {
                                    this._Message += "RowGUID is missing in source and destination\r\n";
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            this._Message += "RowGUID in source is not unique or missing\r\n";
                            return false;
                        }
                    }
                    else return false;
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    return false;
                }
                return OK;
            }
        }

        private bool CheckPresenceOfData()
        {
            try
            {
                this._RowGUIDisPresentInSource = true;
                string SqlCountRowGUID = "SELECT COUNT(*) FROM [" + this._TableName + "] WHERE RowGUID = '" + this._DataRow["RowGUID"].ToString() + "'";
                this._RowGUIDisPresentInDestination = false;
                int i;
                if (int.TryParse(this.SqlExecuteScalarInDestination(SqlCountRowGUID), out i) && i == 1)
                    this._RowGUIDisPresentInDestination = true;
                bool KeysFound = false;
                string Message = "";
                string SqlCountPK = "SELECT COUNT(*) FROM [" + this._TableName + "] WHERE 1 = 1 ";
                if (this._PrimaryKey.Count > 0)
                {
                    System.Collections.Generic.Dictionary<string, string> Keys = this.GetForeignKeyValuesForNewDataset(ref KeysFound, ref Message);
                    if (!KeysFound && Message.Length > 0)
                        return false;

                    foreach (string s in this._PrimaryKey)
                    {
                        if (Keys.ContainsKey(s))
                            SqlCountPK += " AND " + s + " = " + this.SqlValue(s, Keys);
                        else
                            SqlCountPK += " AND " + s + " = " + this.SqlValue(s);
                    }
                }
                this._PKisPresentInDestination = false;
                if (int.TryParse(this.SqlExecuteScalarInDestination(SqlCountPK), out i) && i == 1)
                    this._PKisPresentInDestination = true;

                string SqlCountMatch = SqlCountPK + " AND RowGUID = '" + this._DataRow["RowGUID"].ToString() + "'";
                this._PKMatchesRowGUIDInDestination = false;
                if (this._RowGUIDisPresentInDestination && this._PKisPresentInDestination)
                {
                    if (int.TryParse(this.SqlExecuteScalarInDestination(SqlCountMatch), out i) && i == 1)
                        this._PKMatchesRowGUIDInDestination = true;
                }
                if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Merge)
                {
                    this._RowGUIDisPresentInSource = false;
                    if (int.TryParse(this.SqlExecuteScalarInSource(SqlCountRowGUID), out i) && i == 1)
                        this._RowGUIDisPresentInSource = true;

                    this._PKisPresentInSource = false;
                    if (int.TryParse(this.SqlExecuteScalarInSource(SqlCountPK), out i) && i == 1)
                        this._PKisPresentInSource = true;

                    this._PKMatchesRowGUIDInSource = false;
                    if (this._RowGUIDisPresentInSource && this._PKisPresentInSource)
                    {
                        if (int.TryParse(this.SqlExecuteScalarInSource(SqlCountMatch), out i) && i == 1)
                            this._PKMatchesRowGUIDInSource = true;
                    }
                }
                int iDel;
                this._RowGUIDhasBeenDeletedInDestination = false;
                this._RowGUIDhasBeenDeletedInSource = false;
                string SqlCountDelete = "SELECT COUNT(*) FROM [" + this._TableName + "_log] " +
                    "WHERE RowGUID = '" + this.RowGUID + "' AND LogState = 'D'";
                if (this._RowGUIDisPresentInDestination && !this._RowGUIDisPresentInSource)
                {
                    if (int.TryParse(this.SqlExecuteScalarInSource(SqlCountDelete), out i) && i == 1)
                        this._RowGUIDhasBeenDeletedInSource = true;
                }
                if (!this._RowGUIDisPresentInDestination && this._RowGUIDisPresentInSource)
                {
                    if (int.TryParse(this.SqlExecuteScalarInDestination(SqlCountDelete), out i) && i == 1)
                        this._RowGUIDhasBeenDeletedInDestination = true;
                }
                return true;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return false;
            }
        }

        public bool RowGUIDhasBeenDeletedInDestination
        {
            get
            {
                if (this._RowGUIDhasBeenDeletedInDestination == null)
                {
                    if (DiversityWorkbench.UserControls.ReplicationRow.IgnoreConflicts && this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Download)
                        this._RowGUIDhasBeenDeletedInDestination = false;
                    else
                    {
                        int i = 0;
                        try
                        {
                            string SqlCount = "IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + this._TableName + "_log]') AND type in (N'U')) " +
                                "SELECT COUNT(*) FROM [" + this._TableName + "_log] WHERE RowGUID = '" + this._DataRow["RowGUID"].ToString() + "' AND LogState = 'D'" +
                                "ELSE SELECT 0";
                            if (int.TryParse(this.SqlExecuteScalarInDestination(SqlCount), out i) && i > 0)
                            {
                                SqlCount = "IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + this._TableName + "]') AND type in (N'U')) " +
                                "SELECT COUNT(*) FROM [" + this._TableName + "] WHERE RowGUID = '" + this._DataRow["RowGUID"].ToString() + "'" +
                                "ELSE SELECT 0";
                                if (int.TryParse(this.SqlExecuteScalarInDestination(SqlCount), out i) && i == 0)
                                    this._RowGUIDhasBeenDeletedInDestination = true;
                                else
                                    this._RowGUIDhasBeenDeletedInDestination = false;
                            }
                            else this._RowGUIDhasBeenDeletedInDestination = false;
                        }
                        catch (System.Exception ex) { this._RowGUIDhasBeenDeletedInDestination = false; }
                    }
                }
                return (bool)_RowGUIDhasBeenDeletedInDestination;
            }
        }

        private System.Collections.Generic.Dictionary<string, string> _ForeignKeyValuesForNewDataset;
        /// <summary>
        /// Getting the foreign key values for a new dataset derived via RowGUID in the destination database
        /// </summary>
        /// <param name="Keysfound">If the keys where found</param>
        /// <param name="Message">The SQL statement that created an error</param>
        /// <returns></returns>
        public System.Collections.Generic.Dictionary<string, string> GetForeignKeyValuesForNewDataset(ref bool Keysfound, ref string Message)
        {
            if (this._ForeignKeyValuesForNewDataset != null)
            {
                if (this._ForeignKeyValuesForNewDataset.Count > 0)
                    Keysfound = true;
                return this._ForeignKeyValuesForNewDataset;
            }

            Keysfound = true;
            string RelatingColumn = "";
            string IdentityColumn = "";
            string RelatedTable = "";

            this._ForeignKeyValuesForNewDataset = new Dictionary<string, string>();
            //System.Collections.Generic.Dictionary<string, string> PK = new Dictionary<string, string>();
            string SQL = "";
            try
            {
                // getting the list of all tables the current table has a relation to
                System.Data.DataTable dtTables = new DataTable();
                SQL = "SELECT DISTINCT C.TABLE_NAME " +
                    "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                    "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON K.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
                    "INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS C ON R.UNIQUE_CONSTRAINT_NAME = C.CONSTRAINT_NAME " +
                    "WHERE (K.TABLE_NAME = '" + this._TableName + "')";
                if (this.SqlFillTableFromDestination(SQL, ref dtTables))
                {
                    foreach (System.Data.DataRow Rtables in dtTables.Rows)
                    {
                        // check if the relation is within the same table and get the columns of the relation
                        RelatedTable = Rtables[0].ToString();
                        SQL = "SELECT DISTINCT K.COLUMN_NAME AS TableColumn, C.COLUMN_NAME AS RelatedColumn " +
                            "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kref INNER JOIN " +
                            "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                            "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON K.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
                            "INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS C ON R.UNIQUE_CONSTRAINT_NAME = C.CONSTRAINT_NAME ON  " +
                            "Kref.TABLE_NAME = C.TABLE_NAME AND Kref.ORDINAL_POSITION = K.ORDINAL_POSITION " +
                            "WHERE  Kref.CONSTRAINT_NAME = C.CONSTRAINT_NAME  " +
                            "AND C.COLUMN_NAME = Kref.COLUMN_NAME " +
                            "AND  (K.TABLE_NAME = '" + this._TableName + "') " +
                            "AND (C.TABLE_NAME = '" + Rtables[0].ToString() + "')";
                        System.Data.DataTable dtColumns = new DataTable();
                        if (this.SqlFillTableFromDestination(SQL, ref dtColumns))
                        {
                            // getting the RowGUID of the related entry
                            SQL = "SELECT RowGUID " +
                                "FROM [" + Rtables[0].ToString() + "] " +
                                "WHERE 1 = 1 ";
                            // build the selection string and check if there is a relation present
                            bool RelationIsPresent = true;
                            foreach (System.Data.DataRow Rcolumn in dtColumns.Rows)
                            {
                                RelatingColumn = Rcolumn[0].ToString();
                                if (this._DataRow[RelatingColumn].Equals(System.DBNull.Value))
                                {
                                    RelationIsPresent = false;
                                    break;
                                }
                                SQL += " AND " + Rcolumn[1].ToString() + " = '" + this._DataRow[Rcolumn[0].ToString()].ToString() + "'";
                            }
                            if (RelationIsPresent)
                            {
                                // getting the RowGUID for the related dataset in the source
                                string RowGUID = this.SqlExecuteScalarInSource(SQL);
                                if (RowGUID.Length > 0)
                                {
                                    // getting the PK of the related table in the destination
                                    SQL = "";
                                    foreach (System.Data.DataRow Rcolumn in dtColumns.Rows)
                                    {
                                        if (SQL.Length > 0) SQL += ", ";
                                        SQL += " " + Rcolumn[1].ToString();
                                    }
                                    SQL = "SELECT " + SQL + " FROM " + Rtables[0].ToString() + " WHERE RowGUID = '" + RowGUID + "'";
                                    System.Data.DataTable dtPK = new DataTable();
                                    if (this.SqlFillTableFromDestination(SQL, ref dtPK))
                                    {
                                        if (dtPK.Rows.Count == 0) // No data where found for this RowGUID, try to find the values via the PK
                                        {
                                            SQL = "";
                                            foreach (System.Data.DataRow Rcolumn in dtColumns.Rows)
                                            {
                                                if (SQL.Length > 0) SQL += ", ";
                                                SQL += " " + Rcolumn[1].ToString();
                                            }
                                            SQL = "SELECT " + SQL + " FROM " + Rtables[0].ToString() + " WHERE  1 = 1 ";
                                            foreach (System.Data.DataRow Rcolumn in dtColumns.Rows)
                                            {
                                                SQL += " AND " + Rcolumn[1].ToString() + " = '" + this._DataRow[Rcolumn[0].ToString()].ToString() + "'";
                                            }
                                            if (this.SqlFillTableFromDestination(SQL, ref dtPK))
                                            {
                                                /// MW 27.3.2015: In case a foreign key relation does exist and the related data had not been replicated, 
                                                /// but a dataset with the same key does exist in the destination, do not use these data
                                                /// this check is restricted to not Enum tables
                                                // Getting the RowGUID for comparision to make sure they do match
                                                string SqlRowGUID = "SELECT RowGUID " + SQL.Substring(SQL.IndexOf(" FROM "));
                                                string RowGUIDInDest = this.SqlExecuteScalarInDestination(SqlRowGUID);
                                                if (RowGUID.ToLower() != RowGUIDInDest.ToLower() &&
                                                    !Rtables[0].ToString().ToLower().EndsWith("_enum"))
                                                {
                                                    Keysfound = false;
                                                    Message = "RowGUIDs do not match: " + RowGUIDInDest + " (Destination) <> " + RowGUID + " (Source); " + SqlRowGUID;
                                                    if (this.Message == null)
                                                        this.Message = Message;
                                                    else this.Message += "; " + Message;
                                                    this._RelationOfData = DataRelation.MissingKeyRelation;
                                                    this.setMessage(Message);
                                                    return this._ForeignKeyValuesForNewDataset;
                                                }
                                                else if (dtPK.Rows.Count > 0)
                                                {
                                                    foreach (System.Data.DataRow Rcolumn in dtColumns.Rows)
                                                    {
                                                        if (!this._ForeignKeyValuesForNewDataset.ContainsKey(Rcolumn[0].ToString()))
                                                        {
                                                            string Column = Rcolumn[0].ToString();
                                                            string Value = dtPK.Rows[0][Rcolumn[1].ToString()].ToString();
                                                            this._ForeignKeyValuesForNewDataset.Add(Column, Value);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    Keysfound = false;
                                                    Message = SQL;
                                                    return this._ForeignKeyValuesForNewDataset;
                                                }
                                            }
                                            else
                                            {
                                                Keysfound = false;
                                                Message = SQL;
                                                return this._ForeignKeyValuesForNewDataset;
                                            }
                                        }
                                        else
                                        {
                                            // PK in destination was found. Fill the dictionary with the values for each column
                                            foreach (System.Data.DataRow Rcolumn in dtColumns.Rows)
                                            {
                                                RelatingColumn = Rcolumn[0].ToString();
                                                IdentityColumn = Rcolumn[1].ToString();

                                                if (!this._ForeignKeyValuesForNewDataset.ContainsKey(IdentityColumn))
                                                {
                                                    string Value = dtPK.Rows[0][IdentityColumn].ToString();
                                                    this._ForeignKeyValuesForNewDataset.Add(RelatingColumn, Value);
                                                }

                                                //if (!PK.ContainsKey(Rcolumn[0].ToString()))
                                                //{
                                                //    string Column = Rcolumn[0].ToString();
                                                //    //string Value = dtPK.Rows[0][Rcolumn[0].ToString()].ToString();
                                                //    string Value = dtPK.Rows[0][Rcolumn[1].ToString()].ToString();
                                                //    //PK.Add(Rcolumn[0].ToString(), dtPK.Rows[0][Rcolumn[0].ToString()].ToString());
                                                //    PK.Add(Rcolumn[0].ToString(), dtPK.Rows[0][Rcolumn[1].ToString()].ToString());
                                                //}
                                            }
                                        }
                                    }
                                }
                            }
                            //Keysfound = false;
                        }
                        else
                        {
                            Keysfound = false;
                            Message = SQL;
                            return this._ForeignKeyValuesForNewDataset;
                        }
                    }
                }
                else
                {
                    Keysfound = false;
                    Message = SQL;
                }
            }
            catch (System.Exception ex)
            {
                Keysfound = false;
                Message = SQL;
            }
            return this._ForeignKeyValuesForNewDataset;
        }

        #endregion

        #region Writing the data

        private System.Collections.Generic.Dictionary<string, string> _ForeignKeyValues;

        private bool Insert()
        {
            System.Collections.Generic.List<string> HandledColumns = new List<string>();
            bool OK = true;
            string SqlInsert = "";
            string SqlColumns = "";
            string SqlValues = "";
            System.Collections.Generic.Dictionary<string, string> ColumnValues = new Dictionary<string, string>();
            bool ForeignKeyFound = true;
            try
            {
                string Error = "";
                this._ForeignKeyValues = this.GetForeignKeyValuesForNewDataset(ref ForeignKeyFound, ref Error);
                if (!ForeignKeyFound)
                {
                    //this._Message = Message;
                    this._RelationOfData = DataRelation.MissingKeyRelation;
                }
                if (this._IdentityColumn.Length > 0 && !this._PKisPresentInDestination)
                {
                    HandledColumns.Add(this._IdentityColumn);
                    SqlColumns = this._IdentityColumn;
                    SqlValues = this._DataRow[this._IdentityColumn].ToString();
                    ColumnValues.Add(this._IdentityColumn, this._DataRow[this._IdentityColumn].ToString());
                }
                else if (!this._PKisPresentInDestination)
                {
                    for (int i = 0; i < this._PrimaryKey.Count; i++)
                    {
                        if (HandledColumns.Contains(this._PrimaryKey[i])) continue;
                        if (!this._ColumnDictionary.ContainsKey(this._PrimaryKey[i])) continue;
                        if (this._DataRow[this._PrimaryKey[i]].Equals(System.DBNull.Value)) continue;
                        if (i > 0 || SqlColumns.Length > 0)
                        {
                            SqlColumns += ", ";
                            SqlValues += ", ";
                        }
                        HandledColumns.Add(this._PrimaryKey[i]);
                        SqlColumns += this._PrimaryKey[i];
                        SqlValues += this.SqlValue(this._PrimaryKey[i], _ForeignKeyValues);
                        ColumnValues.Add(this._PrimaryKey[i], this.SqlValue(this._PrimaryKey[i], _ForeignKeyValues));
                    }
                }
                else if (this._PKisPresentInDestination && !this._PKMatchesRowGUIDInDestination && this._IdentityColumn.Length == 0)
                {
                    this._RelationOfData = DataRelation.Conflict;
                    this._Message = "Same key with different RowGUID: \r\n";
                    for (int i = 0; i < this._PrimaryKey.Count; i++)
                    {
                        this._Message += this._PrimaryKey[i] + ": " + this._DataRow[this._PrimaryKey[i]].ToString() + "\r\n";
                    }
                    this._Message += "\r\n";
                    this._RelationOfData = DataRelation.Error;
                    return false;
                }
                else if (this._PKisPresentInDestination && !this._PKMatchesRowGUIDInDestination && this._IdentityColumn.Length > 0)
                {
                    HandledColumns.Add(this._IdentityColumn);
                }
                for (int i = 0; i < this._PrimaryKey.Count; i++)
                {
                    if (HandledColumns.Contains(this._PrimaryKey[i])) continue;
                    if (this._PrimaryKey[i].ToLower() == "loginsertedwhen"
                        || this._PrimaryKey[i].ToLower() == "logcreatedwhen"
                        || this._PrimaryKey[i].ToLower() == "logupdatedwhen")
                        continue;
                    if (!this._ColumnDictionary.ContainsKey(this._PrimaryKey[i])) continue;
                    if (this._PrimaryKey[i] == this._IdentityColumn && !this._PKisPresentInDestination) continue;
                    if (SqlColumns.IndexOf(this._PrimaryKey[i] + ", ") > -1 || SqlColumns.EndsWith(", " + this._PrimaryKey[i])) continue;
                    if (i > 0 || SqlColumns.Length > 0)
                    {
                        SqlColumns += ", ";
                        SqlValues += ", ";
                    }
                    HandledColumns.Add(this._PrimaryKey[i]);
                    SqlColumns += this._PrimaryKey[i];
                    SqlValues += this.SqlValue(this._PrimaryKey[i], _ForeignKeyValues);
                    ColumnValues.Add(this._PrimaryKey[i], this.SqlValue(this._PrimaryKey[i], _ForeignKeyValues));
                }
                for (int i = 0; i < this._DataColumns.Count; i++)
                {
                    if (HandledColumns.Contains(this._DataColumns[i])) continue;
                    if (this._DataColumns[i].ToLower() == "loginsertedwhen"
                        || this._DataColumns[i].ToLower() == "logcreatedwhen"
                        || this._DataColumns[i].ToLower() == "logupdatedwhen")
                        continue;
                    if (!this._ColumnDictionary.ContainsKey(this._DataColumns[i])) continue;
                    if (this._DataRow[this._DataColumns[i]].Equals(System.DBNull.Value)) continue;
                    if (i > 0 || SqlColumns.Length > 0)
                    {
                        SqlColumns += ", ";
                        SqlValues += ", ";
                    }
                    HandledColumns.Add(this._DataColumns[i]);
                    SqlColumns += this._DataColumns[i];
                    SqlValues += this.SqlValue(this._DataColumns[i], _ForeignKeyValues);
                    ColumnValues.Add(this._DataColumns[i], this.SqlValue(this._DataColumns[i], _ForeignKeyValues));
                }
                if (SqlColumns.StartsWith(","))
                    SqlColumns = SqlColumns.Substring(1);
                if (SqlValues.StartsWith(","))
                    SqlValues = SqlValues.Substring(1);
                SqlInsert += " INSERT INTO [" + this._TableName + "] (" + SqlColumns + ") VALUES (" + SqlValues + ")";
                if (this._IdentityColumn.Length > 0
                    && ((!this._PKisPresentInSource && this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Merge)
                    || (!this._PKisPresentInDestination)))
                {
                    SqlInsert = "SET IDENTITY_INSERT [" + this._TableName + "] ON;" + SqlInsert + "; SET IDENTITY_INSERT [" + this._TableName + "] OFF;";
                }
                if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Merge &&
                    !this._RowGUIDisPresentInSource && !(bool)this._RowGUIDhasBeenDeletedInSource)
                {
                    if (this.SqlExecuteNonQueryInSource(SqlInsert))
                    {
                        this._RelationOfData = DataRelation.Inserted;
                    }
                    else
                    {
                        this._RelationOfData = DataRelation.Error;
                        OK = false;
                    }
                }
                else
                {
                    if (this.SqlExecuteNonQueryInDestination(SqlInsert))
                    {
                        this._RelationOfData = DataRelation.Inserted;
                    }
                    else
                    {
                        if (this._ConflictType == ConflictType.ForeignKeySameTable)
                        {
                            if (this.ForeignKeySameTableConflicts.Count > 0)
                            {
                                SqlColumns = "";
                                SqlValues = "";
                                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in ColumnValues)
                                {
                                    if (this.ForeignKeySameTableConflicts.ContainsKey(KV.Key))
                                        continue;
                                    if (SqlColumns.Length > 0)
                                    {
                                        SqlColumns += ", ";
                                        SqlValues += ", ";
                                    }
                                    SqlColumns += KV.Key;
                                    SqlValues += KV.Value;
                                }
                                SqlInsert = "INSERT INTO [" + this._TableName + "] (" + SqlColumns + ") VALUES (" + SqlValues + ")";
                                if (this._IdentityColumn.Length > 0
                                    && ((!this._PKisPresentInSource && this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Merge)
                                    || (!this._PKisPresentInDestination)))
                                {
                                    SqlInsert = "SET IDENTITY_INSERT [" + this._TableName + "] ON; DECLARE @i int; " + SqlInsert + "; SET IDENTITY_INSERT [" + this._TableName + "] OFF;  SET @i = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]) SELECT @i; SELECT IDENT_CURRENT( '" + this._TableName + "' )";
                                    //string  (this.SqlExecuteScalarInDestination(SQL))
                                }
                                else
                                {
                                    if (this.SqlExecuteNonQueryInDestination(SqlInsert))
                                    {
                                    }
                                    else
                                    {
                                    }
                                }
                            }
                            this._RelationOfData = DataRelation.ForeignKeySameTableConflict;
                        }
                        else
                            this._RelationOfData = DataRelation.Error;
                        OK = false;
                    }
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
                //if (SqlInsert.Length > 0) 
                this._Message += ex.Message + "\r\n\r\n";
                if (SqlInsert.Length > 0)
                    this._Message += "SQL:\r\n" + SqlInsert + "\r\n\r\n";
                this._RelationOfData = DataRelation.Error;
            }
            return OK;
        }

        private System.Collections.Generic.Dictionary<string, string> _ForeignKeySameTableConflicts;

        public System.Collections.Generic.Dictionary<string, string> ForeignKeySameTableConflicts
        {
            get
            {
                if (this._ForeignKeySameTableConflicts == null)
                {
                    this._ForeignKeySameTableConflicts = new Dictionary<string, string>();
                    if (this._ConflictType == ConflictType.ForeignKeySameTable)
                    {
                        foreach (DiversityWorkbench.UserControls.ForeignKey FK in this._ForeignKeys)
                        {
                            if (FK.ForeignTable == FK.TableName)
                            {
                                string Value = this.SqlValue(FK.ColumnName, this._ForeignKeyValues);
                                if (Value.Length > 0)
                                    this._ForeignKeySameTableConflicts.Add(FK.ColumnName, Value);
                            }
                        }
                    }
                }
                return _ForeignKeySameTableConflicts;
            }
            //set { _ForeignKeySameTableConflicts = value; }
        }

        private System.Collections.Generic.Dictionary<string, string> _ForeignKeySameTableKeyValues;

        public System.Collections.Generic.Dictionary<string, string> ForeignKeySameTableKeyValues
        {
            get
            {
                if (_ForeignKeySameTableKeyValues == null)
                    _ForeignKeySameTableKeyValues = new Dictionary<string, string>();
                return _ForeignKeySameTableKeyValues;
            }
            //set { _ForeignKeySameTableKeyValues = value; }
        }

        private string SqlValue(string ColumnName, System.Collections.Generic.Dictionary<string, string> ForeignKeyValues)
        {
            string SqlValues = "";
            if (ColumnName.ToLower() == "loginsertedwhen"
                || ColumnName.ToLower() == "logcreatedwhen")
            {
                if (!this._ColumnDictionary.ContainsKey(ColumnName))
                    return "";
            }
            if (this._DataRow[ColumnName].Equals(System.DBNull.Value)) return "";
            if (SqlValues.Length > 0)
            {
                SqlValues += ", ";
            }
            if (this._ColumnDictionary[ColumnName].ToLower() == "datetime" ||
                this._ColumnDictionary[ColumnName].ToLower() == "datetime2" ||
                this._ColumnDictionary[ColumnName].ToLower() == "smalldatetime" ||
                this._ColumnDictionary[ColumnName].ToLower() == "date")
            {
                SqlValues += "CONVERT(DATETIME, '";
                if (ForeignKeyValues.ContainsKey(ColumnName))
                    SqlValues += ForeignKeyValues[ColumnName];
                else
                    SqlValues += this._DataRow[ColumnName].ToString();
                SqlValues += "', 121)";
            }
            else if (this._ColumnDictionary[ColumnName].ToLower() == "int"
               || this._ColumnDictionary[ColumnName].ToLower() == "tinyint"
               || this._ColumnDictionary[ColumnName].ToLower() == "smallint"
               || this._ColumnDictionary[ColumnName].ToLower() == "int"
               || this._ColumnDictionary[ColumnName].ToLower() == "int")
                if (ForeignKeyValues.ContainsKey(ColumnName))
                    SqlValues += ForeignKeyValues[ColumnName];
                else
                    SqlValues += this._DataRow[ColumnName].ToString();
            else if (this._ColumnDictionary[ColumnName].ToLower() == "float"
               || this._ColumnDictionary[ColumnName].ToLower() == "double")
                if (ForeignKeyValues.ContainsKey(ColumnName))
                    SqlValues += ForeignKeyValues[ColumnName];
                else
                    SqlValues += this._DataRow[ColumnName].ToString().Replace(',', '.');
            else
            {
                string V = "";
                if (ForeignKeyValues.ContainsKey(ColumnName))
                    V = ForeignKeyValues[ColumnName];
                else
                    V = this._DataRow[ColumnName].ToString();
                SqlValues += "'" + DiversityWorkbench.Forms.FormFunctions.SqlRemoveHyphens(V) + "'";
            }
            return SqlValues;
        }

        public bool SetForeignKeySameTableValues()
        {
            bool OK = true;
            string SQL = "";
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ForeignKeySameTableConflicts)
            {
                if (SQL.Length > 0)
                    SQL += ", ";
                SQL += KV.Key + " = " + KV.Value;
            }
            SQL = "UPDATE " + this._TableName + " SET " + SQL + " WHERE ";
            for (int i = 0; i < _PrimaryKey.Count; i++)
            {
                if (i > 0) SQL += ", ";
                SQL += _PrimaryKey[i] + " = " + this.SqlValue(_PrimaryKey[i]);
            }
            OK = this.SqlExecuteNonQueryInDestination(SQL);
            return OK;
        }

        private string SqlValue(string ColumnName)
        {
            string SqlValues = "";
            if (ColumnName.ToLower() == "loginsertedwhen"
                || ColumnName.ToLower() == "logcreatedwhen")
                if (!this._ColumnDictionary.ContainsKey(ColumnName)) return "";
            if (this._DataRow[ColumnName].Equals(System.DBNull.Value)) return "";
            if (SqlValues.Length > 0)
            {
                SqlValues += ", ";
            }
            if (this._ColumnDictionary[ColumnName].ToLower() == "datetime")
            {
                SqlValues += "CONVERT(DATETIME, '";
                SqlValues += this._DataRow[ColumnName].ToString();
                SqlValues += "', 121)";
            }
            else if (this._ColumnDictionary[ColumnName].ToLower() == "int"
               || this._ColumnDictionary[ColumnName].ToLower() == "tinyint"
               || this._ColumnDictionary[ColumnName].ToLower() == "smallint"
               || this._ColumnDictionary[ColumnName].ToLower() == "int"
               || this._ColumnDictionary[ColumnName].ToLower() == "int")
                SqlValues += this._DataRow[ColumnName].ToString();
            else if (this._ColumnDictionary[ColumnName].ToLower() == "float"
               || this._ColumnDictionary[ColumnName].ToLower() == "double")
                SqlValues += this._DataRow[ColumnName].ToString().Replace(',', '.');
            else
            {
                string V = "";
                V = this._DataRow[ColumnName].ToString();
                SqlValues += "'" + DiversityWorkbench.Forms.FormFunctions.SqlRemoveHyphens(V) + "'";
            }
            return SqlValues;
        }

        private bool Update()
        {
            /*Possible situations:
             * both datarows unchanged -> do nothing
             * one datarow changed -> update
             * both datarows changed
             *      get the values for each column and list the differences
             *      for each difference check the logtables
             * */
            bool OK = true;
            string SqlUpdate = "";
            string SqlValues = "";
            string SqlWhere = "";
            try
            {
                if (this.StateOfPublisherRow == DataRowState.NoChanges &&
                    this.StateOfSubscriberRow == DataRowState.NoChanges)
                {
                    this._RelationOfData = DataRelation.NoDifference;
                    return OK;
                }

                if (this.StateOfSubscriberRow == DataRowState.NoChanges &&
                    this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Upload)
                    return OK;

                if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Download &&
                    this.StateOfPublisherRow == DataRowState.NoChanges)
                    return OK;

                if (this.StateOfPublisherRow == DataRowState.Updated &&
                    this.StateOfSubscriberRow == DataRowState.Updated)
                {
                    if (!DiversityWorkbench.UserControls.ReplicationRow.IgnoreConflicts)
                    {
                        bool DifferencesFound = false;
                        if (this.DataRowDestination != null)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._ColumnDictionary)
                            {
                                if (KV.Key == "LogCreatedWhen" || KV.Key == "LogInsertedWhen" ||
                                    KV.Key == "LogCreatedBy" || KV.Key == "LogInsertedBy" ||
                                    KV.Key == "LogUpdatedWhen" || KV.Key == "LogUpdatedBy")
                                    continue;
                                if (this._DataRow[KV.Key].ToString() != this.DataRowDestination[KV.Key].ToString())
                                {
                                    DifferencesFound = true;
                                    break;
                                }
                            }
                        }
                        if (DifferencesFound)
                        {
                            this._RelationOfData = DataRelation.Conflict;
                            return false;
                        }
                        else
                            return OK;
                    }
                }

                SqlUpdate = "UPDATE [" + this._TableName + "] SET ";
                SqlValues = "";
                foreach (string s in this._DataColumns)
                {
                    // Test
                    //if (s.ToLower().StartsWith("log"))
                    //{ }
                    if (s == "RowGUID")
                        continue;
                    // Keep the initial creation date in the publisher
                    if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Upload &&
                        (s == "LogCreatedWhen" || s == "LogInsertedWhen"))
                        continue;
                    // Keep the initial creator
                    if (s == "LogCreatedBy" || s == "LogInsertedBy")
                        continue;
                    if (SqlValues.Length > 0)
                        SqlValues += ", ";
                    SqlValues += s + " = ";
                    if (this._ColumnDictionary[s].ToLower() == "datetime" ||
                        this._ColumnDictionary[s].ToLower() == "datetime2" ||
                        this._ColumnDictionary[s].ToLower() == "date" ||
                        this._ColumnDictionary[s].ToLower() == "smalldatetime")
                    {
                        {
                            if (this._DataRow[s].Equals(System.DBNull.Value))
                                SqlValues += " NULL ";
                            else if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Download &&
                                (s == "LogCreatedWhen" || s == "LogInsertedWhen"))
                            {
                                SqlValues += "CONVERT(DATETIME, '" + System.DateTime.Now.Year.ToString()
                                    + "-" + System.DateTime.Now.Month.ToString()
                                    + "-" + System.DateTime.Now.Day.ToString()
                                    + " " + System.DateTime.Now.Hour.ToString()
                                    + ":" + System.DateTime.Now.Minute.ToString()
                                    + ":" + System.DateTime.Now.Second.ToString()
                                    + "." + System.DateTime.Now.Millisecond.ToString() + "', 121)";
                            }
                            else
                                SqlValues += "CONVERT(DATETIME, '" + this._DataRow[s].ToString() + "', 121)";
                        }
                    }
                    else if (this._ColumnDictionary[s].ToLower() == "int"
                       || this._ColumnDictionary[s].ToLower() == "tinyint"
                       || this._ColumnDictionary[s].ToLower() == "smallint"
                       || this._ColumnDictionary[s].ToLower() == "int"
                       || this._ColumnDictionary[s].ToLower() == "int")
                    {
                        if (this._DataRow[s].Equals(System.DBNull.Value))
                            SqlValues += " NULL ";
                        else
                        {
                            ///MW 27.03.2015: If the key values in the destination differ from the values in the source
                            ///happens e.g. for PK values where Key is the same but RowGUID differs
                            if (this._ForeignKeyValuesForNewDataset.ContainsKey(s))
                                SqlValues += this._ForeignKeyValuesForNewDataset[s];
                            else
                                SqlValues += this._DataRow[s].ToString();
                        }
                    }
                    else if (this._ColumnDictionary[s].ToLower() == "float"
                       || this._ColumnDictionary[s].ToLower() == "double")
                        if (this._DataRow[s].Equals(System.DBNull.Value))
                            SqlValues += " NULL ";
                        else
                            SqlValues += this._DataRow[s].ToString().Replace(',', '.');
                    else
                    {
                        if (this._DataRow[s].Equals(System.DBNull.Value))
                            SqlValues += " NULL ";
                        else
                            SqlValues += "'" + DiversityWorkbench.Forms.FormFunctions.SqlRemoveHyphens(this._DataRow[s].ToString()) + "'";
                    }

                }
                SqlWhere = " RowGUID = '" + this._DataRow["RowGUID"].ToString() + "'";
                SqlUpdate += SqlValues + " WHERE " + SqlWhere;
                if (this.SqlExecuteNonQueryInDestination(SqlUpdate))
                    this._RelationOfData = DataRelation.Updated;
                else
                {
                    this._RelationOfData = DataRelation.Error;
                    OK = false;
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        private bool Delete()
        {
            bool OK = true;
            string SqlDelete = "";
            string SqlWhere = "";
            try
            {
                if (this.StateOfPublisherRow != DataRowState.Deleted && this.StateOfPublisherRow != DataRowState.ToBeDeleted &&
                    this.StateOfSubscriberRow != DataRowState.Deleted && this.StateOfSubscriberRow != DataRowState.ToBeDeleted &&
                    this._ReplicationDirection != UserControlReplicateTable.ReplicationDirection.Clean)
                    return OK;
                else if (this.StateOfPublisherRow == DataRowState.NoChanges &&
                    this.StateOfSubscriberRow == DataRowState.NoChanges &&
                    this._ReplicationDirection != UserControlReplicateTable.ReplicationDirection.Clean)
                    return OK;
                else if (this.StateOfSubscriberRow == DataRowState.NoChanges &&
                    this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Upload &&
                    this._ReplicationDirection != UserControlReplicateTable.ReplicationDirection.Clean)
                    return OK;
                else if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Download &&
                    this.StateOfPublisherRow == DataRowState.NoChanges &&
                    this._ReplicationDirection != UserControlReplicateTable.ReplicationDirection.Clean)
                    return OK;
                else if ((this.StateOfPublisherRow == DataRowState.Updated &&
                    this.StateOfSubscriberRow == DataRowState.Deleted &&
                    this._ReplicationDirection != UserControlReplicateTable.ReplicationDirection.Clean)
                    ||
                    (this.StateOfSubscriberRow == DataRowState.Updated &&
                     this.StateOfPublisherRow == DataRowState.Deleted &&
                    this._ReplicationDirection != UserControlReplicateTable.ReplicationDirection.Clean))
                {
                    this._RelationOfData = DataRelation.Conflict;
                    return false;
                }
                else if ((this.StateOfPublisherRow == DataRowState.NoChanges &&
                    this.StateOfSubscriberRow == DataRowState.Deleted &&
                    this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Upload)
                    ||
                    (this._StateOfSubscriberRow == DataRowState.NoChanges &&
                    this._StateOfPublisherRow == DataRowState.Deleted &&
                    this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Download)
                    ||
                    this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Clean
                    ||
                    (this.StateOfSubscriberRow == DataRowState.ToBeDeleted &&
                    this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Download)
                    ||
                    (this._StateOfPublisherRow == DataRowState.ToBeDeleted &&
                    this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Upload))
                {
                    SqlDelete = "DELETE [" + this._TableName + "] WHERE RowGUID = '" + this.RowGUID + "'";
                    bool IsDeleted = false;
                    if (this._StateOfPublisherRow == DataRowState.ToBeDeleted &&
                        this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Upload)
                        IsDeleted = this.SqlExecuteNonQueryInSource(SqlDelete);
                    if (!IsDeleted && this._StateOfSubscriberRow == DataRowState.ToBeDeleted &&
                        this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Download)
                        IsDeleted = this.SqlExecuteNonQueryInSource(SqlDelete);
                    if (!IsDeleted)
                        IsDeleted = this.SqlExecuteNonQueryInSource(SqlDelete);
                    if (IsDeleted)
                    //if ((this._StateOfPublisherRow == DataRowState.ToBeDeleted &&
                    //    this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Upload &&
                    //    this.SqlExecuteNonQueryInSource(SqlDelete))
                    //    ||
                    //    (this._StateOfSubscriberRow == DataRowState.ToBeDeleted &&
                    //    this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Download &&
                    //    this.SqlExecuteNonQueryInDestination(SqlDelete))
                    //    || 
                    //    (this.SqlExecuteNonQueryInDestination(SqlDelete)))
                    {
                        this._RelationOfData = DataRelation.Deleted;
                        if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Clean)
                            this._StateOfSubscriberRow = DataRowState.Deleted;
                        else if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Download)
                            this._StateOfSubscriberRow = DataRowState.Deleted;
                        else if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Upload)
                            this._StateOfPublisherRow = DataRowState.Deleted;
                    }
                    else
                    {
                        this._RelationOfData = DataRelation.Error;
                        OK = false;
                    }
                }
                else return false;
            }
            catch { OK = false; }
            return OK;
        }

        #endregion

        #region SQL

        private bool SqlFillTableFromDestination(string SQL, ref System.Data.DataTable DT)
        {
            bool OK = true;
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ConnectionDestination);//, this.ConnectionStringDestination);
                ad.Fill(DT);
                //ad.Dispose();
            }
            catch (System.Exception ex) { OK = false; }
            return OK;
        }

        private string SqlExecuteScalarInDestination(string SQL)
        {
            string Result = "";
            string ConnectionString = "";
            try
            {
                if (this.ConnectionDestination != null && this.ConnectionDestination.ConnectionString != null)
                    ConnectionString = this.ConnectionDestination.ConnectionString;
                //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this.ConnectionStringDestination);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.ConnectionDestination);//, con);
                //con.Open();
                Result = C.ExecuteScalar()?.ToString();
                //con.Close();
                //con.Dispose();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "SQL: " + SQL + ". Connection: " + ConnectionString);
            }
            return Result;
        }

        private bool SqlExecuteNonQueryInDestination(string SQL)
        {
            bool OK = true;
            try
            {
                //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this.ConnectionStringDestination);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.ConnectionDestination);//, con);
                //con.Open();
                C.ExecuteNonQuery();
                //con.Close();
                //con.Dispose();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547 && ex.Message.IndexOf("FOREIGN KEY SAME TABLE") > -1)
                {
                    this._ConflictType = ConflictType.ForeignKeySameTable;
                }
                else
                {
                    if (this._Message == null)
                        this._Message = "";
                    this._Message += ex.Message + "\r\n";
                    if (SQL.Length > 0)
                        this._Message += "\r\nSQL:" + SQL + "\r\n\r\n\r\n";
                }
                OK = false;
            }
            catch (System.Exception ex)
            {
                if (this._Message == null)
                    this._Message = "";
                this._Message += ex.Message + "\r\n";
                if (SQL.Length > 0)
                    this._Message += "\r\nSQL:" + SQL + "\r\n\r\n\r\n";
                OK = false;
            }
            return OK;
        }

        //private bool SqlExecuteNonQueryInDestination(string SQL, ref string Message)
        //{
        //    bool OK = true;
        //    try
        //    {
        //        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.ConnectionDestination);
        //        C.ExecuteNonQuery();
        //    }
        //    catch (Microsoft.Data.SqlClient.SqlException ex)
        //    {
        //        if (ex.Number == 547 && ex.Message.IndexOf("FOREIGN KEY SAME TABLE") > -1)
        //        {
        //            Message = "FOREIGN KEY SAME TABLE CONFLICT";
        //        }
        //        if (this._Message == null)
        //            this._Message = "";
        //        this._Message += ex.Message + "\r\n";
        //        if (SQL.Length > 0)
        //            this._Message += "\r\nSQL:" + SQL + "\r\n\r\n\r\n";
        //        OK = false;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        if (this._Message == null)
        //            this._Message = "";
        //        this._Message += ex.Message + "\r\n";
        //        if (SQL.Length > 0)
        //            this._Message += "\r\nSQL:" + SQL + "\r\n\r\n\r\n";
        //        OK = false;
        //    }
        //    return OK;
        //}

        private string SqlExecuteScalarInSource(string SQL)
        {
            string Result = "";
            try
            {
                //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this.ConnectionStringSource);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.ConnectionSource);//, con);
                //con.Open();
                Result = C.ExecuteScalar()?.ToString();
                //con.Close();
                //con.Dispose();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Result;
        }

        private bool SqlExecuteNonQueryInSource(string SQL)
        {
            bool OK = true;
            try
            {
                //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this.ConnectionStringSource);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.ConnectionSource);//, con);
                //con.Open();
                C.ExecuteNonQuery();
                //con.Close();
                //con.Dispose();
            }
            catch (System.Exception ex)
            {
                if (this._Message == null)
                    this._Message = "";
                this._Message += ex.Message + "\r\n";
                if (SQL.Length > 0)
                    this._Message += "\r\nSQL:" + SQL + "\r\n\r\n\r\n";
                OK = false;
            }
            return OK;
        }

        private Microsoft.Data.SqlClient.SqlConnection ConnectionDestination
        {
            get
            {
                if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Upload || this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Merge)
                    return DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher;
                else return DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber;
            }
        }

        private Microsoft.Data.SqlClient.SqlConnection ConnectionSource
        {
            get
            {
                if (this._ReplicationDirection == UserControlReplicateTable.ReplicationDirection.Download)
                    return DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher;
                else return DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber;
            }
        }


        #endregion

        #region LogDateColumns

        private string ColumnUpdateDateInSubscriber
        {
            get
            {
                if (this._ColumnUpdateDateInSubscriber.Length > 0) return this._ColumnUpdateDateInSubscriber;
                if (this._ColumnDictionary.ContainsKey("LogUpdatedWhen")) this._ColumnUpdateDateInSubscriber = "LogUpdatedWhen";
                return this._ColumnUpdateDateInSubscriber;
            }
        }

        private string ColumnInsertDateInSubscriber
        {
            get
            {
                if (this._ColumnInsertDateInSubscriber.Length > 0) return this._ColumnInsertDateInSubscriber;
                if (this._ColumnDictionary.ContainsKey("LogInsertedWhen")) this._ColumnInsertDateInSubscriber = "LogInsertedWhen";
                else if (this._ColumnDictionary.ContainsKey("LogCreatedWhen")) this._ColumnInsertDateInSubscriber = "LogCreatedWhen";
                else
                {
                    this._ColumnInsertDateInSubscriber = this.ColumnInsertDate(true);
                }
                //if (this._ColumnInsertDateInSource.Length == 0) this._ColumnInsertDateInSource = "LogUpdatedWhen";
                return this._ColumnInsertDateInSubscriber;
            }
        }

        private string ColumnUpdateDateInPublisher
        {
            get
            {
                if (this._ColumnUpdateDateInPublisher.Length > 0) return this._ColumnUpdateDateInPublisher;
                if (this._ColumnDictionary.ContainsKey("LogUpdatedWhen")) this._ColumnUpdateDateInPublisher = "LogUpdatedWhen";
                return this._ColumnUpdateDateInPublisher;
            }
        }

        private string ColumnInsertDateInPublisher
        {
            get
            {
                if (this._ColumnInsertDateInPublisher.Length > 0) return this._ColumnInsertDateInPublisher;
                if (this._ColumnDictionary.ContainsKey("LogInsertedWhen")) this._ColumnInsertDateInPublisher = "LogInsertedWhen";
                else if (this._ColumnDictionary.ContainsKey("LogCreatedWhen")) this._ColumnInsertDateInPublisher = "LogCreatedWhen";
                else
                {
                    try
                    {
                        this._ColumnInsertDateInPublisher = this.ColumnInsertDate(false);
                    }
                    catch { }
                }
                return this._ColumnInsertDateInPublisher;
            }
        }

        private string ColumnInsertDate(bool InSubscriber)
        {
            string Column = "";
            string SQL = "SELECT COLUMN_NAME " +
                "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                "WHERE TABLE_NAME = '" + this._TableName + "' AND (COLUMN_NAME IN ('LogCreatedWhen', 'LogInsertedWhen'))";
            try
            {
                //string Connection = this._ConnectionPublisher;
                //if (InSubscriber) Connection = this._ConnectionSubscriber;
                //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(Connection);
                Microsoft.Data.SqlClient.SqlCommand C;// = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                if (InSubscriber)
                    C = new Microsoft.Data.SqlClient.SqlCommand(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber);
                else C = new Microsoft.Data.SqlClient.SqlCommand(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher);
                //con.Open();
                Column = C.ExecuteScalar()?.ToString();
                //con.Close();
                //con.Dispose();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Column;
        }

        #endregion

    }

}
