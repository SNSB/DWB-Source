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
    public partial class FormGeoConversion : Form
    {
        #region Parameter

        private DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionEventLocalisationRow _R;
        private DiversityCollection.Datasets.DataSetCollectionSpecimen _DS;
        private int _LocalisationSystemID_Ori;
        private System.Data.DataTable _dtCoordinates;
        private enum CoordinateSystems { WGS84, PotsdamDatum, GaussKrüger };
        private System.Collections.Generic.Dictionary<CoordinateSystems, int> _CoordinateLocalisationSystemIDs;
        private CoordinateSystems _OriginalCoordinateSytem;
        private DiversityWorkbench.GeoCoordinate _GeoCoordinate = new DiversityWorkbench.GeoCoordinate();
        //private DiversityWorkbench.GeoCoordinate _Longitude = new DiversityWorkbench.GeoCoordinate();
        
        #endregion


        #region Construction and Form

        public FormGeoConversion(
            DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionEventLocalisationRow R,
            ref DiversityCollection.Datasets.DataSetCollectionSpecimen DS)
        {
            InitializeComponent();
            this._DS = DS;
            this._R = R;
            this.initForm();
            this.userControlDialogPanel.buttonOK.Enabled = false;
            this.buttonConvert.Enabled = false;
        }

        private void initForm()
        {
            this._LocalisationSystemID_Ori = int.Parse(this._R["LocalisationSystemID"].ToString());
            //string CoordinateSystem = LookupTable.LocalisationSystemName(this._LocalisationSystemID_Ori);
            //this.labelCoordinatesOri.Text = CoordinateSystem;
            this.textBoxLatitudeOri.Text = this._R.Location2.ToString();
            this.textBoxLongitudeOri.Text = this._R.Location1.ToString();
            switch (this._LocalisationSystemID_Ori)
            {
                /*
                LocalisationSystemID	LocalisationSystemName
                2	Gauss-Krüger coordinates
                6	Greenwich Coordinates
                8	Coordinates WGS84
                9	Coordinates
                12	Coordinates PD
                16	Dutch RD coordinates
                */
                case 2:
                    this.comboBoxCoordinatesResult.Items.Add(CoordinateSystems.WGS84);
                    this.comboBoxCoordinatesResult.Items.Add(CoordinateSystems.PotsdamDatum);
                    this._OriginalCoordinateSytem = CoordinateSystems.GaussKrüger;
                    break;
                case 8:
                    this.comboBoxCoordinatesResult.Items.Add(CoordinateSystems.GaussKrüger);
                    this.comboBoxCoordinatesResult.Items.Add(CoordinateSystems.PotsdamDatum);
                    this._OriginalCoordinateSytem = CoordinateSystems.WGS84;
                    break;
                case 12:
                    this.comboBoxCoordinatesResult.Items.Add(CoordinateSystems.WGS84);
                    this.comboBoxCoordinatesResult.Items.Add(CoordinateSystems.GaussKrüger);
                    this._OriginalCoordinateSytem = CoordinateSystems.PotsdamDatum;
                    break;
            }
            this.labelCoordinatesOri.Text = this._OriginalCoordinateSytem.ToString();
        }
        
        private void FormGeoConversion_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK
                && this.textBoxLatitudeResult.Text.Length > 0
                && this.textBoxLongitudeResult.Text.Length > 0)
            {
                CoordinateSystems SelectedCoordinateSytem = (CoordinateSystems)this.comboBoxCoordinatesResult.SelectedItem;
                System.Data.DataRow[] rr = this._DS.CollectionEventLocalisation.Select("LocalisationSystemID = " + this.CoordinateLocalisationSystemIDs[SelectedCoordinateSytem].ToString());
                if (rr.Length > 0)
                {
                    rr[0]["Location1"] = this.textBoxLongitudeResult.Text;
                    rr[0]["Location2"] = this.textBoxLatitudeResult.Text;
                    rr[0]["LocationNotes"] = "[Origin.: " + this._OriginalCoordinateSytem.ToString() + "]";
                    if (SelectedCoordinateSytem == CoordinateSystems.WGS84)
                    {
                        rr[0]["AverageLongitudeCache"] = this.textBoxLongitudeResult.Text;
                        rr[0]["AverageLatitudeCache"] = this.textBoxLatitudeResult.Text;
                    }
                    else
                    {
                        rr[0]["AverageLongitudeCache"] = 0;
                        rr[0]["AverageLatitudeCache"] = 0;
                    }
                }
                else
                {
                    DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionEventLocalisationRow R = this._DS.CollectionEventLocalisation.NewCollectionEventLocalisationRow();
                    R.Location1 = this.textBoxLongitudeResult.Text;
                    R.Location2 = this.textBoxLatitudeResult.Text;
                    R.LocationNotes = "[Origin.: " + this._OriginalCoordinateSytem.ToString() + "]";

                    System.Data.DataRow[] rrOri = this._DS.CollectionEventLocalisation.Select("LocalisationSystemID = " + this._LocalisationSystemID_Ori.ToString());
                    if (rrOri.Length > 0)
                    {
                        R.LocationAccuracy = rrOri[0]["LocationAccuracy"].ToString();
                        R.DirectionToLocation = rrOri[0]["DirectionToLocation"].ToString();
                        R.DistanceToLocation = rrOri[0]["DistanceToLocation"].ToString();
                    }
                    if (SelectedCoordinateSytem == CoordinateSystems.WGS84)
                    {
                        R.AverageLatitudeCache = double.Parse(this.textBoxLatitudeResult.Text);
                        R.AverageLongitudeCache = double.Parse(this.textBoxLongitudeResult.Text);
                        if (R.LocationAccuracy.Length == 0)
                            R.LocationAccuracy = DiversityWorkbench.GeoFunctions.AccuracyFromDegNumeric(R.AverageLatitudeCache, R.AverageLongitudeCache);
                    }
                    else
                    {
                        R.AverageLatitudeCache = 0;
                        R.AverageLongitudeCache = 0;
                    }
                    R.LocalisationSystemID = this.CoordinateLocalisationSystemIDs[SelectedCoordinateSytem];
                    int CollectionEventID;
                    if (int.TryParse(this._DS.CollectionEvent.Rows[0]["CollectionEventID"].ToString(), out CollectionEventID))
                    {
                        R.CollectionEventID = CollectionEventID;
                        this._DS.CollectionEventLocalisation.Rows.Add(R);
                    }
                }
            }
            else
                this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        private void comboBoxCoordinatesResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            CoordinateSystems SelectedCoordinateSytem = (CoordinateSystems)this.comboBoxCoordinatesResult.SelectedItem;
            this.buttonConvert.Enabled = true;
            this.userControlDialogPanel.buttonOK.Enabled = false;
            double x;
            double y;
            System.Data.DataRow[] rr = this._DS.CollectionEventLocalisation.Select("LocalisationSystemID = " + this.CoordinateLocalisationSystemIDs[SelectedCoordinateSytem].ToString());
            if (rr.Length > 0)
            {
                System.Globalization.CultureInfo InvC = new System.Globalization.CultureInfo("");
                if (double.TryParse(rr[0]["Location1"].ToString(), System.Globalization.NumberStyles.Number, InvC, out x) &&
                    double.TryParse(rr[0]["Location2"].ToString(), System.Globalization.NumberStyles.Number, InvC, out y))
                {
                    if (x < 180 && x > -180 && y < 90 && y > -90)
                    {
                        this.textBoxLatitudeResult.Text = y.ToString();
                        this.textBoxLongitudeResult.Text = x.ToString();
                    }
                    else if (double.TryParse(rr[0]["Location1"].ToString().Replace(',', '.'), System.Globalization.NumberStyles.Number, InvC, out x) &&
                    double.TryParse(rr[0]["Location2"].ToString().Replace(',', '.'), System.Globalization.NumberStyles.Number, InvC, out y))
                    {
                        if (x < 180 && x > -180 && y < 90 && y > -90)
                        {
                            this.textBoxLatitudeResult.Text = y.ToString();
                            this.textBoxLongitudeResult.Text = x.ToString();
                        }
                    }
                }
            }
            else
            {
                this.textBoxLatitudeResult.Text = "";
                this.textBoxLongitudeResult.Text = "";
            }
        }

        private System.Collections.Generic.Dictionary<CoordinateSystems, int> CoordinateLocalisationSystemIDs
        {
            get
            {
                if (this._CoordinateLocalisationSystemIDs == null)
                {
                    this._CoordinateLocalisationSystemIDs = new Dictionary<CoordinateSystems, int>();
                    this._CoordinateLocalisationSystemIDs.Add(CoordinateSystems.WGS84, 8);
                    this._CoordinateLocalisationSystemIDs.Add(CoordinateSystems.GaussKrüger, 2);
                    this._CoordinateLocalisationSystemIDs.Add(CoordinateSystems.PotsdamDatum, 12);
                }
                return this._CoordinateLocalisationSystemIDs;
            }
        }

        private void buttonConvert_Click(object sender, EventArgs e)
        {
            if (this.comboBoxCoordinatesResult.SelectedIndex == -1)
            {
                System.Windows.Forms.MessageBox.Show("Please select a target coordinate system");
                return;
            }
            System.Globalization.CultureInfo InvC = new System.Globalization.CultureInfo("");
            CoordinateSystems SelectedCoordinateSytem = (CoordinateSystems)this.comboBoxCoordinatesResult.SelectedItem;
            GeoConversion.GeoCon GeoConverter = new GeoConversion.GeoCon();
            bool ConversionSuccessfull = true;
            //this._Longitude.Direction = DiversityWorkbench.GeoFunctions.GeoDirection.Longitude;
            DiversityWorkbench.GeoFunctions.ConvertDegreeToNumeric(this._R.Location2, this._R.Location1, ref this._GeoCoordinate, ref ConversionSuccessfull);
            //double xOri = DiversityWorkbench.GeoFunctions.ConvertDegreeToNumeric(this._R.Location1, ref this._Longitude, ref ConversionSuccessfull);
            if (!ConversionSuccessfull) return;
            //this._GeoCoordinate.Direction = DiversityWorkbench.GeoFunctions.GeoDirection.Latitude;
            //double yOri = DiversityWorkbench.GeoFunctions.ConvertDegreeToNumeric(this._R.Location2, ref this._GeoCoordinate, ref ConversionSuccessfull);
            double xOri = this._GeoCoordinate.NumericEW;
            double yOri = this._GeoCoordinate.NumericNS;
            if (!ConversionSuccessfull) return;
            double xResult = 0;
            double yResult = 0;

            switch (this._OriginalCoordinateSytem)
            {
                case CoordinateSystems.GaussKrüger:
                    switch (SelectedCoordinateSytem)
                    {
                        case CoordinateSystems.PotsdamDatum:
                            // convert GK to PD
                            GeoConverter.CoordPotsdamGkToGeo(xOri, yOri, ref xResult, ref yResult);
                            break;
                        case CoordinateSystems.WGS84:
                            // convert GK to PD
                            GeoConverter.CoordPotsdamGkToGeo(xOri, yOri, ref xResult, ref yResult);
                            // convert PD to WGS84
                            xOri = xResult;
                            yOri = yResult;
                            GeoConverter.DatumPotsdamToWgs84(xOri, yOri, ref xResult, ref yResult);
                            break;
                    }
                    break;
                case CoordinateSystems.PotsdamDatum:
                    switch (SelectedCoordinateSytem)
                    {
                        case CoordinateSystems.GaussKrüger:
                            // convert PD to GK
                            GeoConverter.CoordPotsdamGeoToGk(xOri, yOri, ref xResult, ref yResult);
                            break;
                        case CoordinateSystems.WGS84:
                            // convert PD to GWS84
                            GeoConverter.DatumPotsdamToWgs84(xOri, yOri, ref xResult, ref yResult);
                            break;
                    }
                    break;
                case CoordinateSystems.WGS84:
                    switch (SelectedCoordinateSytem)
                    {
                        case CoordinateSystems.GaussKrüger:
                            // convert WGS84 to PD
                            GeoConverter.DatumWgs84ToPotsdam(xOri, yOri, ref xResult, ref yResult);
                            // convert PD to GK
                            xOri = xResult;
                            yOri = yResult;
                            GeoConverter.CoordPotsdamGeoToGk(xOri, yOri, ref xResult, ref yResult);
                            break;
                        case CoordinateSystems.PotsdamDatum:
                            // convert WGS84 to PD
                            GeoConverter.DatumWgs84ToPotsdam(xOri, yOri, ref xResult, ref yResult);
                            break;
                    }
                    break;
            }
            this.textBoxLongitudeResult.Text = xResult.ToString();
            this.textBoxLatitudeResult.Text = yResult.ToString();
            this.userControlDialogPanel.buttonOK.Enabled = true;
        }

    }
}
