using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityProjects.GFBio
{
    public partial class FormGFBioImport : Form
    {

        #region Parameter

        private System.Collections.Generic.List<GFBioValue> _GFBioValues;
        
        #endregion

        #region Construction, Form and init

        public FormGFBioImport()
        {
            InitializeComponent();
            this.initForm();
        }

        private void initForm()
        {

            this.tabControl.TabPages.Remove(this.tabPageGFBio);

            System.Data.DataTable dt = new DataTable();
            string SQL = "SELECT NULL AS ProjectID, NULL AS Project UNION SELECT P.ProjectID, P.Project FROM dbo.ProjectList() AS L, Project P WHERE P.ProjectID = L.ProjectID ORDER BY Project";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            this.comboBoxImportIntoProject.DataSource = dt;
            this.comboBoxImportIntoProject.DisplayMember = "Project";
            this.comboBoxImportIntoProject.ValueMember = "ProjectID";
            System.Data.DataTable dtParent = dt.Copy();
            this.comboBoxParentProject.DataSource = dtParent;
            this.comboBoxParentProject.DisplayMember = "Project";
            this.comboBoxParentProject.ValueMember = "ProjectID";

            this.initSettings();

            this.comboBoxAgentsDatabase.Items.Clear();

            foreach(System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> SC in  DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityAgents"].ServerConnectionList())
            {
                this.comboBoxAgentsDatabase.Items.Add(SC.Key);
            }

            if (this.comboBoxAgentsDatabase.Items.Count > 0)
                this.comboBoxAgentsDatabase.SelectedIndex = 0;


            this.helpProvider.HelpNamespace = System.Windows.Forms.Application.StartupPath + "\\DiversityProjects.chm";
        }

        private void initSettings()
        {
            this.userControlModuleRelatedEntryInstitution.textBoxValue.TextChanged += new EventHandler(textBoxSubmitter_TextChanged);
            this.userControlModuleRelatedEntryIPR.textBoxValue.TextChanged += new EventHandler(textBoxIPR_TextChanged);
            this.userControlModuleRelatedEntryLicenseHolder.textBoxValue.TextChanged += new EventHandler(textBoxLicenseHolder_TextChanged);
            this.userControlModuleRelatedEntryPublisher.textBoxValue.TextChanged += new EventHandler(textBoxPublisher_TextChanged);

            DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
            this.userControlModuleRelatedEntryInstitution.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
            this.userControlModuleRelatedEntryIPR.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
            this.userControlModuleRelatedEntryLicenseHolder.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
            this.userControlModuleRelatedEntryPublisher.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;

            if (GFBio.SettingsGFBio.Default.Submitter != null)
                this.userControlModuleRelatedEntryInstitution.textBoxValue.Text = GFBio.SettingsGFBio.Default.Submitter;
            if (GFBio.SettingsGFBio.Default.SubmitterURI != null)
                this.userControlModuleRelatedEntryInstitution.labelURI.Text = GFBio.SettingsGFBio.Default.SubmitterURI;

            if (GFBio.SettingsGFBio.Default.IPR != null)
                this.userControlModuleRelatedEntryIPR.textBoxValue.Text = GFBio.SettingsGFBio.Default.IPR;
            if (GFBio.SettingsGFBio.Default.IPRURI != null)
                this.userControlModuleRelatedEntryIPR.labelURI.Text = GFBio.SettingsGFBio.Default.IPRURI;

            if (GFBio.SettingsGFBio.Default.LicenseHolder != null)
                this.userControlModuleRelatedEntryLicenseHolder.textBoxValue.Text = GFBio.SettingsGFBio.Default.LicenseHolder;
            if (GFBio.SettingsGFBio.Default.LicenseHolderURI != null)
                this.userControlModuleRelatedEntryLicenseHolder.labelURI.Text = GFBio.SettingsGFBio.Default.LicenseHolderURI;

            if (GFBio.SettingsGFBio.Default.Publisher != null)
                this.userControlModuleRelatedEntryPublisher.textBoxValue.Text = GFBio.SettingsGFBio.Default.Publisher;
            if (GFBio.SettingsGFBio.Default.PublisherURI != null)
                this.userControlModuleRelatedEntryPublisher.labelURI.Text = GFBio.SettingsGFBio.Default.PublisherURI;

            if (GFBio.SettingsGFBio.Default.User != null)
                this.textBoxUser.Text = GFBio.SettingsGFBio.Default.User;

            switch(GFBio.SettingsGFBio.Default.ProjectNameGeneration)
            {
                case 0:
                    this.radioButtonEditProjectName.Checked = true;
                    break;
                case 1:
                    this.radioButtonKeepKeyAsName.Checked = true;
                    break;
                case 2:
                    this.radioButtonAutogenerateName.Checked = true;
                    break;
            }

        }
        
        private void textBoxLicenseHolder_TextChanged(object sender, EventArgs e)
        {
            GFBio.SettingsGFBio.Default.LicenseHolder = this.userControlModuleRelatedEntryLicenseHolder.textBoxValue.Text;
        }

        private void textBoxIPR_TextChanged(object sender, EventArgs e)
        {
            GFBio.SettingsGFBio.Default.IPR = this.userControlModuleRelatedEntryIPR.textBoxValue.Text;
        }

        private void textBoxSubmitter_TextChanged(object sender, EventArgs e)
        {
            GFBio.SettingsGFBio.Default.Submitter = this.userControlModuleRelatedEntryInstitution.textBoxValue.Text;
        }

        private void textBoxPublisher_TextChanged(object sender, EventArgs e)
        {
            GFBio.SettingsGFBio.Default.Publisher = this.userControlModuleRelatedEntryPublisher.textBoxValue.Text;

        }

        private void FormGFBioImport_FormClosing(object sender, FormClosingEventArgs e)
        {
            GFBio.SettingsGFBio.Default.SubmitterURI = this.userControlModuleRelatedEntryInstitution.labelURI.Text;
            GFBio.SettingsGFBio.Default.Submitter = this.userControlModuleRelatedEntryInstitution.textBoxValue.Text;

            GFBio.SettingsGFBio.Default.IPR = this.userControlModuleRelatedEntryIPR.textBoxValue.Text;
            GFBio.SettingsGFBio.Default.IPRURI = this.userControlModuleRelatedEntryIPR.labelURI.Text;

            GFBio.SettingsGFBio.Default.LicenseHolder = this.userControlModuleRelatedEntryLicenseHolder.textBoxValue.Text;
            GFBio.SettingsGFBio.Default.LicenseHolderURI = this.userControlModuleRelatedEntryLicenseHolder.labelURI.Text;

            GFBio.SettingsGFBio.Default.Publisher = this.userControlModuleRelatedEntryPublisher.textBoxValue.Text;
            GFBio.SettingsGFBio.Default.PublisherURI = this.userControlModuleRelatedEntryPublisher.labelURI.Text;

            GFBio.SettingsGFBio.Default.Save();
        }

        #endregion

        #region Connect

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (this.LoginParametersGiven() && ParametersGiven())
            {
                this.panelImport.SuspendLayout();
                try
                {
                    string DefaultRole = "";
                    if (this.comboBoxAgentRoleDefault.DataSource != null && this.comboBoxAgentRoleDefault.Text.Length > 0)
                        DefaultRole = this.comboBoxAgentRoleDefault.Text;
                    Import I = new Import(this.textBoxUser.Text, this.textBoxPassword.Text, this.textBoxKey.Text, DefaultRole);
                    string Project = "";
                    if (GFBio.SettingsGFBio.Default.ProjectNameGeneration == 1)
                        Project = this.textBoxKey.Text;
                    this._GFBioValues = I.GFBioValues(Project);
                    this.panelImport.Controls.Clear();
                    foreach (GFBioValue G in this._GFBioValues)
                    {
                        UserControlGFBioValue U = new UserControlGFBioValue(G);
                        U.Dock = DockStyle.Top;
                        if (this.panelImport.Controls.Count == 0)
                            U.IsHeader();
                        this.panelImport.Controls.Add(U);
                        U.BringToFront();
                        if (G.Table == "Project" && G.Column == "Project" && textBoxImportNewProject.Text.Length == 0)
                        {
                            this.textBoxImportNewProject.Text = G.Value;
                            if (GFBio.SettingsGFBio.Default.ProjectNameGeneration > 0)
                                this.textBoxImportNewProject.ReadOnly = true;
                        }
                    }
                }
                catch(System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                this.panelImport.ResumeLayout();
                if (Project.IdentifierTypeAvailable(this._GFBioValues))
                    this.buttonImportStart.Enabled = true;
                else
                    this.buttonImportStart.Enabled = false;
            }
        }

        bool LoginParametersGiven()
        {
            bool OK = true;
            if (this.textBoxKey.Text.Length == 0 || this.textBoxPassword.Text.Length == 0 || this.textBoxUser.Text.Length == 0)
                OK = false;
            return OK;
        }

        bool ParametersGiven()
        {
            bool OK = true;
            if (this.userControlModuleRelatedEntryIPR.textBoxValue.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please enter a information about the IPR");
                return false;
            }

            //if (this.textBoxIPR.Text.Length == 0)
            //{
            //    System.Windows.Forms.MessageBox.Show("Please enter a information about the IPR");
            //    return false;
            //}
            if (this.userControlModuleRelatedEntryLicenseHolder.textBoxValue.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please enter a license holder");
                return false;
            }
            if (this.userControlModuleRelatedEntryPublisher.textBoxValue.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please enter a publisher");
                return false;
            }
            if (this.userControlModuleRelatedEntryInstitution.textBoxValue.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please enter an institution");
                return false;
            }

            //if (this.textBoxSubmitter.Text.Length == 0)
            //{
            //    System.Windows.Forms.MessageBox.Show("Please enter a submitter");
            //    return false;
            //}
            if (this.radioButtonImportIntoProject.Checked)
            {
                if (this.comboBoxImportIntoProject.SelectedIndex == -1)
                {
                    System.Windows.Forms.MessageBox.Show("Please select a project");
                    return false;
                }
            }
            else
            {
                if (this.comboBoxAgentsDatabase.SelectedIndex == -1)
                {
                    System.Windows.Forms.MessageBox.Show("Please select the agents database");
                    return false;
                }
                if (this.comboBoxAgentsProject.SelectedIndex == -1)
                {
                    System.Windows.Forms.MessageBox.Show("Please select the agents project");
                    return false;
                }
            }
            return OK;
        }

        private void textBoxUser_TextChanged(object sender, EventArgs e)
        {
            this.setLoginControlColor(this.textBoxUser);
            GFBio.SettingsGFBio.Default.User = this.textBoxUser.Text;
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            this.setLoginControlColor(this.textBoxPassword);
        }

        private void textBoxKey_TextChanged(object sender, EventArgs e)
        {
            this.setLoginControlColor(this.textBoxKey);
        }


        private void setLoginControlColor(System.Windows.Forms.TextBox TB)
        {
            if (TB.Text.Length > 0)
                TB.BackColor = System.Drawing.SystemColors.Window;
            else
                TB.BackColor = System.Drawing.Color.Pink;
        }
        
        #endregion

        #region Import
       
        private void radioButtonImportNewProject_Click(object sender, EventArgs e)
        {
            this.setImportControls();
        }

        private void radioButtonImportIntoProject_Click(object sender, EventArgs e)
        {
            this.setImportControls();
        }
        
        private void textBoxImportNewProject_TextChanged(object sender, EventArgs e)
        {
            this.setImportControls();
        }

        private void comboBoxImportIntoProject_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.setImportControls();
        }

        private void comboBoxAgentsDatabase_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBoxAgentsDatabase.BackColor = System.Drawing.SystemColors.Window;
            this.comboBoxAgentsProject.BackColor = System.Drawing.Color.Pink;
            this.setAgentProjectList();
        }

        private void setAgentProjectList()
        {
            DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityAgents"].ServerConnectionList()[this.comboBoxAgentsDatabase.SelectedItem.ToString()];
            string SQL = "SELECT ProjectID, Project FROM ProjectProxy ORDER BY Project";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, SC.ConnectionString);
            System.Data.DataTable dt = new DataTable();
            ad.Fill(dt);
            this.comboBoxAgentsProject.DataSource = dt;
            this.comboBoxAgentsProject.DisplayMember = "Project";
            this.comboBoxAgentsProject.ValueMember = "ProjectID";
        }

        private void comboBoxAgentsProject_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBoxAgentsProject.BackColor = System.Drawing.SystemColors.Window;
        }

        private void setImportControls()
        {
            if (this.radioButtonImportIntoProject.Checked)
            {
                if (GFBio.SettingsGFBio.Default.ProjectNameGeneration > 0)
                    this.textBoxImportNewProject.Enabled = false;
                this.textBoxImportNewProject.BackColor = System.Drawing.SystemColors.Window;
                this.comboBoxParentProject.Enabled = false;
                this.comboBoxImportIntoProject.Enabled = true;
                if (this.comboBoxImportIntoProject.SelectedIndex > 0)
                    this.comboBoxImportIntoProject.BackColor = System.Drawing.SystemColors.Window;
                else
                    this.comboBoxImportIntoProject.BackColor = System.Drawing.Color.Pink;
            }
            else
            {
                this.comboBoxImportIntoProject.Enabled = false;
                this.comboBoxImportIntoProject.BackColor = System.Drawing.SystemColors.Window;
                this.comboBoxParentProject.Enabled = true;
                this.textBoxImportNewProject.Enabled = true;
            }
        }

        private int ProjectID()
        {
            int ID = 0;
            if (this.radioButtonImportNewProject.Checked)
            {
                string Message = "";
                string SQL = "INSERT INTO Project (Project";
                if (this.comboBoxParentProject.SelectedIndex > 0)
                    SQL += ", ProjectParentID";
                SQL += ") VALUES ('" + this.textBoxImportNewProject.Text + "'";
                if (this.comboBoxParentProject.SelectedIndex > 0)
                    SQL += ", " + this.comboBoxParentProject.SelectedValue.ToString();
                SQL +=")";
                bool OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message);
                if (!OK && Message.Length > 0)
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("ProjectID()", SQL, Message);
                SQL = "SELECT MAX(ProjectID) FROM Project WHERE Project = '" + this.textBoxImportNewProject.Text + "'";
                if (this.comboBoxParentProject.SelectedIndex > 0)
                    SQL += " AND ProjectParentID = " + this.comboBoxParentProject.SelectedValue.ToString();
                Message = "";
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Message.Length > 0)
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("ProjectID()", SQL, Message);
                if (int.TryParse(Result, out ID))
                {
                    SQL = "INSERT INTO ProjectUser (LoginName, ProjectID) VALUES (USER_NAME(), " + ID.ToString() + ")";
                    DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                }
                else
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("ProjectID()", Result, "Could not parse " + Result + " to integer");
            }
            else
            {
                ID = int.Parse(this.comboBoxImportIntoProject.SelectedValue.ToString());
            }
            return ID;
        }

        private void comboBoxAgentRoleDefault_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxAgentRoleDefault.DataSource == null)
            {
                System.Data.DataTable dt = new DataTable();
                string SQL = "SELECT '' AS CODE UNION SELECT Code FROM ProjectAgentRole_Enum ORDER BY Code";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                this.comboBoxAgentRoleDefault.DataSource = dt;
                this.comboBoxAgentRoleDefault.DisplayMember = "Code";
                this.comboBoxAgentRoleDefault.ValueMember = "Code";
            }
        }

        private void buttonImportStart_Click(object sender, EventArgs e)
        {
            try
            {
                // Check for same project
                if (this.radioButtonImportNewProject.Checked)
                {
                    string SQL = "SELECT COUNT(*) FROM Project P WHERE P.Project = '" + this.textBoxImportNewProject.Text + "'";
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Result != "0")
                    {
                        string Message = "A project with the name\r\n" + this.textBoxImportNewProject.Text + "\r\nalready exists.\r\n" +
                            "Do you really want to import these data?";
                        if (System.Windows.Forms.MessageBox.Show(Message, "Import same project?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                            return;
                    }
                }

                if (GFBio.SettingsGFBio.Default.IPR == null || GFBio.SettingsGFBio.Default.IPR.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Please enter the license IPR");
                    return;
                }
                if (GFBio.SettingsGFBio.Default.LicenseHolder == null || GFBio.SettingsGFBio.Default.LicenseHolder.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Please enter the license holder");
                    return;
                }

                if (GFBio.SettingsGFBio.Default.Submitter == null || GFBio.SettingsGFBio.Default.Submitter.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Please enter the submitter");
                    return;
                }

                if (GFBio.SettingsGFBio.Default.Publisher == null || GFBio.SettingsGFBio.Default.Publisher.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Please enter the publisher");
                    return;
                }


                if (this.comboBoxAgentsProject.SelectedItem == null)
                {
                    System.Windows.Forms.MessageBox.Show("Please enter the project for the agents");
                    return;
                }

                GFBioValue LH = new GFBioValue();
                LH.Table = "ProjectLicense";
                LH.Column = "LicenseHolder";
                LH.Value = GFBio.SettingsGFBio.Default.LicenseHolder;
                this._GFBioValues.Add(LH);

                GFBioValue IPR = new GFBioValue();
                IPR.Table = "ProjectLicense";
                IPR.Column = "IPR";
                IPR.Value = GFBio.SettingsGFBio.Default.IPR;
                this._GFBioValues.Add(IPR);

                bool Replace = false;
                GFBioValue del = new GFBioValue();
                GFBioValue ins = new GFBioValue();
                foreach (GFBioValue V in this._GFBioValues)
                {
                    if (V.Table == "Project" && V.Column == "Project")
                    {
                        if (V.Value != this.textBoxImportNewProject.Text)
                        {
                            del = V;
                            ins = new GFBioValue();
                            ins.Column = V.Column;
                            ins.Identifier = V.Identifier;
                            ins.Setting = V.Setting;
                            ins.Table = V.Table;
                            ins.Value = this.textBoxImportNewProject.Text;
                            Replace = true;
                        }
                    }
                }
                if (Replace)
                {
                    this._GFBioValues.Remove(del);
                    this._GFBioValues.Add(ins);
                }

                //GFBioValue Submitter = new GFBioValue();
                //IPR.Table = "ProjectLicense";
                //IPR.Column = "Submitter";
                //IPR.Value = GFBio.SettingsGFBio.Default.Submitter;
                //this._GFBioValues.Add(Submitter);

                //GFBioValue Publisher = new GFBioValue();
                //IPR.Table = "Agent";
                //IPR.Column = "GivenName";
                //IPR.Value = GFBio.SettingsGFBio.Default.Publisher;
                //this._GFBioValues.Add(Publisher);

                DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityAgents"].ServerConnectionList()[this.comboBoxAgentsDatabase.SelectedItem.ToString()];

                int AgentProjectID;
                if (!int.TryParse(this.comboBoxAgentsProject.SelectedValue.ToString(), out AgentProjectID))
                {
                    System.Windows.Forms.MessageBox.Show("Please select a project for the agents");
                    return;
                }
                Project P = new Project(this.ProjectID(), this._GFBioValues, SC, AgentProjectID);
                string Error = "";
                if (!P.ImportData(ref Error))
                    System.Windows.Forms.MessageBox.Show("Import produced errors\r\n" + Error);
                else
                {
                    System.Windows.Forms.MessageBox.Show("Data imported");
                    this._DataImported = true;
                }
                this.buttonImportStart.Enabled = false;
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Interface

        private bool _DataImported = false;
        public bool DataImported()
        {
            return this._DataImported;
        }

        #endregion

        private enum NameGeneration { Edit, Key, Auto }

        private void radioButtonEditProjectName_Click(object sender, EventArgs e)
        {
            this.setNameGeneration(NameGeneration.Edit);
        }

        private void radioButtonKeepKeyAsName_Click(object sender, EventArgs e)
        {
            this.setNameGeneration(NameGeneration.Key);
        }

        private void radioButtonAutogenerateName_Click(object sender, EventArgs e)
        {
            this.setNameGeneration(NameGeneration.Auto);
        }

        private void setNameGeneration(NameGeneration Generation)
        {
            GFBio.SettingsGFBio.Default.ProjectNameGeneration = (int)Generation;
            switch(Generation)
            {
                case 0:
                    this.textBoxImportNewProject.Enabled = true;
                    this.textBoxImportNewProject.ReadOnly = false;
                    break;
                default:
                    this.textBoxImportNewProject.Text = "";
                    this.textBoxImportNewProject.Enabled = false;
                    this.textBoxImportNewProject.ReadOnly = true;
                    break;
            }
        }
    }
}
