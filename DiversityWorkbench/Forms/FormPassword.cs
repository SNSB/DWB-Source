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
    public partial class FormPassword : Form
    {
        private string _LoginName;
        bool _IsCurrentUser;

        #region Construction
        // for changes of the password by the current user
        public FormPassword()
        {
            this._IsCurrentUser = true;
            this.initForm();
        }

        // for changes of the password by an administrator
        public FormPassword(string LoginName)
        {
            this._LoginName = LoginName;
            this._IsCurrentUser = false;
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

        private void initForm()
        {
            InitializeComponent();
            if (this._LoginName == null)
            {
                if (DiversityWorkbench.Settings.IsTrustedConnection)
                {
                    System.Windows.Forms.MessageBox.Show("Changing of password is only possible for SQL accounts");
                    this.Close();
                    return;
                }
                this._LoginName = DiversityWorkbench.Settings.DatabaseUser;
            }
            this.labelOldPW.Visible = this._IsCurrentUser;
            this.textBoxOldPW.Visible = this._IsCurrentUser;
            if (this._IsCurrentUser)
                this.labelHaeder.Text = "Please enter your new password";
            else
                this.labelHaeder.Text = "Please enter the new password for the login\r\n" + this._LoginName;
            this.userControlDialogPanel.buttonOK.Click += this.ChangePassword;
            this.userControlDialogPanel.buttonOK.Enabled = false;
            try
            {
                this.labelLoginInfo.Text = "";
                string SQL = "SELECT LOGINPROPERTY('" + this._LoginName + "', 'DefaultDatabase')";
                string Info = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                int i;
                SQL = "select LOGINPROPERTY('" + this._LoginName + "','BadPasswordCount')";
                Info = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (int.TryParse(Info, out i) && i > 0)
                {
                    this.labelLoginInfo.Text = i.ToString() + " failed logins. ";
                    SQL = "select convert(varchar(10), LOGINPROPERTY('" + this._LoginName + "','BadPasswordTime'),  120)";
                    Info = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Info.Length > 0)
                        this.labelLoginInfo.Text += "Last trial: " + Info + ". ";
                }
                SQL = "select convert(varchar(10), LOGINPROPERTY('" + this._LoginName + "','PasswordLastSetTime'),  120)";
                Info = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Info.Length > 0)
                    this.labelLoginInfo.Text += "Last change of PW: " + Info + ". ";
            }
            catch (System.Exception ex) { }

        }

        private void ChangePassword(object sender, EventArgs e)
        {
            System.Collections.Generic.List<string> CC = new List<string>();
            CC.Add("'"); CC.Add("{"); CC.Add("}"); CC.Add("["); CC.Add("]"); CC.Add("\\");
            bool ContainsVorbiddenCharacter = false;
            string VorbiddenCharacters = "";
            foreach (string C in CC)
            {
                if (this.textBoxNewPW1.Text.IndexOf(C) > -1)
                {
                    ContainsVorbiddenCharacter = true;
                    VorbiddenCharacters += " " + C;
                }
            }
            if (ContainsVorbiddenCharacter)
            {
                System.Windows.Forms.MessageBox.Show("The characters\r\n" + VorbiddenCharacters + "\r\nare not valid in a password", "Wrong character", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            if (this._LoginName == null)
                this._LoginName = DiversityWorkbench.Settings.DatabaseUser;
            string SQL = "ALTER LOGIN " + this._LoginName + " WITH PASSWORD = '" + this.textBoxNewPW1.Text.Replace("'", "''") + "'";
            if (this._IsCurrentUser)
                SQL += " OLD_PASSWORD = '" + this.textBoxOldPW.Text.Replace("'", "''") + "';";
            try
            {
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString))
                    {
                        //con.Open(); Toni 20230905: Don't open connection twice!
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                        con.Open();
                        C.ExecuteNonQuery();
                        con.Close();
                    }
                }
                //else if (DiversityWorkbench.Forms.FormFunctions.Connection() != null)
                //{
                //    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, DiversityWorkbench.Forms.FormFunctions.Connection());
                //    C.ExecuteNonQuery();
                //}
                else
                {
                    System.Windows.Forms.MessageBox.Show("failed to change password");
                    return;
                }
                if (this._IsCurrentUser
                    && this._LoginName == DiversityWorkbench.Settings.DatabaseUser
                    && !DiversityWorkbench.Settings.IsTrustedConnection)
                {
                    DiversityWorkbench.Settings.Password = this.textBoxNewPW1.Text.Replace("'", "''");
                    DiversityWorkbench.Settings.DatabaseUser = DiversityWorkbench.Settings.DatabaseUser;
                }
                System.Windows.Forms.MessageBox.Show("Password has been changed");
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void textBoxOldPW_TextChanged(object sender, EventArgs e)
        {
            this.CheckPasswords();
        }

        private void textBoxNewPW1_TextChanged(object sender, EventArgs e)
        {
            if (FormFunctions.CheckIfEntryOK(textBoxNewPW1.Text))
                this.CheckPasswords();
        }

        private void textBoxNewPW2_TextChanged(object sender, EventArgs e)
        {
            if (FormFunctions.CheckIfEntryOK(textBoxNewPW2.Text))
                this.CheckPasswords();
        }

        private bool CheckPasswords()
        {
            bool OK = DiversityWorkbench.Forms.FormFunctions.CheckPasswordMatch(this.textBoxNewPW1.Text, this.textBoxNewPW2.Text); // true;
            if (this.textBoxNewPW1.Text.Length == 0
                || this.textBoxNewPW2.Text.Length == 0
                || (this.textBoxOldPW.Text.Length == 0 && this.textBoxOldPW.Visible))
                OK = false;
            //if (this.textBoxNewPW1.Text != this.textBoxNewPW2.Text)
            //    OK = false;
            this.userControlDialogPanel.buttonOK.Enabled = OK;
            return OK;
        }

    }
}
