using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DiversityCollection.UserControls
{
    public partial class UserControl_EventLocality : UserControl__Data
    {

        #region Parameter

        private enum MeasurementUnit { m, km, feet, mile, degree, percent, numeric, deg_min_sec, orientation };

        private const string ValueColumn = "Location2";
        private string[] NamedOrientation = new string[16] { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW" };
        private readonly string GeoOriValStart = "[Ori.val.: ";
        private readonly string GeoOriValEnd = "]";
        private string _Geography;
        private System.Windows.Forms.BindingSource _SourceEvent;
        
        #endregion

        #region Construction

        public UserControl_EventLocality(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            System.Windows.Forms.BindingSource SourceEvent,
            string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this._Source = Source;
            this._SourceEvent = SourceEvent;
            this._HelpNamespace = HelpNamespace;
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }

        #endregion

        #region Override base

        public override void SetPosition(int Position = 0)
        {
            try
            {
                this._Source.CurrencyManager.Position = Position;
                this.setEventLocalisationControls();
                this.setGeography();

                //System.Collections.Generic.List<string> Settings = new List<string>();
                //Settings.Add("ModuleSource");
                //Settings.Add("CollectionEventLocalisation");
                //Settings.Add("ResponsibleAgentURI");
                //this.setUserControlModuleRelatedEntrySources(Settings, ref this.userControlModuleRelatedEntryLocalisationResponsible);
                this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryLocalisationResponsible, "ResponsibleAgentURI");

                DiversityWorkbench.UserSettings U = new DiversityWorkbench.UserSettings();
                string Source = "";
                string ProjectID = "";
                string Project = "";

                string LocalisationSystemID = "";
                System.Data.DataRowView RL = (System.Data.DataRowView)this._Source.Current;
                LocalisationSystemID = "LocalisationSystemID_" + RL["LocalisationSystemID"].ToString();
                System.Collections.Generic.List<string> SettingsLocalisation = new List<string>();
                SettingsLocalisation.Add("ModuleSource");
                SettingsLocalisation.Add("CollectionEventLocalisation");
                SettingsLocalisation.Add(LocalisationSystemID);
                Source = U.GetSetting(SettingsLocalisation);
                string ParsingMethod = DiversityCollection.LookupTable.ParsingMethodName(int.Parse(RL["LocalisationSystemID"].ToString()));
                switch (ParsingMethod)
                {
                    case "Gazetteer":
                        if (Source.Length > 0)
                            this.userControlModuleRelatedEntryGazetteer.FixSource(Source);
                        else
                        {
                            Source = U.GetSetting(SettingsLocalisation, "Database");
                            if (Source.Length == 0)
                                this.userControlModuleRelatedEntryGazetteer.ReleaseSource();
                            else
                            {
                                ProjectID = U.GetSetting(SettingsLocalisation, "ProjectID");
                                Project = U.GetSetting(SettingsLocalisation, "Project");
                                this.userControlModuleRelatedEntryGazetteer.FixSource(Source, ProjectID, Project);
                            }
                        }
                        break;
                    case "SamplingPlot":
                        if (Source.Length > 0)
                            this.userControlModuleRelatedEntrySamplingPlot.FixSource(Source);
                        else
                        {
                            Source = U.GetSetting(SettingsLocalisation, "Database");
                            if (Source.Length == 0)
                                this.userControlModuleRelatedEntrySamplingPlot.ReleaseSource();
                            else
                            {
                                ProjectID = U.GetSetting(SettingsLocalisation, "ProjectID");
                                Project = U.GetSetting(SettingsLocalisation, "Project");
                                this.userControlModuleRelatedEntrySamplingPlot.FixSource(Source, ProjectID, Project);
                            }
                        }
                        break;
                }

                System.Data.DataRowView rLoc = (System.Data.DataRowView)this._Source.Current;
                System.Data.DataRow[] rrLocGeo = this._iMainForm.DataSetCollectionSpecimen().CollectionEventLocalisationGeo.Select("LocalisationSystemID = " + rLoc["LocalisationSystemID"].ToString());

                if (rrLocGeo.Length > 0 )
                    this.textBoxGeography.Text = rrLocGeo[0]["GeographyAsString"].ToString();
                else
                    this.textBoxGeography.Text = "";
                this.textBoxGeography.Visible = true;
                this.textBoxGeography.ReadOnly = true;

                if (this._iMainForm.SelectedUnitHierarchyNode() != null)
                {
                    this.groupBoxEventLocalisation.Text = DiversityCollection.LookupTable.LocalisationSystemName(int.Parse(RL["LocalisationSystemID"].ToString()));
                    this.pictureBoxEventLocalisation.Image = DiversityCollection.Specimen.ImageForLocalisationSystem(int.Parse(RL["LocalisationSystemID"].ToString()));
                }

                //this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryGazetteer, ValueColumn);
                //this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryLocalisationResponsible, ValueColumn);
                //this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntrySamplingPlot, ValueColumn);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.setAvailability();
        }
        
        #endregion

        #region Control, private
        
        private void initControl()
        {
            try
            {

                this.comboBoxGeographyLocationUnits.SelectedIndexChanged += new System.EventHandler(this.comboBoxGeographyLocationUnits_SelectedIndexChanged);

                this.textBoxLocationDirection.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "DirectionToLocation", true));
                this.textBoxLocationDistance.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "DistanceToLocation", true));
                this.textBoxLocationAccuracy.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "LocationAccuracy", true));
                this.textBoxGeographyDeterminationDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "DeterminationDate", true));
                this.textBoxLocationNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "LocationNotes", true));
                this.textBoxLongitudeCache.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "AverageLongitudeCache", true));
                this.textBoxLatitudeCache.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "AverageLatitudeCache", true));
                this.textBoxAltitudeCache.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "AverageAltitudeCache", true));
                this.textBoxLocation1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Location1", true));
                this.textBoxLocation2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, ValueColumn, true));
                this.comboBoxLocalisationRecordingMethod.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "RecordingMethod", true));

                DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxLocationDirection);

                //this._SourceGeography = new BindingSource(this._iMainForm.DataSetCollectionSpecimen(), this._iMainForm.DataSetCollectionSpecimen().CollectionEventLocalisationGeo.TableName.ToString());
                //this._SourceGeography.DataMember = "CollectionEventLocalisationGeo";
                //this._SourceGeography.DataSource = this._iMainForm.DataSetCollectionSpecimen();
                //this.textBoxGeography.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._SourceGeography, "GeographyAsString", true));

                //this.textBoxGeography.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._SourceGeography, "GeographyAsString", true));
                this.setGeography();

                DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryLocalisationResponsible, A, "CollectionEventLocalisation", "ResponsibleName", "ResponsibleAgentURI", this._Source);

                DiversityWorkbench.Gazetteer G = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryGazetteer, G, "CollectionEventLocalisation", "Location1", ValueColumn, this._Source);

                DiversityWorkbench.SamplingPlot S = new DiversityWorkbench.SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntrySamplingPlot, S, "CollectionEventLocalisation", "Location1", ValueColumn, this._Source);

                this.setEventLocalisationControls();

                this.textBoxLongitudeCache.ReadOnly = true;
                this.textBoxLatitudeCache.ReadOnly = true;
                this.textBoxAltitudeCache.ReadOnly = true;

                System.Collections.Generic.List<DiversityWorkbench.UserControls.RemoteValueBinding> RvbSamplingPlot = new List<DiversityWorkbench.UserControls.RemoteValueBinding>();

                DiversityWorkbench.UserControls.RemoteValueBinding RvbPlotCountry = new DiversityWorkbench.UserControls.RemoteValueBinding();
                RvbPlotCountry.BindingSource = this._SourceEvent;
                RvbPlotCountry.Column = "CountryCache";
                RvbPlotCountry.RemoteParameter = "Country";
                RvbPlotCountry.ModeOfUpdate = DiversityWorkbench.UserControls.RemoteValueBinding.UpdateMode.AskForUpdate;
                RvbSamplingPlot.Add(RvbPlotCountry);

                DiversityWorkbench.UserControls.RemoteValueBinding RvbLat = new DiversityWorkbench.UserControls.RemoteValueBinding();
                RvbLat.BindingSource = this._Source;
                RvbLat.Column = "AverageLatitudeCache";
                RvbLat.RemoteParameter = "Latitude";
                RvbSamplingPlot.Add(RvbLat);

                DiversityWorkbench.UserControls.RemoteValueBinding RvbLong = new DiversityWorkbench.UserControls.RemoteValueBinding();
                RvbLong.BindingSource = this._Source;
                RvbLong.Column = "AverageLongitudeCache";
                RvbLong.RemoteParameter = "Longitude";
                RvbSamplingPlot.Add(RvbLong);

                DiversityWorkbench.UserControls.RemoteValueBinding RvbAlt = new DiversityWorkbench.UserControls.RemoteValueBinding();
                RvbAlt.BindingSource = this._Source;
                RvbAlt.Column = "AverageAltitudeCache";
                RvbAlt.RemoteParameter = "Altitude";
                RvbSamplingPlot.Add(RvbAlt);

                DiversityWorkbench.UserControls.RemoteValueBinding RvbAcc = new DiversityWorkbench.UserControls.RemoteValueBinding();
                RvbAcc.BindingSource = this._Source;
                RvbAcc.Column = "LocationAccuracy";
                RvbAcc.RemoteParameter = "Accuracy";
                RvbSamplingPlot.Add(RvbAcc);

                DiversityWorkbench.UserControls.RemoteValueBinding RvbGeo = new DiversityWorkbench.UserControls.RemoteValueBinding();
                RvbGeo.BindingSource = this.CollectionEventLocalisationGeoBindingSource;
                RvbGeo.Column = "GeographyAsString";
                RvbGeo.RemoteParameter = "Geography";
                RvbSamplingPlot.Add(RvbGeo);

                this.userControlModuleRelatedEntrySamplingPlot.setRemoteValueBindings(RvbSamplingPlot);


                System.Collections.Generic.List<DiversityWorkbench.UserControls.RemoteValueBinding> RvbGazetteer = new List<DiversityWorkbench.UserControls.RemoteValueBinding>();

                DiversityWorkbench.UserControls.RemoteValueBinding RvbCountry = new DiversityWorkbench.UserControls.RemoteValueBinding();
                RvbCountry.BindingSource = this._SourceEvent;
                RvbCountry.Column = "CountryCache";
                RvbCountry.RemoteParameter = "Country";
                RvbCountry.ModeOfUpdate = DiversityWorkbench.UserControls.RemoteValueBinding.UpdateMode.AskForUpdate;
                RvbGazetteer.Add(RvbCountry);

                DiversityWorkbench.UserControls.RemoteValueBinding RvbLatitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                RvbLatitude.BindingSource = this._Source;
                RvbLatitude.Column = "AverageLatitudeCache";
                RvbLatitude.RemoteParameter = "Latitude";
                RvbGazetteer.Add(RvbLatitude);

                DiversityWorkbench.UserControls.RemoteValueBinding RvbLongitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                RvbLongitude.BindingSource = this._Source;
                RvbLongitude.Column = "AverageLongitudeCache";
                RvbLongitude.RemoteParameter = "Longitude";
                RvbGazetteer.Add(RvbLongitude);

                DiversityWorkbench.UserControls.RemoteValueBinding RvbAltitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                RvbAltitude.BindingSource = this._Source;
                RvbAltitude.Column = "AverageAltitudeCache";
                RvbAltitude.RemoteParameter = "Altitude";
                RvbGazetteer.Add(RvbAltitude);

                DiversityWorkbench.UserControls.RemoteValueBinding RvbGazGeo = new DiversityWorkbench.UserControls.RemoteValueBinding();
                RvbGazGeo.BindingSource = this.CollectionEventLocalisationGeoBindingSource;
                RvbGazGeo.Column = "GeographyAsString";
                RvbGazGeo.RemoteParameter = "Geography";
                RvbGazetteer.Add(RvbGazGeo);

                this.userControlModuleRelatedEntryGazetteer.setRemoteValueBindings(RvbGazetteer);

                DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);

                DiversityWorkbench.Entity.setEntity(this, this.toolTip);

                this.CheckIfClientIsUpToDate();
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void setGeography()
        {
            try
            {
                if (this._Source.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                    string SQL = "SELECT TOP 1 Geography.ToString() AS Geography " +
                        " FROM CollectionEventLocalisation " +
                        " WHERE CollectionEventID = " + R["CollectionEventID"].ToString() +
                        " AND LocalisationSystemID = " + R["LocalisationSystemID"].ToString();
                    this._Geography = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    this.textBoxGeography.Text = this._Geography;
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        public System.Windows.Forms.BindingSource CollectionEventLocalisationGeoBindingSource
        {
            get
            {
                if (this._Source == null)
                {
                    if (this.components != null)
                        this._Source = new System.Windows.Forms.BindingSource(this.components);
                    else
                        this._Source = new BindingSource(this._iMainForm.DataSetCollectionSpecimen(), this._iMainForm.DataSetCollectionSpecimen().CollectionEventLocalisationGeo.TableName.ToString());
                    this.textBoxGeography.DataBindings.Clear();
                    this.textBoxGeography.ReadOnly = true;
                    ((System.ComponentModel.ISupportInitialize)(this._Source)).BeginInit();
                    this.textBoxGeography.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "GeographyAsString", true));
                    this._Source.DataMember = "CollectionEventLocalisationGeo";
                    this._Source.DataSource = this._iMainForm.DataSetCollectionSpecimen();
                    ((System.ComponentModel.ISupportInitialize)(this._Source)).EndInit();
                }
                return _Source;
            }
            //set { _collectionEventLocalisationGeoBindingSource = value; }
        }

        public void setEventLocalisationControls()//System.Data.DataRow DataRow)
        {
            try
            {
                string ParsingMethodName;
                string LocalisationSystem;
                if (this._Source.Current != null)
                {
                    int LocalisationSystemID = 0;
                    System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                    if (int.TryParse(R["LocalisationSystemID"].ToString(), out LocalisationSystemID))
                    {
                        ParsingMethodName = DiversityCollection.LookupTable.ParsingMethodName(LocalisationSystemID);
                        LocalisationSystem = DiversityCollection.LookupTable.LocalisationSystemName(LocalisationSystemID);
                        try
                        {
                            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtLocalisationSystem.Select("LocalisationSystemID = " + LocalisationSystemID.ToString());
                            if (rr.Length > 0)
                            {
                                System.Data.DataRow r = rr[0];
                                this.labelLocation1.Text = r["DisplayTextLocation1"].ToString();
                                if (this.labelLocation1.Text.Length > 11)
                                    this.labelLocation1.Text = this.labelLocation1.Text.Substring(0, 10) + ".";
                                this.labelLocation2.Text = r["DisplayTextLocation2"].ToString();
                                if (this.labelLocation2.Text.Length > 11)
                                    this.labelLocation2.Text = this.labelLocation2.Text.Substring(0, 10) + ".";
                                this.toolTip.SetToolTip(this.textBoxLocation1, r["DescriptionLocation1"].ToString());
                                this.toolTip.SetToolTip(this.textBoxLocation2, r["DescriptionLocation2"].ToString());

                                this.labelLocation1Custom.Text = r["DisplayTextLocation1"].ToString();
                                this.labelLocation2Custom.Text = r["DisplayTextLocation2"].ToString();
                                this.toolTip.SetToolTip(this.textBoxLocation1a, r["DescriptionLocation1"].ToString());
                                this.toolTip.SetToolTip(this.textBoxLocation2a, r["DescriptionLocation2"].ToString());

                                this.setGeographyCustomUnitList(ParsingMethodName);

                                if (this.comboBoxGeographyLocationUnits.Items.Count > 0)
                                    this.comboBoxGeographyLocationUnits.Visible = true;
                                else this.comboBoxGeographyLocationUnits.Visible = false;
                                this.buttonGetCoordinatesFromGoogleMaps.Visible = false;
                                this.buttonCoordinatesConvert.Visible = false;
                                this.textBoxGeography.Visible = false;
                                if (this.textBoxGeography.Text.Length > 0)
                                    this.textBoxGeography.Visible = true;

                                this.buttonEditGeography.Visible = false;

                                switch (ParsingMethodName)
                                {
                                    case "Altitude":
                                        this.userControlModuleRelatedEntryGazetteer.Visible = false;
                                        this.userControlModuleRelatedEntrySamplingPlot.Visible = false;
                                        this.tableLayoutPanelGeographyLocationStandard.Visible = true;
                                        this.tableLayoutPanelGeographyLocation.Visible = true;

                                        this.labelLocationDirection.Visible = false;
                                        this.textBoxLocationDirection.Visible = false;
                                        this.labelLocationDistance.Visible = false;
                                        this.textBoxLocationDistance.Visible = false;

                                        this.labelLongitudeCache.Visible = false;
                                        this.labelLatitudeCache.Visible = false;
                                        this.textBoxLongitudeCache.Visible = false;
                                        this.textBoxLatitudeCache.Visible = false;
                                        this.labelAltitudeCache.Visible = true;
                                        this.textBoxAltitudeCache.Visible = true;
                                        break;
                                    case "Coordinates":
                                        this.userControlModuleRelatedEntryGazetteer.Visible = false;
                                        this.userControlModuleRelatedEntrySamplingPlot.Visible = false;
                                        this.tableLayoutPanelGeographyLocation.Visible = true;
                                        this.tableLayoutPanelGeographyLocationStandard.Visible = true;
                                        if (LocalisationSystem == "Coordinates WGS84")
                                        {
                                            this.buttonGetCoordinatesFromGoogleMaps.Visible = true;
                                            this.textBoxGeography.Visible = true;
                                        }
                                        this.textBoxAltitudeCache.Visible = false;
                                        this.labelAltitudeCache.Visible = false;
                                        this.buttonCoordinatesConvert.Visible = true;
                                        this.buttonEditGeography.Visible = true;
                                        goto default;
                                    case "Gazetteer":
                                        this.tableLayoutPanelGeographyLocation.Visible = false;
                                        this.tableLayoutPanelGeographyLocationStandard.Visible = false;
                                        this.userControlModuleRelatedEntryGazetteer.Visible = true;
                                        this.userControlModuleRelatedEntrySamplingPlot.Visible = false;
                                        goto default;
                                    case "SamplingPlot":
                                        this.tableLayoutPanelGeographyLocation.Visible = false;
                                        this.tableLayoutPanelGeographyLocationStandard.Visible = false;
                                        this.userControlModuleRelatedEntryGazetteer.Visible = false;
                                        this.userControlModuleRelatedEntrySamplingPlot.Visible = true;
                                        goto default;
                                    case "UTM":
                                    case "RD":
                                    case "GK":
                                        this.userControlModuleRelatedEntryGazetteer.Visible = false;
                                        this.userControlModuleRelatedEntrySamplingPlot.Visible = false;
                                        this.tableLayoutPanelGeographyLocation.Visible = true;
                                        this.tableLayoutPanelGeographyLocationStandard.Visible = true;
                                        goto default;
                                    case "MTB":
                                        this.buttonGetCoordinatesFromGoogleMaps.Visible = true;
                                        this.userControlModuleRelatedEntrySamplingPlot.Visible = false;
                                        this.userControlModuleRelatedEntryGazetteer.Visible = false;
                                        this.tableLayoutPanelGeographyLocation.Visible = true;
                                        this.tableLayoutPanelGeographyLocationStandard.Visible = true;
                                        this.textBoxGeography.Visible = true;
                                        goto default;
                                    case "Height":
                                    case "Depth":
                                    case "Exposition":
                                    case "Slope":
                                        this.userControlModuleRelatedEntryGazetteer.Visible = false;
                                        this.userControlModuleRelatedEntrySamplingPlot.Visible = false;
                                        this.tableLayoutPanelGeographyLocation.Visible = true;
                                        this.tableLayoutPanelGeographyLocationStandard.Visible = true;

                                        this.labelLocationDirection.Visible = false;
                                        this.textBoxLocationDirection.Visible = false;
                                        this.labelLocationDistance.Visible = false;
                                        this.textBoxLocationDistance.Visible = false;

                                        this.labelAltitudeCache.Visible = false;
                                        this.labelLongitudeCache.Visible = false;
                                        this.labelLatitudeCache.Visible = false;
                                        this.textBoxLongitudeCache.Visible = false;
                                        this.textBoxLatitudeCache.Visible = false;
                                        this.textBoxAltitudeCache.Visible = false;

                                        break;
                                    case "Top50":
                                        goto default;
                                    default:
                                        this.labelLocationDirection.Visible = true;
                                        this.textBoxLocationDirection.Visible = true;
                                        this.labelLocationDistance.Visible = true;
                                        this.textBoxLocationDistance.Visible = true;
                                        this.labelLongitudeCache.Visible = true;
                                        this.labelLatitudeCache.Visible = true;
                                        this.textBoxLongitudeCache.Visible = true;
                                        this.textBoxLatitudeCache.Visible = true;
                                        break;
                                }
                            }
                            else // ensure correct display of old systems that are not used any more
                            {
                                System.Data.DataTable dtLocSys = new DataTable();
                                string SQL = "SELECT * FROM LocalisationSystem L WHERE L.LocalisationSystemID = " + LocalisationSystemID.ToString();
                                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                                ad.Fill(dtLocSys);
                                if (dtLocSys.Rows.Count == 1)
                                {
                                    this.labelLocation1.Text = dtLocSys.Rows[0]["DisplayTextLocation1"].ToString();
                                    if (this.labelLocation1.Text.Length > 11)
                                        this.labelLocation1.Text = this.labelLocation1.Text.Substring(0, 10) + ".";
                                    this.labelLocation2.Text = dtLocSys.Rows[0]["DisplayTextLocation2"].ToString();
                                    if (this.labelLocation2.Text.Length > 11)
                                        this.labelLocation2.Text = this.labelLocation2.Text.Substring(0, 10) + ".";
                                    this.toolTip.SetToolTip(this.textBoxLocation1, dtLocSys.Rows[0]["DescriptionLocation1"].ToString());
                                    this.toolTip.SetToolTip(this.textBoxLocation2, dtLocSys.Rows[0]["DescriptionLocation2"].ToString());

                                    this.labelLocation1Custom.Text = dtLocSys.Rows[0]["DisplayTextLocation1"].ToString();
                                    this.labelLocation2Custom.Text = dtLocSys.Rows[0]["DisplayTextLocation2"].ToString();
                                    this.toolTip.SetToolTip(this.textBoxLocation1a, dtLocSys.Rows[0]["DescriptionLocation1"].ToString());
                                    this.toolTip.SetToolTip(this.textBoxLocation2a, dtLocSys.Rows[0]["DescriptionLocation2"].ToString());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void buttonGetCoordinatesFromGoogleMaps_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(sender);
                CultureInfo InvC = new CultureInfo("");
                string ParsingMethodName = "";
                string LocalisationSystem = "";
                int LocalisationSystemID = 0;
                System.Data.DataRowView RLoc = (System.Data.DataRowView)this._Source.Current;

                // get the current Localisation system
                if (int.TryParse(RLoc["LocalisationSystemID"].ToString(), out LocalisationSystemID))
                {
                    ParsingMethodName = DiversityCollection.LookupTable.ParsingMethodName(LocalisationSystemID);
                    LocalisationSystem = DiversityCollection.LookupTable.LocalisationSystemName(LocalisationSystemID);
                }

                // try to find existing Coordinates
                bool OK = false;
                double la = 0.0;
                double lo = 0.0;
                string Lat = this.textBoxLocation2.Text.ToString(InvC).Replace(",", ".");
                string Long = this.textBoxLocation1.Text.ToString(InvC).Replace(",", ".");
                if (Lat.Length > 0 && Long.Length > 0 && ParsingMethodName != "MTB")
                {
                    OK = true;
                    if (!double.TryParse(Lat, NumberStyles.Float, InvC, out la)) OK = false;
                    if (!double.TryParse(Long, NumberStyles.Float, InvC, out lo)) OK = false;
                }
                else if (ParsingMethodName == "MTB")
                {
                    Lat = this.textBoxLatitudeCache.Text.ToString(InvC).Replace(",", ".");
                    Long = this.textBoxLongitudeCache.Text.ToString(InvC).Replace(",", ".");
                    OK = true;
                    if (!double.TryParse(Lat, NumberStyles.Float, InvC, out la)) OK = false;
                    if (!double.TryParse(Long, NumberStyles.Float, InvC, out lo)) OK = false;
                }
                else if (ParsingMethodName == "UTM")
                {
                    Lat = this.textBoxLatitudeCache.Text.ToString(InvC).Replace(",", ".");
                    Long = this.textBoxLongitudeCache.Text.ToString(InvC).Replace(",", ".");
                    OK = true;
                    if (!double.TryParse(Lat, NumberStyles.Float, InvC, out la)) OK = false;
                    if (!double.TryParse(Long, NumberStyles.Float, InvC, out lo)) OK = false;
                }
                if (!OK)
                {
                    // try to find existing coordinates
                    System.Data.DataRow[] rrC = this._iMainForm.DataSetCollectionSpecimen().CollectionEventLocalisation.Select("AverageLatitudeCache <> 0 AND AverageLongitudeCache <> 0");
                    if (rrC.Length > 0)
                    {
                        OK = true;
                        if (!double.TryParse(rrC[0]["AverageLatitudeCache"].ToString(), NumberStyles.Float, InvC, out la)) OK = false;
                        if (!OK) OK = double.TryParse(rrC[0]["AverageLatitudeCache"].ToString().Replace(".", ","), NumberStyles.Float, InvC, out la);
                        if (!OK) OK = double.TryParse(rrC[0]["AverageLatitudeCache"].ToString().Replace(",", "."), NumberStyles.Float, InvC, out la);
                        if (la > 180) la = 0.0;

                        if (!double.TryParse(rrC[0]["AverageLongitudeCache"].ToString(), NumberStyles.Float, InvC, out lo)) OK = false;
                        if (!OK) OK = double.TryParse(rrC[0]["AverageLongitudeCache"].ToString().Replace(".", ","), NumberStyles.Float, InvC, out lo);
                        if (!OK) OK = double.TryParse(rrC[0]["AverageLongitudeCache"].ToString().Replace(",", "."), NumberStyles.Float, InvC, out lo);
                        if (lo > 90) lo = 0.0;
                    }
                }

                // get the correct form
                if (LocalisationSystem == "Coordinates WGS84")
                {
                    DiversityWorkbench.Forms.FormGoogleMapsCoordinates f;


                    if (OK) f = new DiversityWorkbench.Forms.FormGoogleMapsCoordinates(la, lo);
                    else f = new DiversityWorkbench.Forms.FormGoogleMapsCoordinates(0.0, 0.0);
                    f.ShowInTaskbar = true;
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        try
                        {
                            if (f.Longitude < 360 && f.Longitude > -360 && f.Latitude < 90 && f.Latitude > -90 && f.LatitudeAccuracy != 0.0 && f.LongitudeAccuracy != 0.0)
                            {
                                string sLatitude = f.Latitude.ToString("F09", InvC);
                                double dLat;
                                if(!double.TryParse(sLatitude, out dLat))
                                {
                                    sLatitude = sLatitude.Replace(",", ".");
                                    if (!double.TryParse(sLatitude, out dLat))
                                    {
                                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("Failed to convert coordinates: " + sLatitude);
                                    }
                                }
                                string sLongitude = f.Longitude.ToString("F09", InvC);
                                double dLong;
                                if (!double.TryParse(sLongitude, out dLong))
                                {
                                    sLongitude = sLongitude.Replace(",", ".");
                                    if (!double.TryParse(sLongitude, out dLong))
                                    {
                                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("Failed to convert coordinates: " + sLongitude);
                                    }
                                }
                                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                                R["Location1"] = sLongitude;// f.Longitude.ToString("F09", InvC);
                                this.textBoxLocation1.Text = sLongitude;// f.Longitude.ToString("F09", InvC);
                                R[ValueColumn] = sLatitude;// f.Latitude.ToString("F09", InvC);
                                this.textBoxLocation2.Text = sLatitude;// f.Latitude.ToString("F09", InvC);
                                R["LocationAccuracy"] = f.Accuracy.ToString("F00", InvC) + " m";
                                this.textBoxLocationAccuracy.Text = f.Accuracy.ToString("F00", InvC) + " m";
                                R["AverageLongitudeCache"] = dLong;// f.Longitude;
                                this.textBoxLongitudeCache.Text = sLongitude;// f.Longitude.ToString("F09", InvC);
                                R["AverageLatitudeCache"] = dLat;// f.Latitude;
                                this.textBoxLatitudeCache.Text = sLatitude;// f.Latitude.ToString("F09", InvC);

                                /// TODO:
                                /// geonames is not available any more
                                /// implement alternativ via OpenStreetMaps
                                //System.Collections.Generic.Dictionary<DiversityWorkbench.GeoFunctions.GeoInfo, string> GeoInfos = DiversityWorkbench.GeoFunctions.getGeoInfos(f.Latitude, f.Longitude);
                                //if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Altitude))
                                //    R["AverageAltitudeCache"] = float.Parse(GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Altitude]);

                                string Notes = "";
                                if (!R["LocationNotes"].Equals(System.DBNull.Value)) Notes = R["LocationNotes"].ToString();
                                if (Notes.Length > 0)
                                {
                                    if (Notes.IndexOf("Derived from Google Maps") == -1)
                                        Notes += ". Derived from Google Maps";
                                }
                                else
                                    Notes = "Derived from Google Maps";
                                R["LocationNotes"] = Notes;
                                this.textBoxLocationNotes.Text = Notes;

                                System.Data.DataRow[] rrGeo = this._iMainForm.DataSetCollectionSpecimen().CollectionEventLocalisationGeo.Select("LocalisationSystemID = " + R["LocalisationSystemID"].ToString());
                                if (rrGeo.Length == 1)
                                {
                                    if (rrGeo[0][2].ToString().StartsWith("POINT"))
                                    {
                                        string Geo = "POINT (" + f.Longitude.ToString("F09", InvC) + " " + f.Latitude.ToString("F09", InvC) + ")";
                                        rrGeo[0][2] = Geo;
                                        rrGeo[0][3] = Geo;
                                    }
                                }

                                /// TODO:
                                /// geonames is not available any more
                                /// implement alternative via OpenStreetMaps
                                if (1 == 1)// 2) 
                                {

                                    System.Collections.Generic.Dictionary<DiversityWorkbench.GeoFunctions.GeoInfo, string> GeoInfos = DiversityWorkbench.GeoFunctions.getGeoInfos(f.Latitude, f.Longitude);

                                    string geoinfosource = "test";
                                    // Ariane set GeoInfos Source (static geoinfosoruce variable from geofunction no longer availbale)
                                    if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Source))
                                    {
                                        geoinfosource = GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Source];
                                    }

                                    // insert Gazetteer entry if missing
                                    System.Data.DataRow[] rrGaz = this._iMainForm.DataSetCollectionSpecimen().CollectionEventLocalisation.Select("LocalisationSystemID = 7");
                                    if (GeoInfos.Count > 0
                                        && rrGaz.Length == 0
                                        && GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.PlaceName))
                                    {
                                        DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionEventLocalisationRow RGazetteer = this._iMainForm.DataSetCollectionSpecimen().CollectionEventLocalisation.NewCollectionEventLocalisationRow();
                                        RGazetteer.CollectionEventID = int.Parse(this._iMainForm.DataSetCollectionSpecimen().CollectionEvent.Rows[0]["CollectionEventID"].ToString());
                                        RGazetteer.LocalisationSystemID = 7;
                                        string Place = GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.PlaceName];
                                        if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.City)
                                            && GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.City] != GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.PlaceName])
                                            Place += ", " + GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.City];
                                        if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.CountrySubdivision))
                                            Place += ", " + GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.CountrySubdivision];
                                        if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Country))
                                            Place += ", " + GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Country];
                                        RGazetteer.Location1 = Place;
                                        RGazetteer.LocationNotes = "Source: " + GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Source];
                                        if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Latitude))
                                            RGazetteer.AverageLatitudeCache = float.Parse(GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Latitude]);
                                        if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Longitude))
                                            RGazetteer.AverageLongitudeCache = float.Parse(GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Longitude]);
                                        if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Distance))
                                        {
                                            RGazetteer.LocationAccuracy = f.Accuracy.ToString("F00", InvC) + " m";
                                            RGazetteer.DistanceToLocation = GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Distance];
                                        }
                                        if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Direction))
                                            RGazetteer.DirectionToLocation = GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Direction];
                                        this._iMainForm.DataSetCollectionSpecimen().CollectionEventLocalisation.Rows.Add(RGazetteer);
                                    }
                                    else if (GeoInfos.Count > 0
                                        && rrGaz.Length == 1
                                        && rrGaz[0]["LocationNotes"].ToString().Contains(geoinfosource) //Ariane alt: Contains(DiversityWorkbench.GeoFunctions.GeoInfoSource)
                                        && GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.PlaceName))
                                    {
                                        string Place = GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.PlaceName];
                                        if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.City)
                                            && GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.City] != GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.PlaceName])
                                            Place += ", " + GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.City];
                                        if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.CountrySubdivision))
                                            Place += ", " + GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.CountrySubdivision];
                                        if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Country))
                                            Place += ", " + GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Country];
                                        rrGaz[0]["Location1"] = Place;
                                        rrGaz[0]["LocationNotes"] = "Source: " + GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Source];
                                        if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Latitude))
                                            rrGaz[0]["AverageLatitudeCache"] = float.Parse(GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Latitude]);
                                        if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Longitude))
                                            rrGaz[0]["AverageLongitudeCache"] = float.Parse(GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Longitude]);
                                        if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Distance))
                                        {
                                            rrGaz[0]["LocationAccuracy"] = GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Distance];
                                            rrGaz[0]["DistanceToLocation"] = GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Distance];
                                        }
                                        if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Direction))
                                            rrGaz[0]["DirectionToLocation"] = GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Direction];
                                    }

                                    // insert altitude entry if missing
                                    System.Data.DataRow[] rrAlt = this._iMainForm.DataSetCollectionSpecimen().CollectionEventLocalisation.Select("LocalisationSystemID = 4");
                                    if (GeoInfos.Count > 0
                                        && rrAlt.Length == 0
                                        && GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Altitude))
                                    {
                                        DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionEventLocalisationRow RAltitude = this._iMainForm.DataSetCollectionSpecimen().CollectionEventLocalisation.NewCollectionEventLocalisationRow();
                                        RAltitude.CollectionEventID = int.Parse(this._iMainForm.DataSetCollectionSpecimen().CollectionEvent.Rows[0]["CollectionEventID"].ToString());
                                        RAltitude.LocalisationSystemID = 4;
                                        RAltitude.Location1 = GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Altitude];
                                        RAltitude.AverageAltitudeCache = float.Parse(GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Altitude]);
                                        RAltitude.LocationNotes = "Source: " + GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.AltitudeSource];
                                        this._iMainForm.DataSetCollectionSpecimen().CollectionEventLocalisation.Rows.Add(RAltitude);
                                    }
                                    else if (GeoInfos.Count > 0
                                        && rrAlt.Length == 1
                                        && rrAlt[0]["LocationNotes"].ToString().Contains(geoinfosource)// Ariane alt: Contains(DiversityWorkbench.GeoFunctions.GeoInfoSource)
                                        && GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Altitude))
                                    {
                                        rrAlt[0]["Location1"] = GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Altitude];
                                        rrAlt[0]["AverageAltitudeCache"] = float.Parse(GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Altitude]);
                                        rrAlt[0]["LocationNotes"] = "Source: " + GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.AltitudeSource];
                                    }
                                    // setting the countryCache
                                    if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Country))
                                    {
                                        if (this._iMainForm.DataSetCollectionSpecimen().CollectionEvent.Rows[0]["CountryCache"].Equals(System.DBNull.Value) ||
                                            this._iMainForm.DataSetCollectionSpecimen().CollectionEvent.Rows[0]["CountryCache"].ToString().Trim().Length == 0)
                                            this._iMainForm.DataSetCollectionSpecimen().CollectionEvent.Rows[0]["CountryCache"] = GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Country];
                                        else
                                        {
                                            if (this._iMainForm.DataSetCollectionSpecimen().CollectionEvent.Rows[0]["CountryCache"].ToString() != GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Country])
                                            {
                                                string Message = "Should the entry for the country\r\n" + this._iMainForm.DataSetCollectionSpecimen().CollectionEvent.Rows[0]["CountryCache"].ToString() +
                                                    "\r\nbe replaced by\r\n" + GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Country];
                                                if (System.Windows.Forms.MessageBox.Show(Message, "Change country", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                                {
                                                    this._iMainForm.DataSetCollectionSpecimen().CollectionEvent.Rows[0]["CountryCache"] = GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Country];
                                                }
                                            }
                                        }
                                    }
                                    this._iMainForm.saveSpecimen();

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                }
                else if (ParsingMethodName == "MTB")//LocalisationSystem == "MTB (A, CH, D)")
                {
                    System.Data.DataRow[] RR = this._iMainForm.DataSetCollectionSpecimen().CollectionEventLocalisation.Select("");
                    if (RR.Length > 0)
                    {
                        string ConnectionString = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].ServerConnection.ConnectionString;
                        string tntServer = global::DiversityCollection.Properties.Settings.Default.TNTServer;
                        if (string.IsNullOrEmpty(tntServer))
                        {
                            MessageBox.Show("A configuration error happens. The TNT Server is not defined.");
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("UserControl_EventLocality.cs: A configuration error happens.The TNT Server is not defined.");
                        }
                            
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KVconn in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].ServerConnectionList())
                        {
                            if (KVconn.Value.ConnectionString.Length > 0)
                                ConnectionString = KVconn.Value.ConnectionString;
                            if (KVconn.Value.DatabaseServer.Equals(tntServer.Split(',')[0], StringComparison.CurrentCultureIgnoreCase))
                                break;
                            if (KVconn.Value.LinkedServer == tntServer && KVconn.Value.DatabaseName == "DiversityGazetteer2")
                            {
                                ConnectionString = KVconn.Value.ConnectionString;
                                break;
                            }
                        }



                        if (lo > 0 && la > 0)
                        {
                            DiversityWorkbench.Forms.FormTK25 f;
                            int iTest = 0;
                            if (!RLoc[ValueColumn].Equals(System.DBNull.Value) && RLoc[ValueColumn].ToString().Length > 0)
                            {
                                f = new DiversityWorkbench.Forms.FormTK25(RLoc["Location1"].ToString(), RLoc[ValueColumn].ToString(), ConnectionString);
                            }
                            else if (!RLoc["Location1"].Equals(System.DBNull.Value) && RLoc["Location1"].ToString().Length > 0 && int.TryParse(RLoc["Location1"].ToString(), out iTest))
                            {
                                f = new DiversityWorkbench.Forms.FormTK25(RLoc["Location1"].ToString(), RLoc[ValueColumn].ToString(), /*(float)lo, (float)la,*/ ConnectionString);
                            }
                            else
                                f = new DiversityWorkbench.Forms.FormTK25((float)lo, (float)la, ConnectionString);
                            try
                            {
                                f.ShowDialog();
                                if (f.DialogResult == DialogResult.OK)
                                {
                                    RLoc["Location1"] = f.TK25;
                                    this.textBoxLocation1.Text = f.TK25;

                                    RLoc[ValueColumn] = f.TK25_Quadrant;
                                    this.textBoxLocation2.Text = f.TK25_Quadrant;

                                    RLoc["LocationNotes"] = f.TK25_Name;
                                    this.textBoxLocationNotes.Text = f.TK25_Name;

                                    RLoc["AverageLatitudeCache"] = f.AverageLatitude;
                                    this.textBoxLatitudeCache.Text = f.AverageLatitude.ToString("F09", InvC);

                                    RLoc["AverageLongitudeCache"] = f.AverageLongitude;
                                    this.textBoxLongitudeCache.Text = f.AverageLongitude.ToString("F09", InvC);

                                    if (f.Geography != null)
                                    {
                                        System.Data.DataRow[] RRGeo = this._iMainForm.DataSetCollectionSpecimen().CollectionEventLocalisationGeo.Select("CollectionEventID = " + RLoc["CollectionEventID"].ToString() + " AND LocalisationSystemID = " + RLoc["LocalisationSystemID"].ToString());
                                        if (RRGeo.Length == 1)
                                        {
                                            RRGeo[0]["GeographyAsString"] = f.Geography;
                                            this.textBoxGeography.Text = f.Geography;
                                            this.textBoxGeography.ReadOnly = true;
                                        }
                                        else if (RRGeo.Length == 0)
                                        {
                                            System.Data.DataRow RGeo = this._iMainForm.DataSetCollectionSpecimen().CollectionEventLocalisationGeo.NewRow();
                                            RGeo["GeographyAsString"] = f.Geography;
                                            RGeo["CollectionEventID"] = RLoc["CollectionEventID"];
                                            RGeo["LocalisationSystemID"] = RLoc["LocalisationSystemID"];
                                            this._iMainForm.DataSetCollectionSpecimen().CollectionEventLocalisationGeo.Rows.Add(RGeo);
                                        }
                                    }
                                }
                            }
                            catch (System.Exception ex) { }
                        }
                        else
                        {
                            string TK25 = RLoc["Location1"].ToString();
                            string TK25_Quadrant = RLoc[ValueColumn].ToString();
                            DiversityWorkbench.Forms.FormTK25 f = new DiversityWorkbench.Forms.FormTK25(TK25, TK25_Quadrant, ConnectionString);
                            try
                            {
                                f.ShowDialog();
                                if (f.DialogResult == DialogResult.OK)
                                {
                                    RLoc["LocationNotes"] = f.TK25_Name;
                                    this.textBoxLocationNotes.Text = f.TK25_Name;

                                    RLoc["Location1"] = f.TK25;
                                    this.textBoxLocation1.Text = f.TK25;

                                    RLoc[ValueColumn] = f.TK25_Quadrant;
                                    this.textBoxLocation2.Text = f.TK25_Quadrant;

                                    RLoc["AverageLatitudeCache"] = f.AverageLatitude;
                                    this.textBoxLatitudeCache.Text = f.AverageLatitude.ToString("F09", InvC);

                                    RLoc["AverageLongitudeCache"] = f.AverageLongitude;
                                    this.textBoxLongitudeCache.Text = f.AverageLongitude.ToString("F09", InvC);

                                    System.Data.DataRow[] RRGeo = this._iMainForm.DataSetCollectionSpecimen().CollectionEventLocalisationGeo.Select("CollectionEventID = " + RLoc["CollectionEventID"].ToString() + " AND LocalisationSystemID = " + RLoc["LocalisationSystemID"].ToString());
                                    if (RRGeo.Length == 1 && f.Geography != null)
                                        RRGeo[0]["GeographyAsString"] = f.Geography;
                                }
                            }
                            catch (System.Exception ex) { }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void dateTimePickerLocalisationDate_CloseUp(object sender, EventArgs e)
        {
            if (this._Source.Current != null)
            {
                try
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                    R["DeterminationDate"] = this.dateTimePickerLocalisationDate.Value.Date.ToString("yyyy-MM-dd HH:mm:ss");
                    this.textBoxGeographyDeterminationDate.Text = this.dateTimePickerLocalisationDate.Value.Date.ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void textBoxGeographyDeterminationDate_Validating(object sender, CancelEventArgs e)
        {
            if (this.textBoxGeographyDeterminationDate.Text.Length == 0)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                R.BeginEdit();
                R["DeterminationDate"] = System.DBNull.Value;
                R.EndEdit();
            }
        }

        private void textBoxLocation1_Leave(object sender, EventArgs e)
        {
            if (this._Source.Current != null)
            {
                System.Data.DataRowView RV = (System.Data.DataRowView)this._Source.Current;
                string Test = RV["Location1"].ToString();
                string NewText = this.textBoxLocation1.Text;
                int i = 0;
                if (!RV["Location1"].Equals(System.DBNull.Value)
                    && RV["Location1"].ToString() != this.textBoxLocation1.Text)//                    && RV["Location1"].ToString() != "New Altitude (mNN)"
                {
                    if (RV["Location1"].ToString() == "New Altitude (mNN)" || RV["Location1"].ToString() == "Altitude (mNN)" || RV["Location1"].ToString().IndexOf("Altitude") > -1)
                        RV["Location1"] = this.textBoxLocation1.Text;
                    else if (!RV["Location1"].Equals(System.DBNull.Value)
                        && int.TryParse(RV["Location1"].ToString(), out i)
                        && RV["Location1"].ToString() != this.textBoxLocation1.Text)//   
                    {
                        RV["Location1"] = this.textBoxLocation1.Text;
                    }
                    bool OK = this.TryToSaveAndCalculateAverageForLocalisation(false);
                    if (OK && this.TryToSaveGeography() && NewText.Length > 0)
                    {
                        if (RV["Location1"].ToString() != NewText)
                            RV["Location1"] = NewText;
                    }
                }
            }
        }

        private void textBoxLocation2_Leave(object sender, EventArgs e)
        {
            if (this._Source.Current != null)
            {
                int i = 0;
                double d = 0;
                System.Data.DataRowView RV = (System.Data.DataRowView)this._Source.Current;
                if (RV[ValueColumn].Equals(System.DBNull.Value) && int.TryParse(this.textBoxLocation2.Text, out i))
                {
                    RV[ValueColumn] = i.ToString();
                }
                else if (!RV[ValueColumn].Equals(System.DBNull.Value)
                    && int.TryParse(RV[ValueColumn].ToString(), out i)
                    && RV[ValueColumn].ToString() != this.textBoxLocation2.Text)//                    && RV["Location2"].ToString() != this.textBoxLocation2.Text)
                {
                    RV[ValueColumn] = this.textBoxLocation2.Text;
                }
                else if (double.TryParse(this.textBoxLocation2.Text, out d)
                    && RV[ValueColumn].ToString() != this.textBoxLocation2.Text)
                {
                    RV[ValueColumn] = this.textBoxLocation2.Text;
                }
                else if (double.TryParse(this.textBoxLocation2.Text.Replace(",", "."), out d)
                    && RV[ValueColumn].ToString().Replace(",", ".") != this.textBoxLocation2.Text.Replace(",", "."))
                {
                    RV[ValueColumn] = this.textBoxLocation2.Text;
                }
                this.TryToSaveAndCalculateAverageForLocalisation(false);
                this.TryToSaveGeography();
            }
        }

        private bool TryToSaveAndCalculateAverageForLocalisation(bool FromDataRow)
        {
            CultureInfo InvC = new CultureInfo("");

            string Loc1 = this.textBoxLocation1.Text;
            string Loc2 = this.textBoxLocation2.Text;
            System.Data.DataRowView RV = (System.Data.DataRowView)this._Source.Current;
            if (FromDataRow)
            {
                if (RV["Location1"].Equals(System.DBNull.Value)) Loc1 = "";
                else Loc1 = RV["Location1"].ToString();
                if (RV[ValueColumn].Equals(System.DBNull.Value)) Loc2 = "";
                else Loc2 = RV[ValueColumn].ToString();
            }
            int LocSysID;
            if (!int.TryParse(RV["LocalisationSystemID"].ToString(), out LocSysID))
                return false;
            string LocalisationSystem = DiversityCollection.LookupTable.LocalisationSystemName(LocSysID);
            switch (LocalisationSystem)
            {
                case "mNN (barometric)":
                case "Altitude (mNN)":
                    double? _AltFrom = null;
                    double? _AltTo = null;
                    double? _AltAvg = null;

                    double AltFrom;
                    double AltTo;
                    if (Loc1.Length == 0) _AltFrom = null;
                    else
                    {
                        if (!double.TryParse(Loc1, NumberStyles.Float, InvC, out AltFrom))
                        {
                            if (!double.TryParse(Loc1.Replace(",", "."), NumberStyles.Float, InvC, out AltFrom))
                                return false;
                        }
                        _AltFrom = AltFrom;
                    }
                    if (Loc2.Length == 0) _AltTo = null;
                    else
                    {
                        if (!double.TryParse(Loc2, NumberStyles.Float, InvC, out AltTo))
                        {
                            if (!double.TryParse(Loc2.Replace(",", "."), NumberStyles.Float, InvC, out AltTo))
                                return false;
                        }
                        _AltTo = AltTo;
                    }
                    if (_AltTo != null && _AltFrom != null) _AltAvg = (_AltFrom + _AltTo) / 2;
                    else if (_AltTo != null && _AltFrom == null) _AltAvg = _AltTo;
                    else if (_AltTo == null && _AltFrom != null) _AltAvg = _AltFrom;
                    if (_AltAvg != null)
                    {
                        RV["AverageAltitudeCache"] = _AltAvg;
                        if (this.labelAltitudeCache.Text != RV["AverageAltitudeCache"].ToString())
                            this.textBoxAltitudeCache.Text = RV["AverageAltitudeCache"].ToString();
                    }
                    break;
                case "Coordinates":
                case "Coordinates WGS84":
                    double Lat = 0;
                    double Long = 0;
                    if (Loc1.Length > 0)
                    {
                        if (!double.TryParse(Loc1, NumberStyles.Float, InvC, out Long))
                        {
                            if (!double.TryParse(Loc1.Replace(",", "."), NumberStyles.Float, InvC, out Long))
                                return false;
                        }
                        RV["AverageLongitudeCache"] = Long;
                        this.textBoxLongitudeCache.Text = Long.ToString();
                    }
                    if (Loc2.Length > 0)
                    {
                        if (!double.TryParse(Loc2, NumberStyles.Float, InvC, out Lat))
                        {
                            if (!double.TryParse(Loc2.Replace(",", "."), NumberStyles.Float, InvC, out Lat))
                                return false;
                        }
                        RV["AverageLatitudeCache"] = Lat;
                        this.textBoxLatitudeCache.Text = Lat.ToString();
                    }
                    break;
                case "UTM":
                    break;
                default:
                    break;
            }
            return true;
        }

        private bool TryToSaveGeography()
        {
            try
            {
                System.Data.DataRowView RV = (System.Data.DataRowView)this._Source.Current;
                int LocSysID;
                int EventID;
                if (!int.TryParse(RV["LocalisationSystemID"].ToString(), out LocSysID))
                    return false;
                if (!int.TryParse(RV["CollectionEventID"].ToString(), out EventID))
                    return false;
                string ParsingMethod = DiversityCollection.LookupTable.LocalisationSystemParingMethod(LocSysID);

                if (ParsingMethod == "Coordinates")
                {
                    System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().CollectionEventLocalisationGeo.Select("GeographyAsString <> '' AND LocalisationSystemID = " + LocSysID.ToString() + " AND CollectionEventID = " + EventID.ToString());
                    if (rr.Length == 1)
                    {
                        string SQL = "UPDATE CollectionEventLocalisation SET Geography = geography::STGeomFromText('" + rr[0][2].ToString() + "', 4326) " +
                            "WHERE CollectionEventID = " + rr[0][0].ToString() + " AND LocalisationSystemID = " + rr[0][1].ToString() + " AND (CollectionEventLocalisation.Geography IS NULL OR CollectionEventLocalisation.Geography.ToString() LIKE 'POINT%')";
                        string Message = "";
                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message);
                        if (Message.Length > 0)
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("TryToSaveGeography()", SQL, Message);
                        this.textBoxGeography.Text = rr[0][2].ToString();
                        this.textBoxGeography.ReadOnly = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return false;
            }
            return true;
        }

        private void buttonCoordinatesConvert_Click(object sender, EventArgs e)
        {
            if (this._Source.Current != null)
            {
                DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionEventLocalisationRow R = (DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionEventLocalisationRow)this._iMainForm.DataSetCollectionSpecimen().CollectionEventLocalisation.Rows[this._Source.Position];
                DiversityCollection.Datasets.DataSetCollectionSpecimen DS = this._iMainForm.DataSetCollectionSpecimen();
                DiversityCollection.Forms.FormGeoConversion f = new DiversityCollection.Forms.FormGeoConversion(R, ref DS);// this._iMainForm.DataSetCollectionSpecimen());
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    this._iMainForm.setSpecimen();// this.buildOverviewHierarchyUnits();
                }
            }
        }

        private void comboBoxLocalisationRecordingMethod_DropDown(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                string LocalisationSystemID = R["LocalisationSystemID"].ToString();
                string SQL = "SELECT DISTINCT RecordingMethod " +
                    "FROM CollectionEventLocalisation " +
                    "WHERE LocalisationSystemID = " + LocalisationSystemID +
                    " AND RecordingMethod LIKE N'" + this.comboBoxLocalisationRecordingMethod.Text + "%' " +
                    "ORDER BY RecordingMethod";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                System.Data.DataTable dt = new DataTable();
                ad.Fill(dt);
                this.comboBoxLocalisationRecordingMethod.DataSource = dt;
                this.comboBoxLocalisationRecordingMethod.DisplayMember = "RecordingMethod";
                this.comboBoxLocalisationRecordingMethod.ValueMember = "RecordingMethod";
            }
            catch { }
        }

        private void buttonEditGeography_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                string WhereClause = "LocalisationSystemID = " + R["LocalisationSystemID"].ToString() + " AND CollectionEventID = " + R["CollectionEventID"].ToString();
                string SQL = "SELECT Geography.ToString() " +
                    "FROM CollectionEventLocalisation " +
                    "WHERE " + WhereClause;
                string Geography = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                DiversityWorkbench.Geography.FormEditGeography formEditGeography = new DiversityWorkbench.Geography.FormEditGeography("Edit the geography of the locality", "CollectionEventLocalisation", "Geography", Geography);
                formEditGeography.ShowDialog();
                if (formEditGeography.DialogResult == DialogResult.OK)
                {
                    if(formEditGeography.SaveGeography("CollectionEventLocalisation", "Geography", WhereClause))
                    {
                        this.setGeography();
                        //this.textBoxGeography.Text = formEditGeography.Geography();
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #region Custom units

        private void comboBoxGeographyLocationUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool Custom = false;
            this.comboBoxGeographyLocationUnits.BackColor = System.Drawing.SystemColors.Window;
            if (this.comboBoxGeographyLocationUnits.Visible)
            {
                if (this.comboBoxGeographyLocationUnits.Items.Count > 0)
                {
                    if (this.comboBoxGeographyLocationUnits.SelectedIndex > 0)
                    {
                        Custom = true;
                        this.comboBoxGeographyLocationUnits.BackColor = System.Drawing.Color.Aquamarine;
                    }
                }
            }
            this.tableLayoutPanelGeographyLocationCustom.Visible = Custom;
            this.tableLayoutPanelGeographyLocationStandard.Visible = !Custom;
            if (Custom)
            {
                this.setGeographyCustomUnitControls();
            }
        }

        private void setGeographyCustomUnitList(string ParsingMethodName)
        {
            this.comboBoxGeographyLocationUnits.Items.Clear();
            this.comboBoxGeographyLocationUnits.Text = "";
            string[] List = this.GeographyCustomUnitList(ParsingMethodName);
            foreach (string s in List)
            {
                this.comboBoxGeographyLocationUnits.Items.Add(s);
            }
            if (this.comboBoxGeographyLocationUnits.Items.Count > 0)
                this.comboBoxGeographyLocationUnits.SelectedIndex = 0;
        }

        private string[] GeographyCustomUnitList(string ParsingMethodName)
        {
            string[] List;

            switch (ParsingMethodName)
            {
                case "Altitude":
                case "Height":
                case "Depth":
                    List = new string[] { "text", MeasurementUnit.m.ToString(), MeasurementUnit.feet.ToString() };
                    break;
                //case "UTM":
                case "Coordinates":
                    List = new string[] { "text", MeasurementUnit.numeric.ToString(), MeasurementUnit.deg_min_sec.ToString() };
                    break;
                case "Exposition":
                    List = new string[] { "text", MeasurementUnit.degree.ToString(), MeasurementUnit.orientation.ToString() };
                    break;
                case "Slope":
                    List = new string[] { "text", MeasurementUnit.degree.ToString(), MeasurementUnit.percent.ToString() }; // };
                    break;
                default:
                    List = new string[] { };
                    break;
            }
            return List;
        }

        private void buttonGeographyCustomSave_Click(object sender, EventArgs e)
        {
            CultureInfo InvC = new CultureInfo("");
            double Value1 = 0;
            double Value2 = 0;
            bool OK1 = false;
            bool OK2 = false;
            string StringValue1 = "";
            string StringValue2 = "";
            string Ori = "";
            if (this._Source.Current != null)
            {
                try
                {
                    string GeographicalUnit = this.comboBoxGeographyLocationUnits.Text;
                    System.Data.DataRowView r = (System.Data.DataRowView)this._Source.Current;
                    System.Data.DataRow R = this._iMainForm.DataSetCollectionSpecimen().CollectionEventLocalisation.Rows[this._Source.Position];
                    R.BeginEdit();
                    r.BeginEdit();
                    switch (GeographicalUnit)
                    {
                        case "numeric":
                            StringValue1 = this.textBoxLocation1a.Text;
                            OK1 = double.TryParse(StringValue1, out Value1);
                            if (OK1)
                            {
                                r["Location1"] = Value1.ToString();
                                if (r["AverageLongitudeCache"].Equals(System.DBNull.Value) || Value1 != 0)
                                    r["AverageLongitudeCache"] = Value1;
                            }

                            StringValue2 = this.textBoxLocation2a.Text;
                            OK2 = double.TryParse(StringValue2, out Value2);
                            if (OK2)
                            {
                                r[ValueColumn] = Value2.ToString();
                                if (r["AverageLatitudeCache"].Equals(System.DBNull.Value) || Value2 != 0)
                                    r["AverageLatitudeCache"] = Value2;
                            }
                            break;

                        case "deg_min_sec":
                            int DegNS = 0;
                            int DegEW = 0;
                            int MinNS = 0;
                            int MinEW = 0;
                            int PrefixNS = 1;
                            int PrefixEW = 1;
                            double SecNS = 0;
                            double SecEW = 0;

                            bool OK = true;
                            string Deg = this.textBoxLocation1a.Text.ToString();
                            Deg = Deg.Replace(" ", "");
                            if (Deg.Trim().StartsWith("-")) PrefixEW = -1;
                            if (Deg.Length == 0) Deg = "0";
                            if (!int.TryParse(Deg, out DegEW)) OK = false;
                            if (OK) DegEW = System.Math.Abs(DegEW);
                            string Min = this.textBoxLocation1b.Text.ToString();
                            if (Min.Length == 0) Min = "0";
                            if (!int.TryParse(Min, out MinEW)) OK = false;
                            string Sec = this.textBoxLocation1c.Text.ToString();
                            if (Sec.Length == 0) Sec = "0";
                            OK = double.TryParse(Sec, NumberStyles.Float, InvC, out SecEW);
                            if (!OK)
                                OK = double.TryParse(Sec.Replace(".", ","), NumberStyles.Float, InvC, out SecEW);
                            if (!OK)
                                OK = double.TryParse(Sec.Replace(",", "."), NumberStyles.Float, InvC, out SecEW);
                            if (OK)
                            {
                                if (!this.convertFromDegMinSecToNumeric(ref Value1, DegEW, MinEW, SecEW))
                                    OK = false;
                            }

                            Deg = this.textBoxLocation2a.Text.ToString();
                            Deg = Deg.Replace(" ", "");
                            if (Deg.Trim().StartsWith("-")) PrefixNS = -1;
                            if (Deg.Length == 0) Deg = "0";
                            if (!int.TryParse(Deg, out DegNS)) OK = false;
                            if (OK) DegNS = System.Math.Abs(DegNS);
                            Min = this.textBoxLocation2b.Text.ToString();
                            if (Min.Length == 0) Min = "0";
                            if (!int.TryParse(Min, out MinNS)) OK = false;
                            Sec = this.textBoxLocation2c.Text.ToString();
                            if (Sec.Length == 0) Sec = "0";
                            OK = double.TryParse(Sec, NumberStyles.Float, InvC, out SecNS);
                            if (!OK)
                                OK = double.TryParse(Sec.Replace(".", ","), NumberStyles.Float, InvC, out SecNS);
                            if (!OK)
                                OK = double.TryParse(Sec.Replace(",", "."), NumberStyles.Float, InvC, out SecNS);
                            //Sec = Sec.Replace(".", ",");
                            //if (Sec.Length == 0) Sec = "0";
                            //if (!double.TryParse(Sec, out SecNS)) OK = false;
                            if (OK)
                            {
                                //if (!this.convertFromDegMinSecToNumeric(ref Value2, int.Parse(Deg), int.Parse(Min), double.Parse(Sec)))
                                if (!this.convertFromDegMinSecToNumeric(ref Value2, DegNS, MinNS, SecNS))
                                    OK = false;
                            }
                            if (OK)
                            {
                                R["Location1"] = (Value1 * PrefixEW).ToString();
                                R[ValueColumn] = (Value2 * PrefixNS).ToString();
                                R["AverageLatitudeCache"] = Value2 * PrefixNS;
                                R["AverageLongitudeCache"] = Value1 * PrefixEW;
                                Ori = "Lat.: " + this.textBoxLocation2a.Text + "° ";
                                if (this.textBoxLocation2b.Text.Length > 0) Ori += this.textBoxLocation2b.Text + "' ";
                                if (this.textBoxLocation2c.Text.Length > 0) Ori += this.textBoxLocation2c.Text + "'' ";
                                Ori += " Long.: " + this.textBoxLocation1a.Text + "° ";
                                if (this.textBoxLocation1b.Text.Length > 0) Ori += this.textBoxLocation1b.Text + "' ";
                                if (this.textBoxLocation1c.Text.Length > 0) Ori += this.textBoxLocation1c.Text + "'' ";
                                R["LocationAccuracy"] = DiversityWorkbench.GeoFunctions.AccuracyFromDegMinSec(DegNS, MinNS, SecNS, DegEW, MinEW, SecEW);
                                this.GeographyLocationSetNotesDateResponsible(R, Ori);
                            }
                            break;

                        case "degree":
                            int degFrom = 0;
                            int degTo = 0;
                            int degAv;
                            int degAcc = 0;
                            bool isDegFrom = false;
                            bool isDegTo = false;
                            if (int.TryParse(this.textBoxLocation1a.Text, out degFrom))
                            {
                                R["Location1"] = degFrom.ToString();
                                isDegFrom = true;
                            }
                            if (int.TryParse(this.textBoxLocation2a.Text, out degTo))
                            {
                                R[ValueColumn] = degTo.ToString();
                                isDegTo = true;
                            }
                            if (isDegFrom || isDegTo)
                            {
                                if (isDegFrom && isDegTo)
                                {
                                    degAv = (degFrom + degTo) / 2;
                                    R["AverageAltitudeCache"] = degAv;
                                }
                                else
                                {
                                    degAv = degFrom + degTo;
                                    R["AverageAltitudeCache"] = degAv;
                                }
                                R["LocationAccuracy"] = DiversityWorkbench.GeoFunctions.AccuracyFromDegree(degFrom, degTo).ToString() + " °";
                            }
                            this.GeographyLocationSetNotesDateResponsible(R, "");
                            break;
                        case "orientation":
                            double dValFrom = 0;
                            double dValTo = 0;
                            bool isFromOri = false;
                            bool isToOri = false;
                            string FromOri = this.comboBoxLocation1a.Text;
                            string ToOri = this.comboBoxLocation2a.Text;
                            if (this.convertFromOrientationToDegree(ref FromOri, ref dValFrom))
                            {
                                R["Location1"] = dValFrom;
                                isFromOri = true;
                            }
                            if (this.convertFromOrientationToDegree(ref ToOri, ref dValTo))
                            {
                                R[ValueColumn] = dValTo;
                                isToOri = true;
                            }
                            if (isFromOri || isToOri)
                            {
                                if (isFromOri) Ori = FromOri;
                                if (isFromOri && isToOri) Ori += " - ";
                                if (isToOri) Ori += ToOri;
                                this.GeographyLocationSetNotesDateResponsible(R, Ori);
                                R["LocationAccuracy"] = DiversityWorkbench.GeoFunctions.AccuracyFromOrientation(this.comboBoxLocation1a.Text, this.comboBoxLocation2a.Text);
                            }
                            break;
                        case "percent":
                            double pFrom = 0;
                            double pTo = 0;
                            double pmValFrom;
                            double pmValTo;
                            double pfAcc;
                            double pmAcc;
                            bool pisFrom = false;
                            bool pisTo = false;
                            if (double.TryParse(this.textBoxLocation1a.Text, out pFrom))
                            {
                                pmValFrom = this.convertUnit(pFrom, MeasurementUnit.percent, MeasurementUnit.degree);
                                R["Location1"] = pmValFrom;
                                pisFrom = true;
                            }
                            if (double.TryParse(this.textBoxLocation2a.Text, out pTo))
                            {
                                pmValTo = this.convertUnit(pTo, MeasurementUnit.percent, MeasurementUnit.degree);
                                R[ValueColumn] = pmValTo;
                                pisTo = true;
                            }
                            if (pisFrom || pisTo)
                            {
                                if (pisFrom) Ori = pFrom.ToString();
                                if (pisFrom && pisTo) Ori += " - ";
                                if (pisTo) Ori += pTo.ToString();
                                Ori += " percent";
                                this.GeographyLocationSetNotesDateResponsible(R, Ori);
                            }
                            if (pFrom != 0 || pTo != 0)
                            {
                                pfAcc = DiversityWorkbench.GeoFunctions.AccuracyFromValues(pFrom, pTo);
                                pmAcc = this.convertUnit((double)pfAcc, MeasurementUnit.percent, MeasurementUnit.degree);
                                R["LocationAccuracy"] = pmAcc + " °";
                            }
                            break;
                        case "feet":
                        case "foot":
                            double fFrom = 0;
                            double fTo = 0;
                            double mValFrom;
                            double mValTo;
                            double fAcc;
                            double mAcc;
                            bool isFrom = false;
                            bool isTo = false;
                            if (double.TryParse(this.textBoxLocation1a.Text, out fFrom))
                            {
                                mValFrom = this.convertUnit(fFrom, MeasurementUnit.feet, MeasurementUnit.m);
                                R["Location1"] = mValFrom;
                                isFrom = true;
                            }
                            if (double.TryParse(this.textBoxLocation2a.Text, out fTo))
                            {
                                mValTo = this.convertUnit(fTo, MeasurementUnit.feet, MeasurementUnit.m);
                                R[ValueColumn] = mValTo;
                                isTo = true;
                            }
                            if (isFrom || isTo)
                            {
                                if (isFrom) Ori = fFrom.ToString();
                                if (isFrom && isTo) Ori += " - ";
                                if (isTo) Ori += fTo.ToString();
                                Ori += " feet";
                                this.GeographyLocationSetNotesDateResponsible(R, Ori);
                            }
                            if (fFrom != 0 || fTo != 0)
                            {
                                fAcc = DiversityWorkbench.GeoFunctions.AccuracyFromValues(fFrom, fTo);
                                mAcc = this.convertUnit((double)fAcc, MeasurementUnit.feet, MeasurementUnit.m);
                                R["LocationAccuracy"] = mAcc + " m";
                            }
                            goto case "AverageAltitude";
                            break;
                        case "m":
                            double From = 0;
                            double To = 0;
                            double Av;
                            decimal ValFrom;
                            decimal ValTo;
                            double Acc = 0;
                            if (double.TryParse(this.textBoxLocation1a.Text, out From))
                            {
                                ValFrom = (decimal)From;
                                ValFrom = Math.Round(ValFrom, 0);
                                R["Location1"] = ValFrom.ToString();
                            }
                            if (double.TryParse(this.textBoxLocation2a.Text, out To))
                            {
                                ValTo = (decimal)To;
                                ValTo = Math.Round(ValTo, 0);
                                R[ValueColumn] = ValTo.ToString();
                            }
                            if (From != 0 || To != 0)
                            {
                                if (From != 0 && To != 0)
                                {
                                    Av = (From + To) / 2;
                                    R["AverageAltitudeCache"] = Av;
                                }
                                else
                                {
                                    Av = From + To;
                                    R["AverageAltitudeCache"] = Av;
                                }
                                R["LocationAccuracy"] = DiversityWorkbench.GeoFunctions.AccuracyFromValues(From, To) + " m";
                            }
                            this.GeographyLocationSetNotesDateResponsible(R, "");
                            goto case "AverageAltitude";
                            break;
                        case "AverageAltitude":
                            int LocSysID;
                            if (int.TryParse(R["LocalisationSystemID"].ToString(), out LocSysID))
                            {
                                string LocalisationSystem = DiversityCollection.LookupTable.LocalisationSystemName(LocSysID);
                                if (LocalisationSystem == "Altitude (mNN)")
                                {
                                    this.TryToSaveAndCalculateAverageForLocalisation(true);
                                }
                            }
                            break;
                        default:
                            this.comboBoxGeographyLocationUnits.Visible = true;
                            break;
                    }
                    R.EndEdit();
                    this._iMainForm.setSpecimen(); //DiversityCollection.HierarchyNode N = (DiversityCollection.HierarchyNode)this.treeViewOverviewHierarchy.SelectedNode;
                    //N.setText();
                }
                catch { }
            }
        }

        private void GeographyLocationSetNotesDateResponsible(System.Data.DataRow R, string OriginalValueNotes)
        {
            string LocationNotes = R["LocationNotes"].ToString();
            if (OriginalValueNotes.Length > 0)
                OriginalValueNotes = this.GeoOriValStart + OriginalValueNotes + this.GeoOriValEnd;
            if (LocationNotes.IndexOf(this.GeoOriValStart) > -1
                && LocationNotes.IndexOf(this.GeoOriValEnd) > LocationNotes.IndexOf(this.GeoOriValStart))
            {
                OriginalValueNotes = LocationNotes.Substring(0, LocationNotes.IndexOf(this.GeoOriValStart)) +
                    OriginalValueNotes +
                    LocationNotes.Substring(LocationNotes.IndexOf(this.GeoOriValEnd) + 1);
            }
            else
            {
                if (LocationNotes.Length > 0)
                    OriginalValueNotes += " " + LocationNotes.Trim();
            }
            R["LocationNotes"] = OriginalValueNotes.Trim();
            R["DeterminationDate"] = System.DateTime.Now.ToShortDateString();
            if (DiversityCollection.Specimen.DefaultUseLocalisationResponsible)
            {
                R["ResponsibleName"] = DiversityCollection.Specimen.DefaultResponsibleName;// DiversityWorkbench.User.CurrentUserName;
                R["ResponsibleAgentURI"] = DiversityCollection.Specimen.DefaultResponsibleURI;
            }
        }

        private void setGeographyCustomUnitControls()
        {
            CultureInfo InvC = new CultureInfo("");

            double Value1 = 0;
            double Value2 = 0;
            bool OK1 = false;
            bool OK2 = false;
            string StringValue1 = "";
            string StringValue2 = "";
            if (this._Source.Current != null)
            {
                System.Data.DataRowView r = (System.Data.DataRowView)this._Source.Current;
                if (!r["Location1"].Equals(System.DBNull.Value) && r["Location1"].ToString().Length > 0)
                {
                    OK1 = double.TryParse(r["Location1"].ToString(), NumberStyles.Float, InvC, out Value1);
                    if (!OK1)
                        OK1 = double.TryParse(r["Location1"].ToString().Replace(".", ","), NumberStyles.Float, InvC, out Value1);
                    if (!OK1)
                        OK1 = double.TryParse(r["Location1"].ToString().Replace(",", "."), NumberStyles.Float, InvC, out Value1);
                    StringValue1 = r["Location1"].ToString();
                }
                if (!r[ValueColumn].Equals(System.DBNull.Value) && r[ValueColumn].ToString().Length > 0)
                {
                    OK2 = double.TryParse(r[ValueColumn].ToString(), NumberStyles.Float, InvC, out Value2);
                    if (!OK2)
                        OK2 = double.TryParse(r[ValueColumn].ToString().Replace(".", ","), NumberStyles.Float, InvC, out Value2);
                    if (!OK2)
                        OK2 = double.TryParse(r[ValueColumn].ToString().Replace(",", "."), NumberStyles.Float, InvC, out Value2);
                    StringValue2 = r[ValueColumn].ToString();
                }
            }
            string GeographicalUnit = this.comboBoxGeographyLocationUnits.Text;

            this.textBoxLocation1a.TextAlign = HorizontalAlignment.Center;
            this.textBoxLocation2a.TextAlign = HorizontalAlignment.Center;
            this.textBoxLocation1b.TextAlign = HorizontalAlignment.Center;
            this.textBoxLocation2b.TextAlign = HorizontalAlignment.Center;
            this.textBoxLocation1a.Visible = true;
            this.textBoxLocation2a.Visible = true;
            this.comboBoxLocation2a.Visible = false;
            this.comboBoxLocation1a.Visible = false;

            switch (GeographicalUnit)
            {
                case "numeric":
                    this.textBoxLocation1b.Visible = false;
                    this.textBoxLocation1c.Visible = false;
                    this.textBoxLocation2b.Visible = false;
                    this.textBoxLocation2c.Visible = false;
                    this.buttonGeographyCustomSave.Visible = true;
                    if (OK1) this.textBoxLocation1a.Text = Value1.ToString(); else this.textBoxLocation1a.Text = "?";
                    if (OK2) this.textBoxLocation2a.Text = Value2.ToString(); else this.textBoxLocation2a.Text = "?";
                    break;
                case "deg_min_sec":
                    int W = this.tableLayoutPanelGeographyLocationCustom.Width - this.labelLocation1Custom.Width - this.labelLocation2Custom.Width - 40;
                    int Wc = (int)((double)W * 0.2);
                    this.textBoxLocation1c.Width = Wc;
                    this.textBoxLocation2c.Width = Wc;
                    this.toolTip.SetToolTip(this.textBoxLocation1a, "Degree EW");
                    this.toolTip.SetToolTip(this.textBoxLocation2a, "Degree NS");
                    this.toolTip.SetToolTip(this.textBoxLocation1b, "Minutes EW");
                    this.toolTip.SetToolTip(this.textBoxLocation1c, "Seconds EW");
                    this.toolTip.SetToolTip(this.textBoxLocation2b, "Minutes NS");
                    this.toolTip.SetToolTip(this.textBoxLocation2c, "Seconds NS");
                    this.textBoxLocation1a.TextAlign = HorizontalAlignment.Right;
                    this.textBoxLocation2a.TextAlign = HorizontalAlignment.Right;
                    this.textBoxLocation1b.TextAlign = HorizontalAlignment.Center;
                    this.textBoxLocation2b.TextAlign = HorizontalAlignment.Center;
                    this.textBoxLocation1b.Visible = true;
                    this.textBoxLocation1c.Visible = true;
                    this.textBoxLocation2b.Visible = true;
                    this.textBoxLocation2c.Visible = true;
                    this.buttonGeographyCustomSave.Visible = true;
                    if (OK1)
                    {
                        int Prefix = 1;
                        int Deg = 0;
                        int Min = 0;
                        double Sek = 0;
                        this.convertFromNumericToDegMinSec(Value1, ref Prefix, ref Deg, ref Min, ref Sek);
                        string sDeg = Deg.ToString();
                        if (Prefix < 0) sDeg = "- " + sDeg;
                        this.textBoxLocation1a.Text = sDeg;
                        if (Min > 0) this.textBoxLocation1b.Text = Min.ToString();
                        else
                        {
                            if (Sek > 0) this.textBoxLocation1b.Text = "0";
                            else this.textBoxLocation1b.Text = "";
                        }
                        if (Sek > 0) this.textBoxLocation1c.Text = Sek.ToString();
                        else this.textBoxLocation1c.Text = "";
                    }
                    else
                    {
                        this.textBoxLocation1a.Text = "";
                        this.textBoxLocation1b.Text = "";
                        this.textBoxLocation1c.Text = "";
                    }
                    if (OK2)
                    {
                        int Prefix = 1;
                        int Deg = 0;
                        int Min = 0;
                        double Sek = 0;
                        this.convertFromNumericToDegMinSec(Value2, ref Prefix, ref Deg, ref Min, ref Sek);
                        string sDeg = Deg.ToString();
                        if (Prefix < 0) sDeg = "- " + sDeg;
                        this.textBoxLocation2a.Text = sDeg;
                        if (Min > 0) this.textBoxLocation2b.Text = Min.ToString();
                        else
                        {
                            if (Sek > 0) this.textBoxLocation2b.Text = "0";
                            else this.textBoxLocation2b.Text = "";
                        }
                        if (Sek > 0) this.textBoxLocation2c.Text = Sek.ToString();
                        else this.textBoxLocation2c.Text = "";
                    }
                    else
                    {
                        this.textBoxLocation2a.Text = "";
                        this.textBoxLocation2b.Text = "";
                        this.textBoxLocation2c.Text = "";
                    }
                    break;
                case "degree":
                    this.textBoxLocation1b.Visible = false;
                    this.textBoxLocation1c.Visible = false;
                    this.textBoxLocation2b.Visible = false;
                    this.textBoxLocation2c.Visible = false;
                    this.buttonGeographyCustomSave.Visible = true;
                    if (OK1) this.textBoxLocation1a.Text = Value1.ToString();
                    else
                    {
                        if (StringValue1.Length < 4)
                        {
                            double Deg = 0;
                            string Ori = StringValue1;
                            if (this.convertFromOrientationToDegree(ref Ori, ref Deg))
                                this.textBoxLocation1a.Text = Deg.ToString();
                        }
                    }
                    if (OK2) this.textBoxLocation2a.Text = Value2.ToString();
                    else
                    {
                        if (StringValue2.Length < 4)
                        {
                            double Deg = 0;
                            string Ori = StringValue2;
                            if (this.convertFromOrientationToDegree(ref Ori, ref Deg))
                                this.textBoxLocation2a.Text = Deg.ToString();
                        }
                    }
                    break;
                case "orientation":
                    this.textBoxLocation1a.Visible = false;
                    this.textBoxLocation1b.Visible = false;
                    this.textBoxLocation1c.Visible = false;
                    this.textBoxLocation2a.Visible = false;
                    this.textBoxLocation2b.Visible = false;
                    this.textBoxLocation2c.Visible = false;
                    this.comboBoxLocation1a.Width = (int)(80 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                    this.comboBoxLocation2a.Width = (int)(80 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                    this.comboBoxLocation1a.Visible = true;
                    this.comboBoxLocation2a.Visible = true;
                    this.comboBoxLocation1a.Items.Clear();
                    this.comboBoxLocation2a.Items.Clear();
                    this.comboBoxLocation1a.Items.Add("");
                    this.comboBoxLocation2a.Items.Add("");
                    for (int i = 0; i < this.NamedOrientation.Length; i++)
                    {
                        this.comboBoxLocation1a.Items.Add(this.NamedOrientation[i]);
                        this.comboBoxLocation2a.Items.Add(this.NamedOrientation[i]);
                    }
                    this.buttonGeographyCustomSave.Visible = true;
                    string OrientFrom = this.convertFromDegreeToOrientation(StringValue1);
                    string OrientTo = this.convertFromDegreeToOrientation(StringValue2);
                    for (int i = 0; i < this.NamedOrientation.Length; i++)
                    {
                        if (this.NamedOrientation[i] == OrientFrom)
                        {
                            this.comboBoxLocation1a.SelectedIndex = i + 1;
                            break;
                        }
                    }
                    for (int i = 0; i < this.NamedOrientation.Length; i++)
                    {
                        if (this.NamedOrientation[i] == OrientTo)
                        {
                            this.comboBoxLocation2a.SelectedIndex = i + 1;
                            break;
                        }
                    }
                    break;
                case "percent":
                    this.textBoxLocation1b.Visible = false;
                    this.textBoxLocation1c.Visible = false;
                    this.textBoxLocation2b.Visible = false;
                    this.textBoxLocation2c.Visible = false;
                    this.buttonGeographyCustomSave.Visible = true;
                    this.textBoxLocation1a.Text = this.convertUnit(StringValue1, MeasurementUnit.degree, MeasurementUnit.percent);
                    this.textBoxLocation2a.Text = this.convertUnit(StringValue2, MeasurementUnit.degree, MeasurementUnit.percent);
                    break;
                case "feet":
                case "foot":
                    this.textBoxLocation1b.Visible = false;
                    this.textBoxLocation1c.Visible = false;
                    this.textBoxLocation2b.Visible = false;
                    this.textBoxLocation2c.Visible = false;
                    this.buttonGeographyCustomSave.Visible = true;
                    this.textBoxLocation1a.Text = this.convertUnit(StringValue1, MeasurementUnit.m, MeasurementUnit.feet);
                    this.textBoxLocation2a.Text = this.convertUnit(StringValue2, MeasurementUnit.m, MeasurementUnit.feet);
                    break;
                case "m":
                    this.textBoxLocation1b.Visible = false;
                    this.textBoxLocation1c.Visible = false;
                    this.textBoxLocation2b.Visible = false;
                    this.textBoxLocation2c.Visible = false;
                    this.buttonGeographyCustomSave.Visible = true;
                    this.textBoxLocation1a.Text = this.convertUnit(StringValue1, MeasurementUnit.m, MeasurementUnit.m);
                    this.textBoxLocation2a.Text = this.convertUnit(StringValue2, MeasurementUnit.m, MeasurementUnit.m);
                    break;
                default:
                    this.textBoxLocation1a.Visible = true;
                    this.textBoxLocation2a.Visible = true;
                    this.buttonGeographyCustomSave.Visible = false;
                    this.comboBoxGeographyLocationUnits.Visible = true;
                    break;
            }
        }

        #region Conversions

        /// <summary>
        /// converts all values to or from SI unit like m, sec
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="UnitSource"></param>
        /// <param name="UnitTarget"></param>
        /// <returns></returns>
        private string convertUnit(string Value, MeasurementUnit UnitSource, MeasurementUnit UnitTarget)
        {
            string Result = "?";
            double Source = 0;
            decimal dResult = 0;
            double Target = 0;
            bool OK = false;
            switch (UnitSource)
            {
                case MeasurementUnit.feet:
                    OK = double.TryParse(Value, out Source);
                    if (OK)
                    {
                        Target = Source / 3.2808;
                        Result = Target.ToString();
                    }
                    break;
                case MeasurementUnit.mile:
                    OK = double.TryParse(Value, out Source);
                    if (OK)
                    {
                        Target = Source * 1609;
                        Result = Target.ToString();
                    }
                    break;
                case MeasurementUnit.km:
                    OK = double.TryParse(Value, out Source);
                    if (OK)
                    {
                        Target = Source * 1000.0;
                        Result = Target.ToString();
                    }
                    break;
                case MeasurementUnit.m:
                    switch (UnitTarget)
                    {
                        case MeasurementUnit.feet:
                            OK = double.TryParse(Value, out Source);
                            if (OK)
                            {
                                Target = Source * 3.2808;
                                goto default;
                            }
                            break;
                        case MeasurementUnit.mile:
                            OK = double.TryParse(Value, out Source);
                            if (OK)
                            {
                                Target = Source / 1609;
                                goto default;
                            }
                            break;
                        case MeasurementUnit.km:
                            OK = double.TryParse(Value, out Source);
                            if (OK)
                            {
                                Target = Source / 1000.0;
                                goto default;
                            }
                            break;
                        default:
                            if (Target == 0)
                            {
                                OK = double.TryParse(Value, out Source);
                                if (OK)
                                {
                                    Target = Source;
                                }
                                else return "";
                            }
                            dResult = (decimal)Target;
                            dResult = System.Math.Round(dResult, 9);
                            Target = (double)dResult;
                            Result = Target.ToString();
                            break;
                    }
                    break;
                case MeasurementUnit.percent:
                    switch (UnitTarget)
                    {
                        case MeasurementUnit.degree:
                            OK = double.TryParse(Value, out Source);
                            if (OK)
                            {
                                double tan = Source / 100.0;
                                double result = System.Math.Atan(tan);
                                double angle = result * 180 / Math.PI;
                                Result = angle.ToString();
                            }
                            break;
                    }
                    break;
                case MeasurementUnit.degree:
                    switch (UnitTarget)
                    {
                        case MeasurementUnit.percent:
                            OK = double.TryParse(Value, out Source);
                            if (OK)
                            {
                                Target = System.Math.Tan(Source * (Math.PI / 180.0)) * 100.0;
                                Result = Target.ToString();
                            }
                            break;
                        case MeasurementUnit.orientation:
                            OK = double.TryParse(Value, out Source);
                            if (OK)
                            {
                                int ori = (int)((Source + 11.25) / 22.5);
                                Result = this.NamedOrientation[ori];
                            }
                            break;
                    }
                    break;
                default:
                    OK = double.TryParse(Value, out Source);
                    if (OK)
                    {
                        Result = Source.ToString();
                    }
                    break;
            }
            return Result;
        }

        /// <summary>
        /// converts all values to or from SI unit like m, sec
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="UnitSource"></param>
        /// <param name="UnitTarget"></param>
        /// <returns></returns>
        private double convertUnit(double Value, MeasurementUnit UnitSource, MeasurementUnit UnitTarget)
        {
            switch (UnitSource)
            {
                case MeasurementUnit.feet:
                    return Value / 3.2808;
                    break;
                case MeasurementUnit.mile:
                    return Value * 1609;
                    break;
                case MeasurementUnit.km:
                    return Value * 1000.0;
                    break;
                case MeasurementUnit.m:
                    switch (UnitTarget)
                    {
                        case MeasurementUnit.feet:
                            return Value * 3.2808;
                            break;
                        case MeasurementUnit.mile:
                            return Value / 1609;
                            break;
                        case MeasurementUnit.km:
                            return Value / 1000.0;
                            break;
                        default:
                            return Value;
                            break;
                    }
                    break;
                case MeasurementUnit.percent:
                    switch (UnitTarget)
                    {
                        case MeasurementUnit.degree:
                            double tan = Value / 100.0;
                            double result = System.Math.Atan(tan);
                            double angle = result * 180 / Math.PI;
                            return angle;
                            //return System.Math.Atan(Value, 100.0);
                            break;
                    }
                    break;
                case MeasurementUnit.degree:
                    switch (UnitTarget)
                    {
                        case MeasurementUnit.percent:
                            return System.Math.Tan(Value * (Math.PI / 180.0)) * 100.0;
                            break;
                    }
                    break;
                default:
                    return Value;
                    break;
            }
            return Value;
        }

        private bool convertFromNumericToDegMinSec(double Numeric, ref int Prefix, ref int Deg, ref int Min, ref double Sec)
        {
            bool OK = true;
            try
            {
                //Prefix
                if (Numeric < 0) Prefix = -1;
                else Prefix = 1;
                // Deg
                Deg = (int)Numeric * Math.Sign(Numeric);
                // Min
                double MinD = Numeric * Math.Sign(Numeric) - Deg;
                MinD = MinD * 0.6;
                MinD = ((MinD * 100));
                Min = (int)MinD;
                // Sec
                double SecD = MinD - Min;
                SecD *= 60;
                decimal dSec = (decimal)SecD;
                dSec = Math.Round(dSec, 9);
                SecD = (double)dSec;
                if (SecD >= 60)
                {
                    SecD -= 60;
                    Min += 1;
                }
                decimal dSecD = (decimal)SecD;
                dSecD = System.Math.Round(dSecD, 9);
                SecD = (double)dSecD;
                Sec = SecD;
                Deg = System.Math.Abs(Deg);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        private bool convertFromDegMinSecToNumeric(ref double Numeric, int Deg, int Min, double Sec)
        {
            try
            {
                if (Deg * Math.Sign(Deg) > 180 || Min >= 60 || Min < 0 || Sec < 0 || Sec >= 60)
                {
                    System.Windows.Forms.MessageBox.Show(DiversityCollection.Forms.FormCollectionSpecimenText.Wrong_entry);
                    return false;
                }
                double dDeg = (double)Deg * Math.Sign(Deg);
                double dMin = (double)Min / (double)60 * Math.Sign(Min);
                double dSec = Sec / 3600;
                Numeric = dDeg + dMin + dSec;
                if (Deg != 0)
                    Numeric = Numeric * Math.Sign(Deg);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return false;
            }
            return true;
        }

        private bool convertFromOrientationToDegree(ref string Orientation, ref double Degree)
        {
            bool OK = true;
            int i;
            try
            {
                if (Orientation.Length > 0 && Orientation.Length < 4)
                {
                    Degree = 0F;
                    Orientation = Orientation.ToUpper();
                    for (i = 0; i < Orientation.Length; i++)
                    {
                        if (Orientation[i].ToString() == "N" || Orientation[i].ToString() == "W" || Orientation[i].ToString() == "E" || Orientation[i].ToString() == "S" || Orientation[i].ToString() == "O")
                        {
                            switch (Orientation[i].ToString())
                            {
                                case "N":
                                    if (Orientation.IndexOf("W") > 0)
                                        Degree += 360.0F;
                                    else
                                        Degree += 0F;
                                    break;
                                case "E":
                                case "O":
                                    Degree += 90.0F;
                                    break;
                                case "S":
                                    Degree += 180.0F;
                                    break;
                                case "W":
                                    Degree += 270.0F;
                                    break;
                                default:
                                    OK = false;
                                    break;
                            }
                        }
                        else
                        {
                            Orientation = "?";
                            break;
                        }
                    }
                    if (Orientation != "?")
                        Degree = Degree / i;
                }
            }
            catch
            {
                OK = false;
            }
            return OK;
        }

        private string convertFromDegreeToOrientation(string Degree)
        {
            string O = "?";
            try
            {
                float D;
                bool OK = float.TryParse(Degree, out D);
                if (OK)
                {
                    int ori = (int)((D + 11.25F) / 22.5F);
                    O = this.NamedOrientation[ori];
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return O;
        }

        #endregion

        #endregion

        #endregion

    }

}
