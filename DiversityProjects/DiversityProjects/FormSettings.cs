using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityProjects
{
    public partial class FormSettings : Form
    {

        #region Construction

        /// <summary>
        /// Form for selecting and editing settings
        /// </summary>
        /// <param name="ProjectID">If a new setting should be selected, missing so far for a project</param>
        public FormSettings(int? ProjectID, System.Data.DataTable DtProjects)
        {
            InitializeComponent();
            if (DtProjects != null)
            {
                this.listBoxProjects.Visible = true;
                this.listBoxProjects.DataSource = DtProjects;
                this.listBoxProjects.DisplayMember = "Project";
                this.listBoxProjects.ValueMember = "ProjectID";
                this.toolStripSettings.Visible = false;
                this.userControlDialogPanel.Visible = true;
                this.Text = "Please select the project from which you want to copy the setting";
            }
            else
            {
                if (ProjectID == null)
                {
                    this.toolStripSettings.Visible = true;
                    this.userControlDialogPanel.Visible = false;
                    this.Text = "Edit the settings";
                    this.BuildSettingTree(null, false);
                }
                else
                {
                    this.toolStripSettings.Visible = false;
                    this.userControlDialogPanel.Visible = true;
                    this.Text = "Please select a setting";
                    this.BuildSettingTree(ProjectID, true);
                }
                this.listBoxProjects.Visible = false;
            }
        }

        #endregion

        #region Interface

        private int? _SelectedSettingID;
        public int? SelectedSettingID() { return this._SelectedSettingID; }

        private int? _SelectedProjectID;
        public int? SelectedProjectID() { return this._SelectedProjectID; }

        #endregion

        private DiversityProjects.Setting _Setting = new Setting();

        #region Tree

        private void treeViewSettings_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.treeViewSettings.SelectedNode != null
                && this.treeViewSettings.SelectedNode.Tag != null)
            {
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewSettings.SelectedNode.Tag;
                this._SelectedSettingID = int.Parse(R["SettingID"].ToString());
                this.toolStripTextBoxSetting.Text = R["DisplayText"].ToString();
                this.toolStripTextBoxDisplayOrder.Text = R["DisplayOrder"].ToString();
                this.toolStripTextBoxID.Text = R["SettingID"].ToString();
            }
        }

        private void BuildSettingTree(int? ProjectID, bool ForSelectionForProject)
        {
            int SettingID = -1;
            if (this.treeViewSettings.SelectedNode != null)
            {
                System.Data.DataRow Rselected = (System.Data.DataRow)this.treeViewSettings.SelectedNode.Tag;
                SettingID = int.Parse(Rselected["SettingID"].ToString());
            }
            this.treeViewSettings.Nodes.Clear();
            this._Setting.BuildHierarchyTree(ref this.treeViewSettings, ProjectID, ForSelectionForProject);
            this.treeViewSettings.ExpandAll();
            if (SettingID != -1)
            {
                foreach (System.Windows.Forms.TreeNode TN in this.treeViewSettings.Nodes)
                    if (this.MarkTreeNode(SettingID, TN))
                        break;
            }
        }

        private bool MarkTreeNode(int SettingID, System.Windows.Forms.TreeNode N)
        {
            foreach (System.Windows.Forms.TreeNode TN in N.Nodes)
            {
                System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                if (!R["SettingID"].Equals(System.DBNull.Value) && R["SettingID"].ToString() == SettingID.ToString())
                {
                    N.EnsureVisible();
                    this.treeViewSettings.SelectedNode = N;
                    return true;
                }
                foreach (System.Windows.Forms.TreeNode NC in N.Nodes)
                {
                    System.Data.DataRow RC = (System.Data.DataRow)NC.Tag;
                    if (!RC["SettingID"].Equals(System.DBNull.Value) && RC["SettingID"].ToString() == SettingID.ToString())
                    {
                        NC.EnsureVisible();
                        this.treeViewSettings.SelectedNode = NC;
                        return true;
                    }
                    else
                    {
                        if (this.MarkTreeNode(SettingID, NC))
                            return true;
                    }
                }
            }
            return false;
        }

        #endregion

        private void listBoxProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxProjects.SelectedValue != null)
            {
                int i;
                if (int.TryParse(this.listBoxProjects.SelectedValue.ToString(), out i))
                {
                    this._SelectedProjectID = i;
                    this.treeViewSettings.Nodes.Clear();
                    this._Setting.BuildHierarchyTree(ref this.treeViewSettings, this._SelectedProjectID, false);
                    this.treeViewSettings.ExpandAll();
                }
            }
        }

        #region Toolstrip

        private void toolStripButtonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name,
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                null,
                null);
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            if (this.treeViewSettings.SelectedNode != null
                && this.treeViewSettings.SelectedNode.Tag != null)
            {
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewSettings.SelectedNode.Tag;
                this._SelectedSettingID = int.Parse(R["SettingID"].ToString());
            }
            else
                this._SelectedSettingID = null;
            DiversityWorkbench.Forms.FormGetString F = new DiversityWorkbench.Forms.FormGetString("New setting", "Please enter the name for the new setting", "");
            F.ShowDialog();
            if (F.DialogResult == System.Windows.Forms.DialogResult.OK && F.String.Length > 0)
            {
                int? NewSettingID = null;
                if (this._Setting.AddSetting(ref NewSettingID, F.String, this._SelectedSettingID))
                {
                    this._Setting.ResetSettingHierarchy();
                    this.BuildSettingTree(null, false);
                }
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.treeViewSettings.SelectedNode != null
                    && this.treeViewSettings.SelectedNode.Tag != null)
                {
                    if (this.treeViewSettings.SelectedNode.Nodes.Count > 0)
                    {
                        if (System.Windows.Forms.MessageBox.Show("Do you really want to delete this setting including all inferior settings?", "Delete", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                            return;
                    }
                    System.Data.DataRow R = (System.Data.DataRow)this.treeViewSettings.SelectedNode.Tag;
                    this._SelectedSettingID = int.Parse(R["SettingID"].ToString());
                    if (this._Setting.RemoveSetting((int)this._SelectedSettingID))
                    {
                        this._Setting.ResetSettingHierarchy();
                        this.BuildSettingTree(null, false);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripTextBoxSetting_Leave(object sender, EventArgs e)
        {
            if (this.treeViewSettings.SelectedNode != null
                && this.treeViewSettings.SelectedNode.Tag != null)
            {
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewSettings.SelectedNode.Tag;
                if (R["DisplayText"].ToString() != this.toolStripTextBoxSetting.Text)
                {
                    this._SelectedSettingID = int.Parse(R["SettingID"].ToString());
                    if (this._Setting.UpdateSettingDisplayText((int)this._SelectedSettingID, this.toolStripTextBoxSetting.Text))
                    {
                        this._Setting.ResetSettingHierarchy();
                        this.BuildSettingTree(null, false);
                    }
                }
            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            this.toolStripTextBoxSetting_Leave(null, null);
        }

        private void toolStripButtonParentSet_Click(object sender, EventArgs e)
        {
            if (this.treeViewSettings.SelectedNode != null
                && this.treeViewSettings.SelectedNode.Tag != null)
            {
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewSettings.SelectedNode.Tag;
                int SettingID;
                if (int.TryParse(R["SettingID"].ToString(), out SettingID))
                {
                    System.Data.DataTable dt = this._Setting.DtSettingHierarchyPotentialParents(SettingID);
                    DiversityWorkbench.Forms.FormGetStringFromList F = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Hierarchy", "SettingID", "Parent setting", "Please select the superior setting from the list");
                    F.ShowDialog();
                    if (F.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        R["ParentSettingID"] = int.Parse(F.SelectedValue);
                        this._Setting.SetSettingParentID(SettingID, int.Parse(F.SelectedValue));
                        this._Setting.ResetSettingHierarchy();
                        this.BuildSettingTree(null, false);
                    }
                }
            }
        }

        private void toolStripButtonParentRemove_Click(object sender, EventArgs e)
        {
            if (this.treeViewSettings.SelectedNode != null
                && this.treeViewSettings.SelectedNode.Tag != null)
            {
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewSettings.SelectedNode.Tag;
                R.BeginEdit();
                R["ParentSettingID"] = System.DBNull.Value;
                R.EndEdit();
                int SettingID;
                if (int.TryParse(R["SettingID"].ToString(), out SettingID))
                {
                    this._Setting.SetSettingParentID(SettingID, null);
                    this._Setting.ResetSettingHierarchy();
                    this.BuildSettingTree(null, false);
                }
            }
        }

        private void toolStripTextBoxDisplayOrder_KeyUp(object sender, KeyEventArgs e)
        {
            int i;
            if (this.toolStripTextBoxDisplayOrder.Text.Length > 0 &&
                !int.TryParse(this.toolStripTextBoxDisplayOrder.Text, out i))
            {
                string Message = this.toolStripTextBoxDisplayOrder.Text + " is not a number";
                System.Windows.Forms.MessageBox.Show(Message);
                this.toolStripTextBoxDisplayOrder.Text = "";
            }
        }

        private void toolStripTextBoxDisplayOrder_Leave(object sender, EventArgs e)
        {
            if (this.treeViewSettings.SelectedNode != null
                && this.treeViewSettings.SelectedNode.Tag != null)
            {
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewSettings.SelectedNode.Tag;
                if (R["DisplayOrder"].ToString() != this.toolStripTextBoxDisplayOrder.Text)
                {
                    this._SelectedSettingID = int.Parse(R["SettingID"].ToString());
                    if (this._Setting.UpdateSettingDisplayOrder((int)this._SelectedSettingID, this.toolStripTextBoxDisplayOrder.Text))
                    {
                        this._Setting.ResetSettingHierarchy();
                        this.BuildSettingTree(null, false);
                    }
                }
            }
        }

        #endregion

        private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.treeViewSettings.SelectedNode != null
                && this.treeViewSettings.SelectedNode.Tag != null)
            {
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewSettings.SelectedNode.Tag;
                R.BeginEdit();
                R.EndEdit();
                if (R["DisplayText"].ToString() != this.toolStripTextBoxSetting.Text ||
                    R["DisplayOrder"].ToString() != this.toolStripTextBoxDisplayOrder.Text)
                {
                    this._SelectedSettingID = int.Parse(R["SettingID"].ToString());
                    if (this._Setting.UpdateSettingDisplayText((int)this._SelectedSettingID, this.toolStripTextBoxSetting.Text) &&
                        this._Setting.UpdateSettingDisplayOrder((int)this._SelectedSettingID, this.toolStripTextBoxDisplayOrder.Text))
                    {
                        this._Setting.ResetSettingHierarchy();
                        this.BuildSettingTree(null, false);
                    }
                }
            }
        }

        private void toolStripButtonHistory_Click(object sender, EventArgs e)
        {
            System.Data.DataRow R = (System.Data.DataRow)this.treeViewSettings.SelectedNode.Tag;
            string Title = "History of " + R["DisplayText"].ToString() + " (SettingID: " + R["SettingID"].ToString() + ")";
            try
            {
                if (this._SelectedSettingID == null)
                    return;
                int SettingID = (int)this._SelectedSettingID;
                System.Collections.Generic.List<System.Data.DataTable> LogTables = new List<DataTable>();
                LogTables.Add(DiversityWorkbench.Database.DtHistory(SettingID, "SettingID", "Setting", ""));

                DiversityWorkbench.Forms.FormHistory f = new DiversityWorkbench.Forms.FormHistory(Title, LogTables, System.Windows.Forms.Application.StartupPath + "\\DiversityProjects.chm");
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
            }

            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

    }
}
