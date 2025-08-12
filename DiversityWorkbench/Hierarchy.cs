using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench
{
    public class Hierarchy
    {
        #region Parameter

        protected System.Windows.Forms.TreeView _TreeView;
        protected System.Data.DataTable _DataTable;
        protected System.Windows.Forms.BindingSource _BindingSource;
        protected string _ColumnID;
        protected string _ColumnParentID;
        protected string _ColumnDisplayText;
        protected string _ColumnDisplayOrder;
        protected string _TopColumn;
        protected string _TopMarker;
        protected System.Collections.Generic.List<int> _IDs;

        //protected System.Windows.Forms.ImageList _imageList = null;
        protected string _ColumnImageSelection = "";
        protected System.Collections.Generic.Dictionary<string, int> _ImageKeys;

        #endregion

        #region Construction

        public Hierarchy(System.Windows.Forms.TreeView TreeView,
        System.Data.DataTable DataTable,
        System.Windows.Forms.BindingSource BindingSource,
        string ColumnID,
        string ColumnParentID,
        string ColumnDisplayText,
        string ColumnDisplayOrder)
            {
                this._TreeView = TreeView;
                this._DataTable = DataTable;
                this._BindingSource = BindingSource;
                this._ColumnID = ColumnID;
                this._ColumnParentID = ColumnParentID;
                this._ColumnDisplayText = ColumnDisplayText;
                this._ColumnDisplayOrder = ColumnDisplayOrder;
                this.initTreeView();
            }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TreeView"></param>
        /// <param name="DataTable"></param>
        /// <param name="BindingSource"></param>
        /// <param name="ColumnID">The Column containing the ID of the datasets</param>
        /// <param name="ColumnParentID">The Column containing the ParentID refering to the ID of the datasets</param>
        /// <param name="IDs">The list of the IDs the user has access to</param>
        /// <param name="ColumnDisplayText"></param>
        /// <param name="ColumnDisplayOrder"></param>
        public Hierarchy(System.Windows.Forms.TreeView TreeView,
        System.Data.DataTable DataTable,
        System.Windows.Forms.BindingSource BindingSource,
        string ColumnID,
        string ColumnParentID,
        System.Collections.Generic.List<int> IDs,
        string ColumnDisplayText,
        string ColumnDisplayOrder)
        {
            this._TreeView = TreeView;
            this._DataTable = DataTable;
            this._BindingSource = BindingSource;
            this._ColumnID = ColumnID;
            this._ColumnParentID = ColumnParentID;
            this._ColumnDisplayText = ColumnDisplayText;
            this._ColumnDisplayOrder = ColumnDisplayOrder;
            this._IDs = IDs;
            this.initTreeView();
        }

        public Hierarchy(System.Windows.Forms.TreeView TreeView,
            System.Data.DataTable DataTable,
            System.Windows.Forms.BindingSource BindingSource,
            string ColumnID,
            string ColumnParentID,
            string ColumnDisplayText,
            string ColumnDisplayOrder,
            bool AddEvents)
        {
            this._TreeView = TreeView;
            this._DataTable = DataTable;
            this._BindingSource = BindingSource;
            this._ColumnID = ColumnID;
            this._ColumnParentID = ColumnParentID;
            this._ColumnDisplayText = ColumnDisplayText;
            this._ColumnDisplayOrder = ColumnDisplayOrder;
            if (AddEvents)
                this.initTreeView();
        }

        public Hierarchy(System.Windows.Forms.TreeView TreeView,
            System.Data.DataTable DataTable,
            string ColumnID,
            string ColumnParentID,
            string ColumnDisplayText,
            string ColumnDisplayOrder)
        {
            this._TreeView = TreeView;
            this._DataTable = DataTable;
            this._ColumnID = ColumnID;
            this._ColumnParentID = ColumnParentID;
            this._ColumnDisplayText = ColumnDisplayText;
            this._ColumnDisplayOrder = ColumnDisplayOrder;
        }

        #endregion     
   
        #region Hierarchy

        /// <summary>
        /// The list of IDs the user has access to
        /// </summary>
        public System.Collections.Generic.List<int> IDs
        {
            get { return _IDs; }
            set { _IDs = value; }
        }


        //public System.Windows.Forms.ImageList ImageList { set { _imageList = value; } }
        public string ColumnImageSelection { set { _ColumnImageSelection = value; } }
        public System.Collections.Generic.Dictionary<string, int> ImageKeys { set { _ImageKeys = value; } }

        #region Tree view

        private void initTreeView()
        {
            this._TreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            this._TreeView.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView_DragOver);
            this._TreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView_DragDrop);
            this._TreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag);
            this._TreeView.AllowDrop = true;
        }

        public void buildHierarchy(int ID)
        {
            this.buildHierarchy();
            this.markSelectedUnitNode(ID);
        }

        public virtual void buildHierarchy()
        {
            try
            {
                this._TreeView.Nodes.Clear();
                // getting the column names
                string ColumnID = this._ColumnID;
                string ColumnParentID = this._ColumnParentID;
                string ColumnDisplayText = this._ColumnDisplayText;
                // list for the rows allready in hierarchy
                System.Collections.Generic.List<System.Data.DataRow> RowsTop = new List<System.Data.DataRow>();
                // list for the rows allready in hierarchy
                System.Collections.Generic.List<System.Data.DataRow> RowsInHierarchy = new List<System.Data.DataRow>();
                // list for the rows to check if all are entered as nodes
                System.Collections.Generic.List<System.Data.DataRow> RowList = new List<System.Data.DataRow>();
                // Check if a column Ord is existing
                string OrderColumn = "";
                foreach (System.Data.DataColumn C in this._DataTable.Columns)
                {
                    if (C.ColumnName.StartsWith("Ord"))
                        OrderColumn = C.ColumnName;
                }
                System.Data.DataRow[] RRHierarchy;
                if (OrderColumn.Length > 0)
                    RRHierarchy = this._DataTable.Select("", "Ord ASC");
                else
                    RRHierarchy = this._DataTable.Select("", "");
                foreach (System.Data.DataRow R in RRHierarchy)
                {
                    if (R.RowState != System.Data.DataRowState.Deleted)
                        RowList.Add(R);
                }
                if (this._DataTable.Rows.Count > 0)
                {
                    if (this.TopMarker != null && this.TopMarker.Length > 0 && this.TopColumn != null && this.TopColumn.Length > 0)
                    {
                        foreach (System.Object O in RowList)
                        {
                            System.Data.DataRow R = (System.Data.DataRow)O;
                            if (R.RowState != System.Data.DataRowState.Deleted)
                            {
                                if (!R[this.TopColumn].Equals(System.DBNull.Value) &&
                                    R[this.TopColumn].ToString() == this.TopMarker)
                                {
                                    RowsTop.Add(R);
                                }
                            }
                        }
                    }
                    if (RowsTop.Count > 0)
                    {
                        foreach (System.Object O in RowsTop)
                        {
                            try
                            {
                                System.Data.DataRow R = (System.Data.DataRow)O;
                                if (R.RowState != System.Data.DataRowState.Deleted)
                                {
                                    if (R[this._ColumnParentID].Equals(System.DBNull.Value) &&
                                        !RowsInHierarchy.Contains(R))
                                    {
                                        string NodeText = R[this._ColumnDisplayText].ToString().Trim();
                                        if (NodeText.Length == 0) NodeText = "      ";
                                        System.Windows.Forms.TreeNode Node = new System.Windows.Forms.TreeNode(NodeText);
                                        Node.Tag = R;
                                        this.setNodeImageKey(Node);
                                        if (!R[this.TopColumn].Equals(System.DBNull.Value) &&
                                            R[this.TopColumn].ToString() == this.TopMarker)
                                        {
                                            System.Drawing.Font F = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, (float)8.25, System.Drawing.FontStyle.Underline);
                                            Node.NodeFont = F;
                                        }
                                        this._TreeView.Nodes.Add(Node);
                                        this.addHierarchyNodes(System.Int32.Parse(R[this._ColumnID].ToString()), Node, ref RowList, ref RowsInHierarchy);
                                        RowsInHierarchy.Add(R);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            }
                        }
                    }
                    else
                    {
                        System.Windows.Forms.TreeNode rootNode = new System.Windows.Forms.TreeNode(this._DataTable.Rows[0][ColumnDisplayText].ToString());
                        rootNode.Tag = this._DataTable.Rows[0];
                        this.setNodeImageKey(rootNode);
                        // if there is no starting point - create one
                        System.Data.DataRow[] RR = this._DataTable.Select(this._ColumnParentID + " IS NULL ");
                        if (RR.Length == 0)
                        {
                            if (this.TopMarker != null && this.TopMarker.Length > 0 && this.TopColumn != null && this.TopColumn.Length > 0)
                            {
                                foreach (System.Object O in RowList)
                                {
                                    try
                                    {
                                        System.Data.DataRow R = (System.Data.DataRow)O;
                                        if (R.RowState != System.Data.DataRowState.Deleted)
                                        {
                                            if (!R[this.TopColumn].Equals(System.DBNull.Value) &&
                                                R[this.TopColumn].ToString() == this.TopMarker)
                                            {
                                                R[this._ColumnParentID] = System.DBNull.Value;
                                                break;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                                    }
                                }
                            }
                            foreach (System.Data.DataRow r in this._DataTable.Rows)
                            {
                                try
                                {
                                    if (r.RowState != System.Data.DataRowState.Deleted)
                                    {
                                        if (!r[this._ColumnParentID].Equals(System.DBNull.Value)
                                            && r[this._ColumnParentID].ToString() == r[this._ColumnID].ToString())
                                        {
                                            r[this._ColumnParentID] = System.DBNull.Value;
                                            break;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                                }
                            }
                        }
                        foreach (System.Data.DataRow r in this._DataTable.Rows)
                        {
                            try
                            {
                                if (r.RowState != System.Data.DataRowState.Deleted)
                                {
                                    if (!r[this._ColumnParentID].Equals(System.DBNull.Value))
                                    {
                                        int ID = System.Int32.Parse(r[this._ColumnParentID].ToString().Trim());
                                        System.Data.DataRow[] rr = this._DataTable.Select(this._ColumnID + " = " + ID.ToString());
                                        if (rr.Length == 0)
                                            r[this._ColumnParentID] = System.DBNull.Value;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            }
                        }
                        // prepare SubstrateID: set SubstrateID at max DisplayOrder to NULL if no starting point can be found
                        System.Data.DataRow[] rrS = this._DataTable.Select(this._ColumnParentID + " IS NULL", "", System.Data.DataViewRowState.CurrentRows);
                        if (rrS.Length == 0)
                        {
                            System.Data.DataRow[] rrD = this._DataTable.Select("", this._ColumnDisplayOrder + " DESC", System.Data.DataViewRowState.CurrentRows);
                            if (rrD.Length > 0)
                            {
                                System.Data.DataRow rrDMax = rrD[0];
                                rrDMax[this._ColumnParentID] = System.DBNull.Value;
                            }
                        }

                        foreach (System.Object O in RowList)
                        {
                            try
                            {
                                System.Data.DataRow R = (System.Data.DataRow)O;
                                if (R.RowState != System.Data.DataRowState.Deleted)
                                {
                                    if (R[this._ColumnParentID].Equals(System.DBNull.Value) &&
                                        !RowsInHierarchy.Contains(R))
                                    {
                                        string NodeText = R[this._ColumnDisplayText].ToString().Trim();
                                        if (NodeText.Length == 0) NodeText = "      ";
                                        System.Windows.Forms.TreeNode Node = new System.Windows.Forms.TreeNode(NodeText);
                                        Node.Tag = R;
                                        this.setNodeImageKey(Node);
                                        if (this.TopMarker != null)
                                        {
                                            if (!R[this.TopColumn].Equals(System.DBNull.Value) &&
                                                R[this.TopColumn].ToString() == this.TopMarker)
                                            {
                                                System.Drawing.Font F = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, (float)8.25, System.Drawing.FontStyle.Underline);
                                                Node.NodeFont = F;
                                            }
                                        }
                                        if (this._IDs != null)
                                        {
                                            int ID;
                                            if (int.TryParse(R[this._ColumnID].ToString(), out ID) && !this._IDs.Contains(ID))
                                                Node.ForeColor = System.Drawing.Color.Red;
                                        }
                                        this._TreeView.Nodes.Add(Node);
                                        this.addHierarchyNodes(System.Int32.Parse(R[this._ColumnID].ToString()), Node, ref RowList, ref RowsInHierarchy);
                                        RowsInHierarchy.Add(R);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            }
                        }
                    }
                    foreach (System.Object O in RowsInHierarchy)
                    {
                        try
                        {
                            System.Data.DataRow R = (System.Data.DataRow)O;
                            RowList.Remove(R);
                        }
                        catch (Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }

                    if (RowList.Count > 0)
                    {
                        foreach (System.Object O in RowList)
                        {
                            try
                            {
                                System.Data.DataRow R = (System.Data.DataRow)O;
                                if (R.RowState != System.Data.DataRowState.Deleted)
                                {
                                    string NodeText = R[this._ColumnDisplayText].ToString().Trim();
                                    if (NodeText.Length == 0) NodeText = "      ";
                                    System.Windows.Forms.TreeNode Node = new System.Windows.Forms.TreeNode(NodeText);
                                    Node.Tag = R;
                                    this.setNodeImageKey(Node);
                                    this._TreeView.Nodes.Add(Node);
                                    //Rows.Remove(Rows[i]);
                                }
                            }
                            catch (Exception ex)
                            {
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            }
                        }
                    }
                    this._TreeView.ExpandAll();
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void setNodeImageKey(System.Windows.Forms.TreeNode node)
        {
            try
            {
                if (this._ImageKeys != null && this._ColumnImageSelection.Length > 0 && node.Tag != null && node.Tag.GetType().BaseType == typeof(System.Data.DataRow))
                {
                    System.Data.DataRow R = (System.Data.DataRow)node.Tag;
                    if (R.Table.Columns.Contains(this._ColumnImageSelection))
                    {
                        string ImageSelection = R[_ColumnImageSelection].ToString();
                        if (this._ImageKeys.ContainsKey(ImageSelection))
                        {
                            node.ImageIndex = this._ImageKeys[ImageSelection];
                            node.SelectedImageIndex = this._ImageKeys[ImageSelection];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// Listing all entries according to a filter
        /// </summary>
        /// <param name="Filter">the text by which the entries should be filtered</param>
        public virtual void buildHierarchy(string Filter)
        {
            this._TreeView.Nodes.Clear();
            // getting the column names
            string ColumnID = this._ColumnID;
            string ColumnParentID = this._ColumnParentID;
            string ColumnDisplayText = this._ColumnDisplayText;
            // list for the rows allready in hierarchy
            System.Collections.Generic.List<System.Data.DataRow> RowsTop = new List<System.Data.DataRow>();
            // list for the rows allready in hierarchy
            System.Collections.Generic.List<System.Data.DataRow> RowsInHierarchy = new List<System.Data.DataRow>();
            // list for the rows to check if all are entered as nodes
            System.Collections.Generic.List<System.Data.DataRow> RowList = new List<System.Data.DataRow>();
            // Check if a column Ord is existing
            string OrderColumn = "";
            foreach (System.Data.DataColumn C in this._DataTable.Columns)
            {
                if (C.ColumnName.StartsWith("Ord"))
                    OrderColumn = C.ColumnName;
            }
            System.Data.DataRow[] RRHierarchy;
            if (OrderColumn.Length > 0)
                RRHierarchy = this._DataTable.Select("", "Ord ASC");
            else
                RRHierarchy = this._DataTable.Select("", "");
            foreach (System.Data.DataRow R in RRHierarchy)
            {
                if (R.RowState != System.Data.DataRowState.Deleted)
                {
                    if (R[ColumnDisplayText].ToString().ToLower().IndexOf(Filter.ToLower()) > -1)
                        RowList.Add(R);
                }
            }
            foreach (System.Data.DataRow R in RowList)
            {
                string NodeText = R[this._ColumnDisplayText].ToString().Trim();
                if (NodeText.Length == 0) NodeText = "      ";
                System.Windows.Forms.TreeNode Node = new System.Windows.Forms.TreeNode(NodeText);
                Node.Tag = R;
                this.setNodeImageKey(Node);
                if (this._IDs != null)
                {
                    int ID;
                    if (int.TryParse(R[this._ColumnID].ToString(), out ID) && !this._IDs.Contains(ID))
                        Node.ForeColor = System.Drawing.Color.Red;
                }
                this._TreeView.Nodes.Add(Node);
            }
            this._TreeView.ExpandAll();
        }

        protected void addHierarchyNodes(int ParentID, System.Windows.Forms.TreeNode pNode)
        {
            System.Data.DataRow[] rr = this._DataTable.Select(this._ColumnParentID + " = " + ParentID.ToString());
            foreach (System.Data.DataRow r in rr)
            {
                int CollectionID = System.Int32.Parse(r[this._ColumnID].ToString());
                System.Windows.Forms.TreeNode Node = new System.Windows.Forms.TreeNode(r[this._ColumnDisplayText].ToString().Trim());
                Node.Tag = r;
                this.setNodeImageKey(Node);
                pNode.Nodes.Add(Node);
                this.addHierarchyNodes(CollectionID, Node);
            }
        }

        protected virtual void addHierarchyNodes(int ParentID, System.Windows.Forms.TreeNode pNode, 
            ref System.Collections.Generic.List<System.Data.DataRow> RowList,
            ref System.Collections.Generic.List<System.Data.DataRow> RowsInHierarchy)
        {
            //System.Data.DataRow[] rr = this._DataTable.Select(this._ColumnParentID + " = " + ParentID.ToString());
            foreach(System.Object O in RowList)
            {
                System.Data.DataRow R = (System.Data.DataRow)O;
                if (R[this._ColumnParentID].ToString() == ParentID.ToString() &&
                    !RowsInHierarchy.Contains(R))
                {
                    int CollectionID = System.Int32.Parse(R[this._ColumnID].ToString());
                    System.Windows.Forms.TreeNode Node = new System.Windows.Forms.TreeNode(R[this._ColumnDisplayText].ToString().Trim());
                    Node.Tag = R;
                    this.setNodeImageKey(Node);
                    if (this.TopMarker != null)
                    {
                        if (!R[this.TopColumn].Equals(System.DBNull.Value) &&
                            R[this.TopColumn].ToString() == this.TopMarker)
                        {
                            System.Drawing.Font F = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, (float)8.25, System.Drawing.FontStyle.Underline);
                            Node.NodeFont = F;
                        }
                    }
                    if (this._IDs != null)
                    {
                        int ID;
                        if (int.TryParse(R[this._ColumnID].ToString(), out ID) && !this._IDs.Contains(ID))
                            Node.ForeColor = System.Drawing.Color.Red;
                    }
                    pNode.Nodes.Add(Node);
                    this.addHierarchyNodes(CollectionID, Node, ref RowList, ref RowsInHierarchy);
                    RowsInHierarchy.Add(R);
                }
            }
        }

        public void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this._TreeView.SelectedNode.Tag != null)
            {
                this.markSelectedUnitNode(this._TreeView.SelectedNode);
                System.Data.DataRow rn = (System.Data.DataRow)this._TreeView.SelectedNode.Tag;
                int ID = System.Int32.Parse(rn[this._ColumnID].ToString());
                if (this._IDs != null && !this._IDs.Contains(ID))
                {
                    System.Windows.Forms.MessageBox.Show("You have no access to this dataset");
                    return;
                }
                if (this._BindingSource == null)
                    return;
                int i = 0;
                if (this._DataTable.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow r in this._DataTable.Rows)
                    {
                        if (r.RowState != System.Data.DataRowState.Deleted)
                        {
                            if (r[this._ColumnID].ToString() == ID.ToString()) break;
                            i++;
                        }
                    }
                }
                try
                {
                    this._BindingSource.Position = i;
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        #endregion

        #region Marking
        #region public static Marking
        public string TopColumn
        {
            get { return _TopColumn; }
            set { _TopColumn = value; }
        }

        public string TopMarker
        {
            get { return _TopMarker; }
            set { _TopMarker = value; }
        }

        public static void markSelectedUnitNode(System.Windows.Forms.TreeView TreeView, int ID)
        {
            foreach (System.Windows.Forms.TreeNode N in TreeView.Nodes)
            {
                N.BackColor = System.Drawing.SystemColors.Window;
                DiversityWorkbench.Hierarchy.unmarkNotSelectedUnitNodes(N);
            }
            System.Windows.Forms.TreeNode Node;
            Node = DiversityWorkbench.Hierarchy.getNodeSelectedByID(TreeView, ID);
            Node.BackColor = System.Drawing.Color.Yellow;
        }



        private static void unmarkNotSelectedUnitNodes(System.Windows.Forms.TreeNode Node)
        {
            foreach (System.Windows.Forms.TreeNode N in Node.Nodes)
            {
                N.BackColor = System.Drawing.SystemColors.Window;
                DiversityWorkbench.Hierarchy.unmarkNotSelectedUnitNodes(N);
            }
        }

        private static System.Windows.Forms.TreeNode getNodeSelectedByID(System.Windows.Forms.TreeView TreeView, int ID)
        {
            System.Windows.Forms.TreeNode Node;
            foreach (System.Windows.Forms.TreeNode N in TreeView.Nodes)
            {
                System.Data.DataRow r = (System.Data.DataRow)N.Tag;
                if (r[0].ToString() == ID.ToString())
                {
                    Node = N;
                    return Node;
                }
            }
            foreach (System.Windows.Forms.TreeNode N in TreeView.Nodes)
            {
                Node = getNodeSelectedByID(N, ID);
                if (Node != null)
                    return Node;
            }
            Node = new TreeNode();
            return Node;
        }

        private static System.Windows.Forms.TreeNode getNodeSelectedByID(System.Windows.Forms.TreeNode Node, int ID)
        {
            System.Windows.Forms.TreeNode NodeByID;
            foreach (System.Windows.Forms.TreeNode N in Node.Nodes)
            {
                System.Data.DataRow r = (System.Data.DataRow)N.Tag;
                if (r[0].ToString() == ID.ToString())
                {
                    NodeByID = N;
                    return NodeByID;
                }
            }
            foreach (System.Windows.Forms.TreeNode N in Node.Nodes)
            {
                NodeByID = getNodeSelectedByID(N, ID);
                if (NodeByID != null)
                    return NodeByID;
            }
            NodeByID = new TreeNode();
            return NodeByID;
        }
        #endregion

        private void markSelectedUnitNode(int ID)
        {
            try
            {
                this.unmarkUnitNodes();
                System.Windows.Forms.TreeNode N = this.getNodeByID(ID);
                N.BackColor = System.Drawing.Color.Yellow;
                this._TreeView.SelectedNode = N;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private System.Windows.Forms.TreeNode getNodeByID(int ID)
        {
            System.Windows.Forms.TreeNode Node;
            foreach (System.Windows.Forms.TreeNode N in this._TreeView.Nodes)
            {
                System.Data.DataRow r = (System.Data.DataRow)N.Tag;
                if (r[0].ToString() == ID.ToString())
                {
                    Node = N;
                    return Node;
                }
            }
            foreach (System.Windows.Forms.TreeNode N in this._TreeView.Nodes)
            {
                Node = this.getNodeByID(N, ID);
                if (Node != null)
                    return Node; // 2
            }
            Node = new TreeNode();
            return Node;
        }

        private System.Windows.Forms.TreeNode getNodeByID(System.Windows.Forms.TreeNode Node, int ID)
        {
            System.Windows.Forms.TreeNode NodeByID;
            foreach (System.Windows.Forms.TreeNode N in Node.Nodes)
            {
                System.Data.DataRow r = (System.Data.DataRow)N.Tag;
                if (r[0].ToString() == ID.ToString())
                {
                    NodeByID = N;
                    return NodeByID; // 1
                }
            }
            foreach (System.Windows.Forms.TreeNode N in Node.Nodes)
            {
                NodeByID = this.getNodeByID(N, ID);
                if (NodeByID != null)
                    return NodeByID;
            }
            NodeByID = new TreeNode();
            return null;// NodeByID; // hier ist das Problem da vorher nix gefunden wird
        }

        private void markSelectedUnitNode(System.Windows.Forms.TreeNode Node)
        {
            this.unmarkUnitNodes();
            Node.BackColor = System.Drawing.Color.Yellow;
        }

        private void unmarkUnitNodes()
        {
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
                        ChildID = System.Int32.Parse(rChild[this._ColumnID].ToString());
                        if (rChild[this._ColumnParentID].Equals(System.DBNull.Value)) oldChildSubstrateID = -1;
                        else oldChildSubstrateID = System.Int32.Parse(rChild[this._ColumnParentID].ToString());
                        System.Data.DataRow rParent;
                        if (ParentNode.Tag != null)
                        {
                            rParent = (System.Data.DataRow)ParentNode.Tag;
                            ParentID = System.Int32.Parse(rParent[this._ColumnID].ToString());
                            rChild[this._ColumnParentID] = ParentID;
                        }
                        else rChild[this._ColumnParentID] = System.DBNull.Value;
                        System.Data.DataRow[] rr = this._DataTable.Select(this._ColumnParentID + " = " + ChildID.ToString());
                        foreach (System.Data.DataRow r in rr)
                        {
                            if (oldChildSubstrateID > -1) r[this._ColumnParentID] = oldChildSubstrateID;
                            else r[this._ColumnParentID] = System.DBNull.Value;
                        }
                        this.buildHierarchy();
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

    }
}
