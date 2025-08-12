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
    public partial class UserControl_GeoAnalysis : UserControl__Data
    {
        #region Construction

        public UserControl_GeoAnalysis(
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

        #region Control

        private void initControl()
        {
            this.textBoxGeoAnalysisNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Notes", true));
            this.dateTimePickerGeoAnalysisDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this._Source, "AnalysisDate", true));

            DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
            this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryGeoAnalysisResponsible, A, "IdentificationUnitGeoAnalysis", "ResponsibleName", "ResponsibleAgentURI", this._Source);

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();
        }

        #region Analysis

        public void setGeoAnalysisText(string Coordinates, string Geography)
        {
            if (Coordinates.Length > 0 && Coordinates.IndexOf(' ') > -1)
            {
                string[] LatLong = Coordinates.Split(new char[] { ' ' });
                this.labelLatitude.Text = LatLong[2].Replace(")", "");
                this.labelLongitude.Text = LatLong[1].Replace("(", "");
            }
            else
            {
                this.labelLatitude.Text = "";
                this.labelLongitude.Text = "";
            }
            this.textBoxGeoAnalysisGeography.Text = Geography;
        }

        private void buttonGeoAnalysisSet_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView RA
                    = (System.Data.DataRowView)this._Source.Current;

                System.Data.DataRow[] RRGA = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitGeoAnalysis.Select(
                    "IdentificationUnitID = " + RA["IdentificationUnitID"].ToString() +
                    " AND CollectionSpecimenID = " + RA["CollectionSpecimenID"].ToString());// +

                System.Data.DataRow[] RR = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitGeoAnalysisGeo.Select(
                    "IdentificationUnitID = " + RA["IdentificationUnitID"].ToString() +
                    " AND CollectionSpecimenID = " + RA["CollectionSpecimenID"].ToString());

                if (RR.Length + 1 == RRGA.Length)
                {
                    DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitGeoAnalysisGeoRow R = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitGeoAnalysisGeo.NewIdentificationUnitGeoAnalysisGeoRow();
                    R.IdentificationUnitID = int.Parse(RA["IdentificationUnitID"].ToString());
                    R.CollectionSpecimenID = int.Parse(RA["CollectionSpecimenID"].ToString());
                    R.AnalysisDate = System.DateTime.Parse(RA["AnalysisDate"].ToString());
                    this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitGeoAnalysisGeo.Rows.Add(R);
                    this.setGeographyViaGisEditor(R);
                }

                else if (RR.Length == 1 && RRGA.Length == 1)
                {
                    this.setGeographyViaGisEditor((DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitGeoAnalysisGeoRow)RR[0]);
                }
                else if (RR.Length == 0)
                {
                    DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitGeoAnalysisGeoRow R = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitGeoAnalysisGeo.NewIdentificationUnitGeoAnalysisGeoRow();
                    R.IdentificationUnitID = int.Parse(RA["IdentificationUnitID"].ToString());
                    R.CollectionSpecimenID = int.Parse(RA["CollectionSpecimenID"].ToString());
                    R.AnalysisDate = System.DateTime.Parse(RA["AnalysisDate"].ToString());
                    this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitGeoAnalysisGeo.Rows.Add(R);
                    this.setGeographyViaGisEditor(R);
                }
                else
                {
                    bool RowFound = false;
                    System.DateTime AnalysisDate;
                    if (System.DateTime.TryParse(RA["AnalysisDate"].ToString(), out AnalysisDate))
                    {
                        foreach (System.Data.DataRow Ra in RR)
                        {
                            System.DateTime CurrentAnalysisDate;
                            if (System.DateTime.TryParse(Ra["AnalysisDate"].ToString(), out CurrentAnalysisDate))
                            {
                                if (CurrentAnalysisDate == AnalysisDate)
                                {
                                    this.setGeographyViaGisEditor((DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitGeoAnalysisGeoRow)Ra);
                                    RowFound = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!RowFound)
                    {
                        System.Data.DataRow[] RRmissing = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitGeoAnalysisGeo.Select("", "AnalysisDate DESC");
                        this.setGeographyViaGisEditor((DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitGeoAnalysisGeoRow)RRmissing[0]);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Geography

        private void setGeographyViaGisEditor(DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitGeoAnalysisGeoRow R)
        {
            try
            {
                GeoObject GO = new GeoObject();
                string Geography = "";
                // getting a geography to start with
                if (!R["GeographyAsString"].Equals(System.DBNull.Value))
                    Geography = R.GeographyAsString.ToString();
                else
                {
                    System.Data.DataRow[] RR = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitGeoAnalysisGeo.Select("GeographyAsString <> ''", "");
                    if (RR.Length > 0)
                    {
                        Geography = RR[0]["GeographyAsString"].ToString();
                    }
                    else if (this._iMainForm.DataSetCollectionSpecimen().CollectionEventLocalisationGeo.Rows.Count > 0
                        && !this._iMainForm.DataSetCollectionSpecimen().CollectionEventLocalisationGeo.Rows[0]["GeographyAsString"].Equals(System.DBNull.Value))
                    {
                        Geography = this._iMainForm.DataSetCollectionSpecimen().CollectionEventLocalisationGeo.Rows[0]["GeographyAsString"].ToString();
                    }
                    else if (DiversityWorkbench.Settings.GeoLatitude != 0 && DiversityWorkbench.Settings.GeoLongitude != 0)
                    {
                        Geography = "POINT(" + DiversityWorkbench.Settings.GeoLongitude.ToString() + " " + DiversityWorkbench.Settings.GeoLatitude.ToString() + ")";
                    }
                    else
                    {
                        Geography = "POINT(0 0)";
                    }
                }

                GO.GeometryData = Geography;
                GO.FillBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 1, 1));
                GO.FillTransparency = 50;
                GO.Identifier = "X";
                GO.PointSymbolSize = 1;
                GO.StrokeBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 1, 1));
                GO.StrokeThickness = 1;
                GO.StrokeTransparency = 255;
                string Unit = "Organism";
                System.Data.DataRow[] RRunit = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Select("IdentificationUnitID = " + R["IdentificationUnitID"].ToString(), "");
                Unit = RRunit[0]["LastIdentificationCache"].ToString();
                GO.DisplayText = "New position of " + Unit;
                System.Collections.Generic.List<GeoObject> L = new List<GeoObject>();
                L.Add(GO);
                DiversityWorkbench.Forms.FormGeography f = new DiversityWorkbench.Forms.FormGeography(L);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK && f.Geography.Length > 0)
                {
                    string SQL = "Update IdentificationUnitGeoAnalysis SET Geography = geography::STGeomFromText('" + f.Geography + "', 4326)"
                        + " WHERE CollectionSpecimenID = " + R.CollectionSpecimenID.ToString()
                        + " AND IdentificationUnitID = " + R.IdentificationUnitID.ToString();
                    if (!R["AnalysisDate"].Equals(System.DBNull.Value))
                    {
                        System.DateTime AnalysisDate;
                        if (System.DateTime.TryParse(R["AnalysisDate"].ToString(), out AnalysisDate))
                        {
                            SQL += " AND YEAR(AnalysisDate) = " + AnalysisDate.Year.ToString()
                                + " AND MONTH(AnalysisDate) = " + AnalysisDate.Month.ToString()
                                + " AND DAY(AnalysisDate) = " + AnalysisDate.Day.ToString()
                                + " AND DATEPART(hh, AnalysisDate) = " + AnalysisDate.Hour.ToString()
                                + " AND DATEPART(n, AnalysisDate) = " + AnalysisDate.Minute.ToString()
                                + " AND DATEPART(s, AnalysisDate) = " + AnalysisDate.Second.ToString();
                        }
                    }
                    this._iMainForm.saveSpecimen();// this.FormFunctions.updateTable(this._iMainForm.DataSetCollectionSpecimen(), "IdentificationUnitGeoAnalysis", this.sqlDataAdapterGeoAnalysis, this.BindingContext);
                    string Message = "";
                    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message))
                    {
                        R["GeographyAsString"] = f.Geography;
                        R.BeginEdit();
                        R.EndEdit();
                        this._iMainForm.setSpecimen();//.setSpecimen(this.ID);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Setting of the geography failed");
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("setGeographyViaGisEditor(DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitGeoAnalysisGeoRow R)", SQL, Message);
                    }
                }
                else if (R.GeographyAsString.Equals(System.DBNull.Value))
                {
                    System.Windows.Forms.MessageBox.Show("No geography was given");
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public override void SetPosition(int Position)
        {
            this._Source.CurrencyManager.Position = Position;
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            foreach (System.Data.DataRow RGeo in this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitGeoAnalysisGeo.Rows)
            {
                if (RGeo["AnalysisDate"].ToString() == R["AnalysisDate"].ToString())// &&!RGeo[3].Equals(System.DBNull.Value))
                {
                    //if (this.userControlGIS.Visible)
                    //{
                    //    this.userControlGIS.DataRowIdentificationUnitGeoAnalysisGeo = RGeo;
                    //    if (this.userControlGIS.SelectedGISObject != DiversityCollection.UserControls.UserControlGIS.SelectedGisObject.Distribution)
                    //        this.userControlGIS.SelectedGISObject = DiversityCollection.UserControls.UserControlGIS.SelectedGisObject.Organism;
                    //}
                    if (!RGeo["GeographyAsString"].Equals(System.DBNull.Value)
                        && RGeo["GeographyAsString"].ToString().Length > 0)
                    {
                        string[] Geography = RGeo["GeographyAsString"].ToString().Split(new char[] { ' ' });
                        //this.labelGeoAnalysisCoordinates.Text = "Long.: " + Geography[1].Replace("(", "") + ", Lat.: " + Geography[2].Replace(")", "");
                        this.labelLatitude.Text = Geography[2].Replace(")", "");
                        this.labelLongitude.Text = Geography[1].Replace("(", "");
                        this.textBoxGeoAnalysisGeography.Text = RGeo["GeographyAsString"].ToString();
                        //this.labelGeoAnalysisGeography.Text = RGeo["GeographyAsString"].ToString();
                    }
                    else
                    {
                        //this.labelGeoAnalysisCoordinates.Text = "";
                        this.labelLatitude.Text = "";
                        this.labelLongitude.Text = "";
                        this.textBoxGeoAnalysisGeography.Text = "";
                        //this.labelGeoAnalysisGeography.Text = "";
                    }
                    break;
                }
            }

            //System.Collections.Generic.List<string> Settings = new List<string>();
            //Settings.Add("ModuleSource");
            //Settings.Add("IdentificationUnitGeoAnalysis");
            //Settings.Add("ResponsibleAgentURI");
            //this.setUserControlModuleRelatedEntrySources(Settings, ref this.userControlModuleRelatedEntryGeoAnalysisResponsible);

            this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryGeoAnalysisResponsible, "ResponsibleAgentURI");

            this.setAvailability();
        }

        private void buttonEditGeography_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                string WhereClause = "CollectionSpecimenID = " + R["CollectionSpecimenID"].ToString() +
                        " AND IdentificationUnitID = " + R["IdentificationUnitID"].ToString();
                if (!R["AnalysisDate"].Equals(System.DBNull.Value))
                {
                    System.DateTime AnalysisDate;
                    if (System.DateTime.TryParse(R["AnalysisDate"].ToString(), out AnalysisDate))
                    {
                        WhereClause += " AND YEAR(AnalysisDate) = " + AnalysisDate.Year.ToString()
                            + " AND MONTH(AnalysisDate) = " + AnalysisDate.Month.ToString()
                            + " AND DAY(AnalysisDate) = " + AnalysisDate.Day.ToString()
                            + " AND DATEPART(hh, AnalysisDate) = " + AnalysisDate.Hour.ToString()
                            + " AND DATEPART(n, AnalysisDate) = " + AnalysisDate.Minute.ToString()
                            + " AND DATEPART(s, AnalysisDate) = " + AnalysisDate.Second.ToString();
                    }
                }
                string SQL = "SELECT Geography.ToString() FROM IdentificationUnitGeoAnalysis " +
                        " WHERE " + WhereClause;
                string Geography = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                DiversityWorkbench.Geography.FormEditGeography formEditGeography = new DiversityWorkbench.Geography.FormEditGeography("Edit the geography of a single organism", "IdentificationUnitGeoAnalysis", "Geography", Geography);
                formEditGeography.ShowDialog();
                if (formEditGeography.DialogResult == DialogResult.OK)
                {
                    if (formEditGeography.SaveGeography("IdentificationUnitGeoAnalysis", "Geography", WhereClause))
                    {
                        this._iMainForm.saveSpecimen();
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #endregion

        #endregion
    }
}
