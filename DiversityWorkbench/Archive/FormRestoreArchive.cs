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
    public partial class FormRestoreArchive : Form
    {

        #region Parameter

        private System.Collections.Generic.Dictionary<string, System.IO.FileInfo> _FileList;
        private System.Collections.Generic.List<Archive.UserControlTable> _RestoreList;
        private System.IO.DirectoryInfo _ArchiveDirectory;
        //System.Collections.Generic.Dictionary<string, Archive.Table> _TableList;
        //DiversityWorkbench.Archive.DataArchive _DataArchive;

        #endregion

        #region Construction

        public FormRestoreArchive(string HeaderText, System.Collections.Generic.List<string> Tables)
        {
            InitializeComponent();
            this.labelHeader.Text = HeaderText;
            this._FileList = new Dictionary<string, System.IO.FileInfo>();
            this.checkBoxIncludeLog.Enabled = false;
            foreach (string T in Tables)
            {
                this._FileList.Add(T, null);
            }
        }

        #endregion

        #region Interface
        
        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private int? _ProjectID;
        public int? ProjectID()
        {
            return this._ProjectID;
        }
        
        #endregion

        #region Datahandling

        private void buttonLoadData_Click(object sender, EventArgs e)
        {
            this.panelTables.Controls.Clear();
            string DirectoryForArchive = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Archive);
            this.folderBrowserDialog.SelectedPath = DirectoryForArchive;
            this.folderBrowserDialog.ShowNewFolderButton = false;
            this.folderBrowserDialog.ShowDialog();
            if (this.folderBrowserDialog.SelectedPath.Length > 0)
            {
                this.labelState.Text = "get files";
                this.textBoxArchiveFolder.Text = this.folderBrowserDialog.SelectedPath;
                this._ArchiveDirectory = new System.IO.DirectoryInfo(this.folderBrowserDialog.SelectedPath);
                System.Collections.Generic.Dictionary<string, System.IO.FileInfo> LogFileList = new Dictionary<string, System.IO.FileInfo>();
                System.IO.FileInfo[] FF = this._ArchiveDirectory.GetFiles();
                foreach (System.IO.FileInfo F in FF)
                {
                    if (F.Extension.ToLower() == ".xml")
                    {
                        string File = F.Name.Substring(0, F.Name.IndexOf("."));
                        if (this._FileList.ContainsKey(File))
                        {
                            this._FileList[File] = F;
                        }
                        if (File.EndsWith("_log")) //!LogFileList.ContainsKey())
                        {
                            string ParentTable = File.Substring(0, File.Length - 4);
                            if (!LogFileList.ContainsKey(ParentTable))
                                LogFileList.Add(ParentTable, F);
                        }
                    }
                }
                this._RestoreList = new List<UserControlTable>();
                foreach (System.Collections.Generic.KeyValuePair<string, System.IO.FileInfo> KV in this._FileList)
                {
                    DiversityWorkbench.Archive.Table T = new Table(KV.Key, KV.Value);
                    T.IncludeLog = this.checkBoxIncludeLog.Checked;
                    DiversityWorkbench.Archive.UserControlTable U = new UserControlTable(T);
                    U.TableName = KV.Key;
                    if (KV.Value != null)
                    {
                        U.FileInfo = KV.Value;
                    }
                    if (LogFileList.ContainsKey(KV.Key) && LogFileList[KV.Key] != null)
                    {
                        T.setLogFile(LogFileList[KV.Key]);
                    }
                    this._RestoreList.Add(U);
                    this.panelTables.Controls.Add(U);
                    U.Dock = DockStyle.Top;
                    U.BringToFront();
                }
                this.labelState.Text = "";
            }
            this.buttonReadData.Enabled = true;
            if (this.checkBoxIncludeLog.Checked)
            {
                this.checkBoxIncludeLog.Checked = false;
                this.checkBoxIncludeLog_Click(null, null);
            }
            this.checkBoxIncludeLog.Enabled = true;
        }

        private void buttonRestoreArchive_Click(object sender, EventArgs e)
        {
            if (DataArchive.SqlConnection() == null)
            {
                return;
            }
            string Message = "";
            string MessageSummary = "";
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            this.ResetTables(true, true);

            this.progressBar.Maximum = this._RestoreList.Count;
            this.progressBar.Visible = true;
            this.progressBar.Value = 0;
            this.RestoreProtocolInit();
            foreach (Archive.UserControlTable U in this._RestoreList)// this.panelTables.Controls)
            {
                this.labelState.Text = "Restoring " + U.TableName;
                if (this.progressBar.Value < this.progressBar.Maximum)
                    this.progressBar.Value++;
                Application.DoEvents();
                try
                {
                    bool LogOnly = false;
                    if (!U.CreateTable())
                    { 
                        Message += "failed to create table " + U.TableName;
                        if (!IncludeLog)
                            continue;
                        else
                        {
                            LogOnly = true;
                        }
                    }
                    int Count = 0;
                    int Log = 0;
                    U.ReadData(ref Count, ref Log);

                    Message = U.RestoreArchive(this.checkBoxIncludeLog.Checked);
                    if (U.ProjectID() != null)
                        this._ProjectID = U.ProjectID();
                    if (Message.Trim().Length > 0)
                    MessageSummary += Message;
                    if (U.NumberOfFailedLines > 0 && this.checkBoxAskForStopOnError.Checked)
                    {
                        string ErrorMessage = "The restore of table " + U.Table() + " resulted in " + U.NumberOfFailedLines.ToString() + " failures.\r\nStop restore?";
                        if (System.Windows.Forms.MessageBox.Show(ErrorMessage, "Stop restore?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            Message += "\r\nArchive restore stopped on error";
                            MessageSummary += Message;
                            break;
                        }
                    }
                    if (U.NumberOfPresentLines > 0)
                    {
                        System.Windows.Forms.MessageBox.Show("The restore of table " + U.Table() + " found " + U.NumberOfPresentLines.ToString() + " allready present datasets.");
                        //string ErrorMessage = "The restore of table " + U.Table() + " found " + U.NumberOfPresentLines.ToString() + " allready present datasets.\r\nStop restore?";
                        //if (System.Windows.Forms.MessageBox.Show(ErrorMessage, "Stop restore?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        //    break;
                    }
                    string Protocol = U.RowCount().ToString() + " Rows of " + U.Table() + " restored.\r\n";
                    if (Message.Trim().Length > 0) Protocol += Message + "\r\n";
                    this.RestoreProtocolWrite(Protocol);
                }
                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            }
            DataArchive.SqlConnection(true);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.progressBar.Visible = false;
            if (Message.Trim().Length == 0)
            {
                Message = "Archive restored";
                System.Windows.Forms.MessageBox.Show(Message);
            }
            else
            {
                DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Archive restore errors", MessageSummary, true);
                f.ShowDialog();
            }
        }

        private void ResetTables(bool IncludeDispose = false, bool KeepNumbers = false)
        {
            try
            {
                this.labelState.Text = "Resetting Tables";
                this.progressBar.Maximum = this._RestoreList.Count;
                this.progressBar.Visible = true;
                this.progressBar.Value = 0;
                Application.DoEvents();
                foreach (Archive.UserControlTable U in this._RestoreList)// this.panelTables.Controls)
                {
                    try
                    {
                        U.ResetDatatable(IncludeDispose, KeepNumbers);
                    }
                    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                    if (this.progressBar.Value < this.progressBar.Maximum)
                        this.progressBar.Value++;
                }
                this.labelState.Text = "";
                Application.DoEvents();
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void RestoreProtocolWrite(string Text)
        {
            using (System.IO.StreamWriter sw = System.IO.File.AppendText(this._ArchiveDirectory + "\\RestoreProtocol.txt"))
            {
                sw.WriteLine(Text);
                sw.Close();
                sw.Dispose();
            }
        }

        private void RestoreProtocolInit()
        {
            using (System.IO.StreamWriter sw = System.IO.File.CreateText(this._ArchiveDirectory + "\\RestoreProtocol.txt"))
            {
                sw.WriteLine("Restore started at:" + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());
                sw.WriteLine("Restored by: " + System.Environment.UserName);
                sw.WriteLine("Restored archive: " + this._ArchiveDirectory.Name);
                sw.WriteLine("");
                sw.Close();
                sw.Dispose();
            }
        }

        private void buttonReadData_Click(object sender, EventArgs e)
        {
            string Message = "";
            int Count = 0;
            int CountLog = 0;
            bool AnyData = false;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.progressBar.Value = 0;
            this.progressBar.Maximum = this._RestoreList.Count;
            this.progressBar.Visible = true;
            foreach (Archive.UserControlTable U in this._RestoreList)// this.panelTables.Controls)
            {
                Message += U.ReadData(ref Count, ref CountLog);
                if (Count > 0)
                    AnyData = true;
                if (this.progressBar.Maximum > this.progressBar.Value)
                    this.progressBar.Value++;
            }
            if (Message.Length > 0)
                System.Windows.Forms.MessageBox.Show(Message);
            else if (AnyData)
                this.buttonRestoreArchive.Enabled = true;
            else
                System.Windows.Forms.MessageBox.Show("No data found");
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.progressBar.Value = 0;
        }

        #endregion

        #region Form events

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        private bool IncludeLog = false;
        private void checkBoxIncludeLog_Click(object sender, EventArgs e)
        {
            this.IncludeLog = this.checkBoxIncludeLog.Checked;
            if (this.IncludeLog)
            {
                this.checkBoxIncludeLog.BackColor = System.Drawing.SystemColors.ActiveCaption;
                this.checkBoxIncludeLog.ForeColor = System.Drawing.SystemColors.Window;
            }
            else
            {
                this.checkBoxIncludeLog.BackColor = System.Drawing.SystemColors.Control;
                this.checkBoxIncludeLog.ForeColor = System.Drawing.SystemColors.ControlText;
            }
            if (this._RestoreList != null)
            {
                foreach (Archive.UserControlTable U in this._RestoreList)// this.panelTables.Controls)
                {
                    U.IncludeLog(IncludeLog);
                }
            }
        }

        #endregion

    }
}
