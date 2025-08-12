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
    public partial class UserControlAddOn : UserControl
    {

        #region Parameter

        private PackageAddOn _AddOn;
        DiversityCollection.CacheDatabase.Project _Project;
        //private UserControlPackage _UserControlPackage;
        
        #endregion

        #region Construction

        public UserControlAddOn(PackageAddOn AddOn, DiversityCollection.CacheDatabase.Project Project/*, UserControlPackage UCpackage*/)
        {
            InitializeComponent();
            this._AddOn = AddOn;
            this._Project = Project;
            //this._UserControlPackage = UCpackage;
            this.initControl();
        }
        
        #endregion

        #region Control

        private void initControl()
        {
            this.labelAddOn.Text = this._AddOn.AddOnOfPackage().ToString();
            //if (PackageAddOn.AddOnTypes()[this._AddOn.AddOnOfPackage()] == PackageAddOn.AddOnType.exclusive)
            //{
            //    this._UserControlPackage.buttonAddOn.Enabled = false;
            //    this._UserControlPackage.buttonUpdate.Enabled = false;
            //}
            this.setUpdate();
        }
        
        #endregion

        #region Events

        private void buttonInfo_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show(PackageAddOn.PackageAddOns()[this._AddOn.AddOnPackage().PackagePack][this._AddOn.AddOnOfPackage()]);
        }
        
        #endregion

        #region Update
        
        private void setUpdate()
        {
            string SQL = "SELECT \"Version\" FROM \"" + this._AddOn.AddOnPackage().Project().SchemaName + "\".\"PackageAddOn\" " +
                " WHERE \"Package\" = '" + this._AddOn.AddOnPackage().Name + "' AND \"AddOn\" = '" + this._AddOn.AddOnOfPackage().ToString() + "';";
            int Version;
            if (int.TryParse(DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL), out Version))
            {
                if (Version < DiversityCollection.CacheDatabase.PackageAddOn.Version(this._AddOn.AddOnOfPackage()))
                {
                    this.buttonUpdate.Visible = true;
                    this.buttonUpdate.Text = "Upd. to vers. " + DiversityCollection.CacheDatabase.PackageAddOn.Version(this._AddOn.AddOnOfPackage()).ToString();
                }
                else
                {
                    this.buttonUpdate.Visible = true;
                    this.buttonUpdate.Text = "Vers. " + DiversityCollection.CacheDatabase.PackageAddOn.Version(this._AddOn.AddOnOfPackage()).ToString();
                    this.buttonUpdate.TextAlign = ContentAlignment.MiddleCenter;
                    this.buttonUpdate.Enabled = false;
                    this.buttonUpdate.Image = null;
                    this.buttonUpdate.FlatStyle = FlatStyle.Flat;
                    this.buttonUpdate.FlatAppearance.BorderSize = 0;
                }
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
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
                            if (de.Key.ToString().StartsWith(this._AddOn.AddOnOfPackage().ToString()))//"DiversityCollectionCacheProjectPG_"))
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
                            this._AddOn.AddOnOfPackage().ToString(),
                            DiversityCollection.CacheDatabase.PackageAddOn.Version(this._AddOn.AddOnOfPackage()),//Package.Version(this._Package.PackagePack),
                            con,
                            Versions,
                            DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace(),
                            this._Project.SchemaName,
                            ReplaceStrings);
                    f.ForPostgres = true;
                    f.ShowInTaskbar = true;
                    f.ShowDialog();
                    this.setUpdate();
                    //if (Package.TransferSteps(this._Package.PackagePack, this._Project.SchemaName).Count > 0)//.FunctionsInProjectSchema(this._Package.PackagePack).Count > 0)
                    //{
                    //    this._Package.setLastTransferDate();
                    //    this.setLastTransferDate();
                    //}
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

    }
}
