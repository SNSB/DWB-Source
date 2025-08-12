using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormSynchronisation : Form
    {
        public FormSynchronisation()
        {
            InitializeComponent();
            this.initForm();
        }

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



        public void initForm()
        {
            this.initDatabases();
            //this.initLogins();
        }

        private void initLogins()
        {
            try
            {
                if (this.comboBoxDatabase.SelectedIndex == -1)
                    return;
                string Database = "";
                if (this.comboBoxDatabase.SelectedValue.GetType() == typeof(System.Data.DataRowView))
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxDatabase.SelectedValue;
                    Database = R[0].ToString();
                }
                else if (this.comboBoxDatabase.SelectedValue.GetType() == typeof(string))
                    Database = this.comboBoxDatabase.SelectedValue.ToString();
                string SQL = "USE " + Database + "; SELECT name " +
                    "FROM sys.sysusers AS s " +
                    "WHERE (islogin = 1) AND (name <> 'sys') " +
                    "AND (name <> 'INFORMATION_SCHEMA') " +
                    "AND (hasdbaccess = 1) " +
                    "AND s.name not in (select SCHEMA_OWNER from INFORMATION_SCHEMA.SCHEMATA) " +
                    "ORDER BY name ";
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                System.Data.DataTable dtLogins = new DataTable();
                a.Fill(dtLogins);
                this.comboBoxUser.DataSource = dtLogins;
                this.comboBoxUser.DisplayMember = "name";
                this.comboBoxUser.ValueMember = "name";
                if (dtLogins.Rows.Count == 0)
                {
                    this.comboBoxUser.Text = "";
                    System.Windows.Forms.MessageBox.Show("No user without a schema found");
                    this.buttonCreateSynchroTables.Enabled = false;
                }
                else
                    this.buttonCreateSynchroTables.Enabled = true;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void initDatabases()
        {
            string SQL = "SELECT name FROM sys.databases where name  LIKE 'Synchronisation%' ORDER BY name";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                System.Data.DataTable dt = new DataTable();
                ad.Fill(dt);
                this.comboBoxDatabase.DataSource = dt;
                this.comboBoxDatabase.DisplayMember = "name";
                this.comboBoxDatabase.ValueMember = "name";
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void buttonCreateSynchroTables_Click(object sender, EventArgs e)
        {
            string Database = this.comboBoxDatabase.SelectedValue.ToString();
            string User = this.comboBoxUser.SelectedValue.ToString();
            string Schema = User;
            if (Schema.IndexOf("\\") > -1)
                Schema = Schema.Substring(Schema.IndexOf("\\") + 1);
            string SQL = "USE " + Database;
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                C.ExecuteNonQuery();
                C.CommandText = @"CREATE SCHEMA [" + Schema + "] AUTHORIZATION [" + User + "]";
                C.ExecuteNonQuery();
                C.CommandText = "CREATE TABLE [" + Schema + "].[SyncItem]( " +
                    "[SyncID] [int] IDENTITY(1,1) NOT NULL, " +
                    "[SyncFK] [int] NULL, " +
                    "[ClassID] [nvarchar](255) NULL, " +
                    "[HashCode] [nvarchar](32) NULL, " +
                    "[SyncGuid] [uniqueidentifier] ROWGUIDCOL  NULL, " +
                    "[RowGuid] [uniqueidentifier] NULL, " +
                    "CONSTRAINT [PK_SyncItem] PRIMARY KEY CLUSTERED " +
                    "([SyncID] ASC)) ON [PRIMARY]";
                C.ExecuteNonQuery();
                con.Close();
                System.Windows.Forms.MessageBox.Show("Schema and tables created");
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void comboBoxDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.initLogins();
        }


    }
}
