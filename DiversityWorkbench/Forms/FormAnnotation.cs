using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormAnnotation : Form
    {

        #region Parameter

        //private System.Data.DataTable _DtAnnotation;
        private System.Data.DataTable _DtAnnotationType;
        //private System.Data.DataSet _Dataset;
        private string _ReferencedTable;
        private int _ReferencedID;
        private System.Data.DataRow _ReferencedDataRow;
        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        private int _SelectionCount = 0;
        private System.Windows.Forms.BindingSource AnnotationBindingSource;
        //private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapter;
        private string _ReferencedRowDisplayText;
        private DiversityWorkbench.AnnotationProvider _AnnotationProvider;

        #endregion

        #region Construction

        private FormAnnotation()
        {
            InitializeComponent();
            this.FormFunctions.setDescriptions();
            try
            {
                this.toolStripButtonDelete.Enabled = this.FormFunctions.getObjectPermissions("Annotation", "DELETE");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.textBoxAnnotation.Visible = false;
            this.labelAnnotationType.Visible = false;
            this.labelDataRow.Visible = false;
            this.pictureBoxAnnotationType.Visible = false;
            this.linkLabelURL.Visible = false;
            this.labelReference.Visible = false;
            this.userControlModuleRelatedEntryReference.Visible = false;
            this.buttonSetURL.Visible = false;
            this.labelAnnotationTitle.Visible = false;
            this.comboBoxTitle.Visible = false;
            this.labelSource.Visible = false;
            this.userControlModuleRelatedEntrySource.Visible = false;
            this.labelCreatedBy.Visible = false;
            this.labelCreatedWhen.Visible = false;
            this.textBoxCreatedBy.Visible = false;
            this.textBoxCreatedWhen.Visible = false;
        }

        //public FormAnnotation(
        //    System.Data.DataSet Dataset
        //    , string ReferencedTable
        //    , int ReferencedID
        //    , System.Data.DataRow ReferencedDataRow
        //    , System.Data.DataTable dtAnnotationType
        //    , string HelpNamespace
        //    , string ReferencedRowDisplayText
        //    , System.Drawing.Image Image
        //    , ref Microsoft.Data.SqlClient.SqlDataAdapter DataAdapter)
        //    : this()
        //{
        //    try
        //    {
        //        this.helpProvider.HelpNamespace = HelpNamespace;
        //        this.pictureBoxAnnotationType.Image = Image;
        //        this._ReferencedTable = ReferencedTable;
        //        this._ReferencedID = ReferencedID;
        //        this._ReferencedDataRow = ReferencedDataRow;
        //        this.labelDataRow.Text = "for " + ReferencedRowDisplayText;
        //        this.linkLabelURL.Text = "";
        //        this._Dataset = Dataset;
        //        this._DtAnnotation = this._Dataset.Tables["Annotation"];
        //        this._DtAnnotationType = dtAnnotationType;
        //        this._SqlDataAdapter = DataAdapter;
        //        this._ReferencedRowDisplayText = ReferencedRowDisplayText;

        //        this.Text = "Annotations for " + this._ReferencedRowDisplayText;

        //        this.setDatabindings();

        //        this.labelReferencedTable.Visible = false;
        //        this.textBoxReferencedTable.Visible = false;

        //        if (this._DtAnnotation.Rows.Count > 0)
        //            this.buildTree(this._DtAnnotation.Rows[0]);
        //        if (this.treeViewAnnotation.Nodes.Count == 0)
        //            this.toolStripButtonNewRootAnnotation_Click(null, null);
        //        //this.toolStripButtonNewAnnotation_Click(null, null);
        //    }
        //    catch (System.Exception ex) { }
        //}

        public FormAnnotation(
            DiversityWorkbench.AnnotationProvider AnnotationProvider
            , string ReferencedTable
            , int ReferencedID
            , System.Data.DataRow ReferencedDataRow
            , System.Data.DataTable dtAnnotationType
            , string HelpNamespace
            , string ReferencedRowDisplayText
            , System.Drawing.Image Image)//            , ref Microsoft.Data.SqlClient.SqlDataAdapter DataAdapter)
            : this()
        {
            try
            {
                this.helpProvider.HelpNamespace = HelpNamespace;
                this.pictureBoxAnnotationType.Image = Image;
                this._AnnotationProvider = AnnotationProvider;
                this._ReferencedTable = ReferencedTable;
                this._ReferencedID = ReferencedID;
                this._ReferencedDataRow = ReferencedDataRow;
                this.labelDataRow.Text = "for " + ReferencedRowDisplayText;
                this.linkLabelURL.Text = "";
                //this._DtAnnotation = this._AnnotationProvider.AnnotationTable();// this._Dataset.Tables["Annotation"];
                this._DtAnnotationType = dtAnnotationType;
                //this._SqlDataAdapter = DataAdapter;
                this._ReferencedRowDisplayText = ReferencedRowDisplayText;

                this.Text = "Annotations for " + this._ReferencedRowDisplayText;

                this.setDatabindings();

                this.labelReferencedTable.Visible = false;
                this.textBoxReferencedTable.Visible = false;

                //if (this._DtAnnotation.Rows.Count > 0)
                //    this.buildTree(this._DtAnnotation.Rows[0]);
                if (this._AnnotationProvider.AnnotationTable().Rows.Count > 0)
                    this.buildTree(this._AnnotationProvider.AnnotationTable().Rows[0]);

                if (this.treeViewAnnotation.Nodes.Count == 0)
                    this.toolStripButtonNewRootAnnotation_Click(null, null);
                //this.toolStripButtonNewAnnotation_Click(null, null);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public FormAnnotation(
            DiversityWorkbench.AnnotationProvider AnnotationProvider
            //System.Data.DataSet Dataset
            , System.Data.DataTable dtAnnotationType
            , string Title
            , string HelpNamespace)
            : this()
        {
            try
            {
                this.labelHeader.Visible = false;
                if (Title.Length > 0)
                    this.Text = Title;
                this.helpProvider.HelpNamespace = HelpNamespace;
                this.toolStripAnnotation.Visible = false;
                this.linkLabelURL.Text = "";
                this._AnnotationProvider = AnnotationProvider;
                //this._Dataset = Dataset;
                //this._DtAnnotation = this._Dataset.Tables["Annotation"];
                this._DtAnnotationType = dtAnnotationType;

                this.setDatabindings();

                this.labelReferencedTable.Visible = true;
                this.textBoxReferencedTable.Visible = true;

                //if (this._DtAnnotation.Rows.Count > 0)
                //    this.buildTree(this._DtAnnotation.Rows[0]);

                if (this._AnnotationProvider.AnnotationTable().Rows.Count > 0)
                    this.buildTree(this._AnnotationProvider.AnnotationTable().Rows[0]);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setDatabindings()
        {
            try
            {
                if (this.components != null)
                    this.AnnotationBindingSource = new System.Windows.Forms.BindingSource(this.components);
                else
                    this.AnnotationBindingSource = new System.Windows.Forms.BindingSource();

                ((System.ComponentModel.ISupportInitialize)(this.AnnotationBindingSource)).BeginInit();
                this.AnnotationBindingSource.DataSource = this._AnnotationProvider.AnnotationTable();
                //this.AnnotationBindingSource.DataMember = this._Dataset.Tables["Annotation"].TableName;
                //this.AnnotationBindingSource.DataSource = this._Dataset;
                this.textBoxAnnotation.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.AnnotationBindingSource, "Annotation", true));
                this.comboBoxTitle.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.AnnotationBindingSource, "Title", true));
                this.labelAnnotationType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.AnnotationBindingSource, "AnnotationType", true));
                this.linkLabelURL.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.AnnotationBindingSource, "URI", true));
                this.textBoxCreatedBy.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.AnnotationBindingSource, "LogCreatedBy", true));
                this.textBoxCreatedWhen.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.AnnotationBindingSource, "LogCreatedWhen", true));
                this.textBoxReferencedTable.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.AnnotationBindingSource, "ReferencedTable", true));
                ((System.ComponentModel.ISupportInitialize)(this.AnnotationBindingSource)).EndInit();

                // reference
                DiversityWorkbench.Reference L = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryReference.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)L;
                this.userControlModuleRelatedEntryReference.bindToData("Annotation", "ReferenceDisplayText", "ReferenceURI", this.AnnotationBindingSource);
                this.userControlModuleRelatedEntryReference.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace;

                // source
                DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntrySource.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntrySource.bindToData("Annotation", "SourceDisplayText", "SourceURI", this.AnnotationBindingSource);
                this.userControlModuleRelatedEntrySource.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Form

        private DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get
            {
                if (this._FormFunctions == null)
                    this._FormFunctions = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                return this._FormFunctions;
            }
        }

        private void FormAnnotation_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //foreach (System.Data.DataRow R in this._DtAnnotation.Rows)
                //{
                //    if (R.RowState == DataRowState.Deleted)
                //        continue;
                //    R.BeginEdit();
                //    R.EndEdit();
                //}
                //if (this.AnnotationBindingSource.Current != null)
                //    this.AnnotationBindingSource.EndEdit();
                ////this.FormFunctions.updateTable(this._Dataset, "Annotation", this._SqlDataAdapter, this.AnnotationBindingSource);
                //this._SqlDataAdapter.Update(this._DtAnnotation);
            }
            catch (System.Data.DBConcurrencyException ex)
            {
                System.Windows.Forms.MessageBox.Show("Could not save changes. Please repeat editing");
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                //System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void toolStripButtonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                null,
                null);
        }

        #endregion

        #region Tree

        private System.Windows.Forms.TreeNode buildTree(System.Data.DataRow SelectedRow)
        {
            System.Windows.Forms.TreeNode SelectedNode = new TreeNode();
            try
            {
                this.treeViewAnnotation.Nodes.Clear();
                string WhereClause = "";
                if (this._ReferencedTable != null)
                    WhereClause = " ReferencedTable = '" + this._ReferencedTable + "' AND ReferencedID = " + this._ReferencedID.ToString() +
                        " AND ReferencedAnnotationID IS NULL AND AnnotationType IN ('Annotation', 'Problem', 'Reference')";
                System.Collections.Generic.List<System.Data.DataRow> AddedAnnotationRows = new List<DataRow>();
                System.Data.DataRow[] RR = this._AnnotationProvider.AnnotationTable().Select(WhereClause, "AnnotationID");
                //System.Data.DataRow[] RR = this._Dataset.Tables["Annotation"].Select(WhereClause, "AnnotationID");
                foreach (System.Data.DataRow R in RR)
                {
                    if (!AddedAnnotationRows.Contains(R))
                    {
                        if (int.Parse(R["AnnotationID"].ToString()) < 0 && !R["ReferencedAnnotationID"].Equals(System.DBNull.Value))
                            continue;
                        AddedAnnotationRows.Add(R);
                        System.Windows.Forms.TreeNode N = this.AnnotationNode(R);
                        N.Tag = R;
                        this.treeViewAnnotation.Nodes.Add(N);
                        if (R == SelectedRow)
                            SelectedNode = N;
                        System.Windows.Forms.TreeNode SelectedChildNode = this.AddChildNodes(N, R, ref AddedAnnotationRows, SelectedRow);
                        if (SelectedNode.Tag == null && SelectedChildNode.Tag != null)
                            SelectedNode = SelectedChildNode;
                    }
                }
                System.Data.DataRow[] RRCheck = this._AnnotationProvider.AnnotationTable().Select(" ReferencedTable = '" + this._ReferencedTable + "'  AND AnnotationType IN ('Annotation', 'Problem', 'Reference') AND ReferencedID = " + this._ReferencedID.ToString(), "AnnotationID");
                //System.Data.DataRow[] RRCheck = this._Dataset.Tables["Annotation"].Select(" ReferencedTable = '" + this._ReferencedTable + "' AND ReferencedID = " + this._ReferencedID.ToString(), "AnnotationID");
                int iCheck = RRCheck.Length;
                if (iCheck > AddedAnnotationRows.Count)
                {
                    foreach (System.Data.DataRow RCheck in RRCheck)
                    {
                        if (!AddedAnnotationRows.Contains(RCheck))
                        {
                            RCheck["ReferencedAnnotationID"] = System.DBNull.Value;
                            System.Windows.Forms.TreeNode N = this.AnnotationNode(RCheck);
                            N.Tag = RCheck;
                            this.treeViewAnnotation.Nodes.Add(N);
                        }
                    }
                }

                this.treeViewAnnotation.ExpandAll();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            if (SelectedNode.Tag == null && this.treeViewAnnotation.Nodes.Count > 0)
                SelectedNode = this.treeViewAnnotation.Nodes[0];
            return SelectedNode;
        }

        private System.Windows.Forms.TreeNode AddChildNodes(System.Windows.Forms.TreeNode N, System.Data.DataRow R
            , ref System.Collections.Generic.List<System.Data.DataRow> AddedAnnotationRows
            , System.Data.DataRow SelectedRow)
        {
            System.Windows.Forms.TreeNode SelectedNode = new TreeNode();
            try
            {
                string AnnotationID = R["AnnotationID"].ToString();
                System.Data.DataRow[] rr = this._AnnotationProvider.AnnotationTable().Select("ReferencedAnnotationID = " + AnnotationID, "AnnotationID");// this._Dataset.Tables["Annotation"].Select("ReferencedAnnotationID = " + AnnotationID, "AnnotationID");
                foreach (System.Data.DataRow r in rr)
                {
                    if (!AddedAnnotationRows.Contains(r))
                    {
                        AddedAnnotationRows.Add(r);
                        System.Windows.Forms.TreeNode Nchild = this.AnnotationNode(r);
                        Nchild.Tag = r;
                        N.Nodes.Add(Nchild);
                        if (r == SelectedRow)
                            SelectedNode = Nchild;
                        System.Windows.Forms.TreeNode SelectedChildNode = this.AddChildNodes(Nchild, r, ref AddedAnnotationRows, SelectedRow);
                        if (SelectedNode.Tag == null && SelectedChildNode.Tag != null)
                            SelectedNode = SelectedChildNode;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return SelectedNode;
        }

        private System.Windows.Forms.TreeNode AnnotationNode(System.Data.DataRow R)
        {
            string Type = R["AnnotationType"].ToString().ToLower();
            //string Annotation = R["Annotation"].ToString();
            //if (Type == "reference" && 
            //    !R["ReferenceDisplayText"].Equals(System.DBNull.Value) &&
            //    R["ReferenceDisplayText"].ToString().Length > 0)
            //    Annotation = R["ReferenceDisplayText"].ToString() + " " + Annotation;
            //if (Annotation.IndexOf("\r\n") > -1) Annotation = Annotation.Replace("\r\n", " ");
            //if (Annotation.Length > 100) Annotation = Annotation.Substring(0, 100) + "...";
            System.Windows.Forms.TreeNode N = new TreeNode(this.NodeText(R));
            switch (Type)
            {
                case "annotation":
                    N.ImageIndex = 0;
                    break;
                case "reference":
                    N.ImageIndex = 1;
                    break;
                case "problem":
                    N.ImageIndex = 2;
                    break;
                default:
                    N.ImageIndex = 3;
                    break;
            }
            N.SelectedImageIndex = N.ImageIndex;

            return N;
        }

        private void treeViewAnnotation_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                this.tableLayoutPanelData.SuspendLayout();
                this.textBoxAnnotation.Visible = false;
                this.labelAnnotationType.Visible = false;
                this.labelDataRow.Visible = false;
                this.pictureBoxAnnotationType.Visible = false;
                this.linkLabelURL.Visible = false;
                this.userControlModuleRelatedEntryReference.Visible = false;
                this.buttonSetURL.Visible = false;
                this.labelAnnotationTitle.Visible = false;
                this.comboBoxTitle.Visible = false;
                this.labelSource.Visible = false;
                this.userControlModuleRelatedEntrySource.Visible = false;
                this.labelCreatedBy.Visible = false;
                this.labelCreatedWhen.Visible = false;
                this.textBoxCreatedBy.Visible = false;
                this.textBoxCreatedWhen.Visible = false;

                if (this.treeViewAnnotation.SelectedNode != null
                    && this.treeViewAnnotation.SelectedNode.Tag != null)
                {
                    System.Data.DataRow R = (System.Data.DataRow)this.treeViewAnnotation.SelectedNode.Tag;
                    string Type = R["AnnotationType"].ToString();
                    this.labelURL.Visible = true;
                    switch (Type)
                    {
                        case "Reference":
                            this.userControlModuleRelatedEntryReference.Visible = true;
                            this.labelReference.Visible = true;

                            this.linkLabelURL.Visible = false;
                            this.buttonSetURL.Visible = false;

                            this.textBoxAnnotation.Visible = true;
                            this.labelAnnotationType.Visible = true;
                            this.labelDataRow.Visible = true;
                            this.pictureBoxAnnotationType.Visible = true;
                            this.userControlModuleRelatedEntryReference.Visible = true;
                            this.labelAnnotationTitle.Visible = true;
                            this.comboBoxTitle.Visible = true;
                            this.labelSource.Visible = true;
                            this.userControlModuleRelatedEntrySource.Visible = true;
                            this.labelCreatedBy.Visible = true;
                            this.labelCreatedWhen.Visible = true;
                            this.textBoxCreatedBy.Visible = true;
                            this.textBoxCreatedWhen.Visible = true;
                            break;
                        case "Annotation":
                        case "Problem":
                            this.userControlModuleRelatedEntryReference.Visible = false;
                            this.labelReference.Visible = false;

                            this.textBoxAnnotation.Visible = true;
                            this.labelAnnotationType.Visible = true;
                            this.labelDataRow.Visible = true;
                            this.pictureBoxAnnotationType.Visible = true;
                            this.linkLabelURL.Visible = true;
                            this.buttonSetURL.Visible = true;
                            this.labelAnnotationTitle.Visible = true;
                            this.comboBoxTitle.Visible = true;
                            this.labelSource.Visible = true;
                            this.userControlModuleRelatedEntrySource.Visible = true;
                            this.labelCreatedBy.Visible = true;
                            this.labelCreatedWhen.Visible = true;
                            this.textBoxCreatedBy.Visible = true;
                            this.textBoxCreatedWhen.Visible = true;
                            break;
                        default:
                            this.userControlModuleRelatedEntryReference.Visible = false;
                            this.labelReference.Visible = false;

                            this.textBoxAnnotation.Visible = true;
                            this.labelAnnotationType.Visible = true;
                            this.labelDataRow.Visible = true;
                            this.pictureBoxAnnotationType.Visible = true;
                            this.linkLabelURL.Visible = false;
                            this.buttonSetURL.Visible = false;
                            this.labelAnnotationTitle.Visible = false;
                            this.labelURL.Visible = false;
                            this.comboBoxTitle.Visible = false;
                            this.labelSource.Visible = false;
                            this.userControlModuleRelatedEntrySource.Visible = false;
                            this.labelCreatedBy.Visible = true;
                            this.labelCreatedWhen.Visible = true;
                            this.textBoxCreatedBy.Visible = true;
                            this.textBoxCreatedWhen.Visible = true;
                            break;
                    }
                    int P = 0;
                    int ID = 0;
                    foreach (System.Data.DataRow r in this._AnnotationProvider.AnnotationTable().Rows)//._DtAnnotation.Rows)
                    {
                        if (r.RowState == DataRowState.Deleted || r.RowState == DataRowState.Detached)
                            continue;
                        if (r["AnnotationID"].ToString() == R["AnnotationID"].ToString())
                        {
                            int.TryParse(r["AnnotationID"].ToString(), out ID);
                            break;
                        }
                        P++;
                    }
                    this.AnnotationBindingSource.Position = P;
                    ///TODO: Hier sollte Editieren nur moeglich sein, wenn Datensatz noch nicht gespeichert ist - evtl. untaugliches Konzept
                    ///Wird besser an Rechten festgemacht d.h. User duerfen nur Insert machen, Editoren auch editieren
                    ///

                    bool CanEdit = true;
                    string TableName = "Annotation";
                    CanEdit = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");

                    //if (ID > 0)
                    //    CanEdit = false;
                    this.userControlModuleRelatedEntryReference.Enabled = CanEdit;
                    this.userControlModuleRelatedEntrySource.Enabled = CanEdit;
                    //this.linkLabelURL.Enabled = CanEdit;
                    this.buttonSetURL.Enabled = CanEdit;
                    this.textBoxAnnotation.ReadOnly = !CanEdit;
                    this.comboBoxTitle.Enabled = CanEdit;
                    this.tableLayoutPanelData.ResumeLayout();
                    if (this._ReferencedTable == null)
                        this.panelAnnotationType.Visible = false;
                    else this.panelAnnotationType.Visible = true;
                    this.pictureBoxAnnotationType.Image = this.imageListAnnotationType.Images[this.treeViewAnnotation.SelectedNode.ImageIndex];
                }
                this._SelectionCount++;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void treeViewAnnotation_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Hochzählen um korrekte Zuordnung bei Hierarchie sicherzustellen
            this._SelectionCount++;
        }

        #endregion

        #region Interface

        public string Header
        {
            set
            {
                this.labelHeader.Text = value;
            }
        }

        #endregion

        #region Data

        private void toolStripButtonNewRootAnnotation_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(this._DtAnnotationType, "Code", "Code", "Annotation type", "Please select the type of the annotation");
                f.CanEditValuesInList = false;
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    string AnnotationType = f.SelectedValue;
                    //int AnnotationID = this.InsertNewAnnotation(AnnotationType, this._ReferencedID, this._ReferencedTable, AnnotationType, null);
                    System.Data.DataRow R = this._AnnotationProvider.AnnotationTable().NewRow();//._DtAnnotation.NewRow();
                    //R["AnnotationID"] = AnnotationID;
                    R["AnnotationType"] = AnnotationType;
                    R["Annotation"] = AnnotationType;
                    R["ReferencedID"] = this._ReferencedID;
                    R["ReferencedTable"] = this._ReferencedTable;
                    this._AnnotationProvider.AnnotationTable().Rows.Add(R);//._DtAnnotation.Rows.Add(R);
                    //this._DtAnnotation.AcceptChanges();
                    this.AnnotationBindingSource.Position = this._AnnotationProvider.AnnotationTable().Rows.Count - 1;// this._DtAnnotation.Rows.Count - 1;
                    System.Windows.Forms.TreeNode N = this.buildTree(R);
                    if (N.Tag != null)
                        this.treeViewAnnotation.SelectedNode = N;
                    //this._SqlDataAdapter.Update(this._DtAnnotation);
                    //this.FormFunctions.updateTable(this._Dataset, "Annotation", this._SqlDataAdapter, this.AnnotationBindingSource);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonNewAnnotation_Click(object sender, EventArgs e)
        {
            if (this.treeViewAnnotation.SelectedNode == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a annotation");
                return;
            }
            try
            {
                //this._AnnotationProvider.UpdateAnnotationTable();

                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(this._DtAnnotationType, "Code", "Code", "Annotation type", "Please select the type of the annotation");
                f.CanEditValuesInList = false;
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    string AnnotationType = f.SelectedValue;
                    System.Data.DataRow rTag = (System.Data.DataRow)this.treeViewAnnotation.SelectedNode.Tag;
                    int ReferencedAnnotationID = int.Parse(rTag["AnnotationID"].ToString());
                    if (ReferencedAnnotationID < 0)
                    {
                        System.Windows.Forms.MessageBox.Show("Please save the " + rTag["AnnotationType"].ToString().ToLower() + " before adding a dependent " + AnnotationType.ToLower());
                        return;
                    }
                    //int AnnotationID = this.InsertNewAnnotation(AnnotationType, this._ReferencedID, this._ReferencedTable, AnnotationType, ReferencedAnnotationID); 
                    System.Data.DataRow R = this._AnnotationProvider.AnnotationTable().NewRow();// this._DtAnnotation.NewRow();
                    //R["AnnotationID"] = AnnotationID;
                    R["AnnotationType"] = AnnotationType;
                    R["Annotation"] = AnnotationType;
                    R["ReferencedID"] = this._ReferencedID;
                    R["ReferencedTable"] = this._ReferencedTable;
                    R["ReferencedAnnotationID"] = ReferencedAnnotationID;
                    //if (this.treeViewAnnotation.SelectedNode != null &&
                    //    this.treeViewAnnotation.SelectedNode.Tag != null &&
                    //    this._SelectionCount > 1)
                    //{
                    //    System.Data.DataRow rTag = (System.Data.DataRow)this.treeViewAnnotation.SelectedNode.Tag;
                    //    R["ReferencedAnnotationID"] = int.Parse(rTag["AnnotationID"].ToString());
                    //}
                    this._AnnotationProvider.AnnotationTable().Rows.Add(R);// this._DtAnnotation.Rows.Add(R);
                    //this._DtAnnotation.AcceptChanges();
                    this.AnnotationBindingSource.Position = this._AnnotationProvider.AnnotationTable().Rows.Count - 1;// this._DtAnnotation.Rows.Count - 1;
                    System.Windows.Forms.TreeNode N = this.buildTree(R);
                    if (N.Tag != null)
                        this.treeViewAnnotation.SelectedNode = N;
                    //this._SqlDataAdapter.Update(this._DtAnnotation);
                    //this.FormFunctions.updateTable(this._Dataset, "Annotation", this._SqlDataAdapter, this.AnnotationBindingSource);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.AnnotationBindingSource.Current != null)
                {
                    if (System.Windows.Forms.MessageBox.Show("Delete the selected annotation?", "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this.AnnotationBindingSource.Current;
                        R.Delete();
                        System.Windows.Forms.TreeNode N = this.buildTree(null);
                        if (N.Tag != null)
                            this.treeViewAnnotation.SelectedNode = N;
                        else
                        {
                            this.treeViewAnnotation.SelectedNode = null;
                            this.treeViewAnnotation_AfterSelect(null, null);
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonSetURL_Click(object sender, EventArgs e)
        {
            string Link = this.linkLabelURL.Text;
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(Link);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                this.linkLabelURL.Text = f.URL;
        }

        private void textBoxAnnotation_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.treeViewAnnotation.SelectedNode != null)
            {
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewAnnotation.SelectedNode.Tag;
                R.BeginEdit();
                R["Annotation"] = this.textBoxAnnotation.Text;
                R.EndEdit();
                //string[] TextParts = this.textBoxAnnotation.Text.Split(new char[]{'\r'});
                treeViewAnnotation.SelectedNode.Text = this.NodeText(R); // TextParts[0];
            }
        }

        private string NodeText(System.Data.DataRow R)
        {
            string Text = "";
            try
            {
                Text = R["Annotation"].ToString();
                string Type = R["AnnotationType"].ToString().ToLower();
                string[] TextParts = Text.Split(new char[] { '\r' });
                if (TextParts[0].Trim().Length > 0)
                    Text = TextParts[0];
                if (Type == "reference" &&
                    !R["ReferenceDisplayText"].Equals(System.DBNull.Value) &&
                    R["ReferenceDisplayText"].ToString().Length > 0)
                    Text = R["ReferenceDisplayText"].ToString() + " (" + Text + ")";
                if (Text.Length > 100)
                    Text = Text.Substring(0, 100) + "...";
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Text;
        }

        private void comboBoxTitle_DropDown(object sender, EventArgs e)
        {
            string SQL = "SELECT DISTINCT Title FROM Annotation ORDER BY Title";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            this.comboBoxTitle.DataSource = dt;
            this.comboBoxTitle.DisplayMember = "Title";
            this.comboBoxTitle.ValueMember = "Title";
        }

        private void linkLabelURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this.linkLabelURL.Text.Length > 0)
            {
                try
                {
                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(this.linkLabelURL.Text);
                    info.UseShellExecute = true;
                    System.Diagnostics.Process.Start(info);
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private int InsertNewAnnotation(string AnnotationType, int ReferencedID, string ReferencedTable, string Annotation, int? ReferencedAnnotationID)
        {
            int ID = 0;
            try
            {
                string SQL = "INSERT INTO Annotation (AnnotationType, ReferencedID, ReferencedTable, Annotation ";
                string SqlValues = "VALUES ('" + AnnotationType + "', " + ReferencedID.ToString() + ", '" + ReferencedTable + "', '" + Annotation + "' ";
                if (ReferencedAnnotationID != null)
                {
                    SQL += ", ReferencedAnnotationID";
                    SqlValues += ", " + ReferencedAnnotationID.ToString();
                }
                SQL += ") " + SqlValues + ") SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                ID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return ID;
        }

        #endregion

    }
}
