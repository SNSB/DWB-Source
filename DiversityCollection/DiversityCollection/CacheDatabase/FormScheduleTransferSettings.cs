using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.CacheDatabase
{

    public partial class FormScheduleTransferSettings : Form
    {

        #region Parameter

        private int? _ProjectID = null;
        private int? _TargetID = null;
        //private bool _ForPostgres = false;
        private string _SourceTable = "ProjectPublished";
        private string _SourceView = "";
        private string _Target = "";
        private string _Project = "";
        #endregion

        #region Construction

        public FormScheduleTransferSettings(string SourceTable, string Project, int ProjectID)
        {
            InitializeComponent();
            this.Text = Project + ": " + this.Text;
            this._ProjectID = ProjectID;
            this._Project = Project;
            this._SourceTable = SourceTable;
            this.initForm();
        }

        public FormScheduleTransferSettings(string SourceTable, int TargetID, string Project, int ProjectID)
        {
            InitializeComponent();
            this.Text = Project + ": " + this.Text;
            this._ProjectID = ProjectID;
            this._Project = Project;
            this._TargetID = TargetID;
            this._SourceTable = SourceTable;
            this.initForm();
        }

        public FormScheduleTransferSettings(string SourceTable, string SourceView, string Target)
        {
            InitializeComponent();
            this._SourceTable = SourceTable;
            this._SourceView = SourceView;
            this._Target = Target;
            this.initForm();
        }

        #endregion

        #region Form and events

        private void initForm()
        {
            try
            {
                this.Text = "Source: " + this._SourceTable;
                if (this._SourceView.Length > 0) this.Text = this._SourceView;
                if (this._Project.Length > 0) this.Text = this._Project;

                string SQL = "SELECT TransferDays, TransferTime, TransferProtocol, TransferErrors, LastUpdatedWhen, TransferIsExecutedBy ";
                SQL += " FROM " + this._SourceTable + " WHERE  1 = 1 ";
                if (this._SourceView.Length > 0)
                    SQL += " AND SourceView = '" + this._SourceView + "'";
                if (this._Target.Length > 0)
                    SQL += " AND Target = '" + this._Target + "'";
                if (this._ProjectID != null)
                    SQL += " AND ProjectID = " + this._ProjectID.ToString();
                if (this._TargetID != null)
                    SQL += " AND TargetID = " + DiversityCollection.CacheDatabase.CacheDB.TargetID().ToString();
                System.Data.DataTable dt = new DataTable();
                string Message = "";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
                if (dt.Rows.Count > 0)
                {
                    string Days = dt.Rows[0]["TransferDays"].ToString();

                    if (Days.IndexOf("0") > -1) this.checkBoxSunday.Checked = true;
                    else this.checkBoxSunday.Checked = false;

                    if (Days.IndexOf("1") > -1) this.checkBoxMonday.Checked = true;
                    else this.checkBoxMonday.Checked = false;

                    if (Days.IndexOf("2") > -1) this.checkBoxTuesday.Checked = true;
                    else this.checkBoxTuesday.Checked = false;

                    if (Days.IndexOf("3") > -1) this.checkBoxWednesday.Checked = true;
                    else this.checkBoxWednesday.Checked = false;

                    if (Days.IndexOf("4") > -1) this.checkBoxThursday.Checked = true;
                    else this.checkBoxThursday.Checked = false;

                    if (Days.IndexOf("5") > -1) this.checkBoxFriday.Checked = true;
                    else this.checkBoxFriday.Checked = false;

                    if (Days.IndexOf("6") > -1) this.checkBoxSaturday.Checked = true;
                    else this.checkBoxSaturday.Checked = false;

                    string Time = dt.Rows[0]["TransferTime"].ToString();
                    if (Time.Length > 0)
                    {
                        int Hour = int.Parse(Time.Substring(0, 2));
                        int Min = int.Parse(Time.Substring(3, 2));
                        int Sec = int.Parse(Time.Substring(6, 2));
                        System.DateTime DT = new DateTime(2000, 1, 1, Hour, Min, Sec);//
                        this.dateTimePickerTimerTime.Value = DT;
                    }
                    if (!dt.Rows[0]["TransferProtocol"].Equals(System.DBNull.Value))
                        this.textBoxProtocol.Text = dt.Rows[0]["TransferProtocol"].ToString();
                    if (!dt.Rows[0]["TransferErrors"].Equals(System.DBNull.Value) && dt.Rows[0]["TransferErrors"].ToString().Length > 0)
                    {
                        this.textBoxTransferErros.Text = dt.Rows[0]["TransferErrors"].ToString();
                        this.textBoxTransferErros.Visible = true;
                        this.labelTransferErrors.Visible = true;
                        this.buttonTransferErrorsClear.Visible = true;
                    }
                    else if (!dt.Rows[0]["TransferIsExecutedBy"].Equals(System.DBNull.Value) && dt.Rows[0]["TransferIsExecutedBy"].ToString().Length > 0)
                    {
                        this.textBoxTransferErros.Text = dt.Rows[0]["TransferIsExecutedBy"].ToString();
                        this.textBoxTransferErros.Visible = true;
                        this.labelTransferErrors.Text = "Active transfer detected";
                        this.labelTransferErrors.Visible = true;
                        this.buttonTransferErrorsClear.Visible = true;
                    }
                    if (!dt.Rows[0]["LastUpdatedWhen"].Equals(System.DBNull.Value))
                        this.labelProtocol.Text += ": " + dt.Rows[0]["LastUpdatedWhen"].ToString();
                }
                else
                    this.Close();

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        private void FormScheduleTransferSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                string Days = "";
                if (this.checkBoxSunday.Checked) Days += "0";
                if (this.checkBoxMonday.Checked) Days += "1";
                if (this.checkBoxTuesday.Checked) Days += "2";
                if (this.checkBoxWednesday.Checked) Days += "3";
                if (this.checkBoxThursday.Checked) Days += "4";
                if (this.checkBoxFriday.Checked) Days += "5";
                if (this.checkBoxSaturday.Checked) Days += "6";
                string SQL = "UPDATE PT SET PT.TransferDays = '" + Days + "', " +
                    " PT.TransferTime = '" + this.dateTimePickerTimerTime.Value.Hour.ToString() +
                    ":" + this.dateTimePickerTimerTime.Value.Minute.ToString() +
                    ":" + this.dateTimePickerTimerTime.Value.Second.ToString() + "' ";
                SQL += " FROM " + this._SourceTable + " AS PT WHERE 1 = 1 ";
                if (this._ProjectID != null)
                    SQL += " AND ProjectID = " + this._ProjectID.ToString();
                if (this._Target.Length > 0)
                    SQL += " AND Target = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
                if (this._TargetID != null)
                    SQL += " AND TargetID = " + DiversityCollection.CacheDatabase.CacheDB.TargetID().ToString();
                if (this._SourceView.Length > 0)
                    SQL += " AND SourceView = '" + this._SourceView + "'";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            }
        }

        private void buttonTransferErrorsClear_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Do you really want to delete the error protocol and the entry of the current transfer?", "Delete", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                string SQL = "UPDATE PT SET PT.TransferErrors = NULL, PT.TransferIsExecutedBy = NULL FROM " + this._SourceTable + " PT WHERE 1 = 1 ";
                if (this._ProjectID != null)
                {
                    SQL += " AND ProjectID = " + this._ProjectID.ToString();
                }
                if (this._Target.Length > 0)
                    SQL += " AND Target = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
                if (this._SourceView.Length > 0)
                    SQL += " AND SourceView = '" + this._SourceView + "'";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                {
                    this.labelTransferErrors.Visible = false;
                    this.textBoxTransferErros.Visible = false;
                    this.buttonTransferErrorsClear.Visible = false;
                }
            }
        }
        
        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
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
