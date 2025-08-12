using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormRemoveSpecimen : Form
    {

        #region Parameter

        private System.Collections.Generic.List<int> _IDs;
        private System.Collections.Generic.Dictionary<int, string> _Errors;
        private System.Collections.Generic.List<string> _LogTablesSpecimen;
        private System.Collections.Generic.List<string> _LogTablesEvent;
        private System.Collections.Generic.List<string> _TablesEvent;

        #endregion

        #region Construction

        public FormRemoveSpecimen(System.Collections.Generic.List<int> IDs)
        {
            InitializeComponent();
            this.labelHeader.Text = "Remove all " + IDs.Count.ToString() + " specimen (incl. coll. events) from the database?";
            this._IDs = IDs;
            this.progressBar.Maximum = this._IDs.Count;
            this.progressBar.Minimum = 0;
            this.progressBar.Value = 0;
            this.initForm();
        }
        
        #endregion

        #region From

        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        private DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get
            {
                if (this._FormFunctions == null)
                    this._FormFunctions = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                return this._FormFunctions;
            }
        }

        private void initForm()
        {
            bool OK = this.FormFunctions.getObjectPermissions("CollectionSpecimen_log", "DELETE");
            this.checkBoxRemoveLog.Enabled = OK;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            string Message = "Do you really want to delete all selected specimen?\r\nLog data will ";
            if (this.RemoveLog) Message += "be removed as well";
            else Message += "NOT be removed";
            if (System.Windows.Forms.MessageBox.Show(Message, "Remove?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                foreach (int ID in this._IDs)
                {
                    try
                    {
                        int? CollectionEventID = this.CollectionEventID(ID);
                        if (this.RemoveSpecimenData(ID))
                        {
                            if (this._StopRemoval)
                                break;
                            if (CollectionEventID != null && this.checkBoxRemoveEvents.Checked)
                                this.RemoveEventData((int)CollectionEventID, ID);
                        }
                        if (this._StopRemoval)
                            break;
                    }
                    catch (Exception ex)
                    {
                    }
                    if (this.progressBar.Value < this.progressBar.Maximum)
                        this.progressBar.Value++;
                }
                if (this.Errors.Count > 0)
                {
                    int Success = this._IDs.Count - this.Errors.Count;
                    string ErrorMessage = Success.ToString() + " specimen of " + this._IDs.Count.ToString() + " removed.\r\n\r\nErrors:\r\n";
                    this.labelHeader.Text = ErrorMessage;
                    ErrorMessage = "";
                    foreach (System.Collections.Generic.KeyValuePair<int, string> KV in this.Errors)
                    {
                        ErrorMessage += "\r\nID " + KV.Key.ToString() + ": " + KV.Value;
                    }
                    this.labelErrorReport.Text = ErrorMessage;
                    int MaxHeight = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
                    int MaxWidth = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
                    if (this.Height + (30 * this.Errors.Count) + 200 < MaxHeight &&
                        this.Width + (30 * this.Errors.Count) + 200 < MaxWidth)
                    {
                        this.Height += (30 * this.Errors.Count) + 100;
                        this.Width += (30 * this.Errors.Count) + 100;
                    }
                    else
                        this.WindowState = FormWindowState.Maximized;
                    //DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Errors", ErrorMessage, true);
                    //f.ShowDialog();
                }
                else
                    this.labelHeader.Text = this._IDs.Count.ToString() + " specimen removed";
                this.buttonStart.Enabled = false;
            }
        }
        
        #endregion

        #region Data handling

        private bool _StopRemoval = false;
        
        private bool RemoveSpecimenData(int CollectionSpecimenID)
        {
            bool OK = true;
            System.Collections.Generic.List<string> Tables = new List<string>();
            Tables.Add("CollectionSpecimenProcessingMethodParameter");
            Tables.Add("CollectionSpecimenProcessingMethod");
            Tables.Add("CollectionSpecimenProcessing");
            Tables.Add("IdentificationUnitAnalysisMethodParameter");
            Tables.Add("IdentificationUnitAnalysisMethod");
            Tables.Add("IdentificationUnitAnalysis");
            Tables.Add("IdentificationUnitInPart");
            Tables.Add("CollectionSpecimen");
            foreach (string T in Tables)
            {
                if (this.RemoveDataFromTable(T, "CollectionSpecimenID", CollectionSpecimenID, CollectionSpecimenID))
                {
                    if (this._StopRemoval)
                        break;
                    if (this.RemoveLog)
                        this.RemoveSpecimenLogData(CollectionSpecimenID);
                    if (this._StopRemoval)
                        break;
                }
                else { OK = false; }
            }
            return OK;
        }

        private bool RemoveSpecimenLogData(int CollectionSpecimenID)
        {
            bool OK = true;
            foreach (string s in this.TablesSpecimenLog)
            {
                if (!this.RemoveDataFromTable(s, "CollectionSpecimenID", CollectionSpecimenID, CollectionSpecimenID))
                    OK = false;
                if (this._StopRemoval)
                    break;
                //if (!OK
                //    && System.Windows.Forms.MessageBox.Show("Deleting specimen with ID = " + CollectionSpecimenID.ToString() + " failed. Stop removal?", "Stop removal?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                //{
                //    break;
                //}
            }
            return OK;
        }

        private bool RemoveEventData(int CollectionEventID, int CollectionSpecimenID)
        {
            bool OK = true;
            foreach (string s in this.TablesEvent)
            {
                if (!this.RemoveDataFromTable(s, "CollectionEventID", CollectionEventID, CollectionSpecimenID))
                    OK = false;
            }
            if (this.RemoveLog && OK)
                this.RemoveEventLogData(CollectionEventID, CollectionSpecimenID);
            return OK;
        }

        private bool RemoveEventLogData(int CollectionEventID, int CollectionSpecimenID)
        {
            bool OK = true;
            foreach (string s in this.TablesEventLog)
            {
                if (!this.RemoveDataFromTable(s, "CollectionEventID", CollectionEventID, CollectionSpecimenID))
                {
                    OK = false;
                    if (this._StopRemoval)
                        break;
                    //if (!OK 
                    //    && System.Windows.Forms.MessageBox.Show("Deleting specimen with ID = " + CollectionSpecimenID.ToString() + " failed. Stop removal?", "Stop removal?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    //{
                    //    break;
                    //}
                }
            }
            return OK;
        }
        
        private bool RemoveDataFromTable(string TableName, string KeyColumnName, int Key, int CollectionSpecimenID)
        {
            bool OK = true;
            string SQL = "DELETE FROM " + TableName + " WHERE " + KeyColumnName + " = " + Key.ToString();
            string ErrorMessage = "";
            if (!DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref ErrorMessage))
            {
                OK = false;
                if (!this.Errors.ContainsKey(CollectionSpecimenID))
                    this.Errors.Add(CollectionSpecimenID, ErrorMessage);
                if (System.Windows.Forms.MessageBox.Show("Deleting specimen with ID = " + CollectionSpecimenID.ToString() + " failed. Stop removal?", "Stop removal?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    OK = false;
                    this._StopRemoval = true;
                }
            }
            return OK;
        }

        private void checkBoxRemoveEvents_CheckedChanged(object sender, EventArgs e)
        {
            this.setHeaderText();
        }

        private void checkBoxRemoveLog_CheckedChanged(object sender, EventArgs e)
        {
            this.setHeaderText();
        }

        private void setHeaderText()
        {
            this.labelHeader.Text = "Remove all " + this._IDs.Count.ToString() + " specimen";
            if (this.checkBoxRemoveEvents.Checked || this.checkBoxRemoveLog.Checked)
                this.labelHeader.Text += " (incl.";
            if (this.checkBoxRemoveEvents.Checked)
                this.labelHeader.Text += " coll. events";
            if (this.checkBoxRemoveEvents.Checked && this.checkBoxRemoveLog.Checked)
                this.labelHeader.Text += " and";
            if (this.checkBoxRemoveLog.Checked)
                this.labelHeader.Text += " log";
            if (this.checkBoxRemoveEvents.Checked || this.checkBoxRemoveLog.Checked)
                this.labelHeader.Text += ")";
            this.labelHeader.Text += " from the database?";
        }

        #endregion

        #region Properties
       
        private bool RemoveLog { get { return this.checkBoxRemoveLog.Checked; } }

        private System.Collections.Generic.Dictionary<int, string> Errors
        {
            get
            {
                if (this._Errors == null)
                    this._Errors = new Dictionary<int, string>();
                return this._Errors;
            }
        }

        private int? CollectionEventID(int CollectionSpecimenID)
        {
            int i = 0;
            string SQL = "SELECT CollectionEventID " +
                "FROM         CollectionSpecimen " +
                "WHERE     (CollectionSpecimenID = " + CollectionSpecimenID.ToString() + ") ";
            if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out i))
            {
                SQL = "SELECT CollectionEventID " +
                    "FROM         CollectionSpecimen " +
                    "WHERE     (CollectionEventID = " + i.ToString() + ") " +
                    "GROUP BY CollectionEventID " +
                    "HAVING      (COUNT(*) = 1)";
                if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out i))
                    return i;
                else return null;
            }
            else return null;
        }

        private System.Collections.Generic.List<string> TablesSpecimenLog
        {
            get
            {
                if (this._LogTablesSpecimen == null)
                {
                    this._LogTablesSpecimen = new List<string>();
                    System.Data.DataTable dt = new DataTable();
                    string SQL = "select TABLE_NAME from INFORMATION_SCHEMA.TABLES " +
                        "where TABLE_NAME like '%log' " +
                        "and(TABLE_NAME like 'Identification%' " +
                        "or TABLE_NAME like 'CollectionSpecimen%' " +
                        "or TABLE_NAME like 'CollectionAgent%')";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    foreach (System.Data.DataRow R in dt.Rows)
                        this._LogTablesSpecimen.Add(R[0].ToString());
                }
                return this._LogTablesSpecimen;
            }
        }

        private System.Collections.Generic.List<string> TablesEventLog
        {
            get
            {
                if (this._LogTablesEvent == null)
                {
                    this._LogTablesEvent = new List<string>();
                    System.Data.DataTable dt = new DataTable();
                    string SQL = "select TABLE_NAME from INFORMATION_SCHEMA.TABLES " +
                        "where TABLE_NAME like '%event%log' " +
                        "and TABLE_NAME not like '%Series%'";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    foreach (System.Data.DataRow R in dt.Rows)
                        this._LogTablesEvent.Add(R[0].ToString());
                }
                return this._LogTablesEvent;
            }
        }

        private System.Collections.Generic.List<string> TablesEvent
        {
            get
            {
                if (this._TablesEvent == null)
                {
                    this._TablesEvent = new List<string>();
                    this._TablesEvent.Add("CollectionEventParameterValue");
                    this._TablesEvent.Add("CollectionEventMethod");
                    this._TablesEvent.Add("CollectionEventImage");
                    this._TablesEvent.Add("CollectionEventLocalisation");
                    this._TablesEvent.Add("CollectionEventProperty");
                    this._TablesEvent.Add("CollectionEvent");
                }
                return this._TablesEvent;
            }
        }

        #endregion

    }
}
