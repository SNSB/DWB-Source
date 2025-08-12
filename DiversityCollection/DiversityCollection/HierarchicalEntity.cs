using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace DiversityCollection
{
    interface FormHierarchicalEntity
    {
        void setFormControls();
    }

    public class HierarchicalEntity
    {
        #region Parameter

        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        protected System.Windows.Forms.ToolTip _toolTip;
        protected System.Windows.Forms.Form _Form;
        protected string _sqlItemFieldList = "";
        protected string _sqlSpecimenCount = "";
        protected string _MainTable = "";
        protected System.Collections.Generic.List<string> _OrderColumns = new List<string>();
        protected System.Data.DataTable _LookupTable;
        protected System.Data.DataTable _LookupTableHierarchy;
        protected string _SpecimenTable = "";
        protected System.Windows.Forms.TreeView _TreeView;
        protected bool _UseHierarchyNodes = false;
        protected System.Data.DataSet _DataSet;
        protected System.Data.DataTable _DataTable;
        protected System.Data.DataTable _DataTableDepend;
        protected System.Windows.Forms.BindingSource _BindingSource;
        protected Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapter;
        protected Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterDepend;
        protected Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterDepend_2;
        protected Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterDepend_3;
        protected Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterDepend_4;
        protected Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterDepend_5;
        protected Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterDepend_6;
        protected DiversityWorkbench.UserControls.UserControlQueryList _UserControlQueryList;
        protected System.Windows.Forms.SplitContainer _SplitContainerMain;
        protected System.Windows.Forms.SplitContainer _SplitContainerData;
        protected System.Windows.Forms.ToolStripButton _ToolStripButtonSpecimenList;
        //protected System.Windows.Forms.ImageList _ImageListSpecimenList;
        protected DiversityCollection.UserControls.UserControlSpecimenList _UserControlSpecimenList;
        protected System.Windows.Forms.HelpProvider _HelpProvider;
        protected int? _ID = null;
        protected System.Windows.Forms.Control _ControlMainData;
        protected System.Windows.Forms.Control _ControlDependentData;

        protected bool _MainTableContainsHierarchy = true;

        protected bool _ShowSpecimenLists = false;

        private bool _suppressSelectedIndexChanged = false;
        
        /// <summary>
        /// list of the controls that should only be shown for a child entry
        /// </summary>
        private System.Collections.Generic.List<System.Windows.Forms.Control> _ChildControls;
        protected bool _IncludeIDinTreeview = false;

        public enum HierarchyDisplayState { Show, Hide, Children, Parents }

        protected HierarchyDisplayState _hierarchyDisplayState = HierarchyDisplayState.Show;
        public string HierarchyStateOfDisplay() { return _hierarchyDisplayState.ToString(); }
        public void SetHierarchyDisplayState(HierarchyDisplayState State)
        {
            this._hierarchyDisplayState = State;
        }

        public HierarchyDisplayState GetHierarchyDisplayState() { return this._hierarchyDisplayState; }

        #endregion

        #region Construction

        public HierarchicalEntity(
            ref System.Data.DataSet Dataset,
            System.Data.DataTable DataTable, 
            ref System.Windows.Forms.TreeView TreeView, 
            System.Windows.Forms.Form Form, 
            DiversityWorkbench.UserControls.UserControlQueryList UserControlQueryList,
            System.Windows.Forms.SplitContainer SplitContainerMain,
            System.Windows.Forms.SplitContainer SplitContainerData,
            System.Windows.Forms.ToolStripButton ToolStripButtonSpecimenList,
            //System.Windows.Forms.ImageList ImageListSpecimenList,
            DiversityCollection.UserControls.UserControlSpecimenList UserControlSpecimenList,
            System.Windows.Forms.HelpProvider HelpProvider,
            System.Windows.Forms.ToolTip ToolTip, 
            ref System.Windows.Forms.BindingSource BindingSource,
            System.Data.DataTable LookupTable,
            System.Data.DataTable LookUpTableHierarchy)
        {
            try
            {
                this._Form = Form;
                this._UserControlQueryList = UserControlQueryList;
                this._SplitContainerMain = SplitContainerMain;
                this._SplitContainerData = SplitContainerData;
                this._ToolStripButtonSpecimenList = ToolStripButtonSpecimenList;
                //this._ImageListSpecimenList = ImageListSpecimenList;
                this._UserControlSpecimenList = UserControlSpecimenList;
                this._HelpProvider = HelpProvider;
                this._toolTip = ToolTip;
                this._TreeView = TreeView;
                this._DataSet = Dataset;
                this._DataTable = DataTable;
                this._BindingSource = BindingSource;
                if (LookupTable != null)
                    this._LookupTable = LookupTable;
                if (LookUpTableHierarchy != null)
                    this._LookupTableHierarchy = LookUpTableHierarchy;
            }
            catch (System.Exception ex)
            {
            }
        }
        
        #endregion

        #region Properties

        public string SqlItemHierarchy { get { return this.sqlItemFieldList + " FROM dbo."  + this.MainTableHierarchy; } }

        public string SqlItem(int? ID)
        {
            string SQL = "SELECT " + this.sqlItemFieldList + " FROM dbo." + this._MainTable;
            if (ID != null)
                SQL += " WHERE " + this.ColumnID + " = " + ID.ToString();
            return SQL;
        }

        public void setLookUpTableHierarchy(System.Data.DataTable LookUpTableHierarchy)
        {
            if (LookUpTableHierarchy != null)
                this._LookupTableHierarchy = LookUpTableHierarchy;
        }

        protected virtual string SqlSpecimenCount(int ID) { return ""; }

        public string MainTable { get { return this._MainTable; } }

        public virtual string MainTableHierarchy { get { return this._MainTable + "Hierarchy"; } }

        public virtual string sqlItemFieldList { get { return this._sqlItemFieldList; } }

        public virtual string ColumnID
        {
            get
            {
                string Column = "";
                if (this.MainTable.EndsWith("]")) Column = this.MainTable.Substring(1, this.MainTable.Length - 2) + "ID";
                else Column = this.MainTable + "ID";
                return Column;
            }
        }

        public bool UseHierarchyNodes
        {
            get { return this._UseHierarchyNodes; }
            set { this._UseHierarchyNodes = value; }
        }


        public virtual string ColumnParentID
        {
            get
            {
                return this.MainTable + "ParentID";
            }
        }

        public virtual string ColumnDisplayText
        {
            get
            {
                string ColumnDisplayText = "DisplayText";
                foreach (System.Data.DataColumn C in this._DataTable.Columns)
                {
                    if (C.ColumnName == this._DataTable.TableName + "Name" || C.ColumnName == "DisplayText" || C.ColumnName == this._DataTable.TableName + "Title" || C.ColumnName == "Name")
                    {
                        ColumnDisplayText = C.ColumnName;
                        break;
                    }
                }
                return ColumnDisplayText;
            }
        }

        public virtual string ColumnDescription
        {
            get
            {
                string ColumnDescription = "";
                foreach (System.Data.DataColumn C in this._DataTable.Columns)
                {
                    if (C.ColumnName == this._DataTable.TableName + "Description" || C.ColumnName == "Description")
                    {
                        ColumnDescription = C.ColumnName;
                        break;
                    }
                }
                return ColumnDescription;
            }
        }


        public virtual string ColumnHierarchyDisplayText
        {
            get
            {
                /*
                 * DtAnalysisHierarchy = HierarchyDisplayText
                 * DtCollectionWithHierarchy = DisplayText
                 * */
                string ColumnDisplayText = "HierarchyDisplayText";
                if (this._LookupTableHierarchy.Columns.Contains("HierarchyDisplayText"))
                    return "HierarchyDisplayText";
                foreach (System.Data.DataColumn C in this._LookupTableHierarchy.Columns)
                {
                    if (C.ColumnName == "DisplayText")
                    {
                        ColumnDisplayText = C.ColumnName;
                        break;
                    }
                }
                return ColumnDisplayText;
            }
        }

        public virtual string ColumnDisplayOrder
        {
            get
            {
                string ColumnDisplayText = "DisplayOrder";
                bool ColumnFound = false;
                foreach (System.Data.DataColumn C in this._DataTable.Columns)
                {
                    if (C.ColumnName == "DisplayOrder")
                    {
                        ColumnFound = true;
                        return ColumnDisplayText;
                    }
                }
                foreach (System.Data.DataColumn C in this._DataTable.Columns)
                {
                    if (C.ColumnName == this._DataTable.TableName + "Name" || C.ColumnName == "DisplayText" || C.ColumnName == this._DataTable.TableName + "Title")
                    {
                        ColumnDisplayText = C.ColumnName;
                        ColumnFound = true;
                        break;
                    }
                }
                if (!ColumnFound)
                {
                    foreach (System.Data.DataColumn C in this._DataTable.Columns)
                    {
                        if (C.ColumnName == "Name" || C.ColumnName == "DisplayText" || C.ColumnName == "Title")
                        {
                            ColumnDisplayText = C.ColumnName;
                            break;
                        }
                    }
                }
                return ColumnDisplayText;
            }
        }

        public virtual System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions { get { return null; } }

        public virtual DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns { get { return null; } }
      
        public DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get
            {
                if (this._FormFunctions == null)
                {
                    this._FormFunctions = new DiversityWorkbench.Forms.FormFunctions(this._Form, DiversityWorkbench.Settings.ConnectionString, ref this._toolTip);
                }
                return this._FormFunctions;
            }
        }

        public System.Windows.Forms.ToolTip ToolTip
        {
            get
            {
                return this._toolTip;
            }
        }

        public System.Collections.Generic.List<System.Windows.Forms.Control> ChildControls
        {
            get 
            {
                if (this._ChildControls == null)
                    this._ChildControls = new List<Control>();
                return _ChildControls; 
            }
            set { _ChildControls = value; }
        }

        public bool IncludeIDinTreeview { get { return this._IncludeIDinTreeview; } set { this._IncludeIDinTreeview = value; } }

        public int? ID
        {
            get { return _ID; }
            //set { _ID = value; }
        }


        #endregion

        #region Data handling

        #region toolStrip events for tree view

        public void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            if (this._DataTable.Rows.Count > 0)
            {
                if (this._BindingSource.Position > -1)
                {
                    int? ParentID = int.Parse(this._DataTable.Rows[this._BindingSource.Position][0].ToString());
                    string DisplayText = "";
                    int? ID = this.CreateNewItem(ref DisplayText, ParentID, null);
                    if (ID != null)
                        this.setItem((int)ID);
                }
            }
            this.buildHierarchy();
        }

        public void toolStripButtonCopy_Click(object sender, EventArgs e)
        {
            if (this._DataTable.Rows.Count > 0)
            {
                if (this._BindingSource.Position > -1)
                {
                    int? ParentID = int.Parse(this._DataTable.Rows[this._BindingSource.Position][0].ToString());
                    System.Data.DataRowView RV = (System.Data.DataRowView)this._BindingSource.Current;
                    System.Data.DataRow R = this._DataTable.NewRow();
                    foreach (System.Data.DataColumn C in RV.Row.Table.Columns)
                    {
                        R[C.ColumnName] = RV[C.ColumnName];
                    }
                    string DisplayText = "";
                    int? ID = this.CreateNewItem(ref DisplayText, ParentID, R);
                    if (ID != null)
                        this.setItem((int)ID);
                }
            }
            this.buildHierarchy();
        }

        public void toolStripButtonCopyHierarchy_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._DataTable.Rows.Count > 0)
                {
                    if (this._BindingSource.Position > -1)
                    {
                        int? ParentID; // = int.Parse(this._DataTable.Rows[this._BindingSource.Position][0].ToString());
                        System.Data.DataRowView RV = (System.Data.DataRowView)this._BindingSource.Current;
                        System.Data.DataRow R = this._DataTable.NewRow();
                        System.Data.DataRow ROri = (System.Data.DataRow)this._TreeView.SelectedNode.Tag;
                        // Markus 1.7.24: Anpassung fuer Transaction - Einordung des neuen Datensatzes unter den zu kopierenden
                        if (!ROri[this.ColumnParentID].Equals(System.DBNull.Value))
                            ParentID = int.Parse(ROri[this.ColumnParentID].ToString());
                        else
                        {
                            if (this._MainTable == "[Transaction]")
                            {
                                ParentID = int.Parse(ROri[this.ColumnID].ToString());
                            }
                            else
                                ParentID = null;
                        }
                        foreach (System.Data.DataColumn C in RV.Row.Table.Columns)
                        {
                            if (this._MainTable == "[Transaction]" && C.ColumnName == this.ColumnParentID)
                            {
                                R[C.ColumnName] = RV[this.ColumnID];
                            }
                            else
                                R[C.ColumnName] = RV[C.ColumnName];
                        }
                        string DisplayText = "";
                        int? ID = this.CopyItemIncludingHierarchy(ref DisplayText, ParentID, R);
                        if (ID != null)
                            this.setItem((int)ID);
                        else
                            return;
                    }
                }
                this.buildHierarchy();
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        public void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._TreeView.SelectedNode == null)
                {
                    System.Windows.Forms.MessageBox.Show("Please select an item in the tree view");
                    return;
                }
                if (this._TreeView.SelectedNode.Nodes.Count > 0)
                {
                    System.Windows.Forms.MessageBox.Show("An item with inferior datasets can not be deleted");
                    return;
                }
                int i = 0;
                string SQL = this.SqlSpecimenCount(int.Parse(this._DataTable.Rows[this._BindingSource.Position][0].ToString()));
                if (SQL.Length > 0)
                {
                    Microsoft.Data.SqlClient.SqlConnection c = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, c);
                    c.Open();
                    i = int.Parse(cmd.ExecuteScalar().ToString());
                    c.Close();
                }
                if (i > 0)
                {
                    System.Windows.Forms.MessageBox.Show("This entry is used in " + i.ToString() + " samples and can not be deleted");
                    return;
                }
                if (System.Windows.Forms.MessageBox.Show("Do you want to delete this item?", "Deleting item", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (this._DataTable.Rows[this._BindingSource.Position].RowState != System.Data.DataRowState.Deleted)
                    {
                        bool ok = this.deleteData();
                        // UI updates
                        if (ok)
                        {
                            this.buildHierarchy();
                            int removeId = this._ID ?? -1;
                            if (removeId != -1)
                                this._UserControlQueryList.RemoveListItem(removeId);
                            //this._SplitContainerMain.Panel2.Visible = false;
                            this._ID = null;
                        }
                    }
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonSpecimenList_Click(object sender, EventArgs e)
        {
            if (this._SplitContainerData.Panel2Collapsed)
            {
                this._SplitContainerData.Panel2Collapsed = false;
                this._ToolStripButtonSpecimenList.Image = DiversityCollection.Resource.List;// this._ImageListSpecimenList.Images[1];
                this._ToolStripButtonSpecimenList.BackColor = System.Drawing.SystemColors.Control;
                this._ToolStripButtonSpecimenList.ToolTipText = "Hide specimen list";
            }
            else
            {
                this._SplitContainerData.Panel2Collapsed = true;
                this._ToolStripButtonSpecimenList.Image = DiversityCollection.Resource.ListGrey;// this._ImageListSpecimenList.Images[0];
                this._ToolStripButtonSpecimenList.BackColor = System.Drawing.Color.Yellow;
                this._ToolStripButtonSpecimenList.ToolTipText = "Show specimen list";
            }
        }

        public void toolStripButtonSetParent_Click(object sender, EventArgs e)
        {
            if (this._LookupTable == null) return;
            if (this._DataTable.Rows.Count > 0)
            {
                if (this._BindingSource.Position > -1)
                {
                    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList
                        (this._LookupTable, this.ColumnDisplayText, this.ColumnID, "Superior " + this._MainTable, "Please select the superior " + this._MainTable);
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        int ID;
                        if (int.TryParse(f.SelectedValue, out ID))
                        {
                            if (ID != this._ID && this.NoHierarchyLoop(ID))
                            {
                                bool ok = this.updateParent(this._ID, ID, true);
                            }
                            else
                            {
                                return;
                                //System.Windows.Forms.MessageBox.Show("This would create a loop within the relations of " + this._MainTable);
                            }
                        }
                    }
                    if (this._ID != null)
                        this.setItem((int)this._ID);
                }
            }
            this.buildHierarchy();
        }

        public void toolStripButtonSetParentWithHierarchy_Click(object sender, EventArgs e)
        {
            if (this._LookupTableHierarchy == null) return;
            if (this._DataTable.Rows.Count > 0)
            {
                if (this._BindingSource.Position > -1)
                {
                    string Table = this._MainTable.Replace("[", "").Replace("]", "").ToLower() ;
                    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList
                        (this._LookupTableHierarchy, this.ColumnHierarchyDisplayText, this.ColumnID, "Superior " + Table, "Please select the superior " + Table);
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        int ID;
                        if (int.TryParse(f.SelectedValue, out ID))
                        {
                            System.Data.DataRowView RV = (System.Data.DataRowView)this._BindingSource.Current;
                            if (RV[this.ColumnID].ToString() != ID.ToString() && this.NoHierarchyLoop(ID))
                                RV[this.ColumnParentID] = ID;
                            else
                            {
                                //System.Windows.Forms.MessageBox.Show("You can not set the parent of a dataset to itself");
                                return;
                            }
                        }
                    }
                    if (this._ID != null)
                        this.setItem((int)this._ID);
                }
            }
            this.buildHierarchy();
        }

        protected bool NoHierarchyLoop(int ID)
        {
            bool NoLoop = true;
            bool HasParent = true;
            int ParentID = ID;
            while (HasParent)
            {
                System.Data.DataRow[] rr = this._LookupTable.Select(this.ColumnID + " = " + ParentID.ToString());
                if (rr.Length == 1)
                {
                    if (rr[0][this.ColumnParentID].Equals(System.DBNull.Value))
                        break;
                    else if (int.TryParse(rr[0][this.ColumnParentID].ToString(), out ParentID))
                    {
                        if (ParentID == this._ID || ParentID == ID)
                        {
                            NoLoop = false;
                            System.Windows.Forms.MessageBox.Show("This would create a loop in the hierarchy for the table " + this._MainTable + " through the entry " + rr[0][this.ColumnDisplayText]);
                            break;
                        }
                    }
                }
                else
                    break;
            }
            return NoLoop;
        }

        public void toolStripButtonRemoveParent_Click(object sender, EventArgs e)
        {
            if (this._LookupTable == null) return;
            if (this._DataTable.Rows.Count > 0)
            {
                if (this._BindingSource.Position > -1)
                {
                    bool ok = this.updateParent(this._ID, null, true);
                    // remove from dataset after update 
                    if (ok)
                    {
                        if (this._ID != null)
                            this.setItem((int)this._ID);
                    }
                }
            }
            this.buildHierarchy();
        }

        public void toolStripButtonIncludeID_Click(object sender, EventArgs e)
        {
            this._IncludeIDinTreeview = !this._IncludeIDinTreeview;
            System.Windows.Forms.ToolStripButton B = (System.Windows.Forms.ToolStripButton)sender;
            if (this._IncludeIDinTreeview)
            {
                B.ForeColor = System.Drawing.Color.Black;
                B.BackColor = System.Drawing.SystemColors.Control;
            }
            else
            {
                B.ForeColor = System.Drawing.Color.DarkGray;
                B.BackColor = System.Drawing.Color.Yellow;
            }
            this.IdIsSetByUser(this._IncludeIDinTreeview);
            this.buildHierarchy();
        }

        #endregion

        #region QueryList events

        private void copyItem(object sender, System.EventArgs e)
        {
            try
            {
                if (this._DataTable.Rows.Count > 0)
                {
                    System.Data.DataRow R = this._DataTable.Rows[this._BindingSource.Position];
                    string DisplayText = R[this.ColumnDisplayText].ToString();
                    if (System.Windows.Forms.MessageBox.Show("Do you want to create a copy of\r\n\r\n" + DisplayText, "Create copy?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        string Display = "";
                        this._ID = this.CreateNewItem(ref Display, null, R);
                        this._UserControlQueryList.AddListItem((int)this._ID, Display);
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void deleteItem_Click(object sender, System.EventArgs e)
        {
            if (this._UserControlQueryList.listBoxQueryResult.SelectedIndex == -1)
            {
                System.Windows.Forms.MessageBox.Show("Please select an item from the list");
                return;
            }
            if (this._TreeView?.SelectedNode?.Nodes?.Count > 0)
            {
                System.Windows.Forms.MessageBox.Show("An item with inferior datasets can not be deleted");
                return;
            }
            int i = 0;
            string SQL = this.SqlSpecimenCount(int.Parse(this._DataTable.Rows[this._BindingSource.Position][0].ToString()));
            if (SQL.Length > 0)
            {
                Microsoft.Data.SqlClient.SqlConnection c = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, c);
                c.Open();
                i = int.Parse(cmd.ExecuteScalar().ToString());
                c.Close();
            }
            if (i > 0)
            {
                System.Windows.Forms.MessageBox.Show("This entry is used in " + i.ToString() + " samples and can not be deleted");
                return;
            }
            if (this._BindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._BindingSource.Current;
                try
                {
                    string DisplayTextDel = R[this.ColumnDisplayText].ToString();
                    if (System.Windows.Forms.MessageBox.Show(
                            "Do you want to delete the " + this._MainTable + "\r\n\r\n" + DisplayTextDel,
                            "Delete " + this._MainTable + "?", System.Windows.Forms.MessageBoxButtons.OKCancel,
                            System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        bool ok = this.deleteData();
                        // UI updates
                        if (ok)
                        {
                            this.buildHierarchy();
                            //this._UserControlQueryList.RemoveSelectedListItem();
                            int removeId = this._ID ?? -1;
                            if (removeId != -1)
                                this._UserControlQueryList.RemoveListItem(removeId);
                            //this._SplitContainerMain.Panel2.Visible = false;
                            this._ID = null;
                        }
                    }
                    else
                        return;
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please select an item from the list");
            }

        }

        private void createNewItem(object sender, System.EventArgs e)
        {
            try
            {
                string DisplayText = "";
                this._ID = this.CreateNewItem(ref DisplayText, null, null);
                if (this._ID != null)
                    this._UserControlQueryList.AddListItem((int)this._ID, DisplayText);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void saveItem(object sender, System.EventArgs e)
        {
            this.saveItem();
        }

        #endregion

        /// <summary>
        /// Creating a new item in Tables containing an ...ID as first column, a ...ParentID 
        /// and a display field like DisplayText, ...Name or ...Title
        /// where ... is the name of the table
        /// </summary>
        /// <param name="ParentID">ID of the parent item, otherwise null</param>
        /// <param name="DataTable">The data table</param>
        /// <param name="R">If data should be copied, the row view that should be copied, otherwise null</param>
        /// <returns></returns>
        public int? CreateNewItem(ref string DisplayText, int? ParentID, System.Data.DataRow R)
        {
            bool CreateCopy = false;
            if (R != null) CreateCopy = true;
            int? NewID = null;
            DisplayText = "New item";
            string Table = this._DataTable.TableName;
            string ColumnParentID = "";
            string ColumnID = this._DataTable.Columns[0].ColumnName;
            string SQL = "";
            string Header = "";
            string Original = "";
            string MainTable = this._MainTable;
            if (!MainTable.StartsWith("["))
                MainTable = "[" + MainTable + "]";
            try
            {
                if (this._SetID)
                {
                    SQL = "SELECT MAX(" + ColumnID + ") + 1 FROM [" + this._MainTable + "]";
                    int NextID;
                    if (!int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out NextID))
                        NextID = 1;
                    string Message = "The next free " + ColumnID + " is " + NextID + ". You may change it if necessary";
                    while (NewID == null)
                    {
                        DiversityWorkbench.Forms.FormGetInteger fI = new DiversityWorkbench.Forms.FormGetInteger(NextID, "New " + ColumnID, Message);
                        fI.ShowDialog();
                        if (fI.DialogResult == DialogResult.OK && fI.Integer != null)
                        {
                            SQL = "SELECT COUNT(*) FROM [" + this._MainTable + "] WHERE " + ColumnID + " = " + ((int)fI.Integer).ToString();
                            int NumberOfData = int.Parse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL));
                            if (NumberOfData > 0)
                            {
                                Message = "The value " + ((int)fI.Integer).ToString() + " for " + ColumnID + " is not free. Please change it. The next free " + ColumnID + " is " + NextID + ". You may change it if necessary";
                            }
                            else
                                NewID = (int)fI.Integer;
                        }
                        else if (fI.DialogResult == DialogResult.Cancel)
                            return null;
                    }
                }
                // getting the column names
                ColumnParentID = this.ColumnParentID;
                ColumnID = this.ColumnID;
                string ColumnDisplayText = this.ColumnDisplayText;
                // building the sql statement
                string SqlInsert = "INSERT INTO [" + Table + "] (";
                string SqlValues = "VALUES (";
                System.Collections.Generic.Dictionary<string, string> Dict = this.AdditionalNotNullColumnsForNewItem(ParentID);
                if (Dict.Count == 0 && this._MainTable == "CollectionTask")
                    return null;
                if (Table == "CollectionTask")
                {
                    DisplayText = "";
                }
                else if (Table == "Task" && !CreateCopy)
                {
                    string Type = "";
                    //if (CreateCopy)
                    //{
                    //    Type = R["Type"].ToString();
                    //}
                    //else
                    //{
                    //}
                    Header = "Please select the type for the new " + this._MainTable;
                    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(DiversityCollection.LookupTable.DtTaskTypes, Header, true, true);
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        Type = f.String;
                        SqlInsert += "Type, ";
                        SqlValues += "'" + Type + "', ";
                        SQL = "SELECT DISTINCT DisplayText FROM Task WHERE(TaskParentID IS NULL) AND(Type = N'" + f.String + "')";
                        System.Data.DataTable dtTask = new System.Data.DataTable();
                        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTask);
                        if (dtTask.Rows.Count == 1)
                            DisplayText = dtTask.Rows[0][0].ToString();
                        else if (dtTask.Rows.Count == 0)
                        {
                            Header = "Please enter a name for the new " + this._MainTable;
                            Original = "";
                            if (CreateCopy)
                            {
                                Header = "Please enter a name for the copy of " + R[ColumnDisplayText].ToString();
                                Original = "Copy of " + R[ColumnDisplayText].ToString();
                            }
                            DiversityWorkbench.Forms.FormGetString fDisplay = new DiversityWorkbench.Forms.FormGetString("New " + this._MainTable, Header, Type);
                            fDisplay.ShowDialog();
                            if (fDisplay.DialogResult == DialogResult.OK)
                                DisplayText = fDisplay.String;
                            else
                                return null;
                        }
                        else
                        {
                            Header = "Please enter a name for the new " + this._MainTable;
                            f = new DiversityWorkbench.Forms.FormGetStringFromList(dtTask, Header, false);
                            f.ShowDialog();
                            if (f.DialogResult == DialogResult.OK && f.String.Length > 0)
                            {
                                DisplayText = f.String;
                            }
                            else
                                return null;
                        }
                    }
                    else
                        return null;
                    if (DiversityCollection.Task.DefaultColumnsForNewTasks(Type).Count > 0)
                    {
                        foreach(System.Collections.Generic.KeyValuePair<string, string> KV in DiversityCollection.Task.DefaultColumnsForNewTasks(Type))
                        {
                            if (SqlInsert.IndexOf(", " + KV.Key) == -1)
                            {
                                SqlInsert += KV.Key + ", ";
                                if (KV.Key.ToLower() == "displaytext" && DisplayText.Length > 0)
                                    SqlValues += "'" + DisplayText + "', ";
                                else
                                    SqlValues += "'" + KV.Value + "', ";
                            }
                        }
                    }
                }
                else if (Dict.ContainsKey(ColumnDisplayText))
                {
                    DisplayText = Dict[ColumnDisplayText];
                }
                else
                {
                    Header = "Please enter a name for the new " + this._MainTable;
                    Original = "";
                    if (CreateCopy)
                    {
                        Header = "Please enter a name for the copy of " + R[ColumnDisplayText].ToString();
                        Original = "Copy of " + R[ColumnDisplayText].ToString();
                    }
                    DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New " + this._MainTable, Header, Original);
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                        DisplayText = f.String;
                    else
                        return null;
                }
                if (this._SetID)
                {
                    SqlInsert += ColumnID + ", ";
                    SqlValues += ((int)NewID).ToString() + ", ";
                }
                if (Dict.Count > 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Dict)
                    {
                        if (KV.Key == ColumnDisplayText) continue;
                        SqlInsert += KV.Key + ", ";
                        SqlValues += KV.Value + ", ";
                    }
                }
                if (ParentID != null)
                {
                    SqlInsert += ColumnParentID + ", ";
                    SqlValues += ParentID.ToString() + ", ";
                    if (this.GetType() == typeof(Collection))
                    {
                        string Type = "";
                        string SqlType = "SELECT Type FROM Collection WHERE CollectionID = " + ParentID.ToString();
                        using (Microsoft.Data.SqlClient.SqlConnection con =
                               new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString))
                        {
                            con.Open();
                            Type = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlType, con);
                            con.Close();
                        }

                        Collection collection = (Collection)this;
                        Type = collection.ChildType(Type);
                        if (Type.Length > 0)
                        {
                            SqlInsert += "[Type], ";
                            SqlValues += "'" + Type + "', ";
                        }
                    }
                }

                if (R == null)
                {
                    if (SqlInsert.ToLower().IndexOf(" " + ColumnDisplayText.ToLower() + ",") == -1)
                    {
                        SqlInsert += ColumnDisplayText;
                        SqlValues += "'" + DisplayText.Replace("'", "''") + "'";
                    }
                    else if (SqlInsert.Trim().EndsWith(","))
                    {
                        SqlInsert = SqlInsert.Trim();
                        SqlInsert = SqlInsert.TrimEnd(new char[] { ',' });
                        SqlValues = SqlValues.Trim();
                        SqlValues = SqlValues.TrimEnd(new char[] { ',' });
                    }

                    SqlInsert += ")";
                    SqlValues += ")";
                }
                else
                {
                    foreach (System.Data.DataColumn C in R.Table.Columns)
                    {
                        if (C.ColumnName != ColumnID &&
                            C.ColumnName != ColumnParentID &&
                            !C.ColumnName.StartsWith("Log") &&
                            !R[C.ColumnName].Equals(System.DBNull.Value) &&
                            !Dict.ContainsKey(C.ColumnName))
                        {
                            bool OK = true;
                            SqlInsert += C.ColumnName + ", ";
                            if (C.DataType == typeof(System.DateTime))
                            {
                                System.DateTime D;
                                OK = System.DateTime.TryParse(R[C.ColumnName].ToString(), out D);
                                if (OK)
                                    SqlValues += "CONVERT(DateTime, '" + D.Year.ToString() + "-" + D.Month.ToString() + "-" + D.Day.ToString() + " " +
                                                 D.Hour.ToString() + ":" + D.Minute.ToString() + ":" + D.Second.ToString() + "." + D.Millisecond.ToString() + "', 121)";
                                else
                                    SqlValues += "NULL";
                            }
                            else if (C.DataType == typeof(System.Boolean))
                            {
                                System.Boolean B;
                                OK = System.Boolean.TryParse(R[C.ColumnName].ToString(), out B);
                                if (OK)
                                {
                                    if (B)
                                        SqlValues += "1";
                                    else SqlValues += "0";
                                }
                                else SqlValues += "NULL";
                            }
                            else
                            {
                                if (C.DataType == typeof(System.String)
                                    || C.DataType == typeof(System.DateTime)) SqlValues += "'";
                                if (C.ColumnName == ColumnDisplayText)
                                {
                                    SqlValues += DisplayText.Replace("'", "''");
                                }
                                else
                                    SqlValues += R[C.ColumnName].ToString();
                                if (C.DataType == typeof(System.String)
                                    || C.DataType == typeof(System.DateTime)) SqlValues += "'";
                            }
                            SqlValues += ", ";
                        }
                    }
                    SqlValues = SqlValues.Substring(0, SqlValues.Length - 2) + ") ";
                    SqlInsert = SqlInsert.Substring(0, SqlInsert.Length - 2) + ") ";
                }
                if (this._SetID)
                {
                    this._ID = (int)NewID;
                    SQL = "SET IDENTITY_INSERT [" + this._MainTable + "] ON; " + SqlInsert + SqlValues + "; SET IDENTITY_INSERT [" + this._MainTable + "] OFF;";
                }
                else
                {
                    SQL = SqlInsert + SqlValues + "; SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];";
                }

                // writing the data
                using (SqlConnection c = new SqlConnection(DiversityWorkbench.Settings.ConnectionString))
                {
                    c.Open();
                    using (SqlTransaction transaction = c.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(SQL, c, transaction))
                            {
                                if (!this._SetID)
                                {
                                    int i;
                                    object scalarResult = cmd.ExecuteScalar();
                                    if (scalarResult != null && int.TryParse(scalarResult.ToString(), out i))
                                    {
                                        this._ID = i;
                                    }
                                    else
                                    {
                                        string maxQuery = "SELECT MAX(" + this.ColumnID + ") FROM " +
                                                          this._MainTable;
                                        string result =
                                            DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(maxQuery);
                                        if (int.TryParse(result, out i))
                                        {
                                            this._ID = i;
                                        }
                                    }

                                    this.insertDependentData(c, transaction, this._ID, ParentID);
                                    
                                    if (this._MainTable == "CollectionTask")
                                    {
                                        // if the task has depending tasks include these in the insert
                                        if (Dict.ContainsKey("TaskID"))
                                        {
                                            // check for depending tasks
                                            string SqlDepend = "SELECT TaskID FROM Task WHERE TaskParentID = " +
                                                               Dict["TaskID"];
                                            System.Data.DataTable dtDepend = new System.Data.DataTable();
                                            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SqlDepend,
                                                ref dtDepend);
                                            foreach (System.Data.DataRow rDepend in dtDepend.Rows)
                                            {
                                                cmd.CommandText =
                                                    "INSERT INTO CollectionTask (TaskID, CollectionTaskParentID) VALUES(" +
                                                    rDepend[0].ToString() + ", " + this._ID.ToString() + ")";

                                                cmd.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    bool OK = true;
                                    string ExMess = "";
                                    try
                                    {
                                        cmd.ExecuteNonQuery();
                                    }
                                    catch (System.Exception ex)
                                    {
                                        OK = false;
                                        ExMess = ex.Message;
                                    }

                                    if (!OK && this._SetID)
                                    {
                        System.Collections.Generic.Dictionary<DiversityWorkbench.Forms.FormFunctions.Permission, bool> Permissions = DiversityWorkbench.Forms.FormFunctions.TablePermissions(this.MainTable);
                        if (Permissions.ContainsKey(DiversityWorkbench.Forms.FormFunctions.Permission.INSERT) &&
                                            Permissions[DiversityWorkbench.Forms.FormFunctions.Permission.INSERT])
                                        {
                                            System.Windows.Forms.MessageBox.Show(
                                                "You do not have the permission to set the ID. Please try without setting the ID",
                                                "Missing permissions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            transaction.Rollback();
                                            return null;
                                        }
                                    }
                                }
                            }

                            // Commit the transaction if all operations succeed
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            // Roll back the transaction if any exception occurs
                            transaction.Rollback();
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            DiversityWorkbench.ExceptionHandling.ShowErrorMessage(
                                "The dataset could not be created. An error occurred: \r\n" + ex.Message);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                DiversityWorkbench.ExceptionHandling.ShowErrorMessage(
                    "The dataset could not be created. An error occurred: \r\n" + ex.Message);
            }
            return (int)this._ID;
        }

        private bool _SetID = false;
        public virtual void IdIsSetByUser(bool IdIsSet) { this._SetID = IdIsSet; }

        /// <summary>
        /// Creating a new item in Tables containing an ...ID as first column, a ...ParentID 
        /// and a display field like DisplayText, ...Name or ...Title
        /// where ... is the name of the table
        /// </summary>
        /// <param name="ParentID">ID of the parent item, otherwise null</param>
        /// <param name="DataTable">The data table</param>
        /// <param name="R">If data should be copied, the row view that should be copied, otherwise null</param>
        /// <returns></returns>
        public int? CopyItemIncludingHierarchy(ref string DisplayText, int? ParentID, System.Data.DataRow R)
        {
            //bool CreateCopy = true;
            int? ID = this.CreateNewItem(ref DisplayText, ParentID, R);
            if (ID == null)
                return ID;
            int OriginalID;
            if (!int.TryParse(R[this.ColumnID].ToString(), out OriginalID))
                return ID;
            this.CopyChildItems(OriginalID, (int)ID);
            return (int)this._ID;
        }

        /// <summary>
        /// Copy all childs of a dataset identified by the original ID
        /// </summary>
        /// <param name="OriginalID">ID of the dataset of which the children should be copied</param>
        /// <param name="ParentID">ID of the dataset which should be the parent of the copied children</param>
        /// <returns></returns>
        public int? CopyChildItems(int OriginalID, int ParentID)
        {
            int ID = 0;
            foreach (System.Data.DataRow R in this._DataTable.Rows)
            {
                if (!R[this.ColumnParentID].Equals(System.DBNull.Value) &&
                    R[this.ColumnParentID].ToString() == OriginalID.ToString())
                {
                    int NewChildID = this.CopyItem(ParentID, R);
                    int OriginalChildID = int.Parse(R[this.ColumnID].ToString());
                    this.CopyChildItems(OriginalChildID, NewChildID);
                }
            }
            return ID;
        }

        public int CopyItem(int ParentID, System.Data.DataRow R)
        {
            int ID = -1;
            string Table = this._DataTable.TableName;
            string ColumnParentID = "";
            string ColumnID = this._DataTable.Columns[0].ColumnName;
            try
            {
                // getting the column names
                ColumnParentID = this.ColumnParentID;
                ColumnID = this.ColumnID;
                string ColumnDisplayText = this.ColumnDisplayText;
                // building the sql statement
                string SqlInsert = "INSERT INTO [" + Table + "] (";
                string SqlValues = "VALUES (";
                System.Collections.Generic.Dictionary<string, string> Dict = this.AdditionalNotNullColumnsForNewItem(ParentID);
                if (Dict.Count > 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Dict)
                    {
                        SqlInsert += KV.Key + ", ";
                        SqlValues += KV.Value + ", ";
                    }
                }
                if (ParentID != null)
                {
                    SqlInsert += ColumnParentID + ", ";
                    SqlValues += ParentID.ToString() + ", ";
                }
                {
                    foreach (System.Data.DataColumn C in R.Table.Columns)
                    {
                        if (C.ColumnName != ColumnID &&
                            C.ColumnName != ColumnParentID &&
                            !C.ColumnName.StartsWith("Log") &&
                            !R[C.ColumnName].Equals(System.DBNull.Value) &&
                            !Dict.ContainsKey(C.ColumnName))
                        {
                            bool OK = true;
                            SqlInsert += C.ColumnName + ", ";
                            if (C.DataType == typeof(System.DateTime))
                            {
                                System.DateTime D;
                                OK = System.DateTime.TryParse(R[C.ColumnName].ToString(), out D);
                                if (OK)
                                    SqlValues += "CONVERT(DateTime, '" + D.Year.ToString() + "-" + D.Month.ToString() + "-" + D.Day.ToString() + " " +
                                                 D.Hour.ToString() + ":" + D.Minute.ToString() + ":" + D.Second.ToString() + "." + D.Millisecond.ToString() + "', 121)";
                                else
                                    SqlValues += "NULL";
                            }
                            else
                            {
                                if (C.DataType == typeof(System.String)
                                    || C.DataType == typeof(System.DateTime)) SqlValues += "'";
                                SqlValues += R[C.ColumnName].ToString();
                                if (C.DataType == typeof(System.String)
                                    || C.DataType == typeof(System.DateTime)) SqlValues += "'";
                            }
                            SqlValues += ", ";
                        }
                    }
                    SqlValues = SqlValues.Substring(0, SqlValues.Length - 2) + ") ";
                    SqlInsert = SqlInsert.Substring(0, SqlInsert.Length - 2) + ") ";
                }
                string SQL = SqlInsert + SqlValues + "; SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];";
                // writing the data
                using (SqlConnection c = new SqlConnection(DiversityWorkbench.Settings.ConnectionString))
                {
                    c.Open();
                    using (SqlTransaction transaction = c.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(SQL, c, transaction))
                            {
                                ID = int.Parse(cmd.ExecuteScalar().ToString());
                                this.insertDependentData(c, transaction, ID, ParentID);
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            // Roll back the transaction if any exception occurs
                            transaction.Rollback();
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            DiversityWorkbench.ExceptionHandling.ShowErrorMessage(
                                "The dataset could not be created. An error occurred: \r\n" + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return ID;
        }

        public virtual System.Collections.Generic.Dictionary<string, string> AdditionalNotNullColumnsForNewItem(int? ParentID)
        {
            System.Collections.Generic.Dictionary<string, string> List = new Dictionary<string, string>();
            return List;
        }

        public virtual void saveTables()
        {
            this._DataTable.Rows[0].BeginEdit();
            this._DataTable.Rows[0].EndEdit();
            this.FormFunctions.updateTable(this._DataSet, this._DataTable.TableName, this._SqlDataAdapter, this._BindingSource);
            this.saveDependentTables();
           
        }
        
        public void saveItem(bool BuildHierarchy = true)
        {
            try
            {
                if (this._DataSet == null || this._DataTable == null || this._BindingSource == null) 
                    return;
                
                this._BindingSource.EndEdit();
                if ((this._DataTable.Rows.Count > 0) && this._DataSet.HasChanges())
                {
                    saveTables();
                    if (BuildHierarchy)
                    {
                        this.buildHierarchy();
                        // zu #205
                        if (this._DataTable.TableName == "Collection" && this._ID != null)
                        {
                            DiversityCollection.Forms.FormCollection f = (DiversityCollection.Forms.FormCollection)this._Form;
                            f.CollectionLocation.BuildHierarchy((int)this._ID, true);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public virtual void saveDependentTables() { }

        protected void setDependentTable(int ID) 
        {
            this.saveDependentTables();
            this.fillDependentTables(ID);
        }

        public virtual bool deleteDependentData(int ID) { return true; }

        public virtual void insertDependentData(SqlConnection connection, SqlTransaction transaction, int? itemId, int? parentId) {}

        public virtual bool updateParent(int? itemId, int? parentId, bool setCurrent)
        {
            if (!setCurrent)
                return true;
            System.Data.DataRowView RV = (System.Data.DataRowView)this._BindingSource.Current;
            RV[this.ColumnParentID] = parentId.HasValue ? parentId.Value : System.DBNull.Value;
            
            return true;
        }
        public virtual bool deleteData()
        {
            if (!this.deleteDependentData((int)this._ID))
            {
                return false;
            }

            string Message = "";
            this._BindingSource.RemoveCurrent();
            if (this.FormFunctions.updateTable(this._DataSet, this._DataTable.TableName, this._SqlDataAdapter,
                    this._BindingSource, ref Message))
            {
                return true;
            }
            else
            {
                if (Message.IndexOf("FK_CollectionManager_Collection") > -1)
                    Message =
                        "Please remove this collection from all collection managers first\r\n(Transaction - Collection manager)\r\n\r\n" +
                        Message;
                System.Windows.Forms.MessageBox.Show(Message);
                return false;
            }
        }

        private void undoChangesInItem_Click(object sender, System.EventArgs e)
        {
            this._ID = int.Parse(this._DataTable.Rows[0][0].ToString());
            this._DataSet.Clear();
            this.setItem((int)this._ID);
        }
        
        public void fillItem(int ID)
        {
            // bool OK = false;
            string SQL = "";
            try
            {
                this._ID = ID;
                this._DataSet.Clear();
                if (this._MainTableContainsHierarchy)
                {
                    SQL = "SELECT " + this.sqlItemFieldList + " FROM dbo." + this._MainTable + " WHERE 2 = 1 ";
                    // 23.11.22 - Markus - search for bug
                    // search for bug - usercontrolmodulrelatedentry is not working due to no change in data
                    //SQL = "SELECT " + this._sqlItemFieldList + " FROM dbo." + this._MainTable + " WHERE " + this.ColumnID + " = " + ID.ToString();
                    if (this._SqlDataAdapter == null)
                    {
                        this._SqlDataAdapter = new Microsoft.Data.SqlClient.SqlDataAdapter(this.SqlItem(ID), DiversityWorkbench.Settings.ConnectionString);
                        //if (this._DataTable.TableName == "CollectionTask")
                        //    this.FormFunctions.initSqlAdapter(ref this._SqlDataAdapter, SQL, this._DataTable, System.Data.ConflictOption.OverwriteChanges);
                        //else
                        this.FormFunctions.initSqlAdapter(ref this._SqlDataAdapter, SQL, this._DataTable);
                    }
                    SQL = "SELECT " + this.sqlItemFieldList + " FROM dbo." + this.MainTableHierarchy + "(" + ID.ToString() + ") " + this.OrderClause;
                    //this._SqlDataAdapter.SelectCommand.CommandText = SQL;
                    //this._SqlDataAdapter.Fill(this._DataTable);
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(this._DataTable);
                    this.fillDependentTables(ID);
                    //this.setUserControlSourceFixing();
                }
                else
                {
                    SQL = "SELECT " + this.sqlItemFieldList + " FROM dbo." + this._MainTable + " WHERE " + this.ColumnID + " = " + ID.ToString() + this.OrderClause;
                    if (this._SqlDataAdapter == null)
                    {
                        this._SqlDataAdapter = new Microsoft.Data.SqlClient.SqlDataAdapter(this.SqlItem(ID), DiversityWorkbench.Settings.ConnectionString);
                    }
                    this.FormFunctions.initSqlAdapter(ref this._SqlDataAdapter, SQL, this._DataTable);
                    this.fillDependentTables(ID);
                }
                string x = this._Form.GetType().ToString();
                try
                {
                    switch (x)
                    {
                        case "DiversityCollection.FormAnalysis":
                            Forms.FormAnalysis fA = (Forms.FormAnalysis)this._Form;
                            //fA.setFormControls();
                            break;
                        //case "DiversityCollection.Forms.FormTransaction":
                        //    break;
                        case "DiversityCollection.FormHierarchicalEntity":
                            FormHierarchicalEntity f = (FormHierarchicalEntity)this._Form;
                            f.setFormControls();
                            break;
                        default:
                            if (this._Form is FormHierarchicalEntity ff)
                                ff.setFormControls();
                            //FormHierarchicalEntity ff = (FormHierarchicalEntity)this._Form;
                            //ff.setFormControls();
                            break;
                    }
                }
                catch (System.Exception ex)
                {
                }

                //OK = true;
            }
            catch (Microsoft.Data.SqlClient.SqlException SclException)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(SclException, SQL);
                DiversityWorkbench.ExceptionHandling.ShowErrorMessage("A database error occurred: " + SclException);
                //OK = false;

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
                DiversityWorkbench.ExceptionHandling.ShowErrorMessage("An error occurred: " + ex);
                //OK = false;
            }
            //return OK;
        }

        //protected virtual void setUserControlSourceFixing() { }

        private string _OrderClause;

        private string OrderClause
        {
            get
            {
                if (this._OrderClause == null)
                {
                    this._OrderClause = "";
                    foreach (string s in this._OrderColumns)
                    {
                        if (_OrderClause.Length > 0) 
                            this._OrderClause += ", ";
                        this._OrderClause += s;
                    }
                    if (this._OrderClause.Length > 0)
                        this._OrderClause = " ORDER BY " + this._OrderClause;
                }
                return this._OrderClause;
            }
        }

        public void setItem(int ID)
        {
            try
            {
                this._ID = ID;
                // #205
                if (this._DataTable.TableName == "Collection")
                {
                    DiversityCollection.Forms.FormCollection f = (DiversityCollection.Forms.FormCollection)this._Form;
                    f.CollectionLocation.ID = ID;
                }

                this.saveItem(!this._MainTableContainsHierarchy);
                this.fillItem(ID);
                this._TreeView.Nodes.Clear();
                if (this._MainTableContainsHierarchy && this._hierarchyDisplayState != HierarchyDisplayState.Hide)
                {
                    this.buildHierarchy();
                    this._TreeView.SelectedNode = this.getNodeByID(ID);
                    this.markSelectedUnitNode(ID);
                }
                else
                {
                    this._BindingSource.Position = this.CurrentPosition(ID);
                }
                if (this._DataTable.Rows.Count > 0)
                {
                    this._SplitContainerMain.Panel2.Visible = true;
                    if (this._SplitContainerData != null && !this._SplitContainerData.Panel2Collapsed)
                        this.setSpecimenList(ID);
                }
                else
                    this._SplitContainerMain.Panel2.Visible = false;
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public virtual void fillDependentTables(int ID) { }

        #endregion

        #region Hierarchy

        #region Tree view

        public void initTreeView()
        {
            try
            {
                this._TreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
                this._TreeView.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView_DragOver);
                this._TreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView_DragDrop);
                this._TreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag);
                this._TreeView.AllowDrop = true;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        protected System.Windows.Forms.TreeNode _CurrentNode = null;

        public virtual void buildHierarchy()
        {
            try
            {
                this._TreeView.SuspendLayout();
                this._TreeView.Nodes.Clear();
                this._CurrentNode = null;

                if (this._hierarchyDisplayState == HierarchyDisplayState.Hide)
                    return;


                // getting the column names
                string ColumnID = this.ColumnID;
                string ColumnParentID = this.ColumnParentID;
                string ColumnDisplayText = this.ColumnDisplayText;
                if (this._DataTable.Rows.Count > 0)
                {
                    System.Windows.Forms.TreeNode rootNode;
                    if (this._DataTable.Rows[0].RowState == DataRowState.Deleted)
                    {
                        rootNode = null;
                    }
                    else
                    {
                        if (this.UseHierarchyNodes)
                            rootNode = new DiversityCollection.HierarchyNode(this._DataTable.Rows[0]);
                        else
                            rootNode = new System.Windows.Forms.TreeNode(this._DataTable.Rows[0][ColumnDisplayText]
                                .ToString());
                    }

                    System.Data.DataRow[] rrAll = this._DataTable.Select("", this.ColumnDisplayOrder + ", " + this.ColumnDisplayText);
                    foreach (System.Data.DataRow r in rrAll)// this._DataTable.Rows)
                    {
                        try
                        {
                            if (r.RowState != System.Data.DataRowState.Deleted)
                            {
                                if (!r[this.ColumnParentID].Equals(System.DBNull.Value))
                                {
                                    int ID = System.Int32.Parse(r[this.ColumnParentID].ToString());
                                    System.Data.DataRow[] rr = this._DataTable.Select(this.ColumnID + " = " + ID.ToString());
                                    if (rr.Length == 0)
                                        r[this.ColumnParentID] = System.DBNull.Value;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                    // prepare SubstrateID: set SubstrateID at max DisplayOrder to NULL if no starting point can be found
                    System.Data.DataRow[] rrS = this._DataTable.Select(this.ColumnParentID + " IS NULL", this.ColumnDisplayOrder + ", " + this.ColumnDisplayText, System.Data.DataViewRowState.CurrentRows);
                    if (rrS.Length == 0)
                    {
                        System.Data.DataRow[] rrD = this._DataTable.Select("", this.ColumnDisplayOrder + " DESC", System.Data.DataViewRowState.CurrentRows);
                        if (rrD.Length > 0)
                        {
                            System.Data.DataRow rrDMax = rrD[0];
                            rrDMax[this.ColumnParentID] = System.DBNull.Value;
                        }
                    }
                    string Restriction = "";
                    System.Drawing.Font F = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    if (this._hierarchyDisplayState == HierarchyDisplayState.Children || this._hierarchyDisplayState == HierarchyDisplayState.Parents)
                    {
                        System.Collections.Generic.Stack<System.Windows.Forms.TreeNode> ParentNodes = new Stack<TreeNode>();
                        Restriction = this.HierarchyRestriction();// this.ColumnID + " = " + ID.ToString();
                        // getting the current node
                        int? ParentID = null;
                        System.Data.DataRow[] rrCurrent = this._DataTable.Select(Restriction, this.ColumnDisplayOrder + ", " + this.ColumnDisplayText, System.Data.DataViewRowState.CurrentRows);
                        if (rrCurrent.Length == 1)
                        {
                            System.Windows.Forms.TreeNode N = this.TreeNode(rrCurrent[0], F);
                            //string NodeTitle = this.HierarchyNodeText(rrCurrent[0]);// = r[this.ColumnDisplayText].ToString();
                            //if (this._IncludeIDinTreeview) NodeTitle += "     [" + rrCurrent[0][this.ColumnID].ToString() + "]";
                            ////System.Windows.Forms.TreeNode N = new System.Windows.Forms.TreeNode(NodeTitle);
                            //System.Windows.Forms.TreeNode N = this.TreeNode(rrCurrent[0], F);// new DiversityCollection.HierarchyNode(rrCurrent[0]);
                            //N.Text = NodeTitle;
                            //N.Tag = rrCurrent[0];
                            //N.NodeFont = F;
                            ParentNodes.Push(N);
                            if (!rrCurrent[0][this.ColumnParentID].Equals(System.DBNull.Value))
                                ParentID = int.Parse(rrCurrent[0][this.ColumnParentID].ToString());
                            while (ParentID != null)
                            {
                                System.Data.DataRow[] rrParent = this._DataTable.Select(this.ColumnID + " = " + ParentID.ToString(), this.ColumnDisplayOrder + ", " + this.ColumnDisplayText, System.Data.DataViewRowState.CurrentRows);
                                System.Windows.Forms.TreeNode ParentNode = this.TreeNode(rrParent[0], F);
                                //NodeTitle = this.HierarchyNodeText(rrParent[0]);
                                //if (this._IncludeIDinTreeview) NodeTitle += "     [" + rrParent[0][this.ColumnID].ToString() + "]";
                                //// System.Windows.Forms.TreeNode ParentNode = new System.Windows.Forms.TreeNode(NodeTitle);
                                //System.Windows.Forms.TreeNode ParentNode = new DiversityCollection.HierarchyNode(rrParent[0]);
                                //ParentNode.Text = NodeTitle;
                                //ParentNode.NodeFont = F;
                                ParentNodes.Push(ParentNode);
                                int PreviousParentID = (int)ParentID;
                                if (!rrParent[0][this.ColumnParentID].Equals(System.DBNull.Value))
                                {
                                    ParentID = int.Parse(rrParent[0][this.ColumnParentID].ToString());
                                }
                                else
                                    ParentID = null;
                            }
                            while (ParentNodes.Count > 0)
                            {
                                if (this._TreeView.Nodes.Count == 0)
                                    this._TreeView.Nodes.Add(ParentNodes.Peek());
                                else
                                {
                                    System.Windows.Forms.TreeNode Nparent = ParentNodes.Pop();
                                    if (ParentNodes.Count > 0)
                                    {
                                        Nparent.Nodes.Add(ParentNodes.Peek());
                                        if (ParentNodes.Count == 1)
                                        {
                                            System.Data.DataRow rNode = (System.Data.DataRow)ParentNodes.Peek().Tag;
                                            if (this.GetType() == typeof(Collection))
                                            {
                                                Collection collection = (Collection)this;
                                                if (collection.HierarchyIncludingParts)
                                                {
                                                    collection.addHierarchyPartNodes(System.Int32.Parse(rNode[this.ColumnID].ToString()), Nparent.FirstNode);
                                                }
                                            }
                                            if (_hierarchyDisplayState == HierarchyDisplayState.Children)
                                                this.addHierarchyNodes(System.Int32.Parse(rNode[this.ColumnID].ToString()), ParentNodes.Pop(), this._hierarchyDisplayState == HierarchyDisplayState.Children);
                                            else if (_hierarchyDisplayState == HierarchyDisplayState.Parents && this._MainTable == "Collection")
                                                this.addHierarchyNodes(System.Int32.Parse(rNode[this.ColumnID].ToString()), ParentNodes.Pop(), this._hierarchyDisplayState == HierarchyDisplayState.Children);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (System.Data.DataRow r in rrAll)// this._DataTable.Rows)
                        {
                            try
                            {
                                if (r.RowState != System.Data.DataRowState.Deleted)
                                {
                                    if (r[this.ColumnParentID].Equals(System.DBNull.Value))
                                    {
                                        System.Windows.Forms.TreeNode Node = this.TreeNode(r, F, this.UseHierarchyNodes);
                                        //string NodeText = this.HierarchyNodeText(r);// = r[this.ColumnDisplayText].ToString();
                                        //if (this._IncludeIDinTreeview) NodeText += "     [" + r[this.ColumnID].ToString() + "]";
                                        //System.Windows.Forms.TreeNode Node;
                                        //if (this.UseHierarchyNodes)
                                        //{
                                        //    Node = new DiversityCollection.HierarchyNode(r);
                                        //    int TextLength = Node.Text.Length / 3;
                                        //    string Trailer = " ";
                                        //    while (TextLength > 0) { Trailer += " "; TextLength--; }
                                        //    Node.Text += Trailer;
                                        //}
                                        //else Node = new System.Windows.Forms.TreeNode(NodeText);
                                        //Node.Tag = r;
                                        //Node.NodeFont = F;
                                        this._TreeView.Nodes.Add(Node);
                                        this.addHierarchyNodes(System.Int32.Parse(r[this.ColumnID].ToString()), Node);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            }
                        }
                    }
                    this.markHierarchyNodes();
                    this._TreeView.ExpandAll();
                }
                if (_CurrentNode != null)
                    _CurrentNode.EnsureVisible();
                this._TreeView.ResumeLayout();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        private System.Windows.Forms.TreeNode TreeNode(System.Data.DataRow R, System.Drawing.Font F = null, bool AddTextTrailer = false)
        {
            if (F == null)
                F = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            string NodeText = this.HierarchyNodeText(R);
            if (this._IncludeIDinTreeview) NodeText += "     [" + R[this.ColumnID].ToString() + "]";
            if (AddTextTrailer)
            {
                string Trailer = " ";
                int TextLength = NodeText.Length / 3;
                while (TextLength > 0) { Trailer += " "; TextLength--; }
                NodeText += Trailer;
            }
            System.Windows.Forms.TreeNode N = new DiversityCollection.HierarchyNode(R);
            N.Text = NodeText;
            N.Tag = R;
            N.NodeFont = F;
            return N;
        }

        protected virtual string HierarchyRestriction()
        {
            return this.ColumnID + " = " + ID.ToString();
        }

        public virtual void markHierarchyNodes()
        {
        }

        protected virtual void addHierarchyNodes(int ParentID, System.Windows.Forms.TreeNode pNode, bool IncludeAllChildren = true)
        {
            System.Data.DataRow[] rr = this._DataTable.Select(this.ColumnParentID + " = " + ParentID.ToString());
            foreach (System.Data.DataRow r in rr)
            {
                int ID = System.Int32.Parse(r[this.ColumnID].ToString());
                string NodeText = this.HierarchyNodeText(r); // r[this.ColumnDisplayText].ToString();
                System.Windows.Forms.TreeNode Node = new System.Windows.Forms.TreeNode(NodeText);
                System.Drawing.Font F = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Node.NodeFont = F;
                Node.Tag = r;
                pNode.Nodes.Add(Node);
                if (IncludeAllChildren)
                    this.addHierarchyNodes(ID, Node);
                else
                    Node.ForeColor = System.Drawing.Color.Gray;
                if (ID == this.ID)
                    this._CurrentNode = Node;
            }
        }

        public virtual string HierarchyNodeText(System.Data.DataRow R)
        {
            string NodeText = "";
            try
            {
                NodeText = R[this.ColumnDisplayText].ToString();
                if (this.ColumnDescription.Length > 0)
                {
                    if (!R[this.ColumnDescription].Equals(System.DBNull.Value) && R[this.ColumnDescription].ToString().Length > 0 && NodeText.Length < 60)
                    {
                        string Description = R[this.ColumnDescription].ToString();
                        if (NodeText.Trim() != Description.Trim())
                        {
                            if (NodeText.Length + Description.Length > 60)
                                Description = Description.Substring(0, 60 - NodeText.Length) + "...";
                            NodeText += " (" + Description + ")";
                        }
                    }
                }
                if (this._IncludeIDinTreeview)
                    NodeText += "     [" + R[this.ColumnID].ToString() + "]";
                if (R[this.ColumnParentID].Equals(System.DBNull.Value))
                {
                    string Spacer = " ";
                    for (int i = 0; i < (int)NodeText.Length / 2; i++)
                        Spacer += " ";
                    NodeText += Spacer;
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return NodeText;
        }

        public virtual void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (this._TreeView.SelectedNode != null && this._TreeView.SelectedNode.Tag != null)
                {
                    if (this._TreeView.SelectedNode.Tag.GetType().BaseType != typeof(System.Data.DataRow))
                        return;
                    this.markSelectedUnitNode(this._TreeView.SelectedNode);
                    System.Data.DataRow rn = (System.Data.DataRow)this._TreeView.SelectedNode.Tag;
                    int ID;// = System.Int32.Parse(rn[this.ColumnID].ToString());
                    if (!int.TryParse(rn[this.ColumnID].ToString(), out ID))
                        return;
                    this._ID = ID;
                    this.setDependentTable(ID);
                    this.setFormControls();
                    try
                    {
                        this._BindingSource.Position = this.CurrentPosition(ID);
                        if (!this._SplitContainerData.Panel2Collapsed)
                        {
                            this.setSpecimenList(ID);
                            this.ItemChanged();
                        }
                        if (this.ChildControls.Count > 0)
                        {
                            this.hideChildControls();
                        }
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                    if (this._UserControlQueryList != null)
                    {
                        if (this._ControlMainData != null)
                            this._ControlMainData.Enabled = true;
                        if (this._ControlDependentData != null)
                            this._ControlDependentData.Enabled = true;
                        System.Collections.Generic.List<DiversityWorkbench.UserControls.QueryRestrictionItem> Restricts = this._UserControlQueryList.getQueryRestrictionList();
                        if (Restricts.Count > 0)
                        {
                            foreach (DiversityWorkbench.UserControls.QueryRestrictionItem Restrict in Restricts)
                            {
                                if (this.ColumnID == Restrict.ColumnName)
                                {
                                    string Res = Restrict.Restriction;
                                    string SQL = "SELECT COUNT(*) FROM " + Restrict.TableName + " T WHERE " + Restrict.ColumnName + " " + Res + " AND " + Restrict.ColumnName + " = " + ID.ToString();
                                    int Count;
                                    if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL).ToString(), out Count) && Count == 0)
                                    {
                                        if (this._ControlMainData != null)
                                            this._ControlMainData.Enabled = false;
                                        if (this._ControlDependentData != null)
                                            this._ControlDependentData.Enabled = false;
                                    }
                                }
                            }
                        }
                    }
                    // #205
                    //if (this._DataTable.TableName == "Collection")
                    //{
                    //    DiversityCollection.Forms.FormCollection f = (DiversityCollection.Forms.FormCollection)this._Form;
                    //    f.CollectionLocation.BuildHierarchy(ID, true);
                    //}
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private int CurrentPosition(int ID)
        {
            int i = 0;
            if (this._DataTable.Rows.Count > 0)
            {
                foreach (System.Data.DataRow r in this._DataTable.Rows)
                {
                    if (r.RowState != System.Data.DataRowState.Deleted)
                    {
                        if (r[this.ColumnID].ToString() == ID.ToString()) break;
                        i++;
                    }
                }
            }
            return i;
        }

        #endregion

        #region Marking

        protected bool markSelectedUnitNode(int ID)
        {
            if (this.UseHierarchyNodes)
                return true;

            bool OK = true;
            this.unmarkUnitNodes();
            System.Windows.Forms.TreeNode N = this.getNodeByID(ID);
            if (N != null && !this.UseHierarchyNodes)// && N.Text.Length > 0)
            {
                N.BackColor = System.Drawing.Color.Yellow;
                //N.EnsureVisible();
            }
            else
                OK = false;
            return OK;
        }

        protected System.Windows.Forms.TreeNode getNodeByID(int ID)
        {
            System.Windows.Forms.TreeNode Node;
            try
            {
                foreach (System.Windows.Forms.TreeNode N in this._TreeView.Nodes)
                {
                    if (N.Tag != null)
                    {
                        System.Data.DataRow r = (System.Data.DataRow)N.Tag;
                        if (r.RowState != System.Data.DataRowState.Detached && r[0].ToString() == ID.ToString())
                        {
                            Node = N;
                            return Node;
                        }
                    }
                }
                foreach (System.Windows.Forms.TreeNode N in this._TreeView.Nodes)
                {
                    Node = this.getNodeByID(N, ID);
                    if (Node != null)
                    {
                        return Node;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            Node = new TreeNode();
            return Node;
        }

        private System.Windows.Forms.TreeNode getNodeByID(System.Windows.Forms.TreeNode Node, int ID)
        {
            System.Windows.Forms.TreeNode NodeByID;
            foreach (System.Windows.Forms.TreeNode N in Node.Nodes)
            {
                if (N.Tag != null)
                {
                    System.Data.DataRow r = (System.Data.DataRow)N.Tag;
                    if (r.RowState != System.Data.DataRowState.Detached && r[0].ToString() == ID.ToString())
                    {
                        NodeByID = N;
                        return NodeByID;
                    }
                    else if (r.RowState == System.Data.DataRowState.Detached)
                    {
                    }
                }
            }
            foreach (System.Windows.Forms.TreeNode N in Node.Nodes)
            {
                NodeByID = this.getNodeByID(N, ID);
                if (NodeByID != null && NodeByID.Text.Length > 0)
                    return NodeByID;
            }
            NodeByID = new TreeNode();
            return NodeByID;
        }

        private void markSelectedUnitNode(System.Windows.Forms.TreeNode Node)
        {
            this.unmarkUnitNodes();
            if (!this.UseHierarchyNodes)
                Node.BackColor = System.Drawing.Color.Yellow;
        }

        private void unmarkUnitNodes()
        {
            if (this.UseHierarchyNodes)
                return;

            foreach (System.Windows.Forms.TreeNode N in this._TreeView.Nodes)
            {
                N.BackColor = System.Drawing.SystemColors.Window;
                unmarkUnitNodes(N);
            }
        }

        private void unmarkUnitNodes(System.Windows.Forms.TreeNode Node)
        {
            foreach (System.Windows.Forms.TreeNode N in Node.Nodes)
            {
                N.BackColor = System.Drawing.SystemColors.Window;
                this.unmarkUnitNodes(N);
            }
        }

        #endregion

        #region Drag & Drop

        public void treeView_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode ChildNode;
            TreeNode ParentNode;
            int ChildID;
            int oldChildSubstrateID;
            int ParentID;
            System.Drawing.Point pt;
            try
            {
                if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
                {
                    pt = this._TreeView.PointToClient(new System.Drawing.Point(e.X, e.Y));
                    ParentNode = this._TreeView.GetNodeAt(pt);
                    ChildNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                    if (!ParentNode.Equals(ChildNode))
                    {
                        System.Data.DataRow rChild = (System.Data.DataRow)ChildNode.Tag;
                        ChildID = System.Int32.Parse(rChild[this.ColumnID].ToString());
                        if (rChild[this.ColumnParentID].Equals(System.DBNull.Value)) oldChildSubstrateID = -1;
                        else oldChildSubstrateID = System.Int32.Parse(rChild[this.ColumnParentID].ToString());
                        System.Data.DataRow rParent;
                        if (ParentNode.Tag != null)
                        {
                            rParent = (System.Data.DataRow)ParentNode.Tag;
                            ParentID = System.Int32.Parse(rParent[this.ColumnID].ToString());
                            rChild[this.ColumnParentID] = ParentID;
                        }
                        else rChild[this.ColumnParentID] = System.DBNull.Value;
                        System.Data.DataRow[] rr = this._DataTable.Select(this.ColumnParentID + " = " + ChildID.ToString());
                        foreach (System.Data.DataRow r in rr)
                        {
                            if (oldChildSubstrateID > -1) r[this.ColumnParentID] = oldChildSubstrateID;
                            else r[this.ColumnParentID] = System.DBNull.Value;
                        }
                        this.buildHierarchy();
                    }
                }
                else if(this.GetType() == typeof(Collection))
                {
                    Collection collection = (Collection)this;
                    if (collection.HierarchyIncludingParts)
                    {
                        pt = this._TreeView.PointToClient(new System.Drawing.Point(e.X, e.Y));
                        ParentNode = this._TreeView.GetNodeAt(pt);
                        if (ParentNode !=null && ParentNode.Tag != null && ParentNode.Tag.GetType().BaseType == typeof(System.Data.DataRow))
                        {
                            System.Data.DataRow rParent = (System.Data.DataRow)ParentNode.Tag;
                            if (rParent.Table.TableName == "Collection")
                            {
                                ParentID = System.Int32.Parse(rParent[this.ColumnID].ToString());
                                ChildNode = (TreeNode)e.Data.GetData("DiversityCollection.HierarchyNode");
                                if (ChildNode != null && !ParentNode.Equals(ChildNode))
                                {
                                    if (ChildNode.Tag != null && ChildNode.Tag.GetType() == typeof(System.Data.DataRow))
                                    {
                                        System.Data.DataRow dataRow = (System.Data.DataRow)ChildNode.Tag;
                                        if (dataRow.Table.TableName == "CollectionSpecimenPart")
                                        {
                                            string SQL = "UPDATE P SET CollectionID = " + ParentID.ToString() + " FROM CollectionSpecimenPart P WHERE P.CollectionSpecimenID = " + dataRow["CollectionSpecimenID"].ToString() + " AND P.SpecimenPartID = " + dataRow["SpecimenPartID"].ToString();
                                            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                                                this.buildHierarchy();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void treeView_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            TreeView tv = sender as TreeView;
            System.Drawing.Point pt = tv.PointToClient(new System.Drawing.Point(e.X, e.Y));
            int delta = tv.Height - pt.Y;
            if ((delta < tv.Height / 2) && (delta > 0))
            {
                TreeNode tn = tv.GetNodeAt(pt.X, pt.Y);
                if (tn.NextVisibleNode != null)
                    tn.NextVisibleNode.EnsureVisible();
            }
            if ((delta > tv.Height / 2) && (delta < tv.Height))
            {
                TreeNode tn = tv.GetNodeAt(pt.X, pt.Y);
                if (tn.PrevVisibleNode != null)
                    tn.PrevVisibleNode.EnsureVisible();
            }
        }

        public void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            this._TreeView.DoDragDrop(e.Item, System.Windows.Forms.DragDropEffects.Move);
        }

        #endregion

        #endregion

        #region Specimen list

        private string sqlSpecimen(int ID)
        {
            string SQL = "SELECT CASE WHEN AccessionNumber IS NULL OR RTRIM(AccessionNumber) = '' THEN 'ID: ' + CAST(CollectionSpecimenID AS VARCHAR) " +
                         "ELSE AccessionNumber END AS DisplayText, CollectionSpecimenID AS ID " +
                         "FROM CollectionSpecimen " +
                         "WHERE CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM " + this._SpecimenTable + " " +
                         "WHERE " + this.ColumnID + " = " + ID.ToString() + ") " +
                         "AND CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionSpecimenID_UserAvailable) " +
                         "ORDER BY DisplayText";
            return SQL;
        }

        #region toolStrip

        #endregion

        public void setSpecimenList(int ID)
        {
            try
            {
                if (!this._SplitContainerData.Panel2Collapsed && this._UserControlSpecimenList != null)
                {
                    this._UserControlSpecimenList.setSpecimenList(this.sqlSpecimen(ID));
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion    

        #region Form

        public void initForm()
        {
            try
            {
                if (this._UserControlQueryList != null)
                    this._UserControlQueryList.setConnection(DiversityWorkbench.Settings.ConnectionString, this._MainTable);
                if (this._MainTableContainsHierarchy)
                    this.initTreeView();
                this._UserControlQueryList.toolStripButtonConnection.Visible = false;
                this._UserControlQueryList.toolStripButtonSwitchOrientation.Visible = false;
                this._UserControlQueryList.toolStripSeparator1.Visible = false;
                this.setQueryControlEvents();
                this._UserControlQueryList.setQueryConditions(this.QueryConditions);
                this._UserControlQueryList.QueryDisplayColumns = this.QueryDisplayColumns;
                if (this._ToolStripButtonSpecimenList != null)
                    this._ToolStripButtonSpecimenList.Click += new System.EventHandler(this.toolStripButtonSpecimenList_Click);
                this._HelpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
                this.FormFunctions.addEditOnDoubleClickToTextboxes();
                this.FormFunctions.setDescriptions();
                DiversityWorkbench.Entity.setEntity(this._Form, this._toolTip);
#if DEBUG
                this._UserControlQueryList.OptimizingAllow(true);
#endif
                if (this._UserControlQueryList.Optimizing_AllowedForQueryList)
                {
                    System.Windows.Forms.Control control = this._UserControlQueryList.Parent;
                    while (control.GetType().BaseType != typeof(System.Windows.Forms.Form))
                        control = control.Parent;
                    DiversityWorkbench.UserControls.UserControlQueryList.ModuleForm_QueryMainTableOptimizing(DiversityWorkbench.Settings.ModuleName, control.Name, this._MainTable);
#if DEBUG
                    this._UserControlQueryList.ManyOrderByColumns_Allow(true, DiversityWorkbench.Settings.ModuleName, control.Name);
#endif
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public virtual void setFormControls() { }
        
        protected void hideChildControls()
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._BindingSource.Current;
                System.Boolean V = true;
                if (R[this.ColumnParentID].Equals(System.DBNull.Value))
                    V = false;
                foreach (System.Windows.Forms.Control C in this.ChildControls)
                    C.Visible = V;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void setToolbarPermission(ref System.Windows.Forms.ToolStripButton B, string TableName, string Permission)
        {
            try
            {
                B.Enabled = this.FormFunctions.getObjectPermissions(TableName, Permission);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void setToolStripButtonNewEvent(System.Windows.Forms.ToolStripButton B)
        {
            try
            {
                B.Click += new System.EventHandler(this.toolStripButtonNew_Click);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void setToolStripMenuItemNewEvent(System.Windows.Forms.ToolStripMenuItem B)
        {
            try
            {
                B.Click += new System.EventHandler(this.toolStripButtonNew_Click);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void setToolStripButtonCopyEvent(System.Windows.Forms.ToolStripButton B)
        {
            try
            {
                B.Click += new System.EventHandler(this.toolStripButtonCopy_Click);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void setToolStripButtonCopyHierarchyEvent(System.Windows.Forms.ToolStripButton B)
        {
            try
            {
                B.Click += new System.EventHandler(this.toolStripButtonCopyHierarchy_Click);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void setToolStripButtonDeleteEvent(System.Windows.Forms.ToolStripButton B)
        {
            try
            {
                B.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void setToolStripButtonSetParentEvent(System.Windows.Forms.ToolStripButton B)
        {
            try
            {
                B.Click += new System.EventHandler(this.toolStripButtonSetParent_Click);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void setToolStripButtonSetParentWithHierarchyEvent(System.Windows.Forms.ToolStripButton B)
        {
            try
            {
                B.Click += new System.EventHandler(this.toolStripButtonSetParentWithHierarchy_Click);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void setToolStripButtonRemoveParentEvent(System.Windows.Forms.ToolStripButton B)
        {
            try
            {
                B.Click += new System.EventHandler(this.toolStripButtonRemoveParent_Click);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void setToolStripButtonIncludeIDEvent(System.Windows.Forms.ToolStripButton B)
        {
            try
            {
                B.Click += new System.EventHandler(this.toolStripButtonIncludeID_Click);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setQueryControlEvents()
        {
            try
            {
                // QueryList
                this._UserControlQueryList.toolStripButtonCopy.Click += new System.EventHandler(this.copyItem);
                this._UserControlQueryList.toolStripButtonDelete.Click += new System.EventHandler(this.deleteItem_Click);
                this._UserControlQueryList.toolStripButtonNew.Click += new System.EventHandler(this.createNewItem);
                this._UserControlQueryList.toolStripButtonSave.Click += new System.EventHandler(this.saveItem);
                this._UserControlQueryList.toolStripButtonUndo.Click += new System.EventHandler(this.undoChangesInItem_Click);
                this._UserControlQueryList.listBoxQueryResult.SelectedIndexChanged += new System.EventHandler(this.listBoxQueryResult_SelectedIndexChanged);
                // Subscribe to the event from the UserControl
                this._UserControlQueryList.SuppressSelectedIndexChangedEvent +=
                    UserControlQueryList_SuppressSelectedIndexChangedEvent;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void UserControlQueryList_SuppressSelectedIndexChangedEvent(object sender, bool suppress)
        {
            _suppressSelectedIndexChanged = suppress;
        }
        
        private void listBoxQueryResult_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (_suppressSelectedIndexChanged)
                return; // Suppress the event
            try
            {
                this._Form.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                this._Form.SuspendLayout();
                System.Windows.Forms.ListBox L = (System.Windows.Forms.ListBox)sender;
                if (L.SelectedIndex > -1)
                {
                    int ID = int.Parse(L.SelectedValue.ToString());
                    this.setItem(ID);
                }
                else
                {
                    this._SplitContainerMain.Panel2.Visible = false;
                }
                this.ItemChanged();
                this._Form.ResumeLayout();
                this._Form.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public virtual void ItemChanged() { }

        public void setReadOnly()
        {
            try
            {
                foreach (System.Windows.Forms.Control C in this._Form.Controls)
                {
                    if (C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlDialogPanel))
                        continue;
                    setReadOnly(C);
                }
                
                //foreach (System.Windows.Forms.Control C in this.ChildControls)
                //    C.Enabled = false;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setReadOnly(System.Windows.Forms.Control ParentControl)
        {
            if (ParentControl.GetType() == typeof(System.Windows.Forms.TableLayoutPanel))
            {
            }
            foreach (System.Windows.Forms.Control C in ParentControl.Controls)
            {
                if (C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryList))
                {
                    continue;
                }
                if (C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlDialogPanel))
                {
                    continue;
                }
                if (C.GetType() == typeof(System.Windows.Forms.SplitContainer))
                {
                    this.setReadOnly(C);
                }
                else if (C.GetType() == typeof(System.Windows.Forms.SplitterPanel))
                {
                    this.setReadOnly(C);
                }
                else if (C.GetType() == typeof(System.Windows.Forms.GroupBox))
                {
                    this.setReadOnly(C);
                }
                else if (C.GetType() == typeof(System.Windows.Forms.TableLayoutPanel))
                {
                    this.setReadOnly(C);
                }
                else if (C.GetType() == typeof(System.Windows.Forms.TabControl))
                {
                    this.setReadOnly(C);
                }
                else if (C.GetType() == typeof(System.Windows.Forms.TabPage))
                {
                    this.setReadOnly(C);
                }
                else
                {
                    C.Enabled = false;
                }
            }
        }


        
        #endregion
    }
}
