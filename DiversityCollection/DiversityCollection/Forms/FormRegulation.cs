using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormRegulation : Form
    {

        #region Parameter

        private Microsoft.Data.SqlClient.SqlDataAdapter _AdapterRegulation;
        private Microsoft.Data.SqlClient.SqlDataAdapter _AdapterRegulationType_Enum;
        private bool _ForQuery = false;
        //private int? _RegulationID = null;
        private string _Regulation = "";
        private readonly string _SqlSelect = "SELECT Regulation, ParentRegulation, Type, ProjectURI, Status, Notes, HierarchyOnly FROM Regulation";
        
        #endregion

        #region Construction

        /// <summary>
        /// Administration of regulations
        /// </summary>
        public FormRegulation()
        {
            InitializeComponent();
            this.splitContainerQuery.Panel2Collapsed = true;
            //this.initTypes();
            this.LoadData();
            //string SQL = "SELECT RegulationID, ParentRegulationID, Regulation, Type, ProjectURI, Notes FROM Regulation ORDER BY Regulation";
            //DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._AdapterRegulation, this.dataSetRegulation.Regulation, SQL, DiversityWorkbench.Settings.ConnectionString);
            this.initProject();
            this.initTree();
        }

        // search for a regulation
        public FormRegulation(string Regulation, bool ForQuery)
        {
            InitializeComponent();
            this._ForQuery = ForQuery;
            this.splitContainerQuery.Panel1Collapsed = true;
            this.splitContainerQuery.Panel2Collapsed = !this._ForQuery;
            this.splitContainerMain.Panel1Collapsed = !this._ForQuery;
            //this.initTypes();
            string SQL = this._SqlSelect + " WHERE 1 = 1 ";
            if (Regulation != null && Regulation.Length > 0)
                SQL += " AND Regulation = '" + Regulation + "'";
            if (this._ForQuery)
                SQL += " AND HierarchyOnly = 0 ";
            this.LoadData(SQL);
            //DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._AdapterRegulation, this.dataSetRegulation.Regulation, SQL, DiversityWorkbench.Settings.ConnectionString);
            this.initProject();
            this.initTree();
            this.userControlDialogPanel.Visible = this._ForQuery;
        }

        // selection of a regulation from a list
        //public FormRegulation(System.Collections.Generic.List<int> RegulationIDs)
        //{
        //    InitializeComponent();
        //    this._ForQuery = true;
        //    this.splitContainerQuery.Panel1Collapsed = true;
        //    this.splitContainerQuery.Panel2Collapsed = !this._ForQuery;
        //    this.initTypes();
        //    string SQL = "";
        //    foreach (int ID in RegulationIDs)
        //    {
        //        if (SQL.Length > 0)
        //            SQL += ", ";
        //        SQL += ID.ToString();
        //    }
        //    SQL = this._SqlSelect + " WHERE RegulationID IN (" + SQL + ")";
        //    this.LoadData(SQL);

        //    //DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._AdapterRegulation, this.dataSetRegulation.Regulation, SQL, DiversityWorkbench.Settings.ConnectionString);
        //    this.initProject();
        //    this.initTree();
        //    this.userControlDialogPanel.Visible = this._ForQuery;
        //}

        #endregion

        #region Form

        private void FormRegulation_Load(object sender, EventArgs e)
        {
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetRegulation.RegulationType_Enum". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.regulationType_EnumTableAdapter.Fill(this.dataSetRegulation.RegulationType_Enum);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetRegulation.Regulation". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.regulationTableAdapter.Fill(this.dataSetRegulation.Regulation);

        }
        
        private void FormRegulation_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this._ForQuery)
                this.SaveData();
        }

        private bool SaveData()
        {
            bool OK = true;
            try
            {
                this._AdapterRegulation.Update(this.dataSetRegulation.Regulation);
                this.LoadData();
                this.initTree();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                OK = false;
            }
            return OK;
        }

        private void LoadData()
        {
            string SQL = this._SqlSelect + " ORDER BY Regulation";
            this.dataSetRegulation.Regulation.Clear();
            DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._AdapterRegulation, this.dataSetRegulation.Regulation, SQL, DiversityWorkbench.Settings.ConnectionString);
        }

        private void LoadData(string SQL)
        {
            DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._AdapterRegulation, this.dataSetRegulation.Regulation, SQL, DiversityWorkbench.Settings.ConnectionString);
        }

        #endregion

        private void initProject()
        {
            DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
            this.userControlModuleRelatedEntryProject.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)P;
            this.userControlModuleRelatedEntryProject.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace;
            this.userControlModuleRelatedEntryProject.bindToData("Regulation", "Regulation", "ProjectURI", this.regulationBindingSource);
        }

        #region Tree

        private void initTree()
        {
            this.treeViewRegulation.Nodes.Clear();
            try
            {
                System.Data.DataRow[] RR = this.dataSetRegulation.Regulation.Select("ParentRegulation IS NULL", "Regulation");
                foreach (System.Data.DataRow R in RR)
                {
                    System.Windows.Forms.TreeNode N = new TreeNode(R["Regulation"].ToString());
                    N.Tag = R;
                    this.treeViewRegulation.Nodes.Add(N);
                    this.AddNodeChildren(N);
                }
                this.treeViewRegulation.ExpandAll();
            }
            catch (System.Exception ex)
            {
            }
        }

        private void AddNodeChildren(System.Windows.Forms.TreeNode ParentNode)
        {
            try
            {
                System.Data.DataRow RP = (System.Data.DataRow)ParentNode.Tag;
                string ParentCode = RP["Regulation"].ToString().Replace("'", "''");
                System.Data.DataRow[] RR = this.dataSetRegulation.Regulation.Select("ParentRegulation = '" + ParentCode + "'", "Regulation");
                foreach (System.Data.DataRow R in RR)
                {
                    System.Windows.Forms.TreeNode N = new TreeNode(R["Regulation"].ToString());
                    N.Tag = R;
                    ParentNode.Nodes.Add(N);
                    this.AddNodeChildren(N);
                }
            }
            catch (System.Exception ex)
            { }
        }

        private void treeViewRegulation_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.tableLayoutPanel.Enabled = true;
            System.Data.DataRow R = (System.Data.DataRow)this.treeViewRegulation.SelectedNode.Tag;
            for (int i = 0; i < this.dataSetRegulation.Regulation.Rows.Count; i++)
            {
                if (R.RowState != DataRowState.Deleted &&
                    this.dataSetRegulation.Regulation.Rows[i].RowState != DataRowState.Deleted &&
                    R["Regulation"].ToString().ToLower() == this.dataSetRegulation.Regulation.Rows[i]["Regulation"].ToString().ToLower())
                {
                    this.regulationBindingSource.Position = i;
                    break;
                }
            }
        }
        
        #endregion

        #region Types

        //private void initTypes()
        //{
        //    string SQL = "SELECT Code, Description, DisplayText, DisplayOrder, DisplayEnable, InternalNotes, ParentCode " +
        //        "FROM RegulationType_Enum ORDER BY DisplayText";
        //    DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._AdapterRegulationType_Enum, this.dataSetRegulation.RegulationType_Enum, SQL, DiversityWorkbench.Settings.ConnectionString);
        //    DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxType, "RegulationType_Enum", DiversityWorkbench.Settings.Connection, false, false, false);
        //}

        //private void buttonEditTypes_Click(object sender, EventArgs e)
        //{
        //    DiversityWorkbench.Forms.FormEnumAdministration f = new DiversityWorkbench.Forms.FormEnumAdministration(DiversityCollection.Resource.Paragraph, "RegulationType_Enum", "Regulation types", "");
        //    f.ShowDialog();
        //    this.initTypes();
        //    DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxType, "RegulationType_Enum", DiversityWorkbench.Settings.Connection, false, false, false);
        //}
        
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

        public string Regulation()
        {
            if (this._ForQuery && this.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                return null;

            return this._Regulation;
        }

        //public int? RegulationID()
        //{
        //    if (this._ForQuery && this.DialogResult == System.Windows.Forms.DialogResult.Cancel)
        //        return null;

        //    return this._RegulationID;
        //    //if (this.listBoxQueryResult.SelectedIndex > -1)
        //    //{
        //    //    //System.Data.DataRowView R = (System.Data.DataRowView)this.regulationBindingSource.Current;
        //    //    //return int.Parse(R["RegulationID"].ToString());
        //    //    return int.Parse(this.listBoxQueryResult.SelectedValue.ToString());
        //    //}
        //    //else return null;
        //}

        #endregion

        #region toolstrip

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New reglation", "Please enter the title of the new regulation", "");
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    System.Data.DataRow R = this.dataSetRegulation.Regulation.NewRegulationRow();
                    R["Regulation"] = f.String;
                    this.dataSetRegulation.Regulation.Rows.Add(R);
                    this.SaveData();
                }
            }
            catch (System.Exception ex)
            { }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.regulationBindingSource.Current;
            R.Delete();
            this.SaveData();
        }
        
        private void toolStripButtonSetParent_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.List<string> Regs = new List<string>();
            //System.Collections.Generic.Dictionary<string, string> RegDict = new Dictionary<string, string>();
            System.Data.DataRowView R = (System.Data.DataRowView)this.regulationBindingSource.Current;
            string Reg = R["Regulation"].ToString();
            foreach(System.Data.DataRow rr in this.dataSetRegulation.Regulation.Rows)
            {
                if (rr["Regulation"].ToString() == Reg)
                    continue;
                else
                {
                    if (rr["ParentRegulation"].Equals(System.DBNull.Value))
                        Regs.Add(rr["Regulation"].ToString());
                    else
                    {
                        if (rr["ParentRegulation"].ToString() == Reg)
                            continue;
                        else
                            Regs.Add(rr["Regulation"].ToString());
                    }
                }
            }
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(Regs, "Parent regulation", "Select the parent regulation", true);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                R.BeginEdit();
                R["ParentRegulation"] = f.SelectedValue;
                R.EndEdit();
                //string SQL = "UPDATE Regulation SET ParentRegulation = '" + f.SelectedValue + "' WHERE Regulation = '" + R["Regulation"].ToString() + "'";
                //DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                this.SaveData();
            }
        }

        private void toolStripButtonRemoveParent_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.regulationBindingSource.Current;
            R.BeginEdit();
            R["ParentRegulation"] = System.DBNull.Value;
            R.EndEdit();
            this.SaveData();
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            this.SaveData();
        }

        #endregion

        #region Query
        
        private void listBoxQueryResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxQueryResult.SelectedItem != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxQueryResult.SelectedItem;
                this._Regulation = R["Regulation"].ToString();
            }
        }

        private void toolStripButtonQuery_Click(object sender, EventArgs e)
        {
            string SQL = this._SqlSelect;
            if (this.toolStripTextBoxQuery.Text.Length > 0) 
                SQL += " WHERE Regulation LIKE '" + this.toolStripTextBoxQuery.Text + "%' AND HierarchyOnly = 0 ";
            SQL += " ORDER BY Regulation ";
            this.dataSetRegulation.Regulation.Clear();
            DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._AdapterRegulation, this.dataSetRegulation.Regulation, SQL, DiversityWorkbench.Settings.ConnectionString);
        }
        
        #endregion

        private void comboBoxStatus_DropDown(object sender, EventArgs e)
        {
            System.Data.DataTable dtStatus = new DataTable();
            string SQL = "SELECT DISTINCT [Status] " +
                "FROM Regulation " +
                "UNION " +
                "SELECT NULL AS [Status] " +
                "ORDER BY [Status]";
            string Message = "";
            if (DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtStatus, ref Message))
            {
                this.comboBoxStatus.DataSource = dtStatus;
                this.comboBoxStatus.DisplayMember = "Status";
                this.comboBoxStatus.ValueMember = "Status";
            }
        }

    }
}
