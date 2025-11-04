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
    public partial class FormCreateLogin : Form
    {

        #region Parameter

        private string _Database;
        private System.Data.DataTable _dtDiversityAgents;
        private System.Data.DataTable _dtCountry;

        private System.Data.DataTable _dtTitle;

        private DiversityWorkbench.Agent _Agent;

        private DiversityWorkbench.Login _Login;

        #endregion

        #region Construction and form

        public FormCreateLogin(string Database)
        {
            InitializeComponent();
            this._Database = Database;
            this.setAddressControls();
            this.SetDiversityAgentsList();
            this.userControlDialogPanel.buttonOK.Click -= this.userControlDialogPanel.buttonOK_Click;
            this.userControlDialogPanel.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            this._Agent = new Agent(DiversityWorkbench.Settings.ServerConnection);
            this.userControlModuleRelatedEntryAgentFromDatabase.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)this._Agent;
            this.userControlModuleRelatedEntryAgentFromDatabase.labelURI.TextChanged += new System.EventHandler(this.userControlModuleRelatedEntryAgentFromDatabase_labelURI_TextChanged);
            this.userControlDialogPanel.buttonOK.Enabled = false;
        }

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void userControlModuleRelatedEntryAgentFromDatabase_labelURI_TextChanged(object sender, EventArgs e)
        {
            this.CheckIfOK();
        }


        #endregion

        #region Events

        private void radioButtonWindowsAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            this.labelUserName.Enabled = this.radioButtonWindowsAuthentication.Checked;
            this.comboBoxUserName.Enabled = this.radioButtonWindowsAuthentication.Checked;
            this.labelPassword.Enabled = !this.radioButtonWindowsAuthentication.Checked;
            this.labelLogin.Enabled = !this.radioButtonWindowsAuthentication.Checked;
            this.textBoxLogin.Enabled = !this.radioButtonWindowsAuthentication.Checked;
            this.textBoxPassword1.Enabled = !this.radioButtonWindowsAuthentication.Checked;
            this.textBoxPassword2.Enabled = !this.radioButtonWindowsAuthentication.Checked;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (this.CheckIfOK() && this.CreateLogin())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        #endregion

        #region Interface

        public bool IsSqlServerLogin
        { get { if (this.radioButtonSqlServerAuthentication.Checked) return true; else return false; } }

        public string LoginName
        { get { return this.textBoxLogin.Text; } }

        public string LoginAgentName
        {
            get
            {
                    return this.userControlModuleRelatedEntryAgentFromDatabase.textBoxValue.Text;
                //if (this.radioButtonNewAgent.Checked)
                //    return this.textBoxGivenName.Text + " " + this.textBoxInheritedName.Text;
                //else if (this.radioButtonAgentFromDatabase.Checked || this.userControlModuleRelatedEntryAgentFromDatabase.labelURI.Text.Length > 0)
                //else
                //    return "";
            }
        }

        public string LoginAgentURI
        {
            get
            {
                    return this.userControlModuleRelatedEntryAgentFromDatabase.labelURI.Text;
                //if (this.radioButtonNewAgent.Checked)
                //    return "";
                //else if (this.radioButtonAgentFromDatabase.Checked || this.userControlModuleRelatedEntryAgentFromDatabase.labelURI.Text.Length > 0)
                //else
                //    return "";
            }
        }

        #endregion

        #region Login

        public DiversityWorkbench.Login Login() { return this._Login; }

        private bool CreateLogin()
        {
            //if (!this.CheckIfOK()) 
            //    return false;
            string Login = this.textBoxLogin.Text;
            string SQL = "CREATE LOGIN [" + Login +
                "] WITH PASSWORD = '" + this.textBoxPassword1.Text + "', DEFAULT_DATABASE=[" + this._Database + "], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON;";
            // Markus 2.5.23: Resetting connection to current server
            DiversityWorkbench.Settings.Connection = null;
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, DiversityWorkbench.Settings.Connection);
            try
            {
                if (DiversityWorkbench.Settings.Connection.State == ConnectionState.Closed)
                    DiversityWorkbench.Settings.Connection.Open();
                C.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                DiversityWorkbench.Settings.Connection.Close();
            }
            SQL = " USE " + this._Database + "; IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'" + Login + "') " +
                " CREATE USER [" + Login + "] FOR LOGIN [" + Login + "] WITH DEFAULT_SCHEMA=[dbo]; ";
            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
            {
                if (this._Login == null || this._Login.LoginName().Length == 0)
                    this._Login = new Login(Login);
                if (this.SaveLoginAgentInfos())
                    return true;
            }
            return false;

            /*
             * Funktion von P.Grobe:
             * 
             * 	SET NOCOUNT ON;
	DECLARE @ErrorMessage nvarchar(4000),  @ErrorSeverity int;
	DECLARE @DropUserStatement nvarchar(200) = 'DROP USER  ' + @LoginName;
	DECLARE @DefaultRole nvarchar(200) = 'Editor';

	IF @Action='Create' BEGIN
		BEGIN TRY
			DECLARE @CreateDB_UserStatement nvarchar(200);
			SET @CreateDB_UserStatement = 'CREATE USER ' +	@LoginName + ' FOR LOGIN ' + @LoginName;
			EXEC sp_executesql @CreateDB_UserStatement;
		END TRY
		BEGIN CATCH
			SELECT @ErrorMessage = ERROR_MESSAGE(),@ErrorSeverity = ERROR_SEVERITY();
			RAISERROR(@ErrorMessage, @ErrorSeverity, 1);
		END CATCH;

		BEGIN TRY
			-- DECLARE @UserRolesStatement nvarchar(200) = 'ALTER ROLE '+@DefaultRole+' ADD MEMBER ' + @LoginName; -- geht nicht, wegen reserviertem Wort `User`!?!
			EXEC sp_addrolemember @DefaultRole, @LoginName;
		END TRY
		BEGIN CATCH
			SELECT @ErrorMessage = ERROR_MESSAGE(),@ErrorSeverity = ERROR_SEVERITY();
			EXEC sp_executesql @DropUserStatement;
			RAISERROR(@ErrorMessage, @ErrorSeverity, 1);
		END CATCH;
	END ELSE BEGIN
		IF @Action='Remove' BEGIN
			EXEC sp_droprolemember @DefaultRole, @LoginName;
			EXEC sp_executesql @DropUserStatement;
		END
	END

             * */
        }

        private bool LoginExists
        {
            get
            {
                int i;
                string SQL = "use master; " +
                    "select count(*) from sys.syslogins l " +
                    "where (l.loginname = '" + this.textBoxLogin.Text + "');";
                if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out i) && i == 0)
                    return false;
                return true;
            }
        }

        #endregion

        #region Agents & Address

        public System.Data.DataTable DtDiversityAgents
        {
            get
            {
                if (this._dtDiversityAgents == null)
                {
                    this._dtDiversityAgents = new DataTable();
                    System.Data.DataColumn cDatabase = new DataColumn("Database", typeof(string));
                    this._dtDiversityAgents.Columns.Add(cDatabase);
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.WorkbenchUnit> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList())
                    {
                        if (KV.Value.ServerConnection.ModuleName == "DiversityAgents")
                        {
                            foreach (string D in KV.Value.DatabaseList())
                            {
                                System.Data.DataRow R = this._dtDiversityAgents.NewRow();
                                R[0] = D;
                                this._dtDiversityAgents.Rows.Add(R);
                            }
                        }
                    }
                }
                return _dtDiversityAgents;
            }
            //set { _dtDiversityAgents = value; }
        }

        private void SetDiversityAgentsList()
        {
            this.comboBoxDatabase.DataSource = this.DtDiversityAgents;
            this.comboBoxDatabase.DisplayMember = "Database";
            this.comboBoxDatabase.ValueMember = "Database";
        }

        private void comboBoxDatabase_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void radioButtonAddress_CheckedChanged(object sender, EventArgs e)
        {
            this.setAddressControls();
        }

        private void radioButtonInstution_CheckedChanged(object sender, EventArgs e)
        {
            this.setAddressControls();
        }

        private void setAddressControls()
        {
            this.textBoxCity.Enabled = this.radioButtonNewAgent.Checked;
            this.comboBoxCountry.Enabled = this.radioButtonNewAgent.Checked;
            this.labelCity.Enabled = this.radioButtonNewAgent.Checked;
            this.labelCountry.Enabled = this.radioButtonNewAgent.Checked;
            this.labelGivenName.Enabled = this.radioButtonNewAgent.Checked;
            this.labelInheritedName.Enabled = this.radioButtonNewAgent.Checked;
            this.labelTitle.Enabled = this.radioButtonNewAgent.Checked;
            this.textBoxGivenName.Enabled = this.radioButtonNewAgent.Checked;
            this.textBoxInheritedName.Enabled = this.radioButtonNewAgent.Checked;
            this.comboBoxTitle.Enabled = this.radioButtonNewAgent.Checked;

            this.userControlModuleRelatedEntryAgentFromDatabase.Enabled = true;// this.radioButtonAgentFromDatabase.Checked;
        }

        private bool SaveLoginAgentInfos()
        {
            if (this.radioButtonNewAgent.Checked)
            {
                try
                {
                    string SQL = "USE " + this.comboBoxDatabase.SelectedValue.ToString() + " GO INSERT INTO Agent " +
                        "(AgentParentID, AgentTitle, GivenName, InheritedName, AgentType) " +
                        "VALUES (1, '" + this.comboBoxTitle.Text + "', '" + this.textBoxGivenName.Text + "', '" +
                        this.textBoxInheritedName.Text + "', 'person') SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
                    string sAgentID = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    int AgentID;
                    if (int.TryParse(sAgentID, out AgentID))
                    {
                        SQL = "USE " + this.comboBoxDatabase.SelectedValue.ToString() + " GO INSERT INTO AgentContactInformation " +
                            "(AgentID, Country, City) " +
                            "VALUES (" + AgentID.ToString() + ", '" + this.comboBoxCountry.Text + "', '" + this.textBoxCity.Text + "')";
                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);

                        // altes Konzept - wohl ueberholt, jetzt alles in UserProxy

                        //SQL = "USE " + this.comboBoxDatabase.SelectedValue.ToString() + " GO INSERT INTO AgentLogin " +
                        //    "(LoginName, AgentID) " +
                        //    "VALUES ('" + this.textBoxLogin.Text + "', " + AgentID.ToString() + ")";
                        //DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            // altes Konzept - wohl ueberholt, jetzt alles in UserProxy

            //else if (this.radioButtonAgentFromDatabase.Checked)
            //{
            //    string AgentURI = this.userControlModuleRelatedEntryAgentFromDatabase.labelURI.Text;
            //    int AgentID = int.Parse(DiversityWorkbench.WorkbenchUnit.getIDFromURI(AgentURI));
            //    string SQL = "USE " + this.comboBoxDatabase.Text + "; INSERT INTO AgentLogin " +
            //        "(LoginName, AgentID) " +
            //        "VALUES ('" + this.textBoxLogin.Text + "', " + AgentID.ToString() + ")";
            //    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
            //        return true;
            //    else return false;
            //}

            else
                return true;

            return false;
        }

        private void comboBoxDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        public System.Data.DataTable DtTitle
        {
            get
            {
                if (this._dtTitle == null)
                {
                    string SQL = "USE " + this.comboBoxDatabase.SelectedValue.ToString() + " GO SELECT DISTINCT Code " +
                        "FROM DiversityAgents_Test.dbo.AgentTitle_Enum " +
                        "ORDER BY Code";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(this._dtTitle);
                }
                return _dtTitle;
            }
            //set { _dtTitle = value; }
        }

        public System.Data.DataTable DtCountry
        {
            get
            {
                if (this._dtCountry == null)
                {
                    this._dtCountry = new DataTable();
                    string GeoDatabase = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].DatabaseList()[0];
                    string SQL = "USE " + this.comboBoxDatabase.SelectedValue.ToString() + "; " +
                       "declare @Countries table(Country nvarchar(500)) " +
                       "BEGIN TRY " +
                       "INSERT INTO @Countries " +
                       "SELECT N.Name AS Country " +
                       "FROM " + GeoDatabase + ".dbo.GeoName AS N INNER JOIN " +
                       GeoDatabase + ".dbo.GeoPlace AS P ON N.PlaceID = P.PlaceID AND N.NameID = P.PreferredNameID " +
                       "WHERE (P.PlaceType = 'nation') " +
                       "END TRY " +
                       "BEGIN CATCH " +
                       "IF (SELECT COUNT(*) FROM @Countries) = 0 " +
                       "BEGIN " +
                       "INSERT INTO @Countries " +
                       "SELECT DISTINCT Country " +
                       "FROM  AgentContactInformation " +
                       "WHERE (Country <> N'') " +
                       "END " +
                       "END CATCH " +
                       "SELECT Country FROM @Countries ORDER BY Country";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(this._dtCountry);
                }
                return _dtCountry;
            }
            //set { _dtCountry = value; }
        }


        #endregion

        #region Check and PW

        private bool CheckIfOK()
        {
            if (this.textBoxLogin.Text.Length == 0 || (this.textBoxLogin.Text.Length > 0 && !this.CheckIfEntryOK(this.textBoxLogin.Text, false)))
            {
                System.Windows.Forms.MessageBox.Show("Please enter a valid name for the login");
                return false;
            }
            string Password = this.textBoxPassword1.Text;
            if (Password.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please enter a password");
                return false;
            }
            if (Password != this.textBoxPassword2.Text)
            {
                System.Windows.Forms.MessageBox.Show("The passwords do not match");
                return false;
            }
            if (this.LoginExists)
            {
                System.Windows.Forms.MessageBox.Show("The login allready exists on the server");
                return false;
            }
            if (this.radioButtonNewAgent.Checked)
            {
                if (this.comboBoxDatabase.SelectedValue.ToString().Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Please select a database to store details for the new login");
                    return false;
                }
                if (this.textBoxGivenName.Text.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Please enter the given name of the person related to the new login");
                    return false;
                }
                if (this.textBoxInheritedName.Text.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Please enter the inherited name of the person related to the new login");
                    return false;
                }
            }
            else if (this.radioButtonAgentFromDatabase.Checked || this.userControlModuleRelatedEntryAgentFromDatabase.labelURI.Text.Length > 0)
            {
                if (this.userControlModuleRelatedEntryAgentFromDatabase.labelURI.Text.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Please select a user from the database");
                    return false;
                }
                else
                {
                    if (this._Login == null)
                        this._Login = new Login(this.LoginName);
                    this._Login.SetLinkToDiversityAgents(this.userControlModuleRelatedEntryAgentFromDatabase.textBoxValue.Text, this.userControlModuleRelatedEntryAgentFromDatabase.labelURI.Text);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please select an agent from the database");
                //System.Windows.Forms.MessageBox.Show("Please enter a new agent or select one from the database");
                return false;
            }
            this.userControlDialogPanel.buttonOK.Enabled = true;
            return true;
        }

        private void textBoxPassword1_TextChanged(object sender, EventArgs e)
        {
            if (this.CheckPasswordMatch() && this.CheckIfEntryOK(this.textBoxPassword1.Text))
                this.CheckIfOK();
        }

        private void textBoxPassword2_TextChanged(object sender, EventArgs e)
        {
            if (this.CheckPasswordMatch() && this.CheckIfEntryOK(this.textBoxPassword2.Text))
                this.CheckIfOK();
        }

        private void comboBoxUserName_TextChanged(object sender, EventArgs e)
        {
            //this.CheckIfEntryOK(this.)
        }

        private void textBoxLogin_TextChanged(object sender, EventArgs e)
        {
            this.CheckIfEntryOK(this.textBoxLogin.Text, false);
        }

        private bool CheckPasswordMatch()
        {
            return DiversityWorkbench.Forms.FormFunctions.CheckPasswordMatch(this.textBoxPassword1.Text, this.textBoxPassword2.Text);
            //bool OK = false;
            //if (this.textBoxPassword1.Text == this.textBoxPassword2.Text && this.textBoxPassword2.Text.Length > 0)
            //{
            //    OK = true;
            //}
            //return OK;
        }

        private bool CheckIfEntryOK(string Entry, bool IsPW = true)
        {
            return DiversityWorkbench.Forms.FormFunctions.CheckIfEntryOK(Entry, IsPW);
            //bool OK = true;
            //string Forbidden = "";
            //string EntryType = "password";
            //if (!IsPW) EntryType = "login";
            //if(!NoForbiddenCharacter(Entry, ref Forbidden))
            //{
            //    string Message = "Invalid " + EntryType + ":\r\nThe character\r\n   \"" + Forbidden + "\"\r\nis not allowed";
            //    System.Windows.Forms.MessageBox.Show(Message, "Illegal " + EntryType, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    OK = false;
            //}
            //if (IsPW && Entry.Length < 8)
            //    OK = false;
            //return OK;
        }

        //private bool NoForbiddenCharacter(string Text, ref string Forbidden)
        //{
        //    bool OK = true;
        //    foreach(char c in Text)
        //    {
        //        if (ForbiddenCharacters.Contains(c))
        //        {
        //            Forbidden = c.ToString();
        //            OK = false;
        //            break;
        //        }
        //    }
        //    return OK;
        //}

        ////[] {}() , ; ? * ! @
        //private System.Collections.Generic.List<char> _ForbiddenCharacters;
        //private System.Collections.Generic.List<char> ForbiddenCharacters
        //{
        //    get
        //    {
        //        if (_ForbiddenCharacters == null)
        //        {
        //            _ForbiddenCharacters = new List<char>();
        //            _ForbiddenCharacters.Add(' ');
        //            _ForbiddenCharacters.Add('\'');
        //            _ForbiddenCharacters.Add('"');
        //            _ForbiddenCharacters.Add('=');
        //            _ForbiddenCharacters.Add('[');
        //            _ForbiddenCharacters.Add(']');
        //            _ForbiddenCharacters.Add('{');
        //            _ForbiddenCharacters.Add('}');
        //            _ForbiddenCharacters.Add('(');
        //            _ForbiddenCharacters.Add(')');
        //            _ForbiddenCharacters.Add(',');
        //            _ForbiddenCharacters.Add(';');
        //            _ForbiddenCharacters.Add('?');
        //            _ForbiddenCharacters.Add('*');
        //            _ForbiddenCharacters.Add('!');
        //            _ForbiddenCharacters.Add('@');
        //        }
        //        return _ForbiddenCharacters;
        //    }
        //}

        #endregion

    }
}
