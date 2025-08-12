using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityCollection.UserControls
{
    public partial class UserControl_EventSeriesDescriptor : UserControl__Data
    {
        #region Construction
        public UserControl_EventSeriesDescriptor(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }


        #endregion

        #region Public

        //public void ShowImages(int? SeriesID)
        //{
        //    System.Data.DataRow[] RR = null;
        //    if (SeriesID != null)
        //    {
        //        RR = this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage.Select("SeriesID = " + SeriesID.ToString());
        //    }
        //    //this.FormFunctions.FillImageList(this.listBoxCollectionEventSeriesImages, this.imageListCollectionEventSeries, this.imageListForm, RR, "URI", this.userControlImageCollectionEventSeries);
        //}

        //public override void setAvailability()
        //{
        //    //bool AnySeries = true;
        //    //if (this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeries.Rows.Count == 0)
        //    //    AnySeries = false;
        //    //if (AnySeries)
        //    //{
        //    //    base.setAvailability();
        //    //    if (!this._iMainForm.ReadOnly())
        //    //    {
        //    //        this.tableLayoutPanelCollectionEventSeriesImages.Enabled = true;
        //    //        bool PermitUpdate = this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE];
        //    //        if (this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage.Rows.Count == 0)
        //    //            PermitUpdate = false;
        //    //        this.tableLayoutPanelEventSeriesImageIPR.Enabled = PermitUpdate;
        //    //        this.tableLayoutPanelEventSeriesImageLicense.Enabled = PermitUpdate;
        //    //        this.tableLayoutPanelEventSeriesImageType.Enabled = PermitUpdate;

        //    //        this.toolStripButtonCollectionEventSeriesImageDelete.Enabled = this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.DELETE];
        //    //        this.toolStripButtonCollectionEventSeriesImageNew.Enabled = this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.INSERT];
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    this.tableLayoutPanelCollectionEventSeriesImages.Enabled = false;
        //    //}
        //}

        //public void setAvailability(bool IsAvailable)
        //{
        //    //this.tableLayoutPanelEventSeriesImageIPR.Enabled = IsAvailable;
        //    //this.tableLayoutPanelEventSeriesImageLicense.Enabled = IsAvailable;
        //    //this.tableLayoutPanelEventSeriesImageType.Enabled = IsAvailable;
        //}

        public override void SetPosition(int Position = 0)
        {
            base.SetPosition(Position);
            //this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryEventSeriesImageCreator, "CreatorAgentURI");
            //this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryEventSeriesImageLicenseHolder, "LicenseHolderAgentURI");
        }

        public override void SetPositionByID(int ID, int Position)
        {
            int i = Position;
            this.SeriesID = ID;
            // try to find position
            //try
            //{
            //    i = 0;
            //    System.Data.DataSet ds = (System.Data.DataSet)this._Source.DataSource;
            //    System.Data.DataTable dt = ds.Tables["CollectionEventSeriesDescriptor"];
            //    foreach (System.Data.DataRow r in dt.Rows)
            //    {
            //        int _ID;
            //        if (int.TryParse(r[0].ToString(), out _ID) && _ID == ID)
            //            break;
            //        i++;
            //    }
            //    Position = i;
            //}
            //catch (System.Exception ex)
            //{
            //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //}
            this._Source.CurrencyManager.Position = Position;
            //System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            this.buildDescriptorTree();
            this.treeViewDescriptor_AfterSelect(null, null);
            //this.textBoxEventSeriesID.Text = R["SeriesID"].ToString();
            //this.setGeographyText();
            //this.setAvailability();
            //this.setDate();
            //this.setDate(false);
            //this.initDescriptors(R);
        }



        #endregion

        #region Control

        private void initControl()
        {
            this._Source.DataMember = "CollectionEventSeriesDescriptor";
            this._Source.DataSource = this._iMainForm.DataSetCollectionEventSeries();

            this.initDescriptorControls();

            this.initDescriptorTypeImages();

            //this.comboBoxCollectionEventSeriesImageType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "ImageType", true));
            //this.comboBoxEventSeriesImageWithholdingReason.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "DataWithholdingReason", true));

            //this._EnumComboBoxes = new Dictionary<ComboBox, string>();
            //this._EnumComboBoxes.Add(this.comboBoxCollectionEventSeriesImageType, "CollEventSeriesImageType_Enum");
            //this.InitLookupSources();

            //this.textBoxCollectionEventSeriesImageNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Notes", true));
            //this.textBoxCollectionEventSeriesImageInternalNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "InternalNotes", true));
            //this.textBoxCollectionEventSeriesImageTitle.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Title", true));
            //this.textBoxEventSeriesImageIPR.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "IPR", true));
            //this.textBoxEventSeriesImageCopyrightStatement.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "CopyrightStatement", true));
            //this.textBoxEventSeriesImageLicenseType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "LicenseType", true));
            //this.textBoxEventSeriesImageLicenseYear.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "LicenseYear", true));


            //this.initRemoteConnections();

            //this.SetDataWithholdingControl(this.comboBoxEventSeriesImageWithholdingReason, this.pictureBoxEventSeriesImageWithholdingReason);

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();

            //DiversityWorkbench.Settings.WebViewUsage(this.toolStripButtonWebView);
        }

        private void initRemoteConnections()
        {
            //DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
            //this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryEventSeriesImageCreator, (DiversityWorkbench.IWorkbenchUnit)A, "CollectionEventSeriesImage", "CreatorAgent", "CreatorAgentURI", this._Source);
            //this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryEventSeriesImageLicenseHolder, (DiversityWorkbench.IWorkbenchUnit)A, "CollectionEventSeriesImage", "LicenseHolder", "LicenseHolderAgentURI", this._Source);
            //this.userControlModuleRelatedEntryEventSeriesImageCreator.setForeColor(System.Drawing.Color.Blue);
            //this.userControlModuleRelatedEntryEventSeriesImageLicenseHolder.setForeColor(System.Drawing.Color.Blue);

        }

        public override void InitLookupSources() { this.InitEnums(); }

        #region Toolstrip

        private void toolStripButtonCollectionEventSeriesImageNew_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (this._iMainForm.EventSeriesID() == null) return;

            //    int? ProjectID = this._iMainForm.ProjectID();
            //    string AccessionNumber = "";
            //    if (!this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows[0]["AccessionNumber"].Equals(System.DBNull.Value))
            //        AccessionNumber = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows[0]["AccessionNumber"].ToString();

            //    string RowGUID = System.Guid.NewGuid().ToString();
            //    DiversityWorkbench.Forms.FormGetImage f = new DiversityWorkbench.Forms.FormGetImage(ProjectID, AccessionNumber);
            //    if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //    {
            //        if (f.ImagePath.Length > 0)
            //        {
            //            DiversityCollection.Datasets.DataSetCollectionEventSeries.CollectionEventSeriesImageRow R = this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage.NewCollectionEventSeriesImageRow();
            //            R.SeriesID = (int)this._iMainForm.EventSeriesID();
            //            R.URI = f.URIImage;
            //            R.Description = f.Exif;
            //            this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage.Rows.Add(R);
            //            this.FormFunctions.FillImageList(this.listBoxCollectionEventSeriesImages, this.imageListCollectionEventSeries, this.imageListForm, this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage, "URI", this.userControlImageCollectionEventSeries);
            //            this.setAvailability();
            //        }
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message);
            //}
        }

        private void toolStripButtonCollectionEventSeriesImageDelete_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    System.Windows.Forms.BindingManagerBase BMB = this.BindingContext[this._iMainForm.DataSetCollectionEventSeries(), "CollectionEventSeriesImage"];
            //    int p = BMB.Position;
            //    System.Data.DataRow r = this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage.Rows[p];
            //    if (r.RowState != System.Data.DataRowState.Deleted)
            //    {
            //        this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage.Rows[p].Delete();
            //        this.FormFunctions.FillImageList(this.listBoxCollectionEventSeriesImages, this.imageListCollectionEventSeries, this.imageListForm, this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage, "URI", this.userControlImageCollectionEventSeries);
            //        if (this.listBoxCollectionEventSeriesImages.Items.Count > 0) this.listBoxCollectionEventSeriesImages.SelectedIndex = 0;
            //        else
            //        {
            //            this.listBoxCollectionEventSeriesImages.SelectedIndex = -1;
            //            this.userControlImageCollectionEventSeries.ImagePath = "";
            //        }
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message);
            //}
        }

        private void toolStripButtonCollectionEventSeriesImageDescription_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormFunctions.setImageDescription((System.Windows.Forms.BindingSource)this._Source);
        }

        #endregion

        //#region Image List

        //private void listBoxCollectionEventSeriesImages_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int i = this.listBoxCollectionEventSeriesImages.SelectedIndex;
        //        if (this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage.Rows.Count == 0) return;
        //        for (int p = 0; p <= i; p++)
        //        {
        //            if (this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage.Rows[p].RowState == System.Data.DataRowState.Deleted) i++;
        //        }
        //        if (this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage.Rows.Count > i)
        //        {
        //            this.setAvailability(true);
        //            System.Data.DataRow r = this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage.Rows[i];
        //            this.userControlImageCollectionEventSeries.ImagePath = r["URI"].ToString();

        //            string XML = r["Description"].ToString();
        //            this.userControlXMLTreeExif.setToDisplayOnly();
        //            this.userControlXMLTreeExif.XML = XML;

        //            // Rotate if EXIF File contains info about orientiation of image
        //            try
        //            {
        //                if (this.userControlImageCollectionEventSeries.AutorotationEnabled && this.userControlImageCollectionEventSeries.Autorotate)
        //                {
        //                    System.Drawing.RotateFlipType Rotate = DiversityWorkbench.Forms.FormFunctions.ExifRotationInfo(XML);
        //                    if (Rotate != RotateFlipType.RotateNoneFlipNone)
        //                        this.userControlImageCollectionEventSeries.RotateImage(Rotate);
        //                }
        //            }
        //            catch (System.Exception ex)
        //            {
        //                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //            }

        //            this._Source.Position = i;

        //            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
        //            string Restriction = "(Code LIKE 'audio%' OR Code LIKE 'video%')";
        //            if (this.userControlImageCollectionEventSeries.MediumType == DiversityWorkbench.Forms.FormFunctions.Medium.Image)
        //                Restriction = "NOT " + Restriction;
        //            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxCollectionEventSeriesImageType, "CollEventSeriesImageType_Enum", con, true, true, true, Restriction);
        //        }
        //        else
        //            this.setAvailability(false);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        System.Windows.Forms.MessageBox.Show(ex.Message);
        //    }
        //}

        //private void listBoxCollectionEventSeriesImages_MeasureItem(object sender, MeasureItemEventArgs e)
        //{
        //    e.ItemHeight = this.imageListCollectionEventSeries.ImageSize.Height;
        //    e.ItemWidth = this.imageListCollectionEventSeries.ImageSize.Width;
        //}

        //private void listBoxCollectionEventSeriesImages_DrawItem(object sender, DrawItemEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Index > -1)
        //            this.imageListCollectionEventSeries.Draw(e.Graphics, e.Bounds.X, e.Bounds.Y, 50, 50, e.Index);
        //    }
        //    catch { }
        //}

        //private void toolStripButtonWebView_Click(object sender, EventArgs e)
        //{
        //    DiversityWorkbench.Settings.WebViewUsage(!DiversityWorkbench.Settings.UseWebView, this.toolStripButtonWebView);
        //}

        //#endregion

        //#region Withholding

        //private void comboBoxEventSeriesImageWithholdingReason_DropDown(object sender, EventArgs e)
        //{
        //    string SQL = "SELECT DISTINCT DataWithholdingReason FROM CollectionEventSeriesImage ORDER BY DataWithholdingReason";
        //    System.Data.DataTable dt = new DataTable();
        //    try
        //    {
        //        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //        ad.Fill(dt);
        //        this.comboBoxEventSeriesImageWithholdingReason.DataSource = dt;
        //        this.comboBoxEventSeriesImageWithholdingReason.DisplayMember = "DataWithholdingReason";
        //        this.comboBoxEventSeriesImageWithholdingReason.ValueMember = "DataWithholdingReason";
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void comboBoxEventSeriesImageWithholdingReason_TextChanged(object sender, EventArgs e)
        //{
        //    this.SetDataWithholdingControl(this.comboBoxEventSeriesImageWithholdingReason, this.pictureBoxEventSeriesImageWithholdingReason);
        //}

        //#endregion

        #endregion


        #region Descriptor

        //private System.Windows.Forms.BindingSource _bindingSourceDescriptor;
        //private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterDescriptor;
        //private System.Data.DataSet _dataSetDescriptor;
        //private System.Data.DataTable _dtDescriptor;

        //private System.Data.DataTable dtDescriptor
        //{
        //    get
        //    {
        //        try
        //        {
        //            if (_dtDescriptor == null)
        //            {
        //                string SQL = "SELECT SeriesID, DescriptorID, Descriptor, URL, DescriptorType " +
        //                    "FROM CollectionEventSeriesDescriptor " +
        //                    "WHERE 1 = 0";
        //                this._sqlDataAdapterDescriptor = new Microsoft.Data.SqlClient.SqlDataAdapter();
        //                _dtDescriptor = new DataTable("CollectionEventSeriesDescriptor");
        //                this.FormFunctions.initSqlAdapter(ref _sqlDataAdapterDescriptor, SQL, _dtDescriptor);
        //            }
        //        }
        //        catch (System.Exception ex)
        //        {
        //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //        }
        //        return _dtDescriptor;
        //    }
        //}

        //private System.Data.DataSet dataSetDescriptor
        //{
        //    get
        //    {
        //        try
        //        {
        //            if (_dataSetDescriptor == null)
        //            {
        //                this._dataSetDescriptor = new DataSet("Descriptor");
        //                this._dataSetDescriptor.Tables.Add(this.dtDescriptor);
        //            }
        //        }
        //        catch (System.Exception ex)
        //        {
        //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //        }
        //        return _dataSetDescriptor;
        //    }
        //}



        private void initDescriptorControls()
        {
            try
            {
                //if (_bindingSourceDescriptor == null)
                //    _bindingSourceDescriptor = new BindingSource(this.dataSetDescriptor, "CollectionEventSeriesDescriptor");

                this.textBoxDescriptor.DataBindings.Clear();
                this.textBoxDescriptor.DataBindings.Add(new Binding("Text", _Source, "Descriptor", true));
                this.textBoxDescriptorURL.DataBindings.Clear();
                this.textBoxDescriptorURL.DataBindings.Add(new Binding("Text", _Source, "URL", true));

                DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryDescriptorAgent, _Source, A, "CollectionEventSeriesDescriptor", "Descriptor", "URL");

                DiversityWorkbench.CollectionSpecimen C = new DiversityWorkbench.CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryDescriptorCollection, _Source, C, "CollectionEventSeriesDescriptor", "Descriptor", "URL");

                DiversityWorkbench.Gazetteer G = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryDescriptorGazetteer, _Source, G, "CollectionEventSeriesDescriptor", "Descriptor", "URL");

                DiversityWorkbench.SamplingPlot SP = new DiversityWorkbench.SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryDescriptorPlots, _Source, SP, "CollectionEventSeriesDescriptor", "Descriptor", "URL");

                DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryDescriptorProject, _Source, P, "CollectionEventSeriesDescriptor", "Descriptor", "URL");

                DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryDescriptorTaxa, _Source, T, "CollectionEventSeriesDescriptor", "Descriptor", "URL");

                DiversityWorkbench.Reference R = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryDescriptorReference, _Source, R, "CollectionEventSeriesDescriptor", "Descriptor", "URL");

                DiversityWorkbench.ScientificTerm ST = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryDescriptorTerms, _Source, ST, "CollectionEventSeriesDescriptor", "Descriptor", "URL");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }




        //private void initDescriptors(System.Data.DataRowView R)
        //{
        //    try
        //    {
        //        this.initDescriptorTypeImages();
        //        int id;
        //        if (int.TryParse(R["SeriesID"].ToString(), out id))
        //        {
        //            this.SeriesID = id;
        //            string SQL = "SELECT SeriesID, DescriptorID, Descriptor, URL, DescriptorType " +
        //                "FROM CollectionEventSeriesDescriptor " +
        //                "WHERE SeriesID = " + SeriesID.ToString();
        //            this._sqlDataAdapterDescriptor = new Microsoft.Data.SqlClient.SqlDataAdapter();
        //            this.FormFunctions.initSqlAdapter(ref _sqlDataAdapterDescriptor, SQL, dtDescriptor);
        //            this.buildDescriptorTree();
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        private int _SeriesID;
        private int SeriesID
        {
            set
            {
                _SeriesID = value;
            }
            get
            {
                return _SeriesID;
            }
        }
        private void setDescriptorControls()
        {
            try
            {
                if (this._Source.Current != null && this.treeViewDescriptor.SelectedNode != null && this.treeViewDescriptor.SelectedNode.Tag != null && this.treeViewDescriptor.SelectedNode.Tag.GetType() == typeof(System.Data.DataRow))
                {
                    System.Data.DataRow R = (System.Data.DataRow)this.treeViewDescriptor.SelectedNode.Tag;
                    string Type = R["DescriptorType"].ToString();
                    DiversityWorkbench.WorkbenchUnit.ModuleType DescriptorModule = this.DescriptorModule(Type);
                    switch (DescriptorModule)
                    {
                        case DiversityWorkbench.WorkbenchUnit.ModuleType.None:
                            this.setDescritorDefaultControlsVisibility(true);
                            break;
                        default:
                            this.setDescritorDefaultControlsVisibility(false);
                            break;
                    }
                    this.setDescriptorModuleControls(DescriptorModule);
                }
                else
                {
                    this.setDescritorDefaultControlsVisibility(false);
                    this.setDescriptorModuleControls(DiversityWorkbench.WorkbenchUnit.ModuleType.None);
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private DiversityWorkbench.WorkbenchUnit.ModuleType DescriptorModuleType(string Module)
        {
            DiversityWorkbench.WorkbenchUnit.ModuleType moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.None;
            try
            {
                switch (Module)
                {
                    case "DiversityAgents":
                        moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.Agents;
                        break;
                    case "DiversityCollection":
                        moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.Collection;
                        break;
                    case "DiversityGazetteer":
                        moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.Gazetteer;
                        break;
                    case "DiversityProjects":
                        moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.Projects;
                        break;
                    case "DiversityReferences":
                        moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.References;
                        break;
                    case "DiversitySamplingPlots":
                        moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.SamplingPlots;
                        break;
                    case "DiversityScientificTerms":
                        moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.ScientificTerms;
                        break;
                    case "DiversityTaxonNames":
                        moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.TaxonNames;
                        break;
                    default:
                        moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.None;
                        break;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return moduleType;
        }

        private void setDescriptorModuleControls(DiversityWorkbench.WorkbenchUnit.ModuleType moduleType)
        {
            try
            {
                if (moduleType == DiversityWorkbench.WorkbenchUnit.ModuleType.None)
                {
                    this.panelDescriptorModules.Visible = false;
                }
                else
                {
                    this.panelDescriptorModules.Visible = true;
                    this.userControlModuleRelatedEntryDescriptorAgent.Visible = false;
                    this.userControlModuleRelatedEntryDescriptorCollection.Visible = false;
                    this.userControlModuleRelatedEntryDescriptorGazetteer.Visible = false;
                    this.userControlModuleRelatedEntryDescriptorProject.Visible = false;
                    this.userControlModuleRelatedEntryDescriptorReference.Visible = false;
                    this.userControlModuleRelatedEntryDescriptorPlots.Visible = false;
                    this.userControlModuleRelatedEntryDescriptorTerms.Visible = false;
                    this.userControlModuleRelatedEntryDescriptorTaxa.Visible = false;
                    switch (moduleType)
                    {
                        case DiversityWorkbench.WorkbenchUnit.ModuleType.Agents:
                            this.userControlModuleRelatedEntryDescriptorAgent.Visible = true;
                            this.userControlModuleRelatedEntryDescriptorAgent.Dock = DockStyle.Fill;
                            break;
                        case DiversityWorkbench.WorkbenchUnit.ModuleType.Collection:
                            this.userControlModuleRelatedEntryDescriptorCollection.Visible = true;
                            this.userControlModuleRelatedEntryDescriptorCollection.Dock = DockStyle.Fill;
                            break;
                        case DiversityWorkbench.WorkbenchUnit.ModuleType.Gazetteer:
                            this.userControlModuleRelatedEntryDescriptorGazetteer.Visible = true;
                            this.userControlModuleRelatedEntryDescriptorGazetteer.Dock = DockStyle.Fill;
                            break;
                        case DiversityWorkbench.WorkbenchUnit.ModuleType.Projects:
                            this.userControlModuleRelatedEntryDescriptorProject.Visible = true;
                            this.userControlModuleRelatedEntryDescriptorProject.Dock = DockStyle.Fill;
                            break;
                        case DiversityWorkbench.WorkbenchUnit.ModuleType.References:
                            this.userControlModuleRelatedEntryDescriptorReference.Visible = true;
                            this.userControlModuleRelatedEntryDescriptorReference.Dock = DockStyle.Fill;
                            break;
                        case DiversityWorkbench.WorkbenchUnit.ModuleType.SamplingPlots:
                            this.userControlModuleRelatedEntryDescriptorPlots.Visible = true;
                            this.userControlModuleRelatedEntryDescriptorPlots.Dock = DockStyle.Fill;
                            break;
                        case DiversityWorkbench.WorkbenchUnit.ModuleType.ScientificTerms:
                            this.userControlModuleRelatedEntryDescriptorTerms.Visible = true;
                            this.userControlModuleRelatedEntryDescriptorTerms.Dock = DockStyle.Fill;
                            break;
                        case DiversityWorkbench.WorkbenchUnit.ModuleType.TaxonNames:
                            this.userControlModuleRelatedEntryDescriptorTaxa.Visible = true;
                            this.userControlModuleRelatedEntryDescriptorTaxa.Dock = DockStyle.Fill;
                            break;
                        default:
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setDescritorDefaultControlsVisibility(bool isVisible)
        {
            this.textBoxDescriptor.Visible = isVisible;
            this.labelDescriptorURL.Visible = isVisible;
            this.textBoxDescriptorURL.Visible = isVisible;
            this.buttonDescriptorURL.Visible = isVisible;
        }

        private DiversityWorkbench.WorkbenchUnit.ModuleType DescriptorModule(string Type)
        {
            try
            {
                System.Data.DataRow[] RR = this.EventSeriesDescriptorType_Enum().Select("Code = '" + Type + "'");
                if (RR.Length > 0)
                {
                    DiversityWorkbench.WorkbenchUnit.ModuleType Module = this.DescriptorModuleType(RR[0]["ModuleName"].ToString());
                    return Module;
                }
                else
                    return DiversityWorkbench.WorkbenchUnit.ModuleType.None;
            }
            catch (System.Exception ex)
            {
                return DiversityWorkbench.WorkbenchUnit.ModuleType.None;
            }

        }

        private System.Data.DataTable _EventSeriesDescriptorType_Enum;
        private System.Data.DataTable EventSeriesDescriptorType_Enum()
        {
            if (this._EventSeriesDescriptorType_Enum == null || this._EventSeriesDescriptorType_Enum.Columns.Count == 0)
            {
                this._EventSeriesDescriptorType_Enum = new DataTable();
                string SQL = "SELECT Code, ParentCode, DisplayText, Icon, ModuleName FROM CollEventSeriesDescriptorType_Enum WHERE (DisplayEnable = 1) ORDER BY DisplayText";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._EventSeriesDescriptorType_Enum);
            }
            return this._EventSeriesDescriptorType_Enum;
        }

        #region tree for Descriptor

        private void initDescriptorTypeImages()
        {
            DiversityWorkbench.CollectionSpecimen.DescriptorTypeImageReset();
            if (this.imageListDescriptor.Images.Count > 10)
            {
                for (int i = this.imageListDescriptor.Images.Count; i > 10; i--)
                    this.imageListDescriptor.Images.RemoveAt(i - 1);
            }
            int LengthOfImageList = this.imageListDescriptor.Images.Count;
            foreach (System.Data.DataRow R in this.EventSeriesDescriptorType_Enum().Rows)
            {
                if (!R["Icon"].Equals(System.DBNull.Value))
                {
                    System.Byte[] B = (System.Byte[])R["Icon"];
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(B);
                    System.Drawing.Image I = System.Drawing.Image.FromStream(ms);
                    System.Drawing.Bitmap BM = new Bitmap(I, 16, 16);
                    System.Drawing.Bitmap BG = DiversityWorkbench.Forms.FormFunctions.MakeGrayscale3(BM);
                    BM.MakeTransparent();
                    DiversityWorkbench.CollectionSpecimen.DescriptorTypeImageAdd(LengthOfImageList, R["Code"].ToString(), BM);
                }
            }
            if (DiversityWorkbench.CollectionSpecimen.DescriptorTypeImageDict.Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, System.Drawing.Image> kv in  DiversityWorkbench.CollectionSpecimen.DescriptorTypeImageDict) // DescriptorImages()) //
                {
                    if (!this.imageListDescriptor.Images.ContainsKey(kv.Key))
                        this.imageListDescriptor.Images.Add(kv.Key, kv.Value);
                }
            }
        }


        private void buildDescriptorTree()
        {
            try
            {
                this.treeViewDescriptor.Nodes.Clear();
                // Getting the basic tree according to the types of the current Descriptors
                string SQL = "declare @Descriptors Table (Code nvarchar(50), ParentCode nvarchar(50)); " +
                    "insert into @Descriptors(Code, ParentCode) " +
                    "select distinct T.code, T.ParentCode from CollectionEventSeriesDescriptor K " +
                    "inner join[dbo].[CollEventSeriesDescriptorType_Enum] T on K.DescriptorType = T.Code " +
                    "where K.SeriesID = " + this._SeriesID.ToString() +
                    "; declare @i int " +
                    "set @i = (select count(*) from @Descriptors c where c.ParentCode not in (select Code from @Descriptors) ); " +
                    "while @i > 0 " +
                    "begin " +
                    " insert into @Descriptors(Code, ParentCode) " +
                    " select distinct T.code, T.ParentCode from @Descriptors K " +
                    " inner join[dbo].[CollEventSeriesDescriptorType_Enum] T on K.ParentCode = T.Code " +
                    " where K.ParentCode not in (select Code from @Descriptors) " +
                    " set @i = (select count(*) from @Descriptors c where c.ParentCode not in (select Code from @Descriptors) ) " +
                    "end; " +
                    "select * from @Descriptors";
                System.Data.DataTable dtTree = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTree);
                System.Data.DataRow[] RR = dtTree.Select("ParentCode IS NULL");
                foreach (System.Data.DataRow R in RR)
                {
                    System.Windows.Forms.TreeNode N = new TreeNode(R["Code"].ToString());
                    N.ForeColor = System.Drawing.Color.Gray;
                    this.treeViewDescriptor.Nodes.Add(N);
                    this.buildDescriptorTreeAddData(N);
                    this.buildDescriptorTreeAddChild(N, dtTree);
                }
                this.treeViewDescriptor.ExpandAll();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buildDescriptorTreeAddChild(System.Windows.Forms.TreeNode NParent, System.Data.DataTable dtTree)
        {
            try
            {

                System.Data.DataRow[] RR = dtTree.Select("ParentCode = '" + NParent.Text + "'");
                foreach (System.Data.DataRow R in RR)
                {
                    System.Windows.Forms.TreeNode N = new TreeNode(R["Code"].ToString());
                    N.ForeColor = System.Drawing.Color.Gray;
                    NParent.Nodes.Add(N);
                    this.buildDescriptorTreeAddData(N);
                    this.buildDescriptorTreeAddChild(N, dtTree);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private System.Collections.Generic.Dictionary<string, System.Drawing.Image> DescriptorImages()
        {
            System.Collections.Generic.Dictionary<string, System.Drawing.Image> images = new Dictionary<string, Image>();
            try
            {
                images.Add("", this.imageListDescriptor.Images[0]);
                images.Add("Descriptor", this.imageListDescriptor.Images[1]);
                images.Add("DiversityAgents", this.imageListDescriptor.Images[2]);
                images.Add("DiversityCollection", this.imageListDescriptor.Images[3]);
                images.Add("DiversityGazetteer", this.imageListDescriptor.Images[4]);
                images.Add("DiversityProjects", this.imageListDescriptor.Images[5]);
                images.Add("DiversityReferences", this.imageListDescriptor.Images[6]);
                images.Add("DiversitySamplingPlots", this.imageListDescriptor.Images[7]);
                images.Add("DiversityScientificTerms", this.imageListDescriptor.Images[8]);
                images.Add("DiversityTaxonNames", this.imageListDescriptor.Images[9]);
                foreach (System.Collections.Generic.KeyValuePair<string, System.Drawing.Image> kv in DiversityWorkbench.CollectionSpecimen.DescriptorTypeImageDict)
                {
                    if (!images.ContainsKey(kv.Key))
                        images.Add(kv.Key, kv.Value);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return images;
        }

        private void buildDescriptorTreeAddData(System.Windows.Forms.TreeNode NParent)
        {
            try
            {
                string SQL = "select SeriesID, DescriptorID, Descriptor, URL, DescriptorType from CollectionEventSeriesDescriptor K " +
                        "where K.SeriesID = " + this.SeriesID.ToString() + " and K.DescriptorType = '" + NParent.Text + "'";
                System.Data.DataTable dtDescriptors = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtDescriptors);
                foreach (System.Data.DataRow R in dtDescriptors.Rows)
                {
                    System.Windows.Forms.TreeNode N = new TreeNode(R["Descriptor"].ToString());
                    N.Tag = R;
                    string Type = R["DescriptorType"].ToString();
                    if (DiversityWorkbench.CollectionSpecimen.DescriptorTypeDict.ContainsKey(Type))
                    {
                        N.ImageIndex = DiversityWorkbench.CollectionSpecimen.DescriptorTypeDict[Type];
                        N.SelectedImageIndex = N.ImageIndex;
                    }
                    else
                    {

                        DiversityWorkbench.WorkbenchUnit.ModuleType Module = this.DescriptorModule(Type);
                        switch (Module)
                        {
                            case DiversityWorkbench.WorkbenchUnit.ModuleType.Agents:
                                N.ImageIndex = 2;
                                break;
                            case DiversityWorkbench.WorkbenchUnit.ModuleType.Collection:
                                N.ImageIndex = 3;
                                break;
                            case DiversityWorkbench.WorkbenchUnit.ModuleType.Gazetteer:
                                N.ImageIndex = 4;
                                break;
                            case DiversityWorkbench.WorkbenchUnit.ModuleType.Projects:
                                N.ImageIndex = 5;
                                break;
                            case DiversityWorkbench.WorkbenchUnit.ModuleType.References:
                                N.ImageIndex = 6;
                                break;
                            case DiversityWorkbench.WorkbenchUnit.ModuleType.SamplingPlots:
                                N.ImageIndex = 7;
                                break;
                            case DiversityWorkbench.WorkbenchUnit.ModuleType.ScientificTerms:
                                N.ImageIndex = 8;
                                break;
                            case DiversityWorkbench.WorkbenchUnit.ModuleType.TaxonNames:
                                N.ImageIndex = 9;
                                break;
                            default:
                                if (DiversityWorkbench.CollectionSpecimen.DescriptorTypeDict.ContainsKey(Type))
                                {
                                    N.ImageIndex = DiversityWorkbench.CollectionSpecimen.DescriptorTypeDict[Type];
                                }
                                else
                                    N.ImageIndex = 1;
                                break;
                        }
                        N.SelectedImageIndex = N.ImageIndex;
                    }
                    NParent.Nodes.Add(N);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void treeViewDescriptor_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (this.treeViewDescriptor.SelectedNode == null || this.treeViewDescriptor.SelectedNode.Tag == null)
                {
                    this.setDescritorDefaultControlsVisibility(false);
                    this.setDescriptorModuleControls(DiversityWorkbench.WorkbenchUnit.ModuleType.None);
                }
                else
                {
                    System.Data.DataRow R = (System.Data.DataRow)this.treeViewDescriptor.SelectedNode.Tag;
                    System.Data.DataTable dtDescriptor = this._iMainForm.DataSetCollectionEventSeries().Tables["CollectionEventSeriesDescriptor"];
                    for (int i = 0; i < dtDescriptor.Rows.Count; i++)
                    {
                        if (R["DescriptorID"].ToString() == dtDescriptor.Rows[i]["DescriptorID"].ToString())
                        {
                            this._Source.Position = i;
                            break;
                        }
                    }
                    this.setDescriptorControls();
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion
        private void buttonDescriptorURL_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxDescriptorURL.Text);
            f.Width = this._iMainForm.FormWidth() - 20;
            f.Height = this._iMainForm.FormHeight() - 20;
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                this.textBoxDescriptorURL.Text = f.URL;
            }
        }

        private void toolStripButtonDescriptorAdd_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataTable dtKeyType = new DataTable();
                string SQL = "SELECT Code, ParentCode, DisplayText FROM CollEventSeriesDescriptorType_Enum WHERE (DisplayEnable = 1)";
                string Message = "";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtKeyType, ref Message);
                if (dtKeyType.Rows.Count == 0)
                {
                    bool OK = this.FormFunctions.getObjectPermissions("CollEventSeriesDescriptorType_Enum", "Insert");
                    if (OK)
                    {
                        if (System.Windows.Forms.MessageBox.Show("No types have been defined so far. Would you like to add descriptor types?", "No descriptor types", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            this.EditDescriptorTypes();
                    }
                    else
                        System.Windows.Forms.MessageBox.Show("No descriptor types have been defined so far. Please turn to your administrator", "No descriptor types", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    DiversityWorkbench.Forms.FormGetItemFromHierarchy f = new DiversityWorkbench.Forms.FormGetItemFromHierarchy(dtKeyType, "Code", "ParentCode", "DisplayText", "DisplayText", "Code", "Please select a type", "Type of the Descriptor", false, true);
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        string Type = f.SelectedValue;
                        System.Data.DataTable dtKeys = new DataTable();
                        SQL = "SELECT DISTINCT Descriptor " +
                            "FROM CollectionEventSeriesDescriptor " +
                            "WHERE (DescriptorType = N'" + Type + "') AND (Descriptor <> N'') " +
                            "ORDER BY Descriptor";
                        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtKeys, ref Message);
                        DiversityWorkbench.Forms.FormGetStringFromList fKey = new DiversityWorkbench.Forms.FormGetStringFromList(dtKeys, "Please enter or select the Descriptor", false, true);
                        fKey.ShowDialog();
                        System.Data.DataTable dtDescriptor = this._iMainForm.DataSetCollectionEventSeries().Tables["CollectionEventSeriesDescriptor"];
                        if (fKey.DialogResult == DialogResult.OK && fKey.String.Length > 0)
                        {
                            DiversityCollection.Datasets.DataSetCollectionEventSeries.CollectionEventSeriesDescriptorRow R = this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesDescriptor.NewCollectionEventSeriesDescriptorRow();
                            R["SeriesID"] = this.SeriesID;
                            R["Descriptor"] = fKey.String;
                            R["DescriptorType"] = Type;
                            this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesDescriptor.Rows.Add(R);
                            this._iMainForm.saveSpecimen();
                            this.buildDescriptorTree();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonDescriptorDelete_Click(object sender, EventArgs e)
        {
            if (this.treeViewDescriptor.SelectedNode.Tag != null)
            {
                //System.Windows.Forms.BindingManagerBase BMB = this.BindingContext[this._iMainForm.DataSetCollectionEventSeries(), "CollectionEventSeriesDescriptor"];
                int p;// = BMB.Position;
                int i = 0;
                System.Data.DataRow dataRow = (System.Data.DataRow)this.treeViewDescriptor.SelectedNode.Tag;
                foreach (System.Data.DataRow R in this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesDescriptor.Rows)
                {
                    if (R["DescriptorID"].ToString() == dataRow["DescriptorID"].ToString())
                        break;
                    i++;
                }
                p = i;
                System.Data.DataRow r = this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesDescriptor.Rows[p];
                if (r.RowState != System.Data.DataRowState.Deleted)
                {
                    this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesDescriptor.Rows[p].Delete();
                    System.Windows.Forms.TreeNode N =  this._iMainForm.SelectedUnitHierarchyNode();
                    this._iMainForm.saveSpecimen();
                    this.buildDescriptorTree();
                }
            }
        }

        #region Types
        private void toolStripButtonDescriptorEditTypes_Click(object sender, EventArgs e)
        {
            this.EditDescriptorTypes();
        }

        private void EditDescriptorTypes()
        {
            try
            {
                DiversityWorkbench.Forms.FormEnumAdministration f = new DiversityWorkbench.Forms.FormEnumAdministration(
                    DiversityCollection.Resource.KeyBlue,
                    "CollEventSeriesDescriptorType_Enum",
                    "Administration of Descriptor types",
                    "", this.DescriptorImages());
                f.HierarchyChangesEnabled = true;
                f.ShowImage = true;
                f.setHelp("Descriptor");
                string SQL = "SELECT DescriptorType " +
                    "FROM CollectionEventSeriesDescriptor " +
                    "WHERE(URL <> '') " +
                    "GROUP BY DescriptorType " +
                    "HAVING(DescriptorType <> N'')";

                System.Data.DataTable dt = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                if (dt.Rows.Count > 0)
                {
                    System.Collections.Generic.List<string> Blocked = new List<string>();
                    foreach (System.Data.DataRow R in dt.Rows)
                        Blocked.Add(R[0].ToString());
                    f.setBlockedModuleTypes(Blocked);
                }

                SQL = "SELECT DescriptorType " +
                    "FROM CollectionEventSeriesDescriptor " +
                    "GROUP BY DescriptorType " +
                    "HAVING(DescriptorType <> N'')";
                dt.Clear();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                if (dt.Rows.Count > 0)
                {
                    System.Collections.Generic.List<string> Blocked = new List<string>();
                    foreach (System.Data.DataRow R in dt.Rows)
                        Blocked.Add(R[0].ToString());
                    f.setDoNotDelete(Blocked);
                }
                f.ShowDialog();
                if (f.DataHaveBeenChanged)
                {
                    this._EventSeriesDescriptorType_Enum = null;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        #endregion

        #endregion



    }
}
