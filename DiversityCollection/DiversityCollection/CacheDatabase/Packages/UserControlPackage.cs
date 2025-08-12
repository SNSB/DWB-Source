using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.CacheDatabase.Packages
{
    public partial class UserControlPackage : UserControl, InterfacePackage
    {
        #region Parameter

        DiversityCollection.CacheDatabase.Project _Project;
        private DiversityCollection.CacheDatabase.Package _Package;
        private DiversityCollection.CacheDatabase.InterfacePackage _IPackage;
        private int _TargetID;

        #endregion

        #region Construction

        public UserControlPackage(
            DiversityCollection.CacheDatabase.Project Project, 
            string Package, 
            DiversityCollection.CacheDatabase.InterfacePackage IPackage,
            int TargetID)
        {
            InitializeComponent();
            this._Package = new Package(Package, Project);
            this.labelName.Text = Package;
            this._Project = Project;
            this._IPackage = IPackage;
            this._TargetID = TargetID;
            this.SetUpdate();
            if (CacheDatabase.PackageAddOn.PackageAddOns().ContainsKey(this._Package.PackagePack))
            {
                this.buttonAddOn.Visible = true;
                this.panelAddOns.Visible = true;
                this.Height = (int)((float)55 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                if (!this._Package.NeedsUpdate())
                {
                    // it the base package needs an update the initAddOns update has to be done after the user clicks on update
                    this.initAddOns();
                }
            }
            else
            {
                this.buttonAddOn.Visible = false;
                this.panelAddOns.Visible = false;
                this.Height = (int)((float)30 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
            }
            if (CacheDatabase.Package.TransferSteps(this._Package.PackagePack, this._Project.SchemaName).Count > 0)//.FunctionsInProjectSchema(this._Package.PackagePack).Count > 0)
            {
                this.buttonTransferToMaterializedTables.Visible = true;
                this.buttonHistory.Visible = true;
                this.labelTransferState.Visible = true;
                this.setLastTransferDate();
                this.Height += (int)((float)27 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
            }
            else
            {
                this.buttonTransferToMaterializedTables.Visible = false;
                this.buttonHistory.Visible = false;
                this.labelTransferState.Visible = false;
            }
            if (DiversityWorkbench.Settings.TimeoutDatabase == 0)
                this.buttonTimeout.Text = "inf.";
            else
                this.buttonTimeout.Text = DiversityWorkbench.Settings.TimeoutDatabase.ToString() + "s.";
            this.buttonTransferToMaterializedTables.Width = (int)((float)this.buttonTransferToMaterializedTables.Width * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
        }
        
        #endregion

        #region Interface

        public bool PackageIsUsingSchemaPublic() { return this._Package.IsUsingSchemaPublic(); }
        public DiversityCollection.CacheDatabase.Package.Pack Pack() { return this._Package.PackagePack; }
        public void ShowTransferState(string StateMessage)
        {
            this.labelTransferState.Text = StateMessage;
            System.Windows.Forms.Application.DoEvents();
        }
        public void setPackages()
        {
        }

        #endregion

        #region Update

        private void SetUpdate()
        {
            if (this._Package.NeedsUpdate())
            {
                this.buttonUpdate.Visible = true;
                this.buttonUpdate.Text = "Upd. to vers. " + DiversityCollection.CacheDatabase.Package.Version(this._Package.PackagePack).ToString();
                this.buttonAddOn.Enabled = false;
                this.buttonExport.Enabled = false;
                this.buttonTransferToMaterializedTables.Enabled = false;
                this.buttonHistory.Enabled = false;
                this.buttonViewContent.Enabled = false;
            }
            else
            {
                this.buttonUpdate.Visible = true;
                this.buttonUpdate.Text = "Vers. " + DiversityCollection.CacheDatabase.Package.Version(this._Package.PackagePack).ToString();
                this.buttonUpdate.TextAlign = ContentAlignment.MiddleCenter;
                this.buttonUpdate.Enabled = false;
                this.buttonUpdate.Image = null;
                this.buttonUpdate.FlatStyle = FlatStyle.Flat;
                this.buttonUpdate.FlatAppearance.BorderSize = 0;
                this.buttonAddOn.Enabled = true;
                this.buttonExport.Enabled = true;
                this.buttonTransferToMaterializedTables.Enabled = true;
                this.buttonHistory.Enabled = true;
                this.buttonViewContent.Enabled = true;
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            //return;
            try
            {
                // check resouces for update scripts
                System.Collections.Generic.Dictionary<string, string> Versions = new Dictionary<string, string>();
                System.Resources.ResourceManager rm = CacheDatabase.Packages.ResourcePackages.ResourceManager;// Properties.Resources.ResourceManager;
                System.Resources.ResourceSet rs = rm.GetResourceSet(new System.Globalization.CultureInfo("en-US"), true, true);
                if (rs != null)
                {
                    System.Collections.IDictionaryEnumerator de = rs.GetEnumerator();
                    while (de.MoveNext() == true)
                    {
                        if (de.Entry.Value is string)
                        {
                            //string Test = de.Key.ToString().Substring(this._Package.Name.Length, 2);
                            if (de.Key.ToString().StartsWith(this._Package.Name) && de.Key.ToString().Substring(this._Package.Name.Length, 2) == "_0")//"DiversityCollectionCacheProjectPG_"))
                            {
                                Versions.Add(de.Key.ToString(), de.Value.ToString());
                            }
                        }
                    }
                }

                if (Versions.Count > 0)
                {
                    System.Collections.Generic.Dictionary<string, string> ReplaceStrings = new Dictionary<string, string>();
                    ReplaceStrings.Add("#project#", this._Project.SchemaName);
                    Npgsql.NpgsqlConnection con = new Npgsql.NpgsqlConnection(DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());
                    DiversityWorkbench.Forms.FormUpdateDatabase f =
                        new DiversityWorkbench.Forms.FormUpdateDatabase(
                            DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name, // .Postgres.PostgresConnection().Database,
                            this._Package.Name,
                            DiversityCollection.CacheDatabase.Package.Version(this._Package.PackagePack),
                            con,
                            Versions,
                            DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace(),
                            this._Project.SchemaName,
                            ReplaceStrings);
                    f.ForPostgres = true;
                    f.ShowInTaskbar = true;
                    f.ShowDialog();
                    this.SetUpdate();
                    if (CacheDatabase.PackageAddOn.PackageAddOns().ContainsKey(this._Package.PackagePack) && !this._Package.NeedsUpdate())
                    {
                        this.initAddOns();
                    }
                    if (Package.TransferSteps(this._Package.PackagePack, this._Project.SchemaName).Count > 0)//.FunctionsInProjectSchema(this._Package.PackagePack).Count > 0)
                    {
                        this._Package.setLastTransferDate();
                        this.setLastTransferDate();
                        // this.Height += (int)((float)27 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Upgrade resources are missing");
                }
            }
            catch (System.Exception ex)
            { }
        }
        
        #endregion

        #region Events

        private void buttonInfo_Click(object sender, EventArgs e)
        {
            string Description = this._Package.GetDescription();
            DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText(this._Package.Name, Description, true);
            f.ShowDialog();
            //System.Windows.Forms.MessageBox.Show(Description, this._Package.Name);
        }

        private void buttonDeletePackage_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            if (this._Package.RemovePackage())
                this._IPackage.setPackages();
            else
            {
                System.Windows.Forms.MessageBox.Show("Removal failed");
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void buttonViewContent_Click(object sender, EventArgs e)
        {
            DiversityCollection.CacheDatabase.FormViewContent f = new FormViewContent(true, this._Project.SchemaName, this._Package.Name, this._Package.IsUsingSchemaPublic());
            f.ShowDialog();
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            DiversityCollection.CacheDatabase.Packages.FormPackageExport f = new FormPackageExport(this._Package, this._Project.SchemaName);
            f.ShowDialog();
        }

        private void buttonTimeout_Click(object sender, EventArgs e)
        {
            int Timeout = DiversityWorkbench.Settings.TimeoutDatabase;
            DiversityWorkbench.Forms.FormGetInteger f = new DiversityWorkbench.Forms.FormGetInteger(Timeout, "Timeout", "Please enter the timeout for database queries in seconds");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.Integer != null)
            {
                DiversityWorkbench.Settings.TimeoutDatabase = (int)f.Integer;
                if (DiversityWorkbench.Settings.TimeoutDatabase == 0)
                    this.buttonTimeout.Text = "inf.";
                else
                    this.buttonTimeout.Text = DiversityWorkbench.Settings.TimeoutDatabase.ToString() + "s.";
            }
        }

        #endregion

        #region AddOns

        private void initAddOns()
        {
            System.Data.DataTable dtAddOn = new DataTable();
            string Message = "";
            string SQL = "SELECT \"AddOn\", \"Version\" " +
                " FROM \"" + this._Project.SchemaName + "\".\"PackageAddOn\" " +
                " WHERE \"Package\" = '" + this._Package.PackagePack.ToString() + "';";
            DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtAddOn, ref Message);
            if (dtAddOn.Rows.Count > 0)
            {
                foreach (System.Data.DataRow R in dtAddOn.Rows)
                {
                    DiversityCollection.CacheDatabase.PackageAddOn PA = new PackageAddOn(this._Package, R[0].ToString());
                    if (PA.TypeOfAddOn() == PackageAddOn.AddOnType.exclusive)
                    {
                        this.buttonUpdate.Enabled = false;
                        this.buttonAddOn.Enabled = false;
                    }
                    DiversityCollection.CacheDatabase.Packages.UserControlAddOn UC = new UserControlAddOn(PA, this._Project/*, this*/);
                    this.panelAddOns.Controls.Add(UC);
                    UC.Dock = DockStyle.Top;
                }
                if (this.panelAddOns.Controls.Count > 1)
                {
                    this.Height = 30 + (this.panelAddOns.Controls.Count - 1) * 24;
                }
            }
        }

        private void buttonAddOn_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.Dictionary<string, string> D = new Dictionary<string, string>();
            int PackageVersion = DiversityCollection.CacheDatabase.Package.Version(this._Package.PackagePack);
            foreach (System.Collections.Generic.KeyValuePair<PackageAddOn.AddOn, string> KV in PackageAddOn.PackageAddOns()[this._Package.PackagePack])
            {
                int CompatibleVersion = DiversityCollection.CacheDatabase.PackageAddOn.PackageCompatibleVersions()[KV.Key];
                if (PackageVersion == CompatibleVersion)
                    D.Add(KV.Key.ToString(), KV.Key + ": " + KV.Value);
            }
            if (D.Count > 0)
            {
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(D, "Add-On", "Please select the add-on that should be installed", true);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    foreach (System.Collections.Generic.KeyValuePair<PackageAddOn.AddOn, string> KV in PackageAddOn.PackageAddOns()[this._Package.PackagePack])
                    {
                        if (KV.Key.ToString() == f.SelectedValue)
                        {
                            string SQL = "INSERT INTO \"" + this._Project.SchemaName + "\".\"PackageAddOn\" (\"Package\", \"AddOn\", \"Version\")" +
                                " VALUES ('" + this._Package.PackagePack.ToString() + "', '" + KV.Key.ToString() + "', 1);";
                            DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);
                            DiversityCollection.CacheDatabase.PackageAddOn PA = new PackageAddOn(this._Package, KV.Key);
                            if (PA.TypeOfAddOn() == PackageAddOn.AddOnType.exclusive)
                            {
                                this.buttonUpdate.Enabled = false;
                                this.buttonAddOn.Enabled = false;
                            }
                            DiversityCollection.CacheDatabase.Packages.UserControlAddOn UC = new UserControlAddOn(PA, this._Project/*, this*/);
                            this.panelAddOns.Controls.Add(UC);
                            UC.Dock = DockStyle.Top;
                        }
                    }
                    if (this.panelAddOns.Controls.Count > 1)
                    {
                        this.Height = 30 + (this.panelAddOns.Controls.Count - 1) * 24;
                    }
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("No add-ons available");
        }
        
        #endregion

        #region Transfer and history

        private void buttonTransferToMaterializedTables_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.Dictionary<string, object> TransferHistory = new Dictionary<string, object>();
            bool DataHaveBeenTransferred = false;
            if (Package.TransferSteps(this._Package.PackagePack, this._Project.SchemaName).Count > 0)
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                CacheDatabase.FormCacheDatabaseTransfer f = new FormCacheDatabaseTransfer(Package.TransferSteps(this._Package.PackagePack, this._Project.SchemaName));//, false);
                if (this._Project.ProjectID != null)
                    f.setProjectID((int)this._Project.ProjectID);
                this.Cursor = System.Windows.Forms.Cursors.Default;
                f.ShowDialog();
                DataHaveBeenTransferred = f.DataHaveBeenTransferred;
                TransferHistory = f.TransferHistory;
                this._Package.setLastTransferDate();
                this.setLastTransferDate();
            }
            else
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                string Error = "";
                if (this._Package.TransferData(ref Error, this, ref TransferHistory))
                {
                    System.Windows.Forms.MessageBox.Show("Data transferred");
                    DataHaveBeenTransferred = true;
                }
                else System.Windows.Forms.MessageBox.Show("Transfer failed: " + Error);
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            if (DataHaveBeenTransferred && TransferHistory.Count > 0)
            {
                DiversityCollection.CacheDatabase.CacheDB.WriteProjectTransferHistory(CacheDB.HistoryTarget.Package, (int)this._Project.ProjectID, this._Project.SchemaName, TransferHistory, this._TargetID, this._Package.Name);
            }
        }

        private void WriteHistory(int TargetID, string Settings, string Package)
        {
            try
            {
                int UserID = int.Parse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("SELECT dbo.UserID()"));
                string SQL = "INSERT INTO ProjectTransfer (ProjectID, ResponsibleUserID, TargetID, Settings, Package) " +
                    "VALUES (" + this._Project.ProjectID.ToString() + ", " + UserID.ToString() + ", " + TargetID.ToString()+ ", '" + Settings + "', '" + Package + "')";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonHistory_Click(object sender, EventArgs e)
        {
            DiversityCollection.CacheDatabase.FormTransferHistory f = new FormTransferHistory((int)this._Project.ProjectID, this._TargetID, this._Package.Name);
            f.ShowDialog();
        }

        private void setLastTransferDate()
        {
            this.labelTransferState.Text = this._Package.LastTransferDate();
        }

        #endregion

    }
}
