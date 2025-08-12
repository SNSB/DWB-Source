using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Archive
{
    public partial class FormCreateArchive : Form
    {
        #region Parameter

        private string _DirectoryForArchive = "";
        private DiversityWorkbench.Archive.DataArchive _DataArchive;
        private bool _RunAsBackground = false;
      
        #endregion

        #region Construction

        public FormCreateArchive(string Title, 
            string RootTable, 
            string DisplayColumn, 
            string ValueColumn, 
            string Restriction, 
            string TempIDTable, 
            string TempIDColumn, 
            string ProtocolColumn, 
            System.Collections.Generic.Dictionary<string, string> DataTables, 
            string LastChangesRetrievalFunction)
        {
            InitializeComponent();
            try
            {
                this.labelHeader.Text = Title;
                this._DataArchive = new DataArchive(RootTable, DisplayColumn, ValueColumn, Restriction, TempIDTable, TempIDColumn, ProtocolColumn, DataTables);
                this._DataArchive.setLastChangesInArchiveDataRetrievalFunction(LastChangesRetrievalFunction);
                foreach (System.Collections.Generic.KeyValuePair<string, Archive.Table> KV in this._DataArchive.getTables())
                {
                    DiversityWorkbench.Archive.UserControlTable U = new UserControlTable(KV.Value);
                    this.panelTables.Controls.Add(U);
                    U.Dock = DockStyle.Top;
                    U.BringToFront();
                }
                this._DirectoryForArchive = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Archive);
                this.textBoxArchiveFolder.Text = this._DirectoryForArchive;
                this.initForm();
            }
            catch (System.Exception ex)
            {
            }
        }

        public FormCreateArchive(System.Collections.Generic.Dictionary<string, string> DataTables, 
            string DirectoryForArchive, 
            string RootTable, 
            string DisplayColumn, 
            string ValueColumn, 
            string RestrictionValue, 
            string TempIDTable, 
            string TempIDColumn, 
            string DecisionColumn, 
            string ProtocolColumn, 
            string LastChangesRetrievalFunction)
        {
            InitializeComponent();
            this.Hide();
            this._RunAsBackground = true;
            try
            {
                this._DataArchive = new DataArchive(RootTable, DisplayColumn, ValueColumn, "", TempIDTable, TempIDColumn, ProtocolColumn, DataTables);
                this._DataArchive.setLastChangesInArchiveDataRetrievalFunction(LastChangesRetrievalFunction);
                this.setRestriction(RestrictionValue);
                foreach (System.Collections.Generic.KeyValuePair<string, Archive.Table> KV in this._DataArchive.getTables())
                {
                    DiversityWorkbench.Archive.UserControlTable U = new UserControlTable(KV.Value);
                    this.panelTables.Controls.Add(U);
                }
                this._DirectoryForArchive = DirectoryForArchive;
                DiversityWorkbench.ExceptionHandling.WriteToLog("FormCreateArchive", "FormCreateArchive(...)", DisplayColumn + " " + RestrictionValue.ToString() + " archived", DiversityWorkbench.Settings.ModuleName);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToLog("FormCreateArchive", "FormCreateArchive(...)", ex.Message, DiversityWorkbench.Settings.ModuleName);
            }
        }

        #endregion

        #region Form and events

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            // #35
            //KeyWord = "archive" + DiversityWorkbench.DwbManual.Hugo.KeywordPostfix.ToLower();

            //DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
            ////DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this.tableLayoutPanelMain, this.helpProvider.HelpNamespace, KeyWord);
            //this.helpProvider.SetHelpKeyword(this.tableLayoutPanelMain, KeyWord);
            this.helpProvider.SetHelpKeyword(this, KeyWord);
        }

        public void SetQueryStringTempIDs(string QueryStringTempIDs) { this._DataArchive.SetQueryStringTempIDs(QueryStringTempIDs); }

        private void initForm()
        {
            this.comboBoxRootSelection.DataSource = this._DataArchive.DtRootTable();
            this.comboBoxRootSelection.DisplayMember = "Display";
            this.comboBoxRootSelection.ValueMember = "Value";

            // #35
            //this.setHelp("");
        }

        private void buttonQueryData_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            //this.comboBoxRootSelection.Enabled = false;
            this.FindData();
            this.buttonCreateArchive.Enabled = true;
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void FindData()
        {
            try
            {
                this._DataArchive.InitTempIDs(this._ResetTempIDs);
                this._ResetTempIDs = false;
                this.progressBar.Visible = true;
                this.progressBar.Value = 0;
                this.progressBar.Maximum = this.panelTables.Controls.Count;
                foreach (DiversityWorkbench.Archive.UserControlTable U in this.panelTables.Controls)
                {
                    string Table = U.Table();
                    System.Collections.Generic.List<Archive.Table> ParentTables = this.ParentTables(Table);
                    U.FindData(ParentTables);
                    Application.DoEvents();
                    if (progressBar.Value < progressBar.Maximum)
                    this.progressBar.Value++;
                }
                progressBar.Visible = false;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToLog("FormCreateArchive", "FindData()", ex.Message, DiversityWorkbench.Settings.ModuleName);
            }
        }

        private bool FindAndArchiveData()
        {
            try
            {
                string Message = "";
                this._DataArchive.ArchiveProtocolAdd("Archive directory: " + this._DirectoryForArchive);
                this._DataArchive.ArchiveProtocolAdd("Creation date: " + System.DateTime.Now.ToLongDateString() + " " + System.DateTime.Now.ToLocalTime());
                this._DataArchive.ArchiveProtocolAdd("Created by: " + System.Environment.UserName);
                this._DataArchive.ArchiveProtocolAdd("LastChanges: " + DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("select dbo." + this._DataArchive.LastChangesInArchiveDataRetrievalFunction() + "(" + this._DataArchive.RestrictionValue + ")"));

                this._DataArchive.InitTempIDs();
                foreach (DiversityWorkbench.Archive.UserControlTable U in this.panelTables.Controls)
                {
                    string Table = U.Table();
                    System.Collections.Generic.List<Archive.Table> ParentTables = this.ParentTables(Table);
                    U.FindData(ParentTables);
                    Message += U.ArchiveData(this.ArchiveFolder());
                    U.ResetDatatable();
                }
                this._DataArchive.TextProtocolWrite(this.ArchiveFolder());
                if (Message.Length > 0)
                    this._DataArchive.DatabaseProtocolWrite(true);
                else this._DataArchive.DatabaseProtocolWrite(false);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToLog("FormCreateArchive", "FindAndArchiveData()", ex.Message, DiversityWorkbench.Settings.ModuleName);
                this._DataArchive.ErrorMessageAdd(ex.Message);
                return false;
            }
            return true;
        }

        private void buttonCreateArchive_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            string Message = this.ArchiveData();// "";
            System.Windows.Forms.MessageBox.Show(Message);
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private string ArchiveData()
        {
            string Message = "";
            this._DataArchive.ArchiveProtocolAdd("");
            this._DataArchive.ArchiveProtocolAdd("Archive directory: " + this._DirectoryForArchive);
            this._DataArchive.ArchiveProtocolAdd("Creation date: " + System.DateTime.Now.ToLongDateString() + " " + System.DateTime.Now.ToLocalTime());
            this._DataArchive.ArchiveProtocolAdd("Created by: " + System.Environment.UserName);
            this._DataArchive.ArchiveProtocolAdd("LastChanges: " + DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("select dbo." + this._DataArchive.LastChangesInArchiveDataRetrievalFunction() + "(" + this._DataArchive.RestrictionValue + ")"));
            foreach (DiversityWorkbench.Archive.UserControlTable U in this.panelTables.Controls)
            {
                try
                {
                    //string Table = U.Table();
                    string Error = U.ArchiveData(this.ArchiveFolder());
                    if (Error.Length > 0)
                        Message += Error;
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToLog("FormCreateArchive", "ArchiveData()", ex.Message, DiversityWorkbench.Settings.ModuleName);
                }
            }

            this._DataArchive.TextProtocolWrite(this.ArchiveFolder());
            if (Message.Length > 0)
                this._DataArchive.DatabaseProtocolWrite(true);
            else this._DataArchive.DatabaseProtocolWrite(false);

            if (Message.Length == 0)
            {
                Message = "Archive created";
                if (this._RunAsBackground)
                    DiversityWorkbench.ExceptionHandling.WriteToLog("FormCreateArchive", "ArchiveData()", "Archive created", DiversityWorkbench.Settings.ModuleName);
            }
            return Message;
        }

        private void WriteDatabaseProtocol()
        {
        }

        //private void WriteTextProtocol()
        //{
        //    try
        //    {
        //        using (System.IO.StreamWriter sw = System.IO.File.CreateText(this.ArchiveFolder() + "\\Protocol.txt"))
        //        {
        //            sw.WriteLine("Archive for: " + DiversityWorkbench.Settings.ModuleName);
        //            sw.WriteLine("Created at: " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToLocalTime());
        //            sw.WriteLine("Created by: " + System.Environment.UserName);
        //            sw.WriteLine("Server: " + DiversityWorkbench.Settings.DatabaseServer);
        //            sw.WriteLine("Database: " + DiversityWorkbench.Settings.DatabaseName);
        //            string SQL = "SELECT dbo.Version()";
        //            string Version = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
        //            if (Version.Length > 0)
        //                sw.WriteLine("Database version: " + Version);
        //            sw.WriteLine("SERVERNAME: " + DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("select @@SERVERNAME"));
        //            sw.WriteLine("BaseURL: " + DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("select dbo.BaseURL()"));
        //            sw.WriteLine("LastChanges: " + DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("select dbo." + this._DataArchive.LastChangesInArchiveDataRetrievalFunction() + "(" + this._DataArchive.RestrictionValue + ")"));
        //            sw.Close();
        //            sw.Dispose();
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToLog("FormCreateArchive", "ArchiveData()", ex.Message, DiversityWorkbench.Settings.ModuleName);
        //    }
        //}

        private string ArchiveFolder()
        {
            string Folder = "";
            if (_RunAsBackground)
            {
                Folder = this._DirectoryForArchive;
                string SQL = "SELECT " + this._DataArchive.DisplayColumn + " FROM " + this._DataArchive.RootTable + " WHERE " + this._DataArchive.ValueColumn + " = " + this._DataArchive.RestrictionValue;
                string Archive = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Archive.Trim().Length > 0)
                {
                    Archive = Archive.Trim().Replace(" ", "_").Replace(".", "_");
                    Folder += "\\" + Archive + "_";
                }
            }
            else
            {
                Folder = this.textBoxArchiveFolder.Text;
                System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxRootSelection.SelectedItem;
                if (!Folder.EndsWith("\\"))
                    Folder += "\\";
                Folder += R["Display"].ToString() + "_";
            }
            Folder += System.DateTime.Now.Year.ToString();
            if (System.DateTime.Now.Month < 10)
                Folder += "0";
            Folder += System.DateTime.Now.Month.ToString();
            if (System.DateTime.Now.Day < 10)
                Folder += "0";
            Folder += System.DateTime.Now.Day.ToString();
            System.IO.DirectoryInfo D = new System.IO.DirectoryInfo(Folder);
            if (!D.Exists)
                D.Create();
            return Folder;
        }

        private System.Collections.Generic.List<Archive.Table> ParentTables(string TableName)
        {
            System.Collections.Generic.List<Archive.Table> List = this._DataArchive.ParentTables(TableName);
            return List;
        }

        private bool _ResetTempIDs = false;

        private void comboBoxRootSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ResetTempIDs = true;
            // Reset controls
            foreach (DiversityWorkbench.Archive.UserControlTable U in this.panelTables.Controls)
            {
                U.ResetDatatable();
            }

            System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxRootSelection.SelectedItem;
            //this._DataArchive.RestrictionValue = R["Value"].ToString();
            this.setRestriction(R["Value"].ToString());
        }
        
        private void setRestriction(string Value)
        {
            this._DataArchive.RestrictionValue = Value;
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        private void buttonDirectory_Click(object sender, EventArgs e)
        {
            if (System.IO.Directory.Exists(this.ArchiveFolder()))
                System.Diagnostics.Process.Start("explorer.exe", this.ArchiveFolder());
        }

        private void checkBoxIncludeLog_Click(object sender, EventArgs e)
        {
            this._DataArchive.IncludeLog = this.checkBoxIncludeLog.Checked;
            if (this._DataArchive.IncludeLog)
            {
                this.checkBoxIncludeLog.BackColor = System.Drawing.SystemColors.ActiveCaption;
                this.checkBoxIncludeLog.ForeColor = System.Drawing.SystemColors.Window;
            }
            else
            {
                this.checkBoxIncludeLog.BackColor = System.Drawing.SystemColors.Control;
                this.checkBoxIncludeLog.ForeColor = System.Drawing.SystemColors.ControlText;
            }
            foreach (DiversityWorkbench.Archive.UserControlTable U in this.panelTables.Controls)
            {
                U.IncludeLog(this._DataArchive.IncludeLog);
            }

        }

        #endregion

        #region Interface

        public bool CreateArchive()
        {
            return this.FindAndArchiveData();
            //this.FindAndArchiveData();
            //this.FindData();
            //this.ArchiveData();
            //return OK;
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
