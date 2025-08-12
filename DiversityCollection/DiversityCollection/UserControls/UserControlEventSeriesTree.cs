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
    public partial class UserControlEventSeriesTree : UserControl
    {

        #region Parameter

        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        private System.Collections.Generic.List<int> _IDs;
        private string _sIDs;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterEventSeries;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterEventSeriesEvent;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterEventSeriesSpecimen;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterEventSeriesUnit;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterEventSeriesGeography;

        private System.Drawing.Color _ColorOfNotPresentNodes = System.Drawing.Color.LightGray;
        private System.Drawing.Color _ColorOfNodes = System.Drawing.Color.Black;

        private enum OrderColumn { Locality, Date };
        private OrderColumn _OrderColumn = OrderColumn.Date;

        private System.Data.DataTable _dtGridData;
        /// <summary>
        /// the datatable in the tree corresponding to the data in the main form
        /// </summary>
        private string _TreeDataTable;
        /// <summary>
        /// The name of the PK column of the Data handled in the main form
        /// </summary>
        private string _KeyColumn;

        private System.Windows.Forms.DataGridView _dataGridView;

        private int? _CollectionSpecimenID;

        #endregion

        #region Construction

        public UserControlEventSeriesTree()
        {
            InitializeComponent();

            this.toolStripComboBoxOrderBy.Items.Add(OrderColumn.Date.ToString());
            this.toolStripComboBoxOrderBy.Items.Add(OrderColumn.Locality.ToString());
            this.toolStripComboBoxOrderBy.SelectedIndex = 0;
        }
        
        #endregion

        #region Properties

        private DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get
            {
                if (this._FormFunctions == null)
                {
                    System.Windows.Forms.Form f = (System.Windows.Forms.Form)this.Parent;
                    this._FormFunctions = new DiversityWorkbench.Forms.FormFunctions(f, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                }
                return this._FormFunctions;
            }
            //set
            //{
            //    this._FormFunctions = value;
            //}
        }
        
        #endregion

        #region Interface

        public void initControl(
            System.Collections.Generic.List<int> IDs
            , string TreeDataTable
            , string KeyColumn
            , System.Windows.Forms.DataGridView dataGridView
            , System.Data.DataTable dtGridData
            , System.EventHandler toolStripButtonSearchSpecimen_Click
            )
        {
            this._IDs = IDs;
            this._sIDs = "";
            for (int i = 0; i < _IDs.Count; i++)
            {
                if (i > 0) _sIDs += ", ";
                this._sIDs += _IDs[i].ToString();
            }
            this._TreeDataTable = TreeDataTable;
            if (this._TreeDataTable == "CollectionEventSeries")
            {
                this.toolStripButtonInsertSeries.Visible = true;
                this.toolStripButtonInsertEvent.Visible = true;
            }
            else if (this._TreeDataTable == "CollectionEvent" || this._TreeDataTable == "CollectionEventList")
            {
                this.toolStripButtonInsertSeries.Visible = true;
                this.toolStripButtonInsertEvent.Visible = false;
            }
            else if (this._TreeDataTable == "IdentificationUnitList" || this._TreeDataTable == "IdentificationUnit")
            {
                this.toolStripButtonInsertSeries.Visible = false;
                this.toolStripButtonInsertEvent.Visible = false;
                this.toolStripSeparatorSave.Visible = false;
            }
            else if (this._TreeDataTable == "CollectionSpecimenPartList" || this._TreeDataTable == "CollectionSpecimenPart")
            {
                this.toolStripButtonInsertSeries.Visible = false;
                this.toolStripButtonInsertEvent.Visible = false;
                this.toolStripSeparatorSave.Visible = false;
            }
            this._KeyColumn = KeyColumn;
            this._dataGridView = dataGridView;
            this._dtGridData = dtGridData;
            this.toolStripButtonSearchSpecimen.Click += new System.EventHandler(toolStripButtonSearchSpecimen_Click);
            this.fillDataSetSeries();
            this.initTree();
        }

        public System.Data.DataTable DtGridData { get { return this._dtGridData; } }

        public System.Data.DataTable DtEventSeries { get { return this.dataSetCollectionEventSeries.CollectionEventSeries; } }

        public System.Data.DataTable DtEventSeriesGeography { get { return this.dataSetCollectionEventSeries.CollectionEventSeriesGeography; } }

        public int? CollectionSpecimenID
        {
            get
            {
                return this._CollectionSpecimenID;
            }
        }
        
        #endregion

        #region Tree

        #region Data

        /// <summary>
        /// The data of the collection event series are included in the dataset for the tree and can therefore be updated via the tree.
        /// The other data are just linked in with restricted parts of their information
        /// </summary>
        public void saveDataEventSeries()
        {
            try
            {
                foreach (System.Data.DataRow R in this.dataSetCollectionEventSeries.CollectionEventSeries.Rows)
                {
                    R.EndEdit();
                }
                if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count > 0
                    && this.dataSetCollectionEventSeries.CollectionEventSeries.DataSet.HasChanges())
                {
                    this._sqlDataAdapterEventSeries.Update(this.dataSetCollectionEventSeries.CollectionEventSeries);
                }
                this.saveDataEventSeriesGeography();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void saveDataEventSeriesGeography()
        {
            try
            {
                foreach (System.Data.DataRow R in this.dataSetCollectionEventSeries.CollectionEventSeriesGeography.Rows)
                {
                    R.EndEdit();
                    if (this.dataSetCollectionEventSeries.CollectionEventSeriesGeography.Rows.Count > 0
                        && this.dataSetCollectionEventSeries.CollectionEventSeriesGeography.DataSet.HasChanges())
                    {
                        if (R["Geography"].Equals(System.DBNull.Value) || R["Geography"].ToString().Length == 0)
                            continue;
                        string SQL = "UPDATE CollectionEventSeries SET Geography = geography::STGeomFromText('" + R["Geography"].ToString() + "', 4326) WHERE SeriesID = " + R["Geography"].ToString();
                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void fillDataSetSeries()
        {
            this.saveDataEventSeries();
            // init the sql adapter
            try
            {
                string WhereClause = "";// WHERE SeriesID = 0";
                //this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeries, DiversityCollection.CollectionSpecimen.SqlEventSeries + WhereClause, this.dataSetCollectionEventSeries.CollectionEventSeries);
                // clear the table
                this.dataSetCollectionEventSeries.CollectionEventSeries.Clear();
                // fill tables
                // EventSeries
                string SQL = "SELECT SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, Description, Notes FROM CollectionEventSeries WHERE SeriesID IN(SELECT SeriesID FROM dbo.FirstLinesSeries('" + this._sIDs + "'))";
                if (this.toolStripComboBoxOrderBy.SelectedItem.ToString() == OrderColumn.Date.ToString())
                    SQL += " Order by DateStart";
                else if (this.toolStripComboBoxOrderBy.SelectedItem.ToString() == OrderColumn.Locality.ToString())
                    SQL += " Order by Description";

                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(this.dataSetCollectionEventSeries.CollectionEventSeries);
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeries, this.dataSetCollectionEventSeries.CollectionEventSeries, SQL, DiversityWorkbench.Settings.ConnectionString);
                //this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeries, SQL, this.dataSetCollectionEventSeries.CollectionEventSeries);
                

                // Geography
                SQL = DiversityCollection.CollectionSpecimen.SqlEventSeriesGeography + " WHERE SeriesID IN(SELECT SeriesID FROM dbo.FirstLinesSeries('" + this._sIDs + "'))";
                //DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeriesGeography, this.dataSetCollectionEventSeries.CollectionEventSeriesGeography, SQL, DiversityWorkbench.Settings.ConnectionString);

                // the collection events
                if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count > 0)
                {
                    string SeriesIDs = "";
                    foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionEventSeries.Rows)
                    {
                        if (SeriesIDs.Length > 0) SeriesIDs += ", ";
                        SeriesIDs += r["SeriesID"].ToString();
                    }
                    SQL = DiversityCollection.CollectionSpecimen.SqlEventSeriesEvent + "WHERE SeriesID IN (" + SeriesIDs + ")";
                    //foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionEventSeries.Rows)
                    //{
                    //    SQL += r["SeriesID"].ToString() + ", ";
                    //}
                }
                //if (SQL.EndsWith(", "))
                //    SQL = SQL.Substring(0, SQL.Length - 2) + ") ORDER BY CollectionDate";
                //else
                //    SQL = SQL.Substring(0, SQL.Length - 1) + ") ORDER BY CollectionDate";
                //if (this.toolStripComboBoxOrderBy.SelectedItem.ToString() == OrderColumn.Date.ToString())
                //    SQL += " Order by CollectionDate";
                //else if (this.toolStripComboBoxOrderBy.SelectedItem.ToString() == OrderColumn.Locality.ToString())
                //    SQL += " Order by LocalityDescription";

                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeriesEvent, this.dataSetCollectionEventSeries.CollectionEventList, SQL, DiversityWorkbench.Settings.ConnectionString);

                // Adding the events without a series
                SQL = DiversityCollection.CollectionSpecimen.SqlEventSeriesEvent + "WHERE SeriesID IS NULL " +
                    "AND CollectionEventID IN (SELECT CollectionEventID FROM CollectionSpecimen " +
                    "WHERE NOT CollectionEventID IS NULL AND CollectionSpecimenID IN (" + this._sIDs + "))";
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeriesEvent, this.dataSetCollectionEventSeries.CollectionEventList, SQL, DiversityWorkbench.Settings.ConnectionString);

                if (this.dataSetCollectionEventSeries.CollectionEventList.Rows.Count > 0)
                {
                    // Specimen
                    SQL = DiversityCollection.CollectionSpecimen.SqlEventSeriesSpecimen + " WHERE CollectionEventID IN (";
                    foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionEventList.Rows)
                    {
                        SQL += r["CollectionEventID"].ToString() + ", ";
                    }
                    SQL = SQL.Substring(0, SQL.Length - 2) + ") ORDER BY AccessionNumber";
                    DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeriesSpecimen, this.dataSetCollectionEventSeries.CollectionSpecimenList, SQL, DiversityWorkbench.Settings.ConnectionString);
                }
                // Adding the specimen without an event
                SQL = DiversityCollection.CollectionSpecimen.SqlEventSeriesSpecimen + " WHERE CollectionEventID IS NULL " +
                    "AND CollectionSpecimenID IN (" + this._sIDs + ")";
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeriesSpecimen, this.dataSetCollectionEventSeries.CollectionSpecimenList, SQL, DiversityWorkbench.Settings.ConnectionString);

                // Unit
                if (this.dataSetCollectionEventSeries.CollectionSpecimenList.Rows.Count > 0)
                {
                    SQL = DiversityCollection.CollectionSpecimen.SqlEventSeriesUnit + " WHERE CollectionSpecimenID IN (";
                    foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionSpecimenList.Rows)
                    {
                        SQL += r["CollectionSpecimenID"].ToString() + ", ";
                    }
                    SQL = SQL.Substring(0, SQL.Length - 2) + ")";
                    DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeriesUnit, this.dataSetCollectionEventSeries.IdentificationUnitList, SQL, DiversityWorkbench.Settings.ConnectionString);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            //this.fillEventSeriesImages();
        }

        #endregion

        #region building the tree

        private void initTree()
        {
            this.treeViewData.ImageList = DiversityCollection.Specimen.ImageList;
            this.treeViewData.Visible = false;
            this.treeViewData.Nodes.Clear();
            try
            {
                try
                {
                    if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count > 0)
                    {
                        this.addEventSeries();
                        this.setColorOfNodesToNotPresent();
                        foreach (System.Windows.Forms.TreeNode N in this.PresentNodes)
                            this.setColorOfParentNode(N);
                    }
                    else if (this.dataSetCollectionEventSeries.CollectionEventList.Rows.Count > 0)
                    {
                        this.getEvents();
                        this.getSpecimen();
                        this.addHierarchyUnits();
                    }
                    else if (this.dataSetCollectionEventSeries.CollectionSpecimenList.Rows.Count > 0)
                    {
                        this.getSpecimen();
                        this.addHierarchyUnits();
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                //this.treeViewData.ExpandAll();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.treeViewData.Visible = true;
            }
        }

        private System.Windows.Forms.TreeNode addEventSeriesSuperiorList()
        {
            System.Windows.Forms.TreeNode ParentNode = new TreeNode();
            try
            {
                if (this.NoLoopInEventSeries())
                {
                    foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionEventSeries.Rows)
                    {
                        if (r["SeriesParentID"].Equals(System.DBNull.Value)/* || r["SeriesParentID"].ToString() == r["SeriesID"].ToString()*/)
                        {
                            DiversityCollection.HierarchyNode N = new HierarchyNode(this.CollectionEventSeriesDataRowFromEventDataset(r), false);
                            ParentNode = N;
                            this.treeViewData.Nodes.Add(N);
                            this.getEventSeriesSuperiorChilds(N, ref ParentNode);
                        }
                    }
                }
                else
                {
                    string SeriesID = this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[0]["SeriesID"].ToString();
                    System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesID = " + SeriesID);
                    ParentNode = new HierarchyNode(this.CollectionEventSeriesDataRowFromEventDataset(rr[0]), false);
                    System.Windows.Forms.MessageBox.Show("The event series contains a loop. Please set the series for the collection event");
                    this.treeViewData.Nodes.Add(ParentNode);
                }
            }
            catch { }

            return ParentNode;
        }

        private bool NoLoopInEventSeries()
        {
            bool NoLoop = true;
            System.Data.DataRow RParent;
            System.Data.DataRow RChild;
            if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count > 0)
            {
                System.Data.DataRow[] RR = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesParentID IS NULL");
                if (RR.Length > 0)
                {
                    RParent = RR[0];
                    return true;
                }
                else
                {
                    RParent = this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[0];
                }
                if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count > 1)
                {
                    if (RR.Length > 0)
                    {
                        System.Data.DataRow[] RRChild = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("NOT SeriesParentID IS NULL");
                        if (RRChild.Length > 0)
                            RChild = RRChild[0];
                        else
                            RChild = this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[0];
                    }
                    else
                        RChild = this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[1];
                }
                else
                    RChild = this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[0];
                if (RChild != null && RParent != null)
                    NoLoop = this.NoLoopInEventSeries(RChild, RParent);
            }
            return NoLoop;
        }

        private System.Data.DataRow CollectionEventSeriesDataRowFromEventDataset(System.Data.DataRow DataRowFromSpecimenDataset)
        {
            System.Data.DataRow Rseries = this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[0];
            foreach (System.Data.DataRow R in this.dataSetCollectionEventSeries.CollectionEventSeries.Rows)
            {
                if (R["SeriesID"].ToString() == DataRowFromSpecimenDataset["SeriesID"].ToString())
                {
                    Rseries = R;
                    break;
                }
            }
            return Rseries;
        }

        private void getEventSeriesSuperiorChilds(System.Windows.Forms.TreeNode Node, ref System.Windows.Forms.TreeNode ParentNode)
        {
            try
            {
                System.Data.DataRow rParent = (System.Data.DataRow)Node.Tag;
                string SeriesParentID = rParent["SeriesID"].ToString();
                System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesParentID = " + rParent["SeriesID"].ToString(), "DateStart");
                foreach (System.Data.DataRow rO in rr)
                {
                    foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionEventSeries.Rows)
                    {
                        if (rO["SeriesID"].ToString() == r["SeriesID"].ToString())
                        {
                            DiversityCollection.HierarchyNode N = new HierarchyNode(this.CollectionEventSeriesDataRowFromEventDataset(r), false);
                            Node.Nodes.Add(N);
                            this.getEventSeriesSuperiorChilds(N, ref ParentNode);
                            ParentNode = N;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void addEventSeries()
        {
            foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionEventSeries.Rows)
            {
                if (r["SeriesParentID"].Equals(System.DBNull.Value))
                {
                    DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
                    this.treeViewData.Nodes.Add(N);
                    this.getEventSeriesChilds(N);
                    this.getEventSeriesEvents(N);
                }
            }
            this.getEvents();
            this.getSpecimen();
            this.addHierarchyUnits();
        }

        private void getEventSeriesChilds(System.Windows.Forms.TreeNode Node)
        {
            try
            {
                System.Data.DataRow rParent = (System.Data.DataRow)Node.Tag;
                string SeriesParentID = rParent["SeriesID"].ToString();
                System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesParentID = " + rParent["SeriesID"].ToString(), "DateStart");
                foreach (System.Data.DataRow rO in rr)
                {
                    foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionEventSeries.Rows)
                    {
                        if (rO["SeriesID"].ToString() == r["SeriesID"].ToString())
                        {
                            DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
                            Node.Nodes.Add(N);
                            this.getEventSeriesChilds(N);
                            this.getEventSeriesEvents(N);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void getEventSeriesEvents(System.Windows.Forms.TreeNode Node)
        {
            try
            {
                if (Node != null)
                {
                    System.Data.DataRow rParent = (System.Data.DataRow)Node.Tag;
                    string SeriesID = rParent["SeriesID"].ToString();
                    foreach (System.Data.DataRow r in this.CollectionEventRows)
                    {
                        if (r["SeriesID"].ToString() == SeriesID)
                        {
                            DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
                            Node.Nodes.Add(N);
                            this.getEventSeriesEventSpecimen(N);
                        }
                    }
                }
                else
                {
                    System.Data.DataRow rEvent = this.dataSetCollectionEventSeries.CollectionEventList.Rows[0];
                    DiversityCollection.HierarchyNode N = new HierarchyNode(rEvent, false);
                    this.treeViewData.Nodes.Add(N);
                    this.getEventSeriesEventSpecimen(N);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        System.Data.DataRow[] CollectionEventRows
        {
            get
            {
                string Sort = "CollectionDate";
                if (this.toolStripComboBoxOrderBy.SelectedItem.ToString() == OrderColumn.Locality.ToString())
                    Sort = "DisplayText";
                System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.CollectionEventList.Select("", Sort);
                return rr;
            }
        }

        private void getEvents()
        {
            try
            {
                foreach (System.Data.DataRow r in this.CollectionEventRows)
                {
                    if (r["SeriesID"].Equals(System.DBNull.Value))
                    {
                        DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
                        this.treeViewData.Nodes.Add(N);
                        this.getEventSeriesEventSpecimen(N);
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void getEventSeriesEventSpecimen(System.Windows.Forms.TreeNode Node)
        {
            try
            {
                System.Data.DataRow rParent = (System.Data.DataRow)Node.Tag;
                string CollectionEventID = rParent["CollectionEventID"].ToString();
                foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionSpecimenList.Rows)
                {
                    if (r["CollectionEventID"].ToString() == CollectionEventID)
                    {
                        DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
                        Node.Nodes.Add(N);
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void getSpecimen()
        {
            try
            {
                foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionSpecimenList.Rows)
                {
                    if (r["CollectionEventID"].Equals(System.DBNull.Value))
                    {
                        DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
                        this.treeViewData.Nodes.Add(N);
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void getEventSeriesEventSpecimen(System.Windows.Forms.TreeNode Node, int MainSpecimenID)
        {
            try
            {
                System.Data.DataRow rParent = (System.Data.DataRow)Node.Tag;
                string CollectionEventID = rParent["CollectionEventID"].ToString();
                foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionSpecimenList.Rows)
                {
                    if (r["CollectionEventID"].ToString() == CollectionEventID && r["CollectionSpecimenID"].ToString() != MainSpecimenID.ToString())
                    {
                        DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
                        Node.Nodes.Add(N);
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void getEventSeriesEventSpecimenUnits(System.Windows.Forms.TreeNode Node)
        {
            if (!this.ShowUnit) return;
            try
            {
                if (Node.Nodes.Count > 0)
                {
                    foreach (System.Windows.Forms.TreeNode NChild in Node.Nodes)
                    {
                        System.Data.DataRow RChildNode = (System.Data.DataRow)NChild.Tag;
                        if (RChildNode.Table.TableName == "IdentificationUnitList")
                        {
                            this.getEventSeriesEventSpecimenUnitChilds(NChild);
                            return;
                        }
                    }
                }

                System.Data.DataRow rParent = (System.Data.DataRow)Node.Tag;
                string CollectionSpecimenID = rParent["CollectionSpecimenID"].ToString();
                foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.IdentificationUnitList.Rows)
                {
                    if (r["CollectionSpecimenID"].ToString() == CollectionSpecimenID
                        && r["RelatedUnitID"].Equals(System.DBNull.Value))
                    {
                        DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
                        Node.Nodes.Add(N);
                        this.getEventSeriesEventSpecimenUnitChilds(N);
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void getEventSeriesEventSpecimenUnitChilds(System.Windows.Forms.TreeNode Node)
        {
            if (!this.ShowUnit) return;
            try
            {
                System.Data.DataRow rParent = (System.Data.DataRow)Node.Tag;
                string CollectionSpecimenID = rParent["CollectionSpecimenID"].ToString();
                string SubstrateID = rParent["IdentificationUnitID"].ToString();
                foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.IdentificationUnitList.Rows)
                {
                    if (r["CollectionSpecimenID"].ToString() == CollectionSpecimenID && r["RelatedUnitID"].ToString() == SubstrateID && SubstrateID != null)
                    {
                        DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
                        Node.Nodes.Add(N);
                        this.getEventSeriesEventSpecimenUnitChilds(N);
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Marking the handled nodes in the tree

        private void setColorOfNodesToNotPresent()
        {
            foreach (System.Windows.Forms.TreeNode N in this.treeViewData.Nodes)
            {
                N.ForeColor = this._ColorOfNotPresentNodes;
                this.setColorOfChildNodesToNotPresent(N);
            }
        }

        private void setColorOfChildNodesToNotPresent(System.Windows.Forms.TreeNode N)
        {
            foreach (System.Windows.Forms.TreeNode NChild in N.Nodes)
            {
                NChild.ForeColor = this._ColorOfNotPresentNodes;
                this.setColorOfChildNodesToNotPresent(NChild);
            }
        }

        private System.Collections.Generic.List<System.Windows.Forms.TreeNode> PresentNodes
        {
            get
            {
                System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
                foreach (System.Windows.Forms.TreeNode N in this.treeViewData.Nodes)
                    this.addPresentNodesToList(ref Nodes, N);
                return Nodes;
            }
        }

        private void addPresentNodesToList(ref System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes, System.Windows.Forms.TreeNode N)
        {
            if (N.Tag != null)
            {
                System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                if (R.Table.TableName == this._TreeDataTable
                    || R.Table.TableName == this._TreeDataTable + "List")
                {
                    string ID = R[this._KeyColumn].ToString();
                    if (this._dtGridData.Rows.Count == 0
                        && this._TreeDataTable == "CollectionEventSeries"
                        && R.Table.TableName == "CollectionEventSeries")
                    {
                        System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.CollectionEventSeries.Select(this._KeyColumn + " = " + ID);
                        if (rr.Length > 0)
                            Nodes.Add(N);
                    }
                    else
                    {
                        System.Data.DataRow[] rr = _dtGridData.Select(this._KeyColumn + " = " + ID);
                        if (rr.Length > 0)
                            Nodes.Add(N);
                    }
                }
            }
            if (N.Nodes.Count > 0)
            {
                foreach (System.Windows.Forms.TreeNode Nchild in N.Nodes)
                    this.addPresentNodesToList(ref Nodes, Nchild);
            }
        }

        private void setColorOfParentNode(System.Windows.Forms.TreeNode N)
        {
            N.ForeColor = this._ColorOfNodes;
            N.Expand();
            if (N.Parent != null)
                this.setColorOfParentNode(N.Parent);
        }

        #endregion

        //private void replaceEventListNode()
        //{
        //    System.Collections.Generic.List<System.Windows.Forms.TreeNode> EventNodes = new List<TreeNode>();
        //    this.getHierarchyNodes(null, "CollectionEventList", this.treeViewData, ref EventNodes);
        //    this.treeViewData.CollapseAll();
        //    foreach (System.Windows.Forms.TreeNode N in EventNodes)
        //    {
        //        System.Data.DataRow R = (System.Data.DataRow)N.Tag;
        //        if (R["CollectionEventID"].ToString() == this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].ToString())
        //        {
        //            System.Windows.Forms.TreeNode ParentNode = N.Parent;
        //            N.Remove();
        //            System.Windows.Forms.TreeNode EventNode = this.OverviewHierarchyEventNode;
        //            ParentNode.Nodes.Add(EventNode);
        //            System.Windows.Forms.TreeNode SpecimenNode = this.OverviewHierarchySpecimenNode;
        //            int CollectionSpecimenID = int.Parse(this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionSpecimenID"].ToString());
        //            this.getEventSeriesEventSpecimen(EventNode, CollectionSpecimenID);
        //            EventNode.Nodes.Add(SpecimenNode);
        //            SpecimenNode.Expand();
        //            this.treeViewData.SelectedNode = SpecimenNode;
        //            this.treeViewData.SelectedNode = null;
        //            EventNode.ExpandAll();
        //            //ParentNode.ExpandAll();
        //            return;
        //        }
        //    }
        //}

        //private void addOverviewHierarchyEventSeriesHierarchy()
        //{
        //    if (this.ShowEventSeries)
        //    {
        //        //System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
        //        //this.getOverviewHierarchyNodes(null, "CollectionEvent", this.treeViewData, ref Nodes);
        //        //foreach (System.Data.DataRow R in this.dataSetCollectionSpecimen.CollectionEventProperty.Rows)
        //        //{
        //        //    DiversityCollection.HierarchyNode NA = new HierarchyNode(R);
        //        //    NA.ImageIndex = (int)DiversityCollection.Specimen.OverviewImage.EventProperty;
        //        //    NA.SelectedImageIndex = (int)DiversityCollection.Specimen.OverviewImage.EventProperty;
        //        //    NA.ForeColor = System.Drawing.Color.Green;
        //        //    Nodes[0].Nodes.Add(NA);
        //        //}
        //    }
        //}

        //private void addOverviewHierarchyEventSeries()
        //{
        //    if (this.ShowEventSeries)
        //    {
        //        //System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
        //        //this.getOverviewHierarchyNodes(null, "CollectionEvent", this.treeViewData, ref Nodes);
        //        //foreach (System.Data.DataRow R in this.dataSetCollectionSpecimen.CollectionEventProperty.Rows)
        //        //{
        //        //    DiversityCollection.HierarchyNode NA = new HierarchyNode(R);
        //        //    NA.ImageIndex = (int)DiversityCollection.Specimen.OverviewImage.EventProperty;
        //        //    NA.SelectedImageIndex = (int)DiversityCollection.Specimen.OverviewImage.EventProperty;
        //        //    NA.ForeColor = System.Drawing.Color.Green;
        //        //    Nodes[0].Nodes.Add(NA);
        //        //}
        //    }
        //}

        #region Drag & Drop

        private void treeViewData_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent("DiversityCollection.HierarchyNode", false))
                {
                    Point pt = this.treeViewData.PointToClient(new Point(e.X, e.Y));
                    TreeNode ParentNode = this.treeViewData.GetNodeAt(pt);
                    TreeNode ChildNode = (TreeNode)e.Data.GetData("DiversityCollection.HierarchyNode");
                    System.Data.DataRow rChild = (System.Data.DataRow)ChildNode.Tag;
                    System.Data.DataRow rParent = (System.Data.DataRow)ParentNode.Tag;
                    string ChildTable = rChild.Table.TableName;
                    string ParentTable = rParent.Table.TableName;
                    string SQL = "";
                    Microsoft.Data.SqlClient.SqlConnection c = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, c);
                    switch (ChildTable)
                    {
                        case "CollectionEventSeries":
                            if (ParentTable == "CollectionEventSeries")
                            {
                                if (this.NoLoopInEventSeries(rChild, rParent))
                                {
                                    rChild["SeriesParentID"] = rParent["SeriesID"];
                                    this.initTree();
                                }
                                else
                                    System.Windows.Forms.MessageBox.Show("This would create a loop in the event series", "Loop", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("Event series can only be placed within other event series");
                                return;
                            }
                            break;
                        case "CollectionSpecimenList":
                            if (ParentTable == "CollectionEvent"
                                || ParentTable == "CollectionEventList")
                            {
                                SQL = "UPDATE CollectionSpecimen SET CollectionEventID  = " + rParent["CollectionEventID"].ToString() + " WHERE CollectionSpecimenID = " + rChild["CollectionSpecimenID"].ToString();
                                cmd.CommandText = SQL;
                                c.Open();
                                cmd.ExecuteNonQuery();
                                c.Close();
                                //this.setSpecimen(this.ID);
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("Specimens can only be placed within collection events");
                                return;
                            }
                            break;
                        case "CollectionEventList":
                            if (ParentTable == "CollectionEventSeries")
                            {
                                rChild["SeriesID"] = rParent["SeriesID"];
                                SQL = "UPDATE CollectionEvent SET SeriesID  = " + rParent["SeriesID"].ToString() + " WHERE CollectionEventID = " + rChild["CollectionEventID"].ToString();
                                cmd.CommandText = SQL;
                                c.Open();
                                cmd.ExecuteNonQuery();
                                c.Close();
                                //this.setSpecimen(this.ID);
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("Collection events can only be placed within event series");
                                //this.buildEventSeriesHierarchy();
                                //this.setSpecimen(this.ID);
                                return;
                            }
                            break;
                        //case "CollectionEventSeries":
                        //    if (ParentTable == "CollectionEventSeries")
                        //    {
                        //        SQL = "UPDATE CollectionEventSeries SET EventSeriesParentID  = " + rParent["SeriesID"].ToString() + " WHERE EventSeriesID = " + rChild["SeriesID"].ToString();
                        //        cmd.CommandText = SQL;
                        //        c.Open();
                        //        cmd.ExecuteNonQuery();
                        //        c.Close();
                        //        //this.buildEventSeriesHierarchy();
                        //        this.setSpecimen(this.ID);
                        //    }
                        //    else
                        //    {
                        //        System.Windows.Forms.MessageBox.Show("EventSeriess / collection event groups can only be placed within EventSeriess / collection event groups");
                        //        //this.buildEventSeriesHierarchy();
                        //        return;
                        //    }
                        //    break;
                        case "IdentificationUnit":
                            int ChildID;
                            int oldChildSubstrateID;
                            int ParentID;
                            //try
                            //{
                            //    if (e.Data.GetDataPresent("DiversityCollection.HierarchyNode", false))
                            //    {
                            //        pt = this.treeViewDataUnitHierarchy.PointToClient(new Point(e.X, e.Y));
                            //        ParentNode = this.treeViewDataUnitHierarchy.GetNodeAt(pt);
                            //        ChildNode = (TreeNode)e.Data.GetData("DiversityCollection.HierarchyNode");
                            if (ParentTable == "IdentificationUnitList")
                            {
                                if (!ParentNode.Equals(ChildNode))
                                {
                                    rChild = (System.Data.DataRow)ChildNode.Tag;
                                    ChildID = System.Int32.Parse(rChild["IdentificationUnitID"].ToString());
                                    if (rChild["RelatedUnitID"].Equals(System.DBNull.Value)) oldChildSubstrateID = -1;
                                    else oldChildSubstrateID = System.Int32.Parse(rChild["RelatedUnitID"].ToString());
                                    if (ParentNode.Tag != null)
                                    {
                                        rParent = (System.Data.DataRow)ParentNode.Tag;
                                        ParentID = System.Int32.Parse(rParent["IdentificationUnitID"].ToString());
                                        rChild["RelatedUnitID"] = ParentID;
                                    }
                                    else rChild["RelatedUnitID"] = System.DBNull.Value;
                                    System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.Tables["IdentificationUnit"].Select("RelatedUnitID = " + ChildID.ToString());
                                    foreach (System.Data.DataRow r in rr)
                                    {
                                        if (oldChildSubstrateID > -1) r["RelatedUnitID"] = oldChildSubstrateID;
                                        else r["RelatedUnitID"] = System.DBNull.Value;
                                    }
                                    //this.buildUnitHierarchy();
                                }
                            }
                            else if (ParentTable == "CollectionSpecimen")
                            {
                                rChild = (System.Data.DataRow)ChildNode.Tag;
                                rChild["RelatedUnitID"] = System.DBNull.Value;
                            }
                            //    }
                            //}
                            //catch (Exception ex)
                            //{
                            //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            //}
                            break;
                        default:
                            System.Windows.Forms.MessageBox.Show("Only event series, collection events, collection speciman and identification units can moved here");
                            //this.buildEventSeriesHierarchy();
                            //return;
                            break;
                    }
                    this.fillDataSetSeries();
                    this.initTree();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private bool NoLoopInEventSeries(System.Data.DataRow rChild, System.Data.DataRow rParent)
        {
            bool NoLoop = true;
            try
            {
                int ChildID = int.Parse(rChild["SeriesID"].ToString());
                int ParentID = int.Parse(rParent["SeriesID"].ToString());
                if (ChildID == ParentID)
                    return false;
                int? ParentOfParentID = null;
                int iPP = 0;
                if (int.TryParse(rParent["SeriesParentID"].ToString(), out iPP))
                    ParentOfParentID = iPP;
                System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesID = " + ParentID);
                if (rr.Length > 0)
                {
                    if (!rr[0]["SeriesParentID"].Equals(System.DBNull.Value))
                    {
                        while (ParentOfParentID != null)
                        {
                            if (ParentOfParentID == ChildID)
                            {
                                NoLoop = false;
                                break;
                            }
                            System.Data.DataRow[] RR = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesID = " + ParentOfParentID);
                            if (RR.Length > 0)
                            {
                                if (RR[0]["SeriesParentID"].Equals(System.DBNull.Value))
                                    break;
                                else
                                {
                                    ParentOfParentID = int.Parse(RR[0]["SeriesParentID"].ToString());
                                }
                            }
                            else break;
                        }
                    }
                }
            }
            catch { }
            return NoLoop;
        }

        private void treeViewData_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                System.Windows.Forms.TreeNode tn;
                e.Effect = DragDropEffects.Move;
                TreeView tv = sender as TreeView;
                Point pt = tv.PointToClient(new Point(e.X, e.Y));
                int delta = tv.Height - pt.Y;
                if ((delta < tv.Height / 2) && (delta > 0))
                {
                    tn = tv.GetNodeAt(pt.X, pt.Y);
                    if (tn != null)
                    {
                        if (tn.NextVisibleNode != null)
                            tn.NextVisibleNode.EnsureVisible();
                    }
                }
                if ((delta > tv.Height / 2) && (delta < tv.Height))
                {
                    tn = tv.GetNodeAt(pt.X, pt.Y);
                    if (tn.PrevVisibleNode != null)
                        tn.PrevVisibleNode.EnsureVisible();
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            //System.Windows.Forms.TreeNode N = (System.Windows.Forms.TreeNode)sender;
            //System.Data.DataRow R = (System.Data.DataRow)N.Tag;
            //string Table = R.Table.TableName;
            //switch (Table)
            //{
            //    case "CollectionEventSeries":
            //        try
            //        {
            //            e.Effect = DragDropEffects.Move;
            //            treeViewData tv = sender as treeViewData;
            //            Point pt = tv.PointToClient(new Point(e.X, e.Y));
            //            int delta = tv.Height - pt.Y;
            //            if ((delta < tv.Height / 2) && (delta > 0))
            //            {
            //                TreeNode tn = tv.GetNodeAt(pt.X, pt.Y);
            //                if (tn.NextVisibleNode != null)
            //                    tn.NextVisibleNode.EnsureVisible();
            //            }
            //            if ((delta > tv.Height / 2) && (delta < tv.Height))
            //            {
            //                TreeNode tn = tv.GetNodeAt(pt.X, pt.Y);
            //                if (tn.PrevVisibleNode != null)
            //                    tn.PrevVisibleNode.EnsureVisible();
            //            }

            //        }
            //        catch (Exception ex)
            //        {
            //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //        }
            //        break;
            //    case "IdentificationUnit":
            //        try
            //        {
            //            e.Effect = DragDropEffects.Move;
            //            treeViewData tv = sender as treeViewData;
            //            Point pt = tv.PointToClient(new Point(e.X, e.Y));
            //            int delta = tv.Height - pt.Y;
            //            if ((delta < tv.Height / 2) && (delta > 0))
            //            {
            //                TreeNode tn = tv.GetNodeAt(pt.X, pt.Y);
            //                if (tn.NextVisibleNode != null)
            //                    tn.NextVisibleNode.EnsureVisible();
            //            }
            //            if ((delta > tv.Height / 2) && (delta < tv.Height))
            //            {
            //                TreeNode tn = tv.GetNodeAt(pt.X, pt.Y);
            //                if (tn.PrevVisibleNode != null)
            //                    tn.PrevVisibleNode.EnsureVisible();
            //            }

            //        }
            //        catch (Exception ex)
            //        {
            //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //        }
            //        break;
            //}
        }

        private void treeViewData_ItemDrag(object sender, ItemDragEventArgs e)
        {
            try
            {
                System.Windows.Forms.TreeNode N = (System.Windows.Forms.TreeNode)e.Item;
                System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                string Table = R.Table.TableName;
                switch (Table)
                {
                    case "CollectionEventSeries":
                    case "CollectionEventList":
                    case "CollectionSpecimenList":
                        //case "IdentificationUnit":
                        this.treeViewData.DoDragDrop(e.Item, System.Windows.Forms.DragDropEffects.Move);
                        break;
                }
            }
            catch { }
        }

        #endregion

        #region handling the tree

        private void treeViewData_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.toolStripButtonDeleteItem.Enabled = false;
            try
            {
                foreach (System.Windows.Forms.DataGridViewRow rr in this._dataGridView.Rows)
                {
                    rr.Selected = false;
                }

                if (this.treeViewData.SelectedNode != null
                    && this.treeViewData.SelectedNode.Tag != null)
                {
                    System.Data.DataRow R = (System.Data.DataRow)this.treeViewData.SelectedNode.Tag;
                    string Table = R.Table.TableName;
                    switch (Table)
                    {
                        case "IdentificationUnitList":
                            if (this._TreeDataTable.StartsWith("IdentificationUnit"))
                            {
                                int UnitID;
                                if (int.TryParse(R["IdentificationUnitID"].ToString(), out UnitID))
                                {
                                    foreach (System.Windows.Forms.DataGridViewRow rr in this._dataGridView.Rows)
                                    {
                                        if (rr.Cells[0].Value != null
                                            && rr.Cells[0].Value.ToString() == UnitID.ToString())
                                            rr.Selected = true;
                                    }
                                }
                            }
                            break;
                        case "CollectionSpecimenList":
                            if (this._TreeDataTable.StartsWith("IdentificationUnit"))
                            {
                                int SpecimenID;
                                if (int.TryParse(R["CollectionSpecimenID"].ToString(), out SpecimenID))
                                {
                                    foreach (System.Windows.Forms.DataGridViewRow rr in this._dataGridView.Rows)
                                    {
                                        if (rr.Cells[1].Value != null
                                            && rr.Cells[1].Value.ToString() == SpecimenID.ToString())
                                            rr.Selected = true;
                                    }
                                }
                            }
                            break;
                        case "CollectionEventList":
                            if (this._TreeDataTable.StartsWith("IdentificationUnit"))
                            {
                                int EventID;
                                if (int.TryParse(R["CollectionEventID"].ToString(), out EventID))
                                {
                                    System.Data.DataRow[] rrSp = this.dataSetCollectionEventSeries.CollectionSpecimenList.Select("CollectionEventID = " + EventID.ToString());
                                    if (rrSp.Length > 0)
                                    {
                                        System.Collections.Generic.List<string> ListSpecimenIDs = new List<string>();
                                        foreach (System.Data.DataRow rSp in rrSp)
                                            ListSpecimenIDs.Add(rSp[0].ToString());
                                        foreach (System.Windows.Forms.DataGridViewRow rr in this._dataGridView.Rows)
                                        {
                                            if (rr.Cells[1].Value != null
                                                && ListSpecimenIDs.Contains(rr.Cells[1].Value.ToString()))
                                                rr.Selected = true;
                                        }
                                    }
                                }
                            }
                            else if (this._TreeDataTable == "CollectionEvent")
                            {
                                int EventID;
                                if (int.TryParse(R["CollectionEventID"].ToString(), out EventID))
                                {
                                    System.Data.DataRow[] rrSp = this.dataSetCollectionEventSeries.CollectionEventList.Select("CollectionEventID = " + EventID.ToString());
                                    if (rrSp.Length > 0)
                                    {
                                        System.Collections.Generic.List<string> ListIDs = new List<string>();
                                        foreach (System.Data.DataRow rSp in rrSp)
                                            ListIDs.Add(rSp[0].ToString());
                                        foreach (System.Windows.Forms.DataGridViewRow rr in this._dataGridView.Rows)
                                        {
                                            if (rr.Cells[0].Value != null
                                                && ListIDs.Contains(rr.Cells[0].Value.ToString()))
                                                rr.Selected = true;
                                        }
                                    }
                                    if (this.treeViewData.SelectedNode.Nodes.Count == 0)
                                        this.toolStripButtonDeleteItem.Enabled = true;
                                }
                            }
                            break;
                        case "CollectionEventSeries":
                            if (this._TreeDataTable == "CollectionEventSeries")
                            {
                                int SeriesID;
                                if (int.TryParse(R["SeriesID"].ToString(), out SeriesID))
                                {
                                    System.Data.DataRow[] rrSp = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesID = " + SeriesID.ToString());
                                    if (rrSp.Length > 0)
                                    {
                                        System.Collections.Generic.List<string> ListSpecimenIDs = new List<string>();
                                        foreach (System.Data.DataRow rSp in rrSp)
                                            ListSpecimenIDs.Add(rSp[0].ToString());
                                        foreach (System.Windows.Forms.DataGridViewRow rr in this._dataGridView.Rows)
                                        {
                                            if (rr.Cells[0].Value != null
                                                && ListSpecimenIDs.Contains(rr.Cells[0].Value.ToString()))
                                                rr.Selected = true;
                                        }
                                    }
                                }
                            }
                            else if (this._TreeDataTable == "CollectionEvent")
                            {
                                int SeriesID;
                                if (int.TryParse(R["SeriesID"].ToString(), out SeriesID))
                                {
                                    System.Data.DataRow[] rrSp = this.dataSetCollectionEventSeries.CollectionEventList.Select("SeriesID = " + SeriesID.ToString());
                                    if (rrSp.Length > 0)
                                    {
                                        System.Collections.Generic.List<string> ListIDs = new List<string>();
                                        foreach (System.Data.DataRow rSp in rrSp)
                                            ListIDs.Add(rSp[0].ToString());
                                        foreach (System.Windows.Forms.DataGridViewRow rr in this._dataGridView.Rows)
                                        {
                                            if (rr.Cells[0].Value != null
                                                && ListIDs.Contains(rr.Cells[0].Value.ToString()))
                                                rr.Selected = true;
                                        }
                                    }
                                }
                            }
                            if (this.treeViewData.SelectedNode.Nodes.Count == 0)
                                this.toolStripButtonDeleteItem.Enabled = true;
                            break;
                    }
                    if (R.Table.Columns.Contains("CollectionSpecimenID"))
                    {
                        int i;
                        if (int.TryParse(R["CollectionSpecimenID"].ToString(), out i))
                            this._CollectionSpecimenID = i;
                        else this._CollectionSpecimenID = null;
                    }
                    else this._CollectionSpecimenID = null;
                    if (this._CollectionSpecimenID == null)
                        this.toolStripButtonSearchSpecimen.Enabled = false;
                    else this.toolStripButtonSearchSpecimen.Enabled = true;
                }
                this.toolStripButtonDeleteItem.Visible = false;
                if (this._dataGridView.SelectedRows.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Selected data are not shown in table");
                    if (this._TreeDataTable == "CollectionEvent" && this.toolStripButtonDeleteItem.Enabled)
                    {
                        this.toolStripButtonDeleteItem.Visible = true;
                    }
                    else if (this._TreeDataTable == "CollectionEventSeries")
                    {
                        System.Data.DataRow R = (System.Data.DataRow)this.treeViewData.SelectedNode.Tag;
                        string Table = R.Table.TableName;
                        if (Table == "CollectionEventList" &&
                            this.treeViewData.SelectedNode.Nodes.Count == 0)
                        {
                            this.toolStripButtonDeleteItem.Visible = true;
                            this.toolStripButtonDeleteItem.Enabled = true;
                        }
                    }
                }
            }
            catch { }
        }

        #region Hiding the unit nodes in the tree

        private bool ShowUnit
        {
            get
            {
                if (this.toolStripButtonShowUnit.Tag == null) return true;
                else
                {
                    if (this.toolStripButtonShowUnit.Tag.ToString() == "Show") return true;
                    else return false;
                }
            }
        }

        private void toolStripButtonShowUnit_Click(object sender, EventArgs e)
        {
            this.setToolStripButtonOverviewHierarchyState(
                this.toolStripButtonShowUnit,
                DiversityCollection.Specimen.ImageList.Images[(int)DiversityCollection.Specimen.OverviewImageTaxon.Plant],
                DiversityCollection.Specimen.ImageList.Images[(int)DiversityCollection.Specimen.OverviewImageTaxon.PlantGrey]);
            if (!this.ShowUnit) this.hideHierarchyUnits();
            else
                this.addHierarchyUnits();
        }

        private void addHierarchyUnits()
        {
            if (this.ShowUnit)
            {
                System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
                this.getHierarchyNodes(null, "CollectionSpecimenList", this.treeViewData, ref Nodes);
                if (Nodes.Count > 0)
                {
                    foreach (System.Windows.Forms.TreeNode N in Nodes)
                    {
                        this.getEventSeriesEventSpecimenUnits(N);
                    }
                }
            }
        }

        private void hideHierarchyUnits()
        {
            this.hideHierarchyNodes("IdentificationUnit");
            this.hideHierarchyNodes("IdentificationUnitList");
        }

        private void hideHierarchyNodes(string Table)
        {
            System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
            this.getHierarchyNodes(null, Table, this.treeViewData, ref Nodes);
            foreach (System.Windows.Forms.TreeNode N in Nodes)
                N.Remove();
        }

        private void getHierarchyNodes(System.Windows.Forms.TreeNode Node, string Table,
            System.Windows.Forms.TreeView treeViewData,
            ref System.Collections.Generic.List<System.Windows.Forms.TreeNode> TreeNodes)
        {
            if (TreeNodes == null) TreeNodes = new List<TreeNode>();
            if (Node == null)
            {
                foreach (System.Windows.Forms.TreeNode N in treeViewData.Nodes)
                {
                    if (N.Tag != null)
                    {
                        try
                        {
                            System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                            if (R.Table.TableName == Table)
                                TreeNodes.Add(N);
                            this.getHierarchyNodes(N, Table, treeViewData, ref TreeNodes);
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
                            this.getHierarchyNodes(N, Table, treeViewData, ref TreeNodes);
                        }
                        catch { }
                    }
                }
            }
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
            {
                Button.Image = ImageHide;
                Button.BackColor = System.Drawing.Color.Yellow;
            }
            else
            {
                Button.Image = ImageShow;
                Button.BackColor = System.Drawing.SystemColors.Control;
            }
        }

        #endregion

        //private void toolStripButtonTreeSearch_Click(object sender, EventArgs e)
        //{
        //    if (this.treeViewData.SelectedNode != null)
        //    {
        //        if (this.treeViewData.SelectedNode.Tag != null)
        //        {
        //            try
        //            {
        //                System.Data.DataRow R = (System.Data.DataRow)treeViewData.SelectedNode.Tag;
        //                switch (R.Table.TableName)
        //                {
        //                    case "IdentificationUnitList":
        //                    case "CollectionSpecimenList":
        //                        int ID;
        //                        if (int.TryParse(R["CollectionSpecimenID"].ToString(), out ID))
        //                        {
        //                            this._SpecimenID = ID;
        //                            this.DialogResult = DialogResult.OK;
        //                            this.Close();
        //                        }
        //                        break;
        //                    default:
        //                        System.Windows.Forms.MessageBox.Show("Please select a specimen");
        //                        break;
        //                }
        //            }
        //            catch { }
        //        }
        //    }
        //}

        #endregion

        #endregion

        #region tool strip buttons

        private void toolStripButtonInsertSeries_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Do you want to create a new event series", "New event series", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New series", "Please enter the description of the series", "");
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        string SeriesParentID = "NULL";
                        if (this.treeViewData.SelectedNode != null)
                        {
                            System.Data.DataRow RParent = (System.Data.DataRow)this.treeViewData.SelectedNode.Tag;
                            if (RParent.Table.Columns.Contains("SeriesID"))
                                SeriesParentID = RParent["SeriesID"].ToString();
                        }
                        string SQL = "INSERT INTO CollectionEventSeries " +
                            "(SeriesParentID, Description) " +
                            "VALUES (" + SeriesParentID + ", '" + f.String + "') (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])";
                        int SeriesID = int.Parse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL));
                        SQL = "SELECT SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, Description, Notes FROM CollectionEventSeries WHERE SeriesID = " + SeriesID.ToString();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(this.dataSetCollectionEventSeries.CollectionEventSeries);
                        System.Data.DataRow[] RR = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesID = " + SeriesID.ToString());
                        if (RR.Length > 0)
                        {
                            DiversityCollection.HierarchyNode N = new HierarchyNode(RR[0], false);
                            if (SeriesParentID != "NULL")
                                this.treeViewData.SelectedNode.Nodes.Add(N);
                            else
                                this.treeViewData.Nodes.Add(N);
                        }
                    }
                }
                catch { }
            }

        }

        private void toolStripButtonInsertEvent_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Do you want to create a new collection event", "New event", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New event", "Please enter the locality description of the collection event", "");
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        string SeriesParentID = "NULL";
                        if (this.treeViewData.SelectedNode != null)
                        {
                            System.Data.DataRow RParent = (System.Data.DataRow)this.treeViewData.SelectedNode.Tag;
                            if (RParent.Table.Columns.Contains("SeriesID"))
                                SeriesParentID = RParent["SeriesID"].ToString();
                        }
                        string SQL = "INSERT INTO CollectionEvent " +
                            "(SeriesID, LocalityDescription) " +
                            "VALUES (" + SeriesParentID + ", '" + f.String + "') (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])";
                        int CollectionEventID = int.Parse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL));
                        SQL = DiversityCollection.CollectionSpecimen.SqlEventSeriesEvent + " WHERE CollectionEventID = " + CollectionEventID.ToString();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(this.dataSetCollectionEventSeries.CollectionEventList);
                        System.Data.DataRow[] RR = this.dataSetCollectionEventSeries.CollectionEventList.Select("CollectionEventID = " + CollectionEventID.ToString());
                        if (RR.Length > 0)
                        {
                            DiversityCollection.HierarchyNode N = new HierarchyNode(RR[0], false);
                            if (SeriesParentID != "NULL")
                                this.treeViewData.SelectedNode.Nodes.Add(N);
                            else
                                this.treeViewData.Nodes.Add(N);
                        }
                    }
                }
                catch { }
            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            this.saveDataEventSeries();
            this.fillDataSetSeries();
            this.initTree();
        }
        
        private void toolStripButtonTaxonList_Click(object sender, EventArgs e)
        {
            if (this.treeViewData.SelectedNode == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select an item in the tree");
                return;
            }
            System.Data.DataRow R = (System.Data.DataRow)this.treeViewData.SelectedNode.Tag;
            string Table = R.Table.TableName;
            int ID = 0;
            //DiversityCollection.Forms.FormTaxonList.TaxonListBase TaxonListBase = DiversityCollection.Forms.FormTaxonList.TaxonListBase.Specimen;
            //switch(Table)
            //{
            //    case "CollectionSpecimen":
            //        TaxonListBase = DiversityCollection.Forms.FormTaxonList.TaxonListBase.Specimen;
            //        ID = int.Parse(R["CollectionSpecimenID"].ToString());
            //        break;
            //    case "CollectionEvent":
            //        TaxonListBase = DiversityCollection.Forms.FormTaxonList.TaxonListBase.Event;
            //        ID = int.Parse(R["CollectionEventID"].ToString());
            //        break;
            //    case "CollectionEventSeries":
            //        TaxonListBase = DiversityCollection.Forms.FormTaxonList.TaxonListBase.Series;
            //        ID = int.Parse(R["SeriesID"].ToString());
            //        break;
            //}
            DiversityCollection.Forms.FormTaxonList f = new DiversityCollection.Forms.FormTaxonList(R);
            f.ShowDialog();
        }

        private void toolStripButtonRebuildTree_Click(object sender, EventArgs e)
        {
            this.initTree();
        }

        private void toolStripButtonDeleteItem_Click(object sender, EventArgs e)
        {
            System.Data.DataRow R = (System.Data.DataRow)this.treeViewData.SelectedNode.Tag;
            string Table = R.Table.TableName;
            switch (Table)
            {
                case "CollectionEventList":
                    if (this._TreeDataTable == "CollectionEvent" || this._TreeDataTable == "CollectionEventSeries")
                    {
                        int ID = 0;
                        if (int.TryParse(R["CollectionEventID"].ToString(), out ID))
                        {
                            string Message = "Do you want to delete the selected collection event\r\n" + this.treeViewData.SelectedNode.Text + " ?";
                            if (System.Windows.Forms.MessageBox.Show(Message, "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                return;
                            try
                            {
                                string SQL = "DELETE FROM CollectionEvent WHERE CollectionEventID = " + ID.ToString();
                                Microsoft.Data.SqlClient.SqlConnection Conn = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                                Microsoft.Data.SqlClient.SqlCommand com = new Microsoft.Data.SqlClient.SqlCommand(SQL, Conn);
                                com.CommandType = System.Data.CommandType.Text;
                                Conn.Open();
                                com.ExecuteNonQuery();
                                Conn.Close();
                                System.Windows.Forms.MessageBox.Show("Collection event\r\n" + this.treeViewData.SelectedNode.Text + " has been deleted");
                                System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.CollectionEventList.Select("CollectionEventID = " + ID.ToString());
                                if (rr.Length == 1)
                                {
                                    rr[0].Delete();
                                }
                                this.initTree();
                            }
                            catch (Exception ex)
                            {
                                System.Windows.Forms.MessageBox.Show(ex.Message);
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            }
                        }
                    }
                    break;
                case "CollectionEventSeries":
                    int SeriesID;
                    if (int.TryParse(R["SeriesID"].ToString(), out SeriesID))
                    {
                        //R.Delete();
                        //this._sqlDataAdapterEventSeries.Update(
                    }
                    break;
            }
        }

        #endregion


    }
}
