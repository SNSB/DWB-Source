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
    public partial class FormCacheDatabaseTransfer : Form
    {

        #region Parameter

        //bool _AutoTransferDataAndClose = false;
        System.Collections.Generic.List<DiversityCollection.CacheDatabase.TransferStep> _TransferSteps;
        bool _ForPostgres;
        bool _ForPackage;
        private string _Report = "";
        private string _Errors = "";
        private bool _DataHaveBeenTransferred = false;
        private bool _AllDataHaveBeenTransferred = false;
        private System.Collections.Generic.Dictionary<string, object> _TransferHistory = new Dictionary<string, object>();
        public System.Collections.Generic.Dictionary<string, object> TransferHistory
        {
            get { return _TransferHistory; }
            //set { _TransferHistory = value; }
        }

        private int? _ProjectID = null;
        public void setProjectID(int ProjectID) { this._ProjectID = ProjectID; }

        private string _TransferDirectory = "";

        #endregion

        #region Construction

        public FormCacheDatabaseTransfer(System.Collections.Generic.List<DiversityCollection.CacheDatabase.TransferStep> TransferSteps, string TransferDirectory = "")//, bool AutoTransferDataAndClose)
        {
            InitializeComponent();
            this._TransferDirectory = TransferDirectory;
            if (TransferSteps.Count == 0)
                this.Close();
            try
            {
                this._ForPostgres = TransferSteps[0].ForPostgres;
                this._ForPackage = TransferSteps[0].ForPackage;
                //this._AutoTransferDataAndClose = AutoTransferDataAndClose;
                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)// !AutoTransferDataAndClose)
                    this.setFormTextsAndImages(TransferSteps);

                if (!this._ForPackage)
                    this.labelTarget.Text = TransferSteps[0].Schema;
                if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)//AutoTransferDataAndClose) 
                    this.ShowInTaskbar = true;
                this._TransferSteps = TransferSteps;
                this.panelTransferSteps.Controls.Clear();
                foreach (DiversityCollection.CacheDatabase.TransferStep T in _TransferSteps)
                {
                    DiversityCollection.CacheDatabase.UserControlTransfer U = new UserControlTransfer(T);
                    T.I_Transfer = U;
                    this.panelTransferSteps.Controls.Add(U);
                    U.Dock = DockStyle.Top;
                    U.BringToFront();
                }
                if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)//AutoTransferDataAndClose)
                {
                    this.TransferData();
                    this.Close();
                }
                this.initLogEventControls();
            }
            catch (System.Exception ex)
            {
            }
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

        public string Report() { return this._Report; }
        public string Errors() { return this._Errors; }

        public bool DataHaveBeenTransferred { get { return this._DataHaveBeenTransferred; } }
        public bool AllDataHaveBeenTransferred { get { return this._AllDataHaveBeenTransferred; } }

        public void SetBulkTransferIcon() { this.pictureBoxBulkTransfer.Visible = true; }

        #endregion

        #region Transfer

        private void buttonStartTransfer_Click(object sender, EventArgs e)
        {
            string CompetingTransfer = this.CompetingTransfer();
            if (CompetingTransfer.Length > 0)
            {
                System.Windows.Forms.MessageBox.Show("Competing transfer:\r\n\r\n" + CompetingTransfer + "\r\n\r\nThis transfer must be finished before you can start the transfer", "Competing transfer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                this.buttonStartTransfer.Enabled = false;
                this._Report = "";
                this._AllDataHaveBeenTransferred = true;
                foreach (DiversityCollection.CacheDatabase.TransferStep T in _TransferSteps)
                {
                    if (T.DoTransferData)
                    {
                        T.I_Transfer.SetTransferState(TransferStep.TransferState.NotStarted);
                        this._Report += T.Report();
                        Application.DoEvents();
                        this._DataHaveBeenTransferred = true;
                    }
                    else
                        this._AllDataHaveBeenTransferred = false;
                }
                string Message = "Data transferred";
                if (this.TransferData())
                {
                    this.labelHaeder.Text += " successfull";
                    Message += " successfull";
                }
                else
                {
                    this.labelHaeder.Text += " finished with errors";
                    Message += " with errors";
                }
                System.Windows.Forms.MessageBox.Show(Message);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                System.Windows.Forms.MessageBox.Show("Transfer failed: " + ex.Message);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                this.buttonStartTransfer.Enabled = true;
            }
        }

        private string CompetingTransfer()
        {
            string CurrentTransfer = "";
            if (this._ProjectID != null)
            {
                try
                {
                    string SQL = "SELECT TransferIsExecutedBy " +
                        "FROM dbo.ProjectPublished AS P WHERE P.ProjectID = " + this._ProjectID.ToString();
                    CurrentTransfer = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                    SQL = "SELECT '. By: ' + SUSER_SNAME() + '. On: " + System.Windows.Forms.SystemInformation.ComputerName.ToString() + "'";
                    string SameUser = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                    if (CurrentTransfer.EndsWith(SameUser))
                        CurrentTransfer = "";
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            return CurrentTransfer;
        }

        private bool TransferData()
        {
            bool OK = true;
            try
            {
                System.DateTime Start = System.DateTime.Now;
                this._TransferHistory.Clear();
                if (this._TransferDirectory.Length > 0 && _TransferSteps[0].PostgresUseBulkTransfer)
                {
                    string SQL = "";
                    if (!this.PostgresTransferViaFile_InitExport(this._TransferDirectory, ref this._Errors, ref SQL))
                    {
                        this._Errors += "\r\n" + SQL;
                        OK = false;
                        return OK;
                    }
                }
                foreach (DiversityCollection.CacheDatabase.TransferStep T in _TransferSteps)
                {
                    System.DateTime TableStart = System.DateTime.Now;
                    if (!T.TransferData(this._TransferDirectory))
                    {
                        OK = false;
                        this._Errors += System.DateTime.Now.Year.ToString() + "." + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() +
                            System.DateTime.Now.Hour.ToString() + ":" + System.DateTime.Now.Minute.ToString() + ":" + System.DateTime.Now.Second.ToString() +
                            "\r\n" + T.Errors();
                        this._TransferHistory.Add(T.TableName(), T.Errors());
                    }
                    else
                    {
                        if (T.DoTransferData)
                        {
                            string TableInfo = "";
                            int ii = 0;
                            if (!this._TransferHistory.ContainsKey(T.TableName()))
                                TableInfo = T.TotalCount.ToString();
                            else if (!int.TryParse(this._TransferHistory[T.TableName()].ToString(), out ii))
                                TableInfo = T.TotalCount.ToString();
                            else
                                TableInfo = (ii + T.TotalCount).ToString();
                            TableInfo += "   (" + TableStart.ToString("HH:mm:ss") + " - " + System.DateTime.Now.ToString("HH:mm:ss") + ")";

                            if (!this._TransferHistory.ContainsKey(T.TableName()))
                                this._TransferHistory.Add(T.TableName(), TableInfo);
                            else
                            {
                                this._TransferHistory[T.TableName()] = TableInfo;
                            }

                        }
                    }
                    if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)//this._AutoTransferDataAndClose)
                        this._Report += T.Report();
                    if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)//!this._AutoTransferDataAndClose)
                        Application.DoEvents();
                    if (!OK && DiversityCollection.CacheDatabase.CacheDBsettings.Default.StopOnError)
                        return OK;
                }
                System.DateTime End = System.DateTime.Now;
                string TimeString = "Start: " + Start.ToString("HH:mm:ss") + " End: " + End.ToString("HH:mm:ss");
                this._TransferHistory.Add("Execution time", TimeString);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        private bool PostgresTransferViaFile_InitExport(string TransferDirectory, ref string Error, ref string SQL)
        {
            string Message = "";
            // Init Export
            SQL = "DECLARE @RC int " +
                "DECLARE @TargetPath varchar(200) " +
                "DECLARE @Schema varchar(200) " +
                "DECLARE @ProtocolFileName varchar(200) " +
                "SET @TargetPath = '" + TransferDirectory + "' " +
                "SET @Schema = '" + this._TransferSteps[0].Schema + "' " +
                "SET @ProtocolFileName = 'Outfile.txt' " +
                "EXECUTE @RC = [dbo].[procBcpInitExport] " +
                "@TargetPath, @Schema, @ProtocolFileName";
            bool OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message);
            if (!OK)
            {
                Error += Message + ": " + SQL;
            }
            return OK;
        }


        private void buttonSelectNone_Click(object sender, EventArgs e)
        {
            foreach (DiversityCollection.CacheDatabase.TransferStep T in _TransferSteps)
            {
                T.DoTransferData = false;
            }
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DiversityCollection.CacheDatabase.TransferStep T in _TransferSteps)
            {
                T.DoTransferData = true;
            }
        }

        #endregion

        #region Form
        
        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        private void setFormTextsAndImages(System.Collections.Generic.List<DiversityCollection.CacheDatabase.TransferStep> TransferSteps)
        {
            this.setButtonTimeoutText();
            this.Text = "Transfer data into ";
            if (this._ForPackage)
            {
                string Package = TransferSteps[0].TransferProcedure.Substring(0, TransferSteps[0].Target.IndexOf("_")).ToUpper();
                this.Text += Package;
                this.labelTarget.Text = Package;
                this.pictureBoxSource.Visible = false;//.Image = this.imageList.Images[2];
                this.pictureBoxTarget.Image = this.imageList.Images[3];
                this.labelCountTarget.Text = "Package";
                this.labelCountSource.Visible = false;//.Text = "Postgres";
            }
            else if (this._ForPostgres)
            {
                this.Text += DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;
                this.pictureBoxSource.Image = this.imageList.Images[1];
                this.pictureBoxTarget.Image = this.imageList.Images[2];
                this.labelCountTarget.Text = "Postgres";
                this.labelCountSource.Text = "CacheDB";
            }
            else
            {
                this.pictureBoxSource.Visible = false;
                this.Text += DiversityCollection.CacheDatabase.CacheDB.DatabaseName;
                this.pictureBoxTarget.Image = this.imageList.Images[1];
                this.labelCountTarget.Text = "CacheDB";
                this.labelCountSource.Text = "";
            }
            this.labelHaeder.Text = "Transfer of the data into";
        }
        
        private void buttonTimeout_Click(object sender, EventArgs e)
        {
            int Timeout = 0;
            if (this._ForPostgres)
                Timeout = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutPostgres;
            else
                Timeout = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutCacheDB;
            if (Timeout > 0) 
                Timeout = Timeout / 60;
            DiversityWorkbench.Forms.FormGetInteger f = new DiversityWorkbench.Forms.FormGetInteger(Timeout, "Set timeout", "Set the timeout or database interactions in minutes (0 = inifinite)");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.Integer != null)
            {
                Timeout = (int)f.Integer;
                if (Timeout > 0)
                    Timeout = Timeout * 60;
                if (this._ForPostgres)
                    DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutPostgres = Timeout;
                else
                    DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutCacheDB = Timeout;
                this.setButtonTimeoutText();
            }
        }

        private void setButtonTimeoutText()
        {
            int Timeout = 0;
            if (this._ForPostgres)
                Timeout = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutPostgres;
            else
                Timeout = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutCacheDB;
            if (Timeout > 0)
            {
                Timeout = Timeout / 60;
                this.buttonTimeout.Text = "      " + Timeout.ToString() + " min.";
            }
            else
                this.buttonTimeout.Text = "      Infinite";
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            DiversityCollection.CacheDatabase.FormSettings f = new FormSettings();
            f.ShowDialog();
        }

        #endregion

        #region Logging and stopping

        private void buttonLogging_Click(object sender, EventArgs e)
        {
            DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents = !DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents;
            DiversityCollection.CacheDatabase.CacheDBsettings.Default.Save();
            this.initLogEventControls();
        }

        private void initLogEventControls()
        {
            if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents)
            {
                this.buttonLogging.Image = DiversityCollection.Resource.List;
                this.toolTip.SetToolTip(this.buttonLogging, "Logging is on");
            }
            else
            {
                this.buttonLogging.Image = DiversityCollection.Resource.ListNot;
                this.toolTip.SetToolTip(this.buttonLogging, "Logging is off");
            }

            if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.StopOnError)
            {
                this.buttonStopOnError.Image = DiversityCollection.Resource.ArrowStop;
                this.toolTip.SetToolTip(this.buttonStopOnError, "Stop on error");
            }
            else
            {
                this.buttonStopOnError.Image = DiversityCollection.Resource.ArrowNextNext;
                this.toolTip.SetToolTip(this.buttonStopOnError, "Do not stop on error, continue import");
            }

        }


        private void buttonStopOnError_Click(object sender, EventArgs e)
        {
            DiversityCollection.CacheDatabase.CacheDBsettings.Default.StopOnError = !DiversityCollection.CacheDatabase.CacheDBsettings.Default.StopOnError;
            DiversityCollection.CacheDatabase.CacheDBsettings.Default.Save();
            this.initLogEventControls();
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
