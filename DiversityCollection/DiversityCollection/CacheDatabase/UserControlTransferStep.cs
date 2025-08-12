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

    public partial class UserControlTransferStep : UserControl, InterfaceTransferStep
    {

        #region Properties and parameter

        private string _SqlProcedure;
        private string _SqlViewResult;
        private string _Message;
        private string _Title;
        private bool _ReadyForAction;

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

        public UserControlTransferStep(string Title, string SqlTransferProcedure, string SqlViewResult, System.Drawing.Image Image)
        {
            InitializeComponent();
            this._Title = Title;
            this._SqlProcedure = SqlTransferProcedure;
            string ProcedureName = this._SqlProcedure.Substring(this._SqlProcedure.IndexOf(" ")).Trim();
            if (ProcedureName.Trim().StartsWith("[dbo]."))
                ProcedureName = ProcedureName.Substring(ProcedureName.IndexOf(".")+1).Trim();
            if (ProcedureName.Trim().IndexOf(" ") > -1)
                ProcedureName = ProcedureName.Substring(0, ProcedureName.IndexOf(" ")).Trim();
            ProcedureName = ProcedureName.Replace("[", "").Replace("]", "").Trim();
            string SQL = "select count(*) from INFORMATION_SCHEMA.ROUTINES R where R.ROUTINE_TYPE = 'PROCEDURE' and R.ROUTINE_NAME = '" + ProcedureName.Trim() + "'";
            string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Result == "0")
            {
                this.buttonInfo.Image = this.imageListInfo.Images[2];
                this.toolTip.SetToolTip(this.buttonInfo, "Procedure " + this._SqlProcedure.Substring(this._SqlProcedure.IndexOf(" ")).Trim() + " not available.");
                this._ReadyForAction = false;
                this.buttonInfo.Visible = true;
            }
            else this._ReadyForAction = true;
            this._SqlViewResult = SqlViewResult;
            this.pictureBoxStep.Image = Image;
            this.initControl();
        }

        public UserControlTransferStep(string Title, DiversityCollection.CacheDatabase.InterfaceCacheDB InterfaceCacheDB, string SqlViewResult, System.Drawing.Image Image)
        {
            InitializeComponent();
            this._Title = Title;
            if (DiversityCollection.CacheDatabase.CacheDB.ConnectionGazetteer() != null)
                this._ReadyForAction = true;
            else
            {
                this.buttonInfo.Image = this.imageListInfo.Images[2];
                this.toolTip.SetToolTip(this.buttonInfo, "No connection to DiversityGazetteer available.");
                this._ReadyForAction = false;
                this.buttonInfo.Visible = true;
            }
            this._InterfaceCacheDB = InterfaceCacheDB;
            this._SqlViewResult = SqlViewResult;
            this.pictureBoxStep.Image = Image;
            this.initControl();
        }
        
        #endregion

        #region Control and functions
        
        private void initControl()
        {
            this.labelStep.Text = this._Title;
            string SQL = this._SqlViewResult.Replace("SELECT * ", "SELECT COUNT(*) ");
            string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            this.labelCurrentCount.Text = "Current count: " + Result;
        }

        public bool StartTransfer()
        {
            if (!this._ReadyForAction)
                return false;

            this.buttonInfo.Visible = true;
            System.Windows.Forms.Application.DoEvents();
            if (this._InterfaceCacheDB == null)
            {
                string Message = "";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(this._SqlProcedure, ref Message))
                    this.buttonInfo.Image = this.imageListInfo.Images[0];
                else
                {
                    this.buttonInfo.Image = this.imageListInfo.Images[2];
                    this.toolTip.SetToolTip(this.buttonInfo, Message);
                }
            }
            else
            {
                this._Message = this._InterfaceCacheDB.TransferCountryIsoCode();
                if (this._Message.Length > 0)
                {
                    this.buttonInfo.Image = this.imageListInfo.Images[1];
                    this.toolTip.SetToolTip(this.buttonInfo, this._Message);
                }
                else this.buttonInfo.Image = this.imageListInfo.Images[0];
            }
            this.initControl();
            System.Windows.Forms.Application.DoEvents();
            return true;
        }

        private void buttonViewResult_Click(object sender, EventArgs e)
        {
            System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(this._SqlViewResult, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            DiversityWorkbench.FormEditTable f = new DiversityWorkbench.FormEditTable(ad, this._Title, "Data for " + this._Title, false);
            f.ShowDialog();
        }

        private void buttonInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._Message != null && this._Message.Length > 0)
                {
                    DiversityWorkbench.FormEditText f = new DiversityWorkbench.FormEditText(this._Title, this._Message, true);
                    f.ShowDialog();
                }
            }
            catch (System.Exception ex)
            {
            }
        }
        
        #endregion
    }
}
