using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormCreateUser : Form
    {
        #region Parameter

        private DiversityWorkbench.ServerConnection _ServerConnection;
        private Microsoft.Data.SqlClient.SqlConnection _SqlConnection;
        private System.Data.DataTable _dtUser;

        #endregion

        #region Construction
        //public FormCreateUser(DiversityWorkbench.ServerConnection S)
        //{
        //    InitializeComponent();
        //    this.labelHeader.Text += S.DatabaseName + " on server " + S.DatabaseServer;
        //    this._ServerConnection = S;
        //}

        public FormCreateUser(Microsoft.Data.SqlClient.SqlConnection SqlConnection)
        {
            InitializeComponent();
            this._SqlConnection = SqlConnection;
            this.labelHeader.Text += "\r\nfor the database " + SqlConnection.Database + "\r\non server " + SqlConnection.DataSource;
            this.fillPicklist();
        }
        
        #endregion

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void fillPicklist()
        {
            string idUri = global::DiversityWorkbench.Properties.Settings.Default.DiversityWokbenchIDUrl + "users/";
            string SQL = "SELECT " + idUri + " CAST(UserID AS VARCHAR) AS UserURI, CombinedNameCache FROM DiversityUsers.dbo.UserInfo  WHERE CombinedNameCache NOT LIKE ':%' ORDER BY CombinedNameCache";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._SqlConnection.ConnectionString);
            this._dtUser = new DataTable();
            try
            {
                ad.Fill(this._dtUser);
                this.comboBoxUserName.DataSource = this._dtUser;
                this.comboBoxUserName.DisplayMember = "CombinedNameCache";
                this.comboBoxUserName.ValueMember = "UserURI";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #region Dialog
		private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (this.comboBoxUserName.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select or enter a Name for the user");
                return;
            }
            if (this.textBoxLogin.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please enter a login for the user");
                return;
            }
            if (this.textBoxPassword1.Text.Length < 8)
            {
                System.Windows.Forms.MessageBox.Show("Please enter a password with at least 8 letters");
                return;
            }
            if (this.textBoxPassword1.Text != this.textBoxPassword2.Text)
            {
                System.Windows.Forms.MessageBox.Show("The passwords do not match");
                return;
            }
            string SQL = "CREATE LOGIN " + this.textBoxLogin.Text +
                " WITH PASSWORD = '" + this.textBoxPassword1.Text + "'; " +
                " USE " + this._SqlConnection.Database + "; " +
                " CREATE USER " + this.textBoxLogin.Text + " FOR LOGIN " + this.textBoxLogin.Text +
                " WITH DEFAULT_SCHEMA = " + this._SqlConnection.Database + "; ";// +
                //" GO";
            //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this._ServerConnection.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, this._SqlConnection);
            try
            {
                if (this._SqlConnection.State == ConnectionState.Closed)
                    this._SqlConnection.Open();
                C.ExecuteNonQuery();
                this._SqlConnection.Close();
                this.DialogResult = DialogResult.OK;
            }
            catch 
            {
                this.DialogResult = DialogResult.Cancel;
            }
            this.Close();
        }
 
	    #endregion   
 
        #region Properties
        public string UserName { get { return this.comboBoxUserName.Text; } }
        public string Login { get { return this.textBoxLogin.Text; } }
        public string UserURI
        { 
            get 
            {
                return this.comboBoxUserName.SelectedValue.ToString();
                //int? ID = null;
                //if (this.comboBoxUserName.DataSource != null)
                //{
                //    if (this.comboBoxUserName.SelectedIndex > -1)
                //    {
                //        int i;
                //        if (int.TryParse(this.comboBoxUserName.SelectedValue.ToString(), out i)) ID = i;
                //    }
                //}
                //return ID;
            } 
        }
        #endregion
    }
}