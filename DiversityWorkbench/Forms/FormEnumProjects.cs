using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormEnumProjects : Form
    {
        #region parameter

        private string _EnumTable;

        #endregion

        #region Construction

        public FormEnumProjects(string EnumTable)
        {
            InitializeComponent();
            this._EnumTable = EnumTable;
            this.Text += DiversityWorkbench.EnumTable.ProjectLinkColumn(this._EnumTable);
            this.labelEnum.Text = DiversityWorkbench.EnumTable.ProjectLinkColumn(this._EnumTable);
            this.initForm();
        }

        #endregion

        #region Form

        private void initForm()
        {
            this.initProjects();
        }

        #region Projects

        private void initProjects()
        {
            string SQL = "SELECT P.Project, P.ProjectID FROM ProjectList P ORDER BY P.Project";
            System.Data.DataTable dt = new DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            this.listBoxProjects.DataSource = dt;
            this.listBoxProjects.DisplayMember = "Project";
            this.listBoxProjects.ValueMember = "ProjectID";
        }

        private void listBoxProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.initEnum();
        }

        private int? ProjectID()
        {
            if (this.listBoxProjects.SelectedValue != null)
            {
                int i;
                if (int.TryParse(this.listBoxProjects.SelectedValue.ToString(), out i))
                    return i;
            }
            return null;
        }

        #endregion

        #region Enum

        private void initEnum()
        {
            if (this.ProjectID() != null)
            {
                string SQL = "SELECT M." + DiversityWorkbench.EnumTable.ProjectLinkColumn(this._EnumTable) + " AS Code " +
                    "FROM " + DiversityWorkbench.EnumTable.ProjectLinkTable(this._EnumTable) + " AS M INNER JOIN " +
                    "ProjectList AS P ON M.ProjectID = P.ProjectID " +
                    "AND  P.ProjectID  = " + this.ProjectID();
                System.Data.DataTable dataTable = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTable);
                this.listBoxEnum.DataSource = dataTable;
                this.listBoxEnum.DisplayMember = "Code";
                this.listBoxEnum.ValueMember = "Code";
            }
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            if (this.ProjectID() != null)
            {
                int ProjectID = (int)this.ProjectID();
                string SQL = "SELECT Code FROM " + this._EnumTable + 
                    " WHERE Code NOT IN(SELECT " + DiversityWorkbench.EnumTable.ProjectLinkColumn(this._EnumTable) + " FROM " + DiversityWorkbench.EnumTable.ProjectLinkTable(this._EnumTable) + " WHERE ProjectID = " + ProjectID.ToString() + " ) "+
                    " ORDER BY Code";
                System.Data.DataTable dt = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Code", "Code", "New " + DiversityWorkbench.EnumTable.ProjectLinkColumn(this._EnumTable), "Please select a " + DiversityWorkbench.EnumTable.ProjectLinkColumn(this._EnumTable), "", false, true, false, DiversityWorkbench.Properties.Resources.Project);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK && f.SelectedValue != null)
                {
                    if (f.SelectedValue.Length > 0)
                    {
                        SQL = "INSERT INTO " + DiversityWorkbench.EnumTable.ProjectLinkTable(this._EnumTable) + " (ProjectID, " + DiversityWorkbench.EnumTable.ProjectLinkColumn(this._EnumTable) + ") " +
                            "VALUES (" + ProjectID.ToString() + ", '" + f.SelectedValue + "' )";
                        if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                            this.initEnum();
                    }
                }
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            string Code = "";
            int ProjectID;
            if (this.ProjectID() != null && this.listBoxEnum.SelectedValue != null)
            {
                ProjectID = (int)this.ProjectID();
                Code = this.listBoxEnum.SelectedValue.ToString();
                string SQL = "DELETE P FROM " + DiversityWorkbench.EnumTable.ProjectLinkTable(this._EnumTable) + " P WHERE P." + DiversityWorkbench.EnumTable.ProjectLinkColumn(this._EnumTable) + " = '" + Code + "' AND P.ProjectID = " + ProjectID.ToString();
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                    this.initEnum();
            }
        }

        #endregion
        #endregion

    }
}
