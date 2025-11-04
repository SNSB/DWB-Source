using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.CacheDatabase
{
    public partial class FormPackages : Form, InterfacePackage
    {
        #region Parameter

        private string _Project;
        private int _TargetID;
        
        #endregion

        #region Construction

        public FormPackages(string Project, int TargetID)
        {
            InitializeComponent();
            this._Project = Project;
            this._TargetID = TargetID;
            this.Text = "Packages for " + Project;
            this.setPackages(this._Project);
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

        public void setPackages()
        {
            this.ResetPackages();
            this.setPackages(this._Project);
        }

        public void ShowTransferState(string StateMessage) { }

        #endregion

        #region Packages

        private void ResetPackages()
        {
            this.listBoxPostgresPackagesAvailable.Items.Clear();
            this.panelPostgresProjectPackages.Controls.Clear();
            DiversityCollection.CacheDatabase.Project.GetProject(this._Project).ResetDtPackages();
        }

        private void setPackages(string Project)
        {
            try
            {
                this.panelPostgresProjectPackages.Controls.Clear();
                this.listBoxPostgresPackagesAvailable.Items.Clear();

                System.Collections.Generic.Dictionary<Package.Pack, string> Packages = DiversityCollection.CacheDatabase.Package.Packages();
                System.Collections.Generic.List<string> AvailablePackages = new List<string>();
                foreach (System.Collections.Generic.KeyValuePair<Package.Pack, string> KV in Packages)
                    AvailablePackages.Add(KV.Key.ToString());
                try
                {
                    if (DiversityCollection.CacheDatabase.Project.GetProject(Project) != null)
                    {
                        DiversityCollection.CacheDatabase.Project.GetProject(Project).ResetDtPackages();
                        System.Data.DataTable dtPackages = DiversityCollection.CacheDatabase.Project.GetProject(Project).dtPackage();
                        foreach (System.Data.DataRow R in dtPackages.Rows)
                        {
                            DiversityCollection.CacheDatabase.Packages.UserControlPackage U = new Packages.UserControlPackage(DiversityCollection.CacheDatabase.Project.GetProject(Project), R[0].ToString(), this, this._TargetID);
                            this.panelPostgresProjectPackages.Controls.Add(U);
                            U.Dock = DockStyle.Top;
                            U.BringToFront();
                            if (AvailablePackages.Contains(R[0].ToString()))
                                AvailablePackages.Remove(R[0].ToString());
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                foreach (string s in AvailablePackages)
                    this.listBoxPostgresPackagesAvailable.Items.Add(s);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonPostgresPackageEstablish_Click(object sender, EventArgs e)
        {
            if (this.listBoxPostgresPackagesAvailable.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a package");
                return;
            }
            try
            {
                string Package = this.listBoxPostgresPackagesAvailable.SelectedItem.ToString();
                Package.Pack Pack = CacheDatabase.Package.Pack.ABCD;
                switch (Package)
                {
                    case "FloraRaster":
                        Pack = CacheDatabase.Package.Pack.FloraRaster;
                        break;
                    case "Observation":
                        Pack = CacheDatabase.Package.Pack.Observation;
                        break;
                    case "LIDO":
                        Pack = CacheDatabase.Package.Pack.LIDO;
                        break;
                }
                string Schema = "";
                if (this.SchemaPublicCollision(Pack, ref Schema))
                    System.Windows.Forms.MessageBox.Show("This package would collide with the same package in schema\r\n" + Schema + "\r\nusing the schema public");
                else
                {
                    if (DiversityCollection.CacheDatabase.Package.Packages().ContainsKey(Pack))
                    {
                        DiversityCollection.CacheDatabase.Project P = DiversityCollection.CacheDatabase.Project.GetProject(this._Project);
                        if (P != null)
                        {
                            P.EstablishPackage(Pack, DiversityCollection.CacheDatabase.Package.Packages()[Pack]);
                            this.setPackages(this._Project);
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("The project " + this._Project + " has not been recognized. Please close and reopen the window");
                        }
                        //DiversityCollection.CacheDatabase.Project.GetProject(this._Project).EstablishPackage(Pack, DiversityCollection.CacheDatabase.Package.Packages()[Pack]);
                        //this.setPackages(this._Project);
                    }
                    else
                    {
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
        }

        private bool SchemaPublicCollision(Package.Pack Pack, ref string Schema)
        {
            if (DiversityCollection.CacheDatabase.Package.IsUsingSchemaPublic(Pack))
            {
                string SQL = "select schema_name from information_schema.schemata where schema_name like 'Project_%'";
                System.Data.DataTable dtSchemata = new DataTable();
                string Message = "";
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtSchemata, ref Message);
                if (dtSchemata.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow R in dtSchemata.Rows)
                    {
                        SQL = "SELECT COUNT(*) FROM \"" + R[0].ToString() + "\".\"Package\" WHERE \"Package\" = '" + Pack.ToString() + "';";
                        string Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                        if (Result == "1")
                        {
                            Schema = R[0].ToString();
                            return true;
                        }
                    }
                }
            }
            else
                return false;
            return false;
        }

        #endregion  

        #region Feedback

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        #endregion

        #region Manual

        /// <summary>
        /// Adding event deletates to form and controls
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task InitManual()
        {
            try
            {

                DiversityWorkbench.DwbManual.Hugo manual = new Hugo(this.helpProvider, this);
                if (manual != null)
                {
                    await manual.addKeyDownF1ToForm();
                }
            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// ensure that init is only done once
        /// </summary>
        private bool _InitManualDone = false;


        /// <summary>
        /// KeyDown of the form adding event deletates to form and controls within the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (!_InitManualDone)
                {
                    await this.InitManual();
                    _InitManualDone = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion

    }
}
