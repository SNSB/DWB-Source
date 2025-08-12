using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormReplicationConflict : Form
    {
        #region Parameter and Properties

        private int _IndexOfSelectedRow = 0;
        private System.Data.DataTable _DtMerge;
        private System.Data.DataTable _DtPublisher;
        private System.Data.DataTable _DtSubscriber;
        private System.Windows.Forms.DataGridViewCellStyle _StyleReadOnly;
        private System.Windows.Forms.DataGridViewCellStyle _StyleConflict;
        private System.Windows.Forms.DataGridViewCellStyle _StyleWarning;
        private System.Collections.Generic.List<string> _PKcolumns;
        private System.Collections.Generic.List<DiversityWorkbench.UserControls.ForeignKey> _ForeignKeys;
        private System.Collections.Generic.Dictionary<string, string> _ConflictColumns;
        private bool _StopConflictResolution = false;

        private DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection _ReplicationDirection;

        public System.Windows.Forms.DataGridViewCellStyle StyleConflict
        {
            get
            {
                if (this._StyleConflict == null)
                {
                    this._StyleConflict = new DataGridViewCellStyle();
                    this._StyleConflict.BackColor = System.Drawing.Color.Pink;
                    this._StyleConflict.ForeColor = System.Drawing.Color.Black;
                    this._StyleConflict.Tag = "Conflict";
                }
                return this._StyleConflict;
            }
        }

        public System.Windows.Forms.DataGridViewCellStyle StyleWarning
        {
            get
            {
                if (this._StyleWarning == null)
                {
                    this._StyleWarning = new DataGridViewCellStyle();
                    this._StyleWarning.BackColor = System.Drawing.Color.Yellow;
                    this._StyleWarning.SelectionBackColor = System.Drawing.Color.Yellow;
                    this._StyleWarning.ForeColor = System.Drawing.Color.DarkGray;
                    this._StyleWarning.SelectionForeColor = System.Drawing.Color.DarkGray;
                    this._StyleWarning.Tag = "Conflict";
                }
                return this._StyleWarning;
            }
        }

        public System.Windows.Forms.DataGridViewCellStyle StyleReadOnly
        {
            get
            {
                if (this._StyleReadOnly == null)
                {
                    this._StyleReadOnly = new DataGridViewCellStyle();
                    this._StyleReadOnly.BackColor = System.Drawing.SystemColors.ControlLightLight;
                    this._StyleReadOnly.SelectionBackColor = System.Drawing.SystemColors.ControlLightLight;
                    this._StyleReadOnly.ForeColor = System.Drawing.Color.DarkGray;
                    this._StyleReadOnly.SelectionForeColor = System.Drawing.Color.DarkGray;
                    this._StyleReadOnly.Tag = "ReadOnly";
                }
                return this._StyleReadOnly;
            }
        }

        private static int _IgnoredConflicts;
        private static int _SolvedConflicts;
        private static int _LeftConflicts;

        public enum ConflictResolution { Upload, Download, DeleteSubscriber, DeletePublisher, None };
        private ConflictResolution _ConflictResolution = ConflictResolution.None;

        public ConflictResolution ResolutionOfConflict
        {
            get { return _ConflictResolution; }
            //set { _ConflictResolution = value; }
        }

        #endregion

        #region Construction and init

        public FormReplicationConflict(System.Collections.Generic.List<string> PKcolumns,
            System.Collections.Generic.List<DiversityWorkbench.UserControls.ForeignKey> ForeignKeys,
            DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection ReplicationDirection,
            System.Data.DataTable DtPublisher,
            System.Data.DataTable DtSubscriber)
        {
            InitializeComponent();

            this._PKcolumns = PKcolumns;
            this._ForeignKeys = ForeignKeys;
            this._ReplicationDirection = ReplicationDirection;

            if (DiversityWorkbench.Forms.FormReplicationConflict._LeftConflicts < 2)
                this.buttonNextConflict.Visible = false;
            this.setConflictNumbers();
            this._DtPublisher = DtPublisher;
            this._DtSubscriber = DtSubscriber;
            if (this._DtPublisher.Rows.Count == 0 || this._DtSubscriber.Rows.Count == 0)
            {
                this.pictureBoxMerge.Visible = false;
                this.radioButtonMerge.Visible = false;
                this.dataGridViewMerge.Visible = false;
                this.radioButtonPublisher.Enabled = false;
                this.radioButtonSubscriber.Enabled = false;
                if (this._DtSubscriber.Rows.Count == 0)
                {
                    this.buttonDeletePublisher.Visible = true;
                    this.radioButtonSubscriber.Checked = true;
                    //System.Data.DataRow R = this._DtSubscriber.NewRow();
                    //this._DtSubscriber.Rows.Add(R);
                    //this.dataGridViewSubscriber.Visible = false;
                    this.radioButtonPublisher.Enabled = true;
                    this.labelHeader.Text = "Data have been deleted in subscriber. Upload or delete data in publisher?";
                }
                else
                {
                    this.buttonDeleteSubscriber.Visible = true;
                    this.radioButtonPublisher.Checked = true;
                    this.radioButtonSubscriber.Enabled = true;
                    this.labelHeader.Text = "Data have been deleted in publisher. Download or delete data in subscriber?";
                }
            }
            else
            {
                if (this._ReplicationDirection == DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Upload)
                    this._DtMerge = DtSubscriber.Copy();
                else
                    this._DtMerge = DtPublisher.Copy();
            }

            this.dataGridViewPublisher.DataSource = this._DtPublisher;
            this.dataGridViewPublisher.ReadOnly = true;
            this.dataGridViewPublisher.AllowUserToAddRows = false;
            this.dataGridViewPublisher.AllowUserToDeleteRows = false;
            this.dataGridViewPublisher.AllowUserToOrderColumns = false;

            this.dataGridViewMerge.DataSource = this._DtMerge;
            this.dataGridViewMerge.AllowUserToAddRows = false;
            this.dataGridViewMerge.ColumnHeadersVisible = false;

            this.dataGridViewSubscriber.DataSource = this._DtSubscriber;
            this.dataGridViewSubscriber.ReadOnly = true;
            this.dataGridViewSubscriber.AllowUserToAddRows = false;
            this.dataGridViewSubscriber.AllowUserToDeleteRows = false;
            this.dataGridViewSubscriber.AllowUserToOrderColumns = false;
            this.dataGridViewSubscriber.ColumnHeadersVisible = false;

            this.buttonSolveConflict.Enabled = false;

            this.initForm();
        }

        private void initForm()
        {
            this.labelTable.Text = "Tab.: " + this._DtPublisher.TableName;
            int WidthGrid = this.dataGridViewMerge.Width;
            int Columns = this._DtPublisher.Columns.Count - 1;
            System.Collections.Generic.List<int> ConflictPositions = new List<int>();
            if (this._DtPublisher.Rows.Count > 0 && this._DtSubscriber.Rows.Count > 0)
            {
                for (int i = 0; i < this._DtPublisher.Columns.Count; i++)
                {
                    string ValuePublisher = this._DtPublisher.Rows[0][i].ToString();
                    string ValueSubscriber = this._DtSubscriber.Rows[0][i].ToString();
                    if (ValuePublisher != ValueSubscriber)
                    {
                        this.dataGridViewMerge.Columns[i].DefaultCellStyle = this.StyleWarning;
                        this.dataGridViewPublisher.Columns[i].DefaultCellStyle = this.StyleWarning;
                        this.dataGridViewSubscriber.Columns[i].DefaultCellStyle = this.StyleWarning;
                    }
                    if (this._DtPublisher.Columns[i].ColumnName == "RowGUID")
                    {
                        this.dataGridViewPublisher.Columns[i].Visible = false;
                        this.dataGridViewMerge.Columns[i].Visible = false;
                        this.dataGridViewSubscriber.Columns[i].Visible = false;
                        continue;
                    }
                    if (this._DtPublisher.Rows[0][i].Equals(System.DBNull.Value) &&
                        this._DtSubscriber.Rows[0][i].Equals(System.DBNull.Value))
                    {
                        continue;
                    }
                    if (this._DtPublisher.Columns[i].ColumnName.StartsWith("Log"))
                    {
                        continue;
                    }
                    if (this._PKcolumns.Contains(this._DtPublisher.Columns[i].ColumnName))
                    {
                        continue;
                    }
                    foreach (DiversityWorkbench.UserControls.ForeignKey F in this._ForeignKeys)
                    {
                        if (F.ColumnName == this._DtPublisher.Columns[i].ColumnName)
                            continue;
                    }
                    if (ValuePublisher != ValueSubscriber)
                    {
                        ConflictPositions.Add(i);
                        if (ValuePublisher.Length == 0)
                            this.dataGridViewMerge.Rows[0].Cells[i].Value = ValueSubscriber;
                        else if (ValueSubscriber.Length == 0)
                            this.dataGridViewMerge.Rows[0].Cells[i].Value = ValuePublisher;
                        else
                        {
                            if (this._DtPublisher.Columns[i].DataType == System.Type.GetType("System.String") &&
                                !this._DtPublisher.Columns[i].ColumnName.EndsWith("ID"))
                            {
                                System.DateTime DatePublisherUpdate;
                                System.DateTime DateSubscriberUpdate;
                                string Value = "";
                                if (System.DateTime.TryParse(this._DtPublisher.Rows[0]["LogUpdatedWhen"].ToString(), out DatePublisherUpdate) &&
                                    System.DateTime.TryParse(this._DtSubscriber.Rows[0]["LogUpdatedWhen"].ToString(), out DateSubscriberUpdate))
                                {
                                    if (DateSubscriberUpdate > DatePublisherUpdate)
                                    {
                                        Value = ValuePublisher + ". " + ValueSubscriber;
                                    }
                                    else
                                    {
                                        Value = ValueSubscriber + ". " + ValuePublisher;
                                    }
                                    this.dataGridViewMerge.Rows[0].Cells[i].Value = Value;
                                }
                            }
                        }
                        if (this._ConflictColumns == null)
                            this._ConflictColumns = new Dictionary<string, string>();
                        this._ConflictColumns.Add(this._DtMerge.Columns[i].ColumnName, this._DtMerge.Rows[0][i].ToString());
                    }
                }
                int ConflictColumnsWidth = ((int)WidthGrid / Columns) + 100;
                int ColumnWidth = (int)(WidthGrid - (ConflictColumnsWidth * ConflictPositions.Count)) / (Columns - ConflictPositions.Count);
                for (int i = 0; i < this._DtPublisher.Columns.Count; i++)
                {
                    if (ConflictPositions.Contains(i))
                    {
                        this.dataGridViewMerge.Columns[i].Width = ConflictColumnsWidth;
                        this.dataGridViewPublisher.Columns[i].Width = ConflictColumnsWidth;
                        this.dataGridViewSubscriber.Columns[i].Width = ConflictColumnsWidth;

                        this.dataGridViewMerge.Columns[i].DefaultCellStyle = this.StyleConflict;
                        this.dataGridViewPublisher.Columns[i].DefaultCellStyle = this.StyleConflict;
                        this.dataGridViewSubscriber.Columns[i].DefaultCellStyle = this.StyleConflict;
                    }
                    else
                    {
                        this.dataGridViewMerge.Columns[i].Width = ColumnWidth;
                        this.dataGridViewPublisher.Columns[i].Width = ColumnWidth;
                        this.dataGridViewSubscriber.Columns[i].Width = ColumnWidth;

                        this.dataGridViewMerge.Columns[i].ReadOnly = true;
                        this.dataGridViewPublisher.Columns[i].ReadOnly = true;
                        this.dataGridViewSubscriber.Columns[i].ReadOnly = true;

                        if (this._DtPublisher.Rows[0][i].ToString() != this._DtSubscriber.Rows[0][i].ToString())
                        {
                            this.dataGridViewMerge.Columns[i].DefaultCellStyle = this.StyleWarning;
                            this.dataGridViewPublisher.Columns[i].DefaultCellStyle = this.StyleWarning;
                            this.dataGridViewSubscriber.Columns[i].DefaultCellStyle = this.StyleWarning;
                        }
                        else
                        {
                            this.dataGridViewMerge.Columns[i].DefaultCellStyle = this.StyleReadOnly;
                            this.dataGridViewPublisher.Columns[i].DefaultCellStyle = this.StyleReadOnly;
                            this.dataGridViewSubscriber.Columns[i].DefaultCellStyle = this.StyleReadOnly;
                        }
                    }
                }
            }
            else
            {
                if (this._DtPublisher.Rows.Count == 0)
                {
                    System.Data.DataRow R = this._DtPublisher.NewRow();
                    this._DtPublisher.Rows.Add(R);
                }
                if (this._DtSubscriber.Rows.Count == 0)
                {
                    System.Data.DataRow R = this._DtSubscriber.NewRow();
                    this._DtSubscriber.Rows.Add(R);
                }
                int ColumnWidth = (int)(WidthGrid) / (Columns + 1);
                for (int i = 0; i < this._DtPublisher.Columns.Count; i++)
                {
                    if (this._DtPublisher.Columns[i].ColumnName == "RowGUID")
                    {
                        this.dataGridViewPublisher.Columns[i].Visible = false;
                        this.dataGridViewSubscriber.Columns[i].Visible = false;
                        continue;
                    }
                    this.dataGridViewPublisher.Columns[i].Width = ColumnWidth;
                    this.dataGridViewSubscriber.Columns[i].Width = ColumnWidth;
                }

            }
        }

        private void setConflictColumns()
        {
            for (int i = 0; i < this._DtPublisher.Columns.Count; i++)
            {
                string ValuePublisher = this._DtPublisher.Rows[0][i].ToString();
                string ValueSubscriber = this._DtSubscriber.Rows[0][i].ToString();
            }
        }

        private void setConflictNumbers()
        {
            this.textBoxConflictsSolved.Text = DiversityWorkbench.Forms.FormReplicationConflict._SolvedConflicts.ToString();
            this.textBoxIgnoredConflicts.Text = DiversityWorkbench.Forms.FormReplicationConflict._IgnoredConflicts.ToString();
            this.textBoxNumberOfConflicts.Text = DiversityWorkbench.Forms.FormReplicationConflict._LeftConflicts.ToString();
        }
        #endregion

        #region Public functions

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        #endregion


        #region Interface

        public static int NumberOfConflicts
        {
            set
            {
                DiversityWorkbench.Forms.FormReplicationConflict._IgnoredConflicts = 0;
                DiversityWorkbench.Forms.FormReplicationConflict._SolvedConflicts = 0;
                DiversityWorkbench.Forms.FormReplicationConflict._LeftConflicts = value;
            }
        }

        public bool StopConflictResolution
        {
            get { return _StopConflictResolution; }
        }

        public System.Data.DataRow SelectedRow
        {
            get
            {
                if (this.radioButtonMerge.Checked) return this._DtMerge.Rows[0];
                else if (this.radioButtonPublisher.Checked) return this._DtPublisher.Rows[0];
                else if (this.radioButtonSubscriber.Checked) return this._DtSubscriber.Rows[0];
                else
                {
                    return null;
                }
            }
        }

        public DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection ReplicationDirection
        {
            get
            {
                if (this.radioButtonMerge.Checked) return DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Merge;
                else if (this.radioButtonPublisher.Checked) return DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Download;
                else if (this.radioButtonSubscriber.Checked) return DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Upload;
                else return DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Clean;
            }
        }

        /// <summary>
        /// returns the colums with the correct content
        /// </summary>
        public System.Collections.Generic.Dictionary<string, string> ConflictColumns
        {
            get
            {
                if (this.radioButtonMerge.Checked)
                    return _ConflictColumns;
                else
                {
                    System.Collections.Generic.Dictionary<string, string> CC = new Dictionary<string, string>();
                    if (this._ConflictColumns != null)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._ConflictColumns)
                        {
                            string Value = "";
                            if (this.radioButtonPublisher.Checked)
                                Value = this._DtPublisher.Rows[0][KV.Key].ToString();
                            else if (this.radioButtonSubscriber.Checked)
                                Value = this._DtSubscriber.Rows[0][KV.Key].ToString();
                            CC.Add(KV.Key, Value);
                        }
                    }
                    return CC;
                }
            }
        }

        #endregion

        #region Controls

        private void radioButtonPublisher_CheckedChanged(object sender, EventArgs e)
        {
            this.buttonSolveConflict.Enabled = true;
        }

        private void radioButtonMerge_CheckedChanged(object sender, EventArgs e)
        {
            this.buttonSolveConflict.Enabled = true;
        }

        private void radioButtonSubscriber_CheckedChanged(object sender, EventArgs e)
        {
            this.buttonSolveConflict.Enabled = true;
        }

        private void buttonStopConflictResolution_Click(object sender, EventArgs e)
        {
            this._StopConflictResolution = true;
            this.DialogResult = DialogResult.Cancel;
            DiversityWorkbench.Forms.FormReplicationConflict._IgnoredConflicts++;
            DiversityWorkbench.Forms.FormReplicationConflict._LeftConflicts--;
            this.Close();
        }

        private void buttonDeletePublisher_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this._ConflictResolution = ConflictResolution.DeletePublisher;
            DiversityWorkbench.Forms.FormReplicationConflict._SolvedConflicts++;
            DiversityWorkbench.Forms.FormReplicationConflict._LeftConflicts--;
            this.Close();
        }

        private void buttonDeleteSubscriber_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this._ConflictResolution = ConflictResolution.DeleteSubscriber;
            DiversityWorkbench.Forms.FormReplicationConflict._SolvedConflicts++;
            DiversityWorkbench.Forms.FormReplicationConflict._LeftConflicts--;
            this.Close();
        }

        private void buttonNextConflict_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            DiversityWorkbench.Forms.FormReplicationConflict._IgnoredConflicts++;
            DiversityWorkbench.Forms.FormReplicationConflict._LeftConflicts--;
            this.Close();
        }

        private void buttonSolveConflict_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            if (this._ReplicationDirection == DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Upload &&
                this.radioButtonSubscriber.Checked)
                this._ConflictResolution = ConflictResolution.Upload;
            else if (this._ReplicationDirection == DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Download &&
                this.radioButtonPublisher.Checked)
                this._ConflictResolution = ConflictResolution.Download;
            //else if (this._ReplicationDirection == DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Merge &&
            //    this.radioButtonMerge.Checked)
            //    this._ConflictResolution = ConflictResolution.Merge;

            DiversityWorkbench.Forms.FormReplicationConflict._SolvedConflicts++;
            DiversityWorkbench.Forms.FormReplicationConflict._LeftConflicts--;
            this.Close();
        }

        #endregion

    }
}
