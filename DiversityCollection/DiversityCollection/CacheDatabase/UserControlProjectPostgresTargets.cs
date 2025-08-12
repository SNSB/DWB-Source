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
    public partial class UserControlProjectPostgresTargets : UserControl
    {

        #region Parameter

        private int _ProjectID;
        private int _TargetID;
        private string _PostgresDatabase;
        private string _Schema;
        private System.Data.DataTable _dtTarget;
        private InterfaceProject _iProject;
        private readonly int _ControlHeight = 17;
        
        #endregion

        #region Construction

        //public UserControlProjectPostgresTargets(int ProjectID, string PostgresDatabase, string Schema)
        //{
        //    InitializeComponent();
        //    this._PostgresDatabase = PostgresDatabase;
        //    this._ProjectID = ProjectID;
        //    this._Schema = Schema;
        //    this.initControl();
        //}

        public UserControlProjectPostgresTargets(int ProjectID, int TargetID, string Schema, InterfaceProject iProject)
        {
            InitializeComponent();
            this._TargetID = TargetID;
            this._ProjectID = ProjectID;
            this._Schema = Schema;
            this._iProject = iProject;
            this.initControl();
        }

        #endregion
        
        
        #region Control
        private void initControl()
        {
            string SQL = "SELECT T.DatabaseName, TransferProtocol, IncludeInTransfer, TransferErrors, T.Server, T.Port " +
                "FROM ProjectTarget P, [Target] T " +
                "WHERE T.TargetID = P.TargetID " +
                " AND ProjectID = " + this._ProjectID.ToString() +
                " AND T.TargetID = " + this._TargetID.ToString();
            string Message = "";
            this._dtTarget = new DataTable();
            this.buttonDelete.Visible = false;
            DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref this._dtTarget, ref Message);
            if (this._dtTarget.Rows.Count > 0)
            {
                this.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(this._ControlHeight);
                this._PostgresDatabase = this._dtTarget.Rows[0]["DatabaseName"].ToString();
                this.labelTarget.Text = this._dtTarget.Rows[0]["DatabaseName"].ToString();// this._PostgresDatabase;
                if (!this._dtTarget.Rows[0]["TransferProtocol"].Equals(System.DBNull.Value) && this._dtTarget.Rows[0]["TransferProtocol"].ToString().Length > 0)
                    this.buttonTransferProtocol.Enabled = true;
                if (!this._dtTarget.Rows[0]["TransferErrors"].Equals(System.DBNull.Value) && this._dtTarget.Rows[0]["TransferErrors"].ToString().Length > 0)
                    this.buttonTransferErrors.Enabled = true;
                if (!this._dtTarget.Rows[0]["IncludeInTransfer"].Equals(System.DBNull.Value) && this._dtTarget.Rows[0]["IncludeInTransfer"].ToString() == "True")
                    this.checkBoxIncludeInTransfer.Checked = true;
                else this.checkBoxIncludeInTransfer.Checked = false;
                if (this._dtTarget.Rows[0]["Server"].ToString() == DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name
                    && this._dtTarget.Rows[0]["Port"].ToString() == DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Port.ToString()
                    )
                {
                    if (DiversityWorkbench.PostgreSQL.Connection.Databases().ContainsKey(this._PostgresDatabase))
                    {
                        this.labelTarget.ForeColor = System.Drawing.Color.Black;
                        this.listBoxPackages.ForeColor = System.Drawing.Color.Black;
                    }
                    else
                    {
                        this.labelTarget.ForeColor = System.Drawing.Color.Red;
                        this.listBoxPackages.ForeColor = System.Drawing.Color.Red;
                        this.buttonDelete.Visible = true;
                    }
                }
                if (this._dtTarget.Rows[0]["Server"].ToString() != DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name ||
                    this._dtTarget.Rows[0]["Port"].ToString() != DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Port.ToString())
                {
                    this.buttonServer.Visible = true;
                    this.buttonDelete.Visible = true;
                }
                this.getPackages();
            }
            else
                this.Height = 0;
        }

        private void getPackages()
        {
            //string SQL = "SELECT \"Package\"  FROM \"" + this._Schema + "\".\"Package\" ORDER BY \"Package\"";
            string SQL = "SELECT P.Package " +
                         "FROM ProjectTargetPackage P " +
                         "WHERE ProjectID = " + this._ProjectID.ToString() +
                         " AND TargetID = " + this._TargetID.ToString();
            string Message = "";

            //bool OK = DiversityDescriptions.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message);
            //if (DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtPackage, ref Message, this._PostgresDatabase))
            System.Data.DataTable dtPackage = new DataTable();
            if (CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dtPackage, ref Message))
            //    string Message = "";
            ////System.Data.DataTable dtPackage = new DataTable();
            //if (DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtPackage, ref Message, this._PostgresDatabase))
            {
                this.listBoxPackages.DataSource = dtPackage;
                this.listBoxPackages.DisplayMember = "Package";
                this.listBoxPackages.ValueMember = "Package";
            }
            if (dtPackage.Rows.Count > 1)
            {
                int h = dtPackage.Rows.Count - 1;
                h = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(this._ControlHeight - 4) * h;
                h += DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(this._ControlHeight);
                this.Height = h;
            }
        }

        #endregion

        #region Button events
        private void buttonTransferProtocol_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Transfer protocol", this._dtTarget.Rows[0]["TransferProtocol"].ToString(), true);
            f.ShowDialog();
        }

        private void buttonTransferErrors_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Transfer errors", this._dtTarget.Rows[0]["TransferErrors"].ToString(), true);
            f.ShowDialog();
        }

        private void buttonServer_Click(object sender, EventArgs e)
        {
            string Message = "Server: " + this._dtTarget.Rows[0]["Server"].ToString() + "\r\nPort: " + this._dtTarget.Rows[0]["Port"].ToString();
            System.Windows.Forms.MessageBox.Show(Message, "Different Server", MessageBoxButtons.OK);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Do you really want to remove this target from the list?", "Remove target?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            string Message = "";
            string SQL = "DELETE P " +
                "FROM ProjectTargetPackage P " +
                "WHERE ProjectID = " + this._ProjectID.ToString() +
                " AND TargetID = " + this._TargetID.ToString();
            bool OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message);
            if (OK)
            {
                SQL = "DELETE P " +
                    "FROM ProjectTarget P " +
                    "WHERE ProjectID = " + this._ProjectID.ToString() +
                    " AND TargetID = " + this._TargetID.ToString();
                OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message);
            }
            if (OK && Message.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Target removed");
                this._iProject.initOtherTargets();
            }
            else
            {
                if (Message.Length > 0)
                    Message = "Failed to remove target: " + Message;
                System.Windows.Forms.MessageBox.Show(Message);
            }
        }

        #endregion

        #region Interface
        public int HeightForDisplay()
        {
            return this.Height;
        }

        #endregion


    }
}
