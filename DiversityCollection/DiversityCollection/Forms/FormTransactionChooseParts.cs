using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormTransactionChooseParts : Form
    {
        #region Parameter

        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterSpecimen;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterPart;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterTransaction;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterCollection;

        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterSpecimenImage;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterAgent;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterProject;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterRelation;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterRelationInvers;

        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterProcessing;

        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterUnit;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterUnitInPart;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterIdentification;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterAnalysis;

        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEvent;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventImage;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterLocalisation;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterLocalisationSystem;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterProperty;

        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesSuperiorList;

        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeries;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesEvent;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesSpecimen;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesSpecimenExtern;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesUnit;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesUnitExtern;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesLocalisation;

        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;

        private int? _CollectionID;
        private string _MaterialCategory;

        private DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenTransactionDataTable _CollectionSpecimenTransaction;
        private DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenPartDataTable _CollectionSpecimenPart;
        private System.Collections.Generic.List<string> _PartIDList = new List<string>();
        #endregion

        #region Construction

        public FormTransactionChooseParts()
        {
            InitializeComponent();
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
        }

        public FormTransactionChooseParts(System.Data.DataTable CollectionSpecimenTransaction)
            : this()
        {
        }

        public FormTransactionChooseParts(System.Data.DataRow[] CollectionSpecimenTransactionRows)
            : this()
        {
            System.Collections.Generic.List<string> CollectionSpecimenIDList = new List<string>();
            foreach (System.Data.DataRow R in CollectionSpecimenTransactionRows)
            {
                if (!CollectionSpecimenIDList.Contains(R["CollectionSpecimenID"].ToString()))
                    CollectionSpecimenIDList.Add(R["CollectionSpecimenID"].ToString());
                if (!this._PartIDList.Contains(R["SpecimenPartID"].ToString()))
                    this._PartIDList.Add(R["SpecimenPartID"].ToString());
            }
            string WhereClause = " CollectionSpecimenID IN (";
            foreach (string s in CollectionSpecimenIDList)
                WhereClause += s + ",";
            WhereClause = WhereClause.Substring(0, WhereClause.Length - 1) + ")";
            this.fillSpecimen(WhereClause);
            this.buildHierarchy();
        }

        public FormTransactionChooseParts(int CollectionSpecimenID, int? CollectionID, string MaterialCategory)
            : this()
        {
            this.fillSpecimen(CollectionSpecimenID);
            this.buildHierarchy();
        }

        public FormTransactionChooseParts(string AccessionNumber, int? CollectionID, string MaterialCategory)
            : this()
        {
            this.fillSpecimen(AccessionNumber);
            this.buildHierarchy();
        }

        //public FormTransactionChooseParts(int CollectionSpecimenID, System.Collections.Generic.List<int> ListOfPartIDs)
        //{
        //    InitializeComponent();
        //    this.fillSpecimen(CollectionSpecimenID);
        //    foreach (DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenPartRow R in this.dataSetCollectionSpecimen.CollectionSpecimenPart.Rows)
        //    {
        //        if (!ListOfPartIDs.Contains(R.SpecimenPartID)) R.Delete();
        //    }
        //    this.buildHierarchy();
        //}

        public FormTransactionChooseParts(DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenPartDataTable dtCollectionSpecimenPart, int? CollectionID, string MaterialCategory)
        {
            InitializeComponent();
            if (dtCollectionSpecimenPart.Rows.Count > 0)
            {
                string WhereClause = " WHERE CollectionSpecimenID IN ( ";
                foreach (System.Data.DataRow R in dtCollectionSpecimenPart.Rows)
                {
                    WhereClause += R["CollectionSpecimenID"].ToString() + ",";
                }
                WhereClause = WhereClause.Substring(0, WhereClause.Length - 1);
                WhereClause += ") AND CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionSpecimenID_UserAvailable)";
                this.fillSpecimen(WhereClause);
                this._CollectionID = CollectionID;
                this._MaterialCategory = MaterialCategory;
                foreach (DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenPartRow RPinSpecimen in this.dataSetCollectionSpecimen.CollectionSpecimenPart.Rows)
                {
                    bool Present = false;
                    foreach (DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenPartRow RP in dtCollectionSpecimenPart.Rows)
                    {
                        if (RP.SpecimenPartID == RPinSpecimen.SpecimenPartID && RPinSpecimen.CollectionSpecimenID == RP.CollectionSpecimenID)
                        {
                            Present = true;
                            break;
                        }
                    }
                    if (!Present)
                    {
                        RPinSpecimen.BeginEdit();
                        RPinSpecimen.Delete();
                        RPinSpecimen.EndEdit();
                    }
                }
                this.dataSetCollectionSpecimen.CollectionSpecimenPart.AcceptChanges();
                this.buildHierarchy();
            }
        }

        #endregion

        #region Hierarchy

        #region Building the Hierarchy

        #region Parts

        #region Common
        private void buildHierarchy()
        {
            if (this.treeViewHierarchy.ImageList == null)
                this.treeViewHierarchy.ImageList = DiversityCollection.Specimen.ImageList;
            LookupTable.DtIdentificationUnit = this.dataSetCollectionSpecimen.IdentificationUnit.Copy();
            this.treeViewHierarchy.Visible = false;
            this.treeViewHierarchy.Nodes.Clear();
            try
            {
                // Tree as Collection tree
                if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count > 0)
                {
                    //this.treeViewHierarchy.LineColor = System.Drawing.Color.DarkOrange;
                    // init the collections table and get the collections
                    string SQL = DiversityCollection.CollectionSpecimen.SqlCollection + " WHERE CollectionID = -1";
                    if (this.dataSetCollectionSpecimen.Collection.Rows.Count == 0)
                    {
                        this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterCollection, SQL, this.dataSetCollectionSpecimen.Collection);
                        this.dataSetCollectionSpecimen.Collection.Clear();
                        SQL = "SELECT * FROM dbo.CollectionHierarchyMulti('";
                        foreach (System.Data.DataRow R in this.dataSetCollectionSpecimen.CollectionSpecimenPart.Rows)
                        {
                            if (!R["CollectionID"].Equals(System.DBNull.Value))
                                SQL += " " + R["CollectionID"].ToString();
                        }
                        if (!this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionID"].Equals(System.DBNull.Value))
                        {
                            SQL += " " + this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionID"].ToString();
                        }
                        SQL += "')";
                    }
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(this.dataSetCollectionSpecimen.Collection);
                    foreach (System.Data.DataRow R in this.dataSetCollectionSpecimen.Collection.Rows)
                    {
                        if (R["CollectionParentID"].Equals(System.DBNull.Value))
                        {
                            System.Drawing.Font F = new Font(System.Drawing.FontFamily.GenericSansSerif, (float)8.25, System.Drawing.FontStyle.Underline);//  
                            DiversityCollection.HierarchyNode Node = new HierarchyNode(R);
                            Node.NodeFont = F;
                            Node.ImageIndex = (int)DiversityCollection.Specimen.OverviewImage.Collection;
                            Node.SelectedImageIndex = (int)DiversityCollection.Specimen.OverviewImage.Collection;
                            Node.Tag = R;
                            this.treeViewHierarchy.Nodes.Add(Node);
                            this.addChildCollectionNodes(Node, this.dataSetCollectionSpecimen.Collection);
                            this.addDependentData(Node);
                            //this.addPartHierarchyDataNodes(Node);
                        }
                    }
                    // if the collection specimen has a CollectinID
                    //if (!this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionID"].Equals(System.DBNull.Value))
                    //{
                    //    System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<System.Windows.Forms.TreeNode>();
                    //    this.getOverviewHierarchyNodes(null, "Collection", this.treeViewHierarchy, ref Nodes);
                    //    foreach (System.Windows.Forms.TreeNode N in Nodes)
                    //    {
                    //        System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                    //        if (R["CollectionID"].ToString() == this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionID"].ToString())
                    //        {
                    //            DiversityCollection.HierarchyNode Node = new HierarchyNode(this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]);
                    //            System.Drawing.Font F = new Font(System.Drawing.FontFamily.GenericSansSerif, (float)8.25, System.Drawing.FontStyle.Underline);//  
                    //            Node.NodeFont = F;
                    //            Node.ImageIndex = (int)DiversityCollection.Specimen.OverviewImage.Specimen;
                    //            Node.SelectedImageIndex = (int)DiversityCollection.Specimen.OverviewImage.SpecimenGrey;
                    //            Node.Tag = R;
                    //            N.Nodes.Add(Node);
                    //            //int PartID = int.Parse(R["SpecimenPartID"].ToString());
                    //            //this.addOverviewHierarchyPartDependentData(Node, PartID);
                    //            break;
                    //        }
                    //    }
                    //}
                    //this.addOverviewHierarchyPartDependentData();
                    this.OverviewHierarchyCollectionNodeToEnd();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.treeViewHierarchy.Visible = true;
            }
            this.OverviewHierarchyPartNodesToEnd();
            this.treeViewHierarchy.ExpandAll();
            //this.setToolStripOverviewHierarchyStorageEditVisibility("");
            //if (this.ShowCollection) 
                this.OverviewHierarchyCollectionNodeToEnd();
        }

        private void addPartHierarchyDataNodes(System.Windows.Forms.TreeNode Node)
        {
            if (Node.Tag != null)
            {
                try
                {
                    System.Data.DataRow R = (System.Data.DataRow)Node.Tag;
                    System.Drawing.Font F = new Font(System.Drawing.FontFamily.GenericSansSerif, (float)8.25, System.Drawing.FontStyle.Bold);
                    foreach (System.Data.DataRow rCS in this.dataSetCollectionSpecimen.CollectionSpecimenPart.Rows)
                    {
                        if (R["CollectionID"].ToString() == rCS["CollectionID"].ToString())
                        {
                            DiversityCollection.HierarchyNode nCS = new HierarchyNode(rCS);
                            this.setPartNodeImage(ref nCS, rCS["MaterialCategory"].ToString());
                            if (this._MaterialCategory.Length > 0 && this._CollectionID != null)
                            {
                                if (rCS["MaterialCategory"].ToString() == this._MaterialCategory
                                    && rCS["CollectionID"].ToString() == this._CollectionID.ToString())
                                    nCS.Checked = true;
                            }
                            nCS.NodeFont = F;
                            Node.Nodes.Add(nCS);
                        }
                    }

                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        //private void treeViewOverviewHierarchyStorage_AfterSelect(object sender, TreeViewEventArgs e)
        //{
        //    string Table = "";
        //    if (this.treeViewOverviewHierarchyStorage.SelectedNode != null)
        //    {
        //        if (this.treeViewOverviewHierarchyStorage.SelectedNode.Tag != null)
        //        {
        //            System.Data.DataRow R = (System.Data.DataRow)this.treeViewOverviewHierarchyStorage.SelectedNode.Tag;
        //            if (R.RowState == DataRowState.Deleted || R.RowState == DataRowState.Detached)
        //            {
        //                if (this.treeViewOverviewHierarchyStorage.SelectedNode.Parent != null)
        //                {
        //                    //this.treeViewOverviewHierarchyStorage.SelectedNode = this.treeViewOverviewHierarchyStorage.SelectedNode.Parent;
        //                    System.Data.DataRow RParent = (System.Data.DataRow)this.treeViewOverviewHierarchyStorage.SelectedNode.Parent.Tag;
        //                    Table = RParent.Table.TableName;
        //                    this.setBindingContext(RParent);
        //                }
        //            }
        //            else
        //            {
        //                Table = R.Table.TableName;
        //                this.setBindingContext(R);
        //            }
        //        }
        //    }
        //    this.setToolStripOverviewHierarchyStorageEditVisibility(Table);
        //    this.setToolStripOverviewHierarchyEditVisibility("");
        //    this.treeViewOverviewHierarchy.SelectedNode = null;
        //    this.setOverviewDataVisibility(Table);
        //    this.setImageVisibility(Table);
        //    this.markSelectedNode(this.treeViewOverviewHierarchyStorage.SelectedNode, this.treeViewOverviewHierarchyStorage);
        //    this.markSelectedNode(null, this.treeViewOverviewHierarchy);
        //}

        private void setPartNodeImage(ref DiversityCollection.HierarchyNode Node, string MaterialCategory)
        {
            MaterialCategory = MaterialCategory.ToLower();
            switch (MaterialCategory)
            {
                case "collection":
                    Node.ImageIndex = (int)DiversityCollection.Specimen.OverviewImage.Collection;
                    break;
                case "processing":
                    Node.ImageIndex = (int)DiversityCollection.Specimen.OverviewImage.Processing;
                    break;
                case "specimen":
                    Node.ImageIndex = (int)DiversityCollection.Specimen.OverviewImageStorage.Specimen;
                    break;
                case "herbarium sheets":
                    Node.ImageIndex = (int)DiversityCollection.Specimen.OverviewImageStorage.HerbariumSheet;
                    break;
                case "sem table":
                    Node.ImageIndex = (int)DiversityCollection.Specimen.OverviewImageStorage.SemTable;
                    break;
                case "dna sample":
                    Node.ImageIndex = (int)DiversityCollection.Specimen.OverviewImageStorage.DNA;
                    break;
                case "cultures":
                    Node.ImageIndex = (int)DiversityCollection.Specimen.OverviewImageStorage.Culture;
                    break;
                case "micr. slide":
                    Node.ImageIndex = (int)DiversityCollection.Specimen.OverviewImageStorage.Slide;
                    break;
                case "icones":
                    Node.ImageIndex = (int)DiversityCollection.Specimen.OverviewImageStorage.Icones;
                    break;
                case "drawing":
                    Node.ImageIndex = (int)DiversityCollection.Specimen.OverviewImageStorage.Drawing;
                    break;
                case "bones":
                    Node.ImageIndex = (int)DiversityCollection.Specimen.OverviewImageStorage.Bones;
                    break;
                default:
                    Node.ImageIndex = (int)DiversityCollection.Specimen.OverviewImage.Collection;
                    break;
            }
            Node.SelectedImageIndex = Node.ImageIndex;
        }

        private void fillPartHierarchyTable(DataRow[] RR,
            ref DataTable DtData, DataTable DtLookup,
            string ColumnID, string ColumnParentID, string ColumnName, string ColumnSecondName)
        {
            foreach (System.Data.DataRow R in RR)
            {
                try
                {
                    System.Data.DataRow[] rCC = DtData.Select(ColumnID + " = " + R[ColumnID].ToString());
                    if (rCC.Length == 0)
                    {
                        foreach (System.Data.DataRow rL in DtLookup.Rows)
                        {
                            if (R[ColumnID].ToString() == rL[ColumnID].ToString())
                            {
                                System.Data.DataRow rn = DtData.NewRow();
                                rn[ColumnID] = R[ColumnID];
                                System.Data.DataRow[] rr = DtLookup.Select(ColumnID + " = " + R[ColumnID].ToString());
                                rn[ColumnName] = rr[0][ColumnName];
                                rn[ColumnParentID] = rr[0][ColumnParentID];
                                if (ColumnSecondName != null)
                                    rn[ColumnSecondName] = rr[0][ColumnSecondName];
                                try { DtData.Rows.Add(rn); }
                                catch { }
                                if (!rn[ColumnParentID].Equals(System.DBNull.Value))
                                {
                                    DataRow[] rrP = DtLookup.Select(ColumnID + " = " + rn[ColumnParentID].ToString());
                                    //this.fillHierarchyTable(rrP, ref DtData, DtLookup, ColumnID, ColumnParentID, ColumnName, ColumnSecondName);
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
        }

        private void OverviewHierarchyCollectionNodeToEnd()
        {
            System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
            this.getOverviewHierarchyNodes(null, "Collection", this.treeViewHierarchy, ref Nodes);
            foreach (System.Windows.Forms.TreeNode N in Nodes)
            {
                if (N.Parent != null)
                {
                    System.Windows.Forms.TreeNode NP = N.Parent;
                    N.Remove();
                    NP.Nodes.Add(N);
                }
            }
        }

        private void addOverviewHierarchyPartDependentData()
        {
            this.addOverviewHierarchyParts();
            //this.addOverviewHierarchyUnitsInPart();
            this.addOverviewHierarchyLoan();
            this.addOverviewHierarchyTransaction();
        }

        private void addDependentData(System.Windows.Forms.TreeNode Node)
        {
            int PartID = 0;
            System.Data.DataRow R = (System.Data.DataRow)Node.Tag;
            string Table = R.Table.TableName;
            switch (Table)
            {
                case "Collection":
                    try
                    {
                        //System.Drawing.Font F = new Font(System.Drawing.FontFamily.GenericSansSerif, (float)8.25, System.Drawing.FontStyle.Bold);
                        foreach (System.Data.DataRow rCS in this.dataSetCollectionSpecimen.CollectionSpecimenPart.Rows)
                        {
                            if (R["CollectionID"].ToString() == rCS["CollectionID"].ToString())
                            {
                                DiversityCollection.HierarchyNode nCS = new HierarchyNode(rCS);
                                this.setPartNodeImage(ref nCS, rCS["MaterialCategory"].ToString());
                                if (this._MaterialCategory == null) this._MaterialCategory = "";
                                if (this._MaterialCategory.Length > 0 && this._CollectionID != null)
                                {
                                    if (rCS["MaterialCategory"].ToString() == this._MaterialCategory
                                        && rCS["CollectionID"].ToString() == this._CollectionID.ToString())
                                        nCS.Checked = true;
                                }
                                //nCS.NodeFont = F;
                                Node.Nodes.Add(nCS);
                                this.addDependentData(nCS);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                    break;
                //case "CollectionSpecimen":
                //    foreach (System.Data.DataRow r in this.dataSetCollectionSpecimen.Tables["CollectionSpecimenPart"].Rows)
                //    {
                //        try
                //        {
                //            if (r.RowState != System.Data.DataRowState.Deleted)
                //            {
                //                if (r["DerivedFromSpecimenPartID"].Equals(System.DBNull.Value))
                //                {
                //                    PartID = System.Int32.Parse(r["SpecimenPartID"].ToString());
                //                    System.Drawing.Font F = new Font(System.Drawing.FontFamily.GenericSansSerif, (float)8.25, System.Drawing.FontStyle.Bold);//
                //                    DiversityCollection.HierarchyNode PartNode = new HierarchyNode(r);
                //                    this.setPartNodeImage(ref PartNode, r["MaterialCategory"].ToString());
                //                    PartNode.NodeFont = F;
                //                    Node.Nodes.Add(PartNode);
                //                    this.addDependentData(PartNode);
                //                }
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                //        }
                //    }
                    //foreach (System.Data.DataRow r in this.dataSetCollectionSpecimen.Tables["CollectionSpecimenProcessing"].Rows)
                    //{
                    //    try
                    //    {
                    //        if (r.RowState != System.Data.DataRowState.Deleted)
                    //        {
                    //            if (r["SpecimenPartID"].Equals(System.DBNull.Value))
                    //            {
                    //                string x = "";
                    //                this.addOverviewHierarchyProcessing(Node);
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    //    }
                    //}
                    //break;
                //case "CollectionSpecimenPart":
                //    PartID = System.Int32.Parse(R["SpecimenPartID"].ToString());
                //    //this.addOverviewHierarchyUnitsInPart(Node, PartID);
                //    //this.addOverviewHierarchyLoan(Node);
                //    //this.addOverviewHierarchyTransaction(Node, PartID);
                //    //this.addOverviewHierarchyPartDependentData(Node);
                //    //if (!this.ShowCollection)
                //    {
                //        try
                //        {
                //            System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.Tables["CollectionSpecimenPart"].Select("DerivedFromSpecimenPartID = " + PartID.ToString());
                //            foreach (System.Data.DataRow r in rr)
                //            {
                //                PartID = System.Int32.Parse(r["SpecimenPartID"].ToString());
                //                System.Drawing.Font F = new Font(System.Drawing.FontFamily.GenericSansSerif, (float)8.25, System.Drawing.FontStyle.Bold);
                //                DiversityCollection.HierarchyNode ChildNode = new HierarchyNode(r);
                //                this.setPartNodeImage(ref ChildNode, r["MaterialCategory"].ToString());
                //                ChildNode.NodeFont = F;
                //                Node.Nodes.Add(ChildNode);
                //                //this.addDependentData(ChildNode);
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                //        }
                //    }
                //    break;
            }
        }

        private void addOverviewHierarchyPartDependentData(System.Windows.Forms.TreeNode Node, int PartID)
        {
            //this.addOverviewHierarchyUnitsInPart(Node, PartID);
            this.addOverviewHierarchyLoan(Node);
            this.addOverviewHierarchyTransaction(Node, PartID);
            this.addOverviewHierarchyParts(Node, PartID);
        }

        #region Hiding the nodes
        private void hideOverviewHierarchyStorageNodes(string Table)
        {
            System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
            this.getOverviewHierarchyNodes(null, Table, this.treeViewHierarchy, ref Nodes);
            foreach (System.Windows.Forms.TreeNode N in Nodes)
                N.Remove();
        }

        private void hideOverviewHierarchyStorageNodes(string Table, string ConditionColumn, string Condition)
        {
            System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
            this.getOverviewHierarchyNodes(null, Table, this.treeViewHierarchy, ref Nodes);
            foreach (System.Windows.Forms.TreeNode N in Nodes)
            {
                System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                if (R[ConditionColumn].ToString() == Condition)
                    N.Remove();
            }
        }

        #endregion

        #endregion

        #region Collection
        private void addChildCollectionNodes(System.Windows.Forms.TreeNode Node, DataTable DtCollections)
        {
            if (Node.Tag != null)
            {
                System.Data.DataRow RN = (System.Data.DataRow)Node.Tag;
                foreach (System.Data.DataRow R in DtCollections.Rows)
                {
                    try
                    {
                        if (R["CollectionParentID"].ToString() == RN["CollectionID"].ToString())
                        {
                            DiversityCollection.HierarchyNode N = new HierarchyNode(R);
                            N.ImageIndex = (int)DiversityCollection.Specimen.OverviewImage.Collection;
                            N.SelectedImageIndex = (int)DiversityCollection.Specimen.OverviewImage.Collection;
                            N.Tag = R;
                            Node.Nodes.Add(N);
                            this.addChildCollectionNodes(N, DtCollections);
                            this.addDependentData(N);
                            //this.addPartHierarchyDataNodes(N);
                        }

                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
        }

        #endregion

        #region Part nodes
        private void OverviewHierarchyPartNodesToEnd()
        {
            System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
            this.getOverviewHierarchyNodes(null, "CollectionSpecimenPart", this.treeViewHierarchy, ref Nodes);
            foreach (System.Windows.Forms.TreeNode N in Nodes)
            {
                System.Windows.Forms.TreeNode NP = N.Parent;
                N.Remove();
                NP.Nodes.Add(N);
            }
        }

        private void addOverviewHierarchyParts()
        {
            System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
            this.getOverviewHierarchyNodes(null, "CollectionSpecimen", this.treeViewHierarchy, ref Nodes);
            if (Nodes.Count > 0)
            {
                foreach (System.Data.DataRow r in this.dataSetCollectionSpecimen.Tables["CollectionSpecimenPart"].Rows)
                {
                    try
                    {
                        if (r.RowState != System.Data.DataRowState.Deleted)
                        {
                            if (r["DerivedFromSpecimenPartID"].Equals(System.DBNull.Value))
                            {
                                int PartID = System.Int32.Parse(r["SpecimenPartID"].ToString());
                                System.Drawing.Font F = new Font(System.Drawing.FontFamily.GenericSansSerif, (float)8.25, System.Drawing.FontStyle.Bold);//
                                DiversityCollection.HierarchyNode Node = new HierarchyNode(r);
                                this.setPartNodeImage(ref Node, r["MaterialCategory"].ToString());
                                Node.NodeFont = F;
                                Nodes[0].Nodes.Add(Node);
                                this.addOverviewHierarchyPartDependentData(Node, PartID);
                                //this.addOverviewHierarchyParts(Node, PartID);
                                //Nodes[0].Expand();
                                //Node.Expand();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
            //this.addOverviewHierarchyUnitsInPart();
            //this.addOverviewHierarchyProcessing();
            //this.addOverviewHierarchyTransaction();
        }

        //private void addOverviewHierarchyParts(System.Windows.Forms.TreeNode Node)
        //{
        //    System.Data.DataRow R = (System.Data.DataRow)Node.Tag;
        //    string Table = R.Table.TableName;
        //    foreach (System.Data.DataRow r in this.dataSetCollectionSpecimen.Tables["CollectionSpecimenPart"].Rows)
        //    {
        //        try
        //        {
        //            if (r.RowState != System.Data.DataRowState.Deleted)
        //            {
        //                switch (Table)
        //                {
        //                    case "CollectionSpecimenPart":
        //                        break;
        //                    case "CollectionSpecimenPart":
        //                        break;
        //                    case "CollectionSpecimenPart":
        //                        break;
        //                }
        //                if (r["DerivedFromSpecimenPartID"].Equals(System.DBNull.Value))
        //                {
        //                    int PartID = System.Int32.Parse(r["SpecimenPartID"].ToString());
        //                    System.Drawing.Font F = new Font(System.Drawing.FontFamily.GenericSansSerif, (float)8.25, System.Drawing.FontStyle.Bold);//
        //                    DiversityCollection.HierarchyNode Node = new HierarchyNode(r);
        //                    this.setPartNodeImage(ref Node, r["MaterialCategory"].ToString());
        //                    Node.NodeFont = F;
        //                    Nodes[0].Nodes.Add(Node);
        //                    this.addOverviewHierarchyPartDependentData(Node, PartID);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //        }
        //    }
        //}

        private void addOverviewHierarchyParts(System.Windows.Forms.TreeNode pNode, int SpecimenPartID)
        {
            try
            {
                System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.Tables["CollectionSpecimenPart"].Select("DerivedFromSpecimenPartID = " + SpecimenPartID.ToString());
                foreach (System.Data.DataRow r in rr)
                {
                    int PartID = System.Int32.Parse(r["SpecimenPartID"].ToString());
                    System.Drawing.Font F = new Font(System.Drawing.FontFamily.GenericSansSerif, (float)8.25, System.Drawing.FontStyle.Bold);
                    DiversityCollection.HierarchyNode Node = new HierarchyNode(r);
                    this.setPartNodeImage(ref Node, r["MaterialCategory"].ToString());
                    Node.NodeFont = F;
                    pNode.Nodes.Add(Node);
                    this.addOverviewHierarchyPartDependentData(Node, PartID);
                    //this.addOverviewHierarchyUnitsInPart(Node, PartID);
                    //this.addOverviewHierarchyLoan(Node);
                    //this.addOverviewHierarchyTransaction(Node, PartID);
                    //this.addOverviewHierarchyProcessing(Node);
                    //this.addOverviewHierarchyParts(Node, PartID);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void refreshHierarchyPartUnitNodes(System.Windows.Forms.TreeNode Node, string Table)
        {
            try
            {
                this.treeViewHierarchy.Visible = false;
                System.Windows.Forms.TreeNode NodeToRefresh;
                System.Data.DataRow R = (System.Data.DataRow)Node.Tag;
                //string TableOfNode = R.Table.TableName;
                //int PartID = int.Parse(R["SpecimenPartID"].ToString());
                switch (Table)
                {
                    case "CollectionSpecimenPart":
                        NodeToRefresh = Node;
                        break;
                    case "CollectionSpecimenProcessing":
                    case "CollectionSpecimenTransaction":
                    case "IdentificationUnitInPart":
                        NodeToRefresh = Node.Parent;
                        break;
                    default:
                        return;
                }
                //if (R.Table.TableName == "CollectionSpecimenPart") NodeToRefresh = Node;
                //else if (R.Table.TableName == "IdentificationUnitInPart") NodeToRefresh = Node.Parent;
                //else return;
                if (NodeToRefresh != null)
                {
                    System.Collections.Generic.List<System.Windows.Forms.TreeNode> UnitNodes = new List<TreeNode>();
                    foreach (System.Windows.Forms.TreeNode N in NodeToRefresh.Nodes)
                        UnitNodes.Add(N);
                    foreach (System.Windows.Forms.TreeNode ND in UnitNodes)
                        ND.Remove();
                    this.addDependentData(NodeToRefresh);
                    this.treeViewHierarchy.SelectedNode = NodeToRefresh;
                    this.treeViewHierarchy.ExpandAll();
                    //if (R.Table.TableName == Table)
                    //    this.treeViewOverviewHierarchyStorage.SelectedNode = NodeToRefresh;
                    //else
                    //{
                    //    foreach (System.Windows.Forms.TreeNode NChild in NodeToRefresh.Nodes)
                    //    {
                    //        if (NChild.Tag != null)
                    //        {
                    //            System.Data.DataRow RChild = (System.Data.DataRow)NChild.Tag;
                    //            if (RChild.Table.TableName == Table)
                    //            {
                    //                this.treeViewOverviewHierarchyStorage.SelectedNode = NChild;
                    //                break;
                    //            }
                    //        }
                    //    }
                    //}
                    //this.addOverviewHierarchyPartDependentData(NodeToRefresh, PartID);
                    //this.addOverviewHierarchyUnitsInPart(NodeToRefresh, PartID);
                    //this.addOverviewHierarchyLoan(NodeToRefresh);
                    //this.addOverviewHierarchyTransaction(NodeToRefresh, PartID);
                    //this.addOverviewHierarchyProcessing(NodeToRefresh);
                    //this.addOverviewHierarchyParts(NodeToRefresh, PartID);
                }
            }
            catch { }
            finally { this.treeViewHierarchy.Visible = true; }
        }

        #endregion

        #region Units in part
        //private void addOverviewHierarchyUnitsInPart()
        //{
        //    //if (this.ShowUnitsInPart)
        //    {
        //        LookupTable.DtIdentificationUnit = this.dataSetCollectionSpecimen.IdentificationUnit.Copy();
        //        System.Collections.Generic.List<System.Windows.Forms.TreeNode> PartNodes = new List<TreeNode>();
        //        this.getOverviewHierarchyNodes(null, "CollectionSpecimenPart", this.treeViewOverviewHierarchyStorage, ref PartNodes);
        //        foreach (System.Windows.Forms.TreeNode N in PartNodes)
        //        {
        //            System.Data.DataRow R = (System.Data.DataRow)N.Tag;
        //            int PartID = int.Parse(R["SpecimenPartID"].ToString());
        //            this.addOverviewHierarchyUnitsInPart(N, PartID);
        //        }
        //    }
        //    this.OverviewHierarchyPartNodesToEnd();
        //}

        //private void addOverviewHierarchyUnitsInPart(System.Windows.Forms.TreeNode N, int PartID)
        //{
        //    //if (this.ShowUnitsInPart)
        //    {
        //        System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.IdentificationUnitInPart.Select("SpecimenPartID = " + PartID.ToString(), "DisplayOrder", System.Data.DataViewRowState.CurrentRows);
        //        if (rr.Length > 0)
        //        {
        //            for (int i = 0; i < rr.Length; i++)
        //            {
        //                if (!rr[i]["DisplayOrder"].Equals(System.DBNull.Value) && rr[i]["DisplayOrder"].ToString() != "0")
        //                {
        //                    DiversityCollection.HierarchyNode NI = new HierarchyNode(rr[i]);
        //                    foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtIdentificationUnit.Rows)
        //                    {
        //                        if (rr[i]["IdentificationUnitID"].ToString() == R["IdentificationUnitID"].ToString())
        //                        {
        //                            string TaxonomicGroup = R["TaxonomicGroup"].ToString();
        //                            NI.ImageIndex = DiversityCollection.Specimen.OverviewTaxonImage(TaxonomicGroup, false);
        //                            NI.SelectedImageIndex = DiversityCollection.Specimen.OverviewTaxonImage(TaxonomicGroup, false);
        //                            break;
        //                        }
        //                    }
        //                    N.Nodes.Add(NI);
        //                }
        //            }
        //            for (int i = 0; i < rr.Length; i++)
        //            {
        //                if (rr[i]["DisplayOrder"].Equals(System.DBNull.Value) || rr[i]["DisplayOrder"].ToString() == "0")
        //                {
        //                    DiversityCollection.HierarchyNode NI = new HierarchyNode(rr[i]);
        //                    foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtIdentificationUnit.Rows)
        //                    {
        //                        if (rr[i]["IdentificationUnitID"].ToString() == R["IdentificationUnitID"].ToString())
        //                        {
        //                            string TaxonomicGroup = R["TaxonomicGroup"].ToString();
        //                            NI.ImageIndex = DiversityCollection.Specimen.OverviewTaxonImage(TaxonomicGroup, true);
        //                            NI.SelectedImageIndex = DiversityCollection.Specimen.OverviewTaxonImage(TaxonomicGroup, true);
        //                            break;
        //                        }
        //                    }
        //                    N.Nodes.Add(NI);
        //                }
        //            }
        //        }
        //    }
        //}

        //private void hideOverviewHierarchyUnitsInPart()
        //{
        //    this.hideOverviewHierarchyStorageNodes("IdentificationUnitInPart");
        //}

        #endregion

        #region Transaction
        private void addOverviewHierarchyLoan()
        {
            //if (this.ShowLoan || this.ShowTransaction)
            {
                System.Collections.Generic.List<System.Windows.Forms.TreeNode> PartNodes = new List<TreeNode>();
                this.getOverviewHierarchyNodes(null, "CollectionSpecimenPart", this.treeViewHierarchy, ref PartNodes);
                foreach (System.Windows.Forms.TreeNode N in PartNodes)
                {
                    System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                    this.addOverviewHierarchyLoan(N);
                }
            }
            this.OverviewHierarchyPartNodesToEnd();
        }

        private void addOverviewHierarchyLoan(System.Windows.Forms.TreeNode N)
        {
            //if (this.ShowLoan || this.ShowTransaction)
            {
                System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                foreach (System.Data.DataRow rP in this.dataSetCollectionSpecimen.CollectionSpecimenTransaction.Rows)
                {
                    if (!rP["IsOnLoan"].Equals(System.DBNull.Value))
                    {
                        if (rP["IsOnLoan"].ToString() == "True" && rP["SpecimenPartID"].ToString() == R["SpecimenPartID"].ToString())
                        {
                            DiversityCollection.HierarchyNode NI = new HierarchyNode(rP);
                            NI.ImageIndex = (int)DiversityCollection.Specimen.OverviewImage.Loan;
                            NI.SelectedImageIndex = (int)DiversityCollection.Specimen.OverviewImage.Loan;
                            NI.ForeColor = System.Drawing.Color.Red;
                            N.Nodes.Add(NI);
                        }
                    }
                }
            }
        }

        private void addOverviewHierarchyTransaction()
        {
            //if (!this.ShowLoan) 
                this.addOverviewHierarchyLoan();
            //if (this.ShowTransaction)
            {
                System.Collections.Generic.List<System.Windows.Forms.TreeNode> PartNodes = new List<TreeNode>();
                this.getOverviewHierarchyNodes(null, "CollectionSpecimenPart", this.treeViewHierarchy, ref PartNodes);
                foreach (System.Windows.Forms.TreeNode N in PartNodes)
                {
                    System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                    this.addOverviewHierarchyTransaction(N, 1);
                }
            }
            this.OverviewHierarchyPartNodesToEnd();
        }

        private void addOverviewHierarchyTransaction(System.Windows.Forms.TreeNode N, int PartID)
        {
            //if (this.ShowTransaction)
            {
                System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                foreach (System.Data.DataRow rP in this.dataSetCollectionSpecimen.CollectionSpecimenTransaction.Rows)
                {
                    if (rP["IsOnLoan"].Equals(System.DBNull.Value) || rP["IsOnLoan"].ToString() == "False")
                    {
                        if (rP["SpecimenPartID"].ToString() == R["SpecimenPartID"].ToString())
                        {
                            DiversityCollection.HierarchyNode NI = new HierarchyNode(rP);
                            NI.ImageIndex = (int)DiversityCollection.Specimen.OverviewImage.Transaction;
                            NI.SelectedImageIndex = (int)DiversityCollection.Specimen.OverviewImage.Transaction;
                            N.Nodes.Add(NI);
                        }
                    }
                }
            }
        }

        private void hideOverviewHierarchyLoans()
        {
            this.hideOverviewHierarchyStorageNodes("CollectionSpecimenTransaction", "IsOnLoan", "True");
        }

        private void hideOverviewHierarchyTransactions()
        {
            this.hideOverviewHierarchyStorageNodes("CollectionSpecimenTransaction");
            this.addOverviewHierarchyLoan();
        }

        #endregion

        #endregion

        #endregion

        #region Auxillary

        private void getOverviewHierarchyNodes(System.Windows.Forms.TreeNode Node, string Table,
            System.Windows.Forms.TreeView Treeview,
            ref System.Collections.Generic.List<System.Windows.Forms.TreeNode> TreeNodes)
        {
            if (TreeNodes == null) TreeNodes = new List<TreeNode>();
            if (Node == null)
            {
                foreach (System.Windows.Forms.TreeNode N in Treeview.Nodes)
                {
                    if (N.Tag != null)
                    {
                        try
                        {
                            System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                            if (R.Table.TableName == Table)
                                TreeNodes.Add(N);
                            this.getOverviewHierarchyNodes(N, Table, Treeview, ref TreeNodes);
                        }
                        catch { }
                    }
                }
            }
            else
            {
                foreach (System.Windows.Forms.TreeNode N in Node.Nodes)
                {
                    if (N.Tag != null)
                    {
                        try
                        {
                            System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                            if (R.Table.TableName == Table) TreeNodes.Add(N);
                            this.getOverviewHierarchyNodes(N, Table, Treeview, ref TreeNodes);
                        }
                        catch { }
                    }
                }
            }
        }

        private System.Drawing.Image OverviewTaxonImage(bool IsGrey, string TaxonomicGroup)
        {
            int I = DiversityCollection.Specimen.TaxonImage(TaxonomicGroup, IsGrey);
            System.Drawing.Image Image = DiversityCollection.Specimen.ImageList.Images[I];// this._ImageList.Images[I];
            return Image;
        }

        private System.Drawing.Image OverviewMaterialCategoryImage(bool IsGrey, string MaterialCategory)
        {
            int I = DiversityCollection.Specimen.MaterialCategoryImage(MaterialCategory, IsGrey);
            System.Drawing.Image Image = DiversityCollection.Specimen.ImageList.Images[I];//this._ImageList.Images[I];
            return Image;
        }

        private void setToolStripButtonOverviewHierarchyState(
            System.Windows.Forms.ToolStripButton Button,
            System.Drawing.Image ImageShow,
            System.Drawing.Image ImageHide)
        {
            if (Button.Tag == null)
                Button.Tag = "Hide";
            else
            {
                if (Button.Tag.ToString() == "Hide")
                    Button.Tag = "Show";
                else
                    Button.Tag = "Hide";
            }
            if (Button.Tag.ToString() == "Hide")
                Button.Image = ImageHide;
            else
                Button.Image = ImageShow;
        }

        private void markSelectedNode(System.Windows.Forms.TreeNode Node, System.Windows.Forms.TreeView TreeView)
        {
            System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
            foreach (System.Windows.Forms.TreeNode N in TreeView.Nodes)
            {
                Nodes.Add(N);
                this.getChildNodes(N, ref Nodes);
            }
            foreach (System.Windows.Forms.TreeNode N in Nodes)
                N.BackColor = System.Drawing.SystemColors.Window;
            if (Node != null)
                Node.BackColor = System.Drawing.Color.Yellow;
        }

        private void getChildNodes(System.Windows.Forms.TreeNode Node, ref System.Collections.Generic.List<System.Windows.Forms.TreeNode> NodeList)
        {
            foreach (System.Windows.Forms.TreeNode N in Node.Nodes)
            {
                NodeList.Add(N);
                this.getChildNodes(N, ref NodeList);
            }
        }
        #endregion

        #endregion

        #region Specimen
        //private bool setSpecimen(int SpecimenID)
        //{
        //    bool OK = true;
        //    try
        //    {
        //        if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count > 0)
        //        {
        //            this.updateSpecimen();
        //        }
        //        this.userControlImageSpecimenImage.ImagePath = "";
        //        this.userControlImageEventImage.ImagePath = "";
        //        this.webBrowserEventMap.Url = new Uri("about:blank ");
        //        this.webBrowserLabel.Url = new Uri("about:blank ");
        //        this.fillSpecimen(SpecimenID);
        //        if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count > 0)
        //        {
        //            this.setHeader();
        //            this.splitContainerData.Visible = true;
        //            this.tableLayoutPanelHeader.Visible = true;
        //            this.setTabPageCustomEntryToDefault();
        //        }
        //        else
        //        {
        //            this.splitContainerData.Visible = false;
        //            if (!this.scanModeToolStripMenuItem.Checked)
        //                this.tableLayoutPanelHeader.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return OK;
        //}

        //private bool setSpecimen(string AccessionNumber)
        //{
        //    bool OK = false;
        //    string SQL = "SELECT CollectionSpecimenID_UserAvailable.CollectionSpecimenID " +
        //        "FROM CollectionSpecimen INNER JOIN " +
        //        "CollectionSpecimenID_UserAvailable ON  " +
        //        "CollectionSpecimen.CollectionSpecimenID = CollectionSpecimenID_UserAvailable.CollectionSpecimenID " +
        //        "WHERE (CollectionSpecimen.AccessionNumber = N'" + AccessionNumber + "')";
        //    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
        //    Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
        //    try
        //    {
        //        con.Open();
        //        int ID;
        //        OK = int.TryParse(C.ExecuteScalar()?.ToString(), out ID);
        //        if (!OK)
        //        {
        //            System.Windows.Forms.MessageBox.Show("The " + AccessionNumber + " could not be found in the database");
        //            return OK;
        //        }
        //        //int IDv = int.Parse(C.ExecuteScalar()?.ToString());
        //        con.Close();
        //        OK = this.setSpecimen(ID);
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return OK;
        //}

        //private void setHeader()
        //{
        //    try
        //    {
        //        if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count > 0)
        //        {
        //            this.textBoxHeaderID.Text = this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionSpecimenID"].ToString();
        //            if (!this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["AccessionNumber"].Equals(System.DBNull.Value))
        //                this.textBoxHeaderAccessionNumber.Text = this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["AccessionNumber"].ToString();
        //            else
        //                this.textBoxHeaderAccessionNumber.Text = "";
        //            if (this.dataSetCollectionSpecimen.IdentificationUnit.Rows.Count > 0)
        //            {
        //                System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.IdentificationUnit.Select("DisplayOrder > 0", "DisplayOrder");
        //                System.Data.DataRow r = rr[0];
        //                if (!r["LastIdentificationCache"].Equals(System.DBNull.Value))
        //                    this.textBoxHeaderIdentification.Text = r["LastIdentificationCache"].ToString();
        //                else
        //                    this.textBoxHeaderIdentification.Text = "";
        //            }
        //            else
        //                this.textBoxHeaderIdentification.Text = "";
        //            string Version = this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["Version"].ToString();
        //            if (this.dataSetCollectionSpecimen.CollectionEvent.Rows.Count > 0)
        //                Version += "/" + this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["Version"].ToString();
        //            this.textBoxHeaderVersion.Text = Version;
        //        }
        //        else
        //        {
        //            this.textBoxHeaderID.Text = "";
        //            this.textBoxHeaderAccessionNumber.Text = "";
        //            this.textBoxHeaderIdentification.Text = "";
        //            this.textBoxHeaderVersion.Text = "";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        private void fillSpecimen(int SpecimenID)
        {
            try
            {
                this.dataSetCollectionSpecimen.Clear();
                //this.treeViewExpedition.Nodes.Clear();
                string WhereClause = " WHERE CollectionSpecimenID = " + SpecimenID.ToString() +
                    " AND CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionSpecimenID_UserAvailable)";

                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSpecimen, DiversityCollection.CollectionSpecimen.SqlSpecimen + WhereClause, this.dataSetCollectionSpecimen.CollectionSpecimen);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterPart, DiversityCollection.CollectionSpecimen.SqlPart + WhereClause, this.dataSetCollectionSpecimen.CollectionSpecimenPart);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterTransaction, DiversityCollection.CollectionSpecimen.SqlTransaction + WhereClause, this.dataSetCollectionSpecimen.CollectionSpecimenTransaction);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error setting the specimen", System.Windows.Forms.MessageBoxButtons.OK);
            }
        }


        private void fillSpecimen(string AccessionNumber)
        {
            try
            {
                this.dataSetCollectionSpecimen.Clear();
                string WhereClause = " WHERE AccessionNumber = '" + AccessionNumber + "' OR CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionSpecimenPart WHERE AccessionNumber = '" + AccessionNumber + "')";
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSpecimen, DiversityCollection.CollectionSpecimen.SqlSpecimen + WhereClause, this.dataSetCollectionSpecimen.CollectionSpecimen);
                WhereClause = " WHERE AccessionNumber = '" + AccessionNumber + "' OR CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionSpecimen WHERE AccessionNumber = '" + AccessionNumber + "')";
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterPart, DiversityCollection.CollectionSpecimen.SqlPart + WhereClause, this.dataSetCollectionSpecimen.CollectionSpecimenPart);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error setting the specimen", System.Windows.Forms.MessageBoxButtons.OK);
            }
        }

        //private void fillSpecimen(string WhereClause)
        //{
        //    try
        //    {
        //        this.dataSetCollectionSpecimen.Clear();
        //        if (!WhereClause.TrimStart().StartsWith("WHERE ")) WhereClause = " WHERE " + WhereClause;
        //        this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSpecimen, DiversityCollection.CollectionSpecimen.SqlSpecimen + WhereClause, this.dataSetCollectionSpecimen.CollectionSpecimen);
        //        this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterPart, DiversityCollection.CollectionSpecimen.SqlPart + WhereClause, this.dataSetCollectionSpecimen.CollectionSpecimenPart);
        //        //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterTransaction, DiversityCollection.CollectionSpecimen.SqlTransaction + WhereClause, this.dataSetCollectionSpecimen.CollectionSpecimenTransaction);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        System.Windows.Forms.MessageBox.Show(ex.Message, "Error setting the specimen", System.Windows.Forms.MessageBoxButtons.OK);
        //    }
        //}


        #endregion

        #region Properties
        //public DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenTransactionDataTable CollectionSpecimenTransaction
        //{
        //    get 
        //    {
        //        if (this._CollectionSpecimenTransaction == null)
        //        {
        //            this._CollectionSpecimenTransaction = new DataSetCollectionSpecimen.CollectionSpecimenTransactionDataTable();
        //            System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
        //            this.getOverviewHierarchyNodes(null, "CollectionSpecimenPart", this.treeViewHierarchy, ref Nodes);
        //            foreach (System.Windows.Forms.TreeNode T in Nodes)
        //            {
        //                if (T.Checked)
        //                {
        //                    System.Data.DataRow R = (System.Data.DataRow)T.Tag;
        //                    DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenTransactionRow TR = (DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenTransactionRow)this._CollectionSpecimenTransaction.NewRow();
        //                    TR.CollectionSpecimenID = int.Parse(R["CollectionSpecimenID"].ToString());
        //                    TR.SpecimenPartID = int.Parse(R["SpecimenPartID"].ToString());
        //                    TR.TransactionID = -1;
        //                    this._CollectionSpecimenTransaction.Rows.Add(TR);
        //                }
        //            }
        //        }
        //        return this._CollectionSpecimenTransaction; 
        //    } 
        //}

        public DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenPartDataTable CollectionSpecimenPart
        {
            get
            {
                if (this._CollectionSpecimenPart == null)
                {
                    this._CollectionSpecimenPart = new Datasets.DataSetCollectionSpecimen.CollectionSpecimenPartDataTable();
                    System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
                    this.getOverviewHierarchyNodes(null, "CollectionSpecimenPart", this.treeViewHierarchy, ref Nodes);
                    foreach (System.Windows.Forms.TreeNode T in Nodes)
                    {
                        if (T.Checked)
                        {
                            System.Data.DataRow R = (System.Data.DataRow)T.Tag;
                            DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenPartRow PR = (DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenPartRow)this._CollectionSpecimenPart.NewRow();
                            PR.CollectionSpecimenID = int.Parse(R["CollectionSpecimenID"].ToString());
                            PR.SpecimenPartID = int.Parse(R["SpecimenPartID"].ToString());
                            PR.CollectionID = int.Parse(R["CollectionID"].ToString());
                            PR.AccessionNumber = R["AccessionNumber"].ToString();
                            PR.MaterialCategory = R["MaterialCategory"].ToString();
                            this._CollectionSpecimenPart.Rows.Add(PR);
                        }
                    }
                }
                return this._CollectionSpecimenPart;
            }
        }

        private DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get
            {
                if (this._FormFunctions == null)
                    this._FormFunctions = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                return this._FormFunctions;
            }
        }

        #endregion
    }
}