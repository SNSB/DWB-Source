using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.UserControls
{
    public partial class UserControl_ProjectsNotes : UserControl__Data
    {

        #region Parameter

        private System.Windows.Forms.BindingSource _SourceProjectList;
        
        #endregion

        #region Construction

        public UserControl_ProjectsNotes(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            System.Windows.Forms.BindingSource SourceProjectList,
            string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this._SourceProjectList = SourceProjectList;
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }

        #endregion

        #region Public

        public override void setAvailability()
        {
            base.setAvailability();
            if (!this._iMainForm.ReadOnly())
            {
                this.FormFunctions.setDataControlEnabled(this.groupBoxNotes, this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE]);
                if (this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE])
                {
                    this.FormFunctions.setDataControlEnabled(this.groupBoxProjects, true);
                    bool OK = this.FormFunctions.getObjectPermissions("CollectionProject", "INSERT");
                    this.toolStripButtonProjectNew.Enabled = OK;
                    this.toolStripButtonProjectNoAccessNew.Enabled = OK;
                    OK = this.FormFunctions.getObjectPermissions("CollectionProject", "DELETE");
                    this.toolStripButtonProjectDelete.Enabled = OK;
                    this.toolStripButtonProjectOpen.Enabled = true;
                }
                else
                {
                    this.FormFunctions.setDataControlEnabled(this.groupBoxProjects, false);
                    this.toolStripButtonProjectOpen.Enabled = false;
                }
            }
        }


        public override void SetPosition(int Position)
        {
            base.SetPosition(Position);
            this.fillProjectList();
        }

        public System.Windows.Forms.TableLayoutPanel TableLayoutPanelNotes { get { return this.tableLayoutPanelNotes; } }

        #endregion

        #region private

        private void initControl()
        {
            this.textBoxOriginalNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "OriginalNotes", true));
            this.textBoxAdditionalNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "AdditionalNotes", true));
            this.textBoxProblems.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Problems", true));
            this.textBoxInternalNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "InternalNotes", true));

            this.listBoxProjects.DataSource = this._SourceProjectList;
            this.listBoxProjects.DisplayMember = "Project";
            this.listBoxProjects.ValueMember = "ProjectID";

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();
        }

        #region Project

        private void toolStripButtonProjectNew_Click(object sender, EventArgs e)
        {
            //System.Data.DataTable Projects = this.DtProjects.Copy();
            System.Data.DataTable Projects = DiversityCollection.LookupTable.DtProjectList.Copy();
            foreach (System.Data.DataRow R in Projects.Rows)
            {
                foreach (System.Data.DataRow rCP in _iMainForm.DataSetCollectionSpecimen().CollectionProject.Rows)
                {
                    try
                    {
                        if (R["ProjectID"].ToString() == rCP["ProjectID"].ToString())
                            R.Delete();
                    }
                    catch { }
                }
            }
            DiversityCollection.Forms.FormGetProject f = new Forms.FormGetProject(Projects);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                try
                {
                    if (f.ProjectID != null)
                    {
                        if (!UseProjectIfLocked((int)f.ProjectID))
                            return;
                        object[] rowVals = new object[2];
                        rowVals[0] = _iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows[0]["CollectionSpecimenID"];
                        rowVals[1] = (int)f.ProjectID;
                        _iMainForm.DataSetCollectionSpecimen().CollectionProject.Rows.Add(rowVals);
                        this._iMainForm.saveSpecimen(); // this.FormFunctions.updateTable(_iMainForm.DataSetCollectionSpecimen(), "CollectionProject", this.sqlDataAdapterProject, this.BindingContext);
                        this.fillProjectList();
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private bool UseProjectIfLocked(int ProjectID)
        {
            bool UseProject = true;
            string SQL = "SELECT IsLocked FROM ProjectProxy WHERE ProjectID = " + ProjectID.ToString();
            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (Result == "1")
            {
                if (System.Windows.Forms.MessageBox.Show("This project is locked.\r\nThe dataset will be set to READ ONLY if this project is added. \r\nDo you really want to add this project", "Adding locked project?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    UseProject = false;
            }
            return UseProject;
        }

        private void toolStripButtonProjectNoAccessNew_Click(object sender, EventArgs e)
        {
            System.Data.DataTable Projects = DiversityCollection.LookupTable.DtProjectNoAccessList.Copy();
            foreach (System.Data.DataRow R in Projects.Rows)
            {
                foreach (System.Data.DataRow rCP in _iMainForm.DataSetCollectionSpecimen().CollectionProject.Rows)
                {
                    try
                    {
                        if (R["ProjectID"].ToString() == rCP["ProjectID"].ToString())
                            R.Delete();
                    }
                    catch { }
                }
            }
            DiversityCollection.Forms.FormGetProject f = new Forms.FormGetProject(Projects, "", false);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                try
                {
                    if (f.ProjectID != null)
                    {
                        object[] rowVals = new object[2];
                        rowVals[0] = _iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows[0]["CollectionSpecimenID"];
                        rowVals[1] = (int)f.ProjectID;
                        _iMainForm.DataSetCollectionSpecimen().CollectionProject.Rows.Add(rowVals);
                        this._iMainForm.saveSpecimen();// this.FormFunctions.updateTable(_iMainForm.DataSetCollectionSpecimen(), "CollectionProject", this.sqlDataAdapterProject, this.BindingContext);
                        this.fillProjectList();
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void toolStripButtonProjectDelete_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
            try
            {
                if (this.listBoxProjects.Items.Count < 2)
                {
                    System.Windows.Forms.MessageBox.Show("The last project can not be deleted from this list");
                    return;
                }
                string Project = rv["Project"].ToString();
                if (System.Windows.Forms.MessageBox.Show("Do you want to remove this specimen from the project \n\r\n\r" + Project, "Delete project?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int ProjectID = int.Parse(rv.Row["ProjectID"].ToString()); // int.Parse(_iMainForm.DataSetCollectionSpecimen().CollectionProjectList.Rows[_Source.Position]["ProjectID"].ToString());
                    //this._Source.RemoveCurrent();
                    foreach (System.Data.DataRow R in _iMainForm.DataSetCollectionSpecimen().CollectionProject.Rows)
                    {
                        if (R["ProjectID"].ToString() == ProjectID.ToString())
                        {
                            R.Delete();
                            break;
                        }
                    }
                    this._iMainForm.saveSpecimen();// this.FormFunctions.updateTable(_iMainForm.DataSetCollectionSpecimen(), "CollectionProject", this.sqlDataAdapterProject, this.BindingContext);
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonProjectOpen_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormRemoteQuery f;
            try
            {
                if (this.listBoxProjects.SelectedItem != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
                    if (!R["ProjectID"].Equals(System.DBNull.Value))
                    {
                        int ID = int.Parse(R["ProjectID"].ToString());
                        string SQL = "SELECT ProjectURI FROM ProjectProxy WHERE ProjectID = " + ID.ToString(); ;
                        string URI = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                        DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
                        f = new DiversityWorkbench.Forms.FormRemoteQuery(URI, P);
                        f.ShowDialog();
                    }
                }
                //if (this._Source != null)
                //{

                //    System.Data.DataRowView RU = (System.Data.DataRowView)this._Source.Current;
                //    int ID = 0;
                //    if (RU != null && RU.Row.Table.TableName == "CollectionProject")
                //    {
                //        if (!RU["ProjectID"].Equals(System.DBNull.Value))
                //        {
                //            ID = int.Parse(RU["ProjectID"].ToString());
                //            DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
                //            f = new DiversityWorkbench.Forms.FormRemoteQuery(ID, (DiversityWorkbench.IWorkbenchUnit)P);
                //            f.ShowDialog();
                //        }
                //    }
                //}
                //else
                //{
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error when trying to open the project. Message: " + ex.Message);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void fillProjectList()
        {
            try
            {
                if (this._iMainForm.ID_Specimen() > -1)
                {
                    // Available
                    string SQL = "SELECT CollectionProject.CollectionSpecimenID, CollectionProject.ProjectID, ProjectList.Project " +
                            "FROM CollectionProject INNER JOIN " +
                            "ProjectList ON CollectionProject.ProjectID = ProjectList.ProjectID " +
                            "WHERE CollectionProject.CollectionSpecimenID = " + this._iMainForm.ID_Specimen().ToString() +
                            " AND ProjectList.ReadOnly = 0 " +
                            " ORDER BY ProjectList.Project";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    _iMainForm.DataSetCollectionSpecimen().CollectionProjectList.Clear();
                    ad.Fill(_iMainForm.DataSetCollectionSpecimen().CollectionProjectList);
                    this.listBoxProjects.SelectedIndex = -1;

                    bool AllowProjectRemoval = DiversityWorkbench.Database.DatabaseRoles().Contains("Administrator") || DiversityWorkbench.Database.DatabaseRoles().Contains("db_owner");

                    // Read only
                    SQL = "SELECT CollectionProject.CollectionSpecimenID, CollectionProject.ProjectID, ProjectList.Project " +
                        "FROM CollectionProject INNER JOIN " +
                        "ProjectList ON CollectionProject.ProjectID = ProjectList.ProjectID " +
                        "WHERE CollectionProject.CollectionSpecimenID = " + this._iMainForm.ID_Specimen().ToString() +
                        " AND ProjectList.ReadOnly <> 0 " +
                        " ORDER BY ProjectList.Project";
                    ad.SelectCommand.CommandText = SQL;
                    System.Data.DataTable dtProjectReadOnly = new DataTable();
                    ad.Fill(dtProjectReadOnly);
                    if (dtProjectReadOnly.Rows.Count > 0)
                    {
                        this.listBoxProjectsReadOnly.DataSource = dtProjectReadOnly;
                        this.listBoxProjectsReadOnly.DisplayMember = "Project";
                        this.listBoxProjectsReadOnly.ValueMember = "Project";
                        this.listBoxProjectsReadOnly.Height = (dtProjectReadOnly.Rows.Count * this.listBoxProjectsReadOnly.ItemHeight) + 10;
                        this.listBoxProjectsReadOnly.Visible = true;
                        this.toolStripReadOnly.Visible = AllowProjectRemoval;
                        this.listBoxProjectsReadOnly.Enabled = AllowProjectRemoval;
                        if (AllowProjectRemoval)
                            this.listBoxProjectsReadOnly.SelectionMode = SelectionMode.One;
                        else
                            this.listBoxProjectsReadOnly.SelectionMode = SelectionMode.None;
                    }
                    else
                    {
                        this.listBoxProjectsReadOnly.Visible = false;
                        this.toolStripReadOnly.Visible = false;
                    }

                    // No Access
                    SQL = "SELECT ProjectProxy.Project, CollectionProject.CollectionSpecimenID, ProjectProxy.ProjectID " +
                        "FROM CollectionProject INNER JOIN " +
                        "ProjectProxy ON CollectionProject.ProjectID = ProjectProxy.ProjectID " +
                        "WHERE (CollectionProject.ProjectID NOT IN " +
                        "(SELECT ProjectID " +
                        "FROM ProjectList)) AND (CollectionProject.CollectionSpecimenID = " + this._iMainForm.ID_Specimen().ToString() + ") " +
                        "ORDER BY ProjectProxy.Project";
                    ad.SelectCommand.CommandText = SQL;
                    System.Data.DataTable dtProjectNoAccess = new DataTable();
                    ad.Fill(dtProjectNoAccess);
                    if (dtProjectNoAccess.Rows.Count > 0)
                    {
                        this.listBoxProjectsNoAccess.DataSource = dtProjectNoAccess;
                        this.listBoxProjectsNoAccess.DisplayMember = "Project";
                        this.listBoxProjectsNoAccess.ValueMember = "Project";
                        this.listBoxProjectsNoAccess.Height = (dtProjectNoAccess.Rows.Count * this.listBoxProjectsNoAccess.ItemHeight) + 10;
                        this.listBoxProjectsNoAccess.Visible = true;
                        this.listBoxProjectsNoAccess.Enabled = AllowProjectRemoval;
                        if (AllowProjectRemoval)
                            this.listBoxProjectsNoAccess.SelectionMode = SelectionMode.One;
                        else
                            this.listBoxProjectsNoAccess.SelectionMode = SelectionMode.None;
                        this.toolStripNoAccess.Visible = AllowProjectRemoval;
                    }
                    else
                    {
                        this.listBoxProjectsNoAccess.Visible = false;
                        this.toolStripNoAccess.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonNoAccessDelete_Click(object sender, EventArgs e)
        {
            if (!this.AllowProjectRemoval())
            {
                return;
            }

            try
            {
                System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxProjectsNoAccess.SelectedItem;
                if (this.RemoveProject(rv))
                    this._iMainForm.saveSpecimen();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonReadOnlyDelete_Click(object sender, EventArgs e)
        {
            if (!this.AllowProjectRemoval())
            {
                return;
            }

            try
            {
                System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxProjectsReadOnly.SelectedItem;
                if (this.RemoveProject(rv))
                    this._iMainForm.saveSpecimen();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private bool AllowProjectRemoval()
        {
            if (!DiversityWorkbench.Database.DatabaseRoles().Contains("Administrator") &&
                !DiversityWorkbench.Database.DatabaseRoles().Contains("db_owner"))
            {
                System.Windows.Forms.MessageBox.Show("Please turn to your administrator to remove the specimen from this project");
                return false;
            }
            return true;
        }

        private bool RemoveProject(System.Data.DataRowView rv)
        {
            bool OK = false;
            if (rv == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a project from the list");
                return false;
            }
            try
            {
                string Project = rv["Project"].ToString();
                if (System.Windows.Forms.MessageBox.Show("Do you want to remove this specimen from the project \n\r\n\r" + Project, "Delete project?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int ProjectID = int.Parse(rv.Row["ProjectID"].ToString());
                    foreach (System.Data.DataRow R in _iMainForm.DataSetCollectionSpecimen().CollectionProject.Rows)
                    {
                        if (R["ProjectID"].ToString() == ProjectID.ToString())
                        {
                            R.Delete();
                            OK = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        #endregion

        #endregion

    }
}
