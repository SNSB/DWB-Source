using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Archive
{
    public partial class UserControlTable : UserControl, iTableGUI
    {

        #region Parameter

        private enum ArchiveState { Find, Archive, Read, Restore, Clear };

        public enum ResetState { KeysRemove, DataRemoved };

        private Archive.Table _Table;

        private string _Message;

        private string _DataPresentError;

        private int _NumberOfLines;

        private int _NumberOfPresentLines = 0;
        public int NumberOfPresentLines
        {
            get { return _NumberOfPresentLines; }
            //set { _NumberOfPresentLines = value; }
        }

        private int _NumberOfFailedLines = 0;
        public int NumberOfFailedLines
        {
            get { return _NumberOfFailedLines; }
            //set { _NumberOfFailedLines = value; }
        }

        private int _LogFailedLines = 0;
        public int LogFailedLines { get { return _LogFailedLines; } }

        private int _LogPresentLines = 0;
        public int LogPresentLines { get { return _LogPresentLines; } }

        private string _TableName;
        public string TableName { set { _TableName = value; } get { return _TableName; } }
        private System.IO.FileInfo _fileInfo;
        public System.IO.FileInfo FileInfo { set { _fileInfo = value; } }


        #endregion

        #region Construction

        public UserControlTable(Archive.Table Table)
        {
            InitializeComponent();
            this._Table = Table;
            this.labelTableName.Text = Table.TableName();
            this.labelCountLog.Visible = this._Table.IncludeLog;
            this.labelCountLog.Enabled = this._Table.HasLog;
        }

        #endregion

        #region Interface

        #region iTableGUI

        public void setMaxRows(int Max)
        {
            this.progressBar.Maximum = Max;
        }

        public void setCurrentRow(int Row)
        {
            if (this.progressBar.Maximum > Row)
                this.progressBar.Value = Row;
            int Section = (int)(this._NumberOfLines / 20);
            if (Section > 0 && Row % Section == 0) Application.DoEvents();
        }

        public void setCurrentRow()
        {
            if (this.progressBar.Maximum > progressBar.Value)
                this.progressBar.Value++;
            int Section = (int)(this._NumberOfLines / 20);
            if (this.progressBar.Value > 0 && Section > 0 && this.progressBar.Value % Section == 0) Application.DoEvents();
        }

        public void setLogCount(int Found, int Inserted)
        {
            if (Found == Inserted) this.labelCountLog.Text = Found.ToString();
            else
            {
                this.labelCountLog.Text = Inserted.ToString();
                this.labelCountLog.ForeColor = System.Drawing.Color.Red;
                this.toolTip.SetToolTip(this.labelCountLog, "For " + _LogFailedLines.ToString() + " of " + _LogPresentLines + " the import in the log failed");
            }
        }


        #endregion

        #region Create archive

        public string Table() { if (this._Table != null) return this._Table.TableName(); else return ""; }
        public int RowCount() { if (this._Table != null) return this._Table.DataCount(); else return 0; }

        public void IncludeLog(bool Include)
        {
            this._Table.IncludeLog = Include;
            this.labelCountLog.Visible = this._Table.IncludeLog;
            if (!this._Table.HasLog)
            {
                this.labelCountLog.Text = "-";
                this.labelCountLog.BackColor = System.Drawing.SystemColors.ControlDark;
            }
        }

        public void FindData(System.Collections.Generic.List<Archive.Table> ParentTables)
        {
            string Message = this._Table.FindData(ParentTables);
            this.labelCount.Text = this._Table.DataCount().ToString();
            if (this._Table.IncludeLog && this._Table.HasLog)
                this.labelCountLog.Text = this._Table.LogCount().ToString();
            this.SetInfoColor(Message, ArchiveState.Find);
        }

        public string ArchiveData(string Folder)
        {
            string Message = "";
            try
            {
                Message = this._Table.ArchiveData(Folder);
                this.SetInfoColor(Message, ArchiveState.Archive);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return Message;
        }

        public void ResetDatatable(bool IncludeDispose = false, bool KeepNumber = false)
        {
            if (this._Table != null)
                this._Table.ResetDatatable(IncludeDispose);
            if (!KeepNumber)
            {
                this.labelCount.Text = "0";
                if (this.labelCountLog.Text != "-")
                    this.labelCountLog.Text = "0";
            }
            if (IncludeDispose)
            {
                this._Table = null;
            }
            this.SetInfoColor("", ArchiveState.Read);
        }

        #endregion

        #region Restore archive

        //public string ReadData()
        //{
        //    int Count = 0;
        //    string Message = this._Table.ReadData(ref Count);
        //    this.labelCount.Text = this._Table.DataCount().ToString();
        //    this.SetInfoColor(Message, ArchiveState.Read);
        //    return Message;
        //}

        public bool CreateTable()
        {
            bool OK = true;
            if (this._TableName != null && this._TableName.Length > 0 && this._fileInfo != null && this._fileInfo.Exists)
                this._Table = new Table(this._TableName, this._fileInfo);
            else
            {
                OK = false;
            }
            return OK;
        }

        public bool CreateLogTable()
        {
            bool OK = true;
            if (this._TableName != null && this._TableName.Length > 0 && this._fileInfo != null && this._fileInfo.Exists)
                this._Table = new Table(this._TableName, this._fileInfo);
            else
            {
                OK = false;
            }
            return OK;
        }


        public string ReadData(ref int Count, ref int CountLog)
        {
            string Message = "";
            try
            {
                if (this._Table != null)
                {
                    Message = this._Table.ReadData(ref Count, ref CountLog);
                    this.labelCount.Text = this._Table.DataCount().ToString();
                    if (this._Table.IncludeLog && this._Table.HasLog)
                        this.labelCountLog.Text = CountLog.ToString();
                }
                else
                {

                }
                this.SetInfoColor(Message, ArchiveState.Read);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return Message;
        }

        private int? _ProjectID = null;
        public int? ProjectID()
        {
            return this._ProjectID;
        }

        public string RestoreArchive(bool IncludeLog)
        {
            if (this._Table == null)
            {
                return "";
            }
            _NumberOfLines = this._Table.DataCount();
            this._Table.IncludeLog = IncludeLog;
            this._Table.SetGUI(this);
            this._NumberOfFailedLines = 0;
            this._NumberOfPresentLines = 0;
            this._LogFailedLines = 0;
            this._LogPresentLines = 0;
            string Message = this._Table.RestoreArchive(ref this._NumberOfFailedLines, ref this._NumberOfPresentLines, ref _LogFailedLines, ref _LogPresentLines);
            this._ProjectID = this._Table.ProjectID();
            this.SetInfoColor(Message.Trim(), ArchiveState.Restore);
            this.labelCount.ForeColor = System.Drawing.Color.Green;
            if (this._NumberOfFailedLines > 0 || this._NumberOfPresentLines > 0)
            {
                this.labelCount.Text = (_NumberOfLines - this._NumberOfFailedLines - this._NumberOfPresentLines).ToString();
                this.toolTip.SetToolTip(this.labelCount, (_NumberOfLines - this._NumberOfFailedLines - this._NumberOfPresentLines).ToString() + " of " + _NumberOfLines + " have been imported");
                if (this._NumberOfFailedLines > 0)
                {
                    this.labelCountFailed.Visible = true;
                    this.labelCountFailed.Text = this._NumberOfFailedLines.ToString();
                    this.toolTip.SetToolTip(this.labelCountFailed, "For " + this._NumberOfFailedLines.ToString() + " of " + _NumberOfLines + " the import failed");
                }
                if (this._NumberOfPresentLines > 0)
                {
                    this.labelCountPresent.Visible = true;
                    this.labelCountPresent.Text = this._NumberOfPresentLines.ToString();
                    this.toolTip.SetToolTip(this.labelCountPresent, "For " + this.labelCountPresent.ToString() + " of " + _NumberOfLines + " the data are allready present");
                }
            }
            else
            {
                this.labelCountFailed.Visible = false;
                this.toolTip.SetToolTip(this.labelCount, "All lines have been imported");
            }
            // Checking the log
            if (_LogFailedLines > 0)
            {
                this.labelCountLog.ForeColor = System.Drawing.Color.Red;
                this.toolTip.SetToolTip(this.labelCountLog, "For " + _LogFailedLines.ToString() + " of " + _LogPresentLines + " the import in the log failed");
            }
            else // if(_LogPresentLines.ToString() == this.labelCount.Text)
            {
                this.labelCountLog.ForeColor = System.Drawing.Color.Green;
            }
            //else if ("-" != this.labelCount.Text)
            //{
            //    this.labelCountLog.ForeColor = System.Drawing.Color.Blue;
            //    this.toolTip.SetToolTip(this.labelCountLog, "Found lines: " + this.labelCount.Text + ". Inserted lines: " + _LogPresentLines.ToString()); ;
            //}

            return Message;
        }

        #endregion

        #region Reset database

        public void CountData()
        {
            this.labelCount.Text = this._Table.Count();
        }

        public void setResetState(ResetState State)
        {
            switch (State)
            {
                case ResetState.KeysRemove:
                    if (this.labelCount.Text != "0")
                    {
                        this.BackColor = System.Drawing.Color.Yellow;
                    }
                    break;
                case ResetState.DataRemoved:
                    //if (this.labelCount.Text != "0")
                    {
                        this.BackColor = System.Drawing.Color.LightGreen;
                    }
                    break;
            }
        }

        public void setClearTableInfo(string Message)
        {
            //string Message = this._Table.ClearTable();
            this.SetInfoColor(Message, ArchiveState.Clear);
            //return Message;
        }

        #endregion

        #endregion

        #region Info
        private void buttonInfo_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormTableContent f = new Forms.FormTableContent(this._Table.TableName(), "Content of table " + this._Table.TableName(), this._Table.DataTable());
            f.ShowDialog();
        }

        private void SetInfoColor(string Message, ArchiveState State)
        {
            if (Message.Trim().Length > 0)
            {
                this._Message = Message;
                this.buttonMessage.Visible = true;
                if (this._NumberOfFailedLines > 0)
                    this.BackColor = System.Drawing.Color.Pink;
                else if (this._NumberOfPresentLines > 0)
                    this.BackColor = System.Drawing.Color.LightBlue;
            }
            else
            {
                this.buttonMessage.Visible = false;
                switch (State)
                {
                    case ArchiveState.Restore:
                    case ArchiveState.Archive:
                        if (this.labelCount.Text != "0")
                        {
                            this.BackColor = System.Drawing.Color.LightGreen;
                        }
                        else
                        {
                            this.BackColor = System.Drawing.SystemColors.Control;
                        }
                        break;
                    case ArchiveState.Read:
                    case ArchiveState.Find:
                        if (this.labelCount.Text != "0")
                        {
                            this.BackColor = System.Drawing.Color.Yellow;
                        }
                        else
                        {
                            this.BackColor = System.Drawing.SystemColors.Control;
                        }
                        break;
                    case ArchiveState.Clear:
                        if (this.labelCount.Text != "0")
                        {
                            if (this._Table.TableName() == "ProjectProxy" && this.labelCount.Text == "1")
                            {
                                this.buttonInfo.BackColor = System.Drawing.SystemColors.Control;
                                this.ForeColor = System.Drawing.Color.Black;
                            }
                            else
                            {
                                this.buttonInfo.BackColor = System.Drawing.Color.Pink;
                                this.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                        else
                        {
                            this.buttonInfo.BackColor = System.Drawing.SystemColors.Control;
                            this.ForeColor = System.Drawing.Color.Black;
                        }
                        break;
                }
            }
        }

        private void buttonMessage_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText(this._Table.TableName(), this._Message, true);
            f.ShowDialog();
            //System.Windows.Forms.MessageBox.Show(this._Message);
        }

        #endregion

    }
}
