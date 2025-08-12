using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormLoginStatistics : Form
    {
        #region Parameter

        private string _Login;

        #endregion

        #region Construction

        public FormLoginStatistics(string Login)
        {
            InitializeComponent();
            this._Login = Login;
            this.initForm();
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

        #endregion

        #region Form and tabpages

        private void initForm()
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                this.Text = "Statistics for the login " + this._Login;
                this.tabControl.TabPages.Clear();
                if (this.DtDatabases.Rows.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("No databases found");
                    this.Close();
                }
                foreach (System.Data.DataRow R in this.DtDatabases.Rows)
                {
                    System.Windows.Forms.TabPage T = new TabPage(R[0].ToString());
                    if (this.initTabPage(T, R[0].ToString()))
                        this.tabControl.TabPages.Add(T);
                }
                if (this.tabControl.TabPages.Count == 0)
                {
                    this.tabControl.Visible = false;
                    System.Windows.Forms.Label L = new Label();
                    L.Text = "No activity in any database could be found for the login\r\n" + this._Login;
                    L.Dock = DockStyle.Fill;
                    L.TextAlign = ContentAlignment.MiddleCenter;
                    this.Controls.Add(L);
                }
            }
            catch (System.Exception ex)
            {
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private System.Data.DataTable _dtDatabases;
        private System.Data.DataTable DtDatabases
        {
            get
            {
                if (this._dtDatabases == null)
                {
                    this._dtDatabases = new DataTable();
                    string SQL = "use master; " +
                        "select DB.name from sys.databases DB " +
                        "where DB.name not in ('master','tempdb','model','msdb','ReportServer','ReportServerTempDB')";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(this._dtDatabases);
                }
                return this._dtDatabases;
            }
        }

        private bool initTabPage(System.Windows.Forms.TabPage T, string DatabaseName)
        {
            bool ContainsData = false;
            System.Data.DataTable DtActivity = new DataTable();
            System.Data.DataTable dt = this.DtTables(DatabaseName);
            foreach (System.Data.DataRow R in dt.Rows)
            {
                string SQL = "use " + DatabaseName + "; " +
                    "select '" + R[0].ToString() + "' AS [Table], MIN(T.LogUpdatedWhen) AS [From], MAX(T.LogUpdatedWhen) as [To], COUNT(*) AS [Number] " +
                    "from [" + R[0].ToString() + "] AS T " +
                    "where T.LogUpdatedBy = '" + this._Login + "' having COUNT(*) > 0";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                try
                {
                    ad.Fill(DtActivity);
                }
                catch (System.Exception ex) { }
            }
            if (DtActivity.Rows.Count > 0)
            {
                ContainsData = true;
                System.Windows.Forms.DataGridView DGV = new DataGridView();
                T.Controls.Add(DGV);
                DGV.Dock = DockStyle.Fill;
                DGV.DataSource = DtActivity;
                DGV.AllowDrop = false;
                DGV.AllowUserToAddRows = false;
                DGV.ReadOnly = true;
                DGV.AutoResizeColumns();
            }
            return ContainsData;
        }

        private System.Data.DataTable DtTables(string DatabaseName)
        {
            System.Data.DataTable dt = new DataTable();
            string SQL = "use " + DatabaseName + "; " +
                "select distinct C.TABLE_NAME from INFORMATION_SCHEMA.COLUMNS C, INFORMATION_SCHEMA.TABLES T where C.COLUMN_NAME in ('LogUpdatedBy','LogUpdatedWhen') " +
                "and C.TABLE_NAME not like '%_log' and C.TABLE_NAME not like '%_log_%' " +
                "and C.TABLE_NAME = T.TABLE_NAME " +
                "and T.TABLE_TYPE = 'BASE TABLE'";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            return dt;
        }

        #endregion
    }
}
