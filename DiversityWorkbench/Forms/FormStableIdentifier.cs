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
    public partial class FormStableIdentifier : Form
    {

        #region Parameter

        //private string _DisplayMember = "Project";
        //private string _ValueMember = "ProjectID";
        //private string _IdentifierBaseColumn = "";
        //private string _IdentifierTypeColumn = "";
        //private System.Windows.Forms.BindingSource _BindingSource;

        Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapter;
        string _ProjectID = "";

        #endregion

        #region Construction

        public FormStableIdentifier()
        {
            InitializeComponent();
            this.initForm();
        }

        public FormStableIdentifier(int ProjectID)
        {
            InitializeComponent();
            this._ProjectID = ProjectID.ToString();
            this.initForm();
        }

        //public FormStableIdentifier(System.Data.DataTable dtIdentifier, string DisplayColumn, string ValueColumn, string IdentifierBaseColumn, string IdentifierTypeColumn)
        //{
        //    InitializeComponent();
        //    //this._DisplayMember = DisplayColumn;
        //    //this._ValueMember = ValueColumn;
        //    //this._IdentifierBaseColumn = IdentifierBaseColumn;
        //    //this._IdentifierTypeColumn = IdentifierTypeColumn;
        //    this.initForm(dtIdentifier);
        //}

        //public FormStableIdentifier(System.Data.DataTable dtIdentifier)
        //{
        //    InitializeComponent();
        //    this.initForm(dtIdentifier);
        //}

        #endregion

        #region Form

        private void initForm()
        {
            try
            {
                string Message = "";
                bool ColumnStableIdentifierBaseExists = false;
                string SQL = "select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ProjectProxy' AND C.COLUMN_NAME = 'StableIdentifierBase'";
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
                int i = 0;
                if (int.TryParse(Result, out i) && i > 0 && Message.Length == 0)
                    ColumnStableIdentifierBaseExists = true;
                if (ColumnStableIdentifierBaseExists)
                {
                    System.Data.DataTable dtType = new DataTable();
                    SQL = "SELECT 'Default' AS DisplayText, 1 AS Value";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dtType);
                    this.comboBoxType.DataSource = dtType;
                    this.comboBoxType.DisplayMember = "DisplayText";
                    this.comboBoxType.ValueMember = "Value";
                }
                else
                {
                    splitContainerMain.Panel2Collapsed = true;
                    this.labelProjects.Text = "List of projects";
                    this.labelHeader.Text = "The basis of the stable identifier can be set for the whole database";
                }


                string SqlProjects = "SELECT Project, ProjectID ";
                if (ColumnStableIdentifierBaseExists)
                    SqlProjects += ", StableIdentifierBase, StableIdentifierTypeID";
                SqlProjects += " FROM ProjectProxy";
                if (this._ProjectID.Length > 0)
                    SqlProjects += " WHERE ProjectID = " + this._ProjectID;
                else
                    SqlProjects += " ORDER BY Project";
                this._SqlDataAdapter = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlProjects, DiversityWorkbench.Settings.ConnectionString);
                this._SqlDataAdapter.Fill(this.dataSetStableIdentifier.ProjectProxy);
                Microsoft.Data.SqlClient.SqlCommandBuilder cb = new Microsoft.Data.SqlClient.SqlCommandBuilder(this._SqlDataAdapter);

                SQL = "SELECT [dbo].[StableIdentifierBase] ()";
                string StableIdentifier = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
                if (Message.Length == 0 && StableIdentifier.Length > 0)
                {
                    this.buttonSetForDatabase.Enabled = false;
                    this.buttonSetForDatabase.Text = "Stable identifier for database: " + StableIdentifier;
                    if (!ColumnStableIdentifierBaseExists)
                        this.labelHeader.Text = "The basis of the stable identifier for the whole database is " + StableIdentifier;
                }
                this.labelBaseURL.Text = this.ExplainStableIdentifier();

                this.initIdService();
            }
            catch (System.Exception ex)
            {
            }
        }

        private void initForm(System.Data.DataTable dtIdentifier)
        {
            try
            {
                //this._BindingSource = new BindingSource(dtIdentifier, dtIdentifier.TableName);
                //this.textBoxBase.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BindingSource, _IdentifierBaseColumn, true));
                //this.listBoxIdentifier.DataSource = dtIdentifier;
                //this.listBoxIdentifier.DisplayMember = _DisplayMember;
                //this.listBoxIdentifier.ValueMember = _ValueMember;

                System.Data.DataTable dtType = new DataTable();
                string SQL = "SELECT 'Default' AS DisplayText, 1 AS Value";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dtType);
                this.comboBoxType.DataSource = dtType;
                this.comboBoxType.DisplayMember = "DisplayText";
                this.comboBoxType.ValueMember = "Value";
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            try
            {
                string ImagePath = DiversityWorkbench.QRCode.QRCodeImage(this.textBoxBase.Text, 500, Application.StartupPath);
                if (ImagePath.Length > 0)
                {
                    System.IO.FileStream stream = new System.IO.FileStream(ImagePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    this.pictureBoxQRcode.Image = Image.FromStream(stream);
                    stream.Close();
                }
                else System.Windows.Forms.MessageBox.Show("Generation failed. See error log for details");
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void listBoxIdentifier_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void FormStableIdentifier_Load(object sender, EventArgs e)
        {
            //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            //this.projectProxyTableAdapter.Adapter.SelectCommand = new Microsoft.Data.SqlClient.SqlCommand("SELECT Project, ProjectID, StableIdentifierBase, StableIdentifierTypeID FROM ProjectProxy ORDER BY Project", con);
            //this.projectProxyTableAdapter.Adapter.SelectCommand.Connection.ConnectionString = DiversityWorkbench.Settings.ConnectionString;
            //this.projectProxyTableAdapter.Fill(this.dataSetStableIdentifier.ProjectProxy);
        }

        private void FormStableIdentifier_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.projectProxyBindingSource.Current;
                R.BeginEdit();
                R.EndEdit();
                this._SqlDataAdapter.Update(this.dataSetStableIdentifier.ProjectProxy);
            }
        }

        private void buttonCopyBasicUrlToAll_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView RV = (System.Data.DataRowView)this.projectProxyBindingSource.Current;
                if (RV["StableIdentifierBase"].Equals(System.DBNull.Value) || RV["StableIdentifierBase"].ToString().Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Please enter a basic URL");
                    return;
                }
                if (RV["StableIdentifierTypeID"].Equals(System.DBNull.Value) || RV["StableIdentifierTypeID"].ToString().Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Please select a type");
                    return;
                }
                string URL = RV["StableIdentifierBase"].ToString();
                int TypeID = int.Parse(RV["StableIdentifierTypeID"].ToString());
                foreach (System.Data.DataRow R in this.dataSetStableIdentifier.ProjectProxy.Rows)
                {
                    R["StableIdentifierBase"] = URL;
                    R["StableIdentifierTypeID"] = TypeID;
                }
            }
            catch (System.Exception ex)
            { }
        }

        public void setHelpProvider(string HelpNamespace, string Keyword)
        {
            this.helpProvider.HelpNamespace = HelpNamespace;
            this.helpProvider.SetHelpNavigator(this, HelpNavigator.KeywordIndex);
            this.helpProvider.SetHelpKeyword(this, Keyword);
        }

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void buttonRemoveAll_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Do you really want to remove all basic URLs", "Remove all?", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == System.Windows.Forms.DialogResult.Yes)
            {
                foreach (System.Data.DataRow R in this.dataSetStableIdentifier.ProjectProxy.Rows)
                {
                    R.BeginEdit();
                    R["StableIdentifierBase"] = System.DBNull.Value;
                    R["StableIdentifierTypeID"] = System.DBNull.Value;
                    R.EndEdit();
                }
            }
        }

        private void buttonSetForDatabase_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT [dbo].[StableIdentifierBase] ()";
            string StableIdentifier = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
            if (StableIdentifier.Length > 0)
            {
                System.Windows.Forms.MessageBox.Show("Stable identifier base already set to:\r\n" + StableIdentifier);
            }
            else
            {
                SQL = "SELECT [dbo].[BaseURL] ()";
                StableIdentifier = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Stable identifier", "Please enter the basis for the stable identifier valid for all data in the database:\r\n" + this.ExplainStableIdentifier(), StableIdentifier);
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    StableIdentifier = f.String;
                    if (!StableIdentifier.EndsWith("/"))
                        StableIdentifier += "/";
                    string Message = "";
                    SQL = "CREATE FUNCTION [dbo].[StableIdentifierBase] () RETURNS  varchar (255) AS BEGIN declare @URL varchar(255) set @URL = '" + StableIdentifier + "' return @URL END";
                    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message))
                    {
                        switch (DiversityWorkbench.Settings.ModuleName)
                        {
                            case "DiversityProjects":
                                SQL = "GRANT EXECUTE ON dbo.StableIdentifierBase TO [DiversityWorkbenchUser]";
                                break;
                            default:
                                SQL = "GRANT EXECUTE ON dbo.StableIdentifierBase TO [db_datareader]";
                                break;
                        }
                        if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message))
                            System.Windows.Forms.MessageBox.Show("Stable identifier base created");
                    }
                    else
                        System.Windows.Forms.MessageBox.Show("Stable identifier creation failed:\r\n" + Message);
                }
            }

        }

        private string _ExplainStableIdentifier = "";
        private string ExplainStableIdentifier()
        {
            if (this._ExplainStableIdentifier.Length == 0)
            {
                string SQL = "Select dbo.BaseURL()";
                string BaseURL = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                string idUri = global::DiversityWorkbench.Properties.Settings.Default.DiversityWokbenchIDUrl + "SNSB/Collection";
                if (BaseURL.Length == 0) BaseURL = idUri;
                string Module = DiversityWorkbench.Settings.ModuleName.Replace("Diversity", "");
                string MainTable = Module;
                if (MainTable.EndsWith("s"))
                    MainTable = MainTable.Remove(MainTable.Length - 1);
                switch (Module)
                {
                    case "Collection":
                        MainTable = "CollectionSpecimen";
                        break;
                    case "Gazetteer":
                        MainTable = "GeoName";
                        break;
                    case "References":
                        MainTable = "ReferenceTitle";
                        break;
                    case "ScientificTerms":
                        MainTable = "TermRepresentation";
                        break;
                }
                this._ExplainStableIdentifier = "Basic URL for the stable identifier e.g.\r\n" +
                    BaseURL +
                    "\r\nto which the ID of the main table " + MainTable + " will be attached resulting in a stable identifier like\r\n" +
                    BaseURL + "2345";
            }
            return this._ExplainStableIdentifier;
        }

        #endregion

        #region idservice

        private void initIdService()
        {
            string Message = "";
            if (this.IdServiceRoleExists() &&
                this.IdServiceLoginExists() &&
                this.IdServiceUserExists())
            {
                this.tableLayoutPanelIdservice.Visible = false;
            }
            else
            {
                if (!this.IdServiceLoginExists())
                {
                    Message = "the login idservice";
                }
                if (!this.IdServiceRoleExists())
                {
                    if (Message.Length > 0)
                        Message += ", ";
                    Message += "the role StableIDServices";
                }
                if (!this.IdServiceUserExists())
                {
                    if (Message.Length > 0)
                        Message += " and ";
                    Message += "the user idservice";
                }
                Message += " for the REST webservice";
                this.labelIdService.Text = Message + " is missing";
                this.buttonIdService.Text = "      Create " + Message;
            }
        }

        private void buttonIdService_Click(object sender, EventArgs e)
        {
            bool OK = true;
            if (!IdServiceLoginExists())
                OK = IdServiceCreateLogin();
            if (OK)
            {
                if (!this.IdServiceRoleExists())
                    OK = this.IdServiceCreateRole();
                if (OK)
                {
                    if (!this.IdServiceUserExists())
                        OK = this.IdServiceCreateUser();
                }
            }
            if (OK)
                System.Windows.Forms.MessageBox.Show("Objects created");
            this.initIdService();
        }

        #region Create

        private bool IdServiceCreateLogin()
        {
            bool OK = false;
            string PW = "";
            DiversityWorkbench.Forms.FormGetString f = new FormGetString("Password", "Please enter the password for the new login idservice", PW);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                PW = f.String;
                string SQL = "CREATE LOGIN idservice WITH PASSWORD = '" + PW + "', DEFAULT_DATABASE=master, CHECK_EXPIRATION=OFF, CHECK_POLICY=ON;";
                OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            }
            return OK;
        }

        private bool IdServiceCreateRole()
        {
            bool OK = false;
            string SQL = "CREATE ROLE StableIDServices";
            OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            if (OK)
            {
                switch (DiversityWorkbench.Settings.ModuleName)
                {
                    case "DiversityCollection":
                        SQL = "GRANT SELECT ON [dbo].[CollectionSpecimen] TO StableIDServices; " +
                            "GRANT SELECT ON [dbo].[CollectionProject] TO StableIDServices; " +
                            "GRANT SELECT ON [dbo].[IdentificationUnit] TO StableIDServices; " +
                            "GRANT SELECT ON [dbo].[CollectionSpecimenPart] TO StableIDServices; " +
                            "GRANT SELECT ON [dbo].[IdentificationUnitInPart] TO StableIDServices; " +
                            "GRANT SELECT ON [dbo].[ProjectProxy] TO StableIDServices;";
                        OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                        break;
                    case "DiversityProjects":
                        SQL = "GRANT SELECT ON [dbo].[Project] TO StableIDServices; ";
                        OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                        break;
                }
                SQL = "EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Role with grants needed for the id service publishing information in context with the stable identifiers according to the CETAF recommendations.' , @level0type=N'USER',@level0name=N'StableIDServices'";
                OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            }
            return OK;
        }

        private bool IdServiceCreateUser()
        {
            bool OK = false;
            string SQL = "CREATE USER idservice FOR LOGIN idservice WITH DEFAULT_SCHEMA=[dbo]";
            OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            if (OK)
            {
                SQL = "EXEC sp_addrolemember N'StableIDServices', N'idservice'";
                OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            }
            return OK;
        }

        #endregion

        #region Exists

        private bool IdServiceLoginExists()
        {
            string SQL = "select count(*) " +
                "from sys.server_principals p  " +
                "where p.name not like '##%##' and p.type in ('S', 'U') and p.name not like 'NT-%' and p.name <> 'sa'   " +
                "and p.name = 'idservice'";
            bool Exists = this.IdServiceObjectsExists(SQL);
            return Exists;
        }

        private bool IdServiceRoleExists()
        {
            string SQL = "Select count(*) From sysusers u " +
                "Where issqlrole = 1 and u.name = 'StableIDServices'";
            bool Exists = this.IdServiceObjectsExists(SQL);
            return Exists;
        }

        private bool IdServiceUserExists()
        {
            string SQL = "SELECT count(*) FROM sys.database_principals WHERE name = N'idservice'";
            bool Exists = this.IdServiceObjectsExists(SQL);
            return Exists;
        }

        private bool IdServiceObjectsExists(string SQL)
        {
            bool Exists = false;
            string Message = "";
            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
            int iRes;
            if (int.TryParse(Result, out iRes) && iRes > 0 && Message.Length == 0)
                Exists = true;
            return Exists;
        }

        #endregion

        #endregion
    }
}
