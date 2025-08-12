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
    /// <summary>
    /// with this form either a user is provided with the website for a download of the latest version
    /// or an administrator can upload the current version to a website 
    /// and then set the ClientVersion in the database to the current version
    /// </summary>
    public partial class FormUpdateClient : Form
    {
        private string _ClientVersionUsed = "";
        private string _ClientVersionAvailable = "";

        public FormUpdateClient(string PathForDownload, string ClientVersionUsed, string ClientVersionAvailable)
        {
            InitializeComponent();
            System.Uri U = new Uri(PathForDownload);
            this.webBrowser.Url = U;
            this._ClientVersionUsed = ClientVersionUsed;
            this._ClientVersionAvailable = ClientVersionAvailable;
            this.labelHeader.Text = "Client used: " + ClientVersionUsed + "\r\nClient available: " + ClientVersionAvailable;
            this.initForm();
            this.Text = "Update to new client version";
            if (ClientVersionAvailable.CompareTo(ClientVersionUsed) > 0)
                this.Text += " " + ClientVersionAvailable;
            else
                this.Text += " " + ClientVersionUsed;
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


        private void initForm()
        {
            string SQL = "select user_name()";
            string User = "";
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    User = C.ExecuteScalar()?.ToString() ?? string.Empty;
                    con.Close();
                    if (User == "dbo")
                    {
                        this.tableLayoutPanelSetClientVersion.Visible = true;
                        this.buttonSetClientVersion.Text += " to " + this._ClientVersionUsed;
                        //this.splitContainer.SplitterDistance = this.splitContainer.Height - 55;
                    }
                    else this.tableLayoutPanelSetClientVersion.Visible = false;
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }

        }

        private void buttonSetClientVersion_Click(object sender, EventArgs e)
        {
            string SQL = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VersionClient]') " +
                "AND type in (N'FN', N'IF', N'TF', N'FS', N'FT')) " +
                "BEGIN SELECT 1 END " +
                "ELSE BEGIN SELECT 0 END ";
            string FunctionExists = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (FunctionExists != "1")
            {
                SQL = "CREATE FUNCTION [dbo].[VersionClient] () RETURNS nvarchar(11) AS BEGIN RETURN '" + this._ClientVersionAvailable + "' END";
            }
            else SQL = "ALTER FUNCTION [dbo].[VersionClient] () RETURNS nvarchar(11) AS BEGIN RETURN '" + this._ClientVersionAvailable + "' END";
            DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
        }
    }
}
