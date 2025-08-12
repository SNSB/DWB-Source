using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.CacheDatabase
{
    public partial class UserControlTransfer : UserControl, InterfaceTransfer
    {

        #region Properties and parameter

        private string _SqlProcedure;
        private string _SqlViewResult;
        private string _Message;
        private string _Title;
        private bool _ReadyForAction;
        private DiversityCollection.CacheDatabase.TransferStep _TransferStep;

        public bool ReadyForAction()
        {
            return _ReadyForAction; 
        }

        public string Title()
        {
            return _Title;
        }

        public DiversityCollection.CacheDatabase.InterfaceCacheDB _InterfaceCacheDB;
        
        #endregion

        #region Construction

        public UserControlTransfer(DiversityCollection.CacheDatabase.TransferStep T)
        {
            try
            {
                InitializeComponent();
                this._Title = T.Title;
                this._TransferStep = T;
                this.pictureBoxStep.Image = T.Image;
                if (T.ForPackage)
                {
                    this._SqlViewResult = "\"" + T.SchemaPostgres + "\".\"" + T.Target + "\"";
                    this._SqlProcedure = T.TransferProcedure;
                    if (this.TransferProcedureDoesExist())
                    {
                        this._ReadyForAction = true;
                        this.buttonShowProcedure.Visible = true;
                    }
                    else
                        this.setProcedureMissing();
                }
                else if (T.ForPostgres)
                {
                    this._SqlViewResult = "\"" + T.Schema + "\".\"" + T.Target + "\"";
                    this._ReadyForAction = true;
                }
                else
                {
                    this._SqlProcedure = T.TransferProcedure;
                    if (this.TransferProcedureDoesExist())
                        this._ReadyForAction = true;
                    else
                        this.setProcedureMissing();
                    this._SqlViewResult = "[" + T.Schema + "]." + T.Target;
                }
                this.initControl();
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        private bool TransferProcedureDoesExist()
        {
            bool Exists = true;
            try
            {
                string ProcedureName = this._SqlProcedure;
                if (ProcedureName.Trim().StartsWith("[dbo]."))
                    ProcedureName = ProcedureName.Substring(ProcedureName.IndexOf(".") + 1).Trim();
                if (ProcedureName.Trim().IndexOf(" ") > -1)
                    ProcedureName = ProcedureName.Substring(0, ProcedureName.IndexOf(" ")).Trim();
                ProcedureName = ProcedureName.Replace("[", "").Replace("]", "").Trim();
                string SQL = "select count(*) from INFORMATION_SCHEMA.ROUTINES R where R.ROUTINE_TYPE = '";
                if (this._TransferStep.ForPackage)
                {
                    SQL += "FUNCTION";
                }
                else
                {
                    SQL += "PROCEDURE";
                }
                SQL += "' and R.ROUTINE_NAME = '" + ProcedureName.Trim().ToLower() + "'  and R.ROUTINE_SCHEMA = '" + this._TransferStep.Schema + "'";
                string Result = "";
                if (this._TransferStep.ForPackage)
                {
                    Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                }
                else
                {
                    Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                }
                if (Result == "0")
                    Exists = false;

            }
            catch (Exception ex)
            {
                Exists = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Exists;
        }

        private void setProcedureMissing()
        {
            this.buttonInfo.Image = this.imageListInfo.Images[2];
            string Message = "Procedure " + this._SqlProcedure + " not available.";
            if (this._SqlProcedure.IndexOf(" ") > -1)
                Message = "Procedure " + this._SqlProcedure.Substring(this._SqlProcedure.IndexOf(" ")).Trim() + " not available.";
            this.toolTip.SetToolTip(this.buttonInfo, Message);
            this._ReadyForAction = false;
            this.buttonInfo.Visible = true;
        }

        public void SetMessage(string Message)
        {
            this._Message = Message;
        }

        public void SetInfo(string Info)
        {
            this.labelInfo.Text = Info;
            System.Windows.Forms.Application.DoEvents();
        }

        public void SetTransferState(DiversityCollection.CacheDatabase.TransferStep.TransferState State)
        {
            this.buttonInfo.Visible = true;
            switch (State)
            {
                case TransferStep.TransferState.Successfull:
                    this.buttonInfo.Image = this.imageListInfo.Images[0];
                    break;
                case TransferStep.TransferState.Error:
                case TransferStep.TransferState.Failed:
                    this.buttonInfo.Image = this.imageListInfo.Images[2];
                    break;
                case TransferStep.TransferState.Transfer:
                    break;
                case TransferStep.TransferState.NotStarted:
                    this.buttonInfo.Visible = false;
                    break;
            }
            if (State == TransferStep.TransferState.Successfull)
            {
                this.labelEndTime.Text = System.DateTime.Now.ToString("HH:mm:ss");// +"," + System.DateTime.Now.Millisecond.ToString();
                if (this.labelEndTime.Text.Length > 0)
                    this.labelEnd.Visible = true;
            }
            
            this.initControl();
            Application.DoEvents();
        }


        public void SetDoTransfer(bool DoTransfer)
        {
            this.checkBoxTransfer.Checked = DoTransfer;
        }

        public void SetTransferStart()
        {
            this.labelStartTime.Text = System.DateTime.Now.Hour.ToString() + ":"
                            + System.DateTime.Now.Minute.ToString() + ":"
                            + System.DateTime.Now.Second.ToString();// +"," + System.DateTime.Now.Millisecond.ToString();
            if (labelStartTime.Text.Length > 0)
                this.labelStart.Visible = true;
            Application.DoEvents();
        }

        public void SetTransferProgress(int PercentReached)
        {
            if (!this.progressBar.Visible) this.progressBar.Visible = true;
            if (this.progressBar.Value != PercentReached)
            {
                if (PercentReached < 100) this.progressBar.Value = PercentReached;
                else this.progressBar.Value = this.progressBar.Maximum;
                Application.DoEvents();
            }
            else
            {
            }
        }

        #region Control and functions
        
        private void initControl()
        {
            try
            {
                this.progressBar.Visible = false;
                this.labelCountSource.Text = "";
                this.labelCount.Text = "";
                this.checkBoxTransfer.Checked = this._TransferStep.DoTransferData;
                this.labelStep.Text = this._Title;
                string Message = "";
                if (this._TransferStep.ForPostgres)
                {
                    string SQL = "SELECT COUNT(*) FROM \"" + this._TransferStep.Schema + "\".\"" + this._TransferStep.Target + "\"";// this._SqlViewResult.Replace("SELECT * ", "SELECT COUNT(*) ");
                    string Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                    this.labelCount.Text = Result;
                    if (!this._TransferStep.ForPackage)
                    {
                        SQL = "SELECT COUNT(*) FROM [" + this._TransferStep.Schema + "].[" + this._TransferStep.Target + "]";
                        Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message);
                        if (Result != this.labelCount.Text && Result.Length > 0)
                        {
                            this.labelCountSource.ForeColor = System.Drawing.Color.Red;
                            this.labelCountSource.Text = Result + " <>";
                        }
                        else
                        {
                            this.labelCountSource.ForeColor = System.Drawing.Color.Gray;
                            if (Result.Length > 0)
                                this.labelCountSource.Text = Result + " =";
                            else this.labelCountSource.Text = Result;
                        }
                    }
                }
                else
                {
                    string SQL = "SELECT COUNT(*) FROM " + this._SqlViewResult;// this._SqlViewResult.Replace("SELECT * ", "SELECT COUNT(*) ");
                    string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message);
                    if (Message.Length == 0)
                    {
                        this.labelCount.Text = Result;
                    }
                    else
                    {
                        //this.labelCurrentCount.Text = "";
                        this.buttonInfo.Image = this.imageListInfo.Images[2];
                        this.buttonInfo.Visible = true;
                        this.toolTip.SetToolTip(this.buttonInfo, Message);
                        this._ReadyForAction = false;
                        this._Message = Message;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        //private void initCount()
        //{
        //    try
        //    {
        //        string Message = "";
        //        if (this._TransferStep.ForPostgres)
        //        {
        //            string SQL = "SELECT COUNT(*) FROM \"" + this._TransferStep.Schema + "\".\"" + this._TransferStep.Target + "\"";// this._SqlViewResult.Replace("SELECT * ", "SELECT COUNT(*) ");
        //            string Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
        //            this.labelCount.Text = Result;
        //            SQL = "SELECT COUNT(*) FROM " + this._TransferStep.Schema + "." + this._TransferStep.Target;
        //            Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message);
        //            if (Result != this.labelCount.Text)
        //            {
        //                this.labelCountSource.ForeColor = System.Drawing.Color.Red;
        //                this.labelCountSource.Text = Result + " <>";
        //            }
        //            else
        //            {
        //                this.labelCountSource.ForeColor = System.Drawing.Color.Gray;
        //                this.labelCountSource.Text = Result + " =";
        //            }
        //        }
        //        else
        //        {
        //            string SQL = "SELECT COUNT(*) FROM " + this._SqlViewResult;// this._SqlViewResult.Replace("SELECT * ", "SELECT COUNT(*) ");
        //            string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message);
        //            if (Message.Length == 0)
        //            {
        //                this.labelCount.Text = Result;
        //            }
        //            else
        //            {
        //                //this.labelCurrentCount.Text = "";
        //                this.buttonInfo.Image = this.imageListInfo.Images[2];
        //                this.buttonInfo.Visible = true;
        //                this.toolTip.SetToolTip(this.buttonInfo, Message);
        //                this._ReadyForAction = false;
        //                this._Message = Message;
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //    }

        //}

        //public bool StartTransfer()
        //{
        //    if (!this._ReadyForAction)
        //        return false;
        //    this.labelStartTime.Text = System.DateTime.Now.Hour.ToString() + ":"
        //        + System.DateTime.Now.Minute.ToString() + ":"
        //        + System.DateTime.Now.Second.ToString() + ","
        //        + System.DateTime.Now.Millisecond.ToString();
        //    if(labelStartTime.Text.Length > 0)
        //        this.labelStart.Visible = true;
        //    this.buttonInfo.Visible = true;
        //    System.Windows.Forms.Application.DoEvents();
        //    if (this._InterfaceCacheDB == null)
        //    {
        //        string Message = "";
        //        if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(this._SqlProcedure, ref Message))
        //            this.buttonInfo.Image = this.imageListInfo.Images[0];
        //        else
        //        {
        //            this.buttonInfo.Image = this.imageListInfo.Images[2];
        //            this.toolTip.SetToolTip(this.buttonInfo, Message);
        //        }
        //    }
        //    else
        //    {
        //        this._Message = this._InterfaceCacheDB.TransferCountryIsoCode();
        //        if (this._Message.Length > 0)
        //        {
        //            this.buttonInfo.Image = this.imageListInfo.Images[1];
        //            this.toolTip.SetToolTip(this.buttonInfo, this._Message);
        //        }
        //        else this.buttonInfo.Image = this.imageListInfo.Images[0];
        //    }
        //    this.labelEndTime.Text = System.DateTime.Now.Hour.ToString() + ":"
        //        + System.DateTime.Now.Minute.ToString() + ":"
        //        + System.DateTime.Now.Second.ToString() + ","
        //        + System.DateTime.Now.Millisecond.ToString();
        //    if (this.labelEndTime.Text.Length > 0)
        //        this.labelEnd.Visible = true;
        //    this.initControl();
        //    System.Windows.Forms.Application.DoEvents();
        //    return true;
        //}

        private void buttonViewResult_Click(object sender, EventArgs e)
        {
            try
            {
                string Title = "First 100 lines of " + this._TransferStep.Target + " in " + this._TransferStep.Schema;
                if (this._TransferStep.ForPostgres)
                    Title += " in " + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;
                else Title += " in " + DiversityCollection.CacheDatabase.CacheDB.DatabaseName;
                System.Data.DataTable dt = new DataTable();
                if (this._TransferStep.ForPackage || this._TransferStep.ForPostgres)
                {
                    string Message = "";
                    string SQL = "SELECT * FROM \"" + this._TransferStep.Schema + "\".\"" + this._TransferStep.Target + "\" LIMIT 100";
                    DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dt, ref Message);
                }
                else
                {
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter("SELECT TOP 100 * FROM [" + this._TransferStep.Schema + "].[" + this._TransferStep.Target + "]", DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    ad.Fill(dt);
                }
                DiversityWorkbench.Forms.FormTableContent f = new DiversityWorkbench.Forms.FormTableContent(this._Title, Title, dt);
                f.ShowDialog();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void buttonInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._Message != null && this._Message.Length > 0)
                {
                    DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText(this._Title, this._Message, true);
                    f.ShowDialog();
                }
            }
            catch (System.Exception ex)
            {
            }
        }
        
        private void checkBoxTransfer_Click(object sender, EventArgs e)
        {
            this._TransferStep.DoTransferData = this.checkBoxTransfer.Checked;
        }

        #endregion

        private void buttonShowProcedure_Click(object sender, EventArgs e)
        {
            // hier kommt teils kein Ergebnis - von offiziellem Beispiel kopiert
            string SQL = "select prosrc from pg_proc where proname like '" + this._TransferStep.TransferProcedure + "';";
            string Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
            SQL = "SELECT d.description FROM pg_proc p " +
                "INNER JOIN pg_namespace n ON n.oid = p.pronamespace " +
                "INNER JOIN pg_description As d ON(d.objoid = p.oid) " +
                "WHERE P.proname = '" + this._TransferStep.TransferProcedure +"' and n.nspname = '" + this._TransferStep.Schema + "'";
            string Message = "";
            string Description = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, ref Message);
            if (Description.Length > 0)
                Result = Description + "\r\n\r\n" + Result;
            DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Definition of " + this._TransferStep.TransferProcedure, Result, true);
            f.ShowDialog();
            //System.Windows.Forms.MessageBox.Show(Result, this._TransferStep.TransferProcedure, MessageBoxButtons.OK);
        }

    }
}
