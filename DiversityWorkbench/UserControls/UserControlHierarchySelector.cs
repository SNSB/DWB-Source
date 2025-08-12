using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.UserControls
{
    public partial class UserControlHierarchySelector : UserControl
    {
        #region Parameter

        private System.Data.DataTable _dtValues;
        private System.Windows.Forms.BindingSource _BindingSource;
        private string _HierarchyColumn;
        private string _HierarchyParentColumn;
        private string _HierarchyDisplayColumn;
        private string _OrderColumn;
        private string _ValueColumn;
        private string _ImageIndexColumn;
        private System.Windows.Forms.Control _Control;
        private bool _RemoveMissingParentReferences = false;

        private bool _IsInitialized = false;
        public bool IsInitialized() { return this._IsInitialized; }

        #endregion

        #region Construction

        public UserControlHierarchySelector()
        {
            InitializeComponent();
        }

        #endregion

        #region Hierarchy
        /// <summary>
        /// set the hierarchy for the controll
        /// </summary>
        /// <param name="DtHierarchy">the data table containing the hierarchy</param>
        /// <param name="HierarchyColumn">The identity column of the hierarchy</param>
        /// <param name="HierarchyParentColumn">The column containing the reference to the identity of the superior entry in the hierarchy</param>
        /// <param name="HierarchyDisplayColumn">The display column</param>
        /// <param name="OrderColumn">The order column</param>
        /// <param name="ValueColumn">The column in the target table, related to the hierarchy table</param>
        /// <param name="BindingSource">The binding source</param>
        public void initHierarchy(
            System.Data.DataTable DtHierarchy,
            string HierarchyColumn,
            string HierarchyParentColumn,
            string HierarchyDisplayColumn,
            string OrderColumn,
            string ValueColumn,
            System.Windows.Forms.BindingSource BindingSource,
            bool BuildHierarchyWhenNeeded)
        {
            this._dtValues = DtHierarchy;
            this._BindingSource = BindingSource;
            this._HierarchyColumn = HierarchyColumn;
            this._HierarchyParentColumn = HierarchyParentColumn;
            this._HierarchyDisplayColumn = HierarchyDisplayColumn;
            this._OrderColumn = OrderColumn;
            this._ValueColumn = ValueColumn;
            if (!BuildHierarchyWhenNeeded)
                this.buildHierarchy();
            else
                this._IsInitialized = false;
        }

        /// <summary>
        /// set the hierarchy for the controll
        /// </summary>
        /// <param name="DtHierarchy">the data table containing the hierarchy</param>
        /// <param name="HierarchyColumn">The identity column of the hierarchy</param>
        /// <param name="HierarchyParentColumn">The column containing the reference to the identity of the superior entry in the hierarchy</param>
        /// <param name="HierarchyDisplayColumn">The display column</param>
        /// <param name="OrderColumn">The order column</param>
        /// <param name="ValueColumn">The column in the target table, related to the hierarchy table</param>
        /// <param name="RemoveMissingHierarchyReferences">If entries that refer to missing relations should be removed</param>
        /// <param name="BindingSource"></param>
        public void initHierarchy(
            System.Data.DataTable DtHierarchy,
            string HierarchyColumn,
            string HierarchyParentColumn,
            string HierarchyDisplayColumn,
            string OrderColumn,
            string ValueColumn,
            bool RemoveMissingHierarchyReferences,
            System.Windows.Forms.BindingSource BindingSource,
            bool BuildHierarchyWhenNeeded)
        {
            this._dtValues = DtHierarchy;
            this._BindingSource = BindingSource;
            this._HierarchyColumn = HierarchyColumn;
            this._HierarchyParentColumn = HierarchyParentColumn;
            this._HierarchyDisplayColumn = HierarchyDisplayColumn;
            this._OrderColumn = OrderColumn;
            this._ValueColumn = ValueColumn;
            this._RemoveMissingParentReferences = RemoveMissingHierarchyReferences;
            if (!BuildHierarchyWhenNeeded)
                this.buildHierarchy();
            else
                this._IsInitialized = false;
        }


        public void initHierarchy(
            System.Data.DataTable DtHierarchy,
            string HierarchyColumn,
            string HierarchyParentColumn,
            string HierarchyDisplayColumn,
            string OrderColumn,
            string ValueColumn,
            string ImageIndexColumn,
            System.Windows.Forms.ImageList ImageList,
            System.Windows.Forms.ComboBox ComboBox,
            System.Windows.Forms.BindingSource BindingSource)
        {
            this._dtValues = DtHierarchy;
            this._BindingSource = BindingSource;
            this._HierarchyColumn = HierarchyColumn;
            this._HierarchyParentColumn = HierarchyParentColumn;
            this._HierarchyDisplayColumn = HierarchyDisplayColumn;
            this._OrderColumn = OrderColumn;
            this._ValueColumn = ValueColumn;
            this._ImageIndexColumn = ImageIndexColumn;
            this._Control = ComboBox;
            this.buildHierarchy(ImageList);
        }

        public void initHierarchy(
            System.Data.DataTable DtHierarchy,
            string HierarchyColumn,
            string HierarchyParentColumn,
            string HierarchyDisplayColumn,
            string OrderColumn,
            string ValueColumn,
            System.Windows.Forms.Control Control)
        {
            this._dtValues = DtHierarchy;
            this._Control = Control;
            this._HierarchyColumn = HierarchyColumn;
            this._HierarchyParentColumn = HierarchyParentColumn;
            this._HierarchyDisplayColumn = HierarchyDisplayColumn;
            this._OrderColumn = OrderColumn;
            this._ValueColumn = ValueColumn;
            this.buildHierarchy();
        }

        public void initHierarchy(
            System.Data.DataTable DtHierarchy,
            string HierarchyColumn,
            string HierarchyParentColumn,
            string HierarchyDisplayColumn,
            string OrderColumn,
            string ValueColumn,
            bool RemoveMissingHierarchyRelations,
            System.Windows.Forms.Control Control)
        {
            this._dtValues = DtHierarchy;
            this._Control = Control;
            this._HierarchyColumn = HierarchyColumn;
            this._HierarchyParentColumn = HierarchyParentColumn;
            this._HierarchyDisplayColumn = HierarchyDisplayColumn;
            this._OrderColumn = OrderColumn;
            this._ValueColumn = ValueColumn;
            this._RemoveMissingParentReferences = RemoveMissingHierarchyRelations;
            this.buildHierarchy();
        }


        /// <summary>
        /// init the hierarchy for the control
        /// </summary>
        /// <param name="DtEnum">the standard enumeration table</param>
        /// <param name="ValueColumn">the column in the data table</param>
        /// <param name="ComboBox">the combobox bound to the value</param>
        /// <param name="BindingSource">the binding source of the datarow</param>
        public void initHierarchyForEnum(
            string DtEnum,
            string ValueColumn,
            System.Windows.Forms.ComboBox ComboBox,
            System.Windows.Forms.BindingSource BindingSource)
        {
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString))
                {
                    con.Open();
                    this._dtValues = DiversityWorkbench.EnumTable.EnumTableForQuery(DtEnum, true, true, true);
                    DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(ComboBox, DtEnum, con, true, true, true);
                    con.Close();
                }
                this._BindingSource = BindingSource;
                this._HierarchyColumn = "Code";
                this._HierarchyParentColumn = "ParentCode";
                this._HierarchyDisplayColumn = "DisplayText";
                this._OrderColumn = "DisplayText";
                this._ValueColumn = ValueColumn;
                this._Control = ComboBox;
                this.buildHierarchy();
            }
        }

        /// <summary>
        /// Remove references to entries that are missing, e.g. if only a part of the table is accessbile for a user
        /// and the entries would find no starting point
        /// </summary>
        public void RemoveMissingParentReferences()
        {
            try
            {
                foreach (System.Data.DataRow r in this._dtValues.Select("NOT " + this._HierarchyParentColumn + " IS NULL"))
                {
                    string Parent = r[this._HierarchyParentColumn].ToString();
                    System.Data.DataRow[] rr = this._dtValues.Select(this._HierarchyColumn + " = " + Parent);
                    if (rr.Length == 0)
                        r[this._HierarchyParentColumn] = System.DBNull.Value;
                }
            }
            catch (System.Exception ex) { }
        }

        private void buildHierarchy(bool IgnoreQueryLimitHierarchy = false)
        {
            try
            {
                if (this._RemoveMissingParentReferences)
                    this.RemoveMissingParentReferences();
                System.Data.DataRow[] rr = this._dtValues.Select(this._HierarchyParentColumn + " IS NULL", this._OrderColumn);
                this.toolStripDropDownButton.DropDownItems.Clear();
                // Check if there are any hierarchical entries
                System.Data.DataRow[] rrHierarchy = this._dtValues.Select(this._HierarchyParentColumn + " IS NOT NULL", this._OrderColumn);
                if (rrHierarchy.Length > 0)
                {
                    // Markus 20.02.2020 Restrict to max number - otherwise very time consuming
                    if (rr.Length > 0 && (rr.Length < DiversityWorkbench.Settings.QueryLimitHierarchy || IgnoreQueryLimitHierarchy))
                    {
                        for (int i = 0; i < rr.Length; i++)
                        {
                            string Display = rr[i][this._HierarchyDisplayColumn].ToString();
                            System.Windows.Forms.ToolStripMenuItem M = new ToolStripMenuItem(Display, null, this.ToolStripMenuItem_Click);
                            M.Tag = rr[i];
                            this.toolStripDropDownButton.DropDownItems.Add(M);
                            this.appendHierarchyChilds(M);
                        }
                    }
                }
                this._IsInitialized = true;
            }
            catch { }
        }

        private void buildHierarchy(System.Windows.Forms.ImageList ImageList)
        {
            try
            {
                if (this._RemoveMissingParentReferences)
                    this.RemoveMissingParentReferences();
                System.Data.DataRow[] rr = this._dtValues.Select(this._HierarchyParentColumn + " IS NULL", this._OrderColumn);
                this.toolStripDropDownButton.DropDownItems.Clear();
                for (int i = 0; i < rr.Length; i++)
                {
                    string Display = rr[i][this._HierarchyDisplayColumn].ToString();
                    System.Windows.Forms.ToolStripMenuItem M;
                    if (rr[i][this._ImageIndexColumn].Equals(System.DBNull.Value))
                        M = new ToolStripMenuItem(Display, null, this.ToolStripMenuItem_Click);
                    else
                    {
                        System.Drawing.Image I = ImageList.Images[int.Parse(rr[i][this._ImageIndexColumn].ToString())];
                        M = new ToolStripMenuItem(Display, I, this.ToolStripMenuItem_Click);
                    }
                    M.Tag = rr[i];
                    this.toolStripDropDownButton.DropDownItems.Add(M);
                    this.appendHierarchyChilds(M, ImageList);
                }
            }
            catch { }
        }

        private void appendHierarchyChilds(System.Windows.Forms.ToolStripMenuItem M)
        {
            try
            {
                System.Data.DataRow R = (System.Data.DataRow)M.Tag;
                if (R[this._HierarchyColumn].Equals(System.DBNull.Value) || R[this._HierarchyColumn].ToString().Length == 0) return;
                string Select = this._HierarchyParentColumn + " = '" + R[this._HierarchyColumn].ToString() + "'";
                System.Data.DataRow[] rr = this._dtValues.Select(Select, this._OrderColumn);
                for (int i = 0; i < rr.Length; i++)
                {
                    string Display = rr[i][this._HierarchyDisplayColumn].ToString();
                    System.Windows.Forms.ToolStripMenuItem MChild = new ToolStripMenuItem(Display, null, this.ToolStripMenuItem_Click);
                    MChild.Tag = rr[i];
                    this.appendHierarchyChilds(MChild);
                    M.DropDownItems.Add(MChild);
                }
            }
            catch { }
        }

        private void appendHierarchyChilds(System.Windows.Forms.ToolStripMenuItem M, ImageList ImageList)
        {
            try
            {
                System.Data.DataRow R = (System.Data.DataRow)M.Tag;
                if (R[this._HierarchyColumn].Equals(System.DBNull.Value) || R[this._HierarchyColumn].ToString().Length == 0) return;
                string Select = this._HierarchyParentColumn + " = '" + R[this._HierarchyColumn].ToString() + "'";
                System.Data.DataRow[] rr = this._dtValues.Select(Select, this._OrderColumn);
                for (int i = 0; i < rr.Length; i++)
                {
                    string Display = rr[i][this._HierarchyDisplayColumn].ToString();
                    System.Windows.Forms.ToolStripMenuItem MChild;
                    if (rr[i][this._ImageIndexColumn].Equals(System.DBNull.Value))
                        MChild = new ToolStripMenuItem(Display, null, this.ToolStripMenuItem_Click);
                    else
                    {
                        System.Drawing.Image I = ImageList.Images[int.Parse(rr[i][this._ImageIndexColumn].ToString())];
                        MChild = new ToolStripMenuItem(Display, I, this.ToolStripMenuItem_Click);
                    }
                    MChild.Tag = rr[i];
                    this.appendHierarchyChilds(MChild);
                    M.DropDownItems.Add(MChild);
                }
            }
            catch { }
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.ToolStripMenuItem M = (System.Windows.Forms.ToolStripMenuItem)sender;
                if (M.Tag != null)
                {
                    System.Data.DataRow R = (System.Data.DataRow)M.Tag;
                    this.toolStripDropDownButton.Text = R[this._HierarchyDisplayColumn].ToString();
                    this.toolStrip.Tag = R;

                    if (this._BindingSource != null)
                    {
                        System.Data.DataRowView RV = (System.Data.DataRowView)this._BindingSource.Current;
                        if (RV.Row.Table.Columns.Contains(this._ValueColumn))
                        {
                            RV.BeginEdit();
                            RV[this._ValueColumn] = R[this._HierarchyColumn];
                            RV.EndEdit();
                        }
                    }
                    else if (this._Control != null)
                    {
                        if (this._Control.GetType() == typeof(System.Windows.Forms.ComboBox))
                        {
                            System.Windows.Forms.ComboBox CB = (System.Windows.Forms.ComboBox)this._Control;
                            if (CB.DataSource != null)
                            {
                                System.Data.DataTable dt = (System.Data.DataTable)CB.DataSource;
                                string ValueColumn = CB.ValueMember;
                                if (CB.Sorted)
                                {
                                    int i = 0;
                                    for (i = 0; i < CB.Items.Count; i++)
                                    {
                                        System.Data.DataRowView RV = (System.Data.DataRowView)CB.Items[i];
                                        if (RV[ValueColumn].ToString() == R[this._ValueColumn].ToString())
                                            break;
                                    }
                                    CB.SelectedIndex = i;
                                }
                                else
                                {
                                    int i = 0;
                                    for (i = 0; i < dt.Rows.Count; i++)
                                    {
                                        if (dt.Rows[i][ValueColumn].ToString() == R[this._ValueColumn].ToString())
                                            break;
                                    }
                                    CB.SelectedIndex = i;
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private string HierarchyString(string Key)
        {
            if (Key.Length == 0) return "";
            string Hierarchy = "";
            try
            {
                string Select = this._HierarchyColumn + " = '" + Key + "'";
                System.Data.DataRow[] rr = this._dtValues.Select(Select, this._OrderColumn);
                if (rr.Length > 0)
                {
                    Hierarchy = this.HierarchyString(rr[0][this._HierarchyParentColumn].ToString());
                    if (Hierarchy.Length > 0) Hierarchy += " - ";
                    Hierarchy += rr[0][this._HierarchyDisplayColumn].ToString();
                }
            }
            catch { }
            return Hierarchy;
        }

        private void toolStripDropDownButton_MouseEnter(object sender, EventArgs e)
        {
            if (this._BindingSource != null)
            {
                System.Data.DataRowView RV = (System.Data.DataRowView)this._BindingSource.Current;
                if (RV == null || !RV.Row.Table.Columns.Contains(this._ValueColumn)) // Markus 16.9.2019 - RV may be null
                    return;
                if (RV[this._ValueColumn].Equals(System.DBNull.Value))
                {
                    this.toolStripDropDownButton.ToolTipText = this.Message("Select_an_item_from_the_hierarchy");// "Select an item from the hierarchy";//ToolTip;
                }
                else
                {
                    string ToolTip = this.HierarchyString(RV[this._ValueColumn].ToString());
                    if (ToolTip.Length == 0) ToolTip = this.Message("Select_an_item_from_the_hierarchy");// "Select an item from the hierarchy";
                    this.toolStripDropDownButton.ToolTipText = ToolTip;
                }
            }
            else if (this._Control != null)
            {
                if (this._Control.GetType() == typeof(System.Windows.Forms.ComboBox))
                {
                    System.Windows.Forms.ComboBox CB = (System.Windows.Forms.ComboBox)this._Control;
                    if (CB.DataSource != null)
                    {
                        if (CB.SelectedIndex > -1)
                        {
                            string ToolTip = this.HierarchyString(CB.SelectedValue.ToString());
                            if (ToolTip.Length == 0) ToolTip = this.Message("Select_an_item_from_the_hierarchy");// "Select an item from the hierarchy";
                            this.toolStripDropDownButton.ToolTipText = ToolTip;
                        }
                    }
                }
            }
        }

        private void initAutoCompletion()
        {
            try
            {
                if (this._Control.GetType() == typeof(System.Windows.Forms.ComboBox))
                {
                    System.Windows.Forms.ComboBox CB = (System.Windows.Forms.ComboBox)this._Control;
                    if (CB.AutoCompleteCustomSource == null)
                        DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(CB, AutoCompleteMode.SuggestAppend);
                }
            }
            catch { }
        }

        #endregion

        private string Message(string Resource)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkbenchMessages));
            string Message = resources.GetString(Resource);
            return Message;
        }

        private void toolStripDropDownButton_Click(object sender, EventArgs e)
        {
            if (!this.IsInitialized())
                this.buildHierarchy();
        }

    }
}
