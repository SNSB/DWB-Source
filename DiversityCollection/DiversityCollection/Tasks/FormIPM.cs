using DiversityCollection.Tasks.Taxa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using static DiversityCollection.Tasks.IPM;

namespace DiversityCollection.Tasks
{
    public partial class FormIPM : Form
    {

        #region Parameter

        private Collection _Collection;
        private System.Data.DataTable _DtCollection;
        private IPM _IPM;
        private string _Date;
        private UserControls.iMainForm _iMainForm;

        private string DateSeleted
        {
            get
            {
                if (_Date == null || _Date.Length == 0)
                    _Date = System.DateTime.Now.ToString("yyyy-MM-dd");
                return _Date;
            }
            set { _Date = value; }
        }

        private string DateSQL
        {
            get
            {
                return "CONVERT(DATETIME, '" + this.DateSeleted + " 00:00:00', 120)";
            }
        }

        #endregion

        #region Construction and form

        public FormIPM(UserControls.iMainForm iMainForm, ref bool OK)
        {
            InitializeComponent();
            this.initForm(iMainForm, ref OK);
        }

        private void FormIPM_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.userControlModuleRelatedEntryResponsible.textBoxValue.Text != Settings.Default.ResponsibleAgent
                || this.userControlModuleRelatedEntryResponsible.labelURI.Text != Settings.Default.ResponsibleAgentURI)
            {
                Settings.Default.ResponsibleAgentURI = this.userControlModuleRelatedEntryResponsible.labelURI.Text;
                Settings.Default.ResponsibleAgent = this.userControlModuleRelatedEntryResponsible.textBoxValue.Text;
            }
            Settings.Default.Save();
            this.DisposeImageControls();
        }

        private bool PreconditionsFullfilled()
        {
            if (!DiversityCollection.Tasks.IPM.InitIPM())
            {
                System.Windows.Forms.MessageBox.Show("Failed to init IPM\r\nPlease turn to your administrator", "Init failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!DiversityCollection.Tasks.IPM.AnyTaxaSelected())
            {
                System.Windows.Forms.MessageBox.Show("No taxa selected", "No taxa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!DiversityCollection.Tasks.IPM.AnyTrapsDefined())
            {
                System.Windows.Forms.MessageBox.Show("No traps defined", "No traps", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private void initForm(UserControls.iMainForm iMainForm, ref bool OK)
        {
            if (!this.PreconditionsFullfilled())
            {
                this.Close();
                return;
            }

            // Show start window
            Forms.FormStarting fs = new Forms.FormStarting(DiversityCollection.Properties.Resources.IPM_256, "IPM");
            fs.setMax(7);
            fs.Show();
            Application.DoEvents();

            try
            {

                this.SuspendLayout();
                fs.ShowCurrentStep("init Form");
                //this.initForm();
                this.buttonCollection.Text = "";
                if (!this.initCollectionID())
                {
                    OK = false;
                    return;
                }

                fs.ShowCurrentStep("init Results");
                this.initGridViewRecordings(IPM.RecordingTarget.TrapPest);
                this.labelPests.Location = new Point(0, 0); // this.DataGridView(IPM.RecordingTarget.TrapPest).ColumnHeadersHeight);
                //this.initGridViewPests();
                fs.ShowCurrentStep("init Bycatch");
                this.initGridViewRecordings(IPM.RecordingTarget.TrapBycatch);
                //this.initGridViewBycatch();
                fs.ShowCurrentStep("init DateRange");
                this.setDateRangeControls();
                fs.ShowCurrentStep("init Collections");
                OK = this.initTreeViewCollection();
                fs.ShowCurrentStep("init Responsible");
                this.initResponsible();
                if (Settings.Default.IncludeCollections)
                {
                    fs.ShowCurrentStep("init Collections");
                    this.initGridViewRecordings(IPM.RecordingTarget.CollectionPest);
                    //this.initGridViewCollections(this.DateSQL);
                }
                if (Settings.Default.IncludeSpecimen)
                {
                    fs.ShowCurrentStep("init Specimens");
                    this.initGridViewRecordings(IPM.RecordingTarget.SpecimenPest);
                    //this.initGridViewSpecimens();
                }
                if (Settings.Default.IncludeTransactions)
                {
                    fs.ShowCurrentStep("init Transactions");
                    this.initGridViewRecordings(IPM.RecordingTarget.TransactionPest);
                    //this.initGridViewTransaction();
                }
                this._iMainForm = iMainForm;
                this.ResumeLayout();
#if !DEBUG
                this.buttonIPMobile.Visible = false;
#endif
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                fs.setEnd();
                fs.Close();
            }
            this.setLanguage();
            this.labelTopCollection.Text = Tasks.FormIPM_Text.TopCollection;
            this.tabPageBeneficials.Text = Tasks.FormIPM_Text.Beneficials;
            this.tabPageCleaning.Text = Tasks.FormIPM_Text.Cleaning;
            this.tabPageImages.Text = Tasks.FormIPM_Text.Images;
            this.dataGridViewResults.Columns[0].HeaderText = Tasks.FormIPM_Text.Group;
            this.labelPreviousDates.Text = Tasks.FormIPM_Text.Inspections;
            this.labelBycatch.Text = Tasks.FormIPM_Text.Bycatch;
            this.labelBycatch.Width = this.dataGridViewResults.Columns[0].Width + 2;
            this.userControlPlan.setEditState(WpfControls.Geometry.UserControlGeometry.State.ReadOnly);
            if (!DiversityCollection.Tasks.Settings.Default.IncludeCollections)
                this.tabControlMonitoring.TabPages.Remove(this.tabPageCollections);
            if (!DiversityCollection.Tasks.Settings.Default.IncludeSpecimen)
                this.tabControlMonitoring.TabPages.Remove(this.tabPageSpecimens);
            if (!DiversityCollection.Tasks.Settings.Default.IncludeTransactions)
                this.tabControlMonitoring.TabPages.Remove(this.tabPageTransaction);
            this.specimenAddToolStripMenuItem.Visible = DiversityCollection.Tasks.Settings.Default.IncludeSpecimen;
            this.collectionAddToolStripMenuItem.Visible = DiversityCollection.Tasks.Settings.Default.IncludeCollections;
            this.menuStripCollection.Visible = DiversityCollection.Tasks.Settings.Default.IncludeSpecimen || DiversityCollection.Tasks.Settings.Default.IncludeCollections;

            this.showPestOnSpecimenToolStripMenuItem.Checked = Settings.Default.IncludeSpecimen;
            this.showPestsOusideTrapsToolStripMenuItem.Checked = Settings.Default.IncludeCollections;
            this.showPestsOnGroupsOfSpecimenToolStripMenuItem.Checked = Settings.Default.IncludeTransactions;
        }

        private void setLanguage()
        {
            if (DiversityWorkbench.Settings.Language != "en-US")
            {
                DiversityWorkbench.Entity.setEntity(this, this.toolTip);
                if (DiversityWorkbench.Settings.Language != System.Globalization.CultureInfo.CurrentUICulture.Name)
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(DiversityWorkbench.Settings.Language);
                }
            }
        }

        //private bool AnyTaxaSelected()
        //{
        //    bool Anything = false;
        //    if (Settings.Default.PestNameUris == null || Settings.Default.PestNameUris.Count == 0)
        //    {
        //        DiversityCollection.Tasks.FormSettings form = new FormSettings("So far no pest taxa have been selected.\r\nPlease select any pests from the list");
        //        form.ShowDialog();
        //        Anything = Settings.Default.PestNameUris != null && Settings.Default.PestNameUris.Count > 0;
        //    }

        //    else Anything = true;
        //    return Anything;
        //}

        //private bool AnyTrapsDefined()
        //{
        //    bool Anything = this.DataTableTraps().Rows.Count > 0;
        //    if (!Anything)
        //    {
        //        System.Windows.Forms.MessageBox.Show("So far no traps have been defined.\r\nPlease define some traps within the collection", "No traps", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        DiversityCollection.FormCollection f = new FormCollection();
        //        f.ShowDialog();
        //        this._CollectionIDList = null;
        //        Anything = this.DataTableTraps().Rows.Count > 0;
        //    }
        //    return Anything;
        //}

        #endregion

        #region common Events for form

        private void buttonIPMobile_Click(object sender, EventArgs e)
        {
            Tasks.FormIPMobile f = new FormIPMobile();
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                "",
                "");
        }

        #endregion

        #region Collection, Hierarchy
        private void buttonCollectionTree_Click(object sender, EventArgs e)
        {
            switch (this.splitContainerMain.Panel1Collapsed)
            {
                case true:
                    this.buttonCollectionTree.Image = DiversityCollection.Resource.ArrowPrevious;
                    this.buttonCollectionTree.BackColor = System.Drawing.Color.White;
                    this.splitContainerMain.Panel1Collapsed = false;
                    break;
                default:
                    this.buttonCollectionTree.Image = DiversityCollection.Resource.ArrowNext;
                    this.buttonCollectionTree.BackColor = System.Drawing.Color.Yellow;
                    this.splitContainerMain.Panel1Collapsed = true;
                    break;
            }
        }

        private void setCollectionHierarchyControls(System.Data.DataRow R, System.Drawing.Image image)
        {
            if(R == (System.Data.DataRow)this.treeViewCollection.Nodes[0].Tag)
            {
                this.buttonSetTopCollection.Visible = false;
                this.buttonSetTopCollection.Tag = null;
            }
            else
            {
                this.buttonSetTopCollection.Tag = R;
                this.buttonSetTopCollection.Visible = true;
                this.buttonSetTopCollection.Image = image;
                this.buttonSetTopCollection.Text = "Set " + R["CollectionName"].ToString();
                this.toolTip.SetToolTip(this.buttonSetTopCollection, "Set " + R["CollectionName"].ToString() + " as top collection in hierarchy");
            }
            //if (this.treeViewCollection.Nodes[0].Tag != null && ((System.Data.DataRow)this.treeViewCollection.Nodes[0].Tag)["CollectionParentID"].Equals(System.DBNull.Value))
            ////if (R["CollectionParentID"].Equals(System.DBNull.Value) || R == this.treeViewCollection.Nodes[0].Tag)
            //{
            //    this.buttonParentCollection.Enabled = false;
            //    this.buttonParentCollection.Tag = null;
            //}
            //else
            //{
            //    this.buttonParentCollection.Enabled = true;
            //    this.buttonParentCollection.Tag = R;
            //}
        }


        #region Hierarchy

        private void buttonParentCollection_Click(object sender, EventArgs e)
        {
            this.setParentCollection();
        }

        private void setParentCollection()
        {
            if (this.buttonParentCollection.Tag != null || Settings.Default.TopCollectionID > -1)
            {
                int CollectionID;
                if (this.buttonParentCollection.Tag != null)
                {
                    System.Data.DataRow R = (System.Data.DataRow)this.buttonParentCollection.Tag;
                    int.TryParse(R["CollectionID"].ToString(), out CollectionID);
                }
                else
                    CollectionID = Settings.Default.TopCollectionID;
                int LocationParentID;
                System.Data.DataRow[] C = DiversityCollection.LookupTable.DtCollection.Select("CollectionID = " + CollectionID.ToString());
                if (int.TryParse(C[0]["LocationParentID"].ToString(), out LocationParentID))
                {
                    this.initControls(LocationParentID);
                    this.buttonSetTopCollection.Visible = false;
                    this.buttonSetTopCollection.Tag = null;
                }
            }
            //else
            //{
            //    if (Settings.Default.TopCollectionID > -1)
            //    {
            //        int CollectionParentID;
            //        System.Data.DataRow[] C = DiversityCollection.LookupTable.DtCollection.Select("CollectionID = " + Settings.Default.TopCollectionID);
            //        if (int.TryParse(C[0]["CollectionParentID"].ToString(), out CollectionParentID))
            //        {
            //            this.initControls(CollectionParentID);
            //            this.buttonSetTopCollection.Visible = false;
            //            this.buttonSetTopCollection.Tag = null;
            //        }
            //    }
            //}
        }

        private void buttonSetTopCollection_Click(object sender, EventArgs e)
        {
            this.setTopCollection();
        }

        private void setTopCollection()
        {
            if (this.buttonSetTopCollection.Tag != null)
            {
                int CollectionID;
                System.Data.DataRow R = (System.Data.DataRow)this.buttonSetTopCollection.Tag;
                if (int.TryParse(R["CollectionID"].ToString(), out CollectionID))
                {
                    this.initControls(CollectionID);
                }
            }
        }

        private void buttonAddCollection_Click(object sender, EventArgs e)
        {
            this.AddCollection();
        }

        private int AddCollection()
        {
            int CollectionID = -1;
            System.Windows.Forms.MessageBox.Show("Available in upcoming version.\r\nPlease use form for administration of collections", "Upcoming", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (CollectionID > -1)
                this.initControls(null);
            return CollectionID;
        }

        #region Collection, IDs

        private int _CurrentCollectionID = -1;

        private bool initCollectionID()
        {
            if (Settings.Default.TopCollectionID == -1 || this.buttonCollection.Text.Length == 0)
            {
                if (Settings.Default.TopCollectionID == -1)
                {
                    System.Windows.Forms.MessageBox.Show("The top collection for IPM has not been defined so far. Please select a collection");
                    return this.setCollectionID();
                }
                else if (this.buttonCollection.Text.Length == 0)
                    return this.setTopCollectionText();
            }
            return true;
        }

        private bool setCollectionID(int? TopCollectionID = null)
        {
            bool OK = false;
            if (TopCollectionID == null)
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                //string SQL = "SELECT C.DisplayText, C.CollectionID FROM [dbo].[CollectionHierarchyAll] () C WHERE /*C.CollectionID = " + Settings.Default.TopCollectionID.ToString() + " AND C.CollectionID IN (SELECT CollectionID FROM dbo.ManagerCollectionList()) ORDER BY DisplayText ";
                string SQL = "SELECT C.DisplayText, C.CollectionID FROM [dbo].[CollectionHierarchyAll] () C WHERE C.CollectionID IN (SELECT CollectionID FROM dbo.ManagerCollectionList()) ORDER BY DisplayText ";
                System.Data.DataTable dt = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                if (dt.Rows.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("You miss the permissions to access collections as a collection manager");
                    return false;
                }
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "DisplayText", "CollectionID", "Top collection", "Please select a collection from the list", "", false, true, true, DiversityCollection.Resource.Collection);
                //DiversityCollection.FormCollection f = new FormCollection(null, null);
                this.Cursor = System.Windows.Forms.Cursors.Default;
                f.ShowInTaskbar = true;
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK && Settings.Default.TopCollectionID != int.Parse(f.SelectedValue))
                {
                    TopCollectionID = int.Parse(f.SelectedValue);
                }
            }
            if (TopCollectionID != null)
            {
                    Settings.Default.TopCollectionID = (int)TopCollectionID;
                    Settings.Default.Save();
                    Tasks.IPM.CollectionIDListReset(); // _CollectionIDList = null;
                    OK = this.setTopCollectionText();
            }
            return OK;
        }

        private bool setTopCollectionText()
        {
            bool OK = false;
            if (Settings.Default.TopCollectionID > -1)
            {
                // Check existance of the ID;
                string SQL = "SELECT COUNT(*) FROM [dbo].[Collection] C WHERE C.CollectionID = " + Settings.Default.TopCollectionID.ToString();
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Result == "0")
                    return this.setCollectionID();

                // Check permissions
                SQL = "SELECT COUNT(*) FROM [dbo].[Collection] C WHERE C.CollectionID = " + Settings.Default.TopCollectionID.ToString() + " AND C.CollectionID IN (SELECT CollectionID FROM dbo.ManagerCollectionList()) ";
                Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
                if (Result == "0")
                    return this.setCollectionID();
                else if (Result.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Missing permission (Collection managers only)", "No permission", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }

                SQL = "SELECT CASE WHEN C.CollectionAcronym <> '' AND C.CollectionAcronym <> C.CollectionName THEN C.CollectionName + ' (' + C.DisplayText + ')' ELSE C.DisplayText END AS Title " +
                    "FROM [dbo].[CollectionHierarchyAll] () C WHERE C.CollectionID = " + Settings.Default.TopCollectionID.ToString() + " AND C.CollectionID IN (SELECT CollectionID FROM dbo.ManagerCollectionList()) ";
                string CollectionName = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);

                //string CollectionName = DiversityCollection.LookupTable.CollectionNameHierarchy(Settings.Default.TopCollectionID);

                if (CollectionName.Length > 0)
                {
                    this.buttonCollection.Text = "      " + CollectionName;
                    this.buttonCollection.Image = DiversityCollection.Specimen.CollectionTypeImage(false, DiversityCollection.LookupTable.CollectionType(Settings.Default.TopCollectionID));
                    OK = true;
                }
                else
                {
                    SQL = "SELECT C.DisplayText FROM [dbo].[CollectionHierarchyAll] () C WHERE C.CollectionID = " + Settings.Default.TopCollectionID.ToString();
                    CollectionName = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
                    if (CollectionName.Length > 0)
                    {
                        string Message = "You miss the proper permissions to access the top collection for IPM\r\n" + CollectionName + "\r\nPlease turn to your administrator";
                        System.Data.DataTable dt = new DataTable();
                        SQL = "SELECT C.DisplayText, C.CollectionID FROM [dbo].[CollectionHierarchyAll] () C WHERE C.CollectionID IN (SELECT CollectionID FROM dbo.ManagerCollectionList()) ORDER BY DisplayText ";
                        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                        if (dt.Rows.Count > 0)
                        {
                            Message += " or select a different collection from the list";
                        }
                        System.Windows.Forms.MessageBox.Show(Message);// );
                        if (dt.Rows.Count > 0)
                        {
                            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "DisplayText", "CollectionID", "Top collection", "Please select a collection from the list", "", false, true, true, DiversityCollection.Resource.Collection);
                            this.Cursor = System.Windows.Forms.Cursors.Default;
                            f.ShowInTaskbar = true;
                            f.ShowDialog();
                            if (f.DialogResult == DialogResult.OK)
                            {
                                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                                this.SuspendLayout();
                                Settings.Default.TopCollectionID = int.Parse(f.SelectedValue);
                                Settings.Default.Save();
                                Tasks.IPM.CollectionIDListReset(); //this._CollectionIDList = null;
                                this.ResumeLayout();
                                this.Cursor = System.Windows.Forms.Cursors.Default;
                                return true;
                            }
                        }
                    }
                    else
                        System.Windows.Forms.MessageBox.Show("The top collection for IPM has not been defined so far. Please select a collection");
                    //this.setCollectionID();
                    OK = false;
                }
            }
            return OK;
        }

        private void buttonCollection_Click(object sender, EventArgs e)
        {
            this.initControls();
        }

        private void initControls(int? TopCollectionID = null)
        {
            this.setCollectionID(TopCollectionID);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.SuspendLayout();
            this.initTreeViewCollection();
            this.initGridViewPests();
            this.initGridViewBycatch();
            this.initGridViewDate();
            this.initGridViewCollections(this.DateSQL);
            this.initGridViewSpecimens();
            this.initGridViewTransaction();
            this.ResumeLayout();

            if (this.treeViewCollection.Nodes[0].Tag != null)
            {
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewCollection.Nodes[0].Tag;
                System.Data.DataRow[] C = DiversityCollection.LookupTable.DtCollection.Select("CollectionID = " + R["CollectionID"].ToString());
                if (C[0]["LocationParentID"].Equals(System.DBNull.Value))
                {
                    this.buttonParentCollection.Enabled = false;
                    this.buttonParentCollection.Tag = null;
                }
                else
                {
                    this.buttonParentCollection.Enabled = true;
                    this.buttonParentCollection.Tag = this.treeViewCollection.Nodes[0].Tag;
                }
            }
            else
            {
                this.buttonParentCollection.Enabled = false;
                this.buttonParentCollection.Tag = null;
            }
            this.buttonSetTopCollection.Visible = TopCollectionID != null;

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        #endregion

        private bool initTreeViewCollection()
        {
            bool OK = true;
            try
            {
                this.treeViewCollection.Nodes.Clear();
                this._DtCollection = DiversityCollection.Tasks.IPM.DtCollection(true);
                System.Data.DataSet dataSet = new DataSet();
                System.Windows.Forms.BindingSource binding = new BindingSource();
                this._Collection = new Collection(ref dataSet, _DtCollection, ref this.treeViewCollection, true, ref binding);
                this._Collection.UseHierarchyNodes = true;
                this._Collection.HierarchyAccordingToLocation = true;
                if (this._Collection != null)
                {
                    System.DateTime currentDate;
                    if (this._Date != null && this._Date.Length > 0 && System.DateTime.TryParse(this._Date, out currentDate))
                    this._Collection.TaskStart = currentDate;
                    this._Collection.SetTaskVisibility(true, Collection.TaskDisplayStyle.RestrictToSameType);
                    this._Collection.buildHierarchy();
                }
                else
                {
                    OK = false;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        private void treeViewCollection_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _CurrentCollectionID = -1;
            this.setDetailStatus(null);
            System.Windows.Forms.TreeNode N = this.treeViewCollection.SelectedNode;
            if (N.Tag != null && N.Tag.GetType() == typeof(System.Data.DataRow))
            {
                System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                this.SetCollectionControls(R, this.treeViewCollection.ImageList.Images[N.ImageIndex]);
                this.setCollectionHierarchyControls(R, this.treeViewCollection.ImageList.Images[N.ImageIndex]);
                int CollectionID;
                if (int.TryParse(R["CollectionID"].ToString(), out CollectionID))
                {
                    _CurrentCollectionID = CollectionID;
                    this.userControlChart.InitChart(CollectionID);
                    this.userControlChart.CollectionID = CollectionID;
                    this.userControlChart.Enabled = true;
                    this.userControlPlan.SetCollectionID(CollectionID);
                    string CollectionType = R["Type"].ToString().ToLower();
                    bool Enable = IPM.CollectionTypeContainingPests.Contains(CollectionType);
                    this.setImages(CollectionID);
                    this.initCleaning(Enable);
                    this.initBeneficials(Enable);
                    this.initTreatments(Enable);
                    this.buttonAddSensor.Visible = CollectionType == "sensor" && this.treeViewCollection.SelectedNode.Nodes.Count == 0;
                }
            }
            else if (N.Tag != null)
            {
                int CollectionTaskID;
                if (int.TryParse(N.Tag.ToString(), out CollectionTaskID))
                {
                    this.userControlChart.InitChart(CollectionTaskID, false);
                    this.userControlChart.CollectionTaskID = CollectionTaskID;
                    this.userControlChart.Enabled = true;
                    int ID;
                    string SQL = "SELECT CollectionID FROM CollectionTask WHERE CollectionTaskID = " + CollectionTaskID.ToString();
                    if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out ID))
                        this.userControlPlan.SetCollectionID(ID);
                    this.setDetailStatus(CollectionTaskID);
                    this.setImages(CollectionTaskID, false);
                    SQL = "SELECT T.Type FROM CollectionTask AS C INNER JOIN Task AS T ON C.TaskID = T.TaskID " +
                        "WHERE C.CollectionTaskID = " + CollectionTaskID.ToString();
                    string Type = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
                    this.buttonAddSensorMetric.Visible = Type.ToLower() == "sensor";
                }
            }
            else
            {
                this.userControlChart.Enabled = false;
                this.userControlPlan.Enabled = false;
                this.setImages(-1);
            }
        }

        private void buttonRemoveTrap_Click(object sender, EventArgs e)
        {
            int ID;
            if (this.treeViewCollection.SelectedNode != null && this.treeViewCollection.SelectedNode.Tag != null && int.TryParse(this.treeViewCollection.SelectedNode.Tag.ToString(), out ID))
            {
                //_Date = System.DateTime.Now.ToString("yyyy-MM-dd");
                //string DateSQL = "CONVERT(DATETIME, '" + _Date + " 00:00:00', 102)";
                string SQL = "UPDATE C SET C.TaskEnd = " + this.DateSQL + " FROM CollectionTask C WHERE CollectionTaskID = " + ID.ToString();
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                this._Collection.buildHierarchy();
            }
        }

        private void buttonImportMetric_Click(object sender, EventArgs e)
        {
            int ID;
            if (buttonImportMetric.Tag != null && int.TryParse(buttonImportMetric.Tag.ToString(), out ID))
            {
                FormPrometheus f = new FormPrometheus(ID);
                f.ShowDialog();
            }
        }

        private System.Windows.Forms.TreeNode getCollectionNode(int CollectionID)
        {
            foreach(System.Windows.Forms.TreeNode node in this.treeViewCollection.Nodes)
            {
                if (node.Tag != null && node.Tag.GetType() == typeof(System.Data.DataRow))
                {
                    System.Data.DataRow R = (System.Data.DataRow)node.Tag;
                    int ID;
                    if (int.TryParse(R["CollectionID"].ToString(), out ID) && ID == CollectionID)
                        return node;
                    else if (this.getCollectionNode(CollectionID, node) != null)
                        return this.getCollectionNode(CollectionID, node);
                }
            }
            return null;
        }

        private System.Windows.Forms.TreeNode getCollectionNode(int CollectionID, System.Windows.Forms.TreeNode N)
        {
            foreach (System.Windows.Forms.TreeNode node in N.Nodes)
            {
                if (node.Tag != null && node.Tag.GetType() == typeof(System.Data.DataRow))
                {
                    System.Data.DataRow R = (System.Data.DataRow)node.Tag;
                    int ID;
                    if (int.TryParse(R["CollectionID"].ToString(), out ID) && ID == CollectionID)
                        return node;
                    else if (this.getCollectionNode(CollectionID, node) != null)
                        return this.getCollectionNode(CollectionID, node);
                }
            }
            return null;
        }

        #endregion

        #endregion

        #region Settings

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                FormSettings f = new FormSettings();
                this.Cursor = System.Windows.Forms.Cursors.Default;
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    this.SuspendLayout();
                    if (f.SelectionHasChanged(RecordingTarget.TrapPest))
                        this.initGridViewRecordings(RecordingTarget.TrapPest, true);
                    else
                        this.initGridViewPests(true);
                    if (f.SelectionHasChanged(RecordingTarget.TrapBycatch))
                        this.initGridViewRecordings(RecordingTarget.TrapBycatch);
                    else
                        this.initGridViewBycatch(true);
                    this.initGridViewCollections(this.DateSQL, true);
                    this.initGridViewSpecimens(true);
                    this.initGridViewTransaction(true);
                    this.ResumeLayout();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #region Settings for the trap grids accessed by the 2 labels located in the upper left corner of the grids

        private void labelPests_Click(object sender, EventArgs e)
        {
            this.setTaxaForGrid(RecordingTarget.TrapPest);
            this.InitGridView_ValueColumns(IPM.RecordingTarget.TrapPest);

            //this.InitGridView_ValueColumns(IPM.RecordingTarget.TrapPest);
            //this.setSourceSettingsForTraps(Tasks.IPM.RecordingTarget.TrapPest, this.dataGridViewResults);
            //this.labelPests.Location = new Point(0, this.dataGridViewResults.ColumnHeadersHeight);
        }

        private void labelBycatch_Click(object sender, EventArgs e)
        {
            this.setSourceSettingsForTraps(Tasks.IPM.RecordingTarget.TrapBycatch, this.dataGridViewBycatch);
        }

        private void setSourceSettingsForTraps(IPM.RecordingTarget recordingTarget, System.Windows.Forms.DataGridView dataGridView)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                System.Collections.Generic.List<IPM.RecordingTarget> recordingTargets = new List<IPM.RecordingTarget>();
                recordingTargets.Add(recordingTarget);
                FormSettings f = new FormSettings(recordingTargets);
                this.Cursor = System.Windows.Forms.Cursors.Default;
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    this.SuspendLayout();
                    if (f.SelectionHasChanged(recordingTarget))
                    {
                        switch (recordingTarget)
                        {
                            case RecordingTarget.TrapPest:
                                this.initGridViewRecordings(RecordingTarget.TrapPest, true);
                                //this.initGridViewPests(true);
                                break;
                            case RecordingTarget.TrapBycatch:
                                this.initGridViewRecordings(RecordingTarget.TrapBycatch, true);
                                //this.initGridViewBycatch(true);
                                break;
                            case RecordingTarget.Beneficial:
                                this.initBeneficials(true);
                                break;
                        }
                    }
                    else
                        this.InitGridView_ValueColumns(recordingTarget);
                    //int HeaderHeight = 0;
                    //this.initGridView(dataGridView, taxonSource, ref HeaderHeight);
                    this.ResumeLayout();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setTaxaForGrid(IPM.RecordingTarget recordingTarget)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                System.Collections.Generic.List<IPM.RecordingTarget> recordingTargets = new List<IPM.RecordingTarget>();
                recordingTargets.Add(recordingTarget);
                FormSettings f = new FormSettings(recordingTargets);
                this.Cursor = System.Windows.Forms.Cursors.Default;
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    this.SuspendLayout();
                    if (f.SelectionHasChanged(recordingTarget))
                    {
                        switch (recordingTarget)
                        {
                            case RecordingTarget.TrapPest:
                                IPM.initTaxaGridViewRows(recordingTarget, this.DataGridView(recordingTarget));
                                IPM.initTaxaGridViewRows(RecordingTarget.TransactionPest, this.DataGridView(RecordingTarget.TransactionPest));
                                IPM.initTaxaGridViewRows(RecordingTarget.CollectionPest, this.DataGridView(RecordingTarget.CollectionPest));
                                IPM.initTaxaGridViewRows(RecordingTarget.SpecimenPest, this.DataGridView(RecordingTarget.SpecimenPest));
                                break;
                            case RecordingTarget.TrapBycatch:
                                IPM.initTaxaGridViewRows(recordingTarget, this.DataGridView(recordingTarget));
                                break;
                            case RecordingTarget.Beneficial:
                                this.initBeneficials(true);
                                break;
                        }
                    }
                    this.ResumeLayout();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #endregion

        #region Grids for monitoring

        private System.Windows.Forms.DataGridView DataGridView(IPM.RecordingTarget recordingTarget)
        {
            switch (recordingTarget)
            {
                case IPM.RecordingTarget.Beneficial:
                    return this.dataGridViewBeneficial;
                case IPM.RecordingTarget.CollectionPest:
                    return this.dataGridViewCollections;
                case IPM.RecordingTarget.TransactionPest:
                    return this.dataGridViewTransaction;
                case IPM.RecordingTarget.SpecimenPest:
                    return this.dataGridViewSpecimens;
                case IPM.RecordingTarget.TrapBycatch:
                    return this.dataGridViewBycatch;
                default:
                    return this.dataGridViewResults;
            }
        }

        #region Synchronize columns of pest and bycatch grids
        private void dataGridViewResults_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            try
            {
                if (this.dataGridViewBycatch.Visible)
                {
                    System.Windows.Forms.DataGridViewColumn C = e.Column;
                    if (this.dataGridViewBycatch.Columns.Count > C.Index)
                        this.dataGridViewBycatch.Columns[C.Index].Width = C.Width;
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void dataGridViewResults_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll && this.dataGridViewBycatch.Visible)
                {
                    this.dataGridViewBycatch.HorizontalScrollingOffset = e.NewValue;
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #endregion

        #region Init the grids

        private void initGridViewRecordings(IPM.RecordingTarget recordingTarget, bool ResetTaxa = false)
        {
            try
            {
                int c = IPM.initTaxaGridViewRows(recordingTarget, this.DataGridView(recordingTarget));
                if (recordingTarget == RecordingTarget.TrapBycatch)
                    this.splitContainerPestAndBycatch.Panel2Collapsed = c == 0;
                if (recordingTarget != RecordingTarget.TrapBycatch || (recordingTarget == RecordingTarget.TrapBycatch && c > 0))
                {
                    this.InitGridView_ValueColumns(recordingTarget);
                    this.ReadRecordings(recordingTarget);
                }
                return;


                this.DataGridView(recordingTarget).SuspendLayout();
                this.DataGridView(recordingTarget).RowHeadersVisible = false;
                this.DataGridView(recordingTarget).AllowUserToAddRows = false;
                this.DataGridView(recordingTarget).AllowUserToDeleteRows = false;

                // removing previous columns for values
                //while (this.DataGridView(recordingTarget).Columns.Count > 3)
                //    this.DataGridView(recordingTarget).Columns.Remove(this.DataGridView(recordingTarget).Columns[3]);

                this.InitGridView_ValueColumns(recordingTarget);//, false);
                DataGridViewCellStyle s = this.DataGridView(recordingTarget).DefaultCellStyle;
                s.WrapMode = DataGridViewTriState.True;
                if (this.DataGridView(recordingTarget).Columns.Count > 0)
                    this.DataGridView(recordingTarget).Columns[0].DefaultCellStyle = s;

                this.DataGridView(recordingTarget).Rows.Clear();
                //IPM iPM = new IPM();
                //DataGridViewCellStyle s = this.DataGridView(recordingTarget).DefaultCellStyle;
                //s.WrapMode = DataGridViewTriState.True;
                //if (this.DataGridView(recordingTarget).Columns.Count > 0)
                //    this.DataGridView(recordingTarget).Columns[0].DefaultCellStyle = s;

                // filling taxa if missing
                //this.FillingMissingRecords(source);

                // filling the grid results
                System.Collections.Generic.Dictionary<string, Taxa.TaxonRecord> Records = Taxa.RecordDicts.ChecklistSelectedRecordings(recordingTarget, ResetTaxa);

                int i = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, Taxa.TaxonRecord> keyValue in Records)
                {
                    if (!this.initGridViewRow(recordingTarget, ref i, keyValue.Key, keyValue.Value, ResetTaxa))
                        continue;
                }
                if (recordingTarget == Tasks.IPM.RecordingTarget.TrapBycatch && this.DataGridView(recordingTarget).Rows.Count > 0)
                {
                    this.DataGridView(recordingTarget).Rows[0].Visible = false;
                    this.splitContainerPestAndBycatch.Panel2Collapsed = this.DataGridView(recordingTarget).Rows.Count < 2;
                }

                this.ReadRecordings(recordingTarget);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.DataGridView(recordingTarget).ResumeLayout();
                this.DataGridView(recordingTarget).AutoResizeColumnHeadersHeight();
            }
        }



        private void initGridViewPests(bool Reset = false)
        {
            if (Reset)
            this.InitGridView_ValueColumns(IPM.RecordingTarget.TrapPest);
            //int HeaderHeight = 0;
            ////this.initGridViewRecords(this.dataGridViewResults, Tasks.IPM.TaxonSource.Pest, ref HeaderHeight, Reset, IPM.MonitoringTarget.Trap, DateSQL);
            //this.labelPests.Location = new Point(0, HeaderHeight);
        }

        private void initGridViewBycatch(bool Reset = false)
        {
            if (Reset) { }
            if (this.setGridviewVisibility(IPM.RecordingTarget.TrapBycatch))
            {
                this.InitGridView_ValueColumns(IPM.RecordingTarget.TrapBycatch);

            }
            //int HeaderHeight = 0;
            //this.initGridViewRecords(this.dataGridViewBycatch, Tasks.IPM.TaxonSource.Bycatch, ref HeaderHeight, Reset, IPM.MonitoringTarget.Trap, DateSQL);

        }

        private void initGridViewCollections(string DateSQL, bool Reset = false)
        {
            if (this.setGridviewVisibility(IPM.RecordingTarget.CollectionPest))
            {
                this.InitGridView_ValueColumns(IPM.RecordingTarget.CollectionPest);

            }
            //if (Settings.Default.IncludeCollections)
            //{
            //    if (!this.tabControlMonitoring.TabPages.Contains(this.tabPageCollections))
            //        this.tabControlMonitoring.TabPages.Add(this.tabPageCollections);
            //}
            //else
            //{
            //    if (this.tabControlMonitoring.TabPages.Contains(this.tabPageCollections))
            //        this.tabControlMonitoring.TabPages.Remove(this.tabPageCollections);
            //}
        }

        private void initGridViewSpecimens(bool Reset = false)
        {
            if (this.setGridviewVisibility(IPM.RecordingTarget.SpecimenPest))
            {
                this.InitGridView_ValueColumns(IPM.RecordingTarget.SpecimenPest);

            }
            //if (!this.tabControlMonitoring.TabPages.Contains(this.tabPageSpecimens))
            //{
            //    this.tabControlMonitoring.TabPages.Add(this.tabPageSpecimens);
            //}
            //else
            //{
            //    if (this.tabControlMonitoring.TabPages.Contains(this.tabPageSpecimens))
            //        this.tabControlMonitoring.TabPages.Remove(this.tabPageSpecimens);
            //}
        }

        private void initGridViewTransaction(bool Reset = false)
        {
            if (this.setGridviewVisibility(IPM.RecordingTarget.TransactionPest))
            {
                this.InitGridView_ValueColumns(IPM.RecordingTarget.TransactionPest);

            }
            //if (!this.tabControlMonitoring.TabPages.Contains(this.tabPageTransaction))
            //{
            //    this.tabControlMonitoring.TabPages.Add(this.tabPageTransaction);
            //}
            //else
            //{
            //    if (this.tabControlMonitoring.TabPages.Contains(this.tabPageTransaction))
            //        this.tabControlMonitoring.TabPages.Remove(this.tabPageTransaction);
            //}
        }

        //private void initGridViewRecords(System.Windows.Forms.DataGridView dataGridView, IPM.TaxonSource source, ref int ColumnHeaderHeight, bool Reset = false, IPM.MonitoringTarget target = Tasks.IPM.MonitoringTarget.Trap, string DateSQL = "")
        //{
        //    try
        //    {
        //        ColumnHeaderHeight = 0;
        //        dataGridView.SuspendLayout();
        //        dataGridView.RowHeadersVisible = false;
        //        dataGridView.AllowUserToAddRows = false;
        //        dataGridView.AllowUserToDeleteRows = false;

        //        dataGridView.Rows.Clear();
        //        //IPM iPM = new IPM();
        //        DataGridViewCellStyle s = dataGridView.DefaultCellStyle;
        //        s.WrapMode = DataGridViewTriState.True;
        //        dataGridView.Columns[0].DefaultCellStyle = s;

        //        // filling taxa if missing
        //        this.FillingMissingRecords(source);

        //        // filling the grid results
        //        System.Collections.Generic.Dictionary<string, Taxa.Record> Records = Taxa.RecordDicts.ChecklistSelectedRecords(source, Reset);

        //        int i = 0;
        //        foreach (System.Collections.Generic.KeyValuePair<string, Taxa.Record> keyValue in Records)
        //        {
        //            if (!this.initGridView(dataGridView, source, ref i, keyValue.Key, keyValue.Value))
        //                continue;
        //        }
        //        if (source == Tasks.IPM.TaxonSource.Bycatch)
        //        {
        //            dataGridView.Rows[0].Visible = false;
        //            this.splitContainerPestAndBycatch.Panel2Collapsed = dataGridView.Rows.Count < 2;
        //        }

        //        // removing previous columns for traps
        //        while (dataGridView.Columns.Count > 3)
        //            dataGridView.Columns.Remove(dataGridView.Columns[3]);
        //        this.InitGrids(dataGridView, source, ref ColumnHeaderHeight, DateSQL, target);
        //        //switch (target)
        //        //{
        //        //    case Tasks.IPM.MonitoringTarget.Trap:
        //        //        this.InitGrids(dataGridView, source, ref ColumnHeaderHeight, DateSQL);
        //        //        //this.InitTrapGrids(dataGridView, source, ref ColumnHeaderHeight);
        //        //        break;
        //        //    case Tasks.IPM.MonitoringTarget.Collection:
        //        //        this.InitGrids(dataGridView, source, ref ColumnHeaderHeight, DateSQL);
        //        //        //this.InitCollectionGrids(dataGridView, source, ref ColumnHeaderHeight, DateSQL);
        //        //        break;
        //        //    case Tasks.IPM.MonitoringTarget.Specimen:
        //        //        this.InitGrids(dataGridView, source, ref ColumnHeaderHeight, DateSQL);
        //        //        break;
        //        //    case Tasks.IPM.MonitoringTarget.Transaction:
        //        //        this.InitGrids(dataGridView, source, ref ColumnHeaderHeight, DateSQL);
        //        //        //this.InitTransactionGrids(dataGridView, source, ref ColumnHeaderHeight, DateSQL);
        //        //        break;
        //        //}
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    finally
        //    {
        //        dataGridView.ResumeLayout();
        //    }
        //}

        //private void InitTrapGrids(System.Windows.Forms.DataGridView dataGridView, IPM.TaxonSource source, ref int ColumnHeaderHeight)
        //{
        //    try
        //    {
        //        System.Data.DataTable dataTable = DiversityCollection.Tasks.IPM.DataTableTraps(); // new DataTable();
        //        DataGridViewCellStyle cellStyle = new DataGridViewCellStyle(dataGridViewResults.DefaultCellStyle);
        //        cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //        System.Drawing.Font F = new Font(cellStyle.Font, FontStyle.Bold);
        //        cellStyle.Font = F;
        //        int ColumnHeaderAddedLineHeight = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(13);
        //        if (dataGridView.Rows.Count > 0)
        //        {
        //            foreach (System.Data.DataRow R in dataTable.Rows)
        //            {
        //                try
        //                {
        //                    System.Windows.Forms.DataGridViewColumn C = new DataGridViewColumn(dataGridView.Rows[0].Cells[0]);
        //                    C.DefaultCellStyle = cellStyle;
        //                    C.Tag = R[0].ToString();
        //                    int h = 0;
        //                    switch (source)
        //                    {
        //                        case Tasks.IPM.TaxonSource.Pest:
        //                            C.HeaderText = R[1].ToString().Replace(" | ", "\r\n");
        //                            foreach (char c in R[1].ToString()) { if (c == '|') h++; }
        //                            break;
        //                    }
        //                    dataGridView.Columns.Add(C);
        //                    if (h > 0)
        //                    {
        //                        int DefaultHeight = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(23);
        //                        int NewHeight = h * DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(13);
        //                        if (ColumnHeaderHeight < (DefaultHeight + NewHeight))
        //                            ColumnHeaderHeight = DefaultHeight + NewHeight;
        //                    }
        //                }
        //                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //            }
        //        }

        //        // reading the values
        //        try
        //        {
        //            switch (source)
        //            {
        //                case Tasks.IPM.TaxonSource.Bycatch:
        //                    this.ReadBycatchValues();
        //                    if (dataGridView.Columns.Count > 2)
        //                    {
        //                        for (int c = 2; c < dataGridView.Columns.Count; c++)
        //                        {
        //                            dataGridView.Columns[c].Width = this.dataGridViewResults.Columns[c].Width;
        //                        }
        //                    }
        //                    break;
        //                case Tasks.IPM.TaxonSource.Pest:
        //                    this.ReadPestValues(this.dataGridViewResults);
        //                    if (dataGridView.Columns.Count > 2)
        //                    {
        //                        for (int c = 2; c < dataGridView.Columns.Count; c++)
        //                        {
        //                            dataGridView.Columns[c].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
        //                        }
        //                    }
        //                    break;
        //            }
        //        }
        //        catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //    }
        //    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //}

        //private void InitCollectionGrids(System.Windows.Forms.DataGridView dataGridView, IPM.TaxonSource source, ref int ColumnHeaderHeight, string DateSQL)
        //{
        //    try
        //    {
        //        System.Data.DataTable dataTable = Tasks.IPM.DataTableCollections(DateSQL); // new DataTable();
        //        DataGridViewCellStyle cellStyle = new DataGridViewCellStyle(this.dataGridViewCollections.DefaultCellStyle);
        //        cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //        System.Drawing.Font F = new Font(cellStyle.Font, FontStyle.Bold);
        //        cellStyle.Font = F;
        //        int ColumnHeaderAddedLineHeight = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(13);
        //        if (dataGridView.Rows.Count > 0)
        //        {
        //            foreach (System.Data.DataRow R in dataTable.Rows)
        //            {
        //                try
        //                {
        //                    System.Windows.Forms.DataGridViewColumn C = new DataGridViewColumn(dataGridView.Rows[0].Cells[0]);
        //                    C.DefaultCellStyle = cellStyle;
        //                    C.Tag = R[0].ToString();
        //                    int h = 0;
        //                    switch (source)
        //                    {
        //                        case Tasks.IPM.TaxonSource.Pest:
        //                            C.HeaderText = R[1].ToString().Replace(" | ", "\r\n");
        //                            foreach (char c in R[1].ToString()) { if (c == '|') h++; }
        //                            break;
        //                    }
        //                    dataGridView.Columns.Add(C);
        //                    if (h > 0)
        //                    {
        //                        int DefaultHeight = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(23);
        //                        int NewHeight = h * DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(13);
        //                        if (ColumnHeaderHeight < (DefaultHeight + NewHeight))
        //                            ColumnHeaderHeight = DefaultHeight + NewHeight;
        //                    }
        //                }
        //                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //            }
        //        }

        //        // reading the values
        //        try
        //        {
        //            switch (source)
        //            {
        //                case Tasks.IPM.TaxonSource.Bycatch:
        //                    this.ReadBycatchValues();
        //                    if (dataGridView.Columns.Count > 2)
        //                    {
        //                        for (int c = 2; c < dataGridView.Columns.Count; c++)
        //                        {
        //                            dataGridView.Columns[c].Width = this.dataGridViewCollections.Columns[c].Width;
        //                        }
        //                    }
        //                    break;
        //                case Tasks.IPM.TaxonSource.Pest:
        //                    this.ReadPestValues(this.dataGridViewCollections);
        //                    if (dataGridView.Columns.Count > 2)
        //                    {
        //                        for (int c = 2; c < dataGridView.Columns.Count; c++)
        //                        {
        //                            dataGridView.Columns[c].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
        //                        }
        //                    }
        //                    break;
        //            }
        //        }
        //        catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //    }
        //    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //}

        //private void InitSpecimenGrids(System.Windows.Forms.DataGridView dataGridView, IPM.TaxonSource source, ref int ColumnHeaderHeight, string DateSQL)
        //{
        //    try
        //    {
        //        System.Data.DataTable dataTable = DiversityCollection.Tasks.IPM.DataTableSpecimens(DateSQL); // new DataTable();
        //        DataGridViewCellStyle cellStyle = new DataGridViewCellStyle(this.dataGridViewSpecimens.DefaultCellStyle);
        //        cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //        System.Drawing.Font F = new Font(cellStyle.Font, FontStyle.Bold);
        //        cellStyle.Font = F;
        //        int ColumnHeaderAddedLineHeight = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(13);
        //        if (dataGridView.Rows.Count > 0)
        //        {
        //            foreach (System.Data.DataRow R in dataTable.Rows)
        //            {
        //                try
        //                {
        //                    System.Windows.Forms.DataGridViewColumn C = new DataGridViewColumn(dataGridView.Rows[0].Cells[0]);
        //                    C.DefaultCellStyle = cellStyle;
        //                    C.Tag = R;
        //                    int h = 0;
        //                    switch (source)
        //                    {
        //                        case Tasks.IPM.TaxonSource.Pest:
        //                            C.HeaderText = R[0].ToString().Replace(" | ", "\r\n");
        //                            foreach (char c in R[1].ToString()) { if (c == '|') h++; }
        //                            break;
        //                    }
        //                    dataGridView.Columns.Add(C);
        //                    if (h > 0)
        //                    {
        //                        int DefaultHeight = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(23);
        //                        int NewHeight = h * DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(13);
        //                        if (ColumnHeaderHeight < (DefaultHeight + NewHeight))
        //                            ColumnHeaderHeight = DefaultHeight + NewHeight;
        //                    }
        //                }
        //                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //            }
        //        }

        //        // reading the values
        //        try
        //        {
        //            switch (source)
        //            {
        //                case Tasks.IPM.TaxonSource.Bycatch:
        //                    this.ReadBycatchValues();
        //                    if (dataGridView.Columns.Count > 2)
        //                    {
        //                        for (int c = 2; c < dataGridView.Columns.Count; c++)
        //                        {
        //                            dataGridView.Columns[c].Width = this.dataGridViewSpecimens.Columns[c].Width;
        //                        }
        //                    }
        //                    break;
        //                case Tasks.IPM.TaxonSource.Pest:
        //                    this.ReadPestValues(this.dataGridViewSpecimens);
        //                    if (dataGridView.Columns.Count > 2)
        //                    {
        //                        for (int c = 2; c < dataGridView.Columns.Count; c++)
        //                        {
        //                            dataGridView.Columns[c].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
        //                        }
        //                    }
        //                    break;
        //            }
        //        }
        //        catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //    }
        //    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //}

        //private void InitTransactionGrids(System.Windows.Forms.DataGridView dataGridView, IPM.TaxonSource source, ref int ColumnHeaderHeight, string DateSQL)
        //{
        //    try
        //    {
        //        System.Data.DataTable dataTable = DiversityCollection.Tasks.IPM.DataTableTransactions(DateSQL); // new DataTable();
        //        DataGridViewCellStyle cellStyle = new DataGridViewCellStyle(this.dataGridViewTransaction.DefaultCellStyle);
        //        cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //        System.Drawing.Font F = new Font(cellStyle.Font, FontStyle.Bold);
        //        cellStyle.Font = F;
        //        int ColumnHeaderAddedLineHeight = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(13);
        //        if (dataGridView.Rows.Count > 0)
        //        {
        //            foreach (System.Data.DataRow R in dataTable.Rows)
        //            {
        //                try
        //                {
        //                    System.Windows.Forms.DataGridViewColumn C = new DataGridViewColumn(dataGridView.Rows[0].Cells[0]);
        //                    C.DefaultCellStyle = cellStyle;
        //                    C.Tag = R;
        //                    int h = 0;
        //                    switch (source)
        //                    {
        //                        case Tasks.IPM.TaxonSource.Pest:
        //                            C.HeaderText = R[0].ToString().Replace(" | ", "\r\n");
        //                            foreach (char c in R[1].ToString()) { if (c == '|') h++; }
        //                            break;
        //                    }
        //                    dataGridView.Columns.Add(C);
        //                    if (h > 0)
        //                    {
        //                        int DefaultHeight = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(23);
        //                        int NewHeight = h * DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(13);
        //                        if (ColumnHeaderHeight < (DefaultHeight + NewHeight))
        //                            ColumnHeaderHeight = DefaultHeight + NewHeight;
        //                    }
        //                }
        //                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //            }
        //        }

        //        // reading the values
        //        try
        //        {
        //            switch (source)
        //            {
        //                case Tasks.IPM.TaxonSource.Bycatch:
        //                    this.ReadBycatchValues();
        //                    if (dataGridView.Columns.Count > 2)
        //                    {
        //                        for (int c = 2; c < dataGridView.Columns.Count; c++)
        //                        {
        //                            dataGridView.Columns[c].Width = this.dataGridViewSpecimens.Columns[c].Width;
        //                        }
        //                    }
        //                    break;
        //                case Tasks.IPM.TaxonSource.Pest:
        //                    this.ReadPestValues(this.dataGridViewTransaction);
        //                    if (dataGridView.Columns.Count > 2)
        //                    {
        //                        for (int c = 2; c < dataGridView.Columns.Count; c++)
        //                        {
        //                            dataGridView.Columns[c].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
        //                        }
        //                    }
        //                    break;
        //            }
        //        }
        //        catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //    }
        //    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //}


        private void InitGridView_ValueColumns(IPM.RecordingTarget recordingTarget) //, bool Reset = true)
        {
            try
            {

                // removing previous columns for values
                while (this.DataGridView(recordingTarget).Columns.Count > 3)
                    this.DataGridView(recordingTarget).Columns.Remove(this.DataGridView(recordingTarget).Columns[this.DataGridView(recordingTarget).Columns.Count - 1]);

                //if (Reset) this.DataGridView(recordingTarget).Columns.Clear();
                //if (this.DataGridView(recordingTarget).Columns.Count == 0)
                //{

                //}
                System.Data.DataTable dataTable = this.DtRecordingTargetTable(recordingTarget);
                DataGridViewCellStyle cellStyle = new DataGridViewCellStyle(this.DataGridView(recordingTarget).DefaultCellStyle);
                cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                System.Drawing.Font F = new Font(cellStyle.Font, FontStyle.Bold);
                cellStyle.Font = F;
                int ColumnHeaderAddedLineHeight = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(13);
                if (this.DataGridView(recordingTarget).Rows.Count > 0)
                {
                    foreach (System.Data.DataRow R in dataTable.Rows)
                    {
                        try
                        {
                            System.Windows.Forms.DataGridViewColumn C = new DataGridViewColumn(this.DataGridView(recordingTarget).Rows[0].Cells[0]);
                            C.DefaultCellStyle = cellStyle;
                            C.Tag = R;
                            int h = 0;
                            switch (recordingTarget)
                            {
                                default:
                                    C.HeaderText = R[0].ToString().Replace(" | ", "\r\n");
                                    foreach (char c in R[0].ToString()) { if (c == '|') h++; }
                                    break;
                            }
                            this.DataGridView(recordingTarget).Columns.Add(C);
                        }
                        catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                    }
                }

                //this.setGridviewVisibility(recordingTarget);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private bool setGridviewVisibility(IPM.RecordingTarget recordingTarget)
        {
            bool ShowGrid = false;
            try
            {
                System.Windows.Forms.TabPage tabPage = null;
                switch (recordingTarget)
                {
                    case IPM.RecordingTarget.CollectionPest:
                        tabPage = this.tabPageCollections;
                        ShowGrid = Settings.Default.IncludeCollections;
                        break;
                    case IPM.RecordingTarget.SpecimenPest:
                        tabPage = this.tabPageSpecimens;
                        ShowGrid = Settings.Default.IncludeSpecimen;
                        break;
                    case IPM.RecordingTarget.TransactionPest:
                        tabPage = this.tabPageTransaction;
                        ShowGrid = Settings.Default.IncludeTransactions;
                        break;
                    case IPM.RecordingTarget.TrapBycatch:
                        this.splitContainerPestAndBycatch.Panel2Collapsed = Settings.Default.BycatchNameUris.Count == 0;
                        ShowGrid = Settings.Default.BycatchNameUris.Count > 0;
                        break;
                }
                if (tabPage != null)
                {
                    if (ShowGrid)
                    {
                        if(!this.tabControlMonitoring.TabPages.Contains(tabPage))
                            this.tabControlMonitoring.TabPages.Add(tabPage);
                    }
                    else
                    {
                        if (this.tabControlMonitoring.TabPages.Contains(tabPage))
                            this.tabControlMonitoring.TabPages.Remove(tabPage);
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return ShowGrid;
        }

        //private void InitGrids(System.Windows.Forms.DataGridView dataGridView, IPM.TaxonSource source, ref int ColumnHeaderHeight, string DateSQL, IPM.MonitoringTarget target = Tasks.IPM.MonitoringTarget.Trap)
        //{
        //    try
        //    {
        //        System.Data.DataTable dataTable = this.DtTargetTable(target);
        //        DataGridViewCellStyle cellStyle = new DataGridViewCellStyle(dataGridView.DefaultCellStyle);
        //        cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //        System.Drawing.Font F = new Font(cellStyle.Font, FontStyle.Bold);
        //        cellStyle.Font = F;
        //        int ColumnHeaderAddedLineHeight = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(13);
        //        if (dataGridView.Rows.Count > 0)
        //        {
        //            foreach (System.Data.DataRow R in dataTable.Rows)
        //            {
        //                try
        //                {
        //                    System.Windows.Forms.DataGridViewColumn C = new DataGridViewColumn(dataGridView.Rows[0].Cells[0]);
        //                    C.DefaultCellStyle = cellStyle;
        //                    C.Tag = R;
        //                    int h = 0;
        //                    switch (source)
        //                    {
        //                        case Tasks.IPM.TaxonSource.Pest:
        //                            switch (target)
        //                            {
        //                                default:
        //                                    C.HeaderText = R[0].ToString().Replace(" | ", "\r\n");
        //                                    foreach (char c in R[0].ToString()) { if (c == '|') h++; }
        //                                    break;
        //                            }
        //                            break;
        //                    }
        //                    dataGridView.Columns.Add(C);
        //                    if (h > 0)
        //                    {
        //                        int DefaultHeight = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(23);
        //                        int NewHeight = h * DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(13);
        //                        if (ColumnHeaderHeight < (DefaultHeight + NewHeight))
        //                            ColumnHeaderHeight = DefaultHeight + NewHeight;
        //                    }
        //                }
        //                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //            }
        //        }

        //        // reading the values
        //        this.ReadValuesForGrid(dataGridView, source, target);
        //        //try
        //        //{
        //        //    switch (source)
        //        //    {
        //        //        case Tasks.IPM.TaxonSource.Bycatch:
        //        //            this.ReadBycatchValues();
        //        //            if (dataGridView.Columns.Count > 2)
        //        //            {
        //        //                for (int c = 2; c < dataGridView.Columns.Count; c++)
        //        //                {
        //        //                    if (this.dataGridViewSpecimens.Columns.Count > c)
        //        //                        dataGridView.Columns[c].Width = this.dataGridViewSpecimens.Columns[c].Width;
        //        //                }
        //        //            }
        //        //            break;
        //        //        case Tasks.IPM.TaxonSource.Pest:
        //        //            this.ReadPestValues(dataGridView);
        //        //            if (dataGridView.Columns.Count > 2)
        //        //            {
        //        //                for (int c = 2; c < dataGridView.Columns.Count; c++)
        //        //                {
        //        //                    dataGridView.Columns[c].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
        //        //                }
        //        //            }
        //        //            break;
        //        //    }
        //        //}
        //        //catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //    }
        //    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //}

        //private System.Data.DataTable DtTargetTable(IPM.MonitoringTarget target = Tasks.IPM.MonitoringTarget.Trap)
        //{
        //    System.Data.DataTable dataTable = new DataTable();
        //    switch (target)
        //    {
        //        case IPM.MonitoringTarget.Trap:
        //            dataTable = DiversityCollection.Tasks.IPM.DataTableTraps(); // 
        //            break;
        //        case IPM.MonitoringTarget.Collection:
        //            dataTable = DiversityCollection.Tasks.IPM.DataTableCollections(DateSQL); // 
        //            break;
        //        case IPM.MonitoringTarget.Specimen:
        //            dataTable = DiversityCollection.Tasks.IPM.DataTableSpecimens(DateSQL); // 
        //            break;
        //        case IPM.MonitoringTarget.Transaction:
        //            dataTable = DiversityCollection.Tasks.IPM.DataTableTransactions(DateSQL); // 
        //            break;
        //    }
        //    return dataTable;
        //}

        private System.Data.DataTable DtRecordingTargetTable(IPM.RecordingTarget recordingTarget)
        {
            System.Data.DataTable dataTable = new DataTable();
            switch (recordingTarget)
            {
                case IPM.RecordingTarget.TrapPest:
                case IPM.RecordingTarget.TrapBycatch:
                    dataTable = DiversityCollection.Tasks.IPM.DataTableTraps();
                    break;
                case IPM.RecordingTarget.CollectionPest:
                    dataTable = DiversityCollection.Tasks.IPM.DataTableCollections(DateSQL); // 
                    break;
                case IPM.RecordingTarget.SpecimenPest:
                    dataTable = DiversityCollection.Tasks.IPM.DataTableSpecimens(DateSQL); // 
                    break;
                case IPM.RecordingTarget.TransactionPest:
                    dataTable = DiversityCollection.Tasks.IPM.DataTableTransactions(DateSQL); // 
                    break;
            }
            return dataTable;
        }

        private bool initGridViewRow(IPM.RecordingTarget recordingTarget, ref int i, string Identifier, Taxa.TaxonRecord record, bool Reset = false)
        {
            bool OK = true;
            try
            {
                System.Collections.Specialized.StringCollection NameUris = IPM.NameUris(recordingTarget);
                if (NameUris.Contains(Identifier) && record.NameID > -1) OK = false;
                this.DataGridView(recordingTarget).Rows.Add(1);
                if (record.NameID > -1)
                    this.DataGridView(recordingTarget).Rows[i].Height = 50;
                else
                {
                    this.DataGridView(recordingTarget).Rows[i].Height = 20;
                    this.DataGridView(recordingTarget).Rows[i].ReadOnly = true;
                    this.DataGridView(recordingTarget).Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                }
                this.DataGridView(recordingTarget).Rows[i].Cells[0].Value = record.Group;
                this.DataGridView(recordingTarget).Rows[i].Cells[1].Value = record.DisplayText;
                DataGridViewCellStyle style = new DataGridViewCellStyle();
                style.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.DataGridView(recordingTarget).Rows[i].Cells[1].Style = style;
                if (record.PreviewImage.Icon != null) // && keyValue.Value.PreviewImages.Count > 0)
                {
                    this.DataGridView(recordingTarget).Rows[i].Cells[2].Value = record.PreviewImage.Icon;
                }
                else
                {
                    this.DataGridView(recordingTarget).Rows[i].Cells[2].Value = DiversityCollection.Resource.CheckYes;
                }
                this.DataGridView(recordingTarget).Rows[i].Tag = Identifier;
                i++;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return OK;
        }


        //private bool initGridView(System.Windows.Forms.DataGridView dataGridView, IPM.TaxonSource source, ref int i, string Identifier, Taxa.Record record, bool Reset = false)
        //{
        //    bool OK = true;
        //    try
        //    {
        //        System.Collections.Specialized.StringCollection NameUris = IPM.NameUris(source);
        //        if (NameUris.Contains(Identifier) && record.NameID > -1) OK = false;
        //        //switch (source)
        //        //{
        //        //    case Tasks.IPM.TaxonSource.Bycatch:
        //        //        if (Settings.Default.BycatchNameUris != null && !Settings.Default.BycatchNameUris.Contains(Identifier) && record.NameID > -1)
        //        //            OK = false;
        //        //        break;
        //        //    case Tasks.IPM.TaxonSource.Pest:
        //        //        if (Settings.Default.PestNameUris != null && !Settings.Default.PestNameUris.Contains(Identifier) && record.NameID > -1)
        //        //            OK = false;
        //        //        break;
        //        //}
        //        dataGridView.Rows.Add(1);
        //        if (record.NameID > -1)
        //            dataGridView.Rows[i].Height = 50;
        //        else
        //        {
        //            dataGridView.Rows[i].Height = 20;
        //            dataGridView.Rows[i].ReadOnly = true;
        //            dataGridView.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
        //        }
        //        dataGridView.Rows[i].Cells[0].Value = record.Group;
        //        dataGridView.Rows[i].Cells[1].Value = record.DisplayText;
        //        DataGridViewCellStyle style = new DataGridViewCellStyle();
        //        style.Alignment = DataGridViewContentAlignment.MiddleRight;
        //        dataGridView.Rows[i].Cells[1].Style = style;
        //        if (record.PreviewImage.Icon != null) // && keyValue.Value.PreviewImages.Count > 0)
        //        {
        //            dataGridView.Rows[i].Cells[2].Value = record.PreviewImage.Icon;
        //        }
        //        else
        //        {
        //            dataGridView.Rows[i].Cells[2].Value = DiversityCollection.Resource.CheckYes;
        //        }
        //        dataGridView.Rows[i].Tag = Identifier;
        //        i++;
        //    }
        //    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //    return OK;
        //}

        //private void initGridView(System.Windows.Forms.DataGridView dataGridView, IPM.TaxonSource source)
        //{
        //    try
        //    {
        //        dataGridView.SuspendLayout();
        //        dataGridView.RowHeadersVisible = false;
        //        dataGridView.AllowUserToAddRows = false;
        //        dataGridView.AllowUserToDeleteRows = false;

        //        dataGridView.Rows.Clear();
        //        DataGridViewCellStyle s = dataGridView.DefaultCellStyle;
        //        s.WrapMode = DataGridViewTriState.True;
        //        dataGridView.Columns[0].DefaultCellStyle = s;

        //        // filling taxa if missing
        //        System.Collections.Specialized.StringCollection NameUris = IPM.NameUris(source);
        //        if (NameUris.Count == 0)
        //        {
        //            Taxa.RecordDicts.ChecklistRecords(source);
        //        }
        //        //switch (source)
        //        //{
        //        //    case Tasks.IPM.TaxonSource.Bycatch:
        //        //        if (Settings.Default.BycatchNameUris != null && Settings.Default.BycatchNameUris.Count == 0)
        //        //        {
        //        //            foreach (System.Collections.Generic.KeyValuePair<string, Taxon> keyValue in iPM.GetBycatchTaxa())
        //        //                Settings.Default.BycatchNameUris.Add(keyValue.Key);
        //        //            Settings.Default.Save();
        //        //        }
        //        //        break;
        //        //    case Tasks.IPM.TaxonSource.Pest:
        //        //        if (Settings.Default.PestNameUris != null && Settings.Default.PestNameUris.Count == 0)
        //        //        {
        //        //            foreach (System.Collections.Generic.KeyValuePair<string, Taxon> keyValue in iPM.GetPestTaxa())
        //        //                Settings.Default.PestNameUris.Add(keyValue.Key);
        //        //            Settings.Default.Save();
        //        //        }
        //        //        break;
        //        //}

        //        // filling the grid results
        //        this.FillingResultGrid(dataGridView, source);

        //        // removing previous columns for traps
        //        while (dataGridView.Columns.Count > 3)
        //            dataGridView.Columns.Remove(dataGridView.Columns[3]);

        //        // setting the header
        //        try
        //        {
        //            string SQL = "SELECT A.[CollectionID], A.DisplayText " +
        //                "FROM dbo.CollectionHierarchyAll() A WHERE A.CollectionID IN (" + Tasks.IPM.CollectionIDList() + ") AND A.Type = 'Trap' " +
        //                "ORDER BY A.DisplayText";
        //            System.Data.DataTable dataTable = new DataTable();
        //            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTable);
        //            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle(dataGridViewResults.DefaultCellStyle);
        //            cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //            System.Drawing.Font F = new Font(cellStyle.Font, FontStyle.Bold);
        //            cellStyle.Font = F;
        //            int ColumnHeaderAddedLineHeight = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(13);
        //            if (dataGridView.Rows.Count > 0)
        //            {
        //                foreach (System.Data.DataRow R in dataTable.Rows)
        //                {
        //                    try
        //                    {
        //                        System.Windows.Forms.DataGridViewColumn C = new DataGridViewColumn(dataGridView.Rows[0].Cells[0]);
        //                        C.DefaultCellStyle = cellStyle;
        //                        C.Tag = R[0].ToString();
        //                        int h = 0;
        //                        switch (source)
        //                        {
        //                            case Tasks.IPM.TaxonSource.Pest:
        //                                C.HeaderText = R[1].ToString().Replace(" | ", "\r\n");
        //                                foreach (char c in R[1].ToString()) { if (c == '|') h++; }
        //                                break;
        //                        }
        //                        dataGridView.Columns.Add(C);
        //                        if (h > 0)
        //                        {
        //                            int DefaultHeight = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(23);
        //                            int NewHeight = h * DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(13);
        //                        }
        //                    }
        //                    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //                }
        //            }
        //        }
        //        catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }

        //        // reading the values
        //        try
        //        {
        //            switch (source)
        //            {
        //                case Tasks.IPM.TaxonSource.Bycatch:
        //                    this.ReadBycatchValues();
        //                    if (dataGridView.Columns.Count > 2)
        //                    {
        //                        for (int c = 2; c < dataGridView.Columns.Count; c++)
        //                        {
        //                            if (this.dataGridViewResults.Columns.Count > c)
        //                                dataGridView.Columns[c].Width = this.dataGridViewResults.Columns[c].Width;
        //                            else break;
        //                        }
        //                    }
        //                    break;
        //                case Tasks.IPM.TaxonSource.Pest:
        //                    this.ReadPestValues(this.dataGridViewResults);
        //                    if (dataGridView.Columns.Count > 2)
        //                    {
        //                        for (int c = 2; c < dataGridView.Columns.Count; c++)
        //                        {
        //                            dataGridView.Columns[c].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
        //                        }
        //                    }
        //                    break;
        //            }
        //        }
        //        catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }

        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    finally
        //    {
        //        dataGridView.ResumeLayout();
        //    }
        //}

//        private void initGridView(System.Windows.Forms.DataGridView dataGridView, IPM.TaxonSource source, ref int ColumnHeaderHeight)
//        {
//            try
//            {
//                ColumnHeaderHeight = 0;
//                dataGridView.SuspendLayout();
//                dataGridView.RowHeadersVisible = false;
//                dataGridView.AllowUserToAddRows = false;
//                dataGridView.AllowUserToDeleteRows = false;

//                dataGridView.Rows.Clear();
//                IPM iPM = new IPM();
//                DataGridViewCellStyle s = dataGridView.DefaultCellStyle;
//                s.WrapMode = DataGridViewTriState.True;
//                dataGridView.Columns[0].DefaultCellStyle = s;

//                // filling taxa if missing
//                //switch (source)
//                //{
//                //    case Tasks.IPM.TaxonSource.Bycatch:
//                //        if (Settings.Default.BycatchNameUris != null && Settings.Default.BycatchNameUris.Count == 0)
//                //        {
//                //            foreach (System.Collections.Generic.KeyValuePair<string, Taxon> keyValue in iPM.GetBycatchTaxa())
//                //                Settings.Default.BycatchNameUris.Add(keyValue.Key);
//                //            Settings.Default.Save();
//                //        }
//                //        break;
//                //    case Tasks.IPM.TaxonSource.Pest:
//                //        if (Settings.Default.PestNameUris != null && Settings.Default.PestNameUris.Count == 0)
//                //        {
//                //            foreach (System.Collections.Generic.KeyValuePair<string, Taxon> keyValue in iPM.GetPestTaxa())
//                //                Settings.Default.PestNameUris.Add(keyValue.Key);
//                //            Settings.Default.Save();
//                //        }
//                //        break;
//                //}

//                // filling the grid results
//#if DEBUG
//                this.FillingResultGrid(dataGridView, source);
//#else
//                //System.Collections.Generic.Dictionary<string, Taxon> Taxa = new Dictionary<string, Taxon>();
//                //switch (source)
//                //{
//                //    case Tasks.IPM.TaxonSource.Bycatch:
//                //        Taxa = iPM.GetBycatchTaxa();
//                //        break;
//                //    case Tasks.IPM.TaxonSource.Pest:
//                //        Taxa = iPM.GetPestTaxa();
//                //        break;
//                //}
//                //try
//                //{
//                //    int i = 0;
//                //    foreach (System.Collections.Generic.KeyValuePair<string, Taxon> keyValue in Taxa)
//                //    {
//                //        try
//                //        {
//                //            switch (source)
//                //            {
//                //                case Tasks.IPM.TaxonSource.Bycatch:
//                //                    if (Settings.Default.BycatchNameUris != null && !Settings.Default.BycatchNameUris.Contains(keyValue.Key) && keyValue.Value.NameID > -1)
//                //                        continue;
//                //                    break;
//                //                case Tasks.IPM.TaxonSource.Pest:
//                //                    if (Settings.Default.PestNameUris != null && !Settings.Default.PestNameUris.Contains(keyValue.Key) && keyValue.Value.NameID > -1)
//                //                        continue;
//                //                    break;
//                //            }
//                //            dataGridView.Rows.Add(1);
//                //            if (keyValue.Value.NameID > -1)
//                //                dataGridView.Rows[i].Height = 50;
//                //            else
//                //            {
//                //                dataGridView.Rows[i].Height = 20;
//                //                dataGridView.Rows[i].ReadOnly = true;
//                //                dataGridView.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
//                //            }
//                //            dataGridView.Rows[i].Cells[0].Value = keyValue.Value.Group;
//                //            dataGridView.Rows[i].Cells[1].Value = keyValue.Value.DisplayText();
//                //            DataGridViewCellStyle style = new DataGridViewCellStyle();
//                //            style.Alignment = DataGridViewContentAlignment.MiddleRight;
//                //            if (keyValue.Value.AcceptedName)
//                //            {
//                //                style.Alignment = DataGridViewContentAlignment.MiddleLeft;
//                //            }
//                //            dataGridView.Rows[i].Cells[1].Style = style;
//                //            if (keyValue.Value.Icones != null && keyValue.Value.Icones.Count > 0)
//                //            {
//                //                dataGridView.Rows[i].Cells[2].Value = keyValue.Value.Icones[0].Icon;
//                //            }
//                //            else
//                //            {
//                //                dataGridView.Rows[i].Cells[2].Value = DiversityCollection.Resource.CheckYes;
//                //            }
//                //            dataGridView.Rows[i].Tag = keyValue.Key;
//                //            i++;
//                //        }
//                //        catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
//                //    }
//                //}catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
//                //if (source == Tasks.IPM.TaxonSource.Bycatch)
//                //{
//                //    dataGridView.Rows[0].Visible = false;
//                //    this.splitContainerPestAndBycatch.Panel2Collapsed = dataGridView.Rows.Count < 2;
//                //}
//#endif

//                // removing previous columns for traps
//                while (dataGridView.Columns.Count > 3)
//                    dataGridView.Columns.Remove(dataGridView.Columns[3]);

//                // setting the header
//                try
//                {
//                    string SQL = "SELECT A.[CollectionID], A.DisplayText " +
//                        "FROM dbo.CollectionHierarchyAll() A WHERE A.CollectionID IN (" + Tasks.IPM.CollectionIDList() + ") AND A.Type = 'Trap' " +
//                        "ORDER BY A.DisplayText";
//                    System.Data.DataTable dataTable = new DataTable();
//                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTable);
//                    DataGridViewCellStyle cellStyle = new DataGridViewCellStyle(dataGridViewResults.DefaultCellStyle);
//                    cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
//                    System.Drawing.Font F = new Font(cellStyle.Font, FontStyle.Bold);
//                    cellStyle.Font = F;
//                    int ColumnHeaderAddedLineHeight = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(13);
//                    if (dataGridView.Rows.Count > 0)
//                    {
//                        foreach (System.Data.DataRow R in dataTable.Rows)
//                        {
//                            try
//                            {
//                                System.Windows.Forms.DataGridViewColumn C = new DataGridViewColumn(dataGridView.Rows[0].Cells[0]);
//                                C.DefaultCellStyle = cellStyle;
//                                C.Tag = R[0].ToString();
//                                int h = 0;
//                                switch (source)
//                                {
//                                    case Tasks.IPM.TaxonSource.Pest:
//                                        C.HeaderText = R[1].ToString().Replace(" | ", "\r\n");
//                                        foreach (char c in R[1].ToString()) { if (c == '|') h++; }
//                                        break;
//                                }
//                                dataGridView.Columns.Add(C);
//                                if (h > 0)
//                                {
//                                    int DefaultHeight = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(23);
//                                    int NewHeight = h * DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(13);
//                                    if (ColumnHeaderHeight < (DefaultHeight + NewHeight))
//                                        ColumnHeaderHeight = DefaultHeight + NewHeight;
//                                }
//                                //if (dataGridView.ColumnHeadersHeight > ColumnHeaderHeight)
//                                //    ColumnHeaderHeight = dataGridView.ColumnHeadersHeight;
//                            }
//                            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
//                        }
//                    }
//                }catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }

//                // reading the values
//                try
//                {
//                    switch (source)
//                    {
//                        case Tasks.IPM.TaxonSource.Bycatch:
//                            this.ReadBycatchValues();
//                            if (dataGridView.Columns.Count > 2)
//                            {
//                                for (int c = 2; c < dataGridView.Columns.Count; c++)
//                                {
//                                    if (this.dataGridViewResults.Columns.Count > c)
//                                        dataGridView.Columns[c].Width = this.dataGridViewResults.Columns[c].Width;
//                                    else break;
//                                }
//                            }
//                            break;
//                        case Tasks.IPM.TaxonSource.Pest:
//                            this.ReadPestValues(this.dataGridViewResults);
//                            if (dataGridView.Columns.Count > 2)
//                            {
//                                for (int c = 2; c < dataGridView.Columns.Count; c++)
//                                {
//                                    dataGridView.Columns[c].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
//                                }
//                            }
//                            break;
//                    }
//                }catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }

//            }
//            catch (System.Exception ex)
//            {
//                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
//            }
//            finally
//            {
//                dataGridView.ResumeLayout();
//            }
//        }

        #region Filling the records

        //private void FillingMissingRecords(IPM.TaxonSource source)
        //{
        //    try
        //    {
        //        System.Collections.Specialized.StringCollection uris = null;
        //        switch (source)
        //        {
        //            case Tasks.IPM.TaxonSource.Bycatch:
        //                uris = Settings.Default.BycatchNameUris;
        //                break;
        //            case Tasks.IPM.TaxonSource.Pest:
        //                uris = Settings.Default.PestNameUris;
        //                break;
        //        }
        //        if (uris != null && uris.Count == 0)
        //        {
        //            foreach (System.Collections.Generic.KeyValuePair<string, Taxa.Record> keyValue in Tasks.Taxa.RecordDicts.ChecklistRecords(source))
        //                uris.Add(keyValue.Key);
        //            Settings.Default.Save();
        //        }
        //    }
        //    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //}

        //private void FillingResultGrid(System.Windows.Forms.DataGridView dataGridView, IPM.TaxonSource source)
        //{
        //    System.Collections.Generic.Dictionary<string, Tasks.Taxa.TaxonStage> TaxonStages = List.TaxonStageDict(source);// new Dictionary<string, TaxonStage>();
        //    try
        //    {
        //        int i = 0;
        //        foreach (System.Collections.Generic.KeyValuePair<string, Tasks.Taxa.TaxonStage> keyValue in TaxonStages)
        //        {
        //            try
        //            {
        //                if (IPM.NameUris(source).Contains(keyValue.Key) && keyValue.Value.NameID > -1)
        //                    continue;
        //                //switch (source)
        //                //{
        //                //    case Tasks.IPM.TaxonSource.Bycatch:
        //                //        if (Settings.Default.BycatchNameUris != null && !Settings.Default.BycatchNameUris.Contains(keyValue.Key) && keyValue.Value.NameID > -1)
        //                //            continue;
        //                //        break;
        //                //    case Tasks.IPM.TaxonSource.Pest:
        //                //        if (Settings.Default.PestNameUris != null && !Settings.Default.PestNameUris.Contains(keyValue.Key) && keyValue.Value.NameID > -1)
        //                //            continue;
        //                //        break;
        //                //}
        //                dataGridView.Rows.Add(1);
        //                if (keyValue.Value.NameID > -1)
        //                    dataGridView.Rows[i].Height = 50;
        //                else
        //                {
        //                    dataGridView.Rows[i].Height = 20;
        //                    dataGridView.Rows[i].ReadOnly = true;
        //                    dataGridView.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
        //                }
        //                dataGridView.Rows[i].Cells[0].Value = keyValue.Value.Group;
        //                dataGridView.Rows[i].Cells[1].Value = keyValue.Value.DisplayText();
        //                DataGridViewCellStyle style = new DataGridViewCellStyle();
        //                style.Alignment = DataGridViewContentAlignment.MiddleRight;
        //                if (keyValue.Value.IsAcceptedName)
        //                {
        //                    style.Alignment = DataGridViewContentAlignment.MiddleLeft;
        //                }
        //                dataGridView.Rows[i].Cells[1].Style = style;
        //                if (keyValue.Value.PreviewImage.Icon != null)
        //                {
        //                    dataGridView.Rows[i].Cells[2].Value = keyValue.Value.PreviewImage.Icon;
        //                }
        //                else
        //                {
        //                    dataGridView.Rows[i].Cells[2].Value = DiversityCollection.Resource.CheckYes;
        //                }
        //                dataGridView.Rows[i].Tag = keyValue.Key;
        //                i++;
        //            }
        //            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //        }
        //    }
        //    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //    if (source == Tasks.IPM.TaxonSource.Bycatch)
        //    {
        //        dataGridView.Rows[0].Visible = false;
        //        this.splitContainerPestAndBycatch.Panel2Collapsed = dataGridView.Rows.Count < 2;
        //    }

        //}

        private System.Collections.Generic.Dictionary<int, int> _CollectionIDs;

        #endregion

        #endregion

        #region Reading the values from the database

        //private void ReadValuesForGrid(System.Windows.Forms.DataGridView dataGridView, IPM.TaxonSource source, IPM.MonitoringTarget target)
        //{
        //    // reading the values
        //    try
        //    {
        //        switch (source)
        //        {
        //            case Tasks.IPM.TaxonSource.Bycatch:
        //                this.ReadBycatchValues();
        //                if (dataGridView.Columns.Count > 2)
        //                {
        //                    for (int c = 2; c < dataGridView.Columns.Count; c++)
        //                    {
        //                        if (this.dataGridViewSpecimens.Columns.Count > c)
        //                            dataGridView.Columns[c].Width = this.dataGridViewSpecimens.Columns[c].Width;
        //                    }
        //                }
        //                break;
        //            case Tasks.IPM.TaxonSource.Pest:
        //                this.ReadPestValues(dataGridView);
        //                if (dataGridView.Columns.Count > 2)
        //                {
        //                    for (int c = 2; c < dataGridView.Columns.Count; c++)
        //                    {
        //                        dataGridView.Columns[c].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
        //                    }
        //                }
        //                break;
        //        }
        //    }
        //    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //}

        private void ReadRecordings(IPM.RecordingTarget recordingTarget)
        {
            try
            {
                Tasks.IPM.CurrentState = State.Reading;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                if (_Date == null || _Date.Length == 0)
                    _Date = System.DateTime.Now.ToString("yyyy-MM-dd");
                // getting the pests
                System.Data.DataTable dtPests = Tasks.IPM.ReadRecordings(ref this._Date,recordingTarget);// new DataTable();

                // getting the CollectionIDs
                //System.Collections.Generic.Dictionary<int, int> CollectionIDs = new Dictionary<int, int>();
                //foreach (System.Windows.Forms.DataGridViewColumn C in this.DataGridView(recordingTarget).Columns)
                //{
                //    int CollectionID;
                //    if (C.Tag != null && int.TryParse(C.Tag.ToString(), out CollectionID))
                //        CollectionIDs.Add(C.Index, CollectionID);
                //}

                // getting the IDs corresponding to the columns
                string Column = "";
                System.Collections.Generic.Dictionary<int, int> ColumnIDs = new Dictionary<int, int>();
                foreach (System.Windows.Forms.DataGridViewColumn C in this.DataGridView(recordingTarget).Columns)
                {
                    int ID;
                    if (C.Tag == null)
                        continue;
                    System.Data.DataRow R = (System.Data.DataRow)C.Tag;
                    switch (recordingTarget)
                    {
                        case IPM.RecordingTarget.TransactionPest:
                            Column = "TransactionID";
                            break;
                        case IPM.RecordingTarget.SpecimenPest:
                            Column = "SpecimenPartID";
                            break;
                        default: Column = "CollectionID";
                            break;
                    }
                    if (int.TryParse(R[Column].ToString(), out ID))
                        ColumnIDs.Add(C.Index, ID);
                }


                // setting the values
                foreach (System.Windows.Forms.DataGridViewRow R in this.DataGridView(recordingTarget).Rows)
                {
                    if (R.Tag != null)
                    {
                        foreach (System.Windows.Forms.DataGridViewCell C in R.Cells)
                        {
                            if (ColumnIDs.ContainsKey(C.ColumnIndex))
                            {
                                try
                                {
                                    System.Data.DataRow[] rr = dtPests.Select(Column + " = " + ColumnIDs[C.ColumnIndex].ToString() + " AND ModuleUri = '" + R.Tag.ToString() + "'");
                                    if (rr.Length > 0)
                                    {
                                        string Value = rr[0]["NumberValue"].ToString();
                                        if (!rr[0]["Result"].Equals(System.DBNull.Value) && rr[0]["Result"].ToString().Length > 0)
                                            Value += "\r\n" + rr[0]["Result"].ToString();
                                        else if (!rr[0]["Notes"].Equals(System.DBNull.Value) && rr[0]["Notes"].ToString().Length > 0)
                                            Value += "\r\n";
                                        if (!rr[0]["Notes"].Equals(System.DBNull.Value) && rr[0]["Notes"].ToString().Length > 0)
                                            Value += "\r\n" + rr[0]["Notes"].ToString();
                                        C.Value = Value;
                                    }
                                    else if (R.Tag.ToString() == "-1")
                                    {
                                        C.Value = "x";
                                    }
                                    else
                                    {
                                        C.Value = null;
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                                }
                            }
                        }
                    }
                    else { }
                }
                this.DataGridView(recordingTarget).Columns[2].HeaderText = _Date;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                Tasks.IPM.CurrentState = State.Editing;
            }
        }


        private void ReadPestValues(System.Windows.Forms.DataGridView dataGridView)
        {
            try
            {
                Tasks.IPM.CurrentState = State.Reading;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                if (_Date == null || _Date.Length == 0)
                    _Date = System.DateTime.Now.ToString("yyyy-MM-dd");
                // getting the pests
                System.Data.DataTable dtPests = Tasks.IPM.ReadTaxonValues(ref this._Date, Tasks.IPM.TaxonSource.Pest);// new DataTable();

                // getting the CollectionIDs
                System.Collections.Generic.Dictionary<int, int> CollectionIDs = new Dictionary<int, int>();
                foreach (System.Windows.Forms.DataGridViewColumn C in dataGridView.Columns)
                {
                    int CollectionID;
                    if (C.Tag != null && int.TryParse(C.Tag.ToString(), out CollectionID))
                        CollectionIDs.Add(C.Index, CollectionID);
                }

                // setting the values
                foreach (System.Windows.Forms.DataGridViewRow R in dataGridView.Rows)
                {
                    if (R.Tag != null)
                    {
                        foreach (System.Windows.Forms.DataGridViewCell C in R.Cells)
                        {
                            if (CollectionIDs.ContainsKey(C.ColumnIndex))
                            {
                                System.Data.DataRow[] rr = dtPests.Select("CollectionID = " + CollectionIDs[C.ColumnIndex].ToString() + " AND ModuleUri = '" + R.Tag.ToString() + "'");
                                if (rr.Length > 0)
                                {
                                    string Value = rr[0]["NumberValue"].ToString();
                                    if (!rr[0]["Result"].Equals(System.DBNull.Value) && rr[0]["Result"].ToString().Length > 0)
                                        Value += "\r\n" + rr[0]["Result"].ToString();
                                    else if (!rr[0]["Notes"].Equals(System.DBNull.Value) && rr[0]["Notes"].ToString().Length > 0)
                                        Value += "\r\n";
                                    if (!rr[0]["Notes"].Equals(System.DBNull.Value) && rr[0]["Notes"].ToString().Length > 0)
                                        Value += "\r\n" + rr[0]["Notes"].ToString();
                                    C.Value = Value;
                                }
                                else if (R.Tag.ToString() == "-1")
                                {
                                    C.Value = "x";
                                }
                                else
                                {
                                    C.Value = null;
                                }
                            }
                        }
                    }
                    else { }
                }
                dataGridView.Columns[2].HeaderText = _Date;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                Tasks.IPM.CurrentState = State.Editing;
            }
        }

        private void ReadBycatchValues()
        {
            this.ReadValues(this.dataGridViewBycatch, Tasks.IPM.TaxonSource.Bycatch);
            return;

        }

        private void ReadValues(System.Windows.Forms.DataGridView dataGridView, IPM.TaxonSource source)
        {
            try
            {
                Tasks.IPM.CurrentState = State.Reading;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                if (_Date == null || _Date.Length == 0)
                    _Date = System.DateTime.Now.ToString("yyyy-MM-dd");

                // getting the pests
                System.Data.DataTable dtPests = Tasks.IPM.ReadTaxonValues(ref _Date, source);// new DataTable();

                // getting the CollectionIDs
                if (source == Tasks.IPM.TaxonSource.Pest || source == Tasks.IPM.TaxonSource.Bycatch)
                {
                    this._CollectionIDs = new Dictionary<int, int>();
                    foreach (System.Windows.Forms.DataGridViewColumn C in dataGridView.Columns)
                    {
                        int CollectionID;
                        if (C.Tag != null)
                        {
                            try
                            {
                                System.Data.DataRow row = (System.Data.DataRow)C.Tag;
                                if (row.Table.Columns.Contains("CollectionID") && int.TryParse(row["CollectionID"].ToString(), out CollectionID))
                                    _CollectionIDs.Add(C.Index, CollectionID);
                            }
                            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                        } 
                        //&& int.TryParse(C.Tag.ToString(), out CollectionID))
                        //    _CollectionIDs.Add(C.Index, CollectionID);
                    }

                    // setting the values
                    foreach (System.Windows.Forms.DataGridViewRow R in dataGridView.Rows)
                    {
                        if (R.Tag != null)
                        {
                            foreach (System.Windows.Forms.DataGridViewCell C in R.Cells)
                            {
                                if (_CollectionIDs.ContainsKey(C.ColumnIndex))
                                {
                                    System.Data.DataRow[] rr = dtPests.Select("CollectionID = " + _CollectionIDs[C.ColumnIndex].ToString() + " AND ModuleUri = '" + R.Tag.ToString() + "'");
                                    if (rr.Length > 0)
                                    {
                                        string Value = rr[0]["NumberValue"].ToString();
                                        if (!rr[0]["Notes"].Equals(System.DBNull.Value) && rr[0]["Notes"].ToString().Length > 0)
                                            Value += " " + rr[0]["Notes"].ToString();
                                        C.Value = Value;
                                    }
                                    else if (R.Tag.ToString() == "-1")
                                    {
                                        C.Value = "x";
                                    }
                                    else
                                    {
                                        C.Value = null;
                                    }
                                }
                            }
                        }
                        else { }
                    }
                    dataGridView.Columns[2].HeaderText = _Date;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                Tasks.IPM.CurrentState = State.Editing;
            }
        }

        //private void ReadValues(System.Windows.Forms.DataGridView dataGridView, IPM.TaxonSource source, IPM.MonitoringTarget monitoringTarget)
        //{
        //    try
        //    {
        //        Tasks.IPM.CurrentState = State.Reading;
        //        this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
        //        if (_Date == null || _Date.Length == 0)
        //            _Date = System.DateTime.Now.ToString("yyyy-MM-dd");
        //        // getting the pests
        //        System.Data.DataTable dtPests = Tasks.IPM.ReadTaxonValues(ref _Date, source);// new DataTable();

        //        // getting the CollectionIDs
        //        if (source == Tasks.IPM.TaxonSource.Pest || source == Tasks.IPM.TaxonSource.Bycatch)
        //        {
        //            this._CollectionIDs = new Dictionary<int, int>();
        //            foreach (System.Windows.Forms.DataGridViewColumn C in dataGridView.Columns)
        //            {
        //                int CollectionID;
        //                if (C.Tag != null && int.TryParse(C.Tag.ToString(), out CollectionID))
        //                    _CollectionIDs.Add(C.Index, CollectionID);
        //            }

        //            // setting the values
        //            foreach (System.Windows.Forms.DataGridViewRow R in dataGridView.Rows)
        //            {
        //                if (R.Tag != null)
        //                {
        //                    foreach (System.Windows.Forms.DataGridViewCell C in R.Cells)
        //                    {
        //                        if (_CollectionIDs.ContainsKey(C.ColumnIndex))
        //                        {
        //                            System.Data.DataRow[] rr = dtPests.Select("CollectionID = " + _CollectionIDs[C.ColumnIndex].ToString() + " AND ModuleUri = '" + R.Tag.ToString() + "'");
        //                            if (rr.Length > 0)
        //                            {
        //                                string Value = rr[0]["NumberValue"].ToString();
        //                                if (!rr[0]["Notes"].Equals(System.DBNull.Value) && rr[0]["Notes"].ToString().Length > 0)
        //                                    Value += " " + rr[0]["Notes"].ToString();
        //                                C.Value = Value;
        //                            }
        //                            else if (R.Tag.ToString() == "-1")
        //                            {
        //                                C.Value = "x";
        //                            }
        //                            else
        //                            {
        //                                C.Value = null;
        //                            }
        //                        }
        //                    }
        //                }
        //                else { }
        //            }
        //            dataGridView.Columns[2].HeaderText = _Date;
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    finally
        //    {
        //        this.Cursor = System.Windows.Forms.Cursors.Default;
        //        Tasks.IPM.CurrentState = State.Editing;
        //    }
        //}

        #endregion

        #region Adding Collections and specimen

        private void SetCollectionControls(System.Data.DataRow R, System.Drawing.Image image)
        {
            if (R["Type"].ToString() != "trap")
            {
                string Collection = R["CollectionName"].ToString();
                this.buttonCollectionsAdd.Text = "Add pests in " + R["Type"].ToString().ToLower() + " " + Collection;
                this.buttonCollectionsAdd.Image = image;
                this.buttonCollectionsAdd.Tag = R;
                this.tabPageCollections.Text = "Pests in " + Collection;
            }
            //if (R["CollectionParentID"].Equals(System.DBNull.Value))
            //{
            //    this.buttonParentCollection.Enabled = false;
            //}
            //else
            //{
            //    this.buttonParentCollection.Enabled = true;
            //}
        }

        private void collectionAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CollectionsAdd();
        }

        private void specimenAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SpecimensAdd();
        }

        private void buttonCollectionsAdd_Click(object sender, EventArgs e)
        {
            this.CollectionsAdd();
        }

        private void buttonSpecimensAdd_Click(object sender, EventArgs e)
        {
            this.SpecimensAdd();
        }

        private void buttonTransactionAdd_Click(object sender, EventArgs e)
        {
            this.TransactionAdd();
        }

        private void SpecimensAdd()
        {
            if (this.treeViewCollection.SelectedNode == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select the collection in the tree");
                return;
            }
            if (System.Windows.Forms.MessageBox.Show("Do you want to add an observation of pests on a specimen in collection \r\n" + this.treeViewCollection.SelectedNode.Text, "Pests in " + this.treeViewCollection.SelectedNode.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int CollectionID;
                if (this.treeViewCollection.SelectedNode.Tag != null)
                {
                    try
                    {
                        System.Data.DataRow r = (System.Data.DataRow)this.treeViewCollection.SelectedNode.Tag;
                        if (int.TryParse(r["CollectionID"].ToString(), out CollectionID))
                        {
                            if (DiversityCollection.Tasks.IPM.AddSpecimen(CollectionID, this.DateSQL))
                                this.initGridViewSpecimens();
                            //else
                            //{

                            //}
                            //string SQL = "SELECT  " +
                            //    "CASE WHEN P.AccessionNumber <> '' THEN P.AccessionNumber ELSE CASE WHEN S.AccessionNumber <> '' THEN S.AccessionNumber ELSE '' END END AS [Accession number], " +
                            //    "CASE WHEN P.AccessionNumber <> '' THEN P.AccessionNumber ELSE CASE WHEN S.AccessionNumber <> '' THEN S.AccessionNumber ELSE '[' + CAST( P.CollectionSpecimenID AS VARCHAR) + '-' + CAST( P.SpecimenPartID AS VARCHAR) + ']' END END " +
                            //    " + ' ' + CASE WHEN P.PartSublabel <> '' THEN P.PartSublabel ELSE '' END " +
                            //    " + ' (' + P.MaterialCategory + ')' AS Specimen, " +
                            //    "S.CollectionSpecimenID, P.SpecimenPartID " +
                            //    "FROM CollectionSpecimen AS S INNER JOIN " +
                            //    "CollectionSpecimenPart AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID AND (S.AccessionNumber <> '' OR P.AccessionNumber <> '') " +
                            //    "WHERE P.CollectionID = " + CollectionID.ToString();
                            //System.Data.DataTable dt = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                            //if (dt.Rows.Count > 0)
                            //{
                            //    System.Collections.Generic.List<int> HiddenColumns = new List<int>();
                            //    HiddenColumns.Add(2);
                            //    HiddenColumns.Add(3);
                            //    DiversityWorkbench.Forms.FormGetRowFromTable f = new DiversityWorkbench.Forms.FormGetRowFromTable("Specimen", "Please select the specimen where the pest was found", dt, true, HiddenColumns);
                            //    f.setFilterColumn(0);
                            //    f.setOperator(1);
                            //    f.ShowDialog();
                            //    if (f.DialogResult == DialogResult.OK && f.SeletedRow() != null)
                            //    {
                            //        System.Data.DataRow R = f.SeletedRow();
                            //        SQL = "INSERT INTO CollectionTask " +
                            //            "(CollectionID, TaskID, DisplayText, " +
                            //            "CollectionSpecimenID, SpecimenPartID, TaskStart, " +
                            //            "ResponsibleAgent, ResponsibleAgentURI) " +
                            //            "SELECT " + CollectionID.ToString() + ", P.TaskID, '" + R["Specimen"].ToString() + "', " 
                            //            + R["CollectionSpecimenID"].ToString() + ", " + R["SpecimenPartID"].ToString() + ", CAST('2024-03-06' AS date), " +
                            //            "U.CombinedNameCache, U.AgentURI " +
                            //            "FROM UserProxy AS U CROSS JOIN " +
                            //            "Task AS I INNER JOIN " +
                            //            "Task AS M ON M.TaskParentID = I.TaskID AND M.Type = 'Monitoring' AND M.DisplayText = 'Monitoring' INNER JOIN " +
                            //            "Task P ON P.TaskParentID = M.TaskID AND P.Type = 'Pest' AND P.DisplayText = 'Pest'  " +
                            //            "WHERE (I.Type = 'IPM') AND (I.DisplayText = 'IPM') AND (U.ID = dbo.UserID()) ";
                            //        if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                            //            this.initGridViewSpecimens();
                            //    }
                            //    else System.Windows.Forms.MessageBox.Show("Nothing selected");
                            //}
                            //else
                            //{
                            //    System.Windows.Forms.MessageBox.Show("So far no specimen are registered for this collection");
                            //}
                        }
                    }
                    catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                }
            }

        }

        private void CollectionsAdd()
        {
            if (this.treeViewCollection.SelectedNode == null || this.buttonCollectionsAdd.Tag == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select the collection in the tree that is not a trap");
                return;
            }
            try
            {
                System.Data.DataRow R = (System.Data.DataRow)this.buttonCollectionsAdd.Tag;
                if (R["Type"].ToString().ToLower() == "trap")
                {
                    System.Windows.Forms.MessageBox.Show("Please select the collection in the tree that is not a trap");
                    return;
                }
                if (System.Windows.Forms.MessageBox.Show("Do you want to add an observation of pests in " + R["Type"].ToString() + " \r\n" + R["CollectionName"].ToString(), "Pests in " + R["Type"].ToString() + R["CollectionName"].ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int CollectionID;
                    System.Data.DataRow r = (System.Data.DataRow)this.treeViewCollection.SelectedNode.Tag;
                    if (int.TryParse(r["CollectionID"].ToString(), out CollectionID))
                    {
                        //string Error = "";
                        //bool CollectionAdded = false;
                        if (DiversityCollection.Tasks.IPM.AddCollection(CollectionID, this.treeViewCollection.SelectedNode.Text, this.DateSQL))
                            this.initGridViewCollections(this.DateSQL);
                        //if (DiversityCollection.Tasks.IPM.cr(CollectionID))
                        //    this.initGridViewSpecimens();
                    }
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void TransactionAdd()
        {
            if (this.treeViewCollection.SelectedNode == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select the collection in the tree");
                return;
            }
            if (System.Windows.Forms.MessageBox.Show("Do you want to add an observation of pests in a convolute or similar in a collection \r\n" + this.treeViewCollection.SelectedNode.Text, "Pests in " + this.treeViewCollection.SelectedNode.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int CollectionID;
                if (this.treeViewCollection.SelectedNode.Tag != null)
                {
                    try
                    {
                        System.Data.DataRow r = (System.Data.DataRow)this.treeViewCollection.SelectedNode.Tag;
                        if (int.TryParse(r["CollectionID"].ToString(), out CollectionID))
                        {
                            if (DiversityCollection.Tasks.IPM.AddTransaction(CollectionID, this.DateSQL))
                                this.initGridViewTransaction();
                        }
                    }
                    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                }
            }

        }



        #endregion

        #region Writing values
        private void dataGridViewResults_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.WriteRecordingValue(IPM.RecordingTarget.TrapPest, e.RowIndex, e.ColumnIndex))
            {
                this.CleanCellWhenWritingFailed(dataGridViewResults, e);
            }
            //this.WriteRecordValue(this.dataGridViewResults, e.RowIndex, e.ColumnIndex);
        }
        private void dataGridViewBycatch_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.WriteRecordingValue(IPM.RecordingTarget.TrapBycatch, e.RowIndex, e.ColumnIndex))
                this.CleanCellWhenWritingFailed(dataGridViewBycatch, e);
            //this.WriteRecordValue(this.dataGridViewBycatch, e.RowIndex, e.ColumnIndex, Tasks.IPM.MonitoringTarget.Trap, Tasks.IPM.TaxonSource.Bycatch);
        }

        private void dataGridViewCollections_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.WriteRecordingValue(IPM.RecordingTarget.CollectionPest, e.RowIndex, e.ColumnIndex))
                this.CleanCellWhenWritingFailed(dataGridViewCollections, e);
            //this.WriteRecordValue(this.dataGridViewCollections, e.RowIndex, e.ColumnIndex, Tasks.IPM.MonitoringTarget.Collection, Tasks.IPM.TaxonSource.Pest);
        }

        private void dataGridViewSpecimens_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.WriteRecordingValue(IPM.RecordingTarget.SpecimenPest, e.RowIndex, e.ColumnIndex))
                this.CleanCellWhenWritingFailed(dataGridViewSpecimens, e);
            //this.WriteRecordValue(this.dataGridViewSpecimens, e.RowIndex, e.ColumnIndex, Tasks.IPM.MonitoringTarget.Specimen, Tasks.IPM.TaxonSource.Pest);
        }

        private void dataGridViewTransaction_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.WriteRecordingValue(IPM.RecordingTarget.TransactionPest, e.RowIndex, e.ColumnIndex))
                this.CleanCellWhenWritingFailed(dataGridViewTransaction, e);
            //this.WriteRecordValue(this.dataGridViewTransaction, e.RowIndex, e.ColumnIndex, Tasks.IPM.MonitoringTarget.Transaction, Tasks.IPM.TaxonSource.Pest);
        }

        private void CleanCellWhenWritingFailed(System.Windows.Forms.DataGridView dataGridView, DataGridViewCellEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Failed to write data", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = null;
        }

        //private void WriteRecordValue(System.Windows.Forms.DataGridView dataGridView, int RowIndex, int ColumnIndex, IPM.MonitoringTarget monitoringTarget = Tasks.IPM.MonitoringTarget.Trap, IPM.TaxonSource taxonSource = Tasks.IPM.TaxonSource.Pest)
        //{
        //    try
        //    {
        //        if (ColumnIndex < 3 || RowIndex < 0 || Tasks.IPM.CurrentState == State.Reading)
        //            return;

        //        switch(monitoringTarget)
        //        {
        //            case Tasks.IPM.MonitoringTarget.Specimen:
        //                break;
        //            default:
        //                break;
        //        }
        //        int CollectionID;
        //        this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

        //        if (dataGridView.Columns[ColumnIndex].Tag != null) // && int.TryParse(this.dataGridViewResults.Columns[ColumnIndex].Tag.ToString(), out CollectionID))
        //        {
        //            System.Data.DataRow row = (System.Data.DataRow)dataGridView.Columns[ColumnIndex].Tag;
        //            int.TryParse(row["CollectionID"].ToString(), out CollectionID);
        //            string RecordKey = dataGridView.Rows[RowIndex].Tag.ToString();
        //            if (RecordKey.Length > 0)
        //            {
        //                double? Count = null;
        //                string Notes = "";
        //                string State = "";
        //                string Error = "";
        //                if (dataGridView.Rows[RowIndex].Cells[ColumnIndex].Value != null)
        //                {
        //                    this.ReadCellContent(dataGridView, RowIndex, ColumnIndex, ref Count, ref State, ref Notes);
        //                    if (monitoringTarget == Tasks.IPM.MonitoringTarget.Specimen)
        //                    {
        //                        if (dataGridView.Columns[ColumnIndex].Tag != null)
        //                        {
        //                            int CollectionSpecimenID;
        //                            int SpecimenPartID;
        //                            System.Data.DataRow R = (System.Data.DataRow)dataGridView.Columns[ColumnIndex].Tag;
        //                        }
        //                        if (!this.WriteRecordValue(CollectionID, RecordKey, Count, ref Error, Notes, taxonSource, monitoringTarget))
        //                        {
        //                            dataGridView.Rows[RowIndex].Cells[ColumnIndex].Value = null;
        //                            System.Windows.Forms.MessageBox.Show("Failed to write data: " + Error);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (!this.WriteRecordValue(CollectionID, RecordKey, Count, ref Error, Notes, taxonSource, monitoringTarget))
        //                        {
        //                            dataGridView.Rows[RowIndex].Cells[ColumnIndex].Value = null;
        //                            System.Windows.Forms.MessageBox.Show("Failed to write data: " + Error);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    finally
        //    {
        //        this.Cursor = System.Windows.Forms.Cursors.Default;
        //    }
        //}

        private bool WriteRecordingValue(IPM.RecordingTarget recordingTarget, int RowIndex, int ColumnIndex)
        {
            bool Success = false;
            try
            {
                if (ColumnIndex < 3 || RowIndex < 0 || Tasks.IPM.CurrentState == State.Reading)
                    return true; // Changed to true as no values are written in header columns

                if (this.DataGridView(recordingTarget).Rows[RowIndex].Cells[ColumnIndex].Value == null)
                    return true; // After removal of invalid entry

                int CollectionID;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                if (this.DataGridView(recordingTarget).Columns[ColumnIndex].Tag != null) // && int.TryParse(this.dataGridViewResults.Columns[ColumnIndex].Tag.ToString(), out CollectionID))
                {
                    System.Data.DataRow row = (System.Data.DataRow)this.DataGridView(recordingTarget).Columns[ColumnIndex].Tag;
                    int.TryParse(row["CollectionID"].ToString(), out CollectionID);
                    string RecordKey = this.DataGridView(recordingTarget).Rows[RowIndex].Tag.ToString();
                    int? CollectionSpecimenID = null;
                    int? SpecimenPartID = null;
                    int? TransactionID = null;
                    if (RecordKey.Length > 0)
                    {
                        double? Count = null;
                        string Notes = "";
                        string State = "";
                        string Error = "";
                        if (this.DataGridView(recordingTarget).Rows[RowIndex].Cells[ColumnIndex].Value != null)
                        {
                            this.ReadCellContent(this.DataGridView(recordingTarget), RowIndex, ColumnIndex, ref Count, ref State, ref Notes);
                            int i;
                            switch (recordingTarget)
                            {
                                case IPM.RecordingTarget.SpecimenPest:
                                    if (int.TryParse(row["CollectionSpecimenID"].ToString(), out i)) CollectionSpecimenID = i;
                                    else return false;
                                    if (int.TryParse(row["SpecimenPartID"].ToString(), out i)) SpecimenPartID = i;
                                    else return false;
                                    break;
                                case IPM.RecordingTarget.TransactionPest:
                                    if (int.TryParse(row["TransactionID"].ToString(), out i)) TransactionID = i;
                                    else return false;
                                    break;
                                default:
                                    break;
                            }
                            if (!this.WriteRecordingValue(recordingTarget, CollectionID, RecordKey, Count, ref Error, State, Notes, CollectionSpecimenID, SpecimenPartID, TransactionID))
                            {
                                this.DataGridView(recordingTarget).Rows[RowIndex].Cells[ColumnIndex].Value = null;
                                System.Windows.Forms.MessageBox.Show("Failed to write data: " + Error);
                            }
                            else Success = true;
                            //if (monitoringTarget == Tasks.IPM.MonitoringTarget.Specimen)
                            //{
                            //    if (this.DataGridView(recordingTarget).Columns[ColumnIndex].Tag != null)
                            //    {
                            //        int CollectionSpecimenID;
                            //        int SpecimenPartID;
                            //        System.Data.DataRow R = (System.Data.DataRow)this.DataGridView(recordingTarget).Columns[ColumnIndex].Tag;
                            //    }
                            //    if (!this.WriteRecordValue(CollectionID, RecordKey, Count, ref Error, Notes, taxonSource, monitoringTarget))
                            //    {
                            //        this.DataGridView(recordingTarget).Rows[RowIndex].Cells[ColumnIndex].Value = null;
                            //        System.Windows.Forms.MessageBox.Show("Failed to write data: " + Error);
                            //    }
                            //}
                            //else
                            //{
                            //    if (!this.WriteRecordValue(CollectionID, RecordKey, Count, ref Error, Notes, taxonSource, monitoringTarget))
                            //    {
                            //        this.DataGridView(recordingTarget).Rows[RowIndex].Cells[ColumnIndex].Value = null;
                            //        System.Windows.Forms.MessageBox.Show("Failed to write data: " + Error);
                            //    }
                            //}
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            return Success;
        }

        private bool WriteRecordingValue(IPM.RecordingTarget recordingTarget, int CollectionID, string RecordUri, double? Count, ref string Error, string State = "", string Notes = "",
            int? CollectionSpecimenID = null, int? SpecimenPartID = null, int? TransactionID = null)
        {
            bool OK = false;
            try
            {
                Tasks.Taxa.TaxonRecord record = RecordDicts.ChecklistRecordings(recordingTarget)[RecordUri];

                bool TrapAdded = false;
                Error = "";
                OK = Tasks.IPM.WriteRecordingValue(recordingTarget, record, ref Error, ref TrapAdded, CollectionID, RecordUri, Count, this.DateSQL, 
                    this.userControlModuleRelatedEntryResponsible.textBoxValue.Text, this.userControlModuleRelatedEntryResponsible.labelURI.Text, 
                    State, Notes, CollectionSpecimenID, SpecimenPartID, TransactionID);
                if (TrapAdded)
                    this.initTreeViewCollection();
                if (Error.Length > 0)
                {
                    OK = false;
                    System.Windows.Forms.MessageBox.Show(Error);
                }
                return OK;

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        //private System.Collections.Generic.Dictionary<string, string> SQLvalues(int CollectionID, string RecordUri, double? Count, ref string Error, string State = "", string Notes = "",
        //    int? CollectionSpecimenID = null, int? SpecimenPartID = null, int? TransactionID = null)
        //{
        //    System.Collections.Generic.Dictionary<string, string> dict = new Dictionary<string, string>();

        //    return dict;
        //}



        //private bool WriteRecordingValue(IPM.RecordingTarget recordingTarget, int CollectionID, string RecordUri, double? Count, ref string Error,
        //    string Notes = "", int? CollectionSpecimenID = null, int? SpecimenPartID = null, int? TransactionID = null)
        //{
        //    bool OK = false;
        //    try
        //    {
        //        Tasks.Taxa.Record record = RecordDicts.ChecklistRecordings(recordingTarget)[RecordUri];

        //        // Current date
        //        //if (_Date.Length == 0)
        //        //    _Date = System.DateTime.Now.ToString("yyyy-MM-dd");
        //        //string DateSQL = "CONVERT(DATETIME, '" + _Date + " 00:00:00', 102)";
        //        bool TrapAdded = false;
        //        OK = Tasks.IPM.WriteRecordingValue(recordingTarget, record, CollectionID, RecordUri, Count, this.DateSQL, this.userControlModuleRelatedEntryResponsible.textBoxValue.Text, this.userControlModuleRelatedEntryResponsible.labelURI.Text, ref Error, ref TrapAdded, Notes, CollectionSpecimenID, SpecimenPartID, TransactionID);
        //        if (TrapAdded)
        //            this.initTreeViewCollection();
        //        if (Error.Length > 0)
        //        {
        //            OK = false;
        //            System.Windows.Forms.MessageBox.Show(Error);
        //        }
        //        return OK;

        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return OK;
        //}




        //private bool WriteRecordValue(int CollectionID, string RecordUri, double? Count, ref string Error, 
        //    string Notes = "", IPM.TaxonSource taxonSource = Tasks.IPM.TaxonSource.Pest, IPM.MonitoringTarget monitoringTarget = Tasks.IPM.MonitoringTarget.Trap, 
        //    int? CollectionSpecimenID = null, int? SpecimenPartID = null)
        //{
        //    bool OK = false;
        //    try
        //    {
        //        Tasks.Taxa.Record record = RecordDicts.ChecklistRecords(taxonSource)[RecordUri];

        //        // Current date
        //        //if (_Date.Length == 0)
        //        //    _Date = System.DateTime.Now.ToString("yyyy-MM-dd");
        //        //string DateSQL = "CONVERT(DATETIME, '" + _Date + " 00:00:00', 102)";
        //        bool TrapAdded = false;
        //        OK = Tasks.IPM.WriteRecordValue(record, CollectionID, RecordUri, Count, this.DateSQL, this.userControlModuleRelatedEntryResponsible.textBoxValue.Text, this.userControlModuleRelatedEntryResponsible.labelURI.Text, ref Error, ref TrapAdded, taxonSource, monitoringTarget, Notes);
        //        if (TrapAdded)
        //            this.initTreeViewCollection();
        //        if (Error.Length > 0)
        //        {
        //            OK = false;
        //            System.Windows.Forms.MessageBox.Show(Error);
        //        }
        //        return OK;

        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return OK;
        //}

        private void ReadCellContent(System.Windows.Forms.DataGridView dataGridView, int RowIndex, int ColumnIndex, ref double? Count, ref string State, ref string Notes)
        {
            try
            {
                if (dataGridView.Rows[RowIndex].Cells[ColumnIndex].Value != null)
                {
                    string Value = dataGridView.Rows[RowIndex].Cells[ColumnIndex].Value.ToString();
                    string[] Content = Value.Split(new string[] {"\r\n"}, StringSplitOptions.None);
                    if(Content.Length > 0)
                    {
                        double d;
                        if (double.TryParse(Content[0], out d))
                            Count = d;
                        else Count = null;
                    }
                    else Count = null;
                    if (Content.Length > 1)
                        State = Content[1];
                    else State = "";
                    if (Content.Length > 2)
                        Notes = Content[2];
                    else Notes = "";
                    //if (Value.IndexOf(" ") > -1)
                    //{
                    //    string C = Value.Substring(0, Value.IndexOf(" ")).Trim();
                    //    if (!double.TryParse(C, out Count))
                    //        Notes = Value;
                    //    else
                    //        Notes = Value.Substring(Value.IndexOf(" ")).Trim();

                    //}
                    //else
                    //{
                    //    if (!double.TryParse(Value.Replace(",", "."), out Count))
                    //    {
                    //        Notes = Value;
                    //    }
                    //}
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        //private int getTrapID(int CollectionID, string DateSQL)
        //{
        //    int TrapID = -1;
        //    string SQL = "SELECT C.CollectionTaskID, C.DisplayText " +
        //        "FROM CollectionTask AS C INNER JOIN " +
        //        "Task AS T ON C.TaskID = T.TaskID " +
        //        "WHERE (T.Type = N'Trap') " +
        //        "AND(C.TaskStart IS NULL OR C.TaskStart <= " + DateSQL + ") " +
        //        "AND(C.TaskEnd IS NULL OR C.TaskEnd >= " + DateSQL + " ) " +
        //        "AND (C.CollectionID = " + CollectionID.ToString() + ")";
        //    System.Data.DataTable dtTraps = new DataTable();
        //    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTraps);
        //    if (dtTraps.Rows.Count == 1)
        //    {
        //        if (!int.TryParse(dtTraps.Rows[0][0].ToString(), out TrapID))
        //        {
        //            TrapID = -1;
        //        }
        //    }
        //    else if (dtTraps.Rows.Count > 1) // more then 1 trap
        //    {
        //        DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtTraps, "DisplayText", "CollectionTaskID", "Please select a trap from the list", "Trap", "", false, true, true, DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Trap));
        //        f.ShowDialog();
        //        if (f.DialogResult == DialogResult.OK)
        //        {
        //            if (!int.TryParse(f.SelectedValue, out TrapID))
        //                TrapID = -1;
        //        }
        //    }
        //    else // no trap
        //    {
        //        if (System.Windows.Forms.MessageBox.Show("There is no trap. Do you want to add a trap containing this pest", "No trap", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //        {
        //            // find monitoring for Collection
        //            int MonitoringID = -1;
        //            SQL = "SELECT C.CollectionTaskID, C.DisplayText " +
        //                "FROM CollectionTask AS C INNER JOIN " +
        //                "Task AS T ON C.TaskID = T.TaskID INNER JOIN " +
        //                "dbo.CollectionHierarchySuperior(" + CollectionID.ToString() + ") S ON S.CollectionID = C.CollectionID " +
        //                "AND (T.Type = N'Monitoring') " +
        //                "AND(C.TaskStart IS NULL OR C.TaskStart <= " + DateSQL + ") " +
        //                "AND(C.TaskEnd IS NULL OR C.TaskEnd >= " + DateSQL + ") " +
        //                "AND (C.CollectionID = " + CollectionID.ToString() + ")";
        //            System.Data.DataTable dtMonitor = new DataTable();
        //            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtMonitor);
        //            if (dtMonitor.Rows.Count == 1)
        //            {
        //                if (!int.TryParse(dtMonitor.Rows[0][0].ToString(), out MonitoringID))
        //                    TrapID = -1;
        //            }
        //            else if (dtMonitor.Rows.Count > 1)
        //            {
        //                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtMonitor, "DisplayText", "CollectionTaskID", "Please select a monitoring from the list", "Monitoring", "", false, true, true, DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Graph));
        //                f.ShowDialog();
        //                if (f.DialogResult == DialogResult.OK)
        //                {
        //                    if (!int.TryParse(f.SelectedValue, out MonitoringID))
        //                        TrapID = -1;
        //                    else
        //                    {
        //                        int TrapTaskID = -1;
        //                        // try to find the trap task
        //                        SQL = "SELECT T.DisplayText, T.TaskID " +
        //                            "FROM[dbo].[CollectionTask] C " +
        //                            "INNER JOIN Task M ON  C.TaskID = M.TaskID AND M.Type = 'Monitoring' AND C.CollectionTaskID = " + MonitoringID.ToString() + " " +
        //                            "INNER JOIN Task T ON T.TaskParentID = M.TaskID AND T.Type = 'Trap'";
        //                        System.Data.DataTable dtTrapTask = new DataTable();
        //                        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTrapTask);
        //                        if (dtTrapTask.Rows.Count == 1)
        //                        {
        //                            int.TryParse(dtTrapTask.Rows[0][0].ToString(), out TrapTaskID);
        //                        }
        //                        else if (dtTrapTask.Rows.Count > 1)
        //                        {
        //                            DiversityWorkbench.Forms.FormGetStringFromList fTT = new DiversityWorkbench.Forms.FormGetStringFromList(dtMonitor, "DisplayText", "TaskID", "Please select a trap from the list", "Trap", "", false, true, true, DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Trap));
        //                            fTT.ShowDialog();
        //                            if (fTT.DialogResult == DialogResult.OK)
        //                            {
        //                                if(int.TryParse(fTT.SelectedValue, out TrapTaskID))
        //                                {
        //                                    // done
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            System.Windows.Forms.MessageBox.Show("So far no traps had been defined for monitoring of the selected collection. Please turn to your administrator to define a trap for the monitoring task");
        //                            TrapID = -1;
        //                        }
        //                        if(TrapTaskID > -1)
        //                        {
        //                            string TrapTitle = "";
        //                            DiversityWorkbench.Forms.FormGetString fTrap = new DiversityWorkbench.Forms.FormGetString("New trap", "Please enter the name for the new trap", "", DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Trap));
        //                            fTrap.ShowDialog();
        //                            if (fTrap.DialogResult == DialogResult.OK && fTrap.String.Length > 0)
        //                            {
        //                                TrapTitle = fTrap.String;
        //                                SQL = "INSERT INTO CollectionTask (CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, DisplayText, TaskStart, ResponsibleAgent, ResponsibleAgentURI) " +
        //                                    "VALUES(" + MonitoringID.ToString() + ", " + CollectionID.ToString() + ", " + TrapTaskID.ToString() + ", 1, '" + TrapTitle + "', " + DateSQL + ", '" + this.userControlModuleRelatedEntryResponsible.textBoxValue.Text.ToString() + "', '" + this.userControlModuleRelatedEntryResponsible.labelURI.Text.ToString() + "') " +
        //                                    "; SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];";
        //                                int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out TrapID);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                System.Windows.Forms.MessageBox.Show("So far no monitoring has been defined for the selected collection. Please turn to your administrator to define a monitoring task");
        //                TrapID = -1;
        //            }
        //        }
        //        else
        //        {
        //            TrapID = -1;
        //        }
        //    }
        //    return TrapID;
        //}

        //private IPM IPM()
        //{
        //    if (this._IPM == null)
        //        this._IPM = new IPM();
        //    return this._IPM;
        //}

        #region getting infos from grid
        private void dataGridViewResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.dataGridView_CellClick(IPM.RecordingTarget.TrapPest, e);
            //this.dataGridView_CellClick(this.dataGridViewResults, Tasks.IPM.TaxonSource.Pest, e);
        }

        private void dataGridViewSpecimens_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.dataGridView_CellClick(IPM.RecordingTarget.SpecimenPest, e);
            //this.dataGridView_CellClick(this.dataGridViewSpecimens, Tasks.IPM.TaxonSource.Pest, e, IPM.MonitoringTarget.Specimen);
        }

        private void dataGridViewCollections_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.dataGridView_CellClick(IPM.RecordingTarget.CollectionPest, e);
            //this.dataGridView_CellClick(this.dataGridViewCollections, Tasks.IPM.TaxonSource.Pest, e, IPM.MonitoringTarget.Collection);
        }

        private void dataGridViewBycatch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.dataGridView_CellClick(IPM.RecordingTarget.TrapBycatch, e);
            //this.dataGridView_CellClick(this.dataGridViewBycatch, Tasks.IPM.TaxonSource.Bycatch, e);
            //return;
        }
        private void dataGridViewTransaction_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.dataGridView_CellClick(IPM.RecordingTarget.TransactionPest, e);
            //this.dataGridView_CellClick(this.dataGridViewTransaction, Tasks.IPM.TaxonSource.Pest, e, IPM.MonitoringTarget.Transaction);
            //return;
        }

        private void dataGridView_CellClick(IPM.RecordingTarget recordingTarget, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                else if (e.RowIndex >= 0 && e.ColumnIndex > 2)
                {
                    try
                    {
                        TaxonRecord record = RecordDicts.ChecklistRecordings(recordingTarget)[this.DataGridView(recordingTarget).Rows[e.RowIndex].Tag.ToString()];
                        System.Drawing.Image image = (System.Drawing.Image)this.DataGridView(recordingTarget).Rows[e.RowIndex].Cells[2].Value;
                        RecordData recordData = new RecordData();
                        if (this.DataGridView(recordingTarget).Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                        {
                            string[] Data = this.DataGridView(recordingTarget).Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                            if (Data.Length > 0) recordData.Count = double.Parse(Data[0]);
                            if (Data.Length > 1) recordData.State = Data[1];
                            if (Data.Length > 2) recordData.Notes = Data[2];
                        }
                        if (this.GetRecordData(record, image, ref recordData, this.DataGridView(recordingTarget).Columns[e.ColumnIndex].HeaderText.Replace("\r\n", " "), this._Date))
                        {
                            string data = recordData.Count.ToString() + "\r\n" + recordData.State + "\r\n" + recordData.Notes;
                            this.DataGridView(recordingTarget).Rows[e.RowIndex].Cells[e.ColumnIndex].Value = data;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                    if (this.DataGridView(recordingTarget).Columns[e.ColumnIndex].Tag != null)
                    {
                        if (this.DataGridView(recordingTarget).Columns[e.ColumnIndex].Tag.GetType() == typeof(System.Data.DataRow))
                        {
                            System.Data.DataRow row = (System.Data.DataRow)this.DataGridView(recordingTarget).Columns[e.ColumnIndex].Tag;
                            string Title = this.DataGridView(recordingTarget).Columns[e.ColumnIndex].HeaderText.Replace("\r\n", ". ");
                            System.Windows.Forms.Button button = null;
                            switch (recordingTarget)
                            {
                                case IPM.RecordingTarget.TrapPest:
                                    break;
                                case IPM.RecordingTarget.CollectionPest:
                                    button = this.buttonCollectionDetails;
                                    break;
                                case IPM.RecordingTarget.SpecimenPest:
                                    button = this.buttonSpecimenDetails;
                                    break;
                                case IPM.RecordingTarget.TransactionPest:
                                    button = this.buttonTransactionDetails;
                                    break;
                            }
                            if (button != null)
                                this.SetDetailsTarget(row, button, Title);
                        }
                    }
                    return;
                }
                string RecordUri = this.DataGridView(recordingTarget).Rows[e.RowIndex].Tag.ToString();
                System.Collections.Generic.List<Tasks.Resource> webResources = null;
                switch (e.ColumnIndex)
                {
                    case 1:
                        webResources = new List<Resource>();
                        System.Collections.Generic.Dictionary<string, TaxonRecord> Records = RecordDicts.ChecklistRecordings(recordingTarget);
                        if (Records.ContainsKey(RecordUri))
                        {
                            foreach (System.Collections.Generic.KeyValuePair<int, Resource> KV in Records[RecordUri].Infos)
                            {
                                if (KV.Value.Uri != null && KV.Value.Uri.ToString().Length > 0)
                                    webResources.Add(KV.Value);
                            }
                        }
                        break;
                    case 2:
                        webResources = new List<Resource>();
                        System.Collections.Generic.Dictionary<string, TaxonRecord> RecordImages = RecordDicts.ChecklistRecordings(recordingTarget);
                        if (RecordImages.ContainsKey(RecordUri))
                        {
                            foreach (System.Collections.Generic.KeyValuePair<int, Resource> KV in RecordImages[RecordUri].Images)
                            {
                                if (KV.Value.Uri != null && KV.Value.Uri.ToString().Length > 0)
                                    webResources.Add(KV.Value);
                            }
                        }
                        break;
                    case 0:
                        break;
                    default:

                        int CollectionID;
                        if (this.DataGridView(recordingTarget).Columns[e.ColumnIndex].Tag != null &&
                            int.TryParse(this.DataGridView(recordingTarget).Columns[e.ColumnIndex].Tag.ToString(), out CollectionID) &&
                            Tasks.IPM.CurrentState == State.Editing)
                        {
                            System.Windows.Forms.TreeNode node = this.getCollectionNode(CollectionID);
                            if (node != null)
                            {
                                this.treeViewCollection.SelectedNode = node;
                                node.EnsureVisible();
                            }
                        }
                        break;
                }
                if (webResources != null && webResources.Count > 0)
                {
                    FormWebview f = new FormWebview(webResources);
                    f.Width = this.Width - 20;
                    f.Height = this.Height - 20;
                    f.StartPosition = FormStartPosition.CenterParent;
                    f.ShowDialog();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }



        //private void dataGridView_CellClick(System.Windows.Forms.DataGridView dataGridView, IPM.TaxonSource source, DataGridViewCellEventArgs e, IPM.MonitoringTarget monitoringTarget = IPM.MonitoringTarget.Trap)
        //{
        //    try
        //    {
        //        if (e.RowIndex < 0)
        //            return;
        //        else if (e.RowIndex >= 0 && e.ColumnIndex > 2)
        //        {
        //            try
        //            {
        //                Record record = RecordDicts.ChecklistRecords(source)[dataGridView.Rows[e.RowIndex].Tag.ToString()];
        //                System.Drawing.Image image = (System.Drawing.Image)dataGridView.Rows[e.RowIndex].Cells[2].Value;
        //                RecordData recordData = new RecordData();
        //                if (dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
        //                {
        //                    string[] Data = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.None);
        //                    if (Data.Length > 0) recordData.Count = double.Parse(Data[0]);
        //                    if (Data.Length > 1) recordData.Notes = Data[1];
        //                    if (Data.Length > 2) recordData.State = Data[2];
        //                }
        //                if (this.GetRecordData(record, image, ref recordData, dataGridView.Columns[e.ColumnIndex].HeaderText.Replace("\r\n", " "), this._Date))
        //                {
        //                    string data = recordData.Count.ToString() + "\r\n" + recordData.State + "\r\n" + recordData.Notes;
        //                    dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = data;
        //                }
        //            }
        //            catch (System.Exception ex)
        //            {
        //                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //            }
        //            if (dataGridView.Columns[e.ColumnIndex].Tag != null)
        //            {
        //                if (dataGridView.Columns[e.ColumnIndex].Tag.GetType() == typeof(System.Data.DataRow))
        //                {
        //                    System.Data.DataRow row = (System.Data.DataRow)dataGridView.Columns[e.ColumnIndex].Tag;
        //                    string Title = dataGridView.Columns[e.ColumnIndex].HeaderText.Replace("\r\n", ". ");
        //                    System.Windows.Forms.Button button = null;
        //                    switch (monitoringTarget)
        //                    {
        //                        case IPM.MonitoringTarget.Trap:
        //                            break;
        //                        case IPM.MonitoringTarget.Collection:
        //                            button = this.buttonCollectionDetails;
        //                            break;
        //                        case IPM.MonitoringTarget.Specimen:
        //                            button = this.buttonSpecimenDetails;
        //                            break;
        //                        case IPM.MonitoringTarget.Transaction:
        //                            button = this.buttonTransactionDetails;
        //                            break;
        //                    }
        //                    if (button != null)
        //                        this.SetDetailsTarget(row, button, Title);
        //                }
        //            }
        //            return;
        //        }
        //        string RecordUri = dataGridView.Rows[e.RowIndex].Tag.ToString();
        //        System.Collections.Generic.List<Tasks.Resource> webResources = null;
        //        switch (e.ColumnIndex)
        //        {
        //            case 1:
        //                webResources = new List<Resource>();
        //                System.Collections.Generic.Dictionary<string, Record> Records = RecordDicts.ChecklistRecords(source);
        //                if (Records.ContainsKey(RecordUri))
        //                {
        //                    foreach (System.Collections.Generic.KeyValuePair<int, Resource> KV in Records[RecordUri].Infos)
        //                    {
        //                        if (KV.Value.Uri != null && KV.Value.Uri.ToString().Length > 0)
        //                            webResources.Add(KV.Value);
        //                    }
        //                }
        //                break;
        //            case 2:
        //                webResources = new List<Resource>();
        //                System.Collections.Generic.Dictionary<string, Record> RecordImages = RecordDicts.ChecklistRecords(source);
        //                if (RecordImages.ContainsKey(RecordUri))
        //                {
        //                    foreach (System.Collections.Generic.KeyValuePair<int, Resource> KV in RecordImages[RecordUri].Images)
        //                    {
        //                        if (KV.Value.Uri != null && KV.Value.Uri.ToString().Length > 0)
        //                            webResources.Add(KV.Value);
        //                    }
        //                }
        //                break;
        //            case 0:
        //                break;
        //            default:

        //                int CollectionID;
        //                if (dataGridView.Columns[e.ColumnIndex].Tag != null &&
        //                    int.TryParse(dataGridView.Columns[e.ColumnIndex].Tag.ToString(), out CollectionID) &&
        //                    Tasks.IPM.CurrentState == State.Editing)
        //                {
        //                    System.Windows.Forms.TreeNode node = this.getCollectionNode(CollectionID);
        //                    if (node != null)
        //                    {
        //                        this.treeViewCollection.SelectedNode = node;
        //                        node.EnsureVisible();
        //                    }
        //                }
        //                break;
        //        }
        //        if (webResources != null && webResources.Count > 0)
        //        {
        //            FormWebview f = new FormWebview(webResources);
        //            f.Width = this.Width - 20;
        //            f.Height = this.Height - 20;
        //            f.StartPosition = FormStartPosition.CenterParent;
        //            f.ShowDialog();
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        private bool GetRecordData(TaxonRecord record, System.Drawing.Image image, ref RecordData recordData, string Where, string date)
        {
            bool OK = true;
            try
            {
                FormIPM_Details f = new FormIPM_Details(record, image, recordData, Where, date);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    recordData = f.Data();
                    OK = true;
                }
                else OK = false;
            }
            catch (System.Exception ex)
            {
                OK = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        #endregion

        #region Details for objects in grids

        private void SetDetailsTarget(System.Data.DataRow Row, System.Windows.Forms.Button button, string Title)
        {
            button.Tag = Row;
            button.Text = "Show details for " + Title;
        }

        private void buttonCollectionDetails_Click(object sender, EventArgs e)
        {
            if(buttonCollectionDetails.Tag != null)
            {
                try
                {
                    System.Data.DataRow row = (System.Data.DataRow)this.buttonCollectionDetails.Tag;
                    int CollectionID;
                    if (int.TryParse(row["CollectionID"].ToString(), out CollectionID))
                    {
                        this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                        DiversityCollection.Forms.FormCollection form = new Forms.FormCollection(CollectionID, null); //, true);
                        form.Width = this.Width - 20;
                        form.Height = this.Height - 20;
                        form.StartPosition = FormStartPosition.CenterScreen;
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        form.ShowDialog();
                    }
                }
                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No collection selected");
            }
        }

        private void buttonSpecimenDetails_Click(object sender, EventArgs e)
        {
            if (this.buttonSpecimenDetails.Tag != null)
            {
                try
                {
                    System.Data.DataRow row = (System.Data.DataRow)this.buttonSpecimenDetails.Tag;
                    int SpecimenID;
                    if (int.TryParse(row["CollectionSpecimenID"].ToString(), out SpecimenID))
                    {
                        this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                        DiversityCollection.Forms.FormCollectionSpecimen form = new Forms.FormCollectionSpecimen(SpecimenID, true, false, Forms.FormCollectionSpecimen.ViewMode.SingleInspectionMode);
                        form.Width = this.Width - 20;
                        form.Height = this.Height - 20;
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        form.ShowDialog();
                    }
                }
                catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            }
        }

        private void buttonTransactionDetails_Click(object sender, EventArgs e)
        {
            if (this.buttonTransactionDetails.Tag != null)
            {
                try
                {
                    System.Data.DataRow row = (System.Data.DataRow)this.buttonTransactionDetails.Tag;
                    int TransactionID;
                    if (int.TryParse(row["TransactionID"].ToString(), out TransactionID))
                    {
                        this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                        DiversityCollection.Forms.FormTransaction form = new Forms.FormTransaction(TransactionID);
                        form.Height = this.Height - 20;
                        form.Width = this.Width - 20;
                        form.StartPosition = FormStartPosition.CenterParent;
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        form.ShowDialog();
                    }
                }
                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            }
        }

        #endregion

        #endregion


        #region Responsible

        private void initResponsible()
        {
            if (Settings.Default.ResponsibleAgent == null || Settings.Default.ResponsibleAgent.Length == 0)
            {
                string SQL = "select U.CombinedNameCache, U.AgentURI from UserProxy U where U.LoginName = USER_NAME();";
                System.Data.DataTable dt = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                if (dt.Rows.Count == 1)
                {
                    if (!dt.Rows[0][0].Equals(System.DBNull.Value) && dt.Rows[0][0].ToString().Length > 0)
                    {
                        Settings.Default.ResponsibleAgent = dt.Rows[0][0].ToString();
                        if (!dt.Rows[0][1].Equals(System.DBNull.Value) && dt.Rows[0][1].ToString().Length > 0)
                            Settings.Default.ResponsibleAgentURI = dt.Rows[0][1].ToString();
                        Settings.Default.Save();
                    }
                }
            }
            this.userControlModuleRelatedEntryResponsible.textBoxValue.Text = Settings.Default.ResponsibleAgent;
            this.userControlModuleRelatedEntryResponsible.labelURI.Text = Settings.Default.ResponsibleAgentURI;
            DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
            this.userControlModuleRelatedEntryResponsible.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
            this.userControlModuleRelatedEntryResponsible.setTableSource("UserProxy", "CombinedNameCache", "AgentURI", "");
        }

        #endregion

        #region Dates

        private void initGridViewDate()
        {
            this.dataGridViewDate.RowHeadersVisible = false;
            this.dataGridViewDate.ColumnHeadersVisible = false;
            this.dataGridViewDate.AllowUserToAddRows = false;
            this.dataGridViewDate.AllowUserToDeleteRows = false;
            try
            {
                if (this.dataGridViewDate.Rows.Count == 0)
                    this.dataGridViewDate.Rows.Add(1);
                while (this.dataGridViewDate.Columns.Count > 1)
                    this.dataGridViewDate.Columns.RemoveAt(0);

                System.Collections.Generic.List<string> Dates = Tasks.IPM.Dates();
                if (Dates.Count > 0)
                {
                    foreach(string D in Dates)
                    {
                        int ColumnIndex = this.dataGridViewDate.ColumnCount - 1;
                        this.dataGridViewDate.Rows[0].Cells[ColumnIndex].Value = D;
                        this.dataGridViewDate.Columns[this.dataGridViewDate.ColumnCount - 1].Width = 70;
                        System.Windows.Forms.DataGridViewColumn C = new DataGridViewColumn(this.dataGridViewDate.Rows[0].Cells[0]);
                        this.dataGridViewDate.Columns.Add(C);
                    }
                    this.dataGridViewDate.Columns.RemoveAt(this.dataGridViewDate.ColumnCount - 1);
                    this.dataGridViewDate.CurrentCell = this.dataGridViewDate.Rows[0].Cells[this.dataGridViewDate.ColumnCount - 1];
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private void dataGridViewDate_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.dataGridViewDate.SelectedCells[0].Value != null)
            {
                _Date = this.dataGridViewDate.SelectedCells[0].Value.ToString();

                this.initTreeViewCollection();
                //this.ReadPestValues(this.dataGridViewResults);
                //this.ReadBycatchValues();
                //int i = 0;
                this.initGridViewRecordings(IPM.RecordingTarget.TrapPest);
                this.initGridViewRecordings(IPM.RecordingTarget.TrapBycatch);
                this.initGridViewRecordings(IPM.RecordingTarget.CollectionPest);
                this.initGridViewRecordings(IPM.RecordingTarget.SpecimenPest);
                this.initGridViewRecordings(IPM.RecordingTarget.TransactionPest);

                //this.initGridViewRecords(this.dataGridViewCollections, IPM.TaxonSource.Pest, ref i,false, IPM.MonitoringTarget.Collection, this.DateSQL);
                //this.initGridViewRecords(this.dataGridViewSpecimens, IPM.TaxonSource.Pest, ref i, false, IPM.MonitoringTarget.Specimen, this.DateSQL);

                //this.ReadPestValues(this.dataGridViewCollections);
                //this.ReadPestValues(this.dataGridViewSpecimens);
            }
        }

        /// <summary>
        /// Adding an additional date that is not in the list so far and not the current date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDatesAdd_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetDate f = new DiversityWorkbench.Forms.FormGetDate(false, false);
            f.SetTitle("Select additional date");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                Tasks.IPM.AdditionalDate = f.Date;
            }
            else
                Tasks.IPM.AdditionalDate = null;
            this.setDateRangeControls();
        }

        private void buttonDatesFilter_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetDate f = new DiversityWorkbench.Forms.FormGetDate(false, true, true, Tasks.IPM.Start, Tasks.IPM.End);
            f.SetTitle("Start and/or end of dates");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                if (f.DateSelected)
                {
                    Tasks.IPM.Start = f.Date;
                }
                else
                {
                    Tasks.IPM.Start = null;
                }
                if (f.DateEndSelected)
                {
                    Tasks.IPM.End = f.EndDate;
                }
                else
                {
                    Tasks.IPM.End = null;
                }
                this.setDateRangeControls();
            }
        }

        private void buttonDatesRemoveStart_Click(object sender, EventArgs e)
        {
            Tasks.IPM.Start = null;
            this.setDateRangeControls();
        }

        private void buttonDatesRemoveEnd_Click(object sender, EventArgs e)
        {
            Tasks.IPM.End = null;
            this.setDateRangeControls();
        }

        private void setDateRangeControls()
        {
            if (Tasks.IPM.Start == null)
            {
                this.buttonDatesRemoveStart.Visible = false;
                this.labelDatesStart.Text = "";
                this.labelDatesStart.BackColor = System.Drawing.Color.Transparent;
            }
            else
            {
                this.buttonDatesRemoveStart.Visible = true;
                System.DateTime start = (DateTime)Tasks.IPM.Start;
                this.labelDatesStart.Text = "Starting at " + start.ToString("yyyy-MM-dd");
                this.labelDatesStart.BackColor = System.Drawing.Color.Pink;
            }
            if (Tasks.IPM.End == null)
            {
                this.buttonDatesRemoveEnd.Visible = false;
                this.labelDatesEnd.Text = "";
                this.labelDatesEnd.BackColor = System.Drawing.Color.Transparent;
            }
            else
            {
                this.buttonDatesRemoveEnd.Visible = true;
                System.DateTime end = (DateTime)Tasks.IPM.End;
                this.labelDatesEnd.Text = "Until " + end.ToString("yyyy-MM-dd");
                this.labelDatesEnd.BackColor = System.Drawing.Color.Pink;
            }
            if (Tasks.IPM.AdditionalDate == null)
            {
                this.labelDateAdditional.Text = "";
                this.labelDateAdditional.BackColor = System.Drawing.Color.Transparent;
            }
            else
            {
                System.DateTime date = (DateTime)Tasks.IPM.AdditionalDate;
                this.labelDateAdditional.Text = "+ " + date.ToString("yyyy-MM-dd");
                this.labelDateAdditional.BackColor = System.Drawing.Color.LightGreen;
            }
            this.initGridViewDate();
        }

        private void labelDatesEnd_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetDate f = new DiversityWorkbench.Forms.FormGetDate(false, false, false, null, Tasks.IPM.End);
            f.SetTitle("Select end date");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                Tasks.IPM.End = f.Date;
                this.setDateRangeControls();
            }
        }

        private void labelDatesStart_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetDate f = new DiversityWorkbench.Forms.FormGetDate(false, false, false, Tasks.IPM.Start);
            f.SetTitle("Select start date");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                Tasks.IPM.Start = f.Date;
                this.setDateRangeControls();
            }
        }

        private void labelDateAdditional_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetDate f = new DiversityWorkbench.Forms.FormGetDate(false, false, false, Tasks.IPM.AdditionalDate);
            f.SetTitle("Select additional date");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                Tasks.IPM.AdditionalDate = f.Date;
                this.setDateRangeControls();
            }
        }

        #endregion

        #region Details

        private void setDetailStatus(int? ID)
        {
            this.buttonDetails.Enabled = false;
            this.buttonRemoveTrap.Enabled = false;
            this.buttonImportMetric.Enabled = false;
            this.buttonDetails.Tag = ID;
            if (ID != null)
            {
                this.buttonDetails.Enabled = true;
                try
                {
                    System.Data.DataTable dt = new DataTable();
                    string SQL = "SELECT CASE WHEN C.TaskEnd < GetDate() THEN 1 ELSE 0 END AS TrapEnded, CASE WHEN C.BoolValue = 1 THEN 1 ELSE 0 END AS Trapped, T.[Type] FROM CollectionTask C INNER JOIN Task T ON C.TaskID = T.TaskID WHERE C.CollectionTaskID = " + ID.ToString();
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                    int True = 0;
                    if (int.TryParse(dt.Rows[0][1].ToString(), out True) && True == 1 && dt.Rows[0][2].ToString().ToLower() == "trap")
                    {
                        this.buttonRemoveTrap.Enabled = true;
                    }
                    else if (dt.Rows[0][2].ToString().ToLower() == "trap")
                    {
                        this.buttonRemoveTrap.Enabled = true;
                    }
                    else if (dt.Rows[0][2].ToString().ToLower() == "sensor")
                    {
                        this.buttonImportMetric.Tag = (int)ID;
                        this.buttonImportMetric.Enabled = true;
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void buttonDetails_Click(object sender, EventArgs e)
        {
            // try to find the current CollectionTaskID
            int CollectionTaskID;
            if (this.buttonDetails.Tag != null)
            {
                if (int.TryParse(this.buttonDetails.Tag.ToString(), out CollectionTaskID))
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    DiversityCollection.Tasks.FormCollectionTask f = new FormCollectionTask(CollectionTaskID, this._iMainForm);
                    f.Width = this.Width - 50;
                    f.Height = this.Height - 50;
                    f.StartPosition = FormStartPosition.CenterParent;
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    f.ShowDialog();
                    this.ReadPestValues(this.dataGridViewResults);
                }
            }
        }

        private void buttonShowDetails_Click(object sender, EventArgs e)
        {
            switch (this.splitContainerChart.Panel2Collapsed)
            {
                case true:
                    this.buttonShowDetails.Image = DiversityCollection.Resource.ArrowDown;
                    this.buttonShowDetails.BackColor = System.Drawing.Color.White;
                    this.splitContainerChart.Panel2Collapsed = false;
                    break;
                default:
                    this.buttonShowDetails.Image = DiversityCollection.Resource.ArrowUp;
                    this.buttonShowDetails.BackColor = System.Drawing.Color.Yellow;
                    this.splitContainerChart.Panel2Collapsed = true;
                    break;
            }
        }

        #region Report

        private void toolStripButtonReportOpenSchemaFile_Click(object sender, EventArgs e)
        {
            string Path = Folder.Report(Folder.ReportFolder.TaskSchemaCollection);
            if (this.textBoxReportSchemaFile.Text.Length > 0)
            {
                try
                {
                    System.IO.FileInfo FI = new System.IO.FileInfo(this.textBoxReportSchemaFile.Text);
                    if (FI.Exists)
                        Path = FI.DirectoryName;
                }
                catch { }
            }
            this.openFileDialogReportSchema = new OpenFileDialog();
            this.openFileDialogReportSchema.RestoreDirectory = true;
            this.openFileDialogReportSchema.Multiselect = false;
            this.openFileDialogReportSchema.InitialDirectory = Path;
            this.openFileDialogReportSchema.Filter = "XSLT Files|*.xslt";
            try
            {
                this.openFileDialogReportSchema.ShowDialog();
                if (this.openFileDialogReportSchema.FileName.Length > 0)
                {
                    this.textBoxReportSchemaFile.Tag = this.openFileDialogReportSchema.FileName;
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialogReportSchema.FileName);
                    this.textBoxReportSchemaFile.Text = f.FullName;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonReportCreate_Click(object sender, EventArgs e)
        {
            System.IO.FileInfo XmlFile = new System.IO.FileInfo(Folder.Report() + "Collection.XML");
            DiversityCollection.XmlExport xmlExport = new XmlExport(this.textBoxReportSchemaFile.Text, XmlFile.FullName);
            DiversityCollection.XmlExport.CollectionTaskSourceForChart ChartSource = XmlExport.CollectionTaskSourceForChart.None;
            if (this.radioButtonReportChartLocation.Checked)
                ChartSource = XmlExport.CollectionTaskSourceForChart.Location;
            else if (this.radioButtonReportChartRoom.Checked)
                ChartSource = XmlExport.CollectionTaskSourceForChart.Room;
            string File = xmlExport.CreateXmlForCollectionTask(Tasks.Settings.Default.TopCollectionID, true, "", XmlExport.QRcodeSourceCollectionTask.None, 1, ChartSource, 
                this.checkBoxReportPlan.Checked, this._ChartHeight, this._ChartWidth);
            if (File.Length > 0)
            {
                try
                {
                    System.Uri URI = new Uri(File);
                    this.webBrowserReport.Url = URI;
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void toolStripButtonReportPrint_Click(object sender, EventArgs e)
        {
            this.webBrowserReport.ShowPrintPreviewDialog();
        }


        private int _ChartHeight = 400;
        private void labelReportChartHeight_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetInteger f = new DiversityWorkbench.Forms.FormGetInteger(this._ChartHeight, "Please set the height for the charts in the report", "Chart height");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK && f.Integer != null && f.Integer > 0)
            {
                this._ChartHeight = (int)f.Integer;
                this.labelReportChartHeight.Text = "      " + this._ChartHeight.ToString();
            }
        }

        private int _ChartWidth = 800;
        private void labelReportChartWidth_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetInteger f = new DiversityWorkbench.Forms.FormGetInteger(this._ChartWidth, "Please set the width for the charts in the report", "Chart width");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK && f.Integer != null && f.Integer > 0)
            {
                this._ChartWidth = (int)f.Integer;
                this.labelReportChartWidth.Text = "      " + this._ChartWidth.ToString();
            }
        }

        private void radioButtonReportChartNone_CheckedChanged(object sender, EventArgs e)
        {
            this.labelReportChartHeight.Visible = !radioButtonReportChartNone.Checked;
            this.labelReportChartWidth.Visible = !radioButtonReportChartNone.Checked;
        }

#endregion

        #region Cleaning

        System.Data.DataTable _dtCleaning;

        private void toolStripButtonCleaningAdd_Click(object sender, EventArgs e)
        {
            this.Action(Tasks.IPM.Treatment.Cleaning, ActionType.Add, this.dataGridViewCleaning);
        }

        private void toolStripButtonCleaningRemove_Click(object sender, EventArgs e)
        {
            this.Action(Tasks.IPM.Treatment.Cleaning, ActionType.Remove, this.dataGridViewCleaning);
        }

        private void toolStripButtonCleaningDetails_Click(object sender, EventArgs e)
        {
            this.Action(Tasks.IPM.Treatment.Cleaning, ActionType.Details, this.dataGridViewCleaning);
        }

        private void toolStripButtonCleaningHide_Click(object sender, EventArgs e)
        {
            this.Action(Tasks.IPM.Treatment.Cleaning, ActionType.Hide, this.dataGridViewCleaning);
        }

        private void toolStripButtonCleaningVisible_Click(object sender, EventArgs e)
        {
            this.Action(Tasks.IPM.Treatment.Cleaning, ActionType.Show, this.dataGridViewCleaning);
        }

        private enum CleaningAction { Hide, Show, Add, Remove, Details }
        private void CleaningSetAction(CleaningAction action)
        {
            try
            {
                if (this.dataGridViewCleaning.SelectedRows.Count == 1)
                {
                    System.Data.DataRowView r = (System.Data.DataRowView)this.dataGridViewCleaning.SelectedRows[0].DataBoundItem;
                    string Message = "";
                    string Caption = "";
                    string SQL = "";
                    switch(action)
                    {
                        case CleaningAction.Add:
                            if (this._CurrentCollectionID == -1)
                            {
                                System.Windows.Forms.MessageBox.Show("Please select a collection from the hierarchy");
                                return;
                            }
                            SQL = "SELECT T.HierarchyDisplayText + ' - ' + R.Result AS Cleaning, T.TaskID, R.Result " +
                                "FROM [dbo].[TaskHierarchyAll] () T INNER JOIN [dbo].[TaskResult] R ON T.TaskID = R.TaskID AND T.Type = 'Cleaning'";
                            System.Data.DataTable dt = new DataTable();
                            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                            if (dt.Rows.Count > 0)
                            {
                                DiversityWorkbench.Forms.FormGetStringFromList fClean = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Cleaning", "TaskID", "Cleaning", "Please select a cleaning from the list");
                                fClean.ShowDialog();
                                if (fClean.DialogResult == DialogResult.OK)
                                {
                                    System.Data.DataRow R = fClean.SelectedRow;
                                    DiversityWorkbench.Forms.FormGetDate f = new DiversityWorkbench.Forms.FormGetDate(false);
                                    f.SetTitle("Please select a date for the cleaning");
                                    f.ShowDialog();
                                    if (f.DialogResult == DialogResult.OK)
                                    {
                                        SQL = "INSERT INTO CollectionTask (CollectionID, TaskID, DisplayOrder, Result, TaskStart ) " +
                                            "VALUES(" + this._CurrentCollectionID.ToString() + ", " + R["TaskID"].ToString() + ", 1, N'" + R["Result"].ToString() + "', CONVERT(DATETIME, '" + f.Date.ToString("yyyy-MM-dd") + " 00:00:00', 102)) ";
                                        if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                                            this.initCleaning();
                                    }
                                }
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("No cleaning defined so far. Please turn to your administrator");
                            }
                            break;
                        case CleaningAction.Details:
                            int ID;
                            if (int.TryParse(r["CollectionTaskID"].ToString(), out ID))
                            {
                                DiversityCollection.Tasks.FormCollectionTask f = new FormCollectionTask(ID, this._iMainForm);
                                f.ShowDialog();
                            }
                            break;
                        case CleaningAction.Remove:
                            Message = "Do you want to delete the selected cleaning?";
                            Caption = "Delete";
                            SQL = "DELETE C FROM CollectionTask C WHERE C.CollectionTaskID = " + r["CollectionTaskID"].ToString();
                            break;
                        case CleaningAction.Hide:
                            Message = "Do you want to hide the selected cleaning from the chart?";
                            Caption = "Hide";
                            SQL = "UPDATE C SET C.DisplayOrder = 0 FROM CollectionTask C WHERE C.CollectionTaskID = " + r["CollectionTaskID"].ToString();
                            break;
                        case CleaningAction.Show:
                            Message = "Do you want to show the selected cleaning in the chart?";
                            Caption = "Show";
                            SQL = "UPDATE C SET C.DisplayOrder = 1 FROM CollectionTask C WHERE C.CollectionTaskID = " + r["CollectionTaskID"].ToString();
                            break;
                    }
                    switch(action)
                    {
                        case CleaningAction.Hide:
                        case CleaningAction.Show:
                        case CleaningAction.Remove:
                            if (System.Windows.Forms.MessageBox.Show(Message, Caption, MessageBoxButtons.OKCancel) == DialogResult.OK)
                            {
                                Message = "";
                                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message))
                                    this.initCleaning();
                                else
                                    System.Windows.Forms.MessageBox.Show(Message);
                            }
                            break;
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Please select a cleaning");
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private bool initCleaning(bool Enable = true)
        {
            return this.initTarget(Tasks.IPM.Treatment.Cleaning, this._dtCleaning, this.dataGridViewCleaning, this.textBoxCleaningCollection, Enable);
        }

        #endregion

        #region Beneficials

        System.Data.DataTable _dtBeneficial;

        private void toolStripButtonBeneficialAdd_Click(object sender, EventArgs e)
        {
            this.Action(Tasks.IPM.Treatment.Beneficial, ActionType.Add, this.dataGridViewBeneficial);
        }

        private void toolStripButtonBeneficialRemove_Click(object sender, EventArgs e)
        {
            this.Action(Tasks.IPM.Treatment.Beneficial, ActionType.Remove, this.dataGridViewBeneficial);
        }

        private void toolStripButtonBeneficialDetails_Click(object sender, EventArgs e)
        {
            this.Action(Tasks.IPM.Treatment.Beneficial, ActionType.Details, this.dataGridViewBeneficial);
        }

        private void toolStripButtonBeneficialHide_Click(object sender, EventArgs e)
        {
            this.Action(Tasks.IPM.Treatment.Beneficial, ActionType.Hide, this.dataGridViewBeneficial);
        }

        private void toolStripButtonBeneficialShow_Click(object sender, EventArgs e)
        {
            this.Action(Tasks.IPM.Treatment.Beneficial, ActionType.Show, this.dataGridViewBeneficial);
        }

        private bool initBeneficials(bool Enable = true)
        {
            return this.initTarget(Tasks.IPM.Treatment.Beneficial, this._dtBeneficial, this.dataGridViewBeneficial, this.textBoxBeneficialCollection, Enable);
        }

        #endregion

        #region Treatment

        System.Data.DataTable _dtTreatment;

        private void toolStripButtonTreatmentAdd_Click(object sender, EventArgs e)
        {
//#if !DEBUG
//            System.Windows.Forms.MessageBox.Show("Available in upcoming version", "Coming soon", MessageBoxButtons.OK, MessageBoxIcon.Information);
//            return;
//#endif
            Tasks.IPM.Treatment treatment = this._TreatmentType;
            this.Action(this._TreatmentType, ActionType.Add, this.dataGridViewTreatment);
        }

        private void toolStripButtonTreatmentDelete_Click(object sender, EventArgs e)
        {
//#if !DEBUG
//            System.Windows.Forms.MessageBox.Show("Available in upcoming version", "Coming soon", MessageBoxButtons.OK, MessageBoxIcon.Information);
//            return;
//#endif
            this.Action(this._TreatmentType, ActionType.Remove, this.dataGridViewTreatment);
        }

        private void toolStripButtonTreatmentDetails_Click(object sender, EventArgs e)
        {
//#if !DEBUG
//            System.Windows.Forms.MessageBox.Show("Available in upcoming version", "Coming soon", MessageBoxButtons.OK, MessageBoxIcon.Information);
//            return;
//#endif
            this.Action(this._TreatmentType, ActionType.Details, this.dataGridViewTreatment);
        }

        private void toolStripButtonTreatmentHide_Click(object sender, EventArgs e)
        {
//#if !DEBUG
//            System.Windows.Forms.MessageBox.Show("Available in upcoming version", "Coming soon", MessageBoxButtons.OK, MessageBoxIcon.Information);
//            return;
//#endif
            this.Action(this._TreatmentType, ActionType.Hide, this.dataGridViewTreatment);
        }

        private void toolStripButtonTreatmentShow_Click(object sender, EventArgs e)
        {
//#if !DEBUG
//            System.Windows.Forms.MessageBox.Show("Available in upcoming version", "Coming soon", MessageBoxButtons.OK, MessageBoxIcon.Information);
//            return;
//#endif
            this.Action(this._TreatmentType, ActionType.Show, this.dataGridViewTreatment);
        }

        private bool initTreatments(bool Enable = true)
        {
            return this.initTarget(this._TreatmentType, this._dtTreatment, this.dataGridViewTreatment, this.textBoxTreatmentLocation, Enable);
        }

        #region Treatment type

        //private enum TreatmentType { Damage, Repair, Gas, Poision, Freezing }
        private IPM.Treatment _TreatmentType = IPM.Treatment.Repair;
        private void toolStripMenuItemRepair_Click(object sender, EventArgs e)
        {
            this.setTreatmentType(IPM.Treatment.Repair, this.toolStripMenuItemRepair);
        }

        private void damageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.setTreatmentType(IPM.Treatment.Damage, this.damageToolStripMenuItem);
        }

        private void toolStripMenuItemFreezing_Click(object sender, EventArgs e)
        {
            this.setTreatmentType(IPM.Treatment.Freezing, this.toolStripMenuItemFreezing);
        }

        private void toolStripMenuItemGas_Click(object sender, EventArgs e)
        {
            this.setTreatmentType(IPM.Treatment.Gas, this.toolStripMenuItemGas);
        }

        private void poisonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.setTreatmentType(IPM.Treatment.Poison, this.poisonToolStripMenuItem);
        }

        private void setTreatmentType(IPM.Treatment treatmentType, ToolStripMenuItem toolStripMenuItem)
        {
            this._TreatmentType = treatmentType;
            toolStripDropDownButtonTreatmentType.Image = toolStripMenuItem.Image;
        }

        #endregion

#endregion

        #region Common

        //private enum ActionTarget { Cleaning, Beneficial }
        private enum ActionType { Hide, Show, Add, Remove, Details }

        private void Action(Tasks.IPM.Treatment target, ActionType action, System.Windows.Forms.DataGridView dataGridView)
        {
            try
            {
                if (action == ActionType.Add)
                {
                    string SQL = "";
                    if (this._CurrentCollectionID == -1)
                    {
                        System.Windows.Forms.MessageBox.Show("Please select a collection from the hierarchy");
                        return;
                    }
                    string CollectionType = DiversityCollection.LookupTable.CollectionType(this._CurrentCollectionID);
                    if (CollectionType == "trap" ||
                        !IPM.CollectionTypeContainingPests.Contains(CollectionType))
                    {
                        System.Windows.Forms.MessageBox.Show("Please select a valid collection for a " + target.ToString().ToLower());
                        return;
                    }
                    switch (target)
                    {
                        case Tasks.IPM.Treatment.Beneficial:
                            SQL = "SELECT T.HierarchyDisplayText + ' - ' + R.Result AS " + target.ToString() + ", T.TaskID, R.Result " +
                                "FROM [dbo].[TaskHierarchyAll] () T INNER JOIN [dbo].[TaskResult] R ON T.TaskID = R.TaskID AND T.Type = 'Beneficial organism'";
                            break;
                        case Tasks.IPM.Treatment.Cleaning:
                            SQL = "SELECT T.HierarchyDisplayText + ' - ' + R.Result AS " + target.ToString() + ", T.TaskID, R.Result " +
                                "FROM [dbo].[TaskHierarchyAll] () T INNER JOIN [dbo].[TaskResult] R ON T.TaskID = R.TaskID AND T.Type = 'Cleaning'";
                            break;
                        default:
                            SQL = "SELECT T.HierarchyDisplayText + ' - ' + R.Result AS " + target.ToString() + ", T.TaskID, R.Result " +
                                "FROM [dbo].[TaskHierarchyAll] () T INNER JOIN [dbo].[TaskResult] R ON T.TaskID = R.TaskID AND T.Type = '" + this._TreatmentType.ToString() + "'";
                            break;
                    }
                    System.Data.DataTable dt = new DataTable();
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);

                    // das wird erst aktiviert, wenn klar ist wie cleaning und beneficials wirklich verwendet werden. Evtl. muss da noch was umgebaut werden. DAnach kann eine automatisierte Erzeugung erfolgen
#if DEBUG
                    if (dt.Rows.Count == 0)
                    {
                        dt = Tasks.IPM.Treatments(target);
                        if (dt.Rows.Count == 0)
                            return;
                    }
#endif

                    if (dt.Rows.Count > 0)
                    {
                        DiversityWorkbench.Forms.FormGetStringFromList fTarget = new DiversityWorkbench.Forms.FormGetStringFromList(dt, target.ToString(), "TaskID", target.ToString(), "Please select a " + target.ToString().ToLower() + " from the list");
                        fTarget.ShowDialog();
                        if (fTarget.DialogResult == DialogResult.OK)
                        {
                            System.Data.DataRow R = fTarget.SelectedRow;
                            DiversityWorkbench.Forms.FormGetDate f = new DiversityWorkbench.Forms.FormGetDate(false, true, true);
                            f.SetTitle("Please select a date for the application of the " + target.ToString().ToLower() + "");
                            f.ShowDialog();
                            if (f.DialogResult == DialogResult.OK)
                            {
                                switch(target)
                                {
                                    case Tasks.IPM.Treatment.Beneficial:
                                        int Number = 1;
                                        DiversityWorkbench.Forms.FormGetInteger fCount = new DiversityWorkbench.Forms.FormGetInteger(Number, "Count", "Please enter the number of applied units");
                                        fCount.ShowDialog();
                                        if (fCount.Integer != null)
                                            Number = (int)fCount.Integer;
                                        SQL = "INSERT INTO CollectionTask (CollectionID, TaskID, DisplayOrder, Result, NumberValue, TaskStart ";
                                        if (f.DateEndSelected && f.EndDate != null)
                                            SQL += ", TaskEnd";
                                        SQL += ") VALUES(" + this._CurrentCollectionID.ToString() + ", " + R["TaskID"].ToString() + ", 1, N'" + R["Result"].ToString() + "', " + Number.ToString() + ", CONVERT(DATETIME, '" + f.Date.ToString("yyyy-MM-dd") + " 00:00:00', 102)";
                                        if (f.DateEndSelected && f.EndDate != null)
                                            SQL += ", CONVERT(DATETIME, '" + f.EndDate.ToString("yyyy-MM-dd") + " 00:00:00', 102)";
                                        SQL += ") ";
                                        break;
                                    case Tasks.IPM.Treatment.Cleaning:
                                        SQL = "INSERT INTO CollectionTask (CollectionID, TaskID, DisplayOrder, Result, TaskStart ";
                                        if (f.DateEndSelected && f.EndDate != null)
                                            SQL += ", TaskEnd";
                                        SQL += ") VALUES(" + this._CurrentCollectionID.ToString() + ", " + R["TaskID"].ToString() + ", 1, N'" + R["Result"].ToString() + "', CONVERT(DATETIME, '" + f.Date.ToString("yyyy-MM-dd") + " 00:00:00', 102)";
                                        if (f.DateEndSelected && f.EndDate != null)
                                            SQL += ", CONVERT(DATETIME, '" + f.EndDate.ToString("yyyy-MM-dd") + " 00:00:00', 102)";
                                        SQL += ") ";
                                        break;
                                    default:
                                        SQL = "INSERT INTO CollectionTask (CollectionID, TaskID, DisplayOrder, Result, TaskStart ";
                                        if (f.DateEndSelected && f.EndDate != null)
                                            SQL += ", TaskEnd";
                                        SQL += ") VALUES(" + this._CurrentCollectionID.ToString() + ", " + R["TaskID"].ToString() + ", 1, N'" + R["Result"].ToString() + "', CONVERT(DATETIME, '" + f.Date.ToString("yyyy-MM-dd") + " 00:00:00', 102)";
                                        if (f.DateEndSelected && f.EndDate != null)
                                            SQL += ", CONVERT(DATETIME, '" + f.EndDate.ToString("yyyy-MM-dd") + " 00:00:00', 102)";
                                        SQL += ") ";
                                        break;
                                }
                                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                                {
                                    switch (target)
                                    {
                                        case Tasks.IPM.Treatment.Beneficial:
                                            this.initTarget(target, this._dtBeneficial, this.dataGridViewBeneficial, this.textBoxBeneficialCollection);
                                            break;
                                        case Tasks.IPM.Treatment.Cleaning:
                                            this.initTarget(target, this._dtCleaning, this.dataGridViewCleaning, this.textBoxCleaningCollection);
                                            break;
                                        default:
                                            this.initTarget(target, this._dtTreatment, this.dataGridViewTreatment, this.textBoxTreatmentLocation);
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("No " + target.ToString().ToLower() + " defined so far. Please turn to your administrator");
                    }
                }
                else
                {
                    if (dataGridView.SelectedRows.Count == 1)
                    {
                        System.Data.DataRowView r = (System.Data.DataRowView)dataGridView.SelectedRows[0].DataBoundItem;
                        string Message = "";
                        string Caption = "";
                        string SQL = "";
                        switch (action)
                        {
                            case ActionType.Add:
                                break;
                            case ActionType.Details:
                                int ID;
                                if (int.TryParse(r["CollectionTaskID"].ToString(), out ID))
                                {
                                    DiversityCollection.Tasks.FormCollectionTask f = new FormCollectionTask(ID, this._iMainForm, false);
                                    f.Width = this.Width - 20;
                                    if (f.Height > this.Height)
                                        f.Height = this.Height - 20;
                                    f.ShowDialog();
                                }
                                break;
                            case ActionType.Remove:
                                Message = "Do you want to delete the selected " + target.ToString().ToLower() + " ?";
                                Caption = "Delete";
                                SQL = "DELETE C FROM CollectionTask C WHERE C.CollectionTaskID = " + r["CollectionTaskID"].ToString();
                                break;
                            case ActionType.Hide:
                                Message = "Do you want to hide the selected " + target.ToString().ToLower() + " from the chart?";
                                Caption = "Hide";
                                SQL = "UPDATE C SET C.DisplayOrder = 0 FROM CollectionTask C WHERE C.CollectionTaskID = " + r["CollectionTaskID"].ToString();
                                break;
                            case ActionType.Show:
                                Message = "Do you want to show the selected " + target.ToString().ToLower() + " in the chart?";
                                Caption = "Show";
                                SQL = "UPDATE C SET C.DisplayOrder = 1 FROM CollectionTask C WHERE C.CollectionTaskID = " + r["CollectionTaskID"].ToString();
                                break;
                        }
                        switch (action)
                        {
                            case ActionType.Hide:
                            case ActionType.Show:
                            case ActionType.Remove:
                                if (System.Windows.Forms.MessageBox.Show(Message, Caption, MessageBoxButtons.OKCancel) == DialogResult.OK)
                                {
                                    Message = "";
                                    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message))
                                        switch (target)
                                        {
                                            case Tasks.IPM.Treatment.Beneficial:
                                                this.initTarget(target, this._dtBeneficial, this.dataGridViewBeneficial, this.textBoxBeneficialCollection);
                                                break;
                                            case Tasks.IPM.Treatment.Cleaning:
                                                this.initTarget(target, this._dtCleaning, this.dataGridViewCleaning, this.textBoxCleaningCollection);
                                                break;
                                            default:
                                                this.initTarget(target, this._dtTreatment, this.dataGridViewTreatment, this.textBoxTreatmentLocation);
                                                break;
                                        }
                                    else
                                        System.Windows.Forms.MessageBox.Show(Message);
                                }
                                break;
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Please select a " + target.ToString().ToLower());
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private bool initTarget(Tasks.IPM.Treatment target, System.Data.DataTable dataTableTarget, System.Windows.Forms.DataGridView dataGridView, System.Windows.Forms.TextBox textBox, bool Enable = true)
        {
            bool OK = true;
            try
            {
                if (!Enable)
                {
                    dataGridView.DataSource = null;
                    textBox.Text = "";
                    return Enable;
                }
                if (_CurrentCollectionID > -1)
                {
                    // setting the header text
                    string SQL = "SELECT C.DisplayText FROM [dbo].[CollectionHierarchyAll] () C WHERE C.CollectionID = " + this._CurrentCollectionID.ToString();
                    textBox.Text = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);

                    // setting the data
                    if (dataTableTarget != null)
                        dataTableTarget = null;
                    dataTableTarget = new DataTable();
                    SQL = "SELECT TOP 1 T.DateBeginType, T.ResultType, ";
                    switch (target)
                    {
                        case Tasks.IPM.Treatment.Beneficial:
                            SQL += "T.Type AS DisplayText, T.NumberType";
                            break;
                        case Tasks.IPM.Treatment.Cleaning:
                            SQL += "T.DisplayText";
                            break;
                        default:
                            SQL += "T.DisplayText";
                            break;
                    }
                    SQL += " FROM Task T INNER JOIN  CollectionTask AS C ON T.TaskID = C.TaskID  AND C.CollectionID = " + this._CurrentCollectionID.ToString();
                    switch (target)
                    {
                        case Tasks.IPM.Treatment.Beneficial:
                            SQL += " AND T.Type = 'Beneficial organism' ";
                            break;
                        case Tasks.IPM.Treatment.Cleaning:
                            SQL += " AND T.Type = 'Cleaning' ";
                            break;
                        default:
                            SQL += " AND T.Type IN ('Damage', 'Freezing', 'Gas', 'Poison', 'Repair') ";
                            break;
                    }
                    System.Data.DataTable dt = new DataTable();
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                    if (dt.Rows.Count == 1)
                    {
                        System.Data.DataRow R = dt.Rows[0];
                        SQL = "SELECT CAST(C.DisplayOrder AS BIT) AS [Show in chart], CONVERT(varchar(10), C.TaskStart, 120) AS [" + R[0].ToString() + "],  ";
                        switch (target)
                        {
                            case Tasks.IPM.Treatment.Cleaning:
                                SQL += " C.Result AS [" + R[1].ToString() + "] , T.HierarchyDisplayText AS [" + R[2].ToString() + "] , C.CollectionTaskID";
                                break;
                            case Tasks.IPM.Treatment.Beneficial:
                                SQL += " T.DisplayText AS [" + R[2].ToString() + "] , C.Result AS [" + R[1].ToString() + "] , C.CollectionTaskID, NumberValue AS [" + R[3].ToString() + "] ";
                                break;
                            default:
                                SQL += " C.Result AS [" + R[1].ToString() + "] , T.HierarchyDisplayText AS [" + R[2].ToString() + "] , C.CollectionTaskID";
                                break;
                        }
                        SQL += " FROM CollectionTask AS C INNER JOIN " +
                            "dbo.TaskHierarchyAll() AS T ON C.TaskID = T.TaskID ";
                        switch (target)
                        {
                            case Tasks.IPM.Treatment.Beneficial:
                                SQL += " AND T.Type = 'Beneficial organism' ";
                                break;
                            case Tasks.IPM.Treatment.Cleaning:
                                SQL += " AND T.Type = 'Cleaning' ";
                                break;
                            default:
                                SQL += " AND T.Type IN ('Damage', 'Freezing', 'Gas', 'Poison', 'Repair') ";
                                break;
                        }
                        SQL += " AND C.CollectionID = " + this._CurrentCollectionID.ToString() +
                            " ORDER BY C.TaskStart ";
                        if (DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTableTarget))
                        {
                            dataGridView.DataSource = dataTableTarget;
                            dataGridView.Columns[4].Visible = false;
                        }
                    }
                    else
                    {
                        dataGridView.DataSource = null;
                    }
                }
                else
                {
                    dataGridView.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        #endregion

        #region Sensors

        #region Button evients
        private void buttonAddSensor_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.TreeNode N = this.treeViewCollection.SelectedNode;
                if (N.Tag != null && N.Tag.GetType() == typeof(System.Data.DataRow))
                {
                    System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                    int CollectionID;
                    if (int.TryParse(R["CollectionID"].ToString(), out CollectionID))
                    {
                        int SensorID = 0;
                        string Error = "";
                        if (this.AddSensor(CollectionID, ref SensorID, ref Error))
                        {
                            this.initTreeViewCollection();
                        }
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void buttonAddSensorMetric_Click(object sender, EventArgs e)
        {

        }

        #endregion

        private bool AddSensor(int CollectionID, ref int SensorID, ref string Error)
        {
            bool OK = false;
            try
            {
                if (Tasks.IPM.AddSensor(CollectionID, ref SensorID, ref Error))
                {
                    Tasks.FormPrometheus f = new FormPrometheus(FormPrometheus.State.GetSensor);
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        if (f.getSensorMetric() != null)
                        {
                            //CollectionTask collectionTask = new CollectionTask();
                            string Message = "";
                            string Sensor = "";
                            int i = 0;
                            System.Collections.Generic.SortedDictionary<string, PrometheusMetric> dict = f.getSensorMetric();
                            foreach (System.Collections.Generic.KeyValuePair<string, PrometheusMetric> KV in dict)
                            {
                                if (Sensor.Length == 0) Sensor = KV.Value.SensorDisplayText;
                                Message += "\r\n" + KV.Value.MetricDisplayText;
                                i++;
                            }
                            if (i > 0)
                            {
                                Message = "The sensor " + Sensor + " provides " + i.ToString() + " metics:" + Message + "\r\n\r\nInclude all metrics?";
                                if (System.Windows.Forms.MessageBox.Show(Message, "Include metrics?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    foreach (System.Collections.Generic.KeyValuePair<string, PrometheusMetric> KV in dict)
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
                if (Error.Length > 0)
                {
                    OK = false;
                    System.Windows.Forms.MessageBox.Show(Error);
                }
                return OK;

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }


        #endregion

        #region Images
        private void setImages(int ID, bool ForCollection = true)
        {
            try
            {
                this.DisposeImageControls();
                this.tabControlImages.TabPages.Clear();

                string SQL = "SELECT T.CollectionTaskID, CASE WHEN C.CollectionAcronym IS NULL THEN C.CollectionName ELSE C.CollectionAcronym END +' ' + nchar(8212) + ' ' + CASE WHEN T.DisplayText IS NULL THEN '' ELSE T.DisplayText END AS DisplayText " +
                    "FROM CollectionTaskImage AS I " +
                    "RIGHT OUTER JOIN CollectionTask AS T ON I.CollectionTaskID = T.CollectionTaskID " +
                    "INNER JOIN Collection AS C ON T.CollectionID = C.CollectionID  ";
                if (ForCollection)
                    SQL += " AND T.CollectionID IN (" + Tasks.IPM.CollectionIDList(ID) + ")  ";
                else
                    SQL += " AND T.CollectionTaskID = " + ID.ToString();
                SQL += " INNER JOIN Task A ON A.TaskID = T.TaskID AND A.Type = 'Trap' " +
                    "GROUP BY T.CollectionTaskID, CASE WHEN C.CollectionAcronym IS NULL THEN C.CollectionName ELSE C.CollectionAcronym END +' ' + nchar(8212) + ' ' + CASE WHEN T.DisplayText IS NULL THEN '' ELSE T.DisplayText END " +
                    "ORDER BY CASE WHEN C.CollectionAcronym IS NULL THEN C.CollectionName ELSE C.CollectionAcronym END +' ' + nchar(8212) + ' ' + CASE WHEN T.DisplayText IS NULL THEN '' ELSE T.DisplayText END";
                System.Data.DataTable dt = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    System.Windows.Forms.TabPage tab = new TabPage(R[1].ToString());
                    tab.Tag = R[0];
                    Tasks.UserControlImages u = new UserControlImages();
                    u.CollectionTaskID = int.Parse(R[0].ToString());
                    tab.Controls.Add(u);
                    u.Dock = DockStyle.Fill;
                    this.tabControlImages.TabPages.Add(tab);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void DisposeImageControls()
        {
            try
            {
                for (int i = this.tabControlImages.TabPages.Count - 1; i >= 0; --i)// System.Windows.Forms.TabPage tab in this.tabControlImages.TabPages)
                {
                    for (int ix = this.tabControlImages.TabPages[i].Controls.Count - 1; ix >= 0; --ix)
                        this.tabControlImages.TabPages[i].Controls[ix].Dispose();
                    this.tabControlImages.TabPages[i].Dispose();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }



        #endregion

#endregion

        #region Menu

        #region Settings

        private void showPestsOusideTrapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Default.IncludeCollections = !Settings.Default.IncludeCollections;
            Settings.Default.Save();
            this.showPestsOusideTrapsToolStripMenuItem.Checked = Settings.Default.IncludeCollections;
            this.initGridViewCollections(this.DateSQL, true);
        }

        private void showPestOnSpecimenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Default.IncludeSpecimen = !Settings.Default.IncludeSpecimen;
            Settings.Default.Save();
            this.showPestOnSpecimenToolStripMenuItem.Checked = Settings.Default.IncludeSpecimen;
            this.initGridViewSpecimens(true);
        }

        private void showPestsOnGroupsOfSpecimenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Default.IncludeTransactions = !Settings.Default.IncludeTransactions;
            Settings.Default.Save();
            this.showPestsOnGroupsOfSpecimenToolStripMenuItem.Checked = Settings.Default.IncludeTransactions;
            this.initGridViewTransaction(true);
        }
        #endregion

        #region Collection

        private void changeToTopCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.setParentCollection();
        }

        private void addCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddCollection();
        }

        #endregion

        #region Taxa

        private void pestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.InitGridView_ValueColumns(IPM.RecordingTarget.TrapPest);
            this.setSourceSettingsForTraps(Tasks.IPM.RecordingTarget.TrapPest, this.dataGridViewResults);

            this.initGridViewPests(true);

        }

        private void bycatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.setSourceSettingsForTraps(Tasks.IPM.RecordingTarget.TrapBycatch, this.dataGridViewBycatch);
            this.initGridViewBycatch(true);
            this.splitContainerPestAndBycatch.Panel2Collapsed = Settings.Default.BycatchNameUris.Count == 0;
        }

        private void beneficialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.setSourceSettingsForTraps(Tasks.IPM.RecordingTarget.Beneficial, this.dataGridViewBeneficial);

            //try
            //{
            //    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            //    System.Collections.Generic.List<IPM.RecordingTarget> recordingTargets = new List<IPM.RecordingTarget>();
            //    recordingTargets.Add(RecordingTarget.Beneficial);
            //    FormSettings f = new FormSettings(recordingTargets);
            //    this.Cursor = System.Windows.Forms.Cursors.Default;
            //    f.ShowDialog();
            //    if (f.DialogResult == DialogResult.OK)
            //    {
            //        this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            //        this.SuspendLayout();
            //        this.InitGridView_ValueColumns(IPM.RecordingTarget.TrapPest);
            //        this.ResumeLayout();
            //        this.Cursor = System.Windows.Forms.Cursors.Default;
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //}
        }

        #endregion

        #endregion


        #region Help
        
        private string _baseUrl = global::DiversityCollection.Properties.Settings.Default.DiversityCollectionManualUrl;
        private void FormIPM_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Help.ShowHelp(this, _baseUrl + "/editing_dc/tasks_dc/ipm_dc/index.html");
        }

        private void tableLayoutPanelTreeCollection_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Help.ShowHelp(this, _baseUrl + "/editing_dc/tasks_dc/ipm_dc/ipm_collections_dc/index.html");
        }

        private void dataGridViewDate_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Help.ShowHelp(this, _baseUrl + "/editing_dc/tasks_dc/ipm_dc/ipm_dates_dc/index.html");
        }

        private void dataGridViewResults_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            // ToDo
        }

        private void dataGridViewBycatch_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Help.ShowHelp(this, _baseUrl + "/editing_dc/tasks_dc/ipm_dc/ipm_pests_dc/ipm_bycatch_dc/index.html");
        }

        private void userControlChart_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Help.ShowHelp(this, _baseUrl + "/editing_dc/tasks_dc/ipm_dc/ipm_chart_dc/index.html");
        }

        private void userControlPlan_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Help.ShowHelp(this, _baseUrl + "/editing_dc/tasks_dc/ipm_dc/ipm_plan_dc/index.html");
        }

        private void tableLayoutPanelReport_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Help.ShowHelp(this, _baseUrl + "/editing_dc/tasks_dc/ipm_dc/ipm_report_dc/index.html");
        }

        private void tableLayoutPanelCleaning_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Help.ShowHelp(this, _baseUrl + "/editing_dc/tasks_dc/ipm_dc/ipm_cleaning_dc/index.html");
        }

        private void tableLayoutPanelBeneficial_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Help.ShowHelp(this, _baseUrl + "/editing_dc/tasks_dc/ipm_dc/ipm_beneficials_dc/index.html");
        }

        private void tableLayoutPanelTreatment_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Help.ShowHelp(this, _baseUrl + "/editing_dc/tasks_dc/ipm_dc/ipm_treatment_dc/index.html");
        }

        #endregion

    }
}
