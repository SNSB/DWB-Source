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
    public partial class UserControlOverviewProject : UserControl
    {
        #region Parameter

        public enum DatabaseManagementSystem { MSSQL, Postgres }
        private DatabaseManagementSystem _DatabaseManagementSystem = DatabaseManagementSystem.MSSQL;
        private string _ConnectionString;
        private InterfaceCacheDB _InterfaceCacheDB;
        private string _ProjectName;
        private int _ProjectID;
        bool _TransferAll;
        
        #endregion

        #region Construction

        public UserControlOverviewProject(string Project, int NumberOfDatasets, System.DateTime LastUpdate)
        {
            InitializeComponent();
            this.buttonTransfer.Visible = false;
            if (Project.Length > 0)
            {
                this.labelProject.Text = Project;
                this._ProjectName = Project;
                this.labelCount.Text = NumberOfDatasets.ToString();
                if (LastUpdate.Year != 1)
                {
                    string Update = LastUpdate.Year.ToString() + "-";
                    if (LastUpdate.Month < 10) Update += "0";
                    Update += LastUpdate.Month.ToString() + "-";
                    if (LastUpdate.Day < 10) Update += "0";
                    Update += LastUpdate.Day.ToString();
                    Update += " ";
                    if (LastUpdate.Hour < 10) Update += "0";
                    Update += LastUpdate.Hour.ToString() + ":";
                    if (LastUpdate.Minute < 10) Update += "0";
                    Update += LastUpdate.Minute.ToString() + ":";
                    if (LastUpdate.Second < 10) Update += "0";
                    Update += LastUpdate.Second.ToString();
                    this.labelDate.Text = "Last upd.: " + Update;
                }
                else this.labelDate.Text = "";
            }
            else
            {
                this.labelProject.Text = "";
                this.labelCount.Text = "";
                this.labelDate.Text = "";
            }
        }

        public UserControlOverviewProject(string Project, int ProjectID, DatabaseManagementSystem DBMS, string ConnectionString, InterfaceCacheDB Interface)
        {
            InitializeComponent();
            this.buttonTransfer.Visible = false;
            if (Project.Length > 0)
            {
                this.labelProject.Text = Project;
                this._ProjectName = Project;
            }
            else
            {
                this.labelProject.Text = "";
                this.labelCount.Text = "";
                this.labelDate.Text = "";
            }
            this.buttonTransfer.Visible = true;
            this._ConnectionString = ConnectionString;
            this._ProjectID = ProjectID;
            this._InterfaceCacheDB = Interface;
            this._DatabaseManagementSystem = DBMS;
            this.buttonTransfer.Visible = true;
            this.RefreshDisplay();
        }
        
        #endregion

        public void TransferButtonIsVisible(bool isVisible)
        {
            this.buttonTransfer.Visible = isVisible;
        }

        public void TransferAll(bool TransferAll)
        {
            this._TransferAll = TransferAll;
            if (this._TransferAll)
            {
                this.buttonTransfer.Image = this.imageList.Images[1];
                this.toolTip.SetToolTip(this.buttonTransfer, "Transfer the data for all projects in the database");
            }
            else
            {
                this.buttonTransfer.Image = this.imageList.Images[0];
                this.toolTip.SetToolTip(this.buttonTransfer, "Transfer the data for this project");
            }
        }

        public void StartTransfer()
        {
            bool OK = false;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                switch (this._DatabaseManagementSystem)
                {
                    //case DatabaseManagementSystem.MSSQL:
                    //    OK = this._InterfaceCacheDB.TransferDataToCacheDB();
                    //    if (OK)
                    //        this._InterfaceCacheDB.initOverviewCacheDB();
                    //    break;
                    case DatabaseManagementSystem.Postgres:
                        OK = this._InterfaceCacheDB.TransferProjectDataToPostgresDB(this._ProjectName, null);
                        if (OK)
                            this.RefreshDisplay();
                        break;
                }
            }
            catch (System.Exception ex)
            {
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
            if (OK)
            {
                switch (this._DatabaseManagementSystem)
                {
                    //case DatabaseManagementSystem.MSSQL:
                    //    System.Windows.Forms.MessageBox.Show("Import of data in cache database finished");
                    //    break;
                    case DatabaseManagementSystem.Postgres:
                        System.Windows.Forms.MessageBox.Show("Import of data for project " + this._ProjectName + " finished");
                        break;
                }
            }
        }

        #region Events

        private void buttonTransfer_Click(object sender, EventArgs e)
        {
            this.StartTransfer();
        }

        private void RefreshDisplay()
        {
            string SQL = "";
            string LastUpdateString = "";
            int Nr = 0;
            switch (this._DatabaseManagementSystem)
            {
                case DatabaseManagementSystem.MSSQL:
                    SQL = "SELECT COUNT(*) FROM ProjectPublished P where ProjectID = " + this._ProjectID.ToString();
                    int i = 0;
                    if (int.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL), out i) && i > 0)
                    {
                        this.buttonTransfer.Visible = true;
                        SQL = "SELECT COUNT(*) FROM CollectionProject P, CollectionSpecimenCache AS S " +
                            "WHERE S.CollectionSpecimenID = P.CollectionSpecimenID  AND ProjectID = " + this._ProjectID.ToString();
                        Nr = int.Parse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL));
                        SQL = "SELECT    CONVERT(varchar(20), S.LogInsertedWhen, 126)  AS LastUpdate " +
                            "FROM CollectionSpecimenCache AS S INNER JOIN " +
                            "CollectionProject P ON S.CollectionSpecimenID = P.CollectionSpecimenID " +
                            "WHERE P.ProjectID = " + this._ProjectID.ToString();
                        LastUpdateString = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                        this.buttonTransfer.Visible = true;
                    }
                    else
                    {
                        this.buttonTransfer.Visible = false;
                    }
                    break;
                case DatabaseManagementSystem.Postgres:
                    try
                    {
                        SQL = "select count(*) from information_schema.schemata WHERE schema_Name = '" + this._ProjectName + "'";
                        int p = 0;
                        if (int.TryParse(DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL), out p) && p > 0)
                        {
                            if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)// .Postgres.PostgresConnection().ConnectionString.Length > 0)
                            {
                                SQL = "SELECT COUNT(*) FROM \"CollectionSpecimenCache\" AS S " +
                                    "WHERE S.\"ProjectID\" = " + this._ProjectID.ToString();
                                Nr = int.Parse(DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL));// .Postgres.PostgresExecuteSqlSkalar(SQL));
                                SQL = "SELECT S.\"InsertTimestamp\" AS LastUpdate " +
                                    "FROM \"CollectionSpecimenCache\" AS S " +
                                    "WHERE S.\"ProjectID\" = " + this._ProjectID.ToString();
                                LastUpdateString = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);// .Postgres.PostgresExecuteSqlSkalar(SQL);
                                this.buttonTransfer.Visible = true;
                            }
                        }
                        else
                        {
                            this.buttonTransfer.Visible = false;
                        }
                    }
                    catch (System.Exception ex)
                    { }

                    break;
            }
            try
            {
                this.labelCount.Text = Nr.ToString();
                if (LastUpdateString.Length > 0)
                {
                    System.DateTime LastUpdate;
                    System.DateTime.TryParse(LastUpdateString, out LastUpdate);
                    if (LastUpdate.Year != 1)
                    {
                        string Update = LastUpdate.Year.ToString() + "-";
                        if (LastUpdate.Month < 10) Update += "0";
                        Update += LastUpdate.Month.ToString() + "-";
                        if (LastUpdate.Day < 10) Update += "0";
                        Update += LastUpdate.Day.ToString();
                        Update += " ";
                        if (LastUpdate.Hour < 10) Update += "0";
                        Update += LastUpdate.Hour.ToString() + ":";
                        if (LastUpdate.Minute < 10) Update += "0";
                        Update += LastUpdate.Minute.ToString() + ":";
                        if (LastUpdate.Second < 10) Update += "0";
                        Update += LastUpdate.Second.ToString();
                        this.labelDate.Text = "Last upd.: " + Update;
                    }
                }
                else this.labelDate.Text = "";
            }
            catch (System.Exception ex)
            { }

        }

        #endregion
    }
}
