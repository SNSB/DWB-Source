using DiversityWorkbench.DwbManual;
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
    public partial class FormAnnotation : Form
    {

        #region Parameter
        
        private System.Data.DataTable _DtAnnotation;
        private System.Data.DataTable _DtAnnotationType;
        private DiversityCollection.Datasets.DataSetCollectionSpecimen _DsCollectionSpecimen;
        private System.Data.DataRow _SelectedDataRow;
        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        private int _SelectionCount = 0;
        private System.Windows.Forms.BindingSource AnnotationBindingSource;
        
        #endregion

        #region Construction
       
        private FormAnnotation()
        {
            InitializeComponent();
            //this.treeViewAnnotation.ImageList = DiversityCollection.Specimen.ImageList;
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
            this.FormFunctions.setDescriptions();
        }

        public FormAnnotation(
            DiversityCollection.Datasets.DataSetCollectionSpecimen DsCollectionSpecimen
            , System.Data.DataRow R
            , System.Data.DataTable dtAnnotationType)
            : this()
        {
            //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            //DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxAnnotationType, "AnnotationType_Enum", con, false, false, true);
            this._SelectedDataRow = R;
            this.linkLabelURL.Text = "";
            this._DsCollectionSpecimen = DsCollectionSpecimen;
            this._DtAnnotation = this._DsCollectionSpecimen.Annotation;
            this._DtAnnotationType = dtAnnotationType;

            this.AnnotationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.AnnotationBindingSource)).BeginInit();
            this.AnnotationBindingSource.DataMember = "Annotation";
            this.AnnotationBindingSource.DataSource = this._DsCollectionSpecimen;
            this.textBoxAnnotation.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.AnnotationBindingSource, "Annotation", true));
            this.labelAnnotationType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.AnnotationBindingSource, "AnnotationType", true));
            this.linkLabelURL.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.AnnotationBindingSource, "URI", true));
            this.textBoxCreatedBy.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.AnnotationBindingSource, "LogCreatedBy", true));
            this.textBoxCreatedWhen.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.AnnotationBindingSource, "LogCreatedWhen", true));
            ((System.ComponentModel.ISupportInitialize)(this.AnnotationBindingSource)).EndInit();

            // reference
            DiversityWorkbench.Reference L = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
            this.userControlModuleRelatedEntryReference.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)L;
            this.userControlModuleRelatedEntryReference.bindToData("Annotation", "DisplayText", "URI", this.AnnotationBindingSource);
            this.userControlModuleRelatedEntryReference.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace;

            if (this._DtAnnotation.Rows.Count == 0)
            {
                this.textBoxAnnotation.Visible = false;
                this.labelAnnotationType.Visible = false;
                this.labelDataRow.Visible = false;
                this.pictureBoxAnnotationType.Visible = false;
                this.linkLabelURL.Visible = false;
                this.userControlModuleRelatedEntryReference.Visible = false;
                this.buttonSetURL.Visible = false;
            }
            else this.buildTree();
            //this.treeViewAnnotation.SelectedNode = null;
            //this.treeViewAnnotation_AfterSelect(null, null);
            //if (this.treeViewAnnotation.SelectedNode != null)
            //{
            //}
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

        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        #endregion

        #region Tree
        
        private void buildTree()
        {
            this.treeViewAnnotation.Nodes.Clear();
            string WhereClause = "";
            System.Collections.Generic.List<System.Data.DataRow> AddedAnnotationRows = new List<DataRow>();
            switch (this._SelectedDataRow.Table.TableName)
            {
                case "CollectionEventSeries":
                    WhereClause = "CollectionEventSeries = " + this._SelectedDataRow["SeriesID"].ToString();
                    break;
                case "CollectionEvent":
                    WhereClause = "CollectionEventID = " + this._SelectedDataRow["CollectionEventID"].ToString();
                    break;
                case "CollectionSpecimen":
                    WhereClause = "CollectionSpecimenID = " + this._SelectedDataRow["CollectionSpecimenID"].ToString();
                    break;
                case "CollectionSpecimenPart":
                    WhereClause = "CollectionSpecimenID = " + this._SelectedDataRow["CollectionSpecimenID"].ToString() +
                    " AND SpecimenPartID = " + this._SelectedDataRow["SpecimenPartID"].ToString();
                    break;
                case "IdentificationUnit":
                    WhereClause = "CollectionSpecimenID = " + this._SelectedDataRow["CollectionSpecimenID"].ToString() +
                    " AND IdentificationUnitID = " + this._SelectedDataRow["IdentificationUnitID"].ToString();
                    break;
                case "IdentificationUnitInPart":
                    WhereClause = "CollectionSpecimenID" + this._SelectedDataRow["CollectionSpecimenID"].ToString() +
                    " AND SpecimenPartID = " + this._SelectedDataRow["SpecimenPartID"].ToString() +
                    " AND IdentificationUnitID = " + this._SelectedDataRow["IdentificationUnitID"].ToString();
                    break;
            }
            System.Data.DataRow[] RR = this._DsCollectionSpecimen.Annotation.Select(WhereClause, "AnnotationID");
            foreach (System.Data.DataRow R in RR)
            {
                if (!AddedAnnotationRows.Contains(R))
                {
                    if (int.Parse(R["AnnotationID"].ToString()) < 0 && !R["ReferencedAnnotationID"].Equals(System.DBNull.Value))
                        continue;
                    AddedAnnotationRows.Add(R);
                    string Annotation = R["Annotation"].ToString();
                    if (Annotation.Length > 100) Annotation = Annotation.Substring(0, 100) + "...";
                    System.Windows.Forms.TreeNode N = this.AnnotationNode(R);
                    N.Tag = R;
                    this.treeViewAnnotation.Nodes.Add(N);
                    this.AddChildNodes(N, R, ref AddedAnnotationRows);
                }
            }
            this.treeViewAnnotation.ExpandAll();
        }

        private void AddChildNodes(System.Windows.Forms.TreeNode N, System.Data.DataRow R
            , ref System.Collections.Generic.List<System.Data.DataRow> AddedAnnotationRows)
        {
            {
                string AnnotationID = R["AnnotationID"].ToString();
                System.Data.DataRow[] rr = this._DsCollectionSpecimen.Annotation.Select("ReferencedAnnotationID = " + AnnotationID, "AnnotationID");
                foreach (System.Data.DataRow r in rr)
                {
                    if (!AddedAnnotationRows.Contains(r))
                    {
                        AddedAnnotationRows.Add(r);
                        string Annotation = r["Annotation"].ToString();
                        if (Annotation.Length > 100) Annotation = Annotation.Substring(0, 100) + "...";
                        System.Windows.Forms.TreeNode Nchild = this.AnnotationNode(r);
                        Nchild.Tag = r;
                        N.Nodes.Add(Nchild);
                        this.AddChildNodes(Nchild, r, ref AddedAnnotationRows);
                    }
                }
            }
        }

        private System.Windows.Forms.TreeNode AnnotationNode(System.Data.DataRow R)
        {
            string Annotation = R["Annotation"].ToString();
            if (Annotation.Length > 100) Annotation = Annotation.Substring(0, 100) + "...";
            System.Windows.Forms.TreeNode N = new TreeNode(Annotation);
            string Type = R["AnnotationType"].ToString().ToLower();
            switch (Type)
            {
                case "reference":
                    N.ImageIndex = 1;
                    break;
                case "problem":
                    N.ImageIndex = 2;
                    break;
                default:
                    N.ImageIndex = 0;
                    break;
            }
            N.SelectedImageIndex = N.ImageIndex;

            //if (!R["IdentificationUnitID"].Equals(System.DBNull.Value))
            //{
            //    System.Data.DataRow[] rrUnit = this._DsCollectionSpecimen.IdentificationUnit.Select("IdentificationUnitID = " + R["IdentificationUnitID"].ToString());
            //    if (rrUnit.Length > 0)
            //    {
            //        N.ImageIndex = DiversityCollection.Specimen.TaxonImageIndex(rrUnit[0]["TaxonomicGroup"].ToString(), rrUnit[0]["UnitDescription"].ToString(), false);
            //    }
            //}
            //else if (!R["SpecimenPartID"].Equals(System.DBNull.Value))
            //{
            //    System.Data.DataRow[] rrPart = this._DsCollectionSpecimen.CollectionSpecimenPart.Select("SpecimenPartID = " + R["SpecimenPartID"].ToString());
            //    if (rrPart.Length > 0)
            //    {
            //        N.ImageIndex = DiversityCollection.Specimen.MaterialCategoryImage(rrPart[0]["MaterialCategory"].ToString(), false);
            //    }
            //}
            //else if (!R["CollectionSpecimenID"].Equals(System.DBNull.Value))
            //{
            //    N.ImageIndex = (int)DiversityCollection.Specimen.OverviewImage.Specimen;
            //}
            //else if (!R["CollectionEventID"].Equals(System.DBNull.Value))
            //{
            //    N.ImageIndex = (int)DiversityCollection.Specimen.OverviewImage.Event;
            //}
            //else if (!R["SeriesID"].Equals(System.DBNull.Value))
            //{
            //    N.ImageIndex = (int)DiversityCollection.Specimen.OverviewImage.EventSeries;
            //}
            //N.SelectedImageIndex = N.ImageIndex;
            return N;
        }
        
        private void treeViewAnnotation_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                this.textBoxAnnotation.Visible = false;
                this.labelAnnotationType.Visible = false;
                this.labelDataRow.Visible = false;
                this.pictureBoxAnnotationType.Visible = false;
                this.linkLabelURL.Visible = false;
                this.userControlModuleRelatedEntryReference.Visible = false;
                this.buttonSetURL.Visible = false;

                System.Data.DataRow R = (System.Data.DataRow)this.treeViewAnnotation.SelectedNode.Tag;
                string Type = R["AnnotationType"].ToString();
                switch (Type)
                {
                    case "Reference":
                        this.labelAnnotationType.Visible = true;
                        this.labelDataRow.Visible = true;
                        this.pictureBoxAnnotationType.Visible = true;
                        this.textBoxAnnotation.Visible = true;
                        this.userControlModuleRelatedEntryReference.Visible = true;
                        break;
                    default:
                        this.textBoxAnnotation.Visible = true;
                        this.labelAnnotationType.Visible = true;
                        this.labelDataRow.Visible = true;
                        this.pictureBoxAnnotationType.Visible = true;
                        this.linkLabelURL.Visible = true;
                        this.buttonSetURL.Visible = true; 
                        break;
                }
                this.setRowImage(R);
                this.setRowHeader(R);
                int P = 0;
                foreach(System.Data.DataRow r in this._DtAnnotation.Rows)
                {
                    if (r["AnnotationID"].ToString() == R["AnnotationID"].ToString())
                        break;
                    P++;
                }
                this.AnnotationBindingSource.Position = P;
            }
            catch { }
            this._SelectionCount++;
        }

        private void setRowImage(System.Data.DataRow R)
        {
            try
            {
                if (!R["IdentificationUnitID"].Equals(System.DBNull.Value))
                {
                    System.Data.DataRow[] rrUnit = this._DsCollectionSpecimen.IdentificationUnit.Select("IdentificationUnitID = " + R["IdentificationUnitID"].ToString());
                    if (rrUnit.Length > 0)
                    {
                        this.pictureBoxAnnotationType.Image = DiversityCollection.Specimen.TaxonImage(false, rrUnit[0]["TaxonomicGroup"].ToString());
                    }
                }
                else if (!R["SpecimenPartID"].Equals(System.DBNull.Value))
                {
                    System.Data.DataRow[] rrPart = this._DsCollectionSpecimen.CollectionSpecimenPart.Select("SpecimenPartID = " + R["SpecimenPartID"].ToString());
                    if (rrPart.Length > 0)
                    {
                        this.pictureBoxAnnotationType.Image = DiversityCollection.Specimen.MaterialCategoryImage(false, rrPart[0]["MaterialCategory"].ToString());
                    }
                }
                else if (!R["CollectionSpecimenID"].Equals(System.DBNull.Value))
                {
                    this.pictureBoxAnnotationType.Image = DiversityCollection.Specimen.Image("CollectionSpecimen.ico");
                }
                else if (!R["CollectionEventID"].Equals(System.DBNull.Value))
                {
                    this.pictureBoxAnnotationType.Image = DiversityCollection.Specimen.Image("CollectionEvent.ico");
                }
                else if (!R["SeriesID"].Equals(System.DBNull.Value))
                {
                    this.pictureBoxAnnotationType.Image = DiversityCollection.Specimen.Image("CollectionEventSeries.ico");
                }
                //Type = Type.ToLower();
                //switch (Type)
                //{
                //    case "reference":
                //        this.pictureBoxAnnotationType.Image = this.imageListAnnotationType.Images[1];
                //        break;
                //    case "problem":
                //        this.pictureBoxAnnotationType.Image = this.imageListAnnotationType.Images[2];
                //        break;
                //    default:
                //        this.pictureBoxAnnotationType.Image = this.imageListAnnotationType.Images[0];
                //        break;
                //}
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void setRowHeader(System.Data.DataRow R)
        {
            try
            {
                System.Data.DataRow Rsource;
                if (!R["SeriesID"].Equals(System.DBNull.Value))
                {
                    this.labelDataRow.Text = "for the event series";
                    return;
                }
                else if (!R["CollectionEventID"].Equals(System.DBNull.Value))
                {
                    Rsource = this._DsCollectionSpecimen.CollectionEvent.Rows[0];
                    DiversityCollection.HierarchyNode N = new HierarchyNode(Rsource, false);
                    this.labelDataRow.Text = "for " + N.Text.Trim();
                }
                else if (!R["IdentificationUnitID"].Equals(System.DBNull.Value) && !R["SpecimenPartID"].Equals(System.DBNull.Value))
                {
                    System.Data.DataRow[] RR = this._DsCollectionSpecimen.IdentificationUnitInPart.Select("IdentificationUnitID = " + R["IdentificationUnitID"].ToString() + " AND SpecimenPartID = " + R["SpecimenPartID"].ToString());
                    Rsource = RR[0];
                    DiversityCollection.HierarchyNode N = new HierarchyNode(Rsource, false);
                    this.labelDataRow.Text = "for " + N.Text.Trim();
                }
                else if (!R["IdentificationUnitID"].Equals(System.DBNull.Value))
                {
                    System.Data.DataRow[] RR = this._DsCollectionSpecimen.IdentificationUnit.Select("IdentificationUnitID = " + R["IdentificationUnitID"].ToString());
                    Rsource = RR[0];
                    DiversityCollection.HierarchyNode N = new HierarchyNode(Rsource, false);
                    this.labelDataRow.Text = "for " + N.Text.Trim();
                }
                else if (!R["SpecimenPartID"].Equals(System.DBNull.Value))
                {
                    System.Data.DataRow[] RR = this._DsCollectionSpecimen.CollectionSpecimenPart.Select("SpecimenPartID = " + R["SpecimenPartID"].ToString());
                    Rsource = RR[0];
                    DiversityCollection.HierarchyNode N = new HierarchyNode(Rsource, false);
                    this.labelDataRow.Text = "for " + N.Text.Trim();
                }
                else if (!R["CollectionSpecimenID"].Equals(System.DBNull.Value))
                {
                    Rsource = this._DsCollectionSpecimen.CollectionSpecimen.Rows[0];
                    DiversityCollection.HierarchyNode N = new HierarchyNode(Rsource, false);
                    this.labelDataRow.Text = "for " + N.Text.Trim();
                }
            }
            catch { }
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
        
        private void toolStripButtonNewAnnotation_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(this._DtAnnotationType, "Annotation type", "Code", "Code", "Please select the type of the annotation");
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    string AnnotationType = f.SelectedValue;
                    System.Data.DataRow R = this._DtAnnotation.NewRow();
                    R["AnnotationType"] = AnnotationType;
                    R["Annotation"] = AnnotationType + this.labelHeader.Text.Replace("Annotations", "").Trim();
                    switch (this._SelectedDataRow.Table.TableName)
                    {
                        case "CollectionEventSeries":
                            R["SeriesID"] = int.Parse(this._SelectedDataRow["SeriesID"].ToString());
                            break;
                        case "CollectionEvent":
                            R["CollectionEventID"] = int.Parse(this._SelectedDataRow["CollectionEventID"].ToString());
                            break;
                        case "CollectionSpecimen":
                            R["CollectionSpecimenID"] = int.Parse(this._SelectedDataRow["CollectionSpecimenID"].ToString());
                            break;
                        case "CollectionSpecimenPart":
                            R["CollectionSpecimenID"] = int.Parse(this._SelectedDataRow["CollectionSpecimenID"].ToString());
                            R["SpecimenPartID"] = int.Parse(this._SelectedDataRow["SpecimenPartID"].ToString());
                            break;
                        case "IdentificationUnit":
                            R["CollectionSpecimenID"] = int.Parse(this._SelectedDataRow["CollectionSpecimenID"].ToString());
                            R["IdentificationUnitID"] = int.Parse(this._SelectedDataRow["IdentificationUnitID"].ToString());
                            break;
                        case "IdentificationUnitInPart":
                            R["CollectionSpecimenID"] = int.Parse(this._SelectedDataRow["CollectionSpecimenID"].ToString());
                            R["SpecimenPartID"] = int.Parse(this._SelectedDataRow["SpecimenPartID"].ToString());
                            R["IdentificationUnitID"] = int.Parse(this._SelectedDataRow["IdentificationUnitID"].ToString());
                            break;
                    }
                    if (this.treeViewAnnotation.SelectedNode != null &&
                        this.treeViewAnnotation.SelectedNode.Tag != null &&
                        this._SelectionCount > 1)
                    {
                        System.Data.DataRow rTag = (System.Data.DataRow)this.treeViewAnnotation.SelectedNode.Tag;
                        R["ReferencedAnnotationID"] = int.Parse(rTag["AnnotationID"].ToString());
                    }
                    this._DtAnnotation.Rows.Add(R);
                    this.AnnotationBindingSource.Position = this._DtAnnotation.Rows.Count - 1;
                    this.buildTree();
                }
            }
            catch { }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonNewRoot_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(this._DtAnnotationType, "Annotation type", "Code", "Code", "Please select the type of the annotation");
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    string AnnotationType = f.SelectedValue;
                    System.Data.DataRow R = this._DtAnnotation.NewRow();
                    R["AnnotationType"] = AnnotationType;
                    R["Annotation"] = AnnotationType + this.labelHeader.Text.Replace("Annotations", "").Trim();
                    switch (this._SelectedDataRow.Table.TableName)
                    {
                        case "CollectionEventSeries":
                            R["SeriesID"] = int.Parse(this._SelectedDataRow["SeriesID"].ToString());
                            break;
                        case "CollectionEvent":
                            R["CollectionEventID"] = int.Parse(this._SelectedDataRow["CollectionEventID"].ToString());
                            break;
                        case "CollectionSpecimen":
                            R["CollectionSpecimenID"] = int.Parse(this._SelectedDataRow["CollectionSpecimenID"].ToString());
                            break;
                        case "CollectionSpecimenPart":
                            R["CollectionSpecimenID"] = int.Parse(this._SelectedDataRow["CollectionSpecimenID"].ToString());
                            R["SpecimenPartID"] = int.Parse(this._SelectedDataRow["SpecimenPartID"].ToString());
                            break;
                        case "IdentificationUnit":
                            R["CollectionSpecimenID"] = int.Parse(this._SelectedDataRow["CollectionSpecimenID"].ToString());
                            R["IdentificationUnitID"] = int.Parse(this._SelectedDataRow["IdentificationUnitID"].ToString());
                            break;
                        case "IdentificationUnitInPart":
                            R["CollectionSpecimenID"] = int.Parse(this._SelectedDataRow["CollectionSpecimenID"].ToString());
                            R["SpecimenPartID"] = int.Parse(this._SelectedDataRow["SpecimenPartID"].ToString());
                            R["IdentificationUnitID"] = int.Parse(this._SelectedDataRow["IdentificationUnitID"].ToString());
                            break;
                    }
                    this._DtAnnotation.Rows.Add(R);
                    this.AnnotationBindingSource.Position = this._DtAnnotation.Rows.Count - 1;
                    this.buildTree();
                }
            }
            catch { }
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
                treeViewAnnotation.SelectedNode.Text = this.textBoxAnnotation.Text;
        }

        #endregion

        #region Manual

        /// <summary>
        /// Adding event deletates to form and controls
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task InitManual()
        {
            try
            {

                DiversityWorkbench.DwbManual.Hugo manual = new Hugo(this.helpProvider, this);
                if (manual != null)
                {
                    await manual.addKeyDownF1ToForm();
                }
            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// ensure that init is only done once
        /// </summary>
        private bool _InitManualDone = false;


        /// <summary>
        /// KeyDown of the form adding event deletates to form and controls within the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (!_InitManualDone)
                {
                    await this.InitManual();
                    _InitManualDone = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion

    }
}
