using DiversityWorkbench.DwbManual;
using DiversityWorkbench.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversitySamplingPlots.Forms
{
    public partial class FormGeoConversion : Form
    {
        #region Parameter

        private DiversitySamplingPlots.Datasets.DataSetSamplingPlot.SamplingPlotLocalisationRow _R;
        private DiversitySamplingPlots.Datasets.DataSetSamplingPlot _DS;
        private int _LocalisationSystemID_Ori;
        private System.Data.DataTable _dtCoordinates;
        private enum CoordinateSystems { WGS84, PotsdamDatum, GaussKrüger };
        private System.Collections.Generic.Dictionary<CoordinateSystems, int> _CoordinateLocalisationSystemIDs;
        private CoordinateSystems _OriginalCoordinateSytem;
        private DiversityWorkbench.GeoCoordinate _GeoCoordinate = new DiversityWorkbench.GeoCoordinate();
        
        #endregion

        #region Construction and Form

        public FormGeoConversion(
            DiversitySamplingPlots.Datasets.DataSetSamplingPlot.SamplingPlotLocalisationRow R,
            ref DiversitySamplingPlots.Datasets.DataSetSamplingPlot DS)
        {
            InitializeComponent();
            this._DS = DS;
            this._R = R;
            this.initForm();
            this.userControlDialogPanel.buttonOK.Enabled = false;
            this.buttonConvert.Enabled = false;

            // Manual
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form_KeyDown);
        }

        private void initForm()
        {
            this._LocalisationSystemID_Ori = int.Parse(this._R["LocalisationSystemID"].ToString());
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
                System.Data.DataRow[] rr = this._DS.SamplingPlotLocalisation.Select("LocalisationSystemID = " + this.CoordinateLocalisationSystemIDs[SelectedCoordinateSytem].ToString());
                if (rr.Length > 0)
                {
                    rr[0]["Location1"] = this.textBoxLongitudeResult.Text;
                    rr[0]["Location2"] = this.textBoxLatitudeResult.Text;
                    rr[0]["LocationNotes"] = "[Origin.: " + this._OriginalCoordinateSytem.ToString() + "]";
                }
                else
                {
                    DiversitySamplingPlots.Datasets.DataSetSamplingPlot.SamplingPlotLocalisationRow R = this._DS.SamplingPlotLocalisation.NewSamplingPlotLocalisationRow();
                    R.Location1 = this.textBoxLongitudeResult.Text;
                    R.Location2 = this.textBoxLatitudeResult.Text;
                    R.LocationNotes = "[Origin.: " + this._OriginalCoordinateSytem.ToString() + "]";

                    System.Data.DataRow[] rrOri = this._DS.SamplingPlotLocalisation.Select("LocalisationSystemID = " + this._LocalisationSystemID_Ori.ToString());
                    if (rrOri.Length > 0)
                    {
                        R.LocationAccuracy = rrOri[0]["LocationAccuracy"].ToString();
                        R.DirectionToLocation = rrOri[0]["DirectionToLocation"].ToString();
                        R.DistanceToLocation = rrOri[0]["DistanceToLocation"].ToString();
                    }
                    R.LocalisationSystemID = this.CoordinateLocalisationSystemIDs[SelectedCoordinateSytem];
                    int PlotID;
                    if (int.TryParse(this._DS.SamplingPlot.Rows[0]["PlotID"].ToString(), out PlotID))
                    {
                        R.PlotID = PlotID;
                        this._DS.SamplingPlotLocalisation.Rows.Add(R);
                    }
                }
            }
            else
                this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #region Event Handlers
        private void comboBoxCoordinatesResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            CoordinateSystems SelectedCoordinateSytem = (CoordinateSystems)this.comboBoxCoordinatesResult.SelectedItem;
            this.buttonConvert.Enabled = true;
            this.userControlDialogPanel.buttonOK.Enabled = false;
            double x;
            double y;
            System.Data.DataRow[] rr = this._DS.SamplingPlotLocalisation.Select("LocalisationSystemID = " + this.CoordinateLocalisationSystemIDs[SelectedCoordinateSytem].ToString());
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
            DiversityWorkbench.GeoFunctions.ConvertDegreeToNumeric(this._R.Location2, this._R.Location1, ref this._GeoCoordinate, ref ConversionSuccessfull);
            if (!ConversionSuccessfull) return;
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

        #endregion

        #region Manual

        private System.Windows.Forms.HelpProvider helpProvider = new HelpProvider();

        /// <summary>
        /// Adding event deletates to form and controls
        /// </summary>
        /// <returns></returns>
        private async Task InitManual()
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
        /// Set help keywords for controls where it does not work automatically
        /// </summary>
        private void InitManualControls()
        {
            try
            {
                helpProvider.SetHelpKeyword(this, "localisation_dsp");
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
                    this.InitManualControls();
                    await this.InitManual();
                    _InitManualDone = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion



    }
}
