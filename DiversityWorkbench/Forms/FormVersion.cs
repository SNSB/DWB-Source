using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormVersion : Form
    {
        #region Parameter
        //private string _VersionOfClientInClient;
        private string _VersionOfDbInClient;
        private int _VersionOfClientPartCount = 4;
        #endregion

        #region Construction

        public FormVersion(string VersionOfDbInClient, string LabelVersionDbInClient)
        {
            InitializeComponent();
            this._VersionOfDbInClient = VersionOfDbInClient;
            if (LabelVersionDbInClient.Length > 0) this.labelEditDbVersion.Text = LabelVersionDbInClient;
            this.initForm();
        }

        #endregion

        #region Form and button events

        private void initForm()
        {
            string SQL = "SELECT dbo.VersionClient()";
            string VersionOfClientInDB = "";
            string VersionDB = "";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                VersionOfClientInDB = C.ExecuteScalar()?.ToString() ?? string.Empty;
                C.CommandText = "SELECT dbo.Version()";
                VersionDB = C.ExecuteScalar()?.ToString() ?? string.Empty;
                con.Close();
            }
            catch { }
            this.textBoxCurrentVersionClient.Text = VersionOfClientInDB;
            this.textBoxCurrentVersionDatabase.Text = VersionDB;
            this.maskedTextBoxVersionDB.Text = VersionDB;

            string[] strArr = System.Windows.Forms.Application.ProductVersion.ToString().Split(new Char[] { '.' });
            if (strArr.Length == 3)
            {
                _VersionOfClientPartCount = 3;
                this.maskedTextBoxVersionClient.Mask = "00/00/00";
            }
            //char Null = System.Convert.ToChar("0");
            string VersionOfCurrentClient = "";
            for (int i = 0; i < strArr.Length; i++)
            {
                if (strArr[i].Length == 1)
                    VersionOfCurrentClient += "0";
                VersionOfCurrentClient += strArr[i];
                if (i < (_VersionOfClientPartCount - 1))
                    VersionOfCurrentClient += ".";
            }
            this.maskedTextBoxVersionClient.Text = VersionOfCurrentClient;

            this.textBoxOldVersionClientAsInClient.Text = VersionOfCurrentClient;
            this.textBoxOldVersionDbAsInClient.Text = this._VersionOfDbInClient;

            if (this.textBoxCurrentVersionClient.Text != this.textBoxOldVersionClientAsInClient.Text)
            {
                this.textBoxOldVersionClientAsInClient.BackColor = System.Drawing.Color.Red;
                this.textBoxCurrentVersionClient.BackColor = System.Drawing.Color.Red;
                this.labelEditClientVersion.Visible = true;
            }
            else
            {
                textBoxOldVersionClientAsInClient.BackColor = System.Drawing.SystemColors.Control;
                this.textBoxCurrentVersionClient.BackColor = System.Drawing.SystemColors.Control;
                this.labelEditClientVersion.Visible = false;
            }

            if (this.textBoxCurrentVersionDatabase.Text != this.textBoxOldVersionDbAsInClient.Text)
            {
                textBoxOldVersionDbAsInClient.BackColor = System.Drawing.Color.Red;
                textBoxCurrentVersionDatabase.BackColor = System.Drawing.Color.Red;
                this.labelEditDbVersion.Visible = true;
            }
            else
            {
                textBoxOldVersionDbAsInClient.BackColor = System.Drawing.SystemColors.Control;
                textBoxCurrentVersionDatabase.BackColor = System.Drawing.SystemColors.Control;
                this.labelEditDbVersion.Visible = false;
            }
        }

        private void buttonSetVersionClient_Click(object sender, EventArgs e)
        {
            string SQL = "ALTER FUNCTION [dbo].[VersionClient] () " +
                "RETURNS nvarchar(11) " +
                "AS BEGIN RETURN '" + this.maskedTextBoxVersionClient.Text.Replace('/', '.') + "' END";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                C.ExecuteNonQuery();
                con.Close();
            }
            catch { }
        }

        private void buttonSetVersionDatabase_Click(object sender, EventArgs e)
        {
            string SQL = "ALTER FUNCTION [dbo].[Version] () " +
                "RETURNS nvarchar(8) " +
                "AS BEGIN RETURN '" + this.maskedTextBoxVersionDB.Text.Replace('/', '.') + "' END";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                C.ExecuteNonQuery();
                con.Close();
            }
            catch { }
        }


        #endregion

        #region Interface

        public string VersionOfDbInClient
        {
            get { return _VersionOfDbInClient; }
            set { _VersionOfDbInClient = value; }
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



    }
}
