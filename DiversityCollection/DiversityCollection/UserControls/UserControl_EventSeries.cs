using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WpfSamplingPlotPage;


namespace DiversityCollection.UserControls
{
    public partial class UserControl_EventSeries : UserControl__Data
    {
        #region Construction

        //public UserControl_EventSeries()
        //{
        //    InitializeComponent();
        //}

        public UserControl_EventSeries(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this._Source = Source;
            this._HelpNamespace = HelpNamespace;
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }

        #endregion

        #region public interface

        public override void SetPosition(int Position)
        {
            this._Source.CurrencyManager.Position = Position;
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            this.textBoxEventSeriesID.Text = R["SeriesID"].ToString();
            this.setGeographyText();
            this.setAvailability();
            this.setDate();
            this.setDate(false);
            //this.initDescriptors(R);
        }

        public override void SetPositionByID(int ID, int Position)
        {
            int i = Position;
            // try to find position
            try
            {
                i = 0;
                System.Data.DataSet ds = (System.Data.DataSet)this._Source.DataSource;
                System.Data.DataTable dt = ds.Tables["CollectionEventSeries"];
                foreach(System.Data.DataRow r in dt.Rows)
                {
                    int _ID;
                    if (int.TryParse(r[0].ToString(), out _ID) && _ID == ID)
                        break;
                    i++;
                }
                Position = i;
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this._Source.CurrencyManager.Position = Position;
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            this.textBoxEventSeriesID.Text = R["SeriesID"].ToString();
            this.setGeographyText();
            this.setAvailability();
            this.setDate();
            this.setDate(false);
            //this.initDescriptors(R);
        }

        public System.Windows.Forms.TableLayoutPanel TableLayoutPanelOverviewEventSeries { get { return this.tableLayoutPanelOverviewEventSeries; } }

        #endregion

        #region Control

        private void initControl()
        {
            this.textBoxOverviewEventSeriesCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "SeriesCode", true));
            this.textBoxOverviewEventSeriesDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Description", true));
            this.textBoxOverviewEventSeriesNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Notes", true));
            this.textBoxDateSupplement.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "DateSupplement", true));

            //this.textBoxCollectionEventSeriesDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "DateStart", true));
            //this.textBoxCollectionEventSeriesEnd.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "DateEnd", true));
            this.dateTimePickerCollectionEventSeriesStart.DataBindings.Add(new System.Windows.Forms.Binding("Value", this._Source, "DateStart", true));
            this.dateTimePickerCollectionEventSeriesEnd.DataBindings.Add(new System.Windows.Forms.Binding("Value", this._Source, "DateEnd", true));

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);

            //DiversityWorkbench.FormFunctions.setAutoCompletion(this.textBoxOverviewEventSeriesCode);

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();

            //this.initDescriptorControls();
        }

        #region Date
        private void dateTimePickerCollectionEventSeriesStart_DropDown(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            if (!R["DateStart"].Equals(System.DBNull.Value))
            {
                System.DateTime Start;
                if (System.DateTime.TryParse(R["DateStart"].ToString(), out Start))
                    this.dateTimePickerCollectionEventSeriesStart.Value = Start;
            }
        }

        private void dateTimePickerCollectionEventSeriesStart_CloseUp(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            R["DateStart"] = this.dateTimePickerCollectionEventSeriesStart.Value;
            this.setDate();
        }

        private void dateTimePickerCollectionEventSeriesEnd_CloseUp(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            R["DateEnd"] = this.dateTimePickerCollectionEventSeriesEnd.Value;
            this.setDate(false);
        }

        private void dateTimePickerCollectionEventSeriesEnd_DropDown(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            if (!R["DateEnd"].Equals(System.DBNull.Value))
            {
                System.DateTime Start;
                if (System.DateTime.TryParse(R["DateEnd"].ToString(), out Start))
                    this.dateTimePickerCollectionEventSeriesEnd.Value = Start;
            }
        }

        private void buttonCollectionEventSeriesStartDelete_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            R["DateStart"] = System.DBNull.Value;
            this.setDate();
        }

        private void buttonCollectionEventSeriesEndDelete_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            R["DateEnd"] = System.DBNull.Value;
            this.setDate(false);
        }

        private void setDate(bool IsStart = true)
        {
            bool DateIsSet = false;
            try
            {
                if (this._Source.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                    string Column = "DateStart";
                    if (!IsStart)
                        Column = "DateEnd";
                    if (!R[Column].Equals(System.DBNull.Value))
                        DateIsSet = true;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            if (DateIsSet)
            {
                if (IsStart)
                    this.dateTimePickerCollectionEventSeriesStart.CustomFormat = "yyyy-MM-dd HH:mm:ss";
                else
                    this.dateTimePickerCollectionEventSeriesEnd.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            }
            else
            {
                if (IsStart)
                    this.dateTimePickerCollectionEventSeriesStart.CustomFormat = "-";
                else
                    this.dateTimePickerCollectionEventSeriesEnd.CustomFormat = "-";
            }
        }

        #endregion

        #region Geography

        private void buttonEventSeriesSetGeo_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView RA
                    = (System.Data.DataRowView)this._Source.Current;
                if (this._iMainForm.DataSetCollectionSpecimen().CollectionEventSeriesGeo.Rows.Count == 0)
                {
                    DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionEventSeriesGeoRow R = this._iMainForm.DataSetCollectionSpecimen().CollectionEventSeriesGeo.NewCollectionEventSeriesGeoRow();
                    R.SeriesID = (int)this._iMainForm.EventSeriesID();
                    this._iMainForm.DataSetCollectionSpecimen().CollectionEventSeriesGeo.Rows.Add(R);
                }
                if (this._iMainForm.DataSetCollectionSpecimen().CollectionEventSeriesGeo.Rows.Count == 1)
                {
                    if (!this._iMainForm.DataSetCollectionSpecimen().CollectionEventSeriesGeo.Rows[0]["GeographyAsString"].Equals(System.DBNull.Value) || this.textBoxGeography.Text.Length > 0)
                    {
                        string Geography = this._iMainForm.DataSetCollectionSpecimen().CollectionEventSeriesGeo.Rows[0]["GeographyAsString"].ToString();
                        if (Geography.Length == 0)
                            Geography = this.textBoxGeography.Text;
                        GeoObject GO = new GeoObject();
                        GO.GeometryData = Geography;
                        GO.FillBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 1, 1));
                        GO.FillTransparency = 50;
                        GO.Identifier = "X";
                        GO.PointSymbolSize = 1;
                        GO.StrokeBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 1, 1));
                        GO.StrokeThickness = 1;
                        GO.StrokeTransparency = 255;
                        GO.DisplayText = "X";
                        System.Collections.Generic.List<GeoObject> L = new List<GeoObject>();
                        L.Add(GO);
                        DiversityWorkbench.Forms.FormGeography f = new DiversityWorkbench.Forms.FormGeography(L);
                        f.ShowDialog();
                        if (f.DialogResult == DialogResult.OK && f.Geography.Length > 0)
                        {
                            string SQL = "Update CollectionEventSeries SET Geography = geography::STGeomFromText('" + f.Geography + "', 4326)"
                               + " WHERE SeriesID = " + this._iMainForm.EventSeriesID().ToString();
                            string Message = "";
                            DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message);
                            if (Message.Length > 0)
                            {
                                System.Windows.Forms.MessageBox.Show(Message);
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("buttonEventSeriesSetGeo_Click(sender, e)", SQL, Message);
                            }
                            this._iMainForm.saveSpecimen(); // this.updateSpecimen();
                        }
                    }
                    else
                    {
                        DiversityWorkbench.Forms.FormGeography f = new DiversityWorkbench.Forms.FormGeography();
                        f.ShowDialog();
                        if (f.DialogResult == DialogResult.OK && f.Geography.Length > 0)
                        {
                            string SQL = "Update CollectionEventSeries SET Geography = geography::STGeomFromText('" + f.Geography + "', 4326)"
                                + " WHERE SeriesID = " + this._iMainForm.EventSeriesID().ToString();
                            string Message = "";
                            DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message);
                            if (Message.Length > 0)
                            {
                                System.Windows.Forms.MessageBox.Show(Message);
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("buttonEventSeriesSetGeo_Click(sender, e)", SQL, Message);
                            }
                            this._iMainForm.saveSpecimen();// this.updateSpecimen();
                        }
                    }
                }
                this.setGeographyText();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setGeographyText()
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            string SQL = "SELECT Geography.ToString() AS Geography " +
                "FROM CollectionEventSeries AS E " +
                "WHERE SeriesID = " + R["SeriesID"].ToString();
            this.textBoxGeography.Text = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
        }

        private void buttonEditGeography_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                string WhereClause = "SeriesID = " + R["SeriesID"].ToString();
                string SQL = "SELECT Geography.ToString() " +
                    "FROM CollectionEventSeries AS E " +
                    "WHERE " + WhereClause;
                string Geography = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                DiversityWorkbench.Geography.FormEditGeography formEditGeography = new DiversityWorkbench.Geography.FormEditGeography("Edit the geography of the event series", "CollectionEventSeries", "Geography", Geography);
                formEditGeography.ShowDialog();
                if (formEditGeography.DialogResult == DialogResult.OK)
                {
                    if (formEditGeography.SaveGeography("CollectionEventSeries", "Geography", WhereClause))
                    {
                        this.setGeographyText();
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #endregion


        //#region Descriptor

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



        //private void initDescriptorControls()
        //{
        //    try
        //    {
        //        if (_bindingSourceDescriptor == null)
        //            _bindingSourceDescriptor = new BindingSource(this.dataSetDescriptor, "CollectionEventSeriesDescriptor");

        //        this.textBoxDescriptor.DataBindings.Clear();
        //        this.textBoxDescriptor.DataBindings.Add(new Binding("Text", _bindingSourceDescriptor, "Descriptor", true));
        //        this.textBoxDescriptorURL.DataBindings.Clear();
        //        this.textBoxDescriptorURL.DataBindings.Add(new Binding("Text", _bindingSourceDescriptor, "URL", true));

        //        DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
        //        this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryDescriptorAgent, _bindingSourceDescriptor, A, "CollectionEventSeriesDescriptor", "Descriptor", "URL");

        //        DiversityWorkbench.CollectionSpecimen C = new DiversityWorkbench.CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
        //        this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryDescriptorCollection, _bindingSourceDescriptor, C, "CollectionEventSeriesDescriptor", "Descriptor", "URL");

        //        DiversityWorkbench.Gazetteer G = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
        //        this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryDescriptorGazetteer, _bindingSourceDescriptor, G, "CollectionEventSeriesDescriptor", "Descriptor", "URL");

        //        DiversityWorkbench.SamplingPlot SP = new DiversityWorkbench.SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
        //        this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryDescriptorPlots, _bindingSourceDescriptor, SP, "CollectionEventSeriesDescriptor", "Descriptor", "URL");

        //        DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
        //        this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryDescriptorProject, _bindingSourceDescriptor, P, "CollectionEventSeriesDescriptor", "Descriptor", "URL");

        //        DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
        //        this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryDescriptorTaxa, _bindingSourceDescriptor, T, "CollectionEventSeriesDescriptor", "Descriptor", "URL");

        //        DiversityWorkbench.Reference R = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
        //        this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryDescriptorReference, _bindingSourceDescriptor, R, "CollectionEventSeriesDescriptor", "Descriptor", "URL");

        //        DiversityWorkbench.ScientificTerm ST = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
        //        this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryDescriptorTerms, _bindingSourceDescriptor, ST, "CollectionEventSeriesDescriptor", "Descriptor", "URL");
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}




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

        //private int _SeriesID;
        //private int SeriesID
        //{
        //    set
        //    {
        //        _SeriesID = value;
        //    }
        //    get 
        //    {
        //        return _SeriesID;
        //    } 
        //}
        //private void setDescriptorControls()
        //{
        //    try
        //    {
        //        if (this._Source.Current != null && this.treeViewDescriptor.SelectedNode != null && this.treeViewDescriptor.SelectedNode.Tag != null && this.treeViewDescriptor.SelectedNode.Tag.GetType() == typeof(System.Data.DataRow))
        //        {
        //            System.Data.DataRow R = (System.Data.DataRow)this.treeViewDescriptor.SelectedNode.Tag;
        //            string Type = R["DescriptorType"].ToString();
        //            DiversityWorkbench.WorkbenchUnit.ModuleType DescriptorModule = this.DescriptorModule(Type);
        //            switch (DescriptorModule)
        //            {
        //                case DiversityWorkbench.WorkbenchUnit.ModuleType.None:
        //                    this.setDescritorDefaultControlsVisibility(true);
        //                    break;
        //                default:
        //                    this.setDescritorDefaultControlsVisibility(false);
        //                    break;
        //            }
        //            this.setDescriptorModuleControls(DescriptorModule);
        //        }
        //        else
        //        {
        //            this.setDescritorDefaultControlsVisibility(false);
        //            this.setDescriptorModuleControls(DiversityWorkbench.WorkbenchUnit.ModuleType.None);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private DiversityWorkbench.WorkbenchUnit.ModuleType DescriptorModuleType(string Module)
        //{
        //    DiversityWorkbench.WorkbenchUnit.ModuleType moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.None;
        //    try
        //    {
        //        switch (Module)
        //        {
        //            case "DiversityAgents":
        //                moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.Agents;
        //                break;
        //            case "DiversityCollection":
        //                moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.Collection;
        //                break;
        //            case "DiversityGazetteer":
        //                moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.Gazetteer;
        //                break;
        //            case "DiversityProjects":
        //                moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.Projects;
        //                break;
        //            case "DiversityReferences":
        //                moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.References;
        //                break;
        //            case "DiversitySamplingPlots":
        //                moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.SamplingPlots;
        //                break;
        //            case "DiversityScientificTerms":
        //                moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.ScientificTerms;
        //                break;
        //            case "DiversityTaxonNames":
        //                moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.TaxonNames;
        //                break;
        //            default:
        //                moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.None;
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return moduleType;
        //}

        //private void setDescriptorModuleControls(DiversityWorkbench.WorkbenchUnit.ModuleType moduleType)
        //{
        //    try
        //    {
        //        if (moduleType == DiversityWorkbench.WorkbenchUnit.ModuleType.None)
        //        {
        //            this.panelDescriptorModules.Visible = false;
        //        }
        //        else
        //        {
        //            this.panelDescriptorModules.Visible = true;
        //            this.userControlModuleRelatedEntryDescriptorAgent.Visible = false;
        //            this.userControlModuleRelatedEntryDescriptorCollection.Visible = false;
        //            this.userControlModuleRelatedEntryDescriptorGazetteer.Visible = false;
        //            this.userControlModuleRelatedEntryDescriptorProject.Visible = false;
        //            this.userControlModuleRelatedEntryDescriptorReference.Visible = false;
        //            this.userControlModuleRelatedEntryDescriptorPlots.Visible = false;
        //            this.userControlModuleRelatedEntryDescriptorTerms.Visible = false;
        //            this.userControlModuleRelatedEntryDescriptorTaxa.Visible = false;
        //            switch (moduleType)
        //            {
        //                case DiversityWorkbench.WorkbenchUnit.ModuleType.Agents:
        //                    this.userControlModuleRelatedEntryDescriptorAgent.Visible = true;
        //                    this.userControlModuleRelatedEntryDescriptorAgent.Dock = DockStyle.Fill;
        //                    break;
        //                case DiversityWorkbench.WorkbenchUnit.ModuleType.Collection:
        //                    this.userControlModuleRelatedEntryDescriptorCollection.Visible = true;
        //                    this.userControlModuleRelatedEntryDescriptorCollection.Dock = DockStyle.Fill;
        //                    break;
        //                case DiversityWorkbench.WorkbenchUnit.ModuleType.Gazetteer:
        //                    this.userControlModuleRelatedEntryDescriptorGazetteer.Visible = true;
        //                    this.userControlModuleRelatedEntryDescriptorGazetteer.Dock = DockStyle.Fill;
        //                    break;
        //                case DiversityWorkbench.WorkbenchUnit.ModuleType.Projects:
        //                    this.userControlModuleRelatedEntryDescriptorProject.Visible = true;
        //                    this.userControlModuleRelatedEntryDescriptorProject.Dock = DockStyle.Fill;
        //                    break;
        //                case DiversityWorkbench.WorkbenchUnit.ModuleType.References:
        //                    this.userControlModuleRelatedEntryDescriptorReference.Visible = true;
        //                    this.userControlModuleRelatedEntryDescriptorReference.Dock = DockStyle.Fill;
        //                    break;
        //                case DiversityWorkbench.WorkbenchUnit.ModuleType.SamplingPlots:
        //                    this.userControlModuleRelatedEntryDescriptorPlots.Visible = true;
        //                    this.userControlModuleRelatedEntryDescriptorPlots.Dock = DockStyle.Fill;
        //                    break;
        //                case DiversityWorkbench.WorkbenchUnit.ModuleType.ScientificTerms:
        //                    this.userControlModuleRelatedEntryDescriptorTerms.Visible = true;
        //                    this.userControlModuleRelatedEntryDescriptorTerms.Dock = DockStyle.Fill;
        //                    break;
        //                case DiversityWorkbench.WorkbenchUnit.ModuleType.TaxonNames:
        //                    this.userControlModuleRelatedEntryDescriptorTaxa.Visible = true;
        //                    this.userControlModuleRelatedEntryDescriptorTaxa.Dock = DockStyle.Fill;
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void setDescritorDefaultControlsVisibility(bool isVisible)
        //{
        //    this.textBoxDescriptor.Visible = isVisible;
        //    this.labelDescriptorURL.Visible = isVisible;
        //    this.textBoxDescriptorURL.Visible = isVisible;
        //    this.buttonDescriptorURL.Visible = isVisible;
        //}

        //private DiversityWorkbench.WorkbenchUnit.ModuleType DescriptorModule(string Type)
        //{
        //    try
        //    {
        //        System.Data.DataRow[] RR = this.EventSeriesDescriptorType_Enum().Select("Code = '" + Type + "'");
        //        if (RR.Length > 0)
        //        {
        //            DiversityWorkbench.WorkbenchUnit.ModuleType Module = this.DescriptorModuleType(RR[0]["ModuleName"].ToString());
        //            return Module;
        //        }
        //        else
        //            return DiversityWorkbench.WorkbenchUnit.ModuleType.None;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return DiversityWorkbench.WorkbenchUnit.ModuleType.None;
        //    }

        //}

        //private System.Data.DataTable _EventSeriesDescriptorType_Enum;
        //private System.Data.DataTable EventSeriesDescriptorType_Enum()
        //{
        //    if (this._EventSeriesDescriptorType_Enum == null || this._EventSeriesDescriptorType_Enum.Columns.Count == 0)
        //    {
        //        this._EventSeriesDescriptorType_Enum = new DataTable();
        //        string SQL = "SELECT Code, ParentCode, DisplayText, Icon, ModuleName FROM CollEventSeriesDescriptorType_Enum WHERE (DisplayEnable = 1) ORDER BY DisplayText";
        //        DiversityWorkbench.FormFunctions.SqlFillTable(SQL, ref this._EventSeriesDescriptorType_Enum);
        //    }
        //    return this._EventSeriesDescriptorType_Enum;
        //}

        //#region tree for Descriptor

        //private void initDescriptorTypeImages()
        //{
        //    DiversityWorkbench.CollectionSpecimen.DescriptorTypeImageReset();
        //    if (this.imageListDescriptor.Images.Count > 10)
        //    {
        //        for (int i = this.imageListDescriptor.Images.Count; i > 10; i--)
        //            this.imageListDescriptor.Images.RemoveAt(i - 1);
        //    }
        //    int LengthOfImageList = this.imageListDescriptor.Images.Count;
        //    foreach (System.Data.DataRow R in this.EventSeriesDescriptorType_Enum().Rows)
        //    {
        //        if (!R["Icon"].Equals(System.DBNull.Value))
        //        {
        //            System.Byte[] B = (System.Byte[])R["Icon"];
        //            System.IO.MemoryStream ms = new System.IO.MemoryStream(B);
        //            System.Drawing.Image I = System.Drawing.Image.FromStream(ms);
        //            System.Drawing.Bitmap BM = new Bitmap(I, 16, 16);
        //            System.Drawing.Bitmap BG = DiversityWorkbench.FormFunctions.MakeGrayscale3(BM);
        //            BM.MakeTransparent();
        //            DiversityWorkbench.CollectionSpecimen.DescriptorTypeImageAdd(LengthOfImageList, R["Code"].ToString(), BM);
        //        }
        //    }
        //    if (DiversityWorkbench.CollectionSpecimen.DescriptorTypeImageDict.Count > 0)
        //    {
        //        foreach (System.Collections.Generic.KeyValuePair<string, System.Drawing.Image> kv in DiversityWorkbench.CollectionSpecimen.DescriptorTypeImageDict)
        //        {
        //            if (!this.imageListDescriptor.Images.ContainsKey(kv.Key))
        //                this.imageListDescriptor.Images.Add(kv.Key, kv.Value);
        //        }
        //    }
        //}


        //private void buildDescriptorTree()
        //{
        //    try
        //    {
        //        this.treeViewDescriptor.Nodes.Clear();
        //        // Getting the basic tree according to the types of the current Descriptors
        //        string SQL = "declare @Descriptors Table (Code nvarchar(50), ParentCode nvarchar(50)); " +
        //            "insert into @Descriptors(Code, ParentCode) " +
        //            "select distinct T.code, T.ParentCode from CollectionEventSeriesDescriptor K " +
        //            "inner join[dbo].[CollEventSeriesDescriptorType_Enum] T on K.DescriptorType = T.Code " +
        //            "where K.SeriesID = " + this.SeriesID.ToString() +
        //            "; declare @i int " +
        //            "set @i = (select count(*) from @Descriptors c where c.ParentCode not in (select Code from @Descriptors) ); " +
        //            "while @i > 0 " +
        //            "begin " +
        //            " insert into @Descriptors(Code, ParentCode) " +
        //            " select distinct T.code, T.ParentCode from @Descriptors K " +
        //            " inner join[dbo].[CollEventSeriesDescriptorType_Enum] T on K.ParentCode = T.Code " +
        //            " where K.ParentCode not in (select Code from @Descriptors) " +
        //            " set @i = (select count(*) from @Descriptors c where c.ParentCode not in (select Code from @Descriptors) ) " +
        //            "end; " +
        //            "select * from @Descriptors";
        //        System.Data.DataTable dtTree = new DataTable();
        //        DiversityWorkbench.FormFunctions.SqlFillTable(SQL, ref dtTree);
        //        System.Data.DataRow[] RR = dtTree.Select("ParentCode IS NULL");
        //        foreach (System.Data.DataRow R in RR)
        //        {
        //            System.Windows.Forms.TreeNode N = new TreeNode(R["Code"].ToString());
        //            N.ForeColor = System.Drawing.Color.Gray;
        //            this.treeViewDescriptor.Nodes.Add(N);
        //            this.buildDescriptorTreeAddData(N);
        //            this.buildDescriptorTreeAddChild(N, dtTree);
        //        }
        //        this.treeViewDescriptor.ExpandAll();
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void buildDescriptorTreeAddChild(System.Windows.Forms.TreeNode NParent, System.Data.DataTable dtTree)
        //{
        //    try
        //    {

        //        System.Data.DataRow[] RR = dtTree.Select("ParentCode = '" + NParent.Text + "'");
        //        foreach (System.Data.DataRow R in RR)
        //        {
        //            System.Windows.Forms.TreeNode N = new TreeNode(R["Code"].ToString());
        //            N.ForeColor = System.Drawing.Color.Gray;
        //            NParent.Nodes.Add(N);
        //            this.buildDescriptorTreeAddData(N);
        //            this.buildDescriptorTreeAddChild(N, dtTree);
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}


        //private System.Collections.Generic.Dictionary<string, System.Drawing.Image> DescriptorImages()
        //{
        //    System.Collections.Generic.Dictionary<string, System.Drawing.Image> images = new Dictionary<string, Image>();
        //    images.Add("", this.imageListDescriptor.Images[0]);
        //    images.Add("Descriptor", this.imageListDescriptor.Images[1]);
        //    images.Add("DiversityAgents", this.imageListDescriptor.Images[2]);
        //    images.Add("DiversityCollection", this.imageListDescriptor.Images[3]);
        //    images.Add("DiversityGazetteer", this.imageListDescriptor.Images[4]);
        //    images.Add("DiversityProjects", this.imageListDescriptor.Images[5]);
        //    images.Add("DiversityReferences", this.imageListDescriptor.Images[6]);
        //    images.Add("DiversitySamplingPlots", this.imageListDescriptor.Images[7]);
        //    images.Add("DiversityScientificTerms", this.imageListDescriptor.Images[8]);
        //    images.Add("DiversityTaxonNames", this.imageListDescriptor.Images[9]);
        //    foreach (System.Collections.Generic.KeyValuePair<string, System.Drawing.Image> kv in DiversityWorkbench.CollectionSpecimen.DescriptorTypeImageDict)
        //    {
        //        if (!images.ContainsKey(kv.Key))
        //            images.Add(kv.Key, kv.Value);
        //    }
        //    return images;
        //}

        //private void buildDescriptorTreeAddData(System.Windows.Forms.TreeNode NParent)
        //{
        //    try
        //    {
        //        string SQL = "select SeriesID, DescriptorID, Descriptor, URL, DescriptorType from CollectionEventSeriesDescriptor K " +
        //                "where K.SeriesID = " + this.SeriesID.ToString() + " and K.DescriptorType = '" + NParent.Text + "'";
        //        System.Data.DataTable dtDescriptors = new DataTable();
        //        DiversityWorkbench.FormFunctions.SqlFillTable(SQL, ref dtDescriptors);
        //        foreach (System.Data.DataRow R in dtDescriptors.Rows)
        //        {
        //            System.Windows.Forms.TreeNode N = new TreeNode(R["Descriptor"].ToString());
        //            N.Tag = R;
        //            string Type = R["DescriptorType"].ToString();
        //            if (DiversityWorkbench.CollectionSpecimen.DescriptorTypeDict.ContainsKey(Type))
        //            {
        //                N.ImageIndex = DiversityWorkbench.CollectionSpecimen.DescriptorTypeDict[Type];
        //                N.SelectedImageIndex = N.ImageIndex;
        //            }
        //            else
        //            {

        //                DiversityWorkbench.WorkbenchUnit.ModuleType Module = this.DescriptorModule(Type);
        //                switch (Module)
        //                {
        //                    case DiversityWorkbench.WorkbenchUnit.ModuleType.Agents:
        //                        N.ImageIndex = 2;
        //                        break;
        //                    case DiversityWorkbench.WorkbenchUnit.ModuleType.Collection:
        //                        N.ImageIndex = 3;
        //                        break;
        //                    case DiversityWorkbench.WorkbenchUnit.ModuleType.Gazetteer:
        //                        N.ImageIndex = 4;
        //                        break;
        //                    case DiversityWorkbench.WorkbenchUnit.ModuleType.Projects:
        //                        N.ImageIndex = 5;
        //                        break;
        //                    case DiversityWorkbench.WorkbenchUnit.ModuleType.References:
        //                        N.ImageIndex = 6;
        //                        break;
        //                    case DiversityWorkbench.WorkbenchUnit.ModuleType.SamplingPlots:
        //                        N.ImageIndex = 7;
        //                        break;
        //                    case DiversityWorkbench.WorkbenchUnit.ModuleType.ScientificTerms:
        //                        N.ImageIndex = 8;
        //                        break;
        //                    case DiversityWorkbench.WorkbenchUnit.ModuleType.TaxonNames:
        //                        N.ImageIndex = 9;
        //                        break;
        //                    default:
        //                        if (DiversityWorkbench.CollectionSpecimen.DescriptorTypeDict.ContainsKey(Type))
        //                        {
        //                            N.ImageIndex = DiversityWorkbench.CollectionSpecimen.DescriptorTypeDict[Type];
        //                        }
        //                        else
        //                            N.ImageIndex = 1;
        //                        break;
        //                }
        //                N.SelectedImageIndex = N.ImageIndex;
        //            }
        //            NParent.Nodes.Add(N);
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void treeViewDescriptor_AfterSelect(object sender, TreeViewEventArgs e)
        //{
        //    try
        //    {
        //        if (this.treeViewDescriptor.SelectedNode.Tag == null)
        //        {
        //            this.setDescritorDefaultControlsVisibility(false);
        //            this.setDescriptorModuleControls(DiversityWorkbench.WorkbenchUnit.ModuleType.None);
        //        }
        //        else
        //        {
        //            System.Data.DataRow R = (System.Data.DataRow)this.treeViewDescriptor.SelectedNode.Tag;
        //            for (int i = 0; i < dtDescriptor.Rows.Count; i++)
        //            {
        //                if (R["DescriptorID"].ToString() == dtDescriptor.Rows[i]["DescriptorID"].ToString())
        //                {
        //                    this._bindingSourceDescriptor.Position = i;
        //                    break;
        //                }
        //            }
        //            this.setDescriptorControls();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //#endregion
        //private void buttonDescriptorURL_Click(object sender, EventArgs e)
        //{
        //    DiversityWorkbench.FormWebBrowser f = new DiversityWorkbench.FormWebBrowser(this.textBoxDescriptorURL.Text);
        //    f.Width = this.Width - 20;
        //    f.Height = this.Height - 20;
        //    f.ShowDialog();
        //    if (f.DialogResult == DialogResult.OK)
        //    {
        //        this.textBoxDescriptorURL.Text = f.URL;
        //    }
        //}

        //private void toolStripButtonDescriptorAdd_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        System.Data.DataTable dtKeyType = new DataTable();
        //        string SQL = "SELECT Code, ParentCode, DisplayText FROM CollEventSeriesDescriptorType_Enum WHERE (DisplayEnable = 1)";
        //        string Message = "";
        //        DiversityWorkbench.FormFunctions.SqlFillTable(SQL, ref dtKeyType, ref Message);
        //        if (dtKeyType.Rows.Count == 0)
        //        {
        //            bool OK = this.FormFunctions.getObjectPermissions("CollEventSeriesDescriptorType_Enum", "Insert");
        //            if (OK)
        //            {
        //                if (System.Windows.Forms.MessageBox.Show("No types have been defined so far. Would you like to add descriptor types?", "No descriptor types", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //                    this.EditDescriptorTypes();
        //            }
        //            else
        //                System.Windows.Forms.MessageBox.Show("No descriptor types have been defined so far. Please turn to your administrator", "No descriptor types", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //        else
        //        {

        //            DiversityWorkbench.FormGetItemFromHierarchy f = new DiversityWorkbench.FormGetItemFromHierarchy(dtKeyType, "Code", "ParentCode", "DisplayText", "DisplayText", "Code", "Please select a type", "Type of the Descriptor", false, true);
        //            f.ShowDialog();
        //            if (f.DialogResult == DialogResult.OK)
        //            {
        //                string Type = f.SelectedValue;
        //                System.Data.DataTable dtKeys = new DataTable();
        //                SQL = "SELECT DISTINCT Descriptor " +
        //                    "FROM CollectionEventSeriesDescriptor " +
        //                    "WHERE (DescriptorType = N'" + Type + "') AND (Descriptor <> N'') " +
        //                    "ORDER BY Descriptor";
        //                DiversityWorkbench.FormFunctions.SqlFillTable(SQL, ref dtKeys, ref Message);
        //                DiversityWorkbench.FormGetStringFromList fKey = new DiversityWorkbench.FormGetStringFromList(dtKeys, "Please enter or select the Descriptor", false, true);
        //                fKey.ShowDialog();
        //                if (fKey.DialogResult == DialogResult.OK && fKey.String.Length > 0)
        //                {
        //                    System.Data.DataRow R = dtDescriptor.NewRow();
        //                    R["SeriesID"] = this.SeriesID;
        //                    R["Descriptor"] = fKey.String;
        //                    R["DescriptorType"] = Type;
        //                    this.dtDescriptor.Rows.Add(R);
        //                    _sqlDataAdapterDescriptor.Update(dtDescriptor);
        //                    this.buildDescriptorTree();
        //                }
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void toolStripButtonDescriptorDelete_Click(object sender, EventArgs e)
        //{
        //    if (this.treeViewDescriptor.SelectedNode.Tag != null)
        //    {
        //        System.Data.DataRow R = (System.Data.DataRow)this.treeViewDescriptor.SelectedNode.Tag;
        //        string SQL = "DELETE FROM CollectionEventSeriesDescriptor WHERE SeriesID = " + this.SeriesID.ToString() + " AND DescriptorID = " + R["DescriptorID"].ToString();
        //        DiversityWorkbench.FormFunctions.SqlExecuteNonQuery(SQL);
        //        R.Delete();
        //        _sqlDataAdapterDescriptor.Update(dtDescriptor);
        //        this.buildDescriptorTree();
        //    }
        //}

        //#region Types
        //private void toolStripButtonDescriptorEditTypes_Click(object sender, EventArgs e)
        //{
        //    this.EditDescriptorTypes();
        //}

        //private void EditDescriptorTypes()
        //{
        //    try
        //    {
        //        DiversityWorkbench.Forms.FormEnumAdministration f = new DiversityWorkbench.Forms.FormEnumAdministration(
        //            DiversityCollection.Resource.KeyBlue,
        //            "CollEventSeriesDescriptorType_Enum",
        //            "Administration of Descriptor types",
        //            "", this.DescriptorImages());
        //        f.HierarchyChangesEnabled = true;
        //        f.ShowImage = true;
        //        f.setHelp("Descriptor");
        //        string SQL = "SELECT DescriptorType " +
        //            "FROM CollectionEventSeriesDescriptor " +
        //            "WHERE(URL <> '') " +
        //            "GROUP BY DescriptorType " +
        //            "HAVING(DescriptorType <> N'')";

        //        System.Data.DataTable dt = new DataTable();
        //        DiversityWorkbench.FormFunctions.SqlFillTable(SQL, ref dt);
        //        if (dt.Rows.Count > 0)
        //        {
        //            System.Collections.Generic.List<string> Blocked = new List<string>();
        //            foreach (System.Data.DataRow R in dt.Rows)
        //                Blocked.Add(R[0].ToString());
        //            f.setBlockedModuleTypes(Blocked);
        //        }

        //        SQL = "SELECT DescriptorType " +
        //            "FROM CollectionEventSeriesDescriptor " +
        //            "GROUP BY DescriptorType " +
        //            "HAVING(DescriptorType <> N'')";
        //        dt.Clear();
        //        DiversityWorkbench.FormFunctions.SqlFillTable(SQL, ref dt);
        //        if (dt.Rows.Count > 0)
        //        {
        //            System.Collections.Generic.List<string> Blocked = new List<string>();
        //            foreach (System.Data.DataRow R in dt.Rows)
        //                Blocked.Add(R[0].ToString());
        //            f.setDoNotDelete(Blocked);
        //        }
        //        f.ShowDialog();
        //        if (f.DataHaveBeenChanged)
        //        {
        //            this._EventSeriesDescriptorType_Enum = null;
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }

        //}

        //#endregion

        //#endregion


        #endregion

    }
}
